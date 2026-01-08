import React, { useState, useCallback, useRef } from 'react';
import { 
  Upload, CheckCircle, AlertCircle, ArrowRight, Package, 
  Link2, AlertTriangle, Check, Loader2, FileText, Building2, Calendar, Hash
} from 'lucide-react';
import { SeletorComBusca } from '../../components/SeletorComBusca';
import { 
  AlertaErro, 
  AlertaSucesso, 
  CabecalhoPagina
} from '../../components/common';
import { 
  EntradaEstoqueService,
  type EntradaXmlResultDto, 
  type PedidoCompraPendenteDto,
  type PedidoCompraComItensPendentesDto,
  type NFeItemDto,
  type ProdutoBuscaDto,
  type ItemEntradaDto,
  type ConfirmarEntradaRequest
} from '../../services/Estoque/EntradaEstoqueService';

type Etapa = 'upload' | 'conferencia' | 'confirmacao';

const EntradaEstoquePage: React.FC = () => {
  // Estados principais
  const [etapa, setEtapa] = useState<Etapa>('upload');
  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState<string | null>(null);
  const [sucesso, setSucesso] = useState<string | null>(null);
  const [chaveAcesso, setChaveAcesso] = useState('');

  // Estados de busca
  const [buscandoPedidos, setBuscandoPedidos] = useState(false);
  const [buscandoProdutos, setBuscandoProdutos] = useState(false);

  // Refs para debounce
  const searchTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  // Função para formatar a chave de acesso (44 dígitos) com espaços a cada 4
  const formatarChaveAcesso = (valor: string) => {
    const apenasNumeros = valor.replace(/\D/g, '').substring(0, 44);
    return apenasNumeros.replace(/(\d{4})(?=\d)/g, '$1 ');
  };

  // Dados do XML
  const [xmlResult, setXmlResult] = useState<EntradaXmlResultDto | null>(null);
  const [itensXml, setItensXml] = useState<NFeItemDto[]>([]);

  // Dados do Pedido
  const [pedidoSelecionado, setPedidoSelecionado] = useState<PedidoCompraPendenteDto | null>(null);
  const [pedidoDetalhes, setPedidoDetalhes] = useState<PedidoCompraComItensPendentesDto | null>(null);
  const [pedidosSugeridos, setPedidosSugeridos] = useState<PedidoCompraPendenteDto[]>([]);

  // Modal de vinculação
  const [produtosSugeridos, setProdutosSugeridos] = useState<ProdutoBuscaDto[]>([]);

  // Upload
  const [dragActive, setDragActive] = useState(false);

  // Resultado final
  const [resultadoEntrada, setResultadoEntrada] = useState<{ sucesso: boolean; mensagem: string; sequencia?: number } | null>(null);

  // === FUNÇÕES DE UPLOAD ===
  const handleFileUpload = async (file: File) => {
    if (!file.name.toLowerCase().endsWith('.xml')) {
      setErro('Por favor, selecione um arquivo XML de NFe.');
      return;
    }

    setLoading(true);
    setErro(null);

    try {
      const result = await EntradaEstoqueService.uploadXml(file);
      setXmlResult(result);
      setItensXml(result.nfeData.itens);
      
      if (result.notaJaImportada) {
        setErro('Esta nota já foi importada anteriormente!');
      }
      
      setEtapa('conferencia');
    } catch (error: any) {
      setErro(error.response?.data || 'Erro ao processar o XML.');
    } finally {
      setLoading(false);
    }
  };

  const handleBuscarSefaz = async () => {
    if (!chaveAcesso || chaveAcesso.length !== 44) {
      setErro('A chave de acesso deve conter 44 dígitos.');
      return;
    }

    setLoading(true);
    setErro(null);

    try {
      const result = await EntradaEstoqueService.buscarSefaz(chaveAcesso);
      setXmlResult(result);
      setItensXml(result.nfeData.itens);
      
      if (result.notaJaImportada) {
        setErro('Esta nota já foi importada anteriormente!');
      }
      
      setEtapa('conferencia');
    } catch (error: any) {
      setErro(error.response?.data?.message || 'Erro ao buscar NFe na SEFAZ. Verifique a chave e o certificado digital.');
    } finally {
      setLoading(false);
    }
  };

  const handleDrag = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(e.type === "dragenter" || e.type === "dragover");
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);
    if (e.dataTransfer.files?.[0]) handleFileUpload(e.dataTransfer.files[0]);
  };

  // === FUNÇÕES DE BUSCA DE PEDIDO ===
  const buscarPedidos = useCallback(async (termo: string) => {
    if (searchTimeoutRef.current) clearTimeout(searchTimeoutRef.current);

    if (termo.length < 1) {
      setPedidosSugeridos([]);
      setBuscandoPedidos(false);
      return;
    }
    
    setBuscandoPedidos(true);
    searchTimeoutRef.current = setTimeout(async () => {
      try {
        const pedidos = await EntradaEstoqueService.buscarPedidos(termo);
        setPedidosSugeridos(pedidos);
      } catch (error) {
        console.error('Erro ao buscar pedidos:', error);
      } finally {
        setBuscandoPedidos(false);
      }
    }, 500);
  }, []);

  const selecionarPedido = async (pedido: PedidoCompraPendenteDto) => {
    setPedidoSelecionado(pedido);

    setLoading(true);
    try {
      const detalhes = await EntradaEstoqueService.obterItensPedido(pedido.idDoPedido);
      setPedidoDetalhes(detalhes);

      // Tentar vincular itens do XML com itens do pedido automaticamente
      const novosItens = [...itensXml];
      let vinculados = 0;

      novosItens.forEach((item, idx) => {
        if (!item.produtoIdSistema) {
          // Tentar encontrar por descrição aproximada ou se houver apenas um item pendente com valor similar
          const match = detalhes.itens.find(ip => 
            ip.descricaoProduto.toLowerCase().includes(item.descricaoProdutoFornecedor.toLowerCase().substring(0, 10)) ||
            (Math.abs(ip.vrUnitario - item.valorUnitario) < 0.01 && detalhes.itens.length === 1)
          );

          if (match) {
            novosItens[idx] = {
              ...item,
              produtoIdSistema: match.idDoProduto,
              nomeProdutoSistema: match.descricaoProduto
            };
            vinculados++;
          }
        }
      });

      if (vinculados > 0) {
        setItensXml(novosItens);
        setSucesso(`${vinculados} itens foram vinculados automaticamente ao pedido.`);
      }
    } catch (error: any) {
      setErro('Erro ao carregar detalhes do pedido.');
    } finally {
      setLoading(false);
    }
  };

  // === FUNÇÕES DE VINCULAÇÃO ===
  const buscarProdutos = useCallback(async (termo: string) => {
    if (searchTimeoutRef.current) clearTimeout(searchTimeoutRef.current);

    if (termo.length < 1) {
      setProdutosSugeridos([]);
      setBuscandoProdutos(false);
      return;
    }
    
    setBuscandoProdutos(true);
    searchTimeoutRef.current = setTimeout(async () => {
      try {
        const produtos = await EntradaEstoqueService.buscarProduto(termo);
        setProdutosSugeridos(produtos);
      } catch (error) {
        console.error('Erro ao buscar produtos:', error);
      } finally {
        setBuscandoProdutos(false);
      }
    }, 500);
  }, []);

  const vincularProduto = (indexItem: number, produto: ProdutoBuscaDto) => {
    const novosItens = [...itensXml];
    novosItens[indexItem] = {
      ...novosItens[indexItem],
      produtoIdSistema: produto.sequenciaDoProduto,
      nomeProdutoSistema: produto.descricao,
      margemDeLucro: produto.margemDeLucro
    };
    setItensXml(novosItens);
  };

  // === CONFIRMAR ENTRADA ===
  const confirmarEntrada = async () => {
    if (!xmlResult) return;

    if (!pedidoSelecionado) {
      setErro('O Pedido de Compra é obrigatório para realizar a entrada.');
      return;
    }

    const itensParaEntrada: ItemEntradaDto[] = itensXml
      .filter(item => item.produtoIdSistema)
      .map(item => {
        const itemPedido = pedidoDetalhes?.itens.find(ip => ip.idDoProduto === item.produtoIdSistema);
        return {
          produtoId: item.produtoIdSistema!,
          codigoProdutoFornecedor: item.codigoProdutoFornecedor,
          sequenciaItemPedido: itemPedido?.sequenciaDoItem,
          quantidade: item.quantidade,
          valorUnitario: item.valorUnitario,
          valorTotal: item.valorTotal,
          aliquotaIcms: item.aliquotaIcms,
          valorIcms: item.valorIcms,
          aliquotaIpi: 0,
          valorIpi: item.valorIpi,
          receber: true
        };
      });

    if (itensParaEntrada.length === 0) {
      setErro('Vincule pelo menos um item do XML a um produto do sistema.');
      return;
    }

    const request: ConfirmarEntradaRequest = {
      numeroNota: xmlResult.nfeData.numeroNota,
      serie: xmlResult.nfeData.serie,
      chaveAcesso: xmlResult.nfeData.chaveAcesso,
      dataEntrada: new Date().toISOString(),
      fornecedorId: xmlResult.fornecedorId || 0,
      idPedido: pedidoSelecionado?.idDoPedido,
      valorFrete: 0,
      valorDesconto: 0,
      valorTotal: xmlResult.nfeData.valorTotal,
      itens: itensParaEntrada
    };

    setLoading(true);
    setErro(null);

    try {
      const resultado = await EntradaEstoqueService.confirmarEntrada(request);
      setResultadoEntrada({
        sucesso: resultado.sucesso,
        mensagem: resultado.mensagem,
        sequencia: resultado.sequenciaDoMovimento
      });
      setEtapa('confirmacao');
    } catch (error: any) {
      setErro(error.response?.data || 'Erro ao confirmar entrada.');
    } finally {
      setLoading(false);
    }
  };

  // === REINICIAR ===
  const reiniciar = () => {
    setEtapa('upload');
    setXmlResult(null);
    setItensXml([]);
    setPedidoSelecionado(null);
    setPedidoDetalhes(null);
    setResultadoEntrada(null);
    setErro(null);
    setSucesso(null);
  };

  // === RENDER ===
  return (
    <div className="min-h-screen bg-gray-50/30 -m-6 p-6">
      <div className="max-w-[1600px] mx-auto space-y-8">
        <CabecalhoPagina
          titulo="Entrada de Estoque"
          subtitulo="Importe notas fiscais via XML ou Chave de Acesso para dar entrada no estoque de forma automatizada."
          icone={Package}
        />

        {/* Progress Steps */}
        <div className="bg-white p-8 rounded-2xl shadow-sm border border-gray-100">
          <div className="flex items-center justify-between max-w-4xl mx-auto">
            {[
              { key: 'upload', label: 'Origem dos Dados', icon: Upload, desc: 'XML ou Chave' },
              { key: 'conferencia', label: 'Conferência', icon: FileText, desc: 'Vincular Itens' },
              { key: 'confirmacao', label: 'Conclusão', icon: CheckCircle, desc: 'Estoque Atualizado' }
            ].map((step, index) => {
              const isActive = etapa === step.key;
              const isCompleted = ['upload', 'conferencia', 'confirmacao'].indexOf(etapa) > index;
              
              return (
                <React.Fragment key={step.key}>
                  <div className="flex flex-col items-center relative z-10">
                    <div className={`w-14 h-14 rounded-2xl flex items-center justify-center border-2 transition-all duration-500
                      ${isActive ? 'border-blue-600 bg-blue-600 text-white shadow-lg shadow-blue-200 scale-110 rotate-3' : 
                        isCompleted ? 'border-green-500 bg-green-50 text-green-500' : 'border-gray-100 bg-gray-50 text-gray-400'}`}>
                      {isCompleted ? <Check className="w-7 h-7" /> : <step.icon className="w-7 h-7" />}
                    </div>
                    <div className="mt-3 text-center">
                      <span className={`block text-sm font-bold tracking-tight
                        ${isActive ? 'text-blue-600' : isCompleted ? 'text-green-600' : 'text-gray-400'}`}>
                        {step.label}
                      </span>
                      <span className="text-[10px] text-gray-400 font-medium uppercase tracking-wider">{step.desc}</span>
                    </div>
                  </div>
                  {index < 2 && (
                    <div className="flex-1 h-1 mx-4 -mt-10 bg-gray-100 rounded-full overflow-hidden">
                      <div className={`h-full transition-all duration-700 ease-in-out ${isCompleted ? 'bg-green-500 w-full' : 'bg-gray-100 w-0'}`} />
                    </div>
                  )}
                </React.Fragment>
              );
            })}
          </div>
        </div>

        {/* Mensagens */}
        <div className="w-full">
          {erro && <AlertaErro mensagem={erro} onFechar={() => setErro(null)} fechavel className="mb-4 shadow-md animate-in slide-in-from-top-2" />}
          {sucesso && <AlertaSucesso mensagem={sucesso} onFechar={() => setSucesso(null)} fechavel className="mb-4 shadow-md animate-in slide-in-from-top-2" />}
        </div>

        {/* Etapa 1: Upload / Chave de Acesso */}
        {etapa === 'upload' && (
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 animate-in fade-in slide-in-from-bottom-8 duration-700">
            {/* Opção 1: Upload de Arquivo */}
            <div className={`group relative bg-white p-12 rounded-3xl border-2 border-dashed transition-all duration-500 
              ${dragActive ? 'border-blue-500 bg-blue-50/50 ring-8 ring-blue-50' : 'border-gray-200 hover:border-blue-400 hover:shadow-2xl hover:-translate-y-1'}`}>
              <div 
                onDragEnter={handleDrag}
                onDragLeave={handleDrag}
                onDragOver={handleDrag}
                onDrop={handleDrop}
                className="flex flex-col items-center justify-center h-full text-center"
              >
                <div className="w-24 h-24 bg-blue-50 text-blue-600 rounded-3xl flex items-center justify-center mb-8 group-hover:scale-110 group-hover:rotate-6 transition-all duration-500 shadow-sm">
                  <Upload className="w-12 h-12" />
                </div>
                <h3 className="text-2xl font-black text-gray-900 mb-3 tracking-tight">Importar Arquivo XML</h3>
                <p className="text-gray-500 mb-10 max-w-xs text-lg leading-relaxed">Arraste o arquivo .xml da NFe aqui ou clique para selecionar do seu computador.</p>
                
                <input 
                  type="file" 
                  id="xml-upload" 
                  className="hidden" 
                  accept=".xml"
                  onChange={(e) => e.target.files?.[0] && handleFileUpload(e.target.files[0])}
                />
                <label 
                  htmlFor="xml-upload"
                  className="px-10 py-4 bg-blue-600 text-white rounded-2xl font-bold hover:bg-blue-700 cursor-pointer transition-all shadow-xl shadow-blue-100 hover:shadow-blue-200 active:scale-95 flex items-center"
                >
                  <FileText className="w-5 h-5 mr-2" />
                  Selecionar Arquivo
                </label>
              </div>
            </div>

            {/* Opção 2: Chave de Acesso */}
            <div className="bg-white p-12 rounded-3xl border border-gray-100 shadow-sm hover:shadow-2xl hover:-translate-y-1 transition-all duration-500 flex flex-col">
              <div className="w-24 h-24 bg-purple-50 text-purple-600 rounded-3xl flex items-center justify-center mb-8 shadow-sm">
                <Link2 className="w-12 h-12" />
              </div>
              <h3 className="text-2xl font-black text-gray-900 mb-3 tracking-tight">Buscar pela Chave de Acesso</h3>
              <p className="text-gray-500 mb-10 text-lg leading-relaxed">Consulte a nota diretamente da SEFAZ utilizando os 44 dígitos da chave de acesso impressa no DANFE.</p>
              
              <div className="mt-auto space-y-6">
                <div className="space-y-2">
                  <div className="relative group">
                    <input 
                      type="text"
                      maxLength={54}
                      placeholder="0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000"
                      className="w-full p-5 bg-gray-50 border-2 border-gray-100 rounded-2xl font-mono text-lg text-center tracking-normal focus:ring-4 focus:ring-purple-50 focus:bg-white focus:border-purple-500 outline-none transition-all"
                      value={formatarChaveAcesso(chaveAcesso)}
                      onChange={(e) => setChaveAcesso(e.target.value.replace(/\D/g, '').substring(0, 44))}
                    />
                    <div className="absolute inset-y-0 right-4 flex items-center pointer-events-none opacity-0 group-focus-within:opacity-100 transition-opacity">
                      <div className="w-2 h-2 bg-purple-500 rounded-full animate-pulse" />
                    </div>
                  </div>
                  <div className="flex justify-between items-center px-2">
                    <span className="text-[10px] font-bold text-gray-400 uppercase tracking-widest">Chave de Acesso NFe</span>
                    {chaveAcesso.length > 0 && (
                      <span className={`text-xs font-black ${chaveAcesso.length === 44 ? 'text-green-500' : 'text-purple-400'}`}>
                        {chaveAcesso.length}/44
                      </span>
                    )}
                  </div>
                </div>
                
                <button 
                  onClick={handleBuscarSefaz}
                  disabled={loading || chaveAcesso.length !== 44}
                  className="w-full py-5 bg-purple-600 text-white rounded-2xl font-bold hover:bg-purple-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all shadow-xl shadow-purple-100 hover:shadow-purple-200 flex items-center justify-center active:scale-95 text-lg"
                >
                  {loading ? (
                    <Loader2 className="w-7 h-7 animate-spin" />
                  ) : (
                    <>
                      <ArrowRight className="w-6 h-6 mr-2" />
                      Consultar SEFAZ
                    </>
                  )}
                </button>
              </div>
            </div>
          </div>
        )}

        {/* Etapa 2: Conferência */}
        {etapa === 'conferencia' && xmlResult && (
          <div className="space-y-8 animate-in fade-in slide-in-from-right-8 duration-700">
            {/* Resumo da Nota - Layout Moderno */}
            <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
              <div className="bg-gradient-to-r from-gray-50 to-white px-8 py-6 border-b border-gray-100 flex flex-wrap justify-between items-center gap-4">
                <div className="flex items-center space-x-4">
                  <div className="p-3 bg-blue-600 text-white rounded-2xl shadow-lg shadow-blue-100">
                    <FileText className="w-6 h-6" />
                  </div>
                  <div>
                    <h3 className="text-xl font-black text-gray-900 tracking-tight">Conferência da Nota Fiscal</h3>
                    <p className="text-sm text-gray-500 font-medium">Verifique os dados e vincule os produtos ao sistema</p>
                  </div>
                </div>
                <div className="flex items-center gap-3">
                  <div className="flex flex-col items-end">
                    <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Número da Nota</span>
                    <span className="text-lg font-black text-blue-600">{xmlResult.nfeData.numeroNota}</span>
                  </div>
                  <div className="w-px h-8 bg-gray-200 mx-2" />
                  <div className="flex flex-col items-end">
                    <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Série</span>
                    <span className="text-lg font-black text-gray-900">{xmlResult.nfeData.serie}</span>
                  </div>
                </div>
              </div>
              
              <div className="p-8 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
                <div className="bg-gray-50/50 p-5 rounded-2xl border border-gray-100 hover:bg-white hover:shadow-md transition-all duration-300">
                  <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest flex items-center mb-3">
                    <Building2 className="w-3.5 h-3.5 mr-1.5 text-blue-500" /> Fornecedor
                  </span>
                  <p className="font-bold text-gray-900 text-lg truncate" title={xmlResult.nfeData.emitente.nome}>{xmlResult.nfeData.emitente.nome}</p>
                  <p className="text-sm text-gray-500 font-mono mt-1">{xmlResult.nfeData.emitente.cnpj}</p>
                </div>
                
                <div className="bg-gray-50/50 p-5 rounded-2xl border border-gray-100 hover:bg-white hover:shadow-md transition-all duration-300">
                  <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest flex items-center mb-3">
                    <Calendar className="w-3.5 h-3.5 mr-1.5 text-purple-500" /> Data de Emissão
                  </span>
                  <p className="font-bold text-gray-900 text-lg">
                    {new Date(xmlResult.nfeData.dataEmissao).toLocaleDateString('pt-BR', { day: '2-digit', month: 'long', year: 'numeric' })}
                  </p>
                  <p className="text-sm text-gray-500 font-medium mt-1">
                    {new Date(xmlResult.nfeData.dataEmissao).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' })}
                  </p>
                </div>

                <div className="bg-gray-50/50 p-5 rounded-2xl border border-gray-100 hover:bg-white hover:shadow-md transition-all duration-300">
                  <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest flex items-center mb-3">
                    <Hash className="w-3.5 h-3.5 mr-1.5 text-amber-500" /> Chave de Acesso
                  </span>
                  <p className="font-mono text-[10px] text-gray-600 break-all leading-relaxed">
                    {xmlResult.nfeData.chaveAcesso}
                  </p>
                </div>

                <div className="bg-blue-600 p-6 rounded-2xl shadow-xl shadow-blue-100 flex flex-col justify-center">
                  <span className="text-[10px] font-black text-blue-100 uppercase tracking-widest mb-1">Valor Total da Nota</span>
                  <p className="text-3xl font-black text-white">
                    {xmlResult.nfeData.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </p>
                </div>
              </div>

              {/* Resumo Financeiro Detalhado */}
              <div className="px-8 py-4 bg-gray-50 border-t border-gray-100 flex flex-wrap gap-8">
                <div className="flex items-center space-x-3">
                  <div className="w-2 h-2 bg-blue-400 rounded-full" />
                  <span className="text-xs font-bold text-gray-500 uppercase tracking-wider">Produtos:</span>
                  <span className="text-sm font-black text-gray-900">
                    {xmlResult.nfeData.valorProdutos.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </span>
                </div>
                <div className="flex items-center space-x-3">
                  <div className="w-2 h-2 bg-purple-400 rounded-full" />
                  <span className="text-xs font-bold text-gray-500 uppercase tracking-wider">ICMS:</span>
                  <span className="text-sm font-black text-gray-900">
                    {xmlResult.nfeData.valorIcms.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </span>
                </div>
                <div className="flex items-center space-x-3">
                  <div className="w-2 h-2 bg-amber-400 rounded-full" />
                  <span className="text-xs font-bold text-gray-500 uppercase tracking-wider">IPI:</span>
                  <span className="text-sm font-black text-gray-900">
                    {xmlResult.nfeData.valorIpi.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </span>
                </div>
              </div>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-12 gap-8 items-start">
              {/* Coluna da Esquerda: Vínculo com Pedido (Sticky) */}
              <div className="lg:col-span-3 space-y-6 lg:sticky lg:top-6">
                <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8">
                  <div className="flex items-center justify-between mb-8">
                    <h3 className="text-lg font-black text-gray-900 flex items-center tracking-tight">
                      <Link2 className="w-5 h-5 mr-2 text-blue-600" />
                      Pedido de Compra
                    </h3>
                    <span className="px-2 py-1 bg-red-50 text-red-500 text-[10px] font-black uppercase rounded-lg border border-red-100">Obrigatório</span>
                  </div>
                  
                  <SeletorComBusca
                    label="Pedido de Compra"
                    value={pedidoSelecionado?.idDoPedido || 0}
                    descricao={pedidoSelecionado ? `Pedido #${pedidoSelecionado.idDoPedido}` : ''}
                    onSelect={(id) => {
                      const pedido = pedidosSugeridos.find(p => p.idDoPedido === id);
                      if (pedido) selecionarPedido(pedido);
                    }}
                    items={pedidosSugeridos}
                    getItemId={(p) => p.idDoPedido}
                    getItemDescricao={(p) => `Pedido #${p.idDoPedido} - ${p.nomeFornecedor}`}
                    getItemSecundario={(p) => `${p.totalDoPedido.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })} - ${p.qtdeItensPendentes} itens`}
                    onSearch={buscarPedidos}
                    loading={buscandoPedidos}
                    placeholder="Busque por número ou fornecedor..."
                    required
                  />
                  
                  {pedidoSelecionado && (
                    <div className="mt-8 p-6 bg-gradient-to-br from-blue-50/50 to-white rounded-2xl border border-blue-100 shadow-sm space-y-5 animate-in zoom-in-95 duration-300">
                      <div className="flex items-center justify-between">
                        <span className="text-[10px] font-black text-blue-400 uppercase tracking-widest">Detalhes do Pedido</span>
                        <span className="px-3 py-1 bg-blue-600 text-white text-xs font-black rounded-xl shadow-lg shadow-blue-100">#{pedidoSelecionado.idDoPedido}</span>
                      </div>
                      
                      <div className="space-y-4">
                        <div className="flex flex-col">
                          <span className="text-[10px] font-bold text-gray-400 uppercase">Fornecedor</span>
                          <span className="text-sm font-bold text-gray-900 truncate">{pedidoSelecionado.nomeFornecedor}</span>
                        </div>
                        <div className="flex justify-between items-end">
                          <div className="flex flex-col">
                            <span className="text-[10px] font-bold text-gray-400 uppercase">Total</span>
                            <span className="text-lg font-black text-blue-600">
                              {pedidoSelecionado.totalDoPedido.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                            </span>
                          </div>
                          <div className="flex flex-col items-end">
                            <span className="text-[10px] font-bold text-gray-400 uppercase">Pendentes</span>
                            <span className="text-sm font-black text-gray-900">{pedidoSelecionado.qtdeItensPendentes} itens</span>
                          </div>
                        </div>
                      </div>
                    </div>
                  )}

                  {!pedidoSelecionado && (
                    <div className="mt-6 p-5 bg-amber-50 rounded-2xl border border-amber-100 flex items-start animate-pulse">
                      <AlertTriangle className="w-6 h-6 text-amber-500 mr-4 flex-shrink-0 mt-0.5" />
                      <p className="text-sm text-amber-800 font-medium leading-relaxed">
                        Selecione o pedido de compra para vincular os itens e validar as quantidades.
                      </p>
                    </div>
                  )}
                </div>

                {/* Resumo do Vínculo - Card Moderno */}
                <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8">
                  <h3 className="text-lg font-black text-gray-900 mb-6 flex items-center tracking-tight">
                    <CheckCircle className="w-5 h-5 mr-2 text-green-600" />
                    Status do Vínculo
                  </h3>
                  <div className="space-y-5">
                    <div className="flex items-center justify-between p-3 bg-gray-50 rounded-xl">
                      <span className="text-sm font-bold text-gray-500">Total de Itens</span>
                      <span className="text-lg font-black text-gray-900">{itensXml.length}</span>
                    </div>
                    <div className="flex items-center justify-between p-3 bg-green-50 rounded-xl border border-green-100">
                      <span className="text-sm font-bold text-green-700">Vinculados</span>
                      <span className="text-lg font-black text-green-700">{itensXml.filter(i => i.produtoIdSistema).length}</span>
                    </div>
                    <div className="flex items-center justify-between p-3 bg-amber-50 rounded-xl border border-amber-100">
                      <span className="text-sm font-bold text-amber-700">Pendentes</span>
                      <span className="text-lg font-black text-amber-700">{itensXml.filter(i => !i.produtoIdSistema).length}</span>
                    </div>
                    
                    <div className="pt-6 space-y-4">
                      <button 
                        onClick={confirmarEntrada}
                        disabled={loading || !pedidoSelecionado || itensXml.filter(i => i.produtoIdSistema).length === 0}
                        className="w-full py-5 bg-green-600 text-white rounded-2xl font-black hover:bg-green-700 disabled:bg-gray-100 disabled:text-gray-400 disabled:cursor-not-allowed transition-all shadow-xl shadow-green-100 hover:shadow-green-200 flex items-center justify-center active:scale-95 text-lg"
                      >
                        {loading ? <Loader2 className="w-7 h-7 animate-spin" /> : (
                          <>
                            <CheckCircle className="w-6 h-6 mr-2" />
                            Finalizar Entrada
                          </>
                        )}
                      </button>
                      <button 
                        onClick={reiniciar}
                        className="w-full py-4 text-sm text-gray-400 font-black hover:text-red-500 hover:bg-red-50 rounded-2xl transition-all flex items-center justify-center uppercase tracking-widest"
                      >
                        Cancelar Operação
                      </button>
                    </div>
                  </div>
                </div>
              </div>

              {/* Coluna da Direita: Tabela de Itens (Full Space) */}
              <div className="lg:col-span-9">
                <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden flex flex-col min-h-[600px]">
                  <div className="px-8 py-6 border-b border-gray-100 bg-gray-50/50 flex justify-between items-center">
                    <div className="flex items-center space-x-3">
                      <div className="p-2 bg-blue-100 text-blue-600 rounded-xl">
                        <Package className="w-5 h-5" />
                      </div>
                      <h3 className="text-lg font-black text-gray-900 tracking-tight">Itens da Nota Fiscal</h3>
                    </div>
                    <div className="flex items-center space-x-2">
                      <span className="text-xs font-bold text-gray-400 uppercase tracking-widest">Visualizando</span>
                      <span className="px-3 py-1 bg-white border border-gray-200 rounded-lg text-sm font-black text-gray-900">{itensXml.length} itens</span>
                    </div>
                  </div>
                  
                  <div className="flex-1 overflow-x-auto">
                    <table className="min-w-full divide-y divide-gray-100">
                      <thead className="bg-gray-50/80 sticky top-0 z-10 backdrop-blur-sm">
                        <tr>
                          <th className="px-8 py-5 text-left text-[10px] font-black text-gray-400 uppercase tracking-widest">Status</th>
                          <th className="px-8 py-5 text-left text-[10px] font-black text-gray-400 uppercase tracking-widest">Produto (Fornecedor)</th>
                          <th className="px-8 py-5 text-center text-[10px] font-black text-gray-400 uppercase tracking-widest">Qtd / Valores</th>
                          <th className="px-8 py-5 text-center text-[10px] font-black text-gray-400 uppercase tracking-widest">Sugestão Venda</th>
                          <th className="px-8 py-5 text-left text-[10px] font-black text-gray-400 uppercase tracking-widest">Vínculo no Sistema</th>
                        </tr>
                      </thead>
                      <tbody className="bg-white divide-y divide-gray-50">
                        {itensXml.map((item, index) => (
                          <tr key={index} className={`group hover:bg-blue-50/30 transition-all duration-200 ${!item.produtoIdSistema ? 'bg-amber-50/20' : ''}`}>
                            <td className="px-8 py-6 whitespace-nowrap">
                              {item.produtoIdSistema ? (
                                <div className="flex items-center justify-center w-10 h-10 bg-green-100 text-green-600 rounded-2xl shadow-sm group-hover:scale-110 transition-transform">
                                  <Check className="w-5 h-5" />
                                </div>
                              ) : (
                                <div className="flex items-center justify-center w-10 h-10 bg-amber-100 text-amber-500 rounded-2xl shadow-sm animate-pulse group-hover:scale-110 transition-transform">
                                  <AlertTriangle className="w-5 h-5" />
                                </div>
                              )}
                            </td>
                            <td className="px-8 py-6">
                              <div className="flex flex-col max-w-md">
                                <span className="text-sm font-black text-gray-900 leading-tight group-hover:text-blue-600 transition-colors" title={item.descricaoProdutoFornecedor}>
                                  {item.descricaoProdutoFornecedor}
                                </span>
                                <div className="flex items-center gap-3 mt-2">
                                  <span className="px-2 py-0.5 bg-gray-100 text-gray-500 text-[10px] font-black rounded border border-gray-200 uppercase tracking-tighter">REF: {item.codigoProdutoFornecedor}</span>
                                  <span className="text-[10px] font-bold text-gray-400 uppercase tracking-tighter">NCM: {item.ncm}</span>
                                </div>
                              </div>
                            </td>
                            <td className="px-8 py-6 text-center">
                              <div className="flex flex-col items-center">
                                <div className="flex items-baseline gap-1">
                                  <span className="text-lg font-black text-gray-900">{item.quantidade}</span>
                                  <span className="text-[10px] font-black text-gray-400 uppercase">{item.unidadeMedida}</span>
                                </div>
                                <span className="text-[10px] font-bold text-gray-400 uppercase mt-1">
                                  {item.valorUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })} / un
                                </span>
                                <div className="mt-2 px-3 py-1 bg-blue-50 text-blue-700 text-xs font-black rounded-xl border border-blue-100 shadow-sm">
                                  {item.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                                </div>
                              </div>
                            </td>
                            <td className="px-8 py-6 text-center">
                              {item.produtoIdSistema && item.margemDeLucro ? (
                                <div className="flex flex-col items-center bg-green-50/50 p-3 rounded-2xl border border-green-100/50">
                                  <span className="text-lg font-black text-green-600">
                                    {(item.valorUnitario * item.margemDeLucro).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                                  </span>
                                  <div className="flex items-center mt-1">
                                    <span className="text-[10px] font-black text-green-500 uppercase tracking-widest">
                                      Margem: {((item.margemDeLucro - 1) * 100).toFixed(0)}%
                                    </span>
                                  </div>
                                </div>
                              ) : (
                                <div className="flex flex-col items-center opacity-30">
                                  <span className="text-xs font-black text-gray-400 italic">AGUARDANDO</span>
                                  <span className="text-[10px] font-bold text-gray-300 uppercase">VÍNCULO</span>
                                </div>
                              )}
                            </td>
                            <td className="px-8 py-6 min-w-[350px]">
                              <div className="relative">
                                <SeletorComBusca
                                  label="Produto no Sistema"
                                  value={item.produtoIdSistema || 0}
                                  descricao={item.nomeProdutoSistema || ''}
                                  onSelect={(id) => {
                                    const prod = produtosSugeridos.find(p => p.sequenciaDoProduto === id);
                                    if (prod) vincularProduto(index, prod);
                                  }}
                                  items={produtosSugeridos}
                                  getItemId={(p) => p.sequenciaDoProduto}
                                  getItemDescricao={(p) => p.descricao}
                                  getItemSecundario={(p) => `Cód: ${p.sequenciaDoProduto} | ${p.unidadeMedida} | Custo: ${p.valorCusto.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}`}
                                  onSearch={buscarProdutos}
                                  loading={buscandoProdutos}
                                  placeholder="Pesquisar produto no sistema..."
                                />
                                {!item.produtoIdSistema && (
                                  <div className="mt-2 flex items-center text-[10px] font-black text-amber-600 uppercase tracking-widest animate-pulse">
                                    <div className="w-1.5 h-1.5 bg-amber-500 rounded-full mr-2" />
                                    Vínculo Obrigatório
                                  </div>
                                )}
                              </div>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}

        {/* Etapa 3: Conclusão */}
        {etapa === 'confirmacao' && resultadoEntrada && (
          <div className="max-w-4xl mx-auto animate-in zoom-in duration-700">
            <div className="bg-white rounded-[40px] shadow-2xl border border-gray-100 overflow-hidden">
              <div className={`p-16 text-center ${resultadoEntrada.sucesso ? 'bg-gradient-to-b from-green-50/50 to-white' : 'bg-gradient-to-b from-red-50/50 to-white'}`}>
                <div className={`w-32 h-32 mx-auto rounded-[32px] flex items-center justify-center mb-10 shadow-2xl transition-transform hover:scale-110 duration-500
                  ${resultadoEntrada.sucesso ? 'bg-green-500 text-white shadow-green-200 rotate-3' : 'bg-red-500 text-white shadow-red-200'}`}>
                  {resultadoEntrada.sucesso ? <CheckCircle className="w-16 h-16" /> : <AlertCircle className="w-16 h-16" />}
                </div>
                
                <h2 className="text-4xl font-black text-gray-900 mb-6 tracking-tight">
                  {resultadoEntrada.sucesso ? 'Entrada Processada!' : 'Ops! Algo deu errado'}
                </h2>
                <p className="text-xl text-gray-500 mb-12 max-w-lg mx-auto leading-relaxed font-medium">
                  {resultadoEntrada.mensagem}
                </p>
                
                {resultadoEntrada.sucesso && (
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-12">
                    <div className="bg-gray-50 p-8 rounded-3xl border border-gray-100 shadow-sm text-left group hover:bg-white hover:shadow-xl transition-all duration-300">
                      <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest block mb-3">Número do Movimento</span>
                      <div className="flex items-center justify-between">
                        <span className="text-3xl font-black text-gray-900">#{resultadoEntrada.sequencia}</span>
                        <div className="p-3 bg-blue-100 text-blue-600 rounded-2xl group-hover:rotate-12 transition-transform">
                          <Hash className="w-6 h-6" />
                        </div>
                      </div>
                    </div>
                    <div className="bg-gray-50 p-8 rounded-3xl border border-gray-100 shadow-sm text-left group hover:bg-white hover:shadow-xl transition-all duration-300">
                      <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest block mb-3">Nota Fiscal</span>
                      <div className="flex items-center justify-between">
                        <span className="text-3xl font-black text-gray-900">{xmlResult?.nfeData.numeroNota}</span>
                        <div className="p-3 bg-purple-100 text-purple-600 rounded-2xl group-hover:rotate-12 transition-transform">
                          <FileText className="w-6 h-6" />
                        </div>
                      </div>
                    </div>
                  </div>
                )}

                <div className="flex flex-col sm:flex-row gap-6 justify-center">
                  <button 
                    onClick={reiniciar}
                    className="px-12 py-5 bg-blue-600 text-white rounded-2xl font-black hover:bg-blue-700 transition-all shadow-xl shadow-blue-100 hover:shadow-blue-200 active:scale-95 flex items-center justify-center text-lg"
                  >
                    <Package className="w-6 h-6 mr-3" />
                    Nova Entrada
                  </button>
                  <button 
                    onClick={() => window.location.href = '/estoque/movimentacoes'}
                    className="px-12 py-5 bg-white text-gray-900 border-2 border-gray-100 rounded-2xl font-black hover:bg-gray-50 transition-all active:scale-95 flex items-center justify-center text-lg"
                  >
                    <FileText className="w-6 h-6 mr-3" />
                    Ver Movimentações
                  </button>
                </div>
              </div>
              
              {resultadoEntrada.sucesso && (
                <div className="bg-gray-50/80 p-8 border-t border-gray-100 text-center backdrop-blur-sm">
                  <div className="flex items-center justify-center space-x-3 text-gray-500">
                    <Check className="w-5 h-5 text-green-500" />
                    <p className="text-sm font-bold">
                      O estoque foi atualizado e os preços de custo foram recalculados automaticamente.
                    </p>
                  </div>
                </div>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default EntradaEstoquePage;
