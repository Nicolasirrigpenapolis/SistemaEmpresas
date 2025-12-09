using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaCorrecao", "SequenciaDaNotaFiscal")]
[Table("Carta de Correção NFe")]
public partial class CartaDeCorrecaoNfe
{
    [Key]
    [Column("Seqüência da Correção")]
    public int SequenciaDaCorrecao { get; set; }

    [Key]
    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("Número da Correção")]
    public short NumeroDaCorrecao { get; set; }

    [Column("Justificativa CCe", TypeName = "text")]
    public string JustificativaCce { get; set; } = null!;

    [Column("Data Correção", TypeName = "datetime")]
    public DateTime? DataCorrecao { get; set; }

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("CartaDeCorrecaoNves")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;
}
