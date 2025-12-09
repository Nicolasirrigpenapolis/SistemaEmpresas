# CLAUDE.md - Memória do Projeto SistemaEmpresas

Este arquivo contém informações importantes que devem ser lembradas ao trabalhar neste projeto.

---

## 🆕 ATUALIZAÇÃO: 09/12/2024 - Documentação de Migração NewSistema

**CRIADO:** Documentação completa para migração de funcionalidades do projeto **NewSistema** para o **SistemaEmpresas**.

### 📚 Documentos Criados:

1. **[docs/INDICE_MIGRACAO_NEWSISTEMA.md](./docs/INDICE_MIGRACAO_NEWSISTEMA.md)**
   - Índice navegável de toda documentação de migração
   - Guia de navegação por perfil (Gestor, Dev, Tech Lead)

2. **[docs/RESUMO_EXECUTIVO_NEWSISTEMA.md](./docs/RESUMO_EXECUTIVO_NEWSISTEMA.md)**
   - Resumo executivo para stakeholders e gestores
   - Análise custo-benefício
   - Aprovações e decisões estratégicas

3. **[docs/PLANO_MIGRACAO_NEWSISTEMA.md](./docs/PLANO_MIGRACAO_NEWSISTEMA.md)**
   - Plano estratégico completo de migração
   - Análise de todos os módulos do NewSistema
   - Recomendações: o que trazer, adaptar ou ignorar

4. **[docs/GUIA_IMPLEMENTACAO_NEWSISTEMA.md](./docs/GUIA_IMPLEMENTACAO_NEWSISTEMA.md)**
   - Guia prático com exemplos de código
   - GenericRepository, DTOs, BaseController, Soft Delete
   - Checklist de implementação

5. **[docs/ANALISE_TECNICA_NEWSISTEMA.md](./docs/ANALISE_TECNICA_NEWSISTEMA.md)**
   - Análise técnica detalhada
   - Comparação entre sistemas
   - Roadmap técnico por sprint

6. **[docs/README.md](./docs/README.md)**
   - Atualizado com índice completo de toda documentação

### 🎯 Principais Recomendações:

**✅ TRAZER (Alta Prioridade):**
- GenericRepository Pattern (reduz 60% código em novos controllers)
- DTOs estruturados (List/Detail/Create/Update)
- BaseController genérico
- Soft Delete (auditoria completa)
- CacheService melhorado

**🔄 AVALIAR (Condicional):**
- Módulo de Viagens (se empresa trabalha com transporte)
- MDFe completo (se precisa emitir manifesto eletrônico)
- Sistema de Veículos

**❌ NÃO TRAZER:**
- Sistema de Usuários novo (manter PwUsuario legado)
- Cadastros duplicados (usar existentes)

### 📋 Próximos Passos:

1. Ler [INDICE_MIGRACAO_NEWSISTEMA.md](./docs/INDICE_MIGRACAO_NEWSISTEMA.md)
2. Validar necessidade de módulos com stakeholders
3. Aprovar Fase 1 (melhorias arquiteturais - 4-6 semanas)
4. Implementar padrões do NewSistema gradualmente

---

## 🔴 REGRA DE OURO - COMPATIBILIDADE COM VB6

**CRÍTICO**: Este sistema está em migração gradual do VB6 para React/.NET. 
O banco de dados é **COMPARTILHADO** entre o sistema legado (VB6) e o novo sistema (React/.NET).

### O que isso significa:

#### ✅ PODE FAZER:
- Usar as mesmas tabelas existentes (`PW~Grupos`, `PW~Usuarios`, `PW~Tabelas`, etc.)
- Usar a **mesma função de criptografia** (`VB6CryptoService.Encripta`/`Decripta`)
- Manter o formato de dados exatamente como o VB6 espera
- Criar interface moderna no React, mas gravando no banco no formato legado
- Ler e escrever dados que o VB6 consegue entender

