using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("NumeroDaParcela", "SequenciaDaEntrada")]
[Table("Parcelas Entrada Contas")]
public partial class ParcelaEntradaConta
{
    [Key]
    [Column("Número da Parcela")]
    public short NumeroDaParcela { get; set; }

    [Key]
    [Column("Seqüência da Entrada")]
    public int SequenciaDaEntrada { get; set; }

    public short Dias { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime DataDeVencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [Column("Seqüência da Cobrança")]
    public short SequenciaDaCobranca { get; set; }

    [ForeignKey("SequenciaDaEntrada")]
    [InverseProperty("ParcelasEntradaConta")]
    public virtual EntradaConta SequenciaDaEntradaNavigation { get; set; } = null!;
}
