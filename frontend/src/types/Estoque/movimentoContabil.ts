// DTOs de Inventário/Ajuste de Estoque

/**
 * Informações de estoque de um produto
 */
export interface EstoqueInfoDto {
  sequenciaDoProduto: number;
  descricao: string;
  codigoDeBarras?: string;
  localizacao?: string;
  siglaUnidade: string;
  /** Estoque contábil atual (calculado da tabela Baixa do Estoque Contábil) */
  estoqueContabil: number;
  /** Valor de custo unitário atual do produto */
  valorCusto: number;
  /** Tipo do produto (0=Acabado, 1=M.Prima, 2=M.Revenda, 3=M.Consumo, 4=M.Imobilizado) */
  tipoDoProduto: number;
  tipoDoProdutoDescricao: string;
}

/**
 * DTO para realizar ajuste de inventário
 */
export interface AjusteMovimentoContabilDto {
  sequenciaDoProduto: number;
  ehConjunto?: boolean;
  quantidadeFisica: number;
  sequenciaDoGeral?: number;
  observacao?: string;
  valorCusto?: number;
}

/**
 * Resultado do ajuste de inventário
 */
export interface AjusteMovimentoContabilResultDto {
  sucesso: boolean;
  mensagem: string;
  sequenciaDaBaixa?: number;
  tipoMovimento?: number;
  tipoMovimentoDescricao: string;
  quantidadeAjustada: number;
  diferenca: number;
  estoqueAnterior: number;
  estoqueNovo: number;
  documento?: string;
}

/**
 * Movimento de estoque
 */
export interface MovimentoEstoqueDto {
  sequenciaDaBaixa: number;
  dataMovimento?: string;
  documento: string;
  tipoMovimento: number;
  tipoMovimentoDescricao: string;
  quantidade: number;
  valorUnitario: number;
  valorCusto: number;
  valorTotal: number;
  observacao?: string;
  razaoSocialGeral?: string;
  saldoAposMovimento: number;
}

/**
 * Filtro para buscar movimentos de estoque
 */
export interface MovimentoEstoqueFiltroDto {
  sequenciaDoProduto: number;
  ehConjunto?: boolean;
  dataInicial?: string;
  dataFinal?: string;
  tipoMovimento?: number;
  documento?: string;
  pageNumber?: number;
  pageSize?: number;
}

/**
 * Item para ajuste em lote
 */
export interface AjusteMovimentoContabilItemDto {
  sequenciaDoProduto: number;
  quantidadeFisica: number;
  observacao?: string;
}

/**
 * DTO para ajuste em lote
 */
export interface AjusteMovimentoContabilLoteDto {
  itens: AjusteMovimentoContabilItemDto[];
  observacaoGeral?: string;
}

/**
 * Resultado do ajuste em lote
 */
export interface AjusteMovimentoContabilLoteResultDto {
  sucesso: boolean;
  mensagem: string;
  totalProcessado: number;
  totalSucesso: number;
  totalErro: number;
  detalhes: {
    sequenciaDoProduto: number;
    sucesso: boolean;
    mensagem: string;
  }[];
}

// Novos tipos para MovimentoContabilNovo (MVTOCONN.FRM)

export interface MovimentoContabilNovoDto {
  sequenciaDoMovimento: number;
  dataDoMovimento: string;
  tipoDoMovimento: number; // 0 = Entrada, 1 = Saída
  documento: string;
  sequenciaDoGeral: number;
  razaoSocialGeral?: string;
  observacao?: string;
  devolucao: boolean;
  produtos: ProdutoMvtoContabilItemDto[];
  conjuntos: ConjuntoMvtoContabilItemDto[];
  despesas: DespesaMvtoContabilItemDto[];
  parcelas: ParcelaMvtoContabilDto[];
  valorDoFrete: number;
  valorDoDesconto: number;
  valorTotalDosProdutos: number;
  valorTotalDoMovimento: number;
}

export interface ProdutoMvtoContabilItemDto {
  sequenciaDoProdutoMvtoNovo: number;
  sequenciaDoProduto: number;
  descricaoProduto?: string;
  quantidade: number;
  valorUnitario: number;
  valorDeCusto: number;
  valorTotal: number;
  valorDoPis: number;
  valorDoCofins: number;
  valorDoIpi: number;
  valorDoIcms: number;
  valorDoFrete: number;
  valorDaSubstituicao: number;
}

export interface ConjuntoMvtoContabilItemDto {
  sequenciaConjuntoMvtoNovo: number;
  sequenciaDoConjunto: number;
  descricaoConjunto?: string;
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
}

export interface DespesaMvtoContabilItemDto {
  sequenciaDespesaMvtoNovo?: number;
  sequenciaDaDespesa: number;
  descricaoDespesa?: string;
  quantidade: number;
  valorUnitario: number;
  valorDeCusto: number;
  valorTotal: number;
}

export interface ParcelaMvtoContabilDto {
  numeroDaParcela: number;
  dataDeVencimento: string;
  valorDaParcela: number;
}

export interface MovimentoContabilFiltroDto {
  dataInicial?: string;
  dataFinal?: string;
  sequenciaDoGeral?: number;
  documento?: string;
  tipoDoMovimento?: number;
  pageNumber?: number;
  pageSize?: number;
}

// ============================================
// TIPOS PARA PRODUÇÃO INTELIGENTE (Cascata)
// ============================================

/**
 * Componente de produção com informações de estoque
 */
export interface ComponenteProducaoDto {
  sequenciaDoProduto: number;
  descricao: string;
  quantidadeNecessaria: number;
  estoqueDisponivel: number;
  falta: number;
  podeSerProduzido: boolean;
  industrializacao: boolean;
  subComponentes?: ComponenteProducaoDto[];
}

/**
 * Resultado da verificação de viabilidade de produção
 */
export interface VerificacaoProducaoResultDto {
  podeProduzir: boolean;
  mensagem: string;
  componentesFaltantes: ComponenteProducaoDto[];
  planoProducaoCascata: ItemPlanoProducaoDto[];
}

/**
 * Item do plano de produção ordenado
 */
export interface ItemPlanoProducaoDto {
  ordem: number;
  sequenciaDoProduto: number;
  descricao: string;
  quantidadeAProduzir: number;
  ehConjunto: boolean;
  dependeDe?: number;
}

/**
 * Requisição para executar produção em cascata
 */
export interface ProducaoCascataRequestDto {
  sequenciaDoProdutoOuConjunto: number;
  quantidade: number;
  ehConjunto: boolean;
  sequenciaDoGeral: number;
  documento?: string;
  observacao?: string;
  executarPlanoCompleto: boolean;
}

/**
 * Resultado da execução de produção em cascata
 */
export interface ProducaoCascataResultDto {
  sucesso: boolean;
  mensagem: string;
  movimentosGerados: MovimentoGeradoDto[];
  totalMovimentos: number;
  valorTotalProduzido: number;
}

/**
 * Movimento gerado na produção em cascata
 */
export interface MovimentoGeradoDto {
  sequenciaDoMovimento: number;
  tipo: string;
  descricao: string;
  quantidade: number;
  valorTotal: number;
}
