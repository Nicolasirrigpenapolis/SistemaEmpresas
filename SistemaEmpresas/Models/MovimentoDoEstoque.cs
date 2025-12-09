using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Movimento do Estoque")]
public partial class MovimentoDoEstoque
{
    [Key]
    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [Column("Data da Compra", TypeName = "datetime")]
    public DateTime? DataDaCompra { get; set; }

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column(TypeName = "text")]
    public string Observacao { get; set; } = null!;

    [Column("Valor Total dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosProdutos { get; set; }

    [Column("Valor Total dos Conjuntos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosConjuntos { get; set; }

    [Column("Processar Custo")]
    public bool ProcessarCusto { get; set; }

    [Column("Data de Entrada", TypeName = "datetime")]
    public DateTime? DataDeEntrada { get; set; }

    [Column("Valor Total IPI dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDosProdutos { get; set; }

    [Column("Valor Total IPI dos Conjuntos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDosConjuntos { get; set; }

    [Column("Tipo do Movimento")]
    public short TipoDoMovimento { get; set; }

    [Column("Valor Total do Movimento", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoMovimento { get; set; }

    [Column("Valor Total do ICMS ST", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoIcmsSt { get; set; }

    [Column("Valor do Fechamento", TypeName = "decimal(11, 2)")]
    public decimal ValorDoFechamento { get; set; }

    public short Fechamento { get; set; }

    [Column("Valor do Seguro", TypeName = "decimal(11, 2)")]
    public decimal ValorDoSeguro { get; set; }

    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("Tipo Movimento")]
    public short TipoMovimento { get; set; }

    [Column("Valor Total IPI das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDasPecas { get; set; }

    [Column("Valor Total das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecas { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    [Column("Valor Total da Base de Cálculo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaBaseDeCalculo { get; set; }

    [Column("Valor Total do ICMS", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoIcms { get; set; }

    [Column("Movimento Cancelado")]
    public bool MovimentoCancelado { get; set; }

    [Column("Não Totaliza")]
    public bool NaoTotaliza { get; set; }

    [Column("Data da Alteração", TypeName = "datetime")]
    public DateTime? DataDaAlteracao { get; set; }

    [Column("Hora da Alteração", TypeName = "datetime")]
    public DateTime? HoraDaAlteracao { get; set; }

    [Column("Usuário da Alteração")]
    [StringLength(60)]
    [Unicode(false)]
    public string UsuarioDaAlteracao { get; set; } = null!;

    [Column("Valor Total IPI das Despesas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDasDespesas { get; set; }

    [Column("Valor Total das Despesas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasDespesas { get; set; }

    public bool Industrializacao { get; set; }

    [Column("Outras Despesas", TypeName = "decimal(10, 2)")]
    public decimal OutrasDespesas { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    [Column("Nao_Alterar")]
    public bool NaoAlterar { get; set; }

    [Column("Nota de venda")]
    public int NotaDeVenda { get; set; }

    [InverseProperty("SequenciaDoMovimentoNavigation")]
    public virtual ICollection<ConjuntoDoMovimentoEstoque> ConjuntosDoMovimentoEstoques { get; set; } = new List<ConjuntoDoMovimentoEstoque>();

    [InverseProperty("SequenciaDoMovimentoNavigation")]
    public virtual ICollection<DespesaDoMovimentoEstoque> DespesasDoMovimentoEstoques { get; set; } = new List<DespesaDoMovimentoEstoque>();

    [InverseProperty("SequenciaDoMovimentoNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDoMovimentoNavigation")]
    public virtual ICollection<ParcelaMovimentoEstoque> ParcelasMovimentoEstoques { get; set; } = new List<ParcelaMovimentoEstoque>();

    [InverseProperty("SequenciaDoMovimentoNavigation")]
    public virtual ICollection<ProdutoDoMovimentoEstoque> ProdutosDoMovimentoEstoques { get; set; } = new List<ProdutoDoMovimentoEstoque>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("MovimentoDoEstoques")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaPropriedade")]
    [InverseProperty("MovimentoDoEstoques")]
    public virtual Propriedade SequenciaDaPropriedadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("MovimentoDoEstoques")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaGrupoDespesa")]
    [InverseProperty("MovimentoDoEstoques")]
    public virtual GrupoDaDespesa SequenciaGrupoDespesaNavigation { get; set; } = null!;

    [ForeignKey("SeqüênciaSubGrupoDespesa, SeqüênciaGrupoDespesa")]
    [InverseProperty("MovimentoDoEstoques")]
    public virtual SubGrupoDespesa SubGrupoDespesa { get; set; } = null!;
}
