import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  ArrowLeft,
  Save,
  Search,
  Building2,
  MapPin,
  Phone,
  FileCheck,
  Loader2,
  AlertCircle,
  CheckCircle,
} from 'lucide-react';
import { emitenteService } from '../../services/emitenteService';
import type {
  EmitenteDto,
  EmitenteCreateUpdateDto,
} from '../../types/emitente';
import { REGIMES_TRIBUTARIOS, AMBIENTES_NFE, UFS } from '../../types/emitente';

// Função para formatar CNPJ
const formatCnpj = (value: string): string => {
  const digits = value.replace(/\D/g, '').slice(0, 14);
  if (digits.length <= 2) return digits;
  if (digits.length <= 5) return `${digits.slice(0, 2)}.${digits.slice(2)}`;
  if (digits.length <= 8) return `${digits.slice(0, 2)}.${digits.slice(2, 5)}.${digits.slice(5)}`;
  if (digits.length <= 12) return `${digits.slice(0, 2)}.${digits.slice(2, 5)}.${digits.slice(5, 8)}/${digits.slice(8)}`;
  return `${digits.slice(0, 2)}.${digits.slice(2, 5)}.${digits.slice(5, 8)}/${digits.slice(8, 12)}-${digits.slice(12)}`;
};

// Função para formatar CEP
const formatCep = (value: string): string => {
  const digits = value.replace(/\D/g, '').slice(0, 8);
  if (digits.length <= 5) return digits;
  return `${digits.slice(0, 5)}-${digits.slice(5)}`;
};

// Função para formatar telefone
const formatTelefone = (value: string): string => {
  const digits = value.replace(/\D/g, '').slice(0, 11);
  if (digits.length <= 2) return digits;
  if (digits.length <= 6) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;
  if (digits.length <= 10) return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`;
  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`;
};

