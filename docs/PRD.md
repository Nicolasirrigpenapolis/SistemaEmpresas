# PRD - Product Requirements Document
## Sistema Empresas

---

**Versão:** 1.1  
**Data:** 29 de novembro de 2025  
**Autor:** Equipe de Desenvolvimento  
**Status:** Em desenvolvimento (Release Candidate)

### Histórico de revisões
| Versão | Data | Autor | Resumo |
|--------|------|-------|--------|
| 1.1 | 29/11/2025 | Equipe de Desenvolvimento | Revisão completa com módulos ClassTrib, Cadastro Geral e orientações de deploy/observabilidade |
| 1.0 | 28/11/2025 | Equipe de Desenvolvimento | Primeira consolidação do PRD após migração inicial do VB6 |

---

## 1. Visão Geral do Produto

### 1.1 Resumo executivo
O **Sistema Empresas** é a nova camada web multi-tenant que convive com o legado VB6. A solução combina **backend em .NET 8** (serviço Windows) e **frontend React 19 + Vite** para entregar dashboards, cadastros e módulos fiscais avançados sem quebrar o banco compartilhado. Todo o tráfego segue o fluxo `TenantService → AppDbContext dinâmico → Controllers`, assegurando isolamento por cliente e compatibilidade total com os formatos de dados legados.

### 1.2 Problema a resolver
- Interface VB6 desatualizada e dependente de acesso local.
- Operação multi-empresa sem isolamento lógico claro.
- Processos fiscais (ClassTrib/IBS-CBS) atualizados manualmente.
- Dificuldade em aplicar políticas modernas de segurança (JWT, HTTPS, logs centralizados).

### 1.3 Proposta de valor
- Frontend responsivo (React + Tailwind) com UX moderna.
- Backend .NET 8 com multi-tenant por connection string dinâmica e middleware dedicado.
- Sistema de permissões 100% compatível com VB6 (`PW~Tabelas`).
- Integração nativa com a API SVRS para sincronizar ClassTrib (IBS/CBS) usando certificado digital.
- Deploy único (publicação .NET + build frontend) empacotado como serviço Windows.

### 1.4 Stakeholders e personas
| Persona | Necessidade principal | Funcionalidades foco |
|---------|----------------------|----------------------|
| **Diretor Operacional** | Visão macro de vendas, estoque e compras | Dashboard, KPIs, relatórios |
| **Comprador/Vendedor** | Atualizar cadastros e pedidos com agilidade | Cadastros Gerais, Produtos, Orçamentos (roadmap) |
| **Analista Fiscal** | Garantir conformidade IBS/CBS e ClassTrib | Módulo ClassTrib + Classificação Fiscal |
| **TI/Infra** | Operar serviço, monitorar erros, gerenciar tenants | Windows Service, TenantsController, logs |

### 1.5 Premissas
- O banco legado permanece **compartilhado** entre VB6 e a nova solução.
- Não é permitido alterar o schema legado sem alinhamento prévio (regra de ouro).
- Comunicação multi-tenant depende do header `X-Tenant` ou host DNS.
- Certificados digitais (`.pfx`) são fornecidos por tenant e armazenados em `SistemaEmpresas/certificado/`.

---

## 2. Objetivos e metas

### 2.1 Objetivos principais
| Objetivo | Métrica de sucesso | Status |
|----------|-------------------|--------|
| Migrar telas críticas do VB6 para web | 100% das telas de produtos, usuários e cadastro geral entregues | ✅ Em uso |
| Manter compatibilidade com o legado | Zero erros de leitura/escrita no VB6 após deploys web | ✅ Monitorado via logs |
| Modernizar UX e reduzir tempo operacional | -50% no tempo médio de abertura de orçamentos | 🔄 Medindo (dashboard pronto) |
| Suportar múltiplos tenants | Tenants isolados por connection string + cache automático | ✅ Implantado |
| Automatizar ClassTrib IBS/CBS | Sincronização via API SVRS com auditoria | ✅ Sincronização manual (POST `/api/classtrib/sync`) |

### 2.2 Metas de negócio
- **Curto prazo (0-3 meses):** Consolidar dashboard, ClassTrib e Cadastro Geral (concluído).
- **Médio prazo (3-6 meses):** Entregar orçamentos e pedidos no frontend; automatizar sync ClassTrib via scheduler.
- **Longo prazo (6-12 meses):** Cobrir faturamento, notas fiscais e descomissionar telas VB6 selecionadas.

### 2.3 Critérios de sucesso
- Acesso web responsivo para usuários de múltiplas empresas.
- Autenticação JWT + refresh token funcionando em ambiente produtivo.
- Monitoramento proativo (logs + exception middleware) sem interferir no VB6.

---

