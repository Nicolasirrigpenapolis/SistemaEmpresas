import api from './api';
import type { 
  LoginRequestDto, 
  LoginResponseDto, 
  UsuarioDto, 
  AlterarSenhaDto 
} from '../types';

// Chaves do localStorage
const STORAGE_KEYS = {
  TOKEN: 'auth_token',
  REFRESH_TOKEN: 'auth_refresh_token',
  USER: 'auth_user',
  TENANT: 'auth_tenant',
  TOKEN_EXPIRATION: 'auth_token_expiration',
} as const;

class AuthService {
  /**
   * Realiza login no sistema
   * O backend já retorna os dados descriptografados - não precisa processar no frontend
   */
  async login(dominioTenant: string, usuario: string, senha: string): Promise<LoginResponseDto> {
    const request: LoginRequestDto = {
      usuario,
      senha,
      dominioTenant,
    };

    const response = await api.post<LoginResponseDto>('/auth/login', request, {
      headers: {
        'X-Tenant': dominioTenant, // Header necessário para multi-tenant
      },
    });

    // Backend já envia dados descriptografados - salva direto
    this.saveAuthData(response.data);

    return response.data;
  }

  /**
   * Faz logout (remove tokens e chama endpoint no backend)
   */
  async logout(): Promise<void> {
    const hasToken = !!this.getToken();

    try {
      // Chamar endpoint de logout no backend (invalida refresh token)
      if (hasToken) {
        await api.post('/auth/logout');
      }
    } catch (error) {
      console.error('Erro ao fazer logout no backend:', error);
    } finally {
      // Sempre limpa dados locais, mesmo se backend falhar
      this.clearAuthData();
    }
  }

  /**
   * Renova o token usando refresh token
   * O backend já retorna os dados descriptografados
   */
  async refreshToken(): Promise<string> {
    const refreshToken = this.getRefreshToken();

    if (!refreshToken) {
      throw new Error('Refresh token não encontrado');
    }

    const response = await api.post<LoginResponseDto>('/auth/refresh', {
      refreshToken,
    });

    // Backend já envia dados descriptografados - salva direto
    this.saveAuthData(response.data);

    return response.data.token;
  }

  /**
   * Busca informações do usuário logado
   */
  async getMe(): Promise<UsuarioDto> {
    const response = await api.get<UsuarioDto>('/auth/me');
    return response.data;
  }

  /**
   * Altera a senha do usuário logado
   */
  async alterarSenha(senhaAtual: string, senhaNova: string, confirmarSenha: string): Promise<void> {
    const request: AlterarSenhaDto = {
      senhaAtual,
      senhaNova,
      confirmarSenha,
    };

    await api.put('/auth/alterar-senha', request);
  }

  /**
   * Salva dados de autenticação no localStorage
   */
  private saveAuthData(data: LoginResponseDto): void {
    localStorage.setItem(STORAGE_KEYS.TOKEN, data.token);
    localStorage.setItem(STORAGE_KEYS.REFRESH_TOKEN, data.refreshToken);
    localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(data.usuario));
    localStorage.setItem(STORAGE_KEYS.TENANT, JSON.stringify(data.tenant));
    localStorage.setItem(STORAGE_KEYS.TOKEN_EXPIRATION, data.expiracao);
  }

  /**
   * Limpa dados de autenticação do localStorage
   */
  private clearAuthData(): void {
    localStorage.removeItem(STORAGE_KEYS.TOKEN);
    localStorage.removeItem(STORAGE_KEYS.REFRESH_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
    localStorage.removeItem(STORAGE_KEYS.TENANT);
    localStorage.removeItem(STORAGE_KEYS.TOKEN_EXPIRATION);
    // Limpa também dados de "lembrar-me" antigos
    localStorage.removeItem('lastEmpresa');
    localStorage.removeItem('lastUsuario');
    localStorage.removeItem('rememberMe');
  }

  /**
   * Limpa apenas os dados locais (sem chamar backend)
   */
  clearLocalAuthData(): void {
    this.clearAuthData();
  }

  /**
   * Retorna o token JWT
   */
  getToken(): string | null {
    return localStorage.getItem(STORAGE_KEYS.TOKEN);
  }

  /**
   * Retorna o refresh token
   */
  getRefreshToken(): string | null {
    return localStorage.getItem(STORAGE_KEYS.REFRESH_TOKEN);
  }

  /**
   * Retorna o usuário salvo no localStorage
   * Os dados já vêm descriptografados do backend
   */
  getUser(): UsuarioDto | null {
    const userJson = localStorage.getItem(STORAGE_KEYS.USER);
    if (!userJson) return null;
    try {
      return JSON.parse(userJson) as UsuarioDto;
    } catch {
      return null;
    }
  }

  /**
   * Retorna o tenant salvo no localStorage
   */
  getTenant(): any | null {
    const tenantJson = localStorage.getItem(STORAGE_KEYS.TENANT);
    return tenantJson ? JSON.parse(tenantJson) : null;
  }

  /**
   * Verifica se o token está expirado
   */
  isTokenExpired(): boolean {
    const expiration = localStorage.getItem(STORAGE_KEYS.TOKEN_EXPIRATION);
    
    if (!expiration) {
      return true;
    }

    const expirationDate = new Date(expiration);
    const now = new Date();

    return now >= expirationDate;
  }

  /**
   * Verifica se o usuário está autenticado
   */
  isAuthenticated(): boolean {
    const token = this.getToken();
    
    if (!token) {
      return false;
    }

    // Se token expirou, não está autenticado
    if (this.isTokenExpired()) {
      return false;
    }

    return true;
  }
}

export const authService = new AuthService();
export default authService;
