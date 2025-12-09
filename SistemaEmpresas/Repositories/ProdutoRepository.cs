using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories;

public interface IProdutoRepository
{
    Task<PagedResult<ProdutoListDto>> ListarAsync(ProdutoFiltroDto filtro);
    Task<ProdutoDto?> ObterPorIdAsync(int id);
    Task<Produto> CriarAsync(ProdutoCreateUpdateDto dto, string usuario);
    Task<Produto?> AtualizarAsync(int id, ProdutoCreateUpdateDto dto, string usuario);
    Task<bool> InativarAsync(int id);
    Task<bool> AtivarAsync(int id);
    Task<List<ProdutoComboDto>> ListarParaComboAsync(string? busca = null);
    Task<int> ContarEstoqueCriticoAsync();
}

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProdutoRepository> _logger;

    public ProdutoRepository(AppDbContext context, ILogger<ProdutoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<ProdutoListDto>> ListarAsync(ProdutoFiltroDto filtro)
    {
        _logger.LogInformation("Listando produtos. Página: {Page}, Tamanho: {Size}", 
            filtro.PageNumber, filtro.PageSize);

        var query = _context.Produtos.AsQueryable();

        // Filtro de inativos
        if (!filtro.IncluirInativos)
        {
            query = query.Where(p => !p.Inativo);
        }

        // Filtro de busca
        if (!string.IsNullOrWhiteSpace(filtro.Busca))
        {
            var busca = filtro.Busca.ToLower();
            query = query.Where(p => 
                p.Descricao.ToLower().Contains(busca) ||
                p.CodigoDeBarras.Contains(busca) ||
                p.SequenciaDoProduto.ToString().Contains(busca));
        }

        // Filtro por grupo
        if (filtro.GrupoProduto.HasValue)
        {
            query = query.Where(p => p.SequenciaDoGrupoProduto == filtro.GrupoProduto.Value);
        }

        // Filtro por subgrupo
        if (filtro.SubGrupoProduto.HasValue)
        {
            query = query.Where(p => p.SequenciaDoSubGrupoProduto == filtro.SubGrupoProduto.Value);
        }

        // Filtro matéria prima
        if (filtro.EMateriaPrima.HasValue)
        {
            query = query.Where(p => p.EMateriaPrima == filtro.EMateriaPrima.Value);
        }

        // Filtro estoque crítico
        if (filtro.EstoqueCritico == true)
        {
            query = query.Where(p => p.QuantidadeNoEstoque < p.QuantidadeMinima && p.QuantidadeMinima > 0);
        }

        // Contagem total
        var totalItems = await query.CountAsync();

        // Paginação e projeção - Ordenado por código do menor para o maior
        var items = await query
            .OrderBy(p => p.SequenciaDoProduto)
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(p => new ProdutoListDto
            {
                SequenciaDoProduto = p.SequenciaDoProduto,
                Descricao = p.Descricao,
                CodigoDeBarras = p.CodigoDeBarras,
                GrupoProduto = p.SequenciaDoGrupoProdutoNavigation.Descricao,
                SubGrupoProduto = p.SubGrupoDoProduto != null ? p.SubGrupoDoProduto.Descricao : "",
                Unidade = p.SequenciaDaUnidadeNavigation.Descricao,
                QuantidadeNoEstoque = p.QuantidadeNoEstoque,
                QuantidadeMinima = p.QuantidadeMinima,
                ValorDeCusto = p.ValorDeCusto,
                ValorTotal = p.ValorTotal,
                MargemDeLucro = p.MargemDeLucro,
                Localizacao = p.Localizacao,
                Inativo = p.Inativo,
                EMateriaPrima = p.EMateriaPrima,
                UltimaCompra = p.UltimaCompra,
                UltimoMovimento = p.UltimoMovimento
            })
            .ToListAsync();

        _logger.LogInformation("Produtos encontrados: {Total}", totalItems);

        return new PagedResult<ProdutoListDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize
        };
    }

    public async Task<ProdutoDto?> ObterPorIdAsync(int id)
    {
        var produto = await _context.Produtos
            .Include(p => p.SequenciaDoGrupoProdutoNavigation)
            .Include(p => p.SubGrupoDoProduto)
            .Include(p => p.SequenciaDaUnidadeNavigation)
            .Include(p => p.SequenciaDaClassificacaoNavigation)
                .ThenInclude(cf => cf!.ClassTribNavigation)
            .FirstOrDefaultAsync(p => p.SequenciaDoProduto == id);

        if (produto == null)
            return null;

        // Buscar nome do fornecedor se houver
        string nomeFornecedor = "";
        if (produto.UltimoFornecedor > 0)
        {
            var fornecedor = await _context.Gerals
                .Where(g => g.SequenciaDoGeral == produto.UltimoFornecedor)
                .Select(g => g.RazaoSocial)
                .FirstOrDefaultAsync();
            nomeFornecedor = fornecedor ?? "";
        }

        return new ProdutoDto
        {
            SequenciaDoProduto = produto.SequenciaDoProduto,
            Descricao = produto.Descricao,
            CodigoDeBarras = produto.CodigoDeBarras,
            SequenciaDoGrupoProduto = produto.SequenciaDoGrupoProduto,
            GrupoProduto = produto.SequenciaDoGrupoProdutoNavigation?.Descricao ?? "",
            SequenciaDoSubGrupoProduto = produto.SequenciaDoSubGrupoProduto,
            SubGrupoProduto = produto.SubGrupoDoProduto?.Descricao ?? "",
            SequenciaDaUnidade = produto.SequenciaDaUnidade,
            Unidade = produto.SequenciaDaUnidadeNavigation?.Descricao ?? "",
            SequenciaDaClassificacao = produto.SequenciaDaClassificacao,
            ClassificacaoFiscal = produto.SequenciaDaClassificacaoNavigation?.DescricaoDoNcm ?? "",
            Ncm = produto.SequenciaDaClassificacaoNavigation?.Ncm.ToString() ?? "",
            QuantidadeNoEstoque = produto.QuantidadeNoEstoque,
            QuantidadeMinima = produto.QuantidadeMinima,
            QuantidadeContabil = produto.QuantidadeContabil,
            QuantidadeFisica = produto.QuantidadeFisica,
            Localizacao = produto.Localizacao,
            ValorDeCusto = produto.ValorDeCusto,
            MargemDeLucro = produto.MargemDeLucro,
            ValorTotal = produto.ValorTotal,
            CustoMedio = produto.CustoMedio,
            ValorDeLista = produto.ValorDeLista,
            ValorContabilAtual = produto.ValorContabilAtual,
            EMateriaPrima = produto.EMateriaPrima,
            TipoDoProduto = produto.TipoDoProduto,
            Peso = produto.Peso,
            Medida = produto.Medida,
            MedidaFinal = produto.MedidaFinal,
            Industrializacao = produto.Industrializacao,
            Importado = produto.Importado,
            MaterialAdquiridoDeTerceiro = produto.MaterialAdquiridoDeTerceiro,
            Sucata = produto.Sucata,
            Obsoleto = produto.Obsoleto,
            Inativo = produto.Inativo,
            Usado = produto.Usado,
            UsadoNoProjeto = produto.UsadoNoProjeto,
            Lance = produto.Lance,
            ERegulador = produto.ERegulador,
            TravaReceita = produto.TravaReceita,
            ReceitaConferida = produto.ReceitaConferida,
            NaoSairNoRelatorio = produto.NaoSairNoRelatorio,
            NaoSairNoChecklist = produto.NaoSairNoChecklist,
            MostrarReceitaSecundaria = produto.MostrarReceitaSecundaria,
            NaoMostrarReceita = produto.NaoMostrarReceita,
            ConferidoPeloContabil = produto.ConferidoPeloContabil,
            MpInicial = produto.MpInicial,
            PesoOk = produto.PesoOk,
            UltimaCompra = produto.UltimaCompra,
            UltimoMovimento = produto.UltimoMovimento,
            UltimaCotacao = produto.UltimaCotacao,
            DataDaContagem = produto.DataDaContagem,
            DataDaAlteracao = produto.DataDaAlteracao,
            UltimoFornecedor = produto.UltimoFornecedor,
            NomeFornecedor = nomeFornecedor,
            ParteDoPivo = produto.ParteDoPivo,
            ModeloDoLance = produto.ModeloDoLance,
            Detalhes = produto.Detalhes,
            UsuarioDaAlteracao = produto.UsuarioDaAlteracao,
            SeparadoMontar = produto.SeparadoMontar,
            CompradosAguardando = produto.CompradosAguardando,
            QuantidadeBalanco = produto.QuantidadeBalanco,
            PercentualIpi = produto.SequenciaDaClassificacaoNavigation?.PorcentagemDoIpi ?? 0,
            // Indicadores de vínculo ClassTrib (IBS/CBS)
            TemClassTrib = produto.SequenciaDaClassificacaoNavigation?.ClassTribId != null,
            CodigoClassTrib = produto.SequenciaDaClassificacaoNavigation?.ClassTribNavigation?.CodigoClassTrib ?? "",
            DescricaoClassTrib = produto.SequenciaDaClassificacaoNavigation?.ClassTribNavigation?.DescricaoClassTrib ?? ""
        };
    }

    public async Task<Produto> CriarAsync(ProdutoCreateUpdateDto dto, string usuario)
    {
        // SequenciaDoProduto é IDENTITY no SQL Server, não precisa definir
        var produto = new Produto
        {
            Descricao = dto.Descricao,
            CodigoDeBarras = dto.CodigoDeBarras ?? "",
            SequenciaDoGrupoProduto = dto.SequenciaDoGrupoProduto,
            SequenciaDoSubGrupoProduto = dto.SequenciaDoSubGrupoProduto,
            SequenciaDaUnidade = dto.SequenciaDaUnidade,
            SequenciaDaClassificacao = dto.SequenciaDaClassificacao,
            QuantidadeMinima = dto.QuantidadeMinima,
            Localizacao = dto.Localizacao ?? "",
            ValorDeCusto = dto.ValorDeCusto,
            MargemDeLucro = dto.MargemDeLucro,
            ValorTotal = dto.ValorTotal,
            ValorDeLista = dto.ValorDeLista,
            EMateriaPrima = dto.EMateriaPrima,
            TipoDoProduto = dto.TipoDoProduto,
            Peso = dto.Peso,
            PesoOk = dto.PesoOk,
            Medida = dto.Medida ?? "",
            MedidaFinal = dto.MedidaFinal ?? "",
            Industrializacao = dto.Industrializacao,
            Importado = dto.Importado,
            MaterialAdquiridoDeTerceiro = dto.MaterialAdquiridoDeTerceiro,
            Sucata = dto.Sucata,
            Obsoleto = dto.Obsoleto,
            Inativo = dto.Inativo,
            Usado = dto.Usado,
            UsadoNoProjeto = dto.UsadoNoProjeto,
            Lance = dto.Lance,
            ERegulador = dto.ERegulador,
            TravaReceita = dto.TravaReceita,
            NaoSairNoRelatorio = dto.NaoSairNoRelatorio,
            NaoSairNoChecklist = dto.NaoSairNoChecklist,
            MostrarReceitaSecundaria = dto.MostrarReceitaSecundaria,
            NaoMostrarReceita = dto.NaoMostrarReceita,
            ConferidoPeloContabil = dto.ConferidoPeloContabil,
            MpInicial = dto.MpInicial,
            ParteDoPivo = dto.ParteDoPivo ?? "",
            ModeloDoLance = dto.ModeloDoLance,
            Detalhes = dto.Detalhes ?? "",
            UsuarioDaAlteracao = usuario,
            DataDaAlteracao = DateTime.Now,
            HoraDaAlteracao = DateTime.Now,
            // Valores padrão
            QuantidadeNoEstoque = 0,
            QuantidadeContabil = 0,
            QuantidadeFisica = 0,
            CustoMedio = 0,
            ValorContabilAtual = 0,
            ValorAnterior = 0,
            ValorAtualizado = false,
            UltimoFornecedor = 0,
            Marcar = false,
            ReceitaConferida = false,
            QuantidadeBalanco = 0,
            QtdeInicial = 0,
            SeparadoMontar = 0,
            CompradosAguardando = 0
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Produto criado: {Id} - {Descricao}", produto.SequenciaDoProduto, produto.Descricao);

        return produto;
    }

    public async Task<Produto?> AtualizarAsync(int id, ProdutoCreateUpdateDto dto, string usuario)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return null;

        produto.Descricao = dto.Descricao;
        produto.CodigoDeBarras = dto.CodigoDeBarras ?? "";
        produto.SequenciaDoGrupoProduto = dto.SequenciaDoGrupoProduto;
        produto.SequenciaDoSubGrupoProduto = dto.SequenciaDoSubGrupoProduto;
        produto.SequenciaDaUnidade = dto.SequenciaDaUnidade;
        produto.SequenciaDaClassificacao = dto.SequenciaDaClassificacao;
        produto.QuantidadeMinima = dto.QuantidadeMinima;
        produto.Localizacao = dto.Localizacao ?? "";
        produto.ValorDeCusto = dto.ValorDeCusto;
        produto.MargemDeLucro = dto.MargemDeLucro;
        produto.ValorTotal = dto.ValorTotal;
        produto.ValorDeLista = dto.ValorDeLista;
        produto.EMateriaPrima = dto.EMateriaPrima;
        produto.TipoDoProduto = dto.TipoDoProduto;
        produto.Peso = dto.Peso;
        produto.PesoOk = dto.PesoOk;
        produto.Medida = dto.Medida ?? "";
        produto.MedidaFinal = dto.MedidaFinal ?? "";
        produto.Industrializacao = dto.Industrializacao;
        produto.Importado = dto.Importado;
        produto.MaterialAdquiridoDeTerceiro = dto.MaterialAdquiridoDeTerceiro;
        produto.Sucata = dto.Sucata;
        produto.Obsoleto = dto.Obsoleto;
        produto.Inativo = dto.Inativo;
        produto.Usado = dto.Usado;
        produto.UsadoNoProjeto = dto.UsadoNoProjeto;
        produto.Lance = dto.Lance;
        produto.ERegulador = dto.ERegulador;
        produto.TravaReceita = dto.TravaReceita;
        produto.NaoSairNoRelatorio = dto.NaoSairNoRelatorio;
        produto.NaoSairNoChecklist = dto.NaoSairNoChecklist;
        produto.MostrarReceitaSecundaria = dto.MostrarReceitaSecundaria;
        produto.NaoMostrarReceita = dto.NaoMostrarReceita;
        produto.ConferidoPeloContabil = dto.ConferidoPeloContabil;
        produto.MpInicial = dto.MpInicial;
        produto.ParteDoPivo = dto.ParteDoPivo ?? "";
        produto.ModeloDoLance = dto.ModeloDoLance;
        produto.Detalhes = dto.Detalhes ?? "";
        produto.UsuarioDaAlteracao = usuario;
        produto.DataDaAlteracao = DateTime.Now;
        produto.HoraDaAlteracao = DateTime.Now;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Produto atualizado: {Id} - {Descricao}", produto.SequenciaDoProduto, produto.Descricao);

        return produto;
    }

    public async Task<bool> InativarAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return false;

        produto.Inativo = true;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Produto inativado: {Id}", id);
        return true;
    }

    public async Task<bool> AtivarAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return false;

        produto.Inativo = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Produto ativado: {Id}", id);
        return true;
    }

    public async Task<List<ProdutoComboDto>> ListarParaComboAsync(string? busca = null)
    {
        var query = _context.Produtos
            .Where(p => !p.Inativo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var buscaLower = busca.ToLower();
            query = query.Where(p => 
                p.Descricao.ToLower().Contains(buscaLower) ||
                p.CodigoDeBarras.Contains(busca) ||
                p.SequenciaDoProduto.ToString().Contains(busca));
        }

        return await query
            .OrderBy(p => p.Descricao)
            .Take(50)
            .Select(p => new ProdutoComboDto
            {
                SequenciaDoProduto = p.SequenciaDoProduto,
                Descricao = p.Descricao,
                CodigoDeBarras = p.CodigoDeBarras,
                Unidade = p.SequenciaDaUnidadeNavigation.Descricao,
                ValorTotal = p.ValorTotal,
                QuantidadeNoEstoque = p.QuantidadeNoEstoque
            })
            .ToListAsync();
    }

    public async Task<int> ContarEstoqueCriticoAsync()
    {
        return await _context.Produtos
            .Where(p => !p.Inativo && p.QuantidadeNoEstoque < p.QuantidadeMinima && p.QuantidadeMinima > 0)
            .CountAsync();
    }
}

// Classe auxiliar para paginação (se não existir)
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
