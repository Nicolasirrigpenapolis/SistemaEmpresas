using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Geral")]
public partial class Geral
{
    public bool Cliente { get; set; }

    public bool Fornecedor { get; set; }

    public bool Despesa { get; set; }

    public bool Imposto { get; set; }

    public bool Transportadora { get; set; }

    public bool Vendedor { get; set; }

    [Key]
    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [Column("Nome Fantasia")]
    [StringLength(60)]
    [Unicode(false)]
    public string NomeFantasia { get; set; } = null!;

    public short Tipo { get; set; }

    [Column("Endereço")]
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

    [Column("Fone 1")]
    [StringLength(14)]
    [Unicode(false)]
    public string? Fone1 { get; set; }

    [Column("Fone 2")]
    [StringLength(14)]
    [Unicode(false)]
    public string? Fone2 { get; set; }

    [StringLength(14)]
    [Unicode(false)]
    public string? Fax { get; set; }

    [StringLength(14)]
    [Unicode(false)]
    public string? Celular { get; set; }

    [StringLength(45)]
    [Unicode(false)]
    public string Contato { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("Home Page")]
    [StringLength(60)]
    [Unicode(false)]
    public string HomePage { get; set; } = null!;

    [Column("Código do Suframa")]
    [StringLength(9)]
    [Unicode(false)]
    public string CodigoDoSuframa { get; set; } = null!;

    [Column("Código da ANTT")]
    [StringLength(20)]
    [Unicode(false)]
    public string CodigoDaAntt { get; set; } = null!;

    [Column("CPF e CNPJ")]
    [StringLength(20)]
    [Unicode(false)]
    public string CpfECnpj { get; set; } = null!;

    [Column("RG e IE")]
    [StringLength(20)]
    [Unicode(false)]
    public string RgEIe { get; set; } = null!;

    [Column("Observação", TypeName = "text")]
    public string Observacao { get; set; } = null!;

    [Column("Endereço de Cobrança")]
    [StringLength(62)]
    [Unicode(false)]
    public string EnderecoDeCobranca { get; set; } = null!;

    [Column("Número do Endereço de Cobrança")]
    [StringLength(20)]
    [Unicode(false)]
    public string NumeroDoEnderecoDeCobranca { get; set; } = null!;

    [Column("Seqüência Município Cobrança")]
    public int SequenciaMunicipioCobranca { get; set; }

    [Column("Cep de Cobrança")]
    [StringLength(9)]
    [Unicode(false)]
    public string CepDeCobranca { get; set; } = null!;

    [Column("Bairro de Cobrança")]
    [StringLength(50)]
    [Unicode(false)]
    public string BairroDeCobranca { get; set; } = null!;

    [Column("Complemento da Cobrança")]
    [StringLength(30)]
    [Unicode(false)]
    public string ComplementoDaCobranca { get; set; } = null!;

    [Column("Caixa Postal da Cobrança")]
    [StringLength(30)]
    [Unicode(false)]
    public string CaixaPostalDaCobranca { get; set; } = null!;

    [Column("Seqüência do Vendedor")]
    public int SequenciaDoVendedor { get; set; }

    [Column("Intermediário do Vendedor")]
    [StringLength(40)]
    [Unicode(false)]
    public string IntermediarioDoVendedor { get; set; } = null!;

    [Column("Nome do Banco 1")]
    [StringLength(20)]
    [Unicode(false)]
    public string NomeDoBanco1 { get; set; } = null!;

    [Column("Nome do Banco 2")]
    [StringLength(20)]
    [Unicode(false)]
    public string NomeDoBanco2 { get; set; } = null!;

    [Column("Agência do Banco 1")]
    [StringLength(20)]
    [Unicode(false)]
    public string AgenciaDoBanco1 { get; set; } = null!;

    [Column("Agência do Banco 2")]
    [StringLength(20)]
    [Unicode(false)]
    public string AgenciaDoBanco2 { get; set; } = null!;

    [Column("Conta Corrente do Banco 1")]
    [StringLength(15)]
    [Unicode(false)]
    public string ContaCorrenteDoBanco1 { get; set; } = null!;

    [Column("Conta Corrente do Banco 2")]
    [StringLength(15)]
    [Unicode(false)]
    public string ContaCorrenteDoBanco2 { get; set; } = null!;

    [Column("Nome do Correntista do Banco 1")]
    [StringLength(60)]
    [Unicode(false)]
    public string NomeDoCorrentistaDoBanco1 { get; set; } = null!;

    [Column("Nome do Correntista do Banco 2")]
    [StringLength(60)]
    [Unicode(false)]
    public string NomeDoCorrentistaDoBanco2 { get; set; } = null!;

    public bool Inativo { get; set; }

    public bool Revenda { get; set; }

    public bool Isento { get; set; }

    [Column("Data do Cadastro", TypeName = "datetime")]
    public DateTime? DataDoCadastro { get; set; }

    [Column("Seqüência do País")]
    public int SequenciaDoPais { get; set; }

    [Column("Orgõn Publico")]
    public bool OrgonPublico { get; set; }

    public bool Cumulativo { get; set; }

    [Column("Empresa Produtor")]
    public bool EmpresaProdutor { get; set; }

