using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.DTOs;

/// <summary>
/// Request para alterar senha
/// </summary>
public class AlterarSenhaDto
{
    [Required(ErrorMessage = "Senha atual é obrigatória")]
    [MinLength(3, ErrorMessage = "Senha atual deve ter no mínimo 3 caracteres")]
    public string SenhaAtual { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha nova é obrigatória")]
    [MinLength(4, ErrorMessage = "Senha nova deve ter no mínimo 4 caracteres")]
    [MaxLength(100, ErrorMessage = "Senha nova deve ter no máximo 100 caracteres")]
    public string SenhaNova { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
    [Compare("SenhaNova", ErrorMessage = "A confirmação de senha não confere")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}
