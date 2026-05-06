using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.ViewModel
{
    // داشبورد الأدمن — مكتب واحد
    public class AdminDashboardVM
    {
        public int TodayApplications { get; set; }
        public int PendingReview { get; set; }
        public int IssuedToday { get; set; }
        public int RejectedToday { get; set; }
        public List<ApplicationSummaryAdminVM> Applications { get; set; } = new();
        public ApplicationStatus? FilterStatus { get; set; }
        public ServiceType? FilterService { get; set; }
    }
}
