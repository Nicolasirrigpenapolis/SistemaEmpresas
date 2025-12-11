using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.DTOs;
using SistemaEmpresas.Models;
using SistemaEmpresas.Repositories;

namespace SistemaEmpresas.Services.Transporte;

public interface IMotoristaService
{
    Task<DTOs.PagedResult<MotoristaListDto>> ListarAsync(MotoristaFiltrosDto filtros);
    Task<List<MotoristaListDto>> ListarAtivosAsync();
    Task<MotoristaDto?> BuscarPorIdAsync(short id);
    Task<MotoristaDto> CriarAsync(MotoristaCreateDto dto);
    Task<MotoristaDto> AtualizarAsync(short id, MotoristaUpdateDto dto);
    Task ExcluirAsync(short id);
}

public class MotoristaService : IMotoristaService
{
    private readonly AppDbContext _context;

    public MotoristaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DTOs.PagedResult<MotoristaListDto>> ListarAsync(MotoristaFiltrosDto filtros)
    {
        var query = _context.Motoristas.AsQueryable();

        // Filtro por busca (nome, CPF)
        if (!string.IsNullOrWhiteSpace(filtros.Busca))
        {
            var busca = filtros.Busca.ToLower();
            query = query.Where(m =>
                m.NomeDoMotorista.ToLower().Contains(busca) ||
                m.Cpf.Contains(busca));
        }

        // Filtro por UF
        if (!string.IsNullOrWhiteSpace(filtros.Uf))
        {
            query = query.Where(m => m.Uf == filtros.Uf);
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(m => m.NomeDoMotorista)
            .Skip((filtros.Pagina - 1) * filtros.TamanhoPagina)
            .Take(filtros.TamanhoPagina)
            .Select(m => new MotoristaListDto
            {
                CodigoDoMotorista = m.CodigoDoMotorista,
                NomeDoMotorista = m.NomeDoMotorista,
                Cpf = m.Cpf,
                Rg = m.Rg,
                Cel = m.Cel,
                Uf = m.Uf
            })
            .ToListAsync();

        return new DTOs.PagedResult<MotoristaListDto>
        {
            Items = items,
            TotalCount = total,
            PageNumber = filtros.Pagina,
            PageSize = filtros.TamanhoPagina
        };
    }

    public async Task<List<MotoristaListDto>> ListarAtivosAsync()
    {
        return await _context.Motoristas
            .OrderBy(m => m.NomeDoMotorista)
            .Select(m => new MotoristaListDto
            {
                CodigoDoMotorista = m.CodigoDoMotorista,
                NomeDoMotorista = m.NomeDoMotorista,
                Cpf = m.Cpf,
                Rg = m.Rg,
                Cel = m.Cel,
                Uf = m.Uf
            })
            .ToListAsync();
    }

    public async Task<MotoristaDto?> BuscarPorIdAsync(short id)
    {
        var motorista = await _context.Motoristas
            .FirstOrDefaultAsync(m => m.CodigoDoMotorista == id);

        if (motorista == null) return null;

        // Buscar nome do município
        var municipioNome = await _context.Municipios
            .Where(m => m.SequenciaDoMunicipio == motorista.Municipio)
            .Select(m => m.Descricao)
            .FirstOrDefaultAsync();

        return new MotoristaDto
        {
            CodigoDoMotorista = motorista.CodigoDoMotorista,
            NomeDoMotorista = motorista.NomeDoMotorista,
            Rg = motorista.Rg,
            Cpf = motorista.Cpf,
            Endereco = motorista.Endereco,
            Numero = motorista.Numero,
            Bairro = motorista.Bairro,
            Municipio = motorista.Municipio,
            MunicipioNome = municipioNome ?? "",
            Uf = motorista.Uf,
            Cep = motorista.Cep,
            Fone = motorista.Fone,
            Cel = motorista.Cel
        };
    }

    public async Task<MotoristaDto> CriarAsync(MotoristaCreateDto dto)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(dto.NomeDoMotorista))
            throw new InvalidOperationException("O nome do motorista é obrigatório");

        if (string.IsNullOrWhiteSpace(dto.Cpf))
            throw new InvalidOperationException("O CPF é obrigatório");

