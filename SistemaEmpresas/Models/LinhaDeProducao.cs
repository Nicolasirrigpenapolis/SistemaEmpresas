using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Linha de Produção")]
public partial class LinhaDeProducao
{
    [Key]
    [Column("Sequencia da Produção")]
    public int SequenciaDaProducao { get; set; }

    [Column("Data da Produção", TypeName = "datetime")]
    public DateTime? DataDaProducao { get; set; }

    [Column("Codigo do setor")]
    public short CodigoDoSetor { get; set; }

    [Column("Codigo do Colaborador")]
    public short CodigoDoColaborador { get; set; }

    [Column("Solicitação de")]
    [StringLength(10)]
    [Unicode(false)]
    public string SolicitacaoDe { get; set; } = null!;

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    public bool Finalizado { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Apenas Montagem")]
    public bool ApenasMontagem { get; set; }

    [ForeignKey("CodigoDoSetor")]
    [InverseProperty("LinhaDeProducaos")]
    public virtual Setore CodigoDoSetorNavigation { get; set; } = null!;
}
