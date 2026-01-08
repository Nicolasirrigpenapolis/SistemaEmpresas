using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.Core.Dtos;
using SistemaEmpresas.Features.Geral.Dtos;
using GeralModel = SistemaEmpresas.Models.Geral;

namespace SistemaEmpresas.Features.Geral.Repositories;

/// <summary>
/// Interface do repositório de Geral (Clientes, Fornecedores, Transportadoras, Vendedores)
/// </summary>
public interface IGeralRepository
{
    Task<PagedResult<GeralListDto>> ListarAsync(GeralFiltroDto filtro);
    Task<GeralDetailDto?> ObterPorIdAsync(int id);
    Task<List<GeralListDto>> BuscarAsync(string termo, string? tipo = null, int limit = 20);
    Task<List<GeralListDto>> ListarVendedoresAsync();
    Task<GeralModel> CriarAsync(GeralModel geral);
    Task<GeralModel?> AtualizarAsync(GeralModel geral);
    Task<bool> InativarAsync(int id, string usuario);
    Task<bool> ExisteCpfCnpjAsync(string cpfCnpj, int? idExcluir = null);
    Task<int?> ObterMunicipioPorNomeUfAsync(string municipio, string uf);
    Task<string?> ObterUfPorMunicipioIdAsync(int sequenciaMunicipio);
}

/// <summary>
/// Repositório de Geral - Acesso a dados
/// </summary>
public class GeralRepository : IGeralRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<GeralRepository> _logger;

    public GeralRepository(AppDbContext context, ILogger<GeralRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<GeralListDto>> ListarAsync(GeralFiltroDto filtro)
    {
        _logger.LogDebug("Listando geral. Página: {Page}, Busca: {Busca}", filtro.PageNumber, filtro.Busca);

        var query = _context.Gerals
            .AsNoTracking()
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .AsQueryable();

        // Filtros por tipo
        if (filtro.Cliente == true)
            query = query.Where(g => g.Cliente);
        if (filtro.Fornecedor == true)
            query = query.Where(g => g.Fornecedor);
        if (filtro.Transportadora == true)
            query = query.Where(g => g.Transportadora);
        if (filtro.Vendedor == true)
            query = query.Where(g => g.Vendedor);

        // Filtro de inativos
        if (!filtro.IncluirInativos)
            query = query.Where(g => !g.Inativo);

        // Busca por texto
        if (!string.IsNullOrWhiteSpace(filtro.Busca))
        {
            var buscaLower = filtro.Busca.ToLower();
            query = query.Where(g =>
                g.RazaoSocial.ToLower().Contains(buscaLower) ||
                g.NomeFantasia.ToLower().Contains(buscaLower) ||
                g.CpfECnpj.Contains(filtro.Busca) ||
                g.Email.ToLower().Contains(buscaLower));
        }

        // Total antes de paginar
        var total = await query.CountAsync();

        // Paginação
        var items = await query
            .OrderBy(g => g.SequenciaDoGeral)
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(ProjectToListDto)
            .ToListAsync();

        return new PagedResult<GeralListDto>
        {
            Items = items,
            TotalCount = total,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize
        };
    }

    public async Task<GeralDetailDto?> ObterPorIdAsync(int id)
    {
        var geral = await _context.Gerals
            .AsNoTracking()
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .Include(g => g.SequenciaMunicipioCobrancaNavigation)
            .Include(g => g.SequenciaDoVendedorNavigation)
            .Include(g => g.SequenciaDoPaisNavigation)
            .FirstOrDefaultAsync(g => g.SequenciaDoGeral == id);

        if (geral == null)
            return null;

        return MapToDetailDto(geral);
    }

    public async Task<List<GeralListDto>> BuscarAsync(string termo, string? tipo = null, int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(termo) || termo.Length < 2)
            return new List<GeralListDto>();

        var query = _context.Gerals
            .AsNoTracking()
            .Include(g => g.SequenciaDoMunicipioNavigation)
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

        return await query
            .OrderBy(g => g.RazaoSocial)
            .Take(limit)
            .Select(ProjectToListDto)
            .ToListAsync();
    }

    public async Task<List<GeralListDto>> ListarVendedoresAsync()
    {
        return await _context.Gerals
            .AsNoTracking()
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .Where(g => g.Vendedor && !g.Inativo)
            .OrderBy(g => g.RazaoSocial)
            .Select(ProjectToListDto)
            .ToListAsync();
    }

    public async Task<GeralModel> CriarAsync(GeralModel geral)
    {
        _context.Gerals.Add(geral);
        await _context.SaveChangesAsync();
        return geral;
    }

    public async Task<GeralModel?> AtualizarAsync(GeralModel geral)
    {
        var existente = await _context.Gerals.FindAsync(geral.SequenciaDoGeral);
        if (existente == null)
            return null;

        // Copia valores da entidade recebida para a entidade rastreada
        _context.Entry(existente).CurrentValues.SetValues(geral);
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> InativarAsync(int id, string usuario)
    {
        var geral = await _context.Gerals.FindAsync(id);
        if (geral == null)
            return false;

        geral.Inativo = true;
        geral.UsuDaAlteracao = usuario;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExisteCpfCnpjAsync(string cpfCnpj, int? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            return false;

        var query = _context.Gerals.Where(g => g.CpfECnpj == cpfCnpj);
        
        if (idExcluir.HasValue)
            query = query.Where(g => g.SequenciaDoGeral != idExcluir.Value);

        return await query.AnyAsync();
    }

    public async Task<int?> ObterMunicipioPorNomeUfAsync(string municipio, string uf)
    {
        if (string.IsNullOrWhiteSpace(municipio) || string.IsNullOrWhiteSpace(uf))
            return null;

        return await _context.Municipios
            .Where(m => m.Descricao.ToLower() == municipio.ToLower() && m.Uf == uf)
            .Select(m => (int?)m.SequenciaDoMunicipio)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> ObterUfPorMunicipioIdAsync(int sequenciaMunicipio)
    {
        if (sequenciaMunicipio <= 0)
            return null;

        return await _context.Municipios
            .Where(m => m.SequenciaDoMunicipio == sequenciaMunicipio)
            .Select(m => m.Uf)
            .FirstOrDefaultAsync();
    }

    #region Mapeamento

    /// <summary>
    /// Projeção para GeralListDto - usado em listagens e buscas
    /// </summary>
    private static readonly Expression<Func<GeralModel, GeralListDto>> ProjectToListDto = g => new GeralListDto
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
    };

    private static GeralDetailDto MapToDetailDto(GeralModel geral)
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
            WhatsAppSincronizado = geral.WhatsAppSincronizado,
            DataDoCadastro = geral.DataDoCadastro,
            UsuDaAlteracao = geral.UsuDaAlteracao ?? "",
            MunicipioNome = geral.SequenciaDoMunicipioNavigation?.Descricao ?? "",
            MunicipioUf = geral.SequenciaDoMunicipioNavigation?.Uf ?? "",
            MunicipioCobrancaNome = geral.SequenciaMunicipioCobrancaNavigation?.Descricao ?? "",
            MunicipioCobrancaUf = geral.SequenciaMunicipioCobrancaNavigation?.Uf ?? "",
            VendedorNome = geral.SequenciaDoVendedorNavigation?.RazaoSocial ?? "",
            PaisNome = geral.SequenciaDoPaisNavigation?.Descricao ?? ""
        };
    }

    #endregion
}