## 3. Escopo funcional de alto nível
| Módulo | Status | Descrição |
|--------|--------|-----------|
| Autenticação & Sessão | ✅ | Login multi-tenant, refresh token e troca segura com VB6CryptoService |
| Dashboard | ✅ | KPIs, timeline de orçamentos, gráficos e lista de recentes |
| Produtos | ✅ | CRUD com filtros, paginação e compatibilidade com campos VB6 |
| Usuários & Permissões | ✅ | CRUD de usuários, grupos, seed automático de telas React |
| Permissões por Tela | ✅ | Hook `usePermissao`, guard de rotas e componentes condicionais |
| Cadastro Geral | ✅ | Clientes, fornecedores, transportadoras e vendedores em único cadastro |
| Classificação Fiscal (NCM) | ✅ | Busca, filtros e manutenção de classificações fiscais legadas |
| ClassTrib IBS/CBS | ✅ | Consulta, filtros avançados e sincronização com API SVRS usando certificado PFX |
| Tenants | ✅ | CRUD de tenants, cache em memória e middleware de injeção |
| Orçamentos/Compras | 🚧 | Em análise (dados expostos via Dashboard; CRUD planejado na Fase 3) |

---

## 4. Funcionalidades detalhadas

### 4.1 Autenticação e sessão
- **Fluxo:** usuário escolhe tenant → envia credenciais → backend descriptografa registros VB6 → gera JWT (1h) + refresh token (7 dias em cache).
- **Segurança:** algoritmo HS256, `Jwt:SecretKey` no appsettings, `ClockSkew = 0`.
- **Compatibilidade:** funções `VB6CryptoService.Encripta/Decripta` mantêm mesmo XOR + Base64 usado pelo VB6.
- **Endpoints principais:**
  - `POST /api/auth/login`
  - `POST /api/auth/refresh`
  - `GET /api/auth/me`

### 4.2 Dashboard
- KPIs: orçamentos abertos, compras pendentes, produtos/conjuntos ativos, estoque crítico.
- Visualizações: área (timeline 30 dias), pizza (status) e tabela de orçamentos recentes.
- Serviço: `DashboardController` com caches rápidos (2 minutos) para aliviar consultas pesadas.
- Frontend: `src/pages/Dashboard/DashboardPage.tsx` usa Recharts, skeleton loaders e botão de refresh.

### 4.3 Produtos
- Listagem com paginação, filtros por código/descrição/grupo e ordenação.
- CRUD completo, respeitando campos e validações do VB6.
- Permissões: tabela `PRODUTOS` em `PW~Tabelas`.

### 4.4 Usuários e permissões
- CRUD de usuários (`UsuariosController`) com criptografia VB6.
- Serviço `UsuarioManagementService` organiza grupos e sincronia com `PW~Grupos`.
- Permissões por tela via string `VIME` (visualizar/incluir/modificar/excluir).
- Hooks e componentes React (`usePermissao`, `ConditionalRender`, `DisableWithoutPermission`, `PermissionRoute`) controlam a UI.

### 4.5 Cadastro Geral (Clientes/Fornecedores/etc.)
- `GeralController` replica o comportamento do VB6 (uma única tabela para todos os cadastros).
- Recursos: filtros por tipo, busca global, listagem paginada, autocomplete, criação/edição detalhada, validações de campos obrigatórios.
- Integrações: carrega municípios, vendedores e dados fiscais relacionados.

### 4.6 Classificação Fiscal (NCM)
- Controller especializado com filtros por NCM, descrição e situação fiscal.
- Dados seguem 100% o layout das tabelas legadas para manter VB6 funcional.

### 4.7 ClassTrib IBS/CBS
- **Objetivo:** consumir API SVRS (`https://cff.svrs.rs.gov.br/api/v1/`) com certificado digital e sincronizar a tabela `ClassTrib` local.
- **HttpClient:** registrado com `ClassTribApiClient`, headers de navegador e certificado `X509Certificate2` (carregado por tenant/ambiente).
- **Serviço de sincronização:** `ClassTribSyncService` realiza bulk upsert, cacheia a última sincronização (24h) e expõe status.
- **Endpoints chave:**
  - `GET /api/classtrib` (paginado + filtros avançados)
  - `GET /api/classtrib/search|autocomplete|estatisticas`
  - `POST /api/classtrib/sync?forcar=false`

### 4.8 Administração de tenants
- `TenantDbContext` armazena `Tenants (Id, Nome, Dominio, ConnectionString, Ativo)`.
- `TenantService` usa `IMemoryCache` (expiração configurável via `TenantCache:ExpiracaoMinutos`) e expos métodos para limpeza (`LimparCache`).
- Middleware `UseTenantMiddleware()` injeta o tenant no `HttpContext.Items`, permitindo `AppDbContext` trocar a connection string dinamicamente.
- `TenantsController` oferece endpoints para listar, criar e ativar/desativar tenants.

### 4.9 Observabilidade e suporte ao usuário
- Middleware `UseGlobalExceptionHandler()` padroniza respostas de erro, loga exceções com `ILogger` e esconde detalhes em produção.
- Logs relevantes: autenticação, seleção de tenant, sincronizações, falhas de certificado.
- Mensagens amigáveis são propagadas ao frontend e exibidas nos toasts/snackbars.

