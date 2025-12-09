import React from 'react';
import { Search, X, ArrowUp, ArrowDown, RotateCcw } from 'lucide-react';
import type { FilterBarProps } from './types';

/**
 * Barra de filtros para a DataTable
 * Contém: seleção de coluna, campo de busca, botão de ordenação, botão limpar
 */
export function FilterBar({
  columns,
  selectedColumn,
  onColumnSelect,
  filterValue,
  onFilterValueChange,
  onSearch,
  onClear,
  sortDirection,
  onToggleSort,
  hasActiveFilters,
}: FilterBarProps) {
  // Encontrar placeholder da coluna selecionada
  const selectedColumnConfig = columns.find((c) => c.key === selectedColumn);
  const placeholder = selectedColumnConfig?.searchPlaceholder || 
    `Buscar por ${selectedColumnConfig?.header.toLowerCase() || 'termo'}...`;

  // Handler para tecla Enter
  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      onSearch();
    }
  };

  // Ícone de ordenação
  const SortIcon = sortDirection === 'asc' ? ArrowUp : ArrowDown;

  return (
    <div className="bg-[var(--surface)] backdrop-blur-sm border border-[var(--border)] rounded-xl p-4 shadow-sm">
      <div className="flex flex-col lg:flex-row gap-3">
        {/* Seletor de Coluna */}
        <div className="flex-shrink-0">
          <div className="flex items-center gap-2">
            <span className="text-sm text-[var(--text-muted)] whitespace-nowrap">Buscar em:</span>
            <select
              value={selectedColumn}
              onChange={(e) => onColumnSelect(e.target.value)}
              className="px-3 py-2.5 text-sm border border-[var(--border)] rounded-lg bg-[var(--surface)] text-[var(--text)] focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 cursor-pointer min-w-[150px]"
            >
              {columns.map((col) => (
                <option key={col.key} value={col.key}>
                  {col.header}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Campo de Busca */}
        <div className="flex-1 relative group">
          <Search className="absolute left-4 top-1/2 transform -translate-y-1/2 w-4 h-4 text-[var(--text-muted)] group-focus-within:text-blue-500 transition-colors" />
          <input
            type="text"
            placeholder={placeholder}
            value={filterValue}
            onChange={(e) => onFilterValueChange(e.target.value)}
            onKeyDown={handleKeyDown}
            className="w-full pl-11 pr-4 py-2.5 bg-[var(--surface)] border border-[var(--border)] rounded-xl text-sm text-[var(--text)] placeholder:text-[var(--text-muted)] focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all shadow-sm"
          />
          {filterValue && (
            <button
              onClick={() => onFilterValueChange('')}
              className="absolute right-3 top-1/2 transform -translate-y-1/2 text-[var(--text-muted)] hover:text-[var(--text)] transition-colors"
              title="Limpar busca"
            >
              <X className="w-4 h-4" />
            </button>
          )}
        </div>

        {/* Botões */}
        <div className="flex gap-2 flex-shrink-0">
          {/* Botão Buscar */}
          <button
            onClick={onSearch}
            className="flex items-center gap-2 px-4 py-2.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all font-medium shadow-sm"
          >
            <Search className="w-4 h-4" />
            <span className="hidden sm:inline">Buscar</span>
          </button>

          {/* Botão Ordenação */}
          <button
            onClick={onToggleSort}
            className={`flex items-center gap-2 px-4 py-2.5 border rounded-xl transition-all font-medium ${
              sortDirection === 'desc'
                ? 'border-blue-500 text-blue-600 bg-blue-50'
                : 'border-[var(--border)] text-[var(--text)] hover:bg-[var(--surface-muted)] bg-[var(--surface)] shadow-sm'
            }`}
            title={`Ordenar ${sortDirection === 'asc' ? 'Z → A' : 'A → Z'}`}
          >
            <SortIcon className="w-4 h-4" />
            <span className="hidden sm:inline">{sortDirection === 'asc' ? 'A → Z' : 'Z → A'}</span>
          </button>

          {/* Botão Limpar */}
          <button
            onClick={onClear}
            disabled={!hasActiveFilters}
            className={`flex items-center gap-2 px-4 py-2.5 border rounded-xl transition-all font-medium ${
              hasActiveFilters
                ? 'border-red-200 text-red-600 bg-red-50 hover:bg-red-100'
                : 'border-[var(--border)] text-[var(--text-muted)] bg-[var(--surface)] opacity-50 cursor-not-allowed'
            }`}
            title="Limpar filtros"
          >
            <RotateCcw className="w-4 h-4" />
            <span className="hidden sm:inline">Limpar</span>
          </button>
        </div>
      </div>

      {/* Indicador de Filtro Ativo */}
      {hasActiveFilters && (
        <div className="mt-3 pt-3 border-t border-[var(--border)]">
          <div className="flex items-center gap-2 text-sm">
            <span className="text-[var(--text-muted)]">Filtro ativo:</span>
            <span className="inline-flex items-center gap-2 px-3 py-1 bg-blue-50 text-blue-700 rounded-full">
              <span className="font-medium">{selectedColumnConfig?.header}</span>
              <span className="text-blue-500">contém</span>
              <span className="font-medium">"{filterValue}"</span>
              <button
                onClick={onClear}
                className="ml-1 text-blue-500 hover:text-blue-700"
              >
                <X className="w-3 h-3" />
              </button>
            </span>
          </div>
        </div>
      )}
    </div>
  );
}

export default FilterBar;
