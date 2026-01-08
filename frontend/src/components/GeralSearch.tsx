import { useState, useEffect, useCallback, useRef } from 'react';
import { User, Loader2, X, Check, ChevronDown } from 'lucide-react';
import api from '../services/api';

interface Geral {
  sequenciaDoGeral: number;
  razaoSocial: string;
  cpfCnpj?: string;
}

interface GeralSearchProps {
  value: number;
  descricao: string;
  onSelect: (id: number, descricao: string) => void;
  label?: string;
  required?: boolean;
  disabled?: boolean;
}

export function GeralSearch({ value, descricao, onSelect, label = "Geral (Fornecedor/Cliente)", required, disabled }: GeralSearchProps) {
  const [busca, setBusca] = useState(descricao);
  const [resultados, setResultados] = useState<Geral[]>([]);
  const [loading, setLoading] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const [focado, setFocado] = useState(false);
  
  const containerRef = useRef<HTMLDivElement>(null);

  const buscarGerais = useCallback(async (termo: string) => {
    if (!termo || termo.length < 1) {
      setResultados([]);
      return;
    }

    try {
      setLoading(true);
      const response = await api.get<Geral[]>(`/geral/buscar?termo=${termo}`);
      setResultados(response.data);
    } catch (err) {
      console.error('Erro ao buscar gerais:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    if (isOpen && busca !== descricao) {
      const timer = setTimeout(() => buscarGerais(busca), 300);
      return () => clearTimeout(timer);
    }
  }, [busca, isOpen, buscarGerais, descricao]);

  // Fechar ao clicar fora
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsOpen(false);
        if (!value) setBusca('');
        else setBusca(descricao);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [value, descricao]);

  const handleSelect = (g: Geral) => {
    onSelect(g.sequenciaDoGeral, g.razaoSocial);
    setBusca(g.razaoSocial);
    setIsOpen(false);
  };

  const handleClear = (e: React.MouseEvent) => {
    e.stopPropagation();
    onSelect(0, '');
    setBusca('');
    setResultados([]);
  };

  return (
    <div className="relative" ref={containerRef}>
      <div className={`
        relative flex items-center border rounded-xl transition-all duration-200
        ${focado || isOpen ? 'border-blue-500 ring-2 ring-blue-500/20 shadow-sm' : 'border-gray-200 hover:border-gray-300'}
        ${disabled ? 'bg-gray-50 cursor-not-allowed' : 'bg-white'}
      `}>
        <span className={`pl-3 ${focado || isOpen ? 'text-blue-500' : 'text-gray-400'} transition-colors`}>
          <User className="w-4 h-4" />
        </span>
        
        <div className="relative flex-1">
          <input
            type="text"
            value={busca}
            onChange={(e) => {
              setBusca(e.target.value);
              setIsOpen(true);
            }}
            onFocus={() => {
              setFocado(true);
              setIsOpen(true);
            }}
            onBlur={() => setFocado(false)}
            disabled={disabled}
            placeholder={focado ? "Digite o nome ou código..." : ""}
            className="w-full px-3 py-3 bg-transparent outline-none text-gray-900 placeholder:text-gray-400 disabled:cursor-not-allowed text-sm font-medium"
          />
          
          <label className={`
            absolute left-1 transition-all duration-200 pointer-events-none
            ${focado || isOpen || busca 
              ? '-top-2.5 text-[10px] bg-white px-1 rounded font-bold uppercase tracking-wider' 
              : 'top-3 text-sm'
            }
            ${focado || isOpen ? 'text-blue-600' : 'text-gray-500'}
          `}>
            {label}{required && <span className="text-red-500 ml-0.5">*</span>}
          </label>
        </div>

        <div className="flex items-center pr-2 gap-1">
          {loading && <Loader2 className="w-4 h-4 animate-spin text-blue-500" />}
          {busca && !disabled && (
            <button
              type="button"
              onClick={handleClear}
              className="p-1 hover:bg-gray-100 rounded-full text-gray-400 hover:text-gray-600 transition-colors"
            >
              <X className="w-3 h-3" />
            </button>
          )}
          <ChevronDown className={`w-4 h-4 text-gray-400 transition-transform duration-200 ${isOpen ? 'rotate-180' : ''}`} />
        </div>
      </div>

      {isOpen && (resultados.length > 0 || loading) && (
        <div className="absolute z-[100] w-full mt-2 bg-white border border-gray-200 rounded-2xl shadow-xl overflow-hidden animate-in fade-in zoom-in-95 duration-200">
          <div className="max-h-72 overflow-y-auto p-2 space-y-1">
            {loading && resultados.length === 0 ? (
              <div className="p-4 text-center text-gray-500 text-sm flex flex-col items-center gap-2">
                <Loader2 className="w-6 h-6 animate-spin text-blue-500" />
                Buscando registros...
              </div>
            ) : (
              resultados.map((g) => (
                <button
                  key={g.sequenciaDoGeral}
                  type="button"
                  className={`
                    w-full text-left px-4 py-3 rounded-xl transition-all flex items-center justify-between group
                    ${value === g.sequenciaDoGeral ? 'bg-blue-50 text-blue-700' : 'hover:bg-gray-50 text-gray-700'}
                  `}
                  onClick={() => handleSelect(g)}
                >
                  <div className="flex flex-col">
                    <span className="font-bold text-sm">{g.razaoSocial}</span>
                    <div className="flex items-center gap-2 mt-0.5">
                      <span className="text-[10px] font-mono bg-gray-100 text-gray-500 px-1.5 py-0.5 rounded uppercase">ID: {g.sequenciaDoGeral}</span>
                      {g.cpfCnpj && (
                        <span className="text-[10px] text-gray-400">CPF/CNPJ: {g.cpfCnpj}</span>
                      )}
                    </div>
                  </div>
                  {value === g.sequenciaDoGeral && <Check className="w-4 h-4 text-blue-600" />}
                </button>
              ))
            )}
          </div>
        </div>
      )}
    </div>
  );
}
