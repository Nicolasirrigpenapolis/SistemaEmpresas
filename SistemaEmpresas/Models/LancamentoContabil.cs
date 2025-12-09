using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Lançamentos Contabil")]
public partial class LancamentoContabil
{
    [Key]
    [Column("Id do Lançamento")]
    public int IdDoLancamento { get; set; }

    [Column("Dt do Lançamento")]
    [StringLength(5)]
    [Unicode(false)]
    public string? DtDoLancamento { get; set; }

    [Column("Conta Debito")]
    public int ContaDebito { get; set; }

    [Column("Conta Credito")]
    public int ContaCredito { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Valor { get; set; }

    [Column("Codigo do Historico")]
    public short CodigoDoHistorico { get; set; }

    [Column("Complemento do Hist", TypeName = "text")]
    public string ComplementoDoHist { get; set; } = null!;

    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }

    [Column("Seqüência da Movimentação CC")]
    public int SequenciaDaMovimentacaoCc { get; set; }

    [Column("Data da Baixa", TypeName = "datetime")]
    public DateTime? DataDaBaixa { get; set; }

    public bool Gerado { get; set; }
}
