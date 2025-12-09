using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Permissões efetivas de um grupo por tela.
/// Esta tabela armazena as permissões reais aplicadas a cada grupo.
/// </summary>
[Table("PermissoesTela")]
[Index(nameof(Grupo), nameof(Tela), IsUnique = true)]
public class PermissoesTela
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Nome do grupo (valor criptografado igual ao PW~Grupos)
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Grupo { get; set; } = null!;

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
    /// Nome amigável da tela para exibição
    /// </summary>
    [Required]
    [StringLength(100)]
    public string NomeTela { get; set; } = null!;

    /// <summary>
    /// Rota da tela no sistema (ex: "/cadastros/clientes", "/produtos")
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Rota { get; set; } = null!;

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
    /// Ordem de exibição na interface
    /// </summary>
    public int Ordem { get; set; } = 0;
}
