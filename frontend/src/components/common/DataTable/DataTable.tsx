import { useMemo } from 'react';
import { Loader2 } from 'lucide-react';
import { FilterBar } from './FilterBar';
import { ColumnHeader } from './ColumnHeader';
import { useDataTable } from './useDataTable';
import type { DataTableProps, ColumnConfig } from './types';
import { EstadoVazio } from '../EstadoVazio';

/**
 * Componente DataTable reutilizável com:
 * - Seleção de coluna para filtro (clique no cabeçalho)
 * - Campo de busca geral
 * - Ordenação por clique (asc/desc)
 * - Botão limpar filtros
 */
export function DataTable<T>({
  data,
  columns,
  getRowKey,
  onSortChange,
  onFilterChange,
  onClearFilters,
  initialState,
  loading = false,
  isRowHighlighted,
  onRowClick,
  rowActions,
  headerExtra,
  className = '',
  showFilterBar = true,
  totalItems,
}: DataTableProps<T>) {
  // Hook para gerenciar estado da tabela
  const {
    state,
    processedData,
    toggleSort,
    setFilterColumn,
    setFilterValue,
    executeSearch,
    clearFilters,
    hasActiveFilters,
  } = useDataTable({
    data,
    columns,
    initialState,
    mode: onSortChange || onFilterChange ? 'server' : 'client',
    onStateChange: (newState) => {
      if (onSortChange && (newState.sortBy !== state.sortBy || newState.sortDirection !== state.sortDirection)) {
        onSortChange(newState.sortBy, newState.sortDirection);
      }
    },
  });

  // Colunas filtráveis para o dropdown
  const filterableColumns = useMemo(() => {
    return columns
      .filter((col) => col.filterable !== false)
      .map((col) => ({
        key: col.key,
        header: col.header,
        searchPlaceholder: col.searchPlaceholder,
      }));
  }, [columns]);

  // Handler para clique no cabeçalho da coluna
  const handleColumnClick = (column: ColumnConfig<T>) => {
    if (column.filterable !== false) {
      setFilterColumn(column.key);
    }
    if (column.sortable) {
      toggleSort(column.key);
    }
  };

  // Handler para busca
  const handleSearch = () => {
    executeSearch();
    if (onFilterChange) {
      onFilterChange(state.filterColumn, state.filterValue);
    }
  };

  // Handler para limpar
  const handleClear = () => {
    clearFilters();
    if (onClearFilters) {
      onClearFilters();
    }
  };

  // Dados a exibir (do hook ou prop, dependendo do modo)
  const displayData = onSortChange || onFilterChange ? data : processedData;

  return (
    <div className={`space-y-4 ${className}`}>
      {/* Barra de Filtros */}
      {showFilterBar && (
        <FilterBar
          columns={filterableColumns}
          selectedColumn={state.filterColumn}
          onColumnSelect={setFilterColumn}
          filterValue={state.filterValue}
          onFilterValueChange={setFilterValue}
          onSearch={handleSearch}
          onClear={handleClear}
          sortBy={state.sortBy}
          sortDirection={state.sortDirection}
          onToggleSort={() => toggleSort()}
          hasActiveFilters={hasActiveFilters}
        />
      )}

      {/* Extra Header (contador, paginação, etc) */}
      {headerExtra && (
        <div className="flex items-center justify-between">
          {totalItems !== undefined && (
            <p className="text-sm text-[var(--text-muted)]">
              <span className="font-semibold text-[var(--text)]">{totalItems}</span> registro(s)
              {hasActiveFilters && displayData.length < totalItems && (
                <span className="text-blue-600 ml-1">
                  ({displayData.length} filtrado(s))
                </span>
              )}
            </p>
          )}
          {headerExtra}
        </div>
      )}

      {/* Tabela */}
      <div className="bg-[var(--surface)] rounded-2xl border border-[var(--border)] shadow-[var(--shadow-soft)] overflow-hidden">
        {loading ? (
          <div className="flex items-center justify-center py-16">
            <div className="text-center">
              <Loader2 className="h-8 w-8 animate-spin text-blue-600 mx-auto mb-4" />
              <p className="text-[var(--text-muted)]">Carregando...</p>
            </div>
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-[var(--border)]">
              {/* Cabeçalho */}
              <thead className="bg-[var(--surface-muted)] sticky top-0 z-10">
                <tr>
                  {columns.map((column) => (
                    <ColumnHeader
                      key={column.key}
                      column={column}
                      isFilterColumn={state.filterColumn === column.key}
                      isSortColumn={state.sortBy === column.key}
                      sortDirection={state.sortDirection}
                      onClick={() => handleColumnClick(column)}
                    />
                  ))}
                  {rowActions && (
                    <th className="px-4 py-4 text-right text-xs font-semibold text-[var(--text-muted)] uppercase tracking-wider">
                      Ações
                    </th>
                  )}
                </tr>
              </thead>

              {/* Corpo */}
              <tbody className="bg-[var(--surface)] divide-y divide-[var(--border)]">
                {displayData.length === 0 ? (
                  <tr>
                    <td colSpan={columns.length + (rowActions ? 1 : 0)}>
                      <EstadoVazio
                        tipoBusca={hasActiveFilters}
                        acao={
                          hasActiveFilters
                            ? { texto: 'Limpar filtros', onClick: handleClear }
                            : undefined
                        }
                      />
                    </td>
                  </tr>
                ) : (
                  displayData.map((item, index) => {
                    const key = getRowKey(item);
                    const isHighlighted = isRowHighlighted?.(item);
                    const isClickable = !!onRowClick;

                    return (
                      <tr
                        key={key}
                        className={`
                          transition-colors
                          ${isHighlighted ? 'bg-blue-50/50' : 'hover:bg-blue-50/30'}
                          ${isClickable ? 'cursor-pointer' : ''}
                        `}
                        onClick={isClickable ? () => onRowClick(item) : undefined}
                      >
                        {columns.map((column) => {
                          const alignClass = {
                            left: 'text-left',
                            center: 'text-center',
                            right: 'text-right',
                          }[column.align || 'left'];

                          return (
                            <td
                              key={column.key}
                              className={`px-4 py-4 ${alignClass}`}
                              style={{ width: column.width }}
                            >
                              {column.render
                                ? column.render(item, index)
                                : String((item as any)[column.key] ?? '-')}
                            </td>
                          );
                        })}
                        {rowActions && (
                          <td className="px-4 py-4 text-right">
                            <div
                              className="flex items-center justify-end gap-1"
                              onClick={(e) => e.stopPropagation()}
                            >
                              {rowActions(item)}
                            </div>
                          </td>
                        )}
                      </tr>
                    );
                  })
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}

export default DataTable;
