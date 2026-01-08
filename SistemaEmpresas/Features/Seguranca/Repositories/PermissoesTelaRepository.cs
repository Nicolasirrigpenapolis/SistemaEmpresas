using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.Features.Seguranca.Dtos;
using SistemaEmpresas.Features.Seguranca.Services;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Features.Seguranca.Repositories;

/// <summary>
/// Interface do repositório de permissões por tela
/// </summary>
public interface IPermissoesTelaRepository
{
    // Templates
    Task<List<PermissoesTemplateListDto>> GetAllTemplatesAsync();
    Task<PermissoesTemplateComDetalhesDto?> GetTemplateByIdAsync(int id);
    Task<PermissoesTemplateComDetalhesDto?> GetTemplateByNomeAsync(string nome);
    Task<int> CreateTemplateAsync(PermissoesTemplateCreateDto dto);
    Task<bool> UpdateTemplateAsync(int id, PermissoesTemplateUpdateDto dto);
    Task<bool> DeleteTemplateAsync(int id);
    
    // Permissões de Tela
    Task<List<PermissoesTelaListDto>> GetPermissoesByGrupoAsync(string grupo);
    Task<PermissoesCompletasGrupoDto> GetPermissoesCompletasByGrupoAsync(string grupo);
    Task<PermissoesTelaListDto?> GetPermissaoTelaAsync(string grupo, string tela);
    Task<bool> SavePermissaoTelaAsync(PermissoesTelaCreateUpdateDto dto);
    Task<bool> SavePermissoesBatchAsync(PermissoesTelasBatchUpdateDto dto);
    Task<bool> DeletePermissaoTelaAsync(int id);
    Task<bool> DeletePermissoesGrupoAsync(string grupo);
    
    // Aplicar Templates
    Task<bool> AplicarTemplateAsync(AplicarTemplateDto dto);
    
    // Consulta de Permissões
    Task<PermissoesUsuarioLogadoDto> GetPermissoesUsuarioAsync(string usuario, string grupo);
    Task<bool> VerificarPermissaoAsync(string grupo, string tela, string acao);
    
    // Estatísticas
    Task<PermissoesEstatisticasDto> GetEstatisticasAsync();
    
    // Telas Disponíveis
    List<ModuloComTelasDto> GetTelasDisponiveis();
}

