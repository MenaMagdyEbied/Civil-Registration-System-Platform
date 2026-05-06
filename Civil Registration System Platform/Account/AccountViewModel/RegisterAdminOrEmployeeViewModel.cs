namespace Civil_Registration_System_Platform.Account.AccountViewModel
{
    public class RegisterAdminOrEmployeeViewModel
    {
        [Required(ErrorMessage = "الاسم بالكامل مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 و 100 حرف")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "رقم الموبايل مطلوب")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "رقم الموبايل يجب أن يكون 11 رقم")]
        [RegularExpression(@"^01[0-2,5][0-9]{8}$", ErrorMessage = "رقم موبايل مصري غير صحيح (01xxxxxxxx)")]
        public string EGPhoneNumber { get; set; }

        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومي يجب أن يكون 14 رقم")]
        [RegularExpression(@"^[0-9]{14}$", ErrorMessage = "الرقم القومي يجب أن يحتوي على أرقام فقط")]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "اختر النوع")]
        public int Gender { get; set; } // enum

        [Required(ErrorMessage = "اختر الحالة الاجتماعية")]
        public int MaritalStatus { get; set; } // enum

        /// <summary>
        /// صورة البطاقة — اختيارية للأدمن/الموظف (مطلوبة فقط للمواطن)
        /// </summary>
        public IFormFile? CardImage { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "اسم المستخدم يجب أن يكون بين 4 و 20 حرف")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور لا تقل عن 6 أحرف")]
        public string PassWord { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("PassWord", ErrorMessage = "كلمات المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "اختر المحافظة")]
        public int GovernorateId { get; set; }
        public int? OfficeId { get; set; }

        public int? ManagerOfficeId { get; set; }
    }
}
