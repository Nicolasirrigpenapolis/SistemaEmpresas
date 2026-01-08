using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.Features.Seguranca.Dtos;
using SistemaEmpresas.Features.Seguranca.Services;
using SistemaEmpresas.Models;
using SistemaEmpresas.Features.Auth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEmpresas.Features.Seguranca.Repositories;

/// <summary>
/// Repositório de gerenciamento de usuários (compatível com legado VB6).
/// </summary>
public interface IUsuarioManagementRepository
{
    // Grupos
    Task<List<GrupoListDto>> GetAllGruposAsync();
    Task<GrupoListDto?> GetGrupoByNomeAsync(string nome);
    Task<bool> GrupoExisteAsync(string nome);
    Task<bool> CreateGrupoAsync(string nome);
    Task<bool> DeleteGrupoAsync(string nome);

    // Usuários
    Task<List<UsuarioListDto>> GetAllUsuariosAsync();
    Task<List<UsuarioListDto>> GetUsuariosByGrupoAsync(string grupo);
    Task<UsuarioListDto?> GetUsuarioByNomeAsync(string nome);
    Task<bool> UsuarioExisteAsync(string nome);
    Task<bool> CreateUsuarioAsync(UsuarioCreateDto dto);
    Task<bool> UpdateUsuarioAsync(string nomeAtual, UsuarioUpdateDto dto);
    Task<bool> DeleteUsuarioAsync(string nome);
    Task<bool> MoverUsuarioParaGrupoAsync(string nomeUsuario, string grupoOrigem, string grupoDestino);

    // Permissões
    Task<PermissoesGrupoDto> GetPermissoesGrupoAsync(string grupo);
    Task<bool> SavePermissaoTabelaAsync(string grupo, PermissaoTabelaDto permissao);
    Task<bool> SavePermissoesEmLoteAsync(string grupo, List<PermissaoTabelaDto> permissoes);
    Task<bool> CopiarPermissoesAsync(string grupoOrigem, string grupoDestino);
    Task<bool> DeletePermissaoAsync(string grupo, string nomeTabela);

    // Estrutura hierárquica
    Task<List<GrupoComUsuariosDto>> GetArvoreGruposUsuariosAsync();
}

