import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Eye,
  EyeOff,
  Package,
  AlertTriangle,
  Barcode,
  MapPin,
  TrendingUp,
  Trash2,
  Box,
  Layers,
  RefreshCw,
} from 'lucide-react';
import { produtoService } from '../../services/Produto/produtoService';
import type { ProdutoListDto, PagedResult, ProdutoFiltroDto } from '../../types';

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
      width: '100px',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar código...',
      render: (item) => (
        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
          #{item.sequenciaDoProduto}
        </span>
      )
    },
    {
      key: 'descricao',
      header: 'Produto',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar produto...',
      render: (item) => (
        <div className="flex flex-col gap-0.5">
          <span className="font-bold text-foreground leading-tight">{item.descricao}</span>
          <div className="flex items-center gap-3">
            {item.codigoDeBarras && (
              <span className="text-[11px] text-muted-foreground flex items-center gap-1 font-medium">
                <Barcode className="h-3 w-3 opacity-70" />
                {item.codigoDeBarras}
              </span>
            )}
            {item.localizacao && (
              <span className="text-[11px] text-muted-foreground flex items-center gap-1 font-medium">
                <MapPin className="h-3 w-3 opacity-70" />
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
          <span className="text-sm font-semibold text-foreground">{item.grupoProduto}</span>
          {item.subGrupoProduto && (
            <span className="text-xs text-muted-foreground font-medium">{item.subGrupoProduto}</span>
          )}
        </div>
      )
    },
    {
      key: 'quantidadeMinima',
      header: 'Estoque Mínimo',
      align: 'right',
      sortable: true,
      render: (item) => (
        <span className="text-sm font-medium text-muted-foreground">
          {item.quantidadeMinima > 0
            ? new Intl.NumberFormat('pt-BR', { minimumFractionDigits: 0 }).format(item.quantidadeMinima)
            : '-'
          }
        </span>
      )
    },
    {
      key: 'quantidadeContabil',
      header: 'Estoque Contábil',
      align: 'right',
      sortable: true,
      render: (item) => (
        <div className="flex flex-col items-end gap-1">
          <span className={`text-sm font-bold ${item.estoqueCritico ? 'text-red-600' :
            item.quantidadeContabil > item.quantidadeMinima * 2 ? 'text-emerald-600' : 'text-foreground'
            }`}>
            {new Intl.NumberFormat('pt-BR', { minimumFractionDigits: 2 }).format(item.quantidadeContabil)}
          </span>
          {item.estoqueCritico && (
            <span
              className="text-[10px] font-bold text-red-600 bg-red-50 px-2 py-0.5 rounded-full flex items-center gap-1 border border-red-100"
              title={`Estoque abaixo do mínimo (${new Intl.NumberFormat('pt-BR', { minimumFractionDigits: 2 }).format(item.quantidadeMinima)})`}
            >
              <AlertTriangle className="w-3 h-3" />
              CRÍTICO
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
        <div className="flex flex-col items-end gap-0.5">
          <span className="font-bold text-foreground">
            {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.valorTotal)}
          </span>
          {item.margemDeLucro > 0 && (
            <span className="text-[10px] font-bold text-emerald-600 bg-emerald-50 px-1.5 py-0.5 rounded-md flex items-center gap-0.5 border border-emerald-100">
              <TrendingUp className="h-2.5 w-2.5" />
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
      width: '120px',
      render: (item) => (
        <div className="flex flex-col gap-1.5 items-center">
          <span className={`px-3 py-1 rounded-full text-[10px] font-bold uppercase tracking-wider border ${!item.inativo
            ? 'bg-emerald-50 text-emerald-700 border-emerald-200'
            : 'bg-gray-50 text-gray-500 border-gray-200'
            }`}>
            {!item.inativo ? 'Ativo' : 'Inativo'}
          </span>
          {item.eMateriaPrima && (
            <span className="px-2 py-0.5 rounded-md text-[9px] font-bold uppercase bg-purple-50 text-purple-600 border border-purple-100">
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
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all font-medium shadow-lg shadow-blue-600/25 active:scale-95"
          >
            <Plus className="w-5 h-5" />
            <span>Novo Produto</span>
          </button>
        }
      />

      <div className="px-6">
        {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

        <DataTable
          data={data?.items || []}
          columns={columns}
          getRowKey={(item) => item.sequenciaDoProduto}
          loading={loading}
          totalItems={data?.totalCount}

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
          headerExtra={
            <div className="flex flex-wrap items-center gap-2">
              <div className="flex items-center bg-surface border border-border p-1 rounded-xl shadow-sm">
                <button
                  onClick={() => {
                    setFiltroEstoqueCritico(false);
                    setFiltroMateriaPrima(undefined);
                    setFiltroIncluirInativos(false);
                    setPageNumber(1);
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all ${!filtroEstoqueCritico && filtroMateriaPrima === undefined && !filtroIncluirInativos
                    ? 'bg-blue-600 text-white shadow-md shadow-blue-600/20'
                    : 'text-muted-foreground hover:text-foreground hover:bg-surface-hover'
                    }`}
                >
                  Todos
                </button>

                <button
                  onClick={() => {
                    setFiltroMateriaPrima(false);
                    setPageNumber(1);
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all flex items-center gap-2 ${filtroMateriaPrima === false
                    ? 'bg-emerald-600 text-white shadow-md shadow-emerald-600/20'
                    : 'text-muted-foreground hover:text-emerald-600 hover:bg-emerald-50'
                    }`}
                >
                  <Box className="w-3.5 h-3.5" />
                  Acabados
                </button>

                <button
                  onClick={() => {
                    setFiltroMateriaPrima(true);
                    setPageNumber(1);
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all flex items-center gap-2 ${filtroMateriaPrima === true
                    ? 'bg-purple-600 text-white shadow-md shadow-purple-600/20'
                    : 'text-muted-foreground hover:text-purple-600 hover:bg-purple-50'
                    }`}
                >
                  <Layers className="w-3.5 h-3.5" />
                  M. Prima
                </button>
              </div>

              <div className="flex items-center gap-2">
                <button
                  onClick={() => {
                    setFiltroEstoqueCritico(!filtroEstoqueCritico);
                    setPageNumber(1);
                  }}
                  className={`px-4 py-2 rounded-xl text-xs font-bold transition-all border flex items-center gap-2 ${filtroEstoqueCritico
                    ? 'bg-red-600 text-white border-red-600 shadow-md shadow-red-600/20'
                    : 'bg-surface text-muted-foreground border-border hover:border-red-600 hover:text-red-600 hover:bg-red-50'
                    }`}
                >
                  <AlertTriangle className="w-3.5 h-3.5" />
                  Estoque Crítico
                </button>

                <button
                  onClick={() => {
                    setFiltroIncluirInativos(!filtroIncluirInativos);
                    setPageNumber(1);
                  }}
                  className={`px-4 py-2 rounded-xl text-xs font-bold transition-all border flex items-center gap-2 ${filtroIncluirInativos
                    ? 'bg-gray-700 text-white border-gray-700 shadow-md shadow-gray-700/20'
                    : 'bg-surface text-muted-foreground border-border hover:border-gray-700 hover:text-gray-700 hover:bg-gray-50'
                    }`}
                >
                  {filtroIncluirInativos ? <Eye className="w-3.5 h-3.5" /> : <EyeOff className="w-3.5 h-3.5" />}
                  Inativos
                </button>

                <div className="w-px h-6 bg-border mx-1" />

                <button
                  onClick={loadData}
                  className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-xl transition-all border border-transparent hover:border-blue-100"
                  title="Atualizar"
                >
                  <RefreshCw className="w-4 h-4" />
                </button>
              </div>
            </div>
          }

          // Ações de Linha
          rowActions={(item) => (
            <div className="flex items-center justify-end gap-2">
              <button
                onClick={() => handleView(item.sequenciaDoProduto)}
                className="p-2 text-blue-600 hover:bg-blue-50 rounded-xl transition-all border border-transparent hover:border-blue-100"
                title="Visualizar"
              >
                <Eye className="h-4 w-4" />
              </button>
              <button
                onClick={() => handleEdit(item.sequenciaDoProduto)}
                className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-xl transition-all border border-transparent hover:border-emerald-100"
                title="Editar"
              >
                <Edit2 className="h-4 w-4" />
              </button>
              {!item.inativo && (
                <button
                  onClick={() => handleInativarClick(item.sequenciaDoProduto, item.descricao)}
                  className="p-2 text-red-600 hover:bg-red-50 rounded-xl transition-all border border-transparent hover:border-red-100"
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
