using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services
{
    public interface ITenantService
    {
        /// <summary>
        /// Obtém o tenant atual baseado no contexto HTTP (domínio ou header X-Tenant)
        /// </summary>
        Tenant? GetTenantAtual(HttpContext httpContext);

        /// <summary>
        /// Limpa o cache de tenants (útil após adicionar/editar/remover tenants)
        /// </summary>
        void LimparCache();
    }
}
