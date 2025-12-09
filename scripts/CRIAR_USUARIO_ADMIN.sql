-- =====================================================
-- SCRIPT: Criar usuário ADMIN com grupo PROGRAMADOR
-- Data: 02/12/2025
-- Descrição: Cria usuário admin com todas as permissões
-- =====================================================

USE IRRIGACAO;
GO

-- =====================================================
-- 1. GARANTIR QUE O GRUPO PROGRAMADOR EXISTE EM AMBAS TABELAS
-- =====================================================
PRINT '=== Verificando grupo PROGRAMADOR ==='

-- Tabela nova (GrupoUsuario)
IF NOT EXISTS (SELECT 1 FROM GrupoUsuario WHERE Nome = 'PROGRAMADOR')
BEGIN
    INSERT INTO GrupoUsuario (Nome, Descricao, Ativo, GrupoSistema, DataCriacao)
    VALUES ('PROGRAMADOR', 'Grupo com permissões totais de administrador do sistema', 1, 1, GETDATE());
    PRINT 'Grupo PROGRAMADOR criado na tabela GrupoUsuario!';
END
ELSE
BEGIN
    PRINT 'Grupo PROGRAMADOR já existe na tabela GrupoUsuario.';
END

-- Tabela legada (PW~Grupos) - necessário para FK
-- Grupo PROGRAMADOR criptografado: EWMJchd5CXgFfhQebhNvEmoabR5uE28Sag==
IF NOT EXISTS (SELECT 1 FROM [PW~Grupos] WHERE [PW~Nome] = 'EWMJchd5CXgFfhQebhNvEmoabR5uE28Sag==')
BEGIN
    INSERT INTO [PW~Grupos] ([PW~Nome])
    VALUES ('EWMJchd5CXgFfhQebhNvEmoabR5uE28Sag==');
    PRINT 'Grupo PROGRAMADOR criado na tabela PW~Grupos (legado)!';
END
ELSE
BEGIN
    PRINT 'Grupo PROGRAMADOR já existe na tabela PW~Grupos.';
END

DECLARE @GrupoProgramadorId INT;
SELECT @GrupoProgramadorId = Id FROM GrupoUsuario WHERE Nome = 'PROGRAMADOR';
PRINT 'ID do grupo PROGRAMADOR: ' + CAST(@GrupoProgramadorId AS VARCHAR(10));
GO

-- =====================================================
-- 2. CRIAR USUÁRIO ADMIN
-- Nome: admin -> IFUrXCsTbxJqGm0ebhNvEmoabR5uE28Sag==
-- Senha: conectairrig@ -> Il4oUCZMJVAzQy9SBRNvEmoabR5uE28Sag==
-- Grupo: PROGRAMADOR -> EWMJchd5CXgFfhQebhNvEmoabR5uE28Sag==
-- =====================================================
PRINT ''
PRINT '=== Criando usuário ADMIN ==='

DECLARE @NomeCripto VARCHAR(100) = 'IFUrXCsTbxJqGm0ebhNvEmoabR5uE28Sag==';
DECLARE @SenhaCripto VARCHAR(100) = 'Il4oUCZMJVAzQy9SBRNvEmoabR5uE28Sag==';
DECLARE @GrupoCripto VARCHAR(100) = 'EWMJchd5CXgFfhQebhNvEmoabR5uE28Sag==';
DECLARE @GrupoId INT;

SELECT @GrupoId = Id FROM GrupoUsuario WHERE Nome = 'PROGRAMADOR';

IF NOT EXISTS (SELECT 1 FROM [PW~Usuarios] WHERE [PW~Nome] = @NomeCripto)
BEGIN
    INSERT INTO [PW~Usuarios] ([PW~Nome], [PW~Senha], [PW~Grupo], [PW~Ativo], [PW~Obs], GrupoUsuarioId)
    VALUES (@NomeCripto, @SenhaCripto, @GrupoCripto, 1, 'Administrador do sistema - criado em 02/12/2025', @GrupoId);
    PRINT 'Usuário ADMIN criado com sucesso!';
END
ELSE
BEGIN
    -- Atualizar usuário existente
    UPDATE [PW~Usuarios]
    SET [PW~Senha] = @SenhaCripto,
        [PW~Grupo] = @GrupoCripto,
        [PW~Ativo] = 1,
        [PW~Obs] = 'Administrador do sistema - atualizado em 02/12/2025',
        GrupoUsuarioId = @GrupoId
    WHERE [PW~Nome] = @NomeCripto;
    PRINT 'Usuário ADMIN atualizado!';
END
GO

-- =====================================================
-- 3. VERIFICAR RESULTADO
-- =====================================================
PRINT ''
PRINT '=== RESULTADO FINAL ==='
PRINT ''
PRINT '--- Grupos: ---'
SELECT Id, Nome, Descricao, Ativo, GrupoSistema FROM GrupoUsuario;

PRINT ''
PRINT '--- Usuário ADMIN: ---'
SELECT 
    u.[PW~Nome] AS NomeCripto,
    u.[PW~Ativo] AS Ativo,
    u.[PW~Obs] AS Observacoes,
    u.GrupoUsuarioId,
    g.Nome AS NomeGrupo
FROM [PW~Usuarios] u
LEFT JOIN GrupoUsuario g ON u.GrupoUsuarioId = g.Id
WHERE u.[PW~Nome] = 'IFUrXCsTbxJqGm0ebhNvEmoabR5uE28Sag==';

PRINT ''
PRINT '=== CREDENCIAIS DE ACESSO ==='
PRINT 'Usuario: admin'
PRINT 'Senha: conectairrig@'
PRINT 'Grupo: PROGRAMADOR (todas as permissões)'
PRINT ''
PRINT '=== Script executado com sucesso! ==='
GO
