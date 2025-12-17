using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Services.Transporte;

// ==========================================
// INTERFACE DO SERVICE DE DESPESAS DE VIAGEM
// ==========================================
public interface IDespesaViagemService
{
    Task<List<DespesaViagemDto>> ListarPorViagemAsync(int viagemId);
    Task<DespesaViagemDto?> ObterPorIdAsync(int id);
    Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(DespesaViagemCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, DespesaViagemCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id);
}

// ==========================================
// IMPLEMENTAÇÃO DO SERVICE DE DESPESAS DE VIAGEM
// ==========================================
public class DespesaViagemService : IDespesaViagemService
{
    private readonly AppDbContext _context;
    private readonly ILogger<DespesaViagemService> _logger;

    public DespesaViagemService(AppDbContext context, ILogger<DespesaViagemService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<DespesaViagemDto>> ListarPorViagemAsync(int viagemId)
    {
        return await _context.DespesasViagem
            .Where(d => d.ViagemId == viagemId && d.Ativo)
            .OrderBy(d => d.DataDespesa)
            .Select(d => MapToDto(d))
            .ToListAsync();
    }

    public async Task<DespesaViagemDto?> ObterPorIdAsync(int id)
    {
        var despesa = await _context.DespesasViagem.FindAsync(id);
        if (despesa == null) return null;

        return MapToDto(despesa);
    }

    public async Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(DespesaViagemCreateUpdateDto dto)
    {
        try
        {
            // Validar viagem
            var viagemExiste = await _context.Viagens.AnyAsync(v => v.Id == dto.ViagemId);
            if (!viagemExiste)
                return (false, "Viagem não encontrada", null);

            var despesa = new DespesaViagem
            {
                ViagemId = dto.ViagemId,
                TipoDespesa = dto.TipoDespesa.Trim(),
                Descricao = dto.Descricao?.Trim(),
                Valor = dto.Valor,
                DataDespesa = dto.DataDespesa,
                NumeroDocumento = dto.NumeroDocumento?.Trim(),
                Local = dto.Local?.Trim(),
                KmAtual = dto.KmAtual,
                Litros = dto.Litros,
                Observacoes = dto.Observacoes?.Trim(),
                Ativo = dto.Ativo,
                DataCriacao = DateTime.Now
            };

            _context.DespesasViagem.Add(despesa);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Despesa criada: ID {Id}, Viagem {ViagemId}, Valor {Valor}", 
                despesa.Id, despesa.ViagemId, despesa.Valor);
            return (true, "Despesa criada com sucesso", despesa.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar despesa para viagem {ViagemId}", dto.ViagemId);
            return (false, "Erro ao criar despesa: " + ex.Message, null);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, DespesaViagemCreateUpdateDto dto)
    {
        try
        {
            var despesa = await _context.DespesasViagem.FindAsync(id);
            if (despesa == null)
                return (false, "Despesa não encontrada");

            despesa.TipoDespesa = dto.TipoDespesa.Trim();
            despesa.Descricao = dto.Descricao?.Trim();
            despesa.Valor = dto.Valor;
            despesa.DataDespesa = dto.DataDespesa;
            despesa.NumeroDocumento = dto.NumeroDocumento?.Trim();
            despesa.Local = dto.Local?.Trim();
            despesa.KmAtual = dto.KmAtual;
            despesa.Litros = dto.Litros;
            despesa.Observacoes = dto.Observacoes?.Trim();
            despesa.Ativo = dto.Ativo;
            despesa.DataUltimaAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Despesa atualizada: ID {Id}", despesa.Id);
            return (true, "Despesa atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar despesa ID: {Id}", id);
            return (false, "Erro ao atualizar despesa: " + ex.Message);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id)
    {
        try
        {
            var despesa = await _context.DespesasViagem.FindAsync(id);
            if (despesa == null)
                return (false, "Despesa não encontrada");

            _context.DespesasViagem.Remove(despesa);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Despesa excluída: ID {Id}", id);
            return (true, "Despesa excluída com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir despesa ID: {Id}", id);
            return (false, "Erro ao excluir despesa: " + ex.Message);
        }
    }

    private static DespesaViagemDto MapToDto(DespesaViagem d)
    {
        return new DespesaViagemDto
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
        };
    }
}

// ==========================================
// INTERFACE DO SERVICE DE RECEITAS DE VIAGEM
// ==========================================
public interface IReceitaViagemService
{
    Task<List<ReceitaViagemDto>> ListarPorViagemAsync(int viagemId);
    Task<ReceitaViagemDto?> ObterPorIdAsync(int id);
    Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ReceitaViagemCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ReceitaViagemCreateUpdateDto dto);
    Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id);
}

