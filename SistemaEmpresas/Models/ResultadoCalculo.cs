namespace SistemaEmpresas.Models;
    /// <summary>
    /// Tipo de item sendo processado
    /// </summary>
    public enum TipoItem
    {
        Produto = 1,
        Conjunto = 2,
        Peca = 3
    }

    /// <summary>
    /// Dados de entrada para processamento de um item
    /// </summary>
    public class DadosProcessamentoItem
    {
        // Identificadores
        public long SeqNotaFiscal { get; set; }
        public long SeqItemNotaFiscal { get; set; } // SeqProdutoNF, SeqConjuntoNF ou SeqPecaNF
        public long SeqItem { get; set; } // SeqProduto, SeqConjunto ou SeqProduto(peça)
        public long SeqGeral { get; set; } // Cliente
        public long SeqPropriedade { get; set; }
        public long SeqClassificacao { get; set; }

        // Valores
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorFrete { get; set; }

        // Dados adicionais (opcionais, preenchidos pelo serviço)
        public string? NCM { get; set; }
        public bool MaterialAdquiridoTerceiro { get; set; }
    }

    /// <summary>
    /// Resultado completo do cálculo de impostos de um item
    /// Réplica dos campos gravados no banco pelo VB6
    /// </summary>
    public class ResultadoCalculo
    {
        // Valores base
        public decimal ValorTotal { get; set; }
        
        // CFOP e CST
        public string CFOP { get; set; } = string.Empty;
        public string CST { get; set; } = string.Empty;
        
        // ICMS
        public decimal BaseCalculoICMS { get; set; }
        public decimal AliquotaICMS { get; set; }
        public decimal ValorICMS { get; set; }
        public decimal PercentualReducao { get; set; }
        public bool Diferido { get; set; }
        
        // IPI
        public decimal AliquotaIPI { get; set; }
        public decimal ValorIPI { get; set; }
        
        // PIS
        public decimal BaseCalculoPIS { get; set; }
        public decimal AliquotaPIS { get; set; }
        public decimal ValorPIS { get; set; }
        
        // COFINS
        public decimal BaseCalculoCOFINS { get; set; }
        public decimal AliquotaCOFINS { get; set; }
        public decimal ValorCOFINS { get; set; }
        
        // Substituição Tributária
        public decimal IVA { get; set; }
        public decimal BaseCalculoST { get; set; }
        public decimal AliquotaICMSST { get; set; }
        public decimal ValorICMSST { get; set; }
        
        // IBS/CBS (Reforma Tributária)
        public decimal ValorIBS { get; set; }
        public decimal ValorCBS { get; set; }
        
        // Total de tributos (soma de todos)
        public decimal ValorTributo { get; set; }
        
        // Campos de importação (apenas para Produtos)
        public decimal BaseCalculoImportacao { get; set; }
        public decimal ValorDespesasAduaneiras { get; set; }
        public decimal ValorImpostoImportacao { get; set; }
        public decimal ValorIOF { get; set; }
    }
