-- =============================================================
-- SCRIPT DE CORREÇÃO - LIMPAR TABELAS DE USUÁRIOS NOVAS
-- Execução: SQL Server Management Studio
-- Data: 02/12/2025
-- Descrição: Limpa as tabelas GrupoUsuario e UsuarioSistema
--            para remover dados migrados incorretamente
-- =============================================================

USE IRRIGACAO; -- Altere para o banco correto se necessário
GO

BEGIN TRANSACTION;

BEGIN TRY
    -- 1. Limpar tabela de usuários (devido à FK)
    DELETE FROM [dbo].[UsuarioSistema];
    PRINT 'Tabela UsuarioSistema limpa.';

    -- 2. Limpar tabela de grupos
    DELETE FROM [dbo].[GrupoUsuario];
    PRINT 'Tabela GrupoUsuario limpa.';

    -- 3. Reinserir grupos padrão
    
    -- Grupo SUPERVISAO
    INSERT INTO [dbo].[GrupoUsuario] ([Nome], [Descricao], [Ativo], [GrupoSistema], [DataCriacao])
    VALUES ('SUPERVISAO', 'Grupo de administradores do sistema - acesso total', 1, 1, GETDATE());
    PRINT 'Grupo SUPERVISAO recriado.';

    -- Grupo SEM GRUPO
    INSERT INTO [dbo].[GrupoUsuario] ([Nome], [Descricao], [Ativo], [GrupoSistema], [DataCriacao])
    VALUES ('SEM GRUPO', 'Grupo padrão para usuários sem grupo específico', 1, 1, GETDATE());
    PRINT 'Grupo SEM GRUPO recriado.';

    COMMIT TRANSACTION;
    PRINT 'Correção concluída com sucesso!';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Erro ao executar correção: ' + ERROR_MESSAGE();
END CATCH
GO
