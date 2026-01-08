using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.DTOs.MovimentoContabil;

public class MovimentoContabilNovoDto
{
    public int SequenciaDoMovimento { get; set; }
    
    [Required(ErrorMessage = "Data do movimento é obrigatória")]
    public DateTime DataDoMovimento { get; set; }
    
    [Required(ErrorMessage = "Tipo do movimento é obrigatório")]
    public short TipoDoMovimento { get; set; } // 0 = Entrada, 1 = Saída
    
    [Required(ErrorMessage = "Documento é obrigatório")]
    [StringLength(20)]
    public string Documento { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Geral (Fornecedor/Cliente) é obrigatório")]
    public int SequenciaDoGeral { get; set; }
    public string? RazaoSocialGeral { get; set; }
    
    public string? Observacao { get; set; }
    public bool Devolucao { get; set; }
    
    public List<ProdutoMvtoContabilItemDto> Produtos { get; set; } = new();
    public List<ConjuntoMvtoContabilItemDto> Conjuntos { get; set; } = new();
    public List<DespesaMvtoContabilItemDto> Despesas { get; set; } = new();
    public List<ParcelaMvtoContabilDto> Parcelas { get; set; } = new();
    
    public decimal ValorTotalDosProdutos { get; set; }
    public decimal ValorTotalDoMovimento { get; set; }
}

public class ProdutoMvtoContabilItemDto
{
    public int SequenciaDoProdutoMvtoNovo { get; set; }
    
    [Required]
    public int SequenciaDoProduto { get; set; }
    public string? DescricaoProduto { get; set; }
    
    [Required]
    public decimal Quantidade { get; set; }
    
    [Required]
    public decimal ValorUnitario { get; set; }
    
    public decimal ValorDeCusto { get; set; }
    public decimal ValorTotal { get; set; }
    
    // Campos adicionais do legado
    public decimal ValorDoPis { get; set; }
    public decimal ValorDoCofins { get; set; }
    public decimal ValorDoIpi { get; set; }
    public decimal ValorDoIcms { get; set; }
    public decimal ValorDoFrete { get; set; }
    public decimal ValorDaSubstituicao { get; set; }
}

public class ConjuntoMvtoContabilItemDto
{
    public int SequenciaConjuntoMvtoNovo { get; set; }
    
    [Required]
    public int SequenciaDoConjunto { get; set; }
    public string? DescricaoConjunto { get; set; }
    
    [Required]
    public decimal Quantidade { get; set; }
    
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

public class DespesaMvtoContabilItemDto
{
    public int SequenciaDespesaMvtoNovo { get; set; }
    
    [Required]
    public int SequenciaDaDespesa { get; set; }
    public string? DescricaoDespesa { get; set; }
    
    [Required]
    public decimal Quantidade { get; set; }
    
    [Required]
    public decimal ValorUnitario { get; set; }
    
    public decimal ValorDeCusto { get; set; }
    public decimal ValorTotal { get; set; }
}

public class ParcelaMvtoContabilDto
{
    public int NumeroDaParcela { get; set; }
    public DateTime DataDeVencimento { get; set; }
    public decimal ValorDaParcela { get; set; }
}

public class MovimentoContabilFiltroDto
{
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public int? SequenciaDoGeral { get; set; }
    public string? Documento { get; set; }
    public short? TipoDoMovimento { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

// DTOs para Produção Inteligente

/// <summary>
/// Representa um componente necessário para produção
/// </summary>
public class ComponenteProducaoDto
{
    public int SequenciaDoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal QuantidadeNecessaria { get; set; }
    public decimal EstoqueDisponivel { get; set; }
    public decimal Falta { get; set; }
    public bool PodeSerProduzido { get; set; }
    public bool Industrializacao { get; set; }
    public List<ComponenteProducaoDto>? SubComponentes { get; set; }
}

/// <summary>
/// Resultado da verificação de viabilidade de produção
/// </summary>
public class VerificacaoProducaoResultDto
{
    public bool PodeProduzir { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public int SequenciaDoProdutoOuConjunto { get; set; }
    public string DescricaoProduto { get; set; } = string.Empty;
    public decimal QuantidadeSolicitada { get; set; }
    public bool EhConjunto { get; set; }
    
    /// <summary>
    /// Lista de componentes com falta de estoque
    /// </summary>
    public List<ComponenteProducaoDto> ComponentesFaltantes { get; set; } = new();
    
    /// <summary>
    /// Plano de produção em cascata sugerido (ordem de produção)
    /// </summary>
    public List<ItemPlanoProducaoDto> PlanoProducaoCascata { get; set; } = new();
}

/// <summary>
/// Item do plano de produção em cascata
/// </summary>
public class ItemPlanoProducaoDto
{
    public int Ordem { get; set; }
    public int SequenciaDoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal QuantidadeAProduzir { get; set; }
    public bool EhConjunto { get; set; }
    public int? DependeDe { get; set; } // Sequência do produto que depende
}

/// <summary>
/// Requisição para produção em cascata
/// </summary>
public class ProducaoCascataRequestDto
{
    public int SequenciaDoGeral { get; set; }
    public string Documento { get; set; } = string.Empty;
    public DateTime DataDoMovimento { get; set; } = DateTime.Today;
    public string? Observacao { get; set; }
    
    /// <summary>
    /// Item final desejado (pode ser produto ou conjunto)
    /// </summary>
    public int SequenciaDoProdutoOuConjunto { get; set; }
    public bool EhConjunto { get; set; }
    public decimal Quantidade { get; set; }
    
    /// <summary>
    /// Se true, produz automaticamente os itens intermediários necessários
    /// </summary>
    public bool ProduzirIntermediarios { get; set; } = true;
}

/// <summary>
/// Resultado da produção em cascata
/// </summary>
public class ProducaoCascataResultDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public List<MovimentoGeradoDto> MovimentosGerados { get; set; } = new();
    public int TotalMovimentos { get; set; }
}

/// <summary>
/// Movimento gerado na produção em cascata
/// </summary>
public class MovimentoGeradoDto
{
    public int SequenciaDoMovimento { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string DescricaoProduto { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
}
