using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Advogado
{
    [Key]
    [Column("Codigo do Advogado")]
    public short CodigoDoAdvogado { get; set; }

    [Column("Nome do Advogado")]
    [StringLength(40)]
    [Unicode(false)]
    public string NomeDoAdvogado { get; set; } = null!;

    [StringLength(14)]
    [Unicode(false)]
    public string Celular { get; set; } = null!;

    [InverseProperty("CodigoDoAdvogadoNavigation")]
    public virtual ICollection<ControleDeProcesso> ControleDeProcessos { get; set; } = new List<ControleDeProcesso>();
}
