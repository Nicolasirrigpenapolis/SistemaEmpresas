# PRD - Modernização do Módulo de Movimentação de Estoque

## Documento de Requisitos do Produto (PRD)
**Versão:** 1.0  
**Data:** 19/12/2025  
**Sistema:** Sistema Irrigação Penápolis  
**Módulos:** MVTOCONN.FRM (Movimento Contábil Novo) e GERESTOQ.FRM (Gerar Entrada do Estoque)

---

# PARTE 1: ANÁLISE DETALHADA DO SISTEMA ATUAL (VB6)

---

## 1. MVTOCONN.FRM - Movimento do Estoque Contábil (Novo)

### 1.1. Informações Gerais
| Atributo | Valor |
|----------|-------|
| **Nome do Módulo** | frmMvtoConN |
| **Função** | Movimento do Estoque Contábil (Novo) |
| **Gerador** | GAS-2007 - Gerador Automático de Sistemas |
| **Tipo de Formulário** | MDI Child |
| **FormID** | 1332 |

### 1.2. Estrutura de Dados Principal

#### 1.2.1. Tabelas Utilizadas
```
- Movimento Contábil Novo (Tabela Principal)
- Produtos Mvto Contábil Novo (Itens de Produtos)
- Conjuntos Mvto Contábil Novo (Itens de Conjuntos/Kits)
- Despesas Mvto Contábil Novo (Itens de Despesas)
- Parcelas Mvto Contabil (Parcelamento Financeiro)
- Baixa do Estoque Contábil (Movimentação Real do Estoque)
- Matéria Prima (Receitas/BOM dos Produtos)
- Itens do Conjunto (Composição dos Kits)
- Manutenção Contas (Integração com Contas a Pagar)
```

#### 1.2.2. Campos do Movimento Principal
| Campo | Tipo | Descrição |
|-------|------|-----------|
| Seqüência do Movimento | Long | Chave primária |
| Data do Movimento | Date | Data da operação |
| Tipo do Movimento | Integer | 0=Entrada, 1=Saída |
| Documento | String | Número do documento/NF (max 20 chars) |
| Seqüência do Geral | Long | FK para Fornecedor/Cliente |
| Observação | String | Campo livre para observações |
| Devolução | Boolean | Indica se é devolução |
| E Produção Própria | Boolean | Indica produção interna |
| Baixa Consumo | Boolean | Indica baixa de consumo |
| Sequencia da Compra | Long | FK para Pedido de Compra |
| Seqüência Grupo Despesa | Integer | FK para grupo de despesa |
| Seqüência SubGrupo Despesa | Integer | FK para subgrupo |
| Forma de Pagamento | String | Vista/Prazo/Antecipado |
| Valor do Frete | Double | Valor do frete |
| Valor do Desconto | Double | Valor do desconto |
| Valor Total dos Produtos | Double | Soma dos produtos |
| Valor Total IPI dos Produtos | Double | Soma do IPI |
| Valor Total das Despesas | Double | Soma das despesas |
| Valor Total do Movimento | Double | Total geral |
| Titulo | String | Tipo de conta (ex: Fornecedores) |
| Codigo do Debito | Long | Código contábil |
| NFe | Double | Número da NFe de referência |
| Fechado | Boolean | Movimento finalizado |
| Seqüência do Orçamento | Long | FK para orçamento relacionado |

### 1.3. Interface do Usuário

#### 1.3.1. Layout Principal
O formulário possui 5 abas (SSTab Control):

**Aba 1 - Dados Principais:**
- Campo de Observação (TextBox multilinha)
- Tipo do Movimento (OptionButton: Entrada/Saída)
- Seqüência do Movimento (readonly)
- Documento (campo editável)
- Fornecedor/Cliente (lookup com pesquisa)
- Checkbox de Devolução

**Aba 2 - Produtos:**
- Grid de Produtos (GListV)
- Campos de inclusão rápida: Produto, Quantidade, ID
- Botões: Incluir, Extornar
- Campos calculados: Valor Unitário, Valor de Custo, Valor Total
- Impostos: PIS, COFINS, IPI, ICMS, Frete, Substituição

**Aba 3 - Conjuntos:**
- Grid de Conjuntos (GListV)
- Campos de inclusão: Conjunto, Quantidade, ID
- Botões: Incluir, Extornar
- Funcionalidade de produção (explosão de materiais)

**Aba 4 - Despesas:**
- Grid de Despesas (GListV)
- Campos similares aos produtos

**Aba 5 - Financeiro:**
- Grupo de Despesa (lookup)
- Sub Grupo de Despesa (lookup)
- Conta/Título (lookup)
- Forma de Pagamento (Vista/Prazo)
- Valor do Frete
- Valor do Desconto
- Totalizadores: IPI, Produtos, Despesas, Total do Movimento
- Grid de Parcelamento
- Código do Débito (lookup)
- Indicadores visuais: Incompleto (vermelho), S/ Parcelas (amarelo), Ok (preto)

### 1.4. Funcionalidades Detalhadas

#### 1.4.1. Tipos de Movimento
| Tipo | Código | Descrição | Efeito no Estoque |
|------|--------|-----------|-------------------|
| Entrada | 0 | Compra, Devolução de Venda, Produção | + Quantidade |
| Saída | 1 | Venda, Consumo, Devolução de Compra | - Quantidade |

#### 1.4.2. Fluxo de Entrada de Produtos
```
1. Usuário seleciona produto via lookup
2. Informa quantidade
3. Sistema calcula automaticamente:
   - Valor Unitário (busca do cadastro ou última compra)
   - Valor de Custo = Unitário - PIS - COFINS - ICMS + IPI + Frete
   - Valor Total = Quantidade × Valor de Custo
4. Ao salvar, executa BaixaReceita():
   - Insere registro na [Baixa do Estoque Contábil]
   - Se produto tem receita (Matéria Prima):
     - Percorre todos os insumos
     - Baixa cada matéria-prima proporcionalmente
   - Atualiza [Quantidade Contábil] do produto
5. Recalcula totalizadores do movimento
```

#### 1.4.3. Fluxo de Produção de Conjuntos
```
1. Usuário seleciona conjunto
2. Informa quantidade a produzir
3. Sistema valida estoque de TODAS as matérias-primas:
   - Consulta [Itens do Conjunto] para obter composição
   - Para cada item, verifica [Quantidade Contábil]
   - Se algum item insuficiente, lista todos e bloqueia
4. Calcula custo do conjunto:
   - Soma (Qtde Usada × Valor Contábil Atual) de cada insumo
5. Executa baixas:
   - Entrada do conjunto produzido
   - Saída de cada matéria-prima utilizada
6. Atualiza estoques contábeis
```

