// Types para Nota Fiscal

// ================================
// DTOs para Listagem
// ================================

export interface NotaFiscalListDto {
  sequenciaDaNotaFiscal: number;
  numeroDaNfe: number;
  numeroDaNotaFiscal: number;
  dataDeEmissao: string | null;
  
  // Cliente
  sequenciaDoGeral: number;
  nomeDoCliente: string;
  documentoCliente: string;
  
  // Natureza
  sequenciaDaNatureza: number;
  descricaoNatureza: string;
  
  // Propriedade
  sequenciaDaPropriedade: number;
  nomePropriedade: string;
  
  // Valores
  valorTotalDaNotaFiscal: number;
  valorTotalDosProdutos: number;
  valorTotalDoIcms: number;
  
  // Status
  notaCancelada: boolean;
  transmitido: boolean;
  autorizado: boolean;
  chaveDeAcessoDaNfe: string;
  
  // Tipo
  tipoDeNota: number;
  tipoDeNotaDescricao: string;
  nfeComplementar: boolean;
  notaDeDevolucao: boolean;
}

// ================================
// DTO Completo
// ================================

export interface NotaFiscalDto {
  // Identificação
  sequenciaDaNotaFiscal: number;
  numeroDaNfe: number;
  numeroDaNfse: number;
  numeroDaNotaFiscal: number;
  
  // Datas
  dataDeEmissao: string | null;
  dataDeSaida: string | null;
  horaDaSaida: string | null;
  
  // Cliente
  sequenciaDoGeral: number;
  nomeDoCliente: string;
  documentoCliente: string;
  enderecoCliente: string;
  cidadeCliente: string;
  ufCliente: string;
  inscricaoCliente: string;
  
  // Propriedade
  sequenciaDaPropriedade: number;
  nomePropriedade: string;
  
  // Natureza
  sequenciaDaNatureza: number;
  descricaoNatureza: string;
  cfopNatureza: string;
  
  // Classificação
  sequenciaDaClassificacao: number;
  descricaoClassificacao: string;
  
  // Cobrança
  sequenciaDaCobranca: number;
  descricaoCobranca: string;
  
  // Transportadora
  transportadoraAvulsa: boolean;
  sequenciaDaTransportadora: number;
  nomeTransportadora: string;
  nomeDaTransportadoraAvulsa: string;
  documentoDaTransportadora: string;
  ieDaTransportadora: string;
  enderecoDaTransportadora: string;
  municipioDaTransportadora: number;
  nomeMunicipioTransportadora: string;
  placaDoVeiculo: string;
  ufDoVeiculo: string;
  codigoDaAntt: string;
  
  // Frete
  frete: string | null;
  valorDoFrete: number;
  
  // Volumes
  volume: number;
  especie: string;
  marca: string;
  numeracao: string;
  pesoBruto: number;
  pesoLiquido: number;
  
  // Valores Totais
  valorTotalDosProdutos: number;
  valorTotalDosConjuntos: number;
  valorTotalDasPecas: number;
  valorTotalDosServicos: number;
  valorTotalDaNotaFiscal: number;
  
  // Impostos
  valorTotalDaBaseDeCalculo: number;
  valorTotalDoIcms: number;
  valorTotalDaBaseSt: number;
  valorTotalDoIcmsSt: number;
  valorTotalIpiDosProdutos: number;
  valorTotalIpiDosConjuntos: number;
  valorTotalIpiDasPecas: number;
  valorTotalDoPis: number;
  valorTotalDoCofins: number;
  valorDoImpostoDeRenda: number;
  aliquotaDoIss: number;
  reterIss: boolean;
  valorTotalDoTributo: number;
  valorTotalDaImportacao: number;
  
  // IBS/CBS (Reforma Tributária)
  valorTotalIbs: number;
  valorTotalCbs: number;
  
  // Outros Valores
  valorDoSeguro: number;
  outrasDespesas: number;
  
