using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Grupos de usuários do sistema web.
/// Esta tabela é independente da tabela PW~Grupos do sistema VB6 legado.
/// Os grupos definem níveis de acesso e permissões no sistema.
/// </summary>
[Table("GrupoUsuario")]
[Index(nameof(Nome), IsUnique = true)]
public class GrupoUsuario
{
    /// <summary>
    /// Identificador único do grupo
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Nome do grupo (ex: PROGRAMADOR, VENDAS, FINANCEIRO)
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    /// <summary>
    /// Descrição do grupo (opcional)
    /// </summary>
    [StringLength(500)]
    public string? Descricao { get; set; }

    /// <summary>
    /// Indica se o grupo está ativo
    /// </summary>
    public bool Ativo { get; set; } = true;

    /// <summary>
    /// Indica se é um grupo de sistema (não pode ser excluído)
    /// Grupos de sistema: PROGRAMADOR
    /// </summary>
    public bool GrupoSistema { get; set; } = false;

    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? DataAtualizacao { get; set; }

    /// <summary>
    /// Usuários (PW~Usuarios) vinculados a este grupo
    /// </summary>
    [InverseProperty(nameof(PwUsuario.GrupoUsuarioNavigation))]
    public virtual ICollection<PwUsuario> Usuarios { get; set; } = new List<PwUsuario>();
}

