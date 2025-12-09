using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Serie Pivos")]
public partial class SeriePivo
{
    [Key]
    [Column("Seq do Pivo")]
    public int SeqDoPivo { get; set; }

    [Column("Modelo do Pivo")]
    [StringLength(6)]
    [Unicode(false)]
    public string ModeloDoPivo { get; set; } = null!;

    [Column("Descri do Pivo")]
    [StringLength(30)]
    [Unicode(false)]
    public string DescriDoPivo { get; set; } = null!;

    [Column("Serie do Pivo")]
    public short SerieDoPivo { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string MesAno { get; set; } = null!;

    [Column("Letra do Pivo")]
    [StringLength(1)]
    [Unicode(false)]
    public string LetraDoPivo { get; set; } = null!;

    [Column("Data de Criação", TypeName = "datetime")]
    public DateTime DataDeCriacao { get; set; }

    [Column("Nro de Serie do Pivo")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDeSerieDoPivo { get; set; } = null!;

    [Column("Codigo do Geral")]
    public int CodigoDoGeral { get; set; }

    public bool Entregue { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    [Column("NF")]
    [StringLength(60)]
    [Unicode(false)]
    public string Nf { get; set; } = null!;

    [Column("Data da Entrega", TypeName = "datetime")]
    public DateTime? DataDaEntrega { get; set; }

    [Column("Codigo do Vendedor")]
    public int CodigoDoVendedor { get; set; }

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

    [Column("Prev Montagem", TypeName = "datetime")]
    public DateTime? PrevMontagem { get; set; }

    [Column(TypeName = "text")]
    public string DadosAd { get; set; } = null!;
}
