import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus, Edit2, Trash2, Wrench, Filter, X, Eye, CheckCircle, XCircle, Calendar
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
          <h2 className="text-xl font-semibold text-primary/80">Acesso Negado</h2>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-primary flex items-center gap-2">
            <Wrench className="w-7 h-7 text-purple-600" /> Manutenções
          </h1>
          <p className="text-muted-foreground mt-1">Controle de manutenções de veículos</p>
        </div>
        {podeIncluir && (
          <button onClick={() => navigate('/transporte/manutencoes/nova')}
            className="inline-flex items-center gap-2 px-4 py-2 bg-gradient-to-r from-purple-500 to-indigo-600 text-white rounded-lg hover:from-purple-600 hover:to-indigo-700 transition-colors">
            <Plus className="w-5 h-5" /> Nova Manutenção
          </button>
        )}
      </div>

      <div className="bg-surface rounded-xl shadow-sm border border-border p-4">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1">
            <input type="text" placeholder="Buscar..." value={filtroBusca} onChange={(e) => setFiltroBusca(e.target.value)} onKeyPress={handleKeyPress}
              className="w-full pl-4 pr-10 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500" />
          </div>
          <div className="flex gap-2">
            <button onClick={() => setShowFilters(!showFilters)}
              className={`inline-flex items-center gap-2 px-4 py-2 border rounded-lg transition-colors ${showFilters ? 'bg-purple-50 border-purple-300 text-purple-700' : 'border-input text-primary/80 hover:bg-surface-hover'}`}>
              <Filter className="w-5 h-5" /> Filtros
            </button>
            <button onClick={handleSearch} className="px-4 py-2 bg-gradient-to-r from-purple-500 to-indigo-600 text-white rounded-lg hover:from-purple-600 hover:to-indigo-700">Buscar</button>
            <button onClick={handleClearFilters} className="px-4 py-2 border border-input text-primary/80 rounded-lg hover:bg-surface-hover">
              <X className="w-5 h-5" />
            </button>
          </div>
        </div>

        {showFilters && (
          <div className="mt-4 pt-4 border-t border-border grid grid-cols-1 md:grid-cols-5 gap-4">
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Veículo</label>
              <select value={filtroVeiculoId || ''} onChange={(e) => setFiltroVeiculoId(e.target.value ? Number(e.target.value) : undefined)}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500">
                <option value="">Todos</option>
                {veiculos.map((v) => (<option key={v.id} value={v.id}>{v.placa}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Tipo</label>
              <select value={filtroTipoManutencao} onChange={(e) => setFiltroTipoManutencao(e.target.value)}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500">
                <option value="">Todos</option>
                {TIPOS_MANUTENCAO.map((t) => (<option key={t} value={t}>{t}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Data Início</label>
              <input type="date" value={filtroDataInicio} onChange={(e) => setFiltroDataInicio(e.target.value)}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Data Fim</label>
              <input type="date" value={filtroDataFim} onChange={(e) => setFiltroDataFim(e.target.value)}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500" />
            </div>
            <div className="flex items-end">
              <label className="flex items-center gap-2 cursor-pointer">
                <input type="checkbox" checked={filtroIncluirInativos} onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                  className="w-4 h-4 text-purple-600 border-input rounded focus:ring-purple-500" />
                <span className="text-sm text-primary/80">Incluir inativos</span>
              </label>
            </div>
          </div>
        )}
      </div>

      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

      <div className="bg-surface rounded-xl shadow-sm border border-border overflow-hidden">
        {loading ? (
          <div className="p-8"><EstadoCarregando mensagem="Carregando manutenções..." /></div>
        ) : !data || !data.items || data.items.length === 0 ? (
          <EstadoVazio titulo="Nenhuma manutenção encontrada" descricao="Não há manutenções cadastradas." icone={Wrench}
            acao={podeIncluir ? { texto: 'Nova Manutenção', onClick: () => navigate('/transporte/manutencoes/nova') } : undefined} />
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-surface-hover">
                  <tr>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">Data</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">Veículo</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">Tipo</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">Fornecedor</th>
                    <th className="px-4 py-3 text-right text-xs font-medium text-muted-foreground uppercase">Valor Total</th>
                    <th className="px-4 py-3 text-center text-xs font-medium text-muted-foreground uppercase">Status</th>
                    <th className="px-4 py-3 text-right text-xs font-medium text-muted-foreground uppercase">Ações</th>
                  </tr>
                </thead>
                <tbody className="bg-surface divide-y divide-gray-200">
                  {data.items.map((manut) => (
                    <tr key={manut.id} className="group hover:bg-surface-hover">
                      <td className="px-4 py-3 whitespace-nowrap">
                        <div className="flex items-center gap-2 text-[var(--text)]">
                          <Calendar className="w-4 h-4 text-gray-400" />
                          {formatDate(manut.dataManutencao)}
                        </div>
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-[var(--text)]">{manut.veiculoPlaca}</td>
                      <td className="px-4 py-3 whitespace-nowrap text-muted-foreground">{manut.tipoManutencao || '-'}</td>
                      <td className="px-4 py-3 whitespace-nowrap text-muted-foreground">{manut.fornecedorNome || '-'}</td>
                      <td className="px-4 py-3 whitespace-nowrap text-right font-medium text-[var(--text)]">{formatCurrency(manut.valorTotal)}</td>
                      <td className="px-4 py-3 whitespace-nowrap text-center">
                        {manut.ativo ? (
                          <span className="inline-flex items-center gap-1 px-2 py-1 text-xs font-medium text-green-700 bg-green-100 rounded-full">
                            <CheckCircle className="w-3 h-3" /> Ativo
                          </span>
                        ) : (
                          <span className="inline-flex items-center gap-1 px-2 py-1 text-xs font-medium text-red-700 bg-red-100 rounded-full">
                            <XCircle className="w-3 h-3" /> Inativo
                          </span>
                        )}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-right">
                        <div className="flex items-center justify-end gap-1 opacity-0 group-hover:opacity-100 transition-opacity">
                          <button onClick={() => navigate(`/transporte/manutencoes/${manut.id}`)} className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg" title="Visualizar">
                            <Eye className="w-4 h-4" />
                          </button>
                          {podeAlterar && (
                            <button onClick={() => navigate(`/transporte/manutencoes/${manut.id}/editar`)} className="p-2 text-muted-foreground hover:text-amber-600 hover:bg-amber-50 rounded-lg" title="Editar">
                              <Edit2 className="w-4 h-4" />
                            </button>
                          )}
                          {podeExcluir && (
                            <button onClick={() => handleDeleteClick(manut.id, `${manut.veiculoPlaca} - ${formatDate(manut.dataManutencao)}`)} className="p-2 text-muted-foreground hover:text-red-600 hover:bg-red-50 rounded-lg" title="Excluir">
                              <Trash2 className="w-4 h-4" />
                            </button>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="px-6 py-4 border-t border-border">
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
