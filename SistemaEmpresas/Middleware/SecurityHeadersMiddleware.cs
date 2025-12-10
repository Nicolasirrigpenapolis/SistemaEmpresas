namespace SistemaEmpresas.Middleware;

/// <summary>
/// Middleware para adicionar headers de segurança HTTP
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Previne que o navegador faça MIME-sniffing
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";

        // Previne que a página seja carregada em um iframe (proteção contra clickjacking)
        context.Response.Headers["X-Frame-Options"] = "DENY";

        // Habilita o filtro XSS do navegador
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

        // Referrer Policy - controla informações enviadas no header Referer
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Permissions Policy - restringe recursos do navegador
        context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";

        // Content-Security-Policy (CSP) - mais permissivo para funcionamento do sistema
        // Em produção, você pode tornar mais restritivo conforme necessário
        context.Response.Headers["Content-Security-Policy"] = 
            "default-src 'self'; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " + // Necessário para React/Vite
            "style-src 'self' 'unsafe-inline'; " + // Necessário para TailwindCSS
            "img-src 'self' data: blob: https:; " +
            "font-src 'self' data:; " +
            "connect-src 'self' https://cff.svrs.rs.gov.br; " + // API externa
            "frame-ancestors 'none'; " +
            "form-action 'self'; " +
            "base-uri 'self';";

        // Strict-Transport-Security (HSTS) - força HTTPS
        // Apenas em produção ou quando usando HTTPS
        if (context.Request.IsHttps)
        {
            // max-age: 1 ano, includeSubDomains
            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
        }

        // Cache-Control para APIs - não cachear por padrão
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, proxy-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
        }

        await _next(context);
    }
}

/// <summary>
/// Extension method para facilitar o uso do middleware
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}
