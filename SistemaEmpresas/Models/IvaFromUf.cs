using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdMva", "Uf", "Ncm")]
[Table("IVA From UFs")]
public partial class IvaFromUf
{
    [Key]
    [Column("ID MVA")]
    public int IdMva { get; set; }

    [Key]
    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    [Key]
    [Column("NCM")]
    public int Ncm { get; set; }

    [Column("IVA", TypeName = "decimal(8, 4)")]
    public decimal Iva { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Teste { get; set; } = null!;
}
