using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services.Fiscal;

/// <summary>
/// Serviço para cálculo de impostos de itens da Nota Fiscal
/// Carrega contexto do banco e usa NotaFiscalItemService para calcular
/// </summary>
public interface ICalculoImpostoService
{
    /// <summary>
    /// Calcula impostos para um item (Produto, Conjunto ou Peça) baseado no contexto da nota fiscal
    /// </summary>
    Task<CalculoImpostoResultDto> CalcularImpostoAsync(int notaFiscalId, CalculoImpostoRequestDto request);
}

public class CalculoImpostoService : ICalculoImpostoService
{
    private readonly AppDbContext _context;
    private readonly NotaFiscalItemService _itemService;
    private readonly ILogger<CalculoImpostoService> _logger;

    public CalculoImpostoService(
        AppDbContext context,
        ILogger<CalculoImpostoService> logger)
    {
        _context = context;
        _itemService = new NotaFiscalItemService();
        _logger = logger;
    }

    public async Task<CalculoImpostoResultDto> CalcularImpostoAsync(int notaFiscalId, CalculoImpostoRequestDto request)
    {
        _logger.LogInformation("Calculando impostos - NF: {NotaFiscalId}, TipoItem: {TipoItem}, SeqItem: {SeqItem}",
            notaFiscalId, request.TipoItem, request.SequenciaDoItem);

        // 1. Carregar dados da Nota Fiscal (cliente, propriedade, UF)
        var nota = await _context.NotaFiscals
            .Include(n => n.SequenciaDoGeralNavigation)
                .ThenInclude(g => g!.SequenciaDoMunicipioNavigation)
            .Include(n => n.SequenciaDaPropriedadeNavigation)
            .FirstOrDefaultAsync(n => n.SequenciaDaNotaFiscal == notaFiscalId);

        if (nota == null)
            throw new InvalidOperationException($"Nota fiscal {notaFiscalId} não encontrada");

        var cliente = nota.SequenciaDoGeralNavigation;
        var municipioCliente = cliente?.SequenciaDoMunicipioNavigation;
        var ufCliente = municipioCliente?.Uf ?? "SP";

        // 2. Carregar dados do item baseado no tipo
        DadosItem? dadosItem = null;
        DadosClassificacaoFiscal? dadosClassFiscal = null;
        string ncm = string.Empty;
        string unidade = string.Empty;
        string descricao = string.Empty;

        var tipoItem = (TipoItem)request.TipoItem;

        switch (tipoItem)
        {
            case TipoItem.Produto:
                var produto = await _context.Produtos
                    .Include(p => p.SequenciaDaClassificacaoNavigation)
                        .ThenInclude(c => c!.ClassTribNavigation)
                    .Include(p => p.SequenciaDaUnidadeNavigation)
                    .FirstOrDefaultAsync(p => p.SequenciaDoProduto == request.SequenciaDoItem);

                if (produto == null)
                    throw new InvalidOperationException($"Produto {request.SequenciaDoItem} não encontrado");

                descricao = produto.Descricao;
                unidade = produto.SequenciaDaUnidadeNavigation?.Descricao ?? "UN";
                ncm = produto.SequenciaDaClassificacaoNavigation?.Ncm.ToString() ?? "";

                dadosItem = new DadosItem
                {
                    SeqItem = produto.SequenciaDoProduto,
                    DescricaoProduto = produto.Descricao,
                    Sucata = produto.Sucata,
                    Importado = false, // produto.Importado não existe no model
                    Usado = produto.Usado,
                    MaterialAdquiridoTerceiro = produto.MaterialAdquiridoDeTerceiro,
                    TipoProduto = produto.TipoDoProduto,
                    SeqClassificacao = produto.SequenciaDaClassificacao
                };

                if (produto.SequenciaDaClassificacaoNavigation != null)
                {
                    var cf = produto.SequenciaDaClassificacaoNavigation;
                    dadosClassFiscal = new DadosClassificacaoFiscal
                    {
                        SeqClassificacao = cf.SequenciaDaClassificacao,
                        NCM = cf.Ncm.ToString(),
                        Inativo = cf.Inativo,
                        DescricaoNCM = cf.DescricaoDoNcm,
                        ReducaoBaseCalculo = cf.ReducaoDeBaseDeCalculo,
                        AnexoReducao = cf.AnexoDaReducao,
                        ProdutoDiferido = cf.ProdutoDiferido,
                        PercentualIPI = cf.PorcentagemDoIpi,
                        TemConvenio = cf.TemConvenio,
                        // ClassTrib
                        CodigoClassTrib = cf.ClassTribNavigation?.CodigoClassTrib ?? "",
                        CST_IBSCBS = cf.ClassTribNavigation?.CodigoSituacaoTributaria ?? "",
                        PercentualReducaoIBS = cf.ClassTribNavigation?.PercentualReducaoIBS ?? 0,
                        PercentualReducaoCBS = cf.ClassTribNavigation?.PercentualReducaoCBS ?? 0,
                        ValidoParaNFe = cf.ClassTribNavigation?.ValidoParaNFe ?? false
                    };
                }
                break;

            case TipoItem.Conjunto:
                var conjunto = await _context.Conjuntos
                    .Include(c => c.SequenciaDaClassificacaoNavigation)
                        .ThenInclude(cf => cf!.ClassTribNavigation)
                    .FirstOrDefaultAsync(c => c.SequenciaDoConjunto == request.SequenciaDoItem);

                if (conjunto == null)
                    throw new InvalidOperationException($"Conjunto {request.SequenciaDoItem} não encontrado");

                descricao = conjunto.Descricao;
                unidade = "UN";
                ncm = conjunto.SequenciaDaClassificacaoNavigation?.Ncm.ToString() ?? "";

                dadosItem = new DadosItem
                {
                    SeqItem = conjunto.SequenciaDoConjunto,
                    DescricaoProduto = conjunto.Descricao,
                    Sucata = false,
                    Importado = false,
                    Usado = conjunto.Usado,
                    MaterialAdquiridoTerceiro = false,
                    TipoProduto = 0,
                    SeqClassificacao = conjunto.SequenciaDaClassificacao
                };

                if (conjunto.SequenciaDaClassificacaoNavigation != null)
                {
                    var cf = conjunto.SequenciaDaClassificacaoNavigation;
                    dadosClassFiscal = new DadosClassificacaoFiscal
                    {
                        SeqClassificacao = cf.SequenciaDaClassificacao,
                        NCM = cf.Ncm.ToString(),
                        Inativo = cf.Inativo,
                        DescricaoNCM = cf.DescricaoDoNcm,
                        ReducaoBaseCalculo = cf.ReducaoDeBaseDeCalculo,
                        AnexoReducao = cf.AnexoDaReducao,
                        ProdutoDiferido = cf.ProdutoDiferido,
                        PercentualIPI = cf.PorcentagemDoIpi,
                        TemConvenio = cf.TemConvenio,
                        CodigoClassTrib = cf.ClassTribNavigation?.CodigoClassTrib ?? "",
                        CST_IBSCBS = cf.ClassTribNavigation?.CodigoSituacaoTributaria ?? "",
                        PercentualReducaoIBS = cf.ClassTribNavigation?.PercentualReducaoIBS ?? 0,
                        PercentualReducaoCBS = cf.ClassTribNavigation?.PercentualReducaoCBS ?? 0,
                        ValidoParaNFe = cf.ClassTribNavigation?.ValidoParaNFe ?? false
                    };
                }
                break;

            case TipoItem.Peca:
                // Peças usam a mesma tabela de Produtos
                var peca = await _context.Produtos
                    .Include(p => p.SequenciaDaClassificacaoNavigation)
                        .ThenInclude(c => c!.ClassTribNavigation)
                    .Include(p => p.SequenciaDaUnidadeNavigation)
                    .FirstOrDefaultAsync(p => p.SequenciaDoProduto == request.SequenciaDoItem);

                if (peca == null)
                    throw new InvalidOperationException($"Peça (Produto) {request.SequenciaDoItem} não encontrada");

                descricao = peca.Descricao;
                unidade = peca.SequenciaDaUnidadeNavigation?.Descricao ?? "UN";
                ncm = peca.SequenciaDaClassificacaoNavigation?.Ncm.ToString() ?? "";

                dadosItem = new DadosItem
                {
                    SeqItem = peca.SequenciaDoProduto,
                    DescricaoProduto = peca.Descricao,
                    Sucata = peca.Sucata,
                    Importado = false,
                    Usado = peca.Usado,
                    MaterialAdquiridoTerceiro = peca.MaterialAdquiridoDeTerceiro,
                    TipoProduto = peca.TipoDoProduto,
                    SeqClassificacao = peca.SequenciaDaClassificacao
                };

                if (peca.SequenciaDaClassificacaoNavigation != null)
                {
                    var cf = peca.SequenciaDaClassificacaoNavigation;
                    dadosClassFiscal = new DadosClassificacaoFiscal
                    {
                        SeqClassificacao = cf.SequenciaDaClassificacao,
                        NCM = cf.Ncm.ToString(),
                        Inativo = cf.Inativo,
                        DescricaoNCM = cf.DescricaoDoNcm,
                        ReducaoBaseCalculo = cf.ReducaoDeBaseDeCalculo,
                        AnexoReducao = cf.AnexoDaReducao,
                        ProdutoDiferido = cf.ProdutoDiferido,
                        PercentualIPI = cf.PorcentagemDoIpi,
                        TemConvenio = cf.TemConvenio,
                        CodigoClassTrib = cf.ClassTribNavigation?.CodigoClassTrib ?? "",
                        CST_IBSCBS = cf.ClassTribNavigation?.CodigoSituacaoTributaria ?? "",
                        PercentualReducaoIBS = cf.ClassTribNavigation?.PercentualReducaoIBS ?? 0,
                        PercentualReducaoCBS = cf.ClassTribNavigation?.PercentualReducaoCBS ?? 0,
                        ValidoParaNFe = cf.ClassTribNavigation?.ValidoParaNFe ?? false
                    };
                }
                break;

            default:
                throw new ArgumentException($"Tipo de item inválido: {request.TipoItem}");
        }

        // 3. Carregar dados do Cliente
        var dadosCliente = new DadosCliente
        {
            SeqGeral = cliente?.SequenciaDoGeral ?? 0,
            Tipo = cliente?.Tipo ?? 0,
            Revenda = cliente?.Revenda ?? false,
            Isento = cliente?.Isento ?? false,
            Cumulativo = cliente?.Cumulativo ?? false,
            CodigoSuframa = cliente?.CodigoDoSuframa,
            EmpresaProdutor = cliente?.EmpresaProdutor ?? false,
            OrgaoPublico = false, // Não existe no model Geral
            RGIE = cliente?.RgEIe,
            SeqMunicipio = cliente?.SequenciaDoMunicipio ?? 0
        };

        // 4. Carregar dados da Propriedade
        DadosPropriedade? dadosPropriedade = null;
        if (nota.SequenciaDaPropriedadeNavigation != null)
        {
            var prop = nota.SequenciaDaPropriedadeNavigation;
            dadosPropriedade = new DadosPropriedade
            {
                SeqPropriedade = prop.SequenciaDaPropriedade,
                InscricaoEstadual = prop.InscricaoEstadual,
                SeqMunicipio = prop.SequenciaDoMunicipio
            };
        }

        // 5. Carregar ICMS por UF
        var icmsUf = await _context.Icms
            .FirstOrDefaultAsync(i => i.Uf == ufCliente);

        DadosICMSUF? dadosIcmsUf = null;
        if (icmsUf != null)
        {
            dadosIcmsUf = new DadosICMSUF
            {
                UF = icmsUf.Uf,
                PercentagemICMS = icmsUf.PorcentagemDeIcms,
                AliquotaInterestadual = icmsUf.AliquotaInterEstadual
            };
        }

        // 6. Carregar MVA/IVA por NCM e UF (se houver)
        DadosMVA? dadosMva = null;
        if (!string.IsNullOrEmpty(ncm))
        {
            // O NCM no banco é inteiro, precisamos converter para buscar na tabela MVA
            // A tabela MVA usa ID_MVA que pode ser o NCM ou parte dele
            if (int.TryParse(ncm, out int ncmInt))
            {
                var mva = await _context.Mvas
                    .FirstOrDefaultAsync(m => m.IdMva == ncmInt && m.Uf == ufCliente);
                
                if (mva != null)
                {
                    dadosMva = new DadosMVA
                    {
                        NCM = ncm,
                        UF = mva.Uf,
                        IVA = mva.Iva
                    };
                }
            }
        }

        // 7. Preparar dados de processamento
        var dadosProcessamento = new DadosProcessamentoItem
        {
            SeqNotaFiscal = notaFiscalId,
            SeqItem = request.SequenciaDoItem,
            SeqGeral = cliente?.SequenciaDoGeral ?? 0,
            SeqPropriedade = nota.SequenciaDaPropriedade,
            SeqClassificacao = dadosItem?.SeqClassificacao ?? 0,
            Quantidade = request.Quantidade,
            ValorUnitario = request.ValorUnitario,
            ValorDesconto = request.Desconto,
            ValorFrete = request.ValorFrete,
            NCM = ncm,
            MaterialAdquiridoTerceiro = dadosItem?.MaterialAdquiridoTerceiro ?? false
        };

        // 8. Criar contexto completo
        var contexto = _itemService.CriarContexto(
            dadosProcessamento,
            dadosItem,
            dadosClassFiscal,
            dadosCliente,
            dadosPropriedade,
            dadosIcmsUf,
            dadosMva,
            ufCliente
        );

        // 9. Processar item (DIFERENTE para Produto vs Conjunto/Peça)
        var resultado = _itemService.ProcessarItem(tipoItem, dadosProcessamento, contexto);

        // 10. Converter para DTO de retorno
        return new CalculoImpostoResultDto
        {
            ValorTotal = resultado.ValorTotal,
            CFOP = resultado.CFOP,
            CST = resultado.CST,
            
            // ICMS
            BaseCalculoICMS = resultado.BaseCalculoICMS,
            AliquotaICMS = resultado.AliquotaICMS,
            ValorICMS = resultado.ValorICMS,
            PercentualReducao = resultado.PercentualReducao,
            Diferido = resultado.Diferido,
            
            // IPI
            AliquotaIPI = resultado.AliquotaIPI,
            ValorIPI = resultado.ValorIPI,
            
            // PIS
            BaseCalculoPIS = resultado.BaseCalculoPIS,
            AliquotaPIS = resultado.AliquotaPIS,
            ValorPIS = resultado.ValorPIS,
            
            // COFINS
            BaseCalculoCOFINS = resultado.BaseCalculoCOFINS,
            AliquotaCOFINS = resultado.AliquotaCOFINS,
            ValorCOFINS = resultado.ValorCOFINS,
            
            // ST
            IVA = resultado.IVA,
            BaseCalculoST = resultado.BaseCalculoST,
            AliquotaICMSST = resultado.AliquotaICMSST,
            ValorICMSST = resultado.ValorICMSST,
            
            // IBS/CBS
            ValorIBS = resultado.ValorIBS,
            ValorCBS = resultado.ValorCBS,
            
            // Totais
            ValorTributo = resultado.ValorTributo,
            
            // Informações adicionais
            NCM = ncm,
            Unidade = unidade,
            DescricaoItem = descricao
        };
    }
}
