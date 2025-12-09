using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Adutora
{
    [Key]
    [Column("Sequencia da Adutora")]
    public int SequenciaDaAdutora { get; set; }

    [Column("Modelo da Adutora")]
    [StringLength(30)]
    [Unicode(false)]
    public string ModeloDaAdutora { get; set; } = null!;

    [Column("DN", TypeName = "decimal(8, 2)")]
    public decimal Dn { get; set; }

    [Column("DN mm", TypeName = "decimal(8, 2)")]
    public decimal DnMm { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Coeficiente { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string Material { get; set; } = null!;

    [Column("E mm", TypeName = "decimal(8, 2)")]
    public decimal EMm { get; set; }

    [Column("DI mm", TypeName = "decimal(8, 2)")]
    public decimal DiMm { get; set; }
}
