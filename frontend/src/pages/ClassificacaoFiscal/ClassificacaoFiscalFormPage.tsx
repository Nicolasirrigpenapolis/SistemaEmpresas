import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  ArrowLeft,
  Save,
  AlertCircle,
  CheckCircle,
  FileText,
  Settings2,
  Calculator,
  Hash,
  Percent,
  Package,
  X,
  Loader2,
} from 'lucide-react';
import { classificacaoFiscalService } from '../../services/Fiscal/classificacaoFiscalService';
import { classTribService } from '../../services/Fiscal/classTribService';
import type { ClassificacaoFiscalInput } from '../../types';
import { DEFAULT_CLASSIFICACAO_FISCAL } from '../../types';
import ClassTribSelector from '../../components/ClassTribSelector';

// ============================================================================
// COMPONENTE PRINCIPAL
// ============================================================================
export default function ClassificacaoFiscalFormPage() {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isNew = id === 'novo';
  
  // Estados
  const [formData, setFormData] = useState<ClassificacaoFiscalInput>(DEFAULT_CLASSIFICACAO_FISCAL);
  const [loading, setLoading] = useState(!isNew);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  // ============================================================================
  // FUNÇÕES
  // ============================================================================
  const loadData = async () => {
    if (isNew) return;
    
    try {
      setLoading(true);
      setError(null);
      
      const data = await classificacaoFiscalService.buscarPorId(Number(id));
      const { sequenciaDaClassificacao, classTribNavigation, ...formFields } = data as any;
      
      // Mapeia classTribNavigation para classTrib (nome usado no frontend)
      setFormData({
        ...formFields,
        classTrib: classTribNavigation || null,
      });
    } catch (err: any) {
      console.error('Erro ao carregar classificação fiscal:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      setSaving(true);
      setError(null);
      setSuccess(null);
      
      // Backend valida campos obrigatórios (NCM, descrição)
      if (isNew) {
        await classificacaoFiscalService.criar(formData);
        setSuccess('Classificação fiscal criada com sucesso!');
      } else {
        await classificacaoFiscalService.atualizar(Number(id), formData);
        setSuccess('Classificação fiscal atualizada com sucesso!');
      }
      
      // Redirecionar para listagem após salvar
      setTimeout(() => navigate('/classificacao-fiscal'), 1000);
    } catch (err: any) {
      console.error('Erro ao salvar:', err);
      setError(err.response?.data?.mensagem || 'Erro ao salvar classificação fiscal');
    } finally {
      setSaving(false);
    }
  };

  const handleChange = (field: keyof ClassificacaoFiscalInput, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  // Função para buscar detalhes completos do ClassTrib selecionado
  const fetchClassTribDetails = async (classTribId: number) => {
    try {
      const details = await classTribService.buscarPorId(classTribId);
      setFormData(prev => ({
        ...prev,
        classTrib: {
          id: details.id,
          codigoClassTrib: details.codigoClassTrib,
          codigoSituacaoTributaria: details.codigoSituacaoTributaria,
          descricaoSituacaoTributaria: details.descricaoSituacaoTributaria,
          descricaoClassTrib: details.descricaoClassTrib,
          percentualReducaoIBS: details.percentualReducaoIBS,
          percentualReducaoCBS: details.percentualReducaoCBS,
          tipoAliquota: details.tipoAliquota,
          validoParaNFe: details.validoParaNFe,
          tributacaoRegular: details.tributacaoRegular,
          creditoPresumidoOperacoes: details.creditoPresumidoOperacoes,
          estornoCredito: details.estornoCredito,
          anexoLegislacao: details.anexoLegislacao,
          linkLegislacao: details.linkLegislacao,
        }
      }));
    } catch (error) {
      console.error('Erro ao buscar detalhes do ClassTrib:', error);
    }
  };

  // ============================================================================
  // EFFECTS
  // ============================================================================
  useEffect(() => {
    loadData();
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
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <button
            onClick={() => navigate('/classificacao-fiscal')}
            className="p-2.5 text-gray-600 hover:text-gray-900 hover:bg-gray-100 rounded-xl transition-all"
          >
            <ArrowLeft className="h-5 w-5" />
          </button>
          
          <div>
            <div className="flex items-center gap-2">
              <div className="p-1.5 bg-blue-100 rounded-lg">
                <FileText className="w-4 h-4 text-blue-600" />
              </div>
              <h1 className="text-xl font-bold text-gray-900">
                {isNew ? 'Nova Classificação Fiscal' : `Classificação Fiscal`}
              </h1>
            </div>
            <div className="flex items-center gap-2 text-sm text-gray-500 mt-0.5">
              {!isNew && (
                <>
                  <Hash className="w-3.5 h-3.5" />
                  <span>Código: {id}</span>
                  <span className="opacity-50">•</span>
                </>
              )}
              <span>{isNew ? 'Preencha os dados abaixo' : 'Editar informações'}</span>
            </div>
          </div>
        </div>
        
        <div className="flex items-center gap-3">
          <button
            type="button"
            onClick={() => navigate('/classificacao-fiscal')}
            className="px-4 py-2.5 text-gray-600 hover:text-gray-800 hover:bg-gray-100 rounded-xl transition-all text-sm font-medium"
          >
            Cancelar
          </button>
          
          <button
            onClick={handleSubmit}
            disabled={saving}
            className="flex items-center gap-2 px-5 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-xl hover:from-blue-700 hover:to-blue-800 shadow-lg shadow-blue-500/30 transition-all text-sm font-medium disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {saving ? (
              <>
                <Loader2 className="h-4 w-4 animate-spin" />
                Salvando...
              </>
            ) : (
              <>
                <Save className="h-4 w-4" />
                Salvar
              </>
            )}
          </button>
        </div>
      </div>

      {/* Mensagens */}
      {error && (
        <div className="p-4 bg-red-50 border border-red-200 rounded-xl flex items-center gap-3 text-red-700 animate-in slide-in-from-top-2 duration-300">
          <div className="p-2 bg-red-100 rounded-lg">
            <AlertCircle className="h-5 w-5" />
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

        {success && (
          <div className="mb-4 p-4 bg-emerald-50 border border-emerald-200 rounded-xl flex items-center gap-3 text-emerald-700 animate-in slide-in-from-top-2 duration-300">
            <div className="p-2 bg-emerald-100 rounded-lg">
              <CheckCircle className="h-5 w-5" />
            </div>
            <p className="font-medium">{success}</p>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-6">
          {/* Dados Básicos */}
          <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden">
            <div className="px-6 py-4 border-b border-gray-100 bg-gradient-to-r from-gray-50 to-white">
              <div className="flex items-center gap-3">
                <div className="p-2 bg-blue-50 rounded-lg text-blue-600">
                  <Package className="w-5 h-5" />
                </div>
                <div>
                  <h2 className="font-semibold text-gray-900">Dados Básicos</h2>
                  <p className="text-xs text-gray-500">Informações principais da classificação</p>
                </div>
              </div>
            </div>
            
            <div className="p-6">
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
                <div className="relative">
                  <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
                    <Hash className="w-4 h-4" />
                  </div>
                  <input
                    type="number"
                    value={formData.ncm || ''}
                    onChange={(e) => handleChange('ncm', Number(e.target.value))}
                    placeholder="84212100"
                    className="w-full pl-10 pr-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                    required
                  />
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">
                    NCM <span className="text-red-500">*</span>
                  </label>
                </div>
                
                <div className="md:col-span-2 relative">
                  <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
                    <FileText className="w-4 h-4" />
                  </div>
                  <input
                    type="text"
                    value={formData.descricaoDoNcm}
                    onChange={(e) => handleChange('descricaoDoNcm', e.target.value)}
                    placeholder="Descrição da classificação fiscal"
                    maxLength={100}
                    className="w-full pl-10 pr-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                    required
                  />
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">
                    Descrição do NCM <span className="text-red-500">*</span>
                  </label>
                </div>
                
                <div className="relative">
                  <input
                    type="text"
                    value={formData.cest}
                    onChange={(e) => handleChange('cest', e.target.value)}
                    placeholder="1234567"
                    maxLength={7}
                    className="w-full px-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                  />
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">CEST</label>
                </div>
                
                <div className="relative">
                  <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
                    <Percent className="w-4 h-4" />
                  </div>
                  <input
                    type="number"
                    step="0.0001"
                    value={formData.porcentagemDoIpi}
                    onChange={(e) => handleChange('porcentagemDoIpi', Number(e.target.value))}
                    className="w-full pl-10 pr-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                  />
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">IPI (%)</label>
                </div>
                
                <div className="relative">
                  <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
                    <Percent className="w-4 h-4" />
                  </div>
                  <input
                    type="number"
                    step="0.0001"
                    value={formData.iva}
                    onChange={(e) => handleChange('iva', Number(e.target.value))}
                    className="w-full pl-10 pr-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                  />
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">IVA</label>
                </div>
                
                <div className="relative">
                  <input
                    type="text"
                    value={formData.unExterior}
                    onChange={(e) => handleChange('unExterior', e.target.value)}
                    placeholder="UN, KG, etc."
                    maxLength={10}
                    className="w-full px-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                  />
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">Unidade Exterior</label>
                </div>
                
                <div className="flex items-center">
                  <label className="flex items-center gap-3 p-3 rounded-xl border border-gray-100 hover:bg-gray-50 cursor-pointer transition-colors w-full">
                    <input
                      type="checkbox"
                      checked={formData.inativo}
                      onChange={(e) => handleChange('inativo', e.target.checked)}
                      className="w-4 h-4 text-red-600 border-gray-300 rounded focus:ring-red-500"
                    />
                    <span className="text-sm font-medium text-gray-700">Inativo</span>
                  </label>
                </div>
              </div>
            </div>
          </div>

          {/* Configurações Fiscais */}
          <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden">
            <div className="px-6 py-4 border-b border-gray-100 bg-gradient-to-r from-gray-50 to-white">
              <div className="flex items-center gap-3">
                <div className="p-2 bg-amber-50 rounded-lg text-amber-600">
                  <Settings2 className="w-5 h-5" />
                </div>
                <div>
                  <h2 className="font-semibold text-gray-900">Configurações Fiscais</h2>
                  <p className="text-xs text-gray-500">Parâmetros de cálculo e reduções</p>
                </div>
              </div>
            </div>
            
            <div className="p-6">
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5">
                <div className="relative">
                  <select
                    value={formData.anexoDaReducao}
                    onChange={(e) => handleChange('anexoDaReducao', Number(e.target.value))}
                    className="w-full px-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all appearance-none cursor-pointer"
                  >
                    <option value={0}>Anexo 1</option>
                    <option value={1}>Anexo 2</option>
                  </select>
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">Anexo da Redução</label>
                </div>
                
                <div className="relative">
                  <select
                    value={formData.aliquotaDoAnexo}
                    onChange={(e) => handleChange('aliquotaDoAnexo', Number(e.target.value))}
                    className="w-full px-3 py-3 bg-white border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all appearance-none cursor-pointer"
                  >
                    <option value={2}>7%</option>
                    <option value={0}>12%</option>
                    <option value={1}>18%</option>
                  </select>
                  <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">Alíquota do Anexo</label>
                </div>
                
                <div className="lg:col-span-2 flex flex-wrap items-center gap-3">
                  {[
                    { campo: 'produtoDiferido', label: 'Produto Diferido' },
                    { campo: 'reducaoDeBaseDeCalculo', label: 'Redução de Base' },
                    { campo: 'temConvenio', label: 'Tem Convênio' },
                  ].map((flag) => (
                    <label 
                      key={flag.campo}
                      className="flex items-center gap-2 px-4 py-2.5 rounded-xl border border-gray-100 hover:bg-gray-50 cursor-pointer transition-colors"
                    >
                      <input
                        type="checkbox"
                        checked={formData[flag.campo as keyof ClassificacaoFiscalInput] as boolean}
                        onChange={(e) => handleChange(flag.campo as keyof ClassificacaoFiscalInput, e.target.checked)}
                        className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                      />
                      <span className="text-sm font-medium text-gray-700">{flag.label}</span>
                    </label>
                  ))}
                </div>
              </div>
            </div>
          </div>

          {/* Tributação IBS/CBS */}
          <div className="bg-white rounded-2xl border border-gray-100 shadow-sm">
            <div className="px-6 py-4 border-b border-gray-100 bg-gradient-to-r from-gray-50 to-white rounded-t-2xl">
              <div className="flex items-center gap-3">
                <div className="p-2 bg-indigo-50 rounded-lg text-indigo-600">
                  <Calculator className="w-5 h-5" />
                </div>
                <div>
                  <h2 className="font-semibold text-gray-900">Tributação IBS/CBS</h2>
                  <p className="text-xs text-gray-500">Classificação conforme a Reforma Tributária</p>
                </div>
              </div>
            </div>
            
            <div className="p-6 space-y-5">
              {/* Seletor */}
              <div className="relative">
                <ClassTribSelector
                  value={formData.classTribId}
                  initialDisplayText={
                    formData.classTrib
                      ? `${formData.classTrib.codigoClassTrib} - CST ${formData.classTrib.codigoSituacaoTributaria}`
                      : undefined
                  }
                  onChange={(id, _classTribData) => {
                    handleChange('classTribId', id);
                    if (!id) {
                      handleChange('classTrib', null);
                      return;
                    }
                    if (id) {
                      fetchClassTribDetails(id);
                    }
                  }}
                />
              </div>

              {/* Dados da tributação selecionada */}
              {formData.classTrib && (
                <div className="bg-gradient-to-br from-indigo-50/50 to-blue-50/30 rounded-xl p-5 border border-indigo-100/50">
                  <h3 className="text-sm font-semibold text-indigo-900 mb-4 flex items-center gap-2">
                    <CheckCircle className="h-4 w-4 text-indigo-500" />
                    Tributação Selecionada
                  </h3>
                  <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
                    <div className="bg-white/80 rounded-lg p-3 border border-indigo-100/50">
                      <span className="block text-indigo-400 text-xs uppercase tracking-wide mb-1">Código</span>
                      <span className="inline-flex items-center px-2 py-1 bg-indigo-100 text-indigo-700 text-sm font-mono font-medium rounded">
                        {formData.classTrib.codigoClassTrib}
                      </span>
                    </div>
                    <div className="bg-white/80 rounded-lg p-3 border border-indigo-100/50">
                      <span className="block text-indigo-400 text-xs uppercase tracking-wide mb-1">CST</span>
                      <span className="font-semibold text-gray-900 text-lg">{formData.classTrib.codigoSituacaoTributaria}</span>
                    </div>
                    <div className="bg-white/80 rounded-lg p-3 border border-indigo-100/50">
                      <span className="block text-indigo-400 text-xs uppercase tracking-wide mb-1">Tipo Alíquota</span>
                      <span className="text-gray-700 font-medium">{formData.classTrib.tipoAliquota || '-'}</span>
                    </div>
                    <div className="bg-white/80 rounded-lg p-3 border border-indigo-100/50">
                      <span className="block text-indigo-400 text-xs uppercase tracking-wide mb-1">Redução IBS</span>
                      <span className="font-semibold text-gray-900">
                        {formData.classTrib.percentualReducaoIBS?.toFixed(2)}%
                      </span>
                    </div>
                    <div className="bg-white/80 rounded-lg p-3 border border-indigo-100/50">
                      <span className="block text-indigo-400 text-xs uppercase tracking-wide mb-1">Redução CBS</span>
                      <span className="font-semibold text-gray-900">
                        {formData.classTrib.percentualReducaoCBS?.toFixed(2)}%
                      </span>
                    </div>
                    <div className="bg-white/80 rounded-lg p-3 border border-indigo-100/50">
                      <span className="block text-indigo-400 text-xs uppercase tracking-wide mb-1">Válido NFe</span>
                      <span className={`inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium ${
                        formData.classTrib.validoParaNFe 
                          ? 'bg-emerald-100 text-emerald-700' 
                          : 'bg-gray-100 text-gray-500'
                      }`}>
                        {formData.classTrib.validoParaNFe ? '✓ Sim' : 'Não'}
                      </span>
                    </div>
                    <div className="col-span-2 md:col-span-3 lg:col-span-6 bg-white/80 rounded-lg p-3 border border-indigo-100/50">
                      <span className="block text-indigo-400 text-xs uppercase tracking-wide mb-1">Descrição</span>
                      <span className="text-gray-700">{formData.classTrib.descricaoClassTrib}</span>
                    </div>
                  </div>
                </div>
              )}
            </div>
          </div>
        </form>
    </div>
  );
}
