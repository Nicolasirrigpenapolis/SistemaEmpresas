import api from './api';

// ==========================================
// TIPOS PARA A NOVA API DE GRUPOS/USUÁRIOS
// ==========================================

export interface GrupoUsuarioListDto {
  id: number;
  nome: string;
  descricao?: string;
  ativo: boolean;
  grupoSistema: boolean;
  quantidadeUsuarios: number;
  dataCriacao: string;
}

export interface GrupoUsuarioCreateDto {
  nome: string;
  descricao?: string;
  ativo?: boolean;
}

export interface GrupoUsuarioUpdateDto {
  nome?: string;
  descricao?: string;
  ativo?: boolean;
}

export interface UsuarioSistemaListDto {
  id: number;
  login: string;
  nomeCompleto: string;
  email?: string;
  grupoId: number;
  grupoNome: string;
  observacoes?: string;
  ativo: boolean;
  dataCriacao: string;
  ultimoLogin?: string;
  deveTrocarSenha: boolean;
}

export interface UsuarioSistemaCreateDto {
  login: string;
  nomeCompleto: string;
  email?: string;
  senha: string;
  confirmarSenha: string;
  grupoId: number;
  observacoes?: string;
  ativo?: boolean;
}

export interface UsuarioSistemaUpdateDto {
  nomeCompleto?: string;
  email?: string;
  grupoId?: number;
  observacoes?: string;
  ativo?: boolean;
  novaSenha?: string;
  confirmarNovaSenha?: string;
}

export interface GrupoComUsuariosSistemaDto {
  id: number;
  nome: string;
  descricao?: string;
  ativo: boolean;
  grupoSistema: boolean;
  usuarios: UsuarioSistemaListDto[];
}

export interface RedefinirSenhaDto {
  novaSenha: string;
  confirmarNovaSenha: string;
}

// Tipos de compatibilidade com a interface antiga
export interface GrupoListDto {
  nome: string;
  quantidadeUsuarios?: number;
}

export interface UsuarioListDto {
  nome: string;
  grupo: string;
  observacoes?: string;
  ativo: boolean;
}

export interface GrupoComUsuariosDto {
  nome: string;
  isAdmin: boolean;
  expandido: boolean;
  usuarios: UsuarioListDto[];
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
  grupo?: string;
  observacoes?: string;
  ativo?: boolean;
  novaSenha?: string;
  confirmarNovaSenha?: string;
}

export interface OperacaoResultDto {
  sucesso: boolean;
  mensagem: string;
}

// Tipos para permissões
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

export interface ModuloTabelasDto {
  nome: string;
  icone: string;
  tabelas: PermissaoTabelaDto[];
}

export interface PermissoesGrupoDto {
  grupo: string;
  isAdmin: boolean;
  tabelas: PermissaoTabelaDto[];
  menus: PermissaoTabelaDto[];
}

export interface AtualizarPermissoesDto {
  grupo: string;
  permissoes: PermissaoTabelaDto[];
}

// URLs das APIs
const GRUPOS_URL = '/grupousuario';  // API de grupos e relacionamentos
const USUARIOS_URL = '/usuarios';     // API de CRUD de usuários

/**
 * Service para gerenciamento de usuários e grupos do sistema web
 * Usa as novas tabelas GrupoUsuario e UsuarioSistema
 */
class UsuarioManagementService {
  // Cache interno de grupos para conversão nome <-> id
  private gruposCache: GrupoUsuarioListDto[] = [];

  private async carregarGruposCache(): Promise<void> {
    if (this.gruposCache.length === 0) {
      const response = await api.get<GrupoUsuarioListDto[]>(`${GRUPOS_URL}/grupos`);
      this.gruposCache = response.data;
    }
  }

  private async obterGrupoIdPorNome(nome: string): Promise<number | null> {
    await this.carregarGruposCache();
    const grupo = this.gruposCache.find(g => g.nome.toLowerCase() === nome.toLowerCase());
    return grupo?.id ?? null;
  }

