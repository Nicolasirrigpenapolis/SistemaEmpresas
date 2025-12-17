import { useEffect, useState } from 'react';
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
  Search,
  Briefcase
} from 'lucide-react';
import { geralService } from '../../services/Geral/geralService';
import type {
  GeralListDto,
  PagedResult,
  TipoEntidade,
} from '../../types';
import { TIPO_LABELS, TIPO_CORES, TIPO_SIGLAS, getTiposAtivos } from '../../types';
import { usePermissaoTela } from '../../hooks/usePermissaoTela';

// Componentes reutilizáveis
import {
  ModalConfirmacao,
  AlertaErro,
  CabecalhoPagina,
  DataTable,
  type ColumnConfig
} from '../../components/common';

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
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [showFilters, setShowFilters] = useState(false);

  // Paginação e Ordenação (gerenciados pelo DataTable, mas precisamos manter estado para API)
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(20);

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
    setFiltroIncluirInativos(false);
    setPageNumber(1);
  };

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

  // ============================================================================
  // EFFECTS
  // ============================================================================
  useEffect(() => {
    loadData();
  }, [pageNumber, pageSize, filtroTipo, filtroIncluirInativos]); // Removido filtroBusca do array de dependências para evitar busca a cada digitação se não houver debounce

  // Debounce para busca
  useEffect(() => {
    const timer = setTimeout(() => {
      loadData();
    }, 500);
    return () => clearTimeout(timer);
  }, [filtroBusca]);

  useEffect(() => {
    // Sincroniza com a URL
    const tipoUrl = searchParams.get('tipo') as TipoEntidade;
    if (tipoUrl && tipoUrl !== filtroTipo) {
      setFiltroTipo(tipoUrl);
    }
  }, [searchParams]);

  // ============================================================================
  // COLUNAS DATATABLE
  // ============================================================================
  const columns: ColumnConfig<GeralListDto>[] = [
    {
      key: 'sequenciaDoGeral',
      header: 'Código',
      width: '80px',
      render: (item) => (
        <span className="text-sm font-mono text-[var(--text-muted)]">
          {item.sequenciaDoGeral}
        </span>
      )
    },
    {
      key: 'razaoSocial',
      header: 'Razão Social / Nome',
      render: (item) => (
        <div>
          <div className="text-sm font-medium text-[var(--text)]">
            {item.razaoSocial}
          </div>
          {item.nomeFantasia && item.nomeFantasia !== item.razaoSocial && (
            <div className="text-xs text-muted-foreground mt-0.5">
              {item.nomeFantasia}
            </div>
          )}
        </div>
      )
    },
    {
      key: 'cpfECnpj',
      header: 'CNPJ/CPF',
      width: '140px',
      render: (item) => (
        <span className="text-sm text-muted-foreground font-mono">
          {item.cpfECnpj || '-'}
        </span>
      )
    },
    {
      key: 'cidade',
      header: 'Cidade/UF',
      render: (item) => (
        item.cidade ? (
          <div className="flex items-center gap-1.5 text-sm text-muted-foreground">
            <MapPin className="w-3.5 h-3.5 opacity-80" />
            <span>{item.cidade}/{item.uf}</span>
          </div>
        ) : (
          <span className="text-sm text-muted-foreground">-</span>
        )
      )
    },
    {
      key: 'fone1',
      header: 'Contato',
      render: (item) => (
        <div className="space-y-1.5">
          {item.fone1 && (
            <div className="flex items-center gap-1.5 text-sm text-muted-foreground">
              <Phone className="w-3.5 h-3.5 opacity-80" />
              <span>{item.fone1}</span>
            </div>
          )}
          {item.email && (
            <div className="flex items-center gap-1.5 text-sm text-muted-foreground">
              <Mail className="w-3.5 h-3.5 opacity-80" />
              <span className="truncate max-w-[150px]" title={item.email}>{item.email}</span>
            </div>
          )}
          {!item.fone1 && !item.email && (
            <span className="text-sm text-muted-foreground">-</span>
          )}
        </div>
      )
    },
    {
      key: 'tipos', // Chave virtual, não existe no DTO diretamente mas usamos para render
      header: 'Tipos',
      render: (item) => (
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
      )
    },
    {
      key: 'inativo',
      header: 'Status',
      width: '100px',
      render: (item) => (
        item.inativo ? (
          <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-red-100 text-red-700">
            Inativo
          </span>
        ) : (
          <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-emerald-100 text-emerald-700">
            Ativo
          </span>
        )
      )
    }
  ];

  // ============================================================================
  // RENDER
  // ============================================================================

  // Carregando permissões
  if (carregandoPermissoes) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <div className="text-center">
          <Loader2 className="h-8 w-8 animate-spin text-primary mx-auto mb-4" />
          <p className="text-muted-foreground">Verificando permissões...</p>
        </div>
      </div>
    );
  }

  // Sem permissão de consulta
  if (!podeConsultar) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <div className="text-center bg-surface p-8 rounded-xl shadow-lg max-w-md border border-border">
          <Lock className="h-12 w-12 text-red-500 mx-auto mb-4" />
          <h2 className="text-xl font-bold text-foreground mb-2">Acesso Negado</h2>
          <p className="text-muted-foreground">Você não tem permissão para acessar esta tela.</p>
          <p className="text-sm text-muted-foreground/70 mt-2">Entre em contato com o administrador para solicitar acesso.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6 pb-8">
      <CabecalhoPagina
        titulo={getPageTitle()}
        subtitulo={getPageSubtitle()}
        icone={Briefcase}
        acoes={
          podeIncluir && (
            <button
              onClick={() => navigate('/cadastros/geral/novo')}
              className="flex items-center justify-center gap-2 px-4 py-2 bg-primary text-primary-foreground rounded-xl hover:bg-primary/90 transition-all shadow-lg shadow-primary/20 font-medium"
            >
              <Plus className="w-4 h-4" />
              <span className="hidden sm:inline">Novo Cadastro</span>
              <span className="sm:hidden">Novo</span>
            </button>
          )
        }
      >
        {/* Filtros */}
        <div className="flex flex-col gap-4 w-full">
          {/* Tabs de Tipo */}
          <div className="flex gap-1 overflow-x-auto pb-2 scrollbar-hide">
            {(['todos', 'cliente', 'fornecedor', 'transportadora', 'vendedor'] as TipoEntidade[]).map((tipo) => (
              <button
                key={tipo}
                onClick={() => handleTipoChange(tipo)}
                className={`flex items-center gap-2 px-3 py-2 text-sm font-medium rounded-lg whitespace-nowrap transition-all border ${filtroTipo === tipo
                  ? 'bg-primary/10 text-primary border-primary/20'
                  : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-foreground'
                  }`}
              >
                {tipo !== 'todos' && (
                  <span className={filtroTipo === tipo ? '' : 'opacity-60'}>
                    {getTipoIcon(tipo)}
                  </span>
                )}
                <span>{TIPO_LABELS[tipo]}</span>
              </button>
            ))}
          </div>

          <div className="flex flex-wrap gap-3 items-center">
            <div className="relative flex-1 min-w-[200px]">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
              <input
                type="text"
                value={filtroBusca}
                onChange={(e) => setFiltroBusca(e.target.value)}
                placeholder="Buscar por nome, código, CPF/CNPJ..."
                className="w-full pl-9 pr-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
              />
            </div>

            <button
              onClick={() => setShowFilters(!showFilters)}
              className={`px-3 py-2 rounded-lg text-sm font-medium transition-all border flex items-center gap-2 ${showFilters || filtroIncluirInativos
                ? 'bg-primary/10 text-primary border-primary/20'
                : 'bg-surface-muted text-muted-foreground border-border hover:bg-surface-hover'
                }`}
            >
              <Filter className="w-4 h-4" />
              Filtros
            </button>

            {(filtroBusca || filtroIncluirInativos) && (
              <button
                onClick={handleClearFilters}
                className="px-3 py-2 text-sm font-medium text-red-600 bg-red-50 hover:bg-red-100 rounded-lg transition-colors flex items-center gap-2"
              >
                <X className="w-4 h-4" />
                Limpar
              </button>
            )}
          </div>

          {/* Filtros Expandidos */}
          {showFilters && (
            <div className="pt-2 animate-slide-down">
              <label className="flex items-center gap-3 px-4 py-3 bg-surface-muted/50 border border-border rounded-xl cursor-pointer hover:bg-surface-muted transition-colors w-fit">
                <input
                  type="checkbox"
                  checked={filtroIncluirInativos}
                  onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                  className="w-4 h-4 text-primary border-border rounded focus:ring-primary"
                />
                <span className="text-sm text-foreground font-medium">Incluir inativos</span>
              </label>
            </div>
          )}
        </div>
      </CabecalhoPagina>

      <div className="px-6">
        {/* Mensagem de Erro */}
        {error && (
          <AlertaErro
            mensagem={error}
            fechavel
            onFechar={() => setError(null)}
            className="mb-6"
          />
        )}

        <DataTable
          data={data?.items || []}
          columns={columns}
          getRowKey={(item) => item.sequenciaDoGeral.toString()}
          loading={loading}
          totalItems={data?.totalCount || 0}
          rowActions={(item) => (
            <>
              <button
                onClick={() => handleView(item.sequenciaDoGeral)}
                className="p-2 text-muted-foreground hover:text-primary hover:bg-surface-hover rounded-lg transition-colors"
                title="Visualizar"
              >
                <Eye className="h-4 w-4" />
              </button>
              {podeAlterar && (
                <button
                  onClick={() => navigate(`/cadastros/geral/${item.sequenciaDoGeral}`)}
                  className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                  title="Editar"
                >
                  <Edit2 className="h-4 w-4" />
                </button>
              )}
              {podeExcluir && (
                <button
                  onClick={() => handleDeleteClick(item.sequenciaDoGeral, item.razaoSocial)}
                  className="p-2 text-muted-foreground hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                  title="Inativar"
                >
                  <Trash2 className="h-4 w-4" />
                </button>
              )}
            </>
          )}
        />

        {/* Paginação Manual (já que a API é paginada) */}
        {!loading && data && data.totalPages > 0 && (
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-border mt-4">
            <div className="text-sm text-muted-foreground">
              Página <span className="font-medium text-primary">{pageNumber}</span> de <span className="font-medium text-primary">{data.totalPages}</span>
            </div>
            <div className="flex items-center gap-2">
              <button
                onClick={() => setPageNumber(prev => Math.max(1, prev - 1))}
                disabled={pageNumber === 1}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Anterior
              </button>
              <button
                onClick={() => setPageNumber(prev => Math.min(data.totalPages, prev + 1))}
                disabled={pageNumber === data.totalPages}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Próxima
              </button>
            </div>
          </div>
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
