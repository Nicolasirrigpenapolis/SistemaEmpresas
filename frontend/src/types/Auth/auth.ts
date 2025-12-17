// ==========================================
// AUTENTICAÇÃO - Corresponde aos DTOs do Backend
// ==========================================

export interface LoginRequestDto {
  usuario: string;
  senha: string;
  dominioTenant: string;
}

export interface UsuarioDto {
  codigo: number;
  nome: string;
  grupo: string;
  email?: string;
  ativo: boolean;
  permissoes?: PermissaoDto[];
}

export interface PermissaoDto {
  projeto: string;
  tabela: string;
  permissoes: string;
}

export interface TenantDto {
  id: number;
  dominio: string;
  nomeFantasia: string;
  ativo: boolean;
}

export interface LoginResponseDto {
  token: string;
  refreshToken: string;
  usuario: UsuarioDto;
  tenant: TenantDto;
  expiracao: string;
}

export interface AlterarSenhaDto {
  senhaAtual: string;
  senhaNova: string;
  confirmarSenha: string;
}

// ==========================================
// CONTEXTO DE AUTENTICAÇÃO
// ==========================================

export interface User {
  codigo: number;
  nome: string;
  grupo: string;
  email?: string;
  ativo: boolean;
  permissoes?: PermissaoDto[];
}

export interface Tenant {
  id: number;
  dominio: string;
  nomeFantasia: string;
  ativo: boolean;
}

export interface AuthContextType {
  user: User | null;
  tenant: Tenant | null;
  token: string | null;
  login: (dominioTenant: string, usuario: string, senha: string) => Promise<void>;
  logout: () => Promise<void>;
  refreshToken: () => Promise<void>;
  alterarSenha: (senhaAtual: string, senhaNova: string, confirmarSenha: string) => Promise<void>;
  isLoading: boolean;
  isAuthenticated: boolean;
}
