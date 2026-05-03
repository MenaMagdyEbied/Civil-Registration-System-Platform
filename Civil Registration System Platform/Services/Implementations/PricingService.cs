using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class PricingService : IPricingService
    {
        private readonly IServicesTypeHelperRepository _serviceRepo;
        private readonly IApplicationTypeHelperRepository _appTypeRepo;

        public PricingService(
            IServicesTypeHelperRepository serviceRepo,
            IApplicationTypeHelperRepository appTypeRepo)
        {
            _serviceRepo = serviceRepo;
            _appTypeRepo = appTypeRepo;
        }

        public async Task<int> CalculatePriceAsync(
            ServiceType serviceType, ApplicationType? applicationType)
        {
            var serviceInfo = await _serviceRepo
                .GetByServiceTypeAsync((int)serviceType);
            var basePrice = serviceInfo?.Price ?? 0;

            if (applicationType.HasValue)
            {
                var appTypeInfo = await _appTypeRepo
                    .GetByApplicationTypeAsync((int)applicationType.Value);

                if (appTypeInfo?.Price > 0)
                    return appTypeInfo.Price;
            }

            return basePrice;
        }

        public async Task<int> GetDurationInDaysAsync(ServiceType serviceType)
        {
            var serviceInfo = await _serviceRepo
                .GetByServiceTypeAsync((int)serviceType);
            return serviceInfo?.DurationInDays ?? 0;
        }

        public async Task<IEnumerable<ServicesTypeHelper>> GetAllServicesPricingAsync()
            => await _serviceRepo.GetAllAsync();
    }
}
