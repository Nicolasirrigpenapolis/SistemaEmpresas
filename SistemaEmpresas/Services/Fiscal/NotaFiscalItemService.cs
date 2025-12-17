using System;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services.Fiscal
{
    /// <summary>
    /// Serviço para processamento de itens da Nota Fiscal
    /// Réplica das funções ProcessaProdutos, ProcessaConjuntos e ProcessaPecas do VB6
    /// </summary>
    public class NotaFiscalItemService
    {
        private readonly ImpostoCalculatorService _calculador;

        // NCMs especiais para PIS/COFINS diferenciado (Produtos)
        // 84248* = Pulverizadores, 7309* = Silos, 87162000 = Carretas agrícolas
        private static readonly string[] NCMs_PIS_COFINS_ESPECIAL = { "84248", "7309", "87162000" };

        // Alíquotas padrão
        private const decimal ALIQ_PIS_PADRAO = 1.65m;
        private const decimal ALIQ_COFINS_PADRAO = 7.6m;
        private const decimal ALIQ_PIS_ESPECIAL = 2.0m;
        private const decimal ALIQ_COFINS_ESPECIAL = 9.6m;
        private const decimal REDUCAO_PIS_COFINS = 48.1m;

        public NotaFiscalItemService()
        {
            _calculador = new ImpostoCalculatorService();
        }

        public NotaFiscalItemService(ImpostoCalculatorService calculador)
        {
            _calculador = calculador;
        }

        #region Método Público Principal

        /// <summary>
        /// Processa um item da nota fiscal (Produto, Conjunto ou Peça)
        /// </summary>
        /// <param name="tipo">Tipo do item (Produto=1, Conjunto=2, Peça=3)</param>
        /// <param name="dados">Dados de entrada para processamento</param>
        /// <param name="contexto">Contexto completo já preparado</param>
        /// <returns>Resultado com todos os impostos calculados</returns>
        public ResultadoCalculo ProcessarItem(TipoItem tipo, DadosProcessamentoItem dados, ContextoCalculoCompleto contexto)
        {
            if (dados == null) throw new ArgumentNullException(nameof(dados));
            if (contexto == null) throw new ArgumentNullException(nameof(contexto));

            // Define o tipo de tabela no contexto
            contexto.Tabela = (int)tipo;

            return tipo switch
            {
                TipoItem.Produto => ProcessarProduto(dados, contexto),
                TipoItem.Conjunto => ProcessarConjuntoPeca(dados, contexto),
                TipoItem.Peca => ProcessarConjuntoPeca(dados, contexto),
                _ => throw new ArgumentException($"Tipo de item inválido: {tipo}", nameof(tipo))
            };
        }

        #endregion

        #region ProcessarProduto - Lógica específica com verificação de NCM

        /// <summary>
        /// Processa um PRODUTO - tem lógica especial de PIS/COFINS por NCM
        /// Réplica do ProcessaProdutos do VB6 (NOTAFISC.FRM linhas 5879-6098)
        /// </summary>
        private ResultadoCalculo ProcessarProduto(DadosProcessamentoItem dados, ContextoCalculoCompleto ctx)
        {
            var resultado = new ResultadoCalculo();
            decimal tributos = 0;

            // Valor Total
            resultado.ValorTotal = Math.Round(dados.Quantidade * dados.ValorUnitario, 2);
            ctx.VrTotal = resultado.ValorTotal;
            ctx.VFrete = dados.ValorFrete;
            ctx.VrAdicional = -dados.ValorDesconto; // Desconto é negativo

            // Prepara contexto
            _calculador.PrepararContexto(ctx);

            // ============ CÁLCULOS VIA CalculaImposto ============

            // CST (Case 5)
            ctx.Oq = 5;
            resultado.CST = _calculador.CalculaImposto(ctx)?.ToString() ?? "000";

            // CFOP (Case 1)
            ctx.Oq = 1;
            resultado.CFOP = _calculador.CalculaImposto(ctx)?.ToString() ?? "5102";

            // Base de Cálculo ICMS (Case 6)
            ctx.Oq = 6;
            resultado.BaseCalculoICMS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Valor ICMS (Case 7)
            ctx.Oq = 7;
            resultado.ValorICMS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorICMS;

            // Alíquota ICMS (Case 3)
            ctx.Oq = 3;
            resultado.AliquotaICMS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Alíquota IPI (Case 4)
            ctx.Oq = 4;
            resultado.AliquotaIPI = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Valor IPI (Case 8)
            ctx.Oq = 8;
            resultado.ValorIPI = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorIPI;

            // Diferido (Case 9)
            ctx.Oq = 9;
            resultado.Diferido = Convert.ToInt32(_calculador.CalculaImposto(ctx)) == 1;

            // Percentual Redução (Case 2)
            ctx.Oq = 2;
            resultado.PercentualReducao = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // ============ PIS/COFINS - LÓGICA ESPECIAL POR NCM ============
            // Base = (Valor + Frete - Desconto) - ICMS
            decimal basePisCofins = Math.Round(
                (dados.Quantidade * dados.ValorUnitario + dados.ValorFrete - dados.ValorDesconto) - resultado.ValorICMS, 
                2);

            // Verifica se é NCM especial E não é material de terceiro
            bool ncmEspecial = VerificarNCMEspecialPisCofins(dados.NCM);
            
            if (ncmEspecial && !dados.MaterialAdquiridoTerceiro)
            {
                // NCM especial COM redução de 48.1%
                decimal reducao = Math.Round(basePisCofins * REDUCAO_PIS_COFINS / 100, 2);
                
                // PIS: Base com redução, alíquota 2%
                resultado.BaseCalculoPIS = Math.Round(basePisCofins - reducao, 2);
                resultado.AliquotaPIS = ALIQ_PIS_ESPECIAL;
                resultado.ValorPIS = Math.Round(resultado.BaseCalculoPIS * ALIQ_PIS_ESPECIAL / 100, 2);
                tributos += resultado.ValorPIS;

                // COFINS: Base com redução, alíquota 9.6%
                resultado.BaseCalculoCOFINS = Math.Round(basePisCofins - reducao, 2);
                resultado.AliquotaCOFINS = ALIQ_COFINS_ESPECIAL;
                resultado.ValorCOFINS = Math.Round(resultado.BaseCalculoCOFINS * ALIQ_COFINS_ESPECIAL / 100, 2);
                tributos += resultado.ValorCOFINS;
            }
            else
            {
                // Demais NCMs SEM redução - alíquotas padrão 1.65%/7.6%
                resultado.BaseCalculoPIS = basePisCofins;
                resultado.AliquotaPIS = ALIQ_PIS_PADRAO;
                resultado.ValorPIS = Math.Round(basePisCofins * ALIQ_PIS_PADRAO / 100, 2);
                tributos += resultado.ValorPIS;

                resultado.BaseCalculoCOFINS = basePisCofins;
                resultado.AliquotaCOFINS = ALIQ_COFINS_PADRAO;
                resultado.ValorCOFINS = Math.Round(basePisCofins * ALIQ_COFINS_PADRAO / 100, 2);
                tributos += resultado.ValorCOFINS;
            }

            // ============ SUBSTITUIÇÃO TRIBUTÁRIA ============

            // IVA (Case 12)
            ctx.Oq = 12;
            resultado.IVA = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Base de Cálculo ST (Case 13)
            ctx.Oq = 13;
            resultado.BaseCalculoST = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Valor ICMS ST (Case 14)
            ctx.Oq = 14;
            resultado.ValorICMSST = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorICMSST;

            // Alíquota ICMS ST (Case 15)
            ctx.Oq = 15;
            resultado.AliquotaICMSST = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // ============ IBS/CBS (Reforma Tributária) ============

            // IBS (Case 16)
            ctx.Oq = 16;
            resultado.ValorIBS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorIBS;

            // CBS (Case 17)
            ctx.Oq = 17;
            resultado.ValorCBS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorCBS;

            // Total de tributos
            resultado.ValorTributo = tributos;

            return resultado;
        }

        #endregion

        #region ProcessarConjuntoPeca - Lógica unificada

        /// <summary>
        /// Processa um CONJUNTO ou PEÇA - lógica idêntica para ambos
        /// Sempre usa redução de 48.1% e alíquotas 2%/9.6%
        /// Réplica do ProcessaConjuntos e ProcessaPecas do VB6
        /// </summary>
        private ResultadoCalculo ProcessarConjuntoPeca(DadosProcessamentoItem dados, ContextoCalculoCompleto ctx)
        {
            var resultado = new ResultadoCalculo();
            decimal tributos = 0;

            // Valor Total
            resultado.ValorTotal = Math.Round(dados.Quantidade * dados.ValorUnitario, 2);
            ctx.VrTotal = resultado.ValorTotal;
            ctx.VFrete = dados.ValorFrete;
            ctx.VrAdicional = -dados.ValorDesconto; // Desconto é negativo

            // Prepara contexto
            _calculador.PrepararContexto(ctx);

            // ============ CÁLCULOS VIA CalculaImposto ============

            // CST (Case 5)
            ctx.Oq = 5;
            resultado.CST = _calculador.CalculaImposto(ctx)?.ToString() ?? "000";

            // CFOP (Case 1)
            ctx.Oq = 1;
            resultado.CFOP = _calculador.CalculaImposto(ctx)?.ToString() ?? "5102";

            // Base de Cálculo ICMS (Case 6)
            ctx.Oq = 6;
            resultado.BaseCalculoICMS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Valor ICMS (Case 7)
            ctx.Oq = 7;
            resultado.ValorICMS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorICMS;

            // Alíquota ICMS (Case 3)
            ctx.Oq = 3;
            resultado.AliquotaICMS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Alíquota IPI (Case 4)
            ctx.Oq = 4;
            resultado.AliquotaIPI = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Valor IPI (Case 8)
            ctx.Oq = 8;
            resultado.ValorIPI = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorIPI;

            // Diferido (Case 9)
            ctx.Oq = 9;
            resultado.Diferido = Convert.ToInt32(_calculador.CalculaImposto(ctx)) == 1;

            // Percentual Redução (Case 2)
            ctx.Oq = 2;
            resultado.PercentualReducao = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // ============ PIS/COFINS - SEMPRE COM REDUÇÃO 48.1% ============
            // Conjuntos e Peças SEMPRE usam redução fixa
            // Base = (Valor + Frete - Desconto) - ICMS
            decimal basePisCofins = Math.Round(
                (dados.Quantidade * dados.ValorUnitario + dados.ValorFrete - dados.ValorDesconto) - resultado.ValorICMS, 
                2);

            // Redução de 48.1% (SEMPRE para Conjunto/Peça)
            decimal reducao = Math.Round(basePisCofins * REDUCAO_PIS_COFINS / 100, 2);

            // PIS: Base com redução, alíquota 2%
            resultado.BaseCalculoPIS = Math.Round(basePisCofins - reducao, 2);
            resultado.AliquotaPIS = ALIQ_PIS_ESPECIAL;
            resultado.ValorPIS = Math.Round(resultado.BaseCalculoPIS * ALIQ_PIS_ESPECIAL / 100, 2);
            tributos += resultado.ValorPIS;

            // COFINS: Base com redução, alíquota 9.6%
            resultado.BaseCalculoCOFINS = Math.Round(basePisCofins - reducao, 2);
            resultado.AliquotaCOFINS = ALIQ_COFINS_ESPECIAL;
            resultado.ValorCOFINS = Math.Round(resultado.BaseCalculoCOFINS * ALIQ_COFINS_ESPECIAL / 100, 2);
            tributos += resultado.ValorCOFINS;

            // ============ SUBSTITUIÇÃO TRIBUTÁRIA ============

            // IVA (Case 12)
            ctx.Oq = 12;
            resultado.IVA = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Base de Cálculo ST (Case 13)
            ctx.Oq = 13;
            resultado.BaseCalculoST = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // Valor ICMS ST (Case 14)
            ctx.Oq = 14;
            resultado.ValorICMSST = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorICMSST;

            // Alíquota ICMS ST (Case 15)
            ctx.Oq = 15;
            resultado.AliquotaICMSST = Convert.ToDecimal(_calculador.CalculaImposto(ctx));

            // ============ IBS/CBS (Reforma Tributária) ============

            // IBS (Case 16)
            ctx.Oq = 16;
            resultado.ValorIBS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorIBS;

            // CBS (Case 17)
            ctx.Oq = 17;
            resultado.ValorCBS = Convert.ToDecimal(_calculador.CalculaImposto(ctx));
            tributos += resultado.ValorCBS;

            // Total de tributos
            resultado.ValorTributo = tributos;

            return resultado;
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Verifica se o NCM é especial para PIS/COFINS (redução 48.1%, alíq 2%/9.6%)
        /// NCMs: 84248* (Pulverizadores), 7309* (Silos), 87162000 (Carretas agrícolas)
        /// </summary>
        private bool VerificarNCMEspecialPisCofins(string? ncm)
        {
            if (string.IsNullOrEmpty(ncm)) return false;

            // 84248* - Pulverizadores
            if (ncm.StartsWith("84248")) return true;

            // 7309* - Silos (VB6 usa Mid(NCM,1,4) = "7309")
            if (ncm.StartsWith("7309")) return true;

            // 87162000 - Carretas agrícolas
            if (ncm == "87162000") return true;

            return false;
        }

        /// <summary>
        /// Cria um contexto completo a partir dos dados de processamento
        /// (Helper para facilitar o uso)
        /// </summary>
        public ContextoCalculoCompleto CriarContexto(
            DadosProcessamentoItem dados,
            DadosItem? item,
            DadosClassificacaoFiscal? classFiscal,
            DadosCliente? cliente,
            DadosPropriedade? propriedade,
            DadosICMSUF? icmsUF,
            DadosMVA? mva,
            string uf)
        {
            return new ContextoCalculoCompleto
            {
                SeqItem = dados.SeqItem,
                SeqGeral = dados.SeqGeral,
                SeqProp = dados.SeqPropriedade,
                VrTotal = dados.Quantidade * dados.ValorUnitario,
                VrAdicional = -dados.ValorDesconto,
                VFrete = dados.ValorFrete,
                Item = item,
                ClassFiscal = classFiscal,
                Cliente = cliente,
                Propriedade = propriedade,
                ICMSUF = icmsUF,
                MVA = mva,
                UF = uf
            };
        }

        #endregion

        #region Conversão para DTOs (para gravar no banco via Repository)

        /// <summary>
        /// Converte ResultadoCalculo para ProdutoDaNotaFiscalCreateDto
        /// Para usar com NotaFiscalRepository.AdicionarProdutoAsync()
        /// </summary>
        public DTOs.ProdutoDaNotaFiscalCreateDto ConverterParaProdutoDto(
            DadosProcessamentoItem dados, 
            ResultadoCalculo resultado)
        {
            return new DTOs.ProdutoDaNotaFiscalCreateDto
            {
                SequenciaDoProduto = (int)dados.SeqItem,
                Quantidade = dados.Quantidade,
                ValorUnitario = dados.ValorUnitario,
                ValorTotal = resultado.ValorTotal,
                Desconto = dados.ValorDesconto,
                
                // ICMS
                BaseDeCalculoIcms = resultado.BaseCalculoICMS,
                AliquotaIcms = resultado.AliquotaICMS,
                ValorIcms = resultado.ValorICMS,
                PercentualReducaoIcms = resultado.PercentualReducao,
                Diferido = resultado.Diferido,
                
                // ICMS ST
                BaseDeCalculoSt = resultado.BaseCalculoST,
                AliquotaSt = resultado.AliquotaICMSST,
                ValorIcmsSt = resultado.ValorICMSST,
                Iva = resultado.IVA,
                
                // IPI
                AliquotaIpi = resultado.AliquotaIPI,
                ValorIpi = resultado.ValorIPI,
                
                // PIS/COFINS
                BcPis = resultado.BaseCalculoPIS,
                AliquotaPis = resultado.AliquotaPIS,
                ValorPis = resultado.ValorPIS,
                BcCofins = resultado.BaseCalculoCOFINS,
                AliquotaCofins = resultado.AliquotaCOFINS,
                ValorCofins = resultado.ValorCOFINS,
                
                // Importação
                BaseDeCalculoImportacao = resultado.BaseCalculoImportacao,
                DespesasAduaneiras = resultado.ValorDespesasAduaneiras,
                ImpostoImportacao = resultado.ValorImpostoImportacao,
                Iof = resultado.ValorIOF,
                
                // Outros
                Cfop = short.TryParse(resultado.CFOP, out var cfop) ? cfop : (short)5102,
                Cst = short.TryParse(resultado.CST, out var cst) ? cst : (short)0,
                ValorTributo = resultado.ValorTributo,
                ValorFrete = dados.ValorFrete
            };
        }

        /// <summary>
        /// Converte ResultadoCalculo para ConjuntoDaNotaFiscalCreateDto
        /// Para usar com NotaFiscalRepository.AdicionarConjuntoAsync()
        /// </summary>
        public DTOs.ConjuntoDaNotaFiscalCreateDto ConverterParaConjuntoDto(
            DadosProcessamentoItem dados, 
            ResultadoCalculo resultado)
        {
            return new DTOs.ConjuntoDaNotaFiscalCreateDto
            {
                SequenciaDoConjunto = (int)dados.SeqItem,
                Quantidade = dados.Quantidade,
                ValorUnitario = dados.ValorUnitario,
                ValorTotal = resultado.ValorTotal,
                Desconto = dados.ValorDesconto,
                
                // ICMS
                BaseDeCalculoIcms = resultado.BaseCalculoICMS,
                AliquotaIcms = resultado.AliquotaICMS,
                ValorIcms = resultado.ValorICMS,
                PercentualReducaoIcms = resultado.PercentualReducao,
                Diferido = resultado.Diferido,
                
                // ICMS ST
                BaseDeCalculoSt = resultado.BaseCalculoST,
                AliquotaSt = resultado.AliquotaICMSST,
                ValorIcmsSt = resultado.ValorICMSST,
                Iva = resultado.IVA,
                
                // IPI
                AliquotaIpi = resultado.AliquotaIPI,
                ValorIpi = resultado.ValorIPI,
                
                // PIS/COFINS
                BcPis = resultado.BaseCalculoPIS,
                AliquotaPis = resultado.AliquotaPIS,
                ValorPis = resultado.ValorPIS,
                BcCofins = resultado.BaseCalculoCOFINS,
                AliquotaCofins = resultado.AliquotaCOFINS,
                ValorCofins = resultado.ValorCOFINS,
                
                // Outros
                Cfop = short.TryParse(resultado.CFOP, out var cfop) ? cfop : (short)5102,
                Cst = short.TryParse(resultado.CST, out var cst) ? cst : (short)0,
                ValorTributo = resultado.ValorTributo,
                ValorFrete = dados.ValorFrete
            };
        }

        /// <summary>
        /// Converte ResultadoCalculo para PecaDaNotaFiscalCreateDto
        /// Para usar com NotaFiscalRepository.AdicionarPecaAsync()
        /// </summary>
        public DTOs.PecaDaNotaFiscalCreateDto ConverterParaPecaDto(
            DadosProcessamentoItem dados, 
            ResultadoCalculo resultado)
        {
            return new DTOs.PecaDaNotaFiscalCreateDto
            {
                SequenciaDoProduto = (int)dados.SeqItem, // Peça usa SeqProduto
                Quantidade = dados.Quantidade,
                ValorUnitario = dados.ValorUnitario,
                ValorTotal = resultado.ValorTotal,
                Desconto = dados.ValorDesconto,
                
                // ICMS
                BaseDeCalculoIcms = resultado.BaseCalculoICMS,
                AliquotaIcms = resultado.AliquotaICMS,
                ValorIcms = resultado.ValorICMS,
                PercentualReducaoIcms = resultado.PercentualReducao,
                Diferido = resultado.Diferido,
                
                // ICMS ST
                BaseDeCalculoSt = resultado.BaseCalculoST,
                AliquotaSt = resultado.AliquotaICMSST,
                ValorIcmsSt = resultado.ValorICMSST,
                Iva = resultado.IVA,
                
                // IPI
                AliquotaIpi = resultado.AliquotaIPI,
                ValorIpi = resultado.ValorIPI,
                
                // PIS/COFINS
                BcPis = resultado.BaseCalculoPIS,
                AliquotaPis = resultado.AliquotaPIS,
                ValorPis = resultado.ValorPIS,
                BcCofins = resultado.BaseCalculoCOFINS,
                AliquotaCofins = resultado.AliquotaCOFINS,
                ValorCofins = resultado.ValorCOFINS,
                
                // Outros
                Cfop = short.TryParse(resultado.CFOP, out var cfop) ? cfop : (short)5102,
                Cst = short.TryParse(resultado.CST, out var cst) ? cst : (short)0,
                ValorTributo = resultado.ValorTributo,
                ValorFrete = dados.ValorFrete
            };
        }

        #endregion
    }
}
