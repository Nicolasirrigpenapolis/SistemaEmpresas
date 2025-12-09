using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Nota Fiscal")]
public partial class NotaFiscal
{
    [Key]
    [Column("Seqüência da Nota Fiscal")]
    public int SequenciaDaNotaFiscal { get; set; }

    [Column("Número da NFe")]
    public int NumeroDaNfe { get; set; }

    [Column("Número da NFSe")]
    public int NumeroDaNfse { get; set; }

    [Column("Número da Nota Fiscal")]
    public int NumeroDaNotaFiscal { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    [Column("Seqüência da Natureza")]
    public short SequenciaDaNatureza { get; set; }

    [Column("Transportadora Avulsa")]
    public bool TransportadoraAvulsa { get; set; }

    [Column("Seqüência da Transportadora")]
    public int SequenciaDaTransportadora { get; set; }

    [Column("Nome da Transportadora Avulsa")]
    [StringLength(60)]
    [Unicode(false)]
    public string NomeDaTransportadoraAvulsa { get; set; } = null!;

    [Column("Placa do Veículo")]
    [StringLength(8)]
    [Unicode(false)]
    public string PlacaDoVeiculo { get; set; } = null!;

    [Column("UF do Veículo")]
    [StringLength(3)]
    [Unicode(false)]
    public string UfDoVeiculo { get; set; } = null!;

    [StringLength(35)]
    [Unicode(false)]
    public string? Frete { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    public short Fechamento { get; set; }

    [Column("Valor do Fechamento", TypeName = "decimal(11, 2)")]
    public decimal ValorDoFechamento { get; set; }

    public int Volume { get; set; }

    [Column("Espécie")]
    [StringLength(20)]
    [Unicode(false)]
    public string Especie { get; set; } = null!;

    [Column("Data de Saída", TypeName = "datetime")]
    public DateTime? DataDeSaida { get; set; }

    [Column("Hora da Saída", TypeName = "datetime")]
    public DateTime? HoraDaSaida { get; set; }

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column("Histórico", TypeName = "text")]
    public string Historico { get; set; } = null!;

    [Column("Valor Total IPI dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDosProdutos { get; set; }

    [Column("Valor Total IPI dos Conjuntos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDosConjuntos { get; set; }

    [Column("Valor Total do ICMS", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoIcms { get; set; }

    [Column("Valor Total dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosProdutos { get; set; }

    [Column("Valor Total dos Conjuntos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosConjuntos { get; set; }

    [Column("Valor Total de Produtos Usados", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDeProdutosUsados { get; set; }

    [Column("Valor Total Conjuntos Usados", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalConjuntosUsados { get; set; }

    [Column("Valor Total dos Serviços", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosServicos { get; set; }

    [Column("Valor Total da Nota Fiscal", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaNotaFiscal { get; set; }

    [Column("Tipo de Nota")]
    public short TipoDeNota { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("Nota Cancelada")]
    public bool NotaCancelada { get; set; }

    [Column("Seqüência do Pedido")]
    public int SequenciaDoPedido { get; set; }

    [Column("Seqüência do Vendedor")]
    public int SequenciaDoVendedor { get; set; }

    [Column("Seqüência da Cobrança")]
    public short SequenciaDaCobranca { get; set; }

    [Column("Nota Fiscal Avulsa")]
    public bool NotaFiscalAvulsa { get; set; }

    [Column("Peso Bruto", TypeName = "decimal(11, 2)")]
    public decimal PesoBruto { get; set; }

    [Column("Peso Líquido", TypeName = "decimal(11, 2)")]
    public decimal PesoLiquido { get; set; }

    [Column("Ocultar Valor Unitário")]
    public bool OcultarValorUnitario { get; set; }

    [Column("Contra Apresentação")]
    public bool ContraApresentacao { get; set; }

    [Column("Município da Transportadora")]
    public int MunicipioDaTransportadora { get; set; }

    [Column("Documento da Transportadora")]
    [StringLength(20)]
    [Unicode(false)]
    public string DocumentoDaTransportadora { get; set; } = null!;

    [Column("NFe Complementar")]
    public bool NfeComplementar { get; set; }

    [Column("Chave Acesso NFe Referenciada")]
    [StringLength(45)]
    [Unicode(false)]
    public string ChaveAcessoNfeReferenciada { get; set; } = null!;

    [Column("Chave de Acesso da NFe")]
    [StringLength(50)]
    [Unicode(false)]
    public string ChaveDeAcessoDaNfe { get; set; } = null!;

    [Column("Protocolo de Autorização NFe")]
    [StringLength(50)]
    [Unicode(false)]
    public string ProtocoloDeAutorizacaoNfe { get; set; } = null!;

    [Column("Data e Hora da NFe")]
    [StringLength(25)]
    [Unicode(false)]
    public string DataEHoraDaNfe { get; set; } = null!;

    public bool Transmitido { get; set; }

    public bool Autorizado { get; set; }

    [Column("Número do Recibo da NFe")]
    [StringLength(20)]
    [Unicode(false)]
    public string NumeroDoReciboDaNfe { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Marca { get; set; } = null!;

    [Column("Numeração")]
    [StringLength(20)]
    [Unicode(false)]
    public string Numeracao { get; set; } = null!;

    [Column("Valor Total IPI das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDasPecas { get; set; }

    [Column("Valor Total das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecas { get; set; }

    [Column("Código da ANTT")]
    [StringLength(20)]
    [Unicode(false)]
    public string CodigoDaAntt { get; set; } = null!;

    [Column("Endereço da Transportadora")]
    [StringLength(40)]
    [Unicode(false)]
    public string EnderecoDaTransportadora { get; set; } = null!;

    [Column("IE da Transportadora")]
    [StringLength(15)]
    [Unicode(false)]
    public string IeDaTransportadora { get; set; } = null!;

    [Column("Observação", TypeName = "text")]
    public string Observacao { get; set; } = null!;

    [Column("Valor Total das Peças Usadas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecasUsadas { get; set; }

    [Column("Valor Total da Base de Cálculo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaBaseDeCalculo { get; set; }

    [Column("Valor do Seguro", TypeName = "decimal(11, 2)")]
    public decimal ValorDoSeguro { get; set; }

    [Column("Valor Total do PIS", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoPis { get; set; }

    [Column("Valor Total do COFINS", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoCofins { get; set; }

    [Column("Valor Total da Base ST", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaBaseSt { get; set; }

    [Column("Valor Total do ICMS ST", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoIcmsSt { get; set; }

    [Column("Alíquota do ISS", TypeName = "decimal(5, 2)")]
    public decimal AliquotaDoIss { get; set; }

    [Column("Reter ISS")]
    public bool ReterIss { get; set; }

    [Column("Recibo NFSe")]
    [StringLength(255)]
    [Unicode(false)]
    public string ReciboNfse { get; set; } = null!;

    public bool Imprimiu { get; set; }

    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Column("Número do Contrato")]
    public int NumeroDoContrato { get; set; }

    [Column("Valor do Imposto de Renda", TypeName = "decimal(11, 2)")]
    public decimal ValorDoImpostoDeRenda { get; set; }

    [Column("Valor Total da Importação", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaImportacao { get; set; }

    [Column("Conjunto Avulso")]
    public bool ConjuntoAvulso { get; set; }

    [Column("Valor Total do Tributo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoTributo { get; set; }

    [Column("Descrição Conjunto Avulso")]
    [StringLength(60)]
    [Unicode(false)]
    public string DescricaoConjuntoAvulso { get; set; } = null!;

    [Column("FinNFe")]
    public short FinNfe { get; set; }

    [Column("Novo Layout")]
    public bool NovoLayout { get; set; }

    [Column("Nota de Devolução")]
    public bool NotaDeDevolucao { get; set; }

    [Column("Chave da Devolução")]
    [StringLength(200)]
    [Unicode(false)]
    public string ChaveDaDevolucao { get; set; } = null!;

    [Column("Outras Despesas", TypeName = "decimal(10, 2)")]
    public decimal OutrasDespesas { get; set; }

    [Column("Chave da Devolução 2")]
    [StringLength(200)]
    [Unicode(false)]
    public string ChaveDaDevolucao2 { get; set; } = null!;

    [Column("Chave da Devolução 3")]
    [StringLength(200)]
    [Unicode(false)]
    public string ChaveDaDevolucao3 { get; set; } = null!;

    [Column("Cancelada no livro")]
    public bool CanceladaNoLivro { get; set; }

    public bool Refaturamento { get; set; }

    [Column("Nota de venda")]
    public int NotaDeVenda { get; set; }

    public bool Financiamento { get; set; }

    [Column("Valor Total IBS", TypeName = "decimal(18, 2)")]
    public decimal ValorTotalIbs { get; set; }

    [Column("Valor Total CBS", TypeName = "decimal(18, 2)")]
    public decimal ValorTotalCbs { get; set; }

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual CancelamentoNfe? CancelamentoNfe { get; set; }

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual ICollection<CartaDeCorrecaoNfe> CartaDeCorrecaoNves { get; set; } = new List<CartaDeCorrecaoNfe>();

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual Comissao? Comissao { get; set; }

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual ICollection<ConjuntoDaNotaFiscal> ConjuntosDaNotaFiscals { get; set; } = new List<ConjuntoDaNotaFiscal>();

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual ICollection<ManutencaoConta> ManutencaoConta { get; set; } = new List<ManutencaoConta>();

    [ForeignKey("MunicipioDaTransportadora")]
    [InverseProperty("NotaFiscals")]
    public virtual Municipio MunicipioDaTransportadoraNavigation { get; set; } = null!;

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual NotaAutorizada? NotasAutorizada { get; set; }

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual ICollection<ParcelaNotaFiscal> ParcelasNotaFiscals { get; set; } = new List<ParcelaNotaFiscal>();

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual ICollection<PecaDaNotaFiscal> PecasDaNotaFiscals { get; set; } = new List<PecaDaNotaFiscal>();

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual ICollection<ProdutoDaNotaFiscal> ProdutosDaNotaFiscals { get; set; } = new List<ProdutoDaNotaFiscal>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("NotaFiscals")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaCobranca")]
    [InverseProperty("NotaFiscals")]
    public virtual TipoDeCobranca SequenciaDaCobrancaNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaNatureza")]
    [InverseProperty("NotaFiscals")]
    public virtual NaturezaDeOperacao SequenciaDaNaturezaNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaPropriedade")]
    [InverseProperty("NotaFiscals")]
    public virtual Propriedade SequenciaDaPropriedadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaTransportadora")]
    [InverseProperty("NotaFiscalSequenciaDaTransportadoraNavigations")]
    public virtual Geral SequenciaDaTransportadoraNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("NotaFiscalSequenciaDoGeralNavigations")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoMovimento")]
    [InverseProperty("NotaFiscals")]
    public virtual MovimentoDoEstoque SequenciaDoMovimentoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoPedido")]
    [InverseProperty("NotaFiscals")]
    public virtual Pedido SequenciaDoPedidoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoVendedor")]
    [InverseProperty("NotaFiscalSequenciaDoVendedorNavigations")]
    public virtual Geral SequenciaDoVendedorNavigation { get; set; } = null!;

    [InverseProperty("SequenciaDaNotaFiscalNavigation")]
    public virtual ICollection<ServicoDaNotaFiscal> ServicosDaNotaFiscals { get; set; } = new List<ServicoDaNotaFiscal>();
}
