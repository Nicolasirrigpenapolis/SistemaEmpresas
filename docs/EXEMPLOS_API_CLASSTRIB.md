# Exemplos de Respostas da API ClassTrib

## 1. AUTOCOMPLETE - Busca por Termo
**Endpoint:** `GET /api/classtrib/autocomplete?q=200&limite=20`

**Descrição:** Busca ClassTribs por código, CST ou descrição. Usado no componente de seleção.

**Resposta (200 OK):**
```json
[
  {
    "id": 6,
    "codigoClassTrib": "200001",
    "cst": "200",
    "descricao": "Aquisições de máquinas, de aparelhos, de instrumentos, de equipamentos, de matérias-primas, de produtos intermediários e de materiais de embalagem realizadas entre empresas autorizadas a operar em zonas de processamento de exportação...",
    "displayText": "200001 - 200 - Aquisições de máquinas, de aparelhos, de instrumentos, de equipamentos, de matérias-primas, de prod..."
  },
  {
    "id": 7,
    "codigoClassTrib": "200002",
    "cst": "200",
    "descricao": "Fornecimento ou importação de tratores, máquinas e implementos agrícolas, destinados a produtor rural, pessoa física ou jurídica, observado o art. 105 da Lei Complementar nº 214, de 2025...",
    "displayText": "200002 - 200 - Fornecimento ou importação de tratores, máquinas e implementos agrícolas, destinados a produtor rura..."
  },
  {
    "id": 8,
    "codigoClassTrib": "200003",
    "cst": "200",
    "descricao": "Vendas de produtos destinados à alimentação humana relacionados no Anexo I da Lei Complementar nº 214, de 2025...",
    "displayText": "200003 - 200 - Vendas de produtos destinados à alimentação humana relacionados no Anexo I da Lei Complementar nº..."
  }
]
```

---

## 2. DETALHES COMPLETOS - Buscar por ID
**Endpoint:** `GET /api/classtrib/{id}`

**Exemplo:** `GET /api/classtrib/6`

**Resposta (200 OK):**
```json
{
  "id": 6,
  "codigoClassTrib": "200001",
  "codigoSituacaoTributaria": "200",
  "descricaoSituacaoTributaria": "Alíquota reduzida",
  "descricaoClassTrib": "Aquisições de máquinas, de aparelhos, de instrumentos, de equipamentos, de matérias-primas, de produtos intermediários e de materiais de embalagem realizadas entre empresas autorizadas a operar em zonas de processamento de exportação, observado o art. 103 da Lei Complementar nº 214, de 2025.",
  "percentualReducaoIBS": 100.00000,
  "percentualReducaoCBS": 100.00000,
  "tipoAliquota": "Padrão",
  "validoParaNFe": true,
  "tributacaoRegular": true,
  "creditoPresumidoOperacoes": false,
  "estornoCredito": false,
  "anexoLegislacao": 5,
  "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
}
```

**Campos:**
- `id`: Identificador único no banco
- `codigoClassTrib`: Código da classificação (ex: "200001")
- `codigoSituacaoTributaria`: CST - Código de Situação Tributária (ex: "200")
- `descricaoSituacaoTributaria`: Descrição do CST
- `descricaoClassTrib`: Descrição completa da classificação
- `percentualReducaoIBS`: Percentual de redução do IBS (0-100)
- `percentualReducaoCBS`: Percentual de redução do CBS (0-100)
- `tipoAliquota`: Tipo da alíquota (Padrão, Fixa, Uniforme Nacional, etc.)
- `validoParaNFe`: Se pode ser usado em Nota Fiscal Eletrônica
- `tributacaoRegular`: Se utiliza tributação regular
- `creditoPresumidoOperacoes`: Se há crédito presumido
- `estornoCredito`: Se há estorno de crédito
- `anexoLegislacao`: Número do anexo da Lei Complementar
- `linkLegislacao`: Link para a legislação

---

## 3. LISTAGEM PAGINADA - Listar Todos
**Endpoint:** `GET /api/classtrib?page=1&pageSize=50&cst=000&somenteNFe=true`

**Parâmetros:**
- `page`: Número da página (padrão: 1)
- `pageSize`: Registros por página (padrão: 50)
- `cst`: Filtrar por CST específico (opcional)
- `descricao`: Filtrar por descrição (opcional)
- `somenteNFe`: Apenas válidos para NFe (opcional)

