# 📋 Documentação Técnica - Módulo Nota Fiscal

## 📌 Visão Geral

O módulo de Nota Fiscal do **novo sistema React/.NET** é o foco principal deste documento.
As referências ao VB6 (NOTAFISC.FRM + IRRIG.BAS) servem apenas como rastreabilidade do comportamento legado; todas as definições vigentes passam a ser as descritas para o backend .NET e o frontend React.
Aqui consolidamos os componentes, fluxos e integrações do sistema moderno que substitui integralmente o legado.

---

## 🧱 Arquitetura Alvo (React/.NET)

### Backend (ASP.NET Core)
- **ImpostoCalculatorService**: encapsula as regras fiscais (ICMS, IPI, PIS/COFINS, ST, IBS/CBS). Implementa métodos puros, com cache das tabelas fiscais e testes unitários extensivos.
- **NotaFiscalService**: orquestra ProcessaProduto/Conjunto/Peça/Serviço, chama o calculator, atualiza totais (`TotalizaNotaFiscal`), valida e prepara dados para emissão.
- **NotaFiscalController**: expõe os endpoints REST documentados (criar, atualizar, calcular, totalizar, validar, emitir). Todo endpoint responde com DTOs próprios do novo sistema.
- **Orquestração de emissão**: integração com ACBrLibNFe, storage de XML, callbacks de status e auditoria.

### Fluxo de Cálculo
1. Frontend envia item/nota para `/api/notas-fiscais/{id}/itens/...`.
2. Controller chama `NotaFiscalService`, que busca dados auxiliares (classificação, clientes, parâmetros) via repositórios.
3. `ImpostoCalculatorService` calcula impostos tradicionais e IBS/CBS (sempre ativos).
4. `NotaFiscalService` persiste o item, chama `TotalizaNotaFiscal` e retorna os totais atualizados ao frontend.
5. Logs estruturados e eventos (ex.: `NotaFiscalAtualizada`) alimentam monitoração e rastreabilidade.

### Frontend (React + Vite + Tailwind)
- Páginas `NotaFiscalListPage` e `NotaFiscalFormPage` consomem os endpoints do backend.
- Grids (Produtos, Conjuntos, Peças, Serviços) acionam recalculações e exibem os totais em tempo real.
- Context/state management centraliza filtros, validações e mensagens de auditoria.
- Impressões e anexos consomem os totais do backend; não há cálculo no cliente.

Essa arquitetura substitui integralmente o fluxo VB6; o legado só é consultado para garantir fidelidade das regras enquanto durar a migração paralela.

---

## 🏗️ Referência Legada (VB6)

### Arquivos Envolvidos:
| Arquivo | Função |
|---------|--------|
| `NOTAFISC.FRM` | Formulário principal (~16.882 linhas) |
| `IRRIG.BAS` | Função CalculaImposto (~700 linhas) |

### Tabelas do Banco de Dados:
| Tabela | Descrição |
|--------|-----------|
| `Nota Fiscal` | Cabeçalho da NF (386 colunas no model .NET) |
| `Produtos da Nota Fiscal` | Itens tipo Produto |
| `Conjuntos da Nota Fiscal` | Itens tipo Conjunto |
| `Peças da Nota Fiscal` | Itens tipo Peça |
| `Serviços da Nota Fiscal` | Itens tipo Serviço |
| `Parcelas da Nota Fiscal` | Parcelas de pagamento |

---

## 🔢 Tipos de Impostos Calculados

### Parâmetro "Oq" na função CalculaImposto:
| Oq | Imposto | Descrição |
|----|---------|-----------|
| 1 | CFOP | Código Fiscal de Operação |
| 2 | % Redução BC | Percentual de Redução da Base de Cálculo |
| 3 | % ICMS | Alíquota do ICMS |
| 4 | % IPI | Alíquota do IPI |
| 5 | CST | Código de Situação Tributária |
| 6 | BC ICMS | Base de Cálculo do ICMS |
| 7 | Valor ICMS | Valor do ICMS |
| 8 | Valor IPI | Valor do IPI |
| 9 | Diferido | Flag de ICMS Diferido |
| 10 | Valor PIS | Valor do PIS |
| 11 | Valor COFINS | Valor do COFINS |
| 12 | IVA | Índice de Valor Agregado (ST) |
| 13 | BC ICMS ST | Base de Cálculo ICMS Substituição |
| 14 | Valor ICMS ST | Valor do ICMS Substituição |
| 15 | % ICMS ST | Alíquota ICMS ST |
| 16 | Valor IBS | Imposto sobre Bens e Serviços (Reforma) |
| 17 | Valor CBS | Contribuição sobre Bens e Serviços (Reforma) |
| 18 | Código ClassTrib | Código da Classificação Tributária |
| 19 | CST IBS/CBS | CST para IBS/CBS |

