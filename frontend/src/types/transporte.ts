// ==========================================
// TIPOS - MÓDULO DE TRANSPORTE
// ==========================================

// ==========================================
// VEÍCULO
// ==========================================
export interface VeiculoDto {
  id: number;
  placa: string;
  placaReboque?: string;
  marca?: string;
  modelo?: string;
  anoFabricacao?: number;
  anoModelo?: number;
  tara: number;
  capacidadeKg?: number;
  tipoRodado: string; // Caminhão, Carreta, etc
  tipoCarroceria: string; // Aberta, Fechada, etc
  uf: string; // UF de registro
  renavam?: string;
  chassi?: string;
  cor?: string;
  tipoCombustivel?: string;
  rntrc?: string;
  proprietarioNome?: string;
  proprietarioCpfCnpj?: string;
  ativo: boolean;
  dataCriacao: string;
  dataAtualizacao?: string;
}

export interface VeiculoCreateDto {
  placa: string;
  placaReboque?: string;
  marca?: string;
  modelo?: string;
  anoFabricacao?: number;
  anoModelo?: number;
  tara: number; // Obrigatório
  capacidadeKg?: number;
  tipoRodado: string; // Obrigatório (Caminhão, Carreta, etc)
  tipoCarroceria: string; // Obrigatório (Aberta, Fechada, etc)
  uf: string; // Obrigatório (UF de registro do veículo)
  renavam?: string;
  chassi?: string;
  cor?: string;
  tipoCombustivel?: string;
  rntrc?: string;
  proprietarioNome?: string;
  proprietarioCpfCnpj?: string;
  ativo?: boolean;
}

export interface VeiculoUpdateDto extends VeiculoCreateDto {}

export interface VeiculoListDto {
  id: number;
  placa: string;
  marca?: string;
  modelo?: string;
  tipoVeiculo?: string;
  capacidadeCarga?: number;
  ativo: boolean;
}

// ==========================================
// REBOQUE
// ==========================================
export interface ReboqueDto {
  id: number;
  placa: string;
  marca?: string;
  modelo?: string;
  anoFabricacao?: number;
  renavam?: string;
  chassi?: string;
  tipoCarroceria?: string;
  capacidadeCarga?: number;
  tara?: number;
  rntrc?: string;
  proprietarioNome?: string;
  proprietarioCpfCnpj?: string;
  ativo: boolean;
  dataCriacao: string;
  dataAtualizacao?: string;
}

export interface ReboqueCreateDto {
  placa: string;
  marca?: string;
  modelo?: string;
  anoFabricacao?: number;
  renavam?: string;
  chassi?: string;
  tipoCarroceria?: string;
  capacidadeCarga?: number;
  tara?: number;
  rntrc?: string;
  proprietarioNome?: string;
  proprietarioCpfCnpj?: string;
  ativo?: boolean;
}

export interface ReboqueUpdateDto extends ReboqueCreateDto {}

export interface ReboqueListDto {
  id: number;
  placa: string;
  marca?: string;
  modelo?: string;
  tipoCarroceria?: string;
  capacidadeCarga?: number;
  ativo: boolean;
}

// ==========================================
// VIAGEM
// ==========================================
export interface ViagemDto {
  id: number;
  veiculoId: number;
  veiculoPlaca: string;
  reboqueId?: number;
  reboquePlaca?: string;
  motoristaId: number;
  motoristaNome: string;
  dataPartida: string;
  dataChegada?: string;
  origemCidade?: string;
  origemUf?: string;
  destinoCidade?: string;
  destinoUf?: string;
  kmSaida?: number;
  kmChegada?: number;
  distanciaPercorrida?: number;
  numeroCte?: string;
  valorFrete?: number;
  status: string;
  observacoes?: string;
  receitaTotal: number;
  totalDespesas: number;
  valorPedagio: number;
  valorCombustivel: number;
  saldoViagem: number;
  ativo: boolean;
  dataCriacao: string;
  dataAtualizacao?: string;
  despesas: DespesaViagemDto[];
  receitas: ReceitaViagemDto[];
}

