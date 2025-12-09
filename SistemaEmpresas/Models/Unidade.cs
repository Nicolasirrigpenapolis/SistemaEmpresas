using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Unidade
{
    [Key]
    [Column("Seqüência da Unidade")]
    public short SequenciaDaUnidade { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Sigla da Unidade")]
    [StringLength(15)]
    [Unicode(false)]
    public string SiglaDaUnidade { get; set; } = null!;

    [InverseProperty("SequenciaDaUnidadeNavigation")]
    public virtual ICollection<Conjunto> Conjuntos { get; set; } = new List<Conjunto>();

    [InverseProperty("SequenciaDaUnidadeNavigation")]
    public virtual ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();

    [InverseProperty("SequenciaDaUnidadeNavigation")]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    [InverseProperty("SequenciaDaUnidadeNavigation")]
    public virtual ICollection<TransferenciaDeReceitum> TransferenciaDeReceita { get; set; } = new List<TransferenciaDeReceitum>();
}
