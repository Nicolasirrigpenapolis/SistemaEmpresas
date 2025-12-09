import { useMemo } from 'react';
import { useAuth } from '../contexts/AuthContext';

// ==========================================
// MAPEAMENTO DE ROTAS PARA TELAS DE PERMISSÃO
// ==========================================
// Os nomes das telas devem corresponder EXATAMENTE ao campo "Tela" 
// da tabela PermissoesTela no banco de dados.
// Veja: PermissoesTelaRepository.GetTelasDisponiveis()
// ==========================================

export const PERMISSAO_MAPPING: Record<string, string> = {
  // ==========================================
  // DASHBOARD
  // ==========================================
  '/dashboard': 'Dashboard',
  '/': 'Dashboard',
  
  // ==========================================
  // CADASTROS
  // ==========================================
  '/cadastros/geral': 'Geral',
  '/cadastros/clientes': 'Geral',      // Usa Geral (filtro por tipo)
  '/cadastros/fornecedores': 'Geral',  // Usa Geral (filtro por tipo)
  '/cadastros/transportadoras': 'Geral', // Usa Geral (filtro por tipo)
  '/cadastros/vendedores': 'Geral',    // Usa Geral (filtro por tipo)
  '/cadastros/produtos': 'Produtos',
  '/produtos': 'Produtos',
  '/emitente': 'DadosEmitente',
  '/emitentes': 'Emitentes',
  
  // ==========================================
  // FISCAL
  // ==========================================
  '/faturamento/notas-fiscais': 'NotaFiscal',
  '/notas-fiscais': 'NotaFiscal',
  '/classificacao-fiscal': 'ClassificacaoFiscal',
  '/classtrib': 'ClassTrib',
  
  // ==========================================
  // SISTEMA
  // ==========================================
  '/usuarios': 'Usuarios',
  '/logs': 'Logs',
};

// ==========================================
// PERMISSÕES SEM RESTRIÇÃO
// ==========================================
// Telas que não precisam de permissão
export const ROTAS_LIVRES = [
  '/perfil',  // Usuário pode sempre ver seu próprio perfil
];

// ==========================================
// INTERFACE DE RETORNO DO HOOK
// ==========================================
export interface PermissaoResult {
  /** Pode visualizar/acessar a tela */
  podeVisualizar: boolean;
  /** Pode incluir novos registros */
  podeIncluir: boolean;
  /** Pode modificar registros existentes */
  podeModificar: boolean;
  /** Pode excluir registros */
  podeExcluir: boolean;
  /** Permissão completa (string "1111") */
  permissaoRaw: string;
  /** Se está carregando */
  carregando: boolean;
  /** Se a tabela/rota existe nas permissões */
  tabelaExiste: boolean;
  /** Nome da tabela de permissão */
  tabela: string;
}

// ==========================================
// COMPORTAMENTO PADRÃO PARA TABELAS NOVAS
// ==========================================
// Se a tabela não existe no PW~Tabelas:
// - 'liberado': Libera acesso (usuário pode acessar)
// - 'bloqueado': Bloqueia acesso (precisa criar no banco)
// - 'admin_only': Só admin pode acessar
export type ComportamentoPadrao = 'liberado' | 'bloqueado' | 'admin_only';

// SEGURANÇA: Por padrão bloquear acesso quando permissão não existe
// Isso evita que usuários acessem telas sem permissão explícita
const COMPORTAMENTO_PADRAO: ComportamentoPadrao = 'bloqueado';

// ==========================================
// HOOK usePermissao
// ==========================================
/**
 * Hook para verificar permissões do usuário logado
 * 
 * @param tabelaOuRota - Nome da tabela (ex: 'CLIENTES') ou rota (ex: '/clientes')
 * @param comportamentoPadrao - O que fazer se a tabela não existir
 * 
 * @example
 * // Por tabela
 * const { podeVisualizar, podeIncluir } = usePermissao('CLIENTES');
 * 
 * @example
 * // Por rota (converte automaticamente)
 * const { podeModificar, podeExcluir } = usePermissao('/clientes');
 * 
 * @example
 * // Com comportamento customizado para tabela nova
 * const permissao = usePermissao('DASHBOARD', 'admin_only');
 */
