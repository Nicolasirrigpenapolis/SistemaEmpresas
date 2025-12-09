using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SeqDoPivo", "SequenciaDoItem")]
[Table("Manutenção Pivo")]
public partial class ManutencaoPivo
{
    [Key]
    [Column("Seq do Pivo")]
    public int SeqDoPivo { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Data da Manutenção", TypeName = "datetime")]
    public DateTime? DataDaManutencao { get; set; }

    [Column("Descrição da Manutenção")]
    [StringLength(120)]
    [Unicode(false)]
    public string DescricaoDaManutencao { get; set; } = null!;
}
