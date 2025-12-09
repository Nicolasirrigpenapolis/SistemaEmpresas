import { useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';

export interface TenantBranding {
  nome: string;
  nomeCompleto: string;
  logo: string;
  favicon: string;
}

// Mapeamento de tenants para suas configurações de branding
const TENANT_BRANDING: Record<string, TenantBranding> = {
  irrigacao: {
    nome: 'Irrigação Penápolis',
    nomeCompleto: 'Irrigação Penápolis Indústria e Comércio',
    logo: '/logos/logo_IP.png',
    favicon: '/logos/logo_IP.png',
  },
  chinellato: {
    nome: 'Chinellato Transportes',
    nomeCompleto: 'Chinellato Transportes Ltda',
    logo: '/logos/logo_chinellato.png',
    favicon: '/logos/logo_chinellato.png',
  },
};

const DEFAULT_BRANDING: TenantBranding = {
  nome: 'Sistema Empresas',
  nomeCompleto: 'Sistema Empresarial',
  logo: '/logos/default.png',
  favicon: '/vite.svg',
};

export function useTenantBranding(): TenantBranding {
  const { tenant, isAuthenticated } = useAuth();

  // Determina o branding baseado no tenant logado
  const branding = isAuthenticated && tenant?.dominio
    ? TENANT_BRANDING[tenant.dominio] || {
        nome: tenant.nomeFantasia || DEFAULT_BRANDING.nome,
        nomeCompleto: tenant.nomeFantasia || DEFAULT_BRANDING.nomeCompleto,
        logo: DEFAULT_BRANDING.logo,
        favicon: DEFAULT_BRANDING.favicon,
      }
    : DEFAULT_BRANDING;

  // Atualiza o título da página
  useEffect(() => {
    document.title = branding.nome;
  }, [branding.nome]);

  // Atualiza o favicon
  useEffect(() => {
    const link = document.querySelector("link[rel*='icon']") as HTMLLinkElement;
    if (link) {
      link.href = branding.favicon;
    }
  }, [branding.favicon]);

  return branding;
}

export function getTenantBranding(dominio?: string): TenantBranding {
  if (!dominio) return DEFAULT_BRANDING;
  return TENANT_BRANDING[dominio] || DEFAULT_BRANDING;
}