  // Valores Usados
  valorTotalDeProdutosUsados: number;
  valorTotalConjuntosUsados: number;
  valorTotalDasPecasUsadas: number;
  
  // Pagamento
  formaDePagamento: string;
  contraApresentacao: boolean;
  fechamento: number;
  valorDoFechamento: number;
  
  // Observações
  historico: string;
  observacao: string;
  
  // Tipo e Status
  tipoDeNota: number;
  notaCancelada: boolean;
  canceladaNoLivro: boolean;
  notaFiscalAvulsa: boolean;
  ocultarValorUnitario: boolean;
  
  // NFe
  transmitido: boolean;
  autorizado: boolean;
  imprimiu: boolean;
  chaveDeAcessoDaNfe: string;
  protocoloDeAutorizacaoNfe: string;
  dataEHoraDaNfe: string;
  numeroDoReciboDaNfe: string;
  
  // NFe Referenciada/Complementar/Devolução
  nfeComplementar: boolean;
  chaveAcessoNfeReferenciada: string;
  notaDeDevolucao: boolean;
  chaveDaDevolucao: string;
  chaveDaDevolucao2: string;
  chaveDaDevolucao3: string;
  finNfe: number;
  novoLayout: boolean;
  
  // NFSe
  reciboNfse: string;
  
  // Conjunto Avulso
  conjuntoAvulso: boolean;
  descricaoConjuntoAvulso: string;
  
  // Relacionamentos
  sequenciaDoPedido: number;
  sequenciaDoVendedor: number;
  nomeVendedor: string;
  sequenciaDoMovimento: number;
  numeroDoContrato: number;
  notaDeVenda: number;
  refaturamento: boolean;
  financiamento: boolean;
  
  // Itens
  produtos: ProdutoDaNotaFiscalDto[];
  conjuntos: ConjuntoDaNotaFiscalDto[];
  pecas: PecaDaNotaFiscalDto[];
  servicos: ServicoDaNotaFiscalDto[];
  parcelas: ParcelaNotaFiscalDto[];
}

// ================================
// DTOs de Itens
// ================================

export interface ProdutoDaNotaFiscalDto {
  sequenciaDoProdutoDaNotaFiscal: number;
  sequenciaDoProduto: number;
  descricaoProduto: string;
  unidade: string;
  ncm: string;
  cfop: string;
  cst: string;
  
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  desconto: number;
  
  // ICMS
  baseDeCalculoIcms: number;
  aliquotaIcms: number;
  valorIcms: number;
  
  // ICMS ST
  baseDeCalculoSt: number;
  aliquotaSt: number;
  valorIcmsSt: number;
  
  // IPI
  baseDeCalculoIpi: number;
  aliquotaIpi: number;
  valorIpi: number;
  
  // PIS/COFINS
  aliquotaPis: number;
  valorPis: number;
  aliquotaCofins: number;
  valorCofins: number;
  
  // IBS/CBS (Reforma Tributária)
  valorIbs: number;
  valorCbs: number;
  
  usado: boolean;
  informacoesAdicionais: string;
}

export interface ConjuntoDaNotaFiscalDto {
  sequenciaDoConjuntoDaNotaFiscal: number;
  sequenciaDoConjunto: number;
  descricaoConjunto: string;
  
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  valorIpi: number;
  
  // IBS/CBS (Reforma Tributária)
  valorIbs: number;
  valorCbs: number;
  
  usado: boolean;
}

export interface PecaDaNotaFiscalDto {
  sequenciaDaPecaDaNotaFiscal: number;
  sequenciaDaPeca: number;
  descricaoPeca: string;
  
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  valorIpi: number;
  
  // IBS/CBS (Reforma Tributária)
  valorIbs: number;
  valorCbs: number;
  
  usado: boolean;
}

export interface ServicoDaNotaFiscalDto {
  sequenciaDoServicoDaNotaFiscal: number;
  sequenciaDoServico: number;
  descricaoServico: string;
  
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  
  aliquotaIss: number;
  valorIss: number;
}

