# PRD - Análise Comparativa e Plano de Implementação
## Módulo de Movimentação de Estoque

**Versão:** 1.0  
**Data:** 19/12/2025  
**Sistema:** Sistema Irrigação Penápolis - Modernização  
**Autor:** Equipe de Desenvolvimento  

---

# SUMÁRIO EXECUTIVO

Este documento apresenta uma análise comparativa entre o sistema legado (VB6) documentado no `PRD_MODERNIZACAO_ESTOQUE.md` e o sistema atual em desenvolvimento (ASP.NET Core + React). O objetivo é identificar:

1. ✅ **O que já foi implementado**
2. ⚠️ **O que está parcialmente implementado**
3. ❌ **O que ainda precisa ser feito**
4. 💡 **Novas abordagens para problemas identificados no legado**
5. 🆕 **Novas funcionalidades propostas (Entrada via XML)**

---

# PARTE 1: STATUS ATUAL DA IMPLEMENTAÇÃO

---

## 1.1. Movimento Contábil (MVTOCONN.FRM → MovimentoContabilForm.tsx)

### ✅ Funcionalidades Implementadas

| Funcionalidade | Status | Arquivo | Observações |
|----------------|--------|---------|-------------|
| Listagem de movimentos | ✅ Completo | `MovimentoContabilList.tsx` | Paginação, filtros |
| Formulário de movimento | ✅ Completo | `MovimentoContabilForm.tsx` | Interface moderna com abas |
| Tipo de movimento (Entrada/Saída) | ✅ Completo | Frontend + Backend | Toggle visual |
| Seleção de Fornecedor/Cliente | ✅ Completo | `GeralSearch.tsx` | Busca com debounce |
| Inclusão de Produtos | ✅ Completo | Aba "Produtos" | Busca + grid |
| Inclusão de Conjuntos | ✅ Completo | Aba "Conjuntos" | Busca + grid |
| Inclusão de Despesas | ✅ Completo | Aba "Despesas" | Busca + grid |
| Frete e Desconto | ✅ Completo | Aba "Financeiro" | Campos editáveis |
| Geração de Parcelas | ✅ Completo | Aba "Financeiro" | Automático + manual |
| Observações | ✅ Completo | Aba "Observações" | Textarea |
| Checkbox Devolução | ✅ Completo | Cabeçalho | Flag booleana |
| Cálculo de Totais | ✅ Completo | `useEffect` | Produtos + Despesas + Frete - Desconto |
| API de CRUD | ✅ Completo | `MovimentoContabilController.cs` | Create, Read, Delete |

### ⚠️ Funcionalidades Parcialmente Implementadas

| Funcionalidade | Status | Arquivo | Pendência |
|----------------|--------|---------|-----------|
| Produção Inteligente | ⚠️ Modal existe | `ProducaoInteligenteModal.tsx` | Falta integração completa com explosão BOM |
| Validação de Estoque | ⚠️ Básico | Repository | Falta validação retroativa (datas futuras) |
| Custo Médio Ponderado | ⚠️ Básico | Backend | Algoritmo simplificado, não implementa histórico completo |
| Integração Financeira | ⚠️ Parcelas UI | Frontend | Não gera registros em Manutenção Contas automaticamente |

### ❌ Funcionalidades Não Implementadas

| Funcionalidade | Prioridade | Descrição |
|----------------|------------|-----------|
| Baixa de Receita (BOM) | 🔴 Alta | Explosão automática de matérias-primas |
| Produção de Conjuntos | 🔴 Alta | Validação e baixa de componentes |
| Estorno de Movimento | 🟡 Média | Reverter baixas já feitas |
| Validação de Período Contábil | 🟡 Média | Impedir lançamentos fora do período |
| Verificação de Duplicidade NF | 🟡 Média | Mesmo documento/fornecedor |
| Audit Trail Completo | 🟢 Baixa | Log do que mudou, não apenas quem/quando |
| Edição de Movimento Existente | 🟡 Média | Apenas exclusão existe |

---

## 1.2. Gerar Entrada do Estoque (GERESTOQ.FRM)

### ❌ Status: NÃO IMPLEMENTADO

O fluxo de entrada via Pedido de Compra **não existe** no sistema novo. Este é um dos pontos críticos que precisa de decisão arquitetural.

