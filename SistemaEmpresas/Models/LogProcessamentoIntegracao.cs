using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("LogProcessamentoIntegracao")]
[Index("Categoria", Name = "IX_LogIntegracao_Categoria")]
[Index("DataHora", Name = "IX_LogIntegracao_DataHora")]
[Index("Nivel", Name = "IX_LogIntegracao_Nivel")]
public partial class LogProcessamentoIntegracao
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataHora { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Nivel { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Categoria { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string Mensagem { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Detalhes { get; set; }

    [Column("SequenciaLancamentoBB")]
    public int? SequenciaLancamentoBb { get; set; }

    public int? SequenciaDaBaixa { get; set; }

    public int? SequenciaDaManutencao { get; set; }

    [Column(TypeName = "text")]
    public string? StackTrace { get; set; }
}
