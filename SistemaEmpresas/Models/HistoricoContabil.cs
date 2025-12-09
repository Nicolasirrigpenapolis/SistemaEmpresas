using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Historico Contabil")]
public partial class HistoricoContabil
{
    [Key]
    [Column("Codigo do Historico")]
    public short CodigoDoHistorico { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    public bool Inativo { get; set; }
}
