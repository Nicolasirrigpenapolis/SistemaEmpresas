using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoOrcamento", "SequenciaDoServicoOrcamento")]
[Table("Serviços do Orçamento")]
public partial class ServicoDoOrcamento
{
    [Key]
    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Key]
    [Column("Seqüência do Serviço Orçamento")]
    public int SequenciaDoServicoOrcamento { get; set; }

    [Column("Seqüência do Serviço")]
    public short SequenciaDoServico { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(12, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Valor Anterior", TypeName = "decimal(12, 2)")]
    public decimal ValorAnterior { get; set; }

    [ForeignKey("SequenciaDoOrcamento")]
    [InverseProperty("ServicosDoOrcamentos")]
    public virtual Orcamento SequenciaDoOrcamentoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoServico")]
    [InverseProperty("ServicosDoOrcamentos")]
    public virtual Servico SequenciaDoServicoNavigation { get; set; } = null!;
}
