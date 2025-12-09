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

