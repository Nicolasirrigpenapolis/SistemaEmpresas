using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Models;
using SistemaEmpresas.Models.Transporte;

namespace SistemaEmpresas.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdicaoDaDeclaracao> AdicoesDaDeclaracaos { get; set; }

    public virtual DbSet<Adutora> Adutoras { get; set; }

    public virtual DbSet<Advogado> Advogados { get; set; }

    public virtual DbSet<AgendamentoDeBackup> AgendamentoDeBackups { get; set; }

    public virtual DbSet<Agencia> Agencias { get; set; }

    public virtual DbSet<AlteracaoBaixaConta> AlteracaoBaixaContas { get; set; }

    public virtual DbSet<AspersorFinal> AspersorFinals { get; set; }

    public virtual DbSet<Aco> Acoes { get; set; }

    public virtual DbSet<BaixaComissaoLote> BaixaComissaoLotes { get; set; }

    public virtual DbSet<BaixaComissaoLoteConta> BaixaComissaoLoteContas { get; set; }

    public virtual DbSet<BaixaConta> BaixaContas { get; set; }

    public virtual DbSet<BaixaDoEstoqueContabil> BaixaDoEstoqueContabils { get; set; }

    public virtual DbSet<BaixaIndustrializacao> BaixaIndustrializacaos { get; set; }

    public virtual DbSet<BaixaMpConjunto> BaixaMpConjuntos { get; set; }

    public virtual DbSet<BaixaMpProduto> BaixaMpProdutos { get; set; }

    public virtual DbSet<BocalAspersorNelson> BocalAspersorNelsons { get; set; }

    public virtual DbSet<BxConsumoPedidoCompra> BxConsumoPedidoCompras { get; set; }

    public virtual DbSet<BxDespesaPedidoCompra> BxDespesasPedidoCompras { get; set; }

    public virtual DbSet<BxProdutoPedidoCompra> BxProdutosPedidoCompras { get; set; }

    public virtual DbSet<Calendario> Calendarios { get; set; }

    public virtual DbSet<CancelamentoNfe> CancelamentoNves { get; set; }

    public virtual DbSet<CartaDeCorrecaoNfe> CartaDeCorrecaoNves { get; set; }

    public virtual DbSet<CheckListMaquina> CheckListMaquinas { get; set; }

    public virtual DbSet<ChequeCancelado> ChequesCancelados { get; set; }

    public virtual DbSet<ClassificacaoFiscal> ClassificacaoFiscals { get; set; }

    public virtual DbSet<ClassTrib> ClassTribs { get; set; }

    public virtual DbSet<ClienteProcesso> ClientesProcessos { get; set; }

    public virtual DbSet<CobrarFornecedor> CobrarFornecedors { get; set; }

    public virtual DbSet<Comissao> Comissaos { get; set; }

    public virtual DbSet<ComissaoDoMontador> ComissaoDoMontadors { get; set; }

    public virtual DbSet<ComposicaoDoEquipamento> ComposicaoDoEquipamentos { get; set; }

    public virtual DbSet<ConciliaContaAntecipadum> ConciliaContaAntecipada { get; set; }

    public virtual DbSet<ConciliacaoDeCheque> ConciliacaoDeCheques { get; set; }

    public virtual DbSet<ConfiguracaoIntegracao> ConfiguracaoIntegracaos { get; set; }

    public virtual DbSet<Conjunto> Conjuntos { get; set; }

    public virtual DbSet<ConjuntoDaNotaFiscal> ConjuntosDaNotaFiscals { get; set; }

    public virtual DbSet<ConjuntoDaOrdemDeServico> ConjuntosDaOrdemDeServicos { get; set; }

    public virtual DbSet<ConjuntoDoMovimentoEstoque> ConjuntosDoMovimentoEstoques { get; set; }

    public virtual DbSet<ConjuntoDoOrcamento> ConjuntosDoOrcamentos { get; set; }

    public virtual DbSet<ConjuntoDoPedido> ConjuntosDoPedidos { get; set; }

    public virtual DbSet<ConjuntoDoProjeto> ConjuntosDoProjetos { get; set; }

    public virtual DbSet<ConjuntoMovimentoContabil> ConjuntosMovimentoContabils { get; set; }

    public virtual DbSet<ConjuntoMvtoContabilNovo> ConjuntosMvtoContabilNovos { get; set; }

    public virtual DbSet<ConsultaNotaDestinadum> ConsultaNotasDestinada { get; set; }

    public virtual DbSet<ConsumoDoPedidoCompra> ConsumoDoPedidoCompras { get; set; }

    public virtual DbSet<ContaContabil> ContaContabils { get; set; }

    public virtual DbSet<ContaCorrenteDaAgencium> ContaCorrenteDaAgencia { get; set; }

    public virtual DbSet<ContaDoVendedor> ContaDoVendedors { get; set; }

    public virtual DbSet<ControleDeCompra> ControleDeCompras { get; set; }

    public virtual DbSet<ControleDeGarantium> ControleDeGarantia { get; set; }

    public virtual DbSet<ControleDePneu> ControleDePneus { get; set; }

    public virtual DbSet<ControleDeProcesso> ControleDeProcessos { get; set; }

    public virtual DbSet<CorrecaoBloko> CorrecaoBlokoKs { get; set; }

    public virtual DbSet<DadoAdicionai> DadosAdicionais { get; set; }

    public virtual DbSet<DeclaracaoDeImportacao> DeclaracoesDeImportacaos { get; set; }

    public virtual DbSet<Despesa> Despesas { get; set; }

    public virtual DbSet<DespesaDaLicitacao> DespesasDaLicitacaos { get; set; }

    public virtual DbSet<DespesaDoMovimentoContabil> DespesasDoMovimentoContabils { get; set; }

    public virtual DbSet<DespesaDoMovimentoEstoque> DespesasDoMovimentoEstoques { get; set; }

    public virtual DbSet<DespesaDoNovoPedido> DespesasDoNovoPedidos { get; set; }

    public virtual DbSet<DespesaDoPedidoCompra> DespesasDoPedidoCompras { get; set; }

    public virtual DbSet<DespesaEVenda> DespesasEVendas { get; set; }

    public virtual DbSet<DespesaMvtoContabilNovo> DespesasMvtoContabilNovos { get; set; }

    public virtual DbSet<DivirgenciaNfe> DivirgenciasNves { get; set; }

    public virtual DbSet<DuplicataDescontada> DuplicatasDescontadas { get; set; }

    public virtual DbSet<Emitente> Emitentes { get; set; }

    public virtual DbSet<EntradaConta> EntradaContas { get; set; }

    public virtual DbSet<FinalidadeNfe> FinalidadeNves { get; set; }

    public virtual DbSet<FollowUpVenda> FollowUpVendas { get; set; }

    public virtual DbSet<Geral> Gerals { get; set; }

    public virtual DbSet<GrupoDaDespesa> GrupoDaDespesas { get; set; }

    public virtual DbSet<GrupoDoProduto> GrupoDoProdutos { get; set; }

    public virtual DbSet<HidroturboVendido> HidroturbosVendidos { get; set; }

    public virtual DbSet<HistoricoContabil> HistoricoContabils { get; set; }

    public virtual DbSet<HistoricoDaContaCorrente> HistoricoDaContaCorrentes { get; set; }

    public virtual DbSet<Icm> Icms { get; set; }

    public virtual DbSet<Importacao> Importacaos { get; set; }

    public virtual DbSet<ImportacaoConjuntoEstoque> ImportacaoConjuntosEstoques { get; set; }

    public virtual DbSet<ImportacaoEstoque> ImportacaoEstoques { get; set; }

    public virtual DbSet<ImportacaoProdutoEstoque> ImportacaoProdutosEstoques { get; set; }

    public virtual DbSet<InutilizacaoNfe> InutilizacaoNves { get; set; }

    public virtual DbSet<InventarioPdf> InventarioPdfs { get; set; }

    public virtual DbSet<ItenDaCorrecao> ItensDaCorrecaos { get; set; }

    public virtual DbSet<ItenDaLicitacao> ItensDaLicitacaos { get; set; }

    public virtual DbSet<ItenDaOrdem> ItensDaOrdems { get; set; }

    public virtual DbSet<ItenDaProducao> ItensDaProducaos { get; set; }

    public virtual DbSet<ItenDaRequisicao> ItensDaRequisicaos { get; set; }

    public virtual DbSet<ItenDaViagem> ItensDaViagems { get; set; }

    public virtual DbSet<ItenDoConjunto> ItensDoConjuntos { get; set; }

    public virtual DbSet<ItenPendente> ItensPendentes { get; set; }

    public virtual DbSet<ItenSaidaBalcao> ItensSaidasBalcaos { get; set; }

    public virtual DbSet<IvaFromUf> IvaFromUfs { get; set; }

    public virtual DbSet<LancamentoBancarioBb> LancamentoBancarioBbs { get; set; }

    public virtual DbSet<LanceDoPivo> LancesDoPivos { get; set; }

    public virtual DbSet<LancamentoContabil> LancamentosContabils { get; set; }

    public virtual DbSet<Licitacao> Licitacaos { get; set; }

    public virtual DbSet<LinhaDeProducao> LinhaDeProducaos { get; set; }

    public virtual DbSet<LogProcessamentoIntegracao> LogProcessamentoIntegracaos { get; set; }

    public virtual DbSet<ManutencaoConta> ManutencaoContas { get; set; }

    public virtual DbSet<ManutencaoHidroturbo> ManutencaoHidroturbos { get; set; }

    public virtual DbSet<ManutencaoPivo> ManutencaoPivos { get; set; }

    public virtual DbSet<MapaDaVazao> MapaDaVazaos { get; set; }

    public virtual DbSet<MateriaPrimaOrcamento> MateriaPrimaOrcamentos { get; set; }

    public virtual DbSet<MaterialExpedicao> MaterialExpedicaos { get; set; }

    public virtual DbSet<MateriaPrima> MateriaPrimas { get; set; }

    public virtual DbSet<Motorista> Motoristas { get; set; }

    // Módulo de Transporte - Manutenção
    public virtual DbSet<ManutencaoPeca> ManutencoesPeca { get; set; }

    public virtual DbSet<ManutencaoVeiculo> ManutencoesVeiculo { get; set; }

    public virtual DbSet<MovimentacaoDaContaCorrente> MovimentacaoDaContaCorrentes { get; set; }

    public virtual DbSet<MovimentoContabilNovo> MovimentoContabilNovos { get; set; }

    public virtual DbSet<MovimentoDoEstoque> MovimentoDoEstoques { get; set; }

    public virtual DbSet<MovimentoDoEstoqueContabil> MovimentoDoEstoqueContabils { get; set; }

    public virtual DbSet<MunicipioDoRevendedore> MunicipiosDosRevendedores { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<Mva> Mvas { get; set; }

    public virtual DbSet<MvtoContaDoVendedor> MvtoContaDoVendedors { get; set; }

    public virtual DbSet<NaturezaDeOperacao> NaturezaDeOperacaos { get; set; }

    public virtual DbSet<NotaFiscal> NotaFiscals { get; set; }

    public virtual DbSet<NotaAutorizada> NotasAutorizadas { get; set; }

    public virtual DbSet<NovaLicitacao> NovaLicitacaos { get; set; }

    public virtual DbSet<OcorrenciaGarantium> OcorrenciasGarantia { get; set; }

    public virtual DbSet<OrdemDeMontagem> OrdemDeMontagems { get; set; }

    public virtual DbSet<OrdemDeServico> OrdemDeServicos { get; set; }

    public virtual DbSet<Orcamento> Orcamentos { get; set; }

    public virtual DbSet<OrcamentoDaCompra> OrcamentosDaCompras { get; set; }

    public virtual DbSet<ParametroDoSpedEcf> ParametrosDoSpedEcfs { get; set; }

    public virtual DbSet<ParcelaDaOrdem> ParcelasDaOrdems { get; set; }

    public virtual DbSet<ParcelaDaViagem> ParcelasDaViagems { get; set; }

    public virtual DbSet<ParcelaDoNovoPedido> ParcelasDoNovoPedidos { get; set; }

    public virtual DbSet<ParcelaDoProjeto> ParcelasDoProjetos { get; set; }

    public virtual DbSet<ParcelaEntradaConta> ParcelasEntradaContas { get; set; }

    public virtual DbSet<ParcelaMovimentoEstoque> ParcelasMovimentoEstoques { get; set; }

    public virtual DbSet<ParcelaMvtoContabil> ParcelasMvtoContabils { get; set; }

    public virtual DbSet<ParcelaNotaFiscal> ParcelasNotaFiscals { get; set; }

    public virtual DbSet<ParcelaOrdemDeServico> ParcelasOrdemDeServicos { get; set; }

    public virtual DbSet<ParcelaOrcamento> ParcelasOrcamentos { get; set; }

    public virtual DbSet<ParcelaPedCompraNovo> ParcelasPedCompraNovos { get; set; }

    public virtual DbSet<ParcelaPedido> ParcelasPedidos { get; set; }

    public virtual DbSet<Parametro> Parametros { get; set; }

    public virtual DbSet<ParametroDaContabilidade> ParametrosDaContabilidades { get; set; }

    public virtual DbSet<ParametroDaNfe> ParametrosDaNves { get; set; }

    public virtual DbSet<ParametroDoProduto> ParametrosDoProdutos { get; set; }

    public virtual DbSet<Paise> Paises { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDeCompraNovo> PedidoDeCompraNovos { get; set; }

    public virtual DbSet<PecaDaNotaFiscal> PecasDaNotaFiscals { get; set; }

    public virtual DbSet<PecaDaOrdemDeServico> PecasDaOrdemDeServicos { get; set; }

    public virtual DbSet<PecaDoMovimentoEstoque> PecasDoMovimentoEstoques { get; set; }

    public virtual DbSet<PecaDoOrcamento> PecasDoOrcamentos { get; set; }

    public virtual DbSet<PecaDoPedido> PecasDoPedidos { get; set; }

    public virtual DbSet<PecaDoProjeto> PecasDoProjetos { get; set; }

    public virtual DbSet<PivoVendido> PivosVendidos { get; set; }

    public virtual DbSet<PlanilhaDeAdiantamento> PlanilhaDeAdiantamentos { get; set; }

    public virtual DbSet<Pneu> Pneus { get; set; }

    public virtual DbSet<PrevisaoDePagto> PrevisoesDePagtos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<ProdutoDaLicitacao> ProdutosDaLicitacaos { get; set; }

    public virtual DbSet<ProdutoDaNotaFiscal> ProdutosDaNotaFiscals { get; set; }

    public virtual DbSet<ProdutoDaOrdemDeServico> ProdutosDaOrdemDeServicos { get; set; }

    public virtual DbSet<ProdutoDoMovimentoContabil> ProdutosDoMovimentoContabils { get; set; }

    public virtual DbSet<ProdutoDoMovimentoEstoque> ProdutosDoMovimentoEstoques { get; set; }

    public virtual DbSet<ProdutoDoNovoPedido> ProdutosDoNovoPedidos { get; set; }

    public virtual DbSet<ProdutoDoOrcamento> ProdutosDoOrcamentos { get; set; }

    public virtual DbSet<ProdutoDoPedido> ProdutosDoPedidos { get; set; }

    public virtual DbSet<ProdutoDoPedidoCompra> ProdutosDoPedidoCompras { get; set; }

    public virtual DbSet<ProdutoMvtoContabilNovo> ProdutosMvtoContabilNovos { get; set; }

    public virtual DbSet<ProjetoDeIrrigacao> ProjetoDeIrrigacaos { get; set; }

    public virtual DbSet<Propriedade> Propriedades { get; set; }

    public virtual DbSet<PropriedadeDoGeral> PropriedadesDoGerals { get; set; }

    public virtual DbSet<PermissoesTemplate> PermissoesTemplates { get; set; }

    public virtual DbSet<PermissoesTemplateDetalhe> PermissoesTemplateDetalhes { get; set; }

    public virtual DbSet<PermissoesTela> PermissoesTelas { get; set; }

    // Novas tabelas do sistema web (independentes do sistema VB6 legado)
    public virtual DbSet<GrupoUsuario> GruposUsuarios { get; set; }

    // Tabela de Logs de Auditoria
    public virtual DbSet<LogAuditoria> LogsAuditoria { get; set; }

    public virtual DbSet<PwGrupo> PwGrupos { get; set; }

    public virtual DbSet<PwTabela> PwTabelas { get; set; }

    public virtual DbSet<PwUsuario> PwUsuarios { get; set; }

    public virtual DbSet<RazaoAuxiliar> RazaoAuxiliars { get; set; }

    public virtual DbSet<ReceitaPrimarium> ReceitaPrimaria { get; set; }

    // Módulo de Transporte - Reboque e Receitas
    public virtual DbSet<Reboque> Reboques { get; set; }

    public virtual DbSet<ReceitaViagem> ReceitasViagem { get; set; }

    public virtual DbSet<RegiaoDoVendedore> RegiaoDosVendedores { get; set; }

    public virtual DbSet<RelatorioDeViagem> RelatorioDeViagems { get; set; }

    public virtual DbSet<Requisicao> Requisicaos { get; set; }

    public virtual DbSet<ResumoAuxiliar> ResumoAuxiliars { get; set; }

    public virtual DbSet<Revendedore> Revendedores { get; set; }

    public virtual DbSet<SaidaDeBalcao> SaidaDeBalcaos { get; set; }

    public virtual DbSet<SerieGerador> SerieGeradors { get; set; }

    public virtual DbSet<SerieHidroturbo> SerieHidroturbos { get; set; }

    public virtual DbSet<SerieMotoBomba> SerieMotoBombas { get; set; }

    public virtual DbSet<SeriePivo> SeriePivos { get; set; }

    public virtual DbSet<SerieRebocador> SerieRebocadors { get; set; }

    public virtual DbSet<Servico> Servicos { get; set; }

    public virtual DbSet<ServicoDaNotaFiscal> ServicosDaNotaFiscals { get; set; }

    public virtual DbSet<ServicoDaOrdem> ServicosDaOrdems { get; set; }

    public virtual DbSet<ServicoDaOrdemDeServico> ServicosDaOrdemDeServicos { get; set; }

    public virtual DbSet<ServicoDoOrcamento> ServicosDoOrcamentos { get; set; }

    public virtual DbSet<ServicoDoPedido> ServicosDoPedidos { get; set; }

    public virtual DbSet<ServicoDoProjeto> ServicosDoProjetos { get; set; }

    public virtual DbSet<Setore> Setores { get; set; }

    public virtual DbSet<SimulaEstoque> SimulaEstoques { get; set; }

    public virtual DbSet<SituacaoDoPedido> SituacaoDosPedidos { get; set; }

    public virtual DbSet<Solicitante> Solicitantes { get; set; }

    public virtual DbSet<SpyBaixaConta> SpyBaixaContas { get; set; }

    public virtual DbSet<StatuDoProcesso> StatusDoProcessos { get; set; }

    public virtual DbSet<SubGrupoDespesa> SubGrupoDespesas { get; set; }

    public virtual DbSet<SubGrupoDoProduto> SubGrupoDoProdutos { get; set; }

    public virtual DbSet<SySequencial> SysSequencials { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<TipoDeAtividade> TipoDeAtividades { get; set; }

    public virtual DbSet<TipoDeCobranca> TipoDeCobrancas { get; set; }

    public virtual DbSet<TipoDeTitulo> TipoDeTitulos { get; set; }

    public virtual DbSet<TransferenciaDeReceitum> TransferenciaDeReceita { get; set; }

    public virtual DbSet<Unidade> Unidades { get; set; }

    public virtual DbSet<ValorAdicionai> ValoresAdicionais { get; set; }

    public virtual DbSet<Vasilhame> Vasilhames { get; set; }

    public virtual DbSet<VeiculoDoMotoristum> VeiculosDoMotorista { get; set; }

    // Módulo de Transporte - Veículos e Viagens
    public virtual DbSet<Veiculo> Veiculos { get; set; }

    public virtual DbSet<Viagem> Viagens { get; set; }

    public virtual DbSet<DespesaViagem> DespesasViagem { get; set; }

    public virtual DbSet<VendedorBloqueio> VendedoresBloqueios { get; set; }

    public virtual DbSet<ViaDeTransporteDi> ViaDeTransporteDis { get; set; }

    public virtual DbSet<VinculaPedidoOrcamento> VinculaPedidoOrcamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdicaoDaDeclaracao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaAdicao).HasName("Seqüência da Adição");

            entity.HasIndex(e => new { e.SequenciaDaDeclaracao, e.NumeroDaAdicao }, "Seq Declaração e Número Adição")
                .IsUnique()
                .HasFillFactor(90);

            entity.HasOne(d => d.SequenciaDaDeclaracaoNavigation).WithMany(p => p.AdicoesDaDeclaracaos).HasConstraintName("TB_Adições_da_Declaração_FK_Seqüência_da_Declaração");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.AdicoesDaDeclaracaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Adições_da_Declaração_FK_Seqüência_do_Geral");
        });

        modelBuilder.Entity<Adutora>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaAdutora).HasName("Sequencia da Adutora");

            entity.Property(e => e.Material).HasDefaultValue("");
            entity.Property(e => e.ModeloDaAdutora).HasDefaultValue("");
        });

        modelBuilder.Entity<Advogado>(entity =>
        {
            entity.HasKey(e => e.CodigoDoAdvogado).HasName("Codigo do Advogado");

            entity.Property(e => e.Celular).HasDefaultValue("");
            entity.Property(e => e.NomeDoAdvogado).HasDefaultValue("");
        });

        modelBuilder.Entity<AgendamentoDeBackup>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoBackup).HasName("Seqüência do Backup");

            entity.HasIndex(e => e.Hora, "Hora")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.Destino).HasDefaultValue("");
            entity.Property(e => e.TipoDoBackup).HasDefaultValue("");
        });

        modelBuilder.Entity<Agencia>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaAgencia).HasName("Seqüência da Agência");

            entity.Property(e => e.Ativa).HasDefaultValue(true);
            entity.Property(e => e.Bairro).HasDefaultValue("");
            entity.Property(e => e.Cep).HasDefaultValue("");
            entity.Property(e => e.Cnpj).HasDefaultValue("");
            entity.Property(e => e.Endereco).HasDefaultValue("");
            entity.Property(e => e.NomeDaAgencia).HasDefaultValue("");
            entity.Property(e => e.NomeDoBanco).HasDefaultValue("");
            entity.Property(e => e.NumeroDaAgencia).HasDefaultValue("");
            entity.Property(e => e.NumeroDoBanco).HasDefaultValue("");
            entity.Property(e => e.Telefone).HasDefaultValue("");
            entity.Property(e => e.Uf).HasDefaultValue("");
        });

        modelBuilder.Entity<AlteracaoBaixaConta>(entity =>
        {
            entity.Property(e => e.QuemPagou).HasDefaultValue("");
            entity.Property(e => e.TpCarteira).HasDefaultValue("");
            entity.Property(e => e.UsuAlteracao).HasDefaultValue("");
        });

        modelBuilder.Entity<AspersorFinal>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoAspersor).HasName("Sequencia do Aspersor");

            entity.Property(e => e.CanhaoOuAspersor).HasDefaultValue("");
            entity.Property(e => e.ModeloDoAspersor).HasDefaultValue("");
        });

        modelBuilder.Entity<Aco>(entity =>
        {
            entity.Property(e => e.DescricaoDaAcao).HasDefaultValue("");
        });

        modelBuilder.Entity<BaixaComissaoLote>(entity =>
        {
            entity.HasKey(e => e.SeqDaBx).HasName("Seq da Bx");

            entity.Property(e => e.UsuDaBaixa).HasDefaultValue("");
        });

        modelBuilder.Entity<BaixaComissaoLoteConta>(entity =>
        {
            entity.HasKey(e => new { e.IdDaBaixa, e.IdDoAdiantamento }).HasName("Id da Baixa");

            entity.Property(e => e.Nfe).HasDefaultValue("");
        });

        modelBuilder.Entity<BaixaConta>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaBaixa).HasName("Seqüência da Baixa");

            entity.Property(e => e.Carteira).HasDefaultValue("");
            entity.Property(e => e.ClienteCarteira).HasDefaultValue("");
            entity.Property(e => e.Conta).HasDefaultValue("");
            entity.Property(e => e.Historico).HasDefaultValue("");
            entity.Property(e => e.NumeroDoCheque).HasDefaultValue("");
            entity.Property(e => e.ProcessadoAutomaticamente).HasDefaultValue(false);

            entity.HasOne(d => d.SequenciaDaManutencaoNavigation).WithMany(p => p.BaixaConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_Contas_FK_Seqüência_da_Manutenção");

            entity.HasOne(d => d.SequenciaDaMovimentacaoCcNavigation).WithMany(p => p.BaixaConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_Contas_FK_Seqüência_da_Movimentação_CC");

            entity.HasOne(d => d.ContaCorrenteDaAgencium).WithMany(p => p.BaixaConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_Contas_FK_Seqüência_da_Agência_Seqüência_da_CC_da_Agência");
        });

        modelBuilder.Entity<BaixaDoEstoqueContabil>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaBaixa).HasName("Seqüência da Baixa Estoque");

            entity.ToTable("Baixa do Estoque Contábil", tb => tb.HasTrigger("trg_ImpedirExclusaoBaixasAntigas"));

            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.Estoque).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaDespesaNavigation).WithMany(p => p.BaixaDoEstoqueContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_do_Estoque_Contábil_FK_Seqüência_da_Despesa");

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.BaixaDoEstoqueContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Conjunto");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.BaixaDoEstoqueContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.BaixaDoEstoqueContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<BaixaIndustrializacao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaBaixa).HasName("Seqüência da Bx");

            entity.HasIndex(e => new { e.SequenciaDoMovimento, e.SequenciaDoItem, e.SequenciaDaBaixa }, "Seq Mvto Seq Bx e Seq Item")
                .IsUnique()
                .HasFillFactor(90);

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.BaixaIndustrializacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_Industrialização_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<BaixaMpConjunto>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaBaixa).HasName("Seq Baixa MP Conj");
        });

        modelBuilder.Entity<BaixaMpProduto>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaBaixa).HasName("Seq Baixa MP");

            entity.HasOne(d => d.SequenciaDaMateriaPrimaNavigation).WithMany(p => p.BaixaMpProdutoSequenciaDaMateriaPrimaNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_MP_Produto_FK_Seqüência_da_Matéria_Prima");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.BaixaMpProdutoSequenciaDoProdutoNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Baixa_MP_Produto_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<BocalAspersorNelson>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoBocal).HasName("Sequencia do Bocal");

            entity.Property(e => e.FabricanteDoAspersor).HasDefaultValue("");
            entity.Property(e => e.ModeloAspersor).HasDefaultValue("");
        });

        modelBuilder.Entity<BxConsumoPedidoCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.IdDespesa }).HasName("Bx Consumo");

            entity.Property(e => e.Notas).HasDefaultValue("");
        });

        modelBuilder.Entity<BxDespesaPedidoCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.IdDaDespesa }).HasName("Bx Despesa");

            entity.Property(e => e.Notas).HasDefaultValue("");
        });

        modelBuilder.Entity<BxProdutoPedidoCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.IdDoProduto, e.SequenciaDoItem }).HasName("Bx Produto");

            entity.Property(e => e.Notas).HasDefaultValue("");
            entity.Property(e => e.Teste).HasDefaultValue("");
        });

        modelBuilder.Entity<Calendario>(entity =>
        {
            entity.HasKey(e => e.SeqDoCalendario).HasName("Seq do Calendario");

            entity.HasIndex(e => e.DtaDoFeriado, "Dta do Feriado")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.DiaDaSemana).HasDefaultValue("");
        });

        modelBuilder.Entity<CancelamentoNfe>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaCancelamentoNfe, e.SequenciaDaNotaFiscal }).HasName("Seq Can NFe e NF");

            entity.HasIndex(e => e.SequenciaDaNotaFiscal, "Seq NF")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.SequenciaCancelamentoNfe).ValueGeneratedOnAdd();
            entity.Property(e => e.Justificativa).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithOne(p => p.CancelamentoNfe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Cancelamento_NFe_FK_Seqüência_da_Nota_Fiscal");
        });

        modelBuilder.Entity<CartaDeCorrecaoNfe>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaCorrecao, e.SequenciaDaNotaFiscal }).HasName("Seq Cor e Seq NF");

            entity.HasIndex(e => new { e.SequenciaDaNotaFiscal, e.NumeroDaCorrecao }, "Seq NF e Num Cor")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.SequenciaDaCorrecao).ValueGeneratedOnAdd();
            entity.Property(e => e.JustificativaCce).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithMany(p => p.CartaDeCorrecaoNves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Carta_de_Correção_NFe_FK_Seqüência_da_Nota_Fiscal");
        });

        modelBuilder.Entity<CheckListMaquina>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProduto, e.SequenciaDoItem }).HasName("Seq_e_itemcheck");

            entity.Property(e => e.Tpproduto).HasDefaultValue("");
        });

        modelBuilder.Entity<ChequeCancelado>(entity =>
        {
            entity.HasKey(e => e.Sequencia).HasName("Sequencia do Cheque");

            entity.Property(e => e.MotivoDoCancelamento).HasDefaultValue("");
            entity.Property(e => e.NroDaConta).HasDefaultValue("");
            entity.Property(e => e.NroDoCheque).HasDefaultValue("");
        });

        modelBuilder.Entity<ClassificacaoFiscal>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaClassificacao).HasName("Seqüência da Classificação");

            entity.Property(e => e.Cest).HasDefaultValue("");
            entity.Property(e => e.DescricaoDoNcm).HasDefaultValue("");
            entity.Property(e => e.UnExterior).HasDefaultValue("");
        });

        modelBuilder.Entity<ClienteProcesso>(entity =>
        {
            entity.HasKey(e => e.CodigoDoCliente).HasName("Codigo do Cliente");

            entity.Property(e => e.NomeDoCliente).HasDefaultValue("");
        });

        modelBuilder.Entity<CobrarFornecedor>(entity =>
        {
            entity.HasKey(e => e.CodigoDaCobranca).HasName("Codigo da Cobrança");

            entity.Property(e => e.Justificacao).HasDefaultValue("");
            entity.Property(e => e.UsuarioDaCobranca).HasDefaultValue("");
        });

        modelBuilder.Entity<Comissao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaComissao).HasName("Seqüência da Comissão");

            entity.HasIndex(e => e.SequenciaDaNotaFiscal, "Seq NF Comissao")
                .IsUnique()
                .HasFillFactor(90);

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithOne(p => p.Comissao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Comissão_FK_Seqüência_da_Nota_Fiscal");
        });

        modelBuilder.Entity<ComissaoDoMontador>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaComissao).HasName("Sequencia da comissão");

            entity.Property(e => e.Nfe).HasDefaultValue("");
        });

        modelBuilder.Entity<ComposicaoDoEquipamento>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProjeto, e.SequenciaDoItem }).HasName("SeqProjeto_item");

            entity.Property(e => e.TipoDoLance).HasDefaultValue("");
        });

        modelBuilder.Entity<ConciliaContaAntecipadum>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaConciliacao).HasName("Sequencia da Conciliação");

            entity.Property(e => e.NotasDaCompra).HasDefaultValue("");
        });

        modelBuilder.Entity<ConciliacaoDeCheque>(entity =>
        {
            entity.HasKey(e => e.SeqDaConciliacao).HasName("Seq da Conciliação");
        });

        modelBuilder.Entity<ConfiguracaoIntegracao>(entity =>
        {
            entity.Property(e => e.DataUltimaAlteracao).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Conjunto>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoConjunto).HasName("Seqüência do Conjunto");

            entity.Property(e => e.AlturaDoConjunto).HasDefaultValue("");
            entity.Property(e => e.Comprimento).HasDefaultValue("");
            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.Detalhes).HasDefaultValue("");
            entity.Property(e => e.LarguraDoConjunto).HasDefaultValue("");
            entity.Property(e => e.Localizacao).HasDefaultValue("");
            entity.Property(e => e.ParteDoPivo).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.Conjuntos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaUnidadeNavigation).WithMany(p => p.Conjuntos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_FK_Seqüência_da_Unidade");

            entity.HasOne(d => d.SequenciaDoGrupoProdutoNavigation).WithMany(p => p.Conjuntos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_FK_Seqüência_do_Grupo_Produto");

            entity.HasOne(d => d.SubGrupoDoProduto).WithMany(p => p.Conjuntos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto");
        });

        modelBuilder.Entity<ConjuntoDaNotaFiscal>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaNotaFiscal, e.SequenciaConjuntoNotaFiscal }).HasName("Seq NF e Seq Conj Nota Fiscal");

            entity.Property(e => e.SequenciaConjuntoNotaFiscal).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithMany(p => p.ConjuntosDaNotaFiscals).HasConstraintName("TB_Conjuntos_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal");

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ConjuntosDaNotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_da_Nota_Fiscal_FK_Seqüência_do_Conjunto");
        });

        modelBuilder.Entity<ConjuntoDaOrdemDeServico>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaOrdemDeServico, e.SequenciaConjuntoOs }).HasName("Seq Ordem e Seq Conjunto");

            entity.Property(e => e.SequenciaConjuntoOs).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaOrdemDeServicoNavigation).WithMany(p => p.ConjuntosDaOrdemDeServicos).HasConstraintName("TB_Conjuntos_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço");

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ConjuntosDaOrdemDeServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_da_Ordem_de_Serviço_FK_Seqüência_do_Conjunto");
        });

        modelBuilder.Entity<ConjuntoDoMovimentoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaConjuntoMovimento }).HasName("Seq Movimento e Seq Conjunto");

            entity.Property(e => e.SequenciaConjuntoMovimento).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ConjuntosDoMovimentoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_do_Movimento_Estoque_FK_Seqüência_do_Conjunto");

            entity.HasOne(d => d.SequenciaDoMovimentoNavigation).WithMany(p => p.ConjuntosDoMovimentoEstoques).HasConstraintName("TB_Conjuntos_do_Movimento_Estoque_FK_Seqüência_do_Movimento");
        });

        modelBuilder.Entity<ConjuntoDoOrcamento>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoOrcamento, e.SequenciaConjuntoOrcamento }).HasName("Seq Orçamento e Seq Conj");

            entity.Property(e => e.SequenciaConjuntoOrcamento).ValueGeneratedOnAdd();
            entity.Property(e => e.ValorDoCbs).HasDefaultValue(0.0);
            entity.Property(e => e.ValorDoIbs).HasDefaultValue(0.0);

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ConjuntosDoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_do_Orçamento_FK_Seqüência_do_Conjunto");

            entity.HasOne(d => d.SequenciaDoOrcamentoNavigation).WithMany(p => p.ConjuntosDoOrcamentos).HasConstraintName("TB_Conjuntos_do_Orçamento_FK_Seqüência_do_Orçamento");
        });

        modelBuilder.Entity<ConjuntoDoPedido>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoPedido, e.SequenciaDoConjuntoPedido }).HasName("Seq Pedido e Seq Conjunto");

            entity.Property(e => e.SequenciaDoConjuntoPedido).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ConjuntosDoPedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_do_Pedido_FK_Seqüência_do_Conjunto");

            entity.HasOne(d => d.SequenciaDoPedidoNavigation).WithMany(p => p.ConjuntosDoPedidos).HasConstraintName("TB_Conjuntos_do_Pedido_FK_Seqüência_do_Pedido");
        });

        modelBuilder.Entity<ConjuntoDoProjeto>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProjeto, e.SequenciaDoItem }).HasName("Seq_e_Conjunto");

            entity.Property(e => e.ParteDoPivo).HasDefaultValue("");
        });

        modelBuilder.Entity<ConjuntoMovimentoContabil>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaConjuntoMovimento }).HasName("Seq Mvto e Seq Conjunto");

            entity.Property(e => e.SequenciaConjuntoMovimento).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ConjuntosMovimentoContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_Movimento_Contábil_FK_Seqüência_do_Conjunto");

            entity.HasOne(d => d.SequenciaDoMovimentoNavigation).WithMany(p => p.ConjuntosMovimentoContabils).HasConstraintName("TB_Conjuntos_Movimento_Contábil_FK_Seqüência_do_Movimento");
        });

        modelBuilder.Entity<ConjuntoMvtoContabilNovo>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaConjuntoMvtoNovo }).HasName("Seq Mvto e Seq Conj Novo");

            entity.Property(e => e.SequenciaConjuntoMvtoNovo).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ConjuntosMvtoContabilNovos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Conjuntos_Mvto_Contábil_Novo_FK_Seqüência_do_Conjunto");
        });

        modelBuilder.Entity<ConsultaNotaDestinadum>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaConsulta).HasName("Seqüência da Consulta");

            entity.Property(e => e.ChaveDeAcessoDaNfe).HasDefaultValue("");
            entity.Property(e => e.Cnpj).HasDefaultValue("");
            entity.Property(e => e.InscricaoEstadual).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
        });

        modelBuilder.Entity<ConsumoDoPedidoCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.IdDespesa }).HasName("Id Consumo");

            entity.HasOne(d => d.IdDaDespesaNavigation).WithMany(p => p.ConsumoDoPedidoCompras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Consumo_do_Pedido_Compra_FK_Id_da_Despesa");
        });

        modelBuilder.Entity<ContaContabil>(entity =>
        {
            entity.HasKey(e => e.CodigoContabil).HasName("Codigo Contabil");

            entity.HasIndex(e => e.ContaContabil1, "Conta_Contab")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.ContaContabil1).HasDefaultValue("");
            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<ContaCorrenteDaAgencium>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaAgencia, e.SequenciaDaCcDaAgencia }).HasName("Seq Agencia e Seq da CC");

            entity.HasIndex(e => new { e.NumeroDaContaCorrente, e.SequenciaDaAgencia }, "Número da Conta Corrente")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.SequenciaDaCcDaAgencia).ValueGeneratedOnAdd();
            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.HabilitarIntegracaoBb).HasDefaultValue(false);
            entity.Property(e => e.NumeroDaContaCorrente).HasDefaultValue("");
        });

        modelBuilder.Entity<ContaDoVendedor>(entity =>
        {
            entity.HasKey(e => e.IdDaConta).HasName("Id da Conta");

            entity.Property(e => e.GerenteRegional).HasDefaultValue("");
            entity.Property(e => e.TitularDaConta).HasDefaultValue("");
        });

        modelBuilder.Entity<ControleDeCompra>(entity =>
        {
            entity.HasIndex(e => new { e.IdDoPedido, e.SequenciaDoItem }, "Id_e_Seq_Compra").HasFillFactor(90);

            entity.Property(e => e.Comprador).HasDefaultValue("");
            entity.Property(e => e.Financeiro).HasDefaultValue("");
            entity.Property(e => e.Prazo).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
        });

        modelBuilder.Entity<ControleDeGarantium>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoControle).HasName("Sequencia do Controle");

            entity.Property(e => e.UsuarioDaAlteracao).HasDefaultValue("");
        });

        modelBuilder.Entity<ControleDePneu>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProjeto, e.SequenciaDoPneu }).HasName("Projeto_Pneu");

            entity.Property(e => e.ModeloDoPneu).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
        });

        modelBuilder.Entity<ControleDeProcesso>(entity =>
        {
            entity.HasKey(e => e.IdDoProcesso).HasName("Id do Processo");

            entity.HasOne(d => d.CodigoDoAdvogadoNavigation).WithMany(p => p.ControleDeProcessos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Controle_de_Processos_FK_Codigo_do_Advogado");

            entity.HasOne(d => d.CodigoDoStatusNavigation).WithMany(p => p.ControleDeProcessos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Controle_de_Processos_FK_Codigo_do_Status");
        });

        modelBuilder.Entity<CorrecaoBloko>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaCorrecao).HasName("Sequencia da Correção");
        });

        modelBuilder.Entity<DadoAdicionai>(entity =>
        {
            entity.HasKey(e => e.SequenciaDosDadosAdicionais).HasName("Seqüência dos Dados Adicionais");

            entity.Property(e => e.DadosAdicionais).HasDefaultValue("");
        });

        modelBuilder.Entity<DeclaracaoDeImportacao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaDeclaracao).HasName("Seqüência da Declaração");

            entity.HasIndex(e => new { e.SequenciaDaNotaFiscal, e.SequenciaProdutoNotaFiscal }, "Seq NF e Seq Prod NF")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.LocalDeDesembaraco).HasDefaultValue("");
            entity.Property(e => e.NumeroDaDeclaracao).HasDefaultValue("");
            entity.Property(e => e.UfDeDesembaraco).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.DeclaracoesDeImportacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Declarações_de_Importação_FK_Seqüência_do_Geral");
        });

        modelBuilder.Entity<Despesa>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaDespesa).HasName("Seqüência da Despesa");

            entity.Property(e => e.CodigoDeBarras).HasDefaultValue("");
            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.Localizacao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.Despesas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaUnidadeNavigation).WithMany(p => p.Despesas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_FK_Seqüência_da_Unidade");

            entity.HasOne(d => d.SequenciaGrupoDespesaNavigation).WithMany(p => p.Despesas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_FK_Seqüência_Grupo_Despesa");

            entity.HasOne(d => d.SubGrupoDespesa).WithMany(p => p.Despesas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa");
        });

        modelBuilder.Entity<DespesaDaLicitacao>(entity =>
        {
            entity.HasKey(e => new { e.CodigoDaLicitacao, e.SequenciaDoItem }).HasName("Cod_e_ItemD");

            entity.HasOne(d => d.SequenciaDaDespesaNavigation).WithMany(p => p.DespesasDaLicitacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_da_Licitação_FK_Sequencia_da_Despesa");
        });

        modelBuilder.Entity<DespesaDoMovimentoContabil>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaDaDespesaMovimento }).HasName("Seq Mvto e Seq Desp Mvto Cont");

            entity.Property(e => e.SequenciaDaDespesaMovimento).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaDespesaNavigation).WithMany(p => p.DespesasDoMovimentoContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_do_Movimento_Contábil_FK_Seqüência_da_Despesa");
        });

        modelBuilder.Entity<DespesaDoMovimentoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaDaDespesaMovimento }).HasName("Seq Mvto e Seq Despesa Mvto");

            entity.Property(e => e.SequenciaDaDespesaMovimento).ValueGeneratedOnAdd();
            entity.Property(e => e.LocalUsado).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaDespesaNavigation).WithMany(p => p.DespesasDoMovimentoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_do_Movimento_Estoque_FK_Seqüência_da_Despesa");

            entity.HasOne(d => d.SequenciaDoMovimentoNavigation).WithMany(p => p.DespesasDoMovimentoEstoques).HasConstraintName("TB_Despesas_do_Movimento_Estoque_FK_Seqüência_do_Movimento");
        });

        modelBuilder.Entity<DespesaDoNovoPedido>(entity =>
        {
            entity.HasKey(e => new { e.CodigoDoPedido, e.SequenciaDoItem }).HasName("Codig_e_Item");

            entity.HasOne(d => d.SequenciaDaDespesaNavigation).WithMany(p => p.DespesasDoNovoPedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_do_Novo_Pedido_FK_Sequencia_da_Despesa");
        });

        modelBuilder.Entity<DespesaDoPedidoCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.IdDaDespesa }).HasName("Id Despesa");

            entity.HasOne(d => d.IdDaDespesaNavigation).WithMany(p => p.DespesasDoPedidoCompras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_do_Pedido_Compra_FK_Id_da_Despesa");
        });

        modelBuilder.Entity<DespesaEVenda>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaSimulacao).HasName("Seq_simula");

            entity.Property(e => e.Ref).HasDefaultValue("");
        });

        modelBuilder.Entity<DespesaMvtoContabilNovo>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaDespesaMvtoNovo }).HasName("Seq Mvto e Seq Desp Cont Novo");

            entity.Property(e => e.SequenciaDespesaMvtoNovo).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaDespesaNavigation).WithMany(p => p.DespesasMvtoContabilNovos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Despesas_Mvto_Contábil_Novo_FK_Seqüência_da_Despesa");
        });

        modelBuilder.Entity<DivirgenciaNfe>(entity =>
        {
            entity.HasKey(e => e.CodigoDaDivirgencia).HasName("Codigo da Divirgencia");

            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
        });

        modelBuilder.Entity<DuplicataDescontada>(entity =>
        {
            entity.HasKey(e => e.SeqDaDuplicata).HasName("Seq da Duplicata");

            entity.Property(e => e.Obs).HasDefaultValue("");
            entity.Property(e => e.TpoDeCarteira).HasDefaultValue("");
        });

        modelBuilder.Entity<EntradaConta>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaEntrada).HasName("Seqüência da Entrada");

            entity.Property(e => e.Conta).HasDefaultValue("");
            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.Historico).HasDefaultValue("");
            entity.Property(e => e.TipoDaConta).HasDefaultValue("");
            entity.Property(e => e.Titulo).HasDefaultValue("");
            entity.Property(e => e.UsuarioDaAlteracao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.EntradaConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Entrada_Contas_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaGrupoDespesaNavigation).WithMany(p => p.EntradaConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Entrada_Contas_FK_Seqüência_Grupo_Despesa");

            entity.HasOne(d => d.SubGrupoDespesa).WithMany(p => p.EntradaConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Entrada_Contas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa");
        });

        modelBuilder.Entity<FinalidadeNfe>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("Codigo");

            entity.Property(e => e.Finalidade).HasDefaultValue("");
        });

        modelBuilder.Entity<FollowUpVenda>(entity =>
        {
            entity.HasKey(e => e.SeqFollowUp).HasName("Seq Follow Up");

            entity.Property(e => e.DescrDoMaterial).HasDefaultValue("");
            entity.Property(e => e.Det1).HasDefaultValue("");
            entity.Property(e => e.Det2).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
            entity.Property(e => e.SerieDoEquipamento).HasDefaultValue("");
            entity.Property(e => e.Stat).HasDefaultValue("");
            entity.Property(e => e.Telefone).HasDefaultValue("");
        });

        modelBuilder.Entity<Geral>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoGeral).HasName("Seqüência do Geral");

            entity.Property(e => e.AgenciaDoBanco1).HasDefaultValue("");
            entity.Property(e => e.AgenciaDoBanco2).HasDefaultValue("");
            entity.Property(e => e.Bairro).HasDefaultValue("");
            entity.Property(e => e.BairroDeCobranca).HasDefaultValue("");
            entity.Property(e => e.CaixaPostal).HasDefaultValue("");
            entity.Property(e => e.CaixaPostalDaCobranca).HasDefaultValue("");
            entity.Property(e => e.Celular).HasDefaultValue("");
            entity.Property(e => e.Cep).HasDefaultValue("");
            entity.Property(e => e.CepDeCobranca).HasDefaultValue("");
            entity.Property(e => e.Complemento).HasDefaultValue("");
            entity.Property(e => e.ComplementoDaCobranca).HasDefaultValue("");
            entity.Property(e => e.ContaCorrenteDoBanco1).HasDefaultValue("");
            entity.Property(e => e.ContaCorrenteDoBanco2).HasDefaultValue("");
            entity.Property(e => e.Contato).HasDefaultValue("");
            entity.Property(e => e.CpfECnpj).HasDefaultValue("");
            entity.Property(e => e.CodigoDaAntt).HasDefaultValue("");
            entity.Property(e => e.CodigoDoSuframa).HasDefaultValue("");
            entity.Property(e => e.Email).HasDefaultValue("");
            entity.Property(e => e.Endereco).HasDefaultValue("");
            entity.Property(e => e.EnderecoDeCobranca).HasDefaultValue("");
            entity.Property(e => e.Fax).HasDefaultValue("");
            entity.Property(e => e.Fone1).HasDefaultValue("");
            entity.Property(e => e.Fone2).HasDefaultValue("");
            entity.Property(e => e.HomePage).HasDefaultValue("");
            entity.Property(e => e.IntermediarioDoVendedor).HasDefaultValue("");
            entity.Property(e => e.NomeDoBanco1).HasDefaultValue("");
            entity.Property(e => e.NomeDoBanco2).HasDefaultValue("");
            entity.Property(e => e.NomeDoCorrentistaDoBanco1).HasDefaultValue("");
            entity.Property(e => e.NomeDoCorrentistaDoBanco2).HasDefaultValue("");
            entity.Property(e => e.NomeFantasia).HasDefaultValue("");
            entity.Property(e => e.NumeroDoEndereco).HasDefaultValue("");
            entity.Property(e => e.NumeroDoEnderecoDeCobranca).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
            entity.Property(e => e.RgEIe).HasDefaultValue("");
            entity.Property(e => e.UsuDaAlteracao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoMunicipioNavigation).WithMany(p => p.GeralSequenciaDoMunicipioNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Geral_FK_Seqüência_do_Município");

            entity.HasOne(d => d.SequenciaDoPaisNavigation).WithMany(p => p.Gerals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Geral_FK_Seqüência_do_País");

            entity.HasOne(d => d.SequenciaDoVendedorNavigation).WithMany(p => p.InverseSequenciaDoVendedorNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Vendedor_Geral");

            entity.HasOne(d => d.SequenciaMunicipioCobrancaNavigation).WithMany(p => p.GeralSequenciaMunicipioCobrancaNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Geral_FK_Seqüência_Município_Cobrança");
        });

        modelBuilder.Entity<GrupoDaDespesa>(entity =>
        {
            entity.HasKey(e => e.SequenciaGrupoDespesa).HasName("Seqüência Grupo Despesa");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<GrupoDoProduto>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoGrupoProduto).HasName("Seqüência do Grupo Produto");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<HidroturboVendido>(entity =>
        {
            entity.HasKey(e => e.SeqDoHidroturbo).HasName("Seq_hidro_ven");

            entity.Property(e => e.Cidade).HasDefaultValue("");
            entity.Property(e => e.ModeloDoHidroturbo).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
            entity.Property(e => e.Uf).HasDefaultValue("");
        });

        modelBuilder.Entity<HistoricoContabil>(entity =>
        {
            entity.HasKey(e => e.CodigoDoHistorico).HasName("Codigo do Historico");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<HistoricoDaContaCorrente>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoHistorico).HasName("Seqüência do Histórico");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<Icm>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoIcms).HasName("Seqüência do ICMS");

            entity.HasIndex(e => e.Uf, "UF do ICMS")
                .IsUnique()
                .HasFillFactor(90);

            // Removido: entity.Property(e => e.Regiao).HasDefaultValue("");
            entity.Property(e => e.Uf).HasDefaultValue("");
        });

        modelBuilder.Entity<ImportacaoConjuntoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaImportacaoEstoque, e.SequenciaImportacaoItem }).HasName("Seq Importação Estoque Seq Con");

            entity.Property(e => e.SequenciaImportacaoItem).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ImportacaoEstoque>(entity =>
        {
            entity.HasKey(e => e.SequenciaImportacaoEstoque).HasName("Seqüência Importação Estoque");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<ImportacaoProdutoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaImportacaoEstoque, e.SequenciaImportacaoItem }).HasName("Sq Importação Estoque Seq Prod");

            entity.Property(e => e.SequenciaImportacaoItem).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ImportacaoProdutosEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Importação_Produtos_Estoque_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<InutilizacaoNfe>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaInutilizacao).HasName("Seqüência da Inutilização");

            entity.Property(e => e.Justificativa).HasDefaultValue("");
        });

        modelBuilder.Entity<InventarioPdf>(entity =>
        {
            entity.HasKey(e => e.CodigoDoPdf).HasName("Codigo do Pdf");

            entity.Property(e => e.CodigoDoPdf).HasDefaultValue("");
            entity.Property(e => e.DataBase).HasDefaultValue("");
            entity.Property(e => e.Decricao).HasDefaultValue("");
            entity.Property(e => e.Unid).HasDefaultValue("");
        });

        modelBuilder.Entity<ItenDaCorrecao>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaCorrecao, e.SequenciaDoProduto }).HasName("SeqCorrecao_Item");
        });

        modelBuilder.Entity<ItenDaLicitacao>(entity =>
        {
            entity.HasKey(e => new { e.Sequencia, e.Produto, e.CodDespesa, e.SequencialDeUm }).HasName("Sequencia");

            entity.Property(e => e.Unidade).HasDefaultValue("");

            entity.HasOne(d => d.ProdutoNavigation).WithMany(p => p.ItensDaLicitacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Itens_da_Licitacao_FK_Produto");
        });

        modelBuilder.Entity<ItenDaOrdem>(entity =>
        {
            entity.HasKey(e => new { e.IdDaOrdem, e.SequenciaDoItem }).HasName("Id da Ordem");
        });

        modelBuilder.Entity<ItenDaProducao>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaProducao, e.SequenciaDoItem }).HasName("seq_e_item_producao");

            entity.Property(e => e.OpeardorTorno).HasDefaultValue("");
            entity.Property(e => e.OperadorCalandra).HasDefaultValue("");
            entity.Property(e => e.OperadorDobra).HasDefaultValue("");
            entity.Property(e => e.OperadorGui).HasDefaultValue("");
            entity.Property(e => e.OperadorOxi).HasDefaultValue("");
            entity.Property(e => e.OperadorPerfiladeira).HasDefaultValue("");
            entity.Property(e => e.OperadorSerra).HasDefaultValue("");
        });

        modelBuilder.Entity<ItenDaRequisicao>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaRequisicao, e.SequenciaProdutoRequisicao }).HasName("Seq Req e Seq Prod Requisição");

            entity.Property(e => e.SequenciaProdutoRequisicao).ValueGeneratedOnAdd();
            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.Veiculo).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaRequisicaoNavigation).WithMany(p => p.ItensDaRequisicaos).HasConstraintName("TB_Itens_da_Requisição_FK_Seqüência_da_Requisição");
        });

        modelBuilder.Entity<ItenDaViagem>(entity =>
        {
            entity.HasKey(e => new { e.SeqDaViagem, e.SequenciaDoItem }).HasName("Seq_e_Item_Viagem");

            entity.Property(e => e.DescricaoDoItem).HasDefaultValue("");
        });

        modelBuilder.Entity<ItenDoConjunto>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoConjunto, e.SequenciaDoProduto }).HasName("Seqüência do Item do Conjunto");

            entity.HasOne(d => d.SequenciaDoConjuntoNavigation).WithMany(p => p.ItensDoConjuntos).HasConstraintName("TB_Itens_do_Conjunto_FK_Seqüência_do_Conjunto");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ItensDoConjuntos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Itens_do_Conjunto_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ItenPendente>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoOrcamento, e.SequenciaDoItem }).HasName("Seq_orcc_e_item");

            entity.Property(e => e.Situacao).HasDefaultValue("");
        });

        modelBuilder.Entity<ItenSaidaBalcao>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaSaida, e.SequenciaDoItem }).HasName("Seq_B_Item");
        });

        modelBuilder.Entity<IvaFromUf>(entity =>
        {
            entity.HasKey(e => new { e.IdMva, e.Uf, e.Ncm }).HasName("ID MVA");

            entity.Property(e => e.Uf).HasDefaultValue("");
            entity.Property(e => e.Teste).HasDefaultValue("");
        });

        modelBuilder.Entity<LancamentoBancarioBb>(entity =>
        {
            entity.Property(e => e.DataImportacao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IndicadorCheque).IsFixedLength();
            entity.Property(e => e.TipoLancamento).IsFixedLength();
        });

        modelBuilder.Entity<LanceDoPivo>(entity =>
        {
            entity.HasKey(e => new { e.ModeloDoLance, e.DescricaoDoLance }).HasName("Modelo do Lance");

            entity.Property(e => e.DescricaoDoLance).HasDefaultValue("");
        });

        modelBuilder.Entity<LancamentoContabil>(entity =>
        {
            entity.HasKey(e => e.IdDoLancamento).HasName("Id do Lançamento");

            entity.Property(e => e.ComplementoDoHist).HasDefaultValue("");
            entity.Property(e => e.DtDoLancamento).HasDefaultValue("");
        });

        modelBuilder.Entity<Licitacao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaLicitacao).HasName("Sequencia da Licitacao");

            entity.Property(e => e.CondPagto1).HasDefaultValue("");
            entity.Property(e => e.CondPagto2).HasDefaultValue("");
            entity.Property(e => e.CondPagto3).HasDefaultValue("");
            entity.Property(e => e.Contato1).HasDefaultValue("");
            entity.Property(e => e.Contato2).HasDefaultValue("");
            entity.Property(e => e.Contato3).HasDefaultValue("");
            entity.Property(e => e.Fone1).HasDefaultValue("");
            entity.Property(e => e.Fone2).HasDefaultValue("");
            entity.Property(e => e.Fone3).HasDefaultValue("");
            entity.Property(e => e.For1).HasDefaultValue("");
            entity.Property(e => e.For2).HasDefaultValue("");
            entity.Property(e => e.For3).HasDefaultValue("");
            entity.Property(e => e.PrevEntrega1).HasDefaultValue("");
            entity.Property(e => e.PrevEntrega2).HasDefaultValue("");
            entity.Property(e => e.PrevEntrega3).HasDefaultValue("");
        });

        modelBuilder.Entity<LinhaDeProducao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaProducao).HasName("Sequencia da Produção");

            entity.Property(e => e.SolicitacaoDe).HasDefaultValue("");

            entity.HasOne(d => d.CodigoDoSetorNavigation).WithMany(p => p.LinhaDeProducaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Linha_de_Produção_FK_Codigo_do_setor");
        });

        modelBuilder.Entity<LogProcessamentoIntegracao>(entity =>
        {
            entity.Property(e => e.DataHora).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ManutencaoConta>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaManutencao).HasName("Seqüência Manutenção");

            entity.Property(e => e.Conta).HasDefaultValue("");
            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.Historico).HasDefaultValue("");
            entity.Property(e => e.NotasDaCompra).HasDefaultValue("");
            entity.Property(e => e.TipoDaConta).HasDefaultValue("");
            entity.Property(e => e.Titulo).HasDefaultValue("");
            entity.Property(e => e.TpoDeRecebimento).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaCobrancaNavigation).WithMany(p => p.ManutencaoConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Manutenção_Contas_FK_Seqüência_da_Cobrança");

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithMany(p => p.ManutencaoConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Manutenção_Contas_FK_Seqüência_da_Nota_Fiscal");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.ManutencaoConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Manutenção_Contas_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaGrupoDespesaNavigation).WithMany(p => p.ManutencaoConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Manutenção_Contas_FK_Seqüência_Grupo_Despesa");

            entity.HasOne(d => d.SubGrupoDespesa).WithMany(p => p.ManutencaoConta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Manutenção_Contas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa");
        });

        modelBuilder.Entity<ManutencaoHidroturbo>(entity =>
        {
            entity.HasKey(e => new { e.SeqDoHidroturbo, e.SequenciaDoItem }).HasName("SeqH_e_Item");

            entity.Property(e => e.DescricaoDaManutencao).HasDefaultValue("");
        });

        modelBuilder.Entity<ManutencaoPivo>(entity =>
        {
            entity.HasKey(e => new { e.SeqDoPivo, e.SequenciaDoItem }).HasName("SeqPivo_e_Item");

            entity.Property(e => e.DescricaoDaManutencao).HasDefaultValue("");
        });

        modelBuilder.Entity<MapaDaVazao>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProjeto, e.SequenciaDoItem }).HasName("Projeto_Vazao");
        });

        modelBuilder.Entity<MateriaPrimaOrcamento>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaExpedicao).HasName("Seqexpedicao");

            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.Localizacao).HasDefaultValue("");
            entity.Property(e => e.SiglaDaUnidade).HasDefaultValue("");
        });

        modelBuilder.Entity<MaterialExpedicao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaExpedicao).HasName("Sequencia da Expedição");

            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.Localizacao).HasDefaultValue("");
            entity.Property(e => e.SiglaDaUnidade).HasDefaultValue("");
        });

        modelBuilder.Entity<MateriaPrima>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaMateriaPrima, e.SequenciaDoProduto }).HasName("Seq Matéria Prima e Seq Prod");

            entity.HasOne(d => d.SequenciaDaMateriaPrimaNavigation).WithMany(p => p.MateriaPrimaSequenciaDaMateriaPrimaNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Matéria_Prima_FK_Seqüência_da_Matéria_Prima");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.MateriaPrimaSequenciaDoProdutoNavigations).HasConstraintName("TB_Matéria_Prima_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<Motorista>(entity =>
        {
            entity.HasKey(e => e.CodigoDoMotorista).HasName("Codigo do Motorista");

            entity.Property(e => e.Bairro).HasDefaultValue("");
            entity.Property(e => e.Cel).HasDefaultValue("");
            entity.Property(e => e.Cep).HasDefaultValue("");
            entity.Property(e => e.Cpf).HasDefaultValue("");
            entity.Property(e => e.Endereco).HasDefaultValue("");
            entity.Property(e => e.Fone).HasDefaultValue("");
            entity.Property(e => e.NomeDoMotorista).HasDefaultValue("");
            entity.Property(e => e.Numero).HasDefaultValue("");
            entity.Property(e => e.Rg).HasDefaultValue("");
            entity.Property(e => e.Uf).HasDefaultValue("");
        });

        modelBuilder.Entity<MovimentacaoDaContaCorrente>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaMovimentacaoCc).HasName("Seqüência da Movimentação CC");

            entity.Property(e => e.Conta).HasDefaultValue("");
            entity.Property(e => e.Historico).HasDefaultValue("");
            entity.Property(e => e.OrigemDaMovimentacao).HasDefaultValue("");
            entity.Property(e => e.TipoDeMovimentoDaCc).HasDefaultValue("");
            entity.Property(e => e.UsuarioDaAlteracao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoHistoricoNavigation).WithMany(p => p.MovimentacaoDaContaCorrentes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimentação_da_Conta_Corrente_FK_Seqüência_do_Histórico");

            entity.HasOne(d => d.ContaCorrenteDaAgencium).WithMany(p => p.MovimentacaoDaContaCorrentes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimentação_da_Conta_Corrente_FK_Seqüência_da_Agência_Seqüência_da_CC_da_Agência");
        });

        modelBuilder.Entity<MovimentoContabilNovo>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoMovimento).HasName("Seq Mvto Contabil Novo");

            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");
            entity.Property(e => e.Titulo).HasDefaultValue("");
            entity.Property(e => e.UsuarioDaAlteracao).HasDefaultValue("");
        });

        modelBuilder.Entity<MovimentoDoEstoque>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoMovimento).HasName("Seq Movimento");

            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");
            entity.Property(e => e.Titulo).HasDefaultValue("");
            entity.Property(e => e.UsuarioDaAlteracao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.MovimentoDoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimento_do_Estoque_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaPropriedadeNavigation).WithMany(p => p.MovimentoDoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimento_do_Estoque_FK_Seqüência_da_Propriedade");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.MovimentoDoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimento_do_Estoque_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaGrupoDespesaNavigation).WithMany(p => p.MovimentoDoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimento_do_Estoque_FK_Seqüência_Grupo_Despesa");

            entity.HasOne(d => d.SubGrupoDespesa).WithMany(p => p.MovimentoDoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimento_do_Estoque_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa");
        });

        modelBuilder.Entity<MovimentoDoEstoqueContabil>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoMovimento).HasName("Seq Mvto Contabil");

            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.MovimentoDoEstoqueContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Movimento_do_Estoque_Contábil_FK_Seqüência_do_Geral");
        });

        modelBuilder.Entity<MunicipioDoRevendedore>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaRevenda, e.SequenciaDoItem, e.IdDaConta }).HasName("Seq_e_Revendedor");

            entity.Property(e => e.Reg).HasDefaultValue("");
            entity.Property(e => e.Uf).HasDefaultValue("");
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.ToTable("Municípios");
            entity.HasKey(e => e.SequenciaDoMunicipio).HasName("Seqüência do Município");

            entity.Property(e => e.SequenciaDoMunicipio).HasColumnName("Seqüência do Município");
            entity.Property(e => e.Descricao).HasColumnName("Descrição").HasDefaultValue("");
            entity.Property(e => e.Uf).HasColumnName("UF").HasDefaultValue("");
            entity.Property(e => e.CodigoDoIbge).HasColumnName("Código do IBGE");
            entity.Property(e => e.Cep).HasColumnName("CEP").HasDefaultValue("");
        });

        modelBuilder.Entity<Mva>(entity =>
        {
            entity.HasKey(e => new { e.IdMva, e.Uf }).HasName("ID_UF");

            entity.Property(e => e.Uf).HasDefaultValue("");
        });

        modelBuilder.Entity<MvtoContaDoVendedor>(entity =>
        {
            entity.HasKey(e => e.IdDoMovimento).HasName("Id do Movimento");

            entity.Property(e => e.Historico).HasDefaultValue("");
        });

        modelBuilder.Entity<NaturezaDeOperacao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaNatureza).HasName("Seqüência da Natureza");

            entity.Property(e => e.DescricaoDaNaturezaOperacao).HasDefaultValue("");
        });

        modelBuilder.Entity<NotaFiscal>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaNotaFiscal).HasName("Seqüência da Nota Fiscal");

            entity.Property(e => e.ChaveAcessoNfeReferenciada).HasDefaultValue("");
            entity.Property(e => e.ChaveDaDevolucao).HasDefaultValue("");
            entity.Property(e => e.ChaveDaDevolucao2).HasDefaultValue("");
            entity.Property(e => e.ChaveDaDevolucao3).HasDefaultValue("");
            entity.Property(e => e.ChaveDeAcessoDaNfe).HasDefaultValue("");
            entity.Property(e => e.CodigoDaAntt).HasDefaultValue("");
            entity.Property(e => e.DataEHoraDaNfe).HasDefaultValue("");
            entity.Property(e => e.DescricaoConjuntoAvulso).HasDefaultValue("");
            entity.Property(e => e.DocumentoDaTransportadora).HasDefaultValue("");
            entity.Property(e => e.EnderecoDaTransportadora).HasDefaultValue("");
            entity.Property(e => e.Especie).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.Frete).HasDefaultValue("");
            entity.Property(e => e.Historico).HasDefaultValue("");
            entity.Property(e => e.IeDaTransportadora).HasDefaultValue("");
            entity.Property(e => e.Marca).HasDefaultValue("");
            entity.Property(e => e.NomeDaTransportadoraAvulsa).HasDefaultValue("");
            entity.Property(e => e.Numeracao).HasDefaultValue("");
            entity.Property(e => e.NumeroDoReciboDaNfe).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");
            entity.Property(e => e.PlacaDoVeiculo).HasDefaultValue("");
            entity.Property(e => e.ProtocoloDeAutorizacaoNfe).HasDefaultValue("");
            entity.Property(e => e.ReciboNfse).HasDefaultValue("");
            entity.Property(e => e.UfDoVeiculo).HasDefaultValue("");

            entity.HasOne(d => d.MunicipioDaTransportadoraNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Município_da_Transportadora");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaCobrancaNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_da_Cobrança");

            entity.HasOne(d => d.SequenciaDaNaturezaNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_da_Natureza");

            entity.HasOne(d => d.SequenciaDaPropriedadeNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_da_Propriedade");

            entity.HasOne(d => d.SequenciaDaTransportadoraNavigation).WithMany(p => p.NotaFiscalSequenciaDaTransportadoraNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_da_Transportadora");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.NotaFiscalSequenciaDoGeralNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaDoMovimentoNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_do_Movimento");

            entity.HasOne(d => d.SequenciaDoPedidoNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_do_Pedido");

            entity.HasOne(d => d.SequenciaDoVendedorNavigation).WithMany(p => p.NotaFiscalSequenciaDoVendedorNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nota_Fiscal_FK_Seqüência_do_Vendedor");
        });

        modelBuilder.Entity<NotaAutorizada>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoNotas, e.SequenciaDaNotaFiscal }).HasName("Seq Notas E Seq NF");

            entity.HasIndex(e => e.SequenciaDaNotaFiscal, "Seq NF Notas")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.SequenciaDoNotas).ValueGeneratedOnAdd();
            entity.Property(e => e.Xml).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithOne(p => p.NotasAutorizada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Notas_Autorizadas_FK_Seqüência_da_Nota_Fiscal");
        });

        modelBuilder.Entity<NovaLicitacao>(entity =>
        {
            entity.HasKey(e => e.CodigoDaLicitacao).HasName("Codigo da Licitação");

            entity.Property(e => e.Comprador).HasDefaultValue("");
            entity.Property(e => e.Contato).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.NomeDoVendedor).HasDefaultValue("");
            entity.Property(e => e.Observacoes).HasDefaultValue("");
            entity.Property(e => e.TipoDaLicitacao).HasDefaultValue("");
            entity.Property(e => e.TipoDeFrete).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaTransportadoraNavigation).WithMany(p => p.NovaLicitacaoSequenciaDaTransportadoraNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nova_Licitação_FK_Sequencia_da_Transportadora");

            entity.HasOne(d => d.SequenciaDoFornecedorNavigation).WithMany(p => p.NovaLicitacaoSequenciaDoFornecedorNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Nova_Licitação_FK_Sequencia_do_Fornecedor");
        });

        modelBuilder.Entity<OcorrenciaGarantium>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoControle, e.SequenciaDoItem }).HasName("Seq_Prod_Controle");

            entity.Property(e => e.NotasDaCompra).HasDefaultValue("");
            entity.Property(e => e.Ocorrencia).HasDefaultValue("");
        });

        modelBuilder.Entity<OrdemDeMontagem>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaMontagem).HasName("Sequencia da Montagem");

            entity.Property(e => e.Obs).HasDefaultValue("");
            entity.Property(e => e.Origem).HasDefaultValue("");
        });

        modelBuilder.Entity<OrdemDeServico>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaOrdemDeServico).HasName("Seqüência da Ordem de Serviço");

            entity.Property(e => e.Bairro).HasDefaultValue("");
            entity.Property(e => e.CaixaPostal).HasDefaultValue("");
            entity.Property(e => e.Cep).HasDefaultValue("");
            entity.Property(e => e.Complemento).HasDefaultValue("");
            entity.Property(e => e.CorDoTrator).HasDefaultValue("");
            entity.Property(e => e.CpfECnpj).HasDefaultValue("");
            entity.Property(e => e.CodigoDoSuframa).HasDefaultValue("");
            entity.Property(e => e.Email).HasDefaultValue("");
            entity.Property(e => e.Endereco).HasDefaultValue("");
            entity.Property(e => e.Fax).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.ModeloDoTrator).HasDefaultValue("");
            entity.Property(e => e.NomeCliente).HasDefaultValue("");
            entity.Property(e => e.NomeDaPropriedade).HasDefaultValue("");
            entity.Property(e => e.NumeroDoChassiDoTrator).HasDefaultValue("");
            entity.Property(e => e.NumeroDoEndereco).HasDefaultValue("");
            entity.Property(e => e.NumeroDoMotorDoTrator).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");
            entity.Property(e => e.RgEIe).HasDefaultValue("");
            entity.Property(e => e.Telefone).HasDefaultValue("");
            entity.Property(e => e.TipoDoRelatorio).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.OrdemDeServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Ordem_de_Serviço_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaPropriedadeNavigation).WithMany(p => p.OrdemDeServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Ordem_de_Serviço_FK_Seqüência_da_Propriedade");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.OrdemDeServicoSequenciaDoGeralNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Ordem_de_Serviço_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaDoMunicipioNavigation).WithMany(p => p.OrdemDeServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Ordem_de_Serviço_FK_Seqüência_do_Município");

            entity.HasOne(d => d.SequenciaDoVendedorNavigation).WithMany(p => p.OrdemDeServicoSequenciaDoVendedorNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Ordem_de_Serviço_FK_Seqüência_do_Vendedor");
        });

        modelBuilder.Entity<Orcamento>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoOrcamento).HasName("Seqüência do Orçamento");

            entity.HasIndex(e => e.NumeroDaProforma, "Número da Proforma").HasFillFactor(90);

            entity.Property(e => e.Aspersor).HasDefaultValue("");
            entity.Property(e => e.AvisoDeEmbarque).HasDefaultValue("");
            entity.Property(e => e.Bairro).HasDefaultValue("");
            entity.Property(e => e.CaixaPostal).HasDefaultValue("");
            entity.Property(e => e.Cep).HasDefaultValue("");
            entity.Property(e => e.Complemento).HasDefaultValue("");
            entity.Property(e => e.CpfECnpj).HasDefaultValue("");
            entity.Property(e => e.CodigoDoSuframa).HasDefaultValue("");
            entity.Property(e => e.DescricaoConjuntoAvulso).HasDefaultValue("");
            entity.Property(e => e.Email).HasDefaultValue("");
            entity.Property(e => e.Endereco).HasDefaultValue("");
            entity.Property(e => e.EntregaTecnica).HasDefaultValue("");
            entity.Property(e => e.Fax).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.Frete).HasDefaultValue("");
            entity.Property(e => e.Hidroturbo).HasDefaultValue("");
            entity.Property(e => e.LocalDeEmbarque).HasDefaultValue("");
            entity.Property(e => e.MarcaBomba).HasDefaultValue("");
            entity.Property(e => e.MarcaDoMotor).HasDefaultValue("");
            entity.Property(e => e.ModeloBomba).HasDefaultValue("");
            entity.Property(e => e.ModeloDoAspersor).HasDefaultValue("");
            entity.Property(e => e.ModeloHidroturbo).HasDefaultValue("");
            entity.Property(e => e.ModeloMotor).HasDefaultValue("");
            entity.Property(e => e.NivelDeProtecao).HasDefaultValue("");
            entity.Property(e => e.NomeCliente).HasDefaultValue("");
            entity.Property(e => e.NomeDaPropriedade).HasDefaultValue("");
            entity.Property(e => e.NumAntt).IsFixedLength();
            entity.Property(e => e.NumeroDoEndereco).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");
            entity.Property(e => e.Pneus).HasDefaultValue("");
            entity.Property(e => e.Rebiut).HasDefaultValue("");
            entity.Property(e => e.RgEIe).HasDefaultValue("");
            entity.Property(e => e.TamanhoBomba).HasDefaultValue("");
            entity.Property(e => e.Telefone).HasDefaultValue("");
            entity.Property(e => e.TotalCbs).HasDefaultValue(0.0);
            entity.Property(e => e.TotalIbs).HasDefaultValue(0.0);
            entity.Property(e => e.Tubos).HasDefaultValue("");
            entity.Property(e => e.UfDeEmbarque).HasDefaultValue("");
            entity.Property(e => e.UfPlaca).IsFixedLength();
            entity.Property(e => e.UsuarioDaAlteracao).HasDefaultValue("");
            entity.Property(e => e.VendedorIntermediario).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.Orcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Orçamento_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaPropriedadeNavigation).WithMany(p => p.Orcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Orçamento_FK_Seqüência_da_Propriedade");

            entity.HasOne(d => d.SequenciaDaTransportadoraNavigation).WithMany(p => p.OrcamentoSequenciaDaTransportadoraNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Orçamento_FK_Seqüência_da_Transportadora");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.OrcamentoSequenciaDoGeralNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Orçamento_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaDoMunicipioNavigation).WithMany(p => p.Orcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Orçamento_FK_Seqüência_do_Município");

            entity.HasOne(d => d.SequenciaDoPaisNavigation).WithMany(p => p.Orcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Orçamento_FK_Seqüência_do_País");

            entity.HasOne(d => d.SequenciaDoVendedorNavigation).WithMany(p => p.OrcamentoSequenciaDoVendedorNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Orçamento_FK_Seqüência_do_Vendedor");
        });

        modelBuilder.Entity<OrcamentoDaCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.SequenciaDoItem }).HasName("Id_orc");
        });

        modelBuilder.Entity<ParametroDoSpedEcf>(entity =>
        {
            entity.Property(e => e.Bairro).HasDefaultValue("");
            entity.Property(e => e.Cep).HasDefaultValue("");
            entity.Property(e => e.Cnpj).HasDefaultValue("");
            entity.Property(e => e.ComplementoDoEndereco).HasDefaultValue("");
            entity.Property(e => e.CpfContabilista).HasDefaultValue("");
            entity.Property(e => e.Crc).HasDefaultValue("");
            entity.Property(e => e.Empresa).HasDefaultValue("");
            entity.Property(e => e.Endereco).HasDefaultValue("");
            entity.Property(e => e.Fone).HasDefaultValue("");
            entity.Property(e => e.NomeDoContabilista).HasDefaultValue("");
            entity.Property(e => e.VersaoSped).HasDefaultValue("");
        });

        modelBuilder.Entity<ParcelaDaOrdem>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaMontagem, e.NumeroDaParcela }).HasName("Seq1_e_pc1");
        });

        modelBuilder.Entity<ParcelaDaViagem>(entity =>
        {
            entity.HasKey(e => new { e.SeqDaViagem, e.NumeroDaParcela }).HasName("Seq_e_Pc");
        });

        modelBuilder.Entity<ParcelaDoNovoPedido>(entity =>
        {
            entity.HasKey(e => new { e.CodigoDoPedido, e.NumeroDaParcela }).HasName("CodPedido_e_Pc");
        });

        modelBuilder.Entity<ParcelaDoProjeto>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProjeto, e.NumeroDaParcela }).HasName("SeqProjeto_e_Parcela");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<ParcelaEntradaConta>(entity =>
        {
            entity.HasKey(e => new { e.NumeroDaParcela, e.SequenciaDaEntrada }).HasName("Num Parcela e Seq da Entrada");

            entity.HasOne(d => d.SequenciaDaEntradaNavigation).WithMany(p => p.ParcelasEntradaConta).HasConstraintName("TB_Parcelas_Entrada_Contas_FK_Seqüência_da_Entrada");
        });

        modelBuilder.Entity<ParcelaMovimentoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.NumeroDaParcela, e.SequenciaDoMovimento }).HasName("Seq Movimento e Pc");

            entity.HasOne(d => d.SequenciaDoMovimentoNavigation).WithMany(p => p.ParcelasMovimentoEstoques).HasConstraintName("TB_Parcelas_Movimento_Estoque_FK_Seqüência_do_Movimento");
        });

        modelBuilder.Entity<ParcelaMvtoContabil>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.NumeroDaParcela }).HasName("Seqcon_e_pc");
        });

        modelBuilder.Entity<ParcelaNotaFiscal>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaNotaFiscal, e.NumeroDaParcela }).HasName("Seq NF e PC");

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithMany(p => p.ParcelasNotaFiscals).HasConstraintName("TB_Parcelas_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal");
        });

        modelBuilder.Entity<ParcelaOrdemDeServico>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaOrdemDeServico, e.NumeroDaParcela }).HasName("Seq Ordem e PC");

            entity.HasOne(d => d.SequenciaDaOrdemDeServicoNavigation).WithMany(p => p.ParcelasOrdemDeServicos).HasConstraintName("TB_Parcelas_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço");
        });

        modelBuilder.Entity<ParcelaOrcamento>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoOrcamento, e.NumeroDaParcela }).HasName("Seq Orçamento e PC");

            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.DescricaoDaCobranca).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoOrcamentoNavigation).WithMany(p => p.ParcelasOrcamentos).HasConstraintName("TB_Parcelas_Orçamento_FK_Seqüência_do_Orçamento");
        });

        modelBuilder.Entity<ParcelaPedCompraNovo>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.NumeroDaParcela }).HasName("Id e Parcela");
        });

        modelBuilder.Entity<ParcelaPedido>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoPedido, e.NumeroDaParcela }).HasName("Seq Pedido e PC");

            entity.HasOne(d => d.SequenciaDoPedidoNavigation).WithMany(p => p.ParcelasPedidos).HasConstraintName("TB_Parcelas_Pedido_FK_Seqüência_do_Pedido");
        });

        modelBuilder.Entity<Parametro>(entity =>
        {
            entity.Property(e => e.CaminhoAtualizacao).HasDefaultValue("");
            entity.Property(e => e.CaminhoAtualizacao2).HasDefaultValue("");
            entity.Property(e => e.DiretorioDasFotos).HasDefaultValue("");
            entity.Property(e => e.DiretorioDesenhoTec).HasDefaultValue("");
            entity.Property(e => e.DiretorioFotosConjuntos).HasDefaultValue("");
            entity.Property(e => e.DiretorioFotosProdutos).HasDefaultValue("");
            entity.Property(e => e.NomeDoServidor).HasDefaultValue("");
        });

        modelBuilder.Entity<ParametroDaContabilidade>(entity =>
        {
            entity.Property(e => e.TrimestreContabil).HasDefaultValue("");
        });

        modelBuilder.Entity<ParametroDaNfe>(entity =>
        {
            entity.Property(e => e.CertificadoDigital).HasDefaultValue("");
            entity.Property(e => e.CpfTestemunha1).HasDefaultValue("");
            entity.Property(e => e.CpfTestemunha2).HasDefaultValue("");
            entity.Property(e => e.Diretorio1NfeHomologacao).HasDefaultValue("");
            entity.Property(e => e.Diretorio1NfeProducao).HasDefaultValue("");
            entity.Property(e => e.Diretorio1NfseHomologacao).HasDefaultValue("");
            entity.Property(e => e.Diretorio1NfseProducao).HasDefaultValue("");
            entity.Property(e => e.Diretorio2NfeHomologacao).HasDefaultValue("");
            entity.Property(e => e.Diretorio2NfeProducao).HasDefaultValue("");
            entity.Property(e => e.Diretorio2NfseHomologacao).HasDefaultValue("");
            entity.Property(e => e.Diretorio2NfseProducao).HasDefaultValue("");
            entity.Property(e => e.Testemunha1).HasDefaultValue("");
            entity.Property(e => e.Testemunha2).HasDefaultValue("");
        });

        modelBuilder.Entity<Paise>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoPais).HasName("Seqüência do País");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoPedido).HasName("Seqüência do Pedido");

            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.Historico).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Pedido_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaOrdemDeServicoNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Pedido_FK_Seqüência_da_Ordem_de_Serviço");

            entity.HasOne(d => d.SequenciaDaPropriedadeNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Pedido_FK_Seqüência_da_Propriedade");

            entity.HasOne(d => d.SequenciaDaTransportadoraNavigation).WithMany(p => p.PedidoSequenciaDaTransportadoraNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Pedido_FK_Seqüência_da_Transportadora");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.PedidoSequenciaDoGeralNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Pedido_FK_Seqüência_do_Geral");

            entity.HasOne(d => d.SequenciaDoOrcamentoNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Pedido_FK_Seqüência_do_Orçamento");

            entity.HasOne(d => d.SequenciaDoVendedorNavigation).WithMany(p => p.PedidoSequenciaDoVendedorNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Pedido_FK_Seqüência_do_Vendedor");
        });

        modelBuilder.Entity<PedidoDeCompraNovo>(entity =>
        {
            entity.HasKey(e => e.IdDoPedido).HasName("Id do Pedido");

            entity.Property(e => e.AgenciaDoBanco1).HasDefaultValue("");
            entity.Property(e => e.BairroDeEntrega).HasDefaultValue("");
            entity.Property(e => e.CepDeEntrega).HasDefaultValue("");
            entity.Property(e => e.CidadeDeEntrega).HasDefaultValue("");
            entity.Property(e => e.CifFob).HasDefaultValue("");
            entity.Property(e => e.Comprador).HasDefaultValue("");
            entity.Property(e => e.ContaCorrenteDoBanco1).HasDefaultValue("");
            entity.Property(e => e.ContatoDeEntrega).HasDefaultValue("");
            entity.Property(e => e.EnderecoDeEntrega).HasDefaultValue("");
            entity.Property(e => e.FoneDeEntrega).HasDefaultValue("");
            entity.Property(e => e.JustificarOAtraso).HasDefaultValue("");
            entity.Property(e => e.NomeDoBanco1).HasDefaultValue("");
            entity.Property(e => e.NomeDoCorrentistaDoBanco1).HasDefaultValue("");
            entity.Property(e => e.NroDaLicitacao).HasDefaultValue("");
            entity.Property(e => e.NumeroDoEndereco).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
            entity.Property(e => e.Prazo).HasDefaultValue("");
            entity.Property(e => e.UfDeEntrega).HasDefaultValue("");
            entity.Property(e => e.Vend).HasDefaultValue("");
        });

        modelBuilder.Entity<PecaDaNotaFiscal>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaNotaFiscal, e.SequenciaDaPecaNotaFiscal }).HasName("Seq NF e Seq Peças Nota Fiscal");

            entity.Property(e => e.SequenciaDaPecaNotaFiscal).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithMany(p => p.PecasDaNotaFiscals).HasConstraintName("TB_Peças_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.PecasDaNotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Peças_da_Nota_Fiscal_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<PecaDaOrdemDeServico>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaOrdemDeServico, e.SequenciaPecasOs }).HasName("Seq Ordem de Seq Peças");

            entity.Property(e => e.SequenciaPecasOs).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaOrdemDeServicoNavigation).WithMany(p => p.PecasDaOrdemDeServicos).HasConstraintName("TB_Peças_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.PecasDaOrdemDeServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Peças_da_Ordem_de_Serviço_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<PecaDoMovimentoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaDaPecaMovimento }).HasName("Seq Mvto e Seq Peça");

            entity.Property(e => e.SequenciaDaPecaMovimento).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.PecasDoMovimentoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Peças_do_Movimento_Estoque_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<PecaDoOrcamento>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoOrcamento, e.SequenciaPecasDoOrcamento }).HasName("Seq Orçamento e Seq Peças");

            entity.Property(e => e.SequenciaPecasDoOrcamento).ValueGeneratedOnAdd();
            entity.Property(e => e.ValorDoCbs).HasDefaultValue(0.0);
            entity.Property(e => e.ValorDoIbs).HasDefaultValue(0.0);

            entity.HasOne(d => d.SequenciaDoOrcamentoNavigation).WithMany(p => p.PecasDoOrcamentos).HasConstraintName("TB_Peças_do_Orçamento_FK_Seqüência_do_Orçamento");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.PecasDoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Peças_do_Orçamento_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<PecaDoPedido>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoPedido, e.SequenciaDaPecaPedido }).HasName("Seq Pedido e Seq Peça");

            entity.Property(e => e.SequenciaDaPecaPedido).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoPedidoNavigation).WithMany(p => p.PecasDoPedidos).HasConstraintName("TB_Peças_do_Pedido_FK_Seqüência_do_Pedido");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.PecasDoPedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Peças_do_Pedido_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<PecaDoProjeto>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProjeto, e.SequenciaDoItem }).HasName("SeqProjeto_e_Item");

            entity.Property(e => e.ParteDoPivo).HasDefaultValue("");
        });

        modelBuilder.Entity<PivoVendido>(entity =>
        {
            entity.HasKey(e => e.SeqDoPivo).HasName("SeqPivoAux");

            entity.Property(e => e.Cidade).HasDefaultValue("");
            entity.Property(e => e.ModeloDoPivo).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
            entity.Property(e => e.Uf).HasDefaultValue("");
        });

        modelBuilder.Entity<PlanilhaDeAdiantamento>(entity =>
        {
            entity.HasKey(e => e.SeqDoAdiantamento).HasName("Seq do Adiantamento");

            entity.Property(e => e.Nfe).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");

            entity.HasOne(d => d.CodDoVendedorNavigation).WithMany(p => p.PlanilhaDeAdiantamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Planilha_de_Adiantamento_FK_Cod_do_Vendedor");
        });

        modelBuilder.Entity<Pneu>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoPneu).HasName("Sequencia do Pneu");

            entity.Property(e => e.ModeloDoPneu).HasDefaultValue("");
        });

        modelBuilder.Entity<PrevisaoDePagto>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaPrevisao).HasName("Sequencia da Previsao");

            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.NomeDaEmpresa).HasDefaultValue("");
            entity.Property(e => e.RazaoSocial).HasDefaultValue("");
            entity.Property(e => e.TpPagto).HasDefaultValue("");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoProduto).HasName("Seqüência do Produto");

            entity.Property(e => e.CodigoDeBarras).HasDefaultValue("");
            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.Detalhes).HasDefaultValue("");
            entity.Property(e => e.Localizacao).HasDefaultValue("");
            entity.Property(e => e.Medida).HasDefaultValue("");
            entity.Property(e => e.MedidaFinal).HasDefaultValue("");
            entity.Property(e => e.ParteDoPivo).HasDefaultValue("");
            entity.Property(e => e.UsuarioDaAlteracao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaClassificacaoNavigation).WithMany(p => p.Produtos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_FK_Seqüência_da_Classificação");

            entity.HasOne(d => d.SequenciaDaUnidadeNavigation).WithMany(p => p.Produtos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_FK_Seqüência_da_Unidade");

            entity.HasOne(d => d.SequenciaDoGrupoProdutoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.SequenciaDoGrupoProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_FK_Seqüência_do_Grupo_Produto");

            entity.HasOne(d => d.SubGrupoDoProduto).WithMany(p => p.Produtos)
                .HasForeignKey(d => new { d.SequenciaDoSubGrupoProduto, d.SequenciaDoGrupoProduto })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto");
        });

        modelBuilder.Entity<ProdutoDaLicitacao>(entity =>
        {
            entity.HasKey(e => new { e.CodigoDaLicitacao, e.SequenciaDoItem }).HasName("CodItem");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDaLicitacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_da_Licitação_FK_Sequencia_do_Produto");
        });

        modelBuilder.Entity<ProdutoDaNotaFiscal>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaNotaFiscal, e.SequenciaProdutoNotaFiscal }).HasName("Seq NF e Seq Prod Nota Fiscal");

            entity.Property(e => e.SequenciaProdutoNotaFiscal).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithMany(p => p.ProdutosDaNotaFiscals).HasConstraintName("TB_Produtos_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDaNotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_da_Nota_Fiscal_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ProdutoDaOrdemDeServico>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaOrdemDeServico, e.SequenciaProdutoOs }).HasName("Seq Os e Seq Prod");

            entity.Property(e => e.SequenciaProdutoOs).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaOrdemDeServicoNavigation).WithMany(p => p.ProdutosDaOrdemDeServicos).HasConstraintName("TB_Produtos_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDaOrdemDeServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_da_Ordem_de_Serviço_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ProdutoDoMovimentoContabil>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaDoProdutoMovimento }).HasName("Seq Mvto e Seq Produto");

            entity.Property(e => e.SequenciaDoProdutoMovimento).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoMovimentoNavigation).WithMany(p => p.ProdutosDoMovimentoContabils).HasConstraintName("TB_Produtos_do_Movimento_Contábil_FK_Seqüência_do_Movimento");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDoMovimentoContabils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_do_Movimento_Contábil_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ProdutoDoMovimentoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaDoProdutoMovimento }).HasName("Seq Mvto e Seq Prod");

            entity.Property(e => e.SequenciaDoProdutoMovimento).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoMovimentoNavigation).WithMany(p => p.ProdutosDoMovimentoEstoques).HasConstraintName("TB_Produtos_do_Movimento_Estoque_FK_Seqüência_do_Movimento");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDoMovimentoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_do_Movimento_Estoque_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ProdutoDoNovoPedido>(entity =>
        {
            entity.HasKey(e => new { e.CodigoDoPedido, e.SequenciaDoItem }).HasName("CodPed_e_Item");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDoNovoPedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_do_Novo_Pedido_FK_Sequencia_do_Produto");
        });

        modelBuilder.Entity<ProdutoDoOrcamento>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoOrcamento, e.SequenciaDoProdutoOrcamento }).HasName("Seq Orçamento e Seq Prod");

            entity.Property(e => e.SequenciaDoProdutoOrcamento).ValueGeneratedOnAdd();
            entity.Property(e => e.ValorDoCbs).HasDefaultValue(0.0);
            entity.Property(e => e.ValorDoIbs).HasDefaultValue(0.0);

            entity.HasOne(d => d.SequenciaDoOrcamentoNavigation).WithMany(p => p.ProdutosDoOrcamentos).HasConstraintName("TB_Produtos_do_Orçamento_FK_Seqüência_do_Orçamento");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_do_Orçamento_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ProdutoDoPedido>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoPedido, e.SequenciaDoProdutoPedido }).HasName("Seq Pedido e Seq Produto");

            entity.Property(e => e.SequenciaDoProdutoPedido).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoPedidoNavigation).WithMany(p => p.ProdutosDoPedidos).HasConstraintName("TB_Produtos_do_Pedido_FK_Seqüência_do_Pedido");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosDoPedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_do_Pedido_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ProdutoDoPedidoCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdDoPedido, e.IdDoProduto, e.SequenciaDoItem }).HasName("ItesProdutos");

            entity.HasOne(d => d.IdDoProdutoNavigation).WithMany(p => p.ProdutosDoPedidoCompras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_do_Pedido_Compra_FK_Id_do_Produto");
        });

        modelBuilder.Entity<ProdutoMvtoContabilNovo>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoMovimento, e.SequenciaDoProdutoMvtoNovo }).HasName("Seq Mvto e Seq Prod Mvto Novo");

            entity.Property(e => e.SequenciaDoProdutoMvtoNovo).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.ProdutosMvtoContabilNovos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Produtos_Mvto_Contábil_Novo_FK_Seqüência_do_Produto");
        });

        modelBuilder.Entity<ProjetoDeIrrigacao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoProjeto).HasName("Sequencia do Projeto");

            entity.Property(e => e.CanhaoOuAspersor).HasDefaultValue("");
            entity.Property(e => e.CelAvulso).HasDefaultValue("");
            entity.Property(e => e.CidadeAvulsa).HasDefaultValue("");
            entity.Property(e => e.ClienteAvulso).HasDefaultValue("");
            entity.Property(e => e.CpfAvulso).HasDefaultValue("");
            entity.Property(e => e.DescricaoDoEquipamento).HasDefaultValue("");
            entity.Property(e => e.EntregaTecnica).HasDefaultValue("");
            entity.Property(e => e.FabricanteSprayFinal).HasDefaultValue("");
            entity.Property(e => e.FixoOuRebocavel).HasDefaultValue("");
            entity.Property(e => e.Fone1).HasDefaultValue("");
            entity.Property(e => e.FormaDePagamento).HasDefaultValue("");
            entity.Property(e => e.LanceEmBalanco).HasDefaultValue("");
            entity.Property(e => e.LocalDeEntrega).HasDefaultValue("");
            entity.Property(e => e.MarcaBombaAux).HasDefaultValue("");
            entity.Property(e => e.MarcaBombaParalela).HasDefaultValue("");
            entity.Property(e => e.MarcaBombaSimples).HasDefaultValue("");
            entity.Property(e => e.MarcaDoMotor).HasDefaultValue("");
            entity.Property(e => e.MatBombaAux).HasDefaultValue("");
            entity.Property(e => e.ModeloBombaAux).HasDefaultValue("");
            entity.Property(e => e.ModeloBombaParalela).HasDefaultValue("");
            entity.Property(e => e.ModeloBombaSimples).HasDefaultValue("");
            entity.Property(e => e.ModeloMotor).HasDefaultValue("");
            entity.Property(e => e.ModeloPivo).HasDefaultValue("");
            entity.Property(e => e.NivelDeProtecao).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
            entity.Property(e => e.Opcao).HasDefaultValue("");
            entity.Property(e => e.PrazoDeEntregaPrevisto).HasDefaultValue("");
            entity.Property(e => e.Proposta).HasDefaultValue("");
            entity.Property(e => e.PropriedadeAvulsa).HasDefaultValue("");
            entity.Property(e => e.Rebiut).HasDefaultValue("");
            entity.Property(e => e.TamanhoBombaAux).HasDefaultValue("");
            entity.Property(e => e.TamanhoBombaParalela).HasDefaultValue("");
            entity.Property(e => e.TamanhoBombaSimples).HasDefaultValue("");
            entity.Property(e => e.VendedorIntermediario).HasDefaultValue("");
        });

        modelBuilder.Entity<Propriedade>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaPropriedade).HasName("Seqüência da Propriedade");

            entity.Property(e => e.Bairro).HasDefaultValue("");
            entity.Property(e => e.CaixaPostal).HasDefaultValue("");
            entity.Property(e => e.Cep).HasDefaultValue("");
            entity.Property(e => e.Cnpj).HasDefaultValue("");
            entity.Property(e => e.Complemento).HasDefaultValue("");
            entity.Property(e => e.Endereco).HasDefaultValue("");
            entity.Property(e => e.InscricaoEstadual).HasDefaultValue("");
            entity.Property(e => e.NomeDaPropriedade).HasDefaultValue("");
            entity.Property(e => e.NumeroDoEndereco).HasDefaultValue("");
        });

        modelBuilder.Entity<PropriedadeDoGeral>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaPropriedadeGeral).HasName("Seqüência da Propriedade Geral");

            entity.HasIndex(e => new { e.SequenciaDoGeral, e.SequenciaDaPropriedade }, "Seq Geral e Seq Prop")
                .IsUnique()
                .HasFillFactor(90);

            entity.HasOne(d => d.SequenciaDaPropriedadeNavigation).WithMany(p => p.PropriedadesDoGerals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Propriedades_do_Geral_FK_Seqüência_da_Propriedade");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.PropriedadesDoGerals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Propriedades_do_Geral_FK_Seqüência_do_Geral");
        });

        modelBuilder.Entity<PermissoesTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Nome, "IX_PermissoesTemplate_Nome").IsUnique();

            entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.DataCriacao).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<PermissoesTemplateDetalhe>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.TemplateId, e.Tela }, "IX_PermissoesTemplateDetalhe_Template_Tela").IsUnique();

            entity.Property(e => e.Modulo).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Tela).HasMaxLength(100).IsRequired();

            entity.HasOne(d => d.Template)
                .WithMany(p => p.Detalhes)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PermissoesTemplateDetalhe_Template");
        });

        modelBuilder.Entity<PermissoesTela>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.Grupo, e.Tela }, "IX_PermissoesTela_Grupo_Tela").IsUnique();

            entity.Property(e => e.Grupo).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Modulo).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Tela).HasMaxLength(100).IsRequired();
            entity.Property(e => e.NomeTela).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Rota).HasMaxLength(200).IsRequired();
        });

        // ============================================
        // NOVAS TABELAS DO SISTEMA WEB
        // ============================================

        modelBuilder.Entity<GrupoUsuario>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("GrupoUsuario");

            entity.HasIndex(e => e.Nome, "IX_GrupoUsuario_Nome").IsUnique();

            entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.DataCriacao).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<PwGrupo>(entity =>
        {
            entity.HasKey(e => e.PwNome).HasName("PW~Nome");
        });

        modelBuilder.Entity<PwTabela>(entity =>
        {
            entity.HasKey(e => new { e.PwProjeto, e.PwGrupo, e.PwNome }).HasName("Chave tabelas");

            entity.HasOne(d => d.PwGrupoNavigation).WithMany(p => p.PwTabelas).HasConstraintName("TB_PW~Tabelas_FK_PW~Grupo");
        });

        modelBuilder.Entity<PwUsuario>(entity =>
        {
            entity.HasKey(e => new { e.PwNome, e.PwSenha }).HasName("Chave usuario");

            entity.HasOne(d => d.PwGrupoNavigation).WithMany(p => p.PwUsuarios).HasConstraintName("TB_PW~Usuarios_FK_PW~Grupo");

            // FK para o novo GrupoUsuario (sistema web)
            entity.HasOne(d => d.GrupoUsuarioNavigation)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.GrupoUsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PwUsuarios_GrupoUsuario");
        });

        modelBuilder.Entity<RazaoAuxiliar>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoRazao).HasName("Sequencia do Razão");

            entity.Property(e => e.HistoricoDoRazao).HasDefaultValue("");
        });

        modelBuilder.Entity<ReceitaPrimarium>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoOrcamento, e.SequenciaDoItem }).HasName("Seq_e_materia");

            entity.Property(e => e.Localizacao).HasDefaultValue("");
            entity.Property(e => e.Pagto).HasDefaultValue("");
            entity.Property(e => e.Pedidos).HasDefaultValue("");
            entity.Property(e => e.Situacao).HasDefaultValue("");
        });

        modelBuilder.Entity<RegiaoDoVendedore>(entity =>
        {
            entity.HasKey(e => e.SeqDoVendedor).HasName("Seq do Vendedor");

            entity.Property(e => e.Nome).HasDefaultValue("");
        });

        modelBuilder.Entity<RelatorioDeViagem>(entity =>
        {
            entity.HasKey(e => e.SeqDaViagem).HasName("Seq da Viagem");

            entity.Property(e => e.DestinoDaViagem).HasDefaultValue("");
            entity.Property(e => e.MotivoDaViagem).HasDefaultValue("");
            entity.Property(e => e.Referencia).HasDefaultValue("");
            entity.Property(e => e.Teste).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.RelatorioDeViagems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Relatorio_de_Viagem_FK_Sequencia_do_Geral");
        });

        modelBuilder.Entity<Requisicao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaRequisicao).HasName("Seqüência da Requisição");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.Requisicaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Requisição_FK_Seqüência_do_Geral");
        });

        modelBuilder.Entity<ResumoAuxiliar>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoResumo).HasName("Sequencia do resumo");
        });

        modelBuilder.Entity<Revendedore>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaRevenda).HasName("Sequencia da Revenda");

            entity.HasOne(d => d.IdDaContaNavigation).WithMany(p => p.Revendedores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Revendedores_FK_Id_da_Conta");
        });

        modelBuilder.Entity<SaidaDeBalcao>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaSaida).HasName("Sequencia da Saida");

            entity.Property(e => e.Documento).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
            entity.Property(e => e.Teste).HasDefaultValue("");
        });

        modelBuilder.Entity<SerieGerador>(entity =>
        {
            entity.HasKey(e => e.SeqDoGerador).HasName("Seq do Gerador");

            entity.HasIndex(e => e.SerieDoGerador, "Serie do Gerador")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.DescriDoGerador).HasDefaultValue("");
            entity.Property(e => e.MesAno).HasDefaultValue("");
            entity.Property(e => e.Nf).HasDefaultValue("");
            entity.Property(e => e.NroDeSerieDoGer).HasDefaultValue("");
            entity.Property(e => e.NroDoGerador).HasDefaultValue("");
            entity.Property(e => e.NroDoMotor).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
        });

        modelBuilder.Entity<SerieHidroturbo>(entity =>
        {
            entity.HasKey(e => e.SeqDoHidroturbo).HasName("Seq do Hidroturbo");

            entity.HasIndex(e => e.SerieDoHidroturbo, "Serie do Hidroturbo")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.CarretelDe).HasDefaultValue("");
            entity.Property(e => e.EntregaTecnica).HasDefaultValue("");
            entity.Property(e => e.LetraDoHidroturbo).HasDefaultValue("");
            entity.Property(e => e.MesAno).HasDefaultValue("");
            entity.Property(e => e.ModeloDoHidroturbo).HasDefaultValue("");
            entity.Property(e => e.Nf).HasDefaultValue("");
            entity.Property(e => e.NroDeSerieHidroturbo).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
        });

        modelBuilder.Entity<SerieMotoBomba>(entity =>
        {
            entity.HasKey(e => e.SeqMotoBomba).HasName("Seq Moto Bomba");

            entity.HasIndex(e => e.SerieDaMotoBomba, "Serie da Moto Bomba")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.EntregaTecnica).HasDefaultValue("");
            entity.Property(e => e.FuncaoMotoBomba).HasDefaultValue("");
            entity.Property(e => e.MesAno).HasDefaultValue("");
            entity.Property(e => e.ModeloDaBomba).HasDefaultValue("");
            entity.Property(e => e.ModeloDoMotor).HasDefaultValue("");
            entity.Property(e => e.Nf).HasDefaultValue("");
            entity.Property(e => e.NroDeSerieBomba).HasDefaultValue("");
            entity.Property(e => e.NroDeSerieMotoBomba).HasDefaultValue("");
            entity.Property(e => e.NroDeSerieMotor).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
            entity.Property(e => e.TpDeMotor).HasDefaultValue("");
        });

        modelBuilder.Entity<SeriePivo>(entity =>
        {
            entity.HasKey(e => e.SeqDoPivo).HasName("Seq do Pivo");

            entity.HasIndex(e => e.SerieDoPivo, "Serie do Pivo")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.DadosAd).HasDefaultValue("");
            entity.Property(e => e.DescriDoPivo).HasDefaultValue("");
            entity.Property(e => e.EntregaTecnica).HasDefaultValue("");
            entity.Property(e => e.LetraDoPivo).HasDefaultValue("");
            entity.Property(e => e.MesAno).HasDefaultValue("");
            entity.Property(e => e.ModeloDoPivo).HasDefaultValue("");
            entity.Property(e => e.Nf).HasDefaultValue("");
            entity.Property(e => e.NroDeSerieDoPivo).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
        });

        modelBuilder.Entity<SerieRebocador>(entity =>
        {
            entity.HasKey(e => e.SeqDoRebocador).HasName("Seq do Rebocador");

            entity.HasIndex(e => e.SerieDoRebocador, "SerieRebocador")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.MesAno).HasDefaultValue("");
            entity.Property(e => e.ModeloDoRebocador).HasDefaultValue("");
            entity.Property(e => e.Nf).HasDefaultValue("");
            entity.Property(e => e.NroDeSerieRebocador).HasDefaultValue("");
            entity.Property(e => e.Obs).HasDefaultValue("");
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoServico).HasName("Seqüência do Serviço");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<ServicoDaNotaFiscal>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaNotaFiscal, e.SequenciaServicoNotaFiscal }).HasName("Seq NF e Seq Serv Nota Fiscal");

            entity.Property(e => e.SequenciaServicoNotaFiscal).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaNotaFiscalNavigation).WithMany(p => p.ServicosDaNotaFiscals).HasConstraintName("TB_Serviços_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal");

            entity.HasOne(d => d.SequenciaDoServicoNavigation).WithMany(p => p.ServicosDaNotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Serviços_da_Nota_Fiscal_FK_Seqüência_do_Serviço");
        });

        modelBuilder.Entity<ServicoDaOrdem>(entity =>
        {
            entity.HasKey(e => new { e.IdDaOrdem, e.SequenciaDoItem }).HasName("Id_e_servico");
        });

        modelBuilder.Entity<ServicoDaOrdemDeServico>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDaOrdemDeServico, e.SequenciaServicoOs }).HasName("Seq OS e Seq Serv");

            entity.Property(e => e.SequenciaServicoOs).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDaOrdemDeServicoNavigation).WithMany(p => p.ServicosDaOrdemDeServicos).HasConstraintName("TB_Serviços_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço");

            entity.HasOne(d => d.SequenciaDoServicoNavigation).WithMany(p => p.ServicosDaOrdemDeServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Serviços_da_Ordem_de_Serviço_FK_Seqüência_do_Serviço");
        });

        modelBuilder.Entity<ServicoDoOrcamento>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoOrcamento, e.SequenciaDoServicoOrcamento }).HasName("Seq Orçamento e Seq Serv");

            entity.Property(e => e.SequenciaDoServicoOrcamento).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoOrcamentoNavigation).WithMany(p => p.ServicosDoOrcamentos).HasConstraintName("TB_Serviços_do_Orçamento_FK_Seqüência_do_Orçamento");

            entity.HasOne(d => d.SequenciaDoServicoNavigation).WithMany(p => p.ServicosDoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Serviços_do_Orçamento_FK_Seqüência_do_Serviço");
        });

        modelBuilder.Entity<ServicoDoPedido>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoPedido, e.SequenciaDoServicoPedido }).HasName("Seq Pedido e Seq Serv");

            entity.Property(e => e.SequenciaDoServicoPedido).ValueGeneratedOnAdd();

            entity.HasOne(d => d.SequenciaDoPedidoNavigation).WithMany(p => p.ServicosDoPedidos).HasConstraintName("TB_Serviços_do_Pedido_FK_Seqüência_do_Pedido");

            entity.HasOne(d => d.SequenciaDoServicoNavigation).WithMany(p => p.ServicosDoPedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Serviços_do_Pedido_FK_Seqüência_do_Serviço");
        });

        modelBuilder.Entity<ServicoDoProjeto>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoProjeto, e.SequenciaDoItem }).HasName("seq_e_servico");

            entity.Property(e => e.ParteDoPivo).HasDefaultValue("");
        });

        modelBuilder.Entity<Setore>(entity =>
        {
            entity.HasKey(e => e.CodigoDoSetor).HasName("Codigo do setor");

            entity.Property(e => e.NomeDoSetor).HasDefaultValue("");
        });

        modelBuilder.Entity<SimulaEstoque>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaSimulacao).HasName("Sequencia da simulação");
        });

        modelBuilder.Entity<SituacaoDoPedido>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoOrcamento).HasName("Seqüência_orc_situacao");

            entity.Property(e => e.DescMaterial).HasDefaultValue("");
            entity.Property(e => e.ObsAlmoxarifado).HasDefaultValue("");
            entity.Property(e => e.ObsCompras).HasDefaultValue("");
            entity.Property(e => e.ObsFabrica).HasDefaultValue("");
            entity.Property(e => e.ObsVendas).HasDefaultValue("");
            entity.Property(e => e.Status).HasDefaultValue("");
        });

        modelBuilder.Entity<Solicitante>(entity =>
        {
            entity.HasKey(e => new { e.CodigoDoSolicitante, e.SequenciaDoItem, e.CodigoDoSetor }).HasName("Codigo do solicitante");

            entity.Property(e => e.CodigoDoSolicitante).ValueGeneratedOnAdd();
            entity.Property(e => e.NomeDoSolicitante).HasDefaultValue("");
        });

        modelBuilder.Entity<SpyBaixaConta>(entity =>
        {
            entity.HasKey(e => e.SeqDoSpy).HasName("Seq do Spy");

            entity.Property(e => e.QuemPagou).HasDefaultValue("");
            entity.Property(e => e.TpCarteira).HasDefaultValue("");
            entity.Property(e => e.TpConta).HasDefaultValue("");
            entity.Property(e => e.Usuario).HasDefaultValue("");
        });

        modelBuilder.Entity<StatuDoProcesso>(entity =>
        {
            entity.HasKey(e => e.CodigoDoStatus).HasName("Codigo do Status");

            entity.Property(e => e.DescricaoDoStatus).HasDefaultValue("");
        });

        modelBuilder.Entity<SubGrupoDespesa>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaSubGrupoDespesa, e.SequenciaGrupoDespesa }).HasName("Seqüência SubGrupo Despesa");

            entity.Property(e => e.SequenciaSubGrupoDespesa).ValueGeneratedOnAdd();
            entity.Property(e => e.Descricao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaGrupoDespesaNavigation).WithMany(p => p.SubGrupoDespesas).HasConstraintName("TB_SubGrupo_Despesa_FK_Seqüência_Grupo_Despesa");
        });

        modelBuilder.Entity<SubGrupoDoProduto>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoSubGrupoProduto, e.SequenciaDoGrupoProduto }).HasName("Seqüência do SubGrupo Produto");

            entity.Property(e => e.SequenciaDoSubGrupoProduto).ValueGeneratedOnAdd();
            entity.Property(e => e.Descricao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDoGrupoProdutoNavigation).WithMany(p => p.SubGrupoDoProdutos).HasConstraintName("TB_SubGrupo_do_Produto_FK_Seqüência_do_Grupo_Produto");
        });

        modelBuilder.Entity<SySequencial>(entity =>
        {
            entity.HasKey(e => new { e.SysTabela, e.SysCampo, e.SysChave }).HasName("Chave sequencial");

            entity.Property(e => e.SysTabela)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SysCampo)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SysChave)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PwProjeto)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SysEstacao)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SysIdentificacao)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SysValor)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SysValorAnterior)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TipoDeAtividade>(entity =>
        {
            entity.HasKey(e => e.CodigoDaAtividade).HasName("Codigo da Atividade");

            entity.Property(e => e.DescricaoDaAtividade).HasDefaultValue("");
        });

        modelBuilder.Entity<TipoDeCobranca>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaCobranca).HasName("Seqüência da Cobrança");

            entity.Property(e => e.Descricao).HasDefaultValue("");
        });

        modelBuilder.Entity<TipoDeTitulo>(entity =>
        {
            entity.HasKey(e => e.SeqDoTitulo).HasName("Seq do Titulo");

            entity.Property(e => e.Titulo).HasDefaultValue("");
        });

        modelBuilder.Entity<TransferenciaDeReceitum>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaTransferencia).HasName("Seqüência da Transferência");

            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.Localizacao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaUnidadeNavigation).WithMany(p => p.TransferenciaDeReceita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Transferência_de_Receita_FK_Seqüência_da_Unidade");

            entity.HasOne(d => d.SequenciaDoGrupoProdutoNavigation).WithMany(p => p.TransferenciaDeReceita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Transferência_de_Receita_FK_Seqüência_do_Grupo_Produto");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.TransferenciaDeReceita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Transferência_de_Receita_FK_Seqüência_do_Produto");

            entity.HasOne(d => d.SubGrupoDoProduto).WithMany(p => p.TransferenciaDeReceita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Transferência_de_Receita_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto");
        });

        modelBuilder.Entity<Unidade>(entity =>
        {
            entity.HasKey(e => e.SequenciaDaUnidade).HasName("Seqüência da Unidade");

            entity.Property(e => e.Descricao).HasDefaultValue("");
            entity.Property(e => e.SiglaDaUnidade).HasDefaultValue("");
        });

        modelBuilder.Entity<ValorAdicionai>(entity =>
        {
            entity.HasKey(e => new { e.SequenciaDoValores, e.SequenciaDaManutencao }).HasName("Seq Valores e Seq Man");

            entity.Property(e => e.SequenciaDoValores).ValueGeneratedOnAdd();
            entity.Property(e => e.Conta).HasDefaultValue("");
            entity.Property(e => e.Observacao).HasDefaultValue("");

            entity.HasOne(d => d.SequenciaDaManutencaoNavigation).WithMany(p => p.ValoresAdicionais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TB_Valores_Adicionais_FK_Seqüência_da_Manutenção");
        });

        modelBuilder.Entity<Vasilhame>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoVasilahme).HasName("Sequencia do Vasilahme");

            entity.Property(e => e.Mov).HasDefaultValue("");
        });

        modelBuilder.Entity<VeiculoDoMotoristum>(entity =>
        {
            entity.Property(e => e.Automovel).HasDefaultValue("");
            entity.Property(e => e.PlacaDaCarreta).HasDefaultValue("");
            entity.Property(e => e.PlacaDoAutomovel).HasDefaultValue("");
        });

        modelBuilder.Entity<VendedorBloqueio>(entity =>
        {
            entity.HasKey(e => e.CodigoDoVendedor).HasName("Codigo do Vendedor Blok");

            entity.Property(e => e.NomeDoVendedor).HasDefaultValue("");
        });

        modelBuilder.Entity<ViaDeTransporteDi>(entity =>
        {
            entity.Property(e => e.Transporte).HasDefaultValue("");
        });

        modelBuilder.Entity<VinculaPedidoOrcamento>(entity =>
        {
            entity.HasKey(e => e.IdVinculacao).HasName("PK__VinculaP__6717027AB8E1433B");

            entity.HasOne(d => d.IdDoPedidoNavigation).WithMany(p => p.VinculaPedidoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VinculaPedidoOrcamento_PedidoCompra");

            entity.HasOne(d => d.SequenciaDoGeralNavigation).WithMany(p => p.VinculaPedidoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VinculaPedidoOrcamento_Geral");

            entity.HasOne(d => d.SequenciaDoOrcamentoNavigation).WithMany(p => p.VinculaPedidoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VinculaPedidoOrcamento_Orcamento");

            entity.HasOne(d => d.SequenciaDoProdutoNavigation).WithMany(p => p.VinculaPedidoOrcamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VinculaPedidoOrcamento_Produto");
        });

        // Configuração do Emitente
        modelBuilder.Entity<Emitente>(entity =>
        {
            entity.ToTable("Emitentes");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Cnpj)
                .IsUnique()
                .HasDatabaseName("IX_Emitentes_CNPJ");

            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .IsRequired();

            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(60)
                .IsRequired();

            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(60);

            entity.Property(e => e.InscricaoEstadual)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.InscricaoMunicipal)
                .HasMaxLength(20);

            entity.Property(e => e.Cnae)
                .HasMaxLength(10);

            entity.Property(e => e.Endereco)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(e => e.Complemento)
                .HasMaxLength(60);

            entity.Property(e => e.Bairro)
                .HasMaxLength(60)
                .IsRequired();

            entity.Property(e => e.CodigoMunicipio)
                .HasMaxLength(7)
                .IsRequired();

            entity.Property(e => e.Municipio)
                .HasMaxLength(60)
                .IsRequired();

            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsRequired();

            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsRequired();

            entity.Property(e => e.CodigoPais)
                .HasMaxLength(4)
                .HasDefaultValue("1058");

            entity.Property(e => e.Pais)
                .HasMaxLength(60)
                .HasDefaultValue("Brasil");

            entity.Property(e => e.Telefone)
                .HasMaxLength(14);

            entity.Property(e => e.Email)
                .HasMaxLength(255);

            entity.Property(e => e.AmbienteNfe)
                .HasDefaultValue(2); // Homologação

            entity.Property(e => e.SerieNfe)
                .HasDefaultValue(1);

            entity.Property(e => e.ProximoNumeroNfe)
                .HasDefaultValue(1);

            entity.Property(e => e.CaminhoCertificado)
                .HasMaxLength(500);

            entity.Property(e => e.SenhaCertificado)
                .HasMaxLength(500);

            entity.Property(e => e.Ativo)
                .HasDefaultValue(true);

            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("GETDATE()");
        });

        // Configuração da tabela LogsAuditoria
        modelBuilder.Entity<LogAuditoria>(entity =>
        {
            entity.ToTable("LogsAuditoria");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DataHora)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UsuarioNome)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.UsuarioGrupo)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.TipoAcao)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Modulo)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Entidade)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.EntidadeId)
                .HasMaxLength(100);

            entity.Property(e => e.Descricao)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.CamposAlterados)
                .HasMaxLength(1000);

            entity.Property(e => e.EnderecoIP)
                .HasMaxLength(50);

            entity.Property(e => e.UserAgent)
                .HasMaxLength(500);

            entity.Property(e => e.MetodoHttp)
                .HasMaxLength(10);

            entity.Property(e => e.UrlRequisicao)
                .HasMaxLength(500);

            entity.Property(e => e.MensagemErro)
                .HasMaxLength(2000);

            entity.Property(e => e.TenantNome)
                .HasMaxLength(100);

            entity.Property(e => e.SessaoId)
                .HasMaxLength(100);

            entity.Property(e => e.CorrelationId)
                .HasMaxLength(50);

            entity.Property(e => e.Erro)
                .HasDefaultValue(false);

            // Índices para performance
            entity.HasIndex(e => e.DataHora)
                .HasDatabaseName("IX_LogsAuditoria_DataHora");

            entity.HasIndex(e => e.UsuarioCodigo)
                .HasDatabaseName("IX_LogsAuditoria_Usuario");

            entity.HasIndex(e => e.TipoAcao)
                .HasDatabaseName("IX_LogsAuditoria_TipoAcao");

            entity.HasIndex(e => e.Modulo)
                .HasDatabaseName("IX_LogsAuditoria_Modulo");

            entity.HasIndex(e => new { e.Entidade, e.EntidadeId })
                .HasDatabaseName("IX_LogsAuditoria_Entidade");

            entity.HasIndex(e => e.TenantId)
                .HasDatabaseName("IX_LogsAuditoria_Tenant");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
