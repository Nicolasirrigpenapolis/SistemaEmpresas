# 🚀 Plano de Implementação - Tela de Nota Fiscal

## 📋 Resumo Executivo

Este documento detalha o plano de implementação da tela de Nota Fiscal no novo sistema React/.NET, replicando a funcionalidade do VB6 (NOTAFISC.FRM).

---

## 🎯 Escopo da Implementação

### O que será implementado:
✅ Tela completa de Nota Fiscal com todas as abas
✅ Grids para Produtos, Conjuntos, Peças e Serviços
✅ Cálculo automático de todos os impostos
✅ Totalizadores em tempo real
✅ Parcelas de pagamento
✅ Emissão de NFe via ACBrLibNFe (substituindo FlexDocs)
✅ DANFE em PDF
✅ Consulta status SEFAZ
✅ Cancelamento e CCe

### O que NÃO será implementado nesta fase:
❌ NFSe (será fase posterior)
❌ Importação XML (será fase posterior)
❌ Manifesto de Destinatário (será fase posterior)

---

## 📐 Arquitetura Proposta

```
┌─────────────────────────────────────────────────────────────────────┐
│                         FRONTEND (React)                            │
├─────────────────────────────────────────────────────────────────────┤
│  NotaFiscalFormPage.tsx                                             │
│  ├── DadosGeraisSection        (Cliente, Natureza, Datas)          │
│  ├── ItensSection                                                   │
│  │   ├── ProdutosGrid          (F2 = Recalcular)                   │
│  │   ├── ConjuntosGrid         (F2 = Recalcular)                   │
│  │   ├── PecasGrid             (F2 = Recalcular)                   │
│  │   └── ServicosGrid                                               │
│  ├── TotalizadoresPanel        (Impostos, Totais)                  │
│  ├── ParcelasSection           (Grid de parcelas)                  │
│  └── TransporteSection         (Frete, Volumes)                    │
└─────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────┐
│                          BACKEND (.NET 8)                           │
├─────────────────────────────────────────────────────────────────────┤
│  NotaFiscalController                                               │
│  ├── POST /api/notas-fiscais                                       │
│  ├── GET  /api/notas-fiscais/{id}                                  │
│  ├── PUT  /api/notas-fiscais/{id}                                  │
│  ├── POST /api/notas-fiscais/{id}/itens                            │
│  ├── POST /api/notas-fiscais/{id}/calcular-item                    │
│  ├── POST /api/notas-fiscais/{id}/totalizar                        │
│  ├── POST /api/notas-fiscais/{id}/emitir                           │
│  └── POST /api/notas-fiscais/{id}/cancelar                         │
├─────────────────────────────────────────────────────────────────────┤
│  Services                                                           │
│  ├── NotaFiscalService         (Orquestração)                      │
│  ├── ImpostoCalculatorService  (Cálculos fiscais)                  │
│  ├── TotalizadorService        (Soma totais)                       │
│  └── NFeService                (ACBrLibNFe)                        │
├─────────────────────────────────────────────────────────────────────┤
│  Repositories                                                       │
│  ├── NotaFiscalRepository                                          │
│  ├── ItemNotaFiscalRepository                                      │
│  └── ParcelaNotaFiscalRepository                                   │
└─────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────┐
│                        ACBrLibNFe (DLL)                             │
├─────────────────────────────────────────────────────────────────────┤
│  ├── GerarXML()                                                    │
│  ├── Assinar()                                                     │
│  ├── Validar()                                                     │
│  ├── Enviar()                                                      │
│  ├── Consultar()                                                   │
│  ├── Cancelar()                                                    │
│  └── GerarDANFE()                                                  │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 📅 Fases de Implementação

### 🔵 FASE 1: Infraestrutura Backend (3-4 dias)

#### 1.1 DTOs
```csharp
// NotaFiscalDtos.cs
public record NotaFiscalListDto { ... }
public record NotaFiscalDetalheDto { ... }
public record NotaFiscalCreateDto { ... }
public record NotaFiscalUpdateDto { ... }
public record ItemNotaFiscalDto { ... }
public record CalculoImpostoResultDto { ... }
public record TotalizadoresDto { ... }
```

#### 1.2 ImpostoCalculatorService
Mapear todos os 19 tipos de cálculo:
```csharp
public class ImpostoCalculatorService
{
    // Oq = 1: CFOP
    public string CalcularCfop(CalculoImpostoInput input);
    
