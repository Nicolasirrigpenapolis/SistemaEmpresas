export interface DashboardKpi {
  orcamentosAbertos: number;
  comprasPendentesValidacao: number;
  totalProdutos: number;
  totalConjuntos: number;
  produtosEstoqueCritico: number;
}

export interface OrcamentoResumido {
  sequenciaDoOrcamento: number;
  dataDeEmissao: string | null;
  nomeCliente: string;
  nomeVendedor: string | null;
  vendaFechada: boolean;
  cancelado: boolean;
  diasAberto: number;
}

export interface OrcamentosPorStatus {
  status: string;
  quantidade: number;
}

export interface OrdensServicoStatus {
  status: string;
  quantidade: number;
}

export interface ProdutoEstoqueCritico {
  sequenciaDoProduto: number;
  descricao: string;
  quantidadeNoEstoque: number;
  quantidadeMinima: number;
  diferenca: number;
  localizacao: string | null;
}

export interface AlertaOperacional {
  tipo: string;
  mensagem: string;
  referencia: string | null;
  quantidade: number;
  dataReferencia: string | null;
}

export interface OrcamentosPorVendedor {
  sequenciaDoVendedor: number;
  nomeVendedor: string;
  totalOrcamentos: number;
  orcamentosFechados: number;
  taxaConversao: number;
}

export interface TimelineOrcamentos {
  periodo: string;
  quantidadeOrcamentos: number;
  quantidadePedidos: number;
}

export interface PedidoCompraResumido {
  idDoPedido: number;
  dataDoPedido: string;
  nomeFornecedor: string;
  previsaoDeEntrega: string | null;
  pedidoFechado: boolean;
  validado: boolean;
  cancelado: boolean;
  diasAtraso: number;
}
