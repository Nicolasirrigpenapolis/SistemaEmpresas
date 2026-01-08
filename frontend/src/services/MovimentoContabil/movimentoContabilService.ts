import api from '../api';
import type { 
  EstoqueInfoDto, 
  AjusteMovimentoContabilDto, 
  AjusteMovimentoContabilResultDto, 
  AjusteMovimentoContabilLoteDto, 
  AjusteMovimentoContabilLoteResultDto,
  MovimentoEstoqueDto,
  MovimentoEstoqueFiltroDto,
  MovimentoContabilNovoDto,
  MovimentoContabilFiltroDto,
  VerificacaoProducaoResultDto,
  ProducaoCascataRequestDto,
  ProducaoCascataResultDto
} from '../../types/Estoque/movimentoContabil';
import type { PagedResult } from '../../types/Common/common';

const BASE_URL = '/movimentocontabil';

export const movimentoContabilService = {
  /**
   * Obtém informações de estoque de um produto
   */
  async obterEstoqueProduto(sequenciaDoProduto: number): Promise<EstoqueInfoDto> {
    const response = await api.get<EstoqueInfoDto>(`${BASE_URL}/produto/${sequenciaDoProduto}`);
    return response.data;
  },

  /**
   * Obtém informações de estoque de um conjunto
   */
  async obterEstoqueConjunto(sequenciaDoConjunto: number): Promise<EstoqueInfoDto> {
    const response = await api.get<EstoqueInfoDto>(`${BASE_URL}/conjunto/${sequenciaDoConjunto}`);
    return response.data;
  },

  /**
   * Busca produtos para ajuste de estoque
   */
  async buscarProdutos(busca?: string, limite: number = 50): Promise<EstoqueInfoDto[]> {
    const params = new URLSearchParams();
    if (busca) params.append('busca', busca);
    params.append('limite', limite.toString());

    const response = await api.get<EstoqueInfoDto[]>(`${BASE_URL}/buscar-produtos?${params.toString()}`);
    return response.data;
  },

  /**
   * Busca conjuntos para movimento contábil
   */
  async buscarConjuntos(busca?: string, limite: number = 50): Promise<EstoqueInfoDto[]> {
    const params = new URLSearchParams();
    if (busca) params.append('busca', busca);
    params.append('limite', limite.toString());

    const response = await api.get<EstoqueInfoDto[]>(`${BASE_URL}/buscar-conjuntos?${params.toString()}`);
    return response.data;
  },

  /**
   * Busca despesas para movimento contábil
   */
  async buscarDespesas(busca?: string, limite: number = 50): Promise<any[]> {
    const params = new URLSearchParams();
    if (busca) params.append('busca', busca);
    params.append('limite', limite.toString());

    const response = await api.get<any[]>(`${BASE_URL}/buscar-despesas?${params.toString()}`);
    return response.data;
  },

  /**
   * Realiza ajuste de estoque para um produto
   */
  async realizarAjuste(dto: AjusteMovimentoContabilDto): Promise<AjusteMovimentoContabilResultDto> {
    const response = await api.post<AjusteMovimentoContabilResultDto>(`${BASE_URL}/ajuste`, dto);
    return response.data;
  },

  /**
   * Realiza ajuste de estoque em lote
   */
  async realizarAjusteLote(dto: AjusteMovimentoContabilLoteDto): Promise<AjusteMovimentoContabilLoteResultDto> {
    const response = await api.post<AjusteMovimentoContabilLoteResultDto>(`${BASE_URL}/ajuste-lote`, dto);
    return response.data;
  },

  /**
   * Lista movimentos de estoque de um produto
   */
  async listarMovimentos(filtro: MovimentoEstoqueFiltroDto): Promise<PagedResult<MovimentoEstoqueDto>> {
    const params = new URLSearchParams();
    params.append('sequenciaDoProduto', filtro.sequenciaDoProduto.toString());
    params.append('ehConjunto', (filtro.ehConjunto || false).toString());
    
    if (filtro.dataInicial) params.append('dataInicial', filtro.dataInicial);
    if (filtro.dataFinal) params.append('dataFinal', filtro.dataFinal);
    if (filtro.tipoMovimento !== undefined) params.append('tipoMovimento', filtro.tipoMovimento.toString());
    if (filtro.documento) params.append('documento', filtro.documento);
    params.append('pageNumber', (filtro.pageNumber || 1).toString());
    params.append('pageSize', (filtro.pageSize || 25).toString());

    const response = await api.get<PagedResult<MovimentoEstoqueDto>>(`${BASE_URL}/movimentos?${params.toString()}`);
    return response.data;
  },

  /**
   * Recalcula e atualiza o estoque contábil de um produto
   */
  async recalcularEstoque(sequenciaDoProduto: number): Promise<{ mensagem: string; novoEstoque: number }> {
    const response = await api.post<{ mensagem: string; novoEstoque: number }>(
      `${BASE_URL}/recalcular/${sequenciaDoProduto}`
    );
    return response.data;
  },

  /**
   * Lista movimentos contábeis novos
   */
  async listarMovimentosNovos(filtro: MovimentoContabilFiltroDto): Promise<PagedResult<MovimentoContabilNovoDto>> {
    const response = await api.get<PagedResult<MovimentoContabilNovoDto>>(`${BASE_URL}/novos`, { params: filtro });
    return response.data;
  },

  /**
   * Obtém um movimento contábil novo pelo ID
   */
  async obterMovimento(id: number): Promise<MovimentoContabilNovoDto> {
    const response = await api.get<MovimentoContabilNovoDto>(`${BASE_URL}/novo/${id}`);
    return response.data;
  },

  /**
   * Cria um novo movimento contábil
   */
  async criarMovimento(dto: MovimentoContabilNovoDto): Promise<MovimentoContabilNovoDto> {
    const response = await api.post<MovimentoContabilNovoDto>(`${BASE_URL}/novo`, dto);
    return response.data;
  },

  /**
   * Atualiza um movimento contábil existente
   */
  async atualizarMovimento(id: number, dto: MovimentoContabilNovoDto): Promise<void> {
    await api.put(`${BASE_URL}/novo/${id}`, dto);
  },

  /**
   * Exclui um movimento contábil
   */
  async excluirMovimento(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/novo/${id}`);
  },

  // ============================================
  // PRODUÇÃO INTELIGENTE (Cascata)
  // ============================================

  /**
   * Verifica viabilidade de produção de um produto/conjunto
   * Retorna componentes faltantes e plano de produção sugerido
   */
  async verificarViabilidadeProducao(
    id: number,
    quantidade: number,
    ehConjunto: boolean = false
  ): Promise<VerificacaoProducaoResultDto> {
    const params = new URLSearchParams();
    params.append('id', id.toString());
    params.append('quantidade', quantidade.toString());
    params.append('ehConjunto', ehConjunto.toString());

    const response = await api.get<VerificacaoProducaoResultDto>(
      `${BASE_URL}/producao/verificar?${params.toString()}`
    );
    return response.data;
  },

  /**
   * Executa produção em cascata
   * Cria automaticamente todos os itens intermediários necessários
   */
  async executarProducaoCascata(request: ProducaoCascataRequestDto): Promise<ProducaoCascataResultDto> {
    const response = await api.post<ProducaoCascataResultDto>(`${BASE_URL}/producao/cascata`, request);
    return response.data;
  }
};

export default movimentoContabilService;
