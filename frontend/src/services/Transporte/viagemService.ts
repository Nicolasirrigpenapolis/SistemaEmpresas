import api from '../api';
import type {
  ViagemDto,
  ViagemCreateDto,
  ViagemUpdateDto,
  ViagemListDto,
  ViagemFiltros,
  DespesaViagemDto,
  DespesaViagemCreateDto,
  DespesaViagemUpdateDto,
  ReceitaViagemDto,
  ReceitaViagemCreateDto,
  ReceitaViagemUpdateDto,
  PagedResult,
} from '../../types/Transporte/transporte';

const BASE_URL = '/transporte/viagens';

export const viagemService = {
  // ==========================================
  // VIAGENS
  // ==========================================

  // Listar viagens com filtros e paginação
  async listar(filtros?: ViagemFiltros): Promise<PagedResult<ViagemListDto>> {
    const params = new URLSearchParams();

    if (filtros?.pageNumber) params.append('pageNumber', filtros.pageNumber.toString());
    if (filtros?.pageSize) params.append('pageSize', filtros.pageSize.toString());
    if (filtros?.busca) params.append('busca', filtros.busca);
    if (filtros?.veiculoId) params.append('veiculoId', filtros.veiculoId.toString());
    if (filtros?.motoristaId) params.append('motoristaId', filtros.motoristaId.toString());
    if (filtros?.status) params.append('status', filtros.status);
    if (filtros?.dataInicio) params.append('dataInicio', filtros.dataInicio);
    if (filtros?.dataFim) params.append('dataFim', filtros.dataFim);
    if (filtros?.incluirInativos) params.append('incluirInativos', 'true');

    const response = await api.get<PagedResult<ViagemListDto>>(`${BASE_URL}?${params.toString()}`);
    return response.data;
  },

  // Buscar viagem por ID (com despesas e receitas)
  async buscarPorId(id: number): Promise<ViagemDto> {
    const response = await api.get<ViagemDto>(`${BASE_URL}/${id}`);
    return response.data;
  },

  // Criar nova viagem
  async criar(dto: ViagemCreateDto): Promise<ViagemDto> {
    const response = await api.post<ViagemDto>(BASE_URL, dto);
    return response.data;
  },

  // Atualizar viagem
  async atualizar(id: number, dto: ViagemUpdateDto): Promise<void> {
    await api.put(`${BASE_URL}/${id}`, dto);
  },

  // Excluir viagem
  async excluir(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/${id}`);
  },

  // ==========================================
  // DESPESAS
  // ==========================================

  // Listar despesas da viagem
  async listarDespesas(viagemId: number): Promise<DespesaViagemDto[]> {
    const response = await api.get<DespesaViagemDto[]>(`${BASE_URL}/${viagemId}/despesas`);
    return response.data;
  },

  // Adicionar despesa à viagem
  async adicionarDespesa(viagemId: number, dto: DespesaViagemCreateDto): Promise<DespesaViagemDto> {
    const response = await api.post<DespesaViagemDto>(`${BASE_URL}/${viagemId}/despesas`, dto);
    return response.data;
  },

  // Atualizar despesa
  async atualizarDespesa(viagemId: number, despesaId: number, dto: DespesaViagemUpdateDto): Promise<void> {
    await api.put(`${BASE_URL}/${viagemId}/despesas/${despesaId}`, dto);
  },

  // Excluir despesa
  async excluirDespesa(viagemId: number, despesaId: number): Promise<void> {
    await api.delete(`${BASE_URL}/${viagemId}/despesas/${despesaId}`);
  },

  // ==========================================
  // RECEITAS
  // ==========================================

  // Listar receitas da viagem
  async listarReceitas(viagemId: number): Promise<ReceitaViagemDto[]> {
    const response = await api.get<ReceitaViagemDto[]>(`${BASE_URL}/${viagemId}/receitas`);
    return response.data;
  },

  // Adicionar receita à viagem
  async adicionarReceita(viagemId: number, dto: ReceitaViagemCreateDto): Promise<ReceitaViagemDto> {
    const response = await api.post<ReceitaViagemDto>(`${BASE_URL}/${viagemId}/receitas`, dto);
    return response.data;
  },

  // Atualizar receita
  async atualizarReceita(viagemId: number, receitaId: number, dto: ReceitaViagemUpdateDto): Promise<void> {
    await api.put(`${BASE_URL}/${viagemId}/receitas/${receitaId}`, dto);
  },

  // Excluir receita
  async excluirReceita(viagemId: number, receitaId: number): Promise<void> {
    await api.delete(`${BASE_URL}/${viagemId}/receitas/${receitaId}`);
  },
};

export default viagemService;