  // ==========================================
  // GRUPOS
  // ==========================================

  /**
   * Lista todos os grupos (compatível com interface antiga)
   */
  async listarGrupos(): Promise<GrupoListDto[]> {
    const response = await api.get<GrupoUsuarioListDto[]>(`${GRUPOS_URL}/grupos`);
    this.gruposCache = response.data;
    
    // Converte para formato antigo
    return response.data.map(g => ({
      nome: g.nome,
      quantidadeUsuarios: g.quantidadeUsuarios
    }));
  }

  /**
   * Lista todos os grupos (formato novo completo)
   */
  async listarGruposCompleto(): Promise<GrupoUsuarioListDto[]> {
    console.log('Chamando listarGruposCompleto (NOVO SERVICE)');
    const response = await api.get<GrupoUsuarioListDto[]>(`${GRUPOS_URL}/grupos`);
    this.gruposCache = response.data;
    return response.data;
  }

  /**
   * Cria um novo grupo
   */
  async criarGrupo(dto: { nome: string; descricao?: string }): Promise<OperacaoResultDto> {
    const novoGrupo: GrupoUsuarioCreateDto = {
      nome: dto.nome,
      descricao: dto.descricao,
      ativo: true
    };
    
    const response = await api.post<GrupoUsuarioListDto>(`${GRUPOS_URL}/grupos`, novoGrupo);
    this.gruposCache = []; // Limpa cache
    return { sucesso: true, mensagem: `Grupo "${response.data.nome}" criado com sucesso!` };
  }

  /**
   * Atualiza um grupo
   */
  async atualizarGrupo(id: number, dto: GrupoUsuarioUpdateDto): Promise<OperacaoResultDto> {
    await api.put(`${GRUPOS_URL}/grupos/${id}`, dto);
    this.gruposCache = []; // Limpa cache
    return { sucesso: true, mensagem: 'Grupo atualizado com sucesso!' };
  }

  /**
   * Exclui um grupo por nome (compatível com interface antiga)
   */
  async excluirGrupo(nome: string): Promise<OperacaoResultDto> {
    const id = await this.obterGrupoIdPorNome(nome);
    if (!id) {
      return { sucesso: false, mensagem: `Grupo "${nome}" não encontrado.` };
    }
    
    await api.delete(`${GRUPOS_URL}/grupos/${id}`);
    this.gruposCache = []; // Limpa cache
    return { sucesso: true, mensagem: `Grupo "${nome}" excluído com sucesso!` };
  }

  /**
   * Exclui um grupo por ID
   */
  async excluirGrupoPorId(id: number): Promise<OperacaoResultDto> {
    await api.delete(`${GRUPOS_URL}/grupos/${id}`);
    this.gruposCache = []; // Limpa cache
    return { sucesso: true, mensagem: 'Grupo excluído com sucesso!' };
  }

  // ==========================================
  // USUÁRIOS
  // ==========================================

  /**
   * Lista todos os grupos com seus usuários (árvore) - compatível com interface antiga
   */
  async listarArvore(): Promise<GrupoComUsuariosDto[]> {
    const response = await api.get<GrupoComUsuariosDto[]>(`${GRUPOS_URL}/arvore`);
    
    // A API já retorna no formato correto com 'nome' nos usuários
    return response.data.map(g => ({
      nome: g.nome,
      isAdmin: g.isAdmin || false,
      expandido: g.expandido || false,
      usuarios: g.usuarios.map(u => ({
        nome: u.nome || '',
        grupo: g.nome,
        observacoes: u.observacoes,
        ativo: u.ativo
      }))
    }));
  }

  /**
   * Lista todos os grupos com seus usuários (formato novo completo)
   */
  async listarArvoreCompleta(): Promise<GrupoComUsuariosSistemaDto[]> {
    const response = await api.get<GrupoComUsuariosSistemaDto[]>(`${GRUPOS_URL}/arvore`);
    return response.data;
  }

