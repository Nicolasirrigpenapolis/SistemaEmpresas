using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Representa o emitente da Nota Fiscal - dados da empresa que emite as notas.
/// Cada tenant (empresa) terá seu próprio emitente cadastrado.
/// </summary>
[Table("Emitentes")]
[Index(nameof(Cnpj), Name = "IX_Emitentes_CNPJ", IsUnique = true)]
public class Emitente
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    /// <summary>
    /// CNPJ do emitente (14 dígitos, sem formatação)
    /// </summary>
    [Required]
    [Column("CNPJ")]
    [StringLength(14)]
    public string Cnpj { get; set; } = null!;

    /// <summary>
    /// Razão Social do emitente
    /// </summary>
    [Required]
    [Column("Razao_Social")]
    [StringLength(60)]
    public string RazaoSocial { get; set; } = null!;

    /// <summary>
    /// Nome Fantasia do emitente
    /// </summary>
    [Column("Nome_Fantasia")]
    [StringLength(60)]
    public string? NomeFantasia { get; set; }

    /// <summary>
    /// Inscrição Estadual do emitente
    /// </summary>
    [Required]
    [Column("Inscricao_Estadual")]
    [StringLength(20)]
    public string InscricaoEstadual { get; set; } = null!;

    /// <summary>
    /// Inscrição Municipal do emitente (para NFSe)
    /// </summary>
    [Column("Inscricao_Municipal")]
    [StringLength(20)]
    public string? InscricaoMunicipal { get; set; }

    /// <summary>
    /// CNAE - Classificação Nacional de Atividades Econômicas
    /// </summary>
    [Column("CNAE")]
    [StringLength(10)]
    public string? Cnae { get; set; }

    /// <summary>
    /// Código do Regime Tributário:
    /// 1 = Simples Nacional
    /// 2 = Simples Nacional – excesso de sublimite de receita bruta
    /// 3 = Regime Normal
    /// </summary>
    [Column("Codigo_Regime_Tributario")]
    public int? CodigoRegimeTributario { get; set; }

    // ===========================================
    // ENDEREÇO DO EMITENTE
    // ===========================================

    /// <summary>
    /// Logradouro (rua, avenida, etc.)
    /// </summary>
    [Required]
    [Column("Endereco")]
    [StringLength(100)]
    public string Endereco { get; set; } = null!;

    /// <summary>
    /// Número do endereço
    /// </summary>
    [Required]
    [Column("Numero")]
    [StringLength(10)]
    public string Numero { get; set; } = null!;

    /// <summary>
    /// Complemento do endereço
    /// </summary>
    [Column("Complemento")]
    [StringLength(60)]
    public string? Complemento { get; set; }

    /// <summary>
    /// Bairro
    /// </summary>
    [Required]
    [Column("Bairro")]
    [StringLength(60)]
    public string Bairro { get; set; } = null!;

    /// <summary>
    /// Código do Município (IBGE - 7 dígitos)
    /// </summary>
    [Required]
    [Column("Codigo_Municipio")]
    [StringLength(7)]
    public string CodigoMunicipio { get; set; } = null!;

    /// <summary>
    /// Nome do Município
    /// </summary>
    [Required]
    [Column("Municipio")]
    [StringLength(60)]
    public string Municipio { get; set; } = null!;

    /// <summary>
    /// UF - Sigla do Estado (2 caracteres)
    /// </summary>
    [Required]
    [Column("UF")]
    [StringLength(2)]
    public string Uf { get; set; } = null!;

    /// <summary>
    /// CEP (8 dígitos, sem formatação)
    /// </summary>
    [Required]
    [Column("CEP")]
    [StringLength(8)]
    public string Cep { get; set; } = null!;

    /// <summary>
    /// Código do País (padrão: 1058 = Brasil)
    /// </summary>
    [Column("Codigo_Pais")]
    [StringLength(4)]
    public string CodigoPais { get; set; } = "1058";

    /// <summary>
    /// Nome do País
    /// </summary>
    [Column("Pais")]
    [StringLength(60)]
    public string Pais { get; set; } = "Brasil";

    // ===========================================
    // CONTATO
    // ===========================================

    /// <summary>
    /// Telefone principal
    /// </summary>
    [Column("Telefone")]
    [StringLength(14)]
    public string? Telefone { get; set; }

    /// <summary>
    /// Email do emitente
    /// </summary>
    [Column("Email")]
    [StringLength(255)]
    public string? Email { get; set; }

    // ===========================================
    // DADOS PARA NFe
    // ===========================================

    /// <summary>
    /// Ambiente da NFe:
    /// 1 = Produção
    /// 2 = Homologação
    /// </summary>
    [Column("Ambiente_NFe")]
    public int AmbienteNfe { get; set; } = 2; // Padrão: Homologação

    /// <summary>
    /// Série da NFe
    /// </summary>
    [Column("Serie_NFe")]
    public int SerieNfe { get; set; } = 1;

    /// <summary>
    /// Próximo número de NFe a ser emitido
    /// </summary>
    [Column("Proximo_Numero_NFe")]
    public int ProximoNumeroNfe { get; set; } = 1;

    /// <summary>
    /// Caminho do certificado digital
    /// </summary>
    [Column("Caminho_Certificado")]
    [StringLength(500)]
    public string? CaminhoCertificado { get; set; }

    /// <summary>
    /// Senha do certificado digital (criptografada)
    /// </summary>
    [Column("Senha_Certificado")]
    [StringLength(500)]
    public string? SenhaCertificado { get; set; }

    /// <summary>
    /// Data de validade do certificado digital
    /// </summary>
    [Column("Validade_Certificado")]
    public DateTime? ValidadeCertificado { get; set; }

    // ===========================================
    // CONTROLE
    // ===========================================

    /// <summary>
    /// Indica se o emitente está ativo
    /// </summary>
    [Column("Ativo")]
    public bool Ativo { get; set; } = true;

    /// <summary>
    /// Data de criação do registro
    /// </summary>
    [Column("Data_Criacao")]
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    [Column("Data_Atualizacao")]
    public DateTime? DataAtualizacao { get; set; }

    /// <summary>
    /// Data da última consulta CNPJ na Receita
    /// </summary>
    [Column("Data_Consulta_CNPJ")]
    public DateTime? DataConsultaCnpj { get; set; }
}
