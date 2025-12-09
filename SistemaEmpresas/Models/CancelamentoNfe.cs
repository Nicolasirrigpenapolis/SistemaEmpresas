using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaCancelamentoNfe", "SequenciaDaNotaFiscal")]
[Table("Cancelamento NFe")]
public partial class CancelamentoNfe
{
    [Key]
    [Column("Seqüência Cancelamento NFe")]
    public int SequenciaCancelamentoNfe { get; set; }

    [Key]
    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Justificativa { get; set; } = null!;

    public short Ambiente { get; set; }

    [Column("Data do Cancelamento", TypeName = "datetime")]
    public DateTime? DataDoCancelamento { get; set; }

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("CancelamentoNfe")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;
}