export function usePermissao(
  tabelaOuRota: string,
  comportamentoPadrao: ComportamentoPadrao = COMPORTAMENTO_PADRAO
): PermissaoResult {
  const { user, isLoading } = useAuth();

  return useMemo(() => {
    // Determina o nome da tabela
    let tabela = tabelaOuRota.toUpperCase();
    
    // Se começa com /, é uma rota - converte para tabela
    if (tabelaOuRota.startsWith('/')) {
      tabela = PERMISSAO_MAPPING[tabelaOuRota] || tabelaOuRota.replace('/', '').toUpperCase();
    }

    // Verifica se é rota livre
    if (ROTAS_LIVRES.includes(tabelaOuRota)) {
      return {
        podeVisualizar: true,
        podeIncluir: true,
        podeModificar: true,
        podeExcluir: true,
        permissaoRaw: '1111',
        carregando: false,
        tabelaExiste: true,
        tabela,
      };
    }

    // Se está carregando ou não tem usuário
    if (isLoading || !user) {
      return {
        podeVisualizar: false,
        podeIncluir: false,
        podeModificar: false,
        podeExcluir: false,
        permissaoRaw: '0000',
        carregando: isLoading,
        tabelaExiste: false,
        tabela,
      };
    }

    // PROGRAMADOR tem acesso total ao sistema
    const isProgramador = user.grupo?.toUpperCase() === 'PROGRAMADOR';
    
    if (isProgramador) {
      return {
        podeVisualizar: true,
        podeIncluir: true,
        podeModificar: true,
        podeExcluir: true,
        permissaoRaw: '1111',
        carregando: false,
        tabelaExiste: true,
        tabela,
      };
    }

    // Busca a permissão da tabela
    const permissao = user.permissoes?.find(
      p => p.tabela?.toUpperCase() === tabela
    );

    // Se tabela não existe nas permissões
    if (!permissao) {
      // Aplica comportamento padrão
      if (comportamentoPadrao === 'liberado') {
        return {
          podeVisualizar: true,
          podeIncluir: true,
          podeModificar: true,
          podeExcluir: true,
          permissaoRaw: '1111',
          carregando: false,
          tabelaExiste: false,
          tabela,
        };
      } else if (comportamentoPadrao === 'admin_only') {
        return {
          podeVisualizar: false,
          podeIncluir: false,
          podeModificar: false,
          podeExcluir: false,
          permissaoRaw: '0000',
          carregando: false,
          tabelaExiste: false,
          tabela,
        };
      } else {
        // bloqueado
        return {
          podeVisualizar: false,
          podeIncluir: false,
          podeModificar: false,
          podeExcluir: false,
          permissaoRaw: '0000',
          carregando: false,
          tabelaExiste: false,
          tabela,
        };
      }
    }

    // Extrai as permissões do formato "1111" (Visualiza, Inclui, Modifica, Exclui)
    const perm = permissao.permissoes || '0000';
    
    return {
      podeVisualizar: perm.charAt(0) === '1',
      podeIncluir: perm.charAt(1) === '1',
      podeModificar: perm.charAt(2) === '1',
      podeExcluir: perm.charAt(3) === '1',
      permissaoRaw: perm,
      carregando: false,
      tabelaExiste: true,
      tabela,
    };
  }, [user, isLoading, tabelaOuRota, comportamentoPadrao]);
}

// ==========================================
// HOOK usePermissaoRota
// ==========================================
/**
 * Hook para verificar permissão da rota atual
 * Útil para usar em layouts/guards
 */
export function usePermissaoRota(): PermissaoResult & { rota: string } {
  // Pega a rota atual do window.location
  const rota = typeof window !== 'undefined' 
    ? window.location.pathname 
    : '/';
  
  const permissao = usePermissao(rota);
  
  return {
    ...permissao,
    rota,
  };
}

// ==========================================
// FUNÇÃO HELPER - getPermissaoTabela
// ==========================================
/**
 * Função para usar fora de componentes React
 * (ex: em serviços, interceptors, etc.)
 */
