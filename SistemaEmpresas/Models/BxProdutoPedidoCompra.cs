using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdDoPedido", "IdDoProduto", "SequenciaDoItem")]
[Table("Bx Produtos Pedido Compra")]
public partial class BxProdutoPedidoCompra
{
    [Key]
    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Key]
    [Column("Id do Produto")]
    public int IdDoProduto { get; set; }

    [Column("Qtde Total", TypeName = "decimal(10, 2)")]
    public decimal QtdeTotal { get; set; }

    [Column("Vr Unitario", TypeName = "decimal(11, 4)")]
    public decimal VrUnitario { get; set; }

    [Column("Vr Total do Pedido", TypeName = "decimal(10, 2)")]
    public decimal VrTotalDoPedido { get; set; }

    [Column("Qtde Recebida", TypeName = "decimal(10, 2)")]
    public decimal QtdeRecebida { get; set; }

    [Column("Qtde Restante", TypeName = "decimal(10, 2)")]
    public decimal QtdeRestante { get; set; }

    [Column("Total Restante", TypeName = "decimal(10, 2)")]
    public decimal TotalRestante { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Notas { get; set; } = null!;

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Teste { get; set; } = null!;
}
