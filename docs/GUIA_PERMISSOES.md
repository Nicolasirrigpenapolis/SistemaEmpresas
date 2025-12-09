# Sistema de Permissões - Guia de Uso

## Visão Geral

O sistema de permissões do React é **100% compatível** com o VB6. Ambos os sistemas compartilham a mesma tabela `PW~Tabelas` no banco de dados.

### Formato das Permissões

As permissões são armazenadas como uma string de 4 caracteres: `"1111"`

| Posição | Permissão   | Exemplo |
|---------|-------------|---------|
| 1       | Visualizar  | `1___`  |
| 2       | Incluir     | `_1__`  |
| 3       | Modificar   | `__1_`  |
| 4       | Excluir     | `___1`  |

- `"1111"` = Acesso total
- `"1000"` = Só visualiza
- `"1100"` = Visualiza e inclui
- `"0000"` = Sem acesso

---

## Como Usar

### 1. Hook `usePermissao`

O hook principal para verificar permissões em qualquer componente:

```tsx
import { usePermissao } from '../hooks/usePermissao';

function MinhaTelaClientes() {
  // Por nome da tabela
  const { podeVisualizar, podeIncluir, podeModificar, podeExcluir } = usePermissao('CLIENTES');
  
  // OU por rota (converte automaticamente)
  const permissao = usePermissao('/clientes');
  
  return (
    <div>
      {/* Botão só aparece se pode incluir */}
      {podeIncluir && (
        <button>Novo Cliente</button>
      )}
      
      {/* Botão só aparece se pode excluir */}
      {podeExcluir && (
        <button>Excluir</button>
      )}
      
      {/* Botão sempre aparece, mas desabilitado se não pode modificar */}
      <button disabled={!podeModificar}>Editar</button>
    </div>
  );
}
```

### 2. Componente `ConditionalRender`

Para esconder elementos baseado em permissão:

```tsx
import { ConditionalRender } from '../components/PermissionGuard';

function BarraAcoes() {
  return (
    <div className="flex gap-2">
      {/* Só renderiza se tem permissão de incluir */}
      <ConditionalRender tabela="CLIENTES" permissao="incluir">
        <button className="btn-primary">Novo Cliente</button>
      </ConditionalRender>
      
      {/* Com fallback - mostra algo diferente se não tem permissão */}
      <ConditionalRender 
        tabela="CLIENTES" 
        permissao="excluir"
        fallback={<span className="text-gray-400">Sem permissão</span>}
      >
        <button className="btn-danger">Excluir</button>
      </ConditionalRender>
    </div>
  );
}
```

### 3. Componente `DisableWithoutPermission`

Para mostrar o botão mas desabilitado:

```tsx
import { DisableWithoutPermission } from '../components/PermissionGuard';

function BotaoExcluir() {
  return (
    <DisableWithoutPermission 
      tabela="CLIENTES" 
      permissao="excluir"
      tooltip="Você não tem permissão para excluir"
    >
      <button className="btn-danger">Excluir</button>
    </DisableWithoutPermission>
  );
}
```

### 4. Componente `PermissionRoute`

Para proteger rotas inteiras (bloqueia acesso à tela):

```tsx
import { PermissionRoute } from '../components/PermissionGuard';

// No App.tsx ou nas rotas
<Route 
  path="/clientes" 
  element={
    <PermissionRoute tabela="CLIENTES">
      <ClientesPage />
    </PermissionRoute>
  } 
/>

// Ou exigindo permissão específica
<Route 
  path="/clientes/novo" 
  element={
    <PermissionRoute tabela="CLIENTES" permissaoMinima="incluir">
      <NovoClientePage />
    </PermissionRoute>
  } 
/>
```

---

## Mapeamento de Rotas

O arquivo `usePermissao.ts` contém o mapeamento de rotas para tabelas:

