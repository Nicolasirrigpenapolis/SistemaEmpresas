import { useEffect, useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import {
  ArrowLeft,
  Save,
  Package,
  Loader2,
  AlertCircle,
  Eye,
  Edit2,
  Hash,
  ChevronRight,
  BadgeCheck,
  X,
  Barcode,
  MapPin,
  Scale,
  Calculator,
  Layers,
  FileText,
  Image,
  DollarSign,
  Box,
  Ruler,
  Calendar,
  Building2,
  Percent,
  ClipboardList,
  CheckCircle2,
  XCircle,
  ExternalLink,
} from 'lucide-react';
import { produtoService } from '../../services/produtoService';
import type { ProdutoCreateUpdateDto } from '../../types/produto';

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
  maxLength?: number;
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
  maxLength,
}: InputModernoProps) {
  const [focado, setFocado] = useState(false);
  const temValor = value !== undefined && value !== '' && value !== 0;
  
  return (
    <div className={`relative ${className}`}>
      <div className={`
        relative flex items-center border rounded-xl transition-all duration-200
        ${focado ? 'border-blue-500 ring-2 ring-blue-500/20 shadow-sm' : 'border-gray-200 hover:border-gray-300'}
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
            maxLength={maxLength}
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
          `}>
            {label}{required && <span className="text-red-500 ml-0.5">*</span>}
          </label>
        </div>
      </div>
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
  icone?: React.ReactNode;
  cor?: string;
  disabled?: boolean;
}

function CheckboxChip({ checked, onChange, label, icone, cor = 'blue', disabled }: CheckboxChipProps) {
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
      {icone && <span className={checked ? estilo.icon : 'text-gray-400'}>{icone}</span>}
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

// Tipos de produto do VB6
const TIPOS_PRODUTO = [
  { value: 0, label: 'Acabado' },
  { value: 1, label: 'Matéria Prima' },
  { value: 2, label: 'Material de Revenda' },
  { value: 3, label: 'Material de Consumo' },
  { value: 4, label: 'Material Imobilizado' },
  { value: 5, label: 'Embalagem' },
  { value: 6, label: 'Produto em Processo' },
  { value: 7, label: 'SubProduto' },
  { value: 8, label: 'Produto Intermediário' },
  { value: 9, label: 'Outros Insumos' },
  { value: 10, label: 'Bens de Valor Irrelevante' },
];

// Default do formulário
const PRODUTO_DEFAULT: ProdutoCreateUpdateDto = {
  descricao: '',
  codigoDeBarras: '',
  sequenciaDoGrupoProduto: 0,
  sequenciaDoSubGrupoProduto: 0,
  sequenciaDaUnidade: 0,
  sequenciaDaClassificacao: 0,
  quantidadeMinima: 0,
  localizacao: '',
  valorDeCusto: 0,
  margemDeLucro: 0,
  valorTotal: 0,
  valorDeLista: 0,
  eMateriaPrima: false,
  tipoDoProduto: 0,
  peso: 0,
  pesoOk: false,
  medida: '',
  medidaFinal: '',
  industrializacao: false,
  importado: false,
  materialAdquiridoDeTerceiro: false,
  sucata: false,
  obsoleto: false,
  inativo: false,
  usado: false,
  usadoNoProjeto: false,
  lance: false,
  eRegulador: false,
  travaReceita: false,
  naoSairNoRelatorio: false,
  naoSairNoChecklist: false,
  mostrarReceitaSecundaria: false,
  naoMostrarReceita: false,
  conferidoPeloContabil: false,
  mpInicial: false,
  parteDoPivo: '',
  modeloDoLance: 0,
  detalhes: '',
};

// ============================================================================
// COMPONENTE DE FORMULÁRIO
// ============================================================================
type TabType = 'dados' | 'receita' | 'contabilidade' | 'foto' | 'detalhes';

export default function ProdutoFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = id && id !== 'novo';
  const isViewMode = location.pathname.includes('/visualizar') || location.pathname.endsWith(`/${id}`);

  // Estados
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<TabType>('dados');
  
  // Dados extras para visualização (somente leitura)
  const [dadosExtras, setDadosExtras] = useState<{
    grupoProduto?: string;
    subGrupoProduto?: string;
    unidade?: string;
    classificacaoFiscal?: string;
    ncm?: string;
    percentualIpi?: number;
    nomeFornecedor?: string;
    quantidadeNoEstoque?: number;
    quantidadeContabil?: number;
    quantidadeFisica?: number;
    custoMedio?: number;
    valorContabilAtual?: number;
    ultimaCompra?: string;
    ultimoMovimento?: string;
    ultimaCotacao?: string;
    dataDaContagem?: string;
    separadoMontar?: number;
    compradosAguardando?: number;
    quantidadeBalanco?: number;
    usuarioDaAlteracao?: string;
    // ClassTrib (IBS/CBS)
    temClassTrib?: boolean;
    codigoClassTrib?: string;
    descricaoClassTrib?: string;
  }>({});
  
  // Form data
  const [formData, setFormData] = useState<ProdutoCreateUpdateDto>(PRODUTO_DEFAULT);

  // ============================================================================
  // FUNÇÕES
  // ============================================================================
  const loadData = async () => {
    if (!isEditing) return;

    try {
      setLoading(true);
      const data = await produtoService.obterPorId(Number(id));
      
      setFormData({
        descricao: data.descricao,
        codigoDeBarras: data.codigoDeBarras,
        sequenciaDoGrupoProduto: data.sequenciaDoGrupoProduto,
        sequenciaDoSubGrupoProduto: data.sequenciaDoSubGrupoProduto,
        sequenciaDaUnidade: data.sequenciaDaUnidade,
        sequenciaDaClassificacao: data.sequenciaDaClassificacao,
        quantidadeMinima: data.quantidadeMinima,
        localizacao: data.localizacao,
        valorDeCusto: data.valorDeCusto,
        margemDeLucro: data.margemDeLucro,
        valorTotal: data.valorTotal,
        valorDeLista: data.valorDeLista,
        eMateriaPrima: data.eMateriaPrima,
        tipoDoProduto: data.tipoDoProduto,
        peso: data.peso,
        pesoOk: data.pesoOk,
        medida: data.medida,
        medidaFinal: data.medidaFinal,
        industrializacao: data.industrializacao,
        importado: data.importado,
        materialAdquiridoDeTerceiro: data.materialAdquiridoDeTerceiro,
        sucata: data.sucata,
        obsoleto: data.obsoleto,
        inativo: data.inativo,
        usado: data.usado,
        usadoNoProjeto: data.usadoNoProjeto,
        lance: data.lance,
        eRegulador: data.eRegulador,
        travaReceita: data.travaReceita,
        naoSairNoRelatorio: data.naoSairNoRelatorio,
        naoSairNoChecklist: data.naoSairNoChecklist,
        mostrarReceitaSecundaria: data.mostrarReceitaSecundaria,
        naoMostrarReceita: data.naoMostrarReceita,
        conferidoPeloContabil: data.conferidoPeloContabil,
        mpInicial: data.mpInicial,
        parteDoPivo: data.parteDoPivo,
        modeloDoLance: data.modeloDoLance,
        detalhes: data.detalhes,
      });
      
      // Guarda dados extras para visualização
      setDadosExtras({
        grupoProduto: data.grupoProduto,
        subGrupoProduto: data.subGrupoProduto,
        unidade: data.unidade,
        classificacaoFiscal: data.classificacaoFiscal,
        ncm: data.ncm,
        percentualIpi: data.percentualIpi,
        nomeFornecedor: data.nomeFornecedor,
        quantidadeNoEstoque: data.quantidadeNoEstoque,
        quantidadeContabil: data.quantidadeContabil,
        quantidadeFisica: data.quantidadeFisica,
        custoMedio: data.custoMedio,
        valorContabilAtual: data.valorContabilAtual,
        ultimaCompra: data.ultimaCompra || undefined,
        ultimoMovimento: data.ultimoMovimento || undefined,
        ultimaCotacao: data.ultimaCotacao || undefined,
        dataDaContagem: data.dataDaContagem || undefined,
        separadoMontar: data.separadoMontar,
        compradosAguardando: data.compradosAguardando,
        quantidadeBalanco: data.quantidadeBalanco,
        usuarioDaAlteracao: data.usuarioDaAlteracao,
        // ClassTrib (IBS/CBS)
        temClassTrib: data.temClassTrib,
        codigoClassTrib: data.codigoClassTrib,
        descricaoClassTrib: data.descricaoClassTrib,
      });
    } catch (err: any) {
      console.error('Erro ao carregar dados:', err);
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

      // Backend valida campos obrigatórios (descrição)
      if (isEditing) {
        await produtoService.atualizar(Number(id), formData);
      } else {
        await produtoService.criar(formData);
      }

      navigate('/cadastros/produtos');
    } catch (err: any) {
      console.error('Erro ao salvar:', err);
      setError(err.response?.data?.mensagem || 'Erro ao salvar dados');
    } finally {
      setSaving(false);
    }
  };

  // Helper para converter string para número de forma segura (evita NaN)
  const safeParseNumber = (value: string, defaultValue: number = 0): number => {
    if (value === '' || value === null || value === undefined) {
      return defaultValue;
    }
    const parsed = Number(value);
    return isNaN(parsed) ? defaultValue : parsed;
  };

  const handleChange = <K extends keyof ProdutoCreateUpdateDto>(
    field: K,
    value: ProdutoCreateUpdateDto[K]
  ) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  // Handler específico para campos numéricos (evita NaN)
  const handleNumberChange = <K extends keyof ProdutoCreateUpdateDto>(
    field: K,
    value: string
  ) => {
    const numValue = safeParseNumber(value, 0);
    setFormData((prev) => ({ ...prev, [field]: numValue as ProdutoCreateUpdateDto[K] }));
  };

  useEffect(() => {
    loadData();
  }, [id]);

  // Formatar valores
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatQuantity = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 4,
    }).format(value);
  };

  const formatDate = (dateStr?: string) => {
    if (!dateStr) return '-';
    try {
      return new Date(dateStr).toLocaleDateString('pt-BR');
    } catch {
      return '-';
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="flex flex-col items-center gap-3">
          <Loader2 className="w-8 h-8 animate-spin text-blue-600" />
          <span className="text-gray-500">Carregando...</span>
        </div>
      </div>
    );
  }

  // Conteúdo de cada aba
  const renderTabContent = () => {
    switch (activeTab) {
      case 'dados':
        return renderDadosPrincipais();
      case 'receita':
        return renderReceita();
      case 'contabilidade':
        return renderContabilidade();
      case 'foto':
        return renderFoto();
      case 'detalhes':
        return renderDetalhes();
      default:
        return null;
    }
  };

  // Aba 1 - Dados Principais
  const renderDadosPrincipais = () => (
    <div className="space-y-6 p-6">
      {/* Classificação */}
      <SecaoCard titulo="Classificação" subtitulo="Grupo, unidade e classificação fiscal" icone={<Layers className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <InputModerno
            label="Grupo"
            value={isViewMode ? dadosExtras.grupoProduto : formData.sequenciaDoGrupoProduto}
            onChange={(v) => handleNumberChange('sequenciaDoGrupoProduto', v)}
            disabled={isViewMode}
            icone={<Layers className="w-4 h-4" />}
          />
          <InputModerno
            label="Sub Grupo"
            value={isViewMode ? dadosExtras.subGrupoProduto : formData.sequenciaDoSubGrupoProduto}
            onChange={(v) => handleNumberChange('sequenciaDoSubGrupoProduto', v)}
            disabled={isViewMode}
          />
          <InputModerno
            label="Unidade"
            value={isViewMode ? dadosExtras.unidade : formData.sequenciaDaUnidade}
            onChange={(v) => handleNumberChange('sequenciaDaUnidade', v)}
            disabled={isViewMode}
            icone={<Box className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>

      {/* Status ClassTrib (IBS/CBS) */}
      {isEditing && (
        <div className={`p-4 rounded-xl border-2 ${
          dadosExtras.temClassTrib 
            ? 'bg-green-50 border-green-200' 
            : 'bg-amber-50 border-amber-200'
        }`}>
          <div className="flex items-start gap-3">
            {dadosExtras.temClassTrib ? (
              <CheckCircle2 className="w-6 h-6 text-green-600 flex-shrink-0 mt-0.5" />
            ) : (
              <XCircle className="w-6 h-6 text-amber-600 flex-shrink-0 mt-0.5" />
            )}
            <div className="flex-1">
              <h4 className={`font-semibold ${dadosExtras.temClassTrib ? 'text-green-800' : 'text-amber-800'}`}>
                {dadosExtras.temClassTrib ? 'ClassTrib Vinculado' : 'ClassTrib Não Vinculado'}
              </h4>
              {dadosExtras.temClassTrib ? (
                <div className="mt-1">
                  <p className="text-sm text-green-700">
                    <span className="font-medium">Código:</span> {dadosExtras.codigoClassTrib}
                  </p>
                  {dadosExtras.descricaoClassTrib && (
                    <p className="text-sm text-green-600 mt-0.5">
                      {dadosExtras.descricaoClassTrib}
                    </p>
                  )}
                  <p className="text-xs text-green-600 mt-1">
                    Este produto está configurado para os novos impostos IBS/CBS.
                  </p>
                </div>
              ) : (
                <div className="mt-1">
                  <p className="text-sm text-amber-700">
                    A Classificação Fiscal deste produto não possui um ClassTrib vinculado.
                  </p>
                  <p className="text-xs text-amber-600 mt-1">
                    Configure o ClassTrib em <span className="font-medium">Fiscal → Classificação Fiscal</span> para habilitar os impostos IBS/CBS.
                  </p>
                </div>
              )}
            </div>
          </div>
        </div>
      )}

      {/* Estoque e Valores */}
      <SecaoCard titulo="Estoque e Valores" subtitulo="Quantidades e preços" icone={<Calculator className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
          <InputModerno
            label="Estoque"
            value={formatQuantity(dadosExtras.quantidadeNoEstoque || 0)}
            onChange={() => {}}
            disabled={true}
            icone={<Package className="w-4 h-4" />}
          />
          <InputModerno
            label="Qtde Mín."
            value={formData.quantidadeMinima}
            onChange={(v) => handleNumberChange('quantidadeMinima', v)}
            type="number"
            disabled={isViewMode}
          />
          <InputModerno
            label="Cód. Barras"
            value={formData.codigoDeBarras}
            onChange={(v) => handleChange('codigoDeBarras', v)}
            disabled={isViewMode}
            maxLength={13}
            icone={<Barcode className="w-4 h-4" />}
          />
          <InputModerno
            label="Localização"
            value={formData.localizacao}
            onChange={(v) => handleChange('localizacao', v)}
            disabled={isViewMode}
            maxLength={50}
            icone={<MapPin className="w-4 h-4" />}
          />
        </div>
        
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4 mt-4">
          <InputModerno
            label="Últ. Compra"
            value={formatDate(dadosExtras.ultimaCompra)}
            onChange={() => {}}
            disabled={true}
            icone={<Calendar className="w-4 h-4" />}
          />
          <InputModerno
            label="Últ. Movimento"
            value={formatDate(dadosExtras.ultimoMovimento)}
            onChange={() => {}}
            disabled={true}
            icone={<Calendar className="w-4 h-4" />}
          />
          <InputModerno
            label="Últ. Cotação"
            value={formatDate(dadosExtras.ultimaCotacao)}
            onChange={() => {}}
            disabled={true}
            icone={<Calendar className="w-4 h-4" />}
          />
          <InputModerno
            label="Margem %"
            value={formData.margemDeLucro}
            onChange={(v) => handleNumberChange('margemDeLucro', v)}
            type="number"
            disabled={true}
            icone={<Percent className="w-4 h-4" />}
          />
          <InputModerno
            label="Valor Venda"
            value={formatCurrency(formData.valorTotal)}
            onChange={() => {}}
            disabled={true}
            icone={<DollarSign className="w-4 h-4" />}
          />
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mt-4">
          <InputModerno
            label="Fornecedor"
            value={dadosExtras.nomeFornecedor || '-'}
            onChange={() => {}}
            disabled={true}
            icone={<Building2 className="w-4 h-4" />}
          />
          <InputModerno
            label="Vr. Custo"
            value={formatCurrency(formData.valorDeCusto)}
            onChange={() => {}}
            disabled={true}
            icone={<DollarSign className="w-4 h-4" />}
          />
          <div className="flex gap-2 items-end">
            <InputModerno
              label="Peso"
              value={formData.peso}
              onChange={(v) => handleNumberChange('peso', v)}
              type="number"
              disabled={isViewMode}
              icone={<Scale className="w-4 h-4" />}
              className="flex-1"
            />
            <CheckboxChip
              checked={formData.pesoOk}
              onChange={(v) => handleChange('pesoOk', v)}
              label="Conferido"
              cor="green"
              disabled={isViewMode}
            />
          </div>
        </div>
      </SecaoCard>

      {/* NCM e Classificação Fiscal */}
      <SecaoCard titulo="Classificação Fiscal" subtitulo="NCM e IPI" icone={<ClipboardList className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <div className="flex gap-2 items-end">
            <InputModerno
              label="NCM"
              value={dadosExtras.ncm || ''}
              onChange={() => {}}
              disabled={true}
              className="flex-1"
            />
            {formData.sequenciaDaClassificacao > 0 && (
              <button
                type="button"
                onClick={() => navigate(`/classificacao-fiscal/${formData.sequenciaDaClassificacao}`)}
                className="flex items-center justify-center px-3 py-3 bg-blue-50 text-blue-600 border border-blue-200 rounded-xl hover:bg-blue-100 hover:border-blue-300 transition-all duration-200"
                title="Abrir Classificação Fiscal"
              >
                <ExternalLink className="w-5 h-5" />
              </button>
            )}
          </div>
          <InputModerno
            label="Classificação Fiscal"
            value={dadosExtras.classificacaoFiscal || '-'}
            onChange={() => {}}
            disabled={true}
          />
          <InputModerno
            label="% IPI"
            value={dadosExtras.percentualIpi?.toFixed(4) || '0'}
            onChange={() => {}}
            disabled={true}
            icone={<Percent className="w-4 h-4" />}
          />
        </div>
        <div className="mt-4">
          <CheckboxChip
            checked={formData.conferidoPeloContabil}
            onChange={(v) => handleChange('conferidoPeloContabil', v)}
            label="NCM - Conferido pela Contabilidade"
            cor="green"
            disabled={isViewMode}
          />
        </div>
      </SecaoCard>

      {/* Medidas e Projeto */}
      <SecaoCard titulo="Medidas e Projeto" subtitulo="Dimensões e referências" icone={<Ruler className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <InputModerno
            label="Medida"
            value={formData.medida}
            onChange={(v) => handleChange('medida', v)}
            disabled={isViewMode}
            maxLength={100}
          />
          <InputModerno
            label="Medida Final"
            value={formData.medidaFinal}
            onChange={(v) => handleChange('medidaFinal', v)}
            disabled={isViewMode}
            maxLength={20}
          />
          <InputModerno
            label="REF - Projeto"
            value={formData.modeloDoLance}
            onChange={(v) => handleNumberChange('modeloDoLance', v)}
            type="number"
            disabled={isViewMode}
          />
          <InputModerno
            label="Parte do Pivo"
            value={formData.parteDoPivo}
            onChange={(v) => handleChange('parteDoPivo', v)}
            disabled={isViewMode}
            maxLength={29}
          />
        </div>

        <div className="flex flex-wrap gap-3 mt-4">
          <CheckboxChip
            checked={formData.usadoNoProjeto}
            onChange={(v) => handleChange('usadoNoProjeto', v)}
            label="Filtrar nos Projetos"
            cor="blue"
            disabled={isViewMode}
          />
        </div>
      </SecaoCard>
    </div>
  );

  // Aba 2 - Receita (matérias primas)
  const renderReceita = () => (
    <div className="p-6">
      <SecaoCard titulo="Receita do Produto" subtitulo="Matérias primas utilizadas na fabricação" icone={<ClipboardList className="w-5 h-5" />}>
        <div className="text-center py-12 text-gray-500">
          <ClipboardList className="w-12 h-12 mx-auto mb-4 text-gray-300" />
          <p className="text-lg font-medium">Receita do Produto</p>
          <p className="text-sm">A gestão de receitas será implementada em breve.</p>
          <p className="text-xs mt-2 text-gray-400">F11 - Deleta Receita</p>
        </div>
      </SecaoCard>
    </div>
  );

  // Aba 3 - Contabilidade
  const renderContabilidade = () => (
    <div className="space-y-6 p-6">
      {/* Tipo do Produto */}
      <SecaoCard titulo="Tipo do Produto" subtitulo="Classificação contábil" icone={<Box className="w-5 h-5" />}>
        <SelectModerno
          label="Tipo"
          value={formData.tipoDoProduto}
          onChange={(v) => handleChange('tipoDoProduto', safeParseNumber(String(v), 0))}
          opcoes={TIPOS_PRODUTO}
          disabled={isViewMode}
          className="max-w-md"
        />
      </SecaoCard>

      {/* Quantidades Contábeis */}
      <SecaoCard titulo="Quantidades Contábeis" subtitulo="Controle de estoque contábil" icone={<Calculator className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <InputModerno
            label="Qtde Contábil"
            value={formatQuantity(dadosExtras.quantidadeContabil || 0)}
            onChange={() => {}}
            disabled={true}
          />
          <InputModerno
            label="Qtde Física"
            value={formatQuantity(dadosExtras.quantidadeFisica || 0)}
            onChange={() => {}}
            disabled={true}
          />
          <InputModerno
            label="Data da Contagem"
            value={formatDate(dadosExtras.dataDaContagem)}
            onChange={() => {}}
            disabled={true}
            icone={<Calendar className="w-4 h-4" />}
          />
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mt-4">
          <InputModerno
            label="Separado Montar"
            value={formatQuantity(dadosExtras.separadoMontar || 0)}
            onChange={() => {}}
            disabled={true}
          />
          <InputModerno
            label="Comprados Aguardando"
            value={formatQuantity(dadosExtras.compradosAguardando || 0)}
            onChange={() => {}}
            disabled={true}
          />
          <InputModerno
            label="Valor Contábil"
            value={formatCurrency(dadosExtras.valorContabilAtual || 0)}
            onChange={() => {}}
            disabled={true}
            icone={<DollarSign className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>

      {/* Último Balanço */}
      <SecaoCard titulo="Último Balanço" subtitulo="Dados do último inventário" icone={<ClipboardList className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <InputModerno
            label="Data da Contagem"
            value={formatDate(dadosExtras.dataDaContagem)}
            onChange={() => {}}
            disabled={true}
            icone={<Calendar className="w-4 h-4" />}
          />
          <InputModerno
            label="Valor"
            value={formatCurrency(dadosExtras.valorContabilAtual || 0)}
            onChange={() => {}}
            disabled={true}
            icone={<DollarSign className="w-4 h-4" />}
          />
          <InputModerno
            label="Quantidade Balanço"
            value={formatQuantity(dadosExtras.quantidadeBalanco || 0)}
            onChange={() => {}}
            disabled={true}
          />
          <InputModerno
            label="Estoque"
            value={formatQuantity(dadosExtras.quantidadeNoEstoque || 0)}
            onChange={() => {}}
            disabled={true}
            icone={<Package className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>

      {/* Características do Produto */}
      <SecaoCard titulo="Características do Produto" subtitulo="Classificação e atributos" icone={<Box className="w-5 h-5" />}>
        <div className="flex flex-wrap gap-3">
          <CheckboxChip
            checked={formData.eMateriaPrima}
            onChange={(v) => handleChange('eMateriaPrima', v)}
            label="Matéria Prima"
            cor="blue"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.usado}
            onChange={(v) => handleChange('usado', v)}
            label="Usado"
            cor="amber"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.materialAdquiridoDeTerceiro}
            onChange={(v) => handleChange('materialAdquiridoDeTerceiro', v)}
            label="Material Ad. de Terceiro"
            cor="blue"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.sucata}
            onChange={(v) => handleChange('sucata', v)}
            label="Sucata"
            cor="amber"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.industrializacao}
            onChange={(v) => handleChange('industrializacao', v)}
            label="Industrialização"
            cor="purple"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.importado}
            onChange={(v) => handleChange('importado', v)}
            label="Importado"
            cor="green"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.obsoleto}
            onChange={(v) => handleChange('obsoleto', v)}
            label="Obsoleto"
            cor="red"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.inativo}
            onChange={(v) => handleChange('inativo', v)}
            label="Inativo"
            cor="red"
            disabled={isViewMode}
          />
        </div>
        
        <div className="flex flex-wrap gap-3 mt-4 pt-4 border-t">
          <CheckboxChip
            checked={formData.lance}
            onChange={(v) => handleChange('lance', v)}
            label="Lance"
            cor="blue"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.eRegulador}
            onChange={(v) => handleChange('eRegulador', v)}
            label="E Regulador"
            cor="orange"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.travaReceita}
            onChange={(v) => handleChange('travaReceita', v)}
            label="Trava Receita"
            cor="red"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.mpInicial}
            onChange={(v) => handleChange('mpInicial', v)}
            label="M.Prima Inicial"
            cor="green"
            disabled={isViewMode}
          />
        </div>
      </SecaoCard>

      {/* Flags de Controle */}
      <SecaoCard titulo="Opções de Controle" subtitulo="Configurações do produto" icone={<ClipboardList className="w-5 h-5" />}>
        <div className="flex flex-wrap gap-3">
          <CheckboxChip
            checked={formData.naoSairNoChecklist}
            onChange={(v) => handleChange('naoSairNoChecklist', v)}
            label="Não mostrar no Controle de Pedidos"
            cor="amber"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.naoMostrarReceita}
            onChange={(v) => handleChange('naoMostrarReceita', v)}
            label="Não Mostrar Receita"
            cor="orange"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.naoSairNoRelatorio}
            onChange={(v) => handleChange('naoSairNoRelatorio', v)}
            label="Não Sair no Relatório da Produção"
            cor="red"
            disabled={isViewMode}
          />
          <CheckboxChip
            checked={formData.mostrarReceitaSecundaria}
            onChange={(v) => handleChange('mostrarReceitaSecundaria', v)}
            label="Mostrar Receita Secundária"
            cor="green"
            disabled={isViewMode}
          />
        </div>
      </SecaoCard>
    </div>
  );

  // Aba 4 - Foto do Produto
  const renderFoto = () => (
    <div className="p-6">
      <SecaoCard titulo="Foto do Produto" subtitulo="Imagem para identificação" icone={<Image className="w-5 h-5" />}>
        <div className="border-2 border-dashed border-gray-300 rounded-xl p-12 text-center">
          <Image className="w-16 h-16 mx-auto mb-4 text-gray-300" />
          <p className="text-gray-500 font-medium">Nenhuma imagem cadastrada</p>
          <p className="text-sm text-gray-400 mt-1">Funcionalidade de upload de imagens será implementada em breve.</p>
        </div>
      </SecaoCard>
    </div>
  );

  // Aba 5 - Detalhes
  const renderDetalhes = () => (
    <div className="p-6">
      <SecaoCard titulo="Detalhes" subtitulo="Informações adicionais do produto" icone={<FileText className="w-5 h-5" />}>
        <textarea
          value={formData.detalhes}
          onChange={(e) => handleChange('detalhes', e.target.value)}
          disabled={isViewMode}
          placeholder="Digite informações detalhadas sobre o produto..."
          className="w-full h-64 p-4 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 resize-none disabled:bg-gray-50"
        />
      </SecaoCard>
    </div>
  );

  return (
    <div className="space-y-4 sm:space-y-6 p-4 sm:p-0">
      {/* Header */}
      <div className={`flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 ${
        isViewMode 
          ? 'bg-gradient-to-r from-indigo-600 via-blue-600 to-cyan-500 -mx-4 sm:-mx-6 -mt-4 sm:-mt-6 px-4 sm:px-6 py-4 rounded-t-xl' 
          : ''
      }`}>
        <div className="flex items-center gap-3 sm:gap-4">
          <button
            onClick={() => navigate('/cadastros/produtos')}
            className={`p-2 sm:p-2.5 rounded-xl transition-all duration-200 ${
              isViewMode 
                ? 'hover:bg-white/20 text-white' 
                : 'hover:bg-gray-100 text-gray-600'
            }`}
          >
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div>
            <div className="flex items-center gap-2">
              {isViewMode && (
                <div className="p-1 sm:p-1.5 bg-white/20 rounded-lg">
                  <Eye className="w-4 h-4 text-white" />
                </div>
              )}
              <h1 className={`text-lg sm:text-xl font-bold ${isViewMode ? 'text-white' : 'text-gray-900'}`}>
                {isViewMode ? 'Visualizar Produto' : isEditing ? 'Editar Produto' : 'Novo Produto'}
              </h1>
            </div>
            <div className={`flex flex-wrap items-center gap-2 text-xs sm:text-sm ${isViewMode ? 'text-blue-100' : 'text-gray-500'}`}>
              {(isEditing || isViewMode) && (
                <>
                  <Hash className="w-3.5 h-3.5" />
                  <span>Código: {id}</span>
                  {/* Indicador ClassTrib */}
                  {dadosExtras.temClassTrib ? (
                    <span className="inline-flex items-center gap-1 px-2 py-0.5 bg-green-100 text-green-700 text-xs font-medium rounded-full">
                      <CheckCircle2 className="w-3 h-3" />
                      <span className="hidden sm:inline">ClassTrib OK</span>
                    </span>
                  ) : (
                    <span className="inline-flex items-center gap-1 px-2 py-0.5 bg-amber-100 text-amber-700 text-xs font-medium rounded-full">
                      <XCircle className="w-3 h-3" />
                      <span className="hidden sm:inline">Sem ClassTrib</span>
                    </span>
                  )}
                </>
              )}
              {!isEditing && !isViewMode && (
                <span>Preencha os dados do produto</span>
              )}
            </div>
          </div>
        </div>
        
        {isViewMode ? (
          <button
            type="button"
            onClick={() => navigate(`/cadastros/produtos/${id}/editar`)}
            className="flex items-center justify-center gap-2 px-5 py-2.5 bg-white text-blue-600 rounded-xl hover:bg-blue-50 transition-all duration-200 shadow-lg shadow-blue-500/20 font-medium w-full sm:w-auto"
          >
            <Edit2 className="w-4 h-4" />
            <span>Editar</span>
          </button>
        ) : (
          <button
            onClick={handleSubmit}
            disabled={saving}
            className="flex items-center justify-center gap-2 px-5 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-xl hover:from-blue-700 hover:to-blue-800 transition-all duration-200 disabled:opacity-50 shadow-lg shadow-blue-500/30 font-medium w-full sm:w-auto"
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

        {/* Descrição - Campo Principal - Igual VB6 */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm p-5">
          {/* Cabeçalho com Descrição e Checkboxes Direita (igual VB6) */}
          <div className="flex items-start justify-between gap-6 mb-4">
            {/* Lado Esquerdo - Descrição */}
            <div className="flex-1">
              <InputModerno
                label="Descrição"
                value={formData.descricao}
                onChange={(v) => handleChange('descricao', v)}
                required
                disabled={isViewMode}
                icone={<Package className="w-4 h-4" />}
              />
            </div>
            
            {/* Lado Direito - Checkboxes (Inativo, Lance, E Regulador, Obsoleto, Trava Receita, M.Prima Inicial) */}
            <div className="flex flex-col gap-2 min-w-fit">
              <div className="flex flex-wrap gap-2 justify-end">
                <CheckboxChip
                  checked={formData.inativo}
                  onChange={(v) => handleChange('inativo', v)}
                  label="Inativo"
                  cor="red"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.lance}
                  onChange={(v) => handleChange('lance', v)}
                  label="Lance"
                  cor="blue"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.eRegulador}
                  onChange={(v) => handleChange('eRegulador', v)}
                  label="E Regulador"
                  cor="orange"
                  disabled={isViewMode}
                />
              </div>
              <div className="flex flex-wrap gap-2 justify-end">
                <CheckboxChip
                  checked={formData.obsoleto}
                  onChange={(v) => handleChange('obsoleto', v)}
                  label="Obsoleto"
                  cor="amber"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.travaReceita}
                  onChange={(v) => handleChange('travaReceita', v)}
                  label="Trava Receita"
                  cor="red"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.mpInicial}
                  onChange={(v) => handleChange('mpInicial', v)}
                  label="M.Prima Inicial"
                  cor="green"
                  disabled={isViewMode}
                />
              </div>
            </div>
          </div>

          {/* Linha de Checkboxes abaixo da descrição (igual VB6) */}
          <div className="flex flex-wrap gap-2 pt-3 border-t border-gray-100">
            <CheckboxChip
              checked={formData.eMateriaPrima}
              onChange={(v) => handleChange('eMateriaPrima', v)}
              label="Matéria Prima"
              cor="purple"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.usado}
              onChange={(v) => handleChange('usado', v)}
              label="Usado"
              cor="amber"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.materialAdquiridoDeTerceiro}
              onChange={(v) => handleChange('materialAdquiridoDeTerceiro', v)}
              label="Material Ad. de Terceiro"
              cor="blue"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.sucata}
              onChange={(v) => handleChange('sucata', v)}
              label="Sucata"
              cor="amber"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.industrializacao}
              onChange={(v) => handleChange('industrializacao', v)}
              label="Industrialização"
              cor="purple"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.importado}
              onChange={(v) => handleChange('importado', v)}
              label="Importado"
              cor="green"
              disabled={isViewMode}
            />
          </div>
        </div>

        {/* Abas Modernas */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden">
          {/* Tab Headers */}
          <div className="flex border-b border-gray-100 bg-gray-50/50 overflow-x-auto">
            {[
              { id: 'dados' as TabType, label: '1 - Dados Principais', icon: FileText },
              { id: 'receita' as TabType, label: '2 - Receita', icon: ClipboardList },
              { id: 'contabilidade' as TabType, label: '3 - Contabilidade', icon: Calculator },
              { id: 'foto' as TabType, label: 'Foto do Produto', icon: Image },
              { id: 'detalhes' as TabType, label: 'Detalhes', icon: FileText },
            ].map((tab) => (
              <button
                key={tab.id}
                type="button"
                onClick={() => setActiveTab(tab.id)}
                className={`flex-shrink-0 flex items-center justify-center gap-2 px-6 py-4 text-sm font-medium transition-all duration-200 relative whitespace-nowrap ${
                  activeTab === tab.id
                    ? 'text-blue-600 bg-white'
                    : 'text-gray-500 hover:text-gray-700 hover:bg-gray-50'
                }`}
              >
                <tab.icon className="w-4 h-4" />
                <span>{tab.label}</span>
                {activeTab === tab.id && (
                  <div className="absolute bottom-0 left-0 right-0 h-0.5 bg-gradient-to-r from-blue-500 to-cyan-500" />
                )}
              </button>
            ))}
          </div>

          {/* Tab Content */}
          {renderTabContent()}
        </div>
      </form>
    </div>
  );
}
