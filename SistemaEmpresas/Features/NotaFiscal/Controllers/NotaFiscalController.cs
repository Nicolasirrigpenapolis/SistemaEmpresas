using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.Core.Dtos;
using SistemaEmpresas.Features.NotaFiscal.Dtos;
using SistemaEmpresas.Features.NotaFiscal.Repositories;
using SistemaEmpresas.Features.Produto.Dtos;
using SistemaEmpresas.Core.Services;
using SistemaEmpresas.Features.Fiscal.Services;
using SistemaEmpresas.Features.Fiscal.Dtos;
using System.Security.Claims;

namespace SistemaEmpresas.Features.NotaFiscal.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotaFiscalController : ControllerBase
{
    private readonly INotaFiscalRepository _repository;
    private readonly ILogger<NotaFiscalController> _logger;
    private readonly ICacheService _cache;
    private readonly ICalculoImpostoService _calculoImpostoService;

    public NotaFiscalController(
        INotaFiscalRepository repository, 
        ILogger<NotaFiscalController> logger, 
        ICacheService cache,
        ICalculoImpostoService calculoImpostoService)
    {
        _repository = repository;
        _logger = logger;
        _cache = cache;
        _calculoImpostoService = calculoImpostoService;
    }

    #region CRUD

    /// <summary>
    /// Lista notas fiscais com paginação e filtros
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<NotaFiscalListDto>>> Listar([FromQuery] NotaFiscalFiltroDto filtro)
    {
        _logger.LogInformation("GET /api/notafiscal - Busca: {Busca}, DataInicial: {DataInicial}, DataFinal: {DataFinal}",
            filtro.Busca, filtro.DataInicial, filtro.DataFinal);

        try
        {
            var result = await _repository.ListarAsync(filtro);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar notas fiscais");
            return StatusCode(500, new { mensagem = "Erro ao listar notas fiscais" });
        }
    }

    /// <summary>
    /// Obtém uma nota fiscal por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<NotaFiscalDto>> ObterPorId(int id)
    {
        _logger.LogInformation("GET /api/notafiscal/{Id}", id);

        try
        {
            var nota = await _repository.ObterPorIdAsync(id);
            if (nota == null)
                return NotFound(new { mensagem = "Nota fiscal não encontrada" });

            return Ok(nota);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter nota fiscal" });
        }
    }

    /// <summary>
    /// Cria uma nova nota fiscal
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<NotaFiscalDto>> Criar([FromBody] NotaFiscalCreateUpdateDto dto)
    {
        _logger.LogInformation("POST /api/notafiscal - Cliente: {Cliente}", dto.SequenciaDoGeral);

        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var nota = await _repository.CriarAsync(dto, usuario);
            var notaDto = await _repository.ObterPorIdAsync(nota.SequenciaDaNotaFiscal);

            return CreatedAtAction(nameof(ObterPorId), new { id = nota.SequenciaDaNotaFiscal }, notaDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar nota fiscal");
            return StatusCode(500, new { mensagem = "Erro ao criar nota fiscal" });
        }
    }

    /// <summary>
    /// Atualiza uma nota fiscal existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<NotaFiscalDto>> Atualizar(int id, [FromBody] NotaFiscalCreateUpdateDto dto)
    {
        _logger.LogInformation("PUT /api/notafiscal/{Id}", id);

        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se pode editar
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal (já transmitida, autorizada ou cancelada)" });

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var nota = await _repository.AtualizarAsync(id, dto, usuario);

            if (nota == null)
                return NotFound(new { mensagem = "Nota fiscal não encontrada" });

            var notaDto = await _repository.ObterPorIdAsync(id);
            return Ok(notaDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao atualizar nota fiscal" });
        }
    }

    /// <summary>
    /// Cancela uma nota fiscal
    /// </summary>
    [HttpPost("{id}/cancelar")]
    public async Task<ActionResult> Cancelar(int id, [FromBody] CancelarNfeDto dto)
    {
        _logger.LogInformation("POST /api/notafiscal/{Id}/cancelar", id);

        try
        {
            if (string.IsNullOrWhiteSpace(dto.Justificativa) || dto.Justificativa.Length < 15)
                return BadRequest(new { mensagem = "Justificativa deve ter no mínimo 15 caracteres" });

            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var result = await _repository.CancelarAsync(id, dto.Justificativa, usuario);

            if (!result)
                return NotFound(new { mensagem = "Nota fiscal não encontrada" });

            return Ok(new { mensagem = "Nota fiscal cancelada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao cancelar nota fiscal" });
        }
    }

