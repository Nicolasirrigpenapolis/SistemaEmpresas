using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class CreateAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acoes",
                columns: table => new
                {
                    CodigodaAção = table.Column<short>(name: "Codigo da Ação", type: "smallint", nullable: false),
                    DescriçãodaAção = table.Column<string>(name: "Descrição da Ação", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Adutoras",
                columns: table => new
                {
                    SequenciadaAdutora = table.Column<int>(name: "Sequencia da Adutora", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelodaAdutora = table.Column<string>(name: "Modelo da Adutora", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    DN = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    DNmm = table.Column<decimal>(name: "DN mm", type: "decimal(8,2)", nullable: false),
                    Coeficiente = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Material = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false, defaultValue: ""),
                    Emm = table.Column<decimal>(name: "E mm", type: "decimal(8,2)", nullable: false),
                    DImm = table.Column<decimal>(name: "DI mm", type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Adutora", x => x.SequenciadaAdutora);
                });

            migrationBuilder.CreateTable(
                name: "Advogados",
                columns: table => new
                {
                    CodigodoAdvogado = table.Column<short>(name: "Codigo do Advogado", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomedoAdvogado = table.Column<string>(name: "Nome do Advogado", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Celular = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do Advogado", x => x.CodigodoAdvogado);
                });

            migrationBuilder.CreateTable(
                name: "Agencias",
                columns: table => new
                {
                    SeqüênciadaAgência = table.Column<short>(name: "Seqüência da Agência", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NúmerodoBanco = table.Column<string>(name: "Número do Banco", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    NúmerodaAgência = table.Column<string>(name: "Número da Agência", type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, defaultValue: ""),
                    NomedoBanco = table.Column<string>(name: "Nome do Banco", type: "varchar(35)", unicode: false, maxLength: 35, nullable: false, defaultValue: ""),
                    NomedaAgência = table.Column<string>(name: "Nome da Agência", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Endereco = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Bairro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    Telefone = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    CNPJ = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: false, defaultValue: ""),
                    NãoCalcular = table.Column<bool>(name: "Não Calcular", type: "bit", nullable: false),
                    Ativa = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Agência", x => x.SeqüênciadaAgência);
                });

            migrationBuilder.CreateTable(
                name: "Agendamento de Backup",
                columns: table => new
                {
                    SeqüênciadoBackup = table.Column<int>(name: "Seqüência do Backup", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipodoBackup = table.Column<string>(name: "Tipo do Backup", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    Hora = table.Column<DateTime>(type: "datetime", nullable: false),
                    Segunda = table.Column<bool>(type: "bit", nullable: false),
                    Terca = table.Column<bool>(type: "bit", nullable: false),
                    Quarta = table.Column<bool>(type: "bit", nullable: false),
                    Quinta = table.Column<bool>(type: "bit", nullable: false),
                    Sexta = table.Column<bool>(type: "bit", nullable: false),
                    Sabado = table.Column<bool>(type: "bit", nullable: false),
                    Domingo = table.Column<bool>(type: "bit", nullable: false),
                    Dia = table.Column<short>(type: "smallint", nullable: false),
                    Destino = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Backup", x => x.SeqüênciadoBackup);
                });

            migrationBuilder.CreateTable(
                name: "Alteracao Baixa Contas",
                columns: table => new
                {
                    SeqdoSpy = table.Column<int>(name: "Seq do Spy", type: "int", nullable: false),
                    SeqdaBaixa = table.Column<int>(name: "Seq da Baixa", type: "int", nullable: false),
                    UsuAlteracao = table.Column<string>(name: "Usu Alteracao", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    DtModificacao = table.Column<DateTime>(name: "Dt Modificacao", type: "datetime", nullable: true),
                    Manutencao = table.Column<long>(type: "bigint", nullable: false),
                    DtaBaixa = table.Column<DateTime>(name: "Dta Baixa", type: "datetime", nullable: true),
                    Juros = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    VrPago = table.Column<decimal>(name: "Vr Pago", type: "decimal(11,2)", nullable: false),
                    TpCarteira = table.Column<string>(name: "Tp Carteira", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    BxdoCliente = table.Column<DateTime>(name: "Bx do Cliente", type: "datetime", nullable: true),
                    QuemPagou = table.Column<string>(name: "Quem Pagou", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    VrdoCliente = table.Column<decimal>(name: "Vr do Cliente", type: "decimal(10,2)", nullable: false),
                    SeqdaAgencia = table.Column<short>(name: "Seq da Agencia", type: "smallint", nullable: false),
                    SeqAccdaAgencia = table.Column<short>(name: "Seq Acc da Agencia", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Aspersor Final",
                columns: table => new
                {
                    SequenciadoAspersor = table.Column<int>(name: "Sequencia do Aspersor", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modelodoaspersor = table.Column<string>(name: "Modelo do aspersor", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    CanhaoouAspersor = table.Column<string>(name: "Canhao ou Aspersor", type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValue: ""),
                    Bocal = table.Column<int>(type: "int", nullable: false),
                    PressãodeTrabalho = table.Column<decimal>(name: "Pressão de Trabalho", type: "decimal(8,2)", nullable: false),
                    Vazao = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Alcance = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    AreaFinal = table.Column<decimal>(name: "Area Final", type: "decimal(8,3)", nullable: false),
                    VolumedeReferencia = table.Column<decimal>(name: "Volume de Referencia", type: "decimal(7,3)", nullable: false),
                    Percentualraiomolhado = table.Column<decimal>(name: "Percentual raio molhado", type: "decimal(7,3)", nullable: false),
                    AlcanceraioMolhado = table.Column<decimal>(name: "Alcance raio Molhado", type: "decimal(7,3)", nullable: false),
                    AreaConsiderada = table.Column<decimal>(name: "Area Considerada", type: "decimal(7,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Aspersor", x => x.SequenciadoAspersor);
                });

            migrationBuilder.CreateTable(
                name: "Baixa Comissão Lote",
                columns: table => new
                {
                    SeqdaBx = table.Column<int>(name: "Seq da Bx", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadaBx = table.Column<DateTime>(name: "Data da Bx", type: "datetime", nullable: false),
                    CoddoVendedor = table.Column<int>(name: "Cod do Vendedor", type: "int", nullable: false),
                    FiltroIni = table.Column<DateTime>(type: "datetime", nullable: true),
                    FiltroFim = table.Column<DateTime>(type: "datetime", nullable: true),
                    UsudaBaixa = table.Column<string>(name: "Usu da Baixa", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Fechado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq da Bx", x => x.SeqdaBx);
                });

            migrationBuilder.CreateTable(
                name: "Baixa Comissão Lote Contas",
                columns: table => new
                {
                    IddaBaixa = table.Column<int>(name: "Id da Baixa", type: "int", nullable: false),
                    IddoAdiantamento = table.Column<int>(name: "Id do Adiantamento", type: "int", nullable: false),
                    NFe = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Parcela = table.Column<short>(type: "smallint", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    ValorPago = table.Column<decimal>(name: "Valor Pago", type: "decimal(11,2)", nullable: false),
                    Vencto = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataPagtoCliente = table.Column<DateTime>(name: "Data Pagto Cliente", type: "datetime", nullable: true),
                    Percentual = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    Comissao = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id da Baixa", x => new { x.IddaBaixa, x.IddoAdiantamento });
                });

            migrationBuilder.CreateTable(
                name: "Baixa MP Conjunto",
                columns: table => new
                {
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    DatadaBaixa = table.Column<DateTime>(name: "Data da Baixa", type: "datetime", nullable: true),
                    HoradaBaixa = table.Column<DateTime>(name: "Hora da Baixa", type: "datetime", nullable: true),
                    SeqüênciadoItem = table.Column<short>(name: "Seqüência do Item", type: "smallint", nullable: false),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    QuantidadedoConjunto = table.Column<decimal>(name: "Quantidade do Conjunto", type: "decimal(9,3)", nullable: false),
                    SeqüênciadaMatériaPrima = table.Column<int>(name: "Seqüência da Matéria Prima", type: "int", nullable: false),
                    QuantidadedaMatériaPrima = table.Column<decimal>(name: "Quantidade da Matéria Prima", type: "decimal(9,3)", nullable: false),
                    CalcularEstoque = table.Column<bool>(name: "Calcular Estoque", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Baixa MP Conj", x => x.SeqüênciadaBaixa);
                });

            migrationBuilder.CreateTable(
                name: "Bocal Aspersor Nelson",
                columns: table => new
                {
                    SequenciadoBocal = table.Column<int>(name: "Sequencia do Bocal", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModeloAspersor = table.Column<string>(name: "Modelo Aspersor", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    BocaldoAspersor = table.Column<decimal>(name: "Bocal do Aspersor", type: "decimal(5,2)", nullable: false),
                    MCA = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Vazao = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    RaiodeAlcancemetros = table.Column<decimal>(name: "Raio de Alcance metros", type: "decimal(6,2)", nullable: false),
                    AreaTotalha = table.Column<decimal>(name: "Area Total ha", type: "decimal(6,2)", nullable: false),
                    VolumeReferenciamm = table.Column<decimal>(name: "Volume Referencia mm", type: "decimal(6,2)", nullable: false),
                    PercentualalcanceMolhado = table.Column<decimal>(name: "Percentual alcance Molhado", type: "decimal(6,2)", nullable: false),
                    AlcenceRaioMolhadom = table.Column<decimal>(name: "Alcence Raio Molhado m", type: "decimal(6,2)", nullable: false),
                    Alcenceaspersorfinalha = table.Column<decimal>(name: "Alcence aspersor final ha", type: "decimal(6,2)", nullable: false),
                    FabricantedoAspersor = table.Column<string>(name: "Fabricante do Aspersor", type: "varchar(12)", unicode: false, maxLength: 12, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Bocal", x => x.SequenciadoBocal);
                });

            migrationBuilder.CreateTable(
                name: "Bx Consumo Pedido Compra",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    IdDespesa = table.Column<int>(name: "Id Despesa", type: "int", nullable: false),
                    IddaDespesa = table.Column<int>(name: "Id da Despesa", type: "int", nullable: false),
                    QtdeTotal = table.Column<decimal>(name: "Qtde Total", type: "decimal(10,2)", nullable: false),
                    VrUnitario = table.Column<decimal>(name: "Vr Unitario", type: "decimal(10,2)", nullable: false),
                    VrTotaldoPedido = table.Column<decimal>(name: "Vr Total do Pedido", type: "decimal(10,2)", nullable: false),
                    QtdeRecebida = table.Column<decimal>(name: "Qtde Recebida", type: "decimal(10,2)", nullable: false),
                    QtdeRestante = table.Column<decimal>(name: "Qtde Restante", type: "decimal(10,2)", nullable: false),
                    TotalRestante = table.Column<decimal>(name: "Total Restante", type: "decimal(10,2)", nullable: false),
                    Notas = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Bx Consumo", x => new { x.IddoPedido, x.IdDespesa });
                });

            migrationBuilder.CreateTable(
                name: "Bx Despesas Pedido Compra",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    IddaDespesa = table.Column<int>(name: "Id da Despesa", type: "int", nullable: false),
                    QtdeTotal = table.Column<decimal>(name: "Qtde Total", type: "decimal(10,2)", nullable: false),
                    VrUnitario = table.Column<decimal>(name: "Vr Unitario", type: "decimal(10,2)", nullable: false),
                    VrTotaldoPedido = table.Column<decimal>(name: "Vr Total do Pedido", type: "decimal(10,2)", nullable: false),
                    QtdeRecebida = table.Column<decimal>(name: "Qtde Recebida", type: "decimal(10,2)", nullable: false),
                    QtdeRestante = table.Column<decimal>(name: "Qtde Restante", type: "decimal(10,2)", nullable: false),
                    TotalRestante = table.Column<decimal>(name: "Total Restante", type: "decimal(10,2)", nullable: false),
                    Notas = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Bx Despesa", x => new { x.IddoPedido, x.IddaDespesa });
                });

            migrationBuilder.CreateTable(
                name: "Bx Produtos Pedido Compra",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    IddoProduto = table.Column<int>(name: "Id do Produto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    QtdeTotal = table.Column<decimal>(name: "Qtde Total", type: "decimal(10,2)", nullable: false),
                    VrUnitario = table.Column<decimal>(name: "Vr Unitario", type: "decimal(11,4)", nullable: false),
                    VrTotaldoPedido = table.Column<decimal>(name: "Vr Total do Pedido", type: "decimal(10,2)", nullable: false),
                    QtdeRecebida = table.Column<decimal>(name: "Qtde Recebida", type: "decimal(10,2)", nullable: false),
                    QtdeRestante = table.Column<decimal>(name: "Qtde Restante", type: "decimal(10,2)", nullable: false),
                    TotalRestante = table.Column<decimal>(name: "Total Restante", type: "decimal(10,2)", nullable: false),
                    Notas = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Teste = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Bx Produto", x => new { x.IddoPedido, x.IddoProduto, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Calendario",
                columns: table => new
                {
                    SeqdoCalendario = table.Column<int>(name: "Seq do Calendario", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtadoFeriado = table.Column<DateTime>(name: "Dta do Feriado", type: "datetime", nullable: false),
                    DiadaSemana = table.Column<string>(name: "Dia da Semana", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Calendario", x => x.SeqdoCalendario);
                });

            migrationBuilder.CreateTable(
                name: "Check list maquina",
                columns: table => new
                {
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    Tpproduto = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false, defaultValue: ""),
                    SeqüênciadaMatériaPrima = table.Column<int>(name: "Seqüência da Matéria Prima", type: "int", nullable: false),
                    Qtdeutilizada = table.Column<decimal>(name: "Qtde utilizada", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_e_itemcheck", x => new { x.SeqüênciadoProduto, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Cheques Cancelados",
                columns: table => new
                {
                    Sequencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    Banco = table.Column<short>(type: "smallint", nullable: false),
                    NrodaConta = table.Column<string>(name: "Nro da Conta", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    NrodoCheque = table.Column<string>(name: "Nro do Cheque", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    MotivodoCancelamento = table.Column<string>(name: "Motivo do Cancelamento", type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Cheque", x => x.Sequencia);
                });

            migrationBuilder.CreateTable(
                name: "Classificação Fiscal",
                columns: table => new
                {
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NCM = table.Column<int>(type: "int", nullable: false),
                    DescriçãodoNCM = table.Column<string>(name: "Descrição do NCM", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    PorcentagemdoIPI = table.Column<decimal>(name: "Porcentagem do IPI", type: "decimal(8,4)", nullable: false),
                    AnexodaRedução = table.Column<short>(name: "Anexo da Redução", type: "smallint", nullable: false),
                    AlíquotadoAnexo = table.Column<short>(name: "Alíquota do Anexo", type: "smallint", nullable: false),
                    ProdutoDiferido = table.Column<bool>(name: "Produto Diferido", type: "bit", nullable: false),
                    ReduçãodeBasedeCálculo = table.Column<bool>(name: "Redução de Base de Cálculo", type: "bit", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    TemConvênio = table.Column<bool>(name: "Tem Convênio", type: "bit", nullable: false),
                    Cest = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false, defaultValue: ""),
                    UnExterior = table.Column<string>(name: "Un Exterior", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Classificação", x => x.SeqüênciadaClassificação);
                });

            migrationBuilder.CreateTable(
                name: "Clientes Processos",
                columns: table => new
                {
                    CodigodoCliente = table.Column<int>(name: "Codigo do Cliente", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomedoCliente = table.Column<string>(name: "Nome do Cliente", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Envolvido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do Cliente", x => x.CodigodoCliente);
                });

            migrationBuilder.CreateTable(
                name: "Cobrar Fornecedor",
                columns: table => new
                {
                    CodigodaCobrança = table.Column<int>(name: "Codigo da Cobrança", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadaCobrança = table.Column<DateTime>(name: "Data da Cobrança", type: "datetime", nullable: true),
                    CodigodoFornecedor = table.Column<int>(name: "Codigo do Fornecedor", type: "int", nullable: false),
                    NovaPrevisão = table.Column<DateTime>(name: "Nova Previsão", type: "datetime", nullable: true),
                    Justificacao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    AntigaPrevisão = table.Column<DateTime>(name: "Antiga Previsão", type: "datetime", nullable: true),
                    UsuariodaCobrança = table.Column<string>(name: "Usuario da Cobrança", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo da Cobrança", x => x.CodigodaCobrança);
                });

            migrationBuilder.CreateTable(
                name: "Comissão do montador",
                columns: table => new
                {
                    Sequenciadacomissão = table.Column<int>(name: "Sequencia da comissão", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoddoVendedor = table.Column<int>(name: "Cod do Vendedor", type: "int", nullable: false),
                    Manutencao = table.Column<int>(type: "int", nullable: false),
                    NFe = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Percentual = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    PagtoVendedor = table.Column<DateTime>(name: "Pagto Vendedor", type: "datetime", nullable: true),
                    Comissao = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Imprimir = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da comissão", x => x.Sequenciadacomissão);
                });

            migrationBuilder.CreateTable(
                name: "Composição do Equipamento",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    ModelodoLance = table.Column<int>(name: "Modelo do Lance", type: "int", nullable: false),
                    TipodoLance = table.Column<string>(name: "Tipo do Lance", type: "varchar(13)", unicode: false, maxLength: 13, nullable: false, defaultValue: ""),
                    QuantdeLance = table.Column<short>(name: "Quant de Lance", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SeqProjeto_item", x => new { x.SequenciadoProjeto, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Concilia Conta Antecipada",
                columns: table => new
                {
                    SequenciadaConciliação = table.Column<int>(name: "Sequencia da Conciliação", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaManutenção = table.Column<int>(name: "Seqüência da Manutenção", type: "int", nullable: false),
                    SequenciadaCompra = table.Column<int>(name: "Sequencia da Compra", type: "int", nullable: false),
                    DatadaConciliação = table.Column<DateTime>(name: "Data da Conciliação", type: "datetime", nullable: true),
                    NotasdaCompra = table.Column<string>(name: "Notas da Compra", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Conciliado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Conciliação", x => x.SequenciadaConciliação);
                });

            migrationBuilder.CreateTable(
                name: "Conciliação de Cheques",
                columns: table => new
                {
                    SeqdaConciliação = table.Column<int>(name: "Seq da Conciliação", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtadaConciliação = table.Column<DateTime>(name: "Dta da Conciliação", type: "datetime", nullable: false),
                    Agencia = table.Column<short>(type: "smallint", nullable: false),
                    NCheque = table.Column<long>(name: "N Cheque", type: "bigint", nullable: false),
                    DtadeEmissão = table.Column<DateTime>(name: "Dta de Emissão", type: "datetime", nullable: false),
                    VrdoCheque = table.Column<decimal>(name: "Vr do Cheque", type: "decimal(10,2)", nullable: false),
                    VrCompensado = table.Column<decimal>(name: "Vr Compensado", type: "decimal(10,2)", nullable: false),
                    Conciliado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq da Conciliação", x => x.SeqdaConciliação);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chave = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Valor = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Descricao = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoIntegracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos do Projeto",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SequenciadoConjunto = table.Column<int>(name: "Sequencia do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    PartedoPivo = table.Column<string>(name: "Parte do Pivo", type: "varchar(29)", unicode: false, maxLength: 29, nullable: false, defaultValue: ""),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_e_Conjunto", x => new { x.SequenciadoProjeto, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Consulta Notas Destinada",
                columns: table => new
                {
                    SeqüênciadaConsulta = table.Column<short>(name: "Seqüência da Consulta", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChavedeAcessodaNFe = table.Column<string>(name: "Chave de Acesso da NFe", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    CNPJ = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: false, defaultValue: ""),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    InscriçãoEstadual = table.Column<string>(name: "Inscrição Estadual", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Consulta", x => x.SeqüênciadaConsulta);
                });

            migrationBuilder.CreateTable(
                name: "Conta Contabil",
                columns: table => new
                {
                    CodigoContabil = table.Column<int>(name: "Codigo Contabil", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContaContabil = table.Column<string>(name: "Conta Contabil", type: "varchar(56)", unicode: false, maxLength: 56, nullable: false, defaultValue: ""),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    SequenciadoGeral = table.Column<int>(name: "Sequencia do Geral", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo Contabil", x => x.CodigoContabil);
                });

            migrationBuilder.CreateTable(
                name: "Conta Corrente da Agência",
                columns: table => new
                {
                    SeqüênciadaAgência = table.Column<short>(name: "Seqüência da Agência", type: "smallint", nullable: false),
                    SeqüênciadaCCdaAgência = table.Column<short>(name: "Seqüência da CC da Agência", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NúmerodaContaCorrente = table.Column<string>(name: "Número da Conta Corrente", type: "varchar(11)", unicode: false, maxLength: 11, nullable: false, defaultValue: ""),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    ValordeSaída = table.Column<decimal>(name: "Valor de Saída", type: "decimal(11,2)", nullable: false),
                    ValordeEntrada = table.Column<decimal>(name: "Valor de Entrada", type: "decimal(11,2)", nullable: false),
                    ValorAtual = table.Column<decimal>(name: "Valor Atual", type: "decimal(11,2)", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    BBApiClientId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BBApiClientSecret = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BBApiDeveloperKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HabilitarIntegracaoBB = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DigitoConta = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Agencia e Seq da CC", x => new { x.SeqüênciadaAgência, x.SeqüênciadaCCdaAgência });
                });

            migrationBuilder.CreateTable(
                name: "Conta do Vendedor",
                columns: table => new
                {
                    IddaConta = table.Column<int>(name: "Id da Conta", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitulardaConta = table.Column<string>(name: "Titular da Conta", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Desativado = table.Column<bool>(type: "bit", nullable: false),
                    ALiberar = table.Column<decimal>(name: "A Liberar", type: "decimal(10,2)", nullable: false),
                    GerenteRegional = table.Column<string>(name: "Gerente Regional", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Fazprojeto = table.Column<bool>(name: "Faz projeto", type: "bit", nullable: false),
                    Montador = table.Column<bool>(type: "bit", nullable: false),
                    Percentual = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    Revenda = table.Column<bool>(type: "bit", nullable: false),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id da Conta", x => x.IddaConta);
                });

            migrationBuilder.CreateTable(
                name: "Controle de Compras",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    DatadoPedido = table.Column<DateTime>(name: "Data do Pedido", type: "datetime", nullable: true),
                    Comprador = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Vr_Unit_Ipi = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    Qtde_Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Qtde_Recebida = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Qtde_Restante = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Prazo = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Financeiro = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    DatadaBaixa = table.Column<DateTime>(name: "Data da Baixa", type: "datetime", nullable: true),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    CodigodoFornecedor = table.Column<int>(name: "Codigo do Fornecedor", type: "int", nullable: false),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Preventrega = table.Column<DateTime>(name: "Prev entrega", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Controle de Garantia",
                columns: table => new
                {
                    SequenciadoControle = table.Column<int>(name: "Sequencia do Controle", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    UsuáriodaAlteração = table.Column<string>(name: "Usuário da Alteração", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    DatadaAlteração = table.Column<DateTime>(name: "Data da Alteração", type: "datetime", nullable: true),
                    HoradaAlteração = table.Column<DateTime>(name: "Hora da Alteração", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Controle", x => x.SequenciadoControle);
                });

            migrationBuilder.CreateTable(
                name: "Controle de Pneus",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    SequenciadoPneu = table.Column<int>(name: "Sequencia do Pneu", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    NFeSaidas = table.Column<decimal>(name: "NFe Saidas", type: "decimal(10,2)", nullable: false),
                    ModelodoPneu = table.Column<string>(name: "Modelo do Pneu", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Projeto_Pneu", x => new { x.SequenciadoProjeto, x.SequenciadoPneu });
                });

            migrationBuilder.CreateTable(
                name: "Correcao Bloko K",
                columns: table => new
                {
                    SequenciadaCorreção = table.Column<int>(name: "Sequencia da Correção", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadaCorreção = table.Column<DateTime>(name: "Data da Correção", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Correção", x => x.SequenciadaCorreção);
                });

            migrationBuilder.CreateTable(
                name: "Dados Adicionais",
                columns: table => new
                {
                    SeqüênciadosDadosAdicionais = table.Column<int>(name: "Seqüência dos Dados Adicionais", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DadosAdicionais = table.Column<string>(name: "Dados Adicionais", type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência dos Dados Adicionais", x => x.SeqüênciadosDadosAdicionais);
                });

            migrationBuilder.CreateTable(
                name: "Despesas e vendas",
                columns: table => new
                {
                    Sequenciadasimulação = table.Column<int>(name: "Sequencia da simulação", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequenciadoGeral = table.Column<int>(name: "Sequencia do Geral", type: "int", nullable: false),
                    TotaldaViagem = table.Column<decimal>(name: "Total da Viagem", type: "decimal(10,2)", nullable: false),
                    Valordoorçamento = table.Column<decimal>(name: "Valor do orçamento", type: "decimal(12,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Comissao = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Ref = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_simula", x => x.Sequenciadasimulação);
                });

            migrationBuilder.CreateTable(
                name: "Divirgencias NFe",
                columns: table => new
                {
                    CodigodaDivirgencia = table.Column<int>(name: "Codigo da Divirgencia", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    NúmerodaNFe = table.Column<int>(name: "Número da NFe", type: "int", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo da Divirgencia", x => x.CodigodaDivirgencia);
                });

            migrationBuilder.CreateTable(
                name: "Duplicatas Descontadas",
                columns: table => new
                {
                    SeqdaDuplicata = table.Column<int>(name: "Seq da Duplicata", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duplicata = table.Column<int>(type: "int", nullable: false),
                    Pc = table.Column<short>(type: "smallint", nullable: false),
                    CoddoGeral = table.Column<int>(name: "Cod do Geral", type: "int", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    TpodeCarteira = table.Column<string>(name: "Tpo de Carteira", type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValue: ""),
                    DatadaBaixa = table.Column<DateTime>(name: "Data da Baixa", type: "datetime", nullable: false),
                    CoddoBanco = table.Column<int>(name: "Cod do Banco", type: "int", nullable: false),
                    CcdoBanco = table.Column<int>(name: "Cc do Banco", type: "int", nullable: false),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq da Duplicata", x => x.SeqdaDuplicata);
                });

            migrationBuilder.CreateTable(
                name: "Finalidade NFe",
                columns: table => new
                {
                    Codigo = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Finalidade = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Follow Up Vendas",
                columns: table => new
                {
                    SeqFollowUp = table.Column<int>(name: "Seq Follow Up", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    SeqüênciadaTransportadora = table.Column<int>(name: "Seqüência da Transportadora", type: "int", nullable: false),
                    DatadeEntrega = table.Column<DateTime>(name: "Data de Entrega", type: "datetime", nullable: true),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    Det1 = table.Column<string>(name: "Det 1", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Det2 = table.Column<string>(name: "Det 2", type: "text", nullable: false, defaultValue: ""),
                    SeriedoEquipamento = table.Column<string>(name: "Serie do Equipamento", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    DescrdoMaterial = table.Column<string>(name: "Descr do Material", type: "text", nullable: true, defaultValue: ""),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Stat = table.Column<string>(type: "varchar(19)", unicode: false, maxLength: 19, nullable: false, defaultValue: ""),
                    VendaFechada = table.Column<bool>(name: "Venda Fechada", type: "bit", nullable: false),
                    Telefone = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Follow Up", x => x.SeqFollowUp);
                });

            migrationBuilder.CreateTable(
                name: "Grupo da Despesa",
                columns: table => new
                {
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência Grupo Despesa", x => x.SeqüênciaGrupoDespesa);
                });

            migrationBuilder.CreateTable(
                name: "Grupo do Produto",
                columns: table => new
                {
                    SeqüênciadoGrupoProduto = table.Column<short>(name: "Seqüência do Grupo Produto", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Grupo Produto", x => x.SeqüênciadoGrupoProduto);
                });

            migrationBuilder.CreateTable(
                name: "Hidroturbos Vendidos",
                columns: table => new
                {
                    SeqdoHidroturbo = table.Column<int>(name: "Seq do Hidroturbo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelodoHidroturbo = table.Column<string>(name: "Modelo do Hidroturbo", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Cidade = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_hidro_ven", x => x.SeqdoHidroturbo);
                });

            migrationBuilder.CreateTable(
                name: "Historico Contabil",
                columns: table => new
                {
                    CodigodoHistorico = table.Column<short>(name: "Codigo do Historico", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do Historico", x => x.CodigodoHistorico);
                });

            migrationBuilder.CreateTable(
                name: "Histórico da Conta Corrente",
                columns: table => new
                {
                    SeqüênciadoHistórico = table.Column<short>(name: "Seqüência do Histórico", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Histórico", x => x.SeqüênciadoHistórico);
                });

            migrationBuilder.CreateTable(
                name: "ICMS",
                columns: table => new
                {
                    SeqüênciadoICMS = table.Column<short>(name: "Seqüência do ICMS", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    Regiao = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    PorcentagemdeICMS = table.Column<decimal>(name: "Porcentagem de ICMS", type: "decimal(5,2)", nullable: false),
                    AlíquotaInterEstadual = table.Column<decimal>(name: "Alíquota InterEstadual", type: "decimal(5,2)", nullable: false),
                    CódigodaUF = table.Column<short>(name: "Código da UF", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do ICMS", x => x.SeqüênciadoICMS);
                });

            migrationBuilder.CreateTable(
                name: "Importação",
                columns: table => new
                {
                    ÚltimaAgência = table.Column<int>(name: "Última Agência", type: "int", nullable: false),
                    ÚltimaBaixaReceber = table.Column<int>(name: "Última Baixa Receber", type: "int", nullable: false),
                    ÚltimaBaixaPagar = table.Column<int>(name: "Última Baixa Pagar", type: "int", nullable: false),
                    ÚltimaClassificaçãoFiscal = table.Column<int>(name: "Última Classificação Fiscal", type: "int", nullable: false),
                    ÚltimoConjunto = table.Column<int>(name: "Último Conjunto", type: "int", nullable: false),
                    ÚltimoDadosAdicionais = table.Column<int>(name: "Último Dados Adicionais", type: "int", nullable: false),
                    ÚltimaEntradaReceber = table.Column<int>(name: "Última Entrada Receber", type: "int", nullable: false),
                    ÚltimaEntradaPagar = table.Column<int>(name: "Última Entrada Pagar", type: "int", nullable: false),
                    ÚltimoCliente = table.Column<int>(name: "Último Cliente", type: "int", nullable: false),
                    ÚltimoFornecedor = table.Column<int>(name: "Último Fornecedor", type: "int", nullable: false),
                    ÚltimoVendedor = table.Column<int>(name: "Último Vendedor", type: "int", nullable: false),
                    ÚltimoGrupodaDespesa = table.Column<int>(name: "Último Grupo da Despesa", type: "int", nullable: false),
                    ÚltimoGrupodoProduto = table.Column<int>(name: "Último Grupo do Produto", type: "int", nullable: false),
                    ÚltimoHistóricodaCC = table.Column<int>(name: "Último Histórico da CC", type: "int", nullable: false),
                    ÚltimoICMS = table.Column<int>(name: "Último ICMS", type: "int", nullable: false),
                    ÚltimaManutençãoPagar = table.Column<int>(name: "Última Manutenção Pagar", type: "int", nullable: false),
                    ÚltimaManutençãoReceber = table.Column<int>(name: "Última Manutenção Receber", type: "int", nullable: false),
                    ÚltimoMovimentodaCC = table.Column<int>(name: "Último Movimento da CC", type: "int", nullable: false),
                    ÚltimaCidade = table.Column<int>(name: "Última Cidade", type: "int", nullable: false),
                    ÚltimaNaturezadeOperação = table.Column<int>(name: "Última Natureza de Operação", type: "int", nullable: false),
                    ÚltimoProduto = table.Column<int>(name: "Último Produto", type: "int", nullable: false),
                    ÚltimoServiço = table.Column<int>(name: "Último Serviço", type: "int", nullable: false),
                    ÚltimaTabelaA = table.Column<int>(name: "Última Tabela A", type: "int", nullable: false),
                    ÚltimaTabelaB = table.Column<int>(name: "Última Tabela B", type: "int", nullable: false),
                    ÚltimaCobrança = table.Column<int>(name: "Última Cobrança", type: "int", nullable: false),
                    ÚltimaUnidade = table.Column<int>(name: "Última Unidade", type: "int", nullable: false),
                    ÚltimoAcertonoEstoque = table.Column<int>(name: "Último Acerto no Estoque", type: "int", nullable: false),
                    ÚltimaEntradanoEstoque = table.Column<int>(name: "Última Entrada no Estoque", type: "int", nullable: false),
                    ÚltimaEntradaReceita = table.Column<int>(name: "Última Entrada Receita", type: "int", nullable: false),
                    ÚltimoMovimentoEstoque = table.Column<int>(name: "Último Movimento Estoque", type: "int", nullable: false),
                    ÚltimoMovimentoEstoqueConj = table.Column<int>(name: "Último Movimento Estoque Conj", type: "int", nullable: false),
                    ÚltimaRequisição = table.Column<int>(name: "Última Requisição", type: "int", nullable: false),
                    ÚltimaEntradaContábil = table.Column<int>(name: "Última Entrada Contábil", type: "int", nullable: false),
                    ÚltimaNotaFiscal = table.Column<int>(name: "Última Nota Fiscal", type: "int", nullable: false),
                    ÚltimoOrçamento = table.Column<int>(name: "Último Orçamento", type: "int", nullable: false),
                    ÚltimaOrdemdeServiço = table.Column<int>(name: "Última Ordem de Serviço", type: "int", nullable: false),
                    ÚltimoPedido = table.Column<int>(name: "Último Pedido", type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Importação Conjuntos Estoque",
                columns: table => new
                {
                    SeqüênciaImportaçãoEstoque = table.Column<int>(name: "Seqüência Importação Estoque", type: "int", nullable: false),
                    SeqüênciaImportaçãoÍtem = table.Column<int>(name: "Seqüência Importação Ítem", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Importação Estoque Seq Con", x => new { x.SeqüênciaImportaçãoEstoque, x.SeqüênciaImportaçãoÍtem });
                });

            migrationBuilder.CreateTable(
                name: "Importação Estoque",
                columns: table => new
                {
                    SeqüênciaImportaçãoEstoque = table.Column<int>(name: "Seqüência Importação Estoque", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência Importação Estoque", x => x.SeqüênciaImportaçãoEstoque);
                });

            migrationBuilder.CreateTable(
                name: "Inutilização NFe",
                columns: table => new
                {
                    SeqüênciadaInutilização = table.Column<int>(name: "Seqüência da Inutilização", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<short>(type: "smallint", nullable: false),
                    Justificativa = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Ambiente = table.Column<short>(type: "smallint", nullable: false),
                    FaixaInicial = table.Column<int>(name: "Faixa Inicial", type: "int", nullable: false),
                    FaixaFinal = table.Column<int>(name: "Faixa Final", type: "int", nullable: false),
                    DatadaInutilização = table.Column<DateTime>(name: "Data da Inutilização", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Inutilização", x => x.SeqüênciadaInutilização);
                });

            migrationBuilder.CreateTable(
                name: "Inventario Pdf",
                columns: table => new
                {
                    CodigodoPdf = table.Column<string>(name: "Codigo do Pdf", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Decricao = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    Unid = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ValorContabilPdf = table.Column<decimal>(name: "Valor Contabil Pdf", type: "decimal(11,4)", nullable: false),
                    ValorTotalPdf = table.Column<decimal>(name: "Valor Total Pdf", type: "decimal(12,2)", nullable: false),
                    DataBase = table.Column<string>(name: "Data Base", type: "varchar(100)", unicode: false, maxLength: 100, nullable: true, defaultValue: ""),
                    SeqItem = table.Column<int>(type: "int", nullable: false),
                    TipodoProduto = table.Column<short>(name: "Tipo do Produto", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do Pdf", x => x.CodigodoPdf);
                });

            migrationBuilder.CreateTable(
                name: "Itens da Correcao",
                columns: table => new
                {
                    SequenciadaCorreção = table.Column<int>(name: "Sequencia da Correção", type: "int", nullable: false),
                    SequenciadoProduto = table.Column<int>(name: "Sequencia do Produto", type: "int", nullable: false),
                    DatadoEstoque = table.Column<DateTime>(name: "Data do Estoque", type: "datetime", nullable: true),
                    QuantidadePositiva = table.Column<decimal>(name: "Quantidade Positiva", type: "decimal(11,4)", nullable: false),
                    QuantidadeNegativa = table.Column<decimal>(name: "Quantidade Negativa", type: "decimal(11,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SeqCorrecao_Item", x => new { x.SequenciadaCorreção, x.SequenciadoProduto });
                });

            migrationBuilder.CreateTable(
                name: "Itens da Ordem",
                columns: table => new
                {
                    IddaOrdem = table.Column<int>(name: "Id da Ordem", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    AliquotadoIPI = table.Column<decimal>(name: "Aliquota do IPI", type: "decimal(8,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id da Ordem", x => new { x.IddaOrdem, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Itens da Produção",
                columns: table => new
                {
                    SequenciadaProdução = table.Column<int>(name: "Sequencia da Produção", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    SeqüênciadaMatériaPrima = table.Column<int>(name: "Seqüência da Matéria Prima", type: "int", nullable: false),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    DatadaProdução = table.Column<DateTime>(name: "Data da Produção", type: "datetime", nullable: true),
                    Naocalcula = table.Column<bool>(name: "Nao calcula", type: "bit", nullable: false),
                    Japroduziu = table.Column<bool>(name: "Ja produziu", type: "bit", nullable: false),
                    Dtfinal = table.Column<DateTime>(name: "Dt final", type: "datetime", nullable: true),
                    Iniserra = table.Column<DateTime>(name: "Ini serra", type: "datetime", nullable: true),
                    Fimserra = table.Column<DateTime>(name: "Fim serra", type: "datetime", nullable: true),
                    Horainiserra = table.Column<DateTime>(name: "Hora ini serra", type: "datetime", nullable: true),
                    Horafimserra = table.Column<DateTime>(name: "Hora fim serra", type: "datetime", nullable: true),
                    Datainicialoxicorte = table.Column<DateTime>(name: "Data inicial oxicorte", type: "datetime", nullable: true),
                    Horainioxi = table.Column<DateTime>(name: "Hora ini oxi", type: "datetime", nullable: true),
                    Datafimoxicorte = table.Column<DateTime>(name: "Data fim oxicorte", type: "datetime", nullable: true),
                    Horafimoxi = table.Column<DateTime>(name: "Hora fim oxi", type: "datetime", nullable: true),
                    Dtiniguilhotina = table.Column<DateTime>(name: "Dt ini guilhotina", type: "datetime", nullable: true),
                    Horainigui = table.Column<DateTime>(name: "Hora ini gui", type: "datetime", nullable: true),
                    Horafimgui = table.Column<DateTime>(name: "Hora fim gui", type: "datetime", nullable: true),
                    Dtfimgui = table.Column<DateTime>(name: "Dt fim gui", type: "datetime", nullable: true),
                    Operadorserra = table.Column<string>(name: "Operador serra", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Operadoroxi = table.Column<string>(name: "Operador oxi", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Operadorgui = table.Column<string>(name: "Operador gui", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Operadordobra = table.Column<string>(name: "Operador dobra", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Operadorcalandra = table.Column<string>(name: "Operador calandra", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Operadorperfiladeira = table.Column<string>(name: "Operador perfiladeira", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Opeardortorno = table.Column<string>(name: "Opeardor torno", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("seq_e_item_producao", x => new { x.SequenciadaProdução, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Itens da Viagem",
                columns: table => new
                {
                    SeqdaViagem = table.Column<int>(name: "Seq da Viagem", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    DescriçãodoItem = table.Column<string>(name: "Descrição do Item", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    ValordoItem = table.Column<decimal>(name: "Valor do Item", type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_e_Item_Viagem", x => new { x.SeqdaViagem, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Itens pendentes",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    Tp = table.Column<short>(type: "smallint", nullable: false),
                    Situacao = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_orcc_e_item", x => new { x.SeqüênciadoOrçamento, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Itens Saidas Balcao",
                columns: table => new
                {
                    SequenciadaSaida = table.Column<int>(name: "Sequencia da Saida", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    Consignado = table.Column<bool>(type: "bit", nullable: false),
                    Seqprincipal = table.Column<int>(name: "Seq principal", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_B_Item", x => new { x.SequenciadaSaida, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "IVA From UFs",
                columns: table => new
                {
                    IDMVA = table.Column<int>(name: "ID MVA", type: "int", nullable: false),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    NCM = table.Column<int>(type: "int", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    Teste = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID MVA", x => new { x.IDMVA, x.UF, x.NCM });
                });

            migrationBuilder.CreateTable(
                name: "LancamentoBancarioBB",
                columns: table => new
                {
                    SequenciaLancamentoBB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequenciaDaAgencia = table.Column<int>(type: "int", nullable: true),
                    SequenciaDaCCDaAgencia = table.Column<int>(type: "int", nullable: true),
                    DataLancamento = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataMovimento = table.Column<DateTime>(type: "datetime", nullable: true),
                    Valor = table.Column<decimal>(type: "money", nullable: false),
                    TipoLancamento = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    CodigoHistorico = table.Column<int>(type: "int", nullable: false),
                    TextoDescricaoHistorico = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    NumeroDocumento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CpfCnpj = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    NomeDevedor = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    IndicadorCheque = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    NumeroCheque = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Processado = table.Column<bool>(type: "bit", nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "datetime", nullable: true),
                    SequenciaDaBaixaGerada = table.Column<int>(type: "int", nullable: true),
                    SequenciaManutencaoVinculada = table.Column<int>(type: "int", nullable: true),
                    MotivoNaoProcessado = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    PedidoIdentificado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DataImportacao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DadosOriginaisJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentoBancarioBB", x => x.SequenciaLancamentoBB);
                });

            migrationBuilder.CreateTable(
                name: "Lançamentos Contabil",
                columns: table => new
                {
                    IddoLançamento = table.Column<int>(name: "Id do Lançamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtdoLançamento = table.Column<string>(name: "Dt do Lançamento", type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, defaultValue: ""),
                    ContaDebito = table.Column<int>(name: "Conta Debito", type: "int", nullable: false),
                    ContaCredito = table.Column<int>(name: "Conta Credito", type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CodigodoHistorico = table.Column<short>(name: "Codigo do Historico", type: "smallint", nullable: false),
                    ComplementodoHist = table.Column<string>(name: "Complemento do Hist", type: "text", nullable: false, defaultValue: ""),
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false),
                    SeqüênciadaMovimentaçãoCC = table.Column<int>(name: "Seqüência da Movimentação CC", type: "int", nullable: false),
                    DatadaBaixa = table.Column<DateTime>(name: "Data da Baixa", type: "datetime", nullable: true),
                    Gerado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id do Lançamento", x => x.IddoLançamento);
                });

            migrationBuilder.CreateTable(
                name: "Lances do Pivo",
                columns: table => new
                {
                    ModelodoLance = table.Column<int>(name: "Modelo do Lance", type: "int", nullable: false),
                    DescriçãodoLance = table.Column<string>(name: "Descrição do Lance", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    LarguradoLance = table.Column<decimal>(name: "Largura do Lance", type: "decimal(8,2)", nullable: false),
                    DiametrodoLance = table.Column<decimal>(name: "Diametro do Lance", type: "decimal(8,2)", nullable: false),
                    QtdedeSpray = table.Column<short>(name: "Qtde de Spray", type: "smallint", nullable: false),
                    Inicial = table.Column<bool>(type: "bit", nullable: false),
                    Inter = table.Column<bool>(type: "bit", nullable: false),
                    Penultimo = table.Column<bool>(type: "bit", nullable: false),
                    Final = table.Column<bool>(type: "bit", nullable: false),
                    CA1 = table.Column<short>(type: "smallint", nullable: false),
                    CA2 = table.Column<short>(type: "smallint", nullable: false),
                    CA3 = table.Column<short>(type: "smallint", nullable: false),
                    CA4 = table.Column<short>(type: "smallint", nullable: false),
                    CA5 = table.Column<short>(type: "smallint", nullable: false),
                    CA6 = table.Column<short>(type: "smallint", nullable: false),
                    CA7 = table.Column<short>(type: "smallint", nullable: false),
                    CA8 = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Modelo do Lance", x => new { x.ModelodoLance, x.DescriçãodoLance });
                });

            migrationBuilder.CreateTable(
                name: "Licitacao",
                columns: table => new
                {
                    SequenciadaLicitacao = table.Column<int>(name: "Sequencia da Licitacao", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadaLicitacao = table.Column<DateTime>(name: "Data da Licitacao", type: "datetime", nullable: false),
                    For1 = table.Column<string>(name: "For 1", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Contato1 = table.Column<string>(name: "Contato 1", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Fone1 = table.Column<string>(name: "Fone 1", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    PrevEntrega1 = table.Column<string>(name: "Prev Entrega 1", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    CondPagto1 = table.Column<string>(name: "Cond Pagto 1", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    For2 = table.Column<string>(name: "For 2", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Contato2 = table.Column<string>(name: "Contato 2", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Fone2 = table.Column<string>(name: "Fone 2", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    PrevEntrega2 = table.Column<string>(name: "Prev Entrega 2", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    CondPagto2 = table.Column<string>(name: "Cond Pagto 2", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    For3 = table.Column<string>(name: "For 3", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Contato3 = table.Column<string>(name: "Contato 3", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Fone3 = table.Column<string>(name: "Fone 3", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    PrevEntrega3 = table.Column<string>(name: "Prev Entrega 3", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    CondPagto3 = table.Column<string>(name: "Cond Pagto 3", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Fechado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Licitacao", x => x.SequenciadaLicitacao);
                });

            migrationBuilder.CreateTable(
                name: "LogProcessamentoIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHora = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Nivel = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Categoria = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Mensagem = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    Detalhes = table.Column<string>(type: "text", nullable: true),
                    SequenciaLancamentoBB = table.Column<int>(type: "int", nullable: true),
                    SequenciaDaBaixa = table.Column<int>(type: "int", nullable: true),
                    SequenciaDaManutencao = table.Column<int>(type: "int", nullable: true),
                    StackTrace = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogProcessamentoIntegracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manutenção Hidroturbo",
                columns: table => new
                {
                    SeqdoHidroturbo = table.Column<int>(name: "Seq do Hidroturbo", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    DatadaManutenção = table.Column<DateTime>(name: "Data da Manutenção", type: "datetime", nullable: true),
                    DescriçãodaManutenção = table.Column<string>(name: "Descrição da Manutenção", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SeqH_e_Item", x => new { x.SeqdoHidroturbo, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Manutenção Pivo",
                columns: table => new
                {
                    SeqdoPivo = table.Column<int>(name: "Seq do Pivo", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    DatadaManutenção = table.Column<DateTime>(name: "Data da Manutenção", type: "datetime", nullable: true),
                    DescriçãodaManutenção = table.Column<string>(name: "Descrição da Manutenção", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SeqPivo_e_Item", x => new { x.SeqdoPivo, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Mapa da Vazao",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    DistanciaaoCentro = table.Column<decimal>(name: "Distancia ao Centro", type: "decimal(8,2)", nullable: false),
                    VazaonaSaida = table.Column<decimal>(name: "Vazao na Saida", type: "decimal(8,4)", nullable: false),
                    VazaoAcumulada = table.Column<decimal>(name: "Vazao Acumulada", type: "decimal(8,4)", nullable: false),
                    VazaoTrecho = table.Column<decimal>(name: "Vazao Trecho", type: "decimal(8,3)", nullable: false),
                    DN = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PerdaCarga = table.Column<decimal>(name: "Perda Carga", type: "decimal(8,4)", nullable: false),
                    VelocidadeTrecho = table.Column<decimal>(name: "Velocidade Trecho", type: "decimal(8,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Projeto_Vazao", x => new { x.SequenciadoProjeto, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Materia prima orçamento",
                columns: table => new
                {
                    SequenciadaExpedição = table.Column<int>(name: "Sequencia da Expedição", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    SigladaUnidade = table.Column<string>(name: "Sigla da Unidade", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    Localizacao = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    Imprimir = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqexpedicao", x => x.SequenciadaExpedição);
                });

            migrationBuilder.CreateTable(
                name: "Material Expedição",
                columns: table => new
                {
                    SequenciadaExpedição = table.Column<int>(name: "Sequencia da Expedição", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    SigladaUnidade = table.Column<string>(name: "Sigla da Unidade", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    Localizacao = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    Peso = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Expedição", x => x.SequenciadaExpedição);
                });

            migrationBuilder.CreateTable(
                name: "Motoristas",
                columns: table => new
                {
                    CodigodoMotorista = table.Column<short>(name: "Codigo do Motorista", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomedoMotorista = table.Column<string>(name: "Nome do Motorista", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    RG = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    CPF = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    Endereco = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Numero = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    Bairro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    Municipio = table.Column<int>(type: "int", nullable: false),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    Fone = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false, defaultValue: ""),
                    CEL = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do Motorista", x => x.CodigodoMotorista);
                });

            migrationBuilder.CreateTable(
                name: "Movimento Contábil Novo",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadoMovimento = table.Column<DateTime>(name: "Data do Movimento", type: "datetime", nullable: true),
                    TipodoMovimento = table.Column<short>(name: "Tipo do Movimento", type: "smallint", nullable: false),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Devolucao = table.Column<bool>(type: "bit", nullable: false),
                    SeqProdPropria = table.Column<int>(name: "Seq Prod Propria", type: "int", nullable: false),
                    EProduçãoPropria = table.Column<bool>(name: "E Produção Propria", type: "bit", nullable: false),
                    BaixaConsumo = table.Column<bool>(name: "Baixa Consumo", type: "bit", nullable: false),
                    SeqBaixaConsumo = table.Column<int>(name: "Seq Baixa Consumo", type: "int", nullable: false),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosProdutos = table.Column<decimal>(name: "Valor Total dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosProdutos = table.Column<decimal>(name: "Valor Total IPI dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoMovimento = table.Column<decimal>(name: "Valor Total do Movimento", type: "decimal(11,2)", nullable: false),
                    DatadaAlteração = table.Column<DateTime>(name: "Data da Alteração", type: "datetime", nullable: true),
                    HoradaAlteração = table.Column<DateTime>(name: "Hora da Alteração", type: "datetime", nullable: true),
                    UsuáriodaAlteração = table.Column<string>(name: "Usuário da Alteração", type: "varchar(60)", unicode: false, maxLength: 60, nullable: true, defaultValue: ""),
                    ValorTotaldasDespesas = table.Column<decimal>(name: "Valor Total das Despesas", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdasDespesas = table.Column<decimal>(name: "Valor Total IPI das Despesas", type: "decimal(11,2)", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Fechado = table.Column<bool>(type: "bit", nullable: false),
                    SequenciadaCompra = table.Column<int>(name: "Sequencia da Compra", type: "int", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    CodigodoDebito = table.Column<int>(name: "Codigo do Debito", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto Contabil Novo", x => x.SeqüênciadoMovimento);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    SeqüênciadoMunicípio = table.Column<int>(name: "Seqüência do Município", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    CódigodoIBGE = table.Column<int>(name: "Código do IBGE", type: "int", nullable: false),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Município", x => x.SeqüênciadoMunicípio);
                });

            migrationBuilder.CreateTable(
                name: "Municipios dos Revendedores",
                columns: table => new
                {
                    SequenciadaRevenda = table.Column<int>(name: "Sequencia da Revenda", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    IddaConta = table.Column<int>(name: "Id da Conta", type: "int", nullable: false),
                    Reg = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false, defaultValue: ""),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    SeqüênciadoMunicípio = table.Column<int>(name: "Seqüência do Município", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_e_Revendedor", x => new { x.SequenciadaRevenda, x.SequenciadoItem, x.IddaConta });
                });

            migrationBuilder.CreateTable(
                name: "MVA",
                columns: table => new
                {
                    IDMVA = table.Column<int>(name: "ID MVA", type: "int", nullable: false),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID_UF", x => new { x.IDMVA, x.UF });
                });

            migrationBuilder.CreateTable(
                name: "Mvto Conta do Vendedor",
                columns: table => new
                {
                    IddoMovimento = table.Column<int>(name: "Id do Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtadoMovimento = table.Column<DateTime>(name: "Dta do Movimento", type: "datetime", nullable: false),
                    IdConta = table.Column<int>(name: "Id Conta", type: "int", nullable: false),
                    ValorEntrada = table.Column<decimal>(name: "Valor Entrada", type: "decimal(10,2)", nullable: false),
                    ValorSaida = table.Column<decimal>(name: "Valor Saida", type: "decimal(10,2)", nullable: false),
                    Historico = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Informativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id do Movimento", x => x.IddoMovimento);
                });

            migrationBuilder.CreateTable(
                name: "Natureza de Operação",
                columns: table => new
                {
                    SeqüênciadaNatureza = table.Column<short>(name: "Seqüência da Natureza", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CódigodaNaturezadeOperação = table.Column<int>(name: "Código da Natureza de Operação", type: "int", nullable: false),
                    DescriçãodaNaturezaOperação = table.Column<string>(name: "Descrição da Natureza Operação", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Natureza", x => x.SeqüênciadaNatureza);
                });

            migrationBuilder.CreateTable(
                name: "Ocorrencias Garantia",
                columns: table => new
                {
                    SequenciadoControle = table.Column<int>(name: "Sequencia do Controle", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    DatadaOcorrencia = table.Column<DateTime>(name: "Data da Ocorrencia", type: "datetime", nullable: true),
                    DataSaida = table.Column<DateTime>(name: "Data Saida", type: "datetime", nullable: true),
                    NúmerodaNFe = table.Column<int>(name: "Número da NFe", type: "int", nullable: false),
                    DatadoRetorno = table.Column<DateTime>(name: "Data do Retorno", type: "datetime", nullable: true),
                    DatadeValidade = table.Column<DateTime>(name: "Data de Validade", type: "datetime", nullable: true),
                    Ocorrencia = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false, defaultValue: ""),
                    UltFornecedor = table.Column<int>(name: "Ult Fornecedor", type: "int", nullable: false),
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    NotasdaCompra = table.Column<string>(name: "Notas da Compra", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_Prod_Controle", x => new { x.SequenciadoControle, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Orçamentos da compra",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id_orc", x => new { x.IddoPedido, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Ordem de Montagem",
                columns: table => new
                {
                    SequenciadaMontagem = table.Column<int>(name: "Sequencia da Montagem", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origem = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    SequenciadaOrigem = table.Column<int>(name: "Sequencia da Origem", type: "int", nullable: false),
                    CodigodoGeral = table.Column<int>(name: "Codigo do Geral", type: "int", nullable: false),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false),
                    SequenciadaOrigem2 = table.Column<int>(name: "Sequencia da Origem 2", type: "int", nullable: false),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    ValorTotaldosProdutos = table.Column<decimal>(name: "Valor Total dos Produtos", type: "decimal(11,2)", nullable: false),
                    TotaldeIpi = table.Column<decimal>(name: "Total de Ipi", type: "decimal(10,2)", nullable: false),
                    ValorTotaldosServiços = table.Column<decimal>(name: "Valor Total dos Serviços", type: "decimal(11,2)", nullable: false),
                    TotaldaOrdem = table.Column<decimal>(name: "Total da Ordem", type: "decimal(10,2)", nullable: false),
                    KmIni = table.Column<decimal>(name: "Km Ini", type: "decimal(8,2)", nullable: false),
                    KmFinal = table.Column<decimal>(name: "Km Final", type: "decimal(8,2)", nullable: false),
                    TotalKm = table.Column<decimal>(name: "Total Km", type: "decimal(9,2)", nullable: false),
                    VrKm = table.Column<decimal>(name: "Vr Km", type: "decimal(8,2)", nullable: false),
                    VrTotalKm = table.Column<decimal>(name: "Vr Total Km", type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Montagem", x => x.SequenciadaMontagem);
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    SeqüênciadoPaís = table.Column<int>(name: "Seqüência do País", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    CódigodoPaís = table.Column<short>(name: "Código do País", type: "smallint", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do País", x => x.SeqüênciadoPaís);
                });

            migrationBuilder.CreateTable(
                name: "Parametros",
                columns: table => new
                {
                    CaminhoAtualização = table.Column<string>(name: "Caminho Atualização", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    CaminhoAtualização2 = table.Column<string>(name: "Caminho Atualização 2", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    NomedoServidor = table.Column<string>(name: "Nome do Servidor", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    DiretoriodasFotos = table.Column<string>(name: "Diretorio das Fotos", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    DiretorioFotosConjuntos = table.Column<string>(name: "Diretorio Fotos Conjuntos", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    DiretorioFotosProdutos = table.Column<string>(name: "Diretorio Fotos Produtos", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    DiretorioDesenhoTec = table.Column<string>(name: "Diretorio Desenho Tec", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Parâmetros da Contabilidade",
                columns: table => new
                {
                    AnoContábil = table.Column<short>(name: "Ano Contábil", type: "smallint", nullable: false),
                    TrimestreContábil = table.Column<string>(name: "Trimestre Contábil", type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Parâmetros da NFe",
                columns: table => new
                {
                    Ambiente = table.Column<short>(type: "smallint", nullable: false),
                    Diretório1NFeHomologação = table.Column<string>(name: "Diretório 1 NFe Homologação", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Diretório2NFeHomologação = table.Column<string>(name: "Diretório 2 NFe Homologação", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Diretório1NFeProdução = table.Column<string>(name: "Diretório 1 NFe Produção", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Diretório2NFeProdução = table.Column<string>(name: "Diretório 2 NFe Produção", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Diretório1NFSeHomologação = table.Column<string>(name: "Diretório 1 NFSe Homologação", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Diretório2NFSeHomologação = table.Column<string>(name: "Diretório 2 NFSe Homologação", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Diretório1NFSeProdução = table.Column<string>(name: "Diretório 1 NFSe Produção", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Diretório2NFSeProdução = table.Column<string>(name: "Diretório 2 NFSe Produção", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    CertificadoDigital = table.Column<string>(name: "Certificado Digital", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Testemunha1 = table.Column<string>(name: "Testemunha 1", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Testemunha2 = table.Column<string>(name: "Testemunha 2", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    CPFTestemunha1 = table.Column<string>(name: "CPF Testemunha 1", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    CPFTestemunha2 = table.Column<string>(name: "CPF Testemunha 2", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    HorariodeVerao = table.Column<bool>(name: "Horario de Verao", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Parâmetros do Produto",
                columns: table => new
                {
                    PercentualAcréscimoProduto = table.Column<decimal>(name: "Percentual Acréscimo Produto", type: "decimal(5,2)", nullable: false),
                    PercentualAcréscimoConjunto = table.Column<decimal>(name: "Percentual Acréscimo Conjunto", type: "decimal(5,2)", nullable: false),
                    AcrescimodoParcelamento = table.Column<decimal>(name: "Acrescimo do Parcelamento", type: "decimal(8,4)", nullable: false),
                    Percentual2 = table.Column<decimal>(name: "Percentual 2", type: "decimal(6,2)", nullable: false),
                    Jaatualizou = table.Column<bool>(name: "Ja atualizou", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Parametros do SPED ECF",
                columns: table => new
                {
                    Versaosped = table.Column<string>(name: "Versao sped", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    NomedoContabilista = table.Column<string>(name: "Nome do Contabilista", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    CPFContabilista = table.Column<string>(name: "CPF Contabilista", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    CRC = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    CNPJ = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: false, defaultValue: ""),
                    Endereco = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    ComplementodoEndereço = table.Column<string>(name: "Complemento do Endereço", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    Bairro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    Fone = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false, defaultValue: ""),
                    Empresa = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Parcelas da Ordem",
                columns: table => new
                {
                    SequenciadaMontagem = table.Column<int>(name: "Sequencia da Montagem", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: true),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq1_e_pc1", x => new { x.SequenciadaMontagem, x.NúmerodaParcela });
                });

            migrationBuilder.CreateTable(
                name: "Parcelas da Viagem",
                columns: table => new
                {
                    SeqdaViagem = table.Column<int>(name: "Seq da Viagem", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: true),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_e_Pc", x => new { x.SeqdaViagem, x.NúmerodaParcela });
                });

            migrationBuilder.CreateTable(
                name: "Parcelas do Novo Pedido",
                columns: table => new
                {
                    CodigodoPedido = table.Column<int>(name: "Codigo do Pedido", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: true),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CodPedido_e_Pc", x => new { x.CodigodoPedido, x.NúmerodaParcela });
                });

            migrationBuilder.CreateTable(
                name: "Parcelas do Projeto",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SeqProjeto_e_Parcela", x => new { x.SequenciadoProjeto, x.NúmerodaParcela });
                });

            migrationBuilder.CreateTable(
                name: "Parcelas mvto contabil",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: true),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    SeqüênciadaCobrança = table.Column<short>(name: "Seqüência da Cobrança", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqcon_e_pc", x => new { x.SeqüênciadoMovimento, x.NúmerodaParcela });
                });

            migrationBuilder.CreateTable(
                name: "Parcelas Ped Compra Novo",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    SeqüênciadaCobrança = table.Column<short>(name: "Seqüência da Cobrança", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id e Parcela", x => new { x.IddoPedido, x.NúmerodaParcela });
                });

            migrationBuilder.CreateTable(
                name: "Peças do Projeto",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SequenciadoProduto = table.Column<int>(name: "Sequencia do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    PartedoPivo = table.Column<string>(name: "Parte do Pivo", type: "varchar(29)", unicode: false, maxLength: 29, nullable: false, defaultValue: ""),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SeqProjeto_e_Item", x => new { x.SequenciadoProjeto, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Pedido de Compra Novo",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadoPedido = table.Column<DateTime>(name: "Data do Pedido", type: "datetime", nullable: false),
                    NrodaLicitação = table.Column<string>(name: "Nro da Licitação", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    CodigodoFornecedor = table.Column<int>(name: "Codigo do Fornecedor", type: "int", nullable: false),
                    CodigodaTransportadora = table.Column<int>(name: "Codigo da Transportadora", type: "int", nullable: false),
                    Comprador = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Vend = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Prazo = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    CifFob = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    VrdoFrete = table.Column<decimal>(name: "Vr do Frete", type: "decimal(10,2)", nullable: false),
                    VrdoDesconto = table.Column<decimal>(name: "Vr do Desconto", type: "decimal(10,2)", nullable: false),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    TotaldosProdutos = table.Column<decimal>(name: "Total dos Produtos", type: "decimal(10,2)", nullable: false),
                    TotaldasDespesas = table.Column<decimal>(name: "Total das Despesas", type: "decimal(10,2)", nullable: false),
                    TotaldoIPI = table.Column<decimal>(name: "Total do IPI", type: "decimal(10,2)", nullable: false),
                    TotaldoICMS = table.Column<decimal>(name: "Total do ICMS", type: "decimal(10,2)", nullable: false),
                    TotaldoPedido = table.Column<decimal>(name: "Total do Pedido", type: "decimal(10,2)", nullable: false),
                    EnderecodeEntrega = table.Column<string>(name: "Endereco de Entrega", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    NumerodoEndereco = table.Column<string>(name: "Numero do Endereco", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    BairrodeEntrega = table.Column<string>(name: "Bairro de Entrega", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    CidadedeEntrega = table.Column<string>(name: "Cidade de Entrega", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    UFDeEntrega = table.Column<string>(name: "UF De Entrega", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    CEPdeEntrega = table.Column<string>(name: "CEP de Entrega", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    FonedeEntrega = table.Column<string>(name: "Fone de Entrega", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ContatodeEntrega = table.Column<string>(name: "Contato de Entrega", type: "varchar(45)", unicode: false, maxLength: 45, nullable: false, defaultValue: ""),
                    PrevisaodeEntrega = table.Column<DateTime>(name: "Previsao de Entrega", type: "datetime", nullable: true),
                    PedidoFechado = table.Column<bool>(name: "Pedido Fechado", type: "bit", nullable: false),
                    Validado = table.Column<bool>(type: "bit", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CodigodaLicitação = table.Column<int>(name: "Codigo da Licitação", type: "int", nullable: false),
                    NomedoBanco1 = table.Column<string>(name: "Nome do Banco 1", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    AgênciadoBanco1 = table.Column<string>(name: "Agência do Banco 1", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ContaCorrentedoBanco1 = table.Column<string>(name: "Conta Corrente do Banco 1", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    NomedoCorrentistadoBanco1 = table.Column<string>(name: "Nome do Correntista do Banco 1", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Prepedido = table.Column<bool>(type: "bit", nullable: false),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    JustificaroAtraso = table.Column<string>(name: "Justificar o Atraso", type: "varchar(150)", unicode: false, maxLength: 150, nullable: false, defaultValue: ""),
                    NovaPrevisao = table.Column<DateTime>(name: "Nova Previsao", type: "datetime", nullable: true),
                    Dias = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id do Pedido", x => x.IddoPedido);
                });

            migrationBuilder.CreateTable(
                name: "Pivos Vendidos",
                columns: table => new
                {
                    SeqdoPivo = table.Column<int>(name: "Seq do Pivo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelodoPivo = table.Column<string>(name: "Modelo do Pivo", type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, defaultValue: ""),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Cidade = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    UF = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SeqPivoAux", x => x.SeqdoPivo);
                });

            migrationBuilder.CreateTable(
                name: "Pneus",
                columns: table => new
                {
                    SequenciadoPneu = table.Column<int>(name: "Sequencia do Pneu", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelodoPneu = table.Column<string>(name: "Modelo do Pneu", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Velocidade = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Pneu", x => x.SequenciadoPneu);
                });

            migrationBuilder.CreateTable(
                name: "Previsoes de Pagtos",
                columns: table => new
                {
                    SequenciadaPrevisao = table.Column<int>(name: "Sequencia da Previsao", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaldoIPG = table.Column<decimal>(name: "Saldo IPG", type: "decimal(10,2)", nullable: false),
                    SaldoChinellato = table.Column<decimal>(name: "Saldo Chinellato", type: "decimal(10,2)", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: true),
                    DatadeEntrada = table.Column<DateTime>(name: "Data de Entrada", type: "datetime", nullable: true),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Parcela = table.Column<short>(type: "smallint", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    ValorPrevisto = table.Column<decimal>(name: "Valor Previsto", type: "decimal(12,2)", nullable: false),
                    Imprimir = table.Column<bool>(type: "bit", nullable: false),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    NomedaEmpresa = table.Column<string>(name: "Nome da Empresa", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    ValorRestante = table.Column<decimal>(name: "Valor Restante", type: "decimal(11,2)", nullable: false),
                    TpPagto = table.Column<string>(name: "Tp Pagto", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Previsao", x => x.SequenciadaPrevisao);
                });

            migrationBuilder.CreateTable(
                name: "Projeto de Irrigação",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequenciadoGeral = table.Column<int>(name: "Sequencia do Geral", type: "int", nullable: false),
                    Proposta = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Opcao = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    DatadaProposta = table.Column<DateTime>(name: "Data da Proposta", type: "datetime", nullable: true),
                    SequenciadaPropriedade = table.Column<int>(name: "Sequencia da Propriedade", type: "int", nullable: false),
                    DescriçãodoEquipamento = table.Column<string>(name: "Descrição do Equipamento", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    LanceemBalanço = table.Column<string>(name: "Lance em Balanço", type: "varchar(2)", unicode: false, maxLength: 2, nullable: false, defaultValue: ""),
                    ExtensãoUltSpray = table.Column<decimal>(name: "Extensão Ult Spray", type: "decimal(8,2)", nullable: false),
                    AlcanceSprayFim = table.Column<decimal>(name: "Alcance Spray Fim", type: "decimal(8,2)", nullable: false),
                    NPosicoes = table.Column<decimal>(name: "N Posicoes", type: "decimal(8,2)", nullable: false),
                    Graus = table.Column<short>(type: "smallint", nullable: false),
                    LaminaBruta = table.Column<decimal>(name: "Lamina Bruta", type: "decimal(8,2)", nullable: false),
                    TempoMaxOpera = table.Column<decimal>(name: "Tempo Max Opera", type: "decimal(8,2)", nullable: false),
                    ModeloTrechoA = table.Column<int>(name: "Modelo Trecho A", type: "int", nullable: false),
                    ModeloTrechoB = table.Column<int>(name: "Modelo Trecho B", type: "int", nullable: false),
                    ModeloTrechoC = table.Column<int>(name: "Modelo Trecho C", type: "int", nullable: false),
                    ModeloTrechoD = table.Column<int>(name: "Modelo Trecho D", type: "int", nullable: false),
                    Com1 = table.Column<decimal>(name: "Com 1", type: "decimal(8,2)", nullable: false),
                    Com2 = table.Column<decimal>(name: "Com 2", type: "decimal(8,2)", nullable: false),
                    Com3 = table.Column<decimal>(name: "Com 3", type: "decimal(8,2)", nullable: false),
                    Com4 = table.Column<decimal>(name: "Com 4", type: "decimal(8,2)", nullable: false),
                    SequenciadoAutotrafo = table.Column<int>(name: "Sequencia do Autotrafo", type: "int", nullable: false),
                    SaidasAcumuladas = table.Column<decimal>(name: "Saidas Acumuladas", type: "decimal(8,2)", nullable: false),
                    EspaçomedioSaidas = table.Column<decimal>(name: "Espaço medio Saidas", type: "decimal(8,3)", nullable: false),
                    PressaonoExtremo = table.Column<decimal>(name: "Pressao no Extremo", type: "decimal(8,2)", nullable: false),
                    DesnivelPontoAlto = table.Column<decimal>(name: "Desnivel Ponto Alto", type: "decimal(8,2)", nullable: false),
                    AlturadosAspersores = table.Column<decimal>(name: "Altura dos Aspersores", type: "decimal(8,2)", nullable: false),
                    DesnivelMotoBomba = table.Column<decimal>(name: "Desnivel Moto Bomba", type: "decimal(8,2)", nullable: false),
                    Alturadesuccao = table.Column<decimal>(name: "Altura de succao", type: "decimal(8,2)", nullable: false),
                    DesnivelmaisBaixo = table.Column<decimal>(name: "Desnivel mais Baixo", type: "decimal(8,2)", nullable: false),
                    PerdaMangueira = table.Column<decimal>(name: "Perda Mangueira", type: "decimal(8,2)", nullable: false),
                    ClienteAvulso = table.Column<string>(name: "Cliente Avulso", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    PropriedadeAvulsa = table.Column<string>(name: "Propriedade Avulsa", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    CidadeAvulsa = table.Column<string>(name: "Cidade Avulsa", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    DesnivelPontoBaixo = table.Column<decimal>(name: "Desnivel Ponto Baixo", type: "decimal(8,2)", nullable: false),
                    QtdeBombaSimples = table.Column<short>(name: "Qtde Bomba Simples", type: "smallint", nullable: false),
                    QtdeBombaParalela = table.Column<short>(name: "Qtde Bomba Paralela", type: "smallint", nullable: false),
                    MarcaBombaSimples = table.Column<string>(name: "Marca Bomba Simples", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    MarcaBombaParalela = table.Column<string>(name: "Marca Bomba Paralela", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    ModeloBombaSimples = table.Column<string>(name: "Modelo Bomba Simples", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    ModeloBombaParalela = table.Column<string>(name: "Modelo Bomba Paralela", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    TamanhoBombaSimples = table.Column<string>(name: "Tamanho Bomba Simples", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    TamanhoBombaParalela = table.Column<string>(name: "Tamanho Bomba Paralela", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    NEstagiosSimples = table.Column<short>(name: "N Estagios Simples", type: "smallint", nullable: false),
                    NEstagiosParalela = table.Column<short>(name: "N Estagios Paralela", type: "smallint", nullable: false),
                    DiametroBombaSimples = table.Column<decimal>(name: "Diametro Bomba Simples", type: "decimal(8,2)", nullable: false),
                    DiametroBombaParalela = table.Column<decimal>(name: "Diametro Bomba Paralela", type: "decimal(8,2)", nullable: false),
                    RendimentoBombaSimples = table.Column<decimal>(name: "Rendimento Bomba Simples", type: "decimal(8,2)", nullable: false),
                    RendimentoBombaParalela = table.Column<decimal>(name: "Rendimento Bomba Paralela", type: "decimal(8,2)", nullable: false),
                    RotaçãoBombaSimples = table.Column<decimal>(name: "Rotação Bomba Simples", type: "decimal(8,3)", nullable: false),
                    RotaçãoBombaParalela = table.Column<decimal>(name: "Rotação Bomba Paralela", type: "decimal(8,3)", nullable: false),
                    PressaoParalela = table.Column<decimal>(name: "Pressao Paralela", type: "decimal(8,2)", nullable: false),
                    MarcadoMotor = table.Column<string>(name: "Marca do Motor", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ModeloMotor = table.Column<string>(name: "Modelo Motor", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    NiveldeProteção = table.Column<string>(name: "Nivel de Proteção", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    PotenciaNominal = table.Column<decimal>(name: "Potencia Nominal", type: "decimal(8,2)", nullable: false),
                    NrodeFases = table.Column<short>(name: "Nro de Fases", type: "smallint", nullable: false),
                    Voltagem = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    QtdedeMotor = table.Column<decimal>(name: "Qtde de Motor", type: "decimal(10,2)", nullable: false),
                    ValorTotaldasPeças = table.Column<decimal>(name: "Valor Total das Peças", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosConjuntos = table.Column<decimal>(name: "Valor Total dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoProjeto = table.Column<decimal>(name: "Valor Total do Projeto", type: "decimal(12,2)", nullable: false),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    LocaldeEntrega = table.Column<string>(name: "Local de Entrega", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    PrazodeEntregaPrevisto = table.Column<string>(name: "Prazo de Entrega Previsto", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Fone1 = table.Column<string>(name: "Fone 1", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    SequenciadoVendedor = table.Column<int>(name: "Sequencia do Vendedor", type: "int", nullable: false),
                    FixoouRebocavel = table.Column<string>(name: "Fixo ou Rebocavel", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    SequenciadoOrçamento = table.Column<int>(name: "Sequencia do Orçamento", type: "int", nullable: false),
                    TotaldosServiços = table.Column<decimal>(name: "Total dos Serviços", type: "decimal(10,2)", nullable: false),
                    SequenciadoPneu = table.Column<int>(name: "Sequencia do Pneu", type: "int", nullable: false),
                    GerouEncargos = table.Column<bool>(name: "Gerou Encargos", type: "bit", nullable: false),
                    AtualizouLista = table.Column<bool>(name: "Atualizou Lista", type: "bit", nullable: false),
                    MarcaBombaAux = table.Column<string>(name: "Marca Bomba Aux", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ModeloBombaAux = table.Column<string>(name: "Modelo Bomba Aux", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    RotorBombaAux = table.Column<decimal>(name: "Rotor Bomba Aux", type: "decimal(8,3)", nullable: false),
                    RotaçãoBombaAux = table.Column<decimal>(name: "Rotação Bomba Aux", type: "decimal(8,3)", nullable: false),
                    MatBombaAux = table.Column<string>(name: "Mat Bomba Aux", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    VazaoBombaAux = table.Column<decimal>(name: "Vazao Bomba Aux", type: "decimal(8,2)", nullable: false),
                    PressaoBombaAux = table.Column<decimal>(name: "Pressao Bomba Aux", type: "decimal(8,2)", nullable: false),
                    RendimentoBombaAux = table.Column<decimal>(name: "Rendimento Bomba Aux", type: "decimal(8,2)", nullable: false),
                    BHPBombaAux = table.Column<decimal>(name: "BHP Bomba Aux", type: "decimal(8,2)", nullable: false),
                    Valordodolar = table.Column<decimal>(name: "Valor do dolar", type: "decimal(7,4)", nullable: false),
                    Qtdebombaaux = table.Column<short>(name: "Qtde bomba aux", type: "smallint", nullable: false),
                    Tamanhobombaaux = table.Column<string>(name: "Tamanho bomba aux", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Nestagiobombaaux = table.Column<short>(name: "N estagio bomba aux", type: "smallint", nullable: false),
                    Diambombaaux = table.Column<short>(name: "Diam bomba aux", type: "smallint", nullable: false),
                    VendaFechada = table.Column<bool>(name: "Venda Fechada", type: "bit", nullable: false),
                    EntregaTecnica = table.Column<string>(name: "Entrega Tecnica", type: "varchar(19)", unicode: false, maxLength: 19, nullable: false, defaultValue: ""),
                    VendedorIntermediario = table.Column<string>(name: "Vendedor Intermediario", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    PercentualdoVendedor = table.Column<decimal>(name: "Percentual do Vendedor", type: "decimal(8,4)", nullable: false),
                    Rebiut = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    PercentualRebiut = table.Column<decimal>(name: "Percentual Rebiut", type: "decimal(8,4)", nullable: false),
                    ModeloPivo = table.Column<string>(name: "Modelo Pivo", type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, defaultValue: ""),
                    FabricanteSprayFinal = table.Column<string>(name: "Fabricante Spray Final", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    CanhãoouAspersor = table.Column<string>(name: "Canhão ou Aspersor", type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValue: ""),
                    IniciodoBalanço = table.Column<short>(name: "Inicio do Balanço", type: "smallint", nullable: false),
                    SequenciadoBocal = table.Column<int>(name: "Sequencia do Bocal", type: "int", nullable: false),
                    BombaBooster = table.Column<bool>(name: "Bomba Booster", type: "bit", nullable: false),
                    CVBombaAux = table.Column<decimal>(name: "CV Bomba Aux", type: "decimal(7,2)", nullable: false),
                    CodigodoConversor = table.Column<int>(name: "Codigo do Conversor", type: "int", nullable: false),
                    OutrasDespesas = table.Column<decimal>(name: "Outras Despesas", type: "decimal(10,2)", nullable: false),
                    CPFAvulso = table.Column<string>(name: "CPF Avulso", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    celavulso = table.Column<string>(name: "cel avulso", type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Projeto", x => x.SequenciadoProjeto);
                });

            migrationBuilder.CreateTable(
                name: "Propriedades",
                columns: table => new
                {
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomedaPropriedade = table.Column<string>(name: "Nome da Propriedade", type: "varchar(62)", unicode: false, maxLength: 62, nullable: false, defaultValue: ""),
                    CNPJ = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: false, defaultValue: ""),
                    InscriçãoEstadual = table.Column<string>(name: "Inscrição Estadual", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Endereco = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    NúmerodoEndereço = table.Column<string>(name: "Número do Endereço", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Complemento = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    CaixaPostal = table.Column<string>(name: "Caixa Postal", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Bairro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    SeqüênciadoMunicípio = table.Column<int>(name: "Seqüência do Município", type: "int", nullable: false),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Propriedade", x => x.SeqüênciadaPropriedade);
                });

            migrationBuilder.CreateTable(
                name: "PW~Grupos",
                columns: table => new
                {
                    PWNome = table.Column<string>(name: "PW~Nome", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PW~Nome", x => x.PWNome);
                });

            migrationBuilder.CreateTable(
                name: "Razão Auxiliar",
                columns: table => new
                {
                    SequenciadoRazão = table.Column<int>(name: "Sequencia do Razão", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    DatadoRazão = table.Column<DateTime>(name: "Data do Razão", type: "datetime", nullable: true),
                    HistoricodoRazão = table.Column<string>(name: "Historico do Razão", type: "text", nullable: false, defaultValue: ""),
                    VrEntrada = table.Column<decimal>(name: "Vr Entrada", type: "decimal(10,2)", nullable: false),
                    VrSaida = table.Column<decimal>(name: "Vr Saida", type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Razão", x => x.SequenciadoRazão);
                });

            migrationBuilder.CreateTable(
                name: "Receita primaria",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    SeqüênciadaMatériaPrima = table.Column<int>(name: "Seqüência da Matéria Prima", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    Situacao = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Pedidos = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    Pagto = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false, defaultValue: ""),
                    QtdeRecebida = table.Column<decimal>(name: "Qtde Recebida", type: "decimal(10,2)", nullable: false),
                    QtdeRestante = table.Column<decimal>(name: "Qtde Restante", type: "decimal(10,2)", nullable: false),
                    QtdeTotal = table.Column<decimal>(name: "Qtde Total", type: "decimal(10,2)", nullable: false),
                    Localizacao = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    Sequenciaprodutoprincipal = table.Column<int>(name: "Sequencia produto principal", type: "int", nullable: false),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    QtSeparada = table.Column<decimal>(name: "Qt Separada", type: "decimal(11,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq_e_materia", x => new { x.SeqüênciadoOrçamento, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Região dos Vendedores",
                columns: table => new
                {
                    SeqdoVendedor = table.Column<int>(name: "Seq do Vendedor", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Vendedor", x => x.SeqdoVendedor);
                });

            migrationBuilder.CreateTable(
                name: "Resumo auxiliar",
                columns: table => new
                {
                    Sequenciadoresumo = table.Column<int>(name: "Sequencia do resumo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadoMovimento = table.Column<DateTime>(name: "Data do Movimento", type: "datetime", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Sdinicial = table.Column<decimal>(name: "Sd inicial", type: "decimal(11,2)", nullable: false),
                    Inicialestoque = table.Column<decimal>(name: "Inicial estoque", type: "decimal(11,4)", nullable: false),
                    Qtentradas = table.Column<decimal>(name: "Qt entradas", type: "decimal(11,4)", nullable: false),
                    Qtsaidas = table.Column<decimal>(name: "Qt saidas", type: "decimal(11,4)", nullable: false),
                    V_entradas = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    V_saidas = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Estoque_final = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    Sdfinal = table.Column<decimal>(name: "Sd final", type: "decimal(11,2)", nullable: false),
                    TipodoMovimento = table.Column<short>(name: "Tipo do Movimento", type: "smallint", nullable: false),
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do resumo", x => x.Sequenciadoresumo);
                });

            migrationBuilder.CreateTable(
                name: "Saida de Balcao",
                columns: table => new
                {
                    SequenciadaSaida = table.Column<int>(name: "Sequencia da Saida", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadaSaida = table.Column<DateTime>(name: "Data da Saida", type: "datetime", nullable: true),
                    Codigodosetor = table.Column<short>(name: "Codigo do setor", type: "smallint", nullable: false),
                    Codigodosolicitante = table.Column<short>(name: "Codigo do solicitante", type: "smallint", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Codigodosolicitante2 = table.Column<short>(name: "Codigo do solicitante 2", type: "smallint", nullable: false),
                    Teste = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Saida", x => x.SequenciadaSaida);
                });

            migrationBuilder.CreateTable(
                name: "Serie Gerador",
                columns: table => new
                {
                    SeqdoGerador = table.Column<int>(name: "Seq do Gerador", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescridoGerador = table.Column<string>(name: "Descri do Gerador", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    SeriedoGerador = table.Column<short>(name: "Serie do Gerador", type: "smallint", nullable: false),
                    MesAno = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false, defaultValue: ""),
                    DatadeCriação = table.Column<DateTime>(name: "Data de Criação", type: "datetime", nullable: false),
                    NrodeSeriedoGer = table.Column<string>(name: "Nro de Serie do Ger", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    NrodoMotor = table.Column<string>(name: "Nro do Motor", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    NrodoGerador = table.Column<string>(name: "Nro do Gerador", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    CodigodoGeral = table.Column<int>(name: "Codigo do Geral", type: "int", nullable: false),
                    Entregue = table.Column<bool>(type: "bit", nullable: false),
                    DtdeEntrega = table.Column<DateTime>(name: "Dt de Entrega", type: "datetime", nullable: true),
                    NF = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Gerador", x => x.SeqdoGerador);
                });

            migrationBuilder.CreateTable(
                name: "Serie Hidroturbo",
                columns: table => new
                {
                    SeqdoHidroturbo = table.Column<int>(name: "Seq do Hidroturbo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelodoHidroturbo = table.Column<string>(name: "Modelo do Hidroturbo", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    SeriedoHidroturbo = table.Column<int>(name: "Serie do Hidroturbo", type: "int", nullable: false),
                    MesAno = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false, defaultValue: ""),
                    LetradoHidroturbo = table.Column<string>(name: "Letra do Hidroturbo", type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    Carretelde = table.Column<string>(name: "Carretel de", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    DatadeCriação = table.Column<DateTime>(name: "Data de Criação", type: "datetime", nullable: false),
                    NrodeSerieHidroturbo = table.Column<string>(name: "Nro de Serie Hidroturbo", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    CodigodoGeral = table.Column<int>(name: "Codigo do Geral", type: "int", nullable: false),
                    CodigodoVendedor = table.Column<int>(name: "Codigo do Vendedor", type: "int", nullable: false),
                    Entregue = table.Column<bool>(type: "bit", nullable: false),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    NF = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    DtadaEntrega = table.Column<DateTime>(name: "Dta da Entrega", type: "datetime", nullable: true),
                    EntregaTecnica = table.Column<string>(name: "Entrega Tecnica", type: "varchar(19)", unicode: false, maxLength: 19, nullable: false, defaultValue: ""),
                    EntregaTec = table.Column<bool>(name: "Entrega Tec", type: "bit", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    Codigodosolicitante = table.Column<short>(name: "Codigo do solicitante", type: "smallint", nullable: false),
                    DataTecnica = table.Column<DateTime>(name: "Data Tecnica", type: "datetime", nullable: true),
                    AparecernoFiltro = table.Column<bool>(name: "Aparecer no Filtro", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Hidroturbo", x => x.SeqdoHidroturbo);
                });

            migrationBuilder.CreateTable(
                name: "Serie Moto Bomba",
                columns: table => new
                {
                    SeqMotoBomba = table.Column<int>(name: "Seq Moto Bomba", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TpdeMotor = table.Column<string>(name: "Tp de Motor", type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    SeriedaMotoBomba = table.Column<int>(name: "Serie da Moto Bomba", type: "int", nullable: false),
                    MesAno = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false, defaultValue: ""),
                    FunçãoMotoBomba = table.Column<string>(name: "Função Moto Bomba", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    NrodeSerieMotoBomba = table.Column<string>(name: "Nro de Serie Moto Bomba", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    NrodeSerieMotor = table.Column<string>(name: "Nro de Serie Motor", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    ModelodoMotor = table.Column<string>(name: "Modelo do Motor", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    NrodeSerieBomba = table.Column<string>(name: "Nro de Serie Bomba", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    ModelodaBomba = table.Column<string>(name: "Modelo da Bomba", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    DatadeCriação = table.Column<DateTime>(name: "Data de Criação", type: "datetime", nullable: false),
                    CodigodoGeral = table.Column<int>(name: "Codigo do Geral", type: "int", nullable: false),
                    CodigodoVendedor = table.Column<int>(name: "Codigo do Vendedor", type: "int", nullable: false),
                    Entregue = table.Column<bool>(type: "bit", nullable: false),
                    NF = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    DtdaEntrega = table.Column<DateTime>(name: "Dt da Entrega", type: "datetime", nullable: true),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    EntregaTecnica = table.Column<string>(name: "Entrega Tecnica", type: "varchar(19)", unicode: false, maxLength: 19, nullable: false, defaultValue: ""),
                    EntregaTec = table.Column<bool>(name: "Entrega Tec", type: "bit", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    Codigodosolicitante = table.Column<short>(name: "Codigo do solicitante", type: "smallint", nullable: false),
                    DataTecnica = table.Column<DateTime>(name: "Data Tecnica", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Moto Bomba", x => x.SeqMotoBomba);
                });

            migrationBuilder.CreateTable(
                name: "Serie Pivos",
                columns: table => new
                {
                    SeqdoPivo = table.Column<int>(name: "Seq do Pivo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelodoPivo = table.Column<string>(name: "Modelo do Pivo", type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, defaultValue: ""),
                    DescridoPivo = table.Column<string>(name: "Descri do Pivo", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    SeriedoPivo = table.Column<short>(name: "Serie do Pivo", type: "smallint", nullable: false),
                    MesAno = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false, defaultValue: ""),
                    LetradoPivo = table.Column<string>(name: "Letra do Pivo", type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    DatadeCriação = table.Column<DateTime>(name: "Data de Criação", type: "datetime", nullable: false),
                    NrodeSeriedoPivo = table.Column<string>(name: "Nro de Serie do Pivo", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    CodigodoGeral = table.Column<int>(name: "Codigo do Geral", type: "int", nullable: false),
                    Entregue = table.Column<bool>(type: "bit", nullable: false),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    NF = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    DatadaEntrega = table.Column<DateTime>(name: "Data da Entrega", type: "datetime", nullable: true),
                    CodigodoVendedor = table.Column<int>(name: "Codigo do Vendedor", type: "int", nullable: false),
                    EntregaTecnica = table.Column<string>(name: "Entrega Tecnica", type: "varchar(19)", unicode: false, maxLength: 19, nullable: false, defaultValue: ""),
                    EntregaTec = table.Column<bool>(name: "Entrega Tec", type: "bit", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    Codigodosolicitante = table.Column<short>(name: "Codigo do solicitante", type: "smallint", nullable: false),
                    DataTecnica = table.Column<DateTime>(name: "Data Tecnica", type: "datetime", nullable: true),
                    PrevMontagem = table.Column<DateTime>(name: "Prev Montagem", type: "datetime", nullable: true),
                    DadosAd = table.Column<string>(type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Pivo", x => x.SeqdoPivo);
                });

            migrationBuilder.CreateTable(
                name: "Serie Rebocador",
                columns: table => new
                {
                    SeqdoRebocador = table.Column<int>(name: "Seq do Rebocador", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadeCriação = table.Column<DateTime>(name: "Data de Criação", type: "datetime", nullable: false),
                    SeriedoRebocador = table.Column<short>(name: "Serie do Rebocador", type: "smallint", nullable: false),
                    ModelodoRebocador = table.Column<string>(name: "Modelo do Rebocador", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    MesAno = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false, defaultValue: ""),
                    NrodeSerieRebocador = table.Column<string>(name: "Nro de Serie Rebocador", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    CodigodoGeral = table.Column<int>(name: "Codigo do Geral", type: "int", nullable: false),
                    Entregue = table.Column<bool>(type: "bit", nullable: false),
                    NF = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    DatadaEntrega = table.Column<DateTime>(name: "Data da Entrega", type: "datetime", nullable: true),
                    CodigodoVendedor = table.Column<int>(name: "Codigo do Vendedor", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Rebocador", x => x.SeqdoRebocador);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    SeqüênciadoServiço = table.Column<short>(name: "Seqüência do Serviço", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    ValordoServiço = table.Column<decimal>(name: "Valor do Serviço", type: "decimal(11,2)", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Serviço", x => x.SeqüênciadoServiço);
                });

            migrationBuilder.CreateTable(
                name: "Serviços da Ordem",
                columns: table => new
                {
                    IddaOrdem = table.Column<int>(name: "Id da Ordem", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SequenciadoServiço = table.Column<int>(name: "Sequencia do Serviço", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitario = table.Column<decimal>(name: "Valor Unitario", type: "decimal(12,2)", nullable: false),
                    ValordoIss = table.Column<decimal>(name: "Valor do Iss", type: "decimal(12,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id_e_servico", x => new { x.IddaOrdem, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Serviços do Projeto",
                columns: table => new
                {
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SeqüênciadoServiço = table.Column<short>(name: "Seqüência do Serviço", type: "smallint", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitario = table.Column<decimal>(name: "Valor Unitario", type: "decimal(12,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    PartedoPivo = table.Column<string>(name: "Parte do Pivo", type: "varchar(29)", unicode: false, maxLength: 29, nullable: false, defaultValue: ""),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seq_e_servico", x => new { x.SequenciadoProjeto, x.SequenciadoItem });
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Codigodosetor = table.Column<short>(name: "Codigo do setor", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nomedosetor = table.Column<string>(name: "Nome do setor", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do setor", x => x.Codigodosetor);
                });

            migrationBuilder.CreateTable(
                name: "Simula estoque",
                columns: table => new
                {
                    Sequenciadasimulação = table.Column<int>(name: "Sequencia da simulação", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Atualestoque = table.Column<decimal>(name: "Atual estoque", type: "decimal(11,2)", nullable: false),
                    Necessarioestoque = table.Column<decimal>(name: "Necessario estoque", type: "decimal(11,2)", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    Saidasestoque = table.Column<decimal>(name: "Saidas estoque", type: "decimal(11,2)", nullable: false),
                    Saidaspeças = table.Column<decimal>(name: "Saidas peças", type: "decimal(11,2)", nullable: false),
                    Entradaspedido = table.Column<decimal>(name: "Entradas pedido", type: "decimal(11,2)", nullable: false),
                    Saidasorcprod = table.Column<decimal>(name: "Saidas orc prod", type: "decimal(11,2)", nullable: false),
                    Saidaorcpeças = table.Column<decimal>(name: "Saida orc peças", type: "decimal(11,2)", nullable: false),
                    NúmerodaNFe = table.Column<int>(name: "Número da NFe", type: "int", nullable: false),
                    ÚltimoFornecedor = table.Column<int>(name: "Último Fornecedor", type: "int", nullable: false),
                    Ultimocusto = table.Column<decimal>(name: "Ultimo custo", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da simulação", x => x.Sequenciadasimulação);
                });

            migrationBuilder.CreateTable(
                name: "Situação dos pedidos",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datapedido = table.Column<DateTime>(name: "Data pedido", type: "datetime", nullable: true),
                    Preventrega = table.Column<DateTime>(name: "Prev entrega", type: "datetime", nullable: true),
                    Diasematraso = table.Column<short>(name: "Dias em atraso", type: "smallint", nullable: false),
                    Obsfabrica = table.Column<string>(name: "Obs fabrica", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Obsvendas = table.Column<string>(name: "Obs vendas", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Obscompras = table.Column<string>(name: "Obs compras", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Obsalmoxarifado = table.Column<string>(name: "Obs almoxarifado", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Descmaterial = table.Column<string>(name: "Desc material", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Status = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência_orc_situacao", x => x.SeqüênciadoOrçamento);
                });

            migrationBuilder.CreateTable(
                name: "Solicitantes",
                columns: table => new
                {
                    Codigodosolicitante = table.Column<short>(name: "Codigo do solicitante", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigodosetor = table.Column<short>(name: "Codigo do setor", type: "smallint", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    Nomedosolicitante = table.Column<string>(name: "Nome do solicitante", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do solicitante", x => new { x.Codigodosolicitante, x.SequenciadoItem, x.Codigodosetor });
                });

            migrationBuilder.CreateTable(
                name: "Spy Baixa Contas",
                columns: table => new
                {
                    SeqdoSpy = table.Column<int>(name: "Seq do Spy", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqdaBaixa = table.Column<int>(name: "Seq da Baixa", type: "int", nullable: false),
                    DtInclusão = table.Column<DateTime>(name: "Dt Inclusão", type: "datetime", nullable: false),
                    Usuario = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    TpConta = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    Manutencao = table.Column<long>(type: "bigint", nullable: false),
                    DtBaixa = table.Column<DateTime>(name: "Dt Baixa", type: "datetime", nullable: true),
                    Juros = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    VrPago = table.Column<decimal>(name: "Vr Pago", type: "decimal(11,2)", nullable: false),
                    TpCarteira = table.Column<string>(name: "Tp Carteira", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    BxCliente = table.Column<DateTime>(name: "Bx Cliente", type: "datetime", nullable: true),
                    QuemPagou = table.Column<string>(name: "Quem Pagou", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    VrCliente = table.Column<decimal>(name: "Vr Cliente", type: "decimal(10,2)", nullable: false),
                    SeqBanco = table.Column<short>(name: "Seq Banco", type: "smallint", nullable: false),
                    SeqAccBanco = table.Column<short>(name: "Seq Acc Banco", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Spy", x => x.SeqdoSpy);
                });

            migrationBuilder.CreateTable(
                name: "Status do Processo",
                columns: table => new
                {
                    CodigodoStatus = table.Column<short>(name: "Codigo do Status", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescriçãodoStatus = table.Column<string>(name: "Descrição do Status", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do Status", x => x.CodigodoStatus);
                });

            migrationBuilder.CreateTable(
                name: "SYS~Sequencial",
                columns: table => new
                {
                    SYSTabela = table.Column<string>(name: "SYS~Tabela", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SYSCampo = table.Column<string>(name: "SYS~Campo", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SYSChave = table.Column<string>(name: "SYS~Chave", type: "varchar(200)", unicode: false, maxLength: 200, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    PWProjeto = table.Column<string>(name: "PW~Projeto", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SYSValor = table.Column<string>(name: "SYS~Valor", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SYSValorAnterior = table.Column<string>(name: "SYS~ValorAnterior", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SYSEstacao = table.Column<string>(name: "SYS~Estacao", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SYSIdentificacao = table.Column<string>(name: "SYS~Identificacao", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: "", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SYSPendentes = table.Column<int>(name: "SYS~Pendentes", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Chave sequencial", x => new { x.SYSTabela, x.SYSCampo, x.SYSChave });
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Dominio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipo de Atividades",
                columns: table => new
                {
                    CodigodaAtividade = table.Column<short>(name: "Codigo da Atividade", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescriçãodaAtividade = table.Column<string>(name: "Descrição da Atividade", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo da Atividade", x => x.CodigodaAtividade);
                });

            migrationBuilder.CreateTable(
                name: "Tipo de Cobrança",
                columns: table => new
                {
                    SeqüênciadaCobrança = table.Column<short>(name: "Seqüência da Cobrança", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Cobrança", x => x.SeqüênciadaCobrança);
                });

            migrationBuilder.CreateTable(
                name: "Tipo de Titulos",
                columns: table => new
                {
                    SeqdoTitulo = table.Column<short>(name: "Seq do Titulo", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Titulo", x => x.SeqdoTitulo);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    SeqüênciadaUnidade = table.Column<short>(name: "Seqüência da Unidade", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    SigladaUnidade = table.Column<string>(name: "Sigla da Unidade", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Unidade", x => x.SeqüênciadaUnidade);
                });

            migrationBuilder.CreateTable(
                name: "Vasilhames",
                columns: table => new
                {
                    SequenciadoVasilahme = table.Column<int>(name: "Sequencia do Vasilahme", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NúmerodaNFe = table.Column<int>(name: "Número da NFe", type: "int", nullable: false),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    Mov = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia do Vasilahme", x => x.SequenciadoVasilahme);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos do Motorista",
                columns: table => new
                {
                    CodigodoMotorista = table.Column<short>(name: "Codigo do Motorista", type: "smallint", nullable: false),
                    Automovel = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    PlacadoAutomovel = table.Column<string>(name: "Placa do Automovel", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    PlacadaCarreta = table.Column<string>(name: "Placa da Carreta", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Vendedores Bloqueio",
                columns: table => new
                {
                    CodigodoVendedor = table.Column<int>(name: "Codigo do Vendedor", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomedoVendedor = table.Column<string>(name: "Nome do Vendedor", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Percentual = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    Codigoipg = table.Column<int>(name: "Codigo ipg", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo do Vendedor Blok", x => x.CodigodoVendedor);
                });

            migrationBuilder.CreateTable(
                name: "Via de Transporte DI",
                columns: table => new
                {
                    SeqdoTransporte = table.Column<short>(name: "Seq do Transporte", type: "smallint", nullable: false),
                    Transporte = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Planilha de Adiantamento",
                columns: table => new
                {
                    SeqdoAdiantamento = table.Column<int>(name: "Seq do Adiantamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<short>(type: "smallint", nullable: false),
                    CoddoVendedor = table.Column<int>(name: "Cod do Vendedor", type: "int", nullable: false),
                    Manutencao = table.Column<int>(type: "int", nullable: false),
                    EmissãoNFe = table.Column<DateTime>(name: "Emissão NFe", type: "datetime", nullable: true),
                    NFe = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Parcela = table.Column<short>(type: "smallint", nullable: false),
                    CoddoGeral = table.Column<int>(name: "Cod do Geral", type: "int", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    Vencto = table.Column<DateTime>(type: "datetime", nullable: true),
                    PagtoCliente = table.Column<DateTime>(name: "Pagto Cliente", type: "datetime", nullable: true),
                    VrIPI = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Percentual = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    Comissao = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PagtoVendedor = table.Column<DateTime>(name: "Pagto Vendedor", type: "datetime", nullable: true),
                    Obs = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Devolucao = table.Column<bool>(type: "bit", nullable: false),
                    ValorPago = table.Column<decimal>(name: "Valor Pago", type: "decimal(11,2)", nullable: false),
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq do Adiantamento", x => x.SeqdoAdiantamento);
                    table.ForeignKey(
                        name: "TB_Planilha_de_Adiantamento_FK_Cod_do_Vendedor",
                        column: x => x.CoddoVendedor,
                        principalTable: "Conta do Vendedor",
                        principalColumn: "Id da Conta");
                });

            migrationBuilder.CreateTable(
                name: "Revendedores",
                columns: table => new
                {
                    SequenciadaRevenda = table.Column<int>(name: "Sequencia da Revenda", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IddaConta = table.Column<int>(name: "Id da Conta", type: "int", nullable: false),
                    TemContrato = table.Column<bool>(name: "Tem Contrato", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Revenda", x => x.SequenciadaRevenda);
                    table.ForeignKey(
                        name: "TB_Revendedores_FK_Id_da_Conta",
                        column: x => x.IddaConta,
                        principalTable: "Conta do Vendedor",
                        principalColumn: "Id da Conta");
                });

            migrationBuilder.CreateTable(
                name: "SubGrupo Despesa",
                columns: table => new
                {
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência SubGrupo Despesa", x => new { x.SeqüênciaSubGrupoDespesa, x.SeqüênciaGrupoDespesa });
                    table.ForeignKey(
                        name: "TB_SubGrupo_Despesa_FK_Seqüência_Grupo_Despesa",
                        column: x => x.SeqüênciaGrupoDespesa,
                        principalTable: "Grupo da Despesa",
                        principalColumn: "Seqüência Grupo Despesa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubGrupo do Produto",
                columns: table => new
                {
                    SeqüênciadoSubGrupoProduto = table.Column<short>(name: "Seqüência do SubGrupo Produto", type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoGrupoProduto = table.Column<short>(name: "Seqüência do Grupo Produto", type: "smallint", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do SubGrupo Produto", x => new { x.SeqüênciadoSubGrupoProduto, x.SeqüênciadoGrupoProduto });
                    table.ForeignKey(
                        name: "TB_SubGrupo_do_Produto_FK_Seqüência_do_Grupo_Produto",
                        column: x => x.SeqüênciadoGrupoProduto,
                        principalTable: "Grupo do Produto",
                        principalColumn: "Seqüência do Grupo Produto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimentação da Conta Corrente",
                columns: table => new
                {
                    SeqüênciadaMovimentaçãoCC = table.Column<int>(name: "Seqüência da Movimentação CC", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaAgência = table.Column<short>(name: "Seqüência da Agência", type: "smallint", nullable: false),
                    SeqüênciadaCCdaAgência = table.Column<short>(name: "Seqüência da CC da Agência", type: "smallint", nullable: false),
                    TipodeMovimentodaCC = table.Column<string>(name: "Tipo de Movimento da CC", type: "varchar(7)", unicode: false, maxLength: 7, nullable: false, defaultValue: ""),
                    DatadoMovimento = table.Column<DateTime>(name: "Data do Movimento", type: "datetime", nullable: false),
                    DatadoÚltimoDia = table.Column<DateTime>(name: "Data do Último Dia", type: "datetime", nullable: true),
                    Historico = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Conta = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    SeqüênciadoLançamento = table.Column<int>(name: "Seqüência do Lançamento", type: "int", nullable: false),
                    SeqüênciadoHistórico = table.Column<short>(name: "Seqüência do Histórico", type: "smallint", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    OrigemdaMovimentação = table.Column<string>(name: "Origem da Movimentação", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Blokeado = table.Column<bool>(type: "bit", nullable: false),
                    CodigodoHistorico = table.Column<short>(name: "Codigo do Historico", type: "smallint", nullable: false),
                    CodigodoDebito = table.Column<int>(name: "Codigo do Debito", type: "int", nullable: false),
                    CodigodoCredito = table.Column<int>(name: "Codigo do Credito", type: "int", nullable: false),
                    UsuáriodaAlteração = table.Column<string>(name: "Usuário da Alteração", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    DatadaAlteração = table.Column<DateTime>(name: "Data da Alteração", type: "datetime", nullable: true),
                    HoradaAlteração = table.Column<DateTime>(name: "Hora da Alteração", type: "datetime", nullable: true),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciaDaAgência = table.Column<short>(type: "smallint", nullable: false),
                    SeqüênciaDaCcDaAgência = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Movimentação CC", x => x.SeqüênciadaMovimentaçãoCC);
                    table.ForeignKey(
                        name: "TB_Movimentação_da_Conta_Corrente_FK_Seqüência_da_Agência_Seqüência_da_CC_da_Agência",
                        columns: x => new { x.SeqüênciaDaAgência, x.SeqüênciaDaCcDaAgência },
                        principalTable: "Conta Corrente da Agência",
                        principalColumns: new[] { "Seqüência da Agência", "Seqüência da CC da Agência" });
                    table.ForeignKey(
                        name: "TB_Movimentação_da_Conta_Corrente_FK_Seqüência_do_Histórico",
                        column: x => x.SeqüênciadoHistórico,
                        principalTable: "Histórico da Conta Corrente",
                        principalColumn: "Seqüência do Histórico");
                });

            migrationBuilder.CreateTable(
                name: "Geral",
                columns: table => new
                {
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cliente = table.Column<bool>(type: "bit", nullable: false),
                    Fornecedor = table.Column<bool>(type: "bit", nullable: false),
                    Despesa = table.Column<bool>(type: "bit", nullable: false),
                    Imposto = table.Column<bool>(type: "bit", nullable: false),
                    Transportadora = table.Column<bool>(type: "bit", nullable: false),
                    Vendedor = table.Column<bool>(type: "bit", nullable: false),
                    RazãoSocial = table.Column<string>(name: "Razão Social", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    NomeFantasia = table.Column<string>(name: "Nome Fantasia", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Tipo = table.Column<short>(type: "smallint", nullable: false),
                    Endereco = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Complemento = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    NúmerodoEndereço = table.Column<string>(name: "Número do Endereço", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Bairro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    CaixaPostal = table.Column<string>(name: "Caixa Postal", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    SeqüênciadoMunicípio = table.Column<int>(name: "Seqüência do Município", type: "int", nullable: false),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    Fone1 = table.Column<string>(name: "Fone 1", type: "varchar(14)", unicode: false, maxLength: 14, nullable: true, defaultValue: ""),
                    Fone2 = table.Column<string>(name: "Fone 2", type: "varchar(14)", unicode: false, maxLength: 14, nullable: true, defaultValue: ""),
                    Fax = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true, defaultValue: ""),
                    Celular = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true, defaultValue: ""),
                    Contato = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false, defaultValue: ""),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    HomePage = table.Column<string>(name: "Home Page", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    CódigodoSuframa = table.Column<string>(name: "Código do Suframa", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    CódigodaANTT = table.Column<string>(name: "Código da ANTT", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    CPFeCNPJ = table.Column<string>(name: "CPF e CNPJ", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    RGeIE = table.Column<string>(name: "RG e IE", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    EndereçodeCobrança = table.Column<string>(name: "Endereço de Cobrança", type: "varchar(62)", unicode: false, maxLength: 62, nullable: false, defaultValue: ""),
                    NúmerodoEndereçodeCobrança = table.Column<string>(name: "Número do Endereço de Cobrança", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    SeqüênciaMunicípioCobrança = table.Column<int>(name: "Seqüência Município Cobrança", type: "int", nullable: false),
                    CepdeCobrança = table.Column<string>(name: "Cep de Cobrança", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    BairrodeCobrança = table.Column<string>(name: "Bairro de Cobrança", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    ComplementodaCobrança = table.Column<string>(name: "Complemento da Cobrança", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    CaixaPostaldaCobrança = table.Column<string>(name: "Caixa Postal da Cobrança", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    SeqüênciadoVendedor = table.Column<int>(name: "Seqüência do Vendedor", type: "int", nullable: false),
                    IntermediáriodoVendedor = table.Column<string>(name: "Intermediário do Vendedor", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    NomedoBanco1 = table.Column<string>(name: "Nome do Banco 1", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    NomedoBanco2 = table.Column<string>(name: "Nome do Banco 2", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    AgênciadoBanco1 = table.Column<string>(name: "Agência do Banco 1", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    AgênciadoBanco2 = table.Column<string>(name: "Agência do Banco 2", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ContaCorrentedoBanco1 = table.Column<string>(name: "Conta Corrente do Banco 1", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    ContaCorrentedoBanco2 = table.Column<string>(name: "Conta Corrente do Banco 2", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    NomedoCorrentistadoBanco1 = table.Column<string>(name: "Nome do Correntista do Banco 1", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    NomedoCorrentistadoBanco2 = table.Column<string>(name: "Nome do Correntista do Banco 2", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    Revenda = table.Column<bool>(type: "bit", nullable: false),
                    Isento = table.Column<bool>(type: "bit", nullable: false),
                    DatadoCadastro = table.Column<DateTime>(name: "Data do Cadastro", type: "datetime", nullable: true),
                    SeqüênciadoPaís = table.Column<int>(name: "Seqüência do País", type: "int", nullable: false),
                    OrgõnPublico = table.Column<bool>(name: "Orgõn Publico", type: "bit", nullable: false),
                    Cumulativo = table.Column<bool>(type: "bit", nullable: false),
                    EmpresaProdutor = table.Column<bool>(name: "Empresa Produtor", type: "bit", nullable: false),
                    UsudaAlteração = table.Column<string>(name: "Usu da Alteração", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    DatadeNascimento = table.Column<DateTime>(name: "Data de Nascimento", type: "datetime", nullable: true),
                    CodigoContabil = table.Column<int>(name: "Codigo Contabil", type: "int", nullable: false),
                    CodigoAdiantamento = table.Column<int>(name: "Codigo Adiantamento", type: "int", nullable: false),
                    Salbruto = table.Column<decimal>(name: "Sal bruto", type: "decimal(10,2)", nullable: false),
                    ImportounoZap = table.Column<bool>(name: "Importou no Zap", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Geral", x => x.SeqüênciadoGeral);
                    table.ForeignKey(
                        name: "TB_Geral_FK_Seqüência_Município_Cobrança",
                        column: x => x.SeqüênciaMunicípioCobrança,
                        principalTable: "Municipios",
                        principalColumn: "Seqüência do Município");
                    table.ForeignKey(
                        name: "TB_Geral_FK_Seqüência_do_Município",
                        column: x => x.SeqüênciadoMunicípio,
                        principalTable: "Municipios",
                        principalColumn: "Seqüência do Município");
                    table.ForeignKey(
                        name: "TB_Geral_FK_Seqüência_do_País",
                        column: x => x.SeqüênciadoPaís,
                        principalTable: "Paises",
                        principalColumn: "Seqüência do País");
                    table.ForeignKey(
                        name: "TB_Vendedor_Geral",
                        column: x => x.SeqüênciadoVendedor,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "PW~Tabelas",
                columns: table => new
                {
                    PWProjeto = table.Column<string>(name: "PW~Projeto", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    PWGrupo = table.Column<string>(name: "PW~Grupo", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PWNome = table.Column<string>(name: "PW~Nome", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PWPermissoes = table.Column<string>(name: "PW~Permissoes", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Chave tabelas", x => new { x.PWProjeto, x.PWGrupo, x.PWNome });
                    table.ForeignKey(
                        name: "TB_PW~Tabelas_FK_PW~Grupo",
                        column: x => x.PWGrupo,
                        principalTable: "PW~Grupos",
                        principalColumn: "PW~Nome",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PW~Usuarios",
                columns: table => new
                {
                    PWNome = table.Column<string>(name: "PW~Nome", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PWSenha = table.Column<string>(name: "PW~Senha", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PWGrupo = table.Column<string>(name: "PW~Grupo", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PWObs = table.Column<string>(name: "PW~Obs", type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PWSenhaHash = table.Column<string>(name: "PW~SenhaHash", type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Chave usuario", x => new { x.PWNome, x.PWSenha });
                    table.ForeignKey(
                        name: "TB_PW~Usuarios_FK_PW~Grupo",
                        column: x => x.PWGrupo,
                        principalTable: "PW~Grupos",
                        principalColumn: "PW~Nome",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Linha de Produção",
                columns: table => new
                {
                    SequenciadaProdução = table.Column<int>(name: "Sequencia da Produção", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadaProdução = table.Column<DateTime>(name: "Data da Produção", type: "datetime", nullable: true),
                    Codigodosetor = table.Column<short>(name: "Codigo do setor", type: "smallint", nullable: false),
                    CodigodoColaborador = table.Column<short>(name: "Codigo do Colaborador", type: "smallint", nullable: false),
                    Solicitaçãode = table.Column<string>(name: "Solicitação de", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    Finalizado = table.Column<bool>(type: "bit", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ApenasMontagem = table.Column<bool>(name: "Apenas Montagem", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia da Produção", x => x.SequenciadaProdução);
                    table.ForeignKey(
                        name: "TB_Linha_de_Produção_FK_Codigo_do_setor",
                        column: x => x.Codigodosetor,
                        principalTable: "Setores",
                        principalColumn: "Codigo do setor");
                });

            migrationBuilder.CreateTable(
                name: "Controle de Processos",
                columns: table => new
                {
                    IddoProcesso = table.Column<int>(name: "Id do Processo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigodoStatus = table.Column<short>(name: "Codigo do Status", type: "smallint", nullable: false),
                    CodigodoAdvogado = table.Column<short>(name: "Codigo do Advogado", type: "smallint", nullable: false),
                    CodigodaAção = table.Column<short>(name: "Codigo da Ação", type: "smallint", nullable: false),
                    OutroEnvolvido = table.Column<int>(name: "Outro Envolvido", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id do Processo", x => x.IddoProcesso);
                    table.ForeignKey(
                        name: "TB_Controle_de_Processos_FK_Codigo_do_Advogado",
                        column: x => x.CodigodoAdvogado,
                        principalTable: "Advogados",
                        principalColumn: "Codigo do Advogado");
                    table.ForeignKey(
                        name: "TB_Controle_de_Processos_FK_Codigo_do_Status",
                        column: x => x.CodigodoStatus,
                        principalTable: "Status do Processo",
                        principalColumn: "Codigo do Status");
                });

            migrationBuilder.CreateTable(
                name: "Despesas",
                columns: table => new
                {
                    SeqüênciadaDespesa = table.Column<int>(name: "Seqüência da Despesa", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    SeqüênciadaUnidade = table.Column<short>(name: "Seqüência da Unidade", type: "smallint", nullable: false),
                    QuantidadenoEstoque = table.Column<decimal>(name: "Quantidade no Estoque", type: "decimal(11,4)", nullable: false),
                    QuantidadeMínima = table.Column<decimal>(name: "Quantidade Mínima", type: "decimal(9,3)", nullable: false),
                    CódigodeBarras = table.Column<string>(name: "Código de Barras", type: "varchar(13)", unicode: false, maxLength: 13, nullable: false, defaultValue: ""),
                    Localizacao = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    ÚltimaCompra = table.Column<DateTime>(name: "Última Compra", type: "datetime", nullable: true),
                    ÚltimoMovimento = table.Column<DateTime>(name: "Último Movimento", type: "datetime", nullable: true),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    CustoMédio = table.Column<decimal>(name: "Custo Médio", type: "decimal(11,2)", nullable: false),
                    ÚltimoFornecedor = table.Column<int>(name: "Último Fornecedor", type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    TipodoProduto = table.Column<short>(name: "Tipo do Produto", type: "smallint", nullable: false),
                    QuantidadeContábil = table.Column<decimal>(name: "Quantidade Contábil", type: "decimal(11,4)", nullable: false),
                    ValorContábilAtual = table.Column<decimal>(name: "Valor Contábil Atual", type: "decimal(13,4)", nullable: false),
                    MargemdeLucro = table.Column<decimal>(name: "Margem de Lucro", type: "decimal(7,2)", nullable: false),
                    Movimentaficha = table.Column<bool>(name: "Movimenta ficha", type: "bit", nullable: false),
                    SeqüênciaSubGrupoDespesa0 = table.Column<short>(name: "SeqüênciaSubGrupoDespesa", type: "smallint", nullable: false),
                    SeqüênciaGrupoDespesa0 = table.Column<short>(name: "SeqüênciaGrupoDespesa", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Despesa", x => x.SeqüênciadaDespesa);
                    table.ForeignKey(
                        name: "TB_Despesas_FK_Seqüência_Grupo_Despesa",
                        column: x => x.SeqüênciaGrupoDespesa,
                        principalTable: "Grupo da Despesa",
                        principalColumn: "Seqüência Grupo Despesa");
                    table.ForeignKey(
                        name: "TB_Despesas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa",
                        columns: x => new { x.SeqüênciaSubGrupoDespesa0, x.SeqüênciaGrupoDespesa0 },
                        principalTable: "SubGrupo Despesa",
                        principalColumns: new[] { "Seqüência SubGrupo Despesa", "Seqüência Grupo Despesa" });
                    table.ForeignKey(
                        name: "TB_Despesas_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Despesas_FK_Seqüência_da_Unidade",
                        column: x => x.SeqüênciadaUnidade,
                        principalTable: "Unidades",
                        principalColumn: "Seqüência da Unidade");
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos",
                columns: table => new
                {
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Detalhes = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    QuantidadenoEstoque = table.Column<decimal>(name: "Quantidade no Estoque", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    SeqüênciadoGrupoProduto = table.Column<short>(name: "Seqüência do Grupo Produto", type: "smallint", nullable: false),
                    SeqüênciadoSubGrupoProduto = table.Column<short>(name: "Seqüência do SubGrupo Produto", type: "smallint", nullable: false),
                    SeqüênciadaUnidade = table.Column<short>(name: "Seqüência da Unidade", type: "smallint", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    Usado = table.Column<bool>(type: "bit", nullable: false),
                    ÚltimoMovimento = table.Column<DateTime>(name: "Último Movimento", type: "datetime", nullable: true),
                    Localizacao = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    QuantidadeContábil = table.Column<decimal>(name: "Quantidade Contábil", type: "decimal(11,4)", nullable: false),
                    ValorContábilAtual = table.Column<decimal>(name: "Valor Contábil Atual", type: "decimal(13,4)", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    QuantidadeMínima = table.Column<decimal>(name: "Quantidade Mínima", type: "decimal(9,3)", nullable: false),
                    ÚltimaEntrada = table.Column<DateTime>(name: "Última Entrada", type: "datetime", nullable: true),
                    AlturadoConjunto = table.Column<string>(name: "Altura do Conjunto", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    LarguradoConjunto = table.Column<string>(name: "Largura do Conjunto", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Comprimento = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    PesodoConjunto = table.Column<decimal>(name: "Peso do Conjunto", type: "decimal(12,3)", nullable: false),
                    PartedoPivo = table.Column<string>(name: "Parte do Pivo", type: "varchar(29)", unicode: false, maxLength: 29, nullable: false, defaultValue: ""),
                    Travareceita = table.Column<bool>(name: "Trava receita", type: "bit", nullable: false),
                    ReceitaConferida = table.Column<bool>(name: "Receita Conferida", type: "bit", nullable: false),
                    MargemdeLucro = table.Column<decimal>(name: "Margem de Lucro", type: "decimal(11,4)", nullable: false),
                    ValorTotalAnterior = table.Column<decimal>(name: "Valor Total Anterior", type: "decimal(12,4)", nullable: true),
                    SeqüênciaDoSubGrupoProduto = table.Column<short>(type: "smallint", nullable: false),
                    SeqüênciaDoGrupoProduto = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Conjunto", x => x.SeqüênciadoConjunto);
                    table.ForeignKey(
                        name: "TB_Conjuntos_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Conjuntos_FK_Seqüência_da_Unidade",
                        column: x => x.SeqüênciadaUnidade,
                        principalTable: "Unidades",
                        principalColumn: "Seqüência da Unidade");
                    table.ForeignKey(
                        name: "TB_Conjuntos_FK_Seqüência_do_Grupo_Produto",
                        column: x => x.SeqüênciadoGrupoProduto,
                        principalTable: "Grupo do Produto",
                        principalColumn: "Seqüência do Grupo Produto");
                    table.ForeignKey(
                        name: "TB_Conjuntos_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto",
                        columns: x => new { x.SeqüênciaDoSubGrupoProduto, x.SeqüênciaDoGrupoProduto },
                        principalTable: "SubGrupo do Produto",
                        principalColumns: new[] { "Seqüência do SubGrupo Produto", "Seqüência do Grupo Produto" });
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    SeqüênciadoGrupoProduto = table.Column<short>(name: "Seqüência do Grupo Produto", type: "smallint", nullable: false),
                    SeqüênciadoSubGrupoProduto = table.Column<short>(name: "Seqüência do SubGrupo Produto", type: "smallint", nullable: false),
                    ÚltimaCompra = table.Column<DateTime>(name: "Última Compra", type: "datetime", nullable: true),
                    QuantidadenoEstoque = table.Column<decimal>(name: "Quantidade no Estoque", type: "decimal(11,4)", nullable: false),
                    QuantidadeMínima = table.Column<decimal>(name: "Quantidade Mínima", type: "decimal(9,3)", nullable: false),
                    ÚltimoMovimento = table.Column<DateTime>(name: "Último Movimento", type: "datetime", nullable: true),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    MargemdeLucro = table.Column<decimal>(name: "Margem de Lucro", type: "decimal(11,4)", nullable: false),
                    SeqüênciadaUnidade = table.Column<short>(name: "Seqüência da Unidade", type: "smallint", nullable: false),
                    CódigodeBarras = table.Column<string>(name: "Código de Barras", type: "varchar(13)", unicode: false, maxLength: 13, nullable: false, defaultValue: ""),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    ÉMatériaPrima = table.Column<bool>(name: "É Matéria Prima", type: "bit", nullable: false),
                    CustoMédio = table.Column<decimal>(name: "Custo Médio", type: "decimal(11,2)", nullable: false),
                    Usado = table.Column<bool>(type: "bit", nullable: false),
                    ÚltimoFornecedor = table.Column<int>(name: "Último Fornecedor", type: "int", nullable: false),
                    TipodoProduto = table.Column<short>(name: "Tipo do Produto", type: "smallint", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    Localizacao = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    QuantidadeContábil = table.Column<decimal>(name: "Quantidade Contábil", type: "decimal(11,4)", nullable: false),
                    ValorContábilAtual = table.Column<decimal>(name: "Valor Contábil Atual", type: "decimal(13,4)", nullable: false),
                    MaterialAdquiridodeTerceiro = table.Column<bool>(name: "Material Adquirido de Terceiro", type: "bit", nullable: false),
                    ValorAtualizado = table.Column<bool>(name: "Valor Atualizado", type: "bit", nullable: false),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false),
                    Sucata = table.Column<bool>(type: "bit", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    Industrializacao = table.Column<bool>(type: "bit", nullable: false),
                    Importado = table.Column<bool>(type: "bit", nullable: false),
                    Medida = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    ValordeLista = table.Column<decimal>(name: "Valor de Lista", type: "decimal(12,2)", nullable: false),
                    UsuáriodaAlteração = table.Column<string>(name: "Usuário da Alteração", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    DatadaAlteração = table.Column<DateTime>(name: "Data da Alteração", type: "datetime", nullable: true),
                    HoradaAlteração = table.Column<DateTime>(name: "Hora da Alteração", type: "datetime", nullable: true),
                    ModelodoLance = table.Column<int>(name: "Modelo do Lance", type: "int", nullable: false),
                    UsadonoProjeto = table.Column<bool>(name: "Usado no Projeto", type: "bit", nullable: false),
                    PartedoPivo = table.Column<string>(name: "Parte do Pivo", type: "varchar(29)", unicode: false, maxLength: 29, nullable: false, defaultValue: ""),
                    QuantidadeFisica = table.Column<decimal>(name: "Quantidade Fisica", type: "decimal(11,4)", nullable: false),
                    DatadaContagem = table.Column<DateTime>(name: "Data da Contagem", type: "datetime", nullable: true),
                    NãoSairnoRelatório = table.Column<bool>(name: "Não Sair no Relatório", type: "bit", nullable: false),
                    MostrarReceitaSecundaria = table.Column<bool>(name: "Mostrar Receita Secundaria", type: "bit", nullable: false),
                    NaoMostrarReceita = table.Column<bool>(name: "Nao Mostrar Receita", type: "bit", nullable: false),
                    Naosairnochecklist = table.Column<bool>(name: "Nao sair no checklist", type: "bit", nullable: false),
                    Travareceita = table.Column<bool>(name: "Trava receita", type: "bit", nullable: false),
                    Lance = table.Column<bool>(type: "bit", nullable: false),
                    Mpinicial = table.Column<bool>(name: "Mp inicial", type: "bit", nullable: false),
                    QtdeInicial = table.Column<decimal>(name: "Qtde Inicial", type: "decimal(10,2)", nullable: false),
                    ERegulador = table.Column<bool>(name: "E Regulador", type: "bit", nullable: false),
                    SeparadoMontar = table.Column<decimal>(name: "Separado Montar", type: "decimal(11,4)", nullable: false),
                    CompradosAguardando = table.Column<decimal>(name: "Comprados Aguardando", type: "decimal(11,4)", nullable: false),
                    ConferidopeloContabil = table.Column<bool>(name: "Conferido pelo Contabil", type: "bit", nullable: false),
                    Obsoleto = table.Column<bool>(type: "bit", nullable: false),
                    Marcar = table.Column<bool>(type: "bit", nullable: false),
                    UltimaCotação = table.Column<DateTime>(name: "Ultima Cotação", type: "datetime", nullable: true),
                    MedidaFinal = table.Column<string>(name: "Medida Final", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ReceitaConferida = table.Column<bool>(name: "Receita Conferida", type: "bit", nullable: false),
                    Detalhes = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    QuantidadeBalanço = table.Column<decimal>(name: "Quantidade Balanço", type: "decimal(11,4)", nullable: false),
                    PesoOk = table.Column<bool>(name: "Peso Ok", type: "bit", nullable: false),
                    ValordeCustoAnterior = table.Column<decimal>(name: "Valor de Custo Anterior", type: "decimal(12,4)", nullable: true),
                    ValorTotalAnterior = table.Column<decimal>(name: "Valor Total Anterior", type: "decimal(12,4)", nullable: true),
                    MargemdeLucroAnterior = table.Column<decimal>(name: "Margem de Lucro Anterior", type: "decimal(12,4)", nullable: true),
                    SeqüênciaDoSubGrupoProduto = table.Column<short>(type: "smallint", nullable: false),
                    SeqüênciaDoGrupoProduto = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Produto", x => x.SeqüênciadoProduto);
                    table.ForeignKey(
                        name: "TB_Produtos_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Produtos_FK_Seqüência_da_Unidade",
                        column: x => x.SeqüênciadaUnidade,
                        principalTable: "Unidades",
                        principalColumn: "Seqüência da Unidade");
                    table.ForeignKey(
                        name: "TB_Produtos_FK_Seqüência_do_Grupo_Produto",
                        column: x => x.SeqüênciadoGrupoProduto,
                        principalTable: "Grupo do Produto",
                        principalColumn: "Seqüência do Grupo Produto");
                    table.ForeignKey(
                        name: "TB_Produtos_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto",
                        columns: x => new { x.SeqüênciaDoSubGrupoProduto, x.SeqüênciaDoGrupoProduto },
                        principalTable: "SubGrupo do Produto",
                        principalColumns: new[] { "Seqüência do SubGrupo Produto", "Seqüência do Grupo Produto" });
                });

            migrationBuilder.CreateTable(
                name: "Declarações de Importação",
                columns: table => new
                {
                    SeqüênciadaDeclaração = table.Column<int>(name: "Seqüência da Declaração", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    SeqüênciaProdutoNotaFiscal = table.Column<int>(name: "Seqüência Produto Nota Fiscal", type: "int", nullable: false),
                    NúmerodaDeclaração = table.Column<string>(name: "Número da Declaração", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    DatadeRegistro = table.Column<DateTime>(name: "Data de Registro", type: "datetime", nullable: false),
                    LocaldeDesembaraço = table.Column<string>(name: "Local de Desembaraço", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    UFdeDesembaraço = table.Column<string>(name: "UF de Desembaraço", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    DatadeDesembaraço = table.Column<DateTime>(name: "Data de Desembaraço", type: "datetime", nullable: false),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    ValordaMarinhaMercante = table.Column<decimal>(name: "Valor da Marinha Mercante", type: "decimal(10,2)", nullable: false),
                    ViaTransporte = table.Column<short>(name: "Via Transporte", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Declaração", x => x.SeqüênciadaDeclaração);
                    table.ForeignKey(
                        name: "TB_Declarações_de_Importação_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Entrada Contas",
                columns: table => new
                {
                    SeqüênciadaEntrada = table.Column<int>(name: "Seqüência da Entrada", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadeEntrada = table.Column<DateTime>(name: "Data de Entrada", type: "datetime", nullable: true),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    NúmerodaNotaFiscal = table.Column<int>(name: "Número da Nota Fiscal", type: "int", nullable: false),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    NúmerodaDuplicata = table.Column<int>(name: "Número da Duplicata", type: "int", nullable: false),
                    Historico = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    TipodaConta = table.Column<string>(name: "Tipo da Conta", type: "varchar(11)", unicode: false, maxLength: 11, nullable: false, defaultValue: ""),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    Conta = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    DatadaAlteração = table.Column<DateTime>(name: "Data da Alteração", type: "datetime", nullable: true),
                    HoradaAlteração = table.Column<DateTime>(name: "Hora da Alteração", type: "datetime", nullable: true),
                    UsuáriodaAlteração = table.Column<string>(name: "Usuário da Alteração", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Previsao = table.Column<bool>(type: "bit", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    SequenciadaCompra = table.Column<int>(name: "Sequencia da Compra", type: "int", nullable: false),
                    CodigodoDebito = table.Column<int>(name: "Codigo do Debito", type: "int", nullable: false),
                    CodigodoCredito = table.Column<int>(name: "Codigo do Credito", type: "int", nullable: false),
                    SeqüênciaSubGrupoDespesa0 = table.Column<short>(name: "SeqüênciaSubGrupoDespesa", type: "smallint", nullable: false),
                    SeqüênciaGrupoDespesa0 = table.Column<short>(name: "SeqüênciaGrupoDespesa", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Entrada", x => x.SeqüênciadaEntrada);
                    table.ForeignKey(
                        name: "TB_Entrada_Contas_FK_Seqüência_Grupo_Despesa",
                        column: x => x.SeqüênciaGrupoDespesa,
                        principalTable: "Grupo da Despesa",
                        principalColumn: "Seqüência Grupo Despesa");
                    table.ForeignKey(
                        name: "TB_Entrada_Contas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa",
                        columns: x => new { x.SeqüênciaSubGrupoDespesa0, x.SeqüênciaGrupoDespesa0 },
                        principalTable: "SubGrupo Despesa",
                        principalColumns: new[] { "Seqüência SubGrupo Despesa", "Seqüência Grupo Despesa" });
                    table.ForeignKey(
                        name: "TB_Entrada_Contas_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Movimento do Estoque",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    DatadaCompra = table.Column<DateTime>(name: "Data da Compra", type: "datetime", nullable: true),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    ValorTotaldosProdutos = table.Column<decimal>(name: "Valor Total dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosConjuntos = table.Column<decimal>(name: "Valor Total dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ProcessarCusto = table.Column<bool>(name: "Processar Custo", type: "bit", nullable: false),
                    DatadeEntrada = table.Column<DateTime>(name: "Data de Entrada", type: "datetime", nullable: true),
                    ValorTotalIPIdosProdutos = table.Column<decimal>(name: "Valor Total IPI dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosConjuntos = table.Column<decimal>(name: "Valor Total IPI dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    TipodoMovimento = table.Column<short>(name: "Tipo do Movimento", type: "smallint", nullable: false),
                    ValorTotaldoMovimento = table.Column<decimal>(name: "Valor Total do Movimento", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMSST = table.Column<decimal>(name: "Valor Total do ICMS ST", type: "decimal(11,2)", nullable: false),
                    ValordoFechamento = table.Column<decimal>(name: "Valor do Fechamento", type: "decimal(11,2)", nullable: false),
                    Fechamento = table.Column<short>(type: "smallint", nullable: false),
                    ValordoSeguro = table.Column<decimal>(name: "Valor do Seguro", type: "decimal(11,2)", nullable: false),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    TipoMovimento = table.Column<short>(name: "Tipo Movimento", type: "smallint", nullable: false),
                    ValorTotalIPIdasPeças = table.Column<decimal>(name: "Valor Total IPI das Peças", type: "decimal(11,2)", nullable: false),
                    ValorTotaldasPeças = table.Column<decimal>(name: "Valor Total das Peças", type: "decimal(11,2)", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false),
                    ValorTotaldaBasedeCálculo = table.Column<decimal>(name: "Valor Total da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMS = table.Column<decimal>(name: "Valor Total do ICMS", type: "decimal(11,2)", nullable: false),
                    MovimentoCancelado = table.Column<bool>(name: "Movimento Cancelado", type: "bit", nullable: false),
                    NãoTotaliza = table.Column<bool>(name: "Não Totaliza", type: "bit", nullable: false),
                    DatadaAlteração = table.Column<DateTime>(name: "Data da Alteração", type: "datetime", nullable: true),
                    HoradaAlteração = table.Column<DateTime>(name: "Hora da Alteração", type: "datetime", nullable: true),
                    UsuáriodaAlteração = table.Column<string>(name: "Usuário da Alteração", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    ValorTotalIPIdasDespesas = table.Column<decimal>(name: "Valor Total IPI das Despesas", type: "decimal(11,2)", nullable: false),
                    ValorTotaldasDespesas = table.Column<decimal>(name: "Valor Total das Despesas", type: "decimal(11,2)", nullable: false),
                    Industrializacao = table.Column<bool>(type: "bit", nullable: false),
                    OutrasDespesas = table.Column<decimal>(name: "Outras Despesas", type: "decimal(10,2)", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Nao_Alterar = table.Column<bool>(type: "bit", nullable: false),
                    Notadevenda = table.Column<int>(name: "Nota de venda", type: "int", nullable: false),
                    SeqüênciaSubGrupoDespesa0 = table.Column<short>(name: "SeqüênciaSubGrupoDespesa", type: "smallint", nullable: false),
                    SeqüênciaGrupoDespesa0 = table.Column<short>(name: "SeqüênciaGrupoDespesa", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Movimento", x => x.SeqüênciadoMovimento);
                    table.ForeignKey(
                        name: "TB_Movimento_do_Estoque_FK_Seqüência_Grupo_Despesa",
                        column: x => x.SeqüênciaGrupoDespesa,
                        principalTable: "Grupo da Despesa",
                        principalColumn: "Seqüência Grupo Despesa");
                    table.ForeignKey(
                        name: "TB_Movimento_do_Estoque_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa",
                        columns: x => new { x.SeqüênciaSubGrupoDespesa0, x.SeqüênciaGrupoDespesa0 },
                        principalTable: "SubGrupo Despesa",
                        principalColumns: new[] { "Seqüência SubGrupo Despesa", "Seqüência Grupo Despesa" });
                    table.ForeignKey(
                        name: "TB_Movimento_do_Estoque_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Movimento_do_Estoque_FK_Seqüência_da_Propriedade",
                        column: x => x.SeqüênciadaPropriedade,
                        principalTable: "Propriedades",
                        principalColumn: "Seqüência da Propriedade");
                    table.ForeignKey(
                        name: "TB_Movimento_do_Estoque_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Movimento do Estoque Contábil",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadoMovimento = table.Column<DateTime>(name: "Data do Movimento", type: "datetime", nullable: true),
                    TipodoMovimento = table.Column<short>(name: "Tipo do Movimento", type: "smallint", nullable: false),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    TipodoProduto = table.Column<short>(name: "Tipo do Produto", type: "smallint", nullable: false),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Devolucao = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto Contabil", x => x.SeqüênciadoMovimento);
                    table.ForeignKey(
                        name: "TB_Movimento_do_Estoque_Contábil_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Nova Licitação",
                columns: table => new
                {
                    CodigodaLicitação = table.Column<int>(name: "Codigo da Licitação", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtdaLicitação = table.Column<DateTime>(name: "Dt da Licitação", type: "datetime", nullable: true),
                    Contato = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false, defaultValue: ""),
                    PrevdeEntrega = table.Column<DateTime>(name: "Prev de Entrega", type: "datetime", nullable: true),
                    SequenciadoFornecedor = table.Column<int>(name: "Sequencia do Fornecedor", type: "int", nullable: false),
                    SequenciadaTransportadora = table.Column<int>(name: "Sequencia da Transportadora", type: "int", nullable: false),
                    Comprador = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    TipodaLicitação = table.Column<string>(name: "Tipo da Licitação", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    TipodeFrete = table.Column<string>(name: "Tipo de Frete", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    TotalizarFrete = table.Column<bool>(name: "Totalizar Frete", type: "bit", nullable: false),
                    NomedoVendedor = table.Column<string>(name: "Nome do Vendedor", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Observacoes = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CodigodoPedido = table.Column<int>(name: "Codigo do Pedido", type: "int", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    TotaldosProdutos = table.Column<decimal>(name: "Total dos Produtos", type: "decimal(10,2)", nullable: false),
                    TotaldeIcms = table.Column<decimal>(name: "Total de Icms", type: "decimal(10,2)", nullable: false),
                    TotaldeIcmsSt = table.Column<decimal>(name: "Total de Icms St", type: "decimal(10,2)", nullable: false),
                    TotaldeIpi = table.Column<decimal>(name: "Total de Ipi", type: "decimal(10,2)", nullable: false),
                    TotaldeDespesas = table.Column<decimal>(name: "Total de Despesas", type: "decimal(10,2)", nullable: false),
                    TotaldaLicitação = table.Column<decimal>(name: "Total da Licitação", type: "decimal(10,2)", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    Fechado = table.Column<bool>(type: "bit", nullable: false),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    IPInaBcFrete = table.Column<bool>(name: "IPI na Bc Frete", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codigo da Licitação", x => x.CodigodaLicitação);
                    table.ForeignKey(
                        name: "TB_Nova_Licitação_FK_Sequencia_da_Transportadora",
                        column: x => x.SequenciadaTransportadora,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Nova_Licitação_FK_Sequencia_do_Fornecedor",
                        column: x => x.SequenciadoFornecedor,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Orçamento",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Fechamento = table.Column<short>(type: "smallint", nullable: false),
                    ValordoFechamento = table.Column<decimal>(name: "Valor do Fechamento", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosProdutos = table.Column<decimal>(name: "Valor Total IPI dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosConjuntos = table.Column<decimal>(name: "Valor Total IPI dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMS = table.Column<decimal>(name: "Valor Total do ICMS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosProdutos = table.Column<decimal>(name: "Valor Total dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosConjuntos = table.Column<decimal>(name: "Valor Total dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldeProdutosUsados = table.Column<decimal>(name: "Valor Total de Produtos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotalConjuntosUsados = table.Column<decimal>(name: "Valor Total Conjuntos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosServiços = table.Column<decimal>(name: "Valor Total dos Serviços", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoOrçamento = table.Column<decimal>(name: "Valor Total do Orçamento", type: "decimal(11,2)", nullable: false),
                    NomeCliente = table.Column<string>(name: "Nome Cliente", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    Endereco = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    NúmerodoEndereço = table.Column<string>(name: "Número do Endereço", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    SeqüênciadoMunicípio = table.Column<int>(name: "Seqüência do Município", type: "int", nullable: false),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    Telefone = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    Fax = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    SeqüênciadoVendedor = table.Column<int>(name: "Seqüência do Vendedor", type: "int", nullable: false),
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    Tipo = table.Column<short>(type: "smallint", nullable: false),
                    CPFeCNPJ = table.Column<string>(name: "CPF e CNPJ", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    RGeIE = table.Column<string>(name: "RG e IE", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    OcultarValorUnitário = table.Column<bool>(name: "Ocultar Valor Unitário", type: "bit", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    ValorTotalIPIdasPeças = table.Column<decimal>(name: "Valor Total IPI das Peças", type: "decimal(11,2)", nullable: false),
                    ValorTotaldasPeças = table.Column<decimal>(name: "Valor Total das Peças", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValorTotaldasPeçasUsadas = table.Column<decimal>(name: "Valor Total das Peças Usadas", type: "decimal(11,2)", nullable: false),
                    Complemento = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Bairro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    CaixaPostal = table.Column<string>(name: "Caixa Postal", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    ÉPropriedade = table.Column<bool>(name: "É Propriedade", type: "bit", nullable: false),
                    NomedaPropriedade = table.Column<string>(name: "Nome da Propriedade", type: "varchar(62)", unicode: false, maxLength: 62, nullable: false, defaultValue: ""),
                    ValorTotaldaBasedeCálculo = table.Column<decimal>(name: "Valor Total da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoSeguro = table.Column<decimal>(name: "Valor do Seguro", type: "decimal(11,2)", nullable: false),
                    DatadoFechamento = table.Column<DateTime>(name: "Data do Fechamento", type: "datetime", nullable: true),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CódigodoSuframa = table.Column<string>(name: "Código do Suframa", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    Revenda = table.Column<bool>(type: "bit", nullable: false),
                    ValorTotaldoPIS = table.Column<decimal>(name: "Valor Total do PIS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoCOFINS = table.Column<decimal>(name: "Valor Total do COFINS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaBaseST = table.Column<decimal>(name: "Valor Total da Base ST", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMSST = table.Column<decimal>(name: "Valor Total do ICMS ST", type: "decimal(11,2)", nullable: false),
                    AlíquotadoISS = table.Column<decimal>(name: "Alíquota do ISS", type: "decimal(5,2)", nullable: false),
                    ReterISS = table.Column<bool>(name: "Reter ISS", type: "bit", nullable: false),
                    EntregaFutura = table.Column<bool>(name: "Entrega Futura", type: "bit", nullable: false),
                    SeqüênciadaTransportadora = table.Column<int>(name: "Seqüência da Transportadora", type: "int", nullable: false),
                    DatadaAlteração = table.Column<DateTime>(name: "Data da Alteração", type: "datetime", nullable: true),
                    HoradaAlteração = table.Column<DateTime>(name: "Hora da Alteração", type: "datetime", nullable: true),
                    UsuáriodaAlteração = table.Column<string>(name: "Usuário da Alteração", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    VendaFechada = table.Column<bool>(name: "Venda Fechada", type: "bit", nullable: false),
                    OrçamentoAvulso = table.Column<bool>(name: "Orçamento Avulso", type: "bit", nullable: false),
                    ValordoImpostodeRenda = table.Column<decimal>(name: "Valor do Imposto de Renda", type: "decimal(11,2)", nullable: false),
                    FaturaProforma = table.Column<bool>(name: "Fatura Proforma", type: "bit", nullable: false),
                    LocaldeEmbarque = table.Column<string>(name: "Local de Embarque", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    UFdeEmbarque = table.Column<string>(name: "UF de Embarque", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    NúmerodaProforma = table.Column<int>(name: "Número da Proforma", type: "int", nullable: false),
                    SeqüênciadoPaís = table.Column<int>(name: "Seqüência do País", type: "int", nullable: false),
                    ValorTotaldoTributo = table.Column<decimal>(name: "Valor Total do Tributo", type: "decimal(11,2)", nullable: false),
                    ConjuntoAvulso = table.Column<bool>(name: "Conjunto Avulso", type: "bit", nullable: false),
                    DescriçãoConjuntoAvulso = table.Column<string>(name: "Descrição Conjunto Avulso", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    VendedorIntermediario = table.Column<string>(name: "Vendedor Intermediario", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    PercentualdoVendedor = table.Column<decimal>(name: "Percentual do Vendedor", type: "decimal(8,4)", nullable: false),
                    Rebiut = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    PercentualRebiut = table.Column<decimal>(name: "Percentual Rebiut", type: "decimal(8,4)", nullable: false),
                    NaoMovimentarEstoque = table.Column<bool>(name: "Nao Movimentar Estoque", type: "bit", nullable: false),
                    GerouEncargos = table.Column<bool>(name: "Gerou Encargos", type: "bit", nullable: false),
                    Hidroturbo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Areairrigada = table.Column<decimal>(name: "Area irrigada", type: "decimal(8,2)", nullable: false),
                    Precipitaçãobruta = table.Column<decimal>(name: "Precipitação bruta", type: "decimal(8,2)", nullable: false),
                    Horasirrigada = table.Column<decimal>(name: "Horas irrigada", type: "decimal(7,2)", nullable: false),
                    Areatotirrigadaem = table.Column<decimal>(name: "Area tot irrigada em", type: "decimal(7,2)", nullable: false),
                    Aspersor = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Modelodoaspersor = table.Column<string>(name: "Modelo do aspersor", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    Bocaldiametro = table.Column<decimal>(name: "Bocal diametro", type: "decimal(7,2)", nullable: false),
                    Pressãodeserviço = table.Column<decimal>(name: "Pressão de serviço", type: "decimal(7,2)", nullable: false),
                    Alcancedojato = table.Column<decimal>(name: "Alcance do jato", type: "decimal(7,2)", nullable: false),
                    Espaçoentrecarreadores = table.Column<decimal>(name: "Espaço entre carreadores", type: "decimal(7,2)", nullable: false),
                    Faixairrigada = table.Column<decimal>(name: "Faixa irrigada", type: "decimal(7,2)", nullable: false),
                    Desnivelmaximonaarea = table.Column<decimal>(name: "Desnivel maximo na area", type: "decimal(7,2)", nullable: false),
                    Alturadesucção = table.Column<decimal>(name: "Altura de sucção", type: "decimal(7,2)", nullable: false),
                    Alturadoaspersor = table.Column<decimal>(name: "Altura do aspersor", type: "decimal(5,2)", nullable: false),
                    Tempoparadoantespercurso = table.Column<decimal>(name: "Tempo parado antes percurso", type: "decimal(5,2)", nullable: false),
                    Com1 = table.Column<decimal>(name: "Com 1", type: "decimal(8,2)", nullable: false),
                    Com2 = table.Column<decimal>(name: "Com 2", type: "decimal(8,2)", nullable: false),
                    Com3 = table.Column<decimal>(name: "Com 3", type: "decimal(8,2)", nullable: false),
                    ModeloTrechoA = table.Column<int>(name: "Modelo Trecho A", type: "int", nullable: false),
                    ModeloTrechoB = table.Column<int>(name: "Modelo Trecho B", type: "int", nullable: false),
                    ModeloTrechoC = table.Column<int>(name: "Modelo Trecho C", type: "int", nullable: false),
                    Qtdebomba = table.Column<short>(name: "Qtde bomba", type: "smallint", nullable: false),
                    Marcabomba = table.Column<string>(name: "Marca bomba", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Modelobomba = table.Column<string>(name: "Modelo bomba", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Tamanhobomba = table.Column<string>(name: "Tamanho bomba", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Nestagios = table.Column<short>(name: "N estagios", type: "smallint", nullable: false),
                    Diametrobomba = table.Column<decimal>(name: "Diametro bomba", type: "decimal(8,2)", nullable: false),
                    Pressaobomba = table.Column<decimal>(name: "Pressao bomba", type: "decimal(8,2)", nullable: false),
                    Rendimentobomba = table.Column<decimal>(name: "Rendimento bomba", type: "decimal(8,2)", nullable: false),
                    Rotaçãobomba = table.Column<decimal>(name: "Rotação bomba", type: "decimal(8,2)", nullable: false),
                    QtdedeMotor = table.Column<decimal>(name: "Qtde de Motor", type: "decimal(10,2)", nullable: false),
                    MarcadoMotor = table.Column<string>(name: "Marca do Motor", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ModeloMotor = table.Column<string>(name: "Modelo Motor", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    NiveldeProteção = table.Column<string>(name: "Nivel de Proteção", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    PotenciaNominal = table.Column<decimal>(name: "Potencia Nominal", type: "decimal(8,2)", nullable: false),
                    NrodeFases = table.Column<short>(name: "Nro de Fases", type: "smallint", nullable: false),
                    Voltagem = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Modelohidroturbo = table.Column<string>(name: "Modelo hidroturbo", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    Eixos = table.Column<short>(type: "smallint", nullable: false),
                    Rodas = table.Column<short>(type: "smallint", nullable: false),
                    Pneus = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Tubos = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Projetista = table.Column<int>(type: "int", nullable: false),
                    PesoBruto = table.Column<decimal>(name: "Peso Bruto", type: "decimal(11,2)", nullable: false),
                    PesoLíquido = table.Column<decimal>(name: "Peso Líquido", type: "decimal(11,2)", nullable: false),
                    Volumes = table.Column<int>(type: "int", nullable: false),
                    Avisodeembarque = table.Column<string>(name: "Aviso de embarque", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    EntregaTecnica = table.Column<string>(name: "Entrega Tecnica", type: "varchar(19)", unicode: false, maxLength: 19, nullable: false, defaultValue: ""),
                    SequenciadoProjeto = table.Column<int>(name: "Sequencia do Projeto", type: "int", nullable: false),
                    OutrasDespesas = table.Column<decimal>(name: "Outras Despesas", type: "decimal(10,2)", nullable: false),
                    Refaturamento = table.Column<bool>(type: "bit", nullable: false),
                    DatadoPedido = table.Column<DateTime>(name: "Data do Pedido", type: "datetime", nullable: true),
                    DatadeEntrega = table.Column<DateTime>(name: "Data de Entrega", type: "datetime", nullable: true),
                    OrdemInterna = table.Column<bool>(name: "Ordem Interna", type: "bit", nullable: false),
                    OrçamentoVinculado = table.Column<int>(name: "Orçamento Vinculado", type: "int", nullable: false),
                    Frete = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true, defaultValue: ""),
                    PlacaVeiculo = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    UfPlaca = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    NumAntt = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    TotalCBS = table.Column<double>(name: "Total CBS", type: "float", nullable: true, defaultValue: 0.0),
                    TotalIBS = table.Column<double>(name: "Total IBS", type: "float", nullable: true, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Orçamento", x => x.SeqüênciadoOrçamento);
                    table.ForeignKey(
                        name: "TB_Orçamento_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Orçamento_FK_Seqüência_da_Propriedade",
                        column: x => x.SeqüênciadaPropriedade,
                        principalTable: "Propriedades",
                        principalColumn: "Seqüência da Propriedade");
                    table.ForeignKey(
                        name: "TB_Orçamento_FK_Seqüência_da_Transportadora",
                        column: x => x.SeqüênciadaTransportadora,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Orçamento_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Orçamento_FK_Seqüência_do_Município",
                        column: x => x.SeqüênciadoMunicípio,
                        principalTable: "Municipios",
                        principalColumn: "Seqüência do Município");
                    table.ForeignKey(
                        name: "TB_Orçamento_FK_Seqüência_do_País",
                        column: x => x.SeqüênciadoPaís,
                        principalTable: "Paises",
                        principalColumn: "Seqüência do País");
                    table.ForeignKey(
                        name: "TB_Orçamento_FK_Seqüência_do_Vendedor",
                        column: x => x.SeqüênciadoVendedor,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Ordem de Serviço",
                columns: table => new
                {
                    SeqüênciadaOrdemdeServiço = table.Column<int>(name: "Seqüência da Ordem de Serviço", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false),
                    DatadoFechamento = table.Column<DateTime>(name: "Data do Fechamento", type: "datetime", nullable: true),
                    Validade = table.Column<short>(type: "smallint", nullable: false),
                    Fechamento = table.Column<short>(type: "smallint", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    ValordoFechamento = table.Column<decimal>(name: "Valor do Fechamento", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosProdutos = table.Column<decimal>(name: "Valor Total IPI dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosConjuntos = table.Column<decimal>(name: "Valor Total IPI dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMS = table.Column<decimal>(name: "Valor Total do ICMS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosProdutos = table.Column<decimal>(name: "Valor Total dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosConjuntos = table.Column<decimal>(name: "Valor Total dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldeProdutosUsados = table.Column<decimal>(name: "Valor Total de Produtos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotalConjuntosUsados = table.Column<decimal>(name: "Valor Total Conjuntos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosServiços = table.Column<decimal>(name: "Valor Total dos Serviços", type: "decimal(11,2)", nullable: false),
                    ValorTotalOrdemdeServiço = table.Column<decimal>(name: "Valor Total Ordem de Serviço", type: "decimal(11,2)", nullable: false),
                    NomeCliente = table.Column<string>(name: "Nome Cliente", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    ÉPropriedade = table.Column<bool>(name: "É Propriedade", type: "bit", nullable: false),
                    NomedaPropriedade = table.Column<string>(name: "Nome da Propriedade", type: "varchar(62)", unicode: false, maxLength: 62, nullable: false, defaultValue: ""),
                    Endereco = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Complemento = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    NúmerodoEndereço = table.Column<string>(name: "Número do Endereço", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Bairro = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    CaixaPostal = table.Column<string>(name: "Caixa Postal", type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: ""),
                    SeqüênciadoMunicípio = table.Column<int>(name: "Seqüência do Município", type: "int", nullable: false),
                    CEP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    Telefone = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    Fax = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false, defaultValue: ""),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    SeqüênciadoVendedor = table.Column<int>(name: "Seqüência do Vendedor", type: "int", nullable: false),
                    Tipo = table.Column<short>(type: "smallint", nullable: false),
                    CPFeCNPJ = table.Column<string>(name: "CPF e CNPJ", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    RGeIE = table.Column<string>(name: "RG e IE", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    ServiçoemGarantia = table.Column<bool>(name: "Serviço em Garantia", type: "bit", nullable: false),
                    TipodoRelatório = table.Column<string>(name: "Tipo do Relatório", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    ModelodoTrator = table.Column<string>(name: "Modelo do Trator", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    AnodeFabricaçãodoTrator = table.Column<short>(name: "Ano de Fabricação do Trator", type: "smallint", nullable: false),
                    NúmerodoMotordoTrator = table.Column<string>(name: "Número do Motor do Trator", type: "varchar(12)", unicode: false, maxLength: 12, nullable: false, defaultValue: ""),
                    NúmerodoChassidoTrator = table.Column<string>(name: "Número do Chassi do Trator", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    HorímetrodoTrator = table.Column<decimal>(name: "Horímetro do Trator", type: "decimal(7,2)", nullable: false),
                    CordoTrator = table.Column<string>(name: "Cor do Trator", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    KilometragemdoTrator = table.Column<decimal>(name: "Kilometragem do Trator", type: "decimal(6,1)", nullable: false),
                    ValorKmRodadodoTrator = table.Column<decimal>(name: "Valor Km Rodado do Trator", type: "decimal(11,2)", nullable: false),
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    ValorTotaldasPeças = table.Column<decimal>(name: "Valor Total das Peças", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdasPeças = table.Column<decimal>(name: "Valor Total IPI das Peças", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValorTotaldasPeçasUsadas = table.Column<decimal>(name: "Valor Total das Peças Usadas", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaBasedeCálculo = table.Column<decimal>(name: "Valor Total da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoSeguro = table.Column<decimal>(name: "Valor do Seguro", type: "decimal(11,2)", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CódigodoSuframa = table.Column<string>(name: "Código do Suframa", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    Revenda = table.Column<bool>(type: "bit", nullable: false),
                    GerarPedido = table.Column<bool>(name: "Gerar Pedido", type: "bit", nullable: false),
                    ValorTotaldoPIS = table.Column<decimal>(name: "Valor Total do PIS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoCOFINS = table.Column<decimal>(name: "Valor Total do COFINS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaBaseST = table.Column<decimal>(name: "Valor Total da Base ST", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMSST = table.Column<decimal>(name: "Valor Total do ICMS ST", type: "decimal(11,2)", nullable: false),
                    AlíquotadoISS = table.Column<decimal>(name: "Alíquota do ISS", type: "decimal(5,2)", nullable: false),
                    ReterISS = table.Column<bool>(name: "Reter ISS", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Ordem de Serviço", x => x.SeqüênciadaOrdemdeServiço);
                    table.ForeignKey(
                        name: "TB_Ordem_de_Serviço_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Ordem_de_Serviço_FK_Seqüência_da_Propriedade",
                        column: x => x.SeqüênciadaPropriedade,
                        principalTable: "Propriedades",
                        principalColumn: "Seqüência da Propriedade");
                    table.ForeignKey(
                        name: "TB_Ordem_de_Serviço_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Ordem_de_Serviço_FK_Seqüência_do_Município",
                        column: x => x.SeqüênciadoMunicípio,
                        principalTable: "Municipios",
                        principalColumn: "Seqüência do Município");
                    table.ForeignKey(
                        name: "TB_Ordem_de_Serviço_FK_Seqüência_do_Vendedor",
                        column: x => x.SeqüênciadoVendedor,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Propriedades do Geral",
                columns: table => new
                {
                    SeqüênciadaPropriedadeGeral = table.Column<int>(name: "Seqüência da Propriedade Geral", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Propriedade Geral", x => x.SeqüênciadaPropriedadeGeral);
                    table.ForeignKey(
                        name: "TB_Propriedades_do_Geral_FK_Seqüência_da_Propriedade",
                        column: x => x.SeqüênciadaPropriedade,
                        principalTable: "Propriedades",
                        principalColumn: "Seqüência da Propriedade");
                    table.ForeignKey(
                        name: "TB_Propriedades_do_Geral_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Relatorio de Viagem",
                columns: table => new
                {
                    SeqdaViagem = table.Column<int>(name: "Seq da Viagem", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequenciadoGeral = table.Column<int>(name: "Sequencia do Geral", type: "int", nullable: false),
                    PeriodoInicial = table.Column<DateTime>(name: "Periodo Inicial", type: "datetime", nullable: true),
                    PeriodoFinal = table.Column<DateTime>(name: "Periodo Final", type: "datetime", nullable: true),
                    DestinodaViagem = table.Column<string>(name: "Destino da Viagem", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    MotivodaViagem = table.Column<string>(name: "Motivo da Viagem", type: "text", nullable: false, defaultValue: ""),
                    TotaldaViagem = table.Column<decimal>(name: "Total da Viagem", type: "decimal(10,2)", nullable: false),
                    TotaldosItens = table.Column<decimal>(name: "Total dos Itens", type: "decimal(10,2)", nullable: false),
                    Referencia = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Teste = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq da Viagem", x => x.SeqdaViagem);
                    table.ForeignKey(
                        name: "TB_Relatorio_de_Viagem_FK_Sequencia_do_Geral",
                        column: x => x.SequenciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Requisição",
                columns: table => new
                {
                    SeqüênciadaRequisição = table.Column<int>(name: "Seqüência da Requisição", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadaRequisição = table.Column<DateTime>(name: "Data da Requisição", type: "datetime", nullable: true),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    TipodoDesconto = table.Column<short>(name: "Tipo do Desconto", type: "smallint", nullable: false),
                    ValorTotaldaRequisição = table.Column<decimal>(name: "Valor Total da Requisição", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Requisição", x => x.SeqüênciadaRequisição);
                    table.ForeignKey(
                        name: "TB_Requisição_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Consumo do Pedido Compra",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    IdDespesa = table.Column<int>(name: "Id Despesa", type: "int", nullable: false),
                    IddaDespesa = table.Column<int>(name: "Id da Despesa", type: "int", nullable: false),
                    Qtde = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VrUnitario = table.Column<decimal>(name: "Vr Unitario", type: "decimal(10,2)", nullable: false),
                    VrTotal = table.Column<decimal>(name: "Vr Total", type: "decimal(10,2)", nullable: false),
                    AliquotadoIPI = table.Column<decimal>(name: "Aliquota do IPI", type: "decimal(8,4)", nullable: false),
                    AliquotadoIcms = table.Column<short>(name: "Aliquota do Icms", type: "smallint", nullable: false),
                    VrdoIPI = table.Column<decimal>(name: "Vr do IPI", type: "decimal(10,2)", nullable: false),
                    VrdoIcms = table.Column<decimal>(name: "Vr do Icms", type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id Consumo", x => new { x.IddoPedido, x.IdDespesa });
                    table.ForeignKey(
                        name: "TB_Consumo_do_Pedido_Compra_FK_Id_da_Despesa",
                        column: x => x.IddaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                });

            migrationBuilder.CreateTable(
                name: "Despesas da Licitação",
                columns: table => new
                {
                    CodigodaLicitação = table.Column<int>(name: "Codigo da Licitação", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SequenciadaDespesa = table.Column<int>(name: "Sequencia da Despesa", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Cod_e_ItemD", x => new { x.CodigodaLicitação, x.SequenciadoItem });
                    table.ForeignKey(
                        name: "TB_Despesas_da_Licitação_FK_Sequencia_da_Despesa",
                        column: x => x.SequenciadaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                });

            migrationBuilder.CreateTable(
                name: "Despesas do Movimento Contábil",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadaDespesaMovimento = table.Column<int>(name: "Seqüência da Despesa Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaDespesa = table.Column<int>(name: "Seqüência da Despesa", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordaSubstituição = table.Column<decimal>(name: "Valor da Substituição", type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Desp Mvto Cont", x => new { x.SeqüênciadoMovimento, x.SeqüênciadaDespesaMovimento });
                    table.ForeignKey(
                        name: "TB_Despesas_do_Movimento_Contábil_FK_Seqüência_da_Despesa",
                        column: x => x.SeqüênciadaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                });

            migrationBuilder.CreateTable(
                name: "Despesas do Novo Pedido",
                columns: table => new
                {
                    CodigodoPedido = table.Column<int>(name: "Codigo do Pedido", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SequenciadaDespesa = table.Column<int>(name: "Sequencia da Despesa", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Codig_e_Item", x => new { x.CodigodoPedido, x.SequenciadoItem });
                    table.ForeignKey(
                        name: "TB_Despesas_do_Novo_Pedido_FK_Sequencia_da_Despesa",
                        column: x => x.SequenciadaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                });

            migrationBuilder.CreateTable(
                name: "Despesas do Pedido Compra",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    IddaDespesa = table.Column<int>(name: "Id da Despesa", type: "int", nullable: false),
                    Qtde = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VrUnitario = table.Column<decimal>(name: "Vr Unitario", type: "decimal(10,2)", nullable: false),
                    VrTotal = table.Column<decimal>(name: "Vr Total", type: "decimal(10,2)", nullable: false),
                    AliquotadoIPI = table.Column<decimal>(name: "Aliquota do IPI", type: "decimal(8,4)", nullable: false),
                    AliquotadoIcms = table.Column<short>(name: "Aliquota do Icms", type: "smallint", nullable: false),
                    VrdoIPI = table.Column<decimal>(name: "Vr do IPI", type: "decimal(10,2)", nullable: false),
                    VrdoIcms = table.Column<decimal>(name: "Vr do Icms", type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id Despesa", x => new { x.IddoPedido, x.IddaDespesa });
                    table.ForeignKey(
                        name: "TB_Despesas_do_Pedido_Compra_FK_Id_da_Despesa",
                        column: x => x.IddaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                });

            migrationBuilder.CreateTable(
                name: "Despesas Mvto Contábil Novo",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciaDespesaMvtoNovo = table.Column<int>(name: "Seqüência Despesa Mvto Novo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaDespesa = table.Column<int>(name: "Seqüência da Despesa", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordaSubstituição = table.Column<decimal>(name: "Valor da Substituição", type: "decimal(12,4)", nullable: false),
                    Valordoipicompra = table.Column<decimal>(name: "Valor do ipi compra", type: "decimal(12,4)", nullable: false),
                    Valortotalcompra = table.Column<decimal>(name: "Valor total compra", type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Desp Cont Novo", x => new { x.SeqüênciadoMovimento, x.SeqüênciaDespesaMvtoNovo });
                    table.ForeignKey(
                        name: "TB_Despesas_Mvto_Contábil_Novo_FK_Seqüência_da_Despesa",
                        column: x => x.SeqüênciadaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos Mvto Contábil Novo",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciaConjuntoMvtoNovo = table.Column<int>(name: "Seqüência Conjunto Mvto Novo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordaSubstituição = table.Column<decimal>(name: "Valor da Substituição", type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Conj Novo", x => new { x.SeqüênciadoMovimento, x.SeqüênciaConjuntoMvtoNovo });
                    table.ForeignKey(
                        name: "TB_Conjuntos_Mvto_Contábil_Novo_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                });

            migrationBuilder.CreateTable(
                name: "Baixa do Estoque Contábil",
                columns: table => new
                {
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipodoMovimento = table.Column<short>(name: "Tipo do Movimento", type: "smallint", nullable: false),
                    DatadoMovimento = table.Column<DateTime>(name: "Data do Movimento", type: "datetime", nullable: true),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordaSubstituição = table.Column<decimal>(name: "Valor da Substituição", type: "decimal(12,4)", nullable: false),
                    TipodoProduto = table.Column<short>(name: "Tipo do Produto", type: "smallint", nullable: false),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Estoque = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadoItem2 = table.Column<int>(name: "Seqüência do Item 2", type: "int", nullable: false),
                    SeqüênciadaDespesa = table.Column<int>(name: "Seqüência da Despesa", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Baixa Estoque", x => x.SeqüênciadaBaixa);
                    table.ForeignKey(
                        name: "TB_Baixa_do_Estoque_Contábil_FK_Seqüência_da_Despesa",
                        column: x => x.SeqüênciadaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                    table.ForeignKey(
                        name: "TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                    table.ForeignKey(
                        name: "TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Baixa_do_Estoque_Contábil_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Baixa Industrialização",
                columns: table => new
                {
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadoItem = table.Column<short>(name: "Seqüência do Item", type: "smallint", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Bx", x => x.SeqüênciadaBaixa);
                    table.ForeignKey(
                        name: "TB_Baixa_Industrialização_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Baixa MP Produto",
                columns: table => new
                {
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    DatadaBaixa = table.Column<DateTime>(name: "Data da Baixa", type: "datetime", nullable: true),
                    HoradaBaixa = table.Column<DateTime>(name: "Hora da Baixa", type: "datetime", nullable: true),
                    SeqüênciadoItem = table.Column<int>(name: "Seqüência do Item", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    QuantidadedoProduto = table.Column<decimal>(name: "Quantidade do Produto", type: "decimal(9,3)", nullable: false),
                    SeqüênciadaMatériaPrima = table.Column<int>(name: "Seqüência da Matéria Prima", type: "int", nullable: false),
                    QuantidadedaMatériaPrima = table.Column<decimal>(name: "Quantidade da Matéria Prima", type: "decimal(9,3)", nullable: false),
                    CalcularEstoque = table.Column<bool>(name: "Calcular Estoque", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Baixa MP", x => x.SeqüênciadaBaixa);
                    table.ForeignKey(
                        name: "TB_Baixa_MP_Produto_FK_Seqüência_da_Matéria_Prima",
                        column: x => x.SeqüênciadaMatériaPrima,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                    table.ForeignKey(
                        name: "TB_Baixa_MP_Produto_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Importação Produtos Estoque",
                columns: table => new
                {
                    SeqüênciaImportaçãoEstoque = table.Column<int>(name: "Seqüência Importação Estoque", type: "int", nullable: false),
                    SeqüênciaImportaçãoÍtem = table.Column<int>(name: "Seqüência Importação Ítem", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sq Importação Estoque Seq Prod", x => new { x.SeqüênciaImportaçãoEstoque, x.SeqüênciaImportaçãoÍtem });
                    table.ForeignKey(
                        name: "TB_Importação_Produtos_Estoque_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Itens da Licitacao",
                columns: table => new
                {
                    Sequencia = table.Column<int>(type: "int", nullable: false),
                    Produto = table.Column<int>(type: "int", nullable: false),
                    CodDespesa = table.Column<int>(name: "Cod Despesa", type: "int", nullable: false),
                    SequencialdeUm = table.Column<int>(name: "Sequencial de Um", type: "int", nullable: false),
                    Unidade = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Qtde1 = table.Column<decimal>(name: "Qtde 1", type: "decimal(10,2)", nullable: false),
                    VrUnit1 = table.Column<decimal>(name: "Vr Unit 1", type: "decimal(10,2)", nullable: false),
                    VrTotal1 = table.Column<decimal>(name: "Vr Total 1", type: "decimal(10,2)", nullable: false),
                    Qtde2 = table.Column<decimal>(name: "Qtde 2", type: "decimal(10,2)", nullable: false),
                    VrUnit2 = table.Column<decimal>(name: "Vr Unit 2", type: "decimal(10,2)", nullable: false),
                    VrTotal2 = table.Column<decimal>(name: "Vr Total 2", type: "decimal(10,2)", nullable: false),
                    Qtde3 = table.Column<decimal>(name: "Qtde 3", type: "decimal(10,2)", nullable: false),
                    VrUnit3 = table.Column<decimal>(name: "Vr Unit 3", type: "decimal(10,2)", nullable: false),
                    VrTotal3 = table.Column<decimal>(name: "Vr Total 3", type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sequencia", x => new { x.Sequencia, x.Produto, x.CodDespesa, x.SequencialdeUm });
                    table.ForeignKey(
                        name: "TB_Itens_da_Licitacao_FK_Produto",
                        column: x => x.Produto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Itens do Conjunto",
                columns: table => new
                {
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    QuantidadedoProduto = table.Column<decimal>(name: "Quantidade do Produto", type: "decimal(9,3)", nullable: false),
                    PesodoItem = table.Column<decimal>(name: "Peso do Item", type: "decimal(12,3)", nullable: false),
                    PesoTotal = table.Column<decimal>(name: "Peso Total", type: "decimal(12,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Item do Conjunto", x => new { x.SeqüênciadoConjunto, x.SeqüênciadoProduto });
                    table.ForeignKey(
                        name: "TB_Itens_do_Conjunto_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Itens_do_Conjunto_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Matéria Prima",
                columns: table => new
                {
                    SeqüênciadaMatériaPrima = table.Column<int>(name: "Seqüência da Matéria Prima", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    QuantidadedeMatériaPrima = table.Column<decimal>(name: "Quantidade de Matéria Prima", type: "decimal(9,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Matéria Prima e Seq Prod", x => new { x.SeqüênciadaMatériaPrima, x.SeqüênciadoProduto });
                    table.ForeignKey(
                        name: "TB_Matéria_Prima_FK_Seqüência_da_Matéria_Prima",
                        column: x => x.SeqüênciadaMatériaPrima,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                    table.ForeignKey(
                        name: "TB_Matéria_Prima_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Peças do Movimento Estoque",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadaPeçaMovimento = table.Column<int>(name: "Seqüência da Peça Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoICMSST = table.Column<decimal>(name: "Valor do ICMS ST", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Peça", x => new { x.SeqüênciadoMovimento, x.SeqüênciadaPeçaMovimento });
                    table.ForeignKey(
                        name: "TB_Peças_do_Movimento_Estoque_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Produtos da Licitação",
                columns: table => new
                {
                    CodigodaLicitação = table.Column<int>(name: "Codigo da Licitação", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SequenciadoProduto = table.Column<int>(name: "Sequencia do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(8,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CodItem", x => new { x.CodigodaLicitação, x.SequenciadoItem });
                    table.ForeignKey(
                        name: "TB_Produtos_da_Licitação_FK_Sequencia_do_Produto",
                        column: x => x.SequenciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Produtos do Novo Pedido",
                columns: table => new
                {
                    CodigodoPedido = table.Column<int>(name: "Codigo do Pedido", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    SequenciadoProduto = table.Column<int>(name: "Sequencia do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CodPed_e_Item", x => new { x.CodigodoPedido, x.SequenciadoItem });
                    table.ForeignKey(
                        name: "TB_Produtos_do_Novo_Pedido_FK_Sequencia_do_Produto",
                        column: x => x.SequenciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Produtos do Pedido Compra",
                columns: table => new
                {
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    IddoProduto = table.Column<int>(name: "Id do Produto", type: "int", nullable: false),
                    SequenciadoItem = table.Column<int>(name: "Sequencia do Item", type: "int", nullable: false),
                    Qtde = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VrUnitario = table.Column<decimal>(name: "Vr Unitario", type: "decimal(11,4)", nullable: false),
                    VrTotal = table.Column<decimal>(name: "Vr Total", type: "decimal(10,2)", nullable: false),
                    AliquotadoIPI = table.Column<decimal>(name: "Aliquota do IPI", type: "decimal(8,4)", nullable: false),
                    AliquotadoIcms = table.Column<decimal>(name: "Aliquota do Icms", type: "decimal(7,4)", nullable: false),
                    VrdoIPI = table.Column<decimal>(name: "Vr do IPI", type: "decimal(10,2)", nullable: false),
                    VrdoIcms = table.Column<decimal>(name: "Vr do Icms", type: "decimal(10,2)", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ItesProdutos", x => new { x.IddoPedido, x.IddoProduto, x.SequenciadoItem });
                    table.ForeignKey(
                        name: "TB_Produtos_do_Pedido_Compra_FK_Id_do_Produto",
                        column: x => x.IddoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Produtos Mvto Contábil Novo",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadoProdutoMvtoNovo = table.Column<int>(name: "Seqüência do Produto Mvto Novo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordaSubstituição = table.Column<decimal>(name: "Valor da Substituição", type: "decimal(12,4)", nullable: false),
                    Valordoipicompra = table.Column<decimal>(name: "Valor do ipi compra", type: "decimal(12,4)", nullable: false),
                    Valortotalcompra = table.Column<decimal>(name: "Valor total compra", type: "decimal(12,4)", nullable: false),
                    SequenciaUnidadeSpeed = table.Column<int>(name: "Sequencia Unidade Speed", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Prod Mvto Novo", x => new { x.SeqüênciadoMovimento, x.SeqüênciadoProdutoMvtoNovo });
                    table.ForeignKey(
                        name: "TB_Produtos_Mvto_Contábil_Novo_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Transferência de Receita",
                columns: table => new
                {
                    SeqüênciadaTransferência = table.Column<int>(name: "Seqüência da Transferência", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    SeqüênciadaUnidade = table.Column<short>(name: "Seqüência da Unidade", type: "smallint", nullable: false),
                    Localizacao = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    SeqüênciadoGrupoProduto = table.Column<short>(name: "Seqüência do Grupo Produto", type: "smallint", nullable: false),
                    SeqüênciadoSubGrupoProduto = table.Column<short>(name: "Seqüência do SubGrupo Produto", type: "smallint", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    SeqüênciaDoSubGrupoProduto = table.Column<short>(type: "smallint", nullable: false),
                    SeqüênciaDoGrupoProduto = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Transferência", x => x.SeqüênciadaTransferência);
                    table.ForeignKey(
                        name: "TB_Transferência_de_Receita_FK_Seqüência_da_Unidade",
                        column: x => x.SeqüênciadaUnidade,
                        principalTable: "Unidades",
                        principalColumn: "Seqüência da Unidade");
                    table.ForeignKey(
                        name: "TB_Transferência_de_Receita_FK_Seqüência_do_Grupo_Produto",
                        column: x => x.SeqüênciadoGrupoProduto,
                        principalTable: "Grupo do Produto",
                        principalColumn: "Seqüência do Grupo Produto");
                    table.ForeignKey(
                        name: "TB_Transferência_de_Receita_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                    table.ForeignKey(
                        name: "TB_Transferência_de_Receita_FK_Seqüência_do_SubGrupo_Produto_Seqüência_do_Grupo_Produto",
                        columns: x => new { x.SeqüênciaDoSubGrupoProduto, x.SeqüênciaDoGrupoProduto },
                        principalTable: "SubGrupo do Produto",
                        principalColumns: new[] { "Seqüência do SubGrupo Produto", "Seqüência do Grupo Produto" });
                });

            migrationBuilder.CreateTable(
                name: "Adições da Declaração",
                columns: table => new
                {
                    SeqüênciadaAdição = table.Column<int>(name: "Seqüência da Adição", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaDeclaração = table.Column<int>(name: "Seqüência da Declaração", type: "int", nullable: false),
                    NúmerodaAdição = table.Column<short>(name: "Número da Adição", type: "smallint", nullable: false),
                    SeqüêncialdoItemdaAdição = table.Column<short>(name: "Seqüêncial do Item da Adição", type: "smallint", nullable: false),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Adição", x => x.SeqüênciadaAdição);
                    table.ForeignKey(
                        name: "TB_Adições_da_Declaração_FK_Seqüência_da_Declaração",
                        column: x => x.SeqüênciadaDeclaração,
                        principalTable: "Declarações de Importação",
                        principalColumn: "Seqüência da Declaração",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Adições_da_Declaração_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Parcelas Entrada Contas",
                columns: table => new
                {
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    SeqüênciadaEntrada = table.Column<int>(name: "Seqüência da Entrada", type: "int", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    SeqüênciadaCobrança = table.Column<short>(name: "Seqüência da Cobrança", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Num Parcela e Seq da Entrada", x => new { x.NúmerodaParcela, x.SeqüênciadaEntrada });
                    table.ForeignKey(
                        name: "TB_Parcelas_Entrada_Contas_FK_Seqüência_da_Entrada",
                        column: x => x.SeqüênciadaEntrada,
                        principalTable: "Entrada Contas",
                        principalColumn: "Seqüência da Entrada",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos do Movimento Estoque",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciaConjuntoMovimento = table.Column<int>(name: "Seqüência Conjunto Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    PorcentagemdoIPI = table.Column<decimal>(name: "Porcentagem do IPI", type: "decimal(8,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoICMSST = table.Column<decimal>(name: "Valor do ICMS ST", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Movimento e Seq Conjunto", x => new { x.SeqüênciadoMovimento, x.SeqüênciaConjuntoMovimento });
                    table.ForeignKey(
                        name: "TB_Conjuntos_do_Movimento_Estoque_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                    table.ForeignKey(
                        name: "TB_Conjuntos_do_Movimento_Estoque_FK_Seqüência_do_Movimento",
                        column: x => x.SeqüênciadoMovimento,
                        principalTable: "Movimento do Estoque",
                        principalColumn: "Seqüência do Movimento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Despesas do Movimento Estoque",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadaDespesaMovimento = table.Column<int>(name: "Seqüência da Despesa Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaDespesa = table.Column<int>(name: "Seqüência da Despesa", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    PorcentagemdeIPI = table.Column<decimal>(name: "Porcentagem de IPI", type: "decimal(8,4)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    LocalUsado = table.Column<string>(name: "Local Usado", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Despesa Mvto", x => new { x.SeqüênciadoMovimento, x.SeqüênciadaDespesaMovimento });
                    table.ForeignKey(
                        name: "TB_Despesas_do_Movimento_Estoque_FK_Seqüência_da_Despesa",
                        column: x => x.SeqüênciadaDespesa,
                        principalTable: "Despesas",
                        principalColumn: "Seqüência da Despesa");
                    table.ForeignKey(
                        name: "TB_Despesas_do_Movimento_Estoque_FK_Seqüência_do_Movimento",
                        column: x => x.SeqüênciadoMovimento,
                        principalTable: "Movimento do Estoque",
                        principalColumn: "Seqüência do Movimento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcelas Movimento Estoque",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    SeqüênciadaCobrança = table.Column<short>(name: "Seqüência da Cobrança", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Movimento e Pc", x => new { x.NúmerodaParcela, x.SeqüênciadoMovimento });
                    table.ForeignKey(
                        name: "TB_Parcelas_Movimento_Estoque_FK_Seqüência_do_Movimento",
                        column: x => x.SeqüênciadoMovimento,
                        principalTable: "Movimento do Estoque",
                        principalColumn: "Seqüência do Movimento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos do Movimento Estoque",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadoProdutoMovimento = table.Column<int>(name: "Seqüência do Produto Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    PorcentagemdeIPI = table.Column<decimal>(name: "Porcentagem de IPI", type: "decimal(8,4)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValorUnitáriocomImpostos = table.Column<decimal>(name: "Valor Unitário com Impostos", type: "decimal(11,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Prod", x => new { x.SeqüênciadoMovimento, x.SeqüênciadoProdutoMovimento });
                    table.ForeignKey(
                        name: "TB_Produtos_do_Movimento_Estoque_FK_Seqüência_do_Movimento",
                        column: x => x.SeqüênciadoMovimento,
                        principalTable: "Movimento do Estoque",
                        principalColumn: "Seqüência do Movimento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Produtos_do_Movimento_Estoque_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos Movimento Contábil",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciaConjuntoMovimento = table.Column<int>(name: "Seqüência Conjunto Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordaSubstituição = table.Column<decimal>(name: "Valor da Substituição", type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Conjunto", x => new { x.SeqüênciadoMovimento, x.SeqüênciaConjuntoMovimento });
                    table.ForeignKey(
                        name: "TB_Conjuntos_Movimento_Contábil_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                    table.ForeignKey(
                        name: "TB_Conjuntos_Movimento_Contábil_FK_Seqüência_do_Movimento",
                        column: x => x.SeqüênciadoMovimento,
                        principalTable: "Movimento do Estoque Contábil",
                        principalColumn: "Seqüência do Movimento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos do Movimento Contábil",
                columns: table => new
                {
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    SeqüênciadoProdutoMovimento = table.Column<int>(name: "Seqüência do Produto Movimento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValordeCusto = table.Column<decimal>(name: "Valor de Custo", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordaSubstituição = table.Column<decimal>(name: "Valor da Substituição", type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Mvto e Seq Produto", x => new { x.SeqüênciadoMovimento, x.SeqüênciadoProdutoMovimento });
                    table.ForeignKey(
                        name: "TB_Produtos_do_Movimento_Contábil_FK_Seqüência_do_Movimento",
                        column: x => x.SeqüênciadoMovimento,
                        principalTable: "Movimento do Estoque Contábil",
                        principalColumn: "Seqüência do Movimento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Produtos_do_Movimento_Contábil_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos do Orçamento",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciaConjuntoOrçamento = table.Column<int>(name: "Seqüência Conjunto Orçamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(8,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(6,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    Bcpis = table.Column<decimal>(name: "Bc pis", type: "decimal(9,2)", nullable: false),
                    Aliqdopis = table.Column<decimal>(name: "Aliq do pis", type: "decimal(5,2)", nullable: false),
                    Bccofins = table.Column<decimal>(name: "Bc cofins", type: "decimal(9,2)", nullable: false),
                    Aliqdocofins = table.Column<decimal>(name: "Aliq do cofins", type: "decimal(5,2)", nullable: false),
                    ValorDoCBS = table.Column<double>(name: "Valor Do CBS", type: "float", nullable: true, defaultValue: 0.0),
                    ValorDoIBS = table.Column<double>(name: "Valor Do IBS", type: "float", nullable: true, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Orçamento e Seq Conj", x => new { x.SeqüênciadoOrçamento, x.SeqüênciaConjuntoOrçamento });
                    table.ForeignKey(
                        name: "TB_Conjuntos_do_Orçamento_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                    table.ForeignKey(
                        name: "TB_Conjuntos_do_Orçamento_FK_Seqüência_do_Orçamento",
                        column: x => x.SeqüênciadoOrçamento,
                        principalTable: "Orçamento",
                        principalColumn: "Seqüência do Orçamento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcelas Orçamento",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    DescriçãodaCobrança = table.Column<string>(name: "Descrição da Cobrança", type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Orçamento e PC", x => new { x.SeqüênciadoOrçamento, x.NúmerodaParcela });
                    table.ForeignKey(
                        name: "TB_Parcelas_Orçamento_FK_Seqüência_do_Orçamento",
                        column: x => x.SeqüênciadoOrçamento,
                        principalTable: "Orçamento",
                        principalColumn: "Seqüência do Orçamento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Peças do Orçamento",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciaPeçasdoOrçamento = table.Column<int>(name: "Seqüência Peças do Orçamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(8,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(6,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    Bcpis = table.Column<decimal>(name: "Bc pis", type: "decimal(9,2)", nullable: false),
                    Aliqdopis = table.Column<decimal>(name: "Aliq do pis", type: "decimal(5,2)", nullable: false),
                    Bccofins = table.Column<decimal>(name: "Bc cofins", type: "decimal(9,2)", nullable: false),
                    Aliqdocofins = table.Column<decimal>(name: "Aliq do cofins", type: "decimal(5,2)", nullable: false),
                    ValorDoCBS = table.Column<double>(name: "Valor Do CBS", type: "float", nullable: true, defaultValue: 0.0),
                    ValorDoIBS = table.Column<double>(name: "Valor Do IBS", type: "float", nullable: true, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Orçamento e Seq Peças", x => new { x.SeqüênciadoOrçamento, x.SeqüênciaPeçasdoOrçamento });
                    table.ForeignKey(
                        name: "TB_Peças_do_Orçamento_FK_Seqüência_do_Orçamento",
                        column: x => x.SeqüênciadoOrçamento,
                        principalTable: "Orçamento",
                        principalColumn: "Seqüência do Orçamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Peças_do_Orçamento_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Produtos do Orçamento",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciadoProdutoOrçamento = table.Column<int>(name: "Seqüência do Produto Orçamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(8,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(6,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    Bcpis = table.Column<decimal>(name: "Bc pis", type: "decimal(9,2)", nullable: false),
                    Bccofins = table.Column<decimal>(name: "Bc cofins", type: "decimal(9,2)", nullable: false),
                    Aliqdopis = table.Column<decimal>(name: "Aliq do pis", type: "decimal(5,2)", nullable: false),
                    Aliqdocofins = table.Column<decimal>(name: "Aliq do cofins", type: "decimal(5,2)", nullable: false),
                    ValorDoCBS = table.Column<double>(name: "Valor Do CBS", type: "float", nullable: true, defaultValue: 0.0),
                    ValorDoIBS = table.Column<double>(name: "Valor Do IBS", type: "float", nullable: true, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Orçamento e Seq Prod", x => new { x.SeqüênciadoOrçamento, x.SeqüênciadoProdutoOrçamento });
                    table.ForeignKey(
                        name: "TB_Produtos_do_Orçamento_FK_Seqüência_do_Orçamento",
                        column: x => x.SeqüênciadoOrçamento,
                        principalTable: "Orçamento",
                        principalColumn: "Seqüência do Orçamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Produtos_do_Orçamento_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Serviços do Orçamento",
                columns: table => new
                {
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciadoServiçoOrçamento = table.Column<int>(name: "Seqüência do Serviço Orçamento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoServiço = table.Column<short>(name: "Seqüência do Serviço", type: "smallint", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValorAnterior = table.Column<decimal>(name: "Valor Anterior", type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Orçamento e Seq Serv", x => new { x.SeqüênciadoOrçamento, x.SeqüênciadoServiçoOrçamento });
                    table.ForeignKey(
                        name: "TB_Serviços_do_Orçamento_FK_Seqüência_do_Orçamento",
                        column: x => x.SeqüênciadoOrçamento,
                        principalTable: "Orçamento",
                        principalColumn: "Seqüência do Orçamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Serviços_do_Orçamento_FK_Seqüência_do_Serviço",
                        column: x => x.SeqüênciadoServiço,
                        principalTable: "Servicos",
                        principalColumn: "Seqüência do Serviço");
                });

            migrationBuilder.CreateTable(
                name: "VinculaPedidoOrcamento",
                columns: table => new
                {
                    ID_Vinculacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    IddoPedido = table.Column<int>(name: "Id do Pedido", type: "int", nullable: false),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Qtde = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VinculaP__6717027AB8E1433B", x => x.ID_Vinculacao);
                    table.ForeignKey(
                        name: "FK_VinculaPedidoOrcamento_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "FK_VinculaPedidoOrcamento_Orcamento",
                        column: x => x.SeqüênciadoOrçamento,
                        principalTable: "Orçamento",
                        principalColumn: "Seqüência do Orçamento");
                    table.ForeignKey(
                        name: "FK_VinculaPedidoOrcamento_PedidoCompra",
                        column: x => x.IddoPedido,
                        principalTable: "Pedido de Compra Novo",
                        principalColumn: "Id do Pedido");
                    table.ForeignKey(
                        name: "FK_VinculaPedidoOrcamento_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos da Ordem de Serviço",
                columns: table => new
                {
                    SeqüênciadaOrdemdeServiço = table.Column<int>(name: "Seqüência da Ordem de Serviço", type: "int", nullable: false),
                    SeqüênciaConjuntoOS = table.Column<int>(name: "Seqüência Conjunto OS", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Ordem e Seq Conjunto", x => new { x.SeqüênciadaOrdemdeServiço, x.SeqüênciaConjuntoOS });
                    table.ForeignKey(
                        name: "TB_Conjuntos_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço",
                        column: x => x.SeqüênciadaOrdemdeServiço,
                        principalTable: "Ordem de Serviço",
                        principalColumn: "Seqüência da Ordem de Serviço",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Conjuntos_da_Ordem_de_Serviço_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                });

            migrationBuilder.CreateTable(
                name: "Parcelas Ordem de Serviço",
                columns: table => new
                {
                    SeqüênciadaOrdemdeServiço = table.Column<int>(name: "Seqüência da Ordem de Serviço", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Ordem e PC", x => new { x.SeqüênciadaOrdemdeServiço, x.NúmerodaParcela });
                    table.ForeignKey(
                        name: "TB_Parcelas_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço",
                        column: x => x.SeqüênciadaOrdemdeServiço,
                        principalTable: "Ordem de Serviço",
                        principalColumn: "Seqüência da Ordem de Serviço",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Peças da Ordem de Serviço",
                columns: table => new
                {
                    SeqüênciadaOrdemdeServiço = table.Column<int>(name: "Seqüência da Ordem de Serviço", type: "int", nullable: false),
                    SeqüênciaPeçasOS = table.Column<int>(name: "Seqüência Peças OS", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Ordem de Seq Peças", x => new { x.SeqüênciadaOrdemdeServiço, x.SeqüênciaPeçasOS });
                    table.ForeignKey(
                        name: "TB_Peças_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço",
                        column: x => x.SeqüênciadaOrdemdeServiço,
                        principalTable: "Ordem de Serviço",
                        principalColumn: "Seqüência da Ordem de Serviço",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Peças_da_Ordem_de_Serviço_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false),
                    Fechamento = table.Column<short>(type: "smallint", nullable: false),
                    ValordoFechamento = table.Column<decimal>(name: "Valor do Fechamento", type: "decimal(11,2)", nullable: false),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Historico = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    ValorTotalIPIdosProdutos = table.Column<decimal>(name: "Valor Total IPI dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosConjuntos = table.Column<decimal>(name: "Valor Total IPI dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMS = table.Column<decimal>(name: "Valor Total do ICMS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosProdutos = table.Column<decimal>(name: "Valor Total dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosConjuntos = table.Column<decimal>(name: "Valor Total dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldeProdutosUsados = table.Column<decimal>(name: "Valor Total de Produtos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotalConjuntosUsados = table.Column<decimal>(name: "Valor Total Conjuntos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosServiços = table.Column<decimal>(name: "Valor Total dos Serviços", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoPedido = table.Column<decimal>(name: "Valor Total do Pedido", type: "decimal(11,2)", nullable: false),
                    SeqüênciadoOrçamento = table.Column<int>(name: "Seqüência do Orçamento", type: "int", nullable: false),
                    SeqüênciadaOrdemdeServiço = table.Column<int>(name: "Seqüência da Ordem de Serviço", type: "int", nullable: false),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    SeqüênciadoVendedor = table.Column<int>(name: "Seqüência do Vendedor", type: "int", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    OcultarValorUnitário = table.Column<bool>(name: "Ocultar Valor Unitário", type: "bit", nullable: false),
                    PedidoCancelado = table.Column<bool>(name: "Pedido Cancelado", type: "bit", nullable: false),
                    ValorTotalIPIdasPeças = table.Column<decimal>(name: "Valor Total IPI das Peças", type: "decimal(11,2)", nullable: false),
                    ValorTotaldasPeças = table.Column<decimal>(name: "Valor Total das Peças", type: "decimal(11,2)", nullable: false),
                    ValorTotaldasPeçasUsadas = table.Column<decimal>(name: "Valor Total das Peças Usadas", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    ValorTotaldaBasedeCálculo = table.Column<decimal>(name: "Valor Total da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoSeguro = table.Column<decimal>(name: "Valor do Seguro", type: "decimal(11,2)", nullable: false),
                    DatadoFechamento = table.Column<DateTime>(name: "Data do Fechamento", type: "datetime", nullable: true),
                    ValorTotaldoPIS = table.Column<decimal>(name: "Valor Total do PIS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoCOFINS = table.Column<decimal>(name: "Valor Total do COFINS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaBaseST = table.Column<decimal>(name: "Valor Total da Base ST", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMSST = table.Column<decimal>(name: "Valor Total do ICMS ST", type: "decimal(11,2)", nullable: false),
                    AlíquotadoISS = table.Column<decimal>(name: "Alíquota do ISS", type: "decimal(5,2)", nullable: false),
                    ReterISS = table.Column<bool>(name: "Reter ISS", type: "bit", nullable: false),
                    EntregaFutura = table.Column<bool>(name: "Entrega Futura", type: "bit", nullable: false),
                    SeqüênciadaTransportadora = table.Column<int>(name: "Seqüência da Transportadora", type: "int", nullable: false),
                    ValordoImpostodeRenda = table.Column<decimal>(name: "Valor do Imposto de Renda", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoTributo = table.Column<decimal>(name: "Valor Total do Tributo", type: "decimal(11,2)", nullable: false),
                    NaoMovimentarEstoque = table.Column<bool>(name: "Nao Movimentar Estoque", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência do Pedido", x => x.SeqüênciadoPedido);
                    table.ForeignKey(
                        name: "TB_Pedido_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Pedido_FK_Seqüência_da_Ordem_de_Serviço",
                        column: x => x.SeqüênciadaOrdemdeServiço,
                        principalTable: "Ordem de Serviço",
                        principalColumn: "Seqüência da Ordem de Serviço");
                    table.ForeignKey(
                        name: "TB_Pedido_FK_Seqüência_da_Propriedade",
                        column: x => x.SeqüênciadaPropriedade,
                        principalTable: "Propriedades",
                        principalColumn: "Seqüência da Propriedade");
                    table.ForeignKey(
                        name: "TB_Pedido_FK_Seqüência_da_Transportadora",
                        column: x => x.SeqüênciadaTransportadora,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Pedido_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Pedido_FK_Seqüência_do_Orçamento",
                        column: x => x.SeqüênciadoOrçamento,
                        principalTable: "Orçamento",
                        principalColumn: "Seqüência do Orçamento");
                    table.ForeignKey(
                        name: "TB_Pedido_FK_Seqüência_do_Vendedor",
                        column: x => x.SeqüênciadoVendedor,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Produtos da Ordem de Serviço",
                columns: table => new
                {
                    SeqüênciadaOrdemdeServiço = table.Column<int>(name: "Seqüência da Ordem de Serviço", type: "int", nullable: false),
                    SeqüênciaProdutoOS = table.Column<int>(name: "Seqüência Produto OS", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Os e Seq Prod", x => new { x.SeqüênciadaOrdemdeServiço, x.SeqüênciaProdutoOS });
                    table.ForeignKey(
                        name: "TB_Produtos_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço",
                        column: x => x.SeqüênciadaOrdemdeServiço,
                        principalTable: "Ordem de Serviço",
                        principalColumn: "Seqüência da Ordem de Serviço",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Produtos_da_Ordem_de_Serviço_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Serviços da Ordem de Serviço",
                columns: table => new
                {
                    SeqüênciadaOrdemdeServiço = table.Column<int>(name: "Seqüência da Ordem de Serviço", type: "int", nullable: false),
                    SeqüênciaServiçoOS = table.Column<int>(name: "Seqüência Serviço OS", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoServiço = table.Column<short>(name: "Seqüência do Serviço", type: "smallint", nullable: false),
                    Horas = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq OS e Seq Serv", x => new { x.SeqüênciadaOrdemdeServiço, x.SeqüênciaServiçoOS });
                    table.ForeignKey(
                        name: "TB_Serviços_da_Ordem_de_Serviço_FK_Seqüência_da_Ordem_de_Serviço",
                        column: x => x.SeqüênciadaOrdemdeServiço,
                        principalTable: "Ordem de Serviço",
                        principalColumn: "Seqüência da Ordem de Serviço",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Serviços_da_Ordem_de_Serviço_FK_Seqüência_do_Serviço",
                        column: x => x.SeqüênciadoServiço,
                        principalTable: "Servicos",
                        principalColumn: "Seqüência do Serviço");
                });

            migrationBuilder.CreateTable(
                name: "Itens da Requisição",
                columns: table => new
                {
                    SeqüênciadaRequisição = table.Column<int>(name: "Seqüência da Requisição", type: "int", nullable: false),
                    SeqüênciaProdutoRequisição = table.Column<int>(name: "Seqüência Produto Requisição", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false, defaultValue: ""),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    Veiculo = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Req e Seq Prod Requisição", x => new { x.SeqüênciadaRequisição, x.SeqüênciaProdutoRequisição });
                    table.ForeignKey(
                        name: "TB_Itens_da_Requisição_FK_Seqüência_da_Requisição",
                        column: x => x.SeqüênciadaRequisição,
                        principalTable: "Requisição",
                        principalColumn: "Seqüência da Requisição",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos do Pedido",
                columns: table => new
                {
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    SeqüênciadoConjuntoPedido = table.Column<int>(name: "Seqüência do Conjunto Pedido", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Pedido e Seq Conjunto", x => new { x.SeqüênciadoPedido, x.SeqüênciadoConjuntoPedido });
                    table.ForeignKey(
                        name: "TB_Conjuntos_do_Pedido_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                    table.ForeignKey(
                        name: "TB_Conjuntos_do_Pedido_FK_Seqüência_do_Pedido",
                        column: x => x.SeqüênciadoPedido,
                        principalTable: "Pedido",
                        principalColumn: "Seqüência do Pedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nota Fiscal",
                columns: table => new
                {
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NúmerodaNFe = table.Column<int>(name: "Número da NFe", type: "int", nullable: false),
                    NúmerodaNFSe = table.Column<int>(name: "Número da NFSe", type: "int", nullable: false),
                    NúmerodaNotaFiscal = table.Column<int>(name: "Número da Nota Fiscal", type: "int", nullable: false),
                    DatadeEmissão = table.Column<DateTime>(name: "Data de Emissão", type: "datetime", nullable: true),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    SeqüênciadaPropriedade = table.Column<short>(name: "Seqüência da Propriedade", type: "smallint", nullable: false),
                    SeqüênciadaNatureza = table.Column<short>(name: "Seqüência da Natureza", type: "smallint", nullable: false),
                    TransportadoraAvulsa = table.Column<bool>(name: "Transportadora Avulsa", type: "bit", nullable: false),
                    SeqüênciadaTransportadora = table.Column<int>(name: "Seqüência da Transportadora", type: "int", nullable: false),
                    NomedaTransportadoraAvulsa = table.Column<string>(name: "Nome da Transportadora Avulsa", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    PlacadoVeículo = table.Column<string>(name: "Placa do Veículo", type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValue: ""),
                    UFdoVeículo = table.Column<string>(name: "UF do Veículo", type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: ""),
                    Frete = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true, defaultValue: ""),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    Fechamento = table.Column<short>(type: "smallint", nullable: false),
                    ValordoFechamento = table.Column<decimal>(name: "Valor do Fechamento", type: "decimal(11,2)", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    Especie = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    DatadeSaída = table.Column<DateTime>(name: "Data de Saída", type: "datetime", nullable: true),
                    HoradaSaída = table.Column<DateTime>(name: "Hora da Saída", type: "datetime", nullable: true),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    Historico = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    ValorTotalIPIdosProdutos = table.Column<decimal>(name: "Valor Total IPI dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotalIPIdosConjuntos = table.Column<decimal>(name: "Valor Total IPI dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMS = table.Column<decimal>(name: "Valor Total do ICMS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosProdutos = table.Column<decimal>(name: "Valor Total dos Produtos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosConjuntos = table.Column<decimal>(name: "Valor Total dos Conjuntos", type: "decimal(11,2)", nullable: false),
                    ValorTotaldeProdutosUsados = table.Column<decimal>(name: "Valor Total de Produtos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotalConjuntosUsados = table.Column<decimal>(name: "Valor Total Conjuntos Usados", type: "decimal(11,2)", nullable: false),
                    ValorTotaldosServiços = table.Column<decimal>(name: "Valor Total dos Serviços", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaNotaFiscal = table.Column<decimal>(name: "Valor Total da Nota Fiscal", type: "decimal(11,2)", nullable: false),
                    TipodeNota = table.Column<short>(name: "Tipo de Nota", type: "smallint", nullable: false),
                    SeqüênciadaClassificação = table.Column<short>(name: "Seqüência da Classificação", type: "smallint", nullable: false),
                    NotaCancelada = table.Column<bool>(name: "Nota Cancelada", type: "bit", nullable: false),
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    SeqüênciadoVendedor = table.Column<int>(name: "Seqüência do Vendedor", type: "int", nullable: false),
                    SeqüênciadaCobrança = table.Column<short>(name: "Seqüência da Cobrança", type: "smallint", nullable: false),
                    NotaFiscalAvulsa = table.Column<bool>(name: "Nota Fiscal Avulsa", type: "bit", nullable: false),
                    PesoBruto = table.Column<decimal>(name: "Peso Bruto", type: "decimal(11,2)", nullable: false),
                    PesoLíquido = table.Column<decimal>(name: "Peso Líquido", type: "decimal(11,2)", nullable: false),
                    OcultarValorUnitário = table.Column<bool>(name: "Ocultar Valor Unitário", type: "bit", nullable: false),
                    ContraApresentação = table.Column<bool>(name: "Contra Apresentação", type: "bit", nullable: false),
                    MunicípiodaTransportadora = table.Column<int>(name: "Município da Transportadora", type: "int", nullable: false),
                    DocumentodaTransportadora = table.Column<string>(name: "Documento da Transportadora", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    NFeComplementar = table.Column<bool>(name: "NFe Complementar", type: "bit", nullable: false),
                    ChaveAcessoNFeReferenciada = table.Column<string>(name: "Chave Acesso NFe Referenciada", type: "varchar(45)", unicode: false, maxLength: 45, nullable: false, defaultValue: ""),
                    ChavedeAcessodaNFe = table.Column<string>(name: "Chave de Acesso da NFe", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    ProtocolodeAutorizaçãoNFe = table.Column<string>(name: "Protocolo de Autorização NFe", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    DataeHoradaNFe = table.Column<string>(name: "Data e Hora da NFe", type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    Transmitido = table.Column<bool>(type: "bit", nullable: false),
                    Autorizado = table.Column<bool>(type: "bit", nullable: false),
                    NúmerodoRecibodaNFe = table.Column<string>(name: "Número do Recibo da NFe", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Marca = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Numeracao = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    ValorTotalIPIdasPeças = table.Column<decimal>(name: "Valor Total IPI das Peças", type: "decimal(11,2)", nullable: false),
                    ValorTotaldasPeças = table.Column<decimal>(name: "Valor Total das Peças", type: "decimal(11,2)", nullable: false),
                    CódigodaANTT = table.Column<string>(name: "Código da ANTT", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    EndereçodaTransportadora = table.Column<string>(name: "Endereço da Transportadora", type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValue: ""),
                    IEdaTransportadora = table.Column<string>(name: "IE da Transportadora", type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValue: ""),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    ValorTotaldasPeçasUsadas = table.Column<decimal>(name: "Valor Total das Peças Usadas", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaBasedeCálculo = table.Column<decimal>(name: "Valor Total da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoSeguro = table.Column<decimal>(name: "Valor do Seguro", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoPIS = table.Column<decimal>(name: "Valor Total do PIS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoCOFINS = table.Column<decimal>(name: "Valor Total do COFINS", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaBaseST = table.Column<decimal>(name: "Valor Total da Base ST", type: "decimal(11,2)", nullable: false),
                    ValorTotaldoICMSST = table.Column<decimal>(name: "Valor Total do ICMS ST", type: "decimal(11,2)", nullable: false),
                    AlíquotadoISS = table.Column<decimal>(name: "Alíquota do ISS", type: "decimal(5,2)", nullable: false),
                    ReterISS = table.Column<bool>(name: "Reter ISS", type: "bit", nullable: false),
                    ReciboNFSe = table.Column<string>(name: "Recibo NFSe", type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Imprimiu = table.Column<bool>(type: "bit", nullable: false),
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    NúmerodoContrato = table.Column<int>(name: "Número do Contrato", type: "int", nullable: false),
                    ValordoImpostodeRenda = table.Column<decimal>(name: "Valor do Imposto de Renda", type: "decimal(11,2)", nullable: false),
                    ValorTotaldaImportação = table.Column<decimal>(name: "Valor Total da Importação", type: "decimal(11,2)", nullable: false),
                    ConjuntoAvulso = table.Column<bool>(name: "Conjunto Avulso", type: "bit", nullable: false),
                    ValorTotaldoTributo = table.Column<decimal>(name: "Valor Total do Tributo", type: "decimal(11,2)", nullable: false),
                    DescriçãoConjuntoAvulso = table.Column<string>(name: "Descrição Conjunto Avulso", type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, defaultValue: ""),
                    FinNFe = table.Column<short>(type: "smallint", nullable: false),
                    NovoLayout = table.Column<bool>(name: "Novo Layout", type: "bit", nullable: false),
                    NotadeDevolução = table.Column<bool>(name: "Nota de Devolução", type: "bit", nullable: false),
                    ChavedaDevolução = table.Column<string>(name: "Chave da Devolução", type: "varchar(200)", unicode: false, maxLength: 200, nullable: false, defaultValue: ""),
                    OutrasDespesas = table.Column<decimal>(name: "Outras Despesas", type: "decimal(10,2)", nullable: false),
                    ChavedaDevolução2 = table.Column<string>(name: "Chave da Devolução 2", type: "varchar(200)", unicode: false, maxLength: 200, nullable: false, defaultValue: ""),
                    ChavedaDevolução3 = table.Column<string>(name: "Chave da Devolução 3", type: "varchar(200)", unicode: false, maxLength: 200, nullable: false, defaultValue: ""),
                    Canceladanolivro = table.Column<bool>(name: "Cancelada no livro", type: "bit", nullable: false),
                    Refaturamento = table.Column<bool>(type: "bit", nullable: false),
                    Notadevenda = table.Column<int>(name: "Nota de venda", type: "int", nullable: false),
                    Financiamento = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Nota Fiscal", x => x.SeqüênciadaNotaFiscal);
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Município_da_Transportadora",
                        column: x => x.MunicípiodaTransportadora,
                        principalTable: "Municipios",
                        principalColumn: "Seqüência do Município");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_da_Classificação",
                        column: x => x.SeqüênciadaClassificação,
                        principalTable: "Classificação Fiscal",
                        principalColumn: "Seqüência da Classificação");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_da_Cobrança",
                        column: x => x.SeqüênciadaCobrança,
                        principalTable: "Tipo de Cobrança",
                        principalColumn: "Seqüência da Cobrança");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_da_Natureza",
                        column: x => x.SeqüênciadaNatureza,
                        principalTable: "Natureza de Operação",
                        principalColumn: "Seqüência da Natureza");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_da_Propriedade",
                        column: x => x.SeqüênciadaPropriedade,
                        principalTable: "Propriedades",
                        principalColumn: "Seqüência da Propriedade");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_da_Transportadora",
                        column: x => x.SeqüênciadaTransportadora,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_do_Movimento",
                        column: x => x.SeqüênciadoMovimento,
                        principalTable: "Movimento do Estoque",
                        principalColumn: "Seqüência do Movimento");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_do_Pedido",
                        column: x => x.SeqüênciadoPedido,
                        principalTable: "Pedido",
                        principalColumn: "Seqüência do Pedido");
                    table.ForeignKey(
                        name: "TB_Nota_Fiscal_FK_Seqüência_do_Vendedor",
                        column: x => x.SeqüênciadoVendedor,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Parcelas Pedido",
                columns: table => new
                {
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Pedido e PC", x => new { x.SeqüênciadoPedido, x.NúmerodaParcela });
                    table.ForeignKey(
                        name: "TB_Parcelas_Pedido_FK_Seqüência_do_Pedido",
                        column: x => x.SeqüênciadoPedido,
                        principalTable: "Pedido",
                        principalColumn: "Seqüência do Pedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Peças do Pedido",
                columns: table => new
                {
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    SeqüênciadaPeçaPedido = table.Column<int>(name: "Seqüência da Peça Pedido", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Pedido e Seq Peça", x => new { x.SeqüênciadoPedido, x.SeqüênciadaPeçaPedido });
                    table.ForeignKey(
                        name: "TB_Peças_do_Pedido_FK_Seqüência_do_Pedido",
                        column: x => x.SeqüênciadoPedido,
                        principalTable: "Pedido",
                        principalColumn: "Seqüência do Pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Peças_do_Pedido_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Produtos do Pedido",
                columns: table => new
                {
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    SeqüênciadoProdutoPedido = table.Column<int>(name: "Seqüência do Produto Pedido", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(5,2)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(5,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Pedido e Seq Produto", x => new { x.SeqüênciadoPedido, x.SeqüênciadoProdutoPedido });
                    table.ForeignKey(
                        name: "TB_Produtos_do_Pedido_FK_Seqüência_do_Pedido",
                        column: x => x.SeqüênciadoPedido,
                        principalTable: "Pedido",
                        principalColumn: "Seqüência do Pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Produtos_do_Pedido_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Serviços do Pedido",
                columns: table => new
                {
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    SeqüênciadoServiçoPedido = table.Column<int>(name: "Seqüência do Serviço Pedido", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoServiço = table.Column<short>(name: "Seqüência do Serviço", type: "smallint", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Pedido e Seq Serv", x => new { x.SeqüênciadoPedido, x.SeqüênciadoServiçoPedido });
                    table.ForeignKey(
                        name: "TB_Serviços_do_Pedido_FK_Seqüência_do_Pedido",
                        column: x => x.SeqüênciadoPedido,
                        principalTable: "Pedido",
                        principalColumn: "Seqüência do Pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Serviços_do_Pedido_FK_Seqüência_do_Serviço",
                        column: x => x.SeqüênciadoServiço,
                        principalTable: "Servicos",
                        principalColumn: "Seqüência do Serviço");
                });

            migrationBuilder.CreateTable(
                name: "Cancelamento NFe",
                columns: table => new
                {
                    SeqüênciaCancelamentoNFe = table.Column<int>(name: "Seqüência Cancelamento NFe", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    Justificativa = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: ""),
                    Ambiente = table.Column<short>(type: "smallint", nullable: false),
                    DatadoCancelamento = table.Column<DateTime>(name: "Data do Cancelamento", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Can NFe e NF", x => new { x.SeqüênciaCancelamentoNFe, x.SeqüênciadaNotaFiscal });
                    table.ForeignKey(
                        name: "TB_Cancelamento_NFe_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal");
                });

            migrationBuilder.CreateTable(
                name: "Carta de Correção NFe",
                columns: table => new
                {
                    SeqüênciadaCorreção = table.Column<int>(name: "Seqüência da Correção", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    NúmerodaCorreção = table.Column<short>(name: "Número da Correção", type: "smallint", nullable: false),
                    JustificativaCCe = table.Column<string>(name: "Justificativa CCe", type: "text", nullable: false, defaultValue: ""),
                    DataCorreção = table.Column<DateTime>(name: "Data Correção", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Cor e Seq NF", x => new { x.SeqüênciadaCorreção, x.SeqüênciadaNotaFiscal });
                    table.ForeignKey(
                        name: "TB_Carta_de_Correção_NFe_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal");
                });

            migrationBuilder.CreateTable(
                name: "Comissao",
                columns: table => new
                {
                    SeqüênciadaComissão = table.Column<int>(name: "Seqüência da Comissão", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PercentualdeComissão = table.Column<decimal>(name: "Percentual de Comissão", type: "decimal(6,2)", nullable: false),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    Percentual2 = table.Column<decimal>(name: "Percentual 2", type: "decimal(6,2)", nullable: false),
                    Intermediario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Comissão", x => x.SeqüênciadaComissão);
                    table.ForeignKey(
                        name: "TB_Comissão_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal");
                });

            migrationBuilder.CreateTable(
                name: "Conjuntos da Nota Fiscal",
                columns: table => new
                {
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    SeqüênciaConjuntoNotaFiscal = table.Column<int>(name: "Seqüência Conjunto Nota Fiscal", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoConjunto = table.Column<int>(name: "Seqüência do Conjunto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(8,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(6,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    Bcpis = table.Column<decimal>(name: "Bc pis", type: "decimal(9,2)", nullable: false),
                    Aliqdopis = table.Column<decimal>(name: "Aliq do pis", type: "decimal(5,2)", nullable: false),
                    Bccofins = table.Column<decimal>(name: "Bc cofins", type: "decimal(9,2)", nullable: false),
                    Aliqdocofins = table.Column<decimal>(name: "Aliq do cofins", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq NF e Seq Conj Nota Fiscal", x => new { x.SeqüênciadaNotaFiscal, x.SeqüênciaConjuntoNotaFiscal });
                    table.ForeignKey(
                        name: "TB_Conjuntos_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Conjuntos_da_Nota_Fiscal_FK_Seqüência_do_Conjunto",
                        column: x => x.SeqüênciadoConjunto,
                        principalTable: "Conjuntos",
                        principalColumn: "Seqüência do Conjunto");
                });

            migrationBuilder.CreateTable(
                name: "Manutenção Contas",
                columns: table => new
                {
                    SeqüênciadaManutenção = table.Column<int>(name: "Seqüência da Manutenção", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Parcela = table.Column<short>(type: "smallint", nullable: false),
                    SeqüênciadoGeral = table.Column<int>(name: "Seqüência do Geral", type: "int", nullable: false),
                    NúmerodaNotaFiscal = table.Column<int>(name: "Número da Nota Fiscal", type: "int", nullable: false),
                    DatadeEntrada = table.Column<DateTime>(name: "Data de Entrada", type: "datetime", nullable: true),
                    Historico = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    FormadePagamento = table.Column<string>(name: "Forma de Pagamento", type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: true),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false),
                    ValorPago = table.Column<decimal>(name: "Valor Pago", type: "decimal(11,2)", nullable: false),
                    ValordoJuros = table.Column<decimal>(name: "Valor do Juros", type: "decimal(11,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    ValorRestante = table.Column<decimal>(name: "Valor Restante", type: "decimal(11,2)", nullable: false),
                    TipodaConta = table.Column<string>(name: "Tipo da Conta", type: "varchar(11)", unicode: false, maxLength: 11, nullable: false, defaultValue: ""),
                    DatadaBaixa = table.Column<DateTime>(name: "Data da Baixa", type: "datetime", nullable: true),
                    SeqüênciadaCobrança = table.Column<short>(name: "Seqüência da Cobrança", type: "smallint", nullable: false),
                    NúmerodaDuplicata = table.Column<int>(name: "Número da Duplicata", type: "int", nullable: false),
                    SeqüênciadaOrigem = table.Column<int>(name: "Seqüência da Origem", type: "int", nullable: false),
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    ChequeImpresso = table.Column<bool>(name: "Cheque Impresso", type: "bit", nullable: false),
                    Conta = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    SeqüênciadoEstoque = table.Column<int>(name: "Seqüência do Estoque", type: "int", nullable: false),
                    SeqüênciadoPedido = table.Column<int>(name: "Seqüência do Pedido", type: "int", nullable: false),
                    DuplicataImpressa = table.Column<bool>(name: "Duplicata Impressa", type: "bit", nullable: false),
                    Imprimir = table.Column<bool>(type: "bit", nullable: false),
                    TpodeRecebimento = table.Column<string>(name: "Tpo de Recebimento", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Previsao = table.Column<bool>(type: "bit", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValue: ""),
                    SequenciadaCompra = table.Column<int>(name: "Sequencia da Compra", type: "int", nullable: false),
                    NotasdaCompra = table.Column<string>(name: "Notas da Compra", type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: ""),
                    Conciliado = table.Column<bool>(type: "bit", nullable: false),
                    VencimentoOriginal = table.Column<DateTime>(name: "Vencimento Original", type: "datetime", nullable: true),
                    SequenciaLanCC = table.Column<int>(name: "Sequencia Lan CC", type: "int", nullable: false),
                    VrdaPrevisão = table.Column<decimal>(name: "Vr da Previsão", type: "decimal(10,2)", nullable: false),
                    ImpPrevisao = table.Column<bool>(name: "Imp Previsao", type: "bit", nullable: false),
                    SeqüênciadoMovimento = table.Column<int>(name: "Seqüência do Movimento", type: "int", nullable: false),
                    CodigodoDebito = table.Column<int>(name: "Codigo do Debito", type: "int", nullable: false),
                    CodigodoCredito = table.Column<int>(name: "Codigo do Credito", type: "int", nullable: false),
                    SeqüênciaSubGrupoDespesa0 = table.Column<short>(name: "SeqüênciaSubGrupoDespesa", type: "smallint", nullable: false),
                    SeqüênciaGrupoDespesa0 = table.Column<short>(name: "SeqüênciaGrupoDespesa", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência Manutenção", x => x.SeqüênciadaManutenção);
                    table.ForeignKey(
                        name: "TB_Manutenção_Contas_FK_Seqüência_Grupo_Despesa",
                        column: x => x.SeqüênciaGrupoDespesa,
                        principalTable: "Grupo da Despesa",
                        principalColumn: "Seqüência Grupo Despesa");
                    table.ForeignKey(
                        name: "TB_Manutenção_Contas_FK_Seqüência_SubGrupo_Despesa_Seqüência_Grupo_Despesa",
                        columns: x => new { x.SeqüênciaSubGrupoDespesa0, x.SeqüênciaGrupoDespesa0 },
                        principalTable: "SubGrupo Despesa",
                        principalColumns: new[] { "Seqüência SubGrupo Despesa", "Seqüência Grupo Despesa" });
                    table.ForeignKey(
                        name: "TB_Manutenção_Contas_FK_Seqüência_da_Cobrança",
                        column: x => x.SeqüênciadaCobrança,
                        principalTable: "Tipo de Cobrança",
                        principalColumn: "Seqüência da Cobrança");
                    table.ForeignKey(
                        name: "TB_Manutenção_Contas_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal");
                    table.ForeignKey(
                        name: "TB_Manutenção_Contas_FK_Seqüência_do_Geral",
                        column: x => x.SeqüênciadoGeral,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                });

            migrationBuilder.CreateTable(
                name: "Notas Autorizadas",
                columns: table => new
                {
                    SeqüênciadoNotas = table.Column<int>(name: "Seqüência do Notas", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    XML = table.Column<string>(type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Notas E Seq NF", x => new { x.SeqüênciadoNotas, x.SeqüênciadaNotaFiscal });
                    table.ForeignKey(
                        name: "TB_Notas_Autorizadas_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal");
                });

            migrationBuilder.CreateTable(
                name: "Parcelas Nota Fiscal",
                columns: table => new
                {
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    NúmerodaParcela = table.Column<short>(name: "Número da Parcela", type: "smallint", nullable: false),
                    Dias = table.Column<short>(type: "smallint", nullable: false),
                    DatadeVencimento = table.Column<DateTime>(name: "Data de Vencimento", type: "datetime", nullable: false),
                    ValordaParcela = table.Column<decimal>(name: "Valor da Parcela", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq NF e PC", x => new { x.SeqüênciadaNotaFiscal, x.NúmerodaParcela });
                    table.ForeignKey(
                        name: "TB_Parcelas_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Peças da Nota Fiscal",
                columns: table => new
                {
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    SeqüênciadaPeçaNotaFiscal = table.Column<int>(name: "Seqüência da Peça Nota Fiscal", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(8,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(6,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    Bcpis = table.Column<decimal>(name: "Bc pis", type: "decimal(9,2)", nullable: false),
                    Aliqdopis = table.Column<decimal>(name: "Aliq do pis", type: "decimal(5,2)", nullable: false),
                    Bccofins = table.Column<decimal>(name: "Bc cofins", type: "decimal(9,2)", nullable: false),
                    Aliqdocofins = table.Column<decimal>(name: "Aliq do cofins", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq NF e Seq Peças Nota Fiscal", x => new { x.SeqüênciadaNotaFiscal, x.SeqüênciadaPeçaNotaFiscal });
                    table.ForeignKey(
                        name: "TB_Peças_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Peças_da_Nota_Fiscal_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Produtos da Nota Fiscal",
                columns: table => new
                {
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    SeqüênciaProdutoNotaFiscal = table.Column<int>(name: "Seqüência Produto Nota Fiscal", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoProduto = table.Column<int>(name: "Seqüência do Produto", type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(12,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(12,4)", nullable: false),
                    ValordoIPI = table.Column<decimal>(name: "Valor do IPI", type: "decimal(12,4)", nullable: false),
                    ValordoICMS = table.Column<decimal>(name: "Valor do ICMS", type: "decimal(12,4)", nullable: false),
                    AlíquotadoIPI = table.Column<decimal>(name: "Alíquota do IPI", type: "decimal(8,4)", nullable: false),
                    AlíquotadoICMS = table.Column<decimal>(name: "Alíquota do ICMS", type: "decimal(5,2)", nullable: false),
                    PercentualdaRedução = table.Column<decimal>(name: "Percentual da Redução", type: "decimal(6,2)", nullable: false),
                    Diferido = table.Column<bool>(type: "bit", nullable: false),
                    ValordaBasedeCálculo = table.Column<decimal>(name: "Valor da Base de Cálculo", type: "decimal(11,2)", nullable: false),
                    ValordoPIS = table.Column<decimal>(name: "Valor do PIS", type: "decimal(11,4)", nullable: false),
                    ValordoCofins = table.Column<decimal>(name: "Valor do Cofins", type: "decimal(11,4)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    BasedeCálculoST = table.Column<decimal>(name: "Base de Cálculo ST", type: "decimal(11,2)", nullable: false),
                    ValorICMSST = table.Column<decimal>(name: "Valor ICMS ST", type: "decimal(11,2)", nullable: false),
                    CFOP = table.Column<short>(type: "smallint", nullable: false),
                    CST = table.Column<short>(type: "smallint", nullable: false),
                    AlíquotadoICMSST = table.Column<decimal>(name: "Alíquota do ICMS ST", type: "decimal(5,2)", nullable: false),
                    BasedeCálculodaImportação = table.Column<decimal>(name: "Base de Cálculo da Importação", type: "decimal(11,2)", nullable: false),
                    ValordasDespesasAduaneiras = table.Column<decimal>(name: "Valor das Despesas Aduaneiras", type: "decimal(11,2)", nullable: false),
                    ValordoImpostodeImportação = table.Column<decimal>(name: "Valor do Imposto de Importação", type: "decimal(11,2)", nullable: false),
                    ValordoIOF = table.Column<decimal>(name: "Valor do IOF", type: "decimal(11,2)", nullable: false),
                    ValordoTributo = table.Column<decimal>(name: "Valor do Tributo", type: "decimal(11,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    ValordoFrete = table.Column<decimal>(name: "Valor do Frete", type: "decimal(12,4)", nullable: false),
                    Bcpis = table.Column<decimal>(name: "Bc pis", type: "decimal(9,2)", nullable: false),
                    Bccofins = table.Column<decimal>(name: "Bc cofins", type: "decimal(9,2)", nullable: false),
                    Aliqdopis = table.Column<decimal>(name: "Aliq do pis", type: "decimal(5,2)", nullable: false),
                    Aliqdocofins = table.Column<decimal>(name: "Aliq do cofins", type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq NF e Seq Prod Nota Fiscal", x => new { x.SeqüênciadaNotaFiscal, x.SeqüênciaProdutoNotaFiscal });
                    table.ForeignKey(
                        name: "TB_Produtos_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Produtos_da_Nota_Fiscal_FK_Seqüência_do_Produto",
                        column: x => x.SeqüênciadoProduto,
                        principalTable: "Produtos",
                        principalColumn: "Seqüência do Produto");
                });

            migrationBuilder.CreateTable(
                name: "Serviços da Nota Fiscal",
                columns: table => new
                {
                    SeqüênciadaNotaFiscal = table.Column<int>(name: "Seqüência da Nota Fiscal", type: "int", nullable: false),
                    SeqüênciaServiçoNotaFiscal = table.Column<int>(name: "Seqüência Serviço Nota Fiscal", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadoServiço = table.Column<short>(name: "Seqüência do Serviço", type: "smallint", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    ValorUnitário = table.Column<decimal>(name: "Valor Unitário", type: "decimal(11,4)", nullable: false),
                    ValorTotal = table.Column<decimal>(name: "Valor Total", type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq NF e Seq Serv Nota Fiscal", x => new { x.SeqüênciadaNotaFiscal, x.SeqüênciaServiçoNotaFiscal });
                    table.ForeignKey(
                        name: "TB_Serviços_da_Nota_Fiscal_FK_Seqüência_da_Nota_Fiscal",
                        column: x => x.SeqüênciadaNotaFiscal,
                        principalTable: "Nota Fiscal",
                        principalColumn: "Seqüência da Nota Fiscal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "TB_Serviços_da_Nota_Fiscal_FK_Seqüência_do_Serviço",
                        column: x => x.SeqüênciadoServiço,
                        principalTable: "Servicos",
                        principalColumn: "Seqüência do Serviço");
                });

            migrationBuilder.CreateTable(
                name: "Baixa Contas",
                columns: table => new
                {
                    SeqüênciadaBaixa = table.Column<int>(name: "Seqüência da Baixa", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaManutenção = table.Column<int>(name: "Seqüência da Manutenção", type: "int", nullable: false),
                    DatadaBaixa = table.Column<DateTime>(name: "Data da Baixa", type: "datetime", nullable: true),
                    ValorPago = table.Column<decimal>(name: "Valor Pago", type: "decimal(11,2)", nullable: false),
                    ValordoJuros = table.Column<decimal>(name: "Valor do Juros", type: "decimal(11,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    SeqüênciadaAgência = table.Column<short>(name: "Seqüência da Agência", type: "smallint", nullable: false),
                    SeqüênciadaCCdaAgência = table.Column<short>(name: "Seqüência da CC da Agência", type: "smallint", nullable: false),
                    NúmerodoCheque = table.Column<string>(name: "Número do Cheque", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Conta = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: ""),
                    Historico = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    SeqüênciadaMovimentaçãoCC = table.Column<int>(name: "Seqüência da Movimentação CC", type: "int", nullable: false),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false),
                    Carteira = table.Column<string>(type: "varchar(17)", unicode: false, maxLength: 17, nullable: false, defaultValue: ""),
                    ClienteCarteira = table.Column<string>(name: "Cliente Carteira", type: "varchar(9)", unicode: false, maxLength: 9, nullable: false, defaultValue: ""),
                    DataRecebimento = table.Column<DateTime>(name: "Data Recebimento", type: "datetime", nullable: true),
                    Compensado = table.Column<bool>(type: "bit", nullable: false),
                    Pago = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DtComissão = table.Column<DateTime>(name: "Dt Comissão", type: "datetime", nullable: true),
                    NFComissão = table.Column<int>(name: "NF Comissão", type: "int", nullable: false),
                    CodigodoHistorico = table.Column<short>(name: "Codigo do Historico", type: "smallint", nullable: false),
                    CodigodoDebito = table.Column<int>(name: "Codigo do Debito", type: "int", nullable: false),
                    CodigodoCredito = table.Column<int>(name: "Codigo do Credito", type: "int", nullable: false),
                    SeqüênciaGrupoDespesa = table.Column<short>(name: "Seqüência Grupo Despesa", type: "smallint", nullable: false),
                    SeqüênciaSubGrupoDespesa = table.Column<short>(name: "Seqüência SubGrupo Despesa", type: "smallint", nullable: false),
                    Beneficiario = table.Column<int>(type: "int", nullable: false),
                    ProcessadoAutomaticamente = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    SequenciaLancamentoBB = table.Column<int>(type: "int", nullable: true),
                    DataCriacaoIntegracao = table.Column<DateTime>(type: "datetime", nullable: true),
                    SeqüênciaDaAgência = table.Column<short>(type: "smallint", nullable: false),
                    SeqüênciaDaCcDaAgência = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seqüência da Baixa", x => x.SeqüênciadaBaixa);
                    table.ForeignKey(
                        name: "TB_Baixa_Contas_FK_Seqüência_da_Agência_Seqüência_da_CC_da_Agência",
                        columns: x => new { x.SeqüênciaDaAgência, x.SeqüênciaDaCcDaAgência },
                        principalTable: "Conta Corrente da Agência",
                        principalColumns: new[] { "Seqüência da Agência", "Seqüência da CC da Agência" });
                    table.ForeignKey(
                        name: "TB_Baixa_Contas_FK_Seqüência_da_Manutenção",
                        column: x => x.SeqüênciadaManutenção,
                        principalTable: "Manutenção Contas",
                        principalColumn: "Seqüência da Manutenção");
                    table.ForeignKey(
                        name: "TB_Baixa_Contas_FK_Seqüência_da_Movimentação_CC",
                        column: x => x.SeqüênciadaMovimentaçãoCC,
                        principalTable: "Movimentação da Conta Corrente",
                        principalColumn: "Seqüência da Movimentação CC");
                });

            migrationBuilder.CreateTable(
                name: "Valores Adicionais",
                columns: table => new
                {
                    SeqüênciadoValores = table.Column<int>(name: "Seqüência do Valores", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeqüênciadaManutenção = table.Column<int>(name: "Seqüência da Manutenção", type: "int", nullable: false),
                    ValordoJuros = table.Column<decimal>(name: "Valor do Juros", type: "decimal(11,2)", nullable: false),
                    ValordoDesconto = table.Column<decimal>(name: "Valor do Desconto", type: "decimal(11,2)", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Conta = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Seq Valores e Seq Man", x => new { x.SeqüênciadoValores, x.SeqüênciadaManutenção });
                    table.ForeignKey(
                        name: "TB_Valores_Adicionais_FK_Seqüência_da_Manutenção",
                        column: x => x.SeqüênciadaManutenção,
                        principalTable: "Manutenção Contas",
                        principalColumn: "Seqüência da Manutenção");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adições da Declaração_Seqüência do Geral",
                table: "Adições da Declaração",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "Seq Declaração e Número Adição",
                table: "Adições da Declaração",
                columns: new[] { "Seqüência da Declaração", "Número da Adição" },
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Hora",
                table: "Agendamento de Backup",
                column: "Hora",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Baixa Contas_Seqüência da Manutenção",
                table: "Baixa Contas",
                column: "Seqüência da Manutenção");

            migrationBuilder.CreateIndex(
                name: "IX_Baixa Contas_Seqüência da Movimentação CC",
                table: "Baixa Contas",
                column: "Seqüência da Movimentação CC");

            migrationBuilder.CreateIndex(
                name: "IX_Baixa Contas_SeqüênciaDaAgência_SeqüênciaDaCcDaAgência",
                table: "Baixa Contas",
                columns: new[] { "SeqüênciaDaAgência", "SeqüênciaDaCcDaAgência" });

            migrationBuilder.CreateIndex(
                name: "IX_Baixa do Estoque Contábil_Seqüência da Despesa",
                table: "Baixa do Estoque Contábil",
                column: "Seqüência da Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Baixa do Estoque Contábil_Seqüência do Conjunto",
                table: "Baixa do Estoque Contábil",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Baixa do Estoque Contábil_Seqüência do Geral",
                table: "Baixa do Estoque Contábil",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Baixa do Estoque Contábil_Seqüência do Produto",
                table: "Baixa do Estoque Contábil",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Baixa Industrialização_Seqüência do Produto",
                table: "Baixa Industrialização",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "Seq Mvto Seq Bx e Seq Item",
                table: "Baixa Industrialização",
                columns: new[] { "Seqüência do Movimento", "Seqüência do Item", "Seqüência da Baixa" },
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Baixa MP Produto_Seqüência da Matéria Prima",
                table: "Baixa MP Produto",
                column: "Seqüência da Matéria Prima");

            migrationBuilder.CreateIndex(
                name: "IX_Baixa MP Produto_Seqüência do Produto",
                table: "Baixa MP Produto",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "Dta do Feriado",
                table: "Calendario",
                column: "Dta do Feriado",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Seq NF",
                table: "Cancelamento NFe",
                column: "Seqüência da Nota Fiscal",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Seq NF e Num Cor",
                table: "Carta de Correção NFe",
                columns: new[] { "Seqüência da Nota Fiscal", "Número da Correção" },
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Seq NF Comissao",
                table: "Comissao",
                column: "Seqüência da Nota Fiscal",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "UK_ConfiguracaoIntegracao_Chave",
                table: "ConfiguracaoIntegracao",
                column: "Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos_Seqüência da Classificação",
                table: "Conjuntos",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos_Seqüência da Unidade",
                table: "Conjuntos",
                column: "Seqüência da Unidade");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos_Seqüência do Grupo Produto",
                table: "Conjuntos",
                column: "Seqüência do Grupo Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos_SeqüênciaDoSubGrupoProduto_SeqüênciaDoGrupoProduto",
                table: "Conjuntos",
                columns: new[] { "SeqüênciaDoSubGrupoProduto", "SeqüênciaDoGrupoProduto" });

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos da Nota Fiscal_Seqüência do Conjunto",
                table: "Conjuntos da Nota Fiscal",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos da Ordem de Serviço_Seqüência do Conjunto",
                table: "Conjuntos da Ordem de Serviço",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos do Movimento Estoque_Seqüência do Conjunto",
                table: "Conjuntos do Movimento Estoque",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos do Orçamento_Seqüência do Conjunto",
                table: "Conjuntos do Orçamento",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos do Pedido_Seqüência do Conjunto",
                table: "Conjuntos do Pedido",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos Movimento Contábil_Seqüência do Conjunto",
                table: "Conjuntos Movimento Contábil",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Conjuntos Mvto Contábil Novo_Seqüência do Conjunto",
                table: "Conjuntos Mvto Contábil Novo",
                column: "Seqüência do Conjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Consumo do Pedido Compra_Id da Despesa",
                table: "Consumo do Pedido Compra",
                column: "Id da Despesa");

            migrationBuilder.CreateIndex(
                name: "Conta_Contab",
                table: "Conta Contabil",
                column: "Conta Contabil",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Número da Conta Corrente",
                table: "Conta Corrente da Agência",
                columns: new[] { "Número da Conta Corrente", "Seqüência da Agência" },
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Id_e_Seq_Compra",
                table: "Controle de Compras",
                columns: new[] { "Id do Pedido", "Sequencia do Item" })
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Controle de Processos_Codigo do Advogado",
                table: "Controle de Processos",
                column: "Codigo do Advogado");

            migrationBuilder.CreateIndex(
                name: "IX_Controle de Processos_Codigo do Status",
                table: "Controle de Processos",
                column: "Codigo do Status");

            migrationBuilder.CreateIndex(
                name: "IX_Declarações de Importação_Seqüência do Geral",
                table: "Declarações de Importação",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "Seq NF e Seq Prod NF",
                table: "Declarações de Importação",
                columns: new[] { "Seqüência da Nota Fiscal", "Seqüência Produto Nota Fiscal" },
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_Seqüência da Classificação",
                table: "Despesas",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_Seqüência da Unidade",
                table: "Despesas",
                column: "Seqüência da Unidade");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_Seqüência Grupo Despesa",
                table: "Despesas",
                column: "Seqüência Grupo Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa",
                table: "Despesas",
                columns: new[] { "SeqüênciaSubGrupoDespesa", "SeqüênciaGrupoDespesa" });

            migrationBuilder.CreateIndex(
                name: "IX_Despesas da Licitação_Sequencia da Despesa",
                table: "Despesas da Licitação",
                column: "Sequencia da Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas do Movimento Contábil_Seqüência da Despesa",
                table: "Despesas do Movimento Contábil",
                column: "Seqüência da Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas do Movimento Estoque_Seqüência da Despesa",
                table: "Despesas do Movimento Estoque",
                column: "Seqüência da Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas do Novo Pedido_Sequencia da Despesa",
                table: "Despesas do Novo Pedido",
                column: "Sequencia da Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas do Pedido Compra_Id da Despesa",
                table: "Despesas do Pedido Compra",
                column: "Id da Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas Mvto Contábil Novo_Seqüência da Despesa",
                table: "Despesas Mvto Contábil Novo",
                column: "Seqüência da Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada Contas_Seqüência do Geral",
                table: "Entrada Contas",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada Contas_Seqüência Grupo Despesa",
                table: "Entrada Contas",
                column: "Seqüência Grupo Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada Contas_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa",
                table: "Entrada Contas",
                columns: new[] { "SeqüênciaSubGrupoDespesa", "SeqüênciaGrupoDespesa" });

            migrationBuilder.CreateIndex(
                name: "IX_Geral_Seqüência do Município",
                table: "Geral",
                column: "Seqüência do Município");

            migrationBuilder.CreateIndex(
                name: "IX_Geral_Seqüência do País",
                table: "Geral",
                column: "Seqüência do País");

            migrationBuilder.CreateIndex(
                name: "IX_Geral_Seqüência do Vendedor",
                table: "Geral",
                column: "Seqüência do Vendedor");

            migrationBuilder.CreateIndex(
                name: "IX_Geral_Seqüência Município Cobrança",
                table: "Geral",
                column: "Seqüência Município Cobrança");

            migrationBuilder.CreateIndex(
                name: "UF do ICMS",
                table: "ICMS",
                column: "UF",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Importação Produtos Estoque_Seqüência do Produto",
                table: "Importação Produtos Estoque",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Itens da Licitacao_Produto",
                table: "Itens da Licitacao",
                column: "Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Itens do Conjunto_Seqüência do Produto",
                table: "Itens do Conjunto",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoBB_Conta",
                table: "LancamentoBancarioBB",
                columns: new[] { "SequenciaDaAgencia", "SequenciaDaCCDaAgencia" });

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoBB_DataLancamento",
                table: "LancamentoBancarioBB",
                column: "DataLancamento");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoBB_Historico",
                table: "LancamentoBancarioBB",
                column: "TextoDescricaoHistorico");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoBB_Processado",
                table: "LancamentoBancarioBB",
                column: "Processado");

            migrationBuilder.CreateIndex(
                name: "IX_Linha de Produção_Codigo do setor",
                table: "Linha de Produção",
                column: "Codigo do setor");

            migrationBuilder.CreateIndex(
                name: "IX_LogIntegracao_Categoria",
                table: "LogProcessamentoIntegracao",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_LogIntegracao_DataHora",
                table: "LogProcessamentoIntegracao",
                column: "DataHora");

            migrationBuilder.CreateIndex(
                name: "IX_LogIntegracao_Nivel",
                table: "LogProcessamentoIntegracao",
                column: "Nivel");

            migrationBuilder.CreateIndex(
                name: "IX_Manutenção Contas_Seqüência da Cobrança",
                table: "Manutenção Contas",
                column: "Seqüência da Cobrança");

            migrationBuilder.CreateIndex(
                name: "IX_Manutenção Contas_Seqüência da Nota Fiscal",
                table: "Manutenção Contas",
                column: "Seqüência da Nota Fiscal");

            migrationBuilder.CreateIndex(
                name: "IX_Manutenção Contas_Seqüência do Geral",
                table: "Manutenção Contas",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Manutenção Contas_Seqüência Grupo Despesa",
                table: "Manutenção Contas",
                column: "Seqüência Grupo Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Manutenção Contas_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa",
                table: "Manutenção Contas",
                columns: new[] { "SeqüênciaSubGrupoDespesa", "SeqüênciaGrupoDespesa" });

            migrationBuilder.CreateIndex(
                name: "IX_Matéria Prima_Seqüência do Produto",
                table: "Matéria Prima",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentação da Conta Corrente_Seqüência do Histórico",
                table: "Movimentação da Conta Corrente",
                column: "Seqüência do Histórico");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentação da Conta Corrente_SeqüênciaDaAgência_SeqüênciaDaCcDaAgência",
                table: "Movimentação da Conta Corrente",
                columns: new[] { "SeqüênciaDaAgência", "SeqüênciaDaCcDaAgência" });

            migrationBuilder.CreateIndex(
                name: "IX_Movimento do Estoque_Seqüência da Classificação",
                table: "Movimento do Estoque",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Movimento do Estoque_Seqüência da Propriedade",
                table: "Movimento do Estoque",
                column: "Seqüência da Propriedade");

            migrationBuilder.CreateIndex(
                name: "IX_Movimento do Estoque_Seqüência do Geral",
                table: "Movimento do Estoque",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Movimento do Estoque_Seqüência Grupo Despesa",
                table: "Movimento do Estoque",
                column: "Seqüência Grupo Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_Movimento do Estoque_SeqüênciaSubGrupoDespesa_SeqüênciaGrupoDespesa",
                table: "Movimento do Estoque",
                columns: new[] { "SeqüênciaSubGrupoDespesa", "SeqüênciaGrupoDespesa" });

            migrationBuilder.CreateIndex(
                name: "IX_Movimento do Estoque Contábil_Seqüência do Geral",
                table: "Movimento do Estoque Contábil",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Município da Transportadora",
                table: "Nota Fiscal",
                column: "Município da Transportadora");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência da Classificação",
                table: "Nota Fiscal",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência da Cobrança",
                table: "Nota Fiscal",
                column: "Seqüência da Cobrança");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência da Natureza",
                table: "Nota Fiscal",
                column: "Seqüência da Natureza");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência da Propriedade",
                table: "Nota Fiscal",
                column: "Seqüência da Propriedade");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência da Transportadora",
                table: "Nota Fiscal",
                column: "Seqüência da Transportadora");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência do Geral",
                table: "Nota Fiscal",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência do Movimento",
                table: "Nota Fiscal",
                column: "Seqüência do Movimento");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência do Pedido",
                table: "Nota Fiscal",
                column: "Seqüência do Pedido");

            migrationBuilder.CreateIndex(
                name: "IX_Nota Fiscal_Seqüência do Vendedor",
                table: "Nota Fiscal",
                column: "Seqüência do Vendedor");

            migrationBuilder.CreateIndex(
                name: "Seq NF Notas",
                table: "Notas Autorizadas",
                column: "Seqüência da Nota Fiscal",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Nova Licitação_Sequencia da Transportadora",
                table: "Nova Licitação",
                column: "Sequencia da Transportadora");

            migrationBuilder.CreateIndex(
                name: "IX_Nova Licitação_Sequencia do Fornecedor",
                table: "Nova Licitação",
                column: "Sequencia do Fornecedor");

            migrationBuilder.CreateIndex(
                name: "IX_Orçamento_Seqüência da Classificação",
                table: "Orçamento",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Orçamento_Seqüência da Propriedade",
                table: "Orçamento",
                column: "Seqüência da Propriedade");

            migrationBuilder.CreateIndex(
                name: "IX_Orçamento_Seqüência da Transportadora",
                table: "Orçamento",
                column: "Seqüência da Transportadora");

            migrationBuilder.CreateIndex(
                name: "IX_Orçamento_Seqüência do Geral",
                table: "Orçamento",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Orçamento_Seqüência do Município",
                table: "Orçamento",
                column: "Seqüência do Município");

            migrationBuilder.CreateIndex(
                name: "IX_Orçamento_Seqüência do País",
                table: "Orçamento",
                column: "Seqüência do País");

            migrationBuilder.CreateIndex(
                name: "IX_Orçamento_Seqüência do Vendedor",
                table: "Orçamento",
                column: "Seqüência do Vendedor");

            migrationBuilder.CreateIndex(
                name: "Número da Proforma",
                table: "Orçamento",
                column: "Número da Proforma")
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Ordem de Serviço_Seqüência da Classificação",
                table: "Ordem de Serviço",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Ordem de Serviço_Seqüência da Propriedade",
                table: "Ordem de Serviço",
                column: "Seqüência da Propriedade");

            migrationBuilder.CreateIndex(
                name: "IX_Ordem de Serviço_Seqüência do Geral",
                table: "Ordem de Serviço",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Ordem de Serviço_Seqüência do Município",
                table: "Ordem de Serviço",
                column: "Seqüência do Município");

            migrationBuilder.CreateIndex(
                name: "IX_Ordem de Serviço_Seqüência do Vendedor",
                table: "Ordem de Serviço",
                column: "Seqüência do Vendedor");

            migrationBuilder.CreateIndex(
                name: "IX_Parcelas Entrada Contas_Seqüência da Entrada",
                table: "Parcelas Entrada Contas",
                column: "Seqüência da Entrada");

            migrationBuilder.CreateIndex(
                name: "IX_Parcelas Movimento Estoque_Seqüência do Movimento",
                table: "Parcelas Movimento Estoque",
                column: "Seqüência do Movimento");

            migrationBuilder.CreateIndex(
                name: "IX_Peças da Nota Fiscal_Seqüência do Produto",
                table: "Peças da Nota Fiscal",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Peças da Ordem de Serviço_Seqüência do Produto",
                table: "Peças da Ordem de Serviço",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Peças do Movimento Estoque_Seqüência do Produto",
                table: "Peças do Movimento Estoque",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Peças do Orçamento_Seqüência do Produto",
                table: "Peças do Orçamento",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Peças do Pedido_Seqüência do Produto",
                table: "Peças do Pedido",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Seqüência da Classificação",
                table: "Pedido",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Seqüência da Ordem de Serviço",
                table: "Pedido",
                column: "Seqüência da Ordem de Serviço");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Seqüência da Propriedade",
                table: "Pedido",
                column: "Seqüência da Propriedade");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Seqüência da Transportadora",
                table: "Pedido",
                column: "Seqüência da Transportadora");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Seqüência do Geral",
                table: "Pedido",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Seqüência do Orçamento",
                table: "Pedido",
                column: "Seqüência do Orçamento");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Seqüência do Vendedor",
                table: "Pedido",
                column: "Seqüência do Vendedor");

            migrationBuilder.CreateIndex(
                name: "IX_Planilha de Adiantamento_Cod do Vendedor",
                table: "Planilha de Adiantamento",
                column: "Cod do Vendedor");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Seqüência da Classificação",
                table: "Produtos",
                column: "Seqüência da Classificação");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Seqüência da Unidade",
                table: "Produtos",
                column: "Seqüência da Unidade");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Seqüência do Grupo Produto",
                table: "Produtos",
                column: "Seqüência do Grupo Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_SeqüênciaDoSubGrupoProduto_SeqüênciaDoGrupoProduto",
                table: "Produtos",
                columns: new[] { "SeqüênciaDoSubGrupoProduto", "SeqüênciaDoGrupoProduto" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos da Licitação_Sequencia do Produto",
                table: "Produtos da Licitação",
                column: "Sequencia do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos da Nota Fiscal_Seqüência do Produto",
                table: "Produtos da Nota Fiscal",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos da Ordem de Serviço_Seqüência do Produto",
                table: "Produtos da Ordem de Serviço",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos do Movimento Contábil_Seqüência do Produto",
                table: "Produtos do Movimento Contábil",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos do Movimento Estoque_Seqüência do Produto",
                table: "Produtos do Movimento Estoque",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos do Novo Pedido_Sequencia do Produto",
                table: "Produtos do Novo Pedido",
                column: "Sequencia do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos do Orçamento_Seqüência do Produto",
                table: "Produtos do Orçamento",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos do Pedido_Seqüência do Produto",
                table: "Produtos do Pedido",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos do Pedido Compra_Id do Produto",
                table: "Produtos do Pedido Compra",
                column: "Id do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos Mvto Contábil Novo_Seqüência do Produto",
                table: "Produtos Mvto Contábil Novo",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Propriedades do Geral_Seqüência da Propriedade",
                table: "Propriedades do Geral",
                column: "Seqüência da Propriedade");

            migrationBuilder.CreateIndex(
                name: "Seq Geral e Seq Prop",
                table: "Propriedades do Geral",
                columns: new[] { "Seqüência do Geral", "Seqüência da Propriedade" },
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_PW~Tabelas_PW~Grupo",
                table: "PW~Tabelas",
                column: "PW~Grupo");

            migrationBuilder.CreateIndex(
                name: "PW~Grupo",
                table: "PW~Usuarios",
                column: "PW~Grupo");

            migrationBuilder.CreateIndex(
                name: "IX_Relatorio de Viagem_Sequencia do Geral",
                table: "Relatorio de Viagem",
                column: "Sequencia do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Requisição_Seqüência do Geral",
                table: "Requisição",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_Revendedores_Id da Conta",
                table: "Revendedores",
                column: "Id da Conta");

            migrationBuilder.CreateIndex(
                name: "Serie do Gerador",
                table: "Serie Gerador",
                column: "Serie do Gerador",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Serie do Hidroturbo",
                table: "Serie Hidroturbo",
                column: "Serie do Hidroturbo",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Serie da Moto Bomba",
                table: "Serie Moto Bomba",
                column: "Serie da Moto Bomba",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "Serie do Pivo",
                table: "Serie Pivos",
                column: "Serie do Pivo",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "SerieRebocador",
                table: "Serie Rebocador",
                column: "Serie do Rebocador",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Serviços da Nota Fiscal_Seqüência do Serviço",
                table: "Serviços da Nota Fiscal",
                column: "Seqüência do Serviço");

            migrationBuilder.CreateIndex(
                name: "IX_Serviços da Ordem de Serviço_Seqüência do Serviço",
                table: "Serviços da Ordem de Serviço",
                column: "Seqüência do Serviço");

            migrationBuilder.CreateIndex(
                name: "IX_Serviços do Orçamento_Seqüência do Serviço",
                table: "Serviços do Orçamento",
                column: "Seqüência do Serviço");

            migrationBuilder.CreateIndex(
                name: "IX_Serviços do Pedido_Seqüência do Serviço",
                table: "Serviços do Pedido",
                column: "Seqüência do Serviço");

            migrationBuilder.CreateIndex(
                name: "IX_SubGrupo Despesa_Seqüência Grupo Despesa",
                table: "SubGrupo Despesa",
                column: "Seqüência Grupo Despesa");

            migrationBuilder.CreateIndex(
                name: "IX_SubGrupo do Produto_Seqüência do Grupo Produto",
                table: "SubGrupo do Produto",
                column: "Seqüência do Grupo Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Dominio",
                table: "Tenants",
                column: "Dominio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transferência de Receita_Seqüência da Unidade",
                table: "Transferência de Receita",
                column: "Seqüência da Unidade");

            migrationBuilder.CreateIndex(
                name: "IX_Transferência de Receita_Seqüência do Grupo Produto",
                table: "Transferência de Receita",
                column: "Seqüência do Grupo Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Transferência de Receita_Seqüência do Produto",
                table: "Transferência de Receita",
                column: "Seqüência do Produto");

            migrationBuilder.CreateIndex(
                name: "IX_Transferência de Receita_SeqüênciaDoSubGrupoProduto_SeqüênciaDoGrupoProduto",
                table: "Transferência de Receita",
                columns: new[] { "SeqüênciaDoSubGrupoProduto", "SeqüênciaDoGrupoProduto" });

            migrationBuilder.CreateIndex(
                name: "IX_Valores Adicionais_Seqüência da Manutenção",
                table: "Valores Adicionais",
                column: "Seqüência da Manutenção");

            migrationBuilder.CreateIndex(
                name: "IX_VinculaPedidoOrcamento_Id do Pedido",
                table: "VinculaPedidoOrcamento",
                column: "Id do Pedido");

            migrationBuilder.CreateIndex(
                name: "IX_VinculaPedidoOrcamento_Seqüência do Geral",
                table: "VinculaPedidoOrcamento",
                column: "Seqüência do Geral");

            migrationBuilder.CreateIndex(
                name: "IX_VinculaPedidoOrcamento_Seqüência do Orçamento",
                table: "VinculaPedidoOrcamento",
                column: "Seqüência do Orçamento");

            migrationBuilder.CreateIndex(
                name: "IX_VinculaPedidoOrcamento_Seqüência do Produto",
                table: "VinculaPedidoOrcamento",
                column: "Seqüência do Produto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acoes");

            migrationBuilder.DropTable(
                name: "Adições da Declaração");

            migrationBuilder.DropTable(
                name: "Adutoras");

            migrationBuilder.DropTable(
                name: "Agencias");

            migrationBuilder.DropTable(
                name: "Agendamento de Backup");

            migrationBuilder.DropTable(
                name: "Alteracao Baixa Contas");

            migrationBuilder.DropTable(
                name: "Aspersor Final");

            migrationBuilder.DropTable(
                name: "Baixa Comissão Lote");

            migrationBuilder.DropTable(
                name: "Baixa Comissão Lote Contas");

            migrationBuilder.DropTable(
                name: "Baixa Contas");

            migrationBuilder.DropTable(
                name: "Baixa do Estoque Contábil");

            migrationBuilder.DropTable(
                name: "Baixa Industrialização");

            migrationBuilder.DropTable(
                name: "Baixa MP Conjunto");

            migrationBuilder.DropTable(
                name: "Baixa MP Produto");

            migrationBuilder.DropTable(
                name: "Bocal Aspersor Nelson");

            migrationBuilder.DropTable(
                name: "Bx Consumo Pedido Compra");

            migrationBuilder.DropTable(
                name: "Bx Despesas Pedido Compra");

            migrationBuilder.DropTable(
                name: "Bx Produtos Pedido Compra");

            migrationBuilder.DropTable(
                name: "Calendario");

            migrationBuilder.DropTable(
                name: "Cancelamento NFe");

            migrationBuilder.DropTable(
                name: "Carta de Correção NFe");

            migrationBuilder.DropTable(
                name: "Check list maquina");

            migrationBuilder.DropTable(
                name: "Cheques Cancelados");

            migrationBuilder.DropTable(
                name: "Clientes Processos");

            migrationBuilder.DropTable(
                name: "Cobrar Fornecedor");

            migrationBuilder.DropTable(
                name: "Comissao");

            migrationBuilder.DropTable(
                name: "Comissão do montador");

            migrationBuilder.DropTable(
                name: "Composição do Equipamento");

            migrationBuilder.DropTable(
                name: "Concilia Conta Antecipada");

            migrationBuilder.DropTable(
                name: "Conciliação de Cheques");

            migrationBuilder.DropTable(
                name: "ConfiguracaoIntegracao");

            migrationBuilder.DropTable(
                name: "Conjuntos da Nota Fiscal");

            migrationBuilder.DropTable(
                name: "Conjuntos da Ordem de Serviço");

            migrationBuilder.DropTable(
                name: "Conjuntos do Movimento Estoque");

            migrationBuilder.DropTable(
                name: "Conjuntos do Orçamento");

            migrationBuilder.DropTable(
                name: "Conjuntos do Pedido");

            migrationBuilder.DropTable(
                name: "Conjuntos do Projeto");

            migrationBuilder.DropTable(
                name: "Conjuntos Movimento Contábil");

            migrationBuilder.DropTable(
                name: "Conjuntos Mvto Contábil Novo");

            migrationBuilder.DropTable(
                name: "Consulta Notas Destinada");

            migrationBuilder.DropTable(
                name: "Consumo do Pedido Compra");

            migrationBuilder.DropTable(
                name: "Conta Contabil");

            migrationBuilder.DropTable(
                name: "Controle de Compras");

            migrationBuilder.DropTable(
                name: "Controle de Garantia");

            migrationBuilder.DropTable(
                name: "Controle de Pneus");

            migrationBuilder.DropTable(
                name: "Controle de Processos");

            migrationBuilder.DropTable(
                name: "Correcao Bloko K");

            migrationBuilder.DropTable(
                name: "Dados Adicionais");

            migrationBuilder.DropTable(
                name: "Despesas da Licitação");

            migrationBuilder.DropTable(
                name: "Despesas do Movimento Contábil");

            migrationBuilder.DropTable(
                name: "Despesas do Movimento Estoque");

            migrationBuilder.DropTable(
                name: "Despesas do Novo Pedido");

            migrationBuilder.DropTable(
                name: "Despesas do Pedido Compra");

            migrationBuilder.DropTable(
                name: "Despesas e vendas");

            migrationBuilder.DropTable(
                name: "Despesas Mvto Contábil Novo");

            migrationBuilder.DropTable(
                name: "Divirgencias NFe");

            migrationBuilder.DropTable(
                name: "Duplicatas Descontadas");

            migrationBuilder.DropTable(
                name: "Finalidade NFe");

            migrationBuilder.DropTable(
                name: "Follow Up Vendas");

            migrationBuilder.DropTable(
                name: "Hidroturbos Vendidos");

            migrationBuilder.DropTable(
                name: "Historico Contabil");

            migrationBuilder.DropTable(
                name: "ICMS");

            migrationBuilder.DropTable(
                name: "Importação");

            migrationBuilder.DropTable(
                name: "Importação Conjuntos Estoque");

            migrationBuilder.DropTable(
                name: "Importação Estoque");

            migrationBuilder.DropTable(
                name: "Importação Produtos Estoque");

            migrationBuilder.DropTable(
                name: "Inutilização NFe");

            migrationBuilder.DropTable(
                name: "Inventario Pdf");

            migrationBuilder.DropTable(
                name: "Itens da Correcao");

            migrationBuilder.DropTable(
                name: "Itens da Licitacao");

            migrationBuilder.DropTable(
                name: "Itens da Ordem");

            migrationBuilder.DropTable(
                name: "Itens da Produção");

            migrationBuilder.DropTable(
                name: "Itens da Requisição");

            migrationBuilder.DropTable(
                name: "Itens da Viagem");

            migrationBuilder.DropTable(
                name: "Itens do Conjunto");

            migrationBuilder.DropTable(
                name: "Itens pendentes");

            migrationBuilder.DropTable(
                name: "Itens Saidas Balcao");

            migrationBuilder.DropTable(
                name: "IVA From UFs");

            migrationBuilder.DropTable(
                name: "LancamentoBancarioBB");

            migrationBuilder.DropTable(
                name: "Lançamentos Contabil");

            migrationBuilder.DropTable(
                name: "Lances do Pivo");

            migrationBuilder.DropTable(
                name: "Licitacao");

            migrationBuilder.DropTable(
                name: "Linha de Produção");

            migrationBuilder.DropTable(
                name: "LogProcessamentoIntegracao");

            migrationBuilder.DropTable(
                name: "Manutenção Hidroturbo");

            migrationBuilder.DropTable(
                name: "Manutenção Pivo");

            migrationBuilder.DropTable(
                name: "Mapa da Vazao");

            migrationBuilder.DropTable(
                name: "Matéria Prima");

            migrationBuilder.DropTable(
                name: "Materia prima orçamento");

            migrationBuilder.DropTable(
                name: "Material Expedição");

            migrationBuilder.DropTable(
                name: "Motoristas");

            migrationBuilder.DropTable(
                name: "Movimento Contábil Novo");

            migrationBuilder.DropTable(
                name: "Municipios dos Revendedores");

            migrationBuilder.DropTable(
                name: "MVA");

            migrationBuilder.DropTable(
                name: "Mvto Conta do Vendedor");

            migrationBuilder.DropTable(
                name: "Notas Autorizadas");

            migrationBuilder.DropTable(
                name: "Nova Licitação");

            migrationBuilder.DropTable(
                name: "Ocorrencias Garantia");

            migrationBuilder.DropTable(
                name: "Orçamentos da compra");

            migrationBuilder.DropTable(
                name: "Ordem de Montagem");

            migrationBuilder.DropTable(
                name: "Parametros");

            migrationBuilder.DropTable(
                name: "Parâmetros da Contabilidade");

            migrationBuilder.DropTable(
                name: "Parâmetros da NFe");

            migrationBuilder.DropTable(
                name: "Parâmetros do Produto");

            migrationBuilder.DropTable(
                name: "Parametros do SPED ECF");

            migrationBuilder.DropTable(
                name: "Parcelas da Ordem");

            migrationBuilder.DropTable(
                name: "Parcelas da Viagem");

            migrationBuilder.DropTable(
                name: "Parcelas do Novo Pedido");

            migrationBuilder.DropTable(
                name: "Parcelas do Projeto");

            migrationBuilder.DropTable(
                name: "Parcelas Entrada Contas");

            migrationBuilder.DropTable(
                name: "Parcelas Movimento Estoque");

            migrationBuilder.DropTable(
                name: "Parcelas mvto contabil");

            migrationBuilder.DropTable(
                name: "Parcelas Nota Fiscal");

            migrationBuilder.DropTable(
                name: "Parcelas Orçamento");

            migrationBuilder.DropTable(
                name: "Parcelas Ordem de Serviço");

            migrationBuilder.DropTable(
                name: "Parcelas Ped Compra Novo");

            migrationBuilder.DropTable(
                name: "Parcelas Pedido");

            migrationBuilder.DropTable(
                name: "Peças da Nota Fiscal");

            migrationBuilder.DropTable(
                name: "Peças da Ordem de Serviço");

            migrationBuilder.DropTable(
                name: "Peças do Movimento Estoque");

            migrationBuilder.DropTable(
                name: "Peças do Orçamento");

            migrationBuilder.DropTable(
                name: "Peças do Pedido");

            migrationBuilder.DropTable(
                name: "Peças do Projeto");

            migrationBuilder.DropTable(
                name: "Pivos Vendidos");

            migrationBuilder.DropTable(
                name: "Planilha de Adiantamento");

            migrationBuilder.DropTable(
                name: "Pneus");

            migrationBuilder.DropTable(
                name: "Previsoes de Pagtos");

            migrationBuilder.DropTable(
                name: "Produtos da Licitação");

            migrationBuilder.DropTable(
                name: "Produtos da Nota Fiscal");

            migrationBuilder.DropTable(
                name: "Produtos da Ordem de Serviço");

            migrationBuilder.DropTable(
                name: "Produtos do Movimento Contábil");

            migrationBuilder.DropTable(
                name: "Produtos do Movimento Estoque");

            migrationBuilder.DropTable(
                name: "Produtos do Novo Pedido");

            migrationBuilder.DropTable(
                name: "Produtos do Orçamento");

            migrationBuilder.DropTable(
                name: "Produtos do Pedido");

            migrationBuilder.DropTable(
                name: "Produtos do Pedido Compra");

            migrationBuilder.DropTable(
                name: "Produtos Mvto Contábil Novo");

            migrationBuilder.DropTable(
                name: "Projeto de Irrigação");

            migrationBuilder.DropTable(
                name: "Propriedades do Geral");

            migrationBuilder.DropTable(
                name: "PW~Tabelas");

            migrationBuilder.DropTable(
                name: "PW~Usuarios");

            migrationBuilder.DropTable(
                name: "Razão Auxiliar");

            migrationBuilder.DropTable(
                name: "Receita primaria");

            migrationBuilder.DropTable(
                name: "Região dos Vendedores");

            migrationBuilder.DropTable(
                name: "Relatorio de Viagem");

            migrationBuilder.DropTable(
                name: "Resumo auxiliar");

            migrationBuilder.DropTable(
                name: "Revendedores");

            migrationBuilder.DropTable(
                name: "Saida de Balcao");

            migrationBuilder.DropTable(
                name: "Serie Gerador");

            migrationBuilder.DropTable(
                name: "Serie Hidroturbo");

            migrationBuilder.DropTable(
                name: "Serie Moto Bomba");

            migrationBuilder.DropTable(
                name: "Serie Pivos");

            migrationBuilder.DropTable(
                name: "Serie Rebocador");

            migrationBuilder.DropTable(
                name: "Serviços da Nota Fiscal");

            migrationBuilder.DropTable(
                name: "Serviços da Ordem");

            migrationBuilder.DropTable(
                name: "Serviços da Ordem de Serviço");

            migrationBuilder.DropTable(
                name: "Serviços do Orçamento");

            migrationBuilder.DropTable(
                name: "Serviços do Pedido");

            migrationBuilder.DropTable(
                name: "Serviços do Projeto");

            migrationBuilder.DropTable(
                name: "Simula estoque");

            migrationBuilder.DropTable(
                name: "Situação dos pedidos");

            migrationBuilder.DropTable(
                name: "Solicitantes");

            migrationBuilder.DropTable(
                name: "Spy Baixa Contas");

            migrationBuilder.DropTable(
                name: "SYS~Sequencial");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Tipo de Atividades");

            migrationBuilder.DropTable(
                name: "Tipo de Titulos");

            migrationBuilder.DropTable(
                name: "Transferência de Receita");

            migrationBuilder.DropTable(
                name: "Valores Adicionais");

            migrationBuilder.DropTable(
                name: "Vasilhames");

            migrationBuilder.DropTable(
                name: "Veiculos do Motorista");

            migrationBuilder.DropTable(
                name: "Vendedores Bloqueio");

            migrationBuilder.DropTable(
                name: "Via de Transporte DI");

            migrationBuilder.DropTable(
                name: "VinculaPedidoOrcamento");

            migrationBuilder.DropTable(
                name: "Declarações de Importação");

            migrationBuilder.DropTable(
                name: "Movimentação da Conta Corrente");

            migrationBuilder.DropTable(
                name: "Advogados");

            migrationBuilder.DropTable(
                name: "Status do Processo");

            migrationBuilder.DropTable(
                name: "Despesas");

            migrationBuilder.DropTable(
                name: "Requisição");

            migrationBuilder.DropTable(
                name: "Conjuntos");

            migrationBuilder.DropTable(
                name: "Setores");

            migrationBuilder.DropTable(
                name: "Entrada Contas");

            migrationBuilder.DropTable(
                name: "Movimento do Estoque Contábil");

            migrationBuilder.DropTable(
                name: "PW~Grupos");

            migrationBuilder.DropTable(
                name: "Conta do Vendedor");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Manutenção Contas");

            migrationBuilder.DropTable(
                name: "Pedido de Compra Novo");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Conta Corrente da Agência");

            migrationBuilder.DropTable(
                name: "Histórico da Conta Corrente");

            migrationBuilder.DropTable(
                name: "Nota Fiscal");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "SubGrupo do Produto");

            migrationBuilder.DropTable(
                name: "Tipo de Cobrança");

            migrationBuilder.DropTable(
                name: "Natureza de Operação");

            migrationBuilder.DropTable(
                name: "Movimento do Estoque");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Grupo do Produto");

            migrationBuilder.DropTable(
                name: "SubGrupo Despesa");

            migrationBuilder.DropTable(
                name: "Ordem de Serviço");

            migrationBuilder.DropTable(
                name: "Orçamento");

            migrationBuilder.DropTable(
                name: "Grupo da Despesa");

            migrationBuilder.DropTable(
                name: "Classificação Fiscal");

            migrationBuilder.DropTable(
                name: "Propriedades");

            migrationBuilder.DropTable(
                name: "Geral");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "Paises");
        }
    }
}
