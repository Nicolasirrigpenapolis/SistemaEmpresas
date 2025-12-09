using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Veiculos do Motorista")]
public partial class VeiculoDoMotoristum
{
    [Column("Codigo do Motorista")]
    public short CodigoDoMotorista { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Automovel { get; set; } = null!;

    [Column("Placa do Automovel")]
    [StringLength(20)]
    [Unicode(false)]
    public string PlacaDoAutomovel { get; set; } = null!;

    [Column("Placa da Carreta")]
    [StringLength(20)]
    [Unicode(false)]
    public string PlacaDaCarreta { get; set; } = null!;
}
