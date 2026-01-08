using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.Core.Dtos;
using SistemaEmpresas.Features.MovimentoContabil.Dtos;
using SistemaEmpresas.Features.MovimentoContabil.Repositories;
using System.Security.Claims;

namespace SistemaEmpresas.Features.MovimentoContabil.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MovimentoContabilController : ControllerBase
{
    private readonly IMovimentoContabilRepository _repository;
    private readonly ILogger<MovimentoContabilController> _logger;

    public MovimentoContabilController(IMovimentoContabilRepository repository, ILogger<MovimentoContabilController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Obtém informações de estoque de um produto
    /// </summary>
    [HttpGet("produto/{id}")]
    public async Task<ActionResult<EstoqueInfoDto>> ObterEstoqueProduto(int id)
    {
        _logger.LogInformation("GET /api/movimentocontabil/produto/{Id}", id);

        try
        {
            var info = await _repository.ObterEstoqueInfoAsync(id);
            if (info == null)
                return NotFound(new { mensagem = "Produto não encontrado" });

            return Ok(info);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estoque do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter informações de estoque" });
        }
    }

    /// <summary>
    /// Obtém informações de estoque de um conjunto
    /// </summary>
    [HttpGet("conjunto/{id}")]
    public async Task<ActionResult<EstoqueInfoDto>> ObterEstoqueConjunto(int id)
    {
        _logger.LogInformation("GET /api/movimentocontabil/conjunto/{Id}", id);

        try
        {
            var info = await _repository.ObterEstoqueConjuntoInfoAsync(id);
            if (info == null)
                return NotFound(new { mensagem = "Conjunto não encontrado" });

            return Ok(info);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estoque do conjunto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter informações de estoque" });
        }
    }

    /// <summary>
    /// Busca produtos para movimento contábil
    /// </summary>
    [HttpGet("buscar-produtos")]
    public async Task<ActionResult<List<EstoqueInfoDto>>> BuscarProdutos([FromQuery] string? busca, [FromQuery] int limite = 50)
    {
        _logger.LogInformation("GET /api/movimentocontabil/buscar-produtos - Busca: {Busca}", busca);

        try
        {
            var produtos = await _repository.BuscarProdutosParaMovimentoAsync(busca, limite);
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar produtos para movimento contábil");
            return StatusCode(500, new { mensagem = "Erro ao buscar produtos" });
        }
    }

    /// <summary>
    /// Busca conjuntos para movimento contábil
    /// </summary>
    [HttpGet("buscar-conjuntos")]
    public async Task<ActionResult<List<EstoqueInfoDto>>> BuscarConjuntos([FromQuery] string? busca, [FromQuery] int limite = 50)
    {
        _logger.LogInformation("GET /api/movimentocontabil/buscar-conjuntos - Busca: {Busca}", busca);

        try
        {
            var conjuntos = await _repository.BuscarConjuntosParaMovimentoAsync(busca, limite);
            return Ok(conjuntos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar conjuntos para movimento contábil");
            return StatusCode(500, new { mensagem = "Erro ao buscar conjuntos" });
        }
    }

    /// <summary>
    /// Busca despesas para movimento contábil
    /// </summary>
    [HttpGet("buscar-despesas")]
    public async Task<ActionResult<List<DespesaMvtoContabilItemDto>>> BuscarDespesas([FromQuery] string? busca, [FromQuery] int limite = 50)
    {
        _logger.LogInformation("GET /api/movimentocontabil/buscar-despesas - Busca: {Busca}", busca);

        try
        {
            var despesas = await _repository.BuscarDespesasParaMovimentoAsync(busca, limite);
            return Ok(despesas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar despesas para movimento contábil");
            return StatusCode(500, new { mensagem = "Erro ao buscar despesas" });
        }
    }

    // Novos endpoints para MovimentoContabilNovo (MVTOCONN.FRM)

    /// <summary>
    /// Lista movimentos contábeis com filtros
    /// </summary>
    [HttpGet("novos")]
    public async Task<ActionResult<PagedResult<MovimentoContabilNovoDto>>> ListarMovimentos([FromQuery] MovimentoContabilFiltroDto filtro)
    {
        _logger.LogInformation("GET /api/movimentocontabil/novos");

        try
        {
            var result = await _repository.ListarMovimentosNovosAsync(filtro);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar movimentos contábeis");
            return StatusCode(500, new { mensagem = "Erro ao listar movimentos" });
        }
    }

    /// <summary>
    /// Obtém um movimento contábil pelo ID
    /// </summary>
    [HttpGet("novo/{id}")]
    public async Task<ActionResult<MovimentoContabilNovoDto>> ObterMovimento(int id)
    {
        _logger.LogInformation("GET /api/movimentocontabil/novo/{Id}", id);

        try
        {
            var movimento = await _repository.ObterMovimentoAsync(id);
            if (movimento == null)
                return NotFound(new { mensagem = "Movimento não encontrado" });

            return Ok(movimento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter movimento {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter movimento" });
        }
    }

    /// <summary>
    /// Cria um novo movimento contábil completo
    /// </summary>
    [HttpPost("novo")]
    public async Task<ActionResult<MovimentoContabilNovoDto>> CriarMovimento([FromBody] MovimentoContabilNovoDto dto)
    {
        _logger.LogInformation("POST /api/movimentocontabil/novo");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var result = await _repository.CriarMovimentoAsync(dto, usuario);
            return CreatedAtAction(nameof(ObterMovimento), new { id = result.SequenciaDoMovimento }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar movimento contábil");
            return StatusCode(500, new { mensagem = "Erro ao criar movimento: " + ex.Message });
        }
    }

    /// <summary>
    /// Exclui um movimento contábil
    /// </summary>
    [HttpDelete("novo/{id}")]
    public async Task<IActionResult> ExcluirMovimento(int id)
    {
        _logger.LogInformation("DELETE /api/movimentocontabil/novo/{Id}", id);

        try
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var sucesso = await _repository.ExcluirMovimentoAsync(id, usuario);
            
            if (!sucesso)
                return NotFound(new { mensagem = "Movimento não encontrado ou erro ao excluir" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir movimento {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao excluir movimento: " + ex.Message, detalhe = ex.InnerException?.Message });
        }
    }

    /// <summary>
    /// Realiza ajuste de inventário para um produto ou conjunto
    /// </summary>
    [HttpPost("ajuste")]
    public async Task<ActionResult<AjusteMovimentoContabilResultDto>> RealizarAjuste([FromBody] AjusteMovimentoContabilDto dto)
    {
        _logger.LogInformation("POST /api/movimentocontabil/ajuste - {Tipo}: {Id}, QuantidadeFisica: {Qtd}", 
            dto.EhConjunto ? "Conjunto" : "Produto", dto.SequenciaDoProduto, dto.QuantidadeFisica);

        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var resultado = await _repository.RealizarAjusteAsync(dto, usuario);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar ajuste de movimento contábil");
            return StatusCode(500, new { mensagem = "Erro ao realizar ajuste de movimento contábil" });
        }
    }

    /// <summary>
    /// Realiza ajuste de inventário em lote
    /// </summary>
    [HttpPost("ajuste-lote")]
    public async Task<ActionResult<AjusteMovimentoContabilLoteResultDto>> RealizarAjusteLote([FromBody] AjusteMovimentoContabilLoteDto dto)
    {
        _logger.LogInformation("POST /api/movimentocontabil/ajuste-lote - Total itens: {Total}", dto.Itens.Count);

        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var resultado = await _repository.RealizarAjusteLoteAsync(dto, usuario);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar ajuste de movimento contábil em lote");
            return StatusCode(500, new { mensagem = "Erro ao realizar ajuste em lote" });
        }
    }

    /// <summary>
    /// Lista movimentos de estoque de um produto
    /// </summary>
    [HttpGet("movimentos")]
    public async Task<ActionResult<PagedResult<MovimentoEstoqueDto>>> ListarMovimentos([FromQuery] MovimentoEstoqueFiltroDto filtro)
    {
        _logger.LogInformation("GET /api/movimentocontabil/movimentos - {Tipo}: {Id}", 
            filtro.EhConjunto ? "Conjunto" : "Produto", filtro.SequenciaDoProduto);

        try
        {
            if (filtro.SequenciaDoProduto <= 0)
                return BadRequest(new { mensagem = "Produto é obrigatório" });

            var movimentos = await _repository.ListarMovimentosAsync(filtro);
            return Ok(movimentos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar movimentos de estoque");
            return StatusCode(500, new { mensagem = "Erro ao listar movimentos" });
        }
    }

    /// <summary>
    /// Recalcula e atualiza o estoque contábil de um produto
    /// </summary>
    [HttpPost("recalcular/{id}")]
    public async Task<ActionResult> RecalcularEstoque(int id)
    {
        _logger.LogInformation("POST /api/movimentocontabil/recalcular/{Id}", id);

        try
        {
            var novoEstoque = await _repository.RecalcularEstoqueContabilAsync(id);
            await _repository.AtualizarEstoqueProdutoAsync(id, novoEstoque);

            return Ok(new { 
                mensagem = "Estoque recalculado com sucesso",
                novoEstoque = novoEstoque
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recalcular estoque do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao recalcular estoque" });
        }
    }

    // ========== ENDPOINTS DE PRODUÇÃO INTELIGENTE ==========

    /// <summary>
    /// Verifica viabilidade de produção de um produto/conjunto e retorna componentes faltantes
    /// </summary>
    [HttpGet("producao/verificar")]
    public async Task<ActionResult<VerificacaoProducaoResultDto>> VerificarViabilidadeProducao(
        [FromQuery] int id, 
        [FromQuery] decimal quantidade, 
        [FromQuery] bool ehConjunto = false)
    {
        _logger.LogInformation("GET /api/movimentocontabil/producao/verificar - {Tipo}: {Id}, Qtd: {Qtd}", 
            ehConjunto ? "Conjunto" : "Produto", id, quantidade);

        try
        {
            if (id <= 0 || quantidade <= 0)
                return BadRequest(new { mensagem = "ID e quantidade devem ser maiores que zero" });

            var result = await _repository.VerificarViabilidadeProducaoAsync(id, quantidade, ehConjunto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar viabilidade de produção");
            return StatusCode(500, new { mensagem = "Erro ao verificar viabilidade" });
        }
    }

    /// <summary>
    /// Executa produção em cascata, criando automaticamente os itens intermediários necessários
    /// </summary>
    [HttpPost("producao/cascata")]
    public async Task<ActionResult<ProducaoCascataResultDto>> ExecutarProducaoCascata([FromBody] ProducaoCascataRequestDto request)
    {
        _logger.LogInformation("POST /api/movimentocontabil/producao/cascata - {Tipo}: {Id}, Qtd: {Qtd}", 
            request.EhConjunto ? "Conjunto" : "Produto", request.SequenciaDoProdutoOuConjunto, request.Quantidade);

        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.SequenciaDoProdutoOuConjunto <= 0 || request.Quantidade <= 0)
                return BadRequest(new { mensagem = "ID e quantidade devem ser maiores que zero" });

            if (request.SequenciaDoGeral <= 0)
                return BadRequest(new { mensagem = "Fornecedor/Cliente (Geral) é obrigatório" });

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var result = await _repository.ExecutarProducaoCascataAsync(request, usuario);
            
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar produção em cascata");
            return StatusCode(500, new { mensagem = "Erro ao executar produção" });
        }
    }
}
