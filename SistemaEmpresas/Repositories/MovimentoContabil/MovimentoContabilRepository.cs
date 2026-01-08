using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.DTOs.MovimentoContabil;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories.MovimentoContabil;

public class MovimentoContabilRepository : IMovimentoContabilRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<MovimentoContabilRepository> _logger;

    public MovimentoContabilRepository(AppDbContext context, ILogger<MovimentoContabilRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EstoqueInfoDto?> ObterEstoqueInfoAsync(int sequenciaDoProduto)
    {
        var produto = await _context.Produtos
            .Include(p => p.SequenciaDaUnidadeNavigation)
            .FirstOrDefaultAsync(p => p.SequenciaDoProduto == sequenciaDoProduto);

        if (produto == null)
            return null;

        var estoqueContabil = await GetSaldoEstoqueAsync(sequenciaDoProduto);

        return new EstoqueInfoDto
        {
            SequenciaDoProduto = produto.SequenciaDoProduto,
            Descricao = produto.Descricao,
            CodigoDeBarras = produto.CodigoDeBarras,
            Localizacao = produto.Localizacao,
            SiglaUnidade = produto.SequenciaDaUnidadeNavigation?.SiglaDaUnidade ?? "UN",
            EstoqueContabil = estoqueContabil,
            ValorCusto = produto.ValorDeCusto,
            TipoDoProduto = produto.TipoDoProduto
        };
    }

    public async Task<List<EstoqueInfoDto>> BuscarProdutosParaMovimentoAsync(string? busca, int limite = 50)
    {
        var query = _context.Produtos
            .Include(p => p.SequenciaDaUnidadeNavigation)
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

        var produtos = await query
            .OrderBy(p => p.Descricao)
            .Take(limite)
            .ToListAsync();

        var result = new List<EstoqueInfoDto>();

        foreach (var produto in produtos)
        {
            var estoqueContabil = await GetSaldoEstoqueAsync(produto.SequenciaDoProduto);
            
            result.Add(new EstoqueInfoDto
            {
                SequenciaDoProduto = produto.SequenciaDoProduto,
                Descricao = produto.Descricao,
                CodigoDeBarras = produto.CodigoDeBarras,
                Localizacao = produto.Localizacao,
                SiglaUnidade = produto.SequenciaDaUnidadeNavigation?.SiglaDaUnidade ?? "UN",
                EstoqueContabil = estoqueContabil,
                ValorCusto = produto.ValorDeCusto,
                TipoDoProduto = produto.TipoDoProduto
            });
        }

        return result;
    }

    public async Task<AjusteMovimentoContabilResultDto> RealizarAjusteAsync(AjusteMovimentoContabilDto dto, string usuario)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            decimal estoqueAtual;
            decimal valorCusto;
            short tipoDoProduto;
            string descricao;
            string documentoPrefix = "AJUSTE";

            if (dto.EhConjunto)
            {
                var conjunto = await _context.Conjuntos.FindAsync(dto.SequenciaDoProduto);
                if (conjunto == null)
                {
                    return new AjusteMovimentoContabilResultDto
                    {
                        Sucesso = false,
                        Mensagem = "Conjunto não encontrado"
                    };
                }

                estoqueAtual = await GetSaldoEstoqueConjuntoAsync(dto.SequenciaDoProduto);
                
                valorCusto = conjunto.ValorTotal;
                tipoDoProduto = 0; // Acabado
                descricao = conjunto.Descricao;
                documentoPrefix = "PROD"; // Produção
            }
            else
            {
                var produto = await _context.Produtos.FindAsync(dto.SequenciaDoProduto);
                if (produto == null)
                {
                    return new AjusteMovimentoContabilResultDto
                    {
                        Sucesso = false,
                        Mensagem = "Produto não encontrado"
                    };
                }

                estoqueAtual = await GetSaldoEstoqueAsync(dto.SequenciaDoProduto);
                valorCusto = dto.ValorCusto ?? produto.ValorDeCusto;
                tipoDoProduto = produto.TipoDoProduto;
                descricao = produto.Descricao;
            }

            var diferenca = dto.QuantidadeFisica - estoqueAtual;

            if (diferenca == 0)
            {
                return new AjusteMovimentoContabilResultDto
                {
                    Sucesso = true,
                    Mensagem = "Estoque já está correto",
                    EstoqueAnterior = estoqueAtual,
                    EstoqueNovo = estoqueAtual,
                    Diferenca = 0,
                    QuantidadeAjustada = 0
                };
            }

            short tipoMovimento = diferenca > 0 ? (short)0 : (short)1;
            var quantidadeAjuste = Math.Abs(diferenca);
            var sequenciaGeral = dto.SequenciaDoGeral ?? 1;
            var dataAtual = DateTime.Now;
            var documento = $"{documentoPrefix} {dataAtual:ddMMyyyy-HHmm}";

            var baixa = new BaixaDoEstoqueContabil
            {
                TipoDoMovimento = tipoMovimento,
                DataDoMovimento = dataAtual,
                Documento = documento,
                SequenciaDoGeral = sequenciaGeral,
                SequenciaDoProduto = dto.EhConjunto ? 0 : dto.SequenciaDoProduto,
                SequenciaDoConjunto = dto.EhConjunto ? dto.SequenciaDoProduto : 0,
                Quantidade = quantidadeAjuste,
                ValorUnitario = valorCusto,
                ValorDeCusto = valorCusto,
                ValorTotal = Math.Round(quantidadeAjuste * valorCusto, 2),
                Observacao = string.IsNullOrWhiteSpace(dto.Observacao) 
                    ? $"Ajuste por {usuario}. Ant: {estoqueAtual:N4}, Fís: {dto.QuantidadeFisica:N4}"
                    : $"{dto.Observacao} | Por {usuario}. Ant: {estoqueAtual:N4}, Fís: {dto.QuantidadeFisica:N4}",
                ValorDoPis = 0,
                ValorDoCofins = 0,
                ValorDoIcms = 0,
                ValorDoIpi = 0,
                ValorDoFrete = 0,
                ValorDaSubstituicao = 0,
                TipoDoProduto = tipoDoProduto,
                Estoque = dto.EhConjunto ? "C" : "P",
                SequenciaDoMovimento = 0,
                SequenciaDoItem2 = 0,
                SequenciaDaDespesa = 0
            };

            _context.BaixaDoEstoqueContabils.Add(baixa);

            if (dto.EhConjunto && tipoMovimento == 0)
            {
                var itens = await _context.ItensDoConjuntos
                    .Include(i => i.SequenciaDoProdutoNavigation)
                    .Where(i => i.SequenciaDoConjunto == dto.SequenciaDoProduto)
                    .ToListAsync();

                foreach (var item in itens)
                {
                    var baixaComponente = new BaixaDoEstoqueContabil
                    {
                        TipoDoMovimento = 1, // Saída
                        DataDoMovimento = dataAtual,
                        Documento = documento,
                        SequenciaDoGeral = sequenciaGeral,
                        SequenciaDoProduto = item.SequenciaDoProduto,
                        SequenciaDoConjunto = 0,
                        Quantidade = item.QuantidadeDoProduto * quantidadeAjuste,
                        ValorUnitario = item.SequenciaDoProdutoNavigation.ValorDeCusto,
                        ValorDeCusto = item.SequenciaDoProdutoNavigation.ValorDeCusto,
                        ValorTotal = Math.Round((item.QuantidadeDoProduto * quantidadeAjuste) * item.SequenciaDoProdutoNavigation.ValorDeCusto, 2),
                        Observacao = $"Baixa receita p/ produção conjunto {dto.SequenciaDoProduto}",
                        ValorDoPis = 0,
                        ValorDoCofins = 0,
                        ValorDoIcms = 0,
                        ValorDoIpi = 0,
                        ValorDoFrete = 0,
                        ValorDaSubstituicao = 0,
                        TipoDoProduto = item.SequenciaDoProdutoNavigation.TipoDoProduto,
                        Estoque = "P",
                        SequenciaDoMovimento = 0,
                        SequenciaDoItem2 = 0,
                        SequenciaDaDespesa = 0
                    };
                    _context.BaixaDoEstoqueContabils.Add(baixaComponente);
                }
            }

            await _context.SaveChangesAsync();

            if (!dto.EhConjunto)
                await AtualizarEstoqueContabilProdutoAsync(dto.SequenciaDoProduto);
            else
                await AtualizarEstoqueContabilConjuntoAsync(dto.SequenciaDoProduto);

            await transaction.CommitAsync();

            return new AjusteMovimentoContabilResultDto
            {
                Sucesso = true,
                Mensagem = $"Ajuste realizado com sucesso",
                SequenciaDaBaixa = baixa.SequenciaDaBaixa,
                TipoMovimento = baixa.TipoDoMovimento,
                QuantidadeAjustada = Math.Abs(diferenca),
                Diferenca = diferenca,
                EstoqueAnterior = estoqueAtual,
                EstoqueNovo = dto.QuantidadeFisica,
                Documento = baixa.Documento
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar ajuste");
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<AjusteMovimentoContabilLoteResultDto> RealizarAjusteLoteAsync(AjusteMovimentoContabilLoteDto dto, string usuario)
    {
        var resultado = new AjusteMovimentoContabilLoteResultDto { Resultados = new List<AjusteMovimentoContabilResultDto>() };
        foreach (var item in dto.Itens)
        {
            var ajusteDto = new AjusteMovimentoContabilDto
            {
                SequenciaDoProduto = item.SequenciaDoProduto,
                QuantidadeFisica = item.QuantidadeFisica,
                Observacao = item.Observacao ?? dto.ObservacaoGeral,
                EhConjunto = false
            };
            var res = await RealizarAjusteAsync(ajusteDto, usuario);
            resultado.Resultados.Add(res);
            resultado.TotalProcessados++;
            if (res.Sucesso) { if (res.QuantidadeAjustada > 0) resultado.TotalAjustados++; else resultado.TotalSemAlteracao++; }
            else resultado.TotalErros++;
        }
        resultado.Sucesso = resultado.TotalErros == 0;
        return resultado;
    }

    public async Task<PagedResult<MovimentoEstoqueDto>> ListarMovimentosAsync(MovimentoEstoqueFiltroDto filtro)
    {
        var query = _context.BaixaDoEstoqueContabils
            .Include(b => b.SequenciaDoGeralNavigation)
            .AsQueryable();

        if (filtro.EhConjunto)
            query = query.Where(b => b.SequenciaDoConjunto == filtro.SequenciaDoProduto && b.SequenciaDoProduto == 0 && b.Estoque == "C");
        else
            query = query.Where(b => b.SequenciaDoProduto == filtro.SequenciaDoProduto && b.Estoque == "P");

        if (filtro.DataInicial.HasValue) query = query.Where(b => b.DataDoMovimento >= filtro.DataInicial.Value);
        if (filtro.DataFinal.HasValue) query = query.Where(b => b.DataDoMovimento <= filtro.DataFinal.Value.AddDays(1));
        if (filtro.TipoMovimento.HasValue) query = query.Where(b => b.TipoDoMovimento == filtro.TipoMovimento.Value);
        if (!string.IsNullOrWhiteSpace(filtro.Documento)) query = query.Where(b => b.Documento.Contains(filtro.Documento));

        var totalItems = await query.CountAsync();
        var movimentos = await query
            .OrderBy(b => b.DataDoMovimento).ThenBy(b => b.SequenciaDaBaixa)
            .Skip((filtro.PageNumber - 1) * filtro.PageSize).Take(filtro.PageSize)
            .Select(b => new MovimentoEstoqueDto
            {
                SequenciaDaBaixa = b.SequenciaDaBaixa,
                DataMovimento = b.DataDoMovimento,
                Documento = b.Documento,
                TipoMovimento = b.TipoDoMovimento,
                Quantidade = b.Quantidade,
                ValorUnitario = b.ValorUnitario,
                ValorCusto = b.ValorDeCusto,
                ValorTotal = b.ValorTotal,
                Observacao = b.Observacao,
                RazaoSocialGeral = b.SequenciaDoGeralNavigation != null ? b.SequenciaDoGeralNavigation.RazaoSocial : null
            }).ToListAsync();

        decimal saldoAcumulado = 0;
        var primeiro = movimentos.FirstOrDefault();
        if (primeiro != null)
        {
            saldoAcumulado = await _context.BaixaDoEstoqueContabils
                .Where(b => b.SequenciaDoProduto == filtro.SequenciaDoProduto && b.Estoque == "P" &&
                            (b.DataDoMovimento < primeiro.DataMovimento || (b.DataDoMovimento == primeiro.DataMovimento && b.SequenciaDaBaixa < primeiro.SequenciaDaBaixa)))
                .SumAsync(b => b.Quantidade * (b.TipoDoMovimento == 0 ? 1 : -1));
        }

        foreach (var mov in movimentos)
        {
            saldoAcumulado += mov.Quantidade * (mov.TipoMovimento == 0 ? 1 : -1);
            mov.SaldoAposMovimento = saldoAcumulado;
        }

        return new PagedResult<MovimentoEstoqueDto> { Items = movimentos, TotalCount = totalItems, PageNumber = filtro.PageNumber, PageSize = filtro.PageSize };
    }

    public async Task<decimal> RecalcularEstoqueContabilAsync(int sequenciaDoProduto)
    {
        var saldo = await GetSaldoEstoqueAsync(sequenciaDoProduto);
        await AtualizarEstoqueProdutoAsync(sequenciaDoProduto, saldo);
        return saldo;
    }

    public async Task AtualizarEstoqueContabilProdutoAsync(int sequenciaDoProduto)
    {
        var produto = await _context.Produtos.FindAsync(sequenciaDoProduto);
        if (produto != null)
        {
            var novoEstoque = await GetSaldoEstoqueAsync(sequenciaDoProduto);
            produto.QuantidadeContabil = novoEstoque;
            produto.UltimoMovimento = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AtualizarEstoqueContabilConjuntoAsync(int sequenciaDoConjunto)
    {
        var conjunto = await _context.Conjuntos.FindAsync(sequenciaDoConjunto);
        if (conjunto != null)
        {
            var novoEstoque = await GetSaldoEstoqueConjuntoAsync(sequenciaDoConjunto);
            conjunto.QuantidadeContabil = novoEstoque;
            conjunto.UltimoMovimento = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<EstoqueInfoDto>> BuscarConjuntosParaMovimentoAsync(string? busca, int limite = 50)
    {
        var query = _context.Conjuntos.Include(c => c.SequenciaDaUnidadeNavigation).Where(c => !c.Inativo).AsQueryable();
        if (!string.IsNullOrWhiteSpace(busca)) query = query.Where(c => c.Descricao.Contains(busca) || c.SequenciaDoConjunto.ToString().Contains(busca));
        var conjuntos = await query.OrderBy(c => c.Descricao).Take(limite).ToListAsync();
        var result = new List<EstoqueInfoDto>();
        foreach (var c in conjuntos)
        {
            result.Add(new EstoqueInfoDto
            {
                SequenciaDoProduto = c.SequenciaDoConjunto,
                Descricao = c.Descricao,
                SiglaUnidade = c.SequenciaDaUnidadeNavigation?.SiglaDaUnidade ?? "UN",
                EstoqueContabil = await GetSaldoEstoqueConjuntoAsync(c.SequenciaDoConjunto),
                ValorCusto = c.ValorTotal,
                TipoDoProduto = 0
            });
        }
        return result;
    }

    public async Task<EstoqueInfoDto?> ObterEstoqueConjuntoInfoAsync(int sequenciaDoConjunto)
    {
        var c = await _context.Conjuntos.Include(un => un.SequenciaDaUnidadeNavigation).FirstOrDefaultAsync(x => x.SequenciaDoConjunto == sequenciaDoConjunto);
        if (c == null) return null;
        return new EstoqueInfoDto
        {
            SequenciaDoProduto = c.SequenciaDoConjunto,
            Descricao = c.Descricao,
            SiglaUnidade = c.SequenciaDaUnidadeNavigation?.SiglaDaUnidade ?? "UN",
            EstoqueContabil = await GetSaldoEstoqueConjuntoAsync(c.SequenciaDoConjunto),
            ValorCusto = c.ValorTotal,
            TipoDoProduto = 0
        };
    }

    public async Task<List<DespesaMvtoContabilItemDto>> BuscarDespesasParaMovimentoAsync(string? busca, int limite = 50)
    {
        var query = _context.Despesas.AsQueryable();
        if (!string.IsNullOrWhiteSpace(busca)) query = query.Where(d => d.Descricao.Contains(busca) || d.SequenciaDaDespesa.ToString().Contains(busca));
        return await query.OrderBy(d => d.Descricao).Take(limite).Select(d => new DespesaMvtoContabilItemDto
        {
            SequenciaDaDespesa = d.SequenciaDaDespesa,
            DescricaoDespesa = d.Descricao,
            Quantidade = 1,
            ValorUnitario = d.ValorContabilAtual,
            ValorDeCusto = d.ValorContabilAtual,
            ValorTotal = d.ValorContabilAtual
        }).ToListAsync();
    }

    public async Task AtualizarEstoqueProdutoAsync(int sequenciaDoProduto, decimal novoEstoque)
    {
        var p = await _context.Produtos.FindAsync(sequenciaDoProduto);
        if (p != null) { p.QuantidadeContabil = novoEstoque; await _context.SaveChangesAsync(); }
    }

    public async Task<MovimentoContabilNovoDto> CriarMovimentoAsync(MovimentoContabilNovoDto dto, string usuario)
    {
        int seqMovimento = await _context.MovimentoContabilNovos.AnyAsync() ? await _context.MovimentoContabilNovos.MaxAsync(m => m.SequenciaDoMovimento) + 1 : 1;
        var movimento = new MovimentoContabilNovo
        {
            SequenciaDoMovimento = seqMovimento,
            DataDoMovimento = dto.DataDoMovimento,
            TipoDoMovimento = dto.TipoDoMovimento,
            Documento = dto.Documento,
            SequenciaDoGeral = dto.SequenciaDoGeral,
            Observacao = dto.Observacao ?? "",
            Devolucao = dto.Devolucao,
            ValorDoFrete = 0,
            ValorDoDesconto = 0,
            ValorTotalDosProdutos = dto.ValorTotalDosProdutos,
            ValorTotalDoMovimento = dto.ValorTotalDoMovimento,
            DataDaAlteracao = DateTime.Now,
            HoraDaAlteracao = DateTime.Now,
            UsuarioDaAlteracao = usuario,
            SeqProdPropria = 0,
            EProducaoPropria = false,
            BaixaConsumo = false,
            SeqBaixaConsumo = 0,
            SequenciaGrupoDespesa = 0,
            SequenciaSubGrupoDespesa = 0,
            FormaDePagamento = "",
            Titulo = "",
            Fechado = false,
            SequenciaDaCompra = 0,
            SequenciaDoOrcamento = 0
        };
        _context.MovimentoContabilNovos.Add(movimento);

        foreach (var pDto in dto.Produtos)
        {
            var p = await _context.Produtos.FindAsync(pDto.SequenciaDoProduto);
            if (p == null) continue;
            _context.ProdutosMvtoContabilNovos.Add(new ProdutoMvtoContabilNovo
            {
                SequenciaDoMovimento = seqMovimento,
                SequenciaDoProduto = pDto.SequenciaDoProduto,
                Quantidade = pDto.Quantidade,
                ValorUnitario = pDto.ValorUnitario,
                ValorDeCusto = pDto.ValorDeCusto > 0 ? pDto.ValorDeCusto : p.ValorDeCusto,
                ValorTotal = Math.Round(pDto.ValorTotal, 2),
                ValorDoPis = pDto.ValorDoPis, ValorDoCofins = pDto.ValorDoCofins, ValorDoIpi = pDto.ValorDoIpi, ValorDoIcms = pDto.ValorDoIcms, ValorDoFrete = pDto.ValorDoFrete, ValorDaSubstituicao = pDto.ValorDaSubstituicao
            });
            _context.BaixaDoEstoqueContabils.Add(new BaixaDoEstoqueContabil
            {
                TipoDoMovimento = dto.TipoDoMovimento, DataDoMovimento = dto.DataDoMovimento, Documento = dto.Documento, SequenciaDoGeral = dto.SequenciaDoGeral,
                SequenciaDoProduto = pDto.SequenciaDoProduto, SequenciaDoConjunto = 0, Quantidade = pDto.Quantidade, ValorUnitario = pDto.ValorUnitario,
                ValorDeCusto = pDto.ValorDeCusto > 0 ? pDto.ValorDeCusto : p.ValorDeCusto, ValorTotal = Math.Round(pDto.ValorTotal, 2),
                Observacao = $"Mvto {seqMovimento} | {dto.Observacao}", TipoDoProduto = p.TipoDoProduto, Estoque = "P", SequenciaDoMovimento = seqMovimento,
                SequenciaDoItem2 = 0, ValorDoPis = pDto.ValorDoPis, ValorDoCofins = pDto.ValorDoCofins, ValorDoIcms = pDto.ValorDoIcms, ValorDoIpi = pDto.ValorDoIpi, ValorDoFrete = pDto.ValorDoFrete, ValorDaSubstituicao = pDto.ValorDaSubstituicao, SequenciaDaDespesa = 0
            });
        }

        foreach (var cDto in dto.Conjuntos)
        {
            var c = await _context.Conjuntos.FindAsync(cDto.SequenciaDoConjunto);
            if (c == null) continue;
            _context.ConjuntosMvtoContabilNovos.Add(new ConjuntoMvtoContabilNovo
            {
                SequenciaDoMovimento = seqMovimento, SequenciaConjuntoMvtoNovo = 1, SequenciaDoConjunto = cDto.SequenciaDoConjunto,
                Quantidade = cDto.Quantidade, ValorUnitario = cDto.ValorUnitario, ValorDeCusto = c.ValorTotal, ValorTotal = Math.Round(cDto.ValorTotal, 2)
            });
            _context.BaixaDoEstoqueContabils.Add(new BaixaDoEstoqueContabil
            {
                TipoDoMovimento = dto.TipoDoMovimento, DataDoMovimento = dto.DataDoMovimento, Documento = dto.Documento, SequenciaDoGeral = dto.SequenciaDoGeral,
                SequenciaDoProduto = 0, SequenciaDoConjunto = cDto.SequenciaDoConjunto, Quantidade = cDto.Quantidade, ValorUnitario = cDto.ValorUnitario,
                ValorDeCusto = c.ValorTotal, ValorTotal = Math.Round(cDto.ValorTotal, 2), Observacao = $"Mvto {seqMovimento} (Conj) | {dto.Observacao}",
                TipoDoProduto = 0, Estoque = "C", SequenciaDoMovimento = seqMovimento, SequenciaDoItem2 = 0, SequenciaDaDespesa = 0
            });
        }

        foreach (var dDto in dto.Despesas)
        {
            var d = await _context.Despesas.FindAsync(dDto.SequenciaDaDespesa);
            _context.DespesasMvtoContabilNovos.Add(new DespesaMvtoContabilNovo
            {
                SequenciaDoMovimento = seqMovimento, SequenciaDaDespesa = dDto.SequenciaDaDespesa, Quantidade = dDto.Quantidade,
                ValorUnitario = dDto.ValorUnitario, ValorDeCusto = dDto.ValorDeCusto > 0 ? dDto.ValorDeCusto : (d?.ValorContabilAtual ?? 0),
                ValorTotal = Math.Round(dDto.ValorTotal, 2), ValorTotalCompra = Math.Round(dDto.ValorTotal, 2)
            });
        }

        foreach (var pDto in dto.Parcelas)
        {
            _context.ManutencaoContas.Add(new ManutencaoConta
            {
                SequenciaDoMovimento = seqMovimento, DataDeVencimento = pDto.DataDeVencimento, ValorDaParcela = pDto.ValorDaParcela,
                ValorTotal = pDto.ValorDaParcela, ValorPago = 0, SequenciaDoGeral = dto.SequenciaDoGeral, Documento = dto.Documento,
                Historico = $"Mvto {seqMovimento} | {dto.Observacao}", FormaDePagamento = "", TipoDaConta = dto.TipoDoMovimento == 0 ? "PAGAR" : "RECEBER",
                Conta = "1", Titulo = "", NotasDaCompra = "", TpoDeRecebimento = "", SequenciaDaCobranca = 0, SequenciaDaNotaFiscal = 0,
                SequenciaGrupoDespesa = 0, SequenciaSubGrupoDespesa = 0
            });
        }

        await _context.SaveChangesAsync();
        dto.SequenciaDoMovimento = seqMovimento;
        return dto;
    }

    public async Task<MovimentoContabilNovoDto?> ObterMovimentoAsync(int sequenciaDoMovimento)
    {
        var m = await _context.MovimentoContabilNovos.FirstOrDefaultAsync(x => x.SequenciaDoMovimento == sequenciaDoMovimento);
        if (m == null) return null;
        var g = await _context.Gerals.FindAsync(m.SequenciaDoGeral);
        var dto = new MovimentoContabilNovoDto
        {
            SequenciaDoMovimento = m.SequenciaDoMovimento, DataDoMovimento = m.DataDoMovimento ?? DateTime.MinValue, TipoDoMovimento = m.TipoDoMovimento,
            Documento = m.Documento, SequenciaDoGeral = m.SequenciaDoGeral, RazaoSocialGeral = g?.RazaoSocial, Observacao = m.Observacao,
            Devolucao = m.Devolucao, ValorTotalDosProdutos = m.ValorTotalDosProdutos, ValorTotalDoMovimento = m.ValorTotalDoMovimento
        };
        dto.Produtos = await _context.ProdutosMvtoContabilNovos.Where(p => p.SequenciaDoMovimento == sequenciaDoMovimento).Select(p => new ProdutoMvtoContabilItemDto
        {
            SequenciaDoProdutoMvtoNovo = p.SequenciaDoProdutoMvtoNovo, SequenciaDoProduto = p.SequenciaDoProduto, DescricaoProduto = p.SequenciaDoProdutoNavigation.Descricao,
            Quantidade = p.Quantidade, ValorUnitario = p.ValorUnitario, ValorDeCusto = p.ValorDeCusto, ValorTotal = p.ValorTotal,
            ValorDoPis = p.ValorDoPis, ValorDoCofins = p.ValorDoCofins, ValorDoIpi = p.ValorDoIpi, ValorDoIcms = p.ValorDoIcms, ValorDoFrete = p.ValorDoFrete, ValorDaSubstituicao = p.ValorDaSubstituicao
        }).ToListAsync();
        dto.Conjuntos = await _context.ConjuntosMvtoContabilNovos.Where(c => c.SequenciaDoMovimento == sequenciaDoMovimento).Select(c => new ConjuntoMvtoContabilItemDto
        {
            SequenciaConjuntoMvtoNovo = c.SequenciaConjuntoMvtoNovo, SequenciaDoConjunto = c.SequenciaDoConjunto, DescricaoConjunto = c.SequenciaDoConjuntoNavigation.Descricao,
            Quantidade = c.Quantidade, ValorUnitario = c.ValorUnitario, ValorTotal = c.ValorTotal
        }).ToListAsync();
        dto.Despesas = await _context.DespesasMvtoContabilNovos.Where(d => d.SequenciaDoMovimento == sequenciaDoMovimento).Select(d => new DespesaMvtoContabilItemDto
        {
            SequenciaDespesaMvtoNovo = d.SequenciaDespesaMvtoNovo, SequenciaDaDespesa = d.SequenciaDaDespesa, DescricaoDespesa = d.SequenciaDaDespesaNavigation.Descricao,
            Quantidade = d.Quantidade, ValorUnitario = d.ValorUnitario, ValorDeCusto = d.ValorDeCusto, ValorTotal = d.ValorTotal
        }).ToListAsync();
        dto.Parcelas = await _context.ManutencaoContas.Where(p => p.SequenciaDoMovimento == sequenciaDoMovimento).Select(p => new ParcelaMvtoContabilDto
        {
            DataDeVencimento = p.DataDeVencimento ?? DateTime.MinValue, ValorDaParcela = p.ValorDaParcela
        }).ToListAsync();
        return dto;
    }

    public async Task<PagedResult<MovimentoContabilNovoDto>> ListarMovimentosNovosAsync(MovimentoContabilFiltroDto filtro)
    {
        var query = _context.MovimentoContabilNovos.AsQueryable();
        if (filtro.DataInicial.HasValue) query = query.Where(m => m.DataDoMovimento >= filtro.DataInicial.Value);
        if (filtro.DataFinal.HasValue) query = query.Where(m => m.DataDoMovimento <= filtro.DataFinal.Value);
        if (filtro.SequenciaDoGeral.HasValue) query = query.Where(m => m.SequenciaDoGeral == filtro.SequenciaDoGeral.Value);
        if (!string.IsNullOrWhiteSpace(filtro.Documento)) query = query.Where(m => m.Documento.Contains(filtro.Documento));
        if (filtro.TipoDoMovimento.HasValue) query = query.Where(m => m.TipoDoMovimento == filtro.TipoDoMovimento.Value);

        var total = await query.CountAsync();
        var itens = await query.OrderByDescending(m => m.DataDoMovimento).ThenByDescending(m => m.SequenciaDoMovimento)
            .Skip((filtro.PageNumber - 1) * filtro.PageSize).Take(filtro.PageSize)
            .Select(m => new MovimentoContabilNovoDto
            {
                SequenciaDoMovimento = m.SequenciaDoMovimento, DataDoMovimento = m.DataDoMovimento ?? DateTime.MinValue, TipoDoMovimento = m.TipoDoMovimento,
                Documento = m.Documento, SequenciaDoGeral = m.SequenciaDoGeral, ValorTotalDoMovimento = m.ValorTotalDoMovimento, Devolucao = m.Devolucao
            }).ToListAsync();

        foreach (var item in itens) { var g = await _context.Gerals.FindAsync(item.SequenciaDoGeral); item.RazaoSocialGeral = g?.RazaoSocial; }
        return new PagedResult<MovimentoContabilNovoDto> { Items = itens, TotalCount = total, PageNumber = filtro.PageNumber, PageSize = filtro.PageSize };
    }

    public async Task<bool> ExcluirMovimentoAsync(int sequenciaDoMovimento, string usuario)
    {
        _logger.LogInformation("Excluindo movimento {Id} por {Usuario}", sequenciaDoMovimento, usuario);
        if (await _context.ManutencaoContas.AnyAsync(m => m.SequenciaDoMovimento == sequenciaDoMovimento && m.ValorPago > 0))
            throw new InvalidOperationException("Existem parcelas pagas vinculadas.");
        if (await _context.NotaFiscals.AnyAsync(nf => nf.SequenciaDoMovimento == sequenciaDoMovimento))
            throw new InvalidOperationException("Existe Nota Fiscal vinculada.");

        var itensAfetados = await _context.BaixaDoEstoqueContabils.Where(b => b.SequenciaDoMovimento == sequenciaDoMovimento)
            .Select(b => new { b.SequenciaDoProduto, b.SequenciaDoConjunto }).ToListAsync();

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Baixa do Estoque Contábil] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Produtos Mvto Contábil Novo] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Conjuntos Mvto Contábil Novo] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Despesas Mvto Contábil Novo] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Parcelas mvto contabil] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Manutenção Contas] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Baixa MP Produto] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Baixa MP Conjunto] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Baixa Industrialização] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Linha de Produção] WHERE [Seqüência do Movimento] = {0}", sequenciaDoMovimento);

            var m = await _context.MovimentoContabilNovos.FirstOrDefaultAsync(x => x.SequenciaDoMovimento == sequenciaDoMovimento);
            if (m != null) { _context.MovimentoContabilNovos.Remove(m); await _context.SaveChangesAsync(); }
            await transaction.CommitAsync();

            foreach (var item in itensAfetados)
            {
                if (item.SequenciaDoProduto > 0) await AtualizarEstoqueContabilProdutoAsync(item.SequenciaDoProduto);
                if (item.SequenciaDoConjunto > 0) await AtualizarEstoqueContabilConjuntoAsync(item.SequenciaDoConjunto);
            }
            return true;
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao excluir"); await transaction.RollbackAsync(); throw; }
    }

    public async Task<VerificacaoProducaoResultDto> VerificarViabilidadeProducaoAsync(int id, decimal qtd, bool ehConjunto)
    {
        var res = new VerificacaoProducaoResultDto { SequenciaDoProdutoOuConjunto = id, QuantidadeSolicitada = qtd, EhConjunto = ehConjunto };
        if (ehConjunto)
        {
            var c = await _context.Conjuntos.FindAsync(id); if (c == null) return res; res.DescricaoProduto = c.Descricao;
            var itens = await _context.ItensDoConjuntos.Include(i => i.SequenciaDoProdutoNavigation).Where(i => i.SequenciaDoConjunto == id).ToListAsync();
            await VerificarComponentesRecursivo(itens.Select(i => new ComponenteInfo { SequenciaDoProduto = i.SequenciaDoProduto, Descricao = i.SequenciaDoProdutoNavigation.Descricao, QuantidadeNecessaria = i.QuantidadeDoProduto * qtd, Industrializacao = i.SequenciaDoProdutoNavigation.Industrializacao }).ToList(), res.ComponentesFaltantes, res.PlanoProducaoCascata, 1);
        }
        else
        {
            var p = await _context.Produtos.FindAsync(id); if (p == null) return res; res.DescricaoProduto = p.Descricao;
            var mps = await _context.MateriaPrimas.Include(mp => mp.SequenciaDaMateriaPrimaNavigation).Where(mp => mp.SequenciaDoProduto == id).ToListAsync();
            await VerificarComponentesRecursivo(mps.Select(mp => new ComponenteInfo { SequenciaDoProduto = mp.SequenciaDaMateriaPrima, Descricao = mp.SequenciaDaMateriaPrimaNavigation.Descricao, QuantidadeNecessaria = mp.QuantidadeDeMateriaPrima * qtd, Industrializacao = mp.SequenciaDaMateriaPrimaNavigation.Industrializacao }).ToList(), res.ComponentesFaltantes, res.PlanoProducaoCascata, 1);
        }
        res.PodeProduzir = !res.ComponentesFaltantes.Any();
        res.PlanoProducaoCascata = res.PlanoProducaoCascata.OrderByDescending(p => p.Ordem).Select((p, i) => { p.Ordem = i + 1; return p; }).ToList();
        return res;
    }

    private class ComponenteInfo { public int SequenciaDoProduto { get; set; } public string Descricao { get; set; } = ""; public decimal QuantidadeNecessaria { get; set; } public bool Industrializacao { get; set; } }

    private async Task VerificarComponentesRecursivo(List<ComponenteInfo> comps, List<ComponenteProducaoDto> faltantes, List<ItemPlanoProducaoDto> plano, int nivel, int? dep = null)
    {
        foreach (var c in comps)
        {
            if (c.Industrializacao) continue;
            var est = await GetSaldoEstoqueAsync(c.SequenciaDoProduto);
            var falta = c.QuantidadeNecessaria - est;
            if (falta > 0)
            {
                var temRec = await _context.MateriaPrimas.AnyAsync(mp => mp.SequenciaDoProduto == c.SequenciaDoProduto);
                var dto = new ComponenteProducaoDto { SequenciaDoProduto = c.SequenciaDoProduto, Descricao = c.Descricao, QuantidadeNecessaria = c.QuantidadeNecessaria, EstoqueDisponivel = est, Falta = falta, PodeSerProduzido = temRec, Industrializacao = c.Industrializacao };
                faltantes.Add(dto);
                if (temRec)
                {
                    plano.Add(new ItemPlanoProducaoDto { Ordem = nivel, SequenciaDoProduto = c.SequenciaDoProduto, Descricao = c.Descricao, QuantidadeAProduzir = falta, EhConjunto = false, DependeDe = dep });
                    var sub = await _context.MateriaPrimas.Include(mp => mp.SequenciaDaMateriaPrimaNavigation).Where(mp => mp.SequenciaDoProduto == c.SequenciaDoProduto).ToListAsync();
                    dto.SubComponentes = new List<ComponenteProducaoDto>();
                    await VerificarComponentesRecursivo(sub.Select(mp => new ComponenteInfo { SequenciaDoProduto = mp.SequenciaDaMateriaPrima, Descricao = mp.SequenciaDaMateriaPrimaNavigation.Descricao, QuantidadeNecessaria = mp.QuantidadeDeMateriaPrima * falta, Industrializacao = mp.SequenciaDaMateriaPrimaNavigation.Industrializacao }).ToList(), dto.SubComponentes, plano, nivel + 1, c.SequenciaDoProduto);
                }
            }
        }
    }

    public async Task<ProducaoCascataResultDto> ExecutarProducaoCascataAsync(ProducaoCascataRequestDto req, string user)
    {
        var res = new ProducaoCascataResultDto();
        var ver = await VerificarViabilidadeProducaoAsync(req.SequenciaDoProdutoOuConjunto, req.Quantidade, req.EhConjunto);
        if (!ver.PodeProduzir && !req.ProduzirIntermediarios) { res.Mensagem = ver.Mensagem; return res; }
        using var trans = await _context.Database.BeginTransactionAsync();
        try
        {
            int docNum = 1;
            if (req.ProduzirIntermediarios)
            {
                foreach (var item in ver.PlanoProducaoCascata)
                {
                    var m = await CriarMovimentoProducaoInternoAsync(req.SequenciaDoGeral, $"{req.Documento}-{docNum++:D2}", req.DataDoMovimento, item.SequenciaDoProduto, item.QuantidadeAProduzir, item.EhConjunto, $"Cascata - {req.Observacao}", user);
                    res.MovimentosGerados.Add(new MovimentoGeradoDto { SequenciaDoMovimento = m.SequenciaDoMovimento, Documento = m.Documento, DescricaoProduto = item.Descricao, Quantidade = item.QuantidadeAProduzir });
                }
            }
            var mf = await CriarMovimentoProducaoInternoAsync(req.SequenciaDoGeral, req.Documento, req.DataDoMovimento, req.SequenciaDoProdutoOuConjunto, req.Quantidade, req.EhConjunto, req.Observacao ?? "Produção", user);
            res.MovimentosGerados.Add(new MovimentoGeradoDto { SequenciaDoMovimento = mf.SequenciaDoMovimento, Documento = mf.Documento, DescricaoProduto = ver.DescricaoProduto, Quantidade = req.Quantidade });
            await trans.CommitAsync();
            res.Sucesso = true; res.TotalMovimentos = res.MovimentosGerados.Count; res.Mensagem = "Sucesso";
        }
        catch (Exception ex) { await trans.RollbackAsync(); res.Mensagem = ex.Message; }
        return res;
    }

    private async Task<MovimentoContabilNovo> CriarMovimentoProducaoInternoAsync(int geral, string? doc, DateTime data, int id, decimal qtd, bool ehConj, string? obs, string user)
    {
        var docF = string.IsNullOrWhiteSpace(doc) ? "PRODUCAO" : doc; if (docF.Length > 20) docF = docF.Substring(0, 20);
        var m = new MovimentoContabilNovo
        {
            DataDoMovimento = data, TipoDoMovimento = 0, Documento = docF, SequenciaDoGeral = geral, Observacao = obs ?? "Produção", Devolucao = false,
            UsuarioDaAlteracao = user, DataDaAlteracao = DateTime.Now, HoraDaAlteracao = DateTime.Now, FormaDePagamento = "", Titulo = "", EProducaoPropria = true,
            SeqProdPropria = 0, BaixaConsumo = false, SeqBaixaConsumo = 0, SequenciaGrupoDespesa = 0, SequenciaSubGrupoDespesa = 0,
            ValorDoFrete = 0, ValorDoDesconto = 0, ValorTotalDosProdutos = 0, ValorTotalIpiDosProdutos = 0, ValorTotalDoMovimento = 0, ValorTotalDasDespesas = 0, ValorTotalIpiDasDespesas = 0,
            Fechado = false, SequenciaDaCompra = 0, SequenciaDoOrcamento = 0
        };
        _context.MovimentoContabilNovos.Add(m); await _context.SaveChangesAsync();
        int seq = m.SequenciaDoMovimento;
        if (ehConj)
        {
            var c = await _context.Conjuntos.FindAsync(id); if (c == null) throw new Exception("Conjunto não encontrado");
            _context.ConjuntosMvtoContabilNovos.Add(new ConjuntoMvtoContabilNovo { SequenciaDoMovimento = seq, SequenciaConjuntoMvtoNovo = 1, SequenciaDoConjunto = id, Quantidade = qtd, ValorUnitario = c.ValorTotal, ValorDeCusto = c.ValorTotal, ValorTotal = Math.Round(qtd * c.ValorTotal, 2) });
            _context.BaixaDoEstoqueContabils.Add(new BaixaDoEstoqueContabil { TipoDoMovimento = 0, DataDoMovimento = data, Documento = docF, SequenciaDoGeral = geral, SequenciaDoProduto = 0, SequenciaDoConjunto = id, Quantidade = qtd, ValorUnitario = c.ValorTotal, ValorDeCusto = c.ValorTotal, ValorTotal = Math.Round(qtd * c.ValorTotal, 2), Observacao = obs ?? "Produção", TipoDoProduto = 0, Estoque = "C", SequenciaDoMovimento = seq, SequenciaDoItem2 = 1, SequenciaDaDespesa = 0 });
            var itens = await _context.ItensDoConjuntos.Include(i => i.SequenciaDoProdutoNavigation).Where(i => i.SequenciaDoConjunto == id).ToListAsync();
            foreach (var i in itens)
            {
                if (i.SequenciaDoProdutoNavigation.Industrializacao) continue;
                decimal q = i.QuantidadeDoProduto * qtd;
                _context.BaixaDoEstoqueContabils.Add(new BaixaDoEstoqueContabil { TipoDoMovimento = 1, DataDoMovimento = data, Documento = docF.Length > 15 ? docF.Substring(0, 15) : docF, SequenciaDoGeral = geral, SequenciaDoProduto = i.SequenciaDoProduto, SequenciaDoConjunto = 0, Quantidade = q, ValorUnitario = i.SequenciaDoProdutoNavigation.ValorDeCusto, ValorDeCusto = i.SequenciaDoProdutoNavigation.ValorDeCusto, ValorTotal = Math.Round(q * i.SequenciaDoProdutoNavigation.ValorDeCusto, 2), Observacao = $"Consumo p/ Conj {id}", TipoDoProduto = i.SequenciaDoProdutoNavigation.TipoDoProduto, Estoque = "P", SequenciaDoMovimento = seq, SequenciaDoItem2 = 0, SequenciaDaDespesa = 0 });
            }
            m.ValorTotalDosProdutos = Math.Round(qtd * c.ValorTotal, 2); m.ValorTotalDoMovimento = m.ValorTotalDosProdutos;
        }
        else
        {
            var p = await _context.Produtos.FindAsync(id); if (p == null) throw new Exception("Produto não encontrado");
            _context.ProdutosMvtoContabilNovos.Add(new ProdutoMvtoContabilNovo { SequenciaDoMovimento = seq, SequenciaDoProduto = id, Quantidade = qtd, ValorUnitario = p.ValorDeCusto, ValorDeCusto = p.ValorDeCusto, ValorTotal = Math.Round(qtd * p.ValorDeCusto, 2) });
            _context.BaixaDoEstoqueContabils.Add(new BaixaDoEstoqueContabil { TipoDoMovimento = 0, DataDoMovimento = data, Documento = docF, SequenciaDoGeral = geral, SequenciaDoProduto = id, SequenciaDoConjunto = 0, Quantidade = qtd, ValorUnitario = p.ValorDeCusto, ValorDeCusto = p.ValorDeCusto, ValorTotal = Math.Round(qtd * p.ValorDeCusto, 2), Observacao = obs ?? "Produção", TipoDoProduto = p.TipoDoProduto, Estoque = "P", SequenciaDoMovimento = seq, SequenciaDoItem2 = 0, SequenciaDaDespesa = 0 });
            var mps = await _context.MateriaPrimas.Include(mp => mp.SequenciaDaMateriaPrimaNavigation).Where(mp => mp.SequenciaDoProduto == id).ToListAsync();
            foreach (var mp in mps)
            {
                if (mp.SequenciaDaMateriaPrimaNavigation == null || mp.SequenciaDaMateriaPrimaNavigation.Industrializacao) continue;
                decimal q = mp.QuantidadeDeMateriaPrima * qtd;
                _context.BaixaDoEstoqueContabils.Add(new BaixaDoEstoqueContabil { TipoDoMovimento = 1, DataDoMovimento = data, Documento = docF.Length > 15 ? docF.Substring(0, 15) : docF, SequenciaDoGeral = geral, SequenciaDoProduto = mp.SequenciaDaMateriaPrima, SequenciaDoConjunto = 0, Quantidade = q, ValorUnitario = mp.SequenciaDaMateriaPrimaNavigation.ValorDeCusto, ValorDeCusto = mp.SequenciaDaMateriaPrimaNavigation.ValorDeCusto, ValorTotal = Math.Round(q * mp.SequenciaDaMateriaPrimaNavigation.ValorDeCusto, 2), Observacao = $"Baixa Receita p/ Prod {id}", TipoDoProduto = mp.SequenciaDaMateriaPrimaNavigation.TipoDoProduto, Estoque = "P", SequenciaDoMovimento = seq, SequenciaDoItem2 = 0, SequenciaDaDespesa = 0 });
            }
            m.ValorTotalDosProdutos = Math.Round(qtd * p.ValorDeCusto, 2); m.ValorTotalDoMovimento = m.ValorTotalDosProdutos;
            m.EProducaoPropria = true;
        }
        await _context.SaveChangesAsync(); return m;
    }

    public async Task<decimal> GetSaldoEstoqueAsync(int id) => await _context.BaixaDoEstoqueContabils.Where(b => b.SequenciaDoProduto == id).SumAsync(b => b.Quantidade * (b.TipoDoMovimento == 0 ? 1 : -1));
    public async Task<decimal> GetSaldoEstoqueAsync(int id, DateTime data) => await _context.BaixaDoEstoqueContabils.Where(b => b.SequenciaDoProduto == id && b.DataDoMovimento <= data).SumAsync(b => b.Quantidade * (b.TipoDoMovimento == 0 ? 1 : -1));
    public async Task<decimal> GetSaldoEstoqueConjuntoAsync(int id) => await _context.BaixaDoEstoqueContabils.Where(b => b.SequenciaDoConjunto == id && b.SequenciaDoProduto == 0).SumAsync(b => b.Quantidade * (b.TipoDoMovimento == 0 ? 1 : -1));
}
