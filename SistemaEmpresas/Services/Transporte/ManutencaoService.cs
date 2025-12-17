using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services.Transporte;

// ==========================================
// INTERFACE DO SERVICE DE MANUTENÇÃO DE VEÍCULOS
// ==========================================
public interface IManutencaoVeiculoService
{
    Task<PagedResult<ManutencaoVeiculoListDto>> ListarAsync(ManutencaoFiltros filtros);
    Task<List<ManutencaoVeiculoListDto>> ListarPorVeiculoAsync(int veiculoId);
    Task<ManutencaoVeiculoDto?> ObterPorIdAsync(int id);
    Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ManutencaoVeiculoCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ManutencaoVeiculoCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id);
}

// ==========================================
// IMPLEMENTAÇÃO DO SERVICE DE MANUTENÇÃO DE VEÍCULOS
// ==========================================
public class ManutencaoVeiculoService : IManutencaoVeiculoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ManutencaoVeiculoService> _logger;

    public ManutencaoVeiculoService(AppDbContext context, ILogger<ManutencaoVeiculoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<ManutencaoVeiculoListDto>> ListarAsync(ManutencaoFiltros filtros)
    {
        var query = _context.ManutencoesVeiculo
            .Include(m => m.Veiculo)
            .Include(m => m.Fornecedor)
            .AsQueryable();
        
        // Filtros
        if (filtros.IncluirInativos != true)
            query = query.Where(m => m.Ativo);

        if (!string.IsNullOrEmpty(filtros.Busca))
        {
            var busca = filtros.Busca.ToLower();
            query = query.Where(m => 
                m.Veiculo.Placa.ToLower().Contains(busca) || 
                (m.Fornecedor != null && m.Fornecedor.NomeFantasia.ToLower().Contains(busca)) ||
                (m.DescricaoServico != null && m.DescricaoServico.ToLower().Contains(busca)) ||
                (m.NumeroOS != null && m.NumeroOS.ToLower().Contains(busca)) ||
                (m.NumeroNF != null && m.NumeroNF.ToLower().Contains(busca)));
        }

        if (filtros.VeiculoId.HasValue)
            query = query.Where(m => m.VeiculoId == filtros.VeiculoId.Value);

        if (!string.IsNullOrEmpty(filtros.TipoManutencao))
            query = query.Where(m => m.TipoManutencao == filtros.TipoManutencao);

        if (filtros.DataInicio.HasValue)
            query = query.Where(m => m.DataManutencao >= filtros.DataInicio.Value);

        if (filtros.DataFim.HasValue)
            query = query.Where(m => m.DataManutencao <= filtros.DataFim.Value);

        // Paginação
        var totalItems = await query.CountAsync();
        var items = await query
            .OrderByDescending(m => m.DataManutencao)
            .Skip((filtros.Pagina - 1) * filtros.TamanhoPagina)
            .Take(filtros.TamanhoPagina)
            .Select(m => new ManutencaoVeiculoListDto
            {
                Id = m.Id,
                VeiculoPlaca = m.Veiculo.Placa,
                FornecedorNome = m.Fornecedor != null ? m.Fornecedor.NomeFantasia : null,
                DataManutencao = m.DataManutencao,
                TipoManutencao = m.TipoManutencao,
                KmAtual = m.KmAtual,
                CustoTotal = m.CustoTotal,
                Ativo = m.Ativo
            })
            .ToListAsync();

        return new PagedResult<ManutencaoVeiculoListDto>
        {
            Items = items,
            TotalCount = totalItems,
            PageNumber = filtros.Pagina,
            PageSize = filtros.TamanhoPagina,
            TotalPages = (int)Math.Ceiling(totalItems / (double)filtros.TamanhoPagina)
        };
    }

    public async Task<List<ManutencaoVeiculoListDto>> ListarPorVeiculoAsync(int veiculoId)
    {
        return await _context.ManutencoesVeiculo
            .Include(m => m.Veiculo)
            .Include(m => m.Fornecedor)
            .Include(m => m.Pecas)
            .Where(m => m.VeiculoId == veiculoId)
            .OrderByDescending(m => m.DataManutencao)
            .Select(m => new ManutencaoVeiculoListDto
            {
                Id = m.Id,
                VeiculoPlaca = m.Veiculo.Placa,
                FornecedorNome = m.Fornecedor != null ? m.Fornecedor.NomeFantasia : null,
                DataManutencao = m.DataManutencao,
                TipoManutencao = m.TipoManutencao,
                KmAtual = m.KmAtual,
                CustoTotal = m.CustoTotal,
                Ativo = m.Ativo
            })
            .ToListAsync();
    }

    public async Task<ManutencaoVeiculoDto?> ObterPorIdAsync(int id)
    {
        var manutencao = await _context.ManutencoesVeiculo
            .Include(m => m.Veiculo)
            .Include(m => m.Fornecedor)
            .Include(m => m.Pecas)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (manutencao == null) return null;

        return MapToDto(manutencao);
    }

    public async Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ManutencaoVeiculoCreateUpdateDto dto)
    {
        try
        {
            // Validar veículo
            var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == dto.VeiculoId && v.Ativo);
            if (!veiculoExiste)
                return (false, "Veículo não encontrado ou inativo", null);

            // Validar fornecedor se informado
            if (dto.FornecedorId.HasValue)
            {
                var fornecedorExiste = await _context.Gerals.AnyAsync(g => g.SequenciaDoGeral == dto.FornecedorId.Value && g.Fornecedor == true);
                if (!fornecedorExiste)
                    return (false, "Fornecedor não encontrado", null);
            }

            var manutencao = new ManutencaoVeiculo
            {
                VeiculoId = dto.VeiculoId,
                FornecedorId = dto.FornecedorId,
                DataManutencao = dto.DataManutencao,
                TipoManutencao = dto.TipoManutencao?.Trim(),
                DescricaoServico = dto.DescricaoServico?.Trim(),
                KmAtual = dto.KmAtual,
                ValorMaoObra = dto.ValorMaoObra,
                ValorServicosTerceiros = dto.ValorServicosTerceiros,
                NumeroOS = dto.NumeroOS?.Trim(),
                NumeroNF = dto.NumeroNF?.Trim(),
                DataProximaManutencao = dto.DataProximaManutencao,
                KmProximaManutencao = dto.KmProximaManutencao,
                Observacoes = dto.Observacoes?.Trim(),
                Ativo = dto.Ativo,
                DataCriacao = DateTime.Now
            };

            _context.ManutencoesVeiculo.Add(manutencao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Manutenção criada: ID {Id}, Veículo {VeiculoId}", manutencao.Id, manutencao.VeiculoId);
            return (true, "Manutenção criada com sucesso", manutencao.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar manutenção para veículo {VeiculoId}", dto.VeiculoId);
            return (false, "Erro ao criar manutenção: " + ex.Message, null);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ManutencaoVeiculoCreateUpdateDto dto)
    {
        try
        {
            var manutencao = await _context.ManutencoesVeiculo.FindAsync(id);
            if (manutencao == null)
                return (false, "Manutenção não encontrada");

            // Validar veículo
            var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == dto.VeiculoId && v.Ativo);
            if (!veiculoExiste)
                return (false, "Veículo não encontrado ou inativo");

            // Validar fornecedor se informado
            if (dto.FornecedorId.HasValue)
            {
                var fornecedorExiste = await _context.Gerals.AnyAsync(g => g.SequenciaDoGeral == dto.FornecedorId.Value && g.Fornecedor == true);
                if (!fornecedorExiste)
                    return (false, "Fornecedor não encontrado");
            }

            manutencao.VeiculoId = dto.VeiculoId;
            manutencao.FornecedorId = dto.FornecedorId;
            manutencao.DataManutencao = dto.DataManutencao;
            manutencao.TipoManutencao = dto.TipoManutencao?.Trim();
            manutencao.DescricaoServico = dto.DescricaoServico?.Trim();
            manutencao.KmAtual = dto.KmAtual;
            manutencao.ValorMaoObra = dto.ValorMaoObra;
            manutencao.ValorServicosTerceiros = dto.ValorServicosTerceiros;
            manutencao.NumeroOS = dto.NumeroOS?.Trim();
            manutencao.NumeroNF = dto.NumeroNF?.Trim();
            manutencao.DataProximaManutencao = dto.DataProximaManutencao;
            manutencao.KmProximaManutencao = dto.KmProximaManutencao;
            manutencao.Observacoes = dto.Observacoes?.Trim();
            manutencao.Ativo = dto.Ativo;
            manutencao.DataUltimaAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Manutenção atualizada: ID {Id}", manutencao.Id);
            return (true, "Manutenção atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar manutenção ID: {Id}", id);
            return (false, "Erro ao atualizar manutenção: " + ex.Message);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id)
    {
        try
        {
            var manutencao = await _context.ManutencoesVeiculo
                .Include(m => m.Pecas)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (manutencao == null)
                return (false, "Manutenção não encontrada");

            // Excluir peças vinculadas
            if (manutencao.Pecas.Any())
                _context.ManutencoesPeca.RemoveRange(manutencao.Pecas);

            _context.ManutencoesVeiculo.Remove(manutencao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Manutenção excluída: ID {Id}", id);
            return (true, "Manutenção excluída com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir manutenção ID: {Id}", id);
            return (false, "Erro ao excluir manutenção: " + ex.Message);
        }
    }

    private static ManutencaoVeiculoDto MapToDto(ManutencaoVeiculo m)
    {
        return new ManutencaoVeiculoDto
        {
            Id = m.Id,
            VeiculoId = m.VeiculoId,
            VeiculoPlaca = m.Veiculo?.Placa ?? "",
            FornecedorId = m.FornecedorId,
            FornecedorNome = m.Fornecedor?.RazaoSocial,
            DataManutencao = m.DataManutencao,
            TipoManutencao = m.TipoManutencao,
            DescricaoServico = m.DescricaoServico,
            KmAtual = m.KmAtual,
            ValorMaoObra = m.ValorMaoObra,
            ValorServicosTerceiros = m.ValorServicosTerceiros,
            NumeroOS = m.NumeroOS,
            NumeroNF = m.NumeroNF,
            DataProximaManutencao = m.DataProximaManutencao,
            KmProximaManutencao = m.KmProximaManutencao,
            Observacoes = m.Observacoes,
            Ativo = m.Ativo,
            DataCriacao = m.DataCriacao,
            DataUltimaAlteracao = m.DataUltimaAlteracao,
            TotalPecas = m.TotalPecas,
            CustoTotal = m.CustoTotal,
            Pecas = m.Pecas?.Select(p => new ManutencaoPecaDto
            {
                Id = p.Id,
                ManutencaoId = p.ManutencaoId,
                DescricaoPeca = p.DescricaoPeca,
                CodigoPeca = p.CodigoPeca,
                Marca = p.Marca,
                Quantidade = p.Quantidade,
                Unidade = p.Unidade,
                ValorUnitario = p.ValorUnitario,
                Observacoes = p.Observacoes,
                Ativo = p.Ativo,
                DataCriacao = p.DataCriacao,
                DataUltimaAlteracao = p.DataUltimaAlteracao,
                ValorTotal = p.ValorTotal
            }).ToList() ?? new List<ManutencaoPecaDto>()
        };
    }
}

// ==========================================
// INTERFACE DO SERVICE DE PEÇAS DE MANUTENÇÃO
// ==========================================
public interface IManutencaoPecaService
{
    Task<List<ManutencaoPecaDto>> ListarPorManutencaoAsync(int manutencaoId);
    Task<ManutencaoPecaDto?> ObterPorIdAsync(int id);
    Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ManutencaoPecaCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ManutencaoPecaCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id);
}

// ==========================================
// IMPLEMENTAÇÃO DO SERVICE DE PEÇAS DE MANUTENÇÃO
// ==========================================
public class ManutencaoPecaService : IManutencaoPecaService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ManutencaoPecaService> _logger;

    public ManutencaoPecaService(AppDbContext context, ILogger<ManutencaoPecaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ManutencaoPecaDto>> ListarPorManutencaoAsync(int manutencaoId)
    {
        return await _context.ManutencoesPeca
            .Where(p => p.ManutencaoId == manutencaoId && p.Ativo)
            .OrderBy(p => p.DescricaoPeca)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<ManutencaoPecaDto?> ObterPorIdAsync(int id)
    {
        var peca = await _context.ManutencoesPeca.FindAsync(id);
        if (peca == null) return null;

        return MapToDto(peca);
    }

    public async Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ManutencaoPecaCreateUpdateDto dto)
    {
        try
        {
            // Validar manutenção
            var manutencaoExiste = await _context.ManutencoesVeiculo.AnyAsync(m => m.Id == dto.ManutencaoId);
            if (!manutencaoExiste)
                return (false, "Manutenção não encontrada", null);

            var peca = new ManutencaoPeca
            {
                ManutencaoId = dto.ManutencaoId,
                DescricaoPeca = dto.DescricaoPeca.Trim(),
                CodigoPeca = dto.CodigoPeca?.Trim(),
                Marca = dto.Marca?.Trim(),
                Quantidade = dto.Quantidade,
                Unidade = dto.Unidade?.Trim() ?? "UN",
                ValorUnitario = dto.ValorUnitario,
                Observacoes = dto.Observacoes?.Trim(),
                Ativo = dto.Ativo,
                DataCriacao = DateTime.Now
            };

            _context.ManutencoesPeca.Add(peca);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Peça criada: ID {Id}, Manutenção {ManutencaoId}", peca.Id, peca.ManutencaoId);
            return (true, "Peça criada com sucesso", peca.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar peça para manutenção {ManutencaoId}", dto.ManutencaoId);
            return (false, "Erro ao criar peça: " + ex.Message, null);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ManutencaoPecaCreateUpdateDto dto)
    {
        try
        {
            var peca = await _context.ManutencoesPeca.FindAsync(id);
            if (peca == null)
                return (false, "Peça não encontrada");

            peca.DescricaoPeca = dto.DescricaoPeca.Trim();
            peca.CodigoPeca = dto.CodigoPeca?.Trim();
            peca.Marca = dto.Marca?.Trim();
            peca.Quantidade = dto.Quantidade;
            peca.Unidade = dto.Unidade?.Trim() ?? "UN";
            peca.ValorUnitario = dto.ValorUnitario;
            peca.Observacoes = dto.Observacoes?.Trim();
            peca.Ativo = dto.Ativo;
            peca.DataUltimaAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Peça atualizada: ID {Id}", peca.Id);
            return (true, "Peça atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar peça ID: {Id}", id);
            return (false, "Erro ao atualizar peça: " + ex.Message);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id)
    {
        try
        {
            var peca = await _context.ManutencoesPeca.FindAsync(id);
            if (peca == null)
                return (false, "Peça não encontrada");

            _context.ManutencoesPeca.Remove(peca);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Peça excluída: ID {Id}", id);
            return (true, "Peça excluída com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir peça ID: {Id}", id);
            return (false, "Erro ao excluir peça: " + ex.Message);
        }
    }

    private static ManutencaoPecaDto MapToDto(ManutencaoPeca p)
    {
        return new ManutencaoPecaDto
        {
            Id = p.Id,
            ManutencaoId = p.ManutencaoId,
            DescricaoPeca = p.DescricaoPeca,
            CodigoPeca = p.CodigoPeca,
            Marca = p.Marca,
            Quantidade = p.Quantidade,
            Unidade = p.Unidade,
            ValorUnitario = p.ValorUnitario,
            Observacoes = p.Observacoes,
            Ativo = p.Ativo,
            DataCriacao = p.DataCriacao,
            DataUltimaAlteracao = p.DataUltimaAlteracao,
            ValorTotal = p.ValorTotal
        };
    }
}
