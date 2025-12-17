import api from '../api';
import type {
  ReboqueDto,
  ReboqueCreateDto,
  ReboqueUpdateDto,
  ReboqueListDto,
  ReboqueFiltros,
  PagedResult,
} from '../../types/Transporte/transporte';

const BASE_URL = '/transporte/reboques';

export const reboqueService = {
  // Listar reboques com filtros e paginação
  async listar(filtros?: ReboqueFiltros): Promise<PagedResult<ReboqueListDto>> {
    const params = new URLSearchParams();

    if (filtros?.pageNumber) params.append('pageNumber', filtros.pageNumber.toString());
    if (filtros?.pageSize) params.append('pageSize', filtros.pageSize.toString());
    if (filtros?.busca) params.append('busca', filtros.busca);
    if (filtros?.tipoCarroceria) params.append('tipoCarroceria', filtros.tipoCarroceria);
    if (filtros?.incluirInativos) params.append('incluirInativos', 'true');

    const response = await api.get<PagedResult<ReboqueListDto>>(`${BASE_URL}?${params.toString()}`);
    return response.data;
  },

  // Buscar reboque por ID
  async buscarPorId(id: number): Promise<ReboqueDto> {
    const response = await api.get<ReboqueDto>(`${BASE_URL}/${id}`);
    return response.data;
  },

  // Buscar reboque por placa
  async buscarPorPlaca(placa: string): Promise<ReboqueDto> {
    const response = await api.get<ReboqueDto>(`${BASE_URL}/placa/${placa}`);
    return response.data;
  },

  // Listar reboques ativos para combo
  async listarAtivos(): Promise<ReboqueListDto[]> {
    const response = await api.get<ReboqueListDto[]>(`${BASE_URL}/ativos`);
    return response.data;
  },

  // Criar novo reboque
  async criar(dto: ReboqueCreateDto): Promise<ReboqueDto> {
    const response = await api.post<ReboqueDto>(BASE_URL, dto);
    return response.data;
  },

  // Atualizar reboque
  async atualizar(id: number, dto: ReboqueUpdateDto): Promise<void> {
    await api.put(`${BASE_URL}/${id}`, dto);
  },

  // Excluir reboque
  async excluir(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/${id}`);
  },
};

export default reboqueService;
