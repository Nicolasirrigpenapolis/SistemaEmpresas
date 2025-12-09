using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Spy Baixa Contas")]
public partial class SpyBaixaConta
{
    [Key]
    [Column("Seq do Spy")]
    public int SeqDoSpy { get; set; }

    [Column("Seq da Baixa")]
    public int SeqDaBaixa { get; set; }

    [Column("Dt Inclusão", TypeName = "datetime")]
    public DateTime DtInclusao { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Usuario { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string TpConta { get; set; } = null!;

    public long Manutencao { get; set; }

    [Column("Dt Baixa", TypeName = "datetime")]
    public DateTime? DtBaixa { get; set; }

    [Column(TypeName = "decimal(11, 2)")]
    public decimal Juros { get; set; }

    [Column(TypeName = "decimal(11, 2)")]
    public decimal Desconto { get; set; }

    [Column("Vr Pago", TypeName = "decimal(11, 2)")]
    public decimal VrPago { get; set; }

    [Column("Tp Carteira")]
    [StringLength(20)]
    [Unicode(false)]
    public string TpCarteira { get; set; } = null!;

    [Column("Bx Cliente", TypeName = "datetime")]
    public DateTime? BxCliente { get; set; }

    [Column("Quem Pagou")]
    [StringLength(20)]
    [Unicode(false)]
    public string QuemPagou { get; set; } = null!;

    [Column("Vr Cliente", TypeName = "decimal(10, 2)")]
    public decimal VrCliente { get; set; }

    [Column("Seq Banco")]
    public short SeqBanco { get; set; }

    [Column("Seq Acc Banco")]
    public short SeqAccBanco { get; set; }
}