**Problemas do Sistema Legado:**
1. Loop de MsgBox para cada item ("O Item X chegou?")
2. SuperInput3 modal para quantidade de cada item
3. Sem visualização prévia dos itens
4. Impossível desfazer parcialmente
5. Alíquotas de impostos hardcoded (PIS 1.65%, COFINS 7.6%, ICMS 12%)

---

## 1.3. Backend - Estrutura Atual

### Controllers Existentes
```
SistemaEmpresas/Controllers/
├── MovimentoContabil/
│   └── MovimentoContabilController.cs  ✅ Implementado
├── Geral/
│   └── GeralController.cs              ✅ Implementado
├── Produtos/
│   └── ProdutosController.cs           ✅ Implementado
└── [Não existe: PedidoCompraController] ❌
```

### DTOs Existentes
```
SistemaEmpresas/DTOs/MovimentoContabil/
├── MovimentoContabilDto.cs             ✅
├── MovimentoContabilNovoDto.cs         ✅ (Completo com itens e parcelas)
├── ProdutoMvtoContabilItemDto.cs       ✅
├── ConjuntoMvtoContabilItemDto.cs      ✅
├── DespesaMvtoContabilItemDto.cs       ✅
├── ParcelaMvtoContabilDto.cs           ✅
├── ComponenteProducaoDto.cs            ✅
├── VerificacaoProducaoResultDto.cs     ✅
├── ProducaoCascataRequestDto.cs        ✅
└── ProducaoCascataResultDto.cs         ✅
```

### Entidades do Banco (Models)
```
SistemaEmpresas/Models/
├── MovimentoContabilNovo.cs            ✅
├── ProdutoMvtoContabilNovo.cs          ✅
├── ConjuntoMvtoContabilNovo.cs         ✅
├── DespesaMvtoContabilNovo.cs          ✅
├── ParcelaMvtoContabil.cs              ✅
├── BaixaDoEstoqueContabil.cs           ✅
├── MateriaPrima.cs                     ✅ (BOM de Produtos)
├── ItemDoConjunto.cs                   ✅ (Composição de Conjuntos)
├── ProdutoDoPedidoCompra.cs            ✅ (Existe a entidade)
├── BxProdutoPedidoCompra.cs            ✅ (Tabela de baixa parcial)
└── [PedidoCompra não mapeado]          ⚠️
```

---

# PARTE 2: PROBLEMAS IDENTIFICADOS E SOLUÇÕES PROPOSTAS

---

## 2.1. Problemas de Usabilidade - Soluções

| # | Problema do Legado | Solução Implementada/Proposta |
|---|-------------------|-------------------------------|
| 1 | MsgBox repetitivo para cada item | ✅ Grid de conferência único com checkboxes |
| 2 | SuperInput3 modal por item | ✅ Campos editáveis diretamente no grid |
| 3 | Sem visualização prévia | ✅ Listagem completa antes de confirmar |
| 4 | Impossível desfazer parcialmente | 💡 Propor: Histórico de ações com rollback |
| 5 | Labels não traduzidos | ✅ Textos em português no código |

---

## 2.2. Problemas de Regras de Negócio - Soluções

| # | Problema do Legado | Solução Proposta |
|---|-------------------|------------------|
| 1 | **Alíquotas hardcoded** | 💡 Criar tabela `ConfiguracaoImpostos` parametrizável |
| 2 | **Bypass por nome de usuário** | 💡 Substituir por sistema de Permissões por Função |
| 3 | **Grupos de despesa fixos** | 💡 Criar configuração de mapeamento de contas |
| 4 | **Regra de 15 dias** | 💡 Parametrizar período contábil por configuração |
| 5 | **Tratamento específico "Galvanizado"** | 💡 Usar flag na entidade Produto (`TipoIndustrializacao`) |

### Proposta: Tabela de Configuração de Impostos

```csharp
public class ConfiguracaoImposto
{
    public int Id { get; set; }
    public TipoImposto Tipo { get; set; }  // PIS, COFINS, ICMS, IPI
    public decimal Aliquota { get; set; }
    public string? NCM { get; set; }        // Filtro opcional por NCM
    public int? EstadoOrigem { get; set; }
    public int? EstadoDestino { get; set; }
    public DateTime VigenciaInicio { get; set; }
    public DateTime? VigenciaFim { get; set; }
    public bool Ativo { get; set; }
}

// Uso:
var aliquotaPIS = await _impostoService.ObterAliquotaAsync(TipoImposto.PIS, produto.NCM, ufOrigem, ufDestino);
```

