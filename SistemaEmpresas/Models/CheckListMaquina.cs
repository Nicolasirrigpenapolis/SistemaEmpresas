using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoProduto", "SequenciaDoItem")]
[Table("Check list maquina")]
public partial class CheckListMaquina
{
    [Key]
    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string Tpproduto { get; set; } = null!;

    [Column("Seqüência da Matéria Prima")]
    public int SequenciaDaMateriaPrima { get; set; }

    [Column("Qtde utilizada", TypeName = "decimal(11, 2)")]
    public decimal QtdeUtilizada { get; set; }
}
