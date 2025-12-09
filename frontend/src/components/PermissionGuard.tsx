import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { ShieldX, Lock, AlertTriangle } from 'lucide-react';
import { usePermissao, PERMISSAO_MAPPING } from '../hooks/usePermissao';
import type { ComportamentoPadrao } from '../hooks/usePermissao';
import { useAuth } from '../contexts/AuthContext';

// ==========================================
// COMPONENTE SemPermissao
// ==========================================
/**
 * Tela exibida quando usuário não tem permissão para acessar
 */
interface SemPermissaoProps {
  tabela?: string;
  mensagem?: string;
  tipo?: 'sem-acesso' | 'nao-encontrado' | 'erro';
}

export const SemPermissao: React.FC<SemPermissaoProps> = ({
  tabela,
  mensagem,
  tipo = 'sem-acesso',
}) => {
  const Icon = tipo === 'erro' ? AlertTriangle : tipo === 'nao-encontrado' ? Lock : ShieldX;
  const titulo = tipo === 'erro' 
    ? 'Ocorreu um erro' 
    : tipo === 'nao-encontrado' 
      ? 'Página não encontrada' 
      : 'Acesso Negado';

  return (
    <div className="flex flex-col items-center justify-center min-h-[60vh] p-8">
      <div className="bg-red-50 rounded-full p-6 mb-6">
        <Icon className="w-16 h-16 text-red-500" />
      </div>
      
      <h1 className="text-2xl font-bold text-[var(--text)] mb-2">{titulo}</h1>
      
      <p className="text-[var(--text-muted)] text-center max-w-md mb-6">
        {mensagem || `Você não tem permissão para acessar esta funcionalidade.`}
        {tabela && (
          <span className="block mt-2 text-sm text-[var(--text-muted)]">
            Permissão necessária: <code className="bg-gray-100 px-2 py-0.5 rounded">{tabela}</code>
          </span>
        )}
      </p>
      
      <div className="flex gap-3">
        <button
          onClick={() => window.history.back()}
          className="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200 transition-colors"
        >
          Voltar
        </button>
        <a
          href="/dashboard"
          className="px-4 py-2 text-white bg-blue-600 rounded-lg hover:bg-blue-700 transition-colors"
        >
          Ir para Dashboard
        </a>
      </div>
    </div>
  );
};

// ==========================================
// COMPONENTE PermissionRoute
// ==========================================
/**
 * Componente para proteger rotas baseado em permissões
 * (Diferente do ProtectedRoute que só verifica autenticação)
 * 
 * @example
 * // Protege baseado na rota atual (auto-detecta)
 * <PermissionRoute>
 *   <MinhaTelaClientes />
 * </PermissionRoute>
 * 
 * @example
 * // Especifica uma tabela específica
 * <PermissionRoute tabela="CLIENTES">
 *   <MinhaTelaClientes />
 * </PermissionRoute>
 * 
 * @example
 * // Exige permissão de modificar
 * <PermissionRoute tabela="CLIENTES" permissaoMinima="modificar">
 *   <FormularioEdicao />
 * </PermissionRoute>
 */
interface PermissionRouteProps {
  children: React.ReactNode;
  /** Nome da tabela de permissão. Se não informado, usa a rota atual */
  tabela?: string;
  /** Permissão mínima necessária */
  permissaoMinima?: 'visualizar' | 'incluir' | 'modificar' | 'excluir';
  /** Comportamento se tabela não existir */
  comportamentoPadrao?: ComportamentoPadrao;
  /** Componente customizado para exibir quando sem permissão */
  fallback?: React.ReactNode;
  /** Se true, redireciona para login ao invés de mostrar tela de erro */
  redirecionarParaLogin?: boolean;
}

