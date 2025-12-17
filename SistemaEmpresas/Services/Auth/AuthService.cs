using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;
using SistemaEmpresas.Services.Seguranca;

namespace SistemaEmpresas.Services.Auth;

public class AuthService : IAuthService
{
    private readonly TenantDbContext _tenantDb;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AuthService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogSegurancaService _logSeguranca;

    public AuthService(
        TenantDbContext tenantDb,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IMemoryCache cache,
        ILogger<AuthService> logger,
        IServiceProvider serviceProvider,
        ILogSegurancaService logSeguranca)
    {
        _tenantDb = tenantDb;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _cache = cache;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _logSeguranca = logSeguranca;
    }

    private string ObterEnderecoIP() => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "";

    public async Task<LoginResponseDto> LoginAsync(string usuario, string senha, string dominioTenant)
    {
        try
        {
            _logger.LogInformation("Tentativa de login para usuário: {Usuario} no tenant: {Tenant}", usuario, dominioTenant);

            // 1. Validar tenant
            var tenant = await _tenantDb.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Dominio == dominioTenant && t.Ativo);

            if (tenant == null)
            {
                _logger.LogWarning("Tenant não encontrado ou inativo: {Tenant}", dominioTenant);
                await _logSeguranca.LogTentativaLoginAsync(usuario, dominioTenant, false, "Tenant não encontrado ou inativo", ObterEnderecoIP());
                throw new UnauthorizedAccessException("Empresa não encontrada ou inativa");
            }

            // ============================================
            // ADMIN HARDCODED - Acesso master ao sistema
            // Usuario: admin / Senha: conectairrig@
            // ============================================
            if (usuario.Equals("admin", StringComparison.OrdinalIgnoreCase) && senha == "conectairrig@")
            {
                _logger.LogInformation("Login com usuário ADMIN (hardcoded)");
                
                // Log de segurança - Login admin
                await _logSeguranca.LogTentativaLoginAsync("admin", dominioTenant, true, null, ObterEnderecoIP());
                
                var adminToken = GerarJwtTokenAdmin(tenant);
                var adminRefreshToken = Guid.NewGuid().ToString();
                
                _cache.Set($"refresh_{adminRefreshToken}", new
                {
                    UsuarioCodificado = "ADMIN",
                    UsuarioNome = "admin",
                    TenantId = tenant.Id,
                    TenantDominio = tenant.Dominio
                }, TimeSpan.FromDays(7));

                return new LoginResponseDto
                {
                    Token = adminToken,
                    RefreshToken = adminRefreshToken,
                    Usuario = new UsuarioDto
                    {
                        Nome = "admin",
                        Grupo = "PROGRAMADOR",
                        Observacoes = "Administrador Master do Sistema",
                        Permissoes = new List<PermissaoDto>()
                    },
                    Tenant = new TenantDto
                    {
                        Id = tenant.Id,
                        Nome = tenant.Nome,
                        Dominio = tenant.Dominio
                    },
                    Expiracao = DateTime.UtcNow.AddHours(8)
                };
            }

            // 2. Criar contexto com o tenant
            // Injetar tenant no HttpContext para o AppDbContext usar
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Items["Tenant"] = tenant;
            }

            // 3. Buscar usuário no banco do tenant
            using var scope = _serviceProvider.CreateScope();
            var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // IMPORTANTE: Nomes e senhas estão codificados em Base64 no banco (compatibilidade VB6)
            // Precisamos buscar todos os usuários e descriptografar para encontrar o correto
            var todosUsuarios = await appDb.PwUsuarios
                .AsNoTracking()
                .Include(u => u.GrupoUsuarioNavigation) // Usa SOMENTE a nova tabela de grupos
                .ToListAsync();

            PwUsuario? pwUsuario = null;
            _logger.LogInformation("Total de usuários no banco: {Total}", todosUsuarios.Count);
            
