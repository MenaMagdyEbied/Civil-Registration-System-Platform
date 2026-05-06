using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    // عمليات الأدمن على الطلبات — مكتب واحد
    public interface IAdminApplicationService
    {
        Task<AdminDashboardVM> GetOfficeDashboardAsync(
            int officeId, ApplicationStatus? filterStatus, ServiceType? filterService);
        Task<AdminDashboardVM> GetOfficesDashboardAsync(
            IEnumerable<int> officeIds, ApplicationStatus? filterStatus, ServiceType? filterService);
        Task<IEnumerable<ApplicationSummaryAdminVM>> GetOfficeApplicationsAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType);
        Task<IEnumerable<ApplicationSummaryAdminVM>> GetOfficesApplicationsAsync(
            IEnumerable<int> officeIds, ApplicationStatus? status, ServiceType? serviceType);
        Task<ReviewApplicationVM?> GetReviewDetailsAsync(int applicationId);

        Task ChangeStatusAsync(int applicationId, ApplicationStatus newStatus,
            string adminId, string? note = null);

        Task ApproveApplicationAsync(int applicationId, string adminId, string? notes);
        Task RejectApplicationAsync(int applicationId, string adminId, string reason);
        Task RequestAdditionalInfoAsync(int applicationId, string adminId, string details);
        Task IssueApplicationAsync(int applicationId, string adminId);
    }
}
