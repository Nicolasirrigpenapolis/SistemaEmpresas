using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoOrcamento", "SequenciaDoItem")]
[Table("Receita primaria")]
public partial class ReceitaPrimarium
{
    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Seqüência da Matéria Prima")]
    public int SequenciaDaMateriaPrima { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Key]
    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Situacao { get; set; } = null!;

    [StringLength(120)]
    [Unicode(false)]
    public string Pedidos { get; set; } = null!;

    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string Pagto { get; set; } = null!;

    [Column("Qtde Recebida", TypeName = "decimal(10, 2)")]
    public decimal QtdeRecebida { get; set; }

    [Column("Qtde Restante", TypeName = "decimal(10, 2)")]
    public decimal QtdeRestante { get; set; }

    [Column("Qtde Total", TypeName = "decimal(10, 2)")]
    public decimal QtdeTotal { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Localizacao { get; set; } = null!;

    [Column("Sequencia produto principal")]
    public int SequenciaProdutoPrincipal { get; set; }

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [Column("Qt Separada", TypeName = "decimal(11, 4)")]
    public decimal QtSeparada { get; set; }
}
