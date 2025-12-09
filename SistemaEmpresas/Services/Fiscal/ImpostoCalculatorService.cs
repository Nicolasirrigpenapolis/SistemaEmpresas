using System;
using SistemaEmpresas.Models.Fiscal;

namespace SistemaEmpresas.Services.Fiscal
{
    /// <summary>
    /// Serviço de Cálculo de Impostos - Réplica COMPLETA do CalculaImposto do IRRIG.BAS
    /// Linhas 2276-3114 do VB6
    /// </summary>
    public class ImpostoCalculatorService
    {
        // Regiões do Brasil (usadas para Convênio 52/91)
        private const string NORTE = "AC AP AM PA RO RR TO";
        private const string NORDESTE = "AL BA CE MA PB PE PI RN SE";
        private const string CENTRO_OESTE = "DF GO MT MS";
        private const string SUDESTE = "MG RJ SP";
        private const string SUL = "PR RS SC";

        // Constantes da Reforma Tributária (IBS/CBS) - alíquotas de transição 2026
        private const decimal RTC_ALIQ_IBS = 0.001m;  // 0.1%
        private const decimal RTC_ALIQ_CBS = 0.009m;  // 0.9%

        // Variável auxiliar para PIS/COFINS (ICMSAux no VB6)
        private decimal _icmsAux;

        /// <summary>
        /// Prepara o contexto calculando todas as flags necessárias
        /// Replica a lógica das linhas 2276-2440 do VB6
        /// </summary>
        public void PrepararContexto(ContextoCalculoCompleto ctx)
        {
            // Validações básicas
            if (ctx.Item == null) return;
            if (ctx.ClassFiscal == null) return;
            if (ctx.ClassFiscal.Inativo)
            {
                throw new InvalidOperationException($"NCM {ctx.ClassFiscal.NCM} está INATIVO e não pode ser utilizado!");
            }

            // Flags do Item
            ctx.Sucata = ctx.Item.Sucata;
            ctx.Importado = ctx.Item.Importado;
            ctx.ProdTerceiro = ctx.Item.MaterialAdquiridoTerceiro;
            ctx.Imobilizado = ctx.Item.TipoProduto;

            // Matéria Prima: produto de terceiro OU usado (exceto conjuntos)
            if (ctx.Tabela == 1) // Produto
            {
                ctx.MateriaPrima = ctx.Item.MaterialAdquiridoTerceiro || ctx.Item.Usado;
            }
            else
            {
                ctx.MateriaPrima = false;
            }

            // Flags do Cliente
            ctx.Revenda = ctx.Cliente?.Revenda ?? false;
            ctx.Isento = ctx.Cliente?.Isento ?? false;
            ctx.Cumulativo = ctx.Cliente?.Cumulativo ?? false;
            ctx.OrgaoPublico = ctx.Cliente?.OrgaoPublico ?? false;
            ctx.EmpresaProdutor = ctx.Cliente?.EmpresaProdutor ?? false;
            ctx.Suframa = !string.IsNullOrEmpty(ctx.Cliente?.CodigoSuframa);

            // Flags da Classificação Fiscal
            ctx.Convenio = ctx.ClassFiscal?.TemConvenio ?? false;
            ctx.ProdutoDiferido = ctx.ClassFiscal?.ProdutoDiferido ?? false;
            ctx.Reducao = ctx.ClassFiscal?.ReducaoBaseCalculo ?? false;
            ctx.AliqIPI = ctx.ClassFiscal?.PercentualIPI ?? 0;

            // UF e flags de localização
            ctx.ForaDoEstado = ctx.UF != "SP";
            ctx.ForaDoPais = ctx.UF == "EX";

            // Contribuinte e Produtor Paulista
            if (ctx.SeqProp > 0 && ctx.Propriedade != null)
            {
                ctx.ProdutorPaulista = ctx.UF == "SP";
                ctx.Contribuinte = !string.IsNullOrEmpty(ctx.Propriedade.InscricaoEstadual);
            }
            else
            {
                ctx.Contribuinte = false;
                ctx.ProdutorPaulista = false;
            }

            // Substituição Tributária: se tem IVA cadastrado > 0
            ctx.Substituicao = (ctx.MVA?.IVA ?? 0) > 0;
            ctx.IVAOriginal = ctx.MVA?.IVA ?? 0;

            // Alíquotas do ICMS
            ctx.AliqICMS = ctx.ICMSUF?.PercentagemICMS ?? 18m;
            ctx.AliquotaInterestadual = ctx.ICMSUF?.AliquotaInterestadual ?? 12m;

            // Regra especial: Pessoa Física fora do estado sem propriedade = sem redução
            if (ctx.Cliente?.Tipo == 0 && ctx.SeqProp == 0 && ctx.UF != "SP")
            {
                ctx.Reducao = false;
            }

            // Calcula BCRed e AliqRed baseado no Convênio 52/91
            CalcularReducaoConvenio(ctx);
        }

