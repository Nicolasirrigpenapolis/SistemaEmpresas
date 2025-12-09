using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoProjeto", "SequenciaDoItem")]
[Table("Serviços do Projeto")]
public partial class ServicoDoProjeto
{
    [Key]
    [Column("Sequencia do Projeto")]
    public int SequenciaDoProjeto { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Seqüência do Serviço")]
    public short SequenciaDoServico { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitario", TypeName = "decimal(12, 2)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Parte do Pivo")]
    [StringLength(29)]
    [Unicode(false)]
    public string ParteDoPivo { get; set; } = null!;

    [Column("Valor Anterior", TypeName = "decimal(12, 2)")]
    public decimal ValorAnterior { get; set; }
}
