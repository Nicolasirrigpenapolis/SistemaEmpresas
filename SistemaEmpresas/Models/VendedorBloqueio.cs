using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Vendedores Bloqueio")]
public partial class VendedorBloqueio
{
    [Key]
    [Column("Codigo do Vendedor")]
    public int CodigoDoVendedor { get; set; }

    [Column("Nome do Vendedor")]
    [StringLength(30)]
    [Unicode(false)]
    public string NomeDoVendedor { get; set; } = null!;

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Percentual { get; set; }

    [Column("Codigo ipg")]
    public int CodigoIpg { get; set; }
}
