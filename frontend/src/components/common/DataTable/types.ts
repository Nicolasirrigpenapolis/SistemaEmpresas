// ============================================================================
// TIPOS PARA O COMPONENTE DataTable
// ============================================================================

export type SortDirection = 'asc' | 'desc';

export interface ColumnConfig<T> {
  /** Identificador único da coluna */
  key: string;
  /** Título exibido no cabeçalho */
  header: string;
  /** Se a coluna pode ser usada para ordenação */
  sortable?: boolean;
  /** Se a coluna pode ser usada para filtro */
  filterable?: boolean;
  /** Largura da coluna (ex: '150px', '20%') */
  width?: string;
  /** Função para renderizar o conteúdo da célula */
  render?: (item: T, index: number) => React.ReactNode;
  /** Função para obter o valor de ordenação */
  getValue?: (item: T) => string | number | boolean | null;
  /** Alinhamento do texto */
  align?: 'left' | 'center' | 'right';
  /** Placeholder do campo de busca quando esta coluna estiver selecionada */
  searchPlaceholder?: string;
  /** Se é a coluna padrão para busca */
  defaultSearch?: boolean;
}

export interface DataTableState {
  /** Campo atual de ordenação */
  sortBy: string;
  /** Direção da ordenação */
  sortDirection: SortDirection;
  /** Coluna selecionada para filtro */
  filterColumn: string;
  /** Valor do filtro/busca */
  filterValue: string;
  /** Página atual */
  pageNumber: number;
  /** Itens por página */
  pageSize: number;
}

export interface DataTableProps<T> {
  /** Dados para exibir na tabela */
  data: T[];
  /** Configuração das colunas */
  columns: ColumnConfig<T>[];
  /** Função para obter a chave única de cada item */
  getRowKey: (item: T) => string | number;
  /** Callback quando a ordenação muda */
  onSortChange?: (sortBy: string, direction: SortDirection) => void;
  /** Callback quando o filtro muda */
  onFilterChange?: (column: string, value: string) => void;
  /** Callback quando limpar filtros */
  onClearFilters?: () => void;
  /** Estado inicial */
  initialState?: Partial<DataTableState>;
  /** Se está carregando */
  loading?: boolean;
  /** Mensagem quando não há dados */
  emptyMessage?: string;
  /** Se uma linha está destacada (ex: selecionada) */
  isRowHighlighted?: (item: T) => boolean;
  /** Callback ao clicar em uma linha */
  onRowClick?: (item: T) => void;
  /** Ações para cada linha (botões) */
  rowActions?: (item: T) => React.ReactNode;
  /** Renderizar conteúdo extra no cabeçalho da tabela */
  headerExtra?: React.ReactNode;
  /** Classes CSS extras para a tabela */
  className?: string;
  /** Se deve mostrar a barra de filtros */
  showFilterBar?: boolean;
  /** Total de itens (para exibição) */
  totalItems?: number;
}

export interface FilterBarProps {
  /** Colunas disponíveis para filtro */
  columns: Array<{ key: string; header: string; searchPlaceholder?: string }>;
  /** Coluna selecionada para filtro */
  selectedColumn: string;
  /** Callback ao selecionar coluna */
  onColumnSelect: (column: string) => void;
  /** Valor atual do filtro */
  filterValue: string;
  /** Callback ao alterar o valor do filtro */
  onFilterValueChange: (value: string) => void;
  /** Callback ao submeter busca (Enter ou botão) */
  onSearch: () => void;
  /** Callback ao limpar filtros */
  onClear: () => void;
  /** Campo de ordenação atual */
  sortBy: string;
  /** Direção da ordenação */
  sortDirection: SortDirection;
  /** Callback ao clicar no botão de ordenação */
  onToggleSort: () => void;
  /** Se há filtros ativos */
  hasActiveFilters: boolean;
}

export interface ColumnHeaderProps {
  /** Configuração da coluna */
  column: ColumnConfig<any>;
  /** Se esta coluna está selecionada para filtro */
  isFilterColumn: boolean;
  /** Se está ordenando por esta coluna */
  isSortColumn: boolean;
  /** Direção da ordenação */
  sortDirection: SortDirection;
  /** Callback ao clicar na coluna */
  onClick: () => void;
}
