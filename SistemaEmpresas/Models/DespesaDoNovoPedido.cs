using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("CodigoDoPedido", "SequenciaDoItem")]
[Table("Despesas do Novo Pedido")]
public partial class DespesaDoNovoPedido
{
    [Key]
    [Column("Codigo do Pedido")]
    public int CodigoDoPedido { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Sequencia da Despesa")]
    public int SequenciaDaDespesa { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(12, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Valor do IPI", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIpi { get; set; }

    [Column("Valor do ICMS", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIcms { get; set; }

    [Column("Alíquota do IPI", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIpi { get; set; }

    [Column("Alíquota do ICMS", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIcms { get; set; }

    [ForeignKey("SequenciaDaDespesa")]
    [InverseProperty("DespesasDoNovoPedidos")]
    public virtual Despesa SequenciaDaDespesaNavigation { get; set; } = null!;
}
