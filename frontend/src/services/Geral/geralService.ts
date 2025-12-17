import api from '../api';
import type {
  GeralListDto,
  GeralCreateDto,
  GeralUpdateDto,
  GeralDetailDto,
  GeralFiltros,
  PagedResult,
} from '../../types/Geral/geral';

// ============================================================================
// TIPOS PARA CONSULTAS CNPJ/CEP
// ============================================================================

export interface CnpjResponse {
  razaoSocial: string;
  nomeFantasia: string;
  logradouro: string;
  numero: string;
  complemento: string;
  bairro: string;
  municipio: string;
  uf: string;
  cep: string;
  telefone: string;
  email: string;
  sequenciaDoMunicipio: number;
}

export interface CepResponse {
  logradouro: string;
  complemento: string;
  bairro: string;
  municipio: string;
  uf: string;
  cep: string;
  sequenciaDoMunicipio: number;
}

// ============================================================================
// SERVIÇO DO CADASTRO GERAL
// ============================================================================

export const geralService = {
  /**
   * Lista registros com paginação e filtros
   */
  async listar(filtros: GeralFiltros = {}): Promise<PagedResult<GeralListDto>> {
    const params = new URLSearchParams();

    if (filtros.pageNumber) params.append('pageNumber', filtros.pageNumber.toString());
    if (filtros.pageSize) params.append('pageSize', filtros.pageSize.toString());
    if (filtros.busca) params.append('busca', filtros.busca);
    if (filtros.cliente !== undefined) params.append('cliente', filtros.cliente.toString());
    if (filtros.fornecedor !== undefined) params.append('fornecedor', filtros.fornecedor.toString());
    if (filtros.transportadora !== undefined) params.append('transportadora', filtros.transportadora.toString());
    if (filtros.vendedor !== undefined) params.append('vendedor', filtros.vendedor.toString());
    if (filtros.incluirInativos) params.append('incluirInativos', 'true');

    const response = await api.get<PagedResult<GeralListDto>>(`/geral?${params.toString()}`);
    return response.data;
  },

  /**
   * Busca por ID
   */
  async buscarPorId(id: number): Promise<GeralDetailDto> {
    const response = await api.get<GeralDetailDto>(`/geral/${id}`);
    return response.data;
  },

  /**
   * Busca simplificada para autocomplete
   */
  async buscar(termo: string, tipo?: string, limit?: number): Promise<GeralListDto[]> {
    const params = new URLSearchParams();
    params.append('termo', termo);
    if (tipo) params.append('tipo', tipo);
    if (limit) params.append('limit', limit.toString());

    const response = await api.get<GeralListDto[]>(`/geral/buscar?${params.toString()}`);
    return response.data;
  },

  /**
   * Lista vendedores para combobox
   */
  async listarVendedores(): Promise<GeralListDto[]> {
    const response = await api.get<GeralListDto[]>('/geral/vendedores');
    return response.data;
  },

  /**
   * Cria novo registro
   */
  async criar(dados: GeralCreateDto): Promise<GeralDetailDto> {
    const response = await api.post<GeralDetailDto>('/geral', dados);
    return response.data;
  },

  /**
   * Atualiza registro existente
   */
  async atualizar(id: number, dados: GeralUpdateDto): Promise<GeralDetailDto> {
    const response = await api.put<GeralDetailDto>(`/geral/${id}`, dados);
    return response.data;
  },

  /**
   * Inativa registro (soft delete)
   */
  async excluir(id: number): Promise<void> {
    await api.delete(`/geral/${id}`);
  },

  /**
   * Consulta dados de empresa pelo CNPJ
   */
  async consultarCnpj(cnpj: string): Promise<CnpjResponse> {
    const response = await api.get<CnpjResponse>(`/geral/consulta-cnpj/${cnpj}`);
    return response.data;
  },

  /**
   * Consulta endereço pelo CEP
   */
  async consultarCep(cep: string): Promise<CepResponse> {
    const response = await api.get<CepResponse>(`/geral/consulta-cep/${cep}`);
    return response.data;
  },
};

export default geralService;
