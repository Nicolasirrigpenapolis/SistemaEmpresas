import { useState, useEffect } from 'react';
import { 
  ArrowUpRight, 
  ArrowDownLeft,
  Trash2,
  Edit2,
  RefreshCw,
  FileText,
  User
} from 'lucide-react';
import { movimentoContabilService } from '../../services/MovimentoContabil/movimentoContabilService';
import type { MovimentoContabilNovoDto, MovimentoContabilFiltroDto } from '../../types/Estoque/movimentoContabil';
import type { PagedResult } from '../../types/Common/common';
import { format } from 'date-fns';
import { 
  DataTable, 
  AlertaErro,
  ModalConfirmacao,
  type ColumnConfig
} from '../../components/common';

interface MovimentoContabilListProps {
  onEditar: (id: number) => void;
}

export function MovimentoContabilList({ onEditar }: MovimentoContabilListProps) {
  const [filtro, setFiltro] = useState<MovimentoContabilFiltroDto>({
    pageNumber: 1,
    pageSize: 25
  });
  const [result, setResult] = useState<PagedResult<MovimentoContabilNovoDto> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalExcluir, setModalExcluir] = useState({
    aberto: false,
    id: 0,
    processando: false
  });

  const carregarMovimentos = async () => {
    try {
      setLoading(true);
      const data = await movimentoContabilService.listarMovimentosNovos(filtro);
      setResult(data);
      setError(null);
    } catch (err: any) {
      console.error('Erro ao carregar movimentos:', err);
      setError(err.response?.data?.mensagem || 'Não foi possível carregar a lista de movimentos.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    carregarMovimentos();
  }, [filtro.pageNumber, filtro.pageSize, filtro.documento, filtro.tipoDoMovimento]);

  const handleExcluir = (id: number) => {
    setModalExcluir({ aberto: true, id, processando: false });
  };

  const confirmarExclusao = async () => {
    try {
      setModalExcluir(prev => ({ ...prev, processando: true }));
      await movimentoContabilService.excluirMovimento(modalExcluir.id);
      setModalExcluir({ aberto: false, id: 0, processando: false });
      carregarMovimentos();
    } catch (err: any) {
      setError('Erro ao excluir movimento: ' + (err.response?.data?.mensagem || err.message));
      setModalExcluir(prev => ({ ...prev, processando: false }));
    }
  };

  // Definição das colunas
  const columns: ColumnConfig<MovimentoContabilNovoDto>[] = [
    {
      key: 'sequenciaDoMovimento',
      header: 'ID',
      width: '80px',
      sortable: true,
      render: (item) => (
        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
          #{item.sequenciaDoMovimento}
        </span>
      )
    },
    {
      key: 'dataDoMovimento',
      header: 'Data',
      width: '120px',
      sortable: true,
      render: (item) => (
        <span className="text-sm font-medium text-muted-foreground">
          {format(new Date(item.dataDoMovimento), 'dd/MM/yyyy')}
        </span>
      )
    },
    {
      key: 'documento',
      header: 'Documento',
      width: '180px',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar documento...',
      render: (item) => (
        <div className="flex flex-col gap-0.5">
          <div className="flex items-center gap-2">
            <FileText className="w-3.5 h-3.5 text-blue-600/50" />
            <span className="font-bold text-foreground leading-tight">{item.documento}</span>
          </div>
          {item.observacao && (
            <span className="text-[11px] text-muted-foreground truncate max-w-[200px] italic ml-5">
              {item.observacao}
            </span>
          )}
        </div>
      )
    },
    {
      key: 'tipoDoMovimento',
      header: 'Tipo',
      width: '140px',
      sortable: true,
      render: (item) => (
        <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-[10px] font-bold border uppercase tracking-wider ${
          item.tipoDoMovimento === 0 
            ? 'bg-emerald-50 text-emerald-700 border-emerald-200' 
            : 'bg-amber-50 text-amber-700 border-amber-200'
        }`}>
          {item.tipoDoMovimento === 0 ? (
            <><ArrowDownLeft className="w-3 h-3" /> Entrada</>
          ) : (
            <><ArrowUpRight className="w-3 h-3" /> Saída</>
          )}
        </span>
      )
    },
    {
      key: 'razaoSocialGeral',
      header: 'Geral / Fornecedor',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar geral...',
      render: (item) => (
        <div className="flex items-center gap-2">
          <div className="p-1 bg-gray-100 rounded-md">
            <User className="w-3 h-3 text-gray-500" />
          </div>
          <span className="font-bold text-foreground leading-tight uppercase text-xs">
            {item.razaoSocialGeral || 'NÃO INFORMADO'}
          </span>
        </div>
      )
    },
    {
      key: 'valorTotalDoMovimento',
      header: 'Valor Total',
      width: '140px',
      align: 'right',
      sortable: true,
      render: (item) => (
        <span className="font-bold text-foreground">
          {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.valorTotalDoMovimento)}
        </span>
      )
    }
  ];

  return (
    <div className="space-y-4">
      {error && (
        <AlertaErro 
          titulo="Erro ao carregar dados"
          mensagem={error}
        />
      )}

      <div className="bg-surface rounded-2xl shadow-sm border border-border p-6">
        <DataTable 
          columns={columns} 
          data={result?.items || []} 
          getRowKey={(item) => item.sequenciaDoMovimento}
          loading={loading}
          totalItems={result?.totalCount || 0}
          onFilterChange={(_, value) => {
            setFiltro({ ...filtro, documento: value, pageNumber: 1 });
          }}
          onClearFilters={() => {
            setFiltro({ pageNumber: 1, pageSize: 25, documento: undefined, tipoDoMovimento: undefined });
          }}
          headerExtra={
            <div className="flex flex-wrap items-center gap-2">
              <div className="flex items-center bg-surface border border-border p-1 rounded-xl shadow-sm">
                <button
                  onClick={() => {
                    setFiltro({ ...filtro, tipoDoMovimento: undefined, pageNumber: 1 });
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all ${filtro.tipoDoMovimento === undefined
                    ? 'bg-blue-600 text-white shadow-md shadow-blue-600/20'
                    : 'text-muted-foreground hover:text-foreground hover:bg-surface-hover'
                    }`}
                >
                  Todos
                </button>

                <button
                  onClick={() => {
                    setFiltro({ ...filtro, tipoDoMovimento: 0, pageNumber: 1 });
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all flex items-center gap-2 ${filtro.tipoDoMovimento === 0
                    ? 'bg-emerald-600 text-white shadow-md shadow-emerald-600/20'
                    : 'text-muted-foreground hover:text-emerald-600 hover:bg-emerald-50'
                    }`}
                >
                  <ArrowDownLeft className="w-3.5 h-3.5" />
                  Entradas
                </button>

                <button
                  onClick={() => {
                    setFiltro({ ...filtro, tipoDoMovimento: 1, pageNumber: 1 });
                  }}
                  className={`px-4 py-1.5 rounded-lg text-xs font-bold transition-all flex items-center gap-2 ${filtro.tipoDoMovimento === 1
                    ? 'bg-amber-600 text-white shadow-md shadow-amber-600/20'
                    : 'text-muted-foreground hover:text-amber-600 hover:bg-amber-50'
                    }`}
                >
                  <ArrowUpRight className="w-3.5 h-3.5" />
                  Saídas
                </button>
              </div>

              <div className="w-px h-6 bg-border mx-1" />

              <button 
                onClick={() => carregarMovimentos()}
                className="p-2 text-muted-foreground hover:text-primary hover:bg-primary/5 rounded-xl transition-all border border-transparent hover:border-primary/10"
                title="Atualizar"
              >
                <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
              </button>
            </div>
          }
          rowActions={(item) => (
            <div className="flex items-center justify-end gap-2">
              <button
                onClick={() => onEditar(item.sequenciaDoMovimento)}
                className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-xl transition-all border border-transparent hover:border-emerald-100"
                title="Editar"
              >
                <Edit2 className="w-4 h-4" />
              </button>
              <button
                onClick={() => handleExcluir(item.sequenciaDoMovimento)}
                className="p-2 text-red-600 hover:bg-red-50 rounded-xl transition-all border border-transparent hover:border-red-100"
                title="Excluir"
              >
                <Trash2 className="w-4 h-4" />
              </button>
            </div>
          )}
        />
        
        {/* Paginação Estilo ProdutosPage */}
        {result && result.totalPages > 0 && (
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-border mt-4">
            <div className="flex items-center gap-4">
              <div className="text-sm text-muted-foreground">
                Página <span className="font-bold text-primary">{filtro.pageNumber}</span> de <span className="font-bold text-primary">{result.totalPages}</span>
              </div>
              <select
                value={filtro.pageSize}
                onChange={(e) => {
                  setFiltro({ ...filtro, pageSize: Number(e.target.value), pageNumber: 1 });
                }}
                className="px-3 py-1.5 text-sm border border-border rounded-xl bg-surface text-foreground focus:ring-2 focus:ring-primary/20 focus:border-primary outline-none transition-all"
              >
                <option value={10}>10 por página</option>
                <option value={25}>25 por página</option>
                <option value={50}>50 por página</option>
                <option value={100}>100 por página</option>
              </select>
            </div>

            <div className="flex items-center gap-2">
              <button
                onClick={() => setFiltro(prev => ({ ...prev, pageNumber: Math.max(1, (prev.pageNumber || 1) - 1) }))}
                disabled={filtro.pageNumber === 1}
                className="px-4 py-2 text-sm font-bold border border-border rounded-xl hover:bg-surface-hover hover:border-primary hover:text-primary disabled:opacity-50 disabled:cursor-not-allowed transition-all bg-surface shadow-sm active:scale-95"
              >
                Anterior
              </button>
              <button
                onClick={() => setFiltro(prev => ({ ...prev, pageNumber: Math.min(result.totalPages, (prev.pageNumber || 1) + 1) }))}
                disabled={filtro.pageNumber === result.totalPages}
                className="px-4 py-2 text-sm font-bold border border-border rounded-xl hover:bg-surface-hover hover:border-primary hover:text-primary disabled:opacity-50 disabled:cursor-not-allowed transition-all bg-surface shadow-sm active:scale-95"
              >
                Próxima
              </button>
            </div>
          </div>
        )}
      </div>

      <ModalConfirmacao
        aberto={modalExcluir.aberto}
        titulo="Excluir Movimento"
        mensagem="Tem certeza que deseja excluir este movimento? O estoque será revertido."
        processando={modalExcluir.processando}
        onConfirmar={confirmarExclusao}
        onCancelar={() => setModalExcluir({ aberto: false, id: 0, processando: false })}
      />
    </div>
  );
}
