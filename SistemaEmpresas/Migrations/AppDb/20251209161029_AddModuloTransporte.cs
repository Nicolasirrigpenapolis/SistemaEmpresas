using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddModuloTransporte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reboques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Tara = table.Column<int>(type: "int", nullable: false),
                    CapacidadeKg = table.Column<int>(type: "int", nullable: true),
                    TipoRodado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoCarroceria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Rntrc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Renavam = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Chassi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reboques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AnoFabricacao = table.Column<int>(type: "int", nullable: true),
                    AnoModelo = table.Column<int>(type: "int", nullable: true),
                    Tara = table.Column<int>(type: "int", nullable: false),
                    CapacidadeKg = table.Column<int>(type: "int", nullable: true),
                    TipoRodado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoCarroceria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Renavam = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Chassi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Cor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TipoCombustivel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Rntrc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManutencoesVeiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeiculoId = table.Column<int>(type: "int", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: true),
                    DataManutencao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoManutencao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DescricaoServico = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    KmAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorMaoObra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorServicosTerceiros = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumeroOS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroNF = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataProximaManutencao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KmProximaManutencao = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManutencoesVeiculo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManutencoesVeiculo_Geral_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Geral",
                        principalColumn: "Seqüência do Geral");
                    table.ForeignKey(
                        name: "FK_ManutencoesVeiculo_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Viagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeiculoId = table.Column<int>(type: "int", nullable: false),
                    MotoristaId = table.Column<short>(type: "smallint", nullable: true),
                    ReboqueId = table.Column<int>(type: "int", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KmInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KmFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Origem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Destino = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DescricaoCarga = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PesoCarga = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viagens_Motoristas_MotoristaId",
                        column: x => x.MotoristaId,
                        principalTable: "Motoristas",
                        principalColumn: "Codigo do Motorista");
                    table.ForeignKey(
                        name: "FK_Viagens_Reboques_ReboqueId",
                        column: x => x.ReboqueId,
                        principalTable: "Reboques",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Viagens_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManutencoesPeca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManutencaoId = table.Column<int>(type: "int", nullable: false),
                    DescricaoPeca = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CodigoPeca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Unidade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManutencoesPeca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManutencoesPeca_ManutencoesVeiculo_ManutencaoId",
                        column: x => x.ManutencaoId,
                        principalTable: "ManutencoesVeiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DespesasViagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViagemId = table.Column<int>(type: "int", nullable: false),
                    TipoDespesa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataDespesa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Local = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    KmAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Litros = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DespesasViagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DespesasViagem_Viagens_ViagemId",
                        column: x => x.ViagemId,
                        principalTable: "Viagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceitasViagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViagemId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataReceita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Origem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cliente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceitasViagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceitasViagem_Viagens_ViagemId",
                        column: x => x.ViagemId,
                        principalTable: "Viagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DespesasViagem_DataDespesa",
                table: "DespesasViagem",
                column: "DataDespesa");

            migrationBuilder.CreateIndex(
                name: "IX_DespesasViagem_TipoDespesa",
                table: "DespesasViagem",
                column: "TipoDespesa");

            migrationBuilder.CreateIndex(
                name: "IX_DespesasViagem_ViagemId",
                table: "DespesasViagem",
                column: "ViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_ManutencoesPeca_ManutencaoId",
                table: "ManutencoesPeca",
                column: "ManutencaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ManutencoesVeiculo_DataManutencao",
                table: "ManutencoesVeiculo",
                column: "DataManutencao");

            migrationBuilder.CreateIndex(
                name: "IX_ManutencoesVeiculo_FornecedorId",
                table: "ManutencoesVeiculo",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ManutencoesVeiculo_VeiculoId",
                table: "ManutencoesVeiculo",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reboques_Placa",
                table: "Reboques",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceitasViagem_DataReceita",
                table: "ReceitasViagem",
                column: "DataReceita");

            migrationBuilder.CreateIndex(
                name: "IX_ReceitasViagem_ViagemId",
                table: "ReceitasViagem",
                column: "ViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Placa",
                table: "Veiculos",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_DataInicio",
                table: "Viagens",
                column: "DataInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_MotoristaId",
                table: "Viagens",
                column: "MotoristaId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_ReboqueId",
                table: "Viagens",
                column: "ReboqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_VeiculoId",
                table: "Viagens",
                column: "VeiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DespesasViagem");

            migrationBuilder.DropTable(
                name: "ManutencoesPeca");

            migrationBuilder.DropTable(
                name: "ReceitasViagem");

            migrationBuilder.DropTable(
                name: "ManutencoesVeiculo");

            migrationBuilder.DropTable(
                name: "Viagens");

            migrationBuilder.DropTable(
                name: "Reboques");

            migrationBuilder.DropTable(
                name: "Veiculos");
        }
    }
}
