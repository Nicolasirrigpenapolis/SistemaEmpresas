using System.Text.RegularExpressions;
using System.Net.Http.Json;
using SistemaEmpresas.Core.Dtos;
using SistemaEmpresas.Features.Geral.Dtos;
using SistemaEmpresas.Features.Geral.Repositories;
using SistemaEmpresas.Utils;
using GeralModel = SistemaEmpresas.Models.Geral;

namespace SistemaEmpresas.Features.Geral.Services;

/// <summary>
/// Interface do serviço de Geral (Clientes, Fornecedores, Transportadoras, Vendedores)
/// </summary>
public interface IGeralService
{
    Task<PagedResult<GeralListDto>> ListarAsync(GeralFiltroDto filtro);
    Task<GeralDetailDto?> ObterPorIdAsync(int id);
    Task<List<GeralListDto>> BuscarAsync(string termo, string? tipo = null, int limit = 20);
    Task<List<GeralListDto>> ListarVendedoresAsync();
    Task<ServiceResult<GeralDetailDto>> CriarAsync(GeralCreateDto dto, string usuario);
    Task<ServiceResult<GeralDetailDto>> AtualizarAsync(int id, GeralUpdateDto dto, string usuario);
    Task<ServiceResult<bool>> InativarAsync(int id, string usuario);
    Task<CnpjResponseDto?> ConsultarCnpjAsync(string cnpj);
    Task<CepResponseDto?> ConsultarCepAsync(string cep);
    
    // Validações - Usa DocumentoValidator internamente
    bool ValidarCpf(string cpf);
    bool ValidarCnpj(string cnpj);
    (bool valido, string? mensagem) ValidarDocumento(string? documento, int tipoPessoa);
}

/// <summary>
/// Resultado de operação do serviço
/// </summary>
public class ServiceResult<T>
{
    public bool Sucesso { get; set; }
    public T? Data { get; set; }
    public string? Mensagem { get; set; }
    public List<string> Erros { get; set; } = new();

    public static ServiceResult<T> Ok(T data, string? mensagem = null)
        => new() { Sucesso = true, Data = data, Mensagem = mensagem };

    public static ServiceResult<T> Erro(string mensagem)
        => new() { Sucesso = false, Mensagem = mensagem };

    public static ServiceResult<T> Erro(List<string> erros)
        => new() { Sucesso = false, Erros = erros };
}

/// <summary>
/// Serviço de Geral - Regras de negócio
/// </summary>
public class GeralService : IGeralService
{
    private readonly IGeralRepository _repository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GeralService> _logger;

