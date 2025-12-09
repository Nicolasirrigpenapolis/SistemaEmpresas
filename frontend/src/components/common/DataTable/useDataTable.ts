import { useState, useCallback, useMemo } from 'react';
import type { DataTableState, SortDirection, ColumnConfig } from './types';

interface UseDataTableOptions<T> {
  /** Dados para ordenar/filtrar */
  data: T[];
  /** Configuração das colunas */
  columns: ColumnConfig<T>[];
  /** Estado inicial */
  initialState?: Partial<DataTableState>;
  /** Callback quando estado muda (para persistência ou busca no servidor) */
  onStateChange?: (state: DataTableState) => void;
  /** Se a ordenação/filtro deve ser feita no cliente ou servidor */
  mode?: 'client' | 'server';
}

interface UseDataTableReturn<T> {
  /** Estado atual da tabela */
  state: DataTableState;
  /** Dados processados (ordenados e filtrados se mode='client') */
  processedData: T[];
  /** Alterar ordenação */
  setSort: (column: string, direction?: SortDirection) => void;
  /** Alternar direção da ordenação */
  toggleSort: (column?: string) => void;
  /** Selecionar coluna para filtro */
  setFilterColumn: (column: string) => void;
  /** Alterar valor do filtro */
  setFilterValue: (value: string) => void;
  /** Executar busca (para mode='server') */
  executeSearch: () => void;
  /** Limpar todos os filtros */
  clearFilters: () => void;
  /** Se há filtros ativos */
  hasActiveFilters: boolean;
  /** Alterar página */
  setPageNumber: (page: number) => void;
  /** Alterar itens por página */
  setPageSize: (size: number) => void;
}

export function useDataTable<T>({
  data,
  columns,
  initialState,
  onStateChange,
  mode = 'client',
}: UseDataTableOptions<T>): UseDataTableReturn<T> {
  // Encontrar coluna padrão para filtro
  const defaultFilterColumn = useMemo(() => {
    const defaultCol = columns.find((c) => c.defaultSearch && c.filterable);
    if (defaultCol) return defaultCol.key;
    const firstFilterable = columns.find((c) => c.filterable);
    return firstFilterable?.key || columns[0]?.key || '';
  }, [columns]);

  // Estado interno
  const [state, setState] = useState<DataTableState>({
    sortBy: initialState?.sortBy || columns[0]?.key || '',
    sortDirection: initialState?.sortDirection || 'asc',
    filterColumn: initialState?.filterColumn || defaultFilterColumn,
    filterValue: initialState?.filterValue || '',
    pageNumber: initialState?.pageNumber || 1,
    pageSize: initialState?.pageSize || 25,
  });

  // Atualizar estado e notificar
  const updateState = useCallback(
    (updates: Partial<DataTableState>) => {
      setState((prev) => {
        const newState = { ...prev, ...updates };
        onStateChange?.(newState);
        return newState;
      });
    },
    [onStateChange]
  );

  // Ordenação
  const setSort = useCallback(
    (column: string, direction?: SortDirection) => {
      updateState({
        sortBy: column,
        sortDirection: direction || 'asc',
      });
    },
    [updateState]
  );

  const toggleSort = useCallback(
    (column?: string) => {
      const targetColumn = column || state.sortBy;
      if (targetColumn === state.sortBy) {
        // Mesma coluna: alternar direção
        updateState({
          sortDirection: state.sortDirection === 'asc' ? 'desc' : 'asc',
        });
      } else {
        // Nova coluna: ordenar ascendente
        updateState({
          sortBy: targetColumn,
          sortDirection: 'asc',
        });
      }
    },
    [state.sortBy, state.sortDirection, updateState]
  );

  // Filtro
  const setFilterColumn = useCallback(
    (column: string) => {
      updateState({ filterColumn: column });
    },
    [updateState]
  );

  const setFilterValue = useCallback(
    (value: string) => {
      updateState({ filterValue: value });
    },
    [updateState]
  );

  const executeSearch = useCallback(() => {
    // Resetar para primeira página ao buscar
    updateState({ pageNumber: 1 });
  }, [updateState]);

  const clearFilters = useCallback(() => {
    updateState({
      filterValue: '',
      filterColumn: defaultFilterColumn,
      sortBy: columns[0]?.key || '',
      sortDirection: 'asc',
      pageNumber: 1,
    });
  }, [updateState, defaultFilterColumn, columns]);

  // Verificar se há filtros ativos
  const hasActiveFilters = useMemo(() => {
    return state.filterValue.trim().length > 0;
  }, [state.filterValue]);

  // Paginação
  const setPageNumber = useCallback(
    (page: number) => {
      updateState({ pageNumber: page });
    },
    [updateState]
  );

  const setPageSize = useCallback(
    (size: number) => {
      updateState({ pageSize: size, pageNumber: 1 });
    },
    [updateState]
  );

  // Processar dados (ordenação e filtro no cliente)
  const processedData = useMemo(() => {
    if (mode === 'server') {
      return data;
    }

    let result = [...data];

    // Filtrar
    if (state.filterValue.trim()) {
      const filterCol = columns.find((c) => c.key === state.filterColumn);
      const searchTerm = state.filterValue.toLowerCase().trim();

      result = result.filter((item) => {
        let value: any;
        if (filterCol?.getValue) {
          value = filterCol.getValue(item);
        } else {
          value = (item as any)[state.filterColumn];
        }

        if (value === null || value === undefined) return false;
        return String(value).toLowerCase().includes(searchTerm);
      });
    }

    // Ordenar
    const sortCol = columns.find((c) => c.key === state.sortBy);
    if (sortCol) {
      const direction = state.sortDirection === 'asc' ? 1 : -1;

      result.sort((a, b) => {
        let valueA: any, valueB: any;

        if (sortCol.getValue) {
          valueA = sortCol.getValue(a);
          valueB = sortCol.getValue(b);
        } else {
          valueA = (a as any)[state.sortBy];
          valueB = (b as any)[state.sortBy];
        }

        // Tratar nulos
        if (valueA === null || valueA === undefined) return 1;
        if (valueB === null || valueB === undefined) return -1;

        // Comparar por tipo
        if (typeof valueA === 'string' && typeof valueB === 'string') {
          return valueA.localeCompare(valueB, 'pt-BR', { sensitivity: 'base' }) * direction;
        }

        if (typeof valueA === 'number' && typeof valueB === 'number') {
          return (valueA - valueB) * direction;
        }

        if (typeof valueA === 'boolean' && typeof valueB === 'boolean') {
          return ((valueA ? 1 : 0) - (valueB ? 1 : 0)) * direction;
        }

        // Fallback: converter para string
        return String(valueA).localeCompare(String(valueB), 'pt-BR') * direction;
      });
    }

    return result;
  }, [data, state, columns, mode]);

  return {
    state,
    processedData,
    setSort,
    toggleSort,
    setFilterColumn,
    setFilterValue,
    executeSearch,
    clearFilters,
    hasActiveFilters,
    setPageNumber,
    setPageSize,
  };
}

export default useDataTable;
