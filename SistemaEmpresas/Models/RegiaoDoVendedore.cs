using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Região dos Vendedores")]
public partial class RegiaoDoVendedore
{
    [Key]
    [Column("Seq do Vendedor")]
    public int SeqDoVendedor { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;
}
