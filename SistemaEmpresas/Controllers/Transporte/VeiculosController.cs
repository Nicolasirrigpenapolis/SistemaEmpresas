using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services;
using SistemaEmpresas.Services.Logs;
using SistemaEmpresas.Services.Transporte;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers.Transporte;

[ApiController]
[Route("api/transporte/[controller]")]
[Authorize]
public class VeiculosController : ControllerBase
{
    private readonly IVeiculoService _service;
    private readonly ILogger<VeiculosController> _logger;
    private readonly ILogAuditoriaService _logAuditoria;

    public VeiculosController(
        IVeiculoService service, 
        ILogger<VeiculosController> logger,
        ILogAuditoriaService logAuditoria)
    {
        _service = service;
        _logger = logger;
        _logAuditoria = logAuditoria;
    }

    /// <summary>
    /// Lista todos os veículos com paginação e filtros
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<VeiculoListDto>>> Listar([FromQuery] VeiculoFiltros filtros)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando veículos. Usuário: {Usuario}, Filtros: {@Filtros}", usuario, filtros);
            
            var resultado = await _service.ListarAsync(filtros);
            
            _logger.LogInformation("Listagem de veículos concluída. Total: {Total}", resultado.TotalCount);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar veículos. Usuário: {Usuario}", User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar veículos", erro = ex.Message, stack = ex.StackTrace });
        }
    }

    /// <summary>
    /// Lista veículos ativos para seleção em combos
    /// </summary>
    [HttpGet("ativos")]
    public async Task<ActionResult<List<VeiculoListDto>>> ListarAtivos()
    {
        try
        {
            var filtros = new VeiculoFiltros 
            { 
                Pagina = 1, 
                TamanhoPagina = 1000, 
                IncluirInativos = false 
            };
            
            var resultado = await _service.ListarAsync(filtros);
            return Ok(resultado.Items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar veículos ativos");
            return StatusCode(500, new { mensagem = "Erro interno ao listar veículos ativos" });
        }
    }

    /// <summary>
    /// Obtém um veículo por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<VeiculoDto>> ObterPorId(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Obtendo veículo ID: {Id}. Usuário: {Usuario}", id, usuario);
            
            var veiculo = await _service.ObterPorIdAsync(id);
            if (veiculo == null)
            {
                _logger.LogWarning("Veículo não encontrado. ID: {Id}. Usuário: {Usuario}", id, usuario);
                return NotFound(new { mensagem = "Veículo não encontrado" });
            }
            
            _logger.LogInformation("Veículo obtido. ID: {Id}, Placa: {Placa}", id, veiculo.Placa);
            return Ok(veiculo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter veículo {Id}. Usuário: {Usuario}", id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao obter veículo" });
        }
    }

    /// <summary>
    /// Obtém um veículo por placa
    /// </summary>
    [HttpGet("placa/{placa}")]
    public async Task<ActionResult<VeiculoDto>> ObterPorPlaca(string placa)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Obtendo veículo por placa: {Placa}. Usuário: {Usuario}", placa, usuario);
            
            var veiculo = await _service.ObterPorPlacaAsync(placa);
            if (veiculo == null)
            {
                _logger.LogWarning("Veículo não encontrado. Placa: {Placa}. Usuário: {Usuario}", placa, usuario);
                return NotFound(new { mensagem = "Veículo não encontrado" });
            }
            
            _logger.LogInformation("Veículo obtido por placa. Placa: {Placa}", placa);
            return Ok(veiculo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter veículo por placa {Placa}. Usuário: {Usuario}", placa, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao obter veículo" });
        }
    }

    /// <summary>
    /// Cria um novo veículo
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] VeiculoCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Criando novo veículo. Placa: {Placa}, Usuário: {Usuario}", dto.Placa, usuario);

            var (sucesso, mensagem, id) = await _service.CriarAsync(dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao criar veículo. Placa: {Placa}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    dto.Placa, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Veículo criado com sucesso. ID: {Id}, Placa: {Placa}, Usuário: {Usuario}", 
                id, dto.Placa, usuario);

            // Auditoria: Registrar criação
            await _logAuditoria.LogCriacaoAsync(
                modulo: "Transporte",
                entidade: "Veiculo",
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
            _logger.LogError(ex, "Erro ao criar veículo. Placa: {Placa}, Usuário: {Usuario}", 
                dto?.Placa, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao criar veículo" });
        }
    }

    /// <summary>
    /// Atualiza um veículo existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, [FromBody] VeiculoCreateUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Atualizando veículo. ID: {Id}, Placa: {Placa}, Usuário: {Usuario}", 
                id, dto.Placa, usuario);

            // Obter dados anteriores para auditoria
            var veiculoAnterior = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.AtualizarAsync(id, dto);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao atualizar veículo. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Veículo atualizado com sucesso. ID: {Id}, Placa: {Placa}, Usuário: {Usuario}", 
                id, dto.Placa, usuario);

            // Auditoria: Registrar alteração
            if (veiculoAnterior != null)
            {
                await _logAuditoria.LogAlteracaoAsync<object>(
                    modulo: "Transporte",
                    entidade: "Veiculo",
                    entidadeId: id.ToString(),
                    dadosAnteriores: veiculoAnterior,
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
            _logger.LogError(ex, "Erro ao atualizar veículo {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar veículo" });
        }
    }

    /// <summary>
    /// Exclui um veículo
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Excluindo veículo. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Obter dados antes da exclusão para auditoria
            var veiculoExcluido = await _service.ObterPorIdAsync(id);

            var (sucesso, mensagem) = await _service.ExcluirAsync(id);
            
            if (!sucesso)
            {
                _logger.LogWarning("Falha ao excluir veículo. ID: {Id}, Mensagem: {Mensagem}, Usuário: {Usuario}", 
                    id, mensagem, usuario);
                return BadRequest(new { mensagem });
            }

            _logger.LogInformation("Veículo excluído com sucesso. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Auditoria: Registrar exclusão
            if (veiculoExcluido != null)
            {
                await _logAuditoria.LogExclusaoAsync(
                    modulo: "Transporte",
                    entidade: "Veiculo",
                    entidadeId: id.ToString(),
                    dadosExcluidos: veiculoExcluido,
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
            _logger.LogError(ex, "Erro ao excluir veículo {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao excluir veículo" });
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
