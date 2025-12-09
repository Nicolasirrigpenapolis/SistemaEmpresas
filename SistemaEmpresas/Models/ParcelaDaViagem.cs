using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SeqDaViagem", "NumeroDaParcela")]
[Table("Parcelas da Viagem")]
public partial class ParcelaDaViagem
{
    [Key]
    [Column("Seq da Viagem")]
    public int SeqDaViagem { get; set; }

    [Key]
    [Column("Número da Parcela")]
    public short NumeroDaParcela { get; set; }

    public short Dias { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime? DataDeVencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }
}
