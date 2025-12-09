using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaNotaFiscal", "SequenciaServicoNotaFiscal")]
[Table("Serviços da Nota Fiscal")]
public partial class ServicoDaNotaFiscal
{
    [Key]
    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Key]
    [Column("Seqüência Serviço Nota Fiscal")]
    public int SequenciaServicoNotaFiscal { get; set; }

    [Column("Seqüência do Serviço")]
    public short SequenciaDoServico { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(11, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
    public decimal ValorTotal { get; set; }

    [ForeignKey("SequenciaDaNotaFiscal")]
    [InverseProperty("ServicosDaNotaFiscals")]
    public virtual NotaFiscal SequenciaDaNotaFiscalNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoServico")]
    [InverseProperty("ServicosDaNotaFiscals")]
    public virtual Servico SequenciaDoServicoNavigation { get; set; } = null!;
}
