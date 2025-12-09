using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Baixa Comissão Lote")]
public partial class BaixaComissaoLote
{
    [Key]
    [Column("Seq da Bx")]
    public int SeqDaBx { get; set; }

    [Column("Data da Bx", TypeName = "datetime")]
    public DateTime DataDaBx { get; set; }

    [Column("Cod do Vendedor")]
    public int CodDoVendedor { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FiltroIni { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FiltroFim { get; set; }

    [Column("Usu da Baixa")]
    [StringLength(40)]
    [Unicode(false)]
    public string UsuDaBaixa { get; set; } = null!;

    public bool Fechado { get; set; }
}
