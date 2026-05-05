namespace Civil_Registration_System_Platform.ViewModel
{
    public class ResetAdminPasswordViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة السر الجديدة مطلوبة")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة السر مطلوب")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمتا السر غير متطابقتين")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
