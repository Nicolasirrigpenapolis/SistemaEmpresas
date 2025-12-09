using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Tipo de Cobrança")]
public partial class TipoDeCobranca
{
    [Key]
    [Column("Seqüência da Cobrança")]
    public short SequenciaDaCobranca { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    public bool Inativo { get; set; }

    [InverseProperty("SequenciaDaCobrancaNavigation")]
    public virtual ICollection<ManutencaoConta> ManutencaoConta { get; set; } = new List<ManutencaoConta>();

    [InverseProperty("SequenciaDaCobrancaNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();
}
