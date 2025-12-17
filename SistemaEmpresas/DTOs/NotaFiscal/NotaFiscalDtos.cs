using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.DTOs;

#region DTOs para Listagem

/// <summary>
/// DTO para listagem de notas fiscais no grid
/// </summary>
public class NotaFiscalListDto
{
    public int SequenciaDaNotaFiscal { get; set; }
    public int NumeroDaNfe { get; set; }
    public int NumeroDaNotaFiscal { get; set; }
    public DateTime? DataDeEmissao { get; set; }
    
    // Cliente
    public int SequenciaDoGeral { get; set; }
    public string NomeDoCliente { get; set; } = string.Empty;
    public string DocumentoCliente { get; set; } = string.Empty;
    
    // Natureza
    public short SequenciaDaNatureza { get; set; }
    public string DescricaoNatureza { get; set; } = string.Empty;
    
    // Propriedade/Filial
    public short SequenciaDaPropriedade { get; set; }
    public string NomePropriedade { get; set; } = string.Empty;
    
    // Valores
    public decimal ValorTotalDaNotaFiscal { get; set; }
    public decimal ValorTotalDosProdutos { get; set; }
    public decimal ValorTotalDoIcms { get; set; }
    
    // Status
    public bool NotaCancelada { get; set; }
    public bool Transmitido { get; set; }
    public bool Autorizado { get; set; }
    public string ChaveDeAcessoDaNfe { get; set; } = string.Empty;
    
    // Tipo
    public short TipoDeNota { get; set; }
    public string TipoDeNotaDescricao => TipoDeNota switch
    {
        0 => "Saída",
        1 => "Entrada",
        2 => "Serviço",
        _ => "Outro"
    };
    
    // NFe
    public bool NfeComplementar { get; set; }
    public bool NotaDeDevolucao { get; set; }
}

#endregion

#region DTO Completo para Visualização/Edição

/// <summary>
/// DTO completo para visualização e edição de nota fiscal
/// </summary>
public class NotaFiscalDto
{
    // Identificação
    public int SequenciaDaNotaFiscal { get; set; }
    public int NumeroDaNfe { get; set; }
    public int NumeroDaNfse { get; set; }
    public int NumeroDaNotaFiscal { get; set; }
    
    // Datas
    public DateTime? DataDeEmissao { get; set; }
    public DateTime? DataDeSaida { get; set; }
    public DateTime? HoraDaSaida { get; set; }
    
    // Cliente/Destinatário
    public int SequenciaDoGeral { get; set; }
    public string NomeDoCliente { get; set; } = string.Empty;
    public string DocumentoCliente { get; set; } = string.Empty;
    public string EnderecoCliente { get; set; } = string.Empty;
    public string CidadeCliente { get; set; } = string.Empty;
    public string UfCliente { get; set; } = string.Empty;
    public string InscricaoCliente { get; set; } = string.Empty;
    
    // Propriedade/Filial
    public short SequenciaDaPropriedade { get; set; }
    public string NomePropriedade { get; set; } = string.Empty;
    
    // Natureza de Operação
    public short SequenciaDaNatureza { get; set; }
    public string DescricaoNatureza { get; set; } = string.Empty;
    public string CfopNatureza { get; set; } = string.Empty;
    
    // Classificação Fiscal
    public short SequenciaDaClassificacao { get; set; }
    public string DescricaoClassificacao { get; set; } = string.Empty;
    
    // Tipo de Cobrança
    public short SequenciaDaCobranca { get; set; }
    public string DescricaoCobranca { get; set; } = string.Empty;
    
    // Transportadora
    public bool TransportadoraAvulsa { get; set; }
    public int SequenciaDaTransportadora { get; set; }
    public string NomeTransportadora { get; set; } = string.Empty;
    public string NomeDaTransportadoraAvulsa { get; set; } = string.Empty;
    public string DocumentoDaTransportadora { get; set; } = string.Empty;
    public string IeDaTransportadora { get; set; } = string.Empty;
    public string EnderecoDaTransportadora { get; set; } = string.Empty;
    public int MunicipioDaTransportadora { get; set; }
    public string NomeMunicipioTransportadora { get; set; } = string.Empty;
    public string PlacaDoVeiculo { get; set; } = string.Empty;
    public string UfDoVeiculo { get; set; } = string.Empty;
    public string CodigoDaAntt { get; set; } = string.Empty;
    
