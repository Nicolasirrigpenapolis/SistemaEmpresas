using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Tipo de Atividades")]
public partial class TipoDeAtividade
{
    [Key]
    [Column("Codigo da Atividade")]
    public short CodigoDaAtividade { get; set; }

    [Column("Descrição da Atividade")]
    [StringLength(40)]
    [Unicode(false)]
    public string DescricaoDaAtividade { get; set; } = null!;

    public bool Inativo { get; set; }
}
