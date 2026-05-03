using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<Application> SubmitApplicationAsync(ApplyViewModel model, string userId);
        // GenerateApplicationNumberAsync
        //  Status = Submitted
        //  FileService.UploadDocumentAsync لكل ملف
        //  حفظ كل مستند في ApplicationDocuments
        //  TimelineEntryRepository.AddEntryAsync — تم استلام طلبك
        //  PricingService.CalculatePriceAsync — حساب السعر
        //  SaveChangesAsync

        Task<Application?> TrackApplicationAsync(string applicationNumber);
        Task<IEnumerable<Application>> GetUserApplicationsAsync(string userId);
        Task<Application?> GetApplicationDetailsAsync(int applicationId, string userId);
        Task ChangeStatusAsync(int applicationId, ApplicationStatus newStatus, string adminId,
            string? note = null);
      
        Task ApproveApplicationAsync(int applicationId, string adminId, string? notes);
        Task RejectApplicationAsync(int applicationId, string adminId, string reason);
        Task RequestAdditionalInfoAsync(int applicationId, string adminId, string details);
        Task IssueApplicationAsync(int applicationId, string adminId);
        Task CancelApplicationAsync(int applicationId, string userId);

        Task<IEnumerable<Application>> GetOfficeApplicationsAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType);

        Task<(int Today, int PendingReview, int Issued, int Rejected)> GetOfficeDashboardStatsAsync(int officeId);
    }
}