**Resposta (200 OK):**
```json
{
  "items": [
    {
      "id": 1,
      "codigoClassTrib": "000001",
      "codigoSituacaoTributaria": "000",
      "descricaoSituacaoTributaria": "Tributação integral",
      "descricaoClassTrib": "Regime automotivo - projetos incentivados, observado o art. 312 da Lei Complementar nº 214, de 2025.",
      "percentualReducaoIBS": 0.00000,
      "percentualReducaoCBS": 0.00000,
      "tipoAliquota": "Padrão",
      "validoParaNFe": true,
      "tributacaoRegular": true,
      "creditoPresumidoOperacoes": false,
      "estornoCredito": false,
      "anexoLegislacao": 1,
      "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
    },
    {
      "id": 2,
      "codigoClassTrib": "000002",
      "codigoSituacaoTributaria": "000",
      "descricaoSituacaoTributaria": "Tributação integral",
      "descricaoClassTrib": "Tributação integral com crédito presumido de operações...",
      "percentualReducaoIBS": 0.00000,
      "percentualReducaoCBS": 0.00000,
      "tipoAliquota": "Padrão",
      "validoParaNFe": true,
      "tributacaoRegular": true,
      "creditoPresumidoOperacoes": true,
      "estornoCredito": false,
      "anexoLegislacao": 1,
      "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
    }
  ],
  "totalItems": 1250,
  "pageNumber": 1,
  "pageSize": 50,
  "totalPages": 25
}
```

---

## 4. LISTAR POR CST - Filtrar por Código de Situação Tributária
**Endpoint:** `GET /api/classtrib/cst/200`

**Resposta (200 OK):**
```json
[
  {
    "id": 6,
    "codigoClassTrib": "200001",
    "codigoSituacaoTributaria": "200",
    "descricaoSituacaoTributaria": "Alíquota reduzida",
    "descricaoClassTrib": "Aquisições de máquinas, de aparelhos...",
    "percentualReducaoIBS": 100.00000,
    "percentualReducaoCBS": 100.00000,
    "tipoAliquota": "Padrão",
    "validoParaNFe": true,
    "tributacaoRegular": true,
    "creditoPresumidoOperacoes": false,
    "estornoCredito": false,
    "anexoLegislacao": 5,
    "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
  },
  {
    "id": 7,
    "codigoClassTrib": "200002",
    "codigoSituacaoTributaria": "200",
    "descricaoSituacaoTributaria": "Alíquota reduzida",
    "descricaoClassTrib": "Fornecimento ou importação de tratores, máquinas...",
    "percentualReducaoIBS": 100.00000,
    "percentualReducaoCBS": 100.00000,
    "tipoAliquota": "Padrão",
    "validoParaNFe": true,
    "tributacaoRegular": true,
    "creditoPresumidoOperacoes": false,
    "estornoCredito": false,
    "anexoLegislacao": 5,
    "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
  }
]
```

---

## 5. BUSCAR POR CÓDIGO - Código Específico
**Endpoint:** `GET /api/classtrib/codigo/200001`

**Resposta (200 OK):**
```json
{
  "id": 6,
  "codigoClassTrib": "200001",
  "codigoSituacaoTributaria": "200",
  "descricaoSituacaoTributaria": "Alíquota reduzida",
  "descricaoClassTrib": "Aquisições de máquinas, de aparelhos, de instrumentos, de equipamentos, de matérias-primas, de produtos intermediários e de materiais de embalagem realizadas entre empresas autorizadas a operar em zonas de processamento de exportação, observado o art. 103 da Lei Complementar nº 214, de 2025.",
  "percentualReducaoIBS": 100.00000,
  "percentualReducaoCBS": 100.00000,
  "tipoAliquota": "Padrão",
  "validoParaNFe": true,
  "tributacaoRegular": true,
  "creditoPresumidoOperacoes": false,
  "estornoCredito": false,
  "anexoLegislacao": 5,
  "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
}
```

---

## 6. LISTAR VÁLIDOS PARA NFe
**Endpoint:** `GET /api/classtrib/nfe`

**Resposta (200 OK):**
```json
[
  {
    "id": 1,
    "codigoClassTrib": "000001",
    "codigoSituacaoTributaria": "000",
    "descricaoSituacaoTributaria": "Tributação integral",
    "descricaoClassTrib": "Regime automotivo - projetos incentivados...",
    "percentualReducaoIBS": 0.00000,
    "percentualReducaoCBS": 0.00000,
    "tipoAliquota": "Padrão",
    "validoParaNFe": true,
    "tributacaoRegular": true,
    "creditoPresumidoOperacoes": false,
    "estornoCredito": false,
    "anexoLegislacao": 1,
    "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
  }
]
```

