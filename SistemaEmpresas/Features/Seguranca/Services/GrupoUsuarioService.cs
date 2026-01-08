using Microsoft.Extensions.Logging;
using SistemaEmpresas.Features.Seguranca.Dtos;
using SistemaEmpresas.Features.Seguranca.Repositories;
using SistemaEmpresas.Models;
using SistemaEmpresas.Features.Auth.Services;

namespace SistemaEmpresas.Features.Seguranca.Services;

public interface IGrupoUsuarioService
{
    Task<List<GrupoUsuarioListDto>> ListarGruposAsync();
    Task<GrupoUsuarioListDto?> ObterGrupoAsync(int id);
    Task<OperacaoResultDto> CriarGrupoAsync(GrupoUsuarioCreateDto dto);
    Task<OperacaoResultDto> AtualizarGrupoAsync(int id, GrupoUsuarioUpdateDto dto);
    Task<OperacaoResultDto> ExcluirGrupoAsync(int id);
    Task<List<UsuarioListDto>> ListarUsuariosAsync();
    Task<OperacaoResultDto> VincularUsuarioAoGrupoAsync(string nomeUsuario, int grupoId);
    Task<List<GrupoComUsuariosDto>> ListarArvoreAsync();
}

public class GrupoUsuarioService : IGrupoUsuarioService
{
    private readonly IGrupoUsuarioRepository _repository;
    private readonly ILogger<GrupoUsuarioService> _logger;
    private const string GRUPO_SEM_GRUPO = "SEM GRUPO";

    public GrupoUsuarioService(IGrupoUsuarioRepository repository, ILogger<GrupoUsuarioService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<GrupoUsuarioListDto>> ListarGruposAsync()
    {
        var grupos = await _repository.GetAllGruposAsync();
        var result = new List<GrupoUsuarioListDto>();
        foreach (var g in grupos)
        {
            var qtd = await _repository.ContarUsuariosDoGrupoAsync(g.Id);
            result.Add(new GrupoUsuarioListDto
            {
                Id = g.Id,
                Nome = g.Nome,
                Descricao = g.Descricao,
                Ativo = g.Ativo,
                GrupoSistema = g.GrupoSistema,
                QuantidadeUsuarios = qtd,
                DataCriacao = g.DataCriacao
            });
        }
        return result;
    }

    public async Task<GrupoUsuarioListDto?> ObterGrupoAsync(int id)
    {
        var grupo = await _repository.GetGrupoByIdAsync(id);
        if (grupo == null) return null;
        var qtd = await _repository.ContarUsuariosDoGrupoAsync(id);
        return new GrupoUsuarioListDto
        {
            Id = grupo.Id,
            Nome = grupo.Nome,
            Descricao = grupo.Descricao,
            Ativo = grupo.Ativo,
            GrupoSistema = grupo.GrupoSistema,
            QuantidadeUsuarios = qtd,
            DataCriacao = grupo.DataCriacao
        };
    }

    public async Task<OperacaoResultDto> CriarGrupoAsync(GrupoUsuarioCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            return new OperacaoResultDto { Sucesso = false, Mensagem = "Nome do grupo e obrigatorio" };
        }
        if (await _repository.GrupoExisteAsync(dto.Nome.Trim()))
        {
            return new OperacaoResultDto { Sucesso = false, Mensagem = "Ja existe um grupo com este nome" };
        }
        var grupo = new GrupoUsuario
        {
            Nome = dto.Nome.Trim().ToUpper(),
            Descricao = dto.Descricao?.Trim(),
            Ativo = dto.Ativo
        };
        await _repository.CreateGrupoAsync(grupo);
        _logger.LogInformation("Grupo criado: {Nome}", grupo.Nome);
        return new OperacaoResultDto { Sucesso = true, Mensagem = "Grupo criado com sucesso", Id = grupo.Id };
    }

    public async Task<OperacaoResultDto> AtualizarGrupoAsync(int id, GrupoUsuarioUpdateDto dto)
    {
        var grupo = await _repository.GetGrupoByIdAsync(id);
        if (grupo == null)
        {
            return new OperacaoResultDto { Sucesso = false, Mensagem = "Grupo nao encontrado" };
        }
        if (!string.IsNullOrWhiteSpace(dto.Nome))
        {
            var nomeNormalizado = dto.Nome.Trim().ToUpper();
            if (nomeNormalizado != grupo.Nome)
            {
                var existe = await _repository.GrupoExisteAsync(nomeNormalizado);
                if (existe)
                {
                    return new OperacaoResultDto { Sucesso = false, Mensagem = "Ja existe um grupo com este nome" };
                }
                grupo.Nome = nomeNormalizado;
            }
        }
        if (dto.Descricao != null) grupo.Descricao = dto.Descricao.Trim();
        if (dto.Ativo.HasValue) grupo.Ativo = dto.Ativo.Value;
        await _repository.UpdateGrupoAsync(grupo);
        _logger.LogInformation("Grupo atualizado: {Nome}", grupo.Nome);
        return new OperacaoResultDto { Sucesso = true, Mensagem = "Grupo atualizado com sucesso" };
    }

