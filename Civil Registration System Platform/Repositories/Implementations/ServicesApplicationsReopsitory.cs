using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class ServicesApplicationsReopsitory : IServicesApplicationsReopsitory
    {
        private readonly AppDbContext _context;
        public ServicesApplicationsReopsitory(AppDbContext context)
        {
            _context = context; 
        }

        public async Task<List<ServicesTypeHelper>> GetAll()
        {
            List<ServicesTypeHelper> servicesTypeHelpers = await _context.ServicesTypeHelpers.ToListAsync(); 
            return servicesTypeHelpers;
        }

        public async Task<ServicesTypeHelper> GetByIds(int servicesType, int ApplicationType)
        {
            ServicesTypeHelper? servicesTypeHelper = await _context.ServicesTypeHelpers.SingleOrDefaultAsync(s=>s.ServicesTypeEnum== servicesType && s.ApplicationTypeEnum == ApplicationType);
            return servicesTypeHelper;
        }
        public async Task<ServicesTypeHelper> Update(ServicesTypeHelper servicesTypeHelper)
        {
            _context.ServicesTypeHelpers.Update(servicesTypeHelper);
            await _context.SaveChangesAsync();
            return servicesTypeHelper;
        }
    }
}
