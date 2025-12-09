BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
BEGIN
    ALTER TABLE [PW~Usuarios] ADD [PW~Ativo] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
BEGIN
    CREATE TABLE [PermissoesTemplate] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        [Descricao] nvarchar(500) NULL,
        [IsPadrao] bit NOT NULL,
        [DataCriacao] datetime2 NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT [PK_PermissoesTemplate] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PermissoesTela_Grupo_Tela] ON [PermissoesTela] ([Grupo], [Tela]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PermissoesTemplate_Nome] ON [PermissoesTemplate] ([Nome]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PermissoesTemplateDetalhe_Template_Tela] ON [PermissoesTemplateDetalhe] ([TemplateId], [Tela]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128133523_AddPermissoesTelas'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251128133523_AddPermissoesTelas', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251202191622_SyncGrupoUsuarioFK'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251202191622_SyncGrupoUsuarioFK', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    CREATE INDEX [IX_LogsAuditoria_DataHora] ON [LogsAuditoria] ([DataHora]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    CREATE INDEX [IX_LogsAuditoria_Entidade] ON [LogsAuditoria] ([Entidade], [EntidadeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    CREATE INDEX [IX_LogsAuditoria_Modulo] ON [LogsAuditoria] ([Modulo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    CREATE INDEX [IX_LogsAuditoria_Tenant] ON [LogsAuditoria] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    CREATE INDEX [IX_LogsAuditoria_TipoAcao] ON [LogsAuditoria] ([TipoAcao]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    CREATE INDEX [IX_LogsAuditoria_Usuario] ON [LogsAuditoria] ([UsuarioCodigo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251203194014_CreateLogsAuditoria'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251203194014_CreateLogsAuditoria', N'8.0.0');
END;
GO

COMMIT;
GO