#### 1.4.4. Validação de Estoque (BlasterTemEstoque)
A função `BlasterTemEstoque` é a principal validação de estoque do sistema:

```vb
' Lógica resumida:
1. Se Tipo = Entrada e NÃO é Devolução:
   - Consulta receita do produto (tabela Matéria Prima)
   - Para cada matéria-prima:
     - Verifica se QtdeContabil >= (Quantidade × QtdeUsada)
     - Verifica histórico futuro para não gerar saldo negativo
   - Se qualquer item faltar, exibe lista e bloqueia

2. Se Tipo = Saída:
   - Verifica apenas se QtdeContabil >= Quantidade do item
   - Verifica se não gerará saldo negativo em datas futuras
```

#### 1.4.5. Cálculo de Custo Médio (Função Ultimo)
```vb
' Algoritmo de Custo Médio Ponderado:
1. Ordena movimentos por Data, Tipo, Sequência
2. Para cada movimento:
   - Se Entrada: Custo = (Total + Qtde × VrCusto) / Estoque
   - Se Saída: Total = Total - (Qtde × Custo)
3. Retorna custo médio atual
```

#### 1.4.6. Integração Financeira
O sistema gera automaticamente registros no Contas a Pagar:

```
1. Usuário define parcelas no grid de Parcelamento
2. Ao salvar (LancaParcelas):
   - Para cada parcela:
     - Insere/Atualiza [Manutenção Contas]
     - Define: Fornecedor, Documento, Vencimento, Valor
3. Se movimento vinculado a Pedido de Compra:
   - Gera previsão do saldo restante
   - Monitora se total de parcelas = Total do Movimento
```

### 1.5. Variáveis e Objetos Públicos

#### 1.5.1. Variáveis de Estado
```vb
Public vgSituacao As Integer        ' ACAO_NAVEGANDO, ACAO_INCLUINDO, ACAO_EDITANDO, etc
Public vgCaracteristica As Integer  ' Característica do módulo
Public vgTipo As Integer           ' Tipo do módulo (TP_COMUM)
Public vgPriVez As Integer         ' Flag de primeiro carregamento
Public vgFormID As Long            ' ID único = 1332
Public vgTemInclusao As Integer    ' Permite inclusão?
Public vgTemExclusao As Integer    ' Permite exclusão?
Public vgTemAlteracao As Integer   ' Permite alteração?
```

#### 1.5.2. Recordsets Principais
```vb
Dim Movimento_Contabil_Novo As New GRecordSet
Dim Produtos_Mvto_Contabil_No As New GRecordSet
Dim Conjuntos_Mvto_Contabil_N As New GRecordSet
Dim Despesas_Mvto_Contabil_No As New GRecordSet
Dim Parcelas_mvto_contabil As New GRecordSet
```

#### 1.5.3. Campos/Variáveis de Dados
```vb
Dim Sequencia_do_Movimento As Long
Dim Data_do_Movimento As Variant
Dim Tipo_do_Movimento As Integer
Dim Documento As String
Dim Sequencia_do_Geral As Long
Dim Devolucao As Boolean
Dim E_Producao_Propria As Boolean
Dim Sequencia_da_Compra As Long
Dim Forma_de_Pagamento As String
Dim Valor_do_Frete As Double
Dim Valor_do_Desconto As Double
Dim Valor_Total_dos_Produtos As Double
Dim Valor_Total_do_Movimento As Double
Dim Codigo_do_Debito As Long
Dim NFe As Double
```

### 1.6. Funções Principais

| Função | Propósito |
|--------|-----------|
| `BaixaReceita()` | Executa entrada/saída de produto com explosão de receita |
| `BaixaConjunto()` | Executa entrada/saída de conjunto com baixa de componentes |
| `BaixaDespesa()` | Executa entrada/saída de despesas |
| `BlasterTemEstoque()` | Valida disponibilidade de estoque (produtos) |
| `BlasterTemEstoqueDespesa()` | Valida disponibilidade de estoque (despesas) |
| `BlasterTemEstoqueConj()` | Valida disponibilidade de estoque (conjuntos) |
| `CalculaValorEntrada()` | Calcula custo de entrada baseado na receita |
| `ProcessaProdutos()` | Processa alterações nos itens de produtos |
| `IncluiRegistro()` | Insere registro na [Baixa do Estoque Contábil] |
| `IncluiRegistroC()` | Insere registro para conjuntos |
| `LancaParcelas()` | Gera registros no Contas a Pagar |
| `AjustaValores()` | Recalcula totalizadores do movimento |
| `MegaEstoqueContabil()` | Rotina otimizada de atualização de estoque |
| `ExcluiBaixaReceitaProduto()` | Estorna baixa de produto |
| `ExcluiBaixaReceitaConjunto()` | Estorna baixa de conjunto |
| `Ultimo()` | Calcula custo médio ponderado |
| `ValidaPeriodoContabil()` | Valida se data está em período aberto |
| `VerificaDocumento()` | Valida duplicidade de NF |

---

## 2. GERESTOQ.FRM - Gerar Entrada do Estoque (Pedido)

### 2.1. Informações Gerais
| Atributo | Valor |
|----------|-------|
| **Nome do Módulo** | frmGerEstoq |
| **Função** | Gerar Mvto do Estoque (Pedido) |
| **Gerador** | GAS-2007 |
| **FormID** | 1332 |
| **Tipo** | Formulário de Processo |

### 2.2. Interface do Usuário

#### 2.2.1. Campos da Tela
| Campo | Tipo | Descrição |
|-------|------|-----------|
| Pedido | Lookup + TextBox | Número do Pedido de Compra |
| Nº NFe | TextBox | Número da Nota Fiscal |
| Tipo | ComboBox | MPrima, MConsumo, Despesas, Ativo |
| Dt. Entrada | DatePicker | Data de entrada no estoque |
| Icms do Frete = 0 | CheckBox | Zera ICMS do frete |
| IPI Imbutido na Bc do ICMS | CheckBox | Inclui IPI na base do ICMS |
| Frete | TextBox (moeda) | Valor do frete |
| Tot. Produtos | TextBox (moeda) | Total dos produtos (para rateio) |
| O Tomador do Serviço é a Irrigação Penápolis? | CheckBox | Indica se frete é por conta da empresa |
| Transportadora | Lookup | Fornecedor transportador |
| Nro CTe | TextBox | Número do CTe |
| Botão Gerar | Button | Executa o processo |

