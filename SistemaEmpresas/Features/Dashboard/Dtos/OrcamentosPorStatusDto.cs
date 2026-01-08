namespace SistemaEmpresas.Features.Dashboard.Dtos;

/// <summary>
/// Distribuição de orçamentos por status
/// </summary>
public class OrcamentosPorStatusDto
{
    public string Status { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}
