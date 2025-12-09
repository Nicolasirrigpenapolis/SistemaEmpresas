import { useEffect, useMemo, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Trash2,
  Users,
  Building2,
  Truck,
  UserCircle,
  Filter,
  X,
  Phone,
  Mail,
  MapPin,
  Eye,
  Lock,
  Loader2,
} from 'lucide-react';
import { geralService } from '../../services/geralService';
import type { 
  GeralListDto, 
  PagedResult,
  TipoEntidade,
} from '../../types/geral';
import { TIPO_LABELS, TIPO_CORES, TIPO_SIGLAS, getTiposAtivos } from '../../types/geral';
import { usePermissaoTela } from '../../hooks/usePermissaoTela';

// Componentes reutilizáveis
import { 
  ModalConfirmacao, 
  Paginacao, 
  EstadoVazio, 
  EstadoCarregando,
  AlertaErro,
  SearchBar,
  type SearchColumn,
  type SortDirection,
} from '../../components/common';

// Colunas disponíveis para busca
const SEARCH_COLUMNS: SearchColumn[] = [
  { key: 'codigo', label: 'Código', placeholder: 'Buscar por código (ex: 123)...' },
  { key: 'nome', label: 'Nome / Razão Social', placeholder: 'Buscar por nome ou razão social...' },
  { key: 'fantasia', label: 'Nome Fantasia', placeholder: 'Buscar por nome fantasia...' },
  { key: 'cpfCnpj', label: 'CPF / CNPJ', placeholder: 'Buscar por CPF ou CNPJ...' },
  { key: 'email', label: 'E-mail', placeholder: 'Buscar por e-mail...' },
  { key: 'cidade', label: 'Cidade', placeholder: 'Buscar por cidade...' },
  { key: 'fone', label: 'Telefone', placeholder: 'Buscar por telefone...' },
];

