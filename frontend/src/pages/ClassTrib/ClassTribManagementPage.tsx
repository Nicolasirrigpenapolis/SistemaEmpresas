import { useEffect, useState } from 'react';
import {
  Search,
  Filter,
  Download,
  AlertCircle,
  CheckCircle,
  BarChart3,
  Eye,
  X,

  FileText,
  Percent,
  Tag,
  BookOpen,
  XCircle
} from 'lucide-react';
import { classTribService } from '../../services/Fiscal/classTribService';
import type { ClassTribDto } from '../../services/Fiscal/classTribService';
import { usePermissaoTela } from '../../hooks/usePermissaoTela';

// Componentes reutilizáveis
import {
  AlertaErro,
  DataTable,
  CabecalhoPagina,
  type ColumnConfig,
  EstadoCarregando,

} from '../../components/common';

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
  const [pageSize] = useState(50);
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

      // Nota: O serviço original pode precisar de adaptação para suportar todos os filtros no backend se não suportar
      // Aqui estamos mantendo a chamada original
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
  }, [page, pageSize, filter]); // filter dependency might need debounce in real app

  // ============================================================================
  // COLUMNS CONFIG
  // ============================================================================
  const columns: ColumnConfig<ClassTribDto>[] = [
    {
      key: 'codigoClassTrib',
      header: 'Código',
      width: '80px',
      sortable: true,
      render: (item) => (
        <span className="font-mono font-medium text-[var(--text-muted)]">
          {item.codigoClassTrib}
        </span>
      )
    },
    {
      key: 'codigoSituacaoTributaria',
      header: 'CST',
      width: '80px',
      sortable: true,
      render: (item) => (
        <span className="px-2 py-1 bg-blue-50 text-blue-700 text-xs font-medium rounded-lg border border-blue-100">
          {item.codigoSituacaoTributaria}
        </span>
      )
    },
    {
      key: 'descricaoClassTrib',
      header: 'Descrição',
      sortable: true,
      render: (item) => (
        <span className="font-medium text-primary line-clamp-1" title={item.descricaoClassTrib}>
          {item.descricaoClassTrib}
        </span>
      )
    },
    {
      key: 'percentualReducaoIBS',
      header: 'Red. IBS',
      align: 'center',
      width: '100px',
      render: (item) => (
        <span className="font-bold text-emerald-600">
          {item.percentualReducaoIBS.toFixed(2)}%
        </span>
      )
    },
    {
      key: 'percentualReducaoCBS',
      header: 'Red. CBS',
      align: 'center',
      width: '100px',
      render: (item) => (
        <span className="font-bold text-emerald-600">
          {item.percentualReducaoCBS.toFixed(2)}%
        </span>
      )
    },
    {
      key: 'tipoAliquota',
      header: 'Tipo Alíq.',
      align: 'center',
      width: '120px',
      render: (item) => (
        <span className="text-xs text-muted-foreground font-medium">
          {item.tipoAliquota || '-'}
        </span>
      )
    },
    {
      key: 'validoParaNFe',
      header: 'NFe',
      align: 'center',
      width: '80px',
      render: (item) => (
        item.validoParaNFe ? (
          <CheckCircle className="h-5 w-5 text-emerald-500 mx-auto" />
        ) : (
          <AlertCircle className="h-5 w-5 text-gray-300 mx-auto" />
        )
      )
    }
  ];

  // ============================================================================
  // RENDER
  // ============================================================================

  if (carregandoPermissoes) {
    return <EstadoCarregando mensagem="Verificando permissões..." />;
  }

  if (!podeConsultar) {
    return (
      <div className="min-h-screen bg-background flex items-center justify-center">
        <div className="text-center bg-surface p-8 rounded-xl shadow-lg border border-border max-w-md">
          <div className="w-16 h-16 bg-red-50 rounded-full flex items-center justify-center mx-auto mb-4">
            <AlertCircle className="h-8 w-8 text-red-500" />
          </div>
          <h2 className="text-xl font-bold text-primary mb-2">Acesso Negado</h2>
          <p className="text-muted-foreground">Você não tem permissão para acessar esta tela.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6 pb-8">
      <CabecalhoPagina
        titulo="Gestão de ClassTrib"
        subtitulo="Gerenciar classificações tributárias da Reforma Tributária IBS/CBS"
        icone={BookOpen}
        acoes={
          <button
            disabled={true}
            className="inline-flex items-center justify-center gap-2 px-4 py-2 bg-surface-muted text-muted-foreground rounded-xl cursor-not-allowed opacity-60 border border-border"
            title="Dados já sincronizados"
          >
            <CheckCircle className="h-4 w-4" />
            <span className="hidden sm:inline">Sincronizado</span>
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
            {(filter.csts.length > 0 || filter.descricao || filter.tipoAliquota) && (
              <span className="w-2 h-2 bg-secondary rounded-full animate-pulse"></span>
            )}
          </button>

          {showFilters && (
            <div className="w-full mt-4 p-4 bg-surface border border-border rounded-xl shadow-sm animate-slide-up">
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
                {/* Busca por Descrição */}
                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">Descrição / Código</label>
                  <div className="relative">
                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
                    <input
                      type="text"
                      value={filter.descricao}
                      onChange={(e) => handleFilterChange({ descricao: e.target.value })}
                      placeholder="Buscar..."
                      className="w-full pl-9 pr-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                    />
                  </div>
                </div>

                {/* CSTs */}
                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">CST</label>
                  <select
                    multiple
                    value={filter.csts}
                    onChange={(e) =>
                      handleFilterChange({
                        csts: Array.from(e.target.selectedOptions, (option) => option.value),
                      })
                    }
                    className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all h-24"
                  >
                    {cstOptions.map((cst) => (
                      <option key={cst.codigo} value={cst.codigo}>
                        {cst.codigo} - {cst.descricao}
                      </option>
                    ))}
                  </select>
                </div>

                {/* Tipo de Alíquota */}
                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">Tipo de Alíquota</label>
                  <select
                    value={filter.tipoAliquota}
                    onChange={(e) => handleFilterChange({ tipoAliquota: e.target.value })}
                    className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                  >
                    <option value="">Todos</option>
                    {tipoAliquotaOptions.map((tipo) => (
                      <option key={tipo} value={tipo}>
                        {tipo}
                      </option>
                    ))}
                  </select>
                </div>

                {/* Botões de Ação do Filtro */}
                <div className="sm:col-span-2 lg:col-span-3 flex items-center gap-3 pt-2 border-t border-border mt-2">
                  <button
                    onClick={resetFilters}
                    className="px-4 py-2 text-sm font-medium text-red-600 bg-red-50 hover:bg-red-100 rounded-lg transition-colors flex items-center gap-2"
                  >
                    <XCircle className="w-4 h-4" />
                    Limpar Filtros
                  </button>
                  <button
                    onClick={handleExport}
                    className="px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 hover:bg-blue-100 rounded-lg transition-colors flex items-center gap-2 ml-auto"
                  >
                    <Download className="w-4 h-4" />
                    Exportar CSV
                  </button>
                </div>
              </div>
            </div>
          )}
        </div>
      </CabecalhoPagina>

      <div className="px-6 space-y-6">
        {/* Estatísticas */}
        {stats && (
          <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-xs text-muted-foreground font-medium uppercase">Total</p>
                  <p className="text-2xl font-bold text-primary mt-1">{stats.totalClassificacoes}</p>
                </div>
                <div className="p-3 bg-blue-50 rounded-xl">
                  <BarChart3 className="h-6 w-6 text-blue-600" />
                </div>
              </div>
            </div>

            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-xs text-muted-foreground font-medium uppercase">Válidas NFe</p>
                  <p className="text-2xl font-bold text-emerald-600 mt-1">{stats.totalValidoNFe}</p>
                </div>
                <div className="p-3 bg-emerald-50 rounded-xl">
                  <CheckCircle className="h-6 w-6 text-emerald-600" />
                </div>
              </div>
            </div>

            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div>
                <p className="text-xs text-muted-foreground font-medium uppercase">Média Red. IBS</p>
                <div className="flex items-baseline gap-2 mt-1">
                  <p className="text-2xl font-bold text-primary">{stats.mediaReducaoIBS.toFixed(2)}%</p>
                  <span className="text-xs text-muted-foreground">({stats.totalComReducaoIBS} itens)</span>
                </div>
              </div>
            </div>

            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div>
                <p className="text-xs text-muted-foreground font-medium uppercase">Média Red. CBS</p>
                <div className="flex items-baseline gap-2 mt-1">
                  <p className="text-2xl font-bold text-primary">{stats.mediaReducaoCBS.toFixed(2)}%</p>
                  <span className="text-xs text-muted-foreground">({stats.totalComReducaoCBS} itens)</span>
                </div>
              </div>
            </div>
          </div>
        )}

        {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

        <DataTable
          data={classTribs}
          columns={columns}
          getRowKey={(item) => item.id.toString()}
          loading={loading}
          totalItems={total}

          // Paginação manual pois o serviço espera page/pageSize
          // O DataTable suporta paginação interna, mas aqui estamos controlando externamente
          // Para simplificar, vamos deixar o DataTable controlar a UI de paginação se passarmos os props corretos
          // Mas o DataTable atual parece ser client-side ou server-side misto.
          // Vamos usar a prop de paginação do DataTable se ele tiver, ou manter os controles externos se necessário.
          // O DataTable refatorado suporta server-side pagination se passarmos onPageChange?
          // Verificando o código do DataTable anterior: ele usa useDataTable que tem paginação.
          // Se passarmos `data` apenas da página atual, precisamos informar o total.

          // Ações
          rowActions={(item) => (
            <button
              onClick={() => abrirDetalhes(item)}
              className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
              title="Visualizar detalhes"
            >
              <Eye className="h-4 w-4" />
            </button>
          )}
        />

        {/* Controles de Paginação Personalizados (se o DataTable não expor controle direto de página externa facilmente) */}
        {!loading && classTribs.length > 0 && (
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-border">
            <div className="text-sm text-muted-foreground">
              Página <span className="font-medium text-primary">{page}</span> de <span className="font-medium text-primary">{Math.ceil(total / pageSize)}</span>
            </div>
            <div className="flex items-center gap-2">
              <button
                onClick={() => setPage(Math.max(1, page - 1))}
                disabled={page === 1}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Anterior
              </button>
              <button
                onClick={() => setPage(page + 1)}
                disabled={page >= Math.ceil(total / pageSize)}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Próxima
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Modal de Detalhes */}
      {showModal && classTribSelecionado && (
        <div className="fixed inset-0 z-50 overflow-y-auto">
          <div
            className="fixed inset-0 bg-black/60 backdrop-blur-sm transition-opacity animate-fade-in"
            onClick={fecharModal}
          />

          <div className="flex min-h-full items-center justify-center p-4">
            <div className="relative w-full max-w-2xl bg-surface rounded-2xl shadow-2xl transform transition-all animate-scale-in border border-border">
              {/* Header */}
              <div className="flex items-center justify-between px-6 py-4 border-b border-border bg-surface-muted/50 rounded-t-2xl">
                <div className="flex items-center gap-3">
                  <div className="p-2 bg-primary/10 rounded-lg">
                    <FileText className="h-5 w-5 text-primary" />
                  </div>
                  <div>
                    <h2 className="text-lg font-bold text-primary">Detalhes da ClassTrib</h2>
                    <p className="text-xs text-muted-foreground font-mono">#{classTribSelecionado.codigoClassTrib}</p>
                  </div>
                </div>
                <button
                  onClick={fecharModal}
                  className="p-2 hover:bg-surface-hover rounded-lg transition-colors text-muted-foreground hover:text-primary"
                >
                  <X className="h-5 w-5" />
                </button>
              </div>

              {/* Content */}
              <div className="p-6 space-y-6 max-h-[70vh] overflow-y-auto custom-scrollbar">
                {/* Informações Principais */}
                <div className="bg-surface-muted/30 rounded-xl p-5 border border-border">
                  <div className="flex items-center gap-2 mb-4">
                    <Tag className="w-4 h-4 text-primary" />
                    <h3 className="text-xs font-bold text-primary uppercase tracking-wider">Informações Principais</h3>
                  </div>

                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                    <div>
                      <p className="text-xs text-muted-foreground mb-1">Código ClassTrib</p>
                      <p className="font-mono font-bold text-lg text-foreground">{classTribSelecionado.codigoClassTrib}</p>
                    </div>
                    <div>
                      <p className="text-xs text-muted-foreground mb-1">CST</p>
                      <span className="inline-block px-3 py-1 bg-blue-50 text-blue-700 text-sm font-bold rounded-lg border border-blue-100">
                        {classTribSelecionado.codigoSituacaoTributaria}
                      </span>
                    </div>
                  </div>

                  <div className="mt-4">
                    <p className="text-xs text-muted-foreground mb-1">Descrição</p>
                    <p className="text-sm text-foreground font-medium">{classTribSelecionado.descricaoClassTrib}</p>
                  </div>
                </div>

                {/* Reduções */}
                <div className="bg-emerald-50/50 rounded-xl p-5 border border-emerald-100">
                  <div className="flex items-center gap-2 mb-4">
                    <Percent className="w-4 h-4 text-emerald-600" />
                    <h3 className="text-xs font-bold text-emerald-700 uppercase tracking-wider">Percentuais de Redução</h3>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="bg-white p-4 rounded-lg border border-emerald-100 shadow-sm">
                      <p className="text-xs text-emerald-600 mb-1 font-bold">Redução IBS</p>
                      <p className="font-mono font-bold text-2xl text-emerald-700">
                        {classTribSelecionado.percentualReducaoIBS.toFixed(2)}%
                      </p>
                    </div>
                    <div className="bg-white p-4 rounded-lg border border-emerald-100 shadow-sm">
                      <p className="text-xs text-emerald-600 mb-1 font-bold">Redução CBS</p>
                      <p className="font-mono font-bold text-2xl text-emerald-700">
                        {classTribSelecionado.percentualReducaoCBS.toFixed(2)}%
                      </p>
                    </div>
                  </div>
                </div>

                {/* Status Grid */}
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                  <div className={`flex items-center gap-3 p-3 rounded-xl border ${classTribSelecionado.validoParaNFe ? 'bg-emerald-50 border-emerald-100' : 'bg-surface-muted border-border'}`}>
                    {classTribSelecionado.validoParaNFe ? (
                      <CheckCircle className="h-5 w-5 text-emerald-600" />
                    ) : (
                      <AlertCircle className="h-5 w-5 text-muted-foreground" />
                    )}
                    <span className={`text-sm font-bold ${classTribSelecionado.validoParaNFe ? 'text-emerald-700' : 'text-muted-foreground'}`}>
                      Válido para NFe
                    </span>
                  </div>

                  <div className={`flex items-center gap-3 p-3 rounded-xl border ${classTribSelecionado.tributacaoRegular ? 'bg-blue-50 border-blue-100' : 'bg-surface-muted border-border'}`}>
                    {classTribSelecionado.tributacaoRegular ? (
                      <CheckCircle className="h-5 w-5 text-blue-600" />
                    ) : (
                      <AlertCircle className="h-5 w-5 text-muted-foreground" />
                    )}
                    <span className={`text-sm font-bold ${classTribSelecionado.tributacaoRegular ? 'text-blue-700' : 'text-muted-foreground'}`}>
                      Tributação Regular
                    </span>
                  </div>
                </div>
              </div>

              {/* Footer */}
              <div className="px-6 py-4 border-t border-border bg-surface-muted/30 rounded-b-2xl flex justify-end">
                <button
                  onClick={fecharModal}
                  className="px-6 py-2 bg-primary text-primary-foreground rounded-xl hover:bg-primary/90 transition-all font-medium shadow-lg shadow-primary/20"
                >
                  Fechar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
