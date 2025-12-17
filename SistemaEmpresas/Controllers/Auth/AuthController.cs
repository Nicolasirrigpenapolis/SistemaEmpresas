using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Services.Auth;
using System.Security.Claims;

namespace SistemaEmpresas.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        _authService = authService;
        _logger = logger;
        _environment = environment;
        _configuration = configuration;
    }

    /// <summary>
    /// Realiza o login do usuário
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            // Validação automática do ModelState (Data Annotations)
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                { 
                    sucesso = false,
                    mensagem = "Dados inválidos",
                    erros = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var response = await _authService.LoginAsync(
                request.Usuario,
                request.Senha,
                request.DominioTenant);

            _logger.LogInformation("Login realizado com sucesso para usuário: {Usuario}", request.Usuario);

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Falha na autenticação: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar login");
            return StatusCode(500, new { message = "Erro ao realizar login" });
        }
    }

    /// <summary>
    /// Renova o token usando refresh token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new { message = "Refresh token é obrigatório" });
            }

            var response = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao renovar token");
            return StatusCode(500, new { message = "Erro ao renovar token" });
        }
    }

    /// <summary>
    /// Retorna informações do usuário autenticado
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public ActionResult<object> GetUsuarioAtual()
    {
        try
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            var grupo = User.FindFirst(ClaimTypes.Role)?.Value;
            var tenantId = User.FindFirst("TenantId")?.Value;
            var tenantNome = User.FindFirst("TenantNome")?.Value;
            var tenantDominio = User.FindFirst("TenantDominio")?.Value;

            return Ok(new
            {
                usuario,
                grupo,
                tenant = new
                {
                    id = tenantId,
                    nome = tenantNome,
                    dominio = tenantDominio
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário atual");
            return StatusCode(500, new { message = "Erro ao obter usuário atual" });
        }
    }

    /// <summary>
    /// Altera a senha do usuário
    /// </summary>
    [HttpPut("alterar-senha")]
    [Authorize]
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDto request)
    {
        try
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            var tenantDominio = User.FindFirst("TenantDominio")?.Value;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(tenantDominio))
            {
                return Unauthorized(new { message = "Usuário não autenticado" });
            }

            // Validação automática do ModelState (Data Annotations)
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                { 
                    sucesso = false,
                    mensagem = "Dados inválidos",
                    erros = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            await _authService.AlterarSenhaAsync(
                usuario,
                request.SenhaAtual,
                request.SenhaNova,
                tenantDominio);

            return Ok(new { message = "Senha alterada com sucesso" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao alterar senha");
            return StatusCode(500, new { message = "Erro ao alterar senha" });
        }
    }

    /// <summary>
    /// Logout (remove refresh token do cache)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        try
        {
            // Em uma implementação completa, removeria o refresh token do cache
            // Por enquanto, apenas retorna sucesso
            // O token JWT continuará válido até expirar (stateless)
            
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            _logger.LogInformation("Logout realizado para usuário: {Usuario}", usuario);

            return Ok(new { message = "Logout realizado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar logout");
            return StatusCode(500, new { message = "Erro ao realizar logout" });
        }
    }
}
