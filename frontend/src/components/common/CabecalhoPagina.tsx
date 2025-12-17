import type { ReactNode } from 'react';
import type { LucideIcon } from 'lucide-react';

// ============================================================================
// TIPOS
// ============================================================================
export interface CabecalhoPaginaProps {
  /** Título da página */
  titulo: string;
  /** Subtítulo/descrição da página */
  subtitulo?: string;
  /** Ícone da página (componente Lucide) */
  icone?: LucideIcon;
  /** Ações do lado direito (botões) */
  acoes?: ReactNode;
  /** Conteúdo adicional abaixo do título (filtros, tabs, etc) */
  children?: ReactNode;
  /** Se deve ser sticky no topo */
  sticky?: boolean;
}

// ============================================================================
// COMPONENTE
// ============================================================================
export function CabecalhoPagina({
  titulo,
  subtitulo,
  icone: Icone,
  acoes,
  children,
  sticky = true,
}: CabecalhoPaginaProps) {
  return (
    <div className={`bg-[var(--surface)]/95 backdrop-blur-sm border-b border-[var(--border)] ${sticky ? 'sticky top-0 z-30' : ''}`}>
      <div className="px-6 py-4">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          {/* Título e Subtítulo */}
          <div className="flex items-center gap-3">
            {Icone && (
              <div className="p-2 bg-blue-100 text-blue-600 rounded-lg">
                <Icone className="w-5 h-5" />
              </div>
            )}
            <div>
              <h1 className="text-xl font-semibold text-[var(--text)]">{titulo}</h1>
              {subtitulo && (
                <p className="text-sm text-[var(--text-muted)]">{subtitulo}</p>
              )}
            </div>
          </div>

          {/* Ações */}
          {acoes && (
            <div className="flex items-center gap-2">
              {acoes}
            </div>
          )}
        </div>

        {/* Conteúdo adicional */}
        {children && (
          <div className="mt-4">
            {children}
          </div>
        )}
      </div>
    </div>
  );
}

export default CabecalhoPagina;
