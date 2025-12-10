import api from './api';
import type {
  VeiculoDto,
  VeiculoCreateDto,
  VeiculoUpdateDto,
  VeiculoListDto,
  VeiculoFiltros,
  PagedResult,
} from '../types/transporte';

const BASE_URL = '/transporte/veiculos';

export const veiculoService = {
  // Listar veículos com filtros e paginação
  async listar(filtros?: VeiculoFiltros): Promise<PagedResult<VeiculoListDto>> {
    const params = new URLSearchParams();
    
    if (filtros?.pageNumber) params.append('pageNumber', filtros.pageNumber.toString());
    if (filtros?.pageSize) params.append('pageSize', filtros.pageSize.toString());
    if (filtros?.busca) params.append('busca', filtros.busca);
    if (filtros?.tipoVeiculo) params.append('tipoVeiculo', filtros.tipoVeiculo);
    if (filtros?.incluirInativos) params.append('incluirInativos', 'true');
    
    const response = await api.get<PagedResult<VeiculoListDto>>(`${BASE_URL}?${params.toString()}`);
    return response.data;
  },

  // Buscar veículo por ID
  async buscarPorId(id: number): Promise<VeiculoDto> {
    const response = await api.get<VeiculoDto>(`${BASE_URL}/${id}`);
    return response.data;
  },

  // Buscar veículo por placa
  async buscarPorPlaca(placa: string): Promise<VeiculoDto> {
    const response = await api.get<VeiculoDto>(`${BASE_URL}/placa/${placa}`);
    return response.data;
  },

  // Listar veículos ativos para combo
  async listarAtivos(): Promise<VeiculoListDto[]> {
    const response = await api.get<VeiculoListDto[]>(`${BASE_URL}/ativos`);
    return response.data;
  },

  // Criar novo veículo
  async criar(dto: VeiculoCreateDto): Promise<VeiculoDto> {
    const response = await api.post<VeiculoDto>(BASE_URL, dto);
    return response.data;
  },

  // Atualizar veículo
  async atualizar(id: number, dto: VeiculoUpdateDto): Promise<void> {
    await api.put(`${BASE_URL}/${id}`, dto);
  },

  // Excluir veículo
  async excluir(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/${id}`);
  },
};

export default veiculoService;
