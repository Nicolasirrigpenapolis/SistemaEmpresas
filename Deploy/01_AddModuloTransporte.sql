BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_DespesasViagem_DataDespesa] ON [DespesasViagem] ([DataDespesa]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_DespesasViagem_TipoDespesa] ON [DespesasViagem] ([TipoDespesa]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_DespesasViagem_ViagemId] ON [DespesasViagem] ([ViagemId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_ManutencoesPeca_ManutencaoId] ON [ManutencoesPeca] ([ManutencaoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_ManutencoesVeiculo_DataManutencao] ON [ManutencoesVeiculo] ([DataManutencao]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_ManutencoesVeiculo_FornecedorId] ON [ManutencoesVeiculo] ([FornecedorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_ManutencoesVeiculo_VeiculoId] ON [ManutencoesVeiculo] ([VeiculoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Reboques_Placa] ON [Reboques] ([Placa]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_ReceitasViagem_DataReceita] ON [ReceitasViagem] ([DataReceita]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_ReceitasViagem_ViagemId] ON [ReceitasViagem] ([ViagemId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Veiculos_Placa] ON [Veiculos] ([Placa]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_Viagens_DataInicio] ON [Viagens] ([DataInicio]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_Viagens_MotoristaId] ON [Viagens] ([MotoristaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_Viagens_ReboqueId] ON [Viagens] ([ReboqueId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    CREATE INDEX [IX_Viagens_VeiculoId] ON [Viagens] ([VeiculoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209161029_AddModuloTransporte'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251209161029_AddModuloTransporte', N'8.0.0');
END;
GO

COMMIT;
GO

