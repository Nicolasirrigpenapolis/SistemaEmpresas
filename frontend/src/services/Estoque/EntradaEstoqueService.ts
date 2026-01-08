import api from '../api';

// DTOs de Pedido de Compra (Pendentes)
export interface PedidoCompraPendenteDto {
  idDoPedido: number;
  dataDoPedido: string;
  codigoDoFornecedor: number;
  nomeFornecedor: string;
  cnpjFornecedor: string;
  totalDoPedido: number;
  previsaoDeEntrega?: string;
  qtdeItensPendentes: number;
}

export interface ItemPedidoPendenteDto {
  idDoProduto: number;
  sequenciaDoItem: number;
  codigoDoProduto: string;
  descricaoProduto: string;
  unidadeMedida: string;
  qtdePedida: number;
  qtdeRecebida: number;
  qtdeRestante: number;
  vrUnitario: number;
  vrTotalRestante: number;
  notasAnteriores: string;
  qtdeAReceber?: number;
  notaFiscal?: string;
}

export interface PedidoCompraComItensPendentesDto {
  idDoPedido: number;
  dataDoPedido: string;
  codigoDoFornecedor: number;
  nomeFornecedor: string;
  cnpjFornecedor: string;
  totalDoPedido: number;
  itens: ItemPedidoPendenteDto[];
}

// DTOs de NFe (XML)
export interface NFeImportDto {
  chaveAcesso: string;
  numeroNota: string;
  serie: string;
  dataEmissao: string;
  emitente: {
    cnpj: string;
    nome: string;
    nomeFantasia: string;
    inscricaoEstadual: string;
  };
  itens: NFeItemDto[];
  valorTotal: number;
  valorProdutos: number;
  valorIcms: number;
  valorIpi: number;
}

export interface NFeItemDto {
  codigoProdutoFornecedor: string;
  descricaoProdutoFornecedor: string;
  ncm: string;
  cfop: string;
  unidadeMedida: string;
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  produtoIdSistema?: number;
  nomeProdutoSistema?: string;
  margemDeLucro?: number;
  valorIcms: number;
  valorIpi: number;
  aliquotaIcms: number;
}

export interface EntradaXmlResultDto {
  nfeData: NFeImportDto;
  notaJaImportada: boolean;
  fornecedorEncontrado: boolean;
  fornecedorId?: number;
  fornecedorNome?: string;
  itensVinculados: number;
  itensSemVinculo: number;
}

// DTOs de Comparação
export interface ItemComparacaoDto {
  idDoProduto: number;
  sequenciaDoItem: number;
  codigoProduto: string;
  descricaoProduto: string;
  unidadeMedida: string;
  qtdePedido: number;
  vrUnitarioPedido: number;
  encontradoNoXml: boolean;
  qtdeXml: number;
  vrUnitarioXml: number;
  codigoProdutoXml?: string;
  descricaoProdutoXml?: string;
  diferencaQtde: number;
  diferencaPreco: number;
  itemExtra: boolean;
}

export interface ComparacaoPedidoResultDto {
  idPedido: number;
  itens: ItemComparacaoDto[];
  itensExtras: ItemComparacaoDto[];
  totalItensConferidos: number;
  totalItensNaoEncontrados: number;
  totalItensExtras: number;
}

// DTOs de Confirmação
export interface ItemEntradaDto {
  produtoId: number;
  codigoProdutoFornecedor?: string;
  sequenciaItemPedido?: number;
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  aliquotaIcms: number;
  valorIcms: number;
  aliquotaIpi: number;
  valorIpi: number;
  receber: boolean;
}

export interface ConfirmarEntradaRequest {
  numeroNota: string;
  serie: string;
  chaveAcesso: string;
  dataEntrada?: string;
  fornecedorId: number;
  idPedido?: number;
  valorFrete: number;
  valorDesconto: number;
  valorTotal: number;
  itens: ItemEntradaDto[];
}

export interface EntradaConfirmadaDto {
  sucesso: boolean;
  sequenciaDoMovimento: number;
  mensagem: string;
  itensProcessados: number;
  pedidoFechado: boolean;
}

export interface ProdutoBuscaDto {
  sequenciaDoProduto: number;
  codigoDoProduto: string;
  descricao: string;
  unidadeMedida: string;
  valorCusto: number;
  margemDeLucro: number;
}

// Serviço
export const EntradaEstoqueService = {
  // Pedidos de Compra
  listarPedidosPendentes: async (fornecedorId?: number, busca?: string): Promise<PedidoCompraPendenteDto[]> => {
    const params = new URLSearchParams();
    if (fornecedorId) params.append('fornecedorId', fornecedorId.toString());
    if (busca) params.append('busca', busca);
    
    const response = await api.get<PedidoCompraPendenteDto[]>(`/pedidos-compra/pendentes?${params}`);
    return response.data;
  },

  obterItensPedido: async (idPedido: number): Promise<PedidoCompraComItensPendentesDto> => {
    const response = await api.get<PedidoCompraComItensPendentesDto>(`/pedidos-compra/${idPedido}/itens-pendentes`);
    return response.data;
  },

  buscarPedidos: async (termo: string): Promise<PedidoCompraPendenteDto[]> => {
    const response = await api.get<PedidoCompraPendenteDto[]>(`/pedidos-compra/buscar?termo=${encodeURIComponent(termo)}`);
    return response.data;
  },

  // Entrada de Estoque
  uploadXml: async (arquivo: File): Promise<EntradaXmlResultDto> => {
    const formData = new FormData();
    formData.append('arquivo', arquivo);

    const response = await api.post<EntradaXmlResultDto>('/estoque/entrada/upload-xml', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    });
    return response.data;
  },

  buscarSefaz: async (chaveAcesso: string): Promise<EntradaXmlResultDto> => {
    const response = await api.get<EntradaXmlResultDto>(`/estoque/entrada/buscar-sefaz/${chaveAcesso}`);
    return response.data;
  },

  compararComPedido: async (idPedido: number, itensXml: NFeItemDto[]): Promise<ComparacaoPedidoResultDto> => {
    const response = await api.post<ComparacaoPedidoResultDto>('/estoque/entrada/comparar-pedido', {
      idPedido,
      itensXml
    });
    return response.data;
  },

  confirmarEntrada: async (request: ConfirmarEntradaRequest): Promise<EntradaConfirmadaDto> => {
    const response = await api.post<EntradaConfirmadaDto>('/estoque/entrada/confirmar', request);
    return response.data;
  },

  buscarProduto: async (termo: string): Promise<ProdutoBuscaDto[]> => {
    const response = await api.get<ProdutoBuscaDto[]>(`/estoque/entrada/buscar-produto?termo=${encodeURIComponent(termo)}`);
    return response.data;
  }
};
