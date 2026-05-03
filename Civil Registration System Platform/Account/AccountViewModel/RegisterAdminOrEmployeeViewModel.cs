namespace Civil_Registration_System_Platform.Account.AccountViewModel
{
    public class RegisterAdminOrEmployeeViewModel
    {
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; }

        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^01[0-2,5][0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number")]
        public string EGPhoneNumber { get; set; }

        [StringLength(14, MinimumLength = 14)]
        public string NationalID { get; set; }
        public int Gender { get; set; } // enum
        public int MaritalStatus { get; set; } // enum
        public IFormFile CardImage { get; set; }

        [StringLength(4, MinimumLength = 4)]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PassWord { get; set; }
        [Compare("PassWord", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public int GovernorateId { get; set; }
        public int OfficeId { get; set; }

        public int ManagerOfficeId { get; set; }
    }
}
