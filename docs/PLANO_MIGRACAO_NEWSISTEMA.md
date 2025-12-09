# 📋 Plano de Migração: NewSistema → SistemaEmpresas

**Data:** 09/12/2024  
**Objetivo:** Analisar e trazer funcionalidades relevantes do projeto NewSistema para o SistemaEmpresas atual.

---

## 📊 Visão Geral

### NewSistema - Estrutura Identificada

**Backend (ASP.NET Core)**
- **Models:** 22 modelos identificados
  - Gestão de Transporte: Veiculo, Reboque, Condutor, Viagem, DespesaViagem, ReceitaViagem
  - MDFe: MDFe, MDFeStatusHistory
  - Cadastros: Emitente, Contratante, Fornecedor, Seguradora
  - Manutenção: ManutencaoVeiculo, ManutencaoPeca
  - Permissões: Usuario, Cargo, Permissao, CargoPermissao
  - Geo: Estado, Municipio
  - Config: ConfiguracaoEmpresa

- **Controllers:** 22 controllers
- **Services:** TenantService, PermissaoService, CacheService, MDFeBusinessService, IBGEService, etc.
- **Repositories:** GenericRepository, PermissaoRepository
- **Migrations:** 24 migrations (histórico completo de evolução)

**Padrões Arquiteturais:**
- ✅ Multi-tenant dinâmico (TenantService + ConfiguracaoEmpresa)
- ✅ Generic Repository Pattern
- ✅ BaseController com DTOs separados (List/Detail/Create/Update)
- ✅ CacheService integrado
- ✅ Soft Delete em todas entidades
- ✅ Health Checks
- ✅ Response Compression + Output Cache
- ✅ Auditoria (DataCriacao, DataUltimaAlteracao, UsuarioExclusao)

---

### SistemaEmpresas Atual - Estrutura

**Backend (ASP.NET Core)**
- **Models:** 200+ modelos (sistema ERP completo de irrigação)
  - Sistema legado VB6: PwUsuario, PwGrupo, PwTabela
  - Novos: GrupoUsuario, PermissoesTela, PermissoesTemplate
  - Financeiro, Estoque, Vendas, Produção, etc.
  - Já existe: Emitente, Motorista, VeiculoDoMotoristum, Fornecedore, etc.

- **Controllers:** 13 controllers
- **Services:** AuthService, TenantService, ClassTribSyncService, PermissoesTelaService, etc.

**Observações:**
- 🔴 Sistema híbrido: parte legado VB6, parte novo C#
- 🔴 Usuarios ainda usa tabela PW~Usuarios (migração parcial)
- 🟢 Já tem sistema de tenants
- 🟢 Já tem emitentes cadastrados
- 🟢 Sistema de permissões em fase de implementação

---

## 🔍 Análise Detalhada por Módulo

### 1️⃣ Sistema de Usuários e Permissões

#### NewSistema
```
Usuario
├── Id
├── UserName
├── Nome
├── PasswordHash
├── CargoId (FK)
└── Soft Delete (DataExclusao, UsuarioExclusao, MotivoExclusao)

Cargo
├── Id
├── Nome
├── Descricao
└── Relacionamento N:N com Permissao

Permissao
├── Id
├── Codigo
├── Nome
├── Descricao
└── Modulo

CargoPermissao (tabela associativa)
├── CargoId
├── PermissaoId
└── DataCriacao
```

#### SistemaEmpresas Atual
```
PwUsuario (legado)
├── PW~Nome (PK)
├── PW~Senha (PK) - texto plano
├── PW~SenhaHash - BCrypt (migração gradual)
├── PW~Grupo
├── GrupoUsuarioId (FK) - novo sistema
└── PwAtivo

GrupoUsuario (novo)
├── Id
├── Nome
├── Descricao
├── Ativo
└── GrupoSistema (flag para grupos imutáveis)

PermissoesTela
├── Id
├── Tela
├── GrupoUsuarioId
└── Permissoes (flags: Criar, Editar, Visualizar, Excluir)
```

**🎯 Decisão: ADAPTAR ao existente**
- ✅ **Manter** GrupoUsuario (equivale a Cargo)
- ✅ **Manter** PwUsuario por compatibilidade legado
- 🔄 **Melhorar** PermissoesTela para ser mais flexível como no NewSistema
- 🔄 **Trazer** padrão de Soft Delete para usuários
- 🔄 **Trazer** PermissaoService com cache

---

### 2️⃣ Módulo de Veículos

