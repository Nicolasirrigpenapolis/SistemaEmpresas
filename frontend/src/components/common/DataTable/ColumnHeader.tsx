import { ArrowUp, ArrowDown, Filter } from 'lucide-react';
import type { ColumnHeaderProps } from './types';

/**
 * Cabeçalho de coluna clicável para ordenação e seleção de filtro
 */
export function ColumnHeader({
  column,
  isFilterColumn,
  isSortColumn,
  sortDirection,
  onClick,
}: ColumnHeaderProps) {
  const alignClass = {
    left: 'text-left',
    center: 'text-center',
    right: 'text-right',
  }[column.align || 'left'];

  const isClickable = column.sortable || column.filterable;

  return (
    <th
      className={`px-4 py-4 text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider ${alignClass} ${
        isClickable ? 'cursor-pointer select-none hover:bg-[var(--surface)] transition-colors group' : ''
      }`}
      style={{ width: column.width }}
      onClick={isClickable ? onClick : undefined}
      title={isClickable ? `Clique para ${column.filterable ? 'filtrar' : 'ordenar'} por ${column.header}` : undefined}
    >
      <div className="flex items-center gap-2">
        <span className={isFilterColumn ? 'text-blue-600 font-bold' : ''}>
          {column.header}
        </span>

        {/* Indicador de coluna de filtro selecionada */}
        {isFilterColumn && column.filterable && (
          <span className="inline-flex items-center justify-center w-5 h-5 bg-blue-100 text-blue-600 rounded">
            <Filter className="w-3 h-3" />
          </span>
        )}

        {/* Indicador de ordenação */}
        {isSortColumn && column.sortable && (
          <span className={`inline-flex items-center justify-center w-5 h-5 rounded ${
            isFilterColumn ? 'bg-blue-100 text-blue-600' : 'bg-[var(--surface-muted)] text-[var(--text-muted)]'
          }`}>
            {sortDirection === 'asc' ? (
              <ArrowUp className="w-3 h-3" />
            ) : (
              <ArrowDown className="w-3 h-3" />
            )}
          </span>
        )}

        {/* Indicador hover para colunas clicáveis */}
        {isClickable && !isFilterColumn && !isSortColumn && (
          <span className="opacity-0 group-hover:opacity-50 transition-opacity">
            <Filter className="w-3 h-3" />
          </span>
        )}
      </div>
    </th>
  );
}

export default ColumnHeader;
