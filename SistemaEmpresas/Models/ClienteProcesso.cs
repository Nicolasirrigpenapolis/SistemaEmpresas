using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Clientes Processos")]
public partial class ClienteProcesso
{
    [Key]
    [Column("Codigo do Cliente")]
    public int CodigoDoCliente { get; set; }

    [Column("Nome do Cliente")]
    [StringLength(40)]
    [Unicode(false)]
    public string NomeDoCliente { get; set; } = null!;

    public bool Envolvido { get; set; }
}
