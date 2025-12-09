using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Produto
{
    [Key]
    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Seqüência do Grupo Produto")]
    public short SequenciaDoGrupoProduto { get; set; }

    [Column("Seqüência do SubGrupo Produto")]
    public short SequenciaDoSubGrupoProduto { get; set; }

    [Column("Última Compra", TypeName = "datetime")]
    public DateTime? UltimaCompra { get; set; }

    [Column("Quantidade no Estoque", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeNoEstoque { get; set; }

    [Column("Quantidade Mínima", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeMinima { get; set; }

    [Column("Último Movimento", TypeName = "datetime")]
    public DateTime? UltimoMovimento { get; set; }

    [Column("Valor de Custo", TypeName = "decimal(12, 4)")]
    public decimal ValorDeCusto { get; set; }

    [Column("Margem de Lucro", TypeName = "decimal(11, 4)")]
    public decimal MargemDeLucro { get; set; }

    [Column("Seqüência da Unidade")]
    public short SequenciaDaUnidade { get; set; }

    [Column("Código de Barras")]
    [StringLength(13)]
    [Unicode(false)]
    public string CodigoDeBarras { get; set; } = null!;

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("É Matéria Prima")]
    public bool EMateriaPrima { get; set; }

    [Column("Custo Médio", TypeName = "decimal(11, 2)")]
    public decimal CustoMedio { get; set; }

    public bool Usado { get; set; }

    [Column("Último Fornecedor")]
    public int UltimoFornecedor { get; set; }

    [Column("Tipo do Produto")]
    public short TipoDoProduto { get; set; }

    public bool Inativo { get; set; }

    [Column("Localização")]
    [StringLength(50)]
    [Unicode(false)]
    public string Localizacao { get; set; } = null!;

    [Column("Quantidade Contábil", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeContabil { get; set; }

    [Column("Valor Contábil Atual", TypeName = "decimal(13, 4)")]
    public decimal ValorContabilAtual { get; set; }

    [Column("Material Adquirido de Terceiro")]
    public bool MaterialAdquiridoDeTerceiro { get; set; }

    [Column("Valor Atualizado")]
    public bool ValorAtualizado { get; set; }

    [Column("Valor Anterior", TypeName = "decimal(12, 2)")]
    public decimal ValorAnterior { get; set; }

    public bool Sucata { get; set; }

    [Column(TypeName = "decimal(12, 3)")]
    public decimal Peso { get; set; }

    [Column("Industrialização")]
    public bool Industrializacao { get; set; }

    public bool Importado { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Medida { get; set; } = null!;

    [Column("Valor de Lista", TypeName = "decimal(12, 2)")]
    public decimal ValorDeLista { get; set; }

    [Column("Usuário da Alteração")]
    [StringLength(60)]
    [Unicode(false)]
    public string UsuarioDaAlteracao { get; set; } = null!;

    [Column("Data da Alteração", TypeName = "datetime")]
    public DateTime? DataDaAlteracao { get; set; }

    [Column("Hora da Alteração", TypeName = "datetime")]
    public DateTime? HoraDaAlteracao { get; set; }

    [Column("Modelo do Lance")]
    public int ModeloDoLance { get; set; }

    [Column("Usado no Projeto")]
    public bool UsadoNoProjeto { get; set; }

    [Column("Parte do Pivo")]
    [StringLength(29)]
    [Unicode(false)]
    public string ParteDoPivo { get; set; } = null!;

    [Column("Quantidade Fisica", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeFisica { get; set; }

    [Column("Data da Contagem", TypeName = "datetime")]
    public DateTime? DataDaContagem { get; set; }

    [Column("Não Sair no Relatório")]
    public bool NaoSairNoRelatorio { get; set; }

    [Column("Mostrar Receita Secundaria")]
    public bool MostrarReceitaSecundaria { get; set; }

    [Column("Nao Mostrar Receita")]
    public bool NaoMostrarReceita { get; set; }

    [Column("Nao sair no checklist")]
    public bool NaoSairNoChecklist { get; set; }

    [Column("Trava receita")]
    public bool TravaReceita { get; set; }

    public bool Lance { get; set; }

    [Column("Mp inicial")]
    public bool MpInicial { get; set; }

    [Column("Qtde Inicial", TypeName = "decimal(10, 2)")]
    public decimal QtdeInicial { get; set; }

    [Column("E Regulador")]
    public bool ERegulador { get; set; }

    [Column("Separado Montar", TypeName = "decimal(11, 4)")]
    public decimal SeparadoMontar { get; set; }

    [Column("Comprados Aguardando", TypeName = "decimal(11, 4)")]
    public decimal CompradosAguardando { get; set; }

    [Column("Conferido pelo Contabil")]
    public bool ConferidoPeloContabil { get; set; }

    public bool Obsoleto { get; set; }

    public bool Marcar { get; set; }

    [Column("Ultima Cotação", TypeName = "datetime")]
    public DateTime? UltimaCotacao { get; set; }

    [Column("Medida Final")]
    [StringLength(20)]
    [Unicode(false)]
    public string MedidaFinal { get; set; } = null!;

    [Column("Receita Conferida")]
    public bool ReceitaConferida { get; set; }

    [Column(TypeName = "text")]
    public string Detalhes { get; set; } = null!;

    [Column("Quantidade Balanço", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeBalanco { get; set; }

    [Column("Peso Ok")]
    public bool PesoOk { get; set; }

    [Column("Valor de Custo Anterior", TypeName = "decimal(12, 4)")]
    public decimal? ValorDeCustoAnterior { get; set; }

    [Column("Valor Total Anterior", TypeName = "decimal(12, 4)")]
    public decimal? ValorTotalAnterior { get; set; }

    [Column("Margem de Lucro Anterior", TypeName = "decimal(12, 4)")]
    public decimal? MargemDeLucroAnterior { get; set; }

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<BaixaDoEstoqueContabil> BaixaDoEstoqueContabils { get; set; } = new List<BaixaDoEstoqueContabil>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<BaixaIndustrializacao> BaixaIndustrializacaos { get; set; } = new List<BaixaIndustrializacao>();

    [InverseProperty("SequenciaDaMateriaPrimaNavigation")]
    public virtual ICollection<BaixaMpProduto> BaixaMpProdutoSequenciaDaMateriaPrimaNavigations { get; set; } = new List<BaixaMpProduto>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<BaixaMpProduto> BaixaMpProdutoSequenciaDoProdutoNavigations { get; set; } = new List<BaixaMpProduto>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ImportacaoProdutoEstoque> ImportacaoProdutosEstoques { get; set; } = new List<ImportacaoProdutoEstoque>();

    [InverseProperty("ProdutoNavigation")]
    public virtual ICollection<ItenDaLicitacao> ItensDaLicitacaos { get; set; } = new List<ItenDaLicitacao>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ItenDoConjunto> ItensDoConjuntos { get; set; } = new List<ItenDoConjunto>();

    [InverseProperty("SequenciaDaMateriaPrimaNavigation")]
    public virtual ICollection<MateriaPrima> MateriaPrimaSequenciaDaMateriaPrimaNavigations { get; set; } = new List<MateriaPrima>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<MateriaPrima> MateriaPrimaSequenciaDoProdutoNavigations { get; set; } = new List<MateriaPrima>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<PecaDaNotaFiscal> PecasDaNotaFiscals { get; set; } = new List<PecaDaNotaFiscal>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<PecaDaOrdemDeServico> PecasDaOrdemDeServicos { get; set; } = new List<PecaDaOrdemDeServico>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<PecaDoMovimentoEstoque> PecasDoMovimentoEstoques { get; set; } = new List<PecaDoMovimentoEstoque>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<PecaDoOrcamento> PecasDoOrcamentos { get; set; } = new List<PecaDoOrcamento>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<PecaDoPedido> PecasDoPedidos { get; set; } = new List<PecaDoPedido>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDaLicitacao> ProdutosDaLicitacaos { get; set; } = new List<ProdutoDaLicitacao>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDaNotaFiscal> ProdutosDaNotaFiscals { get; set; } = new List<ProdutoDaNotaFiscal>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDaOrdemDeServico> ProdutosDaOrdemDeServicos { get; set; } = new List<ProdutoDaOrdemDeServico>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDoMovimentoContabil> ProdutosDoMovimentoContabils { get; set; } = new List<ProdutoDoMovimentoContabil>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDoMovimentoEstoque> ProdutosDoMovimentoEstoques { get; set; } = new List<ProdutoDoMovimentoEstoque>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDoNovoPedido> ProdutosDoNovoPedidos { get; set; } = new List<ProdutoDoNovoPedido>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDoOrcamento> ProdutosDoOrcamentos { get; set; } = new List<ProdutoDoOrcamento>();

    [InverseProperty("IdDoProdutoNavigation")]
    public virtual ICollection<ProdutoDoPedidoCompra> ProdutosDoPedidoCompras { get; set; } = new List<ProdutoDoPedidoCompra>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoDoPedido> ProdutosDoPedidos { get; set; } = new List<ProdutoDoPedido>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<ProdutoMvtoContabilNovo> ProdutosMvtoContabilNovos { get; set; } = new List<ProdutoMvtoContabilNovo>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("Produtos")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaUnidade")]
    [InverseProperty("Produtos")]
    public virtual Unidade SequenciaDaUnidadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGrupoProduto")]
    [InverseProperty("Produtos")]
    public virtual GrupoDoProduto SequenciaDoGrupoProdutoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoSubGrupoProduto, SequenciaDoGrupoProduto")]
    [InverseProperty("Produtos")]
    public virtual SubGrupoDoProduto SubGrupoDoProduto { get; set; } = null!;

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<TransferenciaDeReceitum> TransferenciaDeReceita { get; set; } = new List<TransferenciaDeReceitum>();

    [InverseProperty("SequenciaDoProdutoNavigation")]
    public virtual ICollection<VinculaPedidoOrcamento> VinculaPedidoOrcamentos { get; set; } = new List<VinculaPedidoOrcamento>();
}
