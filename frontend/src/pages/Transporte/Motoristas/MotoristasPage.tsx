import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Eye,
  User,
  Phone,
  MapPin,
  Trash2,
  RefreshCw,
} from 'lucide-react';
import { motoristaService } from '../../../services/Transporte/motoristaService';
import type { MotoristaListDto, PagedResult, MotoristaFiltros } from '../../../types';

// Componentes reutilizáveis
import {
  ModalConfirmacao,
  AlertaErro,
  DataTable,
  CabecalhoPagina,
  type ColumnConfig
} from '../../../components/common';
import { formatarCPF, formatarTelefone } from '../../../utils/formatters';

export default function MotoristasPage() {
  const navigate = useNavigate();

  // Estados
  const [data, setData] = useState<PagedResult<MotoristaListDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Filtros
  const [filtroBusca, setFiltroBusca] = useState('');
  const [filtroUF, setFiltroUF] = useState<string | undefined>(undefined);

  // Paginação
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);

  // Modal de exclusão (se necessário, mas seguindo o layout de produtos que tem inativação)
  const [deleteModal, setDeleteModal] = useState<{ open: boolean; id: number; nome: string; deleting: boolean }>({
    open: false,
    id: 0,
    nome: '',
    deleting: false,
  });

  // Carregar dados
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const filtro: MotoristaFiltros = {
        pagina: pageNumber,
        tamanhoPagina: pageSize,
        busca: filtroBusca || undefined,
        uf: filtroUF || undefined,
      };

      const result = await motoristaService.listar(filtro);
      setData(result);
    } catch (err: any) {
      console.error('Erro ao carregar motoristas:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar motoristas');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, [pageNumber, pageSize, filtroBusca, filtroUF]);

  const handleView = (id: number) => {
    navigate(`/transporte/motoristas/${id}/visualizar`);
  };

  const handleEdit = (id: number) => {
    navigate(`/transporte/motoristas/${id}/editar`);
  };

  const handleNew = () => {
    navigate('/transporte/motoristas/novo');
  };

  const handleDeleteClick = (id: number, nome: string) => {
    setDeleteModal({ open: true, id, nome, deleting: false });
  };

  const handleDeleteConfirm = async () => {
    setDeleteModal((prev) => ({ ...prev, deleting: true }));
    try {
      await motoristaService.excluir(deleteModal.id);
      setDeleteModal({ open: false, id: 0, nome: '', deleting: false });
      await loadData();
    } catch (err: any) {
      console.error('Erro ao excluir:', err);
      setError(err.response?.data?.mensagem || 'Erro ao excluir motorista');
      setDeleteModal((prev) => ({ ...prev, deleting: false }));
    }
  };

  // Definição das colunas
  const columns: ColumnConfig<MotoristaListDto>[] = [
    {
      key: 'codigoDoMotorista',
      header: 'Código',
      width: '100px',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar código...',
      render: (item) => (
        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
          #{item.codigoDoMotorista}
        </span>
      )
    },
    {
      key: 'nomeDoMotorista',
      header: 'Motorista',
      sortable: true,
      render: (item) => (
        <div className="flex flex-col gap-0.5">
          <span className="font-bold text-foreground leading-tight">{item.nomeDoMotorista}</span>
          <div className="flex items-center gap-3">
            <span className="text-[11px] text-muted-foreground flex items-center gap-1 font-medium">
              <User className="h-3 w-3 opacity-70" />
              {formatarCPF(item.cpf)}
            </span>
            {item.rg && (
              <span className="text-[11px] text-muted-foreground flex items-center gap-1 font-medium">
                RG: {item.rg}
              </span>
            )}
          </div>
        </div>
      )
    },
    {
      key: 'cel',
      header: 'Contato',
      render: (item) => (
        <div className="flex items-center gap-2">
          <div className="p-1.5 bg-emerald-50 text-emerald-600 rounded-lg">
            <Phone className="h-3.5 w-3.5" />
          </div>
          <span className="text-sm font-medium text-foreground">
            {formatarTelefone(item.cel) || 'Não informado'}
          </span>
        </div>
      )
    },
    {
      key: 'uf',
      header: 'UF',
      align: 'center',
      render: (item) => (
        <div className="flex items-center justify-center gap-1.5">
          <MapPin className="h-3.5 w-3.5 text-muted-foreground" />
          <span className="text-sm font-bold text-foreground">{item.uf}</span>
        </div>
      )
    }
  ];

  return (
    <div className="space-y-6 pb-8">
      <CabecalhoPagina
        titulo="Motoristas"
        subtitulo="Gerenciamento de motoristas e condutores"
        icone={User}
        acoes={
          <button
            onClick={handleNew}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all font-medium shadow-lg shadow-blue-600/25 active:scale-95"
          >
            <Plus className="w-5 h-5" />
            <span>Novo Motorista</span>
          </button>
        }
      />

      <div className="px-6">
        {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

        <DataTable
          data={data?.items || []}
          columns={columns}
          getRowKey={(item) => item.codigoDoMotorista}
          loading={loading}
          totalItems={data?.totalCount}
          
          // Filtros Server-Side
          onFilterChange={(_, value) => {
            setFiltroBusca(value);
            setPageNumber(1);
          }}
          onClearFilters={() => {
            setFiltroBusca('');
            setFiltroUF(undefined);
            setPageNumber(1);
          }}
          headerExtra={
            <div className="flex flex-wrap items-center gap-2">
              <div className="flex items-center bg-surface border border-border p-1 rounded-xl shadow-sm">
                <button
                  onClick={() => {
                    setFiltroUF(undefined);
                    setPageNumber(1);
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all ${!filtroUF
                    ? 'bg-blue-600 text-white shadow-md shadow-blue-600/20'
                    : 'text-muted-foreground hover:text-foreground hover:bg-surface-hover'
                    }`}
                >
                  Todos
                </button>
                <button
                  onClick={() => {
                    setFiltroUF('SP');
                    setPageNumber(1);
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all ${filtroUF === 'SP'
                    ? 'bg-blue-600 text-white shadow-md shadow-blue-600/20'
                    : 'text-muted-foreground hover:text-foreground hover:bg-surface-hover'
                    }`}
                >
                  São Paulo
                </button>
              </div>

              <button
                onClick={() => loadData()}
                className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-xl transition-all"
                title="Atualizar lista"
              >
                <RefreshCw className={`w-5 h-5 ${loading ? 'animate-spin' : ''}`} />
              </button>
            </div>
          }
          // Ações de Linha
          rowActions={(item) => (
            <div className="flex items-center justify-end gap-2">
              <button
                onClick={() => handleView(item.codigoDoMotorista)}
                className="p-2 text-blue-600 hover:bg-blue-50 rounded-xl transition-all border border-transparent hover:border-blue-100"
                title="Visualizar"
              >
                <Eye className="h-4 w-4" />
              </button>
              <button
                onClick={() => handleEdit(item.codigoDoMotorista)}
                className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-xl transition-all border border-transparent hover:border-emerald-100"
                title="Editar"
              >
                <Edit2 className="h-4 w-4" />
              </button>
              <button
                onClick={() => handleDeleteClick(item.codigoDoMotorista, item.nomeDoMotorista)}
                className="p-2 text-red-600 hover:bg-red-50 rounded-xl transition-all border border-transparent hover:border-red-100"
                title="Excluir"
              >
                <Trash2 className="h-4 w-4" />
              </button>
            </div>
          )}
        />

        {/* Paginação */}
        {!loading && data && data.totalPages > 0 && (
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-border mt-4">
            <div className="flex items-center gap-4">
              <div className="text-sm text-muted-foreground">
                Página <span className="font-medium text-primary">{pageNumber}</span> de <span className="font-medium text-primary">{data.totalPages}</span>
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
                onClick={() => setPageNumber(prev => Math.min(data.totalPages, prev + 1))}
                disabled={pageNumber === data.totalPages}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Próxima
              </button>
            </div>
          </div>
        )}
      </div>

      <ModalConfirmacao
        aberto={deleteModal.open}
        titulo="Excluir Motorista"
        mensagem={`Deseja realmente excluir o motorista "${deleteModal.nome}"? Esta ação não poderá ser desfeita.`}
        textoBotaoConfirmar="Excluir"
        textoBotaoCancelar="Cancelar"
        variante="danger"
        processando={deleteModal.deleting}
        onConfirmar={handleDeleteConfirm}
        onCancelar={() => setDeleteModal({ open: false, id: 0, nome: '', deleting: false })}
      />
    </div>
  );
}
