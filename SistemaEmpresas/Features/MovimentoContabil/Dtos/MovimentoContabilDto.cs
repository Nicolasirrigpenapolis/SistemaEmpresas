using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.Features.MovimentoContabil.Dtos;

/// <summary>
/// DTO para buscar informações de estoque de um produto
/// </summary>
public class EstoqueInfoDto
{
    public int SequenciaDoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? CodigoDeBarras { get; set; }
    public string? Localizacao { get; set; }
    public string SiglaUnidade { get; set; } = string.Empty;
    
    /// <summary>
    /// Estoque contábil atual (calculado da tabela Baixa do Estoque Contábil)
    /// </summary>
    public decimal EstoqueContabil { get; set; }
    
    /// <summary>
    /// Valor de custo unitário atual do produto
    /// </summary>
    public decimal ValorCusto { get; set; }
    
    /// <summary>
    /// Tipo do produto (0=Acabado, 1=M.Prima, 2=M.Revenda, 3=M.Consumo, 4=M.Imobilizado)
    /// </summary>
    public short TipoDoProduto { get; set; }
    
    public string TipoDoProdutoDescricao => TipoDoProduto switch
    {
        0 => "Acabado",
        1 => "Matéria Prima",
        2 => "Mercadoria Revenda",
        3 => "Material Consumo",
        4 => "Material Imobilizado",
        _ => "Não definido"
    };
}

/// <summary>
/// DTO para realizar ajuste de movimento contábil
/// </summary>
public class AjusteMovimentoContabilDto
{
    [Required(ErrorMessage = "Produto ou Conjunto é obrigatório")]
    public int SequenciaDoProduto { get; set; }
    
    public bool EhConjunto { get; set; }

    [Required(ErrorMessage = "Quantidade física é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Quantidade física deve ser maior ou igual a zero")]
    public decimal QuantidadeFisica { get; set; }
    
    /// <summary>
    /// Sequência do Geral (fornecedor/cliente) - opcional, pode usar empresa padrão
    /// </summary>
    public int? SequenciaDoGeral { get; set; }
    
    /// <summary>
    /// Observação do ajuste
    /// </summary>
    [StringLength(500)]
    public string? Observacao { get; set; }
    
    /// <summary>
    /// Valor de custo unitário para o ajuste (opcional, se não informado usa o do produto)
    /// </summary>
    public decimal? ValorCusto { get; set; }
}

/// <summary>
/// DTO de resposta após ajuste de movimento contábil
/// </summary>
public class AjusteMovimentoContabilResultDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    
    /// <summary>
    /// Sequência da baixa gerada
    /// </summary>
    public int? SequenciaDaBaixa { get; set; }
    
    /// <summary>
    /// Tipo do movimento gerado (0=Entrada, 1=Saída)
    /// </summary>
    public short? TipoMovimento { get; set; }
    
    public string TipoMovimentoDescricao => TipoMovimento switch
    {
        0 => "Entrada",
        1 => "Saída",
        _ => "Nenhum"
    };
    
    /// <summary>
    /// Quantidade ajustada (sempre positiva)
    /// </summary>
    public decimal QuantidadeAjustada { get; set; }
    
    /// <summary>
    /// Diferença calculada (Físico - Sistema)
    /// </summary>
    public decimal Diferenca { get; set; }
    
    /// <summary>
    /// Estoque anterior ao ajuste
    /// </summary>
    public decimal EstoqueAnterior { get; set; }
    
    /// <summary>
    /// Novo estoque após ajuste
    /// </summary>
    public decimal EstoqueNovo { get; set; }
    
    /// <summary>
    /// Documento gerado
    /// </summary>
    public string? Documento { get; set; }
}

/// <summary>
/// DTO para listar movimentos de estoque de um produto
/// </summary>
public class MovimentoEstoqueDto
{
    public int SequenciaDaBaixa { get; set; }
    public DateTime? DataMovimento { get; set; }
    public string Documento { get; set; } = string.Empty;
    public short TipoMovimento { get; set; }
    public string TipoMovimentoDescricao => TipoMovimento == 0 ? "Entrada" : "Saída";
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorCusto { get; set; }
    public decimal ValorTotal { get; set; }
    public string? Observacao { get; set; }
    public string? RazaoSocialGeral { get; set; }
    
    /// <summary>
    /// Saldo após este movimento
    /// </summary>
    public decimal SaldoAposMovimento { get; set; }
}

/// <summary>
/// Filtro para buscar movimentos de estoque
/// </summary>
public class MovimentoEstoqueFiltroDto
{
    public int SequenciaDoProduto { get; set; }
    public bool EhConjunto { get; set; }
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public short? TipoMovimento { get; set; }
    public string? Documento { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

/// <summary>
/// DTO para ajuste em lote (múltiplos produtos)
/// </summary>
public class AjusteMovimentoContabilLoteDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Informe pelo menos um produto para ajuste")]
    public List<AjusteMovimentoContabilItemDto> Itens { get; set; } = new();
    
    public string? ObservacaoGeral { get; set; }
}

public class AjusteMovimentoContabilItemDto
{
    [Required]
    public int SequenciaDoProduto { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal QuantidadeFisica { get; set; }
    
    public string? Observacao { get; set; }
}

/// <summary>
/// Resultado do ajuste em lote
/// </summary>
public class AjusteMovimentoContabilLoteResultDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public int TotalProcessados { get; set; }
    public int TotalAjustados { get; set; }
    public int TotalSemAlteracao { get; set; }
    public int TotalErros { get; set; }
    public List<AjusteMovimentoContabilResultDto> Resultados { get; set; } = new();
}
