using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Status do Processo")]
public partial class StatuDoProcesso
{
    [Key]
    [Column("Codigo do Status")]
    public short CodigoDoStatus { get; set; }

    [Column("Descrição do Status")]
    [StringLength(20)]
    [Unicode(false)]
    public string DescricaoDoStatus { get; set; } = null!;

    [InverseProperty("CodigoDoStatusNavigation")]
    public virtual ICollection<ControleDeProcesso> ControleDeProcessos { get; set; } = new List<ControleDeProcesso>();
}
