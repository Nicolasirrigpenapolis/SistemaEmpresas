import React, { useState } from 'react';
import { Upload, CheckCircle, AlertCircle, ArrowRight, Package, Search } from 'lucide-react';
import { EntradaNotaService, type NFeImportDto } from '../../services/Fiscal/EntradaNotaService';
import { toast } from 'react-hot-toast';

const EntradaNotaPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [nfeData, setNfeData] = useState<NFeImportDto | null>(null);
  const [dragActive, setDragActive] = useState(false);

  const handleFileUpload = async (file: File) => {
    if (!file.name.endsWith('.xml')) {
      toast.error('Por favor, selecione um arquivo XML de NFe.');
      return;
    }

    setLoading(true);
    try {
      const data = await EntradaNotaService.uploadXml(file);
      setNfeData(data);
      toast.success('XML processado com sucesso!');
    } catch (error: any) {
      console.error(error);
      toast.error(error.response?.data || 'Erro ao processar o XML.');
    } finally {
      setLoading(false);
    }
  };

  const handleDrag = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (e.type === "dragenter" || e.type === "dragover") {
      setDragActive(true);
    } else if (e.type === "dragleave") {
      setDragActive(false);
    }
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);
    if (e.dataTransfer.files && e.dataTransfer.files[0]) {
      handleFileUpload(e.dataTransfer.files[0]);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    e.preventDefault();
    if (e.target.files && e.target.files[0]) {
      handleFileUpload(e.target.files[0]);
    }
  };

  return (
    <div className="p-6 max-w-7xl mx-auto">
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Entrada de Notas via XML</h1>
        <p className="text-gray-600">Importe arquivos XML de NFe para dar entrada no estoque e financeiro.</p>
      </div>

      {!nfeData ? (
        <div 
          className={`border-2 border-dashed rounded-xl p-12 text-center transition-colors ${
            dragActive ? 'border-blue-500 bg-blue-50' : 'border-gray-300 bg-white'
          }`}
          onDragEnter={handleDrag}
          onDragLeave={handleDrag}
          onDragOver={handleDrag}
          onDrop={handleDrop}
        >
          <div className="flex flex-col items-center">
            <div className="p-4 bg-blue-100 rounded-full mb-4">
              <Upload className="w-8 h-8 text-blue-600" />
            </div>
            <h3 className="text-lg font-medium text-gray-900">Arraste o XML aqui</h3>
            <p className="text-gray-500 mb-6">ou clique para selecionar o arquivo do seu computador</p>
            
            <label className="cursor-pointer bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition-colors">
              Selecionar Arquivo
              <input 
                type="file" 
                className="hidden" 
                accept=".xml" 
                onChange={handleChange}
                disabled={loading}
              />
            </label>
            
            {loading && (
              <div className="mt-4 flex items-center text-blue-600">
                <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-blue-600 mr-2"></div>
                Processando XML...
              </div>
            )}
          </div>
        </div>
      ) : (
        <div className="space-y-6">
          {/* Cabeçalho da Nota */}
          <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
            <div className="flex justify-between items-start mb-6">
              <div>
                <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800 mb-2">
                  XML Carregado
                </span>
                <h2 className="text-xl font-bold text-gray-900">NFe nº {nfeData.numeroNota} - Série {nfeData.serie}</h2>
                <p className="text-sm text-gray-500 font-mono">{nfeData.chaveAcesso}</p>
              </div>
              <button 
                onClick={() => setNfeData(null)}
                className="text-sm text-blue-600 hover:underline"
              >
                Carregar outro XML
              </button>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="p-4 bg-gray-50 rounded-lg">
                <p className="text-xs text-gray-500 uppercase font-semibold mb-1">Fornecedor</p>
                <p className="font-medium text-gray-900">{nfeData.emitente.nome}</p>
                <p className="text-sm text-gray-600">CNPJ: {nfeData.emitente.cnpj}</p>
              </div>
              <div className="p-4 bg-gray-50 rounded-lg">
                <p className="text-xs text-gray-500 uppercase font-semibold mb-1">Data de Emissão</p>
                <p className="font-medium text-gray-900">{new Date(nfeData.dataEmissao).toLocaleDateString('pt-BR')}</p>
              </div>
              <div className="p-4 bg-gray-50 rounded-lg">
                <p className="text-xs text-gray-500 uppercase font-semibold mb-1">Valor Total</p>
                <p className="text-xl font-bold text-blue-600">
                  {nfeData.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                </p>
              </div>
            </div>
          </div>

          {/* Itens da Nota */}
          <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
            <div className="px-6 py-4 border-b border-gray-200 bg-gray-50 flex justify-between items-center">
              <h3 className="font-bold text-gray-900 flex items-center">
                <Package className="w-5 h-5 mr-2 text-gray-500" />
                Itens da Nota ({nfeData.itens.length})
              </h3>
              <span className="text-xs text-gray-500 italic">Associe os itens do XML aos produtos do seu sistema</span>
            </div>
            
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead>
                  <tr className="bg-gray-50">
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Item no XML</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Qtd / Un</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Vlr. Unit</th>
                    <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">Vínculo Sistema</th>
                    <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Total</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {nfeData.itens.map((item, index) => (
                    <tr key={index} className="hover:bg-gray-50 transition-colors">
                      <td className="px-6 py-4">
                        <div className="text-sm font-medium text-gray-900">{item.descricaoProdutoFornecedor}</div>
                        <div className="text-xs text-gray-500">Cód: {item.codigoProdutoFornecedor} | NCM: {item.ncm}</div>
                      </td>
                      <td className="px-6 py-4 text-sm text-gray-600">
                        {item.quantidade} {item.unidadeMedida}
                      </td>
                      <td className="px-6 py-4 text-sm text-gray-600">
                        {item.valorUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                      </td>
                      <td className="px-6 py-4">
                        <div className="flex items-center justify-center">
                          {item.produtoIdSistema ? (
                            <div className="flex items-center text-green-600 bg-green-50 px-3 py-1 rounded-full border border-green-100">
                              <CheckCircle className="w-4 h-4 mr-2" />
                              <span className="text-xs font-medium">{item.nomeProdutoSistema}</span>
                            </div>
                          ) : (
                            <button className="flex items-center text-blue-600 hover:text-blue-800 bg-blue-50 px-3 py-1 rounded-full border border-blue-100 transition-colors">
                              <Search className="w-4 h-4 mr-2" />
                              <span className="text-xs font-medium">Vincular Produto</span>
                            </button>
                          )}
                        </div>
                      </td>
                      <td className="px-6 py-4 text-right text-sm font-bold text-gray-900">
                        {item.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>

          {/* Ações Finais */}
          <div className="flex justify-end space-x-4">
            <button 
              className="px-6 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50 transition-colors"
              onClick={() => setNfeData(null)}
            >
              Cancelar
            </button>
            <button 
              className="px-6 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors flex items-center shadow-sm"
              disabled={nfeData.itens.some(i => !i.produtoIdSistema)}
            >
              Confirmar Entrada
              <ArrowRight className="w-4 h-4 ml-2" />
            </button>
          </div>
          
          {nfeData.itens.some(i => !i.produtoIdSistema) && (
            <div className="flex items-center justify-end text-amber-600 text-sm">
              <AlertCircle className="w-4 h-4 mr-1" />
              Vincule todos os produtos para confirmar a entrada.
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default EntradaNotaPage;
