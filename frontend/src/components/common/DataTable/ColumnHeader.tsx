import { ArrowUp, ArrowDown, Filter } from 'lucide-react';
import type { ColumnHeaderProps } from './types';

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
      className={`
        px-4 py-4 text-xs font-bold text-muted-foreground uppercase tracking-wider ${alignClass}
        ${isClickable ? 'cursor-pointer select-none hover:bg-surface-hover hover:text-primary transition-colors group' : ''}
      `}
      style={{ width: column.width }}
      onClick={isClickable ? onClick : undefined}
      title={isClickable ? `Clique para ${column.filterable ? 'filtrar' : 'ordenar'} por ${column.header}` : undefined}
    >
      <div className={`flex items-center gap-2 ${column.align === 'right' ? 'justify-end' : column.align === 'center' ? 'justify-center' : 'justify-start'}`}>
        <span className={`transition-colors ${isFilterColumn ? 'text-secondary font-extrabold' : ''}`}>
          {column.header}
        </span>

        {/* Indicador de coluna de filtro selecionada */}
        {isFilterColumn && column.filterable && (
          <span className="inline-flex items-center justify-center w-5 h-5 bg-secondary/10 text-secondary rounded-md shadow-sm">
            <Filter className="w-3 h-3" />
          </span>
        )}

        {/* Indicador de ordenação */}
        {isSortColumn && column.sortable && (
          <span className={`inline-flex items-center justify-center w-5 h-5 rounded-md shadow-sm ${isFilterColumn ? 'bg-secondary/10 text-secondary' : 'bg-surface-active text-primary'
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
          <span className="opacity-0 group-hover:opacity-100 transition-opacity text-muted-foreground/50">
            <Filter className="w-3 h-3" />
          </span>
        )}
      </div>
    </th>
  );
}

export default ColumnHeader;
