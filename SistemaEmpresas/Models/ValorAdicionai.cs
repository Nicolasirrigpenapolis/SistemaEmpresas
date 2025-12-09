using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoValores", "SequenciaDaManutencao")]
[Table("Valores Adicionais")]
public partial class ValorAdicionai
{
    [Key]
    [Column("Seqüência do Valores")]
    public int SequenciaDoValores { get; set; }

    [Key]
    [Column("Seqüência da Manutenção")]
    public int SequenciaDaManutencao { get; set; }

    [Column("Valor do Juros", TypeName = "decimal(11, 2)")]
    public decimal ValorDoJuros { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column(TypeName = "text")]
    public string Observacao { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Conta { get; set; } = null!;

    [ForeignKey("SequenciaDaManutencao")]
    [InverseProperty("ValoresAdicionais")]
    public virtual ManutencaoConta SequenciaDaManutencaoNavigation { get; set; } = null!;
}
