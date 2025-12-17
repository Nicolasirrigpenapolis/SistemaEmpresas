/**
 * Tipos comuns compartilhados entre todos os módulos
 */

/**
 * Resultado paginado da API
 */
export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

/**
 * Resultado de operação genérico
 */
export interface OperacaoResultDto {
  sucesso: boolean;
  mensagem: string;
  id?: number;
}
