import { AlertTriangle, Clock, Package, AlertCircle, CheckCircle } from 'lucide-react';
import type { AlertaOperacional } from '../../../types/Dashboard/dashboard';

interface AlertasPanelProps {
  alertas: AlertaOperacional[];
  loading?: boolean;
}

export default function AlertasPanel({ alertas, loading }: AlertasPanelProps) {
  const getIcon = (tipo: string) => {
    switch (tipo) {
      case 'Atraso':
        return Clock;
      case 'EstoqueCritico':
        return Package;
      case 'ComprasPendentes':
        return AlertCircle;
      default:
        return AlertTriangle;
    }
  };

  const getColorClass = (tipo: string) => {
    switch (tipo) {
      case 'Atraso':
        return 'text-red-600 bg-red-50';
      case 'EstoqueCritico':
        return 'text-amber-600 bg-amber-50';
      case 'ComprasPendentes':
        return 'text-blue-600 bg-blue-50';
      default:
        return 'text-gray-600 bg-gray-50';
    }
  };

  if (loading) {
    return (
      <div className="space-y-3">
        {[1, 2, 3].map((i) => (
          <div key={i} className="animate-pulse h-14 bg-gray-100 rounded-lg" />
        ))}
      </div>
    );
  }

  if (!alertas || alertas.length === 0) {
    return (
      <div className="text-center py-8">
        <CheckCircle className="w-10 h-10 text-green-500 mx-auto mb-2" />
        <p className="text-gray-600 font-medium">Tudo certo!</p>
        <p className="text-sm text-gray-400">Nenhum alerta no momento</p>
      </div>
    );
  }

  return (
    <div className="space-y-2">
      {alertas.map((alerta, index) => {
        const Icon = getIcon(alerta.tipo);
        const colorClass = getColorClass(alerta.tipo);
        return (
          <div
            key={index}
            className={`flex items-center gap-3 p-3 rounded-lg ${colorClass}`}
          >
            <Icon className="w-4 h-4 flex-shrink-0" />
            <span className="flex-1 text-sm font-medium truncate">{alerta.mensagem}</span>
            <span className="font-bold">{alerta.quantidade}</span>
          </div>
        );
      })}
    </div>
  );
}
