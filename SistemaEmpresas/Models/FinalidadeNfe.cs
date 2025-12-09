using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Finalidade NFe")]
public partial class FinalidadeNfe
{
    [Key]
    public short Codigo { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Finalidade { get; set; } = null!;
}
