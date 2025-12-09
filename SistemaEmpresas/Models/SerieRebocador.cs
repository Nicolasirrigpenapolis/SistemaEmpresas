using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Serie Rebocador")]
public partial class SerieRebocador
{
    [Key]
    [Column("Seq do Rebocador")]
    public int SeqDoRebocador { get; set; }

    [Column("Data de Criação", TypeName = "datetime")]
    public DateTime DataDeCriacao { get; set; }

    [Column("Serie do Rebocador")]
    public short SerieDoRebocador { get; set; }

    [Column("Modelo do Rebocador")]
    [StringLength(50)]
    [Unicode(false)]
    public string ModeloDoRebocador { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string MesAno { get; set; } = null!;

    [Column("Nro de Serie Rebocador")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDeSerieRebocador { get; set; } = null!;

    [Column("Codigo do Geral")]
    public int CodigoDoGeral { get; set; }

    public bool Entregue { get; set; }

    [Column("NF")]
    [StringLength(60)]
    [Unicode(false)]
    public string Nf { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    [Column("Data da Entrega", TypeName = "datetime")]
    public DateTime? DataDaEntrega { get; set; }

    [Column("Codigo do Vendedor")]
    public int CodigoDoVendedor { get; set; }
}
