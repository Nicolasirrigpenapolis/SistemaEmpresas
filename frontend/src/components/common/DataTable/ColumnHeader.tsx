import type { ColumnHeaderProps } from './types';

export function ColumnHeader({
  column,
  isFilterColumn,
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
        px-4 py-4 text-xs font-bold text-muted-foreground uppercase tracking-wider ${alignClass} align-middle
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
      </div>
    </th>
  );
}

export default ColumnHeader;