  /**
   * Lista todos os usuários
   */
  async listarUsuarios(): Promise<UsuarioSistemaListDto[]> {
    const response = await api.get<UsuarioSistemaListDto[]>(`${GRUPOS_URL}/usuarios`);
    return response.data;
  }

  /**
   * Cria um novo usuário (compatível com interface antiga)
   * IMPORTANTE: A API /api/usuarios espera UsuarioCreateDto com campos:
   * - Nome (string)
   * - Senha (string) 
   * - ConfirmarSenha (string)
   * - Grupo (string - nome do grupo)
   * - Observacoes (string opcional)
   * - Ativo (boolean)
   */
  async criarUsuario(dto: UsuarioCreateDto): Promise<OperacaoResultDto> {
    // Enviar diretamente no formato esperado pelo backend
    const dadosParaEnviar = {
      nome: dto.nome,
      senha: dto.senha,
      confirmarSenha: dto.confirmarSenha,
      grupo: dto.grupo,
      observacoes: dto.observacoes,
      ativo: dto.ativo ?? true
    };
    
    const response = await api.post<OperacaoResultDto>(`${USUARIOS_URL}`, dadosParaEnviar);
    return response.data;
  }

  /**
   * Cria um novo usuário (formato novo)
   */
  async criarUsuarioNovo(dto: UsuarioSistemaCreateDto): Promise<UsuarioSistemaListDto> {
    const response = await api.post<UsuarioSistemaListDto>(`${USUARIOS_URL}`, dto);
    return response.data;
  }

  /**
   * Atualiza um usuário por nome (compatível com interface antiga)
   * Backend espera PUT /api/usuarios/{nome} com UsuarioUpdateDto
   */
  async atualizarUsuario(nome: string, dto: UsuarioUpdateDto): Promise<OperacaoResultDto> {
    try {
      // Chamar diretamente o endpoint com o nome
      const response = await api.put<OperacaoResultDto>(`${USUARIOS_URL}/${encodeURIComponent(nome)}`, dto);
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.mensagem || error.response?.data?.message || 'Erro ao atualizar usuário';
      return { sucesso: false, mensagem };
    }
  }

  /**
   * Atualiza um usuário por ID (formato novo)
   */
  async atualizarUsuarioPorId(id: number, dto: UsuarioSistemaUpdateDto): Promise<OperacaoResultDto> {
    await api.put(`${USUARIOS_URL}/${id}`, dto);
    return { sucesso: true, mensagem: 'Usuário atualizado com sucesso!' };
  }

  /**
   * Exclui um usuário por nome (compatível com interface antiga)
   * O backend espera DELETE /api/usuarios/{nome}
   */
  async excluirUsuario(nome: string): Promise<OperacaoResultDto> {
    try {
      // Codifica o nome para uso na URL (caracteres especiais)
      const nomeEncoded = encodeURIComponent(nome);
      await api.delete(`${USUARIOS_URL}/${nomeEncoded}`);
      return { sucesso: true, mensagem: `Usuário "${nome}" excluído com sucesso!` };
    } catch (error: any) {
      console.error('Erro ao excluir usuário:', error);
      const mensagem = error.response?.data?.message || error.message || 'Erro ao excluir usuário';
      return { sucesso: false, mensagem };
    }
  }

  /**
   * Exclui um usuário por ID ou Nome
   * Para o sistema legado, usamos o nome como identificador
   */
  async excluirUsuarioPorId(idOuNome: number | string): Promise<OperacaoResultDto> {
    try {
      const identificador = encodeURIComponent(String(idOuNome));
      await api.delete(`${USUARIOS_URL}/${identificador}`);
      return { sucesso: true, mensagem: 'Usuário excluído com sucesso!' };
    } catch (error: any) {
      console.error('Erro ao excluir usuário:', error);
      const mensagem = error.response?.data?.message || error.message || 'Erro ao excluir usuário';
      return { sucesso: false, mensagem };
    }
  }

