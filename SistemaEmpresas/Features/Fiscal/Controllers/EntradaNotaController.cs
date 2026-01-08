using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.Features.Fiscal.Services;
using SistemaEmpresas.Features.Fiscal.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace SistemaEmpresas.Features.Fiscal.Controllers;

[Authorize]
[ApiController]
[Route("api/fiscal/entrada-nota")]
public class EntradaNotaController : ControllerBase
{
    private readonly INFeXmlParserService _xmlParserService;

    public EntradaNotaController(INFeXmlParserService xmlParserService)
    {
        _xmlParserService = xmlParserService;
    }

    [HttpPost("upload-xml")]
    public IActionResult UploadXml(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        if (!file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            return BadRequest("O arquivo deve ser um XML.");

        try
        {
            using var stream = file.OpenReadStream();
            var nfeData = _xmlParserService.Parse(stream);
            
            // TODO: Aqui poderíamos buscar no banco se o emitente já existe
            // e se os produtos já estão vinculados (De/Para)
            
            return Ok(nfeData);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao processar XML: {ex.Message}");
        }
    }
}
