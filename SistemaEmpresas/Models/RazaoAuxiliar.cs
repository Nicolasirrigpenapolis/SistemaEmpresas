using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Razão Auxiliar")]
public partial class RazaoAuxiliar
{
    [Key]
    [Column("Sequencia do Razão")]
    public int SequenciaDoRazao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Data do Razão", TypeName = "datetime")]
    public DateTime? DataDoRazao { get; set; }

    [Column("Historico do Razão", TypeName = "text")]
    public string HistoricoDoRazao { get; set; } = null!;

    [Column("Vr Entrada", TypeName = "decimal(10, 2)")]
    public decimal VrEntrada { get; set; }

    [Column("Vr Saida", TypeName = "decimal(10, 2)")]
    public decimal VrSaida { get; set; }
}
