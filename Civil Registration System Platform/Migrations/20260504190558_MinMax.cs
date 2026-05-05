using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Civil_Registration_System_Platform.Migrations
{
    /// <inheritdoc />
    public partial class MinMax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "ServicesTypeHelpers",
                newName: "MinDays");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ServicesTypeHelpers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxDays",
                table: "ServicesTypeHelpers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b2a63b3-16cb-4909-96c4-977c9eb58ccc", "AQAAAAIAAYagAAAAELnXAbAxdYhVDAEDCVRopq6mt4grk9ICuGxfL4yqdLRMRhVPfVdMrpehFcOk5WLeZw==", "a5d411f1-6d87-483e-b4a6-7889fa9ceb28" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDays",
                table: "ServicesTypeHelpers");

            migrationBuilder.RenameColumn(
                name: "MinDays",
                table: "ServicesTypeHelpers",
                newName: "DurationInDays");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ServicesTypeHelpers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac5b3c18-8aec-40d6-901a-82e68068cd7f", "AQAAAAIAAYagAAAAEOsudK2uBaLtJnKBFD+PVJsFjdufs5YVvNjvIllk0XyFblW17Q49cpT6pgpY3npRCw==", "ced70ac6-1303-4238-b766-e20854888a73" });
        }
    }
}
