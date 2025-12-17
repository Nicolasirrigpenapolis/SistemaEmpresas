using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories.Seguranca;

public interface IGrupoUsuarioRepository
{
    Task<List<GrupoUsuario>> GetAllGruposAsync();
    Task<GrupoUsuario?> GetGrupoByIdAsync(int id);
    Task<GrupoUsuario?> GetGrupoByNomeAsync(string nome);
    Task<bool> GrupoExisteAsync(string nome);
    Task<bool> GrupoExisteAsync(int id);
    Task<GrupoUsuario> CreateGrupoAsync(GrupoUsuario grupo);
    Task<bool> UpdateGrupoAsync(GrupoUsuario grupo);
    Task<bool> DeleteGrupoAsync(int id);
    Task<int> ContarUsuariosDoGrupoAsync(int grupoId);
    Task<bool> MoverUsuariosDoGrupoAsync(int grupoOrigemId, int grupoDestinoId);
    Task<List<PwUsuario>> GetAllUsuariosAsync();
    Task<List<PwUsuario>> GetUsuariosByGrupoIdAsync(int grupoId);
    Task<PwUsuario?> GetUsuarioByNomeAsync(string nome);
    Task<bool> VincularUsuarioAoGrupoAsync(string pwNome, string pwSenha, int grupoId);
    Task<bool> DesvincularUsuarioDoGrupoAsync(string pwNome, string pwSenha);
}

public class GrupoUsuarioRepository : IGrupoUsuarioRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<GrupoUsuarioRepository> _logger;

    public GrupoUsuarioRepository(AppDbContext context, ILogger<GrupoUsuarioRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<GrupoUsuario>> GetAllGruposAsync()
    {
        return await _context.GruposUsuarios.OrderBy(g => g.Nome).ToListAsync();
    }

    public async Task<GrupoUsuario?> GetGrupoByIdAsync(int id)
    {
        return await _context.GruposUsuarios.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<GrupoUsuario?> GetGrupoByNomeAsync(string nome)
    {
        return await _context.GruposUsuarios.FirstOrDefaultAsync(g => g.Nome.ToUpper() == nome.ToUpper());
    }

    public async Task<bool> GrupoExisteAsync(string nome)
    {
        return await _context.GruposUsuarios.AnyAsync(g => g.Nome.ToUpper() == nome.ToUpper());
    }

    public async Task<bool> GrupoExisteAsync(int id)
    {
        return await _context.GruposUsuarios.AnyAsync(g => g.Id == id);
    }

    public async Task<GrupoUsuario> CreateGrupoAsync(GrupoUsuario grupo)
    {
        grupo.DataCriacao = DateTime.Now;
        grupo.Nome = grupo.Nome.ToUpper();
        _context.GruposUsuarios.Add(grupo);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Grupo criado: {Nome} (ID: {Id})", grupo.Nome, grupo.Id);
        return grupo;
    }

    public async Task<bool> UpdateGrupoAsync(GrupoUsuario grupo)
    {
        try
        {
            grupo.DataAtualizacao = DateTime.Now;
            grupo.Nome = grupo.Nome.ToUpper();
            _context.GruposUsuarios.Update(grupo);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Grupo atualizado: {Nome} (ID: {Id})", grupo.Nome, grupo.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar grupo: {Id}", grupo.Id);
            return false;
        }
    }

    public async Task<bool> DeleteGrupoAsync(int id)
    {
        try
        {
            var grupo = await GetGrupoByIdAsync(id);
            if (grupo == null) return false;
            if (grupo.GrupoSistema)
            {
                _logger.LogWarning("Tentativa de excluir grupo de sistema: {Nome}", grupo.Nome);
                return false;
            }
            _context.GruposUsuarios.Remove(grupo);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Grupo excluido: {Nome} (ID: {Id})", grupo.Nome, id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir grupo: {Id}", id);
            return false;
        }
    }

    public async Task<int> ContarUsuariosDoGrupoAsync(int grupoId)
    {
        return await _context.PwUsuarios.CountAsync(u => u.GrupoUsuarioId == grupoId);
    }

    public async Task<bool> MoverUsuariosDoGrupoAsync(int grupoOrigemId, int grupoDestinoId)
    {
        try
        {
            var usuarios = await _context.PwUsuarios.Where(u => u.GrupoUsuarioId == grupoOrigemId).ToListAsync();
            if (!usuarios.Any()) return true;
            foreach (var usuario in usuarios) { usuario.GrupoUsuarioId = grupoDestinoId; }
            await _context.SaveChangesAsync();
            _logger.LogInformation("Movidos {Qtd} usuarios do grupo {Origem} para {Destino}", usuarios.Count, grupoOrigemId, grupoDestinoId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao mover usuarios do grupo {Origem} para {Destino}", grupoOrigemId, grupoDestinoId);
            return false;
        }
    }

    public async Task<List<PwUsuario>> GetAllUsuariosAsync()
    {
        return await _context.PwUsuarios.Include(u => u.GrupoUsuarioNavigation).OrderBy(u => u.PwNome).ToListAsync();
    }

    public async Task<List<PwUsuario>> GetUsuariosByGrupoIdAsync(int grupoId)
    {
        return await _context.PwUsuarios.Include(u => u.GrupoUsuarioNavigation).Where(u => u.GrupoUsuarioId == grupoId).OrderBy(u => u.PwNome).ToListAsync();
    }

    public async Task<PwUsuario?> GetUsuarioByNomeAsync(string nome)
    {
        return await _context.PwUsuarios.Include(u => u.GrupoUsuarioNavigation).FirstOrDefaultAsync(u => u.PwNome == nome);
    }

    public async Task<bool> VincularUsuarioAoGrupoAsync(string pwNome, string pwSenha, int grupoId)
    {
        try
        {
            var usuario = await _context.PwUsuarios.FirstOrDefaultAsync(u => u.PwNome == pwNome && u.PwSenha == pwSenha);
            if (usuario == null)
            {
                _logger.LogWarning("Usuario nao encontrado para vincular: {Nome}", pwNome);
                return false;
            }
            usuario.GrupoUsuarioId = grupoId;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Usuario {Nome} vinculado ao grupo {GrupoId}", pwNome, grupoId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao vincular usuario {Nome} ao grupo {GrupoId}", pwNome, grupoId);
            return false;
        }
    }

    public async Task<bool> DesvincularUsuarioDoGrupoAsync(string pwNome, string pwSenha)
    {
        try
        {
            var usuario = await _context.PwUsuarios.FirstOrDefaultAsync(u => u.PwNome == pwNome && u.PwSenha == pwSenha);
            if (usuario == null)
            {
                _logger.LogWarning("Usuario nao encontrado para desvincular: {Nome}", pwNome);
                return false;
            }
            usuario.GrupoUsuarioId = null;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Usuario {Nome} desvinculado do grupo", pwNome);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desvincular usuario {Nome}", pwNome);
            return false;
        }
    }
}
