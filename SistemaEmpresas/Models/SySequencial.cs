using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SysTabela", "SysCampo", "SysChave")]
[Table("SYS~Sequencial")]
public partial class SySequencial
{
    [Column("PW~Projeto")]
    [StringLength(10)]
    [Unicode(false)]
    public string PwProjeto { get; set; } = null!;

    [Key]
    [Column("SYS~Tabela")]
    [StringLength(30)]
    [Unicode(false)]
    public string SysTabela { get; set; } = null!;

    [Key]
    [Column("SYS~Campo")]
    [StringLength(30)]
    [Unicode(false)]
    public string SysCampo { get; set; } = null!;

    [Key]
    [Column("SYS~Chave")]
    [StringLength(200)]
    [Unicode(false)]
    public string SysChave { get; set; } = null!;

    [Column("SYS~Valor")]
    [StringLength(50)]
    [Unicode(false)]
    public string SysValor { get; set; } = null!;

    [Column("SYS~ValorAnterior")]
    [StringLength(50)]
    [Unicode(false)]
    public string SysValorAnterior { get; set; } = null!;

    [Column("SYS~Estacao")]
    [StringLength(50)]
    [Unicode(false)]
    public string SysEstacao { get; set; } = null!;

    [Column("SYS~Identificacao")]
    [StringLength(30)]
    [Unicode(false)]
    public string SysIdentificacao { get; set; } = null!;

    [Column("SYS~Pendentes")]
    public int SysPendentes { get; set; }
}
