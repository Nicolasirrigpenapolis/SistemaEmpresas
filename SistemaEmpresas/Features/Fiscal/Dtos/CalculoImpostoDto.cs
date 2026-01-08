namespace SistemaEmpresas.Features.Fiscal.Dtos;

/// <summary>
/// Request para cálculo de impostos de um item
/// </summary>
public class CalculoImpostoRequestDto
{
    /// <summary>
    /// Tipo do item: 1=Produto, 2=Conjunto, 3=Peça
    /// </summary>
    public int TipoItem { get; set; } = 1;
    
    /// <summary>
    /// Sequência do item (SequenciaDoProduto, SequenciaDoConjunto ou SequenciaDoProduto para peça)
    /// </summary>
    public int SequenciaDoItem { get; set; }
    
    /// <summary>
    /// Quantidade do item
    /// </summary>
    public decimal Quantidade { get; set; } = 1;
    
    /// <summary>
    /// Valor unitário
    /// </summary>
    public decimal ValorUnitario { get; set; }
    
    /// <summary>
    /// Valor do desconto
    /// </summary>
    public decimal Desconto { get; set; }
    
    /// <summary>
    /// Valor do frete rateado para este item
    /// </summary>
    public decimal ValorFrete { get; set; }
}

/// <summary>
/// Resultado do cálculo de impostos
/// </summary>
public class CalculoImpostoResultDto
{
    // Valores base
    public decimal ValorTotal { get; set; }
    
    // CFOP e CST
    public string CFOP { get; set; } = string.Empty;
    public string CST { get; set; } = string.Empty;
    
    // ICMS
    public decimal BaseCalculoICMS { get; set; }
    public decimal AliquotaICMS { get; set; }
    public decimal ValorICMS { get; set; }
    public decimal PercentualReducao { get; set; }
    public bool Diferido { get; set; }
    
    // IPI
    public decimal AliquotaIPI { get; set; }
    public decimal ValorIPI { get; set; }
    
    // PIS
    public decimal BaseCalculoPIS { get; set; }
    public decimal AliquotaPIS { get; set; }
    public decimal ValorPIS { get; set; }
    
    // COFINS
    public decimal BaseCalculoCOFINS { get; set; }
    public decimal AliquotaCOFINS { get; set; }
    public decimal ValorCOFINS { get; set; }
    
    // Substituição Tributária
    public decimal IVA { get; set; }
    public decimal BaseCalculoST { get; set; }
    public decimal AliquotaICMSST { get; set; }
    public decimal ValorICMSST { get; set; }
    
    // IBS/CBS (Reforma Tributária)
    public decimal ValorIBS { get; set; }
    public decimal ValorCBS { get; set; }
    
    // Total de tributos
    public decimal ValorTributo { get; set; }
    
    // Informações adicionais do item
    public string NCM { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public string DescricaoItem { get; set; } = string.Empty;
}
