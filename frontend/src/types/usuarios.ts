// ==========================================
// DTOs para Gerenciamento de Usuários
// Sistema Web - Novas tabelas GrupoUsuario e UsuarioSistema
// ==========================================

// #region Grupos

export interface GrupoListDto {
  nome: string;
  quantidadeUsuarios?: number;
}

export interface GrupoUsuarioListDto {
  id: number;
  nome: string;
  descricao?: string;
  ativo: boolean;
  grupoSistema: boolean;
  quantidadeUsuarios: number;
  dataCriacao: string;
}

export interface GrupoCreateDto {
  nome: string;
  descricao?: string;
}

// #endregion

// #region Usuários

export interface UsuarioListDto {
  nome: string;
  grupo: string;
  observacoes?: string;
  ativo: boolean;
}

export interface UsuarioCreateDto {
  nome: string;
  senha: string;
  confirmarSenha: string;
  grupo: string;
  observacoes?: string;
  ativo?: boolean;
}

export interface UsuarioUpdateDto {
  novaSenha?: string;
  confirmarNovaSenha?: string;
  grupo?: string;
  observacoes?: string;
  ativo?: boolean;
}

export interface MoverUsuarioDto {
  nomeUsuario: string;
  grupoOrigem: string;
  grupoDestino: string;
}

// #endregion

// #region Permissões

export interface PermissaoTabelaDto {
  projeto: string;
  nome: string;
  nomeExibicao: string;
  visualiza: boolean;
  inclui: boolean;
  modifica: boolean;
  exclui: boolean;
  tipo: 'TABELA' | 'MENU';
  modulo?: string | null;
}

export interface AtualizarPermissoesDto {
  grupo: string;
  permissoes: PermissaoTabelaDto[];
}

export interface PermissoesGrupoDto {
  grupo: string;
  isAdmin: boolean;
  tabelas: PermissaoTabelaDto[];
  menus: PermissaoTabelaDto[];
}

export interface CopiarPermissoesDto {
  grupoOrigem: string;
  grupoDestino: string;
}

// #endregion

// #region Estrutura Hierárquica

export interface GrupoComUsuariosDto {
  nome: string;
  isAdmin: boolean;
  expandido: boolean;
  usuarios: UsuarioListDto[];
}

export interface ModuloTabelasDto {
  nome: string;
  icone: string;
  tabelas: PermissaoTabelaDto[];
}

// #endregion

// #region Respostas

export interface OperacaoResultDto {
  sucesso: boolean;
  mensagem: string;
  erros?: string[] | null;
}

// #endregion

// #region Estado do Componente

export type TabType = 'usuario' | 'menus' | 'tabelas' | 'templates';

export interface UsuarioManagementState {
  // Dados
  arvore: GrupoComUsuariosDto[];
  grupoSelecionado: string | null;
  usuarioSelecionado: string | null;
  
  // Permissões
  permissoesGrupo: PermissoesGrupoDto | null;
  tabelasDisponiveis: ModuloTabelasDto[];
  menusDisponiveis: PermissaoTabelaDto[];
  
  // UI
  tabAtiva: TabType;
  modoEdicao: boolean;
  loading: boolean;
  salvando: boolean;
  
  // Formulários
  novoGrupo: string;
  usuarioForm: UsuarioCreateDto | null;
}

// #endregion
