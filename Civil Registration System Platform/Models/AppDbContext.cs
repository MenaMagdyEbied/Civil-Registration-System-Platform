
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

        }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }        
        public DbSet<TimelineEntry> TimelineEntries { get; set; }   
        public DbSet<ApplicationDocuments> ApplicationDocuments { get; set; }       
        public DbSet<ApplicationTypeHelper> ApplicationTypeHelpers { get; set; }    
        public DbSet<ServicesTypeHelper> ServicesTypeHelpers { get; set; }  

    }
}