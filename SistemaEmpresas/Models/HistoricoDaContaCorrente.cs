using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Histórico da Conta Corrente")]
public partial class HistoricoDaContaCorrente
{
    [Key]
    [Column("Seqüência do Histórico")]
    public short SequenciaDoHistorico { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [InverseProperty("SequenciaDoHistoricoNavigation")]
    public virtual ICollection<MovimentacaoDaContaCorrente> MovimentacaoDaContaCorrentes { get; set; } = new List<MovimentacaoDaContaCorrente>();
}