#### NewSistema
```
Veiculo
├── Id
├── Placa (8 chars, required)
├── Marca (100 chars)
├── Tara (int, required)
├── TipoRodado (50 chars, required)
├── TipoCarroceria (50 chars, required)
├── Uf (2 chars, required)
├── Ativo
├── DataCriacao, DataUltimaAlteracao
└── Soft Delete

Reboque
├── Similar ao Veiculo
└── Para carretas/reboques
```

#### SistemaEmpresas Atual
```
VeiculoDoMotoristum
├── VeiPlaca (PK)
├── VeiMarca
├── VeiModelo
├── VeiAnoFab
├── VeiAnoMod
└── (outros campos)

ControleDePneu
└── Relacionado a veículos
```

**🎯 Decisão: CRIAR NOVO ou ADAPTAR?**

**Opção A - Criar tabela Veiculo nova (RECOMENDADO)**
- ✅ Modelo mais simples e focado
- ✅ Soft Delete nativo
- ✅ Pronto para MDFe
- 🔴 Duplicação com VeiculoDoMotoristum
- 💡 Solução: Avaliar se VeiculoDoMotoristum é usado ativamente

**Opção B - Adaptar VeiculoDoMotoristum**
- ✅ Sem duplicação
- 🔴 Mais complexo
- 🔴 Pode quebrar sistema legado

---

### 3️⃣ Módulo de Condutores (Motoristas)

#### NewSistema
```
Condutor
├── Id
├── Nome (200 chars, required)
├── Cpf (11 chars, required)
├── Telefone (20 chars)
├── Ativo
└── Soft Delete
```

#### SistemaEmpresas Atual
```
Motorista
├── MotCodigo (PK, identity)
├── MotNome
├── MotCpf
├── MotRg
├── MotEndereco
├── MotCidade
└── (muitos outros campos)
```

**🎯 Decisão: REUSAR Motorista**
- ✅ Tabela Motorista já existe e é mais completa
- 🔄 Adicionar Soft Delete se necessário
- 🔄 Criar view ou adapter se precisar simplificar para MDFe

---

### 4️⃣ Módulo de Viagens

#### NewSistema
```
Viagem
├── Id
├── VeiculoId (FK, required)
├── CondutorId (FK)
├── DataInicio (required)
├── DataFim (required)
├── KmInicial, KmFinal
├── OrigemDestino (500 chars)
├── Observacoes (1000 chars)
├── ReceitaTotal (calculado)
├── TotalDespesas (calculado)
└── SaldoLiquido (calculado)

DespesaViagem
├── Id
├── ViagemId (FK)
├── TipoDespesa
├── Descricao
├── Valor (decimal 18,2)
├── DataDespesa
└── Local

ReceitaViagem
├── Id
├── ViagemId (FK)
├── Descricao
├── Valor
├── DataReceita
└── Origem
```

#### SistemaEmpresas Atual
```
RelatorioDeViagem
├── RelCodigo (PK)
├── RelData
├── RelVeiculo
├── RelMotorista
├── RelKmSaida, RelKmChegada
└── (outros)

ItenDaViagem
├── IteRelCodigo (FK)
└── Itens da viagem

ParcelaDaViagem
├── ParRelCodigo (FK)
└── Parcelas de pagamento
```

**🎯 Decisão: AVALIAR USO ATUAL**

Se **RelatorioDeViagem está em uso ativo:**
- 🔴 **NÃO TRAZER** módulo de Viagens
- ✅ Apenas usar dados existentes para MDFe

Se **RelatorioDeViagem NÃO é usado ou está obsoleto:**
- ✅ **TRAZER** módulo completo de Viagens do NewSistema
- ✅ Mais moderno, com cálculos automáticos
- ✅ DTOs bem estruturados

---

### 5️⃣ Módulo MDFe (Manifesto Eletrônico)

#### NewSistema
```
MDFe (modelo GIGANTE - 1373 linhas!)
├── Dados do Emitente (snapshot)
├── Dados do Condutor (snapshot)
├── Dados do Veículo (snapshot)
├── Dados do Reboque (snapshot)
├── Percurso (UFs)
├── Carregamento/Descarregamento (municípios)
├── Documentos vinculados (NFes)
├── Seguro, Vale Pedágio
├── Totalizadores de carga
├── Informações de autorização SEFAZ
└── Status e rastreabilidade completa

MDFeStatusHistory
├── Histórico de mudanças de status
└── Auditoria completa

MDFeBusinessService
└── Lógica de negócio complexa
```

#### SistemaEmpresas Atual
```
NotaFiscal
└── Sistema já implementado

InutilizacaoNfe, CancelamentoNfe, CartaDeCorrecaoNfe
└── Gestão de NF-e
```

