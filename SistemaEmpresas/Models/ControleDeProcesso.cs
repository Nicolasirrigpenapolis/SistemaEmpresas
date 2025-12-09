using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Controle de Processos")]
public partial class ControleDeProcesso
{
    [Key]
    [Column("Id do Processo")]
    public int IdDoProcesso { get; set; }

    [Column("Codigo do Status")]
    public short CodigoDoStatus { get; set; }

    [Column("Codigo do Advogado")]
    public short CodigoDoAdvogado { get; set; }

    [Column("Codigo da Ação")]
    public short CodigoDaAcao { get; set; }

    [Column("Outro Envolvido")]
    public int OutroEnvolvido { get; set; }

    [ForeignKey("CodigoDoAdvogado")]
    [InverseProperty("ControleDeProcessos")]
    public virtual Advogado CodigoDoAdvogadoNavigation { get; set; } = null!;

    [ForeignKey("CodigoDoStatus")]
    [InverseProperty("ControleDeProcessos")]
    public virtual StatuDoProcesso CodigoDoStatusNavigation { get; set; } = null!;
}
