IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Acoes] (
    [Codigo da Ação] smallint NOT NULL,
    [Descrição da Ação] varchar(120) NOT NULL DEFAULT ''
);
GO

CREATE TABLE [Adutoras] (
    [Sequencia da Adutora] int NOT NULL IDENTITY,
    [Modelo da Adutora] varchar(30) NOT NULL DEFAULT '',
    [DN] decimal(8,2) NOT NULL,
    [DN mm] decimal(8,2) NOT NULL,
    [Coeficiente] decimal(8,2) NOT NULL,
    [Material] varchar(7) NOT NULL DEFAULT '',
    [E mm] decimal(8,2) NOT NULL,
    [DI mm] decimal(8,2) NOT NULL,
    CONSTRAINT [Sequencia da Adutora] PRIMARY KEY ([Sequencia da Adutora])
);
GO

CREATE TABLE [Advogados] (
    [Codigo do Advogado] smallint NOT NULL IDENTITY,
    [Nome do Advogado] varchar(40) NOT NULL DEFAULT '',
    [Celular] varchar(14) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo do Advogado] PRIMARY KEY ([Codigo do Advogado])
);
GO

CREATE TABLE [Agencias] (
    [Seqüência da Agência] smallint NOT NULL IDENTITY,
    [Número do Banco] varchar(3) NOT NULL DEFAULT '',
    [Número da Agência] varchar(6) NOT NULL DEFAULT '',
    [Nome do Banco] varchar(35) NOT NULL DEFAULT '',
    [Nome da Agência] varchar(20) NOT NULL DEFAULT '',
    [Endereco] varchar(100) NOT NULL DEFAULT '',
    [Bairro] varchar(50) NOT NULL DEFAULT '',
    [CEP] varchar(9) NOT NULL DEFAULT '',
    [UF] varchar(3) NOT NULL DEFAULT '',
    [Telefone] varchar(14) NOT NULL DEFAULT '',
    [CNPJ] varchar(18) NOT NULL DEFAULT '',
    [Não Calcular] bit NOT NULL,
    [Ativa] bit NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [Seqüência da Agência] PRIMARY KEY ([Seqüência da Agência])
);
GO

CREATE TABLE [Agendamento de Backup] (
    [Seqüência do Backup] int NOT NULL IDENTITY,
    [Tipo do Backup] varchar(15) NOT NULL DEFAULT '',
    [Hora] datetime NOT NULL,
    [Segunda] bit NOT NULL,
    [Terca] bit NOT NULL,
    [Quarta] bit NOT NULL,
    [Quinta] bit NOT NULL,
    [Sexta] bit NOT NULL,
    [Sabado] bit NOT NULL,
    [Domingo] bit NOT NULL,
    [Dia] smallint NOT NULL,
    [Destino] varchar(255) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência do Backup] PRIMARY KEY ([Seqüência do Backup])
);
GO

CREATE TABLE [Alteracao Baixa Contas] (
    [Seq do Spy] int NOT NULL,
    [Seq da Baixa] int NOT NULL,
    [Usu Alteracao] varchar(20) NOT NULL DEFAULT '',
    [Dt Modificacao] datetime NULL,
    [Manutencao] bigint NOT NULL,
    [Dta Baixa] datetime NULL,
    [Juros] decimal(11,2) NOT NULL,
    [Desconto] decimal(11,2) NOT NULL,
    [Vr Pago] decimal(11,2) NOT NULL,
    [Tp Carteira] varchar(20) NOT NULL DEFAULT '',
    [Bx do Cliente] datetime NULL,
    [Quem Pagou] varchar(20) NOT NULL DEFAULT '',
    [Vr do Cliente] decimal(10,2) NOT NULL,
    [Seq da Agencia] smallint NOT NULL,
    [Seq Acc da Agencia] smallint NOT NULL
);
GO

CREATE TABLE [Aspersor Final] (
    [Sequencia do Aspersor] int NOT NULL IDENTITY,
    [Modelo do aspersor] varchar(40) NOT NULL DEFAULT '',
    [Canhao ou Aspersor] varchar(8) NOT NULL DEFAULT '',
    [Bocal] int NOT NULL,
    [Pressão de Trabalho] decimal(8,2) NOT NULL,
    [Vazao] decimal(8,2) NOT NULL,
    [Alcance] decimal(8,3) NOT NULL,
    [Area Final] decimal(8,3) NOT NULL,
    [Volume de Referencia] decimal(7,3) NOT NULL,
    [Percentual raio molhado] decimal(7,3) NOT NULL,
    [Alcance raio Molhado] decimal(7,3) NOT NULL,
    [Area Considerada] decimal(7,3) NOT NULL,
    CONSTRAINT [Sequencia do Aspersor] PRIMARY KEY ([Sequencia do Aspersor])
);
GO

CREATE TABLE [Baixa Comissão Lote] (
    [Seq da Bx] int NOT NULL IDENTITY,
    [Data da Bx] datetime NOT NULL,
    [Cod do Vendedor] int NOT NULL,
    [FiltroIni] datetime NULL,
    [FiltroFim] datetime NULL,
    [Usu da Baixa] varchar(40) NOT NULL DEFAULT '',
    [Fechado] bit NOT NULL,
    CONSTRAINT [Seq da Bx] PRIMARY KEY ([Seq da Bx])
);
GO

CREATE TABLE [Baixa Comissão Lote Contas] (
    [Id da Baixa] int NOT NULL,
    [Id do Adiantamento] int NOT NULL,
    [NFe] varchar(10) NOT NULL DEFAULT '',
    [Parcela] smallint NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Valor Pago] decimal(11,2) NOT NULL,
    [Vencto] datetime NOT NULL,
    [Data Pagto Cliente] datetime NULL,
    [Percentual] decimal(8,4) NOT NULL,
    [Comissao] decimal(10,2) NOT NULL,
    CONSTRAINT [Id da Baixa] PRIMARY KEY ([Id da Baixa], [Id do Adiantamento])
);
GO

CREATE TABLE [Baixa MP Conjunto] (
    [Seqüência da Baixa] int NOT NULL IDENTITY,
    [Seqüência do Movimento] int NOT NULL,
    [Data da Baixa] datetime NULL,
    [Hora da Baixa] datetime NULL,
    [Seqüência do Item] smallint NOT NULL,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade do Conjunto] decimal(9,3) NOT NULL,
    [Seqüência da Matéria Prima] int NOT NULL,
    [Quantidade da Matéria Prima] decimal(9,3) NOT NULL,
    [Calcular Estoque] bit NOT NULL,
    CONSTRAINT [Seq Baixa MP Conj] PRIMARY KEY ([Seqüência da Baixa])
);
GO

CREATE TABLE [Bocal Aspersor Nelson] (
    [Sequencia do Bocal] int NOT NULL IDENTITY,
    [Modelo Aspersor] varchar(9) NOT NULL DEFAULT '',
    [Bocal do Aspersor] decimal(5,2) NOT NULL,
    [MCA] decimal(7,2) NOT NULL,
    [Vazao] decimal(8,2) NOT NULL,
    [Raio de Alcance metros] decimal(6,2) NOT NULL,
    [Area Total ha] decimal(6,2) NOT NULL,
    [Volume Referencia mm] decimal(6,2) NOT NULL,
    [Percentual alcance Molhado] decimal(6,2) NOT NULL,
    [Alcence Raio Molhado m] decimal(6,2) NOT NULL,
    [Alcence aspersor final ha] decimal(6,2) NOT NULL,
    [Fabricante do Aspersor] varchar(12) NOT NULL DEFAULT '',
    CONSTRAINT [Sequencia do Bocal] PRIMARY KEY ([Sequencia do Bocal])
);
GO

CREATE TABLE [Bx Consumo Pedido Compra] (
    [Id do Pedido] int NOT NULL,
    [Id Despesa] int NOT NULL,
    [Id da Despesa] int NOT NULL,
    [Qtde Total] decimal(10,2) NOT NULL,
    [Vr Unitario] decimal(10,2) NOT NULL,
    [Vr Total do Pedido] decimal(10,2) NOT NULL,
    [Qtde Recebida] decimal(10,2) NOT NULL,
    [Qtde Restante] decimal(10,2) NOT NULL,
    [Total Restante] decimal(10,2) NOT NULL,
    [Notas] varchar(100) NOT NULL DEFAULT '',
    CONSTRAINT [Bx Consumo] PRIMARY KEY ([Id do Pedido], [Id Despesa])
);
GO

CREATE TABLE [Bx Despesas Pedido Compra] (
    [Id do Pedido] int NOT NULL,
    [Id da Despesa] int NOT NULL,
    [Qtde Total] decimal(10,2) NOT NULL,
    [Vr Unitario] decimal(10,2) NOT NULL,
    [Vr Total do Pedido] decimal(10,2) NOT NULL,
    [Qtde Recebida] decimal(10,2) NOT NULL,
    [Qtde Restante] decimal(10,2) NOT NULL,
    [Total Restante] decimal(10,2) NOT NULL,
    [Notas] varchar(100) NOT NULL DEFAULT '',
    CONSTRAINT [Bx Despesa] PRIMARY KEY ([Id do Pedido], [Id da Despesa])
);
GO

CREATE TABLE [Bx Produtos Pedido Compra] (
    [Id do Pedido] int NOT NULL,
    [Id do Produto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Qtde Total] decimal(10,2) NOT NULL,
    [Vr Unitario] decimal(11,4) NOT NULL,
    [Vr Total do Pedido] decimal(10,2) NOT NULL,
    [Qtde Recebida] decimal(10,2) NOT NULL,
    [Qtde Restante] decimal(10,2) NOT NULL,
    [Total Restante] decimal(10,2) NOT NULL,
    [Notas] varchar(100) NOT NULL DEFAULT '',
    [Teste] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [Bx Produto] PRIMARY KEY ([Id do Pedido], [Id do Produto], [Sequencia do Item])
);
GO

CREATE TABLE [Calendario] (
    [Seq do Calendario] int NOT NULL IDENTITY,
    [Dta do Feriado] datetime NOT NULL,
    [Dia da Semana] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [Seq do Calendario] PRIMARY KEY ([Seq do Calendario])
);
GO

CREATE TABLE [Check list maquina] (
    [Seqüência do Produto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Tpproduto] varchar(7) NOT NULL DEFAULT '',
    [Seqüência da Matéria Prima] int NOT NULL,
    [Qtde utilizada] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq_e_itemcheck] PRIMARY KEY ([Seqüência do Produto], [Sequencia do Item])
);
GO

CREATE TABLE [Cheques Cancelados] (
    [Sequencia] int NOT NULL IDENTITY,
    [Data] datetime NOT NULL,
    [Banco] smallint NOT NULL,
    [Nro da Conta] varchar(10) NOT NULL DEFAULT '',
    [Nro do Cheque] varchar(10) NOT NULL DEFAULT '',
    [Motivo do Cancelamento] text NOT NULL DEFAULT '',
    CONSTRAINT [Sequencia do Cheque] PRIMARY KEY ([Sequencia])
);
GO

CREATE TABLE [Classificação Fiscal] (
    [Seqüência da Classificação] smallint NOT NULL IDENTITY,
    [NCM] int NOT NULL,
    [Descrição do NCM] varchar(100) NOT NULL DEFAULT '',
    [Porcentagem do IPI] decimal(8,4) NOT NULL,
    [Anexo da Redução] smallint NOT NULL,
    [Alíquota do Anexo] smallint NOT NULL,
    [Produto Diferido] bit NOT NULL,
    [Redução de Base de Cálculo] bit NOT NULL,
    [Inativo] bit NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Tem Convênio] bit NOT NULL,
    [Cest] varchar(7) NOT NULL DEFAULT '',
    [Un Exterior] varchar(10) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência da Classificação] PRIMARY KEY ([Seqüência da Classificação])
);
GO

CREATE TABLE [Clientes Processos] (
    [Codigo do Cliente] int NOT NULL IDENTITY,
    [Nome do Cliente] varchar(40) NOT NULL DEFAULT '',
    [Envolvido] bit NOT NULL,
    CONSTRAINT [Codigo do Cliente] PRIMARY KEY ([Codigo do Cliente])
);
GO

CREATE TABLE [Cobrar Fornecedor] (
    [Codigo da Cobrança] int NOT NULL IDENTITY,
    [Data da Cobrança] datetime NULL,
    [Codigo do Fornecedor] int NOT NULL,
    [Nova Previsão] datetime NULL,
    [Justificacao] varchar(120) NOT NULL DEFAULT '',
    [Id do Pedido] int NOT NULL,
    [Antiga Previsão] datetime NULL,
    [Usuario da Cobrança] varchar(20) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo da Cobrança] PRIMARY KEY ([Codigo da Cobrança])
);
GO

CREATE TABLE [Comissão do montador] (
    [Sequencia da comissão] int NOT NULL IDENTITY,
    [Cod do Vendedor] int NOT NULL,
    [Manutencao] int NOT NULL,
    [NFe] varchar(10) NOT NULL DEFAULT '',
    [Percentual] decimal(8,4) NOT NULL,
    [Pagto Vendedor] datetime NULL,
    [Comissao] decimal(10,2) NOT NULL,
    [Imprimir] bit NOT NULL,
    CONSTRAINT [Sequencia da comissão] PRIMARY KEY ([Sequencia da comissão])
);
GO

CREATE TABLE [Composição do Equipamento] (
    [Sequencia do Projeto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Modelo do Lance] int NOT NULL,
    [Tipo do Lance] varchar(13) NOT NULL DEFAULT '',
    [Quant de Lance] smallint NOT NULL,
    CONSTRAINT [SeqProjeto_item] PRIMARY KEY ([Sequencia do Projeto], [Sequencia do Item])
);
GO

CREATE TABLE [Concilia Conta Antecipada] (
    [Sequencia da Conciliação] int NOT NULL IDENTITY,
    [Seqüência da Manutenção] int NOT NULL,
    [Sequencia da Compra] int NOT NULL,
    [Data da Conciliação] datetime NULL,
    [Notas da Compra] varchar(100) NOT NULL DEFAULT '',
    [Conciliado] bit NOT NULL,
    CONSTRAINT [Sequencia da Conciliação] PRIMARY KEY ([Sequencia da Conciliação])
);
GO

CREATE TABLE [Conciliação de Cheques] (
    [Seq da Conciliação] int NOT NULL IDENTITY,
    [Dta da Conciliação] datetime NOT NULL,
    [Agencia] smallint NOT NULL,
    [N Cheque] bigint NOT NULL,
    [Dta de Emissão] datetime NOT NULL,
    [Vr do Cheque] decimal(10,2) NOT NULL,
    [Vr Compensado] decimal(10,2) NOT NULL,
    [Conciliado] bit NOT NULL,
    CONSTRAINT [Seq da Conciliação] PRIMARY KEY ([Seq da Conciliação])
);
GO

CREATE TABLE [ConfiguracaoIntegracao] (
    [Id] int NOT NULL IDENTITY,
    [Chave] varchar(100) NOT NULL,
    [Valor] varchar(500) NULL,
    [Descricao] varchar(500) NULL,
    [DataUltimaAlteracao] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK_ConfiguracaoIntegracao] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Conjuntos do Projeto] (
    [Sequencia do Projeto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Sequencia do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Parte do Pivo] varchar(29) NOT NULL DEFAULT '',
    [Valor Anterior] decimal(12,2) NOT NULL,
    CONSTRAINT [Seq_e_Conjunto] PRIMARY KEY ([Sequencia do Projeto], [Sequencia do Item])
);
GO

CREATE TABLE [Consulta Notas Destinada] (
    [Seqüência da Consulta] smallint NOT NULL IDENTITY,
    [Chave de Acesso da NFe] varchar(50) NOT NULL DEFAULT '',
    [CNPJ] varchar(18) NOT NULL DEFAULT '',
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    [Inscrição Estadual] varchar(20) NOT NULL DEFAULT '',
    [Data de Emissão] datetime NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    CONSTRAINT [Seqüência da Consulta] PRIMARY KEY ([Seqüência da Consulta])
);
GO

CREATE TABLE [Conta Contabil] (
    [Codigo Contabil] int NOT NULL IDENTITY,
    [Conta Contabil] varchar(56) NOT NULL DEFAULT '',
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Sequencia do Geral] int NOT NULL,
    CONSTRAINT [Codigo Contabil] PRIMARY KEY ([Codigo Contabil])
);
GO

CREATE TABLE [Conta Corrente da Agência] (
    [Seqüência da Agência] smallint NOT NULL,
    [Seqüência da CC da Agência] smallint NOT NULL IDENTITY,
    [Número da Conta Corrente] varchar(11) NOT NULL DEFAULT '',
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Valor de Saída] decimal(11,2) NOT NULL,
    [Valor de Entrada] decimal(11,2) NOT NULL,
    [Valor Atual] decimal(11,2) NOT NULL,
    [Inativo] bit NOT NULL,
    [BBApiClientId] nvarchar(500) NULL,
    [BBApiClientSecret] nvarchar(500) NULL,
    [BBApiDeveloperKey] nvarchar(500) NULL,
    [HabilitarIntegracaoBB] bit NULL DEFAULT CAST(0 AS bit),
    [DigitoConta] nvarchar(2) NULL,
    CONSTRAINT [Seq Agencia e Seq da CC] PRIMARY KEY ([Seqüência da Agência], [Seqüência da CC da Agência])
);
GO

CREATE TABLE [Conta do Vendedor] (
    [Id da Conta] int NOT NULL IDENTITY,
    [Titular da Conta] varchar(40) NOT NULL DEFAULT '',
    [Desativado] bit NOT NULL,
    [A Liberar] decimal(10,2) NOT NULL,
    [Gerente Regional] varchar(40) NOT NULL DEFAULT '',
    [Faz projeto] bit NOT NULL,
    [Montador] bit NOT NULL,
    [Percentual] decimal(8,4) NOT NULL,
    [Revenda] bit NOT NULL,
    [Seqüência do Geral] int NOT NULL,
    CONSTRAINT [Id da Conta] PRIMARY KEY ([Id da Conta])
);
GO

CREATE TABLE [Controle de Compras] (
    [Id do Pedido] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Data do Pedido] datetime NULL,
    [Comprador] varchar(30) NOT NULL DEFAULT '',
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência do Geral] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Vr_Unit_Ipi] decimal(11,4) NOT NULL,
    [Qtde_Total] decimal(10,2) NOT NULL,
    [Qtde_Recebida] decimal(10,2) NOT NULL,
    [Qtde_Restante] decimal(10,2) NOT NULL,
    [Prazo] varchar(40) NOT NULL DEFAULT '',
    [Financeiro] varchar(30) NOT NULL DEFAULT '',
    [Data da Baixa] datetime NULL,
    [Dias] smallint NOT NULL,
    [Codigo do Fornecedor] int NOT NULL,
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    [Prev entrega] datetime NULL
);
GO

CREATE TABLE [Controle de Garantia] (
    [Sequencia do Controle] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Usuário da Alteração] varchar(60) NOT NULL DEFAULT '',
    [Data da Alteração] datetime NULL,
    [Hora da Alteração] datetime NULL,
    CONSTRAINT [Sequencia do Controle] PRIMARY KEY ([Sequencia do Controle])
);
GO

CREATE TABLE [Controle de Pneus] (
    [Sequencia do Projeto] int NOT NULL,
    [Sequencia do Pneu] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [NFe Saidas] decimal(10,2) NOT NULL,
    [Modelo do Pneu] varchar(30) NOT NULL DEFAULT '',
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    CONSTRAINT [Projeto_Pneu] PRIMARY KEY ([Sequencia do Projeto], [Sequencia do Pneu])
);
GO

CREATE TABLE [Correcao Bloko K] (
    [Sequencia da Correção] int NOT NULL IDENTITY,
    [Data da Correção] datetime NULL,
    CONSTRAINT [Sequencia da Correção] PRIMARY KEY ([Sequencia da Correção])
);
GO

CREATE TABLE [Dados Adicionais] (
    [Seqüência dos Dados Adicionais] int NOT NULL IDENTITY,
    [Dados Adicionais] text NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência dos Dados Adicionais] PRIMARY KEY ([Seqüência dos Dados Adicionais])
);
GO

CREATE TABLE [Despesas e vendas] (
    [Sequencia da simulação] int NOT NULL IDENTITY,
    [Sequencia do Geral] int NOT NULL,
    [Total da Viagem] decimal(10,2) NOT NULL,
    [Valor do orçamento] decimal(12,2) NOT NULL,
    [Saldo] decimal(10,2) NOT NULL,
    [Comissao] decimal(10,2) NOT NULL,
    [Salario] decimal(10,2) NOT NULL,
    [Ref] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [Seq_simula] PRIMARY KEY ([Sequencia da simulação])
);
GO

CREATE TABLE [Divirgencias NFe] (
    [Codigo da Divirgencia] int NOT NULL IDENTITY,
    [Data de Emissão] datetime NULL,
    [Número da NFe] int NOT NULL,
    [CFOP] smallint NOT NULL,
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo da Divirgencia] PRIMARY KEY ([Codigo da Divirgencia])
);
GO

CREATE TABLE [Duplicatas Descontadas] (
    [Seq da Duplicata] int NOT NULL IDENTITY,
    [Duplicata] int NOT NULL,
    [Pc] smallint NOT NULL,
    [Cod do Geral] int NOT NULL,
    [Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Tpo de Carteira] varchar(8) NOT NULL DEFAULT '',
    [Data da Baixa] datetime NOT NULL,
    [Cod do Banco] int NOT NULL,
    [Cc do Banco] int NOT NULL,
    [Obs] text NOT NULL DEFAULT '',
    CONSTRAINT [Seq da Duplicata] PRIMARY KEY ([Seq da Duplicata])
);
GO

CREATE TABLE [Finalidade NFe] (
    [Codigo] smallint NOT NULL IDENTITY,
    [Finalidade] varchar(20) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo] PRIMARY KEY ([Codigo])
);
GO

CREATE TABLE [Follow Up Vendas] (
    [Seq Follow Up] int NOT NULL IDENTITY,
    [Seqüência do Orçamento] int NOT NULL,
    [Data de Emissão] datetime NULL,
    [Seqüência da Transportadora] int NOT NULL,
    [Data de Entrega] datetime NULL,
    [Dias] smallint NOT NULL,
    [Det 1] varchar(100) NOT NULL DEFAULT '',
    [Det 2] text NOT NULL DEFAULT '',
    [Serie do Equipamento] varchar(25) NOT NULL DEFAULT '',
    [Descr do Material] text NULL DEFAULT '',
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    [Stat] varchar(19) NOT NULL DEFAULT '',
    [Venda Fechada] bit NOT NULL,
    [Telefone] varchar(14) NOT NULL DEFAULT '',
    CONSTRAINT [Seq Follow Up] PRIMARY KEY ([Seq Follow Up])
);
GO

CREATE TABLE [Grupo da Despesa] (
    [Seqüência Grupo Despesa] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência Grupo Despesa] PRIMARY KEY ([Seqüência Grupo Despesa])
);
GO

CREATE TABLE [Grupo do Produto] (
    [Seqüência do Grupo Produto] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência do Grupo Produto] PRIMARY KEY ([Seqüência do Grupo Produto])
);
GO

CREATE TABLE [Hidroturbos Vendidos] (
    [Seq do Hidroturbo] int NOT NULL IDENTITY,
    [Modelo do Hidroturbo] varchar(40) NOT NULL DEFAULT '',
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    [Cidade] varchar(40) NOT NULL DEFAULT '',
    [UF] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [Seq_hidro_ven] PRIMARY KEY ([Seq do Hidroturbo])
);
GO

CREATE TABLE [Historico Contabil] (
    [Codigo do Historico] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    CONSTRAINT [Codigo do Historico] PRIMARY KEY ([Codigo do Historico])
);
GO

CREATE TABLE [Histórico da Conta Corrente] (
    [Seqüência do Histórico] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência do Histórico] PRIMARY KEY ([Seqüência do Histórico])
);
GO

CREATE TABLE [ICMS] (
    [Seqüência do ICMS] smallint NOT NULL IDENTITY,
    [UF] varchar(3) NOT NULL DEFAULT '',
    [Regiao] varchar(20) NOT NULL DEFAULT '',
    [Porcentagem de ICMS] decimal(5,2) NOT NULL,
    [Alíquota InterEstadual] decimal(5,2) NOT NULL,
    [Código da UF] smallint NOT NULL,
    CONSTRAINT [Seqüência do ICMS] PRIMARY KEY ([Seqüência do ICMS])
);
GO

CREATE TABLE [Importação] (
    [Última Agência] int NOT NULL,
    [Última Baixa Receber] int NOT NULL,
    [Última Baixa Pagar] int NOT NULL,
    [Última Classificação Fiscal] int NOT NULL,
    [Último Conjunto] int NOT NULL,
    [Último Dados Adicionais] int NOT NULL,
    [Última Entrada Receber] int NOT NULL,
    [Última Entrada Pagar] int NOT NULL,
    [Último Cliente] int NOT NULL,
    [Último Fornecedor] int NOT NULL,
    [Último Vendedor] int NOT NULL,
    [Último Grupo da Despesa] int NOT NULL,
    [Último Grupo do Produto] int NOT NULL,
    [Último Histórico da CC] int NOT NULL,
    [Último ICMS] int NOT NULL,
    [Última Manutenção Pagar] int NOT NULL,
    [Última Manutenção Receber] int NOT NULL,
    [Último Movimento da CC] int NOT NULL,
    [Última Cidade] int NOT NULL,
    [Última Natureza de Operação] int NOT NULL,
    [Último Produto] int NOT NULL,
    [Último Serviço] int NOT NULL,
    [Última Tabela A] int NOT NULL,
    [Última Tabela B] int NOT NULL,
    [Última Cobrança] int NOT NULL,
    [Última Unidade] int NOT NULL,
    [Último Acerto no Estoque] int NOT NULL,
    [Última Entrada no Estoque] int NOT NULL,
    [Última Entrada Receita] int NOT NULL,
    [Último Movimento Estoque] int NOT NULL,
    [Último Movimento Estoque Conj] int NOT NULL,
    [Última Requisição] int NOT NULL,
    [Última Entrada Contábil] int NOT NULL,
    [Última Nota Fiscal] int NOT NULL,
    [Último Orçamento] int NOT NULL,
    [Última Ordem de Serviço] int NOT NULL,
    [Último Pedido] int NOT NULL
);
GO

CREATE TABLE [Importação Conjuntos Estoque] (
    [Seqüência Importação Estoque] int NOT NULL,
    [Seqüência Importação Ítem] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    CONSTRAINT [Seq Importação Estoque Seq Con] PRIMARY KEY ([Seqüência Importação Estoque], [Seqüência Importação Ítem])
);
GO

CREATE TABLE [Importação Estoque] (
    [Seqüência Importação Estoque] int NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência Importação Estoque] PRIMARY KEY ([Seqüência Importação Estoque])
);
GO

