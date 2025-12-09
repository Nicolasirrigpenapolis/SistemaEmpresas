import { useEffect, useState } from 'react';
import {
  FileText,
  Package,
  Box,
  Layers,
  AlertTriangle,
  RefreshCw,
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

// Cores para o gráfico de pizza
const STATUS_COLORS: Record<string, string> = {
  'Aberto': '#3B82F6',
  'Fechado': '#10B981',
  'Cancelado': '#EF4444',
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

  // Formatar período do gráfico (dd/MM)
  const formatPeriodo = (periodo: string) => {
    if (!periodo) return '';
    const date = new Date(periodo);
    return date.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit' });
  };

  const getStatusColor = (orcamento: OrcamentoResumido) => {
    if (orcamento.cancelado) return 'text-red-600 bg-red-50';
    if (orcamento.vendaFechada) return 'text-emerald-600 bg-emerald-50';
    return 'text-blue-600 bg-blue-50';
  };

  const getStatusLabel = (orcamento: OrcamentoResumido) => {
    if (orcamento.cancelado) return 'Cancelado';
    if (orcamento.vendaFechada) return 'Fechado';
    return 'Aberto';
  };

  // Skeleton loader
  if (loading && !kpis) {
    return (
      <div className="space-y-6 animate-pulse">
        <div className="h-8 w-48 bg-gray-200 rounded" />
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          {[1, 2, 3, 4].map(i => (
            <div key={i} className="h-24 bg-gray-200 rounded-xl" />
          ))}
        </div>
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2 h-80 bg-gray-200 rounded-xl" />
          <div className="h-80 bg-gray-200 rounded-xl" />
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-4 md:space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
        <h1 className="text-xl md:text-2xl font-bold text-[var(--text)]">Dashboard</h1>
        <button
          onClick={loadDashboardData}
          disabled={loading}
          className="inline-flex items-center justify-center gap-2 px-4 py-2 text-sm font-medium text-[var(--text-muted)] hover:text-[var(--text)] bg-[var(--surface)] border border-[var(--border)] hover:border-gray-300 rounded-lg transition-colors disabled:opacity-50"
        >
          <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
          <span className="hidden sm:inline">Atualizar</span>
        </button>
      </div>

      {/* KPIs */}
      {kpis && (
        <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-5 gap-3 md:gap-4">
          <KpiCard
            title="Orçamentos Abertos"
            value={kpis.orcamentosAbertos}
            icon={FileText}
            color="blue"
          />
          <KpiCard
            title="Compras Pendentes"
            value={kpis.comprasPendentesValidacao}
            icon={Package}
            color="amber"
          />
          <KpiCard
            title="Produtos Cadastrados"
            value={kpis.totalProdutos}
            icon={Box}
            color="emerald"
          />
          <KpiCard
            title="Conjuntos Cadastrados"
            value={kpis.totalConjuntos}
            icon={Layers}
            color="indigo"
          />
          <KpiCard
            title="Estoque Crítico"
            value={kpis.produtosEstoqueCritico}
            icon={AlertTriangle}
            color="red"
          />
        </div>
      )}

      {/* Gráficos */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 md:gap-6">
        {/* Gráfico de Timeline */}
        <div className="lg:col-span-2 bg-[var(--surface)] border border-[var(--border)] rounded-xl p-4 md:p-6">
          <h3 className="text-sm md:text-base font-semibold text-[var(--text)] mb-4">
            Orçamentos (Últimos 30 dias)
          </h3>
          {timeline.length > 0 ? (
            <ResponsiveContainer width="100%" height={220}>
              <AreaChart data={timeline} margin={{ top: 10, right: 10, left: -20, bottom: 0 }}>
                <defs>
                  <linearGradient id="colorOrcamentos" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#3B82F6" stopOpacity={0.3} />
                    <stop offset="95%" stopColor="#3B82F6" stopOpacity={0} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#E5E7EB" vertical={false} />
                <XAxis 
                  dataKey="periodo" 
                  tickFormatter={formatPeriodo}
                  tick={{ fontSize: 11, fill: '#6B7280' }}
                  tickLine={false}
                  axisLine={{ stroke: '#E5E7EB' }}
                  interval="preserveStartEnd"
                />
                <YAxis 
                  tick={{ fontSize: 12, fill: '#6B7280' }}
                  tickLine={false}
                  axisLine={false}
                  width={30}
                  allowDecimals={false}
                />
                <Tooltip
                  labelFormatter={formatPeriodo}
                  contentStyle={{
                    backgroundColor: '#fff',
                    border: '1px solid #E5E7EB',
                    borderRadius: '8px',
                    boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)',
                  }}
                  labelStyle={{ fontWeight: 600, marginBottom: 4 }}
                />
                <Area
                  type="monotone"
                  dataKey="quantidadeOrcamentos"
                  name="Orçamentos"
                  stroke="#3B82F6"
                  strokeWidth={2}
                  fill="url(#colorOrcamentos)"
                  dot={{ fill: '#3B82F6', strokeWidth: 0, r: 3 }}
                  activeDot={{ r: 5, strokeWidth: 0 }}
                />
              </AreaChart>
            </ResponsiveContainer>
          ) : (
            <div className="h-[220px] flex items-center justify-center text-gray-400">
              Sem dados para exibir
            </div>
          )}
        </div>

        {/* Gráfico de Status */}
        <div className="bg-[var(--surface)] border border-[var(--border)] rounded-xl p-4 md:p-6">
          <h3 className="text-sm md:text-base font-semibold text-[var(--text)] mb-4">
            Status dos Orçamentos
          </h3>
          {orcamentosPorStatus.length > 0 ? (
            <div className="flex flex-col items-center">
              <ResponsiveContainer width="100%" height={200}>
                <PieChart>
                  <Pie
                    data={orcamentosPorStatus as unknown as Record<string, unknown>[]}
                    dataKey="quantidade"
                    nameKey="status"
                    cx="50%"
                    cy="50%"
                    innerRadius={50}
                    outerRadius={80}
                    paddingAngle={2}
                  >
                    {orcamentosPorStatus.map((entry, index) => (
                      <Cell 
                        key={`cell-${index}`} 
                        fill={STATUS_COLORS[entry.status] || '#9CA3AF'} 
                      />
                    ))}
                  </Pie>
                  <Tooltip
                    contentStyle={{
                      backgroundColor: '#fff',
                      border: '1px solid #E5E7EB',
                      borderRadius: '8px',
                    }}
                  />
                </PieChart>
              </ResponsiveContainer>
              <div className="flex flex-wrap justify-center gap-4 mt-2">
                {orcamentosPorStatus.map((item) => (
                  <div key={item.status} className="flex items-center gap-2 text-sm">
                    <div
                      className="w-3 h-3 rounded-full"
                      style={{ backgroundColor: STATUS_COLORS[item.status] || '#9CA3AF' }}
                    />
                    <span className="text-[var(--text-muted)]">{item.status}</span>
                    <span className="font-semibold text-[var(--text)]">{item.quantidade}</span>
                  </div>
                ))}
              </div>
            </div>
          ) : (
            <div className="h-[200px] flex items-center justify-center text-gray-400">
              Sem dados para exibir
            </div>
          )}
        </div>
      </div>

      {/* Orçamentos Recentes */}
      <div className="bg-[var(--surface)] border border-[var(--border)] rounded-xl p-4 md:p-6">
        <h3 className="text-sm md:text-base font-semibold text-[var(--text)] mb-4">
          Últimos Orçamentos
        </h3>
        {orcamentosRecentes.length > 0 ? (
          <div className="overflow-x-auto -mx-4 md:mx-0">
            <div className="min-w-[600px] md:min-w-0 px-4 md:px-0">
              <table className="w-full">
                <thead>
                  <tr className="border-b border-[var(--border)]">
                    <th className="text-left py-3 px-2 md:px-4 text-xs font-medium text-[var(--text-muted)] uppercase">Nº</th>
                    <th className="text-left py-3 px-2 md:px-4 text-xs font-medium text-[var(--text-muted)] uppercase">Cliente</th>
                    <th className="text-left py-3 px-2 md:px-4 text-xs font-medium text-[var(--text-muted)] uppercase">Data</th>
                    <th className="text-left py-3 px-2 md:px-4 text-xs font-medium text-[var(--text-muted)] uppercase hidden sm:table-cell">Vendedor</th>
                    <th className="text-left py-3 px-2 md:px-4 text-xs font-medium text-[var(--text-muted)] uppercase">Status</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-50">
                  {orcamentosRecentes.map((orc) => (
                    <tr key={orc.sequenciaDoOrcamento} className="hover:bg-[var(--surface-muted)] transition-colors">
                      <td className="py-3 px-2 md:px-4 text-sm font-medium text-[var(--text)]">
                        #{orc.sequenciaDoOrcamento}
                      </td>
                      <td className="py-3 px-2 md:px-4 text-sm text-gray-700 max-w-[120px] md:max-w-[200px] truncate">
                        {orc.nomeCliente}
                      </td>
                      <td className="py-3 px-2 md:px-4 text-sm text-[var(--text-muted)]">
                        {formatDate(orc.dataDeEmissao)}
                      </td>
                      <td className="py-3 px-2 md:px-4 text-sm text-[var(--text-muted)] hidden sm:table-cell">
                        {orc.nomeVendedor || '-'}
                      </td>
                      <td className="py-3 px-2 md:px-4">
                        <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${getStatusColor(orc)}`}>
                          {getStatusLabel(orc)}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        ) : (
          <div className="text-center py-8 text-gray-400">
            Nenhum orçamento encontrado
          </div>
        )}
      </div>
    </div>
  );
}

// Componente KPI Card inline (simplificado)
interface KpiCardProps {
  title: string;
  value: number;
  icon: React.ComponentType<{ className?: string }>;
  color: 'blue' | 'emerald' | 'amber' | 'indigo' | 'red';
}

function KpiCard({ title, value, icon: Icon, color }: KpiCardProps) {
  const colorClasses = {
    blue: 'bg-blue-50 text-blue-600',
    emerald: 'bg-emerald-50 text-emerald-600',
    amber: 'bg-amber-50 text-amber-600',
    indigo: 'bg-indigo-50 text-indigo-600',
    red: 'bg-red-50 text-red-600',
  };

  return (
    <div className="bg-[var(--surface)] border border-[var(--border)] rounded-xl p-3 md:p-5">
      <div className="flex items-center justify-between gap-2">
        <div className="min-w-0 flex-1">
          <p className="text-xs md:text-sm text-[var(--text-muted)] mb-1 truncate">{title}</p>
          <p className="text-xl md:text-3xl font-bold text-[var(--text)]">{value}</p>
        </div>
        <div className={`p-2 md:p-3 rounded-xl ${colorClasses[color]} flex-shrink-0`}>
          <Icon className="w-4 h-4 md:w-6 md:h-6" />
        </div>
      </div>
    </div>
  );
}
