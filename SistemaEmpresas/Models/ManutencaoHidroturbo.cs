using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SeqDoHidroturbo", "SequenciaDoItem")]
[Table("Manutenção Hidroturbo")]
public partial class ManutencaoHidroturbo
{
    [Key]
    [Column("Seq do Hidroturbo")]
    public int SeqDoHidroturbo { get; set; }

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
