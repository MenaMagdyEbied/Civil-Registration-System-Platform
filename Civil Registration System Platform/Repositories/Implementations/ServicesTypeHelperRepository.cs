using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class ServicesTypeHelperRepository : IServicesTypeHelperRepository
    {
        private readonly AppDbContext _context;
        public ServicesTypeHelperRepository(AppDbContext context) => _context = context;

        public async Task<ServicesTypeHelper?> GetByServiceTypeAsync(int serviceTypeEnum)
            => await _context.ServicesTypeHelpers
                .FirstOrDefaultAsync(s => s.ServicesTypeEnum == serviceTypeEnum);

        public async Task<IEnumerable<ServicesTypeHelper>> GetAllAsync()
            => await _context.ServicesTypeHelpers.ToListAsync();
    }

    public class ApplicationTypeHelperRepository : IApplicationTypeHelperRepository
    {
        private readonly AppDbContext _context;
        public ApplicationTypeHelperRepository(AppDbContext context) => _context = context;

        public async Task<ApplicationTypeHelper?> GetByApplicationTypeAsync(int appTypeEnum)
            => await _context.ApplicationTypeHelpers
                .FirstOrDefaultAsync(a => a.ApplicationTypeEnum == appTypeEnum);

        public async Task<IEnumerable<ApplicationTypeHelper>> GetAllAsync()
            => await _context.ApplicationTypeHelpers.ToListAsync();
    }
}
