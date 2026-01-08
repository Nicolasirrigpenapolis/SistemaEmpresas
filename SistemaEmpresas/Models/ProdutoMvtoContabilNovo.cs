using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoMovimento", "SequenciaDoProdutoMvtoNovo")]
[Table("Produtos Mvto Contábil Novo")]
public partial class ProdutoMvtoContabilNovo
{
    [Key]
    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Key]
    [Column("Seqüência do Produto Mvto Novo")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SequenciaDoProdutoMvtoNovo { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(12, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor de Custo", TypeName = "decimal(12, 4)")]
    public decimal ValorDeCusto { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Valor do PIS", TypeName = "decimal(11, 4)")]
    public decimal ValorDoPis { get; set; }

    [Column("Valor do Cofins", TypeName = "decimal(11, 4)")]
    public decimal ValorDoCofins { get; set; }

    [Column("Valor do IPI", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIpi { get; set; }

    [Column("Valor do ICMS", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIcms { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor da Substituição", TypeName = "decimal(12, 4)")]
    public decimal ValorDaSubstituicao { get; set; }

    [Column("Valor do ipi compra", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIpiCompra { get; set; }

    [Column("Valor total compra", TypeName = "decimal(12, 4)")]
    public decimal ValorTotalCompra { get; set; }

    [Column("Sequencia Unidade Speed")]
    public int SequenciaUnidadeSpeed { get; set; }

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("ProdutosMvtoContabilNovos")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
