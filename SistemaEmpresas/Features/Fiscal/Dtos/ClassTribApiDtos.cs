using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SistemaEmpresas.Features.Fiscal.Dtos;

/// <summary>
/// DTO de Resposta da API SVRS ClassTrib
/// Representa um CST (Código de Situação Tributária) e suas classificações
/// Endpoint: https://cff.svrs.rs.gov.br/api/v1/consultas/classTrib
/// </summary>
public class ClassTribApiResponse
{
    /// <summary>
    /// CST - Código de Situação Tributária (Ex: "000", "200", "410")
    /// </summary>
    [JsonPropertyName("CST")]
    public string CodigoSituacaoTributaria { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do CST (Ex: "Tributação integral", "Alíquota reduzida")
    /// </summary>
    [JsonPropertyName("DescricaoCST")]
    public string DescricaoSituacaoTributaria { get; set; } = string.Empty;

    /// <summary>
    /// Indicador de IBS/CBS
    /// </summary>
    [JsonPropertyName("IndIBSCBS")]
    public bool IndicadorIbsCbs { get; set; }

    /// <summary>
    /// Indicador de Redução de Base de Cálculo
    /// </summary>
    [JsonPropertyName("IndRedBC")]
    public bool IndicadorReducaoBaseCalculo { get; set; }

    /// <summary>
    /// Indicador de Redução de Alíquota
    /// </summary>
    [JsonPropertyName("IndRedAliq")]
    public bool IndicadorReducaoAliquota { get; set; }

    /// <summary>
    /// Indicador de Diferimento
    /// </summary>
    [JsonPropertyName("IndDif")]
    public bool IndicadorDiferimento { get; set; }

    /// <summary>
    /// Lista de Classificações Tributárias vinculadas a este CST
    /// </summary>
    [JsonPropertyName("classificacoesTributarias")]
    public List<ClassificacaoTributariaApiDto> ClassificacoesTributarias { get; set; } = new();
}

/// <summary>
/// DTO de Classificação Tributária individual da API SVRS
/// Representa um cClassTrib específico com todas as suas regras
/// </summary>
public class ClassificacaoTributariaApiDto
{
    /// <summary>
    /// cClassTrib - Código único da Classificação (Ex: "000001", "200047")
    /// </summary>
    [JsonPropertyName("cClassTrib")]
    public string CodigoClassificacaoTributaria { get; set; } = string.Empty;

    /// <summary>
    /// Descrição completa e detalhada da classificação tributária
    /// </summary>
    [JsonPropertyName("DescricaoClassTrib")]
    public string DescricaoCompleta { get; set; } = string.Empty;

    /// <summary>
    /// Percentual de Redução IBS (0.00000 a 100.00000)
    /// Ex: 100.00000 = alíquota zero
    /// </summary>
    [JsonPropertyName("pRedIBS")]
    public decimal PercentualReducaoIBS { get; set; }

    /// <summary>
    /// Percentual de Redução CBS (0.00000 a 100.00000)
    /// </summary>
    [JsonPropertyName("pRedCBS")]
    public decimal PercentualReducaoCBS { get; set; }

    /// <summary>
    /// Indica se utiliza tributação regular
    /// </summary>
    [JsonPropertyName("IndTribRegular")]
    public bool TributacaoRegular { get; set; }

    /// <summary>
    /// Indica se há crédito presumido de operações
    /// </summary>
    [JsonPropertyName("IndCredPresOper")]
    public bool CreditoPresumidoOperacoes { get; set; }

    /// <summary>
    /// Indica se há estorno de crédito
    /// </summary>
    [JsonPropertyName("IndEstornoCred")]
    public bool EstornoCredito { get; set; }

    /// <summary>
    /// Tipo de Alíquota (Ex: "Padrão", "Fixa", "Uniforme Nacional", "Sem Alíquota")
    /// </summary>
    [JsonPropertyName("TipoAliquota")]
    public string TipoAliquota { get; set; } = string.Empty;

    /// <summary>
    /// Válido para NFe (Nota Fiscal Eletrônica)
    /// CAMPO PRINCIPAL - Sistema trabalha apenas com NFe
    /// </summary>
    [JsonPropertyName("IndNFe")]
    public bool ValidoParaNFe { get; set; }

    /// <summary>
    /// Número do Anexo da legislação (quando aplicável)
    /// Ex: 1, 4, 12 - Referente aos anexos da LC 214/2025
    /// </summary>
    [JsonPropertyName("Anexo")]
    public int? NumeroAnexoLegislacao { get; set; }

    /// <summary>
    /// Link para a legislação completa (Lei Complementar, artigos, etc)
    /// </summary>
    [JsonPropertyName("Link")]
    public string? LinkLegislacao { get; set; }
}
