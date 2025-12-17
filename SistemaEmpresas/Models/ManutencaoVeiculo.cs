using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Registro de manutenções realizadas em veículos da frota.
/// Inclui serviços como troca de óleo, revisões, reparos, etc.
/// Módulo de Manutenção - migrado do NewSistema.
/// </summary>
[Table("ManutencoesVeiculo")]
[Index(nameof(VeiculoId), Name = "IX_ManutencoesVeiculo_VeiculoId")]
[Index(nameof(FornecedorId), Name = "IX_ManutencoesVeiculo_FornecedorId")]
[Index(nameof(DataManutencao), Name = "IX_ManutencoesVeiculo_DataManutencao")]
public class ManutencaoVeiculo
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Veículo que recebeu a manutenção
    /// </summary>
    [Required(ErrorMessage = "Veículo é obrigatório")]
    public int VeiculoId { get; set; }

    /// <summary>
    /// Fornecedor/Oficina que realizou a manutenção
    /// Referência para tabela Geral onde Fornecedor = true
    /// Usa int porque a PK de Geral é int
    /// </summary>
    public int? FornecedorId { get; set; }

    /// <summary>
    /// Data em que a manutenção foi realizada
    /// </summary>
    [Required(ErrorMessage = "Data da manutenção é obrigatória")]
    public DateTime DataManutencao { get; set; }

    /// <summary>
    /// Tipo de manutenção (Preventiva, Corretiva, Revisão, etc.)
    /// </summary>
    [StringLength(100)]
    public string? TipoManutencao { get; set; }

    /// <summary>
    /// Descrição geral do serviço realizado
    /// </summary>
    [StringLength(500)]
    public string? DescricaoServico { get; set; }

    /// <summary>
    /// Quilometragem do veículo no momento da manutenção
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? KmAtual { get; set; }

    /// <summary>
    /// Valor da mão de obra
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorMaoObra { get; set; } = 0;

    /// <summary>
    /// Valor de serviços terceirizados
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorServicosTerceiros { get; set; } = 0;

    /// <summary>
    /// Número da Ordem de Serviço
    /// </summary>
    [StringLength(50)]
    public string? NumeroOS { get; set; }

    /// <summary>
    /// Número da Nota Fiscal do serviço
    /// </summary>
    [StringLength(50)]
    public string? NumeroNF { get; set; }

    /// <summary>
    /// Data prevista para próxima manutenção
    /// </summary>
    public DateTime? DataProximaManutencao { get; set; }

    /// <summary>
    /// Km prevista para próxima manutenção
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? KmProximaManutencao { get; set; }

    /// <summary>
    /// Observações adicionais
    /// </summary>
    [StringLength(1000)]
    public string? Observacoes { get; set; }

    /// <summary>
    /// Indica se a manutenção está ativa
    /// </summary>
    public bool Ativo { get; set; } = true;

    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataUltimaAlteracao { get; set; }

    // Propriedades calculadas
    /// <summary>
    /// Soma de todas as peças utilizadas
    /// </summary>
    [NotMapped]
    public decimal TotalPecas => Pecas?.Sum(p => p.ValorTotal) ?? 0;

    /// <summary>
    /// Custo total da manutenção (Peças + Mão de Obra + Terceiros)
    /// </summary>
    [NotMapped]
    public decimal CustoTotal => TotalPecas + ValorMaoObra + ValorServicosTerceiros;

    // Relacionamentos
    [ForeignKey("VeiculoId")]
    public virtual Veiculo Veiculo { get; set; } = null!;

    /// <summary>
    /// Fornecedor/Oficina - Referência à tabela Geral onde Fornecedor = true
    /// </summary>
    [ForeignKey("FornecedorId")]
    public virtual Geral? Fornecedor { get; set; }

    /// <summary>
    /// Peças utilizadas nesta manutenção
    /// </summary>
    public virtual ICollection<ManutencaoPeca> Pecas { get; set; } = new List<ManutencaoPeca>();
}
