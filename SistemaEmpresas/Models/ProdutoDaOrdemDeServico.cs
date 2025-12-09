using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaOrdemDeServico", "SequenciaProdutoOs")]
[Table("Produtos da Ordem de Serviço")]
public partial class ProdutoDaOrdemDeServico
{
    [Key]
    [Column("Seqüência da Ordem de Serviço")]
    public int SequenciaDaOrdemDeServico { get; set; }

    [Key]
    [Column("Seqüência Produto OS")]
    public int SequenciaProdutoOs { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Valor Unitário", TypeName = "decimal(11, 4)")]
    public decimal ValorUnitario { get; set; }

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
    public decimal ValorTotal { get; set; }

    [Column("Valor do IPI", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIpi { get; set; }

    [Column("Valor do ICMS", TypeName = "decimal(12, 4)")]
    public decimal ValorDoIcms { get; set; }

    [Column("Alíquota do IPI", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIpi { get; set; }

    [Column("Alíquota do ICMS", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIcms { get; set; }

    [Column("Percentual da Redução", TypeName = "decimal(5, 2)")]
    public decimal PercentualDaReducao { get; set; }

    public bool Diferido { get; set; }

    [Column("Valor da Base de Cálculo", TypeName = "decimal(11, 2)")]
    public decimal ValorDaBaseDeCalculo { get; set; }

    [Column("Valor do PIS", TypeName = "decimal(11, 4)")]
    public decimal ValorDoPis { get; set; }

    [Column("Valor do Cofins", TypeName = "decimal(11, 4)")]
    public decimal ValorDoCofins { get; set; }

    [Column("IVA", TypeName = "decimal(7, 4)")]
    public decimal Iva { get; set; }

    [Column("Base de Cálculo ST", TypeName = "decimal(11, 2)")]
    public decimal BaseDeCalculoSt { get; set; }

    [Column("Valor ICMS ST", TypeName = "decimal(11, 2)")]
    public decimal ValorIcmsSt { get; set; }

    [Column("CFOP")]
    public short Cfop { get; set; }

    [Column("CST")]
    public short Cst { get; set; }

    [Column("Alíquota do ICMS ST", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIcmsSt { get; set; }

    [ForeignKey("SequenciaDaOrdemDeServico")]
    [InverseProperty("ProdutosDaOrdemDeServicos")]
    public virtual OrdemDeServico SequenciaDaOrdemDeServicoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoProduto")]
    [InverseProperty("ProdutosDaOrdemDeServicos")]
    public virtual Produto SequenciaDoProdutoNavigation { get; set; } = null!;
}
