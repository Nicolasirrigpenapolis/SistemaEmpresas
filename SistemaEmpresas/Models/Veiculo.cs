using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

/// <summary>
/// Cadastro de veículos da frota.
/// Módulo de Gestão de Transporte - migrado do NewSistema.
/// </summary>
[Table("Veiculos")]
[Index(nameof(Placa), IsUnique = true, Name = "IX_Veiculos_Placa")]
public class Veiculo
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Placa do veículo (formato antigo ou Mercosul)
    /// </summary>
    [Required(ErrorMessage = "Placa é obrigatória")]
    [StringLength(8)]
    public string Placa { get; set; } = string.Empty;

    /// <summary>
    /// Marca do veículo (ex: Volvo, Scania, Mercedes)
    /// </summary>
    [StringLength(100)]
    public string? Marca { get; set; }

    /// <summary>
    /// Modelo do veículo
    /// </summary>
    [StringLength(100)]
    public string? Modelo { get; set; }

    /// <summary>
    /// Ano de fabricação
    /// </summary>
    public int? AnoFabricacao { get; set; }

    /// <summary>
    /// Ano do modelo
    /// </summary>
    public int? AnoModelo { get; set; }

    /// <summary>
    /// Tara do veículo em kg (peso vazio)
    /// </summary>
    [Required(ErrorMessage = "Tara é obrigatória")]
    public int? Tara { get; set; }

    /// <summary>
    /// Capacidade máxima de carga em kg
    /// </summary>
    public int? CapacidadeKg { get; set; }

    /// <summary>
    /// Tipo de rodado: Truck, Toco, Cavalo Mecânico, VAN, Utilitário, Outros
    /// </summary>
    [Required(ErrorMessage = "Tipo de rodado é obrigatório")]
    [StringLength(50)]
    public string TipoRodado { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de carroceria: Aberta, Fechada/Baú, Graneleiro, Porta Container, Sider, Outros
    /// </summary>
    [Required(ErrorMessage = "Tipo de carroceria é obrigatório")]
    [StringLength(50)]
    public string TipoCarroceria { get; set; } = string.Empty;

    /// <summary>
    /// UF de licenciamento do veículo
    /// </summary>
    [Required(ErrorMessage = "UF é obrigatória")]
    [StringLength(2)]
    public string Uf { get; set; } = string.Empty;

    /// <summary>
    /// RENAVAM do veículo
    /// </summary>
    [StringLength(20)]
    public string? Renavam { get; set; }

    /// <summary>
    /// Chassi do veículo
    /// </summary>
    [StringLength(30)]
    public string? Chassi { get; set; }

    /// <summary>
    /// Cor predominante
    /// </summary>
    [StringLength(30)]
    public string? Cor { get; set; }

    /// <summary>
    /// Tipo de combustível: Diesel, Gasolina, Etanol, Flex, GNV, Elétrico
    /// </summary>
    [StringLength(30)]
    public string? TipoCombustivel { get; set; }

    /// <summary>
    /// RNTRC - Registro Nacional de Transportadores Rodoviários de Cargas
    /// </summary>
    [StringLength(20)]
    public string? Rntrc { get; set; }

    /// <summary>
    /// Indica se o veículo está ativo
    /// </summary>
    public bool Ativo { get; set; } = true;

    /// <summary>
    /// Observações gerais
    /// </summary>
    [StringLength(1000)]
    public string? Observacoes { get; set; }

    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataUltimaAlteracao { get; set; }

    // Propriedades computadas
    [NotMapped]
    public int? TaraKg => Tara;

    [NotMapped]
    public string UfPlaca => Uf;

    // Relacionamentos
    public virtual ICollection<Viagem> Viagens { get; set; } = new List<Viagem>();
    public virtual ICollection<ManutencaoVeiculo> Manutencoes { get; set; } = new List<ManutencaoVeiculo>();
}
