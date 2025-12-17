using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpresas.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddEmailToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Verifica se a coluna já existe antes de adicionar
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'PW~Usuarios' 
                    AND COLUMN_NAME = 'Email'
                )
                BEGIN
                    ALTER TABLE [PW~Usuarios] ADD [Email] varchar(255) NULL
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "PW~Usuarios");
        }
    }
}
