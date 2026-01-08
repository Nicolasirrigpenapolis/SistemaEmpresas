using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.Features.Transporte.Dtos;
using SistemaEmpresas.Features.Transporte.Services;
using SistemaEmpresas.Services.Logs;
using SistemaEmpresas.Core.Dtos;
using System.Security.Claims;

namespace SistemaEmpresas.Features.Transporte.Controllers;

[ApiController]
[Route("api/transporte/[controller]")]
[Authorize]
public class ManutencoesController : ControllerBase
{
    private readonly IManutencaoVeiculoService _service;
    private readonly IManutencaoPecaService _pecaService;
    private readonly ILogger<ManutencoesController> _logger;
    private readonly ILogAuditoriaService _logAuditoria;

    public ManutencoesController(
        IManutencaoVeiculoService service,
        IManutencaoPecaService pecaService,
        ILogger<ManutencoesController> logger,
        ILogAuditoriaService logAuditoria)
    {
        _service = service;
        _pecaService = pecaService;
        _logger = logger;
        _logAuditoria = logAuditoria;
    }

    #region Métodos Auxiliares para Auditoria
    private int ObterUsuarioCodigo()
    {
        var codigo = User.FindFirst("codigo")?.Value;
        return int.TryParse(codigo, out var id) ? id : 0;
    }

    private string ObterUsuarioNome() => User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
    private string ObterUsuarioGrupo() => User.FindFirst(ClaimTypes.Role)?.Value ?? "";
    private string ObterEnderecoIP() => HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
    #endregion

    /// <summary>
    /// Lista todas as manutenções
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ManutencaoVeiculoListDto>>> Listar([FromQuery] ManutencaoFiltros filtros)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando manutenções. Usuário: {Usuario}, Filtros: {@Filtros}", usuario, filtros);

            var resultado = await _service.ListarAsync(filtros);
            
