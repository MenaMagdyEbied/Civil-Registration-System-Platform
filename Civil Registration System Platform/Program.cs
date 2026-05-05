using Civil_Registration_System_Platform.Repositories.Implementations;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Implementations;
using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

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
            builder.Services.AddScoped<IServicesApplicationsReopsitory, ServicesApplicationsReopsitory>();

            // Services
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IGovernorateServices, GovernorateServices>();
            builder.Services.AddScoped<IOfficeServices, OfficeServices>();
            builder.Services.AddScoped<IAccountServices, AccountServices>();
            builder.Services.AddScoped<IAcccountManageServices, AcccountManageServices>();
            builder.Services.AddScoped<IServicesApplicationsServices, ServicesApplicationsServices>();  

            builder.Services.AddHttpContextAccessor();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<UserAccount, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            // connction string
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            // app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
             //   .WithStaticAssets();

            app.Run();
        }
    }
}





