using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoOrcamento", "NumeroDaParcela")]
[Table("Parcelas Orçamento")]
public partial class ParcelaOrcamento
{
    [Key]
    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Key]
    [Column("Número da Parcela")]
    public short NumeroDaParcela { get; set; }

    public short Dias { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime DataDeVencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Descrição da Cobrança")]
    [StringLength(120)]
    [Unicode(false)]
    public string DescricaoDaCobranca { get; set; } = null!;

    [ForeignKey("SequenciaDoOrcamento")]
    [InverseProperty("ParcelasOrcamentos")]
    public virtual Orcamento SequenciaDoOrcamentoNavigation { get; set; } = null!;
}
