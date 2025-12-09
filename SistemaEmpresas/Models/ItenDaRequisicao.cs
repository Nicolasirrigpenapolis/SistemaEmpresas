using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaRequisicao", "SequenciaProdutoRequisicao")]
[Table("Itens da Requisição")]
public partial class ItenDaRequisicao
{
    [Key]
    [Column("Seqüência da Requisição")]
    public int SequenciaDaRequisicao { get; set; }

    [Key]
    [Column("Seqüência Produto Requisição")]
    public int SequenciaProdutoRequisicao { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(12, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Veiculo { get; set; } = null!;

    [ForeignKey("SequenciaDaRequisicao")]
    [InverseProperty("ItensDaRequisicaos")]
    public virtual Requisicao SequenciaDaRequisicaoNavigation { get; set; } = null!;
}
