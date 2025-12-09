using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations
{
    /// <inheritdoc />
    public partial class InitialTenants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            // Insere o tenant Irrigação Penápolis
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Ativo", "ConnectionString", "Dominio", "Nome" },
                values: new object[] { 1, true, "Server=DESKTOP-CHS14C0\\SQLIRRIGACAO;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;", "irrigacao", "Irrigação Penápolis" });

            // Insere o tenant Chinellato Transportes
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Ativo", "ConnectionString", "Dominio", "Nome" },
                values: new object[] { 2, true, "Server=DESKTOP-CHS14C0\\SQLIRRIGACAO;Database=ChinellatoTransportes;Trusted_Connection=True;TrustServerCertificate=True;", "chinellato", "Chinellato Transportes" });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Dominio",
                table: "Tenants",
                column: "Dominio",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
