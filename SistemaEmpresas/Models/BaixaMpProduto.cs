using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Baixa MP Produto")]
public partial class BaixaMpProduto
{
    [Key]
    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }

    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Column("Data da Baixa", TypeName = "datetime")]
    public DateTime? DataDaBaixa { get; set; }

    [Column("Hora da Baixa", TypeName = "datetime")]
    public DateTime? HoraDaBaixa { get; set; }

    [Column("Seqüência do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Quantidade do Produto", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeDoProduto { get; set; }

    [Column("Seqüência da Matéria Prima")]
    public int SequenciaDaMateriaPrima { get; set; }

    [Column("Quantidade da Matéria Prima", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeDaMateriaPrima { get; set; }

    [Column("Calcular Estoque")]
    public bool CalcularEstoque { get; set; }

    [ForeignKey("SequenciaDaMateriaPrima")]
    [InverseProperty("BaixaMpProdutoSequenciaDaMateriaPrimaNavigations")]
    public virtual Produto SequenciaDaMateriaPrimaNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("BaixaMpProdutoSequenciaDoProdutoNavigations")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
