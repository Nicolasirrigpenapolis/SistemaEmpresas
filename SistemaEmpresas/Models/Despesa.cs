using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Despesa
{
    [Key]
    [Column("Seqüência da Despesa")]
    public int SequenciaDaDespesa { get; set; }

    public bool Inativo { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [Column("Seqüência da Unidade")]
    public short SequenciaDaUnidade { get; set; }

    [Column("Quantidade no Estoque", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeNoEstoque { get; set; }

    [Column("Quantidade Mínima", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeMinima { get; set; }

    [Column("Código de Barras")]
    [StringLength(13)]
    [Unicode(false)]
    public string CodigoDeBarras { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Localizacao { get; set; } = null!;

    [Column("Última Compra", TypeName = "datetime")]
    public DateTime? UltimaCompra { get; set; }

    [Column("Último Movimento", TypeName = "datetime")]
    public DateTime? UltimoMovimento { get; set; }

    [Column("Valor de Custo", TypeName = "decimal(12, 4)")]
    public decimal ValorDeCusto { get; set; }

    [Column("Custo Médio", TypeName = "decimal(11, 2)")]
    public decimal CustoMedio { get; set; }

    [Column("Último Fornecedor")]
    public int UltimoFornecedor { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("Tipo do Produto")]
    public short TipoDoProduto { get; set; }

    [Column("Quantidade Contábil", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeContabil { get; set; }

    [Column("Valor Contábil Atual", TypeName = "decimal(13, 4)")]
    public decimal ValorContabilAtual { get; set; }

    [Column("Margem de Lucro", TypeName = "decimal(7, 2)")]
    public decimal MargemDeLucro { get; set; }

    [Column("Movimenta ficha")]
    public bool MovimentaFicha { get; set; }

    [InverseProperty("SequenciaDaDespesaNavigation")]
    public virtual ICollection<BaixaDoEstoqueContabil> BaixaDoEstoqueContabils { get; set; } = new List<BaixaDoEstoqueContabil>();

    [InverseProperty("IdDaDespesaNavigation")]
    public virtual ICollection<ConsumoDoPedidoCompra> ConsumoDoPedidoCompras { get; set; } = new List<ConsumoDoPedidoCompra>();

    [InverseProperty("SequenciaDaDespesaNavigation")]
    public virtual ICollection<DespesaDaLicitacao> DespesasDaLicitacaos { get; set; } = new List<DespesaDaLicitacao>();

    [InverseProperty("SequenciaDaDespesaNavigation")]
    public virtual ICollection<DespesaDoMovimentoContabil> DespesasDoMovimentoContabils { get; set; } = new List<DespesaDoMovimentoContabil>();

    [InverseProperty("SequenciaDaDespesaNavigation")]
    public virtual ICollection<DespesaDoMovimentoEstoque> DespesasDoMovimentoEstoques { get; set; } = new List<DespesaDoMovimentoEstoque>();

    [InverseProperty("SequenciaDaDespesaNavigation")]
    public virtual ICollection<DespesaDoNovoPedido> DespesasDoNovoPedidos { get; set; } = new List<DespesaDoNovoPedido>();

    [InverseProperty("IdDaDespesaNavigation")]
    public virtual ICollection<DespesaDoPedidoCompra> DespesasDoPedidoCompras { get; set; } = new List<DespesaDoPedidoCompra>();

    [InverseProperty("SequenciaDaDespesaNavigation")]
    public virtual ICollection<DespesaMvtoContabilNovo> DespesasMvtoContabilNovos { get; set; } = new List<DespesaMvtoContabilNovo>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("Despesas")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaUnidade")]
    [InverseProperty("Despesas")]
    public virtual Unidade SequenciaDaUnidadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaGrupoDespesa")]
    [InverseProperty("Despesas")]
    public virtual GrupoDaDespesa SequenciaGrupoDespesaNavigation { get; set; } = null!;

    [ForeignKey("SeqüênciaSubGrupoDespesa, SeqüênciaGrupoDespesa")]
    [InverseProperty("Despesas")]
    public virtual SubGrupoDespesa SubGrupoDespesa { get; set; } = null!;
}