    public async Task<OperacaoResultDto> ExcluirGrupoAsync(int id)
    {
        var grupo = await _repository.GetGrupoByIdAsync(id);
        if (grupo == null)
        {
            return new OperacaoResultDto { Sucesso = false, Mensagem = "Grupo nao encontrado" };
        }
        if (grupo.GrupoSistema)
        {
            return new OperacaoResultDto { Sucesso = false, Mensagem = "Nao e possivel excluir um grupo de sistema" };
        }
        var qtdUsuarios = await _repository.ContarUsuariosDoGrupoAsync(id);
        if (qtdUsuarios > 0)
        {
            var grupoSemGrupo = await _repository.GetGrupoByNomeAsync(GRUPO_SEM_GRUPO);
            if (grupoSemGrupo == null)
            {
                grupoSemGrupo = await _repository.CreateGrupoAsync(new GrupoUsuario
                {
                    Nome = GRUPO_SEM_GRUPO,
                    Descricao = "Usuarios sem grupo definido",
                    Ativo = true,
                    GrupoSistema = true
                });
            }
            await _repository.MoverUsuariosDoGrupoAsync(id, grupoSemGrupo.Id);
            _logger.LogInformation("Movidos {Qtd} usuarios para o grupo SEM GRUPO", qtdUsuarios);
        }
        await _repository.DeleteGrupoAsync(id);
        _logger.LogInformation("Grupo excluido: {Nome}", grupo.Nome);
        return new OperacaoResultDto { Sucesso = true, Mensagem = "Grupo excluido com sucesso" };
    }

    public async Task<List<UsuarioListDto>> ListarUsuariosAsync()
    {
        var usuarios = await _repository.GetAllUsuariosAsync();
        var result = new List<UsuarioListDto>();
        foreach (var u in usuarios)
        {
            var nomeDecriptado = VB6CryptoService.Decripta(u.PwNome).TrimEnd('+');
            result.Add(new UsuarioListDto
            {
                Nome = nomeDecriptado,
                Grupo = u.GrupoUsuarioNavigation?.Nome ?? "SEM GRUPO",
                Observacoes = u.PwObs,
                Ativo = u.PwAtivo,
                GrupoUsuarioId = u.GrupoUsuarioId
            });
        }
        return result;
    }

    public async Task<OperacaoResultDto> VincularUsuarioAoGrupoAsync(string nomeUsuario, int grupoId)
    {
        var grupo = await _repository.GetGrupoByIdAsync(grupoId);
        if (grupo == null)
        {
            return new OperacaoResultDto { Sucesso = false, Mensagem = "Grupo nao encontrado" };
        }
        var usuarios = await _repository.GetAllUsuariosAsync();
        foreach (var u in usuarios)
        {
            var nomeDecriptado = VB6CryptoService.Decripta(u.PwNome).TrimEnd('+');
            if (string.Equals(nomeDecriptado, nomeUsuario, StringComparison.OrdinalIgnoreCase))
            {
                await _repository.VincularUsuarioAoGrupoAsync(u.PwNome, u.PwSenha, grupoId);
                _logger.LogInformation("Usuario {Nome} vinculado ao grupo {Grupo}", nomeUsuario, grupo.Nome);
                return new OperacaoResultDto { Sucesso = true, Mensagem = "Usuario vinculado com sucesso" };
            }
        }
        return new OperacaoResultDto { Sucesso = false, Mensagem = "Usuario nao encontrado" };
    }

    public async Task<List<GrupoComUsuariosDto>> ListarArvoreAsync()
    {
        // Lista todos os grupos
        var grupos = await _repository.GetAllGruposAsync();
        var usuarios = await _repository.GetAllUsuariosAsync();

        var result = new List<GrupoComUsuariosDto>();

        foreach (var grupo in grupos)
        {
            var usuariosDoGrupo = usuarios
                .Where(u => u.GrupoUsuarioId == grupo.Id)
                .Select(u => new UsuarioListDto
                {
                    Nome = VB6CryptoService.Decripta(u.PwNome).TrimEnd('+'),
                    Grupo = grupo.Nome,
                    Observacoes = DecriptarObservacoes(u.PwObs),
                    Ativo = u.PwAtivo,
                    GrupoUsuarioId = grupo.Id
                })
                .ToList();

            result.Add(new GrupoComUsuariosDto
            {
                Nome = grupo.Nome,
                IsAdmin = grupo.GrupoSistema || AdminGroupHelper.IsAdminGroup(grupo.Nome),
                Expandido = true,
                Usuarios = usuariosDoGrupo
            });
        }

        // Adicionar grupo virtual "SEM GRUPO" para usuarios sem GrupoUsuarioId
        var usuariosSemGrupo = usuarios
            .Where(u => u.GrupoUsuarioId == null)
            .Select(u => new UsuarioListDto
            {
                Nome = VB6CryptoService.Decripta(u.PwNome).TrimEnd('+'),
                Grupo = "SEM GRUPO",
                Observacoes = DecriptarObservacoes(u.PwObs),
                Ativo = u.PwAtivo,
                GrupoUsuarioId = null
            })
            .ToList();

        if (usuariosSemGrupo.Any())
        {
            result.Add(new GrupoComUsuariosDto
            {
                Nome = "SEM GRUPO",
                IsAdmin = false,
                Expandido = true,
                Usuarios = usuariosSemGrupo
            });
        }

        return result.OrderBy(g => g.Nome).ToList();
    }

    /// <summary>
    /// Decripta observações de forma segura, retornando texto padrão em caso de erro
    /// </summary>
    private static string DecriptarObservacoes(string? obs)
    {
        if (string.IsNullOrEmpty(obs))
            return "Sem observações";

        try
        {
            var decriptado = VB6CryptoService.Decripta(obs).TrimEnd('+').Trim();
            return string.IsNullOrEmpty(decriptado) ? "Sem observações" : decriptado;
        }
        catch
        {
            // Se falhar na decriptação, retorna o valor original (pode ser texto não criptografado)
            return obs;
        }
    }
}
