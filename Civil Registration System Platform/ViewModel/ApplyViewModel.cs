using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.ViewModel
{
    public class ApplyViewModel
    {
        public ServiceType ServiceType { get; set; }
        public ApplicationType? ApplicationType { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public int GovernorateId { get; set; }
        public string? Address { get; set; }
        public List<IFormFile>? Documents { get; set; }
        public int OfficeId { get; set; }
        public string? Notes { get; set; }
    }
}