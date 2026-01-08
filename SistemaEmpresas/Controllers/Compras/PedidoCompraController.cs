using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs.Compras;

namespace SistemaEmpresas.Controllers.Compras;

[Authorize]
[ApiController]
[Route("api/pedidos-compra")]
public class PedidoCompraController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidoCompraController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista pedidos de compra pendentes de recebimento (para tela de Entrada de Estoque)
    /// </summary>
    [HttpGet("pendentes")]
    public async Task<ActionResult<List<PedidoCompraPendenteDto>>> ListarPendentes(
        [FromQuery] int? fornecedorId = null,
        [FromQuery] string? busca = null)
    {
        var query = from pc in _context.PedidoDeCompraNovos
                    join g in _context.Gerals on pc.CodigoDoFornecedor equals g.SequenciaDoGeral
                    where !pc.PedidoFechado && !pc.Cancelado
                    select new { Pedido = pc, Fornecedor = g };

        if (fornecedorId.HasValue)
            query = query.Where(x => x.Pedido.CodigoDoFornecedor == fornecedorId.Value);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.ToLower();
            query = query.Where(x =>
                x.Pedido.IdDoPedido.ToString().Contains(termo) ||
                x.Fornecedor.RazaoSocial.ToLower().Contains(termo));
        }

        var pedidos = await query
            .OrderByDescending(x => x.Pedido.DataDoPedido)
            .Take(50)
            .Select(x => new PedidoCompraPendenteDto
            {
                IdDoPedido = x.Pedido.IdDoPedido,
                DataDoPedido = x.Pedido.DataDoPedido,
                CodigoDoFornecedor = x.Pedido.CodigoDoFornecedor,
                NomeFornecedor = x.Fornecedor.RazaoSocial,
                CnpjFornecedor = x.Fornecedor.CpfECnpj ?? "",
                TotalDoPedido = x.Pedido.TotalDoPedido,
                PrevisaoDeEntrega = x.Pedido.PrevisaoDeEntrega
            })
            .ToListAsync();

        // Carregar contagem de itens pendentes para cada pedido
        var pedidoIds = pedidos.Select(p => p.IdDoPedido).ToList();
        
        var itensPendentes = await _context.BxProdutosPedidoCompras
            .Where(b => pedidoIds.Contains(b.IdDoPedido) && b.QtdeRestante > 0)
            .GroupBy(b => b.IdDoPedido)
            .Select(g => new { IdDoPedido = g.Key, Qtde = g.Count() })
            .ToListAsync();

        foreach (var pedido in pedidos)
        {
            pedido.QtdeItensPendentes = itensPendentes
                .FirstOrDefault(i => i.IdDoPedido == pedido.IdDoPedido)?.Qtde ?? 0;
        }

        // Filtrar apenas pedidos que têm itens pendentes
        pedidos = pedidos.Where(p => p.QtdeItensPendentes > 0).ToList();

        return Ok(pedidos);
    }

    /// <summary>
    /// Obtém detalhes de um pedido de compra com seus itens pendentes
    /// </summary>
    [HttpGet("{id}/itens-pendentes")]
    public async Task<ActionResult<PedidoCompraComItensPendentesDto>> ObterItensPendentes(int id)
    {
        var pedido = await (from pc in _context.PedidoDeCompraNovos
                           join g in _context.Gerals on pc.CodigoDoFornecedor equals g.SequenciaDoGeral
                           where pc.IdDoPedido == id
                           select new PedidoCompraComItensPendentesDto
                           {
                               IdDoPedido = pc.IdDoPedido,
                               DataDoPedido = pc.DataDoPedido,
                               CodigoDoFornecedor = pc.CodigoDoFornecedor,
                               NomeFornecedor = g.RazaoSocial,
                               CnpjFornecedor = g.CpfECnpj ?? "",
                               TotalDoPedido = pc.TotalDoPedido
                           }).FirstOrDefaultAsync();

        if (pedido == null)
            return NotFound("Pedido não encontrado.");

        // Buscar itens pendentes do pedido
        pedido.Itens = await (from bx in _context.BxProdutosPedidoCompras
                              join p in _context.Produtos on bx.IdDoProduto equals p.SequenciaDoProduto
                              join u in _context.Unidades on p.SequenciaDaUnidade equals u.SequenciaDaUnidade into units
                              from u in units.DefaultIfEmpty()
                              where bx.IdDoPedido == id && bx.QtdeRestante > 0
                              select new ItemPedidoPendenteDto
                              {
                                  IdDoProduto = bx.IdDoProduto,
                                  SequenciaDoItem = bx.SequenciaDoItem,
                                  CodigoDoProduto = p.SequenciaDoProduto.ToString(),
                                  DescricaoProduto = p.Descricao ?? "",
                                  UnidadeMedida = u.SiglaDaUnidade ?? "UN",
                                  QtdePedida = bx.QtdeTotal,
                                  QtdeRecebida = bx.QtdeRecebida,
                                  QtdeRestante = bx.QtdeRestante,
                                  VrUnitario = bx.VrUnitario,
                                  VrTotalRestante = bx.TotalRestante,
                                  NotasAnteriores = bx.Notas ?? ""
                              }).ToListAsync();

        return Ok(pedido);
    }

    /// <summary>
    /// Busca pedidos por fornecedor (para autocomplete)
    /// </summary>
    [HttpGet("buscar")]
    public async Task<ActionResult<List<PedidoCompraPendenteDto>>> BuscarPedidos(
        [FromQuery] string termo,
        [FromQuery] int limite = 10)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Ok(new List<PedidoCompraPendenteDto>());

        var termoLower = termo.ToLower();

        var pedidos = await (from pc in _context.PedidoDeCompraNovos
                            join g in _context.Gerals on pc.CodigoDoFornecedor equals g.SequenciaDoGeral
                            where !pc.PedidoFechado && !pc.Cancelado &&
                                  (pc.IdDoPedido.ToString().Contains(termo) ||
                                   g.RazaoSocial.ToLower().Contains(termoLower) ||
                                   (g.CpfECnpj != null && g.CpfECnpj.Contains(termo)))
                            orderby pc.DataDoPedido descending
                            select new PedidoCompraPendenteDto
                            {
                                IdDoPedido = pc.IdDoPedido,
                                DataDoPedido = pc.DataDoPedido,
                                CodigoDoFornecedor = pc.CodigoDoFornecedor,
                                NomeFornecedor = g.RazaoSocial,
                                CnpjFornecedor = g.CpfECnpj ?? "",
                                TotalDoPedido = pc.TotalDoPedido,
                                PrevisaoDeEntrega = pc.PrevisaoDeEntrega
                            })
                            .Take(limite)
                            .ToListAsync();

        return Ok(pedidos);
    }
}

