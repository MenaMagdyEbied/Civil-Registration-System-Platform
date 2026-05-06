using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class HomePageService : IHomePageService
    {
        private readonly IServicesTypeHelperRepository _serviceRepo;
        public HomePageService(IServicesTypeHelperRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        public async Task<HomePageVM> GetHomePageDataAsync()
        {
            var allRows = (await _serviceRepo.GetAllAsync()).ToList();

            var serviceCards = BuildServiceCards(allRows);
            var feeRows  = BuildFeeRows(allRows);

            return new HomePageVM
            {
                ServiceCards = serviceCards,
                FeeRows      = feeRows
            };
        }

        private static List<ServiceCardVM> BuildServiceCards(
            List<Models.ServicesTypeHelper> allRows)
        {
            var meta = GetServiceMeta();

            return allRows
                .GroupBy(r => r.ServicesTypeEnum)
                .Where(g => meta.ContainsKey(g.Key))
                .Select(g =>
                {
                    var serviceType = (ServiceType)g.Key;
                    var m   = meta[g.Key];
                    var rows  = g.ToList();

                    var minPrice = rows.Min(r => r.Price);
                    var maxPrice = rows.Max(r => r.Price);
                    var minDays  = rows.Min(r => r.DurationInDays);
                    var maxDays  = rows.Max(r => r.DurationInDays);

                    var priceDisplay = minPrice == maxPrice
                        ? (minPrice > 0 ? $"{minPrice} ج.م" : "مجاناً")
                        : $"{minPrice} - {maxPrice} ج.م";

                    var durationDisplay = minDays == maxDays
                        ? FormatDuration(minDays)
                        : $"{FormatDuration(minDays)} - {FormatDuration(maxDays)}";

                    return new ServiceCardVM
                    {
                        ServiceTypeId   = g.Key,
                        ServiceTypeName = serviceType.ToArabicName(),
                        Description = m.Description,
                        Icon = m.Icon,
                        ColorClass = m.ColorClass,
                        Category = m.Category,
                        PriceRange = priceDisplay,
                        DurationText = durationDisplay,
                        ApplyUrl = $"/Apply?serviceType={(int)serviceType}"
                    };
                })
                .ToList();
        }

        private static List<FeeRowVM> BuildFeeRows(List<Models.ServicesTypeHelper> allRows)
        {
            var rows = new List<FeeRowVM>();

            var grouped = allRows
                .OrderBy(r => r.ServicesTypeEnum)
                .ThenBy(r => r.ApplicationTypeEnum)
                .GroupBy(r => r.ServicesTypeEnum)
                .ToList();

            foreach (var group in grouped)
            {
                var serviceType = (ServiceType)group.Key;
                var serviceName = serviceType.ToArabicName();
                var groupList   = group.ToList();
                bool isFirst    = true;

                foreach (var row in groupList)
                {
                    var appType = (ApplicationType)row.ApplicationTypeEnum;

                    rows.Add(new FeeRowVM
                    {
                        ServiceTypeName = serviceName,
                        ApplicationTypeName = appType.ToArabicName(),
                        Price   = row.Price,
                        PriceDisplay = row.Price > 0 ? $"{row.Price} ج.م" : "مجاناً",
                        DurationInDays  = row.DurationInDays,
                        DurationDisplay = FormatDuration(row.DurationInDays),
                        IsGroupHeader = isFirst,
                        RowSpan  = isFirst ? groupList.Count : 0
                    });

                    isFirst = false;
                }
            }

            return rows;
        }
        private static string FormatDuration(int days) => days switch
        {
            0     => "فوري",
            1     => "يوم واحد",
            <= 3  => $"1-{days} أيام",
            <= 7  => $"3-{days} أيام",
            <= 14 => $"7-{days} يوم",
            _     => $"{days} يوم"
        };

        private static Dictionary<int, (string Icon, string ColorClass, string Category, string Description)>
            GetServiceMeta() => new()
        {
            [(int)ServiceType.BirthCertificate]    = ("bi-file-earmark-medical", "color-birth",      "civil",    "إصدار أو استخراج شهادة ميلاد مميكنة"),
            [(int)ServiceType.DeathCertificate]    = ("bi-file-earmark-minus",   "color-death",      "civil",    "إصدار شهادة وفاة مميكنة"),
            [(int)ServiceType.MarriageCertificate] = ("bi-heart",                "color-marriage",   "civil",    "استخراج قسيمة زواج رسمية"),
            [(int)ServiceType.DivorceCertificate]  = ("bi-scissors",             "color-divorce",    "civil",    "استخراج قسيمة طلاق رسمية"),
            [(int)ServiceType.NationalId]          = ("bi-person-badge",         "color-nationalid", "identity", "استخراج أو تجديد أو بدل فاقد/تالف"),
            [(int)ServiceType.Passport]            = ("bi-airplane",             "color-passport",   "travel",   "استخراج أو تجديد أو بدل فاقد/تالف"),
            [(int)ServiceType.DriversLicense]      = ("bi-car-front",            "color-license",    "travel",   "استخراج أو تجديد أو بدل فاقد/تالف"),
            [(int)ServiceType.FamilyCard]          = ("bi-people",               "color-family",     "civil",    "استخراج قيد عائلي رسمي"),
            [(int)ServiceType.IndividualRecord]    = ("bi-person-lines-fill",    "color-individual", "identity", "استخراج قيد فردي رسمي"),
            [(int)ServiceType.CriminalRecord]      = ("bi-shield-check",         "color-individual", "identity", "صحيفة السوابق الجنائية"),
            [(int)ServiceType.CustomDocument]      = ("bi-file-earmark-text",    "color-individual", "civil",    "مستند مخصص"),
        };
    }
}
