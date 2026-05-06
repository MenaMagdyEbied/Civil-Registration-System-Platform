using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Civil_Registration_System_Platform.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "52783d8d-3189-457d-b5c7-d3b8d8c61d3b", new DateOnly(2026, 5, 6), "AQAAAAIAAYagAAAAELgAcoelx90HarYseWwEMyWGjtjE34C3LKidJa+B3X571GZcdXv4adfzxGrs4T6B5w==", "4a148f79-ac5f-42a4-88ec-2c5d6070d738" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2b372abf-5513-4802-a1b4-e4584e03d528", new DateOnly(2026, 5, 5), "AQAAAAIAAYagAAAAEKYQ2tzvM38LPlV1z+8MG+tK7/G1Ng3Gtk75CdE59B4Z4LEr0KcucraibliIe2DGIA==", "4b224923-3faf-4c86-b0c7-ca9010830528" });
        }
    }
}
