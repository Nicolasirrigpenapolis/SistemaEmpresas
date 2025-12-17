import api from '../api';
import type {
  ClassificacaoFiscal,
  ClassificacaoFiscalInput,
  ClassificacaoFiscalFiltros,
  PagedResult,
  SyncResult,
  SyncStatus,
} from '../../types/Fiscal/classificacaoFiscal';

// ============================================================================
// SERVICE DE CLASSIFICAÇÃO FISCAL
// Endpoints: /api/classificacao-fiscal
// ============================================================================

export const classificacaoFiscalService = {
  // ==========================================================================
  // LISTAGEM E CONSULTAS
  // ==========================================================================

  /**
   * Lista classificações fiscais com paginação e filtros
   */
  async listar(filtros?: ClassificacaoFiscalFiltros): Promise<PagedResult<ClassificacaoFiscal>> {
    const params = new URLSearchParams();

    if (filtros) {
      if (filtros.pageNumber) params.append('pageNumber', filtros.pageNumber.toString());
      if (filtros.pageSize) params.append('pageSize', filtros.pageSize.toString());
      if (filtros.ncm) params.append('ncm', filtros.ncm);
      if (filtros.descricao) params.append('descricao', filtros.descricao);
      if (filtros.somenteNFe) params.append('somenteNFe', 'true');
      if (filtros.incluirInativos) params.append('incluirInativos', 'true');
      if (filtros.tributacao && filtros.tributacao !== 'todos') {
        params.append('tributacao', filtros.tributacao);
      }
    }

    const response = await api.get<PagedResult<ClassificacaoFiscal>>(
      `/classificacao-fiscal?${params}`
    );
    return response.data;
  },

  /**
   * Busca classificação por ID (sequência)
   */
  async buscarPorId(id: number): Promise<ClassificacaoFiscal> {
    const response = await api.get<ClassificacaoFiscal>(`/classificacao-fiscal/${id}`);
    return response.data;
  },

  /**
   * Busca classificação por NCM
   */
  async buscarPorNcm(ncm: string): Promise<ClassificacaoFiscal> {
    const response = await api.get<ClassificacaoFiscal>(`/classificacao-fiscal/ncm/${ncm}`);
    return response.data;
  },

  /**
   * Busca classificação por código ClassTrib (SVRS)
   */
  async buscarPorClassTrib(cClassTrib: string): Promise<ClassificacaoFiscal> {
    const response = await api.get<ClassificacaoFiscal>(
      `/classificacao-fiscal/classtrib/${cClassTrib}`
    );
    return response.data;
  },

  /**
   * Lista apenas classificações válidas para NFe
   */
  async listarNFe(): Promise<ClassificacaoFiscal[]> {
    const response = await api.get<ClassificacaoFiscal[]>('/classificacao-fiscal/nfe');
    return response.data;
  },

  /**
   * Pesquisa classificações por termo (NCM ou descrição)
   */
  async pesquisar(termo: string): Promise<ClassificacaoFiscal[]> {
    const response = await api.get<ClassificacaoFiscal[]>(
      `/classificacao-fiscal/pesquisar?termo=${encodeURIComponent(termo)}`
    );
    return response.data;
  },

  // ==========================================================================
  // CRUD
  // ==========================================================================

  /**
   * Cria nova classificação fiscal
   */
  async criar(dados: ClassificacaoFiscalInput): Promise<ClassificacaoFiscal> {
    const response = await api.post<ClassificacaoFiscal>('/classificacao-fiscal', dados);
    return response.data;
  },

  /**
   * Atualiza classificação fiscal existente
   */
  async atualizar(id: number, dados: ClassificacaoFiscalInput): Promise<ClassificacaoFiscal> {
    const payload = {
      sequenciaDaClassificacao: id,
      ...dados,
    };
    const response = await api.put<ClassificacaoFiscal>(`/classificacao-fiscal/${id}`, payload);
    return response.data;
  },

  /**
   * Exclui (inativa) classificação fiscal
   */
  async excluir(id: number): Promise<void> {
    await api.delete(`/classificacao-fiscal/${id}`);
  },

  // ==========================================================================
  // SINCRONIZAÇÃO COM API SVRS
  // ==========================================================================

  /**
   * Sincroniza todas as classificações NFe com API SVRS
   * @param forcar - Força atualização ignorando cache
   */
  async sincronizar(forcar: boolean = false): Promise<SyncResult> {
    const response = await api.post<SyncResult>(
      `/classificacao-fiscal/sincronizar?forcar=${forcar}`
    );
    return response.data;
  },

  /**
   * Sincroniza uma classificação específica por código ClassTrib
   */
  async sincronizarPorClassTrib(cClassTrib: string): Promise<ClassificacaoFiscal> {
    const response = await api.post<ClassificacaoFiscal>(
      `/classificacao-fiscal/sincronizar/${cClassTrib}`
    );
    return response.data;
  },

  /**
   * Obtém status da última sincronização
   */
  async getStatusSincronizacao(): Promise<SyncStatus> {
    const response = await api.get<SyncStatus>('/classificacao-fiscal/status-sincronizacao');
    return response.data;
  },
};

export default classificacaoFiscalService;
