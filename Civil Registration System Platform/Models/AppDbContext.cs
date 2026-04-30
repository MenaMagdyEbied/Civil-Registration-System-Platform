
using Civil_Registration_System_Platform.Relation_Constraint_Configration;

namespace Civil_Registration_System_Platform.Models
{
    public class AppDbContext : IdentityDbContext<UserAccount>
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  
        {
            
        }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration can be added here if needed
            modelBuilder.ConfigureAll();    
        }

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