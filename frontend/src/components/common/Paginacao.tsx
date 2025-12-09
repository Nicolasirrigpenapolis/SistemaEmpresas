import { ChevronLeft, ChevronRight } from 'lucide-react';

// ============================================================================
// TIPOS
// ============================================================================
export interface PaginacaoProps {
  /** Página atual (começa em 1) */
  paginaAtual: number;
  /** Total de páginas */
  totalPaginas: number;
  /** Total de itens */
  totalItens: number;
  /** Itens por página */
  itensPorPagina: number;
  /** Callback ao mudar de página */
  onMudarPagina: (pagina: number) => void;
  /** Callback ao mudar itens por página (opcional) */
  onMudarItensPorPagina?: (itens: number) => void;
  /** Opções de itens por página */
  opcoesItensPorPagina?: number[];
  /** Se está carregando */
  carregando?: boolean;
}

// ============================================================================
// COMPONENTE
// ============================================================================
export function Paginacao({
  paginaAtual,
  totalPaginas,
  totalItens,
  itensPorPagina,
  onMudarPagina,
  onMudarItensPorPagina,
  opcoesItensPorPagina = [10, 25, 50, 100],
  carregando = false,
}: PaginacaoProps) {
  const inicio = totalItens === 0 ? 0 : (paginaAtual - 1) * itensPorPagina + 1;
  const fim = Math.min(paginaAtual * itensPorPagina, totalItens);

  const podeVoltar = paginaAtual > 1;
  const podeAvancar = paginaAtual < totalPaginas;

  // Gera os números das páginas para exibir
  const gerarNumerosPaginas = (): (number | 'ellipsis')[] => {
    const paginas: (number | 'ellipsis')[] = [];
    const maxVisivel = 5;

    if (totalPaginas <= maxVisivel) {
      for (let i = 1; i <= totalPaginas; i++) {
        paginas.push(i);
      }
    } else {
      // Sempre mostra primeira página
      paginas.push(1);

      if (paginaAtual > 3) {
        paginas.push('ellipsis');
      }

      // Páginas ao redor da atual
      const inicio = Math.max(2, paginaAtual - 1);
      const fim = Math.min(totalPaginas - 1, paginaAtual + 1);

      for (let i = inicio; i <= fim; i++) {
        if (!paginas.includes(i)) {
          paginas.push(i);
        }
      }

      if (paginaAtual < totalPaginas - 2) {
        paginas.push('ellipsis');
      }

      // Sempre mostra última página
      if (!paginas.includes(totalPaginas)) {
        paginas.push(totalPaginas);
      }
    }

    return paginas;
  };

  if (totalItens === 0) {
    return null;
  }

  return (
    <div className="flex flex-col sm:flex-row items-center justify-between gap-4 px-4 py-3 bg-[var(--surface)] border-t border-[var(--border)]">
      {/* Info de registros */}
      <div className="flex items-center gap-4">
        <p className="text-sm text-gray-700">
          Mostrando <span className="font-medium">{inicio}</span> a{' '}
          <span className="font-medium">{fim}</span> de{' '}
          <span className="font-medium">{totalItens.toLocaleString('pt-BR')}</span> registros
        </p>

        {/* Seletor de itens por página */}
        {onMudarItensPorPagina && (
          <div className="flex items-center gap-2">
            <span className="text-sm text-[var(--text-muted)]">Exibir:</span>
            <select
              value={itensPorPagina}
              onChange={(e) => onMudarItensPorPagina(Number(e.target.value))}
              disabled={carregando}
              className="text-sm border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500 disabled:opacity-50"
            >
              {opcoesItensPorPagina.map((opcao) => (
                <option key={opcao} value={opcao}>
                  {opcao}
                </option>
              ))}
            </select>
          </div>
        )}
      </div>

      {/* Navegação de páginas */}
      <div className="flex items-center gap-1">
        {/* Botão Anterior */}
        <button
          onClick={() => onMudarPagina(paginaAtual - 1)}
          disabled={!podeVoltar || carregando}
          className="p-2 text-[var(--text-muted)] hover:text-gray-700 hover:bg-gray-100 rounded-lg disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          title="Página anterior"
        >
          <ChevronLeft className="w-5 h-5" />
        </button>

        {/* Números das páginas */}
        <div className="hidden sm:flex items-center gap-1">
          {gerarNumerosPaginas().map((pagina, index) => {
            if (pagina === 'ellipsis') {
              return (
                <span key={`ellipsis-${index}`} className="px-2 text-gray-400">
                  ...
                </span>
              );
            }

            const isAtual = pagina === paginaAtual;
            return (
              <button
                key={pagina}
                onClick={() => onMudarPagina(pagina)}
                disabled={carregando}
                className={`min-w-[36px] h-9 px-3 text-sm font-medium rounded-lg transition-colors ${
                  isAtual
                    ? 'bg-blue-600 text-white'
                    : 'text-gray-700 hover:bg-gray-100'
                } disabled:opacity-50`}
              >
                {pagina}
              </button>
            );
          })}
        </div>

        {/* Info mobile */}
        <span className="sm:hidden px-3 text-sm text-gray-700">
          {paginaAtual} / {totalPaginas}
        </span>

        {/* Botão Próximo */}
        <button
          onClick={() => onMudarPagina(paginaAtual + 1)}
          disabled={!podeAvancar || carregando}
          className="p-2 text-[var(--text-muted)] hover:text-gray-700 hover:bg-gray-100 rounded-lg disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          title="Próxima página"
        >
          <ChevronRight className="w-5 h-5" />
        </button>
      </div>
    </div>
  );
}

export default Paginacao;
