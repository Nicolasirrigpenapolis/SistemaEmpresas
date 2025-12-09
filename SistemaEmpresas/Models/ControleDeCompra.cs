using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Controle de Compras")]
public partial class ControleDeCompra
{
    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Data do Pedido", TypeName = "datetime")]
    public DateTime? DataDoPedido { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Comprador { get; set; } = null!;

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Vr_Unit_Ipi", TypeName = "decimal(11, 4)")]
    public decimal VrUnitIpi { get; set; }

    [Column("Qtde_Total", TypeName = "decimal(10, 2)")]
    public decimal QtdeTotal { get; set; }

    [Column("Qtde_Recebida", TypeName = "decimal(10, 2)")]
    public decimal QtdeRecebida { get; set; }

    [Column("Qtde_Restante", TypeName = "decimal(10, 2)")]
    public decimal QtdeRestante { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string Prazo { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Financeiro { get; set; } = null!;

    [Column("Data da Baixa", TypeName = "datetime")]
    public DateTime? DataDaBaixa { get; set; }

    public short Dias { get; set; }

    [Column("Codigo do Fornecedor")]
    public int CodigoDoFornecedor { get; set; }

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [Column("Prev entrega", TypeName = "datetime")]
    public DateTime? PrevEntrega { get; set; }
}
