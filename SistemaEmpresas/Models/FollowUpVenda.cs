using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Follow Up Vendas")]
public partial class FollowUpVenda
{
    [Key]
    [Column("Seq Follow Up")]
    public int SeqFollowUp { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Seqüência da Transportadora")]
    public int SequenciaDaTransportadora { get; set; }

    [Column("Data de Entrega", TypeName = "datetime")]
    public DateTime? DataDeEntrega { get; set; }

    public short Dias { get; set; }

    [Column("Det 1")]
    [StringLength(100)]
    [Unicode(false)]
    public string Det1 { get; set; } = null!;

    [Column("Det 2", TypeName = "text")]
    public string Det2 { get; set; } = null!;

    [Column("Serie do Equipamento")]
    [StringLength(25)]
    [Unicode(false)]
    public string SerieDoEquipamento { get; set; } = null!;

    [Column("Descr do Material", TypeName = "text")]
    public string? DescrDoMaterial { get; set; }

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [StringLength(19)]
    [Unicode(false)]
    public string Stat { get; set; } = null!;

    [Column("Venda Fechada")]
    public bool VendaFechada { get; set; }

    [StringLength(14)]
    [Unicode(false)]
    public string Telefone { get; set; } = null!;
}
