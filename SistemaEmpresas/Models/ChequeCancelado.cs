using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Cheques Cancelados")]
public partial class ChequeCancelado
{
    [Key]
    public int Sequencia { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Data { get; set; }

    public short Banco { get; set; }

    [Column("Nro da Conta")]
    [StringLength(10)]
    [Unicode(false)]
    public string NroDaConta { get; set; } = null!;

    [Column("Nro do Cheque")]
    [StringLength(10)]
    [Unicode(false)]
    public string NroDoCheque { get; set; } = null!;

    [Column("Motivo do Cancelamento", TypeName = "text")]
    public string MotivoDoCancelamento { get; set; } = null!;
}
