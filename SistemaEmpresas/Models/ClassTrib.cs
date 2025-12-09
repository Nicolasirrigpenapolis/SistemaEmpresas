using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Tabela de Classificações Tributárias (ClassTrib) da API SVRS
/// Armazena os dados sincronizados da Reforma Tributária (IBS/CBS)
/// Esta tabela é populada via sincronização com a API SVRS
/// </summary>
[Table("ClassTrib")]
[Index(nameof(CodigoClassTrib), IsUnique = true)]
[Index(nameof(CodigoSituacaoTributaria))]
public class ClassTrib
{
    /// <summary>
    /// ID interno (autoincremento)
    /// </summary>
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    /// <summary>
    /// cClassTrib - Código único da Classificação Tributária SVRS
    /// Ex: "000001", "200047", "410001"
    /// </summary>
    [Required]
    [Column("CodigoClassTrib")]
    [StringLength(6)]
    [Unicode(false)]
    public string CodigoClassTrib { get; set; } = string.Empty;

    /// <summary>
    /// CST - Código de Situação Tributária
    /// Ex: "000" (Tributação integral), "200" (Alíquota reduzida), "410" (Isenção)
    /// </summary>
    [Required]
    [Column("CodigoSituacaoTributaria")]
    [StringLength(3)]
    [Unicode(false)]
    public string CodigoSituacaoTributaria { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do CST
    /// Ex: "Tributação integral", "Alíquota reduzida", "Isenção"
    /// </summary>
    [Column("DescricaoSituacaoTributaria")]
    [StringLength(200)]
    public string? DescricaoSituacaoTributaria { get; set; }

    /// <summary>
    /// Descrição completa da Classificação Tributária conforme API SVRS
    /// </summary>
    [Required]
    [Column("DescricaoClassTrib")]
    public string DescricaoClassTrib { get; set; } = string.Empty;

    /// <summary>
    /// Percentual de Redução do IBS (Imposto sobre Bens e Serviços)
    /// Ex: 0.00000 = sem redução, 60.00000 = 60% redução, 100.00000 = isenção
    /// </summary>
    [Column("PercentualReducaoIBS", TypeName = "decimal(8, 5)")]
    public decimal PercentualReducaoIBS { get; set; }

    /// <summary>
    /// Percentual de Redução do CBS (Contribuição sobre Bens e Serviços)
    /// Ex: 0.00000 = sem redução, 60.00000 = 60% redução, 100.00000 = isenção
    /// </summary>
    [Column("PercentualReducaoCBS", TypeName = "decimal(8, 5)")]
    public decimal PercentualReducaoCBS { get; set; }

    /// <summary>
    /// Tipo de Alíquota aplicável
    /// Valores: "Padrão", "Fixa", "Uniforme Nacional", "Uniforme Setorial", "Sem Alíquota"
    /// </summary>
    [Column("TipoAliquota")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TipoAliquota { get; set; }

    /// <summary>
    /// Indica se a classificação é válida para NFe (Nota Fiscal Eletrônica)
    /// CAMPO PRINCIPAL - Sistema trabalha com NFe
    /// </summary>
    [Column("ValidoParaNFe")]
    public bool ValidoParaNFe { get; set; }

    /// <summary>
    /// Indica se utiliza tributação regular
    /// </summary>
    [Column("TributacaoRegular")]
    public bool TributacaoRegular { get; set; }

    /// <summary>
    /// Indica se há crédito presumido de operações
    /// </summary>
    [Column("CreditoPresumidoOperacoes")]
    public bool CreditoPresumidoOperacoes { get; set; }

    /// <summary>
    /// Indica se há estorno de crédito
    /// </summary>
    [Column("EstornoCredito")]
    public bool EstornoCredito { get; set; }

    /// <summary>
    /// Número do Anexo da legislação (LC 214/2025)
    /// Ex: 1, 4, 12
    /// </summary>
    [Column("AnexoLegislacao")]
    public int? AnexoLegislacao { get; set; }

    /// <summary>
    /// Link para a legislação completa
    /// </summary>
    [Column("LinkLegislacao")]
    [StringLength(500)]
    [Unicode(false)]
    public string? LinkLegislacao { get; set; }

    /// <summary>
    /// Data da última sincronização com API SVRS
    /// </summary>
    [Column("DataSincronizacao")]
    public DateTime DataSincronizacao { get; set; }

    /// <summary>
    /// Indica se o registro está ativo
    /// </summary>
    [Column("Ativo")]
    public bool Ativo { get; set; } = true;

    // ============================================================================
    // NAVIGATION PROPERTIES
    // ============================================================================

    /// <summary>
    /// Classificações Fiscais que utilizam este ClassTrib
    /// </summary>
    [InverseProperty("ClassTribNavigation")]
    [JsonIgnore]
    public virtual ICollection<ClassificacaoFiscal> ClassificacoesFiscais { get; set; } = new List<ClassificacaoFiscal>();
}
