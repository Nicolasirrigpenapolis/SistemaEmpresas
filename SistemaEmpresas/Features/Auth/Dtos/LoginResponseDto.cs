using SistemaEmpresas.Models;

namespace SistemaEmpresas.Features.Auth.Dtos;

/// <summary>
/// Response do login com token JWT
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Token JWT para autenticação nas requisições
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Refresh token para renovar o token principal
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Dados do usuário autenticado
    /// </summary>
    public UsuarioDto Usuario { get; set; } = null!;

    /// <summary>
    /// Dados do tenant
    /// </summary>
    public TenantDto Tenant { get; set; } = null!;

    /// <summary>
    /// Data de expiração do token
    /// </summary>
    public DateTime Expiracao { get; set; }
}

/// <summary>
/// Dados do usuário autenticado
/// </summary>
public class UsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Observacoes { get; set; }
    public List<PermissaoDto> Permissoes { get; set; } = new();
}

/// <summary>
/// Dados do tenant
/// </summary>
public class TenantDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Dominio { get; set; } = string.Empty;
}

/// <summary>
/// Permissão do usuário
/// </summary>
public class PermissaoDto
{
    public string Projeto { get; set; } = string.Empty;
    public string Tabela { get; set; } = string.Empty;
    public string Permissoes { get; set; } = string.Empty;
}