---

## 5. Arquitetura técnica

### 5.1 Stack tecnológica
#### Backend (.NET 8)
| Componente | Tecnologia | Versão |
|------------|------------|--------|
| Framework | .NET | 8.0 |
| Linguagem | C# | 12 |
| ORM | Entity Framework Core | 8.0 |
| Autenticação | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) | 8.0.11 |
| Documentação | Swashbuckle/Swagger | 6.6.2 |
| Banco | SQL Server | 2014+ |
| Hospedagem | Windows Service (Microsoft.Extensions.Hosting.WindowsServices) | - |

#### Frontend (React)
| Componente | Tecnologia | Versão |
|------------|------------|--------|
| Framework | React | 19.1.1 |
| Linguagem | TypeScript | 5.9.3 |
| Build Tool | Vite | 7.1.7 |
| Roteamento | React Router DOM | 7.9.5 |
| Estilização | Tailwind CSS | 4.1.16 |
| Gráficos | Recharts | 3.5.0 |
| HTTP Client | Axios | 1.13.1 |
| Ícones | Lucide React | 0.552.0 |

### 5.2 Estrutura do projeto
```
SistemaEmpresas/
├── frontend/                 # React + TS + Vite
│   └── src/ (components, contexts, hooks, pages, services, types, utils)
├── SistemaEmpresas/          # API .NET 8
│   ├── Controllers/          # REST endpoints (Auth, Dashboard, Geral, etc.)
│   ├── Services/             # AuthService, TenantService, ClassTribSyncService...
│   ├── Repositories/         # Acesso a dados e filtros avançados
│   ├── Middleware/           # Exception + Tenant
│   ├── Data/                 # DbContexts, DbInitializer
│   └── certificado/          # PFX por tenant
└── publish/                  # Build pronto para deploy (API + wwwroot)
```

### 5.3 Fluxo multi-tenant
1. **Identificação:** header `X-Tenant` ou `Host` → `TenantMiddleware`.
2. **Cache:** `TenantService` consulta `TenantDbContext` e guarda a lista em memória.
3. **Contexto dinâmico:** `AppDbContext` lê `HttpContext.Items["Tenant"]` e troca a connection string antes de cada request.
4. **Regras de fallback:** se nenhum tenant encontrado, request é logado e retorna 401/404 conforme necessidade do endpoint.

### 5.4 Serviços cross-cutting
- **DbInitializer:** garante coluna `PW~SenhaHash`, cria usuário/grupo padrão e seeds das novas telas React.
- **Cache distribuído:** `AddDistributedMemoryCache` (pronto para Redis em produção).
- **HttpClient ClassTrib:** configura headers anti-bot, tempo limite (60s) e cookies próprios.
- **TLS:** switches aplicados no `Program.cs` habilitam TLS 1.0–1.3 e evitam avisos do SqlClient.

### 5.5 Dependências externas
| Sistema | Uso | Observações |
|---------|-----|-------------|
| API SVRS (IBS/CBS) | Sincronização ClassTrib | Requer certificado digital instalado e headers de navegador |
| SQL Server 2014+ | Banco principal e multi-tenant | Conexões definidas em `TenantDbContext` e `appsettings.*` |
| Windows Service | Hospedagem | Script `publish/install-service.ps1` auxilia instalação/gerência |

### 5.6 Dados sensíveis
- `appsettings.json` contém connection strings e secrets: proteger via `appsettings.{Environment}.json` + secret manager/KeyVault no futuro.
- Certificados `.pfx` devem permanecer fora do controle de versão.

---

## 6. APIs e contratos

### 6.1 Controllers expostos
| Controller | Base route | Principais operações |
|------------|------------|----------------------|
| `AuthController` | `/api/auth` | `login`, `refresh`, `me` |
| `DashboardController` | `/api/dashboard` | KPIs, timeline, status de orçamentos |
| `ProdutoController` | `/api/produto` | CRUD completo, filtros |
| `UsuariosController` | `/api/usuarios` | CRUD de usuários/grupos |
| `PermissoesController` | `/api/permissoes` | Consulta e atualização de permissões por tela |
| `GeralController` | `/api/geral` | Cadastro geral unificado |
| `ClassTribController` | `/api/classtrib` | Consultas, filtros, sync SVRS |
| `ClassificacaoFiscalController` | `/api/classificacaofiscal` | NCM, consultas avançadas |
| `TenantsController` | `/api/tenants` | Gerenciar tenants ativos |

### 6.2 Autenticação por header
```http
Authorization: Bearer <jwt_token>
X-Tenant: <dominio_tenant>
```

### 6.3 Exemplo de sincronização ClassTrib
```http
POST /api/classtrib/sync?forcar=true
Authorization: Bearer <token>
X-Tenant: irrigacao
```
Resposta (200):
```json
{
  "sucesso": true,
  "mensagem": "Sincronização concluída com sucesso. 1280 classificações processadas",
  "totalApiSvrs": 1280,
  "totalProcessado": 1204,
  "dataHoraInicio": "2025-11-29T10:23:12",
  "dataHoraFim": "2025-11-29T10:23:54",
  "tempoDecorrido": "00:00:42"
}
```

