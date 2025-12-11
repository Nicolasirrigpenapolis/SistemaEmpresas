import { AlertTriangle, Loader2, X } from 'lucide-react';

export interface ModalConfirmacaoProps {
  aberto: boolean;
  titulo?: string;
  mensagem: string;
  nomeItem?: string;
  textoBotaoConfirmar?: string;
  textoBotaoCancelar?: string;
  processando?: boolean;
  variante?: 'danger' | 'warning';
  onConfirmar: () => void;
  onCancelar: () => void;
}

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
    danger: 'bg-red-600 hover:bg-red-700 shadow-lg shadow-red-600/20',
    warning: 'bg-amber-600 hover:bg-amber-700 shadow-lg shadow-amber-600/20',
  };

  const coresIcone = {
    danger: 'text-red-600 bg-red-100 ring-red-500/20',
    warning: 'text-amber-600 bg-amber-100 ring-amber-500/20',
  };

  return (
    <div className="fixed inset-0 z-[100] overflow-y-auto">
      {/* Overlay */}
      <div
        className="fixed inset-0 bg-background/80 backdrop-blur-sm transition-opacity animate-fade-in"
        onClick={!processando ? onCancelar : undefined}
      />

      {/* Modal */}
      <div className="flex min-h-full items-center justify-center p-4">
        <div className="relative bg-surface rounded-2xl shadow-2xl max-w-md w-full transform transition-all animate-scale-in border border-border">
          {/* Botão Fechar */}
          <button
            onClick={onCancelar}
            disabled={processando}
            className="absolute top-4 right-4 p-2 rounded-lg text-muted-foreground hover:bg-surface-hover hover:text-primary transition-colors disabled:opacity-50"
          >
            <X className="w-5 h-5" />
          </button>

          {/* Conteúdo */}
          <div className="p-8">
            {/* Ícone */}
            <div className={`mx-auto w-16 h-16 rounded-2xl flex items-center justify-center ring-4 ${coresIcone[variante]} mb-6`}>
              <AlertTriangle className="w-8 h-8" />
            </div>

            {/* Título */}
            <h3 className="text-xl font-bold text-primary text-center mb-2">
              {titulo}
            </h3>

            {/* Mensagem */}
            <p className="text-muted-foreground text-center leading-relaxed">
              {mensagem}
            </p>

            {/* Nome do Item */}
            {nomeItem && (
              <div className="mt-4 p-4 bg-surface-active/30 rounded-xl border border-border">
                <p className="text-sm font-semibold text-primary text-center truncate">
                  {nomeItem}
                </p>
              </div>
            )}

            {/* Botões */}
            <div className="mt-8 flex gap-4">
              <button
                onClick={onCancelar}
                disabled={processando}
                className="flex-1 px-4 py-3 text-sm font-bold text-muted-foreground bg-surface border border-border rounded-xl hover:bg-surface-hover hover:text-primary transition-all disabled:opacity-50"
              >
                {textoBotaoCancelar}
              </button>
              <button
                onClick={onConfirmar}
                disabled={processando}
                className={`flex-1 px-4 py-3 text-sm font-bold text-white rounded-xl transition-all active:scale-95 disabled:opacity-50 disabled:active:scale-100 flex items-center justify-center gap-2 ${coresBotao[variante]}`}
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
