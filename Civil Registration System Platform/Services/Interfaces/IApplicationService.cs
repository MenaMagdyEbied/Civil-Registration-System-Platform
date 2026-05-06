using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.ViewModel;
using Civil_Registration_System_Platform.ViewModel.Application;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
   
    //عمليات المواطن فقط — Submit / Track / عرض طلباته / إلغاء
    
    public interface IApplicationService
    {
        Task<string> SubmitApplicationAsync(ApplyViewModel model, string userId);

        Task<ApplicationTrackResultVM?> TrackApplicationAsync(string applicationNumber);

        Task<IEnumerable<ApplicationSummaryClientVM>> GetUserApplicationsAsync(string userId);

        Task<ApplicationTrackResultVM?> GetApplicationDetailsAsync(int applicationId, string userId);
        Task CancelApplicationAsync(int applicationId, string userId);
        Task<string> RespondToAdditionalInfoAsync(
            int applicationId,
            string userId,
            List<IFormFile>? additionalDocs,
            string? note);
    }
}
