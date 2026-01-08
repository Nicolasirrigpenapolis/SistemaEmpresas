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
  Truck,
  Box,

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
        { id: 'entrada-estoque', label: 'Entrada de Estoque', path: '/estoque/entrada' },
        { id: 'movimento-contabil', label: 'Movimento Contábil', path: '/estoque/movimento-contabil' },
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
      id: 'transporte',
      label: 'Transporte',
      icon: Truck,
      children: [
        { id: 'veiculos', label: 'Veículos', path: '/transporte/veiculos' },
        { id: 'reboques', label: 'Reboques', path: '/transporte/reboques' },
        { id: 'motoristas', label: 'Motoristas', path: '/transporte/motoristas' },
        { id: 'viagens', label: 'Viagens', path: '/transporte/viagens' },
        { id: 'manutencoes', label: 'Manutenções', path: '/transporte/manutencoes' },
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
        { id: 'emitente', label: 'Configurações do Sistema', path: '/emitente' },
        { id: 'usuarios', label: 'Usuários e Permissões', path: '/usuarios' },
        { id: 'logs', label: 'Logs de Auditoria', path: '/logs' },
      ],
    },
  ];

  const filteredMenuItems = useMemo(() => {
    return menuItems
      .map(item => {
        if (item.path) {
          if (!podeVisualizarRota(user, item.path)) return null;
          return item;
        }
        if (item.children) {
          const filteredChildren = item.children.filter(child =>
            podeVisualizarRota(user, child.path)
          );
          if (filteredChildren.length === 0) return null;
          return { ...item, children: filteredChildren };
        }
        return item;
      })
      .filter((item): item is MenuItem => item !== null);
  }, [user]);

  const renderMenuItem = (item: MenuItem) => {
    const hasChildren = item.children && item.children.length > 0;
    const isExpanded = expandedMenus.includes(item.id);
    const active = isMenuActive(item);
    const Icon = item.icon;

    return (
      <div key={item.id} className="relative mb-1">
        <button
          onClick={() => hasChildren ? toggleSubmenu(item.id) : item.path && handleNavigate(item.path)}
          onMouseEnter={() => setHoveredItem(item.id)}
          onMouseLeave={() => setHoveredItem(null)}
          className={`
            w-full flex items-center gap-3 px-3 py-2.5 rounded-xl transition-all duration-200 group
            ${active
              ? 'bg-secondary/10 text-secondary'
              : 'text-muted-foreground hover:bg-surface-hover hover:text-primary'
            }
            ${isCollapsed ? 'justify-center' : ''}
          `}
        >
          <div className={`
            flex items-center justify-center w-9 h-9 rounded-lg transition-all duration-300
            ${active
              ? 'bg-secondary text-white shadow-lg shadow-secondary/30'
              : 'bg-surface-active/50 text-muted-foreground group-hover:bg-surface-active group-hover:text-primary'
            }
          `}>
            <Icon className="w-5 h-5" />
          </div>

          {!isCollapsed && (
            <>
              <span className="flex-1 text-left font-medium text-sm tracking-wide">{item.label}</span>

              {item.badge && (
                <span className={`
                  px-2 py-0.5 text-[10px] font-bold uppercase tracking-wider rounded-full text-white shadow-sm
                  ${item.badgeColor || 'bg-secondary'}
                `}>
                  {item.badge}
                </span>
              )}

              {hasChildren && (
                <ChevronDown className={`
                  w-4 h-4 text-muted-foreground/70 transition-transform duration-300
                  ${isExpanded ? 'rotate-180' : ''}
                `} />
              )}
            </>
          )}
        </button>

        {isCollapsed && hoveredItem === item.id && (
          <div className="absolute left-full top-1/2 -translate-y-1/2 ml-4 z-50 animate-fade-in">
            <div className="bg-primary text-primary-foreground text-sm font-medium px-4 py-2.5 rounded-xl shadow-xl whitespace-nowrap">
              {item.label}
              {item.badge && (
                <span className={`ml-2 px-1.5 py-0.5 text-xs rounded ${item.badgeColor || 'bg-secondary'}`}>
                  {item.badge}
                </span>
              )}
              <div className="absolute right-full top-1/2 -translate-y-1/2 border-8 border-transparent border-r-primary" />
            </div>
          </div>
        )}

        {hasChildren && !isCollapsed && (
          <div className={`
            overflow-hidden transition-all duration-300 ease-in-out
            ${isExpanded ? 'max-h-96 opacity-100' : 'max-h-0 opacity-0'}
          `}>
            <div className="ml-7 mt-1 pl-4 border-l border-border space-y-1">
              {item.children?.map(child => (
                <button
                  key={child.id}
                  onClick={() => handleNavigate(child.path)}
                  className={`
                    w-full flex items-center justify-between px-3 py-2 rounded-lg text-sm transition-all duration-200
                    ${isActive(child.path)
                      ? 'bg-secondary/5 text-secondary font-medium'
                      : 'text-muted-foreground hover:bg-surface-hover hover:text-primary'
                    }
                  `}
                >
                  <span>{child.label}</span>
                  {child.badge && (
                    <span className="px-2 py-0.5 text-[10px] font-bold rounded-full bg-secondary/10 text-secondary">
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
        ${isCollapsed ? 'w-24' : 'w-72'}
        ${mobileOpen ? 'translate-x-0' : '-translate-x-full md:translate-x-0'}
        bg-surface border-r border-border shadow-2xl shadow-black/5
      `}
    >
      <div className="relative h-full flex flex-col">
        {/* Header / Logo */}
        <div className={`
          h-24 flex items-center px-6
          ${isCollapsed && !mobileOpen ? 'justify-center' : 'justify-between'}
        `}>
          {mobileOpen && (
            <button
              onClick={onMobileClose}
              className="md:hidden absolute right-4 top-8 p-2 rounded-lg text-muted-foreground hover:bg-surface-hover transition-colors"
            >
              <X className="w-5 h-5" />
            </button>
          )}

          {(!isCollapsed || mobileOpen) ? (
            <div className="flex items-center gap-4 group cursor-pointer" onClick={() => navigate('/dashboard')}>
              <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-secondary to-accent flex items-center justify-center shadow-lg shadow-secondary/20 group-hover:scale-105 transition-transform duration-300">
                {branding.logo ? (
                  <img src={branding.logo} alt={branding.nome} className="w-8 h-8 object-contain brightness-0 invert" />
                ) : (
                  <Box className="w-6 h-6 text-white" />
                )}
              </div>
              <div className="flex flex-col">
                <span className="text-primary font-display font-bold text-lg leading-tight tracking-tight">
                  {branding.nome || 'Sistema'}
                </span>
              </div>
            </div>
          ) : (
            <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-secondary to-accent flex items-center justify-center shadow-lg shadow-secondary/20 cursor-pointer hover:scale-105 transition-transform duration-300" onClick={() => navigate('/dashboard')}>
              {branding.logo ? (
                <img src={branding.logo} alt={branding.nome} className="w-8 h-8 object-contain brightness-0 invert" />
              ) : (
                <Box className="w-6 h-6 text-white" />
              )}
            </div>
          )}
        </div>

        {/* Menu Label */}
        {(!isCollapsed || mobileOpen) && (
          <div className="px-6 pb-4">
            <span className="text-[10px] font-bold uppercase tracking-widest text-muted-foreground/60">
              Menu Principal
            </span>
          </div>
        )}

        {/* Navigation */}
        <nav className="flex-1 overflow-y-auto px-4 pb-6 space-y-1 custom-scrollbar">
          {filteredMenuItems.map(item => renderMenuItem(item))}
        </nav>

        {/* Footer Toggle */}
        <div className="p-4 hidden md:block">
          <button
            onClick={handleCollapse}
            className={`
              w-full flex items-center gap-3 px-3 py-3 rounded-xl transition-all duration-200
              border border-border hover:bg-surface-hover text-muted-foreground hover:text-primary
              ${isCollapsed ? 'justify-center' : ''}
            `}
          >
            {isCollapsed ? (
              <ChevronRight className="w-5 h-5" />
            ) : (
              <>
                <div className="flex items-center justify-center w-6 h-6 rounded bg-surface-active/50">
                  <ChevronLeft className="w-4 h-4" />
                </div>
                <span className="font-medium text-sm">Recolher Menu</span>
              </>
            )}
          </button>
        </div>
      </div>
    </aside>
  );
}
