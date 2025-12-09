# 🛠️ Guia de Implementação - Migração NewSistema

Este documento complementa o [PLANO_MIGRACAO_NEWSISTEMA.md](./PLANO_MIGRACAO_NEWSISTEMA.md) com exemplos práticos de código.

---

## 📁 Estrutura de Pastas Sugerida

```
SistemaEmpresas/
├── Controllers/
│   ├── Base/
│   │   └── BaseController.cs          ← NOVO (do NewSistema)
│   ├── AuthController.cs
│   └── ...
├── DTOs/
│   ├── Base/
│   │   ├── IListDto.cs                ← NOVO
│   │   ├── IDetailDto.cs              ← NOVO
│   │   ├── ICreateDto.cs              ← NOVO
│   │   └── IUpdateDto.cs              ← NOVO
│   ├── Usuario/
│   │   ├── UsuarioListDto.cs          ← NOVO
│   │   ├── UsuarioDetailDto.cs        ← NOVO
│   │   ├── UsuarioCreateDto.cs        ← NOVO
│   │   └── UsuarioUpdateDto.cs        ← NOVO
│   └── ...
├── Repositories/
│   ├── Interfaces/
│   │   └── IGenericRepository.cs      ← NOVO (do NewSistema)
│   ├── GenericRepository.cs           ← NOVO (do NewSistema)
│   └── ...
├── Models/
│   ├── Base/
│   │   ├── ISoftDelete.cs             ← NOVO
│   │   └── IAuditable.cs              ← NOVO
│   └── ...
└── Services/
    ├── CacheService.cs                ← MELHORAR (do NewSistema)
    └── ...
```

---

## 1️⃣ GenericRepository Pattern

### IGenericRepository.cs (NOVO)

```csharp
using System.Linq.Expressions;

namespace SistemaEmpresas.Repositories.Interfaces
{
    /// <summary>
    /// Interface genérica para operações de repositório.
    /// Baseado no padrão do NewSistema.
    /// </summary>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Consultas
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        
        // Paginação
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int page, 
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        
        // Manipulação
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // SaveChanges
        Task<int> SaveChangesAsync();
    }
}
```

### GenericRepository.cs (NOVO)

```csharp
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SistemaEmpresas.Repositories
{
    /// <summary>
    /// Implementação genérica de repositório.
    /// Baseado no padrão do NewSistema.
    /// </summary>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
```

---

## 2️⃣ Padrão de DTOs

### Interfaces Base

```csharp
namespace SistemaEmpresas.DTOs.Base
{
    /// <summary>
    /// Interface para DTOs de listagem (menos campos, mais rápido)
    /// </summary>
    public interface IListDto
    {
        int Id { get; set; }
    }

    /// <summary>
    /// Interface para DTOs detalhados (todos os campos + relacionamentos)
    /// </summary>
    public interface IDetailDto : IListDto
    {
    }

    /// <summary>
    /// Interface para DTOs de criação (sem Id, sem campos calculados)
    /// </summary>
    public interface ICreateDto
    {
    }

    /// <summary>
    /// Interface para DTOs de atualização (com Id, sem campos calculados)
    /// </summary>
    public interface IUpdateDto
    {
        int Id { get; set; }
    }
}
```

### Exemplo: DTOs de GrupoUsuario

```csharp
using SistemaEmpresas.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.DTOs.GrupoUsuario
{
    /// <summary>
    /// DTO para listagem de grupos (mínimo de dados)
    /// </summary>
    public class GrupoUsuarioListDto : IListDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public bool Ativo { get; set; }
        public bool GrupoSistema { get; set; }
        public int TotalUsuarios { get; set; } // Calculado
    }

    /// <summary>
    /// DTO para detalhes completos do grupo
    /// </summary>
    public class GrupoUsuarioDetailDto : IDetailDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool GrupoSistema { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        
        // Relacionamentos
        public List<UsuarioSimpleDto> Usuarios { get; set; } = new();
        public List<PermissaoDto> Permissoes { get; set; } = new();
    }

    /// <summary>
    /// DTO para criação de novo grupo
    /// </summary>
    public class GrupoUsuarioCreateDto : ICreateDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;
    }

    /// <summary>
    /// DTO para atualização de grupo existente
    /// </summary>
    public class GrupoUsuarioUpdateDto : IUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; }
    }

    /// <summary>
    /// DTO simplificado para uso em relacionamentos
    /// </summary>
    public class UsuarioSimpleDto
    {
        public string Nome { get; set; } = null!;
        public bool Ativo { get; set; }
    }

    public class PermissaoDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nome { get; set; } = null!;
    }
}
```

