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
public class ViagensController : ControllerBase
{
    private readonly IViagemService _service;
    private readonly IDespesaViagemService _despesaService;
    private readonly IReceitaViagemService _receitaService;
    private readonly ILogger<ViagensController> _logger;
    private readonly ILogAuditoriaService _logAuditoria;

    public ViagensController(
        IViagemService service,
        IDespesaViagemService despesaService,
        IReceitaViagemService receitaService,
        ILogger<ViagensController> logger,
        ILogAuditoriaService logAuditoria)
    {
        _service = service;
        _despesaService = despesaService;
        _receitaService = receitaService;
        _logger = logger;
        _logAuditoria = logAuditoria;
    }

    /// <summary>
    /// Lista todas as viagens
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ViagemListDto>>> Listar([FromQuery] bool apenasAtivos = true)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando viagens. Usuário: {Usuario}, ApenasAtivos: {ApenasAtivos}", usuario, apenasAtivos);

            var viagens = await _service.ListarAsync(apenasAtivos);
            
            _logger.LogInformation("Viagens listadas com sucesso. Total: {Total}, Usuário: {Usuario}", 
                viagens?.Count ?? 0, usuario);
            return Ok(viagens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar viagens. Usuário: {Usuario}", 
                User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar viagens" });
        }
    }

    /// <summary>
    /// Lista viagens por veículo
    /// </summary>
    [HttpGet("veiculo/{veiculoId}")]
    public async Task<ActionResult<List<ViagemListDto>>> ListarPorVeiculo(int veiculoId)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando viagens por veículo. VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                veiculoId, usuario);

            var viagens = await _service.ListarPorVeiculoAsync(veiculoId);
            
            _logger.LogInformation("Viagens do veículo listadas com sucesso. Total: {Total}, VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                viagens?.Count ?? 0, veiculoId, usuario);
            return Ok(viagens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar viagens do veículo {VeiculoId}. Usuário: {Usuario}", 
                veiculoId, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar viagens" });
        }
    }

    /// <summary>
    /// Lista viagens por motorista
    /// </summary>
    [HttpGet("motorista/{motoristaId}")]
    public async Task<ActionResult<List<ViagemListDto>>> ListarPorMotorista(short motoristaId)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando viagens por motorista. MotoristaID: {MotoristaId}, Usuário: {Usuario}", 
                motoristaId, usuario);

            var viagens = await _service.ListarPorMotoristaAsync(motoristaId);
            
            _logger.LogInformation("Viagens do motorista listadas com sucesso. Total: {Total}, MotoristaID: {MotoristaId}, Usuário: {Usuario}", 
                viagens?.Count ?? 0, motoristaId, usuario);
            return Ok(viagens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar viagens do motorista {MotoristaId}. Usuário: {Usuario}", 
                motoristaId, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar viagens" });
        }
    }

    /// <summary>
    /// Lista viagens por período
    /// </summary>
    [HttpGet("periodo")]
    public async Task<ActionResult<List<ViagemListDto>>> ListarPorPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando viagens por período. Início: {DataInicio}, Fim: {DataFim}, Usuário: {Usuario}", 
                dataInicio.Date, dataFim.Date, usuario);

            var viagens = await _service.ListarPorPeriodoAsync(dataInicio, dataFim);
            
            _logger.LogInformation("Viagens do período listadas com sucesso. Total: {Total}, Período: {DataInicio} - {DataFim}, Usuário: {Usuario}", 
                viagens?.Count ?? 0, dataInicio.Date, dataFim.Date, usuario);
            return Ok(viagens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar viagens do período {DataInicio} - {DataFim}. Usuário: {Usuario}", 
                dataInicio, dataFim, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar viagens" });
        }
    }

    /// <summary>
    /// Obtém uma viagem por ID (com despesas e receitas)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ViagemDto>> ObterPorId(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Obtendo viagem. ID: {Id}, Usuário: {Usuario}", id, usuario);

            var viagem = await _service.ObterPorIdAsync(id);
            if (viagem == null)
            {
                _logger.LogWarning("Viagem não encontrada. ID: {Id}, Usuário: {Usuario}", id, usuario);
                return NotFound(new { mensagem = "Viagem não encontrada" });
            }

            _logger.LogInformation("Viagem obtida com sucesso. ID: {Id}, VeiculoID: {VeiculoId}, MotoristaID: {MotoristaId}, Usuário: {Usuario}", 
                id, viagem.VeiculoId, viagem.MotoristaId, usuario);
            return Ok(viagem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter viagem {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao obter viagem" });
        }
    }

    /// <summary>
    /// Cria uma nova viagem
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] ViagemCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Criando nova viagem. VeiculoID: {VeiculoId}, MotoristaID: {MotoristaId}, Usuário: {Usuario}", 
                dto.VeiculoId, dto.MotoristaId, usuario);

            var (sucesso, mensagem, id) = await _service.CriarAsync(dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao criar viagem. VeiculoID: {VeiculoId}, MotoristaID: {MotoristaId}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    dto.VeiculoId, dto.MotoristaId, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Viagem criada com sucesso. ID: {Id}, VeiculoID: {VeiculoId}, MotoristaID: {MotoristaId}, Usuário: {Usuario}", 
                id, dto.VeiculoId, dto.MotoristaId, usuario);

            // Auditoria
            await _logAuditoria.LogCriacaoAsync(
                modulo: "Transporte",
                entidade: "Viagem",
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
            _logger.LogError(ex, "Erro ao criar viagem. VeiculoID: {VeiculoId}, Usuário: {Usuario}", 
                dto?.VeiculoId, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao criar viagem" });
        }
    }

    /// <summary>
    /// Atualiza uma viagem existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, [FromBody] ViagemCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Atualizando viagem. ID: {Id}, VeiculoID: {VeiculoId}, MotoristaID: {MotoristaId}, Usuário: {Usuario}", 
                id, dto.VeiculoId, dto.MotoristaId, usuario);

            // Dados anteriores para auditoria
            var viagemAnterior = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.AtualizarAsync(id, dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao atualizar viagem. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Viagem atualizada com sucesso. ID: {Id}, VeiculoID: {VeiculoId}, MotoristaID: {MotoristaId}, Usuário: {Usuario}", 
                id, dto.VeiculoId, dto.MotoristaId, usuario);

            // Auditoria
            if (viagemAnterior != null)
            {
                await _logAuditoria.LogAlteracaoAsync<object>(
                    modulo: "Transporte",
                    entidade: "Viagem",
                    entidadeId: id.ToString(),
                    dadosAnteriores: viagemAnterior,
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
            _logger.LogError(ex, "Erro ao atualizar viagem {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar viagem" });
        }
    }

    /// <summary>
    /// Exclui uma viagem (e todas despesas/receitas associadas)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Excluindo viagem. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Dados para auditoria antes da exclusão
            var viagemExcluida = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.ExcluirAsync(id);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao excluir viagem. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Viagem excluída com sucesso. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Auditoria
            if (viagemExcluida != null)
            {
                await _logAuditoria.LogExclusaoAsync(
                    modulo: "Transporte",
                    entidade: "Viagem",
                    entidadeId: id.ToString(),
                    dadosExcluidos: viagemExcluida,
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
            _logger.LogError(ex, "Erro ao excluir viagem {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao excluir viagem" });
        }
    }

    // ==========================================
    // DESPESAS DA VIAGEM
    // ==========================================

    /// <summary>
    /// Lista despesas de uma viagem
    /// </summary>
    [HttpGet("{viagemId}/despesas")]
    public async Task<ActionResult<List<DespesaViagemDto>>> ListarDespesas(int viagemId)
    {
        try
        {
            var despesas = await _despesaService.ListarPorViagemAsync(viagemId);
            return Ok(despesas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar despesas da viagem {ViagemId}", viagemId);
            return StatusCode(500, new { mensagem = "Erro interno ao listar despesas" });
        }
    }

    /// <summary>
    /// Adiciona uma despesa à viagem
    /// </summary>
    [HttpPost("{viagemId}/despesas")]
    public async Task<ActionResult> AdicionarDespesa(int viagemId, [FromBody] DespesaViagemCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ViagemId = viagemId;
            var (sucesso, mensagem, id) = await _despesaService.CriarAsync(dto);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return CreatedAtAction(nameof(ObterPorId), new { id = viagemId }, new { id, mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar despesa à viagem {ViagemId}", viagemId);
            return StatusCode(500, new { mensagem = "Erro interno ao adicionar despesa" });
        }
    }

    /// <summary>
    /// Atualiza uma despesa
    /// </summary>
    [HttpPut("despesas/{id}")]
    public async Task<ActionResult> AtualizarDespesa(int id, [FromBody] DespesaViagemCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (sucesso, mensagem) = await _despesaService.AtualizarAsync(id, dto);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar despesa {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar despesa" });
        }
    }

    /// <summary>
    /// Exclui uma despesa
    /// </summary>
    [HttpDelete("despesas/{id}")]
    public async Task<ActionResult> ExcluirDespesa(int id)
    {
        try
        {
            var (sucesso, mensagem) = await _despesaService.ExcluirAsync(id);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir despesa {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao excluir despesa" });
        }
    }

    // ==========================================
    // RECEITAS DA VIAGEM
    // ==========================================

    /// <summary>
    /// Lista receitas de uma viagem
    /// </summary>
    [HttpGet("{viagemId}/receitas")]
    public async Task<ActionResult<List<ReceitaViagemDto>>> ListarReceitas(int viagemId)
    {
        try
        {
            var receitas = await _receitaService.ListarPorViagemAsync(viagemId);
            return Ok(receitas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar receitas da viagem {ViagemId}", viagemId);
            return StatusCode(500, new { mensagem = "Erro interno ao listar receitas" });
        }
    }

    /// <summary>
    /// Adiciona uma receita à viagem
    /// </summary>
    [HttpPost("{viagemId}/receitas")]
    public async Task<ActionResult> AdicionarReceita(int viagemId, [FromBody] ReceitaViagemCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ViagemId = viagemId;
            var (sucesso, mensagem, id) = await _receitaService.CriarAsync(dto);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return CreatedAtAction(nameof(ObterPorId), new { id = viagemId }, new { id, mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar receita à viagem {ViagemId}", viagemId);
            return StatusCode(500, new { mensagem = "Erro interno ao adicionar receita" });
        }
    }

    /// <summary>
    /// Atualiza uma receita
    /// </summary>
    [HttpPut("receitas/{id}")]
    public async Task<ActionResult> AtualizarReceita(int id, [FromBody] ReceitaViagemCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (sucesso, mensagem) = await _receitaService.AtualizarAsync(id, dto);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar receita {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar receita" });
        }
    }

    /// <summary>
    /// Exclui uma receita
    /// </summary>
    [HttpDelete("receitas/{id}")]
    public async Task<ActionResult> ExcluirReceita(int id)
    {
        try
        {
            var (sucesso, mensagem) = await _receitaService.ExcluirAsync(id);
            
            if (!sucesso)
                return BadRequest(new { mensagem });

            return Ok(new { mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir receita {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao excluir receita" });
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
