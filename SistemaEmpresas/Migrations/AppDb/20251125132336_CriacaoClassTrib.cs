using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class CriacaoClassTrib : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cria a tabela ClassTrib para armazenar as classificações tributárias
            migrationBuilder.CreateTable(
                name: "ClassTrib",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoClassTrib = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false),
                    CodigoSituacaoTributaria = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    DescricaoSituacaoTributaria = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DescricaoClassTrib = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PercentualReducaoIBS = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    PercentualReducaoCBS = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    TipoAliquota = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ValidoParaNFe = table.Column<bool>(type: "bit", nullable: false),
                    TributacaoRegular = table.Column<bool>(type: "bit", nullable: false),
                    CreditoPresumidoOperacoes = table.Column<bool>(type: "bit", nullable: false),
                    EstornoCredito = table.Column<bool>(type: "bit", nullable: false),
                    AnexoLegislacao = table.Column<int>(type: "int", nullable: true),
                    LinkLegislacao = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    DataSincronizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTrib", x => x.Id);
                });

            // Adiciona a coluna ClassTribId na tabela Classificação Fiscal
            migrationBuilder.AddColumn<int>(
                name: "ClassTribId",
                table: "Classificação Fiscal",
                type: "int",
                nullable: true);

            // Cria índices
            migrationBuilder.CreateIndex(
                name: "IX_Classificação Fiscal_ClassTribId",
                table: "Classificação Fiscal",
                column: "ClassTribId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTrib_CodigoClassTrib",
                table: "ClassTrib",
                column: "CodigoClassTrib",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassTrib_CodigoSituacaoTributaria",
                table: "ClassTrib",
                column: "CodigoSituacaoTributaria");

            // Adiciona a FK entre Classificação Fiscal e ClassTrib
            migrationBuilder.AddForeignKey(
                name: "FK_Classificação Fiscal_ClassTrib_ClassTribId",
                table: "Classificação Fiscal",
                column: "ClassTribId",
                principalTable: "ClassTrib",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classificação Fiscal_ClassTrib_ClassTribId",
                table: "Classificação Fiscal");

            migrationBuilder.DropIndex(
                name: "IX_Classificação Fiscal_ClassTribId",
                table: "Classificação Fiscal");

            migrationBuilder.DropColumn(
                name: "ClassTribId",
                table: "Classificação Fiscal");

            migrationBuilder.DropTable(
                name: "ClassTrib");
        }
    }
}
