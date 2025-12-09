using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Previsoes de Pagtos")]
public partial class PrevisaoDePagto
{
    [Key]
    [Column("Sequencia da Previsao")]
    public int SequenciaDaPrevisao { get; set; }

    [Column("Saldo IPG", TypeName = "decimal(10, 2)")]
    public decimal SaldoIpg { get; set; }

    [Column("Saldo Chinellato", TypeName = "decimal(10, 2)")]
    public decimal SaldoChinellato { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime? DataDeVencimento { get; set; }

    [Column("Data de Entrada", TypeName = "datetime")]
    public DateTime? DataDeEntrada { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    public short Parcela { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [Column("Valor Previsto", TypeName = "decimal(12, 2)")]
    public decimal ValorPrevisto { get; set; }

    public bool Imprimir { get; set; }

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [Column("Nome da Empresa")]
    [StringLength(30)]
    [Unicode(false)]
    public string NomeDaEmpresa { get; set; } = null!;

    [Column("Valor Restante", TypeName = "decimal(11, 2)")]
    public decimal ValorRestante { get; set; }

    [Column("Tp Pagto")]
    [StringLength(20)]
    [Unicode(false)]
    public string TpPagto { get; set; } = null!;
}
