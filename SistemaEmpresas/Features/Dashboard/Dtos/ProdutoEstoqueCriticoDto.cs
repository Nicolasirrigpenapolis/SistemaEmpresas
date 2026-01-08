namespace SistemaEmpresas.Features.Dashboard.Dtos;

/// <summary>
/// Produto com estoque crítico (abaixo do mínimo)
/// </summary>
public class ProdutoEstoqueCriticoDto
{
    public int SequenciaDoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal QuantidadeNoEstoque { get; set; }
    public decimal QuantidadeMinima { get; set; }
    public decimal Diferenca { get; set; }
    public string? Localizacao { get; set; }
}
