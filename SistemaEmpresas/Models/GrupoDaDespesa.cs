using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Grupo da Despesa")]
public partial class GrupoDaDespesa
{
    [Key]
    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    public bool Inativo { get; set; }

    [InverseProperty("SequenciaGrupoDespesaNavigation")]
    public virtual ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();

    [InverseProperty("SequenciaGrupoDespesaNavigation")]
    public virtual ICollection<EntradaConta> EntradaConta { get; set; } = new List<EntradaConta>();

    [InverseProperty("SequenciaGrupoDespesaNavigation")]
    public virtual ICollection<ManutencaoConta> ManutencaoConta { get; set; } = new List<ManutencaoConta>();

    [InverseProperty("SequenciaGrupoDespesaNavigation")]
    public virtual ICollection<MovimentoDoEstoque> MovimentoDoEstoques { get; set; } = new List<MovimentoDoEstoque>();

    [InverseProperty("SequenciaGrupoDespesaNavigation")]
    public virtual ICollection<SubGrupoDespesa> SubGrupoDespesas { get; set; } = new List<SubGrupoDespesa>();
}
