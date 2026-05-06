using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{

    public class ApplyFormService : IApplyFormService
    {
        private readonly IPricingService _pricingService;
        private readonly IGovernorateServices _governorateServices;

        public ApplyFormService(
            IPricingService pricingService,
            IGovernorateServices governorateServices)
        {
            _pricingService = pricingService;
            _governorateServices = governorateServices;
        }

        public async Task<ApplyFormDataVM?> GetApplyFormDataAsync(ServiceType serviceType)
        {
            var rows = (await _pricingService.GetServiceTypesAsync(serviceType)).ToList();
            if (!rows.Any()) return null;

            var meta = GetServiceMeta(serviceType);

            var availableTypes = rows.Select(r =>
            {
                var appType = (ApplicationType)r.ApplicationTypeEnum;
                return new ServiceTypeOptionVM
                {
                    ApplicationTypeId  = r.ApplicationTypeEnum,
                    ApplicationTypeName = appType.ToArabicName(),
                    Price = r.Price,
                    PriceDisplay  = r.Price > 0 ? $"{r.Price} ج.م" : "مجاناً",
                    DurationInDays  = r.DurationInDays,
                    DurationDisplay = FormatDuration(r.DurationInDays),
                    RequiredDocuments  = ParseRequiredDocs(r.Details)
                };
            }).ToList();

            var governorates = await _governorateServices.GetAllGovernoratesAsync();

            return new ApplyFormDataVM
            {
                ServiceTypeId = (int)serviceType,
                ServiceTypeName = serviceType.ToArabicName(),
                Icon = meta.Icon,
                ColorClass = meta.ColorClass,
                Description = meta.Description,
                AvailableTypes = availableTypes,
                Governorates = governorates
            };
        }


        private static (string Icon, string ColorClass, string Description) GetServiceMeta(ServiceType type)
            => type switch
            {
                ServiceType.BirthCertificate  => ("bi-file-earmark-medical", "color-birth",      "إصدار أو استخراج شهادة ميلاد مميكنة"),
                ServiceType.DeathCertificate    => ("bi-file-earmark-minus",   "color-death",      "إصدار شهادة وفاة مميكنة"),
                ServiceType.MarriageCertificate => ("bi-heart",                "color-marriage",   "استخراج قسيمة زواج رسمية"),
                ServiceType.DivorceCertificate  => ("bi-scissors",             "color-divorce",    "استخراج قسيمة طلاق رسمية"),
                ServiceType.NationalId => ("bi-person-badge",         "color-nationalid", "استخراج أو تجديد أو بدل فاقد/تالف"),
                ServiceType.Passport  => ("bi-airplane",             "color-passport",   "استخراج أو تجديد أو بدل فاقد/تالف"),
                ServiceType.DriversLicense => ("bi-car-front",            "color-license",    "استخراج أو تجديد أو بدل فاقد/تالف"),
                ServiceType.FamilyCard  => ("bi-people",               "color-family",     "استخراج قيد عائلي رسمي"),
                ServiceType.IndividualRecord => ("bi-person-lines-fill",    "color-individual", "استخراج قيد فردي رسمي"),
                ServiceType.CriminalRecord => ("bi-shield-check",         "color-individual", "صحيفة السوابق الجنائية"),
                ServiceType.CustomDocument  => ("bi-file-earmark-text",    "color-individual", "مستند مخصص"),
                _                               => ("bi-file-earmark",         "color-birth",      string.Empty)
            };

        private static string FormatDuration(int days) => days switch
        {
            0  => "فوري",
            1  => "يوم واحد",
            <= 3  => $"1-{days} أيام",
            <= 7  => $"3-{days} أيام",
            <= 14 => $"7-{days} يوم",
            _     => $"{days} يوم"
        };

     
        private static List<string> ParseRequiredDocs(string? details)
        {
            if (string.IsNullOrWhiteSpace(details)) return new();

            var separators = new[] { '|', '\n', '\r', ';' };
            return details
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();
        }
    }
}
