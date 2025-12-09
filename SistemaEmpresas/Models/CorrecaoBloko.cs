using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Correcao Bloko K")]
public partial class CorrecaoBloko
{
    [Key]
    [Column("Sequencia da Correção")]
    public int SequenciaDaCorrecao { get; set; }

    [Column("Data da Correção", TypeName = "datetime")]
    public DateTime? DataDaCorrecao { get; set; }
}
