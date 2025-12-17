import { useState, useEffect, useRef, useCallback } from "react";
import { Search, X, ChevronDown, Loader2, Check, FileText } from "lucide-react";
import { classTribService } from "../services/Fiscal/classTribService";
import type { ClassTribAutocomplete } from '../services/Fiscal/classTribService';

interface ClassTribSelectorProps {
  value: number | null;
  onChange: (id: number | null, classTrib: ClassTribAutocomplete | null) => void;
  initialDisplayText?: string;
  disabled?: boolean;
  className?: string;
}

export default function ClassTribSelector({
  value,
  onChange,
  initialDisplayText,
  disabled = false,
  className = '',
}: ClassTribSelectorProps) {
  const [isOpen, setIsOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [options, setOptions] = useState<ClassTribAutocomplete[]>([]);
  const [loading, setLoading] = useState(false);
  const [displayText, setDisplayText] = useState(initialDisplayText || '');
  const [highlightedIndex, setHighlightedIndex] = useState(-1);
  
  const containerRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const listRef = useRef<HTMLUListElement>(null);

  // Debounce para pesquisa
  useEffect(() => {
    if (!searchTerm || searchTerm.length < 2) {
      setOptions([]);
      return;
    }

    const timeoutId = setTimeout(async () => {
      try {
        setLoading(true);
        const results = await classTribService.autocomplete(searchTerm, 20);
        setOptions(results);
        setHighlightedIndex(-1);
      } catch (error) {
        console.error('Erro ao buscar ClassTribs:', error);
        setOptions([]);
      } finally {
        setLoading(false);
      }
    }, 300);

    return () => clearTimeout(timeoutId);
  }, [searchTerm]);

  // Fechar dropdown ao clicar fora
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsOpen(false);
        setHighlightedIndex(-1);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  // Atualizar displayText quando value ou initialDisplayText mudar
  useEffect(() => {
    if (!value) {
      setDisplayText('');
    } else if (initialDisplayText) {
      setDisplayText(initialDisplayText);
    }
  }, [value, initialDisplayText]);

  const handleSelect = useCallback((item: ClassTribAutocomplete) => {
    setDisplayText(item.displayText);
    setSearchTerm('');
    setIsOpen(false);
    setHighlightedIndex(-1);
    onChange(item.id, item);
  }, [onChange]);

  const handleClear = () => {
    setDisplayText('');
    setSearchTerm('');
    setOptions([]);
    setHighlightedIndex(-1);
    onChange(null, null);
    inputRef.current?.focus();
  };

  const handleInputFocus = () => {
    setIsOpen(true);
    if (displayText) {
      setSearchTerm(displayText.split(' - ')[0]);
    }
  };

  // Navegação por teclado
  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (!isOpen) {
      if (e.key === 'ArrowDown' || e.key === 'Enter') {
        setIsOpen(true);
      }
      return;
    }

    switch (e.key) {
      case 'ArrowDown':
        e.preventDefault();
        setHighlightedIndex(prev => 
          prev < options.length - 1 ? prev + 1 : prev
        );
        break;
      case 'ArrowUp':
        e.preventDefault();
        setHighlightedIndex(prev => prev > 0 ? prev - 1 : 0);
        break;
      case 'Enter':
        e.preventDefault();
        if (highlightedIndex >= 0 && options[highlightedIndex]) {
          handleSelect(options[highlightedIndex]);
        }
        break;
      case 'Escape':
        setIsOpen(false);
        setHighlightedIndex(-1);
        break;
    }
  };

  // Scroll para item destacado
  useEffect(() => {
    if (highlightedIndex >= 0 && listRef.current) {
      const item = listRef.current.children[highlightedIndex] as HTMLElement;
      if (item) {
        item.scrollIntoView({ block: 'nearest' });
      }
    }
  }, [highlightedIndex]);

  // Destacar termo pesquisado no texto
  const highlightMatch = (text: string, term: string) => {
    if (!term || term.length < 2) return text;
    const regex = new RegExp(`(${term.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')})`, 'gi');
    const parts = text.split(regex);
    return parts.map((part, i) => 
      regex.test(part) ? (
        <mark key={i} className="bg-yellow-200 text-yellow-900 rounded px-0.5">{part}</mark>
      ) : part
    );
  };

  return (
    <div ref={containerRef} className={`relative ${className}`}>
      {/* Campo de entrada */}
      <div className={`
        relative flex items-center border-2 rounded-xl transition-all duration-200
        ${isOpen ? 'ring-2 ring-indigo-500/20 border-indigo-500 shadow-sm' : 'border-[var(--border)] hover:border-gray-300'}
        ${disabled ? 'bg-[var(--surface-muted)] cursor-not-allowed' : 'bg-[var(--surface)]'}
      `}>
        <Search className={`h-5 w-5 absolute left-3.5 transition-colors ${isOpen ? 'text-indigo-500' : 'text-gray-400'}`} />
        
        <input
          ref={inputRef}
          type="text"
          value={isOpen ? searchTerm : displayText}
          onChange={(e) => setSearchTerm(e.target.value)}
          onFocus={handleInputFocus}
          onKeyDown={handleKeyDown}
          placeholder="Pesquisar por código, CST ou descrição..."
          disabled={disabled}
          className={`
            w-full pl-11 pr-20 py-3 bg-transparent outline-none text-sm font-medium
            placeholder:text-gray-400 placeholder:font-normal
            ${disabled ? 'cursor-not-allowed text-[var(--text-muted)]' : 'text-[var(--text)]'}
          `}
        />

        <div className="absolute right-3 flex items-center gap-1.5">
          {loading && (
            <Loader2 className="h-4 w-4 text-indigo-500 animate-spin" />
          )}
          
          {value && !disabled && (
            <button
              type="button"
              onClick={handleClear}
              className="p-1.5 text-gray-400 hover:text-red-500 hover:bg-red-50 rounded-lg transition-colors"
              title="Limpar seleção"
            >
              <X className="h-4 w-4" />
            </button>
          )}
          
          <button
            type="button"
            onClick={() => !disabled && setIsOpen(!isOpen)}
            disabled={disabled}
            className={`p-1.5 rounded-lg transition-colors ${
              disabled ? 'cursor-not-allowed text-gray-300' : 'text-gray-400 hover:text-[var(--text-muted)] hover:bg-gray-100'
            }`}
          >
            <ChevronDown className={`h-4 w-4 transition-transform duration-200 ${isOpen ? 'rotate-180' : ''}`} />
          </button>
        </div>
      </div>

      {/* Dropdown com opções */}
      {isOpen && !disabled && (
        <div className="absolute z-50 w-full mt-2 bg-[var(--surface)] rounded-xl shadow-xl border border-[var(--border)] overflow-hidden">
          {/* Header do dropdown */}
          <div className="px-4 py-2.5 bg-gradient-to-r from-gray-50 to-white border-b border-[var(--border)]">
            <p className="text-xs text-[var(--text-muted)] flex items-center gap-1.5">
              <FileText className="h-3.5 w-3.5" />
              {loading ? 'Buscando tributações...' : 
               options.length > 0 ? `${options.length} resultado${options.length > 1 ? 's' : ''} encontrado${options.length > 1 ? 's' : ''}` :
               searchTerm.length >= 2 ? 'Nenhum resultado' : 'Digite para pesquisar'}
            </p>
          </div>

          <div className="max-h-80 overflow-y-auto">
            {loading ? (
              <div className="p-6 text-center">
                <Loader2 className="h-6 w-6 text-indigo-500 animate-spin mx-auto mb-2" />
                <p className="text-sm text-[var(--text-muted)]">Buscando tributações...</p>
              </div>
            ) : options.length > 0 ? (
              <ul ref={listRef} className="py-1">
                {options.map((item, index) => (
                  <li key={item.id}>
                    <button
                      type="button"
                      onClick={() => handleSelect(item)}
                      onMouseEnter={() => setHighlightedIndex(index)}
                      className={`
                        w-full text-left px-4 py-3 transition-colors
                        ${highlightedIndex === index ? 'bg-indigo-50' : ''}
                        ${item.id === value ? 'bg-indigo-100/70' : 'hover:bg-[var(--surface-muted)]'}
                      `}
                    >
                      <div className="flex items-start justify-between gap-3">
                        <div className="flex-1 min-w-0">
                          {/* Linha principal: Código + CST */}
                          <div className="flex items-center gap-2 mb-1">
                            <span className="inline-flex items-center px-2 py-0.5 bg-indigo-100 text-indigo-700 text-sm font-mono font-semibold rounded">
                              {highlightMatch(item.codigoClassTrib, searchTerm)}
                            </span>
                            <span className="inline-flex items-center px-2 py-0.5 bg-emerald-100 text-emerald-700 text-xs font-medium rounded">
                              CST {item.cst}
                            </span>
                          </div>
                          {/* Descrição completa - sem truncate */}
                          <p className="text-sm text-[var(--text-muted)] leading-relaxed">
                            {highlightMatch(item.descricao, searchTerm)}
                          </p>
                        </div>
                        {item.id === value && (
                          <div className="flex-shrink-0 p-1 bg-indigo-500 rounded-full">
                            <Check className="h-3.5 w-3.5 text-white" />
                          </div>
                        )}
                      </div>
                    </button>
                  </li>
                ))}
              </ul>
            ) : searchTerm.length >= 2 ? (
              <div className="p-6 text-center">
                <div className="w-12 h-12 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-3">
                  <Search className="h-6 w-6 text-gray-400" />
                </div>
                <p className="text-sm font-medium text-gray-700">Nenhuma tributação encontrada</p>
                <p className="text-xs text-[var(--text-muted)] mt-1">Tente outro código, CST ou descrição</p>
              </div>
            ) : (
              <div className="p-6 text-center">
                <div className="w-12 h-12 bg-indigo-50 rounded-full flex items-center justify-center mx-auto mb-3">
                  <Search className="h-6 w-6 text-indigo-400" />
                </div>
                <p className="text-sm font-medium text-gray-700">Digite para pesquisar</p>
                <p className="text-xs text-[var(--text-muted)] mt-1">Mínimo de 2 caracteres</p>
              </div>
            )}
          </div>

          {/* Dica de navegação */}
          {options.length > 0 && (
            <div className="px-4 py-2 bg-[var(--surface-muted)] border-t border-[var(--border)]">
              <p className="text-xs text-gray-400 flex items-center gap-3">
                <span className="flex items-center gap-1">
                  <kbd className="px-1.5 py-0.5 bg-[var(--surface)] border border-[var(--border)] rounded text-[var(--text-muted)] font-mono text-[10px]">↑</kbd>
                  <kbd className="px-1.5 py-0.5 bg-[var(--surface)] border border-[var(--border)] rounded text-[var(--text-muted)] font-mono text-[10px]">↓</kbd>
                  navegar
                </span>
                <span className="flex items-center gap-1">
                  <kbd className="px-1.5 py-0.5 bg-[var(--surface)] border border-[var(--border)] rounded text-[var(--text-muted)] font-mono text-[10px]">Enter</kbd>
                  selecionar
                </span>
                <span className="flex items-center gap-1">
                  <kbd className="px-1.5 py-0.5 bg-[var(--surface)] border border-[var(--border)] rounded text-[var(--text-muted)] font-mono text-[10px]">Esc</kbd>
                  fechar
                </span>
              </p>
            </div>
          )}
        </div>
      )}

      {/* Valor selecionado (oculto) */}
      {value && (
        <input type="hidden" value={value} />
      )}
    </div>
  );
}
