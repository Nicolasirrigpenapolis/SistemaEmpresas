using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Duplicatas Descontadas")]
public partial class DuplicataDescontada
{
    [Key]
    [Column("Seq da Duplicata")]
    public int SeqDaDuplicata { get; set; }

    public int Duplicata { get; set; }

    public short Pc { get; set; }

    [Column("Cod do Geral")]
    public int CodDoGeral { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Vencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [Column("Tpo de Carteira")]
    [StringLength(8)]
    [Unicode(false)]
    public string TpoDeCarteira { get; set; } = null!;

    [Column("Data da Baixa", TypeName = "datetime")]
    public DateTime DataDaBaixa { get; set; }

    [Column("Cod do Banco")]
    public int CodDoBanco { get; set; }

    [Column("Cc do Banco")]
    public int CcDoBanco { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;
}
