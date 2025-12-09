using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Conta do Vendedor")]
public partial class ContaDoVendedor
{
    [Key]
    [Column("Id da Conta")]
    public int IdDaConta { get; set; }

    [Column("Titular da Conta")]
    [StringLength(40)]
    [Unicode(false)]
    public string TitularDaConta { get; set; } = null!;

    public bool Desativado { get; set; }

    [Column("A Liberar", TypeName = "decimal(10, 2)")]
    public decimal ALiberar { get; set; }

    [Column("Gerente Regional")]
    [StringLength(40)]
    [Unicode(false)]
    public string GerenteRegional { get; set; } = null!;

    [Column("Faz projeto")]
    public bool FazProjeto { get; set; }

    public bool Montador { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Percentual { get; set; }

    public bool Revenda { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [InverseProperty("CodDoVendedorNavigation")]
    public virtual ICollection<PlanilhaDeAdiantamento> PlanilhaDeAdiantamentos { get; set; } = new List<PlanilhaDeAdiantamento>();

    [InverseProperty("IdDaContaNavigation")]
    public virtual ICollection<Revendedore> Revendedores { get; set; } = new List<Revendedore>();
}
