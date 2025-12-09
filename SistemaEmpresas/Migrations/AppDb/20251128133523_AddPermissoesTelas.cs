using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddPermissoesTelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona coluna PW~Ativo na tabela PW~Usuarios
            migrationBuilder.AddColumn<bool>(
                name: "PW~Ativo",
                table: "PW~Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: true);

            // Cria tabela PermissoesTela
            migrationBuilder.CreateTable(
                name: "PermissoesTela",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grupo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tela = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NomeTela = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Rota = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Consultar = table.Column<bool>(type: "bit", nullable: false),
                    Incluir = table.Column<bool>(type: "bit", nullable: false),
                    Alterar = table.Column<bool>(type: "bit", nullable: false),
                    Excluir = table.Column<bool>(type: "bit", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesTela", x => x.Id);
                });

            // Cria tabela PermissoesTemplate
            migrationBuilder.CreateTable(
                name: "PermissoesTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPadrao = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesTemplate", x => x.Id);
                });

            // Cria tabela PermissoesTemplateDetalhe
            migrationBuilder.CreateTable(
                name: "PermissoesTemplateDetalhe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tela = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Consultar = table.Column<bool>(type: "bit", nullable: false),
                    Incluir = table.Column<bool>(type: "bit", nullable: false),
                    Alterar = table.Column<bool>(type: "bit", nullable: false),
                    Excluir = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesTemplateDetalhe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissoesTemplateDetalhe_Template",
                        column: x => x.TemplateId,
                        principalTable: "PermissoesTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Cria índices
            migrationBuilder.CreateIndex(
                name: "IX_PermissoesTela_Grupo_Tela",
                table: "PermissoesTela",
                columns: new[] { "Grupo", "Tela" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesTemplate_Nome",
                table: "PermissoesTemplate",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesTemplateDetalhe_Template_Tela",
                table: "PermissoesTemplateDetalhe",
                columns: new[] { "TemplateId", "Tela" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissoesTemplateDetalhe");

            migrationBuilder.DropTable(
                name: "PermissoesTemplate");

            migrationBuilder.DropTable(
                name: "PermissoesTela");

            migrationBuilder.DropColumn(
                name: "PW~Ativo",
                table: "PW~Usuarios");
        }
    }
}
