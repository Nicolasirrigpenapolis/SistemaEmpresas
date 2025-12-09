using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Transferência de Receita")]
public partial class TransferenciaDeReceitum
{
    [Key]
    [Column("Seqüência da Transferência")]
    public int SequenciaDaTransferencia { get; set; }

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Seqüência da Unidade")]
    public short SequenciaDaUnidade { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Localizacao { get; set; } = null!;

    [Column("Seqüência do Grupo Produto")]
    public short SequenciaDoGrupoProduto { get; set; }

    [Column("Seqüência do SubGrupo Produto")]
    public short SequenciaDoSubGrupoProduto { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [ForeignKey("SequenciaDaUnidade")]
    [InverseProperty("TransferenciaDeReceita")]
    public virtual Unidade SequenciaDaUnidadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGrupoProduto")]
    [InverseProperty("TransferenciaDeReceita")]
    public virtual GrupoDoProduto SequenciaDoGrupoProdutoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("TransferenciaDeReceita")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;

    [ForeignKey("SeqüênciaDoSubGrupoProduto, SeqüênciaDoGrupoProduto")]
    [InverseProperty("TransferenciaDeReceita")]
    public virtual SubGrupoDoProduto SubGrupoDoProduto { get; set; } = null!;
}
