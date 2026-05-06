using Microsoft.AspNetCore.Mvc.Rendering;

namespace Civil_Registration_System_Platform.Account.AccountViewModel
{
    public class RegisterViewModel
    {
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; }

        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^01[0-2,5][0-9]{8}$", ErrorMessage = "رقم الموبايل المصري غير صحيح")]
        public string EGPhoneNumber { get; set; }

        [StringLength(14, MinimumLength = 14)]
        public string NationalID { get; set; }
        public int Gender { get; set; } // enum
        public int MaritalStatus { get; set; } // enum
        public IFormFile CardImage { get; set; }

        [MinLength(4, ErrorMessage = "اسم المستخدم يجب ألا يقل عن 4 أحرف")]
        [StringLength(50, ErrorMessage = "اسم المستخدم يجب ألا يزيد عن 50 حرفاً")]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PassWord { get; set; }
        [Compare("PassWord", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } 

        public int GovernorateId { get; set; }
        public int OfficeId { get; set; }
    }
}
