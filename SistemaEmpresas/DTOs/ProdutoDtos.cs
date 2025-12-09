namespace SistemaEmpresas.DTOs;

// DTO para listagem de produtos (resumido)
public class ProdutoListDto
{
    public int SequenciaDoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string CodigoDeBarras { get; set; } = string.Empty;
    public string GrupoProduto { get; set; } = string.Empty;
    public string SubGrupoProduto { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public decimal QuantidadeNoEstoque { get; set; }
    public decimal QuantidadeMinima { get; set; }
    public decimal ValorDeCusto { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal MargemDeLucro { get; set; }
    public string Localizacao { get; set; } = string.Empty;
    public bool Inativo { get; set; }
    public bool EMateriaPrima { get; set; }
    public DateTime? UltimaCompra { get; set; }
    public DateTime? UltimoMovimento { get; set; }
    
    // Indicadores visuais
    public bool EstoqueCritico => QuantidadeNoEstoque < QuantidadeMinima && QuantidadeMinima > 0;
}

// DTO completo para visualização/edição
public class ProdutoDto
{
    public int SequenciaDoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string CodigoDeBarras { get; set; } = string.Empty;
    
    // Classificação
    public short SequenciaDoGrupoProduto { get; set; }
    public string GrupoProduto { get; set; } = string.Empty;
    public short SequenciaDoSubGrupoProduto { get; set; }
    public string SubGrupoProduto { get; set; } = string.Empty;
    public short SequenciaDaUnidade { get; set; }
    public string Unidade { get; set; } = string.Empty;
    public short SequenciaDaClassificacao { get; set; }
    public string ClassificacaoFiscal { get; set; } = string.Empty;
    public string Ncm { get; set; } = string.Empty;
    public decimal PercentualIpi { get; set; } // % IPI da classificação fiscal
    
    // Indicadores de vínculo ClassTrib (IBS/CBS)
    public bool TemClassTrib { get; set; } // Indica se a Classificação Fiscal tem ClassTrib vinculado
    public string CodigoClassTrib { get; set; } = string.Empty; // Código ClassTrib se vinculado
    public string DescricaoClassTrib { get; set; } = string.Empty; // Descrição do ClassTrib se vinculado
    
    // Estoque
    public decimal QuantidadeNoEstoque { get; set; }
    public decimal QuantidadeMinima { get; set; }
    public decimal QuantidadeContabil { get; set; }
    public decimal QuantidadeFisica { get; set; }
    public string Localizacao { get; set; } = string.Empty;
    
    // Valores
    public decimal ValorDeCusto { get; set; }
    public decimal MargemDeLucro { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal CustoMedio { get; set; }
    public decimal ValorDeLista { get; set; }
    public decimal ValorContabilAtual { get; set; }
    
    // Características
    public bool EMateriaPrima { get; set; }
    public short TipoDoProduto { get; set; }
    public decimal Peso { get; set; }
    public bool PesoOk { get; set; } // Peso Conferido
    public string Medida { get; set; } = string.Empty;
    public string MedidaFinal { get; set; } = string.Empty;
    public bool Industrializacao { get; set; }
    public bool Importado { get; set; }
    public bool MaterialAdquiridoDeTerceiro { get; set; }
    public bool Sucata { get; set; }
    public bool Obsoleto { get; set; }
    
    // Flags
    public bool Inativo { get; set; }
    public bool Usado { get; set; }
    public bool UsadoNoProjeto { get; set; }
    public bool Lance { get; set; }
    public bool ERegulador { get; set; }
    public bool TravaReceita { get; set; }
    public bool ReceitaConferida { get; set; }
    public bool NaoSairNoRelatorio { get; set; }
    public bool NaoSairNoChecklist { get; set; }
    public bool MostrarReceitaSecundaria { get; set; }
    public bool NaoMostrarReceita { get; set; }
    public bool ConferidoPeloContabil { get; set; } // NCM Conferido pela Contabilidade
    public bool MpInicial { get; set; } // M.Prima Inicial
    
    // Datas
    public DateTime? UltimaCompra { get; set; }
    public DateTime? UltimoMovimento { get; set; }
    public DateTime? UltimaCotacao { get; set; } // Últ. Cotação
    public DateTime? DataDaContagem { get; set; }
    public DateTime? DataDaAlteracao { get; set; }
    
    // Outros
    public int UltimoFornecedor { get; set; }
    public string NomeFornecedor { get; set; } = string.Empty;
    public string ParteDoPivo { get; set; } = string.Empty;
    public int ModeloDoLance { get; set; }
    public string Detalhes { get; set; } = string.Empty;
    public string UsuarioDaAlteracao { get; set; } = string.Empty;
    
    // Controle de produção
    public decimal SeparadoMontar { get; set; }
    public decimal CompradosAguardando { get; set; }
    
    // Último Balanço (aba Contabilidade)
    public decimal QuantidadeBalanco { get; set; }
}

// DTO para criação/atualização
public class ProdutoCreateUpdateDto
{
    public string Descricao { get; set; } = string.Empty;
    public string CodigoDeBarras { get; set; } = string.Empty;
    public short SequenciaDoGrupoProduto { get; set; }
    public short SequenciaDoSubGrupoProduto { get; set; }
    public short SequenciaDaUnidade { get; set; }
    public short SequenciaDaClassificacao { get; set; }
    public decimal QuantidadeMinima { get; set; }
    public string Localizacao { get; set; } = string.Empty;
    public decimal ValorDeCusto { get; set; }
    public decimal MargemDeLucro { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorDeLista { get; set; }
    public bool EMateriaPrima { get; set; }
    public short TipoDoProduto { get; set; }
    public decimal Peso { get; set; }
    public bool PesoOk { get; set; } // Peso Conferido
    public string Medida { get; set; } = string.Empty;
    public string MedidaFinal { get; set; } = string.Empty;
    public bool Industrializacao { get; set; }
    public bool Importado { get; set; }
    public bool MaterialAdquiridoDeTerceiro { get; set; }
    public bool Sucata { get; set; }
    public bool Obsoleto { get; set; }
    public bool Inativo { get; set; }
    public bool Usado { get; set; }
    public bool UsadoNoProjeto { get; set; }
    public bool Lance { get; set; }
    public bool ERegulador { get; set; }
    public bool TravaReceita { get; set; }
    public bool NaoSairNoRelatorio { get; set; }
    public bool NaoSairNoChecklist { get; set; }
    public bool MostrarReceitaSecundaria { get; set; }
    public bool NaoMostrarReceita { get; set; }
    public bool ConferidoPeloContabil { get; set; } // NCM Conferido pela Contabilidade
    public bool MpInicial { get; set; } // M.Prima Inicial
    public string ParteDoPivo { get; set; } = string.Empty;
    public int ModeloDoLance { get; set; }
    public string Detalhes { get; set; } = string.Empty;
}

// DTO para filtros de busca
public class ProdutoFiltroDto
{
    public string? Busca { get; set; }
    public short? GrupoProduto { get; set; }
    public short? SubGrupoProduto { get; set; }
    public bool? EMateriaPrima { get; set; }
    public bool? EstoqueCritico { get; set; }
    public bool IncluirInativos { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

// DTO para combo/select
public class ProdutoComboDto
{
    public int SequenciaDoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string CodigoDeBarras { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public string Ncm { get; set; } = string.Empty;
    public string Cfop { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal QuantidadeNoEstoque { get; set; }
}
