using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SistemaEmpresas.Middleware;

/// <summary>
/// Filtro de ação que valida automaticamente todos os inputs de string contra XSS
/// </summary>
public class AntiXssValidationFilter : IActionFilter
{
    private readonly ILogger<AntiXssValidationFilter> _logger;

    public AntiXssValidationFilter(ILogger<AntiXssValidationFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value == null)
                continue;

            var type = argument.Value.GetType();

            // Para strings simples
            if (type == typeof(string))
            {
                var value = argument.Value as string;
                if (Utils.InputSanitizer.ContainsDangerousContent(value))
                {
                    _logger.LogWarning(
                        "⚠️ XSS DETECTADO: Conteúdo perigoso detectado no parâmetro '{Parameter}'. Requisição bloqueada. IP: {IP}",
                        argument.Key,
                        context.HttpContext.Connection.RemoteIpAddress);

                    context.Result = new BadRequestObjectResult(new
                    {
                        mensagem = "Entrada inválida detectada. Por favor, remova caracteres especiais.",
                        parametro = argument.Key
                    });
                    return;
                }
            }
            // Para objetos complexos (DTOs)
            else if (!type.IsPrimitive && type != typeof(decimal) && type != typeof(DateTime))
            {
                ValidateObject(argument.Value, argument.Key, context);
                if (context.Result != null)
                    return;
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Não precisa fazer nada após a execução
    }

    private void ValidateObject(object obj, string prefix, ActionExecutingContext context)
    {
        if (obj == null) return;

        var properties = obj.GetType().GetProperties();

        foreach (var prop in properties)
        {
            if (!prop.CanRead) continue;

            try
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;

                var propType = prop.PropertyType;
                var fullPath = $"{prefix}.{prop.Name}";

                // Valida strings
                if (propType == typeof(string))
                {
                    var stringValue = value as string;
                    if (Utils.InputSanitizer.ContainsDangerousContent(stringValue))
                    {
                        _logger.LogWarning(
                            "⚠️ XSS DETECTADO: Conteúdo perigoso detectado na propriedade '{Property}'. Requisição bloqueada. IP: {IP}",
                            fullPath,
                            context.HttpContext.Connection.RemoteIpAddress);

                        context.Result = new BadRequestObjectResult(new
                        {
                            mensagem = "Entrada inválida detectada. Por favor, remova caracteres especiais.",
                            campo = prop.Name
                        });
                        return;
                    }
                }
                // Recursão para objetos aninhados
                else if (!propType.IsPrimitive && propType != typeof(decimal) && propType != typeof(DateTime) && !propType.IsEnum)
                {
                    // Evita loops infinitos em coleções
                    if (propType.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(propType))
                    {
                        if (value is System.Collections.IEnumerable enumerable)
                        {
                            int index = 0;
                            foreach (var item in enumerable)
                            {
                                if (item is string strItem)
                                {
                                    if (Utils.InputSanitizer.ContainsDangerousContent(strItem))
                                    {
                                        _logger.LogWarning(
                                            "⚠️ XSS DETECTADO: Conteúdo perigoso detectado em '{Property}[{Index}]'. IP: {IP}",
                                            fullPath, index, context.HttpContext.Connection.RemoteIpAddress);

                                        context.Result = new BadRequestObjectResult(new
                                        {
                                            mensagem = "Entrada inválida detectada. Por favor, remova caracteres especiais.",
                                            campo = $"{prop.Name}[{index}]"
                                        });
                                        return;
                                    }
                                }
                                else if (item != null && !item.GetType().IsPrimitive)
                                {
                                    ValidateObject(item, $"{fullPath}[{index}]", context);
                                    if (context.Result != null) return;
                                }
                                index++;
                            }
                        }
                    }
                    else if (propType.IsClass)
                    {
                        ValidateObject(value, fullPath, context);
                        if (context.Result != null) return;
                    }
                }
            }
            catch
            {
                // Ignora erros de reflexão
            }
        }
    }
}