// ============================================================================
// COMPONENTE PRINCIPAL
// ============================================================================
export default function GeralPage() {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  
  // Permissões da tela
  const { podeConsultar, podeIncluir, podeAlterar, podeExcluir, carregando: carregandoPermissoes } = usePermissaoTela('Geral');
  
  // Pega o tipo da URL (ex: /cadastros/geral?tipo=cliente)
  const tipoParam = (searchParams.get('tipo') as TipoEntidade) || 'todos';

  // Estados
  const [data, setData] = useState<PagedResult<GeralListDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // Filtros
  const [filtroTipo, setFiltroTipo] = useState<TipoEntidade>(tipoParam);
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroCampo, setFiltroCampo] = useState('nome');
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [showFilters, setShowFilters] = useState(false);
  
  // Paginação e Ordenação
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);
  const [sortDirection, setSortDirection] = useState<SortDirection>('asc');

  // Modal de Exclusão
  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; nome: string; deleting: boolean }>({
    open: false,
    id: 0,
    nome: '',
    deleting: false,
  });

  // ============================================================================
  // FUNÇÕES
  // ============================================================================
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const filtros: any = {
        pageNumber,
        pageSize,
        incluirInativos: filtroIncluirInativos,
      };

      if (filtroBusca) {
        filtros.busca = filtroBusca;
      }

      // Aplica filtro por tipo
      if (filtroTipo === 'cliente') filtros.cliente = true;
      if (filtroTipo === 'fornecedor') filtros.fornecedor = true;
      if (filtroTipo === 'transportadora') filtros.transportadora = true;
      if (filtroTipo === 'vendedor') filtros.vendedor = true;

      const result = await geralService.listar(filtros);
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar dados:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

  const handleView = (id: number) => {
    navigate(`/cadastros/geral/${id}/visualizar`);
  };

  const handleDeleteClick = (id: number, nome: string) => {
    setDeleteModal({ open: true, id, nome, deleting: false });
  };

  const handleDeleteConfirm = async () => {
    setDeleteModal((prev) => ({ ...prev, deleting: true }));
    try {
      await geralService.excluir(deleteModal.id);
      setDeleteModal({ open: false, id: 0, nome: '', deleting: false });
      await loadData();
    } catch (err: any) {
      console.error('Erro ao excluir:', err);
      setError(err.response?.data?.mensagem || 'Erro ao excluir registro');
      setDeleteModal((prev) => ({ ...prev, deleting: false }));
    }
  };

  const handleDelete = async (id: number, nome: string) => {
    handleDeleteClick(id, nome);
  };

  const handleTipoChange = (novoTipo: TipoEntidade) => {
    setFiltroTipo(novoTipo);
    setPageNumber(1);
    
    // Atualiza a URL
    if (novoTipo === 'todos') {
      searchParams.delete('tipo');
    } else {
      searchParams.set('tipo', novoTipo);
    }
    setSearchParams(searchParams);
  };

  const handleClearFilters = () => {
    setFiltroBusca('');
    setFiltroCampo('nome');
    setFiltroIncluirInativos(false);
    setSortDirection('asc');
    setPageNumber(1);
  };

  const handleClearAllFilters = () => {
    handleClearFilters();
    if (filtroTipo !== 'todos') {
      setFiltroTipo('todos');
      searchParams.delete('tipo');
      setSearchParams(searchParams);
    }
  };

  // Handler para busca do SearchBar
  const handleSearchBar = (column: string, value: string, direction: SortDirection) => {
    setFiltroCampo(column);
    setFiltroBusca(value);
    setSortDirection(direction);
    setPageNumber(1);
    // A busca será feita via useEffect quando esses estados mudarem
  };

  // Filtrar e ordenar items
  const sortedItems = useMemo(() => {
    if (!data?.items) return [];
    let items = [...data.items];
    const direction = sortDirection === 'asc' ? 1 : -1;

    // FILTRAR pelo campo selecionado e valor digitado
    if (filtroBusca.trim()) {
      const searchTerm = filtroBusca.trim().toLowerCase();
      
      items = items.filter((item) => {
        switch (filtroCampo) {
          case 'codigo':
            // Para código, busca exata ou que começa com o número digitado
            const codigoStr = String(item.sequenciaDoGeral);
            return codigoStr === searchTerm || codigoStr.startsWith(searchTerm);
          case 'nome':
            return item.razaoSocial.toLowerCase().includes(searchTerm);
          case 'fantasia':
            return (item.nomeFantasia || '').toLowerCase().includes(searchTerm);
          case 'cpfCnpj':
            return (item.cpfECnpj || '').toLowerCase().includes(searchTerm);
          case 'email':
            return (item.email || '').toLowerCase().includes(searchTerm);
          case 'cidade':
            return (`${item.cidade || ''} ${item.uf || ''}`).toLowerCase().includes(searchTerm);
          case 'fone':
            return (item.fone1 || '').toLowerCase().includes(searchTerm);
          default:
            return true;
        }
      });
    }

    // ORDENAR pelo campo selecionado
    items.sort((a, b) => {
      switch (filtroCampo) {
        case 'codigo':
          // Ordenação numérica para código
          return (a.sequenciaDoGeral - b.sequenciaDoGeral) * direction;
        case 'nome':
          return a.razaoSocial.localeCompare(b.razaoSocial) * direction;
        case 'fantasia':
          return (a.nomeFantasia || '').localeCompare(b.nomeFantasia || '') * direction;
        case 'cpfCnpj':
          return (a.cpfECnpj || '').localeCompare(b.cpfECnpj || '') * direction;
        case 'email':
          return (a.email || '').localeCompare(b.email || '') * direction;
        case 'cidade':
          return (`${a.cidade || ''}${a.uf || ''}`).localeCompare(`${b.cidade || ''}${b.uf || ''}`) * direction;
        case 'fone':
          return (a.fone1 || '').localeCompare(b.fone1 || '') * direction;
        default:
          return (a.sequenciaDoGeral - b.sequenciaDoGeral) * direction;
      }
    });

    return items;
  }, [data, filtroCampo, filtroBusca, sortDirection]);

  const getTipoIcon = (tipo: TipoEntidade) => {
    switch (tipo) {
      case 'cliente': return <Users className="w-4 h-4" />;
      case 'fornecedor': return <Building2 className="w-4 h-4" />;
      case 'transportadora': return <Truck className="w-4 h-4" />;
      case 'vendedor': return <UserCircle className="w-4 h-4" />;
      default: return <Users className="w-4 h-4" />;
    }
  };

  const getPageTitle = () => {
    if (filtroTipo === 'todos') return 'Cadastro Geral';
    return TIPO_LABELS[filtroTipo];
  };

  const getPageSubtitle = () => {
    if (filtroTipo === 'todos') return 'Clientes, Fornecedores, Transportadoras e Vendedores';
    return `Gerenciamento de ${TIPO_LABELS[filtroTipo].toLowerCase()}`;
  };

  const activeFilters = useMemo(() => {
    const list: { label: string; clear?: () => void }[] = [];
    if (filtroBusca) list.push({ label: `Busca: "${filtroBusca}"`, clear: () => setFiltroBusca('') });
    if (filtroIncluirInativos) list.push({ label: 'Incluindo inativos', clear: () => setFiltroIncluirInativos(false) });
    if (filtroTipo !== 'todos') list.push({ label: `Tipo: ${TIPO_LABELS[filtroTipo]}` });
    return list;
  }, [filtroBusca, filtroIncluirInativos, filtroTipo]);

  // ============================================================================
  // EFFECTS
  // ============================================================================
  useEffect(() => {
    loadData();
  }, [pageNumber, pageSize, filtroTipo, filtroIncluirInativos]);

  useEffect(() => {
    // Sincroniza com a URL
    const tipoUrl = searchParams.get('tipo') as TipoEntidade;
    if (tipoUrl && tipoUrl !== filtroTipo) {
      setFiltroTipo(tipoUrl);
    }
  }, [searchParams]);

  // ============================================================================
  // RENDER
  // ============================================================================

  // Carregando permissões
  if (carregandoPermissoes) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <div className="text-center">
          <Loader2 className="h-8 w-8 animate-spin text-blue-600 mx-auto mb-4" />
          <p className="text-gray-500">Verificando permissões...</p>
        </div>
      </div>
    );
  }

  // Sem permissão de consulta
  if (!podeConsultar) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <div className="text-center bg-white p-8 rounded-xl shadow-lg max-w-md">
          <Lock className="h-12 w-12 text-red-500 mx-auto mb-4" />
          <h2 className="text-xl font-bold text-gray-900 mb-2">Acesso Negado</h2>
          <p className="text-gray-500">Você não tem permissão para acessar esta tela.</p>
          <p className="text-sm text-gray-400 mt-2">Entre em contato com o administrador para solicitar acesso.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-4 md:space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 md:gap-4">
        <div>
          <h1 className="text-xl md:text-2xl font-bold text-[var(--text)]">{getPageTitle()}</h1>
          <p className="text-xs md:text-sm text-[var(--text-muted)] hidden sm:block">{getPageSubtitle()}</p>
        </div>
        
        {podeIncluir && (
          <button
            onClick={() => navigate('/cadastros/geral/novo')}
            className="flex items-center justify-center gap-2 px-4 md:px-5 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-xl hover:from-blue-700 hover:to-blue-800 transition-all shadow-lg shadow-blue-500/30 font-medium text-sm md:text-base"
          >
            <Plus className="w-4 h-4" />
            <span className="hidden sm:inline">Novo Cadastro</span>
            <span className="sm:hidden">Novo</span>
          </button>
        )}
      </div>

      {/* Tabs de Tipo */}
      <div className="bg-[var(--surface)] border border-[var(--border)] rounded-xl shadow-sm">
        <div className="flex gap-1 overflow-x-auto p-1 scrollbar-hide -mx-1 px-1">
          {(['todos', 'cliente', 'fornecedor', 'transportadora', 'vendedor'] as TipoEntidade[]).map((tipo) => (
            <button
              key={tipo}
              onClick={() => handleTipoChange(tipo)}
              className={`flex items-center gap-2 px-3 md:px-4 py-2.5 md:py-3 text-xs md:text-sm font-medium rounded-lg whitespace-nowrap transition-all ${
                filtroTipo === tipo
                  ? 'bg-blue-50 text-blue-600'
                  : 'text-[var(--text-muted)] hover:text-[var(--text)] hover:bg-[var(--surface-muted)]'
              }`}
            >
              {tipo !== 'todos' && (
                <span className={filtroTipo === tipo ? '' : 'opacity-60'}>
                  {getTipoIcon(tipo)}
                </span>
              )}
              <span>{TIPO_LABELS[tipo]}</span>
              {filtroTipo === tipo && data && (
                <span className="ml-1 px-2 py-0.5 text-xs bg-blue-100 text-blue-700 rounded-full">
                  {data.total}
                </span>
              )}
            </button>
          ))}
        </div>
      </div>

      {/* Barra de Busca com Seleção de Coluna */}
      <SearchBar
        columns={SEARCH_COLUMNS}
        onSearch={handleSearchBar}
        onClear={handleClearFilters}
        initialValue={filtroBusca}
        initialColumn={filtroCampo}
        initialSortDirection={sortDirection}
        loading={loading}
      />

      {/* Filtros Extras */}
      <div className="flex flex-wrap items-center gap-3">
        <button
          onClick={() => setShowFilters(!showFilters)}
          className={`flex items-center gap-2 px-4 py-2 border rounded-xl transition-all font-medium text-sm ${
            showFilters || filtroIncluirInativos
              ? 'border-blue-500 text-blue-600 bg-blue-50'
              : 'border-[var(--border)] text-[var(--text)] hover:bg-[var(--surface-muted)] bg-[var(--surface)] shadow-sm'
          }`}
        >
          <Filter className="w-4 h-4" />
          <span>Mais Filtros</span>
          {filtroIncluirInativos && (
            <span className="w-2 h-2 bg-blue-500 rounded-full"></span>
          )}
        </button>

        {/* Filtros Expandidos */}
        {showFilters && (
          <>
            <label className="flex items-center gap-3 px-4 py-2 bg-[var(--surface)] border border-[var(--border)] rounded-xl cursor-pointer hover:bg-[var(--surface-muted)] transition-colors">
              <input
                type="checkbox"
                checked={filtroIncluirInativos}
                onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
              />
              <span className="text-sm text-[var(--text)] font-medium">Incluir inativos</span>
            </label>

            {(filtroBusca || filtroIncluirInativos) && (
              <button
                onClick={handleClearFilters}
                className="flex items-center gap-1.5 text-sm text-[var(--text-muted)] hover:text-red-600 transition-colors"
              >
                <X className="w-4 h-4" />
                <span>Limpar tudo</span>
              </button>
            )}
          </>
        )}
      </div>

      {activeFilters.length > 0 && (
        <div className="flex flex-wrap items-center gap-2 -mt-2">
          {activeFilters.map((filter, idx) => (
            <span
              key={idx}
              className="inline-flex items-center gap-2 px-3 py-1.5 bg-[var(--surface)] border border-[var(--border)] rounded-full text-sm text-[var(--text)] shadow-sm"
            >
              {filter.label}
              {filter.clear && (
                <button
                  type="button"
                  onClick={filter.clear}
                  className="text-[var(--text-muted)] hover:text-red-500"
                >
                  <X className="w-3 h-3" />
                </button>
              )}
            </span>
          ))}
          <button
            type="button"
            onClick={handleClearAllFilters}
            className="text-sm text-blue-600 hover:text-blue-700 font-medium"
          >
            Limpar tudo
          </button>
        </div>
      )}

      {/* Conteúdo */}
      <div>
        {/* Mensagem de Erro */}
        {error && (
          <AlertaErro 
            mensagem={error} 
            fechavel 
            onFechar={() => setError(null)}
            className="mb-4"
          />
        )}

        {/* Loading */}
        {loading ? (
          <EstadoCarregando mensagem="Carregando cadastros..." />
        ) : (
          <>
            {/* Contador, ordenação e itens por página */}
            <div className="mb-4 flex flex-col lg:flex-row lg:items-center lg:justify-between gap-3">
              <p className="text-sm text-[var(--text-muted)]">
                <span className="font-semibold text-[var(--text)]">{data?.total || 0}</span> registro(s) encontrado(s)
              </p>
              <div className="flex flex-wrap items-center gap-3">
                <div className="flex items-center gap-2">
                  <span className="text-sm text-[var(--text-muted)]">Exibir</span>
                  <select
                    value={pageSize}
                    onChange={(e) => {
                      setPageSize(Number(e.target.value));
                      setPageNumber(1);
                    }}
                    className="px-3 py-1.5 text-sm border border-[var(--border)] rounded-lg bg-[var(--surface)] text-[var(--text)] focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500"
                  >
                    <option value={10}>10</option>
                    <option value={25}>25</option>
                    <option value={50}>50</option>
                    <option value={100}>100</option>
                  </select>
                  <span className="text-sm text-[var(--text-muted)]">por página</span>
                </div>
              </div>
            </div>

            {/* Tabela com visual moderno */}
            <div className="bg-[var(--surface)] rounded-2xl border border-[var(--border)] shadow-[var(--shadow-soft)] overflow-hidden">
              <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-[var(--border)]">
                  <thead className="bg-[var(--surface-muted)] sticky top-0 z-10">
                    <tr>
                      <th className="px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        Código
                      </th>
                      <th className="px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        Razão Social / Nome
                      </th>
                      <th className="px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        CNPJ/CPF
                      </th>
                      <th className="px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        Cidade/UF
                      </th>
                      <th className="px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        Contato
                      </th>
                      <th className="px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        Tipos
                      </th>
                      <th className="px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        Status
                      </th>
                      <th className="px-4 py-4 text-right text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                        Ações
                      </th>
                    </tr>
                  </thead>
                  <tbody className="bg-[var(--surface)] divide-y divide-[var(--border)]">
                    {sortedItems.length === 0 ? (
                      <tr>
                        <td colSpan={8}>
                          <EstadoVazio 
                            tipoBusca={!!filtroBusca}
                            acao={filtroBusca ? { texto: 'Limpar busca', onClick: handleClearFilters } : undefined}
                          />
                        </td>
                      </tr>
                    ) : (
                      sortedItems.map((item) => (
                        <tr 
                          key={item.sequenciaDoGeral} 
                          className={`hover:bg-blue-50/30 transition-colors ${item.inativo ? 'opacity-60' : ''}`}
                        >
                          <td className="px-4 py-4 whitespace-nowrap">
                            <span className="text-sm font-mono text-[var(--text-muted)] bg-[var(--surface-muted)] px-2 py-1 rounded">
                              {item.sequenciaDoGeral}
                            </span>
                          </td>
                          <td className="px-4 py-4">
                            <div className="text-sm font-semibold text-[var(--text)]">
                              {item.razaoSocial}
                            </div>
                            {item.nomeFantasia && item.nomeFantasia !== item.razaoSocial && (
                              <div className="text-xs text-[var(--text-muted)] mt-0.5">
                                {item.nomeFantasia}
                              </div>
                            )}
                          </td>
                          <td className="px-4 py-4 whitespace-nowrap text-sm text-[var(--text-muted)] font-mono">
                            {item.cpfECnpj || '-'}
                          </td>
                          <td className="px-4 py-4 whitespace-nowrap">
                            {item.cidade ? (
                              <div className="flex items-center gap-1.5 text-sm text-[var(--text-muted)]">
                                <MapPin className="w-3.5 h-3.5 text-[var(--text-muted)] opacity-80" />
                                <span>{item.cidade}/{item.uf}</span>
                              </div>
                            ) : (
                              <span className="text-sm text-[var(--text-muted)]">-</span>
                            )}
                          </td>
                          <td className="px-4 py-4">
                            <div className="space-y-1.5">
                              {item.fone1 && (
                                <div className="flex items-center gap-1.5 text-sm text-[var(--text-muted)]">
                                  <Phone className="w-3.5 h-3.5 text-[var(--text-muted)] opacity-80" />
                                  <span>{item.fone1}</span>
                                </div>
                              )}
                              {item.email && (
                                <div className="flex items-center gap-1.5 text-sm text-[var(--text-muted)]">
                                  <Mail className="w-3.5 h-3.5 text-[var(--text-muted)] opacity-80" />
                                  <span className="truncate max-w-[150px]">{item.email}</span>
                                </div>
                              )}
                              {!item.fone1 && !item.email && (
                                <span className="text-sm text-[var(--text-muted)]">-</span>
                              )}
                            </div>
                          </td>
                          <td className="px-4 py-4">
                            <div className="flex flex-wrap gap-1">
                              {getTiposAtivos(item).map((tipo) => (
                                <span
                                  key={tipo}
                                  className={`inline-flex items-center px-2 py-0.5 rounded-md text-[10px] font-bold uppercase tracking-wide ${TIPO_CORES[tipo as TipoEntidade]}`}
                                  title={TIPO_LABELS[tipo as TipoEntidade]}
                                >
                                  {TIPO_SIGLAS[tipo as TipoEntidade]}
                                </span>
                              ))}
                            </div>
                          </td>
                          <td className="px-4 py-4 whitespace-nowrap">
                            {item.inativo ? (
                              <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-700">
                                Inativo
                              </span>
                            ) : (
                              <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-semibold bg-emerald-100 text-emerald-700">
                                Ativo
                              </span>
                            )}
                          </td>
                          <td className="px-4 py-4 whitespace-nowrap text-right">
                            <div className="flex items-center justify-end gap-1">
                              <button
                                onClick={() => handleView(item.sequenciaDoGeral)}
                                className="p-2 text-[var(--text-muted)] hover:text-indigo-600 hover:bg-indigo-50 rounded-lg transition-all"
                                title="Visualizar"
                              >
                                <Eye className="w-4 h-4" />
                              </button>
                              {podeAlterar && (
                                <button
                                  onClick={() => navigate(`/cadastros/geral/${item.sequenciaDoGeral}`)}
                                  className="p-2 text-[var(--text-muted)] hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-all"
                                  title="Editar"
                                >
                                  <Edit2 className="w-4 h-4" />
                                </button>
                              )}
                              {podeExcluir && (
                                <button
                                  onClick={() => handleDelete(item.sequenciaDoGeral, item.razaoSocial)}
                                  className="p-2 text-[var(--text-muted)] hover:text-red-600 hover:bg-red-50 rounded-lg transition-all"
                                  title="Inativar"
                                >
                                  <Trash2 className="w-4 h-4" />
                                </button>
                              )}
                            </div>
                          </td>
                        </tr>
                      ))
                    )}
                  </tbody>
                </table>
              </div>

              {/* Paginação */}
              {data && data.totalPages > 0 && (
                <Paginacao
                  paginaAtual={data.pageNumber}
                  totalPaginas={data.totalPages}
                  totalItens={data.total}
                  itensPorPagina={pageSize}
                  onMudarPagina={setPageNumber}
                  carregando={loading}
                />
              )}
            </div>
          </>
        )}
      </div>

      {/* Modal de Exclusão */}
      <ModalConfirmacao
        aberto={deleteModal.open}
        titulo="Confirmar Inativação"
        mensagem="Tem certeza que deseja inativar este cadastro? Esta ação pode ser revertida posteriormente."
        nomeItem={deleteModal.nome}
        textoBotaoConfirmar="Inativar"
        processando={deleteModal.deleting}
        onConfirmar={handleDeleteConfirm}
        onCancelar={() => setDeleteModal({ open: false, id: 0, nome: '', deleting: false })}
      />
    </div>
  );
}
