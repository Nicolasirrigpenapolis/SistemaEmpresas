// ==========================================
// DTOs para Sistema de Permissões por Tela
// Compatível com backend .NET
// ==========================================

// #region Templates de Permissões

export interface PermissoesTemplateListDto {
  id: number;
  nome: string;
  descricao?: string | null;
  isPadrao: boolean;
  dataCriacao: string;
  quantidadeTelas: number;
}

export interface PermissoesTemplateCreateDto {
  nome: string;
  descricao?: string | null;
  detalhes: PermissoesTemplateDetalheDto[];
}

export interface PermissoesTemplateUpdateDto {
  nome: string;
  descricao?: string | null;
  detalhes: PermissoesTemplateDetalheDto[];
}

export interface PermissoesTemplateDetalheDto {
  id?: number | null;
  modulo: string;
  tela: string;
  consultar: boolean;
  incluir: boolean;
  alterar: boolean;
  excluir: boolean;
}

export interface PermissoesTemplateComDetalhesDto {
  id: number;
  nome: string;
  descricao?: string | null;
  isPadrao: boolean;
  dataCriacao: string;
  detalhes: PermissoesTemplateDetalheDto[];
}

// #endregion

// #region Permissões por Tela

export interface PermissoesTelaListDto {
  id: number;
  grupo: string;
  modulo: string;
  tela: string;
  nomeTela: string;
  rota: string;
  consultar: boolean;
  incluir: boolean;
  alterar: boolean;
  excluir: boolean;
  ordem: number;
}

export interface PermissoesTelaCreateUpdateDto {
  grupo: string;
  modulo: string;
  tela: string;
  nomeTela: string;
  rota: string;
  consultar: boolean;
  incluir: boolean;
  alterar: boolean;
  excluir: boolean;
  ordem: number;
}

export interface PermissoesTelasBatchUpdateDto {
  grupo: string;
  permissoes: PermissoesTelaCreateUpdateDto[];
}

export interface AplicarTemplateDto {
  templateId: number;
  grupo: string;
  substituirExistentes: boolean;
}

// #endregion

// #region Agrupamentos e Visualização

export interface ModuloPermissoesDto {
  nome: string;
  icone: string;
  ordem: number;
  telas: PermissoesTelaListDto[];
}

export interface PermissoesCompletasGrupoDto {
  grupo: string;
  nomeGrupo: string;
  isAdmin: boolean;
  modulos: ModuloPermissoesDto[];
}

export interface TelaDisponivelDto {
  modulo: string;
  tela: string;
  nomeTela: string;
  rota: string;
  icone: string;
  ordem: number;
  requirePermissao: boolean;
}

export interface ModuloComTelasDto {
  nome: string;
  icone: string;
  ordem: number;
  telas: TelaDisponivelDto[];
}

// #endregion

// #region Consultas de Permissão

export interface VerificarPermissaoDto {
  tela: string;
  acao: 'consultar' | 'incluir' | 'alterar' | 'excluir';
}

export interface PermissaoResultDto {
  permitido: boolean;
  tela: string;
  acao: string;
  mensagem?: string | null;
}

export interface PermissoesUsuarioLogadoDto {
  usuario: string;
  grupo: string;
  isAdmin: boolean;
  telas: Record<string, PermissaoTelaResumoDto>;
}

export interface PermissaoTelaResumoDto {
  c: boolean; // Consultar
  i: boolean; // Incluir
  a: boolean; // Alterar
  e: boolean; // Excluir
}

// #endregion

// #region Estatísticas

export interface PermissoesEstatisticasDto {
  totalGrupos: number;
  totalUsuarios: number;
  totalUsuariosAtivos: number;
  totalUsuariosInativos: number;
  totalTemplates: number;
  totalTelasConfiguradas: number;
}

// #endregion

// #region Estado do Componente de Permissões

export type PermissoesTabType = 'usuarios' | 'permissoes' | 'templates';

export interface PermissoesTelaState {
  // Dados
  grupos: { nome: string; quantidadeUsuarios: number; isAdmin: boolean }[];
  grupoSelecionado: string | null;
  permissoesGrupo: PermissoesCompletasGrupoDto | null;
  templates: PermissoesTemplateListDto[];
  templateSelecionado: PermissoesTemplateComDetalhesDto | null;
  telasDisponiveis: ModuloComTelasDto[];
  estatisticas: PermissoesEstatisticasDto | null;
  
  // UI
  tabAtiva: PermissoesTabType;
  loading: boolean;
  salvando: boolean;
  
  // Formulários
  modoEdicaoTemplate: boolean;
  templateForm: PermissoesTemplateCreateDto | null;
}

// #endregion