    [Column("Usu da Alteração")]
    [StringLength(40)]
    [Unicode(false)]
    public string UsuDaAlteracao { get; set; } = null!;

    [Column("Data de Nascimento", TypeName = "datetime")]
    public DateTime? DataDeNascimento { get; set; }

    [Column("Codigo Contabil")]
    public int CodigoContabil { get; set; }

    [Column("Codigo Adiantamento")]
    public int CodigoAdiantamento { get; set; }

    [Column("Sal bruto", TypeName = "decimal(10, 2)")]
    public decimal SalBruto { get; set; }

    [Column("Importou no Zap")]
    public bool WhatsAppSincronizado { get; set; }

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<AdicaoDaDeclaracao> AdicoesDaDeclaracaos { get; set; } = new List<AdicaoDaDeclaracao>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<BaixaDoEstoqueContabil> BaixaDoEstoqueContabils { get; set; } = new List<BaixaDoEstoqueContabil>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<DeclaracaoDeImportacao> DeclaracoesDeImportacaos { get; set; } = new List<DeclaracaoDeImportacao>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<EntradaConta> EntradaConta { get; set; } = new List<EntradaConta>();

    [InverseProperty("SequenciaDoVendedorNavigation")]
    public virtual ICollection<Geral> InverseSequenciaDoVendedorNavigation { get; set; } = new List<Geral>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<ManutencaoConta> ManutencaoConta { get; set; } = new List<ManutencaoConta>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<MovimentoDoEstoqueContabil> MovimentoDoEstoqueContabils { get; set; } = new List<MovimentoDoEstoqueContabil>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<MovimentoDoEstoque> MovimentoDoEstoques { get; set; } = new List<MovimentoDoEstoque>();

    [InverseProperty("SequenciaDaTransportadoraNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscalSequenciaDaTransportadoraNavigations { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscalSequenciaDoGeralNavigations { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDoVendedorNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscalSequenciaDoVendedorNavigations { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDaTransportadoraNavigation")]
    public virtual ICollection<NovaLicitacao> NovaLicitacaoSequenciaDaTransportadoraNavigations { get; set; } = new List<NovaLicitacao>();

    [InverseProperty("SequenciaDoFornecedorNavigation")]
    public virtual ICollection<NovaLicitacao> NovaLicitacaoSequenciaDoFornecedorNavigations { get; set; } = new List<NovaLicitacao>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<OrdemDeServico> OrdemDeServicoSequenciaDoGeralNavigations { get; set; } = new List<OrdemDeServico>();

    [InverseProperty("SequenciaDoVendedorNavigation")]
    public virtual ICollection<OrdemDeServico> OrdemDeServicoSequenciaDoVendedorNavigations { get; set; } = new List<OrdemDeServico>();

    [InverseProperty("SequenciaDaTransportadoraNavigation")]
    public virtual ICollection<Orcamento> OrcamentoSequenciaDaTransportadoraNavigations { get; set; } = new List<Orcamento>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<Orcamento> OrcamentoSequenciaDoGeralNavigations { get; set; } = new List<Orcamento>();

    [InverseProperty("SequenciaDoVendedorNavigation")]
    public virtual ICollection<Orcamento> OrcamentoSequenciaDoVendedorNavigations { get; set; } = new List<Orcamento>();

    [InverseProperty("SequenciaDaTransportadoraNavigation")]
    public virtual ICollection<Pedido> PedidoSequenciaDaTransportadoraNavigations { get; set; } = new List<Pedido>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<Pedido> PedidoSequenciaDoGeralNavigations { get; set; } = new List<Pedido>();

    [InverseProperty("SequenciaDoVendedorNavigation")]
    public virtual ICollection<Pedido> PedidoSequenciaDoVendedorNavigations { get; set; } = new List<Pedido>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<PropriedadeDoGeral> PropriedadesDoGerals { get; set; } = new List<PropriedadeDoGeral>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<RelatorioDeViagem> RelatorioDeViagems { get; set; } = new List<RelatorioDeViagem>();

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<Requisicao> Requisicaos { get; set; } = new List<Requisicao>();

    [ForeignKey("SequenciaDoMunicipio")]
    [InverseProperty("GeralSequenciaDoMunicipioNavigations")]
    public virtual Municipio SequenciaDoMunicipioNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoPais")]
    [InverseProperty("Gerals")]
    public virtual Pais SequenciaDoPaisNavigation { get; set; } = null!;

    [ForeignKey("SequenciaDoVendedor")]
    [InverseProperty("InverseSequenciaDoVendedorNavigation")]
    public virtual Geral SequenciaDoVendedorNavigation { get; set; } = null!;

    [ForeignKey("SequenciaMunicipioCobranca")]
    [InverseProperty("GeralSequenciaMunicipioCobrancaNavigations")]
    public virtual Municipio SequenciaMunicipioCobrancaNavigation { get; set; } = null!;

    [InverseProperty("SequenciaDoGeralNavigation")]
    public virtual ICollection<VinculaPedidoOrcamento> VinculaPedidoOrcamentos { get; set; } = new List<VinculaPedidoOrcamento>();
}
