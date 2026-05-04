using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.ViewModel;
using Civil_Registration_System_Platform.ViewModel.Application;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<string> SubmitApplicationAsync(ApplyViewModel model, string userId);

        Task<ApplicationTrackResultVM?> TrackApplicationAsync(string applicationNumber);

        Task<IEnumerable<ApplicationSummaryClientVM>> GetUserApplicationsAsync(string userId);

        Task<ApplicationTrackResultVM?> GetApplicationDetailsAsync(int applicationId, string userId);
        Task ChangeStatusAsync(int applicationId, ApplicationStatus newStatus,
            string adminId, string? note = null);

        Task ApproveApplicationAsync(int applicationId, string adminId, string? notes);
        Task RejectApplicationAsync(int applicationId, string adminId, string reason);
        Task RequestAdditionalInfoAsync(int applicationId, string adminId, string details);
        Task IssueApplicationAsync(int applicationId, string adminId);
        Task CancelApplicationAsync(int applicationId, string userId);

        // Admin  list applications office 
        Task<IEnumerable<ApplicationSummaryAdminVM>> GetOfficeApplicationsAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType);

        // Admin  dashboard stats card values
        Task<AdminDashboardVM> GetOfficeDashboardAsync(
            int officeId, ApplicationStatus? filterStatus, ServiceType? filterService);

        // Admin full details for review 
        Task<ReviewApplicationVM?> GetReviewDetailsAsync(int applicationId);
    }
}
