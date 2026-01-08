using System.Text.RegularExpressions;

namespace SistemaEmpresas.Utils;

/// <summary>
/// Validador de documentos brasileiros (CPF e CNPJ)
/// </summary>
public static class DocumentoValidator
{
    /// <summary>
    /// Valida um documento (CPF ou CNPJ) baseado no tipo
    /// </summary>
    /// <param name="documento">O documento a ser validado</param>
    /// <param name="tipoPessoa">0 = Pessoa Física (CPF), 1 = Pessoa Jurídica (CNPJ)</param>
    /// <returns>Tuple com valido e mensagem de erro</returns>
    public static (bool valido, string? mensagem) Validar(string? documento, int tipoPessoa)
    {
        var docLimpo = LimparDocumento(documento);
        
        if (string.IsNullOrWhiteSpace(docLimpo))
            return (true, null); // Documento não informado é válido (campo opcional)

        return tipoPessoa switch
        {
            0 => ValidarCpf(docLimpo),
            1 => ValidarCnpj(docLimpo),
            _ => (true, null)
        };
    }

    /// <summary>
    /// Limpa o documento removendo caracteres não numéricos
    /// </summary>
    public static string LimparDocumento(string? documento)
    {
        if (string.IsNullOrWhiteSpace(documento))
            return string.Empty;
        return Regex.Replace(documento, @"\D", "");
    }

    /// <summary>
    /// Valida um CPF
    /// </summary>
    public static (bool valido, string? mensagem) ValidarCpf(string cpf)
    {
        var cpfLimpo = LimparDocumento(cpf);

        if (cpfLimpo.Length != 11)
            return (false, "CPF deve ter 11 dígitos");

        // Verifica se todos os dígitos são iguais
        if (cpfLimpo.Distinct().Count() == 1)
            return (false, "CPF inválido");

        // Calcula primeiro dígito verificador
        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(cpfLimpo[i].ToString()) * (10 - i);
        
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cpfLimpo[9].ToString()) != digito1)
            return (false, "CPF inválido");

        // Calcula segundo dígito verificador
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(cpfLimpo[i].ToString()) * (11 - i);
        
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cpfLimpo[10].ToString()) != digito2)
            return (false, "CPF inválido");

        return (true, null);
    }

    /// <summary>
    /// Valida um CNPJ
    /// </summary>
    public static (bool valido, string? mensagem) ValidarCnpj(string cnpj)
    {
        var cnpjLimpo = LimparDocumento(cnpj);

        if (cnpjLimpo.Length != 14)
            return (false, "CNPJ deve ter 14 dígitos");

        // Verifica se todos os dígitos são iguais
        if (cnpjLimpo.Distinct().Count() == 1)
            return (false, "CNPJ inválido");

        // Primeiro dígito verificador
        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;
        for (int i = 0; i < 12; i++)
            soma += int.Parse(cnpjLimpo[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cnpjLimpo[12].ToString()) != digito1)
            return (false, "CNPJ inválido");

        // Segundo dígito verificador
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += int.Parse(cnpjLimpo[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cnpjLimpo[13].ToString()) != digito2)
            return (false, "CNPJ inválido");

        return (true, null);
    }

    /// <summary>
    /// Verifica se o documento é um CPF válido (boolean simples)
    /// </summary>
    public static bool IsCpfValido(string cpf) => ValidarCpf(cpf).valido;

    /// <summary>
    /// Verifica se o documento é um CNPJ válido (boolean simples)
    /// </summary>
    public static bool IsCnpjValido(string cnpj) => ValidarCnpj(cnpj).valido;
}
