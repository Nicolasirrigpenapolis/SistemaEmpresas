using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Agendamento de Backup")]
public partial class AgendamentoDeBackup
{
    [Key]
    [Column("Seqüência do Backup")]
    public int SequenciaDoBackup { get; set; }

    [Column("Tipo do Backup")]
    [StringLength(15)]
    [Unicode(false)]
    public string TipoDoBackup { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Hora { get; set; }

    public bool Segunda { get; set; }

    public bool Terca { get; set; }

    public bool Quarta { get; set; }

    public bool Quinta { get; set; }

    public bool Sexta { get; set; }

    public bool Sabado { get; set; }

    public bool Domingo { get; set; }

    public short Dia { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Destino { get; set; } = null!;
}
