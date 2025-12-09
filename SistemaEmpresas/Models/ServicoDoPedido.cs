using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoPedido", "SequenciaDoServicoPedido")]
[Table("Serviços do Pedido")]
public partial class ServicoDoPedido
{
    [Key]
    [Column("Seqüência do Pedido")]
    public int SequenciaDoPedido { get; set; }

    [Key]
    [Column("Seqüência do Serviço Pedido")]
    public int SequenciaDoServicoPedido { get; set; }

    [Column("Seqüência do Serviço")]
    public short SequenciaDoServico { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(11, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
    public decimal ValorTotal { get; set; }

    [ForeignKey("SequenciaDoPedido")]
    [InverseProperty("ServicosDoPedidos")]
    public virtual Pedido SequenciaDoPedidoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoServico")]
    [InverseProperty("ServicosDoPedidos")]
    public virtual Servico SequenciaDoServicoNavigation { get; set; } = null!;
}
