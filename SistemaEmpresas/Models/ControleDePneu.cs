using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoProjeto", "SequenciaDoPneu")]
[Table("Controle de Pneus")]
public partial class ControleDePneu
{
    [Key]
    [Column("Sequencia do Projeto")]
    public int SequenciaDoProjeto { get; set; }

    [Key]
    [Column("Sequencia do Pneu")]
    public int SequenciaDoPneu { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("NFe Saidas", TypeName = "decimal(10, 2)")]
    public decimal NfeSaidas { get; set; }

    [Column("Modelo do Pneu")]
    [StringLength(30)]
    [Unicode(false)]
    public string ModeloDoPneu { get; set; } = null!;

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;
}
