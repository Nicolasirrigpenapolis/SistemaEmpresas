-- ============================================
-- Script para adicionar permissão "Dados do Emitente"
-- Para todos os grupos existentes
-- Data: 04/12/2025
-- ============================================

PRINT 'Iniciando adição da permissão Dados do Emitente...'
GO

-- Verificar se a tabela PermissoesTela existe
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PermissoesTela')
BEGIN
    -- Adicionar a permissão DadosEmitente para todos os grupos existentes que ainda não possuem
    INSERT INTO [PermissoesTela] ([Grupo], [Modulo], [Tela], [NomeTela], [Rota], [Consultar], [Incluir], [Alterar], [Excluir], [Ordem])
    SELECT DISTINCT 
        pt.[Grupo],
        'Sistema' AS [Modulo],
        'DadosEmitente' AS [Tela],
        'Dados do Emitente' AS [NomeTela],
        '/emitente' AS [Rota],
        1 AS [Consultar],
        1 AS [Incluir],
        1 AS [Alterar],
        1 AS [Excluir],
        0 AS [Ordem]
    FROM [PermissoesTela] pt
    WHERE NOT EXISTS (
        SELECT 1 
        FROM [PermissoesTela] pe 
        WHERE pe.[Grupo] = pt.[Grupo] 
        AND pe.[Tela] = 'DadosEmitente'
    )
    GROUP BY pt.[Grupo];

    DECLARE @RowsAffected INT = @@ROWCOUNT;
    PRINT CONCAT('Permissão DadosEmitente adicionada para ', @RowsAffected, ' grupo(s).');
END
ELSE
BEGIN
    PRINT 'AVISO: Tabela PermissoesTela não existe neste banco. Verifique se está conectado ao banco correto da empresa (IRRIGACAO, CHINELLATO, etc).';
END
GO

-- Adicionar nos templates que ainda não possuem (se existir a tabela de templates)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PermissoesTemplateDetalhes')
BEGIN
    INSERT INTO [PermissoesTemplateDetalhes] ([TemplateId], [Modulo], [Tela], [Consultar], [Incluir], [Alterar], [Excluir])
    SELECT 
        t.[Id],
        'Sistema' AS [Modulo],
        'DadosEmitente' AS [Tela],
        1 AS [Consultar],
        1 AS [Incluir],
        1 AS [Alterar],
        1 AS [Excluir]
    FROM [PermissoesTemplates] t
    WHERE NOT EXISTS (
        SELECT 1 
        FROM [PermissoesTemplateDetalhes] d 
        WHERE d.[TemplateId] = t.[Id] 
        AND d.[Tela] = 'DadosEmitente'
    );

    DECLARE @TemplatesAffected INT = @@ROWCOUNT;
    PRINT CONCAT('Permissão DadosEmitente adicionada para ', @TemplatesAffected, ' template(s).');
END
GO

PRINT '============================================'
PRINT 'Script executado com sucesso!'
PRINT '============================================'
