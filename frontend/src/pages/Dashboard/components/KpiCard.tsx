import type { LucideIcon } from 'lucide-react';

interface KpiCardProps {
  title: string;
  value: number | string;
  icon: LucideIcon;
  subtitle?: string;
  colorClass?: string;
}

export default function KpiCard({
  title,
  value,
  icon: Icon,
  subtitle,
  colorClass = 'bg-blue-600 text-white',
}: KpiCardProps) {
  return (
    <div className="bg-white rounded-lg border border-gray-200 p-5 hover:shadow-sm transition-shadow">
      <div className="flex items-start justify-between mb-3">
        <h3 className="text-sm font-medium text-gray-600">{title}</h3>
        <div className={`${colorClass} p-2 rounded-lg`}>
          <Icon className="w-4 h-4" strokeWidth={2} />
        </div>
      </div>
      
      <p className="text-2xl font-bold text-gray-900 mb-1">{value}</p>

      {subtitle && (
        <p className="text-xs text-gray-500">{subtitle}</p>
      )}
    </div>
  );
}
