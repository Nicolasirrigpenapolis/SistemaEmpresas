import { useState, useEffect } from 'react';
import { Search, X, Check } from 'lucide-react';

interface BuscaModalProps<T> {
  isOpen: boolean;
  onClose: () => void;
  onSelect: (item: T) => void;
  items: T[];
  titulo: string;
  placeholder?: string;
  renderItem: (item: T) => React.ReactNode;
  getItemKey: (item: T) => string | number;
  searchFields: (item: T) => string[];
  selectedId?: number | string;
}

export function BuscaModal<T>({
  isOpen,
  onClose,
  onSelect,
  items,
  titulo,
  placeholder = 'Pesquisar...',
  renderItem,
  getItemKey,
  searchFields,
  selectedId,
}: BuscaModalProps<T>) {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredItems, setFilteredItems] = useState<T[]>(items);

  useEffect(() => {
    if (!searchTerm.trim()) {
      setFilteredItems(items);
      return;
    }

    const term = searchTerm.toLowerCase();
    const filtered = items.filter((item) =>
      searchFields(item).some((field) =>
        field.toLowerCase().includes(term)
      )
    );
    setFilteredItems(filtered);
  }, [searchTerm, items, searchFields]);

  useEffect(() => {
    if (isOpen) {
      setSearchTerm('');
    }
  }, [isOpen]);

  if (!isOpen) return null;

  const handleSelect = (item: T) => {
    onSelect(item);
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm">
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-2xl max-h-[80vh] flex flex-col animate-in fade-in zoom-in duration-200">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-gray-200">
          <h2 className="text-xl font-bold text-gray-900">{titulo}</h2>
          <button
            onClick={onClose}
            className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
          >
            <X className="w-5 h-5 text-gray-500" />
          </button>
        </div>

        {/* Search */}
        <div className="p-6 border-b border-gray-200">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder={placeholder}
              autoFocus
              className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all"
            />
          </div>
        </div>

        {/* List */}
        <div className="flex-1 overflow-y-auto p-4">
          {filteredItems.length === 0 ? (
            <div className="text-center py-12">
              <Search className="w-12 h-12 mx-auto mb-4 text-gray-300" />
              <p className="text-gray-500">Nenhum resultado encontrado</p>
            </div>
          ) : (
            <div className="space-y-2">
              {filteredItems.map((item) => {
                const key = getItemKey(item);
                const isSelected = selectedId === key;
                
                return (
                  <button
                    key={key}
                    onClick={() => handleSelect(item)}
                    className={`w-full text-left p-4 rounded-xl border-2 transition-all duration-200 ${
                      isSelected
                        ? 'border-blue-500 bg-blue-50 shadow-sm'
                        : 'border-gray-200 hover:border-blue-300 hover:bg-gray-50'
                    }`}
                  >
                    <div className="flex items-center justify-between">
                      <div className="flex-1">{renderItem(item)}</div>
                      {isSelected && (
                        <Check className="w-5 h-5 text-blue-600 ml-3 flex-shrink-0" />
                      )}
                    </div>
                  </button>
                );
              })}
            </div>
          )}
        </div>

        {/* Footer */}
        <div className="p-6 border-t border-gray-200 bg-gray-50">
          <div className="flex justify-between items-center text-sm text-gray-600">
            <span>
              {filteredItems.length} {filteredItems.length === 1 ? 'resultado' : 'resultados'}
            </span>
            <button
              onClick={onClose}
              className="px-4 py-2 text-gray-700 hover:bg-gray-200 rounded-lg transition-colors"
            >
              Cancelar
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
