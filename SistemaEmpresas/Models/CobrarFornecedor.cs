using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Cobrar Fornecedor")]
public partial class CobrarFornecedor
{
    [Key]
    [Column("Codigo da Cobrança")]
    public int CodigoDaCobranca { get; set; }

    [Column("Data da Cobrança", TypeName = "datetime")]
    public DateTime? DataDaCobranca { get; set; }

    [Column("Codigo do Fornecedor")]
    public int CodigoDoFornecedor { get; set; }

    [Column("Nova Previsão", TypeName = "datetime")]
    public DateTime? NovaPrevisao { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string Justificacao { get; set; } = null!;

    [Column("Id do Pedido")]
    public int IdDoPedido { get; set; }

    [Column("Antiga Previsão", TypeName = "datetime")]
    public DateTime? AntigaPrevisao { get; set; }

    [Column("Usuario da Cobrança")]
    [StringLength(20)]
    [Unicode(false)]
    public string UsuarioDaCobranca { get; set; } = null!;
}
