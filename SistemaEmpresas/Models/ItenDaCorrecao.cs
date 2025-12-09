using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaCorrecao", "SequenciaDoProduto")]
[Table("Itens da Correcao")]
public partial class ItenDaCorrecao
{
    [Key]
    [Column("Sequencia da Correção")]
    public int SequenciaDaCorrecao { get; set; }

    [Key]
    [Column("Sequencia do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Data do Estoque", TypeName = "datetime")]
    public DateTime? DataDoEstoque { get; set; }

    [Column("Quantidade Positiva", TypeName = "decimal(11, 4)")]
    public decimal QuantidadePositiva { get; set; }

    [Column("Quantidade Negativa", TypeName = "decimal(11, 4)")]
    public decimal QuantidadeNegativa { get; set; }
}
