import { useEffect, useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import {
  ArrowLeft, Save, Loader2, Wrench, XCircle, Plus, Trash2, DollarSign, Settings, Package
} from 'lucide-react';
import { manutencaoService } from '../../../services/Transporte/manutencaoService';
import { veiculoService } from '../../../services/Transporte/veiculoService';
import type {
  ManutencaoVeiculoCreateDto, VeiculoListDto, ManutencaoPecaCreateDto
} from '../../../types';
import { TIPOS_MANUTENCAO } from '../../../types';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import { AlertaErro, AlertaSucesso, EstadoCarregando } from '../../../components/common';

export default function ManutencaoFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const { podeIncluir, podeAlterar, carregando: carregandoPermissoes } = usePermissaoTela('ManutencaoVeiculo');

  // Detectar modo pela URL
  const isEditing = id && id !== 'nova';
  const isViewMode = location.pathname.includes('/visualizar');
  const modo = isViewMode ? 'visualizar' : isEditing ? 'editar' : 'criar';

  const [loading, setLoading] = useState(modo !== 'criar');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const [veiculos, setVeiculos] = useState<VeiculoListDto[]>([]);

  const [formData, setFormData] = useState<ManutencaoVeiculoCreateDto>({
    veiculoId: 0,
    fornecedorId: undefined,
    dataManutencao: new Date().toISOString().split('T')[0],
    tipoManutencao: 'Preventiva',
    descricaoServico: '',
    kmAtual: undefined,
    valorMaoObra: undefined,
    valorServicosTerceiros: undefined,
    numeroOS: '',
    numeroNF: '',
    dataProximaManutencao: undefined,
    kmProximaManutencao: undefined,
    observacoes: '',
    pecas: [],
  });

  const [pecas, setPecas] = useState<ManutencaoPecaCreateDto[]>([]);

  const somenteVisualizacao = modo === 'visualizar';
  const titulo = modo === 'criar' ? 'Nova Manutenção' : modo === 'editar' ? 'Editar Manutenção' : 'Detalhes da Manutenção';

  useEffect(() => {
    loadVeiculos();
    if (id && id !== 'nova') {
      loadManutencao(Number(id));
    }
  }, [id]);

  const loadVeiculos = async () => {
    try {
      const lista = await veiculoService.listarAtivos();
      setVeiculos(lista);
    } catch (err) {
      console.error('Erro ao carregar veículos:', err);
    }
  };

  const loadManutencao = async (manutencaoId: number) => {
    try {
      setLoading(true);
      const manutencao = await manutencaoService.buscarPorId(manutencaoId);
      setFormData({
        veiculoId: manutencao.veiculoId,
        fornecedorId: manutencao.fornecedorId,
        dataManutencao: manutencao.dataManutencao.split('T')[0],
        tipoManutencao: manutencao.tipoManutencao || 'Preventiva',
        descricaoServico: manutencao.descricaoServico || '',
        kmAtual: manutencao.kmAtual,
        valorMaoObra: manutencao.valorMaoObra,
        valorServicosTerceiros: manutencao.valorServicosTerceiros,
        numeroOS: manutencao.numeroOS || '',
        numeroNF: manutencao.numeroNF || '',
        dataProximaManutencao: manutencao.dataProximaManutencao?.split('T')[0],
        kmProximaManutencao: manutencao.kmProximaManutencao,
        observacoes: manutencao.observacoes || '',
      });
      setPecas(manutencao.pecas.map(p => ({
        codigoPeca: p.codigoPeca,
        descricaoPeca: p.descricaoPeca,
        quantidade: p.quantidade,
        valorUnitario: p.valorUnitario,
      })));
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao carregar manutenção');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    if (type === 'number') {
      setFormData((prev) => ({ ...prev, [name]: value ? Number(value) : undefined }));
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  // Peças
  const addPeca = () => {
    setPecas([...pecas, {
      codigoPeca: '',
      descricaoPeca: '',
      quantidade: 1,
      valorUnitario: 0,
    }]);
  };

  const removePeca = (index: number) => {
    setPecas(pecas.filter((_, i) => i !== index));
  };

  const updatePeca = (index: number, field: keyof ManutencaoPecaCreateDto, value: any) => {
    const updated = [...pecas];
    updated[index] = { ...updated[index], [field]: value };
    setPecas(updated);
  };

  const totalPecas = pecas.reduce((sum, p) => sum + (p.quantidade * p.valorUnitario), 0);
  const valorMaoObra = formData.valorMaoObra || 0;
  const valorTerceiros = formData.valorServicosTerceiros || 0;
  const valorTotal = totalPecas + valorMaoObra + valorTerceiros;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.veiculoId) { setError('Selecione um veículo'); return; }

    try {
      setSaving(true);
      setError(null);
      const payload = { ...formData, pecas };

      if (modo === 'criar') {
        await manutencaoService.criar(payload);
        setSuccess('Manutenção cadastrada com sucesso!');
      } else {
        await manutencaoService.atualizar(Number(id), payload);
        setSuccess('Manutenção atualizada com sucesso!');
      }
      setTimeout(() => navigate('/transporte/manutencoes'), 1500);
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao salvar manutenção');
    } finally {
      setSaving(false);
    }
  };

  if (carregandoPermissoes || loading) return <EstadoCarregando mensagem="Carregando..." />;

  if ((modo === 'criar' && !podeIncluir) || (modo === 'editar' && !podeAlterar)) {
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
      <div className="flex items-center gap-4">
        <button onClick={() => navigate('/transporte/manutencoes')} className="p-2 text-muted-foreground hover:text-primary/80 hover:bg-gray-100 rounded-lg">
          <ArrowLeft className="w-5 h-5" />
        </button>
        <h1 className="text-2xl font-bold text-primary flex items-center gap-2">
          <Wrench className="w-7 h-7 text-purple-600" /> {titulo}
        </h1>
      </div>

      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}
      {success && <AlertaSucesso mensagem={success} />}

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Dados Principais */}
        <div className="bg-surface rounded-xl shadow-sm border border-border p-6">
          <h2 className="text-lg font-semibold text-primary mb-4 flex items-center gap-2 pb-3 border-b border-border">
            <Settings className="w-5 h-5 text-purple-500" />
            Dados da Manutenção
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Veículo <span className="text-red-500">*</span></label>
              <select name="veiculoId" value={formData.veiculoId} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover">
                <option value={0}>Selecione...</option>
                {Array.isArray(veiculos) && veiculos.map((v) => (<option key={v.id} value={v.id}>{v.placa} - {v.marca} {v.modelo}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Data Manutenção</label>
              <input type="date" name="dataManutencao" value={formData.dataManutencao} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Tipo</label>
              <select name="tipoManutencao" value={formData.tipoManutencao} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover">
                {TIPOS_MANUTENCAO.map((t) => (<option key={t} value={t}>{t}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">KM Atual</label>
              <input type="number" name="kmAtual" value={formData.kmAtual || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Fornecedor (Código)</label>
              <input type="number" name="fornecedorId" value={formData.fornecedorId || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover"
                placeholder="Código do fornecedor" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Número OS</label>
              <input type="text" name="numeroOS" value={formData.numeroOS} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Número NF</label>
              <input type="text" name="numeroNF" value={formData.numeroNF} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Valor Mão de Obra (R$)</label>
              <input type="number" name="valorMaoObra" value={formData.valorMaoObra || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" step="0.01" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Valor Serv. Terceiros (R$)</label>
              <input type="number" name="valorServicosTerceiros" value={formData.valorServicosTerceiros || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" step="0.01" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Próxima Manutenção</label>
              <input type="date" name="dataProximaManutencao" value={formData.dataProximaManutencao || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">KM Próxima Manutenção</label>
              <input type="number" name="kmProximaManutencao" value={formData.kmProximaManutencao || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
            </div>
          </div>

          <div className="mt-4">
            <label className="block text-sm font-medium text-primary/80 mb-1">Descrição do Serviço</label>
            <textarea name="descricaoServico" value={formData.descricaoServico} onChange={handleChange} disabled={somenteVisualizacao} rows={3}
              className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
          </div>

          <div className="mt-4">
            <label className="block text-sm font-medium text-primary/80 mb-1">Observações</label>
            <textarea name="observacoes" value={formData.observacoes} onChange={handleChange} disabled={somenteVisualizacao} rows={2}
              className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
          </div>
        </div>

        {/* Peças */}
        <div className="bg-surface rounded-xl shadow-sm border border-border p-6">
          <div className="flex items-center justify-between mb-4 pb-3 border-b border-border">
            <h2 className="text-lg font-semibold text-primary flex items-center gap-2">
              <Package className="w-5 h-5 text-amber-500" /> Peças Utilizadas
            </h2>
            {!somenteVisualizacao && (
              <button type="button" onClick={addPeca} className="inline-flex items-center gap-1 px-3 py-1 text-sm bg-purple-50 text-purple-600 rounded-lg hover:bg-purple-100">
                <Plus className="w-4 h-4" /> Adicionar Peça
              </button>
            )}
          </div>

          {pecas.length === 0 ? (
            <p className="text-muted-foreground text-center py-4">Nenhuma peça cadastrada</p>
          ) : (
            <div className="space-y-3">
              {pecas.map((peca, index) => (
                <div key={index} className="grid grid-cols-1 md:grid-cols-6 gap-3 p-3 bg-surface-hover rounded-lg items-end">
                  <div>
                    <label className="block text-xs text-muted-foreground mb-1">Código</label>
                    <input type="text" placeholder="Código" value={peca.codigoPeca || ''} onChange={(e) => updatePeca(index, 'codigoPeca', e.target.value)} disabled={somenteVisualizacao}
                      className="w-full px-2 py-1 border border-input rounded text-sm focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-xs text-muted-foreground mb-1">Descrição</label>
                    <input type="text" placeholder="Descrição da peça" value={peca.descricaoPeca} onChange={(e) => updatePeca(index, 'descricaoPeca', e.target.value)} disabled={somenteVisualizacao}
                      className="w-full px-2 py-1 border border-input rounded text-sm focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" />
                  </div>
                  <div>
                    <label className="block text-xs text-muted-foreground mb-1">Qtde</label>
                    <input type="number" placeholder="Qtd" value={peca.quantidade} onChange={(e) => updatePeca(index, 'quantidade', Number(e.target.value))} disabled={somenteVisualizacao}
                      className="w-full px-2 py-1 border border-input rounded text-sm focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" min={1} />
                  </div>
                  <div>
                    <label className="block text-xs text-muted-foreground mb-1">Valor Unit.</label>
                    <input type="number" placeholder="Valor" value={peca.valorUnitario} onChange={(e) => updatePeca(index, 'valorUnitario', Number(e.target.value))} disabled={somenteVisualizacao}
                      className="w-full px-2 py-1 border border-input rounded text-sm focus:ring-2 focus:ring-purple-500 disabled:bg-surface-hover" step="0.01" />
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="text-sm font-semibold text-primary/80">
                      R$ {(peca.quantidade * peca.valorUnitario).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                    </span>
                    {!somenteVisualizacao && (
                      <button type="button" onClick={() => removePeca(index)} className="p-1 text-red-500 hover:bg-red-100 rounded">
                        <Trash2 className="w-4 h-4" />
                      </button>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}

          <div className="mt-4 text-right text-lg font-semibold text-purple-600">
            Total Peças: R$ {totalPecas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
          </div>
        </div>

        {/* Resumo */}
        <div className="bg-surface rounded-xl shadow-sm border border-border p-6">
          <h2 className="text-lg font-semibold text-primary mb-4 flex items-center gap-2 pb-3 border-b border-border">
            <DollarSign className="w-5 h-5 text-blue-500" /> Resumo de Custos
          </h2>
          <div className="grid grid-cols-4 gap-4 text-center">
            <div className="p-4 bg-surface-hover rounded-lg">
              <p className="text-sm text-gray-600">Peças</p>
              <p className="text-xl font-bold text-primary/80">R$ {totalPecas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
            </div>
            <div className="p-4 bg-surface-hover rounded-lg">
              <p className="text-sm text-gray-600">Mão de Obra</p>
              <p className="text-xl font-bold text-primary/80">R$ {valorMaoObra.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
            </div>
            <div className="p-4 bg-surface-hover rounded-lg">
              <p className="text-sm text-gray-600">Serv. Terceiros</p>
              <p className="text-xl font-bold text-primary/80">R$ {valorTerceiros.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
            </div>
            <div className="p-4 bg-purple-50 rounded-lg">
              <p className="text-sm text-gray-600">Total Geral</p>
              <p className="text-xl font-bold text-purple-600">R$ {valorTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
            </div>
          </div>
        </div>

        {/* Botões */}
        {!somenteVisualizacao && (
          <div className="flex justify-end gap-4">
            <button type="button" onClick={() => navigate('/transporte/manutencoes')} className="px-4 py-2 text-primary/80 bg-gray-100 rounded-lg hover:bg-gray-200">
              Cancelar
            </button>
            <button type="submit" disabled={saving} className="inline-flex items-center gap-2 px-4 py-2 bg-gradient-to-r from-purple-500 to-indigo-600 text-white rounded-lg hover:from-purple-600 hover:to-indigo-700 disabled:opacity-50">
              {saving ? (<><Loader2 className="w-5 h-5 animate-spin" />Salvando...</>) : (<><Save className="w-5 h-5" />Salvar</>)}
            </button>
          </div>
        )}
      </form>
    </div>
  );
}
