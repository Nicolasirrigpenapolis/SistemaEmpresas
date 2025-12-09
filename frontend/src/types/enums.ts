/**
 * Constantes para padronizar valores em todo o frontend
 * Corresponde aos enums do backend
 * Usando 'as const' para type safety sem enums
 */

export const StatusOrcamento = {
  Aberto: 'Aberto',
  EmAnalise: 'Em Análise',
  Aprovado: 'Aprovado',
  Rejeitado: 'Rejeitado',
  Cancelado: 'Cancelado',
} as const;

export type StatusOrcamento = typeof StatusOrcamento[keyof typeof StatusOrcamento];

export const StatusPedido = {
  Aberto: 'Aberto',
  EmProducao: 'Em Produção',
  Finalizado: 'Finalizado',
  Cancelado: 'Cancelado',
  Faturado: 'Faturado',
} as const;

export type StatusPedido = typeof StatusPedido[keyof typeof StatusPedido];

export const StatusOrdemServico = {
  Aberta: 'Aberta',
  EmAndamento: 'Em Andamento',
  Concluida: 'Concluída',
  Cancelada: 'Cancelada',
} as const;

export type StatusOrdemServico = typeof StatusOrdemServico[keyof typeof StatusOrdemServico];

export const PrioridadeAlerta = {
  Baixa: 'Baixa',
  Media: 'Média',
  Alta: 'Alta',
  Critica: 'Crítica',
} as const;

export type PrioridadeAlerta = typeof PrioridadeAlerta[keyof typeof PrioridadeAlerta];

export const TipoAlerta = {
  EstoqueBaixo: 'Estoque Baixo',
  PedidoAtrasado: 'Pedido Atrasado',
  OrdemServicoAtrasada: 'Ordem de Serviço Atrasada',
  VencimentoProximo: 'Vencimento Próximo',
} as const;

export type TipoAlerta = typeof TipoAlerta[keyof typeof TipoAlerta];

/**
 * Helper para obter cor baseada no status do orçamento
 */
export const getStatusOrcamentoColor = (status: StatusOrcamento): string => {
  switch (status) {
    case StatusOrcamento.Aberto:
      return 'text-blue-600 bg-blue-50';
    case StatusOrcamento.EmAnalise:
      return 'text-yellow-600 bg-yellow-50';
    case StatusOrcamento.Aprovado:
      return 'text-green-600 bg-green-50';
    case StatusOrcamento.Rejeitado:
      return 'text-red-600 bg-red-50';
    case StatusOrcamento.Cancelado:
      return 'text-gray-600 bg-gray-50';
    default:
      return 'text-gray-600 bg-gray-50';
  }
};

/**
 * Helper para obter cor baseada na prioridade do alerta
 */
export const getPrioridadeAlertaColor = (prioridade: PrioridadeAlerta): string => {
  switch (prioridade) {
    case PrioridadeAlerta.Baixa:
      return 'text-blue-600 bg-blue-50';
    case PrioridadeAlerta.Media:
      return 'text-yellow-600 bg-yellow-50';
    case PrioridadeAlerta.Alta:
      return 'text-orange-600 bg-orange-50';
    case PrioridadeAlerta.Critica:
      return 'text-red-600 bg-red-50';
    default:
      return 'text-gray-600 bg-gray-50';
  }
};
