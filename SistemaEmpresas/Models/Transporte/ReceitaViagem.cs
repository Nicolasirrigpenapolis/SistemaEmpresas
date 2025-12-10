using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models.Transporte;

/// <summary>
/// Registro de receitas associadas a uma viagem.
/// Inclui fretes, bônus, reembolsos, etc.
/// Módulo de Gestão de Transporte - migrado do NewSistema.
/// </summary>
[Table("ReceitasViagem")]
[Index(nameof(ViagemId), Name = "IX_ReceitasViagem_ViagemId")]
[Index(nameof(DataReceita), Name = "IX_ReceitasViagem_DataReceita")]
public class ReceitaViagem
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Viagem à qual esta receita pertence
    /// </summary>
    [Required(ErrorMessage = "Viagem é obrigatória")]
    public int ViagemId { get; set; }

    /// <summary>
    /// Descrição da receita
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500)]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor da receita
    /// </summary>
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    /// <summary>
    /// Data em que a receita foi realizada/recebida
    /// </summary>
    [Required(ErrorMessage = "Data da receita é obrigatória")]
    public DateTime DataReceita { get; set; }

    /// <summary>
    /// Origem da receita (Frete, Bônus, Reembolso, etc.)
    /// </summary>
    [StringLength(100)]
    public string? Origem { get; set; }

    /// <summary>
    /// Número do documento associado (CT-e, NF, etc.)
    /// </summary>
    [StringLength(50)]
    public string? NumeroDocumento { get; set; }

    /// <summary>
    /// Cliente que pagou (nome ou razão social)
    /// </summary>
    [StringLength(200)]
    public string? Cliente { get; set; }

    /// <summary>
    /// Observações adicionais
    /// </summary>
    [StringLength(500)]
    public string? Observacoes { get; set; }

    /// <summary>
    /// Indica se a receita está ativa
    /// </summary>
    public bool Ativo { get; set; } = true;

    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataUltimaAlteracao { get; set; }

    // Relacionamento
    [ForeignKey("ViagemId")]
    public virtual Viagem Viagem { get; set; } = null!;
}
