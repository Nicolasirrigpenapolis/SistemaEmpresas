using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoControle", "SequenciaDoItem")]
[Table("Ocorrencias Garantia")]
public partial class OcorrenciaGarantium
{
    [Key]
    [Column("Sequencia do Controle")]
    public int SequenciaDoControle { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Data da Ocorrencia", TypeName = "datetime")]
    public DateTime? DataDaOcorrencia { get; set; }

    [Column("Data Saida", TypeName = "datetime")]
    public DateTime? DataSaida { get; set; }

    [Column("Número da NFe")]
    public int NumeroDaNfe { get; set; }

    [Column("Data do Retorno", TypeName = "datetime")]
    public DateTime? DataDoRetorno { get; set; }

    [Column("Data de Validade", TypeName = "datetime")]
    public DateTime? DataDeValidade { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Ocorrencia { get; set; } = null!;

    [Column("Ult Fornecedor")]
    public int UltFornecedor { get; set; }

    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Column("Notas da Compra")]
    [StringLength(100)]
    [Unicode(false)]
    public string NotasDaCompra { get; set; } = null!;
}
