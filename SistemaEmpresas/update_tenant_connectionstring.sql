-- Script para atualizar a connection string do tenant 'irrigacao'
-- Execute este script no banco IRRIGACAO

USE [IRRIGACAO];
GO

-- Atualiza a connection string do tenant irrigacao
UPDATE [Tenants]
SET [ConnectionString] = 'Server=DESKTOP-CHS14C0\SQLIRRIGACAO;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;'
WHERE [Dominio] = 'irrigacao';
GO

-- Verifica se foi atualizado
SELECT * FROM [Tenants] WHERE [Dominio] = 'irrigacao';
GO
