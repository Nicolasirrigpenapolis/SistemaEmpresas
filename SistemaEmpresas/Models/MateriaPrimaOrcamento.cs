using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Materia prima orçamento")]
public partial class MateriaPrimaOrcamento
{
    [Key]
    [Column("Sequencia da Expedição")]
    public int SequenciaDaExpedicao { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Sigla da Unidade")]
    [StringLength(15)]
    [Unicode(false)]
    public string SiglaDaUnidade { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Localizacao { get; set; } = null!;

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column(TypeName = "decimal(12, 3)")]
    public decimal Peso { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    public bool Imprimir { get; set; }
}
