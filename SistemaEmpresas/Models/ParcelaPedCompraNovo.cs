using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("IdDoPedido", "NumeroDaParcela")]
[Table("Parcelas Ped Compra Novo")]
public partial class ParcelaPedCompraNovo
{
    [Key]
    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Key]
    [Column("Número da Parcela")]
    public short NumeroDaParcela { get; set; }

    public short Dias { get; set; }

    [Column("Data de Vencimento", TypeName = "datetime")]
    public DateTime DataDeVencimento { get; set; }

    [Column("Valor da Parcela", TypeName = "decimal(11, 2)")]
    public decimal ValorDaParcela { get; set; }

    public int Nota { get; set; }

    [Column("Seqüência da Cobrança")]
    public short SequenciaDaCobranca { get; set; }
}
