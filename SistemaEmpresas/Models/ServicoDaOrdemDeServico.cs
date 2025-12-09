using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaOrdemDeServico", "SequenciaServicoOs")]
[Table("Serviços da Ordem de Serviço")]
public partial class ServicoDaOrdemDeServico
{
    [Key]
    [Column("Seqüência da Ordem de Serviço")]
    public int SequenciaDaOrdemDeServico { get; set; }

    [Key]
    [Column("Seqüência Serviço OS")]
    public int SequenciaServicoOs { get; set; }

    [Column("Seqüência do Serviço")]
    public short SequenciaDoServico { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal Horas { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(11, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
    public decimal ValorTotal { get; set; }

    [ForeignKey("SequenciaDaOrdemDeServico")]
    [InverseProperty("ServicosDaOrdemDeServicos")]
    public virtual OrdemDeServico SequenciaDaOrdemDeServicoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoServico")]
    [InverseProperty("ServicosDaOrdemDeServicos")]
    public virtual Servico SequenciaDoServicoNavigation { get; set; } = null!;
}
