using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoProjeto", "SequenciaDoItem")]
[Table("Mapa da Vazao")]
public partial class MapaDaVazao
{
    [Key]
    [Column("Sequencia do Projeto")]
    public int SequenciaDoProjeto { get; set; }

    [Column("Distancia ao Centro", TypeName = "decimal(8, 2)")]
    public decimal DistanciaAoCentro { get; set; }

    [Column("Vazao na Saida", TypeName = "decimal(8, 4)")]
    public decimal VazaoNaSaida { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Vazao Acumulada", TypeName = "decimal(8, 4)")]
    public decimal VazaoAcumulada { get; set; }

    [Column("Vazao Trecho", TypeName = "decimal(8, 3)")]
    public decimal VazaoTrecho { get; set; }

    [Column("DN", TypeName = "decimal(8, 2)")]
    public decimal Dn { get; set; }

    [Column("Perda Carga", TypeName = "decimal(8, 4)")]
    public decimal PerdaCarga { get; set; }

    [Column("Velocidade Trecho", TypeName = "decimal(8, 3)")]
    public decimal VelocidadeTrecho { get; set; }
}
