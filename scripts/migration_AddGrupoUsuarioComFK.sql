-- Migration: AddGrupoUsuarioComFK
-- Data: 2024-12-02
-- Descricao: Cria tabela GrupoUsuario e adiciona FK em PW~Usuarios

-- 1. Criar tabela GrupoUsuario
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

    -- Indice unico no nome
    CREATE UNIQUE INDEX [IX_GrupoUsuario_Nome] ON [GrupoUsuario] ([Nome]);
    
    PRINT 'Tabela GrupoUsuario criada com sucesso';
END
ELSE
BEGIN
    PRINT 'Tabela GrupoUsuario ja existe';
END
GO

-- 2. Adicionar coluna GrupoUsuarioId na tabela PW~Usuarios (se nao existir)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[PW~Usuarios]') AND name = 'GrupoUsuarioId')
BEGIN
    ALTER TABLE [PW~Usuarios] ADD [GrupoUsuarioId] int NULL;
    
    -- Criar indice
    CREATE INDEX [IX_PW~Usuarios_GrupoUsuarioId] ON [PW~Usuarios] ([GrupoUsuarioId]);
    
    -- Criar FK
    ALTER TABLE [PW~Usuarios] 
    ADD CONSTRAINT [FK_PwUsuarios_GrupoUsuario] 
    FOREIGN KEY ([GrupoUsuarioId]) REFERENCES [GrupoUsuario] ([Id]) 
    ON DELETE SET NULL;
    
    PRINT 'Coluna GrupoUsuarioId adicionada em PW~Usuarios';
END
ELSE
BEGIN
    PRINT 'Coluna GrupoUsuarioId ja existe em PW~Usuarios';
END
GO

-- 3. Registrar na tabela de migrations do EF (opcional - para manter consistencia)
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251202192000_AddGrupoUsuarioComFK')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251202192000_AddGrupoUsuarioComFK', '8.0.0');
    
    PRINT 'Migration registrada em __EFMigrationsHistory';
END
GO

PRINT 'Migration AddGrupoUsuarioComFK executada com sucesso!';