    /// <summary>
    /// Duplica uma nota fiscal existente
    /// Copia todos os dados exceto: autorização, transmissão, chaves NFe e XMLs
    /// A nova nota recebe o próximo número disponível
    /// </summary>
    [HttpPost("{id}/duplicar")]
    public async Task<ActionResult<NotaFiscalDto>> Duplicar(int id)
    {
        _logger.LogInformation("POST /api/notafiscal/{Id}/duplicar", id);

        try
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var novaNota = await _repository.DuplicarAsync(id, usuario);
            var notaDto = await _repository.ObterPorIdAsync(novaNota.SequenciaDaNotaFiscal);

            return CreatedAtAction(nameof(ObterPorId), new { id = novaNota.SequenciaDaNotaFiscal }, notaDto);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao duplicar nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao duplicar nota fiscal" });
        }
    }

    /// <summary>
    /// Verifica se pode editar uma nota fiscal
    /// </summary>
    [HttpGet("{id}/pode-editar")]
    public async Task<ActionResult<bool>> PodeEditar(int id)
    {
        try
        {
            var podeEditar = await _repository.PodeEditarAsync(id);
            return Ok(new { podeEditar });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar se pode editar nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao verificar permissão de edição" });
        }
    }

    /// <summary>
    /// Obtém o próximo número da nota fiscal para uma propriedade
    /// </summary>
    [HttpGet("proximo-numero/{propriedade}")]
    public async Task<ActionResult<int>> ObterProximoNumero(short propriedade)
    {
        try
        {
            var numero = await _repository.ObterProximoNumeroAsync(propriedade);
            return Ok(new { numero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter próximo número para propriedade {Propriedade}", propriedade);
            return StatusCode(500, new { mensagem = "Erro ao obter próximo número" });
        }
    }

    #endregion

    #region Combos

    /// <summary>
    /// Lista clientes para combo/autocomplete
    /// </summary>
    [HttpGet("combo/clientes")]
    public async Task<ActionResult<List<ClienteComboDto>>> ListarClientesCombo([FromQuery] string? busca)
    {
        try
        {
            var clientes = await _repository.ListarClientesComboAsync(busca);
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar clientes para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar clientes" });
        }
    }

    /// <summary>
    /// Obtém dados de um cliente específico
    /// </summary>
    [HttpGet("combo/clientes/{id}")]
    public async Task<ActionResult<ClienteComboDto>> ObterCliente(int id)
    {
        try
        {
            var cliente = await _repository.ObterClienteAsync(id);
            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado" });

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter cliente {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter cliente" });
        }
    }

    /// <summary>
    /// Lista transportadoras para combo/autocomplete
    /// </summary>
    [HttpGet("combo/transportadoras")]
    public async Task<ActionResult<List<TransportadoraComboDto>>> ListarTransportadorasCombo([FromQuery] string? busca)
    {
        try
        {
            var transportadoras = await _repository.ListarTransportadorasComboAsync(busca);
            return Ok(transportadoras);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar transportadoras para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar transportadoras" });
        }
    }

    /// <summary>
    /// Obtém dados de uma transportadora específica
    /// </summary>
    [HttpGet("combo/transportadoras/{id}")]
    public async Task<ActionResult<TransportadoraComboDto>> ObterTransportadora(int id)
    {
        try
        {
            var transportadora = await _repository.ObterTransportadoraAsync(id);
            if (transportadora == null)
                return NotFound(new { mensagem = "Transportadora não encontrada" });

            return Ok(transportadora);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter transportadora {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter transportadora" });
        }
    }

    /// <summary>
    /// Lista naturezas de operação para combo
    /// </summary>
    [HttpGet("combo/naturezas")]
    public async Task<ActionResult<List<NaturezaOperacaoComboDto>>> ListarNaturezasCombo([FromQuery] bool? entradaSaida)
    {
        try
        {
            var cacheKey = entradaSaida.HasValue ? $"nf:combo:naturezas:{entradaSaida}" : "nf:combo:naturezas:all";
            var naturezas = await _cache.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.ListarNaturezasComboAsync(entradaSaida),
                CacheService.CacheDurations.Long,
                CacheService.CacheDurations.Medium);
            return Ok(naturezas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar naturezas para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar naturezas" });
        }
    }

    /// <summary>
    /// Lista propriedades/filiais para combo
    /// </summary>
    [HttpGet("combo/propriedades")]
    public async Task<ActionResult<List<PropriedadeComboDto>>> ListarPropriedadesCombo()
    {
        try
        {
            var propriedades = await _cache.GetOrCreateAsync(
                "nf:combo:propriedades",
                async () => await _repository.ListarPropriedadesComboAsync(),
                CacheService.CacheDurations.VeryLong,
                CacheService.CacheDurations.Long);
            return Ok(propriedades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar propriedades para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar propriedades" });
        }
    }

    /// <summary>
    /// Lista propriedades vinculadas a um cliente específico
    /// </summary>
    [HttpGet("combo/propriedades/cliente/{sequenciaDoGeral}")]
    public async Task<ActionResult<List<PropriedadeComboDto>>> ListarPropriedadesPorCliente(int sequenciaDoGeral)
    {
        try
        {
            var propriedades = await _repository.ListarPropriedadesPorClienteAsync(sequenciaDoGeral);
            return Ok(propriedades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar propriedades do cliente {SequenciaDoGeral}", sequenciaDoGeral);
            return StatusCode(500, new { mensagem = "Erro ao listar propriedades do cliente" });
        }
    }

    /// <summary>
    /// Lista tipos de cobrança para combo
    /// </summary>
    [HttpGet("combo/tipos-cobranca")]
    public async Task<ActionResult<List<TipoCobrancaComboDto>>> ListarTiposCobrancaCombo()
    {
        try
        {
            var tipos = await _cache.GetOrCreateAsync(
                "nf:combo:tipos-cobranca",
                async () => await _repository.ListarTiposCobrancaComboAsync(),
                CacheService.CacheDurations.VeryLong,
                CacheService.CacheDurations.Long);
            return Ok(tipos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tipos de cobrança para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar tipos de cobrança" });
        }
    }

    /// <summary>
    /// Lista vendedores para combo/autocomplete
    /// </summary>
    [HttpGet("combo/vendedores")]
    public async Task<ActionResult<List<VendedorComboDto>>> ListarVendedoresCombo([FromQuery] string? busca)
    {
        try
        {
            var vendedores = await _repository.ListarVendedoresComboAsync(busca);
            return Ok(vendedores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar vendedores para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar vendedores" });
        }
    }

    /// <summary>
    /// Lista produtos para combo/autocomplete na nota fiscal
    /// </summary>
    [HttpGet("combo/produtos")]
    public async Task<ActionResult<List<ProdutoComboDto>>> ListarProdutosCombo([FromQuery] string? busca)
    {
        try
        {
            var produtos = await _repository.ListarProdutosComboAsync(busca);
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos para combo");
            return StatusCode(500, new { mensagem = "Erro ao listar produtos" });
        }
    }

    #endregion
    
    #region Itens da Nota - Produtos
    
    /// <summary>
    /// Lista produtos da nota fiscal
    /// </summary>
    [HttpGet("{id}/produtos")]
    public async Task<ActionResult<List<ProdutoDaNotaFiscalDto>>> ListarProdutos(int id)
    {
        try
        {
            var produtos = await _repository.ListarProdutosAsync(id);
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao listar produtos" });
        }
    }
    
    /// <summary>
    /// Adiciona produto à nota fiscal
    /// </summary>
    [HttpPost("{id}/produtos")]
    public async Task<ActionResult<ProdutoDaNotaFiscalDto>> AdicionarProduto(int id, [FromBody] ProdutoDaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var produto = await _repository.AdicionarProdutoAsync(id, dto);
            return CreatedAtAction(nameof(ListarProdutos), new { id }, produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar produto à nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao adicionar produto" });
        }
    }
    
    /// <summary>
    /// Atualiza produto da nota fiscal
    /// </summary>
    [HttpPut("{id}/produtos/{sequenciaProduto}")]
    public async Task<ActionResult<ProdutoDaNotaFiscalDto>> AtualizarProduto(int id, int sequenciaProduto, [FromBody] ProdutoDaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var produto = await _repository.AtualizarProdutoAsync(id, sequenciaProduto, dto);
            if (produto == null)
                return NotFound(new { mensagem = "Produto não encontrado" });
                
            return Ok(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao atualizar produto" });
        }
    }
    
    /// <summary>
    /// Remove produto da nota fiscal
    /// </summary>
    [HttpDelete("{id}/produtos/{sequenciaProduto}")]
    public async Task<ActionResult> RemoverProduto(int id, int sequenciaProduto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var result = await _repository.RemoverProdutoAsync(id, sequenciaProduto);
            if (!result)
                return NotFound(new { mensagem = "Produto não encontrado" });
                
            return Ok(new { mensagem = "Produto removido com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover produto da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao remover produto" });
        }
    }
    
    /// <summary>
    /// Calcula impostos para um item da nota fiscal (Produto, Conjunto ou Peça)
    /// </summary>
    /// <remarks>
    /// TipoItem: 1=Produto, 2=Conjunto, 3=Peça
    /// 
    /// Este endpoint é chamado quando o usuário seleciona um item para adicionar na nota.
    /// Retorna todos os impostos calculados baseado no contexto da nota (cliente, UF, etc.)
    /// 
    /// Produtos têm lógica especial de PIS/COFINS por NCM.
    /// Conjuntos e Peças usam lógica padrão.
    /// </remarks>
    [HttpPost("{id}/calcular-imposto")]
    public async Task<ActionResult<CalculoImpostoResultDto>> CalcularImposto(int id, [FromBody] CalculoImpostoRequestDto request)
    {
        try
        {
            _logger.LogInformation("Calculando impostos - NF: {Id}, TipoItem: {TipoItem}, SeqItem: {SeqItem}", 
                id, request.TipoItem, request.SequenciaDoItem);
                
            var resultado = await _calculoImpostoService.CalcularImpostoAsync(id, request);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos para cálculo de imposto na nota {Id}", id);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argumento inválido para cálculo de imposto na nota {Id}", id);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao calcular impostos para nota fiscal {Id}", id);
            // Retorna detalhes do erro para facilitar debug
            return StatusCode(500, new { 
                mensagem = "Erro ao calcular impostos", 
                detalhes = ex.Message,
                stackTrace = ex.StackTrace,
                innerException = ex.InnerException?.Message
            });
        }
    }
    
    #endregion
    
    #region Itens da Nota - Conjuntos
    
    /// <summary>
    /// Lista conjuntos da nota fiscal
    /// </summary>
    [HttpGet("{id}/conjuntos")]
    public async Task<ActionResult<List<ConjuntoDaNotaFiscalDto>>> ListarConjuntos(int id)
    {
        try
        {
            var conjuntos = await _repository.ListarConjuntosAsync(id);
            return Ok(conjuntos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar conjuntos da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao listar conjuntos" });
        }
    }
    
    /// <summary>
    /// Adiciona conjunto à nota fiscal
    /// </summary>
    [HttpPost("{id}/conjuntos")]
    public async Task<ActionResult<ConjuntoDaNotaFiscalDto>> AdicionarConjunto(int id, [FromBody] ConjuntoDaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var conjunto = await _repository.AdicionarConjuntoAsync(id, dto);
            return CreatedAtAction(nameof(ListarConjuntos), new { id }, conjunto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar conjunto à nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao adicionar conjunto" });
        }
    }
    
    /// <summary>
    /// Atualiza conjunto da nota fiscal
    /// </summary>
    [HttpPut("{id}/conjuntos/{sequenciaConjunto}")]
    public async Task<ActionResult<ConjuntoDaNotaFiscalDto>> AtualizarConjunto(int id, int sequenciaConjunto, [FromBody] ConjuntoDaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var conjunto = await _repository.AtualizarConjuntoAsync(id, sequenciaConjunto, dto);
            if (conjunto == null)
                return NotFound(new { mensagem = "Conjunto não encontrado" });
                
            return Ok(conjunto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar conjunto da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao atualizar conjunto" });
        }
    }
    
    /// <summary>
    /// Remove conjunto da nota fiscal
    /// </summary>
    [HttpDelete("{id}/conjuntos/{sequenciaConjunto}")]
    public async Task<ActionResult> RemoverConjunto(int id, int sequenciaConjunto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var result = await _repository.RemoverConjuntoAsync(id, sequenciaConjunto);
            if (!result)
                return NotFound(new { mensagem = "Conjunto não encontrado" });
                
            return Ok(new { mensagem = "Conjunto removido com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover conjunto da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao remover conjunto" });
        }
    }
    
    #endregion
    
    #region Itens da Nota - Peças
    
    /// <summary>
    /// Lista peças da nota fiscal
    /// </summary>
    [HttpGet("{id}/pecas")]
    public async Task<ActionResult<List<PecaDaNotaFiscalDto>>> ListarPecas(int id)
    {
        try
        {
            var pecas = await _repository.ListarPecasAsync(id);
            return Ok(pecas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar peças da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao listar peças" });
        }
    }
    
    /// <summary>
    /// Adiciona peça à nota fiscal
    /// </summary>
    [HttpPost("{id}/pecas")]
    public async Task<ActionResult<PecaDaNotaFiscalDto>> AdicionarPeca(int id, [FromBody] PecaDaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var peca = await _repository.AdicionarPecaAsync(id, dto);
            return CreatedAtAction(nameof(ListarPecas), new { id }, peca);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar peça à nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao adicionar peça" });
        }
    }
    
    /// <summary>
    /// Atualiza peça da nota fiscal
    /// </summary>
    [HttpPut("{id}/pecas/{sequenciaPeca}")]
    public async Task<ActionResult<PecaDaNotaFiscalDto>> AtualizarPeca(int id, int sequenciaPeca, [FromBody] PecaDaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var peca = await _repository.AtualizarPecaAsync(id, sequenciaPeca, dto);
            if (peca == null)
                return NotFound(new { mensagem = "Peça não encontrada" });
                
            return Ok(peca);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar peça da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao atualizar peça" });
        }
    }
    
    /// <summary>
    /// Remove peça da nota fiscal
    /// </summary>
    [HttpDelete("{id}/pecas/{sequenciaPeca}")]
    public async Task<ActionResult> RemoverPeca(int id, int sequenciaPeca)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var result = await _repository.RemoverPecaAsync(id, sequenciaPeca);
            if (!result)
                return NotFound(new { mensagem = "Peça não encontrada" });
                
            return Ok(new { mensagem = "Peça removida com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover peça da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao remover peça" });
        }
    }
    
    #endregion
    
    #region Itens da Nota - Parcelas
    
    /// <summary>
    /// Lista parcelas da nota fiscal
    /// </summary>
    [HttpGet("{id}/parcelas")]
    public async Task<ActionResult<List<ParcelaNotaFiscalDto>>> ListarParcelas(int id)
    {
        try
        {
            var parcelas = await _repository.ListarParcelasAsync(id);
            return Ok(parcelas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar parcelas da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao listar parcelas" });
        }
    }
    
    /// <summary>
    /// Adiciona parcela à nota fiscal
    /// </summary>
    [HttpPost("{id}/parcelas")]
    public async Task<ActionResult<ParcelaNotaFiscalDto>> AdicionarParcela(int id, [FromBody] ParcelaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var parcela = await _repository.AdicionarParcelaAsync(id, dto);
            return CreatedAtAction(nameof(ListarParcelas), new { id }, parcela);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar parcela à nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao adicionar parcela" });
        }
    }
    
    /// <summary>
    /// Atualiza parcela da nota fiscal
    /// </summary>
    [HttpPut("{id}/parcelas/{numeroParcela}")]
    public async Task<ActionResult<ParcelaNotaFiscalDto>> AtualizarParcela(int id, short numeroParcela, [FromBody] ParcelaNotaFiscalCreateDto dto)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var parcela = await _repository.AtualizarParcelaAsync(id, numeroParcela, dto);
            if (parcela == null)
                return NotFound(new { mensagem = "Parcela não encontrada" });
                
            return Ok(parcela);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar parcela da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao atualizar parcela" });
        }
    }
    
    /// <summary>
    /// Remove parcela da nota fiscal
    /// </summary>
    [HttpDelete("{id}/parcelas/{numeroParcela}")]
    public async Task<ActionResult> RemoverParcela(int id, short numeroParcela)
    {
        try
        {
            if (!await _repository.PodeEditarAsync(id))
                return BadRequest(new { mensagem = "Não é possível editar esta nota fiscal" });
                
            var result = await _repository.RemoverParcelaAsync(id, numeroParcela);
            if (!result)
                return NotFound(new { mensagem = "Parcela não encontrada" });
                
            return Ok(new { mensagem = "Parcela removida com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover parcela da nota fiscal {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao remover parcela" });
        }
    }
    
    #endregion
}
