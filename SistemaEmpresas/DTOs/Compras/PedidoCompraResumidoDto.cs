namespace SistemaEmpresas.DTOs;

/// <summary>
/// Pedido de compra com informações de atraso
/// </summary>
public class PedidoCompraResumidoDto
{
    public int IdDoPedido { get; set; }
    public DateTime DataDoPedido { get; set; }
    public string NomeFornecedor { get; set; } = string.Empty;
    public DateTime? PrevisaoDeEntrega { get; set; }
    public bool PedidoFechado { get; set; }
    public bool Validado { get; set; }
    public bool Cancelado { get; set; }
    public int DiasAtraso { get; set; }
}
