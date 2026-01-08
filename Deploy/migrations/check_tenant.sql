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

CREATE TABLE [Tenants] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Dominio] nvarchar(200) NOT NULL,
    [ConnectionString] nvarchar(500) NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Tenants] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'ConnectionString', N'Dominio', N'Nome') AND [object_id] = OBJECT_ID(N'[Tenants]'))
    SET IDENTITY_INSERT [Tenants] ON;
INSERT INTO [Tenants] ([Id], [Ativo], [ConnectionString], [Dominio], [Nome])
VALUES (1, CAST(1 AS bit), N'Server=DESKTOP-CHS14C0\SQLIRRIGACAO;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;', N'irrigacao', N'Irrigação Penápolis');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'ConnectionString', N'Dominio', N'Nome') AND [object_id] = OBJECT_ID(N'[Tenants]'))
    SET IDENTITY_INSERT [Tenants] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'ConnectionString', N'Dominio', N'Nome') AND [object_id] = OBJECT_ID(N'[Tenants]'))
    SET IDENTITY_INSERT [Tenants] ON;
INSERT INTO [Tenants] ([Id], [Ativo], [ConnectionString], [Dominio], [Nome])
VALUES (2, CAST(1 AS bit), N'Server=DESKTOP-CHS14C0\SQLIRRIGACAO;Database=ChinellatoTransportes;Trusted_Connection=True;TrustServerCertificate=True;', N'chinellato', N'Chinellato Transportes');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'ConnectionString', N'Dominio', N'Nome') AND [object_id] = OBJECT_ID(N'[Tenants]'))
    SET IDENTITY_INSERT [Tenants] OFF;
GO

CREATE UNIQUE INDEX [IX_Tenants_Dominio] ON [Tenants] ([Dominio]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251104134513_InitialTenants', N'8.0.0');
GO

COMMIT;
GO

