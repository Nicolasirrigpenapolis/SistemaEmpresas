# Mapeamento Aba 1 - Dados Principais

## Visão Geral
A Aba 1 "Dados Principais" contém os campos de cabeçalho da Nota Fiscal, incluindo:
- Identificação da NF
- Cliente/Destinatário
- Transportadora
- Informações de Volumes

---

## 📋 Campos do Cabeçalho (fora da aba)

| Index | DataField | Label | Tipo | Editável | Observação |
|-------|-----------|-------|------|----------|------------|
| 65 | Sequência da Nota Fiscal | (ID) | Numérico | Não | PK - Somente leitura |
| 66 | Sequência do Geral | Cliente | Numérico | Sim | FK para tabela Geral (Cliente) |
| 67 | (Exibição) | Nº NF | Texto | Não | Exibe número da NF em fonte grande |
| 68 | Data de Emissão | Data Emissão | Data | Sim | - |
| 69 | Sequência da Propriedade | Propriedade | Numérico | Sim | FK Propriedades |
| 70 | (Campo auxiliar) | Origem 1 | Texto | Não | Somente leitura |
| 71 | (Campo auxiliar) | Origem 2 | Texto | Não | Somente leitura |
| 72 | (Campo auxiliar) | Contrato | Texto | Não | Somente leitura |
| 75 | (Campo auxiliar) | NF Mãe | Texto | Não | Somente leitura |

### Checkboxes do Cabeçalho:
| Index | DataField | Label |
|-------|-----------|-------|
| 7 | Nota Fiscal Avulsa | N.F. Avulsa (Devolução, Remessa, Entrega Futura...) |
| 8 | Ocultar Valor Unitário | Ocultar Vr. Unitário |
| 9 | Novo Layout | Versão 4.0 |
| 10 | (cEnq Manual) | cEnq Manual |
| 11 | (Layout Antigo) | Layout Antigo |

---

## 📦 Seção: Natureza da Operação e Datas

| Index | DataField | Label | Tipo | Editável |
|-------|-----------|-------|------|----------|
| 14 | Sequência da Natureza | Nat. Ope. | Numérico | Sim |
| 10 | Data de Saída | *Dt. Saída | Data | Sim |
| 11 | Hora da Saída | Hora | Hora | Sim |
| 54 | Alíquota do ISS | % ISS | Decimal | Sim |

### Opções de Tipo de Nota (Radio):
- opcPainel1(0) = Tipo de Nota
- opcPainel2(0) = Fechamento

---

## 🚛 Seção: Transportador / Volumes Transportados

| Index | DataField | Label | Tipo | Editável | MaxLength |
|-------|-----------|-------|------|----------|-----------|
| 13 | Sequência da Transportadora | Transport. | Numérico | Sim | - |
| 4 | Nome da Transportadora Avulsa | (Nome Transp.) | Texto | Sim | 60 |
| 17 | Frete | Frete | Texto | Sim | - |
| 26 | Código da ANTT | ANTT | Texto | Sim | 20 |
| 15 | Placa do Veículo | Placa | Texto | Sim | 8 |
| 16 | UF do Veículo | *UF | Texto | Sim | 3 |
| 12 | Endereço da Transportadora | Endereço | Texto | Sim | 40 |
| 6 | Município da Transportadora | *Município | Texto | Sim | - |
| 7 | IE da Transportadora | I.E. | Texto | Sim | 15 |

### Campos de Exibição (Somente Leitura):
| Index | DataField | Label |
|-------|-----------|-------|
| 2 | (CPF/CNPJ Transportadora) | *CPF/CNPJ |
| 3 | Documento da Transportadora | (Doc.) |
| 5 | (Município Display) | Município |
| 8 | (Endereço Display) | Endereço |
| 9 | (IE Display) | I.E. |

---

## 📦 Seção: Volumes

| Index | DataField | Label | Tipo | Editável | MaxLength |
|-------|-----------|-------|------|----------|-----------|
| 19 | Volume | Vol. | Numérico | Sim | - |
| 20 | Espécie | Esp. | Texto | Sim | 20 |
| 22 | Marca | Marca | Texto | Sim | 20 |
| 25 | Numeração | Numeração | Texto | Sim | 20 |
| 23 | Peso Bruto | P. Bruto | Decimal | Sim | - |
| 24 | Peso Líquido | P. Líquido | Decimal | Sim | - |
| 50 | (Local Embarque) | Embar. | Texto | Sim | - |
| 51 | (País) | País | Texto | Sim | - |
| 56 | Sequência do Vendedor | (Vendedor) | Numérico | Não | - |

