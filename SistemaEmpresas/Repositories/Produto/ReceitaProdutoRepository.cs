using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs.Produto;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories.Produto;

/// <summary>
/// Repositorio de receita do produto (materias primas)
/// Gerencia a composicao de materiais de um produto acabado
/// </summary>
public class ReceitaProdutoRepository : IReceitaProdutoRepository
{
    private readonly AppDbContext _context;

    public ReceitaProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ReceitaProdutoDto> ObterReceitaAsync(int sequenciaDoProduto)
    {
        var produto = await _context.Produtos
            .Where(p => p.SequenciaDoProduto == sequenciaDoProduto)
            .Select(p => new { p.SequenciaDoProduto, p.Descricao })
            .FirstOrDefaultAsync();

        if (produto == null)
        {
            return new ReceitaProdutoDto
            {
                SequenciaDoProduto = sequenciaDoProduto,
                DescricaoDoProduto = "Produto nao encontrado",
                Itens = new List<ReceitaProdutoListDto>()
            };
        }

        var itens = await ListarItensAsync(sequenciaDoProduto);

        return new ReceitaProdutoDto
        {
            SequenciaDoProduto = produto.SequenciaDoProduto,
            DescricaoDoProduto = produto.Descricao ?? string.Empty,
            Itens = itens
        };
    }

    /// <inheritdoc />
    public async Task<List<ReceitaProdutoListDto>> ListarItensAsync(int sequenciaDoProduto)
    {
        var itens = await _context.MateriaPrimas
            .Where(mp => mp.SequenciaDoProduto == sequenciaDoProduto)
            .Join(
                _context.Produtos,
                mp => mp.SequenciaDaMateriaPrima,
                p => p.SequenciaDoProduto,
                (mp, p) => new { MateriaPrima = mp, Produto = p }
            )
            .Select(x => new ReceitaProdutoListDto
            {
                SequenciaDoProduto = x.MateriaPrima.SequenciaDoProduto,
                SequenciaDaMateriaPrima = x.MateriaPrima.SequenciaDaMateriaPrima,
                DescricaoDaMateriaPrima = x.Produto.Descricao ?? string.Empty,
                Quantidade = x.MateriaPrima.QuantidadeDeMateriaPrima,
                Unidade = x.Produto.SequenciaDaUnidadeNavigation != null ? x.Produto.SequenciaDaUnidadeNavigation.SiglaDaUnidade : string.Empty,
                Peso = x.Produto.Peso,
                ValorDeCusto = x.Produto.ValorDeCusto,
                CustoTotal = x.MateriaPrima.QuantidadeDeMateriaPrima * x.Produto.ValorDeCusto,
                PesoTotal = x.MateriaPrima.QuantidadeDeMateriaPrima * x.Produto.Peso
            })
            .OrderBy(x => x.DescricaoDaMateriaPrima)
            .ToListAsync();

        return itens;
    }

    /// <inheritdoc />
    public async Task<ReceitaProdutoListDto> AdicionarItemAsync(int sequenciaDoProduto, ReceitaProdutoCreateUpdateDto dto)
    {
        // Verifica se ja existe
        if (await ItemExisteAsync(sequenciaDoProduto, dto.SequenciaDaMateriaPrima))
        {
            throw new InvalidOperationException("Esta materia prima ja existe na receita do produto.");
        }

        // Verifica se nao esta tentando adicionar o proprio produto como materia prima
        if (sequenciaDoProduto == dto.SequenciaDaMateriaPrima)
        {
            throw new InvalidOperationException("Nao e possivel adicionar o proprio produto como materia prima.");
        }

        // Verifica se a materia prima existe
        var materiaPrimaExiste = await _context.Produtos
            .AnyAsync(p => p.SequenciaDoProduto == dto.SequenciaDaMateriaPrima);

        if (!materiaPrimaExiste)
        {
            throw new InvalidOperationException("Materia prima nao encontrada.");
        }

        var novoItem = new MateriaPrima
        {
            SequenciaDoProduto = sequenciaDoProduto,
            SequenciaDaMateriaPrima = dto.SequenciaDaMateriaPrima,
            QuantidadeDeMateriaPrima = dto.Quantidade
        };

        _context.MateriaPrimas.Add(novoItem);
        await _context.SaveChangesAsync();

        // Busca os dados completos para retorno
        var itemCompleto = await _context.MateriaPrimas
            .Where(mp => mp.SequenciaDoProduto == sequenciaDoProduto && mp.SequenciaDaMateriaPrima == dto.SequenciaDaMateriaPrima)
            .Join(
                _context.Produtos,
                mp => mp.SequenciaDaMateriaPrima,
                p => p.SequenciaDoProduto,
                (mp, p) => new { MateriaPrima = mp, Produto = p }
            )
            .Select(x => new ReceitaProdutoListDto
            {
                SequenciaDoProduto = x.MateriaPrima.SequenciaDoProduto,
                SequenciaDaMateriaPrima = x.MateriaPrima.SequenciaDaMateriaPrima,
                DescricaoDaMateriaPrima = x.Produto.Descricao ?? string.Empty,
                Quantidade = x.MateriaPrima.QuantidadeDeMateriaPrima,
                Unidade = x.Produto.SequenciaDaUnidadeNavigation != null ? x.Produto.SequenciaDaUnidadeNavigation.SiglaDaUnidade : string.Empty,
                Peso = x.Produto.Peso,
                ValorDeCusto = x.Produto.ValorDeCusto,
                CustoTotal = x.MateriaPrima.QuantidadeDeMateriaPrima * x.Produto.ValorDeCusto,
                PesoTotal = x.MateriaPrima.QuantidadeDeMateriaPrima * x.Produto.Peso
            })
            .FirstOrDefaultAsync();

        return itemCompleto ?? new ReceitaProdutoListDto
        {
            SequenciaDoProduto = sequenciaDoProduto,
            SequenciaDaMateriaPrima = dto.SequenciaDaMateriaPrima,
            Quantidade = dto.Quantidade
        };
    }

