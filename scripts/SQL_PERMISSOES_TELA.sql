-- ============================================
-- Script para criar tabelas de Permissões por Tela
-- Sistema de Permissões Granulares
-- ============================================

-- Tabela de Templates de Permissões
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PermissoesTemplates')
BEGIN
    CREATE TABLE [PermissoesTemplates] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Nome] NVARCHAR(100) NOT NULL,
        [Descricao] NVARCHAR(500) NULL,
        [IsPadrao] BIT NOT NULL DEFAULT 0,
        [DataCriacao] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_PermissoesTemplates] PRIMARY KEY ([Id])
    );
    
    CREATE UNIQUE INDEX [IX_PermissoesTemplate_Nome] ON [PermissoesTemplates] ([Nome]);
    
    PRINT 'Tabela PermissoesTemplates criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela PermissoesTemplates já existe.';
END
GO

-- Tabela de Detalhes do Template
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PermissoesTemplateDetalhes')
BEGIN
    CREATE TABLE [PermissoesTemplateDetalhes] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [TemplateId] INT NOT NULL,
        [Modulo] NVARCHAR(100) NOT NULL,
        [Tela] NVARCHAR(100) NOT NULL,
        [Consultar] BIT NOT NULL DEFAULT 0,
        [Incluir] BIT NOT NULL DEFAULT 0,
        [Alterar] BIT NOT NULL DEFAULT 0,
        [Excluir] BIT NOT NULL DEFAULT 0,
        CONSTRAINT [PK_PermissoesTemplateDetalhes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PermissoesTemplateDetalhe_Template] FOREIGN KEY ([TemplateId]) 
            REFERENCES [PermissoesTemplates]([Id]) ON DELETE CASCADE
    );
    
    CREATE UNIQUE INDEX [IX_PermissoesTemplateDetalhe_Template_Tela] 
        ON [PermissoesTemplateDetalhes] ([TemplateId], [Tela]);
    
    PRINT 'Tabela PermissoesTemplateDetalhes criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela PermissoesTemplateDetalhes já existe.';
END
GO

-- Tabela de Permissões por Tela (por grupo)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PermissoesTelas')
BEGIN
    CREATE TABLE [PermissoesTelas] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Grupo] NVARCHAR(100) NOT NULL,  -- Nome do grupo criptografado (como PW~Grupos)
        [Modulo] NVARCHAR(100) NOT NULL,
        [Tela] NVARCHAR(100) NOT NULL,
        [NomeTela] NVARCHAR(200) NOT NULL,
        [Rota] NVARCHAR(200) NOT NULL,
        [Consultar] BIT NOT NULL DEFAULT 0,
        [Incluir] BIT NOT NULL DEFAULT 0,
        [Alterar] BIT NOT NULL DEFAULT 0,
        [Excluir] BIT NOT NULL DEFAULT 0,
        [Ordem] INT NOT NULL DEFAULT 0,
        CONSTRAINT [PK_PermissoesTelas] PRIMARY KEY ([Id])
    );
    
    CREATE UNIQUE INDEX [IX_PermissoesTela_Grupo_Tela] 
        ON [PermissoesTelas] ([Grupo], [Tela]);
    
    PRINT 'Tabela PermissoesTelas criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela PermissoesTelas já existe.';
END
GO

-- Adicionar coluna PW~Ativo na tabela PW~Usuarios se não existir
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'PW~Usuarios' AND COLUMN_NAME = 'PW~Ativo')
BEGIN
    ALTER TABLE [PW~Usuarios] ADD [PW~Ativo] BIT NOT NULL DEFAULT 1;
    PRINT 'Coluna PW~Ativo adicionada à tabela PW~Usuarios!';
END
ELSE
BEGIN
    PRINT 'Coluna PW~Ativo já existe na tabela PW~Usuarios.';
END
GO

-- ============================================
-- Inserir Templates Padrão
-- ============================================

