using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaRevenda", "SequenciaDoItem", "IdDaConta")]
[Table("Municipios dos Revendedores")]
public partial class MunicipioDoRevendedore
{
    [Key]
    [Column("Sequencia da Revenda")]
    public int SequenciaDaRevenda { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Key]
    [Column("Id da Conta")]
    public int IdDaConta { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string Reg { get; set; } = null!;

    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    [Column("Seqüência do Município")]
    public int SequenciaDoMunicipio { get; set; }
}
