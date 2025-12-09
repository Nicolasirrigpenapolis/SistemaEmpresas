using Microsoft.Extensions.Logging;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Repositories;

namespace SistemaEmpresas.Services;

/// <summary>
/// Interface do serviço de gerenciamento de usuários
/// </summary>
public interface IUsuarioManagementService
{
    // Grupos
    Task<List<GrupoListDto>> ListarGruposAsync();
    Task<OperacaoResultDto> CriarGrupoAsync(GrupoCreateDto dto);
    Task<OperacaoResultDto> ExcluirGrupoAsync(string nome);

    // Usuários
    Task<List<GrupoComUsuariosDto>> ListarArvoreUsuariosAsync();
    Task<UsuarioListDto?> ObterUsuarioAsync(string nome);
    Task<OperacaoResultDto> CriarUsuarioAsync(UsuarioCreateDto dto);
    Task<OperacaoResultDto> AtualizarUsuarioAsync(string nomeAtual, UsuarioUpdateDto dto);
    Task<OperacaoResultDto> ExcluirUsuarioAsync(string nome);
    Task<OperacaoResultDto> MoverUsuarioAsync(MoverUsuarioDto dto);

    // Permissões
    Task<PermissoesGrupoDto> ObterPermissoesGrupoAsync(string grupo);
    Task<OperacaoResultDto> AtualizarPermissoesAsync(AtualizarPermissoesDto dto);
    Task<OperacaoResultDto> CopiarPermissoesAsync(CopiarPermissoesDto dto);
    Task<List<ModuloTabelasDto>> ListarTabelasDisponiveisAsync();
    Task<List<PermissaoTabelaDto>> ListarMenusDisponiveisAsync();
}

/// <summary>
/// Serviço de gerenciamento de usuários
/// Implementa regras de negócio mantendo compatibilidade com VB6
/// </summary>
public class UsuarioManagementService : IUsuarioManagementService
{
    private readonly IUsuarioManagementRepository _repository;
    private readonly ILogger<UsuarioManagementService> _logger;

