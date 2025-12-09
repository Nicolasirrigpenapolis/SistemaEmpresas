-- =====================================================
-- SCRIPT: Criação de Índices para Performance
-- Sistema: SistemaEmpresas
-- Data: 2025-11-29
-- Descrição: Índices para otimizar consultas do Dashboard
-- =====================================================

-- ATENÇÃO: Execute este script no banco de dados IRRIGACAO (ou outro tenant)
-- Recomendado executar em horário de baixo movimento

USE IRRIGACAO;
GO

PRINT '=== Iniciando criação de índices para performance ==='
PRINT ''

-- =====================================================
-- TABELA: Orçamento
-- Queries afetadas: Dashboard KPIs, Status, Timeline
-- =====================================================

-- Índice para filtros de status (Venda Fechada + Cancelado)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orcamento_Status' AND object_id = OBJECT_ID('[Orçamento]'))
BEGIN
    PRINT 'Criando índice IX_Orcamento_Status...'
    CREATE NONCLUSTERED INDEX IX_Orcamento_Status
    ON [Orçamento] ([Venda Fechada], [Cancelado])
    INCLUDE ([Seqüência do Orçamento], [Data de Emissão], [Nome Cliente]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_Orcamento_Status já existe'
GO

-- Índice para timeline de orçamentos (Data de Emissão + Cancelado)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orcamento_DataEmissao' AND object_id = OBJECT_ID('[Orçamento]'))
BEGIN
    PRINT 'Criando índice IX_Orcamento_DataEmissao...'
    CREATE NONCLUSTERED INDEX IX_Orcamento_DataEmissao
    ON [Orçamento] ([Data de Emissão], [Cancelado])
    INCLUDE ([Venda Fechada]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_Orcamento_DataEmissao já existe'
GO

-- =====================================================
-- TABELA: Pedido de Compra Novo
-- Queries afetadas: Dashboard Compras Pendentes
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PedidoCompra_Status' AND object_id = OBJECT_ID('[Pedido de Compra Novo]'))
BEGIN
    PRINT 'Criando índice IX_PedidoCompra_Status...'
    CREATE NONCLUSTERED INDEX IX_PedidoCompra_Status
    ON [Pedido de Compra Novo] ([Pedido Fechado], [Cancelado], [Validado])
    INCLUDE ([Id do Pedido], [Data do Pedido], [Previsão de Entrega]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_PedidoCompra_Status já existe'
GO

-- =====================================================
-- TABELA: Produto
-- Queries afetadas: Dashboard Estoque Crítico, Combos
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Produto_EstoqueCritico' AND object_id = OBJECT_ID('[Produto]'))
BEGIN
    PRINT 'Criando índice IX_Produto_EstoqueCritico...'
    CREATE NONCLUSTERED INDEX IX_Produto_EstoqueCritico
    ON [Produto] ([Inativo], [Quantidade no Estoque], [Quantidade Mínima])
    INCLUDE ([Seqüência do Produto], [Descrição], [Localização]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_Produto_EstoqueCritico já existe'
GO

-- =====================================================
-- TABELA: Ordem de Serviço
-- Queries afetadas: Dashboard Status OS
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OrdemServico_Status' AND object_id = OBJECT_ID('[Ordem de Serviço]'))
BEGIN
    PRINT 'Criando índice IX_OrdemServico_Status...'
    CREATE NONCLUSTERED INDEX IX_OrdemServico_Status
    ON [Ordem de Serviço] ([Fechamento], [Serviço em Garantia])
    INCLUDE ([Seqüência de Controle]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_OrdemServico_Status já existe'
GO

-- =====================================================
-- TABELA: Geral (Clientes/Fornecedores/etc)
-- Queries afetadas: Autocompletes, Combos
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Geral_Busca' AND object_id = OBJECT_ID('[Geral]'))
BEGIN
    PRINT 'Criando índice IX_Geral_Busca...'
    CREATE NONCLUSTERED INDEX IX_Geral_Busca
    ON [Geral] ([Inativo], [Cliente], [Fornecedor], [Transportadora], [Vendedor])
    INCLUDE ([Seqüência do Geral], [Razão Social], [Nome Fantasia], [CPF e CNPJ], [Email]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_Geral_Busca já existe'
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Geral_RazaoSocial' AND object_id = OBJECT_ID('[Geral]'))
BEGIN
    PRINT 'Criando índice IX_Geral_RazaoSocial...'
    CREATE NONCLUSTERED INDEX IX_Geral_RazaoSocial
    ON [Geral] ([Razão Social])
    INCLUDE ([Seqüência do Geral], [Nome Fantasia], [CPF e CNPJ]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_Geral_RazaoSocial já existe'
GO

-- =====================================================
-- TABELA: Situação dos Pedidos
-- Queries afetadas: Dashboard Alertas de Atraso
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SituacaoPedidos_Atraso' AND object_id = OBJECT_ID('[Situação dos Pedidos]'))
BEGIN
    PRINT 'Criando índice IX_SituacaoPedidos_Atraso...'
    CREATE NONCLUSTERED INDEX IX_SituacaoPedidos_Atraso
    ON [Situação dos Pedidos] ([Dias em Atraso]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_SituacaoPedidos_Atraso já existe'
GO

-- =====================================================
-- TABELA: ClassTrib (Classificação Tributária)
-- Queries afetadas: Combos e pesquisas
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ClassTrib_CST' AND object_id = OBJECT_ID('[ClassTrib]'))
BEGIN
    PRINT 'Criando índice IX_ClassTrib_CST...'
    CREATE NONCLUSTERED INDEX IX_ClassTrib_CST
    ON [ClassTrib] ([CodigoSituacaoTributaria], [ValidoParaNFe])
    INCLUDE ([Id], [CodigoClassTrib], [DescricaoClassTrib]);
    PRINT '  OK - Índice criado com sucesso'
END
ELSE
    PRINT '  SKIP - IX_ClassTrib_CST já existe'
GO

-- =====================================================
-- Atualizar estatísticas das tabelas principais
-- =====================================================

PRINT ''
PRINT '=== Atualizando estatísticas ==='

UPDATE STATISTICS [Orçamento];
PRINT '  Orçamento - OK'

UPDATE STATISTICS [Pedido de Compra Novo];
PRINT '  Pedido de Compra Novo - OK'

UPDATE STATISTICS [Produto];
PRINT '  Produto - OK'

UPDATE STATISTICS [Geral];
PRINT '  Geral - OK'

PRINT ''
PRINT '=== Script finalizado com sucesso ==='
PRINT 'Recomendação: Monitore o desempenho das queries após aplicar os índices'
GO
