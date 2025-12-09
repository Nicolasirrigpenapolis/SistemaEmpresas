using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Divirgencias NFe")]
public partial class DivirgenciaNfe
{
    [Key]
    [Column("Codigo da Divirgencia")]
    public int CodigoDaDivirgencia { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Número da NFe")]
    public int NumeroDaNfe { get; set; }

    [Column("CFOP")]
    public short Cfop { get; set; }

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;
}