export interface ParcelaNotaFiscalDto {
  sequenciaDaParcela: number;
  numeroDaParcela: number;
  dataDeVencimento: string | null;
  valor: number;
  pago: boolean;
}

// ================================
// DTOs de Criação de Itens
// ================================

export interface ProdutoDaNotaFiscalCreateDto {
  sequenciaDoProduto: number;
  quantidade: number;
  valorUnitario: number;
  desconto?: number;
  
  // ICMS
  cstIcms?: string;
  aliquotaIcms?: number;
  baseDeCalculoIcms?: number;
  valorIcms?: number;
  
  // ICMS ST
  cstSt?: string;
  aliquotaSt?: number;
  baseDeCalculoSt?: number;
  valorIcmsSt?: number;
  
  // IPI
  cstIpi?: string;
  aliquotaIpi?: number;
  baseDeCalculoIpi?: number;
  valorIpi?: number;
  
  // PIS
  cstPis?: string;
  aliquotaPis?: number;
  valorPis?: number;
  
  // COFINS
  cstCofins?: string;
  aliquotaCofins?: number;
  valorCofins?: number;
  
  // Importação
  valorDaImportacao?: number;
  valorDoIof?: number;
  
  usado?: boolean;
  informacoesAdicionais?: string;
}

export interface ConjuntoDaNotaFiscalCreateDto {
  sequenciaDoConjunto: number;
  quantidade: number;
  valorUnitario: number;
  desconto?: number;
  
  // ICMS
  cstIcms?: string;
  aliquotaIcms?: number;
  baseDeCalculoIcms?: number;
  valorIcms?: number;
  
  // ICMS ST
  cstSt?: string;
  aliquotaSt?: number;
  baseDeCalculoSt?: number;
  valorIcmsSt?: number;
  
  // IPI
  cstIpi?: string;
  aliquotaIpi?: number;
  baseDeCalculoIpi?: number;
  valorIpi?: number;
  
  // PIS
  cstPis?: string;
  aliquotaPis?: number;
  valorPis?: number;
  
  // COFINS
  cstCofins?: string;
  aliquotaCofins?: number;
  valorCofins?: number;
  
  // Importação
  valorDaImportacao?: number;
  valorDoIof?: number;
  
  usado?: boolean;
  informacoesAdicionais?: string;
}

export interface PecaDaNotaFiscalCreateDto {
  sequenciaDaPeca: number;
  quantidade: number;
  valorUnitario: number;
  desconto?: number;
  
  // ICMS
  cstIcms?: string;
  aliquotaIcms?: number;
  baseDeCalculoIcms?: number;
  valorIcms?: number;
  
  // ICMS ST
  cstSt?: string;
  aliquotaSt?: number;
  baseDeCalculoSt?: number;
  valorIcmsSt?: number;
  
  // IPI
  cstIpi?: string;
  aliquotaIpi?: number;
  baseDeCalculoIpi?: number;
  valorIpi?: number;
  
  // PIS
  cstPis?: string;
  aliquotaPis?: number;
  valorPis?: number;
  
  // COFINS
  cstCofins?: string;
  aliquotaCofins?: number;
  valorCofins?: number;
  
  // Importação
  valorDaImportacao?: number;
  valorDoIof?: number;
  
  usado?: boolean;
  informacoesAdicionais?: string;
}

export interface ParcelaNotaFiscalCreateDto {
  numeroDaParcela: number;
  dias?: number;
  dataDeVencimento?: string | null;
  valor: number;
}

// Combos para seleção de itens
export interface ProdutoComboDto {
  sequenciaDoProduto: number;
  descricao: string;
  ncm: string;
  unidade: string;
  cfop: string;
  precoVenda: number;
  codigoDeBarras?: string;
  valorTotal?: number;
  quantidadeNoEstoque?: number;
}

