import { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  FileText,
  Plus,
  Filter,
  RefreshCw,
  Eye,
  Edit,
  Calendar,
  Building2,
  Loader2,
  Lock,
  Copy,
  Download,
  Printer,
} from 'lucide-react';
import { notaFiscalService } from '../../services/NotaFiscal/notaFiscalService';
import type {
  NotaFiscalListDto,
  NotaFiscalFiltroDto,
  PagedResult,
  PropriedadeComboDto,
  NaturezaOperacaoComboDto,
} from '../../types';
import { getStatusNfe, getStatusNfeColor, TIPOS_NOTA } from '../../types';
import {
  CabecalhoPagina,
  DataTable,
  type ColumnConfig,
  AlertaErro,
  ModalConfirmacao
} from '../../components/common';
import { usePermissaoTela } from '../../hooks/usePermissaoTela';

export function NotaFiscalListPage() {
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
  const [page, setPage] = useState(1);
  const [pageSize] = useState(25);

  // Estado para duplicação
  const [duplicating, setDuplicating] = useState<number | null>(null);
  const [modalDuplicar, setModalDuplicar] = useState({
    aberto: false,
    id: 0
  });

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
        pageNumber: page,
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
    page,
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
  const handleClearFilters = () => {
    setFiltroBusca('');
    setFiltroDataInicial('');
    setFiltroDataFinal('');
    setFiltroPropriedade(undefined);
    setFiltroNatureza(undefined);
    setFiltroTipoNota(undefined);
    setFiltroAutorizadas(undefined);
    setFiltroCanceladas(undefined);
    setPage(1);
  };

  const handleNew = () => {
    navigate('/faturamento/notas-fiscais/nova');
  };

  const handleView = (id: number) => {
    navigate(`/faturamento/notas-fiscais/${id}`);
  };

  const handleEdit = (id: number) => {
    navigate(`/faturamento/notas-fiscais/${id}/editar`);
  };

  const handleDuplicate = (id: number) => {
    setModalDuplicar({ aberto: true, id });
  };

  const confirmarDuplicacao = async () => {
    const id = modalDuplicar.id;
    setModalDuplicar({ aberto: false, id: 0 });
    
    if (duplicating) return;

    try {
      setDuplicating(id);
      const novaNota = await notaFiscalService.duplicar(id);
      navigate(`/notas-fiscais/editar/${novaNota.sequenciaDaNotaFiscal}`);
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao duplicar nota fiscal');
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
      red: 'bg-red-50 text-red-700 border-red-100',
      green: 'bg-emerald-50 text-emerald-700 border-emerald-100',
      blue: 'bg-blue-50 text-blue-700 border-blue-100',
      gray: 'bg-gray-50 text-gray-600 border-gray-200',
    };

    const dotColors: Record<string, string> = {
      red: 'bg-red-500',
      green: 'bg-emerald-500',
      blue: 'bg-blue-500',
      gray: 'bg-gray-400',
    };

    return (
      <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 text-xs font-bold uppercase rounded-full border ${bgColors[color]}`}>
        <span className={`w-1.5 h-1.5 rounded-full ${dotColors[color]}`} />
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
      header: 'Código',
      width: '100px',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar código...',
      render: (item) => (
        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
          #{item.sequenciaDaNotaFiscal}
        </span>
      )
    },
    {
      key: 'numeroDaNotaFiscal',
      header: 'Número / Data',
      render: (item) => (
        <div className="flex flex-col">
          <span className="text-sm font-bold text-gray-900 group-hover:text-blue-600 transition-colors">
            {item.numeroDaNfe > 0 ? (
              <>NFe {item.numeroDaNfe}</>
            ) : (
              <>Nº {item.numeroDaNotaFiscal}</>
            )}
          </span>
          <span className="text-xs text-gray-500 flex items-center gap-1 mt-0.5">
            <Calendar className="w-3 h-3 text-blue-500" />
            {formatDate(item.dataDeEmissao)}
          </span>
          <div className="flex gap-1 mt-1.5">
            {item.nfeComplementar && (
              <span className="inline-flex items-center px-1.5 py-0.5 text-[10px] font-bold uppercase bg-purple-50 text-purple-700 border border-purple-100 rounded">
                Comp
              </span>
            )}
            {item.notaDeDevolucao && (
              <span className="inline-flex items-center px-1.5 py-0.5 text-[10px] font-bold uppercase bg-orange-50 text-orange-700 border border-orange-100 rounded">
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
          <span className="text-sm font-bold text-gray-900 truncate max-w-[200px]" title={item.nomeDoCliente}>
            {item.nomeDoCliente}
          </span>
          <span className="text-xs text-gray-500 font-medium mt-0.5">
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
          <span className="text-sm font-bold text-gray-900 truncate max-w-[180px]" title={item.descricaoNatureza}>
            {item.descricaoNatureza}
          </span>
          <span className="text-xs text-gray-500 mt-0.5">
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
        <span className="text-sm font-bold text-gray-900">
          {formatCurrency(item.valorTotalDaNotaFiscal)}
        </span>
      )
    },
    {
      key: 'status', // Virtual key
      header: 'Status',
      align: 'center',
      width: '140px',
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
              className="flex items-center justify-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all shadow-lg shadow-blue-600/20 font-bold text-sm group"
            >
              <Plus className="w-4 h-4 group-hover:scale-110 transition-transform" />
              <span className="hidden sm:inline">Nova Nota Fiscal</span>
              <span className="sm:hidden">Nova</span>
            </button>
          )
        }
      >
        {/* Filtros Expandidos */}
        {showFilters && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 pt-6 border-t border-gray-100 animate-in fade-in slide-in-from-top-2 duration-300">
            {/* Data Inicial */}
            <div className="space-y-2">
              <label className="block text-xs font-bold text-gray-500 uppercase tracking-wider">
                Data Inicial
              </label>
              <div className="relative">
                <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                <input
                  type="date"
                  value={filtroDataInicial}
                  onChange={(e) => setFiltroDataInicial(e.target.value)}
                  className="w-full pl-10 pr-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all outline-none"
                />
              </div>
            </div>

            {/* Data Final */}
            <div className="space-y-2">
              <label className="block text-xs font-bold text-gray-500 uppercase tracking-wider">
                Data Final
              </label>
              <div className="relative">
                <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                <input
                  type="date"
                  value={filtroDataFinal}
                  onChange={(e) => setFiltroDataFinal(e.target.value)}
                  className="w-full pl-10 pr-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all outline-none"
                />
              </div>
            </div>

            {/* Propriedade */}
            <div className="space-y-2">
              <label className="block text-xs font-bold text-gray-500 uppercase tracking-wider">
                Empresa/Filial
              </label>
              <div className="relative">
                <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                <select
                  value={filtroPropriedade || ''}
                  onChange={(e) => setFiltroPropriedade(e.target.value ? Number(e.target.value) : undefined)}
                  className="w-full pl-10 pr-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all outline-none appearance-none"
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
            <div className="space-y-2">
              <label className="block text-xs font-bold text-gray-500 uppercase tracking-wider">
                Natureza de Operação
              </label>
              <select
                value={filtroNatureza || ''}
                onChange={(e) => setFiltroNatureza(e.target.value ? Number(e.target.value) : undefined)}
                className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all outline-none appearance-none"
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
            <div className="space-y-2">
              <label className="block text-xs font-bold text-gray-500 uppercase tracking-wider">
                Tipo de Nota
              </label>
              <select
                value={filtroTipoNota ?? ''}
                onChange={(e) => setFiltroTipoNota(e.target.value !== '' ? Number(e.target.value) : undefined)}
                className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all outline-none appearance-none"
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
            <div className="space-y-2">
              <label className="block text-xs font-bold text-gray-500 uppercase tracking-wider">
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
                className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all outline-none appearance-none"
              >
                <option value="">Todos</option>
                <option value="autorizadas">Autorizadas</option>
                <option value="pendentes">Pendentes</option>
                <option value="canceladas">Canceladas</option>
              </select>
            </div>
          </div>
        )}
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
          onFilterChange={(_, value) => {
            setFiltroBusca(value);
          }}
          onClearFilters={handleClearFilters}
          headerExtra={
            <div className="flex items-center gap-3">
              <button
                onClick={() => setShowFilters(!showFilters)}
                className={`px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-all border flex items-center gap-2 ${showFilters || hasActiveFilters
                  ? 'bg-blue-600 text-white border-blue-600 shadow-md shadow-blue-600/20'
                  : 'bg-white text-gray-500 border-gray-200 hover:border-blue-300 hover:text-blue-600'
                  }`}
              >
                <Filter className="w-4 h-4" />
                Filtros Avançados
                {hasActiveFilters && (
                  <span className="w-2 h-2 bg-amber-400 rounded-full animate-pulse" />
                )}
              </button>

              <button
                onClick={loadData}
                className="p-2 text-gray-400 hover:text-blue-600 hover:bg-blue-50 rounded-xl border border-transparent hover:border-blue-100 transition-all"
                title="Atualizar"
              >
                <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
              </button>
            </div>
          }
          rowActions={(item) => (
            <div className="flex items-center justify-end gap-2">
              <button
                onClick={() => handleView(item.sequenciaDaNotaFiscal)}
                className="p-2 text-blue-600 hover:bg-blue-50 rounded-xl border border-transparent hover:border-blue-100 transition-all"
                title="Visualizar"
              >
                <Eye className="h-4 w-4" />
              </button>

              {podeAlterar && (
                <button
                  onClick={() => handleEdit(item.sequenciaDaNotaFiscal)}
                  className="p-2 text-amber-600 hover:bg-amber-50 rounded-xl border border-transparent hover:border-amber-100 transition-all"
                  title="Editar"
                >
                  <Edit className="h-4 w-4" />
                </button>
              )}

              <button
                onClick={() => handleDuplicate(item.sequenciaDaNotaFiscal)}
                disabled={duplicating === item.sequenciaDaNotaFiscal}
                className="p-2 text-indigo-600 hover:bg-indigo-50 rounded-xl border border-transparent hover:border-indigo-100 transition-all disabled:opacity-50"
                title="Duplicar"
              >
                <Copy className="h-4 w-4" />
              </button>

              {item.autorizado && (
                <>
                  <button
                    onClick={() => window.open(`${import.meta.env.VITE_API_URL}/notafiscal/${item.sequenciaDaNotaFiscal}/danfe`, '_blank')}
                    className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-xl border border-transparent hover:border-emerald-100 transition-all"
                    title="Imprimir DANFE"
                  >
                    <Printer className="h-4 w-4" />
                  </button>
                  <button
                    onClick={() => window.open(`${import.meta.env.VITE_API_URL}/notafiscal/${item.sequenciaDaNotaFiscal}/xml`, '_blank')}
                    className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-xl border border-transparent hover:border-emerald-100 transition-all"
                    title="Baixar XML"
                  >
                    <Download className="h-4 w-4" />
                  </button>
                </>
              )}
            </div>
          )}
        />

        {/* Modal de Confirmação para Duplicação */}
        <ModalConfirmacao
          aberto={modalDuplicar.aberto}
          onCancelar={() => setModalDuplicar({ aberto: false, id: 0 })}
          onConfirmar={confirmarDuplicacao}
          titulo="Duplicar Nota Fiscal"
          mensagem="Tem certeza que deseja duplicar esta nota fiscal? Uma nova nota será criada com os mesmos dados."
          textoBotaoConfirmar="Duplicar"
          textoBotaoCancelar="Cancelar"
          variante="warning"
        />
      </div>
    </div>
  );
}

export default NotaFiscalListPage;
