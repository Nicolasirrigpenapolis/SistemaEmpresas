using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Comissao")]
public partial class Comissao
{
    [Key]
    [Column("Seqüência da Comissão")]
    public int SequenciaDaComissao { get; set; }

    [Column("Percentual de Comissão", TypeName = "decimal(6, 2)")]
    public decimal PercentualDeComissao { get; set; }

    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("Percentual 2", TypeName = "decimal(6, 2)")]
    public decimal Percentual2 { get; set; }

    public int Intermediario { get; set; }

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("Comissao")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;
}
