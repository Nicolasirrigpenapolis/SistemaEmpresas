using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Movimento do Estoque Contábil")]
public partial class MovimentoDoEstoqueContabil
{
    [Key]
    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Column("Data do Movimento", TypeName = "datetime")]
    public DateTime? DataDoMovimento { get; set; }

    [Column("Tipo do Movimento")]
    public short TipoDoMovimento { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [Column("Tipo do Produto")]
    public short TipoDoProduto { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column(TypeName = "text")]
    public string Observacao { get; set; } = null!;

    public bool Devolucao { get; set; }

    [InverseProperty("SequenciaDoMovimentoNavigation")]
    public virtual ICollection<ConjuntoMovimentoContabil> ConjuntosMovimentoContabils { get; set; } = new List<ConjuntoMovimentoContabil>();

    [InverseProperty("SequenciaDoMovimentoNavigation")]
    public virtual ICollection<ProdutoDoMovimentoContabil> ProdutosDoMovimentoContabils { get; set; } = new List<ProdutoDoMovimentoContabil>();

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("MovimentoDoEstoqueContabils")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;
}
