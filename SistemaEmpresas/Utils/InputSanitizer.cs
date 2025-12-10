using System.Net;
using System.Text.RegularExpressions;

namespace SistemaEmpresas.Utils;

/// <summary>
/// Helper para sanitização de inputs e prevenção de XSS
/// </summary>
public static partial class InputSanitizer
{
    // Padrões perigosos comuns
    private static readonly string[] DangerousPatterns = new[]
    {
        "<script",
        "</script>",
        "javascript:",
        "onclick",
        "onerror",
        "onload",
        "onmouseover",
        "onfocus",
        "onblur",
        "onchange",
        "onsubmit",
        "eval(",
        "expression(",
        "vbscript:",
        "data:text/html",
        "<iframe",
        "</iframe>",
        "<object",
        "</object>",
        "<embed",
        "</embed>",
        "document.cookie",
        "document.write",
        "window.location",
        "innerHTML"
    };

    // Regex para detectar tags HTML (gerado em tempo de compilação)
    [GeneratedRegex(@"<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex HtmlTagRegex();

    // Regex para detectar scripts inline
    [GeneratedRegex(@"on\w+\s*=", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex InlineEventRegex();

    /// <summary>
    /// Sanitiza uma string removendo potenciais ataques XSS
    /// </summary>
    public static string Sanitize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1. HTML encode para neutralizar tags
        var sanitized = WebUtility.HtmlEncode(input);

        return sanitized.Trim();
    }

    /// <summary>
    /// Sanitiza removendo completamente tags HTML (mais restritivo)
    /// </summary>
    public static string StripHtml(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remove todas as tags HTML
        var result = HtmlTagRegex().Replace(input, string.Empty);

        // Remove eventos inline
        result = InlineEventRegex().Replace(result, string.Empty);

        return result.Trim();
    }

    /// <summary>
    /// Verifica se uma string contém conteúdo potencialmente perigoso
    /// </summary>
    public static bool ContainsDangerousContent(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var lowerInput = input.ToLowerInvariant();

        foreach (var pattern in DangerousPatterns)
        {
            if (lowerInput.Contains(pattern.ToLowerInvariant()))
                return true;
        }

        // Verifica eventos inline
        if (InlineEventRegex().IsMatch(input))
            return true;

        return false;
    }

    /// <summary>
    /// Sanitiza um email (remove caracteres perigosos mantendo formato válido)
    /// </summary>
    public static string SanitizeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;

        // Remove espaços e caracteres perigosos
        var sanitized = email.Trim().ToLowerInvariant();
        
        // Remove qualquer coisa que não seja válido em email
        sanitized = Regex.Replace(sanitized, @"[^\w.@+-]", string.Empty);

        return sanitized;
    }

    /// <summary>
    /// Sanitiza um nome (permite apenas letras, espaços, acentos e alguns caracteres)
    /// </summary>
    public static string SanitizeName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        // Permite letras (incluindo acentuadas), espaços, hífen e apóstrofo
        var sanitized = Regex.Replace(name, @"[^\p{L}\s\-']", string.Empty);

        return sanitized.Trim();
    }

    /// <summary>
    /// Sanitiza um número (permite apenas dígitos)
    /// </summary>
    public static string SanitizeNumeric(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return Regex.Replace(input, @"[^\d]", string.Empty);
    }

    /// <summary>
    /// Sanitiza um telefone (permite apenas dígitos, parênteses, hífen e espaços)
    /// </summary>
    public static string SanitizePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return string.Empty;

        return Regex.Replace(phone, @"[^\d\s\-\(\)\+]", string.Empty).Trim();
    }

    /// <summary>
    /// Sanitiza um CPF/CNPJ (mantém apenas dígitos)
    /// </summary>
    public static string SanitizeCpfCnpj(string? value)
    {
        return SanitizeNumeric(value);
    }

    /// <summary>
    /// Sanitiza uma placa de veículo
    /// </summary>
    public static string SanitizePlaca(string? placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            return string.Empty;

        // Permite apenas letras e números
        return Regex.Replace(placa, @"[^A-Za-z0-9]", string.Empty).ToUpperInvariant();
    }

    /// <summary>
    /// Limita o tamanho de uma string
    /// </summary>
    public static string Truncate(string? input, int maxLength)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return input.Length <= maxLength ? input : input[..maxLength];
    }

    /// <summary>
    /// Sanitiza e trunca uma string
    /// </summary>
    public static string SanitizeAndTruncate(string? input, int maxLength)
    {
        var sanitized = Sanitize(input);
        return Truncate(sanitized, maxLength);
    }
}
