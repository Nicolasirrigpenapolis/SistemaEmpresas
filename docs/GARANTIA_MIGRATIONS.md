# GARANTIA - O que vai acontecer no servidor

## ✅ ESTÁ 100% SEGURO

Quando você rodar no servidor:
```powershell
cd C:\SistemaEmpresas
dotnet ef database update --context AppDbContext
```

**O EF Core vai fazer ISSO:**

1. **Conectar no banco de dados**
2. **Verificar a tabela `__EFMigrationsHistory`**
3. **Ver quais migrations já estão registradas**

### No seu caso (servidor):

```
__EFMigrationsHistory já contém:
├─ 20251111183501_CreateAllTables      ✅ (já aplicada)
├─ 20251125132336_CriacaoClassTrib     ✅ (já aplicada)
└─ 20251128133523_AddPermissoesTelas   ❌ (PENDENTE)
```

4. **O EF vai calcular a diferença e rodar APENAS a migration pendente**

---

## ❌ O QUE NÃO VAI ACONTECER:

- ❌ Não vai tentar recriar as tabelas antigas (Adutoras, Acoes, etc)
- ❌ Não vai dar erro "table already exists"
- ❌ Não vai excluir dados
- ❌ Não vai reaplicar migrations antigas

---

## ✅ O QUE VAI ACONTECER:

- ✅ Vai adicionar coluna `PW~Ativo` em `PW~Usuarios`
- ✅ Vai criar tabela `PermissoesTela`
- ✅ Vai criar tabela `PermissoesTemplate`
- ✅ Vai criar tabela `PermissoesTemplateDetalhe`
- ✅ Vai registrar `20251128133523_AddPermissoesTelas` em `__EFMigrationsHistory`

---

## Por que funciona assim?

O Entity Framework Core **nunca executa 2 vezes** a mesma migration. Ele usa a tabela `__EFMigrationsHistory` como "controle de versão":

```sql
-- Quando você rodar database update, ele faz:
SELECT MigrationId FROM __EFMigrationsHistory
-- Se encontrar a migration, ele PULA
-- Se não encontrar, ele EXECUTA
```

---

## Comparação:

| Ambiente | CreateAllTables | CriacaoClassTrib | AddPermissoesTelas |
|----------|-----------------|------------------|--------------------|
| **Desenvolvimento** | ✅ Aplicada | ✅ Aplicada | ✅ Aplicada |
| **Servidor** | ✅ Aplicada | ✅ Aplicada | ❌ Vai aplicar |

**Resultado final:** Ambos ficarão com as 3 migrations aplicadas! 🎯

---

## TL;DR

**SIM, é 100% seguro!** 

O servidor só vai:
1. Detectar que a migration `AddPermissoesTelas` não foi aplicada
2. Aplicar APENAS essa migration
3. Pronto! ✅

Pode rodar tranquilo! 👍