    // Oq = 2: % Redução BC
    public decimal CalcularPercentualReducao(CalculoImpostoInput input);
    
    // Oq = 3: % ICMS
    public decimal CalcularAliquotaIcms(CalculoImpostoInput input);
    
    // ... demais métodos ...
    
    // Método principal que orquestra tudo
    public CalculoImpostoResult CalcularTodosImpostos(ItemNotaFiscal item);
}
```

#### 1.3 NotaFiscalService
```csharp
public class NotaFiscalService
{
    public Task<NotaFiscalDetalheDto> CriarAsync(NotaFiscalCreateDto dto);
    public Task<ItemNotaFiscalDto> AdicionarItemAsync(int notaId, ItemNotaFiscalCreateDto dto);
    public Task<CalculoImpostoResultDto> ProcessarItemAsync(int notaId, int itemId);
    public Task<TotalizadoresDto> TotalizarAsync(int notaId);
    public Task<ValidationResult> ValidarParaEmissaoAsync(int notaId);
}
```

---

### 🟢 FASE 2: API Endpoints (2-3 dias)

#### Endpoints CRUD:
```
POST   /api/notas-fiscais                    → Criar nota fiscal
GET    /api/notas-fiscais                    → Listar notas fiscais
GET    /api/notas-fiscais/{id}               → Buscar nota por ID
PUT    /api/notas-fiscais/{id}               → Atualizar nota
DELETE /api/notas-fiscais/{id}               → Excluir/Cancelar nota
```

#### Endpoints de Itens:
```
POST   /api/notas-fiscais/{id}/produtos      → Adicionar produto
PUT    /api/notas-fiscais/{id}/produtos/{itemId}  → Editar produto
DELETE /api/notas-fiscais/{id}/produtos/{itemId}  → Remover produto

POST   /api/notas-fiscais/{id}/conjuntos     → Adicionar conjunto
PUT    /api/notas-fiscais/{id}/conjuntos/{itemId} → Editar conjunto
DELETE /api/notas-fiscais/{id}/conjuntos/{itemId} → Remover conjunto

POST   /api/notas-fiscais/{id}/pecas         → Adicionar peça
PUT    /api/notas-fiscais/{id}/pecas/{itemId}     → Editar peça
DELETE /api/notas-fiscais/{id}/pecas/{itemId}     → Remover peça

POST   /api/notas-fiscais/{id}/servicos      → Adicionar serviço
PUT    /api/notas-fiscais/{id}/servicos/{itemId}  → Editar serviço
DELETE /api/notas-fiscais/{id}/servicos/{itemId}  → Remover serviço
```

#### Endpoints de Cálculo:
```
POST /api/notas-fiscais/{id}/calcular-item   → Recalcular impostos de um item
POST /api/notas-fiscais/{id}/totalizar       → Recalcular totais da NF
POST /api/notas-fiscais/{id}/validar         → Validar para emissão
```

#### Endpoints NFe:
```
POST /api/notas-fiscais/{id}/emitir          → Emitir NFe (SEFAZ)
GET  /api/notas-fiscais/{id}/consultar       → Consultar status SEFAZ
POST /api/notas-fiscais/{id}/cancelar        → Cancelar NFe
POST /api/notas-fiscais/{id}/cce             → Carta de Correção
GET  /api/notas-fiscais/{id}/danfe           → Gerar DANFE PDF
GET  /api/notas-fiscais/{id}/xml             → Download XML
```

---

### 🟡 FASE 3: Frontend - Estrutura Base (2-3 dias)

#### Estrutura de Arquivos:
```
frontend/src/
├── pages/
│   └── NotasFiscais/
│       ├── index.ts                    # Barrel export
│       ├── NotaFiscalListPage.tsx      # Lista de NFs
│       ├── NotaFiscalFormPage.tsx      # Formulário principal
│       └── components/
│           ├── DadosGeraisSection.tsx
│           ├── ClienteSelector.tsx
│           ├── NaturezaSelector.tsx
│           ├── ProdutosGrid.tsx
│           ├── ConjuntosGrid.tsx
│           ├── PecasGrid.tsx
│           ├── ServicosGrid.tsx
│           ├── ItemFormModal.tsx
│           ├── TotalizadoresPanel.tsx
│           ├── ParcelasGrid.tsx
│           ├── TransporteSection.tsx
│           └── AcoesNFe.tsx
├── services/
│   └── notaFiscalService.ts
└── types/
    └── notaFiscal.ts
