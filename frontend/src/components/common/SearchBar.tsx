import React, { useState, useCallback, useMemo } from 'react';
import { Search, X, ArrowUp, ArrowDown, RotateCcw } from 'lucide-react';

// ============================================================================
// TIPOS
// ============================================================================

export type SortDirection = 'asc' | 'desc';

export interface SearchColumn {
  key: string;
  label: string;
  placeholder?: string;
}

export interface SearchBarProps {
  /** Colunas disponíveis para busca */
  columns: SearchColumn[];
  /** Callback quando buscar (Enter ou botão) */
  onSearch: (column: string, value: string, sortDirection: SortDirection) => void;
  /** Callback quando limpar */
  onClear: () => void;
  /** Valor inicial da busca */
  initialValue?: string;
  /** Coluna inicial selecionada */
  initialColumn?: string;
  /** Direção inicial da ordenação */
  initialSortDirection?: SortDirection;
  /** Se está carregando */
  loading?: boolean;
  /** Classes extras */
  className?: string;
}

// ============================================================================
// COMPONENTE SearchBar
// ============================================================================

export function SearchBar({
  columns,
  onSearch,
  onClear,
  initialValue = '',
  initialColumn,
  initialSortDirection = 'asc',
  loading = false,
  className = '',
}: SearchBarProps) {
  // Estado local
  const [selectedColumn, setSelectedColumn] = useState(initialColumn || columns[0]?.key || '');
  const [searchValue, setSearchValue] = useState(initialValue);
  const [sortDirection, setSortDirection] = useState<SortDirection>(initialSortDirection);

  // Placeholder dinâmico
  const placeholder = useMemo(() => {
    const col = columns.find((c) => c.key === selectedColumn);
    return col?.placeholder || `Buscar por ${col?.label.toLowerCase() || 'termo'}...`;
  }, [columns, selectedColumn]);

  // Se há filtro ativo
  const hasFilter = searchValue.trim().length > 0;

  // Handlers
  const handleSearch = useCallback(() => {
    onSearch(selectedColumn, searchValue.trim(), sortDirection);
  }, [selectedColumn, searchValue, sortDirection, onSearch]);

  const handleKeyDown = useCallback(
    (e: React.KeyboardEvent) => {
      if (e.key === 'Enter') {
        handleSearch();
      }
    },
    [handleSearch]
  );

  const handleClear = useCallback(() => {
    setSearchValue('');
    setSortDirection('asc');
    onClear();
  }, [onClear]);

  const handleToggleSort = useCallback(() => {
    const newDirection = sortDirection === 'asc' ? 'desc' : 'asc';
    setSortDirection(newDirection);
    // Se já tem busca ativa, reexecuta com nova direção
    if (hasFilter) {
      onSearch(selectedColumn, searchValue.trim(), newDirection);
    }
  }, [sortDirection, hasFilter, selectedColumn, searchValue, onSearch]);

  const handleColumnChange = useCallback((newColumn: string) => {
    setSelectedColumn(newColumn);
  }, []);

  // Ícone de ordenação
  const SortIcon = sortDirection === 'asc' ? ArrowUp : ArrowDown;

  return (
    <div className={`bg-[var(--surface)] backdrop-blur-sm border border-[var(--border)] rounded-xl p-4 shadow-sm ${className}`}>
      <div className="flex flex-col lg:flex-row gap-3">
        {/* Seletor de Coluna */}
        <div className="flex-shrink-0">
          <div className="flex items-center gap-2">
            <span className="text-sm text-[var(--text-muted)] whitespace-nowrap">Buscar em:</span>
            <select
              value={selectedColumn}
              onChange={(e) => handleColumnChange(e.target.value)}
              disabled={loading}
              className="px-3 py-2.5 text-sm border border-[var(--border)] rounded-lg bg-[var(--surface)] text-[var(--text)] focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 cursor-pointer min-w-[160px] disabled:opacity-50"
            >
              {columns.map((col) => (
                <option key={col.key} value={col.key}>
                  {col.label}
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
            value={searchValue}
            onChange={(e) => setSearchValue(e.target.value)}
            onKeyDown={handleKeyDown}
            disabled={loading}
            className="w-full pl-11 pr-10 py-2.5 bg-[var(--surface)] border border-[var(--border)] rounded-xl text-sm text-[var(--text)] placeholder:text-[var(--text-muted)] focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all shadow-sm disabled:opacity-50"
          />
          {searchValue && (
            <button
              onClick={() => setSearchValue('')}
              className="absolute right-3 top-1/2 transform -translate-y-1/2 text-[var(--text-muted)] hover:text-[var(--text)] transition-colors"
              title="Limpar campo"
            >
              <X className="w-4 h-4" />
            </button>
          )}
        </div>

        {/* Botões */}
        <div className="flex gap-2 flex-shrink-0">
          {/* Botão Buscar */}
          <button
            onClick={handleSearch}
            disabled={loading}
            className="flex items-center gap-2 px-4 py-2.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all font-medium shadow-sm disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <Search className="w-4 h-4" />
            <span className="hidden sm:inline">Buscar</span>
          </button>

          {/* Botão Ordenação */}
          <button
            onClick={handleToggleSort}
            disabled={loading}
            className={`flex items-center gap-2 px-4 py-2.5 border rounded-xl transition-all font-medium ${
              sortDirection === 'desc'
                ? 'border-blue-500 text-blue-600 bg-blue-50'
                : 'border-[var(--border)] text-[var(--text)] hover:bg-[var(--surface-muted)] bg-[var(--surface)] shadow-sm'
            } disabled:opacity-50 disabled:cursor-not-allowed`}
            title={`Ordenar ${sortDirection === 'asc' ? 'Z → A (decrescente)' : 'A → Z (crescente)'}`}
          >
            <SortIcon className="w-4 h-4" />
            <span className="hidden sm:inline">{sortDirection === 'asc' ? 'A → Z' : 'Z → A'}</span>
          </button>

          {/* Botão Limpar */}
          <button
            onClick={handleClear}
            disabled={!hasFilter || loading}
            className={`flex items-center gap-2 px-4 py-2.5 border rounded-xl transition-all font-medium ${
              hasFilter
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
      {hasFilter && (
        <div className="mt-3 pt-3 border-t border-[var(--border)]">
          <div className="flex items-center gap-2 text-sm flex-wrap">
            <span className="text-[var(--text-muted)]">Filtrando:</span>
            <span className="inline-flex items-center gap-2 px-3 py-1 bg-blue-50 text-blue-700 rounded-full">
              <span className="font-medium">{columns.find((c) => c.key === selectedColumn)?.label}</span>
              <span className="text-blue-500">contém</span>
              <span className="font-medium">"{searchValue}"</span>
              <span className="text-blue-500 mx-1">•</span>
              <span className="text-blue-500">{sortDirection === 'asc' ? '↑ crescente' : '↓ decrescente'}</span>
              <button
                onClick={handleClear}
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

export default SearchBar;
