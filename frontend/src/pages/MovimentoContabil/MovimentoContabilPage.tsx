import { useState } from 'react';
import { FileText, Plus } from 'lucide-react';
import { CabecalhoPagina } from '../../components/common';
import { MovimentoContabilList } from './MovimentoContabilList';
import { MovimentoContabilForm } from './MovimentoContabilForm';

export default function MovimentoContabilPage() {
  const [view, setView] = useState<'list' | 'form'>('list');
  const [selectedId, setSelectedId] = useState<number | undefined>(undefined);

  const handleNovo = () => {
    setSelectedId(undefined);
    setView('form');
  };

  const handleEditar = (id: number) => {
    setSelectedId(id);
    setView('form');
  };

  const handleClose = () => {
    setView('list');
    setSelectedId(undefined);
  };

  const handleSaved = () => {
    setView('list');
    setSelectedId(undefined);
  };

  return (
    <div className="h-full flex flex-col">
      {view === 'list' && (
        <div className="space-y-6 pb-8">
          <CabecalhoPagina 
            titulo="Movimento Contábil" 
            subtitulo="Gerencie entradas, saídas e ajustes de estoque contábil"
            icone={FileText}
            acoes={
              <button
                onClick={handleNovo}
                className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all font-medium shadow-lg shadow-blue-600/25 active:scale-95"
              >
                <Plus className="w-5 h-5" />
                <span>Novo Movimento</span>
              </button>
            }
          />

          <div className="px-6">
            <MovimentoContabilList 
              onEditar={handleEditar} 
            />
          </div>
        </div>
      )}

      {view === 'form' && (
        <div className="flex-1 overflow-hidden">
          <MovimentoContabilForm 
            movimentoId={selectedId} 
            onClose={handleClose} 
            onSuccess={handleSaved} 
          />
        </div>
      )}
    </div>
  );
}