export interface ConjuntoComboDto {
  sequenciaDoConjunto: number;
  descricao: string;
  precoVenda: number;
}

export interface PecaComboDto {
  sequenciaDaPeca: number;
  descricao: string;
  precoVenda: number;
  ncm: string;
  unidade: string;
}

// ================================
// DTOs de Criação/Atualização
// ================================

export interface NotaFiscalCreateUpdateDto {
  numeroDaNotaFiscal?: number;
  
  // Datas
  dataDeEmissao: string;
  dataDeSaida?: string | null;
  horaDaSaida?: string | null;
  
  // Cliente
  sequenciaDoGeral: number;
  
  // Propriedade
  sequenciaDaPropriedade: number;
  
  // Natureza
  sequenciaDaNatureza: number;
  
  // Classificação
  sequenciaDaClassificacao?: number;
  
  // Cobrança
  sequenciaDaCobranca?: number;
  
  // Transportadora
  transportadoraAvulsa?: boolean;
  sequenciaDaTransportadora?: number;
  nomeDaTransportadoraAvulsa?: string;
  documentoDaTransportadora?: string;
  ieDaTransportadora?: string;
  enderecoDaTransportadora?: string;
  municipioDaTransportadora?: number;
  placaDoVeiculo?: string;
  ufDoVeiculo?: string;
  codigoDaAntt?: string;
  
  // Frete
  frete?: string | null;
  valorDoFrete?: number;
  
  // Volumes
  volume?: number;
  especie?: string;
  marca?: string;
  numeracao?: string;
  pesoBruto?: number;
  pesoLiquido?: number;
  
  // Pagamento
  formaDePagamento?: string;
  contraApresentacao?: boolean;
  fechamento?: number;
  valorDoFechamento?: number;
  
  // Observações
  historico?: string;
  observacao?: string;
  
  // Tipo e Flags
  tipoDeNota?: number;
  notaFiscalAvulsa?: boolean;
  ocultarValorUnitario?: boolean;
  
  // Outros valores
  valorDoSeguro?: number;
  outrasDespesas?: number;
  
  // NFe Complementar/Devolução
  nfeComplementar?: boolean;
  chaveAcessoNfeReferenciada?: string;
  notaDeDevolucao?: boolean;
  chaveDaDevolucao?: string;
  chaveDaDevolucao2?: string;
  chaveDaDevolucao3?: string;
  
  // Conjunto Avulso
  conjuntoAvulso?: boolean;
  descricaoConjuntoAvulso?: string;
  
  // Relacionamentos
  sequenciaDoPedido?: number;
  sequenciaDoVendedor?: number;
  numeroDoContrato?: number;
  refaturamento?: boolean;
  financiamento?: boolean;
}

// ================================
// DTOs de Filtros
// ================================

export interface NotaFiscalFiltroDto {
  busca?: string;
  dataInicial?: string;
  dataFinal?: string;
  cliente?: number;
  natureza?: number;
  propriedade?: number;
  tipoDeNota?: number;
  canceladas?: boolean;
  transmitidas?: boolean;
  autorizadas?: boolean;
  numeroDaNfe?: number;
  
  // Paginação
  pageNumber?: number;
  pageSize?: number;
}

// ================================
// DTOs de Combos
// ================================

export interface ClienteComboDto {
  sequenciaDoGeral: number;
  nome: string;
  documento: string;
  cidade: string;
  uf: string;
}

export interface NaturezaOperacaoComboDto {
  sequenciaDaNatureza: number;
  descricao: string;
  cfop: string;
  entradaSaida: boolean;
}

export interface PropriedadeComboDto {
  sequenciaDaPropriedade: number;
  nome: string;
  cnpj: string;
}

export interface TransportadoraComboDto {
  sequenciaDoGeral: number;
  nome: string;
  documento: string;
  cidade: string;
  uf: string;
}

export interface TipoCobrancaComboDto {
  sequenciaDaCobranca: number;
  descricao: string;
}

