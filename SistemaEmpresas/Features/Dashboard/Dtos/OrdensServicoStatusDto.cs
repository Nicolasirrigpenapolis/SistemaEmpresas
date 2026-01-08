namespace SistemaEmpresas.Features.Dashboard.Dtos;

/// <summary>
/// Ordens de serviço por status
/// </summary>
public class OrdensServicoStatusDto
{
    public string Status { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}
