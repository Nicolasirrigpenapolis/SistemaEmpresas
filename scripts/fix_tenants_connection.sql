-- Script para corrigir as Connection Strings dos Tenants
-- Este script atualiza os tenants para usar SQL Authentication em vez de Windows Auth

USE IRRIGACAO;

-- Verificar tenants atuais
SELECT Id, Nome, Dominio, ConnectionString FROM Tenants WHERE Ativo = 1;

-- Atualizar Connection String para Irrigação Penápolis
UPDATE Tenants 
SET ConnectionString = 'Server=SRVSQL\SQLEXPRESS;Database=IRRIGACAO;User Id=admin;Password=conectairrig@;TrustServerCertificate=True;'
WHERE Dominio = 'irrigacao' AND Ativo = 1;

-- Atualizar Connection String para Chinellato Transportes
UPDATE Tenants 
SET ConnectionString = 'Server=SRVSQL\SQLEXPRESS;Database=CHINELLATO;User Id=admin;Password=conectairrig@;TrustServerCertificate=True;'
WHERE Dominio = 'chinellato' AND Ativo = 1;

-- Verificar resultado após atualização
SELECT Id, Nome, Dominio, ConnectionString FROM Tenants WHERE Ativo = 1;
