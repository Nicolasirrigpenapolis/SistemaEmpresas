using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Vasilhame
{
    [Key]
    [Column("Sequencia do Vasilahme")]
    public int SequenciaDoVasilahme { get; set; }

    [Column("Número da NFe")]
    public int NumeroDaNfe { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Mov { get; set; } = null!;
}
