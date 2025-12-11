using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models.Transporte;

namespace SistemaEmpresas.Services.Transporte;

// ==========================================
// INTERFACE DO SERVICE DE VEÍCULOS
// ==========================================
public interface IVeiculoService
{
    Task<PagedResult<VeiculoListDto>> ListarAsync(VeiculoFiltros? filtros = null);
    Task<VeiculoDto?> ObterPorIdAsync(int id);
    Task<VeiculoDto?> ObterPorPlacaAsync(string placa);
    Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(VeiculoCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, VeiculoCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id);
    Task<bool> PlacaExisteAsync(string placa, int? idIgnorar = null);
}

// ==========================================
// IMPLEMENTAÇÃO DO SERVICE DE VEÍCULOS
// ==========================================
public class VeiculoService : IVeiculoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<VeiculoService> _logger;

    public VeiculoService(AppDbContext context, ILogger<VeiculoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<VeiculoListDto>> ListarAsync(VeiculoFiltros? filtros = null)
    {
        var query = _context.Veiculos.AsQueryable();
        
        // Filtros
        if (filtros != null)
        {
            if (filtros.IncluirInativos == false) // Default é false se nulo? Frontend manda false explicitamente
                query = query.Where(v => v.Ativo);
                
            if (!string.IsNullOrWhiteSpace(filtros.Busca))
            {
                var busca = filtros.Busca.ToUpper().Trim();
                query = query.Where(v => 
                    v.Placa.Contains(busca) || 
                    (v.Marca != null && v.Marca.ToUpper().Contains(busca)) ||
                    (v.Modelo != null && v.Modelo.ToUpper().Contains(busca)));
            }
            
            if (!string.IsNullOrWhiteSpace(filtros.TipoVeiculo))
                query = query.Where(v => v.TipoRodado == filtros.TipoVeiculo);
        }
        else
        {
            // Comportamento padrão se não passar filtros (apenas ativos)
            query = query.Where(v => v.Ativo);
        }

        // Paginação
        var pageNumber = filtros?.Pagina ?? 1;
        var pageSize = filtros?.TamanhoPagina ?? 25;
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(v => v.Placa)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VeiculoListDto
            {
                Id = v.Id,
                Placa = v.Placa,
                Marca = v.Marca,
                Modelo = v.Modelo,
                AnoModelo = v.AnoModelo,
                Tara = v.Tara,
                TipoRodado = v.TipoRodado,
                TipoCarroceria = v.TipoCarroceria,
                Uf = v.Uf,
                Ativo = v.Ativo
            })
            .ToListAsync();
            
        return new PagedResult<VeiculoListDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<VeiculoDto?> ObterPorIdAsync(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null) return null;

        return MapToDto(veiculo);
    }

    public async Task<VeiculoDto?> ObterPorPlacaAsync(string placa)
    {
        var veiculo = await _context.Veiculos
            .FirstOrDefaultAsync(v => v.Placa == placa.ToUpper().Trim());
        
        if (veiculo == null) return null;
        return MapToDto(veiculo);
    }

    public async Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(VeiculoCreateUpdateDto dto)
    {
        try
        {
            var placaNormalizada = dto.Placa.ToUpper().Trim();
            
            if (await PlacaExisteAsync(placaNormalizada))
                return (false, "Já existe um veículo com esta placa", null);

            var veiculo = new Veiculo
            {
                Placa = placaNormalizada,
                Marca = dto.Marca?.Trim(),
                Modelo = dto.Modelo?.Trim(),
                AnoFabricacao = dto.AnoFabricacao,
                AnoModelo = dto.AnoModelo,
                Tara = dto.Tara,
                CapacidadeKg = dto.CapacidadeKg,
                TipoRodado = dto.TipoRodado.Trim(),
                TipoCarroceria = dto.TipoCarroceria.Trim(),
                Uf = dto.Uf.ToUpper().Trim(),
                Renavam = dto.Renavam?.Trim(),
                Chassi = dto.Chassi?.Trim(),
                Cor = dto.Cor?.Trim(),
                TipoCombustivel = dto.TipoCombustivel?.Trim(),
                Rntrc = dto.Rntrc?.Trim(),
                Ativo = dto.Ativo,
                Observacoes = dto.Observacoes?.Trim(),
                DataCriacao = DateTime.Now
            };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Veículo criado: {Placa} (ID: {Id})", veiculo.Placa, veiculo.Id);
            return (true, "Veículo criado com sucesso", veiculo.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar veículo: {Placa}", dto.Placa);
            return (false, "Erro ao criar veículo: " + ex.Message, null);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, VeiculoCreateUpdateDto dto)
    {
        try
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return (false, "Veículo não encontrado");

            var placaNormalizada = dto.Placa.ToUpper().Trim();
            
            if (await PlacaExisteAsync(placaNormalizada, id))
                return (false, "Já existe outro veículo com esta placa");

            veiculo.Placa = placaNormalizada;
            veiculo.Marca = dto.Marca?.Trim();
            veiculo.Modelo = dto.Modelo?.Trim();
            veiculo.AnoFabricacao = dto.AnoFabricacao;
            veiculo.AnoModelo = dto.AnoModelo;
            veiculo.Tara = dto.Tara;
            veiculo.CapacidadeKg = dto.CapacidadeKg;
            veiculo.TipoRodado = dto.TipoRodado.Trim();
            veiculo.TipoCarroceria = dto.TipoCarroceria.Trim();
            veiculo.Uf = dto.Uf.ToUpper().Trim();
            veiculo.Renavam = dto.Renavam?.Trim();
            veiculo.Chassi = dto.Chassi?.Trim();
            veiculo.Cor = dto.Cor?.Trim();
            veiculo.TipoCombustivel = dto.TipoCombustivel?.Trim();
            veiculo.Rntrc = dto.Rntrc?.Trim();
            veiculo.Ativo = dto.Ativo;
            veiculo.Observacoes = dto.Observacoes?.Trim();
            veiculo.DataUltimaAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Veículo atualizado: {Placa} (ID: {Id})", veiculo.Placa, veiculo.Id);
            return (true, "Veículo atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar veículo ID: {Id}", id);
            return (false, "Erro ao atualizar veículo: " + ex.Message);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id)
    {
        try
        {
            var veiculo = await _context.Veiculos
                .Include(v => v.Viagens)
                .Include(v => v.Manutencoes)
                .FirstOrDefaultAsync(v => v.Id == id);
            
            if (veiculo == null)
                return (false, "Veículo não encontrado");

            // Verificar se tem viagens ou manutenções vinculadas
            if (veiculo.Viagens.Any())
                return (false, $"Não é possível excluir. Existem {veiculo.Viagens.Count} viagem(ns) vinculada(s)");

            if (veiculo.Manutencoes.Any())
                return (false, $"Não é possível excluir. Existem {veiculo.Manutencoes.Count} manutenção(ões) vinculada(s)");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Veículo excluído: {Placa} (ID: {Id})", veiculo.Placa, id);
            return (true, "Veículo excluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir veículo ID: {Id}", id);
            return (false, "Erro ao excluir veículo: " + ex.Message);
        }
    }

    public async Task<bool> PlacaExisteAsync(string placa, int? idIgnorar = null)
    {
        var query = _context.Veiculos.Where(v => v.Placa == placa.ToUpper().Trim());
        
        if (idIgnorar.HasValue)
            query = query.Where(v => v.Id != idIgnorar.Value);

        return await query.AnyAsync();
    }

    private static VeiculoDto MapToDto(Veiculo v)
    {
        return new VeiculoDto
        {
            Id = v.Id,
            Placa = v.Placa,
            Marca = v.Marca,
            Modelo = v.Modelo,
            AnoFabricacao = v.AnoFabricacao,
            AnoModelo = v.AnoModelo,
            Tara = v.Tara,
            CapacidadeKg = v.CapacidadeKg,
            TipoRodado = v.TipoRodado,
            TipoCarroceria = v.TipoCarroceria,
            Uf = v.Uf,
            Renavam = v.Renavam,
            Chassi = v.Chassi,
            Cor = v.Cor,
            TipoCombustivel = v.TipoCombustivel,
            Rntrc = v.Rntrc,
            Ativo = v.Ativo,
            Observacoes = v.Observacoes,
            DataCriacao = v.DataCriacao,
            DataUltimaAlteracao = v.DataUltimaAlteracao
        };
    }
}
