using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("VinculaPedidoOrcamento")]
public partial class VinculaPedidoOrcamento
{
    [Key]
    [Column("ID_Vinculacao")]
    public int IdVinculacao { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Qtde { get; set; }

    [ForeignKey("IdDoPedido")]
    [InverseProperty("VinculaPedidoOrcamentos")]
    public virtual PedidoDeCompraNovo IdDoPedidoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("VinculaPedidoOrcamentos")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoOrcamento")]
    [InverseProperty("VinculaPedidoOrcamentos")]
    public virtual Orcamento SequenciaDoOrcamentoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("VinculaPedidoOrcamentos")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