            _logger.LogInformation("Manutenções listadas com sucesso. Total: {Total}, Usuário: {Usuario}", 
                resultado.TotalCount, usuario);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar manutenções. Usuário: {Usuario}", 
                User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar manutenções", erro = ex.Message, stack = ex.StackTrace });
        }
    }

    /// <summary>
    /// Lista manutenções por veículo
    /// </summary>
    [HttpGet("veiculo/{veiculoId}")]
    public async Task<ActionResult<List<ManutencaoVeiculoListDto>>> ListarPorVeiculo(int veiculoId)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando manutenções por veículo. VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                veiculoId, usuario);

            var manutencoes = await _service.ListarPorVeiculoAsync(veiculoId);
            
            _logger.LogInformation("Manutenções do veículo listadas com sucesso. Total: {Total}, VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                manutencoes?.Count ?? 0, veiculoId, usuario);
            return Ok(manutencoes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar manutenções do veículo {VeiculoId}. Usuário: {Usuario}", 
                veiculoId, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar manutenções" });
        }
    }

    /// <summary>
    /// Obtém uma manutenção por ID (com peças)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ManutencaoVeiculoDto>> ObterPorId(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Obtendo manutenção. ID: {Id}, Usuário: {Usuario}", id, usuario);

            var manutencao = await _service.ObterPorIdAsync(id);
            if (manutencao == null)
            {
                _logger.LogWarning("Manutenção não encontrada. ID: {Id}, Usuário: {Usuario}", id, usuario);
                return NotFound(new { mensagem = "Manutenção não encontrada" });
            }

            _logger.LogInformation("Manutenção obtida com sucesso. ID: {Id}, VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                id, manutencao.VeiculoId, usuario);
            return Ok(manutencao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter manutenção {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao obter manutenção" });
        }
    }

    /// <summary>
    /// Cria uma nova manutenção
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] ManutencaoVeiculoCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Criando nova manutenção. VeiculoID: {VeiculoId}, Data: {Data}, Usuário: {Usuario}", 
                dto.VeiculoId, dto.DataManutencao.Date, usuario);

            var (sucesso, mensagem, id) = await _service.CriarAsync(dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao criar manutenção. VeiculoID: {VeiculoId}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    dto.VeiculoId, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            // Log de auditoria
            await _logAuditoria.LogCriacaoAsync(
                modulo: "Transporte",
                entidade: "ManutencaoVeiculo",
                entidadeId: id?.ToString() ?? "0",
                dados: dto,
                usuarioCodigo: ObterUsuarioCodigo(),
                usuarioNome: ObterUsuarioNome(),
                grupo: ObterUsuarioGrupo(),
                ip: ObterEnderecoIP()
            );

            _logger.LogInformation("Manutenção criada com sucesso. ID: {Id}, VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                id, dto.VeiculoId, usuario);
            return CreatedAtAction(nameof(ObterPorId), new { id }, new { id, mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar manutenção. VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                dto?.VeiculoId, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao criar manutenção" });
        }
    }

    /// <summary>
    /// Atualiza uma manutenção existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, [FromBody] ManutencaoVeiculoCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Atualizando manutenção. ID: {Id}, VeiculoID: {VeiculoId}, Data: {Data}, Usuário: {Usuario}", 
                id, dto.VeiculoId, dto.DataManutencao.Date, usuario);

            // Captura dados anteriores para auditoria
            var manutencaoAnterior = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.AtualizarAsync(id, dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao atualizar manutenção. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            // Log de auditoria
            await _logAuditoria.LogAlteracaoAsync<object>(
                modulo: "Transporte",
                entidade: "ManutencaoVeiculo",
                entidadeId: id.ToString(),
                dadosAnteriores: (object?)manutencaoAnterior ?? new { },
                dadosNovos: dto,
                usuarioCodigo: ObterUsuarioCodigo(),
                usuarioNome: ObterUsuarioNome(),
                grupo: ObterUsuarioGrupo(),
                ip: ObterEnderecoIP()
            );

            _logger.LogInformation("Manutenção atualizada com sucesso. ID: {Id}, VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                id, dto.VeiculoId, usuario);
            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar manutenção {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar manutenção" });
        }
    }

    /// <summary>
    /// Exclui uma manutenção (e todas as peças associadas)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Excluindo manutenção. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Captura dados para auditoria antes da exclusão
            var manutencaoExcluida = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.ExcluirAsync(id);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao excluir manutenção. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            // Log de auditoria
            if (manutencaoExcluida != null)
            {
                await _logAuditoria.LogExclusaoAsync(
                    modulo: "Transporte",
                    entidade: "ManutencaoVeiculo",
                    entidadeId: id.ToString(),
                    dadosExcluidos: manutencaoExcluida,
                    usuarioCodigo: ObterUsuarioCodigo(),
                    usuarioNome: ObterUsuarioNome(),
                    grupo: ObterUsuarioGrupo(),
                    ip: ObterEnderecoIP()
                );
            }

            _logger.LogInformation("Manutenção excluída com sucesso. ID: {Id}, Usuário: {Usuario}", id, usuario);
            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir manutenção {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao excluir manutenção" });
        }
    }

    // ==========================================
    // PEÇAS DA MANUTENÇÃO
    // ==========================================

    /// <summary>
    /// Lista peças de uma manutenção
    /// </summary>
    [HttpGet("{manutencaoId}/pecas")]
    public async Task<ActionResult<List<ManutencaoPecaDto>>> ListarPecas(int manutencaoId)
    {
        try
        {
            var pecas = await _pecaService.ListarPorManutencaoAsync(manutencaoId);
            return Ok(pecas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar peças da manutenção {ManutencaoId}", manutencaoId);
            return StatusCode(500, new { mensagem = "Erro interno ao listar peças" });
        }
    }

    /// <summary>
    /// Adiciona uma peça à manutenção
    /// </summary>
    [HttpPost("{manutencaoId}/pecas")]
    public async Task<ActionResult> AdicionarPeca(int manutencaoId, [FromBody] ManutencaoPecaCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ManutencaoId = manutencaoId;
            var (sucesso, mensagem, id) = await _pecaService.CriarAsync(dto);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return CreatedAtAction(nameof(ObterPorId), new { id = manutencaoId }, new { id, mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar peça à manutenção {ManutencaoId}", manutencaoId);
            return StatusCode(500, new { mensagem = "Erro interno ao adicionar peça" });
        }
    }

    /// <summary>
    /// Atualiza uma peça
    /// </summary>
    [HttpPut("pecas/{id}")]
    public async Task<ActionResult> AtualizarPeca(int id, [FromBody] ManutencaoPecaCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (sucesso, mensagem) = await _pecaService.AtualizarAsync(id, dto);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar peça {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar peça" });
        }
    }

    /// <summary>
    /// Exclui uma peça
    /// </summary>
    [HttpDelete("pecas/{id}")]
    public async Task<ActionResult> ExcluirPeca(int id)
    {
        try
        {
            var (sucesso, mensagem) = await _pecaService.ExcluirAsync(id);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir peça {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao excluir peça" });
        }
    }
}
