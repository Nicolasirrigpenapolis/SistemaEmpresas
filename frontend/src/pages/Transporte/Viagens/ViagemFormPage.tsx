import { useEffect, useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import {
  ArrowLeft, Save, Loader2, Route, XCircle, Plus, Trash2,
  DollarSign, TrendingUp, TrendingDown
} from 'lucide-react';
import { viagemService } from '../../../services/viagemService';
import { veiculoService } from '../../../services/veiculoService';
import { reboqueService } from '../../../services/reboqueService';
import { motoristaService } from '../../../services/motoristaService';
import type {
  ViagemCreateDto, VeiculoListDto, ReboqueListDto, MotoristaListDto,
  DespesaViagemCreateDto, ReceitaViagemCreateDto
} from '../../../types/transporte';
import { STATUS_VIAGEM, TIPOS_DESPESA, TIPOS_RECEITA, UFS_BRASIL } from '../../../types/transporte';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import { AlertaErro, AlertaSucesso, EstadoCarregando } from '../../../components/common';

export default function ViagemFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const { podeIncluir, podeAlterar, carregando: carregandoPermissoes } = usePermissaoTela('Viagem');

  // Detectar modo pela URL
  const isEditing = id && id !== 'nova';
  const isViewMode = location.pathname.includes('/visualizar');
  const modo = isViewMode ? 'visualizar' : isEditing ? 'editar' : 'criar';

  const [loading, setLoading] = useState(modo !== 'criar');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const [veiculos, setVeiculos] = useState<VeiculoListDto[]>([]);
  const [reboques, setReboques] = useState<ReboqueListDto[]>([]);
  const [motoristas, setMotoristas] = useState<MotoristaListDto[]>([]);

  const [formData, setFormData] = useState<ViagemCreateDto>({
    veiculoId: 0,
    reboqueId: undefined,
    motoristaId: 0,
    dataPartida: new Date().toISOString().split('T')[0],
    dataChegada: undefined,
    origemCidade: '',
    origemUf: 'SP',
    destinoCidade: '',
    destinoUf: 'SP',
    kmSaida: undefined,
    kmChegada: undefined,
    numeroCte: '',
    valorFrete: undefined,
    status: 'Planejada',
    observacoes: '',
    despesas: [],
    receitas: [],
  });

  const [despesas, setDespesas] = useState<DespesaViagemCreateDto[]>([]);
  const [receitas, setReceitas] = useState<ReceitaViagemCreateDto[]>([]);

  const somenteVisualizacao = modo === 'visualizar';
  const titulo = modo === 'criar' ? 'Nova Viagem' : modo === 'editar' ? 'Editar Viagem' : 'Detalhes da Viagem';

  useEffect(() => {
    loadCombos();
    if (id && id !== 'nova') {
      loadViagem(Number(id));
    }
  }, [id]);

  const loadCombos = async () => {
    try {
      const [veiculosRes, reboquesRes, motoristasRes] = await Promise.all([
        veiculoService.listarAtivos(),
        reboqueService.listarAtivos(),
        motoristaService.listarAtivos(),
      ]);
      setVeiculos(veiculosRes);
      setReboques(reboquesRes);
      setMotoristas(motoristasRes);
    } catch (err) {
      console.error('Erro ao carregar combos:', err);
    }
  };

  const loadViagem = async (viagemId: number) => {
    try {
      setLoading(true);
      const viagem = await viagemService.buscarPorId(viagemId);
      setFormData({
        veiculoId: viagem.veiculoId,
        reboqueId: viagem.reboqueId,
        motoristaId: viagem.motoristaId,
        dataPartida: viagem.dataPartida.split('T')[0],
        dataChegada: viagem.dataChegada?.split('T')[0],
        origemCidade: viagem.origemCidade || '',
        origemUf: viagem.origemUf || 'SP',
        destinoCidade: viagem.destinoCidade || '',
        destinoUf: viagem.destinoUf || 'SP',
        kmSaida: viagem.kmSaida,
        kmChegada: viagem.kmChegada,
        numeroCte: viagem.numeroCte || '',
        valorFrete: viagem.valorFrete,
        status: viagem.status,
        observacoes: viagem.observacoes || '',
      });
      setDespesas(viagem.despesas.map(d => ({
        tipoDespesa: d.tipoDespesa,
        descricao: d.descricao,
        valor: d.valor,
        data: d.data.split('T')[0],
        fornecedor: d.fornecedor,
        numeroDocumento: d.numeroDocumento,
        observacoes: d.observacoes,
      })));
      setReceitas(viagem.receitas.map(r => ({
        tipoReceita: r.tipoReceita,
        descricao: r.descricao,
        valor: r.valor,
        data: r.data.split('T')[0],
        cliente: r.cliente,
        numeroDocumento: r.numeroDocumento,
        observacoes: r.observacoes,
      })));
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao carregar viagem');
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

  // Despesas
  const addDespesa = () => {
    setDespesas([...despesas, {
      tipoDespesa: 'Combustível',
      descricao: '',
      valor: 0,
      data: new Date().toISOString().split('T')[0],
      fornecedor: '',
      numeroDocumento: '',
      observacoes: '',
    }]);
  };

  const removeDespesa = (index: number) => {
    setDespesas(despesas.filter((_, i) => i !== index));
  };

  const updateDespesa = (index: number, field: keyof DespesaViagemCreateDto, value: any) => {
    const updated = [...despesas];
    updated[index] = { ...updated[index], [field]: value };
    setDespesas(updated);
  };

  // Receitas
  const addReceita = () => {
    setReceitas([...receitas, {
      tipoReceita: 'Frete',
      descricao: '',
      valor: 0,
      data: new Date().toISOString().split('T')[0],
      cliente: '',
      numeroDocumento: '',
      observacoes: '',
    }]);
  };

  const removeReceita = (index: number) => {
    setReceitas(receitas.filter((_, i) => i !== index));
  };

  const updateReceita = (index: number, field: keyof ReceitaViagemCreateDto, value: any) => {
    const updated = [...receitas];
    updated[index] = { ...updated[index], [field]: value };
    setReceitas(updated);
  };

  const totalDespesas = despesas.reduce((sum, d) => sum + (d.valor || 0), 0);
  const totalReceitas = receitas.reduce((sum, r) => sum + (r.valor || 0), 0);
  const saldo = totalReceitas - totalDespesas;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.veiculoId) { setError('Selecione um veículo'); return; }
    if (!formData.motoristaId) { setError('Selecione um motorista'); return; }

    try {
      setSaving(true);
      setError(null);
      const payload = { ...formData, despesas, receitas };

      if (modo === 'criar') {
        await viagemService.criar(payload);
        setSuccess('Viagem cadastrada com sucesso!');
      } else {
        await viagemService.atualizar(Number(id), payload);
        setSuccess('Viagem atualizada com sucesso!');
      }
      setTimeout(() => navigate('/transporte/viagens'), 1500);
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao salvar viagem');
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
          <h2 className="text-xl font-semibold text-gray-700">Acesso Negado</h2>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center gap-4">
        <button onClick={() => navigate('/transporte/viagens')} className="p-2 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-lg">
          <ArrowLeft className="w-5 h-5" />
        </button>
        <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
          <Route className="w-7 h-7 text-green-600" /> {titulo}
        </h1>
      </div>

      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}
      {success && <AlertaSucesso mensagem={success} />}

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Dados Principais */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">Dados da Viagem</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Veículo <span className="text-red-500">*</span></label>
              <select name="veiculoId" value={formData.veiculoId} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                <option value={0}>Selecione...</option>
                {Array.isArray(veiculos) && veiculos.map((v) => (<option key={v.id} value={v.id}>{v.placa} - {v.marca} {v.modelo}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Reboque</label>
              <select name="reboqueId" value={formData.reboqueId || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                <option value="">Nenhum</option>
                {Array.isArray(reboques) && reboques.map((r) => (<option key={r.id} value={r.id}>{r.placa}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Motorista <span className="text-red-500">*</span></label>
              <select name="motoristaId" value={formData.motoristaId || 0} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                <option value={0}>Selecione...</option>
                {Array.isArray(motoristas) && motoristas.map((m) => (<option key={m.codigoDoMotorista} value={m.codigoDoMotorista}>{m.nomeDoMotorista}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Status</label>
              <select name="status" value={formData.status} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                {STATUS_VIAGEM.map((s) => (<option key={s} value={s}>{s}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Data Partida</label>
              <input type="date" name="dataPartida" value={formData.dataPartida} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Data Chegada</label>
              <input type="date" name="dataChegada" value={formData.dataChegada || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">KM Saída</label>
              <input type="number" name="kmSaida" value={formData.kmSaida || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">KM Chegada</label>
              <input type="number" name="kmChegada" value={formData.kmChegada || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
          </div>

          {/* Origem/Destino */}
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mt-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Cidade Origem</label>
              <input type="text" name="origemCidade" value={formData.origemCidade} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">UF Origem</label>
              <select name="origemUf" value={formData.origemUf} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                {UFS_BRASIL.map((uf) => (<option key={uf} value={uf}>{uf}</option>))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Cidade Destino</label>
              <input type="text" name="destinoCidade" value={formData.destinoCidade} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">UF Destino</label>
              <select name="destinoUf" value={formData.destinoUf} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                {UFS_BRASIL.map((uf) => (<option key={uf} value={uf}>{uf}</option>))}
              </select>
            </div>
          </div>

          {/* CT-e e Frete */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mt-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Número CT-e</label>
              <input type="text" name="numeroCte" value={formData.numeroCte} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Valor Frete (R$)</label>
              <input type="number" name="valorFrete" value={formData.valorFrete || ''} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" step="0.01" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Observações</label>
              <input type="text" name="observacoes" value={formData.observacoes} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
            </div>
          </div>
        </div>

        {/* Despesas */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold text-gray-900 flex items-center gap-2">
              <TrendingDown className="w-5 h-5 text-red-500" /> Despesas
            </h2>
            {!somenteVisualizacao && (
              <button type="button" onClick={addDespesa} className="inline-flex items-center gap-1 px-3 py-1 text-sm bg-red-50 text-red-600 rounded-lg hover:bg-red-100">
                <Plus className="w-4 h-4" /> Adicionar
              </button>
            )}
          </div>
          {despesas.length === 0 ? (
            <p className="text-gray-500 text-center py-4">Nenhuma despesa cadastrada</p>
          ) : (
            <div className="space-y-3">
              {despesas.map((desp, index) => (
                <div key={index} className="grid grid-cols-1 md:grid-cols-6 gap-3 p-3 bg-gray-50 rounded-lg">
                  <select value={desp.tipoDespesa} onChange={(e) => updateDespesa(index, 'tipoDespesa', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                    {TIPOS_DESPESA.map((t) => (<option key={t} value={t}>{t}</option>))}
                  </select>
                  <input type="text" placeholder="Descrição" value={desp.descricao || ''} onChange={(e) => updateDespesa(index, 'descricao', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
                  <input type="number" placeholder="Valor" value={desp.valor || ''} onChange={(e) => updateDespesa(index, 'valor', Number(e.target.value))} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" step="0.01" />
                  <input type="date" value={desp.data} onChange={(e) => updateDespesa(index, 'data', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
                  <input type="text" placeholder="Fornecedor" value={desp.fornecedor || ''} onChange={(e) => updateDespesa(index, 'fornecedor', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
                  {!somenteVisualizacao && (
                    <button type="button" onClick={() => removeDespesa(index)} className="p-1 text-red-500 hover:bg-red-100 rounded">
                      <Trash2 className="w-4 h-4" />
                    </button>
                  )}
                </div>
              ))}
            </div>
          )}
          <div className="mt-4 text-right text-lg font-semibold text-red-600">
            Total Despesas: R$ {totalDespesas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
          </div>
        </div>

        {/* Receitas */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold text-gray-900 flex items-center gap-2">
              <TrendingUp className="w-5 h-5 text-green-500" /> Receitas
            </h2>
            {!somenteVisualizacao && (
              <button type="button" onClick={addReceita} className="inline-flex items-center gap-1 px-3 py-1 text-sm bg-green-50 text-green-600 rounded-lg hover:bg-green-100">
                <Plus className="w-4 h-4" /> Adicionar
              </button>
            )}
          </div>
          {receitas.length === 0 ? (
            <p className="text-gray-500 text-center py-4">Nenhuma receita cadastrada</p>
          ) : (
            <div className="space-y-3">
              {receitas.map((rec, index) => (
                <div key={index} className="grid grid-cols-1 md:grid-cols-6 gap-3 p-3 bg-gray-50 rounded-lg">
                  <select value={rec.tipoReceita} onChange={(e) => updateReceita(index, 'tipoReceita', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100">
                    {TIPOS_RECEITA.map((t) => (<option key={t} value={t}>{t}</option>))}
                  </select>
                  <input type="text" placeholder="Descrição" value={rec.descricao || ''} onChange={(e) => updateReceita(index, 'descricao', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
                  <input type="number" placeholder="Valor" value={rec.valor || ''} onChange={(e) => updateReceita(index, 'valor', Number(e.target.value))} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" step="0.01" />
                  <input type="date" value={rec.data} onChange={(e) => updateReceita(index, 'data', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
                  <input type="text" placeholder="Cliente" value={rec.cliente || ''} onChange={(e) => updateReceita(index, 'cliente', e.target.value)} disabled={somenteVisualizacao}
                    className="px-2 py-1 border border-gray-300 rounded text-sm focus:ring-2 focus:ring-green-500 disabled:bg-gray-100" />
                  {!somenteVisualizacao && (
                    <button type="button" onClick={() => removeReceita(index)} className="p-1 text-red-500 hover:bg-red-100 rounded">
                      <Trash2 className="w-4 h-4" />
                    </button>
                  )}
                </div>
              ))}
            </div>
          )}
          <div className="mt-4 text-right text-lg font-semibold text-green-600">
            Total Receitas: R$ {totalReceitas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
          </div>
        </div>

        {/* Resumo Financeiro */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
          <h2 className="text-lg font-semibold text-gray-900 mb-4 flex items-center gap-2">
            <DollarSign className="w-5 h-5 text-blue-500" /> Resumo Financeiro
          </h2>
          <div className="grid grid-cols-3 gap-4 text-center">
            <div className="p-4 bg-green-50 rounded-lg">
              <p className="text-sm text-gray-600">Total Receitas</p>
              <p className="text-xl font-bold text-green-600">R$ {totalReceitas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
            </div>
            <div className="p-4 bg-red-50 rounded-lg">
              <p className="text-sm text-gray-600">Total Despesas</p>
              <p className="text-xl font-bold text-red-600">R$ {totalDespesas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
            </div>
            <div className={`p-4 rounded-lg ${saldo >= 0 ? 'bg-blue-50' : 'bg-orange-50'}`}>
              <p className="text-sm text-gray-600">Saldo da Viagem</p>
              <p className={`text-xl font-bold ${saldo >= 0 ? 'text-blue-600' : 'text-orange-600'}`}>
                R$ {saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
              </p>
            </div>
          </div>
        </div>

        {/* Botões */}
        {!somenteVisualizacao && (
          <div className="flex justify-end gap-4">
            <button type="button" onClick={() => navigate('/transporte/viagens')} className="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200">
              Cancelar
            </button>
            <button type="submit" disabled={saving} className="inline-flex items-center gap-2 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 disabled:opacity-50">
              {saving ? (<><Loader2 className="w-5 h-5 animate-spin" />Salvando...</>) : (<><Save className="w-5 h-5" />Salvar</>)}
            </button>
          </div>
        )}
      </form>
    </div>
  );
}
