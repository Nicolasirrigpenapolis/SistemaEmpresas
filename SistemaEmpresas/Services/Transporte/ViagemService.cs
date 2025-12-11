using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models.Transporte;

namespace SistemaEmpresas.Services.Transporte;

// ==========================================
// INTERFACE DO SERVICE DE VIAGENS
// ==========================================
public interface IViagemService
{
    Task<DTOs.PagedResult<ViagemListDto>> ListarAsync(ViagemFiltros? filtros = null);
    Task<List<ViagemListDto>> ListarPorVeiculoAsync(int veiculoId);
    Task<List<ViagemListDto>> ListarPorMotoristaAsync(short motoristaId);
    Task<List<ViagemListDto>> ListarPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<ViagemDto?> ObterPorIdAsync(int id);
    Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ViagemCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ViagemCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id);
}

// ==========================================
// IMPLEMENTAÇÃO DO SERVICE DE VIAGENS
// ==========================================
public class ViagemService : IViagemService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ViagemService> _logger;

    public ViagemService(AppDbContext context, ILogger<ViagemService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DTOs.PagedResult<ViagemListDto>> ListarAsync(ViagemFiltros? filtros = null)
    {
        var query = _context.Viagens
            .Include(v => v.Veiculo)
            .Include(v => v.Motorista)
            .Include(v => v.Reboque)
            .Include(v => v.Despesas)
            .Include(v => v.Receitas)
            .AsQueryable();
        
        // Filtros
        if (filtros != null)
        {
            if (!string.IsNullOrWhiteSpace(filtros.Busca))
            {
                var busca = filtros.Busca.ToUpper().Trim();
                query = query.Where(v => 
                    v.Origem.ToUpper().Contains(busca) || 
                    v.Destino.ToUpper().Contains(busca) ||
                    (v.Veiculo != null && v.Veiculo.Placa.Contains(busca)) ||
                    (v.Motorista != null && v.Motorista.NomeDoMotorista.ToUpper().Contains(busca)));
            }

            if (filtros.DataInicio.HasValue)
                query = query.Where(v => v.DataInicio >= filtros.DataInicio.Value);

            if (filtros.DataFim.HasValue)
                query = query.Where(v => v.DataFim <= filtros.DataFim.Value);
        }

        // Paginação
        var pageNumber = filtros?.Pagina ?? 1;
        var pageSize = filtros?.TamanhoPagina ?? 25;
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(v => v.DataInicio)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(v => MapToListDto(v))
            .ToListAsync();
            
        return new DTOs.PagedResult<ViagemListDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<List<ViagemListDto>> ListarPorVeiculoAsync(int veiculoId)
    {
        return await _context.Viagens
            .Include(v => v.Veiculo)
            .Include(v => v.Motorista)
            .Include(v => v.Reboque)
            .Include(v => v.Despesas)
            .Include(v => v.Receitas)
            .Where(v => v.VeiculoId == veiculoId)
            .OrderByDescending(v => v.DataInicio)
            .Select(v => MapToListDto(v))
            .ToListAsync();
    }

    public async Task<List<ViagemListDto>> ListarPorMotoristaAsync(short motoristaId)
    {
        return await _context.Viagens
            .Include(v => v.Veiculo)
            .Include(v => v.Motorista)
            .Include(v => v.Reboque)
            .Include(v => v.Despesas)
            .Include(v => v.Receitas)
            .Where(v => v.MotoristaId == motoristaId)
            .OrderByDescending(v => v.DataInicio)
            .Select(v => MapToListDto(v))
            .ToListAsync();
    }

    public async Task<List<ViagemListDto>> ListarPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _context.Viagens
            .Include(v => v.Veiculo)
            .Include(v => v.Motorista)
            .Include(v => v.Reboque)
            .Include(v => v.Despesas)
            .Include(v => v.Receitas)
            .Where(v => v.DataInicio >= dataInicio && v.DataFim <= dataFim)
            .OrderByDescending(v => v.DataInicio)
            .Select(v => MapToListDto(v))
            .ToListAsync();
    }

    public async Task<ViagemDto?> ObterPorIdAsync(int id)
    {
        var viagem = await _context.Viagens
            .Include(v => v.Veiculo)
            .Include(v => v.Motorista)
            .Include(v => v.Reboque)
            .Include(v => v.Despesas)
            .Include(v => v.Receitas)
            .FirstOrDefaultAsync(v => v.Id == id);
        
        if (viagem == null) return null;

        return MapToDto(viagem);
    }

    public async Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ViagemCreateUpdateDto dto)
    {
        try
        {
            // Validar veículo
            var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == dto.VeiculoId && v.Ativo);
            if (!veiculoExiste)
                return (false, "Veículo não encontrado ou inativo", null);

            // Validar motorista se informado
            if (dto.MotoristaId.HasValue)
            {
                var motoristaExiste = await _context.Motoristas.AnyAsync(m => m.CodigoDoMotorista == dto.MotoristaId.Value);
                if (!motoristaExiste)
                    return (false, "Motorista não encontrado", null);
            }

            // Validar reboque se informado
            if (dto.ReboqueId.HasValue)
            {
                var reboqueExiste = await _context.Reboques.AnyAsync(r => r.Id == dto.ReboqueId.Value && r.Ativo);
                if (!reboqueExiste)
                    return (false, "Reboque não encontrado ou inativo", null);
            }

            // Validar datas
            if (dto.DataFim < dto.DataInicio)
                return (false, "Data de fim não pode ser anterior à data de início", null);

            var viagem = new Viagem
            {
                VeiculoId = dto.VeiculoId,
                MotoristaId = dto.MotoristaId,
                ReboqueId = dto.ReboqueId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                KmInicial = dto.KmInicial,
                KmFinal = dto.KmFinal,
                Origem = dto.Origem?.Trim(),
                Destino = dto.Destino?.Trim(),
                DescricaoCarga = dto.DescricaoCarga?.Trim(),
                PesoCarga = dto.PesoCarga,
                Observacoes = dto.Observacoes?.Trim(),
                Ativo = dto.Ativo,
                DataCriacao = DateTime.Now
            };

            _context.Viagens.Add(viagem);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Viagem criada: ID {Id}, Veículo {VeiculoId}", viagem.Id, viagem.VeiculoId);
            return (true, "Viagem criada com sucesso", viagem.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar viagem");
            return (false, "Erro ao criar viagem: " + ex.Message, null);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ViagemCreateUpdateDto dto)
    {
        try
        {
            var viagem = await _context.Viagens.FindAsync(id);
            if (viagem == null)
                return (false, "Viagem não encontrada");

            // Validar veículo
            var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == dto.VeiculoId && v.Ativo);
            if (!veiculoExiste)
                return (false, "Veículo não encontrado ou inativo");

            // Validar motorista se informado
            if (dto.MotoristaId.HasValue)
            {
                var motoristaExiste = await _context.Motoristas.AnyAsync(m => m.CodigoDoMotorista == dto.MotoristaId.Value);
                if (!motoristaExiste)
                    return (false, "Motorista não encontrado");
            }

            // Validar reboque se informado
            if (dto.ReboqueId.HasValue)
            {
                var reboqueExiste = await _context.Reboques.AnyAsync(r => r.Id == dto.ReboqueId.Value && r.Ativo);
                if (!reboqueExiste)
                    return (false, "Reboque não encontrado ou inativo");
            }

            // Validar datas
            if (dto.DataFim < dto.DataInicio)
                return (false, "Data de fim não pode ser anterior à data de início");

            viagem.VeiculoId = dto.VeiculoId;
            viagem.MotoristaId = dto.MotoristaId;
            viagem.ReboqueId = dto.ReboqueId;
            viagem.DataInicio = dto.DataInicio;
            viagem.DataFim = dto.DataFim;
            viagem.KmInicial = dto.KmInicial;
            viagem.KmFinal = dto.KmFinal;
            viagem.Origem = dto.Origem?.Trim();
            viagem.Destino = dto.Destino?.Trim();
            viagem.DescricaoCarga = dto.DescricaoCarga?.Trim();
            viagem.PesoCarga = dto.PesoCarga;
            viagem.Observacoes = dto.Observacoes?.Trim();
            viagem.Ativo = dto.Ativo;
            viagem.DataUltimaAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Viagem atualizada: ID {Id}", viagem.Id);
            return (true, "Viagem atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar viagem ID: {Id}", id);
            return (false, "Erro ao atualizar viagem: " + ex.Message);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id)
    {
        try
        {
            var viagem = await _context.Viagens
                .Include(v => v.Despesas)
                .Include(v => v.Receitas)
                .FirstOrDefaultAsync(v => v.Id == id);
            
            if (viagem == null)
                return (false, "Viagem não encontrada");

            // Excluir despesas e receitas vinculadas
            if (viagem.Despesas.Any())
                _context.DespesasViagem.RemoveRange(viagem.Despesas);
            
            if (viagem.Receitas.Any())
                _context.ReceitasViagem.RemoveRange(viagem.Receitas);

            _context.Viagens.Remove(viagem);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Viagem excluída: ID {Id}", id);
            return (true, "Viagem excluída com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir viagem ID: {Id}", id);
            return (false, "Erro ao excluir viagem: " + ex.Message);
        }
    }

    private static ViagemListDto MapToListDto(Viagem v)
    {
        return new ViagemListDto
        {
            Id = v.Id,
            VeiculoPlaca = v.Veiculo?.Placa ?? "",
            MotoristaNome = v.Motorista?.NomeDoMotorista,
            ReboquePlaca = v.Reboque?.Placa,
            DataInicio = v.DataInicio,
            DataFim = v.DataFim,
            Origem = v.Origem,
            Destino = v.Destino,
            KmPercorrido = v.KmPercorrido,
            TotalDespesas = v.TotalDespesas,
            ReceitaTotal = v.ReceitaTotal,
            SaldoLiquido = v.SaldoLiquido,
            Ativo = v.Ativo
        };
    }

    private static ViagemDto MapToDto(Viagem v)
    {
        return new ViagemDto
        {
            Id = v.Id,
            VeiculoId = v.VeiculoId,
            VeiculoPlaca = v.Veiculo?.Placa ?? "",
            MotoristaId = v.MotoristaId,
            MotoristaNome = v.Motorista?.NomeDoMotorista,
            ReboqueId = v.ReboqueId,
            ReboquePlaca = v.Reboque?.Placa,
            DataInicio = v.DataInicio,
            DataFim = v.DataFim,
            KmInicial = v.KmInicial,
            KmFinal = v.KmFinal,
            Origem = v.Origem,
            Destino = v.Destino,
            DescricaoCarga = v.DescricaoCarga,
            PesoCarga = v.PesoCarga,
            Observacoes = v.Observacoes,
            Ativo = v.Ativo,
            DataCriacao = v.DataCriacao,
            DataUltimaAlteracao = v.DataUltimaAlteracao,
            KmPercorrido = v.KmPercorrido,
            DuracaoDias = v.DuracaoDias,
            TotalDespesas = v.TotalDespesas,
            ReceitaTotal = v.ReceitaTotal,
            SaldoLiquido = v.SaldoLiquido,
            CustoPorKm = v.CustoPorKm,
            Despesas = v.Despesas?.Select(d => new DespesaViagemDto
            {
                Id = d.Id,
                ViagemId = d.ViagemId,
                TipoDespesa = d.TipoDespesa,
                Descricao = d.Descricao,
                Valor = d.Valor,
                DataDespesa = d.DataDespesa,
                NumeroDocumento = d.NumeroDocumento,
                Local = d.Local,
                KmAtual = d.KmAtual,
                Litros = d.Litros,
                Observacoes = d.Observacoes,
                Ativo = d.Ativo,
                DataCriacao = d.DataCriacao,
                DataUltimaAlteracao = d.DataUltimaAlteracao,
                PrecoPorLitro = d.PrecoPorLitro
            }).ToList() ?? new List<DespesaViagemDto>(),
            Receitas = v.Receitas?.Select(r => new ReceitaViagemDto
            {
                Id = r.Id,
                ViagemId = r.ViagemId,
                Descricao = r.Descricao,
                Valor = r.Valor,
                DataReceita = r.DataReceita,
                Origem = r.Origem,
                NumeroDocumento = r.NumeroDocumento,
                Cliente = r.Cliente,
                Observacoes = r.Observacoes,
                Ativo = r.Ativo,
                DataCriacao = r.DataCriacao,
                DataUltimaAlteracao = r.DataUltimaAlteracao
            }).ToList() ?? new List<ReceitaViagemDto>()
        };
    }
}
