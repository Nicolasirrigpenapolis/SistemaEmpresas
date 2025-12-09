import axios, { type AxiosError, type InternalAxiosRequestConfig } from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5001/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Flag para evitar loop infinito no refresh token
let isRefreshing = false;
let failedQueue: Array<{
  resolve: (value?: any) => void;
  reject: (reason?: any) => void;
}> = [];

const processQueue = (error: Error | null, token: string | null = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};

// ==========================================
// INTERCEPTOR DE REQUEST - Adiciona JWT e Tenant
// ==========================================
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // 1. Adicionar Token JWT
    const token = localStorage.getItem('auth_token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    // 2. Adicionar Header X-Tenant (multi-tenant)
    const tenantJson = localStorage.getItem('auth_tenant');
    if (tenantJson && config.headers) {
      try {
        const tenant = JSON.parse(tenantJson);
        config.headers['X-Tenant'] = tenant.dominio;
      } catch (error) {
        console.error('Erro ao parsear tenant:', error);
      }
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// ==========================================
// INTERCEPTOR DE RESPONSE - Trata erros 401 e refresh token
// ==========================================
api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

    // IGNORA erros nas rotas de autenticação (login, refresh, etc)
    // Para não limpar dados e redirecionar quando o usuário erra a senha
    // Verifica tanto no path relativo quanto na URL completa
    const requestUrl = originalRequest?.url || '';
    const isAuthRoute = requestUrl.includes('/auth/') || requestUrl.includes('/api/auth/');
    
    if (isAuthRoute) {
      // Apenas rejeita o erro sem fazer nenhuma ação adicional
      return Promise.reject(error);
    }

    // Se erro 401 (Unauthorized) e não é rota de login
    if (error.response?.status === 401 && originalRequest && !originalRequest._retry) {
      // Se já está tentando fazer refresh, adiciona à fila
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then(token => {
            if (originalRequest.headers) {
              originalRequest.headers.Authorization = `Bearer ${token}`;
            }
            return api(originalRequest);
          })
          .catch(err => {
            return Promise.reject(err);
          });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      const refreshToken = localStorage.getItem('auth_refresh_token');

      // Se não tem refresh token, desloga
      if (!refreshToken) {
        processQueue(new Error('Sessão expirada'), null);
        isRefreshing = false;
        
        // Limpa storage e redireciona para login
        localStorage.clear();
        window.location.href = '/login';
        
        return Promise.reject(error);
      }

      try {
        // Tenta renovar o token
        const response = await axios.post(`${API_URL}/auth/refresh`, {
          refreshToken,
        });

        const { token: newToken, refreshToken: newRefreshToken } = response.data;

        // Salva novos tokens
        localStorage.setItem('auth_token', newToken);
        localStorage.setItem('auth_refresh_token', newRefreshToken);

        // Processa fila de requisições pendentes
        processQueue(null, newToken);

        // Atualiza token na requisição original
        if (originalRequest.headers) {
          originalRequest.headers.Authorization = `Bearer ${newToken}`;
        }

        isRefreshing = false;

        // Reexecuta requisição original
        return api(originalRequest);
      } catch (refreshError) {
        processQueue(refreshError as Error, null);
        isRefreshing = false;

        // Limpa storage e redireciona para login
        localStorage.clear();
        window.location.href = '/login';

        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

// ==========================================
// EXPORTAR INSTÂNCIA DO AXIOS
// ==========================================
export default api;