export const PermissionRoute: React.FC<PermissionRouteProps> = ({
  children,
  tabela,
  permissaoMinima = 'visualizar',
  comportamentoPadrao,
  fallback,
  redirecionarParaLogin = false,
}) => {
  const location = useLocation();
  const { user, isLoading: authLoading } = useAuth();
  
  // Se não informou tabela, usa a rota atual
  const tabelaFinal = tabela || PERMISSAO_MAPPING[location.pathname] || location.pathname;
  
  const permissao = usePermissao(tabelaFinal, comportamentoPadrao);

  // Se está carregando auth ou permissão, mostra loading
  if (authLoading || permissao.carregando) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600" />
      </div>
    );
  }

  // Se não tem usuário, redireciona para login
  if (!user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // Verifica a permissão mínima
  let temPermissao = false;
  switch (permissaoMinima) {
    case 'visualizar':
      temPermissao = permissao.podeVisualizar;
      break;
    case 'incluir':
      temPermissao = permissao.podeIncluir;
      break;
    case 'modificar':
      temPermissao = permissao.podeModificar;
      break;
    case 'excluir':
      temPermissao = permissao.podeExcluir;
      break;
  }

  // Se não tem permissão
  if (!temPermissao) {
    if (redirecionarParaLogin) {
      return <Navigate to="/login" state={{ from: location }} replace />;
    }
    
    if (fallback) {
      return <>{fallback}</>;
    }
    
    return <SemPermissao tabela={permissao.tabela} />;
  }

  // Tem permissão, renderiza os children
  return <>{children}</>;
};

// ==========================================
// COMPONENTE ConditionalRender
// ==========================================
/**
 * Renderiza condicionalmente baseado em permissões
 * Útil para esconder botões, links, etc.
 * 
 * @example
 * <ConditionalRender tabela="CLIENTES" permissao="incluir">
 *   <Button>Novo Cliente</Button>
 * </ConditionalRender>
 * 
 * @example
 * // Com fallback (mostra algo diferente se não tem permissão)
 * <ConditionalRender tabela="CLIENTES" permissao="excluir" fallback={<span>Sem permissão</span>}>
 *   <Button variant="danger">Excluir</Button>
 * </ConditionalRender>
 */
interface ConditionalRenderProps {
  children: React.ReactNode;
  /** Nome da tabela de permissão */
  tabela: string;
  /** Permissão necessária */
  permissao: 'visualizar' | 'incluir' | 'modificar' | 'excluir';
  /** O que renderizar se não tiver permissão */
  fallback?: React.ReactNode;
}

export const ConditionalRender: React.FC<ConditionalRenderProps> = ({
  children,
  tabela,
  permissao,
  fallback = null,
}) => {
  const perm = usePermissao(tabela);

  let temPermissao = false;
  switch (permissao) {
    case 'visualizar':
      temPermissao = perm.podeVisualizar;
      break;
    case 'incluir':
      temPermissao = perm.podeIncluir;
      break;
    case 'modificar':
      temPermissao = perm.podeModificar;
      break;
    case 'excluir':
      temPermissao = perm.podeExcluir;
      break;
  }

  if (perm.carregando) {
    return null;
  }

  return temPermissao ? <>{children}</> : <>{fallback}</>;
};

// ==========================================
// COMPONENTE DisableWithoutPermission
// ==========================================
/**
 * Desabilita um elemento se não tiver permissão
 * Útil para botões que devem aparecer mas ficar desabilitados
 * 
 * @example
 * <DisableWithoutPermission tabela="CLIENTES" permissao="excluir">
 *   <Button>Excluir</Button>
 * </DisableWithoutPermission>
 */
interface DisableWithoutPermissionProps {
  children: React.ReactElement<{ disabled?: boolean; className?: string }>;
  tabela: string;
  permissao: 'visualizar' | 'incluir' | 'modificar' | 'excluir';
  /** Texto do tooltip quando desabilitado */
  tooltip?: string;
}

export const DisableWithoutPermission: React.FC<DisableWithoutPermissionProps> = ({
  children,
  tabela,
  permissao,
  tooltip = 'Você não tem permissão para esta ação',
}) => {
  const perm = usePermissao(tabela);

  let temPermissao = false;
  switch (permissao) {
    case 'visualizar':
      temPermissao = perm.podeVisualizar;
      break;
    case 'incluir':
      temPermissao = perm.podeIncluir;
      break;
    case 'modificar':
      temPermissao = perm.podeModificar;
      break;
    case 'excluir':
      temPermissao = perm.podeExcluir;
      break;
  }

  if (perm.carregando) {
    return React.cloneElement(children, { disabled: true } as { disabled: boolean });
  }

  if (!temPermissao) {
    return (
      <div title={tooltip}>
        {React.cloneElement(children, { 
          disabled: true, 
          className: `${children.props.className || ''} cursor-not-allowed opacity-50` 
        } as { disabled: boolean; className: string })}
      </div>
    );
  }

  return children;
};

export default PermissionRoute;