        /// <summary>
        /// Calcula Base de Cálculo Reduzida conforme Convênio ICMS 52/91
        /// Replica a lógica das linhas 2440-2600 do VB6
        /// </summary>
        private void CalcularReducaoConvenio(ContextoCalculoCompleto ctx)
        {
            if (!ctx.Reducao) return;

            bool isNorteNordesteCO = NORTE.Contains(ctx.UF) || NORDESTE.Contains(ctx.UF) || 
                                      CENTRO_OESTE.Contains(ctx.UF) || ctx.UF == "ES";
            bool isSulSudeste = SUL.Contains(ctx.UF) || SUDESTE.Contains(ctx.UF);
            int anexo = ctx.ClassFiscal?.AnexoReducao ?? 0;

            if (!ctx.EmpresaProdutor)
            {
                if (ctx.Contribuinte)
                {
                    if (anexo == 0) // Anexo I
                    {
                        if (isNorteNordesteCO)
                        {
                            ctx.BCRed = 73.43m;
                            ctx.AliqRed = 26.57m;
                        }
                        else if (isSulSudeste)
                        {
                            ctx.BCRed = 73.33m;
                            ctx.AliqRed = 26.67m;
                        }
                    }
                    else // Anexo II
                    {
                        if (isNorteNordesteCO)
                        {
                            ctx.BCRed = 58.57m;
                            ctx.AliqRed = 41.43m;
                        }
                        else if (isSulSudeste)
                        {
                            ctx.BCRed = 58.33m;
                            ctx.AliqRed = 41.67m;
                        }
                        if (ctx.UF == "SP")
                        {
                            ctx.BCRed = 46.67m;
                            ctx.AliqRed = 53.33m;
                        }
                    }
                }
                else // Não contribuinte
                {
                    if (anexo == 0) // Anexo I
                    {
                        if (isNorteNordesteCO)
                        {
                            ctx.BCRed = 73.43m;
                            ctx.AliqRed = 26.57m;
                        }
                        else if (isSulSudeste)
                        {
                            ctx.BCRed = 73.33m;
                            ctx.AliqRed = 26.67m;
                        }
                    }
                    else // Anexo II
                    {
                        if (ctx.ForaDoEstado)
                        {
                            if (isNorteNordesteCO)
                            {
                                ctx.BCRed = 58.57m;
                                ctx.AliqRed = 41.43m;
                            }
                            else
                            {
                                ctx.BCRed = 58.33m;
                                ctx.AliqRed = 41.67m;
                            }
                        }
                        else // Dentro do estado (SP)
                        {
                            ctx.BCRed = 46.67m;
                            ctx.AliqRed = 53.33m;
                        }
                    }
                }
            }
            else // Empresa Produtor
            {
                if (anexo == 0) // Anexo I
                {
                    if (isNorteNordesteCO)
                    {
                        ctx.BCRed = 73.43m;
                        ctx.AliqRed = 26.57m;
                    }
                    else if (isSulSudeste)
                    {
                        ctx.BCRed = 73.33m;
                        ctx.AliqRed = 26.67m;
                    }
                }
                else // Anexo II
                {
                    if (isNorteNordesteCO)
                    {
                        ctx.BCRed = 58.57m;
                        ctx.AliqRed = 41.43m;
                    }
                    else if (isSulSudeste)
                    {
                        ctx.BCRed = 58.33m;
                        ctx.AliqRed = 41.67m;
                    }
                    if (ctx.UF == "SP")
                    {
                        ctx.BCRed = 46.67m;
                        ctx.AliqRed = 53.33m;
                    }
                }
            }
        }

        /// <summary>
        /// Calcula o imposto conforme o parâmetro Oq (1-19)
        /// Réplica COMPLETA do Select Case do VB6
        /// </summary>
        public object CalculaImposto(ContextoCalculoCompleto ctx)
        {
            switch (ctx.Oq)
            {
                case 1: return CalcularCFOP(ctx);
                case 2: return CalcularReducaoBC(ctx);
                case 3: return CalcularAliqICMS(ctx);
                case 4: return CalcularAliqIPI(ctx);
                case 5: return CalcularCST(ctx);
                case 6: return CalcularBCICMS(ctx);
                case 7: return CalcularValorICMS(ctx);
                case 8: return CalcularValorIPI(ctx);
                case 9: return CalcularDiferido(ctx);
                case 10: return CalcularValorPIS(ctx);
                case 11: return CalcularValorCOFINS(ctx);
                case 12: return CalcularIVA(ctx);
                case 13: return CalcularBCST(ctx);
                case 14: return CalcularValorST(ctx);
                case 15: return CalcularAliqST(ctx);
                case 16: return CalcularIBS(ctx);
                case 17: return CalcularCBS(ctx);
                case 18: return CalcularCodigoClassTrib(ctx);
                case 19: return CalcularCSTIBSCBS(ctx);
                default: return 0;
            }
        }

