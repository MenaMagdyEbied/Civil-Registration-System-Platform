using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Civil_Registration_System_Platform.Migrations
{
    /// <inheritdoc />
    public partial class SeedServicesTypeHelperData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f13a3b84-e88b-4dd0-b75d-1964737a49bb", new DateOnly(2026, 5, 5), "AQAAAAIAAYagAAAAEOzJeOGCPuekLOZZuOQE66ZQXtsAvTepo4fe+p5b4XNDj0JYg5vYjriRK+jdor/2EA==", "efed3c4a-84aa-4616-9302-ba4898560034" });

            migrationBuilder.InsertData(
                table: "ServicesTypeHelpers",
                columns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum", "Details", "DurationInDays", "Price" },
                values: new object[,]
                {
                    { 1, 1, "الرقم القومي|اسم الأم الثلاثي", 0, 63 },
                    { 6, 1, "إشعار ولادة من المستشفى|بطاقة ولي الأمر", 0, 0 },
                    { 1, 2, "تقرير طبي|إشعار الوفاة من المستشفى", 1, 63 },
                    { 1, 3, "بيانات الزوج والزوجة (ثلاثي-رباعي)|تاريخ الزواج", 1, 63 },
                    { 1, 4, "بيانات الزوج والزوجة|تاريخ الطلاق", 1, 63 },
                    { 1, 5, "شهادة ميلاد مميكنة|عدد 2 صورة شخصية|قيد عائلي أو إثبات إقامة", 7, 75 },
                    { 2, 5, "البطاقة القديمة|عدد 2 صورة شخصية", 7, 75 },
                    { 3, 5, "محضر فقدان|عدد 2 صورة شخصية", 7, 315 },
                    { 4, 5, "البطاقة التالفة|عدد 2 صورة شخصية", 7, 265 },
                    { 1, 6, "بطاقة قومي سارية|عدد 4 صور شخصية|شهادة موقف التجنيد (للذكور)", 7, 500 },
                    { 2, 6, "الجواز القديم|بطاقة قومي سارية|عدد 4 صور شخصية", 7, 500 },
                    { 3, 6, "محضر فقدان|بطاقة قومي سارية|عدد 4 صور شخصية|شهادة موقف التجنيد (للذكور)", 7, 600 },
                    { 4, 6, "الجواز التالف|بطاقة قومي سارية", 7, 500 },
                    { 1, 8, "الرقم القومي|اسم رب الأسرة", 1, 63 },
                    { 1, 9, "الرقم القومي", 1, 63 },
                    { 1, 10, "بطاقة قومي + مؤهل دراسي|عدد 2 صورة شخصية|إثبات إقامة", 14, 1140 },
                    { 2, 10, "الرخصة القديمة|شهادة براءة ذمة", 14, 1105 },
                    { 3, 10, "بطاقة قومي|محضر فقدان|شهادة براءة ذمة", 14, 265 },
                    { 4, 10, "بطاقة قومي|الرخصة التالفة|شهادة براءة ذمة", 14, 215 }
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
                keyValues: new object[] { 6, 1 });

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
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 4, 5 });

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
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 4, 6 });

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
                keyValues: new object[] { 3, 10 });

            migrationBuilder.DeleteData(
                table: "ServicesTypeHelpers",
                keyColumns: new[] { "ApplicationTypeEnum", "ServicesTypeEnum" },
                keyValues: new object[] { 4, 10 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac5b3c18-8aec-40d6-901a-82e68068cd7f", new DateOnly(2026, 5, 4), "AQAAAAIAAYagAAAAEOsudK2uBaLtJnKBFD+PVJsFjdufs5YVvNjvIllk0XyFblW17Q49cpT6pgpY3npRCw==", "ced70ac6-1303-4238-b766-e20854888a73" });
        }
    }
}
