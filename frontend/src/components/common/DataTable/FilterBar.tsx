import React from 'react';
import { Search, X, ArrowUp, ArrowDown, RotateCcw, Filter } from 'lucide-react';
import type { FilterBarProps } from './types';

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
  const selectedColumnConfig = columns.find((c) => c.key === selectedColumn);
  const placeholder = selectedColumnConfig?.searchPlaceholder ||
    `Buscar por ${selectedColumnConfig?.header.toLowerCase() || 'termo'}...`;

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      onSearch();
    }
  };

  const SortIcon = sortDirection === 'asc' ? ArrowUp : ArrowDown;

  return (
    <div className="bg-surface/50 backdrop-blur-sm border border-border rounded-2xl p-5 shadow-sm transition-all hover:shadow-md">
      <div className="flex flex-col lg:flex-row gap-4">
        {/* Seletor de Coluna */}
        <div className="flex-shrink-0">
          <div className="flex items-center gap-3">
            <div className="p-2.5 bg-secondary/10 rounded-xl text-secondary">
              <Filter className="w-5 h-5" />
            </div>
            <div className="relative">
              <select
                value={selectedColumn}
                onChange={(e) => onColumnSelect(e.target.value)}
                className="appearance-none w-full pl-4 pr-10 py-2.5 text-sm font-medium border border-border rounded-xl bg-surface text-primary focus:ring-2 focus:ring-secondary/20 focus:border-secondary cursor-pointer min-w-[180px] transition-all hover:border-secondary/50"
              >
                {columns.map((col) => (
                  <option key={col.key} value={col.key}>
                    {col.header}
                  </option>
                ))}
              </select>
              <div className="absolute right-3 top-1/2 -translate-y-1/2 pointer-events-none text-muted-foreground">
                <ArrowDown className="w-4 h-4" />
              </div>
            </div>
          </div>
        </div>

        {/* Campo de Busca */}
        <div className="flex-1 relative group">
          <Search className="absolute left-4 top-1/2 transform -translate-y-1/2 w-5 h-5 text-muted-foreground group-focus-within:text-secondary transition-colors" />
          <input
            type="text"
            placeholder={placeholder}
            value={filterValue}
            onChange={(e) => onFilterValueChange(e.target.value)}
            onKeyDown={handleKeyDown}
            className="w-full pl-12 pr-10 py-2.5 bg-surface border border-border rounded-xl text-sm font-medium text-primary placeholder:text-muted-foreground/70 focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all shadow-sm"
          />
          {filterValue && (
            <button
              onClick={() => onFilterValueChange('')}
              className="absolute right-3 top-1/2 transform -translate-y-1/2 p-1 rounded-lg text-muted-foreground hover:bg-surface-hover hover:text-primary transition-colors"
              title="Limpar busca"
            >
              <X className="w-4 h-4" />
            </button>
          )}
        </div>

        {/* Botões */}
        <div className="flex gap-2 flex-shrink-0">
          <button
            onClick={onSearch}
            className="flex items-center gap-2 px-5 py-2.5 bg-secondary text-white rounded-xl hover:bg-secondary/90 transition-all font-bold shadow-lg shadow-secondary/20 active:scale-95"
          >
            <Search className="w-4 h-4" />
            <span className="hidden sm:inline">Buscar</span>
          </button>

          <button
            onClick={onToggleSort}
            className={`flex items-center gap-2 px-4 py-2.5 border rounded-xl transition-all font-bold ${sortDirection === 'desc'
                ? 'border-secondary text-secondary bg-secondary/10'
                : 'border-border text-muted-foreground hover:bg-surface-hover hover:text-primary bg-surface'
              }`}
            title={`Ordenar ${sortDirection === 'asc' ? 'Z → A' : 'A → Z'}`}
          >
            <SortIcon className="w-4 h-4" />
            <span className="hidden sm:inline">{sortDirection === 'asc' ? 'A → Z' : 'Z → A'}</span>
          </button>

          <button
            onClick={onClear}
            disabled={!hasActiveFilters}
            className={`flex items-center gap-2 px-4 py-2.5 border rounded-xl transition-all font-bold ${hasActiveFilters
                ? 'border-red-200 text-red-600 bg-red-50 hover:bg-red-100 hover:border-red-300'
                : 'border-border text-muted-foreground/50 bg-surface opacity-50 cursor-not-allowed'
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
        <div className="mt-4 pt-4 border-t border-border animate-fade-in">
          <div className="flex items-center gap-3 text-sm">
            <span className="text-muted-foreground font-medium">Filtro ativo:</span>
            <span className="inline-flex items-center gap-2 px-3 py-1.5 bg-secondary/10 text-secondary rounded-lg border border-secondary/20">
              <span className="font-bold">{selectedColumnConfig?.header}</span>
              <span className="text-secondary/70">contém</span>
              <span className="font-bold">"{filterValue}"</span>
              <button
                onClick={onClear}
                className="ml-2 p-0.5 rounded-md hover:bg-secondary/20 transition-colors"
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
