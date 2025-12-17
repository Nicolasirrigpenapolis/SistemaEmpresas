namespace SistemaEmpresas.Models;
    /// <summary>
    /// Contexto completo para cálculo de impostos - replica todas as variáveis do VB6
    /// </summary>
    public class ContextoCalculoCompleto
    {
        // Parâmetros de entrada (assinatura original do VB6)
        public long SeqItem { get; set; }
        public long SeqGeral { get; set; }
        public int Oq { get; set; } // 1-19: tipo de cálculo
        public int Tabela { get; set; } // 1=Produto, 2=Conjunto, 3=Peça
        public decimal VrTotal { get; set; }
        public decimal VrAdicional { get; set; } // Desconto (negativo) ou acréscimo
        public long SeqProp { get; set; }
        public long Ncm { get; set; }
        public bool SemIPI { get; set; }
        public string? UFAvulso { get; set; }
        public decimal VFrete { get; set; }

        // Dados carregados das tabelas
        public DadosItem? Item { get; set; }
        public DadosClassificacaoFiscal? ClassFiscal { get; set; }
        public DadosCliente? Cliente { get; set; }
        public DadosPropriedade? Propriedade { get; set; }
        public DadosICMSUF? ICMSUF { get; set; }
        public DadosMVA? MVA { get; set; }
        public string UF { get; set; } = string.Empty;

        // Flags calculadas (variáveis locais do VB6)
        public bool Revenda { get; set; }
        public bool Substituicao { get; set; }
        public bool MateriaPrima { get; set; }
        public bool ForaDoEstado { get; set; }
        public bool ForaDoPais { get; set; }
        public bool Reducao { get; set; }
        public bool Contribuinte { get; set; }
        public bool ProdutorPaulista { get; set; }
        public bool Suframa { get; set; }
        public bool Convenio { get; set; }
        public bool Importado { get; set; }
        public bool ProdutoDiferido { get; set; }
        public bool Sucata { get; set; }
        public bool Isento { get; set; }
        public bool OrgaoPublico { get; set; }
        public bool EmpresaProdutor { get; set; }
        public int Imobilizado { get; set; } // TipoProduto = 4 é Ativo Imobilizado
        public bool ProdTerceiro { get; set; }
        public bool Cumulativo { get; set; }

        // Valores calculados intermediários
        public decimal AliqICMS { get; set; }
        public decimal AliqIPI { get; set; }
        public decimal BCRed { get; set; } // Base de Cálculo Reduzida (percentual)
        public decimal AliqRed { get; set; } // Alíquota de Redução
        public decimal IVAOriginal { get; set; }
        public decimal AliquotaInterestadual { get; set; }

        // Valor auxiliar para PIS/COFINS (ICMSAux no VB6)
        public decimal ICMSAux { get; set; }
    }
