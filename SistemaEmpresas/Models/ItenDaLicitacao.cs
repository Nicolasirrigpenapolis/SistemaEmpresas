using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("Sequencia", "Produto", "CodDespesa", "SequencialDeUm")]
[Table("Itens da Licitacao")]
public partial class ItenDaLicitacao
{
    [Key]
    public int Sequencia { get; set; }

    [Key]
    public int Produto { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Unidade { get; set; } = null!;

    [Column("Qtde 1", TypeName = "decimal(10, 2)")]
    public decimal Qtde1 { get; set; }

    [Column("Vr Unit 1", TypeName = "decimal(10, 2)")]
    public decimal VrUnit1 { get; set; }

    [Column("Vr Total 1", TypeName = "decimal(10, 2)")]
    public decimal VrTotal1 { get; set; }

    [Column("Qtde 2", TypeName = "decimal(10, 2)")]
    public decimal Qtde2 { get; set; }

    [Column("Vr Unit 2", TypeName = "decimal(10, 2)")]
    public decimal VrUnit2 { get; set; }

    [Column("Vr Total 2", TypeName = "decimal(10, 2)")]
    public decimal VrTotal2 { get; set; }

    [Column("Qtde 3", TypeName = "decimal(10, 2)")]
    public decimal Qtde3 { get; set; }

    [Column("Vr Unit 3", TypeName = "decimal(10, 2)")]
    public decimal VrUnit3 { get; set; }

    [Column("Vr Total 3", TypeName = "decimal(10, 2)")]
    public decimal VrTotal3 { get; set; }

    [Key]
    [Column("Cod Despesa")]
    public int CodDespesa { get; set; }

    [Key]
    [Column("Sequencial de Um")]
    public int SequencialDeUm { get; set; }

    [ForeignKey("Produto")]
    [InverseProperty("ItensDaLicitacaos")]
    public virtual Produto ProdutoNavigation { get; set; } = null!;
}
