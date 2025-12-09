using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Importação Estoque")]
public partial class ImportacaoEstoque
{
    [Key]
    [Column("Seqüência Importação Estoque")]
    public int SequenciaImportacaoEstoque { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;
}
