using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services.Common;
using SistemaEmpresas.Services.Seguranca;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers.Seguranca;

/// <summary>
/// Controller para gerenciamento de permissões por tela
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissoesController : ControllerBase
{
    private readonly IPermissoesTelaService _service;
    private readonly ILogger<PermissoesController> _logger;
    private readonly ICacheService _cache;

    public PermissoesController(
        IPermissoesTelaService service,
        ILogger<PermissoesController> logger,
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

    /// <summary>
    /// Obtém o nome do usuário logado
    /// </summary>
    private string GetUsuarioLogado()
    {
        return User.FindFirst(ClaimTypes.Name)?.Value ?? "";
    }

    /// <summary>
    /// Obtém o grupo do usuário logado
    /// </summary>
    private string GetGrupoLogado()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? "";
    }

    #endregion

    #region Templates

    /// <summary>
    /// Lista todos os templates de permissões
    /// </summary>
    [HttpGet("templates")]
    public async Task<ActionResult<List<PermissoesTemplateListDto>>> ListarTemplates()
    {
        try
        {
            var templates = await _cache.GetOrCreateAsync(
                "permissoes:templates:all",
                async () => await _service.ListarTemplatesAsync(),
                CacheService.CacheDurations.Medium,
                CacheService.CacheDurations.Short);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar templates");
            return StatusCode(500, new { message = "Erro ao listar templates" });
        }
    }

    /// <summary>
    /// Obtém um template específico com seus detalhes
    /// </summary>
    [HttpGet("templates/{id}")]
    public async Task<ActionResult<PermissoesTemplateComDetalhesDto>> ObterTemplate(int id)
    {
        try
        {
            var template = await _service.ObterTemplateAsync(id);
            
            if (template == null)
                return NotFound(new { message = "Template não encontrado" });

            return Ok(template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter template {Id}", id);
            return StatusCode(500, new { message = "Erro ao obter template" });
        }
    }

    /// <summary>
    /// Cria um novo template
    /// </summary>
    [HttpPost("templates")]
    public async Task<ActionResult<OperacaoResultDto>> CriarTemplate([FromBody] PermissoesTemplateCreateDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.CriarTemplateAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar template");
            return StatusCode(500, new { message = "Erro ao criar template" });
        }
    }

    /// <summary>
    /// Atualiza um template existente
    /// </summary>
    [HttpPut("templates/{id}")]
    public async Task<ActionResult<OperacaoResultDto>> AtualizarTemplate(int id, [FromBody] PermissoesTemplateUpdateDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.AtualizarTemplateAsync(id, dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar template {Id}", id);
            return StatusCode(500, new { message = "Erro ao atualizar template" });
        }
    }

    /// <summary>
    /// Exclui um template
    /// </summary>
    [HttpDelete("templates/{id}")]
    public async Task<ActionResult<OperacaoResultDto>> ExcluirTemplate(int id)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.ExcluirTemplateAsync(id);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir template {Id}", id);
            return StatusCode(500, new { message = "Erro ao excluir template" });
        }
    }

    #endregion

    #region Permissões por Grupo

    /// <summary>
    /// Obtém as permissões completas de um grupo
    /// </summary>
    [HttpGet("grupo/{grupo}")]
    public async Task<ActionResult<PermissoesCompletasGrupoDto>> ObterPermissoesGrupo(string grupo)
    {
        try
        {
            var permissoes = await _service.ObterPermissoesGrupoAsync(grupo);
            return Ok(permissoes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter permissões do grupo {Grupo}", grupo);
            return StatusCode(500, new { message = "Erro ao obter permissões do grupo" });
        }
    }

    /// <summary>
    /// Salva as permissões de um grupo (em lote)
    /// </summary>
    [HttpPost("grupo")]
    public async Task<ActionResult<OperacaoResultDto>> SalvarPermissoes([FromBody] PermissoesTelasBatchUpdateDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.SalvarPermissoesAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar permissões do grupo {Grupo}", dto.Grupo);
            return StatusCode(500, new { message = "Erro ao salvar permissões" });
        }
    }

    /// <summary>
    /// Aplica um template a um grupo
    /// </summary>
    [HttpPost("aplicar-template")]
    public async Task<ActionResult<OperacaoResultDto>> AplicarTemplate([FromBody] AplicarTemplateDto dto)
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.AplicarTemplateAsync(dto);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao aplicar template {TemplateId} ao grupo {Grupo}", dto.TemplateId, dto.Grupo);
            return StatusCode(500, new { message = "Erro ao aplicar template" });
        }
    }

    #endregion

    #region Minhas Permissões (usuário logado)

    /// <summary>
    /// Obtém as permissões do usuário logado
    /// </summary>
    [HttpGet("minhas")]
    public async Task<ActionResult<PermissoesUsuarioLogadoDto>> ObterMinhasPermissoes()
    {
        try
        {
            var usuario = GetUsuarioLogado();
            var grupo = GetGrupoLogado();

            var permissoes = await _service.ObterMinhasPermissoesAsync(usuario, grupo);
            return Ok(permissoes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter permissões do usuário logado");
            return StatusCode(500, new { message = "Erro ao obter permissões" });
        }
    }

    /// <summary>
    /// Verifica se o usuário logado tem permissão em uma tela específica
    /// </summary>
    [HttpPost("verificar")]
    public async Task<ActionResult<PermissaoResultDto>> VerificarPermissao([FromBody] VerificarPermissaoDto dto)
    {
        try
        {
            var grupo = GetGrupoLogado();
            var resultado = await _service.VerificarPermissaoAsync(grupo, dto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar permissão");
            return StatusCode(500, new { message = "Erro ao verificar permissão" });
        }
    }

    #endregion

    #region Telas Disponíveis

    /// <summary>
    /// Lista todas as telas disponíveis no sistema (agrupadas por módulo)
    /// </summary>
    [HttpGet("telas")]
    public ActionResult<List<ModuloComTelasDto>> ListarTelasDisponiveis()
    {
        try
        {
            var telas = _service.ListarTelasDisponiveis();
            return Ok(telas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar telas disponíveis");
            return StatusCode(500, new { message = "Erro ao listar telas" });
        }
    }

    #endregion

    #region Estatísticas

    /// <summary>
    /// Obtém estatísticas de permissões
    /// </summary>
    [HttpGet("estatisticas")]
    public async Task<ActionResult<PermissoesEstatisticasDto>> ObterEstatisticas()
    {
        try
        {
            var stats = await _service.ObterEstatisticasAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas");
            return StatusCode(500, new { message = "Erro ao obter estatísticas" });
        }
    }

    #endregion

    #region Seeds

    /// <summary>
    /// Cria os templates padrão do sistema (apenas admin)
    /// </summary>
    [HttpPost("seeds/templates")]
    public async Task<ActionResult<OperacaoResultDto>> CriarTemplatesPadrao()
    {
        try
        {
            var permissao = VerificarPermissaoAdmin();
            if (permissao != null) return permissao;

            var resultado = await _service.CriarTemplatesPadraoAsync();

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar templates padrão");
            return StatusCode(500, new { message = "Erro ao criar templates padrão" });
        }
    }

    #endregion
}

