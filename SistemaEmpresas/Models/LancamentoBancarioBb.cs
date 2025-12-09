using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("LancamentoBancarioBB")]
[Index("SequenciaDaAgencia", "SequenciaDaCcdaAgencia", Name = "IX_LancamentoBB_Conta")]
[Index("DataLancamento", Name = "IX_LancamentoBB_DataLancamento")]
[Index("TextoDescricaoHistorico", Name = "IX_LancamentoBB_Historico")]
[Index("Processado", Name = "IX_LancamentoBB_Processado")]
public partial class LancamentoBancarioBb
{
    [Key]
    [Column("SequenciaLancamentoBB")]
    public int SequenciaLancamentoBb { get; set; }

    public int? SequenciaDaAgencia { get; set; }

    [Column("SequenciaDaCCDaAgencia")]
    public int? SequenciaDaCcdaAgencia { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataLancamento { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataMovimento { get; set; }

    [Column(TypeName = "money")]
    public decimal Valor { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string TipoLancamento { get; set; } = null!;

    public int CodigoHistorico { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string TextoDescricaoHistorico { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? NumeroDocumento { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? CpfCnpj { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? NomeDevedor { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? IndicadorCheque { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? NumeroCheque { get; set; }

    public bool Processado { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataProcessamento { get; set; }

    public int? SequenciaDaBaixaGerada { get; set; }

    public int? SequenciaManutencaoVinculada { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? MotivoNaoProcessado { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PedidoIdentificado { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataImportacao { get; set; }

    [Column(TypeName = "text")]
    public string? DadosOriginaisJson { get; set; }
}
