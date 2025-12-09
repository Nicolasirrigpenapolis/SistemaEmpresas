using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Data;

public partial class AppDbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly IConfiguration? _configuration;
    private readonly ILogger<AppDbContext>? _logger;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        ILogger<AppDbContext> logger) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _logger = logger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var tenant = _httpContextAccessor?.HttpContext?.Items["Tenant"] as Tenant;

        if (tenant != null && !string.IsNullOrWhiteSpace(tenant.ConnectionString))
        {
            optionsBuilder.UseSqlServer(tenant.ConnectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(30);
            });
            _logger?.LogDebug(
                "AppDbContext configurado com a connection string do tenant {TenantNome} (ID: {TenantId}).",
                tenant.Nome,
                tenant.Id);
        }
        else if (!optionsBuilder.IsConfigured && _configuration is not null)
        {
            var fallbackConnection = _configuration.GetConnectionString("ConexaoPadrao");
            if (!string.IsNullOrWhiteSpace(fallbackConnection))
            {
                optionsBuilder.UseSqlServer(fallbackConnection, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(30);
                });
                _logger?.LogWarning(
                    "AppDbContext usando connection string padrão por ausência de tenant identificado.");
            }
        }
    }
}
