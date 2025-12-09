-- ============================================
-- VALIDAÇÃO PRÉ-MIGRATION
-- Execute ANTES de aplicar as migrations
-- ============================================

PRINT '================================================'
PRINT '  VALIDAÇÃO PRÉ-MIGRATION - SistemaEmpresas'
PRINT '  Data: ' + CONVERT(VARCHAR, GETDATE(), 103) + ' ' + CONVERT(VARCHAR, GETDATE(), 108)
PRINT '================================================'
PRINT ''

-- 1. VERIFICAR MIGRATIONS JÁ APLICADAS
PRINT '1. MIGRATIONS JÁ APLICADAS:'
PRINT '----------------------------'
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory ORDER BY MigrationId
PRINT ''

-- 2. VERIFICAR SE AS NOVAS MIGRATIONS JÁ FORAM APLICADAS
PRINT '2. STATUS DAS MIGRATIONS PENDENTES:'
PRINT '------------------------------------'

IF EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20251128133523_AddPermissoesTelas')
    PRINT '   [!] AddPermissoesTelas - JÁ APLICADA (será ignorada)'
ELSE
    PRINT '   [+] AddPermissoesTelas - PENDENTE (será aplicada)'

IF EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20251202191622_SyncGrupoUsuarioFK')
    PRINT '   [!] SyncGrupoUsuarioFK - JÁ APLICADA (será ignorada)'
ELSE
    PRINT '   [+] SyncGrupoUsuarioFK - PENDENTE (será aplicada)'

IF EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20251203194014_CreateLogsAuditoria')
    PRINT '   [!] CreateLogsAuditoria - JÁ APLICADA (será ignorada)'
ELSE
    PRINT '   [+] CreateLogsAuditoria - PENDENTE (será aplicada)'

PRINT ''

-- 3. VERIFICAR SE TABELAS JÁ EXISTEM
PRINT '3. VERIFICANDO TABELAS:'
PRINT '------------------------'

IF OBJECT_ID('PermissoesTela', 'U') IS NOT NULL
    PRINT '   [!] PermissoesTela - JÁ EXISTE'
ELSE
    PRINT '   [OK] PermissoesTela - Não existe (será criada)'

IF OBJECT_ID('PermissoesTemplate', 'U') IS NOT NULL
    PRINT '   [!] PermissoesTemplate - JÁ EXISTE'
ELSE
    PRINT '   [OK] PermissoesTemplate - Não existe (será criada)'

IF OBJECT_ID('PermissoesTemplateDetalhe', 'U') IS NOT NULL
    PRINT '   [!] PermissoesTemplateDetalhe - JÁ EXISTE'
ELSE
    PRINT '   [OK] PermissoesTemplateDetalhe - Não existe (será criada)'

IF OBJECT_ID('LogsAuditoria', 'U') IS NOT NULL
    PRINT '   [!] LogsAuditoria - JÁ EXISTE'
ELSE
    PRINT '   [OK] LogsAuditoria - Não existe (será criada)'

PRINT ''

-- 4. VERIFICAR COLUNA PW~Ativo
PRINT '4. VERIFICANDO COLUNA PW~Ativo em PW~Usuarios:'
PRINT '-----------------------------------------------'

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PW~Usuarios' AND COLUMN_NAME = 'PW~Ativo')
    PRINT '   [!] Coluna PW~Ativo - JÁ EXISTE'
ELSE
    PRINT '   [OK] Coluna PW~Ativo - Não existe (será criada)'

PRINT ''

-- 5. VERIFICAR TABELA PW~Usuarios EXISTE
PRINT '5. VERIFICANDO TABELA PW~Usuarios:'
PRINT '-----------------------------------'

IF OBJECT_ID('PW~Usuarios', 'U') IS NOT NULL
    PRINT '   [OK] Tabela PW~Usuarios existe'
ELSE
    PRINT '   [ERRO] Tabela PW~Usuarios NÃO EXISTE - Migration vai falhar!'

PRINT ''
PRINT '================================================'
PRINT '  VALIDAÇÃO CONCLUÍDA'
PRINT '================================================'
PRINT ''
PRINT 'Se todos os itens estão [OK] ou [+], pode aplicar o script!'
PRINT 'Se houver [ERRO], corrija antes de continuar.'
PRINT ''