**🎯 Decisão: TRAZER SE NECESSÁRIO**

**Depende do negócio:**
- ❓ A empresa precisa emitir MDF-e?
- ❓ Faz transporte de cargas?

**Se SIM:**
- ✅ **TRAZER** módulo completo MDFe
- ✅ É um módulo standalone
- ✅ Não conflita com NF-e existente
- 🔄 Depende de: Veiculo, Condutor, Emitente

**Se NÃO:**
- 🔴 **IGNORAR** por enquanto
- 💡 Deixar documentado para futuro

---

### 6️⃣ Módulos Cadastrais

#### Fornecedor
- **NewSistema:** Tem modelo Fornecedor com Observacoes
- **SistemaEmpresas:** Já tem tabela `Fornecedore`
- **Decisão:** ✅ MANTER o existente

#### Seguradora
- **NewSistema:** Tem modelo Seguradora
- **SistemaEmpresas:** ❓ Verificar se existe
- **Decisão:** 🔄 Trazer se não existir

#### Contratante
- **NewSistema:** Modelo específico para contratantes de frete
- **SistemaEmpresas:** Pode ser que já exista como Geral/Cliente
- **Decisão:** 🔄 Avaliar necessidade vs. duplicação

---

### 7️⃣ Módulo de Manutenção

#### NewSistema
```
ManutencaoVeiculo
├── Manutenções de veículos
└── Controle de peças

ManutencaoPeca
└── Peças utilizadas
```

#### SistemaEmpresas Atual
```
ManutencaoConta
ManutencaoHidroturbo
ManutencaoPivo
```

**🎯 Decisão: AVALIAR CONTEXTO**
- Se empresa **não usa veículos**: 🔴 IGNORAR
- Se empresa **tem frota**: ✅ TRAZER
- Parece ser outro contexto (irrigação vs. transporte)

---

### 8️⃣ Padrões Arquiteturais e Infraestrutura

#### O que trazer do NewSistema:

**✅ TRAZER - Alta Prioridade:**

1. **GenericRepository Pattern**
   - Reduz código duplicado
   - Facilita CRUD operations
   - Já implementado e testado

2. **BaseController com DTOs separados**
   ```csharp
   BaseController<TEntity, TListDto, TDetailDto, TCreateDto, TUpdateDto>
   ```
   - Separação clara de responsabilidades
   - DTOs específicos para cada operação
   - Melhor performance (menos dados trafegados)

3. **Soft Delete padrão**
   ```csharp
   DateTime? DataExclusao
   string? UsuarioExclusao
   string? MotivoExclusao
   ```
   - Auditoria completa
   - Recuperação de dados
   - Compliance

4. **CacheService melhorado**
   - Cache em memória
   - Invalidação inteligente
   - Redução de carga no DB

5. **Output Cache + Response Compression**
   - Melhor performance em listagens
   - Redução de payload
   - Configurado no Program.cs

**🔄 ADAPTAR:**

6. **Multi-tenant dinâmico**
   - NewSistema: ConfiguracaoEmpresa em JSON
   - SistemaEmpresas: Tenant em banco
   - Adaptar melhor dos dois mundos

**🔴 NÃO TRAZER:**

7. **ACBrLib MDFe** (se não for usar MDF-e)
8. **Health Checks** (pode ser adicionado depois)

---

## 📋 Plano de Implementação Sugerido

### Fase 1: Fundação (PRIORITÁRIO)
**Objetivo:** Melhorar arquitetura base sem quebrar nada

1. ✅ **Implementar GenericRepository**
   - Criar IGenericRepository<T>
   - Criar GenericRepository<T>
   - Sem impacto em código existente

2. ✅ **Criar padrão de DTOs**
   - Criar pasta DTOs/
   - Definir ListDto, DetailDto, CreateDto, UpdateDto base
   - Implementar em 1-2 controllers como piloto

3. ✅ **Melhorar CacheService**
   - Trazer versão do NewSistema
   - Substituir/melhorar o existente
   - Adicionar cache em endpoints críticos

4. ✅ **Adicionar Soft Delete**
   - Criar migration para adicionar campos em tabelas chave
   - Implementar em models principais
   - Não precisa ser em tudo de uma vez

**Tempo estimado:** 2-3 semanas

---

### Fase 2: Sistema de Permissões (IMPORTANTE)
**Objetivo:** Padronizar e melhorar controle de acesso

1. 🔄 **Refatorar PermissoesTela**
   - Migrar para modelo Permissao + GrupoPermissao
   - Criar tabela associativa
   - Manter retrocompatibilidade

