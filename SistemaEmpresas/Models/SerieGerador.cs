using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Serie Gerador")]
public partial class SerieGerador
{
    [Key]
    [Column("Seq do Gerador")]
    public int SeqDoGerador { get; set; }

    [Column("Descri do Gerador")]
    [StringLength(50)]
    [Unicode(false)]
    public string DescriDoGerador { get; set; } = null!;

    [Column("Serie do Gerador")]
    public short SerieDoGerador { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string MesAno { get; set; } = null!;

    [Column("Data de Criação", TypeName = "datetime")]
    public DateTime DataDeCriacao { get; set; }

    [Column("Nro de Serie do Ger")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDeSerieDoGer { get; set; } = null!;

    [Column("Nro do Motor")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDoMotor { get; set; } = null!;

    [Column("Nro do Gerador")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDoGerador { get; set; } = null!;

    [Column("Codigo do Geral")]
    public int CodigoDoGeral { get; set; }

    public bool Entregue { get; set; }

    [Column("Dt de Entrega", TypeName = "datetime")]
    public DateTime? DtDeEntrega { get; set; }

    [Column("NF")]
    [StringLength(60)]
    [Unicode(false)]
    public string Nf { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;
}
