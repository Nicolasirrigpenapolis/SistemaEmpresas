using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers;

/// <summary>
/// Controller para gerenciamento de usuários, grupos e permissões
/// Compatível com banco legado VB6
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioManagementService _service;
    private readonly ILogger<UsuariosController> _logger;
    private readonly ICacheService _cache;

    public UsuariosController(
        IUsuarioManagementService service,
        ILogger<UsuariosController> logger,
        ICacheService cache)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    #region Verificação de Acesso

    /// <summary>
    /// Verifica se o usuário atual é administrador (PROGRAMADOR)
    /// </summary>
    private bool IsAdmin()
    {
        var grupo = User.FindFirst(ClaimTypes.Role)?.Value;
        return AdminGroupHelper.IsAdminGroup(grupo);
    }

    /// <summary>
    /// Retorna Forbid se não for admin
    /// </summary>
    private ActionResult? VerificarPermissaoAdmin()
    {
        if (!IsAdmin())
        {
            _logger.LogWarning("Acesso negado: usuário não é administrador");
            return Forbid();
        }
        return null;
    }

    #endregion

    #region Grupos

    /// <summary>
    /// Lista todos os grupos
    /// </summary>
    [HttpGet("grupos")]
    public async Task<ActionResult<List<GrupoListDto>>> ListarGrupos()
    {
        try
        {
            var grupos = await _cache.GetOrCreateAsync(
                "usuarios:grupos:all",
                async () => await _service.ListarGruposAsync(),
                CacheService.CacheDurations.Medium,
                CacheService.CacheDurations.Short);
            return Ok(grupos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar grupos");
            return StatusCode(500, new { message = "Erro ao listar grupos" });
        }
    }

    /// <summary>
    /// Cria um novo grupo
    /// </summary>
    [HttpPost("grupos")]
    public async Task<ActionResult<OperacaoResultDto>> CriarGrupo([FromBody] GrupoCreateDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.CriarGrupoAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar grupo");
            return StatusCode(500, new { message = "Erro ao criar grupo" });
        }
    }

    /// <summary>
    /// Exclui um grupo
    /// </summary>
    [HttpDelete("grupos/{nome}")]
    public async Task<ActionResult<OperacaoResultDto>> ExcluirGrupo(string nome)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.ExcluirGrupoAsync(nome);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir grupo: {Nome}", nome);
            return StatusCode(500, new { message = "Erro ao excluir grupo" });
        }
    }

    #endregion

    #region Usuários

    /// <summary>
    /// Lista todos os grupos com seus usuários (árvore)
    /// </summary>
    [HttpGet("arvore")]
    public async Task<ActionResult<List<GrupoComUsuariosDto>>> ListarArvore()
    {
        try
        {
            var arvore = await _service.ListarArvoreUsuariosAsync();
            return Ok(arvore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar árvore de usuários");
            return StatusCode(500, new { message = "Erro ao listar usuários" });
        }
    }

    /// <summary>
    /// Obtém dados de um usuário específico
    /// </summary>
    [HttpGet("{nome}")]
    public async Task<ActionResult<UsuarioListDto>> ObterUsuario(string nome)
    {
        try
        {
            var usuario = await _service.ObterUsuarioAsync(nome);

            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário: {Nome}", nome);
            return StatusCode(500, new { message = "Erro ao obter usuário" });
        }
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OperacaoResultDto>> CriarUsuario([FromBody] UsuarioCreateDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.CriarUsuarioAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return StatusCode(500, new { message = "Erro ao criar usuário" });
        }
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    [HttpPut("{nome}")]
    public async Task<ActionResult<OperacaoResultDto>> AtualizarUsuario(string nome, [FromBody] UsuarioUpdateDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.AtualizarUsuarioAsync(nome, dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Nome}", nome);
            return StatusCode(500, new { message = "Erro ao atualizar usuário" });
        }
    }

    /// <summary>
    /// Exclui um usuário
    /// </summary>
    [HttpDelete("{nome}")]
    public async Task<ActionResult<OperacaoResultDto>> ExcluirUsuario(string nome)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.ExcluirUsuarioAsync(nome);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir usuário: {Nome}", nome);
            return StatusCode(500, new { message = "Erro ao excluir usuário" });
        }
    }

    /// <summary>
    /// Move um usuário para outro grupo
    /// </summary>
    [HttpPost("mover")]
    public async Task<ActionResult<OperacaoResultDto>> MoverUsuario([FromBody] MoverUsuarioDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.MoverUsuarioAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao mover usuário");
            return StatusCode(500, new { message = "Erro ao mover usuário" });
        }
    }

    #endregion

    #region Permissões

    /// <summary>
    /// Obtém permissões de um grupo
    /// </summary>
    [HttpGet("permissoes/{grupo}")]
    public async Task<ActionResult<PermissoesGrupoDto>> ObterPermissoesGrupo(string grupo)
    {
        try
        {
            var permissoes = await _service.ObterPermissoesGrupoAsync(grupo);
            return Ok(permissoes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter permissões do grupo: {Grupo}", grupo);
            return StatusCode(500, new { message = "Erro ao obter permissões" });
        }
    }

    /// <summary>
    /// Atualiza permissões de um grupo
    /// </summary>
    [HttpPut("permissoes")]
    public async Task<ActionResult<OperacaoResultDto>> AtualizarPermissoes([FromBody] AtualizarPermissoesDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.AtualizarPermissoesAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar permissões do grupo: {Grupo}", dto.Grupo);
            return StatusCode(500, new { message = "Erro ao atualizar permissões" });
        }
    }

    /// <summary>
    /// Copia permissões de um grupo para outro
    /// </summary>
    [HttpPost("permissoes/copiar")]
    public async Task<ActionResult<OperacaoResultDto>> CopiarPermissoes([FromBody] CopiarPermissoesDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.CopiarPermissoesAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao copiar permissões");
            return StatusCode(500, new { message = "Erro ao copiar permissões" });
        }
    }

    /// <summary>
    /// Lista tabelas disponíveis para permissões (agrupadas por módulo)
    /// </summary>
    [HttpGet("tabelas-disponiveis")]
    public async Task<ActionResult<List<ModuloTabelasDto>>> ListarTabelasDisponiveis()
    {
        try
        {
            var tabelas = await _service.ListarTabelasDisponiveisAsync();
            return Ok(tabelas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tabelas disponíveis");
            return StatusCode(500, new { message = "Erro ao listar tabelas" });
        }
    }

    /// <summary>
    /// Lista menus disponíveis para permissões
    /// </summary>
    [HttpGet("menus-disponiveis")]
    public async Task<ActionResult<List<PermissaoTabelaDto>>> ListarMenusDisponiveis()
    {
        try
        {
            var menus = await _service.ListarMenusDisponiveisAsync();
            return Ok(menus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar menus disponíveis");
            return StatusCode(500, new { message = "Erro ao listar menus" });
        }
    }

    #endregion
}
