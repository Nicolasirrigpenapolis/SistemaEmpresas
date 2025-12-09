using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Baixa Contas")]
public partial class BaixaConta
{
    [Key]
    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }

    [Column("Seqüência da Manutenção")]
    public int SequenciaDaManutencao { get; set; }

    [Column("Data da Baixa", TypeName = "datetime")]
    public DateTime? DataDaBaixa { get; set; }

    [Column("Valor Pago", TypeName = "decimal(11, 2)")]
    public decimal ValorPago { get; set; }

    [Column("Valor do Juros", TypeName = "decimal(11, 2)")]
    public decimal ValorDoJuros { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column("Seqüência da Agência")]
    public short SequenciaDaAgencia { get; set; }

    [Column("Seqüência da CC da Agência")]
    public short SequenciaDaCcDaAgencia { get; set; }

    [Column("Número do Cheque")]
    [StringLength(20)]
    [Unicode(false)]
    public string NumeroDoCheque { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Conta { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Historico { get; set; } = null!;

    [Column("Seqüência da Movimentação CC")]
    public int SequenciaDaMovimentacaoCc { get; set; }

    public bool Bloqueado { get; set; }

    [StringLength(17)]
    [Unicode(false)]
    public string Carteira { get; set; } = null!;

    [Column("Cliente Carteira")]
    [StringLength(9)]
    [Unicode(false)]
    public string ClienteCarteira { get; set; } = null!;

    [Column("Data Recebimento", TypeName = "datetime")]
    public DateTime? DataRecebimento { get; set; }

    public bool Compensado { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Pago { get; set; }

    [Column("Dt Comissão", TypeName = "datetime")]
    public DateTime? DtComissao { get; set; }

    [Column("NF Comissão")]
    public int NfComissao { get; set; }

    [Column("Codigo do Historico")]
    public short CodigoDoHistorico { get; set; }

    [Column("Codigo do Debito")]
    public int CodigoDoDebito { get; set; }

    [Column("Codigo do Credito")]
    public int CodigoDoCredito { get; set; }

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    public int Beneficiario { get; set; }

    public bool? ProcessadoAutomaticamente { get; set; }

    [Column("SequenciaLancamentoBB")]
    public int? SequenciaLancamentoBb { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataCriacaoIntegracao { get; set; }

    [ForeignKey("SeqüênciaDaAgência, SeqüênciaDaCcDaAgência")]
    [InverseProperty("BaixaConta")]
    public virtual ContaCorrenteDaAgencium ContaCorrenteDaAgencium { get; set; } = null!;

    [ForeignKey("SequenciaDaManutencao")]
    [InverseProperty("BaixaConta")]
    public virtual ManutencaoConta SequenciaDaManutencaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaMovimentacaoCc")]
    [InverseProperty("BaixaConta")]
    public virtual MovimentacaoDaContaCorrente SequenciaDaMovimentacaoCcNavigation { get; set; } = null!;
}
