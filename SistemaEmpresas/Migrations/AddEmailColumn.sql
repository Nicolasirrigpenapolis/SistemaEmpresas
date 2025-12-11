-- Migration: Adiciona campo PW~Email na tabela PW~Usuarios
-- Data: 2024-12-11
-- Descrição: Adiciona campo de email para os usuários. O email pode ser duplicado entre usuários diferentes.

-- Verifica se a coluna já existe antes de criar
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'PW~Usuarios' AND COLUMN_NAME = 'PW~Email'
)
BEGIN
    ALTER TABLE [PW~Usuarios] 
    ADD [PW~Email] VARCHAR(255) NULL;
    
    PRINT 'Coluna [PW~Email] adicionada com sucesso à tabela [PW~Usuarios]';
END
ELSE
BEGIN
    PRINT 'Coluna [PW~Email] já existe na tabela [PW~Usuarios]';
END
GO
