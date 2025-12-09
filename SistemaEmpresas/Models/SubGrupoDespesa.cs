using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaSubGrupoDespesa", "SequenciaGrupoDespesa")]
[Table("SubGrupo Despesa")]
public partial class SubGrupoDespesa
{
    [Key]
    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Key]
    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [InverseProperty("SubGrupoDespesa")]
    public virtual ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();

    [InverseProperty("SubGrupoDespesa")]
    public virtual ICollection<EntradaConta> EntradaConta { get; set; } = new List<EntradaConta>();

    [InverseProperty("SubGrupoDespesa")]
    public virtual ICollection<ManutencaoConta> ManutencaoConta { get; set; } = new List<ManutencaoConta>();

    [InverseProperty("SubGrupoDespesa")]
    public virtual ICollection<MovimentoDoEstoque> MovimentoDoEstoques { get; set; } = new List<MovimentoDoEstoque>();

    [ForeignKey("SequenciaGrupoDespesa")]
    [InverseProperty("SubGrupoDespesas")]
    public virtual GrupoDaDespesa SequenciaGrupoDespesaNavigation { get; set; } = null!;
}
