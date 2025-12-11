import { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Plus,
  Edit2,
  Trash2,
  User,
  Filter,
  X,
  Eye,
  Search,
  Phone,
} from 'lucide-react';
import { motoristaService } from '../../../services/motoristaService';
import type { MotoristaListDto, PagedResult, MotoristaFiltros } from '../../../types/transporte';
import { UFS_BRASIL } from '../../../types/transporte';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import {
  ModalConfirmacao,
  Paginacao,
  EstadoVazio,
  EstadoCarregando,
  AlertaErro,
} from '../../../components/common';
import { formatarCPF, formatarTelefone } from '../../../utils/formatters';

export default function MotoristasPage() {
  const navigate = useNavigate();
  const { podeIncluir, podeAlterar, podeExcluir, carregando: carregandoPermissoes } = usePermissaoTela('Motorista');

  const [motoristas, setMotoristas] = useState<MotoristaListDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [totalItems, setTotalItems] = useState(0);
  const [totalPages, setTotalPages] = useState(1);

  // Filtros
  const [filtros, setFiltros] = useState<MotoristaFiltros>({
    busca: '',
    uf: '',
    pagina: 1,
    tamanhoPagina: 10,
  });
  const [mostrarFiltros, setMostrarFiltros] = useState(false);
  const [buscaTemp, setBuscaTemp] = useState('');

  // Exclusão
  const [motoristaExcluir, setMotoristaExcluir] = useState<MotoristaListDto | null>(null);
  const [excluindo, setExcluindo] = useState(false);

  const carregarMotoristas = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const resultado: PagedResult<MotoristaListDto> = await motoristaService.listar(filtros);
      setMotoristas(resultado.items);
      setTotalItems(resultado.totalCount);
      setTotalPages(resultado.totalPages);
    } catch (err: any) {
      console.error('Erro ao carregar motoristas:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar motoristas');
    } finally {
      setLoading(false);
    }
  }, [filtros]);

  useEffect(() => {
    carregarMotoristas();
  }, [carregarMotoristas]);

  const handleBuscar = () => {
    setFiltros((prev) => ({ ...prev, busca: buscaTemp, pagina: 1 }));
  };

  const handleLimparFiltros = () => {
    setBuscaTemp('');
    setFiltros({ busca: '', uf: '', pagina: 1, tamanhoPagina: 10 });
  };

  const handleExcluir = async () => {
    if (!motoristaExcluir) return;

    try {
      setExcluindo(true);
      await motoristaService.excluir(motoristaExcluir.codigoDoMotorista);
      setMotoristaExcluir(null);
      carregarMotoristas();
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao excluir motorista');
    } finally {
      setExcluindo(false);
    }
  };

  if (carregandoPermissoes) {
    return <EstadoCarregando mensagem="Verificando permissões..." />;
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-primary flex items-center gap-2">
            <User className="w-7 h-7 text-blue-600" />
            Motoristas
          </h1>
          <p className="text-muted-foreground mt-1">Gerencie os motoristas/condutores do sistema</p>
        </div>
        {podeIncluir && (
          <button
            onClick={() => navigate('/transporte/motoristas/novo')}
            className="inline-flex items-center gap-2 px-4 py-2 bg-gradient-to-r from-blue-600 to-indigo-600 text-white rounded-lg hover:from-blue-700 hover:to-indigo-700 transition-colors"
          >
            <Plus className="w-5 h-5" />
            Novo Motorista
          </button>
        )}
      </div>

      {/* Barra de Busca e Filtros */}
      <div className="bg-surface rounded-xl shadow-sm border border-border p-4">
        <div className="flex flex-col md:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground/70 w-5 h-5" />
            <input
              type="text"
              placeholder="Buscar por nome ou CPF..."
              value={buscaTemp}
              onChange={(e) => setBuscaTemp(e.target.value)}
              onKeyDown={(e) => e.key === 'Enter' && handleBuscar()}
              className="w-full pl-10 pr-4 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
          <div className="flex gap-2">
            <button
              onClick={handleBuscar}
              className="px-4 py-2 bg-gradient-to-r from-blue-600 to-indigo-600 text-white rounded-lg hover:from-blue-700 hover:to-indigo-700"
            >
              Buscar
            </button>
            <button
              onClick={() => setMostrarFiltros(!mostrarFiltros)}
              className={`p-2 rounded-lg border ${mostrarFiltros ? 'bg-blue-100 border-blue-500' : 'border-input hover:bg-surface-hover'}`}
            >
              <Filter className="w-5 h-5" />
            </button>
            {(filtros.busca || filtros.uf) && (
              <button
                onClick={handleLimparFiltros}
                className="p-2 rounded-lg border border-input hover:bg-surface-hover text-muted-foreground"
              >
                <X className="w-5 h-5" />
              </button>
            )}
          </div>
        </div>

        {/* Filtros expandidos */}
        {mostrarFiltros && (
          <div className="mt-4 pt-4 border-t border-border">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">UF</label>
                <select
                  value={filtros.uf || ''}
                  onChange={(e) => setFiltros((prev) => ({ ...prev, uf: e.target.value, pagina: 1 }))}
                  className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">Todas</option>
                  {UFS_BRASIL.map((uf) => (
                    <option key={uf} value={uf}>{uf}</option>
                  ))}
                </select>
              </div>
            </div>
          </div>
        )}
      </div>

      {/* Alerta de Erro */}
      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

      {/* Tabela */}
      <div className="bg-surface rounded-xl shadow-sm border border-border overflow-hidden">
        {loading ? (
          <EstadoCarregando mensagem="Carregando motoristas..." />
        ) : !motoristas || motoristas.length === 0 ? (
          <EstadoVazio
            icone={User}
            titulo="Nenhum motorista encontrado"
            descricao={filtros.busca || filtros.uf ? 'Tente ajustar os filtros' : 'Adicione seu primeiro motorista'}
            acao={podeIncluir ? { texto: 'Novo Motorista', onClick: () => navigate('/transporte/motoristas/novo') } : undefined}
          />
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-surface-hover">
                  <tr>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">Código</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">Nome</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">CPF</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">RG</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">Celular</th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-muted-foreground uppercase">UF</th>
                    <th className="px-4 py-3 text-right text-xs font-medium text-muted-foreground uppercase">Ações</th>
                  </tr>
                </thead>
                <tbody className="bg-surface divide-y divide-gray-200">
                  {motoristas?.map((m) => (
                    <tr key={m.codigoDoMotorista} className="group hover:bg-surface-hover">
                      <td className="px-4 py-3 whitespace-nowrap text-sm text-[var(--text-muted)]">
                        {m.codigoDoMotorista}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-sm text-[var(--text)]">
                        {m.nomeDoMotorista}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-sm text-muted-foreground">
                        {formatarCPF(m.cpf)}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-sm text-muted-foreground">
                        {m.rg || '-'}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-sm text-muted-foreground">
                        <div className="flex items-center gap-1">
                          <Phone className="w-4 h-4 text-muted-foreground/70" />
                          {formatarTelefone(m.cel) || '-'}
                        </div>
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-sm text-muted-foreground">
                        {m.uf || '-'}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-right text-sm">
                        <div className="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                          <button
                            onClick={() => navigate(`/transporte/motoristas/${m.codigoDoMotorista}/visualizar`)}
                            className="p-1.5 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg"
                            title="Visualizar"
                          >
                            <Eye className="w-4 h-4" />
                          </button>
                          {podeAlterar && (
                            <button
                              onClick={() => navigate(`/transporte/motoristas/${m.codigoDoMotorista}/editar`)}
                              className="p-1.5 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg"
                              title="Editar"
                            >
                              <Edit2 className="w-4 h-4" />
                            </button>
                          )}
                          {podeExcluir && (
                            <button
                              onClick={() => setMotoristaExcluir(m)}
                              className="p-1.5 text-muted-foreground hover:text-red-600 hover:bg-red-50 rounded-lg"
                              title="Excluir"
                            >
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

            {/* Paginação */}
            <div className="px-4 py-3 border-t border-border">
              <Paginacao
                paginaAtual={filtros.pagina || 1}
                totalPaginas={totalPages}
                totalItens={totalItems}
                itensPorPagina={filtros.tamanhoPagina || 10}
                onMudarPagina={(p) => setFiltros((prev) => ({ ...prev, pagina: p }))}
              />
            </div>
          </>
        )}
      </div>

      {/* Modal de Confirmação de Exclusão */}
      <ModalConfirmacao
        aberto={!!motoristaExcluir}
        titulo="Excluir Motorista"
        mensagem={`Tem certeza que deseja excluir o motorista "${motoristaExcluir?.nomeDoMotorista}"? Esta ação não pode ser desfeita.`}
        textoBotaoConfirmar="Excluir"
        variante="danger"
        processando={excluindo}
        onConfirmar={handleExcluir}
        onCancelar={() => setMotoristaExcluir(null)}
      />
    </div>
  );
}
