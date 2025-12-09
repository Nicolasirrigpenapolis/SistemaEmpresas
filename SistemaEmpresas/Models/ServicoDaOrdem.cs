using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdDaOrdem", "SequenciaDoItem")]
[Table("Serviços da Ordem")]
public partial class ServicoDaOrdem
{
    [Key]
    [Column("Id da Ordem")]
    public int IdDaOrdem { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Sequencia do Serviço")]
    public int SequenciaDoServico { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitario", TypeName = "decimal(12, 2)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor do Iss", TypeName = "decimal(12, 2)")]
    public decimal ValorDoIss { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }
}
