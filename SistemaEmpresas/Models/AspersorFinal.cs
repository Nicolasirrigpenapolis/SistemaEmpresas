using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Aspersor Final")]
public partial class AspersorFinal
{
    [Key]
    [Column("Sequencia do Aspersor")]
    public int SequenciaDoAspersor { get; set; }

    [Column("Modelo do aspersor")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloDoAspersor { get; set; } = null!;

    [Column("Canhao ou Aspersor")]
    [StringLength(8)]
    [Unicode(false)]
    public string CanhaoOuAspersor { get; set; } = null!;

    public int Bocal { get; set; }

    [Column("Pressão de Trabalho", TypeName = "decimal(8, 2)")]
    public decimal PressaoDeTrabalho { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Vazao { get; set; }

    [Column(TypeName = "decimal(8, 3)")]
    public decimal Alcance { get; set; }

    [Column("Area Final", TypeName = "decimal(8, 3)")]
    public decimal AreaFinal { get; set; }

    [Column("Volume de Referencia", TypeName = "decimal(7, 3)")]
    public decimal VolumeDeReferencia { get; set; }

    [Column("Percentual raio molhado", TypeName = "decimal(7, 3)")]
    public decimal PercentualRaioMolhado { get; set; }

    [Column("Alcance raio Molhado", TypeName = "decimal(7, 3)")]
    public decimal AlcanceRaioMolhado { get; set; }

    [Column("Area Considerada", TypeName = "decimal(7, 3)")]
    public decimal AreaConsiderada { get; set; }
}
