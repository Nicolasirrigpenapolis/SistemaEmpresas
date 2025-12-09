using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaAgencia", "SequenciaDaCcDaAgencia")]
[Table("Conta Corrente da Agência")]
public partial class ContaCorrenteDaAgencium
{
    [Key]
    [Column("Seqüência da Agência")]
    public short SequenciaDaAgencia { get; set; }

    [Key]
    [Column("Seqüência da CC da Agência")]
    public short SequenciaDaCcDaAgencia { get; set; }

    [Column("Número da Conta Corrente")]
    [StringLength(11)]
    [Unicode(false)]
    public string NumeroDaContaCorrente { get; set; } = null!;

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Valor de Saída", TypeName = "decimal(11, 2)")]
    public decimal ValorDeSaida { get; set; }

    [Column("Valor de Entrada", TypeName = "decimal(11, 2)")]
    public decimal ValorDeEntrada { get; set; }

    [Column("Valor Atual", TypeName = "decimal(11, 2)")]
    public decimal ValorAtual { get; set; }

    public bool Inativo { get; set; }

    [Column("BBApiClientId")]
    [StringLength(500)]
    public string? BbapiClientId { get; set; }

    [Column("BBApiClientSecret")]
    [StringLength(500)]
    public string? BbapiClientSecret { get; set; }

    [Column("BBApiDeveloperKey")]
    [StringLength(500)]
    public string? BbapiDeveloperKey { get; set; }

    [Column("HabilitarIntegracaoBB")]
    public bool? HabilitarIntegracaoBb { get; set; }

    [StringLength(2)]
    public string? DigitoConta { get; set; }

    [InverseProperty("ContaCorrenteDaAgencium")]
    public virtual ICollection<BaixaConta> BaixaConta { get; set; } = new List<BaixaConta>();

    [InverseProperty("ContaCorrenteDaAgencium")]
    public virtual ICollection<MovimentacaoDaContaCorrente> MovimentacaoDaContaCorrentes { get; set; } = new List<MovimentacaoDaContaCorrente>();
}