### 2.3. Variáveis de Controle
```vb
Dim PedidoTela As Double        ' Número do pedido selecionado
Dim NroNFe As Double           ' Número da NF
Dim Tipo_da_Licitacao As String ' MPrima|MConsumo|Despesas|Ativo
Dim Dt_Entrada As Variant      ' Data de entrada
Dim Vr_do_Frete As Double      ' Valor do frete
Dim Totprod As Double          ' Total dos produtos
Dim Simples As Boolean         ' Fornecedor é Simples Nacional
Dim Ipi_icms As Boolean        ' IPI na base do ICMS
Dim Tomador As Boolean         ' Frete por conta da empresa
Dim Transporte As Double       ' Código da transportadora
Dim Cte As Double              ' Número do CTe
```

### 2.4. Fluxo Principal (EntradaDoCompras)

```
INÍCIO
│
├─ 1. Carrega dados do Pedido de Compra
│   └─ SELECT * FROM [Pedido de Compra Novo] WHERE [Id do Pedido] = PedidoTela
│
├─ 2. Carrega código contábil do fornecedor
│   └─ Se Prazo = "Antecipado": usa [Codigo Adiantamento]
│   └─ Senão: usa [Codigo Contabil]
│
├─ 3. Valida duplicidade de NF
│   └─ Se existe: Msg "Já existe uma Entrada com essa Nota Fiscal" → SAIR
│
├─ 4. Calcula rateio do frete (se houver)
│   ├─ PisFrete = Frete × 1.65%
│   ├─ CofinsFrete = Frete × 7.6%
│   ├─ IcmsFrete = Frete × 12% (se não Simples)
│   ├─ FreteAux = Frete - PIS - COFINS - ICMS
│   └─ AliquotaFrete = FreteAux / TotalProdutos × 100
│
├─ 5. Cria registro do Movimento Contábil
│   └─ INSERT [Movimento Contábil Novo] com dados do pedido
│
├─ 6. LOOP: Para cada item do pedido
│   │
│   ├─ 6.1. MsgBox "O Item X chegou?" → Se NÃO: próximo item
│   │
│   ├─ 6.2. SuperInput3 → Solicita quantidade recebida
│   │   └─ Valida: Qtde <= Qtde Pedida - Qtde já recebida
│   │
│   ├─ 6.3. Calcula valores:
│   │   ├─ Se Tipo ≠ MConsumo: VrCusto -= (Unit × ICMS%)
│   │   ├─ VrCusto -= (Unit × PIS 1.65%)
│   │   ├─ VrCusto -= (Unit × COFINS 7.6%)
│   │   ├─ VrCusto += (Unit × AliquotaFrete%)
│   │   └─ Se MConsumo: VrCusto += (Unit × IPI%)
│   │
│   ├─ 6.4. INSERT [Produtos Mvto Contábil Novo]
│   │
│   ├─ 6.5. Se não é Industrialização e não é Imobilizado:
│   │   └─ INSERT [Baixa do Estoque Contábil]
│   │
│   └─ 6.6. Atualiza cadastro do produto:
│       ├─ ValorCustoContabilNovo()
│       ├─ UltimoFornecedor()
│       ├─ UltimaCompra()
│       └─ ValorTotal()
│
├─ 7. MegaEstoqueContabil() → Atualiza saldos
│
├─ 8. Se Tomador e Frete > 0:
│   └─ LancaFrete() → Cria movimento separado para o frete
│
├─ 9. Abre MVTOCONN.FRM no registro criado
│
└─ FIM
```

### 2.5. Validações (PreValidaPedido)

```vb
' Validações executadas antes de processar:
1. ValidaPeriodoContabil(Dt_Entrada)
   └─ Data não pode ser > 15 dias no passado (exceto YGOR)

2. Pedido tem itens?
   └─ Se não tem Produtos, Despesas nem Consumo → ERRO

3. Total das entradas não pode exceder Total do Pedido
   └─ Se TotalEntradas > TotalPedido → ERRO
   └─ Se TotalEntradas = TotalPedido → ERRO (pedido completo)

4. Fornecedor tem código contábil?
   └─ Se CodigoContabil = 0 → ERRO
   └─ Se Prazo = Antecipado e CodigoAdiantamento = 0 → ERRO
```

### 2.6. Cálculo de Impostos

#### 2.6.1. Fórmula de Custo de Entrada
```
VrCusto = ValorUnitario
        - (ValorUnitario × ICMS%) [se não MConsumo]
        - (ValorUnitario × PIS%)
        - (ValorUnitario × COFINS%)
        + (ValorUnitario × AliquotaFrete%)
        + (ValorUnitario × IPI%) [se MConsumo]
```

#### 2.6.2. Alíquotas Hardcoded
| Imposto | Alíquota | Local no Código |
|---------|----------|-----------------|
| PIS | 1.65% | `PisFrete = (Vr_do_Frete * 1.65) / 100` |
| COFINS | 7.6% | `CofinsFrete = (Vr_do_Frete * 7.6) / 100` |
| ICMS (Frete) | 12% | `IcmsFrete = (Vr_do_Frete * 12) / 100` |

### 2.7. Funcionalidade de Lançamento de Frete Separado

Quando o tomador do serviço é a empresa, um movimento separado é criado:
```vb
Private Sub LancaFrete()
  ' Cria movimento com:
  ' - Grupo Despesa = 45
  ' - SubGrupo Despesa = 358
  ' - Despesa = 3 (Frete)
  ' - Quantidade = 1
  ' - Valor = Vr_do_Frete
End Sub
```

### 2.8. Validação de Integridade do Pedido de Compra

O sistema atual realiza uma série de verificações críticas para garantir que a entrada de estoque esteja em conformidade com o que foi negociado pelo setor de Compras:

| Verificação | Lógica no Código | Objetivo |
|-------------|------------------|----------|
| **Existência de Itens** | `If Itens.RecordCount = 0 And Despesas.RecordCount = 0...` | Impede a entrada de pedidos vazios ou sem itens válidos. |
| **Saldo do Pedido** | `If TotAux >= Tb![Total do Pedido]` | Bloqueia a entrada se o valor total já recebido atingiu ou superou o valor do pedido original. |
| **Limite por Item** | `If QtdEstoqueAux > ProdutosPedido!Qtde` | Impede que o almoxarifado receba uma quantidade maior do que a solicitada no pedido para um item específico. |
| **Configuração Contábil** | `If GeralAux![Codigo Contabil] = 0` | Garante que o fornecedor tenha conta contábil configurada antes de gerar o movimento financeiro. |
| **Adiantamentos** | `If Tb!Prazo = "Antecipado" And CodigoAdiantamento = 0` | Valida se pedidos antecipados possuem a conta de adiantamento correta. |
| **Consistência de Frete** | Cálculo baseado em `CIFFOB` | Ajusta a validação do total do pedido dependendo se o frete deve ou não ser somado ao limite de entrada. |

### 2.9. Lógica de Baixa Parcial (PCOMPRN.FRM)

O sistema permite que um pedido de compra seja recebido parcialmente através de tabelas intermediárias de "Baixa". Esta lógica é disparada por botões no formulário de Pedido de Compra (`PCOMPRN.FRM`).

