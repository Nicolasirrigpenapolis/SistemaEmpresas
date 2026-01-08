using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.Features.Auth.Dtos;

/// <summary>
/// Request para login
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Nome de usuário (PW~Nome)
    /// </summary>
    [Required(ErrorMessage = "Usuário é obrigatório")]
    [MinLength(3, ErrorMessage = "Usuário deve ter no mínimo 3 caracteres")]
    [MaxLength(100, ErrorMessage = "Usuário deve ter no máximo 100 caracteres")]
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário (PW~Senha)
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(3, ErrorMessage = "Senha deve ter no mínimo 3 caracteres")]
    [MaxLength(100, ErrorMessage = "Senha deve ter no máximo 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Domínio do tenant para login
    /// </summary>
    [Required(ErrorMessage = "Domínio do tenant é obrigatório")]
    [MinLength(3, ErrorMessage = "Domínio deve ter no mínimo 3 caracteres")]
    public string DominioTenant { get; set; } = string.Empty;
}
