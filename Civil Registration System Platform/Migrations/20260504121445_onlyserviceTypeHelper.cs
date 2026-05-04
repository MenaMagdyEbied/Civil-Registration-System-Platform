using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Civil_Registration_System_Platform.Migrations
{
    /// <inheritdoc />
    public partial class onlyserviceTypeHelper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
         name: "ApplicationTypeHelpers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicesTypeHelpers",
                table: "ServicesTypeHelpers");

            // حذف العمود اللي كان فيه Identity
            migrationBuilder.DropColumn(
                name: "ServicesTypeEnum",
                table: "ServicesTypeHelpers");

            // إضافته تاني بدون Identity
            migrationBuilder.AddColumn<int>(
                name: "ServicesTypeEnum",
                table: "ServicesTypeHelpers",
                type: "int",
                nullable: false);

            // العمود الجديد
            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeEnum",
                table: "ServicesTypeHelpers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Composite Primary Key
            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicesTypeHelpers",
                table: "ServicesTypeHelpers",
                columns: new[] { "ServicesTypeEnum", "ApplicationTypeEnum" });

            // سيبها زي ما هي
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[]
                {
            "ac5b3c18-8aec-40d6-901a-82e68068cd7f",
            new DateOnly(2026, 5, 4),
            "AQAAAAIAAYagAAAAEOsudK2uBaLtJnKBFD+PVJsFjdufs5YVvNjvIllk0XyFblW17Q49cpT6pgpY3npRCw==",
            "ced70ac6-1303-4238-b766-e20854888a73"
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
         name: "PK_ServicesTypeHelpers",
         table: "ServicesTypeHelpers");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeEnum",
                table: "ServicesTypeHelpers");

            migrationBuilder.DropColumn(
                name: "ServicesTypeEnum",
                table: "ServicesTypeHelpers");

            // رجوع العمود بـ Identity
            migrationBuilder.AddColumn<int>(
                name: "ServicesTypeEnum",
                table: "ServicesTypeHelpers",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicesTypeHelpers",
                table: "ServicesTypeHelpers",
                column: "ServicesTypeEnum");

            migrationBuilder.CreateTable(
                name: "ApplicationTypeHelpers",
                columns: table => new
                {
                    ApplicationTypeEnum = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Details = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypeHelpers", x => x.ApplicationTypeEnum);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[]
                {
            "82e467e0-ef99-4992-8f31-eed66e989b6d",
            new DateOnly(2026, 5, 3),
            "AQAAAAIAAYagAAAAEHiThdRleC1ur2z1XBFAFlCTHgAcwGFF6uSBSQwLnPOd73m6LOUnm57eiZVCoTzoFA==",
            "05c89636-b8bd-4615-9a24-21c50263febb"
                });
        }
    }
}
