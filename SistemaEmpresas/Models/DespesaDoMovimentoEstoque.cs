using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoMovimento", "SequenciaDaDespesaMovimento")]
[Table("Despesas do Movimento Estoque")]
public partial class DespesaDoMovimentoEstoque
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

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
    public decimal ValorTotal { get; set; }

    [Column("Porcentagem de IPI", TypeName = "decimal(8, 4)")]
    public decimal PorcentagemDeIpi { get; set; }

    [Column("Valor ICMS ST", TypeName = "decimal(11, 2)")]
    public decimal ValorIcmsSt { get; set; }

    [Column("Local Usado")]
    [StringLength(50)]
    [Unicode(false)]
    public string LocalUsado { get; set; } = null!;

    [ForeignKey("SequenciaDaDespesa")]
    [InverseProperty("DespesasDoMovimentoEstoques")]
    public virtual Despesa SequenciaDaDespesaNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoMovimento")]
    [InverseProperty("DespesasDoMovimentoEstoques")]
    public virtual MovimentoDoEstoque SequenciaDoMovimentoNavigation { get; set; } = null!;
}
