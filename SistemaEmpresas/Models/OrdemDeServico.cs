using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Ordem de Serviço")]
public partial class OrdemDeServico
{
    [Key]
    [Column("Seqüência da Ordem de Serviço")]
    public int SequenciaDaOrdemDeServico { get; set; }

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime? DataDeEmissao { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    [Column("Data do Fechamento", TypeName = "datetime")]
    public DateTime? DataDoFechamento { get; set; }

    public short Validade { get; set; }

    public short Fechamento { get; set; }

    [Column(TypeName = "text")]
    public string Observacao { get; set; } = null!;

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

    [Column("Valor Total Ordem de Serviço", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalOrdemDeServico { get; set; }

    [Column("Nome Cliente")]
    [StringLength(60)]
    [Unicode(false)]
    public string NomeCliente { get; set; } = null!;

    [Column("É Propriedade")]
    public bool EPropriedade { get; set; }

    [Column("Nome da Propriedade")]
    [StringLength(62)]
    [Unicode(false)]
    public string NomeDaPropriedade { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Complemento { get; set; } = null!;

    [Column("Número do Endereço")]
    [StringLength(10)]
    [Unicode(false)]
    public string NumeroDoEndereco { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("Caixa Postal")]
    [StringLength(30)]
    [Unicode(false)]
    public string CaixaPostal { get; set; } = null!;

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

    [Column("Serviço em Garantia")]
    public bool ServicoEmGarantia { get; set; }

    [Column("Tipo do Relatório")]
    [StringLength(10)]
    [Unicode(false)]
    public string TipoDoRelatorio { get; set; } = null!;

    [Column("Modelo do Trator")]
    [StringLength(15)]
    [Unicode(false)]
    public string ModeloDoTrator { get; set; } = null!;

    [Column("Ano de Fabricação do Trator")]
    public short AnoDeFabricacaoDoTrator { get; set; }

    [Column("Número do Motor do Trator")]
    [StringLength(12)]
    [Unicode(false)]
    public string NumeroDoMotorDoTrator { get; set; } = null!;

    [Column("Número do Chassi do Trator")]
    [StringLength(10)]
    [Unicode(false)]
    public string NumeroDoChassiDoTrator { get; set; } = null!;

    [Column("Horímetro do Trator", TypeName = "decimal(7, 2)")]
    public decimal HorimetroDoTrator { get; set; }

    [Column("Cor do Trator")]
    [StringLength(25)]
    [Unicode(false)]
    public string CorDoTrator { get; set; } = null!;

    [Column("Kilometragem do Trator", TypeName = "decimal(6, 1)")]
    public decimal KilometragemDoTrator { get; set; }

    [Column("Valor Km Rodado do Trator", TypeName = "decimal(11, 2)")]
    public decimal ValorKmRodadoDoTrator { get; set; }

    [Column("Seqüência do Pedido")]
    public int SequenciaDoPedido { get; set; }

    [Column("Seqüência da Classificação")]
    public short SequenciaDaClassificacao { get; set; }

    [Column("Valor Total das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecas { get; set; }

    [Column("Valor Total IPI das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDasPecas { get; set; }

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor Total das Peças Usadas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecasUsadas { get; set; }

    [Column("Valor Total da Base de Cálculo", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDaBaseDeCalculo { get; set; }

    [Column("Valor do Seguro", TypeName = "decimal(11, 2)")]
    public decimal ValorDoSeguro { get; set; }

    public bool Cancelado { get; set; }

    [Column("Código do Suframa")]
    [StringLength(9)]
    [Unicode(false)]
    public string CodigoDoSuframa { get; set; } = null!;

    public bool Revenda { get; set; }

    [Column("Gerar Pedido")]
    public bool GerarPedido { get; set; }

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

    [InverseProperty("SequenciaDaOrdemDeServicoNavigation")]
    public virtual ICollection<ConjuntoDaOrdemDeServico> ConjuntosDaOrdemDeServicos { get; set; } = new List<ConjuntoDaOrdemDeServico>();

    [InverseProperty("SequenciaDaOrdemDeServicoNavigation")]
    public virtual ICollection<ParcelaOrdemDeServico> ParcelasOrdemDeServicos { get; set; } = new List<ParcelaOrdemDeServico>();

    [InverseProperty("SequenciaDaOrdemDeServicoNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("SequenciaDaOrdemDeServicoNavigation")]
    public virtual ICollection<PecaDaOrdemDeServico> PecasDaOrdemDeServicos { get; set; } = new List<PecaDaOrdemDeServico>();

    [InverseProperty("SequenciaDaOrdemDeServicoNavigation")]
    public virtual ICollection<ProdutoDaOrdemDeServico> ProdutosDaOrdemDeServicos { get; set; } = new List<ProdutoDaOrdemDeServico>();

    [ForeignKey("SequenciaDaClassificacao")]
    [InverseProperty("OrdemDeServicos")]
    public virtual ClassificacaoFiscal SequenciaDaClassificacaoNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDaPropriedade")]
    [InverseProperty("OrdemDeServicos")]
    public virtual Propriedade SequenciaDaPropriedadeNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("OrdemDeServicoSequenciaDoGeralNavigations")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoMunicipio")]
    [InverseProperty("OrdemDeServicos")]
    public virtual Municipio SequenciaDoMunicipioNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoVendedor")]
    [InverseProperty("OrdemDeServicoSequenciaDoVendedorNavigations")]
    public virtual Geral SequenciaDoVendedorNavigation { get; set; } = null!;

    [InverseProperty("SequenciaDaOrdemDeServicoNavigation")]
    public virtual ICollection<ServicoDaOrdemDeServico> ServicosDaOrdemDeServicos { get; set; } = new List<ServicoDaOrdemDeServico>();
}
