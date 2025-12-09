import api from './api';
import type {
  EmitenteListDto,
  EmitenteDto,
  EmitenteCreateUpdateDto,
  ConsultaCnpjDto,
} from '../types/emitente';

const BASE_URL = '/emitentes';

export const emitenteService = {
  /**
   * Lista todos os emitentes
   */
  async listar(): Promise<EmitenteListDto[]> {
    const response = await api.get<EmitenteListDto[]>(BASE_URL);
    return response.data;
  },

  /**
   * Obtém o emitente atual (empresa logada)
   */
  async obterAtual(): Promise<EmitenteDto> {
    const response = await api.get<EmitenteDto>(`${BASE_URL}/atual`);
    return response.data;
  },

  /**
   * Obtém um emitente por ID
   */
  async obterPorId(id: number): Promise<EmitenteDto> {
    const response = await api.get<EmitenteDto>(`${BASE_URL}/${id}`);
    return response.data;
  },

  /**
   * Cria um novo emitente
   */
  async criar(emitente: EmitenteCreateUpdateDto): Promise<EmitenteDto> {
    const response = await api.post<EmitenteDto>(BASE_URL, emitente);
    return response.data;
  },

  /**
   * Atualiza um emitente existente
   */
  async atualizar(id: number, emitente: EmitenteCreateUpdateDto): Promise<EmitenteDto> {
    const response = await api.put<EmitenteDto>(`${BASE_URL}/${id}`, emitente);
    return response.data;
  },

  /**
   * Remove um emitente
   */
  async remover(id: number): Promise<void> {
    await api.delete(`${BASE_URL}/${id}`);
  },

  /**
   * Consulta CNPJ na Receita Federal
   */
  async consultarCnpj(cnpj: string): Promise<ConsultaCnpjDto> {
    // Remove formatação do CNPJ
    const cnpjLimpo = cnpj.replace(/\D/g, '');
    const response = await api.get<ConsultaCnpjDto>(`${BASE_URL}/consultar-cnpj/${cnpjLimpo}`);
    return response.data;
  },
};

export default emitenteService;
