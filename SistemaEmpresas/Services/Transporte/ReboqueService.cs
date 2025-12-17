using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services.Transporte;

// ==========================================
// INTERFACE DO SERVICE DE REBOQUES
// ==========================================
public interface IReboqueService
{
    Task<DTOs.PagedResult<ReboqueListDto>> ListarAsync(ReboqueFiltros? filtros = null);
    Task<ReboqueDto?> ObterPorIdAsync(int id);
    Task<ReboqueDto?> ObterPorPlacaAsync(string placa);
    Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ReboqueCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ReboqueCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id);
    Task<bool> PlacaExisteAsync(string placa, int? idIgnorar = null);
}

// ==========================================
// IMPLEMENTAÇÃO DO SERVICE DE REBOQUES
// ==========================================
public class ReboqueService : IReboqueService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReboqueService> _logger;

    public ReboqueService(AppDbContext context, ILogger<ReboqueService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DTOs.PagedResult<ReboqueListDto>> ListarAsync(ReboqueFiltros? filtros = null)
    {
        var query = _context.Reboques.AsQueryable();
        
        // Filtros
        if (filtros != null)
        {
            // Filtro de ativos/inativos
            if (!filtros.IncluirInativos)
            {
                query = query.Where(r => r.Ativo);
            }
            
            if (!string.IsNullOrWhiteSpace(filtros.Busca))
            {
                var busca = filtros.Busca.ToUpper().Trim();
                query = query.Where(r => 
                    r.Placa.Contains(busca) || 
                    (r.Marca != null && r.Marca.ToUpper().Contains(busca)) ||
                    (r.Modelo != null && r.Modelo.ToUpper().Contains(busca)));
            }
            
            if (!string.IsNullOrWhiteSpace(filtros.Placa))
            {
                query = query.Where(r => r.Placa.Contains(filtros.Placa.ToUpper().Trim()));
            }
        }

        // Paginação
        var pageNumber = filtros?.Pagina ?? 1;
        var pageSize = filtros?.TamanhoPagina ?? 25;
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.Placa)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReboqueListDto
            {
                Id = r.Id,
                Placa = r.Placa,
                Marca = r.Marca,
                Modelo = r.Modelo,
                Tara = r.Tara,
                TipoCarroceria = r.TipoCarroceria,
                Uf = r.Uf,
                Ativo = r.Ativo
            })
            .ToListAsync();
            
        return new DTOs.PagedResult<ReboqueListDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<ReboqueDto?> ObterPorIdAsync(int id)
    {
        var reboque = await _context.Reboques.FindAsync(id);
        if (reboque == null) return null;

        return MapToDto(reboque);
    }

    public async Task<ReboqueDto?> ObterPorPlacaAsync(string placa)
    {
        var reboque = await _context.Reboques
            .FirstOrDefaultAsync(r => r.Placa == placa.ToUpper().Trim());
        
        if (reboque == null) return null;
        return MapToDto(reboque);
    }

    public async Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ReboqueCreateUpdateDto dto)
    {
        try
        {
            var placaNormalizada = dto.Placa.ToUpper().Trim();
            
            if (await PlacaExisteAsync(placaNormalizada))
                return (false, "Já existe um reboque com esta placa", null);

            var reboque = new Reboque
            {
                Placa = placaNormalizada,
                Marca = dto.Marca?.Trim(),
                Modelo = dto.Modelo?.Trim(),
                AnoFabricacao = dto.AnoFabricacao,
                Tara = dto.Tara,
                CapacidadeKg = dto.CapacidadeKg,
                TipoCarroceria = dto.TipoCarroceria.Trim(),
                Uf = dto.Uf.ToUpper().Trim(),
                Renavam = dto.Renavam?.Trim(),
                Chassi = dto.Chassi?.Trim(),
                Rntrc = dto.Rntrc?.Trim(),
                Ativo = dto.Ativo,
                Observacoes = dto.Observacoes?.Trim(),
                DataCriacao = DateTime.Now
            };

            _context.Reboques.Add(reboque);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Reboque criado: {Placa} (ID: {Id})", reboque.Placa, reboque.Id);
            return (true, "Reboque criado com sucesso", reboque.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar reboque: {Placa}", dto.Placa);
            return (false, "Erro ao criar reboque: " + ex.Message, null);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ReboqueCreateUpdateDto dto)
    {
        try
        {
            var reboque = await _context.Reboques.FindAsync(id);
            if (reboque == null)
                return (false, "Reboque não encontrado");

            var placaNormalizada = dto.Placa.ToUpper().Trim();
            
            if (await PlacaExisteAsync(placaNormalizada, id))
                return (false, "Já existe outro reboque com esta placa");

            reboque.Placa = placaNormalizada;
            reboque.Marca = dto.Marca?.Trim();
            reboque.Modelo = dto.Modelo?.Trim();
            reboque.AnoFabricacao = dto.AnoFabricacao;
            reboque.Tara = dto.Tara;
            reboque.CapacidadeKg = dto.CapacidadeKg;
            reboque.TipoCarroceria = dto.TipoCarroceria.Trim();
            reboque.Uf = dto.Uf.ToUpper().Trim();
            reboque.Renavam = dto.Renavam?.Trim();
            reboque.Chassi = dto.Chassi?.Trim();
            reboque.Rntrc = dto.Rntrc?.Trim();
            reboque.Ativo = dto.Ativo;
            reboque.Observacoes = dto.Observacoes?.Trim();
            reboque.DataUltimaAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Reboque atualizado: {Placa} (ID: {Id})", reboque.Placa, reboque.Id);
            return (true, "Reboque atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar reboque ID: {Id}", id);
            return (false, "Erro ao atualizar reboque: " + ex.Message);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id)
    {
        try
        {
            var reboque = await _context.Reboques
                .Include(r => r.Viagens)
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (reboque == null)
                return (false, "Reboque não encontrado");

            if (reboque.Viagens.Any())
                return (false, $"Não é possível excluir. Existem {reboque.Viagens.Count} viagem(ns) vinculada(s)");

            _context.Reboques.Remove(reboque);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Reboque excluído: {Placa} (ID: {Id})", reboque.Placa, id);
            return (true, "Reboque excluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir reboque ID: {Id}", id);
            return (false, "Erro ao excluir reboque: " + ex.Message);
        }
    }

    public async Task<bool> PlacaExisteAsync(string placa, int? idIgnorar = null)
    {
        var query = _context.Reboques.Where(r => r.Placa == placa.ToUpper().Trim());
        
        if (idIgnorar.HasValue)
            query = query.Where(r => r.Id != idIgnorar.Value);

        return await query.AnyAsync();
    }

    private static ReboqueDto MapToDto(Reboque r)
    {
        return new ReboqueDto
        {
            Id = r.Id,
            Placa = r.Placa,
            Marca = r.Marca,
            Modelo = r.Modelo,
            AnoFabricacao = r.AnoFabricacao,
            Tara = r.Tara,
            CapacidadeKg = r.CapacidadeKg,
            TipoCarroceria = r.TipoCarroceria,
            Uf = r.Uf,
            Renavam = r.Renavam,
            Chassi = r.Chassi,
            Rntrc = r.Rntrc,
            Ativo = r.Ativo,
            Observacoes = r.Observacoes,
            DataCriacao = r.DataCriacao,
            DataUltimaAlteracao = r.DataUltimaAlteracao
        };
    }
}
