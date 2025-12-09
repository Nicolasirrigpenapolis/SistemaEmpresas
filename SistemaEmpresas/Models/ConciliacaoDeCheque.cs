using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Conciliação de Cheques")]
public partial class ConciliacaoDeCheque
{
    [Key]
    [Column("Seq da Conciliação")]
    public int SeqDaConciliacao { get; set; }

    [Column("Dta da Conciliação", TypeName = "datetime")]
    public DateTime DtaDaConciliacao { get; set; }

    public short Agencia { get; set; }

    [Column("N Cheque")]
    public long NCheque { get; set; }

    [Column("Dta de Emissão", TypeName = "datetime")]
    public DateTime DtaDeEmissao { get; set; }

    [Column("Vr do Cheque", TypeName = "decimal(10, 2)")]
    public decimal VrDoCheque { get; set; }

    [Column("Vr Compensado", TypeName = "decimal(10, 2)")]
    public decimal VrCompensado { get; set; }

    public bool Conciliado { get; set; }
}
