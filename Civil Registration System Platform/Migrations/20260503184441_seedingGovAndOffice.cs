using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Civil_Registration_System_Platform.Migrations
{
    /// <inheritdoc />
    public partial class seedingGovAndOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "82e467e0-ef99-4992-8f31-eed66e989b6d", "AQAAAAIAAYagAAAAEHiThdRleC1ur2z1XBFAFlCTHgAcwGFF6uSBSQwLnPOd73m6LOUnm57eiZVCoTzoFA==", "05c89636-b8bd-4615-9a24-21c50263febb" });

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "GovernorateId", "Code", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "C", true, "القاهرة" },
                    { 2, "GZ", true, "الجيزة" },
                    { 3, "ALX", true, "الإسكندرية" },
                    { 4, "DK", true, "الدقهلية" },
                    { 5, "BA", true, "البحر الأحمر" },
                    { 6, "BH", true, "البحيرة" },
                    { 7, "FYM", true, "الفيوم" },
                    { 8, "GH", true, "الغربية" },
                    { 9, "IS", true, "الإسماعيلية" },
                    { 10, "MN", true, "المنوفية" },
                    { 11, "MNIA", true, "المنيا" },
                    { 12, "KB", true, "القليوبية" },
                    { 13, "WAD", true, "الوادي الجديد" },
                    { 14, "SUZ", true, "السويس" },
                    { 15, "ASN", true, "اسوان" },
                    { 16, "AST", true, "اسيوط" },
                    { 17, "BNS", true, "بني سويف" },
                    { 18, "PTS", true, "بورسعيد" },
                    { 19, "DT", true, "دمياط" },
                    { 20, "SHR", true, "الشرقية" },
                    { 21, "JS", true, "جنوب سيناء" },
                    { 22, "KFS", true, "كفر الشيخ" },
                    { 23, "MT", true, "مطروح" },
                    { 24, "LUX", true, "الأقصر" },
                    { 25, "KN", true, "قنا" },
                    { 26, "NS", true, "شمال سيناء" },
                    { 27, "SHG", true, "سوهاج" }
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "OfficeId", "GovernorateId", "IsActive", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, 1, true, "مركز النزهة", "01000000001" },
                    { 2, 1, true, "مركز مدينة نصر", "01000000002" },
                    { 3, 1, true, "مركز عين شمس", "01000000003" },
                    { 4, 1, true, "مركز حلوان", "01000000004" },
                    { 5, 1, true, "مركز المعادي", "01000000005" },
                    { 6, 1, true, "مركز شبرا", "01000000006" },
                    { 7, 1, true, "مركز الزيتون", "01000000007" },
                    { 8, 1, true, "مركز التبين", "01000000008" },
                    { 9, 2, true, "مركز الجيزة", "01000000009" },
                    { 10, 2, true, "مركز الشيخ زايد", "01000000010" },
                    { 11, 2, true, "مركز أكتوبر", "01000000011" },
                    { 12, 2, true, "مركز الحوامدية", "01000000012" },
                    { 13, 2, true, "مركز البدرشين", "01000000013" },
                    { 14, 2, true, "مركز العياط", "01000000014" },
                    { 15, 2, true, "مركز الصف", "01000000015" },
                    { 16, 2, true, "مركز أطفيح", "01000000016" },
                    { 17, 3, true, "مركز المنتزه", "01000000017" },
                    { 18, 3, true, "مركز العجمي", "01000000018" },
                    { 19, 3, true, "مركز برج العرب", "01000000019" },
                    { 20, 3, true, "مركز الدخيلة", "01000000020" },
                    { 21, 3, true, "مركز سيدي بشر", "01000000021" },
                    { 22, 4, true, "مركز المنصورة", "01000000022" },
                    { 23, 4, true, "مركز طلخا", "01000000023" },
                    { 24, 4, true, "مركز دكرنس", "01000000024" },
                    { 25, 4, true, "مركز ميت غمر", "01000000025" },
                    { 26, 4, true, "مركز أجا", "01000000026" },
                    { 27, 4, true, "مركز المنزلة", "01000000027" },
                    { 28, 4, true, "مركز بلقاس", "01000000028" },
                    { 29, 4, true, "مركز شربين", "01000000029" },
                    { 30, 4, true, "مركز السنبلاوين", "01000000030" },
                    { 31, 4, true, "مركز تمي الأمديد", "01000000031" },
                    { 32, 5, true, "مركز الغردقة", "01000000032" },
                    { 33, 5, true, "مركز سفاجا", "01000000033" },
                    { 34, 5, true, "مركز القصير", "01000000034" },
                    { 35, 5, true, "مركز مرسى علم", "01000000035" },
                    { 36, 5, true, "مركز رأس غارب", "01000000036" },
                    { 37, 6, true, "مركز دمنهور", "01000000037" },
                    { 38, 6, true, "مركز كفر الدوار", "01000000038" },
                    { 39, 6, true, "مركز الرحمانية", "01000000039" },
                    { 40, 6, true, "مركز إيتاي البارود", "01000000040" },
                    { 41, 6, true, "مركز أبو حمص", "01000000041" },
                    { 42, 6, true, "مركز المحمودية", "01000000042" },
                    { 43, 6, true, "مركز رشيد", "01000000043" },
                    { 44, 6, true, "مركز شبراخيت", "01000000044" },
                    { 45, 6, true, "مركز وادي النطرون", "01000000045" },
                    { 46, 7, true, "مركز الفيوم", "01000000046" },
                    { 47, 7, true, "مركز سنورس", "01000000047" },
                    { 48, 7, true, "مركز إطسا", "01000000048" },
                    { 49, 7, true, "مركز طامية", "01000000049" },
                    { 50, 7, true, "مركز يوسف الصديق", "01000000050" },
                    { 51, 8, true, "مركز طنطا", "01000000051" },
                    { 52, 8, true, "مركز المحلة الكبرى", "01000000052" },
                    { 53, 8, true, "مركز زفتى", "01000000053" },
                    { 54, 8, true, "مركز السنطة", "01000000054" },
                    { 55, 8, true, "مركز قطور", "01000000055" },
                    { 56, 8, true, "مركز بسيون", "01000000056" },
                    { 57, 8, true, "مركز كفر الزيات", "01000000057" },
                    { 58, 9, true, "مركز الإسماعيلية", "01000000058" },
                    { 59, 9, true, "مركز فايد", "01000000059" },
                    { 60, 9, true, "مركز القنطرة شرق", "01000000060" },
                    { 61, 9, true, "مركز القنطرة غرب", "01000000061" },
                    { 62, 9, true, "مركز أبو صوير", "01000000062" },
                    { 63, 10, true, "مركز شبين الكوم", "01000000063" },
                    { 64, 10, true, "مركز منوف", "01000000064" },
                    { 65, 10, true, "مركز السادات", "01000000065" },
                    { 66, 10, true, "مركز تلا", "01000000066" },
                    { 67, 10, true, "مركز قويسنا", "01000000067" },
                    { 68, 10, true, "مركز بركة السبع", "01000000068" },
                    { 69, 10, true, "مركز أشمون", "01000000069" },
                    { 70, 10, true, "مركز الشهداء", "01000000070" },
                    { 71, 11, true, "مركز المنيا", "01000000071" },
                    { 72, 11, true, "مركز أبو قرقاص", "01000000072" },
                    { 73, 11, true, "مركز بني مزار", "01000000073" },
                    { 74, 11, true, "مركز مطاي", "01000000074" },
                    { 75, 11, true, "مركز ملوي", "01000000075" },
                    { 76, 11, true, "مركز سمالوط", "01000000076" },
                    { 77, 11, true, "مركز العدوة", "01000000077" },
                    { 78, 11, true, "مركز دير مواس", "01000000078" },
                    { 79, 12, true, "مركز بنها", "01000000079" },
                    { 80, 12, true, "مركز شبين القناطر", "01000000080" },
                    { 81, 12, true, "مركز قليوب", "01000000081" },
                    { 82, 12, true, "مركز القناطر الخيرية", "01000000082" },
                    { 83, 12, true, "مركز طوخ", "01000000083" },
                    { 84, 12, true, "مركز الخانكة", "01000000084" },
                    { 85, 12, true, "مركز كفر شكر", "01000000085" },
                    { 86, 13, true, "مركز الخارجة", "01000000086" },
                    { 87, 13, true, "مركز الداخلة", "01000000087" },
                    { 88, 13, true, "مركز الفرافرة", "01000000088" },
                    { 89, 13, true, "مركز بلاط", "01000000089" },
                    { 90, 14, true, "مركز السويس", "01000000090" },
                    { 91, 14, true, "مركز عتاقة", "01000000091" },
                    { 92, 14, true, "مركز فيصل", "01000000092" },
                    { 93, 15, true, "مركز أسوان", "01000000093" },
                    { 94, 15, true, "مركز إدفو", "01000000094" },
                    { 95, 15, true, "مركز كوم أمبو", "01000000095" },
                    { 96, 15, true, "مركز نصر النوبة", "01000000096" },
                    { 97, 15, true, "مركز دراو", "01000000097" },
                    { 98, 16, true, "مركز أسيوط", "01000000098" },
                    { 99, 16, true, "مركز أبنوب", "01000000099" },
                    { 100, 16, true, "مركز البداري", "01000000100" },
                    { 101, 16, true, "مركز ديروط", "01000000101" },
                    { 102, 16, true, "مركز القوصية", "01000000102" },
                    { 103, 16, true, "مركز منفلوط", "01000000103" },
                    { 104, 16, true, "مركز صدفا", "01000000104" },
                    { 105, 16, true, "مركز الفتح", "01000000105" },
                    { 106, 16, true, "مركز ساحل سليم", "01000000106" },
                    { 107, 17, true, "مركز بني سويف", "01000000107" },
                    { 108, 17, true, "مركز الفشن", "01000000108" },
                    { 109, 17, true, "مركز ناصر", "01000000109" },
                    { 110, 17, true, "مركز إهناسيا", "01000000110" },
                    { 111, 17, true, "مركز ببا", "01000000111" },
                    { 112, 17, true, "مركز سمسطا", "01000000112" },
                    { 113, 18, true, "مركز بورسعيد", "01000000113" },
                    { 114, 18, true, "مركز بورفؤاد", "01000000114" },
                    { 115, 18, true, "مركز الزهور", "01000000115" },
                    { 116, 19, true, "مركز دمياط", "01000000116" },
                    { 117, 19, true, "مركز فارسكور", "01000000117" },
                    { 118, 19, true, "مركز كفر سعد", "01000000118" },
                    { 119, 19, true, "مركز الزرقا", "01000000119" },
                    { 120, 20, true, "مركز الزقازيق", "01000000120" },
                    { 121, 20, true, "مركز بلبيس", "01000000121" },
                    { 122, 20, true, "مركز منيا القمح", "01000000122" },
                    { 123, 20, true, "مركز أبو حماد", "01000000123" },
                    { 124, 20, true, "مركز ديرب نجم", "01000000124" },
                    { 125, 20, true, "مركز الحسينية", "01000000125" },
                    { 126, 20, true, "مركز فاقوس", "01000000126" },
                    { 127, 20, true, "مركز القنايات", "01000000127" },
                    { 128, 20, true, "مركز كفر صقر", "01000000128" },
                    { 129, 20, true, "مركز ههيا", "01000000129" },
                    { 130, 20, true, "مركز العاشر من رمضان", "01000000130" },
                    { 131, 21, true, "مركز شرم الشيخ", "01000000131" },
                    { 132, 21, true, "مركز دهب", "01000000132" },
                    { 133, 21, true, "مركز طور سيناء", "01000000133" },
                    { 134, 21, true, "مركز أبو رديس", "01000000134" },
                    { 135, 21, true, "مركز نويبع", "01000000135" },
                    { 136, 22, true, "مركز كفر الشيخ", "01000000136" },
                    { 137, 22, true, "مركز دسوق", "01000000137" },
                    { 138, 22, true, "مركز فوة", "01000000138" },
                    { 139, 22, true, "مركز بيلا", "01000000139" },
                    { 140, 22, true, "مركز الرياض", "01000000140" },
                    { 141, 22, true, "مركز مطوبس", "01000000141" },
                    { 142, 22, true, "مركز سيدي سالم", "01000000142" },
                    { 143, 22, true, "مركز الحامول", "01000000143" },
                    { 144, 22, true, "مركز قلين", "01000000144" },
                    { 145, 23, true, "مركز مطروح", "01000000145" },
                    { 146, 23, true, "مركز سيدي براني", "01000000146" },
                    { 147, 23, true, "مركز الضبعة", "01000000147" },
                    { 148, 23, true, "مركز سلوم", "01000000148" },
                    { 149, 23, true, "مركز السلوم", "01000000149" },
                    { 150, 24, true, "مركز الأقصر", "01000000150" },
                    { 151, 24, true, "مركز إسنا", "01000000151" },
                    { 152, 24, true, "مركز أرمنت", "01000000152" },
                    { 153, 25, true, "مركز قنا", "01000000153" },
                    { 154, 25, true, "مركز نجع حمادي", "01000000154" },
                    { 155, 25, true, "مركز قوص", "01000000155" },
                    { 156, 25, true, "مركز دشنا", "01000000156" },
                    { 157, 25, true, "مركز أبو تشت", "01000000157" },
                    { 158, 25, true, "مركز فرشوط", "01000000158" },
                    { 159, 25, true, "مركز نقادة", "01000000159" },
                    { 160, 26, true, "مركز العريش", "01000000160" },
                    { 161, 26, true, "مركز رفح", "01000000161" },
                    { 162, 26, true, "مركز الشيخ زويد", "01000000162" },
                    { 163, 26, true, "مركز بئر العبد", "01000000163" },
                    { 164, 26, true, "مركز نخل", "01000000164" },
                    { 165, 27, true, "مركز سوهاج", "01000000165" },
                    { 166, 27, true, "مركز طهطا", "01000000166" },
                    { 167, 27, true, "مركز جرجا", "01000000167" },
                    { 168, 27, true, "مركز أخميم", "01000000168" },
                    { 169, 27, true, "مركز البلينا", "01000000169" },
                    { 170, 27, true, "مركز دار السلام", "01000000170" },
                    { 171, 27, true, "مركز المراغة", "01000000171" },
                    { 172, 27, true, "مركز المنشاة", "01000000172" },
                    { 173, 27, true, "مركز ساقلتة", "01000000173" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "OfficeId",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "GovernorateId",
                keyValue: 27);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da0121aa-eb52-4731-acd5-b190c4e8f96e", "AQAAAAIAAYagAAAAEMRHMYIapqMBOpA0t3mCK6GSu0oVblp874gBi4ij37t5/PNAs5dz9GUZ1q7ODG5JjA==", "bfd66b21-3f6c-49bd-9950-40c79279449b" });
        }
    }
}
