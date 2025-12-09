using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdDaBaixa", "IdDoAdiantamento")]
[Table("Baixa Comissão Lote Contas")]
public partial class BaixaComissaoLoteConta
{
    [Key]
    [Column("Id da Baixa")]
    public int IdDaBaixa { get; set; }

    [Key]
    [Column("Id do Adiantamento")]
    public int IdDoAdiantamento { get; set; }

    [Column("NFe")]
    [StringLength(10)]
    [Unicode(false)]
    public string Nfe { get; set; } = null!;

    public short Parcela { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    [Column("Valor Pago", TypeName = "decimal(11, 2)")]
    public decimal ValorPago { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Vencto { get; set; }

    [Column("Data Pagto Cliente", TypeName = "datetime")]
    public DateTime? DataPagtoCliente { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Percentual { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Comissao { get; set; }
}