-- Template Administrador (acesso total)
IF NOT EXISTS (SELECT * FROM [PermissoesTemplates] WHERE [Nome] = 'Administrador')
BEGIN
    INSERT INTO [PermissoesTemplates] ([Nome], [Descricao], [IsPadrao], [DataCriacao])
    VALUES ('Administrador', 'Acesso total a todas as telas do sistema', 1, GETDATE());
    
    DECLARE @AdminId INT = SCOPE_IDENTITY();
    
    -- Inserir todas as telas com acesso total
    INSERT INTO [PermissoesTemplateDetalhes] ([TemplateId], [Modulo], [Tela], [Consultar], [Incluir], [Alterar], [Excluir])
    VALUES 
        (@AdminId, 'Cadastros', 'Clientes', 1, 1, 1, 1),
        (@AdminId, 'Cadastros', 'Fornecedores', 1, 1, 1, 1),
        (@AdminId, 'Cadastros', 'Produtos', 1, 1, 1, 1),
        (@AdminId, 'Cadastros', 'Vendedores', 1, 1, 1, 1),
        (@AdminId, 'Cadastros', 'Transportadoras', 1, 1, 1, 1),
        (@AdminId, 'Estoque', 'MovimentoEstoque', 1, 1, 1, 1),
        (@AdminId, 'Estoque', 'Inventario', 1, 1, 1, 1),
        (@AdminId, 'Estoque', 'ConsultaEstoque', 1, 1, 1, 1),
        (@AdminId, 'Comercial', 'Orcamentos', 1, 1, 1, 1),
        (@AdminId, 'Comercial', 'Pedidos', 1, 1, 1, 1),
        (@AdminId, 'Comercial', 'OrdensServico', 1, 1, 1, 1),
        (@AdminId, 'Fiscal', 'NotasFiscais', 1, 1, 1, 1),
        (@AdminId, 'Fiscal', 'ClassificacaoFiscal', 1, 1, 1, 1),
        (@AdminId, 'Fiscal', 'ClassTrib', 1, 1, 1, 1),
        (@AdminId, 'Financeiro', 'ContasReceber', 1, 1, 1, 1),
        (@AdminId, 'Financeiro', 'ContasPagar', 1, 1, 1, 1),
        (@AdminId, 'Financeiro', 'FluxoCaixa', 1, 1, 1, 1),
        (@AdminId, 'Sistema', 'Usuarios', 1, 1, 1, 1),
        (@AdminId, 'Sistema', 'Configuracoes', 1, 1, 1, 1),
        (@AdminId, 'Sistema', 'Logs', 1, 1, 1, 1);
    
    PRINT 'Template Administrador criado com sucesso!';
END
GO

-- Template Somente Consulta
IF NOT EXISTS (SELECT * FROM [PermissoesTemplates] WHERE [Nome] = 'Somente Consulta')
BEGIN
    INSERT INTO [PermissoesTemplates] ([Nome], [Descricao], [IsPadrao], [DataCriacao])
    VALUES ('Somente Consulta', 'Acesso apenas para consultar dados, sem permissão de alteração', 1, GETDATE());
    
    DECLARE @ConsultaId INT = SCOPE_IDENTITY();
    
    -- Inserir todas as telas apenas com consulta
    INSERT INTO [PermissoesTemplateDetalhes] ([TemplateId], [Modulo], [Tela], [Consultar], [Incluir], [Alterar], [Excluir])
    VALUES 
        (@ConsultaId, 'Cadastros', 'Clientes', 1, 0, 0, 0),
        (@ConsultaId, 'Cadastros', 'Fornecedores', 1, 0, 0, 0),
        (@ConsultaId, 'Cadastros', 'Produtos', 1, 0, 0, 0),
        (@ConsultaId, 'Cadastros', 'Vendedores', 1, 0, 0, 0),
        (@ConsultaId, 'Cadastros', 'Transportadoras', 1, 0, 0, 0),
        (@ConsultaId, 'Estoque', 'MovimentoEstoque', 1, 0, 0, 0),
        (@ConsultaId, 'Estoque', 'Inventario', 1, 0, 0, 0),
        (@ConsultaId, 'Estoque', 'ConsultaEstoque', 1, 0, 0, 0),
        (@ConsultaId, 'Comercial', 'Orcamentos', 1, 0, 0, 0),
        (@ConsultaId, 'Comercial', 'Pedidos', 1, 0, 0, 0),
        (@ConsultaId, 'Comercial', 'OrdensServico', 1, 0, 0, 0),
        (@ConsultaId, 'Fiscal', 'NotasFiscais', 1, 0, 0, 0),
        (@ConsultaId, 'Fiscal', 'ClassificacaoFiscal', 1, 0, 0, 0),
        (@ConsultaId, 'Fiscal', 'ClassTrib', 1, 0, 0, 0),
        (@ConsultaId, 'Financeiro', 'ContasReceber', 1, 0, 0, 0),
        (@ConsultaId, 'Financeiro', 'ContasPagar', 1, 0, 0, 0),
        (@ConsultaId, 'Financeiro', 'FluxoCaixa', 1, 0, 0, 0);
    
    PRINT 'Template Somente Consulta criado com sucesso!';
