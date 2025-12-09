using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaNotaFiscal", "NumeroDaParcela")]
[Table("Parcelas Nota Fiscal")]
public partial class ParcelaNotaFiscal
{
    [Key]
    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Key]
    [Column("Número da Parcela")]
    public short NumeroDaParcela { get; set; }

    public short Dias { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime DataDeVencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("ParcelasNotaFiscals")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;
}
