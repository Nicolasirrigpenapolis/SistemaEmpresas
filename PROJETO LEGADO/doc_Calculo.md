# **Documentação de Cálculos Fiscais (Orçamento)**

Este documento centraliza as regras de negócio para os cálculos de ICMS e PIS/COFINS aplicados nos orçamentos.

## **Cálculo 1: Redução de ICMS (Parâmetro Oq \= 2\)**

Detalha como o sistema calcula o percentual de redução (AliqRed) para a base de cálculo do ICMS.

### **Etapa 1: Definição da Regra (Configuração)**

A escolha da regra (Anexo I ou II) é manual e definida na **Classificação Fiscal** (cadastro do NCM).

* **Condição Principal:** O campo \[Redução de Base de Cálculo\] deve estar **marcado**.  
* **Escolha do Anexo:** O usuário deve selecionar qual anexo será usado:  
  * Botão "1" → Salva \[Anexo da Redução\] como **0** (Usa regras do **Anexo I**).  
  * Botão "2" → Salva \[Anexo da Redução\] como **1** (Usa regras do **Anexo II**).

### **Etapa 2: Lógica de Aplicação (Execução)**

Quando o cálculo é executado, o sistema:

1. **Verifica o Anexo:** Lê o campo \[Anexo da Redução\] (0 ou 1\) para decidir qual tabela de percentuais usar.  
2. **Verifica a UF:** A UF do destino (do cliente ou da propriedade) determina qual faixa (Norte/Nordeste vs. Sul/Sudeste) será aplicada.  
3. **Verifica Exceções:** A redução é **cancelada** quando **todas** as condições abaixo ocorrem juntas:  
   * Cliente classificado como **Pessoa Física**;  
   * Destino **fora do estado de SP**;  
   * Sem propriedade rural vinculada (logo, **sem IE**).
4. **Aplica travas adicionais:** Mesmo com redução configurada, há bloqueios específicos:  
   * **Órgão Público dentro de SP** → Redução fica zerada.  
   * **Sucata dentro de SP** → Redução fica zerada.  
   * Itens marcados como **Produto Diferido** para **Produtor Paulista** (produto novo) → Redução ignorada.  
   * Itens com **Convênio** e marcados como **Usados** → Aplicam redução fixa de **80%**.

> **Empresa Produtor:** Se o cliente estiver marcado como *Empresa Produtor*, o sistema replica as mesmas regras de redução dos contribuintes (IE ativa) mesmo que a propriedade não possa ser cadastrada.

### **Etapa 3: Percentuais de Redução Aplicados**

#### **A) PARA CONTRIBUINTES (Com Inscrição Estadual)**

| Anexo | Região/Estado | Base Reduzida (BCRed) | Alíq. ICMS | Redução Aplicada (AliqRed) |
| :---- | :---- | :---- | :---- | :---- |
| **Anexo I** | Norte, Nordeste, Centro-Oeste e ES | 73.43% | 7% | **26.57%** |
| **Anexo I** | Sul, Sudeste (MG, RJ, SP) | 73.33% | 12% | **26.67%** |
| **Anexo II** | Norte, Nordeste, Centro-Oeste e ES | 58.57% | 7% | **41.43%** |
| **Anexo II** | Sul, Sudeste (MG, RJ) \- *Exceto SP* | 58.33% | 12% | **41.67%** |
| **Anexo II** | São Paulo (SP) \- *Especial* | 46.67% | 12% | **53.33%** |

#### **B) PARA NÃO CONTRIBUINTES (Sem Inscrição Estadual)**

| Anexo | Localização | Base Reduzida (BCRed) | Alíq. ICMS | Redução Aplicada (AliqRed) |
| :---- | :---- | :---- | :---- | :---- |
| **Anexo I** | *Todas as Regiões* | *Idêntico aos Contribuintes* |  |  |
| **Anexo II** | **FORA do Estado de SP** |  |  |  |
|  | (Norte, NE, CO, ES) | 58.57% | 7% | **41.43%** |
|  | (Sul, Sudeste) | 58.33% | 12% | **41.67%** |
| **Anexo II** | **DENTRO do Estado de SP** |  |  |  |
|  | Se for **REVENDA** | 0% | 0% | **100.00%** |
|  | Se **NÃO** for Revenda | 46.67% | 12% | **53.33%** |

### **Etapa 4: Resumo de Saídas para o Cálculo**

- **Redução aplicada (AliqRed):** porcentual retornado quando `Oq = 2`.  
- **Alíquota ICMS (AliqICMS):** utilizada nas bases calculadas (`Oq = 3`).  
- **Base Reduzida (BCRed):** percentual utilizado nas rotinas de partilha/ST quando a redução está ativa.

## **Cálculo 2: PIS/COFINS (Regime Monofásico)**

Detalha o cálculo especial de PIS/COFINS para itens de "fabricação própria".

### **Etapa 1: Condições de Ativação (Regime Monofásico)**

