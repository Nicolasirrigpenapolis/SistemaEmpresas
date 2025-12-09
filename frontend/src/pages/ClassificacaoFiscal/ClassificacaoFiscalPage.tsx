import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Trash2,
  FileText,
  CheckCircle,
  XCircle,
  Filter,
  AlertCircle,
} from 'lucide-react';
import { classificacaoFiscalService } from '../../services/classificacaoFiscalService';
import type { 
  ClassificacaoFiscal, 
  PagedResult,
} from '../../types/classificacaoFiscal';

// Componentes reutilizáveis
import { 
  ModalConfirmacao, 
  Paginacao, 
  EstadoVazio, 
  EstadoCarregando,
  AlertaErro,
  SearchBar,
} from '../../components/common';
import type { SortDirection } from '../../components/common';

// Colunas de busca disponíveis
const SEARCH_COLUMNS = [
  { key: 'codigo', label: 'Código', placeholder: 'Buscar por código (ex: 123)...' },
  { key: 'ncm', label: 'NCM', placeholder: 'Buscar por NCM (ex: 01012100)...' },
  { key: 'descricao', label: 'Descrição', placeholder: 'Buscar por descrição...' },
];

// ============================================================================
// COMPONENTE PRINCIPAL
// ============================================================================
export default function ClassificacaoFiscalPage() {
  const navigate = useNavigate();
  
  // Estados
  const [data, setData] = useState<PagedResult<ClassificacaoFiscal> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // Filtros - SearchBar
  const [filtroCampo, setFiltroCampo] = useState<string>('codigo');
  const [filtroBusca, setFiltroBusca] = useState('');
  const [sortDirection, setSortDirection] = useState<SortDirection>('asc');
  
  // Filtros avançados
  const [filtroNcm, setFiltroNcm] = useState('');
  const [filtroDescricao, setFiltroDescricao] = useState('');
  const [filtroSomenteNFe, setFiltroSomenteNFe] = useState(false);
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [filtroTributacao, setFiltroTributacao] = useState<'todos' | 'vinculados' | 'pendentes'>('todos');
  const [showFilters, setShowFilters] = useState(false);
  
  // Paginação
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);

  // Modal de Exclusão
  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; ncm: string; deleting: boolean }>({
    open: false,
    id: 0,
    ncm: '',
    deleting: false,
  });

  // ============================================================================
  // FUNÇÕES
  // ============================================================================
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const result = await classificacaoFiscalService.listar({
        pageNumber,
        pageSize,
        ncm: filtroNcm || undefined,
        descricao: filtroDescricao || undefined,
        somenteNFe: filtroSomenteNFe || undefined,
        incluirInativos: filtroIncluirInativos || undefined,
        tributacao: filtroTributacao !== 'todos' ? filtroTributacao : undefined,
      });
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar classificações fiscais:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

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

  const handleClearAllFilters = () => {
    handleClearFilters();
    setShowFilters(false);
  };

  // Handler para SearchBar
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
            const codigoStr = String(item.sequenciaDaClassificacao);
            return codigoStr === searchTerm || codigoStr.startsWith(searchTerm);
          case 'ncm':
            const ncmStr = String(item.ncm).padStart(8, '0');
            return ncmStr.includes(searchTerm);
          case 'descricao':
            return item.descricaoDoNcm.toLowerCase().includes(searchTerm);
          default:
            return true;
        }
      });
    }

    // ORDENAR pelo campo selecionado
    items.sort((a, b) => {
      switch (filtroCampo) {
        case 'codigo':
          return (a.sequenciaDaClassificacao - b.sequenciaDaClassificacao) * dir;
        case 'ncm':
          return String(a.ncm).localeCompare(String(b.ncm)) * dir;
        case 'descricao':
          return a.descricaoDoNcm.localeCompare(b.descricaoDoNcm) * dir;
        default:
          return (a.sequenciaDaClassificacao - b.sequenciaDaClassificacao) * dir;
      }
    });

    return items;
  }, [data, filtroCampo, filtroBusca, sortDirection]);

  const activeFilters = useMemo(() => {
    const chips: { label: string; clear?: () => void }[] = [];
    if (filtroBusca) chips.push({ label: `Busca: "${filtroBusca}"`, clear: () => setFiltroBusca('') });
    if (filtroNcm) chips.push({ label: `NCM: ${filtroNcm}`, clear: () => setFiltroNcm('') });
    if (filtroDescricao) chips.push({ label: `Descrição: "${filtroDescricao}"`, clear: () => setFiltroDescricao('') });
    if (filtroSomenteNFe) chips.push({ label: 'Somente NF-e', clear: () => setFiltroSomenteNFe(false) });
    if (filtroIncluirInativos) chips.push({ label: 'Incluindo inativos', clear: () => setFiltroIncluirInativos(false) });
    if (filtroTributacao !== 'todos') {
      const label = filtroTributacao === 'vinculados' ? 'Tributação vinculada' : 'Tributação pendente';
      chips.push({ label });
    }
    return chips;
  }, [filtroBusca, filtroNcm, filtroDescricao, filtroSomenteNFe, filtroIncluirInativos, filtroTributacao]);

  // ============================================================================
  // EFFECTS
  // ============================================================================
  useEffect(() => {
    loadData();
  }, [pageNumber, pageSize]);

  // ============================================================================
  // RENDER
  // ============================================================================
  return (
    <div className="space-y-6">
      {/* Header Moderno */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-[var(--text)]">Classificação Fiscal</h1>
          <p className="text-sm text-[var(--text-muted)]">Gerencie os códigos NCM e configurações fiscais</p>
        </div>
        
        <div className="flex items-center gap-3">
          <button
            onClick={() => navigate('/classificacao-fiscal/novo')}
            className="inline-flex items-center gap-2 px-5 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-xl hover:from-blue-700 hover:to-blue-800 transition-all text-sm font-medium shadow-lg shadow-blue-500/30"
          >
            <Plus className="h-4 w-4" />
            <span>Nova Classificação</span>
          </button>
        </div>
      </div>

      {/* SearchBar Unificado */}
      <SearchBar
        columns={SEARCH_COLUMNS}
        onSearch={handleSearchBar}
        onClear={handleClearFilters}
        initialColumn={filtroCampo}
        initialValue={filtroBusca}
        initialSortDirection={sortDirection}
      />

      {/* Filtros Avançados */}
      <div className="bg-[var(--surface)] backdrop-blur-sm rounded-2xl border border-[var(--border)] shadow-sm">
        <div className="p-4">
          <button
            onClick={() => setShowFilters(!showFilters)}
            className={`inline-flex items-center gap-2 px-4 py-2.5 text-sm font-medium rounded-xl border transition-all ${
              showFilters 
                ? 'bg-blue-600 text-white border-blue-600' 
                : 'bg-[var(--surface)] text-[var(--text-muted)] border-[var(--border)] hover:bg-[var(--surface-muted)] shadow-sm'
            }`}
          >
            <Filter className="h-4 w-4" />
            Filtros Avançados
            {(filtroNcm || filtroSomenteNFe || filtroIncluirInativos || filtroTributacao !== 'todos') && (
              <span className="w-2 h-2 bg-blue-400 rounded-full"></span>
            )}
          </button>
        </div>
          
        {/* Filtros avançados */}
        {showFilters && (
          <div className="px-4 pb-4 pt-1 border-t border-[var(--border)]">
            <div className="grid grid-cols-1 sm:grid-cols-5 gap-4 mt-4">
              <div className="relative">
                <input
                  type="text"
                  value={filtroNcm}
                    onChange={(e) => setFiltroNcm(e.target.value)}
                    placeholder="00000000"
                    className="w-full px-3 py-3 text-sm bg-[var(--surface)] border border-[var(--border)] rounded-xl text-[var(--text)] placeholder:text-[var(--text-muted)] focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                  />
                  <label className="absolute -top-2.5 left-3 text-xs text-[var(--text-muted)] bg-[var(--surface)] px-1 rounded">NCM</label>
                </div>
                
                <div className="relative">
                  <select
                    value={filtroTributacao}
                    onChange={(e) => setFiltroTributacao(e.target.value as 'todos' | 'vinculados' | 'pendentes')}
                    className="w-full px-3 py-3 text-sm bg-[var(--surface)] border border-[var(--border)] rounded-xl text-[var(--text)] focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all appearance-none cursor-pointer"
                  >
                    <option value="todos">Todos</option>
                    <option value="vinculados">✓ Vinculados</option>
                    <option value="pendentes">⚠ Pendentes</option>
                  </select>
                  <label className="absolute -top-2.5 left-3 text-xs text-[var(--text-muted)] bg-[var(--surface)] px-1 rounded">Tributação</label>
                </div>
                
                <div className="sm:col-span-2 flex flex-wrap items-center gap-3">
                  <label className="flex items-center gap-2.5 px-4 py-2.5 bg-[var(--surface)] border border-[var(--border)] rounded-xl cursor-pointer hover:bg-[var(--surface-muted)] transition-colors">
                    <input
                      type="checkbox"
                      checked={filtroSomenteNFe}
                      onChange={(e) => setFiltroSomenteNFe(e.target.checked)}
                      className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                    />
                    <span className="text-sm text-[var(--text)] font-medium">Apenas NFe</span>
                  </label>
                  
                  <label className="flex items-center gap-2.5 px-4 py-2.5 bg-[var(--surface)] border border-[var(--border)] rounded-xl cursor-pointer hover:bg-[var(--surface-muted)] transition-colors">
                    <input
                      type="checkbox"
                      checked={filtroIncluirInativos}
                      onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                      className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                    />
                    <span className="text-sm text-[var(--text)] font-medium">Incluir inativos</span>
                  </label>
                </div>
                
                <div className="flex items-center">
                  <button
                    onClick={handleClearFilters}
                    className="w-full px-4 py-2.5 text-sm text-[var(--text-muted)] hover:text-red-600 hover:bg-red-50 rounded-xl transition-colors font-medium"
                  >
                    Limpar filtros
                  </button>
                </div>
              </div>
            </div>
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
                    <XCircle className="w-4 h-4" />
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

        {/* Erro */}
        {error && (
          <AlertaErro 
            mensagem={error} 
            fechavel 
            onFechar={() => setError(null)}
            className="mb-6"
          />
        )}

        {/* Resumo */}
        <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-3">
          <p className="text-sm text-[var(--text-muted)]">
            <span className="font-semibold text-[var(--text)]">{data?.total || 0}</span> classificação(ões) encontradas
          </p>
        </div>

        {/* Tabela Modernizada */}
        <div className="bg-[var(--surface)] rounded-2xl border border-[var(--border)] shadow-[var(--shadow-soft)] overflow-hidden">
          {loading ? (
            <EstadoCarregando mensagem="Carregando classificações fiscais..." />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead className="bg-[var(--surface-muted)]">
                    <tr>
                      <th className="px-3 sm:px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">#</th>
                      <th className="px-3 sm:px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">NCM</th>
                      <th className="px-3 sm:px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider hidden md:table-cell">Descrição</th>
                      <th className="px-3 sm:px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider hidden sm:table-cell">Tributação</th>
                      <th className="px-3 sm:px-4 py-4 text-left text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider hidden lg:table-cell">CEST</th>
                      <th className="px-3 sm:px-4 py-4 text-center text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider hidden lg:table-cell">IPI %</th>
                      <th className="px-3 sm:px-4 py-4 text-center text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider hidden sm:table-cell">Status</th>
                      <th className="px-3 sm:px-4 py-4 text-right text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">Ações</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-[var(--border)]">
                    {sortedItems.map((item) => (
                      <tr 
                        key={item.sequenciaDaClassificacao} 
                        className={`transition-colors ${
                          item.inativo 
                            ? 'bg-[var(--surface-muted)] opacity-70' 
                            : 'hover:bg-blue-50/40'
                        }`}
                      >
                        <td className="px-3 sm:px-4 py-4">
                          <span className="text-sm font-mono text-[var(--text-muted)] bg-[var(--surface-muted)] px-2 py-1 rounded">
                            {item.sequenciaDaClassificacao}
                          </span>
                        </td>
                        <td className="px-3 sm:px-4 py-4">
                          <span className="inline-flex items-center px-2 sm:px-2.5 py-1 bg-indigo-50 text-indigo-700 text-xs sm:text-sm font-mono font-medium rounded-lg">
                            {item.ncm.toString().padStart(8, '0')}
                          </span>
                        </td>
                        <td className="px-3 sm:px-4 py-4 max-w-md hidden md:table-cell">
                          <p className="text-sm font-medium text-[var(--text)] truncate" title={item.descricaoDoNcm}>
                            {item.descricaoDoNcm}
                          </p>
                        </td>
                        <td className="px-3 sm:px-4 py-4 hidden sm:table-cell">
                          {item.classTribId ? (
                            <span className="inline-flex items-center gap-1.5 px-2.5 py-1 bg-emerald-50 text-emerald-700 text-xs font-semibold rounded-full">
                              <CheckCircle className="h-3.5 w-3.5" />
                              Vinculado
                            </span>
                          ) : (
                            <span className="inline-flex items-center gap-1.5 px-2.5 py-1 bg-amber-50 text-amber-600 text-xs font-semibold rounded-full">
                              <AlertCircle className="h-3.5 w-3.5" />
                              Pendente
                            </span>
                          )}
                        </td>
                        <td className="px-3 sm:px-4 py-4 hidden lg:table-cell">
                          <span className="text-sm text-[var(--text-muted)] font-mono">
                            {item.cest || <span className="text-[var(--text-muted)]">–</span>}
                          </span>
                        </td>
                        <td className="px-3 sm:px-4 py-4 text-center hidden lg:table-cell">
                          <span className="text-sm font-medium text-[var(--text)]">
                            {item.porcentagemDoIpi.toFixed(2)}%
                          </span>
                        </td>
                        <td className="px-3 sm:px-4 py-4 text-center hidden sm:table-cell">
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
                        <td className="px-3 sm:px-4 py-4">
                          <div className="flex items-center justify-end gap-1">
                            <button
                              onClick={() => navigate(`/classificacao-fiscal/${item.sequenciaDaClassificacao}`)}
                              className="p-2 text-gray-400 hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-all"
                              title="Editar"
                            >
                              <Edit2 className="h-4 w-4" />
                            </button>
                            <button
                              onClick={() => handleDeleteClick(item.sequenciaDaClassificacao, item.ncm.toString().padStart(8, '0'))}
                              className="p-2 text-gray-400 hover:text-red-600 hover:bg-red-50 rounded-lg transition-all"
                              title="Inativar"
                            >
                              <Trash2 className="h-4 w-4" />
                            </button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>

              {/* Sem resultados */}
              {sortedItems.length === 0 && (
                <EstadoVazio 
                  icone={FileText}
                  titulo="Nenhuma classificação encontrada"
                  descricao="Tente ajustar os filtros ou cadastrar uma nova"
                  acao={{ texto: 'Limpar filtros', onClick: handleClearFilters }}
                />
              )}

              {/* Paginação */}
              {data && data.totalPages > 0 && (
                <Paginacao
                  paginaAtual={data.pageNumber}
                  totalPaginas={data.totalPages}
                  totalItens={data.total}
                  itensPorPagina={pageSize}
                  onMudarPagina={setPageNumber}
                  onMudarItensPorPagina={(itens) => {
                    setPageSize(itens);
                    setPageNumber(1);
                  }}
                  carregando={loading}
                />
              )}
            </>
          )}
        </div>

      {/* Modal de Exclusão */}
      <ModalConfirmacao
        aberto={deleteModal.open}
        titulo="Confirmar Inativação"
        mensagem="Tem certeza que deseja inativar esta classificação fiscal?"
        nomeItem={`NCM: ${deleteModal.ncm}`}
        textoBotaoConfirmar="Inativar"
        processando={deleteModal.deleting}
        onConfirmar={handleDeleteConfirm}
        onCancelar={() => setDeleteModal({ open: false, id: 0, ncm: '', deleting: false })}
      />
    </div>
  );
}
