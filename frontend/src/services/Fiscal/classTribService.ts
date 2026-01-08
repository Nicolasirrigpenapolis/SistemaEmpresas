import api from '../api';
import type {
  ClassTribDto,
  ClassTribAutocomplete,
  ClassTribPagedResult,
  ClassTribSyncResult,
  ClassTribSyncStatus,
} from '../../types/Fiscal/classTrib';

// Re-exportar tipos para compatibilidade
export type { ClassTribDto, ClassTribAutocomplete, ClassTribPagedResult };

// Alias para compatibilidade com código existente
export type SyncResult = ClassTribSyncResult;
export type SyncStatus = ClassTribSyncStatus;

// ============================================================================
// SERVICE DE CLASSTRIB
// Endpoints: /api/classtrib
// ============================================================================

export const classTribService = {
  // ==========================================================================
  // LISTAGEM E CONSULTAS
  // ==========================================================================

  /**
   * Lista ClassTribs com paginação e filtros
   */
  async listar(
    page: number = 1,
    pageSize: number = 50,
    cst?: string,
    descricao?: string,
    somenteNFe?: boolean
  ): Promise<ClassTribPagedResult> {
    const params = new URLSearchParams();
    params.append('page', page.toString());
    params.append('pageSize', pageSize.toString());
    if (cst) params.append('cst', cst);
    if (descricao) params.append('descricao', descricao);
    if (somenteNFe) params.append('somenteNFe', 'true');

    const response = await api.get<ClassTribPagedResult>(`/classtrib?${params}`);
    return response.data;
  },

  /**
   * Busca ClassTrib por ID
   */
  async buscarPorId(id: number): Promise<ClassTribDto> {
    const response = await api.get<ClassTribDto>(`/classtrib/${id}`);
    return response.data;
  },

  /**
   * Busca ClassTrib por código (cClassTrib)
   */
  async buscarPorCodigo(codigo: string): Promise<ClassTribDto> {
    const response = await api.get<ClassTribDto>(`/classtrib/codigo/${codigo}`);
    return response.data;
  },

  /**
   * Lista ClassTribs válidos para NFe
   */
  async listarNFe(): Promise<ClassTribDto[]> {
    const response = await api.get<ClassTribDto[]>('/classtrib/nfe');
    return response.data;
  },

  /**
   * Lista ClassTribs por CST
   */
  async listarPorCst(cst: string): Promise<ClassTribDto[]> {
    const response = await api.get<ClassTribDto[]>(`/classtrib/cst/${cst}`);
    return response.data;
  },

  /**
   * Pesquisa ClassTribs por termo
   */
  async pesquisar(termo: string, limite: number = 50): Promise<ClassTribDto[]> {
    const response = await api.get<ClassTribDto[]>(
      `/classtrib/search?q=${encodeURIComponent(termo)}&limite=${limite}`
    );
    return response.data;
  },

  /**
   * Autocomplete para seleção de ClassTrib
   */
  async autocomplete(termo: string, limite: number = 20): Promise<ClassTribAutocomplete[]> {
    if (!termo || termo.length < 1) return [];
    
    const response = await api.get<ClassTribAutocomplete[]>(
      `/classtrib/autocomplete?q=${encodeURIComponent(termo)}&limite=${limite}`
    );
    return response.data;
  },

  // ==========================================================================
  // FILTROS AVANÇADOS
  // ==========================================================================

  /**
   * Busca com filtros avançados
   */
  async filtroAvancado(
    page: number = 1,
    pageSize: number = 50,
    csts?: string,
    tipoAliquota?: string,
    minReducaoIBS?: number,
    maxReducaoIBS?: number,
    minReducaoCBS?: number,
    maxReducaoCBS?: number,
    validoNFe?: boolean,
    tributacaoRegular?: boolean,
    creditoPresumido?: boolean,
    descricao?: string,
    ordenarPor?: string
  ): Promise<ClassTribPagedResult> {
    const params = new URLSearchParams();
    params.append('page', page.toString());
    params.append('pageSize', pageSize.toString());
    if (csts) params.append('csts', csts);
    if (tipoAliquota) params.append('tipoAliquota', tipoAliquota);
    if (minReducaoIBS !== undefined) params.append('minReducaoIBS', minReducaoIBS.toString());
    if (maxReducaoIBS !== undefined) params.append('maxReducaoIBS', maxReducaoIBS.toString());
    if (minReducaoCBS !== undefined) params.append('minReducaoCBS', minReducaoCBS.toString());
    if (maxReducaoCBS !== undefined) params.append('maxReducaoCBS', maxReducaoCBS.toString());
    if (validoNFe !== undefined) params.append('validoNFe', validoNFe.toString());
    if (tributacaoRegular !== undefined) params.append('tributacaoRegular', tributacaoRegular.toString());
    if (creditoPresumido !== undefined) params.append('creditoPresumido', creditoPresumido.toString());
    if (descricao) params.append('descricao', descricao);
    if (ordenarPor) params.append('ordenarPor', ordenarPor);

    const response = await api.get<ClassTribPagedResult>(`/classtrib/filtros/avancado?${params}`);
    return response.data;
  },

  /**
   * Lista tipos de alíquota disponíveis
   */
  async getTiposAliquota(): Promise<string[]> {
    const response = await api.get<string[]>('/classtrib/filtros/tipos-aliquota');
    return response.data;
  },

  /**
   * Lista CSTs disponíveis com contagem
   */
  async getCsts(): Promise<Array<{ codigo: string; descricao: string; total: number }>> {
    const response = await api.get<Array<{ codigo: string; descricao: string; total: number }>>('/classtrib/filtros/csts');
    return response.data;
  },

  /**
   * Obtém estatísticas de ClassTrib
   */
  async getEstatisticas(): Promise<any> {
    const response = await api.get('/classtrib/filtros/estatisticas');
    return response.data;
  },

  // ==========================================================================
  // SINCRONIZAÇÃO COM API SVRS
  // ==========================================================================

  /**
   * Sincroniza ClassTribs com API SVRS
   */
  async sincronizar(forcar: boolean = false): Promise<SyncResult> {
    const response = await api.post<ClassTribSyncResult>(`/classtrib/sync?forcar=${forcar}`);
    return response.data;
  },

  /**
   * Obtém status da sincronização
   */
  async getStatusSincronizacao(): Promise<ClassTribSyncStatus> {
    const response = await api.get<ClassTribSyncStatus>('/classtrib/sync/status');
    return response.data;
  },
};

export default classTribService;
