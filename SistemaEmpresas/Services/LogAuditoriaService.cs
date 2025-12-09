using Microsoft.Extensions.Logging;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;
using SistemaEmpresas.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SistemaEmpresas.Services;

/// <summary>
/// Serviço de auditoria para registrar todas as ações do sistema.
/// </summary>
public class LogAuditoriaService : ILogAuditoriaService
{
    private readonly ILogAuditoriaRepository _repository;
    private readonly ILogger<LogAuditoriaService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public LogAuditoriaService(
        ILogAuditoriaRepository repository,
        ILogger<LogAuditoriaService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    #region Registro de Logs

    /// <summary>
    /// Registra um log genérico.
    /// </summary>
    public async Task LogAsync(LogAuditoriaCreateDto dto)
    {
        try
        {
            var log = new LogAuditoria
            {
                DataHora = DateTime.Now,
                UsuarioCodigo = dto.UsuarioCodigo,
                UsuarioNome = dto.UsuarioNome,
                UsuarioGrupo = dto.UsuarioGrupo,
                TipoAcao = dto.TipoAcao,
                Modulo = dto.Modulo,
                Entidade = dto.Entidade,
                EntidadeId = dto.EntidadeId,
                Descricao = dto.Descricao,
                DadosAnteriores = dto.DadosAnteriores,
                DadosNovos = dto.DadosNovos,
                CamposAlterados = dto.CamposAlterados,
                EnderecoIP = dto.EnderecoIP,
                UserAgent = dto.UserAgent,
                MetodoHttp = dto.MetodoHttp,
                UrlRequisicao = dto.UrlRequisicao,
                StatusCode = dto.StatusCode,
                TempoExecucaoMs = dto.TempoExecucaoMs,
                Erro = dto.Erro,
                MensagemErro = dto.MensagemErro,
                TenantId = dto.TenantId,
                TenantNome = dto.TenantNome,
                SessaoId = dto.SessaoId,
                CorrelationId = dto.CorrelationId
            };

            await _repository.CreateAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log de auditoria: {Message}", ex.Message);
            // Não propaga exceção para não interromper operações principais
        }
    }

    /// <summary>
    /// Registra login de usuário.
    /// </summary>
    public async Task LogLoginAsync(int usuarioCodigo, string usuarioNome, string grupo, string ip, string? userAgent = null)
    {
        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = TipoAcaoAuditoria.LOGIN,
            Modulo = "Sistema",
            Entidade = "Usuario",
            EntidadeId = usuarioCodigo.ToString(),
            Descricao = $"Usuário {usuarioNome} realizou login no sistema",
            EnderecoIP = ip,
            UserAgent = userAgent
        });
    }

    /// <summary>
    /// Registra logout de usuário.
    /// </summary>
    public async Task LogLogoutAsync(int usuarioCodigo, string usuarioNome, string grupo, string ip)
    {
        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = TipoAcaoAuditoria.LOGOUT,
            Modulo = "Sistema",
            Entidade = "Usuario",
            EntidadeId = usuarioCodigo.ToString(),
            Descricao = $"Usuário {usuarioNome} realizou logout do sistema",
            EnderecoIP = ip
        });
    }

    /// <summary>
    /// Registra criação de entidade.
    /// </summary>
    public async Task LogCriacaoAsync<T>(string modulo, string entidade, string entidadeId, T dados, 
        int usuarioCodigo, string usuarioNome, string grupo, string? ip = null)
    {
        var dadosJson = SerializeObject(dados);
        
        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = TipoAcaoAuditoria.CRIAR,
            Modulo = modulo,
            Entidade = entidade,
            EntidadeId = entidadeId,
            Descricao = $"Criado {entidade} com ID {entidadeId}",
            DadosNovos = dadosJson,
            EnderecoIP = ip
        });
    }

    /// <summary>
    /// Registra alteração de entidade.
    /// </summary>
    public async Task LogAlteracaoAsync<T>(string modulo, string entidade, string entidadeId, 
        T dadosAnteriores, T dadosNovos, int usuarioCodigo, string usuarioNome, string grupo, string? ip = null)
    {
        var anterioresJson = SerializeObject(dadosAnteriores);
        var novosJson = SerializeObject(dadosNovos);
        var camposAlterados = GetCamposAlterados(dadosAnteriores, dadosNovos);

        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = TipoAcaoAuditoria.ALTERAR,
            Modulo = modulo,
            Entidade = entidade,
            EntidadeId = entidadeId,
            Descricao = $"Alterado {entidade} com ID {entidadeId}",
            DadosAnteriores = anterioresJson,
            DadosNovos = novosJson,
            CamposAlterados = camposAlterados,
            EnderecoIP = ip
        });
    }

    /// <summary>
    /// Registra exclusão de entidade.
    /// </summary>
    public async Task LogExclusaoAsync<T>(string modulo, string entidade, string entidadeId, T dadosExcluidos, 
        int usuarioCodigo, string usuarioNome, string grupo, string? ip = null)
    {
        var dadosJson = SerializeObject(dadosExcluidos);

        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = TipoAcaoAuditoria.EXCLUIR,
            Modulo = modulo,
            Entidade = entidade,
            EntidadeId = entidadeId,
            Descricao = $"Excluído {entidade} com ID {entidadeId}",
            DadosAnteriores = dadosJson,
            EnderecoIP = ip
        });
    }

    /// <summary>
    /// Registra consulta de entidade.
    /// </summary>
    public async Task LogConsultaAsync(string modulo, string entidade, string? entidadeId, 
        int usuarioCodigo, string usuarioNome, string grupo, string? ip = null)
    {
        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = TipoAcaoAuditoria.VISUALIZAR,
            Modulo = modulo,
            Entidade = entidade,
            EntidadeId = entidadeId,
            Descricao = entidadeId != null 
                ? $"Consultado {entidade} com ID {entidadeId}" 
                : $"Listado registros de {entidade}",
            EnderecoIP = ip
        });
    }

    /// <summary>
    /// Registra erro no sistema.
    /// </summary>
    public async Task LogErroAsync(string modulo, string entidade, string mensagem, Exception? ex, 
        int usuarioCodigo, string usuarioNome, string grupo, string? ip = null)
    {
        var mensagemCompleta = ex != null 
            ? $"{mensagem}: {ex.Message}" 
            : mensagem;

        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = TipoAcaoAuditoria.ERRO,
            Modulo = modulo,
            Entidade = entidade,
            Descricao = mensagemCompleta,
            Erro = true,
            MensagemErro = ex?.ToString(),
            EnderecoIP = ip
        });
    }

    /// <summary>
    /// Registra ação customizada.
    /// </summary>
    public async Task LogAcaoCustomAsync(string tipoAcao, string modulo, string entidade, string descricao, 
        int usuarioCodigo, string usuarioNome, string grupo, string? ip = null)
    {
        await LogAsync(new LogAuditoriaCreateDto
        {
            UsuarioCodigo = usuarioCodigo,
            UsuarioNome = usuarioNome,
            UsuarioGrupo = grupo,
            TipoAcao = tipoAcao,
            Modulo = modulo,
            Entidade = entidade,
            Descricao = descricao,
            EnderecoIP = ip
        });
    }

    #endregion

    #region Consulta de Logs

    /// <summary>
    /// Busca logs paginados com filtros.
    /// </summary>
    public async Task<LogAuditoriaPagedResult> GetLogsAsync(LogAuditoriaFiltroDto filtro)
    {
        return await _repository.GetPagedAsync(filtro);
    }

    /// <summary>
    /// Busca detalhes de um log específico.
    /// </summary>
    public async Task<LogAuditoriaDetalheDto?> GetLogDetalheAsync(long id)
    {
        var log = await _repository.GetByIdAsync(id);
        if (log == null) return null;

        return new LogAuditoriaDetalheDto
        {
            Id = log.Id,
            DataHora = log.DataHora,
            UsuarioCodigo = log.UsuarioCodigo,
            UsuarioNome = log.UsuarioNome,
            UsuarioGrupo = log.UsuarioGrupo,
            TipoAcao = log.TipoAcao,
            Modulo = log.Modulo,
            Entidade = log.Entidade,
            EntidadeId = log.EntidadeId,
            Descricao = log.Descricao,
            DadosAnteriores = log.DadosAnteriores,
            DadosNovos = log.DadosNovos,
            CamposAlterados = log.CamposAlterados,
            EnderecoIP = log.EnderecoIP,
            UserAgent = log.UserAgent,
            MetodoHttp = log.MetodoHttp,
            UrlRequisicao = log.UrlRequisicao,
            StatusCode = log.StatusCode,
            TempoExecucaoMs = log.TempoExecucaoMs,
            Erro = log.Erro,
            MensagemErro = log.MensagemErro,
            TenantId = log.TenantId,
            TenantNome = log.TenantNome,
            SessaoId = log.SessaoId,
            CorrelationId = log.CorrelationId
        };
    }

    /// <summary>
    /// Obtém estatísticas dos logs.
    /// </summary>
    public async Task<LogAuditoriaEstatisticasDto> GetEstatisticasAsync(DateTime? dataInicio, DateTime? dataFim)
    {
        return await _repository.GetEstatisticasAsync(dataInicio, dataFim);
    }

    /// <summary>
    /// Busca logs de uma entidade específica.
    /// </summary>
    public async Task<IEnumerable<LogAuditoriaListDto>> GetLogsByEntidadeAsync(string entidade, string entidadeId)
    {
        var logs = await _repository.GetByEntidadeAsync(entidade, entidadeId);
        return logs.Select(MapToListDto);
    }

    /// <summary>
    /// Busca logs de um usuário específico.
    /// </summary>
    public async Task<IEnumerable<LogAuditoriaListDto>> GetLogsByUsuarioAsync(int usuarioCodigo, int limite = 100)
    {
        var logs = await _repository.GetByUsuarioAsync(usuarioCodigo, limite);
        return logs.Select(MapToListDto);
    }

    #endregion

    #region Manutenção

    /// <summary>
    /// Limpa logs antigos.
    /// </summary>
    public async Task<int> LimparLogsAntigosAsync(int diasParaManter = 90)
    {
        return await _repository.LimparLogsAntigosAsync(diasParaManter);
    }

    #endregion

    #region Helpers

    private string? SerializeObject<T>(T obj)
    {
        if (obj == null) return null;
        
        try
        {
            return JsonSerializer.Serialize(obj, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao serializar objeto para auditoria");
            return obj.ToString();
        }
    }

    private string? GetCamposAlterados<T>(T antes, T depois)
    {
        if (antes == null || depois == null) return null;

        try
        {
            var camposAlterados = new List<string>();
            var type = typeof(T);
            var properties = type.GetProperties();

            foreach (var prop in properties)
            {
                var valorAntes = prop.GetValue(antes);
                var valorDepois = prop.GetValue(depois);

                if (!Equals(valorAntes, valorDepois))
                {
                    camposAlterados.Add(prop.Name);
                }
            }

            return camposAlterados.Count > 0 
                ? string.Join(", ", camposAlterados) 
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao detectar campos alterados");
            return null;
        }
    }

    private LogAuditoriaListDto MapToListDto(LogAuditoria log)
    {
        return new LogAuditoriaListDto
        {
            Id = log.Id,
            DataHora = log.DataHora,
            UsuarioCodigo = log.UsuarioCodigo,
            UsuarioNome = log.UsuarioNome,
            UsuarioGrupo = log.UsuarioGrupo,
            TipoAcao = log.TipoAcao,
            Modulo = log.Modulo,
            Entidade = log.Entidade,
            EntidadeId = log.EntidadeId,
            Descricao = log.Descricao,
            EnderecoIP = log.EnderecoIP,
            Erro = log.Erro,
            TenantNome = log.TenantNome
        };
    }

    #endregion
}
