using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
public partial class Aco
{
    [Column("Codigo da Ação")]
    public short CodigoDaAcao { get; set; }

    [Column("Descrição da Ação")]
    [StringLength(120)]
    [Unicode(false)]
    public string DescricaoDaAcao { get; set; } = null!;
}
