using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class CreateLogsAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsuarioCodigo = table.Column<int>(type: "int", nullable: false),
                    UsuarioNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsuarioGrupo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoAcao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Entidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntidadeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DadosAnteriores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DadosNovos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CamposAlterados = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EnderecoIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MetodoHttp = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UrlRequisicao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    TempoExecucaoMs = table.Column<long>(type: "bigint", nullable: true),
                    Erro = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MensagemErro = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SessaoId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_DataHora",
                table: "LogsAuditoria",
                column: "DataHora");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Entidade",
                table: "LogsAuditoria",
                columns: new[] { "Entidade", "EntidadeId" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Modulo",
                table: "LogsAuditoria",
                column: "Modulo");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Tenant",
                table: "LogsAuditoria",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_TipoAcao",
                table: "LogsAuditoria",
                column: "TipoAcao");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Usuario",
                table: "LogsAuditoria",
                column: "UsuarioCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsAuditoria");
        }
    }
}
