using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoConjunto", "SequenciaDoProduto")]
[Table("Itens do Conjunto")]
public partial class ItenDoConjunto
{
    [Key]
    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [Key]
    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Quantidade do Produto", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeDoProduto { get; set; }

    [Column("Peso do Item", TypeName = "decimal(12, 3)")]
    public decimal PesoDoItem { get; set; }

    [Column("Peso Total", TypeName = "decimal(12, 3)")]
    public decimal PesoTotal { get; set; }

    [ForeignKey("SequenciaDoConjunto")]
    [InverseProperty("ItensDoConjuntos")]
    public virtual Conjunto SequenciaDoConjuntoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("ItensDoConjuntos")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