#### 2.9.1. Tabelas de Controle de Baixa
- `Bx Produtos Pedido Compra`: Itens de produtos para recebimento parcial.
- `Bx Despesas Pedido Compra`: Itens de despesas para recebimento parcial.
- `Bx Consumo Pedido Compra`: Itens de material de consumo para recebimento parcial.

#### 2.9.2. Fluxo de Inicialização da Baixa Parcial (`LancaBxProdutoParcial`)
1. **Validação**: Verifica se o pedido está aberto (`Pedido Fechado = 0`) e se já não existe uma baixa em andamento (tabela de baixa vazia).
2. **Carga de Dados**: Lê todos os itens de `Produtos do Pedido Compra`.
3. **Cálculo de Valor Unitário com IPI**:
   ```vb
   VrUnitarioComIPI = VrUnitario + (VrDoIPI / Qtde)
   ```
4. **Inserção na Tabela de Baixa**:
   - `Qtde Total`: Quantidade original do pedido.
   - `Qtde Recebida`: Inicializada com **0**.
   - `Qtde Restante`: Inicializada com a **Qtde Total**.
   - `Vr Unitario`: Valor unitário calculado com IPI.

#### 2.9.3. Atualização da Baixa (`AjustaBxProdutos`)
Conforme o usuário informa a quantidade recebida no grid, o sistema recalcula os saldos:
- `Qtde Restante = Qtde Total - Qtde Recebida`
- `Total Restante = (Qtde Total * Vr Unitario) - (Qtde Recebida * Vr Unitario)`

#### 2.9.4. Baixa Total (`LancaBxProdutoTotal`)
Atalho que preenche automaticamente:
- `Qtde Recebida = Qtde Total`
- `Qtde Restante = 0`

#### 2.9.5. Integração com a Geração de Entrada (`GERESTOQ.FRM`)
Ao gerar a entrada via `GERESTOQ.FRM`, o sistema prioriza as tabelas de baixa:
- Se houver registros em `Bx Produtos Pedido Compra`, o sistema utiliza a `Qtde Recebida` desta tabela em vez da quantidade original do pedido.
- Isso permite que o almoxarifado confirme exatamente o que está entrando, mantendo o rastreio do que ainda falta receber.

---

# PARTE 2: PROBLEMAS E FALHAS IDENTIFICADOS

---

## 3. Análise Crítica de Falhas

### 3.1. Problemas de Usabilidade

| # | Problema | Localização | Impacto |
|---|----------|-------------|---------|
| 1 | **MsgBox repetitivo para cada item** | `GERESTOQ.FRM:EntradaDoCompras` - `MsgBox "O Item X chegou?"` | Usuário precisa clicar OK/Não centenas de vezes em pedidos grandes |
| 2 | **SuperInput3 modal para quantidade** | `GERESTOQ.FRM:1095` | Mais um popup por item, travando o fluxo |
| 3 | **Sem visualização prévia dos itens** | Ausente | Usuário não vê lista completa antes de iniciar |
| 4 | **Impossível desfazer parcialmente** | `GERESTOQ.FRM` | Se errar no meio do processo, precisa excluir tudo |
| 5 | **Labels não traduzidos** | `LoadGasString()` | Depende de arquivo externo de strings |

### 3.2. Problemas de Arquitetura

| # | Problema | Localização | Impacto |
|---|----------|-------------|---------|
| 1 | **Lógica de negócio no form** | Todo o arquivo `.FRM` | Impossível testar unitariamente |
| 2 | **SQL inline em strings** | Múltiplos locais | Vulnerável a SQL Injection, difícil manutenção |
| 3 | **Recordsets sem Using/Dispose** | `Dim Tb As New GRecordSet` | Memory leaks potenciais |
| 4 | **Variáveis globais excessivas** | Seção `Public` e `Dim` | Estado compartilhado imprevisível |
| 5 | **Código duplicado** | `BlasterTemEstoque` vs `BlasterTemEstoqueDespesa` vs `BlasterTemEstoqueConj` | Mesma lógica em 3 funções |

### 3.3. Problemas de Regras de Negócio

| # | Problema | Código | Impacto |
|---|----------|--------|---------|
| 1 | **Alíquotas hardcoded** | `PIS = 1.65%`, `COFINS = 7.6%` | Não acompanha mudanças na legislação |
| 2 | **Validação por nome de usuário** | `If vgPWUsuario = "YGOR" Then ValidaPeriodoContabil = True` | Bypass de segurança por nome |
| 3 | **Grupos de despesa fixos** | `Grupo = 45, SubGrupo = 358` para frete | Inflexível |
| 4 | **Tratamento de industrialização** | `If Produto!Sub = "Galvanizado"` | Lógica específica hardcoded |
| 5 | **Regra de 15 dias** | `DtaBase = Date - 15` | Período contábil fixo no código |

### 3.4. Problemas de Performance

| # | Problema | Localização | Impacto |
|---|----------|-------------|---------|
| 1 | **Loop síncrono com MsgBox** | `GERESTOQ:EntradaDoCompras` | Trava UI completamente |
| 2 | **Múltiplos SELECTs por item** | Dentro do `Do While` | N+1 queries |
| 3 | **Recálculo de custo médio** | `Ultimo()` percorre TODOS os movimentos | O(n) a cada operação |
| 4 | **CTE recursiva complexa** | `BlasterTemEstoque`, `IncluiConjunto` | Queries pesadas |
| 5 | **Sem índices otimizados** | Queries sem hints | Full table scans |

### 3.5. Problemas de Integridade

| # | Problema | Localização | Impacto |
|---|----------|-------------|---------|
| 1 | **Transações parciais** | `BeginTrans/CommitTrans` não envolve todo o fluxo | Dados inconsistentes se erro no meio |
| 2 | **Concorrência** | Sem locks de registro | Dois usuários podem baixar mesmo item |
| 3 | **Validação de estoque retroativa** | `Data do Movimento` pode ser passada | Pode gerar saldo negativo histórico |
| 4 | **Sem auditoria completa** | Apenas `Usuário/Data/Hora da Alteração` | Não registra o que mudou |

### 3.6. Código Problemático Específico

#### 3.6.1. Bypass de Validação por Usuário
```vb
' GERESTOQ.FRM - Linha ~1434
If vgPWUsuario = "YGOR" Then ValidaPeriodoContabil = True: Exit Function

' MVTOCONN.FRM - Similar
If vgPWUsuario = "GIOVANE" Then
   ' Permite itens que não são de consumo
End If
```

