# Proposta de Reorganizacao do Projeto SistemaEmpresas

## Objetivo
Organizar o projeto para crescer sem virar caos, mantendo facil encontrar onde esta cada coisa
e reduzindo o custo mental para um unico desenvolvedor fullstack.

## Premissas e limites
- Models nao podem ser alterados (scaffold do EF Core).
- Evitar mover a raiz do backend/frontend agora para reduzir impacto em build, sln e scripts.
- Migracao incremental, um modulo por vez, com commits pequenos.
- Estrutura deve ser simples e previsivel.

## Diagnostico rapido (resumo)
- Backend: Controllers/Services/DTOs/Repositories espalhados por pasta tecnica.
- Frontend: pages, components, services e types separados por area e por tipo.
- Dificil saber "onde mexer" e "qual o dono" de cada arquivo.

## Proposta de estrutura (fase 1: sem mover raiz)

### Raiz do repo
```
SistemaEmpresas/
  Deploy/
  docs/
  frontend/
  PROJETO LEGADO/
  SistemaEmpresas/
  testsprite_tests/
  README.md
  SistemaEmpresas.sln
```

### Backend (SistemaEmpresas/)
```
SistemaEmpresas/
  Program.cs
  SistemaEmpresas.csproj
  appsettings.*.json
  wwwroot/

  Models/                 # NAO ALTERAR (EF Core scaffold)
  Data/                   # DbContext, initializer
  Migrations/             # manter aqui no inicio
  Middleware/

  Core/                   # cross-cutting
    Enums/
    Exceptions/
    Extensions/
    Interfaces/
    Dtos/                 # DTOs compartilhados
    Validation/

  Infrastructure/         # detalhes tecnicos
    Security/
    Audit/

  Features/               # por modulo de negocio
    Auth/
      Controllers/
      Services/
      Dtos/
    Cadastros/
      Geral/
        Controllers/
        Services/
        Repositories/
        Dtos/
      Produtos/
      Emitentes/
    Estoque/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Compras/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Fiscal/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Financeiro/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Transporte/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Seguranca/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Logs/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Dashboard/
      Controllers/
      Services/
      Dtos/
    MovimentoContabil/
      Controllers/
      Services/
      Repositories/
      Dtos/
    NotaFiscal/
      Controllers/
      Services/
      Repositories/
      Dtos/
    Geral/                # somente se houver fluxo realmente geral
    Tenants/
```

### Mapeamento direto do que existe hoje
- `Controllers/<Area>` -> `Features/<Area>/Controllers`
- `Services/<Area>` -> `Features/<Area>/Services`
- `Repositories/<Area>` -> `Features/<Area>/Repositories`
- `DTOs/<Area>` -> `Features/<Area>/Dtos`
- `Utils/*` -> `Core/Validation` ou `Core/Extensions` conforme o caso
- `Enums/StatusEnums.cs` -> `Core/Enums/StatusEnums.cs` (e criar enums por area no futuro)

### Frontend (frontend/src)
```
frontend/src/
  app/                    # App.tsx, main.tsx, routes.tsx
  core/                   # compartilhado
    components/
    hooks/
    contexts/
    utils/
    types/
  services/               # api, auth, etc (pode ficar por area)
  features/               # por modulo de negocio
    auth/
      pages/
      components/
      types/
      hooks/
    dashboard/
      pages/
      components/
      types/
    cadastros/
      geral/
      produtos/
      emitentes/
      usuarios/
    estoque/
    fiscal/
    financeiro/
    transporte/
    logs/
    movimento-contabil/
    nota-fiscal/
  assets/
  index.css
```

### Observacao importante (frontend)
No inicio, `pages/`, `components/`, `services/` e `types/` atuais podem ficar.
Tudo novo vai para `features/` e `core/`. Assim a migracao e gradual.

## Regras simples para nao se perder
- Todo arquivo deve ter um "dono" (feature ou core).
- Novo codigo: sempre dentro de `Features/<Area>` no back e `features/<area>` no front.
- Compartilhado: vai para `Core/` (back) ou `core/` (front).
- Evitar novos "utils" soltos. Se for generico, vai para `Core/`.
- Um modulo por vez na migracao. Nao mexer em tudo junto.

## Plano de migracao incremental

### Fase 0 - Preparacao (1 dia)
- Definir lista de modulos existentes (Transporte, Fiscal, Estoque, etc).
- Criar pastas novas (Core, Infrastructure, Features) vazias.
- Documentar o mapeamento por modulo.

### Fase 1 - Backend por modulo (1 a 2 dias por modulo)
1. Escolher um modulo pequeno (ex.: Transporte).
2. Mover Controllers/Services/Repositories/DTOs para `Features/Transporte/`.
3. Ajustar namespaces e usings.
4. Atualizar DI no Program.cs se necessario.
5. Compilar e commitar.

### Fase 2 - Core e Infrastructure (1 dia)
- Mover Utils e Enums para `Core/`.
- Mover Audit/Security/Middleware especifico para `Infrastructure/`.
- Manter `Data/` e `Migrations/` no lugar por enquanto.

### Fase 3 - Frontend por modulo (0.5 a 1 dia por modulo)
1. Criar `core/` e `features/`.
2. Migrar uma feature (ex.: Transporte) do `pages/` para `features/transporte/`.
3. Ajustar imports e rotas.
4. Build e commit.

### Fase 4 - Ajustes finais
- Se desejar, reorganizar docs e scripts.
- Opcional: mover backend/front para `src/` somente quando tudo estiver estavel.

## Convencoes de nomenclatura (simples)

### Backend
```
Features/<Area>/
  Controllers/<Nome>Controller.cs
  Services/I<Nome>Service.cs
  Services/<Nome>Service.cs
  Repositories/I<Nome>Repository.cs
  Repositories/<Nome>Repository.cs
  Dtos/<Nome>Dto.cs
  Dtos/Create<Nome>Dto.cs
  Dtos/Update<Nome>Dto.cs

Models/                  # NAO ALTERAR
```

### Frontend
```
features/<area>/
  pages/<Nome>Page.tsx
  components/<Nome>Component.tsx
  hooks/use<Nome>.ts
  types/<nome>.types.ts
```

## Check-list por modulo (backend)
- [ ] Controllers movidos
- [ ] Services movidos
- [ ] Repositories movidos
- [ ] DTOs movidos
- [ ] Namespaces ajustados
- [ ] Program.cs e DI revisados
- [ ] Build ok

## Conclusao
Para um time de uma pessoa, o melhor e um padrao simples, previsivel e incremental.
Com `Features` por area e `Core` para o compartilhado, fica facil saber onde mexer
e o sistema continua crescendo sem virar bagunca.
