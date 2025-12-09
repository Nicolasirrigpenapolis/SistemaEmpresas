using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories
{
    public interface ILogAuditoriaRepository
    {
        Task<LogAuditoria> CreateAsync(LogAuditoria log);
        Task<LogAuditoria?> GetByIdAsync(long id);
        Task<LogAuditoriaPagedResult> GetPagedAsync(LogAuditoriaFiltroDto filtro);
        Task<LogAuditoriaEstatisticasDto> GetEstatisticasAsync(DateTime? dataInicio, DateTime? dataFim);
        Task<IEnumerable<LogAuditoria>> GetByEntidadeAsync(string entidade, string entidadeId);
        Task<IEnumerable<LogAuditoria>> GetByUsuarioAsync(int usuarioCodigo, int limite = 100);
        Task<int> LimparLogsAntigosAsync(int diasParaManter = 90);
    }
}
