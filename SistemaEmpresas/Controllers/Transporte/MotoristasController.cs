using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services.Logs;
using SistemaEmpresas.Services.Transporte;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers.Transporte;

[ApiController]
[Route("api/transporte/[controller]")]
[Authorize]
public class MotoristasController : ControllerBase
{
    private readonly IMotoristaService _motoristaService;
    private readonly ILogger<MotoristasController> _logger;
    private readonly ILogAuditoriaService _logAuditoria;

    public MotoristasController(
        IMotoristaService motoristaService, 
        ILogger<MotoristasController> logger,
        ILogAuditoriaService logAuditoria)
    {
        _motoristaService = motoristaService;
        _logger = logger;
        _logAuditoria = logAuditoria;
    }

    /// <summary>
    /// Lista motoristas com paginação e filtros
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] MotoristaFiltrosDto? filtros)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando motoristas. Filtros: {@Filtros}, Usuário: {Usuario}", filtros, usuario);

            var resultado = await _motoristaService.ListarAsync(filtros ?? new MotoristaFiltrosDto());
            
            _logger.LogInformation("Motoristas listados com sucesso. Total: {Total}, Página: {Pagina}, Usuário: {Usuario}", 
                resultado.TotalCount, filtros?.Pagina ?? 1, usuario);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar motoristas. Usuário: {Usuario}", 
                User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar motoristas" });
        }
    }

    /// <summary>
    /// Lista todos os motoristas ativos (para combos)
    /// </summary>
    [HttpGet("ativos")]
    public async Task<IActionResult> ListarAtivos()
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Listando motoristas ativos. Usuário: {Usuario}", usuario);

            var motoristas = await _motoristaService.ListarAtivosAsync();
            
            _logger.LogInformation("Motoristas ativos listados com sucesso. Total: {Total}, Usuário: {Usuario}", 
                motoristas?.Count ?? 0, usuario);
            return Ok(motoristas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar motoristas ativos. Usuário: {Usuario}", 
                User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao listar motoristas" });
        }
    }

    /// <summary>
    /// Busca um motorista por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(short id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Obtendo motorista. ID: {Id}, Usuário: {Usuario}", id, usuario);

            var motorista = await _motoristaService.BuscarPorIdAsync(id);
            if (motorista == null)
            {
                _logger.LogWarning("Motorista não encontrado. ID: {Id}, Usuário: {Usuario}", id, usuario);
                return NotFound(new { mensagem = "Motorista não encontrado" });
            }

            _logger.LogInformation("Motorista obtido com sucesso. ID: {Id}, Nome: {Nome}, Usuário: {Usuario}", 
                id, motorista.NomeDoMotorista, usuario);
            return Ok(motorista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar motorista {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao buscar motorista" });
        }
    }

    /// <summary>
    /// Cria um novo motorista
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] MotoristaCreateDto dto)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Criando novo motorista. Nome: {Nome}, CPF: {Cpf}, Usuário: {Usuario}", 
                dto.NomeDoMotorista, dto.Cpf, usuario);

            var motorista = await _motoristaService.CriarAsync(dto);
            
            _logger.LogInformation("Motorista criado com sucesso. ID: {Id}, Nome: {Nome}, Usuário: {Usuario}", 
                motorista.CodigoDoMotorista, motorista.NomeDoMotorista, usuario);

            // Auditoria
            await _logAuditoria.LogCriacaoAsync(
                modulo: "Transporte",
                entidade: "Motorista",
                entidadeId: motorista.CodigoDoMotorista.ToString(),
                dados: dto,
                usuarioCodigo: ObterUsuarioCodigo(),
                usuarioNome: ObterUsuarioNome(),
                grupo: ObterUsuarioGrupo(),
                ip: ObterEnderecoIP()
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = motorista.CodigoDoMotorista }, motorista);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Falha ao criar motorista. Motivo: {Motivo}, Usuário: {Usuario}", 
                ex.Message, User?.Identity?.Name ?? "Desconhecido");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar motorista. Nome: {Nome}, Usuário: {Usuario}", 
                dto?.NomeDoMotorista, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao criar motorista" });
        }
    }

    /// <summary>
    /// Atualiza um motorista existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(short id, [FromBody] MotoristaUpdateDto dto)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Atualizando motorista. ID: {Id}, Nome: {Nome}, Usuário: {Usuario}", 
                id, dto.NomeDoMotorista, usuario);

            // Dados anteriores para auditoria
            var motoristaAnterior = await _motoristaService.BuscarPorIdAsync(id);

            var motorista = await _motoristaService.AtualizarAsync(id, dto);
            
            _logger.LogInformation("Motorista atualizado com sucesso. ID: {Id}, Nome: {Nome}, Usuário: {Usuario}", 
                id, motorista.NomeDoMotorista, usuario);

            // Auditoria
            if (motoristaAnterior != null)
            {
                await _logAuditoria.LogAlteracaoAsync<object>(
                    modulo: "Transporte",
                    entidade: "Motorista",
                    entidadeId: id.ToString(),
                    dadosAnteriores: motoristaAnterior,
                    dadosNovos: dto,
                    usuarioCodigo: ObterUsuarioCodigo(),
                    usuarioNome: ObterUsuarioNome(),
                    grupo: ObterUsuarioGrupo(),
                    ip: ObterEnderecoIP()
                );
            }

            return Ok(motorista);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Motorista não encontrado para atualização. ID: {Id}, Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return NotFound(new { mensagem = "Motorista não encontrado" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Falha ao atualizar motorista. ID: {Id}, Motivo: {Motivo}, Usuário: {Usuario}", 
                id, ex.Message, User?.Identity?.Name ?? "Desconhecido");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar motorista {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar motorista" });
        }
    }

    /// <summary>
    /// Exclui um motorista
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(short id)
    {
        try
        {
            var usuario = User?.Identity?.Name ?? "Desconhecido";
            _logger.LogInformation("Excluindo motorista. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Dados para auditoria antes da exclusão
            var motoristaExcluido = await _motoristaService.BuscarPorIdAsync(id);

            await _motoristaService.ExcluirAsync(id);
            
            _logger.LogInformation("Motorista excluído com sucesso. ID: {Id}, Usuário: {Usuario}", id, usuario);

            // Auditoria
            if (motoristaExcluido != null)
            {
                await _logAuditoria.LogExclusaoAsync(
                    modulo: "Transporte",
                    entidade: "Motorista",
                    entidadeId: id.ToString(),
                    dadosExcluidos: motoristaExcluido,
                    usuarioCodigo: ObterUsuarioCodigo(),
                    usuarioNome: ObterUsuarioNome(),
                    grupo: ObterUsuarioGrupo(),
                    ip: ObterEnderecoIP()
                );
            }

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Motorista não encontrado para exclusão. ID: {Id}, Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return NotFound(new { mensagem = "Motorista não encontrado" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Falha ao excluir motorista. ID: {Id}, Motivo: {Motivo}, Usuário: {Usuario}", 
                id, ex.Message, User?.Identity?.Name ?? "Desconhecido");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir motorista {Id}. Usuário: {Usuario}", 
                id, User?.Identity?.Name ?? "Desconhecido");
            return StatusCode(500, new { mensagem = "Erro interno ao excluir motorista" });
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
