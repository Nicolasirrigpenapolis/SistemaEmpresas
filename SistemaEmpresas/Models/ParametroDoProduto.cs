using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Parâmetros do Produto")]
public partial class ParametroDoProduto
{
    [Column("Percentual Acréscimo Produto", TypeName = "decimal(5, 2)")]
    public decimal PercentualAcrescimoProduto { get; set; }

    [Column("Percentual Acréscimo Conjunto", TypeName = "decimal(5, 2)")]
    public decimal PercentualAcrescimoConjunto { get; set; }

    [Column("Acrescimo do Parcelamento", TypeName = "decimal(8, 4)")]
    public decimal AcrescimoDoParcelamento { get; set; }

    [Column("Percentual 2", TypeName = "decimal(6, 2)")]
    public decimal Percentual2 { get; set; }

    [Column("Ja atualizou")]
    public bool JaAtualizou { get; set; }
}
