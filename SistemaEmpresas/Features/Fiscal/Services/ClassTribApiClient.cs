using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Features.Fiscal.Dtos;

namespace SistemaEmpresas.Features.Fiscal.Services;

/// <summary>
/// Cliente HTTP para consumir a API SVRS ClassTrib
/// Endpoint: https://cff.svrs.rs.gov.br/api/v1/consultas/classTrib
/// </summary>
public class ClassTribApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ClassTribApiClient> _logger;
    private const string API_BASE_URL = "https://cff.svrs.rs.gov.br/api/v1/";
    // IMPORTANTE: endpoint sem barra inicial para concatenar corretamente com BaseAddress
    private const string ENDPOINT_CLASSTRIB = "consultas/classTrib";

    public ClassTribApiClient(HttpClient httpClient, ILogger<ClassTribApiClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Configurar base address se não estiver configurado
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri(API_BASE_URL);
        }

        // Configurar timeout
        _httpClient.Timeout = TimeSpan.FromSeconds(60);
    }

    /// <summary>
    /// Busca todas as classificações tributárias da API SVRS
    /// Retorna lista de CSTs com suas classificações
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de ClassTribApiResponse com todos os CSTs e classificações</returns>
    /// <exception cref="HttpRequestException">Erro na comunicação HTTP</exception>
    /// <exception cref="InvalidOperationException">Erro na desserialização JSON</exception>
    public async Task<List<ClassTribApiResponse>> BuscarTodasClassificacoesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iniciando consulta à API SVRS ClassTrib: {Endpoint}", ENDPOINT_CLASSTRIB);

            var response = await _httpClient.GetAsync(ENDPOINT_CLASSTRIB, cancellationToken);

            // Log do status code
            _logger.LogInformation("Resposta da API SVRS recebida. Status: {StatusCode}", response.StatusCode);

            // Lançar exceção se status não for success
            response.EnsureSuccessStatusCode();

            // Desserializar JSON para lista de ClassTribApiResponse
            var classificacoes = await response.Content.ReadFromJsonAsync<List<ClassTribApiResponse>>(
                cancellationToken: cancellationToken);

            if (classificacoes == null)
            {
                _logger.LogWarning("API SVRS retornou resposta vazia ou nula");
                return new List<ClassTribApiResponse>();
            }

            _logger.LogInformation(
                "API SVRS: Recebidos {TotalCST} CSTs com total de {TotalClassificacoes} classificações",
                classificacoes.Count,
                ContarTotalClassificacoes(classificacoes));

            return classificacoes;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, 
                "Erro HTTP ao consultar API SVRS ClassTrib. Endpoint: {Endpoint}", 
                ENDPOINT_CLASSTRIB);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, 
                "Timeout ao consultar API SVRS ClassTrib. Endpoint: {Endpoint}", 
                ENDPOINT_CLASSTRIB);
            throw new HttpRequestException("Timeout na requisição à API SVRS", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Erro inesperado ao consultar API SVRS ClassTrib");
            throw;
        }
    }

    /// <summary>
    /// Busca classificações tributárias filtradas por CST específico
    /// </summary>
    /// <param name="cst">Código de Situação Tributária (ex: "000", "200", "410")</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>ClassTribApiResponse do CST solicitado ou null se não encontrado</returns>
    public async Task<ClassTribApiResponse?> BuscarPorCstAsync(
        string cst, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cst))
        {
            throw new ArgumentException("CST não pode ser vazio", nameof(cst));
        }

        _logger.LogInformation("Buscando classificações para CST: {CST}", cst);

        var todasClassificacoes = await BuscarTodasClassificacoesAsync(cancellationToken);
        
        var resultado = todasClassificacoes.Find(c => 
            c.CodigoSituacaoTributaria.Equals(cst, StringComparison.OrdinalIgnoreCase));

        if (resultado == null)
        {
            _logger.LogWarning("CST não encontrado na API SVRS: {CST}", cst);
        }
        else
        {
            _logger.LogInformation(
                "CST {CST} encontrado com {Total} classificações",
                cst,
                resultado.ClassificacoesTributarias.Count);
        }

        return resultado;
    }

    /// <summary>
    /// Busca classificação tributária específica por código cClassTrib
    /// </summary>
    /// <param name="cClassTrib">Código da Classificação Tributária (ex: "000001", "200047")</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>ClassificacaoTributariaApiDto se encontrado, null caso contrário</returns>
    public async Task<ClassificacaoTributariaApiDto?> BuscarPorCodigoClassTribAsync(
        string cClassTrib,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cClassTrib))
        {
            throw new ArgumentException("cClassTrib não pode ser vazio", nameof(cClassTrib));
        }

        _logger.LogInformation("Buscando classificação por cClassTrib: {cClassTrib}", cClassTrib);

        var todasClassificacoes = await BuscarTodasClassificacoesAsync(cancellationToken);

        foreach (var cst in todasClassificacoes)
        {
            var classificacao = cst.ClassificacoesTributarias.Find(ct =>
                ct.CodigoClassificacaoTributaria.Equals(cClassTrib, StringComparison.OrdinalIgnoreCase));

            if (classificacao != null)
            {
                _logger.LogInformation(
                    "cClassTrib {cClassTrib} encontrado no CST {CST}",
                    cClassTrib,
                    cst.CodigoSituacaoTributaria);
                return classificacao;
            }
        }

        _logger.LogWarning("cClassTrib não encontrado na API SVRS: {cClassTrib}", cClassTrib);
        return null;
    }

    /// <summary>
    /// Busca apenas classificações válidas para NFe
    /// MÉTODO PRINCIPAL - Foco do sistema
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de todas as classificações válidas para NFe</returns>
    public async Task<List<ClassificacaoTributariaApiDto>> BuscarClassificacoesNFeAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Buscando classificações válidas para NFe (foco principal)");

        var todasClassificacoes = await BuscarTodasClassificacoesAsync(cancellationToken);
        var classificacoesNFe = new List<ClassificacaoTributariaApiDto>();

        foreach (var cst in todasClassificacoes)
        {
            var validasNFe = cst.ClassificacoesTributarias
                .Where(ct => ct.ValidoParaNFe)
                .ToList();

            classificacoesNFe.AddRange(validasNFe);
        }

        _logger.LogInformation(
            "Total de classificações válidas para NFe: {Total}",
            classificacoesNFe.Count);

        return classificacoesNFe;
    }

    /// <summary>
    /// Verifica se a API SVRS está disponível e respondendo
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se API está disponível, False caso contrário</returns>
    public async Task<bool> VerificarDisponibilidadeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Verificando disponibilidade da API SVRS");

            var response = await _httpClient.GetAsync(ENDPOINT_CLASSTRIB, cancellationToken);
            
            var disponivel = response.IsSuccessStatusCode;
            
            _logger.LogInformation(
                "API SVRS disponibilidade: {Disponivel} (Status: {StatusCode})",
                disponivel,
                response.StatusCode);

            return disponivel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar disponibilidade da API SVRS");
            return false;
        }
    }

    #region Métodos Auxiliares

    /// <summary>
    /// Conta o total de classificações tributárias em todos os CSTs
    /// </summary>
    private static int ContarTotalClassificacoes(List<ClassTribApiResponse> classificacoes)
    {
        return classificacoes.Sum(c => c.ClassificacoesTributarias.Count);
    }

    #endregion
}
