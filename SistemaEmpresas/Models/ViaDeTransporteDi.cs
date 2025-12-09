using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Via de Transporte DI")]
public partial class ViaDeTransporteDi
{
    [Column("Seq do Transporte")]
    public short SeqDoTransporte { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Transporte { get; set; } = null!;
}