            foreach (var u in todosUsuarios)
            {
                try
                {
                    var nomeDecriptado = VB6CryptoService.Decripta(u.PwNome).TrimEnd('+');
                    _logger.LogInformation("Usuario codificado: {Encoded} -> Decodificado: {Decoded}", 
                        u.PwNome.Substring(0, Math.Min(20, u.PwNome.Length)) + "...", nomeDecriptado);
                    
                    if (string.Equals(nomeDecriptado, usuario, StringComparison.OrdinalIgnoreCase))
                    {
                        pwUsuario = u;
                        _logger.LogInformation("Usuario encontrado! Nome: {Nome}", nomeDecriptado);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Erro ao descriptografar usuário: {Nome}", u.PwNome.Substring(0, Math.Min(10, u.PwNome.Length)));
                    continue;
                }
            }

            if (pwUsuario == null)
            {
                _logger.LogWarning("Usuário não encontrado: {Usuario}", usuario);
                await _logSeguranca.LogTentativaLoginAsync(usuario, dominioTenant, false, "Usuário não encontrado", ObterEnderecoIP());
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            // 4. Validar senha usando descriptografia VB6
            bool senhaValida = VB6CryptoService.ValidatePassword(senha, pwUsuario.PwSenha);
            
            if (!senhaValida)
            {
                _logger.LogWarning("Senha inválida para usuário: {Usuario}", usuario);
                await _logSeguranca.LogTentativaLoginAsync(usuario, dominioTenant, false, "Senha inválida", ObterEnderecoIP());
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            var nomeReal = VB6CryptoService.Decripta(pwUsuario.PwNome).TrimEnd('+');
            if (string.IsNullOrWhiteSpace(nomeReal))
            {
                nomeReal = usuario;
            }

            // Determinar o grupo do usuário - SOMENTE da nova tabela GrupoUsuario
            if (pwUsuario.GrupoUsuarioNavigation == null || !pwUsuario.GrupoUsuarioId.HasValue)
            {
                _logger.LogWarning("Usuário {Usuario} não possui grupo definido na tabela GrupoUsuario", nomeReal);
                throw new UnauthorizedAccessException("Usuário sem grupo definido. Entre em contato com o departamento de TI.");
            }

            var grupoToken = pwUsuario.GrupoUsuarioNavigation.Nome.ToUpper();

            _logger.LogInformation("Usuário autenticado com sucesso: {Usuario} - Grupo: {Grupo}", 
                nomeReal, grupoToken);

            // Log de segurança - Login bem-sucedido
            await _logSeguranca.LogTentativaLoginAsync(nomeReal, dominioTenant, true, null, ObterEnderecoIP());

            // Buscar permissões da tabela PermissoesTela (nova tabela de permissões)
            var permissoes = await appDb.PermissoesTelas
                .AsNoTracking()
                .Where(p => p.Grupo == grupoToken)
                .Select(p => new PermissaoDto
                {
                    Projeto = p.Modulo,
                    Tabela = p.Tela,
                    // Formato "1111" = Consultar, Incluir, Alterar, Excluir
                    Permissoes = $"{(p.Consultar ? "1" : "0")}{(p.Incluir ? "1" : "0")}{(p.Alterar ? "1" : "0")}{(p.Excluir ? "1" : "0")}"
                })
                .ToListAsync();

            _logger.LogInformation("Permissões carregadas para grupo {Grupo}: {Count} telas", grupoToken, permissoes.Count);

            // Gerar JWT token
            var token = GerarJwtToken(pwUsuario, tenant, nomeReal, grupoToken);
            var expiracao = DateTime.UtcNow.AddHours(
                _configuration.GetValue<int>("Jwt:ExpiracaoHoras", 1));

            // 6. Gerar refresh token e guardar em cache
            var refreshToken = Guid.NewGuid().ToString();
            _cache.Set($"refresh_{refreshToken}", new
            {
                UsuarioCodificado = pwUsuario.PwNome,
                UsuarioNome = nomeReal,
                TenantId = tenant.Id,
                TenantDominio = tenant.Dominio
            }, TimeSpan.FromDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpiracaoDias", 7)));

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Usuario = new UsuarioDto
                {
                    Nome = nomeReal,
                    Grupo = grupoToken,
                    Email = pwUsuario.PwEmail,
                    Observacoes = pwUsuario.PwObs,
                    Permissoes = permissoes
                },
                Tenant = new TenantDto
                {
                    Id = tenant.Id,
                    Nome = tenant.Nome,
                    Dominio = tenant.Dominio
                },
                Expiracao = expiracao
            };
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar login para usuário: {Usuario}", usuario);
            throw new Exception("Erro ao realizar login", ex);
        }
    }

    public Task<UsuarioDto?> ValidarTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey não configurado"));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var usuario = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var grupo = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            return Task.FromResult<UsuarioDto?>(new UsuarioDto
            {
                Nome = usuario,
                Grupo = grupo
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token inválido ou expirado");
            return Task.FromResult<UsuarioDto?>(null);
        }
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        if (!_cache.TryGetValue($"refresh_{refreshToken}", out dynamic? cachedData) || cachedData == null)
        {
            _logger.LogWarning("Refresh token inválido ou expirado");
            throw new UnauthorizedAccessException("Refresh token inválido ou expirado");
        }

        // Extrai valores antes de usar (evita warning de null reference)
    string usuarioCodificado = cachedData!.UsuarioCodificado;
    string usuarioNome = cachedData.UsuarioNome;
        string tenantDominio = cachedData.TenantDominio;
        int tenantId = (int)cachedData.TenantId;

    _logger.LogInformation("Renovando token para usuário: {Usuario}", usuarioNome);

        // Buscar senha não é ideal, mas como é texto plano no legado, vamos buscar
        // Em produção, considere armazenar hash ou ter um mecanismo melhor
        var tenant = await _tenantDb.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tenantId && t.Ativo);

