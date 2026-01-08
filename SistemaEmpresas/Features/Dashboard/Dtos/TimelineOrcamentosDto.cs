namespace SistemaEmpresas.Features.Dashboard.Dtos;

/// <summary>
/// Timeline de orçamentos/pedidos por período
/// </summary>
public class TimelineOrcamentosDto
{
    public DateTime Periodo { get; set; } // Data (pode ser dia, semana ou mês)
    public int QuantidadeOrcamentos { get; set; }
    public int QuantidadePedidos { get; set; }
}
