using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoOrcamento", "SequenciaDoItem")]
[Table("Itens pendentes")]
public partial class ItenPendente
{
    [Key]
    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(12, 4)")]
    public decimal ValorUnitario { get; set; }

    public short Tp { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Situacao { get; set; } = null!;

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }
}
