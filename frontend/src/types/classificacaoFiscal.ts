// ============================================================================
// TIPOS PARA CLASSIFICAÇÃO FISCAL (NCM + ClassTrib SVRS)
// ============================================================================

/**
 * Interface principal de Classificação Fiscal
 * Campos VB6 originais + FK para ClassTrib (dados SVRS vêm via navigation)
 */
export interface ClassificacaoFiscal {
  // Campos básicos (originais do VB6)
  sequenciaDaClassificacao: number;
  ncm: number;
  descricaoDoNcm: string;
  porcentagemDoIpi: number;
  anexoDaReducao: number;
  aliquotaDoAnexo: number;
  produtoDiferido: boolean;
  reducaoDeBaseDeCalculo: boolean;
  inativo: boolean;
  iva: number;
  temConvenio: boolean;
  cest: string;
  unExterior: string;

  // FK para tabela ClassTrib (nova arquitetura)
  classTribId: number | null;
  
  // Dados do ClassTrib (vêm via Include/navigation no backend)
  classTrib?: ClassTribInfo | null;
}

/**
 * Dados do ClassTrib retornados via navigation property
 */
export interface ClassTribInfo {
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
 * Tipo para criação/atualização (sem o ID de sequência)
 */
export type ClassificacaoFiscalInput = Omit<ClassificacaoFiscal, 'sequenciaDaClassificacao'>;

/**
 * Resultado paginado da API
 */
export interface PagedResult<T> {
  items: T[];
  total: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

/**
 * Filtros para listagem
 */
export interface ClassificacaoFiscalFiltros {
  pageNumber?: number;
  pageSize?: number;
  ncm?: string;
  descricao?: string;
  somenteNFe?: boolean;
  incluirInativos?: boolean;
  tributacao?: 'todos' | 'vinculados' | 'pendentes';
}

/**
 * Resultado de sincronização com API SVRS
 */
export interface SyncResult {
  sucesso: boolean;
  mensagem: string;
  totalSincronizados: number;
  totalErros: number;
  erros: string[];
  dataHora?: string;
}

/**
 * Status da sincronização
 */
export interface SyncStatus {
  ultimaSincronizacao: string | null;
  totalRegistrosSincronizados: number;
  totalRegistrosManuais: number;
  totalRegistros: number;
  proximaSincronizacaoSugerida: string | null;
}

/**
 * Opções para o campo Tipo de Alíquota
 */
export const TIPOS_ALIQUOTA = [
  'Padrão',
  'Fixa',
  'Uniforme Nacional',
  'Uniforme Setorial',
  'Sem Alíquota',
] as const;

export type TipoAliquota = typeof TIPOS_ALIQUOTA[number];

/**
 * Opções para o campo Origem dos Dados
 */
export const ORIGENS_DADOS = ['MANUAL', 'SVRS'] as const;

export type OrigemDados = typeof ORIGENS_DADOS[number];

/**
 * Opções para Anexo da Redução
 * No banco VB6: 0 = Anexo 1, 1 = Anexo 2
 */
export const ANEXOS_REDUCAO = [0, 1] as const;

/**
 * Mapeamento de Alíquota do Anexo
 * No banco VB6: 0 = 12%, 1 = 18%, 2 = 7%
 * Índice no DB → Valor percentual
 */
export const ALIQUOTA_DB_TO_PERCENT: Record<number, number> = {
  0: 12,
  1: 18,
  2: 7,
};

export const ALIQUOTA_PERCENT_TO_DB: Record<number, number> = {
  12: 0,
  18: 1,
  7: 2,
};

/**
 * Opções para Alíquota do Anexo (valores percentuais para exibição)
 */
export const ALIQUOTAS_ANEXO = [7, 12, 18] as const;

/**
 * Converte valor do banco para percentual
 */
export function aliquotaDbToPercent(dbValue: number): number {
  return ALIQUOTA_DB_TO_PERCENT[dbValue] ?? 12;
}

/**
 * Converte percentual para valor do banco
 */
export function aliquotaPercentToDb(percent: number): number {
  return ALIQUOTA_PERCENT_TO_DB[percent] ?? 0;
}

/**
 * Valores padrão para novo registro
 */
export const DEFAULT_CLASSIFICACAO_FISCAL: ClassificacaoFiscalInput = {
  ncm: 0,
  descricaoDoNcm: '',
  porcentagemDoIpi: 0,
  anexoDaReducao: 0, // 0 no DB = Anexo 1
  aliquotaDoAnexo: 0, // 0 no DB = 12%
  produtoDiferido: false,
  reducaoDeBaseDeCalculo: false,
  inativo: false,
  iva: 0,
  temConvenio: false,
  cest: '',
  unExterior: '',
  classTribId: null,
};
