using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(AppDbContext context) : base(context) { }

        public async Task<Application?> GetByApplicationNumberAsync(string applicationNumber)
            => await _context.Applications
                .Include(a => a.Office)
                .Include(a => a.UserAccount)
                .Include(a => a.TimelineEntries.Where(t => !t.IsDeleted).OrderBy(t => t.Timestamp))
                    .ThenInclude(t => t.PerformedBy)
                .FirstOrDefaultAsync(a => a.ApplicationNumber == applicationNumber && !a.IsDeleted);

        public async Task<Application?> GetWithFullDetailsAsync(int applicationId)
            => await _context.Applications
                .Include(a => a.UserAccount)
                    .ThenInclude(u => u.Governorate)
                .Include(a => a.Office)
                    .ThenInclude(o => o.Governorate)
                .Include(a => a.ReviewedUserAccount)
                .Include(a => a.ApplicationDocuments.Where(d => !d.IsDeleted))
                .Include(a => a.TimelineEntries.Where(t => !t.IsDeleted).OrderBy(t => t.Timestamp))
                    .ThenInclude(t => t.PerformedBy)
                .Include(a => a.Appointments.Where(ap => !ap.IsDeleted))
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && !a.IsDeleted);

        public async Task<IEnumerable<Application>> GetByUserIdAsync(string userId)
            => await _context.Applications
                .Include(a => a.Office)
                .Where(a => a.UserAccountId == userId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Application>> GetByOfficeIdAsync(int officeId)
            => await _context.Applications
                .Include(a => a.UserAccount)
                .Where(a => a.OfficeId == officeId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Application>> GetPendingReviewAsync(int officeId)
            => await _context.Applications
                .Include(a => a.UserAccount)
                .Where(a => a.OfficeId == officeId
                         && !a.IsDeleted
                         && (a.Status == (int)ApplicationStatus.Submitted
                          || a.Status == (int)ApplicationStatus.UnderReview))
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Application>> GetFilteredAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType)
        {
            var query = _context.Applications
                .Include(a => a.UserAccount)
                .Where(a => a.OfficeId == officeId && !a.IsDeleted)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(a => a.Status == (int)status.Value);

            if (serviceType.HasValue)
                query = query.Where(a => a.ServiceType == (int)serviceType.Value);

            return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetFilteredByOfficesAsync(
            IEnumerable<int> officeIds, ApplicationStatus? status, ServiceType? serviceType)
        {
            var ids = officeIds.Distinct().ToList();
            var query = _context.Applications
                .Include(a => a.UserAccount)
                .Where(a => ids.Contains(a.OfficeId) && !a.IsDeleted)
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
        public async Task<int> CountByStatusAsync(int officeId, params ApplicationStatus[] statuses)
        {
            var statusInts = statuses.Select(s => (int)s).ToArray();
            return await _context.Applications
                .CountAsync(a => a.OfficeId == officeId
                              && !a.IsDeleted
                              && statusInts.Contains(a.Status));
        }

        public async Task<string> GenerateApplicationNumberAsync()
        {
            var year  = DateTime.Now.Year;
            var count = await _context.Applications
                .CountAsync(a => a.CreatedAt.Year == year);
            return $"CRS-{year}-{(count + 1):D4}";
        }

        // ─── Bulk stats — replaces N×M queries with one GroupBy ──────────

        public async Task<Dictionary<int, Dictionary<int, int>>> GetStatsByOfficesAsync(
            IEnumerable<int> officeIds)
        {
            var ids = officeIds.ToList();

            // استعلام واحد فقط — Group by (OfficeId, Status)
            var rows = await _context.Applications
                .Where(a => !a.IsDeleted && ids.Contains(a.OfficeId))
                .GroupBy(a => new { a.OfficeId, a.Status })
                .Select(g => new { g.Key.OfficeId, g.Key.Status, Count = g.Count() })
                .ToListAsync();

            // pivot to nested dictionary: {OfficeId → {Status → Count}}
            var result = ids.ToDictionary(id => id, id => new Dictionary<int, int>());
            foreach (var row in rows)
                result[row.OfficeId][row.Status] = row.Count;

            return result;
        }

        public async Task<Dictionary<int, int>> GetTodayCountByOfficesAsync(
            IEnumerable<int> officeIds)
        {
            var ids = officeIds.ToList();
            var today = DateTime.Today;

            var rows = await _context.Applications
                .Where(a => !a.IsDeleted
                         && ids.Contains(a.OfficeId)
                         && a.CreatedAt.Date == today)
                .GroupBy(a => a.OfficeId)
                .Select(g => new { OfficeId = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = ids.ToDictionary(id => id, _ => 0);
            foreach (var row in rows)
                result[row.OfficeId] = row.Count;

            return result;
        }
    }
}
