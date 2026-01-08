namespace SistemaEmpresas.Features.Transporte.Dtos;

public class ViagemFiltros
{
    public string? Busca { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
}
