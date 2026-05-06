using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.ViewModel
{
    public class PricingFormVM
    {
        [Required(ErrorMessage = "اختر الخدمة")]
        public ServiceType ServiceType { get; set; }

        [Required(ErrorMessage = "اختر نوع الطلب")]
        public ApplicationType ApplicationType { get; set; }

        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(0, 1000000, ErrorMessage = "السعر لازم يكون رقم موجب")]
        public int Price { get; set; }

        [Required(ErrorMessage = "المدة مطلوبة")]
        [Range(0, 365, ErrorMessage = "المدة لازم تكون بين 0 و 365 يوم")]
        public int DurationInDays { get; set; }

        [MaxLength(2000)]
        public string? Details { get; set; }
    }
}