2. 🔄 **Trazer PermissaoService**
   - Implementar com cache
   - Métodos: GetUserPermissions, HasPermission, etc.
   - Integrar com controllers

3. 🔄 **Criar middleware de autorização**
   - Baseado em códigos de permissão
   - Atributos [RequirePermission("codigo")]

**Tempo estimado:** 2 semanas

---

### Fase 3: Módulos de Negócio (CONDICIONAL)
**Objetivo:** Trazer funcionalidades se fizerem sentido para o negócio

#### 3A: Se empresa trabalha com transporte/MDF-e

1. ✅ **Módulo de Veículos**
   - Criar tabela Veiculo (novo modelo limpo)
   - Migração
   - Controller + DTOs
   - CRUD completo

2. ✅ **Adaptar Motorista para Condutor**
   - Usar tabela Motorista existente
   - Criar adapter/view se necessário

3. ✅ **Módulo de Viagens** (se não houver conflito)
   - Viagem, DespesaViagem, ReceitaViagem
   - Controllers + Services
   - Relatórios

4. ✅ **Módulo MDFe** (GRANDE)
   - MDFe, MDFeStatusHistory
   - MDFeBusinessService
   - Integração ACBrLib
   - **ATENÇÃO:** Projeto grande e complexo

**Tempo estimado:** 6-8 semanas

#### 3B: Se empresa NÃO trabalha com transporte

- 🔴 **PULAR** Fase 3
- 💡 Focar em melhorias do core

---

### Fase 4: Otimizações (FUTURO)

1. Response Compression
2. Output Cache policies
3. Health Checks
4. Melhorias de performance baseadas no NewSistema

**Tempo estimado:** 1-2 semanas

---

## ⚠️ Riscos e Considerações

### Riscos Técnicos

1. **🔴 ALTO - Compatibilidade com sistema legado VB6**
   - Mudanças em PwUsuario podem quebrar VB6
   - Mitigation: Manter retrocompatibilidade, testar extensivamente

2. **🟡 MÉDIO - Duplicação de tabelas**
   - Veiculo vs VeiculoDoMotoristum
   - Mitigation: Decidir claramente qual usar

3. **🟡 MÉDIO - Migrations em produção**
   - Sistema em uso, precisa de downtime?
   - Mitigation: Migrations não-destrutivas, rollback plans

4. **🟢 BAIXO - Padrões arquiteturais**
   - GenericRepository, DTOs são aditivos
   - Não quebram código existente

### Riscos de Negócio

1. **❓ Funcionalidades realmente necessárias?**
   - Validar com stakeholders se MDFe/Viagens fazem sentido
   - Não trazer módulos desnecessários

2. **⏰ Tempo vs. Valor**
   - MDFe é um projeto grande
   - Avaliar ROI antes de começar

---

## 🎯 Recomendações Finais

### FAZER AGORA (Alta prioridade e baixo risco):

1. ✅ **GenericRepository** - Melhora arquitetura, fácil de implementar
2. ✅ **DTOs padrão** - Organiza código, melhora performance
3. ✅ **CacheService** - Performance imediata, baixo risco
4. ✅ **Soft Delete** - Auditoria, segurança, recuperação de dados

### FAZER DEPOIS (Após validação de necessidade):

5. 🔄 **Sistema de Permissões melhorado** - Se o atual não atende
6. 🔄 **Módulo de Veículos/Viagens** - Se empresa trabalha com transporte
7. 🔄 **MDFe completo** - Se houver demanda real do negócio

### NÃO FAZER (Ou deixar para muito depois):

8. 🔴 **Reescrever sistema de usuários** - Legado funciona, não mexer
9. 🔴 **Trazer tudo de uma vez** - Risco muito alto
10. 🔴 **Duplicar cadastros** - Usar os existentes (Fornecedor, etc.)

---

## 📝 Próximos Passos

1. **Revisar este documento com a equipe/stakeholders**
2. **Validar quais módulos fazem sentido para o negócio**
3. **Priorizar Fase 1 (fundação arquitetural)**
4. **Criar backlog detalhado da Fase 1**
5. **Implementar piloto com 1-2 funcionalidades**
6. **Avaliar resultados antes de continuar**

---

## 📚 Documentos Relacionados

- [PRD.md](./PRD.md) - Requisitos do produto
- [GUIA_PERMISSOES.md](./GUIA_PERMISSOES.md) - Sistema de permissões atual
- [VERSIONAMENTO_SISTEMA.md](./VERSIONAMENTO_SISTEMA.md) - Controle de versão

---

**Elaborado por:** GitHub Copilot  
**Data:** 09/12/2024  
**Status:** 🟢 Pronto para revisão