export interface ViagemCreateDto {
  veiculoId: number;
  reboqueId?: number;
  motoristaId: number;
  dataPartida: string;
  dataChegada?: string;
  origemCidade?: string;
  origemUf?: string;
  destinoCidade?: string;
  destinoUf?: string;
  kmSaida?: number;
  kmChegada?: number;
  numeroCte?: string;
  valorFrete?: number;
  status?: string;
  observacoes?: string;
  despesas?: DespesaViagemCreateDto[];
  receitas?: ReceitaViagemCreateDto[];
}

export interface ViagemUpdateDto extends ViagemCreateDto {}

export interface ViagemListDto {
  id: number;
  veiculoPlaca: string;
  motoristaNome: string;
  dataPartida: string;
  origemCidade?: string;
  origemUf?: string;
  destinoCidade?: string;
  destinoUf?: string;
  status: string;
  receitaTotal: number;
  totalDespesas: number;
  saldoViagem: number;
  ativo: boolean;
}

// ==========================================
// DESPESA VIAGEM
// ==========================================
export interface DespesaViagemDto {
  id: number;
  viagemId: number;
  tipoDespesa: string;
  descricao?: string;
  valor: number;
  data: string;
  fornecedor?: string;
  numeroDocumento?: string;
  observacoes?: string;
  dataCriacao: string;
}

export interface DespesaViagemCreateDto {
  tipoDespesa: string;
  descricao?: string;
  valor: number;
  data: string;
  fornecedor?: string;
  numeroDocumento?: string;
  observacoes?: string;
}

export interface DespesaViagemUpdateDto extends DespesaViagemCreateDto {
  viagemId: number;
}

// ==========================================
// RECEITA VIAGEM
// ==========================================
export interface ReceitaViagemDto {
  id: number;
  viagemId: number;
  tipoReceita: string;
  descricao?: string;
  valor: number;
  data: string;
  cliente?: string;
  numeroDocumento?: string;
  observacoes?: string;
  dataCriacao: string;
}

export interface ReceitaViagemCreateDto {
  tipoReceita: string;
  descricao?: string;
  valor: number;
  data: string;
  cliente?: string;
  numeroDocumento?: string;
  observacoes?: string;
}

export interface ReceitaViagemUpdateDto extends ReceitaViagemCreateDto {
  viagemId: number;
}

// ==========================================
// MANUTENÇÃO VEÍCULO
// ==========================================
export interface ManutencaoVeiculoDto {
  id: number;
  veiculoId: number;
  veiculoPlaca: string;
  fornecedorId?: number;
  fornecedorNome?: string;
  dataManutencao: string;
  tipoManutencao?: string;
  descricaoServico?: string;
  kmAtual?: number;
  valorMaoObra?: number;
  valorServicosTerceiros?: number;
  valorTotal: number;
  valorTotalPecas: number;
  numeroOS?: string;
  numeroNF?: string;
  dataProximaManutencao?: string;
  kmProximaManutencao?: number;
  observacoes?: string;
  ativo: boolean;
  dataCriacao: string;
  dataAtualizacao?: string;
  pecas: ManutencaoPecaDto[];
}

export interface ManutencaoVeiculoCreateDto {
  veiculoId: number;
  fornecedorId?: number;
  dataManutencao: string;
  tipoManutencao?: string;
  descricaoServico?: string;
  kmAtual?: number;
  valorMaoObra?: number;
  valorServicosTerceiros?: number;
  numeroOS?: string;
  numeroNF?: string;
  dataProximaManutencao?: string;
  kmProximaManutencao?: number;
  observacoes?: string;
  pecas?: ManutencaoPecaCreateDto[];
}

export interface ManutencaoVeiculoUpdateDto extends ManutencaoVeiculoCreateDto {}

export interface ManutencaoVeiculoListDto {
  id: number;
  veiculoPlaca: string;
  fornecedorNome?: string;
  dataManutencao: string;
  tipoManutencao?: string;
  valorTotal: number;
  ativo: boolean;
}

