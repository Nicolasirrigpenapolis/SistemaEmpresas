using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Declarações de Importação")]
public partial class DeclaracaoDeImportacao
{
    [Key]
    [Column("Seqüência da Declaração")]
    public int SequenciaDaDeclaracao { get; set; }

    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("Seqüência Produto Nota Fiscal")]
    public int SequenciaProdutoNotaFiscal { get; set; }

    [Column("Número da Declaração")]
    [StringLength(10)]
    [Unicode(false)]
    public string NumeroDaDeclaracao { get; set; } = null!;

    [Column("Data de Registro", TypeName = "datetime")]
    public DateTime DataDeRegistro { get; set; }

    [Column("Local de Desembaraço")]
    [StringLength(60)]
    [Unicode(false)]
    public string LocalDeDesembaraco { get; set; } = null!;

    [Column("UF de Desembaraço")]
    [StringLength(3)]
    [Unicode(false)]
    public string UfDeDesembaraco { get; set; } = null!;

    [Column("Data de Desembaraço", TypeName = "datetime")]
    public DateTime DataDeDesembaraco { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Valor da Marinha Mercante", TypeName = "decimal(10, 2)")]
    public decimal ValorDaMarinhaMercante { get; set; }

    [Column("Via Transporte")]
    public short ViaTransporte { get; set; }

    [InverseProperty("SequenciaDaDeclaracaoNavigation")]
    public virtual ICollection<AdicaoDaDeclaracao> AdicoesDaDeclaracaos { get; set; } = new List<AdicaoDaDeclaracao>();

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("DeclaracoesDeImportacaos")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;
}
