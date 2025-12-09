using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Parametros do SPED ECF")]
public partial class ParametroDoSpedEcf
{
    [Column("Versao sped")]
    [StringLength(3)]
    [Unicode(false)]
    public string VersaoSped { get; set; } = null!;

    [Column("Nome do Contabilista")]
    [StringLength(40)]
    [Unicode(false)]
    public string NomeDoContabilista { get; set; } = null!;

    [Column("CPF Contabilista")]
    [StringLength(14)]
    [Unicode(false)]
    public string CpfContabilista { get; set; } = null!;

    [Column("CRC")]
    [StringLength(20)]
    [Unicode(false)]
    public string Crc { get; set; } = null!;

    [Column("CEP")]
    [StringLength(9)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [Column("CNPJ")]
    [StringLength(18)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [Column("Complemento do Endereço")]
    [StringLength(15)]
    [Unicode(false)]
    public string ComplementoDoEndereco { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [StringLength(13)]
    [Unicode(false)]
    public string Fone { get; set; } = null!;

    [StringLength(40)]
    [Unicode(false)]
    public string Empresa { get; set; } = null!;
}