#### 3.6.2. Alíquotas Fixas
```vb
' GERESTOQ.FRM - Linhas 1020-1030
PisFrete = (Vr_do_Frete * 1.65) / 100
CofinsFrete = (Vr_do_Frete * 7.6) / 100
If Not Simples Then
   IcmsFrete = (Vr_do_Frete * 12) / 100
End If
```

#### 3.6.3. MsgBox em Loop
```vb
' GERESTOQ.FRM - Linha ~1072
Do While Not ProdutosPedido.EOF
   If MsgBox("O Item " & Id & " - " & Nome & " chegou?", vbYesNo) = vbYes Then
      ' ... processa
   End If
   ProdutosPedido.MoveNext
Loop
```

---

# PARTE 3: PROPOSTA DE MODERNIZAÇÃO (ASP.NET)

---

## 4. Arquitetura Proposta

### 4.1. Stack Tecnológico
| Camada | Tecnologia |
|--------|------------|
| **Backend** | ASP.NET Core 8.0 Web API |
| **Frontend** | Blazor Server ou React + TypeScript |
| **ORM** | Entity Framework Core 8.0 |
| **Banco** | SQL Server (existente) |
| **Autenticação** | Identity + JWT |
| **Validação** | FluentValidation |
| **Mapeamento** | AutoMapper |
| **Logs** | Serilog |
| **Testes** | xUnit + Moq |

### 4.2. Estrutura de Projetos
```
SistemaIrrigacao.sln
├── src/
│   ├── SistemaIrrigacao.Domain/           # Entidades, Interfaces
│   ├── SistemaIrrigacao.Application/      # Casos de uso, DTOs, Services
│   ├── SistemaIrrigacao.Infrastructure/   # EF, Repositories
│   ├── SistemaIrrigacao.API/              # Controllers, Middlewares
│   └── SistemaIrrigacao.Web/              # Frontend Blazor/React
├── tests/
│   ├── SistemaIrrigacao.UnitTests/
│   └── SistemaIrrigacao.IntegrationTests/
└── docs/
```

### 4.3. Entidades Principais (Domain)

```csharp
public class MovimentoContabil
{
    public long Id { get; set; }
    public DateTime DataMovimento { get; set; }
    public TipoMovimento Tipo { get; set; }  // Enum: Entrada=0, Saida=1
    public string Documento { get; set; }
    public long FornecedorId { get; set; }
    public string Observacao { get; set; }
    public bool Devolucao { get; set; }
    public bool ProducaoPropria { get; set; }
    public long? PedidoCompraId { get; set; }
    public int GrupoDespesaId { get; set; }
    public int SubGrupoDespesaId { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public decimal ValorFrete { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorTotalProdutos { get; set; }
    public decimal ValorTotalIPI { get; set; }
    public decimal ValorTotalDespesas { get; set; }
    public decimal ValorTotalMovimento { get; set; }
    public long? CodigoDebito { get; set; }
    public long? NFeReferencia { get; set; }
    public bool Fechado { get; set; }
    
    // Navegação
    public virtual Geral Fornecedor { get; set; }
    public virtual ICollection<MovimentoProduto> Produtos { get; set; }
    public virtual ICollection<MovimentoConjunto> Conjuntos { get; set; }
    public virtual ICollection<MovimentoDespesa> Despesas { get; set; }
    public virtual ICollection<ParcelaMovimento> Parcelas { get; set; }
}

public class MovimentoProduto
{
    public long Id { get; set; }
    public long MovimentoId { get; set; }
    public long ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorCusto { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal AliquotaPIS { get; set; }
    public decimal AliquotaCOFINS { get; set; }
    public decimal AliquotaICMS { get; set; }
    public decimal AliquotaIPI { get; set; }
    public decimal AliquotaFrete { get; set; }
    public long? UnidadeSpeedId { get; set; }
    
    // Navegação
    public virtual MovimentoContabil Movimento { get; set; }
    public virtual Produto Produto { get; set; }
}
```

### 4.4. Serviços de Aplicação

```csharp
public interface IMovimentoContabilService
{
    Task<MovimentoContabilDto> GetByIdAsync(long id);
    Task<PagedResult<MovimentoContabilDto>> GetAllAsync(MovimentoFiltro filtro);
    Task<MovimentoContabilDto> CreateAsync(CreateMovimentoCommand command);
    Task UpdateAsync(UpdateMovimentoCommand command);
    Task DeleteAsync(long id);
    Task<ResultadoValidacaoEstoque> ValidarEstoqueAsync(long movimentoId);
    Task ProcessarBaixasAsync(long movimentoId);
    Task GerarParcelasAsync(long movimentoId, List<ParcelaDto> parcelas);
}

public interface IEntradaPedidoService
{
    Task<PedidoCompraDto> GetPedidoAsync(long pedidoId);
    Task<List<ItemPedidoDto>> GetItensDisponiveis(long pedidoId);
    Task<MovimentoContabilDto> ProcessarEntrada(ProcessarEntradaCommand command);
    Task<decimal> CalcularRateioFrete(decimal valorFrete, decimal totalProdutos);
    Task<CustoCalculado> CalcularCustoProduto(CalculoCustoRequest request);
}

public interface IEstoqueService
{
    Task<decimal> GetSaldoContabil(long produtoId, DateTime? data = null);
    Task<decimal> GetCustoMedio(long produtoId, DateTime? data = null);
    Task<bool> ValidarDisponibilidade(long produtoId, decimal quantidade, DateTime data);
    Task AtualizarSaldosAsync(long movimentoId);
}
```

### 4.5. Motor de Impostos Configurável

```csharp
public class ConfiguracaoImposto
{
    public int Id { get; set; }
    public TipoImposto Tipo { get; set; }  // PIS, COFINS, ICMS, IPI
    public decimal Aliquota { get; set; }
    public string NCM { get; set; }         // Filtro por NCM (opcional)
    public int? TipoOperacao { get; set; }  // Filtro por operação
    public int? EstadoOrigem { get; set; }
    public int? EstadoDestino { get; set; }
    public DateTime VigenciaInicio { get; set; }
    public DateTime? VigenciaFim { get; set; }
    public bool Ativo { get; set; }
}

public interface ICalculadoraImpostos
{
    Task<ImpostosCalculados> CalcularAsync(CalculoImpostoRequest request);
}
```

---

## 5. PRD - Nova Tela de Entrada de Notas (Substituindo GERESTOQ)

### 5.1. Objetivo
Transformar o processo de entrada de notas fiscais de um fluxo baseado em popups sequenciais para uma interface moderna de conferência em grid.

### 5.2. Requisitos Funcionais

#### RF01 - Seleção de Pedido
- Busca avançada por: Número, Fornecedor, Data, Status
- Exibição de dados do fornecedor selecionado
- Indicador visual de pedidos com entregas pendentes

