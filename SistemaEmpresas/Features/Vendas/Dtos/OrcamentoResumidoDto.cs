namespace SistemaEmpresas.Features.Vendas.Dtos;

/// <summary>
/// Orçamento resumido para listas do dashboard
/// </summary>
public class OrcamentoResumidoDto
{
    public int SequenciaDoOrcamento { get; set; }
    public DateTime? DataDeEmissao { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string? NomeVendedor { get; set; }
    public bool VendaFechada { get; set; }
    public bool Cancelado { get; set; }
    public int DiasAberto { get; set; }
}