        #region Case 1 - CFOP

        /// <summary>
        /// Case 1 - CFOP (Código Fiscal de Operações e Prestações)
        /// Réplica das linhas 2644-2696 do VB6
        /// </summary>
        private string CalcularCFOP(ContextoCalculoCompleto ctx)
        {
            string ncm = ctx.ClassFiscal?.NCM ?? "";

            if (!ctx.ForaDoPais)
            {
                // SUFRAMA
                if (ctx.Suframa && !ctx.MateriaPrima) return "6109";
                if (ctx.Suframa && ctx.MateriaPrima) return "6110";

                // Sucata
                if (!ctx.ForaDoEstado && ctx.Sucata) return "5101";
                if (ctx.ForaDoEstado && ctx.Sucata) return "6101";

                // NCM 85071090 (Bateria) com ST
                if (ctx.ForaDoEstado && ctx.Substituicao && ctx.MateriaPrima && ncm == "85071090") return "6403";
                if (!ctx.ForaDoEstado && ctx.Substituicao && ctx.MateriaPrima && ncm == "85071090") return "5403";

                // Venda para não contribuinte fora do estado (DIFAL)
                if (ctx.ForaDoEstado && ctx.Cliente?.Tipo == 0 && ctx.SeqProp == 0) return "6108";

                // Matéria Prima (produto de terceiro)
                if (ctx.MateriaPrima && ctx.ForaDoEstado && ctx.Imobilizado != 4) return "6102";
                if (!ctx.ForaDoEstado && ctx.MateriaPrima && ctx.Imobilizado != 4) return "5102";

                // Ativo Imobilizado
                if (ctx.Imobilizado == 4 && !ctx.ForaDoEstado) return "5551";
                if (ctx.Imobilizado == 4 && ctx.ForaDoEstado) return "6551";

                // Revenda com Substituição
                if (ctx.Revenda && ctx.Substituicao && !ctx.ForaDoEstado && !ctx.MateriaPrima) return "5401";
                if (ctx.Revenda && ctx.Substituicao && ctx.ForaDoEstado && !ctx.MateriaPrima) return "6401";

                // Produção Própria
                if (!ctx.MateriaPrima && !ctx.ForaDoEstado) return "5101";
                if (!ctx.MateriaPrima && ctx.ForaDoEstado) return "6101";
            }
            else // Exterior
            {
                if (!ctx.MateriaPrima) return "7101";
                if (ctx.MateriaPrima) return "7102";
            }

            return "5102"; // Default
        }

        #endregion

        #region Case 2 - Redução BC

        /// <summary>
        /// Case 2 - Percentual de Redução da Base de Cálculo
        /// Réplica das linhas 2697-2712 do VB6
        /// </summary>
        private decimal CalcularReducaoBC(ContextoCalculoCompleto ctx)
        {
            if (ctx.OrgaoPublico && !ctx.ForaDoEstado) return 0;
            if (!ctx.ForaDoEstado && ctx.Sucata) return 0;
            if (!ctx.Reducao) return 0;
            if (ctx.ProdutoDiferido && ctx.ProdutorPaulista && !(ctx.Item?.Usado ?? false)) return 0;

            // Convênio 80% para usado
            if (ctx.Convenio && (ctx.Item?.Usado ?? false))
            {
                return 80m;
            }

            return ctx.AliqRed;
        }

        #endregion

        #region Case 3 - % ICMS

