import api from './api';

// Types
export interface LogAuditoriaListDto {
  id: number;
  dataHora: string;
  usuarioCodigo: number;
  usuarioNome: string;
  usuarioGrupo: string;
  tipoAcao: string;
  modulo: string;
  entidade: string;
  entidadeId?: string;
  descricao: string;
  enderecoIP?: string;
  erro: boolean;
  tenantNome?: string;
  tipoAcaoLabel?: string;
  tipoAcaoCor?: string;
  tipoAcaoIcone?: string;
}

export interface LogAuditoriaDetalheDto extends LogAuditoriaListDto {
  dadosAnteriores?: string;
  dadosNovos?: string;
  camposAlterados?: string;
  userAgent?: string;
  metodoHttp?: string;
  urlRequisicao?: string;
  statusCode?: number;
  tempoExecucaoMs?: number;
  mensagemErro?: string;
  tenantId?: number;
  sessaoId?: string;
  correlationId?: string;
}

export interface LogAuditoriaFiltroDto {
  dataInicio?: string;
  dataFim?: string;
  usuarioCodigo?: number;
  usuarioNome?: string;
  tipoAcao?: string;
  tiposAcao?: string[];
  modulo?: string;
  entidade?: string;
  entidadeId?: string;
  busca?: string;
  apenasErros?: boolean;
  pagina?: number;
  itensPorPagina?: number;
  ordenarPor?: string;
  ordemDescrescente?: boolean;
}

export interface LogAuditoriaPagedResult {
  items: LogAuditoriaListDto[];
  totalItems: number;
  paginaAtual: number;
  itensPorPagina: number;
  totalPaginas: number;
}

export interface LogAuditoriaEstatisticasDto {
  dataInicio: string;
  dataFim: string;
  totalAcoes: number;
  usuariosAtivos: number;
  totalErros: number;
  acoesPorTipo: { tipoAcao: string; quantidade: number }[];
  acoesPorModulo: { modulo: string; quantidade: number }[];
  topUsuarios: { usuarioNome: string; quantidade: number }[];
  acoesPorDia: { data: string; quantidade: number }[];
}

export interface TipoAcaoInfo {
  valor: string;
  label: string;
  cor: string;
  icone: string;
}

export interface ModuloInfo {
  valor: string;
  label: string;
}

// Service
const logsService = {
  /**
   * Lista logs com paginação e filtros
   */
  async getLogs(filtro: LogAuditoriaFiltroDto): Promise<LogAuditoriaPagedResult> {
    const params = new URLSearchParams();
    
    if (filtro.dataInicio) params.append('dataInicio', filtro.dataInicio);
    if (filtro.dataFim) params.append('dataFim', filtro.dataFim);
    if (filtro.usuarioCodigo) params.append('usuarioCodigo', filtro.usuarioCodigo.toString());
    if (filtro.usuarioNome) params.append('usuarioNome', filtro.usuarioNome);
    if (filtro.tipoAcao) params.append('tipoAcao', filtro.tipoAcao);
    if (filtro.modulo) params.append('modulo', filtro.modulo);
    if (filtro.entidade) params.append('entidade', filtro.entidade);
    if (filtro.entidadeId) params.append('entidadeId', filtro.entidadeId);
    if (filtro.busca) params.append('busca', filtro.busca);
    if (filtro.apenasErros) params.append('apenasErros', 'true');
    if (filtro.pagina) params.append('pagina', filtro.pagina.toString());
    if (filtro.itensPorPagina) params.append('itensPorPagina', filtro.itensPorPagina.toString());
    if (filtro.ordenarPor) params.append('ordenarPor', filtro.ordenarPor);
    if (filtro.ordemDescrescente !== undefined) params.append('ordemDescrescente', filtro.ordemDescrescente.toString());

    const response = await api.get<LogAuditoriaPagedResult>(`/logs?${params.toString()}`);
    return response.data;
  },

  /**
   * Obtém detalhes de um log específico
   */
  async getLogDetalhe(id: number): Promise<LogAuditoriaDetalheDto> {
    const response = await api.get<LogAuditoriaDetalheDto>(`/logs/${id}`);
    return response.data;
  },

  /**
   * Obtém estatísticas dos logs
   */
  async getEstatisticas(dataInicio?: string, dataFim?: string): Promise<LogAuditoriaEstatisticasDto> {
    const params = new URLSearchParams();
    if (dataInicio) params.append('dataInicio', dataInicio);
    if (dataFim) params.append('dataFim', dataFim);

    const response = await api.get<LogAuditoriaEstatisticasDto>(`/logs/estatisticas?${params.toString()}`);
    return response.data;
  },

  /**
   * Busca logs de uma entidade específica
   */
  async getLogsByEntidade(entidade: string, entidadeId: string): Promise<LogAuditoriaListDto[]> {
    const response = await api.get<LogAuditoriaListDto[]>(`/logs/entidade/${entidade}/${entidadeId}`);
    return response.data;
  },

  /**
   * Busca logs de um usuário específico
   */
  async getLogsByUsuario(usuarioCodigo: number, limite: number = 100): Promise<LogAuditoriaListDto[]> {
    const response = await api.get<LogAuditoriaListDto[]>(`/logs/usuario/${usuarioCodigo}?limite=${limite}`);
    return response.data;
  },

  /**
   * Obtém tipos de ação disponíveis
   */
  async getTiposAcao(): Promise<TipoAcaoInfo[]> {
    const response = await api.get<TipoAcaoInfo[]>('/logs/tipos-acao');
    return response.data;
  },

  /**
   * Obtém módulos disponíveis
   */
  async getModulos(): Promise<ModuloInfo[]> {
    const response = await api.get<ModuloInfo[]>('/logs/modulos');
    return response.data;
  },

  /**
   * Obtém atividade do usuário logado
   */
  async getMinhaAtividade(limite: number = 50): Promise<LogAuditoriaListDto[]> {
    const response = await api.get<LogAuditoriaListDto[]>(`/logs/minha-atividade?limite=${limite}`);
    return response.data;
  },

  /**
   * Limpa logs antigos (apenas admin)
   */
  async limparLogsAntigos(diasParaManter: number = 90): Promise<{ sucesso: boolean; mensagem: string; registrosExcluidos: number }> {
    const response = await api.delete(`/logs/limpar?diasParaManter=${diasParaManter}`);
    return response.data;
  }
};

export default logsService;
