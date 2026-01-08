namespace SistemaEmpresas.Features.Fiscal.Dtos;

public class NFeImportDto
{
    public string ChaveAcesso { get; set; } = string.Empty;
    public string NumeroNota { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    
    public EmitenteDto Emitente { get; set; } = new();
    public List<NFeItemDto> Itens { get; set; } = new();
    
    public decimal ValorTotal { get; set; }
    public decimal ValorProdutos { get; set; }
    public decimal ValorIcms { get; set; }
    public decimal ValorIpi { get; set; }
}

public class EmitenteDto
{
    public string Cnpj { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string InscricaoEstadual { get; set; } = string.Empty;
}

public class NFeItemDto
{
    public string CodigoProdutoFornecedor { get; set; } = string.Empty;
    public string DescricaoProdutoFornecedor { get; set; } = string.Empty;
    public string Ncm { get; set; } = string.Empty;
    public string Cfop { get; set; } = string.Empty;
    public string UnidadeMedida { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    
    // Campos para o "De/Para" no Frontend
    public int? ProdutoIdSistema { get; set; }
    public string? NomeProdutoSistema { get; set; }
    
    // Impostos do Item
    public decimal ValorIcms { get; set; }
    public decimal ValorIpi { get; set; }
    public decimal AliquotaIcms { get; set; }
}