// ==========================================
// MANUTENÇÃO PEÇA
// ==========================================
export interface ManutencaoPecaDto {
  id: number;
  manutencaoId: number;
  codigoPeca?: string;
  descricaoPeca: string;
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  dataCriacao: string;
}

export interface ManutencaoPecaCreateDto {
  codigoPeca?: string;
  descricaoPeca: string;
  quantidade: number;
  valorUnitario: number;
}

export interface ManutencaoPecaUpdateDto extends ManutencaoPecaCreateDto {
  manutencaoId: number;
}

// ==========================================
// PAGINAÇÃO
// ==========================================
export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

// ==========================================
// FILTROS
// ==========================================
export interface VeiculoFiltros {
  pageNumber?: number;
  pageSize?: number;
  busca?: string;
  incluirInativos?: boolean;
  tipoVeiculo?: string;
}

export interface ReboqueFiltros {
  pageNumber?: number;
  pageSize?: number;
  busca?: string;
  incluirInativos?: boolean;
  tipoCarroceria?: string;
}

export interface ViagemFiltros {
  pageNumber?: number;
  pageSize?: number;
  busca?: string;
  veiculoId?: number;
  motoristaId?: number;
  status?: string;
  dataInicio?: string;
  dataFim?: string;
  incluirInativos?: boolean;
}

export interface ManutencaoFiltros {
  pageNumber?: number;
  pageSize?: number;
  busca?: string;
  veiculoId?: number;
  tipoManutencao?: string;
  dataInicio?: string;
  dataFim?: string;
  incluirInativos?: boolean;
}

// ==========================================
// CONSTANTES
// ==========================================
export const TIPOS_VEICULO = [
  'Caminhão',
  'Carreta',
  'Truck',
  'Toco',
  'Van',
  'Utilitário',
  'Outros',
] as const;

export const TIPOS_CARROCERIA = [
  'Aberta',
  'Fechada (Baú)',
  'Graneleira',
  'Tanque',
  'Sider',
  'Frigorífica',
  'Cegonha',
  'Prancha',
  'Outros',
] as const;

export const TIPOS_DESPESA = [
  'Combustível',
  'Pedágio',
  'Alimentação',
  'Hospedagem',
  'Manutenção',
  'Lavagem',
  'Estacionamento',
  'Multas',
  'Outros',
] as const;

export const TIPOS_RECEITA = [
  'Frete',
  'Adiantamento',
  'Outros',
] as const;

export const STATUS_VIAGEM = [
  'Planejada',
  'Em Andamento',
  'Concluída',
  'Cancelada',
] as const;

export const TIPOS_MANUTENCAO = [
  'Preventiva',
  'Corretiva',
  'Emergencial',
  'Revisão',
  'Troca de Óleo',
  'Troca de Pneus',
  'Outros',
] as const;

export const UFS_BRASIL = [
  'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA',
  'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN',
  'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO',
] as const;

// ==========================================
// MOTORISTA
// ==========================================
export interface MotoristaDto {
  codigoDoMotorista: number;
  nomeDoMotorista: string;
  rg: string;
  cpf: string;
  endereco: string;
  numero: string;
  bairro: string;
  municipio: number;
  municipioNome: string;
  uf: string;
  cep: string;
  fone: string;
  cel: string;
}

export interface MotoristaListDto {
  codigoDoMotorista: number;
  nomeDoMotorista: string;
  cpf: string;
  rg: string;
  cel: string;
  uf: string;
}

export interface MotoristaCreateDto {
  nomeDoMotorista: string;
  rg?: string;
  cpf: string;
  endereco?: string;
  numero?: string;
  bairro?: string;
  municipio?: number;
  uf?: string;
  cep?: string;
  fone?: string;
  cel?: string;
}

export interface MotoristaUpdateDto extends MotoristaCreateDto {}

export interface MotoristaFiltros {
  busca?: string;
  uf?: string;
  pagina?: number;
  tamanhoPagina?: number;
}
