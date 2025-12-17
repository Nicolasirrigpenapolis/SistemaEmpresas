import { useEffect, useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import {
  ArrowLeft,
  Save,
  Loader2,
  User,
  XCircle,
  MapPin,
  Phone,
} from 'lucide-react';
import { motoristaService } from '../../../services/Transporte/motoristaService';
import type { MotoristaCreateDto } from '../../../types';
import { UFS_BRASIL } from '../../../types';
import { usePermissaoTela } from '../../../hooks/usePermissaoTela';
import { AlertaErro, AlertaSucesso, EstadoCarregando } from '../../../components/common';
import { mascaraCPF, mascaraTelefone, mascaraCEP, formatarCPF, formatarTelefone, formatarCEP, limparNumeros } from '../../../utils/formatters';

export default function MotoristaFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const { podeIncluir, podeAlterar, carregando: carregandoPermissoes } = usePermissaoTela('Motorista');

  // Detectar modo pela URL
  const isEditing = id && id !== 'novo';
  const isViewMode = location.pathname.includes('/visualizar');
  const modo = isViewMode ? 'visualizar' : isEditing ? 'editar' : 'criar';

  const [loading, setLoading] = useState(modo !== 'criar');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const [formData, setFormData] = useState<MotoristaCreateDto>({
    nomeDoMotorista: '',
    rg: '',
    cpf: '',
    endereco: '',
    numero: '',
    bairro: '',
    municipio: 0,
    uf: 'SP',
    cep: '',
    fone: '',
    cel: '',
  });

  const somenteVisualizacao = modo === 'visualizar';
  const titulo = modo === 'criar' ? 'Novo Motorista' : modo === 'editar' ? 'Editar Motorista' : 'Detalhes do Motorista';

  useEffect(() => {
    if (id && id !== 'novo') {
      loadMotorista(Number(id));
    }
  }, [id]);

  const loadMotorista = async (motoristaId: number) => {
    try {
      setLoading(true);
      setError(null);
      const motorista = await motoristaService.buscarPorId(motoristaId);
      setFormData({
        nomeDoMotorista: motorista.nomeDoMotorista || '',
        rg: motorista.rg || '',
        cpf: formatarCPF(motorista.cpf) || '',
        endereco: motorista.endereco || '',
        numero: motorista.numero || '',
        bairro: motorista.bairro || '',
        municipio: motorista.municipio || 0,
        uf: motorista.uf || 'SP',
        cep: formatarCEP(motorista.cep) || '',
        fone: formatarTelefone(motorista.fone) || '',
        cel: formatarTelefone(motorista.cel) || '',
      });
    } catch (err: any) {
      console.error('Erro ao carregar motorista:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar motorista');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;

    // Aplicar máscaras
    let valorFormatado = value;
    if (name === 'cpf') {
      valorFormatado = mascaraCPF(e as React.ChangeEvent<HTMLInputElement>);
    } else if (name === 'fone' || name === 'cel') {
      valorFormatado = mascaraTelefone(e as React.ChangeEvent<HTMLInputElement>);
    } else if (name === 'cep') {
      valorFormatado = mascaraCEP(e as React.ChangeEvent<HTMLInputElement>);
    }

    setFormData((prev) => ({ ...prev, [name]: valorFormatado }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.nomeDoMotorista?.trim()) {
      setError('O nome do motorista é obrigatório');
      return;
    }

    if (!formData.cpf?.trim()) {
      setError('O CPF é obrigatório');
      return;
    }

    try {
      setSaving(true);
      setError(null);

      // Limpar máscaras antes de enviar
      const payload = {
        ...formData,
        cpf: limparNumeros(formData.cpf),
        cep: limparNumeros(formData.cep),
        fone: limparNumeros(formData.fone),
        cel: limparNumeros(formData.cel),
      };

      if (modo === 'criar') {
        await motoristaService.criar(payload);
        setSuccess('Motorista cadastrado com sucesso!');
      } else {
        await motoristaService.atualizar(Number(id), payload);
        setSuccess('Motorista atualizado com sucesso!');
      }

      setTimeout(() => navigate('/transporte/motoristas'), 1500);
    } catch (err: any) {
      console.error('Erro ao salvar motorista:', err);
      setError(err.response?.data?.mensagem || 'Erro ao salvar motorista');
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
          <h2 className="text-xl font-semibold text-primary/80">Acesso Negado</h2>
          <p className="text-muted-foreground mt-2">Você não tem permissão para esta operação.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate('/transporte/motoristas')}
          className="p-2 text-muted-foreground hover:text-primary/80 hover:bg-surface-hover rounded-lg transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-primary flex items-center gap-2">
            <User className="w-7 h-7 text-blue-600" />
            {titulo}
          </h1>
        </div>
      </div>

      {/* Alertas */}
      {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}
      {success && <AlertaSucesso mensagem={success} />}

      {/* Formulário */}
      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Seção 1: Dados Pessoais */}
        <div className="bg-surface rounded-xl shadow-sm border border-border p-6">
          <h3 className="text-lg font-semibold text-primary mb-4 flex items-center gap-2 pb-3 border-b border-border">
            <User className="w-5 h-5 text-blue-500" />
            Dados Pessoais
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div className="lg:col-span-2">
              <label className="block text-sm font-medium text-primary/80 mb-1">
                Nome <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                name="nomeDoMotorista"
                value={formData.nomeDoMotorista}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={30}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="Nome completo do motorista"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">
                CPF <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                name="cpf"
                value={formData.cpf}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={14}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="000.000.000-00"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">RG</label>
              <input
                type="text"
                name="rg"
                value={formData.rg}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={20}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="Número do RG"
              />
            </div>
          </div>
        </div>

        {/* Seção 2: Endereço */}
        <div className="bg-surface rounded-xl shadow-sm border border-border p-6">
          <h3 className="text-lg font-semibold text-primary mb-4 flex items-center gap-2 pb-3 border-b border-border">
            <MapPin className="w-5 h-5 text-green-500" />
            Endereço
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">CEP</label>
              <input
                type="text"
                name="cep"
                value={formData.cep}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={9}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="00000-000"
              />
            </div>
            <div className="lg:col-span-2">
              <label className="block text-sm font-medium text-primary/80 mb-1">Endereço</label>
              <input
                type="text"
                name="endereco"
                value={formData.endereco}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={100}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="Rua, Avenida..."
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Número</label>
              <input
                type="text"
                name="numero"
                value={formData.numero}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={9}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="Nº"
              />
            </div>
            <div className="lg:col-span-2">
              <label className="block text-sm font-medium text-primary/80 mb-1">Bairro</label>
              <input
                type="text"
                name="bairro"
                value={formData.bairro}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={50}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="Bairro"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">UF</label>
              <select
                name="uf"
                value={formData.uf}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
              >
                {UFS_BRASIL.map((uf) => (
                  <option key={uf} value={uf}>{uf}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Código Município</label>
              <input
                type="number"
                name="municipio"
                value={formData.municipio || ''}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="Código IBGE"
              />
            </div>
          </div>
        </div>

        {/* Seção 3: Contato */}
        <div className="bg-surface rounded-xl shadow-sm border border-border p-6">
          <h3 className="text-lg font-semibold text-primary mb-4 flex items-center gap-2 pb-3 border-b border-border">
            <Phone className="w-5 h-5 text-amber-500" />
            Contato
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Telefone</label>
              <input
                type="text"
                name="fone"
                value={formData.fone}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={15}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="(00) 0000-0000"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-primary/80 mb-1">Celular</label>
              <input
                type="text"
                name="cel"
                value={formData.cel}
                onChange={handleChange}
                disabled={somenteVisualizacao}
                maxLength={15}
                className="w-full px-3 py-2 border border-input rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent disabled:bg-surface-hover"
                placeholder="(00) 00000-0000"
              />
            </div>
          </div>
        </div>

        {/* Botões */}
        {!somenteVisualizacao && (
          <div className="flex justify-end gap-3">
            <button
              type="button"
              onClick={() => navigate('/transporte/motoristas')}
              className="px-4 py-2 text-primary/80 bg-surface-hover rounded-lg hover:bg-gray-200 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={saving}
              className="inline-flex items-center gap-2 px-4 py-2 bg-gradient-to-r from-blue-600 to-indigo-600 text-white rounded-lg hover:from-blue-700 hover:to-indigo-700 disabled:opacity-50 transition-colors"
            >
              {saving ? (
                <>
                  <Loader2 className="w-4 h-4 animate-spin" />
                  Salvando...
                </>
              ) : (
                <>
                  <Save className="w-4 h-4" />
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
