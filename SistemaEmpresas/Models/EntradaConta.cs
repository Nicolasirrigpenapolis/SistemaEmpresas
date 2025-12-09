using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Entrada Contas")]
public partial class EntradaConta
{
    [Key]
    [Column("Seqüência da Entrada")]
    public int SequenciaDaEntrada { get; set; }

    [Column("Data de Entrada", TypeName = "datetime")]
    public DateTime? DataDeEntrada { get; set; }

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Número da Nota Fiscal")]
    public int NumeroDaNotaFiscal { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [Column("Número da Duplicata")]
    public int NumeroDaDuplicata { get; set; }

    [Column(TypeName = "text")]
    public string Historico { get; set; } = null!;

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column("Valor Total", TypeName = "decimal(12, 4)")]
    public decimal ValorTotal { get; set; }

    [Column("Tipo da Conta")]
    [StringLength(11)]
    [Unicode(false)]
    public string TipoDaConta { get; set; } = null!;

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Conta { get; set; } = null!;

    [Column("Data da Alteração", TypeName = "datetime")]
    public DateTime? DataDaAlteracao { get; set; }

    [Column("Hora da Alteração", TypeName = "datetime")]
    public DateTime? HoraDaAlteracao { get; set; }

    [Column("Usuário da Alteração")]
    [StringLength(60)]
    [Unicode(false)]
    public string UsuarioDaAlteracao { get; set; } = null!;

    public bool Previsao { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    [Column("Sequencia da Compra")]
    public int SequenciaDaCompra { get; set; }

    [Column("Codigo do Debito")]
    public int CodigoDoDebito { get; set; }

    [Column("Codigo do Credito")]
    public int CodigoDoCredito { get; set; }

    [InverseProperty("SequenciaDaEntradaNavigation")]
    public virtual ICollection<ParcelaEntradaConta> ParcelasEntradaConta { get; set; } = new List<ParcelaEntradaConta>();

    [ForeignKey("SequenciaDoGeral")]
    [InverseProperty("EntradaConta")]
    public virtual Geral SequenciaDoGeralNavigation { get; set; } = null!;

    [ForeignKey("SequenciaGrupoDespesa")]
    [InverseProperty("EntradaConta")]
    public virtual GrupoDaDespesa SequenciaGrupoDespesaNavigation { get; set; } = null!;

    [ForeignKey("SeqüênciaSubGrupoDespesa, SeqüênciaGrupoDespesa")]
    [InverseProperty("EntradaConta")]
    public virtual SubGrupoDespesa SubGrupoDespesa { get; set; } = null!;
}