// ==========================================
// IMPLEMENTAÇÃO DO SERVICE DE RECEITAS DE VIAGEM
// ==========================================
public class ReceitaViagemService : IReceitaViagemService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReceitaViagemService> _logger;

    public ReceitaViagemService(AppDbContext context, ILogger<ReceitaViagemService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ReceitaViagemDto>> ListarPorViagemAsync(int viagemId)
    {
        return await _context.ReceitasViagem
            .Where(r => r.ViagemId == viagemId && r.Ativo)
            .OrderBy(r => r.DataReceita)
            .Select(r => MapToDto(r))
            .ToListAsync();
    }

    public async Task<ReceitaViagemDto?> ObterPorIdAsync(int id)
    {
        var receita = await _context.ReceitasViagem.FindAsync(id);
        if (receita == null) return null;

        return MapToDto(receita);
    }

    public async Task<(bool Sucesso, string Mensagem, int? Id)> CriarAsync(ReceitaViagemCreateUpdateDto dto)
    {
        try
        {
            // Validar viagem
            var viagemExiste = await _context.Viagens.AnyAsync(v => v.Id == dto.ViagemId);
            if (!viagemExiste)
                return (false, "Viagem não encontrada", null);

            var receita = new ReceitaViagem
            {
                ViagemId = dto.ViagemId,
                Descricao = dto.Descricao.Trim(),
                Valor = dto.Valor,
                DataReceita = dto.DataReceita,
                Origem = dto.Origem?.Trim(),
                NumeroDocumento = dto.NumeroDocumento?.Trim(),
                Cliente = dto.Cliente?.Trim(),
                Observacoes = dto.Observacoes?.Trim(),
                Ativo = dto.Ativo,
                DataCriacao = DateTime.Now
            };

            _context.ReceitasViagem.Add(receita);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Receita criada: ID {Id}, Viagem {ViagemId}, Valor {Valor}", 
                receita.Id, receita.ViagemId, receita.Valor);
            return (true, "Receita criada com sucesso", receita.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar receita para viagem {ViagemId}", dto.ViagemId);
            return (false, "Erro ao criar receita: " + ex.Message, null);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, ReceitaViagemCreateUpdateDto dto)
    {
        try
        {
            var receita = await _context.ReceitasViagem.FindAsync(id);
            if (receita == null)
                return (false, "Receita não encontrada");

            receita.Descricao = dto.Descricao.Trim();
            receita.Valor = dto.Valor;
            receita.DataReceita = dto.DataReceita;
            receita.Origem = dto.Origem?.Trim();
            receita.NumeroDocumento = dto.NumeroDocumento?.Trim();
            receita.Cliente = dto.Cliente?.Trim();
            receita.Observacoes = dto.Observacoes?.Trim();
            receita.Ativo = dto.Ativo;
            receita.DataUltimaAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Receita atualizada: ID {Id}", receita.Id);
            return (true, "Receita atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar receita ID: {Id}", id);
            return (false, "Erro ao atualizar receita: " + ex.Message);
        }
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id)
    {
        try
        {
            var receita = await _context.ReceitasViagem.FindAsync(id);
            if (receita == null)
                return (false, "Receita não encontrada");

            _context.ReceitasViagem.Remove(receita);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Receita excluída: ID {Id}", id);
            return (true, "Receita excluída com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir receita ID: {Id}", id);
            return (false, "Erro ao excluir receita: " + ex.Message);
        }
    }

    private static ReceitaViagemDto MapToDto(ReceitaViagem r)
    {
        return new ReceitaViagemDto
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
        };
    }
}
