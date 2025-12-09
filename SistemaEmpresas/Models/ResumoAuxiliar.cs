using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Resumo auxiliar")]
public partial class ResumoAuxiliar
{
    [Key]
    [Column("Sequencia do resumo")]
    public int SequenciaDoResumo { get; set; }

    [Column("Data do Movimento", TypeName = "datetime")]
    public DateTime DataDoMovimento { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Sd inicial", TypeName = "decimal(11, 2)")]
    public decimal SdInicial { get; set; }

    [Column("Inicial estoque", TypeName = "decimal(11, 4)")]
    public decimal InicialEstoque { get; set; }

    [Column("Qt entradas", TypeName = "decimal(11, 4)")]
    public decimal QtEntradas { get; set; }

    [Column("Qt saidas", TypeName = "decimal(11, 4)")]
    public decimal QtSaidas { get; set; }

    [Column("V_entradas", TypeName = "decimal(12, 4)")]
    public decimal VEntradas { get; set; }

    [Column("V_saidas", TypeName = "decimal(12, 4)")]
    public decimal VSaidas { get; set; }

    [Column("Estoque_final", TypeName = "decimal(11, 4)")]
    public decimal EstoqueFinal { get; set; }

    [Column("Sd final", TypeName = "decimal(11, 2)")]
    public decimal SdFinal { get; set; }

    [Column("Tipo do Movimento")]
    public short TipoDoMovimento { get; set; }

    [Column("Seqüência da Baixa")]
    public int SequenciaDaBaixa { get; set; }
}
