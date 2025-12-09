using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Tipo de Titulos")]
public partial class TipoDeTitulo
{
    [Key]
    [Column("Seq do Titulo")]
    public short SeqDoTitulo { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;
}
