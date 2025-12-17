import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Trash2,
  FileText,
  CheckCircle,
  AlertCircle,
  Filter,
  XCircle,

} from 'lucide-react';
import { classificacaoFiscalService } from '../../services/Fiscal/classificacaoFiscalService';
import type {
  ClassificacaoFiscal,
  PagedResult,
  ClassificacaoFiscalFiltros
} from '../../types';

// Componentes reutilizáveis
import {
  ModalConfirmacao,
  AlertaErro,
  DataTable,
  CabecalhoPagina,
  type ColumnConfig
} from '../../components/common';

export default function ClassificacaoFiscalPage() {
  const navigate = useNavigate();

  // Estados
  const [data, setData] = useState<PagedResult<ClassificacaoFiscal> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Filtros
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroCampo, setFiltroCampo] = useState('codigo');
  const [filtroNcm, setFiltroNcm] = useState('');
  const [filtroDescricao, setFiltroDescricao] = useState('');
  const [filtroSomenteNFe, setFiltroSomenteNFe] = useState(false);
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [filtroTributacao, setFiltroTributacao] = useState<'todos' | 'vinculados' | 'pendentes'>('todos');
  const [showFilters, setShowFilters] = useState(false);

  // Paginação e Ordenação
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(25);


  // Modal de Exclusão
  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; ncm: string; deleting: boolean }>({
    open: false,
    id: 0,
    ncm: '',
    deleting: false,
  });

  // Carregar dados
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const filtro: ClassificacaoFiscalFiltros = {
        pageNumber,
        pageSize,
        ncm: filtroNcm || undefined,
        descricao: filtroDescricao || undefined,
        somenteNFe: filtroSomenteNFe || undefined,
        incluirInativos: filtroIncluirInativos || undefined,
        tributacao: filtroTributacao !== 'todos' ? filtroTributacao : undefined,
      };

      // Adaptação para busca genérica se necessário, ou usar os filtros específicos
      if (filtroBusca) {
        if (filtroCampo === 'ncm') filtro.ncm = filtroBusca;
        if (filtroCampo === 'descricao') filtro.descricao = filtroBusca;
      }

      const result = await classificacaoFiscalService.listar(filtro);
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar classificações fiscais:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, [pageNumber, pageSize, filtroNcm, filtroDescricao, filtroSomenteNFe, filtroIncluirInativos, filtroTributacao, filtroBusca]);

  const handleDeleteClick = (id: number, ncm: string) => {
    setDeleteModal({ open: true, id, ncm, deleting: false });
  };

  const handleDeleteConfirm = async () => {
    setDeleteModal((prev) => ({ ...prev, deleting: true }));
    try {
      await classificacaoFiscalService.excluir(deleteModal.id);
      setDeleteModal({ open: false, id: 0, ncm: '', deleting: false });
      await loadData();
    } catch (err: any) {
      console.error('Erro ao excluir:', err);
      setError(err.response?.data?.mensagem || 'Erro ao excluir classificação');
      setDeleteModal((prev) => ({ ...prev, deleting: false }));
    }
  };

  const handleClearFilters = () => {
    setFiltroNcm('');
    setFiltroDescricao('');
    setFiltroBusca('');
    setFiltroSomenteNFe(false);
    setFiltroIncluirInativos(false);
    setFiltroTributacao('todos');
    setPageNumber(1);
  };

  // Definição das colunas
  const columns: ColumnConfig<ClassificacaoFiscal>[] = [
    {
      key: 'sequenciaDaClassificacao',
      header: 'Código',
      width: '80px',
      sortable: true,
      render: (item) => (
        <span className="font-mono text-[var(--text-muted)]">
          {item.sequenciaDaClassificacao}
        </span>
      )
    },
    {
      key: 'ncm',
      header: 'NCM',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar NCM...',
      render: (item) => (
        <span className="inline-flex items-center px-2.5 py-1 bg-indigo-50 text-indigo-700 text-sm font-mono rounded-lg border border-indigo-100">
          {item.ncm.toString().padStart(8, '0')}
        </span>
      )
    },
    {
      key: 'descricaoDoNcm',
      header: 'Descrição',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar descrição...',
      render: (item) => (
        <span className="text-[var(--text)] line-clamp-2" title={item.descricaoDoNcm}>
          {item.descricaoDoNcm}
        </span>
      )
    },
    {
      key: 'classTribId',
      header: 'Tributação',
      align: 'center',
      render: (item) => (
        item.classTribId ? (
          <span className="inline-flex items-center gap-1.5 px-2.5 py-1 bg-emerald-50 text-emerald-700 text-xs font-medium rounded-full border border-emerald-100">
            <CheckCircle className="h-3.5 w-3.5" />
            Vinculado
          </span>
        ) : (
          <span className="inline-flex items-center gap-1.5 px-2.5 py-1 bg-amber-50 text-amber-600 text-xs font-medium rounded-full border border-amber-100">
            <AlertCircle className="h-3.5 w-3.5" />
            Pendente
          </span>
        )
      )
    },
    {
      key: 'cest',
      header: 'CEST',
      width: '100px',
      render: (item) => (
        <span className="text-sm text-muted-foreground font-mono">
          {item.cest || '-'}
        </span>
      )
    },
    {
      key: 'porcentagemDoIpi',
      header: 'IPI %',
      align: 'center',
      width: '80px',
      render: (item) => (
        <span className="text-[var(--text)]">
          {item.porcentagemDoIpi.toFixed(2)}%
        </span>
      )
    },
    {
      key: 'inativo',
      header: 'Status',
      align: 'center',
      width: '100px',
      render: (item) => (
        item.inativo ? (
          <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-red-100 text-red-700 border border-red-200">
            Inativo
          </span>
        ) : (
          <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-emerald-100 text-emerald-700 border border-emerald-200">
            Ativo
          </span>
        )
      )
    }
  ];

  return (
    <div className="space-y-6 pb-8">
      <CabecalhoPagina
        titulo="Classificação Fiscal"
        subtitulo="Gerencie os códigos NCM e configurações fiscais"
        icone={FileText}
        acoes={
          <button
            onClick={() => navigate('/classificacao-fiscal/novo')}
            className="flex items-center gap-2 px-4 py-2 bg-primary text-white rounded-xl hover:bg-primary/90 transition-all font-medium shadow-lg shadow-primary/25 active:scale-95"
          >
            <Plus className="w-5 h-5" />
            <span>Nova Classificação</span>
          </button>
        }
      >
        {/* Filtros Rápidos */}
        <div className="flex flex-wrap gap-3">
          <button
            onClick={() => setShowFilters(!showFilters)}
            className={`px-4 py-2 rounded-xl text-sm font-bold transition-all border flex items-center gap-2 ${showFilters
              ? 'bg-primary text-white border-primary shadow-md'
              : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-primary'
              }`}
          >
            <Filter className="w-4 h-4" />
            Filtros Avançados
            {(filtroNcm || filtroSomenteNFe || filtroIncluirInativos || filtroTributacao !== 'todos') && (
              <span className="w-2 h-2 bg-secondary rounded-full animate-pulse"></span>
            )}
          </button>

          {showFilters && (
            <div className="w-full mt-4 p-4 bg-surface border border-border rounded-xl shadow-sm animate-slide-up">
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">NCM</label>
                  <input
                    type="text"
                    value={filtroNcm}
                    onChange={(e) => setFiltroNcm(e.target.value)}
                    placeholder="00000000"
                    className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                  />
                </div>

                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">Tributação</label>
                  <select
                    value={filtroTributacao}
                    onChange={(e) => setFiltroTributacao(e.target.value as any)}
                    className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                  >
                    <option value="todos">Todos</option>
                    <option value="vinculados">Vinculados</option>
                    <option value="pendentes">Pendentes</option>
                  </select>
                </div>

                <div className="flex items-center gap-4 pt-6">
                  <label className="flex items-center gap-2 cursor-pointer group">
                    <input
                      type="checkbox"
                      checked={filtroSomenteNFe}
                      onChange={(e) => setFiltroSomenteNFe(e.target.checked)}
                      className="w-4 h-4 rounded border-border text-primary focus:ring-primary"
                    />
                    <span className="text-sm font-medium text-muted-foreground group-hover:text-primary transition-colors">Apenas NFe</span>
                  </label>

                  <label className="flex items-center gap-2 cursor-pointer group">
                    <input
                      type="checkbox"
                      checked={filtroIncluirInativos}
                      onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                      className="w-4 h-4 rounded border-border text-primary focus:ring-primary"
                    />
                    <span className="text-sm font-medium text-muted-foreground group-hover:text-primary transition-colors">Incluir Inativos</span>
                  </label>
                </div>

                <div className="flex items-end">
                  <button
                    onClick={handleClearFilters}
                    className="w-full px-4 py-2 text-sm font-medium text-red-600 bg-red-50 hover:bg-red-100 rounded-lg transition-colors flex items-center justify-center gap-2"
                  >
                    <XCircle className="w-4 h-4" />
                    Limpar Filtros
                  </button>
                </div>
              </div>
            </div>
          )}
        </div>
      </CabecalhoPagina>

      <div className="px-6">
        {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

        <DataTable
          data={data?.items || []}
          columns={columns}
          getRowKey={(item) => item.sequenciaDaClassificacao}
          loading={loading}
          totalItems={data?.totalCount}

          // Filtros e Ordenação Server-Side
          onFilterChange={(column, value) => {
            setFiltroCampo(column);
            setFiltroBusca(value);
            setPageNumber(1);
          }}

          onClearFilters={handleClearFilters}

          // Ações de Linha
          rowActions={(item) => (
            <div className="flex items-center justify-end gap-1">
              <button
                onClick={() => navigate(`/classificacao-fiscal/${item.sequenciaDaClassificacao}`)}
                className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                title="Editar"
              >
                <Edit2 className="h-4 w-4" />
              </button>
              <button
                onClick={() => handleDeleteClick(item.sequenciaDaClassificacao, item.ncm.toString().padStart(8, '0'))}
                className="p-2 text-muted-foreground hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                title="Inativar"
              >
                <Trash2 className="h-4 w-4" />
              </button>
            </div>
          )}
        />
      </div>

      <ModalConfirmacao
        aberto={deleteModal.open}
        titulo="Confirmar Inativação"
        mensagem="Tem certeza que deseja inativar esta classificação fiscal?"
        nomeItem={`NCM: ${deleteModal.ncm}`}
        textoBotaoConfirmar="Inativar"
        variante="danger"
        processando={deleteModal.deleting}
        onConfirmar={handleDeleteConfirm}
        onCancelar={() => setDeleteModal({ open: false, id: 0, ncm: '', deleting: false })}
      />
    </div>
  );
}
