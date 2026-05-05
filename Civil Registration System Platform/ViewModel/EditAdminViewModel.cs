namespace Civil_Registration_System_Platform.ViewModel
{
    public class EditAdminViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EGPhoneNumber { get; set; } = string.Empty;
        public string NationalID { get; set; } = string.Empty;
        public int? OfficeId { get; set; }
        public int? ManageOfficeId { get; set; }
    }
}
