using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.Services.Certificados;

namespace SistemaEmpresas.Services.Fiscal;

/// <summary>
/// Handler que injeta dinamicamente o certificado digital do emitente atual nas requisições HTTP
/// </summary>
public class CertificadoDinamicoHandler : DelegatingHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CertificadoDinamicoHandler> _logger;

    public CertificadoDinamicoHandler(
        IServiceProvider serviceProvider,
        ILogger<CertificadoDinamicoHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Criar um scope para resolver serviços scoped
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var certificadoService = scope.ServiceProvider.GetRequiredService<ICertificadoService>();

        try
        {
            // Buscar o emitente ativo (primeiro emitente ativo = emitente da empresa)
            var emitente = await context.Emitentes
                .AsNoTracking()
                .Where(e => e.Ativo)
                .FirstOrDefaultAsync(cancellationToken);

            if (emitente != null && !string.IsNullOrWhiteSpace(emitente.CaminhoCertificado))
            {
                // Carregar certificado do emitente
                var certificado = certificadoService.CarregarCertificado(emitente);

                if (certificado != null)
                {
                    // Validar certificado
                    var (isValido, mensagem) = certificadoService.ValidarCertificado(certificado);

                    if (isValido)
                    {
                        // Anexar certificado ao handler
                        var handler = GetHttpClientHandler();
                        if (handler != null)
                        {
                            handler.ClientCertificates.Clear();
                            handler.ClientCertificates.Add(certificado);
                            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

                            _logger.LogInformation(
                                "Certificado do emitente '{Emitente}' anexado à requisição. Subject: {Subject}",
                                emitente.RazaoSocial,
                                certificado.Subject);
                        }
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Certificado do emitente '{Emitente}' inválido: {Mensagem}",
                            emitente.RazaoSocial,
                            mensagem);
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "Não foi possível carregar o certificado do emitente '{Emitente}'",
                        emitente.RazaoSocial);
                }
            }
            else
            {
                _logger.LogWarning("Nenhum emitente ativo com certificado configurado encontrado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar certificado do emitente para requisição HTTP");
        }

        // Continuar com a requisição
        return await base.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// Obtém o HttpClientHandler da cadeia de handlers
    /// </summary>
    private HttpClientHandler? GetHttpClientHandler()
    {
        var handler = InnerHandler;
        while (handler != null)
        {
            if (handler is HttpClientHandler httpClientHandler)
            {
                return httpClientHandler;
            }

            if (handler is DelegatingHandler delegatingHandler)
            {
                handler = delegatingHandler.InnerHandler;
            }
            else
            {
                break;
            }
        }

        return null;
    }
}
