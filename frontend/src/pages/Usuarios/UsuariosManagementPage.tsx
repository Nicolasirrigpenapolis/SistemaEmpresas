import { useState, useEffect, useCallback } from 'react';
import {
  Users,
  UserPlus,
  FolderPlus,
  Trash2,
  Save,
  X,
  Shield,
  ChevronRight,
  ChevronDown,
  RefreshCw,
  Search,
  Eye,
  Edit,
  Plus,
  Check,
  AlertTriangle,
  User,
  Settings,
  FileText,
  ShoppingCart,
  DollarSign,
  Database,
  Cog,
  CheckSquare,
  Square,
  Filter,
  RotateCcw,
  Zap,
  Lock,
  Unlock,
  FileStack,
  Copy,
} from 'lucide-react';
import { usuarioManagementService } from '../../services/usuarioManagementService';
import permissoesTelaService from '../../services/permissoesTelaService';
import { useAuth } from '../../contexts/AuthContext';
import { ModalConfirmacao } from '../../components/common/ModalConfirmacao';
import type {
  GrupoComUsuariosDto,
  UsuarioListDto,
  UsuarioCreateDto,
  UsuarioUpdateDto,
  PermissoesGrupoDto,
  PermissaoTabelaDto,
  ModuloTabelasDto,
  TabType,
  PermissoesTemplateListDto,
  PermissoesTemplateComDetalhesDto,
  PermissoesTemplateCreateDto,
  PermissoesTemplateUpdateDto,
  PermissoesTemplateDetalheDto,
  ModuloComTelasDto,
} from '../../types';

// Ícones por módulo
const moduleIcons: Record<string, React.ReactNode> = {
  Comercial: <ShoppingCart className="w-4 h-4" />,
  Operacional: <Settings className="w-4 h-4" />,
  Financeiro: <DollarSign className="w-4 h-4" />,
  Cadastros: <Database className="w-4 h-4" />,
  Relatórios: <FileText className="w-4 h-4" />,
  Sistema: <Cog className="w-4 h-4" />,
};

