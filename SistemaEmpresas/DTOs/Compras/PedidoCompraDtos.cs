namespace SistemaEmpresas.DTOs.Compras;

#region DTOs de Listagem

/// <summary>
/// DTO para listagem de pedidos de compra no grid
/// </summary>
public class PedidoCompraListDto
{
    public int IdDoPedido { get; set; }
    public DateTime DataDoPedido { get; set; }
    public string NroDaLicitacao { get; set; } = string.Empty;
    
    // Fornecedor
    public int CodigoDoFornecedor { get; set; }
    public string NomeFornecedor { get; set; } = string.Empty;
    public string CnpjFornecedor { get; set; } = string.Empty;
    
    // Transportadora
    public int CodigoDaTransportadora { get; set; }
    public string NomeTransportadora { get; set; } = string.Empty;
    
    // Comprador
    public string Comprador { get; set; } = string.Empty;
    
    // Valores
    public decimal TotalDosProdutos { get; set; }
    public decimal TotalDasDespesas { get; set; }
    public decimal VrDoFrete { get; set; }
    public decimal VrDoDesconto { get; set; }
    public decimal TotalDoIpi { get; set; }
    public decimal TotalDoIcms { get; set; }
    public decimal TotalDoPedido { get; set; }
    
    // Status
    public bool PedidoFechado { get; set; }
    public bool Validado { get; set; }
    public bool Cancelado { get; set; }
    public bool Prepedido { get; set; }
    
    // Entrega
    public DateTime? PrevisaoDeEntrega { get; set; }
    public int DiasAtraso => PrevisaoDeEntrega.HasValue && !PedidoFechado && DateTime.Now > PrevisaoDeEntrega.Value 
        ? (int)(DateTime.Now - PrevisaoDeEntrega.Value).TotalDays 
        : 0;
    
    // Contagem de itens
    public int QtdeProdutos { get; set; }
    public int QtdeDespesas { get; set; }
}

#endregion

#region DTO Completo

/// <summary>
/// DTO completo para visualização e edição de pedido de compra
/// </summary>
public class PedidoCompraDto
{
    // Identificação
    public int IdDoPedido { get; set; }
    public DateTime DataDoPedido { get; set; }
    public string NroDaLicitacao { get; set; } = string.Empty;
    public int CodigoDaLicitacao { get; set; }
    
    // Fornecedor
    public int CodigoDoFornecedor { get; set; }
    public string NomeFornecedor { get; set; } = string.Empty;
    public string CnpjFornecedor { get; set; } = string.Empty;
    
    // Transportadora
    public int CodigoDaTransportadora { get; set; }
    public string NomeTransportadora { get; set; } = string.Empty;
    
    // Responsáveis
    public string Comprador { get; set; } = string.Empty;
    public string Vend { get; set; } = string.Empty;
    
    // Condições
    public string Prazo { get; set; } = string.Empty;
    public string CifFob { get; set; } = string.Empty;
    
    // Valores
    public decimal VrDoFrete { get; set; }
    public decimal VrDoDesconto { get; set; }
    public decimal TotalDosProdutos { get; set; }
    public decimal TotalDasDespesas { get; set; }
    public decimal TotalDoIpi { get; set; }
    public decimal TotalDoIcms { get; set; }
    public decimal TotalDoPedido { get; set; }
    
    // Endereço de Entrega
    public string EnderecoDeEntrega { get; set; } = string.Empty;
    public string NumeroDoEndereco { get; set; } = string.Empty;
    public string BairroDeEntrega { get; set; } = string.Empty;
    public string CidadeDeEntrega { get; set; } = string.Empty;
    public string UfDeEntrega { get; set; } = string.Empty;
    public string CepDeEntrega { get; set; } = string.Empty;
    public string FoneDeEntrega { get; set; } = string.Empty;
    public string ContatoDeEntrega { get; set; } = string.Empty;
    
    // Datas
    public DateTime? PrevisaoDeEntrega { get; set; }
    public DateTime? NovaPrevisao { get; set; }
    public short Dias { get; set; }
    
    // Status
    public bool PedidoFechado { get; set; }
    public bool Validado { get; set; }
    public bool Cancelado { get; set; }
    public bool Prepedido { get; set; }
    
    // Observações
    public string Obs { get; set; } = string.Empty;
    public string JustificarOAtraso { get; set; } = string.Empty;
    
    // Dados Bancários
    public string NomeDoBanco1 { get; set; } = string.Empty;
    public string AgenciaDoBanco1 { get; set; } = string.Empty;
    public string ContaCorrenteDoBanco1 { get; set; } = string.Empty;
    public string NomeDoCorrentistaDoBanco1 { get; set; } = string.Empty;
    
    // Grupo de Despesa
    public short SequenciaGrupoDespesa { get; set; }
    public short SequenciaSubGrupoDespesa { get; set; }
    
    // Itens
    public List<ProdutoPedidoCompraDto> Produtos { get; set; } = new();
    public List<DespesaPedidoCompraDto> Despesas { get; set; } = new();
    public List<ParcelaPedidoCompraDto> Parcelas { get; set; } = new();
}

#endregion

#region DTOs de Itens

/// <summary>
/// Produto do pedido de compra
/// </summary>
public class ProdutoPedidoCompraDto
{
    public int IdDoPedido { get; set; }
    public int IdDoProduto { get; set; }
    public int SequenciaDoItem { get; set; }
    
    // Dados do Produto
    public string CodigoDoProduto { get; set; } = string.Empty;
    public string DescricaoProduto { get; set; } = string.Empty;
    public string UnidadeMedida { get; set; } = string.Empty;
    
    // Valores
    public decimal Qtde { get; set; }
    public decimal VrUnitario { get; set; }
    public decimal VrTotal { get; set; }
    