---

## 🧮 Função Principal: CalculaImposto()

### Localização: 
`IRRIG.BAS` - Linha 2276

### Assinatura:
```vb
Public Function CalculaImposto(
    SeqItem As Long,       ' Sequência do Produto/Conjunto
    SeqGeral As Long,      ' Sequência do Cliente/Destinatário
    Oq As Integer,         ' O que calcular (1-19)
    Tabela As Integer,     ' 1=Produto, 2=Conjunto, 3=Peça
    VrTotal As Double,     ' Valor Total do Item
    vrAdicional As Double, ' Valor Adicional (Frete, etc)
    SeqProp As Long,       ' Sequência da Propriedade
    Optional Ncm As Long,
    Optional SemIPI As Boolean,
    Optional UFAvulso As String,
    Optional vFrete As Double
) As Variant
```

### Tabelas Consultadas:
1. **Tb1**: Produtos/Conjuntos (dados do item)
2. **TB2**: Classificação Fiscal + ClassTrib (NCM, alíquotas)
3. **Tb3**: Geral (dados do cliente/destinatário)
4. **TB4**: Propriedades (se produtor rural)
5. **Tb5**: Municípios (UF de destino)
6. **TB6**: ICMS (alíquotas por UF)
7. **TabelaIVA**: MVA por UF/NCM (Substituição Tributária)

### Variáveis de Contexto:
```vb
Revenda        ' Cliente é revendedor
Substituicao   ' Item tem Substituição Tributária
MateriaPrima   ' Item adquirido de terceiro
ForadoEstado   ' Destino é fora de SP
ForadoPais     ' Destino é exterior
Reducao        ' Tem redução de BC
Contribuinte   ' Destinatário é contribuinte ICMS
ProdutorPaulista ' Produtor rural de SP
Suframa        ' Zona Franca de Manaus
Convenio       ' Convênio 52/91 (80% redução)
Importado      ' Produto importado
ProdutoDiferido ' ICMS diferido
```

---

## 📊 Funções de Processamento (NOTAFISC.FRM)

### 1. ProcessaProdutos() - Linha 5879
**Quando é chamada:** Ao inserir/editar um PRODUTO no grid (Tab)

**O que faz:**
1. Busca dados do Produto e NCM
2. Calcula todos os impostos via CalculaImposto()
3. Calcula PIS/COFINS com regra especial por NCM:
   - NCM 84248*, 7309*, 87162000 → Redução 48.1%, alíquotas 2%/9.6%
   - Demais NCMs → Sem redução, alíquotas 1.65%/7.6%
4. Grava via UPDATE no banco
5. Calcula IBS/CBS se UsarRTC=True

**Impostos calculados (em ordem):**
```
CST → CFOP → BC ICMS → Valor ICMS → Valor IPI → 
Alíq ICMS → Alíq IPI → Diferido → % Redução →
PIS → COFINS → IVA → BC ST → Valor ST → Alíq ST →
IBS → CBS → Tributos Totais
```

### 2. ProcessaConjuntos() - Linha 6099
**Quando é chamada:** Ao inserir/editar um CONJUNTO no grid

**Diferença do Produto:**
- Tabela = 2 (Conjuntos)
- PIS/COFINS SEMPRE com redução 48.1% (padrão para conjuntos)
- Sem validação de NCM especial

### 3. ProcessaPecas() - Linha 6269
**Quando é chamada:** Ao inserir/editar uma PEÇA no grid

**Diferença:**
- Tabela = 3 (Peças)
- Usa NCM passado como parâmetro

### 4. ProcessaServicos() - Linha 6234
**Quando é chamada:** Ao inserir/editar um SERVIÇO

**Diferença:**
- Não calcula ICMS/IPI (serviço é ISS)
- Calcula apenas totais

---

