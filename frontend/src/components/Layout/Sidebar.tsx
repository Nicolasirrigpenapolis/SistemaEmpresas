import { useState, useEffect, useMemo } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import {
  LayoutDashboard,
  Warehouse,
  ChevronLeft,
  ChevronRight,
  ChevronDown,
  ClipboardList,
  Receipt,
  Settings,
  FileText,
  X,
} from 'lucide-react';
import { useTenantBranding } from '../../hooks/useTenantBranding';
import { useAuth } from '../../contexts/AuthContext';
import { podeVisualizarRota } from '../../hooks/usePermissao';

interface SidebarProps {
  collapsed?: boolean;
  onCollapse?: (collapsed: boolean) => void;
  mobileOpen?: boolean;
  onMobileClose?: () => void;
}

interface MenuItem {
  id: string;
  label: string;
  icon: React.ElementType;
  path?: string;
  badge?: string | number;
  badgeColor?: string;
  children?: {
    id: string;
    label: string;
    path: string;
    badge?: string | number;
  }[];
}

export default function Sidebar({ collapsed = false, onCollapse, mobileOpen = false, onMobileClose }: SidebarProps) {
  const navigate = useNavigate();
  const location = useLocation();
  const branding = useTenantBranding();
  const { user } = useAuth();
  const [isCollapsed, setIsCollapsed] = useState(collapsed);
  const [expandedMenus, setExpandedMenus] = useState<string[]>(['vendas']);
  const [hoveredItem, setHoveredItem] = useState<string | null>(null);

  useEffect(() => {
    setIsCollapsed(collapsed);
  }, [collapsed]);

  const handleCollapse = () => {
    const newState = !isCollapsed;
    setIsCollapsed(newState);
    onCollapse?.(newState);
  };

  const toggleSubmenu = (menuId: string) => {
    if (isCollapsed && !mobileOpen) {
      setIsCollapsed(false);
      onCollapse?.(false);
      setExpandedMenus([menuId]);
    } else {
      setExpandedMenus(prev =>
        prev.includes(menuId)
          ? prev.filter(id => id !== menuId)
          : [...prev, menuId]
      );
    }
  };

  const handleNavigate = (path: string) => {
    navigate(path);
    // Fecha menu mobile após navegação
    if (mobileOpen) {
      onMobileClose?.();
    }
  };

  const isActive = (path?: string) => {
    if (!path) return false;
    return location.pathname === path || location.pathname.startsWith(path + '/');
  };

  const isMenuActive = (item: MenuItem) => {
    if (item.path && isActive(item.path)) return true;
    if (item.children) {
      return item.children.some(child => isActive(child.path));
    }
    return false;
  };

  // Menu items configuration
  const menuItems: MenuItem[] = [
    {
      id: 'dashboard',
      label: 'Dashboard',
      icon: LayoutDashboard,
      path: '/dashboard',
    },
    {
      id: 'cadastros',
      label: 'Cadastros',
      icon: ClipboardList,
      children: [
        { id: 'geral', label: 'Geral (Todos)', path: '/cadastros/geral' },
        { id: 'clientes', label: 'Clientes', path: '/cadastros/clientes?tipo=cliente' },
        { id: 'fornecedores', label: 'Fornecedores', path: '/cadastros/fornecedores?tipo=fornecedor' },
        { id: 'transportadoras', label: 'Transportadoras', path: '/cadastros/transportadoras?tipo=transportadora' },
        { id: 'vendedores', label: 'Vendedores', path: '/cadastros/vendedores?tipo=vendedor' },
      ],
    },
    {
      id: 'estoque',
      label: 'Estoque',
      icon: Warehouse,
      children: [
        { id: 'produtos', label: 'Produtos', path: '/produtos' },
      ],
    },
    {
      id: 'faturamento',
      label: 'Faturamento',
      icon: FileText,
      children: [
        { id: 'notas-fiscais', label: 'Notas Fiscais', path: '/faturamento/notas-fiscais' },
      ],
    },
    {
      id: 'fiscal',
      label: 'Fiscal',
      icon: Receipt,
      children: [
        { id: 'classificacao-fiscal', label: 'Classificação Fiscal', path: '/classificacao-fiscal' },
        { id: 'classtrib', label: 'ClassTrib (IBS/CBS)', path: '/classtrib' },
      ],
    },
    {
      id: 'sistema',
      label: 'Sistema',
      icon: Settings,
      children: [
        { id: 'emitente', label: 'Dados do Emitente', path: '/emitente' },
        { id: 'usuarios', label: 'Usuários e Permissões', path: '/usuarios' },
        { id: 'logs', label: 'Logs de Auditoria', path: '/logs' },
      ],
    },
  ];

  // Filtra os menus baseado nas permissões do usuário
  const filteredMenuItems = useMemo(() => {
    return menuItems
      .map(item => {
        // Se tem path direto, verifica permissão
        if (item.path) {
          if (!podeVisualizarRota(user, item.path)) {
            return null;
          }
          return item;
        }
        
        // Se tem filhos, filtra os filhos que o usuário pode ver
        if (item.children) {
          const filteredChildren = item.children.filter(child => 
            podeVisualizarRota(user, child.path)
          );
          
          // Se não sobrou nenhum filho visível, não mostra o menu pai
          if (filteredChildren.length === 0) {
            return null;
          }
          
          return {
            ...item,
            children: filteredChildren,
          };
        }
        
        return item;
      })
      .filter((item): item is MenuItem => item !== null);
  }, [user]);

  const renderMenuItem = (item: MenuItem, _isBottom = false) => {
    const hasChildren = item.children && item.children.length > 0;
    const isExpanded = expandedMenus.includes(item.id);
    const active = isMenuActive(item);
    const Icon = item.icon;

    return (
      <div key={item.id} className="relative">
        {/* Menu Item */}
        <button
          onClick={() => hasChildren ? toggleSubmenu(item.id) : item.path && handleNavigate(item.path)}
          onMouseEnter={() => setHoveredItem(item.id)}
          onMouseLeave={() => setHoveredItem(null)}
          className={`
            w-full flex items-center gap-3 px-3 py-2.5 rounded-lg transition-all duration-200 group
            ${active
              ? 'bg-blue-50 text-blue-600'
              : 'text-[var(--text-muted)] hover:bg-[var(--surface-muted)] hover:text-[var(--text)]'
            }
            ${isCollapsed ? 'justify-center' : ''}
          `}
        >
          {/* Icon */}
          <div className={`
            flex items-center justify-center w-8 h-8 rounded-lg transition-all duration-200
            ${active
              ? 'bg-blue-500 text-white shadow-sm'
              : 'bg-[var(--surface-muted)] text-[var(--text-muted)] group-hover:bg-[var(--surface-muted)] group-hover:text-[var(--text)]'
            }
          `}>
            <Icon className="w-[18px] h-[18px]" />
          </div>

          {/* Label e Badge */}
          {!isCollapsed && (
            <>
              <span className="flex-1 text-left font-medium text-sm">{item.label}</span>
              
              {/* Badge */}
              {item.badge && (
                <span className={`
                  px-2 py-0.5 text-xs font-semibold rounded-full text-white
                  ${item.badgeColor || 'bg-blue-500'}
                `}>
                  {item.badge}
                </span>
              )}

              {/* Chevron para submenus */}
              {hasChildren && (
                <ChevronDown className={`
                  w-4 h-4 text-gray-400 transition-transform duration-200
                  ${isExpanded ? 'rotate-180' : ''}
                `} />
              )}
            </>
          )}
        </button>

        {/* Tooltip quando collapsed */}
        {isCollapsed && hoveredItem === item.id && (
          <div className="absolute left-full top-1/2 -translate-y-1/2 ml-4 z-50">
            <div className="bg-gray-800 text-white text-sm font-medium px-3 py-2 rounded-lg shadow-xl whitespace-nowrap">
              {item.label}
              {item.badge && (
                <span className={`ml-2 px-1.5 py-0.5 text-xs rounded ${item.badgeColor || 'bg-blue-500'}`}>
                  {item.badge}
                </span>
              )}
              <div className="absolute right-full top-1/2 -translate-y-1/2 border-8 border-transparent border-r-gray-800" />
            </div>
          </div>
        )}

        {/* Submenu */}
        {hasChildren && !isCollapsed && (
          <div className={`
            overflow-hidden transition-all duration-300 ease-in-out
            ${isExpanded ? 'max-h-96 opacity-100' : 'max-h-0 opacity-0'}
          `}>
            <div className="ml-6 mt-1 pl-4 border-l-2 border-[var(--border)] space-y-0.5">
              {item.children?.map(child => (
                <button
                  key={child.id}
                  onClick={() => handleNavigate(child.path)}
                  className={`
                    w-full flex items-center justify-between px-3 py-2 rounded-lg text-sm transition-all duration-200
                    ${isActive(child.path)
                      ? 'bg-blue-50 text-blue-600 font-medium'
                      : 'text-[var(--text-muted)] hover:bg-[var(--surface-muted)] hover:text-[var(--text)]'
                    }
                  `}
                >
                  <span>{child.label}</span>
                  {child.badge && (
                    <span className="px-2 py-0.5 text-xs font-semibold rounded-full bg-blue-100 text-blue-600">
                      {child.badge}
                    </span>
                  )}
                </button>
              ))}
            </div>
          </div>
        )}
      </div>
    );
  };

  return (
    <aside
      className={`
        fixed left-0 top-0 h-screen z-50 transition-all duration-300 ease-in-out
        ${isCollapsed ? 'w-20' : 'w-72'}
        ${mobileOpen ? 'translate-x-0' : '-translate-x-full md:translate-x-0'}
      `}
    >
      {/* Background clean */}
      <div className="absolute inset-0 bg-[var(--surface)] text-[var(--text)]" />
      
      {/* Borda direita */}
      <div className="absolute right-0 top-0 bottom-0 w-px bg-[var(--border)]" />

      <div className="relative h-full flex flex-col">
        {/* Header / Logo */}
        <div className={`
          h-20 flex items-center border-b border-[var(--border)] px-4
          ${isCollapsed && !mobileOpen ? 'justify-center' : 'justify-between'}
        `}>
          {/* Botão fechar mobile */}
          {mobileOpen && (
            <button
              onClick={onMobileClose}
              className="md:hidden absolute right-4 top-6 p-2 rounded-lg text-[var(--text-muted)] hover:bg-[var(--surface-muted)] hover:text-[var(--text)] transition-colors"
            >
              <X className="w-5 h-5" />
            </button>
          )}
          
          {(!isCollapsed || mobileOpen) ? (
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 rounded-xl bg-[var(--surface)] flex items-center justify-center shadow-md overflow-hidden">
                <img 
                  src={branding.logo} 
                  alt={branding.nome}
                  className="w-9 h-9 object-contain"
                  onError={(e) => {
                    // Fallback para o ícone se a imagem não carregar
                    e.currentTarget.style.display = 'none';
                    e.currentTarget.parentElement!.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-6 h-6 text-blue-500"><path d="M12 2.69l5.66 5.66a8 8 0 1 1-11.31 0z"/></svg>';
                  }}
                />
              </div>
              <div className="flex flex-col">
                <span className="text-[var(--text)] font-bold text-sm leading-tight">
                  {branding.nome}
                </span>
              </div>
            </div>
          ) : (
            <div className="w-10 h-10 rounded-xl bg-[var(--surface)] flex items-center justify-center shadow-md overflow-hidden">
              <img 
                src={branding.logo} 
                alt={branding.nome}
                className="w-9 h-9 object-contain"
                onError={(e) => {
                  e.currentTarget.style.display = 'none';
                  e.currentTarget.parentElement!.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-6 h-6 text-blue-500"><path d="M12 2.69l5.66 5.66a8 8 0 1 1-11.31 0z"/></svg>';
                }}
              />
            </div>
          )}
        </div>

        {/* Label de seção - Mais acima */}
        {(!isCollapsed || mobileOpen) && (
          <div className="px-6 pt-4 pb-2">
            <span className="text-[11px] font-semibold uppercase tracking-wider text-[var(--text-muted)]">
              Menu Principal
            </span>
          </div>
        )}

        {/* Navigation */}
        <nav className="flex-1 overflow-y-auto px-3 pb-6 space-y-1 scrollbar-thin scrollbar-thumb-gray-200 scrollbar-track-transparent">
          {filteredMenuItems.map(item => renderMenuItem(item))}
        </nav>

        {/* Botão Recolher/Expandir na parte inferior - hidden on mobile */}
        <div className="border-t border-[var(--border)] p-3 hidden md:block">
          <button
            onClick={handleCollapse}
            className={`
              w-full flex items-center gap-3 px-3 py-2.5 rounded-lg transition-all duration-200
              text-[var(--text-muted)] hover:bg-[var(--surface-muted)] hover:text-[var(--text)]
              ${isCollapsed ? 'justify-center' : ''}
            `}
            title={isCollapsed ? 'Expandir menu' : 'Recolher menu'}
          >
            <div className="flex items-center justify-center w-8 h-8 rounded-lg bg-[var(--surface-muted)] text-[var(--text-muted)]">
              {isCollapsed ? (
                <ChevronRight className="w-[18px] h-[18px]" />
              ) : (
                <ChevronLeft className="w-[18px] h-[18px]" />
              )}
            </div>
            {!isCollapsed && (
              <span className="font-medium text-sm">Recolher menu</span>
            )}
          </button>
        </div>
      </div>
    </aside>
  );
}
