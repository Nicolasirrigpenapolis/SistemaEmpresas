using Microsoft.Extensions.Logging;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Repositories;

namespace SistemaEmpresas.Services;

/// <summary>
/// Interface do serviço de permissões por tela
/// </summary>
public interface IPermissoesTelaService
{
    // Templates
    Task<List<PermissoesTemplateListDto>> ListarTemplatesAsync();
    Task<PermissoesTemplateComDetalhesDto?> ObterTemplateAsync(int id);
    Task<OperacaoResultDto> CriarTemplateAsync(PermissoesTemplateCreateDto dto);
    Task<OperacaoResultDto> AtualizarTemplateAsync(int id, PermissoesTemplateUpdateDto dto);
    Task<OperacaoResultDto> ExcluirTemplateAsync(int id);
    
    // Permissões por Grupo
    Task<PermissoesCompletasGrupoDto> ObterPermissoesGrupoAsync(string grupo);
    Task<OperacaoResultDto> SalvarPermissoesAsync(PermissoesTelasBatchUpdateDto dto);
    Task<OperacaoResultDto> AplicarTemplateAsync(AplicarTemplateDto dto);
    
    // Consulta de Permissões (para usuário logado)
    Task<PermissoesUsuarioLogadoDto> ObterMinhasPermissoesAsync(string usuario, string grupo);
    Task<PermissaoResultDto> VerificarPermissaoAsync(string grupo, VerificarPermissaoDto dto);
    
    // Telas Disponíveis
    List<ModuloComTelasDto> ListarTelasDisponiveis();
    
    // Estatísticas
    Task<PermissoesEstatisticasDto> ObterEstatisticasAsync();
    
    // Seeds (templates padrão)
    Task<OperacaoResultDto> CriarTemplatesPadraoAsync();
}

/// <summary>
/// Serviço de permissões por tela
/// </summary>
public class PermissoesTelaService : IPermissoesTelaService
{
    private readonly IPermissoesTelaRepository _repository;
    private readonly ILogger<PermissoesTelaService> _logger;

