using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IApplyFormService
    {
        Task<ApplyFormDataVM?> GetApplyFormDataAsync(ServiceType serviceType);
    }
}
