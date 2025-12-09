using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaMateriaPrima", "SequenciaDoProduto")]
[Table("Matéria Prima")]
public partial class MateriaPrima
{
    [Key]
    [Column("Seqüência da Matéria Prima")]
    public int SequenciaDaMateriaPrima { get; set; }

    [Key]
    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Quantidade de Matéria Prima", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeDeMateriaPrima { get; set; }

    [ForeignKey("SequenciaDaMateriaPrima")]
    [InverseProperty("MateriaPrimaSequenciaDaMateriaPrimaNavigations")]
    public virtual Produto SequenciaDaMateriaPrimaNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("MateriaPrimaSequenciaDoProdutoNavigations")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
