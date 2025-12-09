using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services
{
    /// <summary>
    /// Interface do serviço de auditoria.
    /// </summary>
    public interface ILogAuditoriaService
    {
        // Registro de logs
        Task LogAsync(LogAuditoriaCreateDto dto);
        Task LogLoginAsync(int usuarioCodigo, string usuarioNome, string grupo, string ip, string? userAgent = null);
        Task LogLogoutAsync(int usuarioCodigo, string usuarioNome, string grupo, string ip);
        Task LogCriacaoAsync<T>(string modulo, string entidade, string entidadeId, T dados, int usuarioCodigo, string usuarioNome, string grupo, string? ip = null);
        Task LogAlteracaoAsync<T>(string modulo, string entidade, string entidadeId, T dadosAnteriores, T dadosNovos, int usuarioCodigo, string usuarioNome, string grupo, string? ip = null);
        Task LogExclusaoAsync<T>(string modulo, string entidade, string entidadeId, T dadosExcluidos, int usuarioCodigo, string usuarioNome, string grupo, string? ip = null);
        Task LogConsultaAsync(string modulo, string entidade, string? entidadeId, int usuarioCodigo, string usuarioNome, string grupo, string? ip = null);
        Task LogErroAsync(string modulo, string entidade, string mensagem, Exception? ex, int usuarioCodigo, string usuarioNome, string grupo, string? ip = null);
        Task LogAcaoCustomAsync(string tipoAcao, string modulo, string entidade, string descricao, int usuarioCodigo, string usuarioNome, string grupo, string? ip = null);
        
        // Consulta de logs
        Task<LogAuditoriaPagedResult> GetLogsAsync(LogAuditoriaFiltroDto filtro);
        Task<LogAuditoriaDetalheDto?> GetLogDetalheAsync(long id);
        Task<LogAuditoriaEstatisticasDto> GetEstatisticasAsync(DateTime? dataInicio, DateTime? dataFim);
        Task<IEnumerable<LogAuditoriaListDto>> GetLogsByEntidadeAsync(string entidade, string entidadeId);
        Task<IEnumerable<LogAuditoriaListDto>> GetLogsByUsuarioAsync(int usuarioCodigo, int limite = 100);
        
        // Manutenção
        Task<int> LimparLogsAntigosAsync(int diasParaManter = 90);
    }
}