END
GO

-- Template Comercial
IF NOT EXISTS (SELECT * FROM [PermissoesTemplates] WHERE [Nome] = 'Comercial')
BEGIN
    INSERT INTO [PermissoesTemplates] ([Nome], [Descricao], [IsPadrao], [DataCriacao])
    VALUES ('Comercial', 'Acesso às áreas comerciais e cadastros básicos', 1, GETDATE());
    
    DECLARE @ComercialId INT = SCOPE_IDENTITY();
    
    INSERT INTO [PermissoesTemplateDetalhes] ([TemplateId], [Modulo], [Tela], [Consultar], [Incluir], [Alterar], [Excluir])
    VALUES 
        (@ComercialId, 'Cadastros', 'Clientes', 1, 1, 1, 0),
        (@ComercialId, 'Cadastros', 'Produtos', 1, 0, 0, 0),
        (@ComercialId, 'Comercial', 'Orcamentos', 1, 1, 1, 1),
        (@ComercialId, 'Comercial', 'Pedidos', 1, 1, 1, 0),
        (@ComercialId, 'Comercial', 'OrdensServico', 1, 0, 0, 0),
        (@ComercialId, 'Estoque', 'ConsultaEstoque', 1, 0, 0, 0);
    
    PRINT 'Template Comercial criado com sucesso!';
END
GO

-- Template Financeiro
IF NOT EXISTS (SELECT * FROM [PermissoesTemplates] WHERE [Nome] = 'Financeiro')
BEGIN
    INSERT INTO [PermissoesTemplates] ([Nome], [Descricao], [IsPadrao], [DataCriacao])
    VALUES ('Financeiro', 'Acesso às áreas financeiras', 1, GETDATE());
    
    DECLARE @FinanceiroId INT = SCOPE_IDENTITY();
    
    INSERT INTO [PermissoesTemplateDetalhes] ([TemplateId], [Modulo], [Tela], [Consultar], [Incluir], [Alterar], [Excluir])
    VALUES 
        (@FinanceiroId, 'Cadastros', 'Clientes', 1, 0, 0, 0),
        (@FinanceiroId, 'Cadastros', 'Fornecedores', 1, 0, 0, 0),
        (@FinanceiroId, 'Financeiro', 'ContasReceber', 1, 1, 1, 1),
        (@FinanceiroId, 'Financeiro', 'ContasPagar', 1, 1, 1, 1),
        (@FinanceiroId, 'Financeiro', 'FluxoCaixa', 1, 0, 0, 0);
    
    PRINT 'Template Financeiro criado com sucesso!';
END
GO

PRINT '============================================';
PRINT 'Script de criação de tabelas de permissões executado com sucesso!';
PRINT '============================================';
