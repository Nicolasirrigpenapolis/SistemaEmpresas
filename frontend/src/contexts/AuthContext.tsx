import React, { createContext, useContext, useState, useEffect } from 'react';
import type { AuthContextType, User, Tenant } from '../types';
import { authService } from '../services/Auth/authService';

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [tenant, setTenant] = useState<Tenant | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Carrega dados do localStorage na inicialização
  useEffect(() => {
    let isSubscribed = true;

    const initializeAuth = async () => {
      try {
        const savedUser = authService.getUser();
        const savedTenant = authService.getTenant();
        const savedToken = authService.getToken();
        const hasStoredSession = !!(savedUser || savedTenant || savedToken);

        // Verifica se todos os dados necessários existem
        if (savedUser && savedTenant && savedToken) {
          // Verifica se token não está expirado
          if (!authService.isTokenExpired()) {
            // Verifica se o usuário tem dados válidos (nome e grupo)
            if (savedUser.nome && savedUser.grupo) {
              if (!isSubscribed) return;
              setUser(savedUser);
              setTenant(savedTenant);
              setToken(savedToken);
            } else {
              // Dados do usuário inválidos, limpa tudo
              console.warn('Dados do usuário inválidos, limpando sessão');
              authService.clearLocalAuthData();
            }
          } else {
            // Token expirado, limpa dados
            authService.clearLocalAuthData();
          }
        } else if (hasStoredSession) {
          // Dados incompletos, limpa tudo para garantir
          authService.clearLocalAuthData();
        }
      } catch (error) {
        console.error('Erro ao carregar dados de autenticação:', error);
        // Em caso de erro, limpa tudo
        authService.clearLocalAuthData();
      } finally {
        if (isSubscribed) {
          setIsLoading(false);
        }
      }
    };

    initializeAuth();

    return () => {
      isSubscribed = false;
    };
  }, []);

  /**
   * Realiza login
   */
  const login = async (dominioTenant: string, usuario: string, senha: string) => {
    setIsLoading(true);
    try {
      const response = await authService.login(dominioTenant, usuario, senha);

      setUser(response.usuario);
      setTenant(response.tenant);
      setToken(response.token);
    } catch (error: any) {
      console.error('Erro no login:', error);
      // Backend retorna 'message', mas mantemos compatibilidade com 'mensagem' também
      const errorMessage = error.response?.data?.message || error.response?.data?.mensagem || 'Erro ao fazer login';
      throw new Error(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  /**
   * Realiza logout
   */
  const logout = async () => {
    setIsLoading(true);
    try {
      await authService.logout();
    } catch (error) {
      console.error('Erro ao fazer logout:', error);
    } finally {
      setUser(null);
      setTenant(null);
      setToken(null);
      setIsLoading(false);
    }
  };

  /**
   * Renova o token
   */
  const refreshToken = async () => {
    try {
      const newToken = await authService.refreshToken();
      setToken(newToken);
    } catch (error) {
      console.error('Erro ao renovar token:', error);
      // Se falhar, desloga
      await logout();
      throw error;
    }
  };

  /**
   * Altera senha do usuário
   */
  const alterarSenha = async (senhaAtual: string, senhaNova: string, confirmarSenha: string) => {
    await authService.alterarSenha(senhaAtual, senhaNova, confirmarSenha);
  };

  const isAuthenticated = !!user && !!token && !authService.isTokenExpired();

  return (
    <AuthContext.Provider 
      value={{ 
        user, 
        tenant, 
        token,
        login, 
        logout, 
        refreshToken,
        alterarSenha,
        isLoading,
        isAuthenticated,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
