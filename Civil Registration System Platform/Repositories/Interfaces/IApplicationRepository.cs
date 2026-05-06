using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<Application?> GetByApplicationNumberAsync(string applicationNumber);
        Task<Application?> GetWithFullDetailsAsync(int applicationId);

        Task<IEnumerable<Application>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Application>> GetByOfficeIdAsync(int officeId);
        Task<IEnumerable<Application>> GetPendingReviewAsync(int officeId);
        Task<IEnumerable<Application>> GetFilteredAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType);
        Task<IEnumerable<Application>> GetFilteredByOfficesAsync(
            IEnumerable<int> officeIds, ApplicationStatus? status, ServiceType? serviceType);

        Task<int> GetTodayCountAsync(int officeId);
        Task<int> CountByStatusAsync(int officeId, params ApplicationStatus[] statuses);
        Task<string> GenerateApplicationNumberAsync();

        Task<Dictionary<int, Dictionary<int, int>>> GetStatsByOfficesAsync(IEnumerable<int> officeIds);
        Task<Dictionary<int, int>> GetTodayCountByOfficesAsync(IEnumerable<int> officeIds);
    }
}
