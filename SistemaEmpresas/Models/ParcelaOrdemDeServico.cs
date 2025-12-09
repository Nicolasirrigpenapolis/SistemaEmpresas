using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaOrdemDeServico", "NumeroDaParcela")]
[Table("Parcelas Ordem de Serviço")]
public partial class ParcelaOrdemDeServico
{
    [Key]
    [Column("Seqüência da Ordem de Serviço")]
    public int SequenciaDaOrdemDeServico { get; set; }

    [Key]
    [Column("Número da Parcela")]
    public short NumeroDaParcela { get; set; }

    public short Dias { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime DataDeVencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [ForeignKey("SequenciaDaOrdemDeServico")]
    [InverseProperty("ParcelasOrdemDeServicos")]
    public virtual OrdemDeServico SequenciaDaOrdemDeServicoNavigation { get; set; } = null!;
}
