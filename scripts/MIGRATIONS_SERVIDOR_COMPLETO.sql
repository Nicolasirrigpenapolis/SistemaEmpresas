-- ============================================
-- SCRIPT COMPLETO DE MIGRATIONS
-- Para servidor que já tem as tabelas criadas manualmente
-- ============================================

-- ================================================
-- PARTE 1: REGISTRAR MIGRATIONS JÁ APLICADAS
-- (Tabelas já existem, só precisa registrar)
-- ================================================

PRINT '=== PARTE 1: Registrando migrations já aplicadas ==='

-- Registrar CreateAllTables (tabelas do sistema já existem)
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111183501_CreateAllTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251111183501_CreateAllTables', N'8.0.0');
    PRINT '   [OK] 20251111183501_CreateAllTables registrada'
END
ELSE
    PRINT '   [--] 20251111183501_CreateAllTables já existe'
GO

-- Registrar CriacaoClassTrib (tabela ClassTrib já existe)
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251125132336_CriacaoClassTrib'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251125132336_CriacaoClassTrib', N'8.0.0');
    PRINT '   [OK] 20251125132336_CriacaoClassTrib registrada'
END
ELSE
    PRINT '   [--] 20251125132336_CriacaoClassTrib já existe'
GO

PRINT ''
PRINT '=== PARTE 2: Criando tabela GrupoUsuario e FK ==='
PRINT ''

-- ================================================
-- PARTE 2: TABELA GrupoUsuario (script manual)
-- ================================================

-- Criar tabela GrupoUsuario
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'GrupoUsuario')
BEGIN
    CREATE TABLE [GrupoUsuario] (
        [Id] int NOT NULL IDENTITY(1,1),
        [Nome] nvarchar(100) NOT NULL,
        [Descricao] nvarchar(500) NULL,
        [Ativo] bit NOT NULL DEFAULT 1,
        [GrupoSistema] bit NOT NULL DEFAULT 0,
        [DataCriacao] datetime2 NOT NULL DEFAULT GETDATE(),
        [DataAtualizacao] datetime2 NULL,
        CONSTRAINT [PK_GrupoUsuario] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_GrupoUsuario_Nome] ON [GrupoUsuario] ([Nome]);
    PRINT '   [OK] Tabela GrupoUsuario criada'
END
ELSE
    PRINT '   [--] Tabela GrupoUsuario já existe'
GO

-- Adicionar coluna GrupoUsuarioId na tabela PW~Usuarios (se não existir)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[PW~Usuarios]') AND name = 'GrupoUsuarioId')
BEGIN
    ALTER TABLE [PW~Usuarios] ADD [GrupoUsuarioId] int NULL;
    PRINT '   [OK] Coluna GrupoUsuarioId adicionada em PW~Usuarios'
END
ELSE
    PRINT '   [--] Coluna GrupoUsuarioId já existe em PW~Usuarios'
GO

-- Criar índice (se não existir)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PW~Usuarios_GrupoUsuarioId')
BEGIN
    CREATE INDEX [IX_PW~Usuarios_GrupoUsuarioId] ON [PW~Usuarios] ([GrupoUsuarioId]);
    PRINT '   [OK] Índice IX_PW~Usuarios_GrupoUsuarioId criado'
END
GO

-- Criar FK (se não existir)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PwUsuarios_GrupoUsuario')
BEGIN
    ALTER TABLE [PW~Usuarios] 
    ADD CONSTRAINT [FK_PwUsuarios_GrupoUsuario] 
    FOREIGN KEY ([GrupoUsuarioId]) REFERENCES [GrupoUsuario] ([Id]) 
    ON DELETE SET NULL;
    PRINT '   [OK] FK FK_PwUsuarios_GrupoUsuario criada'
END
ELSE
    PRINT '   [--] FK FK_PwUsuarios_GrupoUsuario já existe'
GO

PRINT ''
PRINT '=== PARTE 3: Aplicando migrations NOVAS ==='
PRINT ''

-- ================================================
-- PARTE 3: APLICAR MIGRATIONS NOVAS
-- ================================================

-- ================================================
-- Migration: 20251128133523_AddPermissoesTelas
-- ================================================

BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
BEGIN
    PRINT '   Aplicando 20251128133523_AddPermissoesTelas...'
    
    -- Adicionar coluna PW~Ativo (se não existir)
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PW~Usuarios' AND COLUMN_NAME = 'PW~Ativo')
    BEGIN
        ALTER TABLE [PW~Usuarios] ADD [PW~Ativo] bit NOT NULL DEFAULT CAST(1 AS bit);
        PRINT '      - Coluna PW~Ativo adicionada'
    END

    -- Criar tabela PermissoesTela
    IF OBJECT_ID('PermissoesTela', 'U') IS NULL
    BEGIN
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
        PRINT '      - Tabela PermissoesTela criada'
    END

    -- Criar tabela PermissoesTemplate
    IF OBJECT_ID('PermissoesTemplate', 'U') IS NULL
    BEGIN
        CREATE TABLE [PermissoesTemplate] (
            [Id] int NOT NULL IDENTITY,
            [Nome] nvarchar(100) NOT NULL,
            [Descricao] nvarchar(500) NULL,
            [IsPadrao] bit NOT NULL,
            [DataCriacao] datetime2 NOT NULL DEFAULT (GETDATE()),
            CONSTRAINT [PK_PermissoesTemplate] PRIMARY KEY ([Id])
        );
        PRINT '      - Tabela PermissoesTemplate criada'
    END

    -- Criar tabela PermissoesTemplateDetalhe
    IF OBJECT_ID('PermissoesTemplateDetalhe', 'U') IS NULL
    BEGIN
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
        PRINT '      - Tabela PermissoesTemplateDetalhe criada'
    END

    -- Criar índices (se não existirem)
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PermissoesTela_Grupo_Tela')
        CREATE UNIQUE INDEX [IX_PermissoesTela_Grupo_Tela] ON [PermissoesTela] ([Grupo], [Tela]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PermissoesTemplate_Nome')
        CREATE UNIQUE INDEX [IX_PermissoesTemplate_Nome] ON [PermissoesTemplate] ([Nome]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PermissoesTemplateDetalhe_Template_Tela')
        CREATE UNIQUE INDEX [IX_PermissoesTemplateDetalhe_Template_Tela] ON [PermissoesTemplateDetalhe] ([TemplateId], [Tela]);

    -- Registrar migration
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251128133523_AddPermissoesTelas', N'8.0.0');
    
    PRINT '   [OK] 20251128133523_AddPermissoesTelas aplicada'
END
ELSE
    PRINT '   [--] 20251128133523_AddPermissoesTelas já existe'

COMMIT;
GO

-- ================================================
-- Migration: 20251202191622_SyncGrupoUsuarioFK (vazia)
-- ================================================

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251202191622_SyncGrupoUsuarioFK'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251202191622_SyncGrupoUsuarioFK', N'8.0.0');
    PRINT '   [OK] 20251202191622_SyncGrupoUsuarioFK registrada'
END
ELSE
    PRINT '   [--] 20251202191622_SyncGrupoUsuarioFK já existe'
GO

-- ================================================
-- Migration: 20251203194014_CreateLogsAuditoria
-- ================================================

BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    PRINT '   Aplicando 20251203194014_CreateLogsAuditoria...'

    -- Criar tabela LogsAuditoria
    IF OBJECT_ID('LogsAuditoria', 'U') IS NULL
    BEGIN
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
        PRINT '      - Tabela LogsAuditoria criada'
    END

    -- Criar índices
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogsAuditoria_DataHora')
        CREATE INDEX [IX_LogsAuditoria_DataHora] ON [LogsAuditoria] ([DataHora]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Entidade')
        CREATE INDEX [IX_LogsAuditoria_Entidade] ON [LogsAuditoria] ([Entidade], [EntidadeId]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Modulo')
        CREATE INDEX [IX_LogsAuditoria_Modulo] ON [LogsAuditoria] ([Modulo]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Tenant')
        CREATE INDEX [IX_LogsAuditoria_Tenant] ON [LogsAuditoria] ([TenantId]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogsAuditoria_TipoAcao')
        CREATE INDEX [IX_LogsAuditoria_TipoAcao] ON [LogsAuditoria] ([TipoAcao]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Usuario')
        CREATE INDEX [IX_LogsAuditoria_Usuario] ON [LogsAuditoria] ([UsuarioCodigo]);

    -- Registrar migration
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251203194014_CreateLogsAuditoria', N'8.0.0');
    
    PRINT '   [OK] 20251203194014_CreateLogsAuditoria aplicada'
END
ELSE
    PRINT '   [--] 20251203194014_CreateLogsAuditoria já existe'

COMMIT;
GO

-- ================================================
-- VERIFICAÇÃO FINAL
-- ================================================

PRINT ''
PRINT '=== VERIFICAÇÃO FINAL ==='
PRINT ''
SELECT MigrationId, ProductVersion FROM [__EFMigrationsHistory] ORDER BY MigrationId
PRINT ''
PRINT '=== SCRIPT CONCLUÍDO COM SUCESSO! ==='