```typescript
export const PERMISSAO_MAPPING: Record<string, string> = {
  // Cadastros (existem no VB6)
  '/clientes': 'CLIENTES',
  '/produtos': 'PRODUTOS',
  '/fornecedores': 'FORNECE',
  
  // Novas telas (só React)
  '/dashboard': 'DASHBOARD',
  '/classtrib': 'CLASSTRIB',
  '/usuarios': 'USUARIOS',
};
```

### Adicionando Nova Tela

1. **Adicione no mapeamento** (`usePermissao.ts`):
```typescript
'/minha-nova-tela': 'MINHATABELA',
```

2. **Adicione no seed** (`DbInitializer.cs`):
```csharp
var tabelasReact = new[]
{
    "DASHBOARD",
    "MINHATABELA",  // <-- Nova
};
```

3. **Use na tela**:
```tsx
const { podeVisualizar } = usePermissao('MINHATABELA');
```

---

## Comportamento Padrão

### Tabela não existe nas permissões
Por padrão, se uma tabela não existe no `PW~Tabelas`, o acesso é **liberado**. Você pode mudar isso:

```tsx
// Bloqueia se não existir
const perm = usePermissao('TABELA_NOVA', 'bloqueado');

// Só admin pode acessar se não existir
const perm = usePermissao('TABELA_NOVA', 'admin_only');

// Libera acesso (padrão)
const perm = usePermissao('TABELA_NOVA', 'liberado');
```

### Usuário Admin
Usuários do grupo `ADMIN` ou com nome `ADMIN` têm **acesso total** automaticamente.

---

## Exemplo Completo

```tsx
import React from 'react';
import { usePermissao } from '../hooks/usePermissao';
import { ConditionalRender, DisableWithoutPermission } from '../components/PermissionGuard';

export default function ClientesPage() {
  const { podeVisualizar, podeIncluir, podeModificar, podeExcluir, carregando } = usePermissao('CLIENTES');

  if (carregando) {
    return <div>Carregando...</div>;
  }

  if (!podeVisualizar) {
    return <div>Você não tem permissão para acessar esta tela.</div>;
  }

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Clientes</h1>
        
        <div className="flex gap-2">
          {/* Botão Novo - só aparece se pode incluir */}
          <ConditionalRender tabela="CLIENTES" permissao="incluir">
            <button className="bg-blue-600 text-white px-4 py-2 rounded">
              Novo Cliente
            </button>
          </ConditionalRender>
        </div>
      </div>

      {/* Tabela de clientes */}
      <table className="w-full">
        <thead>
          <tr>
            <th>Nome</th>
            <th>CPF/CNPJ</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {/* ... linhas da tabela ... */}
          <tr>
            <td>João Silva</td>
            <td>123.456.789-00</td>
            <td className="flex gap-2">
              {/* Editar - desabilitado se não pode modificar */}
              <DisableWithoutPermission tabela="CLIENTES" permissao="modificar">
                <button className="text-blue-600">Editar</button>
              </DisableWithoutPermission>
              
              {/* Excluir - só aparece se pode excluir */}
              <ConditionalRender tabela="CLIENTES" permissao="excluir">
                <button className="text-red-600">Excluir</button>
              </ConditionalRender>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
}
```

---

## Compatibilidade VB6

### Regra de Ouro
- ✅ Telas que existem no VB6 usam a **mesma tabela de permissão**
- ✅ Telas novas do React criam tabelas **novas** (VB6 ignora)
- ✅ O formato `"1111"` é **idêntico** nos dois sistemas
- ✅ Alterações feitas no VB6 refletem no React e vice-versa

### Tabelas VB6 vs React

| Tabela VB6 | Rota React | Status |
|------------|------------|--------|
| CLIENTES | /clientes | Compartilhada |
| PRODUTOS | /produtos | Compartilhada |
| FORNECE | /fornecedores | Compartilhada |
| - | /dashboard | Só React |
| - | /classtrib | Só React |
| - | /usuarios | Só React |
