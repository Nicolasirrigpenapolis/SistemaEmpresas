# Scripts SQL de Migração - Sistema Empresas

Gerado em: 12 de dezembro de 2025

## 📋 Arquivos Gerados

### 1. `migration_script_appdb.sql`
**Descrição:** Script completo com TODAS as migrações do AppDbContext (banco principal)
**Uso:** Execute este script em um banco de dados VAZIO para criar toda a estrutura

**Migrações incluídas:**
- 20251111183501_CreateAllTables
- 20251125132336_CriacaoClassTrib
- 20251128133523_AddPermissoesTelas
- 20251202191622_SyncGrupoUsuarioFK
- 20251203194014_CreateLogsAuditoria
- 20251209161029_AddModuloTransporte
- 20251210205540_AddMarcaModeloToReboques
- 20251211125540_AddEmailToUsuario

---

### 2. `migration_script_recent.sql`
**Descrição:** Script apenas com as migrações MAIS RECENTES (após CreateLogsAuditoria)
**Uso:** Execute este script em um banco que já possui as migrações anteriores

**Migrações incluídas:**
- 20251209161029_AddModuloTransporte (Módulo de Transporte completo)
- 20251210205540_AddMarcaModeloToReboques (Adiciona Marca/Modelo aos reboques)
- 20251211125540_AddEmailToUsuario (Adiciona campo Email ao usuário)

**Tabelas criadas:**
- `Veiculos` - Cadastro de veículos
- `Reboques` - Cadastro de reboques
- `Viagens` - Registro de viagens
- `ManutencoesVeiculo` - Manutenções de veículos
- `ManutencoesPeca` - Peças utilizadas nas manutenções
- `DespesasViagem` - Despesas das viagens
- `ReceitasViagem` - Receitas das viagens

---

### 3. `migration_script_tenantdb.sql`
**Descrição:** Script completo do TenantDbContext (gerenciamento de tenants/empresas)
**Uso:** Execute este script para criar a estrutura de multi-tenancy

**Migrações incluídas:**
- 20251104134513_InitialTenants

**Tabelas criadas:**
- `Tenants` - Cadastro de empresas/tenants

---

## 🚀 Como Usar

### Para um banco NOVO (primeira instalação):
```sql
-- 1. Execute primeiro o script de tenants
USE [SeuBancoDeDados]
GO
-- Abra e execute: migration_script_tenantdb.sql

-- 2. Execute o script completo do AppDb
-- Abra e execute: migration_script_appdb.sql
```

### Para um banco EXISTENTE (atualização):
```sql
-- Execute apenas as migrações que faltam
USE [SeuBancoDeDados]
GO
-- Abra e execute: migration_script_recent.sql
```

---

## ⚠️ IMPORTANTE

1. **SEMPRE faça backup do banco antes de executar os scripts!**

2. **Verifique quais migrações já foram aplicadas:**
   ```sql
   SELECT * FROM [__EFMigrationsHistory] ORDER BY MigrationId
   ```

3. **Os scripts já incluem controle de transação:**
   - Começam com `BEGIN TRANSACTION`
   - Terminam com `COMMIT`
   - Se houver erro, faça `ROLLBACK`

4. **Script de Email (20251211125540_AddEmailToUsuario):**
   - Este script verifica se a coluna Email já existe antes de criar
   - É seguro executar mesmo se a coluna já existir

---

## 📝 Detalhes das Migrações Recentes

### AddModuloTransporte (20251209161029)
Adiciona o módulo completo de gestão de transporte, incluindo:
- Gestão de veículos e reboques
- Controle de viagens com KM, origem/destino
- Registro de despesas e receitas por viagem
- Controle de manutenções com peças e fornecedores

### AddMarcaModeloToReboques (20251210205540)
Adiciona campos à tabela Reboques:
- Marca (varchar 100)
- Modelo (varchar 100)
- AnoFabricacao (int)

### AddEmailToUsuario (20251211125540)
Adiciona campo Email à tabela PW~Usuarios:
- Email (varchar 255, nullable)
- Inclui validação para não duplicar coluna se já existir

---

## 🔧 Regenerar Scripts

Para regenerar os scripts após novas migrações:

```powershell
# Script completo AppDb
dotnet ef migrations script --context AppDbContext --output "migration_script_appdb.sql"

# Script incremental (a partir de uma migração específica)
dotnet ef migrations script [MigracaoInicial] --context AppDbContext --output "migration_script_recent.sql"

# Script TenantDb
dotnet ef migrations script --context TenantDbContext --output "migration_script_tenantdb.sql"
```

---

## 📞 Suporte

Em caso de dúvidas ou problemas na aplicação dos scripts, entre em contato com a equipe de desenvolvimento.