### Proposta: Sistema de Permissões por Função

```csharp
public enum PermissaoEstoque
{
    BypassPeriodoContabil,      // Antes: if (usuario == "YGOR")
    PermitirExclusao,           // Antes: if (usuario IN ["YGOR", "JUCELI", "JERONIMO"])
    EditarDocumentoFechado,     // Antes: if (usuario IN ["YGOR", "JUCELI"])
    LancarSemValidacaoEstoque   // Novo: para ajustes de inventário
}

// Uso:
if (!await _permissaoService.UsuarioTemPermissaoAsync(usuario, PermissaoEstoque.BypassPeriodoContabil))
{
    throw new BusinessException("Período contábil fechado para lançamentos.");
}
```

---

## 2.3. Problemas de Performance - Soluções

| # | Problema do Legado | Solução Proposta |
|---|-------------------|------------------|
| 1 | **Loop síncrono com MsgBox** | ✅ Grid assíncrono com React |
| 2 | **Múltiplos SELECTs por item (N+1)** | 💡 Usar Include/ThenInclude no EF Core |
| 3 | **Recálculo de custo médio O(n)** | 💡 Criar tabela `SaldoDiario` com triggers |
| 4 | **CTE recursiva pesada** | 💡 Materializar BOM em tabela auxiliar |
| 5 | **Queries sem índices** | 💡 Adicionar índices compostos |

### Proposta: Tabela de Saldo Diário para Custo Médio

```sql
CREATE TABLE [SaldoDiario] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [SequenciaDoProduto] INT NOT NULL,
    [Data] DATE NOT NULL,
    [QuantidadeAcumulada] DECIMAL(18,4) NOT NULL,
    [CustoMedio] DECIMAL(18,4) NOT NULL,
    [ValorTotalEstoque] DECIMAL(18,2) NOT NULL,
    CONSTRAINT [UK_SaldoDiario] UNIQUE ([SequenciaDoProduto], [Data])
);

-- Trigger para atualizar automaticamente após cada baixa
CREATE TRIGGER [TR_AtualizaSaldoDiario]
ON [Baixa do Estoque Contábil]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Lógica de atualização incremental
END;
```

---

## 2.4. Problemas de Integridade - Soluções

| # | Problema do Legado | Solução Proposta |
|---|-------------------|------------------|
| 1 | **Transações parciais** | 💡 Usar `TransactionScope` envolvendo todo o fluxo |
| 2 | **Concorrência sem locks** | 💡 Implementar Optimistic Locking com `RowVersion` |
| 3 | **Validação retroativa de estoque** | 💡 Verificar saldo em datas futuras antes de permitir |
| 4 | **Auditoria incompleta** | 💡 Usar biblioteca de Audit Trail (ex: Audit.NET) |

### Proposta: Optimistic Locking

```csharp
public class MovimentoContabilNovo
{
    // ... outros campos ...
    
    [Timestamp]
    public byte[] RowVersion { get; set; }
}

// No repository:
try
{
    await _context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
    throw new ConcurrencyException("O registro foi modificado por outro usuário. Recarregue e tente novamente.");
}
```

---

# PARTE 3: ENTRADA DE NOTAS VIA XML DA NFe

---

## 3.1. Visão Geral da Proposta

### 🆕 Nova Funcionalidade: Importação de XML

**Objetivo:** Permitir a entrada de notas fiscais diretamente do arquivo XML da NFe, eliminando digitação manual e reduzindo erros.

**Benefícios:**
- ⚡ Agilidade: Todos os dados preenchidos automaticamente
- 🎯 Precisão: Dados fiscais exatos (chave, valores, impostos)
- 🔗 Rastreabilidade: Vínculo direto com a NFe de origem
- ✅ Validação: Conferência automática de CNPJ, valores