---

## 📝 Seção: Informações Complementares

| Index | DataField | Label | Tipo | Editável |
|-------|-----------|-------|------|----------|
| 21 | Histórico | Histórico | Memo | Sim |

---

## 💰 Seção: Valores Financeiros (Aba 1 - lado direito)

| Index | DataField | Label | Tipo | Editável |
|-------|-----------|-------|------|----------|
| 0 | Valor do Imposto de Renda | (IR) | Decimal | Sim |
| 1 | Valor do Seguro | Valor do Seguro | Decimal | Sim |
| 18 | Valor do Fechamento | Fechamento | Decimal | Sim |
| 27 | Valor do Frete | Valor do Frete | Decimal | Sim |
| 61 | Outras Despesas | Outras Despesas | Decimal | Não |

---

## ✅ Checkboxes Adicionais

| Index | DataField | Label |
|-------|-----------|-------|
| 2 | (Transportadora Avulsa flag) | - |
| 3 | Reter ISS | (checkbox) |
| 5 | (Cliente Amazonas sem SUFRAMA) | Cliente do Amazonas sem o código do suframa |
| 6 | Nota de Devolução | NFe Devolução |

---

## 🔄 Campos para NFe Devolução

| Index | DataField | Label | Tipo | MaxLength |
|-------|-----------|-------|------|-----------|
| 59 | FinNFe | Finalidade NFe | Numérico | - |
| 60 | Chave da Devolução | (Chave 1) | Texto | 200 |
| 62 | Chave da Devolução 2 | (Chave 2) | Texto | 200 |
| 63 | Chave da Devolução 3 | (Chave 3) | Texto | 200 |
| 64 | (Aux) | - | Numérico | - |

---

## 🎨 Layout Visual da Aba 1

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ [Logo]  Seq: 65    Dt.Emissão: 68    Cliente: 66         Propriedade: 69   │
│         Nº NF: 67  [  ] NF Avulsa    [  ] Ocultar Vr. Unitário             │
├─────────────────────────────────────────────────────────────────────────────┤
│ Nat.Ope: 14            *Dt.Saída: 10   Hora: 11    %ISS: 54                │
│ Tipo: [opcPainel1]     Fechamento: [opcPainel2]    Valor: 18               │
├─────────────────────────────────────────────────────────────────────────────┤
│ ═══════════════════ Transportador / Volumes Transportados ═══════════════  │
│ Transport.: 13    Nome: 4                                                   │
│ Frete: 17         ANTT: 26        Placa: 15        *UF: 16                 │
│ Endereço: 12                      *Município: 6     I.E.: 7                │
│ *CPF/CNPJ: 2      Doc: 3                                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│ Vol: 19   Esp: 20   Marca: 22   Numeração: 25   P.Bruto: 23   P.Líq: 24   │
│ Embar: 50          País: 51                      Vendedor: 56              │
├─────────────────────────────────────────────────────────────────────────────┤
│ ═══════════════════ Informações Complementares ════════════════════════════│
│ Histórico: 21                                                               │
│ [                                                                         ] │
│ [                                                                         ] │
├─────────────────────────────────────────────────────────────────────────────┤
│ [  ] NFe Devolução: 6    Chave: 60                                         │
│ Chave 2: 62              Chave 3: 63                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                       │  Valor do Seguro: 1                 │
│                                       │  Valor do Frete: 27                 │
│                                       │  Outras Despesas: 61                │
│                                       │  Fechamento: 18                     │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 📊 Resumo de Campos da Aba 1

### Total de Campos de Texto: ~35
### Total de Checkboxes: 8
### Total de Radio Buttons: 4 (2 grupos)

### Campos Obrigatórios (marcados com *):
1. Data de Saída
2. UF do Veículo  
3. Município (Transportadora)
4. CPF/CNPJ (Transportadora)

### Campos com Lookup (FK):
1. Sequência do Geral (Cliente) → Tabela Geral
2. Sequência da Propriedade → Tabela Propriedades
3. Sequência da Natureza → Tabela Natureza de Operação
4. Sequência da Transportadora → Tabela Geral
5. Sequência do Vendedor → Tabela Vendedores
6. Sequência da Cobrança → Tabela Cobranças

---

*Documento gerado em: 29/11/2025*
*Fonte: NOTAFISC.FRM (VB6)*
