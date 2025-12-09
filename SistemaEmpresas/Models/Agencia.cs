using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Agencia
{
    [Key]
    [Column("Seqüência da Agência")]
    public short SequenciaDaAgencia { get; set; }

    [Column("Número do Banco")]
    [StringLength(3)]
    [Unicode(false)]
    public string NumeroDoBanco { get; set; } = null!;

    [Column("Número da Agência")]
    [StringLength(6)]
    [Unicode(false)]
    public string NumeroDaAgencia { get; set; } = null!;

    [Column("Nome do Banco")]
    [StringLength(35)]
    [Unicode(false)]
    public string NomeDoBanco { get; set; } = null!;

    [Column("Nome da Agência")]
    [StringLength(20)]
    [Unicode(false)]
    public string NomeDaAgencia { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("CEP")]
    [StringLength(9)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    [StringLength(14)]
    [Unicode(false)]
    public string Telefone { get; set; } = null!;

    [Column("CNPJ")]
    [StringLength(18)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [Column("Não Calcular")]
    public bool NaoCalcular { get; set; }

    public bool? Ativa { get; set; }
}
