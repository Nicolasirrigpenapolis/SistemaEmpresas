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
  sticky = false,
}: CabecalhoPaginaProps) {
  return (
    <div className={`bg-surface/95 backdrop-blur-md border-b border-border ${sticky ? 'sticky top-16 md:top-20 z-30' : ''}`}>
      <div className="px-6 py-6">
        <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-6">
          {/* Título e Subtítulo */}
          <div className="flex items-center gap-4">
            {Icone && (
              <div className="p-3 bg-blue-600 rounded-2xl shadow-lg shadow-blue-500/20">
                <Icone className="w-6 h-6 text-white" />
              </div>
            )}
            <div>
              <h1 className="text-2xl font-bold text-gray-900 tracking-tight">{titulo}</h1>
              {subtitulo && (
                <p className="text-sm text-gray-500 font-medium mt-0.5">{subtitulo}</p>
              )}
            </div>
          </div>

          {/* Ações */}
          {acoes && (
            <div className="flex items-center gap-3">
              {acoes}
            </div>
          )}
        </div>

        {/* Conteúdo adicional */}
        {children && (
          <div className="mt-6">
            {children}
          </div>
        )}
      </div>
    </div>
  );
}

export default CabecalhoPagina;
