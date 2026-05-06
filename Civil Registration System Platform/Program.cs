using Civil_Registration_System_Platform.Repositories.Implementations;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Implementations;
using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Civil_Registration_System_Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Global
            builder.Services.AddScoped<IUserGlobalServices, UserGlobalServices>();


            // Repositories
            builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();// account
            builder.Services.AddScoped<IGovernorateRepository, GovernorateRepository>();
            builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IApplicationDocumentRepository, ApplicationDocumentRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<ITimelineEntryRepository, TimelineEntryRepository>();
            builder.Services.AddScoped<IServicesTypeHelperRepository, ServicesTypeHelperRepository>();
          //  builder.Services.AddScoped<IApplicationTypeHelperRepository, ApplicationTypeHelperRepository>();

            // Services
            builder.Services.AddScoped<IPricingService, PricingService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IGovernorateServices, GovernorateServices>();
            builder.Services.AddScoped<IOfficeServices, OfficeServices>();
            builder.Services.AddScoped<IAccountServices, AccountServices>();
            builder.Services.AddScoped<IAcccountManageServices, AcccountManageServices>();

            // Services — already implemented but were missing DI registration
            builder.Services.AddScoped<IHomePageService, HomePageService>();
            builder.Services.AddScoped<IAdminApplicationService, AdminApplicationService>();
            builder.Services.AddScoped<ISuperAdminService, SuperAdminService>();

            // Services — newly added (citizen dashboard, apply form, role management)
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IApplyFormService, ApplyFormService>();
            builder.Services.AddScoped<IAdminManagementService, AdminManagementService>();
            builder.Services.AddScoped<IEmployeeManagementService, EmployeeManagementService>();

            // SuperAdmin-only — pricing management (CRUD on ServicesTypeHelper)
            builder.Services.AddScoped<IPricingManagementService, PricingManagementService>();

            builder.Services.AddHttpContextAccessor();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<UserAccount, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            // connction string
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                SeedIdentityDataAsync(scope.ServiceProvider).GetAwaiter().GetResult();
            }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
             //   .WithStaticAssets();

            app.Run();
        }

        private static async Task SeedIdentityDataAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<UserAccount>>();

            string[] roles = ["SuperAdmin", "Admin", "Employee", "User", "AccountReviewer"];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var reviewer = await userManager.FindByEmailAsync("acrev@crs.gov.eg")
                        ?? await userManager.FindByNameAsync("acrev");

            if (reviewer == null)
            {
                reviewer = new UserAccount
                {
                    UserName = "acrev",
                    Email = "acrev@crs.gov.eg",
                    EmailConfirmed = true,
                    FullName = "Account Reviewer",
                    EGPhoneNumber = "01000000001",
                    NationalID = "12345678901235",
                    Gender = 1,
                    MaritalStatus = 1,
                    IsConfirmed = true,
                    CardImagePath = string.Empty,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsRejected = false
                };

                var createResult = await userManager.CreateAsync(reviewer, "Reviewer@123");
                if (!createResult.Succeeded)
                    throw new InvalidOperationException(string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }
            else
            {
                reviewer.UserName = "acrev";
                reviewer.Email = "acrev@crs.gov.eg";
                reviewer.EmailConfirmed = true;
                reviewer.IsConfirmed = true;
                reviewer.IsRejected = false;
                reviewer.RejectionMessage = null;
                reviewer.PasswordHash = userManager.PasswordHasher.HashPassword(reviewer, "Reviewer@123");
                await userManager.UpdateAsync(reviewer);
            }

            if (!await userManager.IsInRoleAsync(reviewer, "AccountReviewer"))
                await userManager.AddToRoleAsync(reviewer, "AccountReviewer");
        }
    }
}





