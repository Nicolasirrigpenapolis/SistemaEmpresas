using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaSaida", "SequenciaDoItem")]
[Table("Itens Saidas Balcao")]
public partial class ItenSaidaBalcao
{
    [Key]
    [Column("Sequencia da Saida")]
    public int SequenciaDaSaida { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    public bool Consignado { get; set; }

    [Column("Seq principal")]
    public int SeqPrincipal { get; set; }
}
