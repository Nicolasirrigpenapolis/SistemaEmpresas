using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Setore
{
    [Key]
    [Column("Codigo do setor")]
    public short CodigoDoSetor { get; set; }

    [Column("Nome do setor")]
    [StringLength(25)]
    [Unicode(false)]
    public string NomeDoSetor { get; set; } = null!;

    [InverseProperty("CodigoDoSetorNavigation")]
    public virtual ICollection<LinhaDeProducao> LinhaDeProducaos { get; set; } = new List<LinhaDeProducao>();
}
