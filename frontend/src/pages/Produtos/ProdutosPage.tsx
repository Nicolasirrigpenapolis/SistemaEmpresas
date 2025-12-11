import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Eye,
  Package,
  AlertTriangle,
  Barcode,
  MapPin,
  TrendingUp,
  Trash2,
  Box,
  Layers
} from 'lucide-react';
import { produtoService } from '../../services/produtoService';
import type { ProdutoListDto, PagedResult, ProdutoFiltroDto } from '../../types/produto';

// Componentes reutilizáveis
import {
  ModalConfirmacao,
  AlertaErro,
  DataTable,
  CabecalhoPagina,
  type ColumnConfig
} from '../../components/common';

export default function ProdutosPage() {
  const navigate = useNavigate();

  // Estados
  const [data, setData] = useState<PagedResult<ProdutoListDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Filtros
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroEstoqueCritico, setFiltroEstoqueCritico] = useState(false);
  const [filtroMateriaPrima, setFiltroMateriaPrima] = useState<boolean | undefined>(undefined);
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);

  // Paginação
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);

  // Modal de inativação
  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; nome: string; deleting: boolean }>({
    open: false,
    id: 0,
    nome: '',
    deleting: false,
  });

  // Carregar dados
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const filtro: ProdutoFiltroDto = {
        pageNumber,
        pageSize,
        incluirInativos: filtroIncluirInativos,
        busca: filtroBusca || undefined,
        estoqueCritico: filtroEstoqueCritico || undefined,
        eMateriaPrima: filtroMateriaPrima,
      };

      const result = await produtoService.listar(filtro);
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar produtos:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar produtos');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, [pageNumber, pageSize, filtroEstoqueCritico, filtroMateriaPrima, filtroIncluirInativos, filtroBusca]);

  const handleView = (id: number) => {
    navigate(`/cadastros/produtos/${id}`);
  };

  const handleEdit = (id: number) => {
    navigate(`/cadastros/produtos/${id}/editar`);
  };

  const handleNew = () => {
    navigate('/cadastros/produtos/novo');
  };

  const handleInativarClick = (id: number, nome: string) => {
    setDeleteModal({ open: true, id, nome, deleting: false });
  };

  const handleInativarConfirm = async () => {
    setDeleteModal((prev) => ({ ...prev, deleting: true }));
    try {
      await produtoService.inativar(deleteModal.id);
      setDeleteModal({ open: false, id: 0, nome: '', deleting: false });
      await loadData();
    } catch (err: any) {
      console.error('Erro ao inativar:', err);
      setError(err.response?.data?.mensagem || 'Erro ao inativar produto');
      setDeleteModal((prev) => ({ ...prev, deleting: false }));
    }
  };

  // Definição das colunas
  const columns: ColumnConfig<ProdutoListDto>[] = [
    {
      key: 'sequenciaDoProduto',
      header: 'Código',
      width: '80px',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar código...',
      render: (item) => (
        <span className="font-mono text-[var(--text-muted)]">#{item.sequenciaDoProduto}</span>
      )
    },
    {
      key: 'descricao',
      header: 'Produto',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar produto...',
      render: (item) => (
        <div className="flex flex-col">
          <span className="font-medium text-[var(--text)]">{item.descricao}</span>
          <div className="flex items-center gap-3 mt-1">
            {item.codigoDeBarras && (
              <span className="text-xs text-[var(--text-muted)] flex items-center gap-1">
                <Barcode className="h-3 w-3" />
                {item.codigoDeBarras}
              </span>
            )}
            {item.localizacao && (
              <span className="text-xs text-[var(--text-muted)] flex items-center gap-1">
                <MapPin className="h-3 w-3" />
                {item.localizacao}
              </span>
            )}
          </div>
        </div>
      )
    },
    {
      key: 'grupoProduto',
      header: 'Grupo / Subgrupo',
      sortable: true,
      render: (item) => (
        <div className="flex flex-col">
          <span className="text-sm text-[var(--text)]">{item.grupoProduto}</span>
          {item.subGrupoProduto && (
            <span className="text-xs text-[var(--text-muted)]">{item.subGrupoProduto}</span>
          )}
        </div>
      )
    },
    {
      key: 'quantidadeNoEstoque',
      header: 'Estoque',
      align: 'right',
      sortable: true,
      render: (item) => (
        <div className="flex flex-col items-end">
          <span className={`font-medium ${item.estoqueCritico ? 'text-red-600' :
            item.quantidadeNoEstoque > item.quantidadeMinima * 2 ? 'text-emerald-600' : 'text-[var(--text)]'
            }`}>
            {new Intl.NumberFormat('pt-BR', { minimumFractionDigits: 2 }).format(item.quantidadeNoEstoque)}
          </span>
          {item.estoqueCritico && (
            <span className="text-[10px] font-medium text-red-600 bg-red-50 px-1.5 py-0.5 rounded-full flex items-center gap-1 mt-0.5">
              <AlertTriangle className="w-3 h-3" />
              Crítico
            </span>
          )}
        </div>
      )
    },
    {
      key: 'valorTotal',
      header: 'Preço Venda',
      align: 'right',
      sortable: true,
      render: (item) => (
        <div className="flex flex-col items-end">
          <span className="font-medium text-[var(--text)]">
            {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.valorTotal)}
          </span>
          {item.margemDeLucro > 0 && (
            <span className="text-xs text-emerald-600 flex items-center gap-0.5">
              <TrendingUp className="h-3 w-3" />
              {item.margemDeLucro.toFixed(1)}%
            </span>
          )}
        </div>
      )
    },
    {
      key: 'ativo',
      header: 'Status',
      align: 'center',
      width: '100px',
      render: (item) => (
        <div className="flex flex-col gap-1 items-center">
          <span className={`px-2.5 py-0.5 rounded-full text-xs font-medium ${!item.inativo
            ? 'bg-emerald-100 text-emerald-700 border border-emerald-200'
            : 'bg-gray-100 text-gray-600 border border-gray-200'
            }`}>
            {!item.inativo ? 'Ativo' : 'Inativo'}
          </span>
          {item.eMateriaPrima && (
            <span className="px-2 py-0.5 rounded-full text-[10px] font-medium bg-purple-100 text-purple-700 border border-purple-200">
              Matéria Prima
            </span>
          )}
        </div>
      )
    }
  ];

  return (
    <div className="space-y-6 pb-8">
      <CabecalhoPagina
        titulo="Produtos"
        subtitulo="Gerenciamento de catálogo e estoque"
        icone={Package}
        acoes={
          <button
            onClick={handleNew}
            className="flex items-center gap-2 px-4 py-2 bg-primary text-white rounded-xl hover:bg-primary/90 transition-all font-medium shadow-lg shadow-primary/25 active:scale-95"
          >
            <Plus className="w-5 h-5" />
            <span>Novo Produto</span>
          </button>
        }
      >
        {/* Filtros Rápidos */}
        <div className="flex flex-wrap gap-3">
          <button
            onClick={() => {
              setFiltroEstoqueCritico(false);
              setFiltroMateriaPrima(undefined);
              setFiltroIncluirInativos(false);
              setPageNumber(1);
            }}
            className={`px-4 py-2 rounded-xl text-sm font-bold transition-all border ${!filtroEstoqueCritico && filtroMateriaPrima === undefined && !filtroIncluirInativos
              ? 'bg-primary text-white border-primary shadow-md'
              : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-primary'
              }`}
          >
            Todos
          </button>

          <button
            onClick={() => {
              setFiltroMateriaPrima(false);
              setPageNumber(1);
            }}
            className={`px-4 py-2 rounded-xl text-sm font-bold transition-all border flex items-center gap-2 ${filtroMateriaPrima === false
              ? 'bg-emerald-600 text-white border-emerald-600 shadow-md'
              : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-emerald-600'
              }`}
          >
            <Box className="w-4 h-4" />
            Produtos Acabados
          </button>

          <button
            onClick={() => {
              setFiltroMateriaPrima(true);
              setPageNumber(1);
            }}
            className={`px-4 py-2 rounded-xl text-sm font-bold transition-all border flex items-center gap-2 ${filtroMateriaPrima === true
              ? 'bg-purple-600 text-white border-purple-600 shadow-md'
              : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-purple-600'
              }`}
          >
            <Layers className="w-4 h-4" />
            Matéria Prima
          </button>

          <button
            onClick={() => {
              setFiltroEstoqueCritico(!filtroEstoqueCritico);
              setPageNumber(1);
            }}
            className={`px-4 py-2 rounded-xl text-sm font-bold transition-all border flex items-center gap-2 ${filtroEstoqueCritico
              ? 'bg-red-600 text-white border-red-600 shadow-md'
              : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-red-600'
              }`}
          >
            <AlertTriangle className="w-4 h-4" />
            Estoque Crítico
          </button>

          <label className={`px-4 py-2 rounded-xl text-sm font-bold transition-all border flex items-center gap-2 cursor-pointer ${filtroIncluirInativos
            ? 'bg-gray-600 text-white border-gray-600 shadow-md'
            : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-gray-600'
            }`}>
            <input
              type="checkbox"
              className="hidden"
              checked={filtroIncluirInativos}
              onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
            />
            <span>Incluir Inativos</span>
          </label>
        </div>
      </CabecalhoPagina>

      <div className="px-6">
        {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

        <DataTable
          data={data?.items || []}
          columns={columns}
          getRowKey={(item) => item.sequenciaDoProduto}
          loading={loading}
          totalItems={data?.totalItems}

          // Filtros Server-Side
          onFilterChange={(_, value) => {
            setFiltroBusca(value);
            setPageNumber(1);
          }}
          onClearFilters={() => {
            setFiltroBusca('');
            setFiltroEstoqueCritico(false);
            setFiltroMateriaPrima(undefined);
            setFiltroIncluirInativos(false);
            setPageNumber(1);
          }}

          // Ações de Linha
          rowActions={(item) => (
            <div className="flex items-center justify-end gap-1">
              <button
                onClick={() => handleView(item.sequenciaDoProduto)}
                className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                title="Visualizar"
              >
                <Eye className="h-4 w-4" />
              </button>
              <button
                onClick={() => handleEdit(item.sequenciaDoProduto)}
                className="p-2 text-muted-foreground hover:text-emerald-600 hover:bg-emerald-50 rounded-lg transition-colors"
                title="Editar"
              >
                <Edit2 className="h-4 w-4" />
              </button>
              {!item.inativo && (
                <button
                  onClick={() => handleInativarClick(item.sequenciaDoProduto, item.descricao)}
                  className="p-2 text-muted-foreground hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                  title="Inativar"
                >
                  <Trash2 className="h-4 w-4" />
                </button>
              )}
            </div>
          )}
        />

        {/* Paginação */}
        {!loading && data && data.totalPages > 0 && (
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-border mt-4">
            <div className="flex items-center gap-4">
              <div className="text-sm text-muted-foreground">
                Página <span className="font-medium text-primary">{pageNumber}</span> de <span className="font-medium text-primary">{data.totalPages}</span>
              </div>
              <select
                value={pageSize}
                onChange={(e) => {
                  setPageSize(Number(e.target.value));
                  setPageNumber(1);
                }}
                className="px-2 py-1 text-sm border border-border rounded-lg bg-surface text-foreground focus:ring-2 focus:ring-primary/20 focus:border-primary"
              >
                <option value={10}>10 por página</option>
                <option value={25}>25 por página</option>
                <option value={50}>50 por página</option>
                <option value={100}>100 por página</option>
              </select>
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

      <ModalConfirmacao
        aberto={deleteModal.open}
        titulo="Inativar Produto"
        mensagem={`Tem certeza que deseja inativar o produto "${deleteModal.nome}"?`}
        textoBotaoConfirmar="Inativar"
        textoBotaoCancelar="Cancelar"
        variante="danger"
        processando={deleteModal.deleting}
        onConfirmar={handleInativarConfirm}
        onCancelar={() => setDeleteModal({ open: false, id: 0, nome: '', deleting: false })}
      />
    </div>
  );
}
