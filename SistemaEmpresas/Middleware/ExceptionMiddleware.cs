using System.Net;
using System.Text.Json;

namespace SistemaEmpresas.Middleware;

/// <summary>
/// Middleware para tratamento global de exceções
/// Captura todas as exceções não tratadas e retorna respostas padronizadas
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Sucesso = false,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case UnauthorizedAccessException unauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Mensagem = unauthorizedException.Message;
                response.StatusCode = 401;
                _logger.LogWarning("Acesso não autorizado: {Message}", unauthorizedException.Message);
                break;

            case KeyNotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Mensagem = notFoundException.Message;
                response.StatusCode = 404;
                _logger.LogWarning("Recurso não encontrado: {Message}", notFoundException.Message);
                break;

            case ArgumentException argumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Mensagem = argumentException.Message;
                response.StatusCode = 400;
                _logger.LogWarning("Argumento inválido: {Message}", argumentException.Message);
                break;

            case InvalidOperationException invalidOpException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Mensagem = invalidOpException.Message;
                response.StatusCode = 400;
                _logger.LogWarning("Operação inválida: {Message}", invalidOpException.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusCode = 500;
                
                // Em produção, não expor detalhes do erro
                if (_env.IsDevelopment())
                {
                    response.Mensagem = exception.Message;
                    response.StackTrace = exception.StackTrace;
                    response.Detalhes = exception.InnerException?.Message;
                }
                else
                {
                    response.Mensagem = "Ocorreu um erro interno no servidor";
                }

                _logger.LogError(exception, "Erro interno do servidor: {Message}", exception.Message);
                break;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _env.IsDevelopment() // Indent apenas em desenvolvimento
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

/// <summary>
/// Modelo de resposta de erro padronizado
/// </summary>
public class ErrorResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; }
    public string? StackTrace { get; set; }
    public string? Detalhes { get; set; }
}

/// <summary>
/// Extension method para registrar o middleware
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
