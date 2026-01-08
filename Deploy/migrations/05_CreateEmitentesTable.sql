IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Emitentes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Emitentes](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [CNPJ] [varchar](14) NOT NULL,
        [Razao_Social] [varchar](60) NOT NULL,
        [Nome_Fantasia] [varchar](60) NULL,
        [Inscricao_Estadual] [varchar](20) NOT NULL,
        [Inscricao_Municipal] [varchar](20) NULL,
        [CNAE] [varchar](10) NULL,
        [Codigo_Regime_Tributario] [int] NULL,
        [Endereco] [varchar](100) NOT NULL,
        [Numero] [varchar](10) NOT NULL,
        [Complemento] [varchar](60) NULL,
        [Bairro] [varchar](60) NOT NULL,
        [Codigo_Municipio] [varchar](7) NOT NULL,
        [Municipio] [varchar](60) NOT NULL,
        [UF] [varchar](2) NOT NULL,
        [CEP] [varchar](8) NOT NULL,
        [Codigo_Pais] [varchar](4) NULL DEFAULT '1058',
        [Pais] [varchar](60) NULL DEFAULT 'Brasil',
        [Telefone] [varchar](14) NULL,
        [Email] [varchar](255) NULL,
        [Ambiente_NFe] [int] NOT NULL DEFAULT 2,
        [Serie_NFe] [int] NOT NULL DEFAULT 1,
        [Proximo_Numero_NFe] [int] NOT NULL DEFAULT 1,
        [Caminho_Certificado] [varchar](500) NULL,
        [Senha_Certificado] [varchar](500) NULL,
        [Validade_Certificado] [datetime] NULL,
        [Ativo] [bit] NOT NULL DEFAULT 1,
        [Data_Criacao] [datetime] NOT NULL DEFAULT GETDATE(),
        [Data_Atualizacao] [datetime] NULL,
        [Data_Consulta_CNPJ] [datetime] NULL,
        CONSTRAINT [PK_Emitentes] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE UNIQUE INDEX [IX_Emitentes_CNPJ] ON [dbo].[Emitentes] ([CNPJ]);
END
GO

-- Inserir um emitente padrão para teste (opcional, mas ajuda a evitar o erro de 'nenhum emitente ativo')
IF NOT EXISTS (SELECT * FROM [dbo].[Emitentes])
BEGIN
    INSERT INTO [dbo].[Emitentes] 
    ([CNPJ], [Razao_Social], [Nome_Fantasia], [Inscricao_Estadual], [Endereco], [Numero], [Bairro], [Codigo_Municipio], [Municipio], [UF], [CEP])
    VALUES 
    ('00000000000000', 'EMPRESA TESTE LTDA', 'EMPRESA TESTE', 'ISENTO', 'RUA TESTE', '123', 'CENTRO', '3537305', 'PENAPOLIS', 'SP', '16300000');
END
GO
