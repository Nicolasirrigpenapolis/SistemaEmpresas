using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SistemaEmpresas.Features.Emitentes.Dtos;

/// <summary>
/// DTO para listagem de emitentes
/// </summary>
public class EmitenteListDto
{
    public int Id { get; set; }
    public string Cnpj { get; set; } = null!;
    public string RazaoSocial { get; set; } = null!;
    public string? NomeFantasia { get; set; }
    public string InscricaoEstadual { get; set; } = null!;
    public string Municipio { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public bool Ativo { get; set; }
    public int AmbienteNfe { get; set; }
    public string AmbienteNfeDescricao => AmbienteNfe == 1 ? "Produção" : "Homologação";
    public bool PossuiCertificado { get; set; }
    public bool CertificadoValido { get; set; }
    public DateTime? ValidadeCertificado { get; set; }
}

/// <summary>
/// DTO completo do emitente
/// </summary>
public class EmitenteDto
{
    public int Id { get; set; }
    public string Cnpj { get; set; } = null!;
    public string RazaoSocial { get; set; } = null!;
    public string? NomeFantasia { get; set; }
    public string InscricaoEstadual { get; set; } = null!;
    public string? InscricaoMunicipal { get; set; }
    public string? Cnae { get; set; }
    public int? CodigoRegimeTributario { get; set; }
    public string CodigoRegimeTributarioDescricao => CodigoRegimeTributario switch
    {
        1 => "Simples Nacional",
        2 => "Simples Nacional - Excesso de sublimite",
        3 => "Regime Normal",
        _ => "Não informado"
    };
    
    // Endereço
    public string Endereco { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = null!;
    public string CodigoMunicipio { get; set; } = null!;
    public string Municipio { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public string Cep { get; set; } = null!;
    public string CodigoPais { get; set; } = "1058";
    public string Pais { get; set; } = "Brasil";
    
    // Contato
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    
    // NFe
    public int AmbienteNfe { get; set; }
    public string AmbienteNfeDescricao => AmbienteNfe == 1 ? "Produção" : "Homologação";
    public int SerieNfe { get; set; }
    public int ProximoNumeroNfe { get; set; }
    public string? CaminhoCertificado { get; set; }
    public DateTime? ValidadeCertificado { get; set; }
    
    // Status do Certificado (calculados)
    public bool PossuiCertificado => !string.IsNullOrWhiteSpace(CaminhoCertificado);
    public bool CertificadoValido => ValidadeCertificado.HasValue && ValidadeCertificado.Value > DateTime.Now;
    public bool CertificadoProximoVencimento => ValidadeCertificado.HasValue && 
        (ValidadeCertificado.Value - DateTime.Now).TotalDays <= 30 &&
        (ValidadeCertificado.Value - DateTime.Now).TotalDays > 0;
    public int? DiasParaVencimento => ValidadeCertificado.HasValue ? 
        (int)(ValidadeCertificado.Value - DateTime.Now).TotalDays : null;
    
    // Controle
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public DateTime? DataConsultaCnpj { get; set; }
}

/// <summary>
/// DTO para criar/atualizar emitente
/// </summary>
public class EmitenteCreateUpdateDto
{
    [Required(ErrorMessage = "CNPJ é obrigatório")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "CNPJ deve ter 14 dígitos")]
    [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ deve conter apenas números")]
    public string Cnpj { get; set; } = null!;

    [Required(ErrorMessage = "Razão Social é obrigatória")]
    [StringLength(60, ErrorMessage = "Razão Social deve ter no máximo 60 caracteres")]
    public string RazaoSocial { get; set; } = null!;

    [StringLength(60, ErrorMessage = "Nome Fantasia deve ter no máximo 60 caracteres")]
    public string? NomeFantasia { get; set; }

    [Required(ErrorMessage = "Inscrição Estadual é obrigatória")]
    [StringLength(20, ErrorMessage = "Inscrição Estadual deve ter no máximo 20 caracteres")]
    public string InscricaoEstadual { get; set; } = null!;

    [StringLength(20, ErrorMessage = "Inscrição Municipal deve ter no máximo 20 caracteres")]
    public string? InscricaoMunicipal { get; set; }

    [StringLength(10, ErrorMessage = "CNAE deve ter no máximo 10 caracteres")]
    public string? Cnae { get; set; }

    [Range(1, 3, ErrorMessage = "Código do Regime Tributário deve ser 1, 2 ou 3")]
    public int? CodigoRegimeTributario { get; set; }

    // Endereço
    [Required(ErrorMessage = "Endereço é obrigatório")]
    [StringLength(100, ErrorMessage = "Endereço deve ter no máximo 100 caracteres")]
    public string Endereco { get; set; } = null!;

    [Required(ErrorMessage = "Número é obrigatório")]
    [StringLength(10, ErrorMessage = "Número deve ter no máximo 10 caracteres")]
    public string Numero { get; set; } = null!;

    [StringLength(60, ErrorMessage = "Complemento deve ter no máximo 60 caracteres")]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "Bairro é obrigatório")]
    [StringLength(60, ErrorMessage = "Bairro deve ter no máximo 60 caracteres")]
    public string Bairro { get; set; } = null!;

