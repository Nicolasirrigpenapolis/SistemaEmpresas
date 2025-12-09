using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Parâmetros da NFe")]
public partial class ParametroDaNfe
{
    public short Ambiente { get; set; }

    [Column("Diretório 1 NFe Homologação")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio1NfeHomologacao { get; set; } = null!;

    [Column("Diretório 2 NFe Homologação")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio2NfeHomologacao { get; set; } = null!;

    [Column("Diretório 1 NFe Produção")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio1NfeProducao { get; set; } = null!;

    [Column("Diretório 2 NFe Produção")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio2NfeProducao { get; set; } = null!;

    [Column("Diretório 1 NFSe Homologação")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio1NfseHomologacao { get; set; } = null!;

    [Column("Diretório 2 NFSe Homologação")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio2NfseHomologacao { get; set; } = null!;

    [Column("Diretório 1 NFSe Produção")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio1NfseProducao { get; set; } = null!;

    [Column("Diretório 2 NFSe Produção")]
    [StringLength(255)]
    [Unicode(false)]
    public string Diretorio2NfseProducao { get; set; } = null!;

    [Column("Certificado Digital")]
    [StringLength(255)]
    [Unicode(false)]
    public string CertificadoDigital { get; set; } = null!;

    [Column("Testemunha 1")]
    [StringLength(255)]
    [Unicode(false)]
    public string Testemunha1 { get; set; } = null!;

    [Column("Testemunha 2")]
    [StringLength(255)]
    [Unicode(false)]
    public string Testemunha2 { get; set; } = null!;

    [Column("CPF Testemunha 1")]
    [StringLength(14)]
    [Unicode(false)]
    public string CpfTestemunha1 { get; set; } = null!;

    [Column("CPF Testemunha 2")]
    [StringLength(14)]
    [Unicode(false)]
    public string CpfTestemunha2 { get; set; } = null!;

    [Column("Horario de Verao")]
    public bool HorarioDeVerao { get; set; }
}
