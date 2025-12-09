-- Script para criar a tabela Emitentes
-- Executar no banco de dados do tenant (Irrigacao, etc.)

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Emitentes')
BEGIN
    CREATE TABLE [dbo].[Emitentes] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [CNPJ] NVARCHAR(14) NOT NULL,
        [Razao_Social] NVARCHAR(60) NOT NULL,
        [Nome_Fantasia] NVARCHAR(60) NULL,
        [Inscricao_Estadual] NVARCHAR(20) NOT NULL,
        [Inscricao_Municipal] NVARCHAR(20) NULL,
        [CNAE] NVARCHAR(10) NULL,
        [Codigo_Regime_Tributario] INT NULL,
        [Endereco] NVARCHAR(100) NOT NULL,
        [Numero] NVARCHAR(10) NOT NULL,
        [Complemento] NVARCHAR(60) NULL,
        [Bairro] NVARCHAR(60) NOT NULL,
        [Codigo_Municipio] NVARCHAR(7) NOT NULL,
        [Municipio] NVARCHAR(60) NOT NULL,
        [UF] NVARCHAR(2) NOT NULL,
        [CEP] NVARCHAR(8) NOT NULL,
        [Codigo_Pais] NVARCHAR(4) NOT NULL DEFAULT '1058',
        [Pais] NVARCHAR(60) NOT NULL DEFAULT 'Brasil',
        [Telefone] NVARCHAR(14) NULL,
        [Email] NVARCHAR(255) NULL,
        [Ambiente_NFe] INT NOT NULL DEFAULT 2,
        [Serie_NFe] INT NOT NULL DEFAULT 1,
        [Proximo_Numero_NFe] INT NOT NULL DEFAULT 1,
        [Caminho_Certificado] NVARCHAR(500) NULL,
        [Senha_Certificado] NVARCHAR(500) NULL,
        [Validade_Certificado] DATETIME2 NULL,
        [Logo] NVARCHAR(MAX) NULL,
        [Ativo] BIT NOT NULL DEFAULT 1,
        [Data_Criacao] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [Data_Atualizacao] DATETIME2 NULL,
        [Data_Consulta_CNPJ] DATETIME2 NULL,
        CONSTRAINT [PK_Emitentes] PRIMARY KEY ([Id])
    );

    CREATE UNIQUE INDEX [IX_Emitentes_CNPJ] ON [dbo].[Emitentes] ([CNPJ]);

    PRINT 'Tabela Emitentes criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela Emitentes já existe!';
END
GO
