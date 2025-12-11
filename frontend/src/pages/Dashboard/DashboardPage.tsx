import { useEffect, useState } from 'react';
import {
  FileText,
  Package,
  Box,
  AlertTriangle,
  RefreshCw,
  TrendingUp,
  ArrowUpRight,
  ArrowDownRight,
  Activity
} from 'lucide-react';
import {
  AreaChart,
  Area,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
} from 'recharts';
import { dashboardService } from '../../services/dashboardService';
import type {
  DashboardKpi,
  OrcamentoResumido,
  OrcamentosPorStatus,
  TimelineOrcamentos,
} from '../../types/dashboard';

// Cores Premium
const COLORS = {
  primary: '#3b82f6',
  secondary: '#6366f1',
  success: '#10b981',
  warning: '#f59e0b',
  error: '#ef4444',
  slate: '#64748b'
};

const STATUS_COLORS: Record<string, string> = {
  'Aberto': COLORS.primary,
  'Fechado': COLORS.success,
  'Cancelado': COLORS.error,
};

export default function DashboardPage() {
  const [kpis, setKpis] = useState<DashboardKpi | null>(null);
  const [orcamentosRecentes, setOrcamentosRecentes] = useState<OrcamentoResumido[]>([]);
  const [orcamentosPorStatus, setOrcamentosPorStatus] = useState<OrcamentosPorStatus[]>([]);
  const [timeline, setTimeline] = useState<TimelineOrcamentos[]>([]);
  const [loading, setLoading] = useState(true);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const [kpisData, orcamentosData, statusData, timelineData] = await Promise.all([
        dashboardService.getKpis(),
        dashboardService.getOrcamentosRecentes(5),
        dashboardService.getOrcamentosPorStatus(),
        dashboardService.getTimelineOrcamentos(30),
      ]);

      setKpis(kpisData);
      setOrcamentosRecentes(orcamentosData);
      setOrcamentosPorStatus(statusData);
      setTimeline(timelineData);
    } catch (err) {
      console.error('Erro ao carregar dashboard:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDashboardData();
  }, []);

  const formatDate = (dateString: string | null) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit' });
  };

  const formatPeriodo = (periodo: string) => {
    if (!periodo) return '';
    const date = new Date(periodo);
    return date.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit' });
  };

  const getStatusColor = (orcamento: OrcamentoResumido) => {
    if (orcamento.cancelado) return 'text-red-600 bg-red-50 ring-red-500/20';
    if (orcamento.vendaFechada) return 'text-emerald-600 bg-emerald-50 ring-emerald-500/20';
    return 'text-blue-600 bg-blue-50 ring-blue-500/20';
  };

  const getStatusLabel = (orcamento: OrcamentoResumido) => {
    if (orcamento.cancelado) return 'Cancelado';
    if (orcamento.vendaFechada) return 'Fechado';
    return 'Aberto';
  };

  if (loading && !kpis) {
    return (
      <div className="space-y-8 animate-pulse p-2">
        <div className="flex justify-between items-center">
          <div className="h-10 w-48 bg-gray-200 rounded-xl" />
          <div className="h-10 w-32 bg-gray-200 rounded-xl" />
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {[1, 2, 3, 4].map(i => (
            <div key={i} className="h-32 bg-gray-200 rounded-2xl" />
          ))}
        </div>
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <div className="lg:col-span-2 h-96 bg-gray-200 rounded-2xl" />
          <div className="h-96 bg-gray-200 rounded-2xl" />
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-8 pb-8">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
        <div>
          <h1 className="text-2xl md:text-3xl font-display font-bold text-primary tracking-tight">Dashboard</h1>
          <p className="text-muted-foreground mt-1 text-sm md:text-base">Visão geral do seu negócio em tempo real</p>
        </div>
        <button
          onClick={loadDashboardData}
          disabled={loading}
          className="inline-flex items-center justify-center gap-2 px-4 md:px-5 py-2 md:py-2.5 text-sm font-medium text-primary bg-surface hover:bg-surface-hover border border-border rounded-xl transition-all shadow-sm hover:shadow-md active:scale-95 disabled:opacity-50 w-full sm:w-auto"
        >
          <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
          <span>Atualizar Dados</span>
        </button>
      </div>

      {/* KPIs */}
      {kpis && (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 md:gap-6">
          <KpiCard
            title="Orçamentos Abertos"
            value={kpis.orcamentosAbertos}
            icon={FileText}
            color="blue"
            trend="+12.5%"
            trendUp={true}
          />
          <KpiCard
            title="Compras Pendentes"
            value={kpis.comprasPendentesValidacao}
            icon={Package}
            color="amber"
            trend="Aguardando"
            trendUp={false}
          />
          <KpiCard
            title="Produtos Ativos"
            value={kpis.totalProdutos}
            icon={Box}
            color="emerald"
            trend="+5 novos"
            trendUp={true}
          />
          <KpiCard
            title="Estoque Crítico"
            value={kpis.produtosEstoqueCritico}
            icon={AlertTriangle}
            color="red"
            trend="Atenção"
            trendUp={false}
          />
        </div>
      )}

      {/* Gráficos */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 md:gap-8">
        {/* Gráfico de Timeline */}
        <div className="lg:col-span-2 bg-surface border border-border rounded-2xl p-6 shadow-sm hover:shadow-md transition-shadow duration-300">
          <div className="flex items-center justify-between mb-6">
            <div>
              <h3 className="text-lg font-bold text-primary">Evolução de Orçamentos</h3>
              <p className="text-sm text-muted-foreground">Últimos 30 dias</p>
            </div>
            <div className="p-2 bg-blue-50 rounded-lg">
              <TrendingUp className="w-5 h-5 text-blue-600" />
            </div>
          </div>

          {timeline.length > 0 ? (
            <div className="h-[250px] md:h-[300px] w-full">
              <ResponsiveContainer width="100%" height="100%">
                <AreaChart data={timeline} margin={{ top: 10, right: 10, left: -20, bottom: 0 }}>
                  <defs>
                    <linearGradient id="colorOrcamentos" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor={COLORS.primary} stopOpacity={0.3} />
                      <stop offset="95%" stopColor={COLORS.primary} stopOpacity={0} />
                    </linearGradient>
                  </defs>
                  <CartesianGrid strokeDasharray="3 3" stroke="var(--color-border)" vertical={false} opacity={0.5} />
                  <XAxis
                    dataKey="periodo"
                    tickFormatter={formatPeriodo}
                    tick={{ fontSize: 12, fill: 'var(--color-muted-foreground)' }}
                    tickLine={false}
                    axisLine={false}
                    interval="preserveStartEnd"
                    dy={10}
                  />
                  <YAxis
                    tick={{ fontSize: 12, fill: 'var(--color-muted-foreground)' }}
                    tickLine={false}
                    axisLine={false}
                    width={30}
                    allowDecimals={false}
                  />
                  <Tooltip
                    contentStyle={{
                      backgroundColor: 'var(--color-surface)',
                      border: '1px solid var(--color-border)',
                      borderRadius: '12px',
                      boxShadow: '0 10px 15px -3px rgb(0 0 0 / 0.1)',
                      padding: '12px'
                    }}
                    itemStyle={{ color: 'var(--color-primary)', fontWeight: 600 }}
                    labelStyle={{ color: 'var(--color-muted-foreground)', marginBottom: 8 }}
                  />
                  <Area
                    type="monotone"
                    dataKey="quantidadeOrcamentos"
                    name="Orçamentos"
                    stroke={COLORS.primary}
                    strokeWidth={3}
                    fill="url(#colorOrcamentos)"
                    dot={{ fill: COLORS.primary, strokeWidth: 2, stroke: 'white', r: 4 }}
                    activeDot={{ r: 6, strokeWidth: 0 }}
                    animationDuration={1500}
                  />
                </AreaChart>
              </ResponsiveContainer>
            </div>
          ) : (
            <div className="h-[250px] md:h-[300px] flex items-center justify-center text-muted-foreground bg-surface-active/10 rounded-xl border border-dashed border-border">
              <div className="text-center">
                <Activity className="w-8 h-8 mx-auto mb-2 opacity-50" />
                <p>Sem dados para exibir</p>
              </div>
            </div>
          )}
        </div>

        {/* Gráfico de Status */}
        <div className="bg-surface border border-border rounded-2xl p-6 shadow-sm hover:shadow-md transition-shadow duration-300 flex flex-col">
          <div className="mb-6">
            <h3 className="text-lg font-bold text-primary">Status dos Orçamentos</h3>
            <p className="text-sm text-muted-foreground">Distribuição atual</p>
          </div>

          {orcamentosPorStatus.length > 0 ? (
            <div className="flex-1 flex flex-col items-center justify-center">
              <div className="w-full h-[220px] relative">
                <ResponsiveContainer width="100%" height="100%">
                  <PieChart>
                    <Pie
                      data={orcamentosPorStatus as any}
                      dataKey="quantidade"
                      nameKey="status"
                      cx="50%"
                      cy="50%"
                      innerRadius={60}
                      outerRadius={80}
                      paddingAngle={5}
                      cornerRadius={5}
                    >
                      {orcamentosPorStatus.map((entry, index) => (
                        <Cell
                          key={`cell-${index}`}
                          fill={STATUS_COLORS[entry.status] || COLORS.slate}
                          strokeWidth={0}
                        />
                      ))}
                    </Pie>
                    <Tooltip
                      contentStyle={{
                        backgroundColor: 'var(--color-surface)',
                        border: '1px solid var(--color-border)',
                        borderRadius: '12px',
                        boxShadow: '0 10px 15px -3px rgb(0 0 0 / 0.1)',
                      }}
                      itemStyle={{ color: 'var(--color-primary)' }}
                    />
                  </PieChart>
                </ResponsiveContainer>
                {/* Center Text */}
                <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
                  <div className="text-center">
                    <span className="text-2xl font-bold text-primary">
                      {orcamentosPorStatus.reduce((acc, curr) => acc + curr.quantidade, 0)}
                    </span>
                    <p className="text-xs text-muted-foreground uppercase font-bold tracking-wider">Total</p>
                  </div>
                </div>
              </div>

              <div className="w-full space-y-3 mt-4">
                {orcamentosPorStatus.map((item) => (
                  <div key={item.status} className="flex items-center justify-between p-2 rounded-lg hover:bg-surface-hover transition-colors">
                    <div className="flex items-center gap-3">
                      <div
                        className="w-3 h-3 rounded-full ring-2 ring-white dark:ring-slate-900 shadow-sm"
                        style={{ backgroundColor: STATUS_COLORS[item.status] || COLORS.slate }}
                      />
                      <span className="text-sm font-medium text-primary">{item.status}</span>
                    </div>
                    <span className="text-sm font-medium text-[var(--text-muted)]">
                      {item.quantidade}
                    </span>
                  </div>
                ))}
              </div>
            </div>
          ) : (
            <div className="flex-1 flex items-center justify-center text-muted-foreground bg-surface-active/10 rounded-xl border border-dashed border-border min-h-[200px]">
              <div className="text-center">
                <Activity className="w-8 h-8 mx-auto mb-2 opacity-50" />
                <p>Sem dados para exibir</p>
              </div>
            </div>
          )}
        </div>
      </div>

      {/* Orçamentos Recentes */}
      <div className="bg-surface border border-border rounded-2xl p-6 shadow-sm hover:shadow-md transition-shadow duration-300">
        <div className="flex items-center justify-between mb-6">
          <div>
            <h3 className="text-lg font-bold text-primary">Últimos Orçamentos</h3>
            <p className="text-sm text-muted-foreground">Transações recentes</p>
          </div>
          <button className="text-sm font-medium text-secondary hover:text-secondary/80 transition-colors">
            Ver todos
          </button>
        </div>

        {orcamentosRecentes.length > 0 ? (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-border">
                  <th className="text-left py-4 px-4 text-xs font-bold text-muted-foreground uppercase tracking-wider">Nº</th>
                  <th className="text-left py-4 px-4 text-xs font-bold text-muted-foreground uppercase tracking-wider">Cliente</th>
                  <th className="text-left py-4 px-4 text-xs font-bold text-muted-foreground uppercase tracking-wider">Data</th>
                  <th className="text-left py-4 px-4 text-xs font-bold text-muted-foreground uppercase tracking-wider hidden sm:table-cell">Vendedor</th>
                  <th className="text-left py-4 px-4 text-xs font-bold text-muted-foreground uppercase tracking-wider">Status</th>
                  <th className="text-right py-4 px-4 text-xs font-bold text-muted-foreground uppercase tracking-wider">Ação</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-border">
                {orcamentosRecentes.map((orc) => (
                  <tr key={orc.sequenciaDoOrcamento} className="group hover:bg-surface-hover transition-colors">
                    <td className="py-4 px-4 text-sm font-bold text-primary">
                      #{orc.sequenciaDoOrcamento}
                    </td>
                    <td className="py-4 px-4">
                      <div className="flex items-center gap-3">
                        <div className="w-8 h-8 rounded-full bg-gradient-to-br from-blue-500 to-indigo-500 flex items-center justify-center text-white text-xs font-bold shadow-sm">
                          {orc.nomeCliente.charAt(0)}
                        </div>
                        <span className="text-sm font-medium text-primary max-w-[150px] md:max-w-[250px] truncate">
                          {orc.nomeCliente}
                        </span>
                      </div>
                    </td>
                    <td className="py-4 px-4 text-sm text-muted-foreground">
                      {formatDate(orc.dataDeEmissao)}
                    </td>
                    <td className="py-4 px-4 text-sm text-muted-foreground hidden sm:table-cell">
                      {orc.nomeVendedor || '-'}
                    </td>
                    <td className="py-4 px-4">
                      <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 text-xs font-bold rounded-full ring-1 ring-inset ${getStatusColor(orc)}`}>
                        <span className="w-1.5 h-1.5 rounded-full bg-current" />
                        {getStatusLabel(orc)}
                      </span>
                    </td>
                    <td className="py-4 px-4 text-right">
                      <button className="text-xs font-medium text-secondary hover:text-secondary/80 bg-secondary/10 hover:bg-secondary/20 px-3 py-1.5 rounded-lg transition-colors opacity-0 group-hover:opacity-100">
                        Detalhes
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : (
          <div className="text-center py-12 text-muted-foreground bg-surface-active/10 rounded-xl border border-dashed border-border">
            <Activity className="w-10 h-10 mx-auto mb-3 opacity-50" />
            <p className="font-medium">Nenhum orçamento encontrado</p>
          </div>
        )}
      </div>
    </div>
  );
}

// Componente KPI Card
interface KpiCardProps {
  title: string;
  value: number;
  icon: React.ComponentType<{ className?: string }>;
  color: 'blue' | 'emerald' | 'amber' | 'indigo' | 'red';
  trend?: string;
  trendUp?: boolean;
}

function KpiCard({ title, value, icon: Icon, color, trend, trendUp }: KpiCardProps) {


  const iconBgClasses = {
    blue: 'bg-blue-500 text-white shadow-blue-500/30',
    emerald: 'bg-emerald-500 text-white shadow-emerald-500/30',
    amber: 'bg-amber-500 text-white shadow-amber-500/30',
    indigo: 'bg-indigo-500 text-white shadow-indigo-500/30',
    red: 'bg-red-500 text-white shadow-red-500/30',
  };

  return (
    <div className="bg-surface border border-border rounded-2xl p-5 shadow-sm hover:shadow-lg hover:-translate-y-1 transition-all duration-300 group">
      <div className="flex items-start justify-between mb-4">
        <div className={`p-3 rounded-xl shadow-lg ${iconBgClasses[color]} transition-transform group-hover:scale-110`}>
          <Icon className="w-6 h-6" />
        </div>
        {trend && (
          <div className={`flex items-center gap-1 text-xs font-bold px-2 py-1 rounded-lg ${trendUp ? 'text-emerald-600 bg-emerald-500/10' : 'text-amber-600 bg-amber-500/10'}`}>
            {trendUp ? <ArrowUpRight className="w-3 h-3" /> : <ArrowDownRight className="w-3 h-3" />}
            {trend}
          </div>
        )}
      </div>

      <div>
        <h3 className="text-3xl font-bold text-primary tracking-tight mb-1">{value}</h3>
        <p className="text-sm font-medium text-muted-foreground">{title}</p>
      </div>
    </div>
  );
}
