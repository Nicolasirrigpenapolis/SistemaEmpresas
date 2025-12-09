using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Revendedore
{
    [Key]
    [Column("Sequencia da Revenda")]
    public int SequenciaDaRevenda { get; set; }

    [Column("Id da Conta")]
    public int IdDaConta { get; set; }

    [Column("Tem Contrato")]
    public bool TemContrato { get; set; }

    [ForeignKey("IdDaConta")]
    [InverseProperty("Revendedores")]
    public virtual ContaDoVendedor IdDaContaNavigation { get; set; } = null!;
}
