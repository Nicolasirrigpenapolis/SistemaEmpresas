using System.Text.RegularExpressions;

namespace SistemaEmpresas.Features.Geral.Services;

/// <summary>
/// Serviço de validação de Inscrição Estadual por UF
/// Baseado na validação do sistema legado VB6 (IRRIG.BAS)
/// </summary>
public static class InscricaoEstadualValidator
{
    /// <summary>
    /// Valida Inscrição Estadual por UF
    /// </summary>
    /// <param name="uf">UF do estado (ex: SP, RJ, MG)</param>
    /// <param name="ie">Inscrição Estadual</param>
    /// <returns>Tupla com (válido, mensagem de erro)</returns>
    public static (bool Valido, string Mensagem) Validar(string uf, string ie)
    {
        if (string.IsNullOrWhiteSpace(ie))
            return (false, "Inscrição Estadual não pode ser vazia. Se for isenta, marque o campo 'Isento'.");

        // Remove caracteres não numéricos
        var inscricao = Regex.Replace(ie, @"\D", "");
        
        if (!inscricao.All(char.IsDigit))
            return (false, "Inscrição Estadual deve conter apenas números.");

        if (string.IsNullOrWhiteSpace(uf))
            return (false, "UF não informada.");

        return uf.ToUpper() switch
        {
            "AC" => ValidarAC(inscricao),
            "AL" => ValidarAL(inscricao),
            "AP" => ValidarAP(inscricao),
            "AM" => ValidarPadrao9Digitos(inscricao, "Amazonas"),
            "BA" => ValidarBA(inscricao),
            "CE" => ValidarPadrao9Digitos(inscricao, "Ceará"),
            "DF" => ValidarDF(inscricao),
            "ES" => ValidarPadrao9Digitos(inscricao, "Espírito Santo"),
            "GO" => ValidarGO(inscricao),
            "MA" => ValidarMA(inscricao),
            "MT" => ValidarMT(inscricao),
            "MS" => ValidarMS(inscricao),
            "MG" => ValidarMG(inscricao),
            "PA" => ValidarPA(inscricao),
            "PB" => ValidarPadrao9Digitos(inscricao, "Paraíba"),
            "PR" => ValidarPR(inscricao),
            "PE" => ValidarPE(inscricao),
            "PI" => ValidarPadrao9Digitos(inscricao, "Piauí"),
            "RJ" => ValidarRJ(inscricao),
            "RN" => ValidarRN(inscricao),
            "RS" => ValidarRS(inscricao),
            "RO" => ValidarRO(inscricao),
            "RR" => ValidarRR(inscricao),
            "SC" => ValidarPadrao9Digitos(inscricao, "Santa Catarina"),
            "SP" => ValidarSP(inscricao),
            "SE" => ValidarPadrao9Digitos(inscricao, "Sergipe"),
            "TO" => ValidarTO(inscricao),
            "EX" => (true, ""), // Exterior - não valida
            _ => (false, $"UF '{uf}' não reconhecida.")
        };
    }

    #region Validações por Estado

    private static (bool, string) ValidarAC(string inscricao)
    {
        if (inscricao.Length != 13)
            return (false, "IE do Acre deve ter 13 dígitos.");
        
        if (!inscricao.StartsWith("01"))
            return (false, "IE do Acre deve começar com '01'.");

        // Validação módulo 11 com dois dígitos verificadores
        var baseIe = inscricao[..11];
        int[] pesos1 = { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv1 = CalcularDV(baseIe, pesos1);
        
        baseIe += dv1;
        int[] pesos2 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv2 = CalcularDV(baseIe, pesos2);

        var ieCalculada = baseIe + dv2;
        return ieCalculada == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Acre.");
    }

    private static (bool, string) ValidarAL(string inscricao)
    {
        if (inscricao.Length != 9)
            return (false, "IE de Alagoas deve ter 9 dígitos.");
        
        if (!inscricao.StartsWith("24"))
            return (false, "IE de Alagoas deve começar com '24'.");

        // Tipos válidos: 0, 3, 5, 7, 8
        var tipo = inscricao[2];
        if (!"03578".Contains(tipo))
            return (false, "3º dígito da IE de Alagoas inválido (deve ser 0, 3, 5, 7 ou 8).");

        var baseIe = inscricao[..8];
        int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
        var soma = 0;
        for (int i = 0; i < 8; i++)
            soma += int.Parse(baseIe[i].ToString()) * pesos[i];

        var produto = soma * 10;
        var resto = produto - (produto / 11 * 11);
        var dv = resto == 10 ? 0 : resto;

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE de Alagoas.");
    }

    private static (bool, string) ValidarAP(string inscricao)
    {
        if (inscricao.Length != 9)
            return (false, "IE do Amapá deve ter 9 dígitos.");
        
        if (!inscricao.StartsWith("03"))
            return (false, "IE do Amapá deve começar com '03'.");

        var baseIe = inscricao[..8];
        var baseNum = long.Parse(baseIe);

        int p = 0, d = 0;
        if (baseNum >= 3000001 && baseNum <= 3017000) { p = 5; d = 0; }
        else if (baseNum >= 3017001 && baseNum <= 3019022) { p = 9; d = 1; }
        else if (baseNum >= 3019023) { p = 0; d = 0; }

        int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
        var soma = p;
        for (int i = 0; i < 8; i++)
            soma += int.Parse(baseIe[i].ToString()) * pesos[i];

        var resto = soma % 11;
        var dv = 11 - resto;
        if (dv == 10) dv = 0;
        else if (dv == 11) dv = d;

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Amapá.");
    }

    private static (bool, string) ValidarPadrao9Digitos(string inscricao, string estado)
    {
        if (inscricao.Length != 9)
            return (false, $"IE de {estado} deve ter 9 dígitos.");

        var baseIe = inscricao[..8];
        int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, $"Dígito verificador incorreto para IE de {estado}.");
    }