---

## 3️⃣ BaseController

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.DTOs.Base;
using SistemaEmpresas.Services;

namespace SistemaEmpresas.Controllers.Base
{
    /// <summary>
    /// Controller base genérico para operações CRUD.
    /// Baseado no padrão do NewSistema.
    /// </summary>
    [ApiController]
    public abstract class BaseController<TEntity, TListDto, TDetailDto, TCreateDto, TUpdateDto> : ControllerBase
        where TEntity : class
        where TListDto : class, IListDto
        where TDetailDto : class, IDetailDto
        where TCreateDto : class, ICreateDto
        where TUpdateDto : class, IUpdateDto
    {
        protected readonly DbContext _context;
        protected readonly ILogger _logger;
        protected readonly ICacheService? _cacheService;

        protected BaseController(
            DbContext context,
            ILogger logger,
            ICacheService? cacheService = null)
        {
            _context = context;
            _logger = logger;
            _cacheService = cacheService;
        }

        // Métodos abstratos para conversão (cada controller implementa)
        protected abstract DbSet<TEntity> GetDbSet();
        protected abstract TListDto EntityToListDto(TEntity entity);
        protected abstract TDetailDto EntityToDetailDto(TEntity entity);
        protected abstract TEntity CreateDtoToEntity(TCreateDto dto);
        protected abstract void UpdateEntityFromDto(TEntity entity, TUpdateDto dto);

        /// <summary>
        /// GET: api/[controller]
        /// Lista todos os registros (paginado)
        /// </summary>
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TListDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? search = null)
        {
            try
            {
                var query = GetDbSet().AsQueryable();

                // Aplicar busca se fornecida (cada controller pode override)
                query = ApplySearch(query, search);

                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = items.Select(EntityToListDto).ToList();

                Response.Headers.Add("X-Total-Count", totalCount.ToString());
                Response.Headers.Add("X-Page", page.ToString());
                Response.Headers.Add("X-Page-Size", pageSize.ToString());

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar registros");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// GET: api/[controller]/{id}
        /// Obtém um registro específico
        /// </summary>
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDetailDto>> GetById(int id)
        {
            try
            {
                var entity = await GetDbSet().FindAsync(id);

                if (entity == null)
                {
                    return NotFound($"Registro com ID {id} não encontrado");
                }

                var dto = EntityToDetailDto(entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar registro {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// POST: api/[controller]
        /// Cria um novo registro
        /// </summary>
        [HttpPost]
        public virtual async Task<ActionResult<TDetailDto>> Create([FromBody] TCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = CreateDtoToEntity(dto);
                GetDbSet().Add(entity);
                await _context.SaveChangesAsync();

                var detailDto = EntityToDetailDto(entity);
                
                // Invalida cache se habilitado
                InvalidateCache();

                return CreatedAtAction(nameof(GetById), new { id = detailDto.Id }, detailDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar registro");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// PUT: api/[controller]/{id}
        /// Atualiza um registro existente
        /// </summary>
        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TDetailDto>> Update(int id, [FromBody] TUpdateDto dto)
        {
            try
            {
                if (dto.Id != id)
                {
                    return BadRequest("ID da URL não corresponde ao ID do body");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = await GetDbSet().FindAsync(id);
                if (entity == null)
                {
                    return NotFound($"Registro com ID {id} não encontrado");
                }

                UpdateEntityFromDto(entity, dto);
                await _context.SaveChangesAsync();

                var detailDto = EntityToDetailDto(entity);
                
                // Invalida cache
                InvalidateCache();

                return Ok(detailDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("O registro foi modificado por outro usuário");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar registro {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// DELETE: api/[controller]/{id}
        /// Remove um registro (ou soft delete se suportado)
        /// </summary>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await GetDbSet().FindAsync(id);
                if (entity == null)
                {
                    return NotFound($"Registro com ID {id} não encontrado");
                }

                // Se suporta soft delete, usar aquele método
                if (entity is ISoftDelete softDeleteEntity)
                {
                    softDeleteEntity.DataExclusao = DateTime.Now;
                    softDeleteEntity.UsuarioExclusao = User.Identity?.Name ?? "Sistema";
                    // MotivoExclusao pode vir do body se necessário
                }
                else
                {
                    GetDbSet().Remove(entity);
                }

                await _context.SaveChangesAsync();
                
                // Invalida cache
                InvalidateCache();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar registro {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        // Métodos auxiliares
        protected virtual IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, string? search)
        {
            // Override em cada controller para implementar busca específica
            return query;
        }

        protected virtual void InvalidateCache()
        {
            // Override em cada controller se precisar invalidação específica
            _cacheService?.Remove($"{typeof(TEntity).Name}:all");
        }
    }
}
```

---

## 4️⃣ Soft Delete

### Interface ISoftDelete

```csharp
namespace SistemaEmpresas.Models.Base
{
    /// <summary>
    /// Interface para entidades que suportam soft delete
    /// </summary>
    public interface ISoftDelete
    {
        DateTime? DataExclusao { get; set; }
        string? UsuarioExclusao { get; set; }
        string? MotivoExclusao { get; set; }
        
        bool IsDeleted => DataExclusao.HasValue;
    }
}
```

### Exemplo de uso em Model

```csharp
using SistemaEmpresas.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.Models
{
    public class GrupoUsuarioNovo : ISoftDelete
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = null!;
        
        [StringLength(500)]
        public string? Descricao { get; set; }
        
        public bool Ativo { get; set; } = true;
        public bool GrupoSistema { get; set; } = false;
        
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }
        
        // Soft Delete (ISoftDelete)
        public DateTime? DataExclusao { get; set; }
        
        [StringLength(200)]
        public string? UsuarioExclusao { get; set; }
        
        [StringLength(500)]
        public string? MotivoExclusao { get; set; }
    }
}
```

### Migration para adicionar Soft Delete

```csharp
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddSoftDeleteToGrupoUsuario : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "DataExclusao",
            table: "GrupoUsuario",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "UsuarioExclusao",
            table: "GrupoUsuario",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "MotivoExclusao",
            table: "GrupoUsuario",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "DataExclusao", table: "GrupoUsuario");
        migrationBuilder.DropColumn(name: "UsuarioExclusao", table: "GrupoUsuario");
        migrationBuilder.DropColumn(name: "MotivoExclusao", table: "GrupoUsuario");
    }
}
```

---

## 5️⃣ CacheService Melhorado

```csharp
using Microsoft.Extensions.Caching.Memory;

namespace SistemaEmpresas.Services
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        void Remove(string key);
        void RemoveByPrefix(string prefix);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;
        private readonly HashSet<string> _keys = new();
        private readonly object _lock = new();

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public T? Get<T>(string key)
        {
            try
            {
                return _cache.Get<T>(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro ao recuperar cache para chave {Key}", key);
                return default;
            }
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
                };

                _cache.Set(key, value, options);

                lock (_lock)
                {
                    _keys.Add(key);
                }

                _logger.LogDebug("Cache definido para chave {Key} com expiração {Expiration}", 
                    key, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro ao definir cache para chave {Key}", key);
            }
        }

        public void Remove(string key)
        {
            try
            {
                _cache.Remove(key);
                
                lock (_lock)
                {
                    _keys.Remove(key);
                }

                _logger.LogDebug("Cache removido para chave {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro ao remover cache para chave {Key}", key);
            }
        }

        public void RemoveByPrefix(string prefix)
        {
            try
            {
                List<string> keysToRemove;
                
                lock (_lock)
                {
                    keysToRemove = _keys.Where(k => k.StartsWith(prefix)).ToList();
                }

                foreach (var key in keysToRemove)
                {
                    Remove(key);
                }

                _logger.LogDebug("Removidos {Count} itens do cache com prefixo {Prefix}", 
                    keysToRemove.Count, prefix);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro ao remover cache por prefixo {Prefix}", prefix);
            }
        }
    }
}
```

### Registro no Program.cs

```csharp
// Adicionar ao Program.cs
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();
```

---

## 6️⃣ Exemplo Completo: GrupoUsuarioController

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Controllers.Base;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs.GrupoUsuario;
using SistemaEmpresas.Models;
using SistemaEmpresas.Services;

namespace SistemaEmpresas.Controllers
{
    [Route("api/[controller]")]
    public class GruposUsuarioController : BaseController<
        GrupoUsuario,
        GrupoUsuarioListDto,
        GrupoUsuarioDetailDto,
        GrupoUsuarioCreateDto,
        GrupoUsuarioUpdateDto>
    {
        public GruposUsuarioController(
            ApplicationDbContext context,
            ILogger<GruposUsuarioController> logger,
            ICacheService? cacheService = null)
            : base(context, logger, cacheService)
        {
        }

        protected override DbSet<GrupoUsuario> GetDbSet()
        {
            return ((ApplicationDbContext)_context).GrupoUsuarios;
        }

        protected override GrupoUsuarioListDto EntityToListDto(GrupoUsuario entity)
        {
            return new GrupoUsuarioListDto
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Ativo = entity.Ativo,
                GrupoSistema = entity.GrupoSistema,
                TotalUsuarios = entity.Usuarios?.Count ?? 0
            };
        }

        protected override GrupoUsuarioDetailDto EntityToDetailDto(GrupoUsuario entity)
        {
            return new GrupoUsuarioDetailDto
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Descricao = entity.Descricao,
                Ativo = entity.Ativo,
                GrupoSistema = entity.GrupoSistema,
                DataCriacao = entity.DataCriacao,
                DataAtualizacao = entity.DataAtualizacao,
                Usuarios = entity.Usuarios?.Select(u => new UsuarioSimpleDto
                {
                    Nome = u.PwNome,
                    Ativo = u.PwAtivo
                }).ToList() ?? new List<UsuarioSimpleDto>()
            };
        }

        protected override GrupoUsuario CreateDtoToEntity(GrupoUsuarioCreateDto dto)
        {
            return new GrupoUsuario
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Ativo = dto.Ativo,
                DataCriacao = DateTime.Now
            };
        }

        protected override void UpdateEntityFromDto(GrupoUsuario entity, GrupoUsuarioUpdateDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Descricao = dto.Descricao;
            entity.Ativo = dto.Ativo;
            entity.DataAtualizacao = DateTime.Now;
        }

        protected override IQueryable<GrupoUsuario> ApplySearch(IQueryable<GrupoUsuario> query, string? search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(g => 
                    g.Nome.Contains(search) || 
                    (g.Descricao != null && g.Descricao.Contains(search)));
            }
            return query;
        }
    }
}
```

---

## 🎯 Checklist de Implementação

### Fase 1: Fundação

- [ ] Criar pasta `Repositories/` e `Repositories/Interfaces/`
- [ ] Implementar `IGenericRepository<T>`
- [ ] Implementar `GenericRepository<T>`
- [ ] Registrar no DI (Program.cs)
- [ ] Criar pasta `DTOs/Base/`
- [ ] Implementar interfaces de DTOs
- [ ] Criar pasta `Controllers/Base/`
- [ ] Implementar `BaseController<...>`
- [ ] Criar pasta `Models/Base/`
- [ ] Implementar `ISoftDelete`
- [ ] Melhorar/Substituir `CacheService`
- [ ] Testar com 1-2 controllers piloto

### Fase 2: Aplicar em Controllers Existentes

- [ ] Criar DTOs para GrupoUsuario
- [ ] Refatorar GrupoUsuarioController para usar BaseController
- [ ] Criar DTOs para Emitente
- [ ] Refatorar EmitentesController
- [ ] Aplicar padrão em outros controllers gradualmente

### Fase 3: Soft Delete

- [ ] Adicionar migration para tabelas principais
- [ ] Implementar ISoftDelete nos models
- [ ] Testar exclusões
- [ ] Criar endpoint para restaurar (opcional)

---

## 📚 Referências

- [PLANO_MIGRACAO_NEWSISTEMA.md](./PLANO_MIGRACAO_NEWSISTEMA.md) - Plano estratégico
- Código fonte NewSistema: `c:\Projetos\SistemaEmpresas\NewSistema\`

---

**Última atualização:** 09/12/2024
