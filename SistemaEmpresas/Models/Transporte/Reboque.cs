using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models.Transporte;

/// <summary>
/// Cadastro de reboques/carretas.
/// Módulo de Gestão de Transporte - migrado do NewSistema.
/// </summary>
[Table("Reboques")]
[Index(nameof(Placa), IsUnique = true, Name = "IX_Reboques_Placa")]
public class Reboque
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Placa do reboque (formato antigo ou Mercosul)
    /// </summary>
    [Required(ErrorMessage = "Placa é obrigatória")]
    [StringLength(8)]
    public string Placa { get; set; } = string.Empty;

    /// <summary>
    /// Marca do reboque (ex: Randon, Facchini, Guerra)
    /// </summary>
    [StringLength(100)]
    public string? Marca { get; set; }

    /// <summary>
    /// Modelo do reboque
    /// </summary>
    [StringLength(100)]
    public string? Modelo { get; set; }

    /// <summary>
    /// Ano de fabricação
    /// </summary>
    public int? AnoFabricacao { get; set; }

    /// <summary>
    /// Tara do reboque em kg (peso vazio)
    /// </summary>
    public int Tara { get; set; }

    /// <summary>
    /// Capacidade máxima de carga em kg
    /// </summary>
    public int? CapacidadeKg { get; set; }

    /// <summary>
    /// Tipo de rodado: Eixo Simples, Eixo Tandem, Rodagem Dupla, Outros
    /// </summary>
    [Required(ErrorMessage = "Tipo de rodado é obrigatório")]
    [StringLength(50)]
    public string TipoRodado { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de carroceria: Aberta, Fechada/Baú, Graneleiro, Tanque, Sider, Outros
    /// </summary>
    [Required(ErrorMessage = "Tipo de carroceria é obrigatório")]
    [StringLength(50)]
    public string TipoCarroceria { get; set; } = string.Empty;

    /// <summary>
    /// UF de licenciamento do reboque
    /// </summary>
    [Required(ErrorMessage = "UF é obrigatória")]
    [StringLength(2)]
    public string Uf { get; set; } = string.Empty;

    /// <summary>
    /// RNTRC - Registro Nacional de Transportadores Rodoviários de Cargas
    /// </summary>
    [StringLength(20)]
    public string? Rntrc { get; set; }

    /// <summary>
    /// RENAVAM do reboque
    /// </summary>
    [StringLength(20)]
    public string? Renavam { get; set; }

    /// <summary>
    /// Chassi do reboque
    /// </summary>
    [StringLength(30)]
    public string? Chassi { get; set; }

    /// <summary>
    /// Indica se o reboque está ativo
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
    public string UfPlaca => Uf;

    // Relacionamentos
    public virtual ICollection<Viagem> Viagens { get; set; } = new List<Viagem>();
}
