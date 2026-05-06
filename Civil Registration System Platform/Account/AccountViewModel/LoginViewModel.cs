namespace Civil_Registration_System_Platform.Account.AccountViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "اسم المستخدم أو البريد أو الرقم القومي مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string PassWord { get; set; }
    }
}
