using SistemaEmpresas.Services;

namespace SistemaEmpresas.Middleware
{
    /// <summary>
    /// Middleware que identifica e injeta o Tenant no contexto da requisição
    /// </summary>
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantMiddleware> _logger;

        public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            try
            {
                var tenant = tenantService.GetTenantAtual(context);
                
                if (tenant != null)
                {
                    // Injeta o tenant no contexto para uso posterior
                    context.Items["Tenant"] = tenant;
                    _logger.LogDebug("Tenant '{TenantNome}' injetado no contexto da requisição", tenant.Nome);
                }
                else
                {
                    _logger.LogWarning("Nenhum tenant identificado para esta requisição. Host: {Host}", 
                        context.Request.Host.Host);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar tenant no middleware");
            }

            await _next(context);
        }
    }

    /// <summary>
    /// Extensão para facilitar o registro do middleware
    /// </summary>
    public static class TenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TenantMiddleware>();
        }
    }
}
