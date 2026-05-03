using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<Application?> GetByApplicationNumberAsync(string applicationNumber);
        Task<Application?> GetWithFullDetailsAsync(int applicationId); // admin dashboard 

        Task<IEnumerable<Application>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Application>> GetByOfficeIdAsync(int officeId); 
        Task<IEnumerable<Application>> GetPendingReviewAsync(int officeId);
        Task<IEnumerable<Application>> GetFilteredAsync(int officeId, ApplicationStatus? status,ServiceType? serviceType);
        Task<int> GetTodayCountAsync(int officeId);
        Task<string> GenerateApplicationNumberAsync();
    }
}
