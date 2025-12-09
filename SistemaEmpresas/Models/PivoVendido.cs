using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Pivos Vendidos")]
public partial class PivoVendido
{
    [Key]
    [Column("Seq do Pivo")]
    public int SeqDoPivo { get; set; }

    [Column("Modelo do Pivo")]
    [StringLength(6)]
    [Unicode(false)]
    public string ModeloDoPivo { get; set; } = null!;

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [StringLength(40)]
    [Unicode(false)]
    public string Cidade { get; set; } = null!;

    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;
}
