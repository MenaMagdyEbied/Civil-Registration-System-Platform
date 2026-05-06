using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.ViewModel
{
    public class ServiceTypeOptionVM
    {
        public int ApplicationTypeId { get; set; }            
        public string ApplicationTypeName { get; set; }       
        public int Price { get; set; }
        public string PriceDisplay { get; set; }
        public int DurationInDays { get; set; }
        public string DurationDisplay { get; set; }
        public List<string> RequiredDocuments { get; set; } = new();
    }

    public class ApplyFormDataVM
    {
        public int ServiceTypeId { get; set; }                
        public string ServiceTypeName { get; set; }
        public string Icon { get; set; }
        public string ColorClass { get; set; }
        public string Description { get; set; }

        public List<ServiceTypeOptionVM> AvailableTypes { get; set; } = new();
        public List<GovernorateGetAll> Governorates { get; set; } = new();
    }
}