CREATE TABLE [Inutilização NFe] (
    [Seqüência da Inutilização] int NOT NULL IDENTITY,
    [Ano] smallint NOT NULL,
    [Justificativa] varchar(255) NOT NULL DEFAULT '',
    [Ambiente] smallint NOT NULL,
    [Faixa Inicial] int NOT NULL,
    [Faixa Final] int NOT NULL,
    [Data da Inutilização] datetime NULL,
    CONSTRAINT [Seqüência da Inutilização] PRIMARY KEY ([Seqüência da Inutilização])
);
GO

CREATE TABLE [Inventario Pdf] (
    [Codigo do Pdf] varchar(10) NOT NULL DEFAULT '',
    [Decricao] varchar(100) NOT NULL DEFAULT '',
    [Quantidade] decimal(11,4) NOT NULL,
    [Unid] varchar(20) NOT NULL DEFAULT '',
    [Valor Contabil Pdf] decimal(11,4) NOT NULL,
    [Valor Total Pdf] decimal(12,2) NOT NULL,
    [Data Base] varchar(100) NULL DEFAULT '',
    [SeqItem] int NOT NULL,
    [Tipo do Produto] smallint NOT NULL,
    CONSTRAINT [Codigo do Pdf] PRIMARY KEY ([Codigo do Pdf])
);
GO

CREATE TABLE [Itens da Correcao] (
    [Sequencia da Correção] int NOT NULL,
    [Sequencia do Produto] int NOT NULL,
    [Data do Estoque] datetime NULL,
    [Quantidade Positiva] decimal(11,4) NOT NULL,
    [Quantidade Negativa] decimal(11,4) NOT NULL,
    CONSTRAINT [SeqCorrecao_Item] PRIMARY KEY ([Sequencia da Correção], [Sequencia do Produto])
);
GO

CREATE TABLE [Itens da Ordem] (
    [Id da Ordem] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Aliquota do IPI] decimal(8,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    CONSTRAINT [Id da Ordem] PRIMARY KEY ([Id da Ordem], [Sequencia do Item])
);
GO

CREATE TABLE [Itens da Produção] (
    [Sequencia da Produção] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Seqüência da Matéria Prima] int NOT NULL,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Data da Produção] datetime NULL,
    [Nao calcula] bit NOT NULL,
    [Ja produziu] bit NOT NULL,
    [Dt final] datetime NULL,
    [Ini serra] datetime NULL,
    [Fim serra] datetime NULL,
    [Hora ini serra] datetime NULL,
    [Hora fim serra] datetime NULL,
    [Data inicial oxicorte] datetime NULL,
    [Hora ini oxi] datetime NULL,
    [Data fim oxicorte] datetime NULL,
    [Hora fim oxi] datetime NULL,
    [Dt ini guilhotina] datetime NULL,
    [Hora ini gui] datetime NULL,
    [Hora fim gui] datetime NULL,
    [Dt fim gui] datetime NULL,
    [Operador serra] varchar(20) NOT NULL DEFAULT '',
    [Operador oxi] varchar(20) NOT NULL DEFAULT '',
    [Operador gui] varchar(20) NOT NULL DEFAULT '',
    [Operador dobra] varchar(20) NOT NULL DEFAULT '',
    [Operador calandra] varchar(20) NOT NULL DEFAULT '',
    [Operador perfiladeira] varchar(20) NOT NULL DEFAULT '',
    [Opeardor torno] varchar(20) NOT NULL DEFAULT '',
    CONSTRAINT [seq_e_item_producao] PRIMARY KEY ([Sequencia da Produção], [Sequencia do Item])
);
GO

CREATE TABLE [Itens da Viagem] (
    [Seq da Viagem] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Descrição do Item] varchar(120) NOT NULL DEFAULT '',
    [Valor do Item] decimal(12,2) NOT NULL,
    CONSTRAINT [Seq_e_Item_Viagem] PRIMARY KEY ([Seq da Viagem], [Sequencia do Item])
);
GO

CREATE TABLE [Itens pendentes] (
    [Seqüência do Orçamento] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Tp] smallint NOT NULL,
    [Situacao] varchar(30) NOT NULL DEFAULT '',
    [Seqüência do Conjunto] int NOT NULL,
    CONSTRAINT [Seq_orcc_e_item] PRIMARY KEY ([Seqüência do Orçamento], [Sequencia do Item])
);
GO

CREATE TABLE [Itens Saidas Balcao] (
    [Sequencia da Saida] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Consignado] bit NOT NULL,
    [Seq principal] int NOT NULL,
    CONSTRAINT [Seq_B_Item] PRIMARY KEY ([Sequencia da Saida], [Sequencia do Item])
);
GO

CREATE TABLE [IVA From UFs] (
    [ID MVA] int NOT NULL,
    [UF] varchar(3) NOT NULL DEFAULT '',
    [NCM] int NOT NULL,
    [IVA] decimal(8,4) NOT NULL,
    [Teste] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [ID MVA] PRIMARY KEY ([ID MVA], [UF], [NCM])
);
GO

CREATE TABLE [LancamentoBancarioBB] (
    [SequenciaLancamentoBB] int NOT NULL IDENTITY,
    [SequenciaDaAgencia] int NULL,
    [SequenciaDaCCDaAgencia] int NULL,
    [DataLancamento] datetime NOT NULL,
    [DataMovimento] datetime NULL,
    [Valor] money NOT NULL,
    [TipoLancamento] char(1) NOT NULL,
    [CodigoHistorico] int NOT NULL,
    [TextoDescricaoHistorico] varchar(500) NOT NULL,
    [NumeroDocumento] varchar(50) NULL,
    [CpfCnpj] varchar(20) NULL,
    [NomeDevedor] varchar(200) NULL,
    [IndicadorCheque] char(1) NULL,
    [NumeroCheque] varchar(20) NULL,
    [Processado] bit NOT NULL,
    [DataProcessamento] datetime NULL,
    [SequenciaDaBaixaGerada] int NULL,
    [SequenciaManutencaoVinculada] int NULL,
    [MotivoNaoProcessado] varchar(500) NULL,
    [PedidoIdentificado] varchar(50) NULL,
    [DataImportacao] datetime NOT NULL DEFAULT ((getdate())),
    [DadosOriginaisJson] text NULL,
    CONSTRAINT [PK_LancamentoBancarioBB] PRIMARY KEY ([SequenciaLancamentoBB])
);
GO

CREATE TABLE [Lançamentos Contabil] (
    [Id do Lançamento] int NOT NULL IDENTITY,
    [Dt do Lançamento] varchar(5) NULL DEFAULT '',
    [Conta Debito] int NOT NULL,
    [Conta Credito] int NOT NULL,
    [Valor] decimal(12,2) NOT NULL,
    [Codigo do Historico] smallint NOT NULL,
    [Complemento do Hist] text NOT NULL DEFAULT '',
    [Seqüência da Baixa] int NOT NULL,
    [Seqüência da Movimentação CC] int NOT NULL,
    [Data da Baixa] datetime NULL,
    [Gerado] bit NOT NULL,
    CONSTRAINT [Id do Lançamento] PRIMARY KEY ([Id do Lançamento])
);
GO

CREATE TABLE [Lances do Pivo] (
    [Modelo do Lance] int NOT NULL,
    [Descrição do Lance] varchar(120) NOT NULL DEFAULT '',
    [Largura do Lance] decimal(8,2) NOT NULL,
    [Diametro do Lance] decimal(8,2) NOT NULL,
    [Qtde de Spray] smallint NOT NULL,
    [Inicial] bit NOT NULL,
    [Inter] bit NOT NULL,
    [Penultimo] bit NOT NULL,
    [Final] bit NOT NULL,
    [CA1] smallint NOT NULL,
    [CA2] smallint NOT NULL,
    [CA3] smallint NOT NULL,
    [CA4] smallint NOT NULL,
    [CA5] smallint NOT NULL,
    [CA6] smallint NOT NULL,
    [CA7] smallint NOT NULL,
    [CA8] smallint NOT NULL,
    CONSTRAINT [Modelo do Lance] PRIMARY KEY ([Modelo do Lance], [Descrição do Lance])
);
GO

CREATE TABLE [Licitacao] (
    [Sequencia da Licitacao] int NOT NULL IDENTITY,
    [Data da Licitacao] datetime NOT NULL,
    [For 1] varchar(40) NOT NULL DEFAULT '',
    [Contato 1] varchar(25) NOT NULL DEFAULT '',
    [Fone 1] varchar(14) NOT NULL DEFAULT '',
    [Prev Entrega 1] varchar(10) NOT NULL DEFAULT '',
    [Cond Pagto 1] varchar(20) NOT NULL DEFAULT '',
    [For 2] varchar(40) NOT NULL DEFAULT '',
    [Contato 2] varchar(25) NOT NULL DEFAULT '',
    [Fone 2] varchar(14) NOT NULL DEFAULT '',
    [Prev Entrega 2] varchar(10) NOT NULL DEFAULT '',
    [Cond Pagto 2] varchar(20) NOT NULL DEFAULT '',
    [For 3] varchar(40) NOT NULL DEFAULT '',
    [Contato 3] varchar(25) NOT NULL DEFAULT '',
    [Fone 3] varchar(14) NOT NULL DEFAULT '',
    [Prev Entrega 3] varchar(10) NOT NULL DEFAULT '',
    [Cond Pagto 3] varchar(20) NOT NULL DEFAULT '',
    [Fechado] bit NOT NULL,
    CONSTRAINT [Sequencia da Licitacao] PRIMARY KEY ([Sequencia da Licitacao])
);
GO

CREATE TABLE [LogProcessamentoIntegracao] (
    [Id] int NOT NULL IDENTITY,
    [DataHora] datetime NOT NULL DEFAULT ((getdate())),
    [Nivel] varchar(10) NOT NULL,
    [Categoria] varchar(50) NOT NULL,
    [Mensagem] varchar(1000) NOT NULL,
    [Detalhes] text NULL,
    [SequenciaLancamentoBB] int NULL,
    [SequenciaDaBaixa] int NULL,
    [SequenciaDaManutencao] int NULL,
    [StackTrace] text NULL,
    CONSTRAINT [PK_LogProcessamentoIntegracao] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Manutenção Hidroturbo] (
    [Seq do Hidroturbo] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Data da Manutenção] datetime NULL,
    [Descrição da Manutenção] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [SeqH_e_Item] PRIMARY KEY ([Seq do Hidroturbo], [Sequencia do Item])
);
GO

CREATE TABLE [Manutenção Pivo] (
    [Seq do Pivo] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Data da Manutenção] datetime NULL,
    [Descrição da Manutenção] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [SeqPivo_e_Item] PRIMARY KEY ([Seq do Pivo], [Sequencia do Item])
);
GO

CREATE TABLE [Mapa da Vazao] (
    [Sequencia do Projeto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Distancia ao Centro] decimal(8,2) NOT NULL,
    [Vazao na Saida] decimal(8,4) NOT NULL,
    [Vazao Acumulada] decimal(8,4) NOT NULL,
    [Vazao Trecho] decimal(8,3) NOT NULL,
    [DN] decimal(8,2) NOT NULL,
    [Perda Carga] decimal(8,4) NOT NULL,
    [Velocidade Trecho] decimal(8,3) NOT NULL,
    CONSTRAINT [Projeto_Vazao] PRIMARY KEY ([Sequencia do Projeto], [Sequencia do Item])
);
GO

CREATE TABLE [Materia prima orçamento] (
    [Sequencia da Expedição] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Sigla da Unidade] varchar(15) NOT NULL DEFAULT '',
    [Localizacao] varchar(50) NOT NULL DEFAULT '',
    [Quantidade] decimal(11,4) NOT NULL,
    [Peso] decimal(12,3) NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Imprimir] bit NOT NULL,
    CONSTRAINT [Seqexpedicao] PRIMARY KEY ([Sequencia da Expedição])
);
GO

CREATE TABLE [Material Expedição] (
    [Sequencia da Expedição] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Sigla da Unidade] varchar(15) NOT NULL DEFAULT '',
    [Localizacao] varchar(50) NOT NULL DEFAULT '',
    [Peso] decimal(12,3) NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    CONSTRAINT [Sequencia da Expedição] PRIMARY KEY ([Sequencia da Expedição])
);
GO

CREATE TABLE [Motoristas] (
    [Codigo do Motorista] smallint NOT NULL IDENTITY,
    [Nome do Motorista] varchar(30) NOT NULL DEFAULT '',
    [RG] varchar(20) NOT NULL DEFAULT '',
    [CPF] varchar(14) NOT NULL DEFAULT '',
    [Endereco] varchar(100) NOT NULL DEFAULT '',
    [Numero] varchar(9) NOT NULL DEFAULT '',
    [Bairro] varchar(50) NOT NULL DEFAULT '',
    [Municipio] int NOT NULL,
    [UF] varchar(3) NOT NULL DEFAULT '',
    [CEP] varchar(9) NOT NULL DEFAULT '',
    [Fone] varchar(13) NOT NULL DEFAULT '',
    [CEL] varchar(14) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo do Motorista] PRIMARY KEY ([Codigo do Motorista])
);
GO

CREATE TABLE [Movimento Contábil Novo] (
    [Seqüência do Movimento] int NOT NULL IDENTITY,
    [Data do Movimento] datetime NULL,
    [Tipo do Movimento] smallint NOT NULL,
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Seqüência do Geral] int NOT NULL,
    [Observacao] text NOT NULL DEFAULT '',
    [Devolucao] bit NOT NULL,
    [Seq Prod Propria] int NOT NULL,
    [E Produção Propria] bit NOT NULL,
    [Baixa Consumo] bit NOT NULL,
    [Seq Baixa Consumo] int NOT NULL,
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Valor Total dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total do Movimento] decimal(11,2) NOT NULL,
    [Data da Alteração] datetime NULL,
    [Hora da Alteração] datetime NULL,
    [Usuário da Alteração] varchar(60) NULL DEFAULT '',
    [Valor Total das Despesas] decimal(11,2) NOT NULL,
    [Valor Total IPI das Despesas] decimal(11,2) NOT NULL,
    [Titulo] varchar(25) NOT NULL DEFAULT '',
    [Fechado] bit NOT NULL,
    [Sequencia da Compra] int NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Codigo do Debito] int NOT NULL,
    CONSTRAINT [Seq Mvto Contabil Novo] PRIMARY KEY ([Seqüência do Movimento])
);
GO

CREATE TABLE [Municipios] (
    [Seqüência do Município] int NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [UF] varchar(3) NOT NULL DEFAULT '',
    [Código do IBGE] int NOT NULL,
    [CEP] varchar(9) NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência do Município] PRIMARY KEY ([Seqüência do Município])
);
GO

CREATE TABLE [Municipios dos Revendedores] (
    [Sequencia da Revenda] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Id da Conta] int NOT NULL,
    [Reg] varchar(11) NOT NULL DEFAULT '',
    [UF] varchar(3) NOT NULL DEFAULT '',
    [Seqüência do Município] int NOT NULL,
    CONSTRAINT [Seq_e_Revendedor] PRIMARY KEY ([Sequencia da Revenda], [Sequencia do Item], [Id da Conta])
);
GO

CREATE TABLE [MVA] (
    [ID MVA] int NOT NULL,
    [UF] varchar(3) NOT NULL DEFAULT '',
    [IVA] decimal(8,4) NOT NULL,
    CONSTRAINT [ID_UF] PRIMARY KEY ([ID MVA], [UF])
);
GO

CREATE TABLE [Mvto Conta do Vendedor] (
    [Id do Movimento] int NOT NULL IDENTITY,
    [Dta do Movimento] datetime NOT NULL,
    [Id Conta] int NOT NULL,
    [Valor Entrada] decimal(10,2) NOT NULL,
    [Valor Saida] decimal(10,2) NOT NULL,
    [Historico] text NOT NULL DEFAULT '',
    [Informativo] bit NOT NULL,
    CONSTRAINT [Id do Movimento] PRIMARY KEY ([Id do Movimento])
);
GO

CREATE TABLE [Natureza de Operação] (
    [Seqüência da Natureza] smallint NOT NULL IDENTITY,
    [Código da Natureza de Operação] int NOT NULL,
    [Descrição da Natureza Operação] varchar(30) NOT NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência da Natureza] PRIMARY KEY ([Seqüência da Natureza])
);
GO

CREATE TABLE [Ocorrencias Garantia] (
    [Sequencia do Controle] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Data da Ocorrencia] datetime NULL,
    [Data Saida] datetime NULL,
    [Número da NFe] int NOT NULL,
    [Data do Retorno] datetime NULL,
    [Data de Validade] datetime NULL,
    [Ocorrencia] varchar(150) NOT NULL DEFAULT '',
    [Ult Fornecedor] int NOT NULL,
    [Id do Pedido] int NOT NULL,
    [Notas da Compra] varchar(100) NOT NULL DEFAULT '',
    CONSTRAINT [Seq_Prod_Controle] PRIMARY KEY ([Sequencia do Controle], [Sequencia do Item])
);
GO

CREATE TABLE [Orçamentos da compra] (
    [Id do Pedido] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    CONSTRAINT [Id_orc] PRIMARY KEY ([Id do Pedido], [Sequencia do Item])
);
GO

CREATE TABLE [Ordem de Montagem] (
    [Sequencia da Montagem] int NOT NULL IDENTITY,
    [Origem] varchar(10) NOT NULL DEFAULT '',
    [Sequencia da Origem] int NOT NULL,
    [Codigo do Geral] int NOT NULL,
    [Data de Emissão] datetime NULL,
    [Seqüência da Propriedade] smallint NOT NULL,
    [Sequencia da Origem 2] int NOT NULL,
    [Obs] text NOT NULL DEFAULT '',
    [Valor Total dos Produtos] decimal(11,2) NOT NULL,
    [Total de Ipi] decimal(10,2) NOT NULL,
    [Valor Total dos Serviços] decimal(11,2) NOT NULL,
    [Total da Ordem] decimal(10,2) NOT NULL,
    [Km Ini] decimal(8,2) NOT NULL,
    [Km Final] decimal(8,2) NOT NULL,
    [Total Km] decimal(9,2) NOT NULL,
    [Vr Km] decimal(8,2) NOT NULL,
    [Vr Total Km] decimal(10,2) NOT NULL,
    CONSTRAINT [Sequencia da Montagem] PRIMARY KEY ([Sequencia da Montagem])
);
GO

CREATE TABLE [Paises] (
    [Seqüência do País] int NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Código do País] smallint NOT NULL,
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência do País] PRIMARY KEY ([Seqüência do País])
);
GO

CREATE TABLE [Parametros] (
    [Caminho Atualização] varchar(255) NOT NULL DEFAULT '',
    [Caminho Atualização 2] varchar(255) NOT NULL DEFAULT '',
    [Nome do Servidor] varchar(30) NOT NULL DEFAULT '',
    [Diretorio das Fotos] varchar(255) NOT NULL DEFAULT '',
    [Diretorio Fotos Conjuntos] varchar(255) NOT NULL DEFAULT '',
    [Diretorio Fotos Produtos] varchar(255) NOT NULL DEFAULT '',
    [Diretorio Desenho Tec] varchar(255) NOT NULL DEFAULT ''
);
GO

CREATE TABLE [Parâmetros da Contabilidade] (
    [Ano Contábil] smallint NOT NULL,
    [Trimestre Contábil] varchar(8) NOT NULL DEFAULT ''
);
GO

CREATE TABLE [Parâmetros da NFe] (
    [Ambiente] smallint NOT NULL,
    [Diretório 1 NFe Homologação] varchar(255) NOT NULL DEFAULT '',
    [Diretório 2 NFe Homologação] varchar(255) NOT NULL DEFAULT '',
    [Diretório 1 NFe Produção] varchar(255) NOT NULL DEFAULT '',
    [Diretório 2 NFe Produção] varchar(255) NOT NULL DEFAULT '',
    [Diretório 1 NFSe Homologação] varchar(255) NOT NULL DEFAULT '',
    [Diretório 2 NFSe Homologação] varchar(255) NOT NULL DEFAULT '',
    [Diretório 1 NFSe Produção] varchar(255) NOT NULL DEFAULT '',
    [Diretório 2 NFSe Produção] varchar(255) NOT NULL DEFAULT '',
    [Certificado Digital] varchar(255) NOT NULL DEFAULT '',
    [Testemunha 1] varchar(255) NOT NULL DEFAULT '',
    [Testemunha 2] varchar(255) NOT NULL DEFAULT '',
    [CPF Testemunha 1] varchar(14) NOT NULL DEFAULT '',
    [CPF Testemunha 2] varchar(14) NOT NULL DEFAULT '',
    [Horario de Verao] bit NOT NULL
);
GO

CREATE TABLE [Parâmetros do Produto] (
    [Percentual Acréscimo Produto] decimal(5,2) NOT NULL,
    [Percentual Acréscimo Conjunto] decimal(5,2) NOT NULL,
    [Acrescimo do Parcelamento] decimal(8,4) NOT NULL,
    [Percentual 2] decimal(6,2) NOT NULL,
    [Ja atualizou] bit NOT NULL
);
GO

CREATE TABLE [Parametros do SPED ECF] (
    [Versao sped] varchar(3) NOT NULL DEFAULT '',
    [Nome do Contabilista] varchar(40) NOT NULL DEFAULT '',
    [CPF Contabilista] varchar(14) NOT NULL DEFAULT '',
    [CRC] varchar(20) NOT NULL DEFAULT '',
    [CEP] varchar(9) NOT NULL DEFAULT '',
    [CNPJ] varchar(18) NOT NULL DEFAULT '',
    [Endereco] varchar(100) NOT NULL DEFAULT '',
    [Complemento do Endereço] varchar(15) NOT NULL DEFAULT '',
    [Bairro] varchar(50) NOT NULL DEFAULT '',
    [Fone] varchar(13) NOT NULL DEFAULT '',
    [Empresa] varchar(40) NOT NULL DEFAULT ''
);
GO

CREATE TABLE [Parcelas da Ordem] (
    [Sequencia da Montagem] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq1_e_pc1] PRIMARY KEY ([Sequencia da Montagem], [Número da Parcela])
);
GO

CREATE TABLE [Parcelas da Viagem] (
    [Seq da Viagem] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq_e_Pc] PRIMARY KEY ([Seq da Viagem], [Número da Parcela])
);
GO

CREATE TABLE [Parcelas do Novo Pedido] (
    [Codigo do Pedido] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    CONSTRAINT [CodPedido_e_Pc] PRIMARY KEY ([Codigo do Pedido], [Número da Parcela])
);
GO

CREATE TABLE [Parcelas do Projeto] (
    [Sequencia do Projeto] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [SeqProjeto_e_Parcela] PRIMARY KEY ([Sequencia do Projeto], [Número da Parcela])
);
GO

CREATE TABLE [Parcelas mvto contabil] (
    [Seqüência do Movimento] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Seqüência da Cobrança] smallint NOT NULL,
    CONSTRAINT [Seqcon_e_pc] PRIMARY KEY ([Seqüência do Movimento], [Número da Parcela])
);
GO

CREATE TABLE [Parcelas Ped Compra Novo] (
    [Id do Pedido] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Nota] int NOT NULL,
    [Seqüência da Cobrança] smallint NOT NULL,
    CONSTRAINT [Id e Parcela] PRIMARY KEY ([Id do Pedido], [Número da Parcela])
);
GO

CREATE TABLE [Peças do Projeto] (
    [Sequencia do Projeto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Sequencia do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Parte do Pivo] varchar(29) NOT NULL DEFAULT '',
    [Valor Anterior] decimal(12,2) NOT NULL,
    CONSTRAINT [SeqProjeto_e_Item] PRIMARY KEY ([Sequencia do Projeto], [Sequencia do Item])
);
GO

CREATE TABLE [Pedido de Compra Novo] (
    [Id do Pedido] int NOT NULL IDENTITY,
    [Data do Pedido] datetime NOT NULL,
    [Nro da Licitação] varchar(9) NOT NULL DEFAULT '',
    [Codigo do Fornecedor] int NOT NULL,
    [Codigo da Transportadora] int NOT NULL,
    [Comprador] varchar(30) NOT NULL DEFAULT '',
    [Vend] varchar(30) NOT NULL DEFAULT '',
    [Prazo] varchar(40) NOT NULL DEFAULT '',
    [CifFob] varchar(3) NOT NULL DEFAULT '',
    [Vr do Frete] decimal(10,2) NOT NULL,
    [Vr do Desconto] decimal(10,2) NOT NULL,
    [Obs] text NOT NULL DEFAULT '',
    [Total dos Produtos] decimal(10,2) NOT NULL,
    [Total das Despesas] decimal(10,2) NOT NULL,
    [Total do IPI] decimal(10,2) NOT NULL,
    [Total do ICMS] decimal(10,2) NOT NULL,
    [Total do Pedido] decimal(10,2) NOT NULL,
    [Endereco de Entrega] varchar(50) NOT NULL DEFAULT '',
    [Numero do Endereco] varchar(9) NOT NULL DEFAULT '',
    [Bairro de Entrega] varchar(50) NOT NULL DEFAULT '',
    [Cidade de Entrega] varchar(50) NOT NULL DEFAULT '',
    [UF De Entrega] varchar(3) NOT NULL DEFAULT '',
    [CEP de Entrega] varchar(9) NOT NULL DEFAULT '',
    [Fone de Entrega] varchar(20) NOT NULL DEFAULT '',
    [Contato de Entrega] varchar(45) NOT NULL DEFAULT '',
    [Previsao de Entrega] datetime NULL,
    [Pedido Fechado] bit NOT NULL,
    [Validado] bit NOT NULL,
    [Cancelado] bit NOT NULL,
    [Codigo da Licitação] int NOT NULL,
    [Nome do Banco 1] varchar(20) NOT NULL DEFAULT '',
    [Agência do Banco 1] varchar(20) NOT NULL DEFAULT '',
    [Conta Corrente do Banco 1] varchar(15) NOT NULL DEFAULT '',
    [Nome do Correntista do Banco 1] varchar(60) NOT NULL DEFAULT '',
    [Prepedido] bit NOT NULL,
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Justificar o Atraso] varchar(150) NOT NULL DEFAULT '',
    [Nova Previsao] datetime NULL,
    [Dias] smallint NOT NULL,
    CONSTRAINT [Id do Pedido] PRIMARY KEY ([Id do Pedido])
);
GO

CREATE TABLE [Pivos Vendidos] (
    [Seq do Pivo] int NOT NULL IDENTITY,
    [Modelo do Pivo] varchar(6) NOT NULL DEFAULT '',
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    [Cidade] varchar(40) NOT NULL DEFAULT '',
    [UF] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [SeqPivoAux] PRIMARY KEY ([Seq do Pivo])
);
GO

CREATE TABLE [Pneus] (
    [Sequencia do Pneu] int NOT NULL IDENTITY,
    [Modelo do Pneu] varchar(30) NOT NULL DEFAULT '',
    [Velocidade] decimal(6,2) NOT NULL,
    CONSTRAINT [Sequencia do Pneu] PRIMARY KEY ([Sequencia do Pneu])
);
GO

