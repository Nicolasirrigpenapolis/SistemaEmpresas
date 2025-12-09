using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Classificação Fiscal")]
public partial class ClassificacaoFiscal
{
    [Key]
    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("NCM")]
    public int Ncm { get; set; }

    [Column("Descrição do NCM")]
    [StringLength(100)]
    [Unicode(false)]
    public string DescricaoDoNcm { get; set; } = null!;

    [Column("Porcentagem do IPI", TypeName = "decimal(8, 4)")]
    public decimal PorcentagemDoIpi { get; set; }

    [Column("Anexo da Redução")]
    public short AnexoDaReducao { get; set; }

    [Column("Alíquota do Anexo")]
    public short AliquotaDoAnexo { get; set; }

    [Column("Produto Diferido")]
    public bool ProdutoDiferido { get; set; }

    [Column("Redução de Base de Cálculo")]
    public bool ReducaoDeBaseDeCalculo { get; set; }

    public bool Inativo { get; set; }

    [Column("IVA", TypeName = "decimal(7, 4)")]
    public decimal Iva { get; set; }

    [Column("Tem Convênio")]
    public bool TemConvenio { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string Cest { get; set; } = null!;

    [Column("Un Exterior")]
    [StringLength(10)]
    [Unicode(false)]
    public string UnExterior { get; set; } = null!;

    // ============================================================================
    // CHAVE ESTRANGEIRA - CLASSTRIB (Integração SVRS IBS/CBS)
    // ============================================================================
    // Os dados de classificação tributária (CST, percentuais IBS/CBS, etc.)
    // vêm da tabela ClassTrib via esta FK.
    // A associação NCM ↔ ClassTrib é feita manualmente pelo usuário.
    // ============================================================================

    /// <summary>
    /// FK para tabela ClassTrib - Classificação Tributária SVRS
    /// Quando preenchido, os dados de IBS/CBS são obtidos via navigation property
    /// </summary>
    [Column("ClassTribId")]
    public int? ClassTribId { get; set; }

    /// <summary>
    /// Navigation Property para ClassTrib
    /// Acesso: ClassTribNavigation.CodigoClassTrib, .PercentualReducaoIBS, etc.
    /// </summary>
    [ForeignKey("ClassTribId")]
    public virtual ClassTrib? ClassTribNavigation { get; set; }

    // ============================================================================
    // NAVIGATION PROPERTIES (Relacionamentos)
    // ============================================================================

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<Conjunto> Conjuntos { get; set; } = new List<Conjunto>();

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<MovimentoDoEstoque> MovimentoDoEstoques { get; set; } = new List<MovimentoDoEstoque>();

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<OrdemDeServico> OrdemDeServicos { get; set; } = new List<OrdemDeServico>();

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<Orcamento> Orcamentos { get; set; } = new List<Orcamento>();

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("SequenciaDaClassificacaoNavigation")]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
