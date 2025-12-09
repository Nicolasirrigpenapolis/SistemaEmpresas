-- ============================================
-- SCRIPT: Verificar status das migrations no servidor
-- Execute este script no banco IRRIGACAO do servidor
-- ============================================

-- 1. Ver todas as migrations já aplicadas
PRINT '=== MIGRATIONS JÁ APLICADAS ==='
SELECT 
    MigrationId,
    ProductVersion,
    'Aplicada' AS Status
FROM __EFMigrationsHistory
ORDER BY MigrationId;

-- 2. Verificar se tabelas das novas migrations existem
PRINT ''
PRINT '=== VERIFICANDO TABELAS DAS MIGRATIONS PENDENTES ==='

-- Migration: 20251128133523_AddPermissoesTelas
IF OBJECT_ID('PermissoesTela', 'U') IS NOT NULL
    PRINT '✅ PermissoesTela - EXISTE (migration já foi aplicada)'
ELSE
    PRINT '❌ PermissoesTela - NÃO EXISTE (migration pendente)'

IF OBJECT_ID('PermissoesTemplate', 'U') IS NOT NULL
    PRINT '✅ PermissoesTemplate - EXISTE'
ELSE
    PRINT '❌ PermissoesTemplate - NÃO EXISTE'

-- Migration: 20251203194014_CreateLogsAuditoria
IF OBJECT_ID('LogsAuditoria', 'U') IS NOT NULL
    PRINT '✅ LogsAuditoria - EXISTE (migration já foi aplicada)'
ELSE
    PRINT '❌ LogsAuditoria - NÃO EXISTE (migration pendente)'

-- 3. Verificar coluna PW~Ativo em PW~Usuarios
PRINT ''
PRINT '=== VERIFICANDO COLUNA PW~Ativo ==='
IF EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'PW~Usuarios' AND COLUMN_NAME = 'PW~Ativo'
)
    PRINT '✅ Coluna PW~Ativo EXISTE em PW~Usuarios'
ELSE
    PRINT '❌ Coluna PW~Ativo NÃO EXISTE em PW~Usuarios (será criada)'

PRINT ''
PRINT '=== FIM DA VERIFICAÇÃO ==='