CREATE TABLE [Previsoes de Pagtos] (
    [Sequencia da Previsao] int NOT NULL IDENTITY,
    [Saldo IPG] decimal(10,2) NOT NULL,
    [Saldo Chinellato] decimal(10,2) NOT NULL,
    [Data de Vencimento] datetime NULL,
    [Data de Entrada] datetime NULL,
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Parcela] smallint NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Valor Previsto] decimal(12,2) NOT NULL,
    [Imprimir] bit NOT NULL,
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    [Nome da Empresa] varchar(30) NOT NULL DEFAULT '',
    [Valor Restante] decimal(11,2) NOT NULL,
    [Tp Pagto] varchar(20) NOT NULL DEFAULT '',
    CONSTRAINT [Sequencia da Previsao] PRIMARY KEY ([Sequencia da Previsao])
);
GO

CREATE TABLE [Projeto de Irrigação] (
    [Sequencia do Projeto] int NOT NULL IDENTITY,
    [Sequencia do Geral] int NOT NULL,
    [Proposta] varchar(40) NOT NULL DEFAULT '',
    [Opcao] varchar(10) NOT NULL DEFAULT '',
    [Data da Proposta] datetime NULL,
    [Sequencia da Propriedade] int NOT NULL,
    [Descrição do Equipamento] varchar(120) NOT NULL DEFAULT '',
    [Lance em Balanço] varchar(2) NOT NULL DEFAULT '',
    [Extensão Ult Spray] decimal(8,2) NOT NULL,
    [Alcance Spray Fim] decimal(8,2) NOT NULL,
    [N Posicoes] decimal(8,2) NOT NULL,
    [Graus] smallint NOT NULL,
    [Lamina Bruta] decimal(8,2) NOT NULL,
    [Tempo Max Opera] decimal(8,2) NOT NULL,
    [Modelo Trecho A] int NOT NULL,
    [Modelo Trecho B] int NOT NULL,
    [Modelo Trecho C] int NOT NULL,
    [Modelo Trecho D] int NOT NULL,
    [Com 1] decimal(8,2) NOT NULL,
    [Com 2] decimal(8,2) NOT NULL,
    [Com 3] decimal(8,2) NOT NULL,
    [Com 4] decimal(8,2) NOT NULL,
    [Sequencia do Autotrafo] int NOT NULL,
    [Saidas Acumuladas] decimal(8,2) NOT NULL,
    [Espaço medio Saidas] decimal(8,3) NOT NULL,
    [Pressao no Extremo] decimal(8,2) NOT NULL,
    [Desnivel Ponto Alto] decimal(8,2) NOT NULL,
    [Altura dos Aspersores] decimal(8,2) NOT NULL,
    [Desnivel Moto Bomba] decimal(8,2) NOT NULL,
    [Altura de succao] decimal(8,2) NOT NULL,
    [Desnivel mais Baixo] decimal(8,2) NOT NULL,
    [Perda Mangueira] decimal(8,2) NOT NULL,
    [Cliente Avulso] varchar(60) NOT NULL DEFAULT '',
    [Propriedade Avulsa] varchar(100) NOT NULL DEFAULT '',
    [Cidade Avulsa] varchar(40) NOT NULL DEFAULT '',
    [Desnivel Ponto Baixo] decimal(8,2) NOT NULL,
    [Qtde Bomba Simples] smallint NOT NULL,
    [Qtde Bomba Paralela] smallint NOT NULL,
    [Marca Bomba Simples] varchar(40) NOT NULL DEFAULT '',
    [Marca Bomba Paralela] varchar(40) NOT NULL DEFAULT '',
    [Modelo Bomba Simples] varchar(40) NOT NULL DEFAULT '',
    [Modelo Bomba Paralela] varchar(40) NOT NULL DEFAULT '',
    [Tamanho Bomba Simples] varchar(20) NOT NULL DEFAULT '',
    [Tamanho Bomba Paralela] varchar(20) NOT NULL DEFAULT '',
    [N Estagios Simples] smallint NOT NULL,
    [N Estagios Paralela] smallint NOT NULL,
    [Diametro Bomba Simples] decimal(8,2) NOT NULL,
    [Diametro Bomba Paralela] decimal(8,2) NOT NULL,
    [Rendimento Bomba Simples] decimal(8,2) NOT NULL,
    [Rendimento Bomba Paralela] decimal(8,2) NOT NULL,
    [Rotação Bomba Simples] decimal(8,3) NOT NULL,
    [Rotação Bomba Paralela] decimal(8,3) NOT NULL,
    [Pressao Paralela] decimal(8,2) NOT NULL,
    [Marca do Motor] varchar(20) NOT NULL DEFAULT '',
    [Modelo Motor] varchar(25) NOT NULL DEFAULT '',
    [Nivel de Proteção] varchar(15) NOT NULL DEFAULT '',
    [Potencia Nominal] decimal(8,2) NOT NULL,
    [Nro de Fases] smallint NOT NULL,
    [Voltagem] decimal(8,2) NOT NULL,
    [Qtde de Motor] decimal(10,2) NOT NULL,
    [Valor Total das Peças] decimal(11,2) NOT NULL,
    [Valor Total dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total do Projeto] decimal(12,2) NOT NULL,
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Local de Entrega] varchar(120) NOT NULL DEFAULT '',
    [Prazo de Entrega Previsto] varchar(120) NOT NULL DEFAULT '',
    [Fone 1] varchar(14) NOT NULL DEFAULT '',
    [Sequencia do Vendedor] int NOT NULL,
    [Fixo ou Rebocavel] varchar(9) NOT NULL DEFAULT '',
    [Sequencia do Orçamento] int NOT NULL,
    [Total dos Serviços] decimal(10,2) NOT NULL,
    [Sequencia do Pneu] int NOT NULL,
    [Gerou Encargos] bit NOT NULL,
    [Atualizou Lista] bit NOT NULL,
    [Marca Bomba Aux] varchar(20) NOT NULL DEFAULT '',
    [Modelo Bomba Aux] varchar(25) NOT NULL DEFAULT '',
    [Rotor Bomba Aux] decimal(8,3) NOT NULL,
    [Rotação Bomba Aux] decimal(8,3) NOT NULL,
    [Mat Bomba Aux] varchar(25) NOT NULL DEFAULT '',
    [Vazao Bomba Aux] decimal(8,2) NOT NULL,
    [Pressao Bomba Aux] decimal(8,2) NOT NULL,
    [Rendimento Bomba Aux] decimal(8,2) NOT NULL,
    [BHP Bomba Aux] decimal(8,2) NOT NULL,
    [Valor do dolar] decimal(7,4) NOT NULL,
    [Qtde bomba aux] smallint NOT NULL,
    [Tamanho bomba aux] varchar(20) NOT NULL DEFAULT '',
    [N estagio bomba aux] smallint NOT NULL,
    [Diam bomba aux] smallint NOT NULL,
    [Venda Fechada] bit NOT NULL,
    [Entrega Tecnica] varchar(19) NOT NULL DEFAULT '',
    [Vendedor Intermediario] varchar(40) NOT NULL DEFAULT '',
    [Percentual do Vendedor] decimal(8,4) NOT NULL,
    [Rebiut] varchar(40) NOT NULL DEFAULT '',
    [Percentual Rebiut] decimal(8,4) NOT NULL,
    [Modelo Pivo] varchar(6) NOT NULL DEFAULT '',
    [Fabricante Spray Final] varchar(9) NOT NULL DEFAULT '',
    [Canhão ou Aspersor] varchar(8) NOT NULL DEFAULT '',
    [Inicio do Balanço] smallint NOT NULL,
    [Sequencia do Bocal] int NOT NULL,
    [Bomba Booster] bit NOT NULL,
    [CV Bomba Aux] decimal(7,2) NOT NULL,
    [Codigo do Conversor] int NOT NULL,
    [Outras Despesas] decimal(10,2) NOT NULL,
    [CPF Avulso] varchar(14) NOT NULL DEFAULT '',
    [cel avulso] varchar(14) NOT NULL DEFAULT '',
    [Obs] text NOT NULL DEFAULT '',
    CONSTRAINT [Sequencia do Projeto] PRIMARY KEY ([Sequencia do Projeto])
);
GO

CREATE TABLE [Propriedades] (
    [Seqüência da Propriedade] smallint NOT NULL IDENTITY,
    [Nome da Propriedade] varchar(62) NOT NULL DEFAULT '',
    [CNPJ] varchar(18) NOT NULL DEFAULT '',
    [Inscrição Estadual] varchar(20) NOT NULL DEFAULT '',
    [Endereco] varchar(100) NOT NULL DEFAULT '',
    [Número do Endereço] varchar(10) NOT NULL DEFAULT '',
    [Complemento] varchar(100) NOT NULL DEFAULT '',
    [Caixa Postal] varchar(30) NOT NULL DEFAULT '',
    [Bairro] varchar(50) NOT NULL DEFAULT '',
    [Seqüência do Município] int NOT NULL,
    [CEP] varchar(9) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência da Propriedade] PRIMARY KEY ([Seqüência da Propriedade])
);
GO

CREATE TABLE [PW~Grupos] (
    [PW~Nome] varchar(100) NOT NULL,
    CONSTRAINT [PW~Nome] PRIMARY KEY ([PW~Nome])
);
GO

CREATE TABLE [Razão Auxiliar] (
    [Sequencia do Razão] int NOT NULL IDENTITY,
    [Seqüência do Geral] int NOT NULL,
    [Data do Razão] datetime NULL,
    [Historico do Razão] text NOT NULL DEFAULT '',
    [Vr Entrada] decimal(10,2) NOT NULL,
    [Vr Saida] decimal(10,2) NOT NULL,
    CONSTRAINT [Sequencia do Razão] PRIMARY KEY ([Sequencia do Razão])
);
GO

CREATE TABLE [Receita primaria] (
    [Seqüência do Orçamento] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Seqüência da Matéria Prima] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Situacao] varchar(30) NOT NULL DEFAULT '',
    [Pedidos] varchar(120) NOT NULL DEFAULT '',
    [Id do Pedido] int NOT NULL,
    [Pagto] varchar(12) NOT NULL DEFAULT '',
    [Qtde Recebida] decimal(10,2) NOT NULL,
    [Qtde Restante] decimal(10,2) NOT NULL,
    [Qtde Total] decimal(10,2) NOT NULL,
    [Localizacao] varchar(50) NOT NULL DEFAULT '',
    [Sequencia produto principal] int NOT NULL,
    [Seqüência do Conjunto] int NOT NULL,
    [Qt Separada] decimal(11,4) NOT NULL,
    CONSTRAINT [Seq_e_materia] PRIMARY KEY ([Seqüência do Orçamento], [Sequencia do Item])
);
GO

CREATE TABLE [Região dos Vendedores] (
    [Seq do Vendedor] int NOT NULL IDENTITY,
    [Nome] varchar(30) NOT NULL DEFAULT '',
    CONSTRAINT [Seq do Vendedor] PRIMARY KEY ([Seq do Vendedor])
);
GO

CREATE TABLE [Resumo auxiliar] (
    [Sequencia do resumo] int NOT NULL IDENTITY,
    [Data do Movimento] datetime NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Sd inicial] decimal(11,2) NOT NULL,
    [Inicial estoque] decimal(11,4) NOT NULL,
    [Qt entradas] decimal(11,4) NOT NULL,
    [Qt saidas] decimal(11,4) NOT NULL,
    [V_entradas] decimal(12,4) NOT NULL,
    [V_saidas] decimal(12,4) NOT NULL,
    [Estoque_final] decimal(11,4) NOT NULL,
    [Sd final] decimal(11,2) NOT NULL,
    [Tipo do Movimento] smallint NOT NULL,
    [Seqüência da Baixa] int NOT NULL,
    CONSTRAINT [Sequencia do resumo] PRIMARY KEY ([Sequencia do resumo])
);
GO

CREATE TABLE [Saida de Balcao] (
    [Sequencia da Saida] int NOT NULL IDENTITY,
    [Data da Saida] datetime NULL,
    [Codigo do setor] smallint NOT NULL,
    [Codigo do solicitante] smallint NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Obs] text NOT NULL DEFAULT '',
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Codigo do solicitante 2] smallint NOT NULL,
    [Teste] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [Sequencia da Saida] PRIMARY KEY ([Sequencia da Saida])
);
GO

CREATE TABLE [Serie Gerador] (
    [Seq do Gerador] int NOT NULL IDENTITY,
    [Descri do Gerador] varchar(50) NOT NULL DEFAULT '',
    [Serie do Gerador] smallint NOT NULL,
    [MesAno] varchar(5) NOT NULL DEFAULT '',
    [Data de Criação] datetime NOT NULL,
    [Nro de Serie do Ger] varchar(30) NOT NULL DEFAULT '',
    [Nro do Motor] varchar(30) NOT NULL DEFAULT '',
    [Nro do Gerador] varchar(30) NOT NULL DEFAULT '',
    [Codigo do Geral] int NOT NULL,
    [Entregue] bit NOT NULL,
    [Dt de Entrega] datetime NULL,
    [NF] varchar(60) NOT NULL DEFAULT '',
    [Obs] text NOT NULL DEFAULT '',
    CONSTRAINT [Seq do Gerador] PRIMARY KEY ([Seq do Gerador])
);
GO

CREATE TABLE [Serie Hidroturbo] (
    [Seq do Hidroturbo] int NOT NULL IDENTITY,
    [Modelo do Hidroturbo] varchar(40) NOT NULL DEFAULT '',
    [Serie do Hidroturbo] int NOT NULL,
    [MesAno] varchar(5) NOT NULL DEFAULT '',
    [Letra do Hidroturbo] varchar(1) NOT NULL DEFAULT '',
    [Carretel de] varchar(3) NOT NULL DEFAULT '',
    [Data de Criação] datetime NOT NULL,
    [Nro de Serie Hidroturbo] varchar(30) NOT NULL DEFAULT '',
    [Codigo do Geral] int NOT NULL,
    [Codigo do Vendedor] int NOT NULL,
    [Entregue] bit NOT NULL,
    [Obs] text NOT NULL DEFAULT '',
    [NF] varchar(60) NOT NULL DEFAULT '',
    [Dta da Entrega] datetime NULL,
    [Entrega Tecnica] varchar(19) NOT NULL DEFAULT '',
    [Entrega Tec] bit NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Codigo do solicitante] smallint NOT NULL,
    [Data Tecnica] datetime NULL,
    [Aparecer no Filtro] bit NOT NULL,
    CONSTRAINT [Seq do Hidroturbo] PRIMARY KEY ([Seq do Hidroturbo])
);
GO

CREATE TABLE [Serie Moto Bomba] (
    [Seq Moto Bomba] int NOT NULL IDENTITY,
    [Tp de Motor] varchar(1) NOT NULL DEFAULT '',
    [Serie da Moto Bomba] int NOT NULL,
    [MesAno] varchar(5) NOT NULL DEFAULT '',
    [Função Moto Bomba] varchar(3) NOT NULL DEFAULT '',
    [Nro de Serie Moto Bomba] varchar(30) NOT NULL DEFAULT '',
    [Nro de Serie Motor] varchar(30) NOT NULL DEFAULT '',
    [Modelo do Motor] varchar(40) NOT NULL DEFAULT '',
    [Nro de Serie Bomba] varchar(30) NOT NULL DEFAULT '',
    [Modelo da Bomba] varchar(40) NOT NULL DEFAULT '',
    [Data de Criação] datetime NOT NULL,
    [Codigo do Geral] int NOT NULL,
    [Codigo do Vendedor] int NOT NULL,
    [Entregue] bit NOT NULL,
    [NF] varchar(60) NOT NULL DEFAULT '',
    [Dt da Entrega] datetime NULL,
    [Obs] text NOT NULL DEFAULT '',
    [Entrega Tecnica] varchar(19) NOT NULL DEFAULT '',
    [Entrega Tec] bit NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Codigo do solicitante] smallint NOT NULL,
    [Data Tecnica] datetime NULL,
    CONSTRAINT [Seq Moto Bomba] PRIMARY KEY ([Seq Moto Bomba])
);
GO

CREATE TABLE [Serie Pivos] (
    [Seq do Pivo] int NOT NULL IDENTITY,
    [Modelo do Pivo] varchar(6) NOT NULL DEFAULT '',
    [Descri do Pivo] varchar(30) NOT NULL DEFAULT '',
    [Serie do Pivo] smallint NOT NULL,
    [MesAno] varchar(5) NOT NULL DEFAULT '',
    [Letra do Pivo] varchar(1) NOT NULL DEFAULT '',
    [Data de Criação] datetime NOT NULL,
    [Nro de Serie do Pivo] varchar(30) NOT NULL DEFAULT '',
    [Codigo do Geral] int NOT NULL,
    [Entregue] bit NOT NULL,
    [Obs] text NOT NULL DEFAULT '',
    [NF] varchar(60) NOT NULL DEFAULT '',
    [Data da Entrega] datetime NULL,
    [Codigo do Vendedor] int NOT NULL,
    [Entrega Tecnica] varchar(19) NOT NULL DEFAULT '',
    [Entrega Tec] bit NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Codigo do solicitante] smallint NOT NULL,
    [Data Tecnica] datetime NULL,
    [Prev Montagem] datetime NULL,
    [DadosAd] text NOT NULL DEFAULT '',
    CONSTRAINT [Seq do Pivo] PRIMARY KEY ([Seq do Pivo])
);
GO

CREATE TABLE [Serie Rebocador] (
    [Seq do Rebocador] int NOT NULL IDENTITY,
    [Data de Criação] datetime NOT NULL,
    [Serie do Rebocador] smallint NOT NULL,
    [Modelo do Rebocador] varchar(50) NOT NULL DEFAULT '',
    [MesAno] varchar(5) NOT NULL DEFAULT '',
    [Nro de Serie Rebocador] varchar(30) NOT NULL DEFAULT '',
    [Codigo do Geral] int NOT NULL,
    [Entregue] bit NOT NULL,
    [NF] varchar(60) NOT NULL DEFAULT '',
    [Obs] text NOT NULL DEFAULT '',
    [Data da Entrega] datetime NULL,
    [Codigo do Vendedor] int NOT NULL,
    CONSTRAINT [Seq do Rebocador] PRIMARY KEY ([Seq do Rebocador])
);
GO

CREATE TABLE [Servicos] (
    [Seqüência do Serviço] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Valor do Serviço] decimal(11,2) NOT NULL,
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência do Serviço] PRIMARY KEY ([Seqüência do Serviço])
);
GO

CREATE TABLE [Serviços da Ordem] (
    [Id da Ordem] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Sequencia do Serviço] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitario] decimal(12,2) NOT NULL,
    [Valor do Iss] decimal(12,2) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    CONSTRAINT [Id_e_servico] PRIMARY KEY ([Id da Ordem], [Sequencia do Item])
);
GO

CREATE TABLE [Serviços do Projeto] (
    [Sequencia do Projeto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Seqüência do Serviço] smallint NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitario] decimal(12,2) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Parte do Pivo] varchar(29) NOT NULL DEFAULT '',
    [Valor Anterior] decimal(12,2) NOT NULL,
    CONSTRAINT [seq_e_servico] PRIMARY KEY ([Sequencia do Projeto], [Sequencia do Item])
);
GO

CREATE TABLE [Setores] (
    [Codigo do setor] smallint NOT NULL IDENTITY,
    [Nome do setor] varchar(25) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo do setor] PRIMARY KEY ([Codigo do setor])
);
GO

CREATE TABLE [Simula estoque] (
    [Sequencia da simulação] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Atual estoque] decimal(11,2) NOT NULL,
    [Necessario estoque] decimal(11,2) NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Saidas estoque] decimal(11,2) NOT NULL,
    [Saidas peças] decimal(11,2) NOT NULL,
    [Entradas pedido] decimal(11,2) NOT NULL,
    [Saidas orc prod] decimal(11,2) NOT NULL,
    [Saida orc peças] decimal(11,2) NOT NULL,
    [Número da NFe] int NOT NULL,
    [Último Fornecedor] int NOT NULL,
    [Ultimo custo] decimal(11,2) NOT NULL,
    CONSTRAINT [Sequencia da simulação] PRIMARY KEY ([Sequencia da simulação])
);
GO

CREATE TABLE [Situação dos pedidos] (
    [Seqüência do Orçamento] int NOT NULL IDENTITY,
    [Data pedido] datetime NULL,
    [Prev entrega] datetime NULL,
    [Dias em atraso] smallint NOT NULL,
    [Obs fabrica] varchar(120) NOT NULL DEFAULT '',
    [Obs vendas] varchar(120) NOT NULL DEFAULT '',
    [Obs compras] varchar(120) NOT NULL DEFAULT '',
    [Obs almoxarifado] varchar(120) NOT NULL DEFAULT '',
    [Desc material] varchar(120) NOT NULL DEFAULT '',
    [Status] varchar(8) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência_orc_situacao] PRIMARY KEY ([Seqüência do Orçamento])
);
GO

CREATE TABLE [Solicitantes] (
    [Codigo do solicitante] smallint NOT NULL IDENTITY,
    [Codigo do setor] smallint NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Nome do solicitante] varchar(25) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo do solicitante] PRIMARY KEY ([Codigo do solicitante], [Sequencia do Item], [Codigo do setor])
);
GO

CREATE TABLE [Spy Baixa Contas] (
    [Seq do Spy] int NOT NULL IDENTITY,
    [Seq da Baixa] int NOT NULL,
    [Dt Inclusão] datetime NOT NULL,
    [Usuario] varchar(20) NOT NULL DEFAULT '',
    [TpConta] varchar(1) NOT NULL DEFAULT '',
    [Manutencao] bigint NOT NULL,
    [Dt Baixa] datetime NULL,
    [Juros] decimal(11,2) NOT NULL,
    [Desconto] decimal(11,2) NOT NULL,
    [Vr Pago] decimal(11,2) NOT NULL,
    [Tp Carteira] varchar(20) NOT NULL DEFAULT '',
    [Bx Cliente] datetime NULL,
    [Quem Pagou] varchar(20) NOT NULL DEFAULT '',
    [Vr Cliente] decimal(10,2) NOT NULL,
    [Seq Banco] smallint NOT NULL,
    [Seq Acc Banco] smallint NOT NULL,
    CONSTRAINT [Seq do Spy] PRIMARY KEY ([Seq do Spy])
);
GO

CREATE TABLE [Status do Processo] (
    [Codigo do Status] smallint NOT NULL IDENTITY,
    [Descrição do Status] varchar(20) NOT NULL DEFAULT '',
    CONSTRAINT [Codigo do Status] PRIMARY KEY ([Codigo do Status])
);
GO

CREATE TABLE [SYS~Sequencial] (
    [SYS~Tabela] varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [SYS~Campo] varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [SYS~Chave] varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [PW~Projeto] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [SYS~Valor] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [SYS~ValorAnterior] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [SYS~Estacao] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [SYS~Identificacao] varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
    [SYS~Pendentes] int NOT NULL,
    CONSTRAINT [Chave sequencial] PRIMARY KEY ([SYS~Tabela], [SYS~Campo], [SYS~Chave])
);
GO

CREATE TABLE [Tenants] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Dominio] nvarchar(200) NOT NULL,
    [ConnectionString] nvarchar(500) NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Tenants] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Tipo de Atividades] (
    [Codigo da Atividade] smallint NOT NULL IDENTITY,
    [Descrição da Atividade] varchar(40) NOT NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    CONSTRAINT [Codigo da Atividade] PRIMARY KEY ([Codigo da Atividade])
);
GO

CREATE TABLE [Tipo de Cobrança] (
    [Seqüência da Cobrança] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência da Cobrança] PRIMARY KEY ([Seqüência da Cobrança])
);
GO

CREATE TABLE [Tipo de Titulos] (
    [Seq do Titulo] smallint NOT NULL IDENTITY,
    [Titulo] varchar(25) NOT NULL DEFAULT '',
    CONSTRAINT [Seq do Titulo] PRIMARY KEY ([Seq do Titulo])
);
GO

CREATE TABLE [Unidades] (
    [Seqüência da Unidade] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Sigla da Unidade] varchar(15) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência da Unidade] PRIMARY KEY ([Seqüência da Unidade])
);
GO

CREATE TABLE [Vasilhames] (
    [Sequencia do Vasilahme] int NOT NULL IDENTITY,
    [Número da NFe] int NOT NULL,
    [Data de Emissão] datetime NULL,
    [Seqüência do Geral] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Mov] varchar(1) NOT NULL DEFAULT '',
    CONSTRAINT [Sequencia do Vasilahme] PRIMARY KEY ([Sequencia do Vasilahme])
);
GO

CREATE TABLE [Veiculos do Motorista] (
    [Codigo do Motorista] smallint NOT NULL,
    [Automovel] varchar(100) NOT NULL DEFAULT '',
    [Placa do Automovel] varchar(20) NOT NULL DEFAULT '',
    [Placa da Carreta] varchar(20) NOT NULL DEFAULT ''
);
GO

CREATE TABLE [Vendedores Bloqueio] (
    [Codigo do Vendedor] int NOT NULL IDENTITY,
    [Nome do Vendedor] varchar(30) NOT NULL DEFAULT '',
    [Percentual] decimal(8,4) NOT NULL,
    [Codigo ipg] int NOT NULL,
    CONSTRAINT [Codigo do Vendedor Blok] PRIMARY KEY ([Codigo do Vendedor])
);
GO

CREATE TABLE [Via de Transporte DI] (
    [Seq do Transporte] smallint NOT NULL,
    [Transporte] varchar(30) NOT NULL DEFAULT ''
);
GO

CREATE TABLE [Planilha de Adiantamento] (
    [Seq do Adiantamento] int NOT NULL IDENTITY,
    [Ano] smallint NOT NULL,
    [Cod do Vendedor] int NOT NULL,
    [Manutencao] int NOT NULL,
    [Emissão NFe] datetime NULL,
    [NFe] varchar(10) NOT NULL DEFAULT '',
    [Parcela] smallint NOT NULL,
    [Cod do Geral] int NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Vencto] datetime NULL,
    [Pagto Cliente] datetime NULL,
    [VrIPI] decimal(10,2) NOT NULL,
    [Percentual] decimal(8,4) NOT NULL,
    [Comissao] decimal(10,2) NOT NULL,
    [Pagto Vendedor] datetime NULL,
    [Obs] text NOT NULL DEFAULT '',
    [Devolucao] bit NOT NULL,
    [Valor Pago] decimal(11,2) NOT NULL,
    [Seqüência da Baixa] int NOT NULL,
    CONSTRAINT [Seq do Adiantamento] PRIMARY KEY ([Seq do Adiantamento]),
    CONSTRAINT [TB_Planilha_de_Adiantamento_FK_Cod_do_Vendedor] FOREIGN KEY ([Cod do Vendedor]) REFERENCES [Conta do Vendedor] ([Id da Conta])
);
GO

