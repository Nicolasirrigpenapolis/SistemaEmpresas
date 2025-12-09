using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SistemaEmpresas.Data;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TenantService> _logger;

        // Chave de cache para todos os tenants
        private const string CACHE_KEY = "TenantsList";

        public TenantService(
            TenantDbContext context,
            IMemoryCache cache,
            IConfiguration configuration,
            ILogger<TenantService> logger)
        {
            _context = context;
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }

        public Tenant? GetTenantAtual(HttpContext httpContext)
        {
            try
            {
                // Tenta obter o domínio do header X-Tenant, senão usa o Host
                var dominio = httpContext.Request.Headers["X-Tenant"].FirstOrDefault()
                    ?? httpContext.Request.Host.Host;

                _logger.LogInformation("Buscando tenant para domínio: {Dominio}", dominio);

                // Verifica se o cache está habilitado
                var cacheHabilitado = _configuration.GetValue<bool>("TenantCache:HabilitarCache", true);

                if (cacheHabilitado)
                {
                    return GetTenantComCache(dominio);
                }
                else
                {
                    return GetTenantSemCache(dominio);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tenant");
                return null;
            }
        }

        private Tenant? GetTenantComCache(string dominio)
        {
            // Tenta obter a lista de tenants do cache
            if (!_cache.TryGetValue(CACHE_KEY, out List<Tenant>? tenants))
            {
                _logger.LogInformation("Cache de tenants vazio. Carregando do banco...");

                // Se não está no cache, busca do banco
                tenants = _context.Tenants
                    .AsNoTracking()
                    .Where(t => t.Ativo)
                    .ToList();

                // Define opções de cache
                var expiracaoMinutos = _configuration.GetValue<int>("TenantCache:ExpiracaoMinutos", 30);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiracaoMinutos),
                    Priority = CacheItemPriority.High
                };

                // Armazena no cache
                _cache.Set(CACHE_KEY, tenants, cacheOptions);

                _logger.LogInformation("Cache de tenants atualizado. {Count} tenants carregados.", tenants?.Count ?? 0);
            }

            // Busca o tenant específico na lista em cache
            var tenant = tenants?.FirstOrDefault(t => 
                t.Dominio.Equals(dominio, StringComparison.OrdinalIgnoreCase));

            if (tenant != null)
            {
                _logger.LogInformation("Tenant encontrado: {Nome} (ID: {Id})", tenant.Nome, tenant.Id);
            }
            else
            {
                _logger.LogWarning("Nenhum tenant encontrado para o domínio: {Dominio}", dominio);
            }

            return tenant;
        }

        private Tenant? GetTenantSemCache(string dominio)
        {
            _logger.LogInformation("Buscando tenant diretamente do banco...");

            var tenant = _context.Tenants
                .AsNoTracking()
                .FirstOrDefault(t => t.Dominio == dominio && t.Ativo);

            if (tenant != null)
            {
                _logger.LogInformation("Tenant encontrado: {Nome} (ID: {Id})", tenant.Nome, tenant.Id);
            }
            else
            {
                _logger.LogWarning("Nenhum tenant encontrado para o domínio: {Dominio}", dominio);
            }

            return tenant;
        }

        public void LimparCache()
        {
            _cache.Remove(CACHE_KEY);
            _logger.LogInformation("Cache de tenants limpo com sucesso.");
        }
    }
}
