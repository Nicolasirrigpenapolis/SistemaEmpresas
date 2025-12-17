import api from '../api';
import type {
  MotoristaDto,
  MotoristaListDto,
  MotoristaCreateDto,
  MotoristaUpdateDto,
  MotoristaFiltros,
  PagedResult,
} from '../../types/Transporte/transporte';

const BASE_URL = '/transporte/motoristas';

export const motoristaService = {
  // Lista com paginação e filtros
  async listar(filtros?: MotoristaFiltros): Promise<PagedResult<MotoristaListDto>> {
    const params = new URLSearchParams();
    if (filtros?.busca) params.append('busca', filtros.busca);
    if (filtros?.uf) params.append('uf', filtros.uf);
    if (filtros?.pagina) params.append('pagina', filtros.pagina.toString());
    if (filtros?.tamanhoPagina) params.append('tamanhoPagina', filtros.tamanhoPagina.toString());

    const response = await api.get(`${BASE_URL}?${params.toString()}`);
    return response.data;
  },

  // Lista todos ativos (para combos)
  async listarAtivos(): Promise<MotoristaListDto[]> {
    const response = await api.get(`${BASE_URL}/ativos`);
    return response.data;
  },

  // Busca por ID
  async buscarPorId(id: number): Promise<MotoristaDto> {
    const response = await api.get(`${BASE_URL}/${id}`);
    return response.data;
  },

  // Criar
  async criar(dto: MotoristaCreateDto): Promise<MotoristaDto> {
    const response = await api.post(BASE_URL, dto);
    return response.data;
  },

  // Atualizar
  async atualizar(id: number, dto: MotoristaUpdateDto): Promise<MotoristaDto> {
    const response = await api.put(`${BASE_URL}/${id}`, dto);
    return response.data;
  },

  // Excluir
  async excluir(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/${id}`);
  },
};

export default motoristaService;