CREATE TABLE [Revendedores] (
    [Sequencia da Revenda] int NOT NULL IDENTITY,
    [Id da Conta] int NOT NULL,
    [Tem Contrato] bit NOT NULL,
    CONSTRAINT [Sequencia da Revenda] PRIMARY KEY ([Sequencia da Revenda]),
    CONSTRAINT [TB_Revendedores_FK_Id_da_Conta] FOREIGN KEY ([Id da Conta]) REFERENCES [Conta do Vendedor] ([Id da Conta])
);
GO

CREATE TABLE [SubGrupo Despesa] (
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência SubGrupo Despesa] PRIMARY KEY ([Seqüência SubGrupo Despesa], [Seqüência Grupo Despesa]),
    CONSTRAINT [TB_SubGrupo_Despesa_FK_Seqüência_Grupo_Despesa] FOREIGN KEY ([Seqüência Grupo Despesa]) REFERENCES [Grupo da Despesa] ([Seqüência Grupo Despesa]) ON DELETE CASCADE
);
GO

CREATE TABLE [SubGrupo do Produto] (
    [Seqüência do SubGrupo Produto] smallint NOT NULL IDENTITY,
    [Seqüência do Grupo Produto] smallint NOT NULL,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [Seqüência do SubGrupo Produto] PRIMARY KEY ([Seqüência do SubGrupo Produto], [Seqüência do Grupo Produto]),
    CONSTRAINT [TB_SubGrupo_do_Produto_FK_Seqüência_do_Grupo_Produto] FOREIGN KEY ([Seqüência do Grupo Produto]) REFERENCES [Grupo do Produto] ([Seqüência do Grupo Produto]) ON DELETE CASCADE
);
GO

CREATE TABLE [Movimentação da Conta Corrente] (
    [Seqüência da Movimentação CC] int NOT NULL IDENTITY,
    [Seqüência da Agência] smallint NOT NULL,
    [Seqüência da CC da Agência] smallint NOT NULL,
    [Tipo de Movimento da CC] varchar(7) NOT NULL DEFAULT '',
    [Data do Movimento] datetime NOT NULL,
    [Data do Último Dia] datetime NULL,
    [Historico] text NOT NULL DEFAULT '',
    [Conta] varchar(1) NOT NULL DEFAULT '',
    [Seqüência do Lançamento] int NOT NULL,
    [Seqüência do Histórico] smallint NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Origem da Movimentação] varchar(30) NOT NULL DEFAULT '',
    [Blokeado] bit NOT NULL,
    [Codigo do Historico] smallint NOT NULL,
    [Codigo do Debito] int NOT NULL,
    [Codigo do Credito] int NOT NULL,
    [Usuário da Alteração] varchar(60) NOT NULL DEFAULT '',
    [Data da Alteração] datetime NULL,
    [Hora da Alteração] datetime NULL,
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [SeqüênciaDaAgência] smallint NOT NULL,
    [SeqüênciaDaCcDaAgência] smallint NOT NULL,
    CONSTRAINT [Seqüência da Movimentação CC] PRIMARY KEY ([Seqüência da Movimentação CC]),
    CONSTRAINT [TB_Movimentação_da_Conta_Corrente_FK_Seqüência_da_Agência_Seqüência_da_CC_da_Agência] FOREIGN KEY ([SeqüênciaDaAgência], [SeqüênciaDaCcDaAgência]) REFERENCES [Conta Corrente da Agência] ([Seqüência da Agência], [Seqüência da CC da Agência]),
    CONSTRAINT [TB_Movimentação_da_Conta_Corrente_FK_Seqüência_do_Histórico] FOREIGN KEY ([Seqüência do Histórico]) REFERENCES [Histórico da Conta Corrente] ([Seqüência do Histórico])
);
GO

CREATE TABLE [Geral] (
    [Seqüência do Geral] int NOT NULL IDENTITY,
    [Cliente] bit NOT NULL,
    [Fornecedor] bit NOT NULL,
    [Despesa] bit NOT NULL,
    [Imposto] bit NOT NULL,
    [Transportadora] bit NOT NULL,
    [Vendedor] bit NOT NULL,
    [Razão Social] varchar(60) NOT NULL DEFAULT '',
    [Nome Fantasia] varchar(60) NOT NULL DEFAULT '',
    [Tipo] smallint NOT NULL,
    [Endereco] varchar(100) NOT NULL DEFAULT '',
    [Complemento] varchar(100) NOT NULL DEFAULT '',
    [Número do Endereço] varchar(10) NOT NULL DEFAULT '',
    [Bairro] varchar(50) NOT NULL DEFAULT '',
    [Caixa Postal] varchar(30) NOT NULL DEFAULT '',
    [Seqüência do Município] int NOT NULL,
    [CEP] varchar(9) NOT NULL DEFAULT '',
    [Fone 1] varchar(14) NULL DEFAULT '',
    [Fone 2] varchar(14) NULL DEFAULT '',
    [Fax] varchar(14) NULL DEFAULT '',
    [Celular] varchar(14) NULL DEFAULT '',
    [Contato] varchar(45) NOT NULL DEFAULT '',
    [Email] varchar(255) NOT NULL DEFAULT '',
    [Home Page] varchar(60) NOT NULL DEFAULT '',
    [Código do Suframa] varchar(9) NOT NULL DEFAULT '',
    [Código da ANTT] varchar(20) NOT NULL DEFAULT '',
    [CPF e CNPJ] varchar(20) NOT NULL DEFAULT '',
    [RG e IE] varchar(20) NOT NULL DEFAULT '',
    [Observacao] text NOT NULL DEFAULT '',
    [Endereço de Cobrança] varchar(62) NOT NULL DEFAULT '',
    [Número do Endereço de Cobrança] varchar(20) NOT NULL DEFAULT '',
    [Seqüência Município Cobrança] int NOT NULL,
    [Cep de Cobrança] varchar(9) NOT NULL DEFAULT '',
    [Bairro de Cobrança] varchar(50) NOT NULL DEFAULT '',
    [Complemento da Cobrança] varchar(30) NOT NULL DEFAULT '',
    [Caixa Postal da Cobrança] varchar(30) NOT NULL DEFAULT '',
    [Seqüência do Vendedor] int NOT NULL,
    [Intermediário do Vendedor] varchar(40) NOT NULL DEFAULT '',
    [Nome do Banco 1] varchar(20) NOT NULL DEFAULT '',
    [Nome do Banco 2] varchar(20) NOT NULL DEFAULT '',
    [Agência do Banco 1] varchar(20) NOT NULL DEFAULT '',
    [Agência do Banco 2] varchar(20) NOT NULL DEFAULT '',
    [Conta Corrente do Banco 1] varchar(15) NOT NULL DEFAULT '',
    [Conta Corrente do Banco 2] varchar(15) NOT NULL DEFAULT '',
    [Nome do Correntista do Banco 1] varchar(60) NOT NULL DEFAULT '',
    [Nome do Correntista do Banco 2] varchar(60) NOT NULL DEFAULT '',
    [Inativo] bit NOT NULL,
    [Revenda] bit NOT NULL,
    [Isento] bit NOT NULL,
    [Data do Cadastro] datetime NULL,
    [Seqüência do País] int NOT NULL,
    [Orgõn Publico] bit NOT NULL,
    [Cumulativo] bit NOT NULL,
    [Empresa Produtor] bit NOT NULL,
    [Usu da Alteração] varchar(40) NOT NULL DEFAULT '',
    [Data de Nascimento] datetime NULL,
    [Codigo Contabil] int NOT NULL,
    [Codigo Adiantamento] int NOT NULL,
    [Sal bruto] decimal(10,2) NOT NULL,
    [Importou no Zap] bit NOT NULL,
    CONSTRAINT [Seqüência do Geral] PRIMARY KEY ([Seqüência do Geral]),
    CONSTRAINT [TB_Geral_FK_Seqüência_Município_Cobrança] FOREIGN KEY ([Seqüência Município Cobrança]) REFERENCES [Municipios] ([Seqüência do Município]),
    CONSTRAINT [TB_Geral_FK_Seqüência_do_Município] FOREIGN KEY ([Seqüência do Município]) REFERENCES [Municipios] ([Seqüência do Município]),
    CONSTRAINT [TB_Geral_FK_Seqüência_do_País] FOREIGN KEY ([Seqüência do País]) REFERENCES [Paises] ([Seqüência do País]),
    CONSTRAINT [TB_Vendedor_Geral] FOREIGN KEY ([Seqüência do Vendedor]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [PW~Tabelas] (
    [PW~Projeto] varchar(10) NOT NULL,
    [PW~Grupo] varchar(100) NOT NULL,
    [PW~Nome] varchar(100) NOT NULL,
    [PW~Permissoes] varchar(100) NOT NULL,
    CONSTRAINT [Chave tabelas] PRIMARY KEY ([PW~Projeto], [PW~Grupo], [PW~Nome]),
    CONSTRAINT [TB_PW~Tabelas_FK_PW~Grupo] FOREIGN KEY ([PW~Grupo]) REFERENCES [PW~Grupos] ([PW~Nome]) ON DELETE CASCADE
);
GO

CREATE TABLE [PW~Usuarios] (
    [PW~Nome] varchar(100) NOT NULL,
    [PW~Senha] varchar(100) NOT NULL,
    [PW~Grupo] varchar(100) NOT NULL,
    [PW~Obs] varchar(100) NULL,
    [PW~SenhaHash] varchar(255) NULL,
    CONSTRAINT [Chave usuario] PRIMARY KEY ([PW~Nome], [PW~Senha]),
    CONSTRAINT [TB_PW~Usuarios_FK_PW~Grupo] FOREIGN KEY ([PW~Grupo]) REFERENCES [PW~Grupos] ([PW~Nome]) ON DELETE CASCADE
);
GO

CREATE TABLE [Linha de Produção] (
    [Sequencia da Produção] int NOT NULL IDENTITY,
    [Data da Produção] datetime NULL,
    [Codigo do setor] smallint NOT NULL,
    [Codigo do Colaborador] smallint NOT NULL,
    [Solicitação de] varchar(10) NOT NULL DEFAULT '',
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência do Movimento] int NOT NULL,
    [Finalizado] bit NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Apenas Montagem] bit NOT NULL,
    CONSTRAINT [Sequencia da Produção] PRIMARY KEY ([Sequencia da Produção]),
    CONSTRAINT [TB_Linha_de_Produção_FK_Codigo_do_setor] FOREIGN KEY ([Codigo do setor]) REFERENCES [Setores] ([Codigo do setor])
);
GO

CREATE TABLE [Controle de Processos] (
    [Id do Processo] int NOT NULL IDENTITY,
    [Codigo do Status] smallint NOT NULL,
    [Codigo do Advogado] smallint NOT NULL,
    [Codigo da Ação] smallint NOT NULL,
    [Outro Envolvido] int NOT NULL,
    CONSTRAINT [Id do Processo] PRIMARY KEY ([Id do Processo]),
    CONSTRAINT [TB_Controle_de_Processos_FK_Codigo_do_Advogado] FOREIGN KEY ([Codigo do Advogado]) REFERENCES [Advogados] ([Codigo do Advogado]),
    CONSTRAINT [TB_Controle_de_Processos_FK_Codigo_do_Status] FOREIGN KEY ([Codigo do Status]) REFERENCES [Status do Processo] ([Codigo do Status])
);
GO

CREATE TABLE [Despesas] (
    [Seqüência da Despesa] int NOT NULL IDENTITY,
    [Inativo] bit NOT NULL,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Seqüência da Unidade] smallint NOT NULL,
    [Quantidade no Estoque] decimal(11,4) NOT NULL,
    [Quantidade Mínima] decimal(9,3) NOT NULL,
    [Código de Barras] varchar(13) NOT NULL DEFAULT '',
    [Localizacao] varchar(50) NOT NULL DEFAULT '',
    [Última Compra] datetime NULL,
    [Último Movimento] datetime NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Custo Médio] decimal(11,2) NOT NULL,
    [Último Fornecedor] int NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [Tipo do Produto] smallint NOT NULL,
    [Quantidade Contábil] decimal(11,4) NOT NULL,
    [Valor Contábil Atual] decimal(13,4) NOT NULL,
    [Margem de Lucro] decimal(7,2) NOT NULL,
    [Movimenta ficha] bit NOT NULL,
    [SeqüênciaSubGrupoDespesa] smallint NOT NULL,
    [SeqüênciaGrupoDespesa] smallint NOT NULL,
    CONSTRAINT [Seqüência da Despesa] PRIMARY KEY ([Seqüência da Despesa]),
    CONSTRAINT [TB_Despesas_FK_Seqüência_Grupo_Despesa] FOREIGN KEY ([Seqüência Grupo Despesa]) REFERENCES [Grupo da Despesa] ([Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Despesas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa] FOREIGN KEY ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]) REFERENCES [SubGrupo Despesa] ([Seqüência SubGrupo Despesa], [Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Despesas_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Despesas_FK_Seqüência_da_Unidade] FOREIGN KEY ([Seqüência da Unidade]) REFERENCES [Unidades] ([Seqüência da Unidade])
);
GO

CREATE TABLE [Conjuntos] (
    [Seqüência do Conjunto] int NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Detalhes] text NOT NULL DEFAULT '',
    [Quantidade no Estoque] decimal(11,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Seqüência do Grupo Produto] smallint NOT NULL,
    [Seqüência do SubGrupo Produto] smallint NOT NULL,
    [Seqüência da Unidade] smallint NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [Usado] bit NOT NULL,
    [Último Movimento] datetime NULL,
    [Localizacao] varchar(50) NOT NULL DEFAULT '',
    [Quantidade Contábil] decimal(11,4) NOT NULL,
    [Valor Contábil Atual] decimal(13,4) NOT NULL,
    [Inativo] bit NOT NULL,
    [Quantidade Mínima] decimal(9,3) NOT NULL,
    [Última Entrada] datetime NULL,
    [Altura do Conjunto] varchar(10) NOT NULL DEFAULT '',
    [Largura do Conjunto] varchar(10) NOT NULL DEFAULT '',
    [Comprimento] varchar(10) NOT NULL DEFAULT '',
    [Peso do Conjunto] decimal(12,3) NOT NULL,
    [Parte do Pivo] varchar(29) NOT NULL DEFAULT '',
    [Trava receita] bit NOT NULL,
    [Receita Conferida] bit NOT NULL,
    [Margem de Lucro] decimal(11,4) NOT NULL,
    [Valor Total Anterior] decimal(12,4) NULL,
    [SeqüênciaDoSubGrupoProduto] smallint NOT NULL,
    [SeqüênciaDoGrupoProduto] smallint NOT NULL,
    CONSTRAINT [Seqüência do Conjunto] PRIMARY KEY ([Seqüência do Conjunto]),
    CONSTRAINT [TB_Conjuntos_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Conjuntos_FK_Seqüência_da_Unidade] FOREIGN KEY ([Seqüência da Unidade]) REFERENCES [Unidades] ([Seqüência da Unidade]),
    CONSTRAINT [TB_Conjuntos_FK_Seqüência_do_Grupo_Produto] FOREIGN KEY ([Seqüência do Grupo Produto]) REFERENCES [Grupo do Produto] ([Seqüência do Grupo Produto]),
    CONSTRAINT [TB_Conjuntos_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto] FOREIGN KEY ([SeqüênciaDoSubGrupoProduto], [SeqüênciaDoGrupoProduto]) REFERENCES [SubGrupo do Produto] ([Seqüência do SubGrupo Produto], [Seqüência do Grupo Produto])
);
GO

CREATE TABLE [Produtos] (
    [Seqüência do Produto] int NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Seqüência do Grupo Produto] smallint NOT NULL,
    [Seqüência do SubGrupo Produto] smallint NOT NULL,
    [Última Compra] datetime NULL,
    [Quantidade no Estoque] decimal(11,4) NOT NULL,
    [Quantidade Mínima] decimal(9,3) NOT NULL,
    [Último Movimento] datetime NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Margem de Lucro] decimal(11,4) NOT NULL,
    [Seqüência da Unidade] smallint NOT NULL,
    [Código de Barras] varchar(13) NOT NULL DEFAULT '',
    [Valor Total] decimal(12,4) NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [É Matéria Prima] bit NOT NULL,
    [Custo Médio] decimal(11,2) NOT NULL,
    [Usado] bit NOT NULL,
    [Último Fornecedor] int NOT NULL,
    [Tipo do Produto] smallint NOT NULL,
    [Inativo] bit NOT NULL,
    [Localizacao] varchar(50) NOT NULL DEFAULT '',
    [Quantidade Contábil] decimal(11,4) NOT NULL,
    [Valor Contábil Atual] decimal(13,4) NOT NULL,
    [Material Adquirido de Terceiro] bit NOT NULL,
    [Valor Atualizado] bit NOT NULL,
    [Valor Anterior] decimal(12,2) NOT NULL,
    [Sucata] bit NOT NULL,
    [Peso] decimal(12,3) NOT NULL,
    [Industrializacao] bit NOT NULL,
    [Importado] bit NOT NULL,
    [Medida] varchar(100) NOT NULL DEFAULT '',
    [Valor de Lista] decimal(12,2) NOT NULL,
    [Usuário da Alteração] varchar(60) NOT NULL DEFAULT '',
    [Data da Alteração] datetime NULL,
    [Hora da Alteração] datetime NULL,
    [Modelo do Lance] int NOT NULL,
    [Usado no Projeto] bit NOT NULL,
    [Parte do Pivo] varchar(29) NOT NULL DEFAULT '',
    [Quantidade Fisica] decimal(11,4) NOT NULL,
    [Data da Contagem] datetime NULL,
    [Não Sair no Relatório] bit NOT NULL,
    [Mostrar Receita Secundaria] bit NOT NULL,
    [Nao Mostrar Receita] bit NOT NULL,
    [Nao sair no checklist] bit NOT NULL,
    [Trava receita] bit NOT NULL,
    [Lance] bit NOT NULL,
    [Mp inicial] bit NOT NULL,
    [Qtde Inicial] decimal(10,2) NOT NULL,
    [E Regulador] bit NOT NULL,
    [Separado Montar] decimal(11,4) NOT NULL,
    [Comprados Aguardando] decimal(11,4) NOT NULL,
    [Conferido pelo Contabil] bit NOT NULL,
    [Obsoleto] bit NOT NULL,
    [Marcar] bit NOT NULL,
    [Ultima Cotação] datetime NULL,
    [Medida Final] varchar(20) NOT NULL DEFAULT '',
    [Receita Conferida] bit NOT NULL,
    [Detalhes] text NOT NULL DEFAULT '',
    [Quantidade Balanço] decimal(11,4) NOT NULL,
    [Peso Ok] bit NOT NULL,
    [Valor de Custo Anterior] decimal(12,4) NULL,
    [Valor Total Anterior] decimal(12,4) NULL,
    [Margem de Lucro Anterior] decimal(12,4) NULL,
    [SeqüênciaDoSubGrupoProduto] smallint NOT NULL,
    [SeqüênciaDoGrupoProduto] smallint NOT NULL,
    CONSTRAINT [Seqüência do Produto] PRIMARY KEY ([Seqüência do Produto]),
    CONSTRAINT [TB_Produtos_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Produtos_FK_Seqüência_da_Unidade] FOREIGN KEY ([Seqüência da Unidade]) REFERENCES [Unidades] ([Seqüência da Unidade]),
    CONSTRAINT [TB_Produtos_FK_Seqüência_do_Grupo_Produto] FOREIGN KEY ([Seqüência do Grupo Produto]) REFERENCES [Grupo do Produto] ([Seqüência do Grupo Produto]),
    CONSTRAINT [TB_Produtos_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto] FOREIGN KEY ([SeqüênciaDoSubGrupoProduto], [SeqüênciaDoGrupoProduto]) REFERENCES [SubGrupo do Produto] ([Seqüência do SubGrupo Produto], [Seqüência do Grupo Produto])
);
GO

