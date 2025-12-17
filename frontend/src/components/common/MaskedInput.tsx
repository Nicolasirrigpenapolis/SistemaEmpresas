import React, { forwardRef, useCallback } from 'react';
import {
  formatarMoedaInput,
  formatarCPF,
  formatarCNPJ,
  formatarDocumento,
  formatarTelefone,
  formatarCEP,
  formatarPlaca,
  formatarQuilometragem,
  formatarPorcentagem,
  formatarRG,
  formatarNumeroInteiro,
  limparNumeros,
} from '../../utils/formatters';

// Tipos de máscara suportados
export type MaskType =
  | 'dinheiro'
  | 'cpf'
  | 'cnpj'
  | 'documento' // Auto-detecta CPF ou CNPJ
  | 'telefone'
  | 'cep'
  | 'placa'
  | 'quilometragem'
  | 'porcentagem'
  | 'rg'
  | 'inteiro';

interface MaskedInputProps extends Omit<React.InputHTMLAttributes<HTMLInputElement>, 'onChange'> {
  mask: MaskType;
  value: string;
  onChange: (value: string) => void;
  prefix?: string; // Ex: "R$ " para dinheiro
  suffix?: string; // Ex: " km" para quilometragem, " %" para porcentagem
  showPrefix?: boolean;
  showSuffix?: boolean;
}

// Mapeamento de máscaras
const maskFunctions: Record<MaskType, (valor: string) => string> = {
  dinheiro: formatarMoedaInput,
  cpf: formatarCPF,
  cnpj: formatarCNPJ,
  documento: formatarDocumento,
  telefone: formatarTelefone,
  cep: formatarCEP,
  placa: formatarPlaca,
  quilometragem: formatarQuilometragem,
  porcentagem: formatarPorcentagem,
  rg: formatarRG,
  inteiro: formatarNumeroInteiro,
};

// Tamanhos máximos (caracteres do valor formatado)
const maxLengths: Partial<Record<MaskType, number>> = {
  cpf: 14, // 000.000.000-00
  cnpj: 18, // 00.000.000/0000-00
  documento: 18,
  telefone: 15, // (00) 00000-0000
  cep: 9, // 00000-000
  placa: 8, // ABC-1D23
  rg: 12, // 00.000.000-0
  dinheiro: 20, // 999.999.999.999,99
  quilometragem: 15,
  porcentagem: 10,
  inteiro: 15,
};

// Prefixos padrão
const defaultPrefixes: Partial<Record<MaskType, string>> = {
  dinheiro: 'R$ ',
};

// Sufixos padrão
const defaultSuffixes: Partial<Record<MaskType, string>> = {
  quilometragem: ' km',
  porcentagem: '%',
};

/**
 * Componente de Input com Máscara
 * 
 * IMPORTANTE: O valor retornado pelo onChange já está formatado (com máscara).
 * Use as funções parse* do formatters.ts para enviar ao backend.
 * 
 * Exemplo de uso:
 * ```tsx
 * <MaskedInput
 *   mask="dinheiro"
 *   value={valor}
 *   onChange={setValor}
 *   showPrefix
 * />
 * 
 * // Ao enviar ao backend:
 * const valorNumerico = parseDinheiro(valor); // 1234.56
 * ```
 */
const MaskedInput = forwardRef<HTMLInputElement, MaskedInputProps>(
  (
    {
      mask,
      value,
      onChange,
      prefix,
      suffix,
      showPrefix = false,
      showSuffix = false,
      className = '',
      placeholder,
      ...rest
    },
    ref
  ) => {
    const maskFn = maskFunctions[mask];
    const maxLength = maxLengths[mask];
    const actualPrefix = prefix ?? defaultPrefixes[mask] ?? '';
    const actualSuffix = suffix ?? defaultSuffixes[mask] ?? '';

    // Aplica a máscara ao digitar
    const handleChange = useCallback(
      (e: React.ChangeEvent<HTMLInputElement>) => {
        let inputValue = e.target.value;

        // Remove prefixo e sufixo se existirem
        if (actualPrefix && inputValue.startsWith(actualPrefix)) {
          inputValue = inputValue.slice(actualPrefix.length);
        }
        if (actualSuffix && inputValue.endsWith(actualSuffix)) {
          inputValue = inputValue.slice(0, -actualSuffix.length);
        }

        // Para placa, permite letras e números
        if (mask === 'placa') {
          inputValue = inputValue.toUpperCase().replace(/[^A-Z0-9]/g, '').slice(0, 7);
        } else if (mask !== 'dinheiro' && mask !== 'porcentagem') {
          // Para outros tipos numéricos, remove não-números
          inputValue = limparNumeros(inputValue);
        }

        // Aplica a máscara
        let maskedValue = maskFn(inputValue);

        // Limita tamanho
        if (maxLength && maskedValue.length > maxLength) {
          maskedValue = maskedValue.slice(0, maxLength);
        }

        onChange(maskedValue);
      },
      [mask, maskFn, maxLength, onChange, actualPrefix, actualSuffix]
    );

    // Valor exibido (com prefixo/sufixo)
    const displayValue = value
      ? `${showPrefix && actualPrefix && value ? actualPrefix : ''}${value}${showSuffix && actualSuffix && value ? actualSuffix : ''}`
      : '';

    // Placeholder padrão
    const getPlaceholder = () => {
      if (placeholder) return placeholder;
      switch (mask) {
        case 'dinheiro':
          return showPrefix ? 'R$ 0,00' : '0,00';
        case 'cpf':
          return '000.000.000-00';
        case 'cnpj':
          return '00.000.000/0000-00';
        case 'documento':
          return 'CPF ou CNPJ';
        case 'telefone':
          return '(00) 00000-0000';
        case 'cep':
          return '00000-000';
        case 'placa':
          return 'ABC-1234';
        case 'quilometragem':
          return showSuffix ? '0 km' : '0';
        case 'porcentagem':
          return showSuffix ? '0,00%' : '0,00';
        case 'rg':
          return '00.000.000-0';
        case 'inteiro':
          return '0';
        default:
          return '';
      }
    };

    return (
      <input
        ref={ref}
        type="text"
        inputMode={mask === 'placa' ? 'text' : 'numeric'}
        value={displayValue}
        onChange={handleChange}
        placeholder={getPlaceholder()}
        maxLength={maxLength ? maxLength + (showPrefix ? actualPrefix.length : 0) + (showSuffix ? actualSuffix.length : 0) : undefined}
        className={className}
        {...rest}
      />
    );
  }
);

MaskedInput.displayName = 'MaskedInput';

export default MaskedInput;