---

## 7. PESQUISA - Busca Geral
**Endpoint:** `GET /api/classtrib/search?q=máquinas&limite=50`

**Resposta (200 OK):**
```json
[
  {
    "id": 6,
    "codigoClassTrib": "200001",
    "codigoSituacaoTributaria": "200",
    "descricaoSituacaoTributaria": "Alíquota reduzida",
    "descricaoClassTrib": "Aquisições de máquinas, de aparelhos, de instrumentos...",
    "percentualReducaoIBS": 100.00000,
    "percentualReducaoCBS": 100.00000,
    "tipoAliquota": "Padrão",
    "validoParaNFe": true,
    "tributacaoRegular": true,
    "creditoPresumidoOperacoes": false,
    "estornoCredito": false,
    "anexoLegislacao": 5,
    "linkLegislacao": "https://www.receita.fazenda.gov.br/..."
  }
]
```

---

## 8. SINCRONIZAÇÃO COM API SVRS
**Endpoint:** `POST /api/classtrib/sync?forcar=false`

**Descrição:** Sincroniza dados com a API da Receita Federal (SVRS)

**Resposta (200 OK):**
```json
{
  "sucesso": true,
  "mensagem": "Sincronização concluída com sucesso",
  "totalApiSvrs": 1250,
  "totalProcessado": 1250,
  "dataHoraInicio": "2025-11-26T12:30:00",
  "dataHoraFim": "2025-11-26T12:35:45",
  "tempoDecorrido": "00:05:45"
}
```

---

## 9. STATUS DE SINCRONIZAÇÃO
**Endpoint:** `GET /api/classtrib/sync/status`

**Resposta (200 OK):**
```json
{
  "dataUltimaSincronizacao": "2025-11-26T12:35:45",
  "totalClassificacoesApiSvrs": 1250,
  "cacheAtivo": true,
  "proximaSincronizacaoRecomendada": "2025-11-27T12:35:45"
}
```

---

## Estrutura de Dados no Banco (Tabela ClassTrib)

```sql
CREATE TABLE [ClassTrib] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [CodigoClassTrib] NVARCHAR(6) NOT NULL UNIQUE,
    [CodigoSituacaoTributaria] NVARCHAR(3) NOT NULL,
    [DescricaoSituacaoTributaria] NVARCHAR(200),
    [DescricaoClassTrib] NVARCHAR(MAX) NOT NULL,
    [PercentualReducaoIBS] DECIMAL(8,5),
    [PercentualReducaoCBS] DECIMAL(8,5),
    [TipoAliquota] NVARCHAR(50),
    [ValidoParaNFe] BIT,
    [TributacaoRegular] BIT,
    [CreditoPresumidoOperacoes] BIT,
    [EstornoCredito] BIT,
    [AnexoLegislacao] INT,
    [LinkLegislacao] NVARCHAR(MAX),
    [Ativo] BIT DEFAULT 1,
    [DataCriacao] DATETIME DEFAULT GETDATE(),
    [DataAtualizacao] DATETIME
);

-- Índices
CREATE INDEX IX_CodigoSituacaoTributaria ON ClassTrib(CodigoSituacaoTributaria);
CREATE INDEX IX_ValidoParaNFe ON ClassTrib(ValidoParaNFe) WHERE ValidoParaNFe = 1;
```

---

## Vincular com Classificação Fiscal

A tabela `ClassificacaoFiscal` tem um campo FK que vincula ao `ClassTrib`:

```sql
ALTER TABLE ClassificacaoFiscal 
ADD ClassTribId INT FOREIGN KEY REFERENCES ClassTrib(Id);
```

**Consulta para visualizar a relação:**
```sql
SELECT 
    cf.Id,
    cf.Ncm,
    cf.DescricaoDoNcm,
    ct.CodigoClassTrib,
    ct.CodigoSituacaoTributaria,
    ct.DescricaoClassTrib,
    ct.PercentualReducaoIBS,
    ct.PercentualReducaoCBS,
    ct.ValidoParaNFe
FROM ClassificacaoFiscal cf
LEFT JOIN ClassTrib ct ON cf.ClassTribId = ct.Id
WHERE cf.ClassTribId IS NOT NULL;
```

