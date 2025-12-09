using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Pedido")]
public partial class Pedido
{
    [Key]
    [Column("Seqüência do Pedido")]
    public int SequenciaDoPedido { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    public short Fechamento { get; set; }

    [Column("Valor do Fechamento", TypeName = "decimal(11, 2)")]
    public decimal ValorDoFechamento { get; set; }

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Historico { get; set; } = null!;

    [Column("Valor Total IPI dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDosProdutos { get; set; }

    [Column("Valor Total IPI dos Conjuntos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDosConjuntos { get; set; }

    [Column("Valor Total do ICMS", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoIcms { get; set; }

    [Column("Valor Total dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosProdutos { get; set; }

    [Column("Valor Total dos Conjuntos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosConjuntos { get; set; }

    [Column("Valor Total de Produtos Usados", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDeProdutosUsados { get; set; }

    [Column("Valor Total Conjuntos Usados", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalConjuntosUsados { get; set; }

    [Column("Valor Total dos Serviços", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosServicos { get; set; }

    [Column("Valor Total do Pedido", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoPedido { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Seqüência da Ordem de Serviço")]
    public int SequenciaDaOrdemDeServico { get; set; }

    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("Seqüência do Vendedor")]
    public int SequenciaDoVendedor { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("Ocultar Valor Unitário")]
    public bool OcultarValorUnitario { get; set; }

    [Column("Pedido Cancelado")]
    public bool PedidoCancelado { get; set; }

    [Column("Valor Total IPI das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDasPecas { get; set; }

    [Column("Valor Total das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecas { get; set; }

    [Column("Valor Total das Peças Usadas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecasUsadas { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor Total da Base de Cálculo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaBaseDeCalculo { get; set; }

    [Column("Valor do Seguro", TypeName = "decimal(11, 2)")]
    public decimal ValorDoSeguro { get; set; }

    [Column("Data do Fechamento", TypeName = "datetime")]
    public DateTime? DataDoFechamento { get; set; }

    [Column("Valor Total do PIS", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoPis { get; set; }

    [Column("Valor Total do COFINS", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoCofins { get; set; }

    [Column("Valor Total da Base ST", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaBaseSt { get; set; }

    [Column("Valor Total do ICMS ST", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoIcmsSt { get; set; }

    [Column("Alíquota do ISS", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIss { get; set; }

    [Column("Reter ISS")]
    public bool ReterIss { get; set; }

    [Column("Entrega Futura")]
    public bool EntregaFutura { get; set; }

    [Column("Seqüência da Transportadora")]
    public int SequenciaDaTransportadora { get; set; }

    [Column("Valor do Imposto de Renda", TypeName = "decimal(11, 2)")]
    public decimal ValorDoImpostoDeRenda { get; set; }

    [Column("Valor Total do Tributo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoTributo { get; set; }

    [Column("Nao Movimentar Estoque")]
    public bool NaoMovimentarEstoque { get; set; }

    [InverseProperty("SequenciaDoPedidoNavigation")]
    public virtual ICollection<ConjuntoDoPedido> ConjuntosDoPedidos { get; set; } = new List<ConjuntoDoPedido>();

    [InverseProperty("SequenciaDoPedidoNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDoPedidoNavigation")]
    public virtual ICollection<ParcelaPedido> ParcelasPedidos { get; set; } = new List<ParcelaPedido>();

    [InverseProperty("SequenciaDoPedidoNavigation")]
    public virtual ICollection<PecaDoPedido> PecasDoPedidos { get; set; } = new List<PecaDoPedido>();

    [InverseProperty("SequenciaDoPedidoNavigation")]
    public virtual ICollection<ProdutoDoPedido> ProdutosDoPedidos { get; set; } = new List<ProdutoDoPedido>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("Pedidos")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaOrdemDeServico")]
    [InverseProperty("Pedidos")]
    public virtual OrdemDeServico SequenciaDaOrdemDeServicoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaPropriedade")]
    [InverseProperty("Pedidos")]
    public virtual Propriedade SequenciaDaPropriedadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaTransportadora")]
    [InverseProperty("PedidoSequenciaDaTransportadoraNavigations")]
    public virtual Geral SequenciaDaTransportadoraNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("PedidoSequenciaDoGeralNavigations")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoOrcamento")]
    [InverseProperty("Pedidos")]
    public virtual Orcamento SequenciaDoOrcamentoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoVendedor")]
    [InverseProperty("PedidoSequenciaDoVendedorNavigations")]
    public virtual Geral SequenciaDoVendedorNavigation { get; set; } = null!;

    [InverseProperty("SequenciaDoPedidoNavigation")]
    public virtual ICollection<ServicoDoPedido> ServicosDoPedidos { get; set; } = new List<ServicoDoPedido>();
}
