using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Dados Adicionais")]
public partial class DadoAdicionai
{
    [Key]
    [Column("Seqüência dos Dados Adicionais")]
    public int SequenciaDosDadosAdicionais { get; set; }

    [Column("Dados Adicionais", TypeName = "text")]
    public string DadosAdicionais { get; set; } = null!;
}