        /// <summary>
        /// Case 3 - Alíquota do ICMS
        /// Réplica das linhas 2713-2763 do VB6
        /// </summary>
        private decimal CalcularAliqICMS(ContextoCalculoCompleto ctx)
        {
            string ncm = ctx.ClassFiscal?.NCM ?? "";

            if (ctx.Suframa) return 0;
            if (ctx.Imobilizado == 4) return 0;
            if (ctx.OrgaoPublico && !ctx.ForaDoEstado) return 0;

            // Rebocador Aeronáutico - Sul/Sudeste = 12%
            if ((SUL.Contains(ctx.UF) || SUDESTE.Contains(ctx.UF)) && ncm == "84271090") return 12m;

            // Conjunto Gerador - NCMs especiais dentro do estado
            if (!ctx.ForaDoEstado)
            {
                if (ncm == "85021110" || ncm == "85021210" || ncm == "85016400" ||
                    ncm == "85444200" || ncm == "85389010" || ncm == "90308990" || ncm == "85365090")
                {
                    return 12m;
                }
                if (ncm == "85016100") return 18m;
            }

            // PJ Isenta com redução e não importado
            if (ctx.Cliente?.Tipo == 1 && ctx.Isento && !ctx.Reducao && !ctx.Importado)
            {
                return ctx.AliqICMS;
            }

            if (!ctx.ForaDoEstado && ctx.Sucata) return 0;
            if (ctx.Substituicao && ctx.MateriaPrima && ncm == "85071090") return 0;
            if (ctx.ProdutoDiferido && ctx.ProdutorPaulista && !(ctx.Item?.Usado ?? false)) return 0;

            // Importado
            if (ctx.Importado && !ctx.ForaDoEstado && !ctx.Reducao) return 18m;
            if (ctx.Importado && ctx.ForaDoEstado && !ctx.Reducao) return 4m;

            // Usado com Convênio ou com Redução
            if ((ctx.Item?.Usado ?? false) && ctx.Convenio) return ctx.AliqICMS;
            if (ctx.Reducao) return ctx.AliqICMS;

            return ctx.AliqICMS;
        }

        #endregion

        #region Case 4/8 - IPI

        /// <summary>
        /// Case 4 - Alíquota do IPI
        /// Réplica das linhas 2764-2781 do VB6
        /// </summary>
        private decimal CalcularAliqIPI(ContextoCalculoCompleto ctx)
        {
            if (!ctx.ForaDoEstado && ctx.Sucata) return 0;
            if (ctx.MateriaPrima) return 0;
            if (ctx.Suframa) return 0;
            if (ctx.AliqIPI == 0) return 0;
            if (ctx.Item?.Usado ?? false) return 0;
            if (ctx.Revenda) return 0;

            return ctx.AliqIPI;
        }

        /// <summary>
        /// Case 8 - Valor do IPI
        /// Réplica das linhas 2764-2781 do VB6
        /// </summary>
        private decimal CalcularValorIPI(ContextoCalculoCompleto ctx)
        {
            decimal aliq = CalcularAliqIPI(ctx);
            if (aliq == 0) return 0;
            if (ctx.OrgaoPublico && !ctx.ForaDoEstado) return 0;

            // Base do IPI = Valor Total + Frete
            return Math.Round((ctx.VrTotal + ctx.VFrete) * (aliq / 100m), 2);
        }

        #endregion

        #region Case 5 - CST

        /// <summary>
        /// Case 5 - Código de Situação Tributária
        /// Réplica das linhas 2782-2812 do VB6
        /// </summary>
        private string CalcularCST(ContextoCalculoCompleto ctx)
        {
            string ncm = ctx.ClassFiscal?.NCM ?? "";
            bool usado = ctx.Item?.Usado ?? false;

            if (ctx.Suframa) return "040";
            if (ctx.ProdutorPaulista && ctx.ProdutoDiferido && !usado) return "051";
            if (!ctx.ForaDoEstado && ctx.Sucata) return "051"; // Sucata dentro UF = Diferido
            if (ctx.Reducao && !(ctx.Revenda && ctx.Substituicao && !ctx.MateriaPrima)) return "020";
            if (ctx.OrgaoPublico && !ctx.ForaDoEstado) return "040";
            if (ctx.Importado && !ctx.ForaDoEstado && ctx.Sucata) return "151";
            if (!ctx.ForaDoEstado && ctx.Sucata) return "051";
            if (ctx.Importado && ctx.ForaDoEstado && ctx.Sucata) return "100";
            if (ctx.ForaDoEstado && ctx.Sucata) return "000";
            if (ctx.Importado && ctx.ForaDoPais) return "141";
            if (ctx.ForaDoPais) return "041";
            if (ctx.Importado && ctx.ProdutorPaulista && ctx.ProdutoDiferido && !usado) return "151";
            if (ctx.ProdutorPaulista && ctx.ProdutoDiferido && !usado) return "051";
            if (ctx.Importado && ctx.Revenda && ctx.Substituicao && !ctx.MateriaPrima) return "110";
            if (ctx.Importado && ctx.Substituicao && ctx.MateriaPrima) return "160";
            if (ctx.Substituicao && ctx.MateriaPrima && ncm == "85071090") return "060";
            if (ctx.Substituicao && !ctx.Revenda && !ctx.MateriaPrima) return "000";
            if (ctx.Importado && ctx.Substituicao && !ctx.Revenda && !ctx.MateriaPrima) return "100";
            if (ctx.Importado && ctx.Reducao) return "020";
            if (ctx.Importado && !ctx.Substituicao) return "100";
            if (ctx.Imobilizado == 4) return "040";
            if (ctx.Revenda && ctx.Substituicao && !ctx.MateriaPrima && !ctx.Reducao) return "010";
            if (ctx.Revenda && ctx.Substituicao && !ctx.MateriaPrima && ctx.Reducao) return "070";

            return "000"; // Tributada integralmente
        }

