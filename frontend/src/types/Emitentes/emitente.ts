// Types para Emitente

export interface EmitenteListDto {
  id: number;
  cnpj: string;
  razaoSocial: string;
  nomeFantasia?: string;
  inscricaoEstadual: string;
  municipio: string;
  uf: string;
  ativo: boolean;
  ambienteNfe: number;
  ambienteNfeDescricao: string;
}

export interface EmitenteDto {
  id: number;
  cnpj: string;
  razaoSocial: string;
  nomeFantasia?: string;
  inscricaoEstadual: string;
  inscricaoMunicipal?: string;
  cnae?: string;
  codigoRegimeTributario?: number;
  codigoRegimeTributarioDescricao: string;
  
  // Endereço
  endereco: string;
  numero: string;
  complemento?: string;
  bairro: string;
  codigoMunicipio: string;
  municipio: string;
  uf: string;
  cep: string;
  codigoPais: string;
  pais: string;
  
  // Contato
  telefone?: string;
  email?: string;
  
  // NFe
  ambienteNfe: number;
  ambienteNfeDescricao: string;
  serieNfe: number;
  proximoNumeroNfe: number;
  caminhoCertificado?: string;
  validadeCertificado?: string;
  
  // Controle
  ativo: boolean;
  dataCriacao: string;
  dataAtualizacao?: string;
  dataConsultaCnpj?: string;
}

export interface EmitenteCreateUpdateDto {
  cnpj: string;
  razaoSocial: string;
  nomeFantasia?: string;
  inscricaoEstadual: string;
  inscricaoMunicipal?: string;
  cnae?: string;
  codigoRegimeTributario?: number;
  
  // Endereço
  endereco: string;
  numero: string;
  complemento?: string;
  bairro: string;
  codigoMunicipio: string;
  municipio: string;
  uf: string;
  cep: string;
  codigoPais?: string;
  pais?: string;
  
  // Contato
  telefone?: string;
  email?: string;
  
  // NFe
  ambienteNfe: number;
  serieNfe: number;
  proximoNumeroNfe: number;
  caminhoCertificado?: string;
  senhaCertificado?: string;
  validadeCertificado?: string;
  
  // Controle
  ativo: boolean;
}

export interface ConsultaCnpjDto {
  cnpj: string;
  razaoSocial: string;
  nomeFantasia?: string;
  endereco: string;
  numero: string;
  complemento?: string;
  bairro: string;
  codigoMunicipio: string;
  municipio: string;
  uf: string;
  cep: string;
  telefone?: string;
  email?: string;
  atividadePrincipal?: string;
  cnae?: string;
  situacao?: string;
  dataConsulta: string;
}

// Parâmetros do Sistema
export interface ParametrosDto {
  diretorioFotosProdutos?: string;
  diretorioFotosConjuntos?: string;
  diretorioDasFotos?: string;
  diretorioDesenhoTec?: string;
  caminhoAtualizacao?: string;
  caminhoAtualizacao2?: string;
  nomeDoServidor?: string;
}

// Opções de Regime Tributário
export const REGIMES_TRIBUTARIOS = [
  { value: 1, label: 'Simples Nacional' },
  { value: 2, label: 'Simples Nacional - Excesso de sublimite' },
  { value: 3, label: 'Regime Normal' },
];

// Opções de Ambiente NFe
export const AMBIENTES_NFE = [
  { value: 1, label: 'Produção' },
  { value: 2, label: 'Homologação' },
];

// Lista de UFs
export const UFS = [
  'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA',
  'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN',
  'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'
];
