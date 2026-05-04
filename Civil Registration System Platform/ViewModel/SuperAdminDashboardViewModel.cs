namespace Civil_Registration_System_Platform.ViewModel
{
    public class SuperAdminDashboardViewModel
    {

        public List<UserAccount> Admins { get; set; } = new();
        public int TotalAdmins { get; set; }
    }
}