        if (tenant == null)
        {
            throw new UnauthorizedAccessException("Tenant não encontrado");
        }

        if (_httpContextAccessor.HttpContext != null)
        {
            _httpContextAccessor.HttpContext.Items["Tenant"] = tenant;
        }

        using var scope = _serviceProvider.CreateScope();
        var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var pwUsuario = await appDb.PwUsuarios
            .AsNoTracking()
            .Include(u => u.GrupoUsuarioNavigation) // Usa SOMENTE a nova tabela de grupos
            .FirstOrDefaultAsync(u => u.PwNome == usuarioCodificado);

        if (pwUsuario == null)
        {
            throw new UnauthorizedAccessException("Usuário não encontrado");
        }

        var nomeReal = VB6CryptoService.Decripta(pwUsuario.PwNome).TrimEnd('+');
        if (string.IsNullOrWhiteSpace(nomeReal))
        {
            nomeReal = usuarioNome ?? string.Empty;
        }
        if (string.IsNullOrWhiteSpace(nomeReal))
        {
            nomeReal = usuarioCodificado;
        }

        // Determinar o grupo do usuário - SOMENTE da nova tabela GrupoUsuario
        if (pwUsuario.GrupoUsuarioNavigation == null || !pwUsuario.GrupoUsuarioId.HasValue)
        {
            _logger.LogWarning("Refresh: Usuário {Usuario} não possui grupo definido na tabela GrupoUsuario", nomeReal);
            throw new UnauthorizedAccessException("Usuário sem grupo definido. Entre em contato com o departamento de TI.");
        }

        var grupoToken = pwUsuario.GrupoUsuarioNavigation.Nome;

        // Buscar permissões da tabela PermissoesTela (nova tabela de permissões)
        var permissoes = await appDb.PermissoesTelas
            .AsNoTracking()
            .Where(p => p.Grupo == grupoToken)
            .Select(p => new PermissaoDto
            {
                Projeto = p.Modulo,
                Tabela = p.Tela,
                // Formato "1111" = Consultar, Incluir, Alterar, Excluir
                Permissoes = $"{(p.Consultar ? "1" : "0")}{(p.Incluir ? "1" : "0")}{(p.Alterar ? "1" : "0")}{(p.Excluir ? "1" : "0")}"
            })
            .ToListAsync();

