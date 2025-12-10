using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models.Transporte;

/// <summary>
/// Registro de despesas associadas a uma viagem.
/// Inclui combustível, pedágio, alimentação, hospedagem, etc.
/// Módulo de Gestão de Transporte - migrado do NewSistema.
/// </summary>
[Table("DespesasViagem")]
[Index(nameof(ViagemId), Name = "IX_DespesasViagem_ViagemId")]
[Index(nameof(DataDespesa), Name = "IX_DespesasViagem_DataDespesa")]
[Index(nameof(TipoDespesa), Name = "IX_DespesasViagem_TipoDespesa")]
public class DespesaViagem
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Viagem à qual esta despesa pertence
    /// </summary>
    [Required(ErrorMessage = "Viagem é obrigatória")]
    public int ViagemId { get; set; }

    /// <summary>
    /// Tipo de despesa (Combustível, Pedágio, Alimentação, etc.)
    /// </summary>
    [Required(ErrorMessage = "Tipo de despesa é obrigatório")]
    [StringLength(100)]
    public string TipoDespesa { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada da despesa
    /// </summary>
    [StringLength(500)]
    public string? Descricao { get; set; }

    /// <summary>
    /// Valor da despesa
    /// </summary>
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    /// <summary>
    /// Data em que a despesa foi realizada
    /// </summary>
    [Required(ErrorMessage = "Data da despesa é obrigatória")]
    public DateTime DataDespesa { get; set; }

    /// <summary>
    /// Número do documento fiscal (cupom, nota, etc.)
    /// </summary>
    [StringLength(50)]
    public string? NumeroDocumento { get; set; }

    /// <summary>
    /// Local onde a despesa foi realizada
    /// </summary>
    [StringLength(200)]
    public string? Local { get; set; }

    /// <summary>
    /// Quilometragem no momento da despesa
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? KmAtual { get; set; }

    /// <summary>
    /// Para despesas de combustível: litros abastecidos
    /// </summary>
    [Column(TypeName = "decimal(18,3)")]
    public decimal? Litros { get; set; }

    /// <summary>
    /// Observações adicionais
    /// </summary>
    [StringLength(500)]
    public string? Observacoes { get; set; }

    /// <summary>
    /// Indica se a despesa está ativa
    /// </summary>
    public bool Ativo { get; set; } = true;

    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataUltimaAlteracao { get; set; }

    // Propriedade calculada para despesas de combustível
    [NotMapped]
    public decimal? PrecoPorLitro => Litros.HasValue && Litros.Value > 0
        ? Valor / Litros.Value
        : null;

    // Relacionamento
    [ForeignKey("ViagemId")]
    public virtual Viagem Viagem { get; set; } = null!;
}
