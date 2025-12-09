using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaNotaFiscal", "SequenciaConjuntoNotaFiscal")]
[Table("Conjuntos da Nota Fiscal")]
public partial class ConjuntoDaNotaFiscal
{
    [Key]
    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Key]
    [Column("Seqüência Conjunto Nota Fiscal")]
    public int SequenciaConjuntoNotaFiscal { get; set; }

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(12, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Valor do IPI", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIpi { get; set; }

    [Column("Valor do ICMS", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIcms { get; set; }

    [Column("Alíquota do IPI", TypeName = "decimal(8, 4)")]
    public decimal AliquotaDoIpi { get; set; }

    [Column("Alíquota do ICMS", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIcms { get; set; }

    [Column("Percentual da Redução", TypeName = "decimal(6, 2)")]
    public decimal PercentualDaReducao { get; set; }

    public bool Diferido { get; set; }

    [Column("Valor da Base de Cálculo", TypeName = "decimal(11, 2)")]
    public decimal ValorDaBaseDeCalculo { get; set; }

    [Column("Valor do PIS", TypeName = "decimal(11, 4)")]
    public decimal ValorDoPis { get; set; }

    [Column("Valor do Cofins", TypeName = "decimal(11, 4)")]
    public decimal ValorDoCofins { get; set; }

    [Column("IVA", TypeName = "decimal(8, 4)")]
    public decimal Iva { get; set; }

    [Column("Base de Cálculo ST", TypeName = "decimal(11, 2)")]
    public decimal BaseDeCalculoSt { get; set; }

    [Column("Valor ICMS ST", TypeName = "decimal(11, 2)")]
    public decimal ValorIcmsSt { get; set; }

    [Column("CFOP")]
    public short Cfop { get; set; }

    [Column("CST")]
    public short Cst { get; set; }

    [Column("Alíquota do ICMS ST", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIcmsSt { get; set; }

    [Column("Valor do Tributo", TypeName = "decimal(11, 2)")]
    public decimal ValorDoTributo { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Bc pis", TypeName = "decimal(9, 2)")]
    public decimal BcPis { get; set; }

    [Column("Aliq do pis", TypeName = "decimal(5, 2)")]
    public decimal AliqDoPis { get; set; }

    [Column("Bc cofins", TypeName = "decimal(9, 2)")]
    public decimal BcCofins { get; set; }

    [Column("Aliq do cofins", TypeName = "decimal(5, 2)")]
    public decimal AliqDoCofins { get; set; }

    [Column("Valor IBS", TypeName = "decimal(18, 2)")]
    public decimal ValorIbs { get; set; }

    [Column("Valor CBS", TypeName = "decimal(18, 2)")]
    public decimal ValorCbs { get; set; }

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("ConjuntosDaNotaFiscals")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoConjunto")]
    [InverseProperty("ConjuntosDaNotaFiscals")]
    public virtual Conjunto SequenciaDoConjuntoNavigation { get; set; } = null!;
}
