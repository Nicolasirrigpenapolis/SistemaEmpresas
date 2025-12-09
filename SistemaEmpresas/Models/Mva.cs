using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdMva", "Uf")]
[Table("MVA")]
public partial class Mva
{
    [Key]
    [Column("ID MVA")]
    public int IdMva { get; set; }

    [Key]
    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    [Column("IVA", TypeName = "decimal(8, 4)")]
    public decimal Iva { get; set; }
}