CREATE TABLE [Declarações de Importação] (
    [Seqüência da Declaração] int NOT NULL IDENTITY,
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Seqüência Produto Nota Fiscal] int NOT NULL,
    [Número da Declaração] varchar(10) NOT NULL DEFAULT '',
    [Data de Registro] datetime NOT NULL,
    [Local de Desembaraço] varchar(60) NOT NULL DEFAULT '',
    [UF de Desembaraço] varchar(3) NOT NULL DEFAULT '',
    [Data de Desembaraço] datetime NOT NULL,
    [Seqüência do Geral] int NOT NULL,
    [Valor da Marinha Mercante] decimal(10,2) NOT NULL,
    [Via Transporte] smallint NOT NULL,
    CONSTRAINT [Seqüência da Declaração] PRIMARY KEY ([Seqüência da Declaração]),
    CONSTRAINT [TB_Declarações_de_Importação_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Entrada Contas] (
    [Seqüência da Entrada] int NOT NULL IDENTITY,
    [Data de Entrada] datetime NULL,
    [Seqüência do Geral] int NOT NULL,
    [Número da Nota Fiscal] int NOT NULL,
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Número da Duplicata] int NOT NULL,
    [Historico] text NOT NULL DEFAULT '',
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Valor Total] decimal(12,4) NOT NULL,
    [Tipo da Conta] varchar(11) NOT NULL DEFAULT '',
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Conta] varchar(1) NOT NULL DEFAULT '',
    [Data da Alteração] datetime NULL,
    [Hora da Alteração] datetime NULL,
    [Usuário da Alteração] varchar(60) NOT NULL DEFAULT '',
    [Previsao] bit NOT NULL,
    [Titulo] varchar(25) NOT NULL DEFAULT '',
    [Sequencia da Compra] int NOT NULL,
    [Codigo do Debito] int NOT NULL,
    [Codigo do Credito] int NOT NULL,
    [SeqüênciaSubGrupoDespesa] smallint NOT NULL,
    [SeqüênciaGrupoDespesa] smallint NOT NULL,
    CONSTRAINT [Seqüência da Entrada] PRIMARY KEY ([Seqüência da Entrada]),
    CONSTRAINT [TB_Entrada_Contas_FK_Seqüência_Grupo_Despesa] FOREIGN KEY ([Seqüência Grupo Despesa]) REFERENCES [Grupo da Despesa] ([Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Entrada_Contas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa] FOREIGN KEY ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]) REFERENCES [SubGrupo Despesa] ([Seqüência SubGrupo Despesa], [Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Entrada_Contas_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Movimento do Estoque] (
    [Seqüência do Movimento] int NOT NULL IDENTITY,
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Seqüência do Geral] int NOT NULL,
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Data da Compra] datetime NULL,
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Observacao] text NOT NULL DEFAULT '',
    [Valor Total dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total dos Conjuntos] decimal(11,2) NOT NULL,
    [Processar Custo] bit NOT NULL,
    [Data de Entrada] datetime NULL,
    [Valor Total IPI dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Conjuntos] decimal(11,2) NOT NULL,
    [Tipo do Movimento] smallint NOT NULL,
    [Valor Total do Movimento] decimal(11,2) NOT NULL,
    [Valor Total do ICMS ST] decimal(11,2) NOT NULL,
    [Valor do Fechamento] decimal(11,2) NOT NULL,
    [Fechamento] smallint NOT NULL,
    [Valor do Seguro] decimal(11,2) NOT NULL,
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Tipo Movimento] smallint NOT NULL,
    [Valor Total IPI das Peças] decimal(11,2) NOT NULL,
    [Valor Total das Peças] decimal(11,2) NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [Seqüência da Propriedade] smallint NOT NULL,
    [Valor Total da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor Total do ICMS] decimal(11,2) NOT NULL,
    [Movimento Cancelado] bit NOT NULL,
    [Não Totaliza] bit NOT NULL,
    [Data da Alteração] datetime NULL,
    [Hora da Alteração] datetime NULL,
    [Usuário da Alteração] varchar(60) NOT NULL DEFAULT '',
    [Valor Total IPI das Despesas] decimal(11,2) NOT NULL,
    [Valor Total das Despesas] decimal(11,2) NOT NULL,
    [Industrializacao] bit NOT NULL,
    [Outras Despesas] decimal(10,2) NOT NULL,
    [Titulo] varchar(25) NOT NULL DEFAULT '',
    [Nao_Alterar] bit NOT NULL,
    [Nota de venda] int NOT NULL,
    [SeqüênciaSubGrupoDespesa] smallint NOT NULL,
    [SeqüênciaGrupoDespesa] smallint NOT NULL,
    CONSTRAINT [Seq Movimento] PRIMARY KEY ([Seqüência do Movimento]),
    CONSTRAINT [TB_Movimento_do_Estoque_FK_Seqüência_Grupo_Despesa] FOREIGN KEY ([Seqüência Grupo Despesa]) REFERENCES [Grupo da Despesa] ([Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Movimento_do_Estoque_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa] FOREIGN KEY ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]) REFERENCES [SubGrupo Despesa] ([Seqüência SubGrupo Despesa], [Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Movimento_do_Estoque_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Movimento_do_Estoque_FK_Seqüência_da_Propriedade] FOREIGN KEY ([Seqüência da Propriedade]) REFERENCES [Propriedades] ([Seqüência da Propriedade]),
    CONSTRAINT [TB_Movimento_do_Estoque_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Movimento do Estoque Contábil] (
    [Seqüência do Movimento] int NOT NULL IDENTITY,
    [Data do Movimento] datetime NULL,
    [Tipo do Movimento] smallint NOT NULL,
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Tipo do Produto] smallint NOT NULL,
    [Seqüência do Geral] int NOT NULL,
    [Observacao] text NOT NULL DEFAULT '',
    [Devolucao] bit NOT NULL,
    CONSTRAINT [Seq Mvto Contabil] PRIMARY KEY ([Seqüência do Movimento]),
    CONSTRAINT [TB_Movimento_do_Estoque_Contábil_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Nova Licitação] (
    [Codigo da Licitação] int NOT NULL IDENTITY,
    [Dt da Licitação] datetime NULL,
    [Contato] varchar(45) NOT NULL DEFAULT '',
    [Prev de Entrega] datetime NULL,
    [Sequencia do Fornecedor] int NOT NULL,
    [Sequencia da Transportadora] int NOT NULL,
    [Comprador] varchar(30) NOT NULL DEFAULT '',
    [Tipo da Licitação] varchar(25) NOT NULL DEFAULT '',
    [Tipo de Frete] varchar(3) NOT NULL DEFAULT '',
    [Totalizar Frete] bit NOT NULL,
    [Nome do Vendedor] varchar(30) NOT NULL DEFAULT '',
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Observacoes] text NOT NULL DEFAULT '',
    [Codigo do Pedido] int NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Total dos Produtos] decimal(10,2) NOT NULL,
    [Total de Icms] decimal(10,2) NOT NULL,
    [Total de Icms St] decimal(10,2) NOT NULL,
    [Total de Ipi] decimal(10,2) NOT NULL,
    [Total de Despesas] decimal(10,2) NOT NULL,
    [Total da Licitação] decimal(10,2) NOT NULL,
    [Cancelado] bit NOT NULL,
    [Fechado] bit NOT NULL,
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [IPI na Bc Frete] bit NOT NULL,
    CONSTRAINT [Codigo da Licitação] PRIMARY KEY ([Codigo da Licitação]),
    CONSTRAINT [TB_Nova_Licitação_FK_Sequencia_da_Transportadora] FOREIGN KEY ([Sequencia da Transportadora]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Nova_Licitação_FK_Sequencia_do_Fornecedor] FOREIGN KEY ([Sequencia do Fornecedor]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Orçamento] (
    [Seqüência do Orçamento] int NOT NULL IDENTITY,
    [Data de Emissão] datetime NULL,
    [Seqüência do Geral] int NOT NULL,
    [Seqüência da Propriedade] smallint NOT NULL,
    [Observacao] text NOT NULL DEFAULT '',
    [Fechamento] smallint NOT NULL,
    [Valor do Fechamento] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total do ICMS] decimal(11,2) NOT NULL,
    [Valor Total dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total de Produtos Usados] decimal(11,2) NOT NULL,
    [Valor Total Conjuntos Usados] decimal(11,2) NOT NULL,
    [Valor Total dos Serviços] decimal(11,2) NOT NULL,
    [Valor Total do Orçamento] decimal(11,2) NOT NULL,
    [Nome Cliente] varchar(60) NOT NULL DEFAULT '',
    [Endereco] varchar(100) NOT NULL DEFAULT '',
    [Número do Endereço] varchar(10) NOT NULL DEFAULT '',
    [Seqüência do Município] int NOT NULL,
    [CEP] varchar(9) NOT NULL DEFAULT '',
    [Telefone] varchar(14) NOT NULL DEFAULT '',
    [Fax] varchar(14) NOT NULL DEFAULT '',
    [Email] varchar(255) NOT NULL DEFAULT '',
    [Seqüência do Vendedor] int NOT NULL,
    [Seqüência do Pedido] int NOT NULL,
    [Tipo] smallint NOT NULL,
    [CPF e CNPJ] varchar(20) NOT NULL DEFAULT '',
    [RG e IE] varchar(20) NOT NULL DEFAULT '',
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Ocultar Valor Unitário] bit NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [Valor Total IPI das Peças] decimal(11,2) NOT NULL,
    [Valor Total das Peças] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor Total das Peças Usadas] decimal(11,2) NOT NULL,
    [Complemento] varchar(100) NOT NULL DEFAULT '',
    [Bairro] varchar(50) NOT NULL DEFAULT '',
    [Caixa Postal] varchar(30) NOT NULL DEFAULT '',
    [É Propriedade] bit NOT NULL,
    [Nome da Propriedade] varchar(62) NOT NULL DEFAULT '',
    [Valor Total da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do Seguro] decimal(11,2) NOT NULL,
    [Data do Fechamento] datetime NULL,
    [Cancelado] bit NOT NULL,
    [Código do Suframa] varchar(9) NOT NULL DEFAULT '',
    [Revenda] bit NOT NULL,
    [Valor Total do PIS] decimal(11,2) NOT NULL,
    [Valor Total do COFINS] decimal(11,2) NOT NULL,
    [Valor Total da Base ST] decimal(11,2) NOT NULL,
    [Valor Total do ICMS ST] decimal(11,2) NOT NULL,
    [Alíquota do ISS] decimal(5,2) NOT NULL,
    [Reter ISS] bit NOT NULL,
    [Entrega Futura] bit NOT NULL,
    [Seqüência da Transportadora] int NOT NULL,
    [Data da Alteração] datetime NULL,
    [Hora da Alteração] datetime NULL,
    [Usuário da Alteração] varchar(60) NOT NULL DEFAULT '',
    [Venda Fechada] bit NOT NULL,
    [Orçamento Avulso] bit NOT NULL,
    [Valor do Imposto de Renda] decimal(11,2) NOT NULL,
    [Fatura Proforma] bit NOT NULL,
    [Local de Embarque] varchar(30) NOT NULL DEFAULT '',
    [UF de Embarque] varchar(3) NOT NULL DEFAULT '',
    [Número da Proforma] int NOT NULL,
    [Seqüência do País] int NOT NULL,
    [Valor Total do Tributo] decimal(11,2) NOT NULL,
    [Conjunto Avulso] bit NOT NULL,
    [Descrição Conjunto Avulso] varchar(60) NOT NULL DEFAULT '',
    [Vendedor Intermediario] varchar(40) NOT NULL DEFAULT '',
    [Percentual do Vendedor] decimal(8,4) NOT NULL,
    [Rebiut] varchar(40) NOT NULL DEFAULT '',
    [Percentual Rebiut] decimal(8,4) NOT NULL,
    [Nao Movimentar Estoque] bit NOT NULL,
    [Gerou Encargos] bit NOT NULL,
    [Hidroturbo] varchar(100) NOT NULL DEFAULT '',
    [Area irrigada] decimal(8,2) NOT NULL,
    [Precipitação bruta] decimal(8,2) NOT NULL,
    [Horas irrigada] decimal(7,2) NOT NULL,
    [Area tot irrigada em] decimal(7,2) NOT NULL,
    [Aspersor] varchar(20) NOT NULL DEFAULT '',
    [Modelo do aspersor] varchar(40) NOT NULL DEFAULT '',
    [Bocal diametro] decimal(7,2) NOT NULL,
    [Pressão de serviço] decimal(7,2) NOT NULL,
    [Alcance do jato] decimal(7,2) NOT NULL,
    [Espaço entre carreadores] decimal(7,2) NOT NULL,
    [Faixa irrigada] decimal(7,2) NOT NULL,
    [Desnivel maximo na area] decimal(7,2) NOT NULL,
    [Altura de sucção] decimal(7,2) NOT NULL,
    [Altura do aspersor] decimal(5,2) NOT NULL,
    [Tempo parado antes percurso] decimal(5,2) NOT NULL,
    [Com 1] decimal(8,2) NOT NULL,
    [Com 2] decimal(8,2) NOT NULL,
    [Com 3] decimal(8,2) NOT NULL,
    [Modelo Trecho A] int NOT NULL,
    [Modelo Trecho B] int NOT NULL,
    [Modelo Trecho C] int NOT NULL,
    [Qtde bomba] smallint NOT NULL,
    [Marca bomba] varchar(25) NOT NULL DEFAULT '',
    [Modelo bomba] varchar(25) NOT NULL DEFAULT '',
    [Tamanho bomba] varchar(20) NOT NULL DEFAULT '',
    [N estagios] smallint NOT NULL,
    [Diametro bomba] decimal(8,2) NOT NULL,
    [Pressao bomba] decimal(8,2) NOT NULL,
    [Rendimento bomba] decimal(8,2) NOT NULL,
    [Rotação bomba] decimal(8,2) NOT NULL,
    [Qtde de Motor] decimal(10,2) NOT NULL,
    [Marca do Motor] varchar(20) NOT NULL DEFAULT '',
    [Modelo Motor] varchar(25) NOT NULL DEFAULT '',
    [Nivel de Proteção] varchar(15) NOT NULL DEFAULT '',
    [Potencia Nominal] decimal(8,2) NOT NULL,
    [Nro de Fases] smallint NOT NULL,
    [Voltagem] decimal(8,2) NOT NULL,
    [Modelo hidroturbo] varchar(30) NOT NULL DEFAULT '',
    [Eixos] smallint NOT NULL,
    [Rodas] smallint NOT NULL,
    [Pneus] varchar(20) NOT NULL DEFAULT '',
    [Tubos] varchar(25) NOT NULL DEFAULT '',
    [Projetista] int NOT NULL,
    [Peso Bruto] decimal(11,2) NOT NULL,
    [Peso Líquido] decimal(11,2) NOT NULL,
    [Volumes] int NOT NULL,
    [Aviso de embarque] varchar(100) NOT NULL DEFAULT '',
    [Entrega Tecnica] varchar(19) NOT NULL DEFAULT '',
    [Sequencia do Projeto] int NOT NULL,
    [Outras Despesas] decimal(10,2) NOT NULL,
    [Refaturamento] bit NOT NULL,
    [Data do Pedido] datetime NULL,
    [Data de Entrega] datetime NULL,
    [Ordem Interna] bit NOT NULL,
    [Orçamento Vinculado] int NOT NULL,
    [Frete] varchar(35) NULL DEFAULT '',
    [PlacaVeiculo] varchar(8) NULL,
    [UfPlaca] char(2) NULL,
    [NumAntt] char(8) NULL,
    [Total CBS] float NULL DEFAULT 0.0E0,
    [Total IBS] float NULL DEFAULT 0.0E0,
    CONSTRAINT [Seqüência do Orçamento] PRIMARY KEY ([Seqüência do Orçamento]),
    CONSTRAINT [TB_Orçamento_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Orçamento_FK_Seqüência_da_Propriedade] FOREIGN KEY ([Seqüência da Propriedade]) REFERENCES [Propriedades] ([Seqüência da Propriedade]),
    CONSTRAINT [TB_Orçamento_FK_Seqüência_da_Transportadora] FOREIGN KEY ([Seqüência da Transportadora]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Orçamento_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Orçamento_FK_Seqüência_do_Município] FOREIGN KEY ([Seqüência do Município]) REFERENCES [Municipios] ([Seqüência do Município]),
    CONSTRAINT [TB_Orçamento_FK_Seqüência_do_País] FOREIGN KEY ([Seqüência do País]) REFERENCES [Paises] ([Seqüência do País]),
    CONSTRAINT [TB_Orçamento_FK_Seqüência_do_Vendedor] FOREIGN KEY ([Seqüência do Vendedor]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Ordem de Serviço] (
    [Seqüência da Ordem de Serviço] int NOT NULL IDENTITY,
    [Data de Emissão] datetime NULL,
    [Seqüência do Geral] int NOT NULL,
    [Seqüência da Propriedade] smallint NOT NULL,
    [Data do Fechamento] datetime NULL,
    [Validade] smallint NOT NULL,
    [Fechamento] smallint NOT NULL,
    [Observacao] text NOT NULL DEFAULT '',
    [Valor do Fechamento] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total do ICMS] decimal(11,2) NOT NULL,
    [Valor Total dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total de Produtos Usados] decimal(11,2) NOT NULL,
    [Valor Total Conjuntos Usados] decimal(11,2) NOT NULL,
    [Valor Total dos Serviços] decimal(11,2) NOT NULL,
    [Valor Total Ordem de Serviço] decimal(11,2) NOT NULL,
    [Nome Cliente] varchar(60) NOT NULL DEFAULT '',
    [É Propriedade] bit NOT NULL,
    [Nome da Propriedade] varchar(62) NOT NULL DEFAULT '',
    [Endereco] varchar(100) NOT NULL DEFAULT '',
    [Complemento] varchar(100) NOT NULL DEFAULT '',
    [Número do Endereço] varchar(10) NOT NULL DEFAULT '',
    [Bairro] varchar(50) NOT NULL DEFAULT '',
    [Caixa Postal] varchar(30) NOT NULL DEFAULT '',
    [Seqüência do Município] int NOT NULL,
    [CEP] varchar(9) NOT NULL DEFAULT '',
    [Telefone] varchar(14) NOT NULL DEFAULT '',
    [Fax] varchar(14) NOT NULL DEFAULT '',
    [Email] varchar(255) NOT NULL DEFAULT '',
    [Seqüência do Vendedor] int NOT NULL,
    [Tipo] smallint NOT NULL,
    [CPF e CNPJ] varchar(20) NOT NULL DEFAULT '',
    [RG e IE] varchar(20) NOT NULL DEFAULT '',
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Serviço em Garantia] bit NOT NULL,
    [Tipo do Relatório] varchar(10) NOT NULL DEFAULT '',
    [Modelo do Trator] varchar(15) NOT NULL DEFAULT '',
    [Ano de Fabricação do Trator] smallint NOT NULL,
    [Número do Motor do Trator] varchar(12) NOT NULL DEFAULT '',
    [Número do Chassi do Trator] varchar(10) NOT NULL DEFAULT '',
    [Horímetro do Trator] decimal(7,2) NOT NULL,
    [Cor do Trator] varchar(25) NOT NULL DEFAULT '',
    [Kilometragem do Trator] decimal(6,1) NOT NULL,
    [Valor Km Rodado do Trator] decimal(11,2) NOT NULL,
    [Seqüência do Pedido] int NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [Valor Total das Peças] decimal(11,2) NOT NULL,
    [Valor Total IPI das Peças] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor Total das Peças Usadas] decimal(11,2) NOT NULL,
    [Valor Total da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do Seguro] decimal(11,2) NOT NULL,
    [Cancelado] bit NOT NULL,
    [Código do Suframa] varchar(9) NOT NULL DEFAULT '',
    [Revenda] bit NOT NULL,
    [Gerar Pedido] bit NOT NULL,
    [Valor Total do PIS] decimal(11,2) NOT NULL,
    [Valor Total do COFINS] decimal(11,2) NOT NULL,
    [Valor Total da Base ST] decimal(11,2) NOT NULL,
    [Valor Total do ICMS ST] decimal(11,2) NOT NULL,
    [Alíquota do ISS] decimal(5,2) NOT NULL,
    [Reter ISS] bit NOT NULL,
    CONSTRAINT [Seqüência da Ordem de Serviço] PRIMARY KEY ([Seqüência da Ordem de Serviço]),
    CONSTRAINT [TB_Ordem_de_Serviço_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Ordem_de_Serviço_FK_Seqüência_da_Propriedade] FOREIGN KEY ([Seqüência da Propriedade]) REFERENCES [Propriedades] ([Seqüência da Propriedade]),
    CONSTRAINT [TB_Ordem_de_Serviço_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Ordem_de_Serviço_FK_Seqüência_do_Município] FOREIGN KEY ([Seqüência do Município]) REFERENCES [Municipios] ([Seqüência do Município]),
    CONSTRAINT [TB_Ordem_de_Serviço_FK_Seqüência_do_Vendedor] FOREIGN KEY ([Seqüência do Vendedor]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Propriedades do Geral] (
    [Seqüência da Propriedade Geral] int NOT NULL IDENTITY,
    [Seqüência do Geral] int NOT NULL,
    [Seqüência da Propriedade] smallint NOT NULL,
    [Inativo] bit NOT NULL,
    CONSTRAINT [Seqüência da Propriedade Geral] PRIMARY KEY ([Seqüência da Propriedade Geral]),
    CONSTRAINT [TB_Propriedades_do_Geral_FK_Seqüência_da_Propriedade] FOREIGN KEY ([Seqüência da Propriedade]) REFERENCES [Propriedades] ([Seqüência da Propriedade]),
    CONSTRAINT [TB_Propriedades_do_Geral_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Relatorio de Viagem] (
    [Seq da Viagem] int NOT NULL IDENTITY,
    [Sequencia do Geral] int NOT NULL,
    [Periodo Inicial] datetime NULL,
    [Periodo Final] datetime NULL,
    [Destino da Viagem] varchar(255) NOT NULL DEFAULT '',
    [Motivo da Viagem] text NOT NULL DEFAULT '',
    [Total da Viagem] decimal(10,2) NOT NULL,
    [Total dos Itens] decimal(10,2) NOT NULL,
    [Referencia] varchar(20) NOT NULL DEFAULT '',
    [Teste] varchar(3) NOT NULL DEFAULT '',
    CONSTRAINT [Seq da Viagem] PRIMARY KEY ([Seq da Viagem]),
    CONSTRAINT [TB_Relatorio_de_Viagem_FK_Sequencia_do_Geral] FOREIGN KEY ([Sequencia do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Requisição] (
    [Seqüência da Requisição] int NOT NULL IDENTITY,
    [Data da Requisição] datetime NULL,
    [Seqüência do Geral] int NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Tipo do Desconto] smallint NOT NULL,
    [Valor Total da Requisição] decimal(11,2) NOT NULL,
    CONSTRAINT [Seqüência da Requisição] PRIMARY KEY ([Seqüência da Requisição]),
    CONSTRAINT [TB_Requisição_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Consumo do Pedido Compra] (
    [Id do Pedido] int NOT NULL,
    [Id Despesa] int NOT NULL,
    [Id da Despesa] int NOT NULL,
    [Qtde] decimal(10,2) NOT NULL,
    [Vr Unitario] decimal(10,2) NOT NULL,
    [Vr Total] decimal(10,2) NOT NULL,
    [Aliquota do IPI] decimal(8,4) NOT NULL,
    [Aliquota do Icms] smallint NOT NULL,
    [Vr do IPI] decimal(10,2) NOT NULL,
    [Vr do Icms] decimal(10,2) NOT NULL,
    CONSTRAINT [Id Consumo] PRIMARY KEY ([Id do Pedido], [Id Despesa]),
    CONSTRAINT [TB_Consumo_do_Pedido_Compra_FK_Id_da_Despesa] FOREIGN KEY ([Id da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa])
);
GO

CREATE TABLE [Despesas da Licitação] (
    [Codigo da Licitação] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Sequencia da Despesa] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    CONSTRAINT [Cod_e_ItemD] PRIMARY KEY ([Codigo da Licitação], [Sequencia do Item]),
    CONSTRAINT [TB_Despesas_da_Licitação_FK_Sequencia_da_Despesa] FOREIGN KEY ([Sequencia da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa])
);
GO

CREATE TABLE [Despesas do Movimento Contábil] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência da Despesa Movimento] int NOT NULL IDENTITY,
    [Seqüência da Despesa] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor da Substituição] decimal(12,4) NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Desp Mvto Cont] PRIMARY KEY ([Seqüência do Movimento], [Seqüência da Despesa Movimento]),
    CONSTRAINT [TB_Despesas_do_Movimento_Contábil_FK_Seqüência_da_Despesa] FOREIGN KEY ([Seqüência da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa])
);
GO

CREATE TABLE [Despesas do Novo Pedido] (
    [Codigo do Pedido] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Sequencia da Despesa] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    CONSTRAINT [Codig_e_Item] PRIMARY KEY ([Codigo do Pedido], [Sequencia do Item]),
    CONSTRAINT [TB_Despesas_do_Novo_Pedido_FK_Sequencia_da_Despesa] FOREIGN KEY ([Sequencia da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa])
);
GO

CREATE TABLE [Despesas do Pedido Compra] (
    [Id do Pedido] int NOT NULL,
    [Id da Despesa] int NOT NULL,
    [Qtde] decimal(10,2) NOT NULL,
    [Vr Unitario] decimal(10,2) NOT NULL,
    [Vr Total] decimal(10,2) NOT NULL,
    [Aliquota do IPI] decimal(8,4) NOT NULL,
    [Aliquota do Icms] smallint NOT NULL,
    [Vr do IPI] decimal(10,2) NOT NULL,
    [Vr do Icms] decimal(10,2) NOT NULL,
    CONSTRAINT [Id Despesa] PRIMARY KEY ([Id do Pedido], [Id da Despesa]),
    CONSTRAINT [TB_Despesas_do_Pedido_Compra_FK_Id_da_Despesa] FOREIGN KEY ([Id da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa])
);
GO

CREATE TABLE [Despesas Mvto Contábil Novo] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência Despesa Mvto Novo] int NOT NULL IDENTITY,
    [Seqüência da Despesa] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor da Substituição] decimal(12,4) NOT NULL,
    [Valor do ipi compra] decimal(12,4) NOT NULL,
    [Valor total compra] decimal(12,4) NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Desp Cont Novo] PRIMARY KEY ([Seqüência do Movimento], [Seqüência Despesa Mvto Novo]),
    CONSTRAINT [TB_Despesas_Mvto_Contábil_Novo_FK_Seqüência_da_Despesa] FOREIGN KEY ([Seqüência da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa])
);
GO

CREATE TABLE [Conjuntos Mvto Contábil Novo] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência Conjunto Mvto Novo] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor da Substituição] decimal(12,4) NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Conj Novo] PRIMARY KEY ([Seqüência do Movimento], [Seqüência Conjunto Mvto Novo]),
    CONSTRAINT [TB_Conjuntos_Mvto_Contábil_Novo_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto])
);
GO

CREATE TABLE [Baixa do Estoque Contábil] (
    [Seqüência da Baixa] int NOT NULL IDENTITY,
    [Tipo do Movimento] smallint NOT NULL,
    [Data do Movimento] datetime NULL,
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Seqüência do Geral] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Observacao] text NOT NULL DEFAULT '',
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor da Substituição] decimal(12,4) NOT NULL,
    [Tipo do Produto] smallint NOT NULL,
    [Seqüência do Conjunto] int NOT NULL,
    [Estoque] varchar(1) NOT NULL DEFAULT '',
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência do Item 2] int NOT NULL,
    [Seqüência da Despesa] int NOT NULL,
    CONSTRAINT [Seqüência da Baixa Estoque] PRIMARY KEY ([Seqüência da Baixa]),
    CONSTRAINT [TB_Baixa_do_Estoque_Contábil_FK_Seqüência_da_Despesa] FOREIGN KEY ([Seqüência da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa]),
    CONSTRAINT [TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto]),
    CONSTRAINT [TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Baixa Industrialização] (
    [Seqüência da Baixa] int NOT NULL IDENTITY,
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência do Item] smallint NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    CONSTRAINT [Seqüência da Bx] PRIMARY KEY ([Seqüência da Baixa]),
    CONSTRAINT [TB_Baixa_Industrialização_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Baixa MP Produto] (
    [Seqüência da Baixa] int NOT NULL IDENTITY,
    [Seqüência do Movimento] int NOT NULL,
    [Data da Baixa] datetime NULL,
    [Hora da Baixa] datetime NULL,
    [Seqüência do Item] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade do Produto] decimal(9,3) NOT NULL,
    [Seqüência da Matéria Prima] int NOT NULL,
    [Quantidade da Matéria Prima] decimal(9,3) NOT NULL,
    [Calcular Estoque] bit NOT NULL,
    CONSTRAINT [Seq Baixa MP] PRIMARY KEY ([Seqüência da Baixa]),
    CONSTRAINT [TB_Baixa_MP_Produto_FK_Seqüência_da_Matéria_Prima] FOREIGN KEY ([Seqüência da Matéria Prima]) REFERENCES [Produtos] ([Seqüência do Produto]),
    CONSTRAINT [TB_Baixa_MP_Produto_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Importação Produtos Estoque] (
    [Seqüência Importação Estoque] int NOT NULL,
    [Seqüência Importação Ítem] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    CONSTRAINT [Sq Importação Estoque Seq Prod] PRIMARY KEY ([Seqüência Importação Estoque], [Seqüência Importação Ítem]),
    CONSTRAINT [TB_Importação_Produtos_Estoque_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Itens da Licitacao] (
    [Sequencia] int NOT NULL,
    [Produto] int NOT NULL,
    [Cod Despesa] int NOT NULL,
    [Sequencial de Um] int NOT NULL,
    [Unidade] varchar(20) NOT NULL DEFAULT '',
    [Qtde 1] decimal(10,2) NOT NULL,
    [Vr Unit 1] decimal(10,2) NOT NULL,
    [Vr Total 1] decimal(10,2) NOT NULL,
    [Qtde 2] decimal(10,2) NOT NULL,
    [Vr Unit 2] decimal(10,2) NOT NULL,
    [Vr Total 2] decimal(10,2) NOT NULL,
    [Qtde 3] decimal(10,2) NOT NULL,
    [Vr Unit 3] decimal(10,2) NOT NULL,
    [Vr Total 3] decimal(10,2) NOT NULL,
    CONSTRAINT [Sequencia] PRIMARY KEY ([Sequencia], [Produto], [Cod Despesa], [Sequencial de Um]),
    CONSTRAINT [TB_Itens_da_Licitacao_FK_Produto] FOREIGN KEY ([Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Itens do Conjunto] (
    [Seqüência do Conjunto] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade do Produto] decimal(9,3) NOT NULL,
    [Peso do Item] decimal(12,3) NOT NULL,
    [Peso Total] decimal(12,3) NOT NULL,
    CONSTRAINT [Seqüência do Item do Conjunto] PRIMARY KEY ([Seqüência do Conjunto], [Seqüência do Produto]),
    CONSTRAINT [TB_Itens_do_Conjunto_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto]) ON DELETE CASCADE,
    CONSTRAINT [TB_Itens_do_Conjunto_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Matéria Prima] (
    [Seqüência da Matéria Prima] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade de Matéria Prima] decimal(9,3) NOT NULL,
    CONSTRAINT [Seq Matéria Prima e Seq Prod] PRIMARY KEY ([Seqüência da Matéria Prima], [Seqüência do Produto]),
    CONSTRAINT [TB_Matéria_Prima_FK_Seqüência_da_Matéria_Prima] FOREIGN KEY ([Seqüência da Matéria Prima]) REFERENCES [Produtos] ([Seqüência do Produto]),
    CONSTRAINT [TB_Matéria_Prima_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto]) ON DELETE CASCADE
);
GO

