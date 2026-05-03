using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class OfficeRepository : GenericRepository<Office>, IOfficeRepository
    {
        public OfficeRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Office>> GetByGovernorateIdAsync(int governorateId)
            => await _context.Offices
                .Where(o => o.GovernorateId == governorateId && o.IsActive)
                .ToListAsync();

        public async Task<Office?> GetWithGovernorateAsync(int officeId)
            => await _context.Offices
                .Include(o => o.Governorate)
                .FirstOrDefaultAsync(o => o.OfficeId == officeId);

        public async Task<IEnumerable<Office>> GetActiveOfficesAsync()
            => await _context.Offices
                .Where(o => o.IsActive)
                .Include(o => o.Governorate)
                .ToListAsync();
    }
}