### 6.4 Convenções gerais
- Padrão REST com respostas JSON camelCase (
`JsonNamingPolicy.CamelCase`).
- Paginação: `page`, `pageSize`; resposta inclui `pageNumber`, `pageSize`, `totalItems`, `totalPages`.
- Filtros avançados aceitam múltiplos parâmetros (`csts`, `tipoAliquota`, `min/max` etc.).

---

## 7. Experiência do usuário (Frontend)
- **Rotas principais:** `/login`, `/dashboard`, `/produtos`, `/usuarios`, `/permissoes`, `/geral`, `/classtrib`, `/classificacao-fiscal`.
- **Contextos React:**
  - `AuthContext`: mantém tokens e tenant atual.
  - `ToastContext`: feedback de sucesso/erro.
- **Hooks principais:** `useAuth`, `useTenant`, `usePermissao`, `useQueryParams`.
- **Componentes compartilhados:** tabelas com filtros, skeleton loaders, guard de permissões e formulários reativos com validação.
- **Design system:** Tailwind 4 + tokens customizados para estados (azul = info, verde = sucesso, vermelho = crítico).

---

## 8. Modelo de dados e migração
- **Tenants:** tabela dedicada (`TenantDbContext`) com domínio e connection string criptografada se necessário.
- **PW~***: tabelas legadas compartilhadas (Grupos, Usuários, Tabelas/Permissões).
- **ClassTrib:** nova tabela (IBS/CBS) com campos `CodigoClassTrib`, `CodigoSituacaoTributaria`, `PercentualReducaoIBS/CBS`, flags de validade.
- **Seed automático:**
  - Cria grupo `Administradores` e usuário `nicolas/2510` (criptografado) se não existirem.
  - Adiciona permissões completas (`1111`) para telas React (`DASHBOARD`, `CLASSTRIB`, `USUARIOS`, `CONFIG`, `RELVENDAS`, `RELESTOQUE`, `RELFINANCEIRO`).
  - Valida/Cria coluna `PW~SenhaHash` no banco legado.

---

## 9. Segurança, operação e deploy

### 9.1 Ambientes e URLs
| Ambiente | URL | Porta | Observações |
|----------|-----|-------|-------------|
| Desenvolvimento Backend | http://localhost:5196 | 5196 | `dotnet run` dentro da pasta `SistemaEmpresas` |
| Desenvolvimento Frontend | http://localhost:5173 | 5173 | `npm run dev` em `frontend/` |
| Produção | http://servidor:5001 | 5001 | Serviço Windows + frontend em `wwwroot` |

### 9.2 Build local
```powershell
# Frontend
cd C:\Projetos\SistemaEmpresas\frontend
npm install
npm run build

# Backend
cd C:\Projetos\SistemaEmpresas\SistemaEmpresas
dotnet clean
dotnet restore
dotnet publish -c Release -o ..\publish --force

# Copiar dist para wwwroot
Copy-Item ..\frontend\dist\* ..\publish\wwwroot\ -Recurse -Force
```

### 9.3 Serviço Windows
```powershell
cd C:\SistemaEmpresas\publish
.\install-service.ps1 -Install   # cria serviço
.\install-service.ps1 -Start     # inicia
.\install-service.ps1 -Stop      # para
.\install-service.ps1 -Status    # status atual
```

### 9.4 Operação
- **Logs:** Event Viewer (Application) + console do serviço.
- **Exceções:** capturadas pelo `ExceptionMiddleware`, retornando payload padronizado `{ sucesso, mensagem, statusCode, timestamp }`.
- **Cache:**
  - Tenants: `MemoryCache` (padrão 30 min).
  - ClassTrib sync: distribuído (`AddDistributedMemoryCache`) com TTL de 24h.
- **Certificados:** armazenados em `publish/certificado/`. Configurar senhas via `appsettings` (não versionar em texto plano fora do dev).
- **Headers CORS:** política `AllowFrontend` libera `http://localhost:5173` e `5174` com credenciais.

### 9.5 Manutenção
- `TenantService.LimparCache()` pode ser acionado via endpoint administrativo ou job para refletir novas empresas.
- Sincronizações ClassTrib devem ser forçadas (`forcar=true`) ao importar certificados ou após mudanças tributárias relevantes.
- Monitorar certificados próximos do vencimento (console mostra validade ao subir o serviço).

---

## 10. Roadmap

### Fase 1 – MVP (✅ Concluído)
- Estrutura do projeto
- Autenticação JWT
- Multi-tenancy
- Dashboard com KPIs
- Sistema de permissões React + compatibilidade VB6

### Fase 2 – Cadastros e Fiscal (🔄 Finalizada nesta versão)
- [x] Gestão de produtos
- [x] Gestão de usuários
- [x] Cadastro Geral (clientes/fornecedores/vendedores)
- [x] Classificação fiscal (NCM) + ClassTrib IBS/CBS

