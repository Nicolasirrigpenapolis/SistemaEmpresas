using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Paise
{
    [Key]
    [Column("Seqüência do País")]
    public int SequenciaDoPais { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Código do País")]
    public short CodigoDoPais { get; set; }

    public bool Inativo { get; set; }

    [InverseProperty("SequenciaDoPaisNavigation")]
    public virtual ICollection<Geral> Gerals { get; set; } = new List<Geral>();

    [InverseProperty("SequenciaDoPaisNavigation")]
    public virtual ICollection<Orcamento> Orcamentos { get; set; } = new List<Orcamento>();
}
