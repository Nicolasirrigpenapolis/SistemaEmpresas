import { useEffect, useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import {
  ArrowLeft,
  Save,
  User,
  Building2,
  Truck,
  UserCircle,
  Receipt,
  CreditCard,
  MapPin,
  Phone,
  FileText,
  Loader2,
  AlertCircle,
  Search,
  CheckCircle2,
  Edit2,
  Eye,
  ChevronRight,
  Calendar,
  Globe,
  Mail,
  Smartphone,
  Hash,
  Landmark,
  Banknote,
  BadgeCheck,
  X,
} from 'lucide-react';
import { geralService } from '../../services/Geral/geralService';
import type {
  GeralCreateDto,
  GeralListDto,
} from '../../types';
import { GERAL_DEFAULT, getLabelDocumento, getLabelIdentidade } from '../../types';
import {
  formatarCPF,
  formatarCNPJ,
  formatarCEP,
  formatarTelefone,
  parseCPFCNPJ,
  parseCEP,
  parseTelefone,
  limparNumeros,
} from '../../utils/formatters';

// ============================================================================
// COMPONENTES DE UI REUTILIZÁVEIS
// ============================================================================

// Input com ícone e label flutuante
interface InputModernoProps {
  label: string;
  value: string | number | undefined;
  onChange: (value: string) => void;
  type?: string;
  placeholder?: string;
  icone?: React.ReactNode;
  disabled?: boolean;
  required?: boolean;
  className?: string;
  sucesso?: boolean;
  erro?: string;
  acaoDireita?: React.ReactNode;
}

function InputModerno({
  label,
  value,
  onChange,
  type = 'text',
  placeholder,
  icone,
  disabled,
  required,
  className = '',
  sucesso,
  erro,
  acaoDireita,
}: InputModernoProps) {
  const [focado, setFocado] = useState(false);
  const temValor = value !== undefined && value !== '' && value !== 0;

  return (
    <div className={`relative ${className}`}>
      <div className={`
        relative flex items-center border rounded-xl transition-all duration-200
        ${focado ? 'border-blue-500 ring-2 ring-blue-500/20 shadow-sm' : 'border-gray-200 hover:border-gray-300'}
        ${sucesso ? 'border-green-500 bg-green-50/50' : ''}
        ${erro ? 'border-red-500 bg-red-50/50' : ''}
        ${disabled ? 'bg-gray-50 cursor-not-allowed' : 'bg-white'}
      `}>
        {icone && (
          <span className={`pl-3 ${focado ? 'text-blue-500' : 'text-gray-400'} transition-colors`}>
            {icone}
          </span>
        )}
        <div className="relative flex-1">
          <input
            type={type}
            value={value ?? ''}
            onChange={(e) => onChange(e.target.value)}
            onFocus={() => setFocado(true)}
            onBlur={() => setFocado(false)}
            disabled={disabled}
            placeholder={focado ? placeholder : ''}
            className={`
              w-full px-3 py-3 bg-transparent outline-none text-gray-900
              placeholder:text-gray-400 disabled:cursor-not-allowed
              ${icone ? 'pl-1' : ''}
            `}
          />
          <label className={`
            absolute left-0 transition-all duration-200 pointer-events-none
            ${icone ? 'left-1' : 'left-3'}
            ${focado || temValor
              ? '-top-2.5 text-xs bg-white px-1 rounded'
              : 'top-3 text-sm'
            }
            ${focado ? 'text-blue-600' : 'text-gray-500'}
            ${sucesso ? 'text-green-600' : ''}
            ${erro ? 'text-red-600' : ''}
          `}>
            {label}{required && <span className="text-red-500 ml-0.5">*</span>}
          </label>
        </div>
        {sucesso && !acaoDireita && (
          <CheckCircle2 className="w-5 h-5 text-green-500 mr-3" />
        )}
        {acaoDireita && <div className="pr-2">{acaoDireita}</div>}
      </div>
      {erro && <p className="text-xs text-red-500 mt-1 ml-1">{erro}</p>}
    </div>
  );
}

// Select moderno
interface SelectModernoProps {
  label: string;
  value: string | number;
  onChange: (value: string | number) => void;
  opcoes: { value: string | number; label: string }[];
  disabled?: boolean;
  icone?: React.ReactNode;
  className?: string;
}

function SelectModerno({ label, value, onChange, opcoes, disabled, icone, className = '' }: SelectModernoProps) {
  return (
    <div className={`relative ${className}`}>
      <div className={`
        relative flex items-center border border-gray-200 rounded-xl transition-all duration-200
        hover:border-gray-300 focus-within:border-blue-500 focus-within:ring-2 focus-within:ring-blue-500/20
        ${disabled ? 'bg-gray-50' : 'bg-white'}
      `}>
        {icone && <span className="pl-3 text-gray-400">{icone}</span>}
        <select
          value={value}
          onChange={(e) => onChange(e.target.value)}
          disabled={disabled}
          className={`
            w-full px-3 py-3 bg-transparent outline-none appearance-none cursor-pointer
            ${icone ? 'pl-1' : ''}
          `}
        >
          {opcoes.map((op) => (
            <option key={op.value} value={op.value}>{op.label}</option>
          ))}
        </select>
        <ChevronRight className="w-4 h-4 text-gray-400 mr-3 rotate-90" />
      </div>
      <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">
        {label}
      </label>
    </div>
  );
}

// Checkbox moderno com estilo de chip
interface CheckboxChipProps {
  checked: boolean;
  onChange: (checked: boolean) => void;
  label: string;
  icone: React.ReactNode;
  cor: string;
  disabled?: boolean;
}

function CheckboxChip({ checked, onChange, label, icone, cor, disabled }: CheckboxChipProps) {
  const cores: Record<string, { bg: string; border: string; text: string; icon: string }> = {
    blue: { bg: 'bg-blue-50', border: 'border-blue-300', text: 'text-blue-700', icon: 'text-blue-500' },
    green: { bg: 'bg-green-50', border: 'border-green-300', text: 'text-green-700', icon: 'text-green-500' },
    purple: { bg: 'bg-purple-50', border: 'border-purple-300', text: 'text-purple-700', icon: 'text-purple-500' },
    orange: { bg: 'bg-orange-50', border: 'border-orange-300', text: 'text-orange-700', icon: 'text-orange-500' },
    red: { bg: 'bg-red-50', border: 'border-red-300', text: 'text-red-700', icon: 'text-red-500' },
    amber: { bg: 'bg-amber-50', border: 'border-amber-300', text: 'text-amber-700', icon: 'text-amber-500' },
  };

  const estilo = cores[cor] || cores.blue;

  return (
    <button
      type="button"
      onClick={() => !disabled && onChange(!checked)}
      disabled={disabled}
      className={`
        flex items-center gap-2 px-4 py-2.5 rounded-xl border-2 transition-all duration-200
        ${checked
          ? `${estilo.bg} ${estilo.border} ${estilo.text} shadow-sm`
          : 'border-gray-200 text-gray-600 hover:border-gray-300 hover:bg-gray-50'
        }
        ${disabled ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'}
      `}
    >
      <span className={checked ? estilo.icon : 'text-gray-400'}>{icone}</span>
      <span className="text-sm font-medium">{label}</span>
      {checked && <BadgeCheck className={`w-4 h-4 ${estilo.icon}`} />}
    </button>
  );
}

// Card de seção com título
interface SecaoCardProps {
  titulo: string;
  subtitulo?: string;
  icone: React.ReactNode;
  children: React.ReactNode;
  className?: string;
}

function SecaoCard({ titulo, subtitulo, icone, children, className = '' }: SecaoCardProps) {
  return (
    <div className={`bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden ${className}`}>
      <div className="px-5 py-4 border-b border-gray-100 bg-gradient-to-r from-gray-50 to-white">
        <div className="flex items-center gap-3">
          <div className="p-2 bg-blue-50 rounded-lg text-blue-600">
            {icone}
          </div>
          <div>
            <h3 className="font-semibold text-gray-900">{titulo}</h3>
            {subtitulo && <p className="text-xs text-gray-500">{subtitulo}</p>}
          </div>
        </div>
      </div>
      <div className="p-5">{children}</div>
    </div>
  );
}

// ============================================================================
// COMPONENTE DE FORMULÁRIO
// ============================================================================
export default function GeralFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = id && id !== 'novo';
  const isViewMode = location.pathname.includes('/visualizar');

  // Estados
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'dados' | 'cobranca'>('dados');
  const [vendedores, setVendedores] = useState<GeralListDto[]>([]);

  // Estados para busca CNPJ/CEP
  const [buscandoCnpj, setBuscandoCnpj] = useState(false);
  const [buscandoCep, setBuscandoCep] = useState(false);
  const [cnpjEncontrado, setCnpjEncontrado] = useState(false);
  const [cepEncontrado, setCepEncontrado] = useState(false);

  // Dados extras para visualização
  const [dadosExtras, setDadosExtras] = useState<{
    municipioNome?: string;
    municipioUf?: string;
    vendedorNome?: string;
    dataDoCadastro?: string;
  }>({});

  // Form data
  const [formData, setFormData] = useState<GeralCreateDto>(GERAL_DEFAULT);

  // ============================================================================
  // FUNÇÕES
  // ============================================================================
  const loadData = async () => {
    if (!isEditing) return;

    try {
      setLoading(true);
      const data = await geralService.buscarPorId(Number(id));

      setFormData({
        cliente: data.cliente,
        fornecedor: data.fornecedor,
        despesa: data.despesa,
        imposto: data.imposto,
        transportadora: data.transportadora,
        vendedor: data.vendedor,
        razaoSocial: data.razaoSocial,
        nomeFantasia: data.nomeFantasia,
        tipo: data.tipo,
        cpfECnpj: data.cpfECnpj,
        rgEIe: data.rgEIe,
        codigoDoSuframa: data.codigoDoSuframa,
        codigoDaAntt: data.codigoDaAntt,
        endereco: data.endereco,
        numeroDoEndereco: data.numeroDoEndereco,
        complemento: data.complemento,
        bairro: data.bairro,
        caixaPostal: data.caixaPostal,
        sequenciaDoMunicipio: data.sequenciaDoMunicipio,
        cep: data.cep,
        sequenciaDoPais: data.sequenciaDoPais,
        fone1: data.fone1,
        fone2: data.fone2,
        fax: data.fax,
        celular: data.celular,
        contato: data.contato,
        email: data.email,
        homePage: data.homePage,
        observacao: data.observacao,
        revenda: data.revenda,
        isento: data.isento,
        orgonPublico: data.orgonPublico,
        empresaProdutor: data.empresaProdutor,
        cumulativo: data.cumulativo,
        inativo: data.inativo,
        sequenciaDoVendedor: data.sequenciaDoVendedor,
        intermediarioDoVendedor: data.intermediarioDoVendedor,
        enderecoDeCobranca: data.enderecoDeCobranca,
        numeroDoEnderecoDeCobranca: data.numeroDoEnderecoDeCobranca,
        complementoDaCobranca: data.complementoDaCobranca,
        bairroDeCobranca: data.bairroDeCobranca,
        caixaPostalDaCobranca: data.caixaPostalDaCobranca,
        sequenciaMunicipioCobranca: data.sequenciaMunicipioCobranca,
        cepDeCobranca: data.cepDeCobranca,
        nomeDoBanco1: data.nomeDoBanco1,
        agenciaDoBanco1: data.agenciaDoBanco1,
        contaCorrenteDoBanco1: data.contaCorrenteDoBanco1,
        nomeDoCorrentistaDoBanco1: data.nomeDoCorrentistaDoBanco1,
        nomeDoBanco2: data.nomeDoBanco2,
        agenciaDoBanco2: data.agenciaDoBanco2,
        contaCorrenteDoBanco2: data.contaCorrenteDoBanco2,
        nomeDoCorrentistaDoBanco2: data.nomeDoCorrentistaDoBanco2,
        dataDeNascimento: data.dataDeNascimento,
        codigoContabil: data.codigoContabil,
        codigoAdiantamento: data.codigoAdiantamento,
        salBruto: data.salBruto,
      });

      // Guarda dados extras para visualização
      setDadosExtras({
        municipioNome: data.municipioNome,
        municipioUf: data.municipioUf,
        vendedorNome: data.vendedorNome,
        dataDoCadastro: data.dataDoCadastro,
      });
    } catch (err: any) {
      console.error('Erro ao carregar dados:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

  const loadVendedores = async () => {
    try {
      const data = await geralService.listarVendedores();
      setVendedores(data);
    } catch (err) {
      console.error('Erro ao carregar vendedores:', err);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      setSaving(true);
      setError(null);

      // Preparar dados para envio (remover máscaras)
      const dadosParaEnviar: GeralCreateDto = {
        ...formData,
        cpfECnpj: parseCPFCNPJ(formData.cpfECnpj),
        cep: parseCEP(formData.cep),
        fone1: parseTelefone(formData.fone1),
        fone2: parseTelefone(formData.fone2),
        celular: parseTelefone(formData.celular),
        fax: parseTelefone(formData.fax),
        cepDeCobranca: parseCEP(formData.cepDeCobranca),
      };

      // Backend valida campos obrigatórios (razão social, tipo)
      if (isEditing) {
        await geralService.atualizar(Number(id), {
          ...dadosParaEnviar,
          sequenciaDoGeral: Number(id),
        });
      } else {
        await geralService.criar(dadosParaEnviar);
      }

      navigate('/cadastros/geral');
    } catch (err: any) {
      console.error('Erro ao salvar:', err);
      setError(err.response?.data?.mensagem || 'Erro ao salvar dados');
    } finally {
      setSaving(false);
    }
  };

  const handleChange = (field: keyof GeralCreateDto, value: any) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    // Reseta os indicadores de encontrado quando o usuário digita
    if (field === 'cpfECnpj') setCnpjEncontrado(false);
    if (field === 'cep') setCepEncontrado(false);
  };

  const handleBuscarCnpj = async () => {
    if (!formData.cpfECnpj) return;

    try {
      setBuscandoCnpj(true);
      setError(null);

      const dados = await geralService.consultarCnpj(formData.cpfECnpj);

      // Preenche os dados no formulário
      setFormData((prev) => ({
        ...prev,
        razaoSocial: dados.razaoSocial || prev.razaoSocial,
        nomeFantasia: dados.nomeFantasia || prev.nomeFantasia,
        endereco: dados.logradouro || prev.endereco,
        numeroDoEndereco: dados.numero || prev.numeroDoEndereco,
        complemento: dados.complemento || prev.complemento,
        bairro: dados.bairro || prev.bairro,
        cep: dados.cep || prev.cep,
        sequenciaDoMunicipio: dados.sequenciaDoMunicipio || prev.sequenciaDoMunicipio,
        fone1: dados.telefone || prev.fone1,
        email: dados.email || prev.email,
      }));

      setCnpjEncontrado(true);
      // Se encontrou o CEP, marca como encontrado também
      if (dados.cep) setCepEncontrado(true);
    } catch (err: any) {
      console.error('Erro ao buscar CNPJ:', err);
      setError(err.response?.data?.mensagem || 'Erro ao buscar dados do CNPJ');
      setCnpjEncontrado(false);
    } finally {
      setBuscandoCnpj(false);
    }
  };

  const handleBuscarCep = async () => {
    if (!formData.cep) return;

    try {
      setBuscandoCep(true);
      setError(null);

      const dados = await geralService.consultarCep(formData.cep);

      // Preenche os dados no formulário
      setFormData((prev) => ({
        ...prev,
        endereco: dados.logradouro || prev.endereco,
        complemento: dados.complemento || prev.complemento,
        bairro: dados.bairro || prev.bairro,
        sequenciaDoMunicipio: dados.sequenciaDoMunicipio || prev.sequenciaDoMunicipio,
      }));

      setCepEncontrado(true);
    } catch (err: any) {
      console.error('Erro ao buscar CEP:', err);
      setError(err.response?.data?.mensagem || 'Erro ao buscar dados do CEP');
      setCepEncontrado(false);
    } finally {
      setBuscandoCep(false);
    }
  };

  // ============================================================================
  // EFFECTS
  // ============================================================================
  useEffect(() => {
    loadData();
    loadVendedores();
  }, [id]);

  // ============================================================================
  // RENDER
  // ============================================================================
  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <div className="text-center">
          <div className="relative">
            <div className="w-16 h-16 border-4 border-blue-200 rounded-full animate-pulse" />
            <Loader2 className="w-8 h-8 text-blue-600 animate-spin absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2" />
          </div>
          <p className="mt-4 text-gray-500 font-medium">Carregando dados...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className={`flex items-center justify-between ${isViewMode
          ? 'bg-gradient-to-r from-indigo-600 via-blue-600 to-cyan-500 -mx-6 -mt-6 px-6 py-4 rounded-t-xl'
          : ''
        }`}>
        <div className="flex items-center gap-4">
          <button
            onClick={() => navigate('/cadastros/geral')}
            className={`p-2.5 rounded-xl transition-all duration-200 ${isViewMode
                ? 'hover:bg-white/20 text-white'
                : 'hover:bg-gray-100 text-gray-600'
              }`}
          >
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div>
            <div className="flex items-center gap-2">
              {isViewMode && (
                <div className="p-1.5 bg-white/20 rounded-lg">
                  <Eye className="w-4 h-4 text-white" />
                </div>
              )}
              <h1 className={`text-xl font-bold ${isViewMode ? 'text-white' : 'text-gray-900'}`}>
                {isViewMode ? 'Visualizar Cadastro' : isEditing ? 'Editar Cadastro' : 'Novo Cadastro'}
              </h1>
            </div>
            <div className={`flex items-center gap-2 text-sm ${isViewMode ? 'text-blue-100' : 'text-gray-500'}`}>
              {(isEditing || isViewMode) && (
                <>
                  <Hash className="w-3.5 h-3.5" />
                  <span>Código: {id}</span>
                </>
              )}
              {!isEditing && !isViewMode && (
                <span>Preencha os dados do cadastro</span>
              )}
              {isViewMode && dadosExtras.municipioNome && (
                <>
                  <span className="opacity-50">•</span>
                  <MapPin className="w-3.5 h-3.5" />
                  <span>{dadosExtras.municipioNome}/{dadosExtras.municipioUf}</span>
                </>
              )}
            </div>
          </div>
        </div>

        {isViewMode ? (
          <button
            type="button"
            onClick={() => navigate(`/cadastros/geral/${id}`)}
            className="flex items-center gap-2 px-5 py-2.5 bg-white text-blue-600 rounded-xl hover:bg-blue-50 transition-all duration-200 shadow-lg shadow-blue-500/20 font-medium"
          >
            <Edit2 className="w-4 h-4" />
            <span>Editar</span>
          </button>
        ) : (
          <button
            onClick={handleSubmit}
            disabled={saving}
            className="flex items-center gap-2 px-5 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-xl hover:from-blue-700 hover:to-blue-800 transition-all duration-200 disabled:opacity-50 shadow-lg shadow-blue-500/30 font-medium"
          >
            {saving ? (
              <Loader2 className="w-4 h-4 animate-spin" />
            ) : (
              <Save className="w-4 h-4" />
            )}
            <span>{saving ? 'Salvando...' : 'Salvar'}</span>
          </button>
        )}
      </div>

      {/* Conteúdo */}
      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Erro */}
        {error && (
          <div className="p-4 bg-red-50 border border-red-200 rounded-xl flex items-center gap-3 text-red-700 animate-in slide-in-from-top-2 duration-300">
            <div className="p-2 bg-red-100 rounded-lg">
              <AlertCircle className="w-5 h-5" />
            </div>
            <div className="flex-1">
              <p className="font-medium">Erro ao processar</p>
              <p className="text-sm text-red-600">{error}</p>
            </div>
            <button
              type="button"
              onClick={() => setError(null)}
              className="p-1.5 hover:bg-red-100 rounded-lg transition-colors"
            >
              <X className="w-4 h-4" />
            </button>
          </div>
        )}

        {/* Tipos de Cadastro - Visual Moderno */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm p-5">
          <div className="flex items-center justify-between mb-4">
            <div>
              <h3 className="font-semibold text-gray-900">Tipo de Cadastro</h3>
              <p className="text-sm text-gray-500">Selecione uma ou mais categorias</p>
            </div>
            {formData.inativo && (
              <span className="px-3 py-1 bg-red-100 text-red-700 rounded-full text-sm font-medium">
                Cadastro Inativo
              </span>
            )}
          </div>

          <div className="flex flex-wrap gap-3">
            <CheckboxChip
              checked={formData.cliente}
              onChange={(v) => handleChange('cliente', v)}
              label="Cliente"
              icone={<User className="w-4 h-4" />}
              cor="blue"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.fornecedor}
              onChange={(v) => handleChange('fornecedor', v)}
              label="Fornecedor"
              icone={<Building2 className="w-4 h-4" />}
              cor="green"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.transportadora}
              onChange={(v) => handleChange('transportadora', v)}
              label="Transportadora"
              icone={<Truck className="w-4 h-4" />}
              cor="purple"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.vendedor}
              onChange={(v) => handleChange('vendedor', v)}
              label="Vendedor"
              icone={<UserCircle className="w-4 h-4" />}
              cor="orange"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.despesa}
              onChange={(v) => handleChange('despesa', v)}
              label="Despesa"
              icone={<Receipt className="w-4 h-4" />}
              cor="red"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.imposto}
              onChange={(v) => handleChange('imposto', v)}
              label="Imposto"
              icone={<CreditCard className="w-4 h-4" />}
              cor="amber"
              disabled={isViewMode}
            />

            <div className="border-l-2 border-gray-200 pl-3 ml-2">
              <CheckboxChip
                checked={formData.inativo}
                onChange={(v) => handleChange('inativo', v)}
                label="Inativo"
                icone={<X className="w-4 h-4" />}
                cor="red"
                disabled={isViewMode}
              />
            </div>
          </div>
        </div>

        {/* Abas Modernas */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden">
          {/* Tab Headers */}
          <div className="flex border-b border-gray-100 bg-gray-50/50">
            <button
              type="button"
              onClick={() => setActiveTab('dados')}
              className={`flex-1 flex items-center justify-center gap-2 px-6 py-4 text-sm font-medium transition-all duration-200 relative ${activeTab === 'dados'
                  ? 'text-blue-600 bg-white'
                  : 'text-gray-500 hover:text-gray-700 hover:bg-gray-50'
                }`}
            >
              <FileText className="w-4 h-4" />
              <span>Dados Principais</span>
              {activeTab === 'dados' && (
                <div className="absolute bottom-0 left-0 right-0 h-0.5 bg-gradient-to-r from-blue-500 to-cyan-500" />
              )}
            </button>
            <button
              type="button"
              onClick={() => setActiveTab('cobranca')}
              className={`flex-1 flex items-center justify-center gap-2 px-6 py-4 text-sm font-medium transition-all duration-200 relative ${activeTab === 'cobranca'
                  ? 'text-blue-600 bg-white'
                  : 'text-gray-500 hover:text-gray-700 hover:bg-gray-50'
                }`}
            >
              <Landmark className="w-4 h-4" />
              <span>Cobrança e Conta Corrente</span>
              {activeTab === 'cobranca' && (
                <div className="absolute bottom-0 left-0 right-0 h-0.5 bg-gradient-to-r from-blue-500 to-cyan-500" />
              )}
            </button>
          </div>

          {/* Tab Content */}
          <div className="p-6">
            {activeTab === 'dados' ? (
              <TabDadosPrincipais
                formData={formData}
                onChange={handleChange}
                vendedores={vendedores}
                onBuscarCnpj={handleBuscarCnpj}
                onBuscarCep={handleBuscarCep}
                buscandoCnpj={buscandoCnpj}
                buscandoCep={buscandoCep}
                cnpjEncontrado={cnpjEncontrado}
                cepEncontrado={cepEncontrado}
                isViewMode={isViewMode}
                dadosExtras={dadosExtras}
              />
            ) : (
              <TabCobrancaConta
                formData={formData}
                onChange={handleChange}
                isViewMode={isViewMode}
              />
            )}
          </div>
        </div>
      </form>
    </div>
  );
}

