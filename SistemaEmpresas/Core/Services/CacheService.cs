using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Core.Services;

/// <summary>
/// Serviço centralizado de cache com suporte a multi-tenant
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Obtém item do cache ou executa factory se não existir
    /// </summary>
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    
    /// <summary>
    /// Define item no cache
    /// </summary>
    void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    
    /// <summary>
    /// Obtém item do cache
    /// </summary>
    T? Get<T>(string key);
    
    /// <summary>
    /// Remove item do cache
    /// </summary>
    void Remove(string key);
    
    /// <summary>
    /// Remove todos os itens que começam com o prefixo
    /// </summary>
    void RemoveByPrefix(string prefix);
    
    /// <summary>
    /// Constrói chave de cache com tenant
    /// </summary>
    string BuildKey(params string[] parts);
}

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CacheService> _logger;
    
    // Lista de chaves para permitir remoção por prefixo
    private static readonly HashSet<string> _cacheKeys = new();
    private static readonly object _keysLock = new();

    // Tempos de cache padrão
    public static class CacheDurations
    {
        public static readonly TimeSpan VeryShort = TimeSpan.FromSeconds(30);
        public static readonly TimeSpan Short = TimeSpan.FromMinutes(1);
        public static readonly TimeSpan Medium = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan Long = TimeSpan.FromMinutes(15);
        public static readonly TimeSpan VeryLong = TimeSpan.FromMinutes(30);
        public static readonly TimeSpan OneHour = TimeSpan.FromHours(1);
    }

    public CacheService(
        IMemoryCache cache, 
        IHttpContextAccessor httpContextAccessor,
        ILogger<CacheService> logger)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string BuildKey(params string[] parts)
    {
        var tenantKey = GetTenantKey();
        var allParts = new List<string> { tenantKey };
        allParts.AddRange(parts);
        return string.Join(":", allParts);
    }

    private string GetTenantKey()
    {
        var tenant = _httpContextAccessor.HttpContext?.Items["Tenant"] as Tenant;
        if (tenant != null)
        {
            return !string.IsNullOrWhiteSpace(tenant.Dominio)
                ? tenant.Dominio.ToLowerInvariant()
                : $"tenant_{tenant.Id}";
        }
        return "default";
    }

    public async Task<T?> GetOrCreateAsync<T>(
        string key, 
        Func<Task<T>> factory, 
        TimeSpan? absoluteExpiration = null, 
        TimeSpan? slidingExpiration = null)
    {
        var fullKey = BuildKey(key);
        
        if (_cache.TryGetValue(fullKey, out T? cachedValue))
        {
            _logger.LogDebug("Cache HIT: {Key}", fullKey);
            return cachedValue;
        }

        _logger.LogDebug("Cache MISS: {Key}", fullKey);
        
        var value = await factory();
        
        if (value != null)
        {
            Set(key, value, absoluteExpiration, slidingExpiration);
        }
        
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        var fullKey = BuildKey(key);
        
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? CacheDurations.Medium,
            SlidingExpiration = slidingExpiration ?? CacheDurations.Short
        };

        // Callback para remover da lista de chaves quando expirar
        options.RegisterPostEvictionCallback((key, value, reason, state) =>
        {
            lock (_keysLock)
            {
                _cacheKeys.Remove(key.ToString()!);
            }
        });

        _cache.Set(fullKey, value, options);
        
        lock (_keysLock)
        {
            _cacheKeys.Add(fullKey);
        }
        
        _logger.LogDebug("Cache SET: {Key}", fullKey);
    }

    public T? Get<T>(string key)
    {
        var fullKey = BuildKey(key);
        return _cache.TryGetValue(fullKey, out T? value) ? value : default;
    }

    public void Remove(string key)
    {
        var fullKey = BuildKey(key);
        _cache.Remove(fullKey);
        
        lock (_keysLock)
        {
            _cacheKeys.Remove(fullKey);
        }
        
        _logger.LogDebug("Cache REMOVE: {Key}", fullKey);
    }

    public void RemoveByPrefix(string prefix)
    {
        var fullPrefix = BuildKey(prefix);
        
        List<string> keysToRemove;
        lock (_keysLock)
        {
            keysToRemove = _cacheKeys.Where(k => k.StartsWith(fullPrefix)).ToList();
        }

        foreach (var key in keysToRemove)
        {
            _cache.Remove(key);
            lock (_keysLock)
            {
                _cacheKeys.Remove(key);
            }
        }
        
        _logger.LogDebug("Cache REMOVE BY PREFIX: {Prefix} ({Count} items)", fullPrefix, keysToRemove.Count);
    }
}
