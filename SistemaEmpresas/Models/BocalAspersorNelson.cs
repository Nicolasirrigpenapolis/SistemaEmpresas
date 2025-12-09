using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Bocal Aspersor Nelson")]
public partial class BocalAspersorNelson
{
    [Key]
    [Column("Sequencia do Bocal")]
    public int SequenciaDoBocal { get; set; }

    [Column("Modelo Aspersor")]
    [StringLength(9)]
    [Unicode(false)]
    public string ModeloAspersor { get; set; } = null!;

    [Column("Bocal do Aspersor", TypeName = "decimal(5, 2)")]
    public decimal BocalDoAspersor { get; set; }

    [Column("MCA", TypeName = "decimal(7, 2)")]
    public decimal Mca { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Vazao { get; set; }

    [Column("Raio de Alcance metros", TypeName = "decimal(6, 2)")]
    public decimal RaioDeAlcanceMetros { get; set; }

    [Column("Area Total ha", TypeName = "decimal(6, 2)")]
    public decimal AreaTotalHa { get; set; }

    [Column("Volume Referencia mm", TypeName = "decimal(6, 2)")]
    public decimal VolumeReferenciaMm { get; set; }

    [Column("Percentual alcance Molhado", TypeName = "decimal(6, 2)")]
    public decimal PercentualAlcanceMolhado { get; set; }

    [Column("Alcence Raio Molhado m", TypeName = "decimal(6, 2)")]
    public decimal AlcenceRaioMolhadoM { get; set; }

    [Column("Alcence aspersor final ha", TypeName = "decimal(6, 2)")]
    public decimal AlcenceAspersorFinalHa { get; set; }

    [Column("Fabricante do Aspersor")]
    [StringLength(12)]
    [Unicode(false)]
    public string FabricanteDoAspersor { get; set; } = null!;
}
