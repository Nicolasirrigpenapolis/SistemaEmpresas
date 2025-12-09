using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Baixa Industrialização")]
public partial class BaixaIndustrializacao
{
    [Key]
    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }

    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Column("Seqüência do Item")]
    public short SequenciaDoItem { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("BaixaIndustrializacaos")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
