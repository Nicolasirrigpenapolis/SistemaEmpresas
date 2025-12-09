using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Serviços")]
public partial class Servico
{
    [Key]
    [Column("Seqüência do Serviço")]
    public short SequenciaDoServico { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("Valor do Serviço", TypeName = "decimal(11, 2)")]
    public decimal ValorDoServico { get; set; }

    public bool Inativo { get; set; }

    [InverseProperty("SequenciaDoServicoNavigation")]
    public virtual ICollection<ServicoDaNotaFiscal> ServicosDaNotaFiscals { get; set; } = new List<ServicoDaNotaFiscal>();

    [InverseProperty("SequenciaDoServicoNavigation")]
    public virtual ICollection<ServicoDaOrdemDeServico> ServicosDaOrdemDeServicos { get; set; } = new List<ServicoDaOrdemDeServico>();

    [InverseProperty("SequenciaDoServicoNavigation")]
    public virtual ICollection<ServicoDoOrcamento> ServicosDoOrcamentos { get; set; } = new List<ServicoDoOrcamento>();

    [InverseProperty("SequenciaDoServicoNavigation")]
    public virtual ICollection<ServicoDoPedido> ServicosDoPedidos { get; set; } = new List<ServicoDoPedido>();
}
