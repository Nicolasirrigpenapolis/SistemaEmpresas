using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.Models;
using SistemaEmpresas.Features.Tenants.Services;

namespace SistemaEmpresas.Features.Tenants.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly TenantDbContext _context;
    private readonly ITenantService _tenantService;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(
        TenantDbContext context,
        ITenantService tenantService,
        ILogger<TenantsController> logger)
    {
        _context = context;
        _tenantService = tenantService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os tenants cadastrados
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
    {
        return await _context.Tenants.ToListAsync();
    }

    /// <summary>
    /// Obtém um tenant específico por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Tenant>> GetTenant(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound();
        }

        return tenant;
    }

    /// <summary>
    /// Obtém o tenant atual da requisição
    /// </summary>
    [HttpGet("atual")]
    public ActionResult<Tenant> GetTenantAtual()
    {
        var tenant = _tenantService.GetTenantAtual(HttpContext);

        if (tenant == null)
        {
            return NotFound(new { message = "Nenhum tenant identificado para esta requisição" });
        }

        return tenant;
    }

    /// <summary>
    /// Cria um novo tenant
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Tenant>> PostTenant(Tenant tenant)
    {
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        // Limpa o cache após adicionar um novo tenant
        _tenantService.LimparCache();
        _logger.LogInformation("Novo tenant criado: {Nome} (ID: {Id})", tenant.Nome, tenant.Id);

        return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);
    }

    /// <summary>
    /// Atualiza um tenant existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTenant(int id, Tenant tenant)
    {
        if (id != tenant.Id)
        {
            return BadRequest();
        }

        _context.Entry(tenant).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            
            // Limpa o cache após atualizar
            _tenantService.LimparCache();
            _logger.LogInformation("Tenant atualizado: {Nome} (ID: {Id})", tenant.Nome, tenant.Id);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TenantExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    /// <summary>
    /// Remove um tenant
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTenant(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }

        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync();

        // Limpa o cache após remover
        _tenantService.LimparCache();
        _logger.LogInformation("Tenant removido: {Nome} (ID: {Id})", tenant.Nome, tenant.Id);

        return NoContent();
    }

    /// <summary>
    /// Limpa o cache de tenants manualmente
    /// </summary>
    [HttpPost("limpar-cache")]
    public IActionResult LimparCache()
    {
        _tenantService.LimparCache();
        return Ok(new { message = "Cache de tenants limpo com sucesso" });
    }

    private bool TenantExists(int id)
    {
        return _context.Tenants.Any(e => e.Id == id);
    }
}
