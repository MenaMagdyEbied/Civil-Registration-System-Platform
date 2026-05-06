namespace Civil_Registration_System_Platform.ViewModel
{
    // داشبورد المواطن
    public class DashboardVM
    {
        public string UserName { get; set; }
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int ApprovedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public List<ApplicationSummaryClientVM> RecentApplications { get; set; } = new();

        public List<AppointmentSummaryVM> UpcomingAppointments { get; set; } = new();
    }
}
