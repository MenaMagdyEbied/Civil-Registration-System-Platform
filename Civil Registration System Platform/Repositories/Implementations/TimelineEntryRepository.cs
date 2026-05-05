using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Implementations;
using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class TimelineEntryRepository
        : GenericRepository<TimelineEntry>, ITimelineEntryRepository
    {
        public TimelineEntryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<TimelineEntry>> GetByApplicationIdAsync(int applicationId)
            => await _context.TimelineEntries
                .Where(t => t.ApplicationId == applicationId && !t.IsDeleted)
                .OrderBy(t => t.Timestamp)
                .ToListAsync();

        public async Task AddEntryAsync(int applicationId, ApplicationStatus status,string description, string performedById)
        {
            var entry = new TimelineEntry
            {
                ApplicationId = applicationId,
                Status = (int)status,
                Description = description,
                PerformedById = performedById,
                UserAccountId = performedById,
                Timestamp = DateTime.UtcNow,
                IsDeleted = false
            };
            await _context.TimelineEntries.AddAsync(entry);
        }
    }
}
