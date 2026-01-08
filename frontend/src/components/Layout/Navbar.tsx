import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import {
  LogOut,
  ChevronDown,
  Search,
  Calendar,
  Moon,
  Sun,

  X,
  Key,
  Eye,
  EyeOff,
  Loader2,
  Menu,
  Bell
} from 'lucide-react';
import { authService } from '../../services/Auth/authService';
import { APP_VERSION } from '../../config/version';

interface NavbarProps {
  sidebarCollapsed?: boolean;
  onMobileMenuToggle?: () => void;
}

const capitalizeName = (name: string | undefined): string => {
  if (!name) return 'Usuário';
  return name
    .toLowerCase()
    .split(' ')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join(' ');
};

const getInitialTheme = () => {
  if (typeof window === 'undefined') return false;
  const stored = localStorage.getItem('theme');
  if (stored === 'dark') return true;
  if (stored === 'light') return false;
  return false;
};

export default function Navbar({ sidebarCollapsed = false, onMobileMenuToggle }: NavbarProps) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [showUserMenu, setShowUserMenu] = useState(false);
  const [showSearch, setShowSearch] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const searchInputRef = useRef<HTMLInputElement>(null);
  const [isDarkMode, setIsDarkMode] = useState(getInitialTheme);

  const [currentTime, setCurrentTime] = useState(new Date());

  // Password Modal State
  const [showPasswordModal, setShowPasswordModal] = useState(false);
  const [senhaAtual, setSenhaAtual] = useState('');
  const [senhaNova, setSenhaNova] = useState('');
  const [confirmarSenha, setConfirmarSenha] = useState('');
  const [showSenhaAtual, setShowSenhaAtual] = useState(false);
  const [showSenhaNova, setShowSenhaNova] = useState(false);
  const [showConfirmarSenha, setShowConfirmarSenha] = useState(false);
  const [isChangingPassword, setIsChangingPassword] = useState(false);
  const [passwordError, setPasswordError] = useState('');
  const [passwordSuccess, setPasswordSuccess] = useState('');
  const [activeResultIndex, setActiveResultIndex] = useState(0);

  useEffect(() => {
    const theme = isDarkMode ? 'dark' : 'light';
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
  }, [isDarkMode]);

  useEffect(() => {
    const timer = setInterval(() => setCurrentTime(new Date()), 60000);
    return () => clearInterval(timer);
  }, []);



  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
        e.preventDefault();
        setShowSearch(prev => !prev);
      }
      if (e.key === 'Escape') {
        setShowSearch(false);
        setSearchQuery('');
      }
    };
    document.addEventListener('keydown', handleKeyDown);
    return () => document.removeEventListener('keydown', handleKeyDown);
  }, []);

  useEffect(() => {
    setActiveResultIndex(0);
  }, [searchQuery, showSearch]);

  useEffect(() => {
    if (showSearch) {
      searchInputRef.current?.focus();
    }
  }, [showSearch]);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };



  const toggleDarkMode = () => {
    setIsDarkMode(!isDarkMode);
  };

  const closePasswordModal = () => {
    setShowPasswordModal(false);
    setSenhaAtual('');
    setSenhaNova('');
    setConfirmarSenha('');
    setShowSenhaAtual(false);
    setShowSenhaNova(false);
    setShowConfirmarSenha(false);
    setPasswordError('');
    setPasswordSuccess('');
  };

  const handleChangePassword = async () => {
    setPasswordError('');
    setPasswordSuccess('');
    setIsChangingPassword(true);

    try {
      await authService.alterarSenha(senhaAtual, senhaNova, confirmarSenha);
      setPasswordSuccess('Senha alterada com sucesso!');
      setTimeout(() => {
        closePasswordModal();
      }, 1500);
    } catch (error: any) {
      const message = error.response?.data?.message || error.response?.data?.mensagem || 'Erro ao alterar senha.';
      setPasswordError(message);
    } finally {
      setIsChangingPassword(false);
    }
  };

  const formatDate = (date: Date) => {
    return date.toLocaleDateString('pt-BR', { weekday: 'short', day: '2-digit', month: 'short' });
  };

  const formatTime = (date: Date) => {
    return date.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
  };

  // Search Logic (Simplified for brevity, same as before but styled)
  const quickActions = [
    { title: 'Novo Orçamento', path: '/orcamentos/novo', subtitle: 'Criar documento comercial', shortcut: 'O' },
    { title: 'Novo Pedido', path: '/pedidos/novo', subtitle: 'Registrar pedido de venda', shortcut: 'P' },
    { title: 'Novo Cliente', path: '/cadastros/geral/novo', subtitle: 'Cadastro rápido de cliente', shortcut: 'C' },
    { title: 'Nota Fiscal', path: '/faturamento/notas-fiscais/nova', subtitle: 'Emitir nota fiscal', shortcut: 'N' },
  ];

  const commandSections = [
    {
      title: 'Navegação',
      items: [
        { title: 'Dashboard', path: '/dashboard', subtitle: 'Visão geral do sistema' },
        { title: 'Orçamentos', path: '/orcamentos', subtitle: 'Pipeline comercial' },
        { title: 'Pedidos', path: '/pedidos', subtitle: 'Pedidos em andamento' },
        { title: 'Notas Fiscais', path: '/faturamento/notas-fiscais', subtitle: 'Faturamento e NF-e' },
        { title: 'Estoque', path: '/estoque', subtitle: 'Controle de estoque' },
        { title: 'Relatórios', path: '/relatorios', subtitle: 'Análises e relatórios' },
      ],
    },
    {
      title: 'Cadastros',
      items: [
        { title: 'Clientes', path: '/cadastros/clientes?tipo=cliente', subtitle: 'Base de clientes' },
        { title: 'Fornecedores', path: '/cadastros/fornecedores?tipo=fornecedor', subtitle: 'Base de fornecedores' },
        { title: 'Transportadoras', path: '/cadastros/transportadoras?tipo=transportadora', subtitle: 'Parceiros logísticos' },
        { title: 'Produtos', path: '/cadastros/produtos', subtitle: 'Catálogo de produtos' },
      ],
    },
    {
      title: 'Sistema',
      items: [
        { title: 'Usuários e Permissões', path: '/usuarios', subtitle: 'Gestão de acesso' },
        { title: 'Configurações do Sistema', path: '/emitente', subtitle: 'Configuração fiscal e parâmetros' },
        { title: 'Perfil', path: '/perfil', subtitle: 'Preferências do usuário' },
      ],
    },
  ];

  const normalizedQuery = searchQuery.trim().toLowerCase();
  const filteredSections = normalizedQuery.length === 0
    ? commandSections
    : commandSections
      .map(section => ({
        ...section,
        items: section.items.filter(item =>
          item.title.toLowerCase().includes(normalizedQuery) ||
          item.subtitle.toLowerCase().includes(normalizedQuery)
        ),
      }))
      .filter(section => section.items.length > 0);

  const flatResults: Array<{ title: string; path: string; subtitle: string; section?: string }> = normalizedQuery.length === 0
    ? quickActions
    : filteredSections.flatMap(section => section.items.map(item => ({ ...item, section: section.title })));

  const handleSelectResult = (path: string) => {
    navigate(path);
    setShowSearch(false);
    setSearchQuery('');
  };

  const handleSearchKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (!showSearch || flatResults.length === 0) return;
    if (e.key === 'ArrowDown') {
      e.preventDefault();
      setActiveResultIndex((prev) => Math.min(prev + 1, flatResults.length - 1));
    }
    if (e.key === 'ArrowUp') {
      e.preventDefault();
      setActiveResultIndex((prev) => Math.max(prev - 1, 0));
    }
    if (e.key === 'Enter') {
      const item = flatResults[activeResultIndex];
      if (item) {
        e.preventDefault();
        handleSelectResult(item.path);
      }
    }
  };

  const renderCommandPalette = () => {
    if (flatResults.length === 0) {
      return (
        <div className="p-8 text-center">
          <Search className="w-12 h-12 text-muted-foreground/30 mx-auto mb-3" />
          <p className="text-sm font-medium text-primary">Nenhum resultado encontrado</p>
          <p className="text-xs text-muted-foreground mt-1">Tente buscar por outro termo</p>
        </div>
      );
    }

    let paletteIndex = -1;

    if (normalizedQuery.length === 0) {
      return (
        <div className="py-2">
          <p className="text-[10px] font-bold text-muted-foreground uppercase tracking-wider mb-2 px-4">Atalhos rápidos</p>
          <div className="space-y-1 px-2">
            {quickActions.map(action => {
              paletteIndex += 1;
              const isActive = activeResultIndex === paletteIndex;
              return (
                <button
                  key={action.path}
                  onClick={() => handleSelectResult(action.path)}
                  onMouseEnter={() => setActiveResultIndex(paletteIndex)}
                  className={`w-full flex items-center justify-between px-3 py-2.5 rounded-lg transition-all ${isActive
                    ? 'bg-secondary/10 text-secondary'
                    : 'hover:bg-surface-hover text-muted-foreground'
                    }`}
                >
                  <div className="text-left">
                    <p className={`text-sm font-medium ${isActive ? 'text-secondary' : 'text-primary'}`}>{action.title}</p>
                    <p className="text-xs opacity-70">{action.subtitle}</p>
                  </div>
                  <kbd className="px-2 py-0.5 text-[10px] font-bold text-muted-foreground bg-surface border border-border rounded shadow-sm">
                    {action.shortcut}
                  </kbd>
                </button>
              );
            })}
          </div>
        </div>
      );
    }

    return (
      <div className="py-2 space-y-3">
        {filteredSections.map(section => (
          <div key={section.title}>
            <p className="text-[10px] font-bold text-muted-foreground uppercase tracking-wider mb-2 px-4">{section.title}</p>
            <div className="space-y-1 px-2">
              {section.items.map(item => {
                paletteIndex += 1;
                const isActive = activeResultIndex === paletteIndex;
                return (
                  <button
                    key={item.path}
                    onClick={() => handleSelectResult(item.path)}
                    onMouseEnter={() => setActiveResultIndex(paletteIndex)}
                    className={`w-full flex items-center gap-3 px-3 py-2.5 rounded-lg transition-all text-left ${isActive
                      ? 'bg-secondary/10 text-secondary'
                      : 'hover:bg-surface-hover text-muted-foreground'
                      }`}
                  >
                    <Search className="w-4 h-4 opacity-50" />
                    <div className="flex-1">
                      <p className={`text-sm font-medium ${isActive ? 'text-secondary' : 'text-primary'}`}>{item.title}</p>
                      <p className="text-xs opacity-70">{item.subtitle}</p>
                    </div>
                  </button>
                );
              })}
            </div>
          </div>
        ))}
      </div>
    );
  };

  return (
    <>
      <header
        className={`h-16 md:h-20 bg-surface/80 backdrop-blur-md border-b border-border fixed top-0 right-0 z-40 transition-all duration-300 ${sidebarCollapsed ? 'md:left-24 left-0' : 'md:left-72 left-0'
          }`}
      >
        <div className="h-full flex items-center justify-between px-3 sm:px-4 md:px-6">
          {/* Mobile Toggle */}
          <button
            onClick={onMobileMenuToggle}
            className="md:hidden p-2 -ml-2 rounded-xl text-muted-foreground hover:bg-surface-hover transition-colors touch-target"
            aria-label="Toggle menu"
          >
            <Menu className="w-6 h-6" />
          </button>

          {/* Search Bar */}
          <div className="flex items-center gap-4 md:gap-8 flex-1 mr-3 md:mr-6">
            <div className="relative flex-1 max-w-2xl">
              <div
                className={`flex items-center gap-3 px-4 py-2.5 bg-surface-active/30 border border-transparent rounded-2xl transition-all duration-200 ${showSearch
                  ? 'ring-2 ring-secondary/20 border-secondary bg-surface shadow-lg shadow-secondary/5'
                  : 'hover:bg-surface-active/50 hover:border-border'
                  }`}
              >
                <Search className={`w-5 h-5 transition-colors ${showSearch ? 'text-secondary' : 'text-muted-foreground'}`} />
                <input
                  type="text"
                  placeholder="Buscar..."
                  value={searchQuery}
                  onChange={(e) => {
                    setSearchQuery(e.target.value);
                    if (!showSearch) setShowSearch(true);
                  }}
                  onFocus={() => setShowSearch(true)}
                  onKeyDown={handleSearchKeyDown}
                  ref={searchInputRef}
                  className="flex-1 bg-transparent text-sm text-primary placeholder:text-muted-foreground outline-none min-w-0"
                />
                {searchQuery && (
                  <button
                    onClick={() => setSearchQuery('')}
                    className="p-1 rounded-full hover:bg-surface-active transition-colors"
                    aria-label="Limpar busca"
                  >
                    <X className="w-4 h-4 text-muted-foreground" />
                  </button>
                )}
                <kbd className="hidden md:inline-flex items-center gap-1 px-2 py-1 text-[10px] font-bold text-muted-foreground bg-surface border border-border rounded-lg shadow-sm">
                  <span className="text-xs">⌘</span>K
                </kbd>
              </div>

              {/* Search Results Dropdown */}
              {showSearch && (
                <>
                  <div className="fixed inset-0 z-[60]" onClick={() => { setShowSearch(false); setSearchQuery(''); }} />
                  <div className="absolute top-full left-0 right-0 mt-4 bg-surface text-primary rounded-2xl shadow-2xl shadow-black/20 border border-border z-[60] overflow-hidden animate-slide-up">
                    <div className="max-h-[32rem] overflow-y-auto custom-scrollbar">
                      {renderCommandPalette()}
                    </div>
                  </div>
                </>
              )}
            </div>
          </div>

          {/* Right Actions */}
          <div className="flex items-center gap-2 md:gap-3">
            {/* Date/Time - Hidden on mobile and tablet */}
            <div className="hidden xl:flex items-center gap-3 text-sm text-muted-foreground px-4 py-2 bg-surface-active/30 rounded-xl border border-transparent hover:border-border transition-all">
              <Calendar className="w-4 h-4 text-secondary" />
              <span className="font-medium text-primary">{formatDate(currentTime)}</span>
              <span className="opacity-30">|</span>
              <span className="font-medium text-primary">{formatTime(currentTime)}</span>
            </div>

            <div className="w-px h-8 bg-border mx-2 hidden xl:block" />

            {/* Theme Toggle */}
            <button
              onClick={toggleDarkMode}
              className="p-2.5 rounded-xl text-muted-foreground hover:bg-surface-hover hover:text-primary transition-all active:scale-95 touch-target"
              title={isDarkMode ? 'Modo claro' : 'Modo escuro'}
              aria-label={isDarkMode ? 'Ativar modo claro' : 'Ativar modo escuro'}
            >
              {isDarkMode ? <Sun className="w-5 h-5" /> : <Moon className="w-5 h-5" />}
            </button>

            {/* Notifications (Mock) - Hidden on small mobile */}
            <button
              className="hidden sm:block p-2.5 rounded-xl text-muted-foreground hover:bg-surface-hover hover:text-primary transition-all active:scale-95 relative touch-target"
              aria-label="Notificações"
            >
              <Bell className="w-5 h-5" />
              <span className="absolute top-2 right-2 w-2 h-2 bg-red-500 rounded-full border-2 border-surface" />
            </button>

            {/* User Menu */}
            <div className="relative ml-2">
              <button
                onClick={() => setShowUserMenu(!showUserMenu)}
                className="flex items-center gap-3 pl-2 pr-4 py-1.5 rounded-full hover:bg-surface-hover transition-all border border-transparent hover:border-border group"
              >
                <div className="w-9 h-9 rounded-full bg-gradient-to-br from-secondary to-accent flex items-center justify-center shadow-md shadow-secondary/20 ring-2 ring-surface group-hover:ring-secondary/20 transition-all">
                  <span className="text-sm font-bold text-white">
                    {capitalizeName(user?.nome).charAt(0)}
                  </span>
                </div>
                <div className="hidden md:block text-left">
                  <p className="text-sm font-semibold text-primary leading-none mb-1">{capitalizeName(user?.nome)}</p>
                  <p className="text-[10px] font-medium text-muted-foreground uppercase tracking-wider leading-none">{user?.grupo || 'Admin'}</p>
                </div>
                <ChevronDown className={`w-4 h-4 text-muted-foreground transition-transform duration-300 ${showUserMenu ? 'rotate-180' : ''}`} />
              </button>

              {showUserMenu && (
                <>
                  <div className="fixed inset-0 z-[60]" onClick={() => setShowUserMenu(false)} />
                  <div className="absolute right-0 mt-4 w-72 bg-surface text-primary rounded-2xl shadow-2xl shadow-black/20 border border-border z-[60] overflow-hidden animate-scale-in origin-top-right">
                    <div className="p-6 bg-gradient-to-br from-secondary/10 to-accent/10 border-b border-border">
                      <div className="flex items-center gap-4">
                        <div className="w-14 h-14 rounded-2xl bg-gradient-to-br from-secondary to-accent flex items-center justify-center shadow-lg shadow-secondary/20 text-white text-xl font-bold">
                          {capitalizeName(user?.nome).charAt(0)}
                        </div>
                        <div>
                          <p className="text-base font-bold text-primary">{capitalizeName(user?.nome)}</p>
                          <p className="text-xs font-medium text-muted-foreground">{user?.email || 'usuario@sistema.com'}</p>
                          <span className="inline-flex items-center px-2 py-0.5 mt-1 rounded text-[10px] font-bold bg-secondary/10 text-secondary uppercase tracking-wider">
                            {user?.grupo || 'Administrador'}
                          </span>
                        </div>
                      </div>
                    </div>

                    <div className="p-2">
                      <button
                        onClick={() => {
                          setShowUserMenu(false);
                          setShowPasswordModal(true);
                        }}
                        className="w-full px-4 py-3 text-left text-sm text-muted-foreground hover:text-primary hover:bg-surface-hover rounded-xl transition-all flex items-center gap-3 group"
                      >
                        <div className="p-2 rounded-lg bg-surface-active/50 text-muted-foreground group-hover:bg-secondary/10 group-hover:text-secondary transition-colors">
                          <Key className="w-4 h-4" />
                        </div>
                        <span className="font-medium">Alterar Senha</span>
                      </button>


                    </div>

                    <div className="p-2 border-t border-border bg-surface-active/30">
                      <button
                        onClick={() => {
                          setShowUserMenu(false);
                          handleLogout();
                        }}
                        className="w-full px-4 py-3 text-left text-sm font-medium text-red-500 hover:bg-red-500/10 rounded-xl flex items-center gap-3 transition-all group"
                      >
                        <LogOut className="w-4 h-4 group-hover:scale-110 transition-transform" />
                        Sair do Sistema
                      </button>
                    </div>

                    <div className="px-6 py-3 bg-surface border-t border-border text-[10px] font-medium text-muted-foreground flex justify-between items-center">
                      <span>Versão {APP_VERSION}</span>
                      <span className="w-2 h-2 rounded-full bg-green-500 shadow-[0_0_8px_rgba(34,197,94,0.5)]" />
                    </div>
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      </header>

      {/* Password Modal */}
      {showPasswordModal && (
        <div className="fixed inset-0 z-[70] flex items-center justify-center p-4">
          <div className="absolute inset-0 bg-background/80 backdrop-blur-sm" onClick={closePasswordModal} />
          <div className="relative bg-surface text-primary rounded-3xl shadow-2xl w-full max-w-md overflow-hidden animate-scale-in border border-border">
            <div className="bg-gradient-to-r from-secondary to-accent px-8 py-6">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-4">
                  <div className="w-12 h-12 rounded-2xl bg-white/20 backdrop-blur-md flex items-center justify-center shadow-inner">
                    <Key className="w-6 h-6 text-white" />
                  </div>
                  <div>
                    <h2 className="text-xl font-bold text-white">Trocar Senha</h2>
                    <p className="text-sm text-white/80 font-medium">Atualize suas credenciais</p>
                  </div>
                </div>
                <button onClick={closePasswordModal} className="p-2 rounded-xl hover:bg-white/20 transition-colors text-white">
                  <X className="w-5 h-5" />
                </button>
              </div>
            </div>

            <div className="p-8 space-y-5">
              {passwordError && (
                <div className="p-4 bg-red-500/10 border border-red-500/20 rounded-xl text-sm font-medium text-red-500 flex items-center gap-3">
                  <div className="w-1.5 h-1.5 rounded-full bg-red-500" />
                  {passwordError}
                </div>
              )}
              {passwordSuccess && (
                <div className="p-4 bg-green-500/10 border border-green-500/20 rounded-xl text-sm font-medium text-green-500 flex items-center gap-3">
                  <div className="w-1.5 h-1.5 rounded-full bg-green-500" />
                  {passwordSuccess}
                </div>
              )}

              <div className="space-y-4">
                {[
                  { label: 'Senha Atual', value: senhaAtual, setter: setSenhaAtual, show: showSenhaAtual, toggle: setShowSenhaAtual },
                  { label: 'Nova Senha', value: senhaNova, setter: setSenhaNova, show: showSenhaNova, toggle: setShowSenhaNova },
                  { label: 'Confirmar Nova Senha', value: confirmarSenha, setter: setConfirmarSenha, show: showConfirmarSenha, toggle: setShowConfirmarSenha }
                ].map((field, idx) => (
                  <div key={idx}>
                    <label className="block text-sm font-bold text-primary mb-2 ml-1">{field.label}</label>
                    <div className="relative group">
                      <input
                        type={field.show ? 'text' : 'password'}
                        value={field.value}
                        onChange={(e) => field.setter(e.target.value)}
                        className="w-full px-4 py-3 bg-surface-active/30 border border-border rounded-xl text-primary placeholder:text-muted-foreground focus:ring-2 focus:ring-secondary/50 focus:border-secondary transition-all outline-none"
                        placeholder="••••••••"
                        disabled={isChangingPassword}
                      />
                      <button
                        type="button"
                        onClick={() => field.toggle(!field.show)}
                        className="absolute right-3 top-1/2 -translate-y-1/2 p-2 text-muted-foreground hover:text-primary transition-colors"
                      >
                        {field.show ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </div>

            <div className="px-8 py-6 bg-surface-active/30 border-t border-border flex justify-end gap-3">
              <button
                onClick={closePasswordModal}
                disabled={isChangingPassword}
                className="px-6 py-2.5 text-sm font-bold text-muted-foreground hover:text-primary hover:bg-surface-active rounded-xl transition-all"
              >
                Cancelar
              </button>
              <button
                onClick={handleChangePassword}
                disabled={isChangingPassword}
                className="px-6 py-2.5 text-sm font-bold text-white bg-gradient-to-r from-secondary to-accent hover:shadow-lg hover:shadow-secondary/25 rounded-xl transition-all active:scale-95 disabled:opacity-50 disabled:active:scale-100 flex items-center gap-2"
              >
                {isChangingPassword ? <Loader2 className="w-4 h-4 animate-spin" /> : 'Confirmar Alteração'}
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
