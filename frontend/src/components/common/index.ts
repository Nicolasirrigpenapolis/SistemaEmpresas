// ============================================================================
// COMPONENTES COMUNS REUTILIZÁVEIS
// ============================================================================

// Modal de Confirmação
export { ModalConfirmacao } from './ModalConfirmacao';
export type { ModalConfirmacaoProps } from './ModalConfirmacao';

// Paginação
export { Paginacao } from './Paginacao';
export type { PaginacaoProps } from './Paginacao';

// Cabeçalho de Página
export { CabecalhoPagina } from './CabecalhoPagina';
export type { CabecalhoPaginaProps } from './CabecalhoPagina';

// Estados de Vazio
export { EstadoVazio } from './EstadoVazio';
export type { EstadoVazioProps } from './EstadoVazio';

// Estados de Carregando
export { EstadoCarregando, SkeletonTabela } from './EstadoCarregando';
export type { EstadoCarregandoProps, SkeletonTabelaProps } from './EstadoCarregando';

// Alertas
export { Alerta, AlertaErro, AlertaAviso, AlertaSucesso, AlertaInfo } from './Alerta';
export type { AlertaProps, TipoAlerta } from './Alerta';

// DataTable - Tabela com filtros avançados
export { DataTable, FilterBar, ColumnHeader, useDataTable } from './DataTable';
export type { 
  ColumnConfig, 
  DataTableProps, 
  DataTableState, 
  FilterBarProps, 
  ColumnHeaderProps, 
  SortDirection 
} from './DataTable';

// SearchBar - Barra de busca com seleção de coluna
export { SearchBar } from './SearchBar';
export type { SearchBarProps, SearchColumn } from './SearchBar';
