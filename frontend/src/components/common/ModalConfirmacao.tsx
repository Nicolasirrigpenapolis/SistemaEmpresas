import { AlertTriangle, Loader2, X } from 'lucide-react';

// ============================================================================
// TIPOS
// ============================================================================
export interface ModalConfirmacaoProps {
  /** Se o modal está aberto */
  aberto: boolean;
  /** Título do modal */
  titulo?: string;
  /** Mensagem de confirmação */
  mensagem: string;
  /** Nome do item sendo excluído (aparece em destaque) */
  nomeItem?: string;
  /** Texto do botão de confirmar */
  textoBotaoConfirmar?: string;
  /** Texto do botão de cancelar */
  textoBotaoCancelar?: string;
  /** Se está processando a ação */
  processando?: boolean;
  /** Variante visual (danger = vermelho, warning = amarelo) */
  variante?: 'danger' | 'warning';
  /** Callback ao confirmar */
  onConfirmar: () => void;
  /** Callback ao cancelar/fechar */
  onCancelar: () => void;
}

// ============================================================================
// COMPONENTE
// ============================================================================
export function ModalConfirmacao({
  aberto,
  titulo = 'Confirmar Exclusão',
  mensagem,
  nomeItem,
  textoBotaoConfirmar = 'Excluir',
  textoBotaoCancelar = 'Cancelar',
  processando = false,
  variante = 'danger',
  onConfirmar,
  onCancelar,
}: ModalConfirmacaoProps) {
  if (!aberto) return null;

  const coresBotao = {
    danger: 'bg-red-600 hover:bg-red-700 focus:ring-red-500',
    warning: 'bg-amber-600 hover:bg-amber-700 focus:ring-amber-500',
  };

  const coresIcone = {
    danger: 'text-red-600 bg-red-100',
    warning: 'text-amber-600 bg-amber-100',
  };

  return (
    <div className="fixed inset-0 z-50 overflow-y-auto">
      {/* Overlay */}
      <div 
        className="fixed inset-0 bg-black/50 transition-opacity"
        onClick={!processando ? onCancelar : undefined}
      />
      
      {/* Modal */}
      <div className="flex min-h-full items-center justify-center p-4">
        <div className="relative bg-[var(--surface)] rounded-xl shadow-2xl max-w-md w-full transform transition-all">
          {/* Botão Fechar */}
          <button
            onClick={onCancelar}
            disabled={processando}
            className="absolute top-4 right-4 text-gray-400 hover:text-[var(--text-muted)] transition-colors disabled:opacity-50"
          >
            <X className="w-5 h-5" />
          </button>

          {/* Conteúdo */}
          <div className="p-6">
            {/* Ícone */}
            <div className={`mx-auto w-12 h-12 rounded-full flex items-center justify-center ${coresIcone[variante]}`}>
              <AlertTriangle className="w-6 h-6" />
            </div>

            {/* Título */}
            <h3 className="mt-4 text-lg font-semibold text-[var(--text)] text-center">
              {titulo}
            </h3>

            {/* Mensagem */}
            <p className="mt-2 text-sm text-[var(--text-muted)] text-center">
              {mensagem}
            </p>

            {/* Nome do Item */}
            {nomeItem && (
              <div className="mt-3 p-3 bg-[var(--surface-muted)] rounded-lg border border-[var(--border)]">
                <p className="text-sm font-medium text-[var(--text)] text-center truncate">
                  {nomeItem}
                </p>
              </div>
            )}

            {/* Botões */}
            <div className="mt-6 flex gap-3">
              <button
                onClick={onCancelar}
                disabled={processando}
                className="flex-1 px-4 py-2.5 text-sm font-medium text-gray-700 bg-[var(--surface)] border border-gray-300 rounded-lg hover:bg-[var(--surface-muted)] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 disabled:opacity-50 transition-colors"
              >
                {textoBotaoCancelar}
              </button>
              <button
                onClick={onConfirmar}
                disabled={processando}
                className={`flex-1 px-4 py-2.5 text-sm font-medium text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 transition-colors flex items-center justify-center gap-2 ${coresBotao[variante]}`}
              >
                {processando ? (
                  <>
                    <Loader2 className="w-4 h-4 animate-spin" />
                    Processando...
                  </>
                ) : (
                  textoBotaoConfirmar
                )}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ModalConfirmacao;
