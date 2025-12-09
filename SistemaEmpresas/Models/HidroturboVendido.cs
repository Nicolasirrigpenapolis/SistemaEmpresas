using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Hidroturbos Vendidos")]
public partial class HidroturboVendido
{
    [Key]
    [Column("Seq do Hidroturbo")]
    public int SeqDoHidroturbo { get; set; }

    [Column("Modelo do Hidroturbo")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloDoHidroturbo { get; set; } = null!;

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [StringLength(40)]
    [Unicode(false)]
    public string Cidade { get; set; } = null!;

    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;
}
