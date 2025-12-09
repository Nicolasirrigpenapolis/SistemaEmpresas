using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Grupo do Produto")]
public partial class GrupoDoProduto
{
    [Key]
    [Column("Seqüência do Grupo Produto")]
    public short SequenciaDoGrupoProduto { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    public bool Inativo { get; set; }

    [InverseProperty("SequenciaDoGrupoProdutoNavigation")]
    public virtual ICollection<Conjunto> Conjuntos { get; set; } = new List<Conjunto>();

    [InverseProperty("SequenciaDoGrupoProdutoNavigation")]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    [InverseProperty("SequenciaDoGrupoProdutoNavigation")]
    public virtual ICollection<SubGrupoDoProduto> SubGrupoDoProdutos { get; set; } = new List<SubGrupoDoProduto>();

    [InverseProperty("SequenciaDoGrupoProdutoNavigation")]
    public virtual ICollection<TransferenciaDeReceitum> TransferenciaDeReceita { get; set; } = new List<TransferenciaDeReceitum>();
}