    // Impostos
    public decimal AliquotaDoIpi { get; set; }
    public decimal AliquotaDoIcms { get; set; }
    public decimal VrDoIpi { get; set; }
    public decimal VrDoIcms { get; set; }
    
    // Orçamento vinculado
    public int SequenciaDoOrcamento { get; set; }
    
    // Dados de Baixa (preenchido na consulta)
    public decimal? QtdeRecebida { get; set; }
    public decimal? QtdeRestante { get; set; }
    public string? NotasBaixa { get; set; }
}

/// <summary>
/// Despesa do pedido de compra
/// </summary>
public class DespesaPedidoCompraDto
{
    public int IdDoPedido { get; set; }
    public int IdDaDespesa { get; set; }
    public int SequenciaDoItem { get; set; }
    
    // Dados da Despesa
    public string DescricaoDespesa { get; set; } = string.Empty;
    
    // Valores
    public decimal Qtde { get; set; }
    public decimal VrUnitario { get; set; }
    public decimal VrTotal { get; set; }
    
    // Dados de Baixa
    public decimal? QtdeRecebida { get; set; }
    public decimal? QtdeRestante { get; set; }
}

/// <summary>
/// Parcela do pedido de compra
/// </summary>
public class ParcelaPedidoCompraDto
{
    public int IdDoPedido { get; set; }
    public int NumeroDaParcela { get; set; }
    public DateTime DataDeVencimento { get; set; }
    public decimal ValorDaParcela { get; set; }
    public string FormaDePagamento { get; set; } = string.Empty;
}

#endregion

#region DTOs de Criação/Edição

/// <summary>
/// DTO para criar ou editar um pedido de compra
/// </summary>
public class PedidoCompraCreateDto
{
    public DateTime DataDoPedido { get; set; } = DateTime.Now;
    public string NroDaLicitacao { get; set; } = string.Empty;
    public int CodigoDaLicitacao { get; set; }
    
    public int CodigoDoFornecedor { get; set; }
    public int CodigoDaTransportadora { get; set; }
    
    public string Comprador { get; set; } = string.Empty;
    public string Vend { get; set; } = string.Empty;
    public string Prazo { get; set; } = string.Empty;
    public string CifFob { get; set; } = "CIF";
    
    public decimal VrDoFrete { get; set; }
    public decimal VrDoDesconto { get; set; }
    
    // Endereço de Entrega
    public string EnderecoDeEntrega { get; set; } = string.Empty;
    public string NumeroDoEndereco { get; set; } = string.Empty;
    public string BairroDeEntrega { get; set; } = string.Empty;
    public string CidadeDeEntrega { get; set; } = string.Empty;
    public string UfDeEntrega { get; set; } = string.Empty;
    public string CepDeEntrega { get; set; } = string.Empty;
    public string FoneDeEntrega { get; set; } = string.Empty;
    public string ContatoDeEntrega { get; set; } = string.Empty;
    
    public DateTime? PrevisaoDeEntrega { get; set; }
    public bool Prepedido { get; set; }
    
    public string Obs { get; set; } = string.Empty;
    
    // Dados Bancários
    public string NomeDoBanco1 { get; set; } = string.Empty;
    public string AgenciaDoBanco1 { get; set; } = string.Empty;
    public string ContaCorrenteDoBanco1 { get; set; } = string.Empty;
    public string NomeDoCorrentistaDoBanco1 { get; set; } = string.Empty;
    
    // Grupo de Despesa
    public short SequenciaGrupoDespesa { get; set; }
    public short SequenciaSubGrupoDespesa { get; set; }
    
    // Itens
    public List<ProdutoPedidoCompraCreateDto> Produtos { get; set; } = new();
    public List<DespesaPedidoCompraCreateDto> Despesas { get; set; } = new();
    public List<ParcelaPedidoCompraCreateDto> Parcelas { get; set; } = new();
}

public class ProdutoPedidoCompraCreateDto
{
    public int IdDoProduto { get; set; }
    public decimal Qtde { get; set; }
    public decimal VrUnitario { get; set; }
    public decimal AliquotaDoIpi { get; set; }
    public decimal AliquotaDoIcms { get; set; }
    public int SequenciaDoOrcamento { get; set; }
}

public class DespesaPedidoCompraCreateDto
{
    public int IdDaDespesa { get; set; }
    public decimal Qtde { get; set; }
    public decimal VrUnitario { get; set; }
}

public class ParcelaPedidoCompraCreateDto
{
    public DateTime DataDeVencimento { get; set; }
    public decimal ValorDaParcela { get; set; }
    public string FormaDePagamento { get; set; } = string.Empty;
}

#endregion

#region DTOs de Filtro e Busca

/// <summary>
/// Filtros para listagem de pedidos
/// </summary>
public class PedidoCompraFiltroDto
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int? CodigoDoFornecedor { get; set; }
    public string? Comprador { get; set; }
    public bool? PedidoFechado { get; set; }
    public bool? Cancelado { get; set; }
    public bool? ApenasAtrasados { get; set; }
    public string? Busca { get; set; }
    public int Pagina { get; set; } = 1;
    public int ItensPorPagina { get; set; } = 20;
    public string OrdenarPor { get; set; } = "DataDoPedido";
    public bool OrdemDescendente { get; set; } = true;
}

/// <summary>
/// Pedido de compra para seleção (dropdown/busca)
/// </summary>
public class PedidoCompraSelectDto
{
    public int IdDoPedido { get; set; }
    public DateTime DataDoPedido { get; set; }
    public string NomeFornecedor { get; set; } = string.Empty;
    public decimal TotalDoPedido { get; set; }
    public bool PedidoFechado { get; set; }
    public int QtdeItensRestantes { get; set; }
}

#endregion
