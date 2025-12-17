using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;
using SistemaEmpresas.Services.Certificados;
using System.Text.Json;

namespace SistemaEmpresas.Controllers.Emitentes;

[Route("api/[controller]")]
[ApiController]
public class EmitentesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<EmitentesController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICertificadoService _certificadoService;

    public EmitentesController(
        AppDbContext context,
        ILogger<EmitentesController> logger,
        IHttpClientFactory httpClientFactory,
        ICertificadoService certificadoService)
    {
        _context = context;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _certificadoService = certificadoService;
    }

    /// <summary>
    /// Lista todos os emitentes
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmitenteListDto>>> GetEmitentes()
    {
        var emitentes = await _context.Emitentes
            .Select(e => new EmitenteListDto
            {
                Id = e.Id,
                Cnpj = e.Cnpj,
                RazaoSocial = e.RazaoSocial,
                NomeFantasia = e.NomeFantasia,
                InscricaoEstadual = e.InscricaoEstadual,
                Municipio = e.Municipio,
                Uf = e.Uf,
                Ativo = e.Ativo,
                AmbienteNfe = e.AmbienteNfe,
                PossuiCertificado = !string.IsNullOrWhiteSpace(e.CaminhoCertificado),
                CertificadoValido = e.ValidadeCertificado.HasValue && e.ValidadeCertificado.Value > DateTime.Now,
                ValidadeCertificado = e.ValidadeCertificado
            })
            .ToListAsync();

        return Ok(emitentes);
    }

    /// <summary>
    /// Obtém o emitente ativo (empresa logada)
    /// Retorna o primeiro emitente ativo cadastrado
    /// </summary>
    [HttpGet("atual")]
    public async Task<ActionResult<EmitenteDto>> GetEmitenteAtual()
    {
        var emitente = await _context.Emitentes
            .Where(e => e.Ativo)
            .FirstOrDefaultAsync();

        if (emitente == null)
        {
            return NotFound(new { message = "Nenhum emitente cadastrado para esta empresa. Por favor, cadastre os dados do emitente." });
        }

        return Ok(MapToDto(emitente));
    }

    /// <summary>
    /// Obtém um emitente por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmitenteDto>> GetEmitente(int id)
    {
        var emitente = await _context.Emitentes.FindAsync(id);

        if (emitente == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(emitente));
    }

    /// <summary>
    /// Cria um novo emitente
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmitenteDto>> CreateEmitente(EmitenteCreateUpdateDto dto)
    {
        // Verifica se já existe emitente com este CNPJ
        var existente = await _context.Emitentes
            .AnyAsync(e => e.Cnpj == dto.Cnpj);

        if (existente)
        {
            return BadRequest(new { message = "Já existe um emitente cadastrado com este CNPJ." });
        }

        var emitente = new Emitente
        {
            Cnpj = dto.Cnpj,
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia,
            InscricaoEstadual = dto.InscricaoEstadual,
            InscricaoMunicipal = dto.InscricaoMunicipal,
            Cnae = dto.Cnae,
            CodigoRegimeTributario = dto.CodigoRegimeTributario,
            Endereco = dto.Endereco,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            CodigoMunicipio = dto.CodigoMunicipio,
            Municipio = dto.Municipio,
            Uf = dto.Uf,
            Cep = dto.Cep,
            CodigoPais = dto.CodigoPais,
            Pais = dto.Pais,
            Telefone = dto.Telefone,
            Email = dto.Email,
            AmbienteNfe = dto.AmbienteNfe,
            SerieNfe = dto.SerieNfe,
            ProximoNumeroNfe = dto.ProximoNumeroNfe,
            CaminhoCertificado = dto.CaminhoCertificado,
            SenhaCertificado = dto.SenhaCertificado,
            ValidadeCertificado = dto.ValidadeCertificado,
            Ativo = dto.Ativo,
            DataCriacao = DateTime.Now
        };

        _context.Emitentes.Add(emitente);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Emitente criado: {RazaoSocial} - CNPJ: {Cnpj}", emitente.RazaoSocial, emitente.Cnpj);

        return CreatedAtAction(nameof(GetEmitente), new { id = emitente.Id }, MapToDto(emitente));
    }

    /// <summary>
    /// Atualiza um emitente existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmitente(int id, EmitenteCreateUpdateDto dto)
    {
        var emitente = await _context.Emitentes.FindAsync(id);

        if (emitente == null)
        {
            return NotFound();
        }

        // Verifica se outro emitente já usa este CNPJ
        var cnpjEmUso = await _context.Emitentes
            .AnyAsync(e => e.Cnpj == dto.Cnpj && e.Id != id);

        if (cnpjEmUso)
        {
            return BadRequest(new { message = "Este CNPJ já está em uso por outro emitente." });
        }

        emitente.Cnpj = dto.Cnpj;
        emitente.RazaoSocial = dto.RazaoSocial;
        emitente.NomeFantasia = dto.NomeFantasia;
        emitente.InscricaoEstadual = dto.InscricaoEstadual;
        emitente.InscricaoMunicipal = dto.InscricaoMunicipal;
        emitente.Cnae = dto.Cnae;
        emitente.CodigoRegimeTributario = dto.CodigoRegimeTributario;
        emitente.Endereco = dto.Endereco;
        emitente.Numero = dto.Numero;
        emitente.Complemento = dto.Complemento;
        emitente.Bairro = dto.Bairro;
        emitente.CodigoMunicipio = dto.CodigoMunicipio;
        emitente.Municipio = dto.Municipio;
        emitente.Uf = dto.Uf;
        emitente.Cep = dto.Cep;
        emitente.CodigoPais = dto.CodigoPais;
        emitente.Pais = dto.Pais;
        emitente.Telefone = dto.Telefone;
        emitente.Email = dto.Email;
        emitente.AmbienteNfe = dto.AmbienteNfe;
        emitente.SerieNfe = dto.SerieNfe;
        emitente.ProximoNumeroNfe = dto.ProximoNumeroNfe;
        emitente.CaminhoCertificado = dto.CaminhoCertificado;
        emitente.ValidadeCertificado = dto.ValidadeCertificado;
        emitente.Ativo = dto.Ativo;
        emitente.DataAtualizacao = DateTime.Now;

        // Só atualiza a senha se foi informada
        if (!string.IsNullOrEmpty(dto.SenhaCertificado))
        {
            emitente.SenhaCertificado = dto.SenhaCertificado;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Emitente atualizado: {RazaoSocial} - CNPJ: {Cnpj}", emitente.RazaoSocial, emitente.Cnpj);

        return Ok(MapToDto(emitente));
    }

    /// <summary>
    /// Remove um emitente
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmitente(int id)
    {
        var emitente = await _context.Emitentes.FindAsync(id);

        if (emitente == null)
        {
            return NotFound();
        }

        _context.Emitentes.Remove(emitente);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Emitente removido: {RazaoSocial} - CNPJ: {Cnpj}", emitente.RazaoSocial, emitente.Cnpj);

        return NoContent();
    }

    /// <summary>
    /// Consulta CNPJ na ReceitaWS e retorna os dados
    /// </summary>
    [HttpGet("consultar-cnpj/{cnpj}")]
    public async Task<ActionResult<ConsultaCnpjDto>> ConsultarCnpj(string cnpj)
    {
        // Remove formatação do CNPJ
        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14)
        {
            return BadRequest(new { message = "CNPJ inválido. Deve conter 14 dígitos." });
        }

        // Verifica se já consultou recentemente (evita consultas excessivas)
        var emitenteExistente = await _context.Emitentes
            .FirstOrDefaultAsync(e => e.Cnpj == cnpj);

        if (emitenteExistente != null && emitenteExistente.DataConsultaCnpj.HasValue)
        {
            var ultimaConsulta = emitenteExistente.DataConsultaCnpj.Value;
            var diferencaHoras = (DateTime.Now - ultimaConsulta).TotalHours;
            
            // Se consultou nas últimas 24 horas, retorna os dados do banco
            if (diferencaHoras < 24)
            {
                _logger.LogInformation("Retornando dados do CNPJ {Cnpj} do cache (última consulta: {UltimaConsulta})", cnpj, ultimaConsulta);
                return Ok(new ConsultaCnpjDto
                {
                    Cnpj = emitenteExistente.Cnpj,
                    RazaoSocial = emitenteExistente.RazaoSocial,
                    NomeFantasia = emitenteExistente.NomeFantasia,
                    Endereco = emitenteExistente.Endereco,
                    Numero = emitenteExistente.Numero,
                    Complemento = emitenteExistente.Complemento,
                    Bairro = emitenteExistente.Bairro,
                    CodigoMunicipio = emitenteExistente.CodigoMunicipio,
                    Municipio = emitenteExistente.Municipio,
                    Uf = emitenteExistente.Uf,
                    Cep = emitenteExistente.Cep,
                    Telefone = emitenteExistente.Telefone,
                    Email = emitenteExistente.Email,
                    Cnae = emitenteExistente.Cnae,
                    DataConsulta = ultimaConsulta
                });
            }
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            // Consulta ReceitaWS (API gratuita)
            var response = await httpClient.GetAsync($"https://www.receitaws.com.br/v1/cnpj/{cnpj}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha ao consultar CNPJ {Cnpj}: {StatusCode}", cnpj, response.StatusCode);
                return BadRequest(new { message = "Não foi possível consultar o CNPJ. Tente novamente mais tarde." });
            }

            var json = await response.Content.ReadAsStringAsync();
            var dados = JsonSerializer.Deserialize<ReceitaWsResponse>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (dados == null || dados.Status == "ERROR")
            {
                return BadRequest(new { message = dados?.Message ?? "CNPJ não encontrado ou inválido." });
            }

            // Extrai código IBGE do município
            var codigoMunicipio = "";
            if (!string.IsNullOrEmpty(dados.Municipio) && !string.IsNullOrEmpty(dados.Uf))
            {
                // Busca o código IBGE no banco de municípios
                var municipio = await _context.Municipios
                    .FirstOrDefaultAsync(m => m.Descricao.ToUpper() == dados.Municipio.ToUpper() && m.Uf == dados.Uf);
                
                if (municipio != null)
                {
                    codigoMunicipio = municipio.CodigoDoIbge.ToString();
                }
            }

            var resultado = new ConsultaCnpjDto
            {
                Cnpj = cnpj,
                RazaoSocial = dados.Nome ?? "",
                NomeFantasia = dados.Fantasia,
                Endereco = dados.Logradouro ?? "",
                Numero = dados.Numero ?? "S/N",
                Complemento = dados.Complemento,
                Bairro = dados.Bairro ?? "",
                CodigoMunicipio = codigoMunicipio,
                Municipio = dados.Municipio ?? "",
                Uf = dados.Uf ?? "",
                Cep = new string((dados.Cep ?? "").Where(char.IsDigit).ToArray()),
                Telefone = new string((dados.Telefone ?? "").Where(c => char.IsDigit(c) || c == ' ').ToArray()).Trim(),
                Email = dados.Email?.ToLower(),
                AtividadePrincipal = dados.AtividadePrincipal?.FirstOrDefault()?.Text,
                Cnae = dados.AtividadePrincipal?.FirstOrDefault()?.Code?.Replace(".", "").Replace("-", ""),
                Situacao = dados.Situacao,
                DataConsulta = DateTime.Now
            };

            _logger.LogInformation("CNPJ {Cnpj} consultado com sucesso: {RazaoSocial}", cnpj, resultado.RazaoSocial);

            return Ok(resultado);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro ao consultar CNPJ {Cnpj}", cnpj);
            return BadRequest(new { message = "Erro de conexão ao consultar CNPJ. Verifique sua conexão com a internet." });
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao consultar CNPJ {Cnpj}", cnpj);
            return BadRequest(new { message = "Tempo de consulta esgotado. Tente novamente." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao consultar CNPJ {Cnpj}", cnpj);
            return BadRequest(new { message = "Erro ao consultar CNPJ. Tente novamente mais tarde." });
        }
    }

    /// <summary>
    /// Faz upload do certificado digital (.pfx) para um emitente
    /// </summary>
    [HttpPost("{id}/certificado")]
    public async Task<ActionResult> UploadCertificado(int id, [FromForm] CertificadoUploadDto dto)
    {
        var emitente = await _context.Emitentes.FindAsync(id);

        if (emitente == null)
        {
            return NotFound(new { message = "Emitente não encontrado" });
        }

        if (dto.Arquivo == null || dto.Arquivo.Length == 0)
        {
            return BadRequest(new { message = "Arquivo do certificado é obrigatório" });
        }

        if (string.IsNullOrWhiteSpace(dto.Senha))
        {
            return BadRequest(new { message = "Senha do certificado é obrigatória" });
        }

        // Validar extensão do arquivo
        var extensao = Path.GetExtension(dto.Arquivo.FileName).ToLowerInvariant();
        if (extensao != ".pfx" && extensao != ".p12")
        {
            return BadRequest(new { message = "Apenas arquivos .pfx ou .p12 são aceitos" });
        }

        try
        {
            // Ler bytes do arquivo
            byte[] certificadoBytes;
            using (var memoryStream = new MemoryStream())
            {
                await dto.Arquivo.CopyToAsync(memoryStream);
                certificadoBytes = memoryStream.ToArray();
            }

            // Validar certificado antes de salvar
            var (certificadoValidado, mensagemErro) = _certificadoService.ValidarCertificadoUpload(certificadoBytes, dto.Senha);

            if (certificadoValidado == null)
            {
                return BadRequest(new { message = mensagemErro ?? "Certificado inválido" });
            }

            // Obter informações do certificado
            var info = _certificadoService.ObterInformacoesCertificado(certificadoValidado);

            // Remover certificado anterior se existir
            if (!string.IsNullOrWhiteSpace(emitente.CaminhoCertificado))
            {
                _certificadoService.RemoverCertificado(emitente.CaminhoCertificado);
            }

            // Salvar novo certificado
            var caminho = await _certificadoService.SalvarCertificadoAsync(id, certificadoBytes, dto.Senha);

            // Criptografar e salvar senha
            var senhaCriptografada = CertificadoService.CriptografarSenha(dto.Senha);

            // Atualizar emitente
            emitente.CaminhoCertificado = caminho;
            emitente.SenhaCertificado = senhaCriptografada;
            emitente.ValidadeCertificado = info.ValidoAte;
            emitente.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Certificado digital carregado para emitente {Emitente}. Validade: {Validade}",
                emitente.RazaoSocial,
                info.ValidoAte);

            return Ok(new
            {
                message = "Certificado carregado com sucesso",
                certificado = new
                {
                    subject = info.Subject,
                    issuer = info.Issuer,
                    validoApartirDe = info.ValidoApartirDe,
                    validoAte = info.ValidoAte,
                    estaValido = info.EstaValido,
                    estaProximoDoVencimento = info.EstaProximoDoVencimento,
                    diasRestantes = info.DiasRestantes
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer upload do certificado para emitente {EmitenteId}", id);
            return StatusCode(500, new { message = "Erro ao processar o certificado" });
        }
    }

    /// <summary>
    /// Valida e retorna informações do certificado digital de um emitente
    /// </summary>
    [HttpGet("{id}/certificado/validar")]
    public async Task<ActionResult> ValidarCertificado(int id)
    {
        var emitente = await _context.Emitentes.FindAsync(id);

        if (emitente == null)
        {
            return NotFound(new { message = "Emitente não encontrado" });
        }

        if (string.IsNullOrWhiteSpace(emitente.CaminhoCertificado))
        {
            return NotFound(new { message = "Emitente não possui certificado configurado" });
        }

        try
        {
            var certificado = _certificadoService.CarregarCertificado(emitente);

            if (certificado == null)
            {
                return BadRequest(new { message = "Não foi possível carregar o certificado" });
            }

            var info = _certificadoService.ObterInformacoesCertificado(certificado);
            var (isValido, mensagem) = _certificadoService.ValidarCertificado(certificado);

            return Ok(new
            {
                valido = isValido,
                mensagem,
                certificado = new
                {
                    subject = info.Subject,
                    issuer = info.Issuer,
                    validoApartirDe = info.ValidoApartirDe,
                    validoAte = info.ValidoAte,
                    estaValido = info.EstaValido,
                    estaProximoDoVencimento = info.EstaProximoDoVencimento,
                    diasRestantes = info.DiasRestantes,
                    thumbprint = info.Thumbprint,
                    serialNumber = info.SerialNumber,
                    possuiChavePrivada = info.PossuiChavePrivada
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar certificado do emitente {EmitenteId}", id);
            return StatusCode(500, new { message = "Erro ao validar o certificado" });
        }
    }

    /// <summary>
    /// Remove o certificado digital de um emitente
    /// </summary>
    [HttpDelete("{id}/certificado")]
    public async Task<ActionResult> RemoverCertificado(int id)
    {
        var emitente = await _context.Emitentes.FindAsync(id);

        if (emitente == null)
        {
            return NotFound(new { message = "Emitente não encontrado" });
        }

        if (string.IsNullOrWhiteSpace(emitente.CaminhoCertificado))
        {
            return NotFound(new { message = "Emitente não possui certificado configurado" });
        }

        try
        {
            // Remover arquivo físico
            _certificadoService.RemoverCertificado(emitente.CaminhoCertificado);

            // Limpar dados do banco
            emitente.CaminhoCertificado = null;
            emitente.SenhaCertificado = null;
            emitente.ValidadeCertificado = null;
            emitente.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Certificado removido do emitente {Emitente}", emitente.RazaoSocial);

            return Ok(new { message = "Certificado removido com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover certificado do emitente {EmitenteId}", id);
            return StatusCode(500, new { message = "Erro ao remover o certificado" });
        }
    }

    #region Métodos Auxiliares

    private static EmitenteDto MapToDto(Emitente e)
    {
        return new EmitenteDto
        {
            Id = e.Id,
            Cnpj = e.Cnpj,
            RazaoSocial = e.RazaoSocial,
            NomeFantasia = e.NomeFantasia,
            InscricaoEstadual = e.InscricaoEstadual,
            InscricaoMunicipal = e.InscricaoMunicipal,
            Cnae = e.Cnae,
            CodigoRegimeTributario = e.CodigoRegimeTributario,
            Endereco = e.Endereco,
            Numero = e.Numero,
            Complemento = e.Complemento,
            Bairro = e.Bairro,
            CodigoMunicipio = e.CodigoMunicipio,
            Municipio = e.Municipio,
            Uf = e.Uf,
            Cep = e.Cep,
            CodigoPais = e.CodigoPais,
            Pais = e.Pais,
            Telefone = e.Telefone,
            Email = e.Email,
            AmbienteNfe = e.AmbienteNfe,
            SerieNfe = e.SerieNfe,
            ProximoNumeroNfe = e.ProximoNumeroNfe,
            CaminhoCertificado = e.CaminhoCertificado,
            ValidadeCertificado = e.ValidadeCertificado,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            DataConsultaCnpj = e.DataConsultaCnpj
        };
    }

    #endregion
}

/// <summary>
/// Classe para deserializar resposta da ReceitaWS
/// </summary>
internal class ReceitaWsResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public string? Nome { get; set; }
    public string? Fantasia { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Municipio { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Situacao { get; set; }
    public List<AtividadeReceitaWs>? AtividadePrincipal { get; set; }
}

internal class AtividadeReceitaWs
{
    public string? Code { get; set; }
    public string? Text { get; set; }
}
