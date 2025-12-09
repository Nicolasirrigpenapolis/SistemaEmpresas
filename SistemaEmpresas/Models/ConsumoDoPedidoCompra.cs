using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdDoPedido", "IdDespesa")]
[Table("Consumo do Pedido Compra")]
public partial class ConsumoDoPedidoCompra
{
    [Key]
    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Key]
    [Column("Id Despesa")]
    public int IdDespesa { get; set; }

    [Column("Id da Despesa")]
    public int IdDaDespesa { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Qtde { get; set; }

    [Column("Vr Unitario", TypeName = "decimal(10, 2)")]
    public decimal VrUnitario { get; set; }

    [Column("Vr Total", TypeName = "decimal(10, 2)")]
    public decimal VrTotal { get; set; }

    [Column("Aliquota do IPI", TypeName = "decimal(8, 4)")]
    public decimal AliquotaDoIpi { get; set; }

    [Column("Aliquota do Icms")]
    public short AliquotaDoIcms { get; set; }

    [Column("Vr do IPI", TypeName = "decimal(10, 2)")]
    public decimal VrDoIpi { get; set; }

    [Column("Vr do Icms", TypeName = "decimal(10, 2)")]
    public decimal VrDoIcms { get; set; }

    [ForeignKey("IdDaDespesa")]
    [InverseProperty("ConsumoDoPedidoCompras")]
    public virtual Despesa IdDaDespesaNavigation { get; set; } = null!;
}
