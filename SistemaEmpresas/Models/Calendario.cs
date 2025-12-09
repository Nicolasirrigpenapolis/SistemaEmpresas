using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Calendario")]
public partial class Calendario
{
    [Key]
    [Column("Seq do Calendario")]
    public int SeqDoCalendario { get; set; }

    [Column("Dta do Feriado", TypeName = "datetime")]
    public DateTime DtaDoFeriado { get; set; }

    [Column("Dia da Semana")]
    [StringLength(3)]
    [Unicode(false)]
    public string DiaDaSemana { get; set; } = null!;
}