#### RF02 - Importação de XML da NFe
- Upload de arquivo XML
- Parsing automático dos campos: Número, Série, Data, Fornecedor, Itens
- Validação de CNPJ do emitente vs. fornecedor do pedido
- Alerta para divergências de preço/quantidade

#### RF03 - Dados da Nota
| Campo | Obrigatório | Validação |
|-------|-------------|-----------|
| Número NF | Sim | Único por fornecedor |
| Série | Não | Numérico |
| Data Emissão | Sim | <= Hoje |
| Data Entrada | Sim | Dentro do período contábil |
| Chave NFe | Não | 44 dígitos |

#### RF04 - Grid de Conferência de Itens
| Coluna | Editável | Descrição |
|--------|----------|-----------|
| Receber | Sim (checkbox) | Marca item para entrada |
| Código | Não | Código do produto |
| Descrição | Não | Nome do produto |
| Qtd. Pedida | Não | Quantidade no pedido |
| Qtd. Já Recebida | Não | Soma das entradas anteriores |
| Qtd. Pendente | Não | Pedida - Já Recebida |
| Qtd. Recebida | Sim | Quantidade desta entrada |
| Vr. Unit. Pedido | Não | Preço do pedido |
| Vr. Unit. NF | Sim | Preço da NF |
| Divergência | Não | Indicador visual se preços diferem |
| IPI % | Não | Alíquota de IPI |
| ICMS % | Não | Alíquota de ICMS |

#### RF05 - Cálculo de Frete
- Opção: "Ratear frete entre os itens"
- Métodos de rateio: Por Valor ou Por Peso
- Exibição prévia do valor rateado por item
- Opção para informar transportadora e CTe

#### RF06 - Totalizadores
| Totalizador | Cálculo |
|-------------|---------|
| Total Produtos | Σ(Qtd × Vr. Unit.) |
| Total IPI | Σ(Total Item × IPI%) |
| Total Frete | Σ(Total Item × Frete%) |
| Total Desconto | Valor informado |
| **Total Geral** | Produtos + IPI + Frete - Desconto |

#### RF07 - Validações em Tempo Real
- Estoque de matéria-prima para itens de produção
- Limite de quantidade vs. pedido
- Período contábil
- Duplicidade de NF

#### RF08 - Ações em Lote
| Ação | Descrição |
|------|-----------|
| Receber Tudo | Marca todos os itens pendentes |
| Limpar Seleção | Desmarca todos |
| Copiar Qtd. Pedida | Preenche Qtd. Recebida com Qtd. Pendente |

#### RF09 - Visualização Financeira
- Preview das parcelas que serão geradas
- Opção de ajustar vencimentos antes de confirmar
- Alerta se valores não batem

### 5.3. Requisitos Não Funcionais

| ID | Requisito | Especificação |
|----|-----------|---------------|
| RNF01 | Responsividade | Funcionar em telas >= 1024px e tablets |
| RNF02 | Performance | Carregar pedido com 500 itens em < 3s |
| RNF03 | Segurança | Perfis: Almoxarifado, Compras, Admin |
| RNF04 | Integridade | Toda operação em transação única |
| RNF05 | Auditoria | Log completo de todas as alterações |
| RNF06 | Disponibilidade | 99.5% uptime |

### 5.4. Wireframe da Interface

```
┌─────────────────────────────────────────────────────────────────────────┐
│ ENTRADA DE NOTAS FISCAIS                                    [Usuário] ▼ │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─ DADOS DO PEDIDO ───────────────────────────────────────────────┐   │
│  │ Pedido: [______] [🔍]   Fornecedor: ACME Ltda - 12.345.678/0001 │   │
│  │ Status: Pendente        Total do Pedido: R$ 125.430,00          │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  ┌─ DADOS DA NOTA ────────────────────────────────────────────────┐   │
│  │ Nº NF: [______]  Série: [__]  Data: [__/__/____]               │   │
│  │ Chave: [____________________________________________]  [📤XML] │   │
│  └────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  ┌─ ITENS ────────────────────────────────────────────────────────┐   │
│  │ [☑ Receber Tudo] [☐ Limpar] [📋 Copiar Qtd]    Filtrar: [___] │   │
│  │ ┌────┬────────┬─────────────────┬───────┬────────┬────────┬──┐ │   │
│  │ │ ☑  │ Código │ Descrição       │ Pedido│ Receber│ Preço  │ !│ │   │
│  │ ├────┼────────┼─────────────────┼───────┼────────┼────────┼──┤ │   │
│  │ │ ☑  │ 001234 │ Tubo Galv. 2"   │ 100   │ [100 ] │ 45,00  │  │ │   │
│  │ │ ☑  │ 001235 │ Flange 2"       │ 50    │ [50  ] │ 12,50  │ ⚠│ │   │
│  │ │ ☐  │ 001236 │ Parafuso M10    │ 500   │ [    ] │ 0,35   │  │ │   │
│  │ └────┴────────┴─────────────────┴───────┴────────┴────────┴──┘ │   │
│  │                                      Página 1 de 5  [<] [>]    │   │
│  └────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  ┌─ FRETE ────────────────┐  ┌─ TOTAIS ──────────────────────────┐   │
│  │ ☑ Ratear frete         │  │ Produtos:        R$   4.500,00    │   │
│  │ Valor: [_____1.200,00] │  │ IPI:             R$     450,00    │   │
│  │ Transportadora: [____] │  │ Frete:           R$   1.200,00    │   │
│  │ CTe: [____________]    │  │ Desconto:        R$       0,00    │   │
│  └────────────────────────┘  │ ─────────────────────────────────  │   │
│                              │ **TOTAL:         R$   6.150,00**  │   │
│                              └────────────────────────────────────┘   │
│                                                                         │
│                              [Cancelar]  [Validar]  [✓ Confirmar]      │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## 6. PRD - Nova Tela de Movimento Contábil (Substituindo MVTOCONN)

### 6.1. Melhorias Propostas

#### 6.1.1. Interface Unificada
- Dashboard com visão geral do movimento
- Navegação por abas mantida, mas com carregamento lazy
- Indicadores visuais de status em tempo real

#### 6.1.2. Produção de Conjuntos
- **Visualização de BOM:** Antes de produzir, exibir árvore de componentes
- **Validação prévia:** Mostrar disponibilidade de cada insumo
- **Produção em lote:** Permitir produzir múltiplos conjuntos de uma vez

#### 6.1.3. Estorno Inteligente
- Visualização do impacto antes de estornar
- Opção de estorno parcial
- Recalculo automático de custo médio

### 6.2. API Endpoints Propostos

```
GET    /api/movimentos                    # Lista paginada
GET    /api/movimentos/{id}               # Detalhes
POST   /api/movimentos                    # Criar
PUT    /api/movimentos/{id}               # Atualizar
DELETE /api/movimentos/{id}               # Excluir (soft delete)

