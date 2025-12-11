using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories;

public class NotaFiscalRepository : INotaFiscalRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<NotaFiscalRepository> _logger;

    public NotaFiscalRepository(AppDbContext context, ILogger<NotaFiscalRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Listagem

    public async Task<PagedResult<NotaFiscalListDto>> ListarAsync(NotaFiscalFiltroDto filtro)
    {
        var query = _context.NotaFiscals
            .Include(n => n.SequenciaDoGeralNavigation)
            .Include(n => n.SequenciaDaNaturezaNavigation)
            .Include(n => n.SequenciaDaPropriedadeNavigation)
            .AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(filtro.Busca))
        {
            var busca = filtro.Busca.ToLower();
            query = query.Where(n =>
                n.NumeroDaNfe.ToString().Contains(busca) ||
                n.NumeroDaNotaFiscal.ToString().Contains(busca) ||
                n.SequenciaDoGeralNavigation.RazaoSocial.ToLower().Contains(busca) ||
                n.SequenciaDoGeralNavigation.NomeFantasia.ToLower().Contains(busca) ||
                n.ChaveDeAcessoDaNfe.Contains(busca));
        }

        if (filtro.DataInicial.HasValue)
            query = query.Where(n => n.DataDeEmissao >= filtro.DataInicial.Value);

        if (filtro.DataFinal.HasValue)
            query = query.Where(n => n.DataDeEmissao <= filtro.DataFinal.Value.AddDays(1));

        if (filtro.Cliente.HasValue)
            query = query.Where(n => n.SequenciaDoGeral == filtro.Cliente.Value);

        if (filtro.Natureza.HasValue)
            query = query.Where(n => n.SequenciaDaNatureza == filtro.Natureza.Value);

        if (filtro.Propriedade.HasValue)
            query = query.Where(n => n.SequenciaDaPropriedade == filtro.Propriedade.Value);

        if (filtro.TipoDeNota.HasValue)
            query = query.Where(n => n.TipoDeNota == filtro.TipoDeNota.Value);

        if (filtro.Canceladas.HasValue)
            query = query.Where(n => n.NotaCancelada == filtro.Canceladas.Value);

        if (filtro.Transmitidas.HasValue)
            query = query.Where(n => n.Transmitido == filtro.Transmitidas.Value);

        if (filtro.Autorizadas.HasValue)
            query = query.Where(n => n.Autorizado == filtro.Autorizadas.Value);

        if (filtro.NumeroDaNfe.HasValue)
            query = query.Where(n => n.NumeroDaNfe == filtro.NumeroDaNfe.Value);

        // Contagem total
        var totalCount = await query.CountAsync();

        // Ordenação (mais recentes primeiro)
        query = query.OrderByDescending(n => n.DataDeEmissao)
                     .ThenByDescending(n => n.SequenciaDaNotaFiscal);

        // Paginação
        var items = await query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(n => new NotaFiscalListDto
            {
                SequenciaDaNotaFiscal = n.SequenciaDaNotaFiscal,
                NumeroDaNfe = n.NumeroDaNfe,
                NumeroDaNotaFiscal = n.NumeroDaNotaFiscal,
                DataDeEmissao = n.DataDeEmissao,
                
                SequenciaDoGeral = n.SequenciaDoGeral,
                NomeDoCliente = n.SequenciaDoGeralNavigation.RazaoSocial,
                DocumentoCliente = n.SequenciaDoGeralNavigation.CpfECnpj,
                
                SequenciaDaNatureza = n.SequenciaDaNatureza,
                DescricaoNatureza = n.SequenciaDaNaturezaNavigation.DescricaoDaNaturezaOperacao,
                
                SequenciaDaPropriedade = n.SequenciaDaPropriedade,
                NomePropriedade = n.SequenciaDaPropriedadeNavigation.NomeDaPropriedade,
                
                ValorTotalDaNotaFiscal = n.ValorTotalDaNotaFiscal,
                ValorTotalDosProdutos = n.ValorTotalDosProdutos,
                ValorTotalDoIcms = n.ValorTotalDoIcms,
                
                NotaCancelada = n.NotaCancelada,
                Transmitido = n.Transmitido,
                Autorizado = n.Autorizado,
                ChaveDeAcessoDaNfe = n.ChaveDeAcessoDaNfe,
                
                TipoDeNota = n.TipoDeNota,
                NfeComplementar = n.NfeComplementar,
                NotaDeDevolucao = n.NotaDeDevolucao
            })
            .ToListAsync();

        return new PagedResult<NotaFiscalListDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize
        };
    }

    #endregion

    #region Obter por ID

    public async Task<NotaFiscalDto?> ObterPorIdAsync(int id)
    {
        var nota = await _context.NotaFiscals
            .Include(n => n.SequenciaDoGeralNavigation)
                .ThenInclude(g => g.SequenciaDoMunicipioNavigation)
            .Include(n => n.SequenciaDaNaturezaNavigation)
            .Include(n => n.SequenciaDaPropriedadeNavigation)
            .Include(n => n.SequenciaDaClassificacaoNavigation)
            .Include(n => n.SequenciaDaCobrancaNavigation)
            .Include(n => n.SequenciaDaTransportadoraNavigation)
            .Include(n => n.MunicipioDaTransportadoraNavigation)
            .Include(n => n.SequenciaDoVendedorNavigation)
            .Include(n => n.ProdutosDaNotaFiscals)
                .ThenInclude(p => p.SequenciaDoProdutoNavigation)
            .Include(n => n.ConjuntosDaNotaFiscals)
                .ThenInclude(c => c.SequenciaDoConjuntoNavigation)
            .Include(n => n.PecasDaNotaFiscals)
                .ThenInclude(p => p.SequenciaDoProdutoNavigation)
            .Include(n => n.ParcelasNotaFiscals)
            .FirstOrDefaultAsync(n => n.SequenciaDaNotaFiscal == id);

        if (nota == null)
            return null;

        var cliente = nota.SequenciaDoGeralNavigation;
        var municipioCliente = cliente?.SequenciaDoMunicipioNavigation;

        return new NotaFiscalDto
        {
            // Identificação
            SequenciaDaNotaFiscal = nota.SequenciaDaNotaFiscal,
            NumeroDaNfe = nota.NumeroDaNfe,
            NumeroDaNfse = nota.NumeroDaNfse,
            NumeroDaNotaFiscal = nota.NumeroDaNotaFiscal,
            
            // Datas
            DataDeEmissao = nota.DataDeEmissao,
            DataDeSaida = nota.DataDeSaida,
            HoraDaSaida = nota.HoraDaSaida,
            
            // Cliente
            SequenciaDoGeral = nota.SequenciaDoGeral,
            NomeDoCliente = cliente?.RazaoSocial ?? "",
            DocumentoCliente = cliente?.CpfECnpj ?? "",
            EnderecoCliente = cliente != null ? $"{cliente.Endereco}, {cliente.NumeroDoEndereco}" : "",
            CidadeCliente = municipioCliente?.Descricao ?? "",
            UfCliente = municipioCliente?.Uf ?? "",
            InscricaoCliente = cliente?.RgEIe ?? "",
            
            // Propriedade
            SequenciaDaPropriedade = nota.SequenciaDaPropriedade,
            NomePropriedade = nota.SequenciaDaPropriedadeNavigation?.NomeDaPropriedade ?? "",
            
            // Natureza
            SequenciaDaNatureza = nota.SequenciaDaNatureza,
            DescricaoNatureza = nota.SequenciaDaNaturezaNavigation?.DescricaoDaNaturezaOperacao ?? "",
            CfopNatureza = nota.SequenciaDaNaturezaNavigation?.CodigoDaNaturezaDeOperacao.ToString() ?? "",
            
            // Classificação
            SequenciaDaClassificacao = nota.SequenciaDaClassificacao,
            DescricaoClassificacao = nota.SequenciaDaClassificacaoNavigation?.DescricaoDoNcm ?? "",
            
            // Cobrança
            SequenciaDaCobranca = nota.SequenciaDaCobranca,
            DescricaoCobranca = nota.SequenciaDaCobrancaNavigation?.Descricao ?? "",
            
            // Transportadora
            TransportadoraAvulsa = nota.TransportadoraAvulsa,
            SequenciaDaTransportadora = nota.SequenciaDaTransportadora,
            NomeTransportadora = nota.SequenciaDaTransportadoraNavigation?.RazaoSocial ?? "",
            NomeDaTransportadoraAvulsa = nota.NomeDaTransportadoraAvulsa,
            DocumentoDaTransportadora = nota.DocumentoDaTransportadora,
            IeDaTransportadora = nota.IeDaTransportadora,
            EnderecoDaTransportadora = nota.EnderecoDaTransportadora,
            MunicipioDaTransportadora = nota.MunicipioDaTransportadora,
            NomeMunicipioTransportadora = nota.MunicipioDaTransportadoraNavigation?.Descricao ?? "",
            PlacaDoVeiculo = nota.PlacaDoVeiculo,
            UfDoVeiculo = nota.UfDoVeiculo,
            CodigoDaAntt = nota.CodigoDaAntt,
            
            // Frete
            Frete = nota.Frete,
            ValorDoFrete = nota.ValorDoFrete,
            
            // Volumes
            Volume = nota.Volume,
            Especie = nota.Especie,
            Marca = nota.Marca,
            Numeracao = nota.Numeracao,
            PesoBruto = nota.PesoBruto,
            PesoLiquido = nota.PesoLiquido,
            
            // Valores Totais
            ValorTotalDosProdutos = nota.ValorTotalDosProdutos,
            ValorTotalDosConjuntos = nota.ValorTotalDosConjuntos,
            ValorTotalDasPecas = nota.ValorTotalDasPecas,
            ValorTotalDosServicos = nota.ValorTotalDosServicos,
            ValorTotalDaNotaFiscal = nota.ValorTotalDaNotaFiscal,
            
            // Impostos
            ValorTotalDaBaseDeCalculo = nota.ValorTotalDaBaseDeCalculo,
            ValorTotalDoIcms = nota.ValorTotalDoIcms,
            ValorTotalDaBaseSt = nota.ValorTotalDaBaseSt,
            ValorTotalDoIcmsSt = nota.ValorTotalDoIcmsSt,
            ValorTotalIpiDosProdutos = nota.ValorTotalIpiDosProdutos,
            ValorTotalIpiDosConjuntos = nota.ValorTotalIpiDosConjuntos,
            ValorTotalIpiDasPecas = nota.ValorTotalIpiDasPecas,
            ValorTotalDoPis = nota.ValorTotalDoPis,
            ValorTotalDoCofins = nota.ValorTotalDoCofins,
            ValorDoImpostoDeRenda = nota.ValorDoImpostoDeRenda,
            AliquotaDoIss = nota.AliquotaDoIss,
            ReterIss = nota.ReterIss,
            ValorTotalDoTributo = nota.ValorTotalDoTributo,
            ValorTotalDaImportacao = nota.ValorTotalDaImportacao,
            ValorTotalIbs = nota.ValorTotalIbs,
            ValorTotalCbs = nota.ValorTotalCbs,
            
            // Outros Valores
            ValorDoSeguro = nota.ValorDoSeguro,
            OutrasDespesas = nota.OutrasDespesas,
            
            // Valores Usados
            ValorTotalDeProdutosUsados = nota.ValorTotalDeProdutosUsados,
            ValorTotalConjuntosUsados = nota.ValorTotalConjuntosUsados,
            ValorTotalDasPecasUsadas = nota.ValorTotalDasPecasUsadas,
            
            // Pagamento
            FormaDePagamento = nota.FormaDePagamento,
            ContraApresentacao = nota.ContraApresentacao,
            Fechamento = nota.Fechamento,
            ValorDoFechamento = nota.ValorDoFechamento,
            
            // Observações
            Historico = nota.Historico,
            Observacao = nota.Observacao,
            
            // Tipo e Status
            TipoDeNota = nota.TipoDeNota,
            NotaCancelada = nota.NotaCancelada,
            CanceladaNoLivro = nota.CanceladaNoLivro,
            NotaFiscalAvulsa = nota.NotaFiscalAvulsa,
            OcultarValorUnitario = nota.OcultarValorUnitario,
            
            // NFe
            Transmitido = nota.Transmitido,
            Autorizado = nota.Autorizado,
            Imprimiu = nota.Imprimiu,
            ChaveDeAcessoDaNfe = nota.ChaveDeAcessoDaNfe,
            ProtocoloDeAutorizacaoNfe = nota.ProtocoloDeAutorizacaoNfe,
            DataEHoraDaNfe = nota.DataEHoraDaNfe,
            NumeroDoReciboDaNfe = nota.NumeroDoReciboDaNfe,
            
            // NFe Referenciada/Complementar/Devolução
            NfeComplementar = nota.NfeComplementar,
            ChaveAcessoNfeReferenciada = nota.ChaveAcessoNfeReferenciada,
            NotaDeDevolucao = nota.NotaDeDevolucao,
            ChaveDaDevolucao = nota.ChaveDaDevolucao,
            ChaveDaDevolucao2 = nota.ChaveDaDevolucao2,
            ChaveDaDevolucao3 = nota.ChaveDaDevolucao3,
            FinNfe = nota.FinNfe,
            NovoLayout = nota.NovoLayout,
            
            // NFSe
            ReciboNfse = nota.ReciboNfse,
            
            // Conjunto Avulso
            ConjuntoAvulso = nota.ConjuntoAvulso,
            DescricaoConjuntoAvulso = nota.DescricaoConjuntoAvulso,
            
            // Relacionamentos
            SequenciaDoPedido = nota.SequenciaDoPedido,
            SequenciaDoVendedor = nota.SequenciaDoVendedor,
            NomeVendedor = nota.SequenciaDoVendedorNavigation?.RazaoSocial ?? "",
            SequenciaDoMovimento = nota.SequenciaDoMovimento,
            NumeroDoContrato = nota.NumeroDoContrato,
            NotaDeVenda = nota.NotaDeVenda,
            Refaturamento = nota.Refaturamento,
            Financiamento = nota.Financiamento,
            
            // Itens da nota fiscal
            Produtos = nota.ProdutosDaNotaFiscals.Select(p => new ProdutoDaNotaFiscalDto
            {
                SequenciaDoProdutoDaNotaFiscal = p.SequenciaProdutoNotaFiscal,
                SequenciaDoProduto = p.SequenciaDoProduto,
                DescricaoProduto = p.SequenciaDoProdutoNavigation?.Descricao ?? "",
                Unidade = "", // Não carregado para evitar erros
                Ncm = "", // Não carregado para evitar erros
                Cfop = p.Cfop.ToString(),
                Cst = p.Cst.ToString(),
                Quantidade = p.Quantidade,
                ValorUnitario = p.ValorUnitario,
                ValorTotal = p.ValorTotal,
                Desconto = p.ValorDoDesconto,
                BaseDeCalculoIcms = p.ValorDaBaseDeCalculo,
                AliquotaIcms = p.AliquotaDoIcms,
                ValorIcms = p.ValorDoIcms,
                BaseDeCalculoSt = p.BaseDeCalculoSt,
                AliquotaSt = p.AliquotaDoIcmsSt,
                ValorIcmsSt = p.ValorIcmsSt,
                BaseDeCalculoIpi = 0, // Não existe na entidade
                AliquotaIpi = p.AliquotaDoIpi,
                ValorIpi = p.ValorDoIpi,
                AliquotaPis = p.AliqDoPis,
                ValorPis = p.ValorDoPis,
                AliquotaCofins = p.AliqDoCofins,
                ValorCofins = p.ValorDoCofins,
                ValorIbs = p.ValorIbs,
                ValorCbs = p.ValorCbs,
                Usado = false
            }).ToList(),
            
            Conjuntos = nota.ConjuntosDaNotaFiscals.Select(c => new ConjuntoDaNotaFiscalDto
            {
                SequenciaDoConjuntoDaNotaFiscal = c.SequenciaConjuntoNotaFiscal,
                SequenciaDoConjunto = c.SequenciaDoConjunto,
                DescricaoConjunto = c.SequenciaDoConjuntoNavigation?.Descricao ?? "",
                Quantidade = c.Quantidade,
                ValorUnitario = c.ValorUnitario,
                ValorTotal = c.ValorTotal,
                ValorIpi = c.ValorDoIpi,
                ValorIbs = c.ValorIbs,
                ValorCbs = c.ValorCbs,
                Usado = false
            }).ToList(),
            
            Pecas = nota.PecasDaNotaFiscals.Select(p => new PecaDaNotaFiscalDto
            {
                SequenciaDaPecaDaNotaFiscal = p.SequenciaDaPecaNotaFiscal,
                SequenciaDaPeca = p.SequenciaDoProduto, // Peças usam SequenciaDoProduto
                DescricaoPeca = p.SequenciaDoProdutoNavigation?.Descricao ?? "",
                Quantidade = p.Quantidade,
                ValorUnitario = p.ValorUnitario,
                ValorTotal = p.ValorTotal,
                ValorIpi = p.ValorDoIpi,
                ValorIbs = p.ValorIbs,
                ValorCbs = p.ValorCbs,
                Usado = false
            }).ToList(),
            
            Servicos = new List<ServicoDaNotaFiscalDto>(), // Não usado
            
            Parcelas = nota.ParcelasNotaFiscals.Select(p => new ParcelaNotaFiscalDto
            {
                NumeroDaParcela = p.NumeroDaParcela,
                Dias = p.Dias,
                DataDeVencimento = p.DataDeVencimento,
                Valor = p.ValorDaParcela
            }).ToList()
        };
    }

    #endregion

    #region Criar

    public async Task<NotaFiscal> CriarAsync(NotaFiscalCreateUpdateDto dto, string usuario)
    {
        var proximoNumero = await ObterProximoNumeroAsync(dto.SequenciaDaPropriedade);
        
        var nota = new NotaFiscal
        {
            NumeroDaNotaFiscal = proximoNumero,
            NumeroDaNfe = 0, // Será preenchido na transmissão
            NumeroDaNfse = 0,
            DataDeEmissao = dto.DataDeEmissao,
            DataDeSaida = dto.DataDeSaida,
            HoraDaSaida = dto.HoraDaSaida,
            
            SequenciaDoGeral = dto.SequenciaDoGeral,
            SequenciaDaPropriedade = dto.SequenciaDaPropriedade,
            SequenciaDaNatureza = dto.SequenciaDaNatureza,
            SequenciaDaClassificacao = dto.SequenciaDaClassificacao,
            SequenciaDaCobranca = dto.SequenciaDaCobranca,
            
            TransportadoraAvulsa = dto.TransportadoraAvulsa,
            SequenciaDaTransportadora = dto.SequenciaDaTransportadora,
            NomeDaTransportadoraAvulsa = dto.NomeDaTransportadoraAvulsa ?? "",
            DocumentoDaTransportadora = dto.DocumentoDaTransportadora ?? "",
            IeDaTransportadora = dto.IeDaTransportadora ?? "",
            EnderecoDaTransportadora = dto.EnderecoDaTransportadora ?? "",
            MunicipioDaTransportadora = dto.MunicipioDaTransportadora,
            PlacaDoVeiculo = dto.PlacaDoVeiculo ?? "",
            UfDoVeiculo = dto.UfDoVeiculo ?? "",
            CodigoDaAntt = dto.CodigoDaAntt ?? "",
            
            Frete = dto.Frete,
            ValorDoFrete = dto.ValorDoFrete,
            
            Volume = dto.Volume,
            Especie = dto.Especie ?? "",
            Marca = dto.Marca ?? "",
            Numeracao = dto.Numeracao ?? "",
            PesoBruto = dto.PesoBruto,
            PesoLiquido = dto.PesoLiquido,
            
            FormaDePagamento = dto.FormaDePagamento ?? "",
            ContraApresentacao = dto.ContraApresentacao,
            Fechamento = dto.Fechamento,
            ValorDoFechamento = dto.ValorDoFechamento,
            
            Historico = dto.Historico ?? "",
            Observacao = dto.Observacao ?? "",
            
            TipoDeNota = dto.TipoDeNota,
            NotaFiscalAvulsa = dto.NotaFiscalAvulsa,
            OcultarValorUnitario = dto.OcultarValorUnitario,
            
            ValorDoSeguro = dto.ValorDoSeguro,
            OutrasDespesas = dto.OutrasDespesas,
            
            NfeComplementar = dto.NfeComplementar,
            ChaveAcessoNfeReferenciada = dto.ChaveAcessoNfeReferenciada ?? "",
            NotaDeDevolucao = dto.NotaDeDevolucao,
            ChaveDaDevolucao = dto.ChaveDaDevolucao ?? "",
            ChaveDaDevolucao2 = dto.ChaveDaDevolucao2 ?? "",
            ChaveDaDevolucao3 = dto.ChaveDaDevolucao3 ?? "",
            
            ConjuntoAvulso = dto.ConjuntoAvulso,
            DescricaoConjuntoAvulso = dto.DescricaoConjuntoAvulso ?? "",
            
            SequenciaDoPedido = dto.SequenciaDoPedido,
            SequenciaDoVendedor = dto.SequenciaDoVendedor,
            NumeroDoContrato = dto.NumeroDoContrato,
            Refaturamento = dto.Refaturamento,
            Financiamento = dto.Financiamento,
            
            // Valores iniciais zerados (serão calculados ao adicionar itens)
            ValorTotalDosProdutos = 0,
            ValorTotalDosConjuntos = 0,
            ValorTotalDasPecas = 0,
            ValorTotalDosServicos = 0,
            ValorTotalDaNotaFiscal = 0,
            ValorTotalDaBaseDeCalculo = 0,
            ValorTotalDoIcms = 0,
            ValorTotalDaBaseSt = 0,
            ValorTotalDoIcmsSt = 0,
            ValorTotalIpiDosProdutos = 0,
            ValorTotalIpiDosConjuntos = 0,
            ValorTotalIpiDasPecas = 0,
            ValorTotalDoPis = 0,
            ValorTotalDoCofins = 0,
            ValorDoImpostoDeRenda = 0,
            AliquotaDoIss = 0,
            ReterIss = false,
            ValorTotalDoTributo = 0,
            ValorTotalDaImportacao = 0,
            ValorTotalDeProdutosUsados = 0,
            ValorTotalConjuntosUsados = 0,
            ValorTotalDasPecasUsadas = 0,
            
            // Status inicial
            NotaCancelada = false,
            CanceladaNoLivro = false,
            Transmitido = false,
            Autorizado = false,
            Imprimiu = false,
            ChaveDeAcessoDaNfe = "",
            ProtocoloDeAutorizacaoNfe = "",
            DataEHoraDaNfe = "",
            NumeroDoReciboDaNfe = "",
            ReciboNfse = "",
            FinNfe = 1, // 1 = Normal
            NovoLayout = true,
            SequenciaDoMovimento = 0,
            NotaDeVenda = 0
        };

        _context.NotaFiscals.Add(nota);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Nota Fiscal {Id} criada por {Usuario}", nota.SequenciaDaNotaFiscal, usuario);
        return nota;
    }

    #endregion

    #region Atualizar

    public async Task<NotaFiscal?> AtualizarAsync(int id, NotaFiscalCreateUpdateDto dto, string usuario)
    {
        var nota = await _context.NotaFiscals.FindAsync(id);
        
        if (nota == null)
            return null;

        // Verificar se pode editar
        if (nota.Transmitido || nota.Autorizado)
        {
            _logger.LogWarning("Tentativa de editar nota fiscal {Id} já transmitida/autorizada", id);
            throw new InvalidOperationException("Não é possível editar uma nota fiscal já transmitida ou autorizada");
        }

        // Atualizar campos
        nota.DataDeEmissao = dto.DataDeEmissao;
        nota.DataDeSaida = dto.DataDeSaida;
        nota.HoraDaSaida = dto.HoraDaSaida;
        
        nota.SequenciaDoGeral = dto.SequenciaDoGeral;
        nota.SequenciaDaPropriedade = dto.SequenciaDaPropriedade;
        nota.SequenciaDaNatureza = dto.SequenciaDaNatureza;
        nota.SequenciaDaClassificacao = dto.SequenciaDaClassificacao;
        nota.SequenciaDaCobranca = dto.SequenciaDaCobranca;
        
        nota.TransportadoraAvulsa = dto.TransportadoraAvulsa;
        nota.SequenciaDaTransportadora = dto.SequenciaDaTransportadora;
        nota.NomeDaTransportadoraAvulsa = dto.NomeDaTransportadoraAvulsa ?? "";
        nota.DocumentoDaTransportadora = dto.DocumentoDaTransportadora ?? "";
        nota.IeDaTransportadora = dto.IeDaTransportadora ?? "";
        nota.EnderecoDaTransportadora = dto.EnderecoDaTransportadora ?? "";
        nota.MunicipioDaTransportadora = dto.MunicipioDaTransportadora;
        nota.PlacaDoVeiculo = dto.PlacaDoVeiculo ?? "";
        nota.UfDoVeiculo = dto.UfDoVeiculo ?? "";
        nota.CodigoDaAntt = dto.CodigoDaAntt ?? "";
        
        nota.Frete = dto.Frete;
        nota.ValorDoFrete = dto.ValorDoFrete;
        
        nota.Volume = dto.Volume;
        nota.Especie = dto.Especie ?? "";
        nota.Marca = dto.Marca ?? "";
        nota.Numeracao = dto.Numeracao ?? "";
        nota.PesoBruto = dto.PesoBruto;
        nota.PesoLiquido = dto.PesoLiquido;
        
        nota.FormaDePagamento = dto.FormaDePagamento ?? "";
        nota.ContraApresentacao = dto.ContraApresentacao;
        nota.Fechamento = dto.Fechamento;
        nota.ValorDoFechamento = dto.ValorDoFechamento;
        
        nota.Historico = dto.Historico ?? "";
        nota.Observacao = dto.Observacao ?? "";
        
        nota.TipoDeNota = dto.TipoDeNota;
        nota.NotaFiscalAvulsa = dto.NotaFiscalAvulsa;
        nota.OcultarValorUnitario = dto.OcultarValorUnitario;
        
        nota.ValorDoSeguro = dto.ValorDoSeguro;
        nota.OutrasDespesas = dto.OutrasDespesas;
        
        nota.NfeComplementar = dto.NfeComplementar;
        nota.ChaveAcessoNfeReferenciada = dto.ChaveAcessoNfeReferenciada ?? "";
        nota.NotaDeDevolucao = dto.NotaDeDevolucao;
        nota.ChaveDaDevolucao = dto.ChaveDaDevolucao ?? "";
        nota.ChaveDaDevolucao2 = dto.ChaveDaDevolucao2 ?? "";
        nota.ChaveDaDevolucao3 = dto.ChaveDaDevolucao3 ?? "";
        
        nota.ConjuntoAvulso = dto.ConjuntoAvulso;
        nota.DescricaoConjuntoAvulso = dto.DescricaoConjuntoAvulso ?? "";
        
        nota.SequenciaDoPedido = dto.SequenciaDoPedido;
        nota.SequenciaDoVendedor = dto.SequenciaDoVendedor;
        nota.NumeroDoContrato = dto.NumeroDoContrato;
        nota.Refaturamento = dto.Refaturamento;
        nota.Financiamento = dto.Financiamento;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Nota Fiscal {Id} atualizada por {Usuario}", id, usuario);
        return nota;
    }

    #endregion

    #region Cancelar

    public async Task<bool> CancelarAsync(int id, string justificativa, string usuario)
    {
        var nota = await _context.NotaFiscals.FindAsync(id);
        
        if (nota == null)
            return false;

        nota.NotaCancelada = true;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Nota Fiscal {Id} cancelada por {Usuario}. Justificativa: {Justificativa}", 
            id, usuario, justificativa);
        return true;
    }

    #endregion

    #region Utilitários

    public async Task<int> ObterProximoNumeroAsync(short propriedade)
    {
        var ultimoNumero = await _context.NotaFiscals
            .Where(n => n.SequenciaDaPropriedade == propriedade)
            .MaxAsync(n => (int?)n.NumeroDaNotaFiscal) ?? 0;

        return ultimoNumero + 1;
    }

    public async Task<bool> PodeEditarAsync(int id)
    {
        var nota = await _context.NotaFiscals
            .Where(n => n.SequenciaDaNotaFiscal == id)
            .Select(n => new { n.Transmitido, n.Autorizado, n.NotaCancelada })
            .FirstOrDefaultAsync();

        if (nota == null)
            return false;

        return !nota.Transmitido && !nota.Autorizado && !nota.NotaCancelada;
    }

    #endregion

    #region Combos

    public async Task<List<ClienteComboDto>> ListarClientesComboAsync(string? busca)
    {
        var query = _context.Gerals
            .Where(g => g.Cliente)
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var buscaLower = busca.ToLower();
            query = query.Where(g =>
                g.RazaoSocial.ToLower().Contains(buscaLower) ||
                g.NomeFantasia.ToLower().Contains(buscaLower) ||
                g.CpfECnpj.Contains(busca));
        }

        return await query
            .OrderBy(g => g.RazaoSocial)
            .Take(50)
            .Select(g => new ClienteComboDto
            {
                SequenciaDoGeral = g.SequenciaDoGeral,
                Nome = g.RazaoSocial,
                Documento = g.CpfECnpj,
                Cidade = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Descricao : "",
                Uf = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Uf : ""
            })
            .ToListAsync();
    }

    public async Task<List<TransportadoraComboDto>> ListarTransportadorasComboAsync(string? busca)
    {
        var query = _context.Gerals
            .Where(g => g.Transportadora)
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var buscaLower = busca.ToLower();
            query = query.Where(g =>
                g.RazaoSocial.ToLower().Contains(buscaLower) ||
                g.NomeFantasia.ToLower().Contains(buscaLower) ||
                g.CpfECnpj.Contains(busca));
        }

        return await query
            .OrderBy(g => g.RazaoSocial)
            .Take(50)
            .Select(g => new TransportadoraComboDto
            {
                SequenciaDoGeral = g.SequenciaDoGeral,
                Nome = g.RazaoSocial,
                Documento = g.CpfECnpj,
                Cidade = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Descricao : "",
                Uf = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Uf : ""
            })
            .ToListAsync();
    }

    public async Task<List<NaturezaOperacaoComboDto>> ListarNaturezasComboAsync(bool? entradaSaida = null)
    {
        var query = _context.NaturezaDeOperacaos
            .Where(n => !n.Inativo)
            .AsQueryable();

        return await query
            .OrderBy(n => n.DescricaoDaNaturezaOperacao)
            .Select(n => new NaturezaOperacaoComboDto
            {
                SequenciaDaNatureza = n.SequenciaDaNatureza,
                Descricao = n.DescricaoDaNaturezaOperacao,
                Cfop = n.CodigoDaNaturezaDeOperacao.ToString(),
                EntradaSaida = false // TODO: Verificar campo correto
            })
            .ToListAsync();
    }

    public async Task<List<PropriedadeComboDto>> ListarPropriedadesComboAsync()
    {
        return await _context.Propriedades
            .OrderBy(p => p.NomeDaPropriedade)
            .Select(p => new PropriedadeComboDto
            {
                SequenciaDaPropriedade = p.SequenciaDaPropriedade,
                Nome = p.NomeDaPropriedade,
                Cnpj = p.Cnpj
            })
            .ToListAsync();
    }

    public async Task<List<PropriedadeComboDto>> ListarPropriedadesPorClienteAsync(int sequenciaDoGeral)
    {
        return await _context.PropriedadesDoGerals
            .Where(pg => pg.SequenciaDoGeral == sequenciaDoGeral && !pg.Inativo)
            .Join(_context.Propriedades,
                pg => pg.SequenciaDaPropriedade,
                p => p.SequenciaDaPropriedade,
                (pg, p) => new PropriedadeComboDto
                {
                    SequenciaDaPropriedade = p.SequenciaDaPropriedade,
                    Nome = p.NomeDaPropriedade,
                    Cnpj = p.Cnpj
                })
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<List<TipoCobrancaComboDto>> ListarTiposCobrancaComboAsync()
    {
        return await _context.TipoDeCobrancas
            .Where(t => !t.Inativo)
            .OrderBy(t => t.Descricao)
            .Select(t => new TipoCobrancaComboDto
            {
                SequenciaDaCobranca = t.SequenciaDaCobranca,
                Descricao = t.Descricao
            })
            .ToListAsync();
    }

    public async Task<List<VendedorComboDto>> ListarVendedoresComboAsync(string? busca)
    {
        var query = _context.Gerals
            .Where(g => g.Vendedor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var buscaLower = busca.ToLower();
            query = query.Where(g => g.RazaoSocial.ToLower().Contains(buscaLower));
        }

        return await query
            .OrderBy(g => g.RazaoSocial)
            .Take(50)
            .Select(g => new VendedorComboDto
            {
                SequenciaDoGeral = g.SequenciaDoGeral,
                Nome = g.RazaoSocial
            })
            .ToListAsync();
    }

    public async Task<List<ProdutoComboDto>> ListarProdutosComboAsync(string? busca)
    {
        var query = _context.Produtos
            .Include(p => p.SequenciaDaUnidadeNavigation)
            .Include(p => p.SequenciaDaClassificacaoNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var buscaLower = busca.ToLower();
            query = query.Where(p => 
                p.Descricao.ToLower().Contains(buscaLower) ||
                p.CodigoDeBarras.ToLower().Contains(buscaLower));
        }

        return await query
            .OrderBy(p => p.Descricao)
            .Take(50)
            .Select(p => new ProdutoComboDto
            {
                SequenciaDoProduto = p.SequenciaDoProduto,
                Descricao = p.Descricao,
                CodigoDeBarras = p.CodigoDeBarras ?? "",
                Unidade = p.SequenciaDaUnidadeNavigation != null ? p.SequenciaDaUnidadeNavigation.SiglaDaUnidade : "UN",
                Ncm = p.SequenciaDaClassificacaoNavigation != null ? p.SequenciaDaClassificacaoNavigation.Ncm.ToString().PadLeft(8, '0') : "",
                Cfop = "", // CFOP vem da natureza de operação, não do produto
                PrecoVenda = p.ValorTotal,
                ValorTotal = p.ValorTotal,
                QuantidadeNoEstoque = p.QuantidadeNoEstoque
            })
            .ToListAsync();
    }

    public async Task<ClienteComboDto?> ObterClienteAsync(int id)
    {
        return await _context.Gerals
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .Where(g => g.SequenciaDoGeral == id)
            .Select(g => new ClienteComboDto
            {
                SequenciaDoGeral = g.SequenciaDoGeral,
                Nome = g.RazaoSocial,
                Documento = g.CpfECnpj,
                Cidade = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Descricao : "",
                Uf = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Uf : ""
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TransportadoraComboDto?> ObterTransportadoraAsync(int id)
    {
        return await _context.Gerals
            .Include(g => g.SequenciaDoMunicipioNavigation)
            .Where(g => g.SequenciaDoGeral == id)
            .Select(g => new TransportadoraComboDto
            {
                SequenciaDoGeral = g.SequenciaDoGeral,
                Nome = g.RazaoSocial,
                Documento = g.CpfECnpj,
                Cidade = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Descricao : "",
                Uf = g.SequenciaDoMunicipioNavigation != null ? g.SequenciaDoMunicipioNavigation.Uf : ""
            })
            .FirstOrDefaultAsync();
    }

    #endregion
    
    #region Itens da Nota Fiscal - Produtos
    
    public async Task<List<ProdutoDaNotaFiscalDto>> ListarProdutosAsync(int notaFiscalId)
    {
        return await _context.ProdutosDaNotaFiscals
            .Include(p => p.SequenciaDoProdutoNavigation)
                .ThenInclude(pr => pr.SequenciaDaUnidadeNavigation)
            .Include(p => p.SequenciaDoProdutoNavigation)
                .ThenInclude(pr => pr.SequenciaDaClassificacaoNavigation)
            .Where(p => p.SequenciaDaNotaFiscal == notaFiscalId)
            .OrderBy(p => p.SequenciaProdutoNotaFiscal)
            .Select(p => new ProdutoDaNotaFiscalDto
            {
                SequenciaDoProdutoDaNotaFiscal = p.SequenciaProdutoNotaFiscal,
                SequenciaDoProduto = p.SequenciaDoProduto,
                DescricaoProduto = p.SequenciaDoProdutoNavigation.Descricao,
                Unidade = p.SequenciaDoProdutoNavigation.SequenciaDaUnidadeNavigation.SiglaDaUnidade,
                Ncm = p.SequenciaDoProdutoNavigation.SequenciaDaClassificacaoNavigation != null 
                    ? p.SequenciaDoProdutoNavigation.SequenciaDaClassificacaoNavigation.Ncm.ToString() 
                    : "",
                Cfop = p.Cfop.ToString(),
                Cst = p.Cst.ToString(),
                Quantidade = p.Quantidade,
                ValorUnitario = p.ValorUnitario,
                ValorTotal = p.ValorTotal,
                Desconto = p.ValorDoDesconto,
                BaseDeCalculoIcms = p.ValorDaBaseDeCalculo,
                AliquotaIcms = p.AliquotaDoIcms,
                ValorIcms = p.ValorDoIcms,
                BaseDeCalculoSt = p.BaseDeCalculoSt,
                AliquotaSt = p.AliquotaDoIcmsSt,
                ValorIcmsSt = p.ValorIcmsSt,
                BaseDeCalculoIpi = p.ValorTotal,
                AliquotaIpi = p.AliquotaDoIpi,
                ValorIpi = p.ValorDoIpi,
                AliquotaPis = p.AliqDoPis,
                ValorPis = p.ValorDoPis,
                AliquotaCofins = p.AliqDoCofins,
                ValorCofins = p.ValorDoCofins,
                ValorIbs = p.ValorIbs,
                ValorCbs = p.ValorCbs
            })
            .ToListAsync();
    }
    
    public async Task<ProdutoDaNotaFiscalDto> AdicionarProdutoAsync(int notaFiscalId, ProdutoDaNotaFiscalCreateDto dto)
    {
        var produto = new ProdutoDaNotaFiscal
        {
            SequenciaDaNotaFiscal = notaFiscalId,
            SequenciaDoProduto = dto.SequenciaDoProduto,
            Quantidade = dto.Quantidade,
            ValorUnitario = dto.ValorUnitario,
            ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : dto.Quantidade * dto.ValorUnitario,
            ValorDoDesconto = dto.Desconto,
            ValorDaBaseDeCalculo = dto.BaseDeCalculoIcms,
            AliquotaDoIcms = dto.AliquotaIcms,
            ValorDoIcms = dto.ValorIcms,
            PercentualDaReducao = dto.PercentualReducaoIcms,
            Diferido = dto.Diferido,
            BaseDeCalculoSt = dto.BaseDeCalculoSt,
            AliquotaDoIcmsSt = dto.AliquotaSt,
            ValorIcmsSt = dto.ValorIcmsSt,
            Iva = dto.Iva,
            AliquotaDoIpi = dto.AliquotaIpi,
            ValorDoIpi = dto.ValorIpi,
            BcPis = dto.BcPis,
            AliqDoPis = dto.AliquotaPis,
            ValorDoPis = dto.ValorPis,
            BcCofins = dto.BcCofins,
            AliqDoCofins = dto.AliquotaCofins,
            ValorDoCofins = dto.ValorCofins,
            BaseDeCalculoDaImportacao = dto.BaseDeCalculoImportacao,
            ValorDasDespesasAduaneiras = dto.DespesasAduaneiras,
            ValorDoImpostoDeImportacao = dto.ImpostoImportacao,
            ValorDoIof = dto.Iof,
            Cfop = dto.Cfop,
            Cst = dto.Cst,
            ValorDoTributo = dto.ValorTributo,
            ValorDoFrete = dto.ValorFrete
        };
        
        _context.ProdutosDaNotaFiscals.Add(produto);
        await _context.SaveChangesAsync();
        
        await RecalcularTotaisAsync(notaFiscalId);
        
        var produtos = await ListarProdutosAsync(notaFiscalId);
        return produtos.First(p => p.SequenciaDoProdutoDaNotaFiscal == produto.SequenciaProdutoNotaFiscal);
    }
    
    public async Task<ProdutoDaNotaFiscalDto?> AtualizarProdutoAsync(int notaFiscalId, int sequenciaProduto, ProdutoDaNotaFiscalCreateDto dto)
    {
        var produto = await _context.ProdutosDaNotaFiscals
            .FirstOrDefaultAsync(p => p.SequenciaDaNotaFiscal == notaFiscalId && p.SequenciaProdutoNotaFiscal == sequenciaProduto);
            
        if (produto == null)
            return null;
            
        produto.SequenciaDoProduto = dto.SequenciaDoProduto;
        produto.Quantidade = dto.Quantidade;
        produto.ValorUnitario = dto.ValorUnitario;
        produto.ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : dto.Quantidade * dto.ValorUnitario;
        produto.ValorDoDesconto = dto.Desconto;
        produto.ValorDaBaseDeCalculo = dto.BaseDeCalculoIcms;
        produto.AliquotaDoIcms = dto.AliquotaIcms;
        produto.ValorDoIcms = dto.ValorIcms;
        produto.PercentualDaReducao = dto.PercentualReducaoIcms;
        produto.Diferido = dto.Diferido;
        produto.BaseDeCalculoSt = dto.BaseDeCalculoSt;
        produto.AliquotaDoIcmsSt = dto.AliquotaSt;
        produto.ValorIcmsSt = dto.ValorIcmsSt;
        produto.Iva = dto.Iva;
        produto.AliquotaDoIpi = dto.AliquotaIpi;
        produto.ValorDoIpi = dto.ValorIpi;
        produto.BcPis = dto.BcPis;
        produto.AliqDoPis = dto.AliquotaPis;
        produto.ValorDoPis = dto.ValorPis;
        produto.BcCofins = dto.BcCofins;
        produto.AliqDoCofins = dto.AliquotaCofins;
        produto.ValorDoCofins = dto.ValorCofins;
        produto.BaseDeCalculoDaImportacao = dto.BaseDeCalculoImportacao;
        produto.ValorDasDespesasAduaneiras = dto.DespesasAduaneiras;
        produto.ValorDoImpostoDeImportacao = dto.ImpostoImportacao;
        produto.ValorDoIof = dto.Iof;
        produto.Cfop = dto.Cfop;
        produto.Cst = dto.Cst;
        produto.ValorDoTributo = dto.ValorTributo;
        produto.ValorDoFrete = dto.ValorFrete;
        
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        var produtos = await ListarProdutosAsync(notaFiscalId);
        return produtos.FirstOrDefault(p => p.SequenciaDoProdutoDaNotaFiscal == sequenciaProduto);
    }
    
    public async Task<bool> RemoverProdutoAsync(int notaFiscalId, int sequenciaProduto)
    {
        var produto = await _context.ProdutosDaNotaFiscals
            .FirstOrDefaultAsync(p => p.SequenciaDaNotaFiscal == notaFiscalId && p.SequenciaProdutoNotaFiscal == sequenciaProduto);
            
        if (produto == null)
            return false;
            
        _context.ProdutosDaNotaFiscals.Remove(produto);
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        return true;
    }
    
    #endregion
    
    #region Itens da Nota Fiscal - Conjuntos
    
    public async Task<List<ConjuntoDaNotaFiscalDto>> ListarConjuntosAsync(int notaFiscalId)
    {
        return await _context.ConjuntosDaNotaFiscals
            .Include(c => c.SequenciaDoConjuntoNavigation)
            .Where(c => c.SequenciaDaNotaFiscal == notaFiscalId)
            .OrderBy(c => c.SequenciaConjuntoNotaFiscal)
            .Select(c => new ConjuntoDaNotaFiscalDto
            {
                SequenciaDoConjuntoDaNotaFiscal = c.SequenciaConjuntoNotaFiscal,
                SequenciaDoConjunto = c.SequenciaDoConjunto,
                DescricaoConjunto = c.SequenciaDoConjuntoNavigation.Descricao,
                Quantidade = c.Quantidade,
                ValorUnitario = c.ValorUnitario,
                ValorTotal = c.ValorTotal,
                ValorIpi = c.ValorDoIpi,
                ValorIbs = c.ValorIbs,
                ValorCbs = c.ValorCbs
            })
            .ToListAsync();
    }
    
    public async Task<ConjuntoDaNotaFiscalDto> AdicionarConjuntoAsync(int notaFiscalId, ConjuntoDaNotaFiscalCreateDto dto)
    {
        var conjunto = new ConjuntoDaNotaFiscal
        {
            SequenciaDaNotaFiscal = notaFiscalId,
            SequenciaDoConjunto = dto.SequenciaDoConjunto,
            Quantidade = dto.Quantidade,
            ValorUnitario = dto.ValorUnitario,
            ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : dto.Quantidade * dto.ValorUnitario,
            ValorDoDesconto = dto.Desconto,
            ValorDaBaseDeCalculo = dto.BaseDeCalculoIcms,
            AliquotaDoIcms = dto.AliquotaIcms,
            ValorDoIcms = dto.ValorIcms,
            PercentualDaReducao = dto.PercentualReducaoIcms,
            Diferido = dto.Diferido,
            BaseDeCalculoSt = dto.BaseDeCalculoSt,
            AliquotaDoIcmsSt = dto.AliquotaSt,
            ValorIcmsSt = dto.ValorIcmsSt,
            Iva = dto.Iva,
            AliquotaDoIpi = dto.AliquotaIpi,
            ValorDoIpi = dto.ValorIpi,
            BcPis = dto.BcPis,
            AliqDoPis = dto.AliquotaPis,
            ValorDoPis = dto.ValorPis,
            BcCofins = dto.BcCofins,
            AliqDoCofins = dto.AliquotaCofins,
            ValorDoCofins = dto.ValorCofins,
            Cfop = dto.Cfop,
            Cst = dto.Cst,
            ValorDoTributo = dto.ValorTributo,
            ValorDoFrete = dto.ValorFrete
        };
        
        _context.ConjuntosDaNotaFiscals.Add(conjunto);
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        var conjuntos = await ListarConjuntosAsync(notaFiscalId);
        return conjuntos.First(c => c.SequenciaDoConjuntoDaNotaFiscal == conjunto.SequenciaConjuntoNotaFiscal);
    }
    
    public async Task<ConjuntoDaNotaFiscalDto?> AtualizarConjuntoAsync(int notaFiscalId, int sequenciaConjunto, ConjuntoDaNotaFiscalCreateDto dto)
    {
        var conjunto = await _context.ConjuntosDaNotaFiscals
            .FirstOrDefaultAsync(c => c.SequenciaDaNotaFiscal == notaFiscalId && c.SequenciaConjuntoNotaFiscal == sequenciaConjunto);
            
        if (conjunto == null)
            return null;
            
        conjunto.SequenciaDoConjunto = dto.SequenciaDoConjunto;
        conjunto.Quantidade = dto.Quantidade;
        conjunto.ValorUnitario = dto.ValorUnitario;
        conjunto.ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : dto.Quantidade * dto.ValorUnitario;
        conjunto.ValorDoDesconto = dto.Desconto;
        conjunto.ValorDaBaseDeCalculo = dto.BaseDeCalculoIcms;
        conjunto.AliquotaDoIcms = dto.AliquotaIcms;
        conjunto.ValorDoIcms = dto.ValorIcms;
        conjunto.PercentualDaReducao = dto.PercentualReducaoIcms;
        conjunto.Diferido = dto.Diferido;
        conjunto.BaseDeCalculoSt = dto.BaseDeCalculoSt;
        conjunto.AliquotaDoIcmsSt = dto.AliquotaSt;
        conjunto.ValorIcmsSt = dto.ValorIcmsSt;
        conjunto.Iva = dto.Iva;
        conjunto.AliquotaDoIpi = dto.AliquotaIpi;
        conjunto.ValorDoIpi = dto.ValorIpi;
        conjunto.BcPis = dto.BcPis;
        conjunto.AliqDoPis = dto.AliquotaPis;
        conjunto.ValorDoPis = dto.ValorPis;
        conjunto.BcCofins = dto.BcCofins;
        conjunto.AliqDoCofins = dto.AliquotaCofins;
        conjunto.ValorDoCofins = dto.ValorCofins;
        conjunto.Cfop = dto.Cfop;
        conjunto.Cst = dto.Cst;
        conjunto.ValorDoTributo = dto.ValorTributo;
        conjunto.ValorDoFrete = dto.ValorFrete;
        
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        var conjuntos = await ListarConjuntosAsync(notaFiscalId);
        return conjuntos.FirstOrDefault(c => c.SequenciaDoConjuntoDaNotaFiscal == sequenciaConjunto);
    }
    
    public async Task<bool> RemoverConjuntoAsync(int notaFiscalId, int sequenciaConjunto)
    {
        var conjunto = await _context.ConjuntosDaNotaFiscals
            .FirstOrDefaultAsync(c => c.SequenciaDaNotaFiscal == notaFiscalId && c.SequenciaConjuntoNotaFiscal == sequenciaConjunto);
            
        if (conjunto == null)
            return false;
            
        _context.ConjuntosDaNotaFiscals.Remove(conjunto);
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        return true;
    }
    
    #endregion
    
    #region Itens da Nota Fiscal - Peças
    
    public async Task<List<PecaDaNotaFiscalDto>> ListarPecasAsync(int notaFiscalId)
    {
        return await _context.PecasDaNotaFiscals
            .Include(p => p.SequenciaDoProdutoNavigation)
            .Where(p => p.SequenciaDaNotaFiscal == notaFiscalId)
            .OrderBy(p => p.SequenciaDaPecaNotaFiscal)
            .Select(p => new PecaDaNotaFiscalDto
            {
                SequenciaDaPecaDaNotaFiscal = p.SequenciaDaPecaNotaFiscal,
                SequenciaDaPeca = p.SequenciaDoProduto,
                DescricaoPeca = p.SequenciaDoProdutoNavigation.Descricao,
                Quantidade = p.Quantidade,
                ValorUnitario = p.ValorUnitario,
                ValorTotal = p.ValorTotal,
                ValorIpi = p.ValorDoIpi,
                ValorIbs = p.ValorIbs,
                ValorCbs = p.ValorCbs
            })
            .ToListAsync();
    }
    
    public async Task<PecaDaNotaFiscalDto> AdicionarPecaAsync(int notaFiscalId, PecaDaNotaFiscalCreateDto dto)
    {
        var peca = new PecaDaNotaFiscal
        {
            SequenciaDaNotaFiscal = notaFiscalId,
            SequenciaDoProduto = dto.SequenciaDoProduto,
            Quantidade = dto.Quantidade,
            ValorUnitario = dto.ValorUnitario,
            ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : dto.Quantidade * dto.ValorUnitario,
            ValorDoDesconto = dto.Desconto,
            ValorDaBaseDeCalculo = dto.BaseDeCalculoIcms,
            AliquotaDoIcms = dto.AliquotaIcms,
            ValorDoIcms = dto.ValorIcms,
            PercentualDaReducao = dto.PercentualReducaoIcms,
            Diferido = dto.Diferido,
            BaseDeCalculoSt = dto.BaseDeCalculoSt,
            AliquotaDoIcmsSt = dto.AliquotaSt,
            ValorIcmsSt = dto.ValorIcmsSt,
            Iva = dto.Iva,
            AliquotaDoIpi = dto.AliquotaIpi,
            ValorDoIpi = dto.ValorIpi,
            BcPis = dto.BcPis,
            AliqDoPis = dto.AliquotaPis,
            ValorDoPis = dto.ValorPis,
            BcCofins = dto.BcCofins,
            AliqDoCofins = dto.AliquotaCofins,
            ValorDoCofins = dto.ValorCofins,
            Cfop = dto.Cfop,
            Cst = dto.Cst,
            ValorDoTributo = dto.ValorTributo,
            ValorDoFrete = dto.ValorFrete
        };
        
        _context.PecasDaNotaFiscals.Add(peca);
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        var pecas = await ListarPecasAsync(notaFiscalId);
        return pecas.First(p => p.SequenciaDaPecaDaNotaFiscal == peca.SequenciaDaPecaNotaFiscal);
    }
    
    public async Task<PecaDaNotaFiscalDto?> AtualizarPecaAsync(int notaFiscalId, int sequenciaPeca, PecaDaNotaFiscalCreateDto dto)
    {
        var peca = await _context.PecasDaNotaFiscals
            .FirstOrDefaultAsync(p => p.SequenciaDaNotaFiscal == notaFiscalId && p.SequenciaDaPecaNotaFiscal == sequenciaPeca);
            
        if (peca == null)
            return null;
            
        peca.SequenciaDoProduto = dto.SequenciaDoProduto;
        peca.Quantidade = dto.Quantidade;
        peca.ValorUnitario = dto.ValorUnitario;
        peca.ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : dto.Quantidade * dto.ValorUnitario;
        peca.ValorDoDesconto = dto.Desconto;
        peca.ValorDaBaseDeCalculo = dto.BaseDeCalculoIcms;
        peca.AliquotaDoIcms = dto.AliquotaIcms;
        peca.ValorDoIcms = dto.ValorIcms;
        peca.PercentualDaReducao = dto.PercentualReducaoIcms;
        peca.Diferido = dto.Diferido;
        peca.BaseDeCalculoSt = dto.BaseDeCalculoSt;
        peca.AliquotaDoIcmsSt = dto.AliquotaSt;
        peca.ValorIcmsSt = dto.ValorIcmsSt;
        peca.Iva = dto.Iva;
        peca.AliquotaDoIpi = dto.AliquotaIpi;
        peca.ValorDoIpi = dto.ValorIpi;
        peca.BcPis = dto.BcPis;
        peca.AliqDoPis = dto.AliquotaPis;
        peca.ValorDoPis = dto.ValorPis;
        peca.BcCofins = dto.BcCofins;
        peca.AliqDoCofins = dto.AliquotaCofins;
        peca.ValorDoCofins = dto.ValorCofins;
        peca.Cfop = dto.Cfop;
        peca.Cst = dto.Cst;
        peca.ValorDoTributo = dto.ValorTributo;
        peca.ValorDoFrete = dto.ValorFrete;
        
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        var pecas = await ListarPecasAsync(notaFiscalId);
        return pecas.FirstOrDefault(p => p.SequenciaDaPecaDaNotaFiscal == sequenciaPeca);
    }
    
    public async Task<bool> RemoverPecaAsync(int notaFiscalId, int sequenciaPeca)
    {
        var peca = await _context.PecasDaNotaFiscals
            .FirstOrDefaultAsync(p => p.SequenciaDaNotaFiscal == notaFiscalId && p.SequenciaDaPecaNotaFiscal == sequenciaPeca);
            
        if (peca == null)
            return false;
            
        _context.PecasDaNotaFiscals.Remove(peca);
        await _context.SaveChangesAsync();
        await RecalcularTotaisAsync(notaFiscalId);
        
        return true;
    }
    
    #endregion
    
    #region Itens da Nota Fiscal - Parcelas
    
    public async Task<List<ParcelaNotaFiscalDto>> ListarParcelasAsync(int notaFiscalId)
    {
        return await _context.ParcelasNotaFiscals
            .Where(p => p.SequenciaDaNotaFiscal == notaFiscalId)
            .OrderBy(p => p.NumeroDaParcela)
            .Select(p => new ParcelaNotaFiscalDto
            {
                NumeroDaParcela = p.NumeroDaParcela,
                Dias = p.Dias,
                DataDeVencimento = p.DataDeVencimento,
                Valor = p.ValorDaParcela
            })
            .ToListAsync();
    }
    
    public async Task<ParcelaNotaFiscalDto> AdicionarParcelaAsync(int notaFiscalId, ParcelaNotaFiscalCreateDto dto)
    {
        var parcela = new ParcelaNotaFiscal
        {
            SequenciaDaNotaFiscal = notaFiscalId,
            NumeroDaParcela = dto.NumeroDaParcela,
            Dias = dto.Dias,
            DataDeVencimento = dto.DataDeVencimento,
            ValorDaParcela = dto.Valor
        };
        
        _context.ParcelasNotaFiscals.Add(parcela);
        await _context.SaveChangesAsync();
        
        return new ParcelaNotaFiscalDto
        {
            NumeroDaParcela = parcela.NumeroDaParcela,
            Dias = parcela.Dias,
            DataDeVencimento = parcela.DataDeVencimento,
            Valor = parcela.ValorDaParcela
        };
    }
    
    public async Task<ParcelaNotaFiscalDto?> AtualizarParcelaAsync(int notaFiscalId, short numeroParcela, ParcelaNotaFiscalCreateDto dto)
    {
        var parcela = await _context.ParcelasNotaFiscals
            .FirstOrDefaultAsync(p => p.SequenciaDaNotaFiscal == notaFiscalId && p.NumeroDaParcela == numeroParcela);
            
        if (parcela == null)
            return null;
            
        parcela.Dias = dto.Dias;
        parcela.DataDeVencimento = dto.DataDeVencimento;
        parcela.ValorDaParcela = dto.Valor;
        
        await _context.SaveChangesAsync();
        
        return new ParcelaNotaFiscalDto
        {
            NumeroDaParcela = parcela.NumeroDaParcela,
            Dias = parcela.Dias,
            DataDeVencimento = parcela.DataDeVencimento,
            Valor = parcela.ValorDaParcela
        };
    }
    
    public async Task<bool> RemoverParcelaAsync(int notaFiscalId, short numeroParcela)
    {
        var parcela = await _context.ParcelasNotaFiscals
            .FirstOrDefaultAsync(p => p.SequenciaDaNotaFiscal == notaFiscalId && p.NumeroDaParcela == numeroParcela);
            
        if (parcela == null)
            return false;
            
        _context.ParcelasNotaFiscals.Remove(parcela);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    #endregion
    
    #region Recalcular Totais
    
    public async Task RecalcularTotaisAsync(int notaFiscalId)
    {
        var nota = await _context.NotaFiscals.FindAsync(notaFiscalId);
        if (nota == null) return;
        
        // Somar produtos
        var produtosTotais = await _context.ProdutosDaNotaFiscals
            .Where(p => p.SequenciaDaNotaFiscal == notaFiscalId)
            .GroupBy(p => 1)
            .Select(g => new
            {
                ValorTotal = g.Sum(p => p.ValorTotal),
                ValorIpi = g.Sum(p => p.ValorDoIpi),
                ValorIcms = g.Sum(p => p.ValorDoIcms),
                BaseCalculo = g.Sum(p => p.ValorDaBaseDeCalculo),
                BaseSt = g.Sum(p => p.BaseDeCalculoSt),
                IcmsSt = g.Sum(p => p.ValorIcmsSt),
                Pis = g.Sum(p => p.ValorDoPis),
                Cofins = g.Sum(p => p.ValorDoCofins),
                Tributo = g.Sum(p => p.ValorDoTributo)
            })
            .FirstOrDefaultAsync();
            
        // Somar conjuntos
        var conjuntosTotais = await _context.ConjuntosDaNotaFiscals
            .Where(c => c.SequenciaDaNotaFiscal == notaFiscalId)
            .GroupBy(c => 1)
            .Select(g => new
            {
                ValorTotal = g.Sum(c => c.ValorTotal),
                ValorIpi = g.Sum(c => c.ValorDoIpi),
                ValorIcms = g.Sum(c => c.ValorDoIcms),
                BaseCalculo = g.Sum(c => c.ValorDaBaseDeCalculo),
                BaseSt = g.Sum(c => c.BaseDeCalculoSt),
                IcmsSt = g.Sum(c => c.ValorIcmsSt),
                Pis = g.Sum(c => c.ValorDoPis),
                Cofins = g.Sum(c => c.ValorDoCofins),
                Tributo = g.Sum(c => c.ValorDoTributo)
            })
            .FirstOrDefaultAsync();
            
        // Somar peças
        var pecasTotais = await _context.PecasDaNotaFiscals
            .Where(p => p.SequenciaDaNotaFiscal == notaFiscalId)
            .GroupBy(p => 1)
            .Select(g => new
            {
                ValorTotal = g.Sum(p => p.ValorTotal),
                ValorIpi = g.Sum(p => p.ValorDoIpi),
                ValorIcms = g.Sum(p => p.ValorDoIcms),
                BaseCalculo = g.Sum(p => p.ValorDaBaseDeCalculo),
                BaseSt = g.Sum(p => p.BaseDeCalculoSt),
                IcmsSt = g.Sum(p => p.ValorIcmsSt),
                Pis = g.Sum(p => p.ValorDoPis),
                Cofins = g.Sum(p => p.ValorDoCofins),
                Tributo = g.Sum(p => p.ValorDoTributo)
            })
            .FirstOrDefaultAsync();
        
        // Atualizar totais
        nota.ValorTotalDosProdutos = produtosTotais?.ValorTotal ?? 0;
        nota.ValorTotalIpiDosProdutos = produtosTotais?.ValorIpi ?? 0;
        
        nota.ValorTotalDosConjuntos = conjuntosTotais?.ValorTotal ?? 0;
        nota.ValorTotalIpiDosConjuntos = conjuntosTotais?.ValorIpi ?? 0;
        
        nota.ValorTotalDasPecas = pecasTotais?.ValorTotal ?? 0;
        nota.ValorTotalIpiDasPecas = pecasTotais?.ValorIpi ?? 0;
        
        // Totais gerais
        nota.ValorTotalDaBaseDeCalculo = (produtosTotais?.BaseCalculo ?? 0) + 
                                          (conjuntosTotais?.BaseCalculo ?? 0) + 
                                          (pecasTotais?.BaseCalculo ?? 0);
                                          
        nota.ValorTotalDoIcms = (produtosTotais?.ValorIcms ?? 0) + 
                                (conjuntosTotais?.ValorIcms ?? 0) + 
                                (pecasTotais?.ValorIcms ?? 0);
                                
        nota.ValorTotalDaBaseSt = (produtosTotais?.BaseSt ?? 0) + 
                                  (conjuntosTotais?.BaseSt ?? 0) + 
                                  (pecasTotais?.BaseSt ?? 0);
                                  
        nota.ValorTotalDoIcmsSt = (produtosTotais?.IcmsSt ?? 0) + 
                                  (conjuntosTotais?.IcmsSt ?? 0) + 
                                  (pecasTotais?.IcmsSt ?? 0);
                                  
        nota.ValorTotalDoPis = (produtosTotais?.Pis ?? 0) + 
                               (conjuntosTotais?.Pis ?? 0) + 
                               (pecasTotais?.Pis ?? 0);
                               
        nota.ValorTotalDoCofins = (produtosTotais?.Cofins ?? 0) + 
                                  (conjuntosTotais?.Cofins ?? 0) + 
                                  (pecasTotais?.Cofins ?? 0);
                                  
        nota.ValorTotalDoTributo = (produtosTotais?.Tributo ?? 0) + 
                                   (conjuntosTotais?.Tributo ?? 0) + 
                                   (pecasTotais?.Tributo ?? 0);
        
        // Valor total da nota
        nota.ValorTotalDaNotaFiscal = nota.ValorTotalDosProdutos + 
                                       nota.ValorTotalDosConjuntos + 
                                       nota.ValorTotalDasPecas + 
                                       nota.ValorTotalDosServicos +
                                       nota.ValorTotalIpiDosProdutos +
                                       nota.ValorTotalIpiDosConjuntos +
                                       nota.ValorTotalIpiDasPecas +
                                       nota.ValorTotalDoIcmsSt +
                                       nota.ValorDoFrete +
                                       nota.ValorDoSeguro +
                                       nota.OutrasDespesas;
        
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Duplica uma nota fiscal existente
    /// </summary>
    public async Task<NotaFiscal> DuplicarAsync(int notaFiscalId, string usuario)
    {
        // Buscar nota original com todos os itens
        var notaOriginal = await _context.NotaFiscals
            .Include(n => n.ProdutosDaNotaFiscals)
            .Include(n => n.ConjuntosDaNotaFiscals)
            .Include(n => n.PecasDaNotaFiscals)
            .Include(n => n.ParcelasNotaFiscals)
            .FirstOrDefaultAsync(n => n.SequenciaDaNotaFiscal == notaFiscalId);

        if (notaOriginal == null)
            throw new InvalidOperationException("Nota fiscal não encontrada");

        // Obter próximo número
        var proximoNumero = await ObterProximoNumeroAsync(notaOriginal.SequenciaDaPropriedade);

        // Criar nova nota com dados copiados (exceto campos de autorização)
        var novaNota = new NotaFiscal
        {
            // Novo número
            NumeroDaNotaFiscal = proximoNumero,
            NumeroDaNfe = 0, // Será preenchido na transmissão
            NumeroDaNfse = 0,
            
            // Datas atualizadas para hoje
            DataDeEmissao = DateTime.Now,
            DataDeSaida = DateTime.Now,
            HoraDaSaida = DateTime.Now,
            
            // Dados do cliente/destinatário
            SequenciaDoGeral = notaOriginal.SequenciaDoGeral,
            SequenciaDaPropriedade = notaOriginal.SequenciaDaPropriedade,
            SequenciaDaNatureza = notaOriginal.SequenciaDaNatureza,
            SequenciaDaClassificacao = notaOriginal.SequenciaDaClassificacao,
            SequenciaDaCobranca = notaOriginal.SequenciaDaCobranca,
            
            // Transportadora
            TransportadoraAvulsa = notaOriginal.TransportadoraAvulsa,
            SequenciaDaTransportadora = notaOriginal.SequenciaDaTransportadora,
            NomeDaTransportadoraAvulsa = notaOriginal.NomeDaTransportadoraAvulsa,
            DocumentoDaTransportadora = notaOriginal.DocumentoDaTransportadora,
            IeDaTransportadora = notaOriginal.IeDaTransportadora,
            EnderecoDaTransportadora = notaOriginal.EnderecoDaTransportadora,
            MunicipioDaTransportadora = notaOriginal.MunicipioDaTransportadora,
            PlacaDoVeiculo = notaOriginal.PlacaDoVeiculo,
            UfDoVeiculo = notaOriginal.UfDoVeiculo,
            CodigoDaAntt = notaOriginal.CodigoDaAntt,
            
            // Frete
            Frete = notaOriginal.Frete,
            ValorDoFrete = notaOriginal.ValorDoFrete,
            
            // Volumes
            Volume = notaOriginal.Volume,
            Especie = notaOriginal.Especie,
            Marca = notaOriginal.Marca,
            Numeracao = notaOriginal.Numeracao,
            PesoBruto = notaOriginal.PesoBruto,
            PesoLiquido = notaOriginal.PesoLiquido,
            
            // Pagamento
            FormaDePagamento = notaOriginal.FormaDePagamento,
            ContraApresentacao = notaOriginal.ContraApresentacao,
            Fechamento = notaOriginal.Fechamento,
            ValorDoFechamento = notaOriginal.ValorDoFechamento,
            
            // Observações
            Historico = notaOriginal.Historico,
            Observacao = notaOriginal.Observacao,
            
            // Tipo e Flags
            TipoDeNota = notaOriginal.TipoDeNota,
            NotaFiscalAvulsa = notaOriginal.NotaFiscalAvulsa,
            OcultarValorUnitario = notaOriginal.OcultarValorUnitario,
            
            // Outros valores
            ValorDoSeguro = notaOriginal.ValorDoSeguro,
            OutrasDespesas = notaOriginal.OutrasDespesas,
            
            // NFe Complementar/Devolução - NÃO copiar referências
            NfeComplementar = false,
            ChaveAcessoNfeReferenciada = "",
            NotaDeDevolucao = false,
            ChaveDaDevolucao = "",
            ChaveDaDevolucao2 = "",
            ChaveDaDevolucao3 = "",
            
            // Conjunto Avulso
            ConjuntoAvulso = notaOriginal.ConjuntoAvulso,
            DescricaoConjuntoAvulso = notaOriginal.DescricaoConjuntoAvulso,
            
            // Relacionamentos
            SequenciaDoPedido = 0, // Não vincular ao mesmo pedido
            SequenciaDoVendedor = notaOriginal.SequenciaDoVendedor,
            NumeroDoContrato = notaOriginal.NumeroDoContrato,
            Refaturamento = notaOriginal.Refaturamento,
            Financiamento = notaOriginal.Financiamento,
            
            // Valores (serão recalculados)
            ValorTotalDosProdutos = notaOriginal.ValorTotalDosProdutos,
            ValorTotalDosConjuntos = notaOriginal.ValorTotalDosConjuntos,
            ValorTotalDasPecas = notaOriginal.ValorTotalDasPecas,
            ValorTotalDosServicos = notaOriginal.ValorTotalDosServicos,
            ValorTotalDaNotaFiscal = notaOriginal.ValorTotalDaNotaFiscal,
            ValorTotalDaBaseDeCalculo = notaOriginal.ValorTotalDaBaseDeCalculo,
            ValorTotalDoIcms = notaOriginal.ValorTotalDoIcms,
            ValorTotalDaBaseSt = notaOriginal.ValorTotalDaBaseSt,
            ValorTotalDoIcmsSt = notaOriginal.ValorTotalDoIcmsSt,
            ValorTotalIpiDosProdutos = notaOriginal.ValorTotalIpiDosProdutos,
            ValorTotalIpiDosConjuntos = notaOriginal.ValorTotalIpiDosConjuntos,
            ValorTotalIpiDasPecas = notaOriginal.ValorTotalIpiDasPecas,
            ValorTotalDoPis = notaOriginal.ValorTotalDoPis,
            ValorTotalDoCofins = notaOriginal.ValorTotalDoCofins,
            ValorDoImpostoDeRenda = notaOriginal.ValorDoImpostoDeRenda,
            AliquotaDoIss = notaOriginal.AliquotaDoIss,
            ReterIss = notaOriginal.ReterIss,
            ValorTotalDoTributo = notaOriginal.ValorTotalDoTributo,
            ValorTotalDaImportacao = notaOriginal.ValorTotalDaImportacao,
            ValorTotalDeProdutosUsados = notaOriginal.ValorTotalDeProdutosUsados,
            ValorTotalConjuntosUsados = notaOriginal.ValorTotalConjuntosUsados,
            ValorTotalDasPecasUsadas = notaOriginal.ValorTotalDasPecasUsadas,
            ValorTotalIbs = notaOriginal.ValorTotalIbs,
            ValorTotalCbs = notaOriginal.ValorTotalCbs,
            
            // Status - Sempre nova (não autorizada)
            NotaCancelada = false,
            CanceladaNoLivro = false,
            Transmitido = false,
            Autorizado = false,
            Imprimiu = false,
            
            // Campos de NFe - Limpos
            ChaveDeAcessoDaNfe = "",
            ProtocoloDeAutorizacaoNfe = "",
            DataEHoraDaNfe = "",
            NumeroDoReciboDaNfe = "",
            ReciboNfse = "",
            
            FinNfe = 1, // Normal
            NovoLayout = true,
            SequenciaDoMovimento = 0,
            NotaDeVenda = 0
        };

        _context.NotaFiscals.Add(novaNota);
        await _context.SaveChangesAsync();

        // Obter próxima sequência para produtos
        var proximaSeqProduto = 1;

        // Duplicar produtos
        foreach (var prodOriginal in notaOriginal.ProdutosDaNotaFiscals)
        {
            var novoProduto = new ProdutoDaNotaFiscal
            {
                SequenciaDaNotaFiscal = novaNota.SequenciaDaNotaFiscal,
                SequenciaProdutoNotaFiscal = proximaSeqProduto++,
                SequenciaDoProduto = prodOriginal.SequenciaDoProduto,
                Quantidade = prodOriginal.Quantidade,
                ValorUnitario = prodOriginal.ValorUnitario,
                ValorTotal = prodOriginal.ValorTotal,
                ValorDoDesconto = prodOriginal.ValorDoDesconto,
                ValorDoIpi = prodOriginal.ValorDoIpi,
                AliquotaDoIpi = prodOriginal.AliquotaDoIpi,
                ValorDoIcms = prodOriginal.ValorDoIcms,
                AliquotaDoIcms = prodOriginal.AliquotaDoIcms,
                PercentualDaReducao = prodOriginal.PercentualDaReducao,
                Diferido = prodOriginal.Diferido,
                ValorDaBaseDeCalculo = prodOriginal.ValorDaBaseDeCalculo,
                BaseDeCalculoSt = prodOriginal.BaseDeCalculoSt,
                ValorIcmsSt = prodOriginal.ValorIcmsSt,
                AliquotaDoIcmsSt = prodOriginal.AliquotaDoIcmsSt,
                ValorDoPis = prodOriginal.ValorDoPis,
                AliqDoPis = prodOriginal.AliqDoPis,
                BcPis = prodOriginal.BcPis,
                ValorDoCofins = prodOriginal.ValorDoCofins,
                AliqDoCofins = prodOriginal.AliqDoCofins,
                BcCofins = prodOriginal.BcCofins,
                ValorDoTributo = prodOriginal.ValorDoTributo,
                Cst = prodOriginal.Cst,
                Cfop = prodOriginal.Cfop,
                Iva = prodOriginal.Iva,
                ValorDoFrete = prodOriginal.ValorDoFrete,
                BaseDeCalculoDaImportacao = prodOriginal.BaseDeCalculoDaImportacao,
                ValorDasDespesasAduaneiras = prodOriginal.ValorDasDespesasAduaneiras,
                ValorDoImpostoDeImportacao = prodOriginal.ValorDoImpostoDeImportacao,
                ValorDoIof = prodOriginal.ValorDoIof,
                ValorIbs = prodOriginal.ValorIbs,
                ValorCbs = prodOriginal.ValorCbs
            };
            _context.ProdutosDaNotaFiscals.Add(novoProduto);
        }

        // Obter próxima sequência para conjuntos
        var proximaSeqConjunto = 1;

        // Duplicar conjuntos
        foreach (var conjOriginal in notaOriginal.ConjuntosDaNotaFiscals)
        {
            var novoConjunto = new ConjuntoDaNotaFiscal
            {
                SequenciaDaNotaFiscal = novaNota.SequenciaDaNotaFiscal,
                SequenciaConjuntoNotaFiscal = proximaSeqConjunto++,
                SequenciaDoConjunto = conjOriginal.SequenciaDoConjunto,
                Quantidade = conjOriginal.Quantidade,
                ValorUnitario = conjOriginal.ValorUnitario,
                ValorTotal = conjOriginal.ValorTotal,
                ValorDoDesconto = conjOriginal.ValorDoDesconto,
                ValorDoIpi = conjOriginal.ValorDoIpi,
                AliquotaDoIpi = conjOriginal.AliquotaDoIpi,
                ValorDoIcms = conjOriginal.ValorDoIcms,
                AliquotaDoIcms = conjOriginal.AliquotaDoIcms,
                PercentualDaReducao = conjOriginal.PercentualDaReducao,
                Diferido = conjOriginal.Diferido,
                ValorDaBaseDeCalculo = conjOriginal.ValorDaBaseDeCalculo,
                BaseDeCalculoSt = conjOriginal.BaseDeCalculoSt,
                ValorIcmsSt = conjOriginal.ValorIcmsSt,
                AliquotaDoIcmsSt = conjOriginal.AliquotaDoIcmsSt,
                ValorDoPis = conjOriginal.ValorDoPis,
                AliqDoPis = conjOriginal.AliqDoPis,
                BcPis = conjOriginal.BcPis,
                ValorDoCofins = conjOriginal.ValorDoCofins,
                AliqDoCofins = conjOriginal.AliqDoCofins,
                BcCofins = conjOriginal.BcCofins,
                ValorDoTributo = conjOriginal.ValorDoTributo,
                Cst = conjOriginal.Cst,
                Cfop = conjOriginal.Cfop,
                Iva = conjOriginal.Iva,
                ValorDoFrete = conjOriginal.ValorDoFrete,
                ValorIbs = conjOriginal.ValorIbs,
                ValorCbs = conjOriginal.ValorCbs
            };
            _context.ConjuntosDaNotaFiscals.Add(novoConjunto);
        }

        // Obter próxima sequência para peças
        var proximaSeqPeca = 1;

        // Duplicar peças
        foreach (var pecaOriginal in notaOriginal.PecasDaNotaFiscals)
        {
            var novaPeca = new PecaDaNotaFiscal
            {
                SequenciaDaNotaFiscal = novaNota.SequenciaDaNotaFiscal,
                SequenciaDaPecaNotaFiscal = proximaSeqPeca++,
                SequenciaDoProduto = pecaOriginal.SequenciaDoProduto,
                Quantidade = pecaOriginal.Quantidade,
                ValorUnitario = pecaOriginal.ValorUnitario,
                ValorTotal = pecaOriginal.ValorTotal,
                ValorDoDesconto = pecaOriginal.ValorDoDesconto,
                ValorDoIpi = pecaOriginal.ValorDoIpi,
                AliquotaDoIpi = pecaOriginal.AliquotaDoIpi,
                ValorDoIcms = pecaOriginal.ValorDoIcms,
                AliquotaDoIcms = pecaOriginal.AliquotaDoIcms,
                PercentualDaReducao = pecaOriginal.PercentualDaReducao,
                Diferido = pecaOriginal.Diferido,
                ValorDaBaseDeCalculo = pecaOriginal.ValorDaBaseDeCalculo,
                BaseDeCalculoSt = pecaOriginal.BaseDeCalculoSt,
                ValorIcmsSt = pecaOriginal.ValorIcmsSt,
                AliquotaDoIcmsSt = pecaOriginal.AliquotaDoIcmsSt,
                ValorDoPis = pecaOriginal.ValorDoPis,
                AliqDoPis = pecaOriginal.AliqDoPis,
                BcPis = pecaOriginal.BcPis,
                ValorDoCofins = pecaOriginal.ValorDoCofins,
                AliqDoCofins = pecaOriginal.AliqDoCofins,
                BcCofins = pecaOriginal.BcCofins,
                ValorDoTributo = pecaOriginal.ValorDoTributo,
                Cst = pecaOriginal.Cst,
                Cfop = pecaOriginal.Cfop,
                Iva = pecaOriginal.Iva,
                ValorDoFrete = pecaOriginal.ValorDoFrete,
                ValorIbs = pecaOriginal.ValorIbs,
                ValorCbs = pecaOriginal.ValorCbs
            };
            _context.PecasDaNotaFiscals.Add(novaPeca);
        }

        // Duplicar parcelas (com datas atualizadas)
        var diasDiferenca = (DateTime.Now - (notaOriginal.DataDeEmissao ?? DateTime.Now)).Days;
        foreach (var parcelaOriginal in notaOriginal.ParcelasNotaFiscals)
        {
            var novaParcela = new ParcelaNotaFiscal
            {
                SequenciaDaNotaFiscal = novaNota.SequenciaDaNotaFiscal,
                NumeroDaParcela = parcelaOriginal.NumeroDaParcela,
                Dias = parcelaOriginal.Dias,
                DataDeVencimento = parcelaOriginal.DataDeVencimento.AddDays(diasDiferenca),
                ValorDaParcela = parcelaOriginal.ValorDaParcela
            };
            _context.ParcelasNotaFiscals.Add(novaParcela);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Nota Fiscal {IdOriginal} duplicada para {IdNova} por {Usuario}", 
            notaFiscalId, novaNota.SequenciaDaNotaFiscal, usuario);

        return novaNota;
    }
    
    #endregion
}
