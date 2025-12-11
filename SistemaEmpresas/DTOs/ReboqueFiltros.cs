namespace SistemaEmpresas.DTOs;

public class ReboqueFiltros
{
    public string? Busca { get; set; }
    public string? Placa { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
}
