// ==========================================
// UTILITÁRIOS DE FORMATAÇÃO PARA UI
// ==========================================
// IMPORTANTE: Estas são apenas máscaras visuais para UX
// Validações de negócio devem estar no backend

/**
 * Remove caracteres não numéricos de uma string
 */
export const limparNumeros = (valor: string | number | undefined | null): string => {
  if (valor === undefined || valor === null) return '';
  return String(valor).replace(/\D/g, '');
};

/**
 * Formata CPF: 000.000.000-00
 */
export const formatarCPF = (valor?: string | null): string => {
  if (!valor) return '';
  const numeros = limparNumeros(valor);
  return numeros
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
};

/**
 * Formata CNPJ: 00.000.000/0000-00
 */
export const formatarCNPJ = (valor?: string | null): string => {
  if (!valor) return '';
  const numeros = limparNumeros(valor);
  return numeros
    .replace(/(\d{2})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1/$2')
    .replace(/(\d{4})(\d{1,2})$/, '$1-$2');
};

/**
 * Formata documento automaticamente (CPF ou CNPJ)
 */
export const formatarDocumento = (valor?: string | null): string => {
  if (!valor) return '';
  const numeros = limparNumeros(valor);
  if (numeros.length <= 11) {
    return formatarCPF(valor);
  }
  return formatarCNPJ(valor);
};

/**
 * Formata telefone: (00) 0000-0000 ou (00) 00000-0000
 */
export const formatarTelefone = (valor?: string | null): string => {
  if (!valor) return '';
  const numeros = limparNumeros(valor);
  
  if (numeros.length === 0) return '';
  if (numeros.length <= 2) return `(${numeros}`;
  if (numeros.length <= 6) return `(${numeros.slice(0, 2)}) ${numeros.slice(2)}`;
  if (numeros.length <= 10) {
    return `(${numeros.slice(0, 2)}) ${numeros.slice(2, 6)}-${numeros.slice(6)}`;
  }
  return `(${numeros.slice(0, 2)}) ${numeros.slice(2, 7)}-${numeros.slice(7, 11)}`;
};

/**
 * Formata CEP: 00000-000
 */
export const formatarCEP = (valor?: string | null): string => {
  if (!valor) return '';
  const numeros = limparNumeros(valor);
  return numeros.replace(/(\d{5})(\d{3})/, '$1-$2');
};

/**
 * Formata placa de veículo: ABC-1234 ou ABC-1D23 (Mercosul)
 */
export const formatarPlaca = (valor?: string | null): string => {
  if (!valor) return '';
  const limpo = valor.toUpperCase().replace(/[^A-Z0-9]/g, '');
  
  if (limpo.length <= 3) return limpo;
  
  return `${limpo.slice(0, 3)}-${limpo.slice(3, 7)}`;
};

/**
 * Formata valor monetário: R$ 1.234,56
 */
export const formatarMoeda = (valor?: number | null): string => {
  if (valor === undefined || valor === null) return 'R$ 0,00';
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  });
};

/**
 * Formata data: DD/MM/AAAA
 */
export const formatarData = (valor?: string | Date | null): string => {
  if (!valor) return '';
  const data = typeof valor === 'string' ? new Date(valor) : valor;
  return data.toLocaleDateString('pt-BR');
};

/**
 * Formata data e hora: DD/MM/AAAA HH:mm
 */
export const formatarDataHora = (valor?: string | Date | null): string => {
  if (!valor) return '';
  const data = typeof valor === 'string' ? new Date(valor) : valor;
  return data.toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

/**
 * Aplica máscara de CPF enquanto digita
 */
export const mascaraCPF = (e: React.ChangeEvent<HTMLInputElement>): string => {
  let valor = e.target.value.replace(/\D/g, '');
  valor = valor.slice(0, 11);
  return formatarCPF(valor);
};

/**
 * Aplica máscara de CNPJ enquanto digita
 */
export const mascaraCNPJ = (e: React.ChangeEvent<HTMLInputElement>): string => {
  let valor = e.target.value.replace(/\D/g, '');
  valor = valor.slice(0, 14);
  return formatarCNPJ(valor);
};

/**
 * Aplica máscara de telefone enquanto digita
 */
export const mascaraTelefone = (e: React.ChangeEvent<HTMLInputElement>): string => {
  let valor = e.target.value.replace(/\D/g, '');
  valor = valor.slice(0, 11);
  return formatarTelefone(valor);
};

/**
 * Aplica máscara de CEP enquanto digita
 */
export const mascaraCEP = (e: React.ChangeEvent<HTMLInputElement>): string => {
  let valor = e.target.value.replace(/\D/g, '');
  valor = valor.slice(0, 8);
  return formatarCEP(valor);
};

/**
 * Aplica máscara de placa enquanto digita
 */
export const mascaraPlaca = (e: React.ChangeEvent<HTMLInputElement>): string => {
  let valor = e.target.value.toUpperCase().replace(/[^A-Z0-9]/g, '');
  valor = valor.slice(0, 7);
  return formatarPlaca(valor);
};
