using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("NumeroDaParcela", "SequenciaDoMovimento")]
[Table("Parcelas Movimento Estoque")]
public partial class ParcelaMovimentoEstoque
{
    [Key]
    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Key]
    [Column("Número da Parcela")]
    public short NumeroDaParcela { get; set; }

    public short Dias { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime DataDeVencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [Column("Seqüência da Cobrança")]
    public short SequenciaDaCobranca { get; set; }

    [ForeignKey("SequenciaDoMovimento")]
    [InverseProperty("ParcelasMovimentoEstoques")]
    public virtual MovimentoDoEstoque SequenciaDoMovimentoNavigation { get; set; } = null!;
}
