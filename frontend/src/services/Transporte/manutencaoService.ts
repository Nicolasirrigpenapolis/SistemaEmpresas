import api from '../api';
import type {
  ManutencaoVeiculoDto,
  ManutencaoVeiculoCreateDto,
  ManutencaoVeiculoUpdateDto,
  ManutencaoVeiculoListDto,
  ManutencaoFiltros,
  ManutencaoPecaDto,
  ManutencaoPecaCreateDto,
  ManutencaoPecaUpdateDto,
  PagedResult,
} from '../../types/Transporte/transporte';

const BASE_URL = '/transporte/manutencoes';

export const manutencaoService = {
  // ==========================================
  // MANUTENÇÕES
  // ==========================================

  // Listar manutenções com filtros e paginação
  async listar(filtros?: ManutencaoFiltros): Promise<PagedResult<ManutencaoVeiculoListDto>> {
    const params = new URLSearchParams();

    if (filtros?.pageNumber) params.append('pageNumber', filtros.pageNumber.toString());
    if (filtros?.pageSize) params.append('pageSize', filtros.pageSize.toString());
    if (filtros?.busca) params.append('busca', filtros.busca);
    if (filtros?.veiculoId) params.append('veiculoId', filtros.veiculoId.toString());
    if (filtros?.tipoManutencao) params.append('tipoManutencao', filtros.tipoManutencao);
    if (filtros?.dataInicio) params.append('dataInicio', filtros.dataInicio);
    if (filtros?.dataFim) params.append('dataFim', filtros.dataFim);
    if (filtros?.incluirInativos) params.append('incluirInativos', 'true');

    const response = await api.get<PagedResult<ManutencaoVeiculoListDto>>(`${BASE_URL}?${params.toString()}`);
    return response.data;
  },

  // Buscar manutenção por ID (com peças)
  async buscarPorId(id: number): Promise<ManutencaoVeiculoDto> {
    const response = await api.get<ManutencaoVeiculoDto>(`${BASE_URL}/${id}`);
    return response.data;
  },

  // Criar nova manutenção
  async criar(dto: ManutencaoVeiculoCreateDto): Promise<ManutencaoVeiculoDto> {
    const response = await api.post<ManutencaoVeiculoDto>(BASE_URL, dto);
    return response.data;
  },

  // Atualizar manutenção
  async atualizar(id: number, dto: ManutencaoVeiculoUpdateDto): Promise<void> {
    await api.put(`${BASE_URL}/${id}`, dto);
  },

  // Excluir manutenção
  async excluir(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/${id}`);
  },

  // ==========================================
  // PEÇAS
  // ==========================================

  // Listar peças da manutenção
  async listarPecas(manutencaoId: number): Promise<ManutencaoPecaDto[]> {
    const response = await api.get<ManutencaoPecaDto[]>(`${BASE_URL}/${manutencaoId}/pecas`);
    return response.data;
  },

  // Adicionar peça à manutenção
  async adicionarPeca(manutencaoId: number, dto: ManutencaoPecaCreateDto): Promise<ManutencaoPecaDto> {
    const response = await api.post<ManutencaoPecaDto>(`${BASE_URL}/${manutencaoId}/pecas`, dto);
    return response.data;
  },

  // Atualizar peça
  async atualizarPeca(manutencaoId: number, pecaId: number, dto: ManutencaoPecaUpdateDto): Promise<void> {
    await api.put(`${BASE_URL}/${manutencaoId}/pecas/${pecaId}`, dto);
  },

  // Excluir peça
  async excluirPeca(manutencaoId: number, pecaId: number): Promise<void> {
    await api.delete(`${BASE_URL}/${manutencaoId}/pecas/${pecaId}`);
  },
};

export default manutencaoService;
