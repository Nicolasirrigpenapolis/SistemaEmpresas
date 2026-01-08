using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs.Fiscal;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services.Fiscal;

public class SefazService : ISefazService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly ILogger<SefazService> _logger;

    public SefazService(
        HttpClient httpClient,
        AppDbContext context,
        ILogger<SefazService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
    }

    public async Task<Stream?> BuscarNFePorChaveAsync(string chaveAcesso)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso) || chaveAcesso.Length != 44)
            {
                throw new ArgumentException("Chave de acesso inválida. Deve conter 44 dígitos.");
            }

            // 1. Obter emitente ativo para pegar CNPJ e Ambiente
            var emitente = await _context.Emitentes
                .AsNoTracking()
                .Where(e => e.Ativo)
                .FirstOrDefaultAsync();

            if (emitente == null)
            {
                throw new Exception("Nenhum emitente ativo configurado no sistema.");
            }

            var cnpjDestinatario = emitente.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            var ambiente = emitente.AmbienteNfe; // 1 = Produção, 2 = Homologação
            var ufAutor = emitente.CodigoMunicipio.Substring(0, 2); // Pega os 2 primeiros dígitos do IBGE (UF)

            // 2. Montar o XML de solicitação (distDFeInt)
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
            var distDFeInt = new XElement(ns + "distDFeInt",
                new XAttribute("versao", "1.01"),
                new XElement(ns + "tpAmb", ambiente),
                new XElement(ns + "cUFAutor", ufAutor),
                new XElement(ns + "CNPJ", cnpjDestinatario),
                new XElement(ns + "consChNFe",
                    new XElement(ns + "chNFe", chaveAcesso)
                )
            );

            // 3. Envelopar em SOAP
            var soapEnvelope = MontarSoapEnvelope(distDFeInt.ToString());

            // 4. Definir Endpoint
            var url = ambiente == 1 
                ? "https://www1.nfe.fazenda.gov.br/NFeDistribuicaoDFe/NFeDistribuicaoDFe.asmx"
                : "https://hom1.nfe.fazenda.gov.br/NFeDistribuicaoDFe/NFeDistribuicaoDFe.asmx";

            // 5. Enviar Requisição
            var content = new StringContent(soapEnvelope, Encoding.UTF8, "application/soap+xml");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/soap+xml");
            // SEFAZ exige o header SOAPAction ou action no Content-Type
            content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("action", "\"http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe/nfeDistDFeInteresse\""));

            _logger.LogInformation("Enviando requisição de distribuição DFe para SEFAZ. Chave: {Chave}", chaveAcesso);
            
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseXml = await response.Content.ReadAsStringAsync();
            return ProcessarRespostaSefaz(responseXml);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar NFe na SEFAZ pela chave {Chave}", chaveAcesso);
            throw;
        }
    }

    private string MontarSoapEnvelope(string bodyContent)
    {
        return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    <nfeDistDFeInteresse xmlns=""http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe"">
      <nfeDadosMsg>{bodyContent}</nfeDadosMsg>
    </nfeDistDFeInteresse>
  </soap12:Body>
</soap12:Envelope>";
    }

    private Stream? ProcessarRespostaSefaz(string responseXml)
    {
        try
        {
            var doc = XDocument.Parse(responseXml);
            XNamespace soapNs = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe";
            XNamespace nfeNs = "http://www.portalfiscal.inf.br/nfe";

            // Localizar a tag retDistDFeInt dentro do envelope SOAP
            var retDistDFeInt = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "retDistDFeInt");
            
            if (retDistDFeInt == null)
            {
                _logger.LogWarning("Resposta da SEFAZ não contém a tag retDistDFeInt.");
                return null;
            }

            var cStat = retDistDFeInt.Element(nfeNs + "cStat")?.Value;
            var xMotivo = retDistDFeInt.Element(nfeNs + "xMotivo")?.Value;

            if (cStat != "138") // 138 = Documento localizado
            {
                _logger.LogWarning("SEFAZ retornou status {Stat}: {Motivo}", cStat, xMotivo);
                return null;
            }

            // Pegar o primeiro documento retornado (docZip)
            var docZip = retDistDFeInt.Element(nfeNs + "loteDistDFeInt")?.Element(nfeNs + "docZip");
            
            if (docZip == null)
            {
                _logger.LogWarning("Documento localizado, mas lote de documentos está vazio.");
                return null;
            }

            // O conteúdo vem em Base64 e compactado em GZip
            var base64Content = docZip.Value;
            var compressedBytes = Convert.FromBase64String(base64Content);

            using var ms = new MemoryStream(compressedBytes);
            using var decompressionStream = new GZipStream(ms, CompressionMode.Decompress);
            var resultStream = new MemoryStream();
            decompressionStream.CopyTo(resultStream);
            resultStream.Position = 0;

            return resultStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar resposta XML da SEFAZ");
            return null;
        }
    }
}
