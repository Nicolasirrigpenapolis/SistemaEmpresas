using System.Xml.Linq;
using System.Globalization;
using SistemaEmpresas.Features.Fiscal.Dtos;

namespace SistemaEmpresas.Features.Fiscal.Services;

public interface INFeXmlParserService
{
    NFeImportDto Parse(Stream xmlStream);
}

public class NFeXmlParserService : INFeXmlParserService
{
    private static readonly XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

    public NFeImportDto Parse(Stream xmlStream)
    {
        var doc = XDocument.Load(xmlStream);
        var root = doc.Root;
        
        if (root == null)
            throw new Exception("XML de NFe inválido ou vazio.");

        XElement? nfe = null;
        if (root.Name == ns + "nfeProc")
        {
            nfe = root.Element(ns + "NFe")?.Element(ns + "infNFe");
        }
        else if (root.Name == ns + "NFe")
        {
            nfe = root.Element(ns + "infNFe");
        }
        else if (root.Name.LocalName == "nfeProc") // Caso venha sem namespace (raro mas possível)
        {
            nfe = root.Element("NFe")?.Element("infNFe");
        }

        if (nfe == null)
            throw new Exception("Não foi possível encontrar a tag <infNFe> no XML. Verifique se o arquivo é uma NFe válida.");

        var ide = nfe.Element(ns + "ide") ?? nfe.Element("ide");
        var emit = nfe.Element(ns + "emit") ?? nfe.Element("emit");
        var total = (nfe.Element(ns + "total") ?? nfe.Element("total"))?.Element(ns + "ICMSTot") 
                    ?? (nfe.Element(ns + "total") ?? nfe.Element("total"))?.Element("ICMSTot");

        var dto = new NFeImportDto
        {
            ChaveAcesso = nfe.Attribute("Id")?.Value?.Replace("NFe", "") ?? "",
            NumeroNota = ide?.Element(ns + "nNF")?.Value ?? ide?.Element("nNF")?.Value ?? "",
            Serie = ide?.Element(ns + "serie")?.Value ?? ide?.Element("serie")?.Value ?? "",
            DataEmissao = ParseDateTime(ide?.Element(ns + "dhEmi")?.Value ?? ide?.Element("dhEmi")?.Value),
            
            Emitente = new EmitenteDto
            {
                Cnpj = emit?.Element(ns + "CNPJ")?.Value ?? emit?.Element("CNPJ")?.Value ?? "",
                Nome = emit?.Element(ns + "xNome")?.Value ?? emit?.Element("xNome")?.Value ?? "",
                NomeFantasia = emit?.Element(ns + "xFant")?.Value ?? emit?.Element("xFant")?.Value ?? "",
                InscricaoEstadual = emit?.Element(ns + "IE")?.Value ?? emit?.Element("IE")?.Value ?? ""
            },
            
            ValorTotal = ParseDecimal(total?.Element(ns + "vNF")?.Value ?? total?.Element("vNF")?.Value),
            ValorProdutos = ParseDecimal(total?.Element(ns + "vProd")?.Value ?? total?.Element("vProd")?.Value),
            ValorIcms = ParseDecimal(total?.Element(ns + "vICMS")?.Value ?? total?.Element("vICMS")?.Value),
            ValorIpi = ParseDecimal(total?.Element(ns + "vIPI")?.Value ?? total?.Element("vIPI")?.Value),
            
            Itens = nfe.Elements(ns + "det").Concat(nfe.Elements("det")).Select(det => {
                var prod = det.Element(ns + "prod") ?? det.Element("prod");
                var imposto = det.Element(ns + "imposto") ?? det.Element("imposto");
                var icms = imposto?.Element(ns + "ICMS")?.Elements().FirstOrDefault() 
                           ?? imposto?.Element("ICMS")?.Elements().FirstOrDefault();
                var ipi = (imposto?.Element(ns + "IPI") ?? imposto?.Element("IPI"))?.Element(ns + "IPITrib")
                          ?? (imposto?.Element(ns + "IPI") ?? imposto?.Element("IPI"))?.Element("IPITrib");

                return new NFeItemDto
                {
                    CodigoProdutoFornecedor = prod?.Element(ns + "cProd")?.Value ?? prod?.Element("cProd")?.Value ?? "",
                    DescricaoProdutoFornecedor = prod?.Element(ns + "xProd")?.Value ?? prod?.Element("xProd")?.Value ?? "",
                    Ncm = prod?.Element(ns + "NCM")?.Value ?? prod?.Element("NCM")?.Value ?? "",
                    Cfop = prod?.Element(ns + "CFOP")?.Value ?? prod?.Element("CFOP")?.Value ?? "",
                    UnidadeMedida = prod?.Element(ns + "uCom")?.Value ?? prod?.Element("uCom")?.Value ?? "",
                    Quantidade = ParseDecimal(prod?.Element(ns + "qCom")?.Value ?? prod?.Element("qCom")?.Value),
                    ValorUnitario = ParseDecimal(prod?.Element(ns + "vUnCom")?.Value ?? prod?.Element("vUnCom")?.Value),
                    ValorTotal = ParseDecimal(prod?.Element(ns + "vProd")?.Value ?? prod?.Element("vProd")?.Value),
                    
                    ValorIcms = ParseDecimal(icms?.Element(ns + "vICMS")?.Value ?? icms?.Element("vICMS")?.Value),
                    AliquotaIcms = ParseDecimal(icms?.Element(ns + "pICMS")?.Value ?? icms?.Element("pICMS")?.Value),
                    ValorIpi = ParseDecimal(ipi?.Element(ns + "vIPI")?.Value ?? ipi?.Element("vIPI")?.Value)
                };
            }).ToList()
        };

        return dto;
    }

    private DateTime ParseDateTime(string? value)
    {
        if (string.IsNullOrEmpty(value)) return DateTime.Now;
        
        if (DateTime.TryParse(value, null, DateTimeStyles.RoundtripKind, out var result))
            return result;
            
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            return result;

        return DateTime.Now;
    }

    private decimal ParseDecimal(string? value)
    {
        if (string.IsNullOrEmpty(value)) return 0;
        
        // Tenta com InvariantCulture (ponto como decimal)
        if (decimal.TryParse(value, CultureInfo.InvariantCulture, out var result))
            return result;
            
        // Tenta com a cultura atual (caso venha com vírgula)
        if (decimal.TryParse(value, out result))
            return result;

        return 0;
    }
}
