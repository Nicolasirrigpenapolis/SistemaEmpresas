using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("ConfiguracaoIntegracao")]
[Index("Chave", Name = "UK_ConfiguracaoIntegracao_Chave", IsUnique = true)]
public partial class ConfiguracaoIntegracao
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Chave { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Valor { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataUltimaAlteracao { get; set; }
}