        #endregion

        #region Case 6 - BC ICMS

        /// <summary>
        /// Case 6 - Base de Cálculo do ICMS
        /// Réplica das linhas 2813-2847 do VB6
        /// </summary>
        private decimal CalcularBCICMS(ContextoCalculoCompleto ctx)
        {
            string ncm = ctx.ClassFiscal?.NCM ?? "";
            bool usado = ctx.Item?.Usado ?? false;

            if (ctx.Suframa) return 0;
            if (ctx.OrgaoPublico && !ctx.ForaDoEstado) return 0;
            if (ctx.Imobilizado == 4) return 0;
            if (!ctx.ForaDoEstado && ctx.Sucata) return 0;
            if (ctx.Substituicao && ctx.MateriaPrima && ncm == "85071090") return 0;
            if (ctx.ProdutoDiferido && ctx.ProdutorPaulista && !usado) return 0;

            decimal BC = 0;

            // Adiciona IPI na base se aplicável
            if (!ctx.SemIPI && ctx.AliqIPI > 0 && !ctx.Suframa && !usado && 
                ctx.Tabela == 1 && !ctx.Revenda && !ctx.MateriaPrima)
            {
                BC = Math.Round((ctx.VrTotal + ctx.VFrete) * (ctx.AliqIPI / 100m), 2);
            }

            // Usado com Convênio: redução de 80%
            if (usado && ctx.Convenio)
            {
                BC = Math.Round((BC + ctx.VrTotal + ctx.VFrete + ctx.VrAdicional) * 20m / 100m, 2);
                return BC;
            }

            // Com Redução
            if (ctx.Reducao)
            {
                BC = Math.Round((BC + ctx.VrTotal + ctx.VFrete + ctx.VrAdicional) * (ctx.BCRed / 100m), 2);
            }
            else
            {
                BC = Math.Round(BC + ctx.VrTotal + ctx.VFrete + ctx.VrAdicional, 2);
            }

            return BC;
        }

        #endregion

        #region Case 7 - Valor ICMS

