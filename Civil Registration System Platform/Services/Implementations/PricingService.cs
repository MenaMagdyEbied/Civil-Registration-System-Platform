using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class PricingService : IPricingService
    {
        private readonly IServicesTypeHelperRepository _serviceRepo;

        public PricingService(IServicesTypeHelperRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }
        public async Task<int> CalculatePriceAsync(
            ServiceType serviceType, ApplicationType? applicationType)
        {
            var appTypeEnum = applicationType.HasValue
                ? (int)applicationType.Value
                : (int)ApplicationType.New;

            var info = await _serviceRepo
                .GetByServiceAndTypeAsync((int)serviceType, appTypeEnum);

            return info?.Price ?? 0;
        }
        public async Task<int> GetDurationInDaysAsync(
            ServiceType serviceType, ApplicationType? applicationType)
        {
            var appTypeEnum = applicationType.HasValue
                ? (int)applicationType.Value
                : (int)ApplicationType.New;

            var info = await _serviceRepo
                .GetByServiceAndTypeAsync((int)serviceType, appTypeEnum);

            return info?.DurationInDays ?? 0;
        }
        public async Task<IEnumerable<ServicesTypeHelper>> GetAllServicesPricingAsync()
            => await _serviceRepo.GetAllAsync();
        public async Task<IEnumerable<ServicesTypeHelper>> GetServiceTypesAsync(ServiceType serviceType)
            => await _serviceRepo.GetByServiceTypeAsync((int)serviceType);
    }
}
