using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("CodigoDoSolicitante", "SequenciaDoItem", "CodigoDoSetor")]
public partial class Solicitante
{
    [Key]
    [Column("Codigo do solicitante")]
    public short CodigoDoSolicitante { get; set; }

    [Column("Nome do solicitante")]
    [StringLength(25)]
    [Unicode(false)]
    public string NomeDoSolicitante { get; set; } = null!;

    [Key]
    [Column("Codigo do setor")]
    public short CodigoDoSetor { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }
}