O regime monofásico é acionado quando:

1. **Origem de fabricação própria:** o campo \[Material Adquirido de Terceiro\] deve estar **desmarcado**.  
2. **Classificação atendida:**
   * Produtos usam o NCM para habilitar o regime → prefixo **84248**, **73090** ou código completo **87162000**.  
   * Conjuntos e Peças (Tabela = 2 ou 3) entram automaticamente no regime desde que sejam de fabricação própria.

Se qualquer uma dessas condições não for cumprida, o item cai no **Cálculo Normal** (Etapa 3).

### **Etapa 2: Cálculo Monofásico (Se Ativado)**

1. **Base Inicial:** (Quantidade \* Valor Unitário).  
2. **Deduzir ICMS:** Um ICMS Auxiliar é calculado e subtraído da base.  
3. **Aplicar Redução Fixa:** A base (já sem ICMS) é reduzida em **48,1%**.  
4. **Aplicar Alíquotas Especiais:** As alíquotas monofásicas são aplicadas sobre esta base reduzida:  
   * **PIS:** 2,0%  
   * **COFINS:** 9,6%

### **Etapa 3: Cálculo Normal (Tributação Padrão)**

Se o item não se enquadra nas condições da Etapa 1:

1. **Base:** Valor \- ICMS (Sem a redução de 48,1%).  
2. **Alíquotas Padrão:**  
   * **PIS:** 1,65%  
   * **COFINS:** 7,6%

### **Etapa 4: Outras Considerações**

* **Descontos e Frete:** Se houver rateio de descontos ou frete, toda a lógica (Etapa 1, 2 e 3\) é reaplicada sobre os valores atualizados.  
* **Entrega Futura (CFOP 5922/6922):**  
  * O ICMS é zerado.  
  * **Importante:** A redução monofásica de PIS/COFINS (**48,1%**) é **MANTIDA** e aplicada.
* **Suframa:** Itens destinados à SUFRAMA retornam PIS/COFINS zerados.  
* **Ativo Imobilizado:** Produtos marcados como imobilizado (Tipo = 4) também têm PIS/COFINS zerados.

## **Exemplos Práticos (Peças com Regime Monofásico)**

### **Exemplo 1 – Venda normal com Redução Regional**

**Cenário:** Peça de fabricação própria, valor unitário de R$ 1.000,00, cliente contribuinte com propriedade em **MG**, NCM cadastrado no **Anexo II** (Sudeste fora de SP), sem frete ou desconto.

1. **Redução regional (ICMS):**  
   $$\text{BC}_{\text{ICMS}} = 1.000{,}00 \times 0,5833 = 583{,}30$$  
   $$\text{ICMS} = 583{,}30 \times 12\% = 70{,}00$$
2. **Base monofásica:**  
   $$\text{Base inicial} = 1.000{,}00 - 70{,}00 = 930{,}00$$
3. **Redução fixa de 48,1%:**  
   $$\text{Redução} = 930{,}00 \times 48{,}1\% = 447{,}33$$  
   $$\text{Base reduzida} = 930{,}00 - 447{,}33 = 482{,}67$$
4. **Tributos monofásicos:**  
   $$\text{PIS} = 482{,}67 \times 2\% = 9{,}65$$  
   $$\text{COFINS} = 482{,}67 \times 9{,}6\% = 46{,}34$$

**Totais destacados:** CFOP `6101`, ICMS = R$ 70,00, PIS = R$ 9,65, COFINS = R$ 46,34.

### **Exemplo 2 – Entrega Futura (CFOP 6922)**

**Cenário:** Mesma peça de R$ 1.000,00, cliente contribuinte em **SP**, NCM no **Anexo II (SP especial)**, sem frete ou desconto, pedido marcado como **Entrega Futura**.

1. **Redução regional calculada internamente:**  
   $$\text{BC}_{\text{ICMS}} = 1.000{,}00 \times 0,4667 = 466{,}70$$  
   $$\text{ICMS auxiliar} = 466{,}70 \times 12\% = 56{,}00$$
   > O sistema zera os campos de ICMS para entrega futura (`Valor do ICMS = 0`), mas mantém esse valor auxiliar para o PIS/COFINS.
2. **Base monofásica:**  
   $$\text{Base inicial} = 1.000{,}00 - 56{,}00 = 944{,}00$$
3. **Redução fixa de 48,1%:**  
   $$\text{Redução} = 944{,}00 \times 48{,}1\% = 453{,}66$$  
   $$\text{Base reduzida} = 944{,}00 - 453{,}66 = 490{,}34$$
4. **Tributos monofásicos:**  
   $$\text{PIS} = 490{,}34 \times 2\% = 9{,}81$$  
   $$\text{COFINS} = 490{,}34 \times 9{,}6\% = 47{,}07$$

**Totais destacados:** CFOP `6922`, ICMS final = R$ 0,00 (por ser entrega futura), PIS = R$ 9,81, COFINS = R$ 47,07.