```

---

### 🟠 FASE 4: Frontend - Grids e Cálculos (4-5 dias)

#### ProdutosGrid.tsx
```tsx
// Funcionalidades:
// - Adicionar produto (busca por código/descrição)
// - Editar item inline
// - Excluir item
// - F2 = Recalcular impostos do item
// - Tab na última coluna = Processa e vai para próxima linha
// - Exibir colunas: Código, Descrição, Qtd, Valor Unit, 
//                   Valor Total, ICMS, IPI, ST, etc.
```

#### TotalizadoresPanel.tsx
```tsx
// Exibe em tempo real:
// - Total de Produtos
// - Total de Conjuntos
// - Total de Peças
// - Total de Serviços
// - Base ICMS
// - Valor ICMS
// - Valor IPI
// - Valor ICMS ST
// - Valor PIS
// - Valor COFINS
// - Valor IBS
// - Valor CBS
// - Frete
// - Seguro
// - Despesas
// - Desconto
// - VALOR TOTAL DA NF
```

---

### 🔴 FASE 5: Integração ACBrLibNFe (3-4 dias)

#### 5.1 Instalação e Configuração
- Baixar e configurar ACBrLibNFe
- Configurar certificado digital
- Configurar ambiente (Homologação/Produção)

#### 5.2 NFeService.cs
```csharp
public class NFeService
{
    // Gera XML da NFe
    public string GerarXml(NotaFiscal nota);
    
    // Assina XML com certificado
    public string AssinarXml(string xml);
    
    // Valida XML contra schema
    public ValidationResult ValidarXml(string xml);
    
    // Envia para SEFAZ
    public EmissaoResult Emitir(string xmlAssinado);
    
    // Consulta status na SEFAZ
    public ConsultaResult Consultar(string chaveNfe);
    
    // Cancela NFe
    public CancelamentoResult Cancelar(string chaveNfe, string justificativa);
    
