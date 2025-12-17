using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SistemaEmpresas.Repositories.Fiscal;
using SistemaEmpresas.Services.Common;
using SistemaEmpresas.Services.Fiscal;
using SistemaEmpresas.DTOs;

namespace SistemaEmpresas.Controllers.Fiscal;

/// <summary>
/// Controller para Classificações Tributárias (ClassTrib)
/// Endpoints para gerenciar dados da API SVRS (Reforma Tributária IBS/CBS)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassTribController : ControllerBase
{
    private readonly IClassTribRepository _repository;
    private readonly ClassTribSyncService _syncService;
    private readonly ILogger<ClassTribController> _logger;
    private readonly ICacheService _cache;

    public ClassTribController(
        IClassTribRepository repository,
        ClassTribSyncService syncService,
        ILogger<ClassTribController> logger,
        ICacheService cache)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    #region Consultas

    /// <summary>
    /// Lista todos os ClassTribs (paginado)
    /// GET api/classtrib?page=1&pageSize=50&cst=000&descricao=alíquota&somenteNFe=true
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedClassTribResponse>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? cst = null,
        [FromQuery] string? descricao = null,
        [FromQuery] bool? somenteNFe = null)
    {
        _logger.LogInformation("Listando ClassTribs. Página: {Page}, Tamanho: {PageSize}", page, pageSize);

        try
        {
            var (items, total) = await _repository.GetPagedAsync(page, pageSize, cst, descricao, somenteNFe);

            var response = new PagedClassTribResponse
            {
                Items = items.Select(c => new ClassTribDto
                {
                    Id = c.Id,
                    CodigoClassTrib = c.CodigoClassTrib,
                    CodigoSituacaoTributaria = c.CodigoSituacaoTributaria,
                    DescricaoSituacaoTributaria = c.DescricaoSituacaoTributaria,
                    DescricaoClassTrib = c.DescricaoClassTrib,
                    PercentualReducaoIBS = c.PercentualReducaoIBS,
                    PercentualReducaoCBS = c.PercentualReducaoCBS,
                    TipoAliquota = c.TipoAliquota,
                    ValidoParaNFe = c.ValidoParaNFe,
                    AnexoLegislacao = c.AnexoLegislacao,
                    LinkLegislacao = c.LinkLegislacao
                }).ToList(),
                TotalCount = total,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar ClassTribs");
            return StatusCode(500, new { error = "Erro ao listar classificações tributárias" });
        }
    }

    /// <summary>
    /// Busca ClassTrib por ID
    /// GET api/classtrib/5
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClassTribDto>> GetById(int id)
    {
        _logger.LogInformation("Buscando ClassTrib por ID: {Id}", id);

        try
        {
            var classTrib = await _repository.GetByIdAsync(id);

            if (classTrib == null)
                return NotFound(new { error = $"ClassTrib ID {id} não encontrado" });

            return Ok(MapToDto(classTrib));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar ClassTrib ID: {Id}", id);
            return StatusCode(500, new { error = "Erro ao buscar classificação tributária" });
        }
    }

    /// <summary>
    /// Busca ClassTrib por código (cClassTrib)
    /// GET api/classtrib/codigo/000001
    /// </summary>
    [HttpGet("codigo/{codigo}")]
    public async Task<ActionResult<ClassTribDto>> GetByCodigo(string codigo)
    {
        _logger.LogInformation("Buscando ClassTrib por código: {Codigo}", codigo);

        try
        {
            var classTrib = await _repository.GetByCodigoAsync(codigo);

            if (classTrib == null)
                return NotFound(new { error = $"ClassTrib código {codigo} não encontrado" });

            return Ok(MapToDto(classTrib));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar ClassTrib código: {Codigo}", codigo);
            return StatusCode(500, new { error = "Erro ao buscar classificação tributária" });
        }
    }

    /// <summary>
    /// Lista ClassTribs válidos para NFe
    /// GET api/classtrib/nfe
    /// </summary>
    [HttpGet("nfe")]
    public async Task<ActionResult<List<ClassTribDto>>> GetValidosNFe()
    {
        _logger.LogInformation("Listando ClassTribs válidos para NFe");

        try
        {
            var dtos = await _cache.GetOrCreateAsync(
                "classtrib:nfe:all",
                async () =>
                {
                    var classTribs = await _repository.GetValidosNFeAsync();
                    return classTribs.Select(MapToDto).ToList();
                },
                CacheService.CacheDurations.Long,
                CacheService.CacheDurations.Medium);

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar ClassTribs NFe");
            return StatusCode(500, new { error = "Erro ao listar classificações NFe" });
        }
    }

    /// <summary>
    /// Lista ClassTribs por CST
    /// GET api/classtrib/cst/000
    /// </summary>
    [HttpGet("cst/{cst}")]
    public async Task<ActionResult<List<ClassTribDto>>> GetByCst(string cst)
    {
        _logger.LogInformation("Listando ClassTribs por CST: {CST}", cst);

        try
        {
            var dtos = await _cache.GetOrCreateAsync(
                $"classtrib:cst:{cst}",
                async () =>
                {
                    var classTribs = await _repository.GetByCstAsync(cst);
                    return classTribs.Select(MapToDto).ToList();
                },
                CacheService.CacheDurations.Long,
                CacheService.CacheDurations.Medium);

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar ClassTribs CST: {CST}", cst);
            return StatusCode(500, new { error = "Erro ao listar classificações por CST" });
        }
    }

    /// <summary>
    /// Pesquisa ClassTribs por termo (código, descrição, CST)
    /// GET api/classtrib/search?q=alíquota&limite=50
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<List<ClassTribDto>>> Search(
        [FromQuery] string q,
        [FromQuery] int limite = 50)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { error = "Termo de busca obrigatório" });

        _logger.LogInformation("Pesquisando ClassTribs: {Termo}", q);

        try
        {
            var classTribs = await _repository.SearchAsync(q, limite);

            var dtos = classTribs.Select(MapToDto).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao pesquisar ClassTribs: {Termo}", q);
            return StatusCode(500, new { error = "Erro ao pesquisar classificações" });
        }
    }

    /// <summary>
    /// Busca para autocomplete (retorna formato simplificado)
    /// GET api/classtrib/autocomplete?q=000&limite=20
    /// </summary>
    [HttpGet("autocomplete")]
    public async Task<ActionResult<List<ClassTribAutocompleteDto>>> Autocomplete(
        [FromQuery] string q,
        [FromQuery] int limite = 20)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            return Ok(new List<ClassTribAutocompleteDto>());

        _logger.LogInformation("Autocomplete ClassTribs: {Termo}", q);

        try
        {
            var classTribs = await _repository.SearchAsync(q, limite);

            var dtos = classTribs.Select(c => new ClassTribAutocompleteDto
            {
                Id = c.Id,
                CodigoClassTrib = c.CodigoClassTrib,
                Cst = c.CodigoSituacaoTributaria,
                Descricao = c.DescricaoClassTrib.Length > 100 
                    ? c.DescricaoClassTrib.Substring(0, 100) + "..." 
                    : c.DescricaoClassTrib,
                DisplayText = $"{c.CodigoClassTrib} - {c.CodigoSituacaoTributaria} - {(c.DescricaoClassTrib.Length > 60 ? c.DescricaoClassTrib.Substring(0, 60) + "..." : c.DescricaoClassTrib)}"
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no autocomplete ClassTribs: {Termo}", q);
            return StatusCode(500, new { error = "Erro ao pesquisar classificações" });
        }
    }

    /// <summary>
    /// Filtros avançados de ClassTrib
    /// GET api/classtrib/filtros/avancado?page=1&pageSize=50&csts=000,200,410&tipoAliquota=Padrão&minReducaoIBS=50&maxReducaoIBS=100&validoNFe=true
    /// </summary>
    [HttpGet("filtros/avancado")]
    public async Task<ActionResult<PagedClassTribResponse>> FiltroAvancado(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? csts = null,
        [FromQuery] string? tipoAliquota = null,
        [FromQuery] decimal? minReducaoIBS = null,
        [FromQuery] decimal? maxReducaoIBS = null,
        [FromQuery] decimal? minReducaoCBS = null,
        [FromQuery] decimal? maxReducaoCBS = null,
        [FromQuery] bool? validoNFe = null,
        [FromQuery] bool? tributacaoRegular = null,
        [FromQuery] bool? creditoPresumido = null,
        [FromQuery] string? descricao = null,
        [FromQuery] string? ordenarPor = null)
    {
        _logger.LogInformation("Filtro avançado ClassTribs. Página: {Page}", page);

        try
        {
            var cstList = !string.IsNullOrEmpty(csts) 
                ? csts.Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() 
                : null;

            var (items, total) = await _repository.GetPagedAdvancedAsync(
                pageNumber: page,
                pageSize: pageSize,
                csts: cstList,
                tipoAliquota: tipoAliquota,
                minReducaoIBS: minReducaoIBS,
                maxReducaoIBS: maxReducaoIBS,
                minReducaoCBS: minReducaoCBS,
                maxReducaoCBS: maxReducaoCBS,
                validoNFe: validoNFe,
                tributacaoRegular: tributacaoRegular,
                creditoPresumido: creditoPresumido,
                descricao: descricao,
                ordenarPor: ordenarPor
            );

            var response = new PagedClassTribResponse
            {
                Items = items.Select(MapToDto).ToList(),
                TotalCount = total,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro em filtro avançado de ClassTribs");
            return StatusCode(500, new { error = "Erro ao aplicar filtros" });
        }
    }

    /// <summary>
    /// Lista tipos de alíquota disponíveis
    /// GET api/classtrib/filtros/tipos-aliquota
    /// </summary>
    [HttpGet("filtros/tipos-aliquota")]
    public async Task<ActionResult<List<string>>> GetTiposAliquota()
    {
        _logger.LogInformation("Listando tipos de alíquota disponíveis");

        try
        {
            var tipos = await _repository.GetTiposAliquotaAsync();
            return Ok(tipos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tipos de alíquota");
            return StatusCode(500, new { error = "Erro ao listar tipos de alíquota" });
        }
    }

    /// <summary>
    /// Lista CSTs disponíveis
    /// GET api/classtrib/filtros/csts
    /// </summary>
    [HttpGet("filtros/csts")]
    public async Task<ActionResult<List<CstOption>>> GetCsts()
    {
        _logger.LogInformation("Listando CSTs disponíveis");

        try
        {
            var csts = await _repository.GetCstsAsync();
            return Ok(csts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar CSTs");
            return StatusCode(500, new { error = "Erro ao listar CSTs" });
        }
    }

    /// <summary>
    /// Estatísticas de ClassTrib
    /// GET api/classtrib/filtros/estatisticas
    /// </summary>
    [HttpGet("filtros/estatisticas")]
    public async Task<ActionResult<ClassTribEstatisticas>> GetEstatisticas()
    {
        _logger.LogInformation("Consultando estatísticas de ClassTrib");

        try
        {
            var stats = await _repository.GetEstatisticasAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas");
            return StatusCode(500, new { error = "Erro ao obter estatísticas" });
        }
    }

    #endregion

    #region Sincronização

    /// <summary>
    /// Sincroniza ClassTribs com API SVRS
    /// POST api/classtrib/sync?forcar=false
    /// </summary>
    [HttpPost("sync")]
    public async Task<ActionResult<SyncResultDto>> Sincronizar([FromQuery] bool forcar = false)
    {
        _logger.LogInformation("Iniciando sincronização ClassTrib. Forçar: {Forcar}", forcar);

        try
        {
            var resultado = await _syncService.SincronizarClassificacoesNFeAsync(forcar);

            if (resultado.Sucesso)
                return Ok(resultado);
            else
                return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na sincronização ClassTrib");
            return StatusCode(500, new { error = "Erro na sincronização: " + ex.Message });
        }
    }

    /// <summary>
    /// Verifica status da última sincronização
    /// GET api/classtrib/sync/status
    /// </summary>
    [HttpGet("sync/status")]
    public async Task<ActionResult<SyncStatusDto>> GetSyncStatus()
    {
        _logger.LogInformation("Consultando status de sincronização");

        try
        {
            var status = await _syncService.GetStatusSincronizacaoAsync();
            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar status de sincronização");
            return StatusCode(500, new { error = "Erro ao consultar status" });
        }
    }

    /// <summary>
    /// Sincronização alternativa: recebe dados da API SVRS via frontend
    /// Útil quando a API SVRS bloqueia requisições do servidor (403)
    /// POST api/classtrib/sync/import
    /// </summary>
    [HttpPost("sync/import")]
    public async Task<ActionResult<SyncResultDto>> ImportarDadosApiSvrs([FromBody] List<ClassTribApiResponse> dadosApi)
    {
        _logger.LogInformation("Importando dados da API SVRS via frontend. Total CSTs recebidos: {Total}", dadosApi?.Count ?? 0);

        var resultado = new SyncResultDto
        {
            DataHoraInicio = DateTime.Now
        };

        try
        {
            if (dadosApi == null || !dadosApi.Any())
            {
                resultado.Mensagem = "Nenhum dado recebido para importação";
                resultado.Sucesso = false;
                return BadRequest(resultado);
            }

            // Transformar dados recebidos em ClassTribs
            var classTribs = new List<Models.ClassTrib>();

            foreach (var cstResponse in dadosApi)
            {
                var items = cstResponse.ClassificacoesTributarias
                    .Where(c => c.ValidoParaNFe)
                    .Select(dto => new Models.ClassTrib
                    {
                        CodigoClassTrib = dto.CodigoClassificacaoTributaria,
                        CodigoSituacaoTributaria = cstResponse.CodigoSituacaoTributaria,
                        DescricaoSituacaoTributaria = cstResponse.DescricaoSituacaoTributaria,
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
                    })
                    .ToList();

                classTribs.AddRange(items);
            }

            _logger.LogInformation("Total de ClassTribs para importar: {Total}", classTribs.Count);
            resultado.TotalApiSvrs = classTribs.Count;

            // Persistir na tabela ClassTrib (Bulk Upsert)
            var totalProcessado = await _repository.BulkUpsertAsync(classTribs);

            resultado.TotalProcessado = totalProcessado;
            resultado.Sucesso = true;
            resultado.DataHoraFim = DateTime.Now;
            resultado.TempoDecorrido = resultado.DataHoraFim - resultado.DataHoraInicio;
            resultado.Mensagem = $"Importação concluída com sucesso. {totalProcessado} classificações processadas";

            _logger.LogInformation(
                "Importação concluída. Total processado: {Total}, Tempo: {Tempo}s",
                totalProcessado,
                resultado.TempoDecorrido.TotalSeconds);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na importação de dados SVRS");
            resultado.Sucesso = false;
            resultado.DataHoraFim = DateTime.Now;
            resultado.TempoDecorrido = resultado.DataHoraFim - resultado.DataHoraInicio;
            resultado.Mensagem = $"Erro na importação: {ex.Message}";
            return StatusCode(500, resultado);
        }
    }

    #endregion

    #region Helpers

    private static ClassTribDto MapToDto(Models.ClassTrib classTrib)
    {
        return new ClassTribDto
        {
            Id = classTrib.Id,
            CodigoClassTrib = classTrib.CodigoClassTrib,
            CodigoSituacaoTributaria = classTrib.CodigoSituacaoTributaria,
            DescricaoSituacaoTributaria = classTrib.DescricaoSituacaoTributaria,
            DescricaoClassTrib = classTrib.DescricaoClassTrib,
            PercentualReducaoIBS = classTrib.PercentualReducaoIBS,
            PercentualReducaoCBS = classTrib.PercentualReducaoCBS,
            TipoAliquota = classTrib.TipoAliquota,
            ValidoParaNFe = classTrib.ValidoParaNFe,
            TributacaoRegular = classTrib.TributacaoRegular,
            CreditoPresumidoOperacoes = classTrib.CreditoPresumidoOperacoes,
            EstornoCredito = classTrib.EstornoCredito,
            AnexoLegislacao = classTrib.AnexoLegislacao,
            LinkLegislacao = classTrib.LinkLegislacao
        };
    }

    #endregion
}

#region DTOs de Resposta

/// <summary>
/// DTO para resposta paginada de ClassTrib
/// </summary>
public class PagedClassTribResponse
{
    public List<ClassTribDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

/// <summary>
/// DTO detalhado de ClassTrib
/// </summary>
public class ClassTribDto
{
    public int Id { get; set; }
    public string CodigoClassTrib { get; set; } = string.Empty;
    public string CodigoSituacaoTributaria { get; set; } = string.Empty;
    public string? DescricaoSituacaoTributaria { get; set; }
    public string DescricaoClassTrib { get; set; } = string.Empty;
    public decimal PercentualReducaoIBS { get; set; }
    public decimal PercentualReducaoCBS { get; set; }
    public string? TipoAliquota { get; set; }
    public bool ValidoParaNFe { get; set; }
    public bool TributacaoRegular { get; set; }
    public bool CreditoPresumidoOperacoes { get; set; }
    public bool EstornoCredito { get; set; }
    public int? AnexoLegislacao { get; set; }
    public string? LinkLegislacao { get; set; }
}

/// <summary>
/// DTO simplificado para autocomplete
/// </summary>
public class ClassTribAutocompleteDto
{
    public int Id { get; set; }
    public string CodigoClassTrib { get; set; } = string.Empty;
    public string Cst { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string DisplayText { get; set; } = string.Empty;
}

/// <summary>
/// Opção de CST para dropdown
/// </summary>
public class CstOption
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Total { get; set; }
}

/// <summary>
/// Estatísticas de ClassTrib
/// </summary>
public class ClassTribEstatisticas
{
    public int TotalClassificacoes { get; set; }
    public int TotalValidoNFe { get; set; }
    public int TotalPorCST { get; set; }
    public Dictionary<string, int> ClassificacoesPorTipo { get; set; } = new();
    public Dictionary<string, int> ClassificacoesPorCST { get; set; } = new();
    public decimal MediaReducaoIBS { get; set; }
    public decimal MediaReducaoCBS { get; set; }
    public int TotalComReducaoIBS { get; set; }
    public int TotalComReducaoCBS { get; set; }
    public DateTime DataUltimaSincronizacao { get; set; }
}

#endregion