/// <summary>
/// Implementação concreta do repositório de gerenciamento de usuários.
/// </summary>
public class UsuarioManagementRepository : IUsuarioManagementRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsuarioManagementRepository> _logger;

    public UsuarioManagementRepository(
        AppDbContext context,
        ILogger<UsuarioManagementRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Grupos

    public async Task<List<GrupoListDto>> GetAllGruposAsync()
    {
        _logger.LogInformation("Listando todos os grupos");

        var grupos = await _context.GruposUsuarios
            .Include(g => g.Usuarios)
            .Where(g => g.Ativo)
            .ToListAsync();

        return grupos.Select(g => new GrupoListDto
        {
            Nome = g.Nome,
            QuantidadeUsuarios = g.Usuarios.Count,
            IsAdmin = AdminGroupHelper.IsAdminGroup(g.Nome)
        }).OrderBy(g => g.Nome).ToList();
    }

    public async Task<GrupoListDto?> GetGrupoByNomeAsync(string nome)
    {
        _logger.LogInformation("Buscando grupo por nome: {Nome}", nome);

        var grupo = await _context.GruposUsuarios
            .Include(g => g.Usuarios)
            .FirstOrDefaultAsync(g => g.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase) && g.Ativo);

        if (grupo == null) return null;

        return new GrupoListDto
        {
            Nome = grupo.Nome,
            QuantidadeUsuarios = grupo.Usuarios.Count,
            IsAdmin = AdminGroupHelper.IsAdminGroup(grupo.Nome)
        };
    }

    public async Task<bool> GrupoExisteAsync(string nome)
    {
        // Usa a tabela GrupoUsuario (nova) para verificar se o grupo existe
        return await _context.GruposUsuarios.AnyAsync(g => g.Nome.ToUpper() == nome.ToUpper());
    }

    public async Task<bool> CreateGrupoAsync(string nome)
    {
        _logger.LogInformation("Criando grupo: {Nome}", nome);

        if (await GrupoExisteAsync(nome))
        {
            _logger.LogWarning("Grupo já existe: {Nome}", nome);
            return false;
        }

        var grupo = new GrupoUsuario
        {
            Nome = nome.ToUpper(),
            Ativo = true,
            DataCriacao = DateTime.Now
        };

        _context.GruposUsuarios.Add(grupo);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Grupo criado com sucesso: {Nome}", nome);
        return true;
    }

    public async Task<bool> DeleteGrupoAsync(string nome)
    {
        _logger.LogInformation("Excluindo grupo: {Nome}", nome);

        // Não permite excluir grupo administrador
        if (AdminGroupHelper.IsAdminGroup(nome))
        {
            _logger.LogWarning("Tentativa de excluir grupo administrador negada");
            return false;
        }

        var grupo = await _context.GruposUsuarios
            .Include(g => g.Usuarios)
            .FirstOrDefaultAsync(g => g.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

        if (grupo == null)
        {
            _logger.LogWarning("Grupo não encontrado: {Nome}", nome);
            return false;
        }

        // Não permite excluir grupo com usuários
        if (grupo.Usuarios.Any())
        {
            _logger.LogWarning("Grupo possui usuários e não pode ser excluído: {Nome}", nome);
            return false;
        }

        // Não permite excluir grupo de sistema
        if (grupo.GrupoSistema)
        {
            _logger.LogWarning("Grupo de sistema não pode ser excluído: {Nome}", nome);
            return false;
        }

        // Remove permissões do grupo (da tabela PermissoesTela)
        var permissoes = await _context.PermissoesTelas
            .Where(p => p.Grupo == grupo.Nome)
            .ToListAsync();

        _context.PermissoesTelas.RemoveRange(permissoes);
        
        // Marca como inativo ao invés de excluir (soft delete)
        grupo.Ativo = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Grupo excluído com sucesso: {Nome}", nome);
        return true;
    }

    #endregion

    #region Usuários

    public async Task<List<UsuarioListDto>> GetAllUsuariosAsync()
    {
        _logger.LogInformation("Listando todos os usuários");

        var usuarios = await _context.PwUsuarios
            .Include(u => u.GrupoUsuarioNavigation)
            .ToListAsync();

        return usuarios.Select(u => new UsuarioListDto
        {
            Nome = DecriptaNome(u.PwNome),
            Grupo = u.GrupoUsuarioNavigation?.Nome ?? "SEM GRUPO",
            Email = u.PwEmail,
            Observacoes = string.IsNullOrEmpty(u.PwObs) ? null : DecriptaNome(u.PwObs),
            IsAdmin = u.GrupoUsuarioNavigation != null && AdminGroupHelper.IsAdminGroup(u.GrupoUsuarioNavigation.Nome),
            Ativo = u.PwAtivo
        }).OrderBy(u => u.Grupo).ThenBy(u => u.Nome).ToList();
    }

    public async Task<List<UsuarioListDto>> GetUsuariosByGrupoAsync(string grupo)
    {
        _logger.LogInformation("Listando usuários do grupo: {Grupo}", grupo);

        var usuarios = await _context.PwUsuarios
            .Include(u => u.GrupoUsuarioNavigation)
            .ToListAsync();

        return usuarios
            .Where(u => u.GrupoUsuarioNavigation != null && 
                        u.GrupoUsuarioNavigation.Nome.Equals(grupo, StringComparison.OrdinalIgnoreCase))
            .Select(u => new UsuarioListDto
            {
                Nome = DecriptaNome(u.PwNome),
                Grupo = u.GrupoUsuarioNavigation!.Nome,
                Email = u.PwEmail,
                Observacoes = string.IsNullOrEmpty(u.PwObs) ? null : DecriptaNome(u.PwObs),
                IsAdmin = AdminGroupHelper.IsAdminGroup(u.GrupoUsuarioNavigation.Nome),
                Ativo = u.PwAtivo
            }).OrderBy(u => u.Nome).ToList();
    }

    public async Task<UsuarioListDto?> GetUsuarioByNomeAsync(string nome)
    {
        _logger.LogInformation("Buscando usuário por nome: {Nome}", nome);

        var usuarios = await _context.PwUsuarios
            .Include(u => u.GrupoUsuarioNavigation)
            .ToListAsync();

        var usuario = usuarios.FirstOrDefault(u =>
            DecriptaNome(u.PwNome).Equals(nome, StringComparison.OrdinalIgnoreCase));

        if (usuario == null) return null;

        return new UsuarioListDto
        {
            Nome = DecriptaNome(usuario.PwNome),
            Grupo = usuario.GrupoUsuarioNavigation?.Nome ?? "SEM GRUPO",
            Email = usuario.PwEmail,
            Observacoes = string.IsNullOrEmpty(usuario.PwObs) ? null : DecriptaNome(usuario.PwObs),
            IsAdmin = usuario.GrupoUsuarioNavigation != null && AdminGroupHelper.IsAdminGroup(usuario.GrupoUsuarioNavigation.Nome),
            Ativo = usuario.PwAtivo
        };
    }

    public async Task<bool> UsuarioExisteAsync(string nome)
    {
        var usuarios = await _context.PwUsuarios.ToListAsync();
        return usuarios.Any(u => DecriptaNome(u.PwNome).Equals(nome, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> CreateUsuarioAsync(UsuarioCreateDto dto)
    {
        _logger.LogInformation("Criando usuário: {Nome}", dto.Nome);

        if (await UsuarioExisteAsync(dto.Nome))
        {
            _logger.LogWarning("Usuário já existe: {Nome}", dto.Nome);
            return false;
        }

        if (!await GrupoExisteAsync(dto.Grupo))
        {
            _logger.LogWarning("Grupo não existe: {Grupo}", dto.Grupo);
            return false;
        }

        // Busca o ID do grupo na tabela GrupoUsuario (nova)
        var grupo = await _context.GruposUsuarios.FirstOrDefaultAsync(g => 
            g.Nome.ToUpper() == dto.Grupo.ToUpper());
        
        if (grupo == null)
        {
            _logger.LogWarning("Grupo não encontrado na tabela GrupoUsuario: {Grupo}", dto.Grupo);
            return false;
        }

        // IMPORTANTE: Para satisfazer a FK da tabela legada PW~Usuarios -> PW~Grupos,
        // sempre usamos o grupo fixo "SEM GRUPO" na tabela legada.
        // O grupo real do usuário é gerenciado pela tabela nova GrupoUsuario via GrupoUsuarioId.
        const string GRUPO_LEGADO_PADRAO = "SEM GRUPO";
        
        // Verifica se o grupo padrão existe na tabela legada, se não existir, cria
        var grupoLegadoExiste = await _context.PwGrupos.AnyAsync(g => g.PwNome.ToUpper() == GRUPO_LEGADO_PADRAO);
        
        if (!grupoLegadoExiste)
        {
            _logger.LogInformation("Criando grupo padrão na tabela legada: {Grupo}", GRUPO_LEGADO_PADRAO);
            _context.PwGrupos.Add(new Models.PwGrupo { PwNome = GRUPO_LEGADO_PADRAO });
            await _context.SaveChangesAsync();
        }

        var usuario = new PwUsuario
        {
            PwNome = VB6CryptoService.Encripta(dto.Nome.ToUpper()),
            PwSenha = VB6CryptoService.Encripta(dto.Senha),
            PwGrupo = GRUPO_LEGADO_PADRAO, // Sempre usa "SEM GRUPO" para satisfazer a FK legada
            PwObs = string.IsNullOrEmpty(dto.Observacoes) ? null : VB6CryptoService.Encripta(dto.Observacoes),
            PwEmail = dto.Email,
            PwAtivo = dto.Ativo,
            GrupoUsuarioId = grupo.Id // Usa o ID do grupo da tabela nova
        };

        _context.PwUsuarios.Add(usuario);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Usuário criado com sucesso: {Nome}", dto.Nome);
        return true;
    }

    public async Task<bool> UpdateUsuarioAsync(string nomeAtual, UsuarioUpdateDto dto)
    {
        _logger.LogInformation("Atualizando usuário: {Nome}", nomeAtual);

        // Busca usuários sem rastreamento para evitar conflitos com a PK composta
        var usuarios = await _context.PwUsuarios.AsNoTracking().ToListAsync();
        var usuario = usuarios.FirstOrDefault(u =>
            DecriptaNome(u.PwNome).Equals(nomeAtual, StringComparison.OrdinalIgnoreCase));

        if (usuario == null)
        {
            _logger.LogWarning("Usuário não encontrado: {Nome}", nomeAtual);
            return false;
        }

        // Prepara os valores
        var obsCripto = string.IsNullOrEmpty(dto.Observacoes) ? null : VB6CryptoService.Encripta(dto.Observacoes);

        // Busca o ID do grupo na tabela GrupoUsuario (nova), se informado
        int? grupoUsuarioId = usuario.GrupoUsuarioId; // mantém o atual se não informado
        
        // Só processa o grupo se for informado e diferente de "SEM GRUPO"
        if (!string.IsNullOrWhiteSpace(dto.Grupo) && 
            !dto.Grupo.Equals("SEM GRUPO", StringComparison.OrdinalIgnoreCase))
        {
            var grupo = await _context.GruposUsuarios.FirstOrDefaultAsync(g => 
                g.Nome.ToUpper() == dto.Grupo.ToUpper());
            if (grupo != null)
            {
                grupoUsuarioId = grupo.Id;
            }
            else
            {
                _logger.LogWarning("Grupo não encontrado na tabela GrupoUsuario: {Grupo}", dto.Grupo);
                // Não retorna false, apenas mantém o grupo atual
            }
        }

        // Usa SQL raw para todas as atualizações devido à PK composta incluir PwSenha
        if (!string.IsNullOrEmpty(dto.NovaSenha))
        {
            var novaSenhaCripto = VB6CryptoService.Encripta(dto.NovaSenha);

            // Atualiza obs, ativo, GrupoUsuarioId, email e senha
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $@"UPDATE [PW~Usuarios] 
                  SET [PW~Obs] = {obsCripto}, [PW~Ativo] = {dto.Ativo}, [PW~Senha] = {novaSenhaCripto}, [GrupoUsuarioId] = {grupoUsuarioId}, [Email] = {dto.Email}
                  WHERE [PW~Nome] = {usuario.PwNome} AND [PW~Senha] = {usuario.PwSenha}");
        }
        else
        {
            // Atualiza obs, ativo, GrupoUsuarioId e email (sem senha)
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $@"UPDATE [PW~Usuarios] 
                  SET [PW~Obs] = {obsCripto}, [PW~Ativo] = {dto.Ativo}, [GrupoUsuarioId] = {grupoUsuarioId}, [Email] = {dto.Email}
                  WHERE [PW~Nome] = {usuario.PwNome} AND [PW~Senha] = {usuario.PwSenha}");
        }

        _logger.LogInformation("Usuário atualizado com sucesso: {Nome}", nomeAtual);
        return true;
    }

    public async Task<bool> DeleteUsuarioAsync(string nome)
    {
        _logger.LogInformation("Excluindo usuário: {Nome}", nome);

        var usuarios = await _context.PwUsuarios
            .Include(u => u.GrupoUsuarioNavigation)
            .ToListAsync();
        var usuario = usuarios.FirstOrDefault(u =>
            DecriptaNome(u.PwNome).Equals(nome, StringComparison.OrdinalIgnoreCase));

        if (usuario == null)
        {
            _logger.LogWarning("Usuário não encontrado: {Nome}", nome);
            return false;
        }

        // Verifica se é o último admin
        var grupoNome = usuario.GrupoUsuarioNavigation?.Nome ?? "";
        if (AdminGroupHelper.IsAdminGroup(grupoNome))
        {
            var adminsCount = usuarios.Count(u =>
                u.GrupoUsuarioNavigation != null && AdminGroupHelper.IsAdminGroup(u.GrupoUsuarioNavigation.Nome));

            if (adminsCount <= 1)
            {
                _logger.LogWarning("Não é possível excluir o último administrador");
                return false;
            }
        }

        _context.PwUsuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Usuário excluído com sucesso: {Nome}", nome);
        return true;
    }

    public async Task<bool> MoverUsuarioParaGrupoAsync(string nomeUsuario, string grupoOrigem, string grupoDestino)
    {
        _logger.LogInformation("Movendo usuário {Usuario} de {Origem} para {Destino}", nomeUsuario, grupoOrigem, grupoDestino);

        // Valida grupo destino
        if (!await GrupoExisteAsync(grupoDestino))
        {
            _logger.LogWarning("Grupo destino não existe: {Grupo}", grupoDestino);
            return false;
        }

        // Busca usuário
        var usuarios = await _context.PwUsuarios
            .Include(u => u.GrupoUsuarioNavigation)
            .ToListAsync();
        var usuario = usuarios.FirstOrDefault(u =>
            DecriptaNome(u.PwNome).Equals(nomeUsuario, StringComparison.OrdinalIgnoreCase));

        if (usuario == null)
        {
            _logger.LogWarning("Usuário não encontrado: {Nome}", nomeUsuario);
            return false;
        }

        var grupoAtual = usuario.GrupoUsuarioNavigation?.Nome ?? "";
        if (!grupoOrigem.Equals(grupoAtual, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Grupo de origem informado ({Origem}) difere do registrado ({Atual}) para o usuário {Usuario}",
                grupoOrigem, grupoAtual, nomeUsuario);
        }

        // Verifica se está tirando o último admin do grupo atual
        if (AdminGroupHelper.IsAdminGroup(grupoAtual) &&
            !AdminGroupHelper.IsAdminGroup(grupoDestino))
        {
            var adminsCount = usuarios.Count(u =>
                u.GrupoUsuarioNavigation != null && AdminGroupHelper.IsAdminGroup(u.GrupoUsuarioNavigation.Nome));

            if (adminsCount <= 1)
            {
                _logger.LogWarning("Não é possível remover o último administrador do grupo administrador");
                return false;
            }
        }

        // Busca o ID do grupo destino na nova tabela GrupoUsuario
        var grupoDestinoEntity = await _context.GruposUsuarios
            .FirstOrDefaultAsync(g => g.Nome.Equals(grupoDestino, StringComparison.OrdinalIgnoreCase));

        if (grupoDestinoEntity == null)
        {
            _logger.LogWarning("Grupo destino não encontrado na tabela GrupoUsuario: {Grupo}", grupoDestino);
            return false;
        }

        usuario.GrupoUsuarioId = grupoDestinoEntity.Id;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Usuário movido com sucesso");
        return true;
    }

    #endregion

    #region Permissões

    public async Task<PermissoesGrupoDto> GetPermissoesGrupoAsync(string grupo)
    {
        _logger.LogInformation("Buscando permissões do grupo: {Grupo}", grupo);

        var resultado = new PermissoesGrupoDto
        {
            Grupo = grupo,
            IsAdmin = AdminGroupHelper.IsAdminGroup(grupo),
            Tabelas = new List<PermissaoTabelaDto>(),
            Menus = new List<PermissaoTabelaDto>()
        };

        // Administrador tem acesso total
        if (resultado.IsAdmin)
        {
            return resultado;
        }

        // Busca grupo na nova tabela GrupoUsuario
        var grupoEntity = await _context.GruposUsuarios
            .FirstOrDefaultAsync(g => g.Nome.Equals(grupo, StringComparison.OrdinalIgnoreCase));

        if (grupoEntity == null)
        {
            return resultado;
        }

        // Busca permissões do grupo na tabela PermissoesTela (nova tabela)
        var permissoes = await _context.PermissoesTelas
            .Where(p => p.Grupo == grupoEntity.Nome)
            .ToListAsync();

        foreach (var p in permissoes)
        {
            var dto = new PermissaoTabelaDto
            {
                Projeto = p.Modulo,
                Nome = p.Tela,
                NomeExibicao = p.NomeTela,
                Visualiza = p.Consultar,
                Inclui = p.Incluir,
                Modifica = p.Alterar,
                Exclui = p.Excluir,
                Tipo = "TABELA"
            };

            resultado.Tabelas.Add(dto);
        }

        return resultado;
    }

    public async Task<bool> SavePermissaoTabelaAsync(string grupo, PermissaoTabelaDto permissao)
    {
        _logger.LogInformation("Salvando permissão: {Grupo} - {Tabela}", grupo, permissao.Nome);

        // Administrador não tem permissões gravadas (acesso total implícito)
        if (AdminGroupHelper.IsAdminGroup(grupo))
        {
            return true;
        }

        // Busca grupo na nova tabela GrupoUsuario
        var grupoEntity = await _context.GruposUsuarios
            .FirstOrDefaultAsync(g => g.Nome.Equals(grupo, StringComparison.OrdinalIgnoreCase));

        if (grupoEntity == null)
        {
            _logger.LogWarning("Grupo não encontrado: {Grupo}", grupo);
            return false;
        }

        // Se todas as permissões estão zeradas, remove o registro
        if (!permissao.Visualiza && !permissao.Inclui && !permissao.Modifica && !permissao.Exclui)
        {
            return await DeletePermissaoAsync(grupo, permissao.Nome);
        }

        // Busca permissão existente na tabela PermissoesTela
        var permissaoExistente = await _context.PermissoesTelas
            .FirstOrDefaultAsync(p =>
                p.Grupo == grupoEntity.Nome &&
                p.Tela == permissao.Nome);

        if (permissaoExistente != null)
        {
            // Atualiza
            permissaoExistente.Consultar = permissao.Visualiza;
            permissaoExistente.Incluir = permissao.Inclui;
            permissaoExistente.Alterar = permissao.Modifica;
            permissaoExistente.Excluir = permissao.Exclui;
        }
        else
        {
            // Cria nova permissão
            var novaPermissao = new PermissoesTela
            {
                Grupo = grupoEntity.Nome,
                Modulo = permissao.Projeto ?? "Sistema",
                Tela = permissao.Nome,
                NomeTela = permissao.NomeExibicao ?? permissao.Nome,
                Rota = $"/{permissao.Nome.ToLower()}",
                Consultar = permissao.Visualiza,
                Incluir = permissao.Inclui,
                Alterar = permissao.Modifica,
                Excluir = permissao.Exclui,
                Ordem = 0
            };
            _context.PermissoesTelas.Add(novaPermissao);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Permissão salva com sucesso");
        return true;
    }

    public async Task<bool> SavePermissoesEmLoteAsync(string grupo, List<PermissaoTabelaDto> permissoes)
    {
        _logger.LogInformation("Salvando {Count} permissões para o grupo: {Grupo}", permissoes.Count, grupo);

        foreach (var permissao in permissoes)
        {
            await SavePermissaoTabelaAsync(grupo, permissao);
        }

        return true;
    }

    public async Task<bool> CopiarPermissoesAsync(string grupoOrigem, string grupoDestino)
    {
        _logger.LogInformation("Copiando permissões de {Origem} para {Destino}", grupoOrigem, grupoDestino);

        // Não pode copiar de/para grupo administrador
        if (AdminGroupHelper.IsAdminGroup(grupoOrigem) ||
            AdminGroupHelper.IsAdminGroup(grupoDestino))
        {
            _logger.LogWarning("Não é possível copiar permissões de/para grupo administrador");
            return false;
        }

        var permissoesOrigem = await GetPermissoesGrupoAsync(grupoOrigem);

        // Remove permissões atuais do destino
        var grupoDestinoEntity = await _context.GruposUsuarios
            .FirstOrDefaultAsync(g => g.Nome.Equals(grupoDestino, StringComparison.OrdinalIgnoreCase));

        if (grupoDestinoEntity != null)
        {
            var permissoesDestino = await _context.PermissoesTelas
                .Where(p => p.Grupo == grupoDestinoEntity.Nome)
                .ToListAsync();

            _context.PermissoesTelas.RemoveRange(permissoesDestino);
            await _context.SaveChangesAsync();
        }

        // Copia permissões
        var todasPermissoes = permissoesOrigem.Tabelas.Concat(permissoesOrigem.Menus).ToList();
        await SavePermissoesEmLoteAsync(grupoDestino, todasPermissoes);

        _logger.LogInformation("Permissões copiadas com sucesso");
        return true;
    }

    public async Task<bool> DeletePermissaoAsync(string grupo, string nomeTabela)
    {
        _logger.LogInformation("Excluindo permissão: {Grupo} - {Tabela}", grupo, nomeTabela);

        var grupoEntity = await _context.GruposUsuarios
            .FirstOrDefaultAsync(g => g.Nome.Equals(grupo, StringComparison.OrdinalIgnoreCase));

        if (grupoEntity == null)
        {
            return false;
        }

        var permissao = await _context.PermissoesTelas
            .FirstOrDefaultAsync(p => 
                p.Grupo == grupoEntity.Nome && 
                p.Tela.Equals(nomeTabela, StringComparison.OrdinalIgnoreCase));

        if (permissao != null)
        {
            _context.PermissoesTelas.Remove(permissao);
            await _context.SaveChangesAsync();
        }

        return true;
    }

    #endregion

    #region Estrutura Hierárquica

    public async Task<List<GrupoComUsuariosDto>> GetArvoreGruposUsuariosAsync()
    {
        _logger.LogInformation("Buscando árvore de grupos e usuários");

        // Busca da tabela GrupoUsuario (nova)
        var grupos = await _context.GruposUsuarios
            .Include(g => g.Usuarios)
            .Where(g => g.Ativo)
            .OrderBy(g => g.Nome)
            .ToListAsync();

        return grupos.Select(g => new GrupoComUsuariosDto
        {
            Nome = g.Nome,
            IsAdmin = AdminGroupHelper.IsAdminGroup(g.Nome),
            Expandido = true,
            Usuarios = g.Usuarios.Select(u => new UsuarioListDto
            {
                Nome = DecriptaNome(u.PwNome),
                Grupo = g.Nome,
                Email = u.PwEmail,
                Observacoes = string.IsNullOrEmpty(u.PwObs) ? null : DecriptaNome(u.PwObs),
                IsAdmin = AdminGroupHelper.IsAdminGroup(g.Nome),
                Ativo = u.PwAtivo
            }).OrderBy(u => u.Nome).ToList()
        }).ToList();
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Descriptografa um nome usando VB6CryptoService e remove caracteres de padding.
    /// </summary>
    private static string DecriptaNome(string? valorCriptografado)
    {
        if (string.IsNullOrEmpty(valorCriptografado))
            return string.Empty;

        try
        {
            var decriptado = VB6CryptoService.Decripta(valorCriptografado);
            return decriptado.TrimEnd('+').Trim();
        }
        catch
        {
            return valorCriptografado;
        }
    }

    #endregion
}
