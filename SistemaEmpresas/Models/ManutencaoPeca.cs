using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Registro de peças utilizadas em manutenções de veículos.
/// Módulo de Manutenção - migrado do NewSistema.
/// </summary>
[Table("ManutencoesPeca")]
[Index(nameof(ManutencaoId), Name = "IX_ManutencoesPeca_ManutencaoId")]
public class ManutencaoPeca
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Manutenção à qual esta peça pertence
    /// </summary>
    [Required(ErrorMessage = "Manutenção é obrigatória")]
    public int ManutencaoId { get; set; }

    /// <summary>
    /// Descrição da peça
    /// </summary>
    [Required(ErrorMessage = "Descrição da peça é obrigatória")]
    [StringLength(200)]
    public string DescricaoPeca { get; set; } = string.Empty;

    /// <summary>
    /// Código da peça (referência do fabricante)
    /// </summary>
    [StringLength(50)]
    public string? CodigoPeca { get; set; }

    /// <summary>
    /// Marca/Fabricante da peça
    /// </summary>
    [StringLength(100)]
    public string? Marca { get; set; }

    /// <summary>
    /// Quantidade de peças utilizadas
    /// </summary>
    [Required(ErrorMessage = "Quantidade é obrigatória")]
    [Column(TypeName = "decimal(18,4)")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public decimal Quantidade { get; set; } = 1;

    /// <summary>
    /// Unidade de medida (UN, PC, KG, LT, etc.)
    /// </summary>
    [StringLength(10)]
    public string Unidade { get; set; } = "UN";

    /// <summary>
    /// Valor unitário da peça
    /// </summary>
    [Required(ErrorMessage = "Valor unitário é obrigatório")]
    [Column(TypeName = "decimal(18,4)")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor unitário não pode ser negativo")]
    public decimal ValorUnitario { get; set; }

    /// <summary>
    /// Observações sobre a peça
    /// </summary>
    [StringLength(500)]
    public string? Observacoes { get; set; }

    /// <summary>
    /// Indica se o registro está ativo
    /// </summary>
    public bool Ativo { get; set; } = true;

    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataUltimaAlteracao { get; set; }

    // Propriedade calculada
    /// <summary>
    /// Valor total da peça (Quantidade x ValorUnitario)
    /// </summary>
    [NotMapped]
    public decimal ValorTotal => Quantidade * ValorUnitario;

    // Relacionamento
    [ForeignKey("ManutencaoId")]
    public virtual ManutencaoVeiculo Manutencao { get; set; } = null!;
}
