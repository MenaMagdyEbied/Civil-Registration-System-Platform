using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class GovernorateRepository : GenericRepository<Governorate>, IGovernorateRepository
    {
        public GovernorateRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Governorate>> GetActiveGovernoratesAsync()
            => await _context.Governorates
                .Where(g => g.IsActive)
                .OrderBy(g => g.Name)
                .ToListAsync();
    }
}
