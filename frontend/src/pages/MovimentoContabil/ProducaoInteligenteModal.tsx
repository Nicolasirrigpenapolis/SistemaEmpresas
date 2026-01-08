import { useState } from 'react';
import { 
  X, 
  Loader2, 
  AlertTriangle, 
  CheckCircle2, 
  ChevronRight, 
  Package,
  Factory,
  Zap,
  AlertCircle,
  Eye
} from 'lucide-react';
import { movimentoContabilService } from '../../services/MovimentoContabil/movimentoContabilService';
import type { 
  VerificacaoProducaoResultDto,
  ProducaoCascataRequestDto,
  ProducaoCascataResultDto
} from '../../types/Estoque/movimentoContabil';

interface ProducaoInteligenteModalProps {
  itemId: number;
  itemDescricao: string;
  ehConjunto: boolean;
  sequenciaDoGeral: number;
  onClose: () => void;
  onSuccess: () => void;
}

type Step = 'quantidade' | 'verificando' | 'resultado' | 'executando' | 'sucesso';

export function ProducaoInteligenteModal({ 
  itemId, 
  itemDescricao, 
  ehConjunto, 
  sequenciaDoGeral,
  onClose,
  onSuccess 
}: ProducaoInteligenteModalProps) {
  const [step, setStep] = useState<Step>('quantidade');
  const [quantidade, setQuantidade] = useState<string>('1');
  const [documento, setDocumento] = useState<string>('');
  const [observacao, setObservacao] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [verificacao, setVerificacao] = useState<VerificacaoProducaoResultDto | null>(null);
  const [resultado, setResultado] = useState<ProducaoCascataResultDto | null>(null);
  const [expandedPlan, setExpandedPlan] = useState<number[]>([]);

  const verificarViabilidade = async () => {
    const qtd = parseFloat(quantidade);
    if (isNaN(qtd) || qtd <= 0) {
      setError('Informe uma quantidade válida.');
      return;
    }

    try {
      setError(null);
      setStep('verificando');
      
      const result = await movimentoContabilService.verificarViabilidadeProducao(
        itemId, 
        qtd, 
        ehConjunto
      );
      
      setVerificacao(result);
      setStep('resultado');
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao verificar viabilidade.');
      setStep('quantidade');
    }
  };

  const executarProducao = async () => {
    if (!verificacao) return;

    const request: ProducaoCascataRequestDto = {
      sequenciaDoProdutoOuConjunto: itemId,
      quantidade: parseFloat(quantidade),
      ehConjunto,
      sequenciaDoGeral,
      documento: documento || undefined,
      observacao: observacao || undefined,
      executarPlanoCompleto: true
    };

    try {
      setError(null);
      setStep('executando');
      
      const result = await movimentoContabilService.executarProducaoCascata(request);
      
      setResultado(result);
      setStep('sucesso');
    } catch (err: any) {
      setError(err.response?.data?.mensagem || 'Erro ao executar produção.');
      setStep('resultado');
    }
  };

  const toggleExpandPlan = (ordem: number) => {
    setExpandedPlan(prev => 
      prev.includes(ordem) 
        ? prev.filter(o => o !== ordem) 
        : [...prev, ordem]
    );
  };

  const renderQuantidadeStep = () => (
    <div className="space-y-4">
      <div className="bg-blue-50 p-4 rounded-lg border border-blue-200">
        <div className="flex items-center gap-2 text-blue-800 font-semibold mb-2">
          <Factory className="w-5 h-5" />
          <span>Produção Inteligente</span>
        </div>
        <p className="text-sm text-blue-700">
          Este recurso verifica automaticamente os componentes necessários e, se faltarem, 
          cria os movimentos de produção em cascata (dos mais básicos para os mais complexos).
        </p>
      </div>

      <div className="bg-gray-50 p-4 rounded-lg border border-gray-200">
        <div className="flex items-center gap-3 mb-4">
          <Package className={`w-10 h-10 ${ehConjunto ? 'text-purple-500' : 'text-blue-500'}`} />
          <div>
            <p className="text-xs text-gray-500">{ehConjunto ? 'Conjunto' : 'Produto'}</p>
            <p className="font-semibold text-gray-900">{itemDescricao}</p>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Quantidade a Produzir
            </label>
            <input
              type="number"
              className="w-full p-2 border border-gray-300 rounded-md text-lg font-bold text-center"
              value={quantidade}
              onChange={(e) => setQuantidade(e.target.value)}
              min="0.0001"
              step="0.0001"
              autoFocus
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Documento (opcional)
            </label>
            <input
              type="text"
              className="w-full p-2 border border-gray-300 rounded-md"
              placeholder="NF, OP, etc."
              value={documento}
              onChange={(e) => setDocumento(e.target.value)}
            />
          </div>
        </div>

        <div className="mt-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Observação (opcional)
          </label>
          <textarea
            className="w-full p-2 border border-gray-300 rounded-md h-16"
            placeholder="Observação adicional..."
            value={observacao}
            onChange={(e) => setObservacao(e.target.value)}
          />
        </div>
      </div>

      {error && (
        <div className="p-3 bg-red-50 border border-red-200 text-red-700 rounded-md text-sm flex items-center gap-2">
          <AlertCircle className="w-4 h-4" />
          {error}
        </div>
      )}

      <div className="flex justify-end gap-3">
        <button
          onClick={onClose}
          className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50"
        >
          Cancelar
        </button>
        <button
          onClick={verificarViabilidade}
          className="px-6 py-2 text-sm font-bold text-white bg-blue-600 rounded-md hover:bg-blue-700 flex items-center gap-2"
        >
          <Eye className="w-4 h-4" />
          Verificar Viabilidade
        </button>
      </div>
    </div>
  );

  const renderVerificandoStep = () => (
    <div className="flex flex-col items-center justify-center py-12">
      <Loader2 className="w-16 h-16 text-blue-600 animate-spin mb-4" />
      <p className="text-lg font-semibold text-gray-700">Verificando viabilidade...</p>
      <p className="text-sm text-gray-500 mt-2">Analisando componentes e estoque disponível</p>
    </div>
  );

  const renderResultadoStep = () => {
    if (!verificacao) return null;

    return (
      <div className="space-y-4">
        {/* Status da Verificação */}
        <div className={`p-4 rounded-lg border ${
          verificacao.podeProduzir 
            ? 'bg-green-50 border-green-200' 
            : 'bg-yellow-50 border-yellow-200'
        }`}>
          <div className="flex items-center gap-2">
            {verificacao.podeProduzir ? (
              <CheckCircle2 className="w-6 h-6 text-green-600" />
            ) : (
              <AlertTriangle className="w-6 h-6 text-yellow-600" />
            )}
            <span className={`font-bold ${verificacao.podeProduzir ? 'text-green-800' : 'text-yellow-800'}`}>
              {verificacao.mensagem}
            </span>
          </div>
        </div>

        {/* Componentes Faltantes */}
        {verificacao.componentesFaltantes && verificacao.componentesFaltantes.length > 0 && (
          <div className="bg-red-50 p-4 rounded-lg border border-red-200">
            <h4 className="font-bold text-red-800 mb-3 flex items-center gap-2">
              <AlertCircle className="w-5 h-5" />
              Componentes Faltantes ({verificacao.componentesFaltantes.length})
            </h4>
            <div className="space-y-2 max-h-32 overflow-auto">
              {verificacao.componentesFaltantes.map((comp, idx) => (
                <div key={idx} className="flex justify-between items-center bg-white p-2 rounded border border-red-100">
                  <span className="text-sm font-medium text-gray-800">{comp.descricao}</span>
                  <div className="text-right text-sm">
                    <span className="text-red-600 font-bold">Falta: {comp.falta.toFixed(4)}</span>
                    <span className="text-gray-500 ml-2">(Estoque: {comp.estoqueDisponivel.toFixed(4)})</span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* Plano de Produção Sugerido */}
        {verificacao.planoProducaoCascata && verificacao.planoProducaoCascata.length > 0 && (
          <div className="bg-purple-50 p-4 rounded-lg border border-purple-200">
            <h4 className="font-bold text-purple-800 mb-3 flex items-center gap-2">
              <Zap className="w-5 h-5" />
              Plano de Produção em Cascata ({verificacao.planoProducaoCascata.length} etapas)
            </h4>
            <div className="space-y-2 max-h-64 overflow-auto">
              {verificacao.planoProducaoCascata.map((item) => (
                <div key={item.ordem} className="bg-white rounded border border-purple-100">
                  <button
                    onClick={() => toggleExpandPlan(item.ordem)}
                    className="w-full flex justify-between items-center p-3 text-left hover:bg-purple-50"
                  >
                    <div className="flex items-center gap-3">
                      <span className="bg-purple-600 text-white text-xs font-bold w-6 h-6 rounded-full flex items-center justify-center">
                        {item.ordem}
                      </span>
                      <span className="font-medium text-gray-800">{item.descricao}</span>
                      <span className={`text-xs px-2 py-0.5 rounded ${item.ehConjunto ? 'bg-purple-100 text-purple-700' : 'bg-blue-100 text-blue-700'}`}>
                        {item.ehConjunto ? 'Conjunto' : 'Produto'}
                      </span>
                    </div>
                    <div className="flex items-center gap-3">
                      <span className="text-sm font-bold text-purple-600">Qtd: {item.quantidadeAProduzir.toFixed(4)}</span>
                      <ChevronRight className={`w-5 h-5 text-gray-400 transition-transform ${expandedPlan.includes(item.ordem) ? 'rotate-90' : ''}`} />
                    </div>
                  </button>
                </div>
              ))}
            </div>
          </div>
        )}

        {error && (
          <div className="p-3 bg-red-50 border border-red-200 text-red-700 rounded-md text-sm flex items-center gap-2">
            <AlertCircle className="w-4 h-4" />
            {error}
          </div>
        )}

        <div className="flex justify-between items-center pt-4 border-t">
          <button
            onClick={() => {
              setStep('quantidade');
              setVerificacao(null);
              setError(null);
            }}
            className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50"
          >
            ← Voltar
          </button>
          
          <div className="flex gap-3">
            <button
              onClick={onClose}
              className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50"
            >
              Cancelar
            </button>
            {verificacao.planoProducaoCascata && verificacao.planoProducaoCascata.length > 0 && (
              <button
                onClick={executarProducao}
                className="px-6 py-2 text-sm font-bold text-white bg-green-600 rounded-md hover:bg-green-700 flex items-center gap-2"
              >
                <Zap className="w-4 h-4" />
                Executar Produção em Cascata
              </button>
            )}
          </div>
        </div>
      </div>
    );
  };

  const renderExecutandoStep = () => (
    <div className="flex flex-col items-center justify-center py-12">
      <Loader2 className="w-16 h-16 text-green-600 animate-spin mb-4" />
      <p className="text-lg font-semibold text-gray-700">Executando produção em cascata...</p>
      <p className="text-sm text-gray-500 mt-2">Criando movimentos de produção automaticamente</p>
    </div>
  );

  const renderSucessoStep = () => {
    if (!resultado) return null;

    return (
      <div className="space-y-4">
        <div className={`p-6 rounded-lg border text-center ${
          resultado.sucesso 
            ? 'bg-green-50 border-green-200' 
            : 'bg-red-50 border-red-200'
        }`}>
          {resultado.sucesso ? (
            <CheckCircle2 className="w-16 h-16 text-green-600 mx-auto mb-4" />
          ) : (
            <AlertCircle className="w-16 h-16 text-red-600 mx-auto mb-4" />
          )}
          <p className={`text-xl font-bold ${resultado.sucesso ? 'text-green-800' : 'text-red-800'}`}>
            {resultado.mensagem}
          </p>
        </div>

        {resultado.sucesso && resultado.movimentosGerados.length > 0 && (
          <div className="bg-gray-50 p-4 rounded-lg border border-gray-200">
            <h4 className="font-bold text-gray-700 mb-3">
              Movimentos Gerados ({resultado.movimentosGerados.length})
            </h4>
            <div className="space-y-2 max-h-48 overflow-auto">
              {resultado.movimentosGerados.map((mov) => (
                <div key={mov.sequenciaDoMovimento} className="flex justify-between items-center bg-white p-3 rounded border">
                  <div>
                    <span className="text-xs text-gray-500">#{mov.sequenciaDoMovimento}</span>
                    <p className="font-medium text-gray-800">{mov.descricao}</p>
                    <span className={`text-xs px-2 py-0.5 rounded ${
                      mov.tipo === 'Entrada' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                    }`}>
                      {mov.tipo}
                    </span>
                  </div>
                  <div className="text-right">
                    <p className="font-mono text-sm">Qtd: {mov.quantidade.toFixed(4)}</p>
                    <p className="font-mono text-sm font-bold text-blue-600">
                      {mov.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                    </p>
                  </div>
                </div>
              ))}
            </div>
            <div className="mt-4 pt-3 border-t flex justify-between items-center text-sm">
              <span className="text-gray-600">Total Produzido:</span>
              <span className="font-bold text-lg text-green-600">
                {resultado.valorTotalProduzido.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
              </span>
            </div>
          </div>
        )}

        <div className="flex justify-end pt-4 border-t">
          <button
            onClick={() => {
              onSuccess();
              onClose();
            }}
            className="px-6 py-2 text-sm font-bold text-white bg-blue-600 rounded-md hover:bg-blue-700"
          >
            Concluir
          </button>
        </div>
      </div>
    );
  };

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center bg-black/50 backdrop-blur-sm">
      <div className="bg-white rounded-lg shadow-2xl w-full max-w-2xl max-h-[90vh] flex flex-col">
        {/* Header */}
        <div className="px-6 py-4 border-b border-gray-200 flex items-center justify-between bg-gradient-to-r from-purple-600 to-blue-600 rounded-t-lg">
          <h2 className="text-lg font-bold text-white flex items-center gap-2">
            <Zap className="w-5 h-5" />
            Produção Inteligente (Cascata)
          </h2>
          <button onClick={onClose} className="text-white/80 hover:text-white p-1">
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Content */}
        <div className="flex-1 overflow-y-auto p-6">
          {step === 'quantidade' && renderQuantidadeStep()}
          {step === 'verificando' && renderVerificandoStep()}
          {step === 'resultado' && renderResultadoStep()}
          {step === 'executando' && renderExecutandoStep()}
          {step === 'sucesso' && renderSucessoStep()}
        </div>
      </div>
    </div>
  );
}