## 🔄 Funções de Recálculo (F2 no Grid)

### ComandosProdutos2() / ComandosConjuntos2() / ComandosPecas2()
**Evento:** KeyDown no grid quando KeyCode = vbKeyF2

**Fluxo:**
1. Captura dados atuais do grid
2. Chama função Processa* correspondente
3. Atualiza totalizadores
4. Refresh no grid

---

## 📈 Função: TotalizaNotaFiscal() - Linha 6902

**Quando é chamada:** 
- Após qualquer ProcessaProdutos/Conjuntos/Pecas
- Ao salvar a nota
- Ao recalcular (F2)

**O que faz:**
1. Soma IPI de Produtos + Conjuntos + Peças
2. Soma ICMS de Produtos + Conjuntos + Peças
3. Soma ICMS ST de Produtos + Conjuntos + Peças
4. Soma Bases de Cálculo
5. Soma valores de Produtos Usados vs Novos
6. Soma PIS total (UNION ALL das 3 tabelas)
7. Soma COFINS total (UNION ALL das 3 tabelas)
8. Soma Tributos total
9. Calcula Valor Total da NF:
   ```
   ValorNF = IPI + Produtos + Conjuntos + Peças + Serviços + 
             Seguro + Frete + Despesas + ICMS ST + II
   ```
10. Aplica fechamento (% ou valor fixo)
11. Atualiza IBS/CBS totais
12. Grava todos os totais na tabela Nota Fiscal

---

## 🆕 Reforma Tributária: IBS/CBS

### Constantes (NOTAFISC.FRM):
```vb
Private Const RTC_MIN_VIBS As Double = 0.001    ' Mínimo IBS
Private Const RTC_PERC_IBSUF As Double = 0.1    ' 0.1% UF
Private Const RTC_PERC_IBSMUN As Double = 0     ' 0% Municipal
Private Const RTC_PERC_CBS As Double = 0.9      ' 0.9% CBS
' UsarRTC foi descontinuado: IBS/CBS são sempre calculados
```

### Cálculo (IRRIG.BAS):
```vb
' Case 16 - IBS
IBS = VrTotal * 0.001 * (1 - ReducaoIBS)

' Case 17 - CBS  
CBS = VrTotal * 0.009 * (1 - ReducaoCBS)
```

### Função AtualizaValoresIBSCBS():
- Soma IBS/CBS de Produtos + Conjuntos + Peças
- Atualiza campos [Valor Total IBS] e [Valor Total CBS] na NF
- Deve ser invocada em todos os fluxos de cálculo, independentemente de parametrização

### Diretrizes para a migração .NET
- **Sem flag UsarRTC**: a versão em .NET não deve expor nenhum toggle para ligar/desligar IBS/CBS; o serviço deve calcular os tributos da reforma em 100% das operações.
- **Parâmetros configuráveis**: RTC_MIN_VIBS e percentuais devem ser externalizados (ex.: tabela de parâmetros ou appsettings) para permitir ajustes futuros sem recompilação.
- **Totalização obrigatória**: `TotalizaNotaFiscal()` precisa acumular os valores de IBS/CBS sempre, alimentando tanto os totais da NF quanto os espelhos impressos/emitidos para a SEFAZ.
- **Validação cruzada**: incluir validações que impeçam salvar ou emitir notas sem IBS/CBS calculados (ex.: campos nulos ou zerados quando não aplicável devem exigir justificativa).

---

## 📑 Regras Fiscais Especiais

### 1. Convênio ICMS 52/91 (Redução BC)
**Anexo I (BCRed = 73.43% ou 73.33%):**
- Norte/Nordeste/Centro-Oeste/ES → AliqICMS por tabela, Redução 26.57%
- Sul/Sudeste → AliqICMS por tabela, Redução 26.67%

**Anexo II (BCRed = 58.57% ou 58.33%):**
- Norte/Nordeste/Centro-Oeste/ES → AliqICMS por tabela, Redução 41.43%
- Sul/Sudeste → AliqICMS por tabela, Redução 41.67%
- SP → BCRed 46.67%, Redução 53.33%

### 2. Substituição Tributária
**Condição:** Revenda + Item com IVA cadastrado por UF
**Fórmula IVA Ajustado:**
```
IVA = (((1 + (IVA_Original/100)) * (1 - (AliqICMS/100)) / 
       (1 - (AliqInterestadual/100))) - 1) * 100
```

