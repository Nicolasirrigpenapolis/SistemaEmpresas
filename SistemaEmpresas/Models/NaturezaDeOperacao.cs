using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Natureza de Operação")]
public partial class NaturezaDeOperacao
{
    [Key]
    [Column("Seqüência da Natureza")]
    public short SequenciaDaNatureza { get; set; }

    [Column("Código da Natureza de Operação")]
    public int CodigoDaNaturezaDeOperacao { get; set; }

    [Column("Descrição da Natureza Operação")]
    [StringLength(30)]
    [Unicode(false)]
    public string DescricaoDaNaturezaOperacao { get; set; } = null!;

    public bool Inativo { get; set; }

    [InverseProperty("SequenciaDaNaturezaNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();
}
