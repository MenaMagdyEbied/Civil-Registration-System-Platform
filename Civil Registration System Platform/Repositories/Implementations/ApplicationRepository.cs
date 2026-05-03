using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(AppDbContext context) : base(context) { }

        public async Task<Application?> GetByApplicationNumberAsync(string applicationNumber)
            => await _context.Applications
                .Include(a => a.TimelineEntries.OrderBy(t => t.Timestamp))
                .Include(a => a.UserAccount)
                .Include(a => a.Office)
                .FirstOrDefaultAsync(a => a.ApplicationNumber == applicationNumber
                                       && !a.IsDeleted);

        public async Task<Application?> GetWithFullDetailsAsync(int applicationId)
            => await _context.Applications
                .Include(a => a.UserAccount)
                    .ThenInclude(u => u.Governorate)
                .Include(a => a.Office)
                    .ThenInclude(o => o.Governorate)
                .Include(a => a.ReviewedUserAccount)
                .Include(a => a.ApplicationDocuments.Where(d => !d.IsDeleted))
                .Include(a => a.TimelineEntries
                    .Where(t => !t.IsDeleted)
                    .OrderBy(t => t.Timestamp))
                .Include(a => a.Appointments.Where(ap => !ap.IsDeleted))
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && !a.IsDeleted);

        public async Task<IEnumerable<Application>> GetByUserIdAsync(string userId)
            => await _context.Applications
                .Where(a => a.UserAccountId == userId && !a.IsDeleted)
                .Include(a => a.Office)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Application>> GetByOfficeIdAsync(int officeId)
            => await _context.Applications
                .Where(a => a.OfficeId == officeId && !a.IsDeleted)
                .Include(a => a.UserAccount)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Application>> GetPendingReviewAsync(int officeId)
            => await _context.Applications
                .Where(a => a.OfficeId == officeId
                         && !a.IsDeleted
                         && (a.Status == (int)ApplicationStatus.Submitted
                          || a.Status == (int)ApplicationStatus.UnderReview))
                .Include(a => a.UserAccount)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Application>> GetFilteredAsync(
            int officeId,
            ApplicationStatus? status,
            ServiceType? serviceType)
        {
            var query = _context.Applications
                .Where(a => a.OfficeId == officeId && !a.IsDeleted)
                .Include(a => a.UserAccount)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(a => a.Status == (int)status.Value);

            if (serviceType.HasValue)
                query = query.Where(a => a.ServiceType == (int)serviceType.Value);

            return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        public async Task<int> GetTodayCountAsync(int officeId)
        {
            var today = DateTime.Today;
            return await _context.Applications
                .CountAsync(a => a.OfficeId == officeId
                              && !a.IsDeleted
                              && a.CreatedAt.Date == today);
        }

        public async Task<string> GenerateApplicationNumberAsync()
        {
            var year = DateTime.Now.Year;
            var count = await _context.Applications
                .CountAsync(a => a.CreatedAt.Year == year);
            return $"CRS-{year}-{(count + 1):D4}";
        }
    }
}
