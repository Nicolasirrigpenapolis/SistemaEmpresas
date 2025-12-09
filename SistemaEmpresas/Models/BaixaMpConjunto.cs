using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Baixa MP Conjunto")]
public partial class BaixaMpConjunto
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
    public short SequenciaDoItem { get; set; }

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [Column("Quantidade do Conjunto", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeDoConjunto { get; set; }

    [Column("Seqüência da Matéria Prima")]
    public int SequenciaDaMateriaPrima { get; set; }

    [Column("Quantidade da Matéria Prima", TypeName = "decimal(9, 3)")]
    public decimal QuantidadeDaMateriaPrima { get; set; }

    [Column("Calcular Estoque")]
    public bool CalcularEstoque { get; set; }
}
