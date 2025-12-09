using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEmpresas.Models;

/// <summary>
/// Template de permissões pré-definido.
/// Facilita a criação de novos grupos com permissões padrão.
/// </summary>
[Table("PermissoesTemplate")]
public class PermissoesTemplate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Nome do template (ex: "Administrador", "Vendedor", "Comprador")
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Nome { get; set; } = null!;

    /// <summary>
    /// Descrição do template
    /// </summary>
    [StringLength(200)]
    public string? Descricao { get; set; }

    /// <summary>
    /// Indica se é um template padrão do sistema (não pode ser excluído/editado)
    /// </summary>
    public bool IsPadrao { get; set; } = false;

    /// <summary>
    /// Data de criação do template
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    /// <summary>
    /// Detalhes das permissões por tela
    /// </summary>
    [InverseProperty("Template")]
    public virtual ICollection<PermissoesTemplateDetalhe> Detalhes { get; set; } = new List<PermissoesTemplateDetalhe>();
}