/// <summary>
/// Repositório de permissões por tela
/// </summary>
public class PermissoesTelaRepository : IPermissoesTelaRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PermissoesTelaRepository> _logger;

    public PermissoesTelaRepository(
        AppDbContext context,
        ILogger<PermissoesTelaRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Templates

    public async Task<List<PermissoesTemplateListDto>> GetAllTemplatesAsync()
    {
        _logger.LogInformation("Listando todos os templates de permissões");

        return await _context.PermissoesTemplates
            .Include(t => t.Detalhes)
            .Select(t => new PermissoesTemplateListDto
            {
                Id = t.Id,
                Nome = t.Nome,
                Descricao = t.Descricao,
                IsPadrao = t.IsPadrao,
                DataCriacao = t.DataCriacao,
                QuantidadeTelas = t.Detalhes.Count
            })
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    public async Task<PermissoesTemplateComDetalhesDto?> GetTemplateByIdAsync(int id)
    {
        _logger.LogInformation("Buscando template por ID: {Id}", id);

        var template = await _context.PermissoesTemplates
            .Include(t => t.Detalhes)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (template == null) return null;

        return MapTemplateToDto(template);
    }

    public async Task<PermissoesTemplateComDetalhesDto?> GetTemplateByNomeAsync(string nome)
    {
        _logger.LogInformation("Buscando template por nome: {Nome}", nome);

        var template = await _context.PermissoesTemplates
            .Include(t => t.Detalhes)
            .FirstOrDefaultAsync(t => t.Nome == nome);

        if (template == null) return null;

        return MapTemplateToDto(template);
    }

    public async Task<int> CreateTemplateAsync(PermissoesTemplateCreateDto dto)
    {
        _logger.LogInformation("Criando template: {Nome}", dto.Nome);

        var template = new PermissoesTemplate
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            IsPadrao = false,
            DataCriacao = DateTime.Now,
            Detalhes = dto.Detalhes.Select(d => new PermissoesTemplateDetalhe
            {
                Modulo = d.Modulo,
                Tela = d.Tela,
                Consultar = d.Consultar,
                Incluir = d.Incluir,
                Alterar = d.Alterar,
                Excluir = d.Excluir
            }).ToList()
        };

        _context.PermissoesTemplates.Add(template);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Template criado com sucesso: {Nome}, ID: {Id}", dto.Nome, template.Id);
        return template.Id;
    }

    public async Task<bool> UpdateTemplateAsync(int id, PermissoesTemplateUpdateDto dto)
    {
        _logger.LogInformation("Atualizando template ID: {Id}", id);

        var template = await _context.PermissoesTemplates
            .Include(t => t.Detalhes)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (template == null)
        {
            _logger.LogWarning("Template não encontrado: {Id}", id);
            return false;
        }

        // Templates padrão não podem ser alterados
        if (template.IsPadrao)
        {
            _logger.LogWarning("Tentativa de alterar template padrão: {Nome}", template.Nome);
            return false;
        }

        template.Nome = dto.Nome;
        template.Descricao = dto.Descricao;

        // Remove detalhes antigos e adiciona novos
        _context.PermissoesTemplateDetalhes.RemoveRange(template.Detalhes);
        
        template.Detalhes = dto.Detalhes.Select(d => new PermissoesTemplateDetalhe
        {
            TemplateId = id,
            Modulo = d.Modulo,
            Tela = d.Tela,
            Consultar = d.Consultar,
            Incluir = d.Incluir,
            Alterar = d.Alterar,
            Excluir = d.Excluir
        }).ToList();

        await _context.SaveChangesAsync();

        _logger.LogInformation("Template atualizado com sucesso: {Id}", id);
        return true;
    }

    public async Task<bool> DeleteTemplateAsync(int id)
    {
        _logger.LogInformation("Excluindo template ID: {Id}", id);

        var template = await _context.PermissoesTemplates.FindAsync(id);

        if (template == null)
        {
            _logger.LogWarning("Template não encontrado: {Id}", id);
            return false;
        }

        // Templates padrão não podem ser excluídos
        if (template.IsPadrao)
        {
            _logger.LogWarning("Tentativa de excluir template padrão: {Nome}", template.Nome);
            return false;
        }

        _context.PermissoesTemplates.Remove(template);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Template excluído com sucesso: {Id}", id);
        return true;
    }

    #endregion

    #region Permissões de Tela

    public async Task<List<PermissoesTelaListDto>> GetPermissoesByGrupoAsync(string grupo)
    {
        _logger.LogInformation("Listando permissões do grupo: {Grupo}", grupo);

        // Busca pelo nome do grupo diretamente (sem encriptação - tabela nova)
        var grupoNormalizado = grupo.ToUpper();

        return await _context.PermissoesTelas
            .Where(p => p.Grupo == grupoNormalizado)
            .OrderBy(p => p.Ordem)
            .ThenBy(p => p.Modulo)
            .ThenBy(p => p.NomeTela)
            .Select(p => new PermissoesTelaListDto
            {
                Id = p.Id,
                Grupo = grupo,
                Modulo = p.Modulo,
                Tela = p.Tela,
                NomeTela = p.NomeTela,
                Rota = p.Rota,
                Consultar = p.Consultar,
                Incluir = p.Incluir,
                Alterar = p.Alterar,
                Excluir = p.Excluir,
                Ordem = p.Ordem
            })
            .ToListAsync();
    }

    public async Task<PermissoesCompletasGrupoDto> GetPermissoesCompletasByGrupoAsync(string grupo)
    {
        _logger.LogInformation("Buscando permissões completas do grupo: {Grupo}", grupo);

        var permissoes = await GetPermissoesByGrupoAsync(grupo);

        // Agrupa por módulo
        var modulos = permissoes
            .GroupBy(p => p.Modulo)
            .Select(g => new ModuloPermissoesDto
            {
                Nome = g.Key,
                Icone = GetIconeModulo(g.Key),
                Ordem = GetOrdemModulo(g.Key),
                Telas = g.OrderBy(t => t.Ordem).ToList()
            })
            .OrderBy(m => m.Ordem)
            .ToList();

        // Verifica se é admin (PROGRAMADOR tem acesso total)
        var isAdmin = grupo.Equals("PROGRAMADOR", StringComparison.OrdinalIgnoreCase);

        return new PermissoesCompletasGrupoDto
        {
            Grupo = grupo.ToUpper(),
            NomeGrupo = grupo,
            IsAdmin = isAdmin,
            Modulos = modulos
        };
    }

    public async Task<PermissoesTelaListDto?> GetPermissaoTelaAsync(string grupo, string tela)
    {
        _logger.LogInformation("Buscando permissão: Grupo={Grupo}, Tela={Tela}", grupo, tela);

        // Usar nome do grupo diretamente (sem encriptação - tabela nova)
        var grupoNormalizado = grupo.ToUpper();

        var permissao = await _context.PermissoesTelas
            .FirstOrDefaultAsync(p => p.Grupo == grupoNormalizado && p.Tela == tela);

        if (permissao == null) return null;

        return new PermissoesTelaListDto
        {
            Id = permissao.Id,
            Grupo = grupo,
            Modulo = permissao.Modulo,
            Tela = permissao.Tela,
            NomeTela = permissao.NomeTela,
            Rota = permissao.Rota,
            Consultar = permissao.Consultar,
            Incluir = permissao.Incluir,
            Alterar = permissao.Alterar,
            Excluir = permissao.Excluir,
            Ordem = permissao.Ordem
        };
    }

    public async Task<bool> SavePermissaoTelaAsync(PermissoesTelaCreateUpdateDto dto)
    {
        _logger.LogInformation("Salvando permissão: Grupo={Grupo}, Tela={Tela}", dto.Grupo, dto.Tela);

        // Usar nome do grupo diretamente (sem encriptação - tabela nova)
        var grupoNormalizado = dto.Grupo.ToUpper();

        var permissao = await _context.PermissoesTelas
            .FirstOrDefaultAsync(p => p.Grupo == grupoNormalizado && p.Tela == dto.Tela);

        if (permissao == null)
        {
            // Cria nova permissão
            permissao = new PermissoesTela
            {
                Grupo = grupoNormalizado,
                Modulo = dto.Modulo,
                Tela = dto.Tela,
                NomeTela = dto.NomeTela,
                Rota = dto.Rota,
                Consultar = dto.Consultar,
                Incluir = dto.Incluir,
                Alterar = dto.Alterar,
                Excluir = dto.Excluir,
                Ordem = dto.Ordem
            };
            _context.PermissoesTelas.Add(permissao);
        }
        else
        {
            // Atualiza permissão existente
            permissao.Consultar = dto.Consultar;
            permissao.Incluir = dto.Incluir;
            permissao.Alterar = dto.Alterar;
            permissao.Excluir = dto.Excluir;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SavePermissoesBatchAsync(PermissoesTelasBatchUpdateDto dto)
    {
        _logger.LogInformation("Salvando permissões em lote para grupo: {Grupo}", dto.Grupo);

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var permissao in dto.Permissoes)
            {
                permissao.Grupo = dto.Grupo;
                await SavePermissaoTelaAsync(permissao);
            }

            await transaction.CommitAsync();
            _logger.LogInformation("Permissões salvas em lote com sucesso para grupo: {Grupo}", dto.Grupo);
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Erro ao salvar permissões em lote para grupo: {Grupo}", dto.Grupo);
            return false;
        }
    }

    public async Task<bool> DeletePermissaoTelaAsync(int id)
    {
        _logger.LogInformation("Excluindo permissão ID: {Id}", id);

        var permissao = await _context.PermissoesTelas.FindAsync(id);

        if (permissao == null)
        {
            _logger.LogWarning("Permissão não encontrada: {Id}", id);
            return false;
        }

        _context.PermissoesTelas.Remove(permissao);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletePermissoesGrupoAsync(string grupo)
    {
        _logger.LogInformation("Excluindo todas as permissões do grupo: {Grupo}", grupo);

        // Usar nome do grupo diretamente (sem encriptação - tabela nova)
        var grupoNormalizado = grupo.ToUpper();

        var permissoes = await _context.PermissoesTelas
            .Where(p => p.Grupo == grupoNormalizado)
            .ToListAsync();

        _context.PermissoesTelas.RemoveRange(permissoes);
        await _context.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Aplicar Templates

    public async Task<bool> AplicarTemplateAsync(AplicarTemplateDto dto)
    {
        _logger.LogInformation("Aplicando template {TemplateId} ao grupo {Grupo}", dto.TemplateId, dto.Grupo);

        var template = await _context.PermissoesTemplates
            .Include(t => t.Detalhes)
            .FirstOrDefaultAsync(t => t.Id == dto.TemplateId);

        if (template == null)
        {
            _logger.LogWarning("Template não encontrado: {TemplateId}", dto.TemplateId);
            return false;
        }

        // Usar nome do grupo diretamente (sem encriptação - tabela nova)
        var grupoNormalizado = dto.Grupo.ToUpper();

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Se deve substituir, remove permissões existentes
            if (dto.SubstituirExistentes)
            {
                await DeletePermissoesGrupoAsync(dto.Grupo);
            }

            // Busca a lista de telas disponíveis para obter informações completas
            var telasDisponiveis = GetTelasDisponiveisFlat();

            // Aplica as permissões do template
            foreach (var detalhe in template.Detalhes)
            {
                var telaInfo = telasDisponiveis.FirstOrDefault(t => t.Tela == detalhe.Tela);
                
                var permissao = new PermissoesTela
                {
                    Grupo = grupoNormalizado,
                    Modulo = detalhe.Modulo,
                    Tela = detalhe.Tela,
                    NomeTela = telaInfo?.NomeTela ?? detalhe.Tela,
                    Rota = telaInfo?.Rota ?? $"/{detalhe.Tela.ToLower()}",
                    Consultar = detalhe.Consultar,
                    Incluir = detalhe.Incluir,
                    Alterar = detalhe.Alterar,
                    Excluir = detalhe.Excluir,
                    Ordem = telaInfo?.Ordem ?? 0
                };

                // Verifica se já existe
                var existente = await _context.PermissoesTelas
                    .FirstOrDefaultAsync(p => p.Grupo == grupoNormalizado && p.Tela == detalhe.Tela);

                if (existente != null)
                {
                    existente.Consultar = permissao.Consultar;
                    existente.Incluir = permissao.Incluir;
                    existente.Alterar = permissao.Alterar;
                    existente.Excluir = permissao.Excluir;
                }
                else
                {
                    _context.PermissoesTelas.Add(permissao);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Template aplicado com sucesso ao grupo {Grupo}", dto.Grupo);
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Erro ao aplicar template ao grupo {Grupo}", dto.Grupo);
            return false;
        }
    }

    #endregion

    #region Consulta de Permissões

    public async Task<PermissoesUsuarioLogadoDto> GetPermissoesUsuarioAsync(string usuario, string grupo)
    {
        _logger.LogInformation("Buscando permissões do usuário: {Usuario}", usuario);

        // PROGRAMADOR tem acesso total ao sistema
        var isAdmin = grupo.Equals("PROGRAMADOR", StringComparison.OrdinalIgnoreCase);
        var permissoes = await GetPermissoesByGrupoAsync(grupo);

        var result = new PermissoesUsuarioLogadoDto
        {
            Usuario = usuario,
            Grupo = grupo,
            IsAdmin = isAdmin,
            Telas = new Dictionary<string, PermissaoTelaResumoDto>()
        };

        // Se admin (PROGRAMADOR), tem todas as permissões
        if (isAdmin)
        {
            var todasTelas = GetTelasDisponiveisFlat();
            foreach (var tela in todasTelas)
            {
                result.Telas[tela.Tela] = new PermissaoTelaResumoDto
                {
                    C = true,
                    I = true,
                    A = true,
                    E = true
                };
            }
        }
        else
        {
            foreach (var permissao in permissoes)
            {
                result.Telas[permissao.Tela] = new PermissaoTelaResumoDto
                {
                    C = permissao.Consultar,
                    I = permissao.Incluir,
                    A = permissao.Alterar,
                    E = permissao.Excluir
                };
            }
        }

        return result;
    }

    public async Task<bool> VerificarPermissaoAsync(string grupo, string tela, string acao)
    {
        _logger.LogDebug("Verificando permissão: Grupo={Grupo}, Tela={Tela}, Acao={Acao}", grupo, tela, acao);

        // Admin tem todas as permissões
        if (AdminGroupHelper.IsAdminGroup(grupo))
            return true;

        var permissao = await GetPermissaoTelaAsync(grupo, tela);

        if (permissao == null)
            return false;

        return acao.ToLower() switch
        {
            "consultar" or "c" => permissao.Consultar,
            "incluir" or "i" => permissao.Incluir,
            "alterar" or "a" => permissao.Alterar,
            "excluir" or "e" => permissao.Excluir,
            _ => false
        };
    }

    #endregion

    #region Estatísticas

    public async Task<PermissoesEstatisticasDto> GetEstatisticasAsync()
    {
        _logger.LogInformation("Calculando estatísticas de permissões");

        var grupos = await _context.PwGrupos.CountAsync();
        var usuarios = await _context.PwUsuarios.ToListAsync();
        var templates = await _context.PermissoesTemplates.CountAsync();
        var telas = await _context.PermissoesTelas.Select(p => new { p.Grupo, p.Tela }).Distinct().CountAsync();

        return new PermissoesEstatisticasDto
        {
            TotalGrupos = grupos,
            TotalUsuarios = usuarios.Count,
            TotalUsuariosAtivos = usuarios.Count(u => u.PwAtivo),
            TotalUsuariosInativos = usuarios.Count(u => !u.PwAtivo),
            TotalTemplates = templates,
            TotalTelasConfiguradas = telas
        };
    }

    #endregion

    #region Telas Disponíveis

    /// <summary>
    /// Retorna a lista de todas as telas disponíveis no sistema agrupadas por módulo
    /// </summary>
    public List<ModuloComTelasDto> GetTelasDisponiveis()
    {
        return new List<ModuloComTelasDto>
        {
            new ModuloComTelasDto
            {
                Nome = "Dashboard",
                Icone = "LayoutDashboard",
                Ordem = 1,
                Telas = new List<TelaDisponivelDto>
                {
                    new() { Modulo = "Dashboard", Tela = "Dashboard", NomeTela = "Dashboard", Rota = "/", Icone = "LayoutDashboard", Ordem = 1, RequirePermissao = false }
                }
            },
            new ModuloComTelasDto
            {
                Nome = "Cadastros",
                Icone = "Database",
                Ordem = 2,
                Telas = new List<TelaDisponivelDto>
                {
                    new() { Modulo = "Cadastros", Tela = "Geral", NomeTela = "Cadastro Geral", Rota = "/cadastros/geral", Icone = "Users", Ordem = 1 },
                    new() { Modulo = "Cadastros", Tela = "Produtos", NomeTela = "Produtos", Rota = "/cadastros/produtos", Icone = "Package", Ordem = 2 },
                    new() { Modulo = "Cadastros", Tela = "Emitentes", NomeTela = "Emitentes", Rota = "/emitentes", Icone = "Building", Ordem = 3 }
                }
            },
            new ModuloComTelasDto
            {
                Nome = "Estoque",
                Icone = "Warehouse",
                Ordem = 3,
                Telas = new List<TelaDisponivelDto>
                {
                    new() { Modulo = "Estoque", Tela = "MovimentoContabil", NomeTela = "Movimento Contábil", Rota = "/estoque/movimento-contabil", Icone = "ClipboardCheck", Ordem = 1 }
                }
            },
            new ModuloComTelasDto
            {
                Nome = "Fiscal",
                Icone = "FileText",
                Ordem = 4,
                Telas = new List<TelaDisponivelDto>
                {
                    new() { Modulo = "Fiscal", Tela = "NotaFiscal", NomeTela = "Notas Fiscais", Rota = "/notas-fiscais", Icone = "FileText", Ordem = 1 },
                    new() { Modulo = "Fiscal", Tela = "ClassificacaoFiscal", NomeTela = "Classificação Fiscal", Rota = "/classificacao-fiscal", Icone = "Tag", Ordem = 2 },
                    new() { Modulo = "Fiscal", Tela = "ClassTrib", NomeTela = "Classificação Tributária (IBS/CBS)", Rota = "/classtrib", Icone = "Calculator", Ordem = 3 }
                }
            },
            new ModuloComTelasDto
            {
                Nome = "Transporte",
                Icone = "Truck",
                Ordem = 4,
                Telas = new List<TelaDisponivelDto>
                {
                    new() { Modulo = "Transporte", Tela = "Veiculo", NomeTela = "Veículos", Rota = "/transporte/veiculos", Icone = "Car", Ordem = 1 },
                    new() { Modulo = "Transporte", Tela = "Reboque", NomeTela = "Reboques", Rota = "/transporte/reboques", Icone = "Truck", Ordem = 2 },
                    new() { Modulo = "Transporte", Tela = "Motorista", NomeTela = "Motoristas", Rota = "/transporte/motoristas", Icone = "User", Ordem = 3 },
                    new() { Modulo = "Transporte", Tela = "Viagem", NomeTela = "Viagens", Rota = "/transporte/viagens", Icone = "Route", Ordem = 4 },
                    new() { Modulo = "Transporte", Tela = "ManutencaoVeiculo", NomeTela = "Manutenções", Rota = "/transporte/manutencoes", Icone = "Wrench", Ordem = 5 }
                }
            },
            new ModuloComTelasDto
            {
                Nome = "Auditoria",
                Icone = "Shield",
                Ordem = 5,
                Telas = new List<TelaDisponivelDto>
                {
                    new() { Modulo = "Auditoria", Tela = "Logs", NomeTela = "Logs de Auditoria", Rota = "/logs", Icone = "Activity", Ordem = 1 }
                }
            },
            new ModuloComTelasDto
            {
                Nome = "Sistema",
                Icone = "Settings",
                Ordem = 99,
                Telas = new List<TelaDisponivelDto>
                {
                    new() { Modulo = "Sistema", Tela = "DadosEmitente", NomeTela = "Configurações do Sistema", Rota = "/emitente", Icone = "Settings", Ordem = 1 },
                    new() { Modulo = "Sistema", Tela = "Usuarios", NomeTela = "Usuários e Permissões", Rota = "/usuarios", Icone = "Users", Ordem = 2 }
                }
            }
        };
    }

    /// <summary>
    /// Retorna lista plana de telas disponíveis
    /// </summary>
    private List<TelaDisponivelDto> GetTelasDisponiveisFlat()
    {
        return GetTelasDisponiveis().SelectMany(m => m.Telas).ToList();
    }

    #endregion

    #region Helpers

    private static PermissoesTemplateComDetalhesDto MapTemplateToDto(PermissoesTemplate template)
    {
        return new PermissoesTemplateComDetalhesDto
        {
            Id = template.Id,
            Nome = template.Nome,
            Descricao = template.Descricao,
            IsPadrao = template.IsPadrao,
            DataCriacao = template.DataCriacao,
            Detalhes = template.Detalhes.Select(d => new PermissoesTemplateDetalheDto
            {
                Id = d.Id,
                Modulo = d.Modulo,
                Tela = d.Tela,
                Consultar = d.Consultar,
                Incluir = d.Incluir,
                Alterar = d.Alterar,
                Excluir = d.Excluir
            }).ToList()
        };
    }

    private static string GetIconeModulo(string modulo)
    {
        return modulo switch
        {
            "Dashboard" => "LayoutDashboard",
            "Cadastros" => "Database",
            "Estoque" => "Package",
            "Comercial" => "ShoppingCart",
            "Fiscal" => "FileText",
            "Transporte" => "Truck",
            "Auditoria" => "Shield",
            "Financeiro" => "DollarSign",
            "Sistema" => "Settings",
            _ => "Folder"
        };
    }

    private static int GetOrdemModulo(string modulo)
    {
        return modulo switch
        {
            "Dashboard" => 1,
            "Cadastros" => 2,
            "Estoque" => 3,
            "Comercial" => 4,
            "Fiscal" => 5,
            "Transporte" => 6,
            "Auditoria" => 7,
            "Financeiro" => 8,
            "Sistema" => 99,
            _ => 50
        };
    }

    #endregion
}
