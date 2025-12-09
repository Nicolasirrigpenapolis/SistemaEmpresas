using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Index("Dominio", Name = "IX_Tenants_Dominio", IsUnique = true)]
public partial class Tenant
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string Nome { get; set; } = null!;

    [StringLength(200)]
    public string Dominio { get; set; } = null!;

    [StringLength(500)]
    public string ConnectionString { get; set; } = null!;

    public bool Ativo { get; set; }
}
