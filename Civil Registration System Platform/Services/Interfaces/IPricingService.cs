using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IPricingService
    {
        Task<int> CalculatePriceAsync(ServiceType serviceType, ApplicationType? applicationType);

        Task<int> GetDurationInDaysAsync(ServiceType serviceType, ApplicationType? applicationType);
        Task<IEnumerable<ServicesTypeHelper>> GetAllServicesPricingAsync();
        Task<IEnumerable<ServicesTypeHelper>> GetServiceTypesAsync(ServiceType serviceType);
    }
}
