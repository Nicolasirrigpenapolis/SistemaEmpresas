# Deploy - Sistema de Permissões

## Data: 28/11/2025

## Migration: `20251128133523_AddPermissoesTelas`

---

## O que foi criado:

### 1. Coluna nova na tabela `PW~Usuarios`
- **`PW~Ativo`** (bit, NOT NULL, default: true) - Indica se o usuário está ativo/inativo

### 2. Tabela `PermissoesTela`
Armazena as permissões de cada grupo/usuário por tela.

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| Id | int (PK, Identity) | Identificador |
| Grupo | nvarchar(100) | Nome do grupo ou usuário |
| Modulo | nvarchar(100) | Módulo do sistema |
| Tela | nvarchar(100) | Identificador da tela |
| NomeTela | nvarchar(200) | Nome amigável da tela |
| Rota | nvarchar(200) | Rota no frontend |
| Consultar | bit | Permissão de consulta |
| Incluir | bit | Permissão de inclusão |
| Alterar | bit | Permissão de alteração |
| Excluir | bit | Permissão de exclusão |
| Ordem | int | Ordem de exibição |

**Índice único:** `IX_PermissoesTela_Grupo_Tela` (Grupo, Tela)

### 3. Tabela `PermissoesTemplate`
Templates de permissões pré-definidos.

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| Id | int (PK, Identity) | Identificador |
| Nome | nvarchar(100) | Nome do template |
| Descricao | nvarchar(500) | Descrição |
| IsPadrao | bit | Se é o template padrão |
| DataCriacao | datetime2 | Data de criação (default: GETDATE()) |

**Índice único:** `IX_PermissoesTemplate_Nome` (Nome)

### 4. Tabela `PermissoesTemplateDetalhe`
Detalhes de cada template (permissões por tela).

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| Id | int (PK, Identity) | Identificador |
| TemplateId | int (FK) | Referência ao template |
| Modulo | nvarchar(100) | Módulo do sistema |
| Tela | nvarchar(100) | Identificador da tela |
| Consultar | bit | Permissão de consulta |
| Incluir | bit | Permissão de inclusão |
| Alterar | bit | Permissão de alteração |
| Excluir | bit | Permissão de exclusão |

**FK:** `FK_PermissoesTemplateDetalhe_Template` → PermissoesTemplate(Id) ON DELETE CASCADE  
**Índice único:** `IX_PermissoesTemplateDetalhe_Template_Tela` (TemplateId, Tela)

---

## Como aplicar no servidor:

### Opção 1: Via EF Core (recomendado)
```powershell
cd C:\SistemaEmpresas
dotnet ef database update --context AppDbContext
```

### Opção 2: Via Script SQL
Execute o arquivo `script_permissoes_servidor.sql` no SQL Server Management Studio.

---

## Arquivos relacionados:

### Backend
- `Models/PermissoesTela.cs`
- `Models/PermissoesTemplate.cs`
- `Models/PermissoesTemplateDetalhe.cs`
- `Models/PwUsuario.cs` (adicionado campo PwAtivo)
- `Repositories/PermissoesTelaRepository.cs`
- `Services/PermissoesTelaService.cs`
- `Controllers/PermissoesController.cs`
- `DTOs/PermissoesTelasDtos.cs`

### Frontend
- `src/types/permissoes.ts`
- `src/services/permissoesTelaService.ts`
- `src/pages/Permissoes/PermissoesPage.tsx`

### Rota adicionada
- `/permissoes` - Página de gerenciamento de permissões

---

## Endpoints da API:

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/permissoes/telas` | Lista telas disponíveis |
| GET | `/api/permissoes/grupo/{grupo}` | Permissões de um grupo |
| POST | `/api/permissoes/grupo/{grupo}` | Salvar permissões do grupo |
| GET | `/api/permissoes/templates` | Lista templates |
| GET | `/api/permissoes/templates/{id}` | Buscar template por ID |
| POST | `/api/permissoes/templates` | Criar template |
| PUT | `/api/permissoes/templates/{id}` | Atualizar template |
| DELETE | `/api/permissoes/templates/{id}` | Excluir template |
| POST | `/api/permissoes/aplicar-template` | Aplicar template a um grupo |
