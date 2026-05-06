using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class ServicesTypeHelperRepository : IServicesTypeHelperRepository
    {
        private readonly AppDbContext _context;
        public ServicesTypeHelperRepository(AppDbContext context) => _context = context;

        // ─── Reads ───

        public async Task<ServicesTypeHelper?> GetByServiceAndTypeAsync(
            int serviceTypeEnum, int applicationTypeEnum)
            => await _context.ServicesTypeHelpers
                .FirstOrDefaultAsync(s => s.ServicesTypeEnum    == serviceTypeEnum
                                       && s.ApplicationTypeEnum == applicationTypeEnum);

        public async Task<IEnumerable<ServicesTypeHelper>> GetByServiceTypeAsync(int serviceTypeEnum)
            => await _context.ServicesTypeHelpers
                .Where(s => s.ServicesTypeEnum == serviceTypeEnum)
                .ToListAsync();

        public async Task<IEnumerable<ServicesTypeHelper>> GetAllAsync()
            => await _context.ServicesTypeHelpers.ToListAsync();

        // ─── Writes ───

        public async Task AddAsync(ServicesTypeHelper entity)
        {
            await _context.ServicesTypeHelpers.AddAsync(entity);
        }

        public Task UpdateAsync(ServicesTypeHelper entity)
        {
            _context.ServicesTypeHelpers.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int serviceTypeEnum, int applicationTypeEnum)
        {
            var entity = await _context.ServicesTypeHelpers
                .FirstOrDefaultAsync(s => s.ServicesTypeEnum == serviceTypeEnum
                                       && s.ApplicationTypeEnum == applicationTypeEnum);
            if (entity == null) return;

            _context.ServicesTypeHelpers.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int serviceTypeEnum, int applicationTypeEnum)
            => await _context.ServicesTypeHelpers
                .AnyAsync(s => s.ServicesTypeEnum == serviceTypeEnum
                            && s.ApplicationTypeEnum == applicationTypeEnum);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
