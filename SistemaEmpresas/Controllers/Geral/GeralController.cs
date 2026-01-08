using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.DTOs.Geral;
using SistemaEmpresas.Services.Common;
using SistemaEmpresas.Services.Geral;

namespace SistemaEmpresas.Controllers.Geral;

/// <summary>
/// Controller para Cadastro Geral (Clientes, Fornecedores, Transportadoras, Vendedores)
/// Sistema unificado igual ao VB6 - Uma tabela para todas as entidades
/// </summary>
[ApiController]
[Route("api/geral")]
[Authorize]
public class GeralController : ControllerBase
{
    private readonly IGeralService _service;
    private readonly ILogger<GeralController> _logger;
    private readonly ICacheService _cache;

    public GeralController(
        IGeralService service,
        ILogger<GeralController> logger,
        ICacheService cache)
    {
        _service = service;
        _logger = logger;
        _cache = cache;
    }

    #region Endpoints de Consulta

    /// <summary>
    /// Lista registros com paginação e filtros
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GeralListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<GeralListDto>>> GetAll([FromQuery] GeralFiltroDto filtro)
    {
        _logger.LogInformation("GET /api/geral - Busca: {Busca}, Cliente: {Cliente}, Fornecedor: {Fornecedor}",
            filtro.Busca, filtro.Cliente, filtro.Fornecedor);

        var result = await _service.ListarAsync(filtro);
        return Ok(result);
    }

    /// <summary>
    /// Busca por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GeralDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GeralDetailDto>> GetById(int id)
    {
        _logger.LogInformation("GET /api/geral/{Id}", id);

        var result = await _service.ObterPorIdAsync(id);
        if (result == null)
            return NotFound(new { mensagem = $"Registro {id} não encontrado" });

        return Ok(result);
    }

    /// <summary>
    /// Busca simplificada para autocomplete/combobox
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(List<GeralListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GeralListDto>>> Buscar(
        [FromQuery] string termo,
        [FromQuery] string? tipo = null,
        [FromQuery] int limit = 20)
    {
        var result = await _service.BuscarAsync(termo, tipo, limit);
        return Ok(result);
    }

    /// <summary>
    /// Lista vendedores para combobox
    /// </summary>
    [HttpGet("vendedores")]
    [ProducesResponseType(typeof(List<GeralListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GeralListDto>>> GetVendedores()
    {
        var vendedores = await _cache.GetOrCreateAsync(
            "geral:vendedores:all",
            async () => await _service.ListarVendedoresAsync(),
            CacheService.CacheDurations.Medium,
            CacheService.CacheDurations.Short);

        return Ok(vendedores);
    }

    #endregion

    #region Endpoints de Modificação

    /// <summary>
    /// Cria novo registro
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GeralDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GeralDetailDto>> Create([FromBody] GeralCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { mensagem = "Dados inválidos", erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

        _logger.LogInformation("POST /api/geral - RazaoSocial: {RazaoSocial}", dto.RazaoSocial);

        var usuario = User.Identity?.Name ?? "Sistema";
        var result = await _service.CriarAsync(dto, usuario);

        if (!result.Sucesso)
        {
            if (result.Erros.Any())
                return BadRequest(new { mensagem = "Erros de validação", erros = result.Erros });
            return BadRequest(new { mensagem = result.Mensagem });
        }

        // Invalida cache de vendedores se necessário
        if (dto.Vendedor)
            _cache.Remove("geral:vendedores:all");

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.SequenciaDoGeral }, result.Data);
    }

    /// <summary>
    /// Atualiza registro existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GeralDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GeralDetailDto>> Update(int id, [FromBody] GeralUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { mensagem = "Dados inválidos", erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

        _logger.LogInformation("PUT /api/geral/{Id}", id);

        if (id != dto.SequenciaDoGeral)
            return BadRequest(new { mensagem = "ID do registro não confere" });

        var usuario = User.Identity?.Name ?? "Sistema";
        var result = await _service.AtualizarAsync(id, dto, usuario);

        if (!result.Sucesso)
        {
            if (result.Mensagem == "Cadastro não encontrado")
                return NotFound(new { mensagem = result.Mensagem });

            if (result.Erros.Any())
                return BadRequest(new { mensagem = "Erros de validação", erros = result.Erros });

            return BadRequest(new { mensagem = result.Mensagem });
        }

        // Invalida cache de vendedores
        _cache.Remove("geral:vendedores:all");

        return Ok(result.Data);
    }

    /// <summary>
    /// Exclui (inativa) registro
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE /api/geral/{Id}", id);

        var usuario = User.Identity?.Name ?? "Sistema";
        var result = await _service.InativarAsync(id, usuario);

        if (!result.Sucesso)
            return NotFound(new { mensagem = result.Mensagem });

        // Invalida cache de vendedores
        _cache.Remove("geral:vendedores:all");

        return Ok(new { mensagem = "Registro inativado com sucesso" });
    }

    #endregion

    #region APIs Externas (CNPJ/CEP)

    /// <summary>
    /// Busca dados de uma empresa pelo CNPJ usando a API Brasil API
    /// </summary>
    [HttpGet("consulta-cnpj/{cnpj}")]
    [ProducesResponseType(typeof(CnpjResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CnpjResponseDto>> ConsultarCnpj(string cnpj)
    {
        var result = await _service.ConsultarCnpjAsync(cnpj);

        if (result == null)
            return BadRequest(new { mensagem = "CNPJ não encontrado ou inválido" });

        return Ok(result);
    }

    /// <summary>
    /// Busca endereço pelo CEP
    /// </summary>
    [HttpGet("consulta-cep/{cep}")]
    [ProducesResponseType(typeof(CepResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CepResponseDto>> ConsultarCep(string cep)
    {
        var result = await _service.ConsultarCepAsync(cep);

        if (result == null)
            return BadRequest(new { mensagem = "CEP não encontrado ou inválido" });

        return Ok(result);
    }

    #endregion

    #region Validação CPF/CNPJ

    /// <summary>
    /// Valida um CPF
    /// </summary>
    [HttpGet("validar-cpf/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult ValidarCpf(string cpf)
    {
        var valido = _service.ValidarCpf(cpf);
        return Ok(new { valido });
    }

    /// <summary>
    /// Valida um CNPJ
    /// </summary>
    [HttpGet("validar-cnpj/{cnpj}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult ValidarCnpj(string cnpj)
    {
        var valido = _service.ValidarCnpj(cnpj);
        return Ok(new { valido });
    }

    #endregion
}
