import { useState, useEffect } from 'react';
import { 
  FileText, 
  Package, 
  AlertCircle,
  Plus,
  Trash2,
  Save,
  Loader2,
  Printer,
  Layers,
  Receipt,
  DollarSign,
  ArrowDownLeft,
  ArrowUpRight,
  X,
  Zap,
  Calculator,
  CreditCard,
  Calendar,
  Hash,
  User,
  ChevronDown
} from 'lucide-react';
import { movimentoContabilService } from '../../services/MovimentoContabil/movimentoContabilService';
import { SeletorComBusca } from '../../components/SeletorComBusca';
import type { 
  MovimentoContabilNovoDto, 
  ProdutoMvtoContabilItemDto, 
  ConjuntoMvtoContabilItemDto,
  DespesaMvtoContabilItemDto,
  ParcelaMvtoContabilDto,
  EstoqueInfoDto
} from '../../types/Estoque/movimentoContabil';
import { GeralSearch } from '../../components/GeralSearch';
import { ProducaoInteligenteModal } from './ProducaoInteligenteModal';

// ============================================================================
// COMPONENTES DE UI REUTILIZÁVEIS (PADRÃO CORPORATIVO)
// ============================================================================

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
        <ChevronDown className="w-4 h-4 text-gray-400 mr-3" />
      </div>
      <label className="absolute -top-2.5 left-3 text-xs text-gray-500 bg-white px-1 rounded">
        {label}
      </label>
    </div>
  );
}

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

interface MovimentoContabilFormProps {
  movimentoId?: number;
  onClose: () => void;
  onSuccess: () => void;
}

type Tab = 'dados' | 'produtos' | 'conjuntos' | 'despesas' | 'financeiro';

