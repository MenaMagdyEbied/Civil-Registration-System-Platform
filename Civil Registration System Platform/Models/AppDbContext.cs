
using Civil_Registration_System_Platform.Relation_Constraint_Configration;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Civil_Registration_System_Platform.Models
{
    public class AppDbContext : IdentityDbContext<UserAccount>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration can be added here if needed
            modelBuilder.ConfigureAll();



            #region seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "1",
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "3",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            },
            new IdentityRole
            {
                Id = "4",
                Name = "User",
                NormalizedName = "USER"
            }
            );

            #endregion

            #region seed superAdmin Account
            var hasher = new PasswordHasher<UserAccount>();
            var admin = new UserAccount
            {
                Id = "1",
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                EmailConfirmed = true,

                FullName = "Super Admin",
                EGPhoneNumber = "01000000000",
                NationalID = "12345678901234",
                Gender = 1,
                MaritalStatus = 1,
                IsConfirmed = true,
                CardImagePath = "default.png",
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),

                IsRejected = false,

                SecurityStamp = Guid.NewGuid().ToString()
            };

            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

            modelBuilder.Entity<UserAccount>().HasData(admin);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = "1",
                RoleId = "1"
            });
            #endregion

            #region seed governorates
            modelBuilder.Entity<Governorate>().HasData(
            new Governorate { GovernorateId = 1, Name = "القاهرة", Code = "C", IsActive = true },
            new Governorate { GovernorateId = 2, Name = "الجيزة", Code = "GZ", IsActive = true },
            new Governorate { GovernorateId = 3, Name = "الإسكندرية", Code = "ALX", IsActive = true },
            new Governorate { GovernorateId = 4, Name = "الدقهلية", Code = "DK", IsActive = true },
            new Governorate { GovernorateId = 5, Name = "البحر الأحمر", Code = "BA", IsActive = true },
            new Governorate { GovernorateId = 6, Name = "البحيرة", Code = "BH", IsActive = true },
            new Governorate { GovernorateId = 7, Name = "الفيوم", Code = "FYM", IsActive = true },
            new Governorate { GovernorateId = 8, Name = "الغربية", Code = "GH", IsActive = true },
            new Governorate { GovernorateId = 9, Name = "الإسماعيلية", Code = "IS", IsActive = true },
            new Governorate { GovernorateId = 10, Name = "المنوفية", Code = "MN", IsActive = true },
            new Governorate { GovernorateId = 11, Name = "المنيا", Code = "MNIA", IsActive = true },
            new Governorate { GovernorateId = 12, Name = "القليوبية", Code = "KB", IsActive = true },
            new Governorate { GovernorateId = 13, Name = "الوادي الجديد", Code = "WAD", IsActive = true },
            new Governorate { GovernorateId = 14, Name = "السويس", Code = "SUZ", IsActive = true },
            new Governorate { GovernorateId = 15, Name = "اسوان", Code = "ASN", IsActive = true },
            new Governorate { GovernorateId = 16, Name = "اسيوط", Code = "AST", IsActive = true },
            new Governorate { GovernorateId = 17, Name = "بني سويف", Code = "BNS", IsActive = true },
            new Governorate { GovernorateId = 18, Name = "بورسعيد", Code = "PTS", IsActive = true },
            new Governorate { GovernorateId = 19, Name = "دمياط", Code = "DT", IsActive = true },
            new Governorate { GovernorateId = 20, Name = "الشرقية", Code = "SHR", IsActive = true },
            new Governorate { GovernorateId = 21, Name = "جنوب سيناء", Code = "JS", IsActive = true },
            new Governorate { GovernorateId = 22, Name = "كفر الشيخ", Code = "KFS", IsActive = true },
            new Governorate { GovernorateId = 23, Name = "مطروح", Code = "MT", IsActive = true },
            new Governorate { GovernorateId = 24, Name = "الأقصر", Code = "LUX", IsActive = true },
            new Governorate { GovernorateId = 25, Name = "قنا", Code = "KN", IsActive = true },
            new Governorate { GovernorateId = 26, Name = "شمال سيناء", Code = "NS", IsActive = true },
            new Governorate { GovernorateId = 27, Name = "سوهاج", Code = "SHG", IsActive = true }
            );

            #endregion

            #region seed offices
            modelBuilder.Entity<Office>().HasData(

    // 1 - القاهرة
    new Office { OfficeId = 1, Name = "مركز النزهة", Phone = "01000000001", IsActive = true, GovernorateId = 1 },
    new Office { OfficeId = 2, Name = "مركز مدينة نصر", Phone = "01000000002", IsActive = true, GovernorateId = 1 },
    new Office { OfficeId = 3, Name = "مركز عين شمس", Phone = "01000000003", IsActive = true, GovernorateId = 1 },
    new Office { OfficeId = 4, Name = "مركز حلوان", Phone = "01000000004", IsActive = true, GovernorateId = 1 },
    new Office { OfficeId = 5, Name = "مركز المعادي", Phone = "01000000005", IsActive = true, GovernorateId = 1 },
    new Office { OfficeId = 6, Name = "مركز شبرا", Phone = "01000000006", IsActive = true, GovernorateId = 1 },
    new Office { OfficeId = 7, Name = "مركز الزيتون", Phone = "01000000007", IsActive = true, GovernorateId = 1 },
    new Office { OfficeId = 8, Name = "مركز التبين", Phone = "01000000008", IsActive = true, GovernorateId = 1 },

    // 2 - الجيزة
    new Office { OfficeId = 9, Name = "مركز الجيزة", Phone = "01000000009", IsActive = true, GovernorateId = 2 },
    new Office { OfficeId = 10, Name = "مركز الشيخ زايد", Phone = "01000000010", IsActive = true, GovernorateId = 2 },
    new Office { OfficeId = 11, Name = "مركز أكتوبر", Phone = "01000000011", IsActive = true, GovernorateId = 2 },
    new Office { OfficeId = 12, Name = "مركز الحوامدية", Phone = "01000000012", IsActive = true, GovernorateId = 2 },
    new Office { OfficeId = 13, Name = "مركز البدرشين", Phone = "01000000013", IsActive = true, GovernorateId = 2 },
    new Office { OfficeId = 14, Name = "مركز العياط", Phone = "01000000014", IsActive = true, GovernorateId = 2 },
    new Office { OfficeId = 15, Name = "مركز الصف", Phone = "01000000015", IsActive = true, GovernorateId = 2 },
    new Office { OfficeId = 16, Name = "مركز أطفيح", Phone = "01000000016", IsActive = true, GovernorateId = 2 },

    // 3 - الإسكندرية
    new Office { OfficeId = 17, Name = "مركز المنتزه", Phone = "01000000017", IsActive = true, GovernorateId = 3 },
    new Office { OfficeId = 18, Name = "مركز العجمي", Phone = "01000000018", IsActive = true, GovernorateId = 3 },
    new Office { OfficeId = 19, Name = "مركز برج العرب", Phone = "01000000019", IsActive = true, GovernorateId = 3 },
    new Office { OfficeId = 20, Name = "مركز الدخيلة", Phone = "01000000020", IsActive = true, GovernorateId = 3 },
    new Office { OfficeId = 21, Name = "مركز سيدي بشر", Phone = "01000000021", IsActive = true, GovernorateId = 3 },

    // 4 - الدقهلية
    new Office { OfficeId = 22, Name = "مركز المنصورة", Phone = "01000000022", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 23, Name = "مركز طلخا", Phone = "01000000023", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 24, Name = "مركز دكرنس", Phone = "01000000024", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 25, Name = "مركز ميت غمر", Phone = "01000000025", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 26, Name = "مركز أجا", Phone = "01000000026", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 27, Name = "مركز المنزلة", Phone = "01000000027", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 28, Name = "مركز بلقاس", Phone = "01000000028", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 29, Name = "مركز شربين", Phone = "01000000029", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 30, Name = "مركز السنبلاوين", Phone = "01000000030", IsActive = true, GovernorateId = 4 },
    new Office { OfficeId = 31, Name = "مركز تمي الأمديد", Phone = "01000000031", IsActive = true, GovernorateId = 4 },

    // 5 - البحر الأحمر
    new Office { OfficeId = 32, Name = "مركز الغردقة", Phone = "01000000032", IsActive = true, GovernorateId = 5 },
    new Office { OfficeId = 33, Name = "مركز سفاجا", Phone = "01000000033", IsActive = true, GovernorateId = 5 },
    new Office { OfficeId = 34, Name = "مركز القصير", Phone = "01000000034", IsActive = true, GovernorateId = 5 },
    new Office { OfficeId = 35, Name = "مركز مرسى علم", Phone = "01000000035", IsActive = true, GovernorateId = 5 },
    new Office { OfficeId = 36, Name = "مركز رأس غارب", Phone = "01000000036", IsActive = true, GovernorateId = 5 },

    // 6 - البحيرة
    new Office { OfficeId = 37, Name = "مركز دمنهور", Phone = "01000000037", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 38, Name = "مركز كفر الدوار", Phone = "01000000038", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 39, Name = "مركز الرحمانية", Phone = "01000000039", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 40, Name = "مركز إيتاي البارود", Phone = "01000000040", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 41, Name = "مركز أبو حمص", Phone = "01000000041", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 42, Name = "مركز المحمودية", Phone = "01000000042", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 43, Name = "مركز رشيد", Phone = "01000000043", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 44, Name = "مركز شبراخيت", Phone = "01000000044", IsActive = true, GovernorateId = 6 },
    new Office { OfficeId = 45, Name = "مركز وادي النطرون", Phone = "01000000045", IsActive = true, GovernorateId = 6 },

    // 7 - الفيوم
    new Office { OfficeId = 46, Name = "مركز الفيوم", Phone = "01000000046", IsActive = true, GovernorateId = 7 },
    new Office { OfficeId = 47, Name = "مركز سنورس", Phone = "01000000047", IsActive = true, GovernorateId = 7 },
    new Office { OfficeId = 48, Name = "مركز إطسا", Phone = "01000000048", IsActive = true, GovernorateId = 7 },
    new Office { OfficeId = 49, Name = "مركز طامية", Phone = "01000000049", IsActive = true, GovernorateId = 7 },
    new Office { OfficeId = 50, Name = "مركز يوسف الصديق", Phone = "01000000050", IsActive = true, GovernorateId = 7 },

    // 8 - الغربية
    new Office { OfficeId = 51, Name = "مركز طنطا", Phone = "01000000051", IsActive = true, GovernorateId = 8 },
    new Office { OfficeId = 52, Name = "مركز المحلة الكبرى", Phone = "01000000052", IsActive = true, GovernorateId = 8 },
    new Office { OfficeId = 53, Name = "مركز زفتى", Phone = "01000000053", IsActive = true, GovernorateId = 8 },
    new Office { OfficeId = 54, Name = "مركز السنطة", Phone = "01000000054", IsActive = true, GovernorateId = 8 },
    new Office { OfficeId = 55, Name = "مركز قطور", Phone = "01000000055", IsActive = true, GovernorateId = 8 },
    new Office { OfficeId = 56, Name = "مركز بسيون", Phone = "01000000056", IsActive = true, GovernorateId = 8 },
    new Office { OfficeId = 57, Name = "مركز كفر الزيات", Phone = "01000000057", IsActive = true, GovernorateId = 8 },

    // 9 - الإسماعيلية
    new Office { OfficeId = 58, Name = "مركز الإسماعيلية", Phone = "01000000058", IsActive = true, GovernorateId = 9 },
    new Office { OfficeId = 59, Name = "مركز فايد", Phone = "01000000059", IsActive = true, GovernorateId = 9 },
    new Office { OfficeId = 60, Name = "مركز القنطرة شرق", Phone = "01000000060", IsActive = true, GovernorateId = 9 },
    new Office { OfficeId = 61, Name = "مركز القنطرة غرب", Phone = "01000000061", IsActive = true, GovernorateId = 9 },
    new Office { OfficeId = 62, Name = "مركز أبو صوير", Phone = "01000000062", IsActive = true, GovernorateId = 9 },

    // 10 - المنوفية
    new Office { OfficeId = 63, Name = "مركز شبين الكوم", Phone = "01000000063", IsActive = true, GovernorateId = 10 },
    new Office { OfficeId = 64, Name = "مركز منوف", Phone = "01000000064", IsActive = true, GovernorateId = 10 },
    new Office { OfficeId = 65, Name = "مركز السادات", Phone = "01000000065", IsActive = true, GovernorateId = 10 },
    new Office { OfficeId = 66, Name = "مركز تلا", Phone = "01000000066", IsActive = true, GovernorateId = 10 },
    new Office { OfficeId = 67, Name = "مركز قويسنا", Phone = "01000000067", IsActive = true, GovernorateId = 10 },
    new Office { OfficeId = 68, Name = "مركز بركة السبع", Phone = "01000000068", IsActive = true, GovernorateId = 10 },
    new Office { OfficeId = 69, Name = "مركز أشمون", Phone = "01000000069", IsActive = true, GovernorateId = 10 },
    new Office { OfficeId = 70, Name = "مركز الشهداء", Phone = "01000000070", IsActive = true, GovernorateId = 10 },

    // 11 - المنيا
    new Office { OfficeId = 71, Name = "مركز المنيا", Phone = "01000000071", IsActive = true, GovernorateId = 11 },
    new Office { OfficeId = 72, Name = "مركز أبو قرقاص", Phone = "01000000072", IsActive = true, GovernorateId = 11 },
    new Office { OfficeId = 73, Name = "مركز بني مزار", Phone = "01000000073", IsActive = true, GovernorateId = 11 },
    new Office { OfficeId = 74, Name = "مركز مطاي", Phone = "01000000074", IsActive = true, GovernorateId = 11 },
    new Office { OfficeId = 75, Name = "مركز ملوي", Phone = "01000000075", IsActive = true, GovernorateId = 11 },
    new Office { OfficeId = 76, Name = "مركز سمالوط", Phone = "01000000076", IsActive = true, GovernorateId = 11 },
    new Office { OfficeId = 77, Name = "مركز العدوة", Phone = "01000000077", IsActive = true, GovernorateId = 11 },
    new Office { OfficeId = 78, Name = "مركز دير مواس", Phone = "01000000078", IsActive = true, GovernorateId = 11 },

    // 12 - القليوبية
    new Office { OfficeId = 79, Name = "مركز بنها", Phone = "01000000079", IsActive = true, GovernorateId = 12 },
    new Office { OfficeId = 80, Name = "مركز شبين القناطر", Phone = "01000000080", IsActive = true, GovernorateId = 12 },
    new Office { OfficeId = 81, Name = "مركز قليوب", Phone = "01000000081", IsActive = true, GovernorateId = 12 },
    new Office { OfficeId = 82, Name = "مركز القناطر الخيرية", Phone = "01000000082", IsActive = true, GovernorateId = 12 },
    new Office { OfficeId = 83, Name = "مركز طوخ", Phone = "01000000083", IsActive = true, GovernorateId = 12 },
    new Office { OfficeId = 84, Name = "مركز الخانكة", Phone = "01000000084", IsActive = true, GovernorateId = 12 },
    new Office { OfficeId = 85, Name = "مركز كفر شكر", Phone = "01000000085", IsActive = true, GovernorateId = 12 },

    // 13 - الوادي الجديد
    new Office { OfficeId = 86, Name = "مركز الخارجة", Phone = "01000000086", IsActive = true, GovernorateId = 13 },
    new Office { OfficeId = 87, Name = "مركز الداخلة", Phone = "01000000087", IsActive = true, GovernorateId = 13 },
    new Office { OfficeId = 88, Name = "مركز الفرافرة", Phone = "01000000088", IsActive = true, GovernorateId = 13 },
    new Office { OfficeId = 89, Name = "مركز بلاط", Phone = "01000000089", IsActive = true, GovernorateId = 13 },

    // 14 - السويس
    new Office { OfficeId = 90, Name = "مركز السويس", Phone = "01000000090", IsActive = true, GovernorateId = 14 },
    new Office { OfficeId = 91, Name = "مركز عتاقة", Phone = "01000000091", IsActive = true, GovernorateId = 14 },
    new Office { OfficeId = 92, Name = "مركز فيصل", Phone = "01000000092", IsActive = true, GovernorateId = 14 },

    // 15 - أسوان
    new Office { OfficeId = 93, Name = "مركز أسوان", Phone = "01000000093", IsActive = true, GovernorateId = 15 },
    new Office { OfficeId = 94, Name = "مركز إدفو", Phone = "01000000094", IsActive = true, GovernorateId = 15 },
    new Office { OfficeId = 95, Name = "مركز كوم أمبو", Phone = "01000000095", IsActive = true, GovernorateId = 15 },
    new Office { OfficeId = 96, Name = "مركز نصر النوبة", Phone = "01000000096", IsActive = true, GovernorateId = 15 },
    new Office { OfficeId = 97, Name = "مركز دراو", Phone = "01000000097", IsActive = true, GovernorateId = 15 },

    // 16 - أسيوط
    new Office { OfficeId = 98, Name = "مركز أسيوط", Phone = "01000000098", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 99, Name = "مركز أبنوب", Phone = "01000000099", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 100, Name = "مركز البداري", Phone = "01000000100", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 101, Name = "مركز ديروط", Phone = "01000000101", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 102, Name = "مركز القوصية", Phone = "01000000102", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 103, Name = "مركز منفلوط", Phone = "01000000103", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 104, Name = "مركز صدفا", Phone = "01000000104", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 105, Name = "مركز الفتح", Phone = "01000000105", IsActive = true, GovernorateId = 16 },
    new Office { OfficeId = 106, Name = "مركز ساحل سليم", Phone = "01000000106", IsActive = true, GovernorateId = 16 },

    // 17 - بني سويف
    new Office { OfficeId = 107, Name = "مركز بني سويف", Phone = "01000000107", IsActive = true, GovernorateId = 17 },
    new Office { OfficeId = 108, Name = "مركز الفشن", Phone = "01000000108", IsActive = true, GovernorateId = 17 },
    new Office { OfficeId = 109, Name = "مركز ناصر", Phone = "01000000109", IsActive = true, GovernorateId = 17 },
    new Office { OfficeId = 110, Name = "مركز إهناسيا", Phone = "01000000110", IsActive = true, GovernorateId = 17 },
    new Office { OfficeId = 111, Name = "مركز ببا", Phone = "01000000111", IsActive = true, GovernorateId = 17 },
    new Office { OfficeId = 112, Name = "مركز سمسطا", Phone = "01000000112", IsActive = true, GovernorateId = 17 },

    // 18 - بورسعيد
    new Office { OfficeId = 113, Name = "مركز بورسعيد", Phone = "01000000113", IsActive = true, GovernorateId = 18 },
    new Office { OfficeId = 114, Name = "مركز بورفؤاد", Phone = "01000000114", IsActive = true, GovernorateId = 18 },
    new Office { OfficeId = 115, Name = "مركز الزهور", Phone = "01000000115", IsActive = true, GovernorateId = 18 },

    // 19 - دمياط
    new Office { OfficeId = 116, Name = "مركز دمياط", Phone = "01000000116", IsActive = true, GovernorateId = 19 },
    new Office { OfficeId = 117, Name = "مركز فارسكور", Phone = "01000000117", IsActive = true, GovernorateId = 19 },
    new Office { OfficeId = 118, Name = "مركز كفر سعد", Phone = "01000000118", IsActive = true, GovernorateId = 19 },
    new Office { OfficeId = 119, Name = "مركز الزرقا", Phone = "01000000119", IsActive = true, GovernorateId = 19 },

    // 20 - الشرقية
    new Office { OfficeId = 120, Name = "مركز الزقازيق", Phone = "01000000120", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 121, Name = "مركز بلبيس", Phone = "01000000121", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 122, Name = "مركز منيا القمح", Phone = "01000000122", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 123, Name = "مركز أبو حماد", Phone = "01000000123", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 124, Name = "مركز ديرب نجم", Phone = "01000000124", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 125, Name = "مركز الحسينية", Phone = "01000000125", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 126, Name = "مركز فاقوس", Phone = "01000000126", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 127, Name = "مركز القنايات", Phone = "01000000127", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 128, Name = "مركز كفر صقر", Phone = "01000000128", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 129, Name = "مركز ههيا", Phone = "01000000129", IsActive = true, GovernorateId = 20 },
    new Office { OfficeId = 130, Name = "مركز العاشر من رمضان", Phone = "01000000130", IsActive = true, GovernorateId = 20 },

    // 21 - جنوب سيناء
    new Office { OfficeId = 131, Name = "مركز شرم الشيخ", Phone = "01000000131", IsActive = true, GovernorateId = 21 },
    new Office { OfficeId = 132, Name = "مركز دهب", Phone = "01000000132", IsActive = true, GovernorateId = 21 },
    new Office { OfficeId = 133, Name = "مركز طور سيناء", Phone = "01000000133", IsActive = true, GovernorateId = 21 },
    new Office { OfficeId = 134, Name = "مركز أبو رديس", Phone = "01000000134", IsActive = true, GovernorateId = 21 },
    new Office { OfficeId = 135, Name = "مركز نويبع", Phone = "01000000135", IsActive = true, GovernorateId = 21 },

    // 22 - كفر الشيخ
    new Office { OfficeId = 136, Name = "مركز كفر الشيخ", Phone = "01000000136", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 137, Name = "مركز دسوق", Phone = "01000000137", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 138, Name = "مركز فوة", Phone = "01000000138", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 139, Name = "مركز بيلا", Phone = "01000000139", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 140, Name = "مركز الرياض", Phone = "01000000140", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 141, Name = "مركز مطوبس", Phone = "01000000141", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 142, Name = "مركز سيدي سالم", Phone = "01000000142", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 143, Name = "مركز الحامول", Phone = "01000000143", IsActive = true, GovernorateId = 22 },
    new Office { OfficeId = 144, Name = "مركز قلين", Phone = "01000000144", IsActive = true, GovernorateId = 22 },

    // 23 - مطروح
    new Office { OfficeId = 145, Name = "مركز مطروح", Phone = "01000000145", IsActive = true, GovernorateId = 23 },
    new Office { OfficeId = 146, Name = "مركز سيدي براني", Phone = "01000000146", IsActive = true, GovernorateId = 23 },
    new Office { OfficeId = 147, Name = "مركز الضبعة", Phone = "01000000147", IsActive = true, GovernorateId = 23 },
    new Office { OfficeId = 148, Name = "مركز سلوم", Phone = "01000000148", IsActive = true, GovernorateId = 23 },
    new Office { OfficeId = 149, Name = "مركز السلوم", Phone = "01000000149", IsActive = true, GovernorateId = 23 },

    // 24 - الأقصر
    new Office { OfficeId = 150, Name = "مركز الأقصر", Phone = "01000000150", IsActive = true, GovernorateId = 24 },
    new Office { OfficeId = 151, Name = "مركز إسنا", Phone = "01000000151", IsActive = true, GovernorateId = 24 },
    new Office { OfficeId = 152, Name = "مركز أرمنت", Phone = "01000000152", IsActive = true, GovernorateId = 24 },

    // 25 - قنا
    new Office { OfficeId = 153, Name = "مركز قنا", Phone = "01000000153", IsActive = true, GovernorateId = 25 },
    new Office { OfficeId = 154, Name = "مركز نجع حمادي", Phone = "01000000154", IsActive = true, GovernorateId = 25 },
    new Office { OfficeId = 155, Name = "مركز قوص", Phone = "01000000155", IsActive = true, GovernorateId = 25 },
    new Office { OfficeId = 156, Name = "مركز دشنا", Phone = "01000000156", IsActive = true, GovernorateId = 25 },
    new Office { OfficeId = 157, Name = "مركز أبو تشت", Phone = "01000000157", IsActive = true, GovernorateId = 25 },
    new Office { OfficeId = 158, Name = "مركز فرشوط", Phone = "01000000158", IsActive = true, GovernorateId = 25 },
    new Office { OfficeId = 159, Name = "مركز نقادة", Phone = "01000000159", IsActive = true, GovernorateId = 25 },

    // 26 - شمال سيناء
    new Office { OfficeId = 160, Name = "مركز العريش", Phone = "01000000160", IsActive = true, GovernorateId = 26 },
    new Office { OfficeId = 161, Name = "مركز رفح", Phone = "01000000161", IsActive = true, GovernorateId = 26 },
    new Office { OfficeId = 162, Name = "مركز الشيخ زويد", Phone = "01000000162", IsActive = true, GovernorateId = 26 },
    new Office { OfficeId = 163, Name = "مركز بئر العبد", Phone = "01000000163", IsActive = true, GovernorateId = 26 },
    new Office { OfficeId = 164, Name = "مركز نخل", Phone = "01000000164", IsActive = true, GovernorateId = 26 },

    // 27 - سوهاج
    new Office { OfficeId = 165, Name = "مركز سوهاج", Phone = "01000000165", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 166, Name = "مركز طهطا", Phone = "01000000166", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 167, Name = "مركز جرجا", Phone = "01000000167", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 168, Name = "مركز أخميم", Phone = "01000000168", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 169, Name = "مركز البلينا", Phone = "01000000169", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 170, Name = "مركز دار السلام", Phone = "01000000170", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 171, Name = "مركز المراغة", Phone = "01000000171", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 172, Name = "مركز المنشاة", Phone = "01000000172", IsActive = true, GovernorateId = 27 },
    new Office { OfficeId = 173, Name = "مركز ساقلتة", Phone = "01000000173", IsActive = true, GovernorateId = 27 }
);
            #endregion


        }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }        
        public DbSet<TimelineEntry> TimelineEntries { get; set; }   
        public DbSet<ApplicationDocuments> ApplicationDocuments { get; set; }       
       // public DbSet<ApplicationTypeHelper> ApplicationTypeHelpers { get; set; }    
        public DbSet<ServicesTypeHelper> ServicesTypeHelpers { get; set; }  

    }
}