// ============================================================================
// ABA 1 - DADOS PRINCIPAIS
// ============================================================================
interface TabProps {
  formData: GeralCreateDto;
  onChange: (field: keyof GeralCreateDto, value: any) => void;
  vendedores?: GeralListDto[];
  onBuscarCnpj?: () => void;
  onBuscarCep?: () => void;
  buscandoCnpj?: boolean;
  buscandoCep?: boolean;
  cnpjEncontrado?: boolean;
  cepEncontrado?: boolean;
  isViewMode?: boolean;
  dadosExtras?: {
    municipioNome?: string;
    municipioUf?: string;
    vendedorNome?: string;
    dataDoCadastro?: string;
  };
}

function TabDadosPrincipais({
  formData,
  onChange,
  vendedores = [],
  onBuscarCnpj,
  onBuscarCep,
  buscandoCnpj,
  buscandoCep,
  cnpjEncontrado,
  cepEncontrado,
  isViewMode,
  // dadosExtras não é usado nesta aba atualmente
}: TabProps) {

  return (
    <div className="space-y-6">
      {/* Identificação */}
      <SecaoCard
        titulo="Identificação"
        subtitulo="Dados do documento e pessoa"
        icone={<FileText className="w-5 h-5" />}
      >
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <SelectModerno
            label="Tipo de Pessoa"
            value={formData.tipo}
            onChange={(v) => onChange('tipo', Number(v))}
            disabled={isViewMode}
            icone={<User className="w-4 h-4" />}
            opcoes={[
              { value: 0, label: 'Pessoa Física' },
              { value: 1, label: 'Pessoa Jurídica' },
            ]}
          />

          <InputModerno
            label={getLabelDocumento(formData.tipo)}
            value={formData.cpfECnpj}
            onChange={(v) => {
              // Aplica máscara de CPF/CNPJ de acordo com o tipo de pessoa
              const numeros = limparNumeros(v).slice(0, formData.tipo === 0 ? 11 : 14);
              // Usa formatação específica baseada no tipo (0=PF/CPF, 1=PJ/CNPJ)
              const formatado = formData.tipo === 0 ? formatarCPF(numeros) : formatarCNPJ(numeros);
              onChange('cpfECnpj', formatado);
            }}
            disabled={isViewMode}
            placeholder={formData.tipo === 0 ? '000.000.000-00' : '00.000.000/0000-00'}
            icone={<Hash className="w-4 h-4" />}
            sucesso={cnpjEncontrado}
            acaoDireita={
              formData.tipo === 1 && !isViewMode && (
                <button
                  type="button"
                  onClick={onBuscarCnpj}
                  disabled={buscandoCnpj || !formData.cpfECnpj || formData.cpfECnpj.replace(/\D/g, '').length < 14}
                  className="p-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all"
                  title="Buscar dados pelo CNPJ"
                >
                  {buscandoCnpj ? (
                    <Loader2 className="w-4 h-4 animate-spin" />
                  ) : cnpjEncontrado ? (
                    <CheckCircle2 className="w-4 h-4" />
                  ) : (
                    <Search className="w-4 h-4" />
                  )}
                </button>
              )
            }
          />

          <InputModerno
            label={getLabelIdentidade(formData.tipo)}
            value={formData.rgEIe}
            onChange={(v) => onChange('rgEIe', v)}
            disabled={isViewMode}
            icone={<FileText className="w-4 h-4" />}
          />

          <InputModerno
            label="Suframa"
            value={formData.codigoDoSuframa}
            onChange={(v) => onChange('codigoDoSuframa', v)}
            disabled={isViewMode}
            icone={<Hash className="w-4 h-4" />}
          />
        </div>

        {cnpjEncontrado && !isViewMode && (
          <div className="mt-3 p-3 bg-green-50 border border-green-200 rounded-xl flex items-center gap-2 text-green-700 text-sm">
            <CheckCircle2 className="w-4 h-4" />
            <span>Dados preenchidos automaticamente a partir do CNPJ</span>
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-4">
          <InputModerno
            label="Razão Social / Nome"
            value={formData.razaoSocial}
            onChange={(v) => onChange('razaoSocial', v)}
            disabled={isViewMode}
            required
            icone={<Building2 className="w-4 h-4" />}
          />

          <InputModerno
            label="Nome Fantasia"
            value={formData.nomeFantasia}
            onChange={(v) => onChange('nomeFantasia', v)}
            disabled={isViewMode}
            icone={<Building2 className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>

      {/* Endereço */}
      <SecaoCard
        titulo="Endereço"
        subtitulo="Localização principal"
        icone={<MapPin className="w-5 h-5" />}
      >
        <div className="grid grid-cols-1 md:grid-cols-12 gap-4">
          <div className="md:col-span-6">
            <InputModerno
              label="Logradouro"
              value={formData.endereco}
              onChange={(v) => onChange('endereco', v)}
              disabled={isViewMode}
              icone={<MapPin className="w-4 h-4" />}
            />
          </div>
          <div className="md:col-span-2">
            <InputModerno
              label="Número"
              value={formData.numeroDoEndereco}
              onChange={(v) => onChange('numeroDoEndereco', v)}
              disabled={isViewMode}
            />
          </div>
          <div className="md:col-span-4">
            <InputModerno
              label="Complemento"
              value={formData.complemento}
              onChange={(v) => onChange('complemento', v)}
              disabled={isViewMode}
            />
          </div>

          <div className="md:col-span-4">
            <InputModerno
              label="Bairro"
              value={formData.bairro}
              onChange={(v) => onChange('bairro', v)}
              disabled={isViewMode}
            />
          </div>
          <div className="md:col-span-3">
            <InputModerno
              label="CEP"
              value={formData.cep}
              onChange={(v) => {
                // Aplica máscara de CEP
                const numeros = limparNumeros(v).slice(0, 8);
                onChange('cep', formatarCEP(numeros));
              }}
              disabled={isViewMode}
              placeholder="00000-000"
              sucesso={cepEncontrado}
              acaoDireita={
                !isViewMode && (
                  <button
                    type="button"
                    onClick={onBuscarCep}
                    disabled={buscandoCep || !formData.cep || formData.cep.replace(/\D/g, '').length < 8}
                    className="p-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all"
                    title="Buscar endereço pelo CEP"
                  >
                    {buscandoCep ? (
                      <Loader2 className="w-4 h-4 animate-spin" />
                    ) : cepEncontrado ? (
                      <CheckCircle2 className="w-4 h-4" />
                    ) : (
                      <Search className="w-4 h-4" />
                    )}
                  </button>
                )
              }
            />
          </div>
          <div className="md:col-span-3">
            <InputModerno
              label="Cód. Município"
              value={formData.sequenciaDoMunicipio || ''}
              onChange={(v) => onChange('sequenciaDoMunicipio', Number(v) || 0)}
              disabled={isViewMode}
              type="number"
            />
          </div>
          <div className="md:col-span-2">
            <InputModerno
              label="Caixa Postal"
              value={formData.caixaPostal}
              onChange={(v) => onChange('caixaPostal', v)}
              disabled={isViewMode}
            />
          </div>
        </div>

        {cepEncontrado && (
          <div className="mt-3 p-3 bg-green-50 border border-green-200 rounded-xl flex items-center gap-2 text-green-700 text-sm">
            <CheckCircle2 className="w-4 h-4" />
            <span>Endereço preenchido automaticamente</span>
          </div>
        )}
      </SecaoCard>

      {/* Contato */}
      <SecaoCard
        titulo="Contato"
        subtitulo="Telefones e meios de comunicação"
        icone={<Phone className="w-5 h-5" />}
      >
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <InputModerno
            label="Telefone 1"
            value={formData.fone1}
            onChange={(v) => {
              const numeros = limparNumeros(v).slice(0, 11);
              onChange('fone1', formatarTelefone(numeros));
            }}
            disabled={isViewMode}
            icone={<Phone className="w-4 h-4" />}
          />
          <InputModerno
            label="Telefone 2"
            value={formData.fone2}
            onChange={(v) => {
              const numeros = limparNumeros(v).slice(0, 11);
              onChange('fone2', formatarTelefone(numeros));
            }}
            disabled={isViewMode}
            icone={<Phone className="w-4 h-4" />}
          />
          <InputModerno
            label="Celular"
            value={formData.celular}
            onChange={(v) => {
              const numeros = limparNumeros(v).slice(0, 11);
              onChange('celular', formatarTelefone(numeros));
            }}
            disabled={isViewMode}
            icone={<Smartphone className="w-4 h-4" />}
          />
          <InputModerno
            label="Fax"
            value={formData.fax}
            onChange={(v) => {
              const numeros = limparNumeros(v).slice(0, 11);
              onChange('fax', formatarTelefone(numeros));
            }}
            disabled={isViewMode}
            icone={<Phone className="w-4 h-4" />}
          />
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mt-4">
          <InputModerno
            label="Nome do Contato"
            value={formData.contato}
            onChange={(v) => onChange('contato', v)}
            disabled={isViewMode}
            icone={<User className="w-4 h-4" />}
          />
          <InputModerno
            label="E-mail"
            value={formData.email}
            onChange={(v) => onChange('email', v)}
            disabled={isViewMode}
            type="email"
            icone={<Mail className="w-4 h-4" />}
          />
          <InputModerno
            label="Site / Homepage"
            value={formData.homePage}
            onChange={(v) => onChange('homePage', v)}
            disabled={isViewMode}
            icone={<Globe className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>

      {/* Vendedor e Flags */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <SecaoCard
          titulo="Vendedor Responsável"
          icone={<UserCircle className="w-5 h-5" />}
        >
          <div className="space-y-4">
            <SelectModerno
              label="Vendedor"
              value={formData.sequenciaDoVendedor}
              onChange={(v) => onChange('sequenciaDoVendedor', Number(v))}
              disabled={isViewMode}
              icone={<UserCircle className="w-4 h-4" />}
              opcoes={[
                { value: 0, label: 'Nenhum' },
                ...vendedores.map((v) => ({
                  value: v.sequenciaDoGeral,
                  label: v.razaoSocial,
                })),
              ]}
            />
            <InputModerno
              label="Intermediário"
              value={formData.intermediarioDoVendedor}
              onChange={(v) => onChange('intermediarioDoVendedor', v)}
              disabled={isViewMode}
            />
          </div>
        </SecaoCard>

        <SecaoCard
          titulo="Flags Fiscais"
          icone={<FileText className="w-5 h-5" />}
        >
          <div className="grid grid-cols-2 gap-3">
            {[
              { campo: 'revenda', label: 'Revenda' },
              { campo: 'isento', label: 'Isento' },
              { campo: 'orgonPublico', label: 'Órgão Público' },
              { campo: 'empresaProdutor', label: 'Empresa Produtor' },
              { campo: 'cumulativo', label: 'Não Cumulativo' },
            ].map((flag) => (
              <label key={flag.campo} className="flex items-center gap-3 p-3 rounded-xl border border-gray-100 hover:bg-gray-50 cursor-pointer transition-colors">
                <input
                  type="checkbox"
                  checked={formData[flag.campo as keyof GeralCreateDto] as boolean}
                  onChange={(e) => onChange(flag.campo as keyof GeralCreateDto, e.target.checked)}
                  disabled={isViewMode}
                  className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                />
                <span className="text-sm font-medium text-gray-700">{flag.label}</span>
              </label>
            ))}
          </div>
        </SecaoCard>
      </div>

      {/* Observações */}
      <SecaoCard
        titulo="Observações"
        icone={<FileText className="w-5 h-5" />}
      >
        <textarea
          value={formData.observacao}
          onChange={(e) => onChange('observacao', e.target.value)}
          disabled={isViewMode}
          rows={4}
          placeholder="Digite observações adicionais sobre este cadastro..."
          className="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all resize-none disabled:bg-gray-50"
        />
      </SecaoCard>
    </div>
  );
}

// ============================================================================
// ABA 2 - COBRANÇA E CONTA CORRENTE
// ============================================================================
function TabCobrancaConta({ formData, onChange, isViewMode }: TabProps) {
  return (
    <div className="space-y-6">
      {/* Endereço de Cobrança */}
      <SecaoCard
        titulo="Endereço de Cobrança"
        subtitulo="Endereço alternativo para cobranças"
        icone={<MapPin className="w-5 h-5" />}
      >
        <div className="grid grid-cols-1 md:grid-cols-12 gap-4">
          <div className="md:col-span-6">
            <InputModerno
              label="Logradouro"
              value={formData.enderecoDeCobranca}
              onChange={(v) => onChange('enderecoDeCobranca', v)}
              disabled={isViewMode}
              icone={<MapPin className="w-4 h-4" />}
            />
          </div>
          <div className="md:col-span-2">
            <InputModerno
              label="Número"
              value={formData.numeroDoEnderecoDeCobranca}
              onChange={(v) => onChange('numeroDoEnderecoDeCobranca', v)}
              disabled={isViewMode}
            />
          </div>
          <div className="md:col-span-4">
            <InputModerno
              label="Complemento"
              value={formData.complementoDaCobranca}
              onChange={(v) => onChange('complementoDaCobranca', v)}
              disabled={isViewMode}
            />
          </div>
          <div className="md:col-span-4">
            <InputModerno
              label="Bairro"
              value={formData.bairroDeCobranca}
              onChange={(v) => onChange('bairroDeCobranca', v)}
              disabled={isViewMode}
            />
          </div>
          <div className="md:col-span-3">
            <InputModerno
              label="CEP"
              value={formData.cepDeCobranca}
              onChange={(v) => {
                const numeros = limparNumeros(v).slice(0, 8);
                onChange('cepDeCobranca', formatarCEP(numeros));
              }}
              disabled={isViewMode}
              placeholder="00000-000"
            />
          </div>
          <div className="md:col-span-3">
            <InputModerno
              label="Cód. Município"
              value={formData.sequenciaMunicipioCobranca || ''}
              onChange={(v) => onChange('sequenciaMunicipioCobranca', Number(v) || 0)}
              disabled={isViewMode}
              type="number"
            />
          </div>
          <div className="md:col-span-2">
            <InputModerno
              label="Caixa Postal"
              value={formData.caixaPostalDaCobranca}
              onChange={(v) => onChange('caixaPostalDaCobranca', v)}
              disabled={isViewMode}
            />
          </div>
        </div>
      </SecaoCard>

      {/* Conta Corrente 1 */}
      <SecaoCard
        titulo="Conta Corrente 1"
        subtitulo="Dados bancários principais"
        icone={<Landmark className="w-5 h-5" />}
      >
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <InputModerno
            label="Banco"
            value={formData.nomeDoBanco1}
            onChange={(v) => onChange('nomeDoBanco1', v)}
            disabled={isViewMode}
            icone={<Landmark className="w-4 h-4" />}
          />
          <InputModerno
            label="Agência"
            value={formData.agenciaDoBanco1}
            onChange={(v) => onChange('agenciaDoBanco1', v)}
            disabled={isViewMode}
            icone={<Hash className="w-4 h-4" />}
          />
          <InputModerno
            label="Conta Corrente"
            value={formData.contaCorrenteDoBanco1}
            onChange={(v) => onChange('contaCorrenteDoBanco1', v)}
            disabled={isViewMode}
            icone={<CreditCard className="w-4 h-4" />}
          />
          <InputModerno
            label="Nome do Correntista"
            value={formData.nomeDoCorrentistaDoBanco1}
            onChange={(v) => onChange('nomeDoCorrentistaDoBanco1', v)}
            disabled={isViewMode}
            icone={<User className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>

      {/* Conta Corrente 2 */}
      <SecaoCard
        titulo="Conta Corrente 2"
        subtitulo="Dados bancários secundários"
        icone={<Landmark className="w-5 h-5" />}
      >
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <InputModerno
            label="Banco"
            value={formData.nomeDoBanco2}
            onChange={(v) => onChange('nomeDoBanco2', v)}
            disabled={isViewMode}
            icone={<Landmark className="w-4 h-4" />}
          />
          <InputModerno
            label="Agência"
            value={formData.agenciaDoBanco2}
            onChange={(v) => onChange('agenciaDoBanco2', v)}
            disabled={isViewMode}
            icone={<Hash className="w-4 h-4" />}
          />
          <InputModerno
            label="Conta Corrente"
            value={formData.contaCorrenteDoBanco2}
            onChange={(v) => onChange('contaCorrenteDoBanco2', v)}
            disabled={isViewMode}
            icone={<CreditCard className="w-4 h-4" />}
          />
          <InputModerno
            label="Nome do Correntista"
            value={formData.nomeDoCorrentistaDoBanco2}
            onChange={(v) => onChange('nomeDoCorrentistaDoBanco2', v)}
            disabled={isViewMode}
            icone={<User className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>

      {/* Dados Adicionais */}
      <SecaoCard
        titulo="Dados Adicionais"
        subtitulo="Informações complementares"
        icone={<FileText className="w-5 h-5" />}
      >
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <InputModerno
            label="Código ANTT (Transportadora)"
            value={formData.codigoDaAntt}
            onChange={(v) => onChange('codigoDaAntt', v)}
            disabled={isViewMode}
            icone={<Truck className="w-4 h-4" />}
          />
          <InputModerno
            label="Data de Nascimento"
            value={formData.dataDeNascimento || ''}
            onChange={(v) => onChange('dataDeNascimento', v || undefined)}
            disabled={isViewMode}
            type="date"
            icone={<Calendar className="w-4 h-4" />}
          />
          <InputModerno
            label="Salário Bruto"
            value={formData.salBruto || ''}
            onChange={(v) => onChange('salBruto', Number(v) || 0)}
            disabled={isViewMode}
            type="number"
            icone={<Banknote className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>
    </div>
  );
}
