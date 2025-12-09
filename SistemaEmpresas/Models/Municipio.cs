using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Municípios")]
public partial class Municipio
{
    [Key]
    [Column("Seqüência do Município")]
    public int SequenciaDoMunicipio { get; set; }

    [Column("Descrição")]
    [StringLength(120)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("UF")]
    [StringLength(3)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    [Column("Código do IBGE")]
    public int CodigoDoIbge { get; set; }

    [Column("CEP")]
    [StringLength(9)]
    [Unicode(false)]
    public string? Cep { get; set; }

    public bool Inativo { get; set; }

    [InverseProperty("SequenciaDoMunicipioNavigation")]
    public virtual ICollection<Geral> GeralSequenciaDoMunicipioNavigations { get; set; } = new List<Geral>();

    [InverseProperty("SequenciaMunicipioCobrancaNavigation")]
    public virtual ICollection<Geral> GeralSequenciaMunicipioCobrancaNavigations { get; set; } = new List<Geral>();

    [InverseProperty("MunicipioDaTransportadoraNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDoMunicipioNavigation")]
    public virtual ICollection<OrdemDeServico> OrdemDeServicos { get; set; } = new List<OrdemDeServico>();

    [InverseProperty("SequenciaDoMunicipioNavigation")]
    public virtual ICollection<Orcamento> Orcamentos { get; set; } = new List<Orcamento>();
}
