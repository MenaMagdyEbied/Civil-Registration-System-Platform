using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Civil_Registration_System_Platform.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2b372abf-5513-4802-a1b4-e4584e03d528", new DateOnly(2026, 5, 5), "AQAAAAIAAYagAAAAEKYQ2tzvM38LPlV1z+8MG+tK7/G1Ng3Gtk75CdE59B4Z4LEr0KcucraibliIe2DGIA==", "4b224923-3faf-4c86-b0c7-ca9010830528" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c6ba6516-881a-4323-95a3-788e0addccbd", new DateOnly(2026, 5, 4), "AQAAAAIAAYagAAAAEHGKwyt9DqxdoeAI8Pmzilxjs/YXa5DD3ysPQVX6GsYe1Ny04iUCjhceQXmU+P8zIA==", "4140e0ad-2b08-4670-b048-1e1d505ac6fd" });
        }
    }
}
