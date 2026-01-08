using SistemaEmpresas.Models;
using SistemaEmpresas.Features.Logs.Repositories;
using System.Text.Json;

namespace SistemaEmpresas.Features.Seguranca.Services;

/// <summary>
/// Implementação do serviço de logs de segurança
/// </summary>
public class LogSegurancaService : ILogSegurancaService
{
    private readonly ILogAuditoriaRepository _repository;
    private readonly ILogger<LogSegurancaService> _logger;

    public LogSegurancaService(
        ILogAuditoriaRepository repository,
        ILogger<LogSegurancaService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task LogTentativaLoginAsync(
        string usuario,
        string tenant,
        bool sucesso,
        string? motivoFalha = null,
        string? ip = null)
    {
        try
        {
            var dados = new
            {
                Usuario = usuario,
                Tenant = tenant,
                Sucesso = sucesso,
                MotivoFalha = motivoFalha,
                DataHora = DateTime.Now
            };

            var log = new LogAuditoria
            {
                DataHora = DateTime.Now,
                Modulo = "Seguranca",
                TipoAcao = sucesso ? "LOGIN_SUCESSO" : "LOGIN_FALHA",
                Entidade = "Autenticacao",
                EntidadeId = usuario,
                Descricao = sucesso 
                    ? $"Login realizado com sucesso para usuário {usuario}" 
                    : $"Tentativa de login falhou para usuário {usuario}: {motivoFalha}",
                DadosNovos = JsonSerializer.Serialize(dados),
                UsuarioCodigo = 0,
                UsuarioNome = usuario,
                UsuarioGrupo = "",
                EnderecoIP = ip
            };

            await _repository.CreateAsync(log);

            if (!sucesso)
            {
                _logger.LogWarning(
                    "⚠️ SEGURANÇA: Tentativa de login falha. Usuário: {Usuario}, Tenant: {Tenant}, IP: {IP}, Motivo: {Motivo}",
                    usuario, tenant, ip, motivoFalha);
            }
            else
            {
                _logger.LogInformation(
                    "✅ SEGURANÇA: Login bem-sucedido. Usuário: {Usuario}, Tenant: {Tenant}, IP: {IP}",
                    usuario, tenant, ip);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log de tentativa de login para usuário: {Usuario}", usuario);
        }
    }

    public async Task LogAlteracaoSenhaAsync(
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        bool sucesso,
        string? motivoFalha = null,
        string? ip = null)
    {
        try
        {
            var dados = new
            {
                Sucesso = sucesso,
                MotivoFalha = motivoFalha,
                DataHora = DateTime.Now
            };

            var log = new LogAuditoria
            {
                DataHora = DateTime.Now,
                Modulo = "Seguranca",
                TipoAcao = sucesso ? "ALTERACAO_SENHA_SUCESSO" : "ALTERACAO_SENHA_FALHA",
                Entidade = "Senha",
                EntidadeId = usuarioCodigo.ToString(),
                Descricao = sucesso 
                    ? $"Senha alterada com sucesso para usuário {usuarioNome}" 
                    : $"Falha ao alterar senha do usuário {usuarioNome}: {motivoFalha}",
                DadosNovos = JsonSerializer.Serialize(dados),
                UsuarioCodigo = usuarioCodigo,
                UsuarioNome = usuarioNome,
                UsuarioGrupo = grupo,
                EnderecoIP = ip
            };

            await _repository.CreateAsync(log);

            _logger.LogInformation(
                "🔑 SEGURANÇA: Alteração de senha {Status}. Usuário: {Usuario} ({Codigo}), IP: {IP}",
                sucesso ? "bem-sucedida" : "falhou",
                usuarioNome, usuarioCodigo, ip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log de alteração de senha para usuário: {Usuario}", usuarioNome);
        }
    }

    public async Task LogAcessoDadosSensiveisAsync(
        string tipoAcesso,
        string descricao,
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        string? ip = null)
    {
        try
        {
            var dados = new
            {
                TipoAcesso = tipoAcesso,
                Descricao = descricao,
                DataHora = DateTime.Now
            };

            var log = new LogAuditoria
            {
                DataHora = DateTime.Now,
                Modulo = "Seguranca",
                TipoAcao = "ACESSO_DADOS_SENSIVEIS",
                Entidade = tipoAcesso,
                EntidadeId = "",
                Descricao = $"Acesso a dados sensíveis: {descricao}",
                DadosNovos = JsonSerializer.Serialize(dados),
                UsuarioCodigo = usuarioCodigo,
                UsuarioNome = usuarioNome,
                UsuarioGrupo = grupo,
                EnderecoIP = ip
            };

            await _repository.CreateAsync(log);

            _logger.LogInformation(
                "📊 SEGURANÇA: Acesso a dados sensíveis. Tipo: {Tipo}, Usuário: {Usuario}, IP: {IP}",
                tipoAcesso, usuarioNome, ip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log de acesso a dados sensíveis");
        }
    }

    public async Task LogAcessoNaoAutorizadoAsync(
        string recurso,
        string metodo,
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        string? ip = null)
    {
        try
        {
            var dados = new
            {
                Recurso = recurso,
                Metodo = metodo,
                DataHora = DateTime.Now
            };

            var log = new LogAuditoria
            {
                DataHora = DateTime.Now,
                Modulo = "Seguranca",
                TipoAcao = "ACESSO_NAO_AUTORIZADO",
                Entidade = "Permissao",
                EntidadeId = recurso,
                Descricao = $"Tentativa de acesso não autorizado ao recurso {recurso} via {metodo}",
                DadosNovos = JsonSerializer.Serialize(dados),
                UsuarioCodigo = usuarioCodigo,
                UsuarioNome = usuarioNome,
                UsuarioGrupo = grupo,
                EnderecoIP = ip
            };

            await _repository.CreateAsync(log);

            _logger.LogWarning(
                "🚫 SEGURANÇA: Tentativa de acesso não autorizado! Recurso: {Recurso}, Método: {Metodo}, Usuário: {Usuario} ({Codigo}), Grupo: {Grupo}, IP: {IP}",
                recurso, metodo, usuarioNome, usuarioCodigo, grupo, ip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log de acesso não autorizado");
        }
    }

    public async Task LogLogoutAsync(
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        string? ip = null)
    {
        try
        {
            var dados = new
            {
                DataHora = DateTime.Now
            };

            var log = new LogAuditoria
            {
                DataHora = DateTime.Now,
                Modulo = "Seguranca",
                TipoAcao = "LOGOUT",
                Entidade = "Sessao",
                EntidadeId = usuarioCodigo.ToString(),
                Descricao = $"Logout realizado pelo usuário {usuarioNome}",
                DadosNovos = JsonSerializer.Serialize(dados),
                UsuarioCodigo = usuarioCodigo,
                UsuarioNome = usuarioNome,
                UsuarioGrupo = grupo,
                EnderecoIP = ip
            };

            await _repository.CreateAsync(log);

            _logger.LogInformation(
                "👋 SEGURANÇA: Logout realizado. Usuário: {Usuario} ({Codigo}), IP: {IP}",
                usuarioNome, usuarioCodigo, ip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log de logout para usuário: {Usuario}", usuarioNome);
        }
    }
}
