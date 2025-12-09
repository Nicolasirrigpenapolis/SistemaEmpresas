using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("PW~Grupos")]
public partial class PwGrupo
{
    [Key]
    [Column("PW~Nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string PwNome { get; set; } = null!;

    [InverseProperty("PwGrupoNavigation")]
    public virtual ICollection<PwTabela> PwTabelas { get; set; } = new List<PwTabela>();

    [InverseProperty("PwGrupoNavigation")]
    public virtual ICollection<PwUsuario> PwUsuarios { get; set; } = new List<PwUsuario>();
}