    // Gera PDF do DANFE
    public byte[] GerarDanfe(string xml);
}
```

---

## 📊 Mapeamento VB6 → .NET

### Função CalculaImposto → ImpostoCalculatorService

| VB6 Case | .NET Method | Descrição |
|----------|-------------|-----------|
| Case 1 | CalcularCfop() | CFOP |
| Case 2 | CalcularPercentualReducao() | % Redução BC |
| Case 3 | CalcularAliquotaIcms() | % ICMS |
| Case 4 | CalcularAliquotaIpi() | % IPI |
| Case 5 | CalcularCst() | CST |
| Case 6 | CalcularBaseIcms() | BC ICMS |
| Case 7 | CalcularValorIcms() | Valor ICMS |
| Case 8 | CalcularValorIpi() | Valor IPI |
| Case 9 | VerificarDiferido() | Flag Diferido |
| Case 10 | CalcularValorPis() | Valor PIS |
| Case 11 | CalcularValorCofins() | Valor COFINS |
| Case 12 | CalcularIva() | IVA |
| Case 13 | CalcularBaseSt() | BC ST |
| Case 14 | CalcularValorSt() | Valor ICMS ST |
| Case 15 | CalcularAliquotaSt() | % ICMS ST |
| Case 16 | CalcularValorIbs() | Valor IBS |
| Case 17 | CalcularValorCbs() | Valor CBS |
| Case 18 | ObterCodigoClassTrib() | Código ClassTrib |
| Case 19 | ObterCstIbsCbs() | CST IBS/CBS |

### ProcessaProdutos → NotaFiscalService.ProcessarProdutoAsync()
```csharp
public async Task<ItemCalculadoDto> ProcessarProdutoAsync(
    int notaFiscalId, 
    int produtoNotaFiscalId)
{
    // 1. Buscar item
    var item = await _repository.GetProdutoAsync(notaFiscalId, produtoNotaFiscalId);
    
    // 2. Calcular todos os impostos
    var impostos = await _calculatorService.CalcularTodosImpostosAsync(item);
    
    // 3. Calcular PIS/COFINS com regra NCM
    var pisCofins = CalcularPisCofins(item, impostos.ValorIcms);
    
    // 4. Atualizar item no banco
    await _repository.AtualizarImpostosAsync(item.Id, impostos, pisCofins);
    
    // 5. Recalcular totais da NF
    await TotalizarAsync(notaFiscalId);
    
    return MapToDto(item);
}
```

---

## ⚡ Otimizações de Performance

### 1. Cache de Dados Fiscais
```csharp
// Usar MemoryCache para:
- Classificação Fiscal (NCM)
- Alíquotas ICMS por UF
- MVA por UF/NCM
- Dados do Cliente (durante sessão da NF)
```

### 2. Batch Updates
```csharp
// Em vez de 20 UPDATEs separados como no VB6,
// fazer um único UPDATE com todos os campos
await _context.ProdutosNotaFiscal
    .Where(p => p.Id == itemId)
    .ExecuteUpdateAsync(p => p
        .SetProperty(x => x.Cst, impostos.Cst)
        .SetProperty(x => x.Cfop, impostos.Cfop)
        .SetProperty(x => x.BaseIcms, impostos.BaseIcms)
        // ... todos os campos
    );
```

### 3. Lazy Loading de Grids
```tsx
// Carregar dados do grid sob demanda
// Não carregar todos os itens de uma vez
```

---

## 🧪 Testes

### Testes Unitários:
- ImpostoCalculatorServiceTests
- NotaFiscalServiceTests
- TotalizadorServiceTests

### Testes de Integração:
- Fluxo completo de criação de NF
- Emissão em homologação
- Cancelamento

### Cenários de Teste Críticos:
1. Produto com redução de BC (Convênio 52/91)
2. Produto com Substituição Tributária
3. Venda para SUFRAMA
4. Venda para não-contribuinte fora do estado
5. Produto importado
6. Venda para produtor rural paulista
7. Item diferido
8. Convênio (usado) com 80% redução

---

## 📝 Checklist de Entrega

### Fase 1 - Backend:
- [ ] DTOs criados
- [ ] ImpostoCalculatorService implementado
- [ ] NotaFiscalService implementado
- [ ] TotalizadorService implementado
- [ ] Testes unitários passando

### Fase 2 - API:
- [ ] Endpoints CRUD funcionando
- [ ] Endpoints de itens funcionando
- [ ] Endpoints de cálculo funcionando
- [ ] Swagger documentado

### Fase 3 - Frontend Base:
- [ ] NotaFiscalListPage funcionando
- [ ] NotaFiscalFormPage estruturada
- [ ] Navegação entre abas funcionando

### Fase 4 - Frontend Grids:
- [ ] ProdutosGrid com cálculo automático
- [ ] ConjuntosGrid com cálculo automático
- [ ] PecasGrid com cálculo automático
- [ ] ServicosGrid funcionando
- [ ] TotalizadoresPanel atualizando em tempo real
- [ ] ParcelasGrid funcionando
- [ ] F2 recalculando impostos

### Fase 5 - NFe:
- [ ] ACBrLibNFe integrado
- [ ] Geração de XML funcionando
- [ ] Emissão em homologação OK
- [ ] DANFE gerando PDF
- [ ] Cancelamento funcionando

---

*Documento criado em: 29/11/2025*
*Estimativa total: 15-20 dias de desenvolvimento*
