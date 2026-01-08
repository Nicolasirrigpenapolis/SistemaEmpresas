// ============================================================================
// TIPOS DO CADASTRO GERAL (Clientes, Fornecedores, Transportadoras, Vendedores)
// ============================================================================

/**
 * DTO para listagem simplificada
 */
export interface GeralListDto {
  sequenciaDoGeral: number;
  razaoSocial: string;
  nomeFantasia: string;
  cpfECnpj: string;
  cidade: string;
  uf: string;
  fone1: string;
  email: string;
  cliente: boolean;
  fornecedor: boolean;
  transportadora: boolean;
  vendedor: boolean;
  inativo: boolean;
  tipo: number; // 0=PF, 1=PJ
}

/**
 * DTO para criação
 */
export interface GeralCreateDto {
  // Tipos (flags)
  cliente: boolean;
  fornecedor: boolean;
  despesa: boolean;
  imposto: boolean;
  transportadora: boolean;
  vendedor: boolean;

  // Dados Principais
  razaoSocial: string;
  nomeFantasia: string;
  tipo: number; // 0=PF, 1=PJ
  cpfECnpj: string;
  rgEIe: string;
  codigoDoSuframa: string;
  codigoDaAntt: string;

  // Endereço
  endereco: string;
  numeroDoEndereco: string;
  complemento: string;
  bairro: string;
  caixaPostal: string;
  sequenciaDoMunicipio: number;
  cep: string;
  sequenciaDoPais: number;

  // Contato
  fone1: string;
  fone2: string;
  fax: string;
  celular: string;
  contato: string;
  email: string;
  homePage: string;

  // Observações
  observacao: string;

  // Flags fiscais
  revenda: boolean;
  isento: boolean;
  orgonPublico: boolean;
  empresaProdutor: boolean;
  cumulativo: boolean;
  inativo: boolean;

  // Vendedor associado
  sequenciaDoVendedor: number;
  intermediarioDoVendedor: string;

  // Dados de Cobrança (aba 2)
  enderecoDeCobranca: string;
  numeroDoEnderecoDeCobranca: string;
  complementoDaCobranca: string;
  bairroDeCobranca: string;
  caixaPostalDaCobranca: string;
  sequenciaMunicipioCobranca: number;
  cepDeCobranca: string;

  // Dados Bancários (aba 2)
  nomeDoBanco1: string;
  agenciaDoBanco1: string;
  contaCorrenteDoBanco1: string;
  nomeDoCorrentistaDoBanco1: string;
  nomeDoBanco2: string;
  agenciaDoBanco2: string;
  contaCorrenteDoBanco2: string;
  nomeDoCorrentistaDoBanco2: string;

  // Dados adicionais
  dataDeNascimento?: string;
  codigoContabil: number;
  codigoAdiantamento: number;
  salBruto: number;
  whatsAppSincronizado: boolean;
}

/**
 * DTO para atualização
 */
export interface GeralUpdateDto extends GeralCreateDto {
  sequenciaDoGeral: number;
}

/**
 * DTO detalhado (resposta da API)
 */
export interface GeralDetailDto extends GeralUpdateDto {
  dataDoCadastro?: string;
  usuDaAlteracao: string;
  municipioNome: string;
  municipioUf: string;
  municipioCobrancaNome: string;
  municipioCobrancaUf: string;
  vendedorNome: string;
}

// PagedResult importado de Common
export type { PagedResult } from '../Common/common';

/**
 * Filtros para listagem
 */
export interface GeralFiltros {
  pageNumber?: number;
  pageSize?: number;
  busca?: string;
  cliente?: boolean;
  fornecedor?: boolean;
  transportadora?: boolean;
  vendedor?: boolean;
  incluirInativos?: boolean;
}

/**
 * Tipos de entidade para navegação
 */
export type TipoEntidade = 'todos' | 'cliente' | 'fornecedor' | 'transportadora' | 'vendedor';

