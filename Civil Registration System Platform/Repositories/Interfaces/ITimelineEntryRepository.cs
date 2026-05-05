using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface ITimelineEntryRepository : IGenericRepository<TimelineEntry>
    {
        Task<IEnumerable<TimelineEntry>> GetByApplicationIdAsync(int applicationId);

        Task AddEntryAsync(int applicationId, ApplicationStatus status, string description, string performedById);
    }
}
