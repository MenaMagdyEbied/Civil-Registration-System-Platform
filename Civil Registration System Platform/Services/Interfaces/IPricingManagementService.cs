using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    // إدارة أسعار الخدمات — السوبر أدمن فقط
    
    public interface IPricingManagementService
    {
        Task<List<PricingListItemVM>> GetAllPricesAsync();

        Task<PricingFormVM?> GetPriceAsync(ServiceType serviceType, ApplicationType applicationType);

        Task<string> UpsertPriceAsync(PricingFormVM model);
        Task<string> DeletePriceAsync(ServiceType serviceType, ApplicationType applicationType);
    }
}