/**
 * Labels para os tipos
 */
export const TIPO_LABELS: Record<TipoEntidade, string> = {
  todos: 'Todos',
  cliente: 'Clientes',
  fornecedor: 'Fornecedores',
  transportadora: 'Transportadoras',
  vendedor: 'Vendedores',
};

/**
 * Cores para badges de tipo
 */
export const TIPO_CORES: Record<string, string> = {
  cliente: 'bg-blue-100 text-blue-800',
  fornecedor: 'bg-green-100 text-green-800',
  transportadora: 'bg-purple-100 text-purple-800',
  vendedor: 'bg-orange-100 text-orange-800',
  despesa: 'bg-red-100 text-red-800',
  imposto: 'bg-yellow-100 text-yellow-800',
};

/**
 * Siglas curtas para badges compactos
 */
export const TIPO_SIGLAS: Record<string, string> = {
  cliente: 'CLI',
  fornecedor: 'FOR',
  transportadora: 'TRANSP',
  vendedor: 'VEN',
  despesa: 'DES',
  imposto: 'IMP',
};

/**
 * Retorna os tipos ativos de um registro
 */
export function getTiposAtivos(item: GeralListDto): string[] {
  const tipos: string[] = [];
  if (item.cliente) tipos.push('cliente');
  if (item.fornecedor) tipos.push('fornecedor');
  if (item.transportadora) tipos.push('transportadora');
  if (item.vendedor) tipos.push('vendedor');
  return tipos;
}

/**
 * Formata o tipo pessoa
 */
export function formatTipoPessoa(tipo: number): string {
  return tipo === 0 ? 'Pessoa Física' : 'Pessoa Jurídica';
}

/**
 * Formata o label CPF/CNPJ
 */
export function getLabelDocumento(tipo: number): string {
  return tipo === 0 ? 'CPF' : 'CNPJ';
}

/**
 * Formata o label RG/IE
 */
export function getLabelIdentidade(tipo: number): string {
  return tipo === 0 ? 'RG' : 'I.E.';
}

/**
 * Valores padrão para novo registro
 */
export const GERAL_DEFAULT: GeralCreateDto = {
  cliente: false,
  fornecedor: false,
  despesa: false,
  imposto: false,
  transportadora: false,
  vendedor: false,
  razaoSocial: '',
  nomeFantasia: '',
  tipo: 1, // Jurídica por padrão
  cpfECnpj: '',
  rgEIe: '',
  codigoDoSuframa: '',
  codigoDaAntt: '',
  endereco: '',
  numeroDoEndereco: '',
  complemento: '',
  bairro: '',
  caixaPostal: '',
  sequenciaDoMunicipio: 0,
  cep: '',
  sequenciaDoPais: 1,
  fone1: '',
  fone2: '',
  fax: '',
  celular: '',
  contato: '',
  email: '',
  homePage: '',
  observacao: '',
  revenda: false,
  isento: false,
  orgonPublico: false,
  empresaProdutor: false,
  cumulativo: false,
  inativo: false,
  sequenciaDoVendedor: 0,
  intermediarioDoVendedor: '',
  enderecoDeCobranca: '',
  numeroDoEnderecoDeCobranca: '',
  complementoDaCobranca: '',
  bairroDeCobranca: '',
  caixaPostalDaCobranca: '',
  sequenciaMunicipioCobranca: 0,
  cepDeCobranca: '',
  nomeDoBanco1: '',
  agenciaDoBanco1: '',
  contaCorrenteDoBanco1: '',
  nomeDoCorrentistaDoBanco1: '',
  nomeDoBanco2: '',
  agenciaDoBanco2: '',
  contaCorrenteDoBanco2: '',
  nomeDoCorrentistaDoBanco2: '',
  codigoContabil: 0,
  codigoAdiantamento: 0,
  salBruto: 0,
  whatsAppSincronizado: false,
};
