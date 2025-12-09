using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Ordem de Montagem")]
public partial class OrdemDeMontagem
{
    [Key]
    [Column("Sequencia da Montagem")]
    public int SequenciaDaMontagem { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Origem { get; set; } = null!;

    [Column("Sequencia da Origem")]
    public int SequenciaDaOrigem { get; set; }

    [Column("Codigo do Geral")]
    public int CodigoDoGeral { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    [Column("Sequencia da Origem 2")]
    public int SequenciaDaOrigem2 { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    [Column("Valor Total dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosProdutos { get; set; }

    [Column("Total de Ipi", TypeName = "decimal(10, 2)")]
    public decimal TotalDeIpi { get; set; }

    [Column("Valor Total dos Serviços", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosServicos { get; set; }

    [Column("Total da Ordem", TypeName = "decimal(10, 2)")]
    public decimal TotalDaOrdem { get; set; }

    [Column("Km Ini", TypeName = "decimal(8, 2)")]
    public decimal KmIni { get; set; }

    [Column("Km Final", TypeName = "decimal(8, 2)")]
    public decimal KmFinal { get; set; }

    [Column("Total Km", TypeName = "decimal(9, 2)")]
    public decimal TotalKm { get; set; }

    [Column("Vr Km", TypeName = "decimal(8, 2)")]
    public decimal VrKm { get; set; }

    [Column("Vr Total Km", TypeName = "decimal(10, 2)")]
    public decimal VrTotalKm { get; set; }
}
