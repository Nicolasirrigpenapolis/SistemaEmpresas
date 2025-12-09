using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Planilha de Adiantamento")]
public partial class PlanilhaDeAdiantamento
{
    [Key]
    [Column("Seq do Adiantamento")]
    public int SeqDoAdiantamento { get; set; }

    public short Ano { get; set; }

    [Column("Cod do Vendedor")]
    public int CodDoVendedor { get; set; }

    public int Manutencao { get; set; }

    [Column("Emissão NFe", TypeName = "datetime")]
    public DateTime? EmissaoNfe { get; set; }

    [Column("NFe")]
    [StringLength(10)]
    [Unicode(false)]
    public string Nfe { get; set; } = null!;

    public short Parcela { get; set; }

    [Column("Cod do Geral")]
    public int CodDoGeral { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Vencto { get; set; }

    [Column("Pagto Cliente", TypeName = "datetime")]
    public DateTime? PagtoCliente { get; set; }

    [Column("VrIPI", TypeName = "decimal(10, 2)")]
    public decimal VrIpi { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Percentual { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Comissao { get; set; }

    [Column("Pagto Vendedor", TypeName = "datetime")]
    public DateTime? PagtoVendedor { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    public bool Devolucao { get; set; }

    [Column("Valor Pago", TypeName = "decimal(11, 2)")]
    public decimal ValorPago { get; set; }

    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }

    [ForeignKey("CodDoVendedor")]
    [InverseProperty("PlanilhaDeAdiantamentos")]
    public virtual ContaDoVendedor CodDoVendedorNavigation { get; set; } = null!;
}