export default function EmitentePage() {
  const navigate = useNavigate();

  // Estados
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [consultandoCnpj, setConsultandoCnpj] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [emitenteExistente, setEmitenteExistente] = useState<EmitenteDto | null>(null);
  const [touched, setTouched] = useState<Set<string>>(new Set()); // Campos tocados para validação

  // Form data
  const [formData, setFormData] = useState<EmitenteCreateUpdateDto>({
    cnpj: '',
    razaoSocial: '',
    nomeFantasia: '',
    inscricaoEstadual: '',
    inscricaoMunicipal: '',
    cnae: '',
    codigoRegimeTributario: 3, // Regime Normal padrão
    endereco: '',
    numero: '',
    complemento: '',
    bairro: '',
    codigoMunicipio: '',
    municipio: '',
    uf: 'SP',
    cep: '',
    codigoPais: '1058',
    pais: 'Brasil',
    telefone: '',
    email: '',
    ambienteNfe: 2, // Homologação padrão
    serieNfe: 1,
    proximoNumeroNfe: 1,
    caminhoCertificado: '',
    senhaCertificado: '',
    ativo: true,
  });

  // Carrega emitente existente ao montar
  useEffect(() => {
    carregarEmitente();
  }, []);

  const carregarEmitente = async () => {
    try {
      setLoading(true);
      const emitente = await emitenteService.obterAtual();
      setEmitenteExistente(emitente);
      
      // Preenche o formulário com dados existentes
      setFormData({
        cnpj: emitente.cnpj,
        razaoSocial: emitente.razaoSocial,
        nomeFantasia: emitente.nomeFantasia || '',
        inscricaoEstadual: emitente.inscricaoEstadual,
        inscricaoMunicipal: emitente.inscricaoMunicipal || '',
        cnae: emitente.cnae || '',
        codigoRegimeTributario: emitente.codigoRegimeTributario || 3,
        endereco: emitente.endereco,
        numero: emitente.numero,
        complemento: emitente.complemento || '',
        bairro: emitente.bairro,
        codigoMunicipio: emitente.codigoMunicipio,
        municipio: emitente.municipio,
        uf: emitente.uf,
        cep: emitente.cep,
        codigoPais: emitente.codigoPais || '1058',
        pais: emitente.pais || 'Brasil',
        telefone: emitente.telefone || '',
        email: emitente.email || '',
        ambienteNfe: emitente.ambienteNfe,
        serieNfe: emitente.serieNfe,
        proximoNumeroNfe: emitente.proximoNumeroNfe,
        caminhoCertificado: emitente.caminhoCertificado || '',
        senhaCertificado: '',
        ativo: emitente.ativo,
      });
    } catch (err: any) {
      // Se não encontrou emitente, é um novo cadastro
      if (err?.response?.status === 404) {
        setEmitenteExistente(null);
      } else {
        console.error('Erro ao carregar emitente:', err);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleConsultarCnpj = async () => {
    const cnpjLimpo = formData.cnpj.replace(/\D/g, '');
    
    if (cnpjLimpo.length !== 14) {
      setError('CNPJ deve ter 14 dígitos');
      return;
    }

    try {
      setConsultandoCnpj(true);
      setError(null);
      
      const dados = await emitenteService.consultarCnpj(cnpjLimpo);
      
      // Preenche o formulário com os dados da consulta
      setFormData(prev => ({
        ...prev,
        razaoSocial: dados.razaoSocial || prev.razaoSocial,
        nomeFantasia: dados.nomeFantasia || prev.nomeFantasia,
        endereco: dados.endereco || prev.endereco,
        numero: dados.numero || prev.numero,
        complemento: dados.complemento || prev.complemento,
        bairro: dados.bairro || prev.bairro,
        codigoMunicipio: dados.codigoMunicipio || prev.codigoMunicipio,
        municipio: dados.municipio || prev.municipio,
        uf: dados.uf || prev.uf,
        cep: dados.cep || prev.cep,
        telefone: dados.telefone || prev.telefone,
        email: dados.email || prev.email,
        cnae: dados.cnae || prev.cnae,
      }));

      setSuccess('Dados carregados da Receita Federal com sucesso!');
      setTimeout(() => setSuccess(null), 5000);
    } catch (err: any) {
      setError(err?.response?.data?.message || 'Erro ao consultar CNPJ');
    } finally {
      setConsultandoCnpj(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      setSaving(true);
      setError(null);

      // Remove formatação do CNPJ e CEP
      const dadosParaSalvar = {
        ...formData,
        cnpj: formData.cnpj.replace(/\D/g, ''),
        cep: formData.cep.replace(/\D/g, ''),
        telefone: formData.telefone?.replace(/\D/g, '') || '',
      };

      if (emitenteExistente) {
        await emitenteService.atualizar(emitenteExistente.id, dadosParaSalvar);
        setSuccess('Emitente atualizado com sucesso!');
      } else {
        const novoEmitente = await emitenteService.criar(dadosParaSalvar);
        setEmitenteExistente(novoEmitente as any);
        setSuccess('Emitente cadastrado com sucesso!');
      }

      setTimeout(() => setSuccess(null), 5000);
    } catch (err: any) {
      setError(err?.response?.data?.message || 'Erro ao salvar emitente');
    } finally {
      setSaving(false);
    }
  };

  const handleChange = (field: keyof EmitenteCreateUpdateDto, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  // Marcar campo como tocado ao sair dele
  const handleBlur = (field: string) => {
    setTouched(prev => new Set(prev).add(field));
  };

  // Verificar se campo é inválido (tocado + vazio + obrigatório)
  const isFieldInvalid = (field: string, value: string | undefined) => {
    return touched.has(field) && !value;
  };

  // Classe CSS para campos com validação
  const getInputClassName = (field: string, value: string | undefined, isRequired: boolean = false) => {
    const baseClass = "w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors";
    if (isRequired && isFieldInvalid(field, value)) {
      return `${baseClass} border-red-300 bg-red-50`;
    }
    return `${baseClass} border-gray-300`;
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <Loader2 className="h-8 w-8 animate-spin text-blue-600" />
        <span className="ml-2 text-gray-600">Carregando dados do emitente...</span>
      </div>
    );
  }

  return (
    <div className="space-y-4 sm:space-y-6 p-4 sm:p-0">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div className="flex items-center gap-3 sm:gap-4">
          <button
            onClick={() => navigate(-1)}
            className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
          >
            <ArrowLeft className="h-5 w-5 text-gray-600" />
          </button>
          <div>
            <h1 className="text-xl sm:text-2xl font-bold text-gray-900">
              Dados do Emitente
            </h1>
            <p className="text-xs sm:text-sm text-gray-500">
              Empresa que emite as notas fiscais
            </p>
          </div>
        </div>
      </div>

      {/* Toast de Sucesso - Fixo no topo */}
      {success && (
        <div className="fixed top-4 right-4 z-50 animate-slide-in-right">
          <div className="bg-green-600 text-white px-6 py-4 rounded-xl shadow-2xl flex items-center gap-3 min-w-[320px]">
            <div className="bg-white/20 rounded-full p-1">
              <CheckCircle className="h-6 w-6 text-white" />
            </div>
            <div>
              <p className="font-semibold">Sucesso!</p>
              <p className="text-sm text-green-100">{success}</p>
            </div>
            <button 
              onClick={() => setSuccess(null)}
              className="ml-auto text-white/70 hover:text-white transition-colors"
            >
              ✕
            </button>
          </div>
        </div>
      )}

      {/* Alerta de Erro */}
      {error && (
        <div className="bg-red-50 border-l-4 border-red-500 rounded-r-lg p-4 flex items-center gap-3 animate-shake">
          <AlertCircle className="h-5 w-5 text-red-600 flex-shrink-0" />
          <div className="flex-1">
            <p className="font-medium text-red-800">Erro</p>
            <p className="text-red-700 text-sm">{error}</p>
          </div>
          <button 
            onClick={() => setError(null)}
            className="text-red-400 hover:text-red-600 transition-colors"
          >
            ✕
          </button>
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4 sm:space-y-6">
        {/* Seção: Dados da Empresa */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
          <div className="bg-blue-50 px-4 sm:px-6 py-4 border-b border-blue-100">
            <div className="flex items-center gap-2">
              <Building2 className="h-5 w-5 text-blue-600" />
              <h2 className="text-lg font-semibold text-blue-900">Dados da Empresa</h2>
            </div>
          </div>

          <div className="p-4 sm:p-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            {/* CNPJ com botão de consulta */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                CNPJ *
              </label>
              <div className="flex gap-2">
                <input
                  type="text"
                  value={formatCnpj(formData.cnpj)}
                  onChange={(e) => handleChange('cnpj', e.target.value.replace(/\D/g, ''))}
                  className="flex-1 px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  placeholder="00.000.000/0000-00"
                  maxLength={18}
                  required
                />
                <button
                  type="button"
                  onClick={handleConsultarCnpj}
                  disabled={consultandoCnpj || formData.cnpj.replace(/\D/g, '').length !== 14}
                  className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-300 disabled:cursor-not-allowed flex items-center gap-2"
                  title="Consultar CNPJ na Receita Federal"
                >
                  {consultandoCnpj ? (
                    <Loader2 className="h-4 w-4 animate-spin" />
                  ) : (
                    <Search className="h-4 w-4" />
                  )}
                </button>
              </div>
            </div>

            {/* Razão Social */}
            <div className="lg:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Razão Social *
              </label>
              <input
                type="text"
                value={formData.razaoSocial}
                onChange={(e) => handleChange('razaoSocial', e.target.value)}
                onBlur={() => handleBlur('razaoSocial')}
                className={getInputClassName('razaoSocial', formData.razaoSocial, true)}
                maxLength={60}
                required
              />
              {isFieldInvalid('razaoSocial', formData.razaoSocial) && (
                <p className="text-xs text-red-500 mt-1">Razão Social é obrigatória</p>
              )}
            </div>

            {/* Nome Fantasia */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Nome Fantasia
              </label>
              <input
                type="text"
                value={formData.nomeFantasia || ''}
                onChange={(e) => handleChange('nomeFantasia', e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                maxLength={60}
              />
            </div>

            {/* Inscrição Estadual */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Inscrição Estadual *
              </label>
              <input
                type="text"
                value={formData.inscricaoEstadual}
                onChange={(e) => handleChange('inscricaoEstadual', e.target.value.replace(/\D/g, ''))}
                onBlur={() => handleBlur('inscricaoEstadual')}
                className={getInputClassName('inscricaoEstadual', formData.inscricaoEstadual, true)}
                maxLength={20}
                required
              />
              {isFieldInvalid('inscricaoEstadual', formData.inscricaoEstadual) && (
                <p className="text-xs text-red-500 mt-1">Inscrição Estadual é obrigatória</p>
              )}
            </div>

            {/* Inscrição Municipal */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Inscrição Municipal
              </label>
              <input
                type="text"
                value={formData.inscricaoMunicipal || ''}
                onChange={(e) => handleChange('inscricaoMunicipal', e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                maxLength={20}
              />
            </div>

            {/* CNAE */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                CNAE
              </label>
              <input
                type="text"
                value={formData.cnae || ''}
                onChange={(e) => handleChange('cnae', e.target.value.replace(/\D/g, ''))}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                maxLength={10}
                placeholder="Ex: 4789099"
              />
            </div>

            {/* Regime Tributário */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Regime Tributário
              </label>
              <select
                value={formData.codigoRegimeTributario || ''}
                onChange={(e) => handleChange('codigoRegimeTributario', e.target.value ? Number(e.target.value) : undefined)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              >
                <option value="">Selecione...</option>
                {REGIMES_TRIBUTARIOS.map(regime => (
                  <option key={regime.value} value={regime.value}>
                    {regime.label}
                  </option>
                ))}
              </select>
            </div>
          </div>
        </div>

        {/* Seção: Endereço */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
          <div className="bg-green-50 px-4 sm:px-6 py-4 border-b border-green-100">
            <div className="flex items-center gap-2">
              <MapPin className="h-5 w-5 text-green-600" />
              <h2 className="text-lg font-semibold text-green-900">Endereço</h2>
            </div>
          </div>

          <div className="p-4 sm:p-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
            {/* CEP */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                CEP *
              </label>
              <input
                type="text"
                value={formatCep(formData.cep)}
                onChange={(e) => handleChange('cep', e.target.value.replace(/\D/g, ''))}
                onBlur={() => handleBlur('cep')}
                className={getInputClassName('cep', formData.cep, true)}
                placeholder="00000-000"
                maxLength={9}
                required
              />
            </div>

            {/* Endereço */}
            <div className="lg:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Endereço *
              </label>
              <input
                type="text"
                value={formData.endereco}
                onChange={(e) => handleChange('endereco', e.target.value)}
                onBlur={() => handleBlur('endereco')}
                className={getInputClassName('endereco', formData.endereco, true)}
                maxLength={100}
                required
              />
            </div>

            {/* Número */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Número *
              </label>
              <input
                type="text"
                value={formData.numero}
                onChange={(e) => handleChange('numero', e.target.value)}
                onBlur={() => handleBlur('numero')}
                className={getInputClassName('numero', formData.numero, true)}
                maxLength={10}
                required
              />
            </div>

            {/* Complemento */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Complemento
              </label>
              <input
                type="text"
                value={formData.complemento || ''}
                onChange={(e) => handleChange('complemento', e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                maxLength={60}
              />
            </div>

            {/* Bairro */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Bairro *
              </label>
              <input
                type="text"
                value={formData.bairro}
                onChange={(e) => handleChange('bairro', e.target.value)}
                onBlur={() => handleBlur('bairro')}
                className={getInputClassName('bairro', formData.bairro, true)}
                maxLength={60}
                required
              />
            </div>

            {/* Município */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Município *
              </label>
              <input
                type="text"
                value={formData.municipio}
                onChange={(e) => handleChange('municipio', e.target.value)}
                onBlur={() => handleBlur('municipio')}
                className={getInputClassName('municipio', formData.municipio, true)}
                maxLength={60}
                required
              />
            </div>

            {/* UF */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                UF *
              </label>
              <select
                value={formData.uf}
                onChange={(e) => handleChange('uf', e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                required
              >
                {UFS.map(uf => (
                  <option key={uf} value={uf}>{uf}</option>
                ))}
              </select>
            </div>

            {/* Código IBGE */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Código IBGE *
              </label>
              <input
                type="text"
                value={formData.codigoMunicipio}
                onChange={(e) => handleChange('codigoMunicipio', e.target.value.replace(/\D/g, ''))}
                onBlur={() => handleBlur('codigoMunicipio')}
                className={getInputClassName('codigoMunicipio', formData.codigoMunicipio, true)}
                placeholder="7 dígitos"
                maxLength={7}
                required
              />
            </div>
          </div>
        </div>

        {/* Seção: Contato */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
          <div className="bg-purple-50 px-4 sm:px-6 py-4 border-b border-purple-100">
            <div className="flex items-center gap-2">
              <Phone className="h-5 w-5 text-purple-600" />
              <h2 className="text-lg font-semibold text-purple-900">Contato</h2>
            </div>
          </div>

          <div className="p-4 sm:p-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
            {/* Telefone */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Telefone
              </label>
              <input
                type="text"
                value={formatTelefone(formData.telefone || '')}
                onChange={(e) => handleChange('telefone', e.target.value.replace(/\D/g, ''))}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                placeholder="(00) 00000-0000"
                maxLength={15}
              />
            </div>

            {/* Email */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Email
              </label>
              <input
                type="email"
                value={formData.email || ''}
                onChange={(e) => handleChange('email', e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                maxLength={255}
              />
            </div>
          </div>
        </div>

        {/* Seção: Configurações NFe */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
          <div className="bg-orange-50 px-4 sm:px-6 py-4 border-b border-orange-100">
            <div className="flex items-center gap-2">
              <FileCheck className="h-5 w-5 text-orange-600" />
              <h2 className="text-lg font-semibold text-orange-900">Configurações NFe</h2>
            </div>
          </div>

          <div className="p-4 sm:p-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
            {/* Ambiente */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Ambiente NFe
              </label>
              <select
                value={formData.ambienteNfe}
                onChange={(e) => handleChange('ambienteNfe', Number(e.target.value))}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              >
                {AMBIENTES_NFE.map(ambiente => (
                  <option key={ambiente.value} value={ambiente.value}>
                    {ambiente.label}
                  </option>
                ))}
              </select>
            </div>

            {/* Série */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Série NFe
              </label>
              <input
                type="number"
                value={formData.serieNfe}
                onChange={(e) => handleChange('serieNfe', Number(e.target.value))}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                min={1}
                max={999}
              />
            </div>

            {/* Caminho Certificado */}
            <div className="lg:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Caminho do Certificado Digital
              </label>
              <input
                type="text"
                value={formData.caminhoCertificado || ''}
                onChange={(e) => handleChange('caminhoCertificado', e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                placeholder="C:\certificados\certificado.pfx"
              />
            </div>

            {/* Senha Certificado */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Senha do Certificado
              </label>
              <input
                type="password"
                value={formData.senhaCertificado || ''}
                onChange={(e) => handleChange('senhaCertificado', e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                placeholder={emitenteExistente ? '(não alterado)' : ''}
              />
            </div>

            {/* Ativo */}
            <div className="flex items-center">
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="checkbox"
                  checked={formData.ativo}
                  onChange={(e) => handleChange('ativo', e.target.checked)}
                  className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                />
                <span className="text-sm font-medium text-gray-700">Emitente Ativo</span>
              </label>
            </div>
          </div>
        </div>

        {/* Botões */}
        <div className="flex flex-col-reverse sm:flex-row justify-end gap-3">
          <button
            type="button"
            onClick={() => navigate(-1)}
            className="px-6 py-2.5 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Cancelar
          </button>
          <button
            type="submit"
            disabled={saving}
            className="px-6 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-400 transition-colors flex items-center justify-center gap-2"
          >
            {saving ? (
              <>
                <Loader2 className="h-4 w-4 animate-spin" />
                Salvando...
              </>
            ) : (
              <>
                <Save className="h-4 w-4" />
                {emitenteExistente ? 'Atualizar' : 'Cadastrar'}
              </>
            )}
          </button>
        </div>
      </form>
    </div>
  );
}
