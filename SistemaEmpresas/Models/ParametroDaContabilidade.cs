using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Parâmetros da Contabilidade")]
public partial class ParametroDaContabilidade
{
    [Column("Ano Contábil")]
    public short AnoContabil { get; set; }

    [Column("Trimestre Contábil")]
    [StringLength(8)]
    [Unicode(false)]
    public string TrimestreContabil { get; set; } = null!;
}