### Fase 3 – Operacional (📋 Planejado)
- [ ] CRUD completo de orçamentos
- [ ] Pedidos de venda
- [ ] Pedidos de compra
- [ ] Integração com estoque avançado e notas fiscais

### Fase 4 – Avançado (🔮 Futuro)
- [ ] Relatórios avançados e BI
- [ ] Integrações bancárias e PIX
- [ ] Aplicativo mobile (React Native)
- [ ] Descomissionamento controlado do VB6

---

## 11. Requisitos não-funcionais

### 11.1 Performance
- Tempo de resposta médio da API < 500 ms (P95) em operações de leitura.
- Sincronização ClassTrib deve finalizar em < 2 minutos para ~1.200 registros.
- Cache de tenants (30 min) e dashboard (2 min) reduzem carga no SQL.

### 11.2 Disponibilidade e resiliência
- Serviço Windows reinicia automaticamente após falhas.
- Banco SQL redundante (recomendado) e backups diários.
- Middleware de exceção evita queda da aplicação por erros não tratados.

### 11.3 Segurança
- HTTPS obrigatório em produção (terminação via IIS/Reverse Proxy ou Kestrel com certificado).
- JWT + refresh token com storage criptografado no frontend.
- Permissões herdadas do VB6 e compatibilidade com grupos legados.

### 11.4 Compatibilidade
- Browsers suportados: Chrome, Edge e Firefox (últimas duas versões).
- Layout responsivo (desktop prioritário; tablet/mobile suportado).
- Código VB6 continua funcionando sem alterações estruturais.

### 11.5 Observabilidade
- Logs por módulo (`ILogger`) e mensagens claras no console durante startup.
- Planejado: enviar logs para Azure Application Insights ou Elastic.

---

## 12. Riscos e mitigação
| Risco | Impacto | Mitigação |
|-------|---------|-----------|
| Divergência entre VB6 e React ao manipular mesmas tabelas | Dados inconsistentes, falhas de auditoria | Manter regra de ouro: sem alterar schema; validar operações via testes integrados |
| Certificado digital expirado | Falha na sincronização ClassTrib | Monitorar validade (log no boot), configurar alertas e manter backup de certificados |
| Cache de tenants desatualizado | Usuário novo não acessa | Endpoint/command para limpar cache + TTL curto (30 min) |
| Falhas na API SVRS | Sincronização indisponível | Cache de 24h, opção `forcar` apenas quando necessário e mensagens claras ao usuário |
| Serviço Windows parado | Sistema fora do ar | Scripts `install-service.ps1` para start/stop, monitoramento via serviços do Windows |

---

## 13. Glossário
| Termo | Definição |
|-------|-----------|
| **Tenant** | Empresa/cliente identificado por domínio ou header `X-Tenant` |
| **VB6** | Sistema legado atual que compartilha o mesmo banco |
| **JWT** | JSON Web Token usado na autenticação |
| **ClassTrib** | Classificação tributária IBS/CBS provinda da API SVRS |
| **VIME** | Formato de permissão (Visualizar, Incluir, Modificar, Excluir) |
| **PW~Tabelas** | Tabela de permissões compartilhada com o VB6 |

---

## 14. Contatos e suporte
- **Repositório:** `github.com/Nicolasirrigpenapolis/SistemaIrrigacao`
- **Branch principal:** `main`
- **Documentação complementar:** arquivos `.md` na raiz (`DOCUMENTACAO_DEPLOY.md`, `GUIA_PERMISSOES.md`, `IMPLEMENTACAO_FILTROS_CLASSTRIB.md`).
- **Suporte técnico:** Equipe interna de TI/Infra (responsável pelo serviço Windows e certificados).

---

*Documento atualizado automaticamente em 29/11/2025.*
# PRD - Product Requirements Document
## Sistema Empresas

---

**Versão:** 1.0  
**Data:** 28 de Novembro de 2025  
**Autor:** Equipe de Desenvolvimento  
**Status:** Em Desenvolvimento

---

## 1. Visão Geral do Produto

### 1.1 Resumo Executivo

O **Sistema Empresas** é uma solução web moderna para gestão empresarial multi-tenant, desenvolvida para migrar gradualmente funcionalidades de um sistema legado em VB6 para uma arquitetura moderna baseada em React e .NET 8. O sistema mantém total compatibilidade com o banco de dados compartilhado, permitindo que ambos os sistemas (legado e novo) coexistam durante o período de transição.

### 1.2 Problema a Resolver

- **Sistema legado em VB6** com interface desatualizada e difícil manutenção
- Necessidade de **acesso web** às funcionalidades do sistema
- **Múltiplas empresas** (tenants) utilizando a mesma infraestrutura
- Demanda por **interface moderna** e responsiva
- Necessidade de **relatórios e dashboards** em tempo real

### 1.3 Solução Proposta