POST   /api/movimentos/{id}/produtos      # Adicionar produto
PUT    /api/movimentos/{id}/produtos/{itemId}
DELETE /api/movimentos/{id}/produtos/{itemId}

POST   /api/movimentos/{id}/conjuntos     # Adicionar conjunto
POST   /api/movimentos/{id}/conjuntos/{itemId}/produzir  # Executar produção

POST   /api/movimentos/{id}/validar-estoque
POST   /api/movimentos/{id}/processar-baixas
POST   /api/movimentos/{id}/gerar-financeiro

GET    /api/estoque/saldo/{produtoId}
GET    /api/estoque/custo-medio/{produtoId}
GET    /api/estoque/historico/{produtoId}
```

---

## 7. Plano de Migração

### 7.1. Fases do Projeto

| Fase | Descrição | Duração Estimada |
|------|-----------|------------------|
| 1 | Análise e modelagem do domínio | 2 semanas |
| 2 | Criação da infraestrutura base | 2 semanas |
| 3 | Implementação do motor de impostos | 1 semana |
| 4 | API de Movimentos Contábeis | 3 semanas |
| 5 | API de Entrada de Pedidos | 2 semanas |
| 6 | Frontend: Entrada de Notas | 2 semanas |
| 7 | Frontend: Movimento Contábil | 3 semanas |
| 8 | Integração com sistema legado | 2 semanas |
| 9 | Testes e homologação | 2 semanas |
| 10 | Go-live e suporte | 1 semana |

### 7.2. Estratégia de Coexistência
Durante a transição, o sistema novo e antigo funcionarão em paralelo:
- Dados compartilhados via banco de dados
- Triggers para sincronização de campos críticos
- Gradual migração de usuários

---

# APÊNDICES

---

## Apêndice A: Algoritmo de Custo Médio Ponderado

### A.1. Função `Ultimo()` - Análise Detalhada

A função `Ultimo()` é responsável por calcular o custo médio ponderado do estoque. 

**Assinatura:**
```vb
Private Function Ultimo( _
    Oque As String, _      ' "Qtde" | "Custo" | "Total"
    Produto As Long, _      ' Código do produto
    Optional ExibirAlerta As Boolean) As Currency
```

**Algoritmo:**
```
INÍCIO
│
├─ 1. Verifica se há movimentos para o produto até a data
│   └─ SELECT COUNT(*) FROM [Baixa do Estoque Contábil]
│      WHERE Produto = X AND Data <= DataMovimento
│
├─ 2. Se Oque = "Qtde":
│   └─ RETURN SUM(Qtde * CASE WHEN Tipo=1 THEN -1 ELSE 1 END)
│
├─ 3. Se Oque = "Custo" ou "Total":
│   │
│   ├─ Ordenar movimentos por Data, Tipo, Sequência
│   │
│   ├─ LOOP por cada movimento:
│   │   │
│   │   ├─ Atualiza Estoque = Estoque + (Qtde * sinal)
│   │   │
│   │   ├─ Se ENTRADA (Tipo=0):
│   │   │   ├─ Se primeiro custo: Custo = VrCusto, Total = Qtde × VrCusto
│   │   │   ├─ Se estoque zerou: Reinicia com novo custo
│   │   │   └─ Senão: Custo = (Total + Qtde × VrCusto) / Estoque
│   │   │            Total = Total + Qtde × VrCusto
│   │   │
│   │   └─ Se SAÍDA (Tipo=1):
│   │       └─ Total = Total - (Qtde × Custo)
│   │
│   └─ RETURN Custo ou Total conforme parâmetro
│
└─ FIM
```

### A.2. Problema de Performance

O algoritmo atual percorre TODOS os movimentos do produto desde o início dos tempos. 
Em produtos com alto giro (milhares de movimentos), isso causa:
- Queries pesadas a cada operação
- Tempo de resposta elevado
- Locks de banco estendidos

**Solução Proposta:** Criar tabela de saldos diários com trigger de atualização automática.

---

## Apêndice B: Queries SQL Críticas

### B.1. Validação de Estoque com CTE Recursiva

```sql
-- BlasterTemEstoque: Verifica se haverá saldo negativo em datas futuras
WITH MovimentosFuturos AS (
    SELECT 
        [Data do Movimento],
        Quantidade * CASE WHEN [Tipo do Movimento] = 1 THEN -1 ELSE 1 END AS QtdeMovimento,
        SUM(Quantidade * CASE WHEN [Tipo do Movimento] = 1 THEN -1 ELSE 1 END) 
            OVER (ORDER BY [Data do Movimento], [Seqüência da Baixa]) AS SaldoAcumulado
    FROM [Baixa do Estoque Contábil]
    WHERE [Seqüência do Produto] = @Produto
      AND [Data do Movimento] >= @DataMovimento
)
SELECT MIN(SaldoAcumulado) AS SaldoMinimo
FROM MovimentosFuturos
```

### B.2. Explosão de BOM para Conjuntos

```sql
-- IncluiConjunto: CTE para obter composição completa do conjunto
WITH SuperQtde(MateriaPrima, QtdeUsada, Produto, Descricao) AS (
    -- Nível base: itens diretos do conjunto
    SELECT 
        [Seqüência da Matéria Prima],
        [Quantidade Utilizada],
        [Seqüência do Produto],
        ''
    FROM [Itens do Conjunto]
    WHERE [Seqüência do Conjunto] = @Conjunto
    
    UNION ALL
    
    -- Recursão: sub-componentes
    SELECT 
        mp.[Seqüência da Matéria Prima],
        mp.[Quantidade Utilizada] * sq.QtdeUsada,
        mp.[Seqüência do Produto],
        ''
    FROM [Matéria Prima] mp
    INNER JOIN SuperQtde sq ON sq.MateriaPrima = mp.[Seqüência do Produto]
)
SELECT 
    MateriaPrima,
    SUM(QtdeUsada) AS QtdeTotal,
    Produto