export default function UsuariosManagementPage() {
  const { user } = useAuth();
  const isAdmin = user?.grupo?.toUpperCase() === 'PROGRAMADOR';

  // Estado da árvore
  const [arvore, setArvore] = useState<GrupoComUsuariosDto[]>([]);
  const [gruposExpandidos, setGruposExpandidos] = useState<Set<string>>(new Set());
  const [grupoSelecionado, setGrupoSelecionado] = useState<string | null>(null);
  const [usuarioSelecionado, setUsuarioSelecionado] = useState<string | null>(null);

  // Estado das permissões
  const [permissoesGrupo, setPermissoesGrupo] = useState<PermissoesGrupoDto | null>(null);
  const [tabelasDisponiveis, setTabelasDisponiveis] = useState<ModuloTabelasDto[]>([]);
  const [menusDisponiveis, setMenusDisponiveis] = useState<PermissaoTabelaDto[]>([]);
  const [permissoesModificadas, setPermissoesModificadas] = useState<Map<string, PermissaoTabelaDto>>(new Map());
  const [modulosExpandidos, setModulosExpandidos] = useState<Set<string>>(new Set(['Comercial', 'Operacional']));

  // Estado UI
  const [tabAtiva, setTabAtiva] = useState<TabType>('usuario');
  const [loading, setLoading] = useState(true);
  const [salvando, setSalvando] = useState(false);
  const [busca, setBusca] = useState('');

  // Estado de formulários
  const [modoNovoGrupo, setModoNovoGrupo] = useState(false);
  const [novoGrupoNome, setNovoGrupoNome] = useState('');
  const [modoNovoUsuario, setModoNovoUsuario] = useState(false);
  const [modoEditarUsuario, setModoEditarUsuario] = useState(false);
  const [usuarioForm, setUsuarioForm] = useState<UsuarioCreateDto>({
    nome: '',
    senha: '',
    confirmarSenha: '',
    grupo: '',
    observacoes: '',
    ativo: true,
  });

  // Toast simples
  const [toast, setToast] = useState<{ tipo: 'sucesso' | 'erro'; mensagem: string } | null>(null);

  // Estados de Templates de Permissões
  const [templates, setTemplates] = useState<PermissoesTemplateListDto[]>([]);
  const [templateSelecionado, setTemplateSelecionado] = useState<PermissoesTemplateComDetalhesDto | null>(null);
  const [telasDisponiveisTemplates, setTelasDisponiveisTemplates] = useState<ModuloComTelasDto[]>([]);
  const [modulosExpandidosTemplates, setModulosExpandidosTemplates] = useState<Set<string>>(new Set(['Comercial', 'Operacional']));
  const [modoEdicaoTemplate, setModoEdicaoTemplate] = useState<'criar' | 'editar' | 'visualizar' | null>(null);
  const [buscaTemplate, setBuscaTemplate] = useState('');
  const [formTemplate, setFormTemplate] = useState<{
    nome: string;
    descricao: string;
    detalhes: PermissoesTemplateDetalheDto[];
  }>({
    nome: '',
    descricao: '',
    detalhes: [],
  });
  const [deleteTemplateModal, setDeleteTemplateModal] = useState<{
    open: boolean;
    template: PermissoesTemplateListDto | null;
    deleting: boolean;
  }>({ open: false, template: null, deleting: false });

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

  // Carregar dados iniciais
  const carregarDados = useCallback(async () => {
    try {
      setLoading(true);
      const [arvoreData, tabelasData, menusData] = await Promise.all([
        usuarioManagementService.listarArvore(),
        usuarioManagementService.listarTabelasDisponiveis(),
        usuarioManagementService.listarMenusDisponiveis(),
      ]);

      setArvore(arvoreData);
      setTabelasDisponiveis(tabelasData);
      setMenusDisponiveis(menusData);

      // Expandir todos os grupos por padrão
      setGruposExpandidos(new Set(arvoreData.map(g => g.nome)));

      // Selecionar primeiro grupo se nenhum selecionado
      if (!grupoSelecionado && arvoreData.length > 0) {
        setGrupoSelecionado(arvoreData[0].nome);
      }
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      mostrarToast('erro', 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  }, [grupoSelecionado]);

  // Carregar templates de permissões
  const carregarTemplates = useCallback(async () => {
    try {
      const [templatesData, telasData] = await Promise.all([
        permissoesTelaService.listarTemplates(),
        permissoesTelaService.listarTelasDisponiveis(),
      ]);
      setTemplates(templatesData);
      setTelasDisponiveisTemplates(telasData);
      setModulosExpandidosTemplates(new Set(telasData.map(m => m.nome)));
    } catch (error) {
      console.error('Erro ao carregar templates:', error);
    }
  }, []);

  useEffect(() => {
    carregarDados();
  }, [carregarDados]);

  // Carregar templates quando a aba for selecionada
  useEffect(() => {
    if (tabAtiva === 'templates' && templates.length === 0) {
      carregarTemplates();
    }
  }, [tabAtiva, templates.length, carregarTemplates]);

  // Carregar permissões quando selecionar grupo
  useEffect(() => {
    if (grupoSelecionado) {
      carregarPermissoesGrupo(grupoSelecionado);
    }
  }, [grupoSelecionado]);

  const carregarPermissoesGrupo = async (grupo: string) => {
    try {
      const permissoes = await usuarioManagementService.obterPermissoesGrupo(grupo);
      setPermissoesGrupo(permissoes);
      setPermissoesModificadas(new Map());
    } catch (error) {
      console.error('Erro ao carregar permissões:', error);
    }
  };

  // Handlers de seleção
  const handleSelecionarGrupo = (grupo: string) => {
    setGrupoSelecionado(grupo);
    setUsuarioSelecionado(null);
    setModoEditarUsuario(false);
    setModoNovoUsuario(false);
  };

  const handleSelecionarUsuario = (usuario: UsuarioListDto) => {
    setUsuarioSelecionado(usuario.nome);
    setGrupoSelecionado(usuario.grupo);
    setModoEditarUsuario(false);
    setModoNovoUsuario(false);
  };

  const toggleExpandirGrupo = (grupo: string) => {
    const novosExpandidos = new Set(gruposExpandidos);
    if (novosExpandidos.has(grupo)) {
      novosExpandidos.delete(grupo);
    } else {
      novosExpandidos.add(grupo);
    }
    setGruposExpandidos(novosExpandidos);
  };

  // Handlers de grupo
  const handleCriarGrupo = async () => {
    try {
      setSalvando(true);
      // Backend valida se o nome está vazio
      const resultado = await usuarioManagementService.criarGrupo({ nome: novoGrupoNome.trim() });

      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem);
        setNovoGrupoNome('');
        setModoNovoGrupo(false);
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
        if (resultado.sucesso && usuarioSelecionado === nome) {
          setUsuarioSelecionado(null);
        }
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

  const handleExcluirGrupo = (nome: string) => {
    abrirModalExclusao('grupo', nome);
  };

  // Handlers de usuário
  const handleIniciarNovoUsuario = () => {
    setModoNovoUsuario(true);
    setModoEditarUsuario(false);
    setUsuarioForm({
      nome: '',
      senha: '',
      confirmarSenha: '',
      grupo: grupoSelecionado || '',
      observacoes: '',
      ativo: true,
    });
  };

  const handleIniciarEditarUsuario = () => {
    if (!usuarioSelecionado) return;

    const usuario = arvore
      .flatMap(g => g.usuarios)
      .find(u => u.nome === usuarioSelecionado);

    if (usuario) {
      setModoEditarUsuario(true);
      setModoNovoUsuario(false);
      setUsuarioForm({
        nome: usuario.nome,
        senha: '',
        confirmarSenha: '',
        grupo: usuario.grupo,
        observacoes: usuario.observacoes || '',
        ativo: usuario.ativo,
      });
    }
  };

  const handleSalvarUsuario = async () => {
    try {
      setSalvando(true);

      if (modoNovoUsuario) {
        // Criar novo usuário - backend valida campos obrigatórios e senhas
        const resultado = await usuarioManagementService.criarUsuario(usuarioForm);

        if (resultado.sucesso) {
          mostrarToast('sucesso', resultado.mensagem);
          setModoNovoUsuario(false);
          await carregarDados();
        } else {
          mostrarToast('erro', resultado.mensagem);
        }
      } else if (modoEditarUsuario && usuarioSelecionado) {
        // Atualizar usuário existente - backend valida tudo
        const updateDto: UsuarioUpdateDto = {
          grupo: usuarioForm.grupo,
          observacoes: usuarioForm.observacoes,
          ativo: usuarioForm.ativo,
        };

        if (usuarioForm.senha) {
          updateDto.novaSenha = usuarioForm.senha;
          updateDto.confirmarNovaSenha = usuarioForm.confirmarSenha;
        }

        const resultado = await usuarioManagementService.atualizarUsuario(usuarioSelecionado, updateDto);

        if (resultado.sucesso) {
          mostrarToast('sucesso', resultado.mensagem);
          setModoEditarUsuario(false);
          await carregarDados();
        } else {
          mostrarToast('erro', resultado.mensagem);
        }
      }
    } catch (error: any) {
      mostrarToast('erro', error.response?.data?.mensagem || 'Erro ao salvar usuário');
    } finally {
      setSalvando(false);
    }
  };

  const handleExcluirUsuario = (nome: string) => {
    abrirModalExclusao('usuario', nome);
  };

  // Handlers de permissões
  const handlePermissaoChange = (
    nomeTabela: string,
    campo: 'visualiza' | 'inclui' | 'modifica' | 'exclui',
    valor: boolean,
    tipo: 'TABELA' | 'MENU'
  ) => {
    const novasPermissoes = new Map(permissoesModificadas);
    
    // Primeiro busca nas permissões modificadas, depois nas permissões do grupo
    let permissaoExistente = novasPermissoes.get(nomeTabela);
    
    if (!permissaoExistente) {
      // Busca a permissão atual do grupo
      const permissaoGrupo = permissoesGrupo?.tabelas.find(p => p.nome === nomeTabela) ||
                             permissoesGrupo?.menus.find(p => p.nome === nomeTabela);
      
      if (permissaoGrupo) {
        permissaoExistente = { ...permissaoGrupo };
      } else {
        // Se não existe, cria uma nova com tudo bloqueado
        permissaoExistente = {
          projeto: ' ',
          nome: nomeTabela,
          nomeExibicao: nomeTabela,
          visualiza: false,
          inclui: false,
          modifica: false,
          exclui: false,
          tipo,
        };
      }
    }

    const novaPermissao = { ...permissaoExistente, [campo]: valor };

    // Se desmarcar Visualiza, desmarca tudo
    if (campo === 'visualiza' && !valor) {
      novaPermissao.inclui = false;
      novaPermissao.modifica = false;
      novaPermissao.exclui = false;
    }
    // Se marcar qualquer outra, marca Visualiza
    else if (campo !== 'visualiza' && valor) {
      novaPermissao.visualiza = true;
    }

    novasPermissoes.set(nomeTabela, novaPermissao);
    setPermissoesModificadas(novasPermissoes);
  };

  // Marcar/desmarcar todas as permissões de uma tabela específica
  const handleToggleTodasPermissoesTela = (
    nomeTabela: string,
    nomeExibicao: string,
    marcar: boolean,
    tipo: 'TABELA' | 'MENU'
  ) => {
    const novasPermissoes = new Map(permissoesModificadas);
    
    const novaPermissao: PermissaoTabelaDto = {
      projeto: ' ',
      nome: nomeTabela,
      nomeExibicao: nomeExibicao,
      visualiza: marcar,
      inclui: marcar,
      modifica: marcar,
      exclui: marcar,
      tipo,
    };

    novasPermissoes.set(nomeTabela, novaPermissao);
    setPermissoesModificadas(novasPermissoes);
  };

  // Marcar/desmarcar todas as permissões de todas as tabelas de um módulo
  const handleToggleTodasPermissoesModulo = (modulo: ModuloTabelasDto, marcar: boolean) => {
    const novasPermissoes = new Map(permissoesModificadas);
    
    modulo.tabelas.forEach(tabela => {
      const novaPermissao: PermissaoTabelaDto = {
        projeto: ' ',
        nome: tabela.nome,
        nomeExibicao: tabela.nomeExibicao,
        visualiza: marcar,
        inclui: marcar,
        modifica: marcar,
        exclui: marcar,
        tipo: 'TABELA',
      };
      novasPermissoes.set(tabela.nome, novaPermissao);
    });
    
    setPermissoesModificadas(novasPermissoes);
  };

  // Marcar/desmarcar todas as permissões de todas as tabelas do sistema
  const handleToggleTodasPermissoesSistema = (marcar: boolean) => {
    const novasPermissoes = new Map(permissoesModificadas);
    
    tabelasDisponiveis.forEach(modulo => {
      modulo.tabelas.forEach(tabela => {
        const novaPermissao: PermissaoTabelaDto = {
          projeto: ' ',
          nome: tabela.nome,
          nomeExibicao: tabela.nomeExibicao,
          visualiza: marcar,
          inclui: marcar,
          modifica: marcar,
          exclui: marcar,
          tipo: 'TABELA',
        };
        novasPermissoes.set(tabela.nome, novaPermissao);
      });
    });
    
    setPermissoesModificadas(novasPermissoes);
  };

  // Marcar/desmarcar todos os menus
  const handleToggleTodosMenus = (marcar: boolean) => {
    const novasPermissoes = new Map(permissoesModificadas);
    
    menusDisponiveis.forEach(menu => {
      const novaPermissao: PermissaoTabelaDto = {
        projeto: ' ',
        nome: menu.nome,
        nomeExibicao: menu.nomeExibicao,
        visualiza: marcar,
        inclui: false,
        modifica: false,
        exclui: false,
        tipo: 'MENU',
      };
      novasPermissoes.set(menu.nome, novaPermissao);
    });
    
    setPermissoesModificadas(novasPermissoes);
  };

  // Verificar se todas as permissões de uma tela estão marcadas
  const todasPermissoesTelaMarcadas = (nomeTabela: string): boolean => {
    const perm = getPermissao(nomeTabela);
    return perm.visualiza && perm.inclui && perm.modifica && perm.exclui;
  };

  // Verificar se todas as permissões de um módulo estão marcadas
  const todasPermissoesModuloMarcadas = (modulo: ModuloTabelasDto): boolean => {
    return modulo.tabelas.every(tabela => todasPermissoesTelaMarcadas(tabela.nome));
  };

  // Verificar se todos os menus estão marcados
  const todosMenusMarcados = (): boolean => {
    return menusDisponiveis.every(menu => getPermissao(menu.nome).visualiza);
  };

  const handleSalvarPermissoes = async () => {
    if (!grupoSelecionado || permissoesModificadas.size === 0) return;

    try {
      setSalvando(true);

      const resultado = await usuarioManagementService.atualizarPermissoes({
        grupo: grupoSelecionado,
        permissoes: Array.from(permissoesModificadas.values()),
      });

      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem);
        await carregarPermissoesGrupo(grupoSelecionado);
      } else {
        mostrarToast('erro', resultado.mensagem);
      }
    } catch (error: any) {
      mostrarToast('erro', error.response?.data?.mensagem || 'Erro ao salvar permissões');
    } finally {
      setSalvando(false);
    }
  };

  // Obter permissão atual para uma tabela
  const getPermissao = (nome: string): PermissaoTabelaDto => {
    // Primeiro verifica se foi modificada
    if (permissoesModificadas.has(nome)) {
      return permissoesModificadas.get(nome)!;
    }

    // Se é admin, todas liberadas
    if (permissoesGrupo?.isAdmin) {
      return {
        projeto: ' ',
        nome,
        nomeExibicao: nome,
        visualiza: true,
        inclui: true,
        modifica: true,
        exclui: true,
        tipo: 'TABELA',
      };
    }

    // Busca nas permissões do grupo
    const permissao = permissoesGrupo?.tabelas.find(p => p.nome === nome) ||
                      permissoesGrupo?.menus.find(p => p.nome === nome);

    if (permissao) {
      return permissao;
    }

    // Padrão: todas bloqueadas (se não tem permissão específica, não tem acesso)
    return {
      projeto: ' ',
      nome,
      nomeExibicao: nome,
      visualiza: false,
      inclui: false,
      modifica: false,
      exclui: false,
      tipo: 'TABELA',
    };
  };

  // ==========================================
  // FUNÇÕES DE TEMPLATES DE PERMISSÕES
  // ==========================================

  // Carregar template completo
  const carregarTemplate = async (id: number) => {
    try {
      const template = await permissoesTelaService.obterTemplate(id);
      setTemplateSelecionado(template);
      return template;
    } catch (error) {
      console.error('Erro ao carregar template:', error);
      mostrarToast('erro', 'Erro ao carregar template');
      return null;
    }
  };

  // Selecionar template para visualização
  const handleSelecionarTemplate = async (template: PermissoesTemplateListDto) => {
    const templateCompleto = await carregarTemplate(template.id);
    if (templateCompleto) {
      setModoEdicaoTemplate('visualizar');
      setFormTemplate({
        nome: templateCompleto.nome,
        descricao: templateCompleto.descricao || '',
        detalhes: templateCompleto.detalhes || [],
      });
    }
  };

  // Iniciar criação de novo template
  const handleNovoTemplate = () => {
    setTemplateSelecionado(null);
    setModoEdicaoTemplate('criar');
    setFormTemplate({ nome: '', descricao: '', detalhes: [] });
  };

  // Iniciar edição de template
  const handleEditarTemplate = async (template: PermissoesTemplateListDto) => {
    if (template.isPadrao) {
      mostrarToast('erro', 'Templates padrão não podem ser editados');
      return;
    }
    const templateCompleto = await carregarTemplate(template.id);
    if (templateCompleto) {
      setModoEdicaoTemplate('editar');
      setFormTemplate({
        nome: templateCompleto.nome,
        descricao: templateCompleto.descricao || '',
        detalhes: templateCompleto.detalhes || [],
      });
    }
  };

  // Duplicar template
  const handleDuplicarTemplate = async (template: PermissoesTemplateListDto) => {
    const templateCompleto = await carregarTemplate(template.id);
    if (templateCompleto) {
      setTemplateSelecionado(null);
      setModoEdicaoTemplate('criar');
      setFormTemplate({
        nome: `${templateCompleto.nome} (Cópia)`,
        descricao: templateCompleto.descricao || '',
        detalhes: templateCompleto.detalhes?.map(d => ({ ...d, id: undefined })) || [],
      });
    }
  };

  // Cancelar edição de template
  const handleCancelarTemplate = () => {
    setModoEdicaoTemplate(null);
    setTemplateSelecionado(null);
    setFormTemplate({ nome: '', descricao: '', detalhes: [] });
  };

  // Salvar template
  const handleSalvarTemplate = async () => {
    if (!formTemplate.nome.trim()) {
      mostrarToast('erro', 'Nome do template é obrigatório');
      return;
    }
    if (formTemplate.detalhes.length === 0) {
      mostrarToast('erro', 'Selecione pelo menos uma tela para o template');
      return;
    }

    try {
      setSalvando(true);

      if (modoEdicaoTemplate === 'criar') {
        const dto: PermissoesTemplateCreateDto = {
          nome: formTemplate.nome.trim(),
          descricao: formTemplate.descricao.trim() || null,
          detalhes: formTemplate.detalhes,
        };
        const resultado = await permissoesTelaService.criarTemplate(dto);
        if (resultado.sucesso) {
          mostrarToast('sucesso', resultado.mensagem || 'Template criado com sucesso');
          handleCancelarTemplate();
          await carregarTemplates();
        } else {
          mostrarToast('erro', resultado.mensagem || 'Erro ao criar template');
        }
      } else if (modoEdicaoTemplate === 'editar' && templateSelecionado) {
        const dto: PermissoesTemplateUpdateDto = {
          nome: formTemplate.nome.trim(),
          descricao: formTemplate.descricao.trim() || null,
          detalhes: formTemplate.detalhes,
        };
        const resultado = await permissoesTelaService.atualizarTemplate(templateSelecionado.id, dto);
        if (resultado.sucesso) {
          mostrarToast('sucesso', resultado.mensagem || 'Template atualizado com sucesso');
          handleCancelarTemplate();
          await carregarTemplates();
        } else {
          mostrarToast('erro', resultado.mensagem || 'Erro ao atualizar template');
        }
      }
    } catch (error) {
      console.error('Erro ao salvar template:', error);
      mostrarToast('erro', 'Erro ao salvar template');
    } finally {
      setSalvando(false);
    }
  };

  // Confirmar exclusão de template
  const handleConfirmarExclusaoTemplate = (template: PermissoesTemplateListDto) => {
    if (template.isPadrao) {
      mostrarToast('erro', 'Templates padrão não podem ser excluídos');
      return;
    }
    setDeleteTemplateModal({ open: true, template, deleting: false });
  };

  // Excluir template
  const handleExcluirTemplate = async () => {
    if (!deleteTemplateModal.template) return;

    try {
      setDeleteTemplateModal(prev => ({ ...prev, deleting: true }));
      const resultado = await permissoesTelaService.excluirTemplate(deleteTemplateModal.template.id);

      if (resultado.sucesso) {
        mostrarToast('sucesso', resultado.mensagem || 'Template excluído com sucesso');
        setDeleteTemplateModal({ open: false, template: null, deleting: false });
        if (templateSelecionado?.id === deleteTemplateModal.template.id) {
          handleCancelarTemplate();
        }
        await carregarTemplates();
      } else {
        mostrarToast('erro', resultado.mensagem || 'Erro ao excluir template');
      }
    } catch (error) {
      console.error('Erro ao excluir template:', error);
      mostrarToast('erro', 'Erro ao excluir template');
    } finally {
      setDeleteTemplateModal(prev => ({ ...prev, deleting: false }));
    }
  };

  // Toggle módulo expandido em templates
  const toggleModuloTemplate = (modulo: string) => {
    setModulosExpandidosTemplates(prev => {
      const newSet = new Set(prev);
      if (newSet.has(modulo)) newSet.delete(modulo);
      else newSet.add(modulo);
      return newSet;
    });
  };

  // Obter detalhe de uma tela no form de template
  const getDetalheTemplate = (modulo: string, tela: string): PermissoesTemplateDetalheDto | undefined => {
    return formTemplate.detalhes.find(d => d.modulo === modulo && d.tela === tela);
  };

  // Atualizar permissão de uma tela no template
  const atualizarPermissaoTemplate = (
    modulo: string,
    tela: string,
    campo: 'consultar' | 'incluir' | 'alterar' | 'excluir',
    valor: boolean
  ) => {
    if (modoEdicaoTemplate === 'visualizar') return;

    setFormTemplate(prev => {
      const detalhesAtualizados = [...prev.detalhes];
      const index = detalhesAtualizados.findIndex(d => d.modulo === modulo && d.tela === tela);

      if (index >= 0) {
        detalhesAtualizados[index] = { ...detalhesAtualizados[index], [campo]: valor };
        const detalhe = detalhesAtualizados[index];
        if (!detalhe.consultar && !detalhe.incluir && !detalhe.alterar && !detalhe.excluir) {
          detalhesAtualizados.splice(index, 1);
        }
      } else if (valor) {
        detalhesAtualizados.push({
          modulo, tela,
          consultar: campo === 'consultar',
          incluir: campo === 'incluir',
          alterar: campo === 'alterar',
          excluir: campo === 'excluir',
        });
      }

      return { ...prev, detalhes: detalhesAtualizados };
    });
  };

  // Marcar todas as permissões de uma tela no template
  const marcarTodasTemplate = (modulo: string, tela: string, marcar: boolean) => {
    if (modoEdicaoTemplate === 'visualizar') return;

    setFormTemplate(prev => {
      const detalhesAtualizados = [...prev.detalhes];
      const index = detalhesAtualizados.findIndex(d => d.modulo === modulo && d.tela === tela);

      if (marcar) {
        if (index >= 0) {
          detalhesAtualizados[index] = { ...detalhesAtualizados[index], consultar: true, incluir: true, alterar: true, excluir: true };
        } else {
          detalhesAtualizados.push({ modulo, tela, consultar: true, incluir: true, alterar: true, excluir: true });
        }
      } else if (index >= 0) {
        detalhesAtualizados.splice(index, 1);
      }

      return { ...prev, detalhes: detalhesAtualizados };
    });
  };

  // Marcar todas as telas de um módulo no template
  const marcarTodasModuloTemplate = (modulo: string, marcar: boolean) => {
    if (modoEdicaoTemplate === 'visualizar') return;

    const moduloData = telasDisponiveisTemplates.find(m => m.nome === modulo);
    if (!moduloData) return;

    setFormTemplate(prev => {
      const detalhesAtualizados = prev.detalhes.filter(d => d.modulo !== modulo);
      if (marcar) {
        moduloData.telas.forEach(tela => {
          detalhesAtualizados.push({ modulo, tela: tela.tela, consultar: true, incluir: true, alterar: true, excluir: true });
        });
      }
      return { ...prev, detalhes: detalhesAtualizados };
    });
  };

  // Verificar se um módulo tem todas as permissões no template
  const moduloTemTodasPermissoesTemplate = (modulo: string): boolean => {
    const moduloData = telasDisponiveisTemplates.find(m => m.nome === modulo);
    if (!moduloData) return false;
    return moduloData.telas.every(tela => {
      const detalhe = getDetalheTemplate(modulo, tela.tela);
      return detalhe?.consultar && detalhe?.incluir && detalhe?.alterar && detalhe?.excluir;
    });
  };

  // Filtrar templates
  const templatesFiltrados = templates.filter(t =>
    t.nome.toLowerCase().includes(buscaTemplate.toLowerCase()) ||
    (t.descricao?.toLowerCase().includes(buscaTemplate.toLowerCase()))
  );

  // Filtrar árvore pela busca
  const arvoreFiltrada = busca.trim()
    ? arvore.map(grupo => ({
        ...grupo,
        usuarios: grupo.usuarios.filter(u =>
          u.nome.toLowerCase().includes(busca.toLowerCase())
        ),
      })).filter(g => g.usuarios.length > 0 || g.nome.toLowerCase().includes(busca.toLowerCase()))
    : arvore;

  // Renderização
  if (loading) {
    return (
      <div className="flex items-center justify-center h-96">
        <RefreshCw className="w-8 h-8 animate-spin text-blue-500" />
        <span className="ml-3 text-[var(--text-muted)]">Carregando...</span>
      </div>
    );
  }

  return (
    <div className="h-full flex flex-col">
      {/* Toast */}
      {toast && (
        <div className={`fixed top-24 right-4 z-40 px-4 py-3 rounded-lg shadow-lg flex items-center gap-2 ${
          toast.tipo === 'sucesso' ? 'bg-green-500 text-white' : 'bg-red-500 text-white'
        }`}>
          {toast.tipo === 'sucesso' ? <Check className="w-5 h-5" /> : <AlertTriangle className="w-5 h-5" />}
          {toast.mensagem}
        </div>
      )}

      {/* Header */}
      <div className="bg-[var(--surface)] border-b px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <Shield className="w-8 h-8 text-blue-600" />
            <div>
              <h1 className="text-2xl font-bold text-[var(--text)]">Controle de Usuários</h1>
              <p className="text-sm text-[var(--text-muted)]">Gerenciamento de usuários, grupos e permissões</p>
            </div>
          </div>
          
          {!isAdmin && (
            <div className="flex items-center gap-2 px-4 py-2 bg-yellow-100 text-yellow-800 rounded-lg">
              <AlertTriangle className="w-5 h-5" />
              <span className="text-sm">Acesso somente leitura</span>
            </div>
          )}
        </div>
      </div>

      {/* Content */}
      <div className="flex-1 flex overflow-hidden">
        {/* Sidebar - Árvore de Grupos/Usuários */}
        <div className="w-80 bg-[var(--surface-muted)] border-r flex flex-col">
          {/* Busca */}
          <div className="p-3 border-b bg-[var(--surface)]">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
              <input
                type="text"
                placeholder="Buscar usuário..."
                value={busca}
                onChange={e => setBusca(e.target.value)}
                className="w-full pl-9 pr-3 py-2 border rounded-lg text-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
          </div>

          {/* Botões de ação */}
          {isAdmin && (
            <div className="p-3 border-b bg-[var(--surface)] flex gap-2">
              <button
                onClick={() => setModoNovoGrupo(!modoNovoGrupo)}
                className="flex-1 flex items-center justify-center gap-2 px-3 py-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100 transition-colors text-sm"
              >
                <FolderPlus className="w-4 h-4" />
                Novo Grupo
              </button>
              <button
                onClick={handleIniciarNovoUsuario}
                disabled={!grupoSelecionado}
                className="flex-1 flex items-center justify-center gap-2 px-3 py-2 bg-green-50 text-green-600 rounded-lg hover:bg-green-100 transition-colors text-sm disabled:opacity-50 disabled:cursor-not-allowed"
              >
                <UserPlus className="w-4 h-4" />
                Novo Usuário
              </button>
            </div>
          )}

          {/* Form novo grupo */}
          {modoNovoGrupo && (
            <div className="p-3 border-b bg-blue-50">
              <input
                type="text"
                placeholder="Nome do grupo"
                value={novoGrupoNome}
                onChange={e => setNovoGrupoNome(e.target.value.toUpperCase())}
                maxLength={25}
                className="w-full px-3 py-2 border rounded-lg text-sm mb-2"
                autoFocus
              />
              <div className="flex gap-2">
                <button
                  onClick={handleCriarGrupo}
                  disabled={salvando}
                  className="flex-1 flex items-center justify-center gap-1 px-3 py-1.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 text-sm disabled:opacity-50"
                >
                  <Save className="w-4 h-4" />
                  Salvar
                </button>
                <button
                  onClick={() => { setModoNovoGrupo(false); setNovoGrupoNome(''); }}
                  className="flex-1 flex items-center justify-center gap-1 px-3 py-1.5 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300 text-sm"
                >
                  <X className="w-4 h-4" />
                  Cancelar
                </button>
              </div>
            </div>
          )}

          {/* Árvore */}
          <div className="flex-1 overflow-auto p-3">
            {arvoreFiltrada.map(grupo => (
              <div key={grupo.nome} className="mb-2">
                {/* Grupo */}
                <div
                  className={`flex items-center gap-2 px-3 py-2 rounded-lg cursor-pointer transition-colors ${
                    grupoSelecionado === grupo.nome && !usuarioSelecionado
                      ? 'bg-blue-100 text-blue-800'
                      : 'hover:bg-gray-100'
                  }`}
                  onClick={() => handleSelecionarGrupo(grupo.nome)}
                >
                  <button
                    onClick={e => { e.stopPropagation(); toggleExpandirGrupo(grupo.nome); }}
                    className="p-0.5 hover:bg-gray-200 rounded"
                  >
                    {gruposExpandidos.has(grupo.nome) 
                      ? <ChevronDown className="w-4 h-4" />
                      : <ChevronRight className="w-4 h-4" />
                    }
                  </button>
                  
                  {grupo.isAdmin ? (
                    <Shield className="w-4 h-4 text-amber-500" />
                  ) : (
                    <Users className="w-4 h-4 text-[var(--text-muted)]" />
                  )}
                  
                  <span className="flex-1 font-medium text-sm">{grupo.nome}</span>
                  
                  <span className="text-xs text-gray-400">({grupo.usuarios.length})</span>
                  
                  {isAdmin && !grupo.isAdmin && grupo.usuarios.length === 0 && (
                    <button
                      onClick={e => { e.stopPropagation(); handleExcluirGrupo(grupo.nome); }}
                      className="p-1 hover:bg-red-100 rounded text-red-500"
                      title="Excluir grupo"
                    >
                      <Trash2 className="w-3.5 h-3.5" />
                    </button>
                  )}
                </div>

                {/* Usuários do grupo */}
                {gruposExpandidos.has(grupo.nome) && (
                  <div className="ml-6 mt-1 space-y-1">
                    {grupo.usuarios.map(usuario => (
                      <div
                        key={usuario.nome}
                        className={`flex items-center gap-2 px-3 py-1.5 rounded-lg cursor-pointer transition-colors ${
                          usuarioSelecionado === usuario.nome
                            ? 'bg-blue-100 text-blue-800'
                            : 'hover:bg-gray-100'
                        }`}
                        onClick={() => handleSelecionarUsuario(usuario)}
                      >
                        <User className="w-4 h-4 text-blue-500" />
                        <span className="flex-1 text-sm">{usuario.nome}</span>
                        
                        {isAdmin && (
                          <button
                            onClick={e => { e.stopPropagation(); handleExcluirUsuario(usuario.nome); }}
                            className="p-1 hover:bg-red-100 rounded text-red-500 opacity-0 group-hover:opacity-100"
                            title="Excluir usuário"
                          >
                            <Trash2 className="w-3.5 h-3.5" />
                          </button>
                        )}
                      </div>
                    ))}
                  </div>
                )}
              </div>
            ))}
          </div>
        </div>

        {/* Main Content */}
        <div className="flex-1 flex flex-col overflow-hidden">
          {/* Tabs */}
          <div className="bg-[var(--surface)] border-b px-6">
            <div className="flex gap-1">
              {[
                { id: 'usuario' as TabType, label: 'Usuário', icon: User },
                { id: 'menus' as TabType, label: 'Menus', icon: FileText },
                { id: 'tabelas' as TabType, label: 'Tabelas', icon: Database },
                { id: 'templates' as TabType, label: 'Templates', icon: FileStack },
              ].map(tab => (
                <button
                  key={tab.id}
                  onClick={() => setTabAtiva(tab.id)}
                  className={`flex items-center gap-2 px-4 py-3 border-b-2 transition-colors ${
                    tabAtiva === tab.id
                      ? 'border-blue-500 text-blue-600'
                      : 'border-transparent text-[var(--text-muted)] hover:text-gray-700'
                  }`}
                >
                  <tab.icon className="w-4 h-4" />
                  {tab.label}
                </button>
              ))}
            </div>
          </div>

          {/* Tab Content */}
          <div className="flex-1 overflow-auto p-6">
            {/* Tab Usuário */}
            {tabAtiva === 'usuario' && (
              <div className="max-w-2xl">
                {modoNovoUsuario || modoEditarUsuario ? (
                  // Formulário de usuário
                  <div className="bg-[var(--surface)] rounded-lg border p-6">
                    <h3 className="text-lg font-semibold mb-4">
                      {modoNovoUsuario ? 'Novo Usuário' : 'Editar Usuário'}
                    </h3>
                    
                    <div className="space-y-4">
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                          Nome do Usuário *
                        </label>
                        <input
                          type="text"
                          value={usuarioForm.nome}
                          onChange={e => setUsuarioForm({ ...usuarioForm, nome: e.target.value.toUpperCase() })}
                          disabled={modoEditarUsuario}
                          maxLength={25}
                          className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
                        />
                      </div>

                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                          Grupo *
                        </label>
                        <select
                          value={usuarioForm.grupo}
                          onChange={e => setUsuarioForm({ ...usuarioForm, grupo: e.target.value })}
                          className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        >
                          <option value="">Selecione...</option>
                          {arvore.map(g => (
                            <option key={g.nome} value={g.nome}>{g.nome}</option>
                          ))}
                        </select>
                      </div>

                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                          {modoEditarUsuario ? 'Nova Senha (deixe em branco para manter)' : 'Senha *'}
                        </label>
                        <input
                          type="password"
                          value={usuarioForm.senha}
                          onChange={e => setUsuarioForm({ ...usuarioForm, senha: e.target.value })}
                          maxLength={25}
                          className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                      </div>

                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                          Confirmar Senha
                        </label>
                        <input
                          type="password"
                          value={usuarioForm.confirmarSenha}
                          onChange={e => setUsuarioForm({ ...usuarioForm, confirmarSenha: e.target.value })}
                          maxLength={25}
                          className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                      </div>

                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                          Observações
                        </label>
                        <textarea
                          value={usuarioForm.observacoes || ''}
                          onChange={e => setUsuarioForm({ ...usuarioForm, observacoes: e.target.value })}
                          rows={3}
                          className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                      </div>

                      <div className="flex gap-3 pt-4">
                        <button
                          onClick={handleSalvarUsuario}
                          disabled={salvando}
                          className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
                        >
                          <Save className="w-4 h-4" />
                          Salvar
                        </button>
                        <button
                          onClick={() => { setModoNovoUsuario(false); setModoEditarUsuario(false); }}
                          className="flex items-center gap-2 px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300"
                        >
                          <X className="w-4 h-4" />
                          Cancelar
                        </button>
                      </div>
                    </div>
                  </div>
                ) : usuarioSelecionado ? (
                  // Visualização do usuário
                  <div className="bg-[var(--surface)] rounded-lg border p-6">
                    <div className="flex items-center justify-between mb-4">
                      <div className="flex items-center gap-3">
                        <div className="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center">
                          <User className="w-6 h-6 text-blue-600" />
                        </div>
                        <div>
                          <h3 className="text-lg font-semibold">{usuarioSelecionado}</h3>
                          <p className="text-sm text-[var(--text-muted)]">
                            Grupo: <span className="font-medium">{grupoSelecionado}</span>
                          </p>
                        </div>
                      </div>
                      
                      {isAdmin && (
                        <div className="flex gap-2">
                          <button
                            onClick={handleIniciarEditarUsuario}
                            className="flex items-center gap-2 px-3 py-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100"
                          >
                            <Edit className="w-4 h-4" />
                            Editar
                          </button>
                          <button
                            onClick={() => handleExcluirUsuario(usuarioSelecionado)}
                            className="flex items-center gap-2 px-3 py-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100"
                          >
                            <Trash2 className="w-4 h-4" />
                            Excluir
                          </button>
                        </div>
                      )}
                    </div>

                    {arvore.flatMap(g => g.usuarios).find(u => u.nome === usuarioSelecionado)?.observacoes && (
                      <div className="mt-4 p-4 bg-[var(--surface-muted)] rounded-lg">
                        <h4 className="text-sm font-medium text-gray-700 mb-1">Observações</h4>
                        <p className="text-sm text-[var(--text-muted)]">
                          {arvore.flatMap(g => g.usuarios).find(u => u.nome === usuarioSelecionado)?.observacoes}
                        </p>
                      </div>
                    )}
                  </div>
                ) : grupoSelecionado ? (
                  // Visualização do grupo
                  <div className="bg-[var(--surface)] rounded-lg border p-6">
                    <div className="flex items-center gap-3 mb-4">
                      <div className="w-12 h-12 bg-amber-100 rounded-full flex items-center justify-center">
                        {arvore.find(g => g.nome === grupoSelecionado)?.isAdmin ? (
                          <Shield className="w-6 h-6 text-amber-600" />
                        ) : (
                          <Users className="w-6 h-6 text-[var(--text-muted)]" />
                        )}
                      </div>
                      <div>
                        <h3 className="text-lg font-semibold">{grupoSelecionado}</h3>
                        <p className="text-sm text-[var(--text-muted)]">
                          {arvore.find(g => g.nome === grupoSelecionado)?.usuarios.length || 0} usuário(s)
                        </p>
                      </div>
                    </div>

                    {arvore.find(g => g.nome === grupoSelecionado)?.isAdmin && (
                      <div className="p-4 bg-amber-50 text-amber-800 rounded-lg">
                        <div className="flex items-center gap-2">
                          <Shield className="w-5 h-5" />
                          <span className="font-medium">Grupo Administrador</span>
                        </div>
                        <p className="text-sm mt-1">
                          Este grupo possui acesso total ao sistema. As permissões não podem ser alteradas.
                        </p>
                      </div>
                    )}
                  </div>
                ) : (
                  <div className="text-center py-12 text-[var(--text-muted)]">
                    <Users className="w-12 h-12 mx-auto mb-3 opacity-50" />
                    <p>Selecione um grupo ou usuário para visualizar</p>
                  </div>
                )}
              </div>
            )}

            {/* Tab Menus */}
            {tabAtiva === 'menus' && grupoSelecionado && (
              <div className="max-w-4xl">
                {permissoesGrupo?.isAdmin ? (
                  <div className="p-4 bg-amber-50 text-amber-800 rounded-lg">
                    <div className="flex items-center gap-2">
                      <Shield className="w-5 h-5" />
                      <span className="font-medium">Grupo Administrador</span>
                    </div>
                    <p className="text-sm mt-1">
                      O grupo PROGRAMADOR possui acesso total a todos os menus.
                    </p>
                  </div>
                ) : (
                  <>
                    <div className="flex items-center justify-between mb-4">
                      <h3 className="text-lg font-semibold">Permissões de Menus - {grupoSelecionado}</h3>
                      <div className="flex items-center gap-2">
                        {isAdmin && (
                          <button
                            onClick={() => handleToggleTodosMenus(!todosMenusMarcados())}
                            className={`flex items-center gap-2 px-3 py-2 rounded-lg text-sm transition-colors ${
                              todosMenusMarcados()
                                ? 'bg-green-100 text-green-700 hover:bg-green-200'
                                : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                            }`}
                          >
                            <Check className="w-4 h-4" />
                            {todosMenusMarcados() ? 'Desmarcar Todos' : 'Marcar Todos'}
                          </button>
                        )}
                        {isAdmin && permissoesModificadas.size > 0 && (
                          <button
                            onClick={handleSalvarPermissoes}
                            disabled={salvando}
                            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
                          >
                            <Save className="w-4 h-4" />
                            Salvar Alterações
                          </button>
                        )}
                      </div>
                    </div>

                    <div className="bg-[var(--surface)] rounded-lg border overflow-hidden">
                      <table className="w-full">
                        <thead className="bg-[var(--surface-muted)]">
                          <tr>
                            <th className="text-left px-4 py-3 text-sm font-medium text-gray-700">Menu</th>
                            <th className="text-center px-4 py-3 text-sm font-medium text-gray-700 w-24">
                              <Eye className="w-4 h-4 mx-auto" />
                            </th>
                          </tr>
                        </thead>
                        <tbody className="divide-y">
                          {menusDisponiveis.map(menu => {
                            const perm = getPermissao(menu.nome);
                            return (
                              <tr key={menu.nome} className="hover:bg-[var(--surface-muted)]">
                                <td className="px-4 py-3">{menu.nomeExibicao}</td>
                                <td className="px-4 py-3 text-center">
                                  <input
                                    type="checkbox"
                                    checked={perm.visualiza}
                                    onChange={e => handlePermissaoChange(menu.nome, 'visualiza', e.target.checked, 'MENU')}
                                    disabled={!isAdmin}
                                    className="w-5 h-5 text-blue-600 rounded focus:ring-blue-500 disabled:opacity-50"
                                  />
                                </td>
                              </tr>
                            );
                          })}
                        </tbody>
                      </table>
                    </div>
                  </>
                )}
              </div>
            )}

            {/* Tab Tabelas - Versão Melhorada */}
            {tabAtiva === 'tabelas' && grupoSelecionado && (
              <div className="h-full flex flex-col">
                {permissoesGrupo?.isAdmin ? (
                  <div className="p-6 bg-gradient-to-r from-amber-50 to-orange-50 border border-amber-200 rounded-xl">
                    <div className="flex items-center gap-3">
                      <div className="p-3 bg-amber-100 rounded-full">
                        <Shield className="w-6 h-6 text-amber-600" />
                      </div>
                      <div>
                        <h3 className="font-semibold text-amber-800">Grupo Administrador</h3>
                        <p className="text-sm text-amber-700">
                          O grupo PROGRAMADOR possui acesso total a todas as tabelas do sistema.
                        </p>
                      </div>
                    </div>
                  </div>
                ) : (
                  <>
                    {/* Header com Controles Rápidos */}
                    <div className="bg-[var(--surface)] rounded-xl border shadow-sm p-4 mb-4">
                      <div className="flex flex-wrap items-center justify-between gap-4">
                        {/* Título e Grupo */}
                        <div className="flex items-center gap-3">
                          <div className="p-2 bg-blue-100 rounded-lg">
                            <Database className="w-5 h-5 text-blue-600" />
                          </div>
                          <div>
                            <h3 className="text-lg font-semibold text-[var(--text)]">Permissões de Tabelas</h3>
                            <p className="text-sm text-[var(--text-muted)]">Grupo: <span className="font-medium text-blue-600">{grupoSelecionado}</span></p>
                          </div>
                        </div>

                        {/* Estatísticas Rápidas */}
                        <div className="flex items-center gap-6">
                          {(() => {
                            const totalTabelas = tabelasDisponiveis.reduce((acc, m) => acc + m.tabelas.length, 0);
                            const tabelasComAcesso = tabelasDisponiveis.reduce((acc, modulo) => {
                              return acc + modulo.tabelas.filter(t => getPermissao(t.nome).visualiza).length;
                            }, 0);
                            const percentual = totalTabelas > 0 ? Math.round((tabelasComAcesso / totalTabelas) * 100) : 0;
                            
                            return (
                              <>
                                <div className="text-center">
                                  <div className="text-2xl font-bold text-[var(--text)]">{tabelasComAcesso}</div>
                                  <div className="text-xs text-[var(--text-muted)]">de {totalTabelas} tabelas</div>
                                </div>
                                <div className="w-24 h-24 relative">
                                  <svg className="w-24 h-24 transform -rotate-90">
                                    <circle
                                      cx="48"
                                      cy="48"
                                      r="40"
                                      stroke="#e5e7eb"
                                      strokeWidth="8"
                                      fill="none"
                                    />
                                    <circle
                                      cx="48"
                                      cy="48"
                                      r="40"
                                      stroke={percentual >= 80 ? '#22c55e' : percentual >= 50 ? '#f59e0b' : '#3b82f6'}
                                      strokeWidth="8"
                                      fill="none"
                                      strokeDasharray={`${2 * Math.PI * 40}`}
                                      strokeDashoffset={`${2 * Math.PI * 40 * (1 - percentual / 100)}`}
                                      strokeLinecap="round"
                                    />
                                  </svg>
                                  <div className="absolute inset-0 flex items-center justify-center">
                                    <span className="text-lg font-bold text-gray-700">{percentual}%</span>
                                  </div>
                                </div>
                              </>
                            );
                          })()}
                        </div>
                      </div>

                      {/* Ações Rápidas */}
                      {isAdmin && (
                        <div className="mt-4 pt-4 border-t flex flex-wrap items-center gap-2">
                          <span className="text-sm font-medium text-[var(--text-muted)] mr-2">Ações Rápidas:</span>
                          
                          <button
                            onClick={() => handleToggleTodasPermissoesSistema(true)}
                            className="inline-flex items-center gap-1.5 px-3 py-1.5 bg-green-100 text-green-700 rounded-lg hover:bg-green-200 transition-colors text-sm font-medium"
                          >
                            <Unlock className="w-4 h-4" />
                            Liberar Tudo
                          </button>
                          
                          <button
                            onClick={() => handleToggleTodasPermissoesSistema(false)}
                            className="inline-flex items-center gap-1.5 px-3 py-1.5 bg-red-100 text-red-700 rounded-lg hover:bg-red-200 transition-colors text-sm font-medium"
                          >
                            <Lock className="w-4 h-4" />
                            Bloquear Tudo
                          </button>

                          <div className="h-6 w-px bg-gray-200 mx-2" />
                          
                          <button
                            onClick={() => {
                              // Marcar apenas Visualiza em tudo
                              const novasPermissoes = new Map(permissoesModificadas);
                              tabelasDisponiveis.forEach(modulo => {
                                modulo.tabelas.forEach(tabela => {
                                  const permAtual = getPermissao(tabela.nome);
                                  novasPermissoes.set(tabela.nome, {
                                    ...permAtual,
                                    projeto: ' ',
                                    nome: tabela.nome,
                                    nomeExibicao: tabela.nomeExibicao,
                                    visualiza: true,
                                    inclui: false,
                                    modifica: false,
                                    exclui: false,
                                    tipo: 'TABELA',
                                  });
                                });
                              });
                              setPermissoesModificadas(novasPermissoes);
                            }}
                            className="inline-flex items-center gap-1.5 px-3 py-1.5 bg-blue-100 text-blue-700 rounded-lg hover:bg-blue-200 transition-colors text-sm font-medium"
                          >
                            <Eye className="w-4 h-4" />
                            Apenas Visualizar
                          </button>

                          <button
                            onClick={() => setPermissoesModificadas(new Map())}
                            disabled={permissoesModificadas.size === 0}
                            className="inline-flex items-center gap-1.5 px-3 py-1.5 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors text-sm font-medium disabled:opacity-50 disabled:cursor-not-allowed"
                          >
                            <RotateCcw className="w-4 h-4" />
                            Desfazer Alterações
                          </button>

                          {permissoesModificadas.size > 0 && (
                            <div className="flex-1 flex justify-end">
                              <button
                                onClick={handleSalvarPermissoes}
                                disabled={salvando}
                                className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors font-medium shadow-sm disabled:opacity-50"
                              >
                                {salvando ? (
                                  <RefreshCw className="w-4 h-4 animate-spin" />
                                ) : (
                                  <Save className="w-4 h-4" />
                                )}
                                Salvar ({permissoesModificadas.size} alterações)
                              </button>
                            </div>
                          )}
                        </div>
                      )}
                    </div>

                    {/* Busca e Filtros */}
                    <div className="bg-[var(--surface)] rounded-xl border shadow-sm p-3 mb-4">
                      <div className="flex items-center gap-3">
                        <div className="relative flex-1 max-w-md">
                          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                          <input
                            type="text"
                            placeholder="Buscar tabela..."
                            className="w-full pl-9 pr-3 py-2 border rounded-lg text-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                            onChange={(e) => {
                              const valor = e.target.value.toLowerCase();
                              // Expandir módulos que têm tabelas correspondentes
                              if (valor) {
                                const modulosComResultados = tabelasDisponiveis
                                  .filter(m => m.tabelas.some(t => 
                                    t.nomeExibicao.toLowerCase().includes(valor) ||
                                    t.nome.toLowerCase().includes(valor)
                                  ))
                                  .map(m => m.nome);
                                setModulosExpandidos(new Set(modulosComResultados));
                              }
                            }}
                          />
                        </div>
                        
                        <div className="flex items-center gap-2 text-sm text-[var(--text-muted)]">
                          <Filter className="w-4 h-4" />
                          <span>Filtrar por:</span>
                        </div>
                        
                        <button
                          onClick={() => setModulosExpandidos(new Set(tabelasDisponiveis.map(m => m.nome)))}
                          className="px-3 py-1.5 text-sm bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200"
                        >
                          Expandir Todos
                        </button>
                        
                        <button
                          onClick={() => setModulosExpandidos(new Set())}
                          className="px-3 py-1.5 text-sm bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200"
                        >
                          Recolher Todos
                        </button>
                      </div>
                    </div>

                    {/* Lista de Módulos e Tabelas */}
                    <div className="flex-1 overflow-auto space-y-3">
                      {tabelasDisponiveis.map(modulo => {
                        const totalTabelas = modulo.tabelas.length;
                        const tabelasLiberadas = modulo.tabelas.filter(t => getPermissao(t.nome).visualiza).length;
                        const tabelasCompletas = modulo.tabelas.filter(t => todasPermissoesTelaMarcadas(t.nome)).length;
                        const isExpanded = modulosExpandidos.has(modulo.nome);
                        const todasModuloMarcadas = todasPermissoesModuloMarcadas(modulo);
                        
                        return (
                          <div key={modulo.nome} className="bg-[var(--surface)] rounded-xl border shadow-sm overflow-hidden">
                            {/* Header do Módulo */}
                            <div 
                              className={`flex items-center gap-3 px-4 py-3 cursor-pointer transition-colors ${
                                isExpanded ? 'bg-[var(--surface-muted)]' : 'hover:bg-[var(--surface-muted)]'
                              }`}
                              onClick={() => {
                                const novos = new Set(modulosExpandidos);
                                if (novos.has(modulo.nome)) {
                                  novos.delete(modulo.nome);
                                } else {
                                  novos.add(modulo.nome);
                                }
                                setModulosExpandidos(novos);
                              }}
                            >
                              <button className="p-1 hover:bg-gray-200 rounded transition-colors">
                                {isExpanded 
                                  ? <ChevronDown className="w-5 h-5 text-[var(--text-muted)]" />
                                  : <ChevronRight className="w-5 h-5 text-[var(--text-muted)]" />
                                }
                              </button>
                              
                              <div className={`p-2 rounded-lg ${
                                todasModuloMarcadas ? 'bg-green-100' : tabelasLiberadas > 0 ? 'bg-blue-100' : 'bg-gray-100'
                              }`}>
                                {moduleIcons[modulo.nome] || <Database className="w-4 h-4" />}
                              </div>
                              
                              <div className="flex-1">
                                <div className="font-semibold text-[var(--text)]">{modulo.nome}</div>
                                <div className="text-xs text-[var(--text-muted)]">
                                  {tabelasLiberadas}/{totalTabelas} tabelas com acesso • {tabelasCompletas} com acesso total
                                </div>
                              </div>

                              {/* Barra de Progresso Mini */}
                              <div className="w-32 h-2 bg-gray-200 rounded-full overflow-hidden">
                                <div 
                                  className={`h-full transition-all ${
                                    tabelasCompletas === totalTabelas ? 'bg-green-500' : 
                                    tabelasLiberadas > 0 ? 'bg-blue-500' : 'bg-gray-300'
                                  }`}
                                  style={{ width: `${(tabelasLiberadas / totalTabelas) * 100}%` }}
                                />
                              </div>

                              {/* Ações do Módulo */}
                              {isAdmin && (
                                <div className="flex items-center gap-1" onClick={e => e.stopPropagation()}>
                                  <button
                                    onClick={() => handleToggleTodasPermissoesModulo(modulo, true)}
                                    className="p-2 text-green-600 hover:bg-green-100 rounded-lg transition-colors"
                                    title="Liberar todo o módulo"
                                  >
                                    <CheckSquare className="w-5 h-5" />
                                  </button>
                                  <button
                                    onClick={() => handleToggleTodasPermissoesModulo(modulo, false)}
                                    className="p-2 text-red-600 hover:bg-red-100 rounded-lg transition-colors"
                                    title="Bloquear todo o módulo"
                                  >
                                    <Square className="w-5 h-5" />
                                  </button>
                                </div>
                              )}
                            </div>

                            {/* Tabelas do Módulo */}
                            {isExpanded && (
                              <div className="border-t">
                                {/* Cabeçalho da Tabela */}
                                <div className="grid grid-cols-12 gap-2 px-4 py-2 bg-[var(--surface-muted)] border-b text-xs font-medium text-[var(--text-muted)] uppercase">
                                  <div className="col-span-5">Tabela</div>
                                  <div className="col-span-1 text-center">
                                    <div className="flex flex-col items-center gap-0.5">
                                      <Eye className="w-4 h-4" />
                                      <span>Ver</span>
                                    </div>
                                  </div>
                                  <div className="col-span-1 text-center">
                                    <div className="flex flex-col items-center gap-0.5">
                                      <Plus className="w-4 h-4" />
                                      <span>Incluir</span>
                                    </div>
                                  </div>
                                  <div className="col-span-1 text-center">
                                    <div className="flex flex-col items-center gap-0.5">
                                      <Edit className="w-4 h-4" />
                                      <span>Editar</span>
                                    </div>
                                  </div>
                                  <div className="col-span-1 text-center">
                                    <div className="flex flex-col items-center gap-0.5">
                                      <Trash2 className="w-4 h-4" />
                                      <span>Excluir</span>
                                    </div>
                                  </div>
                                  <div className="col-span-3 text-center">Ações Rápidas</div>
                                </div>

                                {/* Linhas das Tabelas */}
                                {modulo.tabelas.map((tabela, idx) => {
                                  const perm = getPermissao(tabela.nome);
                                  const modificada = permissoesModificadas.has(tabela.nome);
                                  const todasMarcadas = todasPermissoesTelaMarcadas(tabela.nome);
                                  const nenhumaMarcada = !perm.visualiza && !perm.inclui && !perm.modifica && !perm.exclui;
                                  
                                  return (
                                    <div 
                                      key={tabela.nome}
                                      className={`grid grid-cols-12 gap-2 px-4 py-3 items-center transition-colors ${
                                        modificada ? 'bg-blue-50 border-l-4 border-l-blue-500' : 
                                        idx % 2 === 0 ? 'bg-[var(--surface)]' : 'bg-gray-50/50'
                                      } hover:bg-gray-100`}
                                    >
                                      {/* Nome da Tabela */}
                                      <div className="col-span-5 flex items-center gap-2">
                                        <div className={`w-2 h-2 rounded-full ${
                                          todasMarcadas ? 'bg-green-500' : 
                                          perm.visualiza ? 'bg-blue-500' : 'bg-gray-300'
                                        }`} />
                                        <span className="text-sm font-medium text-gray-700">{tabela.nomeExibicao}</span>
                                        {modificada && (
                                          <span className="text-xs px-1.5 py-0.5 bg-blue-100 text-blue-700 rounded">modificado</span>
                                        )}
                                      </div>

                                      {/* Checkboxes de Permissão */}
                                      {(['visualiza', 'inclui', 'modifica', 'exclui'] as const).map(campo => (
                                        <div key={campo} className="col-span-1 flex justify-center">
                                          <label className="relative inline-flex items-center cursor-pointer">
                                            <input
                                              type="checkbox"
                                              checked={perm[campo]}
                                              onChange={e => handlePermissaoChange(tabela.nome, campo, e.target.checked, 'TABELA')}
                                              disabled={!isAdmin}
                                              className="sr-only peer"
                                            />
                                            <div className={`w-6 h-6 rounded-md border-2 flex items-center justify-center transition-all
                                              ${perm[campo] 
                                                ? 'bg-blue-600 border-blue-600' 
                                                : 'bg-[var(--surface)] border-gray-300 hover:border-blue-400'
                                              }
                                              ${!isAdmin ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'}
                                            `}>
                                              {perm[campo] && <Check className="w-4 h-4 text-white" />}
                                            </div>
                                          </label>
                                        </div>
                                      ))}

                                      {/* Ações Rápidas da Linha */}
                                      <div className="col-span-3 flex items-center justify-center gap-1">
                                        {isAdmin && (
                                          <>
                                            <button
                                              onClick={() => handleToggleTodasPermissoesTela(tabela.nome, tabela.nomeExibicao, true, 'TABELA')}
                                              disabled={todasMarcadas}
                                              className={`px-2 py-1 text-xs rounded font-medium transition-colors ${
                                                todasMarcadas 
                                                  ? 'bg-gray-100 text-gray-400 cursor-not-allowed' 
                                                  : 'bg-green-100 text-green-700 hover:bg-green-200'
                                              }`}
                                            >
                                              <Zap className="w-3 h-3 inline mr-1" />
                                              Todas
                                            </button>
                                            <button
                                              onClick={() => handleToggleTodasPermissoesTela(tabela.nome, tabela.nomeExibicao, false, 'TABELA')}
                                              disabled={nenhumaMarcada}
                                              className={`px-2 py-1 text-xs rounded font-medium transition-colors ${
                                                nenhumaMarcada 
                                                  ? 'bg-gray-100 text-gray-400 cursor-not-allowed' 
                                                  : 'bg-red-100 text-red-700 hover:bg-red-200'
                                              }`}
                                            >
                                              Nenhuma
                                            </button>
                                            <button
                                              onClick={() => {
                                                const novasPermissoes = new Map(permissoesModificadas);
                                                novasPermissoes.set(tabela.nome, {
                                                  projeto: ' ',
                                                  nome: tabela.nome,
                                                  nomeExibicao: tabela.nomeExibicao,
                                                  visualiza: true,
                                                  inclui: false,
                                                  modifica: false,
                                                  exclui: false,
                                                  tipo: 'TABELA',
                                                });
                                                setPermissoesModificadas(novasPermissoes);
                                              }}
                                              className="px-2 py-1 text-xs rounded font-medium bg-blue-100 text-blue-700 hover:bg-blue-200 transition-colors"
                                            >
                                              Só Ver
                                            </button>
                                          </>
                                        )}
                                      </div>
                                    </div>
                                  );
                                })}
                              </div>
                            )}
                          </div>
                        );
                      })}
                    </div>

                    {/* Barra de Ações Fixa no Rodapé */}
                    {isAdmin && permissoesModificadas.size > 0 && (
                      <div className="mt-4 p-4 bg-blue-50 border border-blue-200 rounded-xl flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="p-2 bg-blue-100 rounded-full">
                            <AlertTriangle className="w-5 h-5 text-blue-600" />
                          </div>
                          <div>
                            <div className="font-medium text-blue-800">Você tem alterações não salvas</div>
                            <div className="text-sm text-blue-600">{permissoesModificadas.size} tabela(s) modificada(s)</div>
                          </div>
                        </div>
                        <div className="flex items-center gap-2">
                          <button
                            onClick={() => setPermissoesModificadas(new Map())}
                            className="px-4 py-2 text-blue-700 hover:bg-blue-100 rounded-lg transition-colors font-medium"
                          >
                            Descartar
                          </button>
                          <button
                            onClick={handleSalvarPermissoes}
                            disabled={salvando}
                            className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors font-medium shadow-sm disabled:opacity-50 flex items-center gap-2"
                          >
                            {salvando ? (
                              <RefreshCw className="w-4 h-4 animate-spin" />
                            ) : (
                              <Save className="w-4 h-4" />
                            )}
                            Salvar Alterações
                          </button>
                        </div>
                      </div>
                    )}
                  </>
                )}
              </div>
            )}

            {/* Sem seleção */}
            {(tabAtiva === 'menus' || tabAtiva === 'tabelas') && !grupoSelecionado && (
              <div className="text-center py-12 text-[var(--text-muted)]">
                <Shield className="w-12 h-12 mx-auto mb-3 opacity-50" />
                <p>Selecione um grupo para gerenciar as permissões</p>
              </div>
            )}

            {/* Tab Templates de Permissões */}
            {tabAtiva === 'templates' && (
              <div className="h-full flex gap-4">
                {/* Lista de Templates */}
                <div className="w-80 bg-[var(--surface)] rounded-lg border flex flex-col">
                  {/* Header */}
                  <div className="p-4 border-b">
                    <div className="flex items-center justify-between mb-3">
                      <h3 className="font-semibold text-[var(--text)]">Templates</h3>
                      {isAdmin && !modoEdicaoTemplate && (
                        <button
                          onClick={handleNovoTemplate}
                          className="flex items-center gap-1 px-2 py-1 text-xs bg-purple-600 text-white rounded hover:bg-purple-700"
                        >
                          <Plus className="w-3 h-3" />
                          Novo
                        </button>
                      )}
                    </div>
                    <div className="relative">
                      <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--text-muted)]" />
                      <input
                        type="text"
                        placeholder="Buscar templates..."
                        value={buscaTemplate}
                        onChange={(e) => setBuscaTemplate(e.target.value)}
                        className="w-full pl-9 pr-3 py-2 bg-[var(--surface-muted)] border border-[var(--border)] rounded-lg text-sm focus:ring-2 focus:ring-purple-500"
                      />
                    </div>
                  </div>

                  {/* Lista */}
                  <div className="flex-1 overflow-y-auto p-2 space-y-1">
                    {templatesFiltrados.length === 0 ? (
                      <div className="text-center py-8 text-[var(--text-muted)] text-sm">
                        Nenhum template encontrado
                      </div>
                    ) : (
                      templatesFiltrados.map(template => (
                        <div
                          key={template.id}
                          onClick={() => handleSelecionarTemplate(template)}
                          className={`p-3 rounded-lg cursor-pointer transition-all ${
                            templateSelecionado?.id === template.id
                              ? 'bg-purple-50 dark:bg-purple-900/20 border border-purple-200 dark:border-purple-700'
                              : 'hover:bg-[var(--surface-muted)] border border-transparent'
                          }`}
                        >
                          <div className="flex items-start justify-between">
                            <div className="flex-1 min-w-0">
                              <div className="flex items-center gap-2">
                                <span className="font-medium text-[var(--text)] truncate text-sm">
                                  {template.nome}
                                </span>
                                {template.isPadrao && (
                                  <span title="Template padrão do sistema">
                                    <Lock className="w-3 h-3 text-amber-500" />
                                  </span>
                                )}
                              </div>
                              {template.descricao && (
                                <p className="text-xs text-[var(--text-muted)] mt-1 line-clamp-2">
                                  {template.descricao}
                                </p>
                              )}
                              <div className="flex items-center gap-2 mt-2 text-xs text-[var(--text-muted)]">
                                <Shield className="w-3 h-3" />
                                <span>{template.quantidadeTelas} telas</span>
                              </div>
                            </div>

                            {/* Ações rápidas */}
                            {isAdmin && (
                              <div className="flex items-center gap-1 ml-2">
                                <button
                                  onClick={(e) => { e.stopPropagation(); handleDuplicarTemplate(template); }}
                                  className="p-1 rounded text-[var(--text-muted)] hover:bg-blue-100 dark:hover:bg-blue-900/30 hover:text-blue-600"
                                  title="Duplicar"
                                >
                                  <Copy className="w-3.5 h-3.5" />
                                </button>
                                {!template.isPadrao && (
                                  <>
                                    <button
                                      onClick={(e) => { e.stopPropagation(); handleEditarTemplate(template); }}
                                      className="p-1 rounded text-[var(--text-muted)] hover:bg-amber-100 dark:hover:bg-amber-900/30 hover:text-amber-600"
                                      title="Editar"
                                    >
                                      <Edit className="w-3.5 h-3.5" />
                                    </button>
                                    <button
                                      onClick={(e) => { e.stopPropagation(); handleConfirmarExclusaoTemplate(template); }}
                                      className="p-1 rounded text-[var(--text-muted)] hover:bg-red-100 dark:hover:bg-red-900/30 hover:text-red-600"
                                      title="Excluir"
                                    >
                                      <Trash2 className="w-3.5 h-3.5" />
                                    </button>
                                  </>
                                )}
                              </div>
                            )}
                          </div>
                        </div>
                      ))
                    )}
                  </div>
                </div>

                {/* Painel de Edição/Visualização */}
                <div className="flex-1 flex flex-col bg-[var(--surface)] rounded-lg border overflow-hidden">
                  {modoEdicaoTemplate ? (
                    <>
                      {/* Header do formulário */}
                      <div className="border-b p-4">
                        <div className="flex items-center justify-between">
                          <div className="flex items-center gap-3">
                            <div className={`p-2 rounded-lg ${
                              modoEdicaoTemplate === 'criar' ? 'bg-green-100' : modoEdicaoTemplate === 'editar' ? 'bg-amber-100' : 'bg-blue-100'
                            }`}>
                              {modoEdicaoTemplate === 'criar' ? (
                                <Plus className="w-5 h-5 text-green-600" />
                              ) : modoEdicaoTemplate === 'editar' ? (
                                <Edit className="w-5 h-5 text-amber-600" />
                              ) : (
                                <Eye className="w-5 h-5 text-blue-600" />
                              )}
                            </div>
                            <div>
                              <h2 className="text-lg font-semibold text-[var(--text)]">
                                {modoEdicaoTemplate === 'criar' ? 'Novo Template' : modoEdicaoTemplate === 'editar' ? 'Editar Template' : 'Visualizar Template'}
                              </h2>
                              {modoEdicaoTemplate === 'visualizar' && templateSelecionado?.isPadrao && (
                                <p className="text-xs text-amber-600 flex items-center gap-1">
                                  <Lock className="w-3 h-3" />
                                  Template padrão (somente leitura)
                                </p>
                              )}
                            </div>
                          </div>

                          <div className="flex items-center gap-2">
                            {modoEdicaoTemplate === 'visualizar' && !templateSelecionado?.isPadrao && isAdmin && (
                              <button
                                onClick={() => setModoEdicaoTemplate('editar')}
                                className="flex items-center gap-2 px-3 py-2 text-amber-600 bg-amber-50 rounded-lg hover:bg-amber-100"
                              >
                                <Edit className="w-4 h-4" />
                                Editar
                              </button>
                            )}
                            {modoEdicaoTemplate !== 'visualizar' && isAdmin && (
                              <button
                                onClick={handleSalvarTemplate}
                                disabled={salvando}
                                className="flex items-center gap-2 px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700 disabled:opacity-50"
                              >
                                {salvando ? <RefreshCw className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                                Salvar
                              </button>
                            )}
                            <button
                              onClick={handleCancelarTemplate}
                              className="flex items-center gap-2 px-3 py-2 text-[var(--text-muted)] hover:bg-[var(--surface-muted)] rounded-lg"
                            >
                              <X className="w-4 h-4" />
                              {modoEdicaoTemplate === 'visualizar' ? 'Fechar' : 'Cancelar'}
                            </button>
                          </div>
                        </div>

                        {/* Campos do formulário */}
                        {modoEdicaoTemplate !== 'visualizar' && (
                          <div className="mt-4 grid grid-cols-2 gap-4">
                            <div>
                              <label className="block text-sm font-medium text-[var(--text)] mb-1">Nome *</label>
                              <input
                                type="text"
                                value={formTemplate.nome}
                                onChange={(e) => setFormTemplate(prev => ({ ...prev, nome: e.target.value }))}
                                placeholder="Ex: Vendedor Padrão"
                                className="w-full px-3 py-2 bg-[var(--surface-muted)] border rounded-lg text-sm focus:ring-2 focus:ring-purple-500"
                                maxLength={100}
                              />
                            </div>
                            <div>
                              <label className="block text-sm font-medium text-[var(--text)] mb-1">Descrição</label>
                              <input
                                type="text"
                                value={formTemplate.descricao}
                                onChange={(e) => setFormTemplate(prev => ({ ...prev, descricao: e.target.value }))}
                                placeholder="Descrição opcional"
                                className="w-full px-3 py-2 bg-[var(--surface-muted)] border rounded-lg text-sm focus:ring-2 focus:ring-purple-500"
                                maxLength={255}
                              />
                            </div>
                          </div>
                        )}

                        {modoEdicaoTemplate === 'visualizar' && (
                          <div className="mt-4">
                            <h3 className="font-semibold text-[var(--text)]">{formTemplate.nome}</h3>
                            {formTemplate.descricao && <p className="text-sm text-[var(--text-muted)] mt-1">{formTemplate.descricao}</p>}
                          </div>
                        )}
                      </div>

                      {/* Grid de permissões */}
                      <div className="flex-1 overflow-auto p-4">
                        <div className="bg-[var(--surface-muted)] rounded-lg border">
                          {/* Header da tabela */}
                          <div className="grid grid-cols-[1fr_60px_60px_60px_60px_60px] gap-2 p-3 bg-gray-100 dark:bg-gray-800 border-b text-xs font-medium text-[var(--text-muted)] uppercase">
                            <span>Tela</span>
                            <span className="text-center">Todas</span>
                            <span className="text-center">C</span>
                            <span className="text-center">I</span>
                            <span className="text-center">A</span>
                            <span className="text-center">E</span>
                          </div>

                          {/* Módulos e telas */}
                          <div className="divide-y divide-[var(--border)]">
                            {telasDisponiveisTemplates.map(modulo => (
                              <div key={modulo.nome}>
                                {/* Header do módulo */}
                                <button
                                  onClick={() => toggleModuloTemplate(modulo.nome)}
                                  className="w-full grid grid-cols-[1fr_60px_60px_60px_60px_60px] gap-2 p-3 bg-gray-50 dark:bg-gray-700/50 hover:bg-gray-100 dark:hover:bg-gray-700"
                                >
                                  <div className="flex items-center gap-2">
                                    {modulosExpandidosTemplates.has(modulo.nome) ? <ChevronDown className="w-4 h-4" /> : <ChevronRight className="w-4 h-4" />}
                                    <span className="p-1 rounded bg-purple-100 dark:bg-purple-900/30">
                                      {moduleIcons[modulo.nome] || <Settings className="w-4 h-4" />}
                                    </span>
                                    <span className="font-semibold text-sm text-[var(--text)]">{modulo.nome}</span>
                                    <span className="text-xs text-[var(--text-muted)]">({modulo.telas.length})</span>
                                  </div>

                                  {modoEdicaoTemplate !== 'visualizar' && isAdmin && (
                                    <div className="flex justify-center">
                                      <button
                                        onClick={(e) => { e.stopPropagation(); marcarTodasModuloTemplate(modulo.nome, !moduloTemTodasPermissoesTemplate(modulo.nome)); }}
                                        className={`p-1 rounded ${moduloTemTodasPermissoesTemplate(modulo.nome) ? 'text-purple-600 bg-purple-100' : 'text-[var(--text-muted)] hover:text-purple-600'}`}
                                        title="Marcar/desmarcar todas do módulo"
                                      >
                                        <Zap className="w-4 h-4" />
                                      </button>
                                    </div>
                                  )}
                                </button>

                                {/* Telas do módulo */}
                                {modulosExpandidosTemplates.has(modulo.nome) && (
                                  <div className="divide-y divide-[var(--border)]">
                                    {modulo.telas.map(tela => {
                                      const detalhe = getDetalheTemplate(modulo.nome, tela.tela);
                                      const todasMarcadas = detalhe?.consultar && detalhe?.incluir && detalhe?.alterar && detalhe?.excluir;

                                      return (
                                        <div
                                          key={`${modulo.nome}-${tela.tela}`}
                                          className="grid grid-cols-[1fr_60px_60px_60px_60px_60px] gap-2 p-3 pl-10 hover:bg-gray-50 dark:hover:bg-gray-800/50"
                                        >
                                          <div className="flex items-center gap-2">
                                            <span className="text-sm text-[var(--text)]">{tela.nomeTela}</span>
                                          </div>

                                          {/* Botão "Todas" */}
                                          <div className="flex justify-center">
                                            {modoEdicaoTemplate !== 'visualizar' && isAdmin ? (
                                              <button
                                                onClick={() => marcarTodasTemplate(modulo.nome, tela.tela, !todasMarcadas)}
                                                className={`p-1 rounded ${todasMarcadas ? 'text-purple-600 bg-purple-100' : 'text-[var(--text-muted)] hover:text-purple-600'}`}
                                              >
                                                <Check className="w-4 h-4" />
                                              </button>
                                            ) : (
                                              <div className={todasMarcadas ? 'text-purple-600' : 'text-[var(--text-muted)]'}>
                                                {todasMarcadas ? <Check className="w-4 h-4" /> : <span className="text-xs">-</span>}
                                              </div>
                                            )}
                                          </div>

                                          {/* Checkboxes de permissões */}
                                          {(['consultar', 'incluir', 'alterar', 'excluir'] as const).map(perm => (
                                            <div key={perm} className="flex justify-center">
                                              {modoEdicaoTemplate !== 'visualizar' && isAdmin ? (
                                                <input
                                                  type="checkbox"
                                                  checked={detalhe?.[perm] || false}
                                                  onChange={(e) => atualizarPermissaoTemplate(modulo.nome, tela.tela, perm, e.target.checked)}
                                                  className="w-4 h-4 text-purple-600 border-gray-300 rounded focus:ring-purple-500"
                                                />
                                              ) : (
                                                <div className={detalhe?.[perm] ? 'text-green-600' : 'text-red-400'}>
                                                  {detalhe?.[perm] ? <Check className="w-4 h-4" /> : <X className="w-4 h-4" />}
                                                </div>
                                              )}
                                            </div>
                                          ))}
                                        </div>
                                      );
                                    })}
                                  </div>
                                )}
                              </div>
                            ))}
                          </div>
                        </div>

                        {/* Contador */}
                        {modoEdicaoTemplate !== 'visualizar' && (
                          <div className="mt-4 flex items-center justify-between p-4 bg-purple-50 dark:bg-purple-900/20 rounded-lg">
                            <div className="flex items-center gap-2 text-purple-700 dark:text-purple-300">
                              <Shield className="w-5 h-5" />
                              <span className="font-medium">{formTemplate.detalhes.length} tela(s) configurada(s)</span>
                            </div>
                            <button
                              onClick={() => setFormTemplate(prev => ({ ...prev, detalhes: [] }))}
                              className="text-sm text-purple-600 hover:underline"
                            >
                              Limpar todas
                            </button>
                          </div>
                        )}
                      </div>
                    </>
                  ) : (
                    <div className="flex flex-col items-center justify-center h-full text-center p-8">
                      <div className="p-4 bg-purple-100 dark:bg-purple-900/30 rounded-full mb-4">
                        <FileStack className="w-12 h-12 text-purple-600 dark:text-purple-400" />
                      </div>
                      <h2 className="text-xl font-semibold text-[var(--text)] mb-2">Selecione um Template</h2>
                      <p className="text-[var(--text-muted)] mb-6 max-w-md">
                        Clique em um template na lista para visualizar ou crie um novo.
                      </p>
                      {isAdmin && (
                        <button
                          onClick={handleNovoTemplate}
                          className="flex items-center gap-2 px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700"
                        >
                          <Plus className="w-4 h-4" />
                          Criar Novo Template
                        </button>
                      )}
                    </div>
                  )}
                </div>
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Modal de Exclusão de Usuário/Grupo */}
      <ModalConfirmacao
        aberto={deleteModal.open}
        titulo={deleteModal.tipo === 'usuario' ? 'Excluir Usuário' : 'Excluir Grupo'}
        mensagem={
          deleteModal.tipo === 'usuario'
            ? 'Tem certeza que deseja excluir este usuário? Esta ação não pode ser desfeita.'
            : 'Tem certeza que deseja excluir este grupo? Os usuários do grupo serão movidos para "SEM GRUPO".'
        }
        nomeItem={deleteModal.nome}
        textoBotaoConfirmar="Excluir"
        processando={deleteModal.deleting}
        onConfirmar={confirmarExclusao}
        onCancelar={() => setDeleteModal({ open: false, tipo: 'usuario', nome: '', deleting: false })}
      />

      {/* Modal de Exclusão de Template */}
      <ModalConfirmacao
        aberto={deleteTemplateModal.open}
        titulo="Excluir Template"
        mensagem="Tem certeza que deseja excluir este template? Esta ação não pode ser desfeita."
        nomeItem={deleteTemplateModal.template?.nome || ''}
        textoBotaoConfirmar="Excluir"
        processando={deleteTemplateModal.deleting}
        onConfirmar={handleExcluirTemplate}
        onCancelar={() => setDeleteTemplateModal({ open: false, template: null, deleting: false })}
      />
    </div>
  );
}

