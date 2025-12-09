using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Conjunto
{
    [Key]
    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Detalhes { get; set; } = null!;

    [Column("Quantidade no Estoque", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeNoEstoque { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Seqüência do Grupo Produto")]
    public short SequenciaDoGrupoProduto { get; set; }

    [Column("Seqüência do SubGrupo Produto")]
    public short SequenciaDoSubGrupoProduto { get; set; }

    [Column("Seqüência da Unidade")]
    public short SequenciaDaUnidade { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    public bool Usado { get; set; }

    [Column("Último Movimento", TypeName = "datetime")]
    public DateTime? UltimoMovimento { get; set; }

    [Column("Localização")]
    [StringLength(50)]
    [Unicode(false)]
    public string Localizacao { get; set; } = null!;

    [Column("Quantidade Contábil", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeContabil { get; set; }

    [Column("Valor Contábil Atual", TypeName = "decimal(13, 4)")]
    public decimal ValorContabilAtual { get; set; }

    public bool Inativo { get; set; }

    [Column("Quantidade Mínima", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeMinima { get; set; }

    [Column("Última Entrada", TypeName = "datetime")]
    public DateTime? UltimaEntrada { get; set; }

    [Column("Altura do Conjunto")]
    [StringLength(10)]
    [Unicode(false)]
    public string AlturaDoConjunto { get; set; } = null!;

    [Column("Largura do Conjunto")]
    [StringLength(10)]
    [Unicode(false)]
    public string LarguraDoConjunto { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Comprimento { get; set; } = null!;

    [Column("Peso do Conjunto", TypeName = "decimal(12, 3)")]
    public decimal PesoDoConjunto { get; set; }

    [Column("Parte do Pivo")]
    [StringLength(29)]
    [Unicode(false)]
    public string ParteDoPivo { get; set; } = null!;

    [Column("Trava receita")]
    public bool TravaReceita { get; set; }

    [Column("Receita Conferida")]
    public bool ReceitaConferida { get; set; }

    [Column("Margem de Lucro", TypeName = "decimal(11, 4)")]
    public decimal MargemDeLucro { get; set; }

    [Column("Valor Total Anterior", TypeName = "decimal(12, 4)")]
    public decimal? ValorTotalAnterior { get; set; }

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<BaixaDoEstoqueContabil> BaixaDoEstoqueContabils { get; set; } = new List<BaixaDoEstoqueContabil>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ConjuntoDaNotaFiscal> ConjuntosDaNotaFiscals { get; set; } = new List<ConjuntoDaNotaFiscal>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ConjuntoDaOrdemDeServico> ConjuntosDaOrdemDeServicos { get; set; } = new List<ConjuntoDaOrdemDeServico>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ConjuntoDoMovimentoEstoque> ConjuntosDoMovimentoEstoques { get; set; } = new List<ConjuntoDoMovimentoEstoque>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ConjuntoDoOrcamento> ConjuntosDoOrcamentos { get; set; } = new List<ConjuntoDoOrcamento>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ConjuntoDoPedido> ConjuntosDoPedidos { get; set; } = new List<ConjuntoDoPedido>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ConjuntoMovimentoContabil> ConjuntosMovimentoContabils { get; set; } = new List<ConjuntoMovimentoContabil>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ConjuntoMvtoContabilNovo> ConjuntosMvtoContabilNovos { get; set; } = new List<ConjuntoMvtoContabilNovo>();

    [InverseProperty("SequenciaDoConjuntoNavigation")]
    public virtual ICollection<ItenDoConjunto> ItensDoConjuntos { get; set; } = new List<ItenDoConjunto>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("Conjuntos")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaUnidade")]
    [InverseProperty("Conjuntos")]
    public virtual Unidade SequenciaDaUnidadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGrupoProduto")]
    [InverseProperty("Conjuntos")]
    public virtual GrupoDoProduto SequenciaDoGrupoProdutoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoSubGrupoProduto, SequenciaDoGrupoProduto")]
    [InverseProperty("Conjuntos")]
    public virtual SubGrupoDoProduto SubGrupoDoProduto { get; set; } = null!;
}
