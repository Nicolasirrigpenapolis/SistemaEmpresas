using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services;
using SistemaEmpresas.Services.Transporte;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers.Transporte;

[ApiController]
[Route("api/transporte/[controller]")]
[Authorize]
public class ReboquesController : ControllerBase
{
    private readonly IReboqueService _service;
    private readonly ILogger<ReboquesController> _logger;
    private readonly ILogAuditoriaService _logAuditoria;

    public ReboquesController(
        IReboqueService service, 
        ILogger<ReboquesController> logger,
        ILogAuditoriaService logAuditoria)
    {
        _service = service;
        _logger = logger;
        _logAuditoria = logAuditoria;
    }

    /// <summary>
    /// Lista todos os reboques com paginação e filtros
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<DTOs.PagedResult<ReboqueListDto>>> Listar([FromQuery] ReboqueFiltros filtros)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando reboques. Usuário: {Usuario}, Filtros: {@Filtros}", usuario, filtros);

            var resultado = await _service.ListarAsync(filtros);
            
            _logger.LogInformation("Reboques listados com sucesso. Total: {Total}, Usuário: {Usuario}", 
                resultado.TotalCount, usuario);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar reboques. Usuário: {Usuario}", 
                User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar reboques", erro = ex.Message, stack = ex.StackTrace });
        }
    }

    /// <summary>
    /// Obtém um reboque por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReboqueDto>> ObterPorId(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Obtendo reboque. ID: {Id}, Usuário: {Usuario}", id, usuario);

            var reboque = await _service.ObterPorIdAsync(id);
            if (reboque == null)
            {
                _logger.LogWarning("Reboque não encontrado. ID: {Id}, Usuário: {Usuario}", id, usuario);
                return NotFound(new { mensagem = "Reboque não encontrado" });
            }

            _logger.LogInformation("Reboque obtido com sucesso. ID: {Id}, Placa: {Placa}, Usuário: {Usuario}", 
                id, reboque.Placa, usuario);
            return Ok(reboque);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter reboque {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao obter reboque" });
        }
    }

    /// <summary>
    /// Obtém um reboque por placa
    /// </summary>
    [HttpGet("placa/{placa}")]
    public async Task<ActionResult<ReboqueDto>> ObterPorPlaca(string placa)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Obtendo reboque por placa. Placa: {Placa}, Usuário: {Usuario}", placa, usuario);

            var reboque = await _service.ObterPorPlacaAsync(placa);
            if (reboque == null)
            {
                _logger.LogWarning("Reboque não encontrado por placa. Placa: {Placa}, Usuário: {Usuario}", placa, usuario);
                return NotFound(new { mensagem = "Reboque não encontrado" });
            }

            _logger.LogInformation("Reboque obtido com sucesso. Placa: {Placa}, ID: {Id}, Usuário: {Usuario}", 
                placa, reboque.Id, usuario);
            return Ok(reboque);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter reboque por placa {Placa}. Usuário: {Usuario}", 
                placa, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao obter reboque" });
        }
    }

    /// <summary>
    /// Cria um novo reboque
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] ReboqueCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Criando novo reboque. Placa: {Placa}, Usuário: {Usuario}", dto.Placa, usuario);

            var (sucesso, mensagem, id) = await _service.CriarAsync(dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao criar reboque. Placa: {Placa}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    dto.Placa, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Reboque criado com sucesso. ID: {Id}, Placa: {Placa}, Usuário: {Usuario}", 
                id, dto.Placa, usuario);

            // Auditoria
            await _logAuditoria.LogCriacaoAsync(
                modulo: "Transporte",
                entidade: "Reboque",
                entidadeId: id?.ToString() ?? "0",
                dados: dto,
                usuarioCodigo: ObterUsuarioCodigo(),
                usuarioNome: ObterUsuarioNome(),
                grupo: ObterUsuarioGrupo(),
                ip: ObterEnderecoIP()
            );

            return CreatedAtAction(nameof(ObterPorId), new { id }, new { id, mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar reboque. Placa: {Placa}, Usuário: {Usuario}", 
                dto?.Placa, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao criar reboque" });
        }
    }

    /// <summary>
    /// Atualiza um reboque existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, [FromBody] ReboqueCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Atualizando reboque. ID: {Id}, Placa: {Placa}, Usuário: {Usuario}", 
                id, dto.Placa, usuario);

            // Dados anteriores para auditoria
            var reboqueAnterior = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.AtualizarAsync(id, dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao atualizar reboque. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Reboque atualizado com sucesso. ID: {Id}, Placa: {Placa}, Usuário: {Usuario}", 
                id, dto.Placa, usuario);

            // Auditoria
            if (reboqueAnterior != null)
            {
                await _logAuditoria.LogAlteracaoAsync<object>(
                    modulo: "Transporte",
                    entidade: "Reboque",
                    entidadeId: id.ToString(),
                    dadosAnteriores: reboqueAnterior,
                    dadosNovos: dto,
                    usuarioCodigo: ObterUsuarioCodigo(),
                    usuarioNome: ObterUsuarioNome(),
                    grupo: ObterUsuarioGrupo(),
                    ip: ObterEnderecoIP()
                );
            }

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar reboque {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar reboque" });
        }
    }

    /// <summary>
    /// Exclui um reboque
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Excluindo reboque. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Dados para auditoria antes da exclusão
            var reboqueExcluido = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.ExcluirAsync(id);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao excluir reboque. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Reboque excluído com sucesso. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Auditoria
            if (reboqueExcluido != null)
            {
                await _logAuditoria.LogExclusaoAsync(
                    modulo: "Transporte",
                    entidade: "Reboque",
                    entidadeId: id.ToString(),
                    dadosExcluidos: reboqueExcluido,
                    usuarioCodigo: ObterUsuarioCodigo(),
                    usuarioNome: ObterUsuarioNome(),
                    grupo: ObterUsuarioGrupo(),
                    ip: ObterEnderecoIP()
                );
            }

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir reboque {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao excluir reboque" });
        }
    }

    #region Métodos Auxiliares

    private int ObterUsuarioCodigo()
    {
        var codigoStr = User.FindFirst("codigo")?.Value;
        return int.TryParse(codigoStr, out var codigo) ? codigo : 0;
    }

    private string ObterUsuarioNome()
    {
        return User?.Identity?.Name ?? "Desconhecido";
    }

    private string ObterUsuarioGrupo()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuário";
    }

    private string? ObterEnderecoIP()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    #endregion
}