#region DTOs específicos para Entrada de Estoque

public class PedidoCompraPendenteDto
{
    public int IdDoPedido { get; set; }
    public DateTime DataDoPedido { get; set; }
    public int CodigoDoFornecedor { get; set; }
    public string NomeFornecedor { get; set; } = string.Empty;
    public string CnpjFornecedor { get; set; } = string.Empty;
    public decimal TotalDoPedido { get; set; }
    public DateTime? PrevisaoDeEntrega { get; set; }
    public int QtdeItensPendentes { get; set; }
}

public class PedidoCompraComItensPendentesDto
{
    public int IdDoPedido { get; set; }
    public DateTime DataDoPedido { get; set; }
    public int CodigoDoFornecedor { get; set; }
    public string NomeFornecedor { get; set; } = string.Empty;
    public string CnpjFornecedor { get; set; } = string.Empty;
    public decimal TotalDoPedido { get; set; }
    public List<ItemPedidoPendenteDto> Itens { get; set; } = new();
}

public class ItemPedidoPendenteDto
{
    public int IdDoProduto { get; set; }
    public int SequenciaDoItem { get; set; }
    public string CodigoDoProduto { get; set; } = string.Empty;
    public string DescricaoProduto { get; set; } = string.Empty;
    public string UnidadeMedida { get; set; } = string.Empty;
    public decimal QtdePedida { get; set; }
    public decimal QtdeRecebida { get; set; }
    public decimal QtdeRestante { get; set; }
    public decimal VrUnitario { get; set; }
    public decimal VrTotalRestante { get; set; }
    public string NotasAnteriores { get; set; } = string.Empty;
    
    // Campos para preenchimento na entrada
    public decimal? QtdeAReceber { get; set; }
    public string? NotaFiscal { get; set; }
}

#endregion
