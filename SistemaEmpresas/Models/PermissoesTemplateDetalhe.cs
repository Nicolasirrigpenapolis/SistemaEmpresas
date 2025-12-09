using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Detalhe das permissões de um template por tela.
/// </summary>
[Table("PermissoesTemplateDetalhe")]
[Index(nameof(TemplateId), nameof(Tela), IsUnique = true)]
public class PermissoesTemplateDetalhe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// ID do template pai
    /// </summary>
    [Required]
    public int TemplateId { get; set; }

    /// <summary>
    /// Módulo da tela (agrupamento visual: Cadastros, Estoque, Fiscal, Sistema)
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Modulo { get; set; } = null!;

    /// <summary>
    /// Identificador único da tela (ex: "Clientes", "Produtos", "ClassificacaoFiscal")
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Tela { get; set; } = null!;

    /// <summary>
    /// Permissão de consultar/visualizar
    /// </summary>
    public bool Consultar { get; set; } = false;

    /// <summary>
    /// Permissão de incluir novos registros
    /// </summary>
    public bool Incluir { get; set; } = false;

    /// <summary>
    /// Permissão de alterar registros existentes
    /// </summary>
    public bool Alterar { get; set; } = false;

    /// <summary>
    /// Permissão de excluir registros
    /// </summary>
    public bool Excluir { get; set; } = false;

    /// <summary>
    /// Template pai
    /// </summary>
    [ForeignKey("TemplateId")]
    [InverseProperty("Detalhes")]
    public virtual PermissoesTemplate Template { get; set; } = null!;
}
