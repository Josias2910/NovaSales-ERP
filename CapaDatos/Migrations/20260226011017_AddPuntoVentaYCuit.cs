using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaVentas.Migrations
{
    /// <inheritdoc />
    public partial class AddPuntoVentaYCuit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RUC",
                table: "Negocios",
                newName: "CUIT");

            migrationBuilder.AddColumn<int>(
                name: "PuntoVenta",
                table: "Negocios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PuntoVenta",
                table: "Negocios");

            migrationBuilder.RenameColumn(
                name: "CUIT",
                table: "Negocios",
                newName: "RUC");
        }
    }
}
