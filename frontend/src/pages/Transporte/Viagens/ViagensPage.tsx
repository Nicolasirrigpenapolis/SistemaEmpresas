import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Trash2,
  Route,
  Filter,
  X,
  Eye,
  XCircle,
  Calendar,
  TrendingUp,
  TrendingDown,
  Search,
} from 'lucide-react';
import { viagemService } from '../../../services/Transporte/viagemService';
import { veiculoService } from '../../../services/Transporte/veiculoService';
import type { ViagemListDto, PagedResult, ViagemFiltros, VeiculoListDto } from '../../../types';
import { STATUS_VIAGEM } from '../../../types';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import {
  ModalConfirmacao,
  Paginacao,
  EstadoVazio,
  EstadoCarregando,
  AlertaErro,
} from '../../../components/common';

export default function ViagensPage() {
  const navigate = useNavigate();
  const { podeConsultar, podeIncluir, podeAlterar, podeExcluir, carregando: carregandoPermissoes } = usePermissaoTela('Viagem');

  const [data, setData] = useState<PagedResult<ViagemListDto> | null>(null);
  const [veiculos, setVeiculos] = useState<VeiculoListDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Filtros
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroVeiculoId, setFiltroVeiculoId] = useState<number | undefined>();
  const [filtroStatus, setFiltroStatus] = useState('');
  const [filtroDataInicio, setFiltroDataInicio] = useState('');
  const [filtroDataFim, setFiltroDataFim] = useState('');
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [showFilters, setShowFilters] = useState(false);

  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);

  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; info: string; deleting: boolean }>({
    open: false, id: 0, info: '', deleting: false,
  });

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const filtros: ViagemFiltros = { pageNumber, pageSize, incluirInativos: filtroIncluirInativos };
      if (filtroBusca) filtros.busca = filtroBusca;
      if (filtroVeiculoId) filtros.veiculoId = filtroVeiculoId;
      if (filtroStatus) filtros.status = filtroStatus;
      if (filtroDataInicio) filtros.dataInicio = filtroDataInicio;
      if (filtroDataFim) filtros.dataFim = filtroDataFim;

      const result = await viagemService.listar(filtros);
      setData(result);
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao carregar viagens');
    } finally {
      setLoading(false);
    }
  };

  const loadVeiculos = async () => {
    try {
      const lista = await veiculoService.listarAtivos();
      setVeiculos(lista);
    } catch (err) {
      console.error('Erro ao carregar veículos:', err);
    }
  };

  useEffect(() => {
    if (!carregandoPermissoes && podeConsultar) {
      loadData();
      loadVeiculos();
    }
  }, [pageNumber, pageSize, carregandoPermissoes, podeConsultar]);

  const handleSearch = () => { setPageNumber(1); loadData(); };
  const handleKeyPress = (e: React.KeyboardEvent) => { if (e.key === 'Enter') handleSearch(); };

  const handleClearFilters = () => {
    setFiltroBusca(''); setFiltroVeiculoId(undefined); setFiltroStatus('');
    setFiltroDataInicio(''); setFiltroDataFim(''); setFiltroIncluirInativos(false);
    setPageNumber(1);
    setTimeout(() => loadData(), 0);
  };

  const handleDeleteClick = (id: number, info: string) => {
    setDeleteModal({ open: true, id, info, deleting: false });
  };

  const handleDeleteConfirm = async () => {
    setDeleteModal((prev) => ({ ...prev, deleting: true }));
    try {
      await viagemService.excluir(deleteModal.id);
      setDeleteModal({ open: false, id: 0, info: '', deleting: false });
      await loadData();
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao excluir viagem');
      setDeleteModal((prev) => ({ ...prev, deleting: false }));
    }
  };

  const formatDate = (dateStr: string) => {
    if (!dateStr) return '-';
    return new Date(dateStr).toLocaleDateString('pt-BR');
  };

  const formatCurrency = (value: number | undefined | null) => {
    if (value === undefined || value === null) return 'R$ 0,00';
    return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Concluída': return 'bg-emerald-50 text-emerald-700 border-emerald-100';
      case 'Em Andamento': return 'bg-blue-50 text-blue-700 border-blue-100';
      case 'Planejada': return 'bg-amber-50 text-amber-700 border-amber-100';
      case 'Cancelada': return 'bg-rose-50 text-rose-700 border-rose-100';
      default: return 'bg-gray-50 text-gray-600 border-gray-100';
    }
  };

  if (carregandoPermissoes) return <EstadoCarregando mensagem="Verificando permissões..." />;

  if (!podeConsultar) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-center">
          <XCircle className="w-16 h-16 text-red-400 mx-auto mb-4" />
          <h2 className="text-xl font-semibold text-gray-900">Acesso Negado</h2>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 tracking-tight flex items-center gap-3">
            <div className="p-2 bg-blue-600 rounded-xl shadow-lg shadow-blue-500/20">
              <Route className="w-7 h-7 text-white" />
            </div>
            Viagens
          </h1>
          <p className="text-gray-500 mt-1 font-medium">Controle de viagens e custos</p>
        </div>
        {podeIncluir && (
          <button onClick={() => navigate('/transporte/viagens/nova')}
            className="inline-flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all shadow-lg shadow-blue-500/25 font-bold">
            <Plus className="w-5 h-5" /> Nova Viagem
          </button>
        )}
      </div>

      {/* Filtros */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-5">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input type="text" placeholder="Buscar por destino, motorista..."
              value={filtroBusca} onChange={(e) => setFiltroBusca(e.target.value)} onKeyPress={handleKeyPress}
              className="w-full pl-10 pr-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all outline-none" />
          </div>
          <div className="flex gap-2">
            <button onClick={() => setShowFilters(!showFilters)}
              className={`inline-flex items-center gap-2 px-4 py-2.5 border rounded-xl font-bold transition-all ${showFilters ? 'bg-blue-50 border-blue-200 text-blue-700' : 'border-gray-200 text-gray-600 hover:bg-gray-50'}`}>
              <Filter className="w-5 h-5" /> Filtros
            </button>
            <button onClick={handleSearch} className="px-6 py-2.5 bg-gray-900 text-white rounded-xl hover:bg-gray-800 transition-all font-bold shadow-sm">Buscar</button>
            <button onClick={handleClearFilters} className="p-2.5 border border-gray-200 text-gray-500 rounded-xl hover:bg-gray-50 transition-all" title="Limpar filtros">
              <X className="w-5 h-5" />
            </button>
          </div>
        </div>

        {showFilters && (
          <div className="mt-5 pt-5 border-t border-gray-100 grid grid-cols-1 md:grid-cols-4 gap-5 animate-in fade-in slide-in-from-top-2 duration-200">
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1.5">Veículo</label>
              <select value={filtroVeiculoId || ''} onChange={(e) => setFiltroVeiculoId(e.target.value ? Number(e.target.value) : undefined)}
                className="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none transition-all">
                <option value="">Todos</option>
                {veiculos.map((v) => (<option key={v.id} value={v.id}>{v.placa}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1.5">Status</label>
              <select value={filtroStatus} onChange={(e) => setFiltroStatus(e.target.value)}
                className="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none transition-all">
                <option value="">Todos</option>
                {STATUS_VIAGEM.map((s) => (<option key={s} value={s}>{s}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1.5">Data Início</label>
              <input type="date" value={filtroDataInicio} onChange={(e) => setFiltroDataInicio(e.target.value)}
                className="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none transition-all" />
            </div>
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1.5">Data Fim</label>
              <input type="date" value={filtroDataFim} onChange={(e) => setFiltroDataFim(e.target.value)}
                className="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none transition-all" />
            </div>
            <div className="flex items-center pt-2">
              <label className="flex items-center gap-3 cursor-pointer group">
                <div className="relative flex items-center">
                  <input type="checkbox" checked={filtroIncluirInativos} onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                    className="peer w-5 h-5 text-blue-600 border-gray-300 rounded-lg focus:ring-blue-500 transition-all cursor-pointer" />
                </div>
                <span className="text-sm font-bold text-gray-600 group-hover:text-gray-900 transition-colors">Incluir inativos</span>
              </label>
            </div>
          </div>
        )}
      </div>

      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

      {/* Tabela */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 overflow-hidden">
        {loading ? (
          <div className="p-12"><EstadoCarregando mensagem="Carregando viagens..." /></div>
        ) : !data || !data.items || data.items.length === 0 ? (
          <EstadoVazio titulo="Nenhuma viagem encontrada" descricao="Não há viagens cadastradas." icone={Route}
            acao={podeIncluir ? { texto: 'Nova Viagem', onClick: () => navigate('/transporte/viagens/nova') } : undefined} />
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Código</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Data</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Veículo</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Motorista</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Rota</th>
                    <th className="px-6 py-4 text-center text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Status</th>
                    <th className="px-6 py-4 text-right text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Receitas</th>
                    <th className="px-6 py-4 text-right text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Despesas</th>
                    <th className="px-6 py-4 text-right text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Saldo</th>
                    <th className="px-6 py-4 text-right text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Ações</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-100">
                  {data.items.map((viagem) => (
                    <tr key={viagem.id} className="group hover:bg-gray-50/50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
                          #{viagem.id}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <div className="flex items-center gap-2.5 text-gray-900 font-medium">
                          <div className="p-1.5 bg-gray-100 rounded-lg group-hover:bg-white transition-colors">
                            <Calendar className="w-4 h-4 text-gray-500" />
                          </div>
                          {formatDate(viagem.dataPartida)}
                        </div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <span className="font-bold text-foreground tracking-wider uppercase whitespace-nowrap">
                          {viagem.veiculoPlaca}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-gray-600 font-medium align-middle">{viagem.motoristaNome}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-gray-500 text-sm align-middle">
                        <div className="flex items-center gap-2">
                          <span className="font-bold text-gray-700">{viagem.origemCidade}</span>
                          <span className="text-gray-300">→</span>
                          <span className="font-bold text-gray-700">{viagem.destinoCidade}</span>
                        </div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center align-middle">
                        <span className={`inline-flex px-3 py-1 text-xs font-bold rounded-full border ${getStatusColor(viagem.status)}`}>
                          {viagem.status}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right align-middle">
                        <span className="text-emerald-600 font-bold flex items-center justify-end gap-1">
                          <TrendingUp className="w-4 h-4" />
                          {formatCurrency(viagem.receitaTotal)}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right align-middle">
                        <span className="text-rose-600 font-bold flex items-center justify-end gap-1">
                          <TrendingDown className="w-4 h-4" />
                          {formatCurrency(viagem.totalDespesas)}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right align-middle">
                        <span className={`font-bold ${viagem.saldoViagem >= 0 ? 'text-emerald-600' : 'text-rose-600'}`}>
                          {formatCurrency(viagem.saldoViagem)}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium align-middle">
                        <div className="flex items-center justify-end gap-1 opacity-0 group-hover:opacity-100 transition-all transform translate-x-2 group-hover:translate-x-0">
                          <button onClick={() => navigate(`/transporte/viagens/${viagem.id}`)} className="p-2 text-gray-400 hover:text-blue-600 hover:bg-blue-50 rounded-xl transition-all" title="Visualizar">
                            <Eye className="w-5 h-5" />
                          </button>
                          {podeAlterar && (
                            <button onClick={() => navigate(`/transporte/viagens/${viagem.id}/editar`)} className="p-2 text-gray-400 hover:text-amber-600 hover:bg-amber-50 rounded-xl transition-all" title="Editar">
                              <Edit2 className="w-5 h-5" />
                            </button>
                          )}
                          {podeExcluir && (
                            <button onClick={() => handleDeleteClick(viagem.id, `${viagem.veiculoPlaca} - ${formatDate(viagem.dataPartida)}`)} className="p-2 text-gray-400 hover:text-rose-600 hover:bg-rose-50 rounded-xl transition-all" title="Excluir">
                              <Trash2 className="w-5 h-5" />
                            </button>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="px-6 py-5 border-t border-gray-100 bg-gray-50/30">
              <Paginacao paginaAtual={data.pageNumber} totalPaginas={data.totalPages} totalItens={data.totalCount} itensPorPagina={data.pageSize}
                onMudarPagina={setPageNumber} onMudarItensPorPagina={(size: number) => { setPageSize(size); setPageNumber(1); }} />
            </div>
          </>
        )}
      </div>

      <ModalConfirmacao aberto={deleteModal.open} onCancelar={() => setDeleteModal({ open: false, id: 0, info: '', deleting: false })}
        onConfirmar={handleDeleteConfirm} titulo="Excluir Viagem" mensagem="Tem certeza que deseja excluir esta viagem?"
        nomeItem={deleteModal.info} processando={deleteModal.deleting} />
    </div>
  );
}
