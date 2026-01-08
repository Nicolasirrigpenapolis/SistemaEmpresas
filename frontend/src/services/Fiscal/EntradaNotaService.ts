import api from '../api';

export interface NFeImportDto {
  chaveAcesso: string;
  numeroNota: string;
  serie: string;
  dataEmissao: string;
  emitente: {
    cnpj: string;
    nome: string;
    nomeFantasia: string;
    inscricaoEstadual: string;
  };
  itens: NFeItemDto[];
  valorTotal: number;
  valorProdutos: number;
  valorIcms: number;
  valorIpi: number;
}

export interface NFeItemDto {
  codigoProdutoFornecedor: string;
  descricaoProdutoFornecedor: string;
  ncm: string;
  cfop: string;
  unidadeMedida: string;
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
  produtoIdSistema?: number;
  nomeProdutoSistema?: string;
  valorIcms: number;
  valorIpi: number;
  aliquotaIcms: number;
}

export const EntradaNotaService = {
  uploadXml: async (file: File): Promise<NFeImportDto> => {
    const formData = new FormData();
    formData.append('file', file);

    const response = await api.post<NFeImportDto>('/fiscal/entrada-nota/upload-xml', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });

    return response.data;
  },
};
