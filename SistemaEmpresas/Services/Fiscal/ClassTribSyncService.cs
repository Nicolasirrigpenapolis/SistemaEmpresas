using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;
using SistemaEmpresas.Repositories.Fiscal;
using System.Text.Json;

namespace SistemaEmpresas.Services.Fiscal;

/// <summary>
/// Serviço de Sincronização ClassTrib SVRS
/// Orquestra o processo: API SVRS → Transformação → Tabela ClassTrib
/// </summary>
public class ClassTribSyncService
{
    private readonly ClassTribApiClient _apiClient;
    private readonly IClassTribRepository _classTribRepository;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ClassTribSyncService> _logger;

    // Chave do cache
    private const string CACHE_KEY_LAST_SYNC = "ClassTrib:LastSync";
    private const string CACHE_KEY_CLASSIFICACOES = "ClassTrib:Classificacoes";
    private const int CACHE_EXPIRATION_HOURS = 24; // Cache de 24 horas

    public ClassTribSyncService(
        ClassTribApiClient apiClient,
        IClassTribRepository classTribRepository,
        IDistributedCache cache,
        ILogger<ClassTribSyncService> logger)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _classTribRepository = classTribRepository ?? throw new ArgumentNullException(nameof(classTribRepository));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Sincroniza classificações tributárias da API SVRS com o banco de dados
    /// FOCO NFe - Importa apenas classificações válidas para NFe
    /// </summary>
    /// <param name="forcarAtualizacao">Se true, ignora cache e força nova consulta à API</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da sincronização com estatísticas</returns>
    public async Task<SyncResultDto> SincronizarClassificacoesNFeAsync(
        bool forcarAtualizacao = false,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Iniciando sincronização ClassTrib SVRS. Forçar atualização: {Forcar}", forcarAtualizacao);

        var resultado = new SyncResultDto
        {
            DataHoraInicio = DateTime.Now
        };

        try
        {
            // 1. Verificar última sincronização
            if (!forcarAtualizacao)
            {
                var ultimaSync = await GetUltimaSincronizacaoAsync();
                if (ultimaSync.HasValue && (DateTime.Now - ultimaSync.Value).TotalHours < 1)
                {
                    _logger.LogInformation("Sincronização recente encontrada. Usando cache");
                    resultado.Mensagem = "Sincronização realizada há menos de 1 hora. Use forçar=true para atualizar";
                    resultado.Sucesso = false;
                    return resultado;
                }
            }

            // 2. Buscar TODAS as classificações da API SVRS (para obter CST junto)
            _logger.LogInformation("Consultando API SVRS para classificações");
            var todosCSTs = await _apiClient.BuscarTodasClassificacoesAsync(cancellationToken);

            if (todosCSTs == null || !todosCSTs.Any())
            {
                _logger.LogWarning("API SVRS retornou lista vazia de classificações");
                resultado.Mensagem = "Nenhuma classificação encontrada na API SVRS";
                resultado.Sucesso = false;
                return resultado;
            }

            // 3. Transformar DTOs em Models ClassTrib (filtrando NFe)
            var classTribs = TransformarCstResponsesParaClassTribs(todosCSTs);
            
            _logger.LogInformation("API SVRS retornou {Total} ClassTribs válidos para NFe", classTribs.Count);
            resultado.TotalApiSvrs = classTribs.Count;

            // 4. Persistir na tabela ClassTrib (Bulk Upsert)
            _logger.LogInformation("Iniciando persistência em lote na tabela ClassTrib");
            var totalProcessado = await _classTribRepository.BulkUpsertAsync(classTribs);

            resultado.TotalProcessado = totalProcessado;
            resultado.Sucesso = true;
            resultado.DataHoraFim = DateTime.Now;
            resultado.TempoDecorrido = resultado.DataHoraFim - resultado.DataHoraInicio;
            resultado.Mensagem = $"Sincronização concluída com sucesso. {totalProcessado} classificações processadas";

            // 5. Atualizar cache
            await AtualizarCacheUltimaSincronizacaoAsync();

            _logger.LogInformation(
                "Sincronização concluída. Total processado: {Total}, Tempo: {Tempo}s",
                totalProcessado,
                resultado.TempoDecorrido.TotalSeconds);

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante sincronização ClassTrib SVRS");
            resultado.Sucesso = false;
            resultado.DataHoraFim = DateTime.Now;
            resultado.TempoDecorrido = resultado.DataHoraFim - resultado.DataHoraInicio;
            resultado.Mensagem = $"Erro na sincronização: {ex.Message}";
            throw;
        }
    }

