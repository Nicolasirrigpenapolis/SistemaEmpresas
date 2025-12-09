using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Serie Moto Bomba")]
public partial class SerieMotoBomba
{
    [Key]
    [Column("Seq Moto Bomba")]
    public int SeqMotoBomba { get; set; }

    [Column("Tp de Motor")]
    [StringLength(1)]
    [Unicode(false)]
    public string TpDeMotor { get; set; } = null!;

    [Column("Serie da Moto Bomba")]
    public int SerieDaMotoBomba { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string MesAno { get; set; } = null!;

    [Column("Função Moto Bomba")]
    [StringLength(3)]
    [Unicode(false)]
    public string FuncaoMotoBomba { get; set; } = null!;

    [Column("Nro de Serie Moto Bomba")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDeSerieMotoBomba { get; set; } = null!;

    [Column("Nro de Serie Motor")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDeSerieMotor { get; set; } = null!;

    [Column("Modelo do Motor")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloDoMotor { get; set; } = null!;

    [Column("Nro de Serie Bomba")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDeSerieBomba { get; set; } = null!;

    [Column("Modelo da Bomba")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloDaBomba { get; set; } = null!;

    [Column("Data de Criação", TypeName = "datetime")]
    public DateTime DataDeCriacao { get; set; }

    [Column("Codigo do Geral")]
    public int CodigoDoGeral { get; set; }

    [Column("Codigo do Vendedor")]
    public int CodigoDoVendedor { get; set; }

    public bool Entregue { get; set; }

    [Column("NF")]
    [StringLength(60)]
    [Unicode(false)]
    public string Nf { get; set; } = null!;

    [Column("Dt da Entrega", TypeName = "datetime")]
    public DateTime? DtDaEntrega { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    [Column("Entrega Tecnica")]
    [StringLength(19)]
    [Unicode(false)]
    public string EntregaTecnica { get; set; } = null!;

    [Column("Entrega Tec")]
    public bool EntregaTec { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Codigo do solicitante")]
    public short CodigoDoSolicitante { get; set; }

    [Column("Data Tecnica", TypeName = "datetime")]
    public DateTime? DataTecnica { get; set; }
}
