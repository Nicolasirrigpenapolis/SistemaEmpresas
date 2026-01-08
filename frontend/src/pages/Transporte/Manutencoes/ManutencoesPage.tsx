import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus, Edit2, Trash2, Wrench, Filter, X, Eye, XCircle, Calendar, Search
} from 'lucide-react';
import { manutencaoService } from '../../../services/Transporte/manutencaoService';
import { veiculoService } from '../../../services/Transporte/veiculoService';
import type { ManutencaoVeiculoListDto, PagedResult, ManutencaoFiltros, VeiculoListDto } from '../../../types';
import { TIPOS_MANUTENCAO } from '../../../types';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import {
  ModalConfirmacao, Paginacao, EstadoVazio, EstadoCarregando, AlertaErro,
} from '../../../components/common';

export default function ManutencoesPage() {
  const navigate = useNavigate();
  const { podeConsultar, podeIncluir, podeAlterar, podeExcluir, carregando: carregandoPermissoes } = usePermissaoTela('ManutencaoVeiculo');

  const [data, setData] = useState<PagedResult<ManutencaoVeiculoListDto> | null>(null);
  const [veiculos, setVeiculos] = useState<VeiculoListDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroVeiculoId, setFiltroVeiculoId] = useState<number | undefined>();
  const [filtroTipoManutencao, setFiltroTipoManutencao] = useState('');
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

      const filtros: ManutencaoFiltros = { pageNumber, pageSize, incluirInativos: filtroIncluirInativos };
      if (filtroBusca) filtros.busca = filtroBusca;
      if (filtroVeiculoId) filtros.veiculoId = filtroVeiculoId;
      if (filtroTipoManutencao) filtros.tipoManutencao = filtroTipoManutencao;
      if (filtroDataInicio) filtros.dataInicio = filtroDataInicio;
      if (filtroDataFim) filtros.dataFim = filtroDataFim;

      const result = await manutencaoService.listar(filtros);
      setData(result);
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao carregar manutenções');
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
    setFiltroBusca(''); setFiltroVeiculoId(undefined); setFiltroTipoManutencao('');
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
      await manutencaoService.excluir(deleteModal.id);
      setDeleteModal({ open: false, id: 0, info: '', deleting: false });
      await loadData();
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao excluir manutenção');
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
              <Wrench className="w-7 h-7 text-white" />
            </div>
            Manutenções
          </h1>
          <p className="text-gray-500 mt-1 font-medium">Controle de manutenções de veículos</p>
        </div>
        {podeIncluir && (
          <button onClick={() => navigate('/transporte/manutencoes/nova')}
            className="inline-flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all shadow-lg shadow-blue-500/25 font-bold">
            <Plus className="w-5 h-5" /> Nova Manutenção
          </button>
        )}
      </div>

      {/* Filtros */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-5">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input type="text" placeholder="Buscar por veículo, fornecedor..." value={filtroBusca} onChange={(e) => setFiltroBusca(e.target.value)} onKeyPress={handleKeyPress}
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
          <div className="mt-5 pt-5 border-t border-gray-100 grid grid-cols-1 md:grid-cols-5 gap-5 animate-in fade-in slide-in-from-top-2 duration-200">
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1.5">Veículo</label>
              <select value={filtroVeiculoId || ''} onChange={(e) => setFiltroVeiculoId(e.target.value ? Number(e.target.value) : undefined)}
                className="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none transition-all">
                <option value="">Todos</option>
                {veiculos.map((v) => (<option key={v.id} value={v.id}>{v.placa}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1.5">Tipo</label>
              <select value={filtroTipoManutencao} onChange={(e) => setFiltroTipoManutencao(e.target.value)}
                className="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none transition-all">
                <option value="">Todos</option>
                {TIPOS_MANUTENCAO.map((t) => (<option key={t} value={t}>{t}</option>))}
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
          <div className="p-12"><EstadoCarregando mensagem="Carregando manutenções..." /></div>
        ) : !data || !data.items || data.items.length === 0 ? (
          <EstadoVazio titulo="Nenhuma manutenção encontrada" descricao="Não há manutenções cadastradas." icone={Wrench}
            acao={podeIncluir ? { texto: 'Nova Manutenção', onClick: () => navigate('/transporte/manutencoes/nova') } : undefined} />
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Código</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Data</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Veículo</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Tipo</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Fornecedor</th>
                    <th className="px-6 py-4 text-right text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Valor Total</th>
                    <th className="px-6 py-4 text-center text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Status</th>
                    <th className="px-6 py-4 text-right text-xs font-bold text-gray-500 uppercase tracking-wider align-middle">Ações</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-100">
                  {data.items.map((manut) => (
                    <tr key={manut.id} className="group hover:bg-gray-50/50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
                          #{manut.id}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <div className="flex items-center gap-2.5 text-gray-900 font-medium">
                          <div className="p-1.5 bg-gray-100 rounded-lg group-hover:bg-white transition-colors">
                            <Calendar className="w-4 h-4 text-gray-500" />
                          </div>
                          {formatDate(manut.dataManutencao)}
                        </div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <span className="font-bold text-foreground tracking-wider uppercase whitespace-nowrap">
                          {manut.veiculoPlaca}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-gray-100 text-gray-800">
                          {manut.tipoManutencao}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap align-middle">
                        <span className="text-sm text-gray-600">{manut.fornecedorNome || '-'}</span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right font-bold text-gray-900 align-middle">
                        {formatCurrency(manut.valorTotal)}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center align-middle">
                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-bold uppercase tracking-wider ${manut.ativo ? 'bg-emerald-50 text-emerald-700' : 'bg-red-50 text-red-700'}`}>
                          {manut.ativo ? 'Ativo' : 'Inativo'}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium align-middle">
                        <div className="flex items-center justify-end gap-1 opacity-0 group-hover:opacity-100 transition-all transform translate-x-2 group-hover:translate-x-0">
                          <button onClick={() => navigate(`/transporte/manutencoes/${manut.id}`)} className="p-2 text-gray-400 hover:text-blue-600 hover:bg-blue-50 rounded-xl transition-all" title="Visualizar">
                            <Eye className="w-5 h-5" />
                          </button>
                          {podeAlterar && (
                            <button onClick={() => navigate(`/transporte/manutencoes/${manut.id}/editar`)} className="p-2 text-gray-400 hover:text-amber-600 hover:bg-amber-50 rounded-xl transition-all" title="Editar">
                              <Edit2 className="w-5 h-5" />
                            </button>
                          )}
                          {podeExcluir && (
                            <button onClick={() => handleDeleteClick(manut.id, `${manut.veiculoPlaca} - ${formatDate(manut.dataManutencao)}`)} className="p-2 text-gray-400 hover:text-rose-600 hover:bg-rose-50 rounded-xl transition-all" title="Excluir">
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
        onConfirmar={handleDeleteConfirm} titulo="Excluir Manutenção" mensagem="Tem certeza que deseja excluir esta manutenção?"
        nomeItem={deleteModal.info} processando={deleteModal.deleting} />
    </div>
  );
}
