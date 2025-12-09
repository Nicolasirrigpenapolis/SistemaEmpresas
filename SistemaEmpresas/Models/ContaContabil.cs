using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Conta Contabil")]
public partial class ContaContabil
{
    [Key]
    [Column("Codigo Contabil")]
    public int CodigoContabil { get; set; }

    [Column("Conta Contabil")]
    [StringLength(56)]
    [Unicode(false)]
    public string ContaContabil1 { get; set; } = null!;

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Sequencia do Geral")]
    public int SequenciaDoGeral { get; set; }
}
