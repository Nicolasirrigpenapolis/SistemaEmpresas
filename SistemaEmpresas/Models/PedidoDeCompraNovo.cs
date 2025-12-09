using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Pedido de Compra Novo")]
public partial class PedidoDeCompraNovo
{
    [Key]
    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Column("Data do Pedido", TypeName = "datetime")]
    public DateTime DataDoPedido { get; set; }

    [Column("Nro da Licitação")]
    [StringLength(9)]
    [Unicode(false)]
    public string NroDaLicitacao { get; set; } = null!;

    [Column("Codigo do Fornecedor")]
    public int CodigoDoFornecedor { get; set; }

    [Column("Codigo da Transportadora")]
    public int CodigoDaTransportadora { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Comprador { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Vend { get; set; } = null!;

    [StringLength(40)]
    [Unicode(false)]
    public string Prazo { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string CifFob { get; set; } = null!;

    [Column("Vr do Frete", TypeName = "decimal(10, 2)")]
    public decimal VrDoFrete { get; set; }

    [Column("Vr do Desconto", TypeName = "decimal(10, 2)")]
    public decimal VrDoDesconto { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    [Column("Total dos Produtos", TypeName = "decimal(10, 2)")]
    public decimal TotalDosProdutos { get; set; }

    [Column("Total das Despesas", TypeName = "decimal(10, 2)")]
    public decimal TotalDasDespesas { get; set; }

    [Column("Total do IPI", TypeName = "decimal(10, 2)")]
    public decimal TotalDoIpi { get; set; }

    [Column("Total do ICMS", TypeName = "decimal(10, 2)")]
    public decimal TotalDoIcms { get; set; }

    [Column("Total do Pedido", TypeName = "decimal(10, 2)")]
    public decimal TotalDoPedido { get; set; }

    [Column("Endereco de Entrega")]
    [StringLength(50)]
    [Unicode(false)]
    public string EnderecoDeEntrega { get; set; } = null!;

    [Column("Numero do Endereco")]
    [StringLength(9)]
    [Unicode(false)]
    public string NumeroDoEndereco { get; set; } = null!;

    [Column("Bairro de Entrega")]
    [StringLength(50)]
    [Unicode(false)]
    public string BairroDeEntrega { get; set; } = null!;

    [Column("Cidade de Entrega")]
    [StringLength(50)]
    [Unicode(false)]
    public string CidadeDeEntrega { get; set; } = null!;

    [Column("UF De Entrega")]
    [StringLength(3)]
    [Unicode(false)]
    public string UfDeEntrega { get; set; } = null!;

    [Column("CEP de Entrega")]
    [StringLength(9)]
    [Unicode(false)]
    public string CepDeEntrega { get; set; } = null!;

    [Column("Fone de Entrega")]
    [StringLength(20)]
    [Unicode(false)]
    public string FoneDeEntrega { get; set; } = null!;

    [Column("Contato de Entrega")]
    [StringLength(45)]
    [Unicode(false)]
    public string ContatoDeEntrega { get; set; } = null!;

    [Column("Previsao de Entrega", TypeName = "datetime")]
    public DateTime? PrevisaoDeEntrega { get; set; }

    [Column("Pedido Fechado")]
    public bool PedidoFechado { get; set; }

    public bool Validado { get; set; }

    public bool Cancelado { get; set; }

    [Column("Codigo da Licitação")]
    public int CodigoDaLicitacao { get; set; }

    [Column("Nome do Banco 1")]
    [StringLength(20)]
    [Unicode(false)]
    public string NomeDoBanco1 { get; set; } = null!;

    [Column("Agência do Banco 1")]
    [StringLength(20)]
    [Unicode(false)]
    public string AgenciaDoBanco1 { get; set; } = null!;

    [Column("Conta Corrente do Banco 1")]
    [StringLength(15)]
    [Unicode(false)]
    public string ContaCorrenteDoBanco1 { get; set; } = null!;

    [Column("Nome do Correntista do Banco 1")]
    [StringLength(60)]
    [Unicode(false)]
    public string NomeDoCorrentistaDoBanco1 { get; set; } = null!;

    public bool Prepedido { get; set; }

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [Column("Justificar o Atraso")]
    [StringLength(150)]
    [Unicode(false)]
    public string JustificarOAtraso { get; set; } = null!;

    [Column("Nova Previsao", TypeName = "datetime")]
    public DateTime? NovaPrevisao { get; set; }

    public short Dias { get; set; }

    [InverseProperty("IdDoPedidoNavigation")]
    public virtual ICollection<VinculaPedidoOrcamento> VinculaPedidoOrcamentos { get; set; } = new List<VinculaPedidoOrcamento>();
}
