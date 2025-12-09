using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoNotas", "SequenciaDaNotaFiscal")]
[Table("Notas Autorizadas")]
public partial class NotaAutorizada
{
    [Key]
    [Column("Seqüência do Notas")]
    public int SequenciaDoNotas { get; set; }

    [Key]
    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("XML", TypeName = "text")]
    public string Xml { get; set; } = null!;

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("NotasAutorizada")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;
}
