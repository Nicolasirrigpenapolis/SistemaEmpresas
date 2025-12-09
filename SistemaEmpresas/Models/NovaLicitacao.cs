using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Nova Licitação")]
public partial class NovaLicitacao
{
    [Key]
    [Column("Codigo da Licitação")]
    public int CodigoDaLicitacao { get; set; }

    [Column("Dt da Licitação", TypeName = "datetime")]
    public DateTime? DtDaLicitacao { get; set; }

    [StringLength(45)]
    [Unicode(false)]
    public string Contato { get; set; } = null!;

    [Column("Prev de Entrega", TypeName = "datetime")]
    public DateTime? PrevDeEntrega { get; set; }

    [Column("Sequencia do Fornecedor")]
    public int SequenciaDoFornecedor { get; set; }

    [Column("Sequencia da Transportadora")]
    public int SequenciaDaTransportadora { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Comprador { get; set; } = null!;

    [Column("Tipo da Licitação")]
    [StringLength(25)]
    [Unicode(false)]
    public string TipoDaLicitacao { get; set; } = null!;

    [Column("Tipo de Frete")]
    [StringLength(3)]
    [Unicode(false)]
    public string TipoDeFrete { get; set; } = null!;

    [Column("Totalizar Frete")]
    public bool TotalizarFrete { get; set; }

    [Column("Nome do Vendedor")]
    [StringLength(30)]
    [Unicode(false)]
    public string NomeDoVendedor { get; set; } = null!;

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Observacoes { get; set; } = null!;

    [Column("Codigo do Pedido")]
    public int CodigoDoPedido { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column("Total dos Produtos", TypeName = "decimal(10, 2)")]
    public decimal TotalDosProdutos { get; set; }

    [Column("Total de Icms", TypeName = "decimal(10, 2)")]
    public decimal TotalDeIcms { get; set; }

    [Column("Total de Icms St", TypeName = "decimal(10, 2)")]
    public decimal TotalDeIcmsSt { get; set; }

    [Column("Total de Ipi", TypeName = "decimal(10, 2)")]
    public decimal TotalDeIpi { get; set; }

    [Column("Total de Despesas", TypeName = "decimal(10, 2)")]
    public decimal TotalDeDespesas { get; set; }

    [Column("Total da Licitação", TypeName = "decimal(10, 2)")]
    public decimal TotalDaLicitacao { get; set; }

    public bool Cancelado { get; set; }

    public bool Fechado { get; set; }

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    public short Dias { get; set; }

    [Column("IPI na Bc Frete")]
    public bool IpiNaBcFrete { get; set; }

    [ForeignKey("SequenciaDaTransportadora")]
    [InverseProperty("NovaLicitacaoSequenciaDaTransportadoraNavigations")]
    public virtual Geral SequenciaDaTransportadoraNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoFornecedor")]
    [InverseProperty("NovaLicitacaoSequenciaDoFornecedorNavigations")]
    public virtual Geral SequenciaDoFornecedorNavigation { get; set; } = null!;
}
