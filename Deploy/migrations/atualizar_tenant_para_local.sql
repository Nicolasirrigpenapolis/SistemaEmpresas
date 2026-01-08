-- Script para atualizar a connection string do tenant 'irrigacao'
-- Execute este script no banco IRRIGACAO do servidor DESKTOP-CHS14C0\SQLIRRIGACAO

USE [IRRIGACAO];
GO

-- Mostra a connection string atual
PRINT 'Connection String ANTES da atualização:';
SELECT [Id], [Nome], [Dominio], [ConnectionString], [Ativo] 
FROM [Tenants] 
WHERE [Dominio] = 'irrigacao';
GO

-- Atualiza a connection string para o servidor local
UPDATE [Tenants]
SET [ConnectionString] = N'Server=DESKTOP-CHS14C0\SQLIRRIGACAO;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;',
    [Ativo] = 1
WHERE [Dominio] = 'irrigacao';
GO

-- Mostra a connection string atualizada
PRINT 'Connection String DEPOIS da atualização:';
SELECT [Id], [Nome], [Dominio], [ConnectionString], [Ativo] 
FROM [Tenants] 
WHERE [Dominio] = 'irrigacao';
GO

PRINT 'Connection string atualizada com sucesso!';
GO