        /// <summary>
        /// Case 7 - Valor do ICMS
        /// Réplica das linhas 2848-2896 do VB6
        /// </summary>
        private decimal CalcularValorICMS(ContextoCalculoCompleto ctx)
        {
            string ncm = ctx.ClassFiscal?.NCM ?? "";
            bool usado = ctx.Item?.Usado ?? false;

            // NCMs especiais - Conjunto Gerador dentro do estado
            if (!ctx.ForaDoEstado)
            {
                if (ncm == "85021110" || ncm == "85021210" || ncm == "85016400" ||
                    ncm == "85444200" || ncm == "85389010" || ncm == "90308990" || ncm == "85365090")
                {
                    _icmsAux = Math.Round(ctx.VrTotal * 0.12m, 2);
                    return _icmsAux;
                }
                if (ncm == "85016100")
                {
                    _icmsAux = Math.Round(ctx.VrTotal * 0.18m, 2);
                    return _icmsAux;
                }
            }

            // Rebocador Aeronáutico
            if ((SUL.Contains(ctx.UF) || SUDESTE.Contains(ctx.UF)) && ncm == "84271090")
            {
                _icmsAux = Math.Round(ctx.VrTotal * 0.12m, 2);
                return _icmsAux;
            }

            // Isenções
            if (ctx.Suframa) { _icmsAux = 0; return 0; }
            if (ctx.OrgaoPublico && !ctx.ForaDoEstado) { _icmsAux = 0; return 0; }
            if (ctx.Imobilizado == 4) { _icmsAux = 0; return 0; }
            if (!ctx.ForaDoEstado && ctx.Sucata) { _icmsAux = 0; return 0; }
            if (ctx.Substituicao && ctx.MateriaPrima && ncm == "85071090") { _icmsAux = 0; return 0; }
            if (ctx.ProdutoDiferido && ctx.ProdutorPaulista && !usado) { _icmsAux = 0; return 0; }

            decimal BC = 0;

            // Adiciona IPI na base
            if (!ctx.SemIPI && ctx.AliqIPI > 0 && !ctx.Suframa && !usado &&
                ctx.Tabela == 1 && !ctx.Revenda && !ctx.MateriaPrima)
            {
                BC = Math.Round((ctx.VrTotal + ctx.VFrete) * (ctx.AliqIPI / 100m), 2);
            }

            // Usado com Convênio
            if (usado && ctx.Convenio)
            {
                BC = Math.Round((BC + ctx.VrTotal + ctx.VFrete + ctx.VrAdicional) * 20m / 100m, 2);
                _icmsAux = Math.Round(BC * (ctx.AliqICMS / 100m), 2);
                return _icmsAux;
            }

            // Com Redução
            if (ctx.Reducao)
            {
                BC = Math.Round((BC + ctx.VrTotal + ctx.VFrete + ctx.VrAdicional) * (ctx.BCRed / 100m), 2);
                _icmsAux = Math.Round(BC * (ctx.AliqICMS / 100m), 2);
                return _icmsAux;
            }

            // Sem Redução
            BC = Math.Round(BC + ctx.VrTotal + ctx.VFrete + ctx.VrAdicional, 2);

            // Contribuinte
            if (ctx.Contribuinte)
            {
                if (!ctx.Importado)
                {
                    _icmsAux = Math.Round(BC * (ctx.ICMSUF?.PercentagemICMS ?? 18m) / 100m, 2);
                }
                else
                {
                    _icmsAux = Math.Round(BC * 0.04m, 2); // Importado = 4%
                }
            }
            else
            {
                if (!ctx.Importado)
                {
                    _icmsAux = Math.Round(BC * (ctx.AliqICMS / 100m), 2);
                }
                else
                {
                    _icmsAux = Math.Round(BC * 0.04m, 2);
                }
            }

            // Importado dentro da UF = 18%
            if (ctx.Importado && !ctx.ForaDoEstado)
            {
                _icmsAux = Math.Round(BC * 0.18m, 2);
            }

            // Importado fora da UF = 4%
            if (ctx.Importado && ctx.ForaDoEstado)
            {
                _icmsAux = Math.Round(BC * 0.04m, 2);
            }

            // PJ Isenta sem redução e não importado
            if (ctx.Cliente?.Tipo == 1 && ctx.Isento && !ctx.Reducao && !ctx.Importado)
            {
                _icmsAux = Math.Round(BC * (ctx.AliqICMS / 100m), 2);
            }

            return _icmsAux;
        }

        #endregion

        #region Case 9 - Diferido

        /// <summary>
        /// Case 9 - Flag de Diferimento
        /// Réplica das linhas 2897-2906 do VB6
        /// </summary>
        private int CalcularDiferido(ContextoCalculoCompleto ctx)
        {
            bool usado = ctx.Item?.Usado ?? false;

            if (ctx.Sucata && !ctx.ForaDoEstado) return 1;
            if (ctx.ProdutoDiferido && ctx.ProdutorPaulista && !usado) return 1;
            if (ctx.ProdutoDiferido && !ctx.ForaDoEstado) return 1;

            return 0;
        }

        #endregion

        #region Case 10/11 - PIS/COFINS

        /// <summary>
        /// Case 10 - Valor do PIS
        /// Réplica das linhas 2907-2929 do VB6
        /// NCMs especiais: 84248*, 73090*, 87162000 ou Conjuntos/Peças = alíquotas diferenciadas
        /// </summary>
        private decimal CalcularValorPIS(ContextoCalculoCompleto ctx)
        {
            if (ctx.Suframa) return 0;
            if (ctx.Imobilizado == 4) return 0;

            string ncm = ctx.ClassFiscal?.NCM ?? "";
            bool ncmEspecial = ncm.StartsWith("84248") || ncm.StartsWith("73090") || ncm == "87162000";
            bool isConjuntoPeca = ctx.Tabela == 2 || ctx.Tabela == 3;

            // Com desconto ou frete: base = VrTotal
            if (ctx.VrAdicional < 0 || ctx.VFrete > 0)
            {
                if ((ncmEspecial || isConjuntoPeca) && !ctx.ProdTerceiro)
                {
                    return Math.Round(ctx.VrTotal * 0.02m, 2); // 2%
                }
                else
                {
                    return Math.Round(ctx.VrTotal * 0.0165m, 2); // 1.65%
                }
            }
            else
            {
                // Sem desconto: base = VrTotal - ICMS
                if ((ncmEspecial || isConjuntoPeca) && !ctx.ProdTerceiro)
                {
                    return Math.Round((ctx.VrTotal - _icmsAux) * 0.02m, 2); // 2%
                }
                else
                {
                    return Math.Round((ctx.VrTotal - _icmsAux) * 0.0165m, 2); // 1.65%
                }
            }
        }

