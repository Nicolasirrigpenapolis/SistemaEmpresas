using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Comissão do montador")]
public partial class ComissaoDoMontador
{
    [Key]
    [Column("Sequencia da comissão")]
    public int SequenciaDaComissao { get; set; }

    [Column("Cod do Vendedor")]
    public int CodDoVendedor { get; set; }

    public int Manutencao { get; set; }

    [Column("NFe")]
    [StringLength(10)]
    [Unicode(false)]
    public string Nfe { get; set; } = null!;

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Percentual { get; set; }

    [Column("Pagto Vendedor", TypeName = "datetime")]
    public DateTime? PagtoVendedor { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Comissao { get; set; }

    public bool Imprimir { get; set; }
}
