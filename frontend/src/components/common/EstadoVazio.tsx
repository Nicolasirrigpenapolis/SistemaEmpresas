import { Inbox, SearchX } from 'lucide-react';
import type { LucideIcon } from 'lucide-react';

// ============================================================================
// TIPOS
// ============================================================================
export interface EstadoVazioProps {
  /** Título principal */
  titulo?: string;
  /** Descrição/mensagem */
  descricao?: string;
  /** Ícone customizado */
  icone?: LucideIcon;
  /** Se é resultado de uma busca */
  tipoBusca?: boolean;
  /** Ação opcional (botão) */
  acao?: {
    texto: string;
    onClick: () => void;
  };
}

// ============================================================================
// COMPONENTE
// ============================================================================
export function EstadoVazio({
  titulo,
  descricao,
  icone,
  tipoBusca = false,
  acao,
}: EstadoVazioProps) {
  const Icone = icone || (tipoBusca ? SearchX : Inbox);
  const tituloFinal = titulo || (tipoBusca ? 'Nenhum resultado encontrado' : 'Nenhum registro');
  const descricaoFinal = descricao || (tipoBusca 
    ? 'Tente ajustar os filtros ou termo de busca'
    : 'Não há dados para exibir no momento'
  );

  return (
    <div className="flex flex-col items-center justify-center py-12 px-4">
      <div className="p-4 bg-gray-100 rounded-full mb-4">
        <Icone className="w-8 h-8 text-gray-400" />
      </div>
      <h3 className="text-lg font-medium text-[var(--text)] mb-1">{tituloFinal}</h3>
      <p className="text-sm text-[var(--text-muted)] text-center max-w-sm">{descricaoFinal}</p>
      
      {acao && (
        <button
          onClick={acao.onClick}
          className="mt-4 px-4 py-2 text-sm font-medium text-blue-600 hover:text-blue-700 hover:bg-blue-50 rounded-lg transition-colors"
        >
          {acao.texto}
        </button>
      )}
    </div>
  );
}

export default EstadoVazio;