#### ❌ NÃO PODE FAZER:
- **NÃO** criar colunas novas nas tabelas existentes (quebraria o VB6)
- **NÃO** mudar o formato dos dados criptografados
- **NÃO** alterar a estrutura das chaves primárias
- **NÃO** usar formatos de dados que o VB6 não entende
- **NÃO** fazer alterações que quebrem o funcionamento do sistema legado

### Formato de Permissões:
- Permissões são armazenadas como string de 4 caracteres: `"1111"`
- Posição 1: Visualiza (0=não, 1=sim)
- Posição 2: Inclui (0=não, 1=sim)
- Posição 3: Modifica (0=não, 1=sim)
- Posição 4: Exclui (0=não, 1=sim)
- Exemplo: `"1100"` = pode visualizar e incluir, mas não modificar nem excluir

### Criptografia:
- Todos os nomes de usuários, grupos e senhas são criptografados no banco
- Usar `VB6CryptoService.Encripta()` para gravar
- Usar `VB6CryptoService.Decripta()` para ler
- A criptografia usa XOR + Base64, compatível com função `Encripta`/`Decripta` do VB6

### Tabelas de Segurança:
- `PW~Grupos`: Grupos de usuários (ex: SUPERVISAO, VENDAS, etc.)
- `PW~Usuarios`: Usuários do sistema (nome, senha, grupo, observações)
- `PW~Tabelas`: Permissões por grupo/tabela (projeto, grupo, nome da tabela, permissões)

---

## Estrutura do Projeto

### Backend (.NET 8)
- **Controllers**: API REST
- **Services**: Lógica de negócio
- **Repositories**: Acesso a dados
- **Models**: Entidades do banco (scaffold do EF Core)
- **DTOs**: Data Transfer Objects para API

### Frontend (React + TypeScript + Vite)
- **pages/**: Páginas da aplicação
- **components/**: Componentes reutilizáveis
- **services/**: Comunicação com API
- **contexts/**: Contextos React (Auth, etc.)
- **types/**: Interfaces TypeScript

---

## Padrões do Projeto

### Nomenclatura:
- Backend: PascalCase para classes/métodos, camelCase para variáveis
- Frontend: camelCase para variáveis/funções, PascalCase para componentes
- DTOs: sufixo `Dto` (ex: `UsuarioDto`, `GrupoDto`)

### Autenticação:
- JWT Token com refresh token
- Multi-tenant por domínio
- Grupo "SUPERVISAO" = Administrador com acesso total

---

*Última atualização: 29/11/2025*

---

## Estrutura de Pastas (Atualizada)

```
SistemaEmpresas/
├── CLAUDE.md                    # Este arquivo (memória do projeto)
├── .gitignore                   # Ignorar arquivos sensíveis e build
├── SistemaEmpresas.sln          # Solution Visual Studio
├── docs/                        # Documentação do projeto
│   ├── PRD.md                   # Product Requirements Document
│   ├── GUIA_RAPIDO.md           # Guia rápido para desenvolvedores
│   └── ...                      # Outras documentações
├── scripts/                     # Scripts SQL úteis
│   ├── SQL_CRIAR_TABELAS.sql
│   └── ...
├── frontend/                    # React + TypeScript + Vite
│   └── src/
│       ├── components/          # Componentes reutilizáveis
│       ├── contexts/            # Contextos React (Auth)
│       ├── hooks/               # Custom hooks
│       ├── pages/               # Páginas da aplicação
│       ├── services/            # Comunicação com API
│       └── types/               # Interfaces TypeScript
├── SistemaEmpresas/             # Backend .NET 8
│   ├── Controllers/             # API REST
│   ├── Services/                # Lógica de negócio
│   ├── Repositories/            # Acesso a dados
│   ├── Models/                  # Entidades EF Core
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Middleware/              # Middlewares personalizados
│   └── Data/                    # DbContext e configurações
└── SistemaIrrigacao/            # [NÃO MEXER] Sistema legado VB6
```