### 3.2. Fluxo Proposto

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        ENTRADA DE NOTAS VIA XML                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. UPLOAD DO XML                                                           │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  [📤 Arrastar XML ou Clicar para Selecionar]                        │   │
│  │                                                                      │   │
│  │  Ou informar Chave de Acesso:                                       │   │
│  │  [____________________________________________] [🔍 Buscar SEFAZ]   │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  2. DADOS EXTRAÍDOS (readonly)                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  Emitente: ACME LTDA - 12.345.678/0001-90                           │   │
│  │  Nº NF: 123456   Série: 1   Emissão: 18/12/2025                     │   │
│  │  Chave: 3524 1212 3456 7800 0190 5500 1000 0012 3456 1234 5678 9012 │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  3. VÍNCULO COM PEDIDO DE COMPRA (opcional)                                │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  Pedido: [______] [🔍]     Status: [Sem vínculo / Vinculado #123]   │   │
│  │                                                                      │   │
│  │  ⚠️ Fornecedor difere do pedido! XML: ACME | Pedido: ACME COM.      │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  4. CONFERÊNCIA DE ITENS                                                   │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  [☑ Selecionar Tudo]  [Vincular com Cadastro]  Filtrar: [_______]  │   │
│  │  ┌────┬──────────┬──────────────────┬───────┬────────┬────────┬───┐ │   │
│  │  │ ☑  │ Cód.XML  │ Descrição XML    │ Qtde  │ Valor  │ Vincul.│ ! │ │   │
│  │  ├────┼──────────┼──────────────────┼───────┼────────┼────────┼───┤ │   │
│  │  │ ☑  │ EXT-001  │ TUBO GALV 2"     │ 100   │ 45,00  │ 001234 │   │ │   │
│  │  │ ☑  │ EXT-002  │ FLANGE 2 POL     │ 50    │ 12,50  │ 001235 │ ⚠ │ │   │
│  │  │ ☐  │ EXT-003  │ PARAF M10X50     │ 500   │ 0,35   │ [____] │ ❌│ │   │
│  │  └────┴──────────┴──────────────────┴───────┴────────┴────────┴───┘ │   │
│  │                                                                      │   │
│  │  Legenda: ⚠ Preço difere do cadastro  ❌ Sem vínculo                │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  5. RESUMO FINANCEIRO                                                       │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  Produtos: R$ 5.450,00    IPI: R$ 272,50    ICMS: R$ 654,00        │   │
│  │  Frete: R$ 350,00         Desconto: R$ 0,00                         │   │
│  │  ───────────────────────────────────────────────────────────────    │   │
│  │  TOTAL DA NOTA: R$ 6.072,50                                         │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│                              [Cancelar]  [Validar]  [✓ Gerar Entrada]      │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 3.3. Estrutura Técnica

### 3.3.1. DTOs para Importação de XML

```csharp
namespace SistemaEmpresas.DTOs.EntradaNota;

/// <summary>
/// Dados extraídos do XML da NFe
/// </summary>
public class NFeImportadaDto
{
    // Identificação
    public string ChaveAcesso { get; set; } = string.Empty;
    public string NumeroNF { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public DateTime DataSaidaEntrada { get; set; }
    public string NaturezaOperacao { get; set; } = string.Empty;
    
    // Emitente
    public string EmitenteCNPJ { get; set; } = string.Empty;
    public string EmitenteRazaoSocial { get; set; } = string.Empty;
    public string EmitenteUF { get; set; } = string.Empty;
    
    // Destinatário
    public string DestinatarioCNPJ { get; set; } = string.Empty;
    public string DestinatarioRazaoSocial { get; set; } = string.Empty;
    
    // Totais
    public decimal TotalProdutos { get; set; }
    public decimal TotalDesconto { get; set; }
    public decimal TotalFrete { get; set; }
    public decimal TotalSeguro { get; set; }
    public decimal TotalOutrasDespesas { get; set; }
    public decimal TotalIPI { get; set; }
    public decimal TotalICMS { get; set; }
    public decimal TotalPIS { get; set; }
    public decimal TotalCOFINS { get; set; }
    public decimal TotalNF { get; set; }
    
    // Itens
    public List<ItemNFeImportadoDto> Itens { get; set; } = new();
    
    // Transporte
    public string? TransportadoraCNPJ { get; set; }
    public string? TransportadoraRazaoSocial { get; set; }
    public string? PlacaVeiculo { get; set; }
    
    // Duplicatas
    public List<DuplicataNFeDto> Duplicatas { get; set; } = new();
}

public class ItemNFeImportadoDto
{
    public int NumeroItem { get; set; }
    public string CodigoProdutoFornecedor { get; set; } = string.Empty;
    public string DescricaoProduto { get; set; } = string.Empty;
    public string NCM { get; set; } = string.Empty;
    public string CFOP { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorDesconto { get; set; }
    
    // Impostos
    public decimal BaseCalculoICMS { get; set; }
    public decimal AliquotaICMS { get; set; }
    public decimal ValorICMS { get; set; }
    public decimal AliquotaIPI { get; set; }
    public decimal ValorIPI { get; set; }
    public decimal AliquotaPIS { get; set; }
    public decimal ValorPIS { get; set; }
    public decimal AliquotaCOFINS { get; set; }
    public decimal ValorCOFINS { get; set; }
    
    // Vínculo com sistema (preenchido pelo usuário)
    public int? SequenciaDoProdutoVinculado { get; set; }
    public string? DescricaoProdutoVinculado { get; set; }
    public bool Selecionado { get; set; } = true;
    
    // Alertas
    public List<string> Alertas { get; set; } = new();
}

public class DuplicataNFeDto
{
    public string Numero { get; set; } = string.Empty;
    public DateTime Vencimento { get; set; }
    public decimal Valor { get; set; }
}
```

### 3.3.2. Serviço de Parsing de XML

```csharp
public interface INFeXmlParserService
{
    /// <summary>
    /// Extrai dados de um arquivo XML da NFe
    /// </summary>
    Task<NFeImportadaDto> ParseXmlAsync(Stream xmlStream);
    
    /// <summary>
    /// Extrai dados de uma string XML
    /// </summary>
    Task<NFeImportadaDto> ParseXmlAsync(string xmlContent);
    
    /// <summary>
    /// Valida a estrutura do XML
    /// </summary>
    Task<ValidationResult> ValidarXmlAsync(Stream xmlStream);
}

public class NFeXmlParserService : INFeXmlParserService
{
    public async Task<NFeImportadaDto> ParseXmlAsync(Stream xmlStream)
    {
        var doc = await XDocument.LoadAsync(xmlStream, LoadOptions.None, CancellationToken.None);
        var ns = doc.Root.GetDefaultNamespace();
        
        var nfe = doc.Descendants(ns + "NFe").FirstOrDefault();
        var infNFe = nfe?.Element(ns + "infNFe");
        var ide = infNFe?.Element(ns + "ide");
        var emit = infNFe?.Element(ns + "emit");
        var dest = infNFe?.Element(ns + "dest");
        var total = infNFe?.Element(ns + "total")?.Element(ns + "ICMSTot");
        var det = infNFe?.Elements(ns + "det");
        
        var dto = new NFeImportadaDto
        {
            ChaveAcesso = infNFe?.Attribute("Id")?.Value?.Replace("NFe", "") ?? "",
            NumeroNF = ide?.Element(ns + "nNF")?.Value ?? "",
            Serie = ide?.Element(ns + "serie")?.Value ?? "",
            DataEmissao = DateTime.Parse(ide?.Element(ns + "dhEmi")?.Value ?? DateTime.Now.ToString()),
            // ... mapear demais campos
        };
        
        // Mapear itens
        foreach (var item in det ?? Enumerable.Empty<XElement>())
        {
            var prod = item.Element(ns + "prod");
            var imposto = item.Element(ns + "imposto");
            
            dto.Itens.Add(new ItemNFeImportadoDto
            {
                NumeroItem = int.Parse(item.Attribute("nItem")?.Value ?? "0"),
                CodigoProdutoFornecedor = prod?.Element(ns + "cProd")?.Value ?? "",
                DescricaoProduto = prod?.Element(ns + "xProd")?.Value ?? "",
                NCM = prod?.Element(ns + "NCM")?.Value ?? "",
                // ... mapear demais campos
            });
        }
        
        return dto;
    }
}
```

### 3.3.3. Endpoint de Importação

```csharp
[ApiController]
[Route("api/entrada-nota")]
[Authorize]
public class EntradaNotaController : ControllerBase
{
    private readonly INFeXmlParserService _xmlParser;
    private readonly IEntradaNotaService _entradaService;
    private readonly IProdutoRepository _produtoRepository;
    
    /// <summary>
    /// Faz upload e parsing do XML da NFe
    /// </summary>
    [HttpPost("importar-xml")]
    public async Task<ActionResult<NFeImportadaDto>> ImportarXml(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest("Arquivo não enviado");
            
        if (!arquivo.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Apenas arquivos XML são aceitos");
        
        using var stream = arquivo.OpenReadStream();
        var nfe = await _xmlParser.ParseXmlAsync(stream);
        
        // Tentar vincular automaticamente com produtos do cadastro
        await VincularProdutosAutomaticamente(nfe);
        
        return Ok(nfe);
    }
    
    /// <summary>
    /// Gera o movimento de entrada a partir do XML importado
    /// </summary>
    [HttpPost("gerar-entrada")]
    public async Task<ActionResult<MovimentoContabilNovoDto>> GerarEntrada([FromBody] GerarEntradaXmlRequest request)
    {
        var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        var resultado = await _entradaService.GerarEntradaAsync(request, usuario);
        
        return CreatedAtAction("ObterMovimento", "MovimentoContabil", 
            new { id = resultado.SequenciaDoMovimento }, resultado);
    }
    
    /// <summary>
    /// Busca produto pelo código do fornecedor ou descrição para vinculação
    /// </summary>
    [HttpGet("buscar-vinculo")]
    public async Task<ActionResult<List<ProdutoVinculoDto>>> BuscarProdutoParaVinculo(
        [FromQuery] string termo, 
        [FromQuery] int? fornecedorId)
    {
        var produtos = await _produtoRepository.BuscarParaVinculoAsync(termo, fornecedorId);
        return Ok(produtos);
    }
    
    private async Task VincularProdutosAutomaticamente(NFeImportadaDto nfe)
    {
        foreach (var item in nfe.Itens)
        {
            // Tentar vincular por código do fornecedor
            var produto = await _produtoRepository.BuscarPorCodigoFornecedorAsync(
                item.CodigoProdutoFornecedor, nfe.EmitenteCNPJ);
            
            if (produto != null)
            {
                item.SequenciaDoProdutoVinculado = produto.SequenciaDoProduto;
                item.DescricaoProdutoVinculado = produto.Descricao;
                
                // Verificar divergência de preço
                if (Math.Abs(produto.ValorCusto - item.ValorUnitario) > 0.01m)
                {
                    item.Alertas.Add($"Preço difere: Cadastro R$ {produto.ValorCusto:N2} | NF R$ {item.ValorUnitario:N2}");
                }
            }
            else
            {
                item.Alertas.Add("Produto não encontrado no cadastro. Vincule manualmente.");
            }
        }
    }
}
```

---

## 3.4. Decisões de Design

### Pergunta 1: O que fazer quando o XML não tem vínculo com Pedido de Compra?

**Opções:**
| Opção | Descrição | Prós | Contras |
|-------|-----------|------|---------|
| A | **Exigir Pedido** | Controle total de compras | Inflexível para pequenas compras |
| B | **Pedido Opcional** | Flexibilidade | Pode perder rastreio |
| C | **Criar Pedido Retroativo** | Mantém histórico | Complexidade extra |

**Recomendação:** Opção B (Pedido Opcional) com alerta visual quando não vinculado.

### Pergunta 2: O que fazer quando o fornecedor do XML não está cadastrado?

**Opções:**
| Opção | Descrição |
|-------|-----------|
| A | Bloquear importação até cadastrar |
| B | Permitir cadastro rápido inline |
| C | Criar cadastro automático com dados do XML |

**Recomendação:** Opção B - Modal de cadastro rápido com campos pré-preenchidos do XML.

### Pergunta 3: Como tratar itens do XML sem vínculo com produtos do cadastro?

**Opções:**
| Opção | Descrição |
|-------|-----------|
| A | Bloquear entrada até vincular todos |
| B | Permitir entrada parcial (apenas vinculados) |
| C | Criar produtos automaticamente |
| D | Permitir entrada sem vínculo (para despesas, por exemplo) |

**Recomendação:** Opção B + D - Permitir escolher quais itens importar e ter tipo "Despesa" para itens sem vínculo com produto.

---

## 3.5. Alternativa: Integração com Pedido de Compra

### Fluxo Híbrido (XML + Pedido)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CONFERÊNCIA COM PEDIDO DE COMPRA                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  XML Importado: NF 123456 - ACME LTDA                                      │
│  Pedido Vinculado: #4521 - ACME COMÉRCIO LTDA                              │
│                                                                             │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │          COMPARAÇÃO XML × PEDIDO                                     │   │
│  │  ┌────┬────────────────┬─────────┬─────────┬─────────┬───────────┐  │   │
│  │  │ ☑  │ Produto        │ Qtd XML │Qtd Ped. │ Pç XML  │ Pç Pedido │  │   │
│  │  ├────┼────────────────┼─────────┼─────────┼─────────┼───────────┤  │   │
│  │  │ ☑  │ Tubo Galv 2"   │   100   │   100   │ 45,00   │   45,00   │  │   │
│  │  │ ⚠️ │ Flange 2"      │    50   │   100   │ 12,50   │   10,00   │  │   │
│  │  │ ❌ │ Parafuso M10   │   500   │     -   │  0,35   │     -     │  │   │
│  │  └────┴────────────────┴─────────┴─────────┴─────────┴───────────┘  │   │
│  │                                                                      │   │
│  │  Legenda:                                                            │   │
│  │  ✅ Confere  ⚠️ Divergência (quantidade ou preço)  ❌ Não no Pedido │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  [Ver Divergências]  [Ignorar Divergências]  [✓ Confirmar Conferência]     │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

# PARTE 4: PLANO DE IMPLEMENTAÇÃO

---

## 4.1. Fases do Projeto

### Fase 1: Consolidar Movimento Contábil (2 semanas)
- [ ] Implementar baixa de receita (explosão BOM)
- [ ] Implementar produção de conjuntos com validação
- [ ] Implementar edição de movimento existente
- [ ] Implementar estorno de movimento
- [ ] Corrigir cálculo de custo médio

### Fase 2: Motor de Impostos (1 semana)
- [ ] Criar tabela `ConfiguracaoImpostos`
- [ ] Criar serviço `ICalculadoraImpostos`
- [ ] Migrar alíquotas hardcoded para banco
- [ ] Criar tela de configuração

### Fase 3: Entrada via XML (3 semanas)
- [ ] Criar serviço de parsing XML (`NFeXmlParserService`)
- [ ] Criar DTOs de importação
- [ ] Criar endpoints de API
- [ ] Criar tela de importação (React)
- [ ] Implementar vinculação automática
- [ ] Implementar conferência e ajustes

### Fase 4: Integração com Pedido de Compra (2 semanas)
- [ ] Criar `PedidoCompraController`
- [ ] Criar tela de conferência híbrida (XML + Pedido)
- [ ] Implementar baixa parcial de pedido
- [ ] Implementar alertas de divergência

### Fase 5: Refinamentos (1 semana)
- [ ] Implementar auditoria completa
- [ ] Otimizar queries
- [ ] Criar tabela de saldo diário
- [ ] Testes de integração

---

## 4.2. Priorização

| Prioridade | Item | Justificativa |
|------------|------|---------------|
| 🔴 P0 | Baixa de Receita (BOM) | Core business - sem isso não baixa estoque corretamente |
| 🔴 P0 | Entrada via XML | Alta demanda operacional |
| 🟡 P1 | Motor de Impostos | Compliance fiscal |
| 🟡 P1 | Integração Pedido Compra | Rastreabilidade |
| 🟢 P2 | Auditoria Completa | Governança |
| 🟢 P2 | Otimização Performance | Melhorias contínuas |

---

# PARTE 5: CONCLUSÃO

---

## 5.1. Resumo do Status

| Módulo | Status | Cobertura |
|--------|--------|-----------|
| Movimento Contábil (UI) | ✅ Implementado | ~80% |
| Movimento Contábil (Regras) | ⚠️ Parcial | ~50% |
| Entrada via Pedido | ❌ Não existe | 0% |
| Entrada via XML | ❌ Não existe | 0% |
| Motor de Impostos | ❌ Não existe | 0% |
| Integração Financeira | ⚠️ Parcial | ~30% |

## 5.2. Decisões Pendentes

1. **Entrada de Notas:** XML puro ou híbrido (XML + Pedido)?
2. **Fornecedor não cadastrado:** Bloquear ou cadastrar inline?
3. **Itens sem vínculo:** Bloquear, ignorar ou criar?
4. **Período contábil:** Parametrizável ou fixo?

## 5.3. Próximos Passos

1. ✅ Validar este PRD com stakeholders
2. 🔄 Definir decisões pendentes
3. 📋 Criar backlog detalhado no board
4. 🚀 Iniciar Fase 1 (Consolidar Movimento Contábil)

---

**Documento elaborado com base em:**
- `PRD_MODERNIZACAO_ESTOQUE.md` (Análise do legado VB6)
- Código-fonte atual do sistema ASP.NET Core + React
- Requisitos levantados com usuários

**Última atualização:** 19/12/2025