    public UsuarioManagementService(
        IUsuarioManagementRepository repository,
        ILogger<UsuarioManagementService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Grupos

    public async Task<List<GrupoListDto>> ListarGruposAsync()
    {
        return await _repository.GetAllGruposAsync();
    }

    public async Task<OperacaoResultDto> CriarGrupoAsync(GrupoCreateDto dto)
    {
        try
        {
            // Validações
            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do grupo é obrigatório"
                };
            }

            if (dto.Nome.Length > 25)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do grupo deve ter no máximo 25 caracteres"
                };
            }

            // Verifica se já existe
            if (await _repository.GrupoExisteAsync(dto.Nome))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Já existe um grupo com este nome"
                };
            }

            // Cria o grupo
            var resultado = await _repository.CreateGrupoAsync(dto.Nome.ToUpper().Trim());

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Grupo criado com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao criar grupo"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar grupo: {Nome}", dto.Nome);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao criar grupo",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> ExcluirGrupoAsync(string nome)
    {
        try
        {
            // Validações
            if (string.IsNullOrWhiteSpace(nome))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do grupo é obrigatório"
                };
            }

            // Não pode excluir grupo administrador
            if (AdminGroupHelper.IsAdminGroup(nome))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "O grupo administrador não pode ser excluído"
                };
            }

            // Verifica se grupo existe
            var grupo = await _repository.GetGrupoByNomeAsync(nome);
            if (grupo == null)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Grupo não encontrado"
                };
            }

            // Verifica se tem usuários
            if (grupo.QuantidadeUsuarios > 0)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = $"O grupo possui {grupo.QuantidadeUsuarios} usuário(s). Mova-os para outro grupo antes de excluir."
                };
            }

            // Exclui o grupo
            var resultado = await _repository.DeleteGrupoAsync(nome);

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Grupo excluído com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao excluir grupo"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir grupo: {Nome}", nome);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao excluir grupo",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    #endregion

    #region Usuários

    public async Task<List<GrupoComUsuariosDto>> ListarArvoreUsuariosAsync()
    {
        return await _repository.GetArvoreGruposUsuariosAsync();
    }

    public async Task<UsuarioListDto?> ObterUsuarioAsync(string nome)
    {
        return await _repository.GetUsuarioByNomeAsync(nome);
    }

    public async Task<OperacaoResultDto> CriarUsuarioAsync(UsuarioCreateDto dto)
    {
        try
        {
            var erros = new List<string>();

            // Validações
            if (string.IsNullOrWhiteSpace(dto.Nome))
                erros.Add("Nome do usuário é obrigatório");

            if (dto.Nome?.Length > 25)
                erros.Add("Nome do usuário deve ter no máximo 25 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Senha))
                erros.Add("Senha é obrigatória");

            if (dto.Senha?.Length > 25)
                erros.Add("Senha deve ter no máximo 25 caracteres");

            if (dto.Senha != dto.ConfirmarSenha)
                erros.Add("As senhas não conferem");

            if (string.IsNullOrWhiteSpace(dto.Grupo))
                erros.Add("Grupo é obrigatório");

            if (erros.Count > 0)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Dados inválidos",
                    Erros = erros
                };
            }

            // Verifica se usuário já existe
            if (await _repository.UsuarioExisteAsync(dto.Nome!))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Já existe um usuário com este nome"
                };
            }

            // Verifica se grupo existe. Se não existir, tenta criar automaticamente
            // (isso evita erro 400 quando o admin informou um novo grupo ao criar um usuário)
            if (!await _repository.GrupoExisteAsync(dto.Grupo))
            {
                var grupoNomeNormalizado = dto.Grupo.Trim().ToUpper();
                var criado = await _repository.CreateGrupoAsync(grupoNomeNormalizado);
                if (!criado)
                {
                    return new OperacaoResultDto
                    {
                        Sucesso = false,
                        Mensagem = "Grupo não encontrado e não foi possível criá-lo"
                    };
                }
            }

            // Cria o usuário
            var resultado = await _repository.CreateUsuarioAsync(dto);

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Usuário criado com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao criar usuário"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário: {Nome}", dto.Nome);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao criar usuário",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> AtualizarUsuarioAsync(string nomeAtual, UsuarioUpdateDto dto)
    {
        try
        {
            var erros = new List<string>();

            // Validações
            if (string.IsNullOrWhiteSpace(nomeAtual))
                erros.Add("Nome do usuário é obrigatório");

            if (!string.IsNullOrEmpty(dto.NovaSenha))
            {
                if (dto.NovaSenha.Length > 25)
                    erros.Add("Senha deve ter no máximo 25 caracteres");

                if (dto.NovaSenha != dto.ConfirmarNovaSenha)
                    erros.Add("As senhas não conferem");
            }

            // Grupo é opcional - se não informado ou "SEM GRUPO", mantém o atual
            // Não exigir grupo para operações simples como ativar/inativar

            if (erros.Count > 0)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Dados inválidos",
                    Erros = erros
                };
            }

            // Verifica se usuário existe
            var usuario = await _repository.GetUsuarioByNomeAsync(nomeAtual);
            if (usuario == null)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado"
                };
            }

            // Verifica se está tirando o último admin (apenas se estiver mudando de grupo)
            if (usuario.IsAdmin && 
                !string.IsNullOrWhiteSpace(dto.Grupo) && 
                !dto.Grupo.Equals("SEM GRUPO", StringComparison.OrdinalIgnoreCase) &&
                !AdminGroupHelper.IsAdminGroup(dto.Grupo))
            {
                var grupos = await _repository.GetAllGruposAsync();
                var grupoAdmin = grupos.FirstOrDefault(g => g.IsAdmin);
                if (grupoAdmin?.QuantidadeUsuarios <= 1)
                {
                    return new OperacaoResultDto
                    {
                        Sucesso = false,
                        Mensagem = "Não é possível remover o último administrador do grupo administrador"
                    };
                }
            }

            // Atualiza o usuário
            var resultado = await _repository.UpdateUsuarioAsync(nomeAtual, dto);

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Usuário atualizado com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao atualizar usuário"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Nome}", nomeAtual);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao atualizar usuário",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> ExcluirUsuarioAsync(string nome)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do usuário é obrigatório"
                };
            }

            // Verifica se usuário existe
            var usuario = await _repository.GetUsuarioByNomeAsync(nome);
            if (usuario == null)
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado"
                };
            }

            // Verifica se é o último admin
            if (usuario.IsAdmin)
            {
                var grupos = await _repository.GetAllGruposAsync();
                var grupoAdmin = grupos.FirstOrDefault(g => g.IsAdmin);
                if (grupoAdmin?.QuantidadeUsuarios <= 1)
                {
                    return new OperacaoResultDto
                    {
                        Sucesso = false,
                        Mensagem = "Não é possível excluir o último administrador do sistema"
                    };
                }
            }

            // Exclui o usuário
            var resultado = await _repository.DeleteUsuarioAsync(nome);

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Usuário excluído com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao excluir usuário"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir usuário: {Nome}", nome);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao excluir usuário",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> MoverUsuarioAsync(MoverUsuarioDto dto)
    {
        try
        {
            // Validações
            if (string.IsNullOrWhiteSpace(dto.NomeUsuario))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Nome do usuário é obrigatório"
                };
            }

            if (string.IsNullOrWhiteSpace(dto.GrupoDestino))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Grupo de destino é obrigatório"
                };
            }

            // Verifica se está tirando o último admin
            if (AdminGroupHelper.IsAdminGroup(dto.GrupoOrigem) &&
                !AdminGroupHelper.IsAdminGroup(dto.GrupoDestino))
            {
                var grupos = await _repository.GetAllGruposAsync();
                var grupoAdmin = grupos.FirstOrDefault(g => g.IsAdmin);
                if (grupoAdmin?.QuantidadeUsuarios <= 1)
                {
                    return new OperacaoResultDto
                    {
                        Sucesso = false,
                        Mensagem = "Não é possível remover o último administrador do grupo administrador"
                    };
                }
            }

            // Move o usuário
            var resultado = await _repository.MoverUsuarioParaGrupoAsync(
                dto.NomeUsuario, dto.GrupoOrigem, dto.GrupoDestino);

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Usuário movido com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao mover usuário"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao mover usuário: {Nome}", dto.NomeUsuario);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao mover usuário",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    #endregion

    #region Permissões

    public async Task<PermissoesGrupoDto> ObterPermissoesGrupoAsync(string grupo)
    {
        return await _repository.GetPermissoesGrupoAsync(grupo);
    }

    public async Task<OperacaoResultDto> AtualizarPermissoesAsync(AtualizarPermissoesDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Grupo))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Grupo é obrigatório"
                };
            }

            // Grupo admin tem acesso total, não grava permissões
            if (AdminGroupHelper.IsAdminGroup(dto.Grupo))
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Grupo administrador possui acesso total"
                };
            }

            // Salva permissões
            var resultado = await _repository.SavePermissoesEmLoteAsync(dto.Grupo, dto.Permissoes);

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Permissões atualizadas com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao atualizar permissões"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar permissões do grupo: {Grupo}", dto.Grupo);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao atualizar permissões",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    public async Task<OperacaoResultDto> CopiarPermissoesAsync(CopiarPermissoesDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.GrupoOrigem))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Grupo de origem é obrigatório"
                };
            }

            if (string.IsNullOrWhiteSpace(dto.GrupoDestino))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Grupo de destino é obrigatório"
                };
            }

            // Não pode copiar de/para grupo admin
            if (AdminGroupHelper.IsAdminGroup(dto.GrupoOrigem) ||
                AdminGroupHelper.IsAdminGroup(dto.GrupoDestino))
            {
                return new OperacaoResultDto
                {
                    Sucesso = false,
                    Mensagem = "Não é possível copiar permissões de/para o grupo administrador"
                };
            }

            // Copia permissões
            var resultado = await _repository.CopiarPermissoesAsync(dto.GrupoOrigem, dto.GrupoDestino);

            if (resultado)
            {
                return new OperacaoResultDto
                {
                    Sucesso = true,
                    Mensagem = "Permissões copiadas com sucesso"
                };
            }

            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro ao copiar permissões"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao copiar permissões de {Origem} para {Destino}", 
                dto.GrupoOrigem, dto.GrupoDestino);
            return new OperacaoResultDto
            {
                Sucesso = false,
                Mensagem = "Erro interno ao copiar permissões",
                Erros = new List<string> { ex.Message }
            };
        }
    }

    /// <summary>
    /// Lista tabelas disponíveis agrupadas por módulo
    /// Baseado na estrutura do sistema (menus do VB6)
    /// </summary>
    public Task<List<ModuloTabelasDto>> ListarTabelasDisponiveisAsync()
    {
        // Tabelas organizadas por módulo do sistema
        var modulos = new List<ModuloTabelasDto>
        {
            new ModuloTabelasDto
            {
                Nome = "Comercial",
                Icone = "ShoppingCart",
                Tabelas = new List<PermissaoTabelaDto>
                {
                    CriarPermissaoTabela("ORCAMENTOS", "Orçamentos"),
                    CriarPermissaoTabela("PEDIDOS", "Pedidos"),
                    CriarPermissaoTabela("PROJETOS", "Projetos de Irrigação"),
                    CriarPermissaoTabela("LICITACOES", "Licitações"),
                    CriarPermissaoTabela("CLIENTES", "Clientes (Geral)"),
                }
            },
            new ModuloTabelasDto
            {
                Nome = "Operacional",
                Icone = "Settings",
                Tabelas = new List<PermissaoTabelaDto>
                {
                    CriarPermissaoTabela("ORDENS_SERVICO", "Ordens de Serviço"),
                    CriarPermissaoTabela("PRODUCAO", "Produção"),
                    CriarPermissaoTabela("ESTOQUE", "Controle de Estoque"),
                    CriarPermissaoTabela("COMPRAS", "Pedidos de Compra"),
                    CriarPermissaoTabela("REQUISICOES", "Requisições"),
                }
            },
            new ModuloTabelasDto
            {
                Nome = "Financeiro",
                Icone = "DollarSign",
                Tabelas = new List<PermissaoTabelaDto>
                {
                    CriarPermissaoTabela("CONTAS_PAGAR", "Contas a Pagar"),
                    CriarPermissaoTabela("CONTAS_RECEBER", "Contas a Receber"),
                    CriarPermissaoTabela("NOTAS_FISCAIS", "Notas Fiscais"),
                    CriarPermissaoTabela("CAIXA", "Movimento de Caixa"),
                    CriarPermissaoTabela("BANCOS", "Contas Bancárias"),
                }
            },
            new ModuloTabelasDto
            {
                Nome = "Cadastros",
                Icone = "Database",
                Tabelas = new List<PermissaoTabelaDto>
                {
                    CriarPermissaoTabela("PRODUTOS", "Produtos"),
                    CriarPermissaoTabela("FORNECEDORES", "Fornecedores"),
                    CriarPermissaoTabela("TRANSPORTADORAS", "Transportadoras"),
                    CriarPermissaoTabela("VENDEDORES", "Vendedores"),
                    CriarPermissaoTabela("CLASSIFICACAO_FISCAL", "Classificação Fiscal"),
                }
            },
            new ModuloTabelasDto
            {
                Nome = "Relatórios",
                Icone = "FileText",
                Tabelas = new List<PermissaoTabelaDto>
                {
                    CriarPermissaoTabela("REL_VENDAS", "Relatórios de Vendas"),
                    CriarPermissaoTabela("REL_ESTOQUE", "Relatórios de Estoque"),
                    CriarPermissaoTabela("REL_FINANCEIRO", "Relatórios Financeiros"),
                    CriarPermissaoTabela("REL_FISCAL", "Relatórios Fiscais"),
                }
            },
            new ModuloTabelasDto
            {
                Nome = "Sistema",
                Icone = "Cog",
                Tabelas = new List<PermissaoTabelaDto>
                {
                    CriarPermissaoTabela("USUARIOS", "Usuários e Permissões"),
                    CriarPermissaoTabela("PARAMETROS", "Parâmetros do Sistema"),
                    CriarPermissaoTabela("BACKUP", "Backup"),
                }
            }
        };

        return Task.FromResult(modulos);
    }

    /// <summary>
    /// Lista menus disponíveis (baseado nos menus do VB6)
    /// </summary>
    public Task<List<PermissaoTabelaDto>> ListarMenusDisponiveisAsync()
    {
        // Menus principais do sistema (baseado no VB6)
        var menus = new List<PermissaoTabelaDto>
        {
            // Arquivo
            CriarPermissaoMenu("10", "Arquivo"),
            CriarPermissaoMenu("11", "Backup"),
            
            // Cadastros
            CriarPermissaoMenu("20", "Cadastros"),
            CriarPermissaoMenu("21", "Clientes"),
            CriarPermissaoMenu("22", "Fornecedores"),
            CriarPermissaoMenu("23", "Produtos"),
            CriarPermissaoMenu("24", "Vendedores"),
            
            // Movimentação
            CriarPermissaoMenu("30", "Movimentação"),
            CriarPermissaoMenu("31", "Orçamentos"),
            CriarPermissaoMenu("32", "Pedidos"),
            CriarPermissaoMenu("33", "Notas Fiscais"),
            CriarPermissaoMenu("34", "Ordens de Serviço"),
            
            // Financeiro
            CriarPermissaoMenu("40", "Financeiro"),
            CriarPermissaoMenu("41", "Contas a Pagar"),
            CriarPermissaoMenu("42", "Contas a Receber"),
            CriarPermissaoMenu("43", "Caixa"),
            
            // Estoque
            CriarPermissaoMenu("50", "Estoque"),
            CriarPermissaoMenu("51", "Entradas"),
            CriarPermissaoMenu("52", "Saídas"),
            CriarPermissaoMenu("53", "Inventário"),
            
            // Relatórios
            CriarPermissaoMenu("60", "Relatórios"),
            CriarPermissaoMenu("61", "Vendas"),
            CriarPermissaoMenu("62", "Financeiros"),
            CriarPermissaoMenu("63", "Estoque"),
            
            // Sistema
            CriarPermissaoMenu("90", "Sistema"),
            CriarPermissaoMenu("91", "Configurações"),
            CriarPermissaoMenu("92", "Usuários"),
        };

        return Task.FromResult(menus);
    }

    #endregion

    #region Helpers

    private static PermissaoTabelaDto CriarPermissaoTabela(string nome, string nomeExibicao, string? modulo = null)
    {
        return new PermissaoTabelaDto
        {
            Projeto = " ",
            Nome = nome,
            NomeExibicao = nomeExibicao,
            Visualiza = true,
            Inclui = true,
            Modifica = true,
            Exclui = true,
            Tipo = "TABELA",
            Modulo = modulo
        };
    }

    private static PermissaoTabelaDto CriarPermissaoMenu(string codigo, string nomeExibicao)
    {
        return new PermissaoTabelaDto
        {
            Projeto = " ",
            Nome = codigo,
            NomeExibicao = nomeExibicao,
            Visualiza = true,
            Inclui = true,
            Modifica = true,
            Exclui = true,
            Tipo = "MENU"
        };
    }

    #endregion
}
