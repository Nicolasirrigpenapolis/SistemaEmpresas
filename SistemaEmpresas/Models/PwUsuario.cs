using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("PwNome", "PwSenha")]
[Table("PW~Usuarios")]
[Index("PwGrupo", Name = "PW~Grupo")]
public partial class PwUsuario
{
    [Column("PW~Grupo")]
    [StringLength(100)]
    [Unicode(false)]
    public string PwGrupo { get; set; } = null!;

    [Key]
    [Column("PW~Nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string PwNome { get; set; } = null!;

    [Key]
    [Column("PW~Senha")]
    [StringLength(100)]
    [Unicode(false)]
    public string PwSenha { get; set; } = null!;

    [Column("PW~Obs")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PwObs { get; set; }

    /// <summary>
    /// Hash BCrypt da senha (opcional - usado para migração gradual).
    /// NULL = ainda usa PW~Senha em texto plano (compatibilidade com sistema legado).
    /// Quando preenchido, ignora PW~Senha e valida contra o hash.
    /// </summary>
    [Column("PW~SenhaHash")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PwSenhaHash { get; set; }

    /// <summary>
    /// Indica se o usuário está ativo no sistema.
    /// Usuários inativos não conseguem fazer login.
    /// </summary>
    [Column("PW~Ativo")]
    public bool PwAtivo { get; set; } = true;

    /// <summary>
    /// FK para a nova tabela GrupoUsuario (sistema web).
    /// Quando preenchido, usa os grupos novos ao invés do PwGrupo legado.
    /// </summary>
    [Column("GrupoUsuarioId")]
    public int? GrupoUsuarioId { get; set; }

    /// <summary>
    /// Email do usuário (opcional).
    /// Pode haver múltiplos usuários com o mesmo email.
    /// </summary>
    [Column("Email")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PwEmail { get; set; }

    [ForeignKey("PwGrupo")]
    [InverseProperty("PwUsuarios")]
    public virtual PwGrupo? PwGrupoNavigation { get; set; }

    /// <summary>
    /// Navegação para o novo grupo de usuário (sistema web)
    /// </summary>
    [ForeignKey("GrupoUsuarioId")]
    public virtual GrupoUsuario? GrupoUsuarioNavigation { get; set; }
}
