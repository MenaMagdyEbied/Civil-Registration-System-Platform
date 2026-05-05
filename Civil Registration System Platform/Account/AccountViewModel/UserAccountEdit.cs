namespace Civil_Registration_System_Platform.Account.AccountViewModel
{
    public class UserAccountEdit
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EGPhoneNumber { get; set; }
        public string NationalID { get; set; }
        public int Gender { get; set; } // enum
        public int MaritalStatus { get; set; } // enum
        public IFormFile CardImage { get; set; }
        public int? GovernorateId { get; set; }
        public int? OfficeId { get; set; }
    }
}