        /// <summary>
        /// Case 11 - Valor do COFINS
        /// Réplica das linhas 2930-2952 do VB6
        /// </summary>
        private decimal CalcularValorCOFINS(ContextoCalculoCompleto ctx)
        {
            if (ctx.Suframa) return 0;
            if (ctx.Imobilizado == 4) return 0;

            string ncm = ctx.ClassFiscal?.NCM ?? "";
            bool ncmEspecial = ncm.StartsWith("84248") || ncm.StartsWith("73090") || ncm == "87162000";
            bool isConjuntoPeca = ctx.Tabela == 2 || ctx.Tabela == 3;

            // Com desconto ou frete: base = VrTotal
            if (ctx.VrAdicional < 0 || ctx.VFrete > 0)
            {
                if ((ncmEspecial || isConjuntoPeca) && !ctx.ProdTerceiro)
                {
                    return Math.Round(ctx.VrTotal * 0.096m, 2); // 9.6%
                }
                else
                {
                    return Math.Round(ctx.VrTotal * 0.076m, 2); // 7.6%
                }
            }
            else
            {
                if ((ncmEspecial || isConjuntoPeca) && !ctx.ProdTerceiro)
                {
                    return Math.Round((ctx.VrTotal - _icmsAux) * 0.096m, 2); // 9.6%
                }
                else
                {
                    return Math.Round((ctx.VrTotal - _icmsAux) * 0.076m, 2); // 7.6%
                }
            }
        }

        #endregion

        #region Case 12/13/14/15 - Substituição Tributária

        /// <summary>
        /// Case 12 - IVA (Margem de Valor Agregado) Ajustado
        /// Réplica das linhas 2953-2983 do VB6
        /// Fórmula: (((1 + (IVA/100)) * (1 - (AliqICMS/100)) / (1 - (AliqInterestadual/100))) - 1) * 100
        /// </summary>
        private decimal CalcularIVA(ContextoCalculoCompleto ctx)
        {
            if (!ctx.Revenda || !ctx.Substituicao || ctx.MateriaPrima) return 0;
            if (ctx.IVAOriginal == 0) return 0;
            if (ctx.Importado && ctx.MateriaPrima) return 0;

            decimal aliqICMS = ctx.Importado ? (ctx.ForaDoEstado ? 4m : 18m) : (ctx.ICMSUF?.PercentagemICMS ?? 18m);
            decimal aliqInter = ctx.AliquotaInterestadual;

            if (aliqInter == 0) aliqInter = 12m; // Fallback

            decimal iva = ((1 + (ctx.IVAOriginal / 100m)) * (1 - (aliqICMS / 100m)) / (1 - (aliqInter / 100m)) - 1) * 100m;
            return Math.Round(iva, 2);
        }

        /// <summary>
        /// Case 13 - Base de Cálculo do ICMS ST
        /// Réplica das linhas 2984-3010 do VB6
        /// </summary>
        private decimal CalcularBCST(ContextoCalculoCompleto ctx)
        {
            if (!ctx.Revenda || !ctx.Substituicao || ctx.MateriaPrima) return 0;
            if (ctx.IVAOriginal == 0) return 0;
            if (ctx.Importado && ctx.MateriaPrima) return 0;

            decimal BC = ctx.VrTotal;
            bool usado = ctx.Item?.Usado ?? false;

            // Com IPI
            if (ctx.AliqIPI > 0 && !usado && ctx.Tabela == 1 && !ctx.MateriaPrima)
            {
                BC = Math.Round(ctx.VrTotal * (ctx.AliqIPI / 100m + 1), 2);
            }

            decimal iva = CalcularIVA(ctx);
            return Math.Round((BC + ctx.VFrete - Math.Abs(ctx.VrAdicional)) * ((iva / 100m) + 1), 2);
        }