    public GeralService(
        IGeralRepository repository,
        IHttpClientFactory httpClientFactory,
        ILogger<GeralService> logger)
    {
        _repository = repository;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public Task<PagedResult<GeralListDto>> ListarAsync(GeralFiltroDto filtro)
        => _repository.ListarAsync(filtro);

    public Task<GeralDetailDto?> ObterPorIdAsync(int id)
        => _repository.ObterPorIdAsync(id);

    public Task<List<GeralListDto>> BuscarAsync(string termo, string? tipo = null, int limit = 20)
        => _repository.BuscarAsync(termo, tipo, limit);

    public Task<List<GeralListDto>> ListarVendedoresAsync()
        => _repository.ListarVendedoresAsync();

    public async Task<ServiceResult<GeralDetailDto>> CriarAsync(GeralCreateDto dto, string usuario)
    {
        // Validações síncronas
        var erros = ValidarGeral(dto);
        
        // Validação de Inscrição Estadual (async)
        var errosIe = await ValidarInscricaoEstadualAsync(dto);
        erros.AddRange(errosIe);
        
        if (erros.Any())
            return ServiceResult<GeralDetailDto>.Erro(erros);

        // Limpar CPF/CNPJ
        var cpfCnpj = LimparCpfCnpj(dto.CpfECnpj);

        // Verificar duplicidade de CPF/CNPJ
        if (!string.IsNullOrWhiteSpace(cpfCnpj))
        {
            if (await _repository.ExisteCpfCnpjAsync(cpfCnpj))
                return ServiceResult<GeralDetailDto>.Erro($"Já existe um cadastro com o CPF/CNPJ: {dto.CpfECnpj}");
        }

        // Mapear para modelo
        var geral = MapToModel(dto);
        geral.CpfECnpj = cpfCnpj;
        geral.DataDoCadastro = DateTime.Now;
        geral.UsuDaAlteracao = usuario;

        await _repository.CriarAsync(geral);
        _logger.LogInformation("Geral criado: {Id} - {Nome}", geral.SequenciaDoGeral, geral.RazaoSocial);

        var result = await _repository.ObterPorIdAsync(geral.SequenciaDoGeral);
        return ServiceResult<GeralDetailDto>.Ok(result!, "Cadastro criado com sucesso");
    }

    public async Task<ServiceResult<GeralDetailDto>> AtualizarAsync(int id, GeralUpdateDto dto, string usuario)
    {
        var geralExistente = await _repository.ObterPorIdAsync(id);
        if (geralExistente == null)
            return ServiceResult<GeralDetailDto>.Erro("Cadastro não encontrado");

        // Validações síncronas
        var erros = ValidarGeral(dto);
        
        // Validação de Inscrição Estadual (async)
        var errosIe = await ValidarInscricaoEstadualAsync(dto);
        erros.AddRange(errosIe);
        
        if (erros.Any())
            return ServiceResult<GeralDetailDto>.Erro(erros);

        // Limpar CPF/CNPJ
        var cpfCnpj = LimparCpfCnpj(dto.CpfECnpj);

        // Verificar duplicidade de CPF/CNPJ (exceto o próprio)
        if (!string.IsNullOrWhiteSpace(cpfCnpj))
        {
            if (await _repository.ExisteCpfCnpjAsync(cpfCnpj, id))
                return ServiceResult<GeralDetailDto>.Erro($"Já existe outro cadastro com o CPF/CNPJ: {dto.CpfECnpj}");
        }

        // Mapear para modelo (mantendo o ID)
        var geral = MapToModel(dto);
        geral.SequenciaDoGeral = id;
        geral.CpfECnpj = cpfCnpj;
        geral.UsuDaAlteracao = usuario;

        await _repository.AtualizarAsync(geral);
        _logger.LogInformation("Geral atualizado: {Id} - {Nome}", id, geral.RazaoSocial);

        var result = await _repository.ObterPorIdAsync(id);
        return ServiceResult<GeralDetailDto>.Ok(result!, "Cadastro atualizado com sucesso");
    }

    public async Task<ServiceResult<bool>> InativarAsync(int id, string usuario)
    {
        var geral = await _repository.ObterPorIdAsync(id);
        if (geral == null)
            return ServiceResult<bool>.Erro("Cadastro não encontrado");

        await _repository.InativarAsync(id, usuario);
        _logger.LogInformation("Geral inativado: {Id}", id);

        return ServiceResult<bool>.Ok(true, "Cadastro inativado com sucesso");
    }

    public async Task<CnpjResponseDto?> ConsultarCnpjAsync(string cnpj)
    {
        try
        {
            var cnpjLimpo = LimparCpfCnpj(cnpj);
            if (cnpjLimpo.Length != 14)
            {
                _logger.LogWarning("CNPJ inválido para consulta: {Cnpj}", cnpj);
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://brasilapi.com.br/api/cnpj/v1/{cnpjLimpo}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Erro ao consultar CNPJ: {StatusCode}", response.StatusCode);
                return null;
            }

            var data = await response.Content.ReadFromJsonAsync<BrasilApiCnpjResponse>();
            if (data == null)
                return null;

            // Buscar município
            int? municipioId = null;
            if (!string.IsNullOrWhiteSpace(data.municipio) && !string.IsNullOrWhiteSpace(data.uf))
            {
                municipioId = await _repository.ObterMunicipioPorNomeUfAsync(data.municipio, data.uf);
            }

            return new CnpjResponseDto
            {
                RazaoSocial = data.razao_social ?? "",
                NomeFantasia = data.nome_fantasia ?? "",
                Logradouro = data.logradouro ?? "",
                Numero = data.numero ?? "",
                Complemento = data.complemento ?? "",
                Bairro = data.bairro ?? "",
                Cep = data.cep ?? "",
                Municipio = data.municipio ?? "",
                Uf = data.uf ?? "",
                Telefone = data.ddd_telefone_1 ?? "",
                Email = data.email ?? "",
                SequenciaDoMunicipio = municipioId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar CNPJ: {Cnpj}", cnpj);
            return null;
        }
    }

    public async Task<CepResponseDto?> ConsultarCepAsync(string cep)
    {
        try
        {
            var cepLimpo = Regex.Replace(cep, @"\D", "");
            if (cepLimpo.Length != 8)
            {
                _logger.LogWarning("CEP inválido para consulta: {Cep}", cep);
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://brasilapi.com.br/api/cep/v1/{cepLimpo}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Erro ao consultar CEP: {StatusCode}", response.StatusCode);
                return null;
            }

            var data = await response.Content.ReadFromJsonAsync<BrasilApiCepResponse>();
            if (data == null)
                return null;

            // Buscar município
            int? municipioId = null;
            if (!string.IsNullOrWhiteSpace(data.city) && !string.IsNullOrWhiteSpace(data.state))
            {
                municipioId = await _repository.ObterMunicipioPorNomeUfAsync(data.city, data.state);
            }

            return new CepResponseDto
            {
                Cep = data.cep ?? "",
                Logradouro = data.street ?? "",
                Bairro = data.neighborhood ?? "",
                Municipio = data.city ?? "",
                Uf = data.state ?? "",
                Complemento = "",
                SequenciaDoMunicipio = municipioId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar CEP: {Cep}", cep);
            return null;
        }
    }

    #region Validação de CPF/CNPJ

    public bool ValidarCpf(string cpf) => DocumentoValidator.IsCpfValido(cpf);

    public bool ValidarCnpj(string cnpj) => DocumentoValidator.IsCnpjValido(cnpj);

    public (bool valido, string? mensagem) ValidarDocumento(string? documento, int tipoPessoa) 
        => DocumentoValidator.Validar(documento, tipoPessoa);

    #endregion

    #region Métodos Auxiliares

    private List<string> ValidarGeral(GeralCreateDto dto)
    {
        var erros = new List<string>();

        // Razão Social obrigatória
        if (string.IsNullOrWhiteSpace(dto.RazaoSocial))
            erros.Add("Razão Social é obrigatória");

        // Nome Fantasia obrigatório (igual ao legado)
        if (string.IsNullOrWhiteSpace(dto.NomeFantasia))
            erros.Add("Nome Fantasia é obrigatório");

        // Pelo menos um tipo deve ser selecionado
        if (!dto.Cliente && !dto.Fornecedor && !dto.Transportadora && !dto.Vendedor && !dto.Despesa && !dto.Imposto)
            erros.Add("Pelo menos um tipo deve ser selecionado (Cliente, Fornecedor, Transportadora, Vendedor, Despesa ou Imposto)");

        // Validar CPF/CNPJ se informado - usando validador consolidado
        if (!string.IsNullOrWhiteSpace(dto.CpfECnpj))
        {
            var (valido, mensagem) = DocumentoValidator.Validar(dto.CpfECnpj, dto.Tipo);
            if (!valido && mensagem != null)
                erros.Add(mensagem);
        }

        // Vendedor não pode ser pessoa jurídica (regra do legado)
        if (dto.Vendedor && dto.Tipo == 1)
            erros.Add("Vendedor deve ser pessoa física");

        return erros;
    }

    /// <summary>
    /// Valida Inscrição Estadual baseado no UF do município
    /// </summary>
    private async Task<List<string>> ValidarInscricaoEstadualAsync(GeralCreateDto dto)
    {
        var erros = new List<string>();

        // Só valida IE para pessoa jurídica que não seja isenta
        if (dto.Tipo != 1 || dto.Isento)
            return erros;

        // Se não tem IE informada, não valida (pode ser isento não marcado)
        if (string.IsNullOrWhiteSpace(dto.RgEIe))
            return erros;

        // Buscar UF do município
        var uf = await _repository.ObterUfPorMunicipioIdAsync(dto.SequenciaDoMunicipio);
        
        // Se UF for exterior, não valida
        if (string.IsNullOrWhiteSpace(uf) || uf.ToUpper() == "EX")
            return erros;

        // Validar IE
        var (valido, mensagem) = InscricaoEstadualValidator.Validar(uf, dto.RgEIe);
        if (!valido)
            erros.Add(mensagem);

        return erros;
    }

    private static string LimparCpfCnpj(string? cpfCnpj) 
        => DocumentoValidator.LimparDocumento(cpfCnpj);

    private static GeralModel MapToModel(GeralCreateDto dto)
    {
        return new GeralModel
        {
            Cliente = dto.Cliente,
            Fornecedor = dto.Fornecedor,
            Despesa = dto.Despesa,
            Imposto = dto.Imposto,
            Transportadora = dto.Transportadora,
            Vendedor = dto.Vendedor,
            RazaoSocial = dto.RazaoSocial ?? "",
            NomeFantasia = dto.NomeFantasia ?? "",
            Tipo = dto.Tipo,
            CpfECnpj = dto.CpfECnpj ?? "",
            RgEIe = dto.RgEIe ?? "",
            CodigoDoSuframa = dto.CodigoDoSuframa ?? "",
            CodigoDaAntt = dto.CodigoDaAntt ?? "",
            Endereco = dto.Endereco ?? "",
            NumeroDoEndereco = dto.NumeroDoEndereco ?? "",
            Complemento = dto.Complemento ?? "",
            Bairro = dto.Bairro ?? "",
            CaixaPostal = dto.CaixaPostal ?? "",
            SequenciaDoMunicipio = dto.SequenciaDoMunicipio,
            Cep = dto.Cep ?? "",
            SequenciaDoPais = dto.SequenciaDoPais,
            Fone1 = dto.Fone1 ?? "",
            Fone2 = dto.Fone2 ?? "",
            Fax = dto.Fax ?? "",
            Celular = dto.Celular ?? "",
            Contato = dto.Contato ?? "",
            Email = dto.Email ?? "",
            HomePage = dto.HomePage ?? "",
            Observacao = dto.Observacao ?? "",
            Revenda = dto.Revenda,
            Isento = dto.Isento,
            OrgonPublico = dto.OrgonPublico,
            EmpresaProdutor = dto.EmpresaProdutor,
            Cumulativo = dto.Cumulativo,
            Inativo = dto.Inativo,
            SequenciaDoVendedor = dto.SequenciaDoVendedor,
            IntermediarioDoVendedor = dto.IntermediarioDoVendedor,
            EnderecoDeCobranca = dto.EnderecoDeCobranca ?? "",
            NumeroDoEnderecoDeCobranca = dto.NumeroDoEnderecoDeCobranca ?? "",
            ComplementoDaCobranca = dto.ComplementoDaCobranca ?? "",
            BairroDeCobranca = dto.BairroDeCobranca ?? "",
            CaixaPostalDaCobranca = dto.CaixaPostalDaCobranca ?? "",
            SequenciaMunicipioCobranca = dto.SequenciaMunicipioCobranca,
            CepDeCobranca = dto.CepDeCobranca ?? "",
            NomeDoBanco1 = dto.NomeDoBanco1 ?? "",
            AgenciaDoBanco1 = dto.AgenciaDoBanco1 ?? "",
            ContaCorrenteDoBanco1 = dto.ContaCorrenteDoBanco1 ?? "",
            NomeDoCorrentistaDoBanco1 = dto.NomeDoCorrentistaDoBanco1 ?? "",
            NomeDoBanco2 = dto.NomeDoBanco2 ?? "",
            AgenciaDoBanco2 = dto.AgenciaDoBanco2 ?? "",
            ContaCorrenteDoBanco2 = dto.ContaCorrenteDoBanco2 ?? "",
            NomeDoCorrentistaDoBanco2 = dto.NomeDoCorrentistaDoBanco2 ?? "",
            DataDeNascimento = dto.DataDeNascimento,
            CodigoContabil = dto.CodigoContabil,
            CodigoAdiantamento = dto.CodigoAdiantamento,
            SalBruto = dto.SalBruto,
            WhatsAppSincronizado = dto.WhatsAppSincronizado
        };
    }

    #endregion
}

#region Classes Auxiliares para API

internal class BrasilApiCnpjResponse
{
    public string? razao_social { get; set; }
    public string? nome_fantasia { get; set; }
    public string? logradouro { get; set; }
    public string? numero { get; set; }
    public string? complemento { get; set; }
    public string? bairro { get; set; }
    public string? cep { get; set; }
    public string? municipio { get; set; }
    public string? uf { get; set; }
    public string? ddd_telefone_1 { get; set; }
    public string? email { get; set; }
}

internal class BrasilApiCepResponse
{
    public string? cep { get; set; }
    public string? state { get; set; }
    public string? city { get; set; }
    public string? neighborhood { get; set; }
    public string? street { get; set; }
}

#endregion
