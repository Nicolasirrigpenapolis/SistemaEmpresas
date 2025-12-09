import React, { useState, useEffect, useCallback, useMemo } from 'react';
import {
  Users,
  UserPlus,
  FolderPlus,
  Trash2,
  Save,
  X,
  RefreshCw,
  Search,
  Eye,
  EyeOff,
  Edit3,
  Check,
  AlertTriangle,
  User,
  Loader2,
  UserX,
  UserCheck,
  Shield,
  Key,
  Building2,
  Settings,
  ChevronDown,
  ChevronRight,
  LayoutDashboard,
  Database,
  Package,
  ShoppingCart,
  FileText,
  DollarSign,
  CheckSquare,
  MinusSquare,
  Sparkles,
  Lock,
  Unlock,
  Calendar,
  TrendingUp,
  Filter,
  Grid3X3,
  List,
  BadgeCheck,
  Activity,
  Layers,
  Zap,
  RotateCcw,
} from 'lucide-react';
import { usuarioManagementService } from '../../services/usuarioManagementService';
import permissoesTelaService from '../../services/permissoesTelaService';
import { ModalConfirmacao } from '../../components/common/ModalConfirmacao';
import { DropdownMenu } from '../../components/common/DropdownMenu';
import type {
  UsuarioListDto,
  UsuarioCreateDto,
  UsuarioUpdateDto,
  PermissoesUsuarioLogadoDto,
  GrupoUsuarioListDto,
  ModuloComTelasDto,
  PermissoesTelaListDto,
  PermissoesCompletasGrupoDto,
} from '../../types';

type TabType = 'usuarios' | 'grupos' | 'permissoes';

// Ícones por módulo com gradientes
const moduleIcons: Record<string, React.ReactNode> = {
  Dashboard: <LayoutDashboard className="w-4 h-4" />,
  Cadastros: <Database className="w-4 h-4" />,
  Estoque: <Package className="w-4 h-4" />,
  Comercial: <ShoppingCart className="w-4 h-4" />,
  Fiscal: <FileText className="w-4 h-4" />,
  Financeiro: <DollarSign className="w-4 h-4" />,
  Sistema: <Settings className="w-4 h-4" />,
};

// Cores para avatares
const avatarColors = [
  'from-violet-500 to-purple-600',
  'from-blue-500 to-cyan-500',
  'from-emerald-500 to-teal-500',
  'from-orange-500 to-amber-500',
  'from-pink-500 to-rose-500',
  'from-indigo-500 to-blue-500',
  'from-red-500 to-pink-500',
  'from-cyan-500 to-blue-500',
];

const getAvatarColor = (name: string) => {
  const index = name.charCodeAt(0) % avatarColors.length;
  return avatarColors[index];
};

