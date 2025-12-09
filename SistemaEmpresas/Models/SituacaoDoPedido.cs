using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Situação dos pedidos")]
public partial class SituacaoDoPedido
{
    [Key]
    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Data pedido", TypeName = "datetime")]
    public DateTime? DataPedido { get; set; }

    [Column("Prev entrega", TypeName = "datetime")]
    public DateTime? PrevEntrega { get; set; }

    [Column("Dias em atraso")]
    public short DiasEmAtraso { get; set; }

    [Column("Obs fabrica")]
    [StringLength(120)]
    [Unicode(false)]
    public string ObsFabrica { get; set; } = null!;

    [Column("Obs vendas")]
    [StringLength(120)]
    [Unicode(false)]
    public string ObsVendas { get; set; } = null!;

    [Column("Obs compras")]
    [StringLength(120)]
    [Unicode(false)]
    public string ObsCompras { get; set; } = null!;

    [Column("Obs almoxarifado")]
    [StringLength(120)]
    [Unicode(false)]
    public string ObsAlmoxarifado { get; set; } = null!;

    [Column("Desc material")]
    [StringLength(120)]
    [Unicode(false)]
    public string DescMaterial { get; set; } = null!;

    [StringLength(8)]
    [Unicode(false)]
    public string Status { get; set; } = null!;
}