CREATE TABLE [Peças do Movimento Estoque] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência da Peça Movimento] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do ICMS ST] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Peça] PRIMARY KEY ([Seqüência do Movimento], [Seqüência da Peça Movimento]),
    CONSTRAINT [TB_Peças_do_Movimento_Estoque_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Produtos da Licitação] (
    [Codigo da Licitação] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Sequencia do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(8,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    CONSTRAINT [CodItem] PRIMARY KEY ([Codigo da Licitação], [Sequencia do Item]),
    CONSTRAINT [TB_Produtos_da_Licitação_FK_Sequencia_do_Produto] FOREIGN KEY ([Sequencia do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Produtos do Novo Pedido] (
    [Codigo do Pedido] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Sequencia do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    CONSTRAINT [CodPed_e_Item] PRIMARY KEY ([Codigo do Pedido], [Sequencia do Item]),
    CONSTRAINT [TB_Produtos_do_Novo_Pedido_FK_Sequencia_do_Produto] FOREIGN KEY ([Sequencia do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Produtos do Pedido Compra] (
    [Id do Pedido] int NOT NULL,
    [Id do Produto] int NOT NULL,
    [Sequencia do Item] int NOT NULL,
    [Qtde] decimal(10,2) NOT NULL,
    [Vr Unitario] decimal(11,4) NOT NULL,
    [Vr Total] decimal(10,2) NOT NULL,
    [Aliquota do IPI] decimal(8,4) NOT NULL,
    [Aliquota do Icms] decimal(7,4) NOT NULL,
    [Vr do IPI] decimal(10,2) NOT NULL,
    [Vr do Icms] decimal(10,2) NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    CONSTRAINT [ItesProdutos] PRIMARY KEY ([Id do Pedido], [Id do Produto], [Sequencia do Item]),
    CONSTRAINT [TB_Produtos_do_Pedido_Compra_FK_Id_do_Produto] FOREIGN KEY ([Id do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Produtos Mvto Contábil Novo] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência do Produto Mvto Novo] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor da Substituição] decimal(12,4) NOT NULL,
    [Valor do ipi compra] decimal(12,4) NOT NULL,
    [Valor total compra] decimal(12,4) NOT NULL,
    [Sequencia Unidade Speed] int NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Prod Mvto Novo] PRIMARY KEY ([Seqüência do Movimento], [Seqüência do Produto Mvto Novo]),
    CONSTRAINT [TB_Produtos_Mvto_Contábil_Novo_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Transferência de Receita] (
    [Seqüência da Transferência] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Seqüência da Unidade] smallint NOT NULL,
    [Localizacao] varchar(50) NOT NULL DEFAULT '',
    [Seqüência do Grupo Produto] smallint NOT NULL,
    [Seqüência do SubGrupo Produto] smallint NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [SeqüênciaDoSubGrupoProduto] smallint NOT NULL,
    [SeqüênciaDoGrupoProduto] smallint NOT NULL,
    CONSTRAINT [Seqüência da Transferência] PRIMARY KEY ([Seqüência da Transferência]),
    CONSTRAINT [TB_Transferência_de_Receita_FK_Seqüência_da_Unidade] FOREIGN KEY ([Seqüência da Unidade]) REFERENCES [Unidades] ([Seqüência da Unidade]),
    CONSTRAINT [TB_Transferência_de_Receita_FK_Seqüência_do_Grupo_Produto] FOREIGN KEY ([Seqüência do Grupo Produto]) REFERENCES [Grupo do Produto] ([Seqüência do Grupo Produto]),
    CONSTRAINT [TB_Transferência_de_Receita_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto]),
    CONSTRAINT [TB_Transferência_de_Receita_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto] FOREIGN KEY ([SeqüênciaDoSubGrupoProduto], [SeqüênciaDoGrupoProduto]) REFERENCES [SubGrupo do Produto] ([Seqüência do SubGrupo Produto], [Seqüência do Grupo Produto])
);
GO

CREATE TABLE [Adições da Declaração] (
    [Seqüência da Adição] int NOT NULL IDENTITY,
    [Seqüência da Declaração] int NOT NULL,
    [Número da Adição] smallint NOT NULL,
    [Seqüêncial do Item da Adição] smallint NOT NULL,
    [Seqüência do Geral] int NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    CONSTRAINT [Seqüência da Adição] PRIMARY KEY ([Seqüência da Adição]),
    CONSTRAINT [TB_Adições_da_Declaração_FK_Seqüência_da_Declaração] FOREIGN KEY ([Seqüência da Declaração]) REFERENCES [Declarações de Importação] ([Seqüência da Declaração]) ON DELETE CASCADE,
    CONSTRAINT [TB_Adições_da_Declaração_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Parcelas Entrada Contas] (
    [Número da Parcela] smallint NOT NULL,
    [Seqüência da Entrada] int NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Seqüência da Cobrança] smallint NOT NULL,
    CONSTRAINT [Num Parcela e Seq da Entrada] PRIMARY KEY ([Número da Parcela], [Seqüência da Entrada]),
    CONSTRAINT [TB_Parcelas_Entrada_Contas_FK_Seqüência_da_Entrada] FOREIGN KEY ([Seqüência da Entrada]) REFERENCES [Entrada Contas] ([Seqüência da Entrada]) ON DELETE CASCADE
);
GO

CREATE TABLE [Conjuntos do Movimento Estoque] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência Conjunto Movimento] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Porcentagem do IPI] decimal(8,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do ICMS ST] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Movimento e Seq Conjunto] PRIMARY KEY ([Seqüência do Movimento], [Seqüência Conjunto Movimento]),
    CONSTRAINT [TB_Conjuntos_do_Movimento_Estoque_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto]),
    CONSTRAINT [TB_Conjuntos_do_Movimento_Estoque_FK_Seqüência_do_Movimento] FOREIGN KEY ([Seqüência do Movimento]) REFERENCES [Movimento do Estoque] ([Seqüência do Movimento]) ON DELETE CASCADE
);
GO

CREATE TABLE [Despesas do Movimento Estoque] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência da Despesa Movimento] int NOT NULL IDENTITY,
    [Seqüência da Despesa] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Porcentagem de IPI] decimal(8,4) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [Local Usado] varchar(50) NOT NULL DEFAULT '',
    CONSTRAINT [Seq Mvto e Seq Despesa Mvto] PRIMARY KEY ([Seqüência do Movimento], [Seqüência da Despesa Movimento]),
    CONSTRAINT [TB_Despesas_do_Movimento_Estoque_FK_Seqüência_da_Despesa] FOREIGN KEY ([Seqüência da Despesa]) REFERENCES [Despesas] ([Seqüência da Despesa]),
    CONSTRAINT [TB_Despesas_do_Movimento_Estoque_FK_Seqüência_do_Movimento] FOREIGN KEY ([Seqüência do Movimento]) REFERENCES [Movimento do Estoque] ([Seqüência do Movimento]) ON DELETE CASCADE
);
GO

CREATE TABLE [Parcelas Movimento Estoque] (
    [Seqüência do Movimento] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Seqüência da Cobrança] smallint NOT NULL,
    CONSTRAINT [Seq Movimento e Pc] PRIMARY KEY ([Número da Parcela], [Seqüência do Movimento]),
    CONSTRAINT [TB_Parcelas_Movimento_Estoque_FK_Seqüência_do_Movimento] FOREIGN KEY ([Seqüência do Movimento]) REFERENCES [Movimento do Estoque] ([Seqüência do Movimento]) ON DELETE CASCADE
);
GO

CREATE TABLE [Produtos do Movimento Estoque] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência do Produto Movimento] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Porcentagem de IPI] decimal(8,4) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor Unitário com Impostos] decimal(11,4) NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Prod] PRIMARY KEY ([Seqüência do Movimento], [Seqüência do Produto Movimento]),
    CONSTRAINT [TB_Produtos_do_Movimento_Estoque_FK_Seqüência_do_Movimento] FOREIGN KEY ([Seqüência do Movimento]) REFERENCES [Movimento do Estoque] ([Seqüência do Movimento]) ON DELETE CASCADE,
    CONSTRAINT [TB_Produtos_do_Movimento_Estoque_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Conjuntos Movimento Contábil] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência Conjunto Movimento] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor da Substituição] decimal(12,4) NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Conjunto] PRIMARY KEY ([Seqüência do Movimento], [Seqüência Conjunto Movimento]),
    CONSTRAINT [TB_Conjuntos_Movimento_Contábil_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto]),
    CONSTRAINT [TB_Conjuntos_Movimento_Contábil_FK_Seqüência_do_Movimento] FOREIGN KEY ([Seqüência do Movimento]) REFERENCES [Movimento do Estoque Contábil] ([Seqüência do Movimento]) ON DELETE CASCADE
);
GO

CREATE TABLE [Produtos do Movimento Contábil] (
    [Seqüência do Movimento] int NOT NULL,
    [Seqüência do Produto Movimento] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor de Custo] decimal(12,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor da Substituição] decimal(12,4) NOT NULL,
    CONSTRAINT [Seq Mvto e Seq Produto] PRIMARY KEY ([Seqüência do Movimento], [Seqüência do Produto Movimento]),
    CONSTRAINT [TB_Produtos_do_Movimento_Contábil_FK_Seqüência_do_Movimento] FOREIGN KEY ([Seqüência do Movimento]) REFERENCES [Movimento do Estoque Contábil] ([Seqüência do Movimento]) ON DELETE CASCADE,
    CONSTRAINT [TB_Produtos_do_Movimento_Contábil_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Conjuntos do Orçamento] (
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência Conjunto Orçamento] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(8,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(6,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(8,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    [Valor Anterior] decimal(12,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Bc pis] decimal(9,2) NOT NULL,
    [Aliq do pis] decimal(5,2) NOT NULL,
    [Bc cofins] decimal(9,2) NOT NULL,
    [Aliq do cofins] decimal(5,2) NOT NULL,
    [Valor Do CBS] float NULL DEFAULT 0.0E0,
    [Valor Do IBS] float NULL DEFAULT 0.0E0,
    CONSTRAINT [Seq Orçamento e Seq Conj] PRIMARY KEY ([Seqüência do Orçamento], [Seqüência Conjunto Orçamento]),
    CONSTRAINT [TB_Conjuntos_do_Orçamento_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto]),
    CONSTRAINT [TB_Conjuntos_do_Orçamento_FK_Seqüência_do_Orçamento] FOREIGN KEY ([Seqüência do Orçamento]) REFERENCES [Orçamento] ([Seqüência do Orçamento]) ON DELETE CASCADE
);
GO

CREATE TABLE [Parcelas Orçamento] (
    [Seqüência do Orçamento] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Descrição da Cobrança] varchar(120) NOT NULL DEFAULT '',
    CONSTRAINT [Seq Orçamento e PC] PRIMARY KEY ([Seqüência do Orçamento], [Número da Parcela]),
    CONSTRAINT [TB_Parcelas_Orçamento_FK_Seqüência_do_Orçamento] FOREIGN KEY ([Seqüência do Orçamento]) REFERENCES [Orçamento] ([Seqüência do Orçamento]) ON DELETE CASCADE
);
GO

CREATE TABLE [Peças do Orçamento] (
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência Peças do Orçamento] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(8,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(6,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(8,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    [Valor Anterior] decimal(12,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Bc pis] decimal(9,2) NOT NULL,
    [Aliq do pis] decimal(5,2) NOT NULL,
    [Bc cofins] decimal(9,2) NOT NULL,
    [Aliq do cofins] decimal(5,2) NOT NULL,
    [Valor Do CBS] float NULL DEFAULT 0.0E0,
    [Valor Do IBS] float NULL DEFAULT 0.0E0,
    CONSTRAINT [Seq Orçamento e Seq Peças] PRIMARY KEY ([Seqüência do Orçamento], [Seqüência Peças do Orçamento]),
    CONSTRAINT [TB_Peças_do_Orçamento_FK_Seqüência_do_Orçamento] FOREIGN KEY ([Seqüência do Orçamento]) REFERENCES [Orçamento] ([Seqüência do Orçamento]) ON DELETE CASCADE,
    CONSTRAINT [TB_Peças_do_Orçamento_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Produtos do Orçamento] (
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência do Produto Orçamento] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(8,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(6,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(8,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    [Valor Anterior] decimal(12,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Bc pis] decimal(9,2) NOT NULL,
    [Bc cofins] decimal(9,2) NOT NULL,
    [Aliq do pis] decimal(5,2) NOT NULL,
    [Aliq do cofins] decimal(5,2) NOT NULL,
    [Valor Do CBS] float NULL DEFAULT 0.0E0,
    [Valor Do IBS] float NULL DEFAULT 0.0E0,
    CONSTRAINT [Seq Orçamento e Seq Prod] PRIMARY KEY ([Seqüência do Orçamento], [Seqüência do Produto Orçamento]),
    CONSTRAINT [TB_Produtos_do_Orçamento_FK_Seqüência_do_Orçamento] FOREIGN KEY ([Seqüência do Orçamento]) REFERENCES [Orçamento] ([Seqüência do Orçamento]) ON DELETE CASCADE,
    CONSTRAINT [TB_Produtos_do_Orçamento_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Serviços do Orçamento] (
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência do Serviço Orçamento] int NOT NULL IDENTITY,
    [Seqüência do Serviço] smallint NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor Anterior] decimal(12,2) NOT NULL,
    CONSTRAINT [Seq Orçamento e Seq Serv] PRIMARY KEY ([Seqüência do Orçamento], [Seqüência do Serviço Orçamento]),
    CONSTRAINT [TB_Serviços_do_Orçamento_FK_Seqüência_do_Orçamento] FOREIGN KEY ([Seqüência do Orçamento]) REFERENCES [Orçamento] ([Seqüência do Orçamento]) ON DELETE CASCADE,
    CONSTRAINT [TB_Serviços_do_Orçamento_FK_Seqüência_do_Serviço] FOREIGN KEY ([Seqüência do Serviço]) REFERENCES [Servicos] ([Seqüência do Serviço])
);
GO

CREATE TABLE [VinculaPedidoOrcamento] (
    [ID_Vinculacao] int NOT NULL IDENTITY,
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência do Geral] int NOT NULL,
    [Id do Pedido] int NOT NULL,
    [Seqüência do Produto] int NOT NULL,
    [Qtde] decimal(10,2) NOT NULL,
    CONSTRAINT [PK__VinculaP__6717027AB8E1433B] PRIMARY KEY ([ID_Vinculacao]),
    CONSTRAINT [FK_VinculaPedidoOrcamento_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [FK_VinculaPedidoOrcamento_Orcamento] FOREIGN KEY ([Seqüência do Orçamento]) REFERENCES [Orçamento] ([Seqüência do Orçamento]),
    CONSTRAINT [FK_VinculaPedidoOrcamento_PedidoCompra] FOREIGN KEY ([Id do Pedido]) REFERENCES [Pedido de Compra Novo] ([Id do Pedido]),
    CONSTRAINT [FK_VinculaPedidoOrcamento_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Conjuntos da Ordem de Serviço] (
    [Seqüência da Ordem de Serviço] int NOT NULL,
    [Seqüência Conjunto OS] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    CONSTRAINT [Seq Ordem e Seq Conjunto] PRIMARY KEY ([Seqüência da Ordem de Serviço], [Seqüência Conjunto OS]),
    CONSTRAINT [TB_Conjuntos_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço] FOREIGN KEY ([Seqüência da Ordem de Serviço]) REFERENCES [Ordem de Serviço] ([Seqüência da Ordem de Serviço]) ON DELETE CASCADE,
    CONSTRAINT [TB_Conjuntos_da_Ordem_de_Serviço_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto])
);
GO

CREATE TABLE [Parcelas Ordem de Serviço] (
    [Seqüência da Ordem de Serviço] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Ordem e PC] PRIMARY KEY ([Seqüência da Ordem de Serviço], [Número da Parcela]),
    CONSTRAINT [TB_Parcelas_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço] FOREIGN KEY ([Seqüência da Ordem de Serviço]) REFERENCES [Ordem de Serviço] ([Seqüência da Ordem de Serviço]) ON DELETE CASCADE
);
GO

CREATE TABLE [Peças da Ordem de Serviço] (
    [Seqüência da Ordem de Serviço] int NOT NULL,
    [Seqüência Peças OS] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    CONSTRAINT [Seq Ordem de Seq Peças] PRIMARY KEY ([Seqüência da Ordem de Serviço], [Seqüência Peças OS]),
    CONSTRAINT [TB_Peças_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço] FOREIGN KEY ([Seqüência da Ordem de Serviço]) REFERENCES [Ordem de Serviço] ([Seqüência da Ordem de Serviço]) ON DELETE CASCADE,
    CONSTRAINT [TB_Peças_da_Ordem_de_Serviço_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Pedido] (
    [Seqüência do Pedido] int NOT NULL IDENTITY,
    [Data de Emissão] datetime NULL,
    [Seqüência do Geral] int NOT NULL,
    [Seqüência da Propriedade] smallint NOT NULL,
    [Fechamento] smallint NOT NULL,
    [Valor do Fechamento] decimal(11,2) NOT NULL,
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Historico] text NOT NULL DEFAULT '',
    [Valor Total IPI dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total do ICMS] decimal(11,2) NOT NULL,
    [Valor Total dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total de Produtos Usados] decimal(11,2) NOT NULL,
    [Valor Total Conjuntos Usados] decimal(11,2) NOT NULL,
    [Valor Total dos Serviços] decimal(11,2) NOT NULL,
    [Valor Total do Pedido] decimal(11,2) NOT NULL,
    [Seqüência do Orçamento] int NOT NULL,
    [Seqüência da Ordem de Serviço] int NOT NULL,
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Seqüência do Vendedor] int NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [Ocultar Valor Unitário] bit NOT NULL,
    [Pedido Cancelado] bit NOT NULL,
    [Valor Total IPI das Peças] decimal(11,2) NOT NULL,
    [Valor Total das Peças] decimal(11,2) NOT NULL,
    [Valor Total das Peças Usadas] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Valor Total da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do Seguro] decimal(11,2) NOT NULL,
    [Data do Fechamento] datetime NULL,
    [Valor Total do PIS] decimal(11,2) NOT NULL,
    [Valor Total do COFINS] decimal(11,2) NOT NULL,
    [Valor Total da Base ST] decimal(11,2) NOT NULL,
    [Valor Total do ICMS ST] decimal(11,2) NOT NULL,
    [Alíquota do ISS] decimal(5,2) NOT NULL,
    [Reter ISS] bit NOT NULL,
    [Entrega Futura] bit NOT NULL,
    [Seqüência da Transportadora] int NOT NULL,
    [Valor do Imposto de Renda] decimal(11,2) NOT NULL,
    [Valor Total do Tributo] decimal(11,2) NOT NULL,
    [Nao Movimentar Estoque] bit NOT NULL,
    CONSTRAINT [Seqüência do Pedido] PRIMARY KEY ([Seqüência do Pedido]),
    CONSTRAINT [TB_Pedido_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Pedido_FK_Seqüência_da_Ordem_de_Serviço] FOREIGN KEY ([Seqüência da Ordem de Serviço]) REFERENCES [Ordem de Serviço] ([Seqüência da Ordem de Serviço]),
    CONSTRAINT [TB_Pedido_FK_Seqüência_da_Propriedade] FOREIGN KEY ([Seqüência da Propriedade]) REFERENCES [Propriedades] ([Seqüência da Propriedade]),
    CONSTRAINT [TB_Pedido_FK_Seqüência_da_Transportadora] FOREIGN KEY ([Seqüência da Transportadora]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Pedido_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Pedido_FK_Seqüência_do_Orçamento] FOREIGN KEY ([Seqüência do Orçamento]) REFERENCES [Orçamento] ([Seqüência do Orçamento]),
    CONSTRAINT [TB_Pedido_FK_Seqüência_do_Vendedor] FOREIGN KEY ([Seqüência do Vendedor]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Produtos da Ordem de Serviço] (
    [Seqüência da Ordem de Serviço] int NOT NULL,
    [Seqüência Produto OS] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    CONSTRAINT [Seq Os e Seq Prod] PRIMARY KEY ([Seqüência da Ordem de Serviço], [Seqüência Produto OS]),
    CONSTRAINT [TB_Produtos_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço] FOREIGN KEY ([Seqüência da Ordem de Serviço]) REFERENCES [Ordem de Serviço] ([Seqüência da Ordem de Serviço]) ON DELETE CASCADE,
    CONSTRAINT [TB_Produtos_da_Ordem_de_Serviço_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Serviços da Ordem de Serviço] (
    [Seqüência da Ordem de Serviço] int NOT NULL,
    [Seqüência Serviço OS] int NOT NULL IDENTITY,
    [Seqüência do Serviço] smallint NOT NULL,
    [Horas] decimal(7,2) NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq OS e Seq Serv] PRIMARY KEY ([Seqüência da Ordem de Serviço], [Seqüência Serviço OS]),
    CONSTRAINT [TB_Serviços_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço] FOREIGN KEY ([Seqüência da Ordem de Serviço]) REFERENCES [Ordem de Serviço] ([Seqüência da Ordem de Serviço]) ON DELETE CASCADE,
    CONSTRAINT [TB_Serviços_da_Ordem_de_Serviço_FK_Seqüência_do_Serviço] FOREIGN KEY ([Seqüência do Serviço]) REFERENCES [Servicos] ([Seqüência do Serviço])
);
GO

CREATE TABLE [Itens da Requisição] (
    [Seqüência da Requisição] int NOT NULL,
    [Seqüência Produto Requisição] int NOT NULL IDENTITY,
    [Descricao] varchar(120) NOT NULL DEFAULT '',
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Veiculo] varchar(30) NOT NULL DEFAULT '',
    CONSTRAINT [Seq Req e Seq Prod Requisição] PRIMARY KEY ([Seqüência da Requisição], [Seqüência Produto Requisição]),
    CONSTRAINT [TB_Itens_da_Requisição_FK_Seqüência_da_Requisição] FOREIGN KEY ([Seqüência da Requisição]) REFERENCES [Requisição] ([Seqüência da Requisição]) ON DELETE CASCADE
);
GO

CREATE TABLE [Conjuntos do Pedido] (
    [Seqüência do Pedido] int NOT NULL,
    [Seqüência do Conjunto Pedido] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Pedido e Seq Conjunto] PRIMARY KEY ([Seqüência do Pedido], [Seqüência do Conjunto Pedido]),
    CONSTRAINT [TB_Conjuntos_do_Pedido_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto]),
    CONSTRAINT [TB_Conjuntos_do_Pedido_FK_Seqüência_do_Pedido] FOREIGN KEY ([Seqüência do Pedido]) REFERENCES [Pedido] ([Seqüência do Pedido]) ON DELETE CASCADE
);
GO

CREATE TABLE [Nota Fiscal] (
    [Seqüência da Nota Fiscal] int NOT NULL IDENTITY,
    [Número da NFe] int NOT NULL,
    [Número da NFSe] int NOT NULL,
    [Número da Nota Fiscal] int NOT NULL,
    [Data de Emissão] datetime NULL,
    [Seqüência do Geral] int NOT NULL,
    [Seqüência da Propriedade] smallint NOT NULL,
    [Seqüência da Natureza] smallint NOT NULL,
    [Transportadora Avulsa] bit NOT NULL,
    [Seqüência da Transportadora] int NOT NULL,
    [Nome da Transportadora Avulsa] varchar(60) NOT NULL DEFAULT '',
    [Placa do Veículo] varchar(8) NOT NULL DEFAULT '',
    [UF do Veículo] varchar(3) NOT NULL DEFAULT '',
    [Frete] varchar(35) NULL DEFAULT '',
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Fechamento] smallint NOT NULL,
    [Valor do Fechamento] decimal(11,2) NOT NULL,
    [Volume] int NOT NULL,
    [Especie] varchar(20) NOT NULL DEFAULT '',
    [Data de Saída] datetime NULL,
    [Hora da Saída] datetime NULL,
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Historico] text NOT NULL DEFAULT '',
    [Valor Total IPI dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total IPI dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total do ICMS] decimal(11,2) NOT NULL,
    [Valor Total dos Produtos] decimal(11,2) NOT NULL,
    [Valor Total dos Conjuntos] decimal(11,2) NOT NULL,
    [Valor Total de Produtos Usados] decimal(11,2) NOT NULL,
    [Valor Total Conjuntos Usados] decimal(11,2) NOT NULL,
    [Valor Total dos Serviços] decimal(11,2) NOT NULL,
    [Valor Total da Nota Fiscal] decimal(11,2) NOT NULL,
    [Tipo de Nota] smallint NOT NULL,
    [Seqüência da Classificação] smallint NOT NULL,
    [Nota Cancelada] bit NOT NULL,
    [Seqüência do Pedido] int NOT NULL,
    [Seqüência do Vendedor] int NOT NULL,
    [Seqüência da Cobrança] smallint NOT NULL,
    [Nota Fiscal Avulsa] bit NOT NULL,
    [Peso Bruto] decimal(11,2) NOT NULL,
    [Peso Líquido] decimal(11,2) NOT NULL,
    [Ocultar Valor Unitário] bit NOT NULL,
    [Contra Apresentação] bit NOT NULL,
    [Município da Transportadora] int NOT NULL,
    [Documento da Transportadora] varchar(20) NOT NULL DEFAULT '',
    [NFe Complementar] bit NOT NULL,
    [Chave Acesso NFe Referenciada] varchar(45) NOT NULL DEFAULT '',
    [Chave de Acesso da NFe] varchar(50) NOT NULL DEFAULT '',
    [Protocolo de Autorização NFe] varchar(50) NOT NULL DEFAULT '',
    [Data e Hora da NFe] varchar(25) NOT NULL DEFAULT '',
    [Transmitido] bit NOT NULL,
    [Autorizado] bit NOT NULL,
    [Número do Recibo da NFe] varchar(20) NOT NULL DEFAULT '',
    [Marca] varchar(20) NOT NULL DEFAULT '',
    [Numeracao] varchar(20) NOT NULL DEFAULT '',
    [Valor Total IPI das Peças] decimal(11,2) NOT NULL,
    [Valor Total das Peças] decimal(11,2) NOT NULL,
    [Código da ANTT] varchar(20) NOT NULL DEFAULT '',
    [Endereço da Transportadora] varchar(40) NOT NULL DEFAULT '',
    [IE da Transportadora] varchar(15) NOT NULL DEFAULT '',
    [Observacao] text NOT NULL DEFAULT '',
    [Valor Total das Peças Usadas] decimal(11,2) NOT NULL,
    [Valor Total da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do Seguro] decimal(11,2) NOT NULL,
    [Valor Total do PIS] decimal(11,2) NOT NULL,
    [Valor Total do COFINS] decimal(11,2) NOT NULL,
    [Valor Total da Base ST] decimal(11,2) NOT NULL,
    [Valor Total do ICMS ST] decimal(11,2) NOT NULL,
    [Alíquota do ISS] decimal(5,2) NOT NULL,
    [Reter ISS] bit NOT NULL,
    [Recibo NFSe] varchar(255) NOT NULL DEFAULT '',
    [Imprimiu] bit NOT NULL,
    [Seqüência do Movimento] int NOT NULL,
    [Número do Contrato] int NOT NULL,
    [Valor do Imposto de Renda] decimal(11,2) NOT NULL,
    [Valor Total da Importação] decimal(11,2) NOT NULL,
    [Conjunto Avulso] bit NOT NULL,
    [Valor Total do Tributo] decimal(11,2) NOT NULL,
    [Descrição Conjunto Avulso] varchar(60) NOT NULL DEFAULT '',
    [FinNFe] smallint NOT NULL,
    [Novo Layout] bit NOT NULL,
    [Nota de Devolução] bit NOT NULL,
    [Chave da Devolução] varchar(200) NOT NULL DEFAULT '',
    [Outras Despesas] decimal(10,2) NOT NULL,
    [Chave da Devolução 2] varchar(200) NOT NULL DEFAULT '',
    [Chave da Devolução 3] varchar(200) NOT NULL DEFAULT '',
    [Cancelada no livro] bit NOT NULL,
    [Refaturamento] bit NOT NULL,
    [Nota de venda] int NOT NULL,
    [Financiamento] bit NOT NULL,
    CONSTRAINT [Seqüência da Nota Fiscal] PRIMARY KEY ([Seqüência da Nota Fiscal]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Município_da_Transportadora] FOREIGN KEY ([Município da Transportadora]) REFERENCES [Municipios] ([Seqüência do Município]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_da_Classificação] FOREIGN KEY ([Seqüência da Classificação]) REFERENCES [Classificação Fiscal] ([Seqüência da Classificação]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_da_Cobrança] FOREIGN KEY ([Seqüência da Cobrança]) REFERENCES [Tipo de Cobrança] ([Seqüência da Cobrança]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_da_Natureza] FOREIGN KEY ([Seqüência da Natureza]) REFERENCES [Natureza de Operação] ([Seqüência da Natureza]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_da_Propriedade] FOREIGN KEY ([Seqüência da Propriedade]) REFERENCES [Propriedades] ([Seqüência da Propriedade]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_da_Transportadora] FOREIGN KEY ([Seqüência da Transportadora]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_do_Movimento] FOREIGN KEY ([Seqüência do Movimento]) REFERENCES [Movimento do Estoque] ([Seqüência do Movimento]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_do_Pedido] FOREIGN KEY ([Seqüência do Pedido]) REFERENCES [Pedido] ([Seqüência do Pedido]),
    CONSTRAINT [TB_Nota_Fiscal_FK_Seqüência_do_Vendedor] FOREIGN KEY ([Seqüência do Vendedor]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Parcelas Pedido] (
    [Seqüência do Pedido] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Pedido e PC] PRIMARY KEY ([Seqüência do Pedido], [Número da Parcela]),
    CONSTRAINT [TB_Parcelas_Pedido_FK_Seqüência_do_Pedido] FOREIGN KEY ([Seqüência do Pedido]) REFERENCES [Pedido] ([Seqüência do Pedido]) ON DELETE CASCADE
);
GO