    private static (bool, string) ValidarBA(string inscricao)
    {
        if (inscricao.Length != 8 && inscricao.Length != 9)
            return (false, "IE da Bahia deve ter 8 ou 9 dígitos.");

        // Validação simplificada - Bahia tem regras complexas
        // Por segurança, aceita se o formato estiver correto
        return (true, "");
    }

    private static (bool, string) ValidarDF(string inscricao)
    {
        if (inscricao.Length != 13)
            return (false, "IE do Distrito Federal deve ter 13 dígitos.");

        if (!inscricao.StartsWith("07"))
            return (false, "IE do Distrito Federal deve começar com '07'.");

        var baseIe = inscricao[..11];
        int[] pesos1 = { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv1 = CalcularDV(baseIe, pesos1);
        
        baseIe += dv1;
        int[] pesos2 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv2 = CalcularDV(baseIe, pesos2);

        return (baseIe + dv2) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Distrito Federal.");
    }

    private static (bool, string) ValidarGO(string inscricao)
    {
        if (inscricao.Length != 9)
            return (false, "IE de Goiás deve ter 9 dígitos.");

        var prefixos = new[] { "10", "11", "15" };
        if (!prefixos.Any(p => inscricao.StartsWith(p)))
            return (false, "IE de Goiás deve começar com '10', '11' ou '15'.");

        return (true, "");
    }

    private static (bool, string) ValidarMA(string inscricao)
    {
        if (inscricao.Length != 9)
            return (false, "IE do Maranhão deve ter 9 dígitos.");

        if (!inscricao.StartsWith("12"))
            return (false, "IE do Maranhão deve começar com '12'.");

        var baseIe = inscricao[..8];
        int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Maranhão.");
    }

    private static (bool, string) ValidarMT(string inscricao)
    {
        if (inscricao.Length != 11)
            return (false, "IE do Mato Grosso deve ter 11 dígitos.");

        var baseIe = inscricao[..10];
        int[] pesos = { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Mato Grosso.");
    }

    private static (bool, string) ValidarMS(string inscricao)
    {
        if (inscricao.Length != 9)
            return (false, "IE do Mato Grosso do Sul deve ter 9 dígitos.");

        if (!inscricao.StartsWith("28"))
            return (false, "IE do Mato Grosso do Sul deve começar com '28'.");

        var baseIe = inscricao[..8];
        int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Mato Grosso do Sul.");
    }

    private static (bool, string) ValidarMG(string inscricao)
    {
        if (inscricao.Length != 13)
            return (false, "IE de Minas Gerais deve ter 13 dígitos.");

        // MG usa validação complexa com módulo 10 e 11
        // Validação simplificada aceita formato correto
        return (true, "");
    }

    private static (bool, string) ValidarPA(string inscricao)
    {
        if (inscricao.Length != 9)
            return (false, "IE do Pará deve ter 9 dígitos.");

        if (!inscricao.StartsWith("15"))
            return (false, "IE do Pará deve começar com '15'.");

        var baseIe = inscricao[..8];
        int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Pará.");
    }

    private static (bool, string) ValidarPR(string inscricao)
    {
        if (inscricao.Length != 10)
            return (false, "IE do Paraná deve ter 10 dígitos.");

        var baseIe = inscricao[..8];
        int[] pesos1 = { 3, 2, 7, 6, 5, 4, 3, 2 };
        var dv1 = CalcularDV(baseIe, pesos1);
        
        baseIe += dv1;
        int[] pesos2 = { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        var dv2 = CalcularDV(baseIe, pesos2);

        return (baseIe + dv2) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Paraná.");
    }

    private static (bool, string) ValidarPE(string inscricao)
    {
        if (inscricao.Length != 9 && inscricao.Length != 14)
            return (false, "IE de Pernambuco deve ter 9 ou 14 dígitos.");

        // Validação simplificada
        return (true, "");
    }

    private static (bool, string) ValidarRJ(string inscricao)
    {
        if (inscricao.Length != 8)
            return (false, "IE do Rio de Janeiro deve ter 8 dígitos.");

        var baseIe = inscricao[..7];
        int[] pesos = { 2, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Rio de Janeiro.");
    }

    private static (bool, string) ValidarRN(string inscricao)
    {
        if (inscricao.Length != 9 && inscricao.Length != 10)
            return (false, "IE do Rio Grande do Norte deve ter 9 ou 10 dígitos.");

        if (!inscricao.StartsWith("20"))
            return (false, "IE do Rio Grande do Norte deve começar com '20'.");

        return (true, "");
    }

    private static (bool, string) ValidarRS(string inscricao)
    {
        if (inscricao.Length != 10)
            return (false, "IE do Rio Grande do Sul deve ter 10 dígitos.");

        var baseIe = inscricao[..9];
        int[] pesos = { 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE do Rio Grande do Sul.");
    }

    private static (bool, string) ValidarRO(string inscricao)
    {
        if (inscricao.Length != 14)
            return (false, "IE de Rondônia deve ter 14 dígitos.");

        var baseIe = inscricao[..13];
        int[] pesos = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var dv = CalcularDV(baseIe, pesos);

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE de Rondônia.");
    }

    private static (bool, string) ValidarRR(string inscricao)
    {
        if (inscricao.Length != 9)
            return (false, "IE de Roraima deve ter 9 dígitos.");

        if (!inscricao.StartsWith("24"))
            return (false, "IE de Roraima deve começar com '24'.");

        var baseIe = inscricao[..8];
        int[] pesos = { 1, 2, 3, 4, 5, 6, 7, 8 };
        var soma = 0;
        for (int i = 0; i < 8; i++)
            soma += int.Parse(baseIe[i].ToString()) * pesos[i];

        var dv = soma % 9;

        return (baseIe + dv) == inscricao 
            ? (true, "") 
            : (false, "Dígito verificador incorreto para IE de Roraima.");
    }

    private static (bool, string) ValidarSP(string inscricao)
    {
        if (inscricao.Length != 12 && inscricao.Length != 13)
            return (false, "IE de São Paulo deve ter 12 ou 13 dígitos.");

        if (inscricao.Length == 12)
        {
            var baseIe = inscricao[..8];
            int[] pesos1 = { 1, 3, 4, 5, 6, 7, 8, 10 };
            var soma = 0;
            for (int i = 0; i < 8; i++)
                soma += int.Parse(baseIe[i].ToString()) * pesos1[i];
            var dv1 = soma % 11;
            dv1 = dv1 % 10; // Pega apenas o último dígito

            baseIe += dv1 + inscricao.Substring(9, 2);

            int[] pesos2 = { 3, 2, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 11; i++)
                soma += int.Parse(baseIe[i].ToString()) * pesos2[i];
            var dv2 = soma % 11;
            dv2 = dv2 % 10;

            var ieCalculada = baseIe + dv2;
            return ieCalculada == inscricao 
                ? (true, "") 
                : (false, "Dígito verificador incorreto para IE de São Paulo.");
        }

        // Para 13 dígitos (produtor rural antigo)
        return (false, "IE de São Paulo com 13 dígitos (formato antigo) - remova a letra 'P' se presente.");
    }

    private static (bool, string) ValidarTO(string inscricao)
    {
        if (inscricao.Length != 9 && inscricao.Length != 11)
            return (false, "IE de Tocantins deve ter 9 ou 11 dígitos.");

        if (!inscricao.StartsWith("29"))
            return (false, "IE de Tocantins deve começar com '29'.");

        if (inscricao.Length == 9)
        {
            var baseIe = inscricao[..8];
            int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
            var dv = CalcularDV(baseIe, pesos);

            return (baseIe + dv) == inscricao 
                ? (true, "") 
                : (false, "Dígito verificador incorreto para IE de Tocantins.");
        }

        return (true, "");
    }

    #endregion

    #region Helpers

    private static int CalcularDV(string baseIe, int[] pesos)
    {
        var soma = 0;
        for (int i = 0; i < baseIe.Length && i < pesos.Length; i++)
            soma += int.Parse(baseIe[i].ToString()) * pesos[i];

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }

    #endregion
}
