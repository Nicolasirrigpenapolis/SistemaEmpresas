namespace SistemaEmpresas.Features.Dashboard.Dtos;

/// <summary>
/// KPIs principais do dashboard
/// </summary>
public class DashboardKpiDto
{
    /// <summary>
    /// Total de orçamentos abertos (não cancelados, não fechados)
    /// </summary>
    public int OrcamentosAbertos { get; set; }

    /// <summary>
    /// Pedidos de compra pendentes (não fechados, não cancelados)
    /// </summary>
    public int ComprasPendentesValidacao { get; set; }

    /// <summary>
    /// Total de produtos ativos cadastrados
    /// </summary>
    public int TotalProdutos { get; set; }

    /// <summary>
    /// Total de conjuntos ativos cadastrados
    /// </summary>
    public int TotalConjuntos { get; set; }

    /// <summary>
    /// Total de itens com estoque abaixo do mínimo
    /// </summary>
    public int ProdutosEstoqueCritico { get; set; }
}
