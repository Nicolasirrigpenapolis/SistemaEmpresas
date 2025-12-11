using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEmpresas.DTOs;

// ==========================================
// VEÍCULO DTOs
// ==========================================

/// <summary>
/// DTO para listagem de veículos
/// </summary>
public class VeiculoListDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? AnoModelo { get; set; }
    public int? Tara { get; set; }
    public string TipoRodado { get; set; } = string.Empty;
    public string TipoCarroceria { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

/// <summary>
/// DTO completo do veículo
/// </summary>
public class VeiculoDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? AnoFabricacao { get; set; }
    public int? AnoModelo { get; set; }
    public int? Tara { get; set; }
    public int? CapacidadeKg { get; set; }
    public string TipoRodado { get; set; } = string.Empty;
    public string TipoCarroceria { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string? Renavam { get; set; }
    public string? Chassi { get; set; }
    public string? Cor { get; set; }
    public string? TipoCombustivel { get; set; }
    public string? Rntrc { get; set; }
    public bool Ativo { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
}

/// <summary>
/// DTO para criação/atualização de veículo
/// </summary>
public class VeiculoCreateUpdateDto
{
    [Required(ErrorMessage = "Placa é obrigatória")]
    [StringLength(8)]
    public string Placa { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Marca { get; set; }

    [StringLength(100)]
    public string? Modelo { get; set; }

    public int? AnoFabricacao { get; set; }
    public int? AnoModelo { get; set; }

    [Required(ErrorMessage = "Tara é obrigatória")]
    public int Tara { get; set; }

    public int? CapacidadeKg { get; set; }

    [Required(ErrorMessage = "Tipo de rodado é obrigatório")]
    [StringLength(50)]
    public string TipoRodado { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo de carroceria é obrigatório")]
    [StringLength(50)]
    public string TipoCarroceria { get; set; } = string.Empty;

    [Required(ErrorMessage = "UF é obrigatória")]
    [StringLength(2)]
    public string Uf { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Renavam { get; set; }

    [StringLength(30)]
    public string? Chassi { get; set; }

    [StringLength(30)]
    public string? Cor { get; set; }

    [StringLength(30)]
    public string? TipoCombustivel { get; set; }

    [StringLength(20)]
    public string? Rntrc { get; set; }

    public bool Ativo { get; set; } = true;

    [StringLength(1000)]
    public string? Observacoes { get; set; }
}

/// <summary>
/// Filtros para listagem de veículos
/// </summary>
public class VeiculoFiltros
{
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 25;
    public string? Busca { get; set; }
    public string? TipoVeiculo { get; set; }
    public bool? IncluirInativos { get; set; }
}

// ==========================================
// REBOQUE DTOs
// ==========================================

/// <summary>
/// DTO para listagem de reboques
/// </summary>
public class ReboqueListDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int Tara { get; set; }
    public string TipoCarroceria { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

/// <summary>
/// DTO completo do reboque
/// </summary>
public class ReboqueDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? AnoFabricacao { get; set; }
    public int Tara { get; set; }
    public int? CapacidadeKg { get; set; }
    public string TipoCarroceria { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string? Renavam { get; set; }
    public string? Chassi { get; set; }
    public string? Rntrc { get; set; }
    public bool Ativo { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
}

/// <summary>
/// DTO para criação/atualização de reboque
/// </summary>
public class ReboqueCreateUpdateDto
{
    [Required(ErrorMessage = "Placa é obrigatória")]
    [StringLength(8)]
    public string Placa { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Marca { get; set; }

    [StringLength(100)]
    public string? Modelo { get; set; }

    public int? AnoFabricacao { get; set; }

    [Required(ErrorMessage = "Tara é obrigatória")]
    public int Tara { get; set; }

    public int? CapacidadeKg { get; set; }

    [Required(ErrorMessage = "Tipo de carroceria é obrigatório")]
    [StringLength(50)]
    public string TipoCarroceria { get; set; } = string.Empty;

    [Required(ErrorMessage = "UF é obrigatória")]
    [StringLength(2)]
    public string Uf { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Renavam { get; set; }

    [StringLength(30)]
    public string? Chassi { get; set; }

    [StringLength(20)]
    public string? Rntrc { get; set; }

    public bool Ativo { get; set; } = true;

    [StringLength(1000)]
    public string? Observacoes { get; set; }
}

// ==========================================
// VIAGEM DTOs
// ==========================================

/// <summary>
/// DTO para listagem de viagens
/// </summary>
public class ViagemListDto
{
    public int Id { get; set; }
    public string VeiculoPlaca { get; set; } = string.Empty;
    public string? MotoristaNome { get; set; }
    public string? ReboquePlaca { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string? Origem { get; set; }
    public string? Destino { get; set; }
    public decimal? KmPercorrido { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal ReceitaTotal { get; set; }
    public decimal SaldoLiquido { get; set; }
    public bool Ativo { get; set; }
}

/// <summary>
/// DTO completo da viagem
/// </summary>
public class ViagemDto
{
    public int Id { get; set; }
    public int VeiculoId { get; set; }
    public string VeiculoPlaca { get; set; } = string.Empty;
    public short? MotoristaId { get; set; }
    public string? MotoristaNome { get; set; }
    public int? ReboqueId { get; set; }
    public string? ReboquePlaca { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public decimal? KmInicial { get; set; }
    public decimal? KmFinal { get; set; }
    public string? Origem { get; set; }
    public string? Destino { get; set; }
    public string? DescricaoCarga { get; set; }
    public decimal? PesoCarga { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }

    // Calculados
    public decimal? KmPercorrido { get; set; }
    public int DuracaoDias { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal ReceitaTotal { get; set; }
    public decimal SaldoLiquido { get; set; }
    public decimal? CustoPorKm { get; set; }

    // Detalhes
    public List<DespesaViagemDto> Despesas { get; set; } = new();
    public List<ReceitaViagemDto> Receitas { get; set; } = new();
}

/// <summary>
/// DTO para criação/atualização de viagem
/// </summary>
public class ViagemCreateUpdateDto
{
    [Required(ErrorMessage = "Veículo é obrigatório")]
    public int VeiculoId { get; set; }

    public short? MotoristaId { get; set; }
    public int? ReboqueId { get; set; }

    [Required(ErrorMessage = "Data de início é obrigatória")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "Data de fim é obrigatória")]
    public DateTime DataFim { get; set; }

    public decimal? KmInicial { get; set; }
    public decimal? KmFinal { get; set; }

    [StringLength(200)]
    public string? Origem { get; set; }

    [StringLength(200)]
    public string? Destino { get; set; }

    [StringLength(500)]
    public string? DescricaoCarga { get; set; }

    public decimal? PesoCarga { get; set; }

    [StringLength(1000)]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}

// ==========================================
// DESPESA VIAGEM DTOs
// ==========================================

/// <summary>
/// DTO completo da despesa de viagem
/// </summary>
public class DespesaViagemDto
{
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public string TipoDespesa { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataDespesa { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? Local { get; set; }
    public decimal? KmAtual { get; set; }
    public decimal? Litros { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }

    // Calculado
    public decimal? PrecoPorLitro { get; set; }
}

/// <summary>
/// DTO para criação/atualização de despesa de viagem
/// </summary>
public class DespesaViagemCreateUpdateDto
{
    [Required(ErrorMessage = "Viagem é obrigatória")]
    public int ViagemId { get; set; }

    [Required(ErrorMessage = "Tipo de despesa é obrigatório")]
    [StringLength(100)]
    public string TipoDespesa { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Data da despesa é obrigatória")]
    public DateTime DataDespesa { get; set; }

    [StringLength(50)]
    public string? NumeroDocumento { get; set; }

    [StringLength(200)]
    public string? Local { get; set; }

    public decimal? KmAtual { get; set; }
    public decimal? Litros { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}

// ==========================================
// RECEITA VIAGEM DTOs
// ==========================================

/// <summary>
/// DTO completo da receita de viagem
/// </summary>
public class ReceitaViagemDto
{
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataReceita { get; set; }
    public string? Origem { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? Cliente { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
}

/// <summary>
/// DTO para criação/atualização de receita de viagem
/// </summary>
public class ReceitaViagemCreateUpdateDto
{
    [Required(ErrorMessage = "Viagem é obrigatória")]
    public int ViagemId { get; set; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Data da receita é obrigatória")]
    public DateTime DataReceita { get; set; }

    [StringLength(100)]
    public string? Origem { get; set; }

    [StringLength(50)]
    public string? NumeroDocumento { get; set; }

    [StringLength(200)]
    public string? Cliente { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}

// ==========================================
// MANUTENÇÃO VEÍCULO DTOs
// ==========================================

/// <summary>
/// DTO para listagem de manutenções
/// </summary>
public class ManutencaoVeiculoListDto
{
    public int Id { get; set; }
    public string VeiculoPlaca { get; set; } = string.Empty;
    public string? FornecedorNome { get; set; }
    public DateTime DataManutencao { get; set; }
    public string? TipoManutencao { get; set; }
    public decimal? KmAtual { get; set; }
    public decimal CustoTotal { get; set; }
    public bool Ativo { get; set; }
}

/// <summary>
/// DTO completo da manutenção
/// </summary>
public class ManutencaoVeiculoDto
{
    public int Id { get; set; }
    public int VeiculoId { get; set; }
    public string VeiculoPlaca { get; set; } = string.Empty;
    public int? FornecedorId { get; set; }
    public string? FornecedorNome { get; set; }
    public DateTime DataManutencao { get; set; }
    public string? TipoManutencao { get; set; }
    public string? DescricaoServico { get; set; }
    public decimal? KmAtual { get; set; }
    public decimal ValorMaoObra { get; set; }
    public decimal ValorServicosTerceiros { get; set; }
    public string? NumeroOS { get; set; }
    public string? NumeroNF { get; set; }
    public DateTime? DataProximaManutencao { get; set; }
    public decimal? KmProximaManutencao { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }

    // Calculados
    public decimal TotalPecas { get; set; }
    public decimal CustoTotal { get; set; }

    // Detalhes
    public List<ManutencaoPecaDto> Pecas { get; set; } = new();
}

/// <summary>
/// DTO para criação/atualização de manutenção
/// </summary>
public class ManutencaoVeiculoCreateUpdateDto
{
    [Required(ErrorMessage = "Veículo é obrigatório")]
    public int VeiculoId { get; set; }

    public int? FornecedorId { get; set; }

    [Required(ErrorMessage = "Data da manutenção é obrigatória")]
    public DateTime DataManutencao { get; set; }

    [StringLength(100)]
    public string? TipoManutencao { get; set; }

    [StringLength(500)]
    public string? DescricaoServico { get; set; }

    public decimal? KmAtual { get; set; }

    public decimal ValorMaoObra { get; set; } = 0;
    public decimal ValorServicosTerceiros { get; set; } = 0;

    [StringLength(50)]
    public string? NumeroOS { get; set; }

    [StringLength(50)]
    public string? NumeroNF { get; set; }

    public DateTime? DataProximaManutencao { get; set; }
    public decimal? KmProximaManutencao { get; set; }

    [StringLength(1000)]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}

/// <summary>
/// Filtros para listagem de manutenções
/// </summary>
public class ManutencaoFiltros
{
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 25;
    public string? Busca { get; set; }
    public int? VeiculoId { get; set; }
    public string? TipoManutencao { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool? IncluirInativos { get; set; }
}

// ==========================================
// MANUTENÇÃO PEÇA DTOs
// ==========================================

/// <summary>
/// DTO completo da peça de manutenção
/// </summary>
public class ManutencaoPecaDto
{
    public int Id { get; set; }
    public int ManutencaoId { get; set; }
    public string DescricaoPeca { get; set; } = string.Empty;
    public string? CodigoPeca { get; set; }
    public string? Marca { get; set; }
    public decimal Quantidade { get; set; }
    public string Unidade { get; set; } = "UN";
    public decimal ValorUnitario { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }

    // Calculado
    public decimal ValorTotal { get; set; }
}

/// <summary>
/// DTO para criação/atualização de peça de manutenção
/// </summary>
public class ManutencaoPecaCreateUpdateDto
{
    [Required(ErrorMessage = "Manutenção é obrigatória")]
    public int ManutencaoId { get; set; }

    [Required(ErrorMessage = "Descrição da peça é obrigatória")]
    [StringLength(200)]
    public string DescricaoPeca { get; set; } = string.Empty;

    [StringLength(50)]
    public string? CodigoPeca { get; set; }

    [StringLength(100)]
    public string? Marca { get; set; }

    [Required(ErrorMessage = "Quantidade é obrigatória")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public decimal Quantidade { get; set; } = 1;

    [StringLength(10)]
    public string Unidade { get; set; } = "UN";

    [Required(ErrorMessage = "Valor unitário é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor unitário não pode ser negativo")]
    public decimal ValorUnitario { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}

// ==========================================
// DTOs AUXILIARES
// ==========================================

/// <summary>
/// DTO resumido do motorista para selects
/// </summary>
public class MotoristaSelectDto
{
    public short CodigoDoMotorista { get; set; }
    public string NomeDoMotorista { get; set; } = string.Empty;
    public string? Cpf { get; set; }
}

/// <summary>
/// DTO resumido do fornecedor para selects
/// </summary>
public class FornecedorSelectDto
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? CnpjCpf { get; set; }
}
