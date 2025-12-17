using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Motorista
{
    [Key]
    [Column("Codigo do Motorista")]
    public short CodigoDoMotorista { get; set; }

    [Column("Nome do Motorista")]
    [StringLength(30)]
    [Unicode(false)]
    public string NomeDoMotorista { get; set; } = null!;

    [Column("RG")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rg { get; set; } = null!;

    [Column("CPF")]
    [StringLength(14)]
    [Unicode(false)]
    public string Cpf { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [StringLength(9)]
    [Unicode(false)]
    public string Numero { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    public int Municipio { get; set; }

    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    [Column("CEP")]
    [StringLength(9)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [StringLength(13)]
    [Unicode(false)]
    public string Fone { get; set; } = null!;

    [Column("CEL")]
    [StringLength(14)]
    [Unicode(false)]
    public string Cel { get; set; } = null!;

    // Relacionamento com módulo de Transporte
    public virtual ICollection<Viagem> Viagens { get; set; } = new List<Viagem>();
}
