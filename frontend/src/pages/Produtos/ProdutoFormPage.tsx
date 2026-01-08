import { useEffect, useState, useRef } from 'react';
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
  X,
  Barcode,
  MapPin,
  Scale,
  Calculator,
  Layers,
  FileText,
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
  Upload,
  Trash2,
  Camera,
  ZoomIn,
  Plus,
  Search,
} from 'lucide-react';
import { produtoService } from '../../services/Produto/produtoService';
import { classificacaoFiscalService } from '../../services/Fiscal/classificacaoFiscalService';
import { SeletorComBusca } from '../../components/SeletorComBusca';
import { ModalConfirmacao } from '../../components/common/ModalConfirmacao';
import type { ProdutoCreateUpdateDto, ReceitaProdutoListDto, ProdutoComboDto } from '../../types';
import type { GrupoProduto, SubGrupoProduto, Unidade } from '../../services/Produto/produtoService';
import type { ClassificacaoFiscal } from '../../types/Fiscal/classificacaoFiscal';

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
  // Considera 0 como valor preenchido para evitar sobrepor o rótulo em campos numéricos
  const temValor = value !== undefined && value !== '';
  
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
    <div className={`bg-white rounded-2xl border border-gray-100 shadow-sm overflow-visible ${className}`}>
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
type TabType = 'dados' | 'receita' | 'contabilidade' | 'detalhes';