    // Frete
    public string? Frete { get; set; }
    public decimal ValorDoFrete { get; set; }
    
    // Volumes
    public int Volume { get; set; }
    public string Especie { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Numeracao { get; set; } = string.Empty;
    public decimal PesoBruto { get; set; }
    public decimal PesoLiquido { get; set; }
    
    // Valores Totais
    public decimal ValorTotalDosProdutos { get; set; }
    public decimal ValorTotalDosConjuntos { get; set; }
    public decimal ValorTotalDasPecas { get; set; }
    public decimal ValorTotalDosServicos { get; set; }
    public decimal ValorTotalDaNotaFiscal { get; set; }
    
    // Impostos
    public decimal ValorTotalDaBaseDeCalculo { get; set; }
    public decimal ValorTotalDoIcms { get; set; }
    public decimal ValorTotalDaBaseSt { get; set; }
    public decimal ValorTotalDoIcmsSt { get; set; }
    public decimal ValorTotalIpiDosProdutos { get; set; }
    public decimal ValorTotalIpiDosConjuntos { get; set; }
    public decimal ValorTotalIpiDasPecas { get; set; }
    public decimal ValorTotalDoPis { get; set; }
    public decimal ValorTotalDoCofins { get; set; }
    public decimal ValorDoImpostoDeRenda { get; set; }
    public decimal AliquotaDoIss { get; set; }
    public bool ReterIss { get; set; }
    public decimal ValorTotalDoTributo { get; set; }
    public decimal ValorTotalDaImportacao { get; set; }
    
    // IBS/CBS (Reforma Tributária)
    public decimal ValorTotalIbs { get; set; }
    public decimal ValorTotalCbs { get; set; }
    
    // Outros Valores
    public decimal ValorDoSeguro { get; set; }
    public decimal OutrasDespesas { get; set; }
    
    // Valores Usados
    public decimal ValorTotalDeProdutosUsados { get; set; }
    public decimal ValorTotalConjuntosUsados { get; set; }
    public decimal ValorTotalDasPecasUsadas { get; set; }
    
    // Pagamento
    public string FormaDePagamento { get; set; } = string.Empty;
    public bool ContraApresentacao { get; set; }
    public short Fechamento { get; set; }
    public decimal ValorDoFechamento { get; set; }
    
    // Observações
    public string Historico { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    
    // Tipo e Status
    public short TipoDeNota { get; set; }
    public bool NotaCancelada { get; set; }
    public bool CanceladaNoLivro { get; set; }
    public bool NotaFiscalAvulsa { get; set; }
    public bool OcultarValorUnitario { get; set; }
    
    // NFe
    public bool Transmitido { get; set; }
    public bool Autorizado { get; set; }
    public bool Imprimiu { get; set; }
    public string ChaveDeAcessoDaNfe { get; set; } = string.Empty;
    public string ProtocoloDeAutorizacaoNfe { get; set; } = string.Empty;
    public string DataEHoraDaNfe { get; set; } = string.Empty;
    public string NumeroDoReciboDaNfe { get; set; } = string.Empty;
    
    // NFe Referenciada/Complementar/Devolução
    public bool NfeComplementar { get; set; }
    public string ChaveAcessoNfeReferenciada { get; set; } = string.Empty;
    public bool NotaDeDevolucao { get; set; }
    public string ChaveDaDevolucao { get; set; } = string.Empty;
    public string ChaveDaDevolucao2 { get; set; } = string.Empty;
    public string ChaveDaDevolucao3 { get; set; } = string.Empty;
    public short FinNfe { get; set; }
    public bool NovoLayout { get; set; }
    
    // NFSe
    public string ReciboNfse { get; set; } = string.Empty;
    
    // Conjunto Avulso
    public bool ConjuntoAvulso { get; set; }
    public string DescricaoConjuntoAvulso { get; set; } = string.Empty;
    
    // Relacionamentos
    public int SequenciaDoPedido { get; set; }
    public int SequenciaDoVendedor { get; set; }
    public string NomeVendedor { get; set; } = string.Empty;
    public int SequenciaDoMovimento { get; set; }
    public int NumeroDoContrato { get; set; }
    public int NotaDeVenda { get; set; }
    public bool Refaturamento { get; set; }
    public bool Financiamento { get; set; }
    
    // Itens da Nota
    public List<ProdutoDaNotaFiscalDto> Produtos { get; set; } = new();
    public List<ConjuntoDaNotaFiscalDto> Conjuntos { get; set; } = new();
    public List<PecaDaNotaFiscalDto> Pecas { get; set; } = new();
    public List<ServicoDaNotaFiscalDto> Servicos { get; set; } = new();
    public List<ParcelaNotaFiscalDto> Parcelas { get; set; } = new();
}

#endregion

#region DTOs para Itens da Nota Fiscal

/// <summary>
/// DTO para produtos da nota fiscal
/// </summary>
public class ProdutoDaNotaFiscalDto
{
    public int SequenciaDoProdutoDaNotaFiscal { get; set; }
    public int SequenciaDoProduto { get; set; }
    public string DescricaoProduto { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public string Ncm { get; set; } = string.Empty;
    public string Cfop { get; set; } = string.Empty;
    public string Cst { get; set; } = string.Empty;
    
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    
    // ICMS
    public decimal BaseDeCalculoIcms { get; set; }
    public decimal AliquotaIcms { get; set; }
    public decimal ValorIcms { get; set; }
    
    // ICMS ST
    public decimal BaseDeCalculoSt { get; set; }
    public decimal AliquotaSt { get; set; }
    public decimal ValorIcmsSt { get; set; }
    
    // IPI
    public decimal BaseDeCalculoIpi { get; set; }
    public decimal AliquotaIpi { get; set; }
    public decimal ValorIpi { get; set; }
    
    // PIS/COFINS
    public decimal AliquotaPis { get; set; }
    public decimal ValorPis { get; set; }
    public decimal AliquotaCofins { get; set; }
    public decimal ValorCofins { get; set; }
    
    // IBS/CBS (Reforma Tributária)
    public decimal ValorIbs { get; set; }
    public decimal ValorCbs { get; set; }
    
    public bool Usado { get; set; }
    public string InformacoesAdicionais { get; set; } = string.Empty;
}

/// <summary>
/// DTO para conjuntos da nota fiscal
/// </summary>
public class ConjuntoDaNotaFiscalDto
{
    public int SequenciaDoConjuntoDaNotaFiscal { get; set; }
    public int SequenciaDoConjunto { get; set; }
    public string DescricaoConjunto { get; set; } = string.Empty;
    
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorIpi { get; set; }
    
    // IBS/CBS (Reforma Tributária)
    public decimal ValorIbs { get; set; }
    public decimal ValorCbs { get; set; }
    
    public bool Usado { get; set; }
}

/// <summary>
/// DTO para peças da nota fiscal
/// </summary>
public class PecaDaNotaFiscalDto
{
    public int SequenciaDaPecaDaNotaFiscal { get; set; }
    public int SequenciaDaPeca { get; set; }
    public string DescricaoPeca { get; set; } = string.Empty;
    
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorIpi { get; set; }
    
    // IBS/CBS (Reforma Tributária)
    public decimal ValorIbs { get; set; }
    public decimal ValorCbs { get; set; }
    
    public bool Usado { get; set; }
}

/// <summary>
/// DTO para serviços da nota fiscal
/// </summary>
public class ServicoDaNotaFiscalDto
{
    public int SequenciaDoServicoDaNotaFiscal { get; set; }
    public int SequenciaDoServico { get; set; }
    public string DescricaoServico { get; set; } = string.Empty;
    
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    
    public decimal AliquotaIss { get; set; }
    public decimal ValorIss { get; set; }
}

/// <summary>
/// DTO para parcelas da nota fiscal
/// </summary>
public class ParcelaNotaFiscalDto
{
    public short NumeroDaParcela { get; set; }
    public short Dias { get; set; }
    public DateTime DataDeVencimento { get; set; }
    public decimal Valor { get; set; }
}

#endregion

#region DTOs para Criação de Itens

/// <summary>
/// DTO para criar/atualizar produto da nota fiscal
/// </summary>
public class ProdutoDaNotaFiscalCreateDto
{
    [Required(ErrorMessage = "Produto é obrigatório")]
    public int SequenciaDoProduto { get; set; }
    
    [Required(ErrorMessage = "Quantidade é obrigatória")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public decimal Quantidade { get; set; }
    
    [Required(ErrorMessage = "Valor unitário é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor unitário não pode ser negativo")]
    public decimal ValorUnitario { get; set; }
    
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    
    // ICMS
    public decimal BaseDeCalculoIcms { get; set; }
    public decimal AliquotaIcms { get; set; }
    public decimal ValorIcms { get; set; }
    public decimal PercentualReducaoIcms { get; set; }
    public bool Diferido { get; set; }
    
    // ICMS ST
    public decimal BaseDeCalculoSt { get; set; }
    public decimal AliquotaSt { get; set; }
    public decimal ValorIcmsSt { get; set; }
    public decimal Iva { get; set; }
    
    // IPI
    public decimal AliquotaIpi { get; set; }
    public decimal ValorIpi { get; set; }
    
    // PIS/COFINS
    public decimal BcPis { get; set; }
    public decimal AliquotaPis { get; set; }
    public decimal ValorPis { get; set; }
    public decimal BcCofins { get; set; }
    public decimal AliquotaCofins { get; set; }
    public decimal ValorCofins { get; set; }
    
    // Importação
    public decimal BaseDeCalculoImportacao { get; set; }
    public decimal DespesasAduaneiras { get; set; }
    public decimal ImpostoImportacao { get; set; }
    public decimal Iof { get; set; }
    
    // Outros
    public short Cfop { get; set; }
    public short Cst { get; set; }
    public decimal ValorTributo { get; set; }
    public decimal ValorFrete { get; set; }
}

/// <summary>
/// DTO para criar/atualizar conjunto da nota fiscal
/// </summary>
public class ConjuntoDaNotaFiscalCreateDto
{
    [Required(ErrorMessage = "Conjunto é obrigatório")]
    public int SequenciaDoConjunto { get; set; }
    
    [Required(ErrorMessage = "Quantidade é obrigatória")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public decimal Quantidade { get; set; }
    
    [Required(ErrorMessage = "Valor unitário é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor unitário não pode ser negativo")]
    public decimal ValorUnitario { get; set; }
    
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    
    // ICMS
    public decimal BaseDeCalculoIcms { get; set; }
    public decimal AliquotaIcms { get; set; }
    public decimal ValorIcms { get; set; }
    public decimal PercentualReducaoIcms { get; set; }
    public bool Diferido { get; set; }
    
    // ICMS ST
    public decimal BaseDeCalculoSt { get; set; }
    public decimal AliquotaSt { get; set; }
    public decimal ValorIcmsSt { get; set; }
    public decimal Iva { get; set; }
    
    // IPI
    public decimal AliquotaIpi { get; set; }
    public decimal ValorIpi { get; set; }
    
    // PIS/COFINS
    public decimal BcPis { get; set; }
    public decimal AliquotaPis { get; set; }
    public decimal ValorPis { get; set; }
    public decimal BcCofins { get; set; }
    public decimal AliquotaCofins { get; set; }
    public decimal ValorCofins { get; set; }
    
    // Outros
    public short Cfop { get; set; }
    public short Cst { get; set; }
    public decimal ValorTributo { get; set; }
    public decimal ValorFrete { get; set; }
}

/// <summary>
/// DTO para criar/atualizar peça da nota fiscal
/// </summary>
public class PecaDaNotaFiscalCreateDto
{
    [Required(ErrorMessage = "Produto/Peça é obrigatório")]
    public int SequenciaDoProduto { get; set; }
    
    [Required(ErrorMessage = "Quantidade é obrigatória")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public decimal Quantidade { get; set; }
    
    [Required(ErrorMessage = "Valor unitário é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor unitário não pode ser negativo")]
    public decimal ValorUnitario { get; set; }
    
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    
    // ICMS
    public decimal BaseDeCalculoIcms { get; set; }
    public decimal AliquotaIcms { get; set; }
    public decimal ValorIcms { get; set; }
    public decimal PercentualReducaoIcms { get; set; }
    public bool Diferido { get; set; }
    
    // ICMS ST
    public decimal BaseDeCalculoSt { get; set; }
    public decimal AliquotaSt { get; set; }
    public decimal ValorIcmsSt { get; set; }
    public decimal Iva { get; set; }
    
    // IPI
    public decimal AliquotaIpi { get; set; }
    public decimal ValorIpi { get; set; }
    
    // PIS/COFINS
    public decimal BcPis { get; set; }
    public decimal AliquotaPis { get; set; }
    public decimal ValorPis { get; set; }
    public decimal BcCofins { get; set; }
    public decimal AliquotaCofins { get; set; }
    public decimal ValorCofins { get; set; }
    
    // Outros
    public short Cfop { get; set; }
    public short Cst { get; set; }
    public decimal ValorTributo { get; set; }
    public decimal ValorFrete { get; set; }
}

/// <summary>
/// DTO para criar/atualizar parcela da nota fiscal
/// </summary>
public class ParcelaNotaFiscalCreateDto
{
    [Required(ErrorMessage = "Número da parcela é obrigatório")]
    [Range(1, short.MaxValue, ErrorMessage = "Número da parcela deve ser maior que zero")]
    public short NumeroDaParcela { get; set; }
    
    public short Dias { get; set; }
    
    [Required(ErrorMessage = "Data de vencimento é obrigatória")]
    public DateTime DataDeVencimento { get; set; }
    
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }
}

#endregion

#region DTOs para Criação/Atualização

/// <summary>
/// DTO para criar/atualizar nota fiscal - Aba 1 (Dados Principais)
/// </summary>
public class NotaFiscalCreateUpdateDto
{
    // Identificação
    public int NumeroDaNotaFiscal { get; set; }
    
    // Datas
    [Required(ErrorMessage = "Data de emissão é obrigatória")]
    public DateTime? DataDeEmissao { get; set; }
    
    public DateTime? DataDeSaida { get; set; }
    public DateTime? HoraDaSaida { get; set; }
    
    // Cliente/Destinatário
    [Required(ErrorMessage = "Cliente é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido")]
    public int SequenciaDoGeral { get; set; }
    
    // Propriedade/Filial
    [Required(ErrorMessage = "Propriedade é obrigatória")]
    public short SequenciaDaPropriedade { get; set; }
    
    // Natureza de Operação
    [Required(ErrorMessage = "Natureza de operação é obrigatória")]
    public short SequenciaDaNatureza { get; set; }
    
    // Classificação Fiscal
    public short SequenciaDaClassificacao { get; set; }
    
    // Tipo de Cobrança
    public short SequenciaDaCobranca { get; set; }
    
    // Transportadora
    public bool TransportadoraAvulsa { get; set; }
    public int SequenciaDaTransportadora { get; set; }
    public string NomeDaTransportadoraAvulsa { get; set; } = string.Empty;
    public string DocumentoDaTransportadora { get; set; } = string.Empty;
    public string IeDaTransportadora { get; set; } = string.Empty;
    public string EnderecoDaTransportadora { get; set; } = string.Empty;
    public int MunicipioDaTransportadora { get; set; }
    public string PlacaDoVeiculo { get; set; } = string.Empty;
    public string UfDoVeiculo { get; set; } = string.Empty;
    public string CodigoDaAntt { get; set; } = string.Empty;
    
    // Frete
    public string? Frete { get; set; }
    public decimal ValorDoFrete { get; set; }
    
    // Volumes
    public int Volume { get; set; }
    public string Especie { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Numeracao { get; set; } = string.Empty;
    public decimal PesoBruto { get; set; }
    public decimal PesoLiquido { get; set; }
    
    // Pagamento
    public string FormaDePagamento { get; set; } = string.Empty;
    public bool ContraApresentacao { get; set; }
    public short Fechamento { get; set; }
    public decimal ValorDoFechamento { get; set; }
    
    // Observações
    public string Historico { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    
    // Tipo e Flags
    public short TipoDeNota { get; set; }
    public bool NotaFiscalAvulsa { get; set; }
    public bool OcultarValorUnitario { get; set; }
    
    // Outros valores
    public decimal ValorDoSeguro { get; set; }
    public decimal OutrasDespesas { get; set; }
    
    // NFe Complementar/Devolução
    public bool NfeComplementar { get; set; }
    public string ChaveAcessoNfeReferenciada { get; set; } = string.Empty;
    public bool NotaDeDevolucao { get; set; }
    public string ChaveDaDevolucao { get; set; } = string.Empty;
    public string ChaveDaDevolucao2 { get; set; } = string.Empty;
    public string ChaveDaDevolucao3 { get; set; } = string.Empty;
    
    // Conjunto Avulso
    public bool ConjuntoAvulso { get; set; }
    public string DescricaoConjuntoAvulso { get; set; } = string.Empty;
    
    // Relacionamentos
    public int SequenciaDoPedido { get; set; }
    public int SequenciaDoVendedor { get; set; }
    public int NumeroDoContrato { get; set; }
    public bool Refaturamento { get; set; }
    public bool Financiamento { get; set; }
}

#endregion

#region DTOs para Filtros

/// <summary>
/// DTO para filtros de busca de notas fiscais
/// </summary>
public class NotaFiscalFiltroDto
{
    public string? Busca { get; set; }
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public int? Cliente { get; set; }
    public short? Natureza { get; set; }
    public short? Propriedade { get; set; }
    public short? TipoDeNota { get; set; }
    public bool? Canceladas { get; set; }
    public bool? Transmitidas { get; set; }
    public bool? Autorizadas { get; set; }
    public int? NumeroDaNfe { get; set; }
    
    // Paginação
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

#endregion

#region DTOs para Combos/Selects

/// <summary>
/// DTO para combo de clientes
/// </summary>
public class ClienteComboDto
{
    public int SequenciaDoGeral { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
}

/// <summary>
/// DTO para combo de naturezas de operação
/// </summary>
public class NaturezaOperacaoComboDto
{
    public short SequenciaDaNatureza { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Cfop { get; set; } = string.Empty;
    public bool EntradaSaida { get; set; }
}

/// <summary>
/// DTO para combo de propriedades/filiais
/// </summary>
public class PropriedadeComboDto
{
    public short SequenciaDaPropriedade { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
}

/// <summary>
/// DTO para combo de transportadoras
/// </summary>
public class TransportadoraComboDto
{
    public int SequenciaDoGeral { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
}

/// <summary>
/// DTO para combo de tipos de cobrança
/// </summary>
public class TipoCobrancaComboDto
{
    public short SequenciaDaCobranca { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

/// <summary>
/// DTO para combo de vendedores
/// </summary>
public class VendedorComboDto
{
    public int SequenciaDoGeral { get; set; }
    public string Nome { get; set; } = string.Empty;
}

#endregion

#region DTO para Ações NFe

/// <summary>
/// DTO para transmissão de NFe
/// </summary>
public class TransmitirNfeDto
{
    public int SequenciaDaNotaFiscal { get; set; }
}

/// <summary>
/// DTO para cancelamento de NFe
/// </summary>
public class CancelarNfeDto
{
    public int SequenciaDaNotaFiscal { get; set; }
    [Required(ErrorMessage = "Justificativa é obrigatória")]
    [MinLength(15, ErrorMessage = "Justificativa deve ter no mínimo 15 caracteres")]
    public string Justificativa { get; set; } = string.Empty;
}

/// <summary>
/// DTO para carta de correção
/// </summary>
public class CartaCorrecaoDto
{
    public int SequenciaDaNotaFiscal { get; set; }
    [Required(ErrorMessage = "Correção é obrigatória")]
    [MinLength(15, ErrorMessage = "Correção deve ter no mínimo 15 caracteres")]
    public string Correcao { get; set; } = string.Empty;
}

#endregion
