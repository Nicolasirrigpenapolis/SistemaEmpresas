using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Movimentação da Conta Corrente")]
public partial class MovimentacaoDaContaCorrente
{
    [Key]
    [Column("Seqüência da Movimentação CC")]
    public int SequenciaDaMovimentacaoCc { get; set; }

    [Column("Seqüência da Agência")]
    public short SequenciaDaAgencia { get; set; }

    [Column("Seqüência da CC da Agência")]
    public short SequenciaDaCcDaAgencia { get; set; }

    [Column("Tipo de Movimento da CC")]
    [StringLength(7)]
    [Unicode(false)]
    public string TipoDeMovimentoDaCc { get; set; } = null!;

    [Column("Data do Movimento", TypeName = "datetime")]
    public DateTime DataDoMovimento { get; set; }

    [Column("Data do Último Dia", TypeName = "datetime")]
    public DateTime? DataDoUltimoDia { get; set; }

    [Column(TypeName = "text")]
    public string Historico { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Conta { get; set; } = null!;

    [Column("Seqüência do Lançamento")]
    public int SequenciaDoLancamento { get; set; }

    [Column("Seqüência do Histórico")]
    public short SequenciaDoHistorico { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Origem da Movimentação")]
    [StringLength(30)]
    [Unicode(false)]
    public string OrigemDaMovimentacao { get; set; } = null!;

    public bool Blokeado { get; set; }

    [Column("Codigo do Historico")]
    public short CodigoDoHistorico { get; set; }

    [Column("Codigo do Debito")]
    public int CodigoDoDebito { get; set; }

    [Column("Codigo do Credito")]
    public int CodigoDoCredito { get; set; }

    [Column("Usuário da Alteração")]
    [StringLength(60)]
    [Unicode(false)]
    public string UsuarioDaAlteracao { get; set; } = null!;

    [Column("Data da Alteração", TypeName = "datetime")]
    public DateTime? DataDaAlteracao { get; set; }

    [Column("Hora da Alteração", TypeName = "datetime")]
    public DateTime? HoraDaAlteracao { get; set; }

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [InverseProperty("SequenciaDaMovimentacaoCcNavigation")]
    public virtual ICollection<BaixaConta> BaixaConta { get; set; } = new List<BaixaConta>();

    [ForeignKey("SeqüênciaDaAgência, SeqüênciaDaCcDaAgência")]
    [InverseProperty("MovimentacaoDaContaCorrentes")]
    public virtual ContaCorrenteDaAgencium ContaCorrenteDaAgencium { get; set; } = null!;

    [ForeignKey("SequenciaDoHistorico")]
    [InverseProperty("MovimentacaoDaContaCorrentes")]
    public virtual HistoricoDaContaCorrente SequenciaDoHistoricoNavigation { get; set; } = null!;
}