Uma aplicação web moderna que:
- Oferece interface React responsiva e intuitiva
- Mantém compatibilidade total com o banco de dados VB6
- Suporta múltiplos tenants (empresas) com isolamento de dados
- Implementa sistema de permissões granular compatível com o legado
- Fornece dashboards e KPIs em tempo real

---

## 2. Objetivos e Metas

### 2.1 Objetivos Principais

| Objetivo | Métrica de Sucesso |
|----------|-------------------|
| Migrar funcionalidades do VB6 para web | 100% das telas críticas migradas |
| Manter compatibilidade com sistema legado | Zero quebras no VB6 durante migração |
| Melhorar experiência do usuário | Redução de 50% no tempo de operações |
| Suportar múltiplos tenants | N tenants com isolamento total |

### 2.2 Metas de Negócio

- **Curto prazo (3 meses):** Dashboard, gestão de produtos, usuários e permissões
- **Médio prazo (6 meses):** Orçamentos, pedidos de compra, classificação fiscal
- **Longo prazo (12 meses):** Migração completa do sistema legado

---

## 3. Arquitetura Técnica

### 3.1 Stack Tecnológica

#### Backend (.NET 8)
| Componente | Tecnologia | Versão |
|------------|------------|--------|
| Framework | .NET | 8.0 |
| Linguagem | C# | 12 |
| ORM | Entity Framework Core | 8.0 |
| Autenticação | JWT Bearer | 8.0.11 |
| Documentação API | Swagger/Swashbuckle | 6.6.2 |
| Banco de Dados | SQL Server | 2014+ |
| Hospedagem | Windows Service | - |

#### Frontend (React)
| Componente | Tecnologia | Versão |
|------------|------------|--------|
| Framework | React | 19.1 |
| Linguagem | TypeScript | 5.9 |
| Build Tool | Vite | 7.1 |
| Roteamento | React Router | 7.9 |
| Estilização | Tailwind CSS | 4.1 |
| Gráficos | Recharts | 3.5 |
| HTTP Client | Axios | 1.13 |
| Ícones | Lucide React | 0.552 |

### 3.2 Estrutura do Projeto

```
SistemaEmpresas/
├── frontend/                 # Aplicação React + TypeScript + Vite
│   ├── src/
│   │   ├── components/       # Componentes reutilizáveis
│   │   ├── contexts/         # Contextos React (Auth, etc.)
│   │   ├── hooks/            # Custom hooks
│   │   ├── pages/            # Páginas da aplicação
│   │   ├── services/         # Comunicação com API
│   │   ├── types/            # Interfaces TypeScript
│   │   └── utils/            # Utilitários
│   ├── dist/                 # Build compilado
│   └── package.json
│
├── SistemaEmpresas/          # Backend .NET 8
│   ├── Controllers/          # API REST endpoints
│   ├── Services/             # Lógica de negócio
│   ├── Repositories/         # Acesso a dados
│   ├── Models/               # Entidades do banco
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Data/                 # DbContext e configurações
│   ├── Middleware/           # Middlewares customizados
│   └── certificado/          # Certificados digitais
│
└── publish/                  # Versão compilada para produção
    ├── SistemaEmpresas.exe
    └── wwwroot/              # Frontend compilado
```

### 3.3 Arquitetura Multi-Tenant

O sistema suporta múltiplos tenants (empresas) com:

- **Identificação por domínio:** Cada tenant é identificado pelo header `X-Tenant` ou hostname
- **Isolamento de dados:** Connection strings separadas por tenant
- **Cache de tenants:** Configuração em memória com expiração configurável
- **Tabela de Tenants:**

```sql
CREATE TABLE Tenants (
    Id INT PRIMARY KEY IDENTITY,
    Nome NVARCHAR(200) NOT NULL,
    Dominio NVARCHAR(200) NOT NULL UNIQUE,
    ConnectionString NVARCHAR(500) NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1
);
```

#### Tenants Configurados

| Tenant | Domínio | Descrição |
|--------|---------|-----------|
| Irrigação Penápolis | irrigacao | Sistema de Gestão Agrícola |
| Chinellato Transportes | chinellato | Logística e Frotas |

---

## 4. Funcionalidades do Sistema

### 4.1 Módulo de Autenticação

#### 4.1.1 Login
- **Descrição:** Autenticação de usuários com suporte multi-tenant
- **Fluxo:**
  1. Usuário seleciona a empresa (tenant)
  2. Insere credenciais (usuário e senha)
  3. Sistema valida contra banco de dados compartilhado
  4. Retorna JWT token + refresh token
- **Endpoints:**
  - `POST /api/auth/login` - Realizar login
  - `POST /api/auth/refresh` - Renovar token
  - `GET /api/auth/me` - Obter usuário atual

#### 4.1.2 Segurança
- **Criptografia:** Compatível com VB6 (XOR + Base64)
- **JWT Token:** Expiração de 1 hora
- **Refresh Token:** Expiração de 7 dias
- **Armazenamento:** LocalStorage com dados criptografados

