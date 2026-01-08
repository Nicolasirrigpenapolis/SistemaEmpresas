using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Models;

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

    public virtual DbSet<Pais> Paises { get; set; }

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

    // Módulo de Transporte - Reboque, Despesas e Receitas
    public virtual DbSet<Reboque> Reboques { get; set; }

    public virtual DbSet<DespesaViagem> DespesasViagem { get; set; }

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

    public virtual DbSet<VinculoProdutoFornecedor> VinculoProdutoFornecedors { get; set; }

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
            entity.Property(e => e.Observacao).HasColumnName("Observação").HasDefaultValue("");

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
                .HasConstraintName("TB_Conjuntos_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Despesa");
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

            entity.Property(e => e.SequenciaDaManutencao).HasColumnName("Seqüência da Manutenção");
            entity.Property(e => e.SequenciaDoGeral).HasColumnName("Seqüência do Geral");
            entity.Property(e => e.NumeroDaNotaFiscal).HasColumnName("Número da Nota Fiscal");
            entity.Property(e => e.DataDeEntrada).HasColumnName("Data de Entrada");
            entity.Property(e => e.Historico).HasColumnName("Histórico");
            entity.Property(e => e.FormaDePagamento).HasColumnName("Forma de Pagamento");
            entity.Property(e => e.DataDeVencimento).HasColumnName("Data de Vencimento");
            entity.Property(e => e.ValorTotal).HasColumnName("Valor Total");
            entity.Property(e => e.ValorDaParcela).HasColumnName("Valor da Parcela");
            entity.Property(e => e.ValorPago).HasColumnName("Valor Pago");
            entity.Property(e => e.ValorDoJuros).HasColumnName("Valor do Juros");
            entity.Property(e => e.ValorDoDesconto).HasColumnName("Valor do Desconto");
            entity.Property(e => e.ValorRestante).HasColumnName("Valor Restante");
            entity.Property(e => e.TipoDaConta).HasColumnName("Tipo da Conta");
            entity.Property(e => e.DataDaBaixa).HasColumnName("Data da Baixa");
            entity.Property(e => e.SequenciaDaCobranca).HasColumnName("Seqüência da Cobrança");
            entity.Property(e => e.NumeroDaDuplicata).HasColumnName("Número da Duplicata");
            entity.Property(e => e.SequenciaDaOrigem).HasColumnName("Seqüência da Origem");
            entity.Property(e => e.SequenciaDaBaixa).HasColumnName("Seqüência da Baixa");
            entity.Property(e => e.SequenciaGrupoDespesa).HasColumnName("Seqüência Grupo Despesa");
            entity.Property(e => e.SequenciaSubGrupoDespesa).HasColumnName("Seqüência SubGrupo Despesa");
            entity.Property(e => e.ChequeImpresso).HasColumnName("Cheque Impresso");
            entity.Property(e => e.SequenciaDaNotaFiscal).HasColumnName("Seqüência da Nota Fiscal");
            entity.Property(e => e.SequenciaDoEstoque).HasColumnName("Seqüência do Estoque");
            entity.Property(e => e.SequenciaDoPedido).HasColumnName("Seqüência do Pedido");
            entity.Property(e => e.DuplicataImpressa).HasColumnName("Duplicata Impressa");
            entity.Property(e => e.TpoDeRecebimento).HasColumnName("Tpo de Recebimento");
            entity.Property(e => e.Previsao).HasColumnName("Previsão");
            entity.Property(e => e.SequenciaDaCompra).HasColumnName("Sequencia da Compra");
            entity.Property(e => e.NotasDaCompra).HasColumnName("Notas da Compra");
            entity.Property(e => e.VencimentoOriginal).HasColumnName("Vencimento Original");
            entity.Property(e => e.SequenciaLanCc).HasColumnName("Sequencia Lan CC");
            entity.Property(e => e.VrDaPrevisao).HasColumnName("Vr da Previsão");
            entity.Property(e => e.ImpPrevisao).HasColumnName("Imp Previsao");
            entity.Property(e => e.SequenciaDoMovimento).HasColumnName("Seqüência do Movimento");
            entity.Property(e => e.CodigoDoDebito).HasColumnName("Codigo do Debito");
            entity.Property(e => e.CodigoDoCredito).HasColumnName("Codigo do Credito");

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

        // Módulo de Transporte - Manutenção
        modelBuilder.Entity<ManutencaoPeca>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ManutencaoVeiculo>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.TipoManutencao).HasDefaultValue("");
            entity.Property(e => e.DescricaoServico).HasDefaultValue("");
            entity.Property(e => e.NumeroOS).HasDefaultValue("");
            entity.Property(e => e.NumeroNF).HasDefaultValue("");
            entity.Property(e => e.Observacoes).HasDefaultValue("");
        });

        modelBuilder.Entity<MovimentoContabilNovo>(entity =>
        {
            entity.HasKey(e => e.SequenciaDoMovimento).HasName("Seqüência do Movimento");
            entity.Property(e => e.SequenciaDoMovimento).HasColumnName("Seqüência do Movimento");
            entity.Property(e => e.SequenciaGrupoDespesa).HasColumnName("Seqüência Grupo Despesa");
            entity.Property(e => e.SequenciaSubGrupoDespesa).HasColumnName("Seqüência SubGrupo Despesa");
            entity.Property(e => e.DataDoMovimento).HasColumnName("Data do Movimento");
            entity.Property(e => e.TipoDoMovimento).HasColumnName("Tipo do Movimento");
            entity.Property(e => e.SequenciaDoGeral).HasColumnName("Seqüência do Geral");
            entity.Property(e => e.Observacao).HasColumnName("Observação");
            entity.Property(e => e.Devolucao).HasColumnName("Devolução");
            entity.Property(e => e.EProducaoPropria).HasColumnName("E Produção Propria");
            entity.Property(e => e.FormaDePagamento).HasColumnName("Forma de Pagamento");
            entity.Property(e => e.ValorDoFrete).HasColumnName("Valor do Frete");
            entity.Property(e => e.ValorDoDesconto).HasColumnName("Valor do Desconto");
            entity.Property(e => e.ValorTotalDosProdutos).HasColumnName("Valor Total dos Produtos");
            entity.Property(e => e.ValorTotalIpiDosProdutos).HasColumnName("Valor Total IPI dos Produtos");
            entity.Property(e => e.ValorTotalDoMovimento).HasColumnName("Valor Total do Movimento");
            entity.Property(e => e.DataDaAlteracao).HasColumnName("Data da Alteração");
            entity.Property(e => e.HoraDaAlteracao).HasColumnName("Hora da Alteração");
            entity.Property(e => e.UsuarioDaAlteracao).HasColumnName("Usuário da Alteração");
            entity.Property(e => e.ValorTotalDasDespesas).HasColumnName("Valor Total das Despesas");
            entity.Property(e => e.ValorTotalIpiDasDespesas).HasColumnName("Valor Total IPI das Despesas");
            entity.Property(e => e.SequenciaDoOrcamento).HasColumnName("Seqüência do Orçamento");
        });

        // Configuração da tabela Parâmetros (nome com acento no banco legado)
        modelBuilder.Entity<Parametro>(entity =>
        {
            entity.ToTable("Parâmetros");
        });
    }
}