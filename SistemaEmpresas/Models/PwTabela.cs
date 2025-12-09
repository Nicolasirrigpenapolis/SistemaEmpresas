using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("PwProjeto", "PwGrupo", "PwNome")]
[Table("PW~Tabelas")]
public partial class PwTabela
{
    [Key]
    [Column("PW~Projeto")]
    [StringLength(10)]
    [Unicode(false)]
    public string PwProjeto { get; set; } = null!;

    [Key]
    [Column("PW~Grupo")]
    [StringLength(100)]
    [Unicode(false)]
    public string PwGrupo { get; set; } = null!;

    [Key]
    [Column("PW~Nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string PwNome { get; set; } = null!;

    [Column("PW~Permissoes")]
    [StringLength(100)]
    [Unicode(false)]
    public string PwPermissoes { get; set; } = null!;

    [ForeignKey("PwGrupo")]
    [InverseProperty("PwTabelas")]
    public virtual PwGrupo PwGrupoNavigation { get; set; } = null!;
}
