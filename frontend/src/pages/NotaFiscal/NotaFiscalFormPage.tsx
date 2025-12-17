import { useEffect, useState, useCallback, useRef } from 'react';
import { createPortal } from 'react-dom';
import { useNavigate, useParams } from 'react-router-dom';
import {
  ArrowLeft,
  Save,
  FileText,
  Truck,
  Package,
  DollarSign,
  FileCheck,
  Loader2,
  AlertCircle,
  Search,
  X,
  Boxes,
  Cog,
  Plus,
  Trash2,
  User,
  Calendar,
  Settings,
  ClipboardList,
  Send,
  Check
} from 'lucide-react';

import { notaFiscalService } from '../../services/NotaFiscal/notaFiscalService';
import { emitenteService } from '../../services/Emitentes/emitenteService';
import type {
  NotaFiscalDto,
  NotaFiscalCreateUpdateDto,
  ClienteComboDto,
  TransportadoraComboDto,
  NaturezaOperacaoComboDto,
  TipoCobrancaComboDto,
  VendedorComboDto,
  ProdutoDaNotaFiscalDto,
  ProdutoDaNotaFiscalCreateDto,
  ProdutoComboDto,
  ConjuntoDaNotaFiscalDto,
  ConjuntoDaNotaFiscalCreateDto,
  ConjuntoComboDto,
  PecaDaNotaFiscalDto,
  PecaDaNotaFiscalCreateDto,
  PecaComboDto,
  ParcelaNotaFiscalDto,
  ParcelaNotaFiscalCreateDto,
  PropriedadeComboDto,
} from '../../types';
import type { EmitenteDto } from '../../types';
import { MODALIDADES_FRETE } from '../../types';

// Tabs da nota fiscal - 7 abas como no VB6 (sem Serviços)
type TabId = 'dados' | 'transportadora' | 'produtos' | 'conjuntos' | 'pecas' | 'financeiro' | 'nfe';

interface Tab {
  id: TabId;
  label: string;
  icon: React.ReactNode;
}

const TABS: Tab[] = [
  { id: 'dados', label: '1 - Dados Principais', icon: <FileText className="h-4 w-4" /> },
  { id: 'transportadora', label: '2 - Transportadora', icon: <Truck className="h-4 w-4" /> },
  { id: 'produtos', label: '3 - Produtos', icon: <Package className="h-4 w-4" /> },
  { id: 'conjuntos', label: '4 - Conjuntos', icon: <Boxes className="h-4 w-4" /> },
  { id: 'pecas', label: '5 - Peças', icon: <Cog className="h-4 w-4" /> },
  { id: 'financeiro', label: '6 - Financeiro', icon: <DollarSign className="h-4 w-4" /> },
  { id: 'nfe', label: '7 - NFe', icon: <FileCheck className="h-4 w-4" /> },
];

