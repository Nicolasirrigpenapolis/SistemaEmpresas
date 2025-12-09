import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Eye,
  Package,
  AlertTriangle,
  Filter,
  X,
  Barcode,
  MapPin,
  TrendingUp,
} from 'lucide-react';
import { produtoService } from '../../services/produtoService';
import type { ProdutoListDto, PagedResult, ProdutoFiltroDto } from '../../types/produto';

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
  { key: 'descricao', label: 'Descrição', placeholder: 'Buscar por descrição do produto...' },
  { key: 'codigoBarras', label: 'Código de Barras', placeholder: 'Buscar por código de barras...' },
  { key: 'localizacao', label: 'Localização', placeholder: 'Buscar por localização...' },
];

export default function ProdutosPage() {
  const navigate = useNavigate();

  // Estados
  const [data, setData] = useState<PagedResult<ProdutoListDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // Filtros
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroCampo, setFiltroCampo] = useState('codigo');
  const [filtroEstoqueCritico, setFiltroEstoqueCritico] = useState(false);
  const [filtroMateriaPrima, setFiltroMateriaPrima] = useState<boolean | undefined>(undefined);
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [showFilters, setShowFilters] = useState(false);
  const [sortDirection, setSortDirection] = useState<SortDirection>('asc');
  
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
      };

      if (filtroBusca) filtro.busca = filtroBusca;
      if (filtroEstoqueCritico) filtro.estoqueCritico = true;
      if (filtroMateriaPrima !== undefined) filtro.eMateriaPrima = filtroMateriaPrima;

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
  }, [pageNumber, pageSize, filtroEstoqueCritico, filtroMateriaPrima, filtroIncluirInativos]);

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

  const handleClearFilters = () => {
    setFiltroBusca('');
    setFiltroCampo('descricao');
    setFiltroEstoqueCritico(false);
    setFiltroMateriaPrima(undefined);
    setFiltroIncluirInativos(false);
    setSortDirection('asc');
    setPageNumber(1);
  };

  const handleClearAllFilters = () => {
    handleClearFilters();
    setShowFilters(false);
  };

  // Handler para busca do SearchBar
  const handleSearchBar = (column: string, value: string, direction: SortDirection) => {
    setFiltroCampo(column);
    setFiltroBusca(value);
    setSortDirection(direction);
    setPageNumber(1);
  };

  // Filtrar e ordenar items
  const sortedItems = useMemo(() => {
    if (!data?.items) return [];
    let items = [...data.items];
    const dir = sortDirection === 'asc' ? 1 : -1;

    // FILTRAR pelo campo selecionado e valor digitado
    if (filtroBusca.trim()) {
      const searchTerm = filtroBusca.trim().toLowerCase();
      
      items = items.filter((item) => {
        switch (filtroCampo) {
          case 'codigo':
            // Para código, busca exata ou que começa com o número digitado
            const codigoStr = String(item.sequenciaDoProduto);
            return codigoStr === searchTerm || codigoStr.startsWith(searchTerm);
          case 'descricao':
            return item.descricao.toLowerCase().includes(searchTerm);
          case 'codigoBarras':
            return (item.codigoDeBarras || '').toLowerCase().includes(searchTerm);
          case 'localizacao':
            return (item.localizacao || '').toLowerCase().includes(searchTerm);
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
          return (a.sequenciaDoProduto - b.sequenciaDoProduto) * dir;
        case 'descricao':
          return a.descricao.localeCompare(b.descricao) * dir;
        case 'codigoBarras':
          return (a.codigoDeBarras || '').localeCompare(b.codigoDeBarras || '') * dir;
        case 'localizacao':
          return (a.localizacao || '').localeCompare(b.localizacao || '') * dir;
        default:
          return (a.sequenciaDoProduto - b.sequenciaDoProduto) * dir;
      }
    });

    return items;
  }, [data, filtroCampo, filtroBusca, sortDirection]);

  const activeFilters = useMemo(() => {
    const chips: { label: string; clear?: () => void }[] = [];
    if (filtroBusca) chips.push({ label: `Busca: "${filtroBusca}"`, clear: () => setFiltroBusca('') });
    if (filtroEstoqueCritico) chips.push({ label: 'Somente estoque crítico', clear: () => setFiltroEstoqueCritico(false) });
    if (filtroMateriaPrima !== undefined) chips.push({ label: filtroMateriaPrima ? 'Matéria-prima' : 'Somente produtos finais', clear: () => setFiltroMateriaPrima(undefined) });
    if (filtroIncluirInativos) chips.push({ label: 'Incluindo inativos', clear: () => setFiltroIncluirInativos(false) });
    return chips;
  }, [filtroBusca, filtroEstoqueCritico, filtroMateriaPrima, filtroIncluirInativos]);

  // Formatar valor monetário
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  // Formatar quantidade
  const formatQuantity = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 4,
    }).format(value);
  };

  return (
    <div className="space-y-4 md:space-y-6">
      {/* Cabeçalho */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 md:gap-4">
        <div>
          <h1 className="text-xl md:text-2xl font-bold text-gray-900 flex items-center gap-2">
            <Package className="h-5 w-5 md:h-7 md:w-7 text-blue-600" />
            Produtos
          </h1>
          <p className="text-gray-500 text-xs md:text-sm mt-1 hidden sm:block">
            Cadastro e gerenciamento de produtos e matérias-primas
          </p>
        </div>
        <button
          onClick={handleNew}
          className="inline-flex items-center justify-center gap-2 px-4 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm text-sm md:text-base"
        >
          <Plus className="h-4 w-4 md:h-5 md:w-5" />
          <span className="hidden sm:inline">Novo Produto</span>
          <span className="sm:hidden">Novo</span>
        </button>
      </div>

      {/* Filtros Rápidos */}
      <div className="flex flex-wrap gap-2 overflow-x-auto">
        <button
          onClick={() => {
            setFiltroEstoqueCritico(false);
            setFiltroMateriaPrima(undefined);
            setPageNumber(1);
          }}
          className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
            !filtroEstoqueCritico && filtroMateriaPrima === undefined
              ? 'bg-blue-100 text-blue-700 border-2 border-blue-300'
              : 'bg-gray-100 text-gray-600 hover:bg-gray-200 border-2 border-transparent'
          }`}
        >
          Todos
          {data && !filtroEstoqueCritico && filtroMateriaPrima === undefined && (
            <span className="ml-2 px-2 py-0.5 bg-blue-200 text-blue-800 rounded-full text-xs">
              {data.totalItems}
            </span>
          )}
        </button>

        <button
          onClick={() => {
            setFiltroMateriaPrima(false);
            setFiltroEstoqueCritico(false);
            setPageNumber(1);
          }}
          className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
            filtroMateriaPrima === false
              ? 'bg-green-100 text-green-700 border-2 border-green-300'
              : 'bg-gray-100 text-gray-600 hover:bg-gray-200 border-2 border-transparent'
          }`}
        >
          <span className="flex items-center gap-1">
            <Package className="h-4 w-4" />
            Produtos Acabados
          </span>
        </button>

        <button
          onClick={() => {
            setFiltroMateriaPrima(true);
            setFiltroEstoqueCritico(false);
            setPageNumber(1);
          }}
          className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
            filtroMateriaPrima === true
              ? 'bg-purple-100 text-purple-700 border-2 border-purple-300'
              : 'bg-gray-100 text-gray-600 hover:bg-gray-200 border-2 border-transparent'
          }`}
        >
          <span className="flex items-center gap-1">
            Matéria Prima
          </span>
        </button>

        <button
          onClick={() => {
            setFiltroEstoqueCritico(true);
            setFiltroMateriaPrima(undefined);
            setPageNumber(1);
          }}
          className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
            filtroEstoqueCritico
              ? 'bg-red-100 text-red-700 border-2 border-red-300'
              : 'bg-gray-100 text-gray-600 hover:bg-gray-200 border-2 border-transparent'
          }`}
        >
          <span className="flex items-center gap-1">
            <AlertTriangle className="h-4 w-4" />
            Estoque Crítico
          </span>
        </button>
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
                onChange={(e) => {
                  setFiltroIncluirInativos(e.target.checked);
                  setPageNumber(1);
                }}
                className="w-4 h-4 text-blue-600 rounded border-gray-300 focus:ring-blue-500"
              />
              <span className="text-sm text-[var(--text)] font-medium">Incluir inativos</span>
            </label>

            {(filtroBusca || filtroIncluirInativos || filtroEstoqueCritico || filtroMateriaPrima !== undefined) && (
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
          {activeFilters.map((chip, idx) => (
            <span
              key={idx}
              className="inline-flex items-center gap-2 px-3 py-1.5 bg-[var(--surface)] border border-[var(--border)] rounded-full text-sm text-[var(--text)] shadow-sm"
            >
              {chip.label}
              {chip.clear && (
                <button
                  type="button"
                  onClick={chip.clear}
                  className="text-[var(--text-muted)] hover:text-red-500"
                >
                  <X className="w-4 h-4" />
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

      {/* Contagem de Resultados */}
      <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-3">
        <p className="text-sm text-[var(--text-muted)]">
          <span className="font-semibold text-[var(--text)]">{data?.totalItems || 0}</span>
          {' '}produto(s) encontrado(s)
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
              className="px-2 py-1 border border-[var(--border)] rounded-md text-sm bg-[var(--surface)] text-[var(--text)] focus:ring-2 focus:ring-blue-500"
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

      {/* Alerta de Erro */}
      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

      {/* Tabela */}
      <div className="bg-[var(--surface)] rounded-xl border border-[var(--border)] shadow-[var(--shadow-soft)] overflow-hidden">
        {loading ? (
          <EstadoCarregando mensagem="Carregando produtos..." />
        ) : !data || sortedItems.length === 0 ? (
          <EstadoVazio
            icone={Package}
            titulo="Nenhum produto encontrado"
            descricao="Tente ajustar os filtros ou cadastre um novo produto."
            acao={{
              texto: 'Novo Produto',
              onClick: handleNew,
            }}
          />
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-gray-50 border-b border-gray-200">
                <tr>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Código
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Descrição
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Grupo
                  </th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Un
                  </th>
                  <th className="px-4 py-3 text-right text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Estoque
                  </th>
                  <th className="px-4 py-3 text-right text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Custo
                  </th>
                  <th className="px-4 py-3 text-right text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Venda
                  </th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Status
                  </th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Ações
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-[var(--border)]">
                {sortedItems.map((produto) => (
                  <tr
                    key={produto.sequenciaDoProduto}
                    className={`hover:bg-blue-50/40 transition-colors ${
                      produto.inativo ? 'bg-[var(--surface-muted)] opacity-70' : ''
                    }`}
                  >
                    <td className="px-4 py-3 whitespace-nowrap">
                      <span className="text-sm font-medium text-gray-900">
                        {produto.sequenciaDoProduto}
                      </span>
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex flex-col">
                        <span className="text-sm font-medium text-gray-900 line-clamp-1">
                          {produto.descricao}
                        </span>
                        {produto.codigoDeBarras && (
                          <span className="text-xs text-gray-500 flex items-center gap-1 mt-0.5">
                            <Barcode className="h-3 w-3" />
                            {produto.codigoDeBarras}
                          </span>
                        )}
                        {produto.localizacao && (
                          <span className="text-xs text-gray-400 flex items-center gap-1">
                            <MapPin className="h-3 w-3" />
                            {produto.localizacao}
                          </span>
                        )}
                      </div>
                    </td>
                    <td className="px-4 py-3 whitespace-nowrap">
                      <div className="flex flex-col">
                        <span className="text-sm text-gray-700">{produto.grupoProduto}</span>
                        {produto.subGrupoProduto && (
                          <span className="text-xs text-gray-400">{produto.subGrupoProduto}</span>
                        )}
                      </div>
                    </td>
                    <td className="px-4 py-3 whitespace-nowrap text-center">
                      <span className="text-sm text-gray-600">{produto.unidade}</span>
                    </td>
                    <td className="px-4 py-3 whitespace-nowrap text-right">
                      <div className="flex flex-col items-end">
                        <span
                          className={`text-sm font-medium ${
                            produto.estoqueCritico
                              ? 'text-red-600'
                              : produto.quantidadeNoEstoque > produto.quantidadeMinima * 2
                              ? 'text-green-600'
                              : 'text-gray-900'
                          }`}
                        >
                          {formatQuantity(produto.quantidadeNoEstoque)}
                        </span>
                        {produto.quantidadeMinima > 0 && (
                          <span className="text-xs text-gray-400">
                            Mín: {formatQuantity(produto.quantidadeMinima)}
                          </span>
                        )}
                      </div>
                      {produto.estoqueCritico && (
                        <div className="flex items-center justify-end gap-1 mt-1">
                          <AlertTriangle className="h-3 w-3 text-red-500" />
                          <span className="text-xs text-red-500 font-medium">Crítico</span>
                        </div>
                      )}
                    </td>
                    <td className="px-4 py-3 whitespace-nowrap text-right">
                      <span className="text-sm text-gray-600">
                        {formatCurrency(produto.valorDeCusto)}
                      </span>
                    </td>
                    <td className="px-4 py-3 whitespace-nowrap text-right">
                      <div className="flex flex-col items-end">
                        <span className="text-sm font-medium text-gray-900">
                          {formatCurrency(produto.valorTotal)}
                        </span>
                        {produto.margemDeLucro > 0 && (
                          <span className="text-xs text-green-600 flex items-center gap-0.5">
                            <TrendingUp className="h-3 w-3" />
                            {produto.margemDeLucro.toFixed(1)}%
                          </span>
                        )}
                      </div>
                    </td>
                    <td className="px-4 py-3 whitespace-nowrap text-center">
                      <div className="flex flex-col items-center gap-1">
                        {produto.inativo ? (
                          <span className="px-2 py-1 text-xs font-medium bg-gray-100 text-gray-600 rounded-full">
                            Inativo
                          </span>
                        ) : (
                          <span className="px-2 py-1 text-xs font-medium bg-green-100 text-green-700 rounded-full">
                            Ativo
                          </span>
                        )}
                        {produto.eMateriaPrima && (
                          <span className="px-2 py-0.5 text-xs bg-purple-100 text-purple-700 rounded">
                            MP
                          </span>
                        )}
                      </div>
                    </td>
                    <td className="px-4 py-3 whitespace-nowrap text-center">
                      <div className="flex items-center justify-center gap-1">
                        <button
                          onClick={() => handleView(produto.sequenciaDoProduto)}
                          className="p-1.5 text-gray-400 hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                          title="Visualizar"
                        >
                          <Eye className="h-4 w-4" />
                        </button>
                        <button
                          onClick={() => handleEdit(produto.sequenciaDoProduto)}
                          className="p-1.5 text-gray-400 hover:text-green-600 hover:bg-green-50 rounded-lg transition-colors"
                          title="Editar"
                        >
                          <Edit2 className="h-4 w-4" />
                        </button>
                        {!produto.inativo && (
                          <button
                            onClick={() => handleInativarClick(produto.sequenciaDoProduto, produto.descricao)}
                            className="p-1.5 text-gray-400 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                            title="Inativar"
                          >
                            <X className="h-4 w-4" />
                          </button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* Paginação */}
      {data && data.totalPages > 1 && (
        <Paginacao
          paginaAtual={pageNumber}
          totalPaginas={data.totalPages}
          totalItens={data.totalItems}
          itensPorPagina={pageSize}
          onMudarPagina={setPageNumber}
        />
      )}

      {/* Modal de Confirmação de Inativação */}
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
