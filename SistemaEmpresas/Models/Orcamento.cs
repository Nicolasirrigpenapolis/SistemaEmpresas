using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Orçamento")]
public partial class Orcamento
{
    [Key]
    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    [Column(TypeName = "text")]
    public string Observacao { get; set; } = null!;

    public short Fechamento { get; set; }

    [Column("Valor do Fechamento", TypeName = "decimal(11, 2)")]
    public decimal ValorDoFechamento { get; set; }

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

    [Column("Valor Total do Orçamento", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoOrcamento { get; set; }

    [Column("Nome Cliente")]
    [StringLength(60)]
    [Unicode(false)]
    public string NomeCliente { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [Column("Número do Endereço")]
    [StringLength(10)]
    [Unicode(false)]
    public string NumeroDoEndereco { get; set; } = null!;

    [Column("Seqüência do Município")]
    public int SequenciaDoMunicipio { get; set; }

    [Column("CEP")]
    [StringLength(9)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [StringLength(14)]
    [Unicode(false)]
    public string Telefone { get; set; } = null!;

    [StringLength(14)]
    [Unicode(false)]
    public string Fax { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("Seqüência do Vendedor")]
    public int SequenciaDoVendedor { get; set; }

    [Column("Seqüência do Pedido")]
    public int SequenciaDoPedido { get; set; }

    public short Tipo { get; set; }

    [Column("CPF e CNPJ")]
    [StringLength(20)]
    [Unicode(false)]
    public string CpfECnpj { get; set; } = null!;

    [Column("RG e IE")]
    [StringLength(20)]
    [Unicode(false)]
    public string RgEIe { get; set; } = null!;

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column("Ocultar Valor Unitário")]
    public bool OcultarValorUnitario { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("Valor Total IPI das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDasPecas { get; set; }

    [Column("Valor Total das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecas { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor Total das Peças Usadas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecasUsadas { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Complemento { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("Caixa Postal")]
    [StringLength(30)]
    [Unicode(false)]
    public string CaixaPostal { get; set; } = null!;

    [Column("É Propriedade")]
    public bool EPropriedade { get; set; }

    [Column("Nome da Propriedade")]
    [StringLength(62)]
    [Unicode(false)]
    public string NomeDaPropriedade { get; set; } = null!;

    [Column("Valor Total da Base de Cálculo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaBaseDeCalculo { get; set; }

    [Column("Valor do Seguro", TypeName = "decimal(11, 2)")]
    public decimal ValorDoSeguro { get; set; }

    [Column("Data do Fechamento", TypeName = "datetime")]
    public DateTime? DataDoFechamento { get; set; }

    public bool Cancelado { get; set; }

    [Column("Código do Suframa")]
    [StringLength(9)]
    [Unicode(false)]
    public string CodigoDoSuframa { get; set; } = null!;

    public bool Revenda { get; set; }

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

    [Column("Entrega Futura")]
    public bool EntregaFutura { get; set; }

    [Column("Seqüência da Transportadora")]
    public int SequenciaDaTransportadora { get; set; }

    [Column("Data da Alteração", TypeName = "datetime")]
    public DateTime? DataDaAlteracao { get; set; }

    [Column("Hora da Alteração", TypeName = "datetime")]
    public DateTime? HoraDaAlteracao { get; set; }

    [Column("Usuário da Alteração")]
    [StringLength(60)]
    [Unicode(false)]
    public string UsuarioDaAlteracao { get; set; } = null!;

    [Column("Venda Fechada")]
    public bool VendaFechada { get; set; }

    [Column("Orçamento Avulso")]
    public bool OrcamentoAvulso { get; set; }

    [Column("Valor do Imposto de Renda", TypeName = "decimal(11, 2)")]
    public decimal ValorDoImpostoDeRenda { get; set; }

    [Column("Fatura Proforma")]
    public bool FaturaProforma { get; set; }

    [Column("Local de Embarque")]
    [StringLength(30)]
    [Unicode(false)]
    public string LocalDeEmbarque { get; set; } = null!;

    [Column("UF de Embarque")]
    [StringLength(3)]
    [Unicode(false)]
    public string UfDeEmbarque { get; set; } = null!;

    [Column("Número da Proforma")]
    public int NumeroDaProforma { get; set; }

    [Column("Seqüência do País")]
    public int SequenciaDoPais { get; set; }

    [Column("Valor Total do Tributo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoTributo { get; set; }

    [Column("Conjunto Avulso")]
    public bool ConjuntoAvulso { get; set; }

    [Column("Descrição Conjunto Avulso")]
    [StringLength(60)]
    [Unicode(false)]
    public string DescricaoConjuntoAvulso { get; set; } = null!;

    [Column("Vendedor Intermediario")]
    [StringLength(40)]
    [Unicode(false)]
    public string VendedorIntermediario { get; set; } = null!;

    [Column("Percentual do Vendedor", TypeName = "decimal(8, 4)")]
    public decimal PercentualDoVendedor { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string Rebiut { get; set; } = null!;

    [Column("Percentual Rebiut", TypeName = "decimal(8, 4)")]
    public decimal PercentualRebiut { get; set; }

    [Column("Nao Movimentar Estoque")]
    public bool NaoMovimentarEstoque { get; set; }

    [Column("Gerou Encargos")]
    public bool GerouEncargos { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Hidroturbo { get; set; } = null!;

    [Column("Area irrigada", TypeName = "decimal(8, 2)")]
    public decimal AreaIrrigada { get; set; }

    [Column("Precipitação bruta", TypeName = "decimal(8, 2)")]
    public decimal PrecipitacaoBruta { get; set; }

    [Column("Horas irrigada", TypeName = "decimal(7, 2)")]
    public decimal HorasIrrigada { get; set; }

    [Column("Area tot irrigada em", TypeName = "decimal(7, 2)")]
    public decimal AreaTotIrrigadaEm { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Aspersor { get; set; } = null!;

    [Column("Modelo do aspersor")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloDoAspersor { get; set; } = null!;

    [Column("Bocal diametro", TypeName = "decimal(7, 2)")]
    public decimal BocalDiametro { get; set; }

    [Column("Pressão de serviço", TypeName = "decimal(7, 2)")]
    public decimal PressaoDeServico { get; set; }

    [Column("Alcance do jato", TypeName = "decimal(7, 2)")]
    public decimal AlcanceDoJato { get; set; }

    [Column("Espaço entre carreadores", TypeName = "decimal(7, 2)")]
    public decimal EspacoEntreCarreadores { get; set; }

    [Column("Faixa irrigada", TypeName = "decimal(7, 2)")]
    public decimal FaixaIrrigada { get; set; }

    [Column("Desnivel maximo na area", TypeName = "decimal(7, 2)")]
    public decimal DesnivelMaximoNaArea { get; set; }

    [Column("Altura de sucção", TypeName = "decimal(7, 2)")]
    public decimal AlturaDeSuccao { get; set; }

    [Column("Altura do aspersor", TypeName = "decimal(5, 2)")]
    public decimal AlturaDoAspersor { get; set; }

    [Column("Tempo parado antes percurso", TypeName = "decimal(5, 2)")]
    public decimal TempoParadoAntesPercurso { get; set; }

    [Column("Com 1", TypeName = "decimal(8, 2)")]
    public decimal Com1 { get; set; }

    [Column("Com 2", TypeName = "decimal(8, 2)")]
    public decimal Com2 { get; set; }

    [Column("Com 3", TypeName = "decimal(8, 2)")]
    public decimal Com3 { get; set; }

    [Column("Modelo Trecho A")]
    public int ModeloTrechoA { get; set; }

    [Column("Modelo Trecho B")]
    public int ModeloTrechoB { get; set; }

    [Column("Modelo Trecho C")]
    public int ModeloTrechoC { get; set; }

    [Column("Qtde bomba")]
    public short QtdeBomba { get; set; }

    [Column("Marca bomba")]
    [StringLength(25)]
    [Unicode(false)]
    public string MarcaBomba { get; set; } = null!;

    [Column("Modelo bomba")]
    [StringLength(25)]
    [Unicode(false)]
    public string ModeloBomba { get; set; } = null!;

    [Column("Tamanho bomba")]
    [StringLength(20)]
    [Unicode(false)]
    public string TamanhoBomba { get; set; } = null!;

    [Column("N estagios")]
    public short NEstagios { get; set; }

    [Column("Diametro bomba", TypeName = "decimal(8, 2)")]
    public decimal DiametroBomba { get; set; }

    [Column("Pressao bomba", TypeName = "decimal(8, 2)")]
    public decimal PressaoBomba { get; set; }

    [Column("Rendimento bomba", TypeName = "decimal(8, 2)")]
    public decimal RendimentoBomba { get; set; }

    [Column("Rotação bomba", TypeName = "decimal(8, 2)")]
    public decimal RotacaoBomba { get; set; }

    [Column("Qtde de Motor", TypeName = "decimal(10, 2)")]
    public decimal QtdeDeMotor { get; set; }

    [Column("Marca do Motor")]
    [StringLength(20)]
    [Unicode(false)]
    public string MarcaDoMotor { get; set; } = null!;

    [Column("Modelo Motor")]
    [StringLength(25)]
    [Unicode(false)]
    public string ModeloMotor { get; set; } = null!;

    [Column("Nivel de Proteção")]
    [StringLength(15)]
    [Unicode(false)]
    public string NivelDeProtecao { get; set; } = null!;

    [Column("Potencia Nominal", TypeName = "decimal(8, 2)")]
    public decimal PotenciaNominal { get; set; }

    [Column("Nro de Fases")]
    public short NroDeFases { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Voltagem { get; set; }

    [Column("Modelo hidroturbo")]
    [StringLength(30)]
    [Unicode(false)]
    public string ModeloHidroturbo { get; set; } = null!;

    public short Eixos { get; set; }

    public short Rodas { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Pneus { get; set; } = null!;

    [StringLength(25)]
    [Unicode(false)]
    public string Tubos { get; set; } = null!;

    public int Projetista { get; set; }

    [Column("Peso Bruto", TypeName = "decimal(11, 2)")]
    public decimal PesoBruto { get; set; }

    [Column("Peso Líquido", TypeName = "decimal(11, 2)")]
    public decimal PesoLiquido { get; set; }

    public int Volumes { get; set; }

    [Column("Aviso de embarque")]
    [StringLength(100)]
    [Unicode(false)]
    public string AvisoDeEmbarque { get; set; } = null!;

    [Column("Entrega Tecnica")]
    [StringLength(19)]
    [Unicode(false)]
    public string EntregaTecnica { get; set; } = null!;

    [Column("Sequencia do Projeto")]
    public int SequenciaDoProjeto { get; set; }

    [Column("Outras Despesas", TypeName = "decimal(10, 2)")]
    public decimal OutrasDespesas { get; set; }

    public bool Refaturamento { get; set; }

    [Column("Data do Pedido", TypeName = "datetime")]
    public DateTime? DataDoPedido { get; set; }

    [Column("Data de Entrega", TypeName = "datetime")]
    public DateTime? DataDeEntrega { get; set; }

    [Column("Ordem Interna")]
    public bool OrdemInterna { get; set; }

    [Column("Orçamento Vinculado")]
    public int OrcamentoVinculado { get; set; }

    [StringLength(35)]
    [Unicode(false)]
    public string? Frete { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string? PlacaVeiculo { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? UfPlaca { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string? NumAntt { get; set; }

    [Column("Total CBS")]
    public double? TotalCbs { get; set; }

    [Column("Total IBS")]
    public double? TotalIbs { get; set; }

    [InverseProperty("SequenciaDoOrcamentoNavigation")]
    public virtual ICollection<ConjuntoDoOrcamento> ConjuntosDoOrcamentos { get; set; } = new List<ConjuntoDoOrcamento>();

    [InverseProperty("SequenciaDoOrcamentoNavigation")]
    public virtual ICollection<ParcelaOrcamento> ParcelasOrcamentos { get; set; } = new List<ParcelaOrcamento>();

    [InverseProperty("SequenciaDoOrcamentoNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("SequenciaDoOrcamentoNavigation")]
    public virtual ICollection<PecaDoOrcamento> PecasDoOrcamentos { get; set; } = new List<PecaDoOrcamento>();

    [InverseProperty("SequenciaDoOrcamentoNavigation")]
    public virtual ICollection<ProdutoDoOrcamento> ProdutosDoOrcamentos { get; set; } = new List<ProdutoDoOrcamento>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("Orcamentos")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaPropriedade")]
    [InverseProperty("Orcamentos")]
    public virtual Propriedade SequenciaDaPropriedadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaTransportadora")]
    [InverseProperty("OrcamentoSequenciaDaTransportadoraNavigations")]
    public virtual Geral SequenciaDaTransportadoraNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("OrcamentoSequenciaDoGeralNavigations")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoMunicipio")]
    [InverseProperty("Orcamentos")]
    public virtual Municipio SequenciaDoMunicipioNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoPais")]
    [InverseProperty("Orcamentos")]
    public virtual Paise SequenciaDoPaisNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoVendedor")]
    [InverseProperty("OrcamentoSequenciaDoVendedorNavigations")]
    public virtual Geral SequenciaDoVendedorNavigation { get; set; } = null!;

    [InverseProperty("SequenciaDoOrcamentoNavigation")]
    public virtual ICollection<ServicoDoOrcamento> ServicosDoOrcamentos { get; set; } = new List<ServicoDoOrcamento>();

    [InverseProperty("SequenciaDoOrcamentoNavigation")]
    public virtual ICollection<VinculaPedidoOrcamento> VinculaPedidoOrcamentos { get; set; } = new List<VinculaPedidoOrcamento>();
}
