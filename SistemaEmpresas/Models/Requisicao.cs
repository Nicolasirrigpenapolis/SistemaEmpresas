using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Requisição")]
public partial class Requisicao
{
    [Key]
    [Column("Seqüência da Requisição")]
    public int SequenciaDaRequisicao { get; set; }

    [Column("Data da Requisição", TypeName = "datetime")]
    public DateTime? DataDaRequisicao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column("Tipo do Desconto")]
    public short TipoDoDesconto { get; set; }

    [Column("Valor Total da Requisição", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaRequisicao { get; set; }

    [InverseProperty("SequenciaDaRequisicaoNavigation")]
    public virtual ICollection<ItenDaRequisicao> ItensDaRequisicaos { get; set; } = new List<ItenDaRequisicao>();

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("Requisicaos")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;
}
