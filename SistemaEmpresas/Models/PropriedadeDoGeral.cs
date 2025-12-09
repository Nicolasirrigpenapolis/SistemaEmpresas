using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Propriedades do Geral")]
public partial class PropriedadeDoGeral
{
    [Key]
    [Column("Seqüência da Propriedade Geral")]
    public int SequenciaDaPropriedadeGeral { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    public bool Inativo { get; set; }

    [ForeignKey("SequenciaDaPropriedade")]
    [InverseProperty("PropriedadesDoGerals")]
    public virtual Propriedade SequenciaDaPropriedadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("PropriedadesDoGerals")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;
}
