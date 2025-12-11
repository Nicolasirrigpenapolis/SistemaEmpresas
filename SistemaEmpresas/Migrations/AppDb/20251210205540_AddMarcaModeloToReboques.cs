using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddMarcaModeloToReboques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnoFabricacao",
                table: "Reboques",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Marca",
                table: "Reboques",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "Reboques",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnoFabricacao",
                table: "Reboques");

            migrationBuilder.DropColumn(
                name: "Marca",
                table: "Reboques");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "Reboques");
        }
    }
}