### 4.2 Módulo de Dashboard

#### 4.2.1 KPIs Principais
| KPI | Descrição | Fonte |
|-----|-----------|-------|
| Orçamentos Abertos | Orçamentos não fechados e não cancelados | Tabela `Orcamento` |
| Compras Pendentes | Pedidos não fechados e não cancelados | Tabela `PedidoDeCompraNovo` |
| Total de Produtos | Produtos ativos cadastrados | Tabela `Produto` |
| Total de Conjuntos | Conjuntos ativos cadastrados | Tabela `Conjunto` |
| Estoque Crítico | Produtos abaixo do estoque mínimo | Tabela `Produto` |

#### 4.2.2 Gráficos
- **Timeline de Orçamentos:** Evolução de orçamentos nos últimos 30 dias
- **Pizza de Status:** Distribuição por status (Aberto, Fechado, Cancelado)
- **Lista de Recentes:** Últimos 5 orçamentos

### 4.3 Módulo de Produtos

#### 4.3.1 Listagem de Produtos
- Grid com paginação, filtros e ordenação
- Busca por código, descrição, grupo
- Exportação para relatórios

#### 4.3.2 Cadastro/Edição de Produtos
- Formulário completo com validações
- Campos compatíveis com VB6
- Upload de imagens (quando aplicável)

### 4.4 Módulo de Usuários

#### 4.4.1 Gerenciamento de Usuários
- CRUD completo de usuários
- Atribuição de grupos
- Criptografia compatível com VB6

#### 4.4.2 Tabelas de Segurança
| Tabela | Descrição |
|--------|-----------|
| `PW~Grupos` | Grupos de usuários (SUPERVISAO, VENDAS, etc.) |
| `PW~Usuarios` | Usuários do sistema |
| `PW~Tabelas` | Permissões por grupo/tabela |

### 4.5 Módulo de Permissões

#### 4.5.1 Sistema de Permissões
- **Formato:** String de 4 caracteres `"VIME"`
  - Posição 1: **V**isualizar (0/1)
  - Posição 2: **I**ncluir (0/1)
  - Posição 3: **M**odificar (0/1)
  - Posição 4: **E**xcluir (0/1)

**Exemplos:**
| Código | Permissões |
|--------|------------|
| `"1111"` | Acesso total |
| `"1000"` | Somente visualização |
| `"1100"` | Visualizar e incluir |
| `"0000"` | Sem acesso |

#### 4.5.2 Componentes de Permissão (Frontend)
- `usePermissao` - Hook para verificar permissões
- `ConditionalRender` - Renderização condicional
- `DisableWithoutPermission` - Desabilitar sem permissão
- `PermissionGuard` - Proteção de rotas

### 4.6 Módulo de Classificação Fiscal (ClassTrib)

#### 4.6.1 Funcionalidades
- Consulta de classificação tributária
- Sincronização com API externa
- Filtros avançados de pesquisa

### 4.7 Módulo Geral

#### 4.7.1 Cadastro de Dados Gerais
- Clientes, fornecedores, vendedores
- Dados compartilhados entre módulos

---

## 5. APIs e Endpoints

### 5.1 Controladores Disponíveis

| Controller | Rota Base | Descrição |
|------------|-----------|-----------|
| AuthController | `/api/auth` | Autenticação e autorização |
| DashboardController | `/api/dashboard` | KPIs e estatísticas |
| ProdutoController | `/api/produto` | CRUD de produtos |
| UsuariosController | `/api/usuarios` | Gestão de usuários |
| PermissoesController | `/api/permissoes` | Sistema de permissões |
| GeralController | `/api/geral` | Dados gerais |
| ClassTribController | `/api/classtrib` | Classificação fiscal |
| ClassificacaoFiscalController | `/api/classificacaofiscal` | Classificação fiscal |
| TenantsController | `/api/tenants` | Gestão de tenants |

### 5.2 Autenticação de Endpoints

```http
Authorization: Bearer <jwt_token>
X-Tenant: <dominio_tenant>
```

---

## 6. Regras de Compatibilidade VB6

### 6.1 Regra de Ouro

> **CRÍTICO:** O banco de dados é COMPARTILHADO entre VB6 e React/.NET. Qualquer alteração deve manter compatibilidade.

### 6.2 O que PODE fazer

✅ Usar as mesmas tabelas existentes (`PW~Grupos`, `PW~Usuarios`, etc.)  
✅ Usar a mesma função de criptografia (`VB6CryptoService`)  
✅ Manter o formato de dados exatamente como o VB6 espera  
✅ Criar interface moderna gravando no formato legado  

### 6.3 O que NÃO PODE fazer

❌ Criar colunas novas nas tabelas existentes  
❌ Mudar o formato dos dados criptografados  
❌ Alterar a estrutura das chaves primárias  
❌ Usar formatos de dados incompatíveis com VB6  

### 6.4 Serviço de Criptografia