FROM SuperQtde
GROUP BY MateriaPrima, Produto
```

### B.3. Cálculo de Custo de Entrada com Receita

```sql
-- CalculaValorEntrada: Calcula custo baseado na receita do produto
WITH SuperCusto(MateriaPrima, QtdeUsada, Custo) AS (
    SELECT 
        [Seqüência da Matéria Prima],
        [Quantidade Utilizada],
        ISNULL(p.[Valor Contábil Atual], 0)
    FROM [Matéria Prima] mp
    LEFT JOIN Produtos p ON mp.[Seqüência da Matéria Prima] = p.[Seqüência do Produto]
    WHERE mp.[Seqüência do Produto] = @Produto
)
SELECT SUM(QtdeUsada * Custo) AS CustoReceita
FROM SuperCusto
```

---

## Apêndice C: Mapeamento de Pré-Validações

### C.1. Regras de Habilitação de Campos

| Campo | Condição para Habilitar | Código Original |
|-------|-------------------------|-----------------|
| Tipo do Movimento | Apenas em inclusão | `vgSituacao = ACAO_INCLUINDO` |
| Devolução (checkbox) | Apenas se Tipo = Entrada | `Tipo_do_Movimento = 0` |
| Documento | Inclusão OU (YGOR/JUCELI) | `isAdmin = (vgPWUsuario = "YGOR" Or vgPWUsuario = "JUCELI")` |
| Fornecedor | Não vinculado a pedido | `Sequencia_da_Compra = 0 And Documento <> "Produção"` |
| Grupo Despesa | Fornecedor informado | `Sequencia_do_Geral > 0` |
| Produto (campo caixinha) | Documento = "Produção" E Tipo = Entrada | `(Documento = "Produção") And Tipo_do_Movimento = 0` |
| Conjunto (campo caixinha) | Documento = "Produção" E Tipo = Entrada | Idem |
| Consumo (campo caixinha) | Documento = "Consumo" E Tipo = Saída | `(Documento = "Consumo") And Tipo_do_Movimento = 1` |
| Código Débito | Apenas Entrada | `Tipo_do_Movimento = 0` |
| Pedido de Compra | Entrada e não Produção | `Tipo_do_Movimento = 0 And Documento <> "Produção"` |
| Orçamento | Apenas JERONIMO ou YGOR | `vgPWUsuario = "JERONIMO" Or vgPWUsuario = "YGOR"` |

### C.2. Regras de Visibilidade de Campos

| Campo | Condição para Visível |
|-------|----------------------|
| txtProduto(14) | Entrada E Documento = "Produção" |
| Labels 26,27 | Documento = "Consumo" OU "Produção" |
| Botão Devolução(2) | Entrada E checkbox Devolução marcado |
| txtConjunto(17) | Entrada E Documento = "Produção" |
| txtConsumo(20) | Saída E Documento = "Consumo" |
| lblOrçamento(39) | Sequencia_do_Orcamento > 0 |

---

## Apêndice D: Grupos e SubGrupos Hardcoded

### D.1. Mapeamento por Tipo de Licitação (GERESTOQ)

| Tipo Licitação | Grupo Despesa | SubGrupo Despesa |
|----------------|---------------|------------------|
| MPrima | 25 | 140 |
| MRevenda | 25 | 142 |
| Ativo | 29 | 198 |
| MConsumo | 25 | 141 |

### D.2. Frete como Despesa (LancaFrete)

| Parâmetro | Valor |
|-----------|-------|
| Grupo Despesa | 45 |
| SubGrupo Despesa | 358 |
| Despesa (item) | 3 (Frete) |

---

## Apêndice E: Permissões de Usuários Hardcoded

### E.1. Bypass de Validação por Nome

| Usuário | Permissão Especial | Localização |
|---------|-------------------|-------------|
| YGOR | Bypass período contábil | `ValidaPeriodoContabil` |
| YGOR | Inclusão sempre permitida | `AnalisaCondicoes` |
| YGOR | Exclusão sempre permitida | `AnalisaCondicoes` |
| JUCELI | Exclusão permitida | `AnalisaCondicoes` |
| JUCELI | Editar Documento | `ExecutaPreValidacao` |
| JERONIMO | Exclusão permitida | `AnalisaCondicoes` |
| JERONIMO | Campo ID produção | `ExecutaPreValidacao` |
| MAYSA | Exclusão permitida | `AnalisaCondicoes` |
| GIOVANE | Permitir itens não-consumo | Verificação especial |

---

## Apêndice F: Estrutura dos Grids

### F.1. Grid de Produtos (Grid 0)

| Coluna | Campo BD | Formato | Editável | Lookup |
|--------|----------|---------|----------|--------|
| Produto | Seqüência do Produto | - | Não | Produtos |
| ID | Seqüência do Produto Mvto Novo | 999999 | Sim (readonly) | - |
| Nossa Unidade | - | - | Sim (readonly) | - |
| Un.Fornecedor | Sequencia Unidade Speed | @x | Não | Unidades |
| Qtde | Quantidade | 999.999,9999 | Não | - |
| %. PIS | Valor do PIS | 999.999,9999 | Não | - |
| %. Cofins | Valor do Cofins | 999.999,9999 | Não | - |
| %. IPI | Valor do IPI | 9.999.999,9999 | Não | - |
| %. ICMS | Valor do ICMS | 9.999.999,9999 | Não | - |
| %. Frete | Valor do Frete | 9.999.999,9999 | Não | - |
| Vr. Substituição | Valor da Substituição | 9.999.999,9999 | Não | - |
| Vr. Unitário | Valor Unitário | 9.999.999,9999 | Não | - |
| Vr. Pis | - (calculado) | 9.999.999,9999 | Sim (readonly) | - |
| Vr. Cofins | - (calculado) | 9.999.999,9999 | Sim (readonly) | - |
| Vr. ICMS | - (calculado) | 9.999.999,9999 | Sim (readonly) | - |
| Vr. Custo | Valor de Custo | 9.999.999,9999 | Não | - |
| Vr. Total | - (calculado) | 99.999.999,99 | Sim (readonly) | - |

### F.2. Grid de Parcelas (Grid 3)

| Coluna | Campo BD | Formato | Editável |
|--------|----------|---------|----------|
| Pc. | Número da Parcela | 9999 | Sim (readonly) |
| Dias | Dias | 9999 | Não |
| Vencimento | Data de Vencimento | 99/99/9999 | Não |
| Valor | Valor da Parcela | 99.999.999,99 | Não |
| Cobrança | Seqüência da Cobrança | @x | Não (lookup Tipo Cobrança) |

---

## 8. Conclusão

A modernização do módulo de estoque não é apenas uma atualização tecnológica, mas uma oportunidade de:

1. **Eliminar gargalos operacionais** - Substituir os loops de MsgBox por interfaces de conferência em lote
2. **Preparar para novas legislações** - Motor de impostos configurável para IBS/CBS
3. **Garantir integridade** - Transações atômicas e validações em tempo real
4. **Aumentar produtividade** - Importação de XML, cálculos automáticos, dashboards
5. **Facilitar manutenção** - Arquitetura em camadas, código testável, documentação

O investimento no novo sistema terá retorno em redução de erros operacionais, tempo de processamento e custos de manutenção.

---

**Documento elaborado com base na análise completa dos arquivos:**
- `MVTOCONN.FRM` (9.260 linhas)
- `GERESTOQ.FRM` (1.924 linhas)

**Última atualização:** 19/12/2025
