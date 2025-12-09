using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories;

/// <summary>
/// Interface do Repositório ClassTrib
/// </summary>
public interface IClassTribRepository
{
    Task<ClassTrib?> GetByIdAsync(int id);
    Task<ClassTrib?> GetByCodigoAsync(string codigoClassTrib);
    Task<List<ClassTrib>> GetAllAsync(bool apenasAtivos = true);
    Task<List<ClassTrib>> GetByCstAsync(string cst);
    Task<List<ClassTrib>> GetValidosNFeAsync();
    Task<List<ClassTrib>> SearchAsync(string termo, int limite = 50);
    Task<(List<ClassTrib> items, int total)> GetPagedAsync(
        int pageNumber = 1, 
        int pageSize = 50, 
        string? filtroCst = null,
        string? filtroDescricao = null,
        bool? somenteNFe = null);
    
    // Novos métodos para filtros avançados
    Task<(List<ClassTrib> items, int total)> GetPagedAdvancedAsync(
        int pageNumber = 1,
        int pageSize = 50,
        List<string>? csts = null,
        string? tipoAliquota = null,
        decimal? minReducaoIBS = null,
        decimal? maxReducaoIBS = null,
        decimal? minReducaoCBS = null,
        decimal? maxReducaoCBS = null,
        bool? validoNFe = null,
        bool? tributacaoRegular = null,
        bool? creditoPresumido = null,
        string? descricao = null,
        string? ordenarPor = null);
    
    Task<List<string>> GetTiposAliquotaAsync();
    Task<List<Controllers.CstOption>> GetCstsAsync();
    Task<Controllers.ClassTribEstatisticas> GetEstatisticasAsync();
    
    Task<ClassTrib> CreateAsync(ClassTrib classT);
    Task<ClassTrib> UpdateAsync(ClassTrib classTrib);
    Task<ClassTrib> UpsertAsync(ClassTrib classTrib);
    Task<int> BulkUpsertAsync(List<ClassTrib> classTribs);
    Task<bool> ExisteAsync(string codigoClassTrib);
}

