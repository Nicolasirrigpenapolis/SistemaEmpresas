using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoMovimento", "SequenciaDaDespesaMovimento")]
[Table("Despesas do Movimento Contábil")]
public partial class DespesaDoMovimentoContabil
{
    [Key]
    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Key]
    [Column("Seqüência da Despesa Movimento")]
    public int SequenciaDaDespesaMovimento { get; set; }

    [Column("Seqüência da Despesa")]
    public int SequenciaDaDespesa { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(11, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor de Custo", TypeName = "decimal(12, 4)")]
    public decimal ValorDeCusto { get; set; }

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
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

    [ForeignKey("SequenciaDaDespesa")]
    [InverseProperty("DespesasDoMovimentoContabils")]
    public virtual Despesa SequenciaDaDespesaNavigation { get; set; } = null!;
}
