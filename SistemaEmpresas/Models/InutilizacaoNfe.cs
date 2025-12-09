using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Inutilização NFe")]
public partial class InutilizacaoNfe
{
    [Key]
    [Column("Seqüência da Inutilização")]
    public int SequenciaDaInutilizacao { get; set; }

    public short Ano { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Justificativa { get; set; } = null!;

    public short Ambiente { get; set; }

    [Column("Faixa Inicial")]
    public int FaixaInicial { get; set; }

    [Column("Faixa Final")]
    public int FaixaFinal { get; set; }

    [Column("Data da Inutilização", TypeName = "datetime")]
    public DateTime? DataDaInutilizacao { get; set; }
}
