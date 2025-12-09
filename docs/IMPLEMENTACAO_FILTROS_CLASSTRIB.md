# 🎯 Implementação de Filtros Avançados para ClassTrib

## ✅ Resumo do que foi feito

Implementei uma **solução completa de filtros avançados** para facilitar a busca e visualização de ClassTribs (Classificações Tributárias). A solução inclui:

### 1️⃣ **Backend (.NET)** - 3 Novos Endpoints

#### **GET `/api/classtrib/filtros/avancado`**
Filtros avançados com paginação e múltiplos critérios:

**Parâmetros:**
- `page`: Página (padrão: 1)
- `pageSize`: Registros por página (padrão: 50)
- `csts`: CSTs separados por vírgula (ex: "000,200,410")
- `tipoAliquota`: Tipo de alíquota exato (ex: "Padrão", "Fixa")
- `minReducaoIBS`: Redução IBS mínima (0-100)
- `maxReducaoIBS`: Redução IBS máxima (0-100)
- `minReducaoCBS`: Redução CBS mínima (0-100)
- `maxReducaoCBS`: Redução CBS máxima (0-100)
- `validoNFe`: Válido para NFe (true/false)
- `tributacaoRegular`: Tributação regular (true/false)
- `creditoPresumido`: Crédito presumido (true/false)
- `descricao`: Busca por descrição ou código
- `ordenarPor`: Ordenação (codigo, descricao, reducaoibs, reducaocbs)

**Exemplo:**
```
GET /api/classtrib/filtros/avancado?page=1&pageSize=50&csts=000,200&minReducaoIBS=50&validoNFe=true&ordenarPor=reducaoibs
```

#### **GET `/api/classtrib/filtros/tipos-aliquota`**
Lista todos os tipos de alíquota disponíveis no sistema.

**Resposta:**
```json
[
  "Padrão",
  "Fixa",
  "Uniforme Nacional",
  "Uniforme Setorial",
  "Sem Alíquota"
]
```

#### **GET `/api/classtrib/filtros/csts`**
Lista CSTs disponíveis com contagem de classificações.

**Resposta:**
```json
[
  {
    "codigo": "000",
    "descricao": "Tributação integral",
    "total": 45
  },
  {
    "codigo": "200",
    "descricao": "Alíquota reduzida",
    "total": 320
  },
  {
    "codigo": "410",
    "descricao": "Isenção",
    "total": 215
  }
]
```

#### **GET `/api/classtrib/filtros/estatisticas`**
Estatísticas gerais de ClassTrib.

**Resposta:**
```json
{
  "totalClassificacoes": 1250,
  "totalValidoNFe": 1120,
  "mediaReducaoIBS": 35.42,
  "mediaReducaoCBS": 42.18,
  "totalComReducaoIBS": 456,
  "totalComReducaoCBS": 512,
  "classificacoesPorCST": {
    "000": 45,
    "200": 320,
    "410": 215
  },
  "classificacoesPorTipo": {
    "Padrão": 890,
    "Fixa": 180,
    "Uniforme Nacional": 150
  },
  "dataUltimaSincronizacao": "2025-11-26T12:35:45"
}
```

### 2️⃣ **Métodos do Repository**

#### **`GetPagedAdvancedAsync()`**
Implementa a lógica de filtros complexos com:
- Filtro por múltiplos CSTs
- Faixas de redução (min/max)
- Flags booleanos
- Busca por descrição
- Múltiplas opções de ordenação

#### **`GetTiposAliquotaAsync()`**
Retorna lista distinta de tipos de alíquota ativos.

#### **`GetCstsAsync()`**
Retorna CSTs com contagem agregada por código e descrição.

#### **`GetEstatisticasAsync()`**
Calcula estatísticas de distribuição e médias.

### 3️⃣ **Frontend (React/TypeScript)**

#### **Novos Métodos no `classTribService`**

```typescript
// Filtro avançado
async filtroAvancado(
  page: number,
  pageSize: number,
  csts?: string,
  tipoAliquota?: string,
  minReducaoIBS?: number,
  maxReducaoIBS?: number,
  minReducaoCBS?: number,
  maxReducaoCBS?: number,
  validoNFe?: boolean,
  tributacaoRegular?: boolean,
  creditoPresumido?: boolean,
  descricao?: string,
  ordenarPor?: string
): Promise<ClassTribPagedResult>

// Obter opções para dropdowns
async getTiposAliquota(): Promise<string[]>
async getCsts(): Promise<Array<{codigo, descricao, total}>>

// Estatísticas
async getEstatisticas(): Promise<ClassTribEstatisticas>
```