export default function NotaFiscalFormPage() {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEditing = Boolean(id && id !== 'nova');

  // Estado do formulário
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<TabId>('dados');

  // Dados da nota fiscal
  const [notaOriginal, setNotaOriginal] = useState<NotaFiscalDto | null>(null);
  const [formData, setFormData] = useState<NotaFiscalCreateUpdateDto>({
    dataDeEmissao: new Date().toISOString().split('T')[0],
    sequenciaDoGeral: 0,
    sequenciaDaPropriedade: 0,
    sequenciaDaNatureza: 0,
    tipoDeNota: 0,
  });

  // Emitente (busca automaticamente)
  const [emitente, setEmitente] = useState<EmitenteDto | null>(null);

  // Combos
  const [propriedades, setPropriedades] = useState<PropriedadeComboDto[]>([]);
  const [naturezas, setNaturezas] = useState<NaturezaOperacaoComboDto[]>([]);
  const [tiposCobranca, setTiposCobranca] = useState<TipoCobrancaComboDto[]>([]);

  // Autocompletes
  const [clienteSelecionado, setClienteSelecionado] = useState<ClienteComboDto | null>(null);
  const [buscaCliente, setBuscaCliente] = useState('');
  const [clientesEncontrados, setClientesEncontrados] = useState<ClienteComboDto[]>([]);
  const [buscandoClientes, setBuscandoClientes] = useState(false);
  const [showClienteDropdown, setShowClienteDropdown] = useState(false);

  const [transportadoraSelecionada, setTransportadoraSelecionada] = useState<TransportadoraComboDto | null>(null);
  const [buscaTransportadora, setBuscaTransportadora] = useState('');
  const [transportadorasEncontradas, setTransportadorasEncontradas] = useState<TransportadoraComboDto[]>([]);
  const [showTransportadoraDropdown, setShowTransportadoraDropdown] = useState(false);

  const [vendedorSelecionado, setVendedorSelecionado] = useState<VendedorComboDto | null>(null);
  const [buscaVendedor, setBuscaVendedor] = useState('');
  const [vendedoresEncontrados, setVendedoresEncontrados] = useState<VendedorComboDto[]>([]);
  const [showVendedorDropdown, setShowVendedorDropdown] = useState(false);

  // Estados para grids editáveis
  // Produtos
  const [produtosLista, setProdutosLista] = useState<ProdutoDaNotaFiscalDto[]>([]);
  const [novoProduto, setNovoProduto] = useState<Partial<ProdutoDaNotaFiscalCreateDto> | null>(null);
  const [produtosCombo, setProdutosCombo] = useState<ProdutoComboDto[]>([]);
  const [buscaProduto, setBuscaProduto] = useState('');
  const [showProdutoDropdown, setShowProdutoDropdown] = useState(false);
  const [salvandoProduto, setSalvandoProduto] = useState(false);
  const [dropdownPosition, setDropdownPosition] = useState<{ top: number; left: number; width: number } | null>(null);
  const produtoInputRef = useRef<HTMLInputElement>(null);

  // Conjuntos
  const [conjuntosLista, setConjuntosLista] = useState<ConjuntoDaNotaFiscalDto[]>([]);
  const [novoConjunto, setNovoConjunto] = useState<Partial<ConjuntoDaNotaFiscalCreateDto> | null>(null);
  const [conjuntosCombo, setConjuntosCombo] = useState<ConjuntoComboDto[]>([]);
  const [buscaConjunto, setBuscaConjunto] = useState('');
  const [showConjuntoDropdown, setShowConjuntoDropdown] = useState(false);
  const [salvandoConjunto, setSalvandoConjunto] = useState(false);

  // Peças
  const [pecasLista, setPecasLista] = useState<PecaDaNotaFiscalDto[]>([]);
  const [novaPeca, setNovaPeca] = useState<Partial<PecaDaNotaFiscalCreateDto> | null>(null);
  const [pecasCombo, setPecasCombo] = useState<PecaComboDto[]>([]);
  const [buscaPeca, setBuscaPeca] = useState('');
  const [showPecaDropdown, setShowPecaDropdown] = useState(false);
  const [salvandoPeca, setSalvandoPeca] = useState(false);

  // Parcelas
  const [parcelasLista, setParcelasLista] = useState<ParcelaNotaFiscalDto[]>([]);
  const [novaParcela, setNovaParcela] = useState<Partial<ParcelaNotaFiscalCreateDto> | null>(null);
  const [salvandoParcela, setSalvandoParcela] = useState(false);

  // Regra de exclusividade: Produto vs Peça/Conjunto
  // Se tem Produto, não pode ter Peça nem Conjunto
  // Se tem Peça ou Conjunto, não pode ter Produto
  const temProdutos = produtosLista.length > 0;
  const temPecasOuConjuntos = pecasLista.length > 0 || conjuntosLista.length > 0;

  // Nota fiscal já autorizada/emitida - bloqueia edição
  const isReadOnly = notaOriginal?.autorizado === true;

  // Função para verificar se uma aba está desabilitada pela regra de exclusividade
  const isTabDisabled = (tabId: TabId): boolean => {
    if (tabId === 'produtos') {
      return temPecasOuConjuntos; // Desabilita Produtos se tem Peça ou Conjunto
    }
    if (tabId === 'conjuntos' || tabId === 'pecas') {
      return temProdutos; // Desabilita Peças/Conjuntos se tem Produto
    }
    return false;
  };

  // Mensagem de explicação para abas desabilitadas
  const getTabDisabledMessage = (tabId: TabId): string | null => {
    if (tabId === 'produtos' && temPecasOuConjuntos) {
      return 'Não é possível adicionar Produtos quando já existem Peças ou Conjuntos';
    }
    if ((tabId === 'conjuntos' || tabId === 'pecas') && temProdutos) {
      return 'Não é possível adicionar Peças ou Conjuntos quando já existem Produtos';
    }
    return null;
  };

  // Carregar combos (sem propriedades - elas dependem do cliente)
  useEffect(() => {
    const loadCombos = async () => {
      try {
        const [emitenteData, natData, cobData] = await Promise.all([
          emitenteService.obterAtual(),
          notaFiscalService.listarNaturezas(),
          notaFiscalService.listarTiposCobranca(),
        ]);

        // Define o emitente (usado em validações)
        setEmitente(emitenteData);
        setNaturezas(natData);
        setTiposCobranca(cobData);
      } catch (err) {
        console.error('Erro ao carregar combos:', err);
      }
    };
    loadCombos();
  }, []);

  // Carregar propriedades quando cliente mudar
  useEffect(() => {
    const loadPropriedades = async () => {
      if (clienteSelecionado?.sequenciaDoGeral) {
        try {
          const props = await notaFiscalService.listarPropriedadesPorCliente(clienteSelecionado.sequenciaDoGeral);
          setPropriedades(props);

          // Se só tem uma propriedade, seleciona automaticamente
          if (props.length === 1 && !isEditing) {
            setFormData(prev => ({
              ...prev,
              sequenciaDaPropriedade: props[0].sequenciaDaPropriedade,
            }));
          } else if (props.length === 0) {
            // Limpa a propriedade se o cliente não tiver nenhuma
            setFormData(prev => ({
              ...prev,
              sequenciaDaPropriedade: 0,
            }));
          }
        } catch (err) {
          console.error('Erro ao carregar propriedades do cliente:', err);
          setPropriedades([]);
        }
      } else {
        setPropriedades([]);
        setFormData(prev => ({
          ...prev,
          sequenciaDaPropriedade: 0,
        }));
      }
    };
    loadPropriedades();
  }, [clienteSelecionado?.sequenciaDoGeral, isEditing]);

  // Carregar nota fiscal para edição
  useEffect(() => {
    if (isEditing && id) {
      const loadNota = async () => {
        try {
          setLoading(true);
          const nota = await notaFiscalService.obterPorId(Number(id));
          setNotaOriginal(nota);

          // Preencher formulário
          setFormData({
            numeroDaNotaFiscal: nota.numeroDaNotaFiscal,
            dataDeEmissao: nota.dataDeEmissao?.split('T')[0] || '',
            dataDeSaida: nota.dataDeSaida?.split('T')[0] || null,
            horaDaSaida: nota.horaDaSaida ? new Date(nota.horaDaSaida).toTimeString().slice(0, 5) : null,
            sequenciaDoGeral: nota.sequenciaDoGeral,
            sequenciaDaPropriedade: nota.sequenciaDaPropriedade,
            sequenciaDaNatureza: nota.sequenciaDaNatureza,
            sequenciaDaClassificacao: nota.sequenciaDaClassificacao,
            sequenciaDaCobranca: nota.sequenciaDaCobranca,
            tipoDeNota: nota.tipoDeNota,
            transportadoraAvulsa: nota.transportadoraAvulsa,
            sequenciaDaTransportadora: nota.sequenciaDaTransportadora,
            nomeDaTransportadoraAvulsa: nota.nomeDaTransportadoraAvulsa,
            documentoDaTransportadora: nota.documentoDaTransportadora,
            ieDaTransportadora: nota.ieDaTransportadora,
            enderecoDaTransportadora: nota.enderecoDaTransportadora,
            placaDoVeiculo: nota.placaDoVeiculo,
            ufDoVeiculo: nota.ufDoVeiculo,
            codigoDaAntt: nota.codigoDaAntt,
            frete: nota.frete,
            valorDoFrete: nota.valorDoFrete,
            volume: nota.volume,
            especie: nota.especie,
            marca: nota.marca,
            numeracao: nota.numeracao,
            pesoBruto: nota.pesoBruto,
            pesoLiquido: nota.pesoLiquido,
            formaDePagamento: nota.formaDePagamento,
            historico: nota.historico,
            observacao: nota.observacao,
            sequenciaDoVendedor: nota.sequenciaDoVendedor,
            valorDoSeguro: nota.valorDoSeguro,
            outrasDespesas: nota.outrasDespesas,
          });

          // Carregar dados do cliente
          if (nota.sequenciaDoGeral) {
            const cliente = await notaFiscalService.obterCliente(nota.sequenciaDoGeral);
            setClienteSelecionado(cliente);
            setBuscaCliente(cliente.nome);
          }

          // Carregar dados da transportadora
          if (nota.sequenciaDaTransportadora && !nota.transportadoraAvulsa) {
            const transp = await notaFiscalService.obterTransportadora(nota.sequenciaDaTransportadora);
            setTransportadoraSelecionada(transp);
            setBuscaTransportadora(transp.nome);
          }

          // Carregar vendedor
          if (nota.sequenciaDoVendedor) {
            setVendedorSelecionado({ sequenciaDoGeral: nota.sequenciaDoVendedor, nome: nota.nomeVendedor });
            setBuscaVendedor(nota.nomeVendedor);
          }

          // Preencher listas de itens
          setProdutosLista(nota.produtos || []);
          setConjuntosLista(nota.conjuntos || []);
          setPecasLista(nota.pecas || []);
          setParcelasLista(nota.parcelas || []);
        } catch (err: any) {
          console.error('Erro ao carregar nota fiscal:', err);
          setError(err.response?.data?.mensagem || 'Erro ao carregar nota fiscal');
        } finally {
          setLoading(false);
        }
      };
      loadNota();
    }
  }, [isEditing, id]);

  // Buscar clientes
  const buscarClientes = useCallback(async (termo: string) => {
    if (!termo || termo.length < 2) {
      setClientesEncontrados([]);
      return;
    }

    try {
      setBuscandoClientes(true);
      const clientes = await notaFiscalService.listarClientes(termo);
      setClientesEncontrados(clientes);
    } catch (err) {
      console.error('Erro ao buscar clientes:', err);
    } finally {
      setBuscandoClientes(false);
    }
  }, []);

  // Buscar transportadoras
  const buscarTransportadoras = useCallback(async (termo: string) => {
    if (!termo || termo.length < 2) {
      setTransportadorasEncontradas([]);
      return;
    }

    try {
      const transportadoras = await notaFiscalService.listarTransportadoras(termo);
      setTransportadorasEncontradas(transportadoras);
    } catch (err) {
      console.error('Erro ao buscar transportadoras:', err);
    }
  }, []);

  // Buscar vendedores
  const buscarVendedores = useCallback(async (termo: string) => {
    if (!termo || termo.length < 2) {
      setVendedoresEncontrados([]);
      return;
    }

    try {
      const vendedores = await notaFiscalService.listarVendedores(termo);
      setVendedoresEncontrados(vendedores);
    } catch (err) {
      console.error('Erro ao buscar vendedores:', err);
    }
  }, []);

  // Debounce para buscas
  useEffect(() => {
    const timer = setTimeout(() => {
      if (showClienteDropdown) buscarClientes(buscaCliente);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaCliente, showClienteDropdown, buscarClientes]);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (showTransportadoraDropdown) buscarTransportadoras(buscaTransportadora);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaTransportadora, showTransportadoraDropdown, buscarTransportadoras]);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (showVendedorDropdown) buscarVendedores(buscaVendedor);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaVendedor, showVendedorDropdown, buscarVendedores]);

  // Handlers
  const handleInputChange = (field: keyof NotaFiscalCreateUpdateDto, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  const handleSelectCliente = (cliente: ClienteComboDto) => {
    setClienteSelecionado(cliente);
    setBuscaCliente(cliente.nome);
    setFormData(prev => ({ ...prev, sequenciaDoGeral: cliente.sequenciaDoGeral }));
    setShowClienteDropdown(false);
  };

  const handleClearCliente = () => {
    setClienteSelecionado(null);
    setBuscaCliente('');
    setFormData(prev => ({ ...prev, sequenciaDoGeral: 0 }));
  };

  const handleSelectTransportadora = (transp: TransportadoraComboDto) => {
    setTransportadoraSelecionada(transp);
    setBuscaTransportadora(transp.nome);
    setFormData(prev => ({
      ...prev,
      sequenciaDaTransportadora: transp.sequenciaDoGeral,
      transportadoraAvulsa: false,
    }));
    setShowTransportadoraDropdown(false);
  };

  const handleClearTransportadora = () => {
    setTransportadoraSelecionada(null);
    setBuscaTransportadora('');
    setFormData(prev => ({ ...prev, sequenciaDaTransportadora: 0 }));
  };

  const handleSelectVendedor = (vendedor: VendedorComboDto) => {
    setVendedorSelecionado(vendedor);
    setBuscaVendedor(vendedor.nome);
    setFormData(prev => ({ ...prev, sequenciaDoVendedor: vendedor.sequenciaDoGeral }));
    setShowVendedorDropdown(false);
  };

  const handleClearVendedor = () => {
    setVendedorSelecionado(null);
    setBuscaVendedor('');
    setFormData(prev => ({ ...prev, sequenciaDoVendedor: 0 }));
  };

  const handleSave = async () => {
    // Validações
    if (!formData.sequenciaDoGeral) {
      setError('Selecione um cliente');
      return;
    }
    // Só exige propriedade se o cliente tiver propriedades cadastradas
    if (propriedades.length > 0 && !formData.sequenciaDaPropriedade) {
      setError('Selecione uma propriedade');
      return;
    }
    if (!emitente) {
      setError('Nenhum emitente cadastrado. Cadastre o emitente em Sistema > Dados do Emitente');
      return;
    }
    if (!formData.sequenciaDaNatureza) {
      setError('Selecione uma natureza de operação');
      return;
    }
    if (!formData.dataDeEmissao) {
      setError('Informe a data de emissão');
      return;
    }

    try {
      setSaving(true);
      setError(null);

      if (isEditing && id) {
        await notaFiscalService.atualizar(Number(id), formData);
      } else {
        const nota = await notaFiscalService.criar(formData);
        navigate(`/faturamento/notas-fiscais/${nota.sequenciaDaNotaFiscal}`, { replace: true });
        return;
      }

      navigate('/faturamento/notas-fiscais');
    } catch (err: any) {
      console.error('Erro ao salvar nota fiscal:', err);
      setError(err.response?.data?.mensagem || 'Erro ao salvar nota fiscal');
    } finally {
      setSaving(false);
    }
  };

  const handleBack = () => {
    navigate('/faturamento/notas-fiscais');
  };

  // ==========================================
  // FUNÇÕES PARA PRODUTOS
  // ==========================================
  const buscarProdutosCombo = useCallback(async (termo: string) => {
    if (!termo || termo.length < 2) {
      setProdutosCombo([]);
      return;
    }
    try {
      const prods = await notaFiscalService.listarProdutosCombo(termo);
      setProdutosCombo(prods);
    } catch (err) {
      console.error('Erro ao buscar produtos:', err);
    }
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (showProdutoDropdown) buscarProdutosCombo(buscaProduto);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaProduto, showProdutoDropdown, buscarProdutosCombo]);

  const handleAddProdutoRow = () => {
    // Inicia nova linha de inserção (sem precisar clicar em botão - já fica disponível)
    if (!novoProduto) {
      setNovoProduto({
        sequenciaDoProduto: 0,
        quantidade: 1,
        valorUnitario: 0,
        aliquotaIcms: 0,
        aliquotaIpi: 0,
      });
      setBuscaProduto('');
    }
  };

  // Auto-inicializa a linha de inserção quando entra na aba de produtos
  useEffect(() => {
    if (activeTab === 'produtos' && isEditing && !novoProduto && !temPecasOuConjuntos && !isReadOnly) {
      handleAddProdutoRow();
    }
  }, [activeTab, isEditing, temPecasOuConjuntos, isReadOnly]);

  const handleCancelProduto = () => {
    setNovoProduto(null);
    setBuscaProduto('');
    setShowProdutoDropdown(false);
    setDropdownPosition(null);
  };

  // Atualiza posição do dropdown quando input recebe foco
  const updateDropdownPosition = useCallback(() => {
    if (produtoInputRef.current) {
      const rect = produtoInputRef.current.getBoundingClientRect();
      setDropdownPosition({
        top: rect.bottom + window.scrollY + 4,
        left: rect.left + window.scrollX,
        width: Math.max(rect.width, 400)
      });
    }
  }, []);

  // Fecha dropdown ao clicar fora
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      const target = event.target as HTMLElement;
      if (showProdutoDropdown && !target.closest('.produto-dropdown-container') && !target.closest('.produto-search-input')) {
        setShowProdutoDropdown(false);
        setDropdownPosition(null);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [showProdutoDropdown]);

  const handleSelectProdutoCombo = async (prod: ProdutoComboDto) => {
    // Primeiro fecha dropdown
    setShowProdutoDropdown(false);
    setDropdownPosition(null);
    setBuscaProduto(prod.descricao);

    // Atualiza dados básicos
    setNovoProduto(prev => ({
      ...prev,
      sequenciaDoProduto: prod.sequenciaDoProduto,
      valorUnitario: prod.precoVenda,
    }));

    // Chama API para calcular impostos
    if (id) {
      try {
        const result = await notaFiscalService.calcularImposto(Number(id), {
          tipoItem: 1, // Produto
          sequenciaDoItem: prod.sequenciaDoProduto,
          quantidade: novoProduto?.quantidade || 1,
          valorUnitario: prod.precoVenda,
          desconto: 0,
          valorFrete: 0
        });

        // Atualiza o novoProduto com os impostos calculados
        // Mapeia nomes do result (maiúsculas) para nomes do DTO (minúsculas)
        setNovoProduto(prev => ({
          ...prev,
          sequenciaDoProduto: prod.sequenciaDoProduto,
          valorUnitario: prod.precoVenda,
          cstIcms: result.cst,
          aliquotaIcms: result.aliquotaICMS || 0,
          valorIcms: result.valorICMS || 0,
          baseDeCalculoIcms: result.baseCalculoICMS || 0,
          aliquotaSt: result.aliquotaICMSST || 0,
          valorIcmsSt: result.valorICMSST || 0,
          baseDeCalculoSt: result.baseCalculoST || 0,
          aliquotaIpi: result.aliquotaIPI || 0,
          valorIpi: result.valorIPI || 0,
          aliquotaPis: result.aliquotaPIS || 0,
          valorPis: result.valorPIS || 0,
          aliquotaCofins: result.aliquotaCOFINS || 0,
          valorCofins: result.valorCOFINS || 0,
        }));
      } catch (err) {
        console.error('Erro ao calcular impostos:', err);
      }
    }
  };

  const handleSaveProduto = async () => {
    if (!novoProduto?.sequenciaDoProduto || !id) return;

    try {
      setSalvandoProduto(true);
      const produtoDto: ProdutoDaNotaFiscalCreateDto = {
        sequenciaDoProduto: novoProduto.sequenciaDoProduto,
        quantidade: novoProduto.quantidade || 1,
        valorUnitario: novoProduto.valorUnitario || 0,
        desconto: novoProduto.desconto || 0,
        // ICMS
        cstIcms: novoProduto.cstIcms,
        aliquotaIcms: novoProduto.aliquotaIcms || 0,
        baseDeCalculoIcms: novoProduto.baseDeCalculoIcms || 0,
        valorIcms: novoProduto.valorIcms || 0,
        // ICMS ST
        aliquotaSt: novoProduto.aliquotaSt || 0,
        baseDeCalculoSt: novoProduto.baseDeCalculoSt || 0,
        valorIcmsSt: novoProduto.valorIcmsSt || 0,
        // IPI
        aliquotaIpi: novoProduto.aliquotaIpi || 0,
        valorIpi: novoProduto.valorIpi || 0,
        // PIS
        aliquotaPis: novoProduto.aliquotaPis || 0,
        valorPis: novoProduto.valorPis || 0,
        // COFINS
        aliquotaCofins: novoProduto.aliquotaCofins || 0,
        valorCofins: novoProduto.valorCofins || 0,
      };

      const novoProd = await notaFiscalService.adicionarProduto(Number(id), produtoDto);
      setProdutosLista(prev => [...prev, novoProd]);

      // Recarregar nota para atualizar totais
      const nota = await notaFiscalService.obterPorId(Number(id));
      setNotaOriginal(nota);

      // Automaticamente abre nova linha de inserção para continuar adicionando
      setNovoProduto({
        sequenciaDoProduto: 0,
        quantidade: 1,
        valorUnitario: 0,
        aliquotaIcms: 0,
        aliquotaIpi: 0,
      });
      setBuscaProduto('');
      setShowProdutoDropdown(false);
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao adicionar produto');
    } finally {
      setSalvandoProduto(false);
    }
  };

  const handleRemoveProduto = async (produtoId: number) => {
    if (!id || !confirm('Confirma a exclusão do produto?')) return;

    try {
      await notaFiscalService.removerProduto(Number(id), produtoId);
      setProdutosLista(prev => prev.filter(p => p.sequenciaDoProdutoDaNotaFiscal !== produtoId));

      // Recarregar nota para atualizar totais
      const nota = await notaFiscalService.obterPorId(Number(id));
      setNotaOriginal(nota);
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao remover produto');
    }
  };

  // ==========================================
  // FUNÇÕES PARA CONJUNTOS
  // ==========================================
  const buscarConjuntosCombo = useCallback(async (termo: string) => {
    if (!termo || termo.length < 2) {
      setConjuntosCombo([]);
      return;
    }
    try {
      const conjs = await notaFiscalService.listarConjuntosCombo(termo);
      setConjuntosCombo(conjs);
    } catch (err) {
      console.error('Erro ao buscar conjuntos:', err);
    }
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (showConjuntoDropdown) buscarConjuntosCombo(buscaConjunto);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaConjunto, showConjuntoDropdown, buscarConjuntosCombo]);

  const handleAddConjuntoRow = () => {
    if (!novoConjunto) {
      setNovoConjunto({
        sequenciaDoConjunto: 0,
        quantidade: 1,
        valorUnitario: 0,
        aliquotaIpi: 0,
      });
      setBuscaConjunto('');
    }
  };

  // Auto-inicializa linha de inserção na aba de conjuntos
  useEffect(() => {
    if (activeTab === 'conjuntos' && isEditing && !novoConjunto && !temProdutos && !isReadOnly) {
      handleAddConjuntoRow();
    }
  }, [activeTab, isEditing, temProdutos, isReadOnly]);

  const handleCancelConjunto = () => {
    setNovoConjunto(null);
    setBuscaConjunto('');
    setShowConjuntoDropdown(false);
  };

  const handleSelectConjuntoCombo = async (conj: ConjuntoComboDto) => {
    // Primeiro fecha dropdown
    setShowConjuntoDropdown(false);
    setBuscaConjunto(conj.descricao);

    // Atualiza dados básicos
    setNovoConjunto(prev => ({
      ...prev,
      sequenciaDoConjunto: conj.sequenciaDoConjunto,
      valorUnitario: conj.precoVenda,
    }));

    // Chama API para calcular impostos
    if (id) {
      try {
        const result = await notaFiscalService.calcularImposto(Number(id), {
          tipoItem: 2, // Conjunto
          sequenciaDoItem: conj.sequenciaDoConjunto,
          quantidade: novoConjunto?.quantidade || 1,
          valorUnitario: conj.precoVenda,
          desconto: 0,
          valorFrete: 0
        });

        // Atualiza o novoConjunto com os impostos calculados
        // Mapeia nomes do result (maiúsculas) para nomes do DTO (minúsculas)
        setNovoConjunto(prev => ({
          ...prev,
          sequenciaDoConjunto: conj.sequenciaDoConjunto,
          valorUnitario: conj.precoVenda,
          cstIcms: result.cst,
          aliquotaIcms: result.aliquotaICMS || 0,
          valorIcms: result.valorICMS || 0,
          baseDeCalculoIcms: result.baseCalculoICMS || 0,
          aliquotaSt: result.aliquotaICMSST || 0,
          valorIcmsSt: result.valorICMSST || 0,
          baseDeCalculoSt: result.baseCalculoST || 0,
          aliquotaIpi: result.aliquotaIPI || 0,
          valorIpi: result.valorIPI || 0,
          aliquotaPis: result.aliquotaPIS || 0,
          valorPis: result.valorPIS || 0,
          aliquotaCofins: result.aliquotaCOFINS || 0,
          valorCofins: result.valorCOFINS || 0,
        }));
      } catch (err) {
        console.error('Erro ao calcular impostos do conjunto:', err);
      }
    }
  };

  const handleSaveConjunto = async () => {
    if (!novoConjunto?.sequenciaDoConjunto || !id) return;

    try {
      setSalvandoConjunto(true);
      const conjuntoDto: ConjuntoDaNotaFiscalCreateDto = {
        sequenciaDoConjunto: novoConjunto.sequenciaDoConjunto,
        quantidade: novoConjunto.quantidade || 1,
        valorUnitario: novoConjunto.valorUnitario || 0,
        desconto: novoConjunto.desconto || 0,
        // ICMS
        cstIcms: novoConjunto.cstIcms,
        aliquotaIcms: novoConjunto.aliquotaIcms || 0,
        baseDeCalculoIcms: novoConjunto.baseDeCalculoIcms || 0,
        valorIcms: novoConjunto.valorIcms || 0,
        // ICMS ST
        aliquotaSt: novoConjunto.aliquotaSt || 0,
        baseDeCalculoSt: novoConjunto.baseDeCalculoSt || 0,
        valorIcmsSt: novoConjunto.valorIcmsSt || 0,
        // IPI
        aliquotaIpi: novoConjunto.aliquotaIpi || 0,
        valorIpi: novoConjunto.valorIpi || 0,
        // PIS
        aliquotaPis: novoConjunto.aliquotaPis || 0,
        valorPis: novoConjunto.valorPis || 0,
        // COFINS
        aliquotaCofins: novoConjunto.aliquotaCofins || 0,
        valorCofins: novoConjunto.valorCofins || 0,
      };

      const novoConj = await notaFiscalService.adicionarConjunto(Number(id), conjuntoDto);
      setConjuntosLista(prev => [...prev, novoConj]);

      const nota = await notaFiscalService.obterPorId(Number(id));
      setNotaOriginal(nota);

      // Automaticamente abre nova linha para continuar adicionando
      setNovoConjunto({
        sequenciaDoConjunto: 0,
        quantidade: 1,
        valorUnitario: 0,
        aliquotaIpi: 0,
      });
      setBuscaConjunto('');
      setShowConjuntoDropdown(false);
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao adicionar conjunto');
    } finally {
      setSalvandoConjunto(false);
    }
  };

  const handleRemoveConjunto = async (conjuntoId: number) => {
    if (!id || !confirm('Confirma a exclusão do conjunto?')) return;

    try {
      await notaFiscalService.removerConjunto(Number(id), conjuntoId);
      setConjuntosLista(prev => prev.filter(c => c.sequenciaDoConjuntoDaNotaFiscal !== conjuntoId));

      const nota = await notaFiscalService.obterPorId(Number(id));
      setNotaOriginal(nota);
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao remover conjunto');
    }
  };

  // ==========================================
  // FUNÇÕES PARA PEÇAS
  // ==========================================
  const buscarPecasCombo = useCallback(async (termo: string) => {
    if (!termo || termo.length < 2) {
      setPecasCombo([]);
      return;
    }
    try {
      const pecas = await notaFiscalService.listarPecasCombo(termo);
      setPecasCombo(pecas);
    } catch (err) {
      console.error('Erro ao buscar peças:', err);
    }
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (showPecaDropdown) buscarPecasCombo(buscaPeca);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaPeca, showPecaDropdown, buscarPecasCombo]);

  const handleAddPecaRow = () => {
    if (!novaPeca) {
      setNovaPeca({
        sequenciaDaPeca: 0,
        quantidade: 1,
        valorUnitario: 0,
        aliquotaIpi: 0,
      });
      setBuscaPeca('');
    }
  };

  // Auto-inicializa linha de inserção na aba de peças
  useEffect(() => {
    if (activeTab === 'pecas' && isEditing && !novaPeca && !temProdutos && !isReadOnly) {
      handleAddPecaRow();
    }
  }, [activeTab, isEditing, temProdutos, isReadOnly]);

  const handleCancelPeca = () => {
    setNovaPeca(null);
    setBuscaPeca('');
    setShowPecaDropdown(false);
  };

  const handleSelectPecaCombo = async (peca: PecaComboDto) => {
    // Primeiro fecha dropdown
    setShowPecaDropdown(false);
    setBuscaPeca(peca.descricao);

    // Atualiza dados básicos
    setNovaPeca(prev => ({
      ...prev,
      sequenciaDaPeca: peca.sequenciaDaPeca,
      valorUnitario: peca.precoVenda,
    }));

    // Chama API para calcular impostos
    if (id) {
      try {
        const result = await notaFiscalService.calcularImposto(Number(id), {
          tipoItem: 3, // Peça
          sequenciaDoItem: peca.sequenciaDaPeca,
          quantidade: novaPeca?.quantidade || 1,
          valorUnitario: peca.precoVenda,
          desconto: 0,
          valorFrete: 0
        });

        // Atualiza a novaPeca com os impostos calculados
        // Mapeia nomes do result (maiúsculas) para nomes do DTO (minúsculas)
        setNovaPeca(prev => ({
          ...prev,
          sequenciaDaPeca: peca.sequenciaDaPeca,
          valorUnitario: peca.precoVenda,
          cstIcms: result.cst,
          aliquotaIcms: result.aliquotaICMS || 0,
          valorIcms: result.valorICMS || 0,
          baseDeCalculoIcms: result.baseCalculoICMS || 0,
          aliquotaSt: result.aliquotaICMSST || 0,
          valorIcmsSt: result.valorICMSST || 0,
          baseDeCalculoSt: result.baseCalculoST || 0,
          aliquotaIpi: result.aliquotaIPI || 0,
          valorIpi: result.valorIPI || 0,
          aliquotaPis: result.aliquotaPIS || 0,
          valorPis: result.valorPIS || 0,
          aliquotaCofins: result.aliquotaCOFINS || 0,
          valorCofins: result.valorCOFINS || 0,
        }));
      } catch (err) {
        console.error('Erro ao calcular impostos da peça:', err);
      }
    }
  };

  const handleSavePeca = async () => {
    if (!novaPeca?.sequenciaDaPeca || !id) return;

    try {
      setSalvandoPeca(true);
      const pecaDto: PecaDaNotaFiscalCreateDto = {
        sequenciaDaPeca: novaPeca.sequenciaDaPeca,
        quantidade: novaPeca.quantidade || 1,
        valorUnitario: novaPeca.valorUnitario || 0,
        desconto: novaPeca.desconto || 0,
        // ICMS
        cstIcms: novaPeca.cstIcms,
        aliquotaIcms: novaPeca.aliquotaIcms || 0,
        baseDeCalculoIcms: novaPeca.baseDeCalculoIcms || 0,
        valorIcms: novaPeca.valorIcms || 0,
        // ICMS ST
        aliquotaSt: novaPeca.aliquotaSt || 0,
        baseDeCalculoSt: novaPeca.baseDeCalculoSt || 0,
        valorIcmsSt: novaPeca.valorIcmsSt || 0,
        // IPI
        aliquotaIpi: novaPeca.aliquotaIpi || 0,
        valorIpi: novaPeca.valorIpi || 0,
        // PIS
        aliquotaPis: novaPeca.aliquotaPis || 0,
        valorPis: novaPeca.valorPis || 0,
        // COFINS
        aliquotaCofins: novaPeca.aliquotaCofins || 0,
        valorCofins: novaPeca.valorCofins || 0,
      };

      const novaPc = await notaFiscalService.adicionarPeca(Number(id), pecaDto);
      setPecasLista(prev => [...prev, novaPc]);

      const nota = await notaFiscalService.obterPorId(Number(id));
      setNotaOriginal(nota);

      // Automaticamente abre nova linha para continuar adicionando
      setNovaPeca({
        sequenciaDaPeca: 0,
        quantidade: 1,
        valorUnitario: 0,
        aliquotaIpi: 0,
      });
      setBuscaPeca('');
      setShowPecaDropdown(false);
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao adicionar peça');
    } finally {
      setSalvandoPeca(false);
    }
  };

  const handleRemovePeca = async (pecaId: number) => {
    if (!id || !confirm('Confirma a exclusão da peça?')) return;

    try {
      await notaFiscalService.removerPeca(Number(id), pecaId);
      setPecasLista(prev => prev.filter(p => p.sequenciaDaPecaDaNotaFiscal !== pecaId));

      const nota = await notaFiscalService.obterPorId(Number(id));
      setNotaOriginal(nota);
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao remover peça');
    }
  };

  // ==========================================
  // FUNÇÕES PARA PARCELAS
  // ==========================================
  const handleAddParcelaRow = () => {
    const proximoNumero = parcelasLista.length + 1;
    setNovaParcela({
      numeroDaParcela: proximoNumero,
      valor: 0,
      dataDeVencimento: '',
    });
  };

  const handleCancelParcela = () => {
    setNovaParcela(null);
  };

  const handleSaveParcela = async () => {
    if (!novaParcela?.valor || !id) return;

    try {
      setSalvandoParcela(true);
      const parcelaDto: ParcelaNotaFiscalCreateDto = {
        numeroDaParcela: novaParcela.numeroDaParcela || 1,
        valor: novaParcela.valor,
        dataDeVencimento: novaParcela.dataDeVencimento || null,
      };

      const novaPc = await notaFiscalService.adicionarParcela(Number(id), parcelaDto);
      setParcelasLista(prev => [...prev, novaPc]);
      handleCancelParcela();
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao adicionar parcela');
    } finally {
      setSalvandoParcela(false);
    }
  };

  const handleRemoveParcela = async (parcelaId: number) => {
    if (!id || !confirm('Confirma a exclusão da parcela?')) return;

    try {
      await notaFiscalService.removerParcela(Number(id), parcelaId);
      setParcelasLista(prev => prev.filter(p => p.sequenciaDaParcela !== parcelaId));
    } catch (err: any) {
      alert(err.response?.data?.mensagem || 'Erro ao remover parcela');
    }
  };

  // Formatar CPF/CNPJ
  const formatCpfCnpj = (doc: string) => {
    if (!doc) return '';
    const numeros = doc.replace(/\D/g, '');
    if (numeros.length === 11) {
      return numeros.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
    }
    if (numeros.length === 14) {
      return numeros.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
    }
    return doc;
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <Loader2 className="h-8 w-8 text-blue-600 animate-spin" />
      </div>
    );
  }

  return (
    <div className="space-y-4 sm:space-y-6 p-4 sm:p-0">
      {/* Header com Status e Total */}
      <div className="flex flex-col gap-4">
        <div className="flex items-center gap-3 sm:gap-4">
          <button
            onClick={handleBack}
            className="p-2 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-lg transition-colors"
          >
            <ArrowLeft className="h-5 w-5" />
          </button>
          <div className="flex-1 min-w-0">
            <div className="flex flex-wrap items-center gap-2 sm:gap-3">
              <h1 className="text-lg sm:text-2xl font-bold text-gray-900 flex items-center gap-2 truncate">
                <FileText className="h-5 sm:h-7 w-5 sm:w-7 text-blue-600 flex-shrink-0" />
                <span className="truncate">{isEditing ? `Nota #${notaOriginal?.numeroDaNotaFiscal || id}` : 'Nova Nota Fiscal'}</span>
              </h1>
              {/* Badge de Status */}
              {notaOriginal && (
                <span className={`px-2 sm:px-2.5 py-1 text-xs font-semibold rounded-full ${notaOriginal.notaCancelada
                  ? 'bg-red-100 text-red-700'
                  : notaOriginal.autorizado
                    ? 'bg-green-100 text-green-700'
                    : 'bg-gray-100 text-gray-600'
                  }`}>
                  {notaOriginal.notaCancelada ? 'Cancelada' : notaOriginal.autorizado ? 'Autorizada' : 'Não Enviada'}
                </span>
              )}
            </div>
            {notaOriginal && (
              <p className="mt-1 text-xs sm:text-sm text-gray-500 truncate">
                {notaOriginal.nomeDoCliente} • {notaOriginal.descricaoNatureza}
              </p>
            )}
          </div>
        </div>
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
          {/* Total da Nota */}
          {notaOriginal && (
            <div className="sm:hidden bg-blue-50 rounded-lg p-3">
              <p className="text-xs text-gray-500 uppercase tracking-wide">Total da Nota</p>
              <p className="text-xl font-bold text-blue-600">
                R$ {(notaOriginal.valorTotalDaNotaFiscal || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
              </p>
            </div>
          )}
          <div className="hidden sm:block">
            {notaOriginal && (
              <div className="text-right">
                <p className="text-xs text-gray-500 uppercase tracking-wide">Total da Nota</p>
                <p className="text-xl font-bold text-blue-600">
                  R$ {(notaOriginal.valorTotalDaNotaFiscal || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                </p>
              </div>
            )}
          </div>
          <div className="flex gap-2">
            <button
              onClick={handleBack}
              className="flex-1 sm:flex-none px-4 py-2.5 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
            >
              Cancelar
            </button>
            {!isReadOnly && (
              <button
                onClick={handleSave}
                disabled={saving}
                className="flex-1 sm:flex-none inline-flex items-center justify-center gap-2 px-4 py-2.5 text-sm font-medium text-white bg-blue-600 rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {saving ? (
                  <Loader2 className="h-4 w-4 animate-spin" />
                ) : (
                  <Save className="h-4 w-4" />
                )}
                <span className="hidden sm:inline">Salvar</span>
              </button>
            )}
            {/* Botão Autorizar - aparece apenas após a nota ser salva e não estar autorizada/cancelada */}
            {isEditing && notaOriginal && !notaOriginal.autorizado && !notaOriginal.notaCancelada && (
              <button
                onClick={() => alert('Funcionalidade de autorização em desenvolvimento')}
                className="hidden sm:inline-flex items-center gap-2 px-4 py-2.5 text-sm font-medium text-white bg-green-600 rounded-lg hover:bg-green-700 transition-colors"
              >
                <Send className="h-4 w-4" />
                Autorizar
              </button>
            )}
          </div>
        </div>
      </div>

      {/* Aviso de nota autorizada - somente leitura */}
      {isReadOnly && (
        <div className="p-4 bg-amber-50 border border-amber-200 rounded-lg flex items-start gap-3">
          <AlertCircle className="h-5 w-5 text-amber-600 flex-shrink-0 mt-0.5" />
          <div>
            <p className="text-sm font-medium text-amber-800">Nota Fiscal Autorizada - Somente Leitura</p>
            <p className="text-sm text-amber-700">Esta nota fiscal já foi autorizada e não pode ser editada. Para fazer alterações, utilize uma carta de correção ou cancele a nota.</p>
          </div>
        </div>
      )}

      {/* Erro */}
      {error && (
        <div className="p-4 bg-red-50 border border-red-200 rounded-lg flex items-start gap-3">
          <AlertCircle className="h-5 w-5 text-red-500 flex-shrink-0 mt-0.5" />
          <div>
            <p className="text-sm font-medium text-red-800">{error}</p>
          </div>
          <button
            onClick={() => setError(null)}
            className="ml-auto p-1 text-red-500 hover:text-red-700"
          >
            <X className="h-4 w-4" />
          </button>
        </div>
      )}

      {/* Tabs */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
        <div className="border-b border-gray-200">
          <nav className="flex overflow-x-auto scrollbar-hide">
            {TABS.map((tab) => {
              const disabled = isTabDisabled(tab.id);
              const disabledMessage = getTabDisabledMessage(tab.id);

              return (
                <button
                  key={tab.id}
                  onClick={() => !disabled && setActiveTab(tab.id)}
                  disabled={disabled}
                  title={disabledMessage || undefined}
                  className={`flex-shrink-0 flex items-center gap-1.5 sm:gap-2 px-3 sm:px-6 py-3 sm:py-4 text-xs sm:text-sm font-medium whitespace-nowrap border-b-2 transition-colors ${disabled
                    ? 'border-transparent text-gray-300 cursor-not-allowed'
                    : activeTab === tab.id
                      ? 'border-blue-600 text-blue-600'
                      : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                    }`}
                >
                  {tab.icon}
                  <span className="hidden sm:inline">{tab.label}</span>
                  <span className="sm:hidden">{tab.label.split(' - ')[0]}</span>
                  {disabled && (
                    <span className="ml-1 text-xs bg-red-100 text-red-600 px-1.5 py-0.5 rounded-full hidden sm:inline">
                      Bloqueado
                    </span>
                  )}
                </button>
              );
            })}
          </nav>
        </div>

        <div className="p-4 sm:p-6">
          {/* Aba 1 - Dados Principais */}
          {activeTab === 'dados' && (
            <div className={`space-y-6 ${isReadOnly ? 'pointer-events-none opacity-60' : ''}`}>
              {/* Seção: Identificação */}
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center gap-2">
                  <FileText className="h-5 w-5 text-blue-600" />
                  Identificação
                </h3>

                {/* Destaque para Número da NFE - Campo principal */}
                {isEditing && notaOriginal && (
                  <div className="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 border-2 border-blue-200 rounded-xl">
                    <div className="flex flex-wrap items-center gap-6">
                      <div className="flex-1 min-w-[200px]">
                        <label className="block text-xs font-medium text-blue-600 mb-1 uppercase tracking-wider">
                          Número da NFe
                        </label>
                        <div className="text-3xl font-bold text-blue-700">
                          {notaOriginal.numeroDaNfe > 0 ? notaOriginal.numeroDaNfe : 'Não emitida'}
                        </div>
                      </div>
                      <div>
                        <label className="block text-xs font-medium text-gray-500 mb-1 uppercase tracking-wider">
                          Sequência
                        </label>
                        <div className="text-xl font-semibold text-gray-700">
                          #{notaOriginal.sequenciaDaNotaFiscal}
                        </div>
                      </div>
                      <div>
                        <label className="block text-xs font-medium text-gray-500 mb-1 uppercase tracking-wider">
                          Número NF
                        </label>
                        <div className="text-xl font-semibold text-gray-700">
                          {notaOriginal.numeroDaNotaFiscal || '-'}
                        </div>
                      </div>
                      {notaOriginal.chaveDeAcessoDaNfe && (
                        <div className="flex-1 min-w-[300px]">
                          <label className="block text-xs font-medium text-gray-500 mb-1 uppercase tracking-wider">
                            Chave de Acesso
                          </label>
                          <div className="text-xs font-mono text-gray-600 break-all">
                            {notaOriginal.chaveDeAcessoDaNfe}
                          </div>
                        </div>
                      )}
                    </div>
                  </div>
                )}

                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  {/* Data de Emissão */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Data de Emissão <span className="text-red-500">*</span>
                    </label>
                    <div className="relative">
                      <Calendar className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                      <input
                        type="date"
                        value={formData.dataDeEmissao || ''}
                        onChange={(e) => handleInputChange('dataDeEmissao', e.target.value)}
                        className="w-full pl-10 pr-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      />
                    </div>
                  </div>

                  {/* Tipo de Nota */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Tipo de Nota
                    </label>
                    <select
                      value={formData.tipoDeNota || 0}
                      onChange={(e) => handleInputChange('tipoDeNota', Number(e.target.value))}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    >
                      <option value={0}>Saída</option>
                      <option value={1}>Entrada</option>
                      <option value={2}>Serviço</option>
                    </select>
                  </div>
                </div>
              </div>

              {/* Cliente e Propriedade */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {/* Cliente */}
                <div className="relative">
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">
                    Cliente <span className="text-red-500">*</span>
                  </label>
                  <div className="relative">
                    {buscandoClientes ? (
                      <Loader2 className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-blue-500 animate-spin" />
                    ) : (
                      <User className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                    )}
                    <input
                      type="text"
                      value={buscaCliente}
                      onChange={(e) => {
                        setBuscaCliente(e.target.value);
                        if (!clienteSelecionado) setShowClienteDropdown(true);
                      }}
                      onFocus={() => !clienteSelecionado && setShowClienteDropdown(true)}
                      placeholder="Buscar cliente..."
                      className="w-full pl-10 pr-10 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                    {clienteSelecionado && (
                      <button
                        onClick={handleClearCliente}
                        className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
                      >
                        <X className="h-4 w-4" />
                      </button>
                    )}
                  </div>
                  {/* Dropdown Cliente */}
                  {showClienteDropdown && clientesEncontrados.length > 0 && (
                    <div className="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded-lg shadow-lg max-h-60 overflow-y-auto">
                      {clientesEncontrados.map((c) => (
                        <button
                          key={c.sequenciaDoGeral}
                          onClick={() => handleSelectCliente(c)}
                          className="w-full px-4 py-3 text-left hover:bg-gray-50 border-b border-gray-100 last:border-0"
                        >
                          <div className="font-medium text-gray-900">{c.nome}</div>
                          <div className="text-sm text-gray-500">{c.cidade}/{c.uf}</div>
                        </button>
                      ))}
                    </div>
                  )}
                </div>

                {/* Propriedade */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">
                    Propriedade
                  </label>
                  <select
                    value={formData.sequenciaDaPropriedade || 0}
                    onChange={(e) => handleInputChange('sequenciaDaPropriedade', Number(e.target.value))}
                    disabled={propriedades.length === 0}
                    className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
                  >
                    <option value={0}>Selecione...</option>
                    {propriedades.map((p) => (
                      <option key={p.sequenciaDaPropriedade} value={p.sequenciaDaPropriedade}>
                        {p.nome}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              {/* Natureza e Cobrança */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {/* Natureza */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">
                    Natureza de Operação <span className="text-red-500">*</span>
                  </label>
                  <select
                    value={formData.sequenciaDaNatureza || 0}
                    onChange={(e) => handleInputChange('sequenciaDaNatureza', Number(e.target.value))}
                    className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value={0}>Selecione...</option>
                    {naturezas.map((n) => (
                      <option key={n.sequenciaDaNatureza} value={n.sequenciaDaNatureza}>
                        {n.descricao}
                      </option>
                    ))}
                  </select>
                </div>

                {/* Tipo de Cobrança */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">
                    Tipo de Cobrança
                  </label>
                  <select
                    value={formData.sequenciaDaCobranca || 0}
                    onChange={(e) => handleInputChange('sequenciaDaCobranca', Number(e.target.value))}
                    className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value={0}>Selecione...</option>
                    {tiposCobranca.map((t) => (
                      <option key={t.sequenciaDaCobranca} value={t.sequenciaDaCobranca}>
                        {t.descricao}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              {/* Vendedor */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="relative">
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">
                    Vendedor
                  </label>
                  <div className="relative">
                    <User className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                    <input
                      type="text"
                      value={buscaVendedor}
                      onChange={(e) => {
                        setBuscaVendedor(e.target.value);
                        if (!vendedorSelecionado) setShowVendedorDropdown(true);
                      }}
                      onFocus={() => !vendedorSelecionado && setShowVendedorDropdown(true)}
                      placeholder="Buscar vendedor..."
                      className="w-full pl-10 pr-10 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                    {vendedorSelecionado && (
                      <button
                        onClick={handleClearVendedor}
                        className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
                      >
                        <X className="h-4 w-4" />
                      </button>
                    )}
                  </div>
                  {showVendedorDropdown && vendedoresEncontrados.length > 0 && (
                    <div className="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded-lg shadow-lg max-h-60 overflow-y-auto">
                      {vendedoresEncontrados.map((v) => (
                        <button
                          key={v.sequenciaDoGeral}
                          onClick={() => handleSelectVendedor(v)}
                          className="w-full px-4 py-3 text-left hover:bg-gray-50 border-b border-gray-100 last:border-0"
                        >
                          <div className="font-medium text-gray-900">{v.nome}</div>
                        </button>
                      ))}
                    </div>
                  )}
                </div>
              </div>

            </div>
          )}

          {/* Aba 2 - Transportadora */}
          {activeTab === 'transportadora' && (
            <div className={`space-y-6 ${isReadOnly ? 'pointer-events-none opacity-60' : ''}`}>
              {!formData.transportadoraAvulsa ? (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {/* Busca de Transportadora */}
                  <div className="relative">
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Transportadora
                    </label>
                    <div className="relative">
                      <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                      <input
                        type="text"
                        value={buscaTransportadora}
                        onChange={(e) => {
                          setBuscaTransportadora(e.target.value);
                          if (!transportadoraSelecionada) setShowTransportadoraDropdown(true);
                        }}
                        onFocus={() => !transportadoraSelecionada && setShowTransportadoraDropdown(true)}
                        placeholder="Digite para buscar transportadora..."
                        className="w-full pl-10 pr-10 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      />
                      {transportadoraSelecionada && (
                        <button
                          onClick={handleClearTransportadora}
                          className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
                        >
                          <X className="h-4 w-4" />
                        </button>
                      )}
                    </div>

                    {showTransportadoraDropdown && transportadorasEncontradas.length > 0 && (
                      <div className="absolute z-10 w-full mt-1 bg-white border border-gray-200 rounded-lg shadow-lg max-h-60 overflow-y-auto">
                        {transportadorasEncontradas.map((t) => (
                          <button
                            key={t.sequenciaDoGeral}
                            onClick={() => handleSelectTransportadora(t)}
                            className="w-full px-4 py-3 text-left hover:bg-gray-50 border-b border-gray-100 last:border-0"
                          >
                            <div className="font-medium text-gray-900">{t.nome}</div>
                            <div className="text-sm text-gray-500">
                              {formatCpfCnpj(t.documento)} • {t.cidade}/{t.uf}
                            </div>
                          </button>
                        ))}
                      </div>
                    )}
                  </div>

                  {/* Card da Transportadora selecionada */}
                  {transportadoraSelecionada && (
                    <div className="bg-gradient-to-br from-emerald-50 to-teal-50 border border-emerald-200 rounded-xl p-4">
                      <div className="flex items-start justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 bg-emerald-600 rounded-full flex items-center justify-center text-white">
                            <Truck className="h-5 w-5" />
                          </div>
                          <div>
                            <p className="font-semibold text-gray-900">{transportadoraSelecionada.nome}</p>
                            <p className="text-sm text-gray-600">{formatCpfCnpj(transportadoraSelecionada.documento)}</p>
                            <p className="text-sm text-gray-500">{transportadoraSelecionada.cidade}/{transportadoraSelecionada.uf}</p>
                          </div>
                        </div>
                      </div>
                    </div>
                  )}
                </div>
              ) : (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Nome da Transportadora
                    </label>
                    <input
                      type="text"
                      value={formData.nomeDaTransportadoraAvulsa || ''}
                      onChange={(e) => handleInputChange('nomeDaTransportadoraAvulsa', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      CNPJ/CPF
                    </label>
                    <input
                      type="text"
                      value={formData.documentoDaTransportadora || ''}
                      onChange={(e) => handleInputChange('documentoDaTransportadora', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Inscrição Estadual
                    </label>
                    <input
                      type="text"
                      value={formData.ieDaTransportadora || ''}
                      onChange={(e) => handleInputChange('ieDaTransportadora', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Endereço
                    </label>
                    <input
                      type="text"
                      value={formData.enderecoDaTransportadora || ''}
                      onChange={(e) => handleInputChange('enderecoDaTransportadora', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                </div>
              )}

              {/* Veículo */}
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4">Veículo</h3>
                <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Placa
                    </label>
                    <input
                      type="text"
                      value={formData.placaDoVeiculo || ''}
                      onChange={(e) => handleInputChange('placaDoVeiculo', e.target.value.toUpperCase())}
                      maxLength={8}
                      placeholder="AAA-0000"
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      UF
                    </label>
                    <input
                      type="text"
                      value={formData.ufDoVeiculo || ''}
                      onChange={(e) => handleInputChange('ufDoVeiculo', e.target.value.toUpperCase())}
                      maxLength={2}
                      placeholder="SP"
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Código ANTT
                    </label>
                    <input
                      type="text"
                      value={formData.codigoDaAntt || ''}
                      onChange={(e) => handleInputChange('codigoDaAntt', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                </div>
              </div>

              {/* Frete */}
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4">Frete</h3>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Modalidade do Frete
                    </label>
                    <select
                      value={formData.frete || '9'}
                      onChange={(e) => handleInputChange('frete', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    >
                      {Object.entries(MODALIDADES_FRETE).map(([key, value]) => (
                        <option key={key} value={key}>
                          {key} - {value}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Valor do Frete
                    </label>
                    <input
                      type="number"
                      value={formData.valorDoFrete || ''}
                      onChange={(e) => handleInputChange('valorDoFrete', parseFloat(e.target.value) || 0)}
                      step="0.01"
                      min="0"
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                </div>
              </div>

              {/* Volumes */}
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4">Volumes</h3>
                <div className="grid grid-cols-2 md:grid-cols-6 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Quantidade
                    </label>
                    <input
                      type="number"
                      value={formData.volume || ''}
                      onChange={(e) => handleInputChange('volume', parseInt(e.target.value) || 0)}
                      min="0"
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Espécie
                    </label>
                    <input
                      type="text"
                      value={formData.especie || ''}
                      onChange={(e) => handleInputChange('especie', e.target.value)}
                      placeholder="CAIXA"
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Marca
                    </label>
                    <input
                      type="text"
                      value={formData.marca || ''}
                      onChange={(e) => handleInputChange('marca', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Numeração
                    </label>
                    <input
                      type="text"
                      value={formData.numeracao || ''}
                      onChange={(e) => handleInputChange('numeracao', e.target.value)}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Peso Bruto (kg)
                    </label>
                    <input
                      type="number"
                      value={formData.pesoBruto || ''}
                      onChange={(e) => handleInputChange('pesoBruto', parseFloat(e.target.value) || 0)}
                      step="0.01"
                      min="0"
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Peso Líquido (kg)
                    </label>
                    <input
                      type="number"
                      value={formData.pesoLiquido || ''}
                      onChange={(e) => handleInputChange('pesoLiquido', parseFloat(e.target.value) || 0)}
                      step="0.01"
                      min="0"
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Aba 3 - Produtos */}
          {activeTab === 'produtos' && (
            <div className={`space-y-6 ${isReadOnly ? 'pointer-events-none opacity-60' : ''}`}>
              {/* Alerta de bloqueio */}
              {temPecasOuConjuntos && (
                <div className="flex items-center gap-3 p-4 bg-amber-50 border border-amber-200 rounded-lg">
                  <AlertCircle className="h-5 w-5 text-amber-600" />
                  <p className="text-sm text-amber-800">
                    <strong>Atenção:</strong> Não é possível adicionar Produtos porque já existem Peças ou Conjuntos nesta nota fiscal.
                  </p>
                </div>
              )}

              {/* Header */}
              <div className="flex items-center justify-between">
                <h3 className="text-lg font-semibold text-gray-900 flex items-center gap-2">
                  <Package className="h-5 w-5 text-blue-600" />
                  Produtos da Nota Fiscal
                </h3>
                <span className="text-sm text-gray-500">
                  {produtosLista.length > 0 && `${produtosLista.length} ${produtosLista.length === 1 ? 'item' : 'itens'}`}
                </span>
              </div>

              {/* GRID DE PRODUTOS - Tabela Profissional */}
              <div className="bg-white border border-gray-300 rounded-lg shadow-sm overflow-hidden">
                {/* Barra de informação */}
                <div className="flex items-center justify-between px-3 py-2 bg-slate-700 text-white">
                  <span className="text-xs font-medium flex items-center gap-2">
                    <Package className="h-4 w-4" />
                    ITENS DA NOTA FISCAL
                  </span>
                  <span className="text-xs opacity-80">← Deslize horizontalmente para ver todos os campos →</span>
                </div>
                {/* Container com scroll horizontal e vertical */}
                <div
                  className="overflow-auto scrollbar-thin scrollbar-thumb-slate-400 scrollbar-track-slate-100"
                  style={{ maxHeight: '500px', minHeight: '300px' }}
                >
                  <table className="min-w-[2200px] w-full text-xs border-collapse">
                    {/* Cabeçalho */}
                    <thead className="bg-slate-100 sticky top-0 z-10">
                      {/* Linha de agrupamento */}
                      <tr className="border-b border-slate-300">
                        <th colSpan={6} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">Identificação</th>
                        <th colSpan={4} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">Valores</th>
                        <th colSpan={3} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">ICMS</th>
                        <th colSpan={3} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">ICMS-ST</th>
                        <th colSpan={2} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">IPI</th>
                        <th colSpan={2} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">PIS</th>
                        <th colSpan={2} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">COFINS</th>
                        <th colSpan={2} className="px-2 py-1.5 text-center text-[10px] font-bold text-slate-600 uppercase tracking-wider border-r border-slate-300">IBS/CBS</th>
                        <th rowSpan={2} className="px-2 py-2 text-center text-[10px] font-bold text-slate-600 uppercase w-14"></th>
                      </tr>
                      {/* Linha de colunas */}
                      <tr className="border-b-2 border-slate-400 bg-slate-50">
                        <th className="px-2 py-2 text-center text-[10px] font-semibold text-slate-700 w-8">#</th>
                        <th className="px-2 py-2 text-left text-[10px] font-semibold text-slate-700 min-w-[200px]">Descrição do Produto</th>
                        <th className="px-2 py-2 text-center text-[10px] font-semibold text-slate-700 w-20">NCM</th>
                        <th className="px-2 py-2 text-center text-[10px] font-semibold text-slate-700 w-14">CFOP</th>
                        <th className="px-2 py-2 text-center text-[10px] font-semibold text-slate-700 w-12">CST</th>
                        <th className="px-2 py-2 text-center text-[10px] font-semibold text-slate-700 w-10 border-r border-slate-300">UN</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16">Qtde</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-20">Vl.Unitário</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16">Desconto</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-20 border-r border-slate-300">Vl.Total</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-18">Base Cálc.</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-10">Alíq.</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16 border-r border-slate-300">Valor</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-18">Base ST</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-10">Alíq.</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16 border-r border-slate-300">Valor</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-10">Alíq.</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16 border-r border-slate-300">Valor</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-10">Alíq.</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16 border-r border-slate-300">Valor</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-10">Alíq.</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16 border-r border-slate-300">Valor</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16">IBS</th>
                        <th className="px-2 py-2 text-right text-[10px] font-semibold text-slate-700 w-16 border-r border-slate-300">CBS</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-200">
                      {/* Linhas de produtos existentes */}
                      {produtosLista.map((produto, index) => (
                        <tr key={produto.sequenciaDoProdutoDaNotaFiscal} className="hover:bg-slate-50 transition-colors">
                          <td className="px-2 py-2 text-center text-xs font-medium text-slate-600">{index + 1}</td>
                          <td className="px-2 py-2">
                            <div className="text-xs font-medium text-slate-900 truncate max-w-[200px]" title={produto.descricaoProduto}>{produto.descricaoProduto}</div>
                          </td>
                          <td className="px-2 py-2 text-center text-xs text-slate-600 font-mono">{produto.ncm}</td>
                          <td className="px-2 py-2 text-center text-xs text-slate-600 font-mono">{produto.cfop}</td>
                          <td className="px-2 py-2 text-center text-xs text-slate-600 font-mono">{produto.cst}</td>
                          <td className="px-2 py-2 text-center text-xs text-slate-500 border-r border-slate-200">{produto.unidade}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-700 font-mono">{produto.quantidade.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{produto.valorUnitario.toLocaleString('pt-BR', { minimumFractionDigits: 4 })}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{(produto.desconto || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right text-xs font-bold text-slate-900 border-r border-slate-200 font-mono">{produto.valorTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* ICMS */}
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{(produto.baseDeCalculoIcms || produto.valorTotal).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{produto.aliquotaIcms}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-700 font-mono border-r border-slate-200">{produto.valorIcms.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* ICMS ST */}
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{(produto.baseDeCalculoSt || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{produto.aliquotaSt || 0}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-700 font-mono border-r border-slate-200">{(produto.valorIcmsSt || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* IPI */}
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{produto.aliquotaIpi}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-700 font-mono border-r border-slate-200">{produto.valorIpi.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* PIS */}
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{produto.aliquotaPis || 0}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-700 font-mono border-r border-slate-200">{(produto.valorPis || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* COFINS */}
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{produto.aliquotaCofins || 0}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-700 font-mono border-r border-slate-200">{(produto.valorCofins || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* IBS/CBS */}
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono">{(produto.valorIbs || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 font-mono border-r border-slate-200">{(produto.valorCbs || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-center">
                            {isEditing && (
                              <button
                                type="button"
                                onClick={() => handleRemoveProduto(produto.sequenciaDoProdutoDaNotaFiscal)}
                                className="p-1 text-slate-400 hover:text-red-600 hover:bg-red-50 rounded transition-colors"
                                title="Excluir"
                              >
                                <Trash2 className="h-3.5 w-3.5" />
                              </button>
                            )}
                          </td>
                        </tr>
                      ))}

                      {/* Linha de inserção de novo produto */}
                      {novoProduto && !temPecasOuConjuntos && (
                        <tr className="bg-amber-50 border-t-2 border-slate-400">
                          <td className="px-2 py-2 text-center text-xs font-medium text-slate-400">+</td>
                          <td className="px-2 py-2">
                            <input
                              ref={produtoInputRef}
                              type="text"
                              value={buscaProduto}
                              onChange={(e) => {
                                setBuscaProduto(e.target.value);
                                setShowProdutoDropdown(true);
                                updateDropdownPosition();
                              }}
                              onFocus={() => {
                                setShowProdutoDropdown(true);
                                updateDropdownPosition();
                              }}
                              placeholder="Digite para buscar produto..."
                              className="produto-search-input w-full px-2 py-1.5 text-xs border border-slate-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-blue-500 bg-white"
                              autoComplete="off"
                            />
                          </td>
                          <td className="px-2 py-2 text-center text-xs text-slate-400">-</td>
                          <td className="px-2 py-2 text-center text-xs text-slate-400">-</td>
                          <td className="px-2 py-2 text-center text-xs text-slate-400">-</td>
                          <td className="px-2 py-2 text-center text-xs text-slate-400 border-r border-slate-200">-</td>
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0.01" value={novoProduto.quantidade || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, quantidade: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="0,00" />
                          </td>
                          <td className="px-2 py-2">
                            <input type="number" step="0.0001" min="0" value={novoProduto.valorUnitario || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, valorUnitario: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="0,0000" />
                          </td>
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" value={novoProduto.desconto || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, desconto: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="0,00" />
                          </td>
                          <td className="px-2 py-2 text-right border-r border-slate-200">
                            <span className="text-xs font-bold text-slate-900 font-mono">
                              {(((novoProduto.quantidade || 0) * (novoProduto.valorUnitario || 0)) - (novoProduto.desconto || 0)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                            </span>
                          </td>
                          {/* ICMS */}
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" value={novoProduto.baseDeCalculoIcms || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, baseDeCalculoIcms: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="BC" />
                          </td>
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" max="100" value={novoProduto.aliquotaIcms || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, aliquotaIcms: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="%" />
                          </td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 border-r border-slate-200 font-mono">
                            {(((novoProduto.baseDeCalculoIcms || (novoProduto.quantidade || 0) * (novoProduto.valorUnitario || 0))) * ((novoProduto.aliquotaIcms || 0) / 100)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          {/* ICMS ST */}
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" value={novoProduto.baseDeCalculoSt || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, baseDeCalculoSt: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="BC" />
                          </td>
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" max="100" value={novoProduto.aliquotaSt || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, aliquotaSt: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="%" />
                          </td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 border-r border-slate-200 font-mono">
                            {(((novoProduto.baseDeCalculoSt || 0)) * ((novoProduto.aliquotaSt || 0) / 100)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          {/* IPI */}
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" max="100" value={novoProduto.aliquotaIpi || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, aliquotaIpi: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="%" />
                          </td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 border-r border-slate-200 font-mono">
                            {(((novoProduto.quantidade || 0) * (novoProduto.valorUnitario || 0)) * ((novoProduto.aliquotaIpi || 0) / 100)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          {/* PIS */}
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" max="100" value={novoProduto.aliquotaPis || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, aliquotaPis: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="%" />
                          </td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 border-r border-slate-200 font-mono">
                            {(((novoProduto.quantidade || 0) * (novoProduto.valorUnitario || 0)) * ((novoProduto.aliquotaPis || 0) / 100)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          {/* COFINS */}
                          <td className="px-2 py-2">
                            <input type="number" step="0.01" min="0" max="100" value={novoProduto.aliquotaCofins || ''} onChange={(e) => setNovoProduto(prev => ({ ...prev, aliquotaCofins: parseFloat(e.target.value) || 0 }))} className="w-full px-1 py-1 text-xs text-right border border-slate-300 rounded focus:ring-1 focus:ring-slate-500 bg-white font-mono" placeholder="%" />
                          </td>
                          <td className="px-2 py-2 text-right text-xs text-slate-600 border-r border-slate-200 font-mono">
                            {(((novoProduto.quantidade || 0) * (novoProduto.valorUnitario || 0)) * ((novoProduto.aliquotaCofins || 0) / 100)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          {/* IBS/CBS */}
                          <td className="px-2 py-2 text-center text-xs text-slate-400">-</td>
                          <td className="px-2 py-2 text-center text-xs text-slate-400 border-r border-slate-200">-</td>
                          <td className="px-2 py-2">
                            <div className="flex items-center justify-center gap-1">
                              <button type="button" onClick={handleSaveProduto} disabled={salvandoProduto || !novoProduto.sequenciaDoProduto} className="p-1 text-white bg-slate-700 rounded hover:bg-slate-800 disabled:opacity-50 disabled:cursor-not-allowed transition-colors" title="Confirmar">
                                {salvandoProduto ? <div className="h-3 w-3 border-2 border-white border-t-transparent rounded-full animate-spin" /> : <Check className="h-3 w-3" />}
                              </button>
                              <button type="button" onClick={handleCancelProduto} className="p-1 text-slate-600 bg-slate-200 rounded hover:bg-slate-300 transition-colors" title="Cancelar">
                                <X className="h-3 w-3" />
                              </button>
                            </div>
                          </td>
                        </tr>
                      )}

                      {/* Mensagem quando vazio */}
                      {produtosLista.length === 0 && !novoProduto && (
                        <tr>
                          <td colSpan={25} className="px-4 py-10 text-center bg-slate-50">
                            <Package className="h-8 w-8 text-slate-300 mx-auto mb-2" />
                            <p className="text-slate-500 text-sm">Nenhum item adicionado</p>
                            <p className="text-xs text-slate-400 mt-1">Clique em "Adicionar Produto" para incluir itens</p>
                          </td>
                        </tr>
                      )}
                    </tbody>

                    {/* Rodapé com totais */}
                    {produtosLista.length > 0 && (
                      <tfoot className="bg-slate-200 border-t-2 border-slate-400 sticky bottom-0">
                        <tr className="text-xs">
                          <td colSpan={6} className="px-2 py-2 text-right font-bold text-slate-700 uppercase border-r border-slate-300">Totais</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-700 font-mono">{produtosLista.reduce((acc, p) => acc + p.quantidade, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right text-slate-400">-</td>
                          <td className="px-2 py-2 text-right font-mono text-slate-600">{produtosLista.reduce((acc, p) => acc + (p.desconto || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-900 border-r border-slate-300 font-mono">{produtosLista.reduce((acc, p) => acc + p.valorTotal, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* ICMS */}
                          <td className="px-2 py-2 text-right font-mono text-slate-700">{produtosLista.reduce((acc, p) => acc + (p.baseDeCalculoIcms || p.valorTotal), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-center text-slate-400">-</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-700 border-r border-slate-300 font-mono">{produtosLista.reduce((acc, p) => acc + p.valorIcms, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* ICMS ST */}
                          <td className="px-2 py-2 text-right font-mono text-slate-700">{produtosLista.reduce((acc, p) => acc + (p.baseDeCalculoSt || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-center text-slate-400">-</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-700 border-r border-slate-300 font-mono">{produtosLista.reduce((acc, p) => acc + (p.valorIcmsSt || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* IPI */}
                          <td className="px-2 py-2 text-center text-slate-400">-</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-700 border-r border-slate-300 font-mono">{produtosLista.reduce((acc, p) => acc + p.valorIpi, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* PIS */}
                          <td className="px-2 py-2 text-center text-slate-400">-</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-700 border-r border-slate-300 font-mono">{produtosLista.reduce((acc, p) => acc + (p.valorPis || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* COFINS */}
                          <td className="px-2 py-2 text-center text-slate-400">-</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-700 border-r border-slate-300 font-mono">{produtosLista.reduce((acc, p) => acc + (p.valorCofins || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          {/* IBS/CBS */}
                          <td className="px-2 py-2 text-right font-bold text-slate-700 font-mono">{produtosLista.reduce((acc, p) => acc + (p.valorIbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2 text-right font-bold text-slate-700 border-r border-slate-300 font-mono">{produtosLista.reduce((acc, p) => acc + (p.valorCbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-2"></td>
                        </tr>
                      </tfoot>
                    )}
                  </table>
                </div>

                {/* Botão Adicionar */}
                {isEditing && !temPecasOuConjuntos && !novoProduto && (
                  <div className="px-3 py-2 bg-slate-100 border-t border-slate-300">
                    <button
                      type="button"
                      onClick={handleAddProdutoRow}
                      className="flex items-center gap-2 px-3 py-1.5 text-white bg-slate-700 rounded hover:bg-slate-800 transition-colors font-medium text-xs"
                    >
                      <Plus className="h-3.5 w-3.5" />
                      Adicionar Produto
                    </button>
                  </div>
                )}
              </div>
            </div>
          )}

          {/* Aba 4 - Conjuntos */}
          {activeTab === 'conjuntos' && (
            <div className={`space-y-6 ${isReadOnly ? 'pointer-events-none opacity-60' : ''}`}>
              {/* Alerta de bloqueio por regra de exclusividade */}
              {temProdutos && (
                <div className="flex items-center gap-3 p-4 bg-amber-50 border border-amber-200 rounded-lg">
                  <AlertCircle className="h-5 w-5 text-amber-600" />
                  <p className="text-sm text-amber-800">
                    <strong>Atenção:</strong> Não é possível adicionar Conjuntos porque já existem Produtos nesta nota fiscal.
                    Remova os Produtos para poder adicionar Conjuntos.
                  </p>
                </div>
              )}

              {/* Header */}
              <div className="flex items-center justify-between">
                <h3 className="text-lg font-semibold text-gray-900 flex items-center gap-2">
                  <Boxes className="h-5 w-5 text-indigo-600" />
                  Conjuntos da Nota Fiscal
                </h3>
                <span className="text-sm text-gray-500">
                  {conjuntosLista.length > 0 && `${conjuntosLista.length} ${conjuntosLista.length === 1 ? 'item' : 'itens'}`}
                </span>
              </div>

              {/* Tabela de Conjuntos */}
              <div className="border border-gray-200 rounded-lg">
                <div className="overflow-x-auto" style={{ maxHeight: '400px' }}>
                  <table className="min-w-[900px] w-full divide-y divide-gray-200 text-xs">
                    <thead className="bg-gray-50 sticky top-0 z-10">
                      <tr>
                        <th className="px-2 py-2 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider w-14">Seq</th>
                        <th className="px-2 py-2 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider min-w-[200px]">Conjunto</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-20">Qtde</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.Unit</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.Total</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-16">%IPI</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.IPI</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.IBS</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.CBS</th>
                        <th className="px-2 py-2 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider w-16">Usado</th>
                        <th className="px-2 py-2 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider w-16">Ações</th>
                      </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                      {/* Linhas de conjuntos existentes PRIMEIRO */}
                      {conjuntosLista.map((conj, index) => (
                        <tr key={conj.sequenciaDoConjuntoDaNotaFiscal || index} className="hover:bg-gray-50">
                          <td className="px-2 py-1.5 text-center">
                            <span className="inline-flex items-center justify-center w-8 h-6 bg-indigo-100 text-indigo-700 text-xs font-bold rounded">
                              {conj.sequenciaDoConjuntoDaNotaFiscal}
                            </span>
                          </td>
                          <td className="px-2 py-1.5">
                            <div className="text-xs font-medium text-gray-900 truncate max-w-[200px]" title={conj.descricaoConjunto}>{conj.descricaoConjunto}</div>
                          </td>
                          <td className="px-2 py-1.5 text-xs text-gray-900 text-right">{conj.quantidade.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-900 text-right">{conj.valorUnitario.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-900 text-right font-medium">{conj.valorTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{conj.valorTotal > 0 ? ((conj.valorIpi / conj.valorTotal) * 100).toFixed(2) : '0.00'}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{conj.valorIpi.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{(conj.valorIbs || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{(conj.valorCbs || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-center">
                            <span className={`px-1.5 py-0.5 text-xs font-medium rounded-full ${conj.usado ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-600'}`}>
                              {conj.usado ? 'S' : 'N'}
                            </span>
                          </td>
                          <td className="px-2 py-1.5 text-center">
                            <button
                              type="button"
                              onClick={() => handleRemoveConjunto(conj.sequenciaDoConjuntoDaNotaFiscal)}
                              disabled={!isEditing}
                              className="p-1 text-red-600 hover:bg-red-100 rounded transition-colors disabled:opacity-50"
                              title="Excluir"
                            >
                              <Trash2 className="h-3.5 w-3.5" />
                            </button>
                          </td>
                        </tr>
                      ))}

                      {/* Linha de inserção SEMPRE NO FINAL */}
                      {novoConjunto && !temProdutos && (
                        <tr className="bg-indigo-50 border-t-2 border-indigo-200">
                          <td className="px-2 py-1.5 text-center text-xs text-gray-400">
                            <span className="inline-flex items-center justify-center w-8 h-6 bg-indigo-200 text-indigo-700 text-xs font-bold rounded">
                              +
                            </span>
                          </td>
                          <td className="px-2 py-1.5">
                            <div className="relative">
                              <input
                                type="text"
                                value={buscaConjunto}
                                onChange={(e) => { setBuscaConjunto(e.target.value); setShowConjuntoDropdown(true); }}
                                onFocus={() => setShowConjuntoDropdown(true)}
                                placeholder="Buscar conjunto..."
                                className="w-full px-2 py-1 text-xs border border-gray-300 rounded focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                              />
                              {showConjuntoDropdown && conjuntosCombo.length > 0 && (
                                <div className="absolute z-20 w-80 mt-1 bg-white border border-gray-200 rounded-lg shadow-lg max-h-48 overflow-y-auto">
                                  {conjuntosCombo.map((c) => (
                                    <button
                                      key={c.sequenciaDoConjunto}
                                      type="button"
                                      onClick={() => handleSelectConjuntoCombo(c)}
                                      className="w-full px-3 py-2 text-left hover:bg-indigo-50 border-b border-gray-100 last:border-0"
                                    >
                                      <div className="text-sm font-medium text-gray-900">{c.descricao}</div>
                                      <div className="text-xs text-gray-500">R$ {c.precoVenda.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</div>
                                    </button>
                                  ))}
                                </div>
                              )}
                            </div>
                          </td>
                          <td className="px-2 py-1.5">
                            <input
                              type="number"
                              step="0.01"
                              min="0.01"
                              value={novoConjunto.quantidade || ''}
                              onChange={(e) => setNovoConjunto(prev => ({ ...prev, quantidade: parseFloat(e.target.value) || 0 }))}
                              className="w-full px-1 py-1 text-xs text-right border border-gray-300 rounded focus:ring-2 focus:ring-indigo-500"
                            />
                          </td>
                          <td className="px-2 py-1.5">
                            <input
                              type="number"
                              step="0.01"
                              min="0"
                              value={novoConjunto.valorUnitario || ''}
                              onChange={(e) => setNovoConjunto(prev => ({ ...prev, valorUnitario: parseFloat(e.target.value) || 0 }))}
                              className="w-full px-1 py-1 text-xs text-right border border-gray-300 rounded focus:ring-2 focus:ring-indigo-500"
                            />
                          </td>
                          <td className="px-2 py-1.5 text-right text-xs font-medium text-gray-900">
                            {((novoConjunto.quantidade || 0) * (novoConjunto.valorUnitario || 0)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          <td className="px-2 py-1.5">
                            <input
                              type="number"
                              step="0.01"
                              min="0"
                              max="100"
                              value={novoConjunto.aliquotaIpi || ''}
                              onChange={(e) => setNovoConjunto(prev => ({ ...prev, aliquotaIpi: parseFloat(e.target.value) || 0 }))}
                              className="w-full px-1 py-1 text-xs text-right border border-gray-300 rounded focus:ring-2 focus:ring-indigo-500"
                            />
                          </td>
                          <td className="px-2 py-1.5 text-right text-xs text-gray-600">
                            {(((novoConjunto.quantidade || 0) * (novoConjunto.valorUnitario || 0)) * ((novoConjunto.aliquotaIpi || 0) / 100)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          <td className="px-2 py-1.5 text-center text-xs text-gray-400">-</td>
                          <td className="px-2 py-1.5 text-center text-xs text-gray-400">-</td>
                          <td className="px-2 py-1.5 text-center">
                            <input
                              type="checkbox"
                              checked={novoConjunto.usado || false}
                              onChange={(e) => setNovoConjunto(prev => ({ ...prev, usado: e.target.checked }))}
                              className="w-3.5 h-3.5 text-indigo-600 rounded"
                            />
                          </td>
                          <td className="px-2 py-1.5 text-center">
                            <div className="flex items-center justify-center gap-1">
                              <button
                                type="button"
                                onClick={handleSaveConjunto}
                                disabled={salvandoConjunto || !novoConjunto.sequenciaDoConjunto}
                                className="px-1.5 py-0.5 text-xs font-medium text-white bg-indigo-600 rounded hover:bg-indigo-700 disabled:opacity-50"
                                title="Confirmar"
                              >
                                {salvandoConjunto ? '...' : '✓'}
                              </button>
                              <button
                                type="button"
                                onClick={handleCancelConjunto}
                                className="px-1.5 py-0.5 text-xs font-medium text-gray-600 bg-gray-200 rounded hover:bg-gray-300"
                                title="Cancelar"
                              >
                                ✕
                              </button>
                            </div>
                          </td>
                        </tr>
                      )}

                      {/* Mensagem quando não há conjuntos e nem linha de inserção */}
                      {conjuntosLista.length === 0 && !novoConjunto && (
                        <tr>
                          <td colSpan={11} className="px-4 py-12 text-center text-gray-500">
                            <Boxes className="h-12 w-12 text-gray-300 mx-auto mb-3" />
                            <p className="font-medium">Nenhum conjunto adicionado</p>
                            <p className="text-sm mt-1">Salve a nota fiscal para começar a adicionar conjuntos</p>
                          </td>
                        </tr>
                      )}
                    </tbody>
                  </table>
                </div>
              </div>

              {/* Totalizadores */}
              {conjuntosLista.length > 0 && (
                <div className="bg-gray-50 rounded-lg p-4">
                  <div className="grid grid-cols-2 md:grid-cols-6 gap-4 text-sm">
                    <div>
                      <span className="text-gray-600">Itens:</span>
                      <span className="ml-2 font-semibold text-gray-900">{conjuntosLista.length}</span>
                    </div>
                    <div>
                      <span className="text-gray-600">Total:</span>
                      <span className="ml-2 font-semibold text-indigo-600">
                        R$ {conjuntosLista.reduce((acc, c) => acc + c.valorTotal, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">IPI:</span>
                      <span className="ml-2 font-semibold text-gray-900">
                        R$ {conjuntosLista.reduce((acc, c) => acc + c.valorIpi, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">IBS:</span>
                      <span className="ml-2 font-semibold text-gray-900">
                        R$ {conjuntosLista.reduce((acc, c) => acc + (c.valorIbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">CBS:</span>
                      <span className="ml-2 font-semibold text-gray-900">
                        R$ {conjuntosLista.reduce((acc, c) => acc + (c.valorCbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">Total c/ Imp.:</span>
                      <span className="ml-2 font-semibold text-green-600">
                        R$ {conjuntosLista.reduce((acc, c) => acc + c.valorTotal + c.valorIpi + (c.valorIbs || 0) + (c.valorCbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                  </div>
                </div>
              )}
            </div>
          )}

          {/* Aba 5 - Peças */}
          {activeTab === 'pecas' && (
            <div className={`space-y-6 ${isReadOnly ? 'pointer-events-none opacity-60' : ''}`}>
              {/* Alerta de bloqueio por regra de exclusividade */}
              {temProdutos && (
                <div className="flex items-center gap-3 p-4 bg-amber-50 border border-amber-200 rounded-lg">
                  <AlertCircle className="h-5 w-5 text-amber-600" />
                  <p className="text-sm text-amber-800">
                    <strong>Atenção:</strong> Não é possível adicionar Peças porque já existem Produtos nesta nota fiscal.
                    Remova os Produtos para poder adicionar Peças.
                  </p>
                </div>
              )}

              {/* Header */}
              <div className="flex items-center justify-between">
                <h3 className="text-lg font-semibold text-gray-900 flex items-center gap-2">
                  <Cog className="h-5 w-5 text-orange-600" />
                  Peças da Nota Fiscal
                </h3>
                <span className="text-sm text-gray-500">
                  {pecasLista.length > 0 && `${pecasLista.length} ${pecasLista.length === 1 ? 'item' : 'itens'}`}
                </span>
              </div>

              {/* Tabela de Peças */}
              <div className="border border-gray-200 rounded-lg">
                <div className="overflow-x-auto" style={{ maxHeight: '400px' }}>
                  <table className="min-w-[900px] w-full divide-y divide-gray-200 text-xs">
                    <thead className="bg-gray-50 sticky top-0 z-10">
                      <tr>
                        <th className="px-2 py-2 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider w-14">Seq</th>
                        <th className="px-2 py-2 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider min-w-[200px]">Peça</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-20">Qtde</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.Unit</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.Total</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-16">%IPI</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.IPI</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.IBS</th>
                        <th className="px-2 py-2 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider w-24">Vl.CBS</th>
                        <th className="px-2 py-2 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider w-16">Usado</th>
                        <th className="px-2 py-2 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider w-16">Ações</th>
                      </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                      {/* Linhas de peças existentes PRIMEIRO */}
                      {pecasLista.map((peca, index) => (
                        <tr key={peca.sequenciaDaPecaDaNotaFiscal || index} className="hover:bg-gray-50">
                          <td className="px-2 py-1.5 text-center">
                            <span className="inline-flex items-center justify-center w-8 h-6 bg-orange-100 text-orange-700 text-xs font-bold rounded">
                              {peca.sequenciaDaPecaDaNotaFiscal}
                            </span>
                          </td>
                          <td className="px-2 py-1.5">
                            <div className="text-xs font-medium text-gray-900 truncate max-w-[200px]" title={peca.descricaoPeca}>{peca.descricaoPeca}</div>
                          </td>
                          <td className="px-2 py-1.5 text-xs text-gray-900 text-right">{peca.quantidade.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-900 text-right">{peca.valorUnitario.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-900 text-right font-medium">{peca.valorTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{peca.valorTotal > 0 ? ((peca.valorIpi / peca.valorTotal) * 100).toFixed(2) : '0.00'}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{peca.valorIpi.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{(peca.valorIbs || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-xs text-gray-600 text-right">{(peca.valorCbs || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</td>
                          <td className="px-2 py-1.5 text-center">
                            <span className={`px-1.5 py-0.5 text-xs font-medium rounded-full ${peca.usado ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-600'}`}>
                              {peca.usado ? 'S' : 'N'}
                            </span>
                          </td>
                          <td className="px-2 py-1.5 text-center">
                            <button
                              type="button"
                              onClick={() => handleRemovePeca(peca.sequenciaDaPecaDaNotaFiscal)}
                              disabled={!isEditing}
                              className="p-1 text-red-600 hover:bg-red-100 rounded transition-colors disabled:opacity-50"
                              title="Excluir"
                            >
                              <Trash2 className="h-3.5 w-3.5" />
                            </button>
                          </td>
                        </tr>
                      ))}

                      {/* Linha de inserção NO FINAL */}
                      {novaPeca && !temProdutos && (
                        <tr className="bg-orange-50 border-t-2 border-orange-200">
                          <td className="px-2 py-1.5 text-center text-xs text-gray-400">
                            <span className="inline-flex items-center justify-center w-8 h-6 bg-orange-200 text-orange-700 text-xs font-bold rounded">
                              +
                            </span>
                          </td>
                          <td className="px-2 py-1.5">
                            <div className="relative">
                              <input
                                type="text"
                                value={buscaPeca}
                                onChange={(e) => { setBuscaPeca(e.target.value); setShowPecaDropdown(true); }}
                                onFocus={() => setShowPecaDropdown(true)}
                                placeholder="Buscar peça..."
                                className="w-full px-2 py-1 text-xs border border-gray-300 rounded focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
                              />
                              {showPecaDropdown && pecasCombo.length > 0 && (
                                <div className="absolute z-20 w-80 mt-1 bg-white border border-gray-200 rounded-lg shadow-lg max-h-48 overflow-y-auto">
                                  {pecasCombo.map((p) => (
                                    <button
                                      key={p.sequenciaDaPeca}
                                      type="button"
                                      onClick={() => handleSelectPecaCombo(p)}
                                      className="w-full px-3 py-2 text-left hover:bg-orange-50 border-b border-gray-100 last:border-0"
                                    >
                                      <div className="text-sm font-medium text-gray-900">{p.descricao}</div>
                                      <div className="text-xs text-gray-500">R$ {p.precoVenda.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</div>
                                    </button>
                                  ))}
                                </div>
                              )}
                            </div>
                          </td>
                          <td className="px-2 py-1.5">
                            <input
                              type="number"
                              step="0.01"
                              min="0.01"
                              value={novaPeca.quantidade || ''}
                              onChange={(e) => setNovaPeca(prev => ({ ...prev, quantidade: parseFloat(e.target.value) || 0 }))}
                              className="w-full px-1 py-1 text-xs text-right border border-gray-300 rounded focus:ring-2 focus:ring-orange-500"
                            />
                          </td>
                          <td className="px-2 py-1.5">
                            <input
                              type="number"
                              step="0.01"
                              min="0"
                              value={novaPeca.valorUnitario || ''}
                              onChange={(e) => setNovaPeca(prev => ({ ...prev, valorUnitario: parseFloat(e.target.value) || 0 }))}
                              className="w-full px-1 py-1 text-xs text-right border border-gray-300 rounded focus:ring-2 focus:ring-orange-500"
                            />
                          </td>
                          <td className="px-2 py-1.5 text-right text-xs font-medium text-gray-900">
                            {((novaPeca.quantidade || 0) * (novaPeca.valorUnitario || 0)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          <td className="px-2 py-1.5">
                            <input
                              type="number"
                              step="0.01"
                              min="0"
                              max="100"
                              value={novaPeca.aliquotaIpi || ''}
                              onChange={(e) => setNovaPeca(prev => ({ ...prev, aliquotaIpi: parseFloat(e.target.value) || 0 }))}
                              className="w-full px-1 py-1 text-xs text-right border border-gray-300 rounded focus:ring-2 focus:ring-orange-500"
                            />
                          </td>
                          <td className="px-2 py-1.5 text-right text-xs text-gray-600">
                            {(((novaPeca.quantidade || 0) * (novaPeca.valorUnitario || 0)) * ((novaPeca.aliquotaIpi || 0) / 100)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </td>
                          <td className="px-2 py-1.5 text-center text-xs text-gray-400">-</td>
                          <td className="px-2 py-1.5 text-center text-xs text-gray-400">-</td>
                          <td className="px-2 py-1.5 text-center">
                            <input
                              type="checkbox"
                              checked={novaPeca.usado || false}
                              onChange={(e) => setNovaPeca(prev => ({ ...prev, usado: e.target.checked }))}
                              className="w-3.5 h-3.5 text-orange-600 rounded"
                            />
                          </td>
                          <td className="px-2 py-1.5 text-center">
                            <div className="flex items-center justify-center gap-1">
                              <button
                                type="button"
                                onClick={handleSavePeca}
                                disabled={salvandoPeca || !novaPeca.sequenciaDaPeca}
                                className="px-1.5 py-0.5 text-xs font-medium text-white bg-orange-600 rounded hover:bg-orange-700 disabled:opacity-50"
                                title="Confirmar"
                              >
                                {salvandoPeca ? '...' : '✓'}
                              </button>
                              <button
                                type="button"
                                onClick={handleCancelPeca}
                                className="px-1.5 py-0.5 text-xs font-medium text-gray-600 bg-gray-200 rounded hover:bg-gray-300"
                                title="Cancelar"
                              >
                                ✕
                              </button>
                            </div>
                          </td>
                        </tr>
                      )}

                      {/* Mensagem quando não há peças e nem linha de inserção */}
                      {pecasLista.length === 0 && !novaPeca && (
                        <tr>
                          <td colSpan={11} className="px-4 py-12 text-center text-gray-500">
                            <Cog className="h-12 w-12 text-gray-300 mx-auto mb-3" />
                            <p className="font-medium">Nenhuma peça adicionada</p>
                            <p className="text-sm mt-1">Salve a nota fiscal para começar a adicionar peças</p>
                          </td>
                        </tr>
                      )}
                    </tbody>
                  </table>
                </div>
              </div>

              {/* Totalizadores */}
              {pecasLista.length > 0 && (
                <div className="bg-gray-50 rounded-lg p-4">
                  <div className="grid grid-cols-2 md:grid-cols-6 gap-4 text-sm">
                    <div>
                      <span className="text-gray-600">Itens:</span>
                      <span className="ml-2 font-semibold text-gray-900">{pecasLista.length}</span>
                    </div>
                    <div>
                      <span className="text-gray-600">Total:</span>
                      <span className="ml-2 font-semibold text-orange-600">
                        R$ {pecasLista.reduce((acc, p) => acc + p.valorTotal, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">IPI:</span>
                      <span className="ml-2 font-semibold text-gray-900">
                        R$ {pecasLista.reduce((acc, p) => acc + p.valorIpi, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">IBS:</span>
                      <span className="ml-2 font-semibold text-gray-900">
                        R$ {pecasLista.reduce((acc, p) => acc + (p.valorIbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">CBS:</span>
                      <span className="ml-2 font-semibold text-gray-900">
                        R$ {pecasLista.reduce((acc, p) => acc + (p.valorCbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                    <div>
                      <span className="text-gray-600">Total c/ Imp.:</span>
                      <span className="ml-2 font-semibold text-green-600">
                        R$ {pecasLista.reduce((acc, p) => acc + p.valorTotal + p.valorIpi + (p.valorIbs || 0) + (p.valorCbs || 0), 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                  </div>
                </div>
              )}
            </div>
          )}

          {/* Aba 6 - Financeiro */}
          {activeTab === 'financeiro' && (
            <div className={`space-y-6 ${isReadOnly ? 'pointer-events-none opacity-60' : ''}`}>
              {/* Header */}
              <div className="flex items-center justify-between">
                <div>
                  <h3 className="text-lg font-semibold text-gray-900">Financeiro da Nota Fiscal</h3>
                  <p className="text-sm text-gray-500 mt-1">Totais, impostos e parcelas de pagamento</p>
                </div>
              </div>

              <div className="grid grid-cols-1 xl:grid-cols-3 gap-6">
                {/* Coluna 1 - Totais por Tipo */}
                <div className="bg-white border border-gray-200 rounded-xl p-5 shadow-sm">
                  <h4 className="text-md font-semibold text-gray-900 mb-4 pb-2 border-b flex items-center gap-2">
                    <Package className="h-5 w-5 text-blue-600" />
                    Valores por Tipo
                  </h4>
                  <div className="space-y-3">
                    <div className="flex justify-between items-center py-2 px-3 bg-gray-50 rounded-lg">
                      <span className="text-gray-600">Produtos:</span>
                      <span className="font-semibold text-gray-900">R$ {(notaOriginal?.valorTotalDosProdutos || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2 px-3 bg-gray-50 rounded-lg">
                      <span className="text-gray-600">Conjuntos:</span>
                      <span className="font-semibold text-gray-900">R$ {(notaOriginal?.valorTotalDosConjuntos || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2 px-3 bg-gray-50 rounded-lg">
                      <span className="text-gray-600">Peças:</span>
                      <span className="font-semibold text-gray-900">R$ {(notaOriginal?.valorTotalDasPecas || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2 px-3 bg-gray-50 rounded-lg">
                      <span className="text-gray-600">Frete:</span>
                      <span className="font-semibold text-gray-900">R$ {(notaOriginal?.valorDoFrete || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2 px-3 bg-gray-50 rounded-lg">
                      <span className="text-gray-600">Seguro:</span>
                      <span className="font-semibold text-gray-900">R$ {(notaOriginal?.valorDoSeguro || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2 px-3 bg-gray-50 rounded-lg">
                      <span className="text-gray-600">Outras Despesas:</span>
                      <span className="font-semibold text-gray-900">R$ {(notaOriginal?.outrasDespesas || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-3 px-4 bg-blue-600 text-white rounded-lg mt-4">
                      <span className="font-semibold">TOTAL DA NOTA:</span>
                      <span className="text-xl font-bold">R$ {(notaOriginal?.valorTotalDaNotaFiscal || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                  </div>
                </div>

                {/* Coluna 2 - Impostos */}
                <div className="bg-white border border-gray-200 rounded-xl p-5 shadow-sm">
                  <h4 className="text-md font-semibold text-gray-900 mb-4 pb-2 border-b flex items-center gap-2">
                    <DollarSign className="h-5 w-5 text-orange-600" />
                    Impostos
                  </h4>
                  <div className="space-y-3">
                    <div className="flex justify-between items-center py-2">
                      <span className="text-gray-600">Base ICMS:</span>
                      <span className="font-medium">R$ {(notaOriginal?.valorTotalDaBaseDeCalculo || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2">
                      <span className="text-gray-600">Valor ICMS:</span>
                      <span className="font-medium text-red-600">R$ {(notaOriginal?.valorTotalDoIcms || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2 border-t pt-3">
                      <span className="text-gray-600">Base ICMS ST:</span>
                      <span className="font-medium">R$ {(notaOriginal?.valorTotalDaBaseSt || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2">
                      <span className="text-gray-600">Valor ICMS ST:</span>
                      <span className="font-medium text-red-600">R$ {(notaOriginal?.valorTotalDoIcmsSt || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2 border-t pt-3">
                      <span className="text-gray-600">Total IPI:</span>
                      <span className="font-medium text-red-600">R$ {((notaOriginal?.valorTotalIpiDosProdutos || 0) + (notaOriginal?.valorTotalIpiDosConjuntos || 0) + (notaOriginal?.valorTotalIpiDasPecas || 0)).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2">
                      <span className="text-gray-600">Total PIS:</span>
                      <span className="font-medium">R$ {(notaOriginal?.valorTotalDoPis || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-2">
                      <span className="text-gray-600">Total COFINS:</span>
                      <span className="font-medium">R$ {(notaOriginal?.valorTotalDoCofins || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                    <div className="flex justify-between items-center py-3 px-4 bg-orange-100 text-orange-800 rounded-lg mt-4">
                      <span className="font-semibold">Valor Aprox. Tributos:</span>
                      <span className="text-lg font-bold">R$ {(notaOriginal?.valorTotalDoTributo || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                  </div>
                </div>

                {/* Coluna 3 - Parcelas */}
                <div className="bg-white border border-gray-200 rounded-xl p-5 shadow-sm">
                  <div className="flex items-center justify-between mb-4 pb-2 border-b">
                    <h4 className="text-md font-semibold text-gray-900 flex items-center gap-2">
                      <ClipboardList className="h-5 w-5 text-green-600" />
                      Parcelas
                    </h4>
                    <button
                      type="button"
                      onClick={handleAddParcelaRow}
                      disabled={!isEditing || novaParcela !== null}
                      className="inline-flex items-center gap-1 px-3 py-1.5 text-xs font-medium text-white bg-green-600 rounded-lg hover:bg-green-700 transition-colors disabled:opacity-50"
                    >
                      <Plus className="h-3 w-3" />
                      Adicionar
                    </button>
                  </div>

                  {/* Forma de Pagamento */}
                  <div className="mb-4">
                    <label className="block text-xs font-medium text-gray-600 mb-1.5">Forma de Pagamento</label>
                    <select
                      value={formData.formaDePagamento || ''}
                      onChange={(e) => handleInputChange('formaDePagamento', e.target.value)}
                      className="w-full px-3 py-2 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    >
                      <option value="">Selecione...</option>
                      <option value="01">01 - Dinheiro</option>
                      <option value="02">02 - Cheque</option>
                      <option value="03">03 - Cartão Crédito</option>
                      <option value="04">04 - Cartão Débito</option>
                      <option value="15">15 - Boleto</option>
                      <option value="17">17 - PIX</option>
                      <option value="90">90 - Sem Pagamento</option>
                      <option value="99">99 - Outros</option>
                    </select>
                  </div>

                  {/* Lista de Parcelas */}
                  <div className="space-y-2 max-h-[300px] overflow-y-auto">
                    {/* Linha de adição de parcela */}
                    {novaParcela && (
                      <div className="flex items-center gap-2 p-2 bg-green-50 rounded-lg">
                        <span className="w-6 h-6 flex items-center justify-center bg-green-600 text-white text-xs font-bold rounded-full">
                          {novaParcela.numeroDaParcela}
                        </span>
                        <input
                          type="date"
                          value={novaParcela.dataDeVencimento || ''}
                          onChange={(e) => setNovaParcela(prev => ({ ...prev, dataDeVencimento: e.target.value }))}
                          className="flex-1 px-2 py-1 text-sm border border-gray-300 rounded focus:ring-2 focus:ring-green-500"
                        />
                        <input
                          type="number"
                          step="0.01"
                          min="0"
                          placeholder="Valor"
                          value={novaParcela.valor || ''}
                          onChange={(e) => setNovaParcela(prev => ({ ...prev, valor: parseFloat(e.target.value) || 0 }))}
                          className="w-24 px-2 py-1 text-sm text-right border border-gray-300 rounded focus:ring-2 focus:ring-green-500"
                        />
                        <button
                          type="button"
                          onClick={handleSaveParcela}
                          disabled={salvandoParcela || !novaParcela.valor}
                          className="px-2 py-1 text-xs font-medium text-white bg-green-600 rounded hover:bg-green-700 disabled:opacity-50"
                        >
                          {salvandoParcela ? '...' : 'OK'}
                        </button>
                        <button
                          type="button"
                          onClick={handleCancelParcela}
                          className="px-2 py-1 text-xs font-medium text-gray-600 bg-gray-200 rounded hover:bg-gray-300"
                        >
                          X
                        </button>
                      </div>
                    )}

                    {/* Parcelas existentes */}
                    {parcelasLista.length > 0 ? (
                      parcelasLista.map((parcela, index) => (
                        <div key={parcela.sequenciaDaParcela || index} className="flex items-center justify-between py-2 px-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors group">
                          <div className="flex items-center gap-3">
                            <span className="w-6 h-6 flex items-center justify-center bg-blue-100 text-blue-700 text-xs font-bold rounded-full">
                              {parcela.numeroDaParcela}
                            </span>
                            <span className="text-sm text-gray-600">
                              {parcela.dataDeVencimento ? new Date(parcela.dataDeVencimento).toLocaleDateString('pt-BR') : '-'}
                            </span>
                          </div>
                          <div className="flex items-center gap-2">
                            <span className="text-sm font-semibold text-gray-900">
                              R$ {parcela.valor.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                            </span>
                            <button
                              type="button"
                              onClick={() => handleRemoveParcela(parcela.sequenciaDaParcela)}
                              disabled={!isEditing}
                              className="p-1 text-red-500 hover:bg-red-100 rounded opacity-0 group-hover:opacity-100 transition-opacity disabled:opacity-0"
                            >
                              <Trash2 className="h-3 w-3" />
                            </button>
                          </div>
                        </div>
                      ))
                    ) : !novaParcela && (
                      <div className="text-center py-8 text-gray-400">
                        <ClipboardList className="h-10 w-10 mx-auto mb-2 opacity-50" />
                        <p className="text-sm">Nenhuma parcela</p>
                      </div>
                    )}
                  </div>

                  {/* Total Parcelas */}
                  {parcelasLista.length > 0 && (
                    <div className="flex justify-between items-center py-3 px-4 bg-green-100 text-green-800 rounded-lg mt-4">
                      <span className="font-semibold">Total Parcelas:</span>
                      <span className="text-lg font-bold">
                        R$ {parcelasLista.reduce((acc, p) => acc + p.valor, 0).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </span>
                    </div>
                  )}
                </div>
              </div>

              {/* Observações */}
              <div className="bg-white border border-gray-200 rounded-xl p-5 shadow-sm">
                <h4 className="text-md font-semibold text-gray-900 mb-4 pb-2 border-b">Informações Adicionais</h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Histórico (Dados Adicionais)</label>
                    <textarea
                      value={formData.historico || ''}
                      onChange={(e) => handleInputChange('historico', e.target.value)}
                      rows={3}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Informações que aparecerão na nota fiscal..."
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Observação (Interesse do Fisco)</label>
                    <textarea
                      value={formData.observacao || ''}
                      onChange={(e) => handleInputChange('observacao', e.target.value)}
                      rows={3}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Informações adicionais de interesse do fisco..."
                    />
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Aba 7 - NFe */}
          {activeTab === 'nfe' && (
            <div className="space-y-6">
              <h3 className="text-lg font-semibold text-gray-900">Dados da NFe / NFSe</h3>

              {/* Status da NFe */}
              <div className="bg-gray-50 rounded-lg p-4">
                <div className="flex items-center gap-4">
                  <div className={`w-3 h-3 rounded-full ${notaOriginal?.autorizado ? 'bg-green-500' : notaOriginal?.transmitido ? 'bg-yellow-500' : 'bg-gray-400'}`}></div>
                  <span className="text-sm font-medium text-gray-700">
                    Status: {notaOriginal?.autorizado ? 'Autorizada' : notaOriginal?.transmitido ? 'Transmitida (aguardando retorno)' : 'Não transmitida'}
                  </span>
                  {notaOriginal?.notaCancelada && (
                    <span className="px-2 py-1 bg-red-100 text-red-700 text-xs font-medium rounded">CANCELADA</span>
                  )}
                </div>
              </div>

              <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                {/* Dados da NFe */}
                <div className="bg-white border border-gray-200 rounded-lg p-5 space-y-4">
                  <h4 className="text-md font-semibold text-gray-900 pb-2 border-b">Identificação NFe</h4>

                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1.5">Número NFe</label>
                      <input
                        type="text"
                        value={notaOriginal?.numeroDaNfe || ''}
                        disabled
                        className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg bg-gray-100 text-gray-600"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1.5">Número NF</label>
                      <input
                        type="text"
                        value={notaOriginal?.numeroDaNotaFiscal || ''}
                        disabled
                        className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg bg-gray-100 text-gray-600"
                      />
                    </div>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Chave de Acesso</label>
                    <input
                      type="text"
                      value={notaOriginal?.chaveDeAcessoDaNfe || ''}
                      disabled
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg bg-gray-100 text-gray-600 font-mono text-xs"
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Protocolo de Autorização</label>
                    <input
                      type="text"
                      value={notaOriginal?.protocoloDeAutorizacaoNfe || ''}
                      disabled
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg bg-gray-100 text-gray-600"
                    />
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1.5">Data/Hora NFe</label>
                      <input
                        type="text"
                        value={notaOriginal?.dataEHoraDaNfe || ''}
                        disabled
                        className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg bg-gray-100 text-gray-600"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1.5">Nº Recibo</label>
                      <input
                        type="text"
                        value={notaOriginal?.numeroDoReciboDaNfe || ''}
                        disabled
                        className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg bg-gray-100 text-gray-600"
                      />
                    </div>
                  </div>
                </div>

                {/* NFe Referenciada / Complementar */}
                <div className="bg-white border border-gray-200 rounded-lg p-5 space-y-4">
                  <h4 className="text-md font-semibold text-gray-900 pb-2 border-b">NFe Referenciada / Complementar</h4>

                  <div className="space-y-3">
                    <label className="flex items-center gap-2 cursor-pointer">
                      <input
                        type="checkbox"
                        checked={formData.nfeComplementar || false}
                        onChange={(e) => handleInputChange('nfeComplementar', e.target.checked)}
                        className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                      />
                      <span className="text-sm text-gray-700">NFe Complementar</span>
                    </label>

                    <label className="flex items-center gap-2 cursor-pointer">
                      <input
                        type="checkbox"
                        checked={formData.notaDeDevolucao || false}
                        onChange={(e) => handleInputChange('notaDeDevolucao', e.target.checked)}
                        className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                      />
                      <span className="text-sm text-gray-700">Nota de Devolução</span>
                    </label>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Chave NFe Referenciada</label>
                    <input
                      type="text"
                      value={formData.chaveAcessoNfeReferenciada || ''}
                      onChange={(e) => handleInputChange('chaveAcessoNfeReferenciada', e.target.value)}
                      maxLength={44}
                      placeholder="44 dígitos..."
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 font-mono"
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Chave Devolução 1</label>
                    <input
                      type="text"
                      value={formData.chaveDaDevolucao || ''}
                      onChange={(e) => handleInputChange('chaveDaDevolucao', e.target.value)}
                      maxLength={44}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 font-mono"
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Chave Devolução 2</label>
                    <input
                      type="text"
                      value={formData.chaveDaDevolucao2 || ''}
                      onChange={(e) => handleInputChange('chaveDaDevolucao2', e.target.value)}
                      maxLength={44}
                      className="w-full px-4 py-2.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 font-mono"
                    />
                  </div>
                </div>
              </div>

              {/* Ações NFe */}
              {notaOriginal && (
                <div className="bg-blue-50 rounded-lg p-4">
                  <h4 className="text-md font-semibold text-gray-900 mb-4">Ações NFe</h4>
                  <div className="flex flex-wrap gap-3">
                    <button
                      type="button"
                      disabled={notaOriginal.autorizado}
                      className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      <FileCheck className="h-4 w-4" />
                      Transmitir NFe
                    </button>
                    <button
                      type="button"
                      disabled={!notaOriginal.autorizado}
                      className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      <FileText className="h-4 w-4" />
                      Imprimir DANFE
                    </button>
                    <button
                      type="button"
                      disabled={!notaOriginal.autorizado || notaOriginal.notaCancelada}
                      className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium text-red-600 bg-red-50 rounded-lg hover:bg-red-100 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      <X className="h-4 w-4" />
                      Cancelar NFe
                    </button>
                    <button
                      type="button"
                      disabled={!notaOriginal.autorizado}
                      className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium text-orange-600 bg-orange-50 rounded-lg hover:bg-orange-100 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      <Settings className="h-4 w-4" />
                      Carta de Correção
                    </button>
                  </div>
                </div>
              )}
            </div>
          )}
        </div>
      </div>

      {/* Dropdown de Produtos usando Portal - fica fora do scroll */}
      {
        showProdutoDropdown && produtosCombo.length > 0 && dropdownPosition && createPortal(
          <div
            className="produto-dropdown-container fixed bg-white border border-slate-300 rounded-lg shadow-2xl overflow-hidden"
            style={{
              top: dropdownPosition.top,
              left: dropdownPosition.left,
              width: dropdownPosition.width,
              zIndex: 99999,
              maxHeight: '320px'
            }}
          >
            <div className="px-3 py-2 bg-slate-100 border-b border-slate-200 sticky top-0">
              <span className="text-xs font-medium text-slate-600">
                {produtosCombo.length} produto(s) encontrado(s)
              </span>
            </div>
            <div className="overflow-y-auto" style={{ maxHeight: '270px' }}>
              {produtosCombo.map((p) => (
                <button
                  key={p.sequenciaDoProduto}
                  type="button"
                  onClick={() => handleSelectProdutoCombo(p)}
                  className="w-full px-3 py-2.5 text-left hover:bg-blue-50 border-b border-slate-100 last:border-0 transition-colors"
                >
                  <div className="text-xs font-medium text-slate-900 truncate">{p.descricao}</div>
                  <div className="flex gap-3 mt-1 text-[10px] text-slate-500">
                    <span className="bg-slate-100 px-1.5 py-0.5 rounded">NCM: {p.ncm}</span>
                    <span className="bg-slate-100 px-1.5 py-0.5 rounded">CFOP: {p.cfop}</span>
                    <span className="font-bold text-green-600">R$ {p.precoVenda.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                  </div>
                </button>
              ))}
            </div>
          </div>,
          document.body
        )
      }
    </div >
  );
}
