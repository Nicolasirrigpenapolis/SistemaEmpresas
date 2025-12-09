using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Alteracao Baixa Contas")]
public partial class AlteracaoBaixaConta
{
    [Column("Seq do Spy")]
    public int SeqDoSpy { get; set; }

    [Column("Seq da Baixa")]
    public int SeqDaBaixa { get; set; }

    [Column("Usu Alteracao")]
    [StringLength(20)]
    [Unicode(false)]
    public string UsuAlteracao { get; set; } = null!;

    [Column("Dt Modificacao", TypeName = "datetime")]
    public DateTime? DtModificacao { get; set; }

    public long Manutencao { get; set; }

    [Column("Dta Baixa", TypeName = "datetime")]
    public DateTime? DtaBaixa { get; set; }

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

    [Column("Bx do Cliente", TypeName = "datetime")]
    public DateTime? BxDoCliente { get; set; }

    [Column("Quem Pagou")]
    [StringLength(20)]
    [Unicode(false)]
    public string QuemPagou { get; set; } = null!;

    [Column("Vr do Cliente", TypeName = "decimal(10, 2)")]
    public decimal VrDoCliente { get; set; }

    [Column("Seq da Agencia")]
    public short SeqDaAgencia { get; set; }

    [Column("Seq Acc da Agencia")]
    public short SeqAccDaAgencia { get; set; }
}
