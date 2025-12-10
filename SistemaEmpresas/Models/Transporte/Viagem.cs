using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models.Transporte;

/// <summary>
/// Registro de viagens realizadas pelos veículos da frota.
/// Módulo de Gestão de Transporte - migrado do NewSistema.
/// </summary>
[Table("Viagens")]
[Index(nameof(VeiculoId), Name = "IX_Viagens_VeiculoId")]
[Index(nameof(MotoristaId), Name = "IX_Viagens_MotoristaId")]
[Index(nameof(DataInicio), Name = "IX_Viagens_DataInicio")]
public class Viagem
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Veículo utilizado na viagem
    /// </summary>
    [Required(ErrorMessage = "Veículo é obrigatório")]
    public int VeiculoId { get; set; }

    /// <summary>
    /// Motorista/Condutor da viagem (FK para tabela Motorista existente)
    /// Usa short porque a PK de Motorista é short
    /// </summary>
    public short? MotoristaId { get; set; }

    /// <summary>
    /// Reboque utilizado (opcional)
    /// </summary>
    public int? ReboqueId { get; set; }

    /// <summary>
    /// Data e hora de início da viagem
    /// </summary>
    [Required(ErrorMessage = "Data de início é obrigatória")]
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data e hora de fim da viagem
    /// </summary>
    [Required(ErrorMessage = "Data de fim é obrigatória")]
    public DateTime DataFim { get; set; }

    /// <summary>
    /// Quilometragem inicial do veículo
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? KmInicial { get; set; }

    /// <summary>
    /// Quilometragem final do veículo
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? KmFinal { get; set; }

    /// <summary>
    /// Local de origem da viagem
    /// </summary>
    [StringLength(200)]
    public string? Origem { get; set; }

    /// <summary>
    /// Local de destino da viagem
    /// </summary>
    [StringLength(200)]
    public string? Destino { get; set; }

    /// <summary>
    /// Descrição da carga transportada
    /// </summary>
    [StringLength(500)]
    public string? DescricaoCarga { get; set; }

    /// <summary>
    /// Peso da carga em kg
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? PesoCarga { get; set; }

    /// <summary>
    /// Observações gerais da viagem
    /// </summary>
    [StringLength(1000)]
    public string? Observacoes { get; set; }

    /// <summary>
    /// Indica se a viagem está ativa
    /// </summary>
    public bool Ativo { get; set; } = true;

    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataUltimaAlteracao { get; set; }

    // Propriedades calculadas (BACKEND calcula, não aceita do frontend!)
    /// <summary>
    /// Soma de todas as receitas da viagem
    /// </summary>
    [NotMapped]
    public decimal ReceitaTotal => Receitas?.Sum(r => r.Valor) ?? 0;

    /// <summary>
    /// Soma de todas as despesas da viagem
    /// </summary>
    [NotMapped]
    public decimal TotalDespesas => Despesas?.Sum(d => d.Valor) ?? 0;

    /// <summary>
    /// Receita - Despesas
    /// </summary>
    [NotMapped]
    public decimal SaldoLiquido => ReceitaTotal - TotalDespesas;

    /// <summary>
    /// Quantidade de dias da viagem
    /// </summary>
    [NotMapped]
    public int DuracaoDias => (DataFim - DataInicio).Days + 1;

    /// <summary>
    /// Km final - Km inicial
    /// </summary>
    [NotMapped]
    public decimal? KmPercorrido => KmFinal.HasValue && KmInicial.HasValue
        ? KmFinal.Value - KmInicial.Value
        : null;

    /// <summary>
    /// Custo por km (Total Despesas / Km Percorrido)
    /// </summary>
    [NotMapped]
    public decimal? CustoPorKm => KmPercorrido.HasValue && KmPercorrido.Value > 0
        ? TotalDespesas / KmPercorrido.Value
        : null;

    // Relacionamentos
    [ForeignKey("VeiculoId")]
    public virtual Veiculo Veiculo { get; set; } = null!;

    /// <summary>
    /// Relacionamento com tabela Motorista existente no sistema
    /// </summary>
    [ForeignKey("MotoristaId")]
    public virtual Motorista? Motorista { get; set; }

    [ForeignKey("ReboqueId")]
    public virtual Reboque? Reboque { get; set; }

    /// <summary>
    /// Despesas desta viagem (combustível, pedágio, etc.)
    /// </summary>
    public virtual ICollection<DespesaViagem> Despesas { get; set; } = new List<DespesaViagem>();

    /// <summary>
    /// Receitas desta viagem (fretes, etc.)
    /// </summary>
    public virtual ICollection<ReceitaViagem> Receitas { get; set; } = new List<ReceitaViagem>();
}