  /**
   * Move um usuário para outro grupo
   */
  async moverUsuario(dto: { usuario: string; grupoDestino: string }): Promise<OperacaoResultDto> {
    const usuarios = await this.listarUsuarios();
    const usuario = usuarios.find(u => u.login.toLowerCase() === dto.usuario.toLowerCase());
    
    if (!usuario) {
      return { sucesso: false, mensagem: `Usuário "${dto.usuario}" não encontrado.` };
    }

    const grupoId = await this.obterGrupoIdPorNome(dto.grupoDestino);
    if (!grupoId) {
      return { sucesso: false, mensagem: `Grupo "${dto.grupoDestino}" não encontrado.` };
    }
    
    await api.put(`${USUARIOS_URL}/${usuario.id}/mover/${grupoId}`);
    return { sucesso: true, mensagem: `Usuário movido para "${dto.grupoDestino}" com sucesso!` };
  }

  /**
   * Redefine a senha de um usuário
   */
  async redefinirSenha(usuarioId: number, dto: RedefinirSenhaDto): Promise<OperacaoResultDto> {
    await api.put(`${USUARIOS_URL}/${usuarioId}/redefinir-senha`, dto);
    return { sucesso: true, mensagem: 'Senha redefinida com sucesso!' };
  }

  /**
   * Redefine a senha de um usuário por nome (compatível)
   */
  async redefinirSenhaPorNome(nome: string, novaSenha: string, confirmarSenha: string): Promise<OperacaoResultDto> {
    const usuarios = await this.listarUsuarios();
    const usuario = usuarios.find(u => u.login.toLowerCase() === nome.toLowerCase());
    
    if (!usuario) {
      return { sucesso: false, mensagem: `Usuário "${nome}" não encontrado.` };
    }
    
    await api.put(`${USUARIOS_URL}/${usuario.id}/redefinir-senha`, {
      novaSenha,
      confirmarNovaSenha: confirmarSenha
    });
    return { sucesso: true, mensagem: 'Senha redefinida com sucesso!' };
  }

  // ==========================================
  // MÉTODOS DE PERMISSÕES
  // ==========================================

  /**
   * Lista todas as tabelas disponíveis organizadas por módulo
   */
  async listarTabelasDisponiveis(): Promise<ModuloTabelasDto[]> {
    const response = await api.get<ModuloTabelasDto[]>(`${USUARIOS_URL}/tabelas-disponiveis`);
    return response.data;
  }

  /**
   * Lista todos os menus disponíveis
   */
  async listarMenusDisponiveis(): Promise<PermissaoTabelaDto[]> {
    const response = await api.get<PermissaoTabelaDto[]>(`${USUARIOS_URL}/menus-disponiveis`);
    return response.data;
  }

  /**
   * Obtém as permissões de um grupo específico
   */
  async obterPermissoesGrupo(grupo: string): Promise<PermissoesGrupoDto> {
    const response = await api.get<PermissoesGrupoDto>(`${USUARIOS_URL}/permissoes/${encodeURIComponent(grupo)}`);
    return response.data;
  }

  /**
   * Atualiza as permissões de um grupo
   */
  async atualizarPermissoes(dto: AtualizarPermissoesDto): Promise<OperacaoResultDto> {
    try {
      const response = await api.put<OperacaoResultDto>(`${USUARIOS_URL}/permissoes`, dto);
      return response.data;
    } catch (error: any) {
      const mensagem = error.response?.data?.mensagem || error.response?.data?.message || 'Erro ao atualizar permissões';
      return { sucesso: false, mensagem };
    }
  }
}

// Exporta instância única
export const usuarioManagementService = new UsuarioManagementService();
export default usuarioManagementService;
