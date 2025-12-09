using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Concilia Conta Antecipada")]
public partial class ConciliaContaAntecipadum
{
    [Key]
    [Column("Sequencia da Conciliação")]
    public int SequenciaDaConciliacao { get; set; }

    [Column("Seqüência da Manutenção")]
    public int SequenciaDaManutencao { get; set; }

    [Column("Sequencia da Compra")]
    public int SequenciaDaCompra { get; set; }

    [Column("Data da Conciliação", TypeName = "datetime")]
    public DateTime? DataDaConciliacao { get; set; }

    [Column("Notas da Compra")]
    [StringLength(100)]
    [Unicode(false)]
    public string NotasDaCompra { get; set; } = null!;

    public bool Conciliado { get; set; }
}