#### **Página de Gestão de ClassTrib**
Nova página em `/pages/ClassTrib/ClassTribManagementPage.tsx` com:

✨ **Recursos:**
- 📊 **Painel de Estatísticas** - Total, válidos para NFe, médias de redução
- 🔍 **Filtros Avançados** - Múltiplos critérios com checkbox de CST múltiplo
- 📋 **Tabela Paginada** - Exibição dos resultados com ordenação
- 💾 **Exportar CSV** - Baixar dados filtrados
- 🔄 **Sincronização** - Botão para sincronizar com API SVRS
- 📈 **Ordenação** - Por código, descrição, redução IBS/CBS

### 4️⃣ **DTOs Adicionais**

#### **`CstOption`**
```csharp
public class CstOption
{
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public int Total { get; set; }
}
```

#### **`ClassTribEstatisticas`**
```csharp
public class ClassTribEstatisticas
{
    public int TotalClassificacoes { get; set; }
    public int TotalValidoNFe { get; set; }
    public Dictionary<string, int> ClassificacoesPorTipo { get; set; }
    public Dictionary<string, int> ClassificacoesPorCST { get; set; }
    public decimal MediaReducaoIBS { get; set; }
    public decimal MediaReducaoCBS { get; set; }
    public int TotalComReducaoIBS { get; set; }
    public int TotalComReducaoCBS { get; set; }
    public DateTime DataUltimaSincronizacao { get; set; }
}
```

## 🚀 Como Usar

### Via Página Web
1. Acesse `/classtrib` no navegador
2. Use os filtros avançados na seção "Filtros"
3. Selecione CSTs múltiplos, defina faixas de redução, etc.
4. Clique em "Limpar Filtros" para resetar
5. Use "Exportar CSV" para baixar os dados

### Via API REST

**Exemplo 1: Buscar isenções (CST 410) com redução 100%**
```bash
GET /api/classtrib/filtros/avancado?page=1&pageSize=50&csts=410&minReducaoIBS=100
```

**Exemplo 2: Buscar alíquotas reduzidas válidas para NFe**
```bash
GET /api/classtrib/filtros/avancado?csts=200&validoNFe=true&ordenarPor=descricao
```

**Exemplo 3: Buscar múltiplos CSTs**
```bash
GET /api/classtrib/filtros/avancado?csts=000,200,410&page=1
```

## 📁 Arquivos Modificados

- ✅ `Controllers/ClassTribController.cs` - 3 novos endpoints + DTOs
- ✅ `Repositories/ClassTribRepository.cs` - Interface + 4 novos métodos
- ✅ `frontend/src/services/classTribService.ts` - 4 novos métodos
- ✅ `frontend/src/pages/ClassTrib/ClassTribManagementPage.tsx` - Nova página
- ✅ `frontend/src/App.tsx` - Rota para nova página

## 🎨 Componentes Frontend

### **Filtros Disponíveis:**
- ✔️ Busca por Descrição/Código
- ✔️ Seleção múltipla de CST
- ✔️ Tipo de alíquota
- ✔️ Faixa de redução IBS (min/max)
- ✔️ Faixa de redução CBS (min/max)
- ✔️ Válido para NFe
- ✔️ Tributação regular
- ✔️ Crédito presumido
- ✔️ Ordenação por múltiplos critérios

## 📊 Melhorias Implementadas

| Funcionalidade | Antes | Depois |
|---|---|---|
| Filtros básicos | Apenas CST, descrição, NFe | Múltiplos critérios + faixas |
| Performance | Busca simples | Query otimizado com Where chains |
| Ordenação | Apenas código | Código, descrição, redução IBS/CBS |
| Interface | Básica | Avançada com painéis de estatísticas |
| Exportação | Não tinha | CSV com filtros aplicados |
| Visibilidade | Apenas 1 CST por vez | Múltiplos CSTs em uma busca |

## 🔧 Próximos Passos Sugeridos

1. Adicionar **filtro por anexo da legislação**
2. Implementar **histórico de sincronizações**
3. Adicionar **comparação de alterações** entre sincronizações
4. Criar **alertas automáticos** para mudanças na legislação
5. Implementar **marcadores/tags** para agrupamento customizado

---

**Status:** ✅ Concluído e pronto para produção
