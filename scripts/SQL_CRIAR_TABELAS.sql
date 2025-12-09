-- ============================================================================
-- SCRIPT SQL - SISTEMA EMPRESAS
-- Criar tabelas novas e adicionar vínculo na Classificação Fiscal
-- 
-- INSTRUÇÕES:
-- 1. Execute este script no banco IRRIGACAO (servidor SRVSQL\SQLEXPRESS)
-- 2. Se for usar para Chinellato, execute no banco ChinellatoTransportes
-- ============================================================================

USE IRRIGACAO; -- Altere para ChinellatoTransportes se necessário
GO

-- ============================================================================
-- 1. TABELA TENANTS (Controle Multi-Tenant)
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tenants')
BEGIN
    CREATE TABLE [dbo].[Tenants] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Nome] NVARCHAR(200) NOT NULL,
        [Dominio] NVARCHAR(200) NOT NULL,
        [ConnectionString] NVARCHAR(500) NOT NULL,
        [Ativo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [IX_Tenants_Dominio] UNIQUE ([Dominio])
    );
    
    PRINT 'Tabela Tenants criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela Tenants já existe.';
END
GO

-- Inserir os tenants (empresas)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Tenants] WHERE [Dominio] = 'irrigacao')
BEGIN
    INSERT INTO [dbo].[Tenants] ([Nome], [Dominio], [ConnectionString], [Ativo])
    VALUES (
        'Irrigação Penápolis', 
        'irrigacao', 
        'Server=SRVSQL\SQLEXPRESS;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;',
        1
    );
    PRINT 'Tenant Irrigação inserido!';
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Tenants] WHERE [Dominio] = 'chinellato')
BEGIN
    INSERT INTO [dbo].[Tenants] ([Nome], [Dominio], [ConnectionString], [Ativo])
    VALUES (
        'Chinellato Transportes', 
        'chinellato', 
        'Server=SRVSQL\SQLEXPRESS;Database=ChinellatoTransportes;Trusted_Connection=True;TrustServerCertificate=True;',
        1
    );
    PRINT 'Tenant Chinellato inserido!';
END
GO

-- ============================================================================
-- 2. TABELA CLASSTRIB (Classificações Tributárias SVRS - IBS/CBS)
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ClassTrib')
BEGIN
    CREATE TABLE [dbo].[ClassTrib] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [CodigoClassTrib] VARCHAR(6) NOT NULL,
        [CodigoSituacaoTributaria] VARCHAR(3) NOT NULL,
        [DescricaoSituacaoTributaria] NVARCHAR(200) NULL,
        [DescricaoClassTrib] NVARCHAR(MAX) NOT NULL,
        [PercentualReducaoIBS] DECIMAL(8,5) NOT NULL DEFAULT 0,
        [PercentualReducaoCBS] DECIMAL(8,5) NOT NULL DEFAULT 0,
        [TipoAliquota] VARCHAR(50) NULL,
        [ValidoParaNFe] BIT NOT NULL DEFAULT 0,
        [TributacaoRegular] BIT NOT NULL DEFAULT 0,
        [CreditoPresumidoOperacoes] BIT NOT NULL DEFAULT 0,
        [EstornoCredito] BIT NOT NULL DEFAULT 0,
        [AnexoLegislacao] INT NULL,
        [LinkLegislacao] VARCHAR(500) NULL,
        [DataSincronizacao] DATETIME NOT NULL DEFAULT GETDATE(),
        [Ativo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_ClassTrib] PRIMARY KEY CLUSTERED ([Id])
    );
    
    -- Índice único no código
    CREATE UNIQUE INDEX [IX_ClassTrib_CodigoClassTrib] ON [dbo].[ClassTrib] ([CodigoClassTrib]);
    
    -- Índice para busca por CST
    CREATE INDEX [IX_ClassTrib_CST] ON [dbo].[ClassTrib] ([CodigoSituacaoTributaria]);
    
    -- Índice para busca por NFe válido
    CREATE INDEX [IX_ClassTrib_ValidoNFe] ON [dbo].[ClassTrib] ([ValidoParaNFe]) WHERE [ValidoParaNFe] = 1;
    
    PRINT 'Tabela ClassTrib criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela ClassTrib já existe.';
END
GO

-- ============================================================================
-- 3. ADICIONAR COLUNA ClassTribId NA TABELA Classificação Fiscal
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Classificação Fiscal]') AND name = N'ClassTribId')
BEGIN
    ALTER TABLE [dbo].[Classificação Fiscal] 
    ADD [ClassTribId] INT NULL;
    
    PRINT 'Coluna ClassTribId adicionada na tabela Classificação Fiscal!';
END
ELSE
BEGIN
    PRINT 'Coluna ClassTribId já existe na tabela Classificação Fiscal.';
END
GO

-- Criar FK (opcional - pode dar erro se tiver dados inconsistentes)
-- Descomente se quiser criar a constraint
/*
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ClassificacaoFiscal_ClassTrib')
BEGIN
    ALTER TABLE [dbo].[Classificação Fiscal]
    ADD CONSTRAINT [FK_ClassificacaoFiscal_ClassTrib] 
    FOREIGN KEY ([ClassTribId]) REFERENCES [dbo].[ClassTrib]([Id]);
    
    PRINT 'FK ClassTribId criada!';
END
GO
*/

-- ============================================================================
-- 4. ADICIONAR COLUNA PW~SenhaHash NA TABELA PW~Usuarios (para senhas novas)
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[PW~Usuarios]') AND name = N'PW~SenhaHash')
BEGIN
    ALTER TABLE [dbo].[PW~Usuarios] 
    ADD [PW~SenhaHash] VARCHAR(255) NULL;
    
    PRINT 'Coluna PW~SenhaHash adicionada na tabela PW~Usuarios!';
END
ELSE
BEGIN
    PRINT 'Coluna PW~SenhaHash já existe na tabela PW~Usuarios.';
END
GO

-- ============================================================================
-- 5. ÍNDICES PARA PERFORMANCE DO DASHBOARD (Opcional - Recomendado)
-- ============================================================================
-- ATENÇÃO: Pode demorar se a tabela Orçamento tiver muitos registros!
-- Execute separadamente se preferir

-- Índice para consultas por data de emissão
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orcamento_DataEmissao' AND object_id = OBJECT_ID('Orçamento'))
BEGIN
    CREATE INDEX [IX_Orcamento_DataEmissao] ON [dbo].[Orçamento] ([Data de Emissão]);
    PRINT 'Índice IX_Orcamento_DataEmissao criado!';
END
GO

-- Índice para consultas por status
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orcamento_Status' AND object_id = OBJECT_ID('Orçamento'))
BEGIN
    CREATE INDEX [IX_Orcamento_Status] ON [dbo].[Orçamento] ([Venda Fechada], [Cancelado]) INCLUDE ([Data de Emissão]);
    PRINT 'Índice IX_Orcamento_Status criado!';
END
GO

-- ============================================================================
-- VERIFICAÇÃO FINAL
-- ============================================================================
PRINT '';
PRINT '============================================';
PRINT 'VERIFICAÇÃO DAS TABELAS:';
PRINT '============================================';

SELECT 'Tenants' AS Tabela, COUNT(*) AS Registros FROM [dbo].[Tenants]
UNION ALL
SELECT 'ClassTrib', COUNT(*) FROM [dbo].[ClassTrib]
UNION ALL
SELECT 'Classificação Fiscal', COUNT(*) FROM [dbo].[Classificação Fiscal];

PRINT '';
PRINT 'Script executado com sucesso!';
PRINT 'Agora você pode sincronizar os ClassTribs pela interface do sistema.';
GO
