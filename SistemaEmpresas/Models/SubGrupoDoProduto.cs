using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoSubGrupoProduto", "SequenciaDoGrupoProduto")]
[Table("SubGrupo do Produto")]
public partial class SubGrupoDoProduto
{
    [Key]
    [Column("Seqüência do SubGrupo Produto")]
    public short SequenciaDoSubGrupoProduto { get; set; }

    [Key]
    [Column("Seqüência do Grupo Produto")]
    public short SequenciaDoGrupoProduto { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [InverseProperty("SubGrupoDoProduto")]
    public virtual ICollection<Conjunto> Conjuntos { get; set; } = new List<Conjunto>();

    [InverseProperty("SubGrupoDoProduto")]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    [ForeignKey("SequenciaDoGrupoProduto")]
    [InverseProperty("SubGrupoDoProdutos")]
    public virtual GrupoDoProduto SequenciaDoGrupoProdutoNavigation { get; set; } = null!;

    [InverseProperty("SubGrupoDoProduto")]
    public virtual ICollection<TransferenciaDeReceitum> TransferenciaDeReceita { get; set; } = new List<TransferenciaDeReceitum>();
}