CREATE TABLE [Peças do Pedido] (
    [Seqüência do Pedido] int NOT NULL,
    [Seqüência da Peça Pedido] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Pedido e Seq Peça] PRIMARY KEY ([Seqüência do Pedido], [Seqüência da Peça Pedido]),
    CONSTRAINT [TB_Peças_do_Pedido_FK_Seqüência_do_Pedido] FOREIGN KEY ([Seqüência do Pedido]) REFERENCES [Pedido] ([Seqüência do Pedido]) ON DELETE CASCADE,
    CONSTRAINT [TB_Peças_do_Pedido_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Produtos do Pedido] (
    [Seqüência do Pedido] int NOT NULL,
    [Seqüência do Produto Pedido] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(5,2) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(5,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(7,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Pedido e Seq Produto] PRIMARY KEY ([Seqüência do Pedido], [Seqüência do Produto Pedido]),
    CONSTRAINT [TB_Produtos_do_Pedido_FK_Seqüência_do_Pedido] FOREIGN KEY ([Seqüência do Pedido]) REFERENCES [Pedido] ([Seqüência do Pedido]) ON DELETE CASCADE,
    CONSTRAINT [TB_Produtos_do_Pedido_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Serviços do Pedido] (
    [Seqüência do Pedido] int NOT NULL,
    [Seqüência do Serviço Pedido] int NOT NULL IDENTITY,
    [Seqüência do Serviço] smallint NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq Pedido e Seq Serv] PRIMARY KEY ([Seqüência do Pedido], [Seqüência do Serviço Pedido]),
    CONSTRAINT [TB_Serviços_do_Pedido_FK_Seqüência_do_Pedido] FOREIGN KEY ([Seqüência do Pedido]) REFERENCES [Pedido] ([Seqüência do Pedido]) ON DELETE CASCADE,
    CONSTRAINT [TB_Serviços_do_Pedido_FK_Seqüência_do_Serviço] FOREIGN KEY ([Seqüência do Serviço]) REFERENCES [Servicos] ([Seqüência do Serviço])
);
GO

CREATE TABLE [Cancelamento NFe] (
    [Seqüência Cancelamento NFe] int NOT NULL IDENTITY,
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Justificativa] varchar(255) NOT NULL DEFAULT '',
    [Ambiente] smallint NOT NULL,
    [Data do Cancelamento] datetime NULL,
    CONSTRAINT [Seq Can NFe e NF] PRIMARY KEY ([Seqüência Cancelamento NFe], [Seqüência da Nota Fiscal]),
    CONSTRAINT [TB_Cancelamento_NFe_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal])
);
GO

CREATE TABLE [Carta de Correção NFe] (
    [Seqüência da Correção] int NOT NULL IDENTITY,
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Número da Correção] smallint NOT NULL,
    [Justificativa CCe] text NOT NULL DEFAULT '',
    [Data Correção] datetime NULL,
    CONSTRAINT [Seq Cor e Seq NF] PRIMARY KEY ([Seqüência da Correção], [Seqüência da Nota Fiscal]),
    CONSTRAINT [TB_Carta_de_Correção_NFe_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal])
);
GO

CREATE TABLE [Comissao] (
    [Seqüência da Comissão] int NOT NULL IDENTITY,
    [Percentual de Comissão] decimal(6,2) NOT NULL,
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Percentual 2] decimal(6,2) NOT NULL,
    [Intermediario] int NOT NULL,
    CONSTRAINT [Seqüência da Comissão] PRIMARY KEY ([Seqüência da Comissão]),
    CONSTRAINT [TB_Comissão_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal])
);
GO

CREATE TABLE [Conjuntos da Nota Fiscal] (
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Seqüência Conjunto Nota Fiscal] int NOT NULL IDENTITY,
    [Seqüência do Conjunto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(8,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(6,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(8,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Bc pis] decimal(9,2) NOT NULL,
    [Aliq do pis] decimal(5,2) NOT NULL,
    [Bc cofins] decimal(9,2) NOT NULL,
    [Aliq do cofins] decimal(5,2) NOT NULL,
    CONSTRAINT [Seq NF e Seq Conj Nota Fiscal] PRIMARY KEY ([Seqüência da Nota Fiscal], [Seqüência Conjunto Nota Fiscal]),
    CONSTRAINT [TB_Conjuntos_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal]) ON DELETE CASCADE,
    CONSTRAINT [TB_Conjuntos_da_Nota_Fiscal_FK_Seqüência_do_Conjunto] FOREIGN KEY ([Seqüência do Conjunto]) REFERENCES [Conjuntos] ([Seqüência do Conjunto])
);
GO

CREATE TABLE [Manutenção Contas] (
    [Seqüência da Manutenção] int NOT NULL IDENTITY,
    [Parcela] smallint NOT NULL,
    [Seqüência do Geral] int NOT NULL,
    [Número da Nota Fiscal] int NOT NULL,
    [Data de Entrada] datetime NULL,
    [Historico] text NOT NULL DEFAULT '',
    [Forma de Pagamento] varchar(10) NOT NULL DEFAULT '',
    [Data de Vencimento] datetime NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    [Valor Pago] decimal(11,2) NOT NULL,
    [Valor do Juros] decimal(11,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Valor Restante] decimal(11,2) NOT NULL,
    [Tipo da Conta] varchar(11) NOT NULL DEFAULT '',
    [Data da Baixa] datetime NULL,
    [Seqüência da Cobrança] smallint NOT NULL,
    [Número da Duplicata] int NOT NULL,
    [Seqüência da Origem] int NOT NULL,
    [Seqüência da Baixa] int NOT NULL,
    [Documento] varchar(20) NOT NULL DEFAULT '',
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Cheque Impresso] bit NOT NULL,
    [Conta] varchar(1) NOT NULL DEFAULT '',
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Seqüência do Estoque] int NOT NULL,
    [Seqüência do Pedido] int NOT NULL,
    [Duplicata Impressa] bit NOT NULL,
    [Imprimir] bit NOT NULL,
    [Tpo de Recebimento] varchar(20) NOT NULL DEFAULT '',
    [Previsao] bit NOT NULL,
    [Titulo] varchar(25) NOT NULL DEFAULT '',
    [Sequencia da Compra] int NOT NULL,
    [Notas da Compra] varchar(100) NOT NULL DEFAULT '',
    [Conciliado] bit NOT NULL,
    [Vencimento Original] datetime NULL,
    [Sequencia Lan CC] int NOT NULL,
    [Vr da Previsão] decimal(10,2) NOT NULL,
    [Imp Previsao] bit NOT NULL,
    [Seqüência do Movimento] int NOT NULL,
    [Codigo do Debito] int NOT NULL,
    [Codigo do Credito] int NOT NULL,
    [SeqüênciaSubGrupoDespesa] smallint NOT NULL,
    [SeqüênciaGrupoDespesa] smallint NOT NULL,
    CONSTRAINT [Seqüência Manutenção] PRIMARY KEY ([Seqüência da Manutenção]),
    CONSTRAINT [TB_Manutenção_Contas_FK_Seqüência_Grupo_Despesa] FOREIGN KEY ([Seqüência Grupo Despesa]) REFERENCES [Grupo da Despesa] ([Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Manutenção_Contas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa] FOREIGN KEY ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]) REFERENCES [SubGrupo Despesa] ([Seqüência SubGrupo Despesa], [Seqüência Grupo Despesa]),
    CONSTRAINT [TB_Manutenção_Contas_FK_Seqüência_da_Cobrança] FOREIGN KEY ([Seqüência da Cobrança]) REFERENCES [Tipo de Cobrança] ([Seqüência da Cobrança]),
    CONSTRAINT [TB_Manutenção_Contas_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal]),
    CONSTRAINT [TB_Manutenção_Contas_FK_Seqüência_do_Geral] FOREIGN KEY ([Seqüência do Geral]) REFERENCES [Geral] ([Seqüência do Geral])
);
GO

CREATE TABLE [Notas Autorizadas] (
    [Seqüência do Notas] int NOT NULL IDENTITY,
    [Seqüência da Nota Fiscal] int NOT NULL,
    [XML] text NOT NULL DEFAULT '',
    CONSTRAINT [Seq Notas E Seq NF] PRIMARY KEY ([Seqüência do Notas], [Seqüência da Nota Fiscal]),
    CONSTRAINT [TB_Notas_Autorizadas_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal])
);
GO

CREATE TABLE [Parcelas Nota Fiscal] (
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Número da Parcela] smallint NOT NULL,
    [Dias] smallint NOT NULL,
    [Data de Vencimento] datetime NOT NULL,
    [Valor da Parcela] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq NF e PC] PRIMARY KEY ([Seqüência da Nota Fiscal], [Número da Parcela]),
    CONSTRAINT [TB_Parcelas_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal]) ON DELETE CASCADE
);
GO

CREATE TABLE [Peças da Nota Fiscal] (
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Seqüência da Peça Nota Fiscal] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(8,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(6,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(8,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Bc pis] decimal(9,2) NOT NULL,
    [Aliq do pis] decimal(5,2) NOT NULL,
    [Bc cofins] decimal(9,2) NOT NULL,
    [Aliq do cofins] decimal(5,2) NOT NULL,
    CONSTRAINT [Seq NF e Seq Peças Nota Fiscal] PRIMARY KEY ([Seqüência da Nota Fiscal], [Seqüência da Peça Nota Fiscal]),
    CONSTRAINT [TB_Peças_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal]) ON DELETE CASCADE,
    CONSTRAINT [TB_Peças_da_Nota_Fiscal_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Produtos da Nota Fiscal] (
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Seqüência Produto Nota Fiscal] int NOT NULL IDENTITY,
    [Seqüência do Produto] int NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(12,4) NOT NULL,
    [Valor Total] decimal(12,4) NOT NULL,
    [Valor do IPI] decimal(12,4) NOT NULL,
    [Valor do ICMS] decimal(12,4) NOT NULL,
    [Alíquota do IPI] decimal(8,4) NOT NULL,
    [Alíquota do ICMS] decimal(5,2) NOT NULL,
    [Percentual da Redução] decimal(6,2) NOT NULL,
    [Diferido] bit NOT NULL,
    [Valor da Base de Cálculo] decimal(11,2) NOT NULL,
    [Valor do PIS] decimal(11,4) NOT NULL,
    [Valor do Cofins] decimal(11,4) NOT NULL,
    [IVA] decimal(8,4) NOT NULL,
    [Base de Cálculo ST] decimal(11,2) NOT NULL,
    [Valor ICMS ST] decimal(11,2) NOT NULL,
    [CFOP] smallint NOT NULL,
    [CST] smallint NOT NULL,
    [Alíquota do ICMS ST] decimal(5,2) NOT NULL,
    [Base de Cálculo da Importação] decimal(11,2) NOT NULL,
    [Valor das Despesas Aduaneiras] decimal(11,2) NOT NULL,
    [Valor do Imposto de Importação] decimal(11,2) NOT NULL,
    [Valor do IOF] decimal(11,2) NOT NULL,
    [Valor do Tributo] decimal(11,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Valor do Frete] decimal(12,4) NOT NULL,
    [Bc pis] decimal(9,2) NOT NULL,
    [Bc cofins] decimal(9,2) NOT NULL,
    [Aliq do pis] decimal(5,2) NOT NULL,
    [Aliq do cofins] decimal(5,2) NOT NULL,
    CONSTRAINT [Seq NF e Seq Prod Nota Fiscal] PRIMARY KEY ([Seqüência da Nota Fiscal], [Seqüência Produto Nota Fiscal]),
    CONSTRAINT [TB_Produtos_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal]) ON DELETE CASCADE,
    CONSTRAINT [TB_Produtos_da_Nota_Fiscal_FK_Seqüência_do_Produto] FOREIGN KEY ([Seqüência do Produto]) REFERENCES [Produtos] ([Seqüência do Produto])
);
GO

CREATE TABLE [Serviços da Nota Fiscal] (
    [Seqüência da Nota Fiscal] int NOT NULL,
    [Seqüência Serviço Nota Fiscal] int NOT NULL IDENTITY,
    [Seqüência do Serviço] smallint NOT NULL,
    [Quantidade] decimal(11,4) NOT NULL,
    [Valor Unitário] decimal(11,4) NOT NULL,
    [Valor Total] decimal(11,2) NOT NULL,
    CONSTRAINT [Seq NF e Seq Serv Nota Fiscal] PRIMARY KEY ([Seqüência da Nota Fiscal], [Seqüência Serviço Nota Fiscal]),
    CONSTRAINT [TB_Serviços_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal] FOREIGN KEY ([Seqüência da Nota Fiscal]) REFERENCES [Nota Fiscal] ([Seqüência da Nota Fiscal]) ON DELETE CASCADE,
    CONSTRAINT [TB_Serviços_da_Nota_Fiscal_FK_Seqüência_do_Serviço] FOREIGN KEY ([Seqüência do Serviço]) REFERENCES [Servicos] ([Seqüência do Serviço])
);
GO

CREATE TABLE [Baixa Contas] (
    [Seqüência da Baixa] int NOT NULL IDENTITY,
    [Seqüência da Manutenção] int NOT NULL,
    [Data da Baixa] datetime NULL,
    [Valor Pago] decimal(11,2) NOT NULL,
    [Valor do Juros] decimal(11,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Seqüência da Agência] smallint NOT NULL,
    [Seqüência da CC da Agência] smallint NOT NULL,
    [Número do Cheque] varchar(20) NOT NULL DEFAULT '',
    [Conta] varchar(1) NOT NULL DEFAULT '',
    [Historico] text NOT NULL DEFAULT '',
    [Seqüência da Movimentação CC] int NOT NULL,
    [Bloqueado] bit NOT NULL,
    [Carteira] varchar(17) NOT NULL DEFAULT '',
    [Cliente Carteira] varchar(9) NOT NULL DEFAULT '',
    [Data Recebimento] datetime NULL,
    [Compensado] bit NOT NULL,
    [Pago] decimal(10,2) NOT NULL,
    [Dt Comissão] datetime NULL,
    [NF Comissão] int NOT NULL,
    [Codigo do Historico] smallint NOT NULL,
    [Codigo do Debito] int NOT NULL,
    [Codigo do Credito] int NOT NULL,
    [Seqüência Grupo Despesa] smallint NOT NULL,
    [Seqüência SubGrupo Despesa] smallint NOT NULL,
    [Beneficiario] int NOT NULL,
    [ProcessadoAutomaticamente] bit NULL DEFAULT CAST(0 AS bit),
    [SequenciaLancamentoBB] int NULL,
    [DataCriacaoIntegracao] datetime NULL,
    [SeqüênciaDaAgência] smallint NOT NULL,
    [SeqüênciaDaCcDaAgência] smallint NOT NULL,
    CONSTRAINT [Seqüência da Baixa] PRIMARY KEY ([Seqüência da Baixa]),
    CONSTRAINT [TB_Baixa_Contas_FK_Seqüência_da_Agência_Seqüência_da_CC_da_Agência] FOREIGN KEY ([SeqüênciaDaAgência], [SeqüênciaDaCcDaAgência]) REFERENCES [Conta Corrente da Agência] ([Seqüência da Agência], [Seqüência da CC da Agência]),
    CONSTRAINT [TB_Baixa_Contas_FK_Seqüência_da_Manutenção] FOREIGN KEY ([Seqüência da Manutenção]) REFERENCES [Manutenção Contas] ([Seqüência da Manutenção]),
    CONSTRAINT [TB_Baixa_Contas_FK_Seqüência_da_Movimentação_CC] FOREIGN KEY ([Seqüência da Movimentação CC]) REFERENCES [Movimentação da Conta Corrente] ([Seqüência da Movimentação CC])
);
GO

CREATE TABLE [Valores Adicionais] (
    [Seqüência do Valores] int NOT NULL IDENTITY,
    [Seqüência da Manutenção] int NOT NULL,
    [Valor do Juros] decimal(11,2) NOT NULL,
    [Valor do Desconto] decimal(11,2) NOT NULL,
    [Observacao] text NOT NULL DEFAULT '',
    [Conta] varchar(1) NOT NULL DEFAULT '',
    CONSTRAINT [Seq Valores e Seq Man] PRIMARY KEY ([Seqüência do Valores], [Seqüência da Manutenção]),
    CONSTRAINT [TB_Valores_Adicionais_FK_Seqüência_da_Manutenção] FOREIGN KEY ([Seqüência da Manutenção]) REFERENCES [Manutenção Contas] ([Seqüência da Manutenção])
);
GO

CREATE INDEX [IX_Adições da Declaração_Seqüência do Geral] ON [Adições da Declaração] ([Seqüência do Geral]);
GO

CREATE UNIQUE INDEX [Seq Declaração e Número Adição] ON [Adições da Declaração] ([Seqüência da Declaração], [Número da Adição]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Hora] ON [Agendamento de Backup] ([Hora]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Baixa Contas_Seqüência da Manutenção] ON [Baixa Contas] ([Seqüência da Manutenção]);
GO

CREATE INDEX [IX_Baixa Contas_Seqüência da Movimentação CC] ON [Baixa Contas] ([Seqüência da Movimentação CC]);
GO

CREATE INDEX [IX_Baixa Contas_SeqüênciaDaAgência_SeqüênciaDaCcDaAgência] ON [Baixa Contas] ([SeqüênciaDaAgência], [SeqüênciaDaCcDaAgência]);
GO

CREATE INDEX [IX_Baixa do Estoque Contábil_Seqüência da Despesa] ON [Baixa do Estoque Contábil] ([Seqüência da Despesa]);
GO

CREATE INDEX [IX_Baixa do Estoque Contábil_Seqüência do Conjunto] ON [Baixa do Estoque Contábil] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Baixa do Estoque Contábil_Seqüência do Geral] ON [Baixa do Estoque Contábil] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Baixa do Estoque Contábil_Seqüência do Produto] ON [Baixa do Estoque Contábil] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Baixa Industrialização_Seqüência do Produto] ON [Baixa Industrialização] ([Seqüência do Produto]);
GO

CREATE UNIQUE INDEX [Seq Mvto Seq Bx e Seq Item] ON [Baixa Industrialização] ([Seqüência do Movimento], [Seqüência do Item], [Seqüência da Baixa]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Baixa MP Produto_Seqüência da Matéria Prima] ON [Baixa MP Produto] ([Seqüência da Matéria Prima]);
GO

CREATE INDEX [IX_Baixa MP Produto_Seqüência do Produto] ON [Baixa MP Produto] ([Seqüência do Produto]);
GO

CREATE UNIQUE INDEX [Dta do Feriado] ON [Calendario] ([Dta do Feriado]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Seq NF] ON [Cancelamento NFe] ([Seqüência da Nota Fiscal]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Seq NF e Num Cor] ON [Carta de Correção NFe] ([Seqüência da Nota Fiscal], [Número da Correção]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Seq NF Comissao] ON [Comissao] ([Seqüência da Nota Fiscal]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [UK_ConfiguracaoIntegracao_Chave] ON [ConfiguracaoIntegracao] ([Chave]);
GO

CREATE INDEX [IX_Conjuntos_Seqüência da Classificação] ON [Conjuntos] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Conjuntos_Seqüência da Unidade] ON [Conjuntos] ([Seqüência da Unidade]);
GO

CREATE INDEX [IX_Conjuntos_Seqüência do Grupo Produto] ON [Conjuntos] ([Seqüência do Grupo Produto]);
GO

CREATE INDEX [IX_Conjuntos_SeqüênciaDoSubGrupoProduto_SeqüênciaDoGrupoProduto] ON [Conjuntos] ([SeqüênciaDoSubGrupoProduto], [SeqüênciaDoGrupoProduto]);
GO

CREATE INDEX [IX_Conjuntos da Nota Fiscal_Seqüência do Conjunto] ON [Conjuntos da Nota Fiscal] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Conjuntos da Ordem de Serviço_Seqüência do Conjunto] ON [Conjuntos da Ordem de Serviço] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Conjuntos do Movimento Estoque_Seqüência do Conjunto] ON [Conjuntos do Movimento Estoque] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Conjuntos do Orçamento_Seqüência do Conjunto] ON [Conjuntos do Orçamento] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Conjuntos do Pedido_Seqüência do Conjunto] ON [Conjuntos do Pedido] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Conjuntos Movimento Contábil_Seqüência do Conjunto] ON [Conjuntos Movimento Contábil] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Conjuntos Mvto Contábil Novo_Seqüência do Conjunto] ON [Conjuntos Mvto Contábil Novo] ([Seqüência do Conjunto]);
GO

CREATE INDEX [IX_Consumo do Pedido Compra_Id da Despesa] ON [Consumo do Pedido Compra] ([Id da Despesa]);
GO

CREATE UNIQUE INDEX [Conta_Contab] ON [Conta Contabil] ([Conta Contabil]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Número da Conta Corrente] ON [Conta Corrente da Agência] ([Número da Conta Corrente], [Seqüência da Agência]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [Id_e_Seq_Compra] ON [Controle de Compras] ([Id do Pedido], [Sequencia do Item]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Controle de Processos_Codigo do Advogado] ON [Controle de Processos] ([Codigo do Advogado]);
GO

CREATE INDEX [IX_Controle de Processos_Codigo do Status] ON [Controle de Processos] ([Codigo do Status]);
GO

CREATE INDEX [IX_Declarações de Importação_Seqüência do Geral] ON [Declarações de Importação] ([Seqüência do Geral]);
GO

CREATE UNIQUE INDEX [Seq NF e Seq Prod NF] ON [Declarações de Importação] ([Seqüência da Nota Fiscal], [Seqüência Produto Nota Fiscal]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Despesas_Seqüência da Classificação] ON [Despesas] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Despesas_Seqüência da Unidade] ON [Despesas] ([Seqüência da Unidade]);
GO

CREATE INDEX [IX_Despesas_Seqüência Grupo Despesa] ON [Despesas] ([Seqüência Grupo Despesa]);
GO

CREATE INDEX [IX_Despesas_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa] ON [Despesas] ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]);
GO

CREATE INDEX [IX_Despesas da Licitação_Sequencia da Despesa] ON [Despesas da Licitação] ([Sequencia da Despesa]);
GO

CREATE INDEX [IX_Despesas do Movimento Contábil_Seqüência da Despesa] ON [Despesas do Movimento Contábil] ([Seqüência da Despesa]);
GO

CREATE INDEX [IX_Despesas do Movimento Estoque_Seqüência da Despesa] ON [Despesas do Movimento Estoque] ([Seqüência da Despesa]);
GO

CREATE INDEX [IX_Despesas do Novo Pedido_Sequencia da Despesa] ON [Despesas do Novo Pedido] ([Sequencia da Despesa]);
GO

CREATE INDEX [IX_Despesas do Pedido Compra_Id da Despesa] ON [Despesas do Pedido Compra] ([Id da Despesa]);
GO

CREATE INDEX [IX_Despesas Mvto Contábil Novo_Seqüência da Despesa] ON [Despesas Mvto Contábil Novo] ([Seqüência da Despesa]);
GO

CREATE INDEX [IX_Entrada Contas_Seqüência do Geral] ON [Entrada Contas] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Entrada Contas_Seqüência Grupo Despesa] ON [Entrada Contas] ([Seqüência Grupo Despesa]);
GO

CREATE INDEX [IX_Entrada Contas_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa] ON [Entrada Contas] ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]);
GO

CREATE INDEX [IX_Geral_Seqüência do Município] ON [Geral] ([Seqüência do Município]);
GO

CREATE INDEX [IX_Geral_Seqüência do País] ON [Geral] ([Seqüência do País]);
GO

CREATE INDEX [IX_Geral_Seqüência do Vendedor] ON [Geral] ([Seqüência do Vendedor]);
GO

CREATE INDEX [IX_Geral_Seqüência Município Cobrança] ON [Geral] ([Seqüência Município Cobrança]);
GO

CREATE UNIQUE INDEX [UF do ICMS] ON [ICMS] ([UF]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Importação Produtos Estoque_Seqüência do Produto] ON [Importação Produtos Estoque] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Itens da Licitacao_Produto] ON [Itens da Licitacao] ([Produto]);
GO

CREATE INDEX [IX_Itens do Conjunto_Seqüência do Produto] ON [Itens do Conjunto] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_LancamentoBB_Conta] ON [LancamentoBancarioBB] ([SequenciaDaAgencia], [SequenciaDaCCDaAgencia]);
GO

CREATE INDEX [IX_LancamentoBB_DataLancamento] ON [LancamentoBancarioBB] ([DataLancamento]);
GO

CREATE INDEX [IX_LancamentoBB_Historico] ON [LancamentoBancarioBB] ([TextoDescricaoHistorico]);
GO

CREATE INDEX [IX_LancamentoBB_Processado] ON [LancamentoBancarioBB] ([Processado]);
GO

CREATE INDEX [IX_Linha de Produção_Codigo do setor] ON [Linha de Produção] ([Codigo do setor]);
GO

CREATE INDEX [IX_LogIntegracao_Categoria] ON [LogProcessamentoIntegracao] ([Categoria]);
GO

CREATE INDEX [IX_LogIntegracao_DataHora] ON [LogProcessamentoIntegracao] ([DataHora]);
GO

CREATE INDEX [IX_LogIntegracao_Nivel] ON [LogProcessamentoIntegracao] ([Nivel]);
GO

CREATE INDEX [IX_Manutenção Contas_Seqüência da Cobrança] ON [Manutenção Contas] ([Seqüência da Cobrança]);
GO

CREATE INDEX [IX_Manutenção Contas_Seqüência da Nota Fiscal] ON [Manutenção Contas] ([Seqüência da Nota Fiscal]);
GO

CREATE INDEX [IX_Manutenção Contas_Seqüência do Geral] ON [Manutenção Contas] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Manutenção Contas_Seqüência Grupo Despesa] ON [Manutenção Contas] ([Seqüência Grupo Despesa]);
GO

