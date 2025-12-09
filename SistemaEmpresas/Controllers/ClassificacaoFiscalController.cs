using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.Models;
using SistemaEmpresas.Repositories;
using SistemaEmpresas.Services;

namespace SistemaEmpresas.Controllers;

/// <summary>
/// Controller de Classificação Fiscal (NCM + ClassTrib SVRS)
/// Endpoints para CRUD e sincronização com API SVRS
/// </summary>
[ApiController]
[Route("api/classificacao-fiscal")]
[Authorize]
public class ClassificacaoFiscalController : ControllerBase
{
    private readonly IClassificacaoFiscalRepository _repository;
    private readonly ClassTribSyncService _syncService;
    private readonly ILogger<ClassificacaoFiscalController> _logger;
    private readonly ICacheService _cache;

    public ClassificacaoFiscalController(
        IClassificacaoFiscalRepository repository,
        ClassTribSyncService syncService,
        ILogger<ClassificacaoFiscalController> logger,
        ICacheService cache)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    #region Endpoints de Consulta

    /// <summary>
    /// Lista todas as classificações fiscais com paginação e filtros
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 50)</param>
    /// <param name="ncm">Filtro por NCM parcial</param>
    /// <param name="descricao">Filtro por descrição</param>
    /// <param name="somenteNFe">Filtrar apenas válidos para NFe</param>
    /// <param name="incluirInativos">Incluir registros inativos</param>
    /// <param name="tributacao">Filtro de tributação: todos, vinculados, pendentes</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<ClassificacaoFiscal>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? ncm = null,
        [FromQuery] string? descricao = null,
        [FromQuery] bool? somenteNFe = null,
        [FromQuery] bool incluirInativos = false,
        [FromQuery] string? tributacao = null)
    {
        _logger.LogInformation("GET /api/classificacao-fiscal - Página: {PageNumber}", pageNumber);

        var (items, total) = await _repository.GetPagedAsync(
            pageNumber,
            pageSize,
            ncm,
            descricao,
            somenteNFe,
            incluirInativos,
            tributacao);

        var resultado = new PagedResultDto<ClassificacaoFiscal>
        {
            Items = items,
            Total = total,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };

        return Ok(resultado);
    }

    /// <summary>
    /// Busca classificação fiscal por ID (Sequência)
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClassificacaoFiscal>> GetById(short id)
    {
        _logger.LogInformation("GET /api/classificacao-fiscal/{Id}", id);

        var classificacao = await _repository.GetByIdAsync(id);

        if (classificacao == null)
        {
            _logger.LogWarning("Classificação fiscal não encontrada: {Id}", id);
            return NotFound(new { mensagem = $"Classificação fiscal {id} não encontrada" });
        }

        return Ok(classificacao);
    }

    /// <summary>
    /// Busca classificação fiscal por NCM
    /// Compatibilidade VB6
    /// </summary>
    [HttpGet("ncm/{ncm}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClassificacaoFiscal>> GetByNcm(string ncm)
    {
        _logger.LogInformation("GET /api/classificacao-fiscal/ncm/{NCM}", ncm);

        var classificacao = await _repository.GetByNcmAsync(ncm);

        if (classificacao == null)
        {
            _logger.LogWarning("Classificação fiscal não encontrada para NCM: {NCM}", ncm);
            return NotFound(new { mensagem = $"NCM {ncm} não encontrado" });
        }

        return Ok(classificacao);
    }

    /// <summary>
    /// Busca classificações fiscais por ClassTribId
    /// </summary>
    [HttpGet("classtrib/{classTribId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClassificacaoFiscal>>> GetByClassTribId(int classTribId)
    {
        _logger.LogInformation("GET /api/classificacao-fiscal/classtrib/{ClassTribId}", classTribId);

        var classificacoes = await _repository.GetByClassTribIdAsync(classTribId);

        return Ok(classificacoes);
    }

    /// <summary>
    /// Lista apenas classificações válidas para NFe
    /// ENDPOINT PRINCIPAL - Foco do sistema
    /// </summary>
    [HttpGet("nfe")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClassificacaoFiscal>>> GetClassificacoesNFe()
    {
        _logger.LogInformation("GET /api/classificacao-fiscal/nfe");

        var classificacoes = await _repository.GetClassificacoesNFeAsync();

        return Ok(classificacoes);
    }

    /// <summary>
    /// Pesquisa classificações fiscais por termo (NCM ou descrição)
    /// </summary>
    [HttpGet("pesquisar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClassificacaoFiscal>>> Search([FromQuery] string termo)
    {
        _logger.LogInformation("GET /api/classificacao-fiscal/pesquisar?termo={Termo}", termo);

        var classificacoes = await _repository.SearchAsync(termo);

        return Ok(classificacoes);
    }

    #endregion

    #region Endpoints de CRUD

    /// <summary>
    /// Cria nova classificação fiscal
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClassificacaoFiscal>> Create([FromBody] ClassificacaoFiscal classificacao)
    {
        _logger.LogInformation("POST /api/classificacao-fiscal - Criando nova classificação");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Validações básicas
        if (classificacao.Ncm == 0)
        {
            return BadRequest(new { mensagem = "NCM é obrigatório" });
        }

        if (string.IsNullOrWhiteSpace(classificacao.DescricaoDoNcm))
        {
            return BadRequest(new { mensagem = "Descrição é obrigatória" });
        }

        try
        {
            var resultado = await _repository.CreateAsync(classificacao);
            _logger.LogInformation("Classificação fiscal criada com ID: {Id}", resultado.SequenciaDaClassificacao);

            return CreatedAtAction(
                nameof(GetById),
                new { id = resultado.SequenciaDaClassificacao },
                resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar classificação fiscal");
            return StatusCode(500, new { mensagem = "Erro interno ao criar classificação fiscal" });
        }
    }

    /// <summary>
    /// Atualiza classificação fiscal existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClassificacaoFiscal>> Update(short id, [FromBody] ClassificacaoFiscal classificacao)
    {
        _logger.LogInformation("PUT /api/classificacao-fiscal/{Id}", id);

        if (id != classificacao.SequenciaDaClassificacao)
        {
            return BadRequest(new { mensagem = "ID da URL não corresponde ao ID do objeto" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existente = await _repository.GetByIdAsync(id);
        if (existente == null)
        {
            return NotFound(new { mensagem = $"Classificação fiscal {id} não encontrada" });
        }

        try
        {
            var resultado = await _repository.UpdateAsync(classificacao);
            _logger.LogInformation("Classificação fiscal {Id} atualizada com sucesso", id);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar classificação fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar classificação fiscal" });
        }
    }

    /// <summary>
    /// Exclui classificação fiscal (soft delete via Inativo)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(short id)
    {
        _logger.LogInformation("DELETE /api/classificacao-fiscal/{Id}", id);

        var sucesso = await _repository.DeleteAsync(id);

        if (!sucesso)
        {
            return NotFound(new { mensagem = $"Classificação fiscal {id} não encontrada" });
        }

        _logger.LogInformation("Classificação fiscal {Id} excluída (inativada) com sucesso", id);
        return Ok(new { mensagem = "Classificação fiscal inativada com sucesso" });
    }

    #endregion

    #region Endpoints de Sincronização API SVRS

    /// <summary>
    /// Sincroniza classificações fiscais com API SVRS
    /// APENAS classificações válidas para NFe
    /// Requer permissão de administrador
    /// </summary>
    /// <param name="forcar">Forçar atualização ignorando cache</param>
    [HttpPost("sincronizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SyncResultDto>> Sincronizar([FromQuery] bool forcar = false)
    {
        _logger.LogInformation("POST /api/classificacao-fiscal/sincronizar - Forçar: {Forcar}", forcar);

        try
        {
            var resultado = await _syncService.SincronizarClassificacoesNFeAsync(forcar);

            if (!resultado.Sucesso)
            {
                return BadRequest(resultado);
            }

            _logger.LogInformation("Sincronização concluída: {Mensagem}", resultado.Mensagem);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na sincronização com API SVRS");
            return StatusCode(500, new
            {
                mensagem = "Erro ao sincronizar com API SVRS",
                erro = ex.Message
            });
        }
    }

    /// <summary>
    /// Sincroniza uma classificação específica por cClassTrib
    /// </summary>
    [HttpPost("sincronizar/{cClassTrib}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClassificacaoFiscal>> SincronizarPorCodigoClassTrib(string cClassTrib)
    {
        _logger.LogInformation("POST /api/classificacao-fiscal/sincronizar/{cClassTrib}", cClassTrib);

        try
        {
            var resultado = await _syncService.SincronizarPorCodigoClassTribAsync(cClassTrib);

            if (resultado == null)
            {
                return NotFound(new
                {
                    mensagem = $"cClassTrib {cClassTrib} não encontrado na API SVRS ou não é válido para NFe"
                });
            }

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao sincronizar cClassTrib: {cClassTrib}", cClassTrib);
            return StatusCode(500, new
            {
                mensagem = "Erro ao sincronizar classificação",
                erro = ex.Message
            });
        }
    }

    /// <summary>
    /// Obtém status da última sincronização
    /// </summary>
    [HttpGet("status-sincronizacao")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SyncStatusDto>> GetStatusSincronizacao()
    {
        _logger.LogInformation("GET /api/classificacao-fiscal/status-sincronizacao");

        var status = await _syncService.GetStatusSincronizacaoAsync();

        return Ok(status);
    }

    #endregion
}

#region DTOs

/// <summary>
/// DTO para resultado paginado
/// </summary>
public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int Total { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

#endregion
