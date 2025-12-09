import { Loader2 } from 'lucide-react';

// ============================================================================
// TIPOS
// ============================================================================
export interface EstadoCarregandoProps {
  /** Mensagem de carregamento */
  mensagem?: string;
  /** Se deve ocupar altura total da tela */
  telaCheia?: boolean;
  /** Altura mínima customizada */
  alturaMinima?: string;
}

// ============================================================================
// COMPONENTE
// ============================================================================
export function EstadoCarregando({
  mensagem = 'Carregando...',
  telaCheia = false,
  alturaMinima = '200px',
}: EstadoCarregandoProps) {
  const classes = telaCheia 
    ? 'min-h-screen' 
    : `min-h-[${alturaMinima}]`;

  return (
    <div className={`flex flex-col items-center justify-center py-12 px-4 ${classes}`}>
      <Loader2 className="w-8 h-8 text-blue-600 animate-spin mb-4" />
      <p className="text-sm text-[var(--text-muted)]">{mensagem}</p>
    </div>
  );
}

// ============================================================================
// VARIANTE: SKELETON DE TABELA
// ============================================================================
export interface SkeletonTabelaProps {
  /** Número de linhas */
  linhas?: number;
  /** Número de colunas */
  colunas?: number;
}

export function SkeletonTabela({ linhas = 5, colunas = 4 }: SkeletonTabelaProps) {
  return (
    <div className="animate-pulse">
      {/* Header */}
      <div className="bg-gray-100 px-4 py-3 border-b border-[var(--border)]">
        <div className="flex gap-4">
          {Array.from({ length: colunas }).map((_, i) => (
            <div key={i} className="h-4 bg-gray-300 rounded flex-1" />
          ))}
        </div>
      </div>
      
      {/* Linhas */}
      {Array.from({ length: linhas }).map((_, rowIndex) => (
        <div key={rowIndex} className="px-4 py-4 border-b border-[var(--border)]">
          <div className="flex gap-4">
            {Array.from({ length: colunas }).map((_, colIndex) => (
              <div 
                key={colIndex} 
                className="h-4 bg-gray-200 rounded flex-1"
                style={{ width: `${Math.random() * 40 + 60}%` }}
              />
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}

export default EstadoCarregando;
