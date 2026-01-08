namespace SistemaEmpresas.Features.Vendas.Dtos;

/// <summary>
/// Pedido resumido para listas do dashboard
/// </summary>
public class PedidoResumidoDto
{
    public int SequenciaDoPedido { get; set; }
    public DateTime? DataDeEmissao { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string? NomeVendedor { get; set; }
    public bool PedidoCancelado { get; set; }
    public int? SequenciaDoOrcamento { get; set; }
}
