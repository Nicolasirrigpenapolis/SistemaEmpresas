using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdDoPedido", "SequenciaDoItem")]
[Table("Orçamentos da compra")]
public partial class OrcamentoDaCompra
{
    [Key]
    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }
}