        // Gerar novo token
        var token = GerarJwtToken(pwUsuario, tenant, nomeReal, grupoToken);
        var expiracao = DateTime.UtcNow.AddHours(
            _configuration.GetValue<int>("Jwt:ExpiracaoHoras", 1));

        // Gerar novo refresh token
        var novoRefreshToken = Guid.NewGuid().ToString();
        _cache.Remove($"refresh_{refreshToken}"); // Remove o antigo
        _cache.Set($"refresh_{novoRefreshToken}", new
        {
            UsuarioCodificado = pwUsuario.PwNome,
            UsuarioNome = nomeReal,
            TenantId = tenant.Id,
            TenantDominio = tenant.Dominio
        }, TimeSpan.FromDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpiracaoDias", 7)));

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = novoRefreshToken,
            Usuario = new UsuarioDto
            {
                Nome = nomeReal,
                Grupo = grupoToken,
                Email = pwUsuario.PwEmail,
                Observacoes = pwUsuario.PwObs,
                Permissoes = permissoes
            },
            Tenant = new TenantDto
            {
                Id = tenant.Id,
                Nome = tenant.Nome,
                Dominio = tenant.Dominio
            },
            Expiracao = expiracao
        };
    }

    public async Task AlterarSenhaAsync(string usuario, string senhaAtual, string senhaNova, string dominioTenant)
    {
        try
        {
            var tenant = await _tenantDb.Tenants
                .FirstOrDefaultAsync(t => t.Dominio == dominioTenant && t.Ativo);

            if (tenant == null)
            {
                throw new UnauthorizedAccessException("Tenant não encontrado");
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Items["Tenant"] = tenant;
            }

            using var scope = _serviceProvider.CreateScope();
            var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pwUsuario = await appDb.PwUsuarios
                .FirstOrDefaultAsync(u => u.PwNome == usuario && u.PwSenha == senhaAtual);

            if (pwUsuario == null)
            {
                throw new UnauthorizedAccessException("Senha atual incorreta");
            }

            pwUsuario.PwSenha = senhaNova;
            await appDb.SaveChangesAsync();

            _logger.LogInformation("Senha alterada com sucesso para usuário: {Usuario}", usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao alterar senha para usuário: {Usuario}", usuario);
            throw;
        }
    }

    
    private string GerarJwtToken(PwUsuario usuario, Tenant tenant, string nomeDecodificado, string grupoDecodificado)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey nao configurado"));

        var grupoNormalizado = AdminGroupHelper.Normalize(grupoDecodificado);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, nomeDecodificado),
            new Claim(ClaimTypes.Role, grupoNormalizado),
            new Claim("TenantId", tenant.Id.ToString()),
            new Claim("TenantDominio", tenant.Dominio),
            new Claim("TenantNome", tenant.Nome)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_configuration.GetValue<int>("Jwt:ExpiracaoHoras", 1)),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Tenta descriptografar o nome do grupo; em falha retorna o valor original.
    /// </summary>
    private static string TryDecryptGroup(string encryptedGroup)
    {
        try
        {
            var decrypted = VB6CryptoService.Decripta(encryptedGroup).TrimEnd('+').Trim();
            return string.IsNullOrWhiteSpace(decrypted) ? encryptedGroup : decrypted;
        }
        catch
        {
            return encryptedGroup;
        }
    }

    /// <summary>
    /// Gera JWT token para o usuário admin hardcoded
    /// </summary>
    private string GerarJwtTokenAdmin(Tenant tenant)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey não configurado"));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "PROGRAMADOR"),
            new Claim("TenantId", tenant.Id.ToString()),
            new Claim("TenantDominio", tenant.Dominio),
            new Claim("TenantNome", tenant.Nome)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8), // Admin tem 8 horas de sessão
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
