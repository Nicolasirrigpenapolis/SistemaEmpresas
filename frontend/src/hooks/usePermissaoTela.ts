import { useState, useEffect, useCallback, useMemo } from "react";
import { useAuth } from "../contexts/AuthContext";
import permissoesTelaService from "../services/Seguranca/permissoesTelaService";
import type { PermissoesUsuarioLogadoDto, PermissaoTelaResumoDto } from '../types';

// ==========================================
// INTERFACE DE RETORNO DO HOOK
// ==========================================
export interface PermissaoTelaResult {
  /** Pode consultar/acessar a tela */
  podeConsultar: boolean;
  /** Pode incluir novos registros */
  podeIncluir: boolean;
  /** Pode alterar registros existentes */
  podeAlterar: boolean;
  /** Pode excluir registros */
  podeExcluir: boolean;
  /** Se está carregando */
  carregando: boolean;
  /** Se é admin (tem todas as permissões) */
  isAdmin: boolean;
  /** Se a tela existe nas permissões */
  telaExiste: boolean;
  /** Nome da tela */
  tela: string;
  /** Recarregar permissões */
  recarregar: () => Promise<void>;
}

// Cache global para evitar múltiplas chamadas
let cachedPermissoes: PermissoesUsuarioLogadoDto | null = null;
let cacheTimestamp: number = 0;
const CACHE_DURATION = 5 * 60 * 1000; // 5 minutos

// ==========================================
// HOOK usePermissaoTela
// ==========================================
/**
 * Hook para verificar permissões do usuário logado usando a nova tabela PermissoesTelas
 * 
 * @param tela - Identificador da tela (ex: 'Usuarios', 'ClassTrib', 'Dashboard')
 * 
 * @example
 * const { podeConsultar, podeIncluir, podeAlterar, podeExcluir } = usePermissaoTela('ClassTrib');
 * 
 * // Uso no componente:
 * {podeIncluir && <button>Novo Registro</button>}
 * {podeAlterar && <button>Editar</button>}
 * {podeExcluir && <button>Excluir</button>}
 */
export function usePermissaoTela(tela: string): PermissaoTelaResult {
  const { user, isLoading: authLoading } = useAuth();
  const [permissoes, setPermissoes] = useState<PermissoesUsuarioLogadoDto | null>(null);
  const [carregando, setCarregando] = useState(true);

  // Função para carregar permissões
  const carregarPermissoes = useCallback(async () => {
    if (!user) {
      setPermissoes(null);
      setCarregando(false);
      return;
    }

    // Verifica se o cache ainda é válido
    const now = Date.now();
    if (cachedPermissoes && (now - cacheTimestamp) < CACHE_DURATION) {
      setPermissoes(cachedPermissoes);
      setCarregando(false);
      return;
    }

    try {
      setCarregando(true);
      const data = await permissoesTelaService.obterMinhasPermissoes();
      cachedPermissoes = data;
      cacheTimestamp = now;
      setPermissoes(data);
    } catch (error) {
      console.error('Erro ao carregar permissões:', error);
      // Em caso de erro, permite acesso (comportamento fail-open)
      setPermissoes(null);
    } finally {
      setCarregando(false);
    }
  }, [user]);

  // Função para forçar recarregamento
  const recarregar = useCallback(async () => {
    cachedPermissoes = null;
    cacheTimestamp = 0;
    await carregarPermissoes();
  }, [carregarPermissoes]);

  // Carrega permissões quando o usuário muda
  useEffect(() => {
    if (!authLoading) {
      carregarPermissoes();
    }
  }, [authLoading, carregarPermissoes]);

  // Calcula as permissões para a tela específica
  return useMemo(() => {
    const telaKey = tela;
    
    // Se está carregando autenticação ou permissões
    if (authLoading || carregando) {
      return {
        podeConsultar: false,
        podeIncluir: false,
        podeAlterar: false,
        podeExcluir: false,
        carregando: true,
        isAdmin: false,
        telaExiste: false,
        tela: telaKey,
        recarregar,
      };
    }

    // Se não tem usuário logado
    if (!user) {
      return {
        podeConsultar: false,
        podeIncluir: false,
        podeAlterar: false,
        podeExcluir: false,
        carregando: false,
        isAdmin: false,
        telaExiste: false,
        tela: telaKey,
        recarregar,
      };
    }

    // PROGRAMADOR tem acesso total
    const isProgramador = user.grupo?.toUpperCase() === 'PROGRAMADOR';
    if (isProgramador) {
      return {
        podeConsultar: true,
        podeIncluir: true,
        podeAlterar: true,
        podeExcluir: true,
        carregando: false,
        isAdmin: true,
        telaExiste: true,
        tela: telaKey,
        recarregar,
      };
    }

    // Se não tem permissões carregadas, libera acesso (fail-open)
    if (!permissoes) {
      return {
        podeConsultar: true,
        podeIncluir: true,
        podeAlterar: true,
        podeExcluir: true,
        carregando: false,
        isAdmin: false,
        telaExiste: false,
        tela: telaKey,
        recarregar,
      };
    }

    // Se é admin, libera tudo
    if (permissoes.isAdmin) {
      return {
        podeConsultar: true,
        podeIncluir: true,
        podeAlterar: true,
        podeExcluir: true,
        carregando: false,
        isAdmin: true,
        telaExiste: true,
        tela: telaKey,
        recarregar,
      };
    }

    // Busca permissões da tela específica
    const permissaoTela: PermissaoTelaResumoDto | undefined = permissoes.telas?.[telaKey];

    // Se a tela não existe nas permissões, bloqueia
    if (!permissaoTela) {
      return {
        podeConsultar: false,
        podeIncluir: false,
        podeAlterar: false,
        podeExcluir: false,
        carregando: false,
        isAdmin: false,
        telaExiste: false,
        tela: telaKey,
        recarregar,
      };
    }

    // Retorna as permissões específicas
    return {
      podeConsultar: permissaoTela.c,
      podeIncluir: permissaoTela.i,
      podeAlterar: permissaoTela.a,
      podeExcluir: permissaoTela.e,
      carregando: false,
      isAdmin: false,
      telaExiste: true,
      tela: telaKey,
      recarregar,
    };
  }, [tela, user, permissoes, authLoading, carregando, recarregar]);
}