CREATE INDEX [IX_Manutenção Contas_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa] ON [Manutenção Contas] ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]);
GO

CREATE INDEX [IX_Matéria Prima_Seqüência do Produto] ON [Matéria Prima] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Movimentação da Conta Corrente_Seqüência do Histórico] ON [Movimentação da Conta Corrente] ([Seqüência do Histórico]);
GO

CREATE INDEX [IX_Movimentação da Conta Corrente_SeqüênciaDaAgência_SeqüênciaDaCcDaAgência] ON [Movimentação da Conta Corrente] ([SeqüênciaDaAgência], [SeqüênciaDaCcDaAgência]);
GO

CREATE INDEX [IX_Movimento do Estoque_Seqüência da Classificação] ON [Movimento do Estoque] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Movimento do Estoque_Seqüência da Propriedade] ON [Movimento do Estoque] ([Seqüência da Propriedade]);
GO

CREATE INDEX [IX_Movimento do Estoque_Seqüência do Geral] ON [Movimento do Estoque] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Movimento do Estoque_Seqüência Grupo Despesa] ON [Movimento do Estoque] ([Seqüência Grupo Despesa]);
GO

CREATE INDEX [IX_Movimento do Estoque_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa] ON [Movimento do Estoque] ([SeqüênciaSubGrupoDespesa], [SeqüênciaGrupoDespesa]);
GO

CREATE INDEX [IX_Movimento do Estoque Contábil_Seqüência do Geral] ON [Movimento do Estoque Contábil] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Nota Fiscal_Município da Transportadora] ON [Nota Fiscal] ([Município da Transportadora]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência da Classificação] ON [Nota Fiscal] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência da Cobrança] ON [Nota Fiscal] ([Seqüência da Cobrança]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência da Natureza] ON [Nota Fiscal] ([Seqüência da Natureza]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência da Propriedade] ON [Nota Fiscal] ([Seqüência da Propriedade]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência da Transportadora] ON [Nota Fiscal] ([Seqüência da Transportadora]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência do Geral] ON [Nota Fiscal] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência do Movimento] ON [Nota Fiscal] ([Seqüência do Movimento]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência do Pedido] ON [Nota Fiscal] ([Seqüência do Pedido]);
GO

CREATE INDEX [IX_Nota Fiscal_Seqüência do Vendedor] ON [Nota Fiscal] ([Seqüência do Vendedor]);
GO

CREATE UNIQUE INDEX [Seq NF Notas] ON [Notas Autorizadas] ([Seqüência da Nota Fiscal]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Nova Licitação_Sequencia da Transportadora] ON [Nova Licitação] ([Sequencia da Transportadora]);
GO

CREATE INDEX [IX_Nova Licitação_Sequencia do Fornecedor] ON [Nova Licitação] ([Sequencia do Fornecedor]);
GO

CREATE INDEX [IX_Orçamento_Seqüência da Classificação] ON [Orçamento] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Orçamento_Seqüência da Propriedade] ON [Orçamento] ([Seqüência da Propriedade]);
GO

CREATE INDEX [IX_Orçamento_Seqüência da Transportadora] ON [Orçamento] ([Seqüência da Transportadora]);
GO

CREATE INDEX [IX_Orçamento_Seqüência do Geral] ON [Orçamento] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Orçamento_Seqüência do Município] ON [Orçamento] ([Seqüência do Município]);
GO

CREATE INDEX [IX_Orçamento_Seqüência do País] ON [Orçamento] ([Seqüência do País]);
GO

CREATE INDEX [IX_Orçamento_Seqüência do Vendedor] ON [Orçamento] ([Seqüência do Vendedor]);
GO

CREATE INDEX [Número da Proforma] ON [Orçamento] ([Número da Proforma]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Ordem de Serviço_Seqüência da Classificação] ON [Ordem de Serviço] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Ordem de Serviço_Seqüência da Propriedade] ON [Ordem de Serviço] ([Seqüência da Propriedade]);
GO

CREATE INDEX [IX_Ordem de Serviço_Seqüência do Geral] ON [Ordem de Serviço] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Ordem de Serviço_Seqüência do Município] ON [Ordem de Serviço] ([Seqüência do Município]);
GO

CREATE INDEX [IX_Ordem de Serviço_Seqüência do Vendedor] ON [Ordem de Serviço] ([Seqüência do Vendedor]);
GO

CREATE INDEX [IX_Parcelas Entrada Contas_Seqüência da Entrada] ON [Parcelas Entrada Contas] ([Seqüência da Entrada]);
GO

CREATE INDEX [IX_Parcelas Movimento Estoque_Seqüência do Movimento] ON [Parcelas Movimento Estoque] ([Seqüência do Movimento]);
GO

CREATE INDEX [IX_Peças da Nota Fiscal_Seqüência do Produto] ON [Peças da Nota Fiscal] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Peças da Ordem de Serviço_Seqüência do Produto] ON [Peças da Ordem de Serviço] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Peças do Movimento Estoque_Seqüência do Produto] ON [Peças do Movimento Estoque] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Peças do Orçamento_Seqüência do Produto] ON [Peças do Orçamento] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Peças do Pedido_Seqüência do Produto] ON [Peças do Pedido] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Pedido_Seqüência da Classificação] ON [Pedido] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Pedido_Seqüência da Ordem de Serviço] ON [Pedido] ([Seqüência da Ordem de Serviço]);
GO

CREATE INDEX [IX_Pedido_Seqüência da Propriedade] ON [Pedido] ([Seqüência da Propriedade]);
GO

CREATE INDEX [IX_Pedido_Seqüência da Transportadora] ON [Pedido] ([Seqüência da Transportadora]);
GO

CREATE INDEX [IX_Pedido_Seqüência do Geral] ON [Pedido] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Pedido_Seqüência do Orçamento] ON [Pedido] ([Seqüência do Orçamento]);
GO

CREATE INDEX [IX_Pedido_Seqüência do Vendedor] ON [Pedido] ([Seqüência do Vendedor]);
GO

CREATE INDEX [IX_Planilha de Adiantamento_Cod do Vendedor] ON [Planilha de Adiantamento] ([Cod do Vendedor]);
GO

CREATE INDEX [IX_Produtos_Seqüência da Classificação] ON [Produtos] ([Seqüência da Classificação]);
GO

CREATE INDEX [IX_Produtos_Seqüência da Unidade] ON [Produtos] ([Seqüência da Unidade]);
GO

CREATE INDEX [IX_Produtos_Seqüência do Grupo Produto] ON [Produtos] ([Seqüência do Grupo Produto]);
GO

CREATE INDEX [IX_Produtos_SeqüênciaDoSubGrupoProduto_SeqüênciaDoGrupoProduto] ON [Produtos] ([SeqüênciaDoSubGrupoProduto], [SeqüênciaDoGrupoProduto]);
GO

CREATE INDEX [IX_Produtos da Licitação_Sequencia do Produto] ON [Produtos da Licitação] ([Sequencia do Produto]);
GO

CREATE INDEX [IX_Produtos da Nota Fiscal_Seqüência do Produto] ON [Produtos da Nota Fiscal] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Produtos da Ordem de Serviço_Seqüência do Produto] ON [Produtos da Ordem de Serviço] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Produtos do Movimento Contábil_Seqüência do Produto] ON [Produtos do Movimento Contábil] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Produtos do Movimento Estoque_Seqüência do Produto] ON [Produtos do Movimento Estoque] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Produtos do Novo Pedido_Sequencia do Produto] ON [Produtos do Novo Pedido] ([Sequencia do Produto]);
GO

CREATE INDEX [IX_Produtos do Orçamento_Seqüência do Produto] ON [Produtos do Orçamento] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Produtos do Pedido_Seqüência do Produto] ON [Produtos do Pedido] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Produtos do Pedido Compra_Id do Produto] ON [Produtos do Pedido Compra] ([Id do Produto]);
GO

CREATE INDEX [IX_Produtos Mvto Contábil Novo_Seqüência do Produto] ON [Produtos Mvto Contábil Novo] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Propriedades do Geral_Seqüência da Propriedade] ON [Propriedades do Geral] ([Seqüência da Propriedade]);
GO

CREATE UNIQUE INDEX [Seq Geral e Seq Prop] ON [Propriedades do Geral] ([Seqüência do Geral], [Seqüência da Propriedade]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_PW~Tabelas_PW~Grupo] ON [PW~Tabelas] ([PW~Grupo]);
GO

CREATE INDEX [PW~Grupo] ON [PW~Usuarios] ([PW~Grupo]);
GO

CREATE INDEX [IX_Relatorio de Viagem_Sequencia do Geral] ON [Relatorio de Viagem] ([Sequencia do Geral]);
GO

CREATE INDEX [IX_Requisição_Seqüência do Geral] ON [Requisição] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_Revendedores_Id da Conta] ON [Revendedores] ([Id da Conta]);
GO

CREATE UNIQUE INDEX [Serie do Gerador] ON [Serie Gerador] ([Serie do Gerador]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Serie do Hidroturbo] ON [Serie Hidroturbo] ([Serie do Hidroturbo]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Serie da Moto Bomba] ON [Serie Moto Bomba] ([Serie da Moto Bomba]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [Serie do Pivo] ON [Serie Pivos] ([Serie do Pivo]) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE INDEX [SerieRebocador] ON [Serie Rebocador] ([Serie do Rebocador]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IX_Serviços da Nota Fiscal_Seqüência do Serviço] ON [Serviços da Nota Fiscal] ([Seqüência do Serviço]);
GO

CREATE INDEX [IX_Serviços da Ordem de Serviço_Seqüência do Serviço] ON [Serviços da Ordem de Serviço] ([Seqüência do Serviço]);
GO

CREATE INDEX [IX_Serviços do Orçamento_Seqüência do Serviço] ON [Serviços do Orçamento] ([Seqüência do Serviço]);
GO

CREATE INDEX [IX_Serviços do Pedido_Seqüência do Serviço] ON [Serviços do Pedido] ([Seqüência do Serviço]);
GO

CREATE INDEX [IX_SubGrupo Despesa_Seqüência Grupo Despesa] ON [SubGrupo Despesa] ([Seqüência Grupo Despesa]);
GO

CREATE INDEX [IX_SubGrupo do Produto_Seqüência do Grupo Produto] ON [SubGrupo do Produto] ([Seqüência do Grupo Produto]);
GO

CREATE UNIQUE INDEX [IX_Tenants_Dominio] ON [Tenants] ([Dominio]);
GO

CREATE INDEX [IX_Transferência de Receita_Seqüência da Unidade] ON [Transferência de Receita] ([Seqüência da Unidade]);
GO

CREATE INDEX [IX_Transferência de Receita_Seqüência do Grupo Produto] ON [Transferência de Receita] ([Seqüência do Grupo Produto]);
GO

CREATE INDEX [IX_Transferência de Receita_Seqüência do Produto] ON [Transferência de Receita] ([Seqüência do Produto]);
GO

CREATE INDEX [IX_Transferência de Receita_SeqüênciaDoSubGrupoProduto_SeqüênciaDoGrupoProduto] ON [Transferência de Receita] ([SeqüênciaDoSubGrupoProduto], [SeqüênciaDoGrupoProduto]);
GO

CREATE INDEX [IX_Valores Adicionais_Seqüência da Manutenção] ON [Valores Adicionais] ([Seqüência da Manutenção]);
GO

CREATE INDEX [IX_VinculaPedidoOrcamento_Id do Pedido] ON [VinculaPedidoOrcamento] ([Id do Pedido]);
GO

CREATE INDEX [IX_VinculaPedidoOrcamento_Seqüência do Geral] ON [VinculaPedidoOrcamento] ([Seqüência do Geral]);
GO

CREATE INDEX [IX_VinculaPedidoOrcamento_Seqüência do Orçamento] ON [VinculaPedidoOrcamento] ([Seqüência do Orçamento]);
GO

CREATE INDEX [IX_VinculaPedidoOrcamento_Seqüência do Produto] ON [VinculaPedidoOrcamento] ([Seqüência do Produto]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251111183501_CreateAllTables', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ClassTrib] (
    [Id] int NOT NULL IDENTITY,
    [CodigoClassTrib] varchar(6) NOT NULL,
    [CodigoSituacaoTributaria] varchar(3) NOT NULL,
    [DescricaoSituacaoTributaria] nvarchar(200) NULL,
    [DescricaoClassTrib] nvarchar(max) NOT NULL,
    [PercentualReducaoIBS] decimal(8,5) NOT NULL,
    [PercentualReducaoCBS] decimal(8,5) NOT NULL,
    [TipoAliquota] varchar(50) NULL,
    [ValidoParaNFe] bit NOT NULL,
    [TributacaoRegular] bit NOT NULL,
    [CreditoPresumidoOperacoes] bit NOT NULL,
    [EstornoCredito] bit NOT NULL,
    [AnexoLegislacao] int NULL,
    [LinkLegislacao] varchar(500) NULL,
    [DataSincronizacao] datetime2 NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_ClassTrib] PRIMARY KEY ([Id])
);
GO

ALTER TABLE [Classificação Fiscal] ADD [ClassTribId] int NULL;
GO

CREATE INDEX [IX_Classificação Fiscal_ClassTribId] ON [Classificação Fiscal] ([ClassTribId]);
GO

CREATE UNIQUE INDEX [IX_ClassTrib_CodigoClassTrib] ON [ClassTrib] ([CodigoClassTrib]);
GO

CREATE INDEX [IX_ClassTrib_CodigoSituacaoTributaria] ON [ClassTrib] ([CodigoSituacaoTributaria]);
GO

ALTER TABLE [Classificação Fiscal] ADD CONSTRAINT [FK_Classificação Fiscal_ClassTrib_ClassTribId] FOREIGN KEY ([ClassTribId]) REFERENCES [ClassTrib] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251125132336_CriacaoClassTrib', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [PW~Usuarios] ADD [PW~Ativo] bit NOT NULL DEFAULT CAST(1 AS bit);
GO

CREATE TABLE [PermissoesTela] (
    [Id] int NOT NULL IDENTITY,
    [Grupo] nvarchar(100) NOT NULL,
    [Modulo] nvarchar(100) NOT NULL,
    [Tela] nvarchar(100) NOT NULL,
    [NomeTela] nvarchar(200) NOT NULL,
    [Rota] nvarchar(200) NOT NULL,
    [Consultar] bit NOT NULL,
    [Incluir] bit NOT NULL,
    [Alterar] bit NOT NULL,
    [Excluir] bit NOT NULL,
    [Ordem] int NOT NULL,
    CONSTRAINT [PK_PermissoesTela] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PermissoesTemplate] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [Descricao] nvarchar(500) NULL,
    [IsPadrao] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_PermissoesTemplate] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PermissoesTemplateDetalhe] (
    [Id] int NOT NULL IDENTITY,
    [TemplateId] int NOT NULL,
    [Modulo] nvarchar(100) NOT NULL,
    [Tela] nvarchar(100) NOT NULL,
    [Consultar] bit NOT NULL,
    [Incluir] bit NOT NULL,
    [Alterar] bit NOT NULL,
    [Excluir] bit NOT NULL,
    CONSTRAINT [PK_PermissoesTemplateDetalhe] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PermissoesTemplateDetalhe_Template] FOREIGN KEY ([TemplateId]) REFERENCES [PermissoesTemplate] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_PermissoesTela_Grupo_Tela] ON [PermissoesTela] ([Grupo], [Tela]);
GO

CREATE UNIQUE INDEX [IX_PermissoesTemplate_Nome] ON [PermissoesTemplate] ([Nome]);
GO

CREATE UNIQUE INDEX [IX_PermissoesTemplateDetalhe_Template_Tela] ON [PermissoesTemplateDetalhe] ([TemplateId], [Tela]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251128133523_AddPermissoesTelas', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251202191622_SyncGrupoUsuarioFK', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [LogsAuditoria] (
    [Id] bigint NOT NULL IDENTITY,
    [DataHora] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UsuarioCodigo] int NOT NULL,
    [UsuarioNome] nvarchar(100) NOT NULL,
    [UsuarioGrupo] nvarchar(50) NOT NULL,
    [TipoAcao] nvarchar(50) NOT NULL,
    [Modulo] nvarchar(50) NOT NULL,
    [Entidade] nvarchar(100) NOT NULL,
    [EntidadeId] nvarchar(100) NULL,
    [Descricao] nvarchar(500) NOT NULL,
    [DadosAnteriores] nvarchar(max) NULL,
    [DadosNovos] nvarchar(max) NULL,
    [CamposAlterados] nvarchar(1000) NULL,
    [EnderecoIP] nvarchar(50) NULL,
    [UserAgent] nvarchar(500) NULL,
    [MetodoHttp] nvarchar(10) NULL,
    [UrlRequisicao] nvarchar(500) NULL,
    [StatusCode] int NULL,
    [TempoExecucaoMs] bigint NULL,
    [Erro] bit NOT NULL DEFAULT CAST(0 AS bit),
    [MensagemErro] nvarchar(2000) NULL,
    [TenantId] int NULL,
    [TenantNome] nvarchar(100) NULL,
    [SessaoId] nvarchar(100) NULL,
    [CorrelationId] nvarchar(50) NULL,
    CONSTRAINT [PK_LogsAuditoria] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_LogsAuditoria_DataHora] ON [LogsAuditoria] ([DataHora]);
GO

CREATE INDEX [IX_LogsAuditoria_Entidade] ON [LogsAuditoria] ([Entidade], [EntidadeId]);
GO

CREATE INDEX [IX_LogsAuditoria_Modulo] ON [LogsAuditoria] ([Modulo]);
GO

CREATE INDEX [IX_LogsAuditoria_Tenant] ON [LogsAuditoria] ([TenantId]);
GO

CREATE INDEX [IX_LogsAuditoria_TipoAcao] ON [LogsAuditoria] ([TipoAcao]);
GO

CREATE INDEX [IX_LogsAuditoria_Usuario] ON [LogsAuditoria] ([UsuarioCodigo]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251203194014_CreateLogsAuditoria', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Reboques] (
    [Id] int NOT NULL IDENTITY,
    [Placa] nvarchar(8) NOT NULL,
    [Tara] int NOT NULL,
    [CapacidadeKg] int NULL,
    [TipoRodado] nvarchar(50) NOT NULL,
    [TipoCarroceria] nvarchar(50) NOT NULL,
    [Uf] nvarchar(2) NOT NULL,
    [Rntrc] nvarchar(20) NULL,
    [Renavam] nvarchar(20) NULL,
    [Chassi] nvarchar(30) NULL,
    [Ativo] bit NOT NULL,
    [Observacoes] nvarchar(1000) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataUltimaAlteracao] datetime2 NULL,
    CONSTRAINT [PK_Reboques] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Veiculos] (
    [Id] int NOT NULL IDENTITY,
    [Placa] nvarchar(8) NOT NULL,
    [Marca] nvarchar(100) NULL,
    [Modelo] nvarchar(100) NULL,
    [AnoFabricacao] int NULL,
    [AnoModelo] int NULL,
    [Tara] int NOT NULL,
    [CapacidadeKg] int NULL,
    [TipoRodado] nvarchar(50) NOT NULL,
    [TipoCarroceria] nvarchar(50) NOT NULL,
    [Uf] nvarchar(2) NOT NULL,
    [Renavam] nvarchar(20) NULL,
    [Chassi] nvarchar(30) NULL,
    [Cor] nvarchar(30) NULL,
    [TipoCombustivel] nvarchar(30) NULL,
    [Rntrc] nvarchar(20) NULL,
    [Ativo] bit NOT NULL,
    [Observacoes] nvarchar(1000) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataUltimaAlteracao] datetime2 NULL,
    CONSTRAINT [PK_Veiculos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ManutencoesVeiculo] (
    [Id] int NOT NULL IDENTITY,
    [VeiculoId] int NOT NULL,
    [FornecedorId] int NULL,
    [DataManutencao] datetime2 NOT NULL,
    [TipoManutencao] nvarchar(100) NULL,
    [DescricaoServico] nvarchar(500) NULL,
    [KmAtual] decimal(18,2) NULL,
    [ValorMaoObra] decimal(18,2) NOT NULL,
    [ValorServicosTerceiros] decimal(18,2) NOT NULL,
    [NumeroOS] nvarchar(50) NULL,
    [NumeroNF] nvarchar(50) NULL,
    [DataProximaManutencao] datetime2 NULL,
    [KmProximaManutencao] decimal(18,2) NULL,
    [Observacoes] nvarchar(1000) NULL,
    [Ativo] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataUltimaAlteracao] datetime2 NULL,
    CONSTRAINT [PK_ManutencoesVeiculo] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ManutencoesVeiculo_Geral_FornecedorId] FOREIGN KEY ([FornecedorId]) REFERENCES [Geral] ([Seqüência do Geral]),
    CONSTRAINT [FK_ManutencoesVeiculo_Veiculos_VeiculoId] FOREIGN KEY ([VeiculoId]) REFERENCES [Veiculos] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Viagens] (
    [Id] int NOT NULL IDENTITY,
    [VeiculoId] int NOT NULL,
    [MotoristaId] smallint NULL,
    [ReboqueId] int NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataFim] datetime2 NOT NULL,
    [KmInicial] decimal(18,2) NULL,
    [KmFinal] decimal(18,2) NULL,
    [Origem] nvarchar(200) NULL,
    [Destino] nvarchar(200) NULL,
    [DescricaoCarga] nvarchar(500) NULL,
    [PesoCarga] decimal(18,2) NULL,
    [Observacoes] nvarchar(1000) NULL,
    [Ativo] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataUltimaAlteracao] datetime2 NULL,
    CONSTRAINT [PK_Viagens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Viagens_Motoristas_MotoristaId] FOREIGN KEY ([MotoristaId]) REFERENCES [Motoristas] ([Codigo do Motorista]),
    CONSTRAINT [FK_Viagens_Reboques_ReboqueId] FOREIGN KEY ([ReboqueId]) REFERENCES [Reboques] ([Id]),
    CONSTRAINT [FK_Viagens_Veiculos_VeiculoId] FOREIGN KEY ([VeiculoId]) REFERENCES [Veiculos] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ManutencoesPeca] (
    [Id] int NOT NULL IDENTITY,
    [ManutencaoId] int NOT NULL,
    [DescricaoPeca] nvarchar(200) NOT NULL,
    [CodigoPeca] nvarchar(50) NULL,
    [Marca] nvarchar(100) NULL,
    [Quantidade] decimal(18,4) NOT NULL,
    [Unidade] nvarchar(10) NOT NULL,
    [ValorUnitario] decimal(18,4) NOT NULL,
    [Observacoes] nvarchar(500) NULL,
    [Ativo] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataUltimaAlteracao] datetime2 NULL,
    CONSTRAINT [PK_ManutencoesPeca] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ManutencoesPeca_ManutencoesVeiculo_ManutencaoId] FOREIGN KEY ([ManutencaoId]) REFERENCES [ManutencoesVeiculo] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DespesasViagem] (
    [Id] int NOT NULL IDENTITY,
    [ViagemId] int NOT NULL,
    [TipoDespesa] nvarchar(100) NOT NULL,
    [Descricao] nvarchar(500) NULL,
    [Valor] decimal(18,2) NOT NULL,
    [DataDespesa] datetime2 NOT NULL,
    [NumeroDocumento] nvarchar(50) NULL,
    [Local] nvarchar(200) NULL,
    [KmAtual] decimal(18,2) NULL,
    [Litros] decimal(18,3) NULL,
    [Observacoes] nvarchar(500) NULL,
    [Ativo] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataUltimaAlteracao] datetime2 NULL,
    CONSTRAINT [PK_DespesasViagem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DespesasViagem_Viagens_ViagemId] FOREIGN KEY ([ViagemId]) REFERENCES [Viagens] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ReceitasViagem] (
    [Id] int NOT NULL IDENTITY,
    [ViagemId] int NOT NULL,
    [Descricao] nvarchar(500) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [DataReceita] datetime2 NOT NULL,
    [Origem] nvarchar(100) NULL,
    [NumeroDocumento] nvarchar(50) NULL,
    [Cliente] nvarchar(200) NULL,
    [Observacoes] nvarchar(500) NULL,
    [Ativo] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataUltimaAlteracao] datetime2 NULL,
    CONSTRAINT [PK_ReceitasViagem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ReceitasViagem_Viagens_ViagemId] FOREIGN KEY ([ViagemId]) REFERENCES [Viagens] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_DespesasViagem_DataDespesa] ON [DespesasViagem] ([DataDespesa]);
GO

CREATE INDEX [IX_DespesasViagem_TipoDespesa] ON [DespesasViagem] ([TipoDespesa]);
GO

CREATE INDEX [IX_DespesasViagem_ViagemId] ON [DespesasViagem] ([ViagemId]);
GO

CREATE INDEX [IX_ManutencoesPeca_ManutencaoId] ON [ManutencoesPeca] ([ManutencaoId]);
GO

CREATE INDEX [IX_ManutencoesVeiculo_DataManutencao] ON [ManutencoesVeiculo] ([DataManutencao]);
GO

CREATE INDEX [IX_ManutencoesVeiculo_FornecedorId] ON [ManutencoesVeiculo] ([FornecedorId]);
GO

CREATE INDEX [IX_ManutencoesVeiculo_VeiculoId] ON [ManutencoesVeiculo] ([VeiculoId]);
GO

CREATE UNIQUE INDEX [IX_Reboques_Placa] ON [Reboques] ([Placa]);
GO

CREATE INDEX [IX_ReceitasViagem_DataReceita] ON [ReceitasViagem] ([DataReceita]);
GO

CREATE INDEX [IX_ReceitasViagem_ViagemId] ON [ReceitasViagem] ([ViagemId]);
GO

CREATE UNIQUE INDEX [IX_Veiculos_Placa] ON [Veiculos] ([Placa]);
GO

CREATE INDEX [IX_Viagens_DataInicio] ON [Viagens] ([DataInicio]);
GO

CREATE INDEX [IX_Viagens_MotoristaId] ON [Viagens] ([MotoristaId]);
GO

CREATE INDEX [IX_Viagens_ReboqueId] ON [Viagens] ([ReboqueId]);
GO

CREATE INDEX [IX_Viagens_VeiculoId] ON [Viagens] ([VeiculoId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251209161029_AddModuloTransporte', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Reboques] ADD [AnoFabricacao] int NULL;
GO

ALTER TABLE [Reboques] ADD [Marca] nvarchar(100) NULL;
GO

ALTER TABLE [Reboques] ADD [Modelo] nvarchar(100) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251210205540_AddMarcaModeloToReboques', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

                IF NOT EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'PW~Usuarios' 
                    AND COLUMN_NAME = 'Email'
                )
                BEGIN
                    ALTER TABLE [PW~Usuarios] ADD [Email] varchar(255) NULL
                END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251211125540_AddEmailToUsuario', N'8.0.0');
GO

COMMIT;
GO

