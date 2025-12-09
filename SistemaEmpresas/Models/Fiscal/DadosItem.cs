namespace SistemaEmpresas.Models.Fiscal
{
    /// <summary>
    /// Dados do Item (Produto, Conjunto ou Peça) - Tb1 no VB6
    /// </summary>
    public class DadosItem
    {
        public long SeqItem { get; set; }
        public string DescricaoProduto { get; set; } = string.Empty;
        public bool Sucata { get; set; }
        public bool Importado { get; set; }
        public bool Usado { get; set; }
        public bool MaterialAdquiridoTerceiro { get; set; } // MateriaPrima
        public int TipoProduto { get; set; } // 4 = Ativo Imobilizado
        public long SeqClassificacao { get; set; } // NCM
    }

    /// <summary>
    /// Dados da Classificação Fiscal + ClassTrib - TB2 no VB6
    /// </summary>
    public class DadosClassificacaoFiscal
    {
        public long SeqClassificacao { get; set; }
        public string NCM { get; set; } = string.Empty;
        public bool Inativo { get; set; }
        public string DescricaoNCM { get; set; } = string.Empty;
        public bool ReducaoBaseCalculo { get; set; }
        public int AnexoReducao { get; set; } // 0 = Anexo I, 1 = Anexo II
        public bool ProdutoDiferido { get; set; }
        public decimal PercentualIPI { get; set; }
        public bool TemConvenio { get; set; }
        
        // ClassTrib (IBS/CBS)
        public string CodigoClassTrib { get; set; } = string.Empty;
        public string CST_IBSCBS { get; set; } = string.Empty;
        public decimal PercentualReducaoIBS { get; set; }
        public decimal PercentualReducaoCBS { get; set; }
        public bool ValidoParaNFe { get; set; }
    }

    /// <summary>
    /// Dados do Cliente/Destinatário - Tb3 no VB6
    /// </summary>
    public class DadosCliente
    {
        public long SeqGeral { get; set; }
        public int Tipo { get; set; } // 0 = PF, 1 = PJ
        public bool Revenda { get; set; }
        public bool Isento { get; set; }
        public bool Cumulativo { get; set; }
        public string? CodigoSuframa { get; set; }
        public bool EmpresaProdutor { get; set; }
        public bool OrgaoPublico { get; set; }
        public string? RGIE { get; set; } // Inscrição Estadual
        public long SeqMunicipio { get; set; }
    }

    /// <summary>
    /// Dados da Propriedade Rural - TB4 no VB6
    /// </summary>
    public class DadosPropriedade
    {
        public long SeqPropriedade { get; set; }
        public string? InscricaoEstadual { get; set; }
        public long SeqMunicipio { get; set; }
    }

    /// <summary>
    /// Dados do ICMS por UF - TB6 no VB6
    /// </summary>
    public class DadosICMSUF
    {
        public string UF { get; set; } = string.Empty;
        public decimal PercentagemICMS { get; set; }
        public decimal AliquotaInterestadual { get; set; }
    }

    /// <summary>
    /// Dados do IVA/MVA por UF e NCM - TabelaIVA no VB6
    /// </summary>
    public class DadosMVA
    {
        public string NCM { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public decimal IVA { get; set; }
    }
}
