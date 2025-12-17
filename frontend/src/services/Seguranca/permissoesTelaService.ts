import api from '../api';
import type {
  PermissoesTemplateListDto,
  PermissoesTemplateCreateDto,
  PermissoesTemplateUpdateDto,
  PermissoesTemplateComDetalhesDto,
  PermissoesCompletasGrupoDto,
  PermissoesTelasBatchUpdateDto,
  AplicarTemplateDto,
  PermissoesUsuarioLogadoDto,
  VerificarPermissaoDto,
  PermissaoResultDto,
  ModuloComTelasDto,
  PermissoesEstatisticasDto,
  OperacaoResultDto,
} from '../../types';

const BASE_URL = '/permissoes';

/**
 * Service para gerenciamento de permissões por tela
 */
class PermissoesTelaService {
  // ==========================================
  // TEMPLATES
  // ==========================================

  /**
   * Lista todos os templates de permissões
   */
  async listarTemplates(): Promise<PermissoesTemplateListDto[]> {
    const response = await api.get<PermissoesTemplateListDto[]>(`${BASE_URL}/templates`);
    return response.data;
  }

  /**
   * Obtém um template específico com seus detalhes
   */
  async obterTemplate(id: number): Promise<PermissoesTemplateComDetalhesDto> {
    const response = await api.get<PermissoesTemplateComDetalhesDto>(`${BASE_URL}/templates/${id}`);
    return response.data;
  }

  /**
   * Cria um novo template
   */
  async criarTemplate(dto: PermissoesTemplateCreateDto): Promise<OperacaoResultDto> {
    const response = await api.post<OperacaoResultDto>(`${BASE_URL}/templates`, dto);
    return response.data;
  }

  /**
   * Atualiza um template existente
   */
  async atualizarTemplate(id: number, dto: PermissoesTemplateUpdateDto): Promise<OperacaoResultDto> {
    const response = await api.put<OperacaoResultDto>(`${BASE_URL}/templates/${id}`, dto);
    return response.data;
  }

  /**
   * Exclui um template
   */
  async excluirTemplate(id: number): Promise<OperacaoResultDto> {
    const response = await api.delete<OperacaoResultDto>(`${BASE_URL}/templates/${id}`);
    return response.data;
  }

  // ==========================================
  // PERMISSÕES POR GRUPO
  // ==========================================

  /**
   * Obtém as permissões completas de um grupo
   */
  async obterPermissoesGrupo(grupo: string): Promise<PermissoesCompletasGrupoDto> {
    const response = await api.get<PermissoesCompletasGrupoDto>(`${BASE_URL}/grupo/${encodeURIComponent(grupo)}`);
    return response.data;
  }

  /**
   * Salva as permissões de um grupo (em lote)
   */
  async salvarPermissoes(dto: PermissoesTelasBatchUpdateDto): Promise<OperacaoResultDto> {
    const response = await api.post<OperacaoResultDto>(`${BASE_URL}/grupo`, dto);
    return response.data;
  }

  /**
   * Aplica um template a um grupo
   */
  async aplicarTemplate(dto: AplicarTemplateDto): Promise<OperacaoResultDto> {
    const response = await api.post<OperacaoResultDto>(`${BASE_URL}/aplicar-template`, dto);
    return response.data;
  }

  // ==========================================
  // MINHAS PERMISSÕES (USUÁRIO LOGADO)
  // ==========================================

  /**
   * Obtém as permissões do usuário logado
   */
  async obterMinhasPermissoes(): Promise<PermissoesUsuarioLogadoDto> {
    const response = await api.get<PermissoesUsuarioLogadoDto>(`${BASE_URL}/minhas`);
    return response.data;
  }

  /**
   * Verifica se o usuário logado tem permissão em uma tela específica
   */
  async verificarPermissao(dto: VerificarPermissaoDto): Promise<PermissaoResultDto> {
    const response = await api.post<PermissaoResultDto>(`${BASE_URL}/verificar`, dto);
    return response.data;
  }

  // ==========================================
  // TELAS DISPONÍVEIS
  // ==========================================

  /**
   * Lista todas as telas disponíveis no sistema (agrupadas por módulo)
   */
  async listarTelasDisponiveis(): Promise<ModuloComTelasDto[]> {
    const response = await api.get<ModuloComTelasDto[]>(`${BASE_URL}/telas`);
    return response.data;
  }

  // ==========================================
  // ESTATÍSTICAS
  // ==========================================

  /**
   * Obtém estatísticas de permissões
   */
  async obterEstatisticas(): Promise<PermissoesEstatisticasDto> {
    const response = await api.get<PermissoesEstatisticasDto>(`${BASE_URL}/estatisticas`);
    return response.data;
  }

  // ==========================================
  // SEEDS
  // ==========================================

  /**
   * Cria os templates padrão do sistema (apenas admin)
   */
  async criarTemplatesPadrao(): Promise<OperacaoResultDto> {
    const response = await api.post<OperacaoResultDto>(`${BASE_URL}/seeds/templates`);
    return response.data;
  }
}

export default new PermissoesTelaService();
