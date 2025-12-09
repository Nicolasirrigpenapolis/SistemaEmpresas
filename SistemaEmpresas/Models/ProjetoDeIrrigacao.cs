using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Projeto de Irrigação")]
public partial class ProjetoDeIrrigacao
{
    [Key]
    [Column("Sequencia do Projeto")]
    public int SequenciaDoProjeto { get; set; }

    [Column("Sequencia do Geral")]
    public int SequenciaDoGeral { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string Proposta { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Opcao { get; set; } = null!;

    [Column("Data da Proposta", TypeName = "datetime")]
    public DateTime? DataDaProposta { get; set; }

    [Column("Sequencia da Propriedade")]
    public int SequenciaDaPropriedade { get; set; }

    [Column("Descrição do Equipamento")]
    [StringLength(120)]
    [Unicode(false)]
    public string DescricaoDoEquipamento { get; set; } = null!;

    [Column("Lance em Balanço")]
    [StringLength(2)]
    [Unicode(false)]
    public string LanceEmBalanco { get; set; } = null!;

    [Column("Extensão Ult Spray", TypeName = "decimal(8, 2)")]
    public decimal ExtensaoUltSpray { get; set; }

    [Column("Alcance Spray Fim", TypeName = "decimal(8, 2)")]
    public decimal AlcanceSprayFim { get; set; }

    [Column("N Posicoes", TypeName = "decimal(8, 2)")]
    public decimal NPosicoes { get; set; }

    public short Graus { get; set; }

    [Column("Lamina Bruta", TypeName = "decimal(8, 2)")]
    public decimal LaminaBruta { get; set; }

    [Column("Tempo Max Opera", TypeName = "decimal(8, 2)")]
    public decimal TempoMaxOpera { get; set; }

    [Column("Modelo Trecho A")]
    public int ModeloTrechoA { get; set; }

    [Column("Modelo Trecho B")]
    public int ModeloTrechoB { get; set; }

    [Column("Modelo Trecho C")]
    public int ModeloTrechoC { get; set; }

    [Column("Modelo Trecho D")]
    public int ModeloTrechoD { get; set; }

    [Column("Com 1", TypeName = "decimal(8, 2)")]
    public decimal Com1 { get; set; }

    [Column("Com 2", TypeName = "decimal(8, 2)")]
    public decimal Com2 { get; set; }

    [Column("Com 3", TypeName = "decimal(8, 2)")]
    public decimal Com3 { get; set; }

    [Column("Com 4", TypeName = "decimal(8, 2)")]
    public decimal Com4 { get; set; }

    [Column("Sequencia do Autotrafo")]
    public int SequenciaDoAutotrafo { get; set; }

    [Column("Saidas Acumuladas", TypeName = "decimal(8, 2)")]
    public decimal SaidasAcumuladas { get; set; }

    [Column("Espaço medio Saidas", TypeName = "decimal(8, 3)")]
    public decimal EspacoMedioSaidas { get; set; }

    [Column("Pressao no Extremo", TypeName = "decimal(8, 2)")]
    public decimal PressaoNoExtremo { get; set; }

    [Column("Desnivel Ponto Alto", TypeName = "decimal(8, 2)")]
    public decimal DesnivelPontoAlto { get; set; }

    [Column("Altura dos Aspersores", TypeName = "decimal(8, 2)")]
    public decimal AlturaDosAspersores { get; set; }

    [Column("Desnivel Moto Bomba", TypeName = "decimal(8, 2)")]
    public decimal DesnivelMotoBomba { get; set; }

    [Column("Altura de succao", TypeName = "decimal(8, 2)")]
    public decimal AlturaDeSuccao { get; set; }

    [Column("Desnivel mais Baixo", TypeName = "decimal(8, 2)")]
    public decimal DesnivelMaisBaixo { get; set; }

    [Column("Perda Mangueira", TypeName = "decimal(8, 2)")]
    public decimal PerdaMangueira { get; set; }

    [Column("Cliente Avulso")]
    [StringLength(60)]
    [Unicode(false)]
    public string ClienteAvulso { get; set; } = null!;

    [Column("Propriedade Avulsa")]
    [StringLength(100)]
    [Unicode(false)]
    public string PropriedadeAvulsa { get; set; } = null!;

    [Column("Cidade Avulsa")]
    [StringLength(40)]
    [Unicode(false)]
    public string CidadeAvulsa { get; set; } = null!;

    [Column("Desnivel Ponto Baixo", TypeName = "decimal(8, 2)")]
    public decimal DesnivelPontoBaixo { get; set; }

    [Column("Qtde Bomba Simples")]
    public short QtdeBombaSimples { get; set; }

    [Column("Qtde Bomba Paralela")]
    public short QtdeBombaParalela { get; set; }

    [Column("Marca Bomba Simples")]
    [StringLength(40)]
    [Unicode(false)]
    public string MarcaBombaSimples { get; set; } = null!;

    [Column("Marca Bomba Paralela")]
    [StringLength(40)]
    [Unicode(false)]
    public string MarcaBombaParalela { get; set; } = null!;

    [Column("Modelo Bomba Simples")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloBombaSimples { get; set; } = null!;

    [Column("Modelo Bomba Paralela")]
    [StringLength(40)]
    [Unicode(false)]
    public string ModeloBombaParalela { get; set; } = null!;

    [Column("Tamanho Bomba Simples")]
    [StringLength(20)]
    [Unicode(false)]
    public string TamanhoBombaSimples { get; set; } = null!;

    [Column("Tamanho Bomba Paralela")]
    [StringLength(20)]
    [Unicode(false)]
    public string TamanhoBombaParalela { get; set; } = null!;

    [Column("N Estagios Simples")]
    public short NEstagiosSimples { get; set; }

    [Column("N Estagios Paralela")]
    public short NEstagiosParalela { get; set; }

    [Column("Diametro Bomba Simples", TypeName = "decimal(8, 2)")]
    public decimal DiametroBombaSimples { get; set; }

    [Column("Diametro Bomba Paralela", TypeName = "decimal(8, 2)")]
    public decimal DiametroBombaParalela { get; set; }

    [Column("Rendimento Bomba Simples", TypeName = "decimal(8, 2)")]
    public decimal RendimentoBombaSimples { get; set; }

    [Column("Rendimento Bomba Paralela", TypeName = "decimal(8, 2)")]
    public decimal RendimentoBombaParalela { get; set; }

    [Column("Rotação Bomba Simples", TypeName = "decimal(8, 3)")]
    public decimal RotacaoBombaSimples { get; set; }

    [Column("Rotação Bomba Paralela", TypeName = "decimal(8, 3)")]
    public decimal RotacaoBombaParalela { get; set; }

    [Column("Pressao Paralela", TypeName = "decimal(8, 2)")]
    public decimal PressaoParalela { get; set; }

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

    [Column("Qtde de Motor", TypeName = "decimal(10, 2)")]
    public decimal QtdeDeMotor { get; set; }

    [Column("Valor Total das Peças", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasPecas { get; set; }

    [Column("Valor Total dos Conjuntos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosConjuntos { get; set; }

    [Column("Valor Total do Projeto", TypeName = "decimal(12, 2)")]
    public decimal ValorTotalDoProjeto { get; set; }

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column("Local de Entrega")]
    [StringLength(120)]
    [Unicode(false)]
    public string LocalDeEntrega { get; set; } = null!;

    [Column("Prazo de Entrega Previsto")]
    [StringLength(120)]
    [Unicode(false)]
    public string PrazoDeEntregaPrevisto { get; set; } = null!;

    [Column("Fone 1")]
    [StringLength(14)]
    [Unicode(false)]
    public string Fone1 { get; set; } = null!;

    [Column("Sequencia do Vendedor")]
    public int SequenciaDoVendedor { get; set; }

    [Column("Fixo ou Rebocavel")]
    [StringLength(9)]
    [Unicode(false)]
    public string FixoOuRebocavel { get; set; } = null!;

    [Column("Sequencia do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Total dos Serviços", TypeName = "decimal(10, 2)")]
    public decimal TotalDosServicos { get; set; }

    [Column("Sequencia do Pneu")]
    public int SequenciaDoPneu { get; set; }

    [Column("Gerou Encargos")]
    public bool GerouEncargos { get; set; }

    [Column("Atualizou Lista")]
    public bool AtualizouLista { get; set; }

    [Column("Marca Bomba Aux")]
    [StringLength(20)]
    [Unicode(false)]
    public string MarcaBombaAux { get; set; } = null!;

    [Column("Modelo Bomba Aux")]
    [StringLength(25)]
    [Unicode(false)]
    public string ModeloBombaAux { get; set; } = null!;

    [Column("Rotor Bomba Aux", TypeName = "decimal(8, 3)")]
    public decimal RotorBombaAux { get; set; }

    [Column("Rotação Bomba Aux", TypeName = "decimal(8, 3)")]
    public decimal RotacaoBombaAux { get; set; }

    [Column("Mat Bomba Aux")]
    [StringLength(25)]
    [Unicode(false)]
    public string MatBombaAux { get; set; } = null!;

    [Column("Vazao Bomba Aux", TypeName = "decimal(8, 2)")]
    public decimal VazaoBombaAux { get; set; }

    [Column("Pressao Bomba Aux", TypeName = "decimal(8, 2)")]
    public decimal PressaoBombaAux { get; set; }

    [Column("Rendimento Bomba Aux", TypeName = "decimal(8, 2)")]
    public decimal RendimentoBombaAux { get; set; }

    [Column("BHP Bomba Aux", TypeName = "decimal(8, 2)")]
    public decimal BhpBombaAux { get; set; }

    [Column("Valor do dolar", TypeName = "decimal(7, 4)")]
    public decimal ValorDoDolar { get; set; }

    [Column("Qtde bomba aux")]
    public short QtdeBombaAux { get; set; }

    [Column("Tamanho bomba aux")]
    [StringLength(20)]
    [Unicode(false)]
    public string TamanhoBombaAux { get; set; } = null!;

    [Column("N estagio bomba aux")]
    public short NEstagioBombaAux { get; set; }

    [Column("Diam bomba aux")]
    public short DiamBombaAux { get; set; }

    [Column("Venda Fechada")]
    public bool VendaFechada { get; set; }

    [Column("Entrega Tecnica")]
    [StringLength(19)]
    [Unicode(false)]
    public string EntregaTecnica { get; set; } = null!;

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

    [Column("Modelo Pivo")]
    [StringLength(6)]
    [Unicode(false)]
    public string ModeloPivo { get; set; } = null!;

    [Column("Fabricante Spray Final")]
    [StringLength(9)]
    [Unicode(false)]
    public string FabricanteSprayFinal { get; set; } = null!;

    [Column("Canhão ou Aspersor")]
    [StringLength(8)]
    [Unicode(false)]
    public string CanhaoOuAspersor { get; set; } = null!;

    [Column("Inicio do Balanço")]
    public short InicioDoBalanco { get; set; }

    [Column("Sequencia do Bocal")]
    public int SequenciaDoBocal { get; set; }

    [Column("Bomba Booster")]
    public bool BombaBooster { get; set; }

    [Column("CV Bomba Aux", TypeName = "decimal(7, 2)")]
    public decimal CvBombaAux { get; set; }

    [Column("Codigo do Conversor")]
    public int CodigoDoConversor { get; set; }

    [Column("Outras Despesas", TypeName = "decimal(10, 2)")]
    public decimal OutrasDespesas { get; set; }

    [Column("CPF Avulso")]
    [StringLength(14)]
    [Unicode(false)]
    public string CpfAvulso { get; set; } = null!;

    [Column("cel avulso")]
    [StringLength(14)]
    [Unicode(false)]
    public string CelAvulso { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Obs { get; set; } = null!;
}
