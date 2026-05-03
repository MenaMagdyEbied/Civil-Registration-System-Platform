using Civil_Registration_System_Platform.Repositories.Implementations;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Implementations;
using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Repositories
            builder.Services.AddScoped<IGovernorateRepository, GovernorateRepository>();
            builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IApplicationDocumentRepository, ApplicationDocumentRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<ITimelineEntryRepository, TimelineEntryRepository>();
            builder.Services.AddScoped<IServicesTypeHelperRepository, ServicesTypeHelperRepository>();
            builder.Services.AddScoped<IApplicationTypeHelperRepository, ApplicationTypeHelperRepository>();

            // Services
            builder.Services.AddScoped<IPricingService, PricingService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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





