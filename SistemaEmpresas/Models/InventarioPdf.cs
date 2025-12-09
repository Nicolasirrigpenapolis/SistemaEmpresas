using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Inventario Pdf")]
public partial class InventarioPdf
{
    [Key]
    [Column("Codigo do Pdf")]
    [StringLength(10)]
    [Unicode(false)]
    public string CodigoDoPdf { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Decricao { get; set; } = null!;

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Unid { get; set; } = null!;

    [Column("Valor Contabil Pdf", TypeName = "decimal(11, 4)")]
    public decimal ValorContabilPdf { get; set; }

    [Column("Valor Total Pdf", TypeName = "decimal(12, 2)")]
    public decimal ValorTotalPdf { get; set; }

    [Column("Data Base")]
    [StringLength(100)]
    [Unicode(false)]
    public string? DataBase { get; set; }

    public int SeqItem { get; set; }

    [Column("Tipo do Produto")]
    public short TipoDoProduto { get; set; }
}
