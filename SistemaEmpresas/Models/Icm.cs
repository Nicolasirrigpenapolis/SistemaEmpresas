using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("ICMS")]
public partial class Icm
{
    [Key]
    [Column("Seqüência do ICMS")]
    public short SequenciaDoIcms { get; set; }

    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    // Removido: Regiao - coluna não existe na tabela do banco
    // [StringLength(20)]
    // [Unicode(false)]
    // public string Regiao { get; set; } = null!;

    [Column("Porcentagem de ICMS", TypeName = "decimal(5, 2)")]
    public decimal PorcentagemDeIcms { get; set; }

    [Column("Alíquota InterEstadual", TypeName = "decimal(5, 2)")]
    public decimal AliquotaInterEstadual { get; set; }

    [Column("Código da UF")]
    public short CodigoDaUf { get; set; }
}
