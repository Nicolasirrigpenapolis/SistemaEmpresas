using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Importação")]
public partial class Importacao
{
    [Column("Última Agência")]
    public int UltimaAgencia { get; set; }

    [Column("Última Baixa Receber")]
    public int UltimaBaixaReceber { get; set; }

    [Column("Última Baixa Pagar")]
    public int UltimaBaixaPagar { get; set; }

    [Column("Última Classificação Fiscal")]
    public int UltimaClassificacaoFiscal { get; set; }

    [Column("Último Conjunto")]
    public int UltimoConjunto { get; set; }

    [Column("Último Dados Adicionais")]
    public int UltimoDadosAdicionais { get; set; }

    [Column("Última Entrada Receber")]
    public int UltimaEntradaReceber { get; set; }

    [Column("Última Entrada Pagar")]
    public int UltimaEntradaPagar { get; set; }

    [Column("Último Cliente")]
    public int UltimoCliente { get; set; }

    [Column("Último Fornecedor")]
    public int UltimoFornecedor { get; set; }

    [Column("Último Vendedor")]
    public int UltimoVendedor { get; set; }

    [Column("Último Grupo da Despesa")]
    public int UltimoGrupoDaDespesa { get; set; }

    [Column("Último Grupo do Produto")]
    public int UltimoGrupoDoProduto { get; set; }

    [Column("Último Histórico da CC")]
    public int UltimoHistoricoDaCc { get; set; }

    [Column("Último ICMS")]
    public int UltimoIcms { get; set; }

    [Column("Última Manutenção Pagar")]
    public int UltimaManutencaoPagar { get; set; }

    [Column("Última Manutenção Receber")]
    public int UltimaManutencaoReceber { get; set; }

    [Column("Último Movimento da CC")]
    public int UltimoMovimentoDaCc { get; set; }

    [Column("Última Cidade")]
    public int UltimaCidade { get; set; }

    [Column("Última Natureza de Operação")]
    public int UltimaNaturezaDeOperacao { get; set; }

    [Column("Último Produto")]
    public int UltimoProduto { get; set; }

    [Column("Último Serviço")]
    public int UltimoServico { get; set; }

    [Column("Última Tabela A")]
    public int UltimaTabelaA { get; set; }

    [Column("Última Tabela B")]
    public int UltimaTabelaB { get; set; }

    [Column("Última Cobrança")]
    public int UltimaCobranca { get; set; }

    [Column("Última Unidade")]
    public int UltimaUnidade { get; set; }

    [Column("Último Acerto no Estoque")]
    public int UltimoAcertoNoEstoque { get; set; }

    [Column("Última Entrada no Estoque")]
    public int UltimaEntradaNoEstoque { get; set; }

    [Column("Última Entrada Receita")]
    public int UltimaEntradaReceita { get; set; }

    [Column("Último Movimento Estoque")]
    public int UltimoMovimentoEstoque { get; set; }

    [Column("Último Movimento Estoque Conj")]
    public int UltimoMovimentoEstoqueConj { get; set; }

    [Column("Última Requisição")]
    public int UltimaRequisicao { get; set; }

    [Column("Última Entrada Contábil")]
    public int UltimaEntradaContabil { get; set; }

    [Column("Última Nota Fiscal")]
    public int UltimaNotaFiscal { get; set; }

    [Column("Último Orçamento")]
    public int UltimoOrcamento { get; set; }

    [Column("Última Ordem de Serviço")]
    public int UltimaOrdemDeServico { get; set; }

    [Column("Último Pedido")]
    public int UltimoPedido { get; set; }
}