export default function UsuariosPage() {
  // Permissões do usuário logado
  const [permissoes, setPermissoes] = useState<PermissoesUsuarioLogadoDto | null>(null);
  // Por padrão permitir todas as ações (proteção real está no backend)
  const podeInserir = permissoes?.isAdmin || permissoes?.telas?.['Usuarios']?.i || true;
  const podeAlterar = permissoes?.isAdmin || permissoes?.telas?.['Usuarios']?.a || true;
  const podeExcluir = permissoes?.isAdmin || permissoes?.telas?.['Usuarios']?.e || true;

  // Tab ativa
  const [tabAtiva, setTabAtiva] = useState<TabType>('usuarios');
  const [viewMode, setViewMode] = useState<'grid' | 'table'>('table');

  // Estado dos dados
  const [grupos, setGrupos] = useState<GrupoUsuarioListDto[]>([]);
  const [usuarios, setUsuarios] = useState<UsuarioListDto[]>([]);
  const [grupoSelecionado, setGrupoSelecionado] = useState<string | null>(null);
  const [usuarioSelecionado, setUsuarioSelecionado] = useState<UsuarioListDto | null>(null);

  // Estados de Permissões
  const [telasDisponiveis, setTelasDisponiveis] = useState<ModuloComTelasDto[]>([]);
  const [permissoesGrupo, setPermissoesGrupo] = useState<PermissoesCompletasGrupoDto | null>(null);
  const [permissoesEditadas, setPermissoesEditadas] = useState<Map<string, PermissoesTelaListDto>>(new Map());
  const [modulosExpandidos, setModulosExpandidos] = useState<Set<string>>(new Set(['Dashboard', 'Cadastros', 'Comercial', 'Fiscal', 'Sistema']));

  // Estado UI
  const [loading, setLoading] = useState(true);
  const [salvando, setSalvando] = useState(false);
  const [busca, setBusca] = useState('');
  const [filtroAtivo, setFiltroAtivo] = useState<'todos' | 'ativos' | 'inativos'>('todos');

  // Modais
  const [modalAberto, setModalAberto] = useState<'criar-grupo' | 'criar-usuario' | 'editar-usuario' | 'alterar-senha' | null>(null);
  const [novoGrupoNome, setNovoGrupoNome] = useState('');
  const [usuarioForm, setUsuarioForm] = useState<UsuarioCreateDto>({
    nome: '',
    senha: '',
    confirmarSenha: '',
    grupo: '',
    observacoes: '',
    ativo: true,
  });
  const [mostrarSenha, setMostrarSenha] = useState(false);

  // Toast
  const [toast, setToast] = useState<{ tipo: 'sucesso' | 'erro'; mensagem: string } | null>(null);

  // Modal de exclusão
  const [deleteModal, setDeleteModal] = useState<{
    open: boolean;
    tipo: 'usuario' | 'grupo';
    nome: string;
    deleting: boolean;
  }>({ open: false, tipo: 'usuario', nome: '', deleting: false });

  const mostrarToast = (tipo: 'sucesso' | 'erro', mensagem: string) => {
    setToast({ tipo, mensagem });
    setTimeout(() => setToast(null), 4000);
  };

  // Carregar dados
  const carregarDados = useCallback(async () => {
    try {
      setLoading(true);
      const [arvoreData, gruposData, perms, telasData] = await Promise.all([
        usuarioManagementService.listarArvore(),
        usuarioManagementService.listarGruposCompleto(),
        permissoesTelaService.obterMinhasPermissoes(),
        permissoesTelaService.listarTelasDisponiveis(),
      ]);

      setGrupos(gruposData);
      setPermissoes(perms);
      setTelasDisponiveis(telasData);

      // Flattening usuarios de todos os grupos
      const todosUsuarios: UsuarioListDto[] = [];
      arvoreData.forEach(g => {
        g.usuarios.forEach(u => {
          todosUsuarios.push({ ...u, grupo: g.nome });
        });
      });
      setUsuarios(todosUsuarios);
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      mostrarToast('erro', 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    carregarDados();
  }, [carregarDados]);

  // Carregar permissões do grupo
  const carregarPermissoesGrupo = useCallback(async (grupo: string) => {
    try {
      const data = await permissoesTelaService.obterPermissoesGrupo(grupo);
      setPermissoesGrupo(data);
      setPermissoesEditadas(new Map());
    } catch (error) {
      console.error('Erro ao carregar permissões:', error);
      mostrarToast('erro', 'Erro ao carregar permissões do grupo');
    }
  }, []);

  // Quando seleciona grupo na aba de permissões
  useEffect(() => {
    if (tabAtiva === 'permissoes' && grupoSelecionado) {
      carregarPermissoesGrupo(grupoSelecionado);
    }
  }, [tabAtiva, grupoSelecionado, carregarPermissoesGrupo]);

  // Filtrar usuários
  const usuariosFiltrados = usuarios.filter(u => {
    const matchBusca = u.nome.toLowerCase().includes(busca.toLowerCase()) ||
                       u.grupo.toLowerCase().includes(busca.toLowerCase());
    const matchAtivo = filtroAtivo === 'todos' || 
                       (filtroAtivo === 'ativos' && u.ativo) ||
                       (filtroAtivo === 'inativos' && !u.ativo);
    return matchBusca && matchAtivo;
  });

  // Contagens
  const contarUsuariosPorGrupo = (grupo: string) => usuarios.filter(u => u.grupo === grupo).length;
  const contarAtivos = usuarios.filter(u => u.ativo).length;
  const contarInativos = usuarios.filter(u => !u.ativo).length;

  // Função para abrir o modal de exclusão
  const abrirModalExclusao = (tipo: 'usuario' | 'grupo', nome: string) => {
    setDeleteModal({ open: true, tipo, nome, deleting: false });
  };

  // Função para confirmar a exclusão
  const confirmarExclusao = async () => {
    const { tipo, nome } = deleteModal;
    setDeleteModal(prev => ({ ...prev, deleting: true }));

    try {
      let resultado;
      if (tipo === 'usuario') {
        resultado = await usuarioManagementService.excluirUsuario(nome);
      } else {
        resultado = await usuarioManagementService.excluirGrupo(nome);
        if (resultado.sucesso && grupoSelecionado === nome) {
          setGrupoSelecionado(null);
        }
      }

      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem);
        await carregarDados();
      } else {
        mostrarToast('erro', resultado.mensagem);
      }
    } catch (error: any) {
      mostrarToast('erro', error.response?.data?.mensagem || `Erro ao excluir ${tipo}`);
    } finally {
      setDeleteModal({ open: false, tipo: 'usuario', nome: '', deleting: false });
    }
  };

  // === HANDLERS DE GRUPO ===
  const handleCriarGrupo = async () => {
    if (!novoGrupoNome.trim()) {
      mostrarToast('erro', 'Digite o nome do grupo');
      return;
    }
    try {
      setSalvando(true);
      const resultado = await usuarioManagementService.criarGrupo({ nome: novoGrupoNome.trim().toUpperCase() });
      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem);
        setNovoGrupoNome('');
        setModalAberto(null);
        await carregarDados();
      } else {
        mostrarToast('erro', resultado.mensagem);
      }
    } catch (error: any) {
      mostrarToast('erro', error.response?.data?.mensagem || 'Erro ao criar grupo');
    } finally {
      setSalvando(false);
    }
  };

  const handleExcluirGrupo = (nome: string) => {
    abrirModalExclusao('grupo', nome);
  };

  // === HANDLERS DE USUÁRIO ===
  const handleCriarUsuario = async () => {
    try {
      setSalvando(true);
      const resultado = await usuarioManagementService.criarUsuario(usuarioForm);
      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem);
        setUsuarioForm({ nome: '', senha: '', confirmarSenha: '', grupo: '', observacoes: '', ativo: true });
        setModalAberto(null);
        await carregarDados();
      } else {
        mostrarToast('erro', resultado.mensagem);
      }
    } catch (error: any) {
      mostrarToast('erro', error.response?.data?.mensagem || 'Erro ao criar usuário');
    } finally {
      setSalvando(false);
    }
  };

  const handleEditarUsuario = async () => {
    if (!usuarioSelecionado) return;
    try {
      setSalvando(true);
      const updateDto: UsuarioUpdateDto = {
        grupo: usuarioForm.grupo,
        observacoes: usuarioForm.observacoes,
        ativo: usuarioForm.ativo,
      };
      const resultado = await usuarioManagementService.atualizarUsuario(usuarioSelecionado.nome, updateDto);
      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem);
        setModalAberto(null);
        await carregarDados();
      } else {
        mostrarToast('erro', resultado.mensagem);
      }
    } catch (error: any) {
      mostrarToast('erro', error.response?.data?.mensagem || 'Erro ao atualizar usuário');
    } finally {
      setSalvando(false);
    }
  };

  const handleAlterarSenha = async () => {
    if (!usuarioSelecionado) return;
    try {
      setSalvando(true);
      const resultado = await usuarioManagementService.atualizarUsuario(usuarioSelecionado.nome, {
        novaSenha: usuarioForm.senha,
        confirmarNovaSenha: usuarioForm.confirmarSenha,
        grupo: usuarioSelecionado.grupo,
        ativo: usuarioSelecionado.ativo,
      });
      if (resultado.sucesso) {
        mostrarToast('sucesso', 'Senha alterada com sucesso');
        setUsuarioForm({ ...usuarioForm, senha: '', confirmarSenha: '' });
        setModalAberto(null);
      } else {
        mostrarToast('erro', resultado.mensagem);
      }
    } catch (error: any) {
      mostrarToast('erro', error.response?.data?.mensagem || 'Erro ao alterar senha');
    } finally {
      setSalvando(false);
    }
  };

  const handleExcluirUsuario = (nome: string) => {
    abrirModalExclusao('usuario', nome);
  };

  const handleToggleAtivo = async (usuario: UsuarioListDto) => {
    try {
      const novoStatus = !usuario.ativo;
      const resultado = await usuarioManagementService.atualizarUsuario(usuario.nome, {
        ativo: novoStatus,
        grupo: usuario.grupo || '',
      });
      
      if (resultado.sucesso) {
        mostrarToast('sucesso', `Usuário ${novoStatus ? 'ativado' : 'inativado'} com sucesso!`);
        await carregarDados();
      } else {
        mostrarToast('erro', resultado.mensagem || 'Erro ao alterar status do usuário');
      }
    } catch (error: any) {
      console.error('Erro ao alterar status do usuário:', error);
      const mensagem = error.response?.data?.mensagem || error.response?.data?.message || 'Erro ao alterar status do usuário';
      mostrarToast('erro', mensagem);
    }
  };

  const abrirEdicao = (usuario: UsuarioListDto) => {
    setUsuarioSelecionado(usuario);
    setUsuarioForm({
      nome: usuario.nome,
      senha: '',
      confirmarSenha: '',
      grupo: usuario.grupo,
      observacoes: usuario.observacoes || '',
      ativo: usuario.ativo,
    });
    setModalAberto('editar-usuario');
  };

  const abrirAlterarSenha = (usuario: UsuarioListDto) => {
    setUsuarioSelecionado(usuario);
    setUsuarioForm({ ...usuarioForm, senha: '', confirmarSenha: '' });
    setModalAberto('alterar-senha');
  };

  // === HANDLERS DE PERMISSÕES ===
  const toggleModulo = (nome: string) => {
    setModulosExpandidos(prev => {
      const novo = new Set(prev);
      if (novo.has(nome)) novo.delete(nome);
      else novo.add(nome);
      return novo;
    });
  };

  const getPermissao = (tela: string): PermissoesTelaListDto | null => {
    if (permissoesEditadas.has(tela)) {
      return permissoesEditadas.get(tela)!;
    }
    if (permissoesGrupo) {
      for (const modulo of permissoesGrupo.modulos) {
        const permissao = modulo.telas.find(t => t.tela === tela);
        if (permissao) return permissao;
      }
    }
    return null;
  };

  const alterarPermissao = (tela: string, campo: 'consultar' | 'incluir' | 'alterar' | 'excluir', valor: boolean) => {
    if (!permissoesGrupo || !grupoSelecionado) return;

    // PRIMEIRO verifica se já existe uma versão editada
    let permissaoAtual: PermissoesTelaListDto | undefined = permissoesEditadas.get(tela);
    
    // Se não existe versão editada, busca a original
    if (!permissaoAtual) {
      for (const modulo of permissoesGrupo.modulos) {
        permissaoAtual = modulo.telas.find(t => t.tela === tela);
        if (permissaoAtual) break;
      }
    }

    if (!permissaoAtual) {
      for (const modulo of telasDisponiveis) {
        const telaInfo = modulo.telas.find(t => t.tela === tela);
        if (telaInfo) {
          permissaoAtual = {
            id: 0,
            grupo: grupoSelecionado,
            modulo: telaInfo.modulo,
            tela: telaInfo.tela,
            nomeTela: telaInfo.nomeTela,
            rota: telaInfo.rota,
            consultar: false,
            incluir: false,
            alterar: false,
            excluir: false,
            ordem: telaInfo.ordem,
          };
          break;
        }
      }
    }

    if (!permissaoAtual) return;

    const novaPermissao = { ...permissaoAtual, [campo]: valor };

    if (campo === 'consultar' && !valor) {
      novaPermissao.incluir = false;
      novaPermissao.alterar = false;
      novaPermissao.excluir = false;
    }

    if (campo !== 'consultar' && valor) {
      novaPermissao.consultar = true;
    }

    setPermissoesEditadas(prev => {
      const novo = new Map(prev);
      novo.set(tela, novaPermissao);
      return novo;
    });
  };

  const salvarPermissoes = async () => {
    if (!grupoSelecionado || permissoesEditadas.size === 0) return;

    try {
      setSalvando(true);

      const permissoesList = Array.from(permissoesEditadas.values()).map(p => ({
        grupo: grupoSelecionado,
        modulo: p.modulo,
        tela: p.tela,
        nomeTela: p.nomeTela,
        rota: p.rota,
        consultar: p.consultar,
        incluir: p.incluir,
        alterar: p.alterar,
        excluir: p.excluir,
        ordem: p.ordem,
      }));

      const resultado = await permissoesTelaService.salvarPermissoes({
        grupo: grupoSelecionado,
        permissoes: permissoesList,
      });

      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem);
        await carregarPermissoesGrupo(grupoSelecionado);
      } else {
        mostrarToast('erro', resultado.mensagem);
      }
    } catch (error) {
      mostrarToast('erro', 'Erro ao salvar permissões');
    } finally {
      setSalvando(false);
    }
  };

  // Checkbox com 3 estados (todos, alguns, nenhum)
  const getModuloStatus = (modulo: string) => {
    const moduloData = telasDisponiveis.find(m => m.nome === modulo);
    if (!moduloData) return 'none';

    let total = 0;
    let marcados = 0;

    moduloData.telas.forEach(tela => {
      const perm = getPermissao(tela.tela);
      total += 4;
      if (perm?.consultar) marcados++;
      if (perm?.incluir) marcados++;
      if (perm?.alterar) marcados++;
      if (perm?.excluir) marcados++;
    });

    if (marcados === 0) return 'none';
    if (marcados === total) return 'all';
    return 'some';
  };

  // === FUNÇÕES DE ESTATÍSTICAS E AÇÕES EM MASSA ===
  
  // Calcula estatísticas de permissões
  const calcularEstatisticas = useMemo(() => {
    if (!grupoSelecionado) return { totalTelas: 0, telasComAcesso: 0, percentual: 0, totalPermissoes: 0, permissoesAtivas: 0 };
    
    let totalTelas = 0;
    let telasComAcesso = 0;
    let totalPermissoes = 0;
    let permissoesAtivas = 0;
    
    telasDisponiveis.forEach(modulo => {
      modulo.telas.forEach(tela => {
        totalTelas++;
        const perm = getPermissao(tela.tela);
        const temAlgumAcesso = perm?.consultar || perm?.incluir || perm?.alterar || perm?.excluir;
        if (temAlgumAcesso) telasComAcesso++;
        
        totalPermissoes += 4;
        if (perm?.consultar) permissoesAtivas++;
        if (perm?.incluir) permissoesAtivas++;
        if (perm?.alterar) permissoesAtivas++;
        if (perm?.excluir) permissoesAtivas++;
      });
    });
    
    const percentual = totalTelas > 0 ? Math.round((telasComAcesso / totalTelas) * 100) : 0;
    return { totalTelas, telasComAcesso, percentual, totalPermissoes, permissoesAtivas };
  }, [grupoSelecionado, telasDisponiveis, permissoesEditadas, permissoesGrupo]);

  // Liberar TODAS as permissões
  const liberarTudo = () => {
    if (!grupoSelecionado) return;
    
    setPermissoesEditadas(prev => {
      const novo = new Map(prev);
      telasDisponiveis.forEach(modulo => {
        modulo.telas.forEach(tela => {
          const permissaoExistente = getPermissao(tela.tela);
          const novaPermissao = {
            id: permissaoExistente?.id || 0,
            grupo: grupoSelecionado,
            modulo: tela.modulo,
            tela: tela.tela,
            nomeTela: tela.nomeTela,
            rota: tela.rota,
            consultar: true,
            incluir: true,
            alterar: true,
            excluir: true,
            ordem: tela.ordem,
          };
          novo.set(tela.tela, novaPermissao);
        });
      });
      return novo;
    });
  };

  // Apenas visualizar (somente consultar)
  const apenasVisualizar = () => {
    if (!grupoSelecionado) return;
    
    setPermissoesEditadas(prev => {
      const novo = new Map(prev);
      telasDisponiveis.forEach(modulo => {
        modulo.telas.forEach(tela => {
          const permissaoExistente = getPermissao(tela.tela);
          const novaPermissao = {
            id: permissaoExistente?.id || 0,
            grupo: grupoSelecionado,
            modulo: tela.modulo,
            tela: tela.tela,
            nomeTela: tela.nomeTela,
            rota: tela.rota,
            consultar: true,
            incluir: false,
            alterar: false,
            excluir: false,
            ordem: tela.ordem,
          };
          novo.set(tela.tela, novaPermissao);
        });
      });
      return novo;
    });
  };

  // Limpar TODAS as permissões (remover todos os acessos)
  const limparTudo = () => {
    if (!grupoSelecionado) return;
    
    setPermissoesEditadas(prev => {
      const novo = new Map(prev);
      telasDisponiveis.forEach(modulo => {
        modulo.telas.forEach(tela => {
          const permissaoExistente = getPermissao(tela.tela);
          const novaPermissao = {
            id: permissaoExistente?.id || 0,
            grupo: grupoSelecionado,
            modulo: tela.modulo,
            tela: tela.tela,
            nomeTela: tela.nomeTela,
            rota: tela.rota,
            consultar: false,
            incluir: false,
            alterar: false,
            excluir: false,
            ordem: tela.ordem,
          };
          novo.set(tela.tela, novaPermissao);
        });
      });
      return novo;
    });
  };

  // Selecionar/Desselecionar todas as permissões de um módulo
  const toggleModuloPermissoes = (nomeModulo: string, e: React.MouseEvent) => {
    e.stopPropagation(); // Não expandir/colapsar o módulo
    if (!grupoSelecionado) return;

    const modulo = telasDisponiveis.find(m => m.nome === nomeModulo);
    if (!modulo) return;

    // Verifica se todas as permissões do módulo já estão marcadas
    const todasMarcadas = modulo.telas.every(tela => {
      const perm = getPermissao(tela.tela);
      // Dashboard só tem consultar
      if (nomeModulo === 'Dashboard') {
        return perm?.consultar;
      }
      return perm?.consultar && perm?.incluir && perm?.alterar && perm?.excluir;
    });

    setPermissoesEditadas(prev => {
      const novo = new Map(prev);
      modulo.telas.forEach(tela => {
        const permissaoExistente = getPermissao(tela.tela);
        const isDashboard = nomeModulo === 'Dashboard';
        
        const novaPermissao = {
          id: permissaoExistente?.id || 0,
          grupo: grupoSelecionado,
          modulo: tela.modulo,
          tela: tela.tela,
          nomeTela: tela.nomeTela,
          rota: tela.rota,
          // Se todas estão marcadas, desmarca. Senão, marca todas.
          consultar: !todasMarcadas,
          incluir: isDashboard ? false : !todasMarcadas,
          alterar: isDashboard ? false : !todasMarcadas,
          excluir: isDashboard ? false : !todasMarcadas,
          ordem: tela.ordem,
        };
        novo.set(tela.tela, novaPermissao);
      });
      return novo;
    });
  };

  // Desfazer alterações pendentes
  const desfazerAlteracoes = () => {
    setPermissoesEditadas(new Map());
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-100">
        <div className="text-center">
          <div className="relative">
            <div className="w-20 h-20 rounded-full bg-gradient-to-r from-blue-500 to-indigo-600 flex items-center justify-center animate-pulse">
              <Shield className="w-10 h-10 text-white" />
            </div>
            <div className="absolute inset-0 w-20 h-20 rounded-full border-4 border-blue-200 border-t-blue-600 animate-spin" />
          </div>
          <p className="mt-4 text-lg font-medium text-gray-700">Carregando...</p>
          <p className="text-sm text-[var(--text-muted)]">Preparando módulo de usuários</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50/30 to-indigo-50/50">
      {/* Toast Notifications */}
      {toast && (
        <div className="fixed top-6 right-6 z-50 transform transition-all duration-500 ease-out animate-slideIn">
          <div
            className={`flex items-center gap-3 px-5 py-4 rounded-2xl shadow-2xl backdrop-blur-xl border ${
              toast.tipo === 'sucesso'
                ? 'bg-gradient-to-r from-emerald-500/90 to-green-500/90 border-emerald-400/30 text-white'
                : 'bg-gradient-to-r from-red-500/90 to-rose-500/90 border-red-400/30 text-white'
            }`}
          >
            <div className="p-2 rounded-xl bg-white/20">
              {toast.tipo === 'sucesso' ? <Check className="w-5 h-5" /> : <AlertTriangle className="w-5 h-5" />}
            </div>
            <span className="font-medium">{toast.mensagem}</span>
          </div>
        </div>
      )}

      {/* Header Premium */}
      <div className="sticky top-0 z-40 bg-white/80 backdrop-blur-xl border-b border-[var(--border)]/50 shadow-sm">
        <div className="max-w-[1600px] mx-auto px-4 md:px-6 py-4 md:py-5">
          <div className="flex items-center justify-between gap-4">
            <div className="flex items-center gap-3 md:gap-5">
              <div className="relative group">
                <div className="absolute inset-0 bg-gradient-to-r from-blue-600 to-indigo-600 rounded-xl md:rounded-2xl blur-xl opacity-40 group-hover:opacity-60 transition-opacity" />
                <div className="relative p-2.5 md:p-4 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-xl md:rounded-2xl shadow-xl">
                  <Shield className="w-5 h-5 md:w-8 md:h-8 text-white" />
                </div>
              </div>
              <div>
                <h1 className="text-xl md:text-3xl font-bold bg-gradient-to-r from-gray-900 via-gray-800 to-gray-900 bg-clip-text text-transparent">
                  Usuários & Permissões
                </h1>
                <p className="text-xs md:text-sm text-[var(--text-muted)] mt-1 hidden sm:flex items-center gap-2">
                  <Activity className="w-4 h-4 text-green-500" />
                  Gerencie acessos e controle de segurança
                </p>
              </div>
            </div>
            <div className="flex items-center gap-3">
              <button
                onClick={() => carregarDados()}
                className="group p-2.5 md:p-3 text-[var(--text-muted)] hover:text-blue-600 bg-[var(--surface)] hover:bg-blue-50 rounded-xl border border-[var(--border)] hover:border-blue-300 transition-all duration-300 shadow-sm hover:shadow-md"
                title="Atualizar"
              >
                <RefreshCw className="w-4 h-4 md:w-5 md:h-5 group-hover:rotate-180 transition-transform duration-500" />
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="max-w-[1600px] mx-auto px-4 md:px-6 py-4 md:py-8 space-y-4 md:space-y-8">
        {/* Stats Cards Premium */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-3 md:gap-6">
          <div className="group relative overflow-hidden bg-[var(--surface)] rounded-xl md:rounded-2xl shadow-sm hover:shadow-xl transition-all duration-500 border border-[var(--border)]">
            <div className="absolute inset-0 bg-gradient-to-br from-blue-500/5 to-indigo-500/5 opacity-0 group-hover:opacity-100 transition-opacity" />
            <div className="relative p-3 md:p-6">
              <div className="flex items-center justify-between gap-2">
                <div className="min-w-0">
                  <p className="text-[10px] md:text-sm font-medium text-[var(--text-muted)] uppercase tracking-wide truncate">Total Usuários</p>
                  <p className="text-2xl md:text-4xl font-bold text-[var(--text)] mt-1 md:mt-2">{usuarios.length}</p>
                </div>
                <div className="p-2.5 md:p-4 bg-gradient-to-br from-blue-100 to-blue-50 rounded-xl md:rounded-2xl group-hover:scale-110 transition-transform duration-500 flex-shrink-0">
                  <Users className="w-5 h-5 md:w-8 md:h-8 text-blue-600" />
                </div>
              </div>
              <div className="mt-2 md:mt-4 hidden sm:flex items-center gap-2 text-sm text-[var(--text-muted)]">
                <TrendingUp className="w-4 h-4 text-blue-500" />
                <span>Cadastrados no sistema</span>
              </div>
            </div>
          </div>

          <div className="group relative overflow-hidden bg-[var(--surface)] rounded-xl md:rounded-2xl shadow-sm hover:shadow-xl transition-all duration-500 border border-[var(--border)]">
            <div className="absolute inset-0 bg-gradient-to-br from-emerald-500/5 to-green-500/5 opacity-0 group-hover:opacity-100 transition-opacity" />
            <div className="relative p-3 md:p-6">
              <div className="flex items-center justify-between gap-2">
                <div className="min-w-0">
                  <p className="text-[10px] md:text-sm font-medium text-[var(--text-muted)] uppercase tracking-wide">Ativos</p>
                  <p className="text-2xl md:text-4xl font-bold text-emerald-600 mt-1 md:mt-2">{contarAtivos}</p>
                </div>
                <div className="p-2.5 md:p-4 bg-gradient-to-br from-emerald-100 to-green-50 rounded-xl md:rounded-2xl group-hover:scale-110 transition-transform duration-500 flex-shrink-0">
                  <UserCheck className="w-5 h-5 md:w-8 md:h-8 text-emerald-600" />
                </div>
              </div>
              <div className="mt-2 md:mt-4 hidden sm:flex items-center gap-2 text-sm text-[var(--text-muted)]">
                <UserCheck className="w-4 h-4 text-emerald-500" />
                <span>Com acesso ao sistema</span>
              </div>
            </div>
          </div>

          <div className="group relative overflow-hidden bg-[var(--surface)] rounded-xl md:rounded-2xl shadow-sm hover:shadow-xl transition-all duration-500 border border-[var(--border)]">
            <div className="absolute inset-0 bg-gradient-to-br from-red-500/5 to-rose-500/5 opacity-0 group-hover:opacity-100 transition-opacity" />
            <div className="relative p-3 md:p-6">
              <div className="flex items-center justify-between gap-2">
                <div className="min-w-0">
                  <p className="text-[10px] md:text-sm font-medium text-[var(--text-muted)] uppercase tracking-wide">Inativos</p>
                  <p className="text-2xl md:text-4xl font-bold text-red-600 mt-1 md:mt-2">{contarInativos}</p>
                </div>
                <div className="p-2.5 md:p-4 bg-gradient-to-br from-red-100 to-rose-50 rounded-xl md:rounded-2xl group-hover:scale-110 transition-transform duration-500 flex-shrink-0">
                  <UserX className="w-5 h-5 md:w-8 md:h-8 text-red-600" />
                </div>
              </div>
              <div className="mt-2 md:mt-4 hidden sm:flex items-center gap-2 text-sm text-[var(--text-muted)]">
                <Lock className="w-4 h-4 text-red-500" />
                <span>Acessos desabilitados</span>
              </div>
            </div>
          </div>

          <div className="group relative overflow-hidden bg-[var(--surface)] rounded-xl md:rounded-2xl shadow-sm hover:shadow-xl transition-all duration-500 border border-[var(--border)]">
            <div className="absolute inset-0 bg-gradient-to-br from-violet-500/5 to-purple-500/5 opacity-0 group-hover:opacity-100 transition-opacity" />
            <div className="relative p-3 md:p-6">
              <div className="flex items-center justify-between gap-2">
                <div className="min-w-0">
                  <p className="text-[10px] md:text-sm font-medium text-[var(--text-muted)] uppercase tracking-wide">Grupos</p>
                  <p className="text-2xl md:text-4xl font-bold text-violet-600 mt-1 md:mt-2">{grupos.length}</p>
                </div>
                <div className="p-2.5 md:p-4 bg-gradient-to-br from-violet-100 to-purple-50 rounded-xl md:rounded-2xl group-hover:scale-110 transition-transform duration-500 flex-shrink-0">
                  <Layers className="w-5 h-5 md:w-8 md:h-8 text-violet-600" />
                </div>
              </div>
              <div className="mt-2 md:mt-4 hidden sm:flex items-center gap-2 text-sm text-[var(--text-muted)]">
                <Layers className="w-4 h-4 text-violet-500" />
                <span>Níveis de acesso</span>
              </div>
            </div>
          </div>
        </div>

        {/* Main Content Card */}
        <div className="bg-[var(--surface)] rounded-2xl md:rounded-3xl shadow-sm border border-[var(--border)] overflow-hidden">
          <div className="border-b border-[var(--border)] bg-gradient-to-r from-gray-50/50 to-white">
            <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between px-4 md:px-6">
              <nav className="flex gap-1 overflow-x-auto scrollbar-hide -mx-4 px-4 sm:mx-0 sm:px-0">
                {[
                  { id: 'usuarios', label: 'Usuários', icon: Users, count: usuarios.length },
                  { id: 'grupos', label: 'Grupos', icon: Building2, count: grupos.length },
                  { id: 'permissoes', label: 'Permissões', icon: Shield, count: null },
                ].map((tab) => (
                  <button
                    key={tab.id}
                    onClick={() => setTabAtiva(tab.id as TabType)}
                    className={`relative px-4 md:px-6 py-4 md:py-5 text-xs md:text-sm font-medium transition-all duration-300 flex items-center gap-2 md:gap-3 whitespace-nowrap ${
                      tabAtiva === tab.id ? 'text-blue-600' : 'text-[var(--text-muted)] hover:text-gray-700'
                    }`}
                  >
                    <tab.icon className="w-4 h-4 md:w-5 md:h-5" />
                    <span>{tab.label}</span>
                    {tab.count !== null && (
                      <span
                        className={`px-2 py-0.5 md:px-2.5 md:py-1 rounded-full text-[10px] md:text-xs font-semibold transition-colors ${
                          tabAtiva === tab.id ? 'bg-blue-100 text-blue-700' : 'bg-gray-100 text-[var(--text-muted)]'
                        }`}
                      >
                        {tab.count}
                      </span>
                    )}
                    {tabAtiva === tab.id && (
                      <div className="absolute bottom-0 left-0 right-0 h-0.5 bg-gradient-to-r from-blue-500 to-indigo-500" />
                    )}
                  </button>
                ))}
              </nav>
              {tabAtiva === 'usuarios' && (
                <div className="hidden sm:flex items-center gap-1 p-1 bg-gray-100 rounded-xl my-2 sm:my-0">
                  <button
                    onClick={() => setViewMode('table')}
                    className={`p-2 rounded-lg transition-all ${
                      viewMode === 'table' ? 'bg-[var(--surface)] shadow-sm text-blue-600' : 'text-[var(--text-muted)] hover:text-gray-700'
                    }`}
                  >
                    <List className="w-5 h-5" />
                  </button>
                  <button
                    onClick={() => setViewMode('grid')}
                    className={`p-2 rounded-lg transition-all ${
                      viewMode === 'grid' ? 'bg-[var(--surface)] shadow-sm text-blue-600' : 'text-[var(--text-muted)] hover:text-gray-700'
                    }`}
                  >
                    <Grid3X3 className="w-5 h-5" />
                  </button>
                </div>
              )}
            </div>
          </div>

          {/* Tab Content */}
          <div className="p-6">
            {/* === TAB USUÁRIOS === */}
            {tabAtiva === 'usuarios' && (
              <div className="space-y-6">
                {/* Toolbar Premium */}
                <div className="flex items-center gap-4 flex-wrap">
                  <div className="relative flex-1 min-w-[280px]">
                    <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                      <Search className="w-5 h-5 text-gray-400" />
                    </div>
                    <input
                      type="text"
                      placeholder="Buscar por nome ou grupo..."
                      value={busca}
                      onChange={(e) => setBusca(e.target.value)}
                      className="w-full pl-12 pr-4 py-3 bg-[var(--surface-muted)] border border-transparent rounded-xl focus:ring-2 focus:ring-blue-500 focus:bg-[var(--surface)] focus:border-blue-200 transition-all duration-300 placeholder-gray-400 shadow-inner"
                    />
                  </div>

                  <div className="flex items-center gap-1 p-1.5 bg-gray-100 rounded-xl shadow-inner">
                    {[
                      { value: 'todos', label: 'Todos', count: usuarios.length },
                      { value: 'ativos', label: 'Ativos', count: contarAtivos },
                      { value: 'inativos', label: 'Inativos', count: contarInativos },
                    ].map((f) => (
                      <button
                        key={f.value}
                        onClick={() => setFiltroAtivo(f.value as any)}
                        className={`px-4 py-2 text-sm font-medium rounded-lg transition-all duration-300 flex items-center gap-2 ${
                          filtroAtivo === f.value 
                            ? 'bg-[var(--surface)] text-[var(--text)] shadow-sm' 
                            : 'text-[var(--text-muted)] hover:text-gray-700'
                        }`}
                      >
                        <span>{f.label}</span>
                        <span className="text-xs px-2 py-0.5 rounded-full bg-gray-200/70 text-[var(--text-muted)]">{f.count}</span>
                      </button>
                    ))}
                  </div>

                  <div className="flex items-center gap-3">
                    <div className="hidden md:flex items-center gap-2 px-3 py-2 rounded-xl border border-emerald-100 bg-emerald-50 text-emerald-700">
                      <BadgeCheck className="w-4 h-4" />
                      <span className="text-sm font-semibold">{contarAtivos} ativos</span>
                    </div>
                    <div className="hidden md:flex items-center gap-2 px-3 py-2 rounded-xl border border-amber-100 bg-amber-50 text-amber-700">
                      <UserX className="w-4 h-4" />
                      <span className="text-sm font-semibold">{contarInativos} inativos</span>
                    </div>
                    {podeInserir && (
                      <button
                        onClick={() => {
                          setUsuarioForm({ nome: '', senha: '', confirmarSenha: '', grupo: '', observacoes: '', ativo: true });
                          setModalAberto('criar-usuario');
                        }}
                        className="group flex items-center gap-2 px-5 py-3 bg-gradient-to-r from-blue-600 to-indigo-600 text-white rounded-xl hover:from-blue-700 hover:to-indigo-700 transition-all duration-300 shadow-lg shadow-blue-500/25 hover:shadow-xl hover:shadow-blue-500/30"
                      >
                        <UserPlus className="w-5 h-5 group-hover:scale-110 transition-transform" />
                        <span className="font-medium">Novo Usuário</span>
                      </button>
                    )}
                  </div>
                </div>

                <div className="flex flex-wrap items-center justify-between gap-3 px-4 py-3 bg-white/80 border border-[var(--border)] rounded-2xl shadow-sm">
                  <div className="flex items-center gap-3 text-sm text-[var(--text-muted)]">
                    <Sparkles className="w-4 h-4 text-blue-500" />
                    <span>Resultados refinados em tempo real.</span>
                    {busca && <span className="px-2 py-1 rounded-lg bg-blue-50 text-blue-700 text-xs">Busca: {busca}</span>}
                  </div>
                  <div className="flex items-center gap-2 text-xs text-[var(--text-muted)]">
                    <span className="px-2 py-1 rounded-full bg-gray-100 text-gray-700">Modo: {viewMode === 'grid' ? 'Cards' : 'Tabela'}</span>
                    <span className="px-2 py-1 rounded-full bg-gray-100 text-gray-700">Filtro: {filtroAtivo}</span>
                    <span className="px-2 py-1 rounded-full bg-gray-100 text-gray-700">Total: {usuariosFiltrados.length}</span>
                  </div>
                </div>

                {/* Grid ou Table View */}
                {viewMode === 'grid' ? (
                  /* Grid View - Cards de Usuários */
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                    {usuariosFiltrados.length === 0 ? (
                      <div className="col-span-full flex flex-col items-center justify-center py-16 text-[var(--text-muted)]">
                        <div className="w-20 h-20 bg-gray-100 rounded-full flex items-center justify-center mb-4">
                          <User className="w-10 h-10 text-gray-300" />
                        </div>
                        <p className="text-lg font-medium">Nenhum usuário encontrado</p>
                        <p className="text-sm text-gray-400">Tente ajustar os filtros de busca</p>
                      </div>
                    ) : (
                      usuariosFiltrados.map((usuario) => (
                        <div
                          key={usuario.nome}
                          className={`group relative bg-[var(--surface)] rounded-2xl border border-[var(--border)] p-5 hover:shadow-xl hover:border-blue-200 transition-all duration-300 ${!usuario.ativo ? 'opacity-70' : ''}`}
                        >
                          <div className="flex items-start justify-between">
                            <div className="flex items-center gap-4">
                              <div className={`relative w-14 h-14 rounded-2xl flex items-center justify-center text-white font-bold text-xl shadow-lg bg-gradient-to-br ${getAvatarColor(usuario.nome)}`}>
                                {usuario.nome.charAt(0).toUpperCase()}
                                <div className={`absolute -bottom-1 -right-1 w-4 h-4 rounded-full border-2 border-white ${usuario.ativo ? 'bg-emerald-500' : 'bg-gray-400'}`} />
                              </div>
                              <div>
                                <h3 className="font-semibold text-[var(--text)]">{usuario.nome}</h3>
                                <span className="inline-flex items-center gap-1 text-xs text-violet-600 bg-violet-50 px-2 py-0.5 rounded-full mt-1">
                                  <Shield className="w-3 h-3" />
                                  {usuario.grupo}
                                </span>
                              </div>
                            </div>
                            <DropdownMenu
                              triggerClassName="p-2 text-gray-400 hover:text-[var(--text-muted)] hover:bg-gray-100 rounded-lg transition-colors opacity-0 group-hover:opacity-100"
                              menuWidth="w-48"
                              items={[
                                ...(podeAlterar ? [
                                  {
                                    label: 'Editar',
                                    icon: <Edit3 className="w-4 h-4 text-[var(--text-muted)]" />,
                                    onClick: () => abrirEdicao(usuario),
                                  },
                                  {
                                    label: 'Alterar Senha',
                                    icon: <Key className="w-4 h-4 text-[var(--text-muted)]" />,
                                    onClick: () => abrirAlterarSenha(usuario),
                                  },
                                  {
                                    label: usuario.ativo ? 'Inativar' : 'Ativar',
                                    icon: usuario.ativo 
                                      ? <UserX className="w-4 h-4 text-orange-500" />
                                      : <UserCheck className="w-4 h-4 text-emerald-500" />,
                                    onClick: () => handleToggleAtivo(usuario),
                                  },
                                ] : []),
                                ...(podeExcluir ? [
                                  {
                                    label: 'Excluir',
                                    icon: <Trash2 className="w-4 h-4 text-red-500" />,
                                    textColor: 'text-red-600',
                                    dividerBefore: podeAlterar,
                                    onClick: () => handleExcluirUsuario(usuario.nome),
                                  },
                                ] : []),
                              ]}
                            />
                          </div>
                          {usuario.observacoes && (
                            <p className="mt-3 text-sm text-[var(--text-muted)] truncate">{usuario.observacoes}</p>
                          )}
                          <div className="mt-4 pt-4 border-t border-[var(--border)] flex items-center justify-between">
                            <button
                              onClick={() => podeAlterar && handleToggleAtivo(usuario)}
                              disabled={!podeAlterar}
                              className={`inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium transition-all ${
                                usuario.ativo
                                  ? 'bg-emerald-50 text-emerald-700 hover:bg-emerald-100'
                                  : 'bg-gray-100 text-[var(--text-muted)] hover:bg-gray-200'
                              } ${!podeAlterar ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'}`}
                            >
                              {usuario.ativo ? <><BadgeCheck className="w-3.5 h-3.5" /> Ativo</> : <><UserX className="w-3.5 h-3.5" /> Inativo</>}
                            </button>
                          </div>
                        </div>
                      ))
                    )}
                  </div>
                ) : (
                  /* Table View - Lista de Usuários */
                  <div className="overflow-hidden rounded-2xl border border-[var(--border)] bg-[var(--surface)] shadow-sm overflow-x-auto">
                    <table className="w-full min-w-[720px]">
                      <thead className="bg-gradient-to-r from-gray-50 to-gray-100/60 sticky top-0 z-10">
                        <tr>
                          <th className="text-left px-6 py-4 text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">Usuário</th>
                          <th className="text-left px-6 py-4 text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">Grupo</th>
                          <th className="text-left px-6 py-4 text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider hidden lg:table-cell">Observações</th>
                          <th className="text-center px-6 py-4 text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">Status</th>
                          <th className="text-center px-6 py-4 text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider w-24">Ações</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-gray-100">
                        {usuariosFiltrados.length === 0 ? (
                          <tr>
                            <td colSpan={5} className="px-6 py-16 text-center">
                              <div className="flex flex-col items-center">
                                <div className="w-20 h-20 bg-gray-100 rounded-full flex items-center justify-center mb-4">
                                  <User className="w-10 h-10 text-gray-300" />
                                </div>
                                <p className="text-lg font-medium text-[var(--text)]">Nenhum usuário encontrado</p>
                                <p className="text-sm text-[var(--text-muted)] mt-1">Tente ajustar os filtros de busca</p>
                              </div>
                            </td>
                          </tr>
                        ) : (
                          usuariosFiltrados.map((usuario, index) => (
                            <tr key={usuario.nome} className="group hover:bg-blue-50/50 transition-colors">
                              <td className="px-6 py-4">
                                <div className="flex items-center gap-4">
                                  <div className={`relative w-12 h-12 rounded-xl flex items-center justify-center text-white font-semibold text-lg shadow-md bg-gradient-to-br ${getAvatarColor(usuario.nome)} transform group-hover:scale-105 transition-transform`}>
                                    {usuario.nome.charAt(0).toUpperCase()}
                                  </div>
                                  <div>
                                    <p className="font-semibold text-[var(--text)]">{usuario.nome}</p>
                                    <p className="text-xs text-[var(--text-muted)] flex items-center gap-1 mt-0.5">
                                      <Calendar className="w-3 h-3" />
                                      Usuário #{index + 1}
                                    </p>
                                  </div>
                                </div>
                              </td>
                              <td className="px-6 py-4">
                                <span className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium bg-gradient-to-r from-violet-50 to-purple-50 text-violet-700 border border-violet-100">
                                  {usuario.grupo}
                                </span>
                              </td>
                              <td className="px-6 py-4 hidden lg:table-cell">
                                <span className="text-sm text-[var(--text-muted)] truncate max-w-[200px] block">
                                  {usuario.observacoes || <span className="text-gray-400 italic">Sem observações</span>}
                                </span>
                              </td>
                              <td className="px-6 py-4 text-center">
                                <button
                                  onClick={() => podeAlterar && handleToggleAtivo(usuario)}
                                  disabled={!podeAlterar}
                                  className={`inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full text-xs font-semibold transition-all duration-300 ${
                                    usuario.ativo
                                      ? 'bg-gradient-to-r from-emerald-50 to-green-50 text-emerald-700 border border-emerald-200 hover:shadow-md hover:shadow-emerald-100'
                                      : 'bg-gradient-to-r from-red-50 to-rose-50 text-red-700 border border-red-200 hover:shadow-md hover:shadow-red-100'
                                  } ${!podeAlterar ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'}`}
                                >
                                  {usuario.ativo ? <><UserCheck className="w-3.5 h-3.5" /> Ativo</> : <><UserX className="w-3.5 h-3.5" /> Inativo</>}
                                </button>
                              </td>
                              <td className="px-6 py-4 text-center">
                                <DropdownMenu
                                  triggerClassName="p-2 text-gray-400 hover:text-[var(--text-muted)] hover:bg-[var(--surface)] rounded-lg transition-all shadow-sm opacity-0 group-hover:opacity-100"
                                  items={[
                                    ...(podeAlterar ? [
                                      {
                                        label: 'Editar',
                                        sublabel: 'Alterar dados',
                                        icon: <Edit3 className="w-4 h-4 text-blue-600" />,
                                        iconBgColor: 'bg-blue-100',
                                        onClick: () => abrirEdicao(usuario),
                                      },
                                      {
                                        label: 'Alterar Senha',
                                        sublabel: 'Nova senha',
                                        icon: <Key className="w-4 h-4 text-amber-600" />,
                                        iconBgColor: 'bg-amber-100',
                                        onClick: () => abrirAlterarSenha(usuario),
                                      },
                                      {
                                        label: usuario.ativo ? 'Inativar' : 'Ativar',
                                        sublabel: usuario.ativo ? 'Desabilitar acesso' : 'Habilitar acesso',
                                        icon: usuario.ativo 
                                          ? <UserX className="w-4 h-4 text-orange-600" />
                                          : <UserCheck className="w-4 h-4 text-emerald-600" />,
                                        iconBgColor: usuario.ativo ? 'bg-orange-100' : 'bg-emerald-100',
                                        onClick: () => handleToggleAtivo(usuario),
                                      },
                                    ] : []),
                                    ...(podeExcluir ? [
                                      {
                                        label: 'Excluir',
                                        sublabel: 'Remover usuário',
                                        icon: <Trash2 className="w-4 h-4 text-red-600" />,
                                        iconBgColor: 'bg-red-100',
                                        textColor: 'text-red-600',
                                        dividerBefore: podeAlterar,
                                        onClick: () => handleExcluirUsuario(usuario.nome),
                                      },
                                    ] : []),
                                  ]}
                                />
                              </td>
                            </tr>
                          ))
                        )}
                      </tbody>
                    </table>
                  </div>
                )}

                {/* Footer */}
                <div className="flex items-center justify-between text-sm text-[var(--text-muted)] pt-2">
                  <span className="flex items-center gap-2">
                    <Filter className="w-4 h-4" />
                    Mostrando {usuariosFiltrados.length} de {usuarios.length} usuários
                  </span>
                  <button
                    onClick={() => carregarDados()}
                    className="inline-flex items-center gap-2 px-3 py-2 rounded-lg border border-[var(--border)] text-[var(--text-muted)] hover:text-blue-600 hover:border-blue-200 transition-colors bg-[var(--surface)]"
                  >
                    <RefreshCw className="w-4 h-4" />
                    Atualizar lista
                  </button>
                </div>
              </div>
            )}

            {/* === TAB GRUPOS === */}
            {tabAtiva === 'grupos' && (
              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <p className="text-[var(--text-muted)]">Gerencie os grupos de acesso do sistema</p>
                  {podeInserir && (
                    <button
                      onClick={() => setModalAberto('criar-grupo')}
                      className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
                    >
                      <FolderPlus className="w-4 h-4" />
                      Novo Grupo
                    </button>
                  )}
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                  {grupos.map(grupo => (
                    <div key={grupo.nome} className="bg-[var(--surface-muted)] rounded-lg border border-[var(--border)] p-4 hover:shadow-md transition-shadow">
                      <div className="flex items-start justify-between">
                        <div className="flex items-center gap-3">
                          <div className="p-2 bg-purple-100 rounded-lg">
                            <Shield className="w-6 h-6 text-purple-600" />
                          </div>
                          <div>
                            <h3 className="font-semibold text-[var(--text)]">{grupo.nome}</h3>
                            <p className="text-sm text-[var(--text-muted)]">
                              {contarUsuariosPorGrupo(grupo.nome)} usuário(s)
                            </p>
                          </div>
                        </div>
                        {podeExcluir && (
                          <button
                            onClick={() => handleExcluirGrupo(grupo.nome)}
                            className="p-1.5 text-gray-400 hover:text-red-500 hover:bg-red-50 rounded transition-colors"
                            title="Excluir grupo"
                          >
                            <Trash2 className="w-4 h-4" />
                          </button>
                        )}
                      </div>

                      {/* Lista de usuários do grupo */}
                      <div className="mt-3 pt-3 border-t border-[var(--border)]">
                        <div className="flex flex-wrap gap-1">
                          {usuarios.filter(u => u.grupo === grupo.nome).slice(0, 5).map(u => (
                            <span key={u.nome} className={`text-xs px-2 py-0.5 rounded ${u.ativo ? 'bg-green-100 text-green-700' : 'bg-gray-200 text-[var(--text-muted)]'}`}>
                              {u.nome}
                            </span>
                          ))}
                          {contarUsuariosPorGrupo(grupo.nome) > 5 && (
                            <span className="text-xs px-2 py-0.5 rounded bg-gray-200 text-[var(--text-muted)]">
                              +{contarUsuariosPorGrupo(grupo.nome) - 5}
                            </span>
                          )}
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {/* === TAB PERMISSÕES === */}
            {tabAtiva === 'permissoes' && (
            <div className="space-y-4">
              {/* Header com seletor e ações */}
              <div className="flex flex-wrap items-center gap-4">
                <div className="flex-1 min-w-[200px]">
                  <label className="block text-sm font-medium text-gray-700 mb-1">Selecione o Grupo</label>
                  <select
                    value={grupoSelecionado || ''}
                    onChange={(e) => setGrupoSelecionado(e.target.value || null)}
                    className="w-full max-w-xs px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="">Selecione um grupo...</option>
                    {grupos.map(g => (
                      <option key={g.nome} value={g.nome}>{g.nome}</option>
                    ))}
                  </select>
                </div>
              </div>

              {!grupoSelecionado ? (
                <div className="text-center py-12 text-[var(--text-muted)]">
                  <Shield className="w-16 h-16 mx-auto mb-4 text-gray-300" />
                  <p className="text-lg font-medium">Selecione um grupo</p>
                  <p className="text-sm">Escolha um grupo para configurar suas permissões</p>
                </div>
              ) : (
                <>
                  {/* Painel de estatísticas e ações em massa */}
                  <div className="bg-gradient-to-r from-slate-50 to-blue-50 rounded-xl p-4 border border-[var(--border)]">
                    <div className="flex flex-wrap items-center gap-6">
                      {/* Círculo de progresso */}
                      <div className="flex items-center gap-4">
                        <div className="relative">
                          <svg className="w-20 h-20 transform -rotate-90">
                            <circle
                              cx="40"
                              cy="40"
                              r="32"
                              stroke="#e5e7eb"
                              strokeWidth="8"
                              fill="none"
                            />
                            <circle
                              cx="40"
                              cy="40"
                              r="32"
                              stroke={calcularEstatisticas.percentual >= 75 ? '#10b981' : calcularEstatisticas.percentual >= 50 ? '#f59e0b' : '#3b82f6'}
                              strokeWidth="8"
                              fill="none"
                              strokeDasharray={`${calcularEstatisticas.percentual * 2.01} 201`}
                              strokeLinecap="round"
                              className="transition-all duration-500"
                            />
                          </svg>
                          <div className="absolute inset-0 flex items-center justify-center">
                            <span className="text-lg font-bold text-gray-800">{calcularEstatisticas.percentual}%</span>
                          </div>
                        </div>
                        <div>
                          <p className="text-sm font-medium text-gray-700">Acesso às Telas</p>
                          <p className="text-xs text-[var(--text-muted)]">{calcularEstatisticas.telasComAcesso} de {calcularEstatisticas.totalTelas} telas</p>
                          <p className="text-xs text-gray-400">{calcularEstatisticas.permissoesAtivas} de {calcularEstatisticas.totalPermissoes} permissões</p>
                        </div>
                      </div>

                      {/* Separador */}
                      <div className="hidden sm:block w-px h-16 bg-gray-300" />

                      {/* Ações rápidas em massa */}
                      <div className="flex-1">
                        <p className="text-sm font-medium text-gray-700 mb-2">Ações Rápidas</p>
                        <div className="flex flex-wrap gap-2">
                          <button
                            onClick={liberarTudo}
                            className="flex items-center gap-1.5 px-3 py-1.5 bg-emerald-100 text-emerald-700 rounded-lg hover:bg-emerald-200 transition-colors text-sm font-medium"
                            title="Liberar todas as permissões"
                          >
                            <Unlock className="w-4 h-4" />
                            Liberar Tudo
                          </button>
                          <button
                            onClick={apenasVisualizar}
                            className="flex items-center gap-1.5 px-3 py-1.5 bg-blue-100 text-blue-700 rounded-lg hover:bg-blue-200 transition-colors text-sm font-medium"
                            title="Apenas permissão de consulta"
                          >
                            <Eye className="w-4 h-4" />
                            Apenas Visualizar
                          </button>
                          <button
                            onClick={limparTudo}
                            className="flex items-center gap-1.5 px-3 py-1.5 bg-red-100 text-red-700 rounded-lg hover:bg-red-200 transition-colors text-sm font-medium"
                            title="Remover todas as permissões"
                          >
                            <Lock className="w-4 h-4" />
                            Limpar Tudo
                          </button>
                          {permissoesEditadas.size > 0 && (
                            <button
                              onClick={desfazerAlteracoes}
                              className="flex items-center gap-1.5 px-3 py-1.5 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors text-sm font-medium"
                              title="Desfazer alterações não salvas"
                            >
                              <RotateCcw className="w-4 h-4" />
                              Desfazer ({permissoesEditadas.size})
                            </button>
                          )}
                        </div>
                      </div>
                    </div>
                  </div>

                  {/* Tabela de permissões */}
                  <div className="border border-[var(--border)] rounded-lg overflow-hidden">
                    {/* Header da tabela */}
                    <div className="bg-gray-100 border-b border-[var(--border)] grid gap-2 px-4 py-3" style={{ gridTemplateColumns: '1fr 80px 80px 80px 80px' }}>
                      <div className="text-xs font-semibold text-[var(--text-muted)] uppercase">Módulo / Tela</div>
                      <div className="text-xs font-semibold text-[var(--text-muted)] uppercase text-center">Consultar</div>
                      <div className="text-xs font-semibold text-[var(--text-muted)] uppercase text-center">Incluir</div>
                      <div className="text-xs font-semibold text-[var(--text-muted)] uppercase text-center">Alterar</div>
                      <div className="text-xs font-semibold text-[var(--text-muted)] uppercase text-center">Excluir</div>
                    </div>

                    {/* Lista de módulos e telas */}
                    <div className="divide-y divide-gray-100">
                      {telasDisponiveis.map(modulo => {
                        const moduloStatus = getModuloStatus(modulo.nome);
                        const isExpanded = modulosExpandidos.has(modulo.nome);

                        return (
                          <div key={modulo.nome}>
                            {/* Módulo Header */}
                            <div 
                              className="bg-[var(--surface-muted)] grid gap-2 px-4 py-2 cursor-pointer hover:bg-gray-100"
                              style={{ gridTemplateColumns: '1fr 80px 80px 80px 80px' }}
                              onClick={() => toggleModulo(modulo.nome)}
                            >
                              <div className="flex items-center gap-2 font-medium text-[var(--text)]">
                                {isExpanded ? <ChevronDown className="w-4 h-4" /> : <ChevronRight className="w-4 h-4" />}
                                {moduleIcons[modulo.nome] || <Settings className="w-4 h-4" />}
                                <span>{modulo.nome}</span>
                                <span className="text-xs text-[var(--text-muted)]">({modulo.telas.length})</span>
                                <button
                                  onClick={(e) => toggleModuloPermissoes(modulo.nome, e)}
                                  className={`ml-2 p-0.5 rounded transition-colors ${
                                    moduloStatus === 'all' 
                                      ? 'text-emerald-600 hover:bg-emerald-100' 
                                      : moduloStatus === 'some'
                                      ? 'text-amber-500 hover:bg-amber-100'
                                      : 'text-gray-400 hover:bg-gray-200'
                                  }`}
                                  title={moduloStatus === 'all' ? 'Desmarcar todas as permissões deste módulo' : 'Marcar todas as permissões deste módulo'}
                                >
                                  {moduloStatus === 'all' ? (
                                    <CheckSquare className="w-4 h-4" />
                                  ) : moduloStatus === 'some' ? (
                                    <MinusSquare className="w-4 h-4" />
                                  ) : (
                                    <CheckSquare className="w-4 h-4" />
                                  )}
                                </button>
                              </div>
                              <div />
                              <div />
                              <div />
                              <div />
                            </div>

                            {/* Telas do módulo */}
                            {isExpanded && modulo.telas.map(tela => {
                              const perm = getPermissao(tela.tela);
                              const isEdited = permissoesEditadas.has(tela.tela);

                              return (
                                <div
                                  key={tela.tela}
                                  className={[
                                    'grid gap-2 px-4 py-2 pl-12 hover:bg-blue-50 group',
                                    isEdited ? 'bg-yellow-50' : '',
                                  ]
                                    .filter(Boolean)
                                    .join(' ')}
                                  style={{ gridTemplateColumns: '1fr 80px 80px 80px 80px' }}
                                >
                                  <div className="text-sm text-gray-700 flex items-center gap-2">
                                    <span>{tela.nomeTela}</span>
                                    {isEdited && <span className="text-xs text-yellow-600">●</span>}
                                  </div>
                                  {['consultar', 'incluir', 'alterar', 'excluir'].map((campo) => {
                                    // Dashboard só tem permissão de consultar
                                    const isDashboard = modulo.nome === 'Dashboard';
                                    const isDisabled = isDashboard && campo !== 'consultar';
                                    
                                    return (
                                      <div key={campo} className="flex justify-center">
                                        {isDisabled ? (
                                          <div className="w-6 h-6 rounded-md border-2 border-gray-200 bg-gray-100 flex items-center justify-center" title="Não aplicável para Dashboard">
                                            <span className="text-gray-400 text-xs">—</span>
                                          </div>
                                        ) : (
                                          <label className="relative cursor-pointer">
                                            <input
                                              type="checkbox"
                                              checked={perm?.[campo as keyof typeof perm] as boolean || false}
                                              onChange={(e) => alterarPermissao(tela.tela, campo as any, e.target.checked)}
                                              className="sr-only peer"
                                            />
                                            <div className={`w-6 h-6 rounded-md border-2 flex items-center justify-center transition-all ${
                                              perm?.[campo as keyof typeof perm]
                                                ? 'bg-blue-600 border-blue-600'
                                                : 'bg-[var(--surface)] border-gray-300 hover:border-blue-400'
                                            }`}>
                                              {perm?.[campo as keyof typeof perm] && (
                                                <Check className="w-4 h-4 text-white" />
                                              )}
                                            </div>
                                          </label>
                                        )}
                                      </div>
                                    );
                                  })}
                                </div>
                              );
                            })}
                          </div>
                        );
                      })}
                    </div>
                  </div>
                </>
              )}

              {/* Barra fixa de ações quando há alterações */}
              {grupoSelecionado && permissoesEditadas.size > 0 && (
                <div className="fixed bottom-0 left-0 right-0 bg-[var(--surface)] border-t border-[var(--border)] shadow-lg z-40">
                  <div className="max-w-[1600px] mx-auto px-6 py-3 flex items-center justify-between">
                    <div className="flex items-center gap-3">
                      <div className="flex items-center gap-2 text-amber-600">
                        <Zap className="w-5 h-5" />
                        <span className="font-medium">{permissoesEditadas.size} alteração(ões) não salva(s)</span>
                      </div>
                      <button
                        onClick={desfazerAlteracoes}
                        className="text-sm text-[var(--text-muted)] hover:text-gray-700 underline"
                      >
                        Desfazer todas
                      </button>
                    </div>
                    <button
                      onClick={salvarPermissoes}
                      disabled={salvando}
                      className="flex items-center gap-2 px-6 py-2.5 bg-gradient-to-r from-emerald-500 to-green-600 text-white rounded-lg hover:from-emerald-600 hover:to-green-700 transition-all shadow-lg hover:shadow-xl disabled:opacity-50 font-medium"
                    >
                      {salvando ? <Loader2 className="w-5 h-5 animate-spin" /> : <Save className="w-5 h-5" />}
                      Salvar Permissões
                    </button>
                  </div>
                </div>
              )}
            </div>
          )}
          </div>
        </div>
      </div>

      {/* === MODAIS === */}

      {/* Modal Criar Grupo */}
      {modalAberto === 'criar-grupo' && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-[var(--surface)] rounded-xl shadow-xl w-full max-w-md mx-4">
            <div className="p-4 border-b border-[var(--border)] flex items-center justify-between">
              <h3 className="font-semibold text-lg text-[var(--text)] flex items-center gap-2">
                <FolderPlus className="w-5 h-5 text-blue-600" />
                Novo Grupo
              </h3>
              <button onClick={() => setModalAberto(null)} className="text-gray-400 hover:text-[var(--text-muted)]">
                <X className="w-5 h-5" />
              </button>
            </div>
            <div className="p-4 space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Nome do Grupo</label>
                <input
                  type="text"
                  value={novoGrupoNome}
                  onChange={(e) => setNovoGrupoNome(e.target.value.toUpperCase())}
                  placeholder="Ex: VENDAS, FINANCEIRO, etc."
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  autoFocus
                />
              </div>
            </div>
            <div className="p-4 border-t border-[var(--border)] flex justify-end gap-3">
              <button onClick={() => setModalAberto(null)} className="px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-lg">
                Cancelar
              </button>
              <button
                onClick={handleCriarGrupo}
                disabled={salvando}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 flex items-center gap-2 disabled:opacity-50"
              >
                {salvando ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                Criar Grupo
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal Criar/Editar Usuário */}
      {(modalAberto === 'criar-usuario' || modalAberto === 'editar-usuario') && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-[var(--surface)] rounded-xl shadow-xl w-full max-w-lg mx-4">
            <div className="p-4 border-b border-[var(--border)] flex items-center justify-between">
              <h3 className="font-semibold text-lg text-[var(--text)] flex items-center gap-2">
                {modalAberto === 'criar-usuario' ? <><UserPlus className="w-5 h-5 text-blue-600" /> Novo Usuário</> : <><Edit3 className="w-5 h-5 text-blue-600" /> Editar Usuário</>}
              </h3>
              <button onClick={() => setModalAberto(null)} className="text-gray-400 hover:text-[var(--text-muted)]">
                <X className="w-5 h-5" />
              </button>
            </div>
            <div className="p-4 space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Nome de Usuário</label>
                <input
                  type="text"
                  value={usuarioForm.nome}
                  onChange={(e) => setUsuarioForm({ ...usuarioForm, nome: e.target.value.toUpperCase() })}
                  placeholder="Nome do usuário"
                  disabled={modalAberto === 'editar-usuario'}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
                />
              </div>

              {modalAberto === 'criar-usuario' && (
                <>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Senha</label>
                    <div className="relative">
                      <input
                        type={mostrarSenha ? 'text' : 'password'}
                        value={usuarioForm.senha}
                        onChange={(e) => setUsuarioForm({ ...usuarioForm, senha: e.target.value })}
                        placeholder="Senha"
                        className="w-full px-3 py-2 pr-10 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                      />
                      <button type="button" onClick={() => setMostrarSenha(!mostrarSenha)} className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400">
                        {mostrarSenha ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    </div>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Confirmar Senha</label>
                    <input
                      type={mostrarSenha ? 'text' : 'password'}
                      value={usuarioForm.confirmarSenha}
                      onChange={(e) => setUsuarioForm({ ...usuarioForm, confirmarSenha: e.target.value })}
                      placeholder="Confirme a senha"
                      className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                    />
                  </div>
                </>
              )}

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Grupo</label>
                <select
                  value={usuarioForm.grupo}
                  onChange={(e) => setUsuarioForm({ ...usuarioForm, grupo: e.target.value })}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">Selecione um grupo</option>
                  {grupos.map(g => <option key={g.nome} value={g.nome}>{g.nome}</option>)}
                </select>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Observações</label>
                <textarea
                  value={usuarioForm.observacoes || ''}
                  onChange={(e) => setUsuarioForm({ ...usuarioForm, observacoes: e.target.value })}
                  placeholder="Observações (opcional)"
                  rows={2}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                />
              </div>

              <div className="flex items-center gap-2">
                <input
                  type="checkbox"
                  id="usuario-ativo"
                  checked={usuarioForm.ativo}
                  onChange={(e) => setUsuarioForm({ ...usuarioForm, ativo: e.target.checked })}
                  className="w-4 h-4 text-blue-600 border-gray-300 rounded"
                />
                <label htmlFor="usuario-ativo" className="text-sm text-gray-700">Usuário ativo</label>
              </div>
            </div>
            <div className="p-4 border-t border-[var(--border)] flex justify-end gap-3">
              <button onClick={() => setModalAberto(null)} className="px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-lg">
                Cancelar
              </button>
              <button
                onClick={modalAberto === 'criar-usuario' ? handleCriarUsuario : handleEditarUsuario}
                disabled={salvando}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 flex items-center gap-2 disabled:opacity-50"
              >
                {salvando ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                {modalAberto === 'criar-usuario' ? 'Criar Usuário' : 'Salvar'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal Alterar Senha */}
      {modalAberto === 'alterar-senha' && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-[var(--surface)] rounded-xl shadow-xl w-full max-w-md mx-4">
            <div className="p-4 border-b border-[var(--border)] flex items-center justify-between">
              <h3 className="font-semibold text-lg text-[var(--text)] flex items-center gap-2">
                <Key className="w-5 h-5 text-blue-600" />
                Alterar Senha - {usuarioSelecionado?.nome}
              </h3>
              <button onClick={() => setModalAberto(null)} className="text-gray-400 hover:text-[var(--text-muted)]">
                <X className="w-5 h-5" />
              </button>
            </div>
            <div className="p-4 space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Nova Senha</label>
                <div className="relative">
                  <input
                    type={mostrarSenha ? 'text' : 'password'}
                    value={usuarioForm.senha}
                    onChange={(e) => setUsuarioForm({ ...usuarioForm, senha: e.target.value })}
                    placeholder="Nova senha"
                    className="w-full px-3 py-2 pr-10 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  />
                  <button type="button" onClick={() => setMostrarSenha(!mostrarSenha)} className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400">
                    {mostrarSenha ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Confirmar Nova Senha</label>
                <input
                  type={mostrarSenha ? 'text' : 'password'}
                  value={usuarioForm.confirmarSenha}
                  onChange={(e) => setUsuarioForm({ ...usuarioForm, confirmarSenha: e.target.value })}
                  placeholder="Confirme a nova senha"
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                />
              </div>
            </div>
            <div className="p-4 border-t border-[var(--border)] flex justify-end gap-3">
              <button onClick={() => setModalAberto(null)} className="px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-lg">
                Cancelar
              </button>
              <button
                onClick={handleAlterarSenha}
                disabled={salvando}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 flex items-center gap-2 disabled:opacity-50"
              >
                {salvando ? <Loader2 className="w-4 h-4 animate-spin" /> : <Key className="w-4 h-4" />}
                Alterar Senha
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal de Exclusão */}
      <ModalConfirmacao
        aberto={deleteModal.open}
        titulo={deleteModal.tipo === 'usuario' ? 'Excluir Usuário' : 'Excluir Grupo'}
        mensagem={
          deleteModal.tipo === 'usuario'
            ? 'Tem certeza que deseja excluir este usuário? Esta ação não pode ser desfeita.'
            : 'Tem certeza que deseja excluir este grupo? Os usuários serão movidos para "SEM GRUPO".'
        }
        nomeItem={deleteModal.nome}
        textoBotaoConfirmar="Excluir"
        processando={deleteModal.deleting}
        onConfirmar={confirmarExclusao}
        onCancelar={() => setDeleteModal({ open: false, tipo: 'usuario', nome: '', deleting: false })}
      />
    </div>
  );
}
