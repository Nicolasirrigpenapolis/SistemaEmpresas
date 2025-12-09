using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers;

/// <summary>
/// Controller para gerenciamento e consulta de logs de auditoria.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LogsController : ControllerBase
{
    private readonly ILogAuditoriaService _logService;
    private readonly ILogger<LogsController> _logger;

    public LogsController(
        ILogAuditoriaService logService,
        ILogger<LogsController> logger)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Lista logs de auditoria com paginação e filtros.
    /// </summary>
    /// <param name="filtro">Filtros de busca</param>
    [HttpGet]
    [ProducesResponseType(typeof(LogAuditoriaPagedResult), 200)]
    public async Task<ActionResult<LogAuditoriaPagedResult>> GetLogs([FromQuery] LogAuditoriaFiltroDto filtro)
    {
        try
        {
            _logger.LogInformation("Consultando logs de auditoria com filtros");
            var resultado = await _logService.GetLogsAsync(filtro);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar logs de auditoria: {Message}", ex.Message);
            return StatusCode(500, new { sucesso = false, mensagem = "Erro ao consultar logs de auditoria" });
        }
    }

    /// <summary>
    /// Obtém detalhes de um log específico.
    /// </summary>
    /// <param name="id">ID do log</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LogAuditoriaDetalheDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<LogAuditoriaDetalheDto>> GetLogDetalhe(long id)
    {
        try
        {
            _logger.LogInformation("Buscando detalhes do log {Id}", id);
            var log = await _logService.GetLogDetalheAsync(id);
            
            if (log == null)
            {
                return NotFound(new { sucesso = false, mensagem = "Log não encontrado" });
            }

            return Ok(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar detalhes do log {Id}: {Message}", id, ex.Message);
            return StatusCode(500, new { sucesso = false, mensagem = "Erro ao buscar detalhes do log" });
        }
    }

    /// <summary>
    /// Obtém estatísticas dos logs de auditoria.
    /// </summary>
    /// <param name="dataInicio">Data inicial (opcional, padrão: últimos 30 dias)</param>
    /// <param name="dataFim">Data final (opcional, padrão: hoje)</param>
    [HttpGet("estatisticas")]
    [ProducesResponseType(typeof(LogAuditoriaEstatisticasDto), 200)]
    public async Task<ActionResult<LogAuditoriaEstatisticasDto>> GetEstatisticas(
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim)
    {
        try
        {
            _logger.LogInformation("Obtendo estatísticas de logs de auditoria");
            var estatisticas = await _logService.GetEstatisticasAsync(dataInicio, dataFim);
            return Ok(estatisticas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas: {Message}", ex.Message);
            return StatusCode(500, new { sucesso = false, mensagem = "Erro ao obter estatísticas" });
        }
    }

    /// <summary>
    /// Busca logs de uma entidade específica (histórico de alterações).
    /// </summary>
    /// <param name="entidade">Nome da entidade (ex: Usuario, Produto)</param>
    /// <param name="entidadeId">ID da entidade</param>
    [HttpGet("entidade/{entidade}/{entidadeId}")]
    [ProducesResponseType(typeof(IEnumerable<LogAuditoriaListDto>), 200)]
    public async Task<ActionResult<IEnumerable<LogAuditoriaListDto>>> GetLogsByEntidade(
        string entidade, 
        string entidadeId)
    {
        try
        {
            _logger.LogInformation("Buscando logs da entidade {Entidade} ID {Id}", entidade, entidadeId);
            var logs = await _logService.GetLogsByEntidadeAsync(entidade, entidadeId);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar logs da entidade: {Message}", ex.Message);
            return StatusCode(500, new { sucesso = false, mensagem = "Erro ao buscar logs da entidade" });
        }
    }

    /// <summary>
    /// Busca logs de um usuário específico.
    /// </summary>
    /// <param name="usuarioCodigo">Código do usuário</param>
    /// <param name="limite">Limite de registros (padrão: 100)</param>
    [HttpGet("usuario/{usuarioCodigo}")]
    [ProducesResponseType(typeof(IEnumerable<LogAuditoriaListDto>), 200)]
    public async Task<ActionResult<IEnumerable<LogAuditoriaListDto>>> GetLogsByUsuario(
        int usuarioCodigo,
        [FromQuery] int limite = 100)
    {
        try
        {
            _logger.LogInformation("Buscando logs do usuário {Usuario}", usuarioCodigo);
            var logs = await _logService.GetLogsByUsuarioAsync(usuarioCodigo, limite);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar logs do usuário: {Message}", ex.Message);
            return StatusCode(500, new { sucesso = false, mensagem = "Erro ao buscar logs do usuário" });
        }
    }

    /// <summary>
    /// Lista tipos de ações disponíveis para filtro.
    /// </summary>
    [HttpGet("tipos-acao")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    public ActionResult<IEnumerable<object>> GetTiposAcao()
    {
        var tipos = new[]
        {
            new { valor = "LOGIN", label = "Login", cor = "bg-green-500", icone = "ArrowRightOnRectangleIcon" },
            new { valor = "LOGOUT", label = "Logout", cor = "bg-gray-500", icone = "ArrowLeftOnRectangleIcon" },
            new { valor = "CRIAR", label = "Criar", cor = "bg-blue-500", icone = "PlusCircleIcon" },
            new { valor = "ALTERAR", label = "Alterar", cor = "bg-yellow-500", icone = "PencilSquareIcon" },
            new { valor = "EXCLUIR", label = "Excluir", cor = "bg-red-500", icone = "TrashIcon" },
            new { valor = "VISUALIZAR", label = "Visualizar", cor = "bg-purple-500", icone = "EyeIcon" },
            new { valor = "EXPORTAR", label = "Exportar", cor = "bg-teal-500", icone = "ArrowDownTrayIcon" },
            new { valor = "IMPORTAR", label = "Importar", cor = "bg-indigo-500", icone = "ArrowUpTrayIcon" },
            new { valor = "APROVAR", label = "Aprovar", cor = "bg-emerald-500", icone = "CheckCircleIcon" },
            new { valor = "REJEITAR", label = "Rejeitar", cor = "bg-rose-500", icone = "XCircleIcon" },
            new { valor = "ERRO", label = "Erro", cor = "bg-red-600", icone = "ExclamationTriangleIcon" }
        };

        return Ok(tipos);
    }

    /// <summary>
    /// Lista módulos disponíveis para filtro.
    /// </summary>
    [HttpGet("modulos")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    public ActionResult<IEnumerable<object>> GetModulos()
    {
        var modulos = new[]
        {
            new { valor = "Sistema", label = "Sistema" },
            new { valor = "Cadastros", label = "Cadastros" },
            new { valor = "Fiscal", label = "Fiscal" },
            new { valor = "Usuarios", label = "Usuários" },
            new { valor = "Permissoes", label = "Permissões" },
            new { valor = "Dashboard", label = "Dashboard" }
        };

        return Ok(modulos);
    }

    /// <summary>
    /// Limpa logs antigos (apenas administradores).
    /// </summary>
    /// <param name="diasParaManter">Quantidade de dias a manter (padrão: 90)</param>
    [HttpDelete("limpar")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(403)]
    public async Task<ActionResult> LimparLogsAntigos([FromQuery] int diasParaManter = 90)
    {
        try
        {
            // Verificar se é admin
            var grupo = User.FindFirst("grupo")?.Value;
            if (grupo?.ToUpper() != "PROGRAMADOR" && grupo?.ToUpper() != "ADMIN")
            {
                return Forbid();
            }

            _logger.LogWarning("Limpando logs com mais de {Dias} dias", diasParaManter);
            var registrosExcluidos = await _logService.LimparLogsAntigosAsync(diasParaManter);
            
            return Ok(new 
            { 
                sucesso = true, 
                mensagem = $"Foram excluídos {registrosExcluidos} registros de log",
                registrosExcluidos 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao limpar logs antigos: {Message}", ex.Message);
            return StatusCode(500, new { sucesso = false, mensagem = "Erro ao limpar logs antigos" });
        }
    }

    /// <summary>
    /// Obtém o histórico de atividades do usuário logado.
    /// </summary>
    [HttpGet("minha-atividade")]
    [ProducesResponseType(typeof(IEnumerable<LogAuditoriaListDto>), 200)]
    public async Task<ActionResult<IEnumerable<LogAuditoriaListDto>>> GetMinhaAtividade([FromQuery] int limite = 50)
    {
        try
        {
            var usuarioCodigoStr = User.FindFirst("codigo")?.Value;
            if (!int.TryParse(usuarioCodigoStr, out var usuarioCodigo))
            {
                return BadRequest(new { sucesso = false, mensagem = "Usuário não identificado" });
            }

            var logs = await _logService.GetLogsByUsuarioAsync(usuarioCodigo, limite);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar atividade do usuário: {Message}", ex.Message);
            return StatusCode(500, new { sucesso = false, mensagem = "Erro ao buscar atividade" });
        }
    }
}
