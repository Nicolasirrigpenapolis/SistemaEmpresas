import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Trash2,
  Container,
  Filter,
  X,
  Eye,
  CheckCircle,
  XCircle,
} from 'lucide-react';
import { reboqueService } from '../../../services/reboqueService';
import type { ReboqueListDto, PagedResult, ReboqueFiltros } from '../../../types/transporte';
import { TIPOS_CARROCERIA } from '../../../types/transporte';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import {
  ModalConfirmacao,
  Paginacao,
  EstadoVazio,
  EstadoCarregando,
  AlertaErro,
} from '../../../components/common';

export default function ReboquesPage() {
  const navigate = useNavigate();
  const { podeConsultar, podeIncluir, podeAlterar, podeExcluir, carregando: carregandoPermissoes } = usePermissaoTela('Reboque');

  // Estados
  const [data, setData] = useState<PagedResult<ReboqueListDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Filtros
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroTipoCarroceria, setFiltroTipoCarroceria] = useState('');
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [showFilters, setShowFilters] = useState(false);

  // Paginação
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);

  // Modal de Exclusão
  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; placa: string; deleting: boolean }>({
    open: false,
    id: 0,
    placa: '',
    deleting: false,
  });

  // Carregar dados
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const filtros: ReboqueFiltros = {
        pageNumber,
        pageSize,
        incluirInativos: filtroIncluirInativos,
      };

      if (filtroBusca) filtros.busca = filtroBusca;
      if (filtroTipoCarroceria) filtros.tipoCarroceria = filtroTipoCarroceria;

      const result = await reboqueService.listar(filtros);
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar reboques:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar reboques');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (!carregandoPermissoes && podeConsultar) {
      loadData();
    }
  }, [pageNumber, pageSize, carregandoPermissoes, podeConsultar]);

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
    setFiltroTipoCarroceria('');
    setFiltroIncluirInativos(false);
    setPageNumber(1);
    setTimeout(() => loadData(), 0);
  };

  const handleDeleteClick = (id: number, placa: string) => {
    setDeleteModal({ open: true, id, placa, deleting: false });
  };

  const handleDeleteConfirm = async () => {
    setDeleteModal((prev) => ({ ...prev, deleting: true }));
    try {
      await reboqueService.excluir(deleteModal.id);
      setDeleteModal({ open: false, id: 0, placa: '', deleting: false });
      await loadData();
    } catch (err: any) {
      console.error('Erro ao excluir:', err);
      setError(err.response?.data?.mensagem || 'Erro ao excluir reboque');
      setDeleteModal((prev) => ({ ...prev, deleting: false }));
    }
  };

  // Loading inicial
  if (carregandoPermissoes) {
    return <EstadoCarregando mensagem="Verificando permissões..." />;
  }

  // Sem permissão
  if (!podeConsultar) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-center">
          <XCircle className="w-16 h-16 text-red-400 mx-auto mb-4" />
          <h2 className="text-xl font-semibold text-primary/80">Acesso Negado</h2>
          <p className="text-muted-foreground mt-2">Você não tem permissão para acessar esta tela.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-primary flex items-center gap-2">
            <Container className="w-7 h-7 text-orange-600" />
            Reboques / Carretas
          </h1>
          <p className="text-muted-foreground mt-1">Gerencie reboques e carretas</p>
        </div>
        {podeIncluir && (
          <button
            onClick={() => navigate('/transporte/reboques/novo')}
            className="inline-flex items-center gap-2 px-4 py-2 bg-gradient-to-r from-orange-500 to-red-500 text-white rounded-lg hover:from-orange-600 hover:to-red-600 transition-colors"
          >
            <Plus className="w-5 h-5" />
            Novo Reboque
          </button>
        )}
      </div>

      {/* Filtros */}
      <div className="bg-surface rounded-xl shadow-sm border border-border p-4">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1">
            <input
              type="text"
              placeholder="Buscar por placa..."
              value={filtroBusca}
              onChange={(e) => setFiltroBusca(e.target.value)}
              onKeyPress={handleKeyPress}
              className="w-full pl-4 pr-10 py-2 border border-input rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
            />
          </div>

          <div className="flex gap-2">
            <button
              onClick={() => setShowFilters(!showFilters)}
              className={`inline-flex items-center gap-2 px-4 py-2 border rounded-lg transition-colors ${showFilters ? 'bg-orange-50 border-orange-300 text-orange-700' : 'border-input text-primary/80 hover:bg-surface-hover'
                }`}
            >
              <Filter className="w-5 h-5" />
              Filtros
            </button>
            <button onClick={handleSearch} className="px-4 py-2 bg-gradient-to-r from-orange-500 to-red-500 text-white rounded-lg hover:from-orange-600 hover:to-red-600 transition-colors">
              Buscar
            </button>
            <button onClick={handleClearFilters} className="px-4 py-2 border border-input text-primary/80 rounded-lg hover:bg-surface-hover transition-colors">
              <X className="w-5 h-5" />
            </button>
          </div>
        </div>

        {showFilters && (
          <div className="mt-4 pt-4 border-t border-border grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Tipo de Carroceria</label>
              <select
                value={filtroTipoCarroceria}
                onChange={(e) => setFiltroTipoCarroceria(e.target.value)}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
              >
                <option value="">Todos</option>
                {TIPOS_CARROCERIA.map((tipo) => (
                  <option key={tipo} value={tipo}>{tipo}</option>
                ))}
              </select>
            </div>

            <div className="flex items-end">
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="checkbox"
                  checked={filtroIncluirInativos}
                  onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                  className="w-4 h-4 text-orange-600 border-input rounded focus:ring-orange-500"
                />
                <span className="text-sm text-primary/80">Incluir inativos</span>
              </label>
            </div>
          </div>
        )}
      </div>

      {/* Erro */}
      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

      {/* Tabela */}
      <div className="bg-surface rounded-xl shadow-sm border border-border overflow-hidden">
        {loading ? (
          <div className="p-8"><EstadoCarregando mensagem="Carregando reboques..." /></div>
        ) : !data || !data.items || data.items.length === 0 ? (
          <EstadoVazio
            titulo="Nenhum reboque encontrado"
            descricao="Não há reboques cadastrados."
            icone={Container}
            acao={podeIncluir ? { texto: 'Cadastrar Reboque', onClick: () => navigate('/transporte/reboques/novo') } : undefined}
          />
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-surface-hover">
                  <tr>
                    <th className="px-6 py-3 text-left text-xs font-medium text-muted-foreground uppercase tracking-wider">Placa</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-muted-foreground uppercase tracking-wider">Marca / Modelo</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-muted-foreground uppercase tracking-wider">Tipo Carroceria</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-muted-foreground uppercase tracking-wider">Capacidade (kg)</th>
                    <th className="px-6 py-3 text-center text-xs font-medium text-muted-foreground uppercase tracking-wider">Status</th>
                    <th className="px-6 py-3 text-right text-xs font-medium text-muted-foreground uppercase tracking-wider">Ações</th>
                  </tr>
                </thead>
                <tbody className="bg-surface divide-y divide-gray-200">
                  {data.items.map((reboque) => (
                    <tr key={reboque.id} className="group hover:bg-surface-hover">
                      <td className="px-6 py-4 whitespace-nowrap text-[var(--text)]">{reboque.placa}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-[var(--text-muted)]">{reboque.marca} {reboque.modelo}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-muted-foreground">{reboque.tipoCarroceria || '-'}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-muted-foreground">{reboque.capacidadeCarga?.toLocaleString('pt-BR') || '-'}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        {reboque.ativo ? (
                          <span className="inline-flex items-center gap-1 px-2 py-1 text-xs font-medium text-green-700 bg-green-100 rounded-full">
                            <CheckCircle className="w-3 h-3" /> Ativo
                          </span>
                        ) : (
                          <span className="inline-flex items-center gap-1 px-2 py-1 text-xs font-medium text-red-700 bg-red-100 rounded-full">
                            <XCircle className="w-3 h-3" /> Inativo
                          </span>
                        )}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right">
                        <div className="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                          <button onClick={() => navigate(`/transporte/reboques/${reboque.id}`)} className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg" title="Visualizar">
                            <Eye className="w-4 h-4" />
                          </button>
                          {podeAlterar && (
                            <button onClick={() => navigate(`/transporte/reboques/${reboque.id}/editar`)} className="p-2 text-muted-foreground hover:text-amber-600 hover:bg-amber-50 rounded-lg" title="Editar">
                              <Edit2 className="w-4 h-4" />
                            </button>
                          )}
                          {podeExcluir && (
                            <button onClick={() => handleDeleteClick(reboque.id, reboque.placa)} className="p-2 text-muted-foreground hover:text-red-600 hover:bg-red-50 rounded-lg" title="Excluir">
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
              <Paginacao
                paginaAtual={data.pageNumber}
                totalPaginas={data.totalPages}
                totalItens={data.totalCount}
                itensPorPagina={data.pageSize}
                onMudarPagina={setPageNumber}
                onMudarItensPorPagina={(size: number) => { setPageSize(size); setPageNumber(1); }}
              />
            </div>
          </>
        )}
      </div>

      {/* Modal de Exclusão */}
      <ModalConfirmacao
        aberto={deleteModal.open}
        onCancelar={() => setDeleteModal({ open: false, id: 0, placa: '', deleting: false })}
        onConfirmar={handleDeleteConfirm}
        titulo="Excluir Reboque"
        mensagem="Tem certeza que deseja excluir este reboque?"
        nomeItem={deleteModal.placa}
        processando={deleteModal.deleting}
      />
    </div>
  );
}