```csharp
// VB6CryptoService.cs
public static string Encripta(string texto);   // Criptografar
public static string Decripta(string texto);   // Descriptografar
// Algoritmo: XOR + Base64 (compatível com VB6)
```

---

## 7. Deploy e Infraestrutura

### 7.1 Ambiente de Desenvolvimento

```powershell
# Frontend (Terminal 1)
cd C:\Projetos\SistemaEmpresas\frontend
npm run dev    # http://localhost:5173

# Backend (Terminal 2)
cd C:\Projetos\SistemaEmpresas\SistemaEmpresas
dotnet run     # http://localhost:5196
```

### 7.2 Build de Produção

```powershell
# Build completo
cd C:\Projetos\SistemaEmpresas
.\build.ps1

# Deploy para servidor
.\build.ps1 -Server
```

### 7.3 Serviço Windows

```powershell
# Instalação do serviço
cd C:\SistemaEmpresas\publish
.\install-service.ps1 -Install

# Comandos de gerenciamento
.\install-service.ps1 -Start    # Iniciar
.\install-service.ps1 -Stop     # Parar
.\install-service.ps1 -Status   # Status
```

### 7.4 Portas e URLs

| Ambiente | URL | Porta |
|----------|-----|-------|
| Produção | http://servidor:5001 | 5001 |
| Dev Frontend | http://localhost:5173 | 5173 |
| Dev Backend | http://localhost:5196 | 5196 |

---

## 8. Modelo de Dados

### 8.1 Principais Entidades

O sistema possui mais de **200 entidades** mapeadas do banco de dados legado. As principais são:

#### Segurança
- `PwGrupo` - Grupos de usuários
- `PwUsuario` - Usuários
- `PwTabela` - Permissões
- `Tenant` - Multi-tenancy

#### Comercial
- `Orcamento` - Orçamentos
- `Pedido` - Pedidos de venda
- `PedidoDeCompraNovo` - Pedidos de compra
- `NotaFiscal` - Notas fiscais

#### Cadastros
- `Produto` - Produtos
- `Conjunto` - Conjuntos/Kits
- `Geral` - Clientes/Fornecedores/Vendedores
- `ClassificacaoFiscal` - NCM e tributação

#### Estoque
- `MovimentoDoEstoque` - Movimentações
- `SimulaEstoque` - Simulações

---

## 9. Segurança

### 9.1 Autenticação

- **Método:** JWT (JSON Web Token)
- **Algoritmo:** HS256
- **Expiração:** 1 hora (access token), 7 dias (refresh token)

### 9.2 Autorização

- **Baseada em grupos:** SUPERVISAO, VENDAS, COMPRAS, etc.
- **Granularidade:** Por tabela/funcionalidade
- **Grupo admin:** SUPERVISAO tem acesso total

### 9.3 Proteção de Dados

- Senhas criptografadas (compatível VB6)
- HTTPS em produção
- Certificados digitais para NFe

---

## 10. Roadmap

### Fase 1 - MVP (Concluído) ✅
- [x] Estrutura do projeto
- [x] Autenticação JWT
- [x] Multi-tenancy
- [x] Dashboard com KPIs
- [x] Sistema de permissões

### Fase 2 - Cadastros (Em Andamento) 🔄
- [x] Gestão de produtos
- [x] Gestão de usuários
- [ ] Gestão de clientes/fornecedores
- [ ] Classificação fiscal completa

### Fase 3 - Operacional (Planejado) 📋
- [ ] Orçamentos
- [ ] Pedidos de venda
- [ ] Pedidos de compra
- [ ] Notas fiscais

### Fase 4 - Avançado (Futuro) 🔮
- [ ] Relatórios avançados
- [ ] Integração bancária
- [ ] App mobile
- [ ] Migração total do VB6

---

## 11. Requisitos Não-Funcionais

### 11.1 Performance
- Tempo de resposta API: < 500ms (95th percentile)
- Cache de tenants: 30 minutos
- Cache de dashboard: 2 minutos

### 11.2 Disponibilidade
- Uptime: 99.5%
- Recuperação automática via Windows Service

### 11.3 Escalabilidade
- Suporte a múltiplos tenants
- Arquitetura stateless (exceto cache)

### 11.4 Compatibilidade
- Navegadores: Chrome, Firefox, Edge (últimas 2 versões)
- Mobile: Responsivo (não nativo)

---

## 12. Glossário

| Termo | Definição |
|-------|-----------|
| **Tenant** | Empresa/organização que utiliza o sistema |
| **VB6** | Visual Basic 6, sistema legado |
| **JWT** | JSON Web Token, padrão de autenticação |
| **KPI** | Key Performance Indicator |
| **DTO** | Data Transfer Object |
| **NCM** | Nomenclatura Comum do Mercosul |

---

## 13. Contatos e Suporte

- **Repositório:** SistemaIrrigacao
- **Branch Principal:** main
- **Documentação:** Arquivos `.md` na raiz do projeto

---

*Documento gerado automaticamente em 28/11/2025*