/// <summary>
/// Repositório para ClassTrib (Classificações Tributárias SVRS)
/// </summary>
public class ClassTribRepository : IClassTribRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ClassTribRepository> _logger;

    public ClassTribRepository(AppDbContext context, ILogger<ClassTribRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Consultas

    public async Task<ClassTrib?> GetByIdAsync(int id)
    {
        _logger.LogDebug("Buscando ClassTrib por ID: {Id}", id);
        return await _context.ClassTribs.FindAsync(id);
    }

    public async Task<ClassTrib?> GetByCodigoAsync(string codigoClassTrib)
    {
        if (string.IsNullOrWhiteSpace(codigoClassTrib))
            return null;

        _logger.LogDebug("Buscando ClassTrib por código: {Codigo}", codigoClassTrib);
        return await _context.ClassTribs
            .FirstOrDefaultAsync(c => c.CodigoClassTrib == codigoClassTrib);
    }

    public async Task<List<ClassTrib>> GetAllAsync(bool apenasAtivos = true)
    {
        _logger.LogDebug("Listando todos os ClassTribs. Apenas ativos: {ApenasAtivos}", apenasAtivos);

        var query = _context.ClassTribs.AsQueryable();

        if (apenasAtivos)
            query = query.Where(c => c.Ativo);

        return await query
            .OrderBy(c => c.CodigoSituacaoTributaria)
            .ThenBy(c => c.CodigoClassTrib)
            .ToListAsync();
    }

    public async Task<List<ClassTrib>> GetByCstAsync(string cst)
    {
        if (string.IsNullOrWhiteSpace(cst))
            return new List<ClassTrib>();

        _logger.LogDebug("Buscando ClassTribs por CST: {CST}", cst);

        return await _context.ClassTribs
            .Where(c => c.CodigoSituacaoTributaria == cst && c.Ativo)
            .OrderBy(c => c.CodigoClassTrib)
            .ToListAsync();
    }

    public async Task<List<ClassTrib>> GetValidosNFeAsync()
    {
        _logger.LogDebug("Listando ClassTribs válidos para NFe");

        return await _context.ClassTribs
            .Where(c => c.ValidoParaNFe && c.Ativo)
            .OrderBy(c => c.CodigoSituacaoTributaria)
            .ThenBy(c => c.CodigoClassTrib)
            .ToListAsync();
    }

    public async Task<List<ClassTrib>> SearchAsync(string termo, int limite = 50)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return new List<ClassTrib>();

        _logger.LogDebug("Pesquisando ClassTribs por termo: {Termo}", termo);

        return await _context.ClassTribs
            .Where(c => c.Ativo && (
                c.CodigoClassTrib.Contains(termo) ||
                c.CodigoSituacaoTributaria.Contains(termo) ||
                c.DescricaoClassTrib.Contains(termo) ||
                (c.DescricaoSituacaoTributaria != null && c.DescricaoSituacaoTributaria.Contains(termo))
            ))
            .OrderBy(c => c.CodigoClassTrib)
            .Take(limite)
            .ToListAsync();
    }

    public async Task<(List<ClassTrib> items, int total)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? filtroCst = null,
        string? filtroDescricao = null,
        bool? somenteNFe = null)
    {
        _logger.LogDebug("Listando ClassTribs paginados. Página: {Page}", pageNumber);

        var query = _context.ClassTribs.Where(c => c.Ativo);

        if (!string.IsNullOrWhiteSpace(filtroCst))
            query = query.Where(c => c.CodigoSituacaoTributaria == filtroCst);

        if (!string.IsNullOrWhiteSpace(filtroDescricao))
            query = query.Where(c => c.DescricaoClassTrib.Contains(filtroDescricao));

        if (somenteNFe == true)
            query = query.Where(c => c.ValidoParaNFe);

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.CodigoSituacaoTributaria)
            .ThenBy(c => c.CodigoClassTrib)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    #endregion

    #region CRUD

    public async Task<ClassTrib> CreateAsync(ClassTrib classTrib)
    {
        if (classTrib == null)
            throw new ArgumentNullException(nameof(classTrib));

        _logger.LogInformation("Criando ClassTrib: {Codigo}", classTrib.CodigoClassTrib);

        classTrib.DataSincronizacao = DateTime.Now;
        _context.ClassTribs.Add(classTrib);
        await _context.SaveChangesAsync();

        return classTrib;
    }

    public async Task<ClassTrib> UpdateAsync(ClassTrib classTrib)
    {
        if (classTrib == null)
            throw new ArgumentNullException(nameof(classTrib));

        _logger.LogInformation("Atualizando ClassTrib: {Codigo}", classTrib.CodigoClassTrib);

        classTrib.DataSincronizacao = DateTime.Now;
        _context.ClassTribs.Update(classTrib);
        await _context.SaveChangesAsync();

        return classTrib;
    }

    public async Task<ClassTrib> UpsertAsync(ClassTrib classTrib)
    {
        if (classTrib == null)
            throw new ArgumentNullException(nameof(classTrib));

        var existente = await GetByCodigoAsync(classTrib.CodigoClassTrib);

        if (existente != null)
        {
            // Atualizar existente
            existente.CodigoSituacaoTributaria = classTrib.CodigoSituacaoTributaria;
            existente.DescricaoSituacaoTributaria = classTrib.DescricaoSituacaoTributaria;
            existente.DescricaoClassTrib = classTrib.DescricaoClassTrib;
            existente.PercentualReducaoIBS = classTrib.PercentualReducaoIBS;
            existente.PercentualReducaoCBS = classTrib.PercentualReducaoCBS;
            existente.TipoAliquota = classTrib.TipoAliquota;
            existente.ValidoParaNFe = classTrib.ValidoParaNFe;
            existente.TributacaoRegular = classTrib.TributacaoRegular;
            existente.CreditoPresumidoOperacoes = classTrib.CreditoPresumidoOperacoes;
            existente.EstornoCredito = classTrib.EstornoCredito;
            existente.AnexoLegislacao = classTrib.AnexoLegislacao;
            existente.LinkLegislacao = classTrib.LinkLegislacao;
            existente.DataSincronizacao = DateTime.Now;
            existente.Ativo = true;

            return await UpdateAsync(existente);
        }
        else
        {
            // Inserir novo
            return await CreateAsync(classTrib);
        }
    }

    public async Task<int> BulkUpsertAsync(List<ClassTrib> classTribs)
    {
        if (classTribs == null || !classTribs.Any())
            return 0;

        _logger.LogInformation("Iniciando Bulk Upsert de {Total} ClassTribs", classTribs.Count);

        int processados = 0;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var classTrib in classTribs)
            {
                await UpsertAsync(classTrib);
                processados++;
            }

            await transaction.CommitAsync();
            _logger.LogInformation("Bulk Upsert concluído. Processados: {Total}", processados);

            return processados;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Erro no Bulk Upsert de ClassTribs");
            throw;
        }
    }

    public async Task<bool> ExisteAsync(string codigoClassTrib)
    {
        if (string.IsNullOrWhiteSpace(codigoClassTrib))
            return false;

        return await _context.ClassTribs
            .AnyAsync(c => c.CodigoClassTrib == codigoClassTrib);
    }

    #region Filtros Avançados

    public async Task<(List<ClassTrib> items, int total)> GetPagedAdvancedAsync(
        int pageNumber = 1,
        int pageSize = 50,
        List<string>? csts = null,
        string? tipoAliquota = null,
        decimal? minReducaoIBS = null,
        decimal? maxReducaoIBS = null,
        decimal? minReducaoCBS = null,
        decimal? maxReducaoCBS = null,
        bool? validoNFe = null,
        bool? tributacaoRegular = null,
        bool? creditoPresumido = null,
        string? descricao = null,
        string? ordenarPor = null)
    {
        _logger.LogDebug("Filtro avançado com múltiplos critérios. Página: {Page}", pageNumber);

        var query = _context.ClassTribs.Where(c => c.Ativo);

        // Filtro por CSTs (múltiplos)
        if (csts != null && csts.Any())
            query = query.Where(c => csts.Contains(c.CodigoSituacaoTributaria));

        // Filtro por tipo de alíquota
        if (!string.IsNullOrEmpty(tipoAliquota))
            query = query.Where(c => c.TipoAliquota == tipoAliquota);

        // Filtro por faixa de redução IBS
        if (minReducaoIBS.HasValue)
            query = query.Where(c => c.PercentualReducaoIBS >= minReducaoIBS);
        if (maxReducaoIBS.HasValue)
            query = query.Where(c => c.PercentualReducaoIBS <= maxReducaoIBS);

        // Filtro por faixa de redução CBS
        if (minReducaoCBS.HasValue)
            query = query.Where(c => c.PercentualReducaoCBS >= minReducaoCBS);
        if (maxReducaoCBS.HasValue)
            query = query.Where(c => c.PercentualReducaoCBS <= maxReducaoCBS);

        // Filtro por validade NFe
        if (validoNFe.HasValue)
            query = query.Where(c => c.ValidoParaNFe == validoNFe);

        // Filtro por tributação regular
        if (tributacaoRegular.HasValue)
            query = query.Where(c => c.TributacaoRegular == tributacaoRegular);

        // Filtro por crédito presumido
        if (creditoPresumido.HasValue)
            query = query.Where(c => c.CreditoPresumidoOperacoes == creditoPresumido);

        // Filtro por descrição
        if (!string.IsNullOrEmpty(descricao))
            query = query.Where(c => c.DescricaoClassTrib.Contains(descricao) || 
                                     c.CodigoClassTrib.Contains(descricao));

        var total = await query.CountAsync();

        // Ordenação
        IOrderedQueryable<ClassTrib> orderedQuery;
        if (ordenarPor?.ToLower() == "descricao")
            orderedQuery = query.OrderBy(c => c.DescricaoClassTrib);
        else if (ordenarPor?.ToLower() == "reducaoibs")
            orderedQuery = query.OrderByDescending(c => c.PercentualReducaoIBS);
        else if (ordenarPor?.ToLower() == "reducaocbs")
            orderedQuery = query.OrderByDescending(c => c.PercentualReducaoCBS);
        else
            orderedQuery = query.OrderBy(c => c.CodigoSituacaoTributaria).ThenBy(c => c.CodigoClassTrib);

        var items = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<List<string>> GetTiposAliquotaAsync()
    {
        _logger.LogDebug("Listando tipos de alíquota disponíveis");

        return await _context.ClassTribs
            .Where(c => c.Ativo && c.TipoAliquota != null)
            .Select(c => c.TipoAliquota!)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();
    }

    public async Task<List<Controllers.CstOption>> GetCstsAsync()
    {
        _logger.LogDebug("Listando CSTs disponíveis com contagem");

        var csts = await _context.ClassTribs
            .Where(c => c.Ativo)
            .GroupBy(c => new { c.CodigoSituacaoTributaria, c.DescricaoSituacaoTributaria })
            .Select(g => new Controllers.CstOption
            {
                Codigo = g.Key.CodigoSituacaoTributaria,
                Descricao = g.Key.DescricaoSituacaoTributaria ?? "Sem descrição",
                Total = g.Count()
            })
            .OrderBy(x => x.Codigo)
            .ToListAsync();

        return csts;
    }

    public async Task<Controllers.ClassTribEstatisticas> GetEstatisticasAsync()
    {
        _logger.LogDebug("Calculando estatísticas de ClassTrib");

        var classTribs = await _context.ClassTribs
            .Where(c => c.Ativo)
            .ToListAsync();

        var stats = new Controllers.ClassTribEstatisticas
        {
            TotalClassificacoes = classTribs.Count,
            TotalValidoNFe = classTribs.Count(c => c.ValidoParaNFe),
            ClassificacoesPorCST = classTribs
                .GroupBy(c => c.CodigoSituacaoTributaria)
                .ToDictionary(g => g.Key, g => g.Count()),
            ClassificacoesPorTipo = classTribs
                .Where(c => c.TipoAliquota != null)
                .GroupBy(c => c.TipoAliquota!)
                .ToDictionary(g => g.Key, g => g.Count()),
            MediaReducaoIBS = classTribs.Any() ? classTribs.Average(c => c.PercentualReducaoIBS) : 0,
            MediaReducaoCBS = classTribs.Any() ? classTribs.Average(c => c.PercentualReducaoCBS) : 0,
            TotalComReducaoIBS = classTribs.Count(c => c.PercentualReducaoIBS > 0),
            TotalComReducaoCBS = classTribs.Count(c => c.PercentualReducaoCBS > 0),
        };

        return stats;
    }

    #endregion

    #endregion
}
