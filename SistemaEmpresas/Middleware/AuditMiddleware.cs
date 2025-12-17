using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;
using SistemaEmpresas.Services;
using SistemaEmpresas.Services.Logs;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SistemaEmpresas.Middleware;

/// <summary>
/// Middleware para captura automática de logs de auditoria em requisições HTTP.
/// </summary>
public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // Rotas que devem ser ignoradas no log
    private static readonly HashSet<string> RotasIgnoradas = new(StringComparer.OrdinalIgnoreCase)
    {
        "/api/logs",           // Evitar recursão
        "/api/logs/estatisticas",
        "/health",
        "/swagger",
        "/favicon.ico"
    };

    // Métodos que não alteram dados (opcionalmente podem ser logados)
    private static readonly HashSet<string> MetodosLeitura = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET", "HEAD", "OPTIONS"
    };

    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";
        
        // Verificar se deve ignorar esta rota
        if (DeveIgnorar(path))
        {
            await _next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];
        
        // Adicionar correlation ID ao response header
        context.Response.Headers.Append("X-Correlation-ID", correlationId);

        // Capturar username para requisições de login (antes de processar)
        string? loginUsername = null;
        if (path.Contains("/login", StringComparison.OrdinalIgnoreCase) && 
            context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            loginUsername = await ExtrairUsernameDoLogin(context);
        }

        Exception? exception = null;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();

            // Registrar log após a requisição
            await RegistrarLogAsync(context, stopwatch.ElapsedMilliseconds, correlationId, exception, loginUsername);
        }
    }

    /// <summary>
    /// Extrai o username do body da requisição de login
    /// </summary>
    private async Task<string?> ExtrairUsernameDoLogin(HttpContext context)
    {
        try
        {
            // Habilitar buffer para poder ler o body múltiplas vezes
            context.Request.EnableBuffering();
            
            using var reader = new StreamReader(
                context.Request.Body, 
                Encoding.UTF8, 
                detectEncodingFromByteOrderMarks: false, 
                leaveOpen: true);
            
            var body = await reader.ReadToEndAsync();
            
            // Resetar a posição do body para que o controller possa lê-lo
            context.Request.Body.Position = 0;
            
            if (!string.IsNullOrEmpty(body))
            {
                var jsonDoc = JsonDocument.Parse(body);
                if (jsonDoc.RootElement.TryGetProperty("usuario", out var usuarioElement))
                {
                    return usuarioElement.GetString();
                }
                // Tentar também com "Usuario" (maiúsculo)
                if (jsonDoc.RootElement.TryGetProperty("Usuario", out var usuarioMaiusculoElement))
                {
                    return usuarioMaiusculoElement.GetString();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao extrair username do body de login");
        }
        
        return null;
    }

    private bool DeveIgnorar(string path)
    {
        // Ignorar rotas específicas
        foreach (var rota in RotasIgnoradas)
        {
            if (path.StartsWith(rota, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        // Ignorar arquivos estáticos
        if (path.Contains('.') && !path.Contains("/api/"))
            return true;

        return false;
    }

    private Task RegistrarLogAsync(
        HttpContext context,
        long tempoExecucaoMs,
        string correlationId,
        Exception? exception,
        string? loginUsername = null)
    {
        try
        {
            var user = context.User;
            var path = context.Request.Path.Value ?? "";
            var method = context.Request.Method;
            var statusCode = context.Response.StatusCode;
            var isErro = statusCode >= 400 || exception != null;

            // ========================================
            // FILTRO: Logar apenas o que importa
            // ========================================
            // Grandes empresas NÃO logam cada GET/consulta.
            // Logamos apenas:
            // - Login/Logout (sempre)
            // - POST, PUT, PATCH, DELETE (alterações)
            // - Erros (qualquer método)
            // - GETs em rotas sensíveis (dados do usuário, permissões)
            
            var isLogin = path.Contains("/login", StringComparison.OrdinalIgnoreCase) || 
                          path.Contains("/logout", StringComparison.OrdinalIgnoreCase);
            var isEscrita = method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                           method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
                           method.Equals("PATCH", StringComparison.OrdinalIgnoreCase) ||
                           method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
            var isGetSensivel = method.Equals("GET", StringComparison.OrdinalIgnoreCase) && 
                               (path.Contains("/usuarios", StringComparison.OrdinalIgnoreCase) ||
                                path.Contains("/permissoes", StringComparison.OrdinalIgnoreCase) ||
                                path.Contains("/grupousuario", StringComparison.OrdinalIgnoreCase));
            
            // Se não é login, não é escrita, não é erro e não é GET sensível, ignorar
            if (!isLogin && !isEscrita && !isErro && !isGetSensivel)
            {
                return Task.CompletedTask;
            }

            // Extrair informações do usuário logado
            var usuarioCodigo = 0;
            var usuarioNome = "Anônimo";
            var usuarioGrupo = "Sistema";

            if (user.Identity?.IsAuthenticated == true)
            {
                int.TryParse(user.FindFirst("codigo")?.Value, out usuarioCodigo);
                usuarioNome = user.FindFirst(ClaimTypes.Name)?.Value 
                    ?? user.FindFirst("nome")?.Value 
                    ?? "Usuário";
                usuarioGrupo = user.FindFirst("grupo")?.Value ?? "Sistema";
            }
            else if (!string.IsNullOrEmpty(loginUsername))
            {
                // Se é uma tentativa de login e temos o username, usar ele
                usuarioNome = loginUsername;
            }

            // Determinar tipo de ação baseado no método HTTP e rota
            var (tipoAcao, modulo, entidade) = DeterminarTipoAcao(method, path);

            // Extrair ID da entidade da URL se disponível
            var entidadeId = ExtrairEntidadeId(path);

            // Criar descrição
            var descricao = CriarDescricao(method, path, statusCode, exception);

            // Obter IP do cliente
            var ip = ObterIpCliente(context);

            // Criar log
            var logDto = new LogAuditoriaCreateDto
            {
                UsuarioCodigo = usuarioCodigo,
                UsuarioNome = usuarioNome,
                UsuarioGrupo = usuarioGrupo,
                TipoAcao = tipoAcao,
                Modulo = modulo,
                Entidade = entidade,
                EntidadeId = entidadeId,
                Descricao = descricao,
                EnderecoIP = ip,
                UserAgent = context.Request.Headers.UserAgent.ToString(),
                MetodoHttp = method,
                UrlRequisicao = $"{path}{context.Request.QueryString}",
                StatusCode = statusCode,
                TempoExecucaoMs = tempoExecucaoMs,
                Erro = isErro,
                MensagemErro = exception?.Message,
                CorrelationId = correlationId
            };

            // Registrar log de forma assíncrona usando um novo escopo
            // Isso evita o erro "Cannot access a disposed context instance"
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var logService = scope.ServiceProvider.GetRequiredService<ILogAuditoriaService>();
                    await logService.LogAsync(logDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao registrar log de auditoria no middleware");
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no middleware de auditoria");
        }

        return Task.CompletedTask;
    }

    private (string tipoAcao, string modulo, string entidade) DeterminarTipoAcao(string method, string path)
    {
        var tipoAcao = method.ToUpper() switch
        {
            "POST" => TipoAcaoAuditoria.CRIAR,
            "PUT" => TipoAcaoAuditoria.ALTERAR,
            "PATCH" => TipoAcaoAuditoria.ALTERAR,
            "DELETE" => TipoAcaoAuditoria.EXCLUIR,
            "GET" => TipoAcaoAuditoria.VISUALIZAR,
            _ => TipoAcaoAuditoria.OUTRO
        };

        // Casos especiais
        if (path.Contains("/login", StringComparison.OrdinalIgnoreCase))
            tipoAcao = TipoAcaoAuditoria.LOGIN;
        else if (path.Contains("/logout", StringComparison.OrdinalIgnoreCase))
            tipoAcao = TipoAcaoAuditoria.LOGOUT;
        else if (path.Contains("/export", StringComparison.OrdinalIgnoreCase))
            tipoAcao = TipoAcaoAuditoria.EXPORTAR;
        else if (path.Contains("/import", StringComparison.OrdinalIgnoreCase))
            tipoAcao = TipoAcaoAuditoria.IMPORTAR;
        else if (path.Contains("/aprovar", StringComparison.OrdinalIgnoreCase))
            tipoAcao = TipoAcaoAuditoria.APROVAR;
        else if (path.Contains("/rejeitar", StringComparison.OrdinalIgnoreCase))
            tipoAcao = TipoAcaoAuditoria.REJEITAR;

        // Determinar módulo e entidade pela URL
        var (modulo, entidade) = ExtrairModuloEntidade(path);

        return (tipoAcao, modulo, entidade);
    }

    private (string modulo, string entidade) ExtrairModuloEntidade(string path)
    {
        var partes = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        
        if (partes.Length < 2)
            return ("Sistema", "Geral");

        // Pular "api" se existir
        var indice = partes[0].Equals("api", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        
        if (indice >= partes.Length)
            return ("Sistema", "Geral");

        var controlador = partes[indice];

        // Mapear controlador para módulo e entidade com nomes amigáveis
        return controlador.ToLower() switch
        {
            "auth" => ("Autenticação", "Sessão"),
            "usuarios" => ("Usuários", "Usuário"),
            "grupousuario" => ("Usuários", "Grupo de Usuário"),
            "produtos" => ("Cadastros", "Produto"),
            "geral" => ("Cadastros", "Cadastro Geral"),
            "emitentes" => ("Cadastros", "Emitente"),
            "notafiscal" => ("Fiscal", "Nota Fiscal"),
            "classificacaofiscal" or "classificacao-fiscal" => ("Fiscal", "Classificação Fiscal"),
            "classtrib" => ("Fiscal", "Classificação Tributária"),
            "dashboard" => ("Dashboard", "Painel"),
            "logs" => ("Auditoria", "Logs"),
            "permissoes" => ("Permissões", "Permissão"),
            "permissao" => ("Permissões", "Permissão"),
            "relatorios" => ("Relatórios", "Relatório"),
            "configuracoes" => ("Configurações", "Configuração"),
            "estoque" => ("Estoque", "Estoque"),
            "vendas" => ("Vendas", "Venda"),
            "financeiro" => ("Financeiro", "Lançamento"),
            "clientes" => ("Cadastros", "Cliente"),
            "fornecedores" => ("Cadastros", "Fornecedor"),
            _ => ("Sistema", FormatarNomeEntidade(controlador))
        };
    }

    private string FormatarNomeEntidade(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;
        
        // Converter kebab-case e camelCase para título
        var resultado = new StringBuilder();
        var primeiroChar = true;
        
        for (int i = 0; i < texto.Length; i++)
        {
            var c = texto[i];
            
            if (c == '-' || c == '_')
            {
                resultado.Append(' ');
                primeiroChar = true;
            }
            else if (char.IsUpper(c) && !primeiroChar && i > 0)
            {
                resultado.Append(' ');
                resultado.Append(c);
                primeiroChar = false;
            }
            else
            {
                resultado.Append(primeiroChar ? char.ToUpper(c) : char.ToLower(c));
                primeiroChar = false;
            }
        }
        
        return resultado.ToString();
    }

    private string? ExtrairEntidadeId(string path)
    {
        var partes = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        
        // Procurar por um ID numérico ou GUID no final da URL
        for (int i = partes.Length - 1; i >= 0; i--)
        {
            var parte = partes[i];
            
            // Verificar se é número
            if (int.TryParse(parte, out _))
                return parte;
            
            // Verificar se é GUID
            if (Guid.TryParse(parte, out _))
                return parte;
        }

        return null;
    }

    private string CriarDescricao(string method, string path, int statusCode, Exception? exception)
    {
        var (_, entidade) = ExtrairModuloEntidade(path);
        var entidadeId = ExtrairEntidadeId(path);
        var isGet = method.Equals("GET", StringComparison.OrdinalIgnoreCase);
        var isPost = method.Equals("POST", StringComparison.OrdinalIgnoreCase);
        var isPut = method.Equals("PUT", StringComparison.OrdinalIgnoreCase);
        var isDelete = method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
        var isErro = statusCode >= 400;
        var isErroInterno = statusCode >= 500;
        
        // ========================================
        // AUTENTICAÇÃO E SESSÃO
        // ========================================
        
        if (path.Contains("/auth/login", StringComparison.OrdinalIgnoreCase) && isPost)
            return isErro ? "Tentativa de login (falhou)" : "Realizou login no sistema";
        
        if (path.Contains("/logout", StringComparison.OrdinalIgnoreCase))
            return "Realizou logout do sistema";
        
        if (path.Contains("/refresh", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de renovar sessão (falhou)" : "Renovou sessão";
        
        if (path.Contains("/alterar-senha", StringComparison.OrdinalIgnoreCase) || path.Contains("/alterarsenha", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de alterar senha (falhou)" : "Alterou senha";
        
        if (path.Contains("/reset-senha", StringComparison.OrdinalIgnoreCase) || path.Contains("/resetsenha", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de resetar senha (falhou)" : "Resetou senha de usuário";
        
        if (path.Contains("/auth/me", StringComparison.OrdinalIgnoreCase) || path.Contains("/auth/usuario-atual", StringComparison.OrdinalIgnoreCase))
            return "Consultou dados do usuário logado";
        
        // ========================================
        // DASHBOARD E ESTATÍSTICAS
        // ========================================
        
        if (path.Contains("/dashboard", StringComparison.OrdinalIgnoreCase) || path.Contains("/kpis", StringComparison.OrdinalIgnoreCase))
        {
            if (isErroInterno) return "❌ Erro ao carregar Dashboard";
            if (isErro) return "⚠️ Erro ao acessar Dashboard";
            return "Acessou Dashboard";
        }
        
        if (path.Contains("/estatisticas", StringComparison.OrdinalIgnoreCase))
            return isErro ? "⚠️ Erro ao consultar estatísticas" : "Consultou estatísticas";
        
        // ========================================
        // USUÁRIOS E GRUPOS
        // ========================================
        
        if (path.Contains("/usuarios/grupos", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet) return "Consultou grupos de usuários";
            if (isPost) return isErro ? "Tentativa de criar grupo (falhou)" : "Criou novo grupo de usuários";
            if (isDelete) return isErro ? "Tentativa de excluir grupo (falhou)" : "Excluiu grupo de usuários";
        }
        
        if (path.Contains("/usuarios/arvore", StringComparison.OrdinalIgnoreCase))
            return "Consultou árvore de usuários";
        
        if (path.Contains("/usuarios/mover", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de mover usuário (falhou)" : "Moveu usuário para outro grupo";
        
        if (path.Contains("/usuarios/permissoes/copiar", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de copiar permissões (falhou)" : "Copiou permissões entre grupos";
        
        if (path.Contains("/usuarios/permissoes", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet) return "Consultou permissões do grupo";
            if (isPut) return isErro ? "Tentativa de alterar permissões (falhou)" : "Alterou permissões do grupo";
        }
        
        if (path.Contains("/usuarios/tabelas-disponiveis", StringComparison.OrdinalIgnoreCase))
            return "Consultou tabelas disponíveis";
        
        if (path.Contains("/usuarios/menus-disponiveis", StringComparison.OrdinalIgnoreCase))
            return "Consultou menus disponíveis";
        
        if (path.Contains("/usuarios", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou usuário {entidadeId}";
            if (isGet) return "Consultou lista de usuários";
            if (isPost) return isErro ? "Tentativa de criar usuário (falhou)" : "Criou novo usuário";
            if (isPut) return isErro ? "Tentativa de alterar usuário (falhou)" : $"Alterou usuário {entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir usuário (falhou)" : $"Excluiu usuário {entidadeId}";
        }
        
        // ========================================
        // PERMISSÕES
        // ========================================
        
        if (path.Contains("/permissoes", StringComparison.OrdinalIgnoreCase) || path.Contains("/permissao", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet) return "Consultou permissões";
            if (isPost) return isErro ? "Tentativa de criar permissão (falhou)" : "Criou nova permissão";
            if (isPut) return isErro ? "Tentativa de alterar permissão (falhou)" : "Alterou permissão";
            if (isDelete) return isErro ? "Tentativa de excluir permissão (falhou)" : "Excluiu permissão";
        }
        
        // ========================================
        // GRUPO DE USUÁRIO
        // ========================================
        
        if (path.Contains("/grupousuario", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou grupo #{entidadeId}";
            if (isGet) return "Consultou grupos de usuários";
            if (isPost) return isErro ? "Tentativa de criar grupo (falhou)" : "Criou novo grupo de usuários";
            if (isPut) return isErro ? "Tentativa de alterar grupo (falhou)" : $"Alterou grupo #{entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir grupo (falhou)" : $"Excluiu grupo #{entidadeId}";
        }
        
        // ========================================
        // CADASTROS - GERAL
        // ========================================
        
        if (path.Contains("/geral", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou cadastro #{entidadeId}";
            if (isGet) return "Consultou cadastros gerais";
            if (isPost) return isErro ? "Tentativa de criar cadastro (falhou)" : "Criou novo cadastro";
            if (isPut) return isErro ? "Tentativa de alterar cadastro (falhou)" : $"Alterou cadastro #{entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir cadastro (falhou)" : $"Excluiu cadastro #{entidadeId}";
        }
        
        // ========================================
        // EMITENTES / EMITENTE
        // ========================================
        
        if (path.Contains("/emitente", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou dados do emitente #{entidadeId}";
            if (isGet) return "Consultou dados do emitente";
            if (isPost) return isErro ? "Tentativa de configurar emitente (falhou)" : "Configurou dados do emitente";
            if (isPut) return isErro ? "Tentativa de alterar emitente (falhou)" : "Alterou dados do emitente";
            if (isDelete) return isErro ? "Tentativa de excluir emitente (falhou)" : "Excluiu dados do emitente";
        }
        
        // ========================================
        // PRODUTOS
        // ========================================
        
        if (path.Contains("/produtos", StringComparison.OrdinalIgnoreCase) || path.Contains("/produto", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou produto #{entidadeId}";
            if (isGet) return "Consultou produtos";
            if (isPost) return isErro ? "Tentativa de criar produto (falhou)" : "Criou novo produto";
            if (isPut) return isErro ? "Tentativa de alterar produto (falhou)" : $"Alterou produto #{entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir produto (falhou)" : $"Excluiu produto #{entidadeId}";
        }
        
        // ========================================
        // NOTA FISCAL
        // ========================================
        
        if (path.Contains("/notafiscal/transmitir", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de transmitir NF-e (falhou)" : "Transmitiu NF-e para SEFAZ";
        
        if (path.Contains("/notafiscal/cancelar", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de cancelar NF-e (falhou)" : "Cancelou NF-e";
        
        if (path.Contains("/notafiscal/inutilizar", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de inutilizar numeração (falhou)" : "Inutilizou numeração de NF-e";
        
        if (path.Contains("/notafiscal/cartacorrecao", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de enviar CC-e (falhou)" : "Enviou Carta de Correção";
        
        if (path.Contains("/notafiscal/pdf", StringComparison.OrdinalIgnoreCase) || path.Contains("/notafiscal/danfe", StringComparison.OrdinalIgnoreCase))
            return "Gerou DANFE (PDF)";
        
        if (path.Contains("/notafiscal/xml", StringComparison.OrdinalIgnoreCase))
            return "Baixou XML da NF-e";
        
        if (path.Contains("/notafiscal/consultar", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de consultar NF-e na SEFAZ (falhou)" : "Consultou NF-e na SEFAZ";
        
        if (path.Contains("/notafiscal", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou NF-e #{entidadeId}";
            if (isGet) return "Consultou notas fiscais";
            if (isPost) return isErro ? "Tentativa de criar NF-e (falhou)" : "Criou nova NF-e";
            if (isPut) return isErro ? "Tentativa de alterar NF-e (falhou)" : $"Alterou NF-e #{entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir NF-e (falhou)" : $"Excluiu NF-e #{entidadeId}";
        }
        
        // ========================================
        // CLASSIFICAÇÃO FISCAL
        // ========================================
        
        if (path.Contains("/classificacaofiscal", StringComparison.OrdinalIgnoreCase) || path.Contains("/classificacao-fiscal", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou classificação fiscal #{entidadeId}";
            if (isGet) return "Consultou classificações fiscais";
            if (isPost) return isErro ? "Tentativa de criar classificação fiscal (falhou)" : "Criou nova classificação fiscal";
            if (isPut) return isErro ? "Tentativa de alterar classificação fiscal (falhou)" : $"Alterou classificação fiscal #{entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir classificação fiscal (falhou)" : $"Excluiu classificação fiscal #{entidadeId}";
        }
        
        // ========================================
        // CLASSTRIB (IBS/CBS)
        // ========================================
        
        if (path.Contains("/classtrib", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou ClassTrib #{entidadeId}";
            if (isGet) return "Consultou ClassTrib (IBS/CBS)";
            if (isPost) return isErro ? "Tentativa de criar ClassTrib (falhou)" : "Criou nova ClassTrib";
            if (isPut) return isErro ? "Tentativa de alterar ClassTrib (falhou)" : $"Alterou ClassTrib #{entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir ClassTrib (falhou)" : $"Excluiu ClassTrib #{entidadeId}";
        }
        
        // ========================================
        // TENANTS (EMPRESAS)
        // ========================================
        
        if (path.Contains("/tenants/atual", StringComparison.OrdinalIgnoreCase))
            return "Consultou empresa atual";
        
        if (path.Contains("/tenants", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou empresa #{entidadeId}";
            if (isGet) return "Consultou empresas";
            if (isPost) return isErro ? "Tentativa de criar empresa (falhou)" : "Criou nova empresa";
            if (isPut) return isErro ? "Tentativa de alterar empresa (falhou)" : $"Alterou empresa #{entidadeId}";
            if (isDelete) return isErro ? "Tentativa de excluir empresa (falhou)" : $"Excluiu empresa #{entidadeId}";
        }
        
        // ========================================
        // LOGS DE AUDITORIA
        // ========================================
        
        if (path.Contains("/logs/estatisticas", StringComparison.OrdinalIgnoreCase))
            return "Consultou estatísticas de logs";
        
        if (path.Contains("/logs", StringComparison.OrdinalIgnoreCase))
        {
            if (isGet && entidadeId != null) return $"Visualizou log #{entidadeId}";
            if (isGet) return "Consultou logs de auditoria";
        }
        
        // ========================================
        // EXPORTAÇÃO E IMPORTAÇÃO
        // ========================================
        
        if (path.Contains("/export", StringComparison.OrdinalIgnoreCase))
            return isErro ? $"Tentativa de exportar {entidade} (falhou)" : $"Exportou dados de {entidade}";
        
        if (path.Contains("/import", StringComparison.OrdinalIgnoreCase))
            return isErro ? $"Tentativa de importar {entidade} (falhou)" : $"Importou dados para {entidade}";
        
        // ========================================
        // RELATÓRIOS
        // ========================================
        
        if (path.Contains("/relatorios", StringComparison.OrdinalIgnoreCase) || path.Contains("/relatorio", StringComparison.OrdinalIgnoreCase))
            return isErro ? "Tentativa de gerar relatório (falhou)" : $"Gerou relatório de {entidade}";
        
        // ========================================
        // DESCRIÇÕES PADRÃO (FALLBACK)
        // ========================================
        
        var descricao = method.ToUpper() switch
        {
            "GET" when entidadeId != null => $"Visualizou {entidade} #{entidadeId}",
            "GET" => $"Consultou {entidade}",
            "POST" => $"Criou registro em {entidade}",
            "PUT" when entidadeId != null => $"Alterou {entidade} #{entidadeId}",
            "PUT" => $"Alterou registro em {entidade}",
            "PATCH" when entidadeId != null => $"Atualizou {entidade} #{entidadeId}",
            "PATCH" => $"Atualizou registro em {entidade}",
            "DELETE" when entidadeId != null => $"Excluiu {entidade} #{entidadeId}",
            "DELETE" => $"Excluiu registro em {entidade}",
            _ => $"Acessou {entidade}"
        };
        
        // Adicionar informação de erro se houver
        if (exception != null)
            return $"❌ {descricao} - Erro: {exception.Message}";
        
        if (isErroInterno)
            return $"❌ Erro interno: {descricao}";
        
        if (isErro)
            return $"⚠️ {descricao} (falhou)";
        
        return descricao;
    }

    private string ExtrairNomeRecursoLegivel(string path)
    {
        var (_, entidade) = ExtrairModuloEntidade(path);
        return entidade;
    }

    private string ObterIpCliente(HttpContext context)
    {
        // Tentar obter IP real através de headers de proxy
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    private string CapitalizarPrimeiraLetra(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;
        
        return char.ToUpper(texto[0]) + texto[1..].ToLower();
    }
}

/// <summary>
/// Extensões para adicionar o middleware de auditoria ao pipeline.
/// </summary>
public static class AuditMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditMiddleware>();
    }
}