export function MovimentoContabilForm({ movimentoId, onClose, onSuccess }: MovimentoContabilFormProps) {
  const [activeTab, setActiveTab] = useState<Tab>('produtos');
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Estado do formulário
  const [formData, setFormData] = useState<MovimentoContabilNovoDto>({
    sequenciaDoMovimento: 0,
    dataDoMovimento: new Date().toISOString().split('T')[0],
    tipoDoMovimento: 0, // Entrada
    documento: '',
    sequenciaDoGeral: 0,
    razaoSocialGeral: '',
    observacao: '',
    devolucao: false,
    produtos: [],
    conjuntos: [],
    despesas: [],
    parcelas: [],
    valorDoFrete: 0,
    valorDoDesconto: 0,
    valorTotalDosProdutos: 0,
    valorTotalDoMovimento: 0
  });

  // Estados para adição de itens
  const [buscaItem, setBuscaItem] = useState('');
  const [resultadosBusca, setResultadosBusca] = useState<EstoqueInfoDto[]>([]);
  const [loadingBusca, setLoadingBusca] = useState(false);
  const [itemSelecionado, setItemSelecionado] = useState<EstoqueInfoDto | null>(null);
  const [quantidadeItem, setQuantidadeItem] = useState<string>('');
  const [valorUnitarioItem, setValorUnitarioItem] = useState<string>('');
  const [modoAjuste, setModoAjuste] = useState(false);
  const [quantidadeFisica, setQuantidadeFisica] = useState<string>('');

  // Estados para despesas
  const [buscaDespesa, setBuscaDespesa] = useState('');
  const [despesasEncontradas, setDespesasEncontradas] = useState<any[]>([]);
  const [loadingDespesa, setLoadingDespesa] = useState(false);
  const [despesaSelecionada, setDespesaSelecionada] = useState<any | null>(null);
  const [quantidadeDespesa, setQuantidadeDespesa] = useState<string>('1');
  const [valorDespesa, setValorDespesa] = useState<string>('');

  // Estados para geração de parcelas
  const [numParcelas, setNumParcelas] = useState(1);
  const [intervaloDias, setIntervaloDias] = useState(30);

  // Estados para Produção Inteligente
  const [showProducaoModal, setShowProducaoModal] = useState(false);
  const [producaoItem, setProducaoItem] = useState<{ id: number; descricao: string; ehConjunto: boolean } | null>(null);

  useEffect(() => {
    if (movimentoId) {
      carregarMovimento();
    }
  }, [movimentoId]);

  const carregarMovimento = async () => {
    try {
      setLoading(true);
      const data = await movimentoContabilService.obterMovimento(movimentoId!);
      setFormData({
        ...data,
        dataDoMovimento: data.dataDoMovimento.split('T')[0]
      });
    } catch (err) {
      setError('Erro ao carregar movimento.');
    } finally {
      setLoading(false);
    }
  };

  const handleSave = async () => {
    try {
      // Validações
      if (!formData.sequenciaDoGeral) {
        alert('Selecione um fornecedor/cliente');
        return;
      }

      if (formData.produtos.length === 0 && formData.conjuntos.length === 0 && formData.despesas.length === 0) {
        alert('Adicione pelo menos um item (produto, conjunto ou despesa)');
        return;
      }

      // Validar financeiro
      const totalParcelas = formData.parcelas.reduce((sum, p) => sum + p.valorDaParcela, 0);
      if (formData.parcelas.length > 0 && Math.abs(totalParcelas - formData.valorTotalDoMovimento) > 0.01) {
        alert(`O total das parcelas (R$ ${totalParcelas.toFixed(2)}) deve ser igual ao total do movimento (R$ ${formData.valorTotalDoMovimento.toFixed(2)})`);
        return;
      }

      setSaving(true);
      if (movimentoId) {
        await movimentoContabilService.atualizarMovimento(movimentoId, formData);
      } else {
        await movimentoContabilService.criarMovimento(formData);
      }
      onSuccess();
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao salvar movimento.');
    } finally {
      setSaving(false);
    }
  };

  // Busca de itens (produtos/conjuntos)
  useEffect(() => {
    const delayDebounceFn = setTimeout(async () => {
      if (buscaItem.length >= 1) {
        setLoadingBusca(true);
        try {
          const data = activeTab === 'produtos' 
            ? await movimentoContabilService.buscarProdutos(buscaItem)
            : await movimentoContabilService.buscarConjuntos(buscaItem);
          setResultadosBusca(data);
        } catch (err) {
          console.error('Erro ao buscar itens:', err);
        } finally {
          setLoadingBusca(false);
        }
      } else {
        setResultadosBusca([]);
      }
    }, 300);

    return () => clearTimeout(delayDebounceFn);
  }, [buscaItem, activeTab]);

  // Busca de despesas
  useEffect(() => {
    const delayDebounceFn = setTimeout(async () => {
      if (buscaDespesa.length >= 1) {
        setLoadingDespesa(true);
        try {
          const data = await movimentoContabilService.buscarDespesas(buscaDespesa);
          setDespesasEncontradas(data);
        } catch (err) {
          console.error('Erro ao buscar despesas:', err);
        } finally {
          setLoadingDespesa(false);
        }
      } else {
        setDespesasEncontradas([]);
      }
    }, 300);

    return () => clearTimeout(delayDebounceFn);
  }, [buscaDespesa]);

  const adicionarItem = () => {
    if (!itemSelecionado) return;

    const qtd = parseFloat(quantidadeItem);
    const vlr = parseFloat(valorUnitarioItem);

    if (isNaN(qtd) || qtd <= 0) {
      alert('Informe uma quantidade válida.');
      return;
    }

    if (activeTab === 'produtos') {
      const novoItem: ProdutoMvtoContabilItemDto = {
        sequenciaDoProdutoMvtoNovo: 0,
        sequenciaDoProduto: itemSelecionado.sequenciaDoProduto,
        descricaoProduto: itemSelecionado.descricao,
        quantidade: qtd,
        valorUnitario: vlr,
        valorDeCusto: itemSelecionado.valorCusto,
        valorTotal: qtd * vlr,
        valorDoPis: 0,
        valorDoCofins: 0,
        valorDoIpi: 0,
        valorDoIcms: 0,
        valorDoFrete: 0,
        valorDaSubstituicao: 0
      };
      setFormData({
        ...formData,
        produtos: [...formData.produtos, novoItem]
      });
    } else {
      const novoConj: ConjuntoMvtoContabilItemDto = {
        sequenciaConjuntoMvtoNovo: 0,
        sequenciaDoConjunto: itemSelecionado.sequenciaDoProduto,
        descricaoConjunto: itemSelecionado.descricao,
        quantidade: qtd,
        valorUnitario: vlr,
        valorTotal: qtd * vlr
      };
      setFormData({
        ...formData,
        conjuntos: [...formData.conjuntos, novoConj]
      });
    }

    // Limpar campos
    setItemSelecionado(null);
    setBuscaItem('');
    setQuantidadeItem('');
    setValorUnitarioItem('');
    setModoAjuste(false);
    setQuantidadeFisica('');
  };

  const adicionarDespesa = () => {
    if (!despesaSelecionada) return;

    const qtd = parseFloat(quantidadeDespesa) || 0;
    const vlr = parseFloat(valorDespesa) || 0;

    if (qtd <= 0 || vlr <= 0) {
      alert('Informe quantidade e valor válidos.');
      return;
    }

    const novaDesp: DespesaMvtoContabilItemDto = {
      sequenciaDespesaMvtoNovo: 0,
      sequenciaDaDespesa: despesaSelecionada.sequenciaDaDespesa,
      descricaoDespesa: despesaSelecionada.descricaoDespesa,
      quantidade: qtd,
      valorUnitario: vlr,
      valorDeCusto: vlr,
      valorTotal: qtd * vlr
    };

    setFormData({
      ...formData,
      despesas: [...formData.despesas, novaDesp]
    });

    // Limpar campos
    setDespesaSelecionada(null);
    setBuscaDespesa('');
    setQuantidadeDespesa('1');
    setValorDespesa('');
  };

  const removerProduto = (index: number) => {
    const novos = [...formData.produtos];
    novos.splice(index, 1);
    setFormData({ ...formData, produtos: novos });
  };

  // Abre modal de produção inteligente
  const abrirProducaoInteligente = () => {
    if (!itemSelecionado) {
      alert('Selecione um item primeiro.');
      return;
    }
    if (!formData.sequenciaDoGeral) {
      alert('Selecione um fornecedor/cliente antes de usar a Produção Inteligente.');
      return;
    }
    setProducaoItem({
      id: itemSelecionado.sequenciaDoProduto,
      descricao: itemSelecionado.descricao,
      ehConjunto: activeTab === 'conjuntos'
    });
    setShowProducaoModal(true);
  };

  const removerConjunto = (index: number) => {
    const novos = [...formData.conjuntos];
    novos.splice(index, 1);
    setFormData({ ...formData, conjuntos: novos });
  };

  const removerDespesa = (index: number) => {
    const novos = [...formData.despesas];
    novos.splice(index, 1);
    setFormData({ ...formData, despesas: novos });
  };

  // Recalcular totais
  useEffect(() => {
    const totalProdutos = formData.produtos.reduce((acc, curr) => acc + curr.valorTotal, 0) +
                         formData.conjuntos.reduce((acc, curr) => acc + curr.valorTotal, 0);
    const totalDespesas = formData.despesas.reduce((acc, curr) => acc + curr.valorTotal, 0);
    const frete = formData.valorDoFrete || 0;
    const desconto = formData.valorDoDesconto || 0;
    
    setFormData(prev => ({
      ...prev,
      valorTotalDosProdutos: totalProdutos,
      valorTotalDoMovimento: totalProdutos + totalDespesas + frete - desconto
    }));
  }, [formData.produtos, formData.conjuntos, formData.despesas, formData.valorDoFrete, formData.valorDoDesconto]);

  // Calcular quantidade baseada no ajuste
  useEffect(() => {
    if (modoAjuste && itemSelecionado) {
      const fisica = parseFloat(quantidadeFisica) || 0;
      const contabil = itemSelecionado.estoqueContabil;
      const diff = fisica - contabil;
      setQuantidadeItem(Math.abs(diff).toString());
    }
  }, [quantidadeFisica, modoAjuste, itemSelecionado]);

  const gerarParcelas = () => {
    const novasParcelas: ParcelaMvtoContabilDto[] = [];
    const valorParcela = Math.floor((formData.valorTotalDoMovimento / numParcelas) * 100) / 100;
    let totalDistribuido = 0;

    for (let i = 1; i <= numParcelas; i++) {
      const dataVenc = new Date(formData.dataDoMovimento);
      dataVenc.setDate(dataVenc.getDate() + (i * intervaloDias));

      const valor = i === numParcelas 
        ? Number((formData.valorTotalDoMovimento - totalDistribuido).toFixed(2))
        : valorParcela;

      novasParcelas.push({
        numeroDaParcela: i,
        dataDeVencimento: dataVenc.toISOString().split('T')[0],
        valorDaParcela: valor
      });

      totalDistribuido += valor;
    }

    setFormData({ ...formData, parcelas: novasParcelas });
  };

  const atualizarParcela = (index: number, campo: keyof ParcelaMvtoContabilDto, valor: any) => {
    const novas = [...formData.parcelas];
    novas[index] = { ...novas[index], [campo]: valor };
    setFormData({ ...formData, parcelas: novas });
  };

  const removerParcela = (index: number) => {
    setFormData({
      ...formData,
      parcelas: formData.parcelas.filter((_, i) => i !== index)
    });
  };

  const adicionarParcelaManual = () => {
    const proximoNumero = formData.parcelas.length > 0 
      ? Math.max(...formData.parcelas.map(p => p.numeroDaParcela)) + 1 
      : 1;
    
    const totalAtual = formData.parcelas.reduce((sum, p) => sum + p.valorDaParcela, 0);
    const valorRestante = Math.max(0, formData.valorTotalDoMovimento - totalAtual);

    setFormData({
      ...formData,
      parcelas: [...formData.parcelas, {
        numeroDaParcela: proximoNumero,
        dataDeVencimento: new Date().toISOString().split('T')[0],
        valorDaParcela: Number(valorRestante.toFixed(2))
      }]
    });
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-full min-h-[400px]">
        <Loader2 className="w-8 h-8 text-blue-600 animate-spin" />
      </div>
    );
  }

  return (
    <div className="flex flex-col h-full bg-surface">
      {/* ========== CABEÇALHO PRINCIPAL ========== */}
      <div className="bg-white border-b border-border px-6 py-4 shadow-sm z-10">
        <div className="flex items-center justify-between mb-6">
          {/* Lado Esquerdo - Título e Tipo */}
          <div className="flex items-center gap-4">
            <div className="w-12 h-12 bg-blue-600 rounded-xl flex items-center justify-center shadow-lg shadow-blue-600/20">
              <Package className="w-6 h-6 text-white" />
            </div>
            <div>
              <h1 className="text-xl font-bold text-foreground leading-tight">
                {movimentoId 
                  ? `Movimento #${formData.sequenciaDoMovimento}` 
                  : 'Novo Movimento de Estoque'}
              </h1>
              <div className="flex items-center gap-3 mt-1">
                <div className="flex bg-surface border border-border rounded-lg p-0.5">
                  <button
                    onClick={() => setFormData({ ...formData, tipoDoMovimento: 0 })}
                    className={`flex items-center gap-1.5 px-3 py-1 rounded-md text-xs font-bold transition-all ${
                      formData.tipoDoMovimento === 0 
                        ? 'bg-emerald-500 text-white shadow-sm' 
                        : 'text-muted-foreground hover:text-foreground'
                    }`}
                  >
                    <ArrowDownLeft className="w-3 h-3" /> Entrada
                  </button>
                  <button
                    onClick={() => setFormData({ ...formData, tipoDoMovimento: 1 })}
                    className={`flex items-center gap-1.5 px-3 py-1 rounded-md text-xs font-bold transition-all ${
                      formData.tipoDoMovimento === 1 
                        ? 'bg-amber-500 text-white shadow-sm' 
                        : 'text-muted-foreground hover:text-foreground'
                    }`}
                  >
                    <ArrowUpRight className="w-3 h-3" /> Saída
                  </button>
                </div>
                {formData.devolucao && (
                  <span className="px-2 py-1 rounded-md text-[10px] font-bold bg-purple-100 text-purple-700 border border-purple-200 uppercase tracking-wider">
                    Devolução
                  </span>
                )}
              </div>
            </div>
          </div>

          {/* Lado Direito - Ações Rápidas */}
          <div className="flex items-center gap-3">
            <label className="flex items-center gap-2 px-3 py-2 bg-surface border border-border rounded-xl cursor-pointer hover:bg-surface-hover transition-colors group">
              <input
                type="checkbox"
                checked={formData.devolucao}
                onChange={(e) => setFormData({ ...formData, devolucao: e.target.checked })}
                className="w-4 h-4 text-purple-600 rounded border-gray-300 focus:ring-purple-500"
              />
              <span className="text-sm font-semibold text-muted-foreground group-hover:text-foreground transition-colors">Devolução</span>
            </label>
            <div className="h-8 w-px bg-border mx-1"></div>
            <button
              className="flex items-center gap-2 px-4 py-2 bg-surface border border-border rounded-xl hover:bg-surface-hover transition-colors font-bold text-sm text-muted-foreground hover:text-foreground"
              title="Imprimir"
            >
              <Printer className="w-4 h-4" />
              Imprimir
            </button>
          </div>
        </div>

        {/* Grid de Campos Principais */}
        <div className="grid grid-cols-12 gap-6">
          {/* Seção Entidade - Destaque */}
          <div className="col-span-12 lg:col-span-8">
            <SecaoCard
              titulo="Entidade"
              subtitulo="Selecione o cliente ou fornecedor deste movimento"
              icone={<User className="w-5 h-5" />}
              className="h-full"
            >
              <GeralSearch
                value={formData.sequenciaDoGeral}
                descricao={formData.razaoSocialGeral || ''}
                onSelect={(id, desc) => setFormData({ ...formData, sequenciaDoGeral: id, razaoSocialGeral: desc })}
                required
                label="Cliente / Fornecedor"
              />
            </SecaoCard>
          </div>

          {/* Seção Resumo Rápido */}
          <div className="col-span-12 lg:col-span-4">
            <div className="bg-blue-600 rounded-2xl p-6 text-white shadow-lg shadow-blue-600/20 h-full flex flex-col justify-center">
              <div className="text-blue-100 text-xs font-bold uppercase tracking-wider mb-1">Valor Total do Movimento</div>
              <div className="text-3xl font-black">
                {formData.valorTotalDoMovimento.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
              </div>
              <div className="mt-4 flex items-center gap-4 text-xs font-medium text-blue-100">
                <div className="flex items-center gap-1">
                  <Package className="w-3 h-3" /> {formData.produtos.length + formData.conjuntos.length} Itens
                </div>
                <div className="flex items-center gap-1">
                  <Receipt className="w-3 h-3" /> {formData.despesas.length} Despesas
                </div>
              </div>
            </div>
          </div>

          {/* Seção Dados do Documento */}
          <div className="col-span-12">
            <SecaoCard
              titulo="Informações do Documento"
              subtitulo="Dados fiscais e vínculos de pedidos"
              icone={<FileText className="w-5 h-5" />}
            >
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
                <div className="lg:col-span-1">
                  <InputModerno
                    label="Sequência"
                    value={formData.sequenciaDoMovimento || 'NOVO'}
                    onChange={() => {}}
                    disabled
                    icone={<Hash className="w-4 h-4" />}
                  />
                </div>
                <div className="lg:col-span-1">
                  <InputModerno
                    label="Data"
                    type="date"
                    value={formData.dataDoMovimento}
                    onChange={(val) => setFormData({ ...formData, dataDoMovimento: val })}
                    required
                    icone={<Calendar className="w-4 h-4" />}
                  />
                </div>
                <div className="lg:col-span-1">
                  <InputModerno
                    label="Documento / NF"
                    value={formData.documento}
                    onChange={(val) => setFormData({ ...formData, documento: val })}
                    required
                    placeholder="Ex: 123456"
                    icone={<FileText className="w-4 h-4" />}
                  />
                </div>
                <div className="lg:col-span-1">
                  <InputModerno
                    label="Ped. Compra"
                    value=""
                    onChange={() => {}}
                    placeholder="-"
                    icone={<ArrowDownLeft className="w-4 h-4" />}
                  />
                </div>
                <div className="lg:col-span-1">
                  <InputModerno
                    label="Ped. Venda"
                    value=""
                    onChange={() => {}}
                    placeholder="-"
                    icone={<ArrowUpRight className="w-4 h-4" />}
                  />
                </div>
              </div>
            </SecaoCard>
          </div>
        </div>
      </div>

      {/* ========== ABAS ========== */}
      <div className="bg-white border-b border-border shadow-sm">
        <div className="flex px-6 overflow-x-auto no-scrollbar">
          {[
            { key: 'produtos', label: `Produtos`, count: formData.produtos.length, icon: Package },
            { key: 'conjuntos', label: `Conjuntos`, count: formData.conjuntos.length, icon: Layers },
            { key: 'despesas', label: `Despesas`, count: formData.despesas.length, icon: Receipt },
            { key: 'financeiro', label: 'Financeiro', icon: DollarSign },
            { key: 'dados', label: 'Observações', icon: FileText },
          ].map((tab) => (
            <button
              key={tab.key}
              onClick={() => setActiveTab(tab.key as Tab)}
              className={`flex items-center gap-2 px-6 py-4 text-sm font-bold border-b-2 transition-all whitespace-nowrap ${
                activeTab === tab.key 
                  ? 'border-blue-600 text-blue-600 bg-blue-50/30' 
                  : 'border-transparent text-muted-foreground hover:text-foreground hover:bg-surface-hover'
              }`}
            >
              <tab.icon className={`w-4 h-4 ${activeTab === tab.key ? 'text-blue-600' : 'text-muted-foreground'}`} />
              {tab.label}
              {tab.count !== undefined && tab.count > 0 && (
                <span className={`ml-1.5 px-2 py-0.5 rounded-full text-[10px] font-bold ${
                  activeTab === tab.key 
                    ? 'bg-blue-600 text-white' 
                    : 'bg-gray-200 text-gray-600'
                }`}>
                  {tab.count}
                </span>
              )}
            </button>
          ))}
        </div>
      </div>

      {/* ========== CONTEÚDO DAS ABAS ========== */}
      <div className="flex-1 overflow-auto p-6">
        {error && (
          <div className="mb-4 p-4 bg-red-50 border border-red-200 text-red-700 rounded-xl text-sm flex items-center justify-between">
            <div className="flex items-center gap-2">
              <AlertCircle className="w-5 h-5" />
              {error}
            </div>
            <button onClick={() => setError(null)} className="p-1 hover:bg-red-100 rounded-lg">
              <X className="w-4 h-4" />
            </button>
          </div>
        )}

        {activeTab === 'dados' && (
          <div className="h-full flex flex-col gap-6">
            <SecaoCard
              titulo="Observações do Movimento"
              subtitulo="Informações adicionais que serão impressas no relatório"
              icone={<FileText className="w-5 h-5" />}
              className="flex-1 flex flex-col"
            >
              <div className="flex-1 flex flex-col min-h-[300px]">
                <textarea
                  className="flex-1 w-full p-5 bg-gray-50 border border-gray-200 rounded-2xl text-gray-900 focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 outline-none resize-none font-medium leading-relaxed transition-all placeholder:text-gray-400"
                  placeholder="Digite aqui as observações gerais deste movimento..."
                  value={formData.observacao}
                  onChange={(e) => setFormData({ ...formData, observacao: e.target.value })}
                />
                <div className="mt-4 flex items-center gap-3 text-xs text-gray-500 bg-blue-50/50 p-3 rounded-xl border border-blue-100/50">
                  <div className="p-1 bg-blue-100 rounded-md text-blue-600">
                    <AlertCircle className="w-4 h-4" />
                  </div>
                  <p>
                    <span className="font-bold text-blue-700">Dica:</span> Estas observações são importantes para o histórico do estoque e aparecerão em todos os relatórios de conferência.
                  </p>
                </div>
              </div>
            </SecaoCard>
          </div>
        )}

        {(activeTab === 'produtos' || activeTab === 'conjuntos') && (
          <div className="space-y-4">
            {/* Título da Aba */}
            <div className="bg-emerald-50 border border-emerald-200 rounded-xl p-4 flex items-center justify-between">
              <div className="flex items-center gap-3">
                {activeTab === 'produtos' ? (
                  <Package className="w-5 h-5 text-emerald-600" />
                ) : (
                  <Layers className="w-5 h-5 text-emerald-600" />
                )}
                <span className="font-bold text-emerald-800">
                  {activeTab === 'produtos' ? 'Produtos' : 'Conjuntos'}
                </span>
              </div>
            </div>

            {/* Busca e Adição de Itens */}
            {!movimentoId && (
              <div className="bg-white rounded-2xl border border-border p-6 shadow-sm">
                <div className="flex flex-col lg:flex-row gap-6 items-end">
                  <div className="flex-1">
                    <SeletorComBusca
                      label={`Buscar ${activeTab === 'produtos' ? 'Produto' : 'Conjunto'}`}
                      value={itemSelecionado?.sequenciaDoProduto || 0}
                      descricao={itemSelecionado?.descricao || ''}
                      onSelect={(id) => {
                        const item = resultadosBusca.find(r => r.sequenciaDoProduto === id);
                        if (item) {
                          setItemSelecionado(item);
                          setValorUnitarioItem(item.valorCusto.toString());
                          setBuscaItem(item.descricao);
                        }
                      }}
                      onSearch={setBuscaItem}
                      items={resultadosBusca}
                      getItemId={(item) => item.sequenciaDoProduto}
                      getItemDescricao={(item) => item.descricao}
                      getItemSecundario={(item) => `Estoque: ${item.estoqueContabil} | Custo: ${item.valorCusto.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}`}
                      placeholder={`Digite o nome do ${activeTab === 'produtos' ? 'produto' : 'conjunto'}...`}
                      loading={loadingBusca}
                    />
                  </div>

                  {itemSelecionado && (
                    <>
                      <div className="w-32">
                        <InputModerno
                          label="Quantidade"
                          type="number"
                          value={quantidadeItem}
                          onChange={(val) => setQuantidadeItem(val)}
                          disabled={modoAjuste}
                          icone={<Hash className="w-4 h-4" />}
                        />
                      </div>
                      <div className="w-40">
                        <InputModerno
                          label="Vlr. Unitário"
                          type="number"
                          value={valorUnitarioItem}
                          onChange={(val) => setValorUnitarioItem(val)}
                          icone={<DollarSign className="w-4 h-4" />}
                        />
                      </div>
                      <div className="flex gap-2">
                        <button
                          onClick={adicionarItem}
                          className="h-[52px] px-6 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all font-bold flex items-center gap-2 shadow-lg shadow-blue-600/20 active:scale-95"
                          title="Adicionar"
                        >
                          <Plus className="w-5 h-5" />
                          Adicionar
                        </button>
                        <button
                          onClick={abrirProducaoInteligente}
                          className="h-[52px] px-6 bg-purple-600 text-white rounded-xl hover:bg-purple-700 transition-all font-bold flex items-center gap-2 shadow-lg shadow-purple-600/20 active:scale-95"
                          title="Produção Inteligente (Cascata)"
                        >
                          <Zap className="w-5 h-5" />
                          Produzir
                        </button>
                      </div>
                    </>
                  )}
                </div>

                {itemSelecionado && (
                  <div className="mt-6 pt-6 border-t border-gray-100 flex items-center gap-4">
                    <button
                      onClick={() => setModoAjuste(!modoAjuste)}
                      className={`flex items-center gap-2 px-4 py-2.5 rounded-xl border font-bold text-sm transition-all ${
                        modoAjuste 
                          ? 'bg-amber-50 border-amber-200 text-amber-700 shadow-sm' 
                          : 'bg-gray-50 border-gray-200 text-gray-500 hover:border-amber-300 hover:text-amber-700'
                      }`}
                    >
                      <Calculator className="w-4 h-4" />
                      Modo Ajuste (Física vs Contábil)
                    </button>
                    {modoAjuste && (
                      <div className="flex items-center gap-4 animate-in fade-in slide-in-from-left-4 duration-300">
                        <div className="w-40">
                          <InputModerno
                            label="Qtd Física"
                            type="number"
                            value={quantidadeFisica}
                            onChange={(val) => setQuantidadeFisica(val)}
                            icone={<Hash className="w-4 h-4" />}
                          />
                        </div>
                        <div className="text-sm text-gray-500 font-medium">
                          Estoque Contábil: <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-0.5 rounded">{itemSelecionado.estoqueContabil}</span>
                        </div>
                      </div>
                    )}
                  </div>
                )}
              </div>
            )}

            {/* Grid de Itens */}
            <div className="bg-white rounded-2xl border border-border shadow-sm overflow-hidden">
              <table className="w-full">
                <thead className="bg-emerald-50 border-b border-emerald-100">
                  <tr>
                    <th className="px-4 py-3 text-left text-xs font-bold text-emerald-700 uppercase tracking-wider">{activeTab === 'produtos' ? 'Produto' : 'Conjunto'}</th>
                    <th className="px-4 py-3 text-center text-xs font-bold text-emerald-700 uppercase tracking-wider w-20">ID</th>
                    <th className="px-4 py-3 text-center text-xs font-bold text-emerald-700 uppercase tracking-wider w-24">Un</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-emerald-700 uppercase tracking-wider w-28">Qtde</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-emerald-700 uppercase tracking-wider w-28">% PIS</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-emerald-700 uppercase tracking-wider w-28">% Cofins</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-emerald-700 uppercase tracking-wider w-32">Unitário</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-emerald-700 uppercase tracking-wider w-32">Total</th>
                    {!movimentoId && <th className="px-4 py-3 text-center text-xs font-bold text-emerald-700 uppercase tracking-wider w-20">Ações</th>}
                  </tr>
                </thead>
                <tbody className="divide-y divide-border">
                  {activeTab === 'produtos' ? (
                    formData.produtos.length === 0 ? (
                      <tr>
                        <td colSpan={9} className="px-4 py-12 text-center text-muted-foreground">
                          <Package className="w-12 h-12 mx-auto mb-3 text-muted-foreground/30" />
                          <p className="font-medium">Nenhum produto adicionado</p>
                          <p className="text-sm">Use a busca acima para adicionar produtos ao movimento</p>
                        </td>
                      </tr>
                    ) : (
                      formData.produtos.map((p, idx) => (
                        <tr key={idx} className="hover:bg-surface-hover transition-colors">
                          <td className="px-4 py-3 font-medium text-foreground">{p.descricaoProduto}</td>
                          <td className="px-4 py-3 text-center">
                            <span className="font-mono text-xs bg-blue-50 text-blue-700 px-2 py-1 rounded-lg">{p.sequenciaDoProduto}</span>
                          </td>
                          <td className="px-4 py-3 text-center text-sm text-muted-foreground">UN</td>
                          <td className="px-4 py-3 text-right font-mono font-medium">{p.quantidade.toFixed(4)}</td>
                          <td className="px-4 py-3 text-right font-mono text-sm text-muted-foreground">{((p.valorDoPis || 0) / p.valorTotal * 100).toFixed(2)}%</td>
                          <td className="px-4 py-3 text-right font-mono text-sm text-muted-foreground">{((p.valorDoCofins || 0) / p.valorTotal * 100).toFixed(2)}%</td>
                          <td className="px-4 py-3 text-right font-mono">{p.valorUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                          <td className="px-4 py-3 text-right font-mono font-bold text-foreground">{p.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                          {!movimentoId && (
                            <td className="px-4 py-3 text-center">
                              <button onClick={() => removerProduto(idx)} className="p-2 text-red-600 hover:bg-red-50 rounded-xl transition-all">
                                <Trash2 className="w-4 h-4" />
                              </button>
                            </td>
                          )}
                        </tr>
                      ))
                    )
                  ) : (
                    formData.conjuntos.length === 0 ? (
                      <tr>
                        <td colSpan={9} className="px-4 py-12 text-center text-muted-foreground">
                          <Layers className="w-12 h-12 mx-auto mb-3 text-muted-foreground/30" />
                          <p className="font-medium">Nenhum conjunto adicionado</p>
                          <p className="text-sm">Use a busca acima para adicionar conjuntos ao movimento</p>
                        </td>
                      </tr>
                    ) : (
                      formData.conjuntos.map((c, idx) => (
                        <tr key={idx} className="hover:bg-surface-hover transition-colors">
                          <td className="px-4 py-3 font-medium text-foreground">{c.descricaoConjunto}</td>
                          <td className="px-4 py-3 text-center">
                            <span className="font-mono text-xs bg-purple-50 text-purple-700 px-2 py-1 rounded-lg">{c.sequenciaDoConjunto}</span>
                          </td>
                          <td className="px-4 py-3 text-center text-sm text-muted-foreground">CJ</td>
                          <td className="px-4 py-3 text-right font-mono font-medium">{c.quantidade.toFixed(4)}</td>
                          <td className="px-4 py-3 text-right font-mono text-sm text-muted-foreground">0,00%</td>
                          <td className="px-4 py-3 text-right font-mono text-sm text-muted-foreground">0,00%</td>
                          <td className="px-4 py-3 text-right font-mono">{c.valorUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                          <td className="px-4 py-3 text-right font-mono font-bold text-foreground">{c.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                          {!movimentoId && (
                            <td className="px-4 py-3 text-center">
                              <button onClick={() => removerConjunto(idx)} className="p-2 text-red-600 hover:bg-red-50 rounded-xl transition-all">
                                <Trash2 className="w-4 h-4" />
                              </button>
                            </td>
                          )}
                        </tr>
                      ))
                    )
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}
        {activeTab === 'despesas' && (
          <div className="space-y-4">
            {/* Título da Aba */}
            <div className="bg-slate-50 border border-slate-200 rounded-xl p-4 flex items-center justify-between">
              <div className="flex items-center gap-3">
                <Receipt className="w-5 h-5 text-slate-600" />
                <span className="font-bold text-slate-800">Despesas</span>
              </div>
            </div>

            {/* Busca e Adição de Despesas */}
            {!movimentoId && (
              <div className="bg-white rounded-2xl border border-border p-6 shadow-sm">
                <div className="flex flex-col lg:flex-row gap-6 items-end">
                  <div className="flex-1">
                    <SeletorComBusca
                      label="Buscar Despesa"
                      value={despesaSelecionada?.sequenciaDaDespesa || 0}
                      descricao={despesaSelecionada?.descricaoDespesa || ''}
                      onSelect={(id) => {
                        const d = despesasEncontradas.find(x => x.sequenciaDaDespesa === id);
                        if (d) {
                          setDespesaSelecionada(d);
                          setValorDespesa(d.valorUnitario.toString());
                          setBuscaDespesa(d.descricaoDespesa);
                        }
                      }}
                      onSearch={setBuscaDespesa}
                      items={despesasEncontradas}
                      getItemId={(item) => item.sequenciaDaDespesa}
                      getItemDescricao={(item) => item.descricaoDespesa}
                      getItemSecundario={(item) => `Custo: ${item.valorUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}`}
                      placeholder="Digite o nome da despesa..."
                      loading={loadingDespesa}
                    />
                  </div>

                  <div className="w-32">
                    <InputModerno
                      label="Quantidade"
                      type="number"
                      value={quantidadeDespesa}
                      onChange={(val) => setQuantidadeDespesa(val)}
                      icone={<Hash className="w-4 h-4" />}
                    />
                  </div>

                  <div className="w-40">
                    <InputModerno
                      label="Vlr. Unitário"
                      type="number"
                      value={valorDespesa}
                      onChange={(val) => setValorDespesa(val)}
                      icone={<DollarSign className="w-4 h-4" />}
                    />
                  </div>

                  <button
                    onClick={adicionarDespesa}
                    disabled={!despesaSelecionada}
                    className="h-[52px] px-8 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 disabled:opacity-50 transition-all font-bold flex items-center gap-2 shadow-lg shadow-indigo-600/20 active:scale-95"
                  >
                    <Plus className="w-5 h-5" />
                    Adicionar
                  </button>
                </div>
              </div>
            )}

            {/* Grid de Despesas */}
            <div className="bg-white rounded-2xl border border-border shadow-sm overflow-hidden">
              <table className="w-full">
                <thead className="bg-slate-50 border-b border-slate-100">
                  <tr>
                    <th className="px-4 py-3 text-left text-xs font-bold text-slate-700 uppercase tracking-wider">Despesa</th>
                    <th className="px-4 py-3 text-center text-xs font-bold text-slate-700 uppercase tracking-wider w-20">ID</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-slate-700 uppercase tracking-wider w-28">Qtde</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-slate-700 uppercase tracking-wider w-32">Unitário</th>
                    <th className="px-4 py-3 text-right text-xs font-bold text-slate-700 uppercase tracking-wider w-32">Total</th>
                    {!movimentoId && <th className="px-4 py-3 text-center text-xs font-bold text-slate-700 uppercase tracking-wider w-20">Ações</th>}
                  </tr>
                </thead>
                <tbody className="divide-y divide-border">
                  {formData.despesas.length === 0 ? (
                    <tr>
                      <td colSpan={6} className="px-4 py-12 text-center text-muted-foreground">
                        <Receipt className="w-12 h-12 mx-auto mb-3 text-muted-foreground/30" />
                        <p className="font-medium">Nenhuma despesa adicionada</p>
                        <p className="text-sm">Use a busca acima para adicionar despesas ao movimento</p>
                      </td>
                    </tr>
                  ) : (
                    formData.despesas.map((item, index) => (
                      <tr key={index} className="hover:bg-surface-hover transition-colors">
                        <td className="px-4 py-3 font-medium text-foreground">{item.descricaoDespesa}</td>
                        <td className="px-4 py-3 text-center">
                          <span className="font-mono text-xs bg-slate-50 text-slate-700 px-2 py-1 rounded-lg">{item.sequenciaDaDespesa}</span>
                        </td>
                        <td className="px-4 py-3 text-right font-mono font-medium">{item.quantidade.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                        <td className="px-4 py-3 text-right font-mono">{item.valorUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                        <td className="px-4 py-3 text-right font-mono font-bold text-foreground">{item.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                        {!movimentoId && (
                          <td className="px-4 py-3 text-center">
                            <button onClick={() => removerDespesa(index)} className="p-2 text-red-600 hover:bg-red-50 rounded-xl transition-all">
                              <Trash2 className="w-4 h-4" />
                            </button>
                          </td>
                        )}
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {activeTab === 'financeiro' && (
          <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
            {/* Resumo Financeiro Consolidado */}
            <SecaoCard
              titulo="Resumo Financeiro"
              subtitulo="Detalhamento dos valores do movimento"
              icone={<DollarSign className="w-5 h-5" />}
            >
              <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-5 gap-6">
                <div className="p-4 bg-surface rounded-2xl border border-border">
                  <span className="text-[10px] font-bold text-muted-foreground uppercase tracking-wider">Produtos/Conjuntos</span>
                  <div className="text-xl font-mono font-bold text-foreground">
                    {formData.valorTotalDosProdutos.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </div>
                </div>
                <div className="p-4 bg-surface rounded-2xl border border-border">
                  <span className="text-[10px] font-bold text-muted-foreground uppercase tracking-wider">Despesas</span>
                  <div className="text-xl font-mono font-bold text-foreground">
                    {formData.despesas.reduce((acc, d) => acc + d.valorTotal, 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </div>
                </div>
                <div className="p-4 bg-blue-50 rounded-2xl border border-blue-100">
                  <span className="text-[10px] font-bold text-blue-600 uppercase tracking-wider">Frete (+)</span>
                  <div className="text-xl font-mono font-bold text-blue-700">
                    {formData.valorDoFrete.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </div>
                </div>
                <div className="p-4 bg-red-50 rounded-2xl border border-red-100">
                  <span className="text-[10px] font-bold text-red-600 uppercase tracking-wider">Desconto (-)</span>
                  <div className="text-xl font-mono font-bold text-red-700">
                    {formData.valorDoDesconto.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </div>
                </div>
                <div className="p-4 bg-blue-600 rounded-2xl shadow-lg shadow-blue-600/20 text-white">
                  <span className="text-[10px] font-bold text-blue-100 uppercase tracking-wider">Total Final</span>
                  <div className="text-2xl font-mono font-black">
                    {formData.valorTotalDoMovimento.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </div>
                </div>
              </div>
            </SecaoCard>

            {/* Classificação Financeira */}
            <SecaoCard
              titulo="Classificação Financeira"
              subtitulo="Defina a categoria e conta para este movimento"
              icone={<CreditCard className="w-5 h-5" />}
            >
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                {/* Grupo */}
                <SeletorComBusca
                  label="Grupo Financeiro"
                  value={0}
                  descricao=""
                  onSelect={() => {}}
                  items={[]}
                  getItemId={(item: any) => item.id}
                  getItemDescricao={(item: any) => item.descricao}
                  placeholder="Selecione o Grupo..."
                />
                {/* Subgrupo */}
                <SeletorComBusca
                  label="Subgrupo"
                  value={0}
                  descricao=""
                  onSelect={() => {}}
                  items={[]}
                  getItemId={(item: any) => item.id}
                  getItemDescricao={(item: any) => item.descricao}
                  placeholder="Selecione o Subgrupo..."
                />
                {/* Conta */}
                <SeletorComBusca
                  label="Conta Financeira"
                  value={0}
                  descricao=""
                  onSelect={() => {}}
                  items={[]}
                  getItemId={(item: any) => item.id}
                  getItemDescricao={(item: any) => item.descricao}
                  placeholder="Selecione a Conta..."
                />
                {/* Forma de Pagamento */}
                <SelectModerno
                  label="Forma de Pagamento"
                  value="vista"
                  onChange={() => {}}
                  opcoes={[
                    { value: 'vista', label: 'A Vista' },
                    { value: 'prazo', label: 'A Prazo' },
                    { value: 'boleto', label: 'Boleto' },
                    { value: 'cartao', label: 'Cartão' },
                    { value: 'pix', label: 'PIX' },
                  ]}
                  icone={<CreditCard className="w-4 h-4" />}
                />
                {/* Cód Contábil */}
                <div className="lg:col-span-2">
                  <SeletorComBusca
                    label="Cód. Contábil (Débito)"
                    value={0}
                    descricao=""
                    onSelect={() => {}}
                    items={[]}
                    getItemId={(item: any) => item.id}
                    getItemDescricao={(item: any) => item.descricao}
                    placeholder="Selecione o código contábil..."
                  />
                </div>
                {/* Frete */}
                <InputModerno
                  label="Valor do Frete"
                  value={formData.valorDoFrete.toString()}
                  onChange={(val) => setFormData({ ...formData, valorDoFrete: parseFloat(val) || 0 })}
                  type="number"
                  placeholder="0,00"
                  icone={<DollarSign className="w-4 h-4" />}
                />
                {/* Desconto */}
                <InputModerno
                  label="Desconto Especial"
                  value={formData.valorDoDesconto.toString()}
                  onChange={(val) => setFormData({ ...formData, valorDoDesconto: parseFloat(val) || 0 })}
                  type="number"
                  placeholder="0,00"
                  icone={<DollarSign className="w-4 h-4" />}
                />
              </div>
            </SecaoCard>

            {/* Parcelamento */}
            <div className="bg-white rounded-2xl border border-border shadow-sm overflow-hidden">
              <div className="bg-surface border-b border-border px-6 py-4 flex items-center justify-between">
                <div className="flex items-center gap-3">
                  <div className="p-2 bg-blue-50 rounded-lg text-blue-600">
                    <DollarSign className="w-5 h-5" />
                  </div>
                  <div>
                    <h3 className="font-semibold text-gray-900">Parcelamento Financeiro</h3>
                    <p className="text-xs text-gray-500">Geração e controle de parcelas</p>
                  </div>
                </div>
                
                {/* Status do Parcelamento */}
                <div className="flex items-center gap-3">
                  {(() => {
                    const totalParcelas = formData.parcelas.reduce((sum, p) => sum + p.valorDaParcela, 0);
                    const totalMovimento = formData.valorTotalDoMovimento;
                    const diferenca = Math.abs(totalParcelas - totalMovimento);
                    
                    if (formData.parcelas.length === 0) {
                      return (
                        <span className="px-3 py-1 bg-amber-100 text-amber-700 rounded-full text-[10px] font-bold uppercase tracking-wider border border-amber-200">
                          Sem Parcelas
                        </span>
                      );
                    }
                    
                    if (diferenca > 0.01) {
                      return (
                        <span className="px-3 py-1 bg-red-100 text-red-700 rounded-full text-[10px] font-bold uppercase tracking-wider border border-red-200">
                          Diferença: {diferenca.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                        </span>
                      );
                    }
                    
                    return (
                      <span className="px-3 py-1 bg-emerald-100 text-emerald-700 rounded-full text-[10px] font-bold uppercase tracking-wider border border-emerald-200">
                        Parcelamento OK
                      </span>
                    );
                  })()}
                </div>
              </div>

              <div className="p-6 space-y-6">
                {/* Gerador de Parcelas */}
                {!movimentoId && (
                  <div className="flex flex-wrap gap-4 items-end bg-blue-50/30 p-6 rounded-2xl border border-blue-100 border-dashed">
                    <div className="w-32">
                      <InputModerno
                        label="Parcelas"
                        type="number"
                        value={numParcelas}
                        onChange={(val) => setNumParcelas(parseInt(val) || 1)}
                        icone={<Hash className="w-4 h-4" />}
                      />
                    </div>
                    <div className="w-40">
                      <InputModerno
                        label="Intervalo (Dias)"
                        type="number"
                        value={intervaloDias}
                        onChange={(val) => setIntervaloDias(parseInt(val) || 30)}
                        icone={<Calendar className="w-4 h-4" />}
                      />
                    </div>
                    <button
                      onClick={gerarParcelas}
                      className="h-[52px] px-8 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all font-bold flex items-center gap-2 shadow-lg shadow-blue-600/20 active:scale-95"
                    >
                      <CreditCard className="w-5 h-5" />
                      Gerar Parcelas
                    </button>
                    <button
                      onClick={adicionarParcelaManual}
                      className="h-[52px] px-6 bg-white border border-gray-200 text-gray-700 rounded-xl hover:bg-gray-50 transition-all font-bold flex items-center gap-2 active:scale-95"
                    >
                      <Plus className="w-5 h-5" />
                      Adicionar Manual
                    </button>
                  </div>
                )}

                {/* Grid de Parcelas */}
                <div className="border border-gray-100 rounded-2xl overflow-hidden shadow-sm">
                  <table className="w-full">
                    <thead className="bg-gray-50 border-b border-gray-100">
                      <tr>
                        <th className="px-6 py-4 text-center text-[10px] font-bold text-gray-500 uppercase tracking-wider w-24">Nº</th>
                        <th className="px-6 py-4 text-center text-[10px] font-bold text-gray-500 uppercase tracking-wider w-48">Vencimento</th>
                        <th className="px-6 py-4 text-right text-[10px] font-bold text-gray-500 uppercase tracking-wider w-48">Valor</th>
                        <th className="px-6 py-4 text-left text-[10px] font-bold text-gray-500 uppercase tracking-wider">Descrição</th>
                        {!movimentoId && <th className="px-6 py-4 text-center text-[10px] font-bold text-gray-500 uppercase tracking-wider w-24">Ações</th>}
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-100">
                      {formData.parcelas.length === 0 ? (
                        <tr>
                          <td colSpan={5} className="px-6 py-16 text-center text-gray-400">
                            <div className="flex flex-col items-center gap-3 opacity-40">
                              <CreditCard className="w-12 h-12" />
                              <p className="text-sm font-bold uppercase tracking-widest">Nenhuma parcela gerada</p>
                              <p className="text-xs font-normal normal-case">Use o gerador acima para criar o parcelamento</p>
                            </div>
                          </td>
                        </tr>
                      ) : (
                        formData.parcelas.map((parcela, index) => (
                          <tr key={index} className="hover:bg-gray-50/50 transition-colors group">
                            <td className="px-6 py-4 text-center">
                              <span className="font-mono font-bold text-blue-600 bg-blue-50 px-3 py-1 rounded-lg text-sm">
                                {parcela.numeroDaParcela.toString().padStart(2, '0')}
                              </span>
                            </td>
                            <td className="px-6 py-4">
                              <div className="relative flex items-center border border-gray-200 rounded-xl bg-white px-3 py-2 focus-within:border-blue-500 transition-all">
                                <Calendar className="w-4 h-4 text-gray-400 mr-2" />
                                <input
                                  type="date"
                                  value={parcela.dataDeVencimento.split('T')[0]}
                                  onChange={(e) => atualizarParcela(index, 'dataDeVencimento', e.target.value)}
                                  className="w-full bg-transparent font-mono text-sm outline-none"
                                  disabled={!!movimentoId}
                                />
                              </div>
                            </td>
                            <td className="px-6 py-4">
                              <div className="relative flex items-center border border-gray-200 rounded-xl bg-white px-3 py-2 focus-within:border-blue-500 transition-all">
                                <span className="text-xs font-bold text-gray-400 mr-2">R$</span>
                                <input
                                  type="number"
                                  value={parcela.valorDaParcela}
                                  onChange={(e) => atualizarParcela(index, 'valorDaParcela', parseFloat(e.target.value) || 0)}
                                  className="w-full bg-transparent text-right font-mono font-bold text-sm outline-none"
                                  step="0.01"
                                  disabled={!!movimentoId}
                                />
                              </div>
                            </td>
                            <td className="px-6 py-4">
                              <div className="flex items-center gap-2 text-sm font-medium text-gray-600">
                                <div className="w-2 h-2 rounded-full bg-blue-400"></div>
                                Parcela {parcela.numeroDaParcela} de {formData.parcelas.length}
                              </div>
                            </td>
                            {!movimentoId && (
                              <td className="px-6 py-4 text-center">
                                <button 
                                  onClick={() => removerParcela(index)}
                                  className="p-2 text-gray-400 hover:text-red-600 hover:bg-red-50 rounded-xl transition-all opacity-0 group-hover:opacity-100"
                                >
                                  <Trash2 className="w-5 h-5" />
                                </button>
                              </td>
                            )}
                          </tr>
                        ))
                      )}
                    </tbody>
                    {formData.parcelas.length > 0 && (
                      <tfoot className="bg-surface/50 border-t border-border">
                        <tr>
                          <td className="px-4 py-4 text-center font-bold text-sm text-muted-foreground">{formData.parcelas.length}x</td>
                          <td className="px-4 py-4 text-right font-bold text-sm text-muted-foreground uppercase tracking-wider" colSpan={1}>Total Parcelas:</td>
                          <td className="px-4 py-4 text-right font-mono font-bold text-blue-600 text-lg">
                            {formData.parcelas.reduce((sum, p) => sum + p.valorDaParcela, 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                          </td>
                          <td colSpan={2}></td>
                        </tr>
                      </tfoot>
                    )}
                  </table>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>

      {/* ========== FOOTER FIXO ========== */}
      <div className="bg-white border-t border-border px-6 py-4 mt-auto">
        <div className="flex items-center justify-between">
          {/* Status de Salvamento ou Info Auxiliar */}
          <div className="flex items-center gap-4 text-muted-foreground">
            {saving && (
              <div className="flex items-center gap-2 px-3 py-1.5 bg-surface rounded-lg border border-border">
                <div className="w-2 h-2 rounded-full bg-amber-500 animate-pulse"></div>
                <span className="text-xs font-bold uppercase tracking-wider">
                  Salvando...
                </span>
              </div>
            )}
          </div>

          {/* Botões de Ação */}
          <div className="flex items-center gap-3">
            <button
              onClick={onClose}
              className="px-6 py-3 text-sm font-bold text-muted-foreground bg-surface border border-border rounded-xl hover:bg-surface-hover transition-all"
            >
              Cancelar
            </button>
            {movimentoId && (
              <button
                onClick={() => window.print()}
                className="px-6 py-3 text-sm font-bold text-white bg-gray-600 rounded-xl hover:bg-gray-700 transition-all flex items-center gap-2 shadow-lg shadow-gray-600/20"
              >
                <Printer className="w-4 h-4" />
                Imprimir
              </button>
            )}
            {!movimentoId && (
              <button
                onClick={handleSave}
                disabled={saving}
                className="px-8 py-3 text-sm font-bold text-white bg-blue-600 rounded-xl hover:bg-blue-700 disabled:opacity-50 transition-all flex items-center gap-2 shadow-lg shadow-blue-600/20"
              >
                {saving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                Salvar Movimento
              </button>
            )}
          </div>
        </div>
      </div>

      {/* Modal de Produção Inteligente */}
      {showProducaoModal && producaoItem && (
        <ProducaoInteligenteModal
          itemId={producaoItem?.id || 0}
          itemDescricao={producaoItem?.descricao || ''}
          ehConjunto={producaoItem?.ehConjunto || false}
          sequenciaDoGeral={formData.sequenciaDoGeral}
          onClose={() => {
            setShowProducaoModal(false);
            setProducaoItem(null);
          }}
          onSuccess={() => {
            // Limpar seleção após produção bem-sucedida
            setItemSelecionado(null);
            setBuscaItem('');
            setQuantidadeItem('');
            setValorUnitarioItem('');
          }}
        />
      )}
    </div>
  );
}
