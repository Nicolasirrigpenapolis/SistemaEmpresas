using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Repositories;
using SistemaEmpresas.Services;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    private readonly ILogger<ProdutoController> _logger;
    private readonly ICacheService _cache;

    public ProdutoController(IProdutoRepository repository, ILogger<ProdutoController> logger, ICacheService cache)
    {
        _repository = repository;
        _logger = logger;
        _cache = cache;
    }

    /// <summary>
    /// Lista produtos com paginação e filtros
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<DTOs.PagedResult<ProdutoListDto>>> Listar([FromQuery] ProdutoFiltroDto filtro)
    {
        _logger.LogInformation("GET /api/produto - Busca: {Busca}, Grupo: {Grupo}", 
            filtro.Busca, filtro.GrupoProduto);

        try
        {
            var result = await _repository.ListarAsync(filtro);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos");
            return StatusCode(500, new { mensagem = "Erro ao listar produtos" });
        }
    }

    /// <summary>
    /// Obtém um produto por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDto>> ObterPorId(int id)
    {
        _logger.LogInformation("GET /api/produto/{Id}", id);

        try
        {
            var produto = await _repository.ObterPorIdAsync(id);
            if (produto == null)
                return NotFound(new { mensagem = "Produto não encontrado" });

            return Ok(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter produto" });
        }
    }

    /// <summary>
    /// Cria um novo produto
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> Criar([FromBody] ProdutoCreateUpdateDto dto)
    {
        _logger.LogInformation("POST /api/produto - Descrição: {Descricao}", dto.Descricao);

        try
        {
            if (string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest(new { mensagem = "Descrição é obrigatória" });

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var produto = await _repository.CriarAsync(dto, usuario);
            var produtoDto = await _repository.ObterPorIdAsync(produto.SequenciaDoProduto);

            return CreatedAtAction(nameof(ObterPorId), new { id = produto.SequenciaDoProduto }, produtoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");
            return StatusCode(500, new { mensagem = "Erro ao criar produto" });
        }
    }

    /// <summary>
    /// Atualiza um produto existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ProdutoDto>> Atualizar(int id, [FromBody] ProdutoCreateUpdateDto dto)
    {
        _logger.LogInformation("PUT /api/produto/{Id}", id);

        try
        {
            if (string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest(new { mensagem = "Descrição é obrigatória" });

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var produto = await _repository.AtualizarAsync(id, dto, usuario);
            
            if (produto == null)
                return NotFound(new { mensagem = "Produto não encontrado" });

            var produtoDto = await _repository.ObterPorIdAsync(id);
            return Ok(produtoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao atualizar produto" });
        }
    }

    /// <summary>
    /// Inativa um produto
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Inativar(int id)
    {
        _logger.LogInformation("DELETE /api/produto/{Id}", id);

        try
        {
            var result = await _repository.InativarAsync(id);
            if (!result)
                return NotFound(new { mensagem = "Produto não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao inativar produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao inativar produto" });
        }
    }

    /// <summary>
    /// Ativa um produto
    /// </summary>
    [HttpPatch("{id}/ativar")]
    public async Task<ActionResult> Ativar(int id)
    {
        _logger.LogInformation("PATCH /api/produto/{Id}/ativar", id);

        try
        {
            var result = await _repository.AtivarAsync(id);
            if (!result)
                return NotFound(new { mensagem = "Produto não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao ativar produto" });
        }
    }

    /// <summary>
    /// Lista produtos para combo/select
    /// </summary>
    [HttpGet("combo")]
    public async Task<ActionResult<List<ProdutoComboDto>>> ListarParaCombo([FromQuery] string? busca)
    {
        try
        {
            // Cache apenas para busca vazia (lista completa)
            if (string.IsNullOrWhiteSpace(busca))
            {
                var cached = await _cache.GetOrCreateAsync(
                    "produto:combo:all",
                    async () => await _repository.ListarParaComboAsync(null),
                    CacheService.CacheDurations.Medium,
                    CacheService.CacheDurations.Short);
                return Ok(cached);
            }
            
            var produtos = await _repository.ListarParaComboAsync(busca);
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar produtos" });
        }
    }

    /// <summary>
    /// Conta produtos com estoque crítico
    /// </summary>
    [HttpGet("estoque-critico/count")]
    public async Task<ActionResult<int>> ContarEstoqueCritico()
    {
        try
        {
            var count = await _cache.GetOrCreateAsync(
                "produto:estoque-critico:count",
                async () => await _repository.ContarEstoqueCriticoAsync(),
                CacheService.CacheDurations.Short,
                CacheService.CacheDurations.VeryShort);
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao contar estoque crítico");
            return StatusCode(500, new { mensagem = "Erro ao contar estoque crítico" });
        }
    }
}
