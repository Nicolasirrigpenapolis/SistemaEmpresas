import { AlertCircle, AlertTriangle, CheckCircle, Info, X } from 'lucide-react';

// ============================================================================
// TIPOS
// ============================================================================
export type TipoAlerta = 'erro' | 'aviso' | 'sucesso' | 'info';

export interface AlertaProps {
  /** Tipo do alerta */
  tipo: TipoAlerta;
  /** Título do alerta (opcional) */
  titulo?: string;
  /** Mensagem do alerta */
  mensagem: string;
  /** Se pode ser fechado */
  fechavel?: boolean;
  /** Callback ao fechar */
  onFechar?: () => void;
  /** Classe CSS adicional */
  className?: string;
}

// ============================================================================
// COMPONENTE
// ============================================================================
export function Alerta({
  tipo,
  titulo,
  mensagem,
  fechavel = false,
  onFechar,
  className = '',
}: AlertaProps) {
  const estilos: Record<TipoAlerta, { bg: string; border: string; text: string; icon: string }> = {
    erro: {
      bg: 'bg-red-50',
      border: 'border-red-200',
      text: 'text-red-800',
      icon: 'text-red-500',
    },
    aviso: {
      bg: 'bg-amber-50',
      border: 'border-amber-200',
      text: 'text-amber-800',
      icon: 'text-amber-500',
    },
    sucesso: {
      bg: 'bg-green-50',
      border: 'border-green-200',
      text: 'text-green-800',
      icon: 'text-green-500',
    },
    info: {
      bg: 'bg-blue-50',
      border: 'border-blue-200',
      text: 'text-blue-800',
      icon: 'text-blue-500',
    },
  };

  const icones: Record<TipoAlerta, typeof AlertCircle> = {
    erro: AlertCircle,
    aviso: AlertTriangle,
    sucesso: CheckCircle,
    info: Info,
  };

  const estilo = estilos[tipo];
  const Icone = icones[tipo];

  return (
    <div className={`${estilo.bg} ${estilo.border} border rounded-lg p-4 ${className}`}>
      <div className="flex">
        <div className="flex-shrink-0">
          <Icone className={`w-5 h-5 ${estilo.icon}`} />
        </div>
        <div className="ml-3 flex-1">
          {titulo && (
            <h3 className={`text-sm font-medium ${estilo.text}`}>{titulo}</h3>
          )}
          <p className={`text-sm ${estilo.text} ${titulo ? 'mt-1' : ''}`}>
            {mensagem}
          </p>
        </div>
        {fechavel && onFechar && (
          <div className="ml-auto pl-3">
            <button
              onClick={onFechar}
              className={`inline-flex rounded-md p-1.5 hover:bg-white/50 focus:outline-none transition-colors ${estilo.text}`}
            >
              <X className="w-4 h-4" />
            </button>
          </div>
        )}
      </div>
    </div>
  );
}

// ============================================================================
// ATALHOS PARA TIPOS COMUNS
// ============================================================================
export function AlertaErro(props: Omit<AlertaProps, 'tipo'>) {
  return <Alerta tipo="erro" {...props} />;
}

export function AlertaAviso(props: Omit<AlertaProps, 'tipo'>) {
  return <Alerta tipo="aviso" {...props} />;
}

export function AlertaSucesso(props: Omit<AlertaProps, 'tipo'>) {
  return <Alerta tipo="sucesso" {...props} />;
}

export function AlertaInfo(props: Omit<AlertaProps, 'tipo'>) {
  return <Alerta tipo="info" {...props} />;
}

export default Alerta;
