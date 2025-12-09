import api from './api';
import type {
  ProdutoListDto,
  ProdutoDto,
  ProdutoCreateUpdateDto,
  ProdutoFiltroDto,
  ProdutoComboDto,
  PagedResult,
} from '../types/produto';

const BASE_URL = '/produto';

export const produtoService = {
  /**
   * Lista produtos com paginação e filtros
   */
  async listar(filtro: ProdutoFiltroDto = {}): Promise<PagedResult<ProdutoListDto>> {
    const params = new URLSearchParams();
    
    if (filtro.busca) params.append('busca', filtro.busca);
    if (filtro.grupoProduto) params.append('grupoProduto', filtro.grupoProduto.toString());
    if (filtro.subGrupoProduto) params.append('subGrupoProduto', filtro.subGrupoProduto.toString());
    if (filtro.eMateriaPrima !== undefined) params.append('eMateriaPrima', filtro.eMateriaPrima.toString());
    if (filtro.estoqueCritico !== undefined) params.append('estoqueCritico', filtro.estoqueCritico.toString());
    if (filtro.incluirInativos) params.append('incluirInativos', 'true');
    params.append('pageNumber', (filtro.pageNumber || 1).toString());
    params.append('pageSize', (filtro.pageSize || 25).toString());

    const response = await api.get<PagedResult<ProdutoListDto>>(`${BASE_URL}?${params.toString()}`);
    return response.data;
  },

  /**
   * Obtém um produto por ID
   */
  async obterPorId(id: number): Promise<ProdutoDto> {
    const response = await api.get<ProdutoDto>(`${BASE_URL}/${id}`);
    return response.data;
  },

  /**
   * Cria um novo produto
   */
  async criar(produto: ProdutoCreateUpdateDto): Promise<ProdutoDto> {
    const response = await api.post<ProdutoDto>(BASE_URL, produto);
    return response.data;
  },

  /**
   * Atualiza um produto existente
   */
  async atualizar(id: number, produto: ProdutoCreateUpdateDto): Promise<ProdutoDto> {
    const response = await api.put<ProdutoDto>(`${BASE_URL}/${id}`, produto);
    return response.data;
  },

  /**
   * Inativa um produto
   */
  async inativar(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/${id}`);
  },

  /**
   * Ativa um produto
   */
  async ativar(id: number): Promise<void> {
    await api.patch(`${BASE_URL}/${id}/ativar`);
  },

  /**
   * Lista produtos para combo/select
   */
  async listarParaCombo(busca?: string): Promise<ProdutoComboDto[]> {
    const params = busca ? `?busca=${encodeURIComponent(busca)}` : '';
    const response = await api.get<ProdutoComboDto[]>(`${BASE_URL}/combo${params}`);
    return response.data;
  },

  /**
   * Conta produtos com estoque crítico
   */
  async contarEstoqueCritico(): Promise<number> {
    const response = await api.get<number>(`${BASE_URL}/estoque-critico/count`);
    return response.data;
  },
};

export default produtoService;