export interface VendedorComboDto {
  sequenciaDoGeral: number;
  nome: string;
}

// ================================
// DTOs de Ações NFe
// ================================

export interface CancelarNfeDto {
  sequenciaDaNotaFiscal: number;
  justificativa: string;
}

export interface CartaCorrecaoDto {
  sequenciaDaNotaFiscal: number;
  correcao: string;
}

// ================================
// Tipos Auxiliares
// ================================

// PagedResult importado de Common
export type { PagedResult } from '../Common/common';

// Tipos de Nota
export const TIPOS_NOTA = {
  0: 'Saída',
  1: 'Entrada',
  2: 'Serviço',
} as const;

export type TipoNota = keyof typeof TIPOS_NOTA;

// Modalidades de Frete NFe
export const MODALIDADES_FRETE = {
  '0': 'Por conta do emitente',
  '1': 'Por conta do destinatário',
  '2': 'Por conta de terceiros',
  '3': 'Transporte próprio por conta do remetente',
  '4': 'Transporte próprio por conta do destinatário',
  '9': 'Sem frete',
} as const;

// Status NFe
export const STATUS_NFE = {
  NAO_ENVIADA: 'Não enviada',
  ENVIADA: 'Enviada',
  AUTORIZADA: 'Autorizada',
  CANCELADA: 'Cancelada',
  DENEGADA: 'Denegada',
} as const;

// Finalidades NFe
export const FINALIDADES_NFE = {
  1: 'Normal',
  2: 'Complementar',
  3: 'Ajuste',
  4: 'Devolução',
} as const;

// Helper functions
export function getStatusNfe(nota: NotaFiscalListDto | NotaFiscalDto): string {
  if (nota.notaCancelada) return STATUS_NFE.CANCELADA;
  if (nota.autorizado) return STATUS_NFE.AUTORIZADA;
  if (nota.transmitido) return STATUS_NFE.ENVIADA;
  return STATUS_NFE.NAO_ENVIADA;
}

export function getStatusNfeColor(nota: NotaFiscalListDto | NotaFiscalDto): string {
  if (nota.notaCancelada) return 'red';
  if (nota.autorizado) return 'green';
  if (nota.transmitido) return 'blue';
  return 'gray';
}

export function formatChaveAcesso(chave: string): string {
  if (!chave || chave.length !== 44) return chave;
  return chave.replace(/(\d{4})(?=\d)/g, '$1 ');
}

// ================================
// Cálculo de Impostos
// ================================

/**
 * Tipo do item: 1=Produto, 2=Conjunto, 3=Peça
 */
export type TipoItem = 1 | 2 | 3;

/**
 * Request para cálculo de impostos
 */
export interface CalculoImpostoRequestDto {
  tipoItem: TipoItem;
  sequenciaDoItem: number;
  quantidade: number;
  valorUnitario: number;
  desconto: number;
  valorFrete: number;
}

/**
 * Resultado do cálculo de impostos
 */
export interface CalculoImpostoResultDto {
  valorTotal: number;
  
  // CFOP e CST
  cfop: string;
  cst: string;
  
  // ICMS
  baseCalculoICMS: number;
  aliquotaICMS: number;
  valorICMS: number;
  percentualReducao: number;
  diferido: boolean;
  
  // IPI
  aliquotaIPI: number;
  valorIPI: number;
  
  // PIS
  baseCalculoPIS: number;
  aliquotaPIS: number;
  valorPIS: number;
  
  // COFINS
  baseCalculoCOFINS: number;
  aliquotaCOFINS: number;
  valorCOFINS: number;
  
  // Substituição Tributária
  iva: number;
  baseCalculoST: number;
  aliquotaICMSST: number;
  valorICMSST: number;
  
  // IBS/CBS
  valorIBS: number;
  valorCBS: number;
  
  // Totais
  valorTributo: number;
  
  // Info adicional
  ncm: string;
  unidade: string;
  descricaoItem: string;
}
