import api from './api';
import type {
  DashboardKpi,
  OrcamentoResumido,
  OrcamentosPorStatus,
  OrdensServicoStatus,
  ProdutoEstoqueCritico,
  AlertaOperacional,
  OrcamentosPorVendedor,
  TimelineOrcamentos,
  PedidoCompraResumido,
} from '../types/dashboard';

export const dashboardService = {
  async getKpis(): Promise<DashboardKpi> {
    const response = await api.get<DashboardKpi>('/dashboard/kpis');
    return response.data;
  },

  async getOrcamentosRecentes(limite: number = 10): Promise<OrcamentoResumido[]> {
    const response = await api.get<OrcamentoResumido[]>('/dashboard/orcamentos-recentes', {
      params: { limite },
    });
    return response.data;
  },

  async getOrcamentosPorStatus(): Promise<OrcamentosPorStatus[]> {
    const response = await api.get<OrcamentosPorStatus[]>('/dashboard/orcamentos-por-status');
    return response.data;
  },

  async getOrdensServicoStatus(): Promise<OrdensServicoStatus[]> {
    const response = await api.get<OrdensServicoStatus[]>('/dashboard/ordens-servico-status');
    return response.data;
  },

  async getEstoqueCritico(limite: number = 20): Promise<ProdutoEstoqueCritico[]> {
    const response = await api.get<ProdutoEstoqueCritico[]>('/dashboard/estoque-critico', {
      params: { limite },
    });
    return response.data;
  },

  async getAlertas(): Promise<AlertaOperacional[]> {
    const response = await api.get<AlertaOperacional[]>('/dashboard/alertas');
    return response.data;
  },

  async getOrcamentosPorVendedor(limite: number = 10): Promise<OrcamentosPorVendedor[]> {
    const response = await api.get<OrcamentosPorVendedor[]>('/dashboard/orcamentos-por-vendedor', {
      params: { limite },
    });
    return response.data;
  },

  async getTimelineOrcamentos(dias: number = 30): Promise<TimelineOrcamentos[]> {
    const response = await api.get<TimelineOrcamentos[]>('/dashboard/timeline-orcamentos', {
      params: { dias },
    });
    return response.data;
  },

  async getComprasAtrasadas(limite: number = 10): Promise<PedidoCompraResumido[]> {
    const response = await api.get<PedidoCompraResumido[]>('/dashboard/compras-atrasadas', {
      params: { limite },
    });
    return response.data;
  },
};

export default dashboardService;
