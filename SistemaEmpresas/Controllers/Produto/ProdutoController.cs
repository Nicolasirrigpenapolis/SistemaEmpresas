using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.DTOs.Produto;
using SistemaEmpresas.Repositories.Produto;
using SistemaEmpresas.Services.Common;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers.Produto;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    private readonly IReceitaProdutoRepository _receitaRepository;
    private readonly ILogger<ProdutoController> _logger;
    private readonly ICacheService _cache;
    private readonly AppDbContext _context;

    public ProdutoController(
        IProdutoRepository repository, 
        IReceitaProdutoRepository receitaRepository,
        ILogger<ProdutoController> logger, 
        ICacheService cache, 
        AppDbContext context)
    {
        _repository = repository;
        _receitaRepository = receitaRepository;
        _logger = logger;
        _cache = cache;
        _context = context;
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
        _logger.LogInformation("POST /api/produto - Descricao: {Descricao}", dto.Descricao);

        try
        {
            // Validacoes de campos obrigatorios conforme sistema legado
            if (string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest(new { mensagem = "Descrição do Produto não pode ser vazio" });

            if (dto.SequenciaDoGrupoProduto <= 0)
                return BadRequest(new { mensagem = "Grupo do Produto é obrigatório" });

            if (dto.SequenciaDaUnidade <= 0)
                return BadRequest(new { mensagem = "Unidade do Produto é obrigatória" });

            if (dto.SequenciaDaClassificacao <= 0)
                return BadRequest(new { mensagem = "Classificação Fiscal (NCM) é obrigatória" });

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
            // Validacoes de campos obrigatorios conforme sistema legado
            if (string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest(new { mensagem = "Descrição do Produto não pode ser vazio" });

            if (dto.SequenciaDoGrupoProduto <= 0)
                return BadRequest(new { mensagem = "Grupo do Produto é obrigatório" });

            if (dto.SequenciaDaUnidade <= 0)
                return BadRequest(new { mensagem = "Unidade do Produto é obrigatória" });

            if (dto.SequenciaDaClassificacao <= 0)
                return BadRequest(new { mensagem = "Classificação Fiscal (NCM) é obrigatória" });

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
                return NotFound(new { mensagem = "Produto nao encontrado" });

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

    #region Dados Auxiliares (Grupo, SubGrupo, Unidade)

    /// <summary>
    /// Lista todos os grupos de produto ativos
    /// </summary>
    [HttpGet("grupos")]
    public async Task<ActionResult> ListarGrupos([FromQuery] bool incluirInativos = false)
    {
        try
        {
            var query = _context.GrupoDoProdutos.AsQueryable();

            // Filtrar apenas códigos >= 1 (remover código 0)
            query = query.Where(g => g.SequenciaDoGrupoProduto > 0);

            if (!incluirInativos)
                query = query.Where(g => !g.Inativo);

            var grupos = await query
                .OrderBy(g => g.SequenciaDoGrupoProduto)
                .Select(g => new
                {
                    sequenciaDoGrupoProduto = g.SequenciaDoGrupoProduto,
                    descricao = g.Descricao,
                    inativo = g.Inativo
                })
                .ToListAsync();

            return Ok(grupos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar grupos de produto");
            return StatusCode(500, new { mensagem = "Erro ao listar grupos de produto" });
        }
    }

    /// <summary>
    /// Lista todos os subgrupos de produto, filtrados por grupo
    /// </summary>
    [HttpGet("subgrupos")]
    public async Task<ActionResult> ListarSubGrupos([FromQuery] short? grupoId = null)
    {
        try
        {
            var query = _context.SubGrupoDoProdutos.AsQueryable();

            // Filtrar apenas códigos >= 1 (remover código 0)
            query = query.Where(s => s.SequenciaDoSubGrupoProduto > 0);

            // Se um grupo foi especificado, filtra por ele
            if (grupoId.HasValue && grupoId.Value > 0)
                query = query.Where(s => s.SequenciaDoGrupoProduto == grupoId.Value);

            var subgrupos = await query
                .OrderBy(s => s.SequenciaDoSubGrupoProduto)
                .Select(s => new
                {
                    sequenciaDoSubGrupoProduto = s.SequenciaDoSubGrupoProduto,
                    sequenciaDoGrupoProduto = s.SequenciaDoGrupoProduto,
                    descricao = s.Descricao
                })
                .ToListAsync();

            return Ok(subgrupos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar subgrupos de produto");
            return StatusCode(500, new { mensagem = "Erro ao listar subgrupos de produto" });
        }
    }

    /// <summary>
    /// Lista todas as unidades
    /// </summary>
    [HttpGet("unidades")]
    public async Task<ActionResult> ListarUnidades()
    {
        try
        {
            var unidades = await _context.Unidades
                .Where(u => u.SequenciaDaUnidade > 0) // Filtrar apenas códigos >= 1
                .OrderBy(u => u.SequenciaDaUnidade)
                .Select(u => new
                {
                    sequenciaDaUnidade = u.SequenciaDaUnidade,
                    descricao = u.Descricao,
                    siglaDaUnidade = u.SiglaDaUnidade
                })
                .ToListAsync();

            return Ok(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar unidades");
            return StatusCode(500, new { mensagem = "Erro ao listar unidades" });
        }
    }

    #endregion

    #region Foto do Produto

    /// <summary>
    /// Obtém a foto de um produto
    /// </summary>
    [HttpGet("{id}/foto")]
    public async Task<ActionResult> ObterFoto(int id)
    {
        try
        {
            // Busca o diretório de fotos dos parâmetros
            var parametros = await _context.Parametros.FirstOrDefaultAsync();
            if (parametros == null)
            {
                _logger.LogWarning("Parâmetros não encontrados na tabela");
                return NotFound(new { mensagem = "Parâmetros não configurados" });
            }
            
            _logger.LogInformation("DiretorioFotosProdutos do banco: '{Diretorio}'", parametros.DiretorioFotosProdutos);
            
            if (string.IsNullOrEmpty(parametros.DiretorioFotosProdutos))
            {
                _logger.LogWarning("DiretorioFotosProdutos está vazio");
                return NotFound(new { mensagem = "Diretório de fotos não configurado" });
            }

            var diretorio = parametros.DiretorioFotosProdutos.Trim();
            _logger.LogInformation("Buscando foto no diretório: '{Diretorio}'", diretorio);

            // Extensões suportadas
            string[] extensoes = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            string? caminhoArquivo = null;

            foreach (var ext in extensoes)
            {
                var caminho = Path.Combine(diretorio, $"{id}{ext}");
                _logger.LogInformation("Verificando arquivo: '{Caminho}' - Existe: {Existe}", caminho, System.IO.File.Exists(caminho));
                if (System.IO.File.Exists(caminho))
                {
                    caminhoArquivo = caminho;
                    break;
                }
            }

            if (caminhoArquivo == null)
            {
                _logger.LogWarning("Nenhuma foto encontrada para produto {Id} no diretório {Diretorio}", id, diretorio);
                return NotFound(new { mensagem = "Foto não encontrada" });
            }

            _logger.LogInformation("Foto encontrada: {Caminho}", caminhoArquivo);
            var bytes = await System.IO.File.ReadAllBytesAsync(caminhoArquivo);
            var contentType = GetContentType(caminhoArquivo);

            return File(bytes, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter foto do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter foto do produto" });
        }
    }

    /// <summary>
    /// Verifica se um produto possui foto
    /// </summary>
    [HttpGet("{id}/tem-foto")]
    public async Task<ActionResult> TemFoto(int id)
    {
        try
        {
            // Busca o diretório de fotos dos parâmetros
            var parametros = await _context.Parametros.FirstOrDefaultAsync();
            if (parametros == null || string.IsNullOrEmpty(parametros.DiretorioFotosProdutos))
            {
                _logger.LogWarning("TemFoto: Parâmetros ou DiretorioFotosProdutos não configurado");
                return Ok(new { temFoto = false });
            }

            var diretorio = parametros.DiretorioFotosProdutos.Trim();
            _logger.LogInformation("TemFoto: Verificando diretório '{Diretorio}' para produto {Id}", diretorio, id);

            // Extensões suportadas
            string[] extensoes = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            foreach (var ext in extensoes)
            {
                var caminho = Path.Combine(diretorio, $"{id}{ext}");
                if (System.IO.File.Exists(caminho))
                {
                    _logger.LogInformation("TemFoto: Foto encontrada em '{Caminho}'", caminho);
                    return Ok(new { temFoto = true });
                }
            }

            _logger.LogInformation("TemFoto: Nenhuma foto encontrada para produto {Id}", id);
            return Ok(new { temFoto = false });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar foto do produto {Id}", id);
            return Ok(new { temFoto = false });
        }
    }

    private static string GetContentType(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }

    /// <summary>
    /// Faz upload de uma foto para o produto
    /// </summary>
    [HttpPost("{id}/foto")]
    public async Task<ActionResult> UploadFoto(int id, IFormFile arquivo)
    {
        try
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest(new { mensagem = "Nenhum arquivo enviado" });
            }

            // Valida o tipo do arquivo
            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (!extensoesPermitidas.Contains(extensao))
            {
                return BadRequest(new { mensagem = "Tipo de arquivo não permitido. Use: JPG, PNG, GIF, BMP ou WebP" });
            }

            // Valida tamanho (máximo 10MB)
            if (arquivo.Length > 10 * 1024 * 1024)
            {
                return BadRequest(new { mensagem = "Arquivo muito grande. Máximo permitido: 10MB" });
            }

            // Busca o diretório de fotos dos parâmetros
            var parametros = await _context.Parametros.FirstOrDefaultAsync();
            if (parametros == null || string.IsNullOrEmpty(parametros.DiretorioFotosProdutos))
            {
                return BadRequest(new { mensagem = "Diretório de fotos não configurado. Configure em Configurações do Sistema." });
            }

            var diretorio = parametros.DiretorioFotosProdutos.Trim();

            // Verifica se o diretório existe
            if (!Directory.Exists(diretorio))
            {
                return BadRequest(new { mensagem = $"Diretório de fotos não encontrado: {diretorio}" });
            }

            // Remove fotos antigas do produto (com qualquer extensão)
            foreach (var ext in extensoesPermitidas)
            {
                var caminhoAntigo = Path.Combine(diretorio, $"{id}{ext}");
                if (System.IO.File.Exists(caminhoAntigo))
                {
                    System.IO.File.Delete(caminhoAntigo);
                    _logger.LogInformation("Foto antiga removida: {Caminho}", caminhoAntigo);
                }
            }

            // Salva a nova foto
            var caminhoNovo = Path.Combine(diretorio, $"{id}{extensao}");
            using (var stream = new FileStream(caminhoNovo, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            _logger.LogInformation("Nova foto salva para produto {Id}: {Caminho}", id, caminhoNovo);

            return Ok(new { mensagem = "Foto salva com sucesso", caminho = caminhoNovo });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer upload da foto do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao salvar foto do produto" });
        }
    }

    /// <summary>
    /// Remove a foto de um produto
    /// </summary>
    [HttpDelete("{id}/foto")]
    public async Task<ActionResult> RemoverFoto(int id)
    {
        try
        {
            // Busca o diretório de fotos dos parâmetros
            var parametros = await _context.Parametros.FirstOrDefaultAsync();
            if (parametros == null || string.IsNullOrEmpty(parametros.DiretorioFotosProdutos))
            {
                return NotFound(new { mensagem = "Diretório de fotos não configurado" });
            }

            var diretorio = parametros.DiretorioFotosProdutos.Trim();
            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var fotoRemovida = false;

            foreach (var ext in extensoesPermitidas)
            {
                var caminho = Path.Combine(diretorio, $"{id}{ext}");
                if (System.IO.File.Exists(caminho))
                {
                    System.IO.File.Delete(caminho);
                    fotoRemovida = true;
                    _logger.LogInformation("Foto removida: {Caminho}", caminho);
                }
            }

            if (fotoRemovida)
            {
                return Ok(new { mensagem = "Foto removida com sucesso" });
            }
            else
            {
                return NotFound(new { mensagem = "Nenhuma foto encontrada para remover" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover foto do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao remover foto do produto" });
        }
    }

    #endregion

    #region Receita do Produto (Materia Prima)

    /// <summary>
    /// Obtem a receita completa de um produto (materias primas)
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <returns>Receita do produto com todos os itens</returns>
    [HttpGet("{id}/receita")]
    public async Task<ActionResult<ReceitaProdutoDto>> ObterReceita(int id)
    {
        _logger.LogInformation("GET /api/produto/{Id}/receita", id);

        try
        {
            // Verifica se o produto existe
            var produto = await _repository.ObterPorIdAsync(id);
            if (produto == null)
            {
                return NotFound(new { mensagem = "Produto nao encontrado" });
            }

            var receita = await _receitaRepository.ObterReceitaAsync(id);
            return Ok(receita);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter receita do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao obter receita do produto" });
        }
    }

    /// <summary>
    /// Lista os itens da receita de um produto
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <returns>Lista de itens da receita</returns>
    [HttpGet("{id}/receita/itens")]
    public async Task<ActionResult<List<ReceitaProdutoListDto>>> ListarItensReceita(int id)
    {
        _logger.LogInformation("GET /api/produto/{Id}/receita/itens", id);

        try
        {
            var itens = await _receitaRepository.ListarItensAsync(id);
            return Ok(itens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar itens da receita do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao listar itens da receita" });
        }
    }

    /// <summary>
    /// Adiciona um item a receita do produto
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <param name="dto">Dados do item a adicionar</param>
    /// <returns>Item adicionado</returns>
    [HttpPost("{id}/receita")]
    public async Task<ActionResult<ReceitaProdutoListDto>> AdicionarItemReceita(int id, [FromBody] ReceitaProdutoCreateUpdateDto dto)
    {
        _logger.LogInformation("POST /api/produto/{Id}/receita - MateriaPrima: {MateriaPrima}", id, dto.SequenciaDaMateriaPrima);

        try
        {
            // Verifica se o produto existe
            var produto = await _repository.ObterPorIdAsync(id);
            if (produto == null)
            {
                return NotFound(new { mensagem = "Produto nao encontrado" });
            }

            // Valida quantidade
            if (dto.Quantidade <= 0)
            {
                return BadRequest(new { mensagem = "A quantidade deve ser maior que zero" });
            }

            // Valida materia prima
            if (dto.SequenciaDaMateriaPrima <= 0)
            {
                return BadRequest(new { mensagem = "Materia prima invalida" });
            }

            var item = await _receitaRepository.AdicionarItemAsync(id, dto);
            return CreatedAtAction(nameof(ObterReceita), new { id }, item);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Operacao invalida ao adicionar item a receita: {Mensagem}", ex.Message);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar item a receita do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao adicionar item a receita" });
        }
    }

    /// <summary>
    /// Atualiza a quantidade de um item da receita
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <param name="materiaPrimaId">ID da materia prima</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Item atualizado</returns>
    [HttpPut("{id}/receita/{materiaPrimaId}")]
    public async Task<ActionResult<ReceitaProdutoListDto>> AtualizarItemReceita(int id, int materiaPrimaId, [FromBody] ReceitaProdutoCreateUpdateDto dto)
    {
        _logger.LogInformation("PUT /api/produto/{Id}/receita/{MateriaPrimaId}", id, materiaPrimaId);

        try
        {
            // Valida quantidade
            if (dto.Quantidade <= 0)
            {
                return BadRequest(new { mensagem = "A quantidade deve ser maior que zero" });
            }

            var item = await _receitaRepository.AtualizarItemAsync(id, materiaPrimaId, dto);
            if (item == null)
            {
                return NotFound(new { mensagem = "Item da receita nao encontrado" });
            }

            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar item da receita do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao atualizar item da receita" });
        }
    }

    /// <summary>
    /// Remove um item da receita
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <param name="materiaPrimaId">ID da materia prima a remover</param>
    /// <returns>Resultado da operacao</returns>
    [HttpDelete("{id}/receita/{materiaPrimaId}")]
    public async Task<ActionResult> RemoverItemReceita(int id, int materiaPrimaId)
    {
        _logger.LogInformation("DELETE /api/produto/{Id}/receita/{MateriaPrimaId}", id, materiaPrimaId);

        try
        {
            var removido = await _receitaRepository.RemoverItemAsync(id, materiaPrimaId);
            if (!removido)
            {
                return NotFound(new { mensagem = "Item da receita nao encontrado" });
            }

            return Ok(new { mensagem = "Item removido da receita com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover item da receita do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao remover item da receita" });
        }
    }

    /// <summary>
    /// Remove todos os itens da receita de um produto (limpa a receita)
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <returns>Quantidade de itens removidos</returns>
    [HttpDelete("{id}/receita")]
    public async Task<ActionResult> LimparReceita(int id)
    {
        _logger.LogInformation("DELETE /api/produto/{Id}/receita", id);

        try
        {
            // Verifica se o produto existe
            var produto = await _repository.ObterPorIdAsync(id);
            if (produto == null)
            {
                return NotFound(new { mensagem = "Produto nao encontrado" });
            }

            var quantidade = await _receitaRepository.LimparReceitaAsync(id);
            return Ok(new { mensagem = $"Receita limpa com sucesso. {quantidade} itens removidos." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao limpar receita do produto {Id}", id);
            return StatusCode(500, new { mensagem = "Erro ao limpar receita do produto" });
        }
    }

    #endregion
}
