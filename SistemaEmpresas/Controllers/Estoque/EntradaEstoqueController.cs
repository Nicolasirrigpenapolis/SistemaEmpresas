using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs.Fiscal;
using SistemaEmpresas.Models;
using SistemaEmpresas.Services.Fiscal;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers.Estoque;

[Authorize]
[ApiController]
[Route("api/estoque/entrada")]
public class EntradaEstoqueController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly INFeXmlParserService _xmlParser;
    private readonly ISefazService _sefazService;

    public EntradaEstoqueController(
        AppDbContext context, 
        INFeXmlParserService xmlParser,
        ISefazService sefazService)
    {
        _context = context;
        _xmlParser = xmlParser;
        _sefazService = sefazService;
    }

    /// <summary>
    /// Busca a NFe diretamente da SEFAZ pela chave de acesso
    /// </summary>
    [HttpGet("buscar-sefaz/{chaveAcesso}")]
    public async Task<ActionResult<EntradaXmlResultDto>> BuscarSefaz(string chaveAcesso)
    {
        try
        {
            using var stream = await _sefazService.BuscarNFePorChaveAsync(chaveAcesso);
            
            if (stream == null)
                return NotFound(new { message = "NFe não encontrada na SEFAZ ou erro na comunicação. Verifique se o certificado digital está configurado corretamente." });

            return await ProcessarXmlStream(stream);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Erro ao buscar NFe na SEFAZ: {ex.Message}" });
        }
    }

    /// <summary>
    /// Faz upload e parsing do XML da NFe, retornando os dados para conferência
    /// </summary>
    [HttpPost("upload-xml")]
    public async Task<ActionResult<EntradaXmlResultDto>> UploadXml(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        if (!arquivo.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            return BadRequest("O arquivo deve ser um XML de NFe.");

        try
        {
            using var stream = arquivo.OpenReadStream();
            return await ProcessarXmlStream(stream);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao processar XML: {ex.Message}");
        }
    }

    private async Task<ActionResult<EntradaXmlResultDto>> ProcessarXmlStream(Stream stream)
    {
        var nfeData = _xmlParser.Parse(stream);

        // Verificar se a nota já foi importada
        var notaExistente = await _context.MovimentoContabilNovos
            .AnyAsync(m => m.Documento == nfeData.NumeroNota && 
                          m.TipoDoMovimento == 0); // Entrada

        // Tentar encontrar fornecedor pelo CNPJ
        var fornecedor = await _context.Gerals
            .Where(g => g.CpfECnpj != null && 
                       g.CpfECnpj.Replace(".", "").Replace("-", "").Replace("/", "") == 
                       nfeData.Emitente.Cnpj.Replace(".", "").Replace("-", "").Replace("/", ""))
            .Select(g => new { g.SequenciaDoGeral, Nome = g.RazaoSocial })
            .FirstOrDefaultAsync();

        // Tentar vincular produtos automaticamente
        foreach (var item in nfeData.Itens)
        {
            // 1. Tentar pelo vínculo salvo anteriormente para este fornecedor
            if (fornecedor != null)
            {
                var vinculo = await _context.VinculoProdutoFornecedors
                    .Where(v => v.SequenciaDoGeral == fornecedor.SequenciaDoGeral && 
                               v.CodigoProdutoFornecedor == item.CodigoProdutoFornecedor)
                    .Select(v => new { v.SequenciaDoProduto, v.Produto.Descricao })
                    .FirstOrDefaultAsync();

                if (vinculo != null)
                {
                    item.ProdutoIdSistema = vinculo.SequenciaDoProduto;
                    item.NomeProdutoSistema = vinculo.Descricao;
                    continue;
                }
            }

            // 2. Tentar pelo código do produto (se o fornecedor usar o mesmo código que o nosso)
            var produto = await _context.Produtos
                .Where(p => p.SequenciaDoProduto.ToString() == item.CodigoProdutoFornecedor)
                .Select(p => new { p.SequenciaDoProduto, p.Descricao })
                .FirstOrDefaultAsync();

            if (produto != null)
            {
                item.ProdutoIdSistema = produto.SequenciaDoProduto;
                item.NomeProdutoSistema = produto.Descricao;
            }
        }

        var resultado = new EntradaXmlResultDto
        {
            NfeData = nfeData,
            NotaJaImportada = notaExistente,
            FornecedorEncontrado = fornecedor != null,
            FornecedorId = fornecedor?.SequenciaDoGeral,
            FornecedorNome = fornecedor?.Nome,
            ItensVinculados = nfeData.Itens.Count(i => i.ProdutoIdSistema.HasValue),
            ItensSemVinculo = nfeData.Itens.Count(i => !i.ProdutoIdSistema.HasValue)
        };

        return Ok(resultado);
    }

    /// <summary>
    /// Compara o XML importado com um pedido de compra existente
    /// </summary>
    [HttpPost("comparar-pedido")]
    public async Task<ActionResult<ComparacaoPedidoResultDto>> CompararComPedido([FromBody] CompararPedidoRequest request)
    {
        // Buscar itens do pedido
        var itensPedido = await (from bx in _context.BxProdutosPedidoCompras
                                 join p in _context.Produtos on bx.IdDoProduto equals p.SequenciaDoProduto
                                 join u in _context.Unidades on p.SequenciaDaUnidade equals u.SequenciaDaUnidade into units
                                 from u in units.DefaultIfEmpty()
                                 where bx.IdDoPedido == request.IdPedido && bx.QtdeRestante > 0
                                 select new ItemComparacaoDto
                                 {
                                     IdDoProduto = bx.IdDoProduto,
                                     SequenciaDoItem = bx.SequenciaDoItem,
                                     CodigoProduto = p.SequenciaDoProduto.ToString(),
                                     DescricaoProduto = p.Descricao ?? "",
                                     UnidadeMedida = u.SiglaDaUnidade ?? "UN",
                                     QtdePedido = bx.QtdeRestante,
                                     VrUnitarioPedido = bx.VrUnitario
                                 }).ToListAsync();

        // Comparar com itens do XML
        foreach (var itemPedido in itensPedido)
        {
            // Tentar encontrar correspondência no XML
            var itemXml = request.ItensXml.FirstOrDefault(x => 
                x.ProdutoIdSistema == itemPedido.IdDoProduto);

            if (itemXml != null)
            {
                itemPedido.EncontradoNoXml = true;
                itemPedido.QtdeXml = itemXml.Quantidade;
                itemPedido.VrUnitarioXml = itemXml.ValorUnitario;
                itemPedido.DiferencaQtde = itemXml.Quantidade - itemPedido.QtdePedido;
                itemPedido.DiferencaPreco = itemXml.ValorUnitario - itemPedido.VrUnitarioPedido;
                itemPedido.CodigoProdutoXml = itemXml.CodigoProdutoFornecedor;
                itemPedido.DescricaoProdutoXml = itemXml.DescricaoProdutoFornecedor;
            }
        }

        // Itens do XML que não estão no pedido
        var itensExtras = request.ItensXml
            .Where(x => x.ProdutoIdSistema.HasValue && 
                       !itensPedido.Any(p => p.IdDoProduto == x.ProdutoIdSistema))
            .Select(x => new ItemComparacaoDto
            {
                IdDoProduto = x.ProdutoIdSistema!.Value,
                CodigoProduto = x.CodigoProdutoFornecedor,
                DescricaoProduto = x.NomeProdutoSistema ?? x.DescricaoProdutoFornecedor,
                QtdeXml = x.Quantidade,
                VrUnitarioXml = x.ValorUnitario,
                EncontradoNoXml = true,
                ItemExtra = true
            }).ToList();

        return Ok(new ComparacaoPedidoResultDto
        {
            IdPedido = request.IdPedido,
            Itens = itensPedido,
            ItensExtras = itensExtras,
            TotalItensConferidos = itensPedido.Count(i => i.EncontradoNoXml),
            TotalItensNaoEncontrados = itensPedido.Count(i => !i.EncontradoNoXml),
            TotalItensExtras = itensExtras.Count
        });
    }

    /// <summary>
    /// Confirma a entrada de estoque, gerando o MovimentoContabil e baixando o pedido
    /// </summary>
    [HttpPost("confirmar")]
    public async Task<ActionResult<EntradaConfirmadaDto>> ConfirmarEntrada([FromBody] ConfirmarEntradaRequest request)
    {
        if (!request.IdPedido.HasValue || request.IdPedido.Value <= 0)
        {
            return BadRequest("O Pedido de Compra é obrigatório para realizar a entrada.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
            var agora = DateTime.Now;

            // 1. Criar Movimento Contábil
            var movimento = new MovimentoContabilNovo
            {
                TipoDoMovimento = 0, // Entrada
                Documento = request.NumeroNota,
                SequenciaDoGeral = request.FornecedorId,
                Observacao = $"Entrada via XML NFe. Chave: {request.ChaveAcesso}",
                DataDoMovimento = agora,
                ValorTotalDasDespesas = 0,
                ValorTotalDosProdutos = request.Itens.Sum(i => i.ValorTotal),
                ValorTotalDoMovimento = request.ValorTotal,
                UsuarioDaAlteracao = usuario,
                DataDaAlteracao = agora,
                HoraDaAlteracao = agora,
                FormaDePagamento = "A VISTA", // Default
                Titulo = "ENTRADA XML",
                SequenciaDaCompra = request.IdPedido.Value
            };

            _context.MovimentoContabilNovos.Add(movimento);
            await _context.SaveChangesAsync();

            foreach (var item in request.Itens)
            {
                if (item.ProdutoId <= 0) continue;

                // 2. Salvar/Atualizar Vínculo (De/Para)
                var vinculoExistente = await _context.VinculoProdutoFornecedors
                    .AnyAsync(v => v.SequenciaDoGeral == request.FornecedorId && 
                                  v.SequenciaDoProduto == item.ProdutoId);
                
                if (!vinculoExistente && !string.IsNullOrEmpty(item.CodigoProdutoFornecedor))
                {
                    _context.VinculoProdutoFornecedors.Add(new VinculoProdutoFornecedor
                    {
                        SequenciaDoGeral = request.FornecedorId,
                        SequenciaDoProduto = item.ProdutoId,
                        CodigoProdutoFornecedor = item.CodigoProdutoFornecedor,
                        DataDoVinculo = agora
                    });
                }

                // 3. Adicionar Itens do Movimento
                var itemMovimento = new ProdutoMvtoContabilNovo
                {
                    SequenciaDoMovimento = movimento.SequenciaDoMovimento,
                    SequenciaDoProduto = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorDeCusto = item.ValorUnitario,
                    ValorTotal = item.ValorTotal,
                    ValorDoIpi = item.ValorIpi,
                    ValorDoIcms = item.ValorIcms
                };
                _context.ProdutosMvtoContabilNovos.Add(itemMovimento);

                // 4. Registrar Baixa no Estoque (Entrada)
                var baixa = new BaixaDoEstoqueContabil
                {
                    SequenciaDoMovimento = movimento.SequenciaDoMovimento,
                    SequenciaDoProduto = item.ProdutoId,
                    SequenciaDoGeral = request.FornecedorId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorDeCusto = item.ValorUnitario,
                    ValorTotal = item.ValorTotal,
                    DataDoMovimento = request.DataEntrada ?? agora,
                    Documento = request.NumeroNota,
                    TipoDoMovimento = 0, // Entrada
                    Estoque = "S", // Sim, movimenta estoque
                    Observacao = $"Entrada NFe {request.NumeroNota}"
                };
                _context.BaixaDoEstoqueContabils.Add(baixa);

                // 5. Atualizar Cadastro do Produto
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto != null)
                {
                    produto.UltimaCompra = agora;
                    produto.UltimoFornecedor = request.FornecedorId;
                    produto.ValorDeCusto = item.ValorUnitario; // Atualiza custo
                    produto.QuantidadeNoEstoque += item.Quantidade; // Atualiza estoque físico
                    produto.QuantidadeContabil += item.Quantidade; // Atualiza estoque contábil
                    
                    // Atualiza Valor de Venda (Valor Total) baseado na Margem de Lucro
                    // Conforme lógica do VB6: [Valor Total] = [Valor de Custo] * [Margem de Lucro]
                    produto.ValorTotal = produto.ValorDeCusto * produto.MargemDeLucro;
                }

                // 6. Se vinculado a pedido, atualizar a tabela de baixa do pedido
                if (item.SequenciaItemPedido.HasValue)
                {
                    var bxPedido = await _context.BxProdutosPedidoCompras
                        .FirstOrDefaultAsync(b => b.IdDoPedido == request.IdPedido.Value &&
                                                  b.IdDoProduto == item.ProdutoId &&
                                                  b.SequenciaDoItem == item.SequenciaItemPedido.Value);

                    if (bxPedido != null)
                    {
                        bxPedido.QtdeRecebida += item.Quantidade;
                        bxPedido.QtdeRestante -= item.Quantidade;
                        if (bxPedido.QtdeRestante < 0) bxPedido.QtdeRestante = 0;
                        bxPedido.TotalRestante = bxPedido.QtdeRestante * bxPedido.VrUnitario;
                        
                        // Concatenar nota fiscal às notas anteriores
                        if (!string.IsNullOrEmpty(bxPedido.Notas))
                            bxPedido.Notas += $", {request.NumeroNota}";
                        else
                            bxPedido.Notas = request.NumeroNota;
                    }
                }
            }

            // 7. Verificar se o pedido foi totalmente recebido
            var temPendente = await _context.BxProdutosPedidoCompras
                .AnyAsync(b => b.IdDoPedido == request.IdPedido.Value && b.QtdeRestante > 0);

            if (!temPendente)
            {
                var pedido = await _context.PedidoDeCompraNovos
                    .FirstOrDefaultAsync(p => p.IdDoPedido == request.IdPedido.Value);
                
                if (pedido != null)
                {
                    pedido.PedidoFechado = true;
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new EntradaConfirmadaDto
            {
                Sucesso = true,
                SequenciaDoMovimento = movimento.SequenciaDoMovimento,
                Mensagem = $"Entrada realizada com sucesso! Movimento #{movimento.SequenciaDoMovimento}",
                ItensProcessados = request.Itens.Count(i => i.Receber),
                PedidoFechado = !await _context.BxProdutosPedidoCompras
                                   .AnyAsync(b => b.IdDoPedido == request.IdPedido.Value && b.QtdeRestante > 0)
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest($"Erro ao confirmar entrada: {ex.Message}");
        }
    }

    /// <summary>
    /// Busca produtos para vincular manualmente (De/Para)
    /// </summary>
    [HttpGet("buscar-produto")]
    public async Task<ActionResult<List<ProdutoBuscaDto>>> BuscarProdutos([FromQuery] string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Ok(new List<ProdutoBuscaDto>());

        var termoLower = termo.ToLower();

        var produtos = await _context.Produtos
            .Include(p => p.SequenciaDaUnidadeNavigation)
            .Where(p => p.SequenciaDoProduto.ToString().Contains(termo) ||
                       (p.Descricao != null && p.Descricao.ToLower().Contains(termoLower)))
            .Take(20)
            .Select(p => new ProdutoBuscaDto
            {
                SequenciaDoProduto = p.SequenciaDoProduto,
                CodigoDoProduto = p.SequenciaDoProduto.ToString(),
                Descricao = p.Descricao ?? "",
                UnidadeMedida = p.SequenciaDaUnidadeNavigation.SiglaDaUnidade ?? "UN",
                ValorCusto = p.ValorDeCusto,
                MargemDeLucro = p.MargemDeLucro
            })
            .ToListAsync();

        return Ok(produtos);
    }
}

#region DTOs de Entrada de Estoque

public class EntradaXmlResultDto
{
    public NFeImportDto NfeData { get; set; } = new();
    public bool NotaJaImportada { get; set; }
    public bool FornecedorEncontrado { get; set; }
    public int? FornecedorId { get; set; }
    public string? FornecedorNome { get; set; }
    public int ItensVinculados { get; set; }
    public int ItensSemVinculo { get; set; }
}

public class CompararPedidoRequest
{
    public int IdPedido { get; set; }
    public List<NFeItemDto> ItensXml { get; set; } = new();
}

public class ComparacaoPedidoResultDto
{
    public int IdPedido { get; set; }
    public List<ItemComparacaoDto> Itens { get; set; } = new();
    public List<ItemComparacaoDto> ItensExtras { get; set; } = new();
    public int TotalItensConferidos { get; set; }
    public int TotalItensNaoEncontrados { get; set; }
    public int TotalItensExtras { get; set; }
}

public class ItemComparacaoDto
{
    public int IdDoProduto { get; set; }
    public int SequenciaDoItem { get; set; }
    public string CodigoProduto { get; set; } = string.Empty;
    public string DescricaoProduto { get; set; } = string.Empty;
    public string UnidadeMedida { get; set; } = string.Empty;
    
    // Dados do Pedido
    public decimal QtdePedido { get; set; }
    public decimal VrUnitarioPedido { get; set; }
    
    // Dados do XML
    public bool EncontradoNoXml { get; set; }
    public decimal QtdeXml { get; set; }
    public decimal VrUnitarioXml { get; set; }
    public string? CodigoProdutoXml { get; set; }
    public string? DescricaoProdutoXml { get; set; }
    
    // Diferenças
    public decimal DiferencaQtde { get; set; }
    public decimal DiferencaPreco { get; set; }
    
    // Flags
    public bool ItemExtra { get; set; }
}

public class ConfirmarEntradaRequest
{
    public string NumeroNota { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string ChaveAcesso { get; set; } = string.Empty;
    public DateTime? DataEntrada { get; set; }
    public int FornecedorId { get; set; }
    public int? IdPedido { get; set; }
    public decimal ValorFrete { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemEntradaDto> Itens { get; set; } = new();
}

public class ItemEntradaDto
{
    public int ProdutoId { get; set; }
    public string? CodigoProdutoFornecedor { get; set; }
    public int? SequenciaItemPedido { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal AliquotaIcms { get; set; }
    public decimal ValorIcms { get; set; }
    public decimal AliquotaIpi { get; set; }
    public decimal ValorIpi { get; set; }
    public bool Receber { get; set; } = true;
}

public class EntradaConfirmadaDto
{
    public bool Sucesso { get; set; }
    public int SequenciaDoMovimento { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public int ItensProcessados { get; set; }
    public bool PedidoFechado { get; set; }
}

public class ProdutoBuscaDto
{
    public int SequenciaDoProduto { get; set; }
    public string CodigoDoProduto { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string UnidadeMedida { get; set; } = string.Empty;
    public decimal ValorCusto { get; set; }
    public decimal MargemDeLucro { get; set; }
}

#endregion
