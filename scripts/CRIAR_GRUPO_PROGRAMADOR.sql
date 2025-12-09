-- =====================================================
-- SCRIPT: Criar grupo PROGRAMADOR e vincular usuário NICOLAS
-- Data: 02/12/2025
-- Descrição: Cria o grupo PROGRAMADOR na nova tabela GrupoUsuario
--            e vincula o usuário NICOLAS a esse grupo
-- =====================================================

USE IRRIGACAO;
GO

-- =====================================================
-- 1. VERIFICAR SE O GRUPO PROGRAMADOR JÁ EXISTE
-- =====================================================
PRINT '=== Verificando grupos existentes ==='
SELECT Id, Nome, Descricao, Ativo, GrupoSistema FROM GrupoUsuario;
GO

-- =====================================================
-- 2. CRIAR O GRUPO PROGRAMADOR (SE NÃO EXISTIR)
-- =====================================================
PRINT '=== Criando grupo PROGRAMADOR ==='

IF NOT EXISTS (SELECT 1 FROM GrupoUsuario WHERE Nome = 'PROGRAMADOR')
BEGIN
    INSERT INTO GrupoUsuario (Nome, Descricao, Ativo, GrupoSistema, DataCriacao)
    VALUES ('PROGRAMADOR', 'Grupo com permissões totais de administrador do sistema', 1, 1, GETDATE());
    
    PRINT 'Grupo PROGRAMADOR criado com sucesso!';
END
ELSE
BEGIN
    PRINT 'Grupo PROGRAMADOR já existe.';
END
GO

-- =====================================================
-- 3. OBTER O ID DO GRUPO PROGRAMADOR
-- =====================================================
DECLARE @GrupoProgramadorId INT;
SELECT @GrupoProgramadorId = Id FROM GrupoUsuario WHERE Nome = 'PROGRAMADOR';
PRINT 'ID do grupo PROGRAMADOR: ' + CAST(@GrupoProgramadorId AS VARCHAR(10));
GO

-- =====================================================
-- 4. VINCULAR NICOLAS AO GRUPO PROGRAMADOR
-- =====================================================
PRINT '=== Vinculando NICOLAS ao grupo PROGRAMADOR ==='

DECLARE @GrupoId INT;
SELECT @GrupoId = Id FROM GrupoUsuario WHERE Nome = 'PROGRAMADOR';

-- Atualizar o usuário NICOLAS
IF EXISTS (SELECT 1 FROM [PW~Usuarios] WHERE [PW~Nome] = 'NICOLAS')
BEGIN
    UPDATE [PW~Usuarios]
    SET GrupoUsuarioId = @GrupoId
    WHERE [PW~Nome] = 'NICOLAS';
    
    PRINT 'Usuário NICOLAS vinculado ao grupo PROGRAMADOR com sucesso!';
END
ELSE
BEGIN
    PRINT 'AVISO: Usuário NICOLAS não encontrado na tabela PW~Usuarios!';
END
GO

-- =====================================================
-- 5. VERIFICAR RESULTADO
-- =====================================================
PRINT '=== Verificando resultado ==='
PRINT ''
PRINT '--- Grupos cadastrados: ---'
SELECT Id, Nome, Descricao, Ativo, GrupoSistema, DataCriacao FROM GrupoUsuario;

PRINT ''
PRINT '--- Usuário NICOLAS: ---'
SELECT 
    u.[PW~Nome] AS Nome,
    u.[PW~Ativo] AS Ativo,
    u.GrupoUsuarioId,
    g.Nome AS NomeGrupo
FROM [PW~Usuarios] u
LEFT JOIN GrupoUsuario g ON u.GrupoUsuarioId = g.Id
WHERE u.[PW~Nome] = 'NICOLAS';
GO

-- =====================================================
-- 6. (OPCIONAL) VINCULAR OUTROS USUÁRIOS AO GRUPO PROGRAMADOR
-- Descomente as linhas abaixo se precisar vincular outros usuários
-- =====================================================
/*
DECLARE @GrupoId INT;
SELECT @GrupoId = Id FROM GrupoUsuario WHERE Nome = 'PROGRAMADOR';

UPDATE [PW~Usuarios]
SET GrupoUsuarioId = @GrupoId
WHERE [PW~Nome] IN ('OUTRO_USUARIO1', 'OUTRO_USUARIO2');
*/

PRINT ''
PRINT '=== Script executado com sucesso! ==='
GO
