import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Trash2,
  Truck,
  Eye,
  XCircle,
  RefreshCw,
} from 'lucide-react';
import { veiculoService } from '../../../services/Transporte/veiculoService';
import type { VeiculoListDto, PagedResult, VeiculoFiltros } from '../../../types';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import {
  ModalConfirmacao,
  EstadoCarregando,
  AlertaErro,
  CabecalhoPagina,
  DataTable,
  type ColumnConfig
} from '../../../components/common';

export default function VeiculosPage() {
  const navigate = useNavigate();
  const { podeConsultar, podeIncluir, podeAlterar, podeExcluir, carregando: carregandoPermissoes } = usePermissaoTela('Veiculo');

  const [data, setData] = useState<PagedResult<VeiculoListDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroIncluirInativos, setFiltroIncluirInativos] = useState(false);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(25);
  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; placa: string; deleting: boolean }>({
    open: false,
    id: 0,
    placa: '',
    deleting: false,
  });

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      const filtros: VeiculoFiltros = {
        pageNumber,
        pageSize,
        incluirInativos: filtroIncluirInativos,
      };
      if (filtroBusca) filtros.busca = filtroBusca;
      const result = await veiculoService.listar(filtros);
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar veículos:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar veículos');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (!carregandoPermissoes && podeConsultar) {
      loadData();
    }
  }, [pageNumber, pageSize, carregandoPermissoes, podeConsultar, filtroIncluirInativos]);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (!carregandoPermissoes && podeConsultar) {
        loadData();
      }
    }, 500);
    return () => clearTimeout(timer);
  }, [filtroBusca]);

  const handleClearFilters = () => {
    setFiltroBusca('');
    setFiltroIncluirInativos(false);
    setPageNumber(1);
  };

  const handleDeleteClick = (id: number, placa: string) => {
    setDeleteModal({ open: true, id, placa, deleting: false });
  };

  const handleDeleteConfirm = async () => {
    setDeleteModal((prev) => ({ ...prev, deleting: true }));
    try {
      await veiculoService.excluir(deleteModal.id);
      setDeleteModal({ open: false, id: 0, placa: '', deleting: false });
      await loadData();
    } catch (err: any) {
      console.error('Erro ao excluir:', err);
      setError(err.response?.data?.mensagem || 'Erro ao excluir veículo');
      setDeleteModal((prev) => ({ ...prev, deleting: false }));
    }
  };

  const columns: ColumnConfig<VeiculoListDto>[] = [
    {
      key: 'id',
      header: 'Código',
      width: '100px',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar código...',
      render: (item) => (
        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
          #{item.id}
        </span>
      )
    },
    {
      key: 'placa',
      header: 'Placa',
      width: '150px',
      filterable: true,
      searchPlaceholder: 'Buscar placa...',
      render: (item) => (
        <span className="font-bold text-foreground tracking-wider uppercase whitespace-nowrap">
          {item.placa}
        </span>
      )
    },
    {
      key: 'marca',
      header: 'Marca / Modelo',
      filterable: true,
      searchPlaceholder: 'Buscar marca/modelo...',
      render: (item) => (
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-xl bg-blue-100 flex items-center justify-center text-blue-600 font-bold shadow-sm">
            <Truck className="w-5 h-5" />
          </div>
          <div>
            <p className="font-bold text-gray-900">{item.marca} {item.modelo}</p>
            <p className="text-xs text-gray-500">Frota Própria</p>
          </div>
        </div>
      )
    },
    {
      key: 'tipoVeiculo',
      header: 'Tipo',
      width: '120px',
      render: (item) => (
        <span className="inline-flex items-center px-2.5 py-1 rounded-lg bg-blue-50 text-blue-700 text-xs font-bold uppercase tracking-wider border border-blue-100">
          {item.tipoVeiculo || '-'}
        </span>
      )
    },
    {
      key: 'capacidadeCarga',
      header: 'Capacidade',
      width: '110px',
      render: (item) => (
        <div className="flex items-center gap-2 text-sm text-gray-600 font-medium">
          <span className="text-gray-900 font-bold">{item.capacidadeCarga?.toLocaleString('pt-BR') || '-'}</span>
          <span className="text-[10px] text-gray-400 uppercase font-bold">kg</span>
        </div>
      )
    },
    {
      key: 'ativo',
      header: 'Status',
      width: '120px',
      render: (item) => (
        <span className={`inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full text-xs font-bold border ${
          item.ativo ? 'bg-emerald-50 text-emerald-700 border-emerald-100' : 'bg-red-50 text-red-700 border-red-100'
        }`}>
          <span className={`w-1.5 h-1.5 rounded-full ${item.ativo ? 'bg-emerald-500' : 'bg-red-500'}`} />
          {item.ativo ? 'Ativo' : 'Inativo'}
        </span>
      )
    }
  ];

  if (carregandoPermissoes) {
    return <EstadoCarregando mensagem="Verificando permissões..." />;
  }

  if (!podeConsultar) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-center">
          <XCircle className="w-16 h-16 text-red-400 mx-auto mb-4" />
          <h2 className="text-xl font-semibold text-gray-700">Acesso Negado</h2>
          <p className="text-gray-500 mt-2">Você não tem permissão para acessar esta tela.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6 pb-8">
      <CabecalhoPagina
        titulo="Veículos"
        subtitulo="Gerencie a frota de veículos e caminhões"
        icone={Truck}
        acoes={
          podeIncluir && (
            <button
              onClick={() => navigate('/transporte/veiculos/novo')}
              className="group flex items-center gap-2 px-5 py-3 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all duration-300 shadow-lg shadow-blue-500/25"
            >
              <Plus className="w-5 h-5 group-hover:scale-110 transition-transform" />
              <span className="font-bold text-sm uppercase tracking-wider">Novo Veículo</span>
            </button>
          )
        }
      />

      <div className="px-6">
        {error && (
          <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} className="mb-6" />
        )}

        <DataTable
          data={data?.items || []}
          columns={columns}
          getRowKey={(item) => item.id.toString()}
          loading={loading}
          totalItems={data?.totalCount || 0}
          onFilterChange={(_, value) => {
            setFiltroBusca(value);
            setPageNumber(1);
          }}
          onClearFilters={handleClearFilters}
          headerExtra={
            <div className="flex items-center gap-3">
              <label className="flex items-center gap-2 px-3 py-2 bg-gray-50 border border-gray-200 rounded-xl cursor-pointer hover:bg-gray-100 transition-colors group">
                <input
                  type="checkbox"
                  checked={filtroIncluirInativos}
                  onChange={(e) => setFiltroIncluirInativos(e.target.checked)}
                  className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                />
                <span className="text-xs text-gray-600 font-bold uppercase tracking-wider group-hover:text-gray-900">Incluir inativos</span>
              </label>
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
                onClick={() => navigate(`/transporte/veiculos/${item.id}`)}
                className="p-2 text-blue-600 hover:bg-blue-50 rounded-xl border border-transparent hover:border-blue-100 transition-all"
                title="Visualizar"
              >
                <Eye className="h-4 w-4" />
              </button>
              {podeAlterar && (
                <button
                  onClick={() => navigate(`/transporte/veiculos/${item.id}/editar`)}
                  className="p-2 text-amber-600 hover:bg-amber-50 rounded-xl border border-transparent hover:border-amber-100 transition-all"
                  title="Editar"
                >
                  <Edit2 className="h-4 w-4" />
                </button>
              )}
              {podeExcluir && (
                <button
                  onClick={() => handleDeleteClick(item.id, item.placa)}
                  className="p-2 text-red-600 hover:bg-red-50 rounded-xl border border-transparent hover:border-red-100 transition-all"
                  title="Excluir"
                >
                  <Trash2 className="h-4 w-4" />
                </button>
              )}
            </div>
          )}
        />
      </div>

      <ModalConfirmacao
        aberto={deleteModal.open}
        onCancelar={() => setDeleteModal({ open: false, id: 0, placa: '', deleting: false })}
        onConfirmar={handleDeleteConfirm}
        titulo="Excluir Veículo"
        mensagem="Tem certeza que deseja excluir este veículo? Esta ação não pode ser desfeita."
        nomeItem={deleteModal.placa}
        processando={deleteModal.deleting}
      />
    </div>
  );
}
