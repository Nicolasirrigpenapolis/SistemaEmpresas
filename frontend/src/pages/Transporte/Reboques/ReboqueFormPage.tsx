import { useEffect, useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { ArrowLeft, Save, Loader2, Container, XCircle } from 'lucide-react';
import { reboqueService } from '../../../services/reboqueService';
import type { ReboqueCreateDto } from '../../../types/transporte';
import { TIPOS_CARROCERIA } from '../../../types/transporte';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import { AlertaErro, AlertaSucesso, EstadoCarregando } from '../../../components/common';
import { mascaraPlaca, formatarDocumento, limparNumeros } from '../../../utils/formatters';

export default function ReboqueFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const { podeIncluir, podeAlterar, carregando: carregandoPermissoes } = usePermissaoTela('Reboque');

  // Detectar modo pela URL
  const isEditing = id && id !== 'novo';
  const isViewMode = location.pathname.includes('/visualizar');
  const modo = isViewMode ? 'visualizar' : isEditing ? 'editar' : 'criar';

  const [loading, setLoading] = useState(modo !== 'criar');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const [formData, setFormData] = useState<ReboqueCreateDto>({
    placa: '',
    marca: '',
    modelo: '',
    anoFabricacao: undefined,
    renavam: '',
    chassi: '',
    tipoCarroceria: '',
    capacidadeCarga: undefined,
    tara: undefined,
    rntrc: '',
    proprietarioNome: '',
    proprietarioCpfCnpj: '',
    ativo: true,
  });

  const somenteVisualizacao = modo === 'visualizar';
  const titulo = modo === 'criar' ? 'Novo Reboque' : modo === 'editar' ? 'Editar Reboque' : 'Detalhes do Reboque';

  useEffect(() => {
    if (id && id !== 'novo') {
      loadReboque(Number(id));
    }
  }, [id]);

  const loadReboque = async (reboqueId: number) => {
    try {
      setLoading(true);
      const reboque = await reboqueService.buscarPorId(reboqueId);
      setFormData({
        placa: reboque.placa,
        marca: reboque.marca || '',
        modelo: reboque.modelo || '',
        anoFabricacao: reboque.anoFabricacao,
        renavam: reboque.renavam || '',
        chassi: reboque.chassi || '',
        tipoCarroceria: reboque.tipoCarroceria || '',
        capacidadeCarga: reboque.capacidadeCarga,
        tara: reboque.tara,
        rntrc: reboque.rntrc || '',
        proprietarioNome: reboque.proprietarioNome || '',
        proprietarioCpfCnpj: reboque.proprietarioCpfCnpj || '',
        ativo: reboque.ativo,
      });
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao carregar reboque');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    if (type === 'checkbox') {
      setFormData((prev) => ({ ...prev, [name]: (e.target as HTMLInputElement).checked }));
    } else if (type === 'number') {
      setFormData((prev) => ({ ...prev, [name]: value ? Number(value) : undefined }));
    } else if (name === 'placa') {
      const valorFormatado = mascaraPlaca(e as React.ChangeEvent<HTMLInputElement>);
      setFormData((prev) => ({ ...prev, [name]: valorFormatado }));
    } else if (name === 'proprietarioCpfCnpj') {
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
        await reboqueService.criar(formData);
        setSuccess('Reboque cadastrado com sucesso!');
      } else {
        await reboqueService.atualizar(Number(id), formData);
        setSuccess('Reboque atualizado com sucesso!');
      }
      setTimeout(() => navigate('/transporte/reboques'), 1500);
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao salvar reboque');
    } finally {
      setSaving(false);
    }
  };

  if (carregandoPermissoes || loading) {
    return <EstadoCarregando mensagem="Carregando..." />;
  }

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
      <div className="flex items-center gap-4">
        <button onClick={() => navigate('/transporte/reboques')} className="p-2 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-lg">
          <ArrowLeft className="w-5 h-5" />
        </button>
        <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
          <Container className="w-7 h-7 text-orange-600" />
          {titulo}
        </h1>
      </div>

      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}
      {success && <AlertaSucesso mensagem={success} />}

      <form onSubmit={handleSubmit} className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Placa <span className="text-red-500">*</span></label>
            <input type="text" name="placa" value={formData.placa} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100 uppercase" maxLength={8} />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Marca</label>
            <input type="text" name="marca" value={formData.marca} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Modelo</label>
            <input type="text" name="modelo" value={formData.modelo} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Ano Fabricação</label>
            <input type="number" name="anoFabricacao" value={formData.anoFabricacao || ''} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" min={1900} max={2100} />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Tipo de Carroceria</label>
            <select name="tipoCarroceria" value={formData.tipoCarroceria} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100">
              <option value="">Selecione...</option>
              {TIPOS_CARROCERIA.map((tipo) => (<option key={tipo} value={tipo}>{tipo}</option>))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Capacidade Carga (kg)</label>
            <input type="number" name="capacidadeCarga" value={formData.capacidadeCarga || ''} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" min={0} />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Tara (kg)</label>
            <input type="number" name="tara" value={formData.tara || ''} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" min={0} />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">RENAVAM</label>
            <input type="text" name="renavam" value={formData.renavam} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" maxLength={11} />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Chassi</label>
            <input type="text" name="chassi" value={formData.chassi} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100 uppercase" maxLength={17} />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">RNTRC</label>
            <input type="text" name="rntrc" value={formData.rntrc} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" maxLength={14} />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Proprietário (Nome)</label>
            <input type="text" name="proprietarioNome" value={formData.proprietarioNome} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Proprietário (CPF/CNPJ)</label>
            <input type="text" name="proprietarioCpfCnpj" value={formData.proprietarioCpfCnpj} onChange={handleChange} disabled={somenteVisualizacao}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 disabled:bg-gray-100" maxLength={18} />
          </div>

          <div className="flex items-end">
            <label className="flex items-center gap-2 cursor-pointer">
              <input type="checkbox" name="ativo" checked={formData.ativo} onChange={handleChange} disabled={somenteVisualizacao}
                className="w-4 h-4 text-orange-600 border-gray-300 rounded focus:ring-orange-500" />
              <span className="text-sm text-gray-700">Reboque ativo</span>
            </label>
          </div>
        </div>

        {!somenteVisualizacao && (
          <div className="mt-8 flex justify-end gap-4">
            <button type="button" onClick={() => navigate('/transporte/reboques')} className="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200">
              Cancelar
            </button>
            <button type="submit" disabled={saving} className="inline-flex items-center gap-2 px-4 py-2 bg-orange-600 text-white rounded-lg hover:bg-orange-700 disabled:opacity-50">
              {saving ? (<><Loader2 className="w-5 h-5 animate-spin" />Salvando...</>) : (<><Save className="w-5 h-5" />Salvar</>)}
            </button>
          </div>
        )}
      </form>
    </div>
  );
}
