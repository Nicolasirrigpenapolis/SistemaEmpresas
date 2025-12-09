using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Licitacao")]
public partial class Licitacao
{
    [Key]
    [Column("Sequencia da Licitacao")]
    public int SequenciaDaLicitacao { get; set; }

    [Column("Data da Licitacao", TypeName = "datetime")]
    public DateTime DataDaLicitacao { get; set; }

    [Column("For 1")]
    [StringLength(40)]
    [Unicode(false)]
    public string For1 { get; set; } = null!;

    [Column("Contato 1")]
    [StringLength(25)]
    [Unicode(false)]
    public string Contato1 { get; set; } = null!;

    [Column("Fone 1")]
    [StringLength(14)]
    [Unicode(false)]
    public string Fone1 { get; set; } = null!;

    [Column("Prev Entrega 1")]
    [StringLength(10)]
    [Unicode(false)]
    public string PrevEntrega1 { get; set; } = null!;

    [Column("Cond Pagto 1")]
    [StringLength(20)]
    [Unicode(false)]
    public string CondPagto1 { get; set; } = null!;

    [Column("For 2")]
    [StringLength(40)]
    [Unicode(false)]
    public string For2 { get; set; } = null!;

    [Column("Contato 2")]
    [StringLength(25)]
    [Unicode(false)]
    public string Contato2 { get; set; } = null!;

    [Column("Fone 2")]
    [StringLength(14)]
    [Unicode(false)]
    public string Fone2 { get; set; } = null!;

    [Column("Prev Entrega 2")]
    [StringLength(10)]
    [Unicode(false)]
    public string PrevEntrega2 { get; set; } = null!;

    [Column("Cond Pagto 2")]
    [StringLength(20)]
    [Unicode(false)]
    public string CondPagto2 { get; set; } = null!;

    [Column("For 3")]
    [StringLength(40)]
    [Unicode(false)]
    public string For3 { get; set; } = null!;

    [Column("Contato 3")]
    [StringLength(25)]
    [Unicode(false)]
    public string Contato3 { get; set; } = null!;

    [Column("Fone 3")]
    [StringLength(14)]
    [Unicode(false)]
    public string Fone3 { get; set; } = null!;

    [Column("Prev Entrega 3")]
    [StringLength(10)]
    [Unicode(false)]
    public string PrevEntrega3 { get; set; } = null!;

    [Column("Cond Pagto 3")]
    [StringLength(20)]
    [Unicode(false)]
    public string CondPagto3 { get; set; } = null!;

    public bool Fechado { get; set; }
}
