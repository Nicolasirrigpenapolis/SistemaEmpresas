using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.Utils;

/// <summary>
/// Atributo de validação que verifica se o campo não contém conteúdo potencialmente perigoso (XSS)
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class NoXssAttribute : ValidationAttribute
{
    public NoXssAttribute() : base("O campo {0} contém caracteres inválidos.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var stringValue = value.ToString();
        if (string.IsNullOrWhiteSpace(stringValue))
            return ValidationResult.Success;

        if (InputSanitizer.ContainsDangerousContent(stringValue))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                new[] { validationContext.MemberName ?? string.Empty });
        }

        return ValidationResult.Success;
    }
}

/// <summary>
/// Atributo que sanitiza automaticamente o valor do campo (remove XSS)
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class SanitizeInputAttribute : ValidationAttribute
{
    public SanitizeType SanitizeType { get; set; } = SanitizeType.General;
    public int MaxLength { get; set; } = 0;

    public SanitizeInputAttribute() : base()
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var stringValue = value.ToString();
        if (string.IsNullOrWhiteSpace(stringValue))
            return ValidationResult.Success;

        // Sanitiza baseado no tipo
        var sanitized = SanitizeType switch
        {
            SanitizeType.Email => InputSanitizer.SanitizeEmail(stringValue),
            SanitizeType.Name => InputSanitizer.SanitizeName(stringValue),
            SanitizeType.Numeric => InputSanitizer.SanitizeNumeric(stringValue),
            SanitizeType.Phone => InputSanitizer.SanitizePhone(stringValue),
            SanitizeType.CpfCnpj => InputSanitizer.SanitizeCpfCnpj(stringValue),
            SanitizeType.Placa => InputSanitizer.SanitizePlaca(stringValue),
            SanitizeType.StripHtml => InputSanitizer.StripHtml(stringValue),
            _ => InputSanitizer.Sanitize(stringValue)
        };

        // Aplica truncate se maxLength > 0
        if (MaxLength > 0)
        {
            sanitized = InputSanitizer.Truncate(sanitized, MaxLength);
        }

        // Atualiza o valor da propriedade
        var property = validationContext.ObjectType.GetProperty(validationContext.MemberName ?? string.Empty);
        if (property != null && property.CanWrite)
        {
            property.SetValue(validationContext.ObjectInstance, sanitized);
        }

        return ValidationResult.Success;
    }
}

/// <summary>
/// Tipos de sanitização disponíveis
/// </summary>
public enum SanitizeType
{
    General,
    Email,
    Name,
    Numeric,
    Phone,
    CpfCnpj,
    Placa,
    StripHtml
}

/// <summary>
/// Atributo para validar CPF
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class CpfValidoAttribute : ValidationAttribute
{
    public CpfValidoAttribute() : base("CPF inválido.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success; // Permite nulo/vazio (use [Required] se obrigatório)

        var cpf = InputSanitizer.SanitizeNumeric(value.ToString());

        if (!ValidarCpf(cpf))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                new[] { validationContext.MemberName ?? string.Empty });
        }

        return ValidationResult.Success;
    }

    private static bool ValidarCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
            return false;

        // Verifica sequências inválidas
        if (cpf.Distinct().Count() == 1)
            return false;

        // Cálculo do primeiro dígito verificador
        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cpf[9].ToString()) != digito1)
            return false;

        // Cálculo do segundo dígito verificador
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return int.Parse(cpf[10].ToString()) == digito2;
    }
}

/// <summary>
/// Atributo para validar CNPJ
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class CnpjValidoAttribute : ValidationAttribute
{
    public CnpjValidoAttribute() : base("CNPJ inválido.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;

        var cnpj = InputSanitizer.SanitizeNumeric(value.ToString());

        if (!ValidarCnpj(cnpj))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                new[] { validationContext.MemberName ?? string.Empty });
        }

        return ValidationResult.Success;
    }

    private static bool ValidarCnpj(string cnpj)
    {
        if (string.IsNullOrEmpty(cnpj) || cnpj.Length != 14)
            return false;

        // Verifica sequências inválidas
        if (cnpj.Distinct().Count() == 1)
            return false;

        // Cálculo do primeiro dígito verificador
        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;
        for (int i = 0; i < 12; i++)
            soma += int.Parse(cnpj[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cnpj[12].ToString()) != digito1)
            return false;

        // Cálculo do segundo dígito verificador
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += int.Parse(cnpj[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return int.Parse(cnpj[13].ToString()) == digito2;
    }
}

/// <summary>
/// Atributo para validar placa de veículo (padrão antigo ou Mercosul)
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class PlacaVeiculoValidaAttribute : ValidationAttribute
{
    public PlacaVeiculoValidaAttribute() : base("Placa de veículo inválida.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;

        var placa = InputSanitizer.SanitizePlaca(value.ToString());

        // Placa antiga: AAA1234 (3 letras + 4 números)
        // Placa Mercosul: AAA1A23 (3 letras + 1 número + 1 letra + 2 números)
        var regexAntiga = new System.Text.RegularExpressions.Regex(@"^[A-Z]{3}\d{4}$");
        var regexMercosul = new System.Text.RegularExpressions.Regex(@"^[A-Z]{3}\d[A-Z]\d{2}$");

        if (!regexAntiga.IsMatch(placa) && !regexMercosul.IsMatch(placa))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                new[] { validationContext.MemberName ?? string.Empty });
        }

        return ValidationResult.Success;
    }
}
