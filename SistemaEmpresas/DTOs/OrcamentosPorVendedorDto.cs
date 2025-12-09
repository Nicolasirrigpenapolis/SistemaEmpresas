namespace SistemaEmpresas.DTOs;

/// <summary>
/// Orçamentos agrupados por vendedor
/// </summary>
public class OrcamentosPorVendedorDto
{
    public int SequenciaDoVendedor { get; set; }
    public string NomeVendedor { get; set; } = string.Empty;
    public int TotalOrcamentos { get; set; }
    public int OrcamentosFechados { get; set; }
    public decimal TaxaConversao { get; set; }
}