        /// <summary>
        /// Case 14 - Valor do ICMS ST
        /// Réplica das linhas 3011-3063 do VB6
        /// </summary>
        private decimal CalcularValorST(ContextoCalculoCompleto ctx)
        {
            if (!ctx.Revenda || !ctx.Substituicao || ctx.MateriaPrima) return 0;
            if (ctx.IVAOriginal == 0) return 0;
            if (ctx.Importado && ctx.MateriaPrima) return 0;

            bool usado = ctx.Item?.Usado ?? false;
            decimal BC = 0;

            // Usado com Convênio
            if (usado && ctx.Convenio)
            {
                BC = ctx.VrTotal * 0.2m;
            }
            // Com Redução
            else if (ctx.Reducao && !ctx.Isento)
            {
                BC = (ctx.VrTotal) * (ctx.BCRed / 100m);
            }
            else
            {
                BC = ctx.VrTotal + ctx.VFrete - Math.Abs(ctx.VrAdicional);
            }

            decimal VrICMS;
            decimal iva;
            decimal aliqInter = ctx.AliquotaInterestadual > 0 ? ctx.AliquotaInterestadual : 12m;

            if (!ctx.Importado)
            {
                if (ctx.Reducao)
                {
                    VrICMS = BC * (ctx.AliqICMS / 100m);
                    iva = ((1 + (ctx.IVAOriginal / 100m)) * (1 - (ctx.AliqICMS / 100m)) / (1 - (aliqInter / 100m)) - 1) * 100m;
                }
                else
                {
                    VrICMS = BC * ((ctx.ICMSUF?.PercentagemICMS ?? 18m) / 100m);
                    iva = ((1 + (ctx.IVAOriginal / 100m)) * (1 - ((ctx.ICMSUF?.PercentagemICMS ?? 18m) / 100m)) / (1 - (aliqInter / 100m)) - 1) * 100m;
                }
            }
            else
            {
                decimal aliqImp = ctx.ForaDoEstado ? 4m : 18m;
                VrICMS = BC * (aliqImp / 100m);
                iva = ((1 + (ctx.IVAOriginal / 100m)) * (1 - (aliqImp / 100m)) / (1 - (aliqInter / 100m)) - 1) * 100m;
            }

            // Com IPI
            if (ctx.AliqIPI > 0 && !usado && ctx.Tabela == 1 && !ctx.MateriaPrima)
            {
                BC = ctx.VrTotal * (ctx.AliqIPI / 100m + 1);
                BC = BC + ctx.VFrete - Math.Abs(ctx.VrAdicional);
            }

            decimal valorST = BC * ((iva / 100m) + 1);
            valorST = valorST * (aliqInter / 100m);
            valorST = Math.Round(valorST - VrICMS, 2);

            return valorST > 0 ? valorST : 0;
        }

        /// <summary>
        /// Case 15 - Alíquota do ICMS ST
        /// </summary>
        private decimal CalcularAliqST(ContextoCalculoCompleto ctx)
        {
            if (!ctx.Revenda || !ctx.Substituicao || ctx.MateriaPrima) return 0;
            if (ctx.IVAOriginal == 0) return 0;
            if (ctx.Importado && ctx.MateriaPrima) return 0;

            return ctx.AliquotaInterestadual > 0 ? ctx.AliquotaInterestadual : 12m;
        }

        #endregion

        #region Case 16/17/18/19 - Reforma Tributária (IBS/CBS)

        /// <summary>
        /// Case 16 - Valor do IBS (Imposto sobre Bens e Serviços)
        /// Réplica das linhas 3064-3082 do VB6
        /// IBS = VrTotal * 0.1% * (1 - ReducaoIBS)
        /// </summary>
        private decimal CalcularIBS(ContextoCalculoCompleto ctx)
        {
            decimal reducao = ctx.ClassFiscal?.PercentualReducaoIBS ?? 0;
            // Se > 1, é percentual (ex: 60), converte para fração (0.60)
            if (reducao > 1) reducao = reducao / 100m;

            return Math.Round(ctx.VrTotal * RTC_ALIQ_IBS * (1 - reducao), 2);
        }

        /// <summary>
        /// Case 17 - Valor do CBS (Contribuição sobre Bens e Serviços)
        /// Réplica das linhas 3083-3099 do VB6
        /// CBS = VrTotal * 0.9% * (1 - ReducaoCBS)
        /// </summary>
        private decimal CalcularCBS(ContextoCalculoCompleto ctx)
        {
            decimal reducao = ctx.ClassFiscal?.PercentualReducaoCBS ?? 0;
            if (reducao > 1) reducao = reducao / 100m;

            return Math.Round(ctx.VrTotal * RTC_ALIQ_CBS * (1 - reducao), 2);
        }

        /// <summary>
        /// Case 18 - Código ClassTrib (para XML da NFe)
        /// </summary>
        private string CalcularCodigoClassTrib(ContextoCalculoCompleto ctx)
        {
            if (string.IsNullOrEmpty(ctx.ClassFiscal?.CodigoClassTrib)) return "";
            if (ctx.ClassFiscal.ValidoParaNFe == false) return ""; // Não usar ClassTrib inválido
            return ctx.ClassFiscal.CodigoClassTrib;
        }

        /// <summary>
        /// Case 19 - CST do IBS/CBS
        /// </summary>
        private string CalcularCSTIBSCBS(ContextoCalculoCompleto ctx)
        {
            if (!string.IsNullOrEmpty(ctx.ClassFiscal?.CST_IBSCBS))
            {
                return ctx.ClassFiscal.CST_IBSCBS;
            }
            return "90"; // Outros
        }

        #endregion
    }
}
