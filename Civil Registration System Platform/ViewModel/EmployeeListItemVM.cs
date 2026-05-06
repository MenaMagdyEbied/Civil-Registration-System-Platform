namespace Civil_Registration_System_Platform.ViewModel
{
    // الأدمن يشوف موظفي مكتبه فقط
    public class EmployeeListItemVM
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EGPhoneNumber { get; set; }
        public string NationalID { get; set; }
        public string CardImagePath { get; set; }
        public DateOnly CreatedAt { get; set; }

        public int? OfficeId { get; set; }
        public string? OfficeName { get; set; }
        public string? GovernorateName { get; set; }
        public bool IsActive { get; set; }
    }
}
