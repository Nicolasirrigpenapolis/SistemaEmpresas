import { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Search,
  Plus,
  Eye,
  Edit2,
  FileText,
  Filter,
  X,
  Calendar,
  Building2,
  Ban,
  CheckCircle2,
  Clock,
  AlertCircle,
  Download,
  Printer,
  RefreshCw,
  Copy,
  Loader2,
  Lock,
  ChevronDown,
} from 'lucide-react';
import { notaFiscalService } from '../../services/notaFiscalService';
import type {
  NotaFiscalListDto,
  NotaFiscalFiltroDto,
  PagedResult,
  PropriedadeComboDto,
  NaturezaOperacaoComboDto,
} from '../../types/notaFiscal';
import { getStatusNfe, getStatusNfeColor, TIPOS_NOTA } from '../../types/notaFiscal';
import {
  CabecalhoPagina,
  DataTable,
  type ColumnConfig,
  AlertaErro
} from '../../components/common';
import { usePermissaoTela } from '../../hooks/usePermissaoTela';

export default function NotaFiscalListPage() {
  const navigate = useNavigate();

  // Permissões da tela
  const { podeConsultar, podeIncluir, podeAlterar, carregando: carregandoPermissoes } = usePermissaoTela('NotaFiscal');

  // Estados principais
  const [data, setData] = useState<PagedResult<NotaFiscalListDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Filtros
  const [showFilters, setShowFilters] = useState(false);
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroDataInicial, setFiltroDataInicial] = useState('');
  const [filtroDataFinal, setFiltroDataFinal] = useState('');
  const [filtroPropriedade, setFiltroPropriedade] = useState<number | undefined>();
  const [filtroNatureza, setFiltroNatureza] = useState<number | undefined>();
  const [filtroTipoNota, setFiltroTipoNota] = useState<number | undefined>();
  const [filtroCanceladas, setFiltroCanceladas] = useState<boolean | undefined>();
  const [filtroAutorizadas, setFiltroAutorizadas] = useState<boolean | undefined>();

  // Combos
  const [propriedades, setPropriedades] = useState<PropriedadeComboDto[]>([]);
  const [naturezas, setNaturezas] = useState<NaturezaOperacaoComboDto[]>([]);

  // Paginação
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);

  // Estado para duplicação
  const [duplicating, setDuplicating] = useState<number | null>(null);

  // Carregar combos
  useEffect(() => {
    const loadCombos = async () => {
      try {
        const [propsData, natData] = await Promise.all([
          notaFiscalService.listarPropriedades(),
          notaFiscalService.listarNaturezas(),
        ]);
        setPropriedades(propsData);
        setNaturezas(natData);
      } catch (err) {
        console.error('Erro ao carregar combos:', err);
      }
    };
    loadCombos();
  }, []);

  // Carregar dados
  const loadData = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const filtro: NotaFiscalFiltroDto = {
        pageNumber,
        pageSize,
      };

      if (filtroBusca) filtro.busca = filtroBusca;
      if (filtroDataInicial) filtro.dataInicial = filtroDataInicial;
      if (filtroDataFinal) filtro.dataFinal = filtroDataFinal;
      if (filtroPropriedade) filtro.propriedade = filtroPropriedade;
      if (filtroNatureza) filtro.natureza = filtroNatureza;
      if (filtroTipoNota !== undefined) filtro.tipoDeNota = filtroTipoNota;
      if (filtroCanceladas !== undefined) filtro.canceladas = filtroCanceladas;
      if (filtroAutorizadas !== undefined) filtro.autorizadas = filtroAutorizadas;

      const result = await notaFiscalService.listar(filtro);
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar notas fiscais:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar notas fiscais');
    } finally {
      setLoading(false);
    }
  }, [
    pageNumber,
    pageSize,
    filtroBusca,
    filtroDataInicial,
    filtroDataFinal,
    filtroPropriedade,
    filtroNatureza,
    filtroTipoNota,
    filtroCanceladas,
    filtroAutorizadas,
  ]);

  useEffect(() => {
    loadData();
  }, [loadData]);

  // Handlers
  const handleSearch = () => {
    setPageNumber(1);
    loadData();
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  const handleClearFilters = () => {
    setFiltroBusca('');
    setFiltroDataInicial('');
    setFiltroDataFinal('');
    setFiltroPropriedade(undefined);
    setFiltroNatureza(undefined);
    setFiltroTipoNota(undefined);
    setFiltroCanceladas(undefined);
    setFiltroAutorizadas(undefined);
    setPageNumber(1);
  };

  const handleView = (id: number) => {
    navigate(`/faturamento/notas-fiscais/${id}`);
  };

  const handleEdit = (id: number) => {
    navigate(`/faturamento/notas-fiscais/${id}/editar`);
  };

  const handleNew = () => {
    navigate('/faturamento/notas-fiscais/nova');
  };

  const handleDuplicate = async (id: number) => {
    if (duplicating) return;

    if (!confirm('Deseja duplicar esta nota fiscal? Uma nova nota será criada com os mesmos dados.')) {
      return;
    }

    try {
      setDuplicating(id);
      const novaNota = await notaFiscalService.duplicar(id);
      alert(`Nota fiscal duplicada com sucesso! Nova nota: ${novaNota.numeroDaNotaFiscal}`);
      navigate(`/faturamento/notas-fiscais/${novaNota.sequenciaDaNotaFiscal}/editar`);
    } catch (err: any) {
      console.error('Erro ao duplicar nota fiscal:', err);
      alert(err.response?.data?.mensagem || 'Erro ao duplicar nota fiscal');
    } finally {
      setDuplicating(null);
    }
  };

  // Formatadores
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateString: string | null) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('pt-BR');
  };

  const formatCpfCnpj = (doc: string) => {
    if (!doc) return '-';
    const numeros = doc.replace(/\D/g, '');
    if (numeros.length === 11) {
      return numeros.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
    }
    if (numeros.length === 14) {
      return numeros.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
    }
    return doc;
  };

  // Componente de Status Badge
  const StatusBadge = ({ nota }: { nota: NotaFiscalListDto }) => {
    const status = getStatusNfe(nota);
    const color = getStatusNfeColor(nota);

    const bgColors: Record<string, string> = {
      red: 'bg-red-100 text-red-800 border-red-200',
      green: 'bg-green-100 text-green-800 border-green-200',
      blue: 'bg-blue-100 text-blue-800 border-blue-200',
      gray: 'bg-gray-100 text-gray-800 border-gray-200',
    };

    const icons: Record<string, React.ReactNode> = {
      red: <Ban className="h-3.5 w-3.5" />,
      green: <CheckCircle2 className="h-3.5 w-3.5" />,
      blue: <Clock className="h-3.5 w-3.5" />,
      gray: <AlertCircle className="h-3.5 w-3.5" />,
    };

    return (
      <span className={`inline-flex items-center gap-1 px-2 py-1 text-xs font-medium uppercase rounded-full border ${bgColors[color]}`}>
        {icons[color]}
        {status}
      </span>
    );
  };

  const hasActiveFilters = Boolean(
    filtroBusca ||
    filtroDataInicial ||
    filtroDataFinal ||
    filtroPropriedade ||
    filtroNatureza ||
    filtroTipoNota !== undefined ||
    filtroCanceladas !== undefined ||
    filtroAutorizadas !== undefined
  );

  // Colunas DataTable
  const columns: ColumnConfig<NotaFiscalListDto>[] = [
    {
      key: 'sequenciaDaNotaFiscal',
      header: 'Seq',
      width: '70px',
      align: 'center',
      render: (item) => (
        <span className="inline-flex items-center justify-center min-w-[32px] h-6 bg-primary/10 text-primary text-xs rounded px-1.5">
          {item.sequenciaDaNotaFiscal}
        </span>
      )
    },
    {
      key: 'numeroDaNotaFiscal',
      header: 'Número / Data',
      render: (item) => (
        <div className="flex flex-col">
          <span className="text-sm font-medium text-[var(--text)]">
            {item.numeroDaNfe > 0 ? (
              <>NFe {item.numeroDaNfe}</>
            ) : (
              <>Nº {item.numeroDaNotaFiscal}</>
            )}
          </span>
          <span className="text-xs text-muted-foreground flex items-center gap-1">
            <Calendar className="w-3 h-3" />
            {formatDate(item.dataDeEmissao)}
          </span>
          <div className="flex gap-1 mt-1">
            {item.nfeComplementar && (
              <span className="inline-flex items-center px-1.5 py-0.5 text-[10px] font-medium uppercase bg-purple-100 text-purple-700 rounded">
                Comp
              </span>
            )}
            {item.notaDeDevolucao && (
              <span className="inline-flex items-center px-1.5 py-0.5 text-[10px] font-medium uppercase bg-orange-100 text-orange-700 rounded">
                Dev
              </span>
            )}
          </div>
        </div>
      )
    },
    {
      key: 'nomeDoCliente',
      header: 'Cliente',
      render: (item) => (
        <div className="flex flex-col">
          <span className="text-sm font-medium text-foreground truncate max-w-[200px]" title={item.nomeDoCliente}>
            {item.nomeDoCliente}
          </span>
          <span className="text-xs text-muted-foreground font-mono">
            {formatCpfCnpj(item.documentoCliente)}
          </span>
        </div>
      )
    },
    {
      key: 'descricaoNatureza',
      header: 'Natureza',
      render: (item) => (
        <div className="flex flex-col">
          <span className="text-sm text-foreground truncate max-w-[180px]" title={item.descricaoNatureza}>
            {item.descricaoNatureza}
          </span>
          <span className="text-xs text-muted-foreground">
            {(TIPOS_NOTA as Record<number, string>)[item.tipoDeNota] || 'Outro'}
          </span>
        </div>
      )
    },
    {
      key: 'valorTotalDaNotaFiscal',
      header: 'Valor Total',
      align: 'right',
      render: (item) => (
        <span className="text-sm font-medium text-[var(--text)]">
          {formatCurrency(item.valorTotalDaNotaFiscal)}
        </span>
      )
    },
    {
      key: 'status', // Virtual key
      header: 'Status',
      align: 'center',
      width: '120px',
      render: (item) => <StatusBadge nota={item} />
    }
  ];

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
        titulo="Notas Fiscais"
        subtitulo="Gerencie as notas fiscais de entrada e saída"
        icone={FileText}
        acoes={
          podeIncluir && (
            <button
              onClick={handleNew}
              className="flex items-center justify-center gap-2 px-4 py-2 bg-primary text-primary-foreground rounded-xl hover:bg-primary/90 transition-all shadow-lg shadow-primary/20 font-medium"
            >
              <Plus className="w-4 h-4" />
              <span className="hidden sm:inline">Nova Nota Fiscal</span>
              <span className="sm:hidden">Nova</span>
            </button>
          )
        }
      >
        {/* Filtros */}
        <div className="flex flex-col gap-4 w-full">
          <div className="flex flex-wrap gap-3 items-center">
            <div className="relative flex-1 min-w-[200px]">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
              <input
                type="text"
                value={filtroBusca}
                onChange={(e) => setFiltroBusca(e.target.value)}
                onKeyPress={handleKeyPress}
                placeholder="Buscar por número, cliente, chave..."
                className="w-full pl-9 pr-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
              />
            </div>

            <button
              onClick={handleSearch}
              className="px-4 py-2 bg-surface-muted text-foreground border border-border hover:bg-surface-hover rounded-lg transition-colors font-medium text-sm flex items-center gap-2"
            >
              <Search className="w-4 h-4" />
              Buscar
            </button>

            <button
              onClick={() => setShowFilters(!showFilters)}
              className={`px-3 py-2 rounded-lg text-sm font-medium transition-all border flex items-center gap-2 ${showFilters || hasActiveFilters
                ? 'bg-primary/10 text-primary border-primary/20'
                : 'bg-surface-muted text-muted-foreground border-border hover:bg-surface-hover'
                }`}
            >
              <Filter className="w-4 h-4" />
              Filtros
              {hasActiveFilters && (
                <span className="w-2 h-2 bg-primary rounded-full animate-pulse" />
              )}
              <ChevronDown className={`w-3 h-3 transition-transform ${showFilters ? 'rotate-180' : ''}`} />
            </button>

            <button
              onClick={loadData}
              className="p-2 text-muted-foreground hover:text-primary hover:bg-surface-hover rounded-lg transition-colors"
              title="Atualizar"
            >
              <RefreshCw className="w-4 h-4" />
            </button>

            {hasActiveFilters && (
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
            <div className="pt-4 border-t border-border grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 animate-slide-down">
              {/* Data Inicial */}
              <div>
                <label className="block text-xs font-bold text-muted-foreground uppercase tracking-wider mb-2">
                  Data Inicial
                </label>
                <div className="relative">
                  <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                  <input
                    type="date"
                    value={filtroDataInicial}
                    onChange={(e) => setFiltroDataInicial(e.target.value)}
                    className="w-full pl-9 pr-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary"
                  />
                </div>
              </div>

              {/* Data Final */}
              <div>
                <label className="block text-xs font-bold text-muted-foreground uppercase tracking-wider mb-2">
                  Data Final
                </label>
                <div className="relative">
                  <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                  <input
                    type="date"
                    value={filtroDataFinal}
                    onChange={(e) => setFiltroDataFinal(e.target.value)}
                    className="w-full pl-9 pr-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary"
                  />
                </div>
              </div>

              {/* Propriedade */}
              <div>
                <label className="block text-xs font-bold text-muted-foreground uppercase tracking-wider mb-2">
                  Empresa/Filial
                </label>
                <div className="relative">
                  <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                  <select
                    value={filtroPropriedade || ''}
                    onChange={(e) => setFiltroPropriedade(e.target.value ? Number(e.target.value) : undefined)}
                    className="w-full pl-9 pr-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary appearance-none"
                  >
                    <option value="">Todas</option>
                    {propriedades.map((p) => (
                      <option key={p.sequenciaDaPropriedade} value={p.sequenciaDaPropriedade}>
                        {p.nome}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              {/* Natureza */}
              <div>
                <label className="block text-xs font-bold text-muted-foreground uppercase tracking-wider mb-2">
                  Natureza de Operação
                </label>
                <select
                  value={filtroNatureza || ''}
                  onChange={(e) => setFiltroNatureza(e.target.value ? Number(e.target.value) : undefined)}
                  className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary"
                >
                  <option value="">Todas</option>
                  {naturezas.map((n) => (
                    <option key={n.sequenciaDaNatureza} value={n.sequenciaDaNatureza}>
                      {n.cfop} - {n.descricao}
                    </option>
                  ))}
                </select>
              </div>

              {/* Tipo de Nota */}
              <div>
                <label className="block text-xs font-bold text-muted-foreground uppercase tracking-wider mb-2">
                  Tipo de Nota
                </label>
                <select
                  value={filtroTipoNota ?? ''}
                  onChange={(e) => setFiltroTipoNota(e.target.value !== '' ? Number(e.target.value) : undefined)}
                  className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary"
                >
                  <option value="">Todos</option>
                  {Object.entries(TIPOS_NOTA).map(([key, value]) => (
                    <option key={key} value={key}>
                      {value}
                    </option>
                  ))}
                </select>
              </div>

              {/* Status NFe */}
              <div>
                <label className="block text-xs font-bold text-muted-foreground uppercase tracking-wider mb-2">
                  Status NFe
                </label>
                <select
                  value={
                    filtroCanceladas === true
                      ? 'canceladas'
                      : filtroAutorizadas === true
                        ? 'autorizadas'
                        : filtroAutorizadas === false
                          ? 'pendentes'
                          : ''
                  }
                  onChange={(e) => {
                    const value = e.target.value;
                    setFiltroCanceladas(value === 'canceladas' ? true : undefined);
                    setFiltroAutorizadas(
                      value === 'autorizadas' ? true : value === 'pendentes' ? false : undefined
                    );
                  }}
                  className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary"
                >
                  <option value="">Todos</option>
                  <option value="autorizadas">Autorizadas</option>
                  <option value="pendentes">Pendentes</option>
                  <option value="canceladas">Canceladas</option>
                </select>
              </div>
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
          getRowKey={(item) => item.sequenciaDaNotaFiscal.toString()}
          loading={loading}
          totalItems={data?.totalCount || 0}
          rowActions={(item) => (
            <>
              <button
                onClick={() => handleView(item.sequenciaDaNotaFiscal)}
                className="p-2 text-muted-foreground hover:text-primary hover:bg-surface-hover rounded-lg transition-colors"
                title="Visualizar"
              >
                <Eye className="h-4 w-4" />
              </button>
              {podeAlterar && !item.notaCancelada && !item.autorizado && (
                <button
                  onClick={() => handleEdit(item.sequenciaDaNotaFiscal)}
                  className="p-2 text-muted-foreground hover:text-green-600 hover:bg-green-50 rounded-lg transition-colors"
                  title="Editar"
                >
                  <Edit2 className="h-4 w-4" />
                </button>
              )}
              {podeIncluir && !item.notaCancelada && (
                <button
                  onClick={() => handleDuplicate(item.sequenciaDaNotaFiscal)}
                  disabled={duplicating === item.sequenciaDaNotaFiscal}
                  className="p-2 text-muted-foreground hover:text-indigo-600 hover:bg-indigo-50 rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                  title="Duplicar nota fiscal"
                >
                  {duplicating === item.sequenciaDaNotaFiscal ? (
                    <Loader2 className="h-4 w-4 animate-spin" />
                  ) : (
                    <Copy className="h-4 w-4" />
                  )}
                </button>
              )}
              {item.autorizado && item.chaveDeAcessoDaNfe && (
                <>
                  <button
                    className="p-2 text-muted-foreground hover:text-purple-600 hover:bg-purple-50 rounded-lg transition-colors hidden sm:inline-flex"
                    title="Download XML"
                  >
                    <Download className="h-4 w-4" />
                  </button>
                  <button
                    className="p-2 text-muted-foreground hover:text-orange-600 hover:bg-orange-50 rounded-lg transition-colors hidden sm:inline-flex"
                    title="Imprimir DANFE"
                  >
                    <Printer className="h-4 w-4" />
                  </button>
                </>
              )}
            </>
          )}
        />

        {/* Paginação Manual (já que a API é paginada) */}
        {!loading && data && data.totalCount > 0 && (
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-border mt-4">
            <div className="flex items-center gap-4">
              <div className="text-sm text-muted-foreground">
                Página <span className="font-medium text-primary">{pageNumber}</span> de <span className="font-medium text-primary">{Math.ceil(data.totalCount / pageSize)}</span>
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
                onClick={() => setPageNumber(prev => Math.min(Math.ceil(data.totalCount / pageSize), prev + 1))}
                disabled={pageNumber === Math.ceil(data.totalCount / pageSize)}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Próxima
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
