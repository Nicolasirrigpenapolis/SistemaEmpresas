using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Pneu
{
    [Key]
    [Column("Sequencia do Pneu")]
    public int SequenciaDoPneu { get; set; }

    [Column("Modelo do Pneu")]
    [StringLength(30)]
    [Unicode(false)]
    public string ModeloDoPneu { get; set; } = null!;

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Velocidade { get; set; }
}
