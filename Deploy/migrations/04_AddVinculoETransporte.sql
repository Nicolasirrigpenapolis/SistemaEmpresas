IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vinculo Produto Fornecedor]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[Vinculo Produto Fornecedor];
END
GO

CREATE TABLE [dbo].[Vinculo Produto Fornecedor](
    [Sequencia do Vinculo] [int] IDENTITY(1,1) NOT NULL,
    [Sequencia do Geral] [int] NOT NULL,
    [Codigo Produto Fornecedor] [varchar](60) NOT NULL,
    [Sequencia do Produto] [int] NOT NULL,
    [Data do Vinculo] [datetime] NOT NULL,
    CONSTRAINT [PK_Vinculo Produto Fornecedor] PRIMARY KEY CLUSTERED 
    (
        [Sequencia do Vinculo] ASC
    )
);
GO

CREATE INDEX [IX_Vinculo_Fornecedor_Codigo] ON [dbo].[Vinculo Produto Fornecedor] ([Sequencia do Geral], [Codigo Produto Fornecedor]);
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DespesasViagem]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DespesasViagem](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ViagemId] [int] NOT NULL,
        [TipoDespesa] [nvarchar](100) NOT NULL,
        [Descricao] [nvarchar](500) NULL,
        [Valor] [decimal](18, 2) NOT NULL,
        [DataDespesa] [datetime] NOT NULL,
        [NumeroDocumento] [nvarchar](50) NULL,
        [Local] [nvarchar](200) NULL,
        [KmAtual] [decimal](18, 2) NULL,
        [Litros] [decimal](18, 3) NULL,
        [Observacoes] [nvarchar](500) NULL,
        [Ativo] [bit] NOT NULL DEFAULT 1,
        [DataCriacao] [datetime] NOT NULL DEFAULT GETDATE(),
        [DataUltimaAlteracao] [datetime] NULL,
        CONSTRAINT [PK_DespesasViagem] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Adicionar FKs separadamente para evitar erros de encoding no script principal
-- O EF Core vai lidar com isso depois se necessário, o importante é a tabela existir

