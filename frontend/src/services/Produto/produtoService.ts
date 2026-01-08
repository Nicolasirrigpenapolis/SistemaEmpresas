import api from '../api';
import type {
  ProdutoListDto,
  ProdutoDto,
  ProdutoCreateUpdateDto,
  ProdutoFiltroDto,
  ProdutoComboDto,
  PagedResult,
  ReceitaProdutoDto,
  ReceitaProdutoListDto,
  ReceitaProdutoCreateUpdateDto,
} from '../../types/Produto/produto';

// Interfaces para dados auxiliares
export interface GrupoProduto {
  sequenciaDoGrupoProduto: number;
  descricao: string;
  inativo: boolean;
}

export interface SubGrupoProduto {
  sequenciaDoSubGrupoProduto: number;
  sequenciaDoGrupoProduto: number;
  descricao: string;
}

export interface Unidade {
  sequenciaDaUnidade: number;
  descricao: string;
  siglaDaUnidade: string;
}

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

  // ===== Dados Auxiliares (Grupo, SubGrupo, Unidade) =====

  /**
   * Lista todos os grupos de produto ativos
   */
  async listarGrupos(incluirInativos: boolean = false): Promise<GrupoProduto[]> {
    const response = await api.get<GrupoProduto[]>(`${BASE_URL}/grupos`, {
      params: { incluirInativos }
    });
    return response.data;
  },

  /**
   * Lista todos os subgrupos de produto, filtrados por grupo
   */
  async listarSubGrupos(grupoId?: number): Promise<SubGrupoProduto[]> {
    const response = await api.get<SubGrupoProduto[]>(`${BASE_URL}/subgrupos`, {
      params: grupoId ? { grupoId } : undefined
    });
    return response.data;
  },

  /**
   * Lista todas as unidades
   */
  async listarUnidades(): Promise<Unidade[]> {
    const response = await api.get<Unidade[]>(`${BASE_URL}/unidades`);
    return response.data;
  },

  // ===== Foto do Produto =====

  /**
   * Retorna a URL da foto do produto (para uso com token)
   */
  getFotoUrl(id: number): string {
    const baseUrl = api.defaults.baseURL || '/api';
    return `${baseUrl}${BASE_URL}/${id}/foto`;
  },

  /**
   * Busca a foto do produto como blob
   */
  async obterFoto(id: number): Promise<string | null> {
    try {
      const response = await api.get(`${BASE_URL}/${id}/foto`, {
        responseType: 'blob'
      });
      // Converte o blob para uma URL de objeto
      const blob = new Blob([response.data], { type: response.headers['content-type'] });
      return URL.createObjectURL(blob);
    } catch {
      return null;
    }
  },

  /**
   * Verifica se o produto possui foto
   */
  async temFoto(id: number): Promise<boolean> {
    try {
      const response = await api.get<{ temFoto: boolean }>(`${BASE_URL}/${id}/tem-foto`);
      return response.data.temFoto;
    } catch {
      return false;
    }
  },

  /**
   * Faz upload de uma foto para o produto
   */
  async uploadFoto(id: number, arquivo: File): Promise<{ mensagem: string }> {
    const formData = new FormData();
    formData.append('arquivo', arquivo);

    const response = await api.post<{ mensagem: string }>(`${BASE_URL}/${id}/foto`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  },

  /**
   * Remove a foto do produto
   */
  async removerFoto(id: number): Promise<{ mensagem: string }> {
    const response = await api.delete<{ mensagem: string }>(`${BASE_URL}/${id}/foto`);
    return response.data;
  },

  // ===== Receita do Produto (Materia Prima) =====

  /**
   * Obtem a receita completa de um produto (materias primas)
   */
  async obterReceita(id: number): Promise<ReceitaProdutoDto> {
    const response = await api.get<ReceitaProdutoDto>(`${BASE_URL}/${id}/receita`);
    return response.data;
  },

  /**
   * Lista os itens da receita de um produto
   */
  async listarItensReceita(id: number): Promise<ReceitaProdutoListDto[]> {
    const response = await api.get<ReceitaProdutoListDto[]>(`${BASE_URL}/${id}/receita/itens`);
    return response.data;
  },

  /**
   * Adiciona um item a receita do produto
   */
  async adicionarItemReceita(id: number, item: ReceitaProdutoCreateUpdateDto): Promise<ReceitaProdutoListDto> {
    const response = await api.post<ReceitaProdutoListDto>(`${BASE_URL}/${id}/receita`, item);
    return response.data;
  },

  /**
   * Atualiza a quantidade de um item da receita
   */
  async atualizarItemReceita(id: number, materiaPrimaId: number, item: ReceitaProdutoCreateUpdateDto): Promise<ReceitaProdutoListDto> {
    const response = await api.put<ReceitaProdutoListDto>(`${BASE_URL}/${id}/receita/${materiaPrimaId}`, item);
    return response.data;
  },

  /**
   * Remove um item da receita
   */
  async removerItemReceita(id: number, materiaPrimaId: number): Promise<{ mensagem: string }> {
    const response = await api.delete<{ mensagem: string }>(`${BASE_URL}/${id}/receita/${materiaPrimaId}`);
    return response.data;
  },

  /**
   * Limpa toda a receita de um produto
   */
  async limparReceita(id: number): Promise<{ mensagem: string }> {
    const response = await api.delete<{ mensagem: string }>(`${BASE_URL}/${id}/receita`);
    return response.data;
  },
};

export default produtoService;
