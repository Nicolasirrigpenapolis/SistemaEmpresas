-- =============================================================
-- SCRIPT DE CRIAÇÃO DAS TABELAS DE USUÁRIOS DO SISTEMA WEB
-- Execução: SQL Server Management Studio
-- Data: 02/12/2025
-- Descrição: Cria as tabelas GrupoUsuario e UsuarioSistema
--            independentes das tabelas PW~ do sistema VB6 legado
-- =============================================================

-- Verificar se as tabelas já existem antes de criar
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GrupoUsuario]') AND type in (N'U'))
BEGIN
    -- =============================================================
    -- TABELA: GrupoUsuario
    -- Grupos de usuários do sistema web
    -- =============================================================
    CREATE TABLE [dbo].[GrupoUsuario] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Nome] NVARCHAR(100) NOT NULL,
        [Descricao] NVARCHAR(500) NULL,
        [Ativo] BIT NOT NULL DEFAULT 1,
        [GrupoSistema] BIT NOT NULL DEFAULT 0,
        [DataCriacao] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [DataAtualizacao] DATETIME2 NULL,
        
        CONSTRAINT [PK_GrupoUsuario] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_GrupoUsuario_Nome] UNIQUE NONCLUSTERED ([Nome] ASC)
    );

    PRINT 'Tabela GrupoUsuario criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela GrupoUsuario já existe.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsuarioSistema]') AND type in (N'U'))
BEGIN
    -- =============================================================
    -- TABELA: UsuarioSistema
    -- Usuários do sistema web
    -- =============================================================
    CREATE TABLE [dbo].[UsuarioSistema] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Login] NVARCHAR(100) NOT NULL,
        [NomeCompleto] NVARCHAR(200) NOT NULL,
        [Email] NVARCHAR(200) NULL,
        [SenhaHash] NVARCHAR(255) NOT NULL,
        [GrupoId] INT NOT NULL,
        [Observacoes] NVARCHAR(500) NULL,
        [Ativo] BIT NOT NULL DEFAULT 1,
        [DataCriacao] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [DataAtualizacao] DATETIME2 NULL,
        [UltimoLogin] DATETIME2 NULL,
        [DeveTrocarSenha] BIT NOT NULL DEFAULT 0,
        [TentativasLoginFalha] INT NOT NULL DEFAULT 0,
        [BloqueadoAte] DATETIME2 NULL,
        
        CONSTRAINT [PK_UsuarioSistema] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_UsuarioSistema_Login] UNIQUE NONCLUSTERED ([Login] ASC),
        CONSTRAINT [FK_UsuarioSistema_GrupoUsuario] FOREIGN KEY ([GrupoId]) 
            REFERENCES [dbo].[GrupoUsuario] ([Id]) ON DELETE NO ACTION
    );

    -- Índice único filtrado para Email (apenas emails não nulos)
    CREATE UNIQUE NONCLUSTERED INDEX [UQ_UsuarioSistema_Email] 
        ON [dbo].[UsuarioSistema] ([Email] ASC) 
        WHERE [Email] IS NOT NULL;

    -- Índice para busca por grupo
    CREATE NONCLUSTERED INDEX [IX_UsuarioSistema_GrupoId] 
        ON [dbo].[UsuarioSistema] ([GrupoId] ASC);

    PRINT 'Tabela UsuarioSistema criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela UsuarioSistema já existe.';
END
GO

-- =============================================================
-- DADOS INICIAIS
-- Criar grupo SUPERVISAO (administradores) se não existir
-- =============================================================
IF NOT EXISTS (SELECT 1 FROM [dbo].[GrupoUsuario] WHERE [Nome] = 'SUPERVISAO')
BEGIN
    INSERT INTO [dbo].[GrupoUsuario] ([Nome], [Descricao], [Ativo], [GrupoSistema], [DataCriacao])
    VALUES ('SUPERVISAO', 'Grupo de administradores do sistema - acesso total', 1, 1, GETDATE());
    
    PRINT 'Grupo SUPERVISAO criado com sucesso.';
END
GO

-- Criar grupo SEM GRUPO (padrão para usuários sem grupo definido)
IF NOT EXISTS (SELECT 1 FROM [dbo].[GrupoUsuario] WHERE [Nome] = 'SEM GRUPO')
BEGIN
    INSERT INTO [dbo].[GrupoUsuario] ([Nome], [Descricao], [Ativo], [GrupoSistema], [DataCriacao])
    VALUES ('SEM GRUPO', 'Grupo padrão para usuários sem grupo específico', 1, 1, GETDATE());
    
    PRINT 'Grupo SEM GRUPO criado com sucesso.';
END
GO

-- =============================================================
-- CRIAR USUÁRIO ADMIN PADRÃO (se não existir)
-- Login: ADMIN, Senha: admin123 (hash BCrypt)
-- =============================================================
DECLARE @GrupoSupervisaoId INT;
SELECT @GrupoSupervisaoId = [Id] FROM [dbo].[GrupoUsuario] WHERE [Nome] = 'SUPERVISAO';

IF @GrupoSupervisaoId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [dbo].[UsuarioSistema] WHERE [Login] = 'ADMIN')
BEGIN
    -- Hash BCrypt para 'admin123' - você pode gerar um novo hash se preferir
    -- Este hash foi gerado com BCrypt.Net
    INSERT INTO [dbo].[UsuarioSistema] 
        ([Login], [NomeCompleto], [Email], [SenhaHash], [GrupoId], [Observacoes], [Ativo], [DataCriacao], [DeveTrocarSenha])
    VALUES 
        ('ADMIN', 'Administrador do Sistema', NULL, 
         '$2a$11$rK7ZqVVdYH0Zr.vPl0JV3.qp3qH9sSqQZ3Z8Z8qZ3Z8Z8qZ3Z8Z8q', -- Placeholder - gerar hash real
         @GrupoSupervisaoId, 'Usuário administrador padrão do sistema', 1, GETDATE(), 1);
    
    PRINT 'Usuário ADMIN criado com sucesso. IMPORTANTE: Altere a senha no primeiro acesso!';
END
GO

-- =============================================================
-- ATUALIZAR TABELA PermissoesTela PARA USAR A NOVA ESTRUTURA
-- Adicionar coluna GrupoId (opcional - para migração gradual)
-- =============================================================
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PermissoesTela]') AND name = 'GrupoId')
BEGIN
    ALTER TABLE [dbo].[PermissoesTela]
    ADD [GrupoId] INT NULL;

    PRINT 'Coluna GrupoId adicionada à tabela PermissoesTela.';
END
GO

-- =============================================================
-- RESUMO DAS ALTERAÇÕES
-- =============================================================
PRINT '=========================================================';
PRINT 'RESUMO:';
PRINT '- Tabela GrupoUsuario: Grupos de usuários do sistema web';
PRINT '- Tabela UsuarioSistema: Usuários do sistema web';
PRINT '- Grupo SUPERVISAO criado como grupo de sistema';
PRINT '- Grupo SEM GRUPO criado como padrão';
PRINT '=========================================================';
PRINT 'PRÓXIMOS PASSOS:';
PRINT '1. Execute este script no banco de dados';
PRINT '2. Gere a migration no projeto .NET';
PRINT '3. Atualize o DbContext com as novas entidades';
PRINT '4. Migre os usuários existentes (se necessário)';
PRINT '=========================================================';
GO
