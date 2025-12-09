using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaImportacaoEstoque", "SequenciaImportacaoItem")]
[Table("Importação Produtos Estoque")]
public partial class ImportacaoProdutoEstoque
{
    [Key]
    [Column("Seqüência Importação Estoque")]
    public int SequenciaImportacaoEstoque { get; set; }

    [Key]
    [Column("Seqüência Importação Ítem")]
    public int SequenciaImportacaoItem { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("ImportacaoProdutosEstoques")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
