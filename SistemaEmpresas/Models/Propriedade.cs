using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

public partial class Propriedade
{
    [Key]
    [Column("Seqüência da Propriedade")]
    public short SequenciaDaPropriedade { get; set; }

    [Column("Nome da Propriedade")]
    [StringLength(62)]
    [Unicode(false)]
    public string NomeDaPropriedade { get; set; } = null!;

    [Column("CNPJ")]
    [StringLength(18)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [Column("Inscrição Estadual")]
    [StringLength(20)]
    [Unicode(false)]
    public string InscricaoEstadual { get; set; } = null!;

    [Column("Endereço")]
    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [Column("Número do Endereço")]
    [StringLength(10)]
    [Unicode(false)]
    public string NumeroDoEndereco { get; set; } = null!;

    [Column("Complemento")]
    [StringLength(100)]
    [Unicode(false)]
    public string Complemento { get; set; } = null!;

    [Column("Caixa Postal")]
    [StringLength(30)]
    [Unicode(false)]
    public string CaixaPostal { get; set; } = null!;

    [Column("Bairro")]
    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("Seqüência do Município")]
    public int SequenciaDoMunicipio { get; set; }

    [Column("CEP")]
    [StringLength(9)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [InverseProperty("SequenciaDaPropriedadeNavigation")]
    public virtual ICollection<MovimentoDoEstoque> MovimentoDoEstoques { get; set; } = new List<MovimentoDoEstoque>();

    [InverseProperty("SequenciaDaPropriedadeNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();

    [InverseProperty("SequenciaDaPropriedadeNavigation")]
    public virtual ICollection<OrdemDeServico> OrdemDeServicos { get; set; } = new List<OrdemDeServico>();

    [InverseProperty("SequenciaDaPropriedadeNavigation")]
    public virtual ICollection<Orcamento> Orcamentos { get; set; } = new List<Orcamento>();

    [InverseProperty("SequenciaDaPropriedadeNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("SequenciaDaPropriedadeNavigation")]
    public virtual ICollection<PropriedadeDoGeral> PropriedadesDoGerals { get; set; } = new List<PropriedadeDoGeral>();
}
