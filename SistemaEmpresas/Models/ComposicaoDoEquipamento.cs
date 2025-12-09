using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDoProjeto", "SequenciaDoItem")]
[Table("Composição do Equipamento")]
public partial class ComposicaoDoEquipamento
{
    [Key]
    [Column("Sequencia do Projeto")]
    public int SequenciaDoProjeto { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Modelo do Lance")]
    public int ModeloDoLance { get; set; }

    [Column("Tipo do Lance")]
    [StringLength(13)]
    [Unicode(false)]
    public string TipoDoLance { get; set; } = null!;

    [Column("Quant de Lance")]
    public short QuantDeLance { get; set; }
}
