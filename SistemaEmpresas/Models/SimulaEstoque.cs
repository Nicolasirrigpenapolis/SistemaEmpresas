using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Simula estoque")]
public partial class SimulaEstoque
{
    [Key]
    [Column("Sequencia da simulação")]
    public int SequenciaDaSimulacao { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Atual estoque", TypeName = "decimal(11, 2)")]
    public decimal AtualEstoque { get; set; }

    [Column("Necessario estoque", TypeName = "decimal(11, 2)")]
    public decimal NecessarioEstoque { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Saidas estoque", TypeName = "decimal(11, 2)")]
    public decimal SaidasEstoque { get; set; }

    [Column("Saidas peças", TypeName = "decimal(11, 2)")]
    public decimal SaidasPecas { get; set; }

    [Column("Entradas pedido", TypeName = "decimal(11, 2)")]
    public decimal EntradasPedido { get; set; }

    [Column("Saidas orc prod", TypeName = "decimal(11, 2)")]
    public decimal SaidasOrcProd { get; set; }

    [Column("Saida orc peças", TypeName = "decimal(11, 2)")]
    public decimal SaidaOrcPecas { get; set; }

    [Column("Número da NFe")]
    public int NumeroDaNfe { get; set; }

    [Column("Último Fornecedor")]
    public int UltimoFornecedor { get; set; }

    [Column("Ultimo custo", TypeName = "decimal(11, 2)")]
    public decimal UltimoCusto { get; set; }
}
