namespace SistemaEmpresas.Features.Geral.Dtos;

/// <summary>
/// DTO para listagem resumida de Geral (grid)
/// </summary>
public class GeralListDto
{
    public int SequenciaDoGeral { get; set; }
    public string RazaoSocial { get; set; } = "";
    public string NomeFantasia { get; set; } = "";
    public string CpfECnpj { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Uf { get; set; } = "";
    public string Fone1 { get; set; } = "";
    public string Email { get; set; } = "";
    public bool Cliente { get; set; }
    public bool Fornecedor { get; set; }
    public bool Transportadora { get; set; }
    public bool Vendedor { get; set; }
    public bool Inativo { get; set; }
    public short Tipo { get; set; } // 0=PF, 1=PJ
}

/// <summary>
/// DTO para criação de novo registro
/// </summary>
public class GeralCreateDto
{
    // Tipos (flags) - Pelo menos um deve ser true
    public bool Cliente { get; set; }
    public bool Fornecedor { get; set; }
    public bool Despesa { get; set; }
    public bool Imposto { get; set; }
    public bool Transportadora { get; set; }
    public bool Vendedor { get; set; }

    // Dados Principais
    public string RazaoSocial { get; set; } = "";
    public string NomeFantasia { get; set; } = "";
    public short Tipo { get; set; } // 0=PF, 1=PJ
    public string CpfECnpj { get; set; } = "";
    public string RgEIe { get; set; } = "";
    public string CodigoDoSuframa { get; set; } = "";
    public string CodigoDaAntt { get; set; } = "";

    // Endereço Principal
    public string Endereco { get; set; } = "";
    public string NumeroDoEndereco { get; set; } = "";
    public string Complemento { get; set; } = "";
    public string Bairro { get; set; } = "";
    public string CaixaPostal { get; set; } = "";
    public int SequenciaDoMunicipio { get; set; }
    public string Cep { get; set; } = "";
    public int SequenciaDoPais { get; set; } = 1; // Brasil

    // Contato
    public string Fone1 { get; set; } = "";
    public string Fone2 { get; set; } = "";
    public string Fax { get; set; } = "";
    public string Celular { get; set; } = "";
    public string Contato { get; set; } = "";
    public string Email { get; set; } = "";
    public string HomePage { get; set; } = "";

    // Observações
    public string Observacao { get; set; } = "";

    // Flags fiscais
    public bool Revenda { get; set; }
    public bool Isento { get; set; }
    public bool OrgonPublico { get; set; }
    public bool EmpresaProdutor { get; set; }
    public bool Cumulativo { get; set; }
    public bool Inativo { get; set; }

    // Vendedor associado
    public int SequenciaDoVendedor { get; set; }
    public string IntermediarioDoVendedor { get; set; } = "";

    // Dados de Cobrança (aba 2)
    public string EnderecoDeCobranca { get; set; } = "";
    public string NumeroDoEnderecoDeCobranca { get; set; } = "";
    public string ComplementoDaCobranca { get; set; } = "";
    public string BairroDeCobranca { get; set; } = "";
    public string CaixaPostalDaCobranca { get; set; } = "";
    public int SequenciaMunicipioCobranca { get; set; }
    public string CepDeCobranca { get; set; } = "";

    // Dados Bancários (aba 2)
    public string NomeDoBanco1 { get; set; } = "";
    public string AgenciaDoBanco1 { get; set; } = "";
    public string ContaCorrenteDoBanco1 { get; set; } = "";
    public string NomeDoCorrentistaDoBanco1 { get; set; } = "";
    public string NomeDoBanco2 { get; set; } = "";
    public string AgenciaDoBanco2 { get; set; } = "";
    public string ContaCorrenteDoBanco2 { get; set; } = "";
    public string NomeDoCorrentistaDoBanco2 { get; set; } = "";

    // Dados adicionais
    public DateTime? DataDeNascimento { get; set; }
    public int CodigoContabil { get; set; }
    public int CodigoAdiantamento { get; set; }
    public decimal SalBruto { get; set; }
    
    // Flag WhatsApp (legado)
    public bool WhatsAppSincronizado { get; set; }
}

/// <summary>
/// DTO para atualização de registro existente
/// </summary>
public class GeralUpdateDto : GeralCreateDto
{
    public int SequenciaDoGeral { get; set; }
}

/// <summary>
/// DTO detalhado para visualização completa
/// </summary>
public class GeralDetailDto : GeralUpdateDto
{
    public DateTime? DataDoCadastro { get; set; }
    public string UsuDaAlteracao { get; set; } = "";
    
    // Dados navegacionais (nomes das FKs)
    public string MunicipioNome { get; set; } = "";
    public string MunicipioUf { get; set; } = "";
    public string MunicipioCobrancaNome { get; set; } = "";
    public string MunicipioCobrancaUf { get; set; } = "";
    public string VendedorNome { get; set; } = "";
    public string PaisNome { get; set; } = "";
}

/// <summary>
/// Filtros para listagem paginada
/// </summary>
public class GeralFiltroDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? Busca { get; set; }
    public bool? Cliente { get; set; }
    public bool? Fornecedor { get; set; }
    public bool? Transportadora { get; set; }
    public bool? Vendedor { get; set; }
    public bool IncluirInativos { get; set; } = false;
}

/// <summary>
/// DTO para resposta de consulta CNPJ (API externa)
/// </summary>
public class CnpjResponseDto
{
    public string RazaoSocial { get; set; } = "";
    public string NomeFantasia { get; set; } = "";
    public string Logradouro { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Complemento { get; set; } = "";
    public string Bairro { get; set; } = "";
    public string Municipio { get; set; } = "";
    public string Uf { get; set; } = "";
    public string Cep { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string Email { get; set; } = "";
    /// <summary>
    /// ID do município no banco. Null se não encontrado.
    /// </summary>
    public int? SequenciaDoMunicipio { get; set; }
}

/// <summary>
/// DTO para resposta de consulta CEP (API externa)
/// </summary>
public class CepResponseDto
{
    public string Logradouro { get; set; } = "";
    public string Complemento { get; set; } = "";
    public string Bairro { get; set; } = "";
    public string Municipio { get; set; } = "";
    public string Uf { get; set; } = "";
    public string Cep { get; set; } = "";
    /// <summary>
    /// ID do município no banco. Null se não encontrado.
    /// </summary>
    public int? SequenciaDoMunicipio { get; set; }
}
