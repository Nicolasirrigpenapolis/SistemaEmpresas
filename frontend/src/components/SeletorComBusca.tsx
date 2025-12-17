import { useState, useEffect, useRef } from 'react';
import { Search, ChevronDown, X, Check } from 'lucide-react';

interface SeletorComBuscaProps<T> {
  label: string;
  value: number;
  descricao: string;
  onSelect: (id: number, descricao: string) => void;
  items: T[];
  getItemId: (item: T) => number;
  getItemDescricao: (item: T) => string;
  getItemSecundario?: (item: T) => string;
  placeholder?: string;
  disabled?: boolean;
  loading?: boolean;
  required?: boolean;
}

export function SeletorComBusca<T>({
  label,
  value,
  descricao,
  onSelect,
  items = [],
  getItemId,
  getItemDescricao,
  getItemSecundario,
  placeholder = 'Selecione...',
  disabled = false,
  loading = false,
  required = false,
}: SeletorComBuscaProps<T>) {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [modalSearchTerm, setModalSearchTerm] = useState('');
  const [filteredItems, setFilteredItems] = useState<T[]>([]);
  const [highlightedIndex, setHighlightedIndex] = useState(-1);
  
  const containerRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const dropdownRef = useRef<HTMLDivElement>(null);

  // Filtrar items baseado no termo de busca
  useEffect(() => {
    const itemsArray = Array.isArray(items) ? items : [];
    const term = (isModalOpen ? modalSearchTerm : searchTerm).toLowerCase().trim();
    
    // Ordenar por ID (do menor para o maior)
    const sortedItems = [...itemsArray].sort((a, b) => getItemId(a) - getItemId(b));
    
    if (!term) {
      setFilteredItems(sortedItems);
      return;
    }

    const filtered = sortedItems.filter((item) => {
      const id = String(getItemId(item));
      const desc = getItemDescricao(item).toLowerCase();
      const sec = getItemSecundario ? getItemSecundario(item).toLowerCase() : '';
      return id.includes(term) || desc.includes(term) || sec.includes(term);
    });
    setFilteredItems(filtered);
    setHighlightedIndex(filtered.length > 0 ? 0 : -1);
  }, [searchTerm, modalSearchTerm, items, getItemDescricao, getItemSecundario, getItemId, isModalOpen]);

  // Fechar dropdown ao clicar fora
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsDropdownOpen(false);
        setSearchTerm('');
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  // Scroll para item highlightado
  useEffect(() => {
    if (highlightedIndex >= 0 && dropdownRef.current) {
      const items = dropdownRef.current.querySelectorAll('[data-item]');
      items[highlightedIndex]?.scrollIntoView({ block: 'nearest' });
    }
  }, [highlightedIndex]);

  const handleSelect = (item: T) => {
    onSelect(getItemId(item), getItemDescricao(item));
    setIsDropdownOpen(false);
    setIsModalOpen(false);
    setSearchTerm('');
    setModalSearchTerm('');
  };

  const handleClear = (e: React.MouseEvent) => {
    e.stopPropagation();
    onSelect(0, '');
    setSearchTerm('');
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setSearchTerm(newValue);
    if (!isDropdownOpen && newValue) {
      setIsDropdownOpen(true);
    }
  };

  const handleInputFocus = () => {
    setIsDropdownOpen(true);
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (!isDropdownOpen) {
      if (e.key === 'ArrowDown' || e.key === 'Enter') {
        setIsDropdownOpen(true);
        e.preventDefault();
      }
      return;
    }

    switch (e.key) {
      case 'ArrowDown':
        e.preventDefault();
        setHighlightedIndex((prev) => 
          prev < filteredItems.length - 1 ? prev + 1 : prev
        );
        break;
      case 'ArrowUp':
        e.preventDefault();
        setHighlightedIndex((prev) => (prev > 0 ? prev - 1 : 0));
        break;
      case 'Enter':
        e.preventDefault();
        if (highlightedIndex >= 0 && filteredItems[highlightedIndex]) {
          handleSelect(filteredItems[highlightedIndex]);
        }
        break;
      case 'Escape':
        setIsDropdownOpen(false);
        setSearchTerm('');
        break;
    }
  };

  const openModal = (e: React.MouseEvent) => {
    e.stopPropagation();
    setIsDropdownOpen(false);
    setIsModalOpen(true);
    setModalSearchTerm('');
  };

  return (
    <div className="relative" ref={containerRef}>
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {label}
        {required && <span className="text-red-500 ml-1">*</span>}
      </label>

      {/* Container do input + botão de busca */}
      <div className="flex gap-1">
        {/* Campo principal com dropdown */}
        <div className="relative flex-1">
          <div
            className={`
              flex items-center border-2 rounded-lg transition-all duration-200 bg-white
              ${disabled || loading
                ? 'bg-gray-50 border-gray-200 cursor-not-allowed'
                : isDropdownOpen
                  ? 'border-blue-500 ring-2 ring-blue-100'
                  : 'border-gray-300 hover:border-gray-400'
              }
            `}
          >
            {/* Badge do código */}
            {value > 0 && (
              <span className="ml-2 inline-flex items-center justify-center px-2 py-0.5 rounded bg-blue-100 text-blue-700 text-xs font-bold flex-shrink-0">
                {value}
              </span>
            )}

            {/* Input para digitar/filtrar */}
            <input
              ref={inputRef}
              type="text"
              value={isDropdownOpen ? searchTerm : (descricao || '')}
              onChange={handleInputChange}
              onFocus={handleInputFocus}
              onKeyDown={handleKeyDown}
              placeholder={placeholder}
              disabled={disabled || loading}
              className={`
                flex-1 px-2 py-2.5 bg-transparent outline-none text-sm
                ${disabled || loading ? 'cursor-not-allowed text-gray-500' : 'text-gray-900'}
                ${value > 0 && !isDropdownOpen ? 'font-medium' : ''}
              `}
            />

            {/* Botões internos */}
            <div className="flex items-center pr-1 gap-0.5">
              {value > 0 && !disabled && (
                <button
                  onClick={handleClear}
                  className="p-1.5 hover:bg-gray-100 rounded transition-colors"
                  type="button"
                  title="Limpar"
                >
                  <X className="w-4 h-4 text-gray-400" />
                </button>
              )}
              <button
                onClick={() => !disabled && setIsDropdownOpen(!isDropdownOpen)}
                className="p-1.5 hover:bg-gray-100 rounded transition-colors"
                type="button"
                disabled={disabled || loading}
              >
                <ChevronDown 
                  className={`w-4 h-4 text-gray-400 transition-transform duration-200 ${isDropdownOpen ? 'rotate-180' : ''}`} 
                />
              </button>
            </div>
          </div>

          {/* Dropdown */}
          {isDropdownOpen && (
            <div 
              ref={dropdownRef}
              className="absolute left-0 right-0 z-[9999] mt-1 bg-white border-2 border-gray-300 rounded-lg shadow-xl max-h-64 overflow-y-auto"
              style={{ top: '100%' }}
            >
              {loading ? (
                <div className="p-4 text-center text-gray-500">
                  <div className="animate-spin w-5 h-5 border-2 border-blue-500 border-t-transparent rounded-full mx-auto mb-2"></div>
                  Carregando...
                </div>
              ) : filteredItems.length === 0 ? (
                <div className="p-4 text-center text-gray-500">
                  <Search className="w-8 h-8 mx-auto mb-2 text-gray-300" />
                  <p className="text-sm">Nenhum resultado</p>
                  {searchTerm && (
                    <p className="text-xs text-gray-400 mt-1">
                      Tente usar a busca avançada
                    </p>
                  )}
                </div>
              ) : (
                filteredItems.map((item, index) => {
                  const itemId = getItemId(item);
                  const itemDesc = getItemDescricao(item);
                  const isSelected = itemId === value;
                  const isHighlighted = index === highlightedIndex;

                  return (
                    <div
                      key={itemId}
                      data-item
                      onClick={() => handleSelect(item)}
                      className={`
                        flex items-center gap-2 px-3 py-2 cursor-pointer transition-colors
                        ${isHighlighted ? 'bg-blue-50' : 'hover:bg-gray-50'}
                        ${isSelected ? 'bg-blue-50' : ''}
                        ${index !== filteredItems.length - 1 ? 'border-b border-gray-100' : ''}
                      `}
                    >
                      <span className={`
                        w-8 text-center text-xs font-semibold py-0.5 rounded
                        ${isSelected ? 'bg-blue-600 text-white' : 'bg-gray-100 text-gray-600'}
                      `}>
                        {itemId}
                      </span>
                      <span className={`flex-1 text-sm truncate ${isSelected ? 'font-medium text-blue-900' : 'text-gray-900'}`}>
                        {itemDesc}
                      </span>
                      {isSelected && (
                        <Check className="w-4 h-4 text-blue-600 flex-shrink-0" />
                      )}
                    </div>
                  );
                })
              )}
            </div>
          )}
        </div>

        {/* Botão de busca avançada (lupa) */}
        <button
          type="button"
          onClick={openModal}
          disabled={disabled || loading}
          className={`
            px-3 border-2 rounded-lg transition-all duration-200 flex items-center justify-center
            ${disabled || loading
              ? 'bg-gray-50 border-gray-200 cursor-not-allowed text-gray-400'
              : 'bg-blue-50 border-blue-200 hover:bg-blue-100 hover:border-blue-300 text-blue-600'
            }
          `}
          title="Busca avançada"
        >
          <Search className="w-5 h-5" />
        </button>
      </div>

      {/* Modal de busca avançada */}
      {isModalOpen && (
        <>
          {/* Backdrop */}
          <div
            className="fixed inset-0 z-40 bg-black/30 backdrop-blur-sm"
            onClick={() => setIsModalOpen(false)}
          />

          {/* Modal */}
          <div className="fixed inset-4 md:inset-auto md:left-1/2 md:top-1/2 md:-translate-x-1/2 md:-translate-y-1/2 md:w-full md:max-w-2xl z-50 bg-white rounded-xl shadow-2xl flex flex-col max-h-[90vh] md:max-h-[600px]">
            {/* Header */}
            <div className="p-4 border-b border-gray-200">
              <div className="flex items-center justify-between mb-3">
                <h3 className="text-lg font-bold text-gray-900 flex items-center gap-2">
                  <Search className="w-5 h-5 text-blue-600" />
                  Buscar {label}
                </h3>
                <button
                  onClick={() => setIsModalOpen(false)}
                  className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
                >
                  <X className="w-5 h-5 text-gray-500" />
                </button>
              </div>

              {/* Campo de busca */}
              <div className="relative">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                <input
                  type="text"
                  value={modalSearchTerm}
                  onChange={(e) => setModalSearchTerm(e.target.value)}
                  placeholder="Digite código ou descrição..."
                  autoFocus
                  className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all text-sm"
                />
                {modalSearchTerm && (
                  <button
                    onClick={() => setModalSearchTerm('')}
                    className="absolute right-3 top-1/2 -translate-y-1/2 p-1 hover:bg-gray-100 rounded"
                  >
                    <X className="w-4 h-4 text-gray-400" />
                  </button>
                )}
              </div>

              {/* Info de resultados */}
              <div className="mt-2 text-xs text-gray-500">
                {filteredItems.length} {filteredItems.length === 1 ? 'resultado' : 'resultados'} encontrados
              </div>
            </div>

            {/* Lista */}
            <div className="flex-1 overflow-y-auto">
              {loading ? (
                <div className="p-8 text-center text-gray-500">
                  <div className="animate-spin w-8 h-8 border-3 border-blue-500 border-t-transparent rounded-full mx-auto mb-3"></div>
                  Carregando dados...
                </div>
              ) : filteredItems.length === 0 ? (
                <div className="p-8 text-center">
                  <Search className="w-12 h-12 mx-auto mb-3 text-gray-300" />
                  <p className="text-gray-500 font-medium">Nenhum resultado encontrado</p>
                  <p className="text-gray-400 text-sm mt-1">
                    Tente pesquisar com outros termos
                  </p>
                </div>
              ) : (
                <table className="w-full">
                  <thead className="bg-gray-50 sticky top-0">
                    <tr>
                      <th className="px-4 py-2 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider w-20">
                        Código
                      </th>
                      <th className="px-4 py-2 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                        Descrição
                      </th>
                      {getItemSecundario && (
                        <th className="px-4 py-2 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider w-32">
                          Info
                        </th>
                      )}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-100">
                    {filteredItems.map((item) => {
                      const itemId = getItemId(item);
                      const itemDesc = getItemDescricao(item);
                      const itemSec = getItemSecundario?.(item);
                      const isSelected = itemId === value;

                      return (
                        <tr
                          key={itemId}
                          onClick={() => handleSelect(item)}
                          className={`
                            cursor-pointer transition-colors
                            ${isSelected 
                              ? 'bg-blue-50 hover:bg-blue-100' 
                              : 'hover:bg-gray-50'
                            }
                          `}
                        >
                          <td className="px-4 py-3">
                            <span className={`
                              inline-flex items-center justify-center min-w-[2.5rem] px-2 py-1 rounded text-sm font-semibold
                              ${isSelected 
                                ? 'bg-blue-600 text-white' 
                                : 'bg-gray-100 text-gray-700'
                              }
                            `}>
                              {itemId}
                            </span>
                          </td>
                          <td className={`px-4 py-3 text-sm ${isSelected ? 'font-medium text-blue-900' : 'text-gray-900'}`}>
                            {itemDesc}
                          </td>
                          {getItemSecundario && (
                            <td className="px-4 py-3 text-sm text-gray-500">
                              {itemSec}
                            </td>
                          )}
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              )}
            </div>

            {/* Footer */}
            <div className="p-3 border-t border-gray-200 bg-gray-50 flex justify-between items-center">
              <span className="text-xs text-gray-500">
                Clique em um item para selecionar
              </span>
              <button
                onClick={() => setIsModalOpen(false)}
                className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
              >
                Fechar
              </button>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
