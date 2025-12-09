import { useEffect, useState } from 'react';
import {
  Search,
  Filter,
  Download,
  AlertCircle,
  CheckCircle,
  BarChart3,
  Loader2,
  Lock,
  Eye,
  X,
  ExternalLink,
  FileText,
  Percent,
  Tag,
  BookOpen,
} from 'lucide-react';
import { classTribService } from '../../services/classTribService';
import type { ClassTribDto } from '../../services/classTribService';
import { usePermissaoTela } from '../../hooks/usePermissaoTela';

interface FilterState {
  csts: string[];
  tipoAliquota: string;
  minReducaoIBS: number | null;
  maxReducaoIBS: number | null;
  minReducaoCBS: number | null;
  maxReducaoCBS: number | null;
  validoNFe: boolean | null;
  tributacaoRegular: boolean | null;
  creditoPresumido: boolean | null;
  descricao: string;
  ordenarPor: string;
}

interface Stats {
  totalClassificacoes: number;
  totalValidoNFe: number;
  mediaReducaoIBS: number;
  mediaReducaoCBS: number;
  totalComReducaoIBS: number;
  totalComReducaoCBS: number;
}

export default function ClassTribManagementPage() {
  // Permissões da tela
  const { podeConsultar, carregando: carregandoPermissoes } = usePermissaoTela('ClassTrib');

  // Estados
  const [classTribs, setClassTribs] = useState<ClassTribDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(50);
  const [total, setTotal] = useState(0);

  const [filter, setFilter] = useState<FilterState>({
    csts: [],
    tipoAliquota: '',
    minReducaoIBS: null,
    maxReducaoIBS: null,
    minReducaoCBS: null,
    maxReducaoCBS: null,
    validoNFe: null,
    tributacaoRegular: null,
    creditoPresumido: null,
    descricao: '',
    ordenarPor: 'codigo',
  });

  const [stats, setStats] = useState<Stats | null>(null);
  const [cstOptions, setCstOptions] = useState<Array<{ codigo: string; descricao: string; total: number }>>([]);
  const [tipoAliquotaOptions, setTipoAliquotaOptions] = useState<string[]>([]);
  const [showFilters, setShowFilters] = useState(false);
  
  // Estado para modal de visualização
  const [classTribSelecionado, setClassTribSelecionado] = useState<ClassTribDto | null>(null);
  const [showModal, setShowModal] = useState(false);

  // ============================================================================
  // FUNÇÕES
  // ============================================================================

  const abrirDetalhes = (classTrib: ClassTribDto) => {
    setClassTribSelecionado(classTrib);
    setShowModal(true);
  };

  const fecharModal = () => {
    setShowModal(false);
    setClassTribSelecionado(null);
  };

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const cstParams = filter.csts.length > 0 ? filter.csts.join(',') : undefined;

      const response = await classTribService.listar(page, pageSize, cstParams);
      setClassTribs(response.items);
      setTotal(response.totalItems);
    } catch (err: any) {
      setError(err.response?.data?.error || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

  const loadFiltersAndStats = async () => {
    try {
      const [csts, tipos, stats] = await Promise.all([
        classTribService.getCsts(),
        classTribService.getTiposAliquota(),
        classTribService.getEstatisticas(),
      ]);

      setCstOptions(csts);
      setTipoAliquotaOptions(tipos);
      setStats(stats);
    } catch (err) {
      console.error('Erro ao carregar filtros e estatísticas:', err);
    }
  };

  const handleFilterChange = (newFilter: Partial<FilterState>) => {
    setFilter(prev => ({ ...prev, ...newFilter }));
    setPage(1);
  };

  const resetFilters = () => {
    setFilter({
      csts: [],
      tipoAliquota: '',
      minReducaoIBS: null,
      maxReducaoIBS: null,
      minReducaoCBS: null,
      maxReducaoCBS: null,
      validoNFe: null,
      tributacaoRegular: null,
      creditoPresumido: null,
      descricao: '',
      ordenarPor: 'codigo',
    });
    setPage(1);
  };

  const handleExport = () => {
    const csv = [
      ['Código', 'CST', 'Descrição', 'Redução IBS', 'Redução CBS', 'Válido NFe'],
      ...classTribs.map(ct => [
        ct.codigoClassTrib,
        ct.codigoSituacaoTributaria,
        ct.descricaoClassTrib,
        ct.percentualReducaoIBS,
        ct.percentualReducaoCBS,
        ct.validoParaNFe ? 'Sim' : 'Não',
      ]),
    ]
      .map(row => row.map(cell => `"${cell}"`).join(','))
      .join('\n');

    const blob = new Blob([csv], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `classtrib-${new Date().toISOString().split('T')[0]}.csv`;
    a.click();
  };

  // ============================================================================
  // EFFECTS
  // ============================================================================

  useEffect(() => {
    loadFiltersAndStats();
  }, []);

  useEffect(() => {
    loadData();
  }, [page, pageSize, filter]);

  // ============================================================================
  // RENDER
  // ============================================================================

  // Carregando permissões
  if (carregandoPermissoes) {
    return (
      <div className="min-h-screen bg-gray-50/50 flex items-center justify-center">
        <div className="text-center">
          <Loader2 className="h-8 w-8 animate-spin text-blue-600 mx-auto mb-4" />
          <p className="text-[var(--text-muted)]">Verificando permissões...</p>
        </div>
      </div>
    );
  }

  // Sem permissão de consulta
  if (!podeConsultar) {
    return (
      <div className="min-h-screen bg-gray-50/50 flex items-center justify-center">
        <div className="text-center bg-[var(--surface)] p-8 rounded-xl shadow-lg max-w-md">
          <Lock className="h-12 w-12 text-red-500 mx-auto mb-4" />
          <h2 className="text-xl font-bold text-[var(--text)] mb-2">Acesso Negado</h2>
          <p className="text-[var(--text-muted)]">Você não tem permissão para acessar esta tela.</p>
          <p className="text-sm text-gray-400 mt-2">Entre em contato com o administrador para solicitar acesso.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50/50">
      {/* Header */}
      <div className="bg-[var(--surface)] border-b border-[var(--border)] sticky top-0 z-10">
        <div className="px-4 sm:px-6 py-4">
          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-4">
            <div>
              <h1 className="text-xl sm:text-2xl font-bold text-[var(--text)]">Gestão de ClassTrib</h1>
              <p className="text-xs sm:text-sm text-[var(--text-muted)]">Gerenciar classificações tributárias da Reforma Tributária IBS/CBS</p>
            </div>

            <button
              disabled={true}
              className="inline-flex items-center justify-center gap-2 px-5 py-2.5 bg-gray-400 text-white rounded-lg cursor-not-allowed opacity-60"
              title="Dados já sincronizados"
            >
              <CheckCircle className="h-4 w-4" />
              <span className="hidden sm:inline">Sincronizado</span>
            </button>
          </div>

          {/* Mensagens */}
          {error && (
            <div className="mb-4 p-3 bg-red-50 border border-red-100 rounded-lg flex items-center gap-3">
              <AlertCircle className="h-5 w-5 text-red-500 flex-shrink-0" />
              <p className="text-sm text-red-700">{error}</p>
            </div>
          )}
        </div>
      </div>

      <div className="px-4 sm:px-6 py-4 sm:py-6">
        {/* Estatísticas */}
        {stats && (
          <div className="grid grid-cols-2 lg:grid-cols-4 gap-3 sm:gap-4 mb-4 sm:mb-6">
            <div className="bg-[var(--surface)] rounded-lg border border-[var(--border)] p-3 sm:p-4">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-xs sm:text-sm text-[var(--text-muted)]">Total de Classificações</p>
                  <p className="text-xl sm:text-2xl font-bold text-[var(--text)]">{stats.totalClassificacoes}</p>
                </div>
                <BarChart3 className="h-6 w-6 sm:h-8 sm:w-8 text-blue-500 opacity-20" />
              </div>
            </div>

            <div className="bg-[var(--surface)] rounded-lg border border-[var(--border)] p-3 sm:p-4">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-xs sm:text-sm text-[var(--text-muted)]">Válidas para NFe</p>
                  <p className="text-xl sm:text-2xl font-bold text-[var(--text)]">{stats.totalValidoNFe}</p>
                </div>
                <CheckCircle className="h-6 w-6 sm:h-8 sm:w-8 text-emerald-500 opacity-20" />
              </div>
            </div>

            <div className="bg-[var(--surface)] rounded-lg border border-[var(--border)] p-3 sm:p-4">
              <div>
                <p className="text-xs sm:text-sm text-[var(--text-muted)]">Média Redução IBS</p>
                <p className="text-xl sm:text-2xl font-bold text-[var(--text)]">{stats.mediaReducaoIBS.toFixed(2)}%</p>
                <p className="text-xs text-gray-400 mt-1 hidden sm:block">{stats.totalComReducaoIBS} com redução</p>
              </div>
            </div>

            <div className="bg-[var(--surface)] rounded-lg border border-[var(--border)] p-3 sm:p-4">
              <div>
                <p className="text-xs sm:text-sm text-[var(--text-muted)]">Média Redução CBS</p>
                <p className="text-xl sm:text-2xl font-bold text-[var(--text)]">{stats.mediaReducaoCBS.toFixed(2)}%</p>
                <p className="text-xs text-gray-400 mt-1 hidden sm:block">{stats.totalComReducaoCBS} com redução</p>
              </div>
            </div>
          </div>
        )}

        {/* Controles de Filtro */}
        <div className="bg-[var(--surface)] rounded-lg border border-[var(--border)] mb-4 sm:mb-6">
          <div className="px-4 sm:px-6 py-4 border-b border-[var(--border)] flex items-center justify-between">
            <h2 className="text-base sm:text-lg font-semibold text-[var(--text)] flex items-center gap-2">
              <Filter className="h-4 w-4 sm:h-5 sm:w-5" />
              Filtros
            </h2>
            <button
              onClick={() => setShowFilters(!showFilters)}
              className="text-sm text-[var(--text-muted)] hover:text-[var(--text)] transition-colors"
            >
              {showFilters ? 'Ocultar' : 'Mostrar'}
            </button>
          </div>

          {showFilters && (
            <div className="p-4 sm:p-6 space-y-4">
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
                {/* Busca por Descrição */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    <Search className="h-4 w-4 inline mr-1" />
                    Descrição / Código
                  </label>
                  <input
                    type="text"
                    value={filter.descricao}
                    onChange={(e) => handleFilterChange({ descricao: e.target.value })}
                    placeholder="Digite para buscar..."
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  />
                </div>

                {/* CSTs */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Código de Situação Tributária
                  </label>
                  <select
                    multiple
                    value={filter.csts}
                    onChange={(e) =>
                      handleFilterChange({
                        csts: Array.from(e.target.selectedOptions, (option) => option.value),
                      })
                    }
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                    size={4}
                  >
                    {cstOptions.map((cst) => (
                      <option key={cst.codigo} value={cst.codigo}>
                        {cst.codigo} - {cst.descricao} ({cst.total})
                      </option>
                    ))}
                  </select>
                </div>

                {/* Tipo de Alíquota */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Tipo de Alíquota</label>
                  <select
                    value={filter.tipoAliquota}
                    onChange={(e) => handleFilterChange({ tipoAliquota: e.target.value })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  >
                    <option value="">Todos</option>
                    {tipoAliquotaOptions.map((tipo) => (
                      <option key={tipo} value={tipo}>
                        {tipo}
                      </option>
                    ))}
                  </select>
                </div>

                {/* Redução IBS Min */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Redução IBS - Mínima (%)</label>
                  <input
                    type="number"
                    min="0"
                    max="100"
                    value={filter.minReducaoIBS ?? ''}
                    onChange={(e) => handleFilterChange({ minReducaoIBS: e.target.value ? Number(e.target.value) : null })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  />
                </div>

                {/* Redução IBS Max */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Redução IBS - Máxima (%)</label>
                  <input
                    type="number"
                    min="0"
                    max="100"
                    value={filter.maxReducaoIBS ?? ''}
                    onChange={(e) => handleFilterChange({ maxReducaoIBS: e.target.value ? Number(e.target.value) : null })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  />
                </div>

                {/* Redução CBS Min */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Redução CBS - Mínima (%)</label>
                  <input
                    type="number"
                    min="0"
                    max="100"
                    value={filter.minReducaoCBS ?? ''}
                    onChange={(e) => handleFilterChange({ minReducaoCBS: e.target.value ? Number(e.target.value) : null })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  />
                </div>

                {/* Redução CBS Max */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Redução CBS - Máxima (%)</label>
                  <input
                    type="number"
                    min="0"
                    max="100"
                    value={filter.maxReducaoCBS ?? ''}
                    onChange={(e) => handleFilterChange({ maxReducaoCBS: e.target.value ? Number(e.target.value) : null })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  />
                </div>

                {/* Válido NFe */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Válido para NFe</label>
                  <select
                    value={filter.validoNFe === null ? '' : filter.validoNFe ? 'true' : 'false'}
                    onChange={(e) =>
                      handleFilterChange({
                        validoNFe: e.target.value === '' ? null : e.target.value === 'true',
                      })
                    }
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  >
                    <option value="">Todos</option>
                    <option value="true">Sim</option>
                    <option value="false">Não</option>
                  </select>
                </div>

                {/* Ordenação */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Ordenar por</label>
                  <select
                    value={filter.ordenarPor}
                    onChange={(e) => handleFilterChange({ ordenarPor: e.target.value })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 focus:border-gray-900"
                  >
                    <option value="codigo">Código</option>
                    <option value="descricao">Descrição</option>
                    <option value="reducaoibs">Redução IBS</option>
                    <option value="reducaocbs">Redução CBS</option>
                  </select>
                </div>
              </div>

              <div className="flex flex-col sm:flex-row gap-3 pt-2">
                <button
                  onClick={resetFilters}
                  className="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors text-sm font-medium"
                >
                  Limpar Filtros
                </button>
                <button
                  onClick={handleExport}
                  className="px-4 py-2 text-gray-700 bg-blue-100 hover:bg-blue-200 rounded-lg transition-colors text-sm font-medium inline-flex items-center justify-center gap-2"
                >
                  <Download className="h-4 w-4" />
                  Exportar CSV
                </button>
              </div>
            </div>
          )}
        </div>

        {/* Tabela de Dados */}
        <div className="bg-[var(--surface)] rounded-lg border border-[var(--border)]">
          <div className="overflow-x-auto">
            {loading ? (
              <div className="p-8 text-center">
                <Loader2 className="h-8 w-8 text-gray-400 animate-spin mx-auto mb-3" />
                <p className="text-[var(--text-muted)]">Carregando dados...</p>
              </div>
            ) : classTribs.length === 0 ? (
              <div className="p-8 text-center">
                <AlertCircle className="h-8 w-8 text-gray-400 mx-auto mb-3" />
                <p className="text-[var(--text-muted)]">Nenhum registro encontrado</p>
              </div>
            ) : (
              <table className="w-full">
                <thead className="border-b border-[var(--border)] bg-[var(--surface-muted)]">
                  <tr>
                    <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700">Código</th>
                    <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700">CST</th>
                    <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700">Descrição</th>
                    <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700">Red. IBS</th>
                    <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700">Red. CBS</th>
                    <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700">Tipo Alíq.</th>
                    <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700">NFe</th>
                    <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700">Ações</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {classTribs.map((ct) => (
                    <tr key={ct.id} className="hover:bg-[var(--surface-muted)] transition-colors">
                      <td className="px-6 py-3">
                        <span className="font-mono text-sm font-medium text-[var(--text)]">{ct.codigoClassTrib}</span>
                      </td>
                      <td className="px-6 py-3">
                        <span className="px-2 py-1 bg-blue-50 text-blue-700 text-xs font-medium rounded">
                          {ct.codigoSituacaoTributaria}
                        </span>
                      </td>
                      <td className="px-6 py-3">
                        <p className="text-sm text-gray-700 max-w-xs truncate">{ct.descricaoClassTrib}</p>
                      </td>
                      <td className="px-6 py-3 text-center">
                        <span className="text-sm font-medium text-[var(--text)]">{ct.percentualReducaoIBS.toFixed(2)}%</span>
                      </td>
                      <td className="px-6 py-3 text-center">
                        <span className="text-sm font-medium text-[var(--text)]">{ct.percentualReducaoCBS.toFixed(2)}%</span>
                      </td>
                      <td className="px-6 py-3 text-center">
                        <span className="text-xs text-[var(--text-muted)]">{ct.tipoAliquota || '-'}</span>
                      </td>
                      <td className="px-6 py-3 text-center">
                        {ct.validoParaNFe ? (
                          <CheckCircle className="h-4 w-4 text-emerald-500 mx-auto" />
                        ) : (
                          <AlertCircle className="h-4 w-4 text-gray-300 mx-auto" />
                        )}
                      </td>
                      <td className="px-6 py-3 text-center">
                        <button
                          onClick={() => abrirDetalhes(ct)}
                          className="inline-flex items-center justify-center w-8 h-8 rounded-lg bg-blue-50 text-blue-600 hover:bg-blue-100 transition-colors"
                          title="Visualizar detalhes"
                        >
                          <Eye className="h-4 w-4" />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>

          {/* Paginação */}
          {!loading && classTribs.length > 0 && (
            <div className="px-4 sm:px-6 py-4 border-t border-[var(--border)] flex flex-col sm:flex-row items-center justify-between gap-4">
              <div className="text-xs sm:text-sm text-[var(--text-muted)] text-center sm:text-left">
                Mostrando <span className="font-medium">{(page - 1) * pageSize + 1}</span> a{' '}
                <span className="font-medium">{Math.min(page * pageSize, total)}</span> de{' '}
                <span className="font-medium">{total}</span> registros
              </div>

              <div className="flex flex-wrap items-center justify-center gap-2 sm:gap-3">
                <button
                  onClick={() => setPage(Math.max(1, page - 1))}
                  disabled={page === 1}
                  className="px-3 py-1 text-sm border border-gray-300 rounded disabled:opacity-50 disabled:cursor-not-allowed hover:bg-[var(--surface-muted)]"
                >
                  Anterior
                </button>

                <span className="text-sm text-[var(--text-muted)]">
                  Página <span className="font-medium">{page}</span> de{' '}
                  <span className="font-medium">{Math.ceil(total / pageSize)}</span>
                </span>

                <button
                  onClick={() => setPage(page + 1)}
                  disabled={page >= Math.ceil(total / pageSize)}
                  className="px-3 py-1 text-sm border border-gray-300 rounded disabled:opacity-50 disabled:cursor-not-allowed hover:bg-[var(--surface-muted)]"
                >
                  Próxima
                </button>

                <select
                  value={pageSize}
                  onChange={(e) => {
                    setPageSize(Number(e.target.value));
                    setPage(1);
                  }}
                  className="px-2 py-1 text-sm border border-gray-300 rounded"
                >
                  <option value={10}>10 por página</option>
                  <option value={25}>25 por página</option>
                  <option value={50}>50 por página</option>
                  <option value={100}>100 por página</option>
                </select>
              </div>
            </div>
          )}
        </div>
      </div>

      {/* Modal de Detalhes */}
      {showModal && classTribSelecionado && (
        <div className="fixed inset-0 z-50 overflow-y-auto">
          {/* Backdrop */}
          <div 
            className="fixed inset-0 bg-black/50 backdrop-blur-sm transition-opacity"
            onClick={fecharModal}
          />
          
          {/* Modal */}
          <div className="flex min-h-full items-end sm:items-center justify-center p-0 sm:p-4">
            <div className="relative w-full sm:max-w-2xl bg-white rounded-t-2xl sm:rounded-2xl shadow-2xl transform transition-all max-h-[90vh] overflow-hidden">
              {/* Header */}
              <div className="flex items-center justify-between px-4 sm:px-6 py-4 border-b border-gray-200 bg-gradient-to-r from-blue-600 to-indigo-600 sm:rounded-t-2xl">
                <div className="flex items-center gap-2 sm:gap-3">
                  <div className="p-1.5 sm:p-2 bg-white/20 rounded-lg">
                    <FileText className="h-4 w-4 sm:h-5 sm:w-5 text-white" />
                  </div>
                  <div>
                    <h2 className="text-base sm:text-lg font-bold text-white">Detalhes da ClassTrib</h2>
                    <p className="text-xs sm:text-sm text-blue-100">Código: {classTribSelecionado.codigoClassTrib}</p>
                  </div>
                </div>
                <button
                  onClick={fecharModal}
                  className="p-2 hover:bg-white/20 rounded-lg transition-colors"
                >
                  <X className="h-5 w-5 text-white" />
                </button>
              </div>

              {/* Content */}
              <div className="p-4 sm:p-6 space-y-4 sm:space-y-6 max-h-[70vh] overflow-y-auto">
                {/* Informações Principais */}
                <div className="bg-gray-50 rounded-xl p-4 sm:p-5 border border-gray-200">
                  <div className="flex items-center gap-2 mb-3 sm:mb-4">
                    <Tag className="w-4 h-4 text-gray-600" />
                    <h3 className="text-xs font-bold text-gray-600 uppercase tracking-wider">Informações Principais</h3>
                  </div>
                  
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div>
                      <p className="text-xs text-gray-500 mb-1">Código ClassTrib</p>
                      <p className="font-mono font-bold text-base sm:text-lg text-gray-900">{classTribSelecionado.codigoClassTrib}</p>
                    </div>
                    <div>
                      <p className="text-xs text-gray-500 mb-1">Código Situação Tributária (CST)</p>
                      <span className="inline-block px-3 py-1 bg-blue-100 text-blue-700 text-sm font-medium rounded-lg">
                        {classTribSelecionado.codigoSituacaoTributaria}
                      </span>
                    </div>
                  </div>

                  <div className="mt-4">
                    <p className="text-xs text-gray-500 mb-1">Descrição da Situação Tributária</p>
                    <p className="text-sm text-gray-700">{classTribSelecionado.descricaoSituacaoTributaria || '-'}</p>
                  </div>

                  <div className="mt-4">
                    <p className="text-xs text-gray-500 mb-1">Descrição da ClassTrib</p>
                    <p className="text-sm text-gray-900 font-medium">{classTribSelecionado.descricaoClassTrib}</p>
                  </div>
                </div>

                {/* Reduções */}
                <div className="bg-emerald-50 rounded-xl p-4 sm:p-5 border border-emerald-200">
                  <div className="flex items-center gap-2 mb-3 sm:mb-4">
                    <Percent className="w-4 h-4 text-emerald-600" />
                    <h3 className="text-xs font-bold text-emerald-700 uppercase tracking-wider">Percentuais de Redução</h3>
                  </div>
                  
                  <div className="grid grid-cols-2 gap-3 sm:gap-4">
                    <div className="bg-white p-3 sm:p-4 rounded-lg border border-emerald-200">
                      <p className="text-xs text-emerald-600 mb-1">Redução IBS</p>
                      <p className="font-mono font-bold text-xl sm:text-2xl text-emerald-700">
                        {classTribSelecionado.percentualReducaoIBS.toFixed(2)}%
                      </p>
                    </div>
                    <div className="bg-white p-3 sm:p-4 rounded-lg border border-emerald-200">
                      <p className="text-xs text-emerald-600 mb-1">Redução CBS</p>
                      <p className="font-mono font-bold text-xl sm:text-2xl text-emerald-700">
                        {classTribSelecionado.percentualReducaoCBS.toFixed(2)}%
                      </p>
                    </div>
                  </div>

                  {classTribSelecionado.tipoAliquota && (
                    <div className="mt-4">
                      <p className="text-xs text-emerald-600 mb-1">Tipo de Alíquota</p>
                      <p className="text-sm text-emerald-800 font-medium">{classTribSelecionado.tipoAliquota}</p>
                    </div>
                  )}
                </div>

                {/* Status e Flags */}
                <div className="bg-gray-50 rounded-xl p-4 sm:p-5 border border-gray-200">
                  <div className="flex items-center gap-2 mb-3 sm:mb-4">
                    <CheckCircle className="w-4 h-4 text-gray-600" />
                    <h3 className="text-xs font-bold text-gray-600 uppercase tracking-wider">Status e Indicadores</h3>
                  </div>
                  
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                    <div className={`flex items-center gap-2 p-3 rounded-lg ${classTribSelecionado.validoParaNFe ? 'bg-emerald-100' : 'bg-gray-100'}`}>
                      {classTribSelecionado.validoParaNFe ? (
                        <CheckCircle className="h-5 w-5 text-emerald-600" />
                      ) : (
                        <AlertCircle className="h-5 w-5 text-gray-400" />
                      )}
                      <span className={`text-sm font-medium ${classTribSelecionado.validoParaNFe ? 'text-emerald-700' : 'text-gray-500'}`}>
                        Válido para NFe
                      </span>
                    </div>

                    <div className={`flex items-center gap-2 p-3 rounded-lg ${classTribSelecionado.tributacaoRegular ? 'bg-blue-100' : 'bg-gray-100'}`}>
                      {classTribSelecionado.tributacaoRegular ? (
                        <CheckCircle className="h-5 w-5 text-blue-600" />
                      ) : (
                        <AlertCircle className="h-5 w-5 text-gray-400" />
                      )}
                      <span className={`text-sm font-medium ${classTribSelecionado.tributacaoRegular ? 'text-blue-700' : 'text-gray-500'}`}>
                        Tributação Regular
                      </span>
                    </div>

                    <div className={`flex items-center gap-2 p-3 rounded-lg ${classTribSelecionado.creditoPresumidoOperacoes ? 'bg-amber-100' : 'bg-gray-100'}`}>
                      {classTribSelecionado.creditoPresumidoOperacoes ? (
                        <CheckCircle className="h-5 w-5 text-amber-600" />
                      ) : (
                        <AlertCircle className="h-5 w-5 text-gray-400" />
                      )}
                      <span className={`text-sm font-medium ${classTribSelecionado.creditoPresumidoOperacoes ? 'text-amber-700' : 'text-gray-500'}`}>
                        Crédito Presumido
                      </span>
                    </div>

                    <div className={`flex items-center gap-2 p-3 rounded-lg ${classTribSelecionado.estornoCredito ? 'bg-red-100' : 'bg-gray-100'}`}>
                      {classTribSelecionado.estornoCredito ? (
                        <AlertCircle className="h-5 w-5 text-red-600" />
                      ) : (
                        <CheckCircle className="h-5 w-5 text-gray-400" />
                      )}
                      <span className={`text-sm font-medium ${classTribSelecionado.estornoCredito ? 'text-red-700' : 'text-gray-500'}`}>
                        Estorno de Crédito
                      </span>
                    </div>
                  </div>
                </div>

                {/* Legislação */}
                {(classTribSelecionado.anexoLegislacao || classTribSelecionado.linkLegislacao) && (
                  <div className="bg-indigo-50 rounded-xl p-5 border border-indigo-200">
                    <div className="flex items-center gap-2 mb-4">
                      <BookOpen className="w-4 h-4 text-indigo-600" />
                      <h3 className="text-xs font-bold text-indigo-700 uppercase tracking-wider">Legislação</h3>
                    </div>
                    
                    <div className="space-y-3">
                      {classTribSelecionado.anexoLegislacao && (
                        <div>
                          <p className="text-xs text-indigo-600 mb-1">Anexo da Legislação</p>
                          <p className="text-sm text-indigo-800 font-medium">Anexo {classTribSelecionado.anexoLegislacao}</p>
                        </div>
                      )}
                      
                      {classTribSelecionado.linkLegislacao && (
                        <div>
                          <p className="text-xs text-indigo-600 mb-1">Link da Legislação</p>
                          <a
                            href={classTribSelecionado.linkLegislacao}
                            target="_blank"
                            rel="noopener noreferrer"
                            className="inline-flex items-center gap-2 text-sm text-indigo-700 hover:text-indigo-900 font-medium underline"
                          >
                            <ExternalLink className="h-4 w-4" />
                            Acessar legislação
                          </a>
                        </div>
                      )}
                    </div>
                  </div>
                )}
              </div>

              {/* Footer */}
              <div className="px-6 py-4 border-t border-gray-200 bg-gray-50 rounded-b-2xl">
                <div className="flex justify-end">
                  <button
                    onClick={fecharModal}
                    className="px-5 py-2.5 bg-gray-900 text-white rounded-lg hover:bg-gray-800 transition-colors font-medium"
                  >
                    Fechar
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