    [Required(ErrorMessage = "Código do Município é obrigatório")]
    [StringLength(7, MinimumLength = 7, ErrorMessage = "Código do Município deve ter 7 dígitos")]
    public string CodigoMunicipio { get; set; } = null!;

    [Required(ErrorMessage = "Município é obrigatório")]
    [StringLength(60, ErrorMessage = "Município deve ter no máximo 60 caracteres")]
    public string Municipio { get; set; } = null!;

    [Required(ErrorMessage = "UF é obrigatória")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve ter 2 caracteres")]
    public string Uf { get; set; } = null!;

    [Required(ErrorMessage = "CEP é obrigatório")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve ter 8 dígitos")]
    public string Cep { get; set; } = null!;

    [StringLength(4)]
    public string CodigoPais { get; set; } = "1058";

    [StringLength(60)]
    public string Pais { get; set; } = "Brasil";

    // Contato
    [StringLength(14, ErrorMessage = "Telefone deve ter no máximo 14 caracteres")]
    public string? Telefone { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
    public string? Email { get; set; }

    // NFe
    [Range(1, 2, ErrorMessage = "Ambiente NFe deve ser 1 (Produção) ou 2 (Homologação)")]
    public int AmbienteNfe { get; set; } = 2;

    [Range(1, 999, ErrorMessage = "Série NFe deve estar entre 1 e 999")]
    public int SerieNfe { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "Próximo Número NFe deve ser maior que 0")]
    public int ProximoNumeroNfe { get; set; } = 1;

    [StringLength(500)]
    public string? CaminhoCertificado { get; set; }

    public string? SenhaCertificado { get; set; }

    public DateTime? ValidadeCertificado { get; set; }

    // Controle
    public bool Ativo { get; set; } = true;
}

/// <summary>
/// DTO para resposta da consulta de CNPJ
/// </summary>
public class ConsultaCnpjDto
{
    public string Cnpj { get; set; } = null!;
    public string RazaoSocial { get; set; } = null!;
    public string? NomeFantasia { get; set; }
    public string Endereco { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = null!;
    public string CodigoMunicipio { get; set; } = null!;
    public string Municipio { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public string Cep { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? AtividadePrincipal { get; set; }
    public string? Cnae { get; set; }
    public string? Situacao { get; set; }
    public DateTime DataConsulta { get; set; }
}

/// <summary>
/// DTO para upload de certificado digital
/// </summary>
public class CertificadoUploadDto
{
    [Required(ErrorMessage = "Arquivo do certificado é obrigatório")]
    public IFormFile Arquivo { get; set; } = null!;

    [Required(ErrorMessage = "Senha do certificado é obrigatória")]
    public string Senha { get; set; } = null!;
}