export default function ProdutoFormPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = id && id !== 'novo';
  const isViewMode = location.pathname.includes('/visualizar') || location.pathname.endsWith(`/${id}`);

  // Ref para input de arquivo
  const fileInputRef = useRef<HTMLInputElement>(null);

  // Estados
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<TabType>('dados');
  
  // Estados para listas auxiliares
  const [grupos, setGrupos] = useState<GrupoProduto[]>([]);
  const [subGrupos, setSubGrupos] = useState<SubGrupoProduto[]>([]);
  const [unidades, setUnidades] = useState<Unidade[]>([]);
  const [classificacoesBusca, setClassificacoesBusca] = useState<ClassificacaoFiscal[]>([]);
  const [buscandoClassificacoes, setBuscandoClassificacoes] = useState(false);
  const [loadingAuxiliares, setLoadingAuxiliares] = useState(false);
  
  // Estado para foto do produto
  const [fotoUrl, setFotoUrl] = useState<string | null>(null);
  const [loadingFoto, setLoadingFoto] = useState(false);
  const [fotoError, setFotoError] = useState(false);
  const [uploadingFoto, setUploadingFoto] = useState(false);
  const [fotoExpandida, setFotoExpandida] = useState(false);
  
  // Estados para Receita do Produto (Materia Prima)
  const [itensReceita, setItensReceita] = useState<ReceitaProdutoListDto[]>([]);
  const [loadingReceita, setLoadingReceita] = useState(false);
  const [produtosBusca, setProdutosBusca] = useState<ProdutoComboDto[]>([]);
  const [buscandoProdutos, setBuscandoProdutos] = useState(false);
  const [termoBuscaProduto, setTermoBuscaProduto] = useState('');
  const [produtoSelecionado, setProdutoSelecionado] = useState<ProdutoComboDto | null>(null);
  const [quantidadeItem, setQuantidadeItem] = useState<string>('1');
  const [editandoItem, setEditandoItem] = useState<number | null>(null);
  const [quantidadeEdicao, setQuantidadeEdicao] = useState<string>('');
  
  // Estado para Modal de Confirmação
  const [modalConfirmacao, setModalConfirmacao] = useState<{
    aberto: boolean;
    titulo: string;
    mensagem: string;
    nomeItem?: string;
    onConfirmar: () => void;
    variante?: 'danger' | 'warning';
  }>({
    aberto: false,
    titulo: '',
    mensagem: '',
    onConfirmar: () => {},
  });
  
  // Dados extras para visualizacao (somente leitura)
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
      
      // Busca a foto do produto
      setLoadingFoto(true);
      const foto = await produtoService.obterFoto(Number(id));
      setFotoUrl(foto);
      setFotoError(false);
      setLoadingFoto(false);
    } catch (err: any) {
      console.error('Erro ao carregar dados:', err);
      setError(err.response?.data?.mensagem || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

  // Carregar dados auxiliares (Grupos, SubGrupos, Unidades)
  const loadAuxiliares = async () => {
    try {
      setLoadingAuxiliares(true);
      const [gruposData, subGruposData, unidadesData] = await Promise.all([
        produtoService.listarGrupos(),
        produtoService.listarSubGrupos(),
        produtoService.listarUnidades(),
      ]);
      setGrupos(gruposData);
      setSubGrupos(subGruposData);
      setUnidades(unidadesData);
    } catch (error) {
      console.error('Erro ao carregar dados auxiliares:', error);
    } finally {
      setLoadingAuxiliares(false);
    }
  };

  // Upload de foto do produto
  const handleUploadFoto = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file || !id) return;

    try {
      setUploadingFoto(true);
      await produtoService.uploadFoto(Number(id), file);
      
      // Recarrega a foto
      const novaFoto = await produtoService.obterFoto(Number(id));
      setFotoUrl(novaFoto);
      setFotoError(false);
    } catch (err: any) {
      console.error('Erro ao fazer upload:', err);
      alert(err.response?.data?.mensagem || 'Erro ao fazer upload da foto');
    } finally {
      setUploadingFoto(false);
      // Limpa o input para permitir selecionar o mesmo arquivo novamente
      if (fileInputRef.current) {
        fileInputRef.current.value = '';
      }
    }
  };

  // Remover foto do produto
  const handleRemoverFoto = async () => {
    if (!id || !fotoUrl) return;
    
    setModalConfirmacao({
      aberto: true,
      titulo: 'Remover Foto',
      mensagem: 'Deseja realmente remover a foto deste produto?',
      variante: 'danger',
      onConfirmar: async () => {
        try {
          setUploadingFoto(true);
          setModalConfirmacao(prev => ({ ...prev, aberto: false }));
          await produtoService.removerFoto(Number(id));
          setFotoUrl(null);
        } catch (err: any) {
          console.error('Erro ao remover foto:', err);
          alert(err.response?.data?.mensagem || 'Erro ao remover a foto');
        } finally {
          setUploadingFoto(false);
        }
      }
    });
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
    loadAuxiliares();
  }, [id]);

  // Recarregar SubGrupos quando o Grupo mudar (igual ao VB6: PoeRelEFiltroCbo)
  useEffect(() => {
    const loadSubGruposFiltrados = async () => {
      if (formData.sequenciaDoGrupoProduto > 0) {
        try {
          const subGruposData = await produtoService.listarSubGrupos(formData.sequenciaDoGrupoProduto);
          setSubGrupos(subGruposData);
        } catch (error) {
          console.error('Erro ao carregar subgrupos:', error);
        }
      } else {
        // Se nao tem grupo selecionado, carrega todos os subgrupos ou limpa
        setSubGrupos([]);
      }
    };
    
    loadSubGruposFiltrados();
  }, [formData.sequenciaDoGrupoProduto]);

  // ===== Funcoes de Receita do Produto =====
  
  // Carregar itens da receita
  const carregarReceita = async () => {
    if (!isEditing) return;
    
    try {
      setLoadingReceita(true);
      const itens = await produtoService.listarItensReceita(Number(id));
      setItensReceita(itens);
    } catch (err) {
      console.error('Erro ao carregar receita:', err);
    } finally {
      setLoadingReceita(false);
    }
  };

  // Carregar receita quando mudar para a aba
  useEffect(() => {
    if (activeTab === 'receita' && isEditing && itensReceita.length === 0) {
      carregarReceita();
    }
  }, [activeTab, isEditing]);

  // Buscar produtos para adicionar na receita
  const buscarProdutos = async (termo: string) => {
    if (!termo || termo.length < 1) {
      setProdutosBusca([]);
      return;
    }

    try {
      setBuscandoProdutos(true);
      const produtos = await produtoService.listarParaCombo(termo);
      // Filtra para nao mostrar o proprio produto
      setProdutosBusca(produtos.filter(p => p.sequenciaDoProduto !== Number(id)));
    } catch (err) {
      console.error('Erro ao buscar produtos:', err);
    } finally {
      setBuscandoProdutos(false);
    }
  };

  // Buscar classificações fiscais (NCM)
  const buscarClassificacoes = async (termo: string) => {
    if (!termo || termo.length < 2) {
      setClassificacoesBusca([]);
      return;
    }

    try {
      setBuscandoClassificacoes(true);
      const result = await classificacaoFiscalService.pesquisar(termo);
      setClassificacoesBusca(result);
    } catch (err) {
      console.error('Erro ao buscar classificações:', err);
    } finally {
      setBuscandoClassificacoes(false);
    }
  };

  // Debounce para busca de produtos
  useEffect(() => {
    const timer = setTimeout(() => {
      buscarProdutos(termoBuscaProduto);
    }, 300);
    return () => clearTimeout(timer);
  }, [termoBuscaProduto]);

  // Adicionar item a receita
  const adicionarItemReceita = async () => {
    if (!produtoSelecionado || !id) return;

    const qtd = parseFloat(quantidadeItem.replace(',', '.'));
    if (isNaN(qtd) || qtd <= 0) {
      alert('Quantidade invalida');
      return;
    }

    try {
      setLoadingReceita(true);
      await produtoService.adicionarItemReceita(Number(id), {
        sequenciaDaMateriaPrima: produtoSelecionado.sequenciaDoProduto,
        quantidade: qtd,
      });
      
      // Limpa campos e recarrega
      setProdutoSelecionado(null);
      setTermoBuscaProduto('');
      setQuantidadeItem('1');
      setProdutosBusca([]);
      await carregarReceita();
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao adicionar item');
    } finally {
      setLoadingReceita(false);
    }
  };

  // Atualizar quantidade de um item
  const atualizarItemReceita = async (materiaPrimaId: number) => {
    if (!id) return;

    const qtd = parseFloat(quantidadeEdicao.replace(',', '.'));
    if (isNaN(qtd) || qtd <= 0) {
      alert('Quantidade invalida');
      return;
    }

    try {
      setLoadingReceita(true);
      await produtoService.atualizarItemReceita(Number(id), materiaPrimaId, {
        sequenciaDaMateriaPrima: materiaPrimaId,
        quantidade: qtd,
      });
      
      setEditandoItem(null);
      setQuantidadeEdicao('');
      await carregarReceita();
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao atualizar item');
    } finally {
      setLoadingReceita(false);
    }
  };

  // Remover item da receita
  const removerItemReceita = async (materiaPrimaId: number, descricao: string) => {
    if (!id) return;
    
    setModalConfirmacao({
      aberto: true,
      titulo: 'Remover Item',
      mensagem: 'Deseja realmente remover este item da receita?',
      nomeItem: descricao,
      variante: 'danger',
      onConfirmar: async () => {
        try {
          setLoadingReceita(true);
          setModalConfirmacao(prev => ({ ...prev, aberto: false }));
          await produtoService.removerItemReceita(Number(id), materiaPrimaId);
          await carregarReceita();
        } catch (err: any) {
          alert(err.response?.data?.mensagem || 'Erro ao remover item');
        } finally {
          setLoadingReceita(false);
        }
      }
    });
  };

  // Limpar toda a receita
  const limparReceita = async () => {
    if (!id) return;
    
    setModalConfirmacao({
      aberto: true,
      titulo: 'Limpar Receita',
      mensagem: 'Deseja realmente remover TODOS os itens da receita? Esta ação não pode ser desfeita.',
      variante: 'danger',
      onConfirmar: async () => {
        try {
          setLoadingReceita(true);
          setModalConfirmacao(prev => ({ ...prev, aberto: false }));
          await produtoService.limparReceita(Number(id));
          setItensReceita([]);
        } catch (err: any) {
          alert(err.response?.data?.mensagem || 'Erro ao limpar receita');
        } finally {
          setLoadingReceita(false);
        }
      }
    });
  };

  // Calcular totais da receita
  const totaisReceita = {
    custoTotal: itensReceita.reduce((acc, item) => acc + item.custoTotal, 0),
    pesoTotal: itensReceita.reduce((acc, item) => acc + item.pesoTotal, 0),
    totalItens: itensReceita.length,
  };

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
          {/* Grupo do Produto */}
          <SeletorComBusca
            label="Grupo do Produto"
            value={formData.sequenciaDoGrupoProduto}
            descricao={dadosExtras.grupoProduto || ''}
            onSelect={(id, descricao) => {
              handleChange('sequenciaDoGrupoProduto', id);
              setDadosExtras(prev => ({ ...prev, grupoProduto: descricao }));
              // Limpar SubGrupo quando trocar de Grupo (igual ao VB6)
              handleChange('sequenciaDoSubGrupoProduto', 0);
              setDadosExtras(prev => ({ ...prev, subGrupoProduto: '' }));
            }}
            items={grupos}
            getItemId={(item) => item.sequenciaDoGrupoProduto}
            getItemDescricao={(item) => item.descricao}
            placeholder="Selecione um grupo"
            disabled={isViewMode}
            loading={loadingAuxiliares}
          />

          {/* SubGrupo do Produto */}
          <SeletorComBusca
            label="SubGrupo do Produto"
            value={formData.sequenciaDoSubGrupoProduto}
            descricao={dadosExtras.subGrupoProduto || ''}
            onSelect={(id, descricao) => {
              handleChange('sequenciaDoSubGrupoProduto', id);
              setDadosExtras(prev => ({ ...prev, subGrupoProduto: descricao }));
            }}
            items={subGrupos}
            getItemId={(item) => item.sequenciaDoSubGrupoProduto}
            getItemDescricao={(item) => item.descricao}
            placeholder="Selecione um subgrupo"
            disabled={isViewMode}
            loading={loadingAuxiliares}
          />

          {/* Unidade */}
          <SeletorComBusca
            label="Unidade"
            value={formData.sequenciaDaUnidade}
            descricao={dadosExtras.unidade || ''}
            onSelect={(id, descricao) => {
              handleChange('sequenciaDaUnidade', id);
              setDadosExtras(prev => ({ ...prev, unidade: descricao }));
            }}
            items={unidades}
            getItemId={(item) => item.sequenciaDaUnidade}
            getItemDescricao={(item) => item.descricao}
            getItemSecundario={(item) => item.siglaDaUnidade}
            placeholder="Selecione uma unidade"
            disabled={isViewMode}
            loading={loadingAuxiliares}
          />
        </div>
      </SecaoCard>

      {/* Status e Características */}
      <SecaoCard titulo="Características do Produto" subtitulo="Classificação e tipo do material" icone={<AlertCircle className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {/* Coluna 1: Status (apenas para edição) */}
          {isEditing && (
            <div className="space-y-3">
              <h4 className="text-sm font-semibold text-gray-700 mb-2">Status</h4>
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="checkbox"
                  checked={formData.inativo}
                  onChange={(e) => handleChange('inativo', e.target.checked)}
                  disabled={isViewMode}
                  className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
                />
                <span className="text-sm text-gray-700">Inativo</span>
              </label>
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="checkbox"
                  checked={formData.obsoleto}
                  onChange={(e) => handleChange('obsoleto', e.target.checked)}
                  disabled={isViewMode}
                  className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
                />
                <span className="text-sm text-gray-700">Obsoleto</span>
              </label>
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="checkbox"
                  checked={formData.usadoNoProjeto}
                  onChange={(e) => handleChange('usadoNoProjeto', e.target.checked)}
                  disabled={isViewMode}
                  className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
                />
                <span className="text-sm text-gray-700">Filtrar nos Projetos</span>
              </label>
            </div>
          )}

          {/* Coluna 2: Tipo de Material */}
          <div className="space-y-3">
            <h4 className="text-sm font-semibold text-gray-700 mb-2">Tipo de Material</h4>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.eMateriaPrima}
                onChange={(e) => handleChange('eMateriaPrima', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Matéria Prima</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.materialAdquiridoDeTerceiro}
                onChange={(e) => handleChange('materialAdquiridoDeTerceiro', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Material Ad. de Terceiro</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.industrializacao}
                onChange={(e) => handleChange('industrializacao', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Industrialização</span>
            </label>
          </div>

          {/* Coluna 3: Uso e Origem */}
          <div className="space-y-3">
            <h4 className="text-sm font-semibold text-gray-700 mb-2">Uso e Origem</h4>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.usado}
                onChange={(e) => handleChange('usado', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Usado</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.sucata}
                onChange={(e) => handleChange('sucata', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Sucata</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.importado}
                onChange={(e) => handleChange('importado', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Importado</span>
            </label>
          </div>
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
                    Configure o ClassTrib em <span className="font-medium">Fiscal -&gt; Classificação Fiscal</span> para habilitar os impostos IBS/CBS.
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
            label="Estoque Contábil"
            value={formatQuantity(dadosExtras.quantidadeContabil || 0)}
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
          </div>
        </div>
      </SecaoCard>

      {/* NCM e Classificação Fiscal */}
      <SecaoCard titulo="Classificação Fiscal" subtitulo="NCM e IPI" icone={<ClipboardList className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <div className="flex gap-2 items-end">
            <SeletorComBusca
              label="NCM"
              value={formData.sequenciaDaClassificacao}
              descricao={dadosExtras.ncm || ''}
              onSelect={(id, ncm) => {
                const item = classificacoesBusca.find(c => c.sequenciaDaClassificacao === id);
                handleChange('sequenciaDaClassificacao', id);
                setDadosExtras(prev => ({ 
                  ...prev, 
                  ncm: ncm,
                  classificacaoFiscal: item?.descricaoDoNcm,
                  percentualIpi: item?.porcentagemDoIpi
                }));
              }}
              onSearch={buscarClassificacoes}
              items={classificacoesBusca}
              getItemId={(item) => item.sequenciaDaClassificacao}
              getItemDescricao={(item) => item.ncm.toString()}
              getItemSecundario={(item) => item.descricaoDoNcm}
              placeholder="Busque por NCM ou descrição..."
              disabled={isViewMode}
              loading={buscandoClassificacoes}
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
      </SecaoCard>

      {/* Opções Adicionais */}
      <SecaoCard titulo="Opções Adicionais" subtitulo="Configurações extras do produto" icone={<ClipboardList className="w-5 h-5" />}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {/* Coluna 1: Características Técnicas */}
          <div className="space-y-3">
            <h4 className="text-sm font-semibold text-gray-700 mb-2">Características Técnicas</h4>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.lance}
                onChange={(e) => handleChange('lance', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Lance</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.eRegulador}
                onChange={(e) => handleChange('eRegulador', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">É Regulador</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.travaReceita}
                onChange={(e) => handleChange('travaReceita', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Trava Receita</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.mpInicial}
                onChange={(e) => handleChange('mpInicial', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">M.Prima Inicial</span>
            </label>
          </div>

          {/* Coluna 2: Opções de Exibição */}
          <div className="space-y-3">
            <h4 className="text-sm font-semibold text-gray-700 mb-2">Opções de Exibição</h4>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.naoSairNoChecklist}
                onChange={(e) => handleChange('naoSairNoChecklist', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Não mostrar no Controle de Pedidos</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.naoMostrarReceita}
                onChange={(e) => handleChange('naoMostrarReceita', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Não Mostrar Receita</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.naoSairNoRelatorio}
                onChange={(e) => handleChange('naoSairNoRelatorio', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Não Sair no Relatório da Produção</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.mostrarReceitaSecundaria}
                onChange={(e) => handleChange('mostrarReceitaSecundaria', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Mostrar Receita Secundária</span>
            </label>
          </div>

          {/* Coluna 3: Outras Opções */}
          <div className="space-y-3">
            <h4 className="text-sm font-semibold text-gray-700 mb-2">Outras Opções</h4>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.pesoOk}
                onChange={(e) => handleChange('pesoOk', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Peso Conferido</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.conferidoPeloContabil}
                onChange={(e) => handleChange('conferidoPeloContabil', e.target.checked)}
                disabled={isViewMode}
                className="w-4 h-4 text-blue-600 rounded focus:ring-2 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">NCM - Conferido pela Contabilidade</span>
            </label>
          </div>
        </div>
      </SecaoCard>
    </div>
  );

  // Aba 2 - Receita (materias primas)
  const renderReceita = () => (
    <div className="p-6 space-y-6">
      {/* Adicionar item - somente em modo edicao */}
      {isEditing && !isViewMode && (
        <SecaoCard titulo="Adicionar Materia Prima" subtitulo="Busque e adicione produtos a receita" icone={<Plus className="w-5 h-5" />}>
          <div className="flex flex-col md:flex-row gap-4">
            {/* Busca de Produto */}
            <div className="flex-1 relative">
              <div className="relative">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400" />
                <input
                  type="text"
                  value={produtoSelecionado ? `${produtoSelecionado.sequenciaDoProduto} - ${produtoSelecionado.descricao}` : termoBuscaProduto}
                  onChange={(e) => {
                    setTermoBuscaProduto(e.target.value);
                    setProdutoSelecionado(null);
                  }}
                  placeholder="Busque por código ou descrição..."
                  className="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-xl focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 outline-none"
                />
                {buscandoProdutos && (
                  <Loader2 className="absolute right-3 top-1/2 transform -translate-y-1/2 w-4 h-4 animate-spin text-blue-500" />
                )}
              </div>
              
              {/* Lista de resultados */}
              {produtosBusca.length > 0 && !produtoSelecionado && (
                <div className="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded-xl shadow-lg max-h-60 overflow-y-auto">
                  {produtosBusca.map((produto) => (
                    <button
                      key={produto.sequenciaDoProduto}
                      type="button"
                      onClick={() => {
                        setProdutoSelecionado(produto);
                        setTermoBuscaProduto('');
                        setProdutosBusca([]);
                      }}
                      className="w-full px-4 py-2 text-left hover:bg-blue-50 flex justify-between items-center border-b border-gray-50 last:border-0"
                    >
                      <div className="flex flex-col">
                        <span className="text-sm font-medium text-gray-900">{produto.descricao}</span>
                        <span className="text-xs text-gray-500">Código: {produto.sequenciaDoProduto}</span>
                      </div>
                      <span className="text-xs font-semibold px-2 py-1 bg-gray-100 text-gray-600 rounded-md">{produto.unidade}</span>
                    </button>
                  ))}
                </div>
              )}
            </div>

            {/* Quantidade */}
            <div className="w-32">
              <InputModerno
                label="Quantidade"
                value={quantidadeItem}
                onChange={setQuantidadeItem}
                type="text"
              />
            </div>

            {/* Botao Adicionar */}
            <button
              type="button"
              onClick={adicionarItemReceita}
              disabled={!produtoSelecionado || loadingReceita}
              className="px-6 py-3 bg-blue-600 text-white rounded-xl hover:bg-blue-700 disabled:bg-gray-300 disabled:cursor-not-allowed flex items-center gap-2"
            >
              {loadingReceita ? (
                <Loader2 className="w-4 h-4 animate-spin" />
              ) : (
                <Plus className="w-4 h-4" />
              )}
              Adicionar
            </button>
          </div>
        </SecaoCard>
      )}

      {/* Lista de itens da receita */}
      <SecaoCard titulo="Receita do Produto" subtitulo={`${totaisReceita.totalItens} itens na composicao`} icone={<ClipboardList className="w-5 h-5" />}>
        {loadingReceita ? (
          <div className="flex items-center justify-center py-12">
            <Loader2 className="w-8 h-8 animate-spin text-blue-500" />
          </div>
        ) : itensReceita.length === 0 ? (
          <div className="text-center py-12 text-gray-500">
            <ClipboardList className="w-12 h-12 mx-auto mb-4 text-gray-300" />
            <p className="text-lg font-medium">Nenhuma materia prima cadastrada</p>
            <p className="text-sm">Adicione produtos para compor a receita</p>
          </div>
        ) : (
          <>
            {/* Tabela de itens */}
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b border-gray-200 bg-gray-50">
                    <th className="text-left py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Produto</th>
                    <th className="text-right py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Qtde</th>
                    <th className="text-center py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Un</th>
                    <th className="text-right py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Peso</th>
                    <th className="text-right py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Custo</th>
                    <th className="text-right py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Custo Total</th>
                    <th className="text-right py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Peso Total</th>
                    {!isViewMode && <th className="text-center py-3 px-4 text-xs font-semibold text-gray-600 uppercase">Acoes</th>}
                  </tr>
                </thead>
                <tbody>
                  {itensReceita.map((item) => (
                    <tr key={item.sequenciaDaMateriaPrima} className="border-b border-gray-100 hover:bg-gray-50">
                      <td className="py-3 px-4 text-sm text-gray-900">{item.descricaoDaMateriaPrima}</td>
                      <td className="py-3 px-4 text-sm text-right">
                        {editandoItem === item.sequenciaDaMateriaPrima ? (
                          <input
                            type="text"
                            value={quantidadeEdicao}
                            onChange={(e) => setQuantidadeEdicao(e.target.value)}
                            onKeyDown={(e) => {
                              if (e.key === 'Enter') atualizarItemReceita(item.sequenciaDaMateriaPrima);
                              if (e.key === 'Escape') { setEditandoItem(null); setQuantidadeEdicao(''); }
                            }}
                            className="w-20 px-2 py-1 border border-blue-400 rounded text-right focus:outline-none focus:ring-2 focus:ring-blue-500"
                            autoFocus
                          />
                        ) : (
                          <span
                            className={!isViewMode ? 'cursor-pointer hover:text-blue-600' : ''}
                            onClick={() => {
                              if (!isViewMode) {
                                setEditandoItem(item.sequenciaDaMateriaPrima);
                                setQuantidadeEdicao(item.quantidade.toString().replace('.', ','));
                              }
                            }}
                          >
                            {formatQuantity(item.quantidade)}
                          </span>
                        )}
                      </td>
                      <td className="py-3 px-4 text-sm text-center text-gray-500">{item.unidade}</td>
                      <td className="py-3 px-4 text-sm text-right text-gray-500">{formatQuantity(item.peso)}</td>
                      <td className="py-3 px-4 text-sm text-right text-gray-500">{formatCurrency(item.valorDeCusto)}</td>
                      <td className="py-3 px-4 text-sm text-right font-medium text-gray-900">{formatCurrency(item.custoTotal)}</td>
                      <td className="py-3 px-4 text-sm text-right text-gray-500">{formatQuantity(item.pesoTotal)}</td>
                      {!isViewMode && (
                        <td className="py-3 px-4 text-center">
                          <div className="flex items-center justify-center gap-2">
                            {editandoItem === item.sequenciaDaMateriaPrima ? (
                              <>
                                <button
                                  type="button"
                                  onClick={() => atualizarItemReceita(item.sequenciaDaMateriaPrima)}
                                  className="p-1.5 text-green-600 hover:bg-green-50 rounded-lg"
                                  title="Confirmar"
                                >
                                  <CheckCircle2 className="w-4 h-4" />
                                </button>
                                <button
                                  type="button"
                                  onClick={() => { setEditandoItem(null); setQuantidadeEdicao(''); }}
                                  className="p-1.5 text-gray-600 hover:bg-gray-100 rounded-lg"
                                  title="Cancelar"
                                >
                                  <X className="w-4 h-4" />
                                </button>
                              </>
                            ) : (
                              <button
                                type="button"
                                onClick={() => removerItemReceita(item.sequenciaDaMateriaPrima, item.descricaoDaMateriaPrima)}
                                className="p-1.5 text-red-600 hover:bg-red-50 rounded-lg"
                                title="Remover"
                              >
                                <Trash2 className="w-4 h-4" />
                              </button>
                            )}
                          </div>
                        </td>
                      )}
                    </tr>
                  ))}
                </tbody>
                {/* Totalizadores */}
                <tfoot>
                  <tr className="bg-gray-100 font-semibold">
                    <td className="py-3 px-4 text-sm text-gray-900">Total ({totaisReceita.totalItens} itens)</td>
                    <td colSpan={4}></td>
                    <td className="py-3 px-4 text-sm text-right text-blue-600">{formatCurrency(totaisReceita.custoTotal)}</td>
                    <td className="py-3 px-4 text-sm text-right text-gray-700">{formatQuantity(totaisReceita.pesoTotal)}</td>
                    {!isViewMode && <td></td>}
                  </tr>
                </tfoot>
              </table>
            </div>

            {/* Botao limpar receita */}
            {!isViewMode && itensReceita.length > 0 && (
              <div className="mt-4 flex justify-end">
                <button
                  type="button"
                  onClick={limparReceita}
                  disabled={loadingReceita}
                  className="px-4 py-2 text-red-600 border border-red-200 rounded-lg hover:bg-red-50 flex items-center gap-2 text-sm"
                >
                  <Trash2 className="w-4 h-4" />
                  Limpar Receita (F11)
                </button>
              </div>
            )}
          </>
        )}
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
            label="Estoque Contábil"
            value={formatQuantity(dadosExtras.quantidadeContabil || 0)}
            onChange={() => {}}
            disabled={true}
            icone={<Package className="w-4 h-4" />}
          />
        </div>
      </SecaoCard>
    </div>
  );

  // Aba 4 - Detalhes
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
          ? 'bg-gradient-to-r from-indigo-600 via-blue-600 to-cyan-500 -mx-4 sm:-mx-6 px-4 sm:px-6 py-4 rounded-xl' 
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

        {/* Cabeçalho Principal - Card com Foto e Informações */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden">
          <div className="p-6">
            <div className="flex gap-6">
              {/* Foto do Produto - Compacta com hover actions (só em edição) */}
              {(isEditing || isViewMode) && (
                <div className="flex-shrink-0">
                  <div className="relative group">
                    {/* Container da Foto */}
                    <div 
                      className={`w-32 h-32 rounded-xl border-2 border-gray-200 overflow-hidden bg-white shadow-sm ${fotoUrl && !fotoError ? 'cursor-pointer' : ''}`}
                      onClick={() => fotoUrl && !fotoError && setFotoExpandida(true)}
                    >
                      {loadingFoto || uploadingFoto ? (
                        <div className="w-full h-full flex flex-col items-center justify-center bg-gray-50">
                          <Loader2 className="w-6 h-6 animate-spin text-blue-500" />
                          <span className="text-[10px] text-gray-400 mt-1">
                            {uploadingFoto ? 'Enviando...' : 'Carregando...'}
                          </span>
                        </div>
                      ) : fotoUrl && !fotoError ? (
                        <img
                          src={fotoUrl}
                          alt={`Foto do produto ${formData.descricao}`}
                          className="w-full h-full object-cover"
                          onError={() => setFotoError(true)}
                        />
                      ) : (
                        <div className="w-full h-full flex flex-col items-center justify-center text-gray-300 bg-gray-50">
                          <Camera className="w-8 h-8" />
                          <span className="text-[10px] mt-1">Sem foto</span>
                        </div>
                      )}
                    </div>
                    
                    {/* Overlay de ações no hover */}
                    {!uploadingFoto && fotoUrl && !fotoError && (
                      <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity rounded-xl flex items-center justify-center gap-2">
                        {/* Botão expandir - sempre visível */}
                        <button
                          type="button"
                          onClick={() => setFotoExpandida(true)}
                          className="p-2 bg-white rounded-lg hover:bg-gray-100 transition-colors shadow-sm"
                          title="Expandir foto"
                        >
                          <ZoomIn className="w-4 h-4 text-gray-600" />
                        </button>
                        
                        {/* Botões de edição - apenas no modo edição */}
                        {!isViewMode && (
                          <>
                            <button
                              type="button"
                              onClick={() => fileInputRef.current?.click()}
                              className="p-2 bg-white rounded-lg hover:bg-gray-100 transition-colors shadow-sm"
                              title="Alterar foto"
                            >
                              <Upload className="w-4 h-4 text-blue-600" />
                            </button>
                            <button
                              type="button"
                              onClick={handleRemoverFoto}
                              className="p-2 bg-white rounded-lg hover:bg-gray-100 transition-colors shadow-sm"
                              title="Remover foto"
                            >
                              <Trash2 className="w-4 h-4 text-red-600" />
                            </button>
                          </>
                        )}
                      </div>
                    )}
                    
                    {/* Overlay apenas para alterar foto quando não tem foto - modo edição */}
                    {!isViewMode && !uploadingFoto && (!fotoUrl || fotoError) && (
                      <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity rounded-xl flex items-center justify-center">
                        <button
                          type="button"
                          onClick={() => fileInputRef.current?.click()}
                          className="p-2 bg-white rounded-lg hover:bg-gray-100 transition-colors shadow-sm"
                          title="Adicionar foto"
                        >
                          <Upload className="w-4 h-4 text-blue-600" />
                        </button>
                      </div>
                    )}
                  </div>
                  
                  {/* Input de arquivo oculto - apenas no modo edição */}
                  {!isViewMode && (
                    <input
                      ref={fileInputRef}
                      type="file"
                      accept="image/*"
                      onChange={handleUploadFoto}
                      className="hidden"
                    />
                  )}
                </div>
              )}
              
              {/* Informações do Produto - Lado Direito */}
              <div className="flex-1 flex flex-col justify-center gap-4">
                {/* Linha 1: Descrição */}
                <InputModerno
                  label="Descrição"
                  value={formData.descricao}
                  onChange={(v) => handleChange('descricao', v)}
                  required
                  disabled={isViewMode}
                  icone={<Package className="w-4 h-4" />}
                />
                
                {/* Linha 2: Badges informativos */}
                <div className="flex flex-wrap items-center gap-3">
                  {/* Código */}
                  <div className="flex items-center gap-1.5 px-3 py-1.5 bg-blue-50 rounded-lg">
                    <Hash className="w-3.5 h-3.5 text-blue-500" />
                    <span className="text-sm font-medium text-blue-700">Código: {id}</span>
                  </div>
                  
                  {/* Status Inativo */}
                  {formData.inativo && (
                    <div className="flex items-center gap-1.5 px-3 py-1.5 bg-red-50 rounded-lg">
                      <XCircle className="w-3.5 h-3.5 text-red-500" />
                      <span className="text-sm font-medium text-red-600">Inativo</span>
                    </div>
                  )}
                  
                  {/* Status Obsoleto */}
                  {formData.obsoleto && (
                    <div className="flex items-center gap-1.5 px-3 py-1.5 bg-orange-50 rounded-lg">
                      <AlertCircle className="w-3.5 h-3.5 text-orange-500" />
                      <span className="text-sm font-medium text-orange-600">Obsoleto</span>
                    </div>
                  )}
                  
                  {/* Matéria Prima */}
                  {formData.eMateriaPrima && (
                    <div className="flex items-center gap-1.5 px-3 py-1.5 bg-green-50 rounded-lg">
                      <Box className="w-3.5 h-3.5 text-green-500" />
                      <span className="text-sm font-medium text-green-600">Matéria Prima</span>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
        {/* Abas Modernas */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm">
          {/* Tab Headers */}
          <div className="flex border-b border-gray-100 bg-gray-50/50 overflow-x-auto">
            {[
              { id: 'dados' as TabType, label: '1 - Dados Principais', icon: FileText },
              { id: 'receita' as TabType, label: '2 - Receita', icon: ClipboardList },
              { id: 'contabilidade' as TabType, label: '3 - Contabilidade', icon: Calculator },
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

      {/* Modal de Foto Expandida */}
      {fotoExpandida && fotoUrl && (
        <div 
          className="fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-sm animate-in fade-in duration-200"
          onClick={() => setFotoExpandida(false)}
        >
          <div className="relative max-w-4xl max-h-[90vh] p-4">
            {/* Botão fechar */}
            <button
              type="button"
              onClick={() => setFotoExpandida(false)}
              className="absolute -top-2 -right-2 z-10 p-2 bg-white rounded-full shadow-lg hover:bg-gray-100 transition-colors"
              title="Fechar"
            >
              <X className="w-5 h-5 text-gray-600" />
            </button>
            
            {/* Imagem expandida */}
            <img
              src={fotoUrl}
              alt={`Foto do produto ${formData.descricao}`}
              className="max-w-full max-h-[85vh] object-contain rounded-xl shadow-2xl"
              onClick={(e) => e.stopPropagation()}
            />
            
            {/* Legenda */}
            <div className="absolute bottom-0 left-4 right-4 p-4 bg-gradient-to-t from-black/70 to-transparent rounded-b-xl">
              <p className="text-white font-medium text-center">{formData.descricao}</p>
              <p className="text-white/70 text-sm text-center">Código: {id}</p>
            </div>
          </div>
        </div>
      )}

      {/* Modal de Confirmação */}
      <ModalConfirmacao
        aberto={modalConfirmacao.aberto}
        titulo={modalConfirmacao.titulo}
        mensagem={modalConfirmacao.mensagem}
        nomeItem={modalConfirmacao.nomeItem}
        variante={modalConfirmacao.variante}
        onConfirmar={modalConfirmacao.onConfirmar}
        onCancelar={() => setModalConfirmacao(prev => ({ ...prev, aberto: false }))}
        processando={loadingReceita}
      />
    </div>
  );
}
