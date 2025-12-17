using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories.Fiscal;

/// <summary>
/// Repositório de Classificação Fiscal
/// Acesso a dados com EF Core para tabela "Classificação Fiscal"
/// 
/// ARQUITETURA:
/// - ClassificacaoFiscal contém dados VB6 (NCM, IPI, CEST, etc.)
/// - ClassTrib contém dados SVRS (CST, percentuais IBS/CBS, etc.)
/// - Associação via FK ClassTribId + navigation property ClassTribNavigation
/// </summary>
public class ClassificacaoFiscalRepository : IClassificacaoFiscalRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ClassificacaoFiscalRepository> _logger;

    public ClassificacaoFiscalRepository(
        AppDbContext context,
        ILogger<ClassificacaoFiscalRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Métodos CRUD Básicos

    public async Task<ClassificacaoFiscal?> GetByIdAsync(short id)
    {
        _logger.LogInformation("Buscando classificação fiscal por ID: {Id}", id);
        return await _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .FirstOrDefaultAsync(c => c.SequenciaDaClassificacao == id);
    }

    public async Task<ClassificacaoFiscal?> GetByNcmAsync(string ncm)
    {
        if (string.IsNullOrWhiteSpace(ncm))
            throw new ArgumentException("NCM não pode ser vazio", nameof(ncm));

        // Tentar converter string para int
        if (!int.TryParse(ncm, out int ncmInt))
            throw new ArgumentException("NCM deve ser um número válido", nameof(ncm));

        _logger.LogInformation("Buscando classificação fiscal por NCM: {NCM}", ncm);
        return await _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .FirstOrDefaultAsync(c => c.Ncm == ncmInt);
    }

    public async Task<List<ClassificacaoFiscal>> GetByClassTribIdAsync(int classTribId)
    {
        _logger.LogInformation("Buscando classificações fiscais por ClassTribId: {ClassTribId}", classTribId);
        return await _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .Where(c => c.ClassTribId == classTribId)
            .OrderBy(c => c.Ncm)
            .ToListAsync();
    }

    public async Task<List<ClassificacaoFiscal>> GetAllAsync(bool incluirInativos = false)
    {
        _logger.LogInformation("Listando todas as classificações fiscais. Incluir inativos: {IncluirInativos}", incluirInativos);

        var query = _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .AsQueryable();

        if (!incluirInativos)
        {
            query = query.Where(c => !c.Inativo);
        }

        return await query
            .OrderBy(c => c.Ncm)
            .ToListAsync();
    }

    public async Task<List<ClassificacaoFiscal>> GetClassificacoesNFeAsync()
    {
        _logger.LogInformation("Listando classificações fiscais válidas para NFe");

        return await _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .Where(c => c.ClassTribNavigation != null && c.ClassTribNavigation.ValidoParaNFe == true)
            .Where(c => !c.Inativo)
            .OrderBy(c => c.ClassTribNavigation!.CodigoSituacaoTributaria)
            .ThenBy(c => c.ClassTribNavigation!.CodigoClassTrib)
            .ToListAsync();
    }

    public async Task<(List<ClassificacaoFiscal> items, int total)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? filtroNcm = null,
        string? filtroDescricao = null,
        bool? somenteNFe = null,
        bool incluirInativos = false,
        string? filtroTributacao = null)
    {
        _logger.LogInformation(
            "Listando classificações fiscais paginadas. Página: {PageNumber}, Tamanho: {PageSize}",
            pageNumber, pageSize);

        var query = _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .AsQueryable();

        // Filtro de inativos
        if (!incluirInativos)
        {
            query = query.Where(c => !c.Inativo);
        }

        // Filtro NFe - usa ClassTrib.ValidoParaNFe
        if (somenteNFe == true)
        {
            query = query.Where(c => c.ClassTribNavigation != null && c.ClassTribNavigation.ValidoParaNFe == true);
        }

        // Filtro Tributação
        if (!string.IsNullOrWhiteSpace(filtroTributacao))
        {
            if (filtroTributacao == "vinculados")
            {
                query = query.Where(c => c.ClassTribId != null);
            }
            else if (filtroTributacao == "pendentes")
            {
                query = query.Where(c => c.ClassTribId == null);
            }
        }

        // Filtro NCM
        if (!string.IsNullOrWhiteSpace(filtroNcm))
        {
            if (int.TryParse(filtroNcm, out int ncmInt))
            {
                query = query.Where(c => c.Ncm == ncmInt);
            }
        }

        // Filtro Descrição - busca no NCM (se for número) e na descrição
        if (!string.IsNullOrWhiteSpace(filtroDescricao))
        {
            var termoBusca = filtroDescricao.Trim();
            
            // Se parece ser um NCM (somente números), busca também no campo NCM
            if (long.TryParse(termoBusca, out long ncmBusca))
            {
                // Para busca "começa com", calculamos o range de valores
                // Ex: "7307" busca NCM >= 73070000 e < 73080000
                var digitos = termoBusca.Length;
                var multiplicador = (long)Math.Pow(10, 8 - digitos);
                var ncmInicio = ncmBusca * multiplicador;
                var ncmFim = (ncmBusca + 1) * multiplicador;
                
                query = query.Where(c =>
                    (c.Ncm >= ncmInicio && c.Ncm < ncmFim) ||
                    (c.DescricaoDoNcm != null && c.DescricaoDoNcm.Contains(termoBusca)) ||
                    (c.ClassTribNavigation != null && c.ClassTribNavigation.DescricaoClassTrib != null && 
                     c.ClassTribNavigation.DescricaoClassTrib.Contains(termoBusca))
                );
            }
            else
            {
                // Busca apenas na descrição
                query = query.Where(c =>
                    (c.DescricaoDoNcm != null && c.DescricaoDoNcm.Contains(termoBusca)) ||
                    (c.ClassTribNavigation != null && c.ClassTribNavigation.DescricaoClassTrib != null && 
                     c.ClassTribNavigation.DescricaoClassTrib.Contains(termoBusca))
                );
            }
        }

        // Total de registros
        var total = await query.CountAsync();

        // Paginação - ordenado por ID (SequenciaDaClassificacao)
        var items = await query
            .OrderBy(c => c.SequenciaDaClassificacao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        _logger.LogInformation("Resultado paginado: {Total} total, {Items} na página", total, items.Count);

        return (items, total);
    }

    public async Task<ClassificacaoFiscal> CreateAsync(ClassificacaoFiscal classificacao)
    {
        if (classificacao == null)
            throw new ArgumentNullException(nameof(classificacao));

        _logger.LogInformation("Criando nova classificação fiscal. NCM: {NCM}", classificacao.Ncm);

        _context.ClassificacaoFiscals.Add(classificacao);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Classificação fiscal criada com ID: {Id}", classificacao.SequenciaDaClassificacao);
        return classificacao;
    }

    public async Task<ClassificacaoFiscal> UpdateAsync(ClassificacaoFiscal classificacao)
    {
        if (classificacao == null)
            throw new ArgumentNullException(nameof(classificacao));

        _logger.LogInformation("Atualizando classificação fiscal ID: {Id}", classificacao.SequenciaDaClassificacao);

        // Busca a entidade existente no contexto
        var existente = await _context.ClassificacaoFiscals
            .FirstOrDefaultAsync(c => c.SequenciaDaClassificacao == classificacao.SequenciaDaClassificacao);

        if (existente == null)
            throw new InvalidOperationException($"Classificação fiscal {classificacao.SequenciaDaClassificacao} não encontrada");

        // Atualiza apenas os campos necessários (evita conflito de tracking)
        existente.Ncm = classificacao.Ncm;
        existente.DescricaoDoNcm = classificacao.DescricaoDoNcm;
        existente.PorcentagemDoIpi = classificacao.PorcentagemDoIpi;
        existente.AnexoDaReducao = classificacao.AnexoDaReducao;
        existente.AliquotaDoAnexo = classificacao.AliquotaDoAnexo;
        existente.ProdutoDiferido = classificacao.ProdutoDiferido;
        existente.ReducaoDeBaseDeCalculo = classificacao.ReducaoDeBaseDeCalculo;
        existente.Inativo = classificacao.Inativo;
        existente.Iva = classificacao.Iva;
        existente.TemConvenio = classificacao.TemConvenio;
        existente.Cest = classificacao.Cest;
        existente.UnExterior = classificacao.UnExterior;
        existente.ClassTribId = classificacao.ClassTribId;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Classificação fiscal atualizada com sucesso");
        return existente;
    }

    public async Task<bool> DeleteAsync(short id)
    {
        _logger.LogInformation("Excluindo (soft delete) classificação fiscal ID: {Id}", id);

        var classificacao = await GetByIdAsync(id);
        if (classificacao == null)
        {
            _logger.LogWarning("Classificação fiscal não encontrada para exclusão: {Id}", id);
            return false;
        }

        // Soft delete via campo Inativo
        classificacao.Inativo = true;
        await UpdateAsync(classificacao);

        _logger.LogInformation("Classificação fiscal marcada como inativa");
        return true;
    }

    #endregion

    #region Métodos de Associação ClassTrib

    public async Task<ClassificacaoFiscal?> AssociarClassTribAsync(short classificacaoId, int? classTribId)
    {
        _logger.LogInformation("Associando ClassTrib {ClassTribId} à classificação fiscal {ClassificacaoId}", 
            classTribId, classificacaoId);

        var classificacao = await _context.ClassificacaoFiscals
            .FirstOrDefaultAsync(c => c.SequenciaDaClassificacao == classificacaoId);

        if (classificacao == null)
        {
            _logger.LogWarning("Classificação fiscal não encontrada: {Id}", classificacaoId);
            return null;
        }

        // Verificar se o ClassTrib existe (se não for null)
        if (classTribId.HasValue)
        {
            var classTribExiste = await _context.ClassTribs.AnyAsync(ct => ct.Id == classTribId.Value);
            if (!classTribExiste)
            {
                _logger.LogWarning("ClassTrib não encontrado: {Id}", classTribId);
                throw new ArgumentException($"ClassTrib com ID {classTribId} não encontrado");
            }
        }

        classificacao.ClassTribId = classTribId;
        await _context.SaveChangesAsync();

        // Recarregar com Include
        return await GetByIdAsync(classificacaoId);
    }

    public async Task<bool> ExistsClassificacaoComClassTribAsync(int classTribId)
    {
        return await _context.ClassificacaoFiscals
            .AnyAsync(c => c.ClassTribId == classTribId);
    }

    #endregion

    #region Métodos de Pesquisa

    public async Task<List<ClassificacaoFiscal>> SearchAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return await GetAllAsync();

        _logger.LogInformation("Pesquisando classificações fiscais. Termo: {Termo}", termo);

        return await _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .Where(c =>
                c.Ncm.ToString().Contains(termo) ||
                (c.DescricaoDoNcm != null && c.DescricaoDoNcm.Contains(termo)) ||
                (c.ClassTribNavigation != null && c.ClassTribNavigation.DescricaoClassTrib != null && 
                 c.ClassTribNavigation.DescricaoClassTrib.Contains(termo)) ||
                (c.ClassTribNavigation != null && c.ClassTribNavigation.CodigoClassTrib != null && 
                 c.ClassTribNavigation.CodigoClassTrib.Contains(termo))
            )
            .Where(c => !c.Inativo)
            .OrderBy(c => c.Ncm)
            .Take(100) // Limitar resultados
            .ToListAsync();
    }

    public async Task<List<ClassificacaoFiscal>> GetByCstAsync(string cst)
    {
        if (string.IsNullOrWhiteSpace(cst))
            throw new ArgumentException("CST não pode ser vazio", nameof(cst));

        _logger.LogInformation("Buscando classificações por CST: {CST}", cst);

        return await _context.ClassificacaoFiscals
            .Include(c => c.ClassTribNavigation)
            .Where(c => c.ClassTribNavigation != null && c.ClassTribNavigation.CodigoSituacaoTributaria == cst)
            .Where(c => !c.Inativo)
            .OrderBy(c => c.ClassTribNavigation!.CodigoClassTrib)
            .ToListAsync();
    }

    #endregion
}
