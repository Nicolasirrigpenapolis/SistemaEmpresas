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

// ==========================================
// MÁSCARAS DE DINHEIRO (INPUT)
// ==========================================

/**
 * Formata valor monetário durante digitação: R$ 1.234,56
 * Aceita string ou number como entrada
 */
export const formatarMoedaInput = (valor?: string | number | null): string => {
  if (valor === undefined || valor === null || valor === '') return '';
  
  // Se for número, converte para centavos
  if (typeof valor === 'number') {
    const centavos = Math.round(valor * 100);
    return formatarMoedaInput(centavos.toString());
  }
  
  // Remove tudo que não é número
  const numeros = valor.replace(/\D/g, '');
  
  if (numeros === '' || numeros === '0' || numeros === '00') return '';
  
  // Converte para número e divide por 100 (centavos)
  const valorNumerico = parseInt(numeros, 10) / 100;
  
  // Formata com separadores brasileiros
  return valorNumerico.toLocaleString('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
};

/**
 * Aplica máscara de dinheiro enquanto digita
 */
export const mascaraDinheiro = (e: React.ChangeEvent<HTMLInputElement>): string => {
  return formatarMoedaInput(e.target.value);
};

/**
 * Formata valor com prefixo R$
 */
export const formatarDinheiroComPrefixo = (valor?: string | number | null): string => {
  const formatado = formatarMoedaInput(valor);
  if (!formatado) return '';
  return `R$ ${formatado}`;
};

// ==========================================
// PARSERS (REMOVER MÁSCARAS PARA ENVIAR AO BACKEND)
// ==========================================

/**
 * Remove máscara de CPF/CNPJ e retorna apenas números
 */
export const parseCPFCNPJ = (valor?: string | null): string => {
  if (!valor) return '';
  return valor.replace(/\D/g, '');
};

/**
 * Remove máscara de telefone e retorna apenas números
 */
export const parseTelefone = (valor?: string | null): string => {
  if (!valor) return '';
  return valor.replace(/\D/g, '');
};

/**
 * Remove máscara de CEP e retorna apenas números
 */
export const parseCEP = (valor?: string | null): string => {
  if (!valor) return '';
  return valor.replace(/\D/g, '');
};

/**
 * Remove máscara de placa e retorna string limpa (sem hífen)
 */
export const parsePlaca = (valor?: string | null): string => {
  if (!valor) return '';
  return valor.replace(/[^A-Za-z0-9]/g, '').toUpperCase();
};

/**
 * Remove máscara de dinheiro e retorna número
 * "1.234,56" -> 1234.56
 */
export const parseDinheiro = (valor?: string | null): number => {
  if (!valor) return 0;
  // Remove R$, pontos de milhar e troca vírgula por ponto
  const limpo = valor
    .replace(/R\$\s?/g, '')
    .replace(/\./g, '')
    .replace(',', '.');
  const numero = parseFloat(limpo);
  return isNaN(numero) ? 0 : numero;
};

/**
 * Remove máscara de dinheiro e retorna número ou null (para campos opcionais)
 */
export const parseDinheiroOuNull = (valor?: string | null): number | null => {
  if (!valor || valor.trim() === '') return null;
  const numero = parseDinheiro(valor);
  return numero === 0 ? null : numero;
};

// ==========================================
// MÁSCARAS DE NÚMERO INTEIRO
// ==========================================

/**
 * Formata número com separador de milhar: 1.234.567
 */
export const formatarNumeroInteiro = (valor?: string | number | null): string => {
  if (valor === undefined || valor === null || valor === '') return '';
  const numeros = String(valor).replace(/\D/g, '');
  if (numeros === '') return '';
  return parseInt(numeros, 10).toLocaleString('pt-BR');
};

/**
 * Aplica máscara de número inteiro enquanto digita
 */
export const mascaraNumeroInteiro = (e: React.ChangeEvent<HTMLInputElement>): string => {
  return formatarNumeroInteiro(e.target.value);
};

/**
 * Remove formatação de número inteiro
 */
export const parseNumeroInteiro = (valor?: string | null): number => {
  if (!valor) return 0;
  const limpo = valor.replace(/\D/g, '');
  return parseInt(limpo, 10) || 0;
};

// ==========================================
// MÁSCARAS DE QUILOMETRAGEM/HODÔMETRO
// ==========================================

/**
 * Formata quilometragem: 123.456 km
 */
export const formatarQuilometragem = (valor?: string | number | null): string => {
  if (valor === undefined || valor === null || valor === '') return '';
  const numeros = String(valor).replace(/\D/g, '');
  if (numeros === '') return '';
  return parseInt(numeros, 10).toLocaleString('pt-BR');
};

/**
 * Aplica máscara de quilometragem enquanto digita
 */
export const mascaraQuilometragem = (e: React.ChangeEvent<HTMLInputElement>): string => {
  return formatarQuilometragem(e.target.value);
};

/**
 * Remove formatação de quilometragem
 */
export const parseQuilometragem = (valor?: string | null): number => {
  if (!valor) return 0;
  const limpo = valor.replace(/\D/g, '');
  return parseInt(limpo, 10) || 0;
};

// ==========================================
// MÁSCARAS DE PORCENTAGEM
// ==========================================

/**
 * Formata porcentagem: 12,50%
 */
export const formatarPorcentagem = (valor?: string | number | null): string => {
  if (valor === undefined || valor === null || valor === '') return '';
  
  if (typeof valor === 'number') {
    return valor.toLocaleString('pt-BR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }
  
  const numeros = valor.replace(/\D/g, '');
  if (numeros === '') return '';
  
  const valorNumerico = parseInt(numeros, 10) / 100;
  return valorNumerico.toLocaleString('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
};

/**
 * Aplica máscara de porcentagem enquanto digita
 */
export const mascaraPorcentagem = (e: React.ChangeEvent<HTMLInputElement>): string => {
  return formatarPorcentagem(e.target.value);
};

/**
 * Remove formatação de porcentagem
 */
export const parsePorcentagem = (valor?: string | null): number => {
  if (!valor) return 0;
  const limpo = valor.replace(/[^\d,]/g, '').replace(',', '.');
  return parseFloat(limpo) || 0;
};

// ==========================================
// MÁSCARA DE RG
// ==========================================

/**
 * Formata RG: 12.345.678-9 (padrão SP)
 */
export const formatarRG = (valor?: string | null): string => {
  if (!valor) return '';
  const numeros = valor.replace(/\D/g, '');
  
  if (numeros.length <= 2) return numeros;
  if (numeros.length <= 5) return `${numeros.slice(0, 2)}.${numeros.slice(2)}`;
  if (numeros.length <= 8) return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5)}`;
  return `${numeros.slice(0, 2)}.${numeros.slice(2, 5)}.${numeros.slice(5, 8)}-${numeros.slice(8, 9)}`;
};

/**
 * Aplica máscara de RG enquanto digita
 */
export const mascaraRG = (e: React.ChangeEvent<HTMLInputElement>): string => {
  let valor = e.target.value.replace(/\D/g, '');
  valor = valor.slice(0, 9);
  return formatarRG(valor);
};

/**
 * Remove máscara de RG
 */
export const parseRG = (valor?: string | null): string => {
  if (!valor) return '';
  return valor.replace(/\D/g, '');
};

// ==========================================
// MÁSCARA DE INSCRIÇÃO ESTADUAL
// ==========================================

/**
 * Formata IE (varia por estado, aqui formato genérico)
 */
export const formatarIE = (valor?: string | null): string => {
  if (!valor) return '';
  // Mantém números e pontos/hífens se já tiver
  return valor.replace(/[^\d.-]/g, '');
};

/**
 * Remove máscara de IE
 */
export const parseIE = (valor?: string | null): string => {
  if (!valor) return '';
  return valor.replace(/\D/g, '');
};

// ==========================================
// UTILITÁRIO PARA CRIAR HANDLER DE INPUT MASCARADO
// ==========================================

type MascaraFn = (e: React.ChangeEvent<HTMLInputElement>) => string;

/**
 * Cria um handler para input mascarado que atualiza o estado
 * @param mascaraFn - Função de máscara a aplicar
 * @param setter - Função setter do estado
 * @param maxLength - Tamanho máximo (opcional)
 */
export const criarHandlerMascara = (
  mascaraFn: MascaraFn,
  setter: (valor: string) => void,
  maxLength?: number
) => {
  return (e: React.ChangeEvent<HTMLInputElement>) => {
    let valor = mascaraFn(e);
    if (maxLength && valor.length > maxLength) {
      valor = valor.slice(0, maxLength);
    }
    setter(valor);
  };
};
