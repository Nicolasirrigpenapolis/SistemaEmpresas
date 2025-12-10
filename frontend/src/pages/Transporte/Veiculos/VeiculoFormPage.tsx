import { useEffect, useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import {
  ArrowLeft,
  Save,
  Loader2,
  Truck,
  XCircle,
} from 'lucide-react';
import { veiculoService } from '../../../services/veiculoService';
import type { VeiculoCreateDto } from '../../../types/transporte';
import { TIPOS_VEICULO, TIPOS_CARROCERIA, UFS_BRASIL } from '../../../types/transporte';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import { AlertaErro, AlertaSucesso, EstadoCarregando } from '../../../components/common';
import { mascaraPlaca, formatarDocumento, limparNumeros } from '../../../utils/formatters';

export default function VeiculoFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const { podeIncluir, podeAlterar, carregando: carregandoPermissoes } = usePermissaoTela('Veiculo');

  // Detectar modo pela URL
  const isEditing = id && id !== 'novo';
  const isViewMode = location.pathname.includes('/visualizar');
  const modo = isViewMode ? 'visualizar' : isEditing ? 'editar' : 'criar';

  // Estados
  const [loading, setLoading] = useState(modo !== 'criar');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  // Formulário
  const [formData, setFormData] = useState<VeiculoCreateDto>({
    placa: '',
    placaReboque: '',
    marca: '',
    modelo: '',
    anoFabricacao: undefined,
    anoModelo: undefined,
    tara: 0, // Obrigatório - valor padrão
    capacidadeKg: undefined,
    tipoRodado: '', // Obrigatório (antes era tipoVeiculo)
    tipoCarroceria: '', // Obrigatório
    uf: '', // Obrigatório - UF de registro
    renavam: '',
    chassi: '',
    cor: '',
    tipoCombustivel: '',
    rntrc: '',
    proprietarioNome: '',
    proprietarioCpfCnpj: '',
    ativo: true,
  });

  const somenteVisualizacao = modo === 'visualizar';
  const titulo = modo === 'criar' ? 'Novo Veículo' : modo === 'editar' ? 'Editar Veículo' : 'Detalhes do Veículo';

  // Carregar dados para edição/visualização
  useEffect(() => {
    if (id && id !== 'novo') {
      loadVeiculo(Number(id));
    }
  }, [id]);

  const loadVeiculo = async (veiculoId: number) => {
    try {
      setLoading(true);
      setError(null);
      const veiculo = await veiculoService.buscarPorId(veiculoId);
      setFormData({
        placa: veiculo.placa,
        placaReboque: veiculo.placaReboque || '',
        marca: veiculo.marca || '',
        modelo: veiculo.modelo || '',
        anoFabricacao: veiculo.anoFabricacao,
        anoModelo: veiculo.anoModelo,
        tara: veiculo.tara,
        capacidadeKg: veiculo.capacidadeKg,
        tipoRodado: veiculo.tipoRodado,
        tipoCarroceria: veiculo.tipoCarroceria,
        uf: veiculo.uf,
        renavam: veiculo.renavam || '',
        chassi: veiculo.chassi || '',
        cor: veiculo.cor || '',
        tipoCombustivel: veiculo.tipoCombustivel || '',
        rntrc: veiculo.rntrc || '',
        proprietarioNome: veiculo.proprietarioNome || '',
        proprietarioCpfCnpj: veiculo.proprietarioCpfCnpj || '',
        ativo: veiculo.ativo,
      });
    } catch (err: any) {
      console.error('Erro ao carregar veículo:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar veículo');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    
    if (type === 'checkbox') {
      const checked = (e.target as HTMLInputElement).checked;
      setFormData((prev) => ({ ...prev, [name]: checked }));
    } else if (type === 'number') {
      setFormData((prev) => ({ ...prev, [name]: value ? Number(value) : undefined }));
    } else if (name === 'placa' || name === 'placaReboque') {
      // Aplicar máscara de placa
      const valorFormatado = mascaraPlaca(e as React.ChangeEvent<HTMLInputElement>);
      setFormData((prev) => ({ ...prev, [name]: valorFormatado }));
    } else if (name === 'proprietarioCpfCnpj') {
      // Aplicar máscara de CPF/CNPJ
      const numeros = limparNumeros(value).slice(0, 14);
      const formatado = formatarDocumento(numeros);
      setFormData((prev) => ({ ...prev, [name]: formatado }));
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.placa?.trim()) {
      setError('A placa é obrigatória');
      return;
    }

    try {
      setSaving(true);
      setError(null);

      if (modo === 'criar') {
        await veiculoService.criar(formData);
        setSuccess('Veículo cadastrado com sucesso!');
      } else {
        await veiculoService.atualizar(Number(id), formData);
        setSuccess('Veículo atualizado com sucesso!');
      }

      setTimeout(() => navigate('/transporte/veiculos'), 1500);
    } catch (err: any) {
      console.error('Erro ao salvar veículo:', err);
      setError(err.response?.data?.mensagem || 'Erro ao salvar veículo');
    } finally {
      setSaving(false);
    }
  };

  // Loading
  if (carregandoPermissoes || loading) {
    return <EstadoCarregando mensagem="Carregando..." />;
  }

  // Verificar permissões
  if ((modo === 'criar' && !podeIncluir) || (modo === 'editar' && !podeAlterar)) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-center">
          <XCircle className="w-16 h-16 text-red-400 mx-auto mb-4" />
          <h2 className="text-xl font-semibold text-gray-700">Acesso Negado</h2>
          <p className="text-gray-500 mt-2">Você não tem permissão para esta operação.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate('/transporte/veiculos')}
          className="p-2 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-lg transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
            <Truck className="w-7 h-7 text-blue-600" />
            {titulo}
          </h1>
        </div>
      </div>

      {/* Alertas */}
      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}
      {success && <AlertaSucesso mensagem={success} />}

      {/* Formulário */}
      <form onSubmit={handleSubmit} className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {/* Placa */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Placa <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              name="placa"
              value={formData.placa}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500 uppercase"
              placeholder="ABC-1234"
              maxLength={8}
            />
          </div>

          {/* Placa Reboque */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Placa Reboque
            </label>
            <input
              type="text"
              name="placaReboque"
              value={formData.placaReboque}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500 uppercase"
              placeholder="ABC-1234"
              maxLength={8}
            />
          </div>

          {/* Marca */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Marca
            </label>
            <input
              type="text"
              name="marca"
              value={formData.marca}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Ex: Volkswagen"
            />
          </div>

          {/* Modelo */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Modelo
            </label>
            <input
              type="text"
              name="modelo"
              value={formData.modelo}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Ex: Constellation 24.280"
            />
          </div>

          {/* Ano Fabricação */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Ano Fabricação
            </label>
            <input
              type="number"
              name="anoFabricacao"
              value={formData.anoFabricacao || ''}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="2024"
              min={1900}
              max={2100}
            />
          </div>

          {/* Cor */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Cor
            </label>
            <input
              type="text"
              name="cor"
              value={formData.cor}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Ex: Branco"
            />
          </div>

          {/* Tipo Rodado (Tipo de Veículo) */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Tipo de Veículo <span className="text-red-500">*</span>
            </label>
            <select
              name="tipoRodado"
              value={formData.tipoRodado}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
            >
              <option value="">Selecione...</option>
              {TIPOS_VEICULO.map((tipo) => (
                <option key={tipo} value={tipo}>
                  {tipo}
                </option>
              ))}
            </select>
          </div>

          {/* Tipo Carroceria */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Tipo de Carroceria
            </label>
            <select
              name="tipoCarroceria"
              value={formData.tipoCarroceria}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
            >
              <option value="">Selecione...</option>
              {TIPOS_CARROCERIA.map((tipo) => (
                <option key={tipo} value={tipo}>
                  {tipo}
                </option>
              ))}
            </select>
          </div>

          {/* Capacidade Carga */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Capacidade de Carga (kg)
            </label>
            <input
              type="number"
              name="capacidadeKg"
              value={formData.capacidadeKg || ''}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Ex: 15000"
              min={0}
            />
          </div>

          {/* Tara */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Tara (kg)
            </label>
            <input
              type="number"
              name="tara"
              value={formData.tara || ''}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Ex: 8000"
              min={0}
            />
          </div>

          {/* RENAVAM */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              RENAVAM
            </label>
            <input
              type="text"
              name="renavam"
              value={formData.renavam}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Ex: 12345678901"
              maxLength={11}
            />
          </div>

          {/* Chassi */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Chassi
            </label>
            <input
              type="text"
              name="chassi"
              value={formData.chassi}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500 uppercase"
              placeholder="Ex: 9BWZZZ377VT004251"
              maxLength={17}
            />
          </div>

          {/* UF */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              UF <span className="text-red-500">*</span>
            </label>
            <select
              name="uf"
              value={formData.uf}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
            >
              <option value="">Selecione...</option>
              {UFS_BRASIL.map((uf) => (
                <option key={uf} value={uf}>
                  {uf}
                </option>
              ))}
            </select>
          </div>

          {/* RNTRC */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              RNTRC
            </label>
            <input
              type="text"
              name="rntrc"
              value={formData.rntrc}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Ex: 12345678"
              maxLength={14}
            />
          </div>

          {/* Proprietário Nome */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Proprietário (Nome)
            </label>
            <input
              type="text"
              name="proprietarioNome"
              value={formData.proprietarioNome}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="Nome do proprietário"
            />
          </div>

          {/* Proprietário CPF/CNPJ */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Proprietário (CPF/CNPJ)
            </label>
            <input
              type="text"
              name="proprietarioCpfCnpj"
              value={formData.proprietarioCpfCnpj}
              onChange={handleChange}
              disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              placeholder="CPF ou CNPJ"
              maxLength={18}
            />
          </div>

          {/* Ativo */}
          <div className="flex items-end">
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                name="ativo"
                checked={formData.ativo}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Veículo ativo</span>
            </label>
          </div>
        </div>

        {/* Botões */}
        {!somenteVisualizacao && (
          <div className="mt-8 flex justify-end gap-4">
            <button
              type="button"
              onClick={() => navigate('/transporte/veiculos')}
              className="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={saving}
              className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50"
            >
              {saving ? (
                <>
                  <Loader2 className="w-5 h-5 animate-spin" />
                  Salvando...
                </>
              ) : (
                <>
                  <Save className="w-5 h-5" />
                  Salvar
                </>
              )}
            </button>
          </div>
        )}
      </form>
    </div>
  );
}
