using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Adições da Declaração")]
public partial class AdicaoDaDeclaracao
{
    [Key]
    [Column("Seqüência da Adição")]
    public int SequenciaDaAdicao { get; set; }

    [Column("Seqüência da Declaração")]
    public int SequenciaDaDeclaracao { get; set; }

    [Column("Número da Adição")]
    public short NumeroDaAdicao { get; set; }

    [Column("Seqüêncial do Item da Adição")]
    public short SequencialDoItemDaAdicao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [ForeignKey("SequenciaDaDeclaracao")]
    [InverseProperty("AdicoesDaDeclaracaos")]
    public virtual DeclaracaoDeImportacao SequenciaDaDeclaracaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("AdicoesDaDeclaracaos")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;
}
