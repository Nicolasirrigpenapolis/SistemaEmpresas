using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;
using System.Text.Json;

namespace SistemaEmpresas.Repositories;

/// <summary>
/// Repositório para operações de Log de Auditoria.
/// </summary>
public class LogAuditoriaRepository : ILogAuditoriaRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<LogAuditoriaRepository> _logger;

    public LogAuditoriaRepository(AppDbContext context, ILogger<LogAuditoriaRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Cria um novo registro de log de auditoria.
    /// </summary>
    public async Task<LogAuditoria> CreateAsync(LogAuditoria log)
    {
        try
        {
            // Usar SQL direto para inserir na tabela com SqlParameter para valores nulos
            var sql = @"
                INSERT INTO [dbo].[LogsAuditoria] 
                ([DataHora], [UsuarioCodigo], [UsuarioNome], [UsuarioGrupo], [TipoAcao], [Modulo], 
                 [Entidade], [EntidadeId], [Descricao], [DadosAnteriores], [DadosNovos], [CamposAlterados],
                 [EnderecoIP], [UserAgent], [MetodoHttp], [UrlRequisicao], [StatusCode], [TempoExecucaoMs],
                 [Erro], [MensagemErro], [TenantId], [TenantNome], [SessaoId], [CorrelationId])
                OUTPUT INSERTED.Id
                VALUES 
                (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23)";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@p0", log.DataHora),
                new SqlParameter("@p1", log.UsuarioCodigo),
                new SqlParameter("@p2", (object?)log.UsuarioNome ?? DBNull.Value),
                new SqlParameter("@p3", (object?)log.UsuarioGrupo ?? DBNull.Value),
                new SqlParameter("@p4", (object?)log.TipoAcao ?? DBNull.Value),
                new SqlParameter("@p5", (object?)log.Modulo ?? DBNull.Value),
                new SqlParameter("@p6", (object?)log.Entidade ?? DBNull.Value),
                new SqlParameter("@p7", (object?)log.EntidadeId ?? DBNull.Value),
                new SqlParameter("@p8", (object?)log.Descricao ?? DBNull.Value),
                new SqlParameter("@p9", (object?)log.DadosAnteriores ?? DBNull.Value),
                new SqlParameter("@p10", (object?)log.DadosNovos ?? DBNull.Value),
                new SqlParameter("@p11", (object?)log.CamposAlterados ?? DBNull.Value),
                new SqlParameter("@p12", (object?)log.EnderecoIP ?? DBNull.Value),
                new SqlParameter("@p13", (object?)log.UserAgent ?? DBNull.Value),
                new SqlParameter("@p14", (object?)log.MetodoHttp ?? DBNull.Value),
                new SqlParameter("@p15", (object?)log.UrlRequisicao ?? DBNull.Value),
                new SqlParameter("@p16", (object?)log.StatusCode ?? DBNull.Value),
                new SqlParameter("@p17", (object?)log.TempoExecucaoMs ?? DBNull.Value),
                new SqlParameter("@p18", log.Erro),
                new SqlParameter("@p19", (object?)log.MensagemErro ?? DBNull.Value),
                new SqlParameter("@p20", (object?)log.TenantId ?? DBNull.Value),
                new SqlParameter("@p21", (object?)log.TenantNome ?? DBNull.Value),
                new SqlParameter("@p22", (object?)log.SessaoId ?? DBNull.Value),
                new SqlParameter("@p23", (object?)log.CorrelationId ?? DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            return log;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar log de auditoria: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Busca um log pelo ID.
    /// </summary>
    public async Task<LogAuditoria?> GetByIdAsync(long id)
    {
        try
        {
            var sql = @"
                SELECT Id, DataHora, UsuarioCodigo, UsuarioNome, UsuarioGrupo, TipoAcao, Modulo,
                       Entidade, EntidadeId, Descricao, DadosAnteriores, DadosNovos, CamposAlterados,
                       EnderecoIP, UserAgent, MetodoHttp, UrlRequisicao, StatusCode, TempoExecucaoMs,
                       Erro, MensagemErro, TenantId, TenantNome, SessaoId, CorrelationId
                FROM [dbo].[LogsAuditoria]
                WHERE Id = @p0";

            var logs = await ExecuteQueryAsync(sql, id);
            return logs.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar log por ID {Id}: {Message}", id, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Busca logs paginados com filtros.
    /// </summary>
    public async Task<LogAuditoriaPagedResult> GetPagedAsync(LogAuditoriaFiltroDto filtro)
    {
        try
        {
            var whereClause = BuildWhereClause(filtro);
            var orderBy = BuildOrderBy(filtro);

            var connection = _context.Database.GetDbConnection();
            var wasOpen = connection.State == System.Data.ConnectionState.Open;
            
            if (!wasOpen)
                await connection.OpenAsync();

            try
            {
                // Contar total
                var countSql = $@"
                    SELECT COUNT(*) AS Total
                    FROM [dbo].[LogsAuditoria] l
                    {whereClause}";

                var totalItems = 0;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = countSql;
                    var result = await command.ExecuteScalarAsync();
                    totalItems = Convert.ToInt32(result);
                }

                // Buscar dados paginados
                var offset = (filtro.Pagina - 1) * filtro.ItensPorPagina;
                var dataSql = $@"
                    SELECT Id, DataHora, UsuarioCodigo, UsuarioNome, UsuarioGrupo, TipoAcao, Modulo,
                           Entidade, EntidadeId, Descricao, DadosAnteriores, DadosNovos, CamposAlterados,
                           EnderecoIP, UserAgent, MetodoHttp, UrlRequisicao, StatusCode, TempoExecucaoMs,
                           Erro, MensagemErro, TenantId, TenantNome, SessaoId, CorrelationId
                    FROM [dbo].[LogsAuditoria] l
                    {whereClause}
                    {orderBy}
                    OFFSET {offset} ROWS FETCH NEXT {filtro.ItensPorPagina} ROWS ONLY";

                var logs = new List<LogAuditoria>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = dataSql;
                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        logs.Add(MapFromReader(reader));
                    }
                }

                var items = logs.Select(MapToListDto).ToList();

                return new LogAuditoriaPagedResult
                {
                    Items = items,
                    TotalItems = totalItems,
                    PaginaAtual = filtro.Pagina,
                    ItensPorPagina = filtro.ItensPorPagina,
                    TotalPaginas = (int)Math.Ceiling((double)totalItems / filtro.ItensPorPagina)
                };
            }
            finally
            {
                if (!wasOpen)
                    await connection.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar logs paginados: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Obtém estatísticas dos logs.
    /// </summary>
    public async Task<LogAuditoriaEstatisticasDto> GetEstatisticasAsync(DateTime? dataInicio, DateTime? dataFim)
    {
        try
        {
            var inicio = dataInicio ?? DateTime.UtcNow.AddDays(-30);
            var fim = dataFim ?? DateTime.UtcNow;

            var stats = new LogAuditoriaEstatisticasDto
            {
                DataInicio = inicio,
                DataFim = fim,
                AcoesPorTipo = new List<LogPorTipoDto>(),
                AcoesPorModulo = new List<LogPorModuloDto>(),
                TopUsuarios = new List<LogPorUsuarioDto>(),
                AcoesPorDia = new List<LogPorDiaDto>()
            };

            var connection = _context.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            // Total de ações
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT COUNT(*) FROM [dbo].[LogsAuditoria] WHERE DataHora >= @inicio AND DataHora <= @fim";
                var p1 = cmd.CreateParameter(); p1.ParameterName = "@inicio"; p1.Value = inicio; cmd.Parameters.Add(p1);
                var p2 = cmd.CreateParameter(); p2.ParameterName = "@fim"; p2.Value = fim; cmd.Parameters.Add(p2);
                stats.TotalAcoes = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);
            }

            // Usuários ativos
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT COUNT(DISTINCT UsuarioCodigo) FROM [dbo].[LogsAuditoria] WHERE DataHora >= @inicio AND DataHora <= @fim";
                var p1 = cmd.CreateParameter(); p1.ParameterName = "@inicio"; p1.Value = inicio; cmd.Parameters.Add(p1);
                var p2 = cmd.CreateParameter(); p2.ParameterName = "@fim"; p2.Value = fim; cmd.Parameters.Add(p2);
                stats.UsuariosAtivos = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);
            }

            // Erros
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT COUNT(*) FROM [dbo].[LogsAuditoria] WHERE Erro = 1 AND DataHora >= @inicio AND DataHora <= @fim";
                var p1 = cmd.CreateParameter(); p1.ParameterName = "@inicio"; p1.Value = inicio; cmd.Parameters.Add(p1);
                var p2 = cmd.CreateParameter(); p2.ParameterName = "@fim"; p2.Value = fim; cmd.Parameters.Add(p2);
                stats.TotalErros = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);
            }

            // Ações por tipo
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TipoAcao, COUNT(*) AS Quantidade FROM [dbo].[LogsAuditoria] 
                    WHERE DataHora >= @inicio AND DataHora <= @fim GROUP BY TipoAcao ORDER BY COUNT(*) DESC";
                var p1 = cmd.CreateParameter(); p1.ParameterName = "@inicio"; p1.Value = inicio; cmd.Parameters.Add(p1);
                var p2 = cmd.CreateParameter(); p2.ParameterName = "@fim"; p2.Value = fim; cmd.Parameters.Add(p2);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    stats.AcoesPorTipo.Add(new LogPorTipoDto 
                    { 
                        TipoAcao = reader.GetString(0), 
                        Quantidade = reader.GetInt32(1) 
                    });
                }
            }

            // Ações por módulo
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT Modulo, COUNT(*) AS Quantidade FROM [dbo].[LogsAuditoria] 
                    WHERE DataHora >= @inicio AND DataHora <= @fim GROUP BY Modulo ORDER BY COUNT(*) DESC";
                var p1 = cmd.CreateParameter(); p1.ParameterName = "@inicio"; p1.Value = inicio; cmd.Parameters.Add(p1);
                var p2 = cmd.CreateParameter(); p2.ParameterName = "@fim"; p2.Value = fim; cmd.Parameters.Add(p2);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    stats.AcoesPorModulo.Add(new LogPorModuloDto 
                    { 
                        Modulo = reader.GetString(0), 
                        Quantidade = reader.GetInt32(1) 
                    });
                }
            }

            // Top usuários
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP 10 UsuarioNome, COUNT(*) AS Quantidade FROM [dbo].[LogsAuditoria] 
                    WHERE DataHora >= @inicio AND DataHora <= @fim GROUP BY UsuarioNome ORDER BY COUNT(*) DESC";
                var p1 = cmd.CreateParameter(); p1.ParameterName = "@inicio"; p1.Value = inicio; cmd.Parameters.Add(p1);
                var p2 = cmd.CreateParameter(); p2.ParameterName = "@fim"; p2.Value = fim; cmd.Parameters.Add(p2);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    stats.TopUsuarios.Add(new LogPorUsuarioDto 
                    { 
                        UsuarioNome = reader.GetString(0), 
                        Quantidade = reader.GetInt32(1) 
                    });
                }
            }

            // Ações por dia
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT CAST(DataHora AS DATE) AS Data, COUNT(*) AS Quantidade FROM [dbo].[LogsAuditoria] 
                    WHERE DataHora >= @inicio AND DataHora <= @fim GROUP BY CAST(DataHora AS DATE) ORDER BY Data";
                var p1 = cmd.CreateParameter(); p1.ParameterName = "@inicio"; p1.Value = inicio; cmd.Parameters.Add(p1);
                var p2 = cmd.CreateParameter(); p2.ParameterName = "@fim"; p2.Value = fim; cmd.Parameters.Add(p2);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    stats.AcoesPorDia.Add(new LogPorDiaDto 
                    { 
                        Data = reader.GetDateTime(0), 
                        Quantidade = reader.GetInt32(1) 
                    });
                }
            }

            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar estatísticas: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Busca logs por entidade específica.
    /// </summary>
    public async Task<IEnumerable<LogAuditoria>> GetByEntidadeAsync(string entidade, string entidadeId)
    {
        try
        {
            var sql = @"
                SELECT TOP 100 Id, DataHora, UsuarioCodigo, UsuarioNome, UsuarioGrupo, TipoAcao, Modulo,
                       Entidade, EntidadeId, Descricao, DadosAnteriores, DadosNovos, CamposAlterados,
                       EnderecoIP, UserAgent, MetodoHttp, UrlRequisicao, StatusCode, TempoExecucaoMs,
                       Erro, MensagemErro, TenantId, TenantNome, SessaoId, CorrelationId
                FROM [dbo].[LogsAuditoria]
                WHERE Entidade = @p0 AND EntidadeId = @p1
                ORDER BY DataHora DESC";

            return await ExecuteQueryAsync(sql, entidade, entidadeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar logs por entidade: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Busca logs por usuário.
    /// </summary>
    public async Task<IEnumerable<LogAuditoria>> GetByUsuarioAsync(int usuarioCodigo, int limite = 100)
    {
        try
        {
            var sql = $@"
                SELECT TOP {limite} Id, DataHora, UsuarioCodigo, UsuarioNome, UsuarioGrupo, TipoAcao, Modulo,
                       Entidade, EntidadeId, Descricao, DadosAnteriores, DadosNovos, CamposAlterados,
                       EnderecoIP, UserAgent, MetodoHttp, UrlRequisicao, StatusCode, TempoExecucaoMs,
                       Erro, MensagemErro, TenantId, TenantNome, SessaoId, CorrelationId
                FROM [dbo].[LogsAuditoria]
                WHERE UsuarioCodigo = @p0
                ORDER BY DataHora DESC";

            return await ExecuteQueryAsync(sql, usuarioCodigo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar logs por usuário: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Limpa logs antigos mantendo os últimos X dias.
    /// </summary>
    public async Task<int> LimparLogsAntigosAsync(int diasParaManter = 90)
    {
        try
        {
            var dataLimite = DateTime.UtcNow.AddDays(-diasParaManter);
            var sql = @"DELETE FROM [dbo].[LogsAuditoria] WHERE DataHora < @p0";
            
            return await _context.Database.ExecuteSqlRawAsync(sql, dataLimite);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao limpar logs antigos: {Message}", ex.Message);
            throw;
        }
    }

    #region Helpers

    private string BuildWhereClause(LogAuditoriaFiltroDto filtro)
    {
        var conditions = new List<string>();

        if (filtro.DataInicio.HasValue)
            conditions.Add($"DataHora >= '{filtro.DataInicio.Value:yyyy-MM-dd HH:mm:ss}'");

        if (filtro.DataFim.HasValue)
            conditions.Add($"DataHora <= '{filtro.DataFim.Value:yyyy-MM-dd HH:mm:ss}'");

        if (filtro.UsuarioCodigo.HasValue)
            conditions.Add($"UsuarioCodigo = {filtro.UsuarioCodigo.Value}");

        if (!string.IsNullOrEmpty(filtro.UsuarioNome))
            conditions.Add($"UsuarioNome LIKE '%{filtro.UsuarioNome.Replace("'", "''")}%'");

        if (!string.IsNullOrEmpty(filtro.TipoAcao))
            conditions.Add($"TipoAcao = '{filtro.TipoAcao.Replace("'", "''")}'");

        if (filtro.TiposAcao?.Any() == true)
        {
            var tipos = string.Join("','", filtro.TiposAcao.Select(t => t.Replace("'", "''")));
            conditions.Add($"TipoAcao IN ('{tipos}')");
        }

        if (!string.IsNullOrEmpty(filtro.Modulo))
            conditions.Add($"Modulo = '{filtro.Modulo.Replace("'", "''")}'");

        if (!string.IsNullOrEmpty(filtro.Entidade))
            conditions.Add($"Entidade = '{filtro.Entidade.Replace("'", "''")}'");

        if (!string.IsNullOrEmpty(filtro.EntidadeId))
            conditions.Add($"EntidadeId = '{filtro.EntidadeId.Replace("'", "''")}'");

        if (!string.IsNullOrEmpty(filtro.Busca))
        {
            var busca = filtro.Busca.Replace("'", "''");
            conditions.Add($"(Descricao LIKE '%{busca}%' OR UsuarioNome LIKE '%{busca}%' OR Entidade LIKE '%{busca}%')");
        }

        if (filtro.ApenasErros == true)
            conditions.Add("Erro = 1");

        return conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";
    }

    private string BuildOrderBy(LogAuditoriaFiltroDto filtro)
    {
        var campo = filtro.OrdenarPor?.ToLower() switch
        {
            "datahora" => "DataHora",
            "usuario" => "UsuarioNome",
            "tipoacao" => "TipoAcao",
            "modulo" => "Modulo",
            "entidade" => "Entidade",
            _ => "DataHora"
        };

        var direcao = filtro.OrdemDescrescente ? "DESC" : "ASC";
        return $"ORDER BY {campo} {direcao}";
    }

    private async Task<List<LogAuditoria>> ExecuteQueryAsync(string sql, params object[] parameters)
    {
        var logs = new List<LogAuditoria>();
        
        var connection = _context.Database.GetDbConnection();
        var wasOpen = connection.State == System.Data.ConnectionState.Open;
        
        if (!wasOpen)
            await connection.OpenAsync();
        
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = command.CreateParameter();
                param.ParameterName = $"@p{i}";
                param.Value = parameters[i] ?? DBNull.Value;
                command.Parameters.Add(param);
            }

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(MapFromReader(reader));
            }
        }
        finally
        {
            if (!wasOpen)
                await connection.CloseAsync();
        }

        return logs;
    }

    private static LogAuditoria MapFromReader(System.Data.Common.DbDataReader reader)
    {
        return new LogAuditoria
        {
            Id = reader.GetInt64(reader.GetOrdinal("Id")),
            DataHora = reader.GetDateTime(reader.GetOrdinal("DataHora")),
            UsuarioCodigo = reader.GetInt32(reader.GetOrdinal("UsuarioCodigo")),
            UsuarioNome = reader.GetString(reader.GetOrdinal("UsuarioNome")),
            UsuarioGrupo = reader.GetString(reader.GetOrdinal("UsuarioGrupo")),
            TipoAcao = reader.GetString(reader.GetOrdinal("TipoAcao")),
            Modulo = reader.GetString(reader.GetOrdinal("Modulo")),
            Entidade = reader.GetString(reader.GetOrdinal("Entidade")),
            EntidadeId = reader.IsDBNull(reader.GetOrdinal("EntidadeId")) ? null : reader.GetString(reader.GetOrdinal("EntidadeId")),
            Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
            DadosAnteriores = reader.IsDBNull(reader.GetOrdinal("DadosAnteriores")) ? null : reader.GetString(reader.GetOrdinal("DadosAnteriores")),
            DadosNovos = reader.IsDBNull(reader.GetOrdinal("DadosNovos")) ? null : reader.GetString(reader.GetOrdinal("DadosNovos")),
            CamposAlterados = reader.IsDBNull(reader.GetOrdinal("CamposAlterados")) ? null : reader.GetString(reader.GetOrdinal("CamposAlterados")),
            EnderecoIP = reader.IsDBNull(reader.GetOrdinal("EnderecoIP")) ? null : reader.GetString(reader.GetOrdinal("EnderecoIP")),
            UserAgent = reader.IsDBNull(reader.GetOrdinal("UserAgent")) ? null : reader.GetString(reader.GetOrdinal("UserAgent")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("MetodoHttp")) ? null : reader.GetString(reader.GetOrdinal("MetodoHttp")),
            UrlRequisicao = reader.IsDBNull(reader.GetOrdinal("UrlRequisicao")) ? null : reader.GetString(reader.GetOrdinal("UrlRequisicao")),
            StatusCode = reader.IsDBNull(reader.GetOrdinal("StatusCode")) ? null : reader.GetInt32(reader.GetOrdinal("StatusCode")),
            TempoExecucaoMs = reader.IsDBNull(reader.GetOrdinal("TempoExecucaoMs")) ? null : reader.GetInt64(reader.GetOrdinal("TempoExecucaoMs")),
            Erro = reader.GetBoolean(reader.GetOrdinal("Erro")),
            MensagemErro = reader.IsDBNull(reader.GetOrdinal("MensagemErro")) ? null : reader.GetString(reader.GetOrdinal("MensagemErro")),
            TenantId = reader.IsDBNull(reader.GetOrdinal("TenantId")) ? null : reader.GetInt32(reader.GetOrdinal("TenantId")),
            TenantNome = reader.IsDBNull(reader.GetOrdinal("TenantNome")) ? null : reader.GetString(reader.GetOrdinal("TenantNome")),
            SessaoId = reader.IsDBNull(reader.GetOrdinal("SessaoId")) ? null : reader.GetString(reader.GetOrdinal("SessaoId")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("CorrelationId")) ? null : reader.GetString(reader.GetOrdinal("CorrelationId"))
        };
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
