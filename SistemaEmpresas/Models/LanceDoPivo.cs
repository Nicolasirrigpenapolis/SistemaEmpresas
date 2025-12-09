using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("ModeloDoLance", "DescricaoDoLance")]
[Table("Lances do Pivo")]
public partial class LanceDoPivo
{
    [Key]
    [Column("Modelo do Lance")]
    public int ModeloDoLance { get; set; }

    [Column("Largura do Lance", TypeName = "decimal(8, 2)")]
    public decimal LarguraDoLance { get; set; }

    [Column("Diametro do Lance", TypeName = "decimal(8, 2)")]
    public decimal DiametroDoLance { get; set; }

    [Column("Qtde de Spray")]
    public short QtdeDeSpray { get; set; }

    [Key]
    [Column("Descrição do Lance")]
    [StringLength(120)]
    [Unicode(false)]
    public string DescricaoDoLance { get; set; } = null!;

    public bool Inicial { get; set; }

    public bool Inter { get; set; }

    public bool Penultimo { get; set; }

    public bool Final { get; set; }

    [Column("CA1")]
    public short Ca1 { get; set; }

    [Column("CA2")]
    public short Ca2 { get; set; }

    [Column("CA3")]
    public short Ca3 { get; set; }

    [Column("CA4")]
    public short Ca4 { get; set; }

    [Column("CA5")]
    public short Ca5 { get; set; }

    [Column("CA6")]
    public short Ca6 { get; set; }

    [Column("CA7")]
    public short Ca7 { get; set; }

    [Column("CA8")]
    public short Ca8 { get; set; }
}
