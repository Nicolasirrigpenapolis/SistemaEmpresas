using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Despesas e vendas")]
public partial class DespesaEVenda
{
    [Key]
    [Column("Sequencia da simulação")]
    public int SequenciaDaSimulacao { get; set; }

    [Column("Sequencia do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Total da Viagem", TypeName = "decimal(10, 2)")]
    public decimal TotalDaViagem { get; set; }

    [Column("Valor do orçamento", TypeName = "decimal(12, 2)")]
    public decimal ValorDoOrcamento { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Saldo { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Comissao { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Salario { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Ref { get; set; } = null!;
}
