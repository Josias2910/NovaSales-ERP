using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaVentas.Migrations
{
    /// <inheritdoc />
    public partial class SeedPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permisos",
                columns: new[] { "Id", "FechaRegistro", "NombreMenu", "RolId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuUsuarios", 1 },
                    { 2, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuMantenedor", 1 },
                    { 3, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuVentas", 1 },
                    { 4, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuCompras", 1 },
                    { 5, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuClientes", 1 },
                    { 6, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuProveedores", 1 },
                    { 7, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuReportes", 1 },
                    { 8, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuAcercaDe", 1 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "FechaRegistro" },
                values: new object[] { 2, "Empleado", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Permisos",
                columns: new[] { "Id", "FechaRegistro", "NombreMenu", "RolId" },
                values: new object[,]
                {
                    { 9, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuVentas", 2 },
                    { 10, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuCompras", 2 },
                    { 11, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuClientes", 2 },
                    { 12, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuProveedores", 2 },
                    { 13, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "menuAcercaDe", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
