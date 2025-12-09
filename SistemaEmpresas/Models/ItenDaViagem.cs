using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SeqDaViagem", "SequenciaDoItem")]
[Table("Itens da Viagem")]
public partial class ItenDaViagem
{
    [Key]
    [Column("Seq da Viagem")]
    public int SeqDaViagem { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Descrição do Item")]
    [StringLength(120)]
    [Unicode(false)]
    public string DescricaoDoItem { get; set; } = null!;

    [Column("Valor do Item", TypeName = "decimal(12, 2)")]
    public decimal ValorDoItem { get; set; }
}
