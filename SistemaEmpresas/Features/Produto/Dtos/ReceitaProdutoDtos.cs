namespace SistemaEmpresas.Features.Produto.Dtos;

/// <summary>
/// DTO para listagem de itens da receita do produto (materias primas)
/// Representa um item da composicao/BOM do produto
/// </summary>
public class ReceitaProdutoListDto
{
    /// <summary>
    /// Sequencia do produto principal (produto que tem a receita)
    /// </summary>
    public int SequenciaDoProduto { get; set; }

    /// <summary>
    /// Sequencia da materia prima (produto componente)
    /// </summary>
    public int SequenciaDaMateriaPrima { get; set; }

    /// <summary>
    /// Descricao da materia prima
    /// </summary>
    public string DescricaoDaMateriaPrima { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade da materia prima necessaria
    /// </summary>
    public decimal Quantidade { get; set; }

    /// <summary>
    /// Unidade de medida da materia prima
    /// </summary>
    public string Unidade { get; set; } = string.Empty;

    /// <summary>
    /// Peso unitario da materia prima
    /// </summary>
    public decimal Peso { get; set; }

    /// <summary>
    /// Valor de custo unitario da materia prima
    /// </summary>
    public decimal ValorDeCusto { get; set; }

    /// <summary>
    /// Custo total (Quantidade * ValorDeCusto)
    /// </summary>
    public decimal CustoTotal { get; set; }

    /// <summary>
    /// Peso total (Quantidade * Peso)
    /// </summary>
    public decimal PesoTotal { get; set; }
}

/// <summary>
/// DTO para criacao/atualizacao de item da receita
/// </summary>
public class ReceitaProdutoCreateUpdateDto
{
    /// <summary>
    /// Sequencia da materia prima (produto componente)
    /// </summary>
    public int SequenciaDaMateriaPrima { get; set; }

    /// <summary>
    /// Quantidade da materia prima necessaria
    /// </summary>
    public decimal Quantidade { get; set; }
}

/// <summary>
/// DTO para retorno completo da receita do produto
/// </summary>
public class ReceitaProdutoDto
{
    /// <summary>
    /// Sequencia do produto principal
    /// </summary>
    public int SequenciaDoProduto { get; set; }

    /// <summary>
    /// Descricao do produto principal
    /// </summary>
    public string DescricaoDoProduto { get; set; } = string.Empty;

    /// <summary>
    /// Lista de itens da receita (materias primas)
    /// </summary>
    public List<ReceitaProdutoListDto> Itens { get; set; } = new();

    /// <summary>
    /// Total de itens na receita
    /// </summary>
    public int TotalItens => Itens.Count;

    /// <summary>
    /// Custo total da receita (soma de todos os custos)
    /// </summary>
    public decimal CustoTotalReceita => Itens.Sum(i => i.CustoTotal);

    /// <summary>
    /// Peso total da receita (soma de todos os pesos)
    /// </summary>
    public decimal PesoTotalReceita => Itens.Sum(i => i.PesoTotal);
}
