// Types para Produtos

// DTO para listagem de produtos
export interface ProdutoListDto {
  sequenciaDoProduto: number;
  descricao: string;
  codigoDeBarras: string;
  grupoProduto: string;
  subGrupoProduto: string;
  unidade: string;
  quantidadeNoEstoque: number; // DEPRECATED: Usar quantidadeContabil
  quantidadeContabil: number; // Estoque Contábil (correto)
  quantidadeMinima: number;
  valorDeCusto: number;
  valorTotal: number;
  margemDeLucro: number;
  localizacao: string;
  inativo: boolean;
  eMateriaPrima: boolean;
  ultimaCompra: string | null;
  ultimoMovimento: string | null;
  estoqueCritico: boolean;
}

// DTO completo para visualização/edição
export interface ProdutoDto {
  sequenciaDoProduto: number;
  descricao: string;
  codigoDeBarras: string;
  
  // Classificação
  sequenciaDoGrupoProduto: number;
  grupoProduto: string;
  sequenciaDoSubGrupoProduto: number;
  subGrupoProduto: string;
  sequenciaDaUnidade: number;
  unidade: string;
  sequenciaDaClassificacao: number;
  classificacaoFiscal: string;
  ncm: string;
  percentualIpi: number; // % IPI da classificação fiscal
  
  // Indicadores de vínculo ClassTrib (IBS/CBS)
  temClassTrib: boolean; // Indica se a Classificação Fiscal tem ClassTrib vinculado
  codigoClassTrib: string; // Código ClassTrib se vinculado
  descricaoClassTrib: string; // Descrição do ClassTrib se vinculado
  
  // Estoque
  quantidadeNoEstoque: number;
  quantidadeMinima: number;
  quantidadeContabil: number;
  quantidadeFisica: number;
  localizacao: string;
  
  // Valores
  valorDeCusto: number;
  margemDeLucro: number;
  valorTotal: number;
  custoMedio: number;
  valorDeLista: number;
  valorContabilAtual: number;
  
  // Características
  eMateriaPrima: boolean;
  tipoDoProduto: number;
  peso: number;
  pesoOk: boolean; // Peso Conferido
  medida: string;
  medidaFinal: string;
  industrializacao: boolean;
  importado: boolean;
  materialAdquiridoDeTerceiro: boolean;
  sucata: boolean;
  obsoleto: boolean;
  
  // Flags
  inativo: boolean;
  usado: boolean;
  usadoNoProjeto: boolean;
  lance: boolean;
  eRegulador: boolean;
  travaReceita: boolean;
  receitaConferida: boolean;
  naoSairNoRelatorio: boolean;
  naoSairNoChecklist: boolean;
  mostrarReceitaSecundaria: boolean;
  naoMostrarReceita: boolean;
  conferidoPeloContabil: boolean; // NCM Conferido pela Contabilidade
  mpInicial: boolean; // M.Prima Inicial
  
  // Datas
  ultimaCompra: string | null;
  ultimoMovimento: string | null;
  ultimaCotacao: string | null; // Últ. Cotação
  dataDaContagem: string | null;
  dataDaAlteracao: string | null;
  
  // Outros
  ultimoFornecedor: number;
  nomeFornecedor: string;
  parteDoPivo: string;
  modeloDoLance: number;
  detalhes: string;
  usuarioDaAlteracao: string;
  
  // Controle de produção
  separadoMontar: number;
  compradosAguardando: number;
  
  // Último Balanço (aba Contabilidade)
  quantidadeBalanco: number;
}

// DTO para criação/atualização
export interface ProdutoCreateUpdateDto {
  descricao: string;
  codigoDeBarras: string;
  sequenciaDoGrupoProduto: number;
  sequenciaDoSubGrupoProduto: number;
  sequenciaDaUnidade: number;
  sequenciaDaClassificacao: number;
  quantidadeMinima: number;
  localizacao: string;
  valorDeCusto: number;
  margemDeLucro: number;
  valorTotal: number;
  valorDeLista: number;
  eMateriaPrima: boolean;
  tipoDoProduto: number;
  peso: number;
  pesoOk: boolean; // Peso Conferido
  medida: string;
  medidaFinal: string;
  industrializacao: boolean;
  importado: boolean;
  materialAdquiridoDeTerceiro: boolean;
  sucata: boolean;
  obsoleto: boolean;
  inativo: boolean;
  usado: boolean;
  usadoNoProjeto: boolean;
  lance: boolean;
  eRegulador: boolean;
  travaReceita: boolean;
  naoSairNoRelatorio: boolean;
  naoSairNoChecklist: boolean;
  mostrarReceitaSecundaria: boolean;
  naoMostrarReceita: boolean;
  conferidoPeloContabil: boolean; // NCM Conferido pela Contabilidade
  mpInicial: boolean; // M.Prima Inicial
  parteDoPivo: string;
  modeloDoLance: number;
  detalhes: string;
}

// DTO para filtros de busca
export interface ProdutoFiltroDto {
  busca?: string;
  grupoProduto?: number;
  subGrupoProduto?: number;
  eMateriaPrima?: boolean;
  estoqueCritico?: boolean;
  incluirInativos?: boolean;
  pageNumber?: number;
  pageSize?: number;
}

// ProdutoComboDto importado de NotaFiscal (versao mais completa)
export type { ProdutoComboDto } from '../NotaFiscal/notaFiscal';

// PagedResult importado de Common
export type { PagedResult } from '../Common/common';

// ============================================================================
// RECEITA DO PRODUTO (MATERIA PRIMA)
// ============================================================================

// DTO para listagem de itens da receita do produto (materias primas)
export interface ReceitaProdutoListDto {
  sequenciaDoProduto: number;
  sequenciaDaMateriaPrima: number;
  descricaoDaMateriaPrima: string;
  quantidade: number;
  unidade: string;
  peso: number;
  valorDeCusto: number;
  custoTotal: number;
  pesoTotal: number;
}

// DTO para criacao/atualizacao de item da receita
export interface ReceitaProdutoCreateUpdateDto {
  sequenciaDaMateriaPrima: number;
  quantidade: number;
}

// DTO para retorno completo da receita do produto
export interface ReceitaProdutoDto {
  sequenciaDoProduto: number;
  descricaoDoProduto: string;
  itens: ReceitaProdutoListDto[];
  totalItens: number;
  custoTotalReceita: number;
  pesoTotalReceita: number;
}

// ============================================================================
// TIPOS DE PRODUTO
// ============================================================================

// Tipos de Produto (conforme o campo TipoDoProduto)
export const TIPO_PRODUTO = {
  0: 'Produto Acabado',
  1: 'Matéria Prima',
  2: 'Serviço',
  3: 'Produto Intermediário',
} as const;

export type TipoProduto = keyof typeof TIPO_PRODUTO;
