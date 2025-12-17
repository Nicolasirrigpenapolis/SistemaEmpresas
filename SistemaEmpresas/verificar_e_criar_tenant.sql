-- Script para verificar e criar o tenant 'irrigacao'
-- Execute este script no banco IRRIGACAO

USE [IRRIGACAO];
GO

-- Verifica se o tenant existe
SELECT * FROM [Tenants];
GO

-- Se não existir, cria o tenant irrigacao
IF NOT EXISTS (SELECT 1 FROM [Tenants] WHERE [Dominio] = 'irrigacao')
BEGIN
    INSERT INTO [Tenants] ([Nome], [Dominio], [ConnectionString], [Ativo])
    VALUES (
        N'Irrigação Penápolis',
        N'irrigacao',
        N'Server=DESKTOP-CHS14C0\SQLIRRIGACAO;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;',
        1
    );
    PRINT 'Tenant irrigacao criado com sucesso!';
END
ELSE
BEGIN
    -- Se existir, atualiza a connection string
    UPDATE [Tenants]
    SET [ConnectionString] = N'Server=DESKTOP-CHS14C0\SQLIRRIGACAO;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;',
        [Ativo] = 1
    WHERE [Dominio] = 'irrigacao';
    PRINT 'Tenant irrigacao atualizado com sucesso!';
END
GO

-- Verifica o resultado
SELECT * FROM [Tenants] WHERE [Dominio] = 'irrigacao';
GO