export function getPermissaoFromUser(
  user: { 
    grupo?: string | null;
    permissoes?: Array<{ tabela: string; permissoes: string }> 
  } | null,
  tabela: string
): PermissaoResult {
  const tabelaUpper = tabela.toUpperCase();
  
  if (!user) {
    return {
      podeVisualizar: false,
      podeIncluir: false,
      podeModificar: false,
      podeExcluir: false,
      permissaoRaw: '0000',
      carregando: false,
      tabelaExiste: false,
      tabela: tabelaUpper,
    };
  }
  
  // PROGRAMADOR tem acesso total
  const isProgramador = user.grupo?.toUpperCase() === 'PROGRAMADOR';
  if (isProgramador) {
    return {
      podeVisualizar: true,
      podeIncluir: true,
      podeModificar: true,
      podeExcluir: true,
      permissaoRaw: '1111',
      carregando: false,
      tabelaExiste: true,
      tabela: tabelaUpper,
    };
  }

  const permissao = user.permissoes?.find(
    p => p.tabela?.toUpperCase() === tabelaUpper
  );

  if (!permissao) {
    // SEGURANÇA: Por padrão bloqueia se não existe permissão
    return {
      podeVisualizar: false,
      podeIncluir: false,
      podeModificar: false,
      podeExcluir: false,
      permissaoRaw: '0000',
      carregando: false,
      tabelaExiste: false,
      tabela: tabelaUpper,
    };
  }

  const perm = permissao.permissoes || '0000';
  
  return {
    podeVisualizar: perm.charAt(0) === '1',
    podeIncluir: perm.charAt(1) === '1',
    podeModificar: perm.charAt(2) === '1',
    podeExcluir: perm.charAt(3) === '1',
    permissaoRaw: perm,
    carregando: false,
    tabelaExiste: true,
    tabela: tabelaUpper,
  };
}

// ==========================================
// HOOK usePermissaoMenu
// ==========================================
/**
 * Hook para verificar permissões de múltiplas rotas do menu
 * Retorna um mapa de rota -> boolean (se pode visualizar)
 * 
 * @example
 * const permissoes = usePermissaoMenu(['/clientes', '/produtos', '/usuarios']);
 * if (permissoes['/clientes']) { // mostrar menu de clientes }
 */
export function usePermissaoMenu(rotas: string[]): Record<string, boolean> {
  const { user, isLoading } = useAuth();

  return useMemo(() => {
    const resultado: Record<string, boolean> = {};

    if (isLoading || !user) {
      rotas.forEach(rota => {
        resultado[rota] = false;
      });
      return resultado;
    }

    // PROGRAMADOR tem acesso total
    const isProgramador = user.grupo?.toUpperCase() === 'PROGRAMADOR';
    
    if (isProgramador) {
      rotas.forEach(rota => {
        resultado[rota] = true;
      });
      return resultado;
    }

    rotas.forEach(rota => {
      // Verificar se é rota livre
      if (ROTAS_LIVRES.includes(rota)) {
        resultado[rota] = true;
        return;
      }

      // Determina o nome da tabela
      // Remove query strings para mapeamento
      const rotaBase = rota.split('?')[0];
      const tabela = PERMISSAO_MAPPING[rotaBase] || rotaBase.replace('/', '').toUpperCase();

      // Busca a permissão da tabela
      const permissao = user.permissoes?.find(
        p => p.tabela?.toUpperCase() === tabela.toUpperCase()
      );

      if (!permissao) {
        // SEGURANÇA: Por padrão bloqueia se não existe permissão
        resultado[rota] = false;
        return;
      }

      // Verifica se pode visualizar (primeiro caractere)
      const perm = permissao.permissoes || '0000';
      resultado[rota] = perm.charAt(0) === '1';
    });

    return resultado;
  }, [user, isLoading, rotas.join(',')]);
}

/**
 * Função helper para verificar se usuário pode visualizar uma rota específica
 * Usada internamente pelo Sidebar
 */
export function podeVisualizarRota(
  user: { 
    grupo?: string | null;
    permissoes?: Array<{ tabela: string; permissoes: string }> 
  } | null,
  rota: string
): boolean {
  if (!user) return false;
  
  // Verifica se é rota livre
  if (ROTAS_LIVRES.includes(rota)) return true;
  
  // PROGRAMADOR tem acesso total
  const isProgramador = user.grupo?.toUpperCase() === 'PROGRAMADOR';
  if (isProgramador) return true;
  
  // Remove query strings para mapeamento
  const rotaBase = rota.split('?')[0];
  const tabela = PERMISSAO_MAPPING[rotaBase] || rotaBase.replace('/', '').toUpperCase();
  
  // Busca a permissão da tabela
  const permissao = user.permissoes?.find(
    p => p.tabela?.toUpperCase() === tabela.toUpperCase()
  );
  
  if (!permissao) return false;
  
  // Verifica se pode visualizar (primeiro caractere)
  const perm = permissao.permissoes || '0000';
  return perm.charAt(0) === '1';
}

export default usePermissao;