    /// <summary>
    /// Sincroniza uma única classificação por cClassTrib
    /// </summary>
    public async Task<ClassTrib?> SincronizarPorCodigoClassTribAsync(
        string cClassTrib,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cClassTrib))
            throw new ArgumentException("cClassTrib não pode ser vazio", nameof(cClassTrib));

        _logger.LogInformation("Sincronizando classificação individual. cClassTrib: {cClassTrib}", cClassTrib);

        try
        {
            // Buscar na API
            var classificacaoApi = await _apiClient.BuscarPorCodigoClassTribAsync(cClassTrib, cancellationToken);

            if (classificacaoApi == null)
            {
                _logger.LogWarning("cClassTrib não encontrado na API SVRS: {cClassTrib}", cClassTrib);
                return null;
            }

            // Validar se é NFe
            if (!classificacaoApi.ValidoParaNFe)
            {
                _logger.LogWarning("cClassTrib {cClassTrib} não é válido para NFe. Ignorando", cClassTrib);
                return null;
            }

            // Transformar e persistir
            var model = TransformarDtoParaClassTrib(classificacaoApi, null);
            var resultado = await _classTribRepository.UpsertAsync(model);

            _logger.LogInformation("Classificação {cClassTrib} sincronizada com sucesso", cClassTrib);
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao sincronizar cClassTrib: {cClassTrib}", cClassTrib);
            throw;
        }
    }

    /// <summary>
    /// Verifica o status da última sincronização
    /// </summary>
    public async Task<SyncStatusDto> GetStatusSincronizacaoAsync()
    {
        var ultimaSync = await GetUltimaSincronizacaoAsync();
        var totalBanco = (await _classTribRepository.GetValidosNFeAsync()).Count;

        return new SyncStatusDto
        {
            DataUltimaSincronizacao = ultimaSync,
            TotalClassificacoesApiSvrs = totalBanco,
            CacheAtivo = ultimaSync.HasValue && (DateTime.Now - ultimaSync.Value).TotalHours < CACHE_EXPIRATION_HOURS,
            ProximaSincronizacaoRecomendada = ultimaSync?.AddHours(CACHE_EXPIRATION_HOURS)
        };
    }

    #region Métodos Privados - Transformação

    /// <summary>
    /// Transforma lista de CstResponse completos em lista de ClassTribs
    /// </summary>
    private List<ClassTrib> TransformarCstResponsesParaClassTribs(List<ClassTribApiResponse> cstResponses)
    {
        _logger.LogInformation("Transformando {Total} CST Responses em ClassTribs", cstResponses.Count);

        var classTribs = new List<ClassTrib>();

        foreach (var cstResponse in cstResponses)
        {
            var items = cstResponse.ClassificacoesTributarias
                .Where(c => c.ValidoParaNFe) // FILTRO NFe
                .Select(dto => TransformarDtoParaClassTrib(dto, cstResponse.CodigoSituacaoTributaria))
                .ToList();

            classTribs.AddRange(items);
        }

        _logger.LogInformation("Transformação concluída. Total ClassTribs: {Total}", classTribs.Count);
        return classTribs;
    }

    /// <summary>
    /// Transforma um DTO API em Model ClassTrib (inclui CST do contexto)
    /// </summary>
    private ClassTrib TransformarDtoParaClassTrib(ClassificacaoTributariaApiDto dto, string? cst)
    {
        return new ClassTrib
        {
            CodigoClassTrib = dto.CodigoClassificacaoTributaria,
            CodigoSituacaoTributaria = cst ?? string.Empty,
            DescricaoClassTrib = dto.DescricaoCompleta,
            PercentualReducaoIBS = dto.PercentualReducaoIBS,
            PercentualReducaoCBS = dto.PercentualReducaoCBS,
            ValidoParaNFe = dto.ValidoParaNFe,
            TributacaoRegular = dto.TributacaoRegular,
            CreditoPresumidoOperacoes = dto.CreditoPresumidoOperacoes,
            EstornoCredito = dto.EstornoCredito,
            TipoAliquota = dto.TipoAliquota,
            AnexoLegislacao = dto.NumeroAnexoLegislacao,
            LinkLegislacao = dto.LinkLegislacao,
            DataSincronizacao = DateTime.Now,
            Ativo = true
        };
    }

    #endregion

    #region Métodos Privados - Cache

    /// <summary>
    /// Obtém data/hora da última sincronização do cache
    /// </summary>
    private async Task<DateTime?> GetUltimaSincronizacaoAsync()
    {
        try
        {
            var cacheValue = await _cache.GetStringAsync(CACHE_KEY_LAST_SYNC);
            if (string.IsNullOrWhiteSpace(cacheValue))
                return null;

            return DateTime.Parse(cacheValue);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao ler cache de última sincronização");
            return null;
        }
    }

    /// <summary>
    /// Atualiza cache com data/hora atual da sincronização
    /// </summary>
    private async Task AtualizarCacheUltimaSincronizacaoAsync()
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CACHE_EXPIRATION_HOURS)
            };

            await _cache.SetStringAsync(
                CACHE_KEY_LAST_SYNC,
                DateTime.Now.ToString("O"),
                options);

            _logger.LogInformation("Cache de última sincronização atualizado");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao atualizar cache de última sincronização");
        }
    }

    #endregion
}

#region DTOs de Resultado

/// <summary>
/// DTO de resultado da sincronização
/// </summary>
public class SyncResultDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public int TotalApiSvrs { get; set; }
    public int TotalProcessado { get; set; }
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public TimeSpan TempoDecorrido { get; set; }
}

/// <summary>
/// DTO de status da sincronização
/// </summary>
public class SyncStatusDto
{
    public DateTime? DataUltimaSincronizacao { get; set; }
    public int TotalClassificacoesApiSvrs { get; set; }
    public bool CacheAtivo { get; set; }
    public DateTime? ProximaSincronizacaoRecomendada { get; set; }
}

#endregion
