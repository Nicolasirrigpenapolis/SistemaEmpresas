import api from './api';
import type {
  NotaFiscalListDto,
  NotaFiscalDto,
  NotaFiscalCreateUpdateDto,
  NotaFiscalFiltroDto,
  ClienteComboDto,
  TransportadoraComboDto,
  NaturezaOperacaoComboDto,
  PropriedadeComboDto,
  TipoCobrancaComboDto,
  VendedorComboDto,
  CancelarNfeDto,
  PagedResult,
  ProdutoDaNotaFiscalDto,
  ProdutoDaNotaFiscalCreateDto,
  ConjuntoDaNotaFiscalDto,
  ConjuntoDaNotaFiscalCreateDto,
  PecaDaNotaFiscalDto,
  PecaDaNotaFiscalCreateDto,
  ParcelaNotaFiscalDto,
  ParcelaNotaFiscalCreateDto,
  ProdutoComboDto,
  ConjuntoComboDto,
  PecaComboDto,
  CalculoImpostoRequestDto,
  CalculoImpostoResultDto,
} from '../types/notaFiscal';

const BASE_URL = '/notafiscal';

export const notaFiscalService = {
  // ================================
  // CRUD
  // ================================

  /**
   * Lista notas fiscais com paginação e filtros
   */
  async listar(filtro: NotaFiscalFiltroDto = {}): Promise<PagedResult<NotaFiscalListDto>> {
    const params = new URLSearchParams();
    
    if (filtro.busca) params.append('busca', filtro.busca);
    if (filtro.dataInicial) params.append('dataInicial', filtro.dataInicial);
    if (filtro.dataFinal) params.append('dataFinal', filtro.dataFinal);
    if (filtro.cliente) params.append('cliente', filtro.cliente.toString());
    if (filtro.natureza) params.append('natureza', filtro.natureza.toString());
    if (filtro.propriedade) params.append('propriedade', filtro.propriedade.toString());
    if (filtro.tipoDeNota !== undefined) params.append('tipoDeNota', filtro.tipoDeNota.toString());
    if (filtro.canceladas !== undefined) params.append('canceladas', filtro.canceladas.toString());
    if (filtro.transmitidas !== undefined) params.append('transmitidas', filtro.transmitidas.toString());
    if (filtro.autorizadas !== undefined) params.append('autorizadas', filtro.autorizadas.toString());
    if (filtro.numeroDaNfe) params.append('numeroDaNfe', filtro.numeroDaNfe.toString());
    
    params.append('pageNumber', (filtro.pageNumber || 1).toString());
    params.append('pageSize', (filtro.pageSize || 25).toString());

    const response = await api.get<PagedResult<NotaFiscalListDto>>(`${BASE_URL}?${params.toString()}`);
    return response.data;
  },

  /**
   * Obtém uma nota fiscal por ID
   */
  async obterPorId(id: number): Promise<NotaFiscalDto> {
    const response = await api.get<NotaFiscalDto>(`${BASE_URL}/${id}`);
    return response.data;
  },

  /**
   * Cria uma nova nota fiscal
   */
  async criar(nota: NotaFiscalCreateUpdateDto): Promise<NotaFiscalDto> {
    const response = await api.post<NotaFiscalDto>(BASE_URL, nota);
    return response.data;
  },

  /**
   * Atualiza uma nota fiscal existente
   */
  async atualizar(id: number, nota: NotaFiscalCreateUpdateDto): Promise<NotaFiscalDto> {
    const response = await api.put<NotaFiscalDto>(`${BASE_URL}/${id}`, nota);
    return response.data;
  },

  /**
   * Cancela uma nota fiscal
   */
  async cancelar(id: number, justificativa: string): Promise<void> {
    const dto: CancelarNfeDto = {
      sequenciaDaNotaFiscal: id,
      justificativa,
    };
    await api.post(`${BASE_URL}/${id}/cancelar`, dto);
  },

  /**
   * Duplica uma nota fiscal existente
   * Copia todos os dados exceto: autorização, transmissão, chaves NFe e XMLs
   * A nova nota recebe o próximo número disponível
   */
  async duplicar(id: number): Promise<NotaFiscalDto> {
    const response = await api.post<NotaFiscalDto>(`${BASE_URL}/${id}/duplicar`);
    return response.data;
  },

  /**
   * Verifica se pode editar uma nota fiscal
   */
  async podeEditar(id: number): Promise<boolean> {
    const response = await api.get<{ podeEditar: boolean }>(`${BASE_URL}/${id}/pode-editar`);
    return response.data.podeEditar;
  },

  /**
   * Obtém o próximo número da nota fiscal
   */
  async obterProximoNumero(propriedade: number): Promise<number> {
    const response = await api.get<{ numero: number }>(`${BASE_URL}/proximo-numero/${propriedade}`);
    return response.data.numero;
  },

  // ================================
  // COMBOS
  // ================================

  /**
   * Lista clientes para combo/autocomplete
   */
  async listarClientes(busca?: string): Promise<ClienteComboDto[]> {
    const params = busca ? `?busca=${encodeURIComponent(busca)}` : '';
    const response = await api.get<ClienteComboDto[]>(`${BASE_URL}/combo/clientes${params}`);
    return response.data;
  },

  /**
   * Obtém um cliente específico
   */
  async obterCliente(id: number): Promise<ClienteComboDto> {
    const response = await api.get<ClienteComboDto>(`${BASE_URL}/combo/clientes/${id}`);
    return response.data;
  },

  /**
   * Lista transportadoras para combo/autocomplete
   */
  async listarTransportadoras(busca?: string): Promise<TransportadoraComboDto[]> {
    const params = busca ? `?busca=${encodeURIComponent(busca)}` : '';
    const response = await api.get<TransportadoraComboDto[]>(`${BASE_URL}/combo/transportadoras${params}`);
    return response.data;
  },

  /**
   * Obtém uma transportadora específica
   */
  async obterTransportadora(id: number): Promise<TransportadoraComboDto> {
    const response = await api.get<TransportadoraComboDto>(`${BASE_URL}/combo/transportadoras/${id}`);
    return response.data;
  },

  /**
   * Lista naturezas de operação
   */
  async listarNaturezas(entradaSaida?: boolean): Promise<NaturezaOperacaoComboDto[]> {
    const params = entradaSaida !== undefined ? `?entradaSaida=${entradaSaida}` : '';
    const response = await api.get<NaturezaOperacaoComboDto[]>(`${BASE_URL}/combo/naturezas${params}`);
    return response.data;
  },

  /**
   * Lista propriedades/filiais
   */
  async listarPropriedades(): Promise<PropriedadeComboDto[]> {
    const response = await api.get<PropriedadeComboDto[]>(`${BASE_URL}/combo/propriedades`);
    return response.data;
  },

  /**
   * Lista propriedades vinculadas a um cliente específico
   */
  async listarPropriedadesPorCliente(sequenciaDoGeral: number): Promise<PropriedadeComboDto[]> {
    const response = await api.get<PropriedadeComboDto[]>(`${BASE_URL}/combo/propriedades/cliente/${sequenciaDoGeral}`);
    return response.data;
  },

  /**
   * Lista tipos de cobrança
   */
  async listarTiposCobranca(): Promise<TipoCobrancaComboDto[]> {
    const response = await api.get<TipoCobrancaComboDto[]>(`${BASE_URL}/combo/tipos-cobranca`);
    return response.data;
  },

  /**
   * Lista vendedores para combo/autocomplete
   */
  async listarVendedores(busca?: string): Promise<VendedorComboDto[]> {
    const params = busca ? `?busca=${encodeURIComponent(busca)}` : '';
    const response = await api.get<VendedorComboDto[]>(`${BASE_URL}/combo/vendedores${params}`);
    return response.data;
  },

  // ================================
  // PRODUTOS
  // ================================

  /**
   * Lista produtos de uma nota fiscal
   */
  async listarProdutos(notaFiscalId: number): Promise<ProdutoDaNotaFiscalDto[]> {
    const response = await api.get<ProdutoDaNotaFiscalDto[]>(`${BASE_URL}/${notaFiscalId}/produtos`);
    return response.data;
  },

  /**
   * Adiciona um produto à nota fiscal
   */
  async adicionarProduto(notaFiscalId: number, produto: ProdutoDaNotaFiscalCreateDto): Promise<ProdutoDaNotaFiscalDto> {
    const response = await api.post<ProdutoDaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/produtos`, produto);
    return response.data;
  },

  /**
   * Atualiza um produto da nota fiscal
   */
  async atualizarProduto(notaFiscalId: number, produtoId: number, produto: ProdutoDaNotaFiscalCreateDto): Promise<ProdutoDaNotaFiscalDto> {
    const response = await api.put<ProdutoDaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/produtos/${produtoId}`, produto);
    return response.data;
  },

  /**
   * Remove um produto da nota fiscal
   */
  async removerProduto(notaFiscalId: number, produtoId: number): Promise<void> {
    await api.delete(`${BASE_URL}/${notaFiscalId}/produtos/${produtoId}`);
  },

  /**
   * Calcula impostos de um item (produto, conjunto ou peça) para a nota fiscal
   */
  async calcularImposto(notaFiscalId: number, request: CalculoImpostoRequestDto): Promise<CalculoImpostoResultDto> {
    const response = await api.post<CalculoImpostoResultDto>(`${BASE_URL}/${notaFiscalId}/calcular-imposto`, request);
    return response.data;
  },

  /**
   * Lista produtos para combo/autocomplete
   */
  async listarProdutosCombo(busca?: string): Promise<ProdutoComboDto[]> {
    const params = busca ? `?busca=${encodeURIComponent(busca)}` : '';
    const response = await api.get<ProdutoComboDto[]>(`${BASE_URL}/combo/produtos${params}`);
    return response.data;
  },

  // ================================
  // CONJUNTOS
  // ================================

  /**
   * Lista conjuntos de uma nota fiscal
   */
  async listarConjuntos(notaFiscalId: number): Promise<ConjuntoDaNotaFiscalDto[]> {
    const response = await api.get<ConjuntoDaNotaFiscalDto[]>(`${BASE_URL}/${notaFiscalId}/conjuntos`);
    return response.data;
  },

  /**
   * Adiciona um conjunto à nota fiscal
   */
  async adicionarConjunto(notaFiscalId: number, conjunto: ConjuntoDaNotaFiscalCreateDto): Promise<ConjuntoDaNotaFiscalDto> {
    const response = await api.post<ConjuntoDaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/conjuntos`, conjunto);
    return response.data;
  },

  /**
   * Atualiza um conjunto da nota fiscal
   */
  async atualizarConjunto(notaFiscalId: number, conjuntoId: number, conjunto: ConjuntoDaNotaFiscalCreateDto): Promise<ConjuntoDaNotaFiscalDto> {
    const response = await api.put<ConjuntoDaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/conjuntos/${conjuntoId}`, conjunto);
    return response.data;
  },

  /**
   * Remove um conjunto da nota fiscal
   */
  async removerConjunto(notaFiscalId: number, conjuntoId: number): Promise<void> {
    await api.delete(`${BASE_URL}/${notaFiscalId}/conjuntos/${conjuntoId}`);
  },

  /**
   * Lista conjuntos para combo/autocomplete
   */
  async listarConjuntosCombo(busca?: string): Promise<ConjuntoComboDto[]> {
    const params = busca ? `?busca=${encodeURIComponent(busca)}` : '';
    const response = await api.get<ConjuntoComboDto[]>(`${BASE_URL}/combo/conjuntos${params}`);
    return response.data;
  },

  // ================================
  // PEÇAS
  // ================================

  /**
   * Lista peças de uma nota fiscal
   */
  async listarPecas(notaFiscalId: number): Promise<PecaDaNotaFiscalDto[]> {
    const response = await api.get<PecaDaNotaFiscalDto[]>(`${BASE_URL}/${notaFiscalId}/pecas`);
    return response.data;
  },

  /**
   * Adiciona uma peça à nota fiscal
   */
  async adicionarPeca(notaFiscalId: number, peca: PecaDaNotaFiscalCreateDto): Promise<PecaDaNotaFiscalDto> {
    const response = await api.post<PecaDaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/pecas`, peca);
    return response.data;
  },

  /**
   * Atualiza uma peça da nota fiscal
   */
  async atualizarPeca(notaFiscalId: number, pecaId: number, peca: PecaDaNotaFiscalCreateDto): Promise<PecaDaNotaFiscalDto> {
    const response = await api.put<PecaDaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/pecas/${pecaId}`, peca);
    return response.data;
  },

  /**
   * Remove uma peça da nota fiscal
   */
  async removerPeca(notaFiscalId: number, pecaId: number): Promise<void> {
    await api.delete(`${BASE_URL}/${notaFiscalId}/pecas/${pecaId}`);
  },

  /**
   * Lista peças para combo/autocomplete
   */
  async listarPecasCombo(busca?: string): Promise<PecaComboDto[]> {
    const params = busca ? `?busca=${encodeURIComponent(busca)}` : '';
    const response = await api.get<PecaComboDto[]>(`${BASE_URL}/combo/pecas${params}`);
    return response.data;
  },

  // ================================
  // PARCELAS
  // ================================

  /**
   * Lista parcelas de uma nota fiscal
   */
  async listarParcelas(notaFiscalId: number): Promise<ParcelaNotaFiscalDto[]> {
    const response = await api.get<ParcelaNotaFiscalDto[]>(`${BASE_URL}/${notaFiscalId}/parcelas`);
    return response.data;
  },

  /**
   * Adiciona uma parcela à nota fiscal
   */
  async adicionarParcela(notaFiscalId: number, parcela: ParcelaNotaFiscalCreateDto): Promise<ParcelaNotaFiscalDto> {
    const response = await api.post<ParcelaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/parcelas`, parcela);
    return response.data;
  },

  /**
   * Atualiza uma parcela da nota fiscal
   */
  async atualizarParcela(notaFiscalId: number, parcelaId: number, parcela: ParcelaNotaFiscalCreateDto): Promise<ParcelaNotaFiscalDto> {
    const response = await api.put<ParcelaNotaFiscalDto>(`${BASE_URL}/${notaFiscalId}/parcelas/${parcelaId}`, parcela);
    return response.data;
  },

  /**
   * Remove uma parcela da nota fiscal
   */
  async removerParcela(notaFiscalId: number, parcelaId: number): Promise<void> {
    await api.delete(`${BASE_URL}/${notaFiscalId}/parcelas/${parcelaId}`);
  },

  /**
   * Gera parcelas automaticamente
   */
  async gerarParcelas(notaFiscalId: number, numeroParcelas: number, diasEntreParcelas: number): Promise<ParcelaNotaFiscalDto[]> {
    const response = await api.post<ParcelaNotaFiscalDto[]>(`${BASE_URL}/${notaFiscalId}/parcelas/gerar`, {
      numeroParcelas,
      diasEntreParcelas,
    });
    return response.data;
  },
};

export default notaFiscalService;
