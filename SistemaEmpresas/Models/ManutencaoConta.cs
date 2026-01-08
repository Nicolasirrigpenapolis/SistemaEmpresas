using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Manutenção Contas")]
public partial class ManutencaoConta
{
    [Key]
    [Column("Seqüência da Manutenção")]
    public int SequenciaDaManutencao { get; set; }

    public short Parcela { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Número da Nota Fiscal")]
    public int NumeroDaNotaFiscal { get; set; }

    [Column("Data de Entrada", TypeName = "datetime")]
    public DateTime? DataDeEntrada { get; set; }

    [Column("Histórico", TypeName = "text")]
    public string Historico { get; set; } = null!;

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime? DataDeVencimento { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [Column("Valor Pago", TypeName = "decimal(11, 2)")]
    public decimal ValorPago { get; set; }

    [Column("Valor do Juros", TypeName = "decimal(11, 2)")]
    public decimal ValorDoJuros { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column("Valor Restante", TypeName = "decimal(11, 2)")]
    public decimal ValorRestante { get; set; }

    [Column("Tipo da Conta")]
    [StringLength(11)]
    [Unicode(false)]
    public string TipoDaConta { get; set; } = null!;

    [Column("Data da Baixa", TypeName = "datetime")]
    public DateTime? DataDaBaixa { get; set; }

    [Column("Seqüência da Cobrança")]
    public short SequenciaDaCobranca { get; set; }

    [Column("Número da Duplicata")]
    public int NumeroDaDuplicata { get; set; }

    [Column("Seqüência da Origem")]
    public int SequenciaDaOrigem { get; set; }

    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [Column("Cheque Impresso")]
    public bool ChequeImpresso { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Conta { get; set; } = null!;

    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("Seqüência do Estoque")]
    public int SequenciaDoEstoque { get; set; }

    [Column("Seqüência do Pedido")]
    public int SequenciaDoPedido { get; set; }

    [Column("Duplicata Impressa")]
    public bool DuplicataImpressa { get; set; }

    public bool Imprimir { get; set; }

    [Column("Tpo de Recebimento")]
    [StringLength(20)]
    [Unicode(false)]
    public string TpoDeRecebimento { get; set; } = null!;

    [Column("Previsão")]
    public bool Previsao { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    [Column("Sequencia da Compra")]
    public int SequenciaDaCompra { get; set; }

    [Column("Notas da Compra")]
    [StringLength(100)]
    [Unicode(false)]
    public string NotasDaCompra { get; set; } = null!;

    public bool Conciliado { get; set; }

    [Column("Vencimento Original", TypeName = "datetime")]
    public DateTime? VencimentoOriginal { get; set; }

    [Column("Sequencia Lan CC")]
    public int SequenciaLanCc { get; set; }

    [Column("Vr da Previsão", TypeName = "decimal(10, 2)")]
    public decimal VrDaPrevisao { get; set; }

    [Column("Imp Previsao")]
    public bool ImpPrevisao { get; set; }

    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Column("Codigo do Debito")]
    public int CodigoDoDebito { get; set; }

    [Column("Codigo do Credito")]
    public int CodigoDoCredito { get; set; }

    [InverseProperty("SequenciaDaManutencaoNavigation")]
    public virtual ICollection<BaixaConta> BaixaConta { get; set; } = new List<BaixaConta>();

    [ForeignKey("SequenciaDaCobranca")]
    [InverseProperty("ManutencaoConta")]
    public virtual TipoDeCobranca SequenciaDaCobrancaNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("ManutencaoConta")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("ManutencaoConta")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaGrupoDespesa")]
    [InverseProperty("ManutencaoConta")]
    public virtual GrupoDaDespesa SequenciaGrupoDespesaNavigation { get; set; } = null!;

    [ForeignKey("SeqüênciaSubGrupoDespesa, SeqüênciaGrupoDespesa")]
    [InverseProperty("ManutencaoConta")]
    public virtual SubGrupoDespesa SubGrupoDespesa { get; set; } = null!;

    [InverseProperty("SequenciaDaManutencaoNavigation")]
    public virtual ICollection<ValorAdicionai> ValoresAdicionais { get; set; } = new List<ValorAdicionai>();
}
