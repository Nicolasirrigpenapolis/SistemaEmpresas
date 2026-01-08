namespace SistemaEmpresas.Features.Dashboard.Dtos;

/// <summary>
/// Alerta operacional do dashboard
/// </summary>
public class AlertaOperacionalDto
{
    public string Tipo { get; set; } = string.Empty; // "Atraso", "EstoqueCritico", "ComprasPendentes", etc
    public string Mensagem { get; set; } = string.Empty;
    public string? Referencia { get; set; } // ID ou código relacionado
    public int Quantidade { get; set; }
    public DateTime? DataReferencia { get; set; }
}
