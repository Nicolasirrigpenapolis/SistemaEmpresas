namespace SistemaEmpresas.Features.Transporte.Dtos;

// DTO para listagem
public class MotoristaListDto
{
    public short CodigoDoMotorista { get; set; }
    public string NomeDoMotorista { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Cel { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
}

// DTO para detalhes
public class MotoristaDto
{
    public short CodigoDoMotorista { get; set; }
    public string NomeDoMotorista { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public int Municipio { get; set; }
    public string MunicipioNome { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Fone { get; set; } = string.Empty;
    public string Cel { get; set; } = string.Empty;
}

// DTO para criação
public class MotoristaCreateDto
{
    public string NomeDoMotorista { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public int Municipio { get; set; }
    public string Uf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Fone { get; set; } = string.Empty;
    public string Cel { get; set; } = string.Empty;
}

// DTO para atualização
public class MotoristaUpdateDto
{
    public string NomeDoMotorista { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public int Municipio { get; set; }
    public string Uf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Fone { get; set; } = string.Empty;
    public string Cel { get; set; } = string.Empty;
}

// DTO para filtros
public class MotoristaFiltrosDto
{
    public string? Busca { get; set; }
    public string? Uf { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
}