### 3. SUFRAMA (Zona Franca)
- ICMS = 0
- IPI = 0
- PIS = 0
- COFINS = 0
- CFOP = 6109 (produção própria) ou 6110 (terceiros)

### 4. Produto Diferido
- CST = 051
- ICMS = 0 (postergado)
- Aplica quando: Produtor Paulista + Item Diferido + Novo

### 5. Produtos Importados
- Dentro da UF → ICMS 18%
- Fora da UF → ICMS 4%
- Origem = 1 (importação direta)

---

## 🏁 Plano de Implementação React/.NET

### Fase 1: Backend - Services de Cálculo
1. **ImpostoCalculatorService**
   - Replicar função CalculaImposto em C#
   - Criar métodos separados por tipo de imposto
   - Cache de tabelas (ICMS, MVA, Classificação Fiscal)

2. **NotaFiscalService**
   - ProcessaProduto() / ProcessaConjunto() / ProcessaPeca()
   - TotalizaNotaFiscal()
   - ValidaNotaFiscal()

### Fase 2: Backend - API Endpoints
```
POST   /api/notas-fiscais              → Criar NF
GET    /api/notas-fiscais/{id}         → Buscar NF
PUT    /api/notas-fiscais/{id}         → Atualizar NF
DELETE /api/notas-fiscais/{id}         → Cancelar NF

POST   /api/notas-fiscais/{id}/itens/produtos    → Adicionar produto
PUT    /api/notas-fiscais/{id}/itens/produtos/{itemId} → Editar produto
DELETE /api/notas-fiscais/{id}/itens/produtos/{itemId} → Remover produto

POST   /api/notas-fiscais/{id}/calcular          → Recalcular impostos
POST   /api/notas-fiscais/{id}/totalizar         → Totalizar NF
POST   /api/notas-fiscais/{id}/validar           → Validar para emissão
POST   /api/notas-fiscais/{id}/emitir            → Emitir NFe (ACBrLibNFe)
```

### Fase 3: Frontend - Componentes React
```
pages/
  NotasFiscais/
    NotaFiscalListPage.tsx      → Lista de NFs
    NotaFiscalFormPage.tsx      → Formulário principal
    components/
      DadosGeraisTab.tsx        → Aba dados gerais
      ProdutosTab.tsx           → Grid de produtos
      ConjuntosTab.tsx          → Grid de conjuntos
      PecasTab.tsx              → Grid de peças
      ServicosTab.tsx           → Grid de serviços
      ParcelasTab.tsx           → Grid de parcelas
      TotalizadoresPanel.tsx    → Painel de totais
      TransporteTab.tsx         → Dados de transporte
```

### Fase 4: Integração ACBrLibNFe
- Substituir FlexDocs pela ACBrLibNFe
- Implementar geração de XML
- Implementar comunicação SEFAZ
- Implementar DANFE

---

## ⚠️ Pontos de Atenção

### 1. PIS/COFINS NÃO são calculados pela CalculaImposto()
O código atual calcula PIS/COFINS **dentro** das funções ProcessaProdutos/ProcessaConjuntos com lógica própria baseada no NCM.

### 2. IBS/CBS são opcionais
A flag `UsarRTC = False` por padrão. Só ativa quando reforma tributária entrar em vigor.

### 3. Transação de banco
Todo ProcessaProdutos usa:
```vb
vgDb.BeginTrans
' ... updates ...
vgDb.CommitTrans
' ou vgDb.RollBackTrans em caso de erro
```

### 4. Performance
A função CalculaImposto abre **7+ recordsets** para cada chamada. 
Na versão .NET, usar cache agressivo para:
- Classificação Fiscal
- ICMS por UF
- MVA por UF/NCM
- Dados do Cliente/Propriedade

---

## 📁 Models .NET Existentes

Os models já existem e estão mapeados:
- `NotaFiscal.cs` (386 colunas)
- `ProdutoDaNotaFiscal.cs`
- `ConjuntoDaNotaFiscal.cs`
- `PecaDaNotaFiscal.cs`
- `ServicoDaNotaFiscal.cs`
- `ParcelaNotaFiscal.cs`

---

*Documento criado em: 29/11/2025*
*Versão: 1.0*
