-- =============================================
-- Script para criar tabela de Logs de Auditoria
-- Sistema Empresas - Rastreamento completo
-- =============================================

-- Criar tabela de logs
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LogsAuditoria')
BEGIN
    CREATE TABLE [dbo].[LogsAuditoria] (
        [Id] BIGINT IDENTITY(1,1) NOT NULL,
        [DataHora] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UsuarioCodigo] INT NOT NULL,
        [UsuarioNome] NVARCHAR(100) NOT NULL,
        [UsuarioGrupo] NVARCHAR(50) NOT NULL,
        [TipoAcao] NVARCHAR(50) NOT NULL,
        [Modulo] NVARCHAR(50) NOT NULL,
        [Entidade] NVARCHAR(100) NOT NULL,
        [EntidadeId] NVARCHAR(100) NULL,
        [Descricao] NVARCHAR(500) NOT NULL,
        [DadosAnteriores] NVARCHAR(MAX) NULL,
        [DadosNovos] NVARCHAR(MAX) NULL,
        [CamposAlterados] NVARCHAR(1000) NULL,
        [EnderecoIP] NVARCHAR(50) NULL,
        [UserAgent] NVARCHAR(500) NULL,
        [MetodoHttp] NVARCHAR(10) NULL,
        [UrlRequisicao] NVARCHAR(500) NULL,
        [StatusCode] INT NULL,
        [TempoExecucaoMs] BIGINT NULL,
        [Erro] BIT NOT NULL DEFAULT 0,
        [MensagemErro] NVARCHAR(2000) NULL,
        [TenantId] INT NULL,
        [TenantNome] NVARCHAR(100) NULL,
        [SessaoId] NVARCHAR(100) NULL,
        [CorrelationId] NVARCHAR(50) NULL,
        
        CONSTRAINT [PK_LogsAuditoria] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    PRINT 'Tabela LogsAuditoria criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela LogsAuditoria já existe.';
END
GO

-- Índices para performance nas consultas
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogsAuditoria_DataHora')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_LogsAuditoria_DataHora] 
    ON [dbo].[LogsAuditoria] ([DataHora] DESC)
    INCLUDE ([UsuarioNome], [TipoAcao], [Modulo], [Entidade], [Descricao]);
    PRINT 'Índice IX_LogsAuditoria_DataHora criado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Usuario')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_LogsAuditoria_Usuario] 
    ON [dbo].[LogsAuditoria] ([UsuarioCodigo], [DataHora] DESC);
    PRINT 'Índice IX_LogsAuditoria_Usuario criado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogsAuditoria_TipoAcao')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_LogsAuditoria_TipoAcao] 
    ON [dbo].[LogsAuditoria] ([TipoAcao], [DataHora] DESC);
    PRINT 'Índice IX_LogsAuditoria_TipoAcao criado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Modulo')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_LogsAuditoria_Modulo] 
    ON [dbo].[LogsAuditoria] ([Modulo], [Entidade], [DataHora] DESC);
    PRINT 'Índice IX_LogsAuditoria_Modulo criado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Entidade')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_LogsAuditoria_Entidade] 
    ON [dbo].[LogsAuditoria] ([Entidade], [EntidadeId], [DataHora] DESC);
    PRINT 'Índice IX_LogsAuditoria_Entidade criado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Erro')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_LogsAuditoria_Erro] 
    ON [dbo].[LogsAuditoria] ([Erro], [DataHora] DESC)
    WHERE [Erro] = 1;
    PRINT 'Índice IX_LogsAuditoria_Erro criado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogsAuditoria_Tenant')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_LogsAuditoria_Tenant] 
    ON [dbo].[LogsAuditoria] ([TenantId], [DataHora] DESC);
    PRINT 'Índice IX_LogsAuditoria_Tenant criado.';
END
GO

-- View para consulta rápida dos últimos logs
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_UltimosLogs')
    DROP VIEW [dbo].[vw_UltimosLogs];
GO

CREATE VIEW [dbo].[vw_UltimosLogs]
AS
SELECT TOP 1000
    Id,
    DataHora,
    UsuarioNome,
    UsuarioGrupo,
    TipoAcao,
    Modulo,
    Entidade,
    EntidadeId,
    Descricao,
    EnderecoIP,
    Erro,
    TenantNome
FROM [dbo].[LogsAuditoria]
ORDER BY DataHora DESC;
GO

PRINT 'View vw_UltimosLogs criada.';
GO

-- Stored Procedure para limpar logs antigos (manter últimos X dias)
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_LimparLogsAntigos')
    DROP PROCEDURE [dbo].[sp_LimparLogsAntigos];
GO

CREATE PROCEDURE [dbo].[sp_LimparLogsAntigos]
    @DiasParaManter INT = 90
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @DataLimite DATETIME2 = DATEADD(DAY, -@DiasParaManter, GETUTCDATE());
    DECLARE @RegistrosExcluidos INT;
    
    DELETE FROM [dbo].[LogsAuditoria]
    WHERE [DataHora] < @DataLimite;
    
    SET @RegistrosExcluidos = @@ROWCOUNT;
    
    SELECT @RegistrosExcluidos AS RegistrosExcluidos, @DataLimite AS DataLimite;
END
GO

PRINT 'Procedure sp_LimparLogsAntigos criada.';
GO

PRINT '========================================';
PRINT 'Script de LogsAuditoria executado com sucesso!';
PRINT '========================================';
