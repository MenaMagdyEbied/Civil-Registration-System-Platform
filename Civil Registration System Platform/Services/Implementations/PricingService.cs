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
            throw new NotImplementedException();    
            //var serviceInfo = await _serviceRepo
            //    .GetByServiceTypeAsync((int)serviceType);
            //var basePrice = serviceInfo?.Price ?? 0;

            //if (applicationType.HasValue)
            //{
            //    var appTypeInfo = await _serviceRepo
            //        .GetByApplicationTypeAsync((int)applicationType.Value);

            //    if (appTypeInfo?.Price > 0)
            //        return appTypeInfo.Price;
            //}

            //return basePrice;
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
