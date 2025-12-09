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
  ChevronDown,
  RefreshCw,
  Copy,
  Loader2,
  Lock,
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
  Paginacao,
  EstadoVazio,
  EstadoCarregando,
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

  // Estado para duplicação
  const [duplicating, setDuplicating] = useState<number | null>(null);

  const handleDuplicate = async (id: number) => {
    if (duplicating) return;
    
    if (!confirm('Deseja duplicar esta nota fiscal? Uma nova nota será criada com os mesmos dados.')) {
      return;
    }

    try {
      setDuplicating(id);
      const novaNota = await notaFiscalService.duplicar(id);
      alert(`Nota fiscal duplicada com sucesso! Nova nota: ${novaNota.numeroDaNotaFiscal}`);
      // Redirecionar para edição da nova nota
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
      <span className={`inline-flex items-center gap-1 px-2 py-1 text-xs font-medium rounded-full border ${bgColors[color]}`}>
        {icons[color]}
        {status}
      </span>
    );
  };

  // Verificar filtros ativos
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

  // Carregando permissões
  if (carregandoPermissoes) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <div className="text-center">
          <Loader2 className="h-8 w-8 animate-spin text-blue-600 mx-auto mb-4" />
          <p className="text-gray-500">Verificando permissões...</p>
        </div>
      </div>
    );
  }

  // Sem permissão de consulta
  if (!podeConsultar) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <div className="text-center bg-white p-8 rounded-xl shadow-lg max-w-md">
          <Lock className="h-12 w-12 text-red-500 mx-auto mb-4" />
          <h2 className="text-xl font-bold text-gray-900 mb-2">Acesso Negado</h2>
          <p className="text-gray-500">Você não tem permissão para acessar esta tela.</p>
          <p className="text-sm text-gray-400 mt-2">Entre em contato com o administrador para solicitar acesso.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
            <FileText className="h-7 w-7 text-blue-600" />
            Notas Fiscais
          </h1>
          <p className="mt-1 text-sm text-gray-500">
            Gerencie as notas fiscais de entrada e saída
          </p>
        </div>
        {podeIncluir && (
          <button
            onClick={handleNew}
            className="inline-flex items-center gap-2 px-4 py-2.5 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 transition-colors shadow-sm"
          >
            <Plus className="h-4 w-4" />
            Nova Nota Fiscal
          </button>
        )}
      </div>

      {/* Card de Filtros */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-200">
        {/* Barra de busca principal */}
        <div className="p-4 border-b border-gray-100">
          <div className="flex flex-col sm:flex-row gap-3">
            <div className="flex-1 relative">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
              <input
                type="text"
                value={filtroBusca}
                onChange={(e) => setFiltroBusca(e.target.value)}
                onKeyPress={handleKeyPress}
                placeholder="Buscar por número, cliente, chave de acesso..."
                className="w-full pl-10 pr-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div className="flex gap-2">
              <button
                onClick={handleSearch}
                className="inline-flex items-center gap-2 px-4 py-2.5 bg-gray-900 text-white text-sm font-medium rounded-lg hover:bg-gray-800 transition-colors"
              >
                <Search className="h-4 w-4" />
                Buscar
              </button>
              <button
                onClick={() => setShowFilters(!showFilters)}
                className={`inline-flex items-center gap-2 px-4 py-2.5 text-sm font-medium rounded-lg border transition-colors ${
                  showFilters || hasActiveFilters
                    ? 'bg-blue-50 border-blue-200 text-blue-700'
                    : 'bg-white border-gray-300 text-gray-700 hover:bg-gray-50'
                }`}
              >
                <Filter className="h-4 w-4" />
                Filtros
                {hasActiveFilters && (
                  <span className="ml-1 px-1.5 py-0.5 text-xs bg-blue-600 text-white rounded-full">
                    !
                  </span>
                )}
                <ChevronDown className={`h-4 w-4 transition-transform ${showFilters ? 'rotate-180' : ''}`} />
              </button>
              <button
                onClick={loadData}
                className="p-2.5 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-lg transition-colors"
                title="Atualizar"
              >
                <RefreshCw className="h-4 w-4" />
              </button>
            </div>
          </div>
        </div>

        {/* Filtros avançados */}
        {showFilters && (
          <div className="p-4 bg-gray-50 border-b border-gray-100">
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
              {/* Data Inicial */}
              <div>
                <label className="block text-xs font-medium text-gray-600 mb-1.5">
                  Data Inicial
                </label>
                <div className="relative">
                  <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <input
                    type="date"
                    value={filtroDataInicial}
                    onChange={(e) => setFiltroDataInicial(e.target.value)}
                    className="w-full pl-10 pr-3 py-2 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              {/* Data Final */}
              <div>
                <label className="block text-xs font-medium text-gray-600 mb-1.5">
                  Data Final
                </label>
                <div className="relative">
                  <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <input
                    type="date"
                    value={filtroDataFinal}
                    onChange={(e) => setFiltroDataFinal(e.target.value)}
                    className="w-full pl-10 pr-3 py-2 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              {/* Propriedade */}
              <div>
                <label className="block text-xs font-medium text-gray-600 mb-1.5">
                  Empresa/Filial
                </label>
                <div className="relative">
                  <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <select
                    value={filtroPropriedade || ''}
                    onChange={(e) => setFiltroPropriedade(e.target.value ? Number(e.target.value) : undefined)}
                    className="w-full pl-10 pr-3 py-2 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 appearance-none bg-white"
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
                <label className="block text-xs font-medium text-gray-600 mb-1.5">
                  Natureza de Operação
                </label>
                <select
                  value={filtroNatureza || ''}
                  onChange={(e) => setFiltroNatureza(e.target.value ? Number(e.target.value) : undefined)}
                  className="w-full px-3 py-2 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
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
                <label className="block text-xs font-medium text-gray-600 mb-1.5">
                  Tipo de Nota
                </label>
                <select
                  value={filtroTipoNota ?? ''}
                  onChange={(e) => setFiltroTipoNota(e.target.value !== '' ? Number(e.target.value) : undefined)}
                  className="w-full px-3 py-2 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
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
                <label className="block text-xs font-medium text-gray-600 mb-1.5">
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
                  className="w-full px-3 py-2 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                >
                  <option value="">Todos</option>
                  <option value="autorizadas">Autorizadas</option>
                  <option value="pendentes">Pendentes</option>
                  <option value="canceladas">Canceladas</option>
                </select>
              </div>
            </div>

            {/* Botão limpar filtros */}
            {hasActiveFilters && (
              <div className="mt-4 flex justify-end">
                <button
                  onClick={handleClearFilters}
                  className="inline-flex items-center gap-1.5 px-3 py-1.5 text-sm text-gray-600 hover:text-gray-900 hover:bg-gray-100 rounded-lg transition-colors"
                >
                  <X className="h-4 w-4" />
                  Limpar filtros
                </button>
              </div>
            )}
          </div>
        )}

        {/* Tabela */}
        <div className="overflow-x-auto">
          {loading ? (
            <EstadoCarregando mensagem="Carregando notas fiscais..." />
          ) : error ? (
            <div className="p-8 text-center">
              <AlertCircle className="h-12 w-12 text-red-500 mx-auto mb-4" />
              <p className="text-red-600 font-medium">{error}</p>
              <button
                onClick={loadData}
                className="mt-4 text-blue-600 hover:text-blue-700 text-sm font-medium"
              >
                Tentar novamente
              </button>
            </div>
          ) : !data || data.items.length === 0 ? (
            <EstadoVazio
              titulo="Nenhuma nota fiscal encontrada"
              descricao={hasActiveFilters ? 'Tente ajustar os filtros de busca' : 'Comece criando uma nova nota fiscal'}
              tipoBusca={hasActiveFilters}
              icone={FileText}
              acao={
                !hasActiveFilters ? {
                  texto: 'Nova Nota Fiscal',
                  onClick: handleNew,
                } : undefined
              }
            />
          ) : (
            <table className="min-w-full divide-y divide-gray-200">
              <thead>
                <tr className="bg-gray-50">
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider w-16">
                    Seq
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                    Número / Data
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider hidden md:table-cell">
                    Cliente
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider hidden lg:table-cell">
                    Natureza
                  </th>
                  <th className="px-4 py-3 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider">
                    Valor Total
                  </th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider hidden sm:table-cell">
                    Status
                  </th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider">
                    Ações
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {data.items.map((nota) => (
                  <tr
                    key={nota.sequenciaDaNotaFiscal}
                    className="hover:bg-gray-50 transition-colors"
                  >
                    {/* Código de Sequência */}
                    <td className="px-4 py-3 text-center">
                      <span className="inline-flex items-center justify-center w-10 h-8 bg-blue-100 text-blue-700 text-sm font-bold rounded">
                        {nota.sequenciaDaNotaFiscal}
                      </span>
                    </td>
                    
                    {/* Número / Data */}
                    <td className="px-4 py-3">
                      <div className="flex flex-col">
                        <span className="text-sm font-semibold text-gray-900">
                          {nota.numeroDaNfe > 0 ? (
                            <>NFe {nota.numeroDaNfe}</>
                          ) : (
                            <>Nº {nota.numeroDaNotaFiscal}</>
                          )}
                        </span>
                        <span className="text-xs text-gray-500">
                          {formatDate(nota.dataDeEmissao)}
                        </span>
                        {nota.nfeComplementar && (
                          <span className="mt-1 inline-flex items-center px-1.5 py-0.5 text-xs font-medium bg-purple-100 text-purple-700 rounded">
                            Complementar
                          </span>
                        )}
                        {nota.notaDeDevolucao && (
                          <span className="mt-1 inline-flex items-center px-1.5 py-0.5 text-xs font-medium bg-orange-100 text-orange-700 rounded">
                            Devolução
                          </span>
                        )}
                      </div>
                    </td>

                    {/* Cliente */}
                    <td className="px-4 py-3 hidden md:table-cell">
                      <div className="flex flex-col">
                        <span className="text-sm font-medium text-gray-900 truncate max-w-xs">
                          {nota.nomeDoCliente}
                        </span>
                        <span className="text-xs text-gray-500">
                          {formatCpfCnpj(nota.documentoCliente)}
                        </span>
                      </div>
                    </td>

                    {/* Natureza */}
                    <td className="px-4 py-3 hidden lg:table-cell">
                      <div className="flex flex-col">
                        <span className="text-sm text-gray-900 truncate max-w-xs">
                          {nota.descricaoNatureza}
                        </span>
                        <span className="text-xs text-gray-500">
                          {(TIPOS_NOTA as Record<number, string>)[nota.tipoDeNota] || 'Outro'}
                        </span>
                      </div>
                    </td>

                    {/* Valor Total */}
                    <td className="px-4 py-3 text-right">
                      <span className="text-sm font-semibold text-gray-900">
                        {formatCurrency(nota.valorTotalDaNotaFiscal)}
                      </span>
                    </td>

                    {/* Status */}
                    <td className="px-4 py-3 text-center hidden sm:table-cell">
                      <StatusBadge nota={nota} />
                    </td>

                    {/* Ações */}
                    <td className="px-2 sm:px-4 py-3">
                      <div className="flex items-center justify-center gap-0.5 sm:gap-1">
                        <button
                          onClick={() => handleView(nota.sequenciaDaNotaFiscal)}
                          className="p-2 text-gray-500 hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                          title="Visualizar"
                        >
                          <Eye className="h-4 w-4" />
                        </button>
                        {podeAlterar && !nota.notaCancelada && !nota.autorizado && (
                          <button
                            onClick={() => handleEdit(nota.sequenciaDaNotaFiscal)}
                            className="p-2 text-gray-500 hover:text-green-600 hover:bg-green-50 rounded-lg transition-colors"
                            title="Editar"
                          >
                            <Edit2 className="h-4 w-4" />
                          </button>
                        )}
                        {/* Botão de Duplicar - disponível para notas não canceladas */}
                        {podeIncluir && !nota.notaCancelada && (
                          <button
                            onClick={() => handleDuplicate(nota.sequenciaDaNotaFiscal)}
                            disabled={duplicating === nota.sequenciaDaNotaFiscal}
                            className="p-2 text-gray-500 hover:text-indigo-600 hover:bg-indigo-50 rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                            title="Duplicar nota fiscal"
                          >
                            {duplicating === nota.sequenciaDaNotaFiscal ? (
                              <Loader2 className="h-4 w-4 animate-spin" />
                            ) : (
                              <Copy className="h-4 w-4" />
                            )}
                          </button>
                        )}
                        {nota.autorizado && nota.chaveDeAcessoDaNfe && (
                          <div className="hidden sm:flex items-center gap-0.5 sm:gap-1">
                            <button
                              className="p-2 text-gray-500 hover:text-purple-600 hover:bg-purple-50 rounded-lg transition-colors"
                              title="Download XML"
                            >
                              <Download className="h-4 w-4" />
                            </button>
                            <button
                              className="p-2 text-gray-500 hover:text-orange-600 hover:bg-orange-50 rounded-lg transition-colors"
                              title="Imprimir DANFE"
                            >
                              <Printer className="h-4 w-4" />
                            </button>
                          </div>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>

        {/* Paginação */}
        {data && data.totalCount > 0 && (
          <div className="px-4 py-3 border-t border-gray-200">
            <Paginacao
              paginaAtual={pageNumber}
              totalPaginas={Math.ceil(data.totalCount / pageSize)}
              totalItens={data.totalCount}
              itensPorPagina={pageSize}
              onMudarPagina={setPageNumber}
              onMudarItensPorPagina={(newSize: number) => {
                setPageSize(newSize);
                setPageNumber(1);
              }}
            />
          </div>
        )}
      </div>
    </div>
  );
}
