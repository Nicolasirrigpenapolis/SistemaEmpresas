using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaImportacaoEstoque", "SequenciaImportacaoItem")]
[Table("Importação Conjuntos Estoque")]
public partial class ImportacaoConjuntoEstoque
{
    [Key]
    [Column("Seqüência Importação Estoque")]
    public int SequenciaImportacaoEstoque { get; set; }

    [Key]
    [Column("Seqüência Importação Ítem")]
    public int SequenciaImportacaoItem { get; set; }

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }
}
