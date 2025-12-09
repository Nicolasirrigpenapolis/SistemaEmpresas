using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdDoPedido", "IdDoProduto", "SequenciaDoItem")]
[Table("Produtos do Pedido Compra")]
public partial class ProdutoDoPedidoCompra
{
    [Key]
    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Key]
    [Column("Id do Produto")]
    public int IdDoProduto { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Qtde { get; set; }

    [Column("Vr Unitario", TypeName = "decimal(11, 4)")]
    public decimal VrUnitario { get; set; }

    [Column("Vr Total", TypeName = "decimal(10, 2)")]
    public decimal VrTotal { get; set; }

    [Column("Aliquota do IPI", TypeName = "decimal(8, 4)")]
    public decimal AliquotaDoIpi { get; set; }

    [Column("Aliquota do Icms", TypeName = "decimal(7, 4)")]
    public decimal AliquotaDoIcms { get; set; }

    [Column("Vr do IPI", TypeName = "decimal(10, 2)")]
    public decimal VrDoIpi { get; set; }

    [Column("Vr do Icms", TypeName = "decimal(10, 2)")]
    public decimal VrDoIcms { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [ForeignKey("IdDoProduto")]
    [InverseProperty("ProdutosDoPedidoCompras")]
    public virtual Produto IdDoProdutoNavigation { get; set; } = null!;
}