    public PermissoesTelaService(
        IPermissoesTelaRepository repository,
        ILogger<PermissoesTelaService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Templates

    public async Task<List<PermissoesTemplateListDto>> ListarTemplatesAsync()
    {
        return await _repository.GetAllTemplatesAsync();
    }

    public async Task<PermissoesTemplateComDetalhesDto?> ObterTemplateAsync(int id)
    {
        return await _repository.GetTemplateByIdAsync(id);
    }

    public async Task<OperacaoResultDto> CriarTemplateAsync(PermissoesTemplateCreateDto dto)
    {
        try
        {
            // Validações
            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do template é obrigatório"
                };
            }

            if (dto.Nome.Length > 100)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do template deve ter no máximo 100 caracteres"
                };
            }

            // Verifica se já existe
            var existente = await _repository.GetTemplateByNomeAsync(dto.Nome);
            if (existente != null)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Já existe um template com este nome"
                };
            }

            // Valida detalhes
            if (dto.Detalhes == null || !dto.Detalhes.Any())
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "O template deve ter pelo menos uma tela configurada"
                };
            }

            var id = await _repository.CreateTemplateAsync(dto);

            return new OperacaoResultDto
            {
                Sucesso = true,
                Mensagem = $"Template criado com sucesso (ID: {id})"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar template: {Nome}", dto.Nome);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao criar template",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> AtualizarTemplateAsync(int id, PermissoesTemplateUpdateDto dto)
    {
        try
        {
            // Validações
            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do template é obrigatório"
                };
            }

            // Verifica se template existe
            var template = await _repository.GetTemplateByIdAsync(id);
            if (template == null)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Template não encontrado"
                };
            }

            // Não permite editar templates padrão
            if (template.IsPadrao)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Não é possível editar templates padrão do sistema"
                };
            }

            // Verifica duplicidade de nome (excluindo o atual)
            var existente = await _repository.GetTemplateByNomeAsync(dto.Nome);
            if (existente != null && existente.Id != id)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Já existe outro template com este nome"
                };
            }

            var resultado = await _repository.UpdateTemplateAsync(id, dto);

            return resultado
                ? new OperacaoResultDto { Sucesso = true, Mensagem = "Template atualizado com sucesso" }
                : new OperacaoResultDto { Sucesso = false, Mensagem = "Erro ao atualizar template" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar template: {Id}", id);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao atualizar template",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> ExcluirTemplateAsync(int id)
    {
        try
        {
            var template = await _repository.GetTemplateByIdAsync(id);
            if (template == null)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Template não encontrado"
                };
            }

            // Não permite excluir templates padrão
            if (template.IsPadrao)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Não é possível excluir templates padrão do sistema"
                };
            }

            var resultado = await _repository.DeleteTemplateAsync(id);

            return resultado
                ? new OperacaoResultDto { Sucesso = true, Mensagem = "Template excluído com sucesso" }
                : new OperacaoResultDto { Sucesso = false, Mensagem = "Erro ao excluir template" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir template: {Id}", id);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao excluir template",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    #endregion

    #region Permissões por Grupo

    public async Task<PermissoesCompletasGrupoDto> ObterPermissoesGrupoAsync(string grupo)
    {
        return await _repository.GetPermissoesCompletasByGrupoAsync(grupo);
    }

    public async Task<OperacaoResultDto> SalvarPermissoesAsync(PermissoesTelasBatchUpdateDto dto)
    {
        try
        {
            // Validações
            if (string.IsNullOrWhiteSpace(dto.Grupo))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Grupo é obrigatório"
                };
            }

            if (dto.Permissoes == null || !dto.Permissoes.Any())
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nenhuma permissão para salvar"
                };
            }

            var resultado = await _repository.SavePermissoesBatchAsync(dto);

            return resultado
                ? new OperacaoResultDto { Sucesso = true, Mensagem = $"Permissões salvas com sucesso ({dto.Permissoes.Count} telas)" }
                : new OperacaoResultDto { Sucesso = false, Mensagem = "Erro ao salvar permissões" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar permissões do grupo: {Grupo}", dto.Grupo);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao salvar permissões",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> AplicarTemplateAsync(AplicarTemplateDto dto)
    {
        try
        {
            // Validações
            if (string.IsNullOrWhiteSpace(dto.Grupo))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Grupo é obrigatório"
                };
            }

            var template = await _repository.GetTemplateByIdAsync(dto.TemplateId);
            if (template == null)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Template não encontrado"
                };
            }

            var resultado = await _repository.AplicarTemplateAsync(dto);

            return resultado
                ? new OperacaoResultDto { Sucesso = true, Mensagem = $"Template '{template.Nome}' aplicado com sucesso ao grupo {dto.Grupo}" }
                : new OperacaoResultDto { Sucesso = false, Mensagem = "Erro ao aplicar template" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao aplicar template {TemplateId} ao grupo {Grupo}", dto.TemplateId, dto.Grupo);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao aplicar template",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    #endregion

    #region Consulta de Permissões

    public async Task<PermissoesUsuarioLogadoDto> ObterMinhasPermissoesAsync(string usuario, string grupo)
    {
        return await _repository.GetPermissoesUsuarioAsync(usuario, grupo);
    }

    public async Task<PermissaoResultDto> VerificarPermissaoAsync(string grupo, VerificarPermissaoDto dto)
    {
        var permitido = await _repository.VerificarPermissaoAsync(grupo, dto.Tela, dto.Acao);

        return new PermissaoResultDto
        {
            Permitido = permitido,
            Tela = dto.Tela,
            Acao = dto.Acao,
            Mensagem = permitido ? null : $"Acesso negado para {dto.Acao} em {dto.Tela}"
        };
    }

    #endregion

    #region Telas Disponíveis

    public List<ModuloComTelasDto> ListarTelasDisponiveis()
    {
        return _repository.GetTelasDisponiveis();
    }

    #endregion

    #region Estatísticas

    public async Task<PermissoesEstatisticasDto> ObterEstatisticasAsync()
    {
        return await _repository.GetEstatisticasAsync();
    }

    #endregion

    #region Seeds

    /// <summary>
    /// Cria templates padrão do sistema (Administrador e Consulta)
    /// </summary>
    public async Task<OperacaoResultDto> CriarTemplatesPadraoAsync()
    {
        try
        {
            var templates = await _repository.GetAllTemplatesAsync();
            var telasDisponiveis = _repository.GetTelasDisponiveis()
                .SelectMany(m => m.Telas)
                .Where(t => t.RequirePermissao)
                .ToList();

            var criados = new List<string>();

            // Template Administrador (acesso total)
            if (!templates.Any(t => t.Nome == "Administrador" && t.IsPadrao))
            {
                var templateAdmin = new PermissoesTemplateCreateDto
                {
                    Nome = "Administrador",
                    Descricao = "Acesso total a todas as telas do sistema",
                    Detalhes = telasDisponiveis.Select(t => new PermissoesTemplateDetalheDto
                    {
                        Modulo = t.Modulo,
                        Tela = t.Tela,
                        Consultar = true,
                        Incluir = true,
                        Alterar = true,
                        Excluir = true
                    }).ToList()
                };

                var id = await _repository.CreateTemplateAsync(templateAdmin);
                criados.Add("Administrador");
            }

            // Template Consulta (apenas visualização)
            if (!templates.Any(t => t.Nome == "Somente Consulta" && t.IsPadrao))
            {
                var templateConsulta = new PermissoesTemplateCreateDto
                {
                    Nome = "Somente Consulta",
                    Descricao = "Acesso apenas para consultar dados, sem permissão de alteração",
                    Detalhes = telasDisponiveis.Select(t => new PermissoesTemplateDetalheDto
                    {
                        Modulo = t.Modulo,
                        Tela = t.Tela,
                        Consultar = true,
                        Incluir = false,
                        Alterar = false,
                        Excluir = false
                    }).ToList()
                };

                await _repository.CreateTemplateAsync(templateConsulta);
                criados.Add("Somente Consulta");
            }

            // Template Comercial (vendedores)
            if (!templates.Any(t => t.Nome == "Comercial"))
            {
                var templateComercial = new PermissoesTemplateCreateDto
                {
                    Nome = "Comercial",
                    Descricao = "Acesso às áreas comerciais e cadastros básicos",
                    Detalhes = new List<PermissoesTemplateDetalheDto>
                    {
                        // Dashboard
                        new() { Modulo = "Dashboard", Tela = "Dashboard", Consultar = true, Incluir = false, Alterar = false, Excluir = false },
                        // Cadastros
                        new() { Modulo = "Cadastros", Tela = "Clientes", Consultar = true, Incluir = true, Alterar = true, Excluir = false },
                        new() { Modulo = "Cadastros", Tela = "Produtos", Consultar = true, Incluir = false, Alterar = false, Excluir = false },
                        // Comercial - acesso total
                        new() { Modulo = "Comercial", Tela = "Orcamentos", Consultar = true, Incluir = true, Alterar = true, Excluir = true },
                        new() { Modulo = "Comercial", Tela = "Pedidos", Consultar = true, Incluir = true, Alterar = true, Excluir = false },
                        new() { Modulo = "Comercial", Tela = "OrdensServico", Consultar = true, Incluir = false, Alterar = false, Excluir = false },
                        // Estoque - consulta
                        new() { Modulo = "Estoque", Tela = "ConsultaEstoque", Consultar = true, Incluir = false, Alterar = false, Excluir = false }
                    }
                };

                await _repository.CreateTemplateAsync(templateComercial);
                criados.Add("Comercial");
            }

            // Template Financeiro
            if (!templates.Any(t => t.Nome == "Financeiro"))
            {
                var templateFinanceiro = new PermissoesTemplateCreateDto
                {
                    Nome = "Financeiro",
                    Descricao = "Acesso às áreas financeiras",
                    Detalhes = new List<PermissoesTemplateDetalheDto>
                    {
                        // Dashboard
                        new() { Modulo = "Dashboard", Tela = "Dashboard", Consultar = true, Incluir = false, Alterar = false, Excluir = false },
                        // Cadastros - consulta
                        new() { Modulo = "Cadastros", Tela = "Clientes", Consultar = true, Incluir = false, Alterar = false, Excluir = false },
                        new() { Modulo = "Cadastros", Tela = "Fornecedores", Consultar = true, Incluir = false, Alterar = false, Excluir = false },
                        // Financeiro - acesso total
                        new() { Modulo = "Financeiro", Tela = "ContasReceber", Consultar = true, Incluir = true, Alterar = true, Excluir = true },
                        new() { Modulo = "Financeiro", Tela = "ContasPagar", Consultar = true, Incluir = true, Alterar = true, Excluir = true },
                        new() { Modulo = "Financeiro", Tela = "FluxoCaixa", Consultar = true, Incluir = false, Alterar = false, Excluir = false }
                    }
                };

                await _repository.CreateTemplateAsync(templateFinanceiro);
                criados.Add("Financeiro");
            }

            if (criados.Any())
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = $"Templates padrão criados: {string.Join(", ", criados)}"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = true,
                Mensagem = "Todos os templates padrão já existem"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar templates padrão");
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao criar templates padrão",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    #endregion
}
