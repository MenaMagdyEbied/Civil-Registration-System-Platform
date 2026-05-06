using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    // السوبر أدمن / أسعار الخدمات 
    
    public class PricingManagementService : IPricingManagementService
    {
        private readonly IServicesTypeHelperRepository _repo;

        public PricingManagementService(IServicesTypeHelperRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<PricingListItemVM>> GetAllPricesAsync()
        {
            var rows = (await _repo.GetAllAsync()).ToList();

            return rows
                .OrderBy(r => r.ServicesTypeEnum)
                .ThenBy(r => r.ApplicationTypeEnum)
                .Select(r =>
                {
                    var serviceType = (ServiceType)r.ServicesTypeEnum;
                    var appType = (ApplicationType)r.ApplicationTypeEnum;

                    return new PricingListItemVM
                    {
                        ServiceTypeId = r.ServicesTypeEnum,
                        ServiceTypeName = serviceType.ToArabicName(),
                        ApplicationTypeId = r.ApplicationTypeEnum,
                        ApplicationTypeName = appType.ToArabicName(),
                        Price = r.Price,
                        PriceDisplay = r.Price > 0 ? $"{r.Price} ج.م" : "مجاناً",
                        DurationInDays = r.DurationInDays,
                        DurationDisplay = FormatDuration(r.DurationInDays),
                        Details = r.Details
                    };
                })
                .ToList();
        }

        public async Task<PricingFormVM?> GetPriceAsync(
            ServiceType serviceType, ApplicationType applicationType)
        {
            var row = await _repo.GetByServiceAndTypeAsync((int)serviceType, (int)applicationType);
            if (row == null) return null;

            return new PricingFormVM
            {
                ServiceType = (ServiceType)row.ServicesTypeEnum,
                ApplicationType = (ApplicationType)row.ApplicationTypeEnum,
                Price = row.Price,
                DurationInDays = row.DurationInDays,
                Details = row.Details
            };
        }

        public async Task<string> UpsertPriceAsync(PricingFormVM model)
        {
            var existing = await _repo.GetByServiceAndTypeAsync(
                (int)model.ServiceType, (int)model.ApplicationType);

            if (existing != null)
            {
                // Update
                existing.Price = model.Price;
                existing.DurationInDays = model.DurationInDays;
                existing.Details = model.Details;

                await _repo.UpdateAsync(existing);
                await _repo.SaveChangesAsync();
                return "تم تحديث السعر بنجاح";
            }

            // Insert
            var entity = new ServicesTypeHelper
            {
                ServicesTypeEnum = (int)model.ServiceType,
                ApplicationTypeEnum = (int)model.ApplicationType,
                Price = model.Price,
                DurationInDays = model.DurationInDays,
                Details = model.Details
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return "تم إضافة السعر بنجاح";
        }

        public async Task<string> DeletePriceAsync(
            ServiceType serviceType, ApplicationType applicationType)
        {
            var exists = await _repo.ExistsAsync((int)serviceType, (int)applicationType);
            if (!exists)
                return "السعر غير موجود";

            await _repo.DeleteAsync((int)serviceType, (int)applicationType);
            await _repo.SaveChangesAsync();
            return "تم حذف السعر بنجاح";
        }

        // ─── helpers ───
        private static string FormatDuration(int days) => days switch
        {
            0     => "فوري",
            1     => "يوم واحد",
            <= 3  => $"1-{days} أيام",
            <= 7  => $"3-{days} أيام",
            <= 14 => $"7-{days} يوم",
            _     => $"{days} يوم"
        };
    }
}
