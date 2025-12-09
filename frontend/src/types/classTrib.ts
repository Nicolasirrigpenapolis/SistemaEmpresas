// ============================================================================
// TYPES PARA CLASSTRIB (Dados API SVRS)
// ============================================================================

/**
 * DTO principal de ClassTrib
 * Representa um registro de classificação tributária do SVRS
 */
export interface ClassTribDto {
  id: number;
  codigoClassTrib: string;
  codigoSituacaoTributaria: string;
  descricaoSituacaoTributaria?: string;
  descricaoClassTrib: string;
  percentualReducaoIBS: number;
  percentualReducaoCBS: number;
  tipoAliquota?: string;
  validoParaNFe: boolean;
  tributacaoRegular: boolean;
  creditoPresumidoOperacoes: boolean;
  estornoCredito: boolean;
  anexoLegislacao?: number;
  linkLegislacao?: string;
}

/**
 * DTO para autocomplete de ClassTrib
 */
export interface ClassTribAutocomplete {
  id: number;
  codigoClassTrib: string;
  cst: string;
  descricao: string;
  displayText: string;
}

/**
 * Resultado paginado de ClassTrib
 */
export interface ClassTribPagedResult {
  items: ClassTribDto[];
  totalItems: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

/**
 * Resultado de sincronização com API SVRS (específico para ClassTrib)
 */
export interface ClassTribSyncResult {
  sucesso: boolean;
  mensagem: string;
  totalApiSvrs: number;
  totalProcessado: number;
  dataHoraInicio: string;
  dataHoraFim: string;
  tempoDecorrido: string;
}

/**
 * Status da sincronização ClassTrib
 */
export interface ClassTribSyncStatus {
  dataUltimaSincronizacao?: string;
  totalClassificacoesApiSvrs: number;
  cacheAtivo: boolean;
  proximaSincronizacaoRecomendada?: string;
}
