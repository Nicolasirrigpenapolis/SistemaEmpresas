using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Controllers.Dashboard;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<DashboardController> _logger;
    private readonly IMemoryCache _cache;
    private const int MaxOrcamentosBuffer = 1500;

    public DashboardController(AppDbContext context, ILogger<DashboardController> logger, IMemoryCache cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    private string BuildCacheKey(string keySuffix)
    {
        if (HttpContext?.Items["Tenant"] is Tenant tenant)
        {
            var tenantKey = !string.IsNullOrWhiteSpace(tenant.Dominio)
                ? tenant.Dominio.ToLowerInvariant()
                : tenant.Id.ToString();

            return $"dashboard:{tenantKey}:{keySuffix}";
        }

        return $"dashboard:default:{keySuffix}";
    }

    /// <summary>
    /// Retorna os KPIs principais do dashboard
    /// </summary>
    [HttpGet("kpis")]
    public async Task<ActionResult<DashboardKpiDto>> GetKpis()
    {
        try
        {
            var cacheKey = BuildCacheKey("kpis:v3");
            if (_cache.TryGetValue(cacheKey, out DashboardKpiDto? cachedKpis))
            {
                return Ok(cachedKpis);
            }

            // Orçamentos Abertos: não fechados e não cancelados (tabela Orcamento)
            var orcamentosAbertos = await _context.Orcamentos
                .AsNoTracking()
                .CountAsync(o => !o.VendaFechada && !o.Cancelado);

            // Compras Pendentes: PedidoFechado = false e não cancelados (tabela Pedido de Compra Novo)
            var comprasPendentes = await _context.PedidoDeCompraNovos
                .AsNoTracking()
                .CountAsync(pc => !pc.PedidoFechado && !pc.Cancelado);

            // Total de Produtos ativos
            var totalProdutos = await _context.Produtos
                .AsNoTracking()
                .CountAsync(p => !p.Inativo);

            // Total de Conjuntos ativos
            var totalConjuntos = await _context.Conjuntos
                .AsNoTracking()
                .CountAsync(c => !c.Inativo);

            // Produtos com estoque crítico
            var produtosEstoqueCritico = await _context.Produtos
                .AsNoTracking()
                .CountAsync(p => !p.Inativo && p.QuantidadeNoEstoque < p.QuantidadeMinima && p.QuantidadeMinima > 0);

            var kpis = new DashboardKpiDto
            {
                OrcamentosAbertos = orcamentosAbertos,
                ComprasPendentesValidacao = comprasPendentes,
                TotalProdutos = totalProdutos,
                TotalConjuntos = totalConjuntos,
                ProdutosEstoqueCritico = produtosEstoqueCritico
            };

            _cache.Set(cacheKey, kpis, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            });

            return Ok(kpis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar KPIs do dashboard");
            return StatusCode(500, new { message = "Erro ao buscar KPIs", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna os últimos orçamentos criados
    /// </summary>
    [HttpGet("orcamentos-recentes")]
    public async Task<ActionResult<IEnumerable<OrcamentoResumidoDto>>> GetOrcamentosRecentes([FromQuery] int limite = 10)
    {
        try
        {
            limite = Math.Clamp(limite, 5, 50);
            var cacheKey = BuildCacheKey($"orcamentos-recentes:{limite}");
            if (_cache.TryGetValue(cacheKey, out List<OrcamentoResumidoDto>? cachedOrcamentos))
            {
                return Ok(cachedOrcamentos);
            }

            var orcamentos = await _context.Orcamentos
                .AsNoTracking()
                .OrderByDescending(o => o.SequenciaDoOrcamento)
                .Take(limite)
                .Select(o => new OrcamentoResumidoDto
                {
                    SequenciaDoOrcamento = o.SequenciaDoOrcamento,
                    DataDeEmissao = o.DataDeEmissao,
                    NomeCliente = o.NomeCliente,
                    NomeVendedor = string.Empty,
                    VendaFechada = o.VendaFechada,
                    Cancelado = o.Cancelado,
                    DiasAberto = o.DataDeEmissao.HasValue
                        ? (int)(DateTime.Now - o.DataDeEmissao.Value).TotalDays
                        : 0
                })
                .ToListAsync();

            _cache.Set(cacheKey, orcamentos, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(45),
                SlidingExpiration = TimeSpan.FromSeconds(15)
            });

            return Ok(orcamentos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar orçamentos recentes");
            return StatusCode(500, new { message = "Erro ao buscar orçamentos recentes", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna orçamentos agrupados por status
    /// </summary>
    [HttpGet("orcamentos-por-status")]
    public async Task<ActionResult<IEnumerable<OrcamentosPorStatusDto>>> GetOrcamentosPorStatus()
    {
        try
        {
            var cacheKey = BuildCacheKey("orcamentos-por-status:v2");
            if (_cache.TryGetValue(cacheKey, out List<OrcamentosPorStatusDto>? cachedStatus))
            {
                return Ok(cachedStatus);
            }

            // Consulta única agrupada para evitar múltiplos COUNT(*) na tabela Orçamento
            var statusData = await _context.Orcamentos
                .AsNoTracking()
                .GroupBy(o => new { o.VendaFechada, o.Cancelado })
                .Select(g => new
                {
                    g.Key.VendaFechada,
                    g.Key.Cancelado,
                    Quantidade = g.Count()
                })
                .ToListAsync();

            var abertos = statusData
                .Where(s => !s.VendaFechada && !s.Cancelado)
                .Sum(s => s.Quantidade);

            var fechados = statusData
                .Where(s => s.VendaFechada)
                .Sum(s => s.Quantidade);

            var cancelados = statusData
                .Where(s => s.Cancelado)
                .Sum(s => s.Quantidade);

            var statusList = new List<OrcamentosPorStatusDto>
            {
                new OrcamentosPorStatusDto { Status = "Aberto", Quantidade = abertos },
                new OrcamentosPorStatusDto { Status = "Fechado", Quantidade = fechados },
                new OrcamentosPorStatusDto { Status = "Cancelado", Quantidade = cancelados }
            };

            _cache.Set(cacheKey, statusList, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            });

            return Ok(statusList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar orçamentos por status");
            return StatusCode(500, new { message = "Erro ao buscar orçamentos por status", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna ordens de serviço agrupadas por status
    /// </summary>
    [HttpGet("ordens-servico-status")]
    public async Task<ActionResult<IEnumerable<OrdensServicoStatusDto>>> GetOrdensServicoStatus()
    {
        try
        {
            var statusList = new List<OrdensServicoStatusDto>();

            var abertas = await _context.OrdemDeServicos
                .AsNoTracking()
                .CountAsync(os => os.Fechamento == 0);

            var fechadas = await _context.OrdemDeServicos
                .AsNoTracking()
                .CountAsync(os => os.Fechamento == 1);

            var garantia = await _context.OrdemDeServicos
                .AsNoTracking()
                .CountAsync(os => os.ServicoEmGarantia);

            statusList.Add(new OrdensServicoStatusDto { Status = "Abertas", Quantidade = abertas });
            statusList.Add(new OrdensServicoStatusDto { Status = "Fechadas", Quantidade = fechadas });
            statusList.Add(new OrdensServicoStatusDto { Status = "Em Garantia", Quantidade = garantia });

            return Ok(statusList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar status das ordens de serviço");
            return StatusCode(500, new { message = "Erro ao buscar status das ordens de serviço", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna produtos com estoque crítico
    /// </summary>
    [HttpGet("estoque-critico")]
    public async Task<ActionResult<IEnumerable<ProdutoEstoqueCriticoDto>>> GetEstoqueCritico([FromQuery] int limite = 20)
    {
        try
        {
            var produtos = await _context.Produtos
                .AsNoTracking()
                .Where(p => !p.Inativo && p.QuantidadeNoEstoque < p.QuantidadeMinima && p.QuantidadeMinima > 0)
                .OrderBy(p => p.QuantidadeNoEstoque - p.QuantidadeMinima)
                .Take(limite)
                .Select(p => new ProdutoEstoqueCriticoDto
                {
                    SequenciaDoProduto = p.SequenciaDoProduto,
                    Descricao = p.Descricao,
                    QuantidadeNoEstoque = p.QuantidadeNoEstoque,
                    QuantidadeMinima = p.QuantidadeMinima,
                    Diferenca = p.QuantidadeNoEstoque - p.QuantidadeMinima,
                    Localizacao = p.Localizacao
                })
                .ToListAsync();

            return Ok(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar produtos com estoque crítico");
            return StatusCode(500, new { message = "Erro ao buscar produtos com estoque crítico", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna alertas operacionais
    /// </summary>
    [HttpGet("alertas")]
    public async Task<ActionResult<IEnumerable<AlertaOperacionalDto>>> GetAlertas()
    {
        try
        {
            var cacheKey = BuildCacheKey("alertas");
            if (_cache.TryGetValue(cacheKey, out List<AlertaOperacionalDto>? cachedAlertas))
            {
                _logger.LogDebug("Retornando alertas do cache para a chave {CacheKey}", cacheKey);
                return Ok(cachedAlertas);
            }

            var alertas = new List<AlertaOperacionalDto>();

            // Alertas de atraso - limitado a 20 para performance máxima
            var itensAtrasados = await _context.SituacaoDosPedidos
                .AsNoTracking()
                .Where(s => s.DiasEmAtraso > 0)
                .Take(20) // Reduzido para 20
                .CountAsync();

            if (itensAtrasados > 0)
            {
                alertas.Add(new AlertaOperacionalDto
                {
                    Tipo = "Atraso",
                    Mensagem = $"{itensAtrasados}+ pedido(s) com atraso na entrega",
                    Quantidade = itensAtrasados,
                    DataReferencia = DateTime.Now
                });
            }

            // Alertas de estoque crítico usando amostragem para evitar full scan
            const int estoqueCriticoSample = 50;
            var estoqueCriticoItens = await _context.Produtos
                .AsNoTracking()
                .Where(p => !p.Inativo && p.QuantidadeNoEstoque < p.QuantidadeMinima && p.QuantidadeMinima > 0)
                .OrderBy(p => p.QuantidadeNoEstoque - p.QuantidadeMinima)
                .Select(p => p.SequenciaDoProduto)
                .Take(estoqueCriticoSample + 1)
                .ToListAsync();

            var estoqueCriticoQuantidade = Math.Min(estoqueCriticoItens.Count, estoqueCriticoSample);
            if (estoqueCriticoQuantidade > 0)
            {
                var sufixoMais = estoqueCriticoItens.Count > estoqueCriticoSample ? "+" : string.Empty;
                alertas.Add(new AlertaOperacionalDto
                {
                    Tipo = "EstoqueCritico",
                    Mensagem = $"{estoqueCriticoQuantidade}{sufixoMais} produto(s) com estoque abaixo do mínimo",
                    Quantidade = estoqueCriticoQuantidade,
                    DataReferencia = DateTime.Now
                });
            }

            // Alertas de compras pendentes
            var comprasPendentes = await _context.PedidoDeCompraNovos
                .AsNoTracking()
                .CountAsync(pc => !pc.Validado && !pc.Cancelado && !pc.PedidoFechado);

            if (comprasPendentes > 0)
            {
                alertas.Add(new AlertaOperacionalDto
                {
                    Tipo = "ComprasPendentes",
                    Mensagem = $"{comprasPendentes} pedido(s) de compra aguardando validação",
                    Quantidade = comprasPendentes,
                    DataReferencia = DateTime.Now
                });
            }

            // Alertas de orçamentos aguardando há mais de 7 dias
            var orcamentosAntigos = await _context.Orcamentos
                .AsNoTracking()
                .Where(o => !o.VendaFechada && !o.Cancelado && o.DataDeEmissao < DateTime.Now.AddDays(-7))
                .CountAsync();

            if (orcamentosAntigos > 0)
            {
                alertas.Add(new AlertaOperacionalDto
                {
                    Tipo = "OrcamentosAntigos",
                    Mensagem = $"{orcamentosAntigos} orçamento(s) em aberto há mais de 7 dias",
                    Quantidade = orcamentosAntigos,
                    DataReferencia = DateTime.Now.AddDays(-7)
                });
            }

            _cache.Set(cacheKey, alertas, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            });

            return Ok(alertas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar alertas operacionais");
            return StatusCode(500, new { message = "Erro ao buscar alertas operacionais", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna orçamentos agrupados por vendedor
    /// </summary>
    [HttpGet("orcamentos-por-vendedor")]
    public async Task<ActionResult<IEnumerable<OrcamentosPorVendedorDto>>> GetOrcamentosPorVendedor([FromQuery] int limite = 10)
    {
        try
        {
            var dataInicio = DateTime.Now.AddMonths(-3); // Últimos 3 meses

            var vendedores = await _context.Orcamentos
                .AsNoTracking()
                .Where(o => o.DataDeEmissao >= dataInicio && !o.Cancelado)
                .GroupBy(o => new { o.SequenciaDoVendedor, o.SequenciaDoVendedorNavigation.NomeFantasia })
                .Select(g => new OrcamentosPorVendedorDto
                {
                    SequenciaDoVendedor = g.Key.SequenciaDoVendedor,
                    NomeVendedor = g.Key.NomeFantasia ?? "Sem vendedor",
                    TotalOrcamentos = g.Count(),
                    OrcamentosFechados = g.Count(o => o.VendaFechada),
                    TaxaConversao = g.Count() > 0 
                        ? Math.Round((decimal)g.Count(o => o.VendaFechada) / g.Count() * 100, 2) 
                        : 0
                })
                .OrderByDescending(v => v.TotalOrcamentos)
                .Take(limite)
                .ToListAsync();

            return Ok(vendedores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar orçamentos por vendedor");
            return StatusCode(500, new { message = "Erro ao buscar orçamentos por vendedor", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna timeline de orçamentos (últimos 30 dias)
    /// </summary>
    [HttpGet("timeline-orcamentos")]
    public async Task<ActionResult<IEnumerable<TimelineOrcamentosDto>>> GetTimelineOrcamentos([FromQuery] int dias = 30)
    {
        try
        {
            dias = Math.Clamp(dias, 7, 90); // Limita entre 7 e 90 dias
            var cacheKey = BuildCacheKey($"timeline-orcamentos:{dias}");
            if (_cache.TryGetValue(cacheKey, out List<TimelineOrcamentosDto>? cachedTimeline))
            {
                return Ok(cachedTimeline);
            }

            var dataInicio = DateTime.Now.AddDays(-dias).Date;

            var timeline = await _context.Orcamentos
                .AsNoTracking()
                .Where(o => o.DataDeEmissao >= dataInicio && !o.Cancelado)
                .GroupBy(o => o.DataDeEmissao!.Value.Date)
                .Select(g => new TimelineOrcamentosDto
                {
                    Periodo = g.Key,
                    QuantidadeOrcamentos = g.Count(),
                    QuantidadePedidos = 0 // Não usado mais
                })
                .OrderBy(x => x.Periodo)
                .ToListAsync();

            _cache.Set(cacheKey, timeline, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });

            return Ok(timeline);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar timeline de orçamentos");
            return StatusCode(500, new { message = "Erro ao buscar timeline de orçamentos", error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna pedidos de compra com atraso
    /// </summary>
    [HttpGet("compras-atrasadas")]
    public async Task<ActionResult<IEnumerable<PedidoCompraResumidoDto>>> GetComprasAtrasadas([FromQuery] int limite = 10)
    {
        try
        {
            var hoje = DateTime.Now.Date;

            var compras = await _context.PedidoDeCompraNovos
                .AsNoTracking()
                .Where(pc => !pc.PedidoFechado && !pc.Cancelado && pc.PrevisaoDeEntrega < hoje)
                .OrderBy(pc => pc.PrevisaoDeEntrega)
                .Take(limite)
                .Select(pc => new PedidoCompraResumidoDto
                {
                    IdDoPedido = pc.IdDoPedido,
                    DataDoPedido = pc.DataDoPedido,
                    NomeFornecedor = "Fornecedor", // Precisaria fazer join com tabela Geral
                    PrevisaoDeEntrega = pc.PrevisaoDeEntrega,
                    PedidoFechado = pc.PedidoFechado,
                    Validado = pc.Validado,
                    Cancelado = pc.Cancelado,
                    DiasAtraso = pc.PrevisaoDeEntrega.HasValue 
                        ? (int)(hoje - pc.PrevisaoDeEntrega.Value).TotalDays 
                        : 0
                })
                .ToListAsync();

            return Ok(compras);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar compras atrasadas");
            return StatusCode(500, new { message = "Erro ao buscar compras atrasadas", error = ex.Message });
        }
    }
}
