using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.ViewModel.Application
{
    public class ApplyViewModel
    {
        [Required]
        public ServiceType ServiceType { get; set; }
        public ApplicationType? ApplicationType { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [StringLength(14, MinimumLength = 14)]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "رقم الموبايل مطلوب")]
        public string Phone { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        public int GovernorateId { get; set; }

        public string? Address { get; set; }

        public List<IFormFile>? Documents { get; set; }

        [Required(ErrorMessage = "اختر المكتب")]
        public int OfficeId { get; set; }

        public string? Notes { get; set; }

        public IEnumerable<Governorate>? Governorates { get; set; }
        public IEnumerable<Office>? Offices { get; set; }
    }
}