    /// <inheritdoc />
    public async Task<ReceitaProdutoListDto?> AtualizarItemAsync(int sequenciaDoProduto, int sequenciaDaMateriaPrima, ReceitaProdutoCreateUpdateDto dto)
    {
        var item = await _context.MateriaPrimas
            .FirstOrDefaultAsync(mp => mp.SequenciaDoProduto == sequenciaDoProduto && mp.SequenciaDaMateriaPrima == sequenciaDaMateriaPrima);

        if (item == null)
        {
            return null;
        }

        item.QuantidadeDeMateriaPrima = dto.Quantidade;
        await _context.SaveChangesAsync();

        // Busca os dados completos para retorno
        var itemCompleto = await _context.MateriaPrimas
            .Where(mp => mp.SequenciaDoProduto == sequenciaDoProduto && mp.SequenciaDaMateriaPrima == sequenciaDaMateriaPrima)
            .Join(
                _context.Produtos,
                mp => mp.SequenciaDaMateriaPrima,
                p => p.SequenciaDoProduto,
                (mp, p) => new { MateriaPrima = mp, Produto = p }
            )
            .Select(x => new ReceitaProdutoListDto
            {
                SequenciaDoProduto = x.MateriaPrima.SequenciaDoProduto,
                SequenciaDaMateriaPrima = x.MateriaPrima.SequenciaDaMateriaPrima,
                DescricaoDaMateriaPrima = x.Produto.Descricao ?? string.Empty,
                Quantidade = x.MateriaPrima.QuantidadeDeMateriaPrima,
                Unidade = x.Produto.SequenciaDaUnidadeNavigation != null ? x.Produto.SequenciaDaUnidadeNavigation.SiglaDaUnidade : string.Empty,
                Peso = x.Produto.Peso,
                ValorDeCusto = x.Produto.ValorDeCusto,
                CustoTotal = x.MateriaPrima.QuantidadeDeMateriaPrima * x.Produto.ValorDeCusto,
                PesoTotal = x.MateriaPrima.QuantidadeDeMateriaPrima * x.Produto.Peso
            })
            .FirstOrDefaultAsync();

        return itemCompleto;
    }

    /// <inheritdoc />
    public async Task<bool> RemoverItemAsync(int sequenciaDoProduto, int sequenciaDaMateriaPrima)
    {
        var item = await _context.MateriaPrimas
            .FirstOrDefaultAsync(mp => mp.SequenciaDoProduto == sequenciaDoProduto && mp.SequenciaDaMateriaPrima == sequenciaDaMateriaPrima);

        if (item == null)
        {
            return false;
        }

        _context.MateriaPrimas.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<int> LimparReceitaAsync(int sequenciaDoProduto)
    {
        var itens = await _context.MateriaPrimas
            .Where(mp => mp.SequenciaDoProduto == sequenciaDoProduto)
            .ToListAsync();

        if (itens.Count == 0)
        {
            return 0;
        }

        _context.MateriaPrimas.RemoveRange(itens);
        await _context.SaveChangesAsync();

        return itens.Count;
    }

    /// <inheritdoc />
    public async Task<bool> ItemExisteAsync(int sequenciaDoProduto, int sequenciaDaMateriaPrima)
    {
        return await _context.MateriaPrimas
            .AnyAsync(mp => mp.SequenciaDoProduto == sequenciaDoProduto && mp.SequenciaDaMateriaPrima == sequenciaDaMateriaPrima);
    }
}
