import api from '../api';
import type {
  EmitenteListDto,
  EmitenteDto,
  EmitenteCreateUpdateDto,
  ConsultaCnpjDto,
  ParametrosDto,
} from '../../types/Emitentes/emitente';

const BASE_URL = '/configuracoes';
const PARAMETROS_URL = '/configuracoes/parametros';

export const emitenteService = {
  /**
   * Lista todos os emitentes
   */
  async listar(): Promise<EmitenteListDto[]> {
    const response = await api.get<EmitenteListDto[]>(`${BASE_URL}/emitentes`);
    return response.data;
  },

  /**
   * Obtém o emitente atual (empresa logada)
   */
  async obterAtual(): Promise<EmitenteDto> {
    const response = await api.get<EmitenteDto>(`${BASE_URL}/emitente/atual`);
    return response.data;
  },

  /**
   * Obtém um emitente por ID
   */
  async obterPorId(id: number): Promise<EmitenteDto> {
    const response = await api.get<EmitenteDto>(`${BASE_URL}/emitente/${id}`);
    return response.data;
  },

  /**
   * Cria um novo emitente
   */
  async criar(emitente: EmitenteCreateUpdateDto): Promise<EmitenteDto> {
    const response = await api.post<EmitenteDto>(`${BASE_URL}/emitente`, emitente);
    return response.data;
  },

  /**
   * Atualiza um emitente existente
   */
  async atualizar(id: number, emitente: EmitenteCreateUpdateDto): Promise<EmitenteDto> {
    const response = await api.put<EmitenteDto>(`${BASE_URL}/emitente/${id}`, emitente);
    return response.data;
  },

  /**
   * Remove um emitente
   */
  async remover(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/emitente/${id}`);
  },

  /**
   * Consulta CNPJ na Receita Federal
   */
  async consultarCnpj(cnpj: string): Promise<ConsultaCnpjDto> {
    // Remove formatação do CNPJ
    const cnpjLimpo = cnpj.replace(/\D/g, '');
    const response = await api.get<ConsultaCnpjDto>(`${BASE_URL}/emitente/consultar-cnpj/${cnpjLimpo}`);
    return response.data;
  },

  /**
   * Faz upload do certificado digital para um emitente
   */
  async uploadCertificado(id: number, arquivo: File, senha: string): Promise<any> {
    const formData = new FormData();
    formData.append('arquivo', arquivo);
    formData.append('senha', senha);

    const response = await api.post(`${BASE_URL}/emitente/${id}/certificado`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  },

  /**
   * Valida e retorna informações do certificado digital
   */
  async validarCertificado(id: number): Promise<any> {
    const response = await api.get(`${BASE_URL}/emitente/${id}/certificado/validar`);
    return response.data;
  },

  /**
   * Remove o certificado digital de um emitente
   */
  async removerCertificado(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/emitente/${id}/certificado`);
  },
};

// Serviço para Parâmetros do Sistema
export const parametrosService = {
  /**
   * Obtém os parâmetros do sistema
   */
  async obter(): Promise<ParametrosDto> {
    const response = await api.get<ParametrosDto>(PARAMETROS_URL);
    return response.data;
  },

  /**
   * Atualiza os parâmetros do sistema
   */
  async atualizar(parametros: ParametrosDto): Promise<ParametrosDto> {
    const response = await api.put<ParametrosDto>(PARAMETROS_URL, parametros);
    return response.data;
  },
};

export default emitenteService;