        // Limpar CPF para validação
        var cpfLimpo = new string(dto.Cpf.Where(char.IsDigit).ToArray());
        if (cpfLimpo.Length != 11)
            throw new InvalidOperationException("CPF deve ter 11 dígitos");

        // Verificar CPF duplicado
        var cpfExiste = await _context.Motoristas
            .AnyAsync(m => m.Cpf.Replace(".", "").Replace("-", "") == cpfLimpo);
        if (cpfExiste)
            throw new InvalidOperationException("Já existe um motorista com este CPF");

        // Obter próximo código
        var ultimoCodigo = await _context.Motoristas.MaxAsync(m => (short?)m.CodigoDoMotorista) ?? 0;

        var motorista = new Motorista
        {
            CodigoDoMotorista = (short)(ultimoCodigo + 1),
            NomeDoMotorista = dto.NomeDoMotorista.Trim(),
            Rg = dto.Rg?.Trim() ?? "",
            Cpf = dto.Cpf.Trim(),
            Endereco = dto.Endereco?.Trim() ?? "",
            Numero = dto.Numero?.Trim() ?? "",
            Bairro = dto.Bairro?.Trim() ?? "",
            Municipio = dto.Municipio,
            Uf = dto.Uf?.Trim() ?? "",
            Cep = dto.Cep?.Trim() ?? "",
            Fone = dto.Fone?.Trim() ?? "",
            Cel = dto.Cel?.Trim() ?? ""
        };

        _context.Motoristas.Add(motorista);
        await _context.SaveChangesAsync();

        return (await BuscarPorIdAsync(motorista.CodigoDoMotorista))!;
    }

    public async Task<MotoristaDto> AtualizarAsync(short id, MotoristaUpdateDto dto)
    {
        var motorista = await _context.Motoristas
            .FirstOrDefaultAsync(m => m.CodigoDoMotorista == id);

        if (motorista == null)
            throw new KeyNotFoundException("Motorista não encontrado");

        // Validações
        if (string.IsNullOrWhiteSpace(dto.NomeDoMotorista))
            throw new InvalidOperationException("O nome do motorista é obrigatório");

        if (string.IsNullOrWhiteSpace(dto.Cpf))
            throw new InvalidOperationException("O CPF é obrigatório");

        // Verificar CPF duplicado
        var cpfLimpo = new string(dto.Cpf.Where(char.IsDigit).ToArray());
        var cpfExiste = await _context.Motoristas
            .AnyAsync(m => m.CodigoDoMotorista != id &&
                          m.Cpf.Replace(".", "").Replace("-", "") == cpfLimpo);
        if (cpfExiste)
            throw new InvalidOperationException("Já existe outro motorista com este CPF");

        motorista.NomeDoMotorista = dto.NomeDoMotorista.Trim();
        motorista.Rg = dto.Rg?.Trim() ?? "";
        motorista.Cpf = dto.Cpf.Trim();
        motorista.Endereco = dto.Endereco?.Trim() ?? "";
        motorista.Numero = dto.Numero?.Trim() ?? "";
        motorista.Bairro = dto.Bairro?.Trim() ?? "";
        motorista.Municipio = dto.Municipio;
        motorista.Uf = dto.Uf?.Trim() ?? "";
        motorista.Cep = dto.Cep?.Trim() ?? "";
        motorista.Fone = dto.Fone?.Trim() ?? "";
        motorista.Cel = dto.Cel?.Trim() ?? "";

        await _context.SaveChangesAsync();

        return (await BuscarPorIdAsync(id))!;
    }

    public async Task ExcluirAsync(short id)
    {
        var motorista = await _context.Motoristas
            .FirstOrDefaultAsync(m => m.CodigoDoMotorista == id);

        if (motorista == null)
            throw new KeyNotFoundException("Motorista não encontrado");

        // Verificar se tem viagens vinculadas
        var temViagens = await _context.Viagens
            .AnyAsync(v => v.MotoristaId == id);
        if (temViagens)
            throw new InvalidOperationException("Não é possível excluir: motorista possui viagens vinculadas");

        _context.Motoristas.Remove(motorista);
        await _context.SaveChangesAsync();
    }
}