// ==========================================
// HOOK useTodasPermissoes
// ==========================================
/**
 * Hook para obter todas as permissões do usuário logado
 * Útil para componentes que precisam verificar múltiplas telas
 */
export function useTodasPermissoes() {
  const { user, isLoading: authLoading } = useAuth();
  const [permissoes, setPermissoes] = useState<PermissoesUsuarioLogadoDto | null>(null);
  const [carregando, setCarregando] = useState(true);

  const carregarPermissoes = useCallback(async () => {
    if (!user) {
      setPermissoes(null);
      setCarregando(false);
      return;
    }

    const now = Date.now();
    if (cachedPermissoes && (now - cacheTimestamp) < CACHE_DURATION) {
      setPermissoes(cachedPermissoes);
      setCarregando(false);
      return;
    }

    try {
      setCarregando(true);
      const data = await permissoesTelaService.obterMinhasPermissoes();
      cachedPermissoes = data;
      cacheTimestamp = now;
      setPermissoes(data);
    } catch (error) {
      console.error('Erro ao carregar permissões:', error);
      setPermissoes(null);
    } finally {
      setCarregando(false);
    }
  }, [user]);

  useEffect(() => {
    if (!authLoading) {
      carregarPermissoes();
    }
  }, [authLoading, carregarPermissoes]);

  // Helper para verificar permissão de uma tela específica
  const verificarPermissao = useCallback((tela: string): PermissaoTelaResumoDto => {
    if (!permissoes || !permissoes.telas) {
      return { c: true, i: true, a: true, e: true }; // Fail-open
    }
    
    if (permissoes.isAdmin) {
      return { c: true, i: true, a: true, e: true };
    }

    return permissoes.telas[tela] || { c: false, i: false, a: false, e: false };
  }, [permissoes]);

  return {
    permissoes,
    carregando: authLoading || carregando,
    isAdmin: permissoes?.isAdmin || false,
    verificarPermissao,
    recarregar: async () => {
      cachedPermissoes = null;
      cacheTimestamp = 0;
      await carregarPermissoes();
    },
  };
}

// ==========================================
// FUNÇÃO HELPER - limparCachePermissoes
// ==========================================
/**
 * Limpa o cache de permissões
 * Chamar após salvar alterações de permissões
 */
export function limparCachePermissoes() {
  cachedPermissoes = null;
  cacheTimestamp = 0;
}

export default usePermissaoTela;
