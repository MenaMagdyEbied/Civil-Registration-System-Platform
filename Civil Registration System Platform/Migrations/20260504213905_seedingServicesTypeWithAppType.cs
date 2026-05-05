using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Civil_Registration_System_Platform.Migrations
{
    /// <inheritdoc />
    public partial class seedingServicesTypeWithAppType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c6ba6516-881a-4323-95a3-788e0addccbd", "AQAAAAIAAYagAAAAEHGKwyt9DqxdoeAI8Pmzilxjs/YXa5DD3ysPQVX6GsYe1Ny04iUCjhceQXmU+P8zIA==", "4140e0ad-2b08-4670-b048-1e1d505ac6fd" });

            migrationBuilder.InsertData(
                table: "ServicesTypeHelpers",
                columns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum", "Details", "MaxDays", "MinDays", "Price" },
                values: new object[,]
                {
                    { 1, 1, null, 0, 0, 63 },
                    { 1, 2, null, 3, 1, 63 },
                    { 1, 3, null, 3, 1, 63 },
                    { 1, 4, null, 3, 1, 63 },
                    { 1, 5, null, 7, 3, 75 },
                    { 2, 5, null, 7, 3, 75 },
                    { 4, 5, null, 7, 3, 315 },
                    { 8, 5, null, 7, 3, 265 },
                    { 1, 6, null, 7, 3, 500 },
                    { 2, 6, null, 7, 3, 500 },
                    { 4, 6, null, 7, 3, 600 },
                    { 8, 6, null, 7, 3, 500 },
                    { 1, 8, null, 3, 1, 63 },
                    { 1, 9, null, 3, 1, 63 },
                    { 1, 10, null, 14, 7, 1140 },
                    { 2, 10, null, 14, 7, 1015 },
                    { 4, 10, null, 14, 7, 265 },
                    { 8, 10, null, 14, 7, 215 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 8, 5 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 8, 6 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 8 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 1, 10 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 2, 10 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 4, 10 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 8, 10 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b2a63b3-16cb-4909-96c4-977c9eb58ccc", "AQAAAAIAAYagAAAAELnXAbAxdYhVDAEDCVRopq6mt4grk9ICuGxfL4yqdLRMRhVPfVdMrpehFcOk5WLeZw==", "a5d411f1-6d87-483e-b4a6-7889fa9ceb28" });
        }
    }
}
