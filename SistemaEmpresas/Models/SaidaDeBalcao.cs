using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Saida de Balcao")]
public partial class SaidaDeBalcao
{
    [Key]
    [Column("Sequencia da Saida")]
    public int SequenciaDaSaida { get; set; }

    [Column("Data da Saida", TypeName = "datetime")]
    public DateTime? DataDaSaida { get; set; }

    [Column("Codigo do setor")]
    public short CodigoDoSetor { get; set; }

    [Column("Codigo do solicitante")]
    public short CodigoDoSolicitante { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [Column("Codigo do solicitante 2")]
    public short CodigoDoSolicitante2 { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Teste { get; set; } = null!;
}
