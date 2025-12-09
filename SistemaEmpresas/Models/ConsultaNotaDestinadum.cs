using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Consulta Notas Destinada")]
public partial class ConsultaNotaDestinadum
{
    [Key]
    [Column("Seqüência da Consulta")]
    public short SequenciaDaConsulta { get; set; }

    [Column("Chave de Acesso da NFe")]
    [StringLength(50)]
    [Unicode(false)]
    public string ChaveDeAcessoDaNfe { get; set; } = null!;

    [Column("CNPJ")]
    [StringLength(18)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [Column("Razão Social")]
    [StringLength(60)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [Column("Inscrição Estadual")]
    [StringLength(20)]
    [Unicode(false)]
    public string InscricaoEstadual { get; set; } = null!;

    [Column("Data de Emissão", TypeName = "datetime")]
    public DateTime DataDeEmissao { get; set; }

    [Column("Valor Total", TypeName = "decimal(11, 2)")]
    public decimal ValorTotal { get; set; }
}
