namespace Civil_Registration_System_Platform.ViewModel
{
   
    public class ServiceCardVM
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ColorClass { get; set; }
        public string Category { get; set; }
        public string PriceRange { get; set; }
        public string DurationText { get; set; }
        public string ApplyUrl { get; set; }
    }

    public class FeeRowVM
    {
        public string ServiceTypeName { get; set; }
        public string ApplicationTypeName { get; set; }
        public int Price { get; set; }
        public string PriceDisplay { get; set; }
        public int DurationInDays { get; set; }
        public string DurationDisplay { get; set; }
        public bool IsGroupHeader { get; set; }
        public int RowSpan { get; set; }
    }

    /// <summary>
    /// بيانات الصفحة الرئيسية — كروت الخدمات + جدول الرسوم
    /// </summary>
    public class HomePageVM
    {
        public List<ServiceCardVM> ServiceCards { get; set; } = new();
        public List<FeeRowVM> FeeRows { get; set; } = new();
    }
}
