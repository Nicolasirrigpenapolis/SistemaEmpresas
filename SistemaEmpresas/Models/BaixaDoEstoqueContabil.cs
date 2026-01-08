using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Baixa do Estoque Contábil")]
public partial class BaixaDoEstoqueContabil
{
    [Key]
    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }

    [Column("Tipo do Movimento")]
    public short TipoDoMovimento { get; set; }

    [Column("Data do Movimento", TypeName = "datetime")]
    public DateTime? DataDoMovimento { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(11, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor de Custo", TypeName = "decimal(12, 4)")]
    public decimal ValorDeCusto { get; set; }

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
    public decimal ValorTotal { get; set; }

    [Column("Observação", TypeName = "text")]
    public string Observacao { get; set; } = null!;

    [Column("Valor do PIS", TypeName = "decimal(11, 4)")]
    public decimal ValorDoPis { get; set; }

    [Column("Valor do Cofins", TypeName = "decimal(11, 4)")]
    public decimal ValorDoCofins { get; set; }

    [Column("Valor do ICMS", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIcms { get; set; }

    [Column("Valor do IPI", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIpi { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor da Substituição", TypeName = "decimal(12, 4)")]
    public decimal ValorDaSubstituicao { get; set; }

    [Column("Tipo do Produto")]
    public short TipoDoProduto { get; set; }

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Estoque { get; set; } = null!;

    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Column("Seqüência do Item 2")]
    public int SequenciaDoItem2 { get; set; }

    [Column("Seqüência da Despesa")]
    public int SequenciaDaDespesa { get; set; }

    [ForeignKey("SequenciaDaDespesa")]
    [InverseProperty("BaixaDoEstoqueContabils")]
    public virtual Despesa SequenciaDaDespesaNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoConjunto")]
    [InverseProperty("BaixaDoEstoqueContabils")]
    public virtual Conjunto SequenciaDoConjuntoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("BaixaDoEstoqueContabils")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("BaixaDoEstoqueContabils")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
