using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Vinculo Produto Fornecedor")]
[Index(nameof(SequenciaDoGeral), nameof(CodigoProdutoFornecedor), Name = "IX_Vinculo_Fornecedor_Codigo")]
public partial class VinculoProdutoFornecedor
{
    [Key]
    [Column("Sequencia do Vinculo")]
    public int SequenciaDoVinculo { get; set; }

    [Column("Sequencia do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Codigo Produto Fornecedor")]
    [StringLength(60)]
    [Unicode(false)]
    public string CodigoProdutoFornecedor { get; set; } = null!;

    [Column("Sequencia do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Data do Vinculo", TypeName = "datetime")]
    public DateTime DataDoVinculo { get; set; }

    [ForeignKey(nameof(SequenciaDoGeral))]
    public virtual Geral Fornecedor { get; set; } = null!;

    [ForeignKey(nameof(SequenciaDoProduto))]
    public virtual Produto Produto { get; set; } = null!;
}
