using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.Models;
using SistemaEmpresas.Services;

namespace SistemaEmpresas.Controllers;

/// <summary>
/// Controller para Cadastro Geral (Clientes, Fornecedores, Transportadoras, Vendedores)
/// Sistema unificado igual ao VB6 - Uma tabela para todas as entidades
/// </summary>
[ApiController]
[Route("api/geral")]
[Authorize]
public class GeralController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<GeralController> _logger;
    private readonly ICacheService _cache;

    public GeralController(AppDbContext context, ILogger<GeralController> logger, ICacheService cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    #region DTOs

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

    public class GeralCreateDto
    {
        // Tipos (flags)
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

        // Endereço
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
    }

    public class GeralUpdateDto : GeralCreateDto
    {
        public int SequenciaDoGeral { get; set; }
    }

    public class GeralDetailDto : GeralUpdateDto
    {
        public DateTime? DataDoCadastro { get; set; }
        public string UsuDaAlteracao { get; set; } = "";
        public string MunicipioNome { get; set; } = "";
        public string MunicipioUf { get; set; } = "";
        public string MunicipioCobrancaNome { get; set; } = "";
        public string MunicipioCobrancaUf { get; set; } = "";
        public string VendedorNome { get; set; } = "";
    }

    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    #endregion

    #region Endpoints de Consulta

    /// <summary>
    /// Lista registros com paginação e filtros
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<GeralListDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? busca = null,
        [FromQuery] bool? cliente = null,
        [FromQuery] bool? fornecedor = null,
        [FromQuery] bool? transportadora = null,
        [FromQuery] bool? vendedor = null,
        [FromQuery] bool incluirInativos = false)
    {
        _logger.LogInformation("GET /api/geral - Busca: {Busca}, Cliente: {Cliente}, Fornecedor: {Fornecedor}", 
            busca, cliente, fornecedor);

        var query = _context.Gerals.AsQueryable();

        // Filtros por tipo
        if (cliente == true)
            query = query.Where(g => g.Cliente);
        if (fornecedor == true)
            query = query.Where(g => g.Fornecedor);
        if (transportadora == true)
            query = query.Where(g => g.Transportadora);
        if (vendedor == true)
            query = query.Where(g => g.Vendedor);

        // Filtro de inativos
        if (!incluirInativos)
            query = query.Where(g => !g.Inativo);

        // Busca por texto
        if (!string.IsNullOrWhiteSpace(busca))
        {
            var buscaLower = busca.ToLower();
            query = query.Where(g =>
                g.RazaoSocial.ToLower().Contains(buscaLower) ||
                g.NomeFantasia.ToLower().Contains(buscaLower) ||
                g.CpfECnpj.Contains(busca) ||
                g.Email.ToLower().Contains(buscaLower));
        }

        // Total antes de paginar
        var total = await query.CountAsync();

        // Paginação - ordenado por código (menor para maior)
        var items = await query
            .OrderBy(g => g.SequenciaDoGeral)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .Select(g => new GeralListDto
            {
                SequenciaDoGeral = g.SequenciaDoGeral,
                RazaoSocial = g.RazaoSocial,
                NomeFantasia = g.NomeFantasia,
                CpfECnpj = g.CpfECnpj,
                Cidade = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Descricao : "",
                Uf = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Uf : "",
                Fone1 = g.Fone1 ?? "",
                Email = g.Email,
                Cliente = g.Cliente,
                Fornecedor = g.Fornecedor,
                Transportadora = g.Transportadora,
                Vendedor = g.Vendedor,
                Inativo = g.Inativo,
                Tipo = g.Tipo
            })
            .ToListAsync();

        return Ok(new PagedResultDto<GeralListDto>
        {
            Items = items,
            Total = total,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    /// <summary>
    /// Busca por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GeralDetailDto>> GetById(int id)
    {
        _logger.LogInformation("GET /api/geral/{Id}", id);

        var geral = await _context.Gerals
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .Include(g => g.SequenciaMunicipioCobrancaNavigation)
            .Include(g => g.SequenciaDoVendedorNavigation)
            .FirstOrDefaultAsync(g => g.SequenciaDoGeral == id);

        if (geral == null)
        {
            return NotFound(new { mensagem = $"Registro {id} não encontrado" });
        }

        var dto = MapToDetailDto(geral);
        return Ok(dto);
    }

    /// <summary>
    /// Busca simplificada para autocomplete/combobox
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GeralListDto>>> Buscar(
        [FromQuery] string termo,
        [FromQuery] string? tipo = null, // cliente, fornecedor, transportadora, vendedor
        [FromQuery] int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(termo) || termo.Length < 2)
        {
            return Ok(new List<GeralListDto>());
        }

        var query = _context.Gerals
            .Where(g => !g.Inativo)
            .Where(g =>
                g.RazaoSocial.ToLower().Contains(termo.ToLower()) ||
                g.NomeFantasia.ToLower().Contains(termo.ToLower()) ||
                g.CpfECnpj.Contains(termo));

        // Filtro por tipo
        query = tipo?.ToLower() switch
        {
            "cliente" => query.Where(g => g.Cliente),
            "fornecedor" => query.Where(g => g.Fornecedor),
            "transportadora" => query.Where(g => g.Transportadora),
            "vendedor" => query.Where(g => g.Vendedor),
            _ => query
        };

        var items = await query
            .OrderBy(g => g.RazaoSocial)
            .Take(limit)
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .Select(g => new GeralListDto
            {
                SequenciaDoGeral = g.SequenciaDoGeral,
                RazaoSocial = g.RazaoSocial,
                NomeFantasia = g.NomeFantasia,
                CpfECnpj = g.CpfECnpj,
                Cidade = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Descricao : "",
                Uf = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Uf : "",
                Fone1 = g.Fone1 ?? "",
                Email = g.Email,
                Cliente = g.Cliente,
                Fornecedor = g.Fornecedor,
                Transportadora = g.Transportadora,
                Vendedor = g.Vendedor,
                Inativo = g.Inativo,
                Tipo = g.Tipo
            })
            .ToListAsync();

        return Ok(items);
    }

    /// <summary>
    /// Lista vendedores para combobox
    /// </summary>
    [HttpGet("vendedores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GeralListDto>>> GetVendedores()
    {
        var vendedores = await _cache.GetOrCreateAsync(
            "geral:vendedores:all",
            async () => await _context.Gerals
                .Where(g => g.Vendedor && !g.Inativo)
                .OrderBy(g => g.RazaoSocial)
                .Select(g => new GeralListDto
                {
                    SequenciaDoGeral = g.SequenciaDoGeral,
                    RazaoSocial = g.RazaoSocial,
                    NomeFantasia = g.NomeFantasia,
                    CpfECnpj = g.CpfECnpj,
                    Fone1 = g.Fone1 ?? "",
                    Email = g.Email,
                    Vendedor = true,
                    Tipo = g.Tipo
                })
                .ToListAsync(),
            CacheService.CacheDurations.Medium,
            CacheService.CacheDurations.Short);

        return Ok(vendedores);
    }

    #endregion

    #region Endpoints de Modificação

    /// <summary>
    /// Cria novo registro
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GeralDetailDto>> Create([FromBody] GeralCreateDto dto)
    {
        _logger.LogInformation("POST /api/geral - RazaoSocial: {RazaoSocial}", dto.RazaoSocial);

        // Validações
        if (string.IsNullOrWhiteSpace(dto.RazaoSocial))
        {
            return BadRequest(new { mensagem = "Razão Social é obrigatória" });
        }

        // Pelo menos um tipo deve estar marcado
        if (!dto.Cliente && !dto.Fornecedor && !dto.Transportadora && !dto.Vendedor && !dto.Despesa && !dto.Imposto)
        {
            return BadRequest(new { mensagem = "Selecione pelo menos um tipo (Cliente, Fornecedor, etc.)" });
        }

        var geral = new Geral
        {
            // Tipos
            Cliente = dto.Cliente,
            Fornecedor = dto.Fornecedor,
            Despesa = dto.Despesa,
            Imposto = dto.Imposto,
            Transportadora = dto.Transportadora,
            Vendedor = dto.Vendedor,

            // Dados Principais
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia ?? "",
            Tipo = dto.Tipo,
            CpfECnpj = dto.CpfECnpj ?? "",
            RgEIe = dto.RgEIe ?? "",
            CodigoDoSuframa = dto.CodigoDoSuframa ?? "",
            CodigoDaAntt = dto.CodigoDaAntt ?? "",

            // Endereço
            Endereco = dto.Endereco ?? "",
            NumeroDoEndereco = dto.NumeroDoEndereco ?? "",
            Complemento = dto.Complemento ?? "",
            Bairro = dto.Bairro ?? "",
            CaixaPostal = dto.CaixaPostal ?? "",
            SequenciaDoMunicipio = dto.SequenciaDoMunicipio > 0 ? dto.SequenciaDoMunicipio : 1,
            Cep = dto.Cep ?? "",
            SequenciaDoPais = dto.SequenciaDoPais > 0 ? dto.SequenciaDoPais : 1,

            // Contato
            Fone1 = dto.Fone1,
            Fone2 = dto.Fone2,
            Fax = dto.Fax,
            Celular = dto.Celular,
            Contato = dto.Contato ?? "",
            Email = dto.Email ?? "",
            HomePage = dto.HomePage ?? "",

            // Observações
            Observacao = dto.Observacao ?? "",

            // Flags fiscais
            Revenda = dto.Revenda,
            Isento = dto.Isento,
            OrgonPublico = dto.OrgonPublico,
            EmpresaProdutor = dto.EmpresaProdutor,
            Cumulativo = dto.Cumulativo,
            Inativo = dto.Inativo,

            // Vendedor
            SequenciaDoVendedor = dto.SequenciaDoVendedor,
            IntermediarioDoVendedor = dto.IntermediarioDoVendedor ?? "",

            // Cobrança
            EnderecoDeCobranca = dto.EnderecoDeCobranca ?? "",
            NumeroDoEnderecoDeCobranca = dto.NumeroDoEnderecoDeCobranca ?? "",
            ComplementoDaCobranca = dto.ComplementoDaCobranca ?? "",
            BairroDeCobranca = dto.BairroDeCobranca ?? "",
            CaixaPostalDaCobranca = dto.CaixaPostalDaCobranca ?? "",
            SequenciaMunicipioCobranca = dto.SequenciaMunicipioCobranca > 0 ? dto.SequenciaMunicipioCobranca : 1,
            CepDeCobranca = dto.CepDeCobranca ?? "",

            // Bancário
            NomeDoBanco1 = dto.NomeDoBanco1 ?? "",
            AgenciaDoBanco1 = dto.AgenciaDoBanco1 ?? "",
            ContaCorrenteDoBanco1 = dto.ContaCorrenteDoBanco1 ?? "",
            NomeDoCorrentistaDoBanco1 = dto.NomeDoCorrentistaDoBanco1 ?? "",
            NomeDoBanco2 = dto.NomeDoBanco2 ?? "",
            AgenciaDoBanco2 = dto.AgenciaDoBanco2 ?? "",
            ContaCorrenteDoBanco2 = dto.ContaCorrenteDoBanco2 ?? "",
            NomeDoCorrentistaDoBanco2 = dto.NomeDoCorrentistaDoBanco2 ?? "",

            // Adicionais
            DataDeNascimento = dto.DataDeNascimento,
            CodigoContabil = dto.CodigoContabil,
            CodigoAdiantamento = dto.CodigoAdiantamento,
            SalBruto = dto.SalBruto,

            // Auditoria
            DataDoCadastro = DateTime.Now,
            UsuDaAlteracao = User.Identity?.Name ?? "Sistema",
            ImportouNoZap = false
        };

        _context.Gerals.Add(geral);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Registro criado: {Id} - {RazaoSocial}", geral.SequenciaDoGeral, geral.RazaoSocial);

        var result = MapToDetailDto(geral);
        return CreatedAtAction(nameof(GetById), new { id = geral.SequenciaDoGeral }, result);
    }

    /// <summary>
    /// Atualiza registro existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GeralDetailDto>> Update(int id, [FromBody] GeralUpdateDto dto)
    {
        _logger.LogInformation("PUT /api/geral/{Id}", id);

        if (id != dto.SequenciaDoGeral)
        {
            return BadRequest(new { mensagem = "ID do registro não confere" });
        }

        var geral = await _context.Gerals.FindAsync(id);
        if (geral == null)
        {
            return NotFound(new { mensagem = $"Registro {id} não encontrado" });
        }

        // Validações
        if (string.IsNullOrWhiteSpace(dto.RazaoSocial))
        {
            return BadRequest(new { mensagem = "Razão Social é obrigatória" });
        }

        // Atualiza campos
        geral.Cliente = dto.Cliente;
        geral.Fornecedor = dto.Fornecedor;
        geral.Despesa = dto.Despesa;
        geral.Imposto = dto.Imposto;
        geral.Transportadora = dto.Transportadora;
        geral.Vendedor = dto.Vendedor;
        geral.RazaoSocial = dto.RazaoSocial;
        geral.NomeFantasia = dto.NomeFantasia ?? "";
        geral.Tipo = dto.Tipo;
        geral.CpfECnpj = dto.CpfECnpj ?? "";
        geral.RgEIe = dto.RgEIe ?? "";
        geral.CodigoDoSuframa = dto.CodigoDoSuframa ?? "";
        geral.CodigoDaAntt = dto.CodigoDaAntt ?? "";
        geral.Endereco = dto.Endereco ?? "";
        geral.NumeroDoEndereco = dto.NumeroDoEndereco ?? "";
        geral.Complemento = dto.Complemento ?? "";
        geral.Bairro = dto.Bairro ?? "";
        geral.CaixaPostal = dto.CaixaPostal ?? "";
        geral.SequenciaDoMunicipio = dto.SequenciaDoMunicipio > 0 ? dto.SequenciaDoMunicipio : geral.SequenciaDoMunicipio;
        geral.Cep = dto.Cep ?? "";
        geral.SequenciaDoPais = dto.SequenciaDoPais > 0 ? dto.SequenciaDoPais : geral.SequenciaDoPais;
        geral.Fone1 = dto.Fone1;
        geral.Fone2 = dto.Fone2;
        geral.Fax = dto.Fax;
        geral.Celular = dto.Celular;
        geral.Contato = dto.Contato ?? "";
        geral.Email = dto.Email ?? "";
        geral.HomePage = dto.HomePage ?? "";
        geral.Observacao = dto.Observacao ?? "";
        geral.Revenda = dto.Revenda;
        geral.Isento = dto.Isento;
        geral.OrgonPublico = dto.OrgonPublico;
        geral.EmpresaProdutor = dto.EmpresaProdutor;
        geral.Cumulativo = dto.Cumulativo;
        geral.Inativo = dto.Inativo;
        geral.SequenciaDoVendedor = dto.SequenciaDoVendedor;
        geral.IntermediarioDoVendedor = dto.IntermediarioDoVendedor ?? "";
        geral.EnderecoDeCobranca = dto.EnderecoDeCobranca ?? "";
        geral.NumeroDoEnderecoDeCobranca = dto.NumeroDoEnderecoDeCobranca ?? "";
        geral.ComplementoDaCobranca = dto.ComplementoDaCobranca ?? "";
        geral.BairroDeCobranca = dto.BairroDeCobranca ?? "";
        geral.CaixaPostalDaCobranca = dto.CaixaPostalDaCobranca ?? "";
        geral.SequenciaMunicipioCobranca = dto.SequenciaMunicipioCobranca > 0 ? dto.SequenciaMunicipioCobranca : geral.SequenciaMunicipioCobranca;
        geral.CepDeCobranca = dto.CepDeCobranca ?? "";
        geral.NomeDoBanco1 = dto.NomeDoBanco1 ?? "";
        geral.AgenciaDoBanco1 = dto.AgenciaDoBanco1 ?? "";
        geral.ContaCorrenteDoBanco1 = dto.ContaCorrenteDoBanco1 ?? "";
        geral.NomeDoCorrentistaDoBanco1 = dto.NomeDoCorrentistaDoBanco1 ?? "";
        geral.NomeDoBanco2 = dto.NomeDoBanco2 ?? "";
        geral.AgenciaDoBanco2 = dto.AgenciaDoBanco2 ?? "";
        geral.ContaCorrenteDoBanco2 = dto.ContaCorrenteDoBanco2 ?? "";
        geral.NomeDoCorrentistaDoBanco2 = dto.NomeDoCorrentistaDoBanco2 ?? "";
        geral.DataDeNascimento = dto.DataDeNascimento;
        geral.CodigoContabil = dto.CodigoContabil;
        geral.CodigoAdiantamento = dto.CodigoAdiantamento;
        geral.SalBruto = dto.SalBruto;
        geral.UsuDaAlteracao = User.Identity?.Name ?? "Sistema";

        await _context.SaveChangesAsync();

        _logger.LogInformation("Registro atualizado: {Id}", id);

        // Recarrega com includes
        await _context.Entry(geral)
            .Reference(g => g.SequenciaDoMunicipioNavigation).LoadAsync();
        await _context.Entry(geral)
            .Reference(g => g.SequenciaMunicipioCobrancaNavigation).LoadAsync();

        var result = MapToDetailDto(geral);
        return Ok(result);
    }

    /// <summary>
    /// Exclui (inativa) registro
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE /api/geral/{Id}", id);

        var geral = await _context.Gerals.FindAsync(id);
        if (geral == null)
        {
            return NotFound(new { mensagem = $"Registro {id} não encontrado" });
        }

        // Soft delete - apenas inativa
        geral.Inativo = true;
        geral.UsuDaAlteracao = User.Identity?.Name ?? "Sistema";

        await _context.SaveChangesAsync();

        _logger.LogInformation("Registro inativado: {Id}", id);

        return Ok(new { mensagem = "Registro inativado com sucesso" });
    }

    #endregion

    #region Métodos Auxiliares

    private GeralDetailDto MapToDetailDto(Geral geral)
    {
        return new GeralDetailDto
        {
            SequenciaDoGeral = geral.SequenciaDoGeral,
            Cliente = geral.Cliente,
            Fornecedor = geral.Fornecedor,
            Despesa = geral.Despesa,
            Imposto = geral.Imposto,
            Transportadora = geral.Transportadora,
            Vendedor = geral.Vendedor,
            RazaoSocial = geral.RazaoSocial,
            NomeFantasia = geral.NomeFantasia,
            Tipo = geral.Tipo,
            CpfECnpj = geral.CpfECnpj,
            RgEIe = geral.RgEIe,
            CodigoDoSuframa = geral.CodigoDoSuframa,
            CodigoDaAntt = geral.CodigoDaAntt,
            Endereco = geral.Endereco,
            NumeroDoEndereco = geral.NumeroDoEndereco,
            Complemento = geral.Complemento,
            Bairro = geral.Bairro,
            CaixaPostal = geral.CaixaPostal,
            SequenciaDoMunicipio = geral.SequenciaDoMunicipio,
            Cep = geral.Cep,
            SequenciaDoPais = geral.SequenciaDoPais,
            Fone1 = geral.Fone1 ?? "",
            Fone2 = geral.Fone2 ?? "",
            Fax = geral.Fax ?? "",
            Celular = geral.Celular ?? "",
            Contato = geral.Contato,
            Email = geral.Email,
            HomePage = geral.HomePage,
            Observacao = geral.Observacao,
            Revenda = geral.Revenda,
            Isento = geral.Isento,
            OrgonPublico = geral.OrgonPublico,
            EmpresaProdutor = geral.EmpresaProdutor,
            Cumulativo = geral.Cumulativo,
            Inativo = geral.Inativo,
            SequenciaDoVendedor = geral.SequenciaDoVendedor,
            IntermediarioDoVendedor = geral.IntermediarioDoVendedor,
            EnderecoDeCobranca = geral.EnderecoDeCobranca,
            NumeroDoEnderecoDeCobranca = geral.NumeroDoEnderecoDeCobranca,
            ComplementoDaCobranca = geral.ComplementoDaCobranca,
            BairroDeCobranca = geral.BairroDeCobranca,
            CaixaPostalDaCobranca = geral.CaixaPostalDaCobranca,
            SequenciaMunicipioCobranca = geral.SequenciaMunicipioCobranca,
            CepDeCobranca = geral.CepDeCobranca,
            NomeDoBanco1 = geral.NomeDoBanco1,
            AgenciaDoBanco1 = geral.AgenciaDoBanco1,
            ContaCorrenteDoBanco1 = geral.ContaCorrenteDoBanco1,
            NomeDoCorrentistaDoBanco1 = geral.NomeDoCorrentistaDoBanco1,
            NomeDoBanco2 = geral.NomeDoBanco2,
            AgenciaDoBanco2 = geral.AgenciaDoBanco2,
            ContaCorrenteDoBanco2 = geral.ContaCorrenteDoBanco2,
            NomeDoCorrentistaDoBanco2 = geral.NomeDoCorrentistaDoBanco2,
            DataDeNascimento = geral.DataDeNascimento,
            CodigoContabil = geral.CodigoContabil,
            CodigoAdiantamento = geral.CodigoAdiantamento,
            SalBruto = geral.SalBruto,
            DataDoCadastro = geral.DataDoCadastro,
            UsuDaAlteracao = geral.UsuDaAlteracao,
            MunicipioNome = geral.SequenciaDoMunicipioNavigation?.Descricao ?? "",
            MunicipioUf = geral.SequenciaDoMunicipioNavigation?.Uf ?? "",
            MunicipioCobrancaNome = geral.SequenciaMunicipioCobrancaNavigation?.Descricao ?? "",
            MunicipioCobrancaUf = geral.SequenciaMunicipioCobrancaNavigation?.Uf ?? "",
            VendedorNome = geral.SequenciaDoVendedorNavigation?.RazaoSocial ?? ""
        };
    }

    #endregion

    #region APIs Externas (CNPJ/CEP)

    /// <summary>
    /// Busca dados de uma empresa pelo CNPJ usando a API Brasil API
    /// </summary>
    [HttpGet("consulta-cnpj/{cnpj}")]
    public async Task<IActionResult> ConsultarCnpj(string cnpj)
    {
        try
        {
            // Remove caracteres não numéricos
            var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());
            
            if (cnpjLimpo.Length != 14)
            {
                return BadRequest(new { mensagem = "CNPJ inválido. Deve conter 14 dígitos." });
            }

            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            // Tenta Brasil API primeiro (mais estável)
            var response = await httpClient.GetAsync($"https://brasilapi.com.br/api/cnpj/v1/{cnpjLimpo}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var dados = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
                
                // Busca o município pelo nome para pegar o código
                var municipio = dados.TryGetProperty("municipio", out var mun) ? mun.GetString() : "";
                var uf = dados.TryGetProperty("uf", out var ufProp) ? ufProp.GetString() : "";
                
                var sequenciaMunicipio = await _context.Municipios
                    .Where(m => m.Descricao.ToLower() == municipio!.ToLower() && m.Uf == uf)
                    .Select(m => m.SequenciaDoMunicipio)
                    .FirstOrDefaultAsync();
                
                return Ok(new CnpjResponseDto
                {
                    RazaoSocial = dados.TryGetProperty("razao_social", out var rs) ? rs.GetString() ?? "" : "",
                    NomeFantasia = dados.TryGetProperty("nome_fantasia", out var nf) ? nf.GetString() ?? "" : "",
                    Logradouro = dados.TryGetProperty("logradouro", out var log) ? log.GetString() ?? "" : "",
                    Numero = dados.TryGetProperty("numero", out var num) ? num.GetString() ?? "" : "",
                    Complemento = dados.TryGetProperty("complemento", out var comp) ? comp.GetString() ?? "" : "",
                    Bairro = dados.TryGetProperty("bairro", out var bairro) ? bairro.GetString() ?? "" : "",
                    Municipio = municipio ?? "",
                    Uf = uf ?? "",
                    Cep = dados.TryGetProperty("cep", out var cep) ? cep.GetString() ?? "" : "",
                    Telefone = dados.TryGetProperty("ddd_telefone_1", out var tel) ? tel.GetString() ?? "" : "",
                    Email = dados.TryGetProperty("email", out var email) ? email.GetString() ?? "" : "",
                    SequenciaDoMunicipio = sequenciaMunicipio
                });
            }
            
            // Se Brasil API falhar, retorna erro
            return BadRequest(new { mensagem = "CNPJ não encontrado ou serviço indisponível" });
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, new { mensagem = "Serviço de consulta CNPJ temporariamente indisponível" });
        }
        catch (TaskCanceledException)
        {
            return StatusCode(504, new { mensagem = "Tempo limite excedido ao consultar CNPJ" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar CNPJ {Cnpj}", cnpj);
            return StatusCode(500, new { mensagem = "Erro interno ao consultar CNPJ" });
        }
    }

    /// <summary>
    /// Busca endereço pelo CEP usando a API ViaCEP
    /// </summary>
    [HttpGet("consulta-cep/{cep}")]
    public async Task<IActionResult> ConsultarCep(string cep)
    {
        try
        {
            // Remove caracteres não numéricos
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            
            if (cepLimpo.Length != 8)
            {
                return BadRequest(new { mensagem = "CEP inválido. Deve conter 8 dígitos." });
            }

            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(15);
            
            var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json/");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                
                // Verifica se retornou erro
                if (json.Contains("\"erro\""))
                {
                    return BadRequest(new { mensagem = "CEP não encontrado" });
                }
                
                var dados = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
                
                // Busca o município pelo nome para pegar o código
                var municipio = dados.TryGetProperty("localidade", out var loc) ? loc.GetString() : "";
                var uf = dados.TryGetProperty("uf", out var ufProp) ? ufProp.GetString() : "";
                
                var sequenciaMunicipio = await _context.Municipios
                    .Where(m => m.Descricao.ToLower() == municipio!.ToLower() && m.Uf == uf)
                    .Select(m => m.SequenciaDoMunicipio)
                    .FirstOrDefaultAsync();
                
                return Ok(new CepResponseDto
                {
                    Logradouro = dados.TryGetProperty("logradouro", out var log) ? log.GetString() ?? "" : "",
                    Complemento = dados.TryGetProperty("complemento", out var comp) ? comp.GetString() ?? "" : "",
                    Bairro = dados.TryGetProperty("bairro", out var bairro) ? bairro.GetString() ?? "" : "",
                    Municipio = municipio ?? "",
                    Uf = uf ?? "",
                    Cep = cepLimpo,
                    SequenciaDoMunicipio = sequenciaMunicipio
                });
            }
            
            return BadRequest(new { mensagem = "CEP não encontrado" });
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, new { mensagem = "Serviço de consulta CEP temporariamente indisponível" });
        }
        catch (TaskCanceledException)
        {
            return StatusCode(504, new { mensagem = "Tempo limite excedido ao consultar CEP" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar CEP {Cep}", cep);
            return StatusCode(500, new { mensagem = "Erro interno ao consultar CEP" });
        }
    }

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
        public int SequenciaDoMunicipio { get; set; }
    }

    public class CepResponseDto
    {
        public string Logradouro { get; set; } = "";
        public string Complemento { get; set; } = "";
        public string Bairro { get; set; } = "";
        public string Municipio { get; set; } = "";
        public string Uf { get; set; } = "";
        public string Cep { get; set; } = "";
        public int SequenciaDoMunicipio { get; set; }
    }

    #endregion
}
