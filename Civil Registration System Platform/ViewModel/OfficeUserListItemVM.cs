namespace Civil_Registration_System_Platform.ViewModel
{
    public class OfficeUserListItemVM
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EGPhoneNumber { get; set; }
        public string NationalID { get; set; }
        public string CardImagePath { get; set; }
        public int Gender { get; set; }
        public int MaritalStatus { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string? GovernorateName { get; set; }
        public string? OfficeName { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsRejected { get; set; }
        public string? RejectionMessage { get; set; }
    }
}
