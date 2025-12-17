using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services.Seguranca;

namespace SistemaEmpresas.Controllers.Seguranca;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GrupoUsuarioController : ControllerBase
{
    private readonly IGrupoUsuarioService _service;
    private readonly ILogger<GrupoUsuarioController> _logger;

    public GrupoUsuarioController(IGrupoUsuarioService service, ILogger<GrupoUsuarioController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("grupos")]
    public async Task<ActionResult<List<GrupoUsuarioListDto>>> ListarGrupos()
    {
        try
        {
            var grupos = await _service.ListarGruposAsync();
            return Ok(grupos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar grupos");
            return StatusCode(500, new { mensagem = "Erro interno ao listar grupos" });
        }
    }

    [HttpGet("grupos/{id}")]
    public async Task<ActionResult<GrupoUsuarioListDto>> ObterGrupo(int id)
    {
        try
        {
            var grupo = await _service.ObterGrupoAsync(id);
            if (grupo == null)
                return NotFound(new { mensagem = "Grupo nao encontrado" });
            return Ok(grupo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter grupo {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao obter grupo" });
        }
    }

    [HttpPost("grupos")]
    public async Task<ActionResult<OperacaoResultDto>> CriarGrupo([FromBody] GrupoUsuarioCreateDto dto)
    {
        try
        {
            var result = await _service.CriarGrupoAsync(dto);
            if (!result.Sucesso)
                return BadRequest(result);
            return CreatedAtAction(nameof(ObterGrupo), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar grupo");
            return StatusCode(500, new { mensagem = "Erro interno ao criar grupo" });
        }
    }

    [HttpPut("grupos/{id}")]
    public async Task<ActionResult<OperacaoResultDto>> AtualizarGrupo(int id, [FromBody] GrupoUsuarioUpdateDto dto)
    {
        try
        {
            var result = await _service.AtualizarGrupoAsync(id, dto);
            if (!result.Sucesso)
                return BadRequest(result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar grupo {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar grupo" });
        }
    }

    [HttpDelete("grupos/{id}")]
    public async Task<ActionResult<OperacaoResultDto>> ExcluirGrupo(int id)
    {
        try
        {
            var result = await _service.ExcluirGrupoAsync(id);
            if (!result.Sucesso)
                return BadRequest(result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir grupo {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao excluir grupo" });
        }
    }

    [HttpGet("usuarios")]
    public async Task<ActionResult<List<UsuarioListDto>>> ListarUsuarios()
    {
        try
        {
            var usuarios = await _service.ListarUsuariosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuarios");
            return StatusCode(500, new { mensagem = "Erro interno ao listar usuarios" });
        }
    }

    [HttpPost("usuarios/{nomeUsuario}/vincular/{grupoId}")]
    public async Task<ActionResult<OperacaoResultDto>> VincularUsuarioAoGrupo(string nomeUsuario, int grupoId)
    {
        try
        {
            var result = await _service.VincularUsuarioAoGrupoAsync(nomeUsuario, grupoId);
            if (!result.Sucesso)
                return BadRequest(result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao vincular usuario {Usuario} ao grupo {GrupoId}", nomeUsuario, grupoId);
            return StatusCode(500, new { mensagem = "Erro interno ao vincular usuario" });
        }
    }

    [HttpGet("arvore")]
    public async Task<ActionResult<List<GrupoComUsuariosDto>>> ListarArvore()
    {
        try
        {
            var arvore = await _service.ListarArvoreAsync();
            return Ok(arvore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar arvore de grupos/usuarios");
            return StatusCode(500, new { mensagem = "Erro interno ao listar arvore" });
        }
    }
}
