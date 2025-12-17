using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services.Auth;

public interface IAuthService
{
    /// <summary>
    /// Realiza o login do usuário
    /// </summary>
    Task<LoginResponseDto> LoginAsync(string usuario, string senha, string dominioTenant);

    /// <summary>
    /// Valida um token JWT
    /// </summary>
    Task<UsuarioDto?> ValidarTokenAsync(string token);

    /// <summary>
    /// Renova o token usando refresh token
    /// </summary>
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Altera a senha do usuário
    /// </summary>
    Task AlterarSenhaAsync(string usuario, string senhaAtual, string senhaNova, string dominioTenant);
}
