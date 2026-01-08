using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Table("Movimento Contábil Novo")]
public partial class MovimentoContabilNovo
{
    [Key]
    [Column("Seqüência do Movimento")]
    public int SequenciaDoMovimento { get; set; }

    [Column("Data do Movimento", TypeName = "datetime")]
    public DateTime? DataDoMovimento { get; set; }

    [Column("Tipo do Movimento")]
    public short TipoDoMovimento { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [Column("Seqüência do Geral")]
    public int SequenciaDoGeral { get; set; }

    [Column("Observação", TypeName = "text")]
    public string Observacao { get; set; } = null!;

    [Column("Devolução")]
    public bool Devolucao { get; set; }

    [Column("Seq Prod Propria")]
    public int SeqProdPropria { get; set; }

    [Column("E Produção Propria")]
    public bool EProducaoPropria { get; set; }

    [Column("Baixa Consumo")]
    public bool BaixaConsumo { get; set; }

    [Column("Seq Baixa Consumo")]
    public int SeqBaixaConsumo { get; set; }

    [Column("Seqüência Grupo Despesa")]
    public short SequenciaGrupoDespesa { get; set; }

    [Column("Seqüência SubGrupo Despesa")]
    public short SequenciaSubGrupoDespesa { get; set; }

    [Column("Forma de Pagamento")]
    [StringLength(10)]
    [Unicode(false)]
    public string FormaDePagamento { get; set; } = null!;

    [Column("Valor do Frete", TypeName = "decimal(12, 4)")]
    public decimal ValorDoFrete { get; set; }

    [Column("Valor do Desconto", TypeName = "decimal(11, 2)")]
    public decimal ValorDoDesconto { get; set; }

    [Column("Valor Total dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDosProdutos { get; set; }

    [Column("Valor Total IPI dos Produtos", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDosProdutos { get; set; }

    [Column("Valor Total do Movimento", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDoMovimento { get; set; }

    [Column("Data da Alteração", TypeName = "datetime")]
    public DateTime? DataDaAlteracao { get; set; }

    [Column("Hora da Alteração", TypeName = "datetime")]
    public DateTime? HoraDaAlteracao { get; set; }

    [Column("Usuário da Alteração")]
    [StringLength(60)]
    [Unicode(false)]
    public string? UsuarioDaAlteracao { get; set; }

    [Column("Valor Total das Despesas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalDasDespesas { get; set; }

    [Column("Valor Total IPI das Despesas", TypeName = "decimal(11, 2)")]
    public decimal ValorTotalIpiDasDespesas { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    public bool Fechado { get; set; }

    [Column("Sequencia da Compra")]
    public int SequenciaDaCompra { get; set; }

    [Column("Seqüência do Orçamento")]
    public int SequenciaDoOrcamento { get; set; }

    [Column("Codigo do Debito")]
    public int CodigoDoDebito { get; set; }
}
