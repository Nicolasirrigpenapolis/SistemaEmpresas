using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Serie Hidroturbo")]
public partial class SerieHidroturbo
{
    [Key]
    [Column("Seq do Hidroturbo")]
    public int SeqDoHidroturbo { get; set; }

    [Column("Modelo do Hidroturbo")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloDoHidroturbo { get; set; } = null!;

    [Column("Serie do Hidroturbo")]
    public int SerieDoHidroturbo { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string MesAno { get; set; } = null!;

    [Column("Letra do Hidroturbo")]
    [StringLength(1)]
    [Unicode(false)]
    public string LetraDoHidroturbo { get; set; } = null!;

    [Column("Carretel de")]
    [StringLength(3)]
    [Unicode(false)]
    public string CarretelDe { get; set; } = null!;

    [Column("Data de Criação", TypeName = "datetime")]
    public DateTime DataDeCriacao { get; set; }

    [Column("Nro de Serie Hidroturbo")]
    [StringLength(30)]
    [Unicode(false)]
    public string NroDeSerieHidroturbo { get; set; } = null!;

    [Column("Codigo do Geral")]
    public int CodigoDoGeral { get; set; }

    [Column("Codigo do Vendedor")]
    public int CodigoDoVendedor { get; set; }

    public bool Entregue { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    [Column("NF")]
    [StringLength(60)]
    [Unicode(false)]
    public string Nf { get; set; } = null!;

    [Column("Dta da Entrega", TypeName = "datetime")]
    public DateTime? DtaDaEntrega { get; set; }

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

    [Column("Aparecer no Filtro")]
    public bool AparecerNoFiltro { get; set; }
}
