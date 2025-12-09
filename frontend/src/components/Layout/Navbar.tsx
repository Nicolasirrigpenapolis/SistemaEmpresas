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
  Maximize2,
  Minimize2,
  X,
  Key,
  Eye,
  EyeOff,
  Loader2,
  Menu,
} from 'lucide-react';
import { authService } from '../../services/authService';
import { APP_VERSION } from '../../config/version';

interface NavbarProps {
  sidebarCollapsed?: boolean;
  onMobileMenuToggle?: () => void;
}

// Função para capitalizar nome (Nome Sobrenome)
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
  // Padrão: tema claro (branco)
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
  const [isFullscreen, setIsFullscreen] = useState(false);
  const [currentTime, setCurrentTime] = useState(new Date());
  
  // Estados do modal de troca de senha
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

  // Aplica tema escolhido
  useEffect(() => {
    const theme = isDarkMode ? 'dark' : 'light';
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
  }, [isDarkMode]);

  // Atualiza o relógio a cada minuto
  useEffect(() => {
    const timer = setInterval(() => setCurrentTime(new Date()), 60000);
    return () => clearInterval(timer);
  }, []);

  // Listener para fullscreen
  useEffect(() => {
    const handleFullscreenChange = () => {
      setIsFullscreen(!!document.fullscreenElement);
    };
    document.addEventListener('fullscreenchange', handleFullscreenChange);
    return () => document.removeEventListener('fullscreenchange', handleFullscreenChange);
  }, []);

  // Atalho de teclado para busca (Ctrl+K)
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

  const toggleFullscreen = () => {
    if (!document.fullscreenElement) {
      document.documentElement.requestFullscreen();
    } else {
      document.exitFullscreen();
    }
  };

  const toggleDarkMode = () => {
    setIsDarkMode(!isDarkMode);
  };

  // Funções do modal de troca de senha
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

  // Formata a data e hora
  const formatDate = (date: Date) => {
    return date.toLocaleDateString('pt-BR', { weekday: 'short', day: '2-digit', month: 'short' });
  };
  
  const formatTime = (date: Date) => {
    return date.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
  };

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
        { title: 'Dados do Emitente', path: '/emitente', subtitle: 'Configuração fiscal' },
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
          <Search className="w-12 h-12 text-[var(--text-muted)] opacity-30 mx-auto mb-3" />
          <p className="text-sm font-medium text-[var(--text)]">Nenhum resultado encontrado</p>
          <p className="text-xs text-[var(--text-muted)] mt-1">Tente buscar por outro termo</p>
        </div>
      );
    }

    let paletteIndex = -1;

    if (normalizedQuery.length === 0) {
      return (
        <div className="py-2">
          <p className="text-[11px] font-semibold text-[var(--text-muted)] uppercase tracking-wider mb-1 px-4">Atalhos rápidos</p>
          <div className="divide-y divide-[var(--border)]">
            {quickActions.map(action => {
              paletteIndex += 1;
              const isActive = activeResultIndex === paletteIndex;
              return (
                <button
                  key={action.path}
                  onClick={() => handleSelectResult(action.path)}
                  onMouseEnter={() => setActiveResultIndex(paletteIndex)}
                  className={`w-full flex items-center justify-between px-4 py-2.5 transition-colors ${
                    isActive
                      ? 'bg-[var(--surface-muted)] ring-1 ring-[var(--accent)] ring-opacity-40'
                      : 'hover:bg-[var(--surface-muted)]'
                  }`}
                >
                  <div className="text-left">
                    <p className="text-sm text-[var(--text)] font-medium">{action.title}</p>
                    <p className="text-xs text-[var(--text-muted)]">{action.subtitle}</p>
                  </div>
                  <kbd className="px-2 py-0.5 text-[11px] font-semibold text-[var(--text-muted)] bg-[var(--surface-muted)] border border-[var(--border)] rounded">
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
            <p className="text-[11px] font-semibold text-[var(--text-muted)] uppercase tracking-wider mb-1 px-4">{section.title}</p>
            <div className="divide-y divide-[var(--border)]">
              {section.items.map(item => {
                paletteIndex += 1;
                const isActive = activeResultIndex === paletteIndex;
                return (
                  <button
                    key={item.path}
                    onClick={() => handleSelectResult(item.path)}
                    onMouseEnter={() => setActiveResultIndex(paletteIndex)}
                    className={`w-full flex items-center gap-3 px-4 py-2.5 transition-colors text-left ${
                      isActive
                        ? 'bg-[var(--surface-muted)] ring-1 ring-[var(--accent)] ring-opacity-40'
                        : 'hover:bg-[var(--surface-muted)]'
                    }`}
                  >
                    <Search className="w-4 h-4 text-[var(--text-muted)]" />
                    <div className="flex-1">
                      <p className="text-sm text-[var(--text)] font-medium">{item.title}</p>
                      <p className="text-xs text-[var(--text-muted)]">{item.subtitle}</p>
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
      {/* Navbar */}
      <header
        className={`h-16 md:h-20 bg-[var(--surface)] text-[var(--text)] border-b border-[var(--border)] fixed top-0 right-0 z-40 transition-all duration-300 shadow-sm ${
          sidebarCollapsed ? 'md:left-20 left-0' : 'md:left-72 left-0'
        }`}
      >
        <div className="h-full flex items-center justify-between px-4 md:px-6 lg:px-8">
          {/* Botão Menu Mobile */}
          <button
            onClick={onMobileMenuToggle}
            className="md:hidden p-2 -ml-2 rounded-lg text-[var(--text-muted)] hover:bg-[var(--surface-muted)] hover:text-[var(--text)] transition-colors"
          >
            <Menu className="w-6 h-6" />
          </button>

          {/* Lado Esquerdo - Busca e Data */}
          <div className="flex items-center gap-4 md:gap-8 flex-1 mr-4 md:mr-6">
            {/* Campo de Busca Inline */}
            <div className="relative flex-1 max-w-xl">
              <div 
                className={`flex items-center gap-2 md:gap-3 px-3 md:px-5 py-2 md:py-3 bg-[var(--surface-muted)] border border-[var(--border)] rounded-xl transition-all ${
                  showSearch 
                    ? 'ring-2 ring-[var(--accent)] ring-opacity-25 border-[var(--accent)] bg-[var(--surface)]' 
                    : 'hover:bg-[var(--surface)] hover:border-[var(--border)]'
                }`}
              >
                <Search className="w-4 h-4 md:w-5 md:h-5 text-[var(--text-muted)] flex-shrink-0" />
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
                  className="flex-1 bg-transparent text-sm md:text-base text-[var(--text)] placeholder:text-[var(--text-muted)] outline-none min-w-0"
                />
                {searchQuery && (
                  <button
                    onClick={() => {
                      setSearchQuery('');
                    }}
                    className="p-1 rounded-md hover:bg-[var(--surface-muted)] transition-colors"
                  >
                    <X className="w-4 h-4 text-[var(--text-muted)]" />
                  </button>
                )}
                <kbd className="hidden lg:inline-flex items-center gap-1.5 px-2.5 py-1 text-sm font-medium text-[var(--text-muted)] bg-[var(--surface-muted)] border border-[var(--border)] rounded-lg flex-shrink-0">
                  <span className="text-xs">Ctrl</span>
                  <span>K</span>
                </kbd>
              </div>

              {/* Dropdown de Resultados */}
              {showSearch && (
                <>
                  <div className="fixed inset-0 z-[60]" onClick={() => { setShowSearch(false); setSearchQuery(''); }} />
                  <div className="absolute top-full left-0 right-0 mt-2 bg-[var(--surface)] text-[var(--text)] rounded-xl shadow-xl border border-[var(--border)] z-[60] overflow-hidden">
                    <div className="max-h-96 overflow-y-auto">
                      {renderCommandPalette()}
                    </div>
                  </div>
                </>
              )}
            </div>
          </div>

          {/* Lado Direito - Ações */}
          <div className="flex items-center gap-2 flex-shrink-0">
            {/* Data e Hora */}
            <div className="hidden xl:flex items-center gap-2 text-sm text-[var(--text-muted)] px-3 py-2 bg-[var(--surface-muted)] border border-[var(--border)] rounded-lg">
              <Calendar className="w-4 h-4" />
              <span className="font-medium text-[var(--text)]">{formatDate(currentTime)}</span>
              <span className="text-[var(--text-muted)] opacity-60">•</span>
              <span className="font-medium text-[var(--text)]">{formatTime(currentTime)}</span>
            </div>

            {/* Separador */}
            <div className="w-px h-10 bg-[var(--border)] mx-1 hidden xl:block" />

            {/* Toggle Dark Mode */}
            <button
              onClick={toggleDarkMode}
              className="p-2.5 rounded-xl text-[var(--text-muted)] hover:bg-[var(--surface-muted)] hover:text-[var(--text)] transition-colors hidden md:flex border border-transparent hover:border-[var(--border)]"
              title={isDarkMode ? 'Modo claro' : 'Modo escuro'}
            >
              {isDarkMode ? <Sun className="w-5 h-5" /> : <Moon className="w-5 h-5" />}
            </button>

            {/* Fullscreen */}
            <button
              onClick={toggleFullscreen}
              className="p-2.5 rounded-xl text-[var(--text-muted)] hover:bg-[var(--surface-muted)] hover:text-[var(--text)] transition-colors hidden md:flex border border-transparent hover:border-[var(--border)]"
              title={isFullscreen ? 'Sair da tela cheia' : 'Tela cheia'}
            >
              {isFullscreen ? <Minimize2 className="w-5 h-5" /> : <Maximize2 className="w-5 h-5" />}
            </button>

            {/* Separador */}
            <div className="w-px h-10 bg-[var(--border)] mx-3 hidden md:block" />

            {/* Menu do usuário */}
            <div className="relative">
              <button
                onClick={() => {
                  setShowUserMenu(!showUserMenu);
                }}
                className="flex items-center gap-3 px-3 py-2 rounded-xl hover:bg-[var(--surface-muted)] transition-colors"
                aria-haspopup="true"
                aria-expanded={showUserMenu}
              >
                <div className="w-10 h-10 rounded-xl bg-gradient-to-br from-blue-600 to-purple-600 flex items-center justify-center shadow-sm">
                  <span className="text-sm font-bold text-white">
                    {capitalizeName(user?.nome).charAt(0)}
                  </span>
                </div>
                <div className="hidden md:block text-left">
                  <p className="text-sm font-medium text-[var(--text)] leading-tight">{capitalizeName(user?.nome)}</p>
                  <p className="text-xs text-[var(--text-muted)] leading-tight">{user?.grupo || 'Administrador'}</p>
                </div>
                <ChevronDown className="w-4 h-4 text-[var(--text-muted)] hidden md:block" />
              </button>
              {showUserMenu && (
                <>
                  <div className="fixed inset-0 z-[60]" onClick={() => setShowUserMenu(false)} />
                  <div className="absolute right-0 mt-2 w-64 bg-[var(--surface)] text-[var(--text)] rounded-xl shadow-xl border border-[var(--border)] z-[60] overflow-hidden">
                    <div className="px-4 py-4 border-b border-[var(--border)] bg-gradient-to-r from-blue-600 to-purple-600">
                      <div className="flex items-center gap-3">
                        <div className="w-12 h-12 rounded-xl bg-white/20 backdrop-blur flex items-center justify-center">
                          <span className="text-lg font-bold text-white">
                            {capitalizeName(user?.nome).charAt(0)}
                          </span>
                        </div>
                        <div>
                          <p className="text-sm font-semibold text-white">{capitalizeName(user?.nome)}</p>
                          <p className="text-xs text-white/80">{user?.grupo || 'Usuário'}</p>
                        </div>
                      </div>
                    </div>
                    <div className="py-2">
                      <button
                        onClick={() => {
                          setShowUserMenu(false);
                          setShowPasswordModal(true);
                          setPasswordError('');
                          setPasswordSuccess('');
                        }}
                        className="w-full px-4 py-2.5 text-left text-sm text-[var(--text)] hover:bg-[var(--surface-muted)] transition-colors flex items-center gap-3"
                      >
                        <Key className="w-4 h-4 text-[var(--text-muted)]" />
                        Trocar Senha
                      </button>
                    </div>
                    <div className="px-4 py-3 bg-[var(--surface-muted)] border-t border-[var(--border)] text-xs text-[var(--text-muted)]">
                      <p className="font-medium text-[var(--text)] mb-1">Versão do Sistema</p>
                      <p className="font-mono text-[var(--text-muted)]">v{APP_VERSION}</p>
                    </div>
                    <div className="border-t border-[var(--border)]">
                      <button
                        onClick={() => {
                          setShowUserMenu(false);
                          handleLogout();
                        }}
                        className="w-full px-4 py-3 text-left text-sm font-medium text-red-600 hover:bg-red-50 flex items-center gap-3 transition-colors"
                      >
                        <LogOut className="w-4 h-4" />
                        Sair do Sistema
                      </button>
                    </div>
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      </header>

      {/* Modal de Troca de Senha */}
      {showPasswordModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center">
          {/* Overlay */}
          <div 
            className="absolute inset-0 bg-black/50 backdrop-blur-sm"
            onClick={closePasswordModal}
          />
          
          {/* Modal */}
          <div className="relative bg-[var(--surface)] text-[var(--text)] rounded-2xl shadow-2xl w-full max-w-md mx-4 overflow-hidden">
            {/* Header */}
            <div className="bg-gradient-to-r from-blue-600 to-purple-600 px-6 py-4">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-xl bg-white/20 backdrop-blur flex items-center justify-center">
                    <Key className="w-5 h-5 text-white" />
                  </div>
                  <div>
                    <h2 className="text-lg font-semibold text-white">Trocar Senha</h2>
                    <p className="text-xs text-white/80">Altere sua senha de acesso</p>
                  </div>
                </div>
                <button
                  onClick={closePasswordModal}
                  className="p-2 rounded-lg hover:bg-white/20 transition-colors"
                >
                  <X className="w-5 h-5 text-white" />
                </button>
              </div>
            </div>

            {/* Body */}
            <div className="p-6 space-y-4">
              {/* Mensagens de erro/sucesso */}
              {passwordError && (
                <div className="p-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-600">
                  {passwordError}
                </div>
              )}
              {passwordSuccess && (
                <div className="p-3 bg-green-50 border border-green-200 rounded-lg text-sm text-green-600">
                  {passwordSuccess}
                </div>
              )}

              {/* Senha Atual */}
              <div>
                <label className="block text-sm font-medium text-[var(--text)] mb-1">
                  Senha Atual
                </label>
                <div className="relative">
                  <input
                    type={showSenhaAtual ? 'text' : 'password'}
                    value={senhaAtual}
                    onChange={(e) => setSenhaAtual(e.target.value)}
                    className="w-full px-4 py-2.5 border border-[var(--border)] bg-[var(--surface-muted)] text-[var(--text)] placeholder:text-[var(--text-muted)] rounded-lg focus:ring-2 focus:ring-[var(--accent)] focus:ring-opacity-50 focus:border-[var(--accent)] pr-10"
                    placeholder="Digite sua senha atual"
                    disabled={isChangingPassword}
                  />
                  <button
                    type="button"
                    onClick={() => setShowSenhaAtual(!showSenhaAtual)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-[var(--text-muted)]"
                  >
                    {showSenhaAtual ? <EyeOff className="w-5 h-5" /> : <Eye className="w-5 h-5" />}
                  </button>
                </div>
              </div>

              {/* Nova Senha */}
              <div>
                <label className="block text-sm font-medium text-[var(--text)] mb-1">
                  Nova Senha
                </label>
                <div className="relative">
                  <input
                    type={showSenhaNova ? 'text' : 'password'}
                    value={senhaNova}
                    onChange={(e) => setSenhaNova(e.target.value)}
                    className="w-full px-4 py-2.5 border border-[var(--border)] bg-[var(--surface-muted)] text-[var(--text)] placeholder:text-[var(--text-muted)] rounded-lg focus:ring-2 focus:ring-[var(--accent)] focus:ring-opacity-50 focus:border-[var(--accent)] pr-10"
                    placeholder="Digite a nova senha"
                    disabled={isChangingPassword}
                  />
                  <button
                    type="button"
                    onClick={() => setShowSenhaNova(!showSenhaNova)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-[var(--text-muted)]"
                  >
                    {showSenhaNova ? <EyeOff className="w-5 h-5" /> : <Eye className="w-5 h-5" />}
                  </button>
                </div>
              </div>

              {/* Confirmar Senha */}
              <div>
                <label className="block text-sm font-medium text-[var(--text)] mb-1">
                  Confirmar Nova Senha
                </label>
                <div className="relative">
                  <input
                    type={showConfirmarSenha ? 'text' : 'password'}
                    value={confirmarSenha}
                    onChange={(e) => setConfirmarSenha(e.target.value)}
                    className="w-full px-4 py-2.5 border border-[var(--border)] bg-[var(--surface-muted)] text-[var(--text)] placeholder:text-[var(--text-muted)] rounded-lg focus:ring-2 focus:ring-[var(--accent)] focus:ring-opacity-50 focus:border-[var(--accent)] pr-10"
                    placeholder="Confirme a nova senha"
                    disabled={isChangingPassword}
                  />
                  <button
                    type="button"
                    onClick={() => setShowConfirmarSenha(!showConfirmarSenha)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-[var(--text-muted)]"
                  >
                    {showConfirmarSenha ? <EyeOff className="w-5 h-5" /> : <Eye className="w-5 h-5" />}
                  </button>
                </div>
              </div>
            </div>

            {/* Footer */}
            <div className="px-6 py-4 bg-[var(--surface-muted)] border-t border-[var(--border)] flex justify-end gap-3">
              <button
                onClick={closePasswordModal}
                disabled={isChangingPassword}
                className="px-4 py-2 text-sm font-medium text-[var(--text)] hover:bg-[var(--surface)] rounded-lg transition-colors disabled:opacity-50"
              >
                Cancelar
              </button>
              <button
                onClick={handleChangePassword}
                disabled={isChangingPassword}
                className="px-4 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-lg transition-colors disabled:opacity-50 flex items-center gap-2"
              >
                {isChangingPassword ? (
                  <>
                    <Loader2 className="w-4 h-4 animate-spin" />
                    Alterando...
                  </>
                ) : (
                  'Alterar Senha'
                )}
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
