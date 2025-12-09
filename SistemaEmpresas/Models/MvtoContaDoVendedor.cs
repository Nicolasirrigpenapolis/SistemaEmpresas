using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Mvto Conta do Vendedor")]
public partial class MvtoContaDoVendedor
{
    [Key]
    [Column("Id do Movimento")]
    public int IdDoMovimento { get; set; }

    [Column("Dta do Movimento", TypeName = "datetime")]
    public DateTime DtaDoMovimento { get; set; }

    [Column("Id Conta")]
    public int IdConta { get; set; }

    [Column("Valor Entrada", TypeName = "decimal(10, 2)")]
    public decimal ValorEntrada { get; set; }

    [Column("Valor Saida", TypeName = "decimal(10, 2)")]
    public decimal ValorSaida { get; set; }

    [Column(TypeName = "text")]
    public string Historico { get; set; } = null!;

    public bool Informativo { get; set; }
}
