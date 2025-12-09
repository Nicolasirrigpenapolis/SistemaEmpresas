using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Relatorio de Viagem")]
public partial class RelatorioDeViagem
{
    [Key]
    [Column("Seq da Viagem")]
    public int SeqDaViagem { get; set; }

    [Column("Sequencia do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Periodo Inicial", TypeName = "datetime")]
    public DateTime? PeriodoInicial { get; set; }

    [Column("Periodo Final", TypeName = "datetime")]
    public DateTime? PeriodoFinal { get; set; }

    [Column("Destino da Viagem")]
    [StringLength(255)]
    [Unicode(false)]
    public string DestinoDaViagem { get; set; } = null!;

    [Column("Motivo da Viagem", TypeName = "text")]
    public string MotivoDaViagem { get; set; } = null!;

    [Column("Total da Viagem", TypeName = "decimal(10, 2)")]
    public decimal TotalDaViagem { get; set; }

    [Column("Total dos Itens", TypeName = "decimal(10, 2)")]
    public decimal TotalDosItens { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Referencia { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string Teste { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("RelatorioDeViagems")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;